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
            double apFromFuriousHowl = getFuriousHowlEffect();
            double apFromHunterRAP = Math.Floor(calculatedStats.apSelfBuffed * apFromHunterScaling);

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

            double apTotal = Math.Round(apFromStrength + apBonus);

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

            petStats.PhysicalHit = hitTotal > 1 ? 1 : (float)hitTotal; // PetHit

            //Full Resists (Spell Hit)
            double levelResistFactor = 0.04;
            if (levelDifference == 1) levelResistFactor = 0.05;
            if (levelDifference == 2) levelResistFactor = 0.06;
            if (levelDifference == 3) levelResistFactor = 0.08;

            double fullResist = levelResistFactor - (hitFromTargetDebuffs + hitFromFocusedAim + hitFromRacial + hitFromHunter);
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

            // Expertise
            double expertiseRatingGain = Math.Round(hitFromHunter * 3.25 * 100);
            double expertiseDodgeReduced = expertiseRatingGain / 4 / 100;

            // Dodge adjust can't help us, only hinder
            double dodgeFactor = (0.05 + levelDifference * 0.005) - expertiseDodgeReduced; // PetTargetDodge
            double damageAdjustDodge = 1 - dodgeFactor;
            if (damageAdjustDodge > 1) damageAdjustDodge = 1;

            double damageAdjustHitCritMelee = (2 * critTotalMelee) + (hitTotal - critTotalMelee - dodgeFactor);
            double damageAdjustHitCritSpecials = (2 * critTotalSpecials) + (hitTotal - critTotalSpecials - dodgeFactor);

            double damageAdjustUnleashedFury = 1 + (character.HunterTalents.UnleashedFury * 0.03);
            double damageAdjustMood = options.petHappiness == PetHappiness.Happy ? 1.25 : options.petHappiness == PetHappiness.Content ? 1 : 0.75;
            double damageAdjustSpikedCollar = 1 + (options.petSpikedCollar * 0.03);
            double damageAdjustRaceModifier = character.Race == CharacterRace.Orc ? 1.05 : 1;
            double damageAdjustGearModifier = isWearingBeastTamersShoulders ? 1.03 : 1;
            double damageAdjustFerociousInspiration = 1; // TODO - getFerociousInspirationEffect();
            double damageAdjustBeastialWrath = 1; // TODO - getBestialWrathEffect();
            double damageAdjustKindredSpirits = 1 + (character.HunterTalents.KindredSpirits * 0.04);
            double damageAdjustSavageRend = 1; // TODO - getSavageRendEffect();
            double damageAdjustFeedingFrenzy = 1; // TODO - getFeedingFrenzyEffect();
            double damageAdjustSancRetributionAura = 1; // TODO
            double damageAdjustTier7Bonus = 1.05; // TODO - getT7Bonus();
            double damageAdjustSharkAttack = 1 + (options.petSharkAttack * 0.03);
            double damageAdjustTargetDebuffs = 1; // TODO
            double damageAdjustPetFamily = 1.05;
            double damageAdjustMarkedForDeath = 1 + (character.HunterTalents.MarkedForDeath * 0.02);
            double damageAdjustCobraReflexes = 1 - (options.petCobraReflexes * 0.075); // this is a negative effect!
            double damageAdjustGlancingBlows = 0.9125; // TODO - getGlancingBlows()

            // Monstrous Bite
            double damageAdjustMonstrousBite = 1;

            if (priorityRotation.getSkillFrequency(PetAttacks.MonstrousBite) > 0)
            {
                double monstrousBiteCooldown = 10;
                double monstrousBiteDuration = 12;

                double monstrousBiteChanceToApply = hitTotal - dodgeFactor;
                double monstrousBiteChancesToMaintain = 1;

                double monstrousBiteChanceToApplyFirst = monstrousBiteChancesToMaintain == 0 ? 0 : monstrousBiteChanceToApply;
                double monstrousBiteChanceToStop = 1 - monstrousBiteChanceToApplyFirst;

                double monstrousBiteAverageIncTime = 1 - monstrousBiteChanceToStop == 0 ? 0 : monstrousBiteCooldown;
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
                stacks[3].time_spent = monstrousBiteChanceToStop == 0 ? 1 : stacks[3].time_to_reach == 0 ? 0 : 1 / monstrousBiteChanceToStop * monstrousBiteCooldown * (1 - (stacks[0].chance_to_max + stacks[1].chance_to_max + stacks[2].chance_to_max));

                double monstrousBiteTotalTime = stacks[0].time_spent + stacks[1].time_spent + stacks[2].time_spent + stacks[3].time_spent;

                stacks[0].percent_time = monstrousBiteTotalTime == 0 ? 1 : stacks[0].time_spent / monstrousBiteTotalTime;
                stacks[1].percent_time = monstrousBiteTotalTime == 0 ? 0 : stacks[1].time_spent / monstrousBiteTotalTime;
                stacks[2].percent_time = monstrousBiteTotalTime == 0 ? 0 : stacks[2].time_spent / monstrousBiteTotalTime;
                stacks[3].percent_time = monstrousBiteTotalTime == 0 ? 0 : stacks[3].time_spent / monstrousBiteTotalTime;

                double monstrousBiteUptime = stacks[0].percent_time + stacks[1].percent_time + stacks[2].percent_time + stacks[3].percent_time;

                stacks[0].total = 0;
                stacks[1].total = 0.03 * stacks[1].percent_time;
                stacks[2].total = 0.06 * stacks[1].percent_time;
                stacks[3].total = stacks[3].chance_to_max == 1 ? 0.09 : 0.09 * stacks[1].percent_time;

                double monstrousBiteProcEffect = stacks[0].total + stacks[1].total + stacks[2].total + stacks[3].total;

                damageAdjustMonstrousBite = 1 + monstrousBiteProcEffect;
            }

            double damageAdjustMangle = 1; // TODO : 1 + (mangle_effect * 0.3)

            double effectiveTargetArmor = options.TargetArmor; // TODO: apply debuffs
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

            double damageAdjustDots = damageAdjustBase;
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
                    // TODO: add nature_debuffs (rly? for every spell school?)
                    S.damage = spellDamageAverage * critSpecialsAdjust * fullResistDamageAdjust* partialResistDamageAdjust * damageAdjustMagic;                       
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


                #endregion

                S.CalculateDPS();
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

        private float getSerenityEffect()
        {
            if (family == PetFamily.Moth)
            {
                float cd = 60 * (1 - character.HunterTalents.Longevity * 0.1f);
                return 0.1f * (15 / cd);
            }
            else
                return 0.0f;
        }

        private int getFuriousHowlEffect()
        {
            if (family == PetFamily.Wolf)
            {
                return 158;
            }
            else
                return 0;
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

        private List<PetSkill> skills = new List<PetSkill>();
        
        private double getSpecialDPS(double bonus, double critmiss, double damageFromAP)
        {
            if (skills.Count == 0) return 0;

            double dps = 0;
            int i = 0;

            foreach (PetSkill S in skills)
            {
                switch (skills[i].Type)
                {
                    case 0:
                        //dps += getFocusDumpDPS(freqs[i], bonus, critmiss, damageFromAP);
                        break;
                    case 1:
                        dps += getPhysicalSpecialDPS(freqs[i], bonus, critmiss, damageFromAP, S.Min, S.Max);
                        break;
                    case 2:
                        dps += getSpellSpecialDPS(freqs[i], bonus, S.Min, S.Max);
                        break;
                    case 3:
                        break; //Non-damaging, Calculations still done because we need the info for the next skill in the rotation
                    case 4:
                        dps += getSavageRendDPS(freqs[i], bonus, damageFromAP);
                        break;
                    case 5:
                        dps += getFireBreathDPS(freqs[i], bonus);
                        break;
                    case 6:
                        dps += getPoisonSpitDps(freqs[i], bonus);
                        break;
                    case 7: 
                        dps += getSpiritSmackDPS(freqs[i], bonus);
                        break;
                    case 8:
                        dps += getScorpidPoionDPS(freqs[i], bonus);
                        break;
                }
                i++;
            }

            return dps;
        }

        private double getScorpidPoionDPS(double freq, double bonus)
        {
            double avgDmg = 110 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            double dmg = avgDmg * bonus * critResist;
            return (dmg / 10) * freq;

        }

        private double getSpiritSmackDPS(double freq, double bonus)
        {
            double avgDmg = 120 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            double dmg = avgDmg * bonus * critResist;
            double dotDmg = 57 + calculatedStats.BasicStats.AttackPower * 0.0429;
            dotDmg *= bonus * critResist;
            return (dmg + dotDmg) / freq;
        }

        private double getPoisonSpitDps(double Freq, double bonus)
        {
            double avgDmg = 120 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            double dmg = avgDmg * bonus * critResist;
            return (dmg / 10) / Freq;
        }

        private double getFireBreathDPS(double Freq, double bonus)
        {
            double dotWithAP = 50 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double initialHit = 50 + calculatedStats.BasicStats.AttackPower * 0.0429;
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            dotWithAP *= bonus * critResist;
            return (initialHit + dotWithAP) / Freq;
        }

        private double getSavageRendDPS(double freq, double bonus, double damageFromAP)
        {
            double dmgPer = ((59 + 83) / 2) + damageFromAP;
            dmgPer *= bonus;
            double armorMit = 1 - (options.TargetArmor / (options.TargetArmor - 22167.5 + (467.5 * options.TargetLevel)));
            dmgPer *= armorMit;
            double bleedDmg = 24 + damageFromAP;
            bleedDmg *= bonus;
            bleedDmg *= armorMit;
            double totalDmg = dmgPer + (bleedDmg * 3);
            return totalDmg / freq;

        }

        private double getSpellSpecialDPS(double freq, double bonus,int min, int max)
        {
            double avgPer = ((max - min) / 2) + min;
            double avgDamage = avgPer + (petStats.AttackPower * 0.0429);
            double critResist = (petStats.PhysicalCrit * 1.5 + 1) * 0.935;
            double totalDamage = avgDamage * critResist * bonus;
            return totalDamage / freq;
        }

        private double getPhysicalSpecialDPS(double freq, double bonus, double critmiss, double damageFromAP, int min, int max)
        {
            double avgPer = ((max - min) / 2) + max;
            double avgDamage = avgPer + damageFromAP;
            double armorMit = options.TargetArmor / (options.TargetArmor - 22167.5 + (467.5 * options.TargetLevel));
            double totalDamage = avgDamage * bonus * (1 - armorMit) * critmiss;
            return totalDamage / freq;
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

        private double getMonstrousBiteEffect()
        {
            if (family == PetFamily.Devilsaur)
            {
                double tagetdodge = (0.05 + (options.TargetLevel - 80) * 0.005) - character.HunterTalents.AnimalHandler * 0.0125;
                double chancetoapply = petStats.PhysicalHit - tagetdodge;
                double cd = 10 * (1 - character.HunterTalents.Longevity * 0.1f);
                double chancestoapply = Math.Floor(11 / cd);
                double falloffchance = 1 - chancetoapply;
                double avgTimToInc = 10f;
                double timeToMax = avgTimToInc + (12 * falloffchance);
                double[] timeToReach = new double[4];
                double[] chanceToMax = new double[4];
                double[] timeSpent = new double[4];

                for (int i = 0; i < 4; i++)
                    timeToReach[i] = (double)10f * i;

                chanceToMax[0] = 0.0f;
                chanceToMax[1] = falloffchance;
                chanceToMax[2] = (1 - chanceToMax[1]) * falloffchance;
                chanceToMax[3] = (1 - chanceToMax[2]) * falloffchance;

                timeSpent[0] = timeToReach[1];
                timeSpent[1] = timeToMax * (1 - chanceToMax[0]);
                timeSpent[2] = timeToMax * (1 - chanceToMax[0] + chanceToMax[1]);
                timeSpent[3] = timeToMax * (1 - chanceToMax[0] + chanceToMax[1] + chanceToMax[2]);

                double totaltime = 0.0f;

                for (int i = 0; i < 4; i++)
                {
                    totaltime += timeSpent[i];
                }

                double total = 0.0f;

                for (int i = 0; i < 4; i++)
                {
                    total += (0.03 * i) * (timeSpent[i] / totaltime);
                }

                return total;
            }
            else
                return 0.0f;
        }

        private double getSavageRendEffect()
        {
            if (family == PetFamily.Raptor)
            {
                double timeBetweenProcs = 60 / petStats.PhysicalCrit;
                double uptime = 30 / timeBetweenProcs;
                return uptime * 0.1f;
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

        public double getFocus()
        {
            double focus = (24.0 + 12.0 * character.HunterTalents.BestialDiscipline) / 4.0;
            double shotsPerSecond = 1.0 / calculatedStats.BaseAttackSpeed + 1.0 / 1.5;
            focus += shotsPerSecond * calculatedStats.BasicStats.PhysicalCrit * character.HunterTalents.GoForTheThroat * 25.0;
            
            return focus;
        }
    }
}
