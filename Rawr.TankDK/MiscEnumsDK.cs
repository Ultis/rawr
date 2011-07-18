using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Rawr.TankDK
{
    public struct TankDKChar
    {
        public Character Char;
        public CalculationOptionsTankDK calcOpts;
        public BossOptions bo;
        //public CombatTable ct;
        //public Rotation Rot;
    }

    public enum SurvivalSub : int { Physical=0, Bleed=1, Magic=2 }
    public enum MitigationSub : int { 
        Crit=0, Haste, Avoidance,  // Damage Avoided
        DamageReduction, Magic, AMS, Armor, Impedences, // Damage Reduced.
        Heals, // Damage Removed.
    }

    enum GemQuality
    {
        Uncommon,
        Rare,
        Epic,
        Jewelcraft
    }

    public enum SMTSubPoints
    {
        Mitigation,
        Survivability,
        Burst,
        Threat
    }
}
