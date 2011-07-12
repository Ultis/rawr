using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Base
{
#if SILVERLIGHT
    public class StatsHunter : Stats
#else
    public unsafe class StatsPaladin : Stats
#endif
    {
        #region ===== Additive Stats ==================
        public float BonusPetCritChance { get; set; }
        public float PetStamina { get; set; }
        public float PetStrength { get; set; }
        public float PetSpirit { get; set; }
        public float PetAttackPower { get; set; }
        #endregion
        #region ===== Multiplicative Stats ============
        public float BonusRangedAttackPowerMultiplier { get; set; }
        public float BonusPetAttackPowerMultiplier { get; set; }
        public float BonusPetDamageMultiplier { get; set; }
        #endregion

        public bool Tier_11_2pc { get; set; }
        public bool Tier_11_4pc { get; set; }
        public bool Tier_12_2pc { get; set; }
        public bool Tier_12_4pc { get; set; }
    }
}