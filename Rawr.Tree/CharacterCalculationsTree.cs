using System;
using System.Collections.Generic;

namespace Rawr.Tree
{
    public class CharacterCalculationsTree : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }

        private float[] subPoints = new float[] { 0f, 0f, 0f };
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

        public Rotation Simulation;

        public Character LocalCharacter { get; set; }

        public float haste { get; set; }
        float spellhaste { get; set; }
        float haste_until_hard_cap { get; set; }
        float haste_until_soft_cap { get; set; }

        public void doHasteCalcs()
        {
            haste = (1 + StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating));
            spellhaste = 1 + BasicStats.SpellHaste;
            float hard = (1.5f / (1f * spellhaste) - 1) * StatConversion.RATING_PER_SPELLHASTE;
            float soft = (1.5f * (1.0f - 0.04f * LocalCharacter.DruidTalents.GiftOfTheEarthmother) / (1.0f * spellhaste) - 1) * StatConversion.RATING_PER_SPELLHASTE;
            haste_until_hard_cap = hard - BasicStats.HasteRating;
            haste_until_soft_cap = soft - BasicStats.HasteRating;
        }

        string LifebloomMethod_ToString(int lbStack, bool LifebloomFastStacking)
        {
            string result;
            if (LifebloomFastStacking)
            {
                switch (lbStack)
                {
                    case 0: result = "Unused";
                        break;
                    case 1: result = "Single blooms";
                        break;
                    case 2: result = "Fast Double blooms";
                        break;
                    case 3: result = "Fast Triple blooms";
                        break;
                    case 4:
                    default: result = "Stack";
                        break;
                }
            }
            else
            {
                switch (lbStack)
                {
                    case 0: result = "Unused";
                        break;
                    case 1: result = "Single blooms";
                        break;
                    case 2: result = "Slow Double blooms";
                        break;
                    case 3: result = "Slow Triple blooms";
                        break;
                    case 4:
                    default: result = "Stack";
                        break;
                }
            }

            return result;
        }


        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("HealBurst", BurstPoints.ToString());
            dictValues.Add("HealSustained", SustainedPoints.ToString());
            dictValues.Add("Survival", SurvivalPoints.ToString());
            dictValues.Add("Overall", OverallPoints.ToString());

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Mana", BasicStats.Mana.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Intellect", BasicStats.Intellect.ToString());
            float spi_from_trinkets = (BasicStats.SpiritFor20SecOnUse2Min / 6f);
            dictValues.Add("Spirit", BasicStats.Spirit.ToString() + (spi_from_trinkets > 0 || BasicStats.ExtraSpiritWhileCasting > 0 ? "*" + Math.Round(spi_from_trinkets, 2).ToString() + " Spirit from trinkets\n" + Math.Round(BasicStats.ExtraSpiritWhileCasting,2).ToString() + " Spirit while casting" : ""));
            dictValues.Add("Healing", (BasicStats.SpellPower + BasicStats.TreeOfLifeAura).ToString() + "*" + BasicStats.Spirit * LocalCharacter.DruidTalents.ImprovedTreeOfLife * 0.05f + " ToL Bonus");

            bool hasSpiWhileCasting = BasicStats.ExtraSpiritWhileCasting > 0;
            dictValues.Add("MP5", Math.Round(Simulation.ManaPer5In5SR).ToString() + "*" + Math.Round(Simulation.ManaPer5Out5SR).ToString() + " Out of FSR\n" + Math.Round(Simulation.ReplenishRegen).ToString() + " From Replenishment\n" + (hasSpiWhileCasting ? "(values include extra Spirit while casting)\n" : "\n") + Math.Round(Simulation.ManaFromInnervate).ToString() + " extra mana from each Innervate");
            dictValues.Add("Spell Crit", BasicStats.SpellCrit.ToString());

            doHasteCalcs();

            dictValues.Add("Spell Haste", Math.Round((spellhaste * haste - 1.0f) * 100.0f, 2) + "%*" + Math.Round((spellhaste - 1.0f) * 100.0f, 2) + "% from spell effects and talents\n" + Math.Round((haste - 1.0f) * 100.0f, 2) + "% from " + BasicStats.HasteRating + " haste rating");
            // Use Nourish cast time to equal normal GCD
            Spell spell = new Nourish(this, BasicStats);
            dictValues.Add("Global CD", Math.Round(spell.gcd, 2) + " sec*" + Math.Round(haste_until_hard_cap, 0).ToString() + " Haste Rating until hard gcd cap");
            spell = new Lifebloom(this, BasicStats);
            dictValues.Add("Lifebloom Global CD", Math.Round(spell.CastTime, 2) + " sec*" + Math.Round(haste_until_soft_cap, 0).ToString() + " Haste Rating until Lifebloom (GotEM) gcd cap");

            dictValues.Add("Armor", Math.Round(BasicStats.Armor, 0) + "*Reduces damage taken by " + Math.Round(StatConversion.GetArmorDamageReduction(83, BasicStats.Armor, 0, 0, 0) * 100.0f, 2) + "%");

            if (Simulation.TotalTime - Simulation.TimeToOOM > 1.0)
            {
                dictValues.Add("Result", "OOM from tank HoTs");
                dictValues.Add("Time until OOM", Math.Round(Simulation.TimeToOOM,1).ToString() + " sec*" + "Keeping HoTs on the tank(s) will cause you to go OOM");
                dictValues.Add("Unused Mana Remaining", Math.Round(Simulation.UnusedMana, 0).ToString());
                dictValues.Add("Unused cast time percentage", Math.Round(Simulation.UnusedCastTimeFrac*100.0f, 0).ToString()+"%");
            }
            else if (Simulation.UnusedMana == 0)
            {
                dictValues.Add("Result", "Mana limited*This isn't neccesarily a problem, but means you cannot cast every available second, your mana needs to be managed to last");
                dictValues.Add("Time until OOM", "End of Fight*" + Simulation.TimeToOOM.ToString() + " sec");
                dictValues.Add("Unused Mana Remaining", Math.Round(Simulation.UnusedMana, 0).ToString());
                dictValues.Add("Unused cast time percentage", Math.Round(Simulation.UnusedCastTimeFrac*100.0f, 0).ToString()+"%* This indicates what percentage of the total fight time, you cannot cast your primary heals in order to avoid going out of mana");
            }
            else
            {
                dictValues.Add("Result", "Cast time limited*Mana shouldn't be a problem. You can cast as much as possible");
                dictValues.Add("Time until OOM", "Not during fight*" + Simulation.TimeToOOM.ToString() + " sec");
                dictValues.Add("Unused Mana Remaining", Math.Round(Simulation.UnusedMana, 0).ToString()+"* Indicates wasted mana you couldn't get time to spend");
                dictValues.Add("Unused cast time percentage", Math.Round(Simulation.UnusedCastTimeFrac*100.0f, 0).ToString()+"%");
            }
            dictValues.Add("Total healing done", Simulation.TotalHealing.ToString());


            dictValues.Add("Number of tanks", Simulation.rotSettings.noTanks.ToString());
            dictValues.Add("Lifebloom method", LifebloomMethod_ToString(Simulation.rotSettings.lifeBloomStackSize, Simulation.rotSettings.lifeBloomFastStack));
            dictValues.Add("Extra Tank HoTs", (Simulation.rotSettings.rgOnTank ? "Regrowth" : "") + (Simulation.rotSettings.rejuvOnTank ? " Rejuv" : ""));
            dictValues.Add("HPS for primary heal", Math.Round(Simulation.HPSFromPrimary,2).ToString());
            dictValues.Add("HPS for tank HoTs", Math.Round(Simulation.HPSFromHots, 2).ToString() + "*" + Math.Round(Simulation.HPSFromTrueHots, 2).ToString() + " from true HoTs\n" + Math.Round(Simulation.HPSFromHots - Simulation.HPSFromTrueHots, 2).ToString()+" in the form of bursts from Regrowth and LB Blooms");
            dictValues.Add("MPS for primary heal", Math.Round(Simulation.MPSFromPrimary,2).ToString());
            dictValues.Add("MPS for tank HoTs", Math.Round(Simulation.MPSFromHots,2).ToString());
            dictValues.Add("MPS for Wild Growth", Math.Round(Simulation.MPSFromWildGrowth, 2).ToString());
            dictValues.Add("HPS for Wild Growth", Math.Round(Simulation.HPSFromWildGrowth, 2).ToString());
            dictValues.Add("MPS for Swiftmend", Math.Round(Simulation.MPSFromSwiftmend, 2).ToString()+"* Doesn't include possible extra MPS to refresh HoTs, if Glyph of Swiftmend not used");
            dictValues.Add("HPS for Swiftmend", Math.Round(Simulation.HPSFromSwiftmend, 2).ToString());
            dictValues.Add("Spell for primary heal", Simulation.rotSettings.primaryHeal.ToString());
            dictValues.Add("Mana regen per second", Math.Round(Simulation.ManaPer5InRotation / 5, 2).ToString());
            dictValues.Add("HoT refresh fraction", Math.Round(Simulation.HotsFraction, 2).ToString());
            dictValues.Add("Casts per minute until OOM", Math.Round(Simulation.CastsPerMinute, 2).ToString());
            dictValues.Add("Crits per minute until OOM", Math.Round(Simulation.CritsPerMinute, 2).ToString());

             spell = new Regrowth(this, BasicStats, true);
            dictValues.Add("RG Heal", Math.Round(spell.AverageHealing, 2) + "*" + 
                "Base: " + Math.Round(spell.BaseMinHeal, 2) + " - " + Math.Round(spell.BaseMaxHeal, 2) + "\n" +
                Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + 
                Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + 
                Math.Round(spell.CritPercent, 2) + "% Crit" + ((LocalCharacter.DruidTalents.GlyphOfRegrowth) ? "\nGlyphed" : ""));
            dictValues.Add("RG Tick", Math.Round(spell.PeriodicTick, 2) + "*" + spell.Duration + "sec Duration\n" + Math.Round(spell.PeriodicTick * 6f, 2) + " - " + Math.Round(spell.PeriodicTick * 9f, 2) + " Swiftmend");
            dictValues.Add("RG HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + "sec Casttime");
            dictValues.Add("RG HPS (HoT)", Math.Round(spell.HPSHoT, 2).ToString());
            dictValues.Add("RG HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");
            dictValues.Add("RG HPM (spam)", Math.Round(spell.AverageHealingwithCrit / spell.ManaCost, 2).ToString());

            spell = new Lifebloom(this, BasicStats);
            dictValues.Add("LB Tick", Math.Round(spell.PeriodicTick, 2) + "*" + spell.Duration + "sec Duration");
            dictValues.Add("LB Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.AverageHealing * 1.5f, 2) + " Crit\n" + spell.CritPercent + "%");
            dictValues.Add("LB HPS", Math.Round(spell.TotalAverageHealing / spell.Duration, 2).ToString());
            dictValues.Add("LB HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");

            spell = new Lifebloom(this, BasicStats, 2, true);
            dictValues.Add("LBx2 (fast stack) HPS", Math.Round(spell.TotalAverageHealing / spell.Duration, 2).ToString());
            dictValues.Add("LBx2 (fast stack) HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");

            spell = new Lifebloom(this, BasicStats, 3, true);
            dictValues.Add("LBx3 (fast stack) HPS", Math.Round(spell.TotalAverageHealing / spell.Duration, 2).ToString());
            dictValues.Add("LBx3 (fast stack) HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");

            spell = new Lifebloom(this, BasicStats, 2, false);
            dictValues.Add("LBx2 (slow stack) HPS", Math.Round(spell.TotalAverageHealing / spell.Duration, 2).ToString());
            dictValues.Add("LBx2 (slow stack) HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");

            spell = new Lifebloom(this, BasicStats, 3, false);
            dictValues.Add("LBx3 (slow stack) HPS", Math.Round(spell.TotalAverageHealing / spell.Duration, 2).ToString());
            dictValues.Add("LBx3 (slow stack) HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");

            spell = new LifebloomStack(this, BasicStats);
            dictValues.Add("LBS Tick", Math.Round(spell.PeriodicTick, 2).ToString());
            dictValues.Add("LBS HPS", Math.Round(spell.TotalAverageHealing/spell.Duration, 2).ToString());
            dictValues.Add("LBS HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");

            spell = new Rejuvenation(this, BasicStats);
            dictValues.Add("RJ Tick", Math.Round(spell.PeriodicTick, 2) + "*" + spell.Duration + "sec Duration\n" + Math.Round(spell.PeriodicTick * 4f, 2) + " - " + Math.Round(spell.PeriodicTick * 6f, 2) + " Swiftmend");
            dictValues.Add("RJ HPS", Math.Round(spell.HPSHoT, 2).ToString());
            dictValues.Add("RJ HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");

            spell = new HealingTouch(this, BasicStats);
            dictValues.Add("HT Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + spell.CritPercent + "% Crit");
            dictValues.Add("HT HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + "sec Casttime" + (spell.castTimeBeforeHaste < spell.CastTime ? ", GCD capped" : ""));
            dictValues.Add("HT HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");

            spell = new WildGrowth(this, BasicStats);
            {
                WildGrowth wg = (WildGrowth)spell;
                string tmp = "";
                for (int i = 0; i <= 6; i++)
                    tmp += (i + 1) + ".Tick - " + Math.Round(wg.getTick(1, i), 2) + "\n";

                dictValues.Add("WG first Tick", Math.Round(wg.getTick(1, 0), 2) + "*" + tmp);
                dictValues.Add("WG HPS(single)", Math.Round(wg.PeriodicTick, 2).ToString());
                dictValues.Add("WG HPM(single)", Math.Round(wg.HPM, 2) + "*" + Math.Round(wg.PeriodicTick * wg.PeriodicTicks, 2) + " Health\n" + Math.Round(wg.ManaCost, 2) + " Manacost");
                dictValues.Add("WG HPS(max)", Math.Round(wg.PeriodicTick * wg.maxTargets, 2).ToString() + "*" + wg.maxTargets.ToString() + " targets being healed");
                dictValues.Add("WG HPM(max)", Math.Round(wg.HPM * wg.maxTargets, 2) + "*" + Math.Round(wg.PeriodicTick * wg.maxTargets * wg.PeriodicTicks, 2) + " Health\n" + Math.Round(wg.ManaCost, 2) + " Manacost");
            }

            spell = new Nourish(this, BasicStats);
            dictValues.Add("N Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + Math.Round(spell.CritPercent, 2) + "% Crit");
            dictValues.Add("N HPM", Math.Round(spell.HPM, 2) + "*" + Math.Round(spell.TotalAverageHealing, 2) + " Health\n" + Math.Round(spell.ManaCost, 2) + " Manacost");
            dictValues.Add("N HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + " Casttime");
            spell = new Nourish(this, BasicStats, 1);
            dictValues.Add("N (1 HoT) Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + Math.Round(spell.CritPercent, 2) + "% Crit");
            dictValues.Add("N (1 HoT) HPM", Math.Round(spell.HPM, 2).ToString());
            dictValues.Add("N (1 HoT) HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + " Casttime");
            spell = new Nourish(this, BasicStats, 3);
            dictValues.Add("N (3 HoTs) Heal", Math.Round(spell.AverageHealing, 2) + "*" + Math.Round(spell.MinHeal, 2) + " - " + Math.Round(spell.MaxHeal, 2) + "\n" + Math.Round(spell.MinHeal * 1.5f, 2) + " - " + Math.Round(spell.MaxHeal * 1.5f, 2) + "\n" + Math.Round(spell.CritPercent, 2) + "% Crit");
            dictValues.Add("N (3 HoTs) HPM", Math.Round(spell.HPM, 2).ToString());
            dictValues.Add("N (3 HoTs) HPS", Math.Round(spell.HPS, 2) + "*" + Math.Round(spell.CastTime, 2) + " Casttime");

            Swiftmend swift = new Swiftmend(this, BasicStats, new Rejuvenation(this, BasicStats), null);
            dictValues.Add("SM Rejuv Heal", Math.Round(swift.AverageHealing, 2) + "*" + Math.Round(swift.MinHeal, 2) + " - " + Math.Round(swift.MaxHeal, 2) + "\n" + Math.Round(swift.MinHeal * 1.5f, 2) + " - " + Math.Round(swift.MaxHeal * 1.5f, 2) + "\n" + Math.Round(swift.CritPercent, 2) + "% Crit");
            dictValues.Add("SM Rejuv HPM", Math.Round(swift.HPM, 2).ToString());
            dictValues.Add("SM Rejuv Lost Ticks", Math.Round(swift.rejuvTicksLost, 2).ToString());
            swift = new Swiftmend(this, BasicStats, null, new Regrowth(this, BasicStats));
            dictValues.Add("SM Regrowth Heal", Math.Round(swift.AverageHealing, 2) + "*" + Math.Round(swift.MinHeal, 2) + " - " + Math.Round(swift.MaxHeal, 2) + "\n" + Math.Round(swift.MinHeal * 1.5f, 2) + " - " + Math.Round(swift.MaxHeal * 1.5f, 2) + "\n" + Math.Round(swift.CritPercent, 2) + "% Crit");
            dictValues.Add("SM Regrowth HPM", Math.Round(swift.HPM, 2).ToString());
            dictValues.Add("SM Regrowth Lost Ticks", Math.Round(swift.regrowthTicksLost, 2).ToString());
            swift = new Swiftmend(this, BasicStats, new Rejuvenation(this, BasicStats), new Regrowth(this, BasicStats));
            dictValues.Add("SM Both Heal", Math.Round(swift.AverageHealing, 2) + "*" + Math.Round(swift.MinHeal, 2) + " - " + Math.Round(swift.MaxHeal, 2) + "\n" + Math.Round(swift.MinHeal * 1.5f, 2) + " - " + Math.Round(swift.MaxHeal * 1.5f, 2) + "\n" + Math.Round(swift.CritPercent, 2) + "% Crit");
            dictValues.Add("SM Both HPM", Math.Round(swift.HPM, 2).ToString());
            dictValues.Add("SM Both Rejuv Lost Ticks", Math.Round(swift.rejuvTicksLost, 2).ToString());
            dictValues.Add("SM Both Regrowth Lost Ticks", Math.Round(swift.regrowthTicksLost, 2).ToString());

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            doHasteCalcs();

            switch (calculation)
            {
                case "Mana": return BasicStats.Mana;
                case "MP5": return Simulation.ManaPer5In5SR;
                case "Spell Haste Percentage": return (spellhaste -1.0f) * 100.0f;
                case "Haste Percentage": return (haste - 1.0f) * 100.0f;
                case "Combined Haste Percentage": return (spellhaste * haste - 1.0f) * 100.0f;
                case "Haste until Lifebloom Cap": return haste_until_soft_cap;
                case "Haste until Hard Cap": return haste_until_hard_cap;
                case "GCD (milliseconds)": return (new Nourish(this, BasicStats)).gcd * 1000.0f;  // Use Nourish gcd to equal normal GCD
                case "Lifebloom GCD (milliseconds)": return (new Lifebloom(this, BasicStats)).CastTime * 1000.0f;
            }
            return 0f;
        }
    }
}
