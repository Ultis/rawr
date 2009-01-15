using System.Collections.Generic;

namespace Rawr.DPSWarr
{
    public class DisplayValue
    {
        //MultiPurpose Enum-Class.  The ("Base Stats", "Health") along with the GroupedList() method below, along
        //with CalculationsRogue.CharacterDisplayCalculationLabels tell the UI to create a group called "Base Stats" and put 
        //in a label named "Health".  The CharacterCalculationsRogue.GetCharacterDisplayCalculationValues method maps a 
        //string (key) to a value.  The UI will look for a key named "Health", and display the corresponding value.  By using 
        //this class as the stand-in for the string, we can keep the UI labels and key-value pairs in sync in a single class.
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

        public static readonly DisplayValue Strength = new DisplayValue("Base Stats", "Strength");
        public static readonly DisplayValue Agility = new DisplayValue("Base Stats", "Agility");
        public static readonly DisplayValue AttackPower = new DisplayValue("Base Stats", "Attack Power");
        public static readonly DisplayValue Haste = new DisplayValue("Base Stats", "Haste Rating");
        public static readonly DisplayValue ArmorPenetration = new DisplayValue("Base Stats", "Armor Penetration");
        public static readonly DisplayValue HitPercent = new DisplayValue("Base Stats", "Hit Rating");
        public static readonly DisplayValue CritPercent = new DisplayValue("Base Stats", "Crit Rating");
        public static readonly DisplayValue MhExpertise = new DisplayValue("Base Stats", "MH Expertise");
        public static readonly DisplayValue OhExpertise = new DisplayValue("Base Stats", "OH Expertise");
        public static readonly DisplayValue Armor = new DisplayValue("Base Stats", "Armor");

        public static readonly DisplayValue buffedMhCrit = new DisplayValue("Buffed Stats", "Base MH Crit");
        public static readonly DisplayValue buffedOhCrit = new DisplayValue("Buffed Stats", "Base OH Crit");
        public static readonly DisplayValue buffedArmorPenetration = new DisplayValue("Buffed Stats", "Effective Boss Armor");
        public static readonly DisplayValue damageReduc = new DisplayValue("Buffed Stats", "Effective Damage Dealt");
        
        //Fury Dps Moves
        public static readonly DisplayValue Bloodsurge = new DisplayValue("DPS Breakdown", "Bloodsurge");
        public static readonly DisplayValue Bloodthirst = new DisplayValue("DPS Breakdown", "Bloodthirst");
        public static readonly DisplayValue Whirlwind = new DisplayValue("DPS Breakdown", "Whirlwind");
        //Arms Dps Moves
        public static readonly DisplayValue MortalStrike = new DisplayValue("DPS Breakdown", "Mortal Strike");
        public static readonly DisplayValue Slam = new DisplayValue("DPS Breakdown", "Slam");
        public static readonly DisplayValue Rend = new DisplayValue("DPS Breakdown", "Rend");
        public static readonly DisplayValue SuddenDeath = new DisplayValue("DPS Breakdown", "Sudden Death");
        public static readonly DisplayValue Overpower = new DisplayValue("DPS Breakdown", "Overpower");
        public static readonly DisplayValue Bladestorm = new DisplayValue("DPS Breakdown", "Bladestorm");
        public static readonly DisplayValue SwordSpec = new DisplayValue("DPS Breakdown", "Sword Spec");
        //General Dps Moves
        public static readonly DisplayValue HeroicStrike = new DisplayValue("DPS Breakdown", "Heroic Strike");
        public static readonly DisplayValue DeepWounds = new DisplayValue("DPS Breakdown", "Deep Wounds");
        public static readonly DisplayValue WhiteDPS = new DisplayValue("DPS Breakdown", "White DPS");
        public static readonly DisplayValue TotalDPS = new DisplayValue("DPS Breakdown", "Total DPS");
      

        public static readonly DisplayValue FreeRage = new DisplayValue("Rage Details", "Free Rage");
        public static readonly DisplayValue WhiteRage = new DisplayValue("Rage Details", "White DPS Rage");
        public static readonly DisplayValue OtherRage = new DisplayValue("Rage Details", "Other Rage");

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