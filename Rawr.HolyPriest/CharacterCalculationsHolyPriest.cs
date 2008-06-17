using System;
using System.Collections.Generic;

namespace Rawr.HolyPriest
{

    public class CharacterCalculationsHolyPriest : CharacterCalculationsBase
    {
        private Stats basicStats;
        private TalentTree talents;

        public float SpiritRegen { get; set; }
        public float RegenInFSR { get; set; }
        public float RegenOutFSR { get; set; }
        
        public TalentTree Talents
        {
            get { return talents; }
            set { talents = value; }
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

        private float[] subPoints = new float[] { 0f, 0f };
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

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", Math.Floor(BasicStats.Spirit).ToString("0"));
            dictValues.Add("Healing", Math.Floor(BasicStats.Healing).ToString("0"));
            dictValues.Add("Mp5", Math.Floor(BasicStats.Mp5).ToString("0"));
            dictValues.Add("Regen InFSR", RegenInFSR.ToString("0"));
            dictValues.Add("Regen OutFSR", RegenOutFSR.ToString("0"));
            
            dictValues.Add("Spell Crit", string.Format("{0}%*{1} Spell Crit rating\n",
                BasicStats.SpellCrit, BasicStats.SpellCritRating.ToString()));
            
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Spell Haste rating\n", 
                Math.Round(BasicStats.SpellHasteRating / 15.7, 2), BasicStats.SpellHasteRating.ToString()));

            dictValues.Add("Renew", new Renew(BasicStats, talents).ToString());
            dictValues.Add("Flash Heal", new FlashHeal(BasicStats, talents).ToString());
            dictValues.Add("Greater Heal", new GreaterHeal(BasicStats, talents).ToString());
            dictValues.Add("Heal", new Heal(BasicStats, talents).ToString());
            dictValues.Add("PoH", new PrayerOfHealing(BasicStats, talents).ToString());
            dictValues.Add("Binding Heal", new BindingHeal(BasicStats, talents).ToString());
            dictValues.Add("Prayer of Mending", new PrayerOfMending(BasicStats, talents).ToString());
            dictValues.Add("Power Word Shield", new PowerWordShield(BasicStats, talents).ToString());

            if (talents.GetTalent("Circle of Healing").PointsInvested > 0)
                dictValues.Add("CoH", new CircleOfHealing(BasicStats, talents).ToString());
            else
                dictValues.Add("CoH", "- *No required talents");

            if (talents.GetTalent("Holy Nova").PointsInvested > 0)
                dictValues.Add("Holy Nova", new HolyNova(BasicStats, talents).ToString());
            else
                dictValues.Add("Holy Nova", "- *No required talents");

            if (talents.GetTalent("Lightwell").PointsInvested > 0)
                dictValues.Add("Lightwell", new Lightwell(BasicStats, talents).ToString());
            else
                dictValues.Add("Lightwell", "- *No required talents");
            
            return dictValues;
        }
    }
}
