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
            get { return new string[] {
                "Relative Stat Values (Bigger Picture)"
            }; }
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

            float baseRegenConstant = 0.00932715221261f;
            float spiritRegen = 0.001f + baseRegenConstant * (float)Math.Sqrt(calculatedStats.BasicStats.Intellect) * calculatedStats.BasicStats.Spirit;

            calculatedStats.OS5SRRegen = spiritRegen + calculatedStats.BasicStats.Mp5 / 5f;
            calculatedStats.IS5SRRegen = spiritRegen * calculatedStats.BasicStats.SpellCombatManaRegeneration + calculatedStats.BasicStats.Mp5 / 5f;

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

            calculatedStats.AddMp5Points(calculatedStats.IS5SRRegen * 5f, "Regen");
            calculatedStats.AddMp5Points(calcOpts.Spriest, "Shadow Priest");
            calculatedStats.AddMp5Points((calcOpts.ManaPotAmt * (1 + calculatedStats.BasicStats.BonusManaPotion)) / (calcOpts.ManaPotDelay * 12), "Potion");

            calculatedStats.solver = new Solver(calcOpts, calculatedStats); // getBestSpellRotation(calcOpts, calculatedStats);

            if (calculatedStats.solver.bestRotation != null)
            {
                calculatedStats.HpSPoints = calculatedStats.solver.HpS;
                calculatedStats.AddMp5Points(calculatedStats.solver.InnervateMp5, "Innervate");
                calculatedStats.AddMp5Points(calculatedStats.solver.ManaPerCastMp5, "Mana per Cast (5%)");
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
                    BonusSpiritMultiplier = calcOpts.LivingSpirit * 0.05f
                } :
                new Stats()
                {
                    Health = 3434f,
                    Mana = 2470f,
                    Stamina = 85f,
                    Agility = 64.5f,
                    Intellect = 115f,
                    Spirit = 135f,
                    BonusSpiritMultiplier = calcOpts.LivingSpirit * 0.05f,
                    BonusHealthMultiplier = 0.05f
                };

            Stats statsBaseGear = GetItemStats(character, additionalItem);
            Stats statsEnchants = GetEnchantsStats(character);
            Stats statsBuffs = GetBuffsStats(character.ActiveBuffs);

            Stats statsTotal = statsBaseGear + statsEnchants + statsBuffs + statsRace;

            statsTotal.Stamina = (float)Math.Floor((statsTotal.Stamina) * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Intellect = (float)Math.Floor((statsTotal.Intellect) * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Spirit = (float)Math.Floor((statsTotal.Spirit) * (1 + statsTotal.BonusSpiritMultiplier));
            statsTotal.Healing = (float)Math.Round(statsTotal.Healing + (statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit));
            statsTotal.Mana = statsTotal.Mana + ((statsTotal.Intellect - 20f) * 15f + 20f);
            statsTotal.Health = statsTotal.Health + (float)Math.Round(statsTotal.Stamina * 10f * (1 + statsTotal.BonusHealthMultiplier));

            return statsTotal;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            switch (chartName)
            {
                case "Relative Stat Values (Bigger Picture)":
                    int multiplier = 10;
                    CharacterCalculationsTree calcBaseValue = GetCharacterCalculations(character) as CharacterCalculationsTree;
                    CharacterCalculationsTree calcHealingValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Healing = 22 * multiplier } }) as CharacterCalculationsTree;
                    CharacterCalculationsTree calcMp5Value = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Mp5 = 4 * multiplier } }) as CharacterCalculationsTree;
                    CharacterCalculationsTree calcHasteValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { SpellHasteRating = 10 * multiplier } }) as CharacterCalculationsTree;
                    CharacterCalculationsTree calcSpiritValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = 10 * multiplier } }) as CharacterCalculationsTree;
                    CharacterCalculationsTree calcIntValue = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = 10 * multiplier } }) as CharacterCalculationsTree;

                    return new ComparisonCalculationBase[] { 
						new ComparisonCalculationTree("22 Healing") {
                            OverallPoints = (calcHealingValue.OverallPoints - calcBaseValue.OverallPoints) / multiplier, 
							HpSPoints = (calcHealingValue.HpSPoints - calcBaseValue.HpSPoints) / multiplier, 
                            Mp5Points = (calcHealingValue.Mp5Points - calcBaseValue.Mp5Points) / multiplier, 
                            SurvivalPoints = (calcHealingValue.SurvivalPoints - calcBaseValue.SurvivalPoints) / multiplier, 
                            ToLPoints = (calcHealingValue.ToLPoints - calcBaseValue.ToLPoints) / multiplier, 
						},
                        new ComparisonCalculationTree("4 Mp5") {
                            OverallPoints = (calcMp5Value.OverallPoints - calcBaseValue.OverallPoints) / multiplier, 
							HpSPoints = (calcMp5Value.HpSPoints - calcBaseValue.HpSPoints) / multiplier, 
                            Mp5Points = (calcMp5Value.Mp5Points - calcBaseValue.Mp5Points) / multiplier, 
                            SurvivalPoints = (calcMp5Value.SurvivalPoints - calcBaseValue.SurvivalPoints) / multiplier, 
                            ToLPoints = (calcMp5Value.ToLPoints - calcBaseValue.ToLPoints) / multiplier, 
                        },
						new ComparisonCalculationTree("10 Spell Haste") {
                            OverallPoints = (calcHasteValue.OverallPoints - calcBaseValue.OverallPoints) / multiplier, 
							HpSPoints = (calcHasteValue.HpSPoints - calcBaseValue.HpSPoints) / multiplier, 
                            Mp5Points = (calcHasteValue.Mp5Points - calcBaseValue.Mp5Points) / multiplier, 
                            SurvivalPoints = (calcHasteValue.SurvivalPoints - calcBaseValue.SurvivalPoints) / multiplier, 
                            ToLPoints = (calcHasteValue.ToLPoints - calcBaseValue.ToLPoints) / multiplier,
                        },
						new ComparisonCalculationTree("10 Spirit") {
                            OverallPoints = (calcSpiritValue.OverallPoints - calcBaseValue.OverallPoints) / multiplier, 
							HpSPoints = (calcSpiritValue.HpSPoints - calcBaseValue.HpSPoints) / multiplier, 
                            Mp5Points = (calcSpiritValue.Mp5Points - calcBaseValue.Mp5Points) / multiplier, 
                            SurvivalPoints = (calcSpiritValue.SurvivalPoints - calcBaseValue.SurvivalPoints) / multiplier, 
                            ToLPoints = (calcSpiritValue.ToLPoints - calcBaseValue.ToLPoints) / multiplier,
                        },
                        new ComparisonCalculationTree("10 Intellect") { 
                            OverallPoints = (calcIntValue.OverallPoints - calcBaseValue.OverallPoints) / multiplier, 
							HpSPoints = (calcIntValue.HpSPoints - calcBaseValue.HpSPoints) / multiplier, 
                            Mp5Points = (calcIntValue.Mp5Points - calcBaseValue.Mp5Points) / multiplier, 
                            SurvivalPoints = (calcIntValue.SurvivalPoints - calcBaseValue.SurvivalPoints) / multiplier, 
                            ToLPoints = (calcIntValue.ToLPoints - calcBaseValue.ToLPoints) / multiplier, 
                        },
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
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            return (stats.Stamina + stats.Health + stats.Intellect + stats.Spirit + stats.Mp5 + stats.Healing + stats.SpellCritRating
                + stats.SpellHasteRating + stats.BonusSpiritMultiplier + stats.SpellDamageFromSpiritPercentage + stats.BonusIntellectMultiplier
                + stats.BonusManaPotion + stats.MementoProc + stats.AverageHeal
                + stats.ManaRestorePerCast_5_15 + stats.LifebloomFinalHealBonus + stats.RegrowthExtraTicks
                + stats.BonusHealingTouchMultiplier + stats.TreeOfLifeAura
                + stats.ReduceRejuvenationCost + stats.ReduceRegrowthCost + stats.ReduceHealingTouchCost
                + stats.RejuvenationHealBonus + stats.LifebloomTickHealBonus + stats.HealingTouchFinalHealBonus
                ) > 0;
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
