namespace Rawr.Warlock
{
    /// <summary>
    /// A class to track our combat statistics.
    /// </summary>
    public class SpellStatistics
    {
        /// <summary>
        /// The total number of spell casts.
        /// </summary>
        public int CastCount { get; set; }
        /// <summary>
        /// The number of spells that missed due to low hit rating.
        /// </summary>
        public int MissCount { get; set; }
        /// <summary>
        /// Total damage from missed spells.
        /// </summary>
        public double MissTotal { get; set; }
        /// <summary>
        /// The number of successful (non-critical) hits on the target.
        /// </summary>
        public int HitCount { get; set; }
        /// <summary>
        /// The average damage for each hit.
        /// </summary>
        public double HitAverage { get; set; }
        /// <summary>
        /// The cumulative hit damage.
        /// </summary>
        public double HitDamage { get; set; }
        /// <summary>
        /// The number of successful crits on the target.
        /// </summary>
        public int CritCount { get; set; }
        /// <summary>
        /// The average damage infliced by each crit.
        /// </summary>
        public double CritAverage { get; set; }
        /// <summary>
        /// The cumulative damage total for crits.
        /// </summary>
        public double CritDamage { get; set; }
        /// <summary>
        /// The number of dot ticks on the target.
        /// </summary>
        public int TickCount { get; set; }
        /// <summary>
        /// The average tick damage inflicted.
        /// </summary>
        public double TickAverage { get; set; }
        /// <summary>
        /// The total tick damage inflicted.
        /// </summary>
        public double TickDamage { get; set; }
        /// <summary>
        /// The total direct damage inflicted.
        /// </summary>
        public double DirectDamage { get; set; }
        /// <summary>
        /// The total damage that hit, crit or ticked on the target.
        /// </summary>
        public double OverallDamage { get; set; }
        /// <summary>
        /// The total mana consumed.
        /// </summary>
        public double ManaUsed { get; set; }

        //public double HitChance { get; set; }
        //public double CooldownReset { get; set; }
    }
}