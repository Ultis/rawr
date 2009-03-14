using System;
using System.Collections.Generic;

namespace Rawr.Tree
{
    public class CharacterCalculationsTree : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }

        private float[] subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }

        public float BurstPoints
        {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }

        public float SustainedPoints
        {
            get { return subPoints[1]; }
            set { subPoints[1] = value; }
        }

        public override float OverallPoints { get; set; }

        public Rotation Simulation;

        public Character LocalCharacter { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("HealBurst", BurstPoints.ToString());
            dictValues.Add("HealSustained", SustainedPoints.ToString());
            dictValues.Add("Overall", OverallPoints.ToString());

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            float spi_from_trinkets = (BasicStats.SpiritFor20SecOnUse2Min / 6f);
            dictValues.Add("Spirit", BasicStats.Spirit.ToString() + (spi_from_trinkets > 0 || BasicStats.ExtraSpiritWhileCasting > 0 ? "*" + Math.Round(spi_from_trinkets, 2).ToString() + " Spirit from trinkets\n" + Math.Round(BasicStats.ExtraSpiritWhileCasting,2).ToString() + " Spirit while casting" : ""));
            dictValues.Add("Healing", (BasicStats.SpellPower + BasicStats.TreeOfLifeAura).ToString() + "*" + BasicStats.Spirit * LocalCharacter.DruidTalents.ImprovedTreeOfLife * 0.05f + " ToL Bonus\n");

            bool hasSpiWhileCasting = BasicStats.ExtraSpiritWhileCasting > 0;
            dictValues.Add("MP5", Math.Round(Simulation.ManaPer5In5SR).ToString() + "*" + Math.Round(Simulation.ManaPer5Out5SR).ToString() + " Out of FSR\n" + Math.Round(Simulation.ReplenishRegen).ToString() + " From Replenishment\n" + (hasSpiWhileCasting ? "(values include extra Spirit while casting)\n" : "\n") + Math.Round(Simulation.ManaFromInnervate).ToString() + " extra mana from each Innervate");
            dictValues.Add("Spell Crit", BasicStats.SpellCrit.ToString());
            float haste = (1 + BasicStats.HasteRating / TreeConstants.HasteRatingToHaste);
            float sp = 1 + BasicStats.SpellHaste;
            float hard = (1.5f / (1f * sp) - 1) * TreeConstants.HasteRatingToHaste;
            float soft = (1.5f / (1.3f * sp) - 1) * TreeConstants.HasteRatingToHaste;
            float haste_until_hard_cap = hard - BasicStats.HasteRating;
            float haste_until_soft_cap = soft - BasicStats.HasteRating;
            dictValues.Add("Spell Haste", Math.Round(sp, 2) + "%");
            dictValues.Add("Global CD", Math.Round(1.5f / (haste * sp), 2) + "sec*" + Math.Round(haste_until_hard_cap, 0).ToString() + " Haste Rating until hard gcd cap\n" + Math.Round(haste_until_soft_cap, 0).ToString() + " Haste Rating until soft (GotEM) gcd cap");

            dictValues.Add("Time until OOM", Simulation.TimeToOOM.ToString() + "* " + Math.Round(Simulation.UnusedMana, 0).ToString() + " mana remaining at end of fight");
            dictValues.Add("Total healing done", Simulation.TotalHealing.ToString());
            dictValues.Add("HPS for primary heal", Math.Round(Simulation.HPSFromPrimary,2).ToString());
            dictValues.Add("HPS for tank HoTs", Math.Round(Simulation.HPSFromHots,2).ToString());
            dictValues.Add("MPS for primary heal", Math.Round(Simulation.MPSFromPrimary,2).ToString());
            dictValues.Add("MPS for tank HoTs", Math.Round(Simulation.MPSFromHots,2).ToString());
            dictValues.Add("MPS for Wild Growth", Math.Round(Simulation.MPSFromWildGrowth, 2).ToString());
            dictValues.Add("Mana regen per second", Math.Round(Simulation.ManaPer5InRotation/5, 2).ToString());
            dictValues.Add("HoT refresh fraction", Math.Round(Simulation.HotsFraction, 2).ToString());
            dictValues.Add("Unused cast time fraction", Math.Round(Simulation.UnusedCastTimeFrac, 2).ToString());
            dictValues.Add("Casts per minute until OOM", Math.Round(Simulation.CastsPerMinute, 2).ToString());
            dictValues.Add("Crits per minute until OOM", Math.Round(Simulation.CritsPerMinute, 2).ToString());

            Spell spell = new Regrowth(this, BasicStats, true);
            dictValues.Add("RG Heal", Math.Round(spell.AverageHealing, 2) + "*" + 
                "Base: " + Math.Round(spell.BaseMinHeal, 2) + " - " + Math.Round(spell.BaseMaxHeal, 2) + "\n" +
                Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + 
                Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + 
                Math.Round(spell.CritPercent, 2) + "% Crit" + (((CalculationOptionsTree)LocalCharacter.CalculationOptions).glyphOfRegrowth ? "\nGlyphed" : ""));
            dictValues.Add("RG Tick", Math.Round(spell.PeriodicTick, 2) + "*" + spell.Duration + "sec Duration\n" + Math.Round(spell.PeriodicTick * 6f, 2) + " - " + Math.Round(spell.PeriodicTick * 9f, 2) + " Swiftmend");
            dictValues.Add("RG HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + "sec Casttime");
            dictValues.Add("RG HPS (HoT)", Math.Round(spell.HPSHoT, 2).ToString());
            dictValues.Add("RG HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.manaCost, 2) + " Manacost");
            dictValues.Add("RG HPM (spam)", Math.Round(spell.AverageHealingwithCrit / spell.manaCost, 2).ToString());

            spell = new Lifebloom(this, BasicStats);
            dictValues.Add("LB Tick", Math.Round(spell.PeriodicTick, 2) + "*" + spell.Duration + "sec Duration");
            dictValues.Add("LB Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.AverageHealing * 1.5f, 2) + " Crit\n" + spell.CritPercent + "%");
            dictValues.Add("LB HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.manaCost, 2) + " Manacost");

            spell = new LifebloomStack(this, BasicStats);
            dictValues.Add("LBS Tick", Math.Round(spell.PeriodicTick, 2).ToString());
            dictValues.Add("LBS HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.manaCost, 2) + " Manacost");

            spell = new Rejuvenation(this, BasicStats);
            dictValues.Add("RJ Tick", Math.Round(spell.PeriodicTick, 2) + "*" + spell.Duration + "sec Duration\n" + Math.Round(spell.PeriodicTick * 4f, 2) + " - " + Math.Round(spell.PeriodicTick * 6f, 2) + " Swiftmend");
            dictValues.Add("RJ HPS", Math.Round(spell.HPSHoT, 2).ToString());
            dictValues.Add("RJ HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.manaCost, 2) + " Manacost");

            spell = new HealingTouch(this, BasicStats);
            dictValues.Add("HT Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + spell.CritPercent + "% Crit");
            dictValues.Add("HT HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + "sec Casttime" + (spell.castTimeBeforeHaste < spell.CastTime ? ", GCD capped" : ""));
            dictValues.Add("HT HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.manaCost, 2) + " Manacost");

            spell = new WildGrowth(this, BasicStats);
            {
                WildGrowth wg = (WildGrowth)spell;
                string tmp = "";
                for (int i = 0; i <= 6; i++)
                    tmp += (i + 1) + ".Tick - " + Math.Round(wg.getTick(1, i), 2) + "\n";

                dictValues.Add("WG first Tick", Math.Round(wg.getTick(1, 0), 2) + "*" + tmp);
                dictValues.Add("WG HPS(single)", Math.Round(wg.PeriodicTick, 2).ToString());
                dictValues.Add("WG HPM(single)", Math.Round(wg.HPM, 2) + "*" + Math.Round(wg.PeriodicTick * wg.PeriodicTicks, 2) + " Health\n" + Math.Round(wg.manaCost, 2) + " Manacost");
                dictValues.Add("WG HPS(max)", Math.Round(wg.PeriodicTick * 5, 2).ToString());
                dictValues.Add("WG HPM(max)", Math.Round(wg.HPM * 5, 2) + "*" + Math.Round(wg.PeriodicTick * 5 * wg.PeriodicTicks, 2) + " Health\n" + Math.Round(wg.manaCost, 2) + " Manacost");
            }

            spell = new Nourish(this, BasicStats);
            dictValues.Add("N Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + Math.Round(spell.CritPercent, 2) + "% Crit");
            dictValues.Add("N HPM", Math.Round(spell.HPM,2).ToString());
            dictValues.Add("N HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + " Casttime");
            spell = new Nourish(this, BasicStats, 3);
            dictValues.Add("N (HoT) Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + Math.Round(spell.CritPercent, 2) + "% Crit");
            dictValues.Add("N (HoT) HPM", Math.Round(spell.HPM, 2).ToString());
            dictValues.Add("N (HoT) HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + " Casttime");
            return dictValues;
        }
    }
}
