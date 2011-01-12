namespace Rawr.Rogue {
    public static class RV
    {
        public static float APperDPS = 14f;
        public static float OHDmgReduc = 0.5f;
        public static class Talents
        {
            public static float VWBonusDmg = 675f;
        }
        public static class BS
        {
            public static float WeapDmgMult = 2f;
            public static float BonusDmg = 690f;
        }
        public static class Envenom
        {
            public static float BaseDmg = 0f;
            public static float TickBaseDmg = 240f;
            public static float TickAPMult = 0.09f;
        }
        public static class Evis
        {
            private static float BaseMinDmg = 177f;
            private static float BaseMaxDmg = 531f;
            public static float BaseAvgDmg = (BaseMinDmg + BaseMaxDmg) / 2;
            public static float TickBaseDmg = 517f;
            public static float TickAPMult = 0.091f;
        }
        public static class Hemo
        {
            public static float WeapDmgMult = 1.1f;
            public static float DaggerDmgMult = 1.595f;
        }
        public static class Muti
        {
            public static float WeapDmgMult = 1.5f;
            public static float BonusDmg = 201;
        }
        public static class RS
        {
            public static float WeapDmgMult = 1.25f;
        }
        public static class Rupt
        {
            public static float BaseDmg = 141f;
            public static float TickBaseDmg = 20f;
            public static float[] TickAPMult = new float[6] {0, 0.015f, 0.024f, 0.03f, 0.03428571f, 0.0375f};
        }
        public static class SS
        {
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
    }
}