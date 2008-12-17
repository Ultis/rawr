using System.Collections.Generic;

namespace Rawr.Rogue
{
    public class DisplayValue
    {
        //This class keeps the UI labels and calculations in sync in a single class.  
        //By simply adding a new "public static readonly DisplayValue..." we automatically add it to the UI, and make it 
        //availble in our list of values to be displayed.
        private DisplayValue(string grouping, string name)            
        {
            Grouping = grouping;
            Name = name;
            _list.Add(this);
        }

        public readonly string Grouping;
        public readonly string Name;
        private static readonly List<DisplayValue> _list = new List<DisplayValue>();

        public static readonly DisplayValue Health = new DisplayValue("Base Stats", "Health");
        public static readonly DisplayValue Stamina = new DisplayValue("Base Stats", "Stamina");
        public static readonly DisplayValue Strength = new DisplayValue("Base Stats", "Strength");
        public static readonly DisplayValue Agility = new DisplayValue("Base Stats", "Agility");
        public static readonly DisplayValue AttackPower = new DisplayValue("Base Stats", "Attack Power");
        public static readonly DisplayValue Haste = new DisplayValue("Base Stats", "Haste %");
        public static readonly DisplayValue HasteRating = new DisplayValue("Base Stats", "Haste Rating");
        public static readonly DisplayValue ArmorPenetration = new DisplayValue("Base Stats", "Armor Penetration");
        public static readonly DisplayValue ArmorPenetrationRating = new DisplayValue("Base Stats", "Armor Penetration Rating");
        public static readonly DisplayValue MhWeaponDamage = new DisplayValue("Base Stats", "MH Weapon Damage");
        public static readonly DisplayValue OhWeaponDamage = new DisplayValue("Base Stats", "OH Weapon Damage");
        public static readonly DisplayValue HitRating = new DisplayValue("Base Stats", "Hit Rating");
        public static readonly DisplayValue HitPercent = new DisplayValue("Base Stats", "Hit %");
        public static readonly DisplayValue MhExpertise = new DisplayValue("Base Stats", "MH Expertise");
        public static readonly DisplayValue OhExpertise = new DisplayValue("Base Stats", "OH Expertise");
        public static readonly DisplayValue BaseMhCrit = new DisplayValue("Base Stats", "Base MH Crit %");
        public static readonly DisplayValue BaseOhCrit = new DisplayValue("Base Stats", "Base OH Crit %");

        public static readonly DisplayValue CPG = new DisplayValue("DPS Breakdown", "CPG");
        public static readonly DisplayValue CpgCrit = new DisplayValue("DPS Breakdown", "CPG Crit %");
        public static readonly DisplayValue CycleTime = new DisplayValue("DPS Breakdown", "Cycle Time (in seconds)");
        public static readonly DisplayValue CPGDPS = new DisplayValue("DPS Breakdown", "CPG DPS");
        public static readonly DisplayValue FinisherDPS = new DisplayValue("DPS Breakdown", "Finisher DPS");
        public static readonly DisplayValue SwordSpecDPS = new DisplayValue("DPS Breakdown", "Sword Spec DPS");
        public static readonly DisplayValue PoisonDPS = new DisplayValue("DPS Breakdown", "Poison DPS");
        public static readonly DisplayValue WhiteDPS = new DisplayValue("DPS Breakdown", "White DPS");
        public static readonly DisplayValue TotalDPS = new DisplayValue("DPS Breakdown", "Total DPS");

        public override string ToString()
        {
            return Name;
        }

        public static string[] GroupedList()
        {
            return _list.ConvertAll<string>(GroupedNameConverter).ToArray();
        }

        private static string GroupedNameConverter(DisplayValue displayValue)
        {
            return string.Format("{0}:{1}", displayValue.Grouping, displayValue.Name); 
        }
    }
}