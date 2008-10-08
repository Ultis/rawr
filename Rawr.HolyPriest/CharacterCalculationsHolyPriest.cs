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

        public float HPSBurstPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float HPSSustainPoints
        {
            get { return subPoints[1]; }
            set { subPoints[1] = value; }
        }

        public float SurvivabilityPoints
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
            dictValues.Add("Spell Power", Math.Floor(BasicStats.SpellPower).ToString("0"));
            //dictValues.Add("Healing", Math.Floor(BasicStats.SpellPower * 1.88f).ToString("0"));
            dictValues.Add("Mp5", Math.Floor(BasicStats.Mp5).ToString("0"));
            dictValues.Add("Regen InFSR", Math.Floor(BasicStats.Mp5 + RegenInFSR).ToString("0"));
            dictValues.Add("Regen OutFSR", Math.Floor(BasicStats.Mp5 + RegenOutFSR).ToString("0"));
            dictValues.Add("Spell Crit", string.Format("{0}%*{1}% from Intellect\r\n{2}% from Rating\r\n{3}% Class Base",
                BasicStats.SpellCrit.ToString("0.00"), (BasicStats.Intellect / 80f).ToString("0.00"), (BasicStats.CritRating / 22.08f).ToString("0.00"), 1.24f));
            dictValues.Add("Healing Crit", string.Format("{0}%*{1} Spell Crit rating\r\n{2} ({2}%) points in Holy Specialization\r\n{3} ({4}%) points in Renewed Hope",
                (BasicStats.SpellCrit + character.PriestTalents.HolySpecialization * 1f + character.PriestTalents.RenewedHope * 2f).ToString("0.00"),
                BasicStats.CritRating.ToString("0"), character.PriestTalents.HolySpecialization, character.PriestTalents.RenewedHope, character.PriestTalents.RenewedHope * 2));          
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Spell Haste rating\r\n{2}% ({2}) points in Enlightenment.", 
                (BasicStats.HasteRating / 15.7f + BasicStats.SpellHaste).ToString("0.00"), BasicStats.HasteRating.ToString(), character.PriestTalents.Enlightenment));
            dictValues.Add("Global Cooldown", Math.Max(1.0f, 1.5f / (1 + BasicStats.HasteRating / 15.7f / 100f + BasicStats.SpellHaste / 100f)).ToString("0.00"));

            dictValues.Add("Renew", new Renew(BasicStats, character).ToString());
            dictValues.Add("Flash Heal", new FlashHeal(BasicStats, character).ToString());
            dictValues.Add("Greater Heal", new GreaterHeal(BasicStats, character).ToString());
            //dictValues.Add("Heal", new Heal(BasicStats, character).ToString());
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

            if (character.PriestTalents.Penance > 0)
                dictValues.Add("Penance", new Penance(BasicStats, character).ToString());
            else
                dictValues.Add("Penance", "- *No required talents");
            
            if(Race == Character.CharacterRace.Draenei)
                dictValues.Add("Gift of the Naaru", new GiftOfTheNaaru(BasicStats, character).ToString());
            else
                dictValues.Add("Gift of the Naaru", "-");

            return dictValues;
        }
    }
}
