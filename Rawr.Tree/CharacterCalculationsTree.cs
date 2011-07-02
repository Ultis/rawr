using System;
using System.Collections.Generic;
using System.Text;
using Rawr.ModelFramework;

namespace Rawr.Tree {
    enum TreeAction
    {
        // must be in the same order of TreeSpells
        RaidNourish,
        RaidHealingTouch,
        RaidRegrowth,
        RaidTolLb,
        RaidRejuvenation,
        RaidTranquility,
        RaidSwiftmend,
        RaidWildGrowth,

        TankNourish,
        TankHealingTouch,
        TankRegrowth,
        TankTolLb,
        TankRejuvenation,
        TankTranquility,
        TankSwiftmend,
        TankWildGrowth,

        RaidRj2NourishNB,
        RaidRj3NourishNB,
        RaidClearHT,
        RaidClearRegrowth,
        RaidSwiftHT,
        RaidTolLbCcHt,

        TankRj2NourishNB,
        TankClearHT,
        TankClearRegrowth,
        TankSwiftHT,
        TankTolLbCcHt, // the LB is on the raid, but the CCHT is on the tank

        ReLifebloom,

        Count
    };

    enum TreePassive
    {
        RollingLifebloom,
        Perserverance,
        NaturesWard,
        HealingTrinkets,

        Count
    };

    sealed internal class TreeStats
    {
        public HasteStats Haste;

        public double SpellPower;
        public double SpellCrit;

        public double TreeOfLifeUptime;
        public double Harmony;

        public double SpellsManaCostReduction;
        public double BonusCritHealMultiplier;

        public double PassiveDirectHealBonus;
        public double PassivePeriodicHealBonus;
        public double DirectHealMultiplier;
        public double PeriodicHealMultiplier;
        public double SpellsManaCostMultiplier;

        public double Healed;

        public TreeStats(Character character, Stats stats, KeyValuePair<double, SpecialEffect>[] hasteProcs, double treeOfLifeUptime)
        {
            CalculationOptionsTree opts = character.CalculationOptions as CalculationOptionsTree;

            bool Restoration = (opts != null) ? opts.Restoration : true;

            Haste = new HasteStats(stats, hasteProcs);

            SpellCrit = StatConversion.GetSpellCritFromIntellect(stats.Intellect) + StatConversion.GetSpellCritFromRating(stats.CritRating) + stats.SpellCrit;

            SpellPower = (float)(stats.SpellPower + Math.Max(0f, stats.Intellect - 10));
            // TODO: does nurturing instinct actually work like this?
            SpellPower += character.DruidTalents.NurturingInstinct * 0.5 * stats.Agility;
            SpellPower *= (1 + stats.BonusSpellPowerMultiplier);

            TreeOfLifeUptime = treeOfLifeUptime;
            double mastery = 8.0f + StatConversion.GetMasteryFromRating(stats.MasteryRating);
            if(Restoration)
                Harmony = mastery * 0.0125f;

            SpellsManaCostReduction = stats.SpellsManaCostReduction + stats.NatureSpellsManaCostReduction;
            BonusCritHealMultiplier = stats.BonusCritHealMultiplier;

            // according to Paragon's Anaram posting on ElitistJerks, Harmony is additive
            PassiveDirectHealBonus = (Restoration ? 1.25f : 1.0f) + Harmony;
            PassivePeriodicHealBonus = PassiveDirectHealBonus + 0.02f * character.DruidTalents.Genesis;
            DirectHealMultiplier = (1 + stats.BonusHealingDoneMultiplier) * (1.0f + character.DruidTalents.MasterShapeshifter * 0.04f) * (1 + TreeOfLifeUptime * 0.15f);
            PeriodicHealMultiplier = DirectHealMultiplier * (1 + stats.BonusPeriodicHealingMultiplier);
            SpellsManaCostMultiplier = 1.0f - character.DruidTalents.Moonglow * 0.03f;

            Healed = stats.Healed;
        }
    }

    sealed class ComputedSpell
    {
        public SpellData Data;
        public TreeStats Stats;

        public int TimeReductionMS;

        public double DirectMultiplier;
        public double TickMultiplier;

        public double ExtraDirectBonus;
        public double ExtraTickBonus;
        public double ExtraDurationMS;

        public double Tick;
        public double Ticks;
        public double Duration;
        public double TPS;

        public DiscreteAction Action;

        public DiscreteAction RaidAction;
        public DiscreteAction TankAction;

        public void Multiply(double v)
        {
            RaidAction.Direct *= v;
            RaidAction.Periodic *= v;
            TankAction.Direct *= v;
            TankAction.Periodic *= v;
        }

        public void MultiplyTank(double v)
        {
            TankAction.Direct *= v;
            TankAction.Periodic *= v;
        }

        public void MultiplyRaid(double v)
        {
            RaidAction.Direct *= v;
            RaidAction.Periodic *= v;
        }

        public void MultiplyDirect(double v)
        {
            TankAction.Direct *= v;
            RaidAction.Direct *= v;
        }

        public void MultiplyPeriodic(double v)
        {
            TankAction.Periodic *= v;
            RaidAction.Periodic *= v;
        }

        public ComputedSpell(SpellData data, TreeStats stats)
        {
            this.Data = data;
            this.Stats = stats;
            this.DirectMultiplier = stats.DirectHealMultiplier;
            this.TickMultiplier = stats.PeriodicHealMultiplier;
        }

        public void ComputeTiming()
        {
            Action.Time = Stats.Haste.ComputeHastedCastTime(Data.BaseTimeMS - TimeReductionMS);

            if (Data.BaseDurationMS > 0)
                Ticks = Stats.Haste.ComputeTicks(Data.BaseTickRateMS, Data.BaseDurationMS + ExtraDurationMS, out Duration, out TPS);
        }

        public void ComputeRest()
        {
            Action.Direct = 0;
            Action.Casts = 0;
            if (Data.AvgHeal > 0 || Data.Coeff > 0)
            {
                double direct = Data.AvgHeal + Stats.SpellPower * Data.Coeff;
                direct *= (Stats.PassiveDirectHealBonus + ExtraDirectBonus) * DirectMultiplier;
                Action.Direct = direct;
                Action.Casts = 1;
            }

            Tick = 0;
            if (Data.TickHeal > 0 || Data.TickCoeff > 0)
            {
                double tick = Data.TickHeal + Stats.SpellPower * Data.TickCoeff;
                tick *= (Stats.PassivePeriodicHealBonus + ExtraTickBonus) * TickMultiplier;

                Tick = tick;
            }

            double mana = Data.Mana;
            mana -= Stats.SpellsManaCostReduction;
            mana *= Stats.SpellsManaCostMultiplier;
            Action.Mana = (int)mana;

            Action.Periodic = Tick * Ticks;

            Action.Ticks = Ticks;
        }
    }

    public class CharacterCalculationsTree : CharacterCalculationsBase
    {
        private float[] subPoints = new float[(int)PointsTree.Count];
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }
        public override float OverallPoints { get; set; }

        public double ManaRegen { get; set; }
        public double BaseRegen { get; set; }
        public double ManaPoolRegen { get; set; }
        public double PotionRegen { get; set; }
        public double SpiritRegen { get; set; }
        public double InnervateRegen { get; set; }
        public double ReplenishmentRegen { get; set; }
        public double RevitalizeRegen { get; set; }

        public double FightLength { get; set; }
        public double Innervates { get; set; }
        public double InnervateEffectDelay { get; set; }
        public SpecialEffect InnervateEffect { get; set; }
        public double MeanMana { get; set; }
        public double InnervateMana { get; set; }
        public double InnervateSize { get; set; }

        public double ProcTriggerInterval { get; set; }
        public double ProcPeriodicTriggerInterval { get; set; }

        public Stats BasicStats { get; set; }
        public double BaseSpellPower { get; set; }

        public FightDivision Division { get; set; }

        private ActionDistributionsByDivision[] solutions = new ActionDistributionsByDivision[(int)PointsTree.Count];
        public ActionDistributionsByDivision[] Solutions { get { return solutions; } }

        internal TreeStats[] Stats { get; set; }
        internal ComputedSpell[][] Spells { get; set; }
        public ContinuousAction[][] Actions { get; set; }

        delegate double SpellValueDelegate(ComputedSpell spell);

        void addSpellCalculationValues(Dictionary<string, string> dict, string name, SpellValueDelegate del)
        {
            for (int i = 0; i < CalculationsTree.SpellData.Length; ++i)
            {
                double avg = 0;
                for (int div = 0; div < Spells.Length; ++div)
                    avg += Division.Fractions[div] * del(Spells[div][i]);

                dict.Add(CalculationsTree.SpellData[i].Name + " " + name, String.Format("{0:F2}", avg) + "*" + Division.GetDivisionDetailTooltip(div => String.Format("{0:F2}", del(Spells[div][i]))));
            }
        }

        delegate double SpellStatValueDelegate(TreeStats stats);

        void addSpellStatValues(Dictionary<string, string> dict, string name, string format, SpellStatValueDelegate del, string extraInfo = null)
        {
            double avg = 0;
            for (int div = 0; div < Spells.Length; ++div)
                avg += Division.Fractions[div] * del(Stats[div]);

            string tooltip = String.Format(format, avg) + "*" + Division.GetDivisionDetailTooltip(div => String.Format(format, del(Stats[div])));
            if(extraInfo != null)
                tooltip += "\n\n" + extraInfo;
            dict.Add(name, tooltip);
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();
            //
            if (BasicStats == null) BasicStats = new Stats();

            retVal.Add("Health", BasicStats.Health.ToString());
            retVal.Add("Mana", BasicStats.Mana.ToString());
            retVal.Add("Armor", BasicStats.Armor.ToString());
            retVal.Add("Agility", Math.Floor(BasicStats.Agility).ToString());
            retVal.Add("Stamina", Math.Floor(BasicStats.Stamina).ToString());
            retVal.Add("Intellect", Math.Floor(BasicStats.Intellect).ToString());
            retVal.Add("Spirit", Math.Floor(BasicStats.Spirit).ToString());

            retVal.Add("Fight Length", String.Format("{0:F0}s", FightLength));
            retVal.Add("Divisions", Division.Fractions.Length.ToString() + "*" + Division.GetDivisionDetailTooltip(i => String.Format("{0:F0}s ({1:F2}%)", Division.Fractions[i] * FightLength, 100 * Division.Fractions[i])));

            retVal.Add("Innervates", String.Format("{0:F2}", Innervates) + ((Innervates != Math.Floor(Innervates)) ? "*Fractional values denote the reduced value of innervates near the fight beginning or end" : ""));
            retVal.Add("Innervate Effect Delay", String.Format("{0:F2}", InnervateEffectDelay) + ((InnervateEffect != null) ? "*Innervate when this effect is up, and it wouldn't cap mana:\n" + InnervateEffect.ToString() : ""));
            retVal.Add("Mean Mana", String.Format("{0:F0}", MeanMana));
            retVal.Add("Innervate Mana", String.Format("{0:F0}", InnervateMana));
            retVal.Add("Innervate Size", String.Format("{0:F0}", InnervateSize));

            addSpellStatValues(retVal, "Spell Power", "{0:F0}", x => x.SpellPower, String.Format("{0:F0} Base Spell Power", Math.Floor(BaseSpellPower)));
            addSpellStatValues(retVal, "Spell Crit", "{0:F2}%", x => x.SpellCrit * 100, String.Format("{0} Crit Rating from Gear, {1:F}% Crit from Gear, {2:F}% Crit from Gear Intellect",
                BasicStats.CritRating,
                100 * StatConversion.GetSpellCritFromRating(BasicStats.CritRating),
                100 * StatConversion.GetSpellCritFromIntellect(BasicStats.Intellect)));
            addSpellStatValues(retVal, "Spell Haste", "{0:F2}%", x => 100 * (1 / x.Haste.HastedSecond - 1), String.Format("{0} Haste Rating from Gear, {1:F}% Haste from Gear, {2:F}% Haste from Gear and Buffs",
                (float)BasicStats.HasteRating,
                100 * StatConversion.GetSpellHasteFromRating((float)BasicStats.HasteRating),
                100 * ((1 + StatConversion.GetSpellHasteFromRating((float)BasicStats.HasteRating)) * (1.0f + BasicStats.SpellHaste) - 1)
                ));
            addSpellStatValues(retVal, "Spell Mana Cost Reduction", "{0:F0}", x => x.SpellsManaCostReduction);
            addSpellStatValues(retVal, "Spell Crit Extra Bonus", "{0:F0}%", x => 100 * x.BonusCritHealMultiplier);
            string masteryInfo = String.Format("{0:F0} Mastery Rating from Gear, {1:F2} Mastery from Gear",
                BasicStats.MasteryRating, 8 + StatConversion.GetMasteryFromRating(BasicStats.MasteryRating));
            addSpellStatValues(retVal, "Harmony", "{0:F}%", x => 100 * x.Harmony, masteryInfo);
            
            retVal.Add("Mana Regen", String.Format("{0:F0}", ManaRegen));
            retVal.Add("Base Mana Regen", String.Format("{0:F0}", BaseRegen));
            retVal.Add("Initial Mana Pool Regen", String.Format("{0:F0}", ManaPoolRegen));
            retVal.Add("Spirit Mana Regen", String.Format("{0:F0}", SpiritRegen));
            retVal.Add("Innervate Mana Regen", String.Format("{0:F0}", InnervateRegen));
            retVal.Add("Replenishment Mana Regen", String.Format("{0:F0}", ReplenishmentRegen));
            retVal.Add("Revitalize Mana Regen", String.Format("{0:F0}", RevitalizeRegen));
            retVal.Add("Potion Mana Regen", String.Format("{0:F0}", PotionRegen));

            retVal.Add("Total Score", String.Format("{0:F2}", OverallPoints));
            string[] longNames = { "Raid Sustained", "Raid Burst", "Tank Sustained", "Tank Burst" };
            for (int i = 0; i < longNames.Length; ++i)
            {
                retVal.Add(longNames[i] + " HPS", String.Format("{0:F2}", Solutions[i].Distribution.TotalEPS()));
                retVal.Add(longNames[i] + " Directs/s", String.Format("{0:F2}", Solutions[i].Distribution.TotalCPS()));
                retVal.Add(longNames[i] + " Ticks/s", String.Format("{0:F2}", Solutions[i].Distribution.TotalTPS()));
            }

            retVal.Add("Proc trigger interval", String.Format("{0:F2}", ProcTriggerInterval));
            retVal.Add("Proc periodic trigger interval", String.Format("{0:F2}", ProcPeriodicTriggerInterval));

            ContinuousAction[] actions = ContinuousAction.AverageActionSets(Actions, Division.Fractions);

            for (int i = 0; i < actions.Length; ++i)
            {
                retVal.Add(CalculationsTree.ActionNames[i] + " HPCT", actions[i].EPSText + "*" + Division.GetDivisionDetailTooltip(div => Actions[div][i].EPSText));
                retVal.Add(CalculationsTree.ActionNames[i] + " MPCT", actions[i].MPSText + "*" + Division.GetDivisionDetailTooltip(div => Actions[div][i].MPSText));
                retVal.Add(CalculationsTree.ActionNames[i] + " HPM", actions[i].EPMText + "*" + Division.GetDivisionDetailTooltip(div => Actions[div][i].EPMText));
            }

            string[] names = {"Raid S.", "Raid B.", "Tank S.", "Tank B."};
            for (int i = 0; i < names.Length; ++i)
                Solutions[i].GetProperties(retVal, names[i] + ' ', CalculationsTree.ActionNames, CalculationsTree.PassiveNames);

            addSpellCalculationValues(retVal, "Time", s => s.Action.Time);
            addSpellCalculationValues(retVal, "Duration", s => s.Duration);
            addSpellCalculationValues(retVal, "Mana", s => s.Action.Mana);
            addSpellCalculationValues(retVal, "Direct", s => s.Action.Direct);
            addSpellCalculationValues(retVal, "Tick", s => s.Tick);
            addSpellCalculationValues(retVal, "Ticks", s => s.Ticks);
            addSpellCalculationValues(retVal, "Periodic", s => s.Action.Periodic);
            addSpellCalculationValues(retVal, "Raid Direct", s => s.RaidAction.Direct);
            addSpellCalculationValues(retVal, "Raid Periodic", s => s.RaidAction.Periodic);
            addSpellCalculationValues(retVal, "Tank Direct", s => s.TankAction.Direct);
            addSpellCalculationValues(retVal, "Tank Periodic", s => s.TankAction.Periodic);
            return retVal;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Intellect": return BasicStats.Intellect;
                case "Spirit": return BasicStats.Spirit;
                case "Haste Rating": return BasicStats.HasteRating;
                case "Crit Rating": return BasicStats.CritRating;
                case "Mastery Rating": return BasicStats.MasteryRating;
                case "Health": return (float)BasicStats.Health;
                case "Mana": return (float)BasicStats.Mana;
                case "Mana Regen": return (float)ManaRegen;
            }
            return 0;
        }
    }
}
