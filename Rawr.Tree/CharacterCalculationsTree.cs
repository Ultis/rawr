using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tree
{
    public class CharacterCalculationsTree : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }

        private float[] subPoints = new float[] { 0f, 0f, 0f};
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }

        public float HpSPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float Mp5Points
        {
            get { return subPoints[1]; }
        }

        public float SurvivalPoints
        {
            get { return subPoints[2]; }
            set { subPoints[2] = value; }
        }

        public override float OverallPoints { get; set; }

        private string mp5PointsBreakdown = "Breakdown:";
        public string Mp5PointsBreakdown
        {
            get { return mp5PointsBreakdown; }
            set { mp5PointsBreakdown = value; }
        }

        public void AddMp5Points(float value, string source)
        {
            if (value == 0)
                return;

            Mp5PointsBreakdown += String.Format("\n{0:0.0} mp5 ({1})", value, source);
            subPoints[1] += value;
        }

        public float ManaRegInFSR { get; set; }
        public float ManaRegOutFSR { get; set; }

        public Character LocalCharacter { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Spirit", BasicStats.Spirit.ToString());
            dictValues.Add("Healing", (BasicStats.SpellPower + BasicStats.TreeOfLifeAura).ToString() + "*" + BasicStats.Spirit * LocalCharacter.DruidTalents.ImprovedTreeOfLife * 0.05f  + " ToL Bonus\n" + BasicStats.AverageHeal + " average spell power");

            dictValues.Add("MP5", ManaRegInFSR.ToString() + "*" + ManaRegOutFSR.ToString() + " Out of FSR\n" + BasicStats.Mana * 0.0025f * 5 * (((CalculationOptionsTree)LocalCharacter.CalculationOptions).haveReplenishSupport ? 1 : 0) + " Replenish");
            dictValues.Add("Spell Crit", BasicStats.SpellCrit.ToString());
            dictValues.Add("Spell Haste", Math.Round(BasicStats.SpellHaste / 15.7f, 2) + "%");
            dictValues.Add("Global CD", Math.Round((1.5f * 1570f) / (1570f + BasicStats.HasteRating), 2) + "sec");

            dictValues.Add("HpS", HpSPoints.ToString());
            dictValues.Add("Mp5", Mp5Points.ToString());
            dictValues.Add("Survival", SurvivalPoints.ToString());
            dictValues.Add("Overall", OverallPoints.ToString());

            Spell spell = new Regrowth(this, true);
            dictValues.Add("RG Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + Math.Round(spell.CritPercent, 2) + "% Crit" + (((CalculationOptionsTree)LocalCharacter.CalculationOptions).glyphOfRegrowth ? "\nGlyphed" : ""));
            dictValues.Add("RG Tick", Math.Round(spell.PeriodicTick, 2) + "*" + spell.Duration + "sec Duration\n" + Math.Round(spell.PeriodicTick * 6f, 2) + " - " + Math.Round(spell.PeriodicTick * 9f, 2) + " Swiftmend");
            dictValues.Add("RG HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + "sec Casttime");
            dictValues.Add("RG HPS (HoT)", Math.Round(spell.HPSHoT, 2).ToString());
            dictValues.Add("RG HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.AverageHealing + (spell.PeriodicTick * spell.PeriodicTicks), 2) + " Health\n" + Math.Round(spell.manaCost, 2) + " Manacost");
            dictValues.Add("RG HPM (spam)", Math.Round(spell.AverageHealing / spell.manaCost, 2).ToString());

            spell = new Lifebloom(this);
            dictValues.Add("LB Tick", Math.Round(spell.PeriodicTick, 2) + "*" + spell.Duration + "sec Duration");
            dictValues.Add("LB Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.AverageHealing * 1.5f, 2) + " Crit\n" + spell.CritPercent + "%");
            dictValues.Add("LB HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.AverageHealing + (spell.PeriodicTick * spell.PeriodicTicks), 2) + " Health\n" + Math.Round(spell.manaCost, 2) + " Manacost");

            spell = new LifebloomStack(this);
            dictValues.Add("LBS Tick", Math.Round(spell.PeriodicTick, 2).ToString());
            dictValues.Add("LBS HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.AverageHealing + (spell.PeriodicTick * spell.PeriodicTicks), 2) + " Health over " + spell.PeriodicTicks + "sec");

            spell = new Rejuvenation(this);
            dictValues.Add("RJ Tick", Math.Round(spell.PeriodicTick, 2) + "*" + spell.Duration + "sec Duration\n" + Math.Round(spell.PeriodicTick * 4f, 2) + " - " + Math.Round(spell.PeriodicTick * 6f, 2) + " Swiftmend");
            dictValues.Add("RJ HPS", Math.Round(spell.HPSHoT, 2).ToString());
            dictValues.Add("RJ HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.AverageHealing + (spell.PeriodicTick * spell.PeriodicTicks), 2) + " Health\n" + Math.Round(spell.manaCost, 2) + " Manacost");

            spell = new HealingTouch(this);
            dictValues.Add("HT Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + spell.CritPercent + "% Crit");
            dictValues.Add("HT HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + "sec Casttime" + (spell.castTime < spell.CastTime ? ", GCD capped" : ""));
            dictValues.Add("HT HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.AverageHealing + (spell.PeriodicTick * spell.PeriodicTicks), 2) + " Health\n" + Math.Round(spell.manaCost, 2) + " Manacost");

            return dictValues;
        }
    }
}
