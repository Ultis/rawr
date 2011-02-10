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

        public RogueRotationCalculatorSubt(Character character, int spec, Stats stats, CalculationOptionsRogue calcOpts, float hasteBonus,
            float mainHandSpeed, float offHandSpeed, float mainHandSpeedNorm, float offHandSpeedNorm, float avoidedWhiteMHAttacks, float avoidedWhiteOHAttacks, float avoidedMHAttacks, float avoidedOHAttacks, float avoidedFinisherAttacks, float avoidedPoisonAttacks,
			float chanceExtraCPPerHit, float chanceExtraCPPerMutiHit,
            RogueAbilityStats mainHandStats, RogueAbilityStats offHandStats, RogueAbilityStats mainGaucheStats, RogueAbilityStats backstabStats, RogueAbilityStats hemoStats, RogueAbilityStats sStrikeStats,
            RogueAbilityStats mutiStats, RogueAbilityStats rStrikeStats, RogueAbilityStats ruptStats, RogueAbilityStats evisStats, RogueAbilityStats envenomStats, RogueAbilityStats snDStats, RogueAbilityStats recupStats, RogueAbilityStats exposeStats,
            RogueAbilityStats iPStats, RogueAbilityStats dPStats, RogueAbilityStats wPStats) : base(character, spec, stats, calcOpts, hasteBonus,
                mainHandSpeed, offHandSpeed, mainHandSpeedNorm, offHandSpeedNorm, avoidedWhiteMHAttacks, avoidedWhiteOHAttacks, avoidedMHAttacks, avoidedOHAttacks, avoidedFinisherAttacks, avoidedPoisonAttacks,
                chanceExtraCPPerHit, chanceExtraCPPerMutiHit, mainHandStats, offHandStats, ruptStats, snDStats, exposeStats, iPStats, dPStats, wPStats)
        {
            BackstabStats = backstabStats;
            HemoStats = hemoStats;
            MutiStats = mutiStats;
            SStrikeStats = sStrikeStats;
            RStrikeStats = rStrikeStats;
            EnvenomStats = envenomStats;
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

        public override RogueRotationCalculation GetRotationCalculations(float duration, int cPG, int recupCP, int ruptCP, bool useRS, int finisher, int finisherCP, int snDCP, int mHPoison, int oHPoison, bool bleedIsUp, bool useTotT, int exposeCP, bool PTRMode)
        {
            Duration = duration;
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
            float whiteMHAttacks = duration / MainHandSpeed;
            float whiteOHAttacks = duration / OffHandSpeed;
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

            #region Recuperate
            float recupCount = 0f;
            if (recupCP > 0)
            {
                float avgRecupCP = _avgCP[recupCP];
                float effRecupCP = Math.Min(RV.MaxCP, avgRecupCP);
                float recupDuration = RecupStats.DurationAverage + RecupStats.DurationPerCP * effRecupCP;
                recupCount = duration / recupDuration;
                float recupTotalEnergy = recupCount * (RecupStats.EnergyCost - RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher * effRecupCP);
                float recupCPRequired = recupCount * (avgRecupCP - CPOnFinisher);
                processFinisher(recupCPRequired, recupTotalEnergy);
            }
            #endregion

            #region Expose Armor
            float rSCount = 0f;
            float exposeCount = 0f;
            if (exposeCP > 0)
            {
                float avgExposeCP = _avgCP[exposeCP] * (1f - ExposeCPCostMult);
                float effExposeCP = Math.Min(RV.MaxCP, avgExposeCP);
                float exposeDuration = ExposeStats.DurationAverage + ExposeStats.DurationPerCP * effExposeCP;
                exposeCount = duration / (exposeDuration * (1f + RSBonus));
                float exposeTotalEnergy = exposeCount * (ExposeStats.EnergyCost - RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher * effExposeCP + (useRS ? exposeCount * RStrikeStats.EnergyCost : 0f));
                float exposeCPRequired = exposeCount * (Math.Max(0f, avgExposeCP - CPOnFinisher) - (useRS ? RStrikeStats.CPPerSwing : 0f));
                rSCount += useRS ? exposeCount : 0f;
                processFinisher(exposeCPRequired, exposeTotalEnergy);
            }
            #endregion

            #region Damage Finishers
            float ruptCount = 0f;
            float evisCount = 0f;
            float envenomCount = 0f;
            #region Rupture
            if (ruptCP > 0)
            {
                float avgRuptCP = _avgCP[ruptCP];
                float effRuptCP = Math.Min(RV.MaxCP, avgRuptCP);
                float ruptDuration = RuptStats.DurationAverage + RuptStats.DurationPerCP * effRuptCP;
                ruptCount = duration / ruptDuration;
                float ruptTotalEnergy = ruptCount * (RuptStats.EnergyCost + (useRS ? exposeCount * RStrikeStats.EnergyCost : 0f) -
                     effRuptCP * (RV.Talents.RelentlessStrikesEnergyBonus * ChanceOnEnergyPerCPFinisher + EnergyRegenTimeOnDamagingCP * EnergyRegen) -
                     ChanceOnEnergyOnGarrRuptTick * RV.Talents.VenemousWoundsEnergy * ruptDuration / RV.Rupt.TickTime);
                ruptCount *= Math.Min(1f, TotalEnergyAvailable / ruptTotalEnergy);
                rSCount += useRS ? ruptCount : 0f;
                float ruptCountReal = Math.Max(1f, (finisher == 1 ? ruptCount * (1f - _avgCP[finisherCP] * ChanceOnRuptResetonEvisCP) : ruptCount));
                float ruptCPRequired = ruptCountReal * (Math.Max(0f, avgRuptCP - CPOnFinisher) - (useRS ? RStrikeStats.CPPerSwing : 0f));
                processFinisher(ruptCPRequired, ruptTotalEnergy);
            }
            #endregion
            #region Eviscerate
            if (finisher == 1 && finisherCP > 0)
            {
                float averageEvisCP = _avgCP[finisherCP];
                float evisCycleEnergy = ((averageEvisCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG) * CPGEnergy + EvisStats.EnergyCost + (useRS ? RStrikeStats.EnergyCost : 0f) - 25f * ChanceOnEnergyPerCPFinisher * averageEvisCP - averageFinisherCP * EnergyRegenTimeOnDamagingCP * EnergyRegen;
                evisCount = TotalEnergyAvailable / evisCycleEnergy;
                if (TotalCPAvailable >= evisCount * (averageEvisCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f))) TotalCPAvailable -= evisCount * (averageEvisCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f));
                else
                {
                    CPGCount += (evisCount * ((averageEvisCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) - TotalCPAvailable) / CPPerCPG);
                    rSCount += useRS ? evisCount : 0f;
                    TotalEnergyAvailable = 0f;
                    TotalCPAvailable = 0;
                }
                evisCount += TotalCPAvailable / averageEvisCP;
            }
            #endregion
            #region Envenom
            else if (finisher == 2 && finisherCP > 0)
            {
                float averageEnvenomCP = _avgCP[finisherCP];
                float envenomCycleEnergy = ((averageEnvenomCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) / CPPerCPG) * CPGEnergy + EnvenomStats.EnergyCost + (useRS ? RStrikeStats.EnergyCost : 0f) - 25f * ChanceOnEnergyPerCPFinisher * averageEnvenomCP - averageFinisherCP * EnergyRegenTimeOnDamagingCP * EnergyRegen;
                envenomCount = TotalEnergyAvailable / envenomCycleEnergy;
                if (TotalCPAvailable >= envenomCount * (averageEnvenomCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f))) TotalCPAvailable -= envenomCount * (averageEnvenomCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f));
                else
                {
                    CPGCount += (envenomCount * ((averageEnvenomCP - CPOnFinisher - (useRS ? RStrikeStats.CPPerSwing : 0f)) - TotalCPAvailable) / CPPerCPG);
                    rSCount += useRS ? envenomCount : 0f;
                    TotalEnergyAvailable = 0f;
                    TotalCPAvailable = 0;
                }
            }
            #endregion
            #endregion

            #region Poisons
            float mHHitCount = whiteMHAttacks * (1f - AvoidedWhiteMHAttacks) + CPGCount + evisCount + envenomCount + snDCount;
            float oHHitCount = whiteOHAttacks * (1f - AvoidedWhiteOHAttacks) + (CPGCount == 0 ? CPGCount : 0);
            float iPCount = 0f;
            float dPTicks = 0f;
            float wPCount = 0f;
            float iPProcRate = 0.2f * (1f + IPFrequencyMultiplier) / 1.4f;
            float dPApplyChance = 0.3f + DPFrequencyBonus;
            float envenomBuffTime = envenomCount * finisherCP + envenomCount;
            #region MainHand Poison
            if (mHPoison == 1)
                iPCount += mHHitCount * MainHandStats.Weapon._speed * iPProcRate * ((duration - envenomBuffTime) / duration +
                                                                 1.75f * envenomBuffTime / duration);
            else if (mHPoison == 2 && oHPoison != 2)
            {
                float dPStackTime = RV.DP.MaxStack * MainHandSpeed / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteMHAttacks));
                float envBuffDuration = envenomCount > 0 ? RV.Envenom.BuffDuration + _avgCP[finisherCP] * RV.Envenom.BuffDurationPerCP : 0f;
                float envBuffRemainder = 0f;
                float dPStackTimeBuff = 0f;
                #region Calculate DP stack time with Envenom buff
                float dPStackTimeBuffed = (1f - ChanceOnNoDPConsumeOnEnvenom) * RV.DP.MaxStack * MainHandSpeed / ((dPApplyChance + RV.Envenom.BuffDPChanceBonus) * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteMHAttacks));
                if (dPStackTimeBuffed >= envBuffDuration)
                {
                    float dPStackTimeRemainder = (1f - envBuffDuration / dPStackTimeBuffed) * RV.DP.MaxStack * MainHandSpeed / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteMHAttacks));
                    dPStackTimeBuff = envBuffDuration + dPStackTimeRemainder;
                }
                else
                {
                    envBuffRemainder = envBuffDuration - dPStackTimeBuffed;
                    dPStackTimeBuff = dPStackTimeBuffed;
                }
                #endregion
                float dPCountAtMaxStack = mHHitCount * dPApplyChance * (1f - AvoidedPoisonAttacks) * (duration - dPStackTime - (dPStackTimeBuff + envBuffRemainder) * envenomCount + (1f + RV.Envenom.BuffDPChanceBonus) * envenomCount * envBuffRemainder) / duration;
                float missedTicks = (1f - ChanceOnNoDPConsumeOnEnvenom) * envenomCount * RV.GetMissedDPTicks(dPStackTimeBuff) + RV.GetMissedDPTicks(dPStackTime);
                dPTicks = mHHitCount * dPApplyChance * (1f - AvoidedPoisonAttacks) * (duration - envenomBuffTime) / duration + mHHitCount * (dPApplyChance + RV.Envenom.BuffDPChanceBonus) * (1f - AvoidedPoisonAttacks) * envenomBuffTime / duration - missedTicks;
                if (oHPoison == 1)
                    iPCount += dPCountAtMaxStack;
                else if (oHPoison == 3)
                    wPCount += dPCountAtMaxStack;
            }
            else if (mHPoison == 3)
                wPCount += mHHitCount * MainHandStats.Weapon._speed * 21.43f / 60f;
            #endregion
            #region OffHand Poison
            if (oHPoison == 1)
                iPCount += oHHitCount * OffHandStats.Weapon._speed * iPProcRate * ((duration - envenomBuffTime) / duration +
                                                                1.75f * envenomBuffTime / duration);
            else if (oHPoison == 2 && mHPoison != 2)
            {
                float dPStackTime = RV.DP.MaxStack * OffHandSpeed / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteMHAttacks));
                float envBuffDuration = envenomCount > 0 ? RV.Envenom.BuffDuration + _avgCP[finisherCP] * RV.Envenom.BuffDurationPerCP : 0f;
                float envBuffRemainder = 0f;
                float dPStackTimeBuff = 0f;
                #region Calculate DP stack time with Envenom buff
                float dPStackTimeBuffed = (1f - ChanceOnNoDPConsumeOnEnvenom) * RV.DP.MaxStack * OffHandSpeed / ((dPApplyChance + RV.Envenom.BuffDPChanceBonus) * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteOHAttacks));
                if (dPStackTimeBuffed >= envBuffDuration)
                {
                    float dPStackTimeRemainder = (1f - envBuffDuration / dPStackTimeBuffed) * RV.DP.MaxStack * OffHandSpeed / (dPApplyChance * (1f - AvoidedPoisonAttacks) * (1f - AvoidedWhiteOHAttacks));
                    dPStackTimeBuff = envBuffDuration + dPStackTimeRemainder;
                }
                else
                {
                    envBuffRemainder = envBuffDuration - dPStackTimeBuffed;
                    dPStackTimeBuff = dPStackTimeBuffed;
                }
                #endregion
                float dPCountAtMaxStack = oHHitCount * dPApplyChance * (1f - AvoidedPoisonAttacks) * (duration - dPStackTime - (dPStackTimeBuff + envBuffRemainder) * envenomCount + (1f + RV.Envenom.BuffDPChanceBonus) * envenomCount * envBuffRemainder) / duration;
                float missedTicks = (1f - ChanceOnNoDPConsumeOnEnvenom) * envenomCount * RV.GetMissedDPTicks(dPStackTimeBuff) + RV.GetMissedDPTicks(dPStackTime);
                dPTicks = oHHitCount * dPApplyChance * (1f - AvoidedPoisonAttacks) * (duration - envenomBuffTime) / duration + oHHitCount * (dPApplyChance + RV.Envenom.BuffDPChanceBonus) * (1f - AvoidedPoisonAttacks) * envenomBuffTime / duration - missedTicks;
                if (oHPoison == 1)
                    iPCount += dPCountAtMaxStack;
                else if (oHPoison == 3)
                    wPCount += dPCountAtMaxStack;
            }
            else if (oHPoison == 3)
                wPCount += oHHitCount * OffHandStats.Weapon._speed * 21.43f / 60f;
            #endregion
            iPCount *= (1f - AvoidedPoisonAttacks);
            wPCount *= (1f - AvoidedPoisonAttacks);
            #endregion

            #region Killing Spree & Adrenaline Rush
            float kSAttacks = 0;
            float kSDuration = 0;
            float kSDmgBonus = RV.KS.DmgMult + (Talents.GlyphOfKillingSpree ? RV.Glyph.KSDmgMultBonus : 0f);
            float restlessBladesBonus = averageFinisherCP * ruptCount + _avgCP[finisherCP] * (evisCount + envenomCount) * Talents.RestlessBlades * RV.Talents.RestlessBladesPerCPCDReduc;
            if (Talents.KillingSpree > 0)
            {
                float kSCount = (duration + restlessBladesBonus) / RV.KS.CD;
                kSDuration = kSCount * RV.KS.Duration;
                kSAttacks = RV.KS.StrikeCount * kSCount;
            }
            if (Talents.AdrenalineRush > 0)
            {
                float ARMult = RV.AR.MeleeSpeedMult * (RV.AR.Duration + (Talents.GlyphOfAdrenalineRush ? RV.Glyph.ARDurationBonus : 0f)) * (duration + restlessBladesBonus) / RV.AR.CD / duration;
                whiteMHAttacks *= 1f + ARMult;
                whiteOHAttacks *= 1f + ARMult;
            }
            #endregion

            #region Damage Totals
            float mainHandDamageTotal = whiteMHAttacks * MainHandStats.DamagePerSwing;
            float offHandDamageTotal = whiteOHAttacks * OffHandStats.DamagePerSwing;
            float backstabDamageTotal = (CPG == 2 ? CPGCount : 0) * BackstabStats.DamagePerSwing;
            float hemoDamageTotal = (CPG == 3 ? CPGCount : 0) * HemoStats.DamagePerSwing;
            float sStrikeDamageTotal = (CPG == 1 ? CPGCount : 0) * SStrikeStats.DamagePerSwing;
            float mutiDamageTotal = (CPG == 0 ? CPGCount : 0) * MutiStats.DamagePerSwing;
            float rStrikeDamageTotal = rSCount * RStrikeStats.DamagePerSwing;
            float ruptDamageTotal = ruptCount * RuptStats.DamagePerSwingArray[(int)Math.Floor((double)ruptCP)] + (ruptCP - (float)Math.Floor((double)ruptCP)) * (RuptStats.DamagePerSwingArray[(int)Math.Min(Math.Floor((double)ruptCP) + 1, 5)] - RuptStats.DamagePerSwingArray[(int)Math.Floor((double)ruptCP)]) * (RuptStats.DurationUptime / 16f) * (useRS ? (1f + RSBonus) : 1f);
            float evisDamageTotal = evisCount * (EvisStats.DamagePerSwing + EvisStats.DamagePerSwingPerCP * Math.Min(_avgCP[finisherCP], 5)) * (useRS ? (1f + RSBonus) : 1f);
            float envenomDamageTotal = envenomCount * (EnvenomStats.DamagePerSwing + EnvenomStats.DamagePerSwingPerCP * Math.Min(_avgCP[finisherCP], 5)) * (useRS ? (1f + RSBonus) : 1f);
            float instantPoisonTotal = iPCount * IPStats.DamagePerSwing;
            float deadlyPoisonTotal = dPTicks * DPStats.DamagePerSwing;
            float woundPoisonTotal = wPCount * WPStats.DamagePerSwing;

            float damageTotal = (mainHandDamageTotal + offHandDamageTotal + backstabDamageTotal + hemoDamageTotal + sStrikeDamageTotal + mutiDamageTotal +
                                  rStrikeDamageTotal + ruptDamageTotal + evisDamageTotal + envenomDamageTotal + instantPoisonTotal + deadlyPoisonTotal + woundPoisonTotal) * (1f + kSDmgBonus * kSDuration / duration);
            if (Talents.BanditsGuile > 0)
            {
                float buildupTime = duration / (((CPG == 1 ? CPGCount : 0) + rSCount) * RV.Talents.BanditsGuileChance[Talents.BanditsGuile]);
                float guileBonus = RV.Talents.BanditsGuileStep / buildupTime + 2f * RV.Talents.BanditsGuileStep / buildupTime + 3 * RV.Talents.BanditsGuileStep / RV.Talents.BanditsGuileDuration;
                damageTotal *= 1f + guileBonus;
            }
            if (Spec == 2) //Master of Subtlety specialization
            {
                damageTotal *= 1f + RV.Mastery.MasterOfSubtletyDmgMult * NumberOfStealths * RV.Mastery.MasterOfSubtletyDuration / duration;
            }
            #endregion

            return new RogueRotationCalculation()
            {
                MultipleSegments = duration < 1f,
                Duration = duration,
                TotalDamage = damageTotal,
                DPS = damageTotal / duration,

                MainHandCount = whiteMHAttacks,
                OffHandCount = whiteOHAttacks,
                MGCount = mGAttacks,
                BackstabCount = (CPG == 2 ? CPGCount : 0),
                HemoCount = (CPG == 3 ? CPGCount : 0),
                SStrikeCount = (CPG == 1 ? CPGCount : 0),
                MutiCount = (CPG == 0 ? CPGCount : 0),
                RStrikeCount = rSCount,
                RuptCount = ruptCount,
                EvisCount = evisCount,
                EnvenomCount = envenomCount,
                SnDCount = snDCount,
                RecupCount = recupCount,
                EACount = exposeCount,
                IPCount = iPCount,
                DPCount = dPTicks,
                WPCount = wPCount,

                FinisherCP = finisherCP,
                EvisCP = (finisher == 1 ? Math.Min(_avgCP[finisherCP], 5) : 0),
                EnvenomCP = (finisher == 2 ? Math.Min(_avgCP[finisherCP], 5) : 0),
                RuptCP = ruptCP,
                SnDCP = snDCP,

                MHPoison = mHPoison,
                OHPoison = oHPoison,

                UseTotT = useTotT,
                CutToTheChase = ChanceOnSnDResetOnEvisEnv,
                SerratedBlades = ChanceOnRuptResetonEvisCP,
            };
        }

        public override float getNumberStealths()
        {
            return 1f + Duration / (RV.Vanish.CD - VanishCDReduction) + (StepVanishResetCD > 0 ? Duration / StepVanishResetCD : 0f);
        }

        public override float getEnergyAvailable()
        {
            return RV.BaseEnergy + BonusMaxEnergy + EnergyRegen * Duration +
                (UseTotT ? (-RV.TotT.Cost + ToTTCostReduction) * (Duration - RV.TotT.Duration) / RV.TotT.CD : 0f) +
                (RecupCP > 0 ? Duration / 3 * EnergyOnRecupTick : 0f);
        }

        public override float getCPAvailable()
        {   
            return (Talents.Premeditation > 0 ? RV.Talents.PremeditationBonusCP * Duration / RV.Talents.PremeditationCD : 0) + (Talents.HonorAmongThieves > 0 ? RV.Talents.HonorAmongThievesCPChance[Talents.HonorAmongThieves] * Duration / RV.Talents.HonorAmongThievesCD[Talents.HonorAmongThieves] : 0);
        }

        public override float getCPGEnergy()
        {
            return BackstabStats.EnergyCost;
        }

        public override float getCPPerCPG()
        {
            return BackstabStats.CPPerSwing;
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