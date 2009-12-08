using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Rawr.Hunter
{   
    public class PetCalculations
    {
        #region Variables
        Character character;
        CharacterCalculationsHunter calculatedStats;
        CalculationOptionsHunter CalcOpts;
        Stats statsPetBuffs;
        Stats statsHunterBuffs;
        Stats statsGear;
        Stats StatS;
        public Stats petStats;
        private PetSkillPriorityRotation priorityRotation;

        public float ferociousInspirationUptime;
        private List<float> freqs = new List<float>();

        // things we save earlier for later DPS calcs
        private float attackSpeedEffective;
        private float compositeSpeed;
        private float killCommandCooldown;
        private float critSpecialsAdjust;

        private float PetChanceToMiss = StatConversion.YELLOW_MISS_CHANCE_CAP[83 - 80];
        private float PetChanceToSpellMiss = StatConversion.GetSpellMiss(83 - 80, false);
        private float PetChanceToBeDodged = StatConversion.YELLOW_DODGE_CHANCE_CAP[83 - 80];
        #endregion

        public PetCalculations(Character character, CharacterCalculationsHunter calculatedStats, CalculationOptionsHunter options,
            Stats stats,Stats statsHunterBuffs, Stats statsPetBuffs, Stats statsGear)
        {
            this.character = character;
            this.calculatedStats = calculatedStats;
            this.CalcOpts = options;
            this.statsHunterBuffs = statsHunterBuffs;
            this.statsPetBuffs = statsPetBuffs;
            this.statsGear = statsGear;
            this.StatS = stats;

            petStats = new Stats();
        }

        public void calculateTimings()
        {
            // See the main hunter module file (CalculationsHunter.cs) for credits
            // and source information.

            // base stats for a level 80 pet
            petStats.Agility   = 113;
            petStats.Strength  = 331;
            petStats.Stamina   = 361;
            petStats.Intellect =  65;
            petStats.Spirit    =  10;

            statsPetBuffs.Stamina  += statsPetBuffs.PetStamina;
            statsPetBuffs.Strength += statsPetBuffs.PetStrength;
            statsPetBuffs.Spirit   += statsPetBuffs.PetSpirit;

            if (statsPetBuffs._rawSpecialEffectData != null && statsPetBuffs._rawSpecialEffectData.Length > 0) {
                Stats add = statsPetBuffs._rawSpecialEffectData[0].GetAverageStats();
                statsPetBuffs.PhysicalHaste *= (1f + add.PhysicalHaste);
            }

            int levelDifference = CalcOpts.TargetLevel - character.Level;

            #region Focus Regen
            float focusRegenBasePer4 = 20f;
            float focusRegenBestialDiscipline = focusRegenBasePer4 * 0.5f * character.HunterTalents.BestialDiscipline;            

            float critHitsPer4 = calculatedStats.shotsPerSecondCritting * calculatedStats.priorityRotation.critsCompositeSum*4f;
            float goForTheThroatPerCrit = 25 * character.HunterTalents.GoForTheThroat;
            float focusRegenGoForTheThroat = critHitsPer4 * goForTheThroatPerCrit;

            float focusRegenPerSecond = (focusRegenBasePer4 + focusRegenBestialDiscipline + focusRegenGoForTheThroat)/4f;

            float owlsFocusEffect = (CalcOpts.PetTalents.OwlsFocus.Value > 0) ? owlsFocusEffect = 1f / (1f / (CalcOpts.PetTalents.OwlsFocus.Value * 0.15f) + 1f) : 0f;
            #endregion
            #region Special Abilities Priority Rotation

            priorityRotation = new PetSkillPriorityRotation(character, CalcOpts);

            priorityRotation.AddSkill(CalcOpts.PetPriority1);
            priorityRotation.AddSkill(CalcOpts.PetPriority2);
            priorityRotation.AddSkill(CalcOpts.PetPriority3);
            priorityRotation.AddSkill(CalcOpts.PetPriority4);
            priorityRotation.AddSkill(CalcOpts.PetPriority5);
            priorityRotation.AddSkill(CalcOpts.PetPriority6);
            priorityRotation.AddSkill(CalcOpts.PetPriority7);

            priorityRotation.owlsFocus = owlsFocusEffect;
            priorityRotation.fpsGained = focusRegenPerSecond;

            priorityRotation.calculateTimings();
            #endregion
            #region Kill Command MPS

            calculatedStats.petKillCommandMPS = 0;
            killCommandCooldown = 0;

            if (CalcOpts.useKillCommand)
            {
                float killCommandManaCost = 0.03f * calculatedStats.baseMana;

                float killCommandReadinessFactor = calculatedStats.priorityRotation.containsShot(Shots.Readiness) ? 1.0f / 180f : 0f;
                float killCommandCooldownBase = 1.0f / (60f - character.HunterTalents.CatlikeReflexes * 10f);

                killCommandCooldown = 1.0f / (killCommandCooldownBase + killCommandReadinessFactor);

                calculatedStats.petKillCommandMPS = killCommandCooldown > 0 ? killCommandManaCost / killCommandCooldown : 0;
            }

            #endregion
            #region Hit Chance
            // This is tied directly to the Hunter's chance to miss
            PetChanceToMiss = Math.Max(0f, StatConversion.YELLOW_MISS_CHANCE_CAP[levelDifference] - calculatedStats.BasicStats.PhysicalHit);
            PetChanceToSpellMiss = Math.Max(0f, StatConversion.GetSpellMiss(levelDifference, false) - calculatedStats.BasicStats.SpellHit);

            calculatedStats.petHitTotal = calculatedStats.BasicStats.PhysicalHit;
            calculatedStats.petHitSpellTotal = calculatedStats.BasicStats.SpellHit;
            //calculatedStats.petExpTotal = 1f - PetChanceToSpellMiss;

            petStats.PhysicalHit = calculatedStats.BasicStats.PhysicalHit;
            petStats.SpellHit    = calculatedStats.BasicStats.SpellHit;

            // If the Hunter is Hit Capped, the pet is also Exp Capped
            // If not, Pet is proportionately lower based on Hunter's Hit
            // Expertise itself doesn't factor in at all
            PetChanceToBeDodged = StatConversion.YELLOW_DODGE_CHANCE_CAP[levelDifference]
                                * (1f - (calculatedStats.BasicStats.PhysicalHit / StatConversion.YELLOW_MISS_CHANCE_CAP[levelDifference]));
            calculatedStats.petTargetDodge = PetChanceToBeDodged;
            #endregion
            #region Crit Chance

            float critAgilityBase = petStats.Agility;
            float critAgilityBuffsAdditive = statsPetBuffs.Agility;
            float critAgilityBuffsMultiplicitive = statsPetBuffs.BonusAgilityMultiplier;
            float critAgilityTotal = (critAgilityBase + critAgilityBuffsAdditive) * (1 + critAgilityBuffsMultiplicitive);

            calculatedStats.petCritFromBase = 0.032f;
            calculatedStats.petCritFromAgility = StatConversion.GetCritFromAgility(critAgilityTotal, CharacterClass.Warrior); // it matched the warrior before, probably need to proof this
            calculatedStats.petCritFromSpidersBite = CalcOpts.PetTalents.SpidersBite.Value * 0.03f;
            calculatedStats.petCritFromFerocity = character.HunterTalents.Ferocity * 0.02f;
            calculatedStats.petCritFromBuffs = statsPetBuffs.PhysicalCrit + StatConversion.GetCritFromRating(statsPetBuffs.CritRating);
            calculatedStats.petCritFromTargetDebuffs = calculatedStats.targetDebuffsCrit;
            calculatedStats.petCritFromDepression = StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];

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
            float cobraStrikesProc = character.HunterTalents.CobraStrikes * 0.2f;
            if (cobraStrikesProc > 0)
            {
                float cobraStrikesCritFreqSteady = calculatedStats.steadyShot.Freq * (1f / calculatedStats.steadyShot.CritChance);
                float cobraStrikesCritFreqArcane = calculatedStats.arcaneShot.Freq * (1f / calculatedStats.arcaneShot.CritChance);
                float cobraStrikesCritFreqKill = calculatedStats.killShot.Freq * (1f / calculatedStats.killShot.CritChance);

                float cobraStrikesCritFreqSteadyInv = cobraStrikesCritFreqSteady > 0 ? 1f / cobraStrikesCritFreqSteady : 0;
                float cobraStrikesCritFreqArcaneInv = cobraStrikesCritFreqArcane > 0 ? 1f / cobraStrikesCritFreqArcane : 0;
                float cobraStrikesCritFreqKillInv = cobraStrikesCritFreqKill > 0 ? 1f / cobraStrikesCritFreqKill : 0;
                float cobraStrikesCritFreqInv = cobraStrikesCritFreqSteadyInv + cobraStrikesCritFreqArcaneInv + cobraStrikesCritFreqKillInv;
                float cobraStrikesCritFreq = cobraStrikesCritFreqInv > 0 ? 1f / cobraStrikesCritFreqInv : 0;
                float cobraStrikesProcAdjust = cobraStrikesCritFreq / cobraStrikesProc;

                float cobraStrikesFreqSteadyInv = calculatedStats.steadyShot.Freq > 0 ? 1f / calculatedStats.steadyShot.Freq : 0;
                float cobraStrikesFreqArcaneInv = calculatedStats.arcaneShot.Freq > 0 ? 1f / calculatedStats.arcaneShot.Freq : 0;
                float cobraStrikesFreqKillInv = calculatedStats.killShot.Freq > 0 ? 1f / calculatedStats.killShot.Freq : 0;
                float cobraStrikesFreqInv = cobraStrikesFreqSteadyInv + cobraStrikesFreqArcaneInv + cobraStrikesFreqKillInv;

                float cobraStrikesTwoSpecials = 2f * priorityRotation.petSpecialFrequency;
                float cobraStrikesUptime = 1f - (float)Math.Pow(1f - calculatedStats.steadyShot.CritChance * cobraStrikesProc, cobraStrikesFreqInv * cobraStrikesTwoSpecials);

                calculatedStats.petCritFromCobraStrikes = (cobraStrikesUptime + (1f - cobraStrikesUptime) * calculatedStats.petCritTotalMelee) - calculatedStats.petCritTotalMelee;
            }

            calculatedStats.petCritTotalSpecials = calculatedStats.petCritTotalMelee + calculatedStats.petCritFromCobraStrikes; // PetCritChance
            critSpecialsAdjust = calculatedStats.petCritTotalSpecials * 1.5f + 1f;

            #endregion
            #region Attack Speed
            float attackSpeedBase = CalcOpts.PetFamily == PetFamily.None ? 0.0f : 2.0f;

            float attackSpeedFromSerpentsSwiftness = 1f + (character.HunterTalents.SerpentsSwiftness * 0.04f);
            float attackSpeedFromCobraReflexes = 1f + (CalcOpts.PetTalents.CobraReflexes.Value * 0.15f);
            float attackSpeedFromMultiplicitiveHasteBuffs = 1f + statsPetBuffs.PhysicalHaste;
            float attackSpeedAdjust = attackSpeedFromSerpentsSwiftness
                                     * attackSpeedFromCobraReflexes
                                     * attackSpeedFromMultiplicitiveHasteBuffs;

            float attackSpeedAdjusted = attackSpeedBase / attackSpeedAdjust;

            if (character.HunterTalents.Frenzy > 0) {
                SpecialEffect frenzy = new SpecialEffect(Trigger.MeleeCrit,
                    new Stats() { PhysicalHaste = 0.30f, },
                    8f, 0f, character.HunterTalents.Frenzy * 0.20f);
                float FrenzyHaste = frenzy.GetAverageStats(attackSpeedAdjusted,
                    calculatedStats.petCritTotalMelee, attackSpeedAdjusted, CalcOpts.Duration).PhysicalHaste;
                attackSpeedAdjust *= 1f + FrenzyHaste;
            }
            attackSpeedAdjusted = attackSpeedBase / attackSpeedAdjust;

            //float attackSpeedFrenzyBoost = 1f + (0.3f * attackSpeedFrenzyUptime);

            attackSpeedEffective = attackSpeedAdjusted; // EffectivePetAttackSpeed            

            compositeSpeed = 1f / (1f / attackSpeedEffective + (priorityRotation.petSpecialFrequency > 0 ? 1f / priorityRotation.petSpecialFrequency : 0)); // PetCompSpeed
            #endregion
            #region Target Debuffs
            float armorDebuffSporeCloud = 0;
            float sporeCloudFrequency = priorityRotation.getSkillFrequency(PetAttacks.SporeCloud);
            if (sporeCloudFrequency > 0)
            {
                float sporeCloudDuration = 9f;
                float sporeCloudUptime = sporeCloudDuration > sporeCloudFrequency ? 1f : sporeCloudDuration / sporeCloudFrequency;

                armorDebuffSporeCloud = sporeCloudUptime * 0.03f;
            }

            float armorDebuffAcidSpit = 0;
            float acidSpitFrequency = priorityRotation.getSkillFrequency(PetAttacks.AcidSpit); // AcidSpitEffectiveRate
            if (acidSpitFrequency > 0)
            {
                float acidSpitCalcFreq = priorityRotation.getSkillCooldown(PetAttacks.AcidSpit);
                float acidSpitDuration = 30;

                float acidSpitChanceToApply = 1f - PetChanceToMiss - PetChanceToBeDodged; // V45
                float acidSpitChancesToMaintain = (float)Math.Floor((acidSpitDuration - 1f) / acidSpitFrequency); // V46
                float acidSpitChanceToApplyFirst = acidSpitChancesToMaintain == 0 ? 0 : acidSpitChanceToApply; // V47
                float acidSpitChanceToStop = 1f - acidSpitChanceToApplyFirst; // AcidSpitChanceToStop
                float acidSpitAverageTimeToInc = acidSpitChanceToApplyFirst > 0 ? acidSpitFrequency : 0; // AcidSpitTimeToInc
                float acidSpitTimeSpentAtMax = acidSpitAverageTimeToInc + (acidSpitDuration * acidSpitChanceToStop); // AcidSpitAverageStackTime

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

                float acidSpitTotalTime = stacks[0].time_spent + stacks[1].time_spent + stacks[2].time_spent;

                stacks[0].percent_time = acidSpitTotalTime == 0 ? 1 : stacks[0].time_spent / acidSpitTotalTime;
                stacks[1].percent_time = acidSpitTotalTime == 0 ? 1 : stacks[1].time_spent / acidSpitTotalTime;
                stacks[2].percent_time = acidSpitTotalTime == 0 ? 1 : stacks[2].time_spent / acidSpitTotalTime;

                stacks[0].total = stacks[0].percent_time * 0.0f;
                stacks[1].total = stacks[0].percent_time * 0.1f;
                stacks[2].total = stacks[0].percent_time * 0.2f;

                armorDebuffAcidSpit = stacks[0].total + stacks[1].total + stacks[2].total;
            }

            float armorDebuffSting = 0;
            float stingFrequency = priorityRotation.getSkillFrequency(PetAttacks.Sting);
            if (stingFrequency > 0)
            {
                float stingUseFreq = priorityRotation.getSkillFrequency(PetAttacks.Sting);
                float stingDuration = 20;
                float stingUptime = stingDuration > stingUseFreq ? 1 : stingDuration / stingUseFreq;

                armorDebuffSting = stingUptime * 0.05f;
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

            calculatedStats.petArmorDebuffs = 0f - (1f - armorDebuffSporeCloud) * (1f - armorDebuffAcidSpit) * (1f - armorDebuffSting) + 1;

            #endregion
            #region Hunter Effects
            // Ferocious Inspiraion
            // (Same as above)
            calculatedStats.ferociousInspirationDamageAdjust = 1;
            if (character.HunterTalents.FerociousInspiration > 0) {
                if (CalcOpts.PetFamily != PetFamily.None) {
                    float ferociousInspirationSpecialsEffect = priorityRotation.petSpecialFrequency == 0 ? 0 : 10f / priorityRotation.petSpecialFrequency;
                    float ferociousInspirationUptime = 1f - (float)Math.Pow(1f - calculatedStats.petCritTotalSpecials, (10f / attackSpeedEffective) + ferociousInspirationSpecialsEffect);
                    float ferociousInspirationEffect = 0.01f * character.HunterTalents.FerociousInspiration;

                    calculatedStats.ferociousInspirationDamageAdjust = 1.0f + ferociousInspirationUptime * ferociousInspirationEffect;                    
                }
            }

            // Roar of Recovery
            calculatedStats.manaRegenRoarOfRecovery = 0;
            float roarOfRecoveryFreq = priorityRotation.getSkillFrequency(PetAttacks.RoarOfRecovery);
            if (roarOfRecoveryFreq > 0) {
                float roarOfRecoveryUseCount = (float)Math.Ceiling(CalcOpts.Duration / roarOfRecoveryFreq);
                float roarOfRecoveryManaRestored = calculatedStats.BasicStats.Mana * 0.3f * roarOfRecoveryUseCount; // E129
                calculatedStats.manaRegenRoarOfRecovery = roarOfRecoveryUseCount > 0 ? roarOfRecoveryManaRestored / CalcOpts.Duration : 0;
            }

            //Invigoration
            //calculatedStats.manaRegenInvigoration = 0;
            float invigorationProcChance = character.HunterTalents.Invigoration * 0.5f; // C32
            if (invigorationProcChance > 0) {
                float invigorationProcFreq = (priorityRotation.petSpecialFrequency / calculatedStats.petCritTotalSpecials) / invigorationProcChance; //C35
                float invigorationEffect = character.HunterTalents.Invigoration > 0 ? 0.01f : 0;
                float invigorationManaGainedPercent = invigorationProcFreq > 0 ? 60f / invigorationProcFreq * invigorationEffect : 0; // C36
                float invigorationManaPerMinute = invigorationProcFreq > 0 ? 60f / invigorationProcFreq * invigorationEffect * calculatedStats.BasicStats.Mana : 0; // C37
                calculatedStats.manaRegenInvigoration = invigorationManaPerMinute / 60f;
            }
            #endregion

            #region Target Armor Effect
            //31-10-2009 Drizz: added Armor effect
            double petEffectiveArmor = CalcOpts.TargetArmor * (1f - calculatedStats.petArmorDebuffs);
            calculatedStats.petTargetArmorReduction =
                StatConversion.GetArmorDamageReduction(80, CalcOpts.TargetArmor, calculatedStats.petArmorDebuffs, 0, 0);
                //petEffectiveArmor/(petEffectiveArmor - 22167.5f + (467.5f*80f));
            #endregion
        }

        public void calculateDPS()
        {
            if (CalcOpts.PetFamily == PetFamily.None) return;

            int levelDifference = CalcOpts.TargetLevel - 80;

            // setup
            #region Attack Power

            petStats.Strength += statsPetBuffs.Strength;
            petStats.Strength *= 1.0f + statsPetBuffs.BonusStrengthMultiplier;

            float apFromHunterScaling = 0.22f * (1 + CalcOpts.PetTalents.WildHunt.Value * 0.15f);
            calculatedStats.petAPFromStrength = (petStats.Strength - 10f) * 2f;
            calculatedStats.petAPFromHunterVsWild = (float)Math.Floor(calculatedStats.BasicStats.Stamina * (0.1f * character.HunterTalents.HunterVsWild));
            calculatedStats.petAPFromBuffs = statsPetBuffs.AttackPower;
            calculatedStats.petAPFromHunterRAP = (float)Math.Floor(calculatedStats.apSelfBuffed * apFromHunterScaling);

            // Tier 9 4-pice bonus is complex
            calculatedStats.petAPFromTier9 = 0;

            // 31-10-2009 Drizz: Name in buffs seems to be Battlegear instead of Pursuit (i.e. not Alliance just Horde)
            if (calculatedStats.BasicStats.BonusHunter_T9_4P_SteadyShotPetAPProc > 0)
            {
                float tier9BonusTimePetShot = 0.9f;
                float tier9BonusTimeBetween = tier9BonusTimePetShot > 0 ? 1f / 0.15f * tier9BonusTimePetShot + 45f : 0;
                SpecialEffect tier94pc = new SpecialEffect(Trigger.PhysicalHit, new Stats() { AttackPower = 600f, }, 15f, 0f, 0.35f);
                float tier94pcUptime = tier94pc.GetAverageUptime(1.5f,
                    1f - Math.Max(0f, StatConversion.YELLOW_MISS_CHANCE_CAP[CalcOpts.TargetLevel - character.Level] - calculatedStats.BasicStats.PhysicalHit),
                    calculatedStats.autoShotStaticSpeed, CalcOpts.Duration);
                calculatedStats.petAPFromTier9 = 600f * tier94pcUptime;
            }

            // Serenity Dust
            calculatedStats.petAPFromSerenityDust = 0;
            if (priorityRotation.getSkillFrequency(PetAttacks.SerenityDust) > 0)
            {
                calculatedStats.petAPFromSerenityDust = 0.025f; // 0.1 * (15 / 60);
            }

            calculatedStats.petAPFromAnimalHandler = character.HunterTalents.AnimalHandler * 0.05f;
            calculatedStats.petAPFromAspectOfTheBeast = calculatedStats.aspectBonusAPBeast;

            calculatedStats.petAPFromOutsideBuffs = statsPetBuffs.BonusAttackPowerMultiplier;

            calculatedStats.petAPFromRabidProc = 0;

            float longevityCooldownAdjust = 1f - character.HunterTalents.Longevity * 0.1f;
            float rabidCooldown = 45f * longevityCooldownAdjust;
            SpecialEffect rabid = new SpecialEffect(Trigger.Use,
                new Stats() { BonusAttackPowerMultiplier = 0.05f },
                20, rabidCooldown, 0.50f, 5);
            //float rabidUptime = CalcOpts.PetTalents.Rabid.Value * rabid.GetAverageUptime(0f, 1f, 2f, CalcOpts.Duration);

            if(CalcOpts.PetTalents.Rabid.Value > 0){
                SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats() { }, 20f, rabidCooldown);
                SpecialEffect secondary = new SpecialEffect(Trigger.MeleeHit,
                    new Stats() { BonusAttackPowerMultiplier = 0.05f }, 20f, 0f, 0.50f, 5);
                calculatedStats.petAPFromRabidProc = secondary.Stats.BonusAttackPowerMultiplier
                    * (secondary.GetAverageStackSize(compositeSpeed, (1f - PetChanceToMiss - PetChanceToBeDodged), compositeSpeed, CalcOpts.Duration)
                    * primary.GetAverageUptime(0f, 1f, compositeSpeed, CalcOpts.Duration));
            }

            /* OBSOLETE WITH SPECIAL EFFECTS
            if (rabidUptime > 0)
            {
                float rabidDuration = 20; // Q24
                // Q25 - rabidCooldown
                float rabidEffectiveRate = compositeSpeed; // Q26
                float rabidHit = PetChanceToMiss;
                float rabidDodge = PetChanceToBeDodged; // Q28
                float rabidChanceToApply = 0.5f * (rabidHit - rabidDodge); // Q29
                float rabidChancesToMaintain = rabidUptime > 0 ? rabidCooldown / rabidEffectiveRate : 0; // Q30

                float rabidChanceApply1 = rabidChancesToMaintain == 0 ? 0 : rabidChanceToApply; // S22
                float rabidChanceApply2 = rabidChancesToMaintain >= 2 ? (1 - rabidChanceApply1) * rabidChanceToApply : 0; // S23
                float rabidChanceApply3 = rabidChancesToMaintain >= 3 ? (1 - (rabidChanceApply1 + rabidChanceApply2)) * rabidChanceToApply : 0; // S24
                float rabidChanceApply4 = rabidChancesToMaintain >= 4 ? (1 - (rabidChanceApply1 + rabidChanceApply2 + rabidChanceApply3)) * rabidChanceToApply : 0; // S25
                float rabidChanceApply5 = rabidChancesToMaintain >= 5 ? (1 - (rabidChanceApply1 + rabidChanceApply2 + rabidChanceApply3 + rabidChanceApply4)) * rabidChanceToApply : 0; // S26
                float rabidChanceApplySum = rabidChanceApply1 + rabidChanceApply2 + rabidChanceApply3 + rabidChanceApply4 + rabidChanceApply5; // SUM(S22:S26)
                float rabidChanceFallOff = 1-rabidChanceApplySum < 0 ? 0 : 1-rabidChanceApplySum; // S27 RabidChanceToStop

                float rabidAverageIncTime = 0; // S28 RabidAverageTimeToInc
                if (1 - rabidChanceFallOff != 0)
                {
                    rabidAverageIncTime = rabidChanceApply1 / rabidChanceApplySum * rabidEffectiveRate
                                        + rabidChanceApply2 / rabidChanceApplySum * rabidEffectiveRate * 2
                                        + rabidChanceApply3 / rabidChanceApplySum * rabidEffectiveRate * 3
                                        + rabidChanceApply4 / rabidChanceApplySum * rabidEffectiveRate * 4
                                        + rabidChanceApply5 / rabidChanceApplySum * rabidEffectiveRate * 5;
                }
                float rabidTimeSpentMax = rabidUptime > 0 ? rabidAverageIncTime + (rabidEffectiveRate * rabidChanceFallOff) : 0; // RabidAverageStackTime

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

                float rabidTotalTime = stacks[0].time_spent + stacks[1].time_spent + stacks[2].time_spent + stacks[3].time_spent
                                      + stacks[4].time_spent + stacks[5].time_spent + stacks[6].time_spent;

                stacks[0].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[0].time_spent / rabidTotalTime;
                stacks[1].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[1].time_spent / rabidTotalTime;
                stacks[2].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[2].time_spent / rabidTotalTime;
                stacks[3].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[3].time_spent / rabidTotalTime;
                stacks[4].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[4].time_spent / rabidTotalTime;
                stacks[5].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[5].time_spent / rabidTotalTime;
                stacks[6].percent_time = rabidTotalTime == 0 ? (rabidUptime > 0 ? 1 : 0) : stacks[6].time_spent / rabidTotalTime;

                stacks[0].total = stacks[0].percent_time * 0.00f;
                stacks[1].total = stacks[1].percent_time * 0.05f;
                stacks[2].total = stacks[2].percent_time * 0.10f;
                stacks[3].total = stacks[3].percent_time * 0.15f;
                stacks[4].total = stacks[4].percent_time * 0.20f;
                stacks[5].total = stacks[5].percent_time * 0.25f;
                stacks[6].total = stacks[6].percent_time * 0.00f;

                calculatedStats.petAPFromRabidProc = rabidEffectiveRate > 0 ? stacks[0].total + stacks[1].total + stacks[2].total + stacks[3].total
                                                               + stacks[4].total + stacks[5].total + stacks[6].total : 0;
            }
            */
            float apScalingFactor = 1f
                                 //* (1f + calculatedStats.petAPFromCallOfTheWild)
                                 * (1f + calculatedStats.petAPFromRabidProc)
                                 * (1f + calculatedStats.petAPFromSerenityDust)
                                 * (1f + calculatedStats.petAPFromOutsideBuffs)
                                 * (1f + calculatedStats.petAPFromAnimalHandler)
                                 * (1f + calculatedStats.petAPFromAspectOfTheBeast);

            float apTotalUnadjusted = calculatedStats.petAPFromStrength
                                    + calculatedStats.petAPFromHunterVsWild
                                    + calculatedStats.petAPFromTier9
                                    + calculatedStats.petAPFromBuffs
                                    + calculatedStats.petAPFromHunterRAP;

            float apTotal = (float)Math.Round(apTotalUnadjusted * apScalingFactor, 1);

            float damageBonusMeleeFromAP = 0.07f * apTotal; // PetAPBonus
            float damageBonusSpellsFromAP = (1.5f / 35f) * apTotal; // PetSpellScaling * PetAP

            petStats.AttackPower = (float)apTotal;

            #endregion
            #region Spell Hit

            //Full Resists (Spell Hit)
            float levelResistFactor = 0.04f;
            if (levelDifference == 1) levelResistFactor = 0.05f;
            if (levelDifference == 2) levelResistFactor = 0.06f;
            if (levelDifference == 3) levelResistFactor = 0.08f;
            
            float fullResist = levelResistFactor - (1f - PetChanceToSpellMiss);
            if (fullResist < 0) fullResist = 0;
            float fullResistDamageAdjust = 1f - fullResist;

            //Partial Resists (Spell Hit)
            float averageResist = (CalcOpts.TargetLevel - character.Level) * 0.02f;
            float resist10 = 5.0f * averageResist;
            float resist20 = 2.5f * averageResist;
            float partialResistDamageAdjust = 1f - (resist10 * 0.1f + resist20 * 0.2f);

            #endregion
            #region Damage Adjustments

            // Dodge adjust can't help us, only hinder
            float damageAdjustDodge = Math.Min(1f, 1f - PetChanceToBeDodged);

            float damageAdjustHitCritMelee    = (2f * calculatedStats.petCritTotalMelee   ) + (1f - (PetChanceToMiss + PetChanceToBeDodged) - calculatedStats.petCritTotalMelee);
            float damageAdjustHitCritSpecials = (2f * calculatedStats.petCritTotalSpecials) + (1f - (PetChanceToMiss + PetChanceToBeDodged) - calculatedStats.petCritTotalSpecials); // CritAdjustments

            float damageAdjustUnleashedFury = 1f + (character.HunterTalents.UnleashedFury * 0.03f);
            float damageAdjustMood = CalcOpts.PetHappinessLevel == PetHappiness.Happy ? 1.25f : CalcOpts.PetHappinessLevel == PetHappiness.Content ? 1f : 0.75f;
            float damageAdjustSpikedCollar = 1f + (CalcOpts.PetTalents.SpikedCollar.Value * 0.03f);
            float damageAdjustRaceModifier = character.Race == CharacterRace.Orc ? 1.05f : 1f;
            float damageAdjustFerociousInspiration = calculatedStats.ferociousInspirationDamageAdjust;
            float damageAdjustKindredSpirits = 1f + (character.HunterTalents.KindredSpirits * 0.04f);
            float damageAdjustSancRetributionAura = 1f + statsPetBuffs.BonusDamageMultiplier;
            float damageAdjustTier7Bonus = 1f + statsHunterBuffs.BonusPetDamageMultiplier;
            float damageAdjustBonusPetDamageMult = 1f + StatS.BonusPetDamageMultiplier;
            float damageAdjustSharkAttack = 1f + (CalcOpts.PetTalents.SharkAttack.Value * 0.03f);
            float damageAdjustTargetDebuffs = calculatedStats.targetDebuffsPetDamage;
            float damageAdjustPetFamily = 1.05f;
            float damageAdjustMarkedForDeath = 1f + (character.HunterTalents.MarkedForDeath * 0.02f);
            float damageAdjustCobraReflexes = 1f - (CalcOpts.PetTalents.CobraReflexes.Value * 0.075f); // this is a negative effect!

            // Feeding Frenzy
            float damageAdjustFeedingFrenzy = 1;
            if (CalcOpts.PetTalents.FeedingFrenzy.Value > 0)
            {
                float feedingFrenzyTimeSpent = CalcOpts.timeSpentSub20 + CalcOpts.timeSpent35To20;
                float feedingFrenzyUptime = feedingFrenzyTimeSpent > 0 ? feedingFrenzyTimeSpent / CalcOpts.Duration : 0;
                damageAdjustFeedingFrenzy = 1f + feedingFrenzyUptime * CalcOpts.PetTalents.FeedingFrenzy.Value * 0.08f;
            }

            // Glancing Blows
            float glancingBlowsSkillDiff = (CalcOpts.TargetLevel * 5) - (CalcOpts.PetLevel * 5); // F55
            if (glancingBlowsSkillDiff < 0) glancingBlowsSkillDiff = 0;
            float glancingBlowsChance  = glancingBlowsSkillDiff > 15 ? 0.25f : 0.1f + glancingBlowsSkillDiff * 0.01f; // F56
            float glancingBlowsLowEnd  = (float)Math.Min(1.3f - 0.05f * glancingBlowsSkillDiff, 0.91f); // F57
            float glancingBlowsHighEnd = (float)Math.Min(1.2f - 0.03f * glancingBlowsSkillDiff, 0.99f); // F58
            float damageAdjustGlancingBlows = 1 - (glancingBlowsChance * (1 - ((glancingBlowsLowEnd + glancingBlowsHighEnd) / 2)));

            // Bestial Wrath
            /*float damageAdjustBeastialWrath = 1;
            if (calculatedStats.beastialWrath.freq > 0)
            {
                float beastialWrathUptime = character.HunterTalents.BestialWrath > 0 ? 10f / calculatedStats.beastialWrath.freq : 0;
                damageAdjustBeastialWrath = 1f + beastialWrathUptime * 0.5f;
            }*/

            // Savage Rend
            float damageAdjustSavageRend = 1;
            float savageRendFrequency = priorityRotation.getSkillFrequency(PetAttacks.SavageRend);
            if (savageRendFrequency > 0)
            {
                float savageRendTimeBetweenProcs = savageRendFrequency * (1f / calculatedStats.petCritTotalSpecials);
                float savageRendUptime = savageRendTimeBetweenProcs > 0 ? 30f / savageRendTimeBetweenProcs : 0;
                damageAdjustSavageRend = 1f + 0.1f * savageRendUptime;
            }

            // Monstrous Bite
            float damageAdjustMonstrousBite = 1;
            float monstrousBiteFrequency = priorityRotation.getSkillFrequency(PetAttacks.MonstrousBite);
            if (monstrousBiteFrequency > 0)
            {
                float monstrousBiteUseFreq = monstrousBiteFrequency;
                float monstrousBiteDuration = 12;

                float monstrousBiteChanceToApply = 1f - PetChanceToMiss - PetChanceToBeDodged;
                float monstrousBiteChancesToMaintain = 1;

                float monstrousBiteChanceToApplyFirst = monstrousBiteChancesToMaintain == 0 ? 0 : monstrousBiteChanceToApply;
                float monstrousBiteChanceToStop = 1f - monstrousBiteChanceToApplyFirst;

                float monstrousBiteAverageIncTime = 1f - monstrousBiteChanceToStop == 0 ? 0 : monstrousBiteUseFreq;
                float monstrousBiteAverageStackTime = monstrousBiteAverageIncTime + monstrousBiteDuration * monstrousBiteChanceToStop;

                PetSkillStack[] stacks = new PetSkillStack[4];
                stacks[0] = new PetSkillStack();
                stacks[1] = new PetSkillStack();
                stacks[2] = new PetSkillStack();
                stacks[3] = new PetSkillStack();

                stacks[0].time_to_reach = 0;
                stacks[1].time_to_reach = 1 * monstrousBiteAverageIncTime;
                stacks[2].time_to_reach = 2 * monstrousBiteAverageIncTime;
                stacks[3].time_to_reach = 3 * monstrousBiteAverageIncTime;

                stacks[0].chance_to_max = 0;
                stacks[1].chance_to_max = monstrousBiteChanceToStop == 1 ? 0 : monstrousBiteChanceToStop;
                stacks[2].chance_to_max = monstrousBiteChanceToStop == 1 ? 0 : monstrousBiteChanceToStop * (1 - stacks[1].chance_to_max);
                stacks[3].chance_to_max = monstrousBiteChanceToStop == 1 ? 0 : 1 - (stacks[0].chance_to_max + stacks[1].chance_to_max + stacks[2].chance_to_max);

                stacks[0].time_spent = stacks[3].chance_to_max == 1 ? 0 : stacks[1].time_to_reach;
                stacks[1].time_spent = stacks[3].chance_to_max == 1 ? 0 : (stacks[1].time_to_reach == 0 ? 0 : monstrousBiteAverageStackTime) * (1 - (stacks[0].chance_to_max));
                stacks[2].time_spent = stacks[3].chance_to_max == 1 ? 0 : (stacks[2].time_to_reach == 0 ? 0 : monstrousBiteAverageStackTime) * (1 - (stacks[0].chance_to_max + stacks[1].chance_to_max));
                stacks[3].time_spent = monstrousBiteChanceToStop == 0 ? 1 : stacks[3].time_to_reach == 0 ? 0 : 1 / monstrousBiteChanceToStop * monstrousBiteUseFreq * (1 - (stacks[0].chance_to_max + stacks[1].chance_to_max + stacks[2].chance_to_max));

                float monstrousBiteTotalTime = stacks[0].time_spent + stacks[1].time_spent + stacks[2].time_spent + stacks[3].time_spent;

                stacks[0].percent_time = monstrousBiteTotalTime == 0 ? 1 : stacks[0].time_spent / monstrousBiteTotalTime;
                stacks[1].percent_time = monstrousBiteTotalTime == 0 ? 0 : stacks[1].time_spent / monstrousBiteTotalTime;
                stacks[2].percent_time = monstrousBiteTotalTime == 0 ? 0 : stacks[2].time_spent / monstrousBiteTotalTime;
                stacks[3].percent_time = monstrousBiteTotalTime == 0 ? 0 : stacks[3].time_spent / monstrousBiteTotalTime;

                stacks[0].total = 0f;
                stacks[1].total = 0.03f * stacks[1].percent_time;
                stacks[2].total = 0.06f * stacks[1].percent_time;
                stacks[3].total = stacks[3].chance_to_max == 1f ? 0.09f : 0.09f * stacks[1].percent_time;

                float monstrousBiteProcEffect = stacks[0].total + stacks[1].total + stacks[2].total + stacks[3].total;

                damageAdjustMonstrousBite = 1f + monstrousBiteProcEffect;
            }

            float damageAdjustMangle = 1f + statsHunterBuffs.BonusBleedDamageMultiplier;

            // Pets don't get ArP Ratings passed, spreadsheet agrees and no mention of it in the community
            float damageAdjustMitigation = 1f - StatConversion.GetArmorDamageReduction(
                character.Level, CalcOpts.TargetArmor, statsPetBuffs.ArmorPenetration, 0f, 0f);

            float damageAdjustBase = 1f
                    * damageAdjustBonusPetDamageMult
                    * damageAdjustUnleashedFury
                    * damageAdjustMood
                    * damageAdjustSpikedCollar
                    * damageAdjustRaceModifier
                    * damageAdjustFerociousInspiration
                 // * damageAdjustBeastialWrath
                    * damageAdjustKindredSpirits
                    * damageAdjustMonstrousBite
                    * damageAdjustSavageRend
                    * damageAdjustFeedingFrenzy
                    * damageAdjustSancRetributionAura
                    * damageAdjustTier7Bonus
                    * damageAdjustSharkAttack
                    * damageAdjustTargetDebuffs
                    * damageAdjustPetFamily;

            float damageAdjustDots = damageAdjustBase; // BasePetModifier
            float damageAdjustWhite = damageAdjustBase * damageAdjustHitCritMelee;
            float damageAdjustMelee = damageAdjustBase * damageAdjustHitCritSpecials; // MeleeAttackAdjustment
            float damageAdjustSpecials = damageAdjustBase * damageAdjustHitCritSpecials * damageAdjustMarkedForDeath; // DamageAdjustment
            float damageAdjustMagic = damageAdjustBase * damageAdjustMarkedForDeath / damageAdjustTargetDebuffs; // MagicDamageAdjustments

            #endregion

            // damage
            #region White Damage

            float whiteDamageBase = (52f + 78f) / 2f;
            float whiteDamageFromAP = (float)Math.Floor(petStats.AttackPower / 14f * 2f);
            float whiteDamageNormal = whiteDamageBase + whiteDamageFromAP;
            float whiteDamageAdjust = damageAdjustWhite * damageAdjustCobraReflexes * damageAdjustMitigation * damageAdjustGlancingBlows;
            float whiteDamageReal = whiteDamageNormal * whiteDamageAdjust;

            float whiteDPS = whiteDamageReal / attackSpeedEffective;

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
                    float focusDumpDamageAverage = ((118f + 168f) / 2f) + damageBonusMeleeFromAP;
                    S.damage = focusDumpDamageAverage * damageAdjustSpecials * damageAdjustMitigation;
                }

                if (S.skillData.type == PetSkillType.SpecialMelee)
                {
                    float meleeDamageAverage = S.skillData.average + damageBonusMeleeFromAP;
                    S.damage = meleeDamageAverage * damageAdjustSpecials * damageAdjustMitigation;
                }

                if (S.skillData.type == PetSkillType.SpecialSpell)
                {
                    float spellDamageAverage = S.skillData.average + damageBonusSpellsFromAP;
                    S.damage = spellDamageAverage * critSpecialsAdjust * fullResistDamageAdjust * partialResistDamageAdjust 
                                * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                }

                #endregion
                #region Unique Skills

                if (S.skillType == PetAttacks.Rake)
                {
                    float rakeDamageFromAP = apTotal * 0.0175f;
                    float rakeAverageDamage = ((47  + 67) / 2) + rakeDamageFromAP;
                    float rakeAverageDamageDot = ((19 + 25) / 2) + rakeDamageFromAP;

                    float rakeInitialHitDamage = rakeAverageDamage * damageAdjustSpecials * damageAdjustMangle; // spreadsheet doesn't add armor mitigation, appears to be wow bug?
                    float rakeDotDamage = rakeAverageDamageDot * damageAdjustMangle * damageAdjustDots;
                    float rakeDots = S.frequency > 9 ? 3 : 2;

                    S.damage = rakeInitialHitDamage + rakeDotDamage * rakeDots;
                }

                if (S.skillType == PetAttacks.FireBreath)
                {
                    float fireBreathDamageAverage = ((43 + 57) / 2) + damageBonusSpellsFromAP;
                    float fireBreathDamageInitial = fireBreathDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust * damageAdjustMagic;
                    float fireBreathDamageDot = 50 + damageBonusSpellsFromAP;
                    S.damage = fireBreathDamageInitial + fireBreathDamageDot;
                }

                if (S.skillType == PetAttacks.SavageRend)
                {
                    float savageRendAverageHitDamage = ((59 + 83) / 2) + damageBonusMeleeFromAP;
                    float savageRendAverageBleedDamage = ((21 + 27) / 2) + damageBonusMeleeFromAP;
                    
                    float savageRendHitDamage = savageRendAverageHitDamage * damageAdjustSpecials * damageAdjustMangle * damageAdjustMitigation;
                    float savageRendBleedDamage = savageRendAverageBleedDamage * damageAdjustDots * damageAdjustMangle;

                    S.damage = savageRendHitDamage + 3 * savageRendBleedDamage;
                }

                if (S.skillType == PetAttacks.ScorpidPoison)
                {
                    float scorpidPoisionDamageAverage = ((100 + 130) / 2) + damageBonusSpellsFromAP;
                    float scorpidPoisionDamageNormal = scorpidPoisionDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust
                                                        * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                    float scorpidPoisionDamageTick = scorpidPoisionDamageNormal / 5;
                    float scorpidPoisionTicks = (float)Math.Floor(S.cooldown / 2f);
                    S.damage = scorpidPoisionDamageTick * scorpidPoisionTicks;
                }

                if (S.skillType == PetAttacks.PoisonSpit)
                {
                    float poisonSpitDamageAverage = ((104 + 136) / 2) + damageBonusSpellsFromAP;
                    float poisonSpitDamageNormal = poisonSpitDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust 
                                                    * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                    float poisonSpitDamageTick = poisonSpitDamageNormal / 3;
                    float poisonSpitTicks = S.cooldown >= 9 ? 3 : 2;
                    S.damage = poisonSpitDamageTick * poisonSpitTicks;
                }

                if (S.skillType == PetAttacks.VenomWebSpray)
                {
                    float venomWebSprayDamageAverage = ((46 + 68) / 2) + damageBonusSpellsFromAP;
                    float venomWebSprayDamageNormal = venomWebSprayDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust
                                                        * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                    S.damage = venomWebSprayDamageNormal * 4;
                }

                if (S.skillType == PetAttacks.SpiritStrike)
                {
                    float spiritStrikeAdjust = fullResistDamageAdjust * partialResistDamageAdjust * damageAdjustMagic
                                                     * calculatedStats.targetDebuffsNature;

                    float spiritStrikeInitialDamageAverage = ((49 + 65) / 2) + damageBonusSpellsFromAP;
                    float spiritStrikeInitialDamageNormal = spiritStrikeInitialDamageAverage * spiritStrikeAdjust * damageAdjustHitCritSpecials;

                    float spiritStrikeDotDamageAverage = ((49 + 65) / 2) + damageBonusSpellsFromAP;
                    float spiritStrikeDotDamageNormal = spiritStrikeDotDamageAverage * spiritStrikeAdjust;

                    S.damage = spiritStrikeInitialDamageNormal + spiritStrikeDotDamageNormal;
                }

                if (S.skillType == PetAttacks.SporeCloud)
                {
                    float spellDamageAverage = S.skillData.average + damageBonusSpellsFromAP;
                    S.damage = spellDamageAverage * fullResistDamageAdjust * partialResistDamageAdjust
                                    * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                }

                if (S.skillType == PetAttacks.AcidSpit)
                {
                    float acidSpitAverage = S.skillData.average + damageBonusSpellsFromAP;
                    S.damage = acidSpitAverage * (calculatedStats.petCritTotalSpecials * 1.5f + 1) * fullResistDamageAdjust * partialResistDamageAdjust * damageAdjustMagic * calculatedStats.targetDebuffsNature;
                }

                #endregion

                S.CalculateDPS();
            }

            // now add everything up...

            priorityRotation.calculateDPS();

            #endregion
            #region Kill Command

            float killCommandDPS = 0;

            if (killCommandCooldown > 0)
            {
                float killCommandDamagePerSpecial = priorityRotation.petSpecialFrequency > 0 ? priorityRotation.kc_dps / (1 / priorityRotation.petSpecialFrequency) : 0;

                float killCommandBonusDamage = killCommandDamagePerSpecial * 1.2f;

                float killCommandDPSOverCooldown = killCommandCooldown > 0 ? killCommandBonusDamage / killCommandCooldown : 0;

                float killCommandFocusedFireCritBonus = character.HunterTalents.FocusedFire * 0.1f;
                // 31-10-2009 Drizz: Remade the AdjustedBonus Calculation from the one below
                // float killCommandAdjustedBonus = killCommandFocusedFireCritBonus *  damageAdjustDodge * damageAdjustMitigation;
                float killCommandAdjustedBonus = killCommandFocusedFireCritBonus
                                               * (2f - 1f)
                                               * (1f - calculatedStats.petTargetArmorReduction);

                killCommandDPS = killCommandDPSOverCooldown * (1f + killCommandAdjustedBonus);
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
