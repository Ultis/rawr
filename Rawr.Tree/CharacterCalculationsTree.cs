using System;
using System.Collections.Generic;

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

        public float SurvivalPoints
        {
            get { return subPoints[2]; }
            set { subPoints[2] = value; }
        }

        public override float OverallPoints { get; set; }

        public float ManaRegInFSR { get; set; }
        public float ManaRegOutFSR { get; set; }

        public float TimeUntilOOM { get; set; }
        public float TotalHealing { get; set; }

        public float[] Simulation;
        public float ManaRegen;
        public float TimeToRegenFull;
        public float CvRFraction;
        public float replenishRegen;

        public Character LocalCharacter { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            float spi_from_trinkets = (BasicStats.SpiritFor20SecOnUse2Min / 6f);
            dictValues.Add("Spirit", BasicStats.Spirit.ToString() + (spi_from_trinkets > 0 || BasicStats.ExtraSpiritWhileCasting > 0 ? "*" + Math.Round(spi_from_trinkets, 2).ToString() + " Spirit from trinkets\n" + Math.Round(BasicStats.ExtraSpiritWhileCasting,2).ToString() + " Spirit while casting" : ""));
            dictValues.Add("Healing", (BasicStats.SpellPower + BasicStats.TreeOfLifeAura).ToString() + "*" + BasicStats.Spirit * LocalCharacter.DruidTalents.ImprovedTreeOfLife * 0.05f + " ToL Bonus\n" + BasicStats.AverageHeal + " average spell power" + (BasicStats.TrollDivinity>0?"\n58 Troll Divinity bonus":""));

            bool hasSpiWhileCasting = BasicStats.ExtraSpiritWhileCasting > 0;
            dictValues.Add("MP5", ManaRegInFSR.ToString() + "*" + ManaRegOutFSR.ToString() + " Out of FSR\n" + replenishRegen.ToString() + " From Replenishment" + (hasSpiWhileCasting?"\n(values include extra Spirit while casting)":""));
            dictValues.Add("Spell Crit", BasicStats.SpellCrit.ToString());
            float hr_from_trinkets = (BasicStats.SpellHasteFor10SecOnCast_10_45 + BasicStats.SpellHasteFor10SecOnHeal_10_45) * .17f;
            dictValues.Add("Spell Haste", Math.Round(BasicStats.HasteRating / TreeConstants.HasteRatingToHaste * 100, 2) + "%" + (hr_from_trinkets>0?"*"+Math.Round(hr_from_trinkets,2).ToString()+" Haste from trinkets":""));
            dictValues.Add("Global CD", Math.Round(1.5f / (1 + BasicStats.HasteRating / TreeConstants.HasteRatingToHaste), 2) + "sec");

            dictValues.Add("Time until OOM", TimeUntilOOM.ToString());
            dictValues.Add("Total healing done", TotalHealing.ToString());
            dictValues.Add("HPS for primary heal", Math.Round(Simulation[2],2).ToString());
            dictValues.Add("HPS for tank HoTs", Math.Round(Simulation[3],2).ToString());
            dictValues.Add("MPS for primary heal", Math.Round(Simulation[4],2).ToString());
            dictValues.Add("MPS for tank HoTs", Math.Round(Simulation[5],2).ToString());
            dictValues.Add("MPS for Wild Growth", Math.Round(Simulation[8], 2).ToString());
            dictValues.Add("Mana regen per second", Math.Round(ManaRegen/5, 2).ToString());
            dictValues.Add("HoT refresh fraction", Math.Round(Simulation[6], 2).ToString());
            dictValues.Add("Casts per minute until OOM", Math.Round(Simulation[7], 2).ToString());
            dictValues.Add("Time to regen full mana", Math.Round(TimeToRegenFull, 2).ToString());
            dictValues.Add("Cast% after OOM", Math.Round(CvRFraction, 2).ToString());

            dictValues.Add("HealBurst", BurstPoints.ToString());
            dictValues.Add("HealSustained", SustainedPoints.ToString());
            dictValues.Add("Survival", SurvivalPoints.ToString());
            dictValues.Add("Overall", OverallPoints.ToString());

            Spell spell = new Regrowth(this, true);
            dictValues.Add("RG Heal", Math.Round(spell.AverageHealing, 2) + "*" + 
                "Base: " + Math.Round(spell.BaseMinHeal, 2) + " - " + Math.Round(spell.BaseMaxHeal, 2) + "\n" +
                Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + 
                Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + 
                Math.Round(spell.CritPercent, 2) + "% Crit" + (((CalculationOptionsTree)LocalCharacter.CalculationOptions).glyphOfRegrowth ? "\nGlyphed" : ""));
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

            spell = new WildGrowth(this);
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

            spell = new Nourish(this);
            dictValues.Add("N Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + Math.Round(spell.CritPercent, 2) + "% Crit");
            dictValues.Add("N HPM", Math.Round(spell.HPM,2).ToString());
            dictValues.Add("N HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + " Casttime");
            spell = new Nourish(this, true);
            dictValues.Add("N (HoT) Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + Math.Round(spell.CritPercent, 2) + "% Crit");
            dictValues.Add("N (HoT) HPM", Math.Round(spell.HPM, 2).ToString());
            dictValues.Add("N (HoT) HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + " Casttime");
            return dictValues;
        }
    }
}
