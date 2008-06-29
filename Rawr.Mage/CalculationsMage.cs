using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Rawr.Mage
{
	[System.ComponentModel.DisplayName("Mage|Spell_Holy_MagicalSentry")]
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
                    "Spell Stats:Spell Crit Rate",
                    "Spell Stats:Spell Hit Rate",
                    "Spell Stats:Spell Penetration",
                    "Spell Stats:Casting Speed",
                    "Spell Stats:Arcane Damage",
                    "Spell Stats:Fire Damage",
                    "Spell Stats:Frost Damage",
                    "Solution:Total Damage",
                    "Solution:Dps",
                    "Solution:Tps*Threat per second",
                    "Solution:Spell Cycles",
                    "Solution:Sequence*Cycle sequence reconstruction based on optimum cycles",
                    "Spell Info:Wand",
                    "Spell Info:Arcane Missiles",
                    //"Spell Info:AM?*Arcane Missiles with Netherwind proc",
                    "Spell Info:Arcane Blast*Spammed",
                    "Spell Info:Arcane Blast(0)*Non-debuffed",
                    //"Spell Info:Arcane Barrage*Requires talent points",
                    "Spell Info:Scorch",
                    "Spell Info:Fire Blast",
                    "Spell Info:Pyroblast*Requires talent points",
                    "Spell Info:Fireball",
                    "Spell Info:FireballScorch*Must enable Maintain Scorch and have points in Improved Scorch talent to enable",
                    "Spell Info:FireballFireBlast",
                    "Spell Info:Frostbolt",
                    "Spell Info:ABAMP*Pause so that AB debuff runs out mid-cast",
                    "Spell Info:ABAM*Spam with no pause",
                    "Spell Info:ABAMCC*AM when AM procs clearcast, ramp up AB before returning to AB-AM",
                    "Spell Info:ABAM3CC*AM when AM procs clearcast, ramp up with AB-AM",
                    "Spell Info:AB3AMSc*Prefer pause over longer filler",
                    "Spell Info:ABAM3Sc*Prefer pause over longer filler",
                    "Spell Info:ABAM3Sc2*Fill until debuff almost out",
                    "Spell Info:ABAM3FrB*Prefer pause over longer filler",
                    "Spell Info:ABAM3FrB2*Fill until debuff almost out",
                    "Spell Info:ABAM3ScCC*AM when AM procs clearcast",
                    "Spell Info:ABAM3Sc2CC*AM when AM procs clearcast",
                    "Spell Info:ABAM3FrBCC*AM when AM procs clearcast",
                    //"Spell Info:ABAM3FrBCCFail*AM when AM procs clearcast",
                    "Spell Info:ABAM3FrBScCC*AM when AM procs clearcast",
                    "Spell Info:ABFrB*Prefer pause over longer filler",
                    "Spell Info:ABFrB3FrB*Prefer pause over longer filler",
                    "Spell Info:ABFrB3FrBSc*Fill until debuff almost out, Scorch used at specific haste levels where adding another Frostbolt results in drop of AB debuff and alternative results in relatively large pause",
                    //"Spell Info:AB3Sc*Fill until debuff almost out",
                    "Spell Info:ABFB3FBSc*Typically FB-FB-Sc filler",
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
                    _customChartNames = new string[] { "Talents (per talent point)", "Talent Specs", "Item Budget" };
                return _customChartNames;
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
			StringBuilder stats = new StringBuilder();
			stats.AppendFormat("Character:\t\t{0}@{1}-{2}\r\nRace:\t\t{3}",
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
                        stats.AppendFormat("\r\n{0}: {1}\r\n{2}\r\n", kvp.Key, value[0], value[1]);
                    }
                    else
                    {
                        stats.AppendFormat("\r\n{0}: {1}", kvp.Key, value[0]);
                    }
                }
			}

            // spell cycles
            stats.AppendFormat("\r\n\r\nSpell Cycles:\r\n\r\n");
            if (calculations.MageArmor != null) stats.AppendLine(calculations.MageArmor);
            Dictionary<string, double> combinedSolution = new Dictionary<string, double>();
            Dictionary<string, int> combinedSolutionData = new Dictionary<string, int>();
            for (int i = 0; i < calculations.SolutionVariable.Count; i++)
            {
                if (calculations.Solution[i] > 0.01)
                {
                    if (i == calculations.ColumnIdleRegen)
                    {
                        stats.AppendLine(String.Format("{0}: {1:F} sec", "Idle Regen", calculations.Solution[0]));
                    }
                    else if (i == calculations.ColumnEvocation)
                    {
                        stats.AppendLine(String.Format("{0}: {1:F}x", "Evocation", calculations.Solution[i] / calculations.EvocationDuration));
                    }
                    else if (i == calculations.ColumnManaPotion)
                    {
                        stats.AppendLine(String.Format("{0}: {1:F}x", "Mana Potion", calculations.Solution[i]));
                    }
                    else if (i == calculations.ColumnManaGem)
                    {
                        stats.AppendLine(String.Format("{0}: {1:F}x", "Mana Gem", calculations.Solution[i]));
                    }
                    else if (i == calculations.ColumnDrumsOfBattle)
                    {
                        stats.AppendLine(String.Format("{0}: {1:F}x", "Drums of Battle", calculations.Solution[i] / calculations.BaseState.GlobalCooldown));
                    }
                    else if (i == calculations.ColumnDrinking)
                    {
                        stats.AppendLine(String.Format("{0}: {1:F} sec", "Drinking", calculations.Solution[i]));
                    }
                    else if (i == calculations.ColumnTimeExtension)
                    {
                    }
                    else if (i == calculations.ColumnAfterFightRegen)
                    {
                        stats.AppendLine(String.Format("{0}: {1:F} sec", "Drinking Regen", calculations.Solution[i]));
                    }
                    else
                    {
                        double value;
                        Spell s = calculations.SolutionVariable[i].Spell;
                        string label = ((calculations.SolutionVariable[i].State.BuffLabel.Length > 0) ? (calculations.SolutionVariable[i].State.BuffLabel + "+") : "") + s.Name;
                        combinedSolution.TryGetValue(label, out value);
                        combinedSolution[label] = value + calculations.Solution[i];
                        combinedSolutionData[label] = i;
                        //stats.AppendLine(String.Format("{2}{0}: {1:F} sec", label, Solution[i], (SolutionSegments == null) ? "" : (SolutionSegments[i].ToString() + " ")));                            
                    }
                }
            }
            foreach (KeyValuePair<string, double> kvp in combinedSolution)
            {
                Spell s = calculations.SolutionVariable[combinedSolutionData[kvp.Key]].Spell;
                if (s != null)
                {
                    stats.AppendLine(String.Format("{0}: {1:F} sec ({2:F} dps, {3:F} mps, {4:F} tps) {5}", kvp.Key, kvp.Value, s.DamagePerSecond, s.CostPerSecond - s.ManaRegenPerSecond, s.ThreatPerSecond, s.Sequence));
                }
                else
                {
                    stats.AppendLine(String.Format("{0}: {1:F} sec", kvp.Key, kvp.Value));
                }
            }
            if (calculations.WaterElemental) stats.AppendLine(String.Format("Water Elemental: {0:F}x", calculations.WaterElementalDuration / 45f));

            // sequence
            string sequence = dict["Sequence"];
            if (sequence != "*Disabled" && sequence != "*Unavailable")
            {
                string[] value = sequence.Split('*');
                stats.AppendFormat("\r\n\r\nSequence:\r\n\r\n");
                stats.Append(value[1]);
            }

			return stats.ToString();
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
            List<int> cooldownList = new List<int>();
            List<SpellId> spellList = new List<SpellId>();
            List<int> segmentList = new List<int>();
            for (int i = 0; i < calculations.SolutionVariable.Count; i++)
            {
                if (calculations.Solution[i] > 0 && calculations.SolutionVariable[i].Spell != null)
                {
                    cooldownList.Add(calculations.SolutionVariable[i].State.IncrementalSetIndex);
                    spellList.Add(calculations.SolutionVariable[i].Spell.SpellId);
                    segmentList.Add(calculations.SolutionVariable[i].Segment);
                }
            }
            calculationOptions.IncrementalSetStateIndexes = cooldownList.ToArray();
            calculationOptions.IncrementalSetSpells = spellList.ToArray();
            calculationOptions.IncrementalSetSegments = segmentList.ToArray();
            calculationOptions.IncrementalSetArmor = calculations.MageArmor;

            List<int> filteredCooldowns = ListUtils.RemoveDuplicates(cooldownList);
            filteredCooldowns.Sort();
            calculationOptions.IncrementalSetSortedCooldowns = filteredCooldowns.ToArray();
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
                if (calculationOptions.MaintainScorch)
                {
                    if (calculationOptions.ImprovedScorch > 0)
                    {
                        Buff improvedScorch = Buff.GetBuffByName("Improved Scorch");
                        if (!character.ActiveBuffs.Contains(improvedScorch))
                        {
                            activeBuffs.Add(improvedScorch);
                            autoActivatedBuffs.Add(improvedScorch);
                        }
                    }
                }
                if (calculationOptions.WintersChill > 0)
                {
                    Buff wintersChill = Buff.GetBuffByName("Winter's Chill");
                    if (!character.ActiveBuffs.Contains(wintersChill))
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
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
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
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
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
                        ArcaneResistance = 10,
                        BonusIntellectMultiplier = 1.05f * (1 + 0.03f * calculationOptions.ArcaneMind) - 1
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
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
                        BonusSpiritMultiplier = 0.1f
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
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
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
                        BonusIntellectMultiplier = 0.03f * calculationOptions.ArcaneMind,
                    };
                    break;
                default:
                    statsRace = new Stats();
                    break;
            }

            Stats statsGearEnchantsBuffs = rawStats;

            if (calculationOptions.WotLK && calculationOptions.StudentOfTheMind > 0)
            {
                statsGearEnchantsBuffs.BonusSpiritMultiplier = (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier) * (1.01f + 0.03f * calculationOptions.StudentOfTheMind) - 1;
            }
            if (calculationOptions.EffectSpiritBonus > 0)
            {
                statsGearEnchantsBuffs.BonusSpiritMultiplier = (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier) * (1f + calculationOptions.EffectSpiritBonus / 100f) - 1;
            }
            Stats statsTotal = statsGearEnchantsBuffs + statsRace;
            statsTotal.Strength = (float)Math.Floor((Math.Floor(statsRace.Strength * (1 + statsRace.BonusStrengthMultiplier)) + statsGearEnchantsBuffs.Strength * (1 + statsRace.BonusStrengthMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor((Math.Floor(statsRace.Agility * (1 + statsRace.BonusAgilityMultiplier)) + statsGearEnchantsBuffs.Agility * (1 + statsRace.BonusAgilityMultiplier)) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier));
            statsTotal.Intellect = (float)Math.Floor((Math.Floor(statsRace.Intellect * (1 + statsRace.BonusIntellectMultiplier)) + statsGearEnchantsBuffs.Intellect * (1 + statsRace.BonusIntellectMultiplier)) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier));
            statsTotal.Stamina = (float)Math.Floor((Math.Floor(statsRace.Stamina * (1 + statsRace.BonusStaminaMultiplier)) + statsGearEnchantsBuffs.Stamina * (1 + statsRace.BonusStaminaMultiplier)) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier));
            statsTotal.Spirit = (float)Math.Floor((Math.Floor(statsRace.Spirit * (1 + statsRace.BonusSpiritMultiplier)) + statsGearEnchantsBuffs.Spirit * (1 + statsRace.BonusSpiritMultiplier)) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier));

            statsTotal.BonusAgilityMultiplier = ((1 + statsRace.BonusAgilityMultiplier) * (1 + statsGearEnchantsBuffs.BonusAgilityMultiplier)) - 1;
            statsTotal.BonusStrengthMultiplier = ((1 + statsRace.BonusStrengthMultiplier) * (1 + statsGearEnchantsBuffs.BonusStrengthMultiplier)) - 1;
            statsTotal.BonusStaminaMultiplier = ((1 + statsRace.BonusStaminaMultiplier) * (1 + statsGearEnchantsBuffs.BonusStaminaMultiplier)) - 1;
            statsTotal.BonusIntellectMultiplier = ((1 + statsRace.BonusIntellectMultiplier) * (1 + statsGearEnchantsBuffs.BonusIntellectMultiplier)) - 1;
            statsTotal.BonusSpiritMultiplier = ((1 + statsRace.BonusSpiritMultiplier) * (1 + statsGearEnchantsBuffs.BonusSpiritMultiplier)) - 1;

            statsTotal.Health = (float)Math.Round(((statsRace.Health + statsGearEnchantsBuffs.Health + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Mana = (float)Math.Round(statsRace.Mana + 15f * statsTotal.Intellect + statsGearEnchantsBuffs.Mana);
            statsTotal.Armor = (float)Math.Round(statsGearEnchantsBuffs.Armor + statsTotal.Agility * 2f + statsTotal.Intellect * calculationOptions.ArcaneFortitude);

            float magicAbsorption = (calculationOptions.WotLK ? 0.2f * 70.0f : 2.0f) * calculationOptions.MagicAbsorption;
            int frostWarding = calculationOptions.FrostWarding;
            statsTotal.AllResist += magicAbsorption;

            if (statsTotal.MageIceArmor > 0)
            {
                statsTotal.Armor += (float)Math.Floor(645 * (1 + 0.15f * frostWarding));
                statsTotal.FrostResistance += (float)Math.Floor(18 * (1 + 0.15f * frostWarding));
            }

            statsTotal.SpellCombatManaRegeneration += 0.1f * calculationOptions.ArcaneMeditation;

            statsTotal.SpellPenetration += 5 * calculationOptions.ArcaneSubtlety;

            statsTotal.Mp5 += calculationOptions.ShadowPriest;

            statsTotal.SpellDamageFromIntellectPercentage += 0.05f * calculationOptions.MindMastery;

            statsTotal.AllResist += statsTotal.MageAllResist;

            if (calculationOptions.WotLK && calculationOptions.PotentSpirit > 0) statsTotal.SpellCritRating += (calculationOptions.PotentSpirit == 2 ? 0.15f : 0.07f) * statsTotal.Spirit;

            statsTotal.SpellDamageRating += statsTotal.SpellDamageFromIntellectPercentage * statsTotal.Intellect;
            statsTotal.SpellDamageRating += statsTotal.SpellDamageFromSpiritPercentage * statsTotal.Spirit;

            statsTotal.ArcaneResistance += statsTotal.AllResist + statsTotal.ArcaneResistanceBuff;
            statsTotal.FireResistance += statsTotal.AllResist + statsTotal.FireResistanceBuff;
            statsTotal.FrostResistance += statsTotal.AllResist + statsTotal.FrostResistanceBuff;
            statsTotal.NatureResistance += statsTotal.AllResist + statsTotal.NatureResistanceBuff;
            statsTotal.ShadowResistance += statsTotal.AllResist + statsTotal.ShadowResistanceBuff;

            int playerLevel = 70;
            float bossHitRate = Math.Min(0.99f, ((playerLevel <= calculationOptions.TargetLevel + 2) ? (0.96f - (playerLevel - calculationOptions.TargetLevel) * 0.01f) : (0.94f - (playerLevel - calculationOptions.TargetLevel - 2) * 0.11f)));
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
            statsTotal.Mp5 += 5 * Math.Min(1f, fullResistRate) * 0.01f * calculationOptions.MagicAbsorption * statsTotal.Mana;

            return statsTotal;
        }

        private static string[] TalentList = { "ArcaneSubtlety", "ArcaneFocus", "ImprovedArcaneMissiles", "WandSpecialization", "MagicAbsorption", "ArcaneConcentration", "MagicAttunement", "ArcaneImpact", "ArcaneFortitude", "ImprovedManaShield", "ImprovedCounterspell", "ArcaneMeditation", "ImprovedBlink", "PresenceOfMind", "ArcaneMind", "PrismaticCloak", "ArcaneInstability", "ArcanePotency", "EmpoweredArcaneMissiles", "ArcanePower", "SpellPower", "MindMastery", "Slow", "ImprovedFireball", "Impact", "Ignite", "FlameThrowing", "ImprovedFireBlast", "Incinerate", "ImprovedFlamestrike", "Pyroblast", "BurningSoul", "ImprovedScorch", "ImprovedFireWard", "MasterOfElements", "PlayingWithFire", "CriticalMass", "BlastWave", "BlazingSpeed", "FirePower", "Pyromaniac", "Combustion", "MoltenFury", "EmpoweredFireball", "DragonsBreath", "FrostWarding", "ImprovedFrostbolt", "ElementalPrecision", "IceShards", "Frostbite", "ImprovedFrostNova", "Permafrost", "PiercingIce", "IcyVeins", "ImprovedBlizzard", "ArcticReach", "FrostChanneling", "Shatter", "FrozenCore", "ColdSnap", "ImprovedConeOfCold", "IceFloes", "WintersChill", "IceBarrier", "ArcticWinds", "EmpoweredFrostbolt", "SummonWaterElemental", "PotentSpirit", "StudentOfTheMind" };
        private static string[] TalentListFriendly = { "Arcane Subtlety", "Arcane Focus", "Improved Arcane Missiles", "Wand Specialization", "Magic Absorption", "Arcane Concentration", "Magic Attunement", "Arcane Impact", "Arcane Fortitude", "Improved Mana Shield", "Improved Counterspell", "Arcane Meditation", "Improved Blink", "Presence of Mind", "Arcane Mind", "Prismatic Cloak", "Arcane Instability", "Arcane Potency", "Empowered Arcane Missiles", "Arcane Power", "Spell Power", "Mind Mastery", "Slow", "Improved Fireball", "Impact", "Ignite", "Flame Throwing", "Improved Fire Blast", "Incinerate", "Improved Flamestrike", "Pyroblast", "Burning Soul", "Improved Scorch", "Improved Fire Ward", "Master of Elements", "Playing with Fire", "Critical Mass", "Blast Wave", "Blazing Speed", "Fire Power", "Pyromaniac", "Combustion", "Molten Fury", "Empowered Fireball", "Dragon's Breath", "Frost Warding", "Improved Frostbolt", "Elemental Precision", "Ice Shards", "Frostbite", "Improved Frost Nova", "Permafrost", "Piercing Ice", "Icy Veins", "Improved Blizzard", "Arctic Reach", "Frost Channeling", "Shatter", "Frozen Core", "Cold Snap", "Improved Cone of Cold", "Ice Floes", "Winter's Chill", "Ice Barrier", "Arctic Winds", "Empowered Frostbolt", "Summon Water Elemental", "Potent Spirit", "Student of the Mind" };
        private static int[] MaxTalentPoints = { 2, 5, 5, 2, 5, 5, 2, 3, 1, 2, 2, 3, 2, 1, 5, 2, 3, 3, 3, 1, 2, 5, 1, 5, 5, 5, 2, 3, 2, 3, 1, 2, 3, 2, 3, 3, 3, 1, 2, 5, 3, 1, 2, 5, 1, 2, 5, 3, 5, 3, 2, 3, 3, 1, 3, 2, 3, 5, 3, 1, 3, 2, 5, 1, 5, 5, 1, 2, 3 };

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
                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;
                    bool savedIncrementalOptimizations = calculationOptions.IncrementalOptimizations;
                    bool savedSmartOptimizations = calculationOptions.SmartOptimization;

                    calculationOptions.IncrementalOptimizations = false;
                    calculationOptions.SmartOptimization = true;

                    for (int index = 0; index < TalentList.Length; index++ )
                    {
                        string talent = TalentList[index];
                        int maxPoints = MaxTalentPoints[index];
                        int currentPoints = calculationOptions.GetTalentByName(talent);

                        if (currentPoints > 0)
                        {
                            calculationOptions.SetTalentByName(talent, 0);
                            calc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = string.Format("{0} ({1})", TalentListFriendly[index], currentPoints);
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

                        if (currentPoints < MaxTalentPoints[index])
                        {
                            calculationOptions.SetTalentByName(talent, MaxTalentPoints[index]);
                            calc = GetCharacterCalculations(character) as CharacterCalculationsMage;

                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = string.Format("{0} ({1})", TalentListFriendly[index], MaxTalentPoints[index]);
                            comparison.Equipped = false;
                            comparison.OverallPoints = (calc.OverallPoints - currentCalc.OverallPoints) / (float)(MaxTalentPoints[index] - currentPoints);
                            subPoints = new float[calc.SubPoints.Length];
                            for (int i = 0; i < calc.SubPoints.Length; i++)
                            {
                                subPoints[i] = (calc.SubPoints[i] - currentCalc.SubPoints[i]) / (float)(MaxTalentPoints[index] - currentPoints);
                            }
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }

                        calculationOptions.SetTalentByName(talent, currentPoints);
                    }

                    calculationOptions.IncrementalOptimizations = savedIncrementalOptimizations;
                    calculationOptions.SmartOptimization = savedSmartOptimizations;

                    return comparisonList.ToArray();
                case "Talent Specs":
                    currentCalc = GetCharacterCalculations(character) as CharacterCalculationsMage;
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

                    comparisonList.Add(comparison);

                    string[] talentSpecList = new string[] { "Fire (2/48/11)", "Fire (10/48/3)", "Fire/Cold Snap (0/40/21)", "Frost (10/0/51)", "Arcane (48/0/13)", "Arcane (43/0/18)", "Arcane/Fire (40/18/3)", "Arcane/Fire (40/10/11)", "Arcane/Frost (40/0/21)" };
                    Character charClone = character.Clone();
                    calculationOptions = charClone.CalculationOptions as CalculationOptionsMage;
                    calculationOptions = calculationOptions.Clone();
                    charClone.CalculationOptions = calculationOptions;
                    calculationOptions.IncrementalOptimizations = false;
                    calculationOptions.SmartOptimization = true;

                    for (int index = 0; index < talentSpecList.Length; index++)
                    {
                        LoadTalentSpec(charClone, talentSpecList[index]);

                        calc = GetCharacterCalculations(charClone) as CharacterCalculationsMage;

                        comparison = CreateNewComparisonCalculation();
                        comparison.Name = talentSpecList[index];
                        comparison.Equipped = false;
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
                        new Item() { Stats = new Stats() { SpellDamageRating = 11.7f } },
                        new Item() { Stats = new Stats() { Mp5 = 4 } },
                        new Item() { Stats = new Stats() { SpellCritRating = 10 } },
                        new Item() { Stats = new Stats() { SpellHasteRating = 10 } },
                        new Item() { Stats = new Stats() { SpellHitRating = 10 } },
                    };
                    string[] statList = new string[] {
                        "11.7 Spell Damage",
                        "4 Mana per 5 sec",
                        "10 Spell Crit Rating",
                        "10 Spell Haste Rating",
                        "10 Spell Hit Rating",
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
                    "Spell Hit Rating",
                    "Spell Haste Rating",
                    "PVP Trinket",
                    "Movement Speed",
					};
                return _optimizableCalculationLabels;
            }
        }

        public static void LoadTalentSpec(Character character, string talentSpec)
        {
            string talentCode = null;
            switch (talentSpec)
            {
                case "Fire (2/48/11)":
                    talentCode = "2000000000000000000000050520201230333115312510532000010000000000000";
                    break;
                case "Fire (10/48/3)":
                    talentCode = "2300050000000000000000050520211230333105312510030000000000000000000";
                    break;
                case "Fire/Cold Snap (0/40/21)":
                    talentCode = "0000000000000000000000050510200230233005112500535000310030010000000";
                    break;
                case "Frost (10/0/51)":
                    talentCode = "2300050000000000000000000000000000000000000000535020312235010251551";
                    break;
                case "Arcane (48/0/13)":
                    talentCode = "2550050300230150333125000000000000000000000000534000010000000000000";
                    break;
                case "Arcane (43/0/18)":
                    talentCode = "2250050300030150333125000000000000000000000000515000310030000000000";
                    break;
                case "Arcane/Fire (40/18/3)":
                    talentCode = "2500050300230150330125050520001230000000000000030000000000000000000";
                    break;
                case "Arcane/Fire (40/10/11)":
                    talentCode = "2500050300230150330125050500000000000000000000532000010000000000000";
                    break;
                case "Arcane/Frost (40/0/21)":
                    talentCode = "2500052300030150330125000000000000000000000000535000310030010000000";
                    break;
            }

            LoadTalentCode(character, talentCode);
        }

        public static void LoadTalentCode(Character character, string talentCode)
        {
            if (talentCode == null || talentCode.Length < 66) return;
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;

            calculationOptions.ArcaneSubtlety = int.Parse(talentCode.Substring(0, 1));
            calculationOptions.ArcaneFocus = int.Parse(talentCode.Substring(1, 1));
            calculationOptions.ImprovedArcaneMissiles = int.Parse(talentCode.Substring(2, 1));
            calculationOptions.WandSpecialization = int.Parse(talentCode.Substring(3, 1));
            calculationOptions.MagicAbsorption = int.Parse(talentCode.Substring(4, 1));
            calculationOptions.ArcaneConcentration = int.Parse(talentCode.Substring(5, 1));
            calculationOptions.MagicAttunement = int.Parse(talentCode.Substring(6, 1));
            calculationOptions.ArcaneImpact = int.Parse(talentCode.Substring(7, 1));
            calculationOptions.ArcaneFortitude = int.Parse(talentCode.Substring(8, 1));
            calculationOptions.ImprovedManaShield = int.Parse(talentCode.Substring(9, 1));
            calculationOptions.ImprovedCounterspell = int.Parse(talentCode.Substring(10, 1));
            calculationOptions.ArcaneMeditation = int.Parse(talentCode.Substring(11, 1));
            calculationOptions.ImprovedBlink = int.Parse(talentCode.Substring(12, 1));
            calculationOptions.PresenceOfMind = int.Parse(talentCode.Substring(13, 1));
            calculationOptions.ArcaneMind = int.Parse(talentCode.Substring(14, 1));
            calculationOptions.PrismaticCloak = int.Parse(talentCode.Substring(15, 1));
            calculationOptions.ArcaneInstability = int.Parse(talentCode.Substring(16, 1));
            calculationOptions.ArcanePotency = int.Parse(talentCode.Substring(17, 1));
            calculationOptions.EmpoweredArcaneMissiles = int.Parse(talentCode.Substring(18, 1));
            calculationOptions.ArcanePower = int.Parse(talentCode.Substring(19, 1));
            calculationOptions.SpellPower = int.Parse(talentCode.Substring(20, 1));
            calculationOptions.MindMastery = int.Parse(talentCode.Substring(21, 1));
            calculationOptions.Slow = int.Parse(talentCode.Substring(22, 1));
            calculationOptions.ImprovedFireball = int.Parse(talentCode.Substring(23, 1));
            calculationOptions.Impact = int.Parse(talentCode.Substring(24, 1));
            calculationOptions.Ignite = int.Parse(talentCode.Substring(25, 1));
            calculationOptions.FlameThrowing = int.Parse(talentCode.Substring(26, 1));
            calculationOptions.ImprovedFireBlast = int.Parse(talentCode.Substring(27, 1));
            calculationOptions.Incinerate = int.Parse(talentCode.Substring(28, 1));
            calculationOptions.ImprovedFlamestrike = int.Parse(talentCode.Substring(29, 1));
            calculationOptions.Pyroblast = int.Parse(talentCode.Substring(30, 1));
            calculationOptions.BurningSoul = int.Parse(talentCode.Substring(31, 1));
            calculationOptions.ImprovedScorch = int.Parse(talentCode.Substring(32, 1));
            calculationOptions.ImprovedFireWard = int.Parse(talentCode.Substring(33, 1));
            calculationOptions.MasterOfElements = int.Parse(talentCode.Substring(34, 1));
            calculationOptions.PlayingWithFire = int.Parse(talentCode.Substring(35, 1));
            calculationOptions.CriticalMass = int.Parse(talentCode.Substring(36, 1));
            calculationOptions.BlastWave = int.Parse(talentCode.Substring(37, 1));
            calculationOptions.BlazingSpeed = int.Parse(talentCode.Substring(38, 1));
            calculationOptions.FirePower = int.Parse(talentCode.Substring(39, 1));
            calculationOptions.Pyromaniac = int.Parse(talentCode.Substring(40, 1));
            calculationOptions.Combustion = int.Parse(talentCode.Substring(41, 1));
            calculationOptions.MoltenFury = int.Parse(talentCode.Substring(42, 1));
            calculationOptions.EmpoweredFireball = int.Parse(talentCode.Substring(43, 1));
            calculationOptions.DragonsBreath = int.Parse(talentCode.Substring(44, 1));
            calculationOptions.FrostWarding = int.Parse(talentCode.Substring(45, 1));
            calculationOptions.ImprovedFrostbolt = int.Parse(talentCode.Substring(46, 1));
            calculationOptions.ElementalPrecision = int.Parse(talentCode.Substring(47, 1));
            calculationOptions.IceShards = int.Parse(talentCode.Substring(48, 1));
            calculationOptions.Frostbite = int.Parse(talentCode.Substring(49, 1));
            calculationOptions.ImprovedFrostNova = int.Parse(talentCode.Substring(50, 1));
            calculationOptions.Permafrost = int.Parse(talentCode.Substring(51, 1));
            calculationOptions.PiercingIce = int.Parse(talentCode.Substring(52, 1));
            calculationOptions.IcyVeins = int.Parse(talentCode.Substring(53, 1));
            calculationOptions.ImprovedBlizzard = int.Parse(talentCode.Substring(54, 1));
            calculationOptions.ArcticReach = int.Parse(talentCode.Substring(55, 1));
            calculationOptions.FrostChanneling = int.Parse(talentCode.Substring(56, 1));
            calculationOptions.Shatter = int.Parse(talentCode.Substring(57, 1));
            calculationOptions.FrozenCore = int.Parse(talentCode.Substring(58, 1));
            calculationOptions.ColdSnap = int.Parse(talentCode.Substring(59, 1));
            calculationOptions.ImprovedConeOfCold = int.Parse(talentCode.Substring(60, 1));
            calculationOptions.IceFloes = int.Parse(talentCode.Substring(61, 1));
            calculationOptions.WintersChill = int.Parse(talentCode.Substring(62, 1));
            calculationOptions.IceBarrier = int.Parse(talentCode.Substring(63, 1));
            calculationOptions.ArcticWinds = int.Parse(talentCode.Substring(64, 1));
            calculationOptions.EmpoweredFrostbolt = int.Parse(talentCode.Substring(65, 1));
            calculationOptions.SummonWaterElemental = int.Parse(talentCode.Substring(66, 1));
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
                SpellCritRating = stats.SpellCritRating,
                SpellDamageRating = stats.SpellDamageRating,
                SpellFireDamageRating = stats.SpellFireDamageRating,
                SpellHasteRating = stats.SpellHasteRating,
                SpellHitRating = stats.SpellHitRating,
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
                BonusArcaneSpellPowerMultiplier = stats.BonusArcaneSpellPowerMultiplier,
                BonusFireSpellPowerMultiplier = stats.BonusFireSpellPowerMultiplier,
                BonusFrostSpellPowerMultiplier = stats.BonusFrostSpellPowerMultiplier,
                SpellFrostCritRating = stats.SpellFrostCritRating,
                ArcaneBlastBonus = stats.ArcaneBlastBonus,
                SpellDamageFor6SecOnCrit = stats.SpellDamageFor6SecOnCrit,
                EvocationExtension = stats.EvocationExtension,
                BonusMageNukeMultiplier = stats.BonusMageNukeMultiplier,
                LightningCapacitorProc = stats.LightningCapacitorProc,
                SpellDamageFor20SecOnUse2Min = stats.SpellDamageFor20SecOnUse2Min,
                SpellHasteFor20SecOnUse2Min = stats.SpellHasteFor20SecOnUse2Min,
                Mp5OnCastFor20SecOnUse2Min = stats.Mp5OnCastFor20SecOnUse2Min,
                ManaRestorePerCast = stats.ManaRestorePerCast,
                ManaRestorePerHit = stats.ManaRestorePerHit,
                BonusManaGem = stats.BonusManaGem,
                SpellDamageFor15SecOnManaGem = stats.SpellDamageFor15SecOnManaGem,
                SpellDamageFor10SecOnHit_10_45 = stats.SpellDamageFor10SecOnHit_10_45,
                SpellDamageFromIntellectPercentage = stats.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                SpellDamageFor10SecOnResist = stats.SpellDamageFor10SecOnResist,
                SpellDamageFor15SecOnCrit_20_45 = stats.SpellDamageFor15SecOnCrit_20_45,
                SpellDamageFor15SecOnUse90Sec = stats.SpellDamageFor15SecOnUse90Sec,
                SpellHasteFor5SecOnCrit_50 = stats.SpellHasteFor5SecOnCrit_50,
                SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                SpellDamageFor10SecOnHit_5 = stats.SpellDamageFor10SecOnHit_5,
                SpellHasteFor6SecOnHit_10_45 = stats.SpellHasteFor6SecOnHit_10_45,
                SpellDamageFor10SecOnCrit_20_45 = stats.SpellDamageFor10SecOnCrit_20_45,
                BonusManaPotion = stats.BonusManaPotion,
                MageSpellCrit = stats.MageSpellCrit,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                SpellHasteFor20SecOnUse5Min = stats.SpellHasteFor20SecOnUse5Min,
                AldorRegaliaInterruptProtection = stats.AldorRegaliaInterruptProtection,
                SpellDamageFor15SecOnUse2Min = stats.SpellDamageFor15SecOnUse2Min,
                ShatteredSunAcumenProc = stats.ShatteredSunAcumenProc,
                ManaRestorePerCast_5_15 = stats.ManaRestorePerCast_5_15,
                InterruptProtection = stats.InterruptProtection,
                ArcaneResistanceBuff = stats.ArcaneResistanceBuff,
                FireResistanceBuff = stats.FireResistanceBuff,
                FrostResistanceBuff = stats.FrostResistanceBuff,
                ShadowResistanceBuff = stats.ShadowResistanceBuff,
                NatureResistanceBuff = stats.NatureResistanceBuff,
                PVPTrinket = stats.PVPTrinket,
                MovementSpeed = stats.MovementSpeed,
                MageIceArmor = stats.MageIceArmor,
            };
        }

        public override bool HasRelevantStats(Stats stats)
        {
            float mageStats = stats.Intellect + stats.Spirit + stats.Mp5 + stats.SpellCritRating + stats.SpellDamageRating + stats.SpellFireDamageRating + stats.SpellHasteRating + stats.SpellHitRating + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpellPowerMultiplier + stats.BonusSpiritMultiplier + stats.SpellFrostDamageRating + stats.SpellArcaneDamageRating + stats.SpellPenetration + stats.Mana + stats.SpellCombatManaRegeneration + stats.BonusArcaneSpellPowerMultiplier + stats.BonusFireSpellPowerMultiplier + stats.BonusFrostSpellPowerMultiplier + stats.SpellFrostCritRating + stats.ArcaneBlastBonus + stats.SpellDamageFor6SecOnCrit + stats.EvocationExtension + stats.BonusMageNukeMultiplier + stats.LightningCapacitorProc + stats.SpellDamageFor20SecOnUse2Min + stats.SpellHasteFor20SecOnUse2Min + stats.Mp5OnCastFor20SecOnUse2Min + stats.ManaRestorePerHit + stats.ManaRestorePerCast + stats.SpellDamageFor15SecOnManaGem + stats.BonusManaGem + stats.SpellDamageFor10SecOnHit_10_45 + stats.SpellDamageFromIntellectPercentage + stats.SpellDamageFromSpiritPercentage + stats.SpellDamageFor10SecOnResist + stats.SpellDamageFor15SecOnCrit_20_45 + stats.SpellDamageFor15SecOnUse90Sec + stats.SpellHasteFor5SecOnCrit_50 + stats.SpellHasteFor6SecOnCast_15_45 + stats.SpellDamageFor10SecOnHit_5 + stats.SpellHasteFor6SecOnHit_10_45 + stats.SpellDamageFor10SecOnCrit_20_45 + stats.BonusManaPotion + stats.MageSpellCrit + stats.ThreatReductionMultiplier + stats.AllResist + stats.MageAllResist + stats.ArcaneResistance + stats.FireResistance + stats.FrostResistance + stats.NatureResistance + stats.ShadowResistance + stats.SpellHasteFor20SecOnUse5Min + stats.AldorRegaliaInterruptProtection + stats.SpellDamageFor15SecOnUse2Min + stats.ShatteredSunAcumenProc + stats.ManaRestorePerCast_5_15 + stats.InterruptProtection + stats.ArcaneResistanceBuff + stats.FrostResistanceBuff + stats.FireResistanceBuff + stats.NatureResistanceBuff + stats.ShadowResistanceBuff + stats.PVPTrinket + stats.MovementSpeed + stats.Resilience + stats.MageIceArmor;
            float ignoreStats = stats.Agility + stats.Strength + stats.AttackPower + stats.Healing + stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry + stats.DodgeRating + stats.ParryRating + stats.Hit + stats.HitRating + stats.ExpertiseRating + stats.Expertise + stats.Block + stats.BlockRating + stats.BlockValue + stats.SpellShadowDamageRating + stats.SpellNatureDamageRating;
            return (mageStats > 0 || ((stats.Health + stats.Stamina) > 0 && ignoreStats == 0.0f));
        }
    }
}
