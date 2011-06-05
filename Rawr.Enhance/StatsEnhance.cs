using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
    public class StatsEnhance : Stats
    {
        public float ConcussionMultiplier { get; set; }
        public float ShieldBonus { get; set; }
        public float CallofFlameBonus { get; set; }
        public float BonusWindfuryDamageMultiplier { get; set; }
        public float BonusStormstrikeDamageMultiplier { get; set; }
        public float BonusLavalashDamageMultiplier { get; set; }

        public float FTSpellPowerMultiplier { get; set; }
    }
}
