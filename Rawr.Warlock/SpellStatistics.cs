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
        /// The cumulative damage total for direct hits.
        /// </summary>
        public double HitDamage { get; set; }
        /// <summary>
        /// The number of successful crits on the target.
        /// </summary>
        public int CritCount { get; set; }
        /// <summary>
        /// The cumulative damage total for crits.
        /// </summary>
        public double CritDamage { get; set; }
        /// <summary>
        /// The number of dot ticks on the target.
        /// </summary>
        public int TickCount { get; set; }
        /// <summary>
        /// The total tick damage inflicted.
        /// </summary>
        public double TickDamage { get; set; }
        /// <summary>
        /// The total mana consumed.
        /// </summary>
        public double ManaUsed { get; set; }
        /// <summary>
        /// The amount of time that a spell was active.
        /// </summary>
        public double ActiveTime { get; set; }

        #region calculation methods
        /// <summary>
        /// The average damage for each hit.
        /// </summary>
        public double HitAverage()
        {
            return (HitDamage / HitCount);
        } 

        /// <summary>
        /// The average damage infliced by each crit.
        /// </summary>
        public double CritAverage()
        {
            return (CritDamage / CritCount);
        }

        /// <summary>
        /// The average tick damage inflicted.
        /// </summary>
        public double TickAverage()
        {
            return (TickDamage / TickCount);
        }

        /// <summary>
        /// The total direct (hits + crits) damage inflicted.
        /// </summary>
        public double DirectDamage()
        {
            return (HitDamage + CritDamage);
        }
        /// <summary>
        /// The total damage that hit, crit or ticked on the target.
        /// </summary>
        public double OverallDamage()
        {
            return (DirectDamage() + TickDamage);
        }
        #endregion
    }
}