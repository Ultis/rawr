namespace Rawr.Rogue {
    public static class RV
    {
        public static float APperAgi = 2f;
        public static float APperDPS = 14f;
        public static float BaseStatCalcReduc = 10f;
        public static float CritDmgMult = 2f;
        public static float CritDmgMultPoison = 1.5f;
        public static float EnergyReturnOnAvoid = 0.8f;
        public static float GlanceMult = 0.75f;
        public static float LeatherSpecialization = 0.05f;
        public static float OHDmgReduc = 0.5f;
        public static float WeapSpeedNorm = 2.4f;
        public static float WeapSpeedNormDagger = 1.7f;
        public static class Mastery
        {
            public static float AssassinsResolve = 0.15f;
            public static float PotentPoisonsDmgMult = 0.28f;
            public static float PotentPoisonsDmgMultPerMast = 0.035f;
            public static float SinisterCallingMult = 0.25f;
            public static float VitalityAPMult = 0.2f;
        }
        public static class Talents
        {
            public static float[] AggressionDmgMult = new float[] { 0, 0.07f, 0.14f, 0.2f };
            public static float[] CoupDeGrace = new float[] { 0, 0.07f, 0.14f, 0.2f };
            public static float ImpSinisterStrikeCostReduc = 2f;
            public static float ImpSinisterStrikeDmgMult = 0.1f;
            public static float ImpSliceAndDice = 0.25f;
            public static float LethalityCritMult = 0.1f;
            public static float LightningReflexesMult = 0.02f;
            public static float MurderousIntentThreshold = 0.35f;
            public static float OpportunityDmgMult = 0.1f;
            public static float PrecisionMult = 0.02f;
            public static float PuncturingWoundsBSCritMult = 0.1f;
            public static float PuncturingWoundsMutiCritMult = 0.05f;
            public static float RelentlessStrikesEnergy = 25f;
            public static float SavageCombatMult = 0.02f;
            public static float SealFateChance = 0.50f;
            public static float SlaughterFTShadowsBSAmbushCostReduc = 7f;
            public static float SlaughterFTShadowsHemoCostReduc = 2f;
            public static float VenemousWoundsEnergy = 10f;
            public static float VenemousWoundsBonusDmg = 675f;
            public static float VenemousWoundsProcChance = 0.3f;
            public static float[] VilePoisonsDmgMult = new float[] { 0, 0.07f, 0.14f, 0.2f };
        }
        public static class Glyph
        {
            public static float BSEnergyOnCrit = 5f;
            public static float EvisCritMult = 0.1f;
            public static float ExposeBonusDuration = 12f;
            public static float MutiCostReduc = 5f;
            public static float RuptBonusDuration = 4f;
            public static float SnDBonusDuration = 6f;
            public static float SSCPBonusChance = 0.2f;
            public static float VendettaDurationMult = 0.2f;
        }
        public static class AR
        {
            public static float Duration = 15f;
            public static float MeleeSpeedMult = 0.2f;
            public static float CD = 180f;
        }
        public static class Ambush
        {
            public static float Cost = 60f;
        }
        public static class BS
        {
            public static float Cost = 60f;
            public static float WeapDmgMult = 2f;
            public static float BonusDmg = 690f;
        }
        public static class Envenom
        {
            public static float Cost = 35f;
            public static float BaseDmg = 0f;
            public static float TickBaseDmg = 240f;
            public static float TickAPMult = 0.09f;
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
            public static float DaggerDmgMult = 1.595f;
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
        public static class DP
        {
            public static float BaseDmg = 508f;
            public static float APMult = 0.108f;
        }
        public static class IP
        {
            private static float BaseMinDmg = 285f;
            private static float BaseMaxDmg = 377f;
            public static float BaseAvgDmg = (BaseMinDmg + BaseMaxDmg) / 2f;
            public static float APMult = 0.09f;
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
    }
}