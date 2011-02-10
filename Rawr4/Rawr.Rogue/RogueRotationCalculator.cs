using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Rogue
{
    public abstract class RogueRotationCalculator
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
        public RogueAbilityStats RuptStats { get; set; }
        public RogueAbilityStats SnDStats { get; set; }
        public RogueAbilityStats ExposeStats { get; set; }
        public RogueAbilityStats IPStats { get; set; }
        public RogueAbilityStats DPStats { get; set; }
        public RogueAbilityStats WPStats { get; set; }

        public float BonusEnergyRegen { get; set; }
        public float BonusFlurryHaste { get; set; }
        public float BonusHemoDamageMultiplier { get; set; }
        public float IPFrequencyMultiplier { get; set; }
        public float DPFrequencyBonus { get; set; }
        public float BonusMaxEnergy { get; set; }
        public float ChanceOnCPOnSSCrit { get; set; }
        public float ChanceOnEnergyOnCrit { get; set; }
        public float ChanceOnEnergyOnGarrRuptTick { get; set; }
        public float ChanceOnEnergyPerCPFinisher { get; set; }
        public float ChanceOnMHAttackOnSwordAxeHit { get; set; }
        public float ChanceOnNoDPConsumeOnEnvenom { get; set; }
        public float ChanceOnMGAttackOnMHAttack { get; set; }
        public float ChanceOnRuptResetonEvisCP { get; set; }
        public float ChanceOnSnDResetOnEvisEnv { get; set; }
        public float CPG { get; set; }
        public float CPGCount { get; set; }
        public float CPGEnergy { get; set; }
        public float CPPerCPG { get; set; }
        public float CPOnFinisher { get; set; }
        public float EnergyOnBelow35BS { get; set; }
        public float EnergyOnOHAttack { get; set; }
        public float EnergyOnRecupTick { get; set; }
        public float EnergyRegen { get; set; }
        public float EnergyRegenTimeOnDamagingCP { get; set; }
        public float ExposeCPCostMult { get; set; }
        public float FlurryCostReduction { get; set; }
        public float NumberOfStealths { get; set; }
        public float TotalCPAvailable { get; set; }
        public float TotalEnergyAvailable { get; set; }
        public float ToTTCDReduction { get; set; }
        public float ToTTCostReduction { get; set; }
        public float RSBonus { get; set; }
        public bool UseTotT { get; set; }
        public float StepVanishResetCD { get; set; }
        public float VanishCDReduction { get; set; }

        public RogueRotationCalculator(Character character, int spec, Stats stats, CalculationOptionsRogue calcOpts, float hasteBonus, float mainHandSpeed, float offHandSpeed,
            float mainHandSpeedNorm, float offHandSpeedNorm, float avoidedWhiteMHAttacks, float avoidedWhiteOHAttacks, float avoidedMHAttacks, float avoidedOHAttacks, float avoidedFinisherAttacks,
            float avoidedPoisonAttacks, float chanceExtraCPPerHit, float chanceExtraCPPerMutiHit, RogueAbilityStats mainHandStats, RogueAbilityStats offHandStats, RogueAbilityStats ruptStats,
            RogueAbilityStats snDStats, RogueAbilityStats exposeStats, RogueAbilityStats iPStats, RogueAbilityStats dPStats, RogueAbilityStats wPStats)
		{
            Char = character;
            Talents = character.RogueTalents;
            Spec = spec;
			Stats = stats;
            CalcOpts = calcOpts;
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
            RuptStats = ruptStats;
            SnDStats = snDStats;
            ExposeStats = exposeStats;
            IPStats = iPStats;
            DPStats = dPStats;
            WPStats = wPStats;

            #region Talent/Mastery bonuses
            BonusMaxEnergy = spec == 0 && (Char.MainHand == null || Char.OffHand == null ? false : Char.MainHand.Type == ItemType.Dagger && Char.MainHand.Type == ItemType.Dagger) ? RV.Mastery.AssassinsResolveEnergyBonus : 0f;
            StepVanishResetCD = RV.Talents.PreparationCD * Talents.Preparation;
            ChanceOnEnergyOnGarrRuptTick = RV.Talents.VenemousWoundsProcChance * Talents.VenomousWounds;
            ChanceOnNoDPConsumeOnEnvenom = RV.Talents.MasterPoisonerNoDPConsumeChance * Talents.MasterPoisoner;
            ChanceOnMGAttackOnMHAttack = spec == 1 ? RV.Mastery.MainGauche + RV.Mastery.MainGauchePerMast * StatConversion.GetMasteryFromRating(stats.MasteryRating) : 0f;
            ChanceOnSnDResetOnEvisEnv = RV.Talents.CutToTheChaseMult[Talents.CutToTheChase];
            ChanceOnRuptResetonEvisCP = RV.Talents.SerratedBladesChance * Talents.SerratedBlades;
            DPFrequencyBonus = spec == 0 ? RV.Mastery.ImprovedPoisonsDPBonus : 0f;
            ExposeCPCostMult = RV.Talents.ImpExposeArmorCPMult * Talents.ImprovedExposeArmor;
            EnergyOnBelow35BS = RV.Talents.MurderousIntentEnergyRefund * Talents.MurderousIntent;
            EnergyRegenTimeOnDamagingCP = (RV.AR.Duration + (Talents.GlyphOfAdrenalineRush ? RV.Glyph.ARDurationBonus : 0f)) / RV.AR.CD * Talents.AdrenalineRush * RV.Talents.RestlessBladesPerCPCDReduc * Talents.RestlessBlades;
            EnergyOnOHAttack = RV.Talents.CombatPotencyProcChance * RV.Talents.CombatPotencyEnergyBonus * Talents.CombatPotency;
            EnergyOnRecupTick = RV.Talents.EnergeticRecoveryEnergyBonus * Talents.EnergeticRecovery;
            IPFrequencyMultiplier = spec == 0 ? RV.Mastery.ImprovedPoisonsIPFreqMult : 0f;
            ChanceOnEnergyPerCPFinisher = RV.Talents.RelentlessStrikesPerCPChance[Talents.RelentlessStrikes];
            CPOnFinisher = RV.Talents.RuthlessnessChance * Talents.Ruthlessness;
            RSBonus = (RV.RS.FinishMult + (Talents.GlyphOfRevealingStrike ? RV.Glyph.RSFinishMultBonus : 0f)) * Talents.RevealingStrike;
            ToTTCostReduction = (Talents.GlyphOfTricksOfTheTrade ? RV.Glyph.TotTCostReduc : 0f);
            VanishCDReduction = RV.Talents.ElusivenessVanishCDReduc * Talents.Elusiveness;
            #endregion
        }

        public abstract RogueRotationCalculation GetRotationCalculations(float duration, int cPG, int recupCP, int ruptCP, bool useRS, int finisher, int finisherCP, int snDCP, int mHPoison, int oHPoison, bool bleedIsUp, bool useTotT, int exposeCP, bool PTRMode);

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

            public float FinisherCP { get; set; }
            public float EvisCP { get; set; }
            public float EnvenomCP { get; set; }
            public float RuptCP { get; set; }
            public int SnDCP { get; set; }

            public int MHPoison { get; set; }
            public int OHPoison { get; set; }

            public bool UseTotT { get; set; }
            public float CutToTheChase { get; set; }
            public float SerratedBlades { get; set; }

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
                if (RuptCount > 0) rotation.AppendFormat("Ru{0} ", RuptCP);
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
                if (EvisCount > 0 && SerratedBlades * FinisherCP == 1) rotation.AppendFormat("Use {0}cp Rupture, kept up with Eviscerate.\r\n", RuptCP);
                else if (EvisCount > 0 && SerratedBlades > 0) rotation.AppendFormat("Use {0}cp Rupture, partially kept up with Eviscerate.\r\n", RuptCP);
                else if (RuptCount > 0 && RStrikeCount == 0) rotation.AppendFormat("Keep {0}cp Rupture up.\r\n", RuptCP);
                else if (RuptCount > 0 && RStrikeCount > 0) rotation.AppendFormat("Keep {0}cp Rupture up, empowered with Revealing Strike.\r\n", RuptCP);
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

                c.RuptCP = a.RuptCP;
                c.EvisCP = a.EvisCP;
                c.EnvenomCP = a.EnvenomCP;

                c.RotationString = string.Format("Multiple rotations*Before 35%:\r\n{0}\r\nAfter 35%\r\n{1}", a.ToString(), b.ToString());

                return c;
            }
        }

        public virtual float getNumberStealths()
        {
            return 1f + Duration / (RV.Vanish.CD - VanishCDReduction);
        }
        public virtual float getEnergyRegen()
        {
            return RV.BaseEnergyRegen;
        }
        public abstract float getEnergyAvailable();
        public virtual float getCPAvailable()
        {
            return 0f;
        }
        public abstract float getCPGEnergy();
        public abstract float getCPPerCPG();
        public virtual float getSnDCount(float snDDuration)
        {
            return Duration / snDDuration;
        }
        public virtual void processFinisher(float cpRequired, float finisherEnergy)
        {
            float cpgToUse = cpRequired / CPPerCPG;
            CPGCount += cpgToUse;
            TotalEnergyAvailable -= cpgToUse * CPGEnergy + finisherEnergy;
            TotalCPAvailable = 0;
        }
    }
}