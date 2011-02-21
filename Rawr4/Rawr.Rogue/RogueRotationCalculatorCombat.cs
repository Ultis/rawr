using System;

namespace Rawr.Rogue
{
    public class RogueRotationCalculatorCombat : RogueRotationCalculator
    {
        public RogueAbilityStats SStrikeStats { get; set; }
        public RogueAbilityStats RStrikeStats { get; set; }
        public RogueAbilityStats EvisStats { get; set; }
        public RogueAbilityStats MainGaucheStats { get; set; }

        public float ChanceOnMGAttackOnMHAttack { get; set; }
        public float EnergyRegenMultiplier { get; set; }

        private float[] _averageNormalCP = new float[6];
        private float[] _averageSStrikeCP = new float[6];

        public RogueRotationCalculatorCombat(Character character, Stats stats, CalculationOptionsRogue calcOpts, float hasteBonus, float mainHandSpeed, float offHandSpeed, float mainHandSpeedNorm,
            float offHandSpeedNorm, float avoidedWhiteMHAttacks, float avoidedWhiteOHAttacks, float avoidedMHAttacks, float avoidedOHAttacks, float avoidedFinisherAttacks,
            float avoidedPoisonAttacks, float chanceExtraCPPerHit, RogueAbilityStats mainHandStats, RogueAbilityStats offHandStats, RogueAbilityStats mainGaucheStats,
            RogueAbilityStats sStrikeStats, RogueAbilityStats rStrikeStats, RogueAbilityStats ruptStats, RogueAbilityStats evisStats, RogueAbilityStats snDStats, RogueAbilityStats exposeStats,
            RogueAbilityStats iPStats, RogueAbilityStats dPStats, RogueAbilityStats wPStats) : base(character, stats, calcOpts, hasteBonus, mainHandSpeed, offHandSpeed, mainHandSpeedNorm,
            offHandSpeedNorm, avoidedWhiteMHAttacks, avoidedWhiteOHAttacks, avoidedMHAttacks, avoidedOHAttacks, avoidedFinisherAttacks, avoidedPoisonAttacks, chanceExtraCPPerHit, mainHandStats,
            offHandStats, ruptStats, snDStats, exposeStats, iPStats, dPStats, wPStats)
        {
            SStrikeStats = sStrikeStats;
            RStrikeStats = rStrikeStats;
            EvisStats = evisStats;
            MainGaucheStats = mainGaucheStats;

            ChanceOnMGAttackOnMHAttack = RV.Mastery.MainGauche + RV.Mastery.MainGauchePerMast * StatConversion.GetMasteryFromRating(stats.MasteryRating);
            EnergyRegenMultiplier = (1f + RV.Mastery.VitalityRegenMult) * (1f + (RV.AR.Duration + (Talents.GlyphOfAdrenalineRush ? RV.Glyph.ARDurationBonus : 0f)) / RV.AR.CD * Talents.AdrenalineRush) * (1f + HasteBonus) - 1f;

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
            #endregion
        }

        public override RogueRotationCalculation GetRotationCalculations(float durationMult, int cPG, int recupCP, int ruptCP, bool useRS, int finisher, int evisCP, int snDCP, int mHPoison, int oHPoison, bool useTotT, int exposeCP, bool PTRMode)
        {
            Duration = CalcOpts.Duration;
            UseTotT = useTotT;
            NumberOfStealths = getNumberStealths();
            EnergyRegen = getEnergyRegen();
            TotalEnergyAvailable = getEnergyAvailable();
            TotalCPAvailable = getCPAvailable();
            float averageGCD = 1f / (1f - AvoidedMHAttacks);
            float averageFinisherGCD = 1f / (1f - AvoidedFinisherAttacks);
            float ruptDurationAverage = RuptStats.DurationAverage;
            float snDDurationAverage = SnDStats.DurationAverage;
            float[] _avgCP = CPG == 1 ? _averageSStrikeCP : _averageNormalCP;

            #region Melee
            float whiteMHAttacks = Duration / MainHandSpeed;
            float whiteOHAttacks = Duration / OffHandSpeed;
            float mGAttacks = ChanceOnMGAttackOnMHAttack * whiteMHAttacks;
            TotalEnergyAvailable += whiteOHAttacks * (1f - AvoidedWhiteOHAttacks) * EnergyOnOHAttack;
            #endregion

            #region Combo Point Generator
            CPGCount = 0f;
            CPGEnergy = getCPGEnergy();
            CPPerCPG = getCPPerCPG();
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

            #region Expose Armor
            float rSCount = 0f;
            float exposeCount = 0f;
            if (exposeCP > 0)
            {
                float avgExposeCP = _avgCP[exposeCP] * (1f - ExposeCPCostMult);
                float effExposeCP = Math.Min(RV.MaxCP, avgExposeCP);
                float exposeDuration = ExposeStats.DurationAverage + ExposeStats.DurationPerCP * effExposeCP;
                exposeCount = Duration / (exposeDuration * (1f + RSBonus));
                float exposeTotalEnergy = exposeCount * (ExposeStats.EnergyCost - RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher * effExposeCP + (useRS ? exposeCount * RStrikeStats.EnergyCost : 0f));
                float exposeCPRequired = exposeCount * (Math.Max(0f, avgExposeCP - CPOnFinisher) - (useRS ? RStrikeStats.CPPerSwing : 0f));
                rSCount += useRS ? exposeCount : 0f;
                processFinisher(exposeCPRequired, exposeTotalEnergy);
            }
            #endregion

            #region Damage Finishers
            float ruptCount = 0f;
            float evisCount = 0f;
            #region Rupture
            if (ruptCP > 0)
            {
                float avgRuptCP = _avgCP[ruptCP];
                float effRuptCP = Math.Min(RV.MaxCP, avgRuptCP);
                float ruptDuration = RuptStats.DurationAverage + RuptStats.DurationPerCP * effRuptCP;
                ruptCount = Duration / ruptDuration;
                float ruptTotalEnergy = ruptCount * (RuptStats.EnergyCost + (useRS ? ruptCount * RStrikeStats.EnergyCost : 0f) -
                     effRuptCP * (RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher + EnergyRegenTimeOnDamagingCP * EnergyRegen));
                rSCount += useRS ? ruptCount : 0f;
                float ruptCPRequired = ruptCount * (Math.Max(0f, avgRuptCP - CPOnFinisher) - (useRS ? RStrikeStats.CPPerSwing : 0f));
                processFinisher(ruptCPRequired, ruptTotalEnergy);
            }
            #endregion
            #region Eviscerate
            float avgEvisCP = _avgCP[evisCP];
            float effEvisCP = Math.Min(RV.MaxCP, avgEvisCP);
            float evisCycleEnergy = ((avgEvisCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG) * CPGEnergy + EvisStats.EnergyCost + (useRS ? RStrikeStats.EnergyCost : 0f) - effEvisCP * (RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher + EnergyRegenTimeOnDamagingCP * EnergyRegen);
            evisCount = TotalEnergyAvailable / evisCycleEnergy;
            CPGCount += evisCount * (avgEvisCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG;
            rSCount += useRS ? evisCount : 0f;
            TotalEnergyAvailable = 0f;
            #endregion
            #endregion

            #region Poisons
            float mHHitCount = whiteMHAttacks * (1f - AvoidedWhiteMHAttacks) + CPGCount + evisCount + snDCount;
            float oHHitCount = whiteOHAttacks * (1f - AvoidedWhiteOHAttacks);
            float iPCount = 0f;
            float dPTicks = 0f;
            float wPCount = 0f;
            float iPProcRate = RV.IP.Chance / RV.IP.NormWeapSpeed;
            float dPApplyChance = RV.DP.Chance;
            #region MainHand Poison
            if (mHPoison == 1)
                iPCount += mHHitCount * MainHandStats.Weapon._speed * iPProcRate;
            else if (mHPoison == 2 && oHPoison != 2)
            {
                float dPStackTime = RV.DP.MaxStack * MainHandSpeed / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteMHAttacks));
                float dPCountAtMaxStack = mHHitCount * dPApplyChance * (1f - AvoidedPoisonAttacks) * (Duration - dPStackTime) / Duration;
                float missedTicks = RV.GetMissedDPTicks(dPStackTime);
                dPTicks = mHHitCount * dPApplyChance * RV.DP.MaxStack * (1f - AvoidedPoisonAttacks) - missedTicks;
                if (oHPoison == 1)
                    iPCount += dPCountAtMaxStack;
                else if (oHPoison == 3)
                    wPCount += dPCountAtMaxStack;
            }
            else if (mHPoison == 3)
                wPCount += mHHitCount * MainHandStats.Weapon._speed * RV.WP.Chance / RV.WP.NormWeapSpeed;
            #endregion
            #region OffHand Poison
            if (oHPoison == 1)
                iPCount += oHHitCount * OffHandStats.Weapon._speed * iPProcRate;
            else if (oHPoison == 2 && mHPoison != 2)
            {
                float dPStackTime = RV.DP.MaxStack * OffHandSpeed / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteMHAttacks));
                float dPCountAtMaxStack = oHHitCount * dPApplyChance * (1f - AvoidedPoisonAttacks) * (Duration - dPStackTime) / Duration;
                float missedTicks = RV.GetMissedDPTicks(dPStackTime);
                dPTicks = oHHitCount * dPApplyChance * RV.DP.MaxStack * (1f - AvoidedPoisonAttacks) - missedTicks;
                if (oHPoison == 1)
                    iPCount += dPCountAtMaxStack;
                else if (oHPoison == 3)
                    wPCount += dPCountAtMaxStack;
            }
            else if (oHPoison == 3)
                wPCount += oHHitCount * OffHandStats.Weapon._speed * RV.WP.Chance / RV.WP.NormWeapSpeed;
            #endregion
            iPCount *= (1f - AvoidedPoisonAttacks);
            wPCount *= (1f - AvoidedPoisonAttacks);
            #endregion

            #region Killing Spree & Adrenaline Rush
            float kSAttacks = 0;
            float kSDuration = 0;
            float kSDmgBonus = RV.KS.DmgMult + (Talents.GlyphOfKillingSpree ? RV.Glyph.KSDmgMultBonus : 0f);
            float restlessBladesBonus = _avgCP[ruptCP] * ruptCount + _avgCP[evisCP] * evisCount * Talents.RestlessBlades * RV.Talents.RestlessBladesPerCPCDReduc;
            if (Talents.KillingSpree > 0)
            {
                float kSCount = (Duration + restlessBladesBonus) / RV.KS.CD;
                kSDuration = kSCount * RV.KS.Duration;
                kSAttacks = RV.KS.StrikeCount * kSCount;
            }
            if (Talents.AdrenalineRush > 0)
            {
                float ARMult = RV.AR.MeleeSpeedMult * (RV.AR.Duration + (Talents.GlyphOfAdrenalineRush ? RV.Glyph.ARDurationBonus : 0f)) * (Duration + restlessBladesBonus) / RV.AR.CD / Duration;
                whiteMHAttacks *= 1f + ARMult;
                whiteOHAttacks *= 1f + ARMult;
            }
            #endregion

            #region Damage Totals
            float mainHandDamageTotal = whiteMHAttacks * MainHandStats.DamagePerSwing +
                                        kSAttacks * MainHandStats.DamagePerSwing * (1f + kSDmgBonus) / (1f + kSDmgBonus * kSDuration / Duration);
            float offHandDamageTotal = whiteOHAttacks * OffHandStats.DamagePerSwing +
                                        kSAttacks * OffHandStats.DamagePerSwing * (1f + kSDmgBonus) / (1f + kSDmgBonus * kSDuration / Duration);
            float mainGaucheDamageTotal = mGAttacks * MainGaucheStats.DamagePerSwing;
            float sStrikeDamageTotal = CPGCount * SStrikeStats.DamagePerSwing;
            float rStrikeDamageTotal = rSCount * RStrikeStats.DamagePerSwing;
            float ruptDamageTotal = ruptCount * RuptStats.DamagePerSwingArray[(int)Math.Floor((double)ruptCP)] + (ruptCP - (float)Math.Floor((double)ruptCP)) * (RuptStats.DamagePerSwingArray[(int)Math.Min(Math.Floor((double)ruptCP) + 1, 5)] - RuptStats.DamagePerSwingArray[(int)Math.Floor((double)ruptCP)]) * (RuptStats.DurationUptime / 16f) * (useRS ? (1f + RSBonus) : 1f);
            float evisDamageTotal = evisCount * (EvisStats.DamagePerSwing + EvisStats.DamagePerSwingPerCP * Math.Min(_avgCP[evisCP], 5)) * (useRS ? (1f + RSBonus) : 1f);
            float instantPoisonTotal = iPCount * IPStats.DamagePerSwing;
            float deadlyPoisonTotal = dPTicks * DPStats.DamagePerSwing;
            float woundPoisonTotal = wPCount * WPStats.DamagePerSwing;

            float damageTotal = (mainHandDamageTotal + offHandDamageTotal + sStrikeDamageTotal + rStrikeDamageTotal + ruptDamageTotal + evisDamageTotal + instantPoisonTotal + deadlyPoisonTotal +
                woundPoisonTotal) * (1f + kSDmgBonus * kSDuration / Duration);
            if (Talents.BanditsGuile > 0)
            {
                float buildupTime = Duration / (((CPG == 1 ? CPGCount : 0) + rSCount) * RV.Talents.BanditsGuileChance[Talents.BanditsGuile]);
                float guileBonus = RV.Talents.BanditsGuileStep / buildupTime + 2f * RV.Talents.BanditsGuileStep / buildupTime + 3 * RV.Talents.BanditsGuileStep / RV.Talents.BanditsGuileDuration;
                damageTotal *= 1f + guileBonus;
            }
            if (Spec == 2) //Master of Subtlety specialization
            {
                damageTotal *= 1f + RV.Mastery.MasterOfSubtletyDmgMult * NumberOfStealths * RV.Mastery.MasterOfSubtletyDuration / Duration;
            }
            #endregion

            return new RogueRotationCalculation()
            {
                MultipleSegments = Duration < 1f,
                Duration = Duration,
                TotalDamage = damageTotal,
                DPS = damageTotal / Duration,

                MainHandCount = whiteMHAttacks,
                OffHandCount = whiteOHAttacks,
                MGCount = mGAttacks,
                SStrikeCount = CPGCount,
                RStrikeCount = rSCount,
                RuptCount = ruptCount,
                EvisCount = evisCount,
                SnDCount = snDCount,
                EACount = exposeCount,
                IPCount = iPCount,
                DPCount = dPTicks,
                WPCount = wPCount,

                FinisherCP = evisCP,
                EvisCP = (finisher == 1 ? Math.Min(_avgCP[evisCP], 5) : 0),
                RuptCP = ruptCP,
                SnDCP = snDCP,

                MHPoison = mHPoison,
                OHPoison = oHPoison,

                UseTotT = useTotT,
            };
        }

        public override float getEnergyRegen()
        {
            return RV.BaseEnergyRegen * (1f + EnergyRegenMultiplier);
        }

        public override float getEnergyAvailable()
        {
            return RV.BaseEnergy + EnergyRegen * Duration +
                (UseTotT ? (-RV.TotT.Cost + ToTTCostReduction) * (Duration - RV.TotT.Duration) / RV.TotT.CD : 0f) +
                Talents.AdrenalineRush * RV.AR.Duration * EnergyRegen * (1f + RV.AR.EnergyRegenMult) * Duration / RV.AR.CD;
        }

        public override float getCPGEnergy()
        {
            return SStrikeStats.EnergyCost;
        }

        public override float getCPPerCPG()
        {
            return SStrikeStats.CPPerSwing;
        }
    }
}
