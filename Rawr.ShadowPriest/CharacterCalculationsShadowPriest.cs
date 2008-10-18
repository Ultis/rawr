using System;
using System.Collections.Generic;

namespace Rawr.ShadowPriest
{

    public class CharacterCalculationsShadowPriest : CharacterCalculationsBase
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

        private float[] subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }

        public float DpsPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float SurvivalPoints
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
            dictValues.Add("Shadow Damage", String.Format("{0}*Shadow: {1}\r\nSpell: {2}", 
                Math.Floor(BasicStats.SpellDamageRating + BasicStats.SpellShadowDamageRating), 
                Math.Floor(BasicStats.SpellShadowDamageRating), 
                Math.Floor(BasicStats.SpellDamageRating)));
            dictValues.Add("Regen", String.Format("{0}*InFSR: {0}\r\nOutFSR: {1}" , RegenInFSR.ToString("0"), RegenOutFSR.ToString("0")));

            dictValues.Add("Spell Crit", string.Format("{0}%*{1} Spell Crit rating\r\n{2}% on Mind Blast, Mind Flay and Mind Sear.\r\n{3}% on Smite, Holy Fire and Penance.",
                (BasicStats.SpellCrit * 100f).ToString("0.00"),
                BasicStats.SpellCritRating.ToString("0"),
                (BasicStats.SpellCrit * 100f + character.PriestTalents.MindMelt * 2f).ToString("0.00"),
                (BasicStats.SpellCrit * 100f + character.PriestTalents.HolySpecialization * 1f).ToString("0.00")));

            int i = (int)Math.Round(CalculationsShadowPriest.GetSpellHitCap(character) - BasicStats.SpellHitRating);
            dictValues.Add("Spell Hit", string.Format("{0}*{1}",
                BasicStats.SpellHitRating,
                (i > 0)? i + " requires to reach hit cap": i == 0? "Exactly hit cap": (-i) + " over hit cap"));
            
            
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Spell Haste rating\n", 
                Math.Round(BasicStats.SpellHasteRating / 15.7, 2), BasicStats.SpellHasteRating.ToString()));
            dictValues.Add("Global Cooldown", Math.Max(1.0f, 1.5f / (1 + BasicStats.SpellHaste)).ToString("0.00"));

            dictValues.Add("Shadow Word Pain", new ShadowWordPain(BasicStats, character).ToString());
            dictValues.Add("Shadow Word Death", new ShadowWordDeath(BasicStats, character).ToString());
            dictValues.Add("Mind Blast", new MindBlast(BasicStats, character).ToString());
            dictValues.Add("Power Word Shield", new PowerWordShield(BasicStats, character).ToString());

            if (character.PriestTalents.VampiricEmbrace > 0)
                dictValues.Add("Vampiric Embrace", new VampiricEmbrace(BasicStats, character).ToString());
            else
                dictValues.Add("Vampiric Embrace", "- *No required talents");

            if (character.PriestTalents.VampiricTouch > 0)
                dictValues.Add("Vampiric Touch", new VampiricTouch(BasicStats, character).ToString());
            else
                dictValues.Add("Vampiric Touch", "- *No required talents");

            if (character.PriestTalents.MindFlay > 0)
                dictValues.Add("Mind Flay", new MindFlay(BasicStats, character).ToString());
            else
                dictValues.Add("Mind Flay", "- *No required talents");

//            Solver solver = new Solver(BasicStats, talents, CalculationOptions);
//            solver.Calculate();

//            dictValues.Add("Damage done", solver.OverallDamage.ToString());
//            dictValues.Add("Dps", solver.OverallDps.ToString());

            return dictValues;
        }
    }
}
