using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Drawing;

namespace Rawr.Mage
{
	[Rawr.Calculations.RawrModelInfo("Mage", "Spell_Holy_MagicalSentry", Character.CharacterClass.Mage)]
    public sealed class CalculationsMage : CalculationsBase
    {
        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                if (_subPointNameColors == null)
                {
                    _subPointNameColors = new Dictionary<string, System.Drawing.Color>();
                    _subPointNameColors.Add("Dps", System.Drawing.Color.FromArgb(0, 128, 255));
                    _subPointNameColors.Add("Survivability", System.Drawing.Color.FromArgb(64, 128, 32));
                }
                return _subPointNameColors;
            }
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMage));
            System.IO.StringReader reader = new System.IO.StringReader(xml);
            CalculationOptionsMage calcOpts = serializer.Deserialize(reader) as CalculationOptionsMage;
            return calcOpts;
        }

        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels
        {
            get
            {
                if (_characterDisplayCalculationLabels == null)
                    _characterDisplayCalculationLabels = new string[] {
					"Basic Stats:Stamina",
					"Basic Stats:Intellect",
					"Basic Stats:Spirit",
					"Basic Stats:Armor",
					"Basic Stats:Health",
					"Basic Stats:Mana",
                    "Spell Stats:Crit Rate",
                    "Spell Stats:Hit Rate",
                    "Spell Stats:Spell Penetration",
                    "Spell Stats:Casting Speed",
                    "Spell Stats:Arcane Damage",
                    "Spell Stats:Fire Damage",
                    "Spell Stats:Frost Damage",
                    "Solution:Total Damage",
                    "Solution:Score",
                    "Solution:Dps",
                    "Solution:Tps*Threat per second",
                    "Solution:Spell Cycles",
                    "Solution:By Spell",
                    "Solution:Sequence*Cycle sequence reconstruction based on optimum cycles",
                    "Spell Info:Wand",
                    "Spell Info:Arcane Missiles",
                    "Spell Info:MBAM*Missile Barrage Arcane Missiles",
                    "Spell Info:Arcane Blast*Spammed",
                    "Spell Info:Arcane Blast(3)*Full debuff stack",
                    "Spell Info:Arcane Blast(0)*Non-debuffed",
                    "Spell Info:Arcane Barrage*Requires talent points",
                    "Spell Info:Scorch",
                    "Spell Info:Fire Blast",
                    "Spell Info:Pyroblast*Requires talent points",
                    "Spell Info:Fireball",
                    "Spell Info:FBPyro*Pyroblast on Hot Streak",
                    "Spell Info:FBLBPyro*Pyroblast on Hot Streak, maintain Living Bomb dot",
                    "Spell Info:ScLBPyro*Pyroblast on Hot Streak, maintain Living Bomb dot",
                    "Spell Info:FBSc*Must enable Maintain Scorch and have points in Improved Scorch talent to enable",
                    "Spell Info:FBScPyro*Maintain Scorch and Pyroblast on Hot Streak",
                    "Spell Info:FBScLBPyro*Maintain Scorch, maintain Living Bomb dot and Pyroblast on Hot Streak",
                    "Spell Info:FBFBlast",
                    "Spell Info:Living Bomb",
                    "Spell Info:Frostfire Bolt",
                    "Spell Info:FFBPyro*Pyroblast on Hot Streak",
                    "Spell Info:FFBLBPyro*Pyroblast on Hot Streak, maintain Living Bomb dot",
                    "Spell Info:FFBScPyro*Maintain Scorch and Pyroblast on Hot Streak",
                    "Spell Info:FFBScLBPyro*Maintain Scorch, maintain Living Bomb dot and Pyroblast on Hot Streak",
                    "Spell Info:Frostbolt",
                    "Spell Info:FrBFB*Fireball on Brain Freeze",
                    "Spell Info:ABP*Pause to let debuff drop",
                    "Spell Info:ABAM",
                    "Spell Info:ABarAM",
                    "Spell Info:ABABar*AB-ABar, on Missile Barrage replace with MBAM-ABar",
                    "Spell Info:ABABarX*AB-ABar, on Missile Barrage replace with AB-MBAM-ABar, only applies in 3.0.8 mode",
                    "Spell Info:ABABarY*AB-ABar, on Missile Barrage replace with AB-MBAM, only applies in 3.0.8 mode",
                    "Spell Info:AB2ABar*AB-AB-ABar, if any of the spells procs Missile Barrage replace next section with MBAM-AB-ABar",
                    "Spell Info:AB2ABarMBAM*AB-AB-ABar, if first AB procs MB insert MBAM before ABar, otherwise add MBAM after ABar",
                    "Spell Info:AB3ABar*AB-AB-AB-ABar, if MB on first 2 casts do MBAM as soon as you notice, followed by ABar, otherwise MBAM after ABar",
                    "Spell Info:AB3ABarX*AB-AB-AB-ABar, always ramp to 3 AB, MBAM either before or after ABar based on when it procs",
                    "Spell Info:AB3ABarY*AB-AB-AB-ABar, MBAM as soon as you notice proc, restart cycle on MBAM (don't follow with ABar)",
                    "Spell Info:ABABarSlow*Arcane Missiles on Missile Barrage (after Arcane Barrage), maintain Slow",
                    "Spell Info:ABMBAM*Arcane Missiles on Missile Barrage",
                    "Spell Info:FBABar*Arcane Missiles on Missile Barrage (after Arcane Barrage)",
                    "Spell Info:FB2ABar*Arcane Missiles on Missile Barrage (after Arcane Barrage)",
                    "Spell Info:FBABarSlow*Arcane Missiles on Missile Barrage (after Arcane Barrage), maintain Slow",
                    "Spell Info:FrBABar*Arcane Missiles on Missile Barrage (after Arcane Barrage)",
                    "Spell Info:FrB2ABar*Arcane Missiles on Missile Barrage (after Arcane Barrage)",
                    "Spell Info:FrBABarSlow*Arcane Missiles on Missile Barrage (after Arcane Barrage), maintain Slow",
                    "Spell Info:FFBABar*Arcane Missiles on Missile Barrage (after Arcane Barrage)",
                    "Spell Info:Arcane Explosion",
                    "Spell Info:Blizzard",
                    "Spell Info:Cone of Cold",
                    "Spell Info:Flamestrike*Spammed",
                    "Spell Info:Blast Wave*Requires talent points",
                    "Spell Info:Dragon's Breath*Requires talent points",
                    "Regeneration:MP5",
                    "Regeneration:Mana Regen",
                    "Regeneration:Health Regen",
                    "Survivability:Arcane Resist",
                    "Survivability:Fire Resist",
                    "Survivability:Nature Resist",
                    "Survivability:Frost Resist",
                    "Survivability:Shadow Resist",
                    "Survivability:Physical Mitigation",
                    "Survivability:Resilience",
                    "Survivability:Defense",
                    "Survivability:Crit Reduction",
                    "Survivability:Dodge",
                    "Survivability:Mean Incoming Dps",
                    "Survivability:Chance to Die",
				};
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _customChartNames = null;
        public override string[] CustomChartNames
        {
            get
            {
                if (_customChartNames == null)
                    _customChartNames = new string[] { "Talents (per talent point)", "Talent Specs", "Item Budget", "Glyphs" };
                return _customChartNames;
            }
        }

        private string[] _customRenderedChartNames = null;
        public override string[] CustomRenderedChartNames
        {
            get
            {
                if (_customRenderedChartNames == null)
                {
                    _customRenderedChartNames = new string[] { "Sequence Reconstruction" };
                }
                return _customRenderedChartNames;
            }
        }

        private CalculationOptionsPanelBase _calculationOptionsPanel = null;
        public override CalculationOptionsPanelBase CalculationOptionsPanel
        {
            get
            {
                if (_calculationOptionsPanel == null)
                {
                    _calculationOptionsPanel = new CalculationOptionsPanelMage();
                }
                return _calculationOptionsPanel;
            }
        }

        private List<Item.ItemType> _relevantItemTypes = null;
        public override List<Item.ItemType> RelevantItemTypes
        {
            get
            {
                if (_relevantItemTypes == null)
                {
                    _relevantItemTypes = new List<Item.ItemType>(new Item.ItemType[]
					{
						Item.ItemType.None,
						Item.ItemType.Cloth,
						Item.ItemType.Dagger,
						Item.ItemType.OneHandSword,
						Item.ItemType.Staff,
						Item.ItemType.Wand,
					});
                }
                return _relevantItemTypes;
            }
        }

        public override string GetCharacterStatsString(Character character)
        {
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Character:\t\t{0}@{1}-{2}\r\nRace:\t\t{3}",
				character.Name, character.Region, character.Realm, character.Race);

            CalculationOptionsMage CalculationOptions = (CalculationOptionsMage)character.CalculationOptions;
            CharacterCalculationsMage calculations;
            bool savedIncrementalOptimizations = CalculationOptions.IncrementalOptimizations;
            CalculationOptions.IncrementalOptimizations = false;
            calculations = Solver.GetCharacterCalculations(character, null, CalculationOptions, this, CalculationOptions.IncrementalSetArmor, CalculationOptions.DisplaySegmentCooldowns, CalculationOptions.DisplayIntegralMana);
            CalculationOptions.IncrementalOptimizations = savedIncrementalOptimizations;

            Dictionary<string, string> dict = calculations.GetCharacterDisplayCalculationValuesInternal();
			foreach (KeyValuePair<string, string> kvp in dict)
			{
                if (kvp.Key != "Sequence" && kvp.Key != "Spell Cycles")
                {
                    string[] value = kvp.Value.Split('*');
                    if (value.Length == 2)
                    {
                        sb.AppendFormat("\r\n{0}: {1}\r\n{2}\r\n", kvp.Key, value[0], value[1]);
                    }
                    else
                    {
                        sb.AppendFormat("\r\n{0}: {1}", kvp.Key, value[0]);
                    }
                }
			}

            // spell cycles
            sb.AppendFormat("\r\n\r\nSpell Cycles:\r\n\r\n");
            if (calculations.MageArmor != null) sb.AppendLine(calculations.MageArmor);
            Dictionary<string, double> combinedSolution = new Dictionary<string, double>();
            Dictionary<string, int> combinedSolutionData = new Dictionary<string, int>();
            double manaPotion = 0;
            double manaGem = 0;
            for (int i = 0; i < calculations.SolutionVariable.Count; i++)
            {
                if (calculations.Solution[i] > 0.01)
                {
                    switch (calculations.SolutionVariable[i].Type)
                    {
                        case VariableType.IdleRegen:
                            sb.AppendLine(String.Format("{0}: {1:F} sec", "Idle Regen", calculations.Solution[i]));
                            break;
                        case VariableType.Evocation:
                            sb.AppendLine(String.Format("{0}: {1:F}x", "Evocation", calculations.Solution[i] / calculations.EvocationDuration));
                            break;
                        case VariableType.ManaPotion:
                            manaPotion += calculations.Solution[i];
                            break;
                        case VariableType.ManaGem:
                            manaGem += calculations.Solution[i];
                            break;
                        case VariableType.DrumsOfBattle:
                            sb.AppendLine(String.Format("{0}: {1:F}x", "Drums of Battle", calculations.Solution[i] / calculations.BaseState.GlobalCooldown));
                            break;
                        case VariableType.Drinking:
                            sb.AppendLine(String.Format("{0}: {1:F} sec", "Drinking", calculations.Solution[i]));
                            break;
                        case VariableType.TimeExtension:
                            break;
                        case VariableType.AfterFightRegen:
                            sb.AppendLine(String.Format("{0}: {1:F} sec", "Drinking Regen", calculations.Solution[i]));
                            break;
                        case VariableType.Wand:
                        case VariableType.Spell:
                            double value;
                            Spell s = calculations.SolutionVariable[i].Spell;
                            string label = ((calculations.SolutionVariable[i].State.BuffLabel.Length > 0) ? (calculations.SolutionVariable[i].State.BuffLabel + "+") : "") + s.Name;
                            combinedSolution.TryGetValue(label, out value);
                            combinedSolution[label] = value + calculations.Solution[i];
                            combinedSolutionData[label] = i;
                            //stats.AppendLine(String.Format("{2}{0}: {1:F} sec", label, Solution[i], (SolutionSegments == null) ? "" : (SolutionSegments[i].ToString() + " ")));                            
                            break;
                    }
                }
            }
            if (manaPotion > 0)
            {
                sb.AppendLine(String.Format("{0}: {1:F}x", "Mana Potion", manaPotion));
            }
            if (manaGem > 0)
            {
                sb.AppendLine(String.Format("{0}: {1:F}x", "Mana Gem", manaGem));
            }
            foreach (KeyValuePair<string, double> kvp in combinedSolution)
            {
                Spell s = calculations.SolutionVariable[combinedSolutionData[kvp.Key]].Spell;
                if (s != null)
                {
                    sb.AppendLine(String.Format("{0}: {1:F} sec ({2:F} dps, {3:F} mps, {4:F} tps) {5}", kvp.Key, kvp.Value, s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond, s.ThreatPerSecond, s.Sequence));
                }
                else
                {
                    sb.AppendLine(String.Format("{0}: {1:F} sec", kvp.Key, kvp.Value));
                }
            }
            //if (calculations.WaterElemental) sb.AppendLine(String.Format("Water Elemental: {0:F}x", calculations.WaterElementalDuration / 45f));

            // sequence
            string sequence = dict["Sequence"];
            if (sequence != "*Disabled" && sequence != "*Unavailable")
            {
                string[] value = sequence.Split('*');
                sb.AppendFormat("\r\n\r\nSequence:\r\n\r\n");
                sb.Append(value[1]);
            }

			return sb.ToString();
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Mage; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationMage(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsMage(); }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem)
        {
            return GetCharacterCalculations(character, additionalItem, false);
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool computeIncrementalSet)
        {
            CharacterCalculationsBase ret;
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            bool savedIncrementalOptimizations = calculationOptions.IncrementalOptimizations;
            if (calculationOptions.IncrementalOptimizations && calculationOptions.IncrementalSetStateIndexes == null) computeIncrementalSet = true;
            if (computeIncrementalSet) calculationOptions.IncrementalOptimizations = false;
            if (calculationOptions.IncrementalOptimizations && !character.DisableBuffAutoActivation)
            {
                ret = GetCharacterCalculations(character, additionalItem, calculationOptions, calculationOptions.IncrementalSetArmor);
            }
            else if (calculationOptions.AutomaticArmor && !character.DisableBuffAutoActivation)
            {
                CharacterCalculationsBase mage = GetCharacterCalculations(character, additionalItem, calculationOptions, "Mage Armor");
                CharacterCalculationsBase molten = GetCharacterCalculations(character, additionalItem, calculationOptions, "Molten Armor");
                CharacterCalculationsBase calc = (mage.OverallPoints > molten.OverallPoints) ? mage : molten;
                if (calculationOptions.MeleeDps + calculationOptions.MeleeDot + calculationOptions.PhysicalDps + calculationOptions.PhysicalDot + calculationOptions.FrostDps + calculationOptions.FrostDot > 0)
                {
                    CharacterCalculationsBase ice = GetCharacterCalculations(character, additionalItem, calculationOptions, "Ice Armor");
                    if (ice.OverallPoints > calc.OverallPoints) calc = ice;
                }
                calculationOptions.IncrementalSetArmor = ((CharacterCalculationsMage)calc).MageArmor;
                if (computeIncrementalSet) StoreIncrementalSet(character, (CharacterCalculationsMage)calc);
                ret = calc;
            }
            else
            {
                CharacterCalculationsBase calc = GetCharacterCalculations(character, additionalItem, calculationOptions, null);
                if (!character.DisableBuffAutoActivation) calculationOptions.IncrementalSetArmor = ((CharacterCalculationsMage)calc).MageArmor;
                if (computeIncrementalSet) StoreIncrementalSet(character, (CharacterCalculationsMage)calc);
                ret = calc;
            }
            calculationOptions.IncrementalOptimizations = savedIncrementalOptimizations;
            return ret;
        }

        private void StoreIncrementalSet(Character character, CharacterCalculationsMage calculations)
        {
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            List<Cooldown> cooldownList = new List<Cooldown>();
            List<SpellId> spellList = new List<SpellId>();
            List<int> segmentList = new List<int>();
            for (int i = 0; i < calculations.SolutionVariable.Count; i++)
            {
                if (calculations.Solution[i] > 0 && calculations.SolutionVariable[i].Type == VariableType.Spell)
                {
                    cooldownList.Add(calculations.SolutionVariable[i].State.Cooldown & Cooldown.NonItemBasedMask);
                    spellList.Add(calculations.SolutionVariable[i].Spell.SpellId);
                    segmentList.Add(calculations.SolutionVariable[i].Segment);
                }
            }
            calculationOptions.IncrementalSetStateIndexes = cooldownList.ToArray();
            calculationOptions.IncrementalSetSpells = spellList.ToArray();
            calculationOptions.IncrementalSetSegments = segmentList.ToArray();
            calculationOptions.IncrementalSetArmor = calculations.MageArmor;

            List<Cooldown> filteredCooldowns = ListUtils.RemoveDuplicates(cooldownList);
            filteredCooldowns.Sort();
            calculationOptions.IncrementalSetSortedStates = filteredCooldowns.ToArray();
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, CalculationOptionsMage calculationOptions, string armor)
        {
            return Solver.GetCharacterCalculations(character, additionalItem, calculationOptions, this, armor, calculationOptions.ComparisonSegmentCooldowns, calculationOptions.ComparisonIntegralMana);
        }

        public Stats GetRawStats(Character character, Item additionalItem, CalculationOptionsMage calculationOptions, List<Buff> autoActivatedBuffs, string armor)
        {
            Stats stats = new Stats();
            AccumulateItemStats(stats, character, additionalItem);
            AccumulateEnchantsStats(stats, character);
            List<Buff> activeBuffs = new List<Buff>();
            activeBuffs.AddRange(character.ActiveBuffs);

            if (!character.DisableBuffAutoActivation)
            {
                Buff improvedScorch = Buff.GetBuffByName("Improved Scorch");
                Buff wintersChill = Buff.GetBuffByName("Winter's Chill");

                if (calculationOptions.MaintainScorch)
                {
                    if (character.MageTalents.ImprovedScorch > 0)
                    {
                        if (!character.ActiveBuffs.Contains(improvedScorch) && !character.ActiveBuffs.Contains(wintersChill))
                        {
                            activeBuffs.Add(improvedScorch);
                            autoActivatedBuffs.Add(improvedScorch);
                        }
                    }
                }
                if (character.MageTalents.WintersChill > 0)
                {
                    if (!character.ActiveBuffs.Contains(wintersChill) && !character.ActiveBuffs.Contains(improvedScorch))
                    {
                        activeBuffs.Add(wintersChill);
                        autoActivatedBuffs.Add(wintersChill);
                    }
                }
                if (armor != null)
                {
                    Buff armorBuff = Buff.GetBuffByName(armor);
                    if (!character.ActiveBuffs.Contains(armorBuff))
                    {
                        activeBuffs.Add(armorBuff);
                        autoActivatedBuffs.Add(armorBuff);
                        RemoveConflictingBuffs(activeBuffs, armorBuff);
                    }
                }
            }

            AccumulateBuffsStats(stats, activeBuffs);

            return stats;
        }

        // required by base class, but never used
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            return GetCharacterStats(character, additionalItem, GetRawStats(character, additionalItem, calculationOptions, new List<Buff>(), null), calculationOptions);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats rawStats, CalculationOptionsMage calculationOptions)
        {
            Stats statsRace;
            switch (calculationOptions.PlayerLevel)
            {
                case 70:
                    switch (character.Race)
                    {
                        case Character.CharacterRace.BloodElf:
                            statsRace = new Stats()
                            {
                                Health = 3213f,
                                Mana = 1961f,
                                Strength = 28f,
                                Agility = 42f,
                                Stamina = 49f,
                                Intellect = 149f,
                                Spirit = 144,
                            };
                            break;
                        case Character.CharacterRace.Draenei:
                            statsRace = new Stats()
                            {
                                Health = 3213f,
                                Mana = 1961f,
                                Strength = 28f,
                                Agility = 42f,
                                Stamina = 50f,
                                Intellect = 152f,
                                Spirit = 147,
                            };
                            break;
                        case Character.CharacterRace.Gnome:
                            statsRace = new Stats()
                            {
                                Health = 3213f,
                                Mana = 1961f,
                                Strength = 28f,
                                Agility = 42f,
                                Stamina = 50f,
                                Intellect = 154f,
                                Spirit = 145,
                            };
                            break;
                        case Character.CharacterRace.Human:
                            statsRace = new Stats()
                            {
                                Health = 3213f,
                                Mana = 1961f,
                                Strength = 28f,
                                Agility = 42f,
                                Stamina = 51f,
                                Intellect = 151f,
                                Spirit = 145,
                            };
                            break;
                        case Character.CharacterRace.Troll:
                            statsRace = new Stats()
                            {
                                Health = 3213f,
                                Mana = 1961f,
                                Strength = 28f,
                                Agility = 42f,
                                Stamina = 52f,
                                Intellect = 147f,
                                Spirit = 146,
                            };
                            break;
                        case Character.CharacterRace.Undead:
                            statsRace = new Stats()
                            {
                                Health = 3213f,
                                Mana = 1961f,
                                Strength = 28f,
                                Agility = 42f,
                                Stamina = 52f,
                                Intellect = 149f,
                                Spirit = 150,
                            };
                            break;
                        default:
                            statsRace = new Stats();
                            break;
                    }
                    break;
                case 71:
                    statsRace = new Stats()
                    {
                        Health = 3308f,
                        Mana = 2063f,
                        Strength = 28f,
                        Agility = 42f,
                        Stamina = 51f,
                        Intellect = 157f,
                        Spirit = 148f,
                    };
                    break;
                case 72:
                    statsRace = new Stats()
                    {
                        Health = 3406f,
                        Mana = 2166f,
                        Strength = 28f,
                        Agility = 43f,
                        Stamina = 52f,
                        Intellect = 160f,
                        Spirit = 151f,
                    };
                    break;
                case 73:
                    statsRace = new Stats()
                    {
                        Health = 3505f,
                        Mana = 2269f,
                        Strength = 28f,
                        Agility = 43f,
                        Stamina = 53f,
                        Intellect = 163f,
                        Spirit = 154f,
                    };
                    break;
                case 74:
                    statsRace = new Stats()
                    {
                        Health = 3623f,
                        Mana = 2372f,
                        Strength = 29f,
                        Agility = 44f,
                        Stamina = 53f,
                        Intellect = 166f,
                        Spirit = 156f,
                    };
                    break;
                case 75:
                    statsRace = new Stats()
                    {
                        Health = 3726f,
                        Mana = 2474f,
                        Strength = 29f,
                        Agility = 44f,
                        Stamina = 54f,
                        Intellect = 169f,
                        Spirit = 159f,
                    };
                    break;
                case 76:
                    statsRace = new Stats()
                    {
                        Health = 3830f,
                        Mana = 2577f,
                        Strength = 29f,
                        Agility = 44f,
                        Stamina = 55f,
                        Intellect = 172f,
                        Spirit = 162f,
                    };
                    break;
                case 77:
                    statsRace = new Stats()
                    {
                        Health = 3937f,
                        Mana = 2680f,
                        Strength = 30f,
                        Agility = 45f,
                        Stamina = 56f,
                        Intellect = 175f,
                        Spirit = 165f,
                    };
                    break;
                case 78:
                    statsRace = new Stats()
                    {
                        Health = 4063f,
                        Mana = 2783f,
                        Strength = 30f,
                        Agility = 45f,
                        Stamina = 56f,
                        Intellect = 178f,
                        Spirit = 168f,
                    };
                    break;
                case 79:
                    statsRace = new Stats()
                    {
                        Health = 4172f,
                        Mana = 2885f,
                        Strength = 30f,
                        Agility = 46f,
                        Stamina = 57f,
                        Intellect = 181f,
                        Spirit = 171f,
                    };
                    break;
                case 80:
                    statsRace = new Stats()
                    {
                        Health = 6783f,
                        Mana = 2988f,
                        Strength = 31f,
                        Agility = 46f,
                        Stamina = 58f,
                        Intellect = 184f,
                        Spirit = 174f,
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }
            if (character.Race == Character.CharacterRace.Gnome)
            {
                statsRace.BonusIntellectMultiplier = 1.05f * (1 + 0.03f * character.MageTalents.ArcaneMind) - 1.0f;
            }
            else
            {
                statsRace.BonusIntellectMultiplier = 0.03f * character.MageTalents.ArcaneMind;
            }
            if (character.Race == Character.CharacterRace.Human)
            {
                statsRace.BonusSpiritMultiplier = 0.03f;
            }
        
            Stats statsGearEnchantsBuffs = rawStats;

            if (character.MageTalents.StudentOfTheMind > 0)
            {
                statsGearEnchantsBuffs.BonusSpiritMultiplier = (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier) * (1.01f + 0.03f * character.MageTalents.StudentOfTheMind) - 1;
            }
            if (calculationOptions.EffectSpiritBonus > 0)
            {
                statsGearEnchantsBuffs.BonusSpiritMultiplier = (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier) * (1f + calculationOptions.EffectSpiritBonus / 100f) - 1;
            }
            Stats statsTotal = statsGearEnchantsBuffs + statsRace;
            statsTotal.Strength = (float)Math.Floor(0.00001 + Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) + (float)Math.Floor(0.00001 + statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor(0.00001 + Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier)) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) + (float)Math.Floor(0.00001 + statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Intellect = (float)Math.Floor(0.00001 + Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier)) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) + (float)Math.Floor(0.00001 + statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            statsTotal.Stamina = (float)Math.Floor(0.00001 + Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) + (float)Math.Floor(0.00001 + statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Spirit = (float)Math.Floor(0.00001 + Math.Floor(statsRace.Spirit * (1 + statsRace.BonusSpiritMultiplier)) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) + (float)Math.Floor(0.00001 + statsGearEnchantsBuffs.Spirit * (1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));

            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) - 1;

            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect + statsGearEnchantsBuffs.Mana);
            statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2f + 0.5f * statsTotal.Intellect * character.MageTalents.ArcaneFortitude);

            float magicAbsorption = 0.5f * calculationOptions.PlayerLevel * character.MageTalents.MagicAbsorption;
            int frostWarding = character.MageTalents.FrostWarding;
            statsTotal.AllResist += magicAbsorption;

            // TODO do similar for Mage Armor and Arcane Shielding
            if (statsTotal.MageIceArmor > 0)
            {
                statsTotal.Armor += (float)Math.Floor((calculationOptions.PlayerLevel < 79 ? 645 : 940) * (1 + 0.25f * frostWarding) * (1 + (calculationOptions.GlyphOfIceArmor ? 0.2f : 0.0f)));
                statsTotal.FrostResistance += (float)Math.Floor((calculationOptions.PlayerLevel < 79 ? 18 : 40) * (1 + 0.25f * frostWarding) * (1 + (calculationOptions.GlyphOfIceArmor ? 0.2f : 0.0f)));
            }
            if (statsTotal.MageMageArmor > 0)
            {
                statsTotal.SpellCombatManaRegeneration += 0.3f + (calculationOptions.GlyphOfMageArmor ? 0.2f : 0.0f);
                statsTotal.AllResist += (calculationOptions.PlayerLevel < 71 ? 18f : (calculationOptions.PlayerLevel < 79 ? 21f : 40f)) * (1 + character.MageTalents.ArcaneShielding * 0.25f);
            }
            if (statsTotal.MageMoltenArmor > 0)
            {
                statsTotal.SpellCrit += 0.03f + (calculationOptions.GlyphOfMoltenArmor ? 0.02f : 0.0f);
            }
            if (calculationOptions.GlyphOfManaGem)
            {
                statsTotal.BonusManaGem = (1 + statsTotal.BonusManaGem) * (1 + 0.1f) - 1;
            }

            statsTotal.SpellCombatManaRegeneration += 0.1f * character.MageTalents.ArcaneMeditation;
            if (calculationOptions.Mode308) statsTotal.SpellCombatManaRegeneration += 0.1f * character.MageTalents.Pyromaniac;

            //statsTotal.Mp5 += calculationOptions.ShadowPriest;

            statsTotal.SpellDamageFromIntellectPercentage += 0.03f * character.MageTalents.MindMastery;

            statsTotal.AllResist += statsTotal.MageAllResist;

            statsTotal.SpellPower += statsTotal.SpellDamageFromIntellectPercentage * statsTotal.Intellect;
            statsTotal.SpellPower += statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit;

            statsTotal.ArcaneResistance += statsTotal.AllResist + statsTotal.ArcaneResistanceBuff;
            statsTotal.FireResistance += statsTotal.AllResist + statsTotal.FireResistanceBuff;
            statsTotal.FrostResistance += statsTotal.AllResist + statsTotal.FrostResistanceBuff;
            statsTotal.NatureResistance += statsTotal.AllResist + statsTotal.NatureResistanceBuff;
            statsTotal.ShadowResistance += statsTotal.AllResist + statsTotal.ShadowResistanceBuff;

            int playerLevel = calculationOptions.PlayerLevel;
            float maxHitRate = 1.0f;
            float bossHitRate = Math.Min(maxHitRate, ((playerLevel <= calculationOptions.TargetLevel + 2) ? (0.96f - (playerLevel - calculationOptions.TargetLevel) * 0.01f) : (0.94f - (playerLevel - calculationOptions.TargetLevel - 2) * 0.11f)));
            statsTotal.Mp5 -= 5 * calculationOptions.EffectShadowManaDrain * calculationOptions.EffectShadowManaDrainFrequency * bossHitRate * Math.Max(1 - statsTotal.ShadowResistance / calculationOptions.TargetLevel * 0.15f, 0.25f);

            float fullResistRate = calculationOptions.EffectArcaneOtherBinary * (1 - bossHitRate * Math.Max(1 - statsTotal.ArcaneResistance / calculationOptions.TargetLevel * 0.15f, 0.25f));
            fullResistRate += calculationOptions.EffectFireOtherBinary * (1 - bossHitRate * Math.Max(1 - statsTotal.FireResistance / calculationOptions.TargetLevel * 0.15f, 0.25f));
            fullResistRate += calculationOptions.EffectFrostOtherBinary * (1 - bossHitRate * Math.Max(1 - statsTotal.FrostResistance / calculationOptions.TargetLevel * 0.15f, 0.25f));
            fullResistRate += calculationOptions.EffectShadowOtherBinary * (1 - bossHitRate * Math.Max(1 - statsTotal.ShadowResistance / calculationOptions.TargetLevel * 0.15f, 0.25f));
            fullResistRate += calculationOptions.EffectNatureOtherBinary * (1 - bossHitRate * Math.Max(1 - statsTotal.NatureResistance / calculationOptions.TargetLevel * 0.15f, 0.25f));
            fullResistRate += calculationOptions.EffectHolyOtherBinary * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectArcaneOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectFireOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectFrostOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectShadowOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectNatureOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectHolyOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectShadowManaDrainFrequency * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectShadowSilenceFrequency * (1 - bossHitRate * Math.Max(1 - statsTotal.ShadowResistance / calculationOptions.TargetLevel * 0.15f, 0.25f));
            statsTotal.Mp5 += 5 * Math.Min(1f, fullResistRate) * 0.01f * character.MageTalents.MagicAbsorption * statsTotal.Mana;

            return statsTotal;
        }
         
        private static string[] GlyphList = { "GlyphOfFireball", "GlyphOfFrostbolt", "GlyphOfIceArmor", "GlyphOfImprovedScorch", "GlyphOfMageArmor", "GlyphOfManaGem", "GlyphOfMoltenArmor", "GlyphOfWaterElemental", "GlyphOfArcaneExplosion", "GlyphOfArcanePower", "GlyphOfFrostfire", "GlyphOfArcaneBlast" };
        private static string[] GlyphListFriendly = { "Glyph of Fireball", "Glyph of Frostbolt", "Glyph of Ice Armor", "Glyph of Improved Scorch", "Glyph of Mage Armor", "Glyph of Mana Gem", "Glyph of Molten Armor", "Glyph of Water Elemental", "Glyph of Arcane Explosion", "Glyph of Arcane Power", "Glyph of Frostfire", "Glyph of Arcane Blast" };

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsMage baseCalc, currentCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            switch (chartName)
            {
                case "Talents (per talent point)":
                    CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
                    MageTalents talents = character.MageTalents;

                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;
                    bool savedIncrementalOptimizations = calculationOptions.IncrementalOptimizations;
                    bool savedSmartOptimizations = calculationOptions.SmartOptimization;

                    calculationOptions.IncrementalOptimizations = false;
                    calculationOptions.SmartOptimization = false;

                    Type t = typeof(MageTalents);
                    foreach (System.Reflection.PropertyInfo info in t.GetProperties())
                    {
                        object[] attributes = info.GetCustomAttributes(typeof(TalentDataAttribute), false);
                        if (attributes.Length > 0)
                        {
                            TalentDataAttribute talentData = (TalentDataAttribute)attributes[0];

                            string talent = talentData.Name;
                            int maxPoints = talentData.MaxPoints;
                            int currentPoints = (int)info.GetValue(talents, null);

                            if (currentPoints > 0)
                            {
                                info.SetValue(talents, 0, null);
                                calc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                                comparison = CreateNewComparisonCalculation();
                                comparison.Name = string.Format("{0} ({1})", talent, currentPoints);
                                comparison.Equipped = true;
                                comparison.OverallPoints = (currentCalc.OverallPoints - calc.OverallPoints) / (float)currentPoints;
                                subPoints = new float[calc.SubPoints.Length];
                                for (int i = 0; i < calc.SubPoints.Length; i++)
                                {
                                    subPoints[i] = (currentCalc.SubPoints[i] - calc.SubPoints[i]) / (float)currentPoints;
                                }
                                comparison.SubPoints = subPoints;

                                comparisonList.Add(comparison);
                            }

                            if (currentPoints < maxPoints)
                            {
                                info.SetValue(talents, maxPoints, null);
                                calc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                                comparison = CreateNewComparisonCalculation();
                                comparison.Name = string.Format("{0} ({1})", talent, maxPoints);
                                comparison.Equipped = false;
                                comparison.OverallPoints = (calc.OverallPoints - currentCalc.OverallPoints) / (float)(maxPoints - currentPoints);
                                subPoints = new float[calc.SubPoints.Length];
                                for (int i = 0; i < calc.SubPoints.Length; i++)
                                {
                                    subPoints[i] = (calc.SubPoints[i] - currentCalc.SubPoints[i]) / (float)(maxPoints - currentPoints);
                                }
                                comparison.SubPoints = subPoints;

                                comparisonList.Add(comparison);
                            }

                            info.SetValue(talents, currentPoints, null);
                        }
                    }

                    calculationOptions.IncrementalOptimizations = savedIncrementalOptimizations;
                    calculationOptions.SmartOptimization = savedSmartOptimizations;

                    return comparisonList.ToArray();
                case "Glyphs":
                    calculationOptions = character.CalculationOptions as CalculationOptionsMage;

                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;
                    savedIncrementalOptimizations = calculationOptions.IncrementalOptimizations;
                    savedSmartOptimizations = calculationOptions.SmartOptimization;

                    calculationOptions.IncrementalOptimizations = false;
                    calculationOptions.SmartOptimization = true;

                    for (int index = 0; index < GlyphList.Length; index++)
                    {
                        string glyph = GlyphList[index];
                        bool glyphEnabled = calculationOptions.GetGlyphByName(glyph);

                        if (glyphEnabled)
                        {
                            calculationOptions.SetGlyphByName(glyph, false);
                            calc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = GlyphListFriendly[index];
                            comparison.Equipped = true;
                            comparison.OverallPoints = (currentCalc.OverallPoints - calc.OverallPoints);
                            subPoints = new float[calc.SubPoints.Length];
                            for (int i = 0; i < calc.SubPoints.Length; i++)
                            {
                                subPoints[i] = (currentCalc.SubPoints[i] - calc.SubPoints[i]);
                            }
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }
                        else
                        {
                            calculationOptions.SetGlyphByName(glyph, true);
                            calc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = GlyphListFriendly[index];
                            comparison.Equipped = false;
                            comparison.OverallPoints = (calc.OverallPoints - currentCalc.OverallPoints);
                            subPoints = new float[calc.SubPoints.Length];
                            for (int i = 0; i < calc.SubPoints.Length; i++)
                            {
                                subPoints[i] = (calc.SubPoints[i] - currentCalc.SubPoints[i]);
                            }
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }

                        calculationOptions.SetGlyphByName(glyph, glyphEnabled);
                    }

                    calculationOptions.IncrementalOptimizations = savedIncrementalOptimizations;
                    calculationOptions.SmartOptimization = savedSmartOptimizations;

                    return comparisonList.ToArray();
                case "Talent Specs":
                    /*currentCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;
                    comparison = CreateNewComparisonCalculation();
                    comparison.Name = "Current";
                    comparison.Equipped = true;
                    comparison.OverallPoints = currentCalc.OverallPoints;
                    subPoints = new float[currentCalc.SubPoints.Length];
                    for (int i = 0; i < currentCalc.SubPoints.Length; i++)
                    {
                        subPoints[i] = currentCalc.SubPoints[i];
                    }
                    comparison.SubPoints = subPoints;

                    comparisonList.Add(comparison);*/

                    Character charClone = character.Clone();
                    calculationOptions = charClone.CalculationOptions as CalculationOptionsMage;
                    calculationOptions = calculationOptions.Clone();
                    charClone.CalculationOptions = calculationOptions;
                    calculationOptions.IncrementalOptimizations = false;
                    calculationOptions.SmartOptimization = false;

                    System.Windows.Forms.Control talentPicker = (((System.Windows.Forms.TabControl)(_calculationOptionsPanel.Parent.Parent)).TabPages[1].Controls[0]);
                    //        public List<SavedTalentSpec> SpecsFor(Character.CharacterClass charClass)

                    foreach (object savedTalentSpec in (System.Collections.IEnumerable)(talentPicker.GetType().GetMethod("SpecsFor").Invoke(talentPicker, new object[] { Character.CharacterClass.Mage })))
                    {
                        charClone.MageTalents = (MageTalents)(savedTalentSpec.GetType().GetMethod("TalentSpec").Invoke(savedTalentSpec, new object[] { }));

                        calc = GetCharacterCalculations(charClone) as CharacterCalculationsMage;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = savedTalentSpec.ToString();
                        comparison.Equipped = (character.MageTalents.ToString().Equals(charClone.MageTalents.ToString()));
                        comparison.OverallPoints = calc.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = calc.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }

                    return comparisonList.ToArray();
                case "Item Budget":
                    Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { SpellPower = 11.7f } },
                        new Item() { Stats = new Stats() { Mp5 = 4 } },
                        new Item() { Stats = new Stats() { CritRating = 10 } },
                        new Item() { Stats = new Stats() { HasteRating = 10 } },
                        new Item() { Stats = new Stats() { HitRating = 10 } },
                    };
                    string[] statList = new string[] {
                        "11.7 Spell Power",
                        "4 Mana per 5 sec",
                        "10 Crit Rating",
                        "10 Haste Rating",
                        "10 Hit Rating",
                    };

                    baseCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                    for (int index = 0; index < statList.Length; index++)
                    {
                        calc = GetCharacterCalculations(character, itemList[index]) as CharacterCalculationsMage;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = statList[index];
                        comparison.Equipped = false;
                        comparison.OverallPoints = calc.OverallPoints - baseCalc.OverallPoints;
                        subPoints = new float[calc.SubPoints.Length];
                        for (int i = 0; i < calc.SubPoints.Length; i++)
                        {
                            subPoints[i] = calc.SubPoints[i] - baseCalc.SubPoints[i];
                        }
                        comparison.SubPoints = subPoints;

                        comparisonList.Add(comparison);
                    }

                    //Intellect
                    CharacterCalculationsMage calcAtAdd;
                    Stats statsAtAdd = baseCalc.BaseStats;
                    float baseInt = baseCalc.BaseStats.Intellect;
                    float intToAdd = 0f;
                    while (baseInt == statsAtAdd.Intellect && intToAdd < 2)
                    {
                        intToAdd += 0.01f;
                        statsAtAdd = GetCharacterStats(character, new Item() { Stats = new Stats() { Intellect = intToAdd } });
                    }
                    calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = intToAdd } }) as CharacterCalculationsMage;

                    Stats statsAtSubtract = baseCalc.BaseStats;
                    float intToSubtract = 0f;
                    while (baseInt == statsAtSubtract.Intellect && intToSubtract > -2)
                    {
                        intToSubtract -= 0.01f;
                        statsAtSubtract = GetCharacterStats(character, new Item() { Stats = new Stats() { Intellect = intToSubtract } });
                    }
                    intToSubtract += 0.01f;

                    comparison = CreateNewComparisonCalculation();
                    comparison.Name = "10 Intellect";
                    comparison.Equipped = false;
                    comparison.OverallPoints = 10 * (calcAtAdd.OverallPoints - baseCalc.OverallPoints) / (intToAdd - intToSubtract);
                    subPoints = new float[baseCalc.SubPoints.Length];
                    for (int i = 0; i < baseCalc.SubPoints.Length; i++)
                    {
                        subPoints[i] = 10 * (calcAtAdd.SubPoints[i] - baseCalc.SubPoints[i]) / (intToAdd - intToSubtract);
                    }
                    comparison.SubPoints = subPoints;

                    comparisonList.Add(comparison);

                    //Spirit
                    statsAtAdd = baseCalc.BaseStats;
                    float baseSpi = baseCalc.BaseStats.Spirit;
                    float spiToAdd = 0f;
                    while (baseSpi == statsAtAdd.Spirit && spiToAdd < 2)
                    {
                        spiToAdd += 0.01f;
                        statsAtAdd = GetCharacterStats(character, new Item() { Stats = new Stats() { Spirit = spiToAdd } });
                    }
                    calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = spiToAdd } }) as CharacterCalculationsMage;

                    statsAtSubtract = baseCalc.BaseStats;
                    float spiToSubtract = 0f;
                    while (baseSpi == statsAtSubtract.Spirit && spiToSubtract > -2)
                    {
                        spiToSubtract -= 0.01f;
                        statsAtSubtract = GetCharacterStats(character, new Item() { Stats = new Stats() { Spirit = spiToSubtract } });
                    }
                    spiToSubtract += 0.01f;

                    comparison = CreateNewComparisonCalculation();
                    comparison.Name = "10 Spirit";
                    comparison.Equipped = false;
                    comparison.OverallPoints = 10 * (calcAtAdd.OverallPoints - baseCalc.OverallPoints) / (spiToAdd - spiToSubtract);
                    subPoints = new float[baseCalc.SubPoints.Length];
                    for (int i = 0; i < baseCalc.SubPoints.Length; i++)
                    {
                        subPoints[i] = 10 * (calcAtAdd.SubPoints[i] - baseCalc.SubPoints[i]) / (spiToAdd - spiToSubtract);
                    }
                    comparison.SubPoints = subPoints;

                    comparisonList.Add(comparison);

                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        private static string TimeFormat(double time)
        {
            TimeSpan span = new TimeSpan((long)(Math.Round(time, 2) / 0.0000001));
            return string.Format("{0:0}:{1:00}", span.Minutes, span.Seconds, span.Milliseconds);
        }

        public override void RenderCustomChart(Character character, string chartName, System.Drawing.Graphics g, int width, int height)
        {
            height -= 2;
            switch (chartName)
            {
                case "Sequence Reconstruction":
                    CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;

                    if (calculationOptions.SequenceReconstruction == null)
                    {
                        Font fontLegend = new Font("Verdana", 10f, GraphicsUnit.Pixel);
                        g.DrawString("Sequence reconstruction data is not available.", fontLegend, Brushes.Black, 2, 2);
                    }
                    else
                    {
                        #region Legend
                        Rectangle rectSubPoint;
                        System.Drawing.Drawing2D.LinearGradientBrush brushSubPointFill;
                        System.Drawing.Drawing2D.ColorBlend blendSubPoint;

                        Font fontLegend = new Font("Verdana", 10f, GraphicsUnit.Pixel);
                        int legendY = 2;

                        Cooldown[] cooldowns = new Cooldown[] { Cooldown.ArcanePower, Cooldown.IcyVeins, Cooldown.MoltenFury, Cooldown.Heroism, Cooldown.PotionOfWildMagic, Cooldown.PotionOfSpeed, Cooldown.FlameCap, Cooldown.Trinket1, Cooldown.Trinket2, Cooldown.Combustion, Cooldown.WaterElemental, Cooldown.ManaGemEffect };
                        string[] cooldownNames = new string[] { "Arcane Power", "Icy Veins", "Molten Fury", "Heroism", "Potion of Wild Magic", "Potion of Speed", "Flame Cap", (character.Trinket1 != null) ? character.Trinket1.Name : "Trinket 1", (character.Trinket2 != null) ? character.Trinket2.Name : "Trinket 2", "Combustion", "Water Elemental", "Mana Gem Effect" };
                        Color[] cooldownColors = new Color[] { Color.Azure, Color.DarkBlue, Color.Crimson, Color.Olive, Color.Purple, Color.LemonChiffon, Color.Orange, Color.Aqua, Color.Blue, Color.OrangeRed, Color.DarkCyan, Color.DarkGreen };
                        Brush[] brushSubPoints = new Brush[cooldownColors.Length];
                        Color[] colorSubPointsA = new Color[cooldownColors.Length];
                        Color[] colorSubPointsB = new Color[cooldownColors.Length];
                        for (int i = 0; i < cooldownColors.Length; i++)
                        {
                            Color baseColor = cooldownColors[i];
                            brushSubPoints[i] = new SolidBrush(Color.FromArgb(baseColor.R / 2, baseColor.G / 2, baseColor.B / 2));
                            colorSubPointsA[i] = Color.FromArgb(baseColor.A / 2, baseColor.R / 2, baseColor.G / 2, baseColor.B / 2);
                            colorSubPointsB[i] = Color.FromArgb(baseColor.A / 2, baseColor);
                        }
                        StringFormat formatSubPoint = new StringFormat();
                        formatSubPoint.Alignment = StringAlignment.Center;
                        formatSubPoint.LineAlignment = StringAlignment.Center;

                        int maxWidth = 1;
                        for (int i = 0; i < cooldownNames.Length; i++)
                        {
                            string subPointName = cooldownNames[i];
                            int widthSubPoint = (int)Math.Ceiling(g.MeasureString(subPointName, fontLegend).Width + 2f);
                            if (widthSubPoint > maxWidth) maxWidth = widthSubPoint;
                        }
                        for (int i = 0; i < cooldownNames.Length; i++)
                        {
                            string cooldownName = cooldownNames[i];
                            rectSubPoint = new Rectangle(2, legendY, maxWidth, 16);
                            blendSubPoint = new System.Drawing.Drawing2D.ColorBlend(3);
                            blendSubPoint.Colors = new Color[] { colorSubPointsA[i], colorSubPointsB[i], colorSubPointsA[i] };
                            blendSubPoint.Positions = new float[] { 0f, 0.5f, 1f };
                            brushSubPointFill = new System.Drawing.Drawing2D.LinearGradientBrush(rectSubPoint, colorSubPointsA[i], colorSubPointsB[i], 67f);
                            brushSubPointFill.InterpolationColors = blendSubPoint;

                            g.FillRectangle(brushSubPointFill, rectSubPoint);
                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);

                            g.DrawString(cooldownName, fontLegend, brushSubPoints[i], rectSubPoint, formatSubPoint);
                            legendY += 16;
                        }

                        g.DrawLine(Pens.Aqua, new Point(maxWidth + 40, 10), new Point(maxWidth + 80, 10));
                        g.DrawString("Mana", fontLegend, Brushes.Black, new Point(maxWidth + 90, 2));
                        g.DrawLine(Pens.Red, new Point(maxWidth + 40, 26), new Point(maxWidth + 80, 26));
                        g.DrawString("Dps", fontLegend, Brushes.Black, new Point(maxWidth + 90, 18));
                        #endregion

                        #region Graph Ticks
                        float graphStart = 20f;
                        float graphWidth = width - 40f;
                        float graphTop = legendY;
                        float graphBottom = height - 48;
                        float graphHeight = graphBottom - graphTop - 40;
                        float maxScale = calculationOptions.FightDuration;
                        float graphEnd = graphStart + graphWidth;
                        float[] ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};
                        Pen black200 = new Pen(Color.FromArgb(200, 0, 0, 0));
                        Pen black150 = new Pen(Color.FromArgb(150, 0, 0, 0));
                        Pen black75 = new Pen(Color.FromArgb(75, 0, 0, 0));
                        Pen black50 = new Pen(Color.FromArgb(50, 0, 0, 0));
                        Pen black25 = new Pen(Color.FromArgb(25, 0, 0, 0));
                        StringFormat formatTick = new StringFormat();
                        formatTick.LineAlignment = StringAlignment.Far;
                        formatTick.Alignment = StringAlignment.Center;
                        Brush black200brush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
                        Brush black150brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
                        Brush black75brush = new SolidBrush(Color.FromArgb(75, 0, 0, 0));
                        Brush black50brush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
                        Brush black25brush = new SolidBrush(Color.FromArgb(25, 0, 0, 0));

                        g.DrawLine(black150, ticks[1], graphTop + 36, ticks[1], graphTop + 39);
                        g.DrawLine(black150, ticks[2], graphTop + 36, ticks[2], graphTop + 39);
                        g.DrawLine(black75, ticks[3], graphTop + 36, ticks[3], graphTop + 39);
                        g.DrawLine(black75, ticks[4], graphTop + 36, ticks[4], graphTop + 39);
                        g.DrawLine(black75, ticks[5], graphTop + 36, ticks[5], graphTop + 39);
                        g.DrawLine(black75, ticks[6], graphTop + 36, ticks[6], graphTop + 39);
                        g.DrawLine(black75, graphEnd, graphTop + 41, graphEnd, height - 4);
                        g.DrawLine(black75, ticks[0], graphTop + 41, ticks[0], height - 4);
                        g.DrawLine(black50, ticks[1], graphTop + 41, ticks[1], height - 4);
                        g.DrawLine(black50, ticks[2], graphTop + 41, ticks[2], height - 4);
                        g.DrawLine(black25, ticks[3], graphTop + 41, ticks[3], height - 4);
                        g.DrawLine(black25, ticks[4], graphTop + 41, ticks[4], height - 4);
                        g.DrawLine(black25, ticks[5], graphTop + 41, ticks[5], height - 4);
                        g.DrawLine(black25, ticks[6], graphTop + 41, ticks[6], height - 4);
                        g.DrawLine(black200, graphStart - 4, graphTop + 40, graphEnd + 4, graphTop + 40);
                        g.DrawLine(black200, graphStart, graphTop + 36, graphStart, height - 4);
                        g.DrawLine(black200, graphEnd, graphTop + 36, graphEnd, height - 4);
                        g.DrawLine(black200, graphStart - 4, graphBottom, graphEnd + 4, graphBottom);

                        g.DrawString(TimeFormat(0f), fontLegend, black200brush, graphStart, graphTop + 36, formatTick);
                        g.DrawString(TimeFormat(maxScale), fontLegend, black200brush, graphEnd, graphTop + 36, formatTick);
                        g.DrawString(TimeFormat(maxScale * 0.5f), fontLegend, black200brush, ticks[0], graphTop + 36, formatTick);
                        g.DrawString(TimeFormat(maxScale * 0.75f), fontLegend, black150brush, ticks[1], graphTop + 36, formatTick);
                        g.DrawString(TimeFormat(maxScale * 0.25f), fontLegend, black150brush, ticks[2], graphTop + 36, formatTick);
                        g.DrawString(TimeFormat(maxScale * 0.125f), fontLegend, black75brush, ticks[3], graphTop + 36, formatTick);
                        g.DrawString(TimeFormat(maxScale * 0.375f), fontLegend, black75brush, ticks[4], graphTop + 36, formatTick);
                        g.DrawString(TimeFormat(maxScale * 0.625f), fontLegend, black75brush, ticks[5], graphTop + 36, formatTick);
                        g.DrawString(TimeFormat(maxScale * 0.875f), fontLegend, black75brush, ticks[6], graphTop + 36, formatTick);
                        #endregion

                        List<SequenceReconstruction.SequenceItem> sequence = calculationOptions.SequenceReconstruction.sequence;
                        CharacterCalculationsMage calculations = calculationOptions.Calculations;

                        float mana = calculations.StartingMana;
                        int gemCount = 0;
                        float time = 0;
                        Color manaFill = Color.FromArgb(50, Color.Blue);
                        float lastMana = mana;
                        float maxMana = calculations.BaseStats.Mana;
                        float maxDps = 0;
                        for (int i = 0; i < sequence.Count; i++)
                        {
                            int index = sequence[i].Index;
                            VariableType type = sequence[i].VariableType;
                            float duration = (float)sequence[i].Duration;
                            Spell spell = sequence[i].Spell;
                            if (spell != null && spell.DamagePerSecond > maxDps) maxDps = spell.DamagePerSecond;
                            CastingState state = sequence[i].CastingState;
                            float mps = (float)sequence[i].Mps;
                            if (sequence[i].IsManaPotionOrGem)
                            {
                                duration = 0;
                                if (sequence[i].VariableType == VariableType.ManaGem)
                                {
                                    mana += (float)((1 + calculations.BaseStats.BonusManaGem) * calculations.ManaGemValue);
                                    gemCount++;
                                }
                                else if (sequence[i].VariableType == VariableType.ManaPotion)
                                {
                                    mana += (float)((1 + calculations.BaseStats.BonusManaPotion) * calculations.ManaPotionValue);
                                }
                                if (mana < 0) mana = 0;
                                if (mana > maxMana)
                                {
                                    mana = maxMana;
                                }
                                g.DrawLine(Pens.Aqua, graphStart + time / maxScale * graphWidth, graphBottom - graphHeight * lastMana / maxMana, graphStart + time / maxScale * graphWidth, height - 44 - graphHeight * mana / maxMana);
                            }
                            else
                            {
                                if (sequence[i].VariableType == VariableType.Evocation)
                                {
                                    mps = -(float)calculationOptions.Calculations.EvocationRegen;
                                }
                                float partTime = duration;
                                if (mana - mps * duration < 0) partTime = mana / mps;
                                else if (mana - mps * duration > maxMana) partTime = (mana - maxMana) / mps;
                                mana -= mps * duration;
                                if (mana < 0) mana = 0;
                                if (mana > maxMana)
                                {
                                    mana = maxMana;
                                }
                                float x1 = graphStart + time / maxScale * graphWidth;
                                float x2 = graphStart + (time + partTime) / maxScale * graphWidth;
                                float x3 = graphStart + (time + duration) / maxScale * graphWidth;
                                float y1 = graphBottom - graphHeight * lastMana / maxMana;
                                float y2 = graphBottom - graphHeight * mana / maxMana;
                                float y3 = graphBottom;
                                g.FillPolygon(new SolidBrush(manaFill), new PointF[] { new PointF(x1, y1), new PointF(x2, y2), new PointF(x3, y2), new PointF(x3, y3), new PointF(x1, y3) });
                                g.DrawLine(Pens.Aqua, x1, y1, x2, y2);
                                g.DrawLine(Pens.Aqua, x2, y2, x3, y2);
                            }
                            lastMana = mana;
                            time += duration;
                        }

                        maxDps *= 1.1f;
                        List<PointF> list = new List<PointF>();
                        time = 0.0f;
                        for (int i = 0; i < sequence.Count; i++)
                        {
                            int index = sequence[i].Index;
                            VariableType type = sequence[i].VariableType;
                            float duration = (float)sequence[i].Duration;
                            Spell spell = sequence[i].Spell;
                            CastingState state = sequence[i].CastingState;
                            float mps = (float)sequence[i].Mps;
                            if (sequence[i].IsManaPotionOrGem) duration = 0;
                            float dps = 0;
                            if (spell != null)
                            {
                                dps = spell.DamagePerSecond;
                            }
                            if (duration > 0)
                            {
                                list.Add(new PointF(graphStart + (time + 0.1f * duration) / maxScale * graphWidth, graphBottom - graphHeight * dps / maxDps));
                                list.Add(new PointF(graphStart + (time + 0.9f * duration) / maxScale * graphWidth, graphBottom - graphHeight * dps / maxDps));
                            }
                            time += duration;
                        }
                        if (list.Count > 0) g.DrawLines(Pens.Red, list.ToArray());

                        for (int cooldown = 0; cooldown < cooldownNames.Length; cooldown++)
                        {
                            blendSubPoint = new System.Drawing.Drawing2D.ColorBlend(3);
                            blendSubPoint.Colors = new Color[] { colorSubPointsA[cooldown], colorSubPointsB[cooldown], colorSubPointsA[cooldown] };
                            blendSubPoint.Positions = new float[] { 0f, 0.5f, 1f };
                            bool on = false;
                            float timeOn = 0.0f;
                            time = 0;
                            for (int i = 0; i < sequence.Count; i++)
                            {
                                float duration = (float)sequence[i].Duration;
                                if (sequence[i].IsManaPotionOrGem) duration = 0;
                                if (on && !sequence[i].CastingState.GetCooldown(cooldowns[cooldown]) && !sequence[i].IsManaPotionOrGem)
                                {
                                    on = false;
                                    if (time > timeOn)
                                    {
                                        RectangleF rect = new RectangleF(graphStart + graphWidth * timeOn / maxScale, graphBottom + cooldown * 4, graphWidth * (time - timeOn) / maxScale, 4);
                                        brushSubPointFill = new System.Drawing.Drawing2D.LinearGradientBrush(rect, colorSubPointsA[cooldown], colorSubPointsB[cooldown], 67f);
                                        brushSubPointFill.InterpolationColors = blendSubPoint;

                                        g.FillRectangle(brushSubPointFill, rect);
                                        g.DrawRectangle(new Pen(brushSubPointFill), rect.X, rect.Y, rect.Width, rect.Height);
                                        g.DrawRectangle(new Pen(brushSubPointFill), rect.X, rect.Y, rect.Width, rect.Height);
                                        g.DrawRectangle(new Pen(brushSubPointFill), rect.X, rect.Y, rect.Width, rect.Height);
                                    }
                                }
                                else if (!on && sequence[i].CastingState.GetCooldown(cooldowns[cooldown]))
                                {
                                    on = true;
                                    timeOn = time;
                                }
                                time += duration;
                            }
                            if (on)
                            {
                                RectangleF rect = new RectangleF(graphStart + graphWidth * timeOn / maxScale, graphBottom + cooldown * 4, graphWidth * (time - timeOn) / maxScale, 4);
                                brushSubPointFill = new System.Drawing.Drawing2D.LinearGradientBrush(rect, colorSubPointsA[cooldown], colorSubPointsB[cooldown], 67f);
                                brushSubPointFill.InterpolationColors = blendSubPoint;

                                g.FillRectangle(brushSubPointFill, rect);
                                g.DrawRectangle(new Pen(brushSubPointFill), rect.X, rect.Y, rect.Width, rect.Height);
                                g.DrawRectangle(new Pen(brushSubPointFill), rect.X, rect.Y, rect.Width, rect.Height);
                                g.DrawRectangle(new Pen(brushSubPointFill), rect.X, rect.Y, rect.Width, rect.Height);
                            }
                        }
                    }
                    break;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels
        {
            get
            {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
					"Health",
                    "Nature Resistance",
                    "Fire Resistance",
                    "Frost Resistance",
                    "Shadow Resistance",
                    "Arcane Resistance",
                    "Chance to Live",
                    "Hit Rating",
                    "Haste Rating",
                    "PVP Trinket",
                    "Movement Speed",
					};
                return _optimizableCalculationLabels;
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            return new Stats()
            {
				AllResist = stats.AllResist,
				MageAllResist = stats.MageAllResist,
				ArcaneResistance = stats.ArcaneResistance,
                FireResistance = stats.FireResistance,
                FrostResistance = stats.FrostResistance,
                NatureResistance = stats.NatureResistance,
                ShadowResistance = stats.ShadowResistance,
                Stamina = stats.Stamina,
                Intellect = stats.Intellect,
                Spirit = stats.Spirit,
                DefenseRating = stats.DefenseRating,
                DodgeRating = stats.DodgeRating,
                Health = stats.Health,
                Mp5 = stats.Mp5,
                Resilience = stats.Resilience,
                CritRating = stats.CritRating,
                SpellPower = stats.SpellPower,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                HasteRating = stats.HasteRating,
                HitRating = stats.HitRating,
                BonusIntellectMultiplier = stats.BonusIntellectMultiplier,
                BonusSpellCritMultiplier = stats.BonusSpellCritMultiplier,
                BonusSpellPowerMultiplier = stats.BonusSpellPowerMultiplier,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusSpiritMultiplier = stats.BonusSpiritMultiplier,
                Mana = stats.Mana,
                SpellArcaneDamageRating = stats.SpellArcaneDamageRating,
                SpellPenetration = stats.SpellPenetration,
                SpellFrostDamageRating = stats.SpellFrostDamageRating,
                Armor = stats.Armor,
                Hp5 = stats.Hp5,
                SpellCombatManaRegeneration = stats.SpellCombatManaRegeneration,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                ArcaneBlastBonus = stats.ArcaneBlastBonus,
                SpellPowerFor6SecOnCrit = stats.SpellPowerFor6SecOnCrit,
                EvocationExtension = stats.EvocationExtension,
                BonusMageNukeMultiplier = stats.BonusMageNukeMultiplier,
                LightningCapacitorProc = stats.LightningCapacitorProc,
                SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaRestorePerCast = stats.ManaRestorePerCast,
                ManaRestoreFromMaxManaPerHit = stats.ManaRestoreFromMaxManaPerHit,
                BonusManaGem = stats.BonusManaGem,
                SpellPowerFor15SecOnManaGem = stats.SpellPowerFor15SecOnManaGem,
                SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                SpellDamageFromIntellectPercentage = stats.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                SpellPowerFor10SecOnResist = stats.SpellPowerFor10SecOnResist,
                SpellPowerFor15SecOnCrit_20_45 = stats.SpellPowerFor15SecOnCrit_20_45,
                SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                SpellHasteFor5SecOnCrit_50 = stats.SpellHasteFor5SecOnCrit_50,
                SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                SpellDamageFor10SecOnHit_5 = stats.SpellDamageFor10SecOnHit_5,
                SpellHasteFor6SecOnHit_10_45 = stats.SpellHasteFor6SecOnHit_10_45,
                SpellPowerFor10SecOnCrit_20_45 = stats.SpellPowerFor10SecOnCrit_20_45,
                BonusManaPotion = stats.BonusManaPotion,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                HasteRatingFor20SecOnUse5Min = stats.HasteRatingFor20SecOnUse5Min,
                AldorRegaliaInterruptProtection = stats.AldorRegaliaInterruptProtection,
                SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                ShatteredSunAcumenProc = stats.ShatteredSunAcumenProc,
                ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
                InterruptProtection = stats.InterruptProtection,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                PVPTrinket = stats.PVPTrinket,
                MovementSpeed = stats.MovementSpeed,
                MageIceArmor = stats.MageIceArmor,
                MageMageArmor = stats.MageMageArmor,
                MageMoltenArmor = stats.MageMoltenArmor,
                ManaRestoreFromMaxManaPerSecond = stats.ManaRestoreFromMaxManaPerSecond,
                SpellHit = stats.SpellHit,
                SpellCrit = stats.SpellCrit,
                SpellHaste = stats.SpellHaste,
                SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                ManaRestoreOnCast_10_45 = stats.ManaRestoreOnCast_10_45,
                SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                ManaRestorePerCrit = stats.ManaRestorePerCrit,
                PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                ThunderCapacitorProc = stats.ThunderCapacitorProc,
                SpellPowerFor20SecOnUse5Min = stats.SpellPowerFor20SecOnUse5Min,
                CritBonusDamage = stats.CritBonusDamage,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            float mageStats = stats.Intellect + stats.Spirit + stats.Mp5 + stats.CritRating + stats.SpellPower + stats.SpellFireDamageRating + stats.HasteRating + stats.HitRating + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusSpiritMultiplier + stats.SpellFrostDamageRating + stats.SpellArcaneDamageRating + stats.SpellPenetration + stats.Mana + stats.SpellCombatManaRegeneration + stats.BonusArcaneDamageMultiplier + stats.BonusFireDamageMultiplier + stats.BonusFrostDamageMultiplier + stats.ArcaneBlastBonus + stats.SpellPowerFor6SecOnCrit + stats.EvocationExtension + stats.BonusMageNukeMultiplier + stats.LightningCapacitorProc + stats.SpellPowerFor20SecOnUse2Min + stats.HasteRatingFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min + stats.ManaRestoreFromMaxManaPerHit + stats.ManaRestorePerCast + stats.SpellPowerFor15SecOnManaGem + stats.BonusManaGem + stats.SpellPowerFor10SecOnHit_10_45 + stats.SpellDamageFromIntellectPercentage + stats.SpellDamageFromSpiritPercentage + stats.SpellPowerFor10SecOnResist + stats.SpellPowerFor15SecOnCrit_20_45 + stats.SpellPowerFor15SecOnUse90Sec + stats.SpellHasteFor5SecOnCrit_50 + stats.SpellHasteFor6SecOnCast_15_45 + stats.SpellDamageFor10SecOnHit_5 + stats.SpellHasteFor6SecOnHit_10_45 + stats.SpellPowerFor10SecOnCrit_20_45 + stats.BonusManaPotion + stats.ThreatReductionMultiplier + stats.AllResist + stats.MageAllResist + stats.ArcaneResistance + stats.FireResistance + stats.FrostResistance + stats.NatureResistance + stats.ShadowResistance + stats.HasteRatingFor20SecOnUse5Min + stats.AldorRegaliaInterruptProtection + stats.SpellPowerFor15SecOnUse2Min + stats.ShatteredSunAcumenProc + stats.ManaRestoreOnCast_5_15 + stats.InterruptProtection + stats.ArcaneResistanceBuff + stats.FrostResistanceBuff + stats.FireResistanceBuff + stats.NatureResistanceBuff + stats.ShadowResistanceBuff + stats.PVPTrinket + stats.MovementSpeed + stats.Resilience + stats.MageIceArmor + stats.MageMageArmor + stats.MageMoltenArmor + stats.ManaRestoreFromMaxManaPerSecond + stats.SpellCrit + stats.SpellHit + stats.SpellHaste + stats.SpellPowerFor10SecOnCast_15_45 + stats.ManaRestoreOnCast_10_45 + stats.SpellHasteFor10SecOnCast_10_45 + stats.ManaRestorePerCrit + stats.PendulumOfTelluricCurrentsProc + stats.ThunderCapacitorProc + stats.SpellPowerFor20SecOnUse5Min + stats.CritBonusDamage;
            float ignoreStats = stats.Agility + stats.Strength + stats.AttackPower + + stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry + stats.DodgeRating + stats.ParryRating + stats.ExpertiseRating + stats.Expertise + stats.Block + stats.BlockRating + stats.BlockValue + stats.SpellShadowDamageRating + stats.SpellNatureDamageRating;
            return (mageStats > 0 || ((stats.Health + stats.Stamina + stats.Armor) > 0 && ignoreStats == 0.0f));
        }
    }
}
