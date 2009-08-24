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
        Stats statsBuffs;
        Stats statsHunterBuffs;
        Stats statsGear;
        public Stats petStats;
        private PetSkillPriorityRotation priorityRotation;

        public double ferociousInspirationUptime;
        private List<double> freqs = new List<double>();

        // things we save earlier for later DPS calcs
        private double attackSpeedEffective;
        private double compositeSpeed;
        private double killCommandCooldown;
        private double critSpecialsAdjust;
        private bool isWearingBeastTamersShoulders;

        public PetCalculations(Character character, CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter options, Stats statsHunterBuffs, Stats statsBuffs, Stats statsGear)
        {
            this.character = character;
            this.calculatedStats = calculatedStats;
            this.options = options;
            this.statsHunterBuffs = statsHunterBuffs;
            this.statsBuffs = statsBuffs;
            this.statsGear = statsGear;

            petStats = new Stats();
        }

        public void calculateTimings()
        {
            // See the main hunter module file (CalculationsHunter.cs) for credits
            // and source information.

            // base stats for a level 80 pet
            petStats.Agility = 113;
            petStats.Strength = 331;
            petStats.Stamina = 361;
            petStats.Intellect = 65;
            petStats.Spirit = 10;

            statsBuffs.Stamina += statsBuffs.PetStamina;
            statsBuffs.Strength += statsBuffs.PetStrength;
            statsBuffs.Spirit += statsBuffs.PetSpirit;

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
            priorityRotation.AddSkill(options.PetPriority5);
            priorityRotation.AddSkill(options.PetPriority6);
            priorityRotation.AddSkill(options.PetPriority7);

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

                double killCommandReadinessFactor = calculatedStats.priorityRotation.containsShot(Shots.Readiness) ? 1.0 / 180 : 0;
                double killCommandCooldownBase = 1.0 / (60 - character.HunterTalents.CatlikeReflexes * 10);

                killCommandCooldown = 1.0 / (killCommandCooldownBase + killCommandReadinessFactor);

                calculatedStats.petKillCommandMPS = killCommandCooldown > 0 ? killCommandManaCost / killCommandCooldown : 0;
            }

            #endregion
            #region Hit Chance

            calculatedStats.petHitFromBase = 0.95;
            calculatedStats.petHitFromTargetDebuffs = 0;
            calculatedStats.petHitFromFocusedAim = character.HunterTalents.FocusedAim * 0.01;
            calculatedStats.petHitFromRacial = character.Race == CharacterRace.Draenei ? 0.01 : statsBuffs.SpellHit;
            calculatedStats.petHitFromHunter = Math.Floor(calculatedStats.BasicStats.HitRating / HunterRatings.HIT_RATING_PER_PERCENT) / 100;
            calculatedStats.petHitFromLevelAdjust = 0 - levelDifference / 100.0;

            calculatedStats.petHitTotal = calculatedStats.petHitFromBase
                            + calculatedStats.petHitFromTargetDebuffs
                            + calculatedStats.petHitFromFocusedAim
                            + calculatedStats.petHitFromRacial
                            + calculatedStats.petHitFromHunter
                            + calculatedStats.petHitFromLevelAdjust;

            calculatedStats.petHitSpellTotal = calculatedStats.petHitFromTargetDebuffs
                            + calculatedStats.petHitFromFocusedAim
                            + calculatedStats.petHitFromRacial
                            + calculatedStats.petHitFromHunter;

            if (calculatedStats.petHitTotal > 1) calculatedStats.petHitTotal = 1; // PetHit
            if (calculatedStats.petHitSpellTotal > 1) calculatedStats.petHitSpellTotal = 1;

            petStats.PhysicalHit = (float)calculatedStats.petHitTotal;

            #endregion
            #region Crit Chance

            isWearingBeastTamersShoulders = character.Shoulders != null && character.Shoulders.Id == 30892;

            double critAgilityBase = petStats.Agility;
            double critAgilityBuffsAdditive = statsBuffs.Agility;
            double critAgilityBuffsMultiplicitive = statsBuffs.BonusAgilityMultiplier;
            double critAgilityTotal = (critAgilityBase + critAgilityBuffsAdditive) * (1 + critAgilityBuffsMultiplicitive);

            calculatedStats.petCritFromBase = 0.032;
            calculatedStats.petCritFromAgility = critAgilityTotal / (100 * 62.5);
            calculatedStats.petCritFromSpidersBite = options.petSpidersBite * 0.03;
            calculatedStats.petCritFromFerocity = character.HunterTalents.Ferocity * 0.02;
            calculatedStats.petCritFromGear = isWearingBeastTamersShoulders ? 0.02 : 0;
            calculatedStats.petCritFromBuffs = statsBuffs.PhysicalCrit + (statsBuffs.CritRating / HunterRatings.CRIT_RATING_PER_PERCENT) / 100;
            calculatedStats.petCritFromTargetDebuffs = calculatedStats.targetDebuffsCrit;
            calculatedStats.petCritFromDepression = (levelDifference > 2) ? 0 - (0.03 + (levelDifference * 0.006)) : 0 - ((levelDifference * 5 * 0.04) / 100);

            // PetCritBaseChance
            calculatedStats.petCritTotalMelee = calculatedStats.petCritFromBase
                             + calculatedStats.petCritFromAgility
                             + calculatedStats.petCritFromSpidersBite
                             + calculatedStats.petCritFromFerocity
                             + calculatedStats.petCritFromGear
                             + calculatedStats.petCritFromBuffs
                             + calculatedStats.petCritFromTargetDebuffs
                             + calculatedStats.petCritFromDepression;

            // Cobra Strikes
            calculatedStats.petCritFromCobraStrikes = 0;
            double cobraStrikesProc = character.HunterTalents.CobraStrikes * 0.2;
            if (cobraStrikesProc > 0)
            {
                double cobraStrikesCritFreqSteady = calculatedStats.steadyShot.freq * (1 / calculatedStats.steadyShot.critChance);
                double cobraStrikesCritFreqArcane = calculatedStats.arcaneShot.freq * (1 / calculatedStats.arcaneShot.critChance);
                double cobraStrikesCritFreqKill = calculatedStats.killShot.freq * (1 / calculatedStats.killShot.critChance);

                double cobraStrikesCritFreqSteadyInv = cobraStrikesCritFreqSteady > 0 ? 1 / cobraStrikesCritFreqSteady : 0;
                double cobraStrikesCritFreqArcaneInv = cobraStrikesCritFreqArcane > 0 ? 1 / cobraStrikesCritFreqArcane : 0;
                double cobraStrikesCritFreqKillInv = cobraStrikesCritFreqKill > 0 ? 1 / cobraStrikesCritFreqKill : 0;
                double cobraStrikesCritFreqInv = cobraStrikesCritFreqSteadyInv + cobraStrikesCritFreqArcaneInv + cobraStrikesCritFreqKillInv;
                double cobraStrikesCritFreq = cobraStrikesCritFreqInv > 0 ? 1 / cobraStrikesCritFreqInv : 0;
                double cobraStrikesProcAdjust = cobraStrikesCritFreq / cobraStrikesProc;

                double cobraStrikesFreqSteadyInv = calculatedStats.steadyShot.freq > 0 ? 1 / calculatedStats.steadyShot.freq : 0;
                double cobraStrikesFreqArcaneInv = calculatedStats.arcaneShot.freq > 0 ? 1 / calculatedStats.arcaneShot.freq : 0;
                double cobraStrikesFreqKillInv = calculatedStats.killShot.freq > 0 ? 1 / calculatedStats.killShot.freq : 0;
                double cobraStrikesFreqInv = cobraStrikesFreqSteadyInv + cobraStrikesFreqArcaneInv + cobraStrikesFreqKillInv;

                double cobraStrikesTwoSpecials = 2 * priorityRotation.petSpecialFrequency;
                double cobraStrikesUptime = 1 - Math.Pow(1 - calculatedStats.steadyShot.critChance * cobraStrikesProc, cobraStrikesFreqInv * cobraStrikesTwoSpecials);

                calculatedStats.petCritFromCobraStrikes = (cobraStrikesUptime + (1 - cobraStrikesUptime) * calculatedStats.petCritTotalMelee) - calculatedStats.petCritTotalMelee;
            }

            calculatedStats.petCritTotalSpecials = calculatedStats.petCritTotalMelee + calculatedStats.petCritFromCobraStrikes; // PetCritChance
            critSpecialsAdjust = calculatedStats.petCritTotalSpecials * 1.5 + 1;

            #endregion
            #region Attack Speed

            double attackSpeedFromSerpentsSwiftness = 1 + (character.HunterTalents.SerpentsSwiftness * 0.04);
            double attackSpeedFromHeroism = 1 + calculatedStats.hasteFromHeroism;
            double attackSpeedFromCobraReflexes = 1 + (options.petCobraReflexes * 0.15);
            double attackSpeedFromMultiplicitiveHasteBuffs = 1 + statsBuffs.PhysicalHaste;
            double attackSpeedAdjust = attackSpeedFromSerpentsSwiftness
                                     * attackSpeedFromHeroism
                                     * attackSpeedFromCobraReflexes
                                     * attackSpeedFromMultiplicitiveHasteBuffs;

            double attackSpeedBase = options.PetFamily == PetFamily.None ? 0.0 : 2.0;
            double attackSpeedAdjusted = attackSpeedBase / attackSpeedAdjust;

            // Frenzy
            double attackSpeedFrenzyUptime = 0;
            if (character.HunterTalents.Frenzy > 0)
            {
                double frenzyNormalSpeed = attackSpeedAdjusted; // L5
                double frenzySpecialSpeed = priorityRotation.petSpecialFrequency; // L6
                double frenzyFrenziedSpeed = frenzyNormalSpeed / 1.3; // L7
                double frenzyNumberAttacks = Math.Floor(8 / frenzyFrenziedSpeed) + (frenzySpecialSpeed == 0 ? 0 : (8 / frenzySpecialSpeed)); // L8

                double frenzyNormalSpeedInvert = frenzyNormalSpeed > 0 ? 1 / frenzyNormalSpeed : 0; // 1/L5
                double frenzySpecialSpeedInvert = frenzySpecialSpeed > 0 ? 1 / frenzySpecialSpeed : 0; // 1/L6

                double frenzyChancePart1 = frenzySpecialSpeed > 0 ? calculatedStats.petCritTotalSpecials * (frenzySpecialSpeedInvert / (frenzyNormalSpeedInvert + frenzySpecialSpeedInvert)) : 0;
                double frenzyChancePart2 = frenzyNormalSpeed > 0 ? calculatedStats.petCritTotalMelee * (frenzyNormalSpeedInvert / (frenzyNormalSpeedInvert + frenzySpecialSpeedInvert)) : 0;

                double frenzyChanceToCause = (frenzyChancePart1 + frenzyChancePart2) * character.HunterTalents.Frenzy * 0.2; // L4

                double frenzyChanceToNot = Math.Pow(1 - frenzyChanceToCause, frenzyNumberAttacks);

                attackSpeedFrenzyUptime = 1 - frenzyChanceToNot;
            }

            double attackSpeedFrenzyBoost = 1 + (0.3 * attackSpeedFrenzyUptime);

            attackSpeedEffective = attackSpeedAdjusted / attackSpeedFrenzyBoost; // EffectivePetAttackSpeed            

            compositeSpeed = 1 / (1 / attackSpeedEffective + (priorityRotation.petSpecialFrequency > 0 ? 1 / priorityRotation.petSpecialFrequency : 0)); // PetCompSpeed

            #endregion
            #region Target Debuffs

            // Expertise
            double hitFromHunterWhole = (calculatedStats.BasicStats.HitRating / HunterRatings.HIT_RATING_PER_PERCENT) / 100;
            double expertiseRatingGain = Math.Round(hitFromHunterWhole * 3.25 * 100);
            double expertiseDodgeReduced = expertiseRatingGain / 4 / 100;

            calculatedStats.petTargetDodge = (0.05 + levelDifference * 0.005) - expertiseDodgeReduced; // PetTargetDodge
            if (calculatedStats.petTargetDodge < 0) calculatedStats.petTargetDodge = 0;

            double armorDebuffSporeCloud = 0;
            double sporeCloudFrequency = priorityRotation.getSkillFrequency(PetAttacks.SporeCloud);
            if (sporeCloudFrequency > 0)
            {
                double sporeCloudDuration = 9;
                double sporeCloudUptime = sporeCloudDuration > sporeCloudFrequency ? 1 : sporeCloudDuration / sporeCloudFrequency;

                armorDebuffSporeCloud = sporeCloudUptime * 0.03;
            }

            double armorDebuffAcidSpit = 0;
            double acidSpitFrequency = priorityRotation.getSkillFrequency(PetAttacks.AcidSpit); // AcidSpitEffectiveRate
            if (acidSpitFrequency > 0)
            {
                double acidSpitCalcFreq = priorityRotation.getSkillCooldown(PetAttacks.AcidSpit);
                double acidSpitDuration = 30;

                double acidSpitChanceToApply = calculatedStats.petHitTotal - calculatedStats.petTargetDodge; // V45
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

                stacks[0].total = stacks[0].percent_time * 0;
                stacks[1].total = stacks[0].percent_time * 0.1;
                stacks[2].total = stacks[0].percent_time * 0.2;

                armorDebuffAcidSpit = stacks[0].total + stacks[1].total + stacks[2].total;
            }

            double armorDebuffSting = 0;
            double stingFrequency = priorityRotation.getSkillFrequency(PetAttacks.Sting);
            if (stingFrequency > 0)
            {
                double stingUseFreq = priorityRotation.getSkillFrequency(PetAttacks.Sting);
                double stingDuration = 20;
                double stingUptime = stingDuration > stingUseFreq ? 1 : stingDuration / stingUseFreq;

                armorDebuffSting = stingUptime * 0.05;
            }

            // these local buffs can be overridden
            if (character.ActiveBuffsConflictingBuffContains("Sting"))
            {
                armorDebuffSporeCloud = 0;
                armorDebuffSting = 0;
            }
            if (character.ActiveBuffsConflictingBuffContains("Acid Spit"))
            {
                armorDebuffAcidSpit = 0;
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

            //Ferocious Inspiraion
            // (Same as above)
            calculatedStats.ferociousInspirationDamageAdjust = 1;
            if (character.HunterTalents.FerociousInspiration > 0)
            {
                if (options.PetFamily != PetFamily.None)
                {
                    double ferociousInspirationSpecialsEffect = priorityRotation.petSpecialFrequency == 0 ? 0 : 10 / priorityRotation.petSpecialFrequency;
                    double ferociousInspirationUptime = 1 - Math.Pow(1 - calculatedStats.petCritTotalSpecials, (10 / attackSpeedEffective) + ferociousInspirationSpecialsEffect);
                    double ferociousInspirationEffect = 0.01 * character.HunterTalents.FerociousInspiration;

                    calculatedStats.ferociousInspirationDamageAdjust = 1.0 + ferociousInspirationUptime * ferociousInspirationEffect;                    
                }
            }

            //Roar of Recovery
            calculatedStats.manaRegenRoarOfRecovery = 0;
            double roarOfRecoveryFreq = priorityRotation.getSkillFrequency(PetAttacks.RoarOfRecovery);
            if (roarOfRecoveryFreq > 0)
            {
                double roarOfRecoveryUseCount = Math.Ceiling(options.duration / roarOfRecoveryFreq);
                double roarOfRecoveryManaRestored = calculatedStats.BasicStats.Mana * 0.3 * roarOfRecoveryUseCount; // E129
                calculatedStats.manaRegenRoarOfRecovery = roarOfRecoveryUseCount > 0 ? roarOfRecoveryManaRestored / options.duration : 0;
            }

            //Invigoration
            //calculatedStats.manaRegenInvigoration = 0;
            double invigorationProcChance = character.HunterTalents.Invigoration * 0.5; // C32
            if (invigorationProcChance > 0)
            {
                double invigorationProcFreq = (priorityRotation.petSpecialFrequency / calculatedStats.petCritTotalSpecials) / invigorationProcChance; //C35
                double invigorationEffect = character.HunterTalents.Invigoration > 0 ? 0.01 : 0;
                double invigorationManaGainedPercent = invigorationProcFreq > 0 ? 60 / invigorationProcFreq * invigorationEffect : 0; // C36
                double invigorationManaPerMinute = invigorationProcFreq > 0 ? 60 / invigorationProcFreq * invigorationEffect * calculatedStats.BasicStats.Mana : 0; // C37
                calculatedStats.manaRegenInvigoration = invigorationManaPerMinute / 60;
            }

            // Call of the Wild
            calculatedStats.apFromCallOfTheWild = 0;
            if (options.petCallOfTheWild > 0)
            {
                double callOfTheWildUseFreq = priorityRotation.getSkillFrequency(PetAttacks.CallOfTheWild);
                double callOfTheWildUptime = CalculationsHunter.CalcUptime(20, callOfTheWildUseFreq, options);
                calculatedStats.apFromCallOfTheWild = 0.1 * callOfTheWildUptime;
            }

            #endregion
        }

        public void calculateDPS()
        {
            if (options.PetFamily == PetFamily.None) return;

            int levelDifference = options.TargetLevel - 80;

            // setup
            #region Attack Power

            petStats.Strength += statsBuffs.Strength;
            petStats.Strength *= 1.0f + statsBuffs.BonusStrengthMultiplier;

            double apFromHunterScaling = 0.22 * (1 + options.petWildHunt * 0.15);
            calculatedStats.petAPFromStrength = (petStats.Strength - 10) * 2;
            calculatedStats.petAPFromHunterVsWild = Math.Floor(calculatedStats.BasicStats.Stamina * (0.1 * character.HunterTalents.HunterVsWild));
            calculatedStats.petAPFromBuffs = statsBuffs.AttackPower;
            calculatedStats.petAPFromHunterRAP = Math.Floor(calculatedStats.apSelfBuffed * apFromHunterScaling);

            // Tier 9 4-pice bonus is complex
            calculatedStats.petAPFromTier9 = 0;
            if (character.ActiveBuffsContains("Windrunner's Pursuit 4 Piece Bonus"))
            {
                double tier9BonusTimePetShot = 0.9;
                double tier9BonusTimeBetween = tier9BonusTimePetShot > 0 ? 1 / 0.15 * tier9BonusTimePetShot + 45 : 0;
                calculatedStats.petAPFromTier9 = 600 * CalculationsHunter.CalcUptime(15, tier9BonusTimeBetween, options);
            }

            // Furious Howl was calculated earlier
            calculatedStats.petAPFromFuriousHowl = calculatedStats.apFromFuriousHowl;

            // Call of the Wild
            calculatedStats.petAPFromCallOfTheWild = calculatedStats.apFromCallOfTheWild;

            // Serenity Dust
            calculatedStats.petAPFromSerenityDust = 0;
            if (priorityRotation.getSkillFrequency(PetAttacks.SerenityDust) > 0)
            {
                calculatedStats.petAPFromSerenityDust = 0.025; // 0.1 * (15 / 60);
            }

            calculatedStats.petAPFromTrueShotAura = calculatedStats.apFromTrueshotAura;
            calculatedStats.petAPFromAnimalHandler = character.HunterTalents.AnimalHandler * 0.05;
            calculatedStats.petAPFromAspectOfTheBeast = calculatedStats.aspectBonusAPBeast;

            calculatedStats.petAPFromOutsideBuffs = 0;
            if (calculatedStats.petAPFromTrueShotAura == 0)
            {
                calculatedStats.petAPFromOutsideBuffs = 0.0 + statsBuffs.BonusAttackPowerMultiplier;
            }

            calculatedStats.petAPFromRabidProc = 0;

            double longevityCooldownAdjust = 1 - character.HunterTalents.Longevity * 0.1;
            double rabidCooldown = 45 * longevityCooldownAdjust;
            double rabidUptime = options.petRabid * CalculationsHunter.CalcUptime(20, rabidCooldown, options);

            if (rabidUptime > 0)
            {
                double rabidDuration = 20; // Q24
                // Q25 - rabidCooldown
                double rabidEffectiveRate = compositeSpeed; // Q26
                double rabidHit = calculatedStats.petHitTotal; // Q27
                double rabidDodge = calculatedStats.petTargetDodge; // Q28
                double rabidChanceToApply = 0.5 * (rabidHit - rabidDodge); // Q29
                double rabidChancesToMaintain = rabidUptime > 0 ? rabidCooldown / rabidEffectiveRate : 0; // Q30

                double rabidChanceApply1 = rabidChancesToMaintain == 0 ? 0 : rabidChanceToApply; // S22
                double rabidChanceApply2 = rabidChancesToMaintain >= 2 ? (1 - rabidChanceApply1) * rabidChanceToApply : 0; // S23
                double rabidChanceApply3 = rabidChancesToMaintain >= 3 ? (1 - (rabidChanceApply1 + rabidChanceApply2)) * rabidChanceToApply : 0; // S24
                double rabidChanceApply4 = rabidChancesToMaintain >= 4 ? (1 - (rabidChanceApply1 + rabidChanceApply2 + rabidChanceApply3)) * rabidChanceToApply : 0; // S25
                double rabidChanceApply5 = rabidChancesToMaintain >= 5 ? (1 - (rabidChanceApply1 + rabidChanceApply2 + rabidChanceApply3 + rabidChanceApply4)) * rabidChanceToApply : 0; // S26
                double rabidChanceApplySum = rabidChanceApply1 + rabidChanceApply2 + rabidChanceApply3 + rabidChanceApply4 + rabidChanceApply5; // SUM(S22:S26)
                double rabidChanceFallOff = 1-rabidChanceApplySum < 0 ? 0 : 1-rabidChanceApplySum; // S27 RabidChanceToStop

                double rabidAverageIncTime = 0; // S28 RabidAverageTimeToInc
                if (1 - rabidChanceFallOff != 0)
                {
                    rabidAverageIncTime = rabidChanceApply1 / rabidChanceApplySum * rabidEffectiveRate
                                        + rabidChanceApply2 / rabidChanceApplySum * rabidEffectiveRate * 2
                                        + rabidChanceApply3 / rabidChanceApplySum * rabidEffectiveRate * 3
                                        + rabidChanceApply4 / rabidChanceApplySum * rabidEffectiveRate * 4
                                        + rabidChanceApply5 / rabidChanceApplySum * rabidEffectiveRate * 5;
                }
                double rabidTimeSpentMax = rabidUptime > 0 ? rabidAverageIncTime + (rabidEffectiveRate * rabidChanceFallOff) : 0; // RabidAverageStackTime

                PetSkillStack[] stacks = new PetSkillStack[7];
                stacks[0] = new PetSkillStack();
                stacks[1] = new PetSkillStack();
                stacks[2] = new PetSkillStack();
                stacks[3] = new PetSkillStack();
                stacks[4] = new PetSkillStack();
                stacks[5] = new PetSkillStack();
                stacks[6] = new PetSkillStack();

                stacks[0].time_to_reach = 0;
                stacks[1].time_to_reach = rabidAverageIncTime * 1;
                stacks[2].time_to_reach = rabidAverageIncTime * 2;
                stacks[3].time_to_reach = rabidAverageIncTime * 3;
                stacks[4].time_to_reach = rabidAverageIncTime * 4;
                stacks[5].time_to_reach = rabidAverageIncTime * 5;
                stacks[6].time_to_reach = rabidDuration;

                stacks[0].chance_to_max = 0;
                stacks[1].chance_to_max = rabidChanceFallOff == 1 ? 0 : rabidChanceFallOff;
                stacks[2].chance_to_max = rabidChanceFallOff == 1 ? 0 : rabidChanceFallOff * (1 - stacks[1].chance_to_max);
                stacks[3].chance_to_max = rabidChanceFallOff == 1 ? 0 : rabidChanceFallOff * (1 - stacks[2].chance_to_max);
                stacks[4].chance_to_max = rabidChanceFallOff == 1 ? 0 : rabidChanceFallOff * (1 - stacks[3].chance_to_max);
                stacks[5].chance_to_max = rabidChanceFallOff == 1 ? 0 : 1 - (stacks[0].chance_to_max + stacks[1].chance_to_max + stacks[2].chance_to_max + stacks[3].chance_to_max + stacks[4].chance_to_max);

                stacks[0].time_spent = stacks[1].time_to_reach;
                stacks[1].time_spent = stacks[1].time_to_reach == 0 ? 0 : rabidTimeSpentMax * (1 - (stacks[0].chance_to_max));
                stacks[2].time_spent = stacks[2].time_to_reach == 0 ? 0 : rabidTimeSpentMax * (1 - (stacks[0].chance_to_max + stacks[1].chance_to_max));
                stacks[3].time_spent = stacks[3].time_to_reach == 0 ? 0 : rabidTimeSpentMax * (1 - (stacks[0].chance_to_max + stacks[1].chance_to_max + stacks[2].chance_to_max));
                stacks[4].time_spent = stacks[4].time_to_reach == 0 ? 0 : rabidTimeSpentMax * (1 - (stacks[0].chance_to_max + stacks[1].chance_to_max + stacks[2].chance_to_max + stacks[3].chance_to_max));
                stacks[5].time_spent = 0;
                if (rabidUptime > 1)
                {
                    stacks[5].time_spent = stacks[5].time_to_reach == 0 ? 0 : 1 / rabidChanceFallOff * rabidCooldown *(1 - (stacks[0].chance_to_max + stacks[1].chance_to_max + stacks[2].chance_to_max));
                }
                else if (rabidUptime > 0)
                {
                    stacks[5].time_spent = rabidDuration - (stacks[0].time_spent + stacks[1].time_spent + stacks[2].time_spent + stacks[3].time_spent + stacks[4].time_spent);
                }
                stacks[6].time_spent = rabidCooldown - rabidDuration;

                double rabidTotalTime = stacks[0].time_spent + stacks[1].time_spent + stacks[2].time_spent + stacks[3].time_spent
                                      + stacks[4].time_spent + stacks[5].time_spent + stacks[6].time_spent;

                stacks[0].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[0].time_spent / rabidTotalTime;
                stacks[1].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[1].time_spent / rabidTotalTime;
                stacks[2].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[2].time_spent / rabidTotalTime;
                stacks[3].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[3].time_spent / rabidTotalTime;
                stacks[4].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[4].time_spent / rabidTotalTime;
                stacks[5].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[5].time_spent / rabidTotalTime;
                stacks[6].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[6].time_spent / rabidTotalTime;

                stacks[0].total = stacks[0].percent_time * 0;
                stacks[1].total = stacks[1].percent_time * 0.05;
                stacks[2].total = stacks[2].percent_time * 0.1;
                stacks[3].total = stacks[3].percent_time * 0.15;
                stacks[4].total = stacks[4].percent_time * 0.2;
                stacks[5].total = stacks[5].percent_time * 0.25;
                stacks[6].total = stacks[6].percent_time * 0;

                calculatedStats.petAPFromRabidProc = rabidEffectiveRate > 0 ? stacks[0].total + stacks[1].total + stacks[2].total + stacks[3].total
                                                               + stacks[4].total + stacks[5].total + stacks[6].total : 0;
            }

            double apScalingFactor = 1
                                 * (1 + calculatedStats.petAPFromCallOfTheWild)
                                 * (1 + calculatedStats.petAPFromRabidProc)
                                 * (1 + calculatedStats.petAPFromSerenityDust)
                                 * (1 + calculatedStats.petAPFromTrueShotAura)
                                 * (1 + calculatedStats.petAPFromOutsideBuffs)
                                 * (1 + calculatedStats.petAPFromAnimalHandler)
                                 * (1 + calculatedStats.petAPFromAspectOfTheBeast);

            double apTotalUnadjusted = calculatedStats.petAPFromStrength
                                     + calculatedStats.petAPFromHunterVsWild
                                     + calculatedStats.petAPFromTier9
                                     + calculatedStats.petAPFromBuffs
                                     + calculatedStats.petAPFromFuriousHowl
                                     + calculatedStats.petAPFromHunterRAP;

            double apTotal = Math.Round(apTotalUnadjusted * apScalingFactor, 1);

            double damageBonusMeleeFromAP = 0.07 * apTotal; // PetAPBonus
            double damageBonusSpellsFromAP = (1.5 / 35) * apTotal; // PetSpellScaling * PetAP

            petStats.AttackPower = (float)apTotal;

            #endregion
            #region Spell Hit

            //Full Resists (Spell Hit)
            double levelResistFactor = 0.04;
            if (levelDifference == 1) levelResistFactor = 0.05;
            if (levelDifference == 2) levelResistFactor = 0.06;
            if (levelDifference == 3) levelResistFactor = 0.08;

            double fullResist = levelResistFactor - calculatedStats.petHitSpellTotal;
            if (fullResist < 0) fullResist = 0;
            double fullResistDamageAdjust = 1 - fullResist;

            //Partial Resists (Spell Hit)
            double averageResist = (options.TargetLevel - 80) * 0.02;
            double resist10 = 5 * averageResist;
            double resist20 = 2.5 * averageResist;
            double partialResistDamageAdjust = 1 - (resist10 * 0.1 + resist20 * 0.2);

            #endregion
            #region Damage Adjustments

            // Dodge adjust can't help us, only hinder
            double damageAdjustDodge = 1 - calculatedStats.petTargetDodge;
            if (damageAdjustDodge > 1) damageAdjustDodge = 1;

            double damageAdjustHitCritMelee = (2 * calculatedStats.petCritTotalMelee) + (calculatedStats.petHitTotal - calculatedStats.petCritTotalMelee - calculatedStats.petTargetDodge);
            double damageAdjustHitCritSpecials = (2 * calculatedStats.petCritTotalSpecials) + (calculatedStats.petHitTotal - calculatedStats.petCritTotalSpecials - calculatedStats.petTargetDodge); // CritAdjustments

            double damageAdjustUnleashedFury = 1 + (character.HunterTalents.UnleashedFury * 0.03);
            double damageAdjustMood = options.petHappiness == PetHappiness.Happy ? 1.25 : options.petHappiness == PetHappiness.Content ? 1 : 0.75;
            double damageAdjustSpikedCollar = 1 + (options.petSpikedCollar * 0.03);
            double damageAdjustRaceModifier = character.Race == CharacterRace.Orc ? 1.05 : 1;
            double damageAdjustGearModifier = isWearingBeastTamersShoulders ? 1.03 : 1;
            double damageAdjustFerociousInspiration = calculatedStats.ferociousInspirationDamageAdjust;
            double damageAdjustKindredSpirits = 1 + (character.HunterTalents.KindredSpirits * 0.04);
            double damageAdjustSancRetributionAura = 1 + statsBuffs.BonusDamageMultiplier;
            double damageAdjustTier7Bonus = 1 + statsHunterBuffs.BonusPetDamageMultiplier;
            double damageAdjustSharkAttack = 1 + (options.petSharkAttack * 0.03);
            double damageAdjustTargetDebuffs = calculatedStats.targetDebuffsPetDamage;
            double damageAdjustPetFamily = 1.05;
            double damageAdjustMarkedForDeath = 1 + (character.HunterTalents.MarkedForDeath * 0.02);
            double damageAdjustCobraReflexes = 1 - (options.petCobraReflexes * 0.075); // this is a negative effect!

            // Feeding Frenzy
            double damageAdjustFeedingFrenzy = 1;
            if (options.petFeedingFrenzy > 0)
            {
                double feedingFrenzyTimeSpent = options.timeSpentSub20 + options.timeSpent35To20;
                double feedingFrenzyUptime = feedingFrenzyTimeSpent > 0 ? feedingFrenzyTimeSpent / options.duration : 0;
                damageAdjustFeedingFrenzy = 1 + feedingFrenzyUptime * options.petFeedingFrenzy * 0.08;
            }

            // Glancing Blows
            double glancingBlowsSkillDiff = (options.TargetLevel * 5) - (options.petLevel * 5); // F55
            if (glancingBlowsSkillDiff < 0) glancingBlowsSkillDiff = 0;
            double glancingBlowsChance = glancingBlowsSkillDiff > 15 ? 0.25 : 0.1 + glancingBlowsSkillDiff * 0.01; // F56
            double glancingBlowsLowEnd = Math.Min(1.3 - 0.05 * glancingBlowsSkillDiff, 0.91); // F57
            double glancingBlowsHighEnd = Math.Min(1.2 - 0.03 * glancingBlowsSkillDiff, 0.99); // F58
            double damageAdjustGlancingBlows = 1 - (glancingBlowsChance * (1 - ((glancingBlowsLowEnd + glancingBlowsHighEnd) / 2)));

            // Bestial Wrath
            double damageAdjustBeastialWrath = 1;
            if (calculatedStats.beastialWrath.freq > 0)
            {
                double beastialWrathUptime = character.HunterTalents.BestialWrath > 0 ? 18 / calculatedStats.beastialWrath.freq : 0;
                damageAdjustBeastialWrath = 1 + beastialWrathUptime * 0.5;
            }

            // Savage Rend
            double damageAdjustSavageRend = 1;
            double savageRendFrequency = priorityRotation.getSkillFrequency(PetAttacks.SavageRend);
            if (savageRendFrequency > 0)
            {
                double savageRendTimeBetweenProcs = savageRendFrequency * (1 / calculatedStats.petCritTotalSpecials);
                double savageRendUptime = savageRendTimeBetweenProcs > 0 ? 30 / savageRendTimeBetweenProcs : 0;
                damageAdjustSavageRend = 1 + 0.1 * savageRendUptime;
            }

            // Monstrous Bite
            double damageAdjustMonstrousBite = 1;
            double monstrousBiteFrequency = priorityRotation.getSkillFrequency(PetAttacks.MonstrousBite);
            if (monstrousBiteFrequency > 0)
            {
                double monstrousBiteUseFreq = monstrousBiteFrequency;
                double monstrousBiteDuration = 12;

                double monstrousBiteChanceToApply = calculatedStats.petHitTotal - calculatedStats.petTargetDodge;
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

                stacks[0].total = 0;
                stacks[1].total = 0.03 * stacks[1].percent_time;
                stacks[2].total = 0.06 * stacks[1].percent_time;
                stacks[3].total = stacks[3].chance_to_max == 1 ? 0.09 : 0.09 * stacks[1].percent_time;

                double monstrousBiteProcEffect = stacks[0].total + stacks[1].total + stacks[2].total + stacks[3].total;

                damageAdjustMonstrousBite = 1 + monstrousBiteProcEffect;
            }

            double damageAdjustMangle = 1 + statsHunterBuffs.BonusBleedDamageMultiplier;

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
            double whiteDamageAdjust = damageAdjustWhite * damageAdjustCobraReflexes * damageAdjustMitigation * damageAdjustGlancingBlows;
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
                    double rakeDots = S.frequency > 9 ? 3 : 2;

                    S.damage = rakeInitialHitDamage + rakeDotDamage * rakeDots;
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
                    double scorpidPoisionDamageAverage = ((100 + 130) / 2) + damageBonusSpellsFromAP;
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

                    double spiritStrikeInitialDamageAverage = ((49 + 65) / 2) + damageBonusSpellsFromAP;
                    double spiritStrikeInitialDamageNormal = spiritStrikeInitialDamageAverage * spiritStrikeAdjust * damageAdjustHitCritSpecials;

                    double spiritStrikeDotDamageAverage = ((49 + 65) / 2) + damageBonusSpellsFromAP;
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
                    S.damage = acidSpitAverage * (calculatedStats.petCritTotalSpecials * 1.5 + 1) * fullResistDamageAdjust * partialResistDamageAdjust * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                }

                #endregion

                if (options.emulateSpreadsheetBugs && S.skillType == PetAttacks.NetherShock) S.damage = 0; // still an issue in 91e
                if (options.emulateSpreadsheetBugs && S.skillType == PetAttacks.VenomWebSpray) S.damage = 0; // still an issue in 91e

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

         
            calculatedStats.petSpecialDPS = priorityRotation.dps;

            calculatedStats.PetDpsPoints = (float)(calculatedStats.petWhiteDPS 
                                                + calculatedStats.petSpecialDPS 
                                                + calculatedStats.petKillCommandDPS);
        }
    }
}
