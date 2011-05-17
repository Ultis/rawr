namespace Rawr.Rogue {
    public static class RV
    {
        public const float APperAgi = 2f;
        public const float APperDPS = 14f;
        public const float BaseEnergy = 100f;
        public const float BaseEnergyRegen = 10f;
        public const float BaseStatCalcReduc = 20f;
        public const float CritDmgMult = 2f;
        public const float CritDmgMultPoison = 1.5f;
        public const float EnergyReturnOnAvoid = 0.8f;
        public const float GlanceMult = 0.75f;
        public const float HPPerStam = 14f;
        public const float LeatherSpecialization = 0.05f;
        public const float MaxCP = 5f;
        public const float OHDmgReduc = -0.5f;
        public const float WeapSpeedNorm = 2.4f;
        public const float WeapSpeedNormDagger = 1.7f;
        public static class Mastery
        {
            public const float AmbidexterityDmgMult = 0.75f;
            public const float AssassinsResolveEnergyBonus = 20f;
            public const float AssassinsResolveMeleeDmgBonus = 0.15f;
            public const float Executioner = 0.16f;
            public const float ExecutionerPerMast = 0.02f;
            public const float ImprovedPoisonsDPBonus = 0.2f;
            public const float ImprovedPoisonsIPFreqMult = 0.5f;
            public const float MainGauche = 0.16f;
            public const float MainGauchePerMast = 0.02f;
            public const float MasterOfSubtletyDmgMult = 0.1f;
            public const float MasterOfSubtletyDuration = 6f;
            public const float PotentPoisonsDmgMult = 0.28f;
            public const float PotentPoisonsDmgMultPerMast = 0.035f;
            public const float SinisterCallingMult = 0.25f;
            public const float VitalityAPMult = 0.2f;
            public const float VitalityRegenMult = 0.25f;
        }
        public static class Talents
        {
            public static float[] AggressionDmgMult = new float[] { 0f, 0.07f, 0.14f, 0.2f };
            public static float[] BanditsGuileChance = new float[] { 0f, 0.33f, 0.67f, 1f };
            public const float BanditsGuileStep = 0.1f;
            public const float BanditsGuileDuration = 15f;
            public const float CombatPotencyProcChance = 0.2f;
            public const float CombatPotencyEnergyBonus = 5f;
            public static float[] CoupDeGraceMult = new float[] { 0f, 0.07f, 0.14f, 0.2f };
            public static float[] CutToTheChaseMult = new float[] { 0f, 0.33f, 0.67f, 1f };
            public const float ElusivenessVanishCDReduc = 30f;
            public const float EnergeticRecoveryEnergyBonus = 4f;
            public const float FindWeaknessArmorIgnore = 0.35f;
            public const float FindWeaknessDuration = 10f;
            public const float HonorAmongThievesCritBonus = 0.05f;
            public static float[] HonorAmongThievesCPChance = new float[] { 0, 0.33f, 0.66f, 1f };
            public static float[] HonorAmongThievesCD = new float[] { 0, 4f, 3f, 2f };
            public const float ImpAmbushCritBonus = 0.2f;
            public const float ImpAmbushDmgMult = 0.05f;
            public const float ImpExposeArmorCPMult = 0.5f;
            public const float ImpSinisterStrikeCostReduc = 2f;
            public const float ImpSinisterStrikeDmgMult = 0.1f;
            public const float ImpSliceAndDice = 0.25f;
            public const float InitiativeChance = 0.5f;
            public const float LethalityCritMult = 0.1f;
            public const float LightningReflexesSpeedMult = 0.02f;
            public const float MasterPoisonerNoDPConsumeChance = 1f;
            public const float MasterPoisonerSpellDmgMult = 0.08f;
            public const float MurderousIntentThreshold = 0.35f;
            public const float MurderousIntentEnergyRefund = 15f;
            public const float OpportunityDmgMult = 0.1f;
            public const float OverkillRegenDuration = 20f;
            public const float OverkillRegenMult = 0.3f;
            public const float PremeditationBonusCP = 2f;
            public const float PremeditationCD = 20f;
            public const float PrecisionMult = 0.02f;
            public const float PreparationCD = 300f;
            public const float PuncturingWoundsBSCritMult = 0.1f;
            public const float PuncturingWoundsMutiCritMult = 0.05f;
            public const float RelentlessStrikesEnergyBonus = 25f;
            public static float[] RelentlessStrikesPerCPChance = new float[] { 0, 0.07f, 0.14f, 0.2f };
            public const float RestlessBladesPerCPCDReduc = 1f;
            public const float RuthlessnessChance = 0.2f;
            public const float SanguinaryVein = 0.05f;
            public const float SavageCombatMult = 0.02f;
            public const float SealFateChance = 0.50f;
            public const float SerratedBladesChance = 0.1f;
            public static float[] SlaughterFTShadowsBSAmbushCostReduc = new float[] { 0, 7f, 14f, 20f };
            public const float SlaughterFTShadowsHemoCostReduc = 2f;
            public const float VenomousWoundsEnergy = 10f;
            public const float VenomousWoundsBonusDmg = 675f;
            public const float VenomousWoundsAPMult = 0.176f;
            public const float VenomousWoundsProcChance = 0.3f;
            public static float[] VilePoisonsDmgMult = new float[] { 0, 0.07f, 0.14f, 0.2f };
        }
        public static class Glyph
        {
            public const float ARDurationBonus = 5f;
            public const float BSEnergyOnCrit = 5f;
            public const float EvisCritMult = 0.1f;
            public const float ExposeBonusDuration = 12f;
            public const float KSDmgMultBonus = 0.1f;
            public const float MutiCostReduc = 5f;
            public const float RuptBonusDuration = 4f;
            public const float RSFinishMultBonus = 0.1f;
            public const float SDBonusDuration = 2f;
            public const float SnDBonusDuration = 6f;
            public const float SSCPBonusChance = 0.2f;
            public const float TotTCostReduc = TotT.Cost;
            public const float VendettaDurationMult = 0.2f;
        }
        public static class Set
        {
            public const float T112CritBonus = 0.05f;
            public const float T114ProcChance = 0.01f;
            public const float T114Duration = 15f;
        }
        public static class AR
        {
            public const float MeleeSpeedMult = 0.2f;
            public const float EnergyRegenMult = 1f;
            public const float Duration = 15f;
            public const float CD = 180f;
        }
        public static class Ambush
        {
            public const float Cost = 60f;
            public const float WeapDmgMult = 1.9f;
            public const float DaggerDmgMult = 1.447f;
            public const float BonusDmg = 367f;
        }
        public static class BS
        {
            public const float Cost = 60f;
            public const float WeapDmgMult = 2f;
            public const float BonusDmg = 690f;
        }
        public static class ColdBlood
        {
            public const float EnergyBonus = 25f;
            public const float CD = 120f;
        }
        public static class DP
        {
            public const float BaseDmg = 540f;
            public const float APMult = 0.14f;
            public const float Chance = 0.3f;
            public const float TickTime = 3f;
            public const float MaxStack = 5f;
            public const float Duration = 12f;
        }
        public static class Envenom
        {
            public const float Cost = 35f;
            public const float BaseDmg = 0f;
            public const float CPBaseDmg = 240f;
            public const float CPAPMult = 0.09f;
            public const float BuffDuration = 1f;
            public const float BuffDurationPerCP = 1f;
            public const float BuffDPChanceBonus = 0.15f;
            public const float BuffIPChanceMult = 0.75f;
        }
        public static class Evis
        {
            public const float Cost = 35f;
            private const float BaseMinDmg = 177f;
            private const float BaseMaxDmg = 531f;
            public const float BaseAvgDmg = (BaseMinDmg + BaseMaxDmg) / 2;
            public const float CPBaseDmg = 517f;
            public const float CPAPMult = 0.091f;
        }
        public static class Expose
        {
            public const float Cost = 25f;
            public const float BaseDuration = 0f;
            public const float DurationPerCP = 10f;
            public const float ArmorReduc = 0.12f;

        }
        public static class Garrote
        {
            public const float Cost = 45f;
        }
        public static class Hemo
        {
            public const float Cost = 35f;
            public const float WeapDmgMult = 1.1f;
            public const float DaggerDmgMult = 0.45f;
            public const float BleedDmgMult = 0.3f;
            public const float DebuffDuration = 60f;
        }
        public static class IP
        {
            private const float BaseMinDmg = 285f;
            private const float BaseMaxDmg = 377f;
            public const float BaseAvgDmg = 352f;
            public const float APMult = 0.09f;
            public const float Chance = 0.2f;
            public const float NormWeapSpeed = 1.4f;
        }
        public static class KS
        {
            public const float DmgMult = 0.2f;
            public const float CD = 120f;
            public const float StrikeCount = 5f;
            private const float StrikeInterval = 0.5f;
            public const float Duration = StrikeCount * StrikeInterval;
        }
        public static class Muti
        {
            public const float Cost = 60f;
            public const float WeapDmgMult = 1.5f;
            public const float BonusDmg = 201;
        }
        public static class Recup
        {
            public const float Cost = 30f;
            public const float BaseDuration = 0f;
            public const float DurationPerCP = 6f;
        }
        public static class RS
        {
            public const float Cost = 40f;
            public const float WeapDmgMult = 1.25f;
            public const float FinishMult = 0.2f;
        }
        public static class Rupt
        {
            public const float Cost = 25f;
            public const float BaseDuration = 6f;
            public const float DurationPerCP = 2f;
            public const float BaseDmg = 141f;
            public const float TickBaseDmg = 20f;
            public static float[] TickAPMult = new float[6] {0, 0.015f, 0.024f, 0.03f, 0.03428571f, 0.0375f};
            public const float TickTime = 2f;
        }
        public static class SD
        {
            public const float Duration = 6f;
            public const float CD = 60f;
        }
        public static class SnD
        {
            public const float Cost = 25f;
            public const float BaseDuration = 6f;
            public const float DurationPerCP = 3f;
            public const float SpeedBonus = 0.4f;
        }
        public static class SS
        {
            public const float Cost = 45f;
            public const float BonusDmg = 200f;
        }
        public static class TotT
        {
            public const float Cost = 15f;
            public const float Duration = 6f;
            public const float CD = 30f;
        }
        public static class Vanish
        {
            public const float CD = 180f;
        }
        public static class WP
        {
            public const float BaseDmg = 258f;
            public const float APMult = 0.036f;
            public const float Chance = 0.5f;
            public const float NormWeapSpeed = 1.4f;
        }
        public static class Vendetta
        {
            public const float Duration = 30f;
            public const float DmgMult = 0.2f;
            public const float CD = 120f;
        }
        public static class Racial
        {
            public const float HumanExpBonus = 3f;
            public const float DwarfExpBonus = 3f;
            public const float OrcExpBonus = 3f;
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