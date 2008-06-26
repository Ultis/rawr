using System;
using System.Collections.Generic;

namespace Rawr.Tree
{

    public class CharacterCalculationsTree : CharacterCalculationsBase
    {
        private Stats basicStats;
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

        private float[] subPoints = new float[] { 0f };
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

        public float OS5SRRegen
        {
            get;
            set;
        }

        public float IS5SRRegen
        {
            get;
            set;
        }

        public List<Spell> Spells
        {
            get;
            set;
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", BasicStats.Spirit.ToString());
            dictValues.Add("Healing", BasicStats.Healing.ToString());
            dictValues.Add("Mp5", string.Format("{0}*{1} mp5 outside the 5-second rule",
                (int) (5*IS5SRRegen),
                (int) (5*OS5SRRegen)));

            dictValues.Add("Spell Crit", string.Format("{0}%*{1} Spell Crit rating",
                BasicStats.SpellCrit, BasicStats.SpellCritRating.ToString()));
            
            dictValues.Add("Spell Haste", string.Format("{0}%*{1} Spell Haste rating\nGlobal cooldown is {2} seconds", 
                Math.Round(BasicStats.SpellHasteRating / 15.7, 2),
                BasicStats.SpellHasteRating.ToString(),
                Math.Round((1.5f * 1570f) / (1570f + BasicStats.SpellHasteRating), 2)));

            addSpellValues(dictValues);

            return dictValues;
        }

        private void addSpellValues(Dictionary<string, string> dictValues) {
            Spell spell;
            if ((spell = Spells.Find(delegate(Spell s) { return s is Lifebloom && !(s is LifebloomStack); })) != null)
			{
				dictValues.Add("LB Tick", Math.Round(spell.PeriodicTick, 2).ToString());
                dictValues.Add("LB Heal", string.Format("{0}*{1}", spell.AverageTotalHeal.ToString(), spell.HealInterval));
                dictValues.Add("LB HPS", string.Format("{0}*HPS is the average amount healed divided by the time to cast the spell", spell.HPS));
                dictValues.Add("LB HPM", string.Format("{0}*{1} mana", spell.HPM, spell.Cost));
			}
			else
			{
				dictValues.Add("LB Tick", "--");
				dictValues.Add("LB Heal", "--");
				dictValues.Add("LB HPS", "--");
				dictValues.Add("LB HPM", "--");
			}

            if ((spell = Spells.Find(delegate(Spell s) { return s is LifebloomStack; })) != null)
            {
                dictValues.Add("LBS Tick", Math.Round(spell.PeriodicTick, 2).ToString());
				dictValues.Add("LBS HPS", string.Format("{0}*Assumes on average {1} ticks per renewed Lifebloom",
                    spell.HPS, spell.PeriodicTicks));
				dictValues.Add("LBS HPM", string.Format("{0}*Assumes on average {1} ticks per renewed Lifebloom",
                    spell.HPM, spell.PeriodicTicks));
			}
			else
			{
				dictValues.Add("LBS Tick", "--");
				dictValues.Add("LBS HPS", "--");
				dictValues.Add("LBS HPM", "--");
			}

            if ((spell = Spells.Find(delegate(Spell s) { return s is Rejuvenation; })) != null)
			{
                dictValues.Add("RJ Tick", Math.Round(spell.PeriodicTick, 2).ToString());
                dictValues.Add("RJ HPS", string.Format("{0}*HPS is the average amount healed divided by the time to cast the spell", spell.HPS));
                dictValues.Add("RJ HPM", string.Format("{0}*{1} mana", spell.HPM, spell.Cost));
			}
			else
			{
				dictValues.Add("RJ Tick", "--");
				dictValues.Add("RJ HPS", "--");
				dictValues.Add("RJ HPM", "--");
			}

            if ((spell = Spells.Find(delegate(Spell s) { return s is Regrowth; })) != null)
            {
                dictValues.Add("RG Tick", Math.Round(spell.PeriodicTick, 2).ToString());
                dictValues.Add("RG Heal", String.Format("{0}*{1}", spell.AverageTotalHeal, spell.HealInterval));
                dictValues.Add("RG HPS", string.Format("{0}*HPS is the average amount healed divided by the time to cast the spell", spell.HPS));
                dictValues.Add("RG HPM", string.Format("{0}*{1} mana", spell.HPM, spell.Cost));
			}
			else
			{
				dictValues.Add("Regrowth Tick", "--");
				dictValues.Add("Regrowth Heal", "--");
				dictValues.Add("Regrowth HPS", "--");
				dictValues.Add("Regrowth HPM", "--");
			}

            if ((spell = Spells.Find(delegate(Spell s) { return s is HealingTouch; })) != null)
            {
                dictValues.Add("HT Heal", String.Format("{0}*{1}", spell.AverageTotalHeal, spell.HealInterval));
                dictValues.Add("HT HPS", string.Format("{0}*HPS is the average amount healed divided by the time to cast the spell\nCasttime: {1}", spell.HPS, Math.Round(spell.CastTime,2)));
                dictValues.Add("HT HPM", string.Format("{0}*{1} mana", spell.HPM, spell.Cost));
			}
			else
			{
				dictValues.Add("HT Heal", "--");
				dictValues.Add("HT HPS", "--");
				dictValues.Add("HT HPM", "--");
			}
        }
    }
}
