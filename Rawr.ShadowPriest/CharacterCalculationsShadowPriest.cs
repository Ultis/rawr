using System;
using System.Collections.Generic;

namespace Rawr.ShadowPriest
{

    public class CharacterCalculationsShadowPriest : CharacterCalculationsBase
    {
        private Stats basicStats;
        private TalentTree talents;
        public CalculationOptionsShadowPriest CalculationOptions { get; set; }

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

            dictValues.Add("Spell Crit", string.Format("{0}%*{1} Spell Crit rating ({3}%)\r\n{2} ({4}%) points in Shadow Power",
                BasicStats.SpellCrit + talents.GetTalent("Shadow Power").PointsInvested * 3, 
                BasicStats.SpellCritRating.ToString(),
                talents.GetTalent("Shadow Power").PointsInvested, BasicStats.SpellCrit, talents.GetTalent("Shadow Power").PointsInvested * 3));

            int i = (int)Math.Round(CalculationsShadowPriest.GetSpellHitCap(talents) - BasicStats.SpellHitRating);
            dictValues.Add("Spell Hit", string.Format("{0}*{1}",
                BasicStats.SpellHitRating,
                (i > 0)? i + " requires to reach hit cap": i == 0? "Exactly hit cap": (-i) + " over hit cap"));
            
            
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Spell Haste rating\n", 
                Math.Round(BasicStats.SpellHasteRating / 15.7, 2), BasicStats.SpellHasteRating.ToString()));
            dictValues.Add("Global Cooldown", Spell.GetGlobalCooldown(BasicStats).ToString("0.00"));

            dictValues.Add("Shadow Word Pain", new ShadowWordPain(BasicStats, talents).ToString());
            dictValues.Add("Shadow Word Death", new ShadowWordDeath(BasicStats, talents).ToString());
            dictValues.Add("Mind Blast", new MindBlast(BasicStats, talents).ToString());
            dictValues.Add("Power Word Shield", new PowerWordShield(BasicStats, talents).ToString());

            if (talents.GetTalent("Vampiric Embrace").PointsInvested > 0)
                dictValues.Add("Vampiric Embrace", new VampiricEmbrace(BasicStats, talents).ToString());
            else
                dictValues.Add("Vampiric Embrace", "- *No required talents");

            if (talents.GetTalent("Vampiric Touch").PointsInvested > 0)
                dictValues.Add("Vampiric Touch", new VampiricTouch(BasicStats, talents).ToString());
            else
                dictValues.Add("Vampiric Touch", "- *No required talents");

            if (talents.GetTalent("Mind Flay").PointsInvested > 0)
                dictValues.Add("Mind Flay", new MindFlay(BasicStats, talents).ToString());
            else
                dictValues.Add("Mind Flay", "- *No required talents");

            Solver solver = new Solver(BasicStats, talents, CalculationOptions);
            solver.Calculate();

            dictValues.Add("Damage done", solver.OverallDamage.ToString());
            dictValues.Add("Dps", solver.OverallDps.ToString());

            return dictValues;
        }
    }
}
