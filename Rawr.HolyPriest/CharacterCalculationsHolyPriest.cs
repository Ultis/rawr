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
            dictValues.Add("Resilience", string.Format("{0}*-{1}% Damage from DoT and Mana Drains\n\r-{1}% Chance to be crit\r\n-{2}% Damage from Crits.", BasicStats.Resilience.ToString(), character.StatConversion.GetResilienceFromRating(BasicStats.Resilience).ToString("0.00"), (character.StatConversion.GetResilienceFromRating(BasicStats.Resilience) * 2f).ToString("0.00")));
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", Math.Floor(BasicStats.Spirit).ToString("0"));
            dictValues.Add("Spell Power", string.Format("{0}*{1} from Inner Fire",
                Math.Floor(BasicStats.SpellPower).ToString("0"),
                CalculationsHolyPriest.GetInnerFireSpellPowerBonus(character)));
            //dictValues.Add("Healing", Math.Floor(BasicStats.SpellPower * 1.88f).ToString("0"));
            dictValues.Add("In FSR MP5", string.Format("{0}*{1} from MP5\r\n{2} from Meditation\r\n{3} Outside FSR\r\n{4} OFSR w/MP5",
                (BasicStats.Mp5 + RegenInFSR).ToString("0"),
                BasicStats.Mp5.ToString("0"),
                RegenInFSR.ToString("0"),
                RegenOutFSR.ToString("0"),
                (BasicStats.Mp5 + RegenOutFSR).ToString("0")));
            dictValues.Add("Spell Crit", string.Format("{0}%*{1}% from Intellect\r\n{2}% from {5} Crit rating\r\n{3}% Class Base\r\n{4}% from Buffs",
                (BasicStats.SpellCrit * 100f).ToString("0.00"), character.StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect).ToString("0.00"), character.StatConversion.GetSpellCritFromRating(BasicStats.CritRating).ToString("0.00"), 1.24f, (BasicStats.SpellCrit * 100f - 1.24f - character.StatConversion.GetSpellCritFromRating(BasicStats.CritRating) - character.StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect)).ToString("0.00"), BasicStats.CritRating));
            dictValues.Add("Healing Crit", string.Format("{0}%*{1} ({1}%) points in Holy Specialization\r\n{2} ({3}%) points in Renewed Hope",
                ((BasicStats.SpellCrit * 100f) + character.PriestTalents.HolySpecialization * 1f + character.PriestTalents.RenewedHope * 2f).ToString("0.00"),
                character.PriestTalents.HolySpecialization, character.PriestTalents.RenewedHope, character.PriestTalents.RenewedHope * 2));
            dictValues.Add("Spell Haste", string.Format("{0}%*{1}% from {2} Haste rating\r\n{3}% ({3}) points in Enlightenment\r\n{4}% from Buffs\r\n{5}s Global Cooldown", 
                (BasicStats.SpellHaste * 100f).ToString("0.00"), character.StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating).ToString("0.00"), BasicStats.HasteRating.ToString(), character.PriestTalents.Enlightenment, (BasicStats.SpellHaste * 100f - character.StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating) - character.PriestTalents.Enlightenment).ToString("0.00"), Math.Max(1.0f, 1.5f / (1 + BasicStats.SpellHaste)).ToString("0.00")));

            Solver solver = new Solver(BasicStats, character, CalculationsHolyPriest.GetRaceStats(character).Mana);
            solver.Calculate(this);

            dictValues.Add("Role", string.Format("{0}*{1}", solver.Role, solver.ActionList));
            dictValues.Add("Burst", string.Format("{0}", HPSBurstPoints.ToString("0")));
            dictValues.Add("Sustained", string.Format("{0}", HPSSustainPoints.ToString("0")));

            dictValues.Add("Renew", new Renew(BasicStats, character).ToString());
            dictValues.Add("Flash Heal", new FlashHeal(BasicStats, character).ToString());
            dictValues.Add("Greater Heal", new Heal(BasicStats, character).ToString());
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
