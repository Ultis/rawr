using System;
using System.Collections.Generic;

namespace Rawr.HolyPriest
{

    public class CharacterCalculationsHolyPriest : CharacterCalculationsBase
    {
        private Stats basicStats;
        private Character character;

        public float SpiritRegen { get; set; }
        public float RegenInFSR { get; set; }
        public float RegenOutFSR { get; set; }
        public Character.CharacterRace Race { get; set; }

        public Character Character
        {
            get { return character; }
            set { character = value; }
        }
      
        public Stats BasicStats
        {
            get { return basicStats; }
            set { basicStats = value; }
        }

        private float overallPoints;
        public override float OverallPoints
        {
            get { return overallPoints; }
            set { overallPoints = value; }
        }

        private float[] subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }

        public float HealPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float RegenPoints
        {
            get { return subPoints[1]; }
            set { subPoints[1] = value; }
        }

        public float HastePoints
        {
            get { return subPoints[2]; }
            set { subPoints[2] = value; }
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", Math.Floor(BasicStats.Spirit).ToString("0"));
            dictValues.Add("Healing", Math.Floor(BasicStats.SpellPower * 1.88f).ToString("0"));
            dictValues.Add("Mp5", Math.Floor(BasicStats.Mp5).ToString("0"));
            dictValues.Add("Regen InFSR", RegenInFSR.ToString("0"));
            dictValues.Add("Regen OutFSR", RegenOutFSR.ToString("0"));

            dictValues.Add("Holy Spell Crit", string.Format("{0}%*{1} Spell Crit rating\n{2} ({2}%) points in Holy Specialization",
                BasicStats.SpellCrit, BasicStats.CritRating.ToString(), character.PriestTalents.HolySpecialization));
            
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Spell Haste rating\n", 
                Math.Round(BasicStats.SpellHasteRating / 15.7, 2), BasicStats.SpellHasteRating.ToString()));
            dictValues.Add("Global Cooldown", Spell.GetGlobalCooldown(BasicStats).ToString("0.00"));

            dictValues.Add("Renew", new Renew(BasicStats, character).ToString());
            dictValues.Add("Flash Heal", new FlashHeal(BasicStats, character).ToString());
            dictValues.Add("Greater Heal", new GreaterHeal(BasicStats, character).ToString());
            dictValues.Add("Heal", new Heal(BasicStats, character).ToString());
            dictValues.Add("PoH", new PrayerOfHealing(BasicStats, character).ToString());
            dictValues.Add("Binding Heal", new BindingHeal(BasicStats, character).ToString());
            dictValues.Add("Prayer of Mending", new PrayerOfMending(BasicStats, character).ToString());
            dictValues.Add("Power Word Shield", new PowerWordShield(BasicStats, character).ToString());
            dictValues.Add("Holy Nova", new HolyNova(BasicStats, character).ToString());

            if (character.PriestTalents.CircleOfHealing > 0)
                dictValues.Add("CoH", new CircleOfHealing(BasicStats, character).ToString());
            else
                dictValues.Add("CoH", "- *No required talents");


            if (character.PriestTalents.Lightwell > 0)
                dictValues.Add("Lightwell", new Lightwell(BasicStats, character).ToString());
            else
                dictValues.Add("Lightwell", "- *No required talents");
            
            if(Race == Character.CharacterRace.Draenei)
                dictValues.Add("Gift of the Naaru", new GiftOfTheNaaru(BasicStats, character).ToString());
            else
                dictValues.Add("Gift of the Naaru", "-");

            return dictValues;
        }
    }
}
