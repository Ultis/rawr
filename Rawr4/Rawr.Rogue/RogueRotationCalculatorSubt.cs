using System;

namespace Rawr.Rogue
{
    public class RogueRotationCalculatorSubt : RogueRotationCalculator
    {
        public RogueAbilityStats BackstabStats { get; set; }
        public RogueAbilityStats HemoStats { get; set; }
        public RogueAbilityStats MutiStats { get; set; }
        public RogueAbilityStats SStrikeStats { get; set; }
        public RogueAbilityStats RStrikeStats { get; set; }
        public RogueAbilityStats EnvenomStats { get; set; }
        public RogueAbilityStats EvisStats { get; set; }
        public RogueAbilityStats RecupStats { get; set; }
        
        public float RecupCP { get; set; }
        private float[] _averageNormalCP = new float[6];

        public RogueRotationCalculatorSubt(Character character, Stats stats, CalculationOptionsRogue calcOpts, float hasteBonus, float mainHandSpeed, float offHandSpeed, float mainHandSpeedNorm,
            float offHandSpeedNorm, float avoidedWhiteMHAttacks, float avoidedWhiteOHAttacks, float avoidedMHAttacks, float avoidedOHAttacks, float avoidedFinisherAttacks,
            float avoidedPoisonAttacks, float chanceExtraCPPerHit, RogueAbilityStats mainHandStats, RogueAbilityStats offHandStats, RogueAbilityStats backstabStats, RogueAbilityStats hemoStats,
            RogueAbilityStats ruptStats, RogueAbilityStats evisStats, RogueAbilityStats snDStats, RogueAbilityStats recupStats, RogueAbilityStats exposeStats, RogueAbilityStats iPStats,
            RogueAbilityStats dPStats, RogueAbilityStats wPStats) : base(character, stats, calcOpts, hasteBonus, mainHandSpeed, offHandSpeed, mainHandSpeedNorm, offHandSpeedNorm,
            avoidedWhiteMHAttacks, avoidedWhiteOHAttacks, avoidedMHAttacks, avoidedOHAttacks, avoidedFinisherAttacks, avoidedPoisonAttacks, chanceExtraCPPerHit, mainHandStats, offHandStats,
            ruptStats, snDStats, exposeStats, iPStats, dPStats, wPStats)
        {
            BackstabStats = backstabStats;
            HemoStats = hemoStats;
            EvisStats = evisStats;
            RecupStats = recupStats;

            #region Probability tables
            float c = ChanceExtraCPPerHit, h = (1f - c), f = CPOnFinisher, nf = (1f - f);
            _averageNormalCP[1] = 1 * (f + nf * h) + 2 * (nf * c);
            _averageNormalCP[2] = 2 * (f * h + nf * c + nf * h * h) + 3 * (f * c);
            _averageNormalCP[3] = 3 * (f * c + f * h * h + 2 * nf * c * h + nf * h * h * h) + 4 * (f * h * c + nf * c * c + nf * h * h * c);
            _averageNormalCP[4] = 4 * (2 * f * c * h + f * h * h * h + nf * c * c + 3 * nf * c * h * h + nf * h * h * h * h) + 5 * (f * c * c + f * h * h * c + 2 * nf * c * h * c + nf * h * h * h * c);
            _averageNormalCP[5] = 5 * (f * c * c + 3 * f * c * h * h + f * h * h * h * h + 3 * nf * c * c * h + 4 * nf * c * h * h * h + nf * h * h * h * h * h) + 6 * (2 * f * c * h * c + f * h * h * h * c + nf * c * c * c + 3 * nf * c * h * h * c + nf * h * h * h * h * c);
            #endregion
        }

        public override RogueRotationCalculation GetRotationCalculations(float durationMult, int cPG, int recupCP, int ruptCP, bool useHemo, int finisher, int finisherCP, int snDCP, int mHPoison, int oHPoison, bool useTotT, int exposeCP, bool PTRMode)
        {
            Duration = CalcOpts.Duration;
            RecupCP = recupCP;
            UseTotT = useTotT;
            CPG = cPG;
            NumberOfStealths = getNumberStealths();
            EnergyRegen = getEnergyRegen();
            TotalEnergyAvailable = getEnergyAvailable();
            TotalCPAvailable = getCPAvailable();
            float averageGCD = 1f / (1f - AvoidedMHAttacks);
            float averageFinisherGCD = 1f / (1f - AvoidedFinisherAttacks);
            float ruptDurationAverage = RuptStats.DurationAverage;
            float snDDurationAverage = SnDStats.DurationAverage;
            float[] _avgCP = _averageNormalCP;
            float averageFinisherCP = _avgCP[5];

            #region Melee
            float whiteMHAttacks = Duration / MainHandSpeed;
            float whiteOHAttacks = Duration / OffHandSpeed;
            TotalEnergyAvailable += whiteOHAttacks * (1f - AvoidedWhiteOHAttacks) * EnergyOnOHAttack;
            #endregion

            #region Combo Point Generator
            CPGCount = 0f;
            CPGEnergy = getCPGEnergy();
            CPPerCPG = getCPPerCPG();
            float hemoCount = 0f;
            if (useHemo)
            {
                hemoCount = Duration / RV.Hemo.DebuffDuration;
                TotalCPAvailable += hemoCount * HemoStats.CPPerSwing;
                TotalEnergyAvailable -= hemoCount * HemoStats.EnergyCost;
            }
            #endregion

            #region Slice and Dice
            float avgSnDCP = _avgCP[snDCP];
            float effSnDCP = Math.Min(RV.MaxCP, avgSnDCP);
            float snDDuration = SnDStats.DurationAverage + SnDStats.DurationPerCP * effSnDCP;
            float snDCount = getSnDCount(snDDuration);
            float snDTotalEnergy = snDCount * (SnDStats.EnergyCost - RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher * effSnDCP);
            float snDCPRequired = snDCount * (avgSnDCP - CPOnFinisher);
            processFinisher(snDCPRequired, snDTotalEnergy);
            #endregion

            #region Recuperate
            float recupCount = 0f;
            if (recupCP > 0)
            {
                float avgRecupCP = _avgCP[recupCP];
                float effRecupCP = Math.Min(RV.MaxCP, avgRecupCP);
                float recupDuration = RecupStats.DurationAverage + RecupStats.DurationPerCP * effRecupCP;
                recupCount = Duration / recupDuration;
                float recupTotalEnergy = recupCount * (RecupStats.EnergyCost - RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher * effRecupCP);
                float recupCPRequired = recupCount * (avgRecupCP - CPOnFinisher);
                processFinisher(recupCPRequired, recupTotalEnergy);
            }
            #endregion

            #region Expose Armor
            float exposeCount = 0f;
            if (exposeCP > 0)
            {
                float avgExposeCP = _avgCP[exposeCP] * (1f - ExposeCPCostMult);
                float effExposeCP = Math.Min(RV.MaxCP, avgExposeCP);
                float exposeDuration = ExposeStats.DurationAverage + ExposeStats.DurationPerCP * effExposeCP;
                exposeCount = Duration / exposeDuration;
                float exposeTotalEnergy = exposeCount * (ExposeStats.EnergyCost - RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher * effExposeCP);
                float exposeCPRequired = exposeCount * (Math.Max(0f, avgExposeCP - CPOnFinisher));
                processFinisher(exposeCPRequired, exposeTotalEnergy);
            }
            #endregion

            #region Damage Finishers
            float ruptCount = 0f;
            float evisCount = 0f;
            float avgRuptCP = _avgCP[ruptCP];
            float effRuptCP = Math.Min(RV.MaxCP, avgRuptCP);
            float realRuptCount = 0f;
            #region Rupture
            if (ruptCP > 0)
            {
                float ruptDuration = RuptStats.DurationAverage + RuptStats.DurationPerCP * effRuptCP;
                ruptCount = Duration / ruptDuration;
                realRuptCount = Math.Max(1f, Duration / ruptDuration * (1f - ChanceOnRuptResetonEvisCP * Math.Min(RV.MaxCP, _avgCP[finisherCP])));
                float ruptTotalEnergy = realRuptCount * (RuptStats.EnergyCost - effRuptCP * (RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher));
                float ruptCPRequired = realRuptCount * (Math.Max(0f, avgRuptCP - CPOnFinisher));
                processFinisher(ruptCPRequired, ruptTotalEnergy);
            }
            #endregion
            #region Eviscerate
            float avgEvisCP = _avgCP[finisherCP];
            float effEvisCP = Math.Min(RV.MaxCP, avgEvisCP);
            float evisCycleEnergy = ((avgEvisCP - CPOnFinisher) / CPPerCPG) * CPGEnergy + EvisStats.EnergyCost - RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher * effEvisCP;
            evisCount = TotalEnergyAvailable / evisCycleEnergy;
            CPGCount += (evisCount * (avgEvisCP - CPOnFinisher) / CPPerCPG);
            TotalEnergyAvailable = 0f;
            evisCount += TotalCPAvailable / avgEvisCP;
            TotalCPAvailable = 0;
            #endregion
            #endregion

            #region Poisons
            float mHPoisonHitCount = (whiteMHAttacks * (1f - AvoidedWhiteMHAttacks) + CPGCount + evisCount + realRuptCount) * (1f - AvoidedPoisonAttacks);
            float oHPoisonHitCount = whiteOHAttacks * (1f - AvoidedWhiteOHAttacks) * (1f - AvoidedPoisonAttacks);
            float iPCount = 0f;
            float dPTicks = 0f;
            float wPCount = 0f;
            float iPProcRate = RV.IP.Chance / RV.IP.NormWeapSpeed;
            float dPApplyChance = RV.DP.Chance;
            #region MainHand Poison
            if (mHPoison == 1)
                iPCount += mHPoisonHitCount * MainHandStats.Weapon._speed * iPProcRate;
            else if (mHPoison == 2 && oHPoison != 2)
            {
                float dPStackTime = RV.DP.MaxStack * MainHandSpeed / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteMHAttacks));
                dPTicks = RV.DP.MaxStack * Duration / RV.DP.TickTime - RV.GetMissedDPTicks(dPStackTime);
                float dPCountAtMaxStack = mHPoisonHitCount * dPApplyChance * (Duration - dPStackTime) / Duration;
                if (oHPoison == 1)
                    iPCount += dPCountAtMaxStack;
                else if (oHPoison == 3)
                    wPCount += dPCountAtMaxStack;
            }
            else if (mHPoison == 3)
                wPCount += mHPoisonHitCount * MainHandStats.Weapon._speed * RV.WP.Chance / RV.WP.NormWeapSpeed;
            #endregion
            #region OffHand Poison
            if (oHPoison == 1)
                iPCount += oHPoisonHitCount * OffHandStats.Weapon._speed * iPProcRate;
            else if (oHPoison == 2 && mHPoison != 2)
            {
                float dPStackTime = RV.DP.MaxStack * OffHandSpeed / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteMHAttacks));
                dPTicks = RV.DP.MaxStack * Duration / RV.DP.TickTime - RV.GetMissedDPTicks(dPStackTime);
                float dPCountAtMaxStack = oHPoisonHitCount * dPApplyChance * (Duration - dPStackTime) / Duration;
                if (mHPoison == 1)
                    iPCount += dPCountAtMaxStack;
                else if (mHPoison == 3)
                    wPCount += dPCountAtMaxStack;
            }
            else if (oHPoison == 3)
                wPCount += oHPoisonHitCount * OffHandStats.Weapon._speed * RV.WP.Chance / RV.WP.NormWeapSpeed;
            #endregion
            iPCount *= (1f - AvoidedPoisonAttacks);
            wPCount *= (1f - AvoidedPoisonAttacks);
            #endregion

            #region Damage Totals
            float mainHandDamageTotal = whiteMHAttacks * MainHandStats.DamagePerSwing;
            float offHandDamageTotal = whiteOHAttacks * OffHandStats.DamagePerSwing;
            float backstabDamageTotal = (CPG == 0 ? CPGCount : 0) * BackstabStats.DamagePerSwing;
            float hemoDamageTotal = ((CPG == 1 ? CPGCount : 0) + hemoCount) * HemoStats.DamagePerSwing;
            float ruptDamageTotal = ruptCount * RuptStats.DamagePerSwingArray[(int)Math.Floor((double)effRuptCP)] + (effRuptCP - (float)Math.Floor((double)effRuptCP)) * (RuptStats.DamagePerSwingArray[(int)Math.Min(Math.Floor((double)effRuptCP) + 1, 5)] - RuptStats.DamagePerSwingArray[(int)Math.Floor((double)effRuptCP)]);
            float evisDamageTotal = evisCount * (EvisStats.DamagePerSwing + EvisStats.DamagePerSwingPerCP * Math.Min(_avgCP[finisherCP], 5));
            float instantPoisonTotal = iPCount * IPStats.DamagePerSwing;
            float deadlyPoisonTotal = dPTicks * DPStats.DamagePerSwing;
            float woundPoisonTotal = wPCount * WPStats.DamagePerSwing;

            float damageTotal = (mainHandDamageTotal + offHandDamageTotal + backstabDamageTotal + hemoDamageTotal + ruptDamageTotal + evisDamageTotal + instantPoisonTotal + deadlyPoisonTotal
                + woundPoisonTotal);
            #endregion

            return new RogueRotationCalculation()
            {
                MultipleSegments = Duration < 1f,
                Duration = Duration,
                TotalDamage = damageTotal,
                DPS = damageTotal / Duration,

                MainHandCount = whiteMHAttacks,
                OffHandCount = whiteOHAttacks,
                BackstabCount = (CPG == 0 ? CPGCount : 0),
                HemoCount = ((CPG == 1 ? CPGCount : 0) + hemoCount),
                RuptCount = ruptCount,
                EvisCount = evisCount,
                SnDCount = snDCount,
                RecupCount = recupCount,
                EACount = exposeCount,
                IPCount = iPCount,
                DPCount = dPTicks,
                WPCount = wPCount,

                FinisherCP = finisherCP,
                EvisCP = Math.Min(_avgCP[finisherCP], RV.MaxCP),
                RuptCP = ruptCP,
                SnDCP = snDCP,

                MHPoison = mHPoison,
                OHPoison = oHPoison,

                UseTotT = useTotT,
                SerratedBlades = ChanceOnRuptResetonEvisCP * Math.Min(RV.MaxCP, _avgCP[finisherCP]),
            };
        }

        public override float getNumberStealths()
        {
            return 1f + Duration / (RV.Vanish.CD - VanishCDReduction) + (StepVanishResetCD > 0 ? Duration / StepVanishResetCD : 0f);
        }

        public override float getEnergyAvailable()
        {
            return RV.BaseEnergy + EnergyRegen * Duration +
                (UseTotT ? (-RV.TotT.Cost + ToTTCostReduction) * (Duration - RV.TotT.Duration) / RV.TotT.CD : 0f) +
                (RecupCP > 0 ? Duration / 3 * EnergyOnRecupTick : 0f);
        }

        public override float getCPAvailable()
        {   
            return (Talents.Premeditation > 0 ? RV.Talents.PremeditationBonusCP * Math.Min(Duration / RV.Talents.PremeditationCD, NumberOfStealths) : 0) + (Talents.HonorAmongThieves > 0 ? RV.Talents.HonorAmongThievesCPChance[Talents.HonorAmongThieves] * Duration / RV.Talents.HonorAmongThievesCD[Talents.HonorAmongThieves] : 0);
        }

        public override float getCPGEnergy()
        {
            return (CPG == 1 ? BackstabStats.EnergyCost : HemoStats.EnergyCost);
        }

        public override float getCPPerCPG()
        {
            return (CPG == 1 ? BackstabStats.CPPerSwing : HemoStats.CPPerSwing);
        }
        public override void processFinisher(float cpRequired, float finisherEnergy)
        {
            if (TotalCPAvailable >= cpRequired) TotalCPAvailable -= cpRequired;
            else
            {
                float cpgToUse = (cpRequired - TotalCPAvailable) / CPPerCPG;
                CPGCount += cpgToUse;
                TotalEnergyAvailable -= cpgToUse * CPGEnergy + finisherEnergy;
                TotalCPAvailable = 0;
            }
        }
    }
}