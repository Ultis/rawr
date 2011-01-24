using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Rogue
{
    public class RogueRotationCalculator
    {
        public Character Char { get; set; }
        public RogueTalents Talents { get; set; }
        public int Spec { get; set; }
        public Stats Stats { get; set; }
        public CalculationOptionsRogue CalcOpts { get; set; }
        public float Duration { get; set; }
        public float HasteBonus { get; set; }
        public float MainHandSpeed { get; set; }
        public float OffHandSpeed { get; set; }
        public float MainHandSpeedNorm { get; set; }
        public float OffHandSpeedNorm { get; set; }
        public float ChanceExtraCPPerHit { get; set; }
        public float ChanceExtraCPPerMutiHit { get; set; }
        public float AvoidedWhiteMHAttacks { get; set; }
        public float AvoidedWhiteOHAttacks { get; set; }
        public float AvoidedMHAttacks { get; set; }
        public float AvoidedOHAttacks { get; set; }
        public float AvoidedFinisherAttacks { get; set; }
        public float AvoidedPoisonAttacks { get; set; }

        public RogueAbilityStats MainHandStats { get; set; }
        public RogueAbilityStats OffHandStats { get; set; }
        public RogueAbilityStats MainGaucheStats { get; set; }
        public RogueAbilityStats BackstabStats { get; set; }
        public RogueAbilityStats HemoStats { get; set; }
        public RogueAbilityStats MutiStats { get; set; }
        public RogueAbilityStats SStrikeStats { get; set; }
        public RogueAbilityStats RStrikeStats { get; set; }
        public RogueAbilityStats RuptStats { get; set; }
        public RogueAbilityStats EnvenomStats { get; set; }
        public RogueAbilityStats EvisStats { get; set; }
        public RogueAbilityStats SnDStats { get; set; }
        public RogueAbilityStats RecupStats { get; set; }
        public RogueAbilityStats ExposeStats { get; set; }
        public RogueAbilityStats IPStats { get; set; }
        public RogueAbilityStats DPStats { get; set; }
        public RogueAbilityStats WPStats { get; set; }
        public RogueAbilityStats APStats { get; set; }

        public float BonusEnergyRegen { get; set; }
        public float BonusFlurryHaste { get; set; }
        public float BonusHemoDamageMultiplier { get; set; }
        public float IPFrequencyMultiplier { get; set; }
        public float DPFrequencyBonus { get; set; }
        public float BonusMaxEnergy { get; set; }
        public float BonusStealthEnergyRegen { get; set; }
        public float ChanceOnCPOnSSCrit { get; set; }
        public float ChanceOnEnergyOnCrit { get; set; }
        public float ChanceOnEnergyOnGarrRuptTick { get; set; }
        public float ChanceOnEnergyPerCPFinisher { get; set; }
        public float ChanceOnMHAttackOnSwordAxeHit { get; set; }
        public float ChanceOnNoDPConsumeOnEnvenom { get; set; }
        public float ChanceOnMGAttackOnMHAttack { get; set; }
        public float ChanceOnSnDResetOnEvisEnv { get; set; }
        public float CPOnFinisher { get; set; }
        public float KSBonusDamage { get; set; }
        public float EnergyOnBelow35BS { get; set; }
        public float EnergyOnOHAttack { get; set; }
        public float EnergyOnRecupTick { get; set; }
        public float EnergyRegenMultiplier { get; set; }
        public float EnergyRegenTimeOnDamagingCP { get; set; }
        public float ExposeCPCostMult { get; set; }
        public float FlurryCostReduction { get; set; }
        public float ToTTCDReduction { get; set; }
        public float ToTTCostReduction { get; set; }
        public float RSBonus { get; set; }
        public float VanishCDReduction { get; set; }

        private float[] _averageNormalCP = new float[6];
        private float[] _averageSStrikeCP = new float[6];
        private float[] _averageMutiCP = new float[6];
        private float[] _avgMutiNeeded = new float[6];

        public RogueRotationCalculator(Character character, int spec, Stats stats, CalculationOptionsRogue calcOpts, float hasteBonus,
            float mainHandSpeed, float offHandSpeed, float mainHandSpeedNorm, float offHandSpeedNorm, float avoidedWhiteMHAttacks, float avoidedWhiteOHAttacks, float avoidedMHAttacks, float avoidedOHAttacks, float avoidedFinisherAttacks, float avoidedPoisonAttacks,
			float chanceExtraCPPerHit, float chanceExtraCPPerMutiHit,
            RogueAbilityStats mainHandStats, RogueAbilityStats offHandStats, RogueAbilityStats mainGaucheStats, RogueAbilityStats backstabStats, RogueAbilityStats hemoStats, RogueAbilityStats sStrikeStats,
            RogueAbilityStats mutiStats, RogueAbilityStats rStrikeStats, RogueAbilityStats ruptStats, RogueAbilityStats evisStats, RogueAbilityStats envenomStats, RogueAbilityStats snDStats, RogueAbilityStats recupStats, RogueAbilityStats exposeStats,
            RogueAbilityStats iPStats, RogueAbilityStats dPStats, RogueAbilityStats wPStats)
		{
            Char = character;
            Talents = character.RogueTalents;
            Spec = spec;
			Stats = stats;
            CalcOpts = calcOpts;
			Duration = CalcOpts.Duration;
            HasteBonus = hasteBonus;
            MainHandSpeed = mainHandSpeed;
            OffHandSpeed = offHandSpeed;
            MainHandSpeedNorm = mainHandSpeedNorm;
            OffHandSpeedNorm = offHandSpeedNorm;
            AvoidedWhiteMHAttacks = avoidedWhiteMHAttacks;
            AvoidedWhiteOHAttacks = avoidedWhiteOHAttacks;
            AvoidedMHAttacks = avoidedMHAttacks;
            AvoidedOHAttacks = avoidedOHAttacks;
            AvoidedFinisherAttacks = avoidedFinisherAttacks;
            AvoidedPoisonAttacks = avoidedPoisonAttacks;
            ChanceExtraCPPerHit = chanceExtraCPPerHit;
            ChanceExtraCPPerMutiHit = chanceExtraCPPerMutiHit;

            MainHandStats = mainHandStats;
            OffHandStats = offHandStats;
            MainGaucheStats = mainGaucheStats;
            BackstabStats = backstabStats;
            HemoStats = hemoStats;
            MutiStats = mutiStats;
            SStrikeStats = sStrikeStats;
            RStrikeStats = rStrikeStats;
            RuptStats = ruptStats;
            EnvenomStats = envenomStats;
            EvisStats = evisStats;
            SnDStats = snDStats;
            RecupStats = recupStats;
            ExposeStats = exposeStats;
            IPStats = iPStats;
            DPStats = dPStats;
            WPStats = wPStats;

            #region Talent/Mastery bonuses
            BonusMaxEnergy = spec == 0 && (Char.MainHand == null || Char.OffHand == null ? false : Char.MainHand.Type == ItemType.Dagger && Char.MainHand.Type == ItemType.Dagger) ? RV.Mastery.AssassinsResolveEnergyBonus : 0f;
            ChanceOnEnergyOnGarrRuptTick = RV.Talents.VenemousWoundsProcChance * Talents.VenomousWounds;
            ChanceOnNoDPConsumeOnEnvenom = RV.Talents.MasterPoisonerNoDPConsumeChance * Talents.MasterPoisoner;
            ChanceOnMGAttackOnMHAttack = spec == 1 ? RV.Mastery.MainGauche + RV.Mastery.MainGauchePerMast * StatConversion.GetMasteryFromRating(stats.MasteryRating) : 0f;
            ChanceOnSnDResetOnEvisEnv = RV.Talents.CutToTheChaseMult[Talents.CutToTheChase];
            DPFrequencyBonus = spec == 0 ? RV.Mastery.ImprovedPoisonsDPBonus : 0f;
            ExposeCPCostMult = RV.Talents.ImpExposeArmorCPMult * Talents.ImprovedExposeArmor;
            EnergyOnBelow35BS = RV.Talents.MurderousIntentEnergyRefund * Talents.MurderousIntent;
            EnergyRegenTimeOnDamagingCP = (RV.AR.Duration + (Talents.GlyphOfAdrenalineRush ? RV.Glyph.ARDurationBonus : 0f)) / RV.AR.CD * Talents.AdrenalineRush * RV.Talents.RestlessBladesARCDReduc * Talents.RestlessBlades;
            EnergyOnOHAttack = RV.Talents.CombatPotencyProcChance * RV.Talents.CombatPotencyEnergyBonus * Talents.CombatPotency;
            EnergyOnRecupTick = RV.Talents.EnergeticRecoveryEnergyBonus * Talents.EnergeticRecovery;
            EnergyRegenMultiplier = (1f + (spec == 1 ? RV.Mastery.VitalityRegenMult : 0f)) * (1f + (RV.AR.Duration + (Talents.GlyphOfAdrenalineRush ? RV.Glyph.ARDurationBonus : 0f)) / RV.AR.CD * Talents.AdrenalineRush) * (1f + HasteBonus) - 1f;
            IPFrequencyMultiplier = spec == 0 ? RV.Mastery.ImprovedPoisonsIPFreqMult : 0f;
            BonusStealthEnergyRegen = RV.Talents.OverkillRegenMult * Talents.Overkill;
            ChanceOnEnergyPerCPFinisher = RV.Talents.RelentlessStrikesPerCPChance[Talents.RelentlessStrikes];
            CPOnFinisher = RV.Talents.RuthlessnessChance * Talents.Ruthlessness;
            KSBonusDamage = (Talents.GlyphOfKillingSpree ? RV.KS.KSDmgMult : 0f);
            RSBonus = (RV.RS.FinishMult + (Talents.GlyphOfRevealingStrike ? RV.Glyph.RSFinishMultBonus : 0f)) * Talents.RevealingStrike;
            ToTTCostReduction = (Talents.GlyphOfTricksOfTheTrade ? RV.Glyph.TotTCostReduc : 0f);
            VanishCDReduction = RV.Talents.ElusivenessVanishCDReduc * Talents.Elusiveness;
            #endregion

            #region Probability tables
            float c = ChanceExtraCPPerHit, h = (1f - c), f = CPOnFinisher, nf = (1f - f);
            _averageNormalCP[1] = 1 * (f + nf * h) + 2 * (nf * c);
            _averageNormalCP[2] = 2 * (f * h + nf * c + nf * h * h) + 3 * (f * c);
            _averageNormalCP[3] = 3 * (f * c + f * h * h + 2 * nf * c * h + nf * h * h * h) + 4 * (f * h * c + nf * c * c + nf * h * h * c);
            _averageNormalCP[4] = 4 * (2 * f * c * h + f * h * h * h + nf * c * c + 3 * nf * c * h * h + nf * h * h * h * h) + 5 * (f * c * c + f * h * h * c + 2 * nf * c * h * c + nf * h * h * h * c);
            _averageNormalCP[5] = 5 * (f * c * c + 3 * f * c * h * h + f * h * h * h * h + 3 * nf * c * c * h + 4 * nf * c * h * h * h + nf * h * h * h * h * h) + 6 * (2 * f * c * h * c + f * h * h * h * c + nf * c * c * c + 3 * nf * c * h * h * c + nf * h * h * h * h * c);
            
            c = ChanceExtraCPPerHit + ChanceOnCPOnSSCrit * SStrikeStats.CritChance; h = (1f - c);
            _averageSStrikeCP[1] = 1 * (f + nf * h) + 2 * (nf * c);
            _averageSStrikeCP[2] = 2 * (f * h + nf * c + nf * h * h) + 3 * (f * c + nf * h * c);
            _averageSStrikeCP[3] = 3 * (f * c + f * h * h + 2 * nf * c * h + nf * h * h * h) + 4 * (f * h * c + nf * c * c + nf * h * h * c);
            _averageSStrikeCP[4] = 4 * (2 * f * c * h + f * h * h * h + nf * c * c + 3 * nf * c * h * h + nf * h * h * h * h) + 5 * (f * c * c + f * h * h * c + 2 * nf * c * h * c + nf * h * h * h * c);
            _averageSStrikeCP[5] = 5 * (f * c * c + 3 * f * c * h * h + f * h * h * h * h + 3 * nf * c * c * h + 4 * nf * c * h * h * h + nf * h * h * h * h * h) + 6 * (2 * f * c * h * c + f * h * h * h * c + nf * c * c * c + 3 * nf * c * h * h * c + nf * h * h * h * h * c);
            
            c = ChanceExtraCPPerMutiHit; h = (1f - c);
            _averageMutiCP[1] = 1 * (f) + 2 * (nf * h) + 3 * (nf * c);
            _averageMutiCP[2] = 2 * (nf * h) + 3 * (f * h + nf * c) + 4 * (f * c);
            _averageMutiCP[3] = 3 * (f * h + nf * c) + 4 * (f * c + nf * h * h) + 5 * (nf * h * c);
            _averageMutiCP[4] = 4 * (f * c + nf * h * h) + 5 * (f * h * h + 2 * nf * h * c) + 5 * (f * h * c + nf * c * c);
            _averageMutiCP[5] = 5 * (f * h * h + 2 * nf * h * c) + 6 * (2 * f * h * c + nf * h * h * h + nf * c * c) + 7 * (f * c * c + nf * h * h * c);

            _avgMutiNeeded[1] = 1 * (nf);
            _avgMutiNeeded[2] = 1 * (f * h + f * c + nf * h + nf * c);
            _avgMutiNeeded[3] = 1 * (f * h + f * c + nf * c) + 2 * (nf * h);
            _avgMutiNeeded[4] = 1 * (f * c) + 2 * (f * h * h + f * h * c + nf * h * h + nf * h * c + nf * c);
            _avgMutiNeeded[5] = 2 * (f + nf * h * c + nf * c) + 3 * (nf * h * h);
            #endregion
        }

        public RogueRotationCalculation GetRotationCalculations(float durationMultiplier, int CPG, int recupCP, int ruptCP, bool useRS, int finisher, int finisherCP, int snDCP, int mHPoison, int oHPoison, bool bleedIsUp, bool useTotT, bool useEA, bool PTRMode)
		{
            float duration = Duration * durationMultiplier;
            float numberOfStealths = 1f + duration / (RV.Vanish.CD - VanishCDReduction);
            float energyRegen = RV.BaseEnergyRegen * (1f + EnergyRegenMultiplier);
            float totalEnergyAvailable = RV.BaseEnergy + BonusMaxEnergy +
                                         energyRegen * duration +
                                         (recupCP > 0 ? duration / 3 * EnergyOnRecupTick : 0f) + 
                                         numberOfStealths * RV.Talents.OverkillRegenDuration * energyRegen * (1f + BonusStealthEnergyRegen) +
                                         (useTotT ? (Stats.BonusToTTEnergy > 0 ? Stats.BonusToTTEnergy : (-15f + ToTTCostReduction)) * (duration - 5f) / (30f - ToTTCDReduction) : 0f) +
                                         (ruptCP > 0 ? 0.02f * (duration / 2f) * Stats.ReduceEnergyCostFromRupture : 0f) +
                                         RV.ColdBlood.EnergyBonus * Talents.ColdBlood * duration / RV.ColdBlood.CD +
                                         energyRegen * 2f * BonusEnergyRegen * (duration / 180f) -
                                         (BonusFlurryHaste > 0 ? (25f - FlurryCostReduction) * duration / 120f : 0f);
            float averageGCD = 1f / (1f - AvoidedMHAttacks);
            float averageFinisherGCD = 1f / (1f - AvoidedFinisherAttacks);
            float ruptDurationAverage = RuptStats.DurationAverage;
            float snDDurationAverage = SnDStats.DurationAverage;
            float[] _averageCP = CPG == 0 ? _averageMutiCP : CPG == 1 ? _averageSStrikeCP : _averageNormalCP;
            float averageFinisherCP = _averageCP[5];
			
			#region Melee
			float whiteMHAttacks = duration / MainHandSpeed + 0.5f * 0.5f * Stats.MoteOfAnger * duration;
            float whiteOHAttacks = duration / OffHandSpeed + 0.5f * 0.5f * Stats.MoteOfAnger * duration;
            float mGAttacks = ChanceOnMGAttackOnMHAttack * whiteMHAttacks;
            totalEnergyAvailable += whiteOHAttacks * (1f - AvoidedWhiteOHAttacks) * EnergyOnOHAttack +
                                    ChanceOnEnergyOnCrit * whiteMHAttacks * MainHandStats.CritChance +
                                    ChanceOnEnergyOnCrit * whiteOHAttacks * OffHandStats.CritChance;
			#endregion

            #region Combo Point Generator
            float cpgCount = 0f;
            float cpgEnergy = CPG == 2 ? (durationMultiplier <= 0.35f ? BackstabStats.EnergyCost - EnergyOnBelow35BS : BackstabStats.EnergyCost) : CPG == 3 ? HemoStats.EnergyCost : CPG == 1 ? SStrikeStats.EnergyCost : MutiStats.EnergyCost;
            float CPPerCPG = CPG == 2 ? BackstabStats.CPPerSwing : CPG == 3 ? HemoStats.CPPerSwing : CPG == 1 ? SStrikeStats.CPPerSwing : MutiStats.CPPerSwing;
            #endregion

            #region Slice and Dice
            float averageSnDCP = _averageCP[snDCP];

            //Lose some time due to SnD/Rupt conflicts
            float snDRuptConflict = (1f / ruptDurationAverage) * 0.5f * (averageGCD * averageFinisherCP / CPPerCPG);

            float snDDuration = SnDStats.DurationAverage + SnDStats.DurationPerCP * Math.Min(5f, averageSnDCP)
                                - snDRuptConflict;
            float snDCount = duration / snDDuration;
            snDCount = Math.Max(1f, (finisher == 1 || finisher == 2 ? snDCount * (1f - ChanceOnSnDResetOnEvisEnv) : snDCount));
            float snDTotalEnergy = snDCount * (SnDStats.EnergyCost - 25f * ChanceOnEnergyPerCPFinisher * Math.Min(5f, averageSnDCP));
            float snDCPRequired = snDCount * (averageSnDCP - CPOnFinisher);
            float cpgToUse = snDCPRequired / CPPerCPG;
            cpgCount += cpgToUse;
            totalEnergyAvailable -= cpgToUse * cpgEnergy + snDTotalEnergy;
            #endregion

            #region Recuperate
            float recupCount = 0f;
            if (recupCP > 0)
            {
                float averageRecupCP = _averageCP[recupCP];

                //Lose some time due to SnD/Rupt conflicts ???Add more
                float recupRuptConflict = (1f / ruptDurationAverage) * 0.5f * (averageGCD * averageFinisherCP / CPPerCPG);

                float recupDuration = RecupStats.DurationAverage + RecupStats.DurationPerCP * Math.Min(5f, averageRecupCP)
                                    - recupRuptConflict;
                recupCount = duration / recupDuration;
                float recupTotalEnergy = recupCount * (RecupStats.EnergyCost - 25f * ChanceOnEnergyPerCPFinisher * Math.Min(5f, averageRecupCP));
                float recupCPRequired = recupCount * (averageRecupCP - CPOnFinisher);
                cpgToUse = recupCPRequired / CPPerCPG;
                cpgCount += cpgToUse;
                totalEnergyAvailable -= cpgToUse * cpgEnergy + recupTotalEnergy;
            }
            #endregion

            #region Expose Armor
            float rSCount = 0f;
            float eACount = 0f;
            if (useEA)
            {
                float averageEACP = _averageCP[5] * (1f - ExposeCPCostMult);

                //Lose GCDs at the start of the fight to get SnD and enough CPGs to get 5CPG.
                float durationEAable = duration - 2f * averageGCD - (averageGCD * (averageFinisherCP / CPPerCPG));

                //Lose some time due to EA/SnD/Rupt conflicts
                float eARuptConflict = (1f / ruptDurationAverage) * 0.5f * (averageGCD * averageFinisherCP / CPPerCPG);
                float eASnDConflict = (1f / snDDurationAverage) * 0.5f * (averageGCD * averageSnDCP / CPPerCPG);

                float eADuration = ExposeStats.DurationAverage + ExposeStats.DurationPerCP * 5f
                                    - eARuptConflict - eASnDConflict;
                eACount = duration / (eADuration * (1f + RSBonus));
                float eATotalEnergy = eACount * (ExposeStats.EnergyCost - 25f * ChanceOnEnergyPerCPFinisher * 5f);
                float eACPRequired = eACount * (Math.Max(0f, averageEACP - CPOnFinisher) - (useRS ? RStrikeStats.CPPerSwing : 0f));
                cpgToUse = eACPRequired / CPPerCPG;
                cpgCount += cpgToUse;
                rSCount += useRS ? eACount : 0f;
                totalEnergyAvailable -= cpgToUse * cpgEnergy + eATotalEnergy + (useRS ? eACount * RStrikeStats.EnergyCost : 0f);
            }
            #endregion

            #region Damage Finishers
            float ruptCount = 0f;
            float evisCount = 0f;
            float envenomCount = 0f;
            if (ruptCP > 0)
            {
                #region Rupture
                float averageRuptCP = _averageCP[ruptCP];
                //Lose GCDs at the start of the fight to get SnD and if applicable EA up and enough CPGs to get the needed CPs.
                float durationRuptable = duration - 2f * averageGCD - (averageGCD * (averageRuptCP / CPPerCPG)) - (useEA ? averageGCD + (averageGCD * (averageFinisherCP / CPPerCPG)) : 0f);
                float ruptCountMax = durationRuptable / (RuptStats.DurationAverage + ruptCP * RV.Rupt.DurationPerCP);
                float ruptCycleEnergy = ((averageRuptCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG) * cpgEnergy + RuptStats.EnergyCost + (useRS ? RStrikeStats.EnergyCost : 0f) - RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher * averageRuptCP - averageRuptCP * EnergyRegenTimeOnDamagingCP * energyRegen;
                ruptCount = Math.Min(ruptCountMax, totalEnergyAvailable / ruptCycleEnergy);
                cpgCount += ((averageRuptCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG) * ruptCount;
                rSCount += useRS ? ruptCount : 0f;
                totalEnergyAvailable -= ruptCycleEnergy * ruptCount - ChanceOnEnergyOnGarrRuptTick * RV.Talents.VenemousWoundsEnergy * durationRuptable / 2f - averageRuptCP * EnergyRegenTimeOnDamagingCP * energyRegen;
                #endregion
            }
            if (finisher == 1 && finisherCP > 0)
            {
                #region Eviscerate
                float averageEvisCP = _averageCP[finisherCP];
                float evisCycleEnergy = ((averageEvisCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG) * cpgEnergy + EvisStats.EnergyCost + (useRS ? RStrikeStats.EnergyCost : 0f) - 25f * ChanceOnEnergyPerCPFinisher * averageEvisCP - averageFinisherCP * EnergyRegenTimeOnDamagingCP * energyRegen;
                evisCount = totalEnergyAvailable / evisCycleEnergy;
                cpgCount += evisCount * ((averageEvisCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG);
                rSCount += useRS ? evisCount : 0f;
                totalEnergyAvailable = 0f;
                #endregion
            }
            else if (finisher == 2 && finisherCP > 0)
            {
                #region Envenom
                float averageEnvenomCP = _averageCP[finisherCP];
                float envenomCycleEnergy = ((averageEnvenomCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG) * cpgEnergy + EnvenomStats.EnergyCost + (useRS ? RStrikeStats.EnergyCost : 0f) - 25f * ChanceOnEnergyPerCPFinisher * averageEnvenomCP - averageFinisherCP * EnergyRegenTimeOnDamagingCP * energyRegen;
                envenomCount = totalEnergyAvailable / envenomCycleEnergy;
                cpgCount += envenomCount * ((averageEnvenomCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG);
                rSCount += useRS ? envenomCount : 0f;
                totalEnergyAvailable = 0f;
                #endregion
            }
            #endregion

            #region Heartpierce proc
            float HP264PPS = 0;
            float HP277PPS = 0;
            float HP264TPS = 0;
            float HP277TPS = 0;
            if (MainHandStats.Weapon.Name == "Heartpierce")
                if (MainHandStats.Weapon.ItemLevel == 264)
                    HP264PPS += MainHandSpeedNorm / 60 * (whiteMHAttacks * (1f - AvoidedWhiteMHAttacks) + cpgCount + evisCount + envenomCount) / duration;
                else if (MainHandStats.Weapon.ItemLevel == 277)
                    HP277PPS += MainHandSpeedNorm / 60 * (whiteMHAttacks * (1f - AvoidedWhiteMHAttacks) + cpgCount + evisCount + envenomCount) / duration;
            if (OffHandStats.Weapon.Name == "Heartpierce")
                if (OffHandStats.Weapon.ItemLevel == 264)
                    HP264PPS += OffHandSpeedNorm / 60 * (whiteOHAttacks * (1f - AvoidedWhiteOHAttacks) + (cpgCount == 0 ? cpgCount : 0)) / duration;
                else if (OffHandStats.Weapon.ItemLevel == 277)
                    HP277PPS += OffHandSpeedNorm / 60 * (whiteOHAttacks * (1f - AvoidedWhiteOHAttacks) + (cpgCount == 0 ? cpgCount : 0)) / duration;
            if (HP264PPS > 0)
            {
                float ChanceVar264 = (float)Math.Exp(-2 * HP264PPS);
                HP264TPS = HP264PPS * ChanceVar264 * (1 - (float)Math.Pow(ChanceVar264, 5)) / (1 - ChanceVar264);
            }
            if (HP277PPS > 0)
            {
                float ChanceVar277 = (float)Math.Exp(-2 * HP277PPS);
                HP277TPS = HP277PPS * ChanceVar277 * (1 - (float)Math.Pow(ChanceVar277, 6)) / (1 - ChanceVar277);
            }
            totalEnergyAvailable += (HP264TPS + HP277TPS) * duration * 4;
            #endregion

            #region Extra Energy turned into Combo Point Generators
            if (totalEnergyAvailable > 0)
            {
                cpgCount += totalEnergyAvailable / cpgEnergy;
                totalEnergyAvailable = 0f;
            }
            #endregion

            #region Extra Mainhand attacks from Hack and Slash
            if (MainHandStats.Weapon._type == ItemType.OneHandAxe || MainHandStats.Weapon._type == ItemType.OneHandSword)
            {
                whiteMHAttacks += ChanceOnMHAttackOnSwordAxeHit * (whiteMHAttacks * (1f - AvoidedWhiteMHAttacks) + cpgCount + evisCount + envenomCount);
            }
            if (OffHandStats.Weapon._type == ItemType.OneHandAxe || OffHandStats.Weapon._type == ItemType.OneHandSword)
            {
                whiteMHAttacks += ChanceOnMHAttackOnSwordAxeHit * (whiteOHAttacks * (1f - AvoidedWhiteOHAttacks) + (cpgCount == 0 ? cpgCount : 0));
            }
            #endregion

            #region Poisons
            float mHHitCount = whiteMHAttacks * (1f - AvoidedWhiteMHAttacks) + cpgCount + evisCount + envenomCount + snDCount;
            float oHHitCount = whiteOHAttacks * (1f - AvoidedWhiteOHAttacks) + (cpgCount == 0 ? cpgCount : 0);
            float iPCount = 0f;
            float dPCount = 0f;
            float wPCount = 0f;
            float aPCount = 0f;
            float iPProcRate = 0.2f * (1f + IPFrequencyMultiplier) / 1.4f;
            float dPApplyChance = 0.3f + DPFrequencyBonus;
            float envenomBuffTime = envenomCount * finisherCP + envenomCount;
            #region MainHand Poison
            if (mHPoison == 1)
                iPCount += mHHitCount * MainHandStats.Weapon._speed * iPProcRate * ((duration - envenomBuffTime) / duration +
                                                                 1.75f * envenomBuffTime / duration);
            else if (mHPoison == 2 && oHPoison != 2)
            {
                float dPCountTemp = mHHitCount * dPApplyChance * (1f - AvoidedPoisonAttacks) *
                                    ((duration - envenomBuffTime) / duration + 1.15f * envenomBuffTime / duration);
                dPCountTemp -= (1f - ChanceOnNoDPConsumeOnEnvenom) * envenomCount * 5 + 5;
                float dPStackTime = 5f / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteMHAttacks)) * MainHandSpeed;
                dPCount = (duration - dPStackTime - (1f - ChanceOnNoDPConsumeOnEnvenom) * envenomCount * dPStackTime) / 3 * 5 +
                          10 + (1f - ChanceOnNoDPConsumeOnEnvenom) * envenomCount * 10;
                if (oHPoison == 1)
                    iPCount += dPCountTemp;
                else if (oHPoison == 3)
                    wPCount += dPCountTemp;
                else if (oHPoison == 4)
                    aPCount += dPCountTemp;
            }
            else if (mHPoison == 3)
                wPCount += mHHitCount * MainHandStats.Weapon._speed * 21.43f / 60f;
            else if (mHPoison == 4)
                aPCount += mHHitCount * 0.5f;
            #endregion
            #region OffHand Poison
            if (oHPoison == 1)
                iPCount += oHHitCount * OffHandStats.Weapon._speed * iPProcRate * ((duration - envenomBuffTime) / duration +
                                                                1.75f * envenomBuffTime / duration);
            else if (oHPoison == 2 && mHPoison != 2)
            {
                float dPCountTemp = oHHitCount * dPApplyChance * (1f - AvoidedPoisonAttacks) *
                                    ((duration - envenomBuffTime) / duration + 1.15f * envenomBuffTime / duration);
                dPCountTemp -= (1f - ChanceOnNoDPConsumeOnEnvenom) * envenomCount * 10 + 10;
                float dPStackTime = 5f / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteOHAttacks)) * OffHandSpeed;
                dPCount = (duration - dPStackTime - (1f - ChanceOnNoDPConsumeOnEnvenom) * envenomCount * dPStackTime) / 3 * 5 +
                          10 + (1f - ChanceOnNoDPConsumeOnEnvenom) * envenomCount * 10;
                if (mHPoison == 1)
                    iPCount += dPCountTemp;
                else if (mHPoison == 3)
                    wPCount += dPCountTemp;
                else if (mHPoison == 4)
                    aPCount += dPCountTemp;
            }
            else if (oHPoison == 3)
                wPCount += oHHitCount * OffHandStats.Weapon._speed * 21.43f / 60f;
            else if (oHPoison == 4)
                aPCount += oHHitCount * 0.5f;
            #endregion
            iPCount *= (1f - AvoidedPoisonAttacks);
            wPCount *= (1f - AvoidedPoisonAttacks);
            aPCount *= (1f - AvoidedPoisonAttacks);
            #endregion

            #region Killing Spree & Adrenaline Rush
            float kSAttacks = 0;
            float kSDuration = 0;
            float kSDmgBonus = 0.2f + KSBonusDamage;
            float restlessBladesBonus = averageFinisherCP * ruptCount + _averageCP[finisherCP] * (evisCount + envenomCount) * Talents.RestlessBlades;
            if (Talents.KillingSpree > 0)
            {
                float kSCount = (duration + restlessBladesBonus) / 120f;
                kSDuration = kSCount * 2.5f;
                kSAttacks = 5f * kSCount;
            }
            if (Talents.AdrenalineRush > 0)
            {
                whiteMHAttacks *= 1f + 0.2f * (15f + (Talents.GlyphOfAdrenalineRush ? 5f : 0f)) * (duration + restlessBladesBonus) / 180f / duration;
                whiteOHAttacks *= 1f + 0.2f * (15f + (Talents.GlyphOfAdrenalineRush ? 5f : 0f)) * (duration + restlessBladesBonus) / 180f / duration;
            }
            #endregion

            #region Damage Totals
            float mainHandDamageTotal = whiteMHAttacks * MainHandStats.DamagePerSwing +
                                        kSAttacks * MainHandStats.DamagePerSwing * (1f + kSDmgBonus) / (1f + kSDmgBonus * kSDuration / duration);
            float offHandDamageTotal = whiteOHAttacks * OffHandStats.DamagePerSwing +
                                        kSAttacks * OffHandStats.DamagePerSwing * (1f + kSDmgBonus) / (1f + kSDmgBonus * kSDuration / duration);
            float mainGaucheDamageTotal = mGAttacks * MainGaucheStats.DamagePerSwing;
            float backstabDamageTotal = (CPG == 2 ? cpgCount : 0) * BackstabStats.DamagePerSwing;
            float hemoDamageTotal = (CPG == 3 ? cpgCount : 0) * HemoStats.DamagePerSwing;
            float sStrikeDamageTotal = (CPG == 1 ? cpgCount : 0) * SStrikeStats.DamagePerSwing;
            float mutiDamageTotal = (CPG == 0 ? cpgCount : 0) * MutiStats.DamagePerSwing;
            float rStrikeDamageTotal = rSCount * RStrikeStats.DamagePerSwing;
            float ruptDamageTotal = ruptCount * RuptStats.DamagePerSwing * (RuptStats.DurationUptime / 16f) * (useRS ? (1f + RSBonus) : 1f);
            float evisDamageTotal = evisCount * (EvisStats.DamagePerSwing + EvisStats.DamagePerSwingPerCP * Math.Min(_averageCP[finisherCP], 5)) * (useRS ? (1f + RSBonus) : 1f);
            float envenomDamageTotal = envenomCount * (EnvenomStats.DamagePerSwing + EnvenomStats.DamagePerSwingPerCP * Math.Min(_averageCP[finisherCP], 5)) * (useRS ? (1f + RSBonus) : 1f);
            float instantPoisonTotal = iPCount * IPStats.DamagePerSwing;
            float deadlyPoisonTotal = dPCount * DPStats.DamagePerSwing;
            float woundPoisonTotal = wPCount * WPStats.DamagePerSwing;
            
            float damageTotal = (mainHandDamageTotal + offHandDamageTotal + backstabDamageTotal + hemoDamageTotal + sStrikeDamageTotal + mutiDamageTotal +
                                  rStrikeDamageTotal + ruptDamageTotal + evisDamageTotal + envenomDamageTotal + instantPoisonTotal + deadlyPoisonTotal + woundPoisonTotal) * (1f + kSDmgBonus * kSDuration / duration);
            if (Talents.BanditsGuile > 0)
            {
                float buildupTime = duration / (((CPG == 1 ? cpgCount : 0) + rSCount) * (Talents.BanditsGuile == 3 ? 1f : Talents.BanditsGuile * 0.33f));
                float guileBonus = 0.05f / buildupTime + 0.1f / buildupTime + 0.15f / 15f;
                damageTotal *= 1f + guileBonus;
            }
            if (Spec == 2) //Master of Subtlety specialization
            {
                damageTotal *= 1f + 0.1f * numberOfStealths * 6f / duration;
            }
            #endregion

            return new RogueRotationCalculation()
            {
                MultipleSegments = durationMultiplier < 1f,
                Duration = duration,
                TotalDamage = damageTotal,
                DPS = damageTotal / duration,

                MainHandCount = whiteMHAttacks,
                OffHandCount = whiteOHAttacks,
                MGCount = mGAttacks,
                BackstabCount = (CPG == 2 ? cpgCount : 0),
                HemoCount = (CPG == 3 ? cpgCount : 0),
                SStrikeCount = (CPG == 1 ? cpgCount : 0),
                MutiCount = (CPG == 0 ? cpgCount : 0),
                RStrikeCount = rSCount,
                RuptCount = ruptCount,
                EvisCount = evisCount,
                EnvenomCount = envenomCount,
                SnDCount = snDCount,
                RecupCount = recupCount,
                EACount = eACount,
                IPCount = iPCount,
                DPCount = dPCount,
                WPCount = wPCount,
                APCount = aPCount,

                FinisherCP = finisherCP,
                EvisCP = (finisher == 1 ? Math.Min(_averageCP[finisherCP], 5) : 0),
                EnvenomCP = (finisher == 2 ? Math.Min(_averageCP[finisherCP], 5) : 0),
                RuptCP = ruptCP,
                SnDCP = snDCP,

                MHPoison = mHPoison,
                OHPoison = oHPoison,

                UseTotT = useTotT,
                CutToTheChase = ChanceOnSnDResetOnEvisEnv,
            };
        }

        public class RogueRotationCalculation
        {
            public bool MultipleSegments { get; set; }
            public float Duration { get; set; }
            public float TotalDamage { get; set; }
            public float DPS { get; set; }

            public float MainHandCount { get; set; }
            public float OffHandCount { get; set; }
            public float MGCount { get; set; }
            public float BackstabCount { get; set; }
            public float HemoCount { get; set; }
            public float SStrikeCount { get; set; }
            public float MutiCount { get; set; }
            public float RStrikeCount { get; set; }
            public float RuptCount { get; set; }
            public float EvisCount { get; set; }
            public float EnvenomCount { get; set; }
            public float SnDCount { get; set; }
            public float RecupCount { get; set; }
            public float EACount { get; set; }
            public float IPCount { get; set; }
            public float DPCount { get; set; }
            public float WPCount { get; set; }
            public float APCount { get; set; }

            public float FinisherCP { get; set; }
            public float EvisCP { get; set; }
            public float EnvenomCP { get; set; }
            public float RuptCP { get; set; }
            public int SnDCP { get; set; }

            public int MHPoison { get; set; }
            public int OHPoison { get; set; }

            public bool UseTotT { get; set; }
            public float CutToTheChase { get; set; }

            public String RotationString { get; set; }

            public override string ToString()
            {
                if (RotationString != null) return RotationString;

                StringBuilder rotation = new StringBuilder();
                if (BackstabCount > 0) rotation.Append("BS ");
                if (HemoCount > 0) rotation.Append("He ");
                if (SStrikeCount > 0) rotation.Append("SS ");
                if (MutiCount > 0) rotation.Append("Mu ");
                if (RStrikeCount > 0) rotation.Append("RS ");
                if (RuptCount > 0) rotation.Append("Ru ");
                if (EvisCount > 0) rotation.AppendFormat("Ev{0} ", FinisherCP);
                if (EnvenomCount > 0) rotation.AppendFormat("En{0} ", FinisherCP);
                rotation.Append("SnD" + SnDCP.ToString());

                if (!MultipleSegments) rotation.Append("*");
                else rotation.Append("\r\n");

                if (RecupCount > 0) rotation.Append("Keep 5cp Recuperate up.\r\n");
                if (EACount > 0 && RStrikeCount == 0) rotation.Append("Keep 5cp Expose Armor up.\r\n");
                else if (EACount > 0 && RStrikeCount > 0) rotation.Append("Keep 5cp Expose Armor up, lengthened with Revealing Strike.\r\n");
                if (EnvenomCount > 0 && CutToTheChase == 1) rotation.AppendFormat("Use {0}cp Slice and Dice, kept up with Envenom.\r\n", SnDCP);
                else if (EnvenomCount > 0 && CutToTheChase > 0) rotation.AppendFormat("Use {0}cp Slice and Dice, partially kept up with Envenom.\r\n", SnDCP);
                else if (EvisCount > 0 && CutToTheChase == 1) rotation.AppendFormat("Use {0}cp Slice and Dice, kept up with Eviscerate.\r\n", SnDCP);
                else if (EvisCount > 0 && CutToTheChase > 0) rotation.AppendFormat("Use {0}cp Slice and Dice, partially kept up with Eviscerate.\r\n", SnDCP);
                else rotation.AppendFormat("Keep {0}cp Slice and Dice up.\r\n", SnDCP);
                if (RuptCount > 0 && RStrikeCount == 0) rotation.Append("Keep 5cp Rupture up.\r\n");
                else if (RuptCount > 0 && RStrikeCount > 0) rotation.Append("Keep 5cp Rupture up, empowered with Revealing Strike.\r\n");
                if (EvisCount > 0 && RStrikeCount == 0) rotation.AppendFormat("Use {0}cp Eviscerates to spend extra combo points.\r\n", FinisherCP);
                else if (EvisCount > 0 && RStrikeCount > 0) rotation.AppendFormat("Use {0}cp Eviscerates to spend extra combo points, empowered with Revealing Strike.\r\n", FinisherCP);
                if (EnvenomCount > 0 && RStrikeCount == 0) rotation.AppendFormat("Use {0}cp Envenoms to spend extra combo points.\r\n", FinisherCP);
                else if (EnvenomCount > 0 && RStrikeCount > 0) rotation.AppendFormat("Use {0}cp Envenoms to spend extra combo points, empowered with Revealing Strike.\r\n", FinisherCP);
                if (BackstabCount > 0) rotation.Append("Use Backstab for combo points.\r\n");
                else if (HemoCount > 0) rotation.Append("Use Hemorrhage for combo points.\r\n");
                else if (SStrikeCount > 0) rotation.Append("Use Sinister Strike for combo points.\r\n");
                else if (MutiCount > 0) rotation.Append("Use Mutilate for combo points.\r\n");
                else rotation.Append("Error: no CPG used, please create an issue on rawr.codeplex.com including your char file.\r\n");
                if (MHPoison == 1) rotation.Append("Use Instant Poison on Mainhand.\r\n");
                else if (MHPoison == 2) rotation.Append("Use Deadly Poison on Mainhand.\r\n");
                else if (MHPoison == 3) rotation.Append("Use Wound Poison on Mainhand.\r\n");
                if (OHPoison == 1) rotation.Append("Use Instant Poison on Offhand.");
                else if (OHPoison == 2) rotation.Append("Use Deadly Poison on Offhand.");
                else if (OHPoison == 3) rotation.Append("Use Wound Poison on Offhand.");
                if (UseTotT) rotation.Append("\r\nUse Tricks of the Trade every cooldown.");

                return rotation.ToString();
            }

            public static RogueRotationCalculation operator +(RogueRotationCalculation a, RogueRotationCalculation b)
            {
                RogueRotationCalculation c = new RogueRotationCalculation();

                c.DPS = (a.TotalDamage + b.TotalDamage) / (a.Duration + b.Duration);
                c.TotalDamage = a.TotalDamage + b.TotalDamage;

                c.MainHandCount = a.MainHandCount + b.MainHandCount;
                c.OffHandCount = a.OffHandCount + b.OffHandCount;
                c.BackstabCount = a.BackstabCount + b.BackstabCount;
                c.HemoCount = a.HemoCount + b.HemoCount;
                c.SStrikeCount = a.SStrikeCount + b.SStrikeCount;
                c.MutiCount = a.MutiCount + b.MutiCount;
                c.RuptCount = a.RuptCount + b.RuptCount;
                c.EvisCount = a.EvisCount + b.EvisCount;
                c.EnvenomCount = a.EnvenomCount + b.EnvenomCount;
                c.SnDCount = a.SnDCount + b.SnDCount;
                c.RecupCount = a.RecupCount + b.RecupCount;
                c.IPCount = a.IPCount + b.IPCount;
                c.DPCount = a.DPCount + b.DPCount;
                c.WPCount = a.WPCount + b.WPCount;
                c.APCount = a.APCount + b.APCount;

                c.RuptCP = a.RuptCP;
                c.EvisCP = a.EvisCP;
                c.EnvenomCP = a.EnvenomCP;

                c.RotationString = string.Format("Multiple rotations*Before 35%:\r\n{0}\r\nAfter 35%\r\n{1}", a.ToString(), b.ToString());

                return c;
            }

        }
    }
}
