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
            dictValues.Add("Spell Power", String.Format("{0}*Shadow: {1}\r\nHoly: {2}", 
                Math.Floor(BasicStats.SpellPower), 
                Math.Floor(BasicStats.SpellPower + BasicStats.SpellShadowDamageRating), 
                Math.Floor(BasicStats.SpellPower /*+ BasicStats.SpellHolyDamageRating*/)));
            dictValues.Add("Regen", String.Format("{0}*MP5: {1}\r\nOutFSR: {2}" , (RegenInFSR + BasicStats.Mp5).ToString("0"), BasicStats.Mp5.ToString(), (RegenOutFSR + BasicStats.Mp5).ToString("0")));

            dictValues.Add("Spell Crit", string.Format("{0}%*{1}% from {2} Spell Crit rating\r\n{3}% from Intellect\r\n{4}% from Base Crit\r\n{5}% from Buffs\r\n{6}% on Mind Blast, Mind Flay and Mind Sear.\r\n{7}% on Smite, Holy Fire and Penance.",
                (BasicStats.SpellCrit * 100f).ToString("0.00"),
                (BasicStats.CritRating / 22.07f).ToString("0.00"),
                BasicStats.CritRating.ToString("0"),
                (BasicStats.Intellect / 80f).ToString("0.00"),
                "1,24",
                (BasicStats.SpellCrit * 100f - BasicStats.CritRating / 22.07f - BasicStats.Intellect / 80f - 1.24f).ToString("0.00"),
                (BasicStats.SpellCrit * 100f + character.PriestTalents.MindMelt * 2f).ToString("0.00"),
                (BasicStats.SpellCrit * 100f + character.PriestTalents.HolySpecialization * 1f).ToString("0.00")));

            int i = (int)Math.Round(CalculationsShadowPriest.GetSpellHitCap(character) - BasicStats.SpellHitRating);
            dictValues.Add("Spell Hit", string.Format("{0}*{1}",
                BasicStats.HitRating,
                (i > 0)? i + " required to reach hit cap": i == 0? "Exactly hit cap": (-i) + " over hit cap"));

            dictValues.Add("Spell Haste", string.Format("{0}%*{1}% from {2} Haste rating\r\n{3}% ({3}) points in Enlightenment\r\n{4}% from Buffs\r\n{5}s Global Cooldown",
                (BasicStats.SpellHaste * 100f).ToString("0.00"), (BasicStats.HasteRating / 15.77).ToString("0.00"), BasicStats.HasteRating.ToString(), character.PriestTalents.Enlightenment, (BasicStats.SpellHaste * 100f - (BasicStats.HasteRating / 15.77f)).ToString("0.00"), Math.Max(1.0f, 1.5f / (1 + BasicStats.SpellHaste)).ToString("0.00")));

            dictValues.Add("SW Pain", new ShadowWordPain(BasicStats, character).ToString());
            dictValues.Add("Devouring Plague", new DevouringPlague(BasicStats, character).ToString());
            dictValues.Add("SW Death", new ShadowWordDeath(BasicStats, character).ToString());
            dictValues.Add("Mind Blast", new MindBlast(BasicStats, character).ToString());
            dictValues.Add("PW Shield", new PowerWordShield(BasicStats, character).ToString());

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

            Solver solver = new Solver(BasicStats, character);
            solver.Calculate();

            dictValues.Add("Damage done", solver.OverallDamage.ToString());
            dictValues.Add("Dps", solver.OverallDps.ToString());

            return dictValues;
        }
    }
}
