using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rawr.Hunter
{
    public class PetCalculations
    {
    
        Character character;
        CharacterCalculationsHunter calculatedStats;
        CalculationOptionsHunter options;
        CalculationsHunter hunterCalc;
        Stats statsBuffs;
        Stats statsGear;
        PetFamily family;
        public Stats petStats;

        private PetSkillPriorityRotation priorityRotation;

        double specialAttackSpeed = 1;
        double whiteAttackSpeed = 1;

        public double ferociousInspirationUptime;
        private List<double> freqs = new List<double>();

        // things we save earlier for later DPS calcs
        private double killCommandCooldown;

        public PetCalculations(Character character, CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter options, Stats statsBuffs, Stats statsGear, CalculationsHunter hunterCalc)
        {
            this.character = character;
            this.calculatedStats = calculatedStats;
            this.options = options;
            this.statsBuffs = statsBuffs;
            this.statsGear = statsGear;
            this.family = options.PetFamily;
            this.hunterCalc = hunterCalc;

            petStats = new Stats();

        }

        public void calculateTimings()
        {
            //All calculations in this function and the functions it calls are based off 
            //Shandara's DPS Spreadsheet and will be updated from that sheet and should 
            //continue to be updated from that sheet until it is no longer actively 
            //maintained.
            //Shandara DPS Spreadhseet Info:
            //Version: 0.87c
            //Release date: April 28, 2009
            //Forum: http://elitistjerks.com/f74/t30710-wotlk_dps_spreadsheet/


            // base stats for a level 80 pet
            petStats.Agility = 113;
            petStats.Strength = 331;
            petStats.Stamina = 361;
            petStats.Intellect = 65;
            petStats.Spirit = 10;

            int levelDifference = options.TargetLevel - 80;

            #region Focus Regen

            double focusRegenBase = 5;
            double focusRegenBestialDiscipline = focusRegenBase * 0.5 * character.HunterTalents.BestialDiscipline;            

            double critHitsPerSecond = calculatedStats.shotsPerSecondCritting * calculatedStats.priorityRotation.critsCompositeSum;
            double goForTheThroatPerCrit = 25 * character.HunterTalents.GoForTheThroat;
            double focusRegenGoForTheThroat = critHitsPerSecond * goForTheThroatPerCrit;

            double focusRegenPerSecond = focusRegenBase + focusRegenBestialDiscipline + focusRegenGoForTheThroat;

            // owl's focus
            double owlsFocusEffect = (options.petOwlsFocus > 0) ? owlsFocusEffect = 1 / (1 / (options.petOwlsFocus * 0.15) + 1) : 0;

            #endregion
            #region Special Abilities Priority Rotation

            priorityRotation = new PetSkillPriorityRotation(character, options);

            priorityRotation.AddSkill(options.PetPriority1);
            priorityRotation.AddSkill(options.PetPriority2);
            priorityRotation.AddSkill(options.PetPriority3);
            priorityRotation.AddSkill(options.PetPriority4);

            priorityRotation.owlsFocus = owlsFocusEffect;
            priorityRotation.fpsGained = focusRegenPerSecond;

            priorityRotation.calculateTimings();
            #endregion
            #region Kill Command MPS

            calculatedStats.petKillCommandMPS = 0;
            killCommandCooldown = 0;

            if (options.useKillCommand)
            {
                double killCommandManaCost = 0.03 * calculatedStats.baseMana;

                double killCommandReadinessFactor = calculatedStats.priorityRotation.containsShot(Shots.Readiness) ? 1 / 180 : 0;
                double killCommandCooldownBase = 1.0 / (60 - character.HunterTalents.CatlikeReflexes * 10);

                killCommandCooldown = 1.0 / (killCommandCooldownBase + killCommandReadinessFactor);

                calculatedStats.petKillCommandMPS = killCommandCooldown > 0 ? killCommandManaCost / killCommandCooldown : 0;
            }

            #endregion
            #region Hit Chance

            double hitFromBase = 0.95;
            double hitFromTargetDebuffs = 0;
            double hitFromFocusedAim = character.HunterTalents.FocusedAim * 0.01;
            double hitFromRacial = character.Race == CharacterRace.Draenei ? 0.01 : 0; // TODO: or draenei buff
            double hitFromHunter = Math.Floor(calculatedStats.BasicStats.HitRating / HunterRatings.HIT_RATING_PER_PERCENT) / 100;
            double hitFromLevelAdjust = 0 - levelDifference / 100.0;

            double hitTotal = hitFromBase
                            + hitFromTargetDebuffs
                            + hitFromFocusedAim
                            + hitFromRacial
                            + hitFromHunter
                            + hitFromLevelAdjust;

            double hitSpellTotal = hitFromTargetDebuffs
                            + hitFromFocusedAim
                            + hitFromRacial
                            + hitFromHunter;

            calculatedStats.petHit = hitTotal > 1 ? 1 : hitTotal; // PetHit
            calculatedStats.petSpellHit = hitSpellTotal > 1 ? 1 : hitSpellTotal;

            petStats.PhysicalHit = hitTotal > 1 ? 1 : (float)hitTotal; 

            #endregion
            #region Target Debuffs

            // Expertise
            double expertiseRatingGain = Math.Round(hitFromHunter * 3.25 * 100);
            double expertiseDodgeReduced = expertiseRatingGain / 4 / 100;

            calculatedStats.petTargetDodge = (0.05 + levelDifference * 0.005) - expertiseDodgeReduced; // PetTargetDodge

            double armorDebuffSporeCloud = 0;
            double sporeCloudFrequency = priorityRotation.getSkillFrequency(PetAttacks.SporeCloud);
            if (sporeCloudFrequency > 0)
            {
                double sporeCloudCalcFreq = options.emulateSpreadsheetBugs ? 10 : sporeCloudFrequency;
                double sporeCloudDuration = 9;
                double sporeCloudUptime = sporeCloudDuration > sporeCloudCalcFreq ? 1 : sporeCloudDuration / sporeCloudCalcFreq;

                armorDebuffSporeCloud = sporeCloudUptime * 0.03;
            }

            double armorDebuffAcidSpit = 0;
            double acidSpitFrequency = priorityRotation.getSkillFrequency(PetAttacks.AcidSpit); // AcidSpitEffectiveRate
            if (acidSpitFrequency > 0)
            {
                double acidSpitCalcFreq = options.emulateSpreadsheetBugs ? priorityRotation.getSkillCooldown(PetAttacks.AcidSpit) : acidSpitFrequency;
                double acidSpitDuration = 30;

                double acidSpitChanceToApply = calculatedStats.petHit - calculatedStats.petTargetDodge; // V45
                double acidSpitChancesToMaintain = Math.Floor((acidSpitDuration - 1) / acidSpitFrequency); // V46
                double acidSpitChanceToApplyFirst = acidSpitChancesToMaintain == 0 ? 0 : acidSpitChanceToApply; // V47
                double acidSpitChanceToStop = 1 - acidSpitChanceToApplyFirst; // AcidSpitChanceToStop
                double acidSpitAverageTimeToInc = acidSpitChanceToApplyFirst > 0 ? acidSpitFrequency : 0; // AcidSpitTimeToInc
                double acidSpitTimeSpentAtMax = acidSpitAverageTimeToInc + (acidSpitDuration * acidSpitChanceToStop); // AcidSpitAverageStackTime

                PetSkillStack[] stacks = new PetSkillStack[3];
                stacks[0] = new PetSkillStack();
                stacks[1] = new PetSkillStack();
                stacks[2] = new PetSkillStack();

                stacks[0].time_to_reach = 0;
                stacks[1].time_to_reach = acidSpitAverageTimeToInc * 1;
                stacks[2].time_to_reach = acidSpitAverageTimeToInc * 2;

                stacks[0].chance_to_max = 0;
                stacks[1].chance_to_max = acidSpitChanceToStop == 1 ? 0 : acidSpitChanceToStop;
                stacks[2].chance_to_max = acidSpitChanceToStop == 1 ? 0 : 1 - (stacks[0].chance_to_max + stacks[1].chance_to_max);

                stacks[0].time_spent = stacks[1].time_to_reach;
                stacks[1].time_spent = stacks[1].time_to_reach == 0 ? 0 : acidSpitTimeSpentAtMax * (1 - stacks[0].chance_to_max);
                stacks[2].time_spent = stacks[1].time_to_reach == 0 ? 0 : acidSpitChanceToStop == 0 ? 1 : 1 / acidSpitChanceToStop * acidSpitCalcFreq * (1-(stacks[0].chance_to_max + stacks[1].chance_to_max));

                double acidSpitTotalTime = stacks[0].time_spent + stacks[1].time_spent + stacks[2].time_spent;

                stacks[0].percent_time = acidSpitTotalTime == 0 ? 1 : stacks[0].time_spent / acidSpitTotalTime;
                stacks[1].percent_time = acidSpitTotalTime == 0 ? 1 : stacks[1].time_spent / acidSpitTotalTime;
                stacks[2].percent_time = acidSpitTotalTime == 0 ? 1 : stacks[2].time_spent / acidSpitTotalTime;

                double acidSpitUptime = stacks[1].percent_time + stacks[2].percent_time;
                if (options.emulateSpreadsheetBugs) acidSpitUptime += stacks[0].percent_time;

                stacks[0].total = stacks[0].percent_time * 0;
                stacks[1].total = stacks[0].percent_time * 0.1;
                stacks[2].total = stacks[0].percent_time * 0.2;

                armorDebuffAcidSpit = stacks[0].total + stacks[1].total + stacks[2].total;
            }

            double armorDebuffSting = 0;
            double stingFrequency = priorityRotation.getSkillFrequency(PetAttacks.Sting);
            if (stingFrequency > 0)
            {
                double stingCalcFreq = options.emulateSpreadsheetBugs ? 6 : stingFrequency;
                double stingDuration = 20;
                double stingUptime = stingDuration > stingCalcFreq ? 1 : stingDuration / stingCalcFreq;

                armorDebuffSting = stingUptime * 0.05;
            }

            calculatedStats.petArmorDebuffs = 0 - (1 - armorDebuffSporeCloud) * (1 - armorDebuffAcidSpit) * (1 - armorDebuffSting) + 1;

            #endregion
            #region Hunter Effects

            // Furious Howl
            // We need to calculate this now since it applies to the Hunter's AP too
            calculatedStats.apFromFuriousHowl = 0;
            double furiousHowlFrequency = priorityRotation.getSkillFrequency(PetAttacks.FuriousHowl);
            if (furiousHowlFrequency > 0)
            {
                double furiousHowlUptime = 20 / furiousHowlFrequency;
                calculatedStats.apFromFuriousHowl = 320 * furiousHowlUptime;
            }

            #endregion
        }

        public void calculateDPS()
        {
            int levelDifference = options.TargetLevel - 80;

            // setup
            #region Attack Power

            petStats.Strength += statsBuffs.Strength;
            petStats.Strength *= 1.0f + statsBuffs.BonusStrengthMultiplier;

            double apFromHunterScaling = 0.22 * (1 + options.petWildHunt * 0.15);
            double apFromStrength = (petStats.Strength - 10) * 2;
            double apFromHunterVsWild = Math.Floor(calculatedStats.BasicStats.Stamina * (0.1 * character.HunterTalents.HunterVsWild));
            double apFromTier9 = 0; //TODO
            double apFromBuffs = 0; //TODO: Pet +AP buffs
            double apFromHunterRAP = Math.Floor(calculatedStats.apSelfBuffed * apFromHunterScaling);

            // Furious Howl was calculated earlier
            double apFromFuriousHowl = calculatedStats.apFromFuriousHowl;

            // Call of the Wild
            double apAdjustFromCallOfTheWild = 0;
            if (options.petCallOfTheWild > 0)
            {
                double callOfTheWildCooldown = 300 * (1 - character.HunterTalents.Longevity * 0.1);
                double callOfTheWildDuration = 20;
                double callOfTheWildUptime = hunterCalc.CalcUptime(callOfTheWildDuration, callOfTheWildCooldown, options);
                apAdjustFromCallOfTheWild = 0.1 * callOfTheWildUptime;
            }

            // Serenity Dust
            double apAdjustFromSerenityDust = 0;
            if (priorityRotation.getSkillFrequency(PetAttacks.SerenityDust) > 0)
            {
                apAdjustFromSerenityDust = 0.025; // 0.1 * (15 / 60);
            }

            double apAdjustFromRabidProc = 0; //TODO getRabidProcEffect(petStats.PhysicalHit);
            double apAdjustFromTrueShotAura = calculatedStats.apFromTrueshotAura;
            double apAdjustFromAnimalHandler = 0; //TODO
            double apAdjustFromAspectOfTheBeast = 0; //TODO

            double apScalingFactor = 1
                                 * (1 + apAdjustFromCallOfTheWild)
                                 * (1 + apAdjustFromRabidProc)
                                 * (1 + apAdjustFromSerenityDust)
                                 * (1 + apAdjustFromTrueShotAura)
                                 * (1 + apAdjustFromAnimalHandler)
                                 * (1 + apAdjustFromAspectOfTheBeast);

            apFromStrength *= apScalingFactor;

            double apBonus = (apFromHunterVsWild
                                 + apFromTier9
                                 + apFromBuffs
                                 + apFromFuriousHowl
                                 + apFromHunterRAP) * apScalingFactor;

            double apTotal = Math.Round(apFromStrength + apBonus, 1);

            double damageBonusMeleeFromAP = 0.07 * apTotal; // PetAPBonus
            double damageBonusSpellsFromAP = (1.5 / 35) * apTotal; // PetSpellScaling * PetAP

            petStats.AttackPower = (float)apTotal;


            /*
                        // TODO: T8 donus is the RAP proc - does it actually apply to pets?
                        //petStats.AttackPower += getT8Bonus();

                        if (options.selectedAspect == Aspect.Beast)
                            petStats.AttackPower *= 1.1f;
                        else if (options.selectedAspect == Aspect.Hawk)
                        {
                            petStats.AttackPower += (300 + (character.HunterTalents.AspectMastery * 90));
                        }
            */
            #endregion
            #region Spell Hit

            //Full Resists (Spell Hit)
            double levelResistFactor = 0.04;
            if (levelDifference == 1) levelResistFactor = 0.05;
            if (levelDifference == 2) levelResistFactor = 0.06;
            if (levelDifference == 3) levelResistFactor = 0.08;

            double fullResist = levelResistFactor - calculatedStats.petSpellHit;
            if (fullResist < 0) fullResist = 0;
            double fullResistDamageAdjust = 1 - fullResist;

            //Partial Resists (Spell Hit)
            double averageResist = (options.TargetLevel - 80) * 0.02;
            double resist10 = 5 * averageResist;
            double resist20 = 2.5 * averageResist;
            double partialResistDamageAdjust = 1 - (resist10 * 0.1 + resist20 * 0.1);

            #endregion
            #region Crit Chance

            bool isWearingBeastTamersShoulders = character.Shoulders != null && character.Shoulders.Id == 30892;

            double critAgilityBase = petStats.Agility;
            double critAgilityBuffsAdditive = 0; // TODO
            double critAgilityBuffsMultiplicitive = 0; // TODO statsBuffs.BonusAgilityMultiplier
            double critAgilityTotal = (critAgilityBase + critAgilityBuffsAdditive) * (1 + critAgilityBuffsMultiplicitive);

            double critBuffsAdditive = 0; // TODO - calculatedStats.BasicStats.BonusPetCritChance

            double critFromBase = 0.032;
            double critFromAgility = critAgilityTotal / (100 * 62.5);
            double critFromSpidersBite = options.petSpidersBite * 0.03;
            double critFromFerocity = character.HunterTalents.Ferocity * 0.02;
            double critFromGear = isWearingBeastTamersShoulders ? 0.02 : 0;
            double critFromBuffs = critBuffsAdditive / (HunterRatings.CRIT_RATING_PER_PERCENT * 100); ;
            double critFromTargetDebuffs = 0; // TODO

            double critFromDepression = (levelDifference > 2) ? 0 - (0.03 + (levelDifference * 0.006)) : 0 - ((levelDifference * 5 * 0.04) / 100);

            double critTotalMelee = critFromBase
                             + critFromAgility
                             + critFromSpidersBite
                             + critFromFerocity
                             + critFromGear
                             + critFromBuffs
                             + critFromTargetDebuffs
                             + critFromDepression;

            //petStats.PhysicalCrit = (float)critTotal;

            double cobraStrikesPetBonus = 0; // TODO
            double critTotalSpecials = critTotalMelee + cobraStrikesPetBonus; // PetCritChance
            double critSpecialsAdjust = critTotalSpecials * 1.5 + 1;

            #endregion
            #region Attack Speed

            double attackSpeedFromSerpentsSwiftness = 1 + (character.HunterTalents.SerpentsSwiftness * 0.04);
            double attackSpeedFromHeroism = 1; // TODO
            double attackSpeedFromCobraReflexes = 1 + (options.petCobraReflexes * 0.15);
            double attackSpeedFromMultiplicitiveHasteBuffs = 1; // TODO
            double attackSpeedAdjust = attackSpeedFromSerpentsSwiftness
                                     * attackSpeedFromHeroism
                                     * attackSpeedFromCobraReflexes
                                     * attackSpeedFromMultiplicitiveHasteBuffs;

            double attackSpeedFrenzyUptime = 0; // TODO: getFrenzyEffect()
            double attackSpeedFrenzyBoost = 1 + (0.3 * attackSpeedFrenzyUptime);

            double attackSpeedBase = 2.0;
            double attackSpeedAdjusted = attackSpeedBase / attackSpeedAdjust;
            double attackSpeedEffective = attackSpeedAdjusted / attackSpeedFrenzyBoost;

            #endregion
            #region Damage Adjustments

            // Dodge adjust can't help us, only hinder
            double damageAdjustDodge = 1 - calculatedStats.petTargetDodge;
            if (damageAdjustDodge > 1) damageAdjustDodge = 1;

            double damageAdjustHitCritMelee = (2 * critTotalMelee) + (calculatedStats.petHit - critTotalMelee - calculatedStats.petTargetDodge);
            double damageAdjustHitCritSpecials = (2 * critTotalSpecials) + (calculatedStats.petHit - critTotalSpecials - calculatedStats.petTargetDodge); // CritAdjustments

            double damageAdjustUnleashedFury = 1 + (character.HunterTalents.UnleashedFury * 0.03);
            double damageAdjustMood = options.petHappiness == PetHappiness.Happy ? 1.25 : options.petHappiness == PetHappiness.Content ? 1 : 0.75;
            double damageAdjustSpikedCollar = 1 + (options.petSpikedCollar * 0.03);
            double damageAdjustRaceModifier = character.Race == CharacterRace.Orc ? 1.05 : 1;
            double damageAdjustGearModifier = isWearingBeastTamersShoulders ? 1.03 : 1;
            double damageAdjustFerociousInspiration = 1; // TODO - getFerociousInspirationEffect();
            double damageAdjustBeastialWrath = 1; // TODO - getBestialWrathEffect();
            double damageAdjustKindredSpirits = 1 + (character.HunterTalents.KindredSpirits * 0.04);
            double damageAdjustFeedingFrenzy = 1; // TODO - getFeedingFrenzyEffect();
            double damageAdjustSancRetributionAura = 1; // TODO
            double damageAdjustTier7Bonus = 1.05; // TODO - getT7Bonus();
            double damageAdjustSharkAttack = 1 + (options.petSharkAttack * 0.03);
            double damageAdjustTargetDebuffs = 1; // TODO
            double damageAdjustPetFamily = 1.05;
            double damageAdjustMarkedForDeath = 1 + (character.HunterTalents.MarkedForDeath * 0.02);
            double damageAdjustCobraReflexes = 1 - (options.petCobraReflexes * 0.075); // this is a negative effect!
            double damageAdjustGlancingBlows = 0.9125; // TODO - getGlancingBlows()

            // Savage Rend
            double damageAdjustSavageRend = 1;
            double savageRendFrequency = priorityRotation.getSkillFrequency(PetAttacks.SavageRend);
            if (savageRendFrequency > 0)
            {
                double savageRendTimeBetweenProcs = savageRendFrequency * (1 / critTotalSpecials);
                double savageRendUptime = savageRendTimeBetweenProcs > 0 ? 30 / savageRendTimeBetweenProcs : 0;
                damageAdjustSavageRend = 1 + 0.1 * savageRendUptime;
            }

            // Monstrous Bite
            double damageAdjustMonstrousBite = 1;
            double monstrousBiteFrequency = priorityRotation.getSkillFrequency(PetAttacks.MonstrousBite);
            if (monstrousBiteFrequency > 0)
            {
                double monstrousBiteUseFreq = options.emulateSpreadsheetBugs ? priorityRotation.getSkillCooldown(PetAttacks.MonstrousBite) : monstrousBiteFrequency;
                double monstrousBiteDuration = 12;

                double monstrousBiteChanceToApply = calculatedStats.petHit - calculatedStats.petTargetDodge;
                double monstrousBiteChancesToMaintain = 1;

                double monstrousBiteChanceToApplyFirst = monstrousBiteChancesToMaintain == 0 ? 0 : monstrousBiteChanceToApply;
                double monstrousBiteChanceToStop = 1 - monstrousBiteChanceToApplyFirst;

                double monstrousBiteAverageIncTime = 1 - monstrousBiteChanceToStop == 0 ? 0 : monstrousBiteUseFreq;
                double monstrousBiteAverageStackTime = monstrousBiteAverageIncTime + monstrousBiteDuration * monstrousBiteChanceToStop;

                PetSkillStack[] stacks = new PetSkillStack[4];
                stacks[0] = new PetSkillStack();
                stacks[1] = new PetSkillStack();
                stacks[2] = new PetSkillStack();
                stacks[3] = new PetSkillStack();

                stacks[0].time_to_reach = 0;
                stacks[1].time_to_reach = monstrousBiteAverageIncTime * 1;
                stacks[2].time_to_reach = monstrousBiteAverageIncTime * 2;
                stacks[3].time_to_reach = monstrousBiteAverageIncTime * 3;

                stacks[0].chance_to_max = 0;
                stacks[1].chance_to_max = monstrousBiteChanceToStop == 1 ? 0 : monstrousBiteChanceToStop;
                stacks[2].chance_to_max = monstrousBiteChanceToStop == 1 ? 0 : monstrousBiteChanceToStop * (1 - stacks[1].chance_to_max);
                stacks[3].chance_to_max = monstrousBiteChanceToStop == 1 ? 0 : 1 - (stacks[0].chance_to_max + stacks[1].chance_to_max + stacks[2].chance_to_max);

                stacks[0].time_spent = stacks[3].chance_to_max == 1 ? 0 : stacks[1].time_to_reach;
                stacks[1].time_spent = stacks[3].chance_to_max == 1 ? 0 : (stacks[1].time_to_reach == 0 ? 0 : monstrousBiteAverageStackTime) * (1 - (stacks[0].chance_to_max));
                stacks[2].time_spent = stacks[3].chance_to_max == 1 ? 0 : (stacks[2].time_to_reach == 0 ? 0 : monstrousBiteAverageStackTime) * (1 - (stacks[0].chance_to_max + stacks[1].chance_to_max));
                stacks[3].time_spent = monstrousBiteChanceToStop == 0 ? 1 : stacks[3].time_to_reach == 0 ? 0 : 1 / monstrousBiteChanceToStop * monstrousBiteUseFreq * (1 - (stacks[0].chance_to_max + stacks[1].chance_to_max + stacks[2].chance_to_max));

                double monstrousBiteTotalTime = stacks[0].time_spent + stacks[1].time_spent + stacks[2].time_spent + stacks[3].time_spent;

                stacks[0].percent_time = monstrousBiteTotalTime == 0 ? 1 : stacks[0].time_spent / monstrousBiteTotalTime;
                stacks[1].percent_time = monstrousBiteTotalTime == 0 ? 0 : stacks[1].time_spent / monstrousBiteTotalTime;
                stacks[2].percent_time = monstrousBiteTotalTime == 0 ? 0 : stacks[2].time_spent / monstrousBiteTotalTime;
                stacks[3].percent_time = monstrousBiteTotalTime == 0 ? 0 : stacks[3].time_spent / monstrousBiteTotalTime;

                double monstrousBiteUptime = stacks[1].percent_time + stacks[2].percent_time + stacks[3].percent_time;
                if (options.emulateSpreadsheetBugs) monstrousBiteUptime += stacks[0].percent_time;

                stacks[0].total = 0;
                stacks[1].total = 0.03 * stacks[1].percent_time;
                stacks[2].total = 0.06 * stacks[1].percent_time;
                stacks[3].total = stacks[3].chance_to_max == 1 ? 0.09 : 0.09 * stacks[1].percent_time;

                double monstrousBiteProcEffect = stacks[0].total + stacks[1].total + stacks[2].total + stacks[3].total;

                damageAdjustMonstrousBite = 1 + monstrousBiteProcEffect;
            }

            double damageAdjustMangle = 1; // TODO : 1 + (mangle_effect * 0.3)

            double effectiveTargetArmor = options.TargetArmor * calculatedStats.targetDebuffsArmor;
            double damageAdjustMitigation = 1 - (effectiveTargetArmor / (effectiveTargetArmor - 22167.5 + (467.5 * 80)));

            double damageAdjustBase = 1
                    * damageAdjustUnleashedFury
                    * damageAdjustMood
                    * damageAdjustSpikedCollar
                    * damageAdjustRaceModifier
                    * damageAdjustGearModifier
                    * damageAdjustFerociousInspiration
                    * damageAdjustBeastialWrath
                    * damageAdjustKindredSpirits
                    * damageAdjustMonstrousBite
                    * damageAdjustSavageRend
                    * damageAdjustFeedingFrenzy
                    * damageAdjustSancRetributionAura
                    * damageAdjustTier7Bonus
                    * damageAdjustSharkAttack
                    * damageAdjustTargetDebuffs
                    * damageAdjustPetFamily;

            double damageAdjustDots = damageAdjustBase; // BasePetModifier
            double damageAdjustWhite = damageAdjustBase * damageAdjustHitCritMelee;
            double damageAdjustMelee = damageAdjustBase * damageAdjustHitCritSpecials; // MeleeAttackAdjustment
            double damageAdjustSpecials = damageAdjustBase * damageAdjustHitCritSpecials * damageAdjustMarkedForDeath; // DamageAdjustment
            double damageAdjustMagic = damageAdjustBase * damageAdjustMarkedForDeath / damageAdjustTargetDebuffs; // MagicDamageAdjustments

            #endregion

            // damage
            #region White Damage

            double whiteDamageBase = (52 + 78) / 2;
            double whiteDamageFromAP = Math.Floor(petStats.AttackPower / 14 * 2);
            double whiteDamageNormal = whiteDamageBase + whiteDamageFromAP;
            double whiteDamageAdjust = damageAdjustWhite * damageAdjustCobraReflexes * damageAdjustMitigation * damageAdjustGlancingBlows * damageAdjustDodge;
            double whiteDamageReal = whiteDamageNormal * whiteDamageAdjust;

            double whiteDPS = whiteDamageReal / attackSpeedEffective;

            calculatedStats.petWhiteDPS = whiteDPS;

            #endregion
            #region Priority Rotation

            // loop over each skill, figuriung out the damage value

            foreach (PetSkillInstance S in priorityRotation.skills)
            {
                S.damage = 0;

                #region Skill Groups

                if (S.skillData.type == PetSkillType.FocusDump)
                {
                    double focusDumpDamageAverage = ((118 + 168) / 2) + damageBonusMeleeFromAP;
                    S.damage = focusDumpDamageAverage * damageAdjustSpecials * damageAdjustMitigation;
                }

                if (S.skillData.type == PetSkillType.SpecialMelee)
                {
                    double meleeDamageAverage = S.skillData.average + damageBonusMeleeFromAP;
                    S.damage = meleeDamageAverage * damageAdjustSpecials * damageAdjustMitigation;
                }

                if (S.skillData.type == PetSkillType.SpecialSpell)
                {
                    double spellDamageAverage = S.skillData.average + damageBonusSpellsFromAP;
                    S.damage = spellDamageAverage * critSpecialsAdjust * fullResistDamageAdjust * partialResistDamageAdjust 
                                * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                }

                #endregion
                #region Unique Skills

                if (S.skillType == PetAttacks.Rake)
                {
                    double rakeDamageFromAP = apTotal * 0.0175;
                    double rakeAverageDamage = ((47  + 67) / 2) + rakeDamageFromAP;
                    double rakeAverageDamageDot = ((19 + 25) / 2) + rakeDamageFromAP;

                    double rakeInitialHitDamage = rakeAverageDamage * damageAdjustSpecials * damageAdjustMangle; // spreadsheet doesn't add armor mitigation, appears to be wow bug?
                    double rakeDotDamage = rakeAverageDamageDot * damageAdjustMangle * damageAdjustDots;

                    S.damage = rakeInitialHitDamage + rakeDotDamage * 3;
                }

                if (S.skillType == PetAttacks.FireBreath)
                {
                    double fireBreathDamageAverage = ((43 + 57) / 2) + damageBonusSpellsFromAP;
                    double fireBreathDamageInitial = fireBreathDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust * damageAdjustMagic;
                    double fireBreathDamageDot = 50 + damageBonusSpellsFromAP;
                    S.damage = fireBreathDamageInitial + fireBreathDamageDot;
                }

                if (S.skillType == PetAttacks.SavageRend)
                {
                    double savageRendAverageHitDamage = ((59 + 83) / 2) + damageBonusMeleeFromAP;
                    double savageRendAverageBleedDamage = ((21 + 27) / 2) + damageBonusMeleeFromAP;
                    
                    double savageRendHitDamage = savageRendAverageHitDamage * damageAdjustSpecials * damageAdjustMangle * damageAdjustMitigation;
                    double savageRendBleedDamage = savageRendAverageBleedDamage * damageAdjustDots * damageAdjustMangle;

                    S.damage = savageRendHitDamage + 3 * savageRendBleedDamage;
                }

                if (S.skillType == PetAttacks.ScorpidPoison)
                {
                    double scorpidPoisionDamageFromAP = options.emulateSpreadsheetBugs ? 0.04 * apTotal : damageBonusSpellsFromAP;
                    double scorpidPoisionDamageAverage = ((100 + 130) / 2) + scorpidPoisionDamageFromAP;
                    double scorpidPoisionDamageNormal = scorpidPoisionDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust
                                                        * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                    double scorpidPoisionDamageTick = scorpidPoisionDamageNormal / 5;
                    double scorpidPoisionTicks = Math.Floor(S.cooldown / 2);
                    S.damage = scorpidPoisionDamageTick * scorpidPoisionTicks;
                }

                if (S.skillType == PetAttacks.PoisonSpit)
                {
                    double poisonSpitDamageAverage = ((104 + 136) / 2) + damageBonusSpellsFromAP;
                    double poisonSpitDamageNormal = poisonSpitDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust 
                                                    * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                    double poisonSpitDamageTick = poisonSpitDamageNormal / 3;
                    double poisonSpitTicks = S.cooldown >= 9 ? 3 : 2;
                    S.damage = poisonSpitDamageTick * poisonSpitTicks;
                    if (options.emulateSpreadsheetBugs)
                    {
                        S.damage = poisonSpitDamageNormal;
                    }
                }

                if (S.skillType == PetAttacks.VenomWebSpray)
                {
                    double venomWebSprayDamageAverage = ((46 + 68) / 2) + damageBonusSpellsFromAP;
                    double venomWebSprayDamageNormal = venomWebSprayDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust
                                                        * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                    S.damage = venomWebSprayDamageNormal * 4;
                }

                if (S.skillType == PetAttacks.SpiritStrike)
                {
                    double spiritStrikeAdjust = fullResistDamageAdjust * partialResistDamageAdjust * damageAdjustMagic
                                                     * calculatedStats.targetDebuffsNature;
                    double spiritStrikeDamageFromAP = options.emulateSpreadsheetBugs ? 0.04 * apTotal : damageBonusSpellsFromAP;

                    double spiritStrikeInitialDamageAverage = ((49 + 65) / 2) + spiritStrikeDamageFromAP;
                    double spiritStrikeInitialDamageNormal = spiritStrikeInitialDamageAverage * spiritStrikeAdjust * damageAdjustHitCritSpecials;

                    double spiritStrikeDotDamageAverage = ((49 + 65) / 2) + spiritStrikeDamageFromAP;
                    double spiritStrikeDotDamageNormal = spiritStrikeDotDamageAverage * spiritStrikeAdjust;

                    S.damage = spiritStrikeInitialDamageNormal + spiritStrikeDotDamageNormal;
                }

                if (S.skillType == PetAttacks.SporeCloud)
                {
                    double spellDamageAverage = S.skillData.average + damageBonusSpellsFromAP;
                    S.damage = spellDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust
                                    * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                }

                if (S.skillType == PetAttacks.AcidSpit)
                {
                    double acidSpitAverage = S.skillData.average + damageBonusSpellsFromAP;
                    S.damage = acidSpitAverage * (critTotalSpecials * 1.5 + 1) * fullResistDamageAdjust * partialResistDamageAdjust * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                }

                #endregion

                if (options.emulateSpreadsheetBugs && S.skillType == PetAttacks.NetherShock) S.damage = 0;
                if (options.emulateSpreadsheetBugs && S.skillType == PetAttacks.VenomWebSpray) S.damage = 0;

                S.CalculateDPS();

                if (options.emulateSpreadsheetBugs && S.skillType == PetAttacks.ScorpidPoison)
                {
                    S.dps = S.cooldown > 0 ? S.damage / S.cooldown : 0;
                    S.kc_dps = S.can_crit ? S.dps : 0;
                }

            }

            // now add everything up...

            priorityRotation.calculateDPS();

            #endregion
            #region Kill Command

            double killCommandDPS = 0;

            if (killCommandCooldown > 0)
            {
                double killCommandDamagePerSpecial = priorityRotation.petSpecialFrequency > 0 ? priorityRotation.kc_dps / (1 / priorityRotation.petSpecialFrequency) : 0;

                double killCommandBonusDamage = killCommandDamagePerSpecial * 1.2;

                double killCommandDPSOverCooldown = killCommandCooldown > 0 ? killCommandBonusDamage / killCommandCooldown : 0;

                double killCommandFocusedFireCritBonus = character.HunterTalents.FocusedFire * 0.1;
                double killCommandAdjustedBonus = killCommandFocusedFireCritBonus * damageAdjustDodge * damageAdjustMitigation;

                killCommandDPS = killCommandDPSOverCooldown * (1 + killCommandAdjustedBonus);
            }

            calculatedStats.petKillCommandDPS = killCommandDPS;

            #endregion
         
            //cruft:
            //ferociousInspirationUptime = 1.0 - Math.Pow(1.0 - petStats.PhysicalCrit, 10.0 / whiteAttackSpeed + 10.0 / specialAttackSpeed);

            calculatedStats.petSpecialDPS = priorityRotation.dps;

            calculatedStats.PetDpsPoints = (float)(calculatedStats.petWhiteDPS 
                                                + calculatedStats.petSpecialDPS 
                                                + calculatedStats.petKillCommandDPS);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////

        // everything below here is not used and is gradually being removed as it gets re-implemented

        ///////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////


        private float getRabidProcEffect(double physhit)
        {
            if (options.petRabid > 0)
            {
                float frequency = 45.0f * (1.0f - character.HunterTalents.Longevity * 0.1f);
                float uptime = 20.0f / frequency;
                float targetdodge = (0.05f + (options.TargetLevel - 80) * 0.005f) - character.HunterTalents.AnimalHandler * 0.0125f;
                float chancetoapply = (float)(0.5f * (physhit - targetdodge));
                float avgattackstofull = 5 / chancetoapply;
                float timetofull = (float)(avgattackstofull * specialAttackSpeed);

                float result = 0;
                if (timetofull > 20)
                {
                    result = (float)(specialAttackSpeed * chancetoapply * 0.5f);
                }
                else
                {
                    result = (timetofull / 20) * 0.125f;
                    result += ((20 - timetofull) / 20) * 0.25f;
                }
                return result;
            }
            else
                return 0.0f;
        }

        private float getCobraEffect()
        {
            if (character.HunterTalents.CobraStrikes > 0)
            {
                float procchance = character.HunterTalents.CobraStrikes * 0.2f;
                float twospecialtime = (float)specialAttackSpeed * 2f;
                float exp = 0.0f;
                if (calculatedStats.priorityRotation.containsShot(Shots.ArcaneShot))
                {
                    exp += 1.0f / 6.0f;
                }
                if (calculatedStats.priorityRotation.containsShot(Shots.SteadyShot))
                {
                    exp += 1.0f / 2.5f;
                }
                float critchance = 0.3566f + statsBuffs.BonusSteadyShotCrit + (character.HunterTalents.SurvivalInstincts * 0.02f);
                float basecrit = 1 - (1 - critchance * procchance);
                return (float)Math.Pow(basecrit , critchance) * twospecialtime;
            }
            else
                return 0.0f;
        }

        private double getFerociousInspirationEffect()
        {
            ferociousInspirationUptime = 1.0 - Math.Pow(1.0 - petStats.PhysicalCrit, 10.0 / whiteAttackSpeed + 10.0 / specialAttackSpeed);
            double benefit = 0.01f * character.HunterTalents.FerociousInspiration;
            return ferociousInspirationUptime * benefit;
        }

        private double getFrenzyEffect(double spd)
        {
            double chancetocause = petStats.CritRating * (1 / spd / ((1 / spd) + (1 / specialAttackSpeed)));
            chancetocause += petStats.CritRating * (1 / spd / ((1 / spd) + (1 / specialAttackSpeed)));
            chancetocause *= character.HunterTalents.Frenzy * 0.2;

            double effectivespd = spd / 1.3f;
            double numattacks = Math.Floor(8 / effectivespd) + (8 / specialAttackSpeed);
            double chancetonotbein = Math.Pow(1 - chancetocause, numattacks);
            return (1 - chancetonotbein) * 0.3f;
        }

        private double getBestialWrathEffect()
        {
            if (character.HunterTalents.BestialWrath >= 1)
            {
                double cd = 120;
                cd *= 1.0f - character.HunterTalents.Longevity * 0.1f;
                double uptime = 18 / cd;
                return uptime * 0.5f;
            }
            else
                return 0.0f;
            
        }

        private double getFeedingFrenzyEffect()
        {
            return (options.petFeedingFrenzy * 0.08) * 0.35f;
        }

        private double getGlancingBlows()
        {
            int diff = 400 - (options.TargetLevel * 5);
            if (diff < 0)
                diff = 0;
            double chance = 0.0f;
            if (diff >= 15)
                chance = 0.25;
            else
                chance = (diff * 0.01) + 0.1f;

            double lowEnd = 1.3 - 0.05 * diff;
            if (lowEnd > 0.91)
                lowEnd = 0.91;
            double hiEnd = 1.2 * 0.03 * diff;
            if (hiEnd > 0.99)
                hiEnd = 0.99;
            double avgDamage = lowEnd + hiEnd / 2;
            return 1 - (chance * (1 - avgDamage));
        }

        public double getT7Bonus()
        {
            int i = 0;

            if (character._head != null && character._chest != null && character._legs != null && character._shoulders != null && character._hands != null)
            {

                if (character._head.Contains("Cryptstalker Battlegear"))
                    i++;

                if (character._chest.Contains("Cryptstalker Battlegear"))
                    i++;

                if (character._legs.Contains("Cryptstalker Battlegear"))
                    i++;

                if (character._shoulders.Contains("Cryptstalker Battlegear"))
                    i++;

                if (character._hands.Contains("Cryptstalker Battlegear"))
                    i++;

                if (i >= 2)
                    return 0.05f;
                else
                    return 0.0f;
            }
            else
                return 0.0f;
        }

        public int getT8Bonus()
        {
            int i = 0;

            if (character._head != null && character._chest != null && character._legs != null && character._shoulders != null && character._hands != null)
            {

                if (character._head.Contains("Scourgestalker Battlegear"))
                    i++;

                if (character._chest.Contains("Scourgestalker Battlegear"))
                    i++;

                if (character._legs.Contains("Scourgestalker Battlegear"))
                    i++;

                if (character._shoulders.Contains("Scourgestalker Battlegear"))
                    i++;

                if (character._hands.Contains("Scourgestalker Battlegear"))
                    i++;

                //only procs if steady shot is being used
                if (calculatedStats.priorityRotation.containsShot(Shots.SteadyShot) && i >= 4)
                    //15s duration of 600AP 45s cooldown = ~33.33%uptime = 200AP average for hunter = 44AP average for pet
                    return 44;
                else
                    return 0;
            }
            else
                return 0;
        }

    }
}
