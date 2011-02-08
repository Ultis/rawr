namespace Rawr.Rogue {
    public static class RV
    {
        public static float APperAgi = 2f;
        public static float APperDPS = 14f;
        public static float BaseEnergy = 100f;
        public static float BaseEnergyRegen = 10f;
        public static float BaseStatCalcReduc = 20f;
        public static float CritDmgMult = 2f;
        public static float CritDmgMultPoison = 1.5f;
        public static float EnergyReturnOnAvoid = 0.8f;
        public static float GlanceMult = 0.75f;
        public static float HPPerStam = 14f;
        public static float LeatherSpecialization = 0.05f;
        public static float OHDmgReduc = 0.5f;
        public static float WeapSpeedNorm = 2.4f;
        public static float WeapSpeedNormDagger = 1.7f;
        public static class Mastery
        {
            public static float AmbidexterityDmgMult = 0.75f;
            public static float AssassinsResolveEnergyBonus = 20f;
            public static float AssassinsResolveMeleeDmgBonus = 0.15f;
            public static float Executioner = 0.16f;
            public static float ExecutionerPerMast = 0.02f;
            public static float ImprovedPoisonsDPBonus = 0.2f;
            public static float ImprovedPoisonsIPFreqMult = 0.5f;
            public static float MainGauche = 0.16f;
            public static float MainGauchePerMast = 0.02f;
            public static float MasterOfSubtletyDmgMult = 0.1f;
            public static float MasterOfSubtletyDuration = 6f;
            public static float PotentPoisonsDmgMult = 0.28f;
            public static float PotentPoisonsDmgMultPerMast = 0.035f;
            public static float SinisterCallingMult = 0.25f;
            public static float VitalityAPMult = 0.2f;
            public static float VitalityRegenMult = 0.25f;
        }
        public static class Talents
        {
            public static float[] AggressionDmgMult = new float[] { 0f, 0.07f, 0.14f, 0.2f };
            public static float[] BanditsGuileChance = new float[] { 0f, 0.33f, 0.67f, 1f };
            public static float BanditsGuileStep = 0.1f;
            public static float BanditsGuileDuration = 15f;
            public static float CombatPotencyProcChance = 0.2f;
            public static float CombatPotencyEnergyBonus = 5f;
            public static float[] CoupDeGraceMult = new float[] { 0f, 0.07f, 0.14f, 0.2f };
            public static float[] CutToTheChaseMult = new float[] { 0f, 0.33f, 0.67f, 1f };
            public static float ElusivenessVanishCDReduc = 30f;
            public static float EnergeticRecoveryEnergyBonus = 4f;
            public static float HonorAmongThievesCritBonus = 0.05f;
            public static float[] HonorAmongThievesCPChance = new float[] { 0, 0.33f, 0.66f, 1f };
            public static float[] HonorAmongThievesCD = new float[] { 0, 4f, 3f, 2f };
            public static float ImpAmbushCritBonus = 0.2f;
            public static float ImpAmbushDmgMult = 0.05f;
            public static float ImpExposeArmorCPMult = 0.5f;
            public static float ImpSinisterStrikeCostReduc = 2f;
            public static float ImpSinisterStrikeDmgMult = 0.1f;
            public static float ImpSliceAndDice = 0.25f;
            public static float InitiativeChance = 0.5f;
            public static float LethalityCritMult = 0.1f;
            public static float LightningReflexesSpeedMult = 0.02f;
            public static float MasterPoisonerNoDPConsumeChance = 1f;
            public static float MasterPoisonerSpellDmgMult = 0.08f;
            public static float MurderousIntentThreshold = 0.35f;
            public static float MurderousIntentEnergyRefund = 15f;
            public static float OpportunityDmgMult = 0.1f;
            public static float OverkillRegenDuration = 20f;
            public static float OverkillRegenMult = 0.3f;
            public static float PremeditationBonusCP = 2f;
            public static float PremeditationCD = 20f;
            public static float PrecisionMult = 0.02f;
            public static float PreparationCD = 300f;
            public static float PuncturingWoundsBSCritMult = 0.1f;
            public static float PuncturingWoundsMutiCritMult = 0.05f;
            public static float RelentlessStrikesEnergyBonus = 25f;
            public static float[] RelentlessStrikesPerCPChance = new float[] { 0, 0.07f, 0.14f, 0.2f };
            public static float RestlessBladesPerCPCDReduc = 1f;
            public static float RuthlessnessChance = 0.2f;
            public static float SanguinaryVein = 0.05f;
            public static float SavageCombatMult = 0.02f;
            public static float SealFateChance = 0.50f;
            public static float SerratedBladesChance = 0.1f;
            public static float[] SlaughterFTShadowsBSAmbushCostReduc = new float[] { 0, 7f, 14f, 20f };
            public static float SlaughterFTShadowsHemoCostReduc = 2f;
            public static float VenemousWoundsEnergy = 10f;
            public static float VenemousWoundsBonusDmg = 675f;
            public static float VenemousWoundsProcChance = 0.3f;
            public static float[] VilePoisonsDmgMult = new float[] { 0, 0.07f, 0.14f, 0.2f };
        }
        public static class Glyph
        {
            public static float ARDurationBonus = 5f;
            public static float BSEnergyOnCrit = 5f;
            public static float EvisCritMult = 0.1f;
            public static float ExposeBonusDuration = 12f;
            public static float KSDmgMultBonus = 0.1f;
            public static float MutiCostReduc = 5f;
            public static float RuptBonusDuration = 4f;
            public static float RSFinishMultBonus = 0.1f;
            public static float SnDBonusDuration = 6f;
            public static float SSCPBonusChance = 0.2f;
            public static float TotTCostReduc = TotT.Cost;
            public static float VendettaDurationMult = 0.2f;
        }
        public static class AR
        {
            public static float MeleeSpeedMult = 0.2f;
            public static float EnergyRegenMult = 1f;
            public static float Duration = 15f;
            public static float CD = 180f;
        }
        public static class Ambush
        {
            public static float Cost = 60f;
            public static float WeapDmgMult = 1.9f;
            public static float DaggerDmgMult = 1.447f;
            public static float BonusDmg = 367f;
        }
        public static class BS
        {
            public static float Cost = 60f;
            public static float WeapDmgMult = 2f;
            public static float BonusDmg = 690f;
        }
        public static class ColdBlood
        {
            public static float EnergyBonus = 25f;
            public static float CD = 120f;
        }
        public static class Envenom
        {
            public static float Cost = 35f;
            public static float BaseDmg = 0f;
            public static float TickBaseDmg = 240f;
            public static float TickAPMult = 0.09f;
            public static float BuffDuration = 1f;
            public static float BuffDurationPerCP = 1f;
            public static float BuffDPChanceBonus = 0.15f;
            public static float BuffIPChanceMult = 0.75f;
        }
        public static class Evis
        {
            public static float Cost = 35f;
            private static float BaseMinDmg = 177f;
            private static float BaseMaxDmg = 531f;
            public static float BaseAvgDmg = (BaseMinDmg + BaseMaxDmg) / 2;
            public static float TickBaseDmg = 517f;
            public static float TickAPMult = 0.091f;
        }
        public static class Expose
        {
            public static float Cost = 25f;
            public static float BaseDuration = 0f;
            public static float DurationPerCP = 10f;
            public static float ArmorReduc = 0.12f;

        }
        public static class Garrote
        {
            public static float Cost = 45f;
        }
        public static class Hemo
        {
            public static float Cost = 35f;
            public static float WeapDmgMult = 1.1f;
            public static float DaggerDmgMult = 0.45f;
            public static float BleedDmgMult = 0.3f;
        }
        public static class KS
        {
            public static float DmgMult = 0.2f;
            public static float CD = 120f;
            public static float StrikeCount = 5f;
            private static float StrikeInterval = 0.5f;
            public static float Duration = StrikeCount * StrikeInterval;
        }
        public static class Muti
        {
            public static float Cost = 60f;
            public static float WeapDmgMult = 1.5f;
            public static float BonusDmg = 201;
        }
        public static class Recup
        {
            public static float Cost = 30f;
            public static float BaseDuration = 0f;
            public static float DurationPerCP = 6f;
        }
        public static class RS
        {
            public static float Cost = 40f;
            public static float WeapDmgMult = 1.25f;
            public static float FinishMult = 0.2f;
        }
        public static class Rupt
        {
            public static float Cost = 25f;
            public static float BaseDuration = 6f;
            public static float DurationPerCP = 2f;
            public static float BaseDmg = 141f;
            public static float TickBaseDmg = 20f;
            public static float[] TickAPMult = new float[6] {0, 0.015f, 0.024f, 0.03f, 0.03428571f, 0.0375f};
        }
        public static class SnD
        {
            public static float Cost = 25f;
            public static float BaseDuration = 6f;
            public static float DurationPerCP = 3f;
            public static float SpeedBonus = 0.4f;
        }
        public static class SS
        {
            public static float Cost = 45f;
            public static float BonusDmg = 200f;
        }
        public static class TotT
        {
            public static float Cost = 15f;
            public static float Duration = 6f;
            public static float CD = 30f;
        }
        public static class Vanish
        {
            public static float CD = 180f;
        }
        public static class DP
        {
            public static float BaseDmg = 508f;
            public static float APMult = 0.108f;
            public static float Chance = 0.3f;
            public static float TickTime = 3f;
            public static float MaxStack = 5f;
        }
        public static class IP
        {
            private static float BaseMinDmg = 285f;
            private static float BaseMaxDmg = 377f;
            public static float BaseAvgDmg = (BaseMinDmg + BaseMaxDmg) / 2f;
            public static float APMult = 0.09f;
            private static float PPM = 8.53f;
            public static float PPS = PPM / 60f;
        }
        public static class WP
        {
            public static float BaseDmg = 258f;
            public static float APMult = 0.036f;
        }
        public static class Vendetta
        {
            public static float Duration = 30f;
            public static float DmgMult = 0.2f;
            public static float CD = 120f;
        }
        public static class Racial
        {
            public static float HumanExpBonus = 3f;
            public static float DwarfExpBonus = 3f;
            public static float OrcExpBonus = 3f;
        }
        public static float GetMissedDPTicks(float stackTime)
        {
            float possibleTicks = DP.MaxStack * stackTime / DP.TickTime;
            float timeBetweenStacks = stackTime / DP.MaxStack;
            float ticks = 0f;
            float i = DP.MaxStack - 1f;
            while (i > 0)
            {
                ticks += i * timeBetweenStacks / DP.TickTime;
                i--;
            }
            return possibleTicks - ticks;
        }
    }
}