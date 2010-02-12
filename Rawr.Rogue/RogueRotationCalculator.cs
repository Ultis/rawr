using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Rogue
{
    public class RogueRotationCalculator
    {
		public Stats Stats { get; set; }
		public float Duration { get; set; }
		public float TempCPPerCPG { get; set; }
		public bool MaintainBleed { get; set; }
        public float MainHandSpeed { get; set; }
        public float OffHandSpeed { get; set; }
        public float ChanceExtraCPPerHit { get; set; }
        public float AvoidedWhiteAttacks { get; set; }
        public float AvoidedAttacks { get; set; }
        public float AvoidedPoisonAttacks { get; set; }

        public RogueAbilityStats MainHandStats { get; set; }
        public RogueAbilityStats OffHandStats { get; set; }
        public RogueAbilityStats BackstabStats { get; set; }
        public RogueAbilityStats HemoStats { get; set; }
        public RogueAbilityStats MutiStats { get; set; }
        public RogueAbilityStats SStrikeStats { get; set; }
        public RogueAbilityStats RuptStats { get; set; }
        public RogueAbilityStats EnvenomStats { get; set; }
        public RogueAbilityStats EvisStats { get; set; }
        public RogueAbilityStats SnDStats { get; set; }
        public RogueAbilityStats IPStats { get; set; }
        public RogueAbilityStats DPStats { get; set; }
        public RogueAbilityStats WPStats { get; set; }
        public RogueAbilityStats APStats { get; set; }

		private float[] _chanceExtraCP = new float[5];

        public RogueRotationCalculator(Stats stats, float duration, float cpPerCPG, bool maintainBleed,
			float mainHandSpeed, float offHandSpeed, float avoidedWhiteAttacks, float avoidedAttacks, float avoidedPoisonAttacks,
			float chanceExtraCPPerHit, float chanceExtraCPPerMutiHit, 
            RogueAbilityStats mainHandStats, RogueAbilityStats offHandStats, RogueAbilityStats backstabStats, RogueAbilityStats hemoStats, RogueAbilityStats sStrikeStats,
            RogueAbilityStats mutiStats, RogueAbilityStats ruptStats, RogueAbilityStats evisStats, RogueAbilityStats envenomStats, RogueAbilityStats snDStats, 
            RogueAbilityStats iPStats, RogueAbilityStats dPStats, RogueAbilityStats wPStats, RogueAbilityStats aPStats)
		{
			Stats = stats;
			Duration = duration;
			TempCPPerCPG = cpPerCPG;
			MaintainBleed = maintainBleed;
            MainHandSpeed = mainHandSpeed;
            OffHandSpeed = offHandSpeed;
            AvoidedWhiteAttacks = avoidedWhiteAttacks;
            AvoidedAttacks = avoidedAttacks;
            AvoidedPoisonAttacks = avoidedPoisonAttacks;
            ChanceExtraCPPerHit = chanceExtraCPPerHit;

            MainHandStats = mainHandStats;
            OffHandStats = offHandStats;
            BackstabStats = backstabStats;
            HemoStats = hemoStats;
            MutiStats = mutiStats;
            SStrikeStats = sStrikeStats;
            RuptStats = ruptStats;
            EnvenomStats = envenomStats;
            EvisStats = evisStats;
            SnDStats = snDStats;
            IPStats = iPStats;
            DPStats = dPStats;
            WPStats = wPStats;
            APStats = aPStats;

			float c = chanceExtraCPPerHit, h = (1f - chanceExtraCPPerHit);
			_chanceExtraCP[0] = c;
			_chanceExtraCP[1] = c*h;
			_chanceExtraCP[2] = c*c+c*h*h;
			_chanceExtraCP[3] = 2*c*c*h+c*h*h*h;
			_chanceExtraCP[4] = c*c*c+3*c*c*h*h+c*h*h*h*h;
		}

        public RogueRotationCalculation GetRotationCalculations(int CPG, bool useRupt, int finisher, int finisherCP, int snDCP, int mHPoison, int oHPoison, bool bleedIsUp, bool useTotT)
		{
            float energyRegen = 10f * (1f + Stats.BonusEnergyRegenMultiplier);
            float totalEnergyAvailable = 100f + Stats.BonusMaxEnergy +
                                         energyRegen * Duration +
                                         (Duration / (180 - Stats.VanishCDReduction)) * 20f * energyRegen * Stats.BonusStealthEnergyRegen +
                                         (useTotT ? (Stats.BonusToTTEnergy > 0 ? Stats.BonusToTTEnergy : (-15f + Stats.ToTTCDReduction)) * (Duration - 5f) / (30f - Stats.ToTTCDReduction) : 0f) +
                                         (useRupt ? 0.02f * (Duration / 2f) * Stats.ReduceEnergyCostFromRupture : 0f) +
                                         energyRegen * 2f * Stats.BonusEnergyRegen * (Duration / 180) -
                                         (Stats.BonusFlurryHaste > 0 ? (25f - Stats.FlurryCostReduction) * Duration / 120f : 0f);
            float totalCPAvailable = 0f;
            float averageGCD = 1f / (1f - AvoidedAttacks);
            float averageFinisherCP = 5f + _chanceExtraCP[4] - Stats.CPOnFinisher;
			
			#region Melee
			float mainHandCount = Duration / MainHandSpeed;
            float offHandCount = Duration / OffHandSpeed;
            totalEnergyAvailable += offHandCount * Stats.ChanceOnEnergyOnOHAttack * AvoidedWhiteAttacks +
                                    Stats.ChanceOnEnergyOnCrit * mainHandCount * MainHandStats.CritChance +
                                    Stats.ChanceOnEnergyOnCrit * offHandCount * OffHandStats.CritChance;
			#endregion

            #region Combo Point Generator
            float cpgCount = 0f;
            float cpgEnergy = CPG == 2 ? BackstabStats.EnergyCost : CPG == 3 ? HemoStats.EnergyCost : CPG == 1 ? SStrikeStats.EnergyCost : MutiStats.EnergyCost;
            float CPPerCPG = TempCPPerCPG +
                             (CPG == 0 ? 1f : 0f) +
                             (CPG == 1 ? Stats.ChanceOnCPOnSSCrit * SStrikeStats.CritChance: 0f);
            #endregion

            #region Slice and Dice
            float averageSnDCP = ((float)snDCP + 1f) * _chanceExtraCP[snDCP - 1]
                + ((float)snDCP) * (1f - _chanceExtraCP[snDCP - 1]);

            //Lose some time due to Roar/Rake, Roar/Mangle, and Roar/Rip conflicts
            //float roarRakeConflict = (1f / RakeStats.DurationAverage) * 0.5f * averageGCD;
            //float roarMangleConflict = (1f / MangleStats.DurationAverage) * 0.5f * averageGCD;
            //float roarRipConflict = (1f / ripDurationAverage) * 0.5f * (averageGCD * averageFinisherCP / CPPerCPG);

            float snDDuration = SnDStats.DurationAverage + 3f * Math.Min(5f, averageSnDCP);
            //    - roarRakeConflict - roarMangleConflict - roarRipConflict;
            float snDCount = Duration / snDDuration;
            snDCount = Math.Max(1f, (finisher == 2 ?  snDCount * (1f - Stats.ChanceOnSnDResetOnEnvenom): snDCount));
            float snDTotalEnergy = snDCount * SnDStats.EnergyCost;
            float snDCPRequired = snDCount * averageSnDCP - (snDCount - 1) * Stats.CPOnFinisher;
            if (totalCPAvailable < snDCPRequired)
            {
                float cpToGenerate = snDCPRequired - totalCPAvailable;
                float cpgToUse = cpToGenerate / CPPerCPG;
                cpgCount += cpgToUse;
                totalEnergyAvailable -= cpgToUse * cpgEnergy;
                totalCPAvailable += cpToGenerate;
            }
            totalCPAvailable -= snDCPRequired;
            totalEnergyAvailable -= snDTotalEnergy - 25 * Stats.ChanceOnEnergyPerCPFinisher * snDCount * averageSnDCP;
            #endregion

            #region Damage Finishers
            float ruptCount = 0f;
            float evisCount = 0f;
            float envenomCount = 0f;
            if (useRupt)
            {
                #region Rupture
                //Lose GCDs at the start of the fight to Mangle/Rake, Roar, and enough CPGs to get 5CPG.
                //float durationRipable = Duration - 2f * averageGCD - (averageGCD * (averageFinisherCP / CPPerCPG));
                float durationRuptable = Duration;

                float ruptCountMax = durationRuptable / RuptStats.DurationAverage;
                float ruptsFromAvailableCP = Math.Min(ruptCountMax, totalCPAvailable / averageFinisherCP);
                ruptCount += ruptsFromAvailableCP;
                totalCPAvailable -= averageFinisherCP * ruptsFromAvailableCP;
                totalEnergyAvailable -= RuptStats.EnergyCost * ruptsFromAvailableCP;

                float ruptCycleEnergy = (averageFinisherCP / CPPerCPG) * cpgEnergy + RuptStats.EnergyCost;
                float ruptsFromNewCP = Math.Min(ruptCountMax - ruptsFromAvailableCP, totalEnergyAvailable / ruptCycleEnergy);

                ruptCount += ruptsFromNewCP;
                cpgCount += (averageFinisherCP / CPPerCPG) * ruptsFromNewCP;
                totalEnergyAvailable -= ruptCycleEnergy * ruptsFromNewCP + 25 * Stats.ChanceOnEnergyPerCPFinisher * ruptCount * averageFinisherCP;
                #endregion
            }
            if (finisher == 1 && finisherCP > 0)
            {
                #region Eviscerate
                float averageEvisCP = ((float)finisherCP + 1f) * _chanceExtraCP[finisherCP - 1]
                + ((float)finisherCP) * (1f - _chanceExtraCP[finisherCP - 1]);
                float evisDamageMultiplier = Math.Min(1f,
                    (EvisStats.DamagePerSwing + EvisStats.DamagePerSwingPerCP * averageEvisCP) /
                    (EvisStats.DamagePerSwing + EvisStats.DamagePerSwingPerCP * 5f));
                float evisFromAvailableCP = totalCPAvailable / averageEvisCP;
                evisCount += evisFromAvailableCP * evisDamageMultiplier;
                totalCPAvailable = 0;
                totalEnergyAvailable -= EvisStats.EnergyCost * evisFromAvailableCP;

                float evisCycleEnergy = (averageEvisCP / CPPerCPG) * cpgEnergy + EvisStats.EnergyCost;
                float evisFromNewCP = totalEnergyAvailable / evisCycleEnergy;

                evisCount += evisFromNewCP * evisDamageMultiplier;
                cpgCount += evisFromNewCP * (averageEvisCP / CPPerCPG);
                totalEnergyAvailable = 0f + 25 * Stats.ChanceOnEnergyPerCPFinisher * evisCount * averageEvisCP;
                #endregion
            }
            else if (finisher == 2 && finisherCP > 0)
            {
                #region Envenom
                float averageEnvenomCP = ((float)finisherCP + 1f) * _chanceExtraCP[finisherCP - 1]
                + ((float)finisherCP) * (1f - _chanceExtraCP[finisherCP - 1]);
                float envenomDamageMultiplier = Math.Min(1f,
                    (EnvenomStats.DamagePerSwing + EnvenomStats.DamagePerSwingPerCP * averageEnvenomCP) /
                    (EnvenomStats.DamagePerSwing + EnvenomStats.DamagePerSwingPerCP * 5f));
                float envenomFromAvailableCP = totalCPAvailable / averageEnvenomCP;
                envenomCount += envenomFromAvailableCP * envenomDamageMultiplier;
                totalCPAvailable = 0;
                totalEnergyAvailable -= EnvenomStats.EnergyCost * envenomFromAvailableCP;

                float envenomCycleEnergy = (averageEnvenomCP / CPPerCPG) * cpgEnergy + EnvenomStats.EnergyCost;
                float envenomFromNewCP = totalEnergyAvailable / envenomCycleEnergy;

                envenomCount += envenomFromNewCP * envenomDamageMultiplier;
                cpgCount += envenomFromNewCP * (averageEnvenomCP / CPPerCPG);
                totalEnergyAvailable = 0f + 25 * Stats.ChanceOnEnergyPerCPFinisher * envenomCount * averageEnvenomCP;
                #endregion
            }
            #endregion

            #region Extra Energy turned into Combo Point Generators
            float backstabCount = 0f;
            float hemoCount = 0f;
            float sStrikeCount = 0f;
            float mutiCount = 0f;
            if (totalEnergyAvailable > 0)
            {
                cpgCount += totalEnergyAvailable / cpgEnergy;
                totalEnergyAvailable = 0f;
            }
            if (CPG == 2) backstabCount += cpgCount;
            else if (CPG == 3) hemoCount += cpgCount;
            else if (CPG == 1) sStrikeCount += cpgCount;
            else if (CPG == 0) mutiCount += cpgCount;
            #endregion

            #region Extra Mainhand attacks from Hack and Slash
            if (MainHandStats.Weapon == ItemType.OneHandAxe || MainHandStats.Weapon == ItemType.OneHandSword) mainHandCount += Stats.ChanceOnMHAttackOnSwordAxeHit * (mainHandCount + backstabCount + hemoCount + sStrikeCount + mutiCount + evisCount + envenomCount + snDCount);
            if (MainHandStats.Weapon == ItemType.OneHandAxe || MainHandStats.Weapon == ItemType.OneHandSword) mainHandCount += Stats.ChanceOnMHAttackOnSwordAxeHit * (offHandCount + mutiCount);
            #endregion

            #region Poisons
            float mHHitCount = mainHandCount + backstabCount + hemoCount + sStrikeCount + mutiCount + evisCount + envenomCount + snDCount;
            float oHHitCount = offHandCount + mutiCount;
            float iPCount = 0f;
            float dPCount = 0f;
            float wPCount = 0f;
            float aPCount = 0f;
            float iPPPS = 8.53f * (1f + Stats.BonusIPFrequencyMultiplier) / 60f;
            float envenomBuffTime = envenomCount * finisherCP + envenomCount;
            #region MainHand Poison
            if (mHPoison == 1)
                iPCount += mHHitCount * MainHandSpeed * iPPPS * ((Duration - envenomBuffTime) / Duration +
                                                                 1.75f * envenomBuffTime / Duration);
            else if (mHPoison == 2)
            {
                float dPCountTemp = mHHitCount * 0.3f * ((Duration - envenomBuffTime) / Duration +
                                                         1.15f * envenomBuffTime / Duration);
                dPCount = dPCountTemp;
                if (oHPoison == 1)
                    iPCount += dPCountTemp - 5;
                else if (oHPoison == 3)
                    wPCount += dPCountTemp - 5;
                else if (oHPoison == 4)
                    aPCount += dPCountTemp - 5;
            }
            else if (mHPoison == 3)
                wPCount += mHHitCount * MainHandSpeed * 21.43f / 60f;
            else if (mHPoison == 4)
                aPCount += mHHitCount * 0.5f;
            #endregion
            #region OffHand Poison
            if (oHPoison == 1)
                iPCount += oHHitCount * OffHandSpeed * iPPPS * ((Duration - envenomBuffTime) / Duration +
                                                                1.75f * envenomBuffTime / Duration);
            else if (oHPoison == 2)
            {
                float dPCountTemp = oHHitCount * 0.3f * ((Duration - envenomBuffTime) / Duration +
                                                         1.15f * envenomBuffTime / Duration);
                dPCount = dPCountTemp;
                if (mHPoison == 1)
                    iPCount += dPCountTemp - 5;
                else if (mHPoison == 3)
                    wPCount += dPCountTemp - 5;
                else if (mHPoison == 4)
                    aPCount += dPCountTemp - 5;
            }
            else if (oHPoison == 3)
                wPCount += oHHitCount * OffHandSpeed * 21.43f / 60f;
            else if (oHPoison == 4)
                aPCount += oHHitCount * 0.5f;
            #endregion
            iPCount *= (1f - AvoidedPoisonAttacks);
            dPCount = (dPCount > 0 ? (Duration - 5) / 3f : 0) * (1f - AvoidedPoisonAttacks);
            wPCount *= (1f - AvoidedPoisonAttacks);
            aPCount *= (1f - AvoidedPoisonAttacks);
            #endregion

            #region Damage Totals
            float HFBMultiplier = (bleedIsUp || ruptCount > 0) ? 1f + Stats.BonusDamageMultiplierHFB: 1f;

            float mainHandDamageTotal = mainHandCount * MainHandStats.DamagePerSwing * HFBMultiplier;
            float offHandDamageTotal = offHandCount * OffHandStats.DamagePerSwing * HFBMultiplier;
            float backstabDamageTotal = backstabCount * BackstabStats.DamagePerSwing * HFBMultiplier;
            float hemoDamageTotal = hemoCount * HemoStats.DamagePerSwing * HFBMultiplier;
            float sStrikeDamageTotal = sStrikeCount * SStrikeStats.DamagePerSwing * HFBMultiplier;
            float mutiDamageTotal = mutiCount * MutiStats.DamagePerSwing * HFBMultiplier;
            float ruptDamageTotal = ruptCount * RuptStats.DamagePerSwing * ((RuptStats.DurationAverage + (2 * RuptStats.DurationPerCP)) / 12f) * HFBMultiplier;
            float evisDamageTotal = evisCount * (EvisStats.DamagePerSwing + EvisStats.DamagePerSwingPerCP * finisherCP) * HFBMultiplier;
            float envenomDamageTotal = envenomCount * (EnvenomStats.DamagePerSwing + EnvenomStats.DamagePerSwingPerCP * finisherCP) * HFBMultiplier;
            float instantPoisonTotal = iPCount * IPStats.DamagePerSwing * HFBMultiplier;
            float deadlyPoisonTotal = dPCount * DPStats.DamagePerSwing * HFBMultiplier;
            float woundPoisonTotal = wPCount * WPStats.DamagePerSwing * HFBMultiplier;
            float anestheticPoisonTotal = aPCount * APStats.DamagePerSwing * HFBMultiplier;

            float damageTotal = mainHandDamageTotal + offHandDamageTotal + backstabDamageTotal + hemoDamageTotal + sStrikeDamageTotal + 
                                  mutiDamageTotal + ruptDamageTotal + evisDamageTotal + envenomDamageTotal + instantPoisonTotal + deadlyPoisonTotal + woundPoisonTotal + anestheticPoisonTotal;
            #endregion

            return new RogueRotationCalculation()
            {
                //Name = rotationName.ToString(),
                DPS = damageTotal / Duration,
                TotalDamage = damageTotal,

                MainHandCount = mainHandCount,
                OffHandCount = offHandCount,
                BackstabCount = backstabCount,
                HemoCount = hemoCount,
                SStrikeCount = sStrikeCount,
                MutiCount = mutiCount,
                RuptCount = ruptCount,
                EvisCount = evisCount,
                EnvenomCount = envenomCount,
                SnDCount = snDCount,
                IPCount = iPCount,
                DPCount = dPCount,
                WPCount = wPCount,
                APCount = aPCount,

                EvisCP = (finisher == 1 ? finisherCP : 0),
                EnvenomCP = (finisher == 2 ? finisherCP : 0),
                SnDCP = snDCP,

                MHPoison = mHPoison,
                OHPoison = oHPoison,

                UseTotT = useTotT,
                CutToTheChase = Stats.ChanceOnSnDResetOnEnvenom,
            };
        }

        public class RogueRotationCalculation
        {
            public float DPS { get; set; }
            public float TotalDamage { get; set; }

            public float MainHandCount { get; set; }
            public float OffHandCount { get; set; }
            public float BackstabCount { get; set; }
            public float HemoCount { get; set; }
            public float SStrikeCount { get; set; }
            public float MutiCount { get; set; }
            public float RuptCount { get; set; }
            public float EvisCount { get; set; }
            public float EnvenomCount { get; set; }
            public float SnDCount { get; set; }
            public float IPCount { get; set; }
            public float DPCount { get; set; }
            public float WPCount { get; set; }
            public float APCount { get; set; }

            public int EvisCP { get; set; }
            public int EnvenomCP { get; set; }
            public int SnDCP { get; set; }

            public int MHPoison { get; set; }
            public int OHPoison { get; set; }

            public float CutToTheChase { get; set; }
            public bool UseTotT { get; set; }

            public override string ToString()
            {
                StringBuilder rotation = new StringBuilder();
                if (BackstabCount > 0) rotation.Append("BS ");
                if (HemoCount > 0) rotation.Append("He ");
                if (SStrikeCount > 0) rotation.Append("SS ");
                if (MutiCount > 0) rotation.Append("Mu ");
                if (RuptCount > 0) rotation.Append("Ru ");
                if (EvisCount > 0) rotation.AppendFormat("Ev{0} ", EvisCP);
                if (EnvenomCount > 0) rotation.AppendFormat("En{0} ", EnvenomCP);
                rotation.Append("SnD" + SnDCP.ToString());

                if (EnvenomCount > 0 && CutToTheChase == 1) rotation.AppendFormat("*Use {0}cp Slice and Dice, kept up with Envenom.\r\n", SnDCP);
                else if (EnvenomCount > 0 && CutToTheChase > 0) rotation.AppendFormat("*Use {0}cp Slice and Dice, partially kept up with Envenom.\r\n", SnDCP);
                else rotation.AppendFormat("*Keep {0}cp Slice and Dice up.\r\n", SnDCP);
                if (RuptCount > 0) rotation.Append("Keep 5cp Rupture up.\r\n");
                if (EvisCount > 0) rotation.AppendFormat("Use {0}cp Eviscerates to spend extra combo points.\r\n", EvisCP);
                if (EnvenomCount > 0) rotation.AppendFormat("Use {0}cp Envenoms to spend extra combo points.\r\n", EnvenomCP);
                if (BackstabCount > 0) rotation.Append("Use Backstab for combo points.\r\n");
                else if (HemoCount > 0) rotation.Append("Use Hemorrhage for combo points.\r\n");
                else if (SStrikeCount > 0) rotation.Append("Use Sinister Strike for combo points.\r\n");
                else rotation.Append("Use Mutilate for combo points.\r\n");
                if (MHPoison == 0) rotation.Append("Use no damage poison on Mainhand.\r\n");
                else if (MHPoison == 1) rotation.Append("Use Instant Poison on Mainhand.\r\n");
                else if (MHPoison == 2) rotation.Append("Use Deadly Poison on Mainhand.\r\n");
                else if (MHPoison == 3) rotation.Append("Use Wound Poison on Mainhand.\r\n");
                if (OHPoison == 0) rotation.Append("Use no damage poison on Offhand.\r\n");
                else if (OHPoison == 1) rotation.Append("Use Instant Poison on Offhand.\r\n");
                else if (OHPoison == 2) rotation.Append("Use Deadly Poison on Offhand.\r\n");
                else if (OHPoison == 3) rotation.Append("Use Wound Poison on Offhand.\r\n");
                if (UseTotT) rotation.Append("Use Tricks of the Trade every cooldown.");

                return rotation.ToString();
            }
        }
    }
}
