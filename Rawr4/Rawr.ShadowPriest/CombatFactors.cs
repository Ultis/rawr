using System;
using Rawr.ShadowPriest.Spells;

namespace Rawr.ShadowPriest
{
    public class CombatFactors : ISpellArgs
    {
        public CombatFactors(PriestTalents talents, Stats stats, int additionalTargets, float latencyCast, float latencyGcd)
        {
            Talents = talents;
            Stats = stats;
            AdditionalTargets = additionalTargets;
            LatencyCast = latencyCast;
            LatencyGCD = latencyGcd;
        }

        #region ISpellArgs Member

        public PriestTalents Talents
        {
            get;
            set;
        }

        public Stats Stats
        {
            get;
            set;
        }

        public float LatencyCast
        {
            get;
            set;
        }

        public float LatencyGCD
        {
            get;
            set;
        }

        public int AdditionalTargets
        {
            get;
            set;
        }

        #endregion

    }
}
