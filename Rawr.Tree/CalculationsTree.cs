using System;
using System.Collections.Generic;

using Rawr;

namespace Rawr.Tree
{
    [System.ComponentModel.DisplayName("Tree|Ability_Druid_TreeofLife")]
    public class CalculationsTree : CalculationsBase
    {
        public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Druid; } }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("HpS", System.Drawing.Color.Red);
                    _subPointNameColors.Add("Mp5", System.Drawing.Color.Blue);
                    _subPointNameColors.Add("Survival", System.Drawing.Color.Green);
                    _subPointNameColors.Add("ToL", System.Drawing.Color.Yellow);
                }
                return _subPointNameColors;
            }
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Health",
					"Basic Stats:Mana",
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Healing",
					"Basic Stats:Mp5",
					"Basic Stats:Spell Crit",
					"Basic Stats:Spell Haste",

                    "Extended Stats:Mana per Cast (5%)",
                    "Extended Stats:HPS Points",
                    "Extended Stats:Mp5 Points",
                    "Extended Stats:Survival Points",
                    "Extended Stats:ToL Points",
                    "Extended Stats:Overall Points",

                    
					"Rotation:Rotation duration",
					"Rotation:Rotation cost",
					"Rotation:Rotation HPS",
					"Rotation:Rotation HPM",
					"Rotation:Max fight duration",
            	    
                    "Lifebloom:LB Tick","Lifebloom:LB Heal","Lifebloom:LB HPS","Lifebloom:LB HPM",
                    "Lifebloom Stack:LBS Tick","Lifebloom Stack:LBS HPS","Lifebloom Stack:LBS HPM",
                    "Rejuvenation:RJ Tick","Rejuvenation:RJ HPS","Rejuvenation:RJ HPM",
                    "Regrowth:RG Tick","Regrowth:RG Heal","Regrowth:RG HPS","Regrowth:RG HPM",
					"Healing Touch:HT Heal","Healing Touch:HT HPS","Healing Touch:HT HPM",
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelTree();
                }
                return _calculationOptionsPanel;
            }
        }

        public override string[] CustomChartNames
        {
            get
            {
                return new string[] {
                "Relative Stat Values (Bigger Picture)"
            };
            }
        }

        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationTree(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsTree(); }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    // I don't know of a fist weapon or two hand mace with healing stats, so...
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]{
                        Item.ItemType.None,
                        Item.ItemType.Cloth,
                        Item.ItemType.Leather,
                        Item.ItemType.Dagger,
                        Item.ItemType.Idol,
                        Item.ItemType.OneHandMace,
                        Item.ItemType.Staff
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;
            CharacterCalculationsTree calculatedStats = new CharacterCalculationsTree();

            calculatedStats.BasicStats = GetCharacterStats(character, additionalItem);

            calculatedStats.BasicStats.SpellCrit = (float)Math.Round((calculatedStats.BasicStats.Intellect / 80) +
                (calculatedStats.BasicStats.SpellCritRating / 22.08) + 1.85 + calcOpts.NaturalPerfection, 2);

            calculatedStats.BasicStats.SpellCombatManaRegeneration += 0.1f * calcOpts.Intensity;

            calculatedStats.BasicStats.TreeOfLifeAura += (calculatedStats.BasicStats.Spirit / 4f);
            calculatedStats.BasicStats.TreeOfLifeAura *= calcOpts.TreeOfLife;

            if (calculatedStats.BasicStats.ShatteredSunRestoProc > 0 && calcOpts.ShattrathFaction == "Aldor")
            {
                calculatedStats.BasicStats.AverageHeal += 44; // 1 proc/50 sec
            }

            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.HealingDoneFor15SecOnUse2Min / 8;
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.HealingDoneFor15SecOnUse90Sec / 6;
            calculatedStats.BasicStats.AverageHeal += calculatedStats.BasicStats.HealingDoneFor20SecOnUse2Min / 6;


            float baseRegenConstant = 0.00932715221261f;
            float spiritRegen = 0.001f + baseRegenConstant * (float)Math.Sqrt(calculatedStats.BasicStats.Intellect) * calculatedStats.BasicStats.Spirit;

            calculatedStats.OS5SRRegenRaw = spiritRegen + calculatedStats.BasicStats.Mp5 / 5f;
            calculatedStats.IS5SRRegenRaw = spiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5 / 5f;

            if (calculatedStats.BasicStats.SpiritFor20SecOnUse2Min > 0)
            {
                spiritRegen = 0.001f + baseRegenConstant * (float)Math.Sqrt(calculatedStats.BasicStats.Intellect) * (calculatedStats.BasicStats.Spirit + calculatedStats.BasicStats.SpiritFor20SecOnUse2Min);
                calculatedStats.OS5SRRegen = calculatedStats.OS5SRRegenRaw * 5f / 6f + (spiritRegen + calculatedStats.BasicStats.Mp5 / 5f) / 6f;
                calculatedStats.IS5SRRegen = calculatedStats.IS5SRRegenRaw * 5f / 6f + (spiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5 / 5f) / 6f;
            }
            else
            {
                calculatedStats.OS5SRRegen = calculatedStats.OS5SRRegenRaw;
                calculatedStats.IS5SRRegen = calculatedStats.IS5SRRegenRaw;
            }

            Spell lbs = new LifebloomStack(character, calculatedStats.BasicStats, true);
            Spell lb = new Lifebloom(character, calculatedStats.BasicStats, true);
            Spell rj = new Rejuvenation(character, calculatedStats.BasicStats, true);
            Spell rg = new Regrowth(character, calculatedStats.BasicStats, true);

            Spell ht = new HealingTouch(character, calculatedStats.BasicStats);
            Spell lbsrh = new LifebloomStack(character, calculatedStats.BasicStats, false);
            Spell lbrh = new Lifebloom(character, calculatedStats.BasicStats, false);
            Spell rjrh = new Rejuvenation(character, calculatedStats.BasicStats, false);
            Spell rgrh = new Regrowth(character, calculatedStats.BasicStats, false);
            Spell nothing = new Nothing(character, calculatedStats.BasicStats);


            calculatedStats.Spells = new List<Spell>();
            calculatedStats.Spells.Add(lbs);
            calculatedStats.Spells.Add(lb);
            calculatedStats.Spells.Add(rj);
            calculatedStats.Spells.Add(rg);
            calculatedStats.Spells.Add(ht);
            calculatedStats.Spells.Add(lbrh);
            calculatedStats.Spells.Add(rjrh);
            calculatedStats.Spells.Add(rgrh);
            calculatedStats.Spells.Add(lbsrh);
            calculatedStats.Spells.Add(nothing);

            calculatedStats.FightLength = calcOpts.FightLength;

            // Calculate scores in another function later to reduce clutter
            int health = (int)calculatedStats.BasicStats.Health;
            int healthBelow = (int)(health < calcOpts.TargetHealth ? health : calcOpts.TargetHealth);
            int healthAbove = health - healthBelow;

            calculatedStats.AddMp5Points(calculatedStats.IS5SRRegenRaw * 5f, "Regen");
            calculatedStats.AddMp5Points((calculatedStats.IS5SRRegen - calculatedStats.IS5SRRegenRaw) * 5f, "Spirit on Use (20 sec/2min)"); 
            calculatedStats.AddMp5Points(calcOpts.Spriest, "Shadow Priest");
            calculatedStats.AddMp5Points((calcOpts.ManaPotAmt * (1 + calculatedStats.BasicStats.BonusManaPotion)) / (calcOpts.ManaPotDelay * 12), "Potion");

            calculatedStats.solver = new Solver(calcOpts, calculatedStats); // getBestSpellRotation(calcOpts, calculatedStats);

            if (calculatedStats.solver.bestRotation != null)
            {
                calculatedStats.HpSPoints = calculatedStats.solver.HpS;
                calculatedStats.AddMp5Points(calculatedStats.solver.InnervateMp5, "Innervate");
                calculatedStats.AddMp5Points(calculatedStats.solver.ManaProcOnCastMp5, "Mana per Cast (5%)");
                calculatedStats.AddMp5Points(calculatedStats.solver.LessMana_15s_1m_Mp5, "Less Mana per Cast (15 sec/1min)");
                calculatedStats.AddMp5Points(calculatedStats.solver.BlueDragonMp5, "Blue Dragon");
                calculatedStats.AddMp5Points(calculatedStats.solver.BangleMp5, "Bangle");
                calculatedStats.AddMp5Points(calculatedStats.solver.MementoMp5, "Memento of Tyrande");
                calculatedStats.SurvivalPoints = healthBelow / calcOpts.SurvScalingBelow + healthAbove / calcOpts.SurvScalingAbove;
                calculatedStats.ToLPoints = calculatedStats.BasicStats.TreeOfLifeAura;
            }
            else
            {
                calculatedStats.HpSPoints = 0;
                calculatedStats.SurvivalPoints = 0;
                calculatedStats.ToLPoints = 0;
            }

            calculatedStats.OverallPoints = calculatedStats.HpSPoints + calculatedStats.Mp5Points + calculatedStats.SurvivalPoints + calculatedStats.ToLPoints;

            return calculatedStats;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsTree calcOpts = character.CalculationOptions as CalculationOptionsTree;

            Stats statsRace = character.Race == Character.CharacterRace.NightElf ?
                new Stats()
                {
                    Health = 3434f,
                    Mana = 2470f,
                    Stamina = 82f,
                    Agility = 75f,
                    Intellect = 120f,
                    Spirit = 133f,
                    BonusAgilityMultiplier = 0.01f * calcOpts.SotF,
                    BonusIntellectMultiplier = 0.01f * calcOpts.SotF,
                    BonusSpiritMultiplier = 0.01f * calcOpts.SotF,
                    BonusStaminaMultiplier = 0.01f * calcOpts.SotF,
                    BonusStrengthMultiplier = 0.01f * calcOpts.SotF,
                } :
                new Stats()
                {
                    Health = 3434f,
                    Mana = 2370f,
                    Stamina = 85f,
                    Agility = 64.5f,
                    Intellect = 115f,
                    Spirit = 135f,
                    BonusHealthMultiplier = 0.05f,
                    BonusAgilityMultiplier = 0.01f * calcOpts.SotF,
                    BonusIntellectMultiplier = 0.01f * calcOpts.SotF,
                    BonusSpiritMultiplier = 0.01f * calcOpts.SotF,
                    BonusStaminaMultiplier = 0.01f * calcOpts.SotF,
                    BonusStrengthMultiplier = 0.01f * calcOpts.SotF,
                };

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;

            statsTotal.Agility = (float)Math.Floor((statsTotal.Agility) * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor((statsTotal.Intellect) * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Intellect = (float)Math.Round((statsTotal.Intellect) * (1 + calcOpts.HotW * 0.04f));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + calcOpts.LivingSpirit * 0.05f));

            float lunarGuidance = (calcOpts.LunarGuidance == 3 ? 0.25f : calcOpts.LunarGuidance * 0.08f);
            statsTotal.SpellDamageRating = (float)Math.Round(statsTotal.SpellDamageRating + statsTotal.Intellect * lunarGuidance);
            statsTotal.Healing = (float)Math.Round(statsTotal.Healing + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit) + (statsTotal.Intellect * lunarGuidance) + (calcOpts.NurturingInstinct * 0.5f * statsTotal.Agility));
            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f);

            statsTotal.Health = (float)Math.Round(((statsTotal.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Mp5 += (float)Math.Floor(statsTotal.Intellect * (calcOpts.Dreamstate > 0 ? calcOpts.Dreamstate * 0.03f + 0.01f : 0f)); 
            return statsTotal;
        }

        private ComparisonCalculationTree GetRelativeValue(Stats baseStats, int minMultiplier, int maxMultiplier, String label, Character character, CharacterCalculationsTree calcBaseValue)
        {
            ComparisonCalculationTree compCalc = null;
            Stats stats = new Stats();
            for (int multiplier = minMultiplier; multiplier < maxMultiplier; multiplier++)
            {
                stats += baseStats;
                CharacterCalculationsTree calcValue = GetCharacterCalculations(character, new Item() { Stats = stats } ) as CharacterCalculationsTree;

                if (compCalc == null || (compCalc.OverallPoints < (calcValue.OverallPoints - calcBaseValue.OverallPoints) / multiplier))
                {
                    compCalc = new ComparisonCalculationTree(label + "*" + multiplier)
                    {
                        OverallPoints = (calcValue.OverallPoints - calcBaseValue.OverallPoints) / multiplier,
                        HpSPoints = (calcValue.HpSPoints - calcBaseValue.HpSPoints) / multiplier,
                        Mp5Points = (calcValue.Mp5Points - calcBaseValue.Mp5Points) / multiplier,
                        SurvivalPoints = (calcValue.SurvivalPoints - calcBaseValue.SurvivalPoints) / multiplier,
                        ToLPoints = (calcValue.ToLPoints - calcBaseValue.ToLPoints) / multiplier,
                    };
                }
            }
            return compCalc;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Relative Stat Values (Bigger Picture)":
                    int multiplier = 5;
                    CharacterCalculationsTree calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsTree;

                    CharacterCalculationsTree calcMp5Value = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 4 * multiplier } }) as CharacterCalculationsTree;
                    CharacterCalculationsTree calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellHasteRating = 10 * multiplier } }) as CharacterCalculationsTree;
                    CharacterCalculationsTree calcSpiritValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 10 * multiplier } }) as CharacterCalculationsTree;
                    CharacterCalculationsTree calcIntValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 10 * multiplier } }) as CharacterCalculationsTree;

                    

                    return new ComparisonCalculationBase[] { 
						GetRelativeValue (new Stats() {Healing = 22}, 5, 10, "22 Healing", character, calcBaseValue),
                        GetRelativeValue (new Stats() {Mp5 = 4}, 5, 10, "4 Mp5", character, calcBaseValue),
                        GetRelativeValue (new Stats() {SpellHasteRating = 10}, 1, 15, "10 Spell Haste", character, calcBaseValue),
                        GetRelativeValue (new Stats() {Spirit = 10}, 5, 10, "10 Spirit", character, calcBaseValue),
                        GetRelativeValue (new Stats() {Intellect = 10}, 5, 10, "10 Intellect", character, calcBaseValue),
                        GetRelativeValue (new Stats() {Healing = 11, Spirit = 5}, 5, 10, "11 Healing 5 Spirit", character, calcBaseValue),
                        GetRelativeValue (new Stats() {Healing = 11, Intellect = 5}, 5, 10, "11 Healing 5 Intellect", character, calcBaseValue),
					};
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Mp5 = stats.Mp5,
                Healing = stats.Healing,
                SpellCritRating = stats.SpellCritRating,
                SpellHasteRating = stats.SpellHasteRating,
                Health = stats.Health,
                Mana = stats.Mana,
                Spirit = stats.Spirit,
                BonusManaPotion = stats.BonusManaPotion,
                MementoProc = stats.MementoProc,
                AverageHeal = stats.AverageHeal,
                ManaRestorePerCast_5_15 = stats.ManaRestorePerCast_5_15,
                LifebloomFinalHealBonus = stats.LifebloomFinalHealBonus,
                RegrowthExtraTicks = stats.RegrowthExtraTicks,
                BonusHealingTouchMultiplier = stats.BonusHealingTouchMultiplier,
                TreeOfLifeAura = stats.TreeOfLifeAura,
                ReduceRejuvenationCost = stats.ReduceRejuvenationCost,
                ReduceRegrowthCost = stats.ReduceRegrowthCost,
                ReduceHealingTouchCost = stats.ReduceHealingTouchCost,
                RejuvenationHealBonus = stats.RejuvenationHealBonus,
                LifebloomTickHealBonus = stats.LifebloomTickHealBonus,
                HealingTouchFinalHealBonus = stats.HealingTouchFinalHealBonus,
                ShatteredSunRestoProc = stats.ShatteredSunRestoProc,
                BangleProc = stats.BangleProc,
                SpiritFor20SecOnUse2Min = stats.SpiritFor20SecOnUse2Min,
                ManacostReduceWithin15OnUse1Min = stats.ManacostReduceWithin15OnUse1Min,
                FullManaRegenFor15SecOnSpellcast = stats.FullManaRegenFor15SecOnSpellcast,
                HealingDoneFor15SecOnUse2Min = stats.HealingDoneFor15SecOnUse2Min,
                HealingDoneFor15SecOnUse90Sec = stats.HealingDoneFor15SecOnUse90Sec,
                HealingDoneFor20SecOnUse2Min = stats.HealingDoneFor20SecOnUse2Min,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            if (stats.Spirit + stats.Mp5 + stats.Healing
                + stats.SpellHasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.MementoProc + stats.AverageHeal
                + stats.ManaRestorePerCast_5_15 + stats.LifebloomFinalHealBonus + stats.RegrowthExtraTicks
                + stats.BonusHealingTouchMultiplier + stats.TreeOfLifeAura
                + stats.ReduceRejuvenationCost + stats.ReduceRegrowthCost + stats.ReduceHealingTouchCost
                + stats.RejuvenationHealBonus + stats.LifebloomTickHealBonus + stats.HealingTouchFinalHealBonus
                + stats.ShatteredSunRestoProc +
                + stats.FullManaRegenFor15SecOnSpellcast + stats.BangleProc > 0)
                return true;
            // This removes feral PvE items - they have Str, Sta and Int (but not Spirit, which means we still get buffs that raise all stats)
            // It does not remove S1 feral items sinc ethey have +healing
            if (stats.Strength + stats.Agility + stats.AttackPower > 0)
                return false;
            if (stats.SpellCombatManaRegeneration == 0.3f)
                return false;
            return (stats.SpellCombatManaRegeneration + stats.Stamina + stats.Intellect > 0);
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTree));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsTree calcOpts = serializer.Deserialize(reader) as CalculationOptionsTree;
            return calcOpts;
        }
    }
}
