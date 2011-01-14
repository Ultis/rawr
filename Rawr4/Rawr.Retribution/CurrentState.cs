
namespace Rawr.Retribution
{
    public static class CurrentState
    {

        /// <summary>
        /// Equipped weapon with integral bonuses, Gems and enchants
        /// </summary>
        public static float WeaponDamage
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        /// <summary>
        /// max weapon base speed as show in tooltip, whithout haste, etc
        /// </summary>
        public static float MaxWeaponSpeed { get; set; }
        /// <summary>
        /// Current base crit with buffs, gems and enchants
        /// </summary>
        public static float CritChance
        {
            get { throw new System.NotImplementedException(); }
            set { CritChance = value; }
        }
        /// <summary>
        /// Attack power with buffs etc
        /// </summary>
        public static float AttackPower
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        /// <summary>
        /// single calc for the old 14 AP = 1 DPS
        /// </summary>
        public static float AP14 = AttackPower / 14;
        public static float SpellPower { get; set; }
        public static float HitChance
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        public static int HolyPowerCount { get; set; }
        public static int CensureCount { get; set; }
        public static float[] ActiveBonuses
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        /// <summary>
        /// Used after an change
        /// </summary>
        public static void UpdateState()
        {
            throw new System.NotImplementedException();
        }
        private static void CalculateWeaponDamage()
        {
            //weapon damage = base weapon + gem bonuses + Enchant bonuses
        }
        private static void CalculateAttackPower()
        {
            //(Strength - 10) * 2 + level * 3 + Enchant bonus totals + AP Bonus from gear + J15) * (1 + %AP from buffs)
        }
        private static void CalculateMeleeCrit()
        {
        }
    }

}
