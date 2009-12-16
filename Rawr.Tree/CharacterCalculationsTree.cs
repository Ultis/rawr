using System;
using System.Collections.Generic;

namespace Rawr.Tree {
    public class CharacterCalculationsTree : CharacterCalculationsBase {
        public Stats BasicStats { get; set; }
        public Stats CombatStats { get; set; }

        private float[] subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints {
            get { return subPoints; }
            set { subPoints = value; }
        }
        public float SingleTargetPoints {
            get { return subPoints[0]; }
            set { subPoints[0] = value; }
        }
        public float SustainedPoints {
            get { return subPoints[1]; }
            set { subPoints[1] = value; }
        }
        public float SurvivalPoints {
            get { return subPoints[2]; }
            set { subPoints[2] = value; }
        } 
        public override float OverallPoints { get; set; }
        public RotationResult Sustained;
        public SingleTargetBurstResult[] SingleTarget;
        public Character LocalCharacter { get; set; }
        public float SingleTargetHPS;
        public float SustainedHPS;

        string LifebloomMethod_ToString(int lbStack, LifeBloomType type) {
            string result;
            if (type == LifeBloomType.Fast)
            {
                switch (lbStack)
                {
                    case 0: result = "Unused"; break;
                    case 1: result = "Single blooms"; break;
                    case 2: result = "Fast Double blooms"; break;
                    case 3: result = "Fast Triple blooms"; break;
                    case 4:
                    default: result = "Stack"; break;
                }
            }
            else if (type == LifeBloomType.Slow)
            {
                switch (lbStack)
                {
                    case 0: result = "Unused"; break;
                    case 1: result = "Single blooms"; break;
                    case 2: result = "Slow Double blooms"; break;
                    case 3: result = "Slow Triple blooms"; break;
                    case 4:
                    default: result = "Stack"; break;
                }
            }
            else result = "Rolling stack";

            return result;
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues() {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Single Target Points", SingleTargetPoints.ToString());
            dictValues.Add("Sustained Points", SustainedPoints.ToString());
            dictValues.Add("Survival Points", SurvivalPoints.ToString());
            dictValues.Add("Overall Points", OverallPoints.ToString());

            dictValues.Add("Base Health", BasicStats.Health.ToString());
            dictValues.Add("Base Armor", Math.Round(BasicStats.Armor, 0) + "*Reduces damage taken by " + Math.Round(StatConversion.GetArmorDamageReduction(83, BasicStats.Armor, 0, 0, 0) * 100.0f, 2) + "%");
            dictValues.Add("Base Mana", BasicStats.Mana.ToString());
            dictValues.Add("Base Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Base Intellect", BasicStats.Intellect.ToString());
            dictValues.Add("Base Spirit", BasicStats.Spirit.ToString());
            dictValues.Add("Base Spell Power", (BasicStats.SpellPower).ToString() + "*" + BasicStats.Spirit * LocalCharacter.DruidTalents.ImprovedTreeOfLife * 0.05f + " ToL Bonus");
            dictValues.Add("Base Spell Crit", BasicStats.SpellCrit.ToString());

            float speed_from_hr = (1f + StatConversion.GetSpellHasteFromRating(BasicStats.HasteRating));
            float speed_from_sh = (1f + BasicStats.SpellHaste);
            float speed = speed_from_hr * speed_from_sh;
            float hard = (1.5f / (1f * speed_from_sh) - 1) * StatConversion.RATING_PER_SPELLHASTE;
            float untilcap = hard - BasicStats.HasteRating;

            dictValues.Add("Base Spell Haste", Math.Round((speed - 1.0f) * 100.0f, 2) + "%*" + Math.Round((speed_from_sh - 1.0f) * 100.0f, 2) + "% from spell effects and talents\n" + Math.Round((speed_from_hr - 1.0f) * 100.0f, 2) + "% from " + BasicStats.HasteRating + " haste rating");
            // Use Nourish cast time to equal normal GCD
            Spell spell = new Nourish(this, BasicStats);
            dictValues.Add("Base Global CD", Math.Round(spell.gcd, 2) + " sec*" + Math.Ceiling(untilcap).ToString() + " Haste Rating until hard gcd cap");

            dictValues.Add("Health", CombatStats.Health.ToString());
            dictValues.Add("Armor", Math.Round(CombatStats.Armor, 0) + "*Reduces damage taken by " + Math.Round(StatConversion.GetArmorDamageReduction(83, CombatStats.Armor, 0, 0, 0) * 100.0f, 2) + "%");
            dictValues.Add("Mana", CombatStats.Mana.ToString());
            dictValues.Add("Stamina", CombatStats.Stamina.ToString());
            dictValues.Add("Intellect", CombatStats.Intellect.ToString());
            dictValues.Add("Spirit", CombatStats.Spirit.ToString());
            dictValues.Add("Spell Power", (CombatStats.SpellPower).ToString() + "*" + CombatStats.Spirit * LocalCharacter.DruidTalents.ImprovedTreeOfLife * 0.05f + " ToL Bonus");
            dictValues.Add("Spell Crit", CombatStats.SpellCrit.ToString());

            speed_from_hr = (1f + StatConversion.GetSpellHasteFromRating(CombatStats.HasteRating));
            speed_from_sh = (1f + CombatStats.SpellHaste);
            speed = speed_from_hr * speed_from_sh;
            hard = (1.5f / (1f * speed_from_sh) - 1) * StatConversion.RATING_PER_SPELLHASTE;
            untilcap = hard - CombatStats.HasteRating;

            dictValues.Add("Spell Haste", Math.Round((speed - 1.0f) * 100.0f, 2) + "%*" + Math.Round((speed_from_sh - 1.0f) * 100.0f, 2) + "% from spell effects and talents\n" + Math.Round((speed_from_hr - 1.0f) * 100.0f, 2) + "% from " + CombatStats.HasteRating + " haste rating");
            // Use Nourish cast time to equal normal GCD
            spell = Sustained.nourish[0];
            dictValues.Add("Global CD", Math.Round(spell.gcd, 2) + " sec*" + Math.Ceiling(untilcap).ToString() + " Haste Rating until soft gcd cap");

            dictValues.Add("Total Time", Math.Round(Sustained.TotalTime, 2).ToString());
            dictValues.Add("Time until OOM", Math.Round(Sustained.TimeToOOM_unreduced, 2).ToString());
            dictValues.Add("Time until OOM (reduced)", Math.Round(Sustained.TimeToOOM, 2).ToString());
            dictValues.Add("Total healing done", Math.Round(Sustained.TotalTime * SustainedHPS, 2).ToString()); // Has extra component from procs
            dictValues.Add("Sustained HPS", Math.Round(SustainedHPS, 2).ToString());
            dictValues.Add("Single Target HPS", Math.Round(SingleTargetHPS, 2).ToString());
            dictValues.Add("Mana regen per second", Math.Round(Sustained.ManaRegen, 2).ToString());
            dictValues.Add("Mana from potions", Math.Round(Sustained.PotionMana, 2).ToString());
            dictValues.Add("Mana from innervates", Math.Round(Sustained.InnervateMana, 2).ToString());
            dictValues.Add("Average casts per minute", Math.Round(Sustained.CastsPerMinute, 2).ToString());
            dictValues.Add("Average crits per minute", Math.Round(Sustained.CritsPerMinute, 2).ToString());
            dictValues.Add("Average heals per minute", Math.Round(Sustained.HealsPerMinute, 2).ToString());
            dictValues.Add("Rejuvenation casts per minute", Math.Round(Sustained.RejuvCPM, 2).ToString());
            dictValues.Add("Rejuvenation average up", Math.Round(Sustained.RejuvAvg, 2).ToString());
            dictValues.Add("Regrowth casts per minute", Math.Round(Sustained.RegrowthCPM, 2).ToString());
            dictValues.Add("Regrowth average up", Math.Round(Sustained.RegrowthAvg, 2).ToString());
            dictValues.Add("Lifebloom (stack) casts per minute", Math.Round(Sustained.LifebloomStackCPM, 2).ToString());
            dictValues.Add("Lifebloom (stack) average up", Math.Round(Sustained.LifebloomStackAvg, 2).ToString());
            dictValues.Add("Lifebloom (stack) method", LifebloomMethod_ToString(3, Sustained.rotSettings.lifeBloomType));
            dictValues.Add("Lifebloom casts per minute", Math.Round(Sustained.LifebloomCPM, 2).ToString());
            dictValues.Add("Lifebloom average up", Math.Round(Sustained.LifebloomAvg, 2).ToString());
            dictValues.Add("Nourish casts per minute", Math.Round(Sustained.NourishCPM, 2).ToString());
            //dictValues.Add("Healing Touch casts per minute", Math.Round(, 2).ToString());
            dictValues.Add("Swiftmend casts per minute", Math.Round(Sustained.SwiftmendCPM, 2).ToString());
            dictValues.Add("Wild Growth casts per minute", Math.Round(Sustained.WildGrowthCPM, 2).ToString());
            dictValues.Add("Revitalize procs per minute", Math.Round(Sustained.RevitalizeProcsPerMinute, 2).ToString());
            
            dictValues.Add("RJ Heal per tick", Math.Round(Sustained.rejuvenate.PeriodicTickwithCrit, 2).ToString());
            dictValues.Add("RJ Tick time", Math.Round(Sustained.rejuvenate.PeriodicTickTime, 2).ToString());
            dictValues.Add("RJ HPS", Math.Round(Sustained.rejuvenate.HPCTD, 2).ToString());
            dictValues.Add("RJ HPM", Math.Round(Sustained.rejuvenate.HPM, 2).ToString());
            dictValues.Add("RJ HPCT", Math.Round(Sustained.rejuvenate.HPCT, 2).ToString());

            dictValues.Add("RG Heal", Math.Round(Sustained.regrowth.AverageHealingwithCrit, 2).ToString());
            dictValues.Add("RG Tick", Math.Round(Sustained.regrowth.PeriodicTickwithCrit, 2).ToString());
            dictValues.Add("RG HPS", Math.Round(Sustained.regrowth.HPCTD, 2).ToString());
            dictValues.Add("RG HPS (HoT only)", Math.Round(Sustained.regrowth.HPSHoT, 2).ToString());
            dictValues.Add("RG HPM", Math.Round(Sustained.regrowth.HPM, 2).ToString());
            dictValues.Add("RG HPCT", Math.Round(Sustained.regrowth.HPCT, 2).ToString());
            dictValues.Add("RG Heal (spam)", Math.Round(Sustained.regrowth.AverageHealingwithCrit, 2).ToString());
            dictValues.Add("RG HPS (spam)", Math.Round(Sustained.regrowthAgain.HPS, 2).ToString());
            dictValues.Add("RG HPM (spam)", Math.Round(Sustained.regrowthAgain.HPM_DH, 2).ToString());
            dictValues.Add("RG HPCT (spam)", Math.Round(Sustained.regrowth.HPCT_DH, 2).ToString());

            dictValues.Add("LB Tick", Math.Round(Sustained.lifebloom.PeriodicTick, 2).ToString());
            dictValues.Add("LB Bloom Heal", Math.Round(Sustained.lifebloom.AverageHealingwithCrit, 2).ToString());
            dictValues.Add("LB HPS", Math.Round(Sustained.lifebloom.HPCTD, 2).ToString());
            dictValues.Add("LB HPS (HoT only)", Math.Round(Sustained.lifebloom.HPSHoT, 2).ToString());
            dictValues.Add("LB HPM", Math.Round(Sustained.lifebloom.HPM, 2).ToString());
            dictValues.Add("LB HPCT", Math.Round(Sustained.lifebloom.HPCT, 2).ToString());

            //dictValues.Add("LBx2 (fast stack) HPS", Math.Round(, 2).ToString());
            //dictValues.Add("LBx2 (fast stack) HPM", Math.Round(, 2).ToString());
            //dictValues.Add("LBx2 (fast stack) HPCT", Math.Round(, 2).ToString());
            dictValues.Add("LBx3 (fast stack) HPS", Math.Round(Sustained.lifebloomFastStack.HPCTD, 2).ToString());
            dictValues.Add("LBx3 (fast stack) HPM", Math.Round(Sustained.lifebloomFastStack.HPM, 2).ToString());
            dictValues.Add("LBx3 (fast stack) HPCT", Math.Round(Sustained.lifebloomFastStack.HPCT, 2).ToString());
            //dictValues.Add("LBx2 (slow stack) HPS", Math.Round(, 2).ToString());
            //dictValues.Add("LBx2 (slow stack) HPM", Math.Round(, 2).ToString());
            //dictValues.Add("LBx2 (slow stack) HPCT", Math.Round(, 2).ToString());
            dictValues.Add("LBx3 (slow stack) HPS", Math.Round(Sustained.lifebloomSlowStack.HPCTD, 2).ToString());
            dictValues.Add("LBx3 (slow stack) HPM", Math.Round(Sustained.lifebloomSlowStack.HPM, 2).ToString());
            dictValues.Add("LBx3 (slow stack) HPCT", Math.Round(Sustained.lifebloomSlowStack.HPCT, 2).ToString());
            dictValues.Add("LBx3 (rolling) Tick", Math.Round(Sustained.lifebloomRollingStack.PeriodicTick, 2).ToString());
            dictValues.Add("LBx3 (rolling) HPS", Math.Round(Sustained.lifebloomRollingStack.HPCTD, 2).ToString());
            dictValues.Add("LBx3 (rolling) HPM", Math.Round(Sustained.lifebloomRollingStack.HPM, 2).ToString());
            dictValues.Add("LBx3 (rolling) HPCT", Math.Round(Sustained.lifebloomRollingStack.HPCT, 2).ToString());

            dictValues.Add("HT Heal", Math.Round(Sustained.healingTouch.AverageHealingwithCrit, 2).ToString());
            dictValues.Add("HT HPS", Math.Round(Sustained.healingTouch.HPS, 2).ToString());
            dictValues.Add("HT HPM", Math.Round(Sustained.healingTouch.HPM_DH, 2).ToString());
            dictValues.Add("HT HPCT", Math.Round(Sustained.healingTouch.HPCT_DH, 2).ToString());

            dictValues.Add("WG first Tick", Math.Round(Sustained.wildGrowth.Tick[0], 2).ToString());
            dictValues.Add("WG HPS(single target)", Math.Round(Sustained.wildGrowth.HPCTD, 2).ToString());
            dictValues.Add("WG HPM(single target)", Math.Round(Sustained.wildGrowth.HPM, 2).ToString());
            dictValues.Add("WG HPS(max)", Math.Round(Sustained.wildGrowth.HPCTD * Sustained.wildGrowth.maxTargets, 2).ToString());
            dictValues.Add("WG HPM(max)", Math.Round(Sustained.wildGrowth.HPM * Sustained.wildGrowth.maxTargets, 2).ToString());

            dictValues.Add("N Heal", Math.Round(Sustained.nourish[0].AverageHealingwithCrit, 2).ToString());
            dictValues.Add("N HPM", Math.Round(Sustained.nourish[0].HPM_DH, 2).ToString());
            dictValues.Add("N HPS", Math.Round(Sustained.nourish[0].HPS, 2).ToString());
            dictValues.Add("N HPCT", Math.Round(Sustained.nourish[0].HPCT_DH, 2).ToString());
            dictValues.Add("N (1 HoT) Heal", Math.Round(Sustained.nourish[1].AverageHealingwithCrit, 2).ToString());
            dictValues.Add("N (1 HoT) HPM", Math.Round(Sustained.nourish[1].HPM_DH, 2).ToString());
            dictValues.Add("N (1 HoT) HPS", Math.Round(Sustained.nourish[1].HPS, 2).ToString());
            dictValues.Add("N (1 HoT) HPCT", Math.Round(Sustained.nourish[1].HPCT_DH, 2).ToString());
            dictValues.Add("N (2 HoTs) Heal", Math.Round(Sustained.nourish[2].AverageHealingwithCrit, 2).ToString());
            dictValues.Add("N (2 HoTs) HPM", Math.Round(Sustained.nourish[2].HPM_DH, 2).ToString());
            dictValues.Add("N (2 HoTs) HPS", Math.Round(Sustained.nourish[2].HPS, 2).ToString());
            dictValues.Add("N (2 HoTs) HPCT", Math.Round(Sustained.nourish[2].HPCT_DH, 2).ToString());
            dictValues.Add("N (3 HoTs) Heal", Math.Round(Sustained.nourish[3].AverageHealingwithCrit, 2).ToString());
            dictValues.Add("N (3 HoTs) HPM", Math.Round(Sustained.nourish[3].HPM_DH, 2).ToString());
            dictValues.Add("N (3 HoTs) HPS", Math.Round(Sustained.nourish[3].HPS, 2).ToString());
            dictValues.Add("N (3 HoTs) HPCT", Math.Round(Sustained.nourish[3].HPCT_DH, 2).ToString());
            dictValues.Add("N (4 HoTs) Heal", Math.Round(Sustained.nourish[4].AverageHealingwithCrit, 2).ToString());
            dictValues.Add("N (4 HoTs) HPM", Math.Round(Sustained.nourish[4].HPM_DH, 2).ToString());
            dictValues.Add("N (4 HoTs) HPS", Math.Round(Sustained.nourish[4].HPS, 2).ToString());
            dictValues.Add("N (4 HoTs) HPCT", Math.Round(Sustained.nourish[4].HPCT_DH, 2).ToString());

            Swiftmend swift = new Swiftmend(this, CombatStats, new Rejuvenation(this, CombatStats), null);
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
        public override float GetOptimizableCalculationValue(string calculation) {
            switch (calculation) {
                case "Mana": return CombatStats.Mana;
                case "MP5": return Sustained.MPSInFSR;
                case "Global CD": return (new Nourish(this, CombatStats)).gcd * 1000.0f;  // Use Nourish gcd to equal normal GCD
            }
            return 0f;
        }
    }
}
