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
        private List<GemmingTemplate> _defaultGemmingTemplates = null;
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                if (_defaultGemmingTemplates == null)
                {
                    _defaultGemmingTemplates = new List<GemmingTemplate>();
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon (Purified)", false, 39911, 39957, 39956, 39959, 39946, 39941);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon (Royal)", false, 39911, 39957, 39956, 39959, 39946, 39943);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon (Glowing)", false, 39911, 39957, 39956, 39959, 39946, 39936);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Uncommon (Jewelcrafting)", false, 39911, 39957, 39956, 39959, 39946, 42144);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Purified)", true, 39998, 40049, 40048, 40051, 40047, 40026);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Royal)", false, 39998, 40049, 40048, 40051, 40047, 40027);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Glowing)", false, 39998, 40049, 40048, 40051, 40047, 40025);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Rare (Jewelcrafting)", false, 39998, 40049, 40048, 40051, 40047, 42144);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Purified)", false, 40113, 40153, 40152, 40155, 40151, 40133);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Royal)", false, 40113, 40153, 40152, 40155, 40151, 40134);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Glowing)", false, 40113, 40153, 40152, 40155, 40151, 40132);
                    AddGemmingTemplateGroup(_defaultGemmingTemplates, "Epic (Jewelcrafting)", false, 40113, 40153, 40152, 40155, 40151, 42144);
                }
                return _defaultGemmingTemplates;
            }
        }

        private void AddGemmingTemplateGroup(List<GemmingTemplate> list, string name, bool enabled, int runed, int veiled, int potent, int reckless, int luminous, int blue)
        {
            list.Add(new GemmingTemplate() { Model = "Mage", Group = name, RedId = runed, YellowId = runed, BlueId = runed, PrismaticId = runed, MetaId = 41285, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Mage", Group = name, RedId = runed, YellowId = veiled, BlueId = blue, PrismaticId = runed, MetaId = 41285, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Mage", Group = name, RedId = runed, YellowId = potent, BlueId = blue, PrismaticId = runed, MetaId = 41285, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Mage", Group = name, RedId = runed, YellowId = reckless, BlueId = blue, PrismaticId = runed, MetaId = 41285, Enabled = enabled });
            list.Add(new GemmingTemplate() { Model = "Mage", Group = name, RedId = runed, YellowId = luminous, BlueId = blue, PrismaticId = runed, MetaId = 41285, Enabled = enabled });
        }

        public CalculationsMage()
        {
            _subPointNameColorsRating = new Dictionary<string, System.Drawing.Color>();
            _subPointNameColorsRating.Add("Dps", System.Drawing.Color.FromArgb(0, 128, 255));
            _subPointNameColorsRating.Add("Survivability", System.Drawing.Color.FromArgb(64, 128, 32));

            _subPointNameColorsMana = new Dictionary<string, Color>();
            _subPointNameColorsMana.Add("Mana", System.Drawing.Color.FromArgb(0, 0, 255));

            _subPointNameColors = _subPointNameColorsRating;
        }

        private Dictionary<string, System.Drawing.Color> _subPointNameColors = null;
        private Dictionary<string, System.Drawing.Color> _subPointNameColorsRating = null;
        private Dictionary<string, System.Drawing.Color> _subPointNameColorsMana = null;
        public override Dictionary<string, System.Drawing.Color> SubPointNameColors
        {
            get
            {
                Dictionary<string, System.Drawing.Color> ret = _subPointNameColors;
                _subPointNameColors = _subPointNameColorsRating;
                return ret;
            }
        }

        public override ICalculationOptionBase DeserializeDataObject(string xml)
        {
            xml = xml.Replace("ArcaneBlast00", "ArcaneBlast0");
            xml = xml.Replace("ArcaneBlast11", "ArcaneBlast1");
            xml = xml.Replace("ArcaneBlast22", "ArcaneBlast2");
            xml = xml.Replace("ArcaneBlast33", "ArcaneBlast3");
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
                    "Solution:Minimum Range",
                    "Solution:Threat Reduction",
                    "Spell Info:Wand",
                    "Spell Info:Arcane Missiles",
                    "Spell Info:MBAM*Missile Barrage Arcane Missiles",
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
                    "Spell Info:Ice Lance",
                    "Spell Info:FrBFB*Fireball on Brain Freeze",
                    "Spell Info:FrBFBIL*Fireball on Brain Freeze, Ice Lance on shatter combo, use Brain Freeze on shatter combo when available",
                    "Spell Info:ABAM*AB-AM regardless of procs",
                    "Spell Info:ABABar*AB-ABar regardless of procs",
                    "Spell Info:ABarAM*ABar-AM regardless of procs",
                    "Spell Info:ABABar0C*AB-ABar, on Missile Barrage replace with MBAM-ABar",
                    "Spell Info:ABABar1C*AB-ABar, on Missile Barrage replace with AB-MBAM-ABar",
                    "Spell Info:ABABar2C*AB-ABar, on MB proc do MBAM-ABar at 2 stack",
                    "Spell Info:ABABar3C*AB-ABar, on MB proc do MBAM-ABar at 3 stack",
                    "Spell Info:ABABar0MBAM*AB-ABar, on MB proc insert MBAM after ABar and restart from AB",
                    "Spell Info:ABABar1MBAM*AB-ABar, on MB proc replace ABar with MBAM",
                    "Spell Info:ABABar2MBAM*AB-ABar, on MB proc do MBAM at 2 stack",
                    "Spell Info:AB2ABar2MBAM*AB-AB-ABar, on MB proc replace ABar with MBAM",
                    "Spell Info:AB2ABar2C*AB-AB-ABar, on MB proc do MBAM-ABar at 2 stack",
                    "Spell Info:AB2ABar3C*AB-AB-ABar, on MB proc do MBAM-ABar at 3 stack",
                    "Spell Info:AB3AM2MBAM*AB-AB-AB-AM, if first AB procs MB do MBAM at 2 stack",
                    "Spell Info:AB3AMABar2C*AB-AB-AB-AM-ABar, if ABar or first AB procs do MBAM-ABar at 2 stack",
                    "Spell Info:AB3AMABar*AB-AB-AB-AM-ABar regardless of procs",
                    "Spell Info:AB3AM*AB-AB-AB-AM regardless of procs",
                    "Spell Info:AB3ABar3MBAM*AB-AB-AB-ABar, on MB proc do MBAM at 3 stack",
                    "Spell Info:AB3ABar3C*AB-AB-AB-ABar, on MB proc do MBAM-ABar at 3 stack",
                    "Spell Info:ABSpamMBAM*Spam AB, MBAM as soon as you notice",
                    "Spell Info:ABSpam3MBAM*Spam AB, always ramp up to 3 stack before MBAM",
                    "Spell Info:ABSpam3C*Spam AB, always ramp up to 3 stack before MBAM-ABar",
                    "Spell Info:ABSpam03C*Spam AB, ramp up to 3 stack or before MBAM-ABar or immediately after ABar",
                    "Spell Info:ABABarSlow*Arcane Missiles on Missile Barrage (after Arcane Barrage), maintain Slow",
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
                    _customChartNames = new string[] { "Item Budget", "Mana Sources", "Mana Usage" };
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
					_customRenderedChartNames = new string[] { "Sequence Reconstruction", "Scaling vs Spell Power", "Scaling vs Crit Rating", "Scaling vs Haste Rating", "Scaling vs Intellect", "Scaling vs Spirit" };
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

        public override void SetDefaults(Character character)
        {
            character.ActiveBuffs.Add(Buff.GetBuffByName("Sanctified Retribution"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Swift Retribution"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Arcane Intellect"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Judgements of the Wise"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Wisdom"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Blessing of Wisdom"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Elemental Oath"));
            if (character.MageTalents.FocusMagic == 1) character.ActiveBuffs.Add(Buff.GetBuffByName("Focus Magic"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Wrath of Air Totem"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Totem of Wrath (Spell Power)"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Divine Spirit"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Power Word: Fortitude"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Power Word: Fortitude"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Mark of the Wild"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Mark of the Wild"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Blessing of Kings"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Concentration Aura"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Concentration Aura"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Heart of the Crusader"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Judgement of Wisdom"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Improved Scorch"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Ebon Plaguebringer"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Misery"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Flask of the Frost Wyrm"));
            character.ActiveBuffs.Add(Buff.GetBuffByName("Fish Feast"));
            if (character.MageTalents.EmpoweredFire > 0)
            {
                if (character.MageTalents.PiercingIce == 3 && character.MageTalents.IceShards == 3)
                {
                    character.MageTalents.GlyphOfFrostfire = true;
                    character.MageTalents.GlyphOfMoltenArmor = true;
                    character.MageTalents.GlyphOfLivingBomb = true;
                }
                else
                {
                    character.MageTalents.GlyphOfFireball = true;
                    character.MageTalents.GlyphOfMoltenArmor = true;
                    character.MageTalents.GlyphOfLivingBomb = true;
                }
            }
            else if (character.MageTalents.EmpoweredFrostbolt > 0)
            {
                character.MageTalents.GlyphOfFrostbolt = true;
                character.MageTalents.GlyphOfMoltenArmor = true;
                character.MageTalents.GlyphOfIceLance = true;
            }
            else if (character.MageTalents.ArcaneEmpowerment > 0)
            {
                character.MageTalents.GlyphOfArcaneMissiles = true;
                character.MageTalents.GlyphOfMoltenArmor = true;
                character.MageTalents.GlyphOfArcaneBlast = true;
            }
        }

        public override string GetCharacterStatsString(Character character)
        {
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("Character:\t\t{0}@{1}-{2}\r\nRace:\t\t{3}",
				character.Name, character.Region, character.Realm, character.Race);

            CalculationOptionsMage CalculationOptions = (CalculationOptionsMage)character.CalculationOptions;

            if (CalculationOptions.Calculations == null || CalculationOptions.Calculations.DisplayCalculationValues == null)
            {
                return base.GetCharacterStatsString(character);
            }

            Dictionary<string, string> dict = CalculationOptions.Calculations.DisplayCalculationValues;
			foreach (KeyValuePair<string, string> kvp in dict)
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

			return sb.ToString();
        }

		public override Character.CharacterClass TargetClass { get { return Character.CharacterClass.Mage; } }
		public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationMage(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsMage(); }

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            return GetCharacterCalculations(character, additionalItem, referenceCalculation && calculationOptions.IncrementalOptimizations, significantChange, calculationOptions.SmartOptimization && !significantChange, needsDisplayCalculations);
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool computeIncrementalSet, bool ignoreIncrementalSet, bool useGlobalOptimizations, bool needsDisplayCalculations)
        {
            CharacterCalculationsBase ret;
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            bool useIncrementalOptimizations = calculationOptions.IncrementalOptimizations && !ignoreIncrementalSet;
            if (useIncrementalOptimizations && calculationOptions.IncrementalSetStateIndexes == null) computeIncrementalSet = true;
            if (computeIncrementalSet) useIncrementalOptimizations = false;
            if (useIncrementalOptimizations && !character.DisableBuffAutoActivation)
            {
                ret = GetCharacterCalculations(character, additionalItem, calculationOptions, calculationOptions.IncrementalSetArmor, useIncrementalOptimizations, useGlobalOptimizations, needsDisplayCalculations, computeIncrementalSet);
            }
            else if (calculationOptions.AutomaticArmor && !character.DisableBuffAutoActivation)
            {
                CharacterCalculationsBase mage = GetCharacterCalculations(character, additionalItem, calculationOptions, "Mage Armor", useIncrementalOptimizations, useGlobalOptimizations, needsDisplayCalculations, computeIncrementalSet);
                CharacterCalculationsBase molten = GetCharacterCalculations(character, additionalItem, calculationOptions, "Molten Armor", useIncrementalOptimizations, useGlobalOptimizations, needsDisplayCalculations, computeIncrementalSet);
                CharacterCalculationsBase calc = (mage.OverallPoints > molten.OverallPoints) ? mage : molten;
                if (calculationOptions.MeleeDps + calculationOptions.MeleeDot + calculationOptions.PhysicalDps + calculationOptions.PhysicalDot + calculationOptions.FrostDps + calculationOptions.FrostDot > 0)
                {
                    CharacterCalculationsBase ice = GetCharacterCalculations(character, additionalItem, calculationOptions, "Ice Armor", useIncrementalOptimizations, useGlobalOptimizations, needsDisplayCalculations, computeIncrementalSet);
                    if (ice.OverallPoints > calc.OverallPoints) calc = ice;
                }
                if (computeIncrementalSet) StoreIncrementalSet(character, (CharacterCalculationsMage)calc);
                ret = calc;
            }
            else
            {
                CharacterCalculationsBase calc = GetCharacterCalculations(character, additionalItem, calculationOptions, null, useIncrementalOptimizations, useGlobalOptimizations, needsDisplayCalculations, computeIncrementalSet);
                if (computeIncrementalSet) StoreIncrementalSet(character, (CharacterCalculationsMage)calc);
                ret = calc;
            }
            return ret;
        }

        private void StoreIncrementalSet(Character character, CharacterCalculationsMage calculations)
        {
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            List<Cooldown> cooldownList = new List<Cooldown>();
            List<CycleId> spellList = new List<CycleId>();
            List<int> segmentList = new List<int>();
            for (int i = 0; i < calculations.SolutionVariable.Count; i++)
            {
                if (calculations.Solution[i] > 0 && calculations.SolutionVariable[i].Type == VariableType.Spell)
                {
                    Cooldown cooldown = calculations.SolutionVariable[i].State.Cooldown & Cooldown.NonItemBasedMask;
                    CycleId spellId = calculations.SolutionVariable[i].Cycle.CycleId;
                    int segment = calculations.SolutionVariable[i].Segment;
                    bool found = false;
                    for (int j = 0; j < cooldownList.Count; j++)
                    {
                        if (cooldownList[j] == cooldown && spellList[j] == spellId && segmentList[j] == segment)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        cooldownList.Add(cooldown);
                        spellList.Add(spellId);
                        segmentList.Add(segment);
                    }
                }
            }
            calculationOptions.IncrementalSetStateIndexes = cooldownList.ToArray();
            calculationOptions.IncrementalSetSpells = spellList.ToArray();
            calculationOptions.IncrementalSetSegments = segmentList.ToArray();
            if (calculationOptions.AutomaticArmor)
            {
                calculationOptions.IncrementalSetArmor = calculations.MageArmor;
            }
            else
            {
                calculationOptions.IncrementalSetArmor = null;
            }

            List<Cooldown> filteredCooldowns = ListUtils.RemoveDuplicates(cooldownList);
            filteredCooldowns.Sort();
            calculationOptions.IncrementalSetSortedStates = filteredCooldowns.ToArray();
        }

        public CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, CalculationOptionsMage calculationOptions, string armor, bool useIncrementalOptimizations, bool useGlobalOptimizations, bool needsDisplayCalculations, bool needsSolutionVariables)
        {
            return Solver.GetCharacterCalculations(character, additionalItem, calculationOptions, this, armor, calculationOptions.ComparisonSegmentCooldowns, calculationOptions.ComparisonIntegralMana, calculationOptions.ComparisonAdvancedConstraintsLevel, useIncrementalOptimizations, useGlobalOptimizations, needsDisplayCalculations, needsSolutionVariables);
        }

        public Stats GetRawStats(Character character, Item additionalItem, CalculationOptionsMage calculationOptions, List<Buff> autoActivatedBuffs, string armor, out List<Buff> activeBuffs)
        {
            Stats stats = new Stats();
            AccumulateItemStats(stats, character, additionalItem);

            activeBuffs = new List<Buff>();
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
                            RemoveConflictingBuffs(activeBuffs, improvedScorch);
                        }
                    }
                }
                if (character.MageTalents.WintersChill > 0)
                {
                    if (!character.ActiveBuffs.Contains(wintersChill) && !character.ActiveBuffs.Contains(improvedScorch))
                    {
                        activeBuffs.Add(wintersChill);
                        autoActivatedBuffs.Add(wintersChill);
                        RemoveConflictingBuffs(activeBuffs, wintersChill);
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

            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.MaxStack > 1 && effect.Chance == 1f && effect.Cooldown == 0f && (effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellHit))
                {
                    effect.Stats.GenerateSparseData();
                    stats.Accumulate(effect.Stats, effect.MaxStack);
                }
            }

            return stats;
        }

        // required by base class, but never used
        public override Stats GetCharacterStats(Character character, Item additionalItem)
        {
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            List<Buff> ignore;
            return GetCharacterStats(character, additionalItem, GetRawStats(character, additionalItem, calculationOptions, new List<Buff>(), null, out ignore), calculationOptions);
        }

        public Stats GetCharacterStats(Character character, Item additionalItem, Stats rawStats, CalculationOptionsMage calculationOptions)
        {
            float statsRaceHealth;
            float statsRaceMana;
            float statsRaceStrength;
            float statsRaceAgility;
            float statsRaceStamina;
            float statsRaceIntellect;
            float statsRaceSpirit;
            switch (calculationOptions.PlayerLevel)
            {
                case 70:
                    statsRaceHealth = 3213f;
                    statsRaceMana = 1961f;
                    switch (character.Race)
                    {
                        case Character.CharacterRace.BloodElf:
                            statsRaceStrength = 28f;
                            statsRaceAgility = 42f;
                            statsRaceStamina = 49f;
                            statsRaceIntellect = 149f;
                            statsRaceSpirit = 144;
                            break;
                        case Character.CharacterRace.Draenei:
                            statsRaceStrength = 28f;
                            statsRaceAgility = 42f;
                            statsRaceStamina = 50f;
                            statsRaceIntellect = 152f;
                            statsRaceSpirit = 147;
                            break;
                        case Character.CharacterRace.Human:
                            statsRaceStrength = 28f;
                            statsRaceAgility = 42f;
                            statsRaceStamina = 51f;
                            statsRaceIntellect = 151f;
                            statsRaceSpirit = 145;
                            break;
                        case Character.CharacterRace.Troll:
                            statsRaceStrength = 28f;
                            statsRaceAgility = 42f;
                            statsRaceStamina = 52f;
                            statsRaceIntellect = 147f;
                            statsRaceSpirit = 146;
                            break;
                        case Character.CharacterRace.Undead:
                            statsRaceStrength = 28f;
                            statsRaceAgility = 42f;
                            statsRaceStamina = 52f;
                            statsRaceIntellect = 149f;
                            statsRaceSpirit = 150;
                            break;
                        case Character.CharacterRace.Gnome:
                        default:
                            statsRaceStrength = 28f;
                            statsRaceAgility = 42f;
                            statsRaceStamina = 50f;
                            statsRaceIntellect = 154f;
                            statsRaceSpirit = 145;
                            break;
                    }
                    break;
                case 71:
                    statsRaceHealth = 3308f;
                    statsRaceMana = 2063f;
                    statsRaceStrength = 28f;
                    statsRaceAgility = 42f;
                    statsRaceStamina = 51f;
                    statsRaceIntellect = 157f;
                    statsRaceSpirit = 148f;
                    break;
                case 72:
                    statsRaceHealth = 3406f;
                    statsRaceMana = 2166f;
                    statsRaceStrength = 28f;
                    statsRaceAgility = 43f;
                    statsRaceStamina = 52f;
                    statsRaceIntellect = 160f;
                    statsRaceSpirit = 151f;
                    break;
                case 73:
                    statsRaceHealth = 3505f;
                    statsRaceMana = 2269f;
                    statsRaceStrength = 28f;
                    statsRaceAgility = 43f;
                    statsRaceStamina = 53f;
                    statsRaceIntellect = 163f;
                    statsRaceSpirit = 154f;
                    break;
                case 74:
                    statsRaceHealth = 3623f;
                    statsRaceMana = 2372f;
                    statsRaceStrength = 29f;
                    statsRaceAgility = 44f;
                    statsRaceStamina = 53f;
                    statsRaceIntellect = 166f;
                    statsRaceSpirit = 156f;
                    break;
                case 75:
                    statsRaceHealth = 3726f;
                    statsRaceMana = 2474f;
                    statsRaceStrength = 29f;
                    statsRaceAgility = 44f;
                    statsRaceStamina = 54f;
                    statsRaceIntellect = 169f;
                    statsRaceSpirit = 159f;
                    break;
                case 76:
                    statsRaceHealth = 3830f;
                    statsRaceMana = 2577f;
                    statsRaceStrength = 29f;
                    statsRaceAgility = 44f;
                    statsRaceStamina = 55f;
                    statsRaceIntellect = 172f;
                    statsRaceSpirit = 162f;
                    break;
                case 77:
                    statsRaceHealth = 3937f;
                    statsRaceMana = 2680f;
                    statsRaceStrength = 30f;
                    statsRaceAgility = 45f;
                    statsRaceStamina = 56f;
                    statsRaceIntellect = 175f;
                    statsRaceSpirit = 165f;
                    break;
                case 78:
                    statsRaceHealth = 4063f;
                    statsRaceMana = 2783f;
                    statsRaceStrength = 30f;
                    statsRaceAgility = 45f;
                    statsRaceStamina = 56f;
                    statsRaceIntellect = 178f;
                    statsRaceSpirit = 168f;
                    break;
                case 79:
                    statsRaceHealth = 4172f;
                    statsRaceMana = 2885f;
                    statsRaceStrength = 30f;
                    statsRaceAgility = 46f;
                    statsRaceStamina = 57f;
                    statsRaceIntellect = 181f;
                    statsRaceSpirit = 171f;
                    break;
                case 80:
                default:
                    statsRaceHealth = 6783f;
                    statsRaceMana = 2988f;
                    switch (character.Race)
                    {
                        case Character.CharacterRace.BloodElf:
                            statsRaceStrength = 33f;
                            statsRaceAgility = 45f;
                            statsRaceStamina = 57f;
                            statsRaceIntellect = 185f;
                            statsRaceSpirit = 173f;
                            break;
                        case Character.CharacterRace.Draenei:
                            statsRaceStrength = 37f;
                            statsRaceAgility = 40f;
                            statsRaceStamina = 58f;
                            statsRaceIntellect = 182f;
                            statsRaceSpirit = 176f;
                            break;
                        case Character.CharacterRace.Human:
                            statsRaceStrength = 36f;
                            statsRaceAgility = 43f;
                            statsRaceStamina = 59f;
                            statsRaceIntellect = 181f;
                            statsRaceSpirit = 174f;
                            break;
                        case Character.CharacterRace.Troll:
                            statsRaceStrength = 37f;
                            statsRaceAgility = 45f;
                            statsRaceStamina = 60f;
                            statsRaceIntellect = 177f;
                            statsRaceSpirit = 175f;
                            break;
                        case Character.CharacterRace.Undead:
                            statsRaceStrength = 35f;
                            statsRaceAgility = 41f;
                            statsRaceStamina = 60f;
                            statsRaceIntellect = 179f;
                            statsRaceSpirit = 179f;
                            break;
                        case Character.CharacterRace.Gnome:
                        default:
                            statsRaceStrength = 31f;
                            statsRaceAgility = 46f;
                            statsRaceStamina = 58f;
                            statsRaceIntellect = 184f;
                            statsRaceSpirit = 174f;
                            break;
                    }
                    break;
            }
            float statsRaceBonusIntellectMultiplier;
            if (character.Race == Character.CharacterRace.Gnome)
            {
                statsRaceBonusIntellectMultiplier = 1.05f * (1 + 0.03f * character.MageTalents.ArcaneMind) - 1.0f;
            }
            else
            {
                statsRaceBonusIntellectMultiplier = 0.03f * character.MageTalents.ArcaneMind;
            }
            float statsRaceBonusSpiritMultiplier = 0.0f;
            if (character.Race == Character.CharacterRace.Human)
            {
                statsRaceBonusSpiritMultiplier = 0.03f;
            }
        
            Stats statsTotal = rawStats;
            if (calculationOptions.EvocationWeapon + calculationOptions.EvocationSpirit > 0) // we'll need raw stats to calculate evocation stats
            {
                statsTotal = rawStats.Clone();
            }

            if (character.MageTalents.StudentOfTheMind > 0)
            {
                statsTotal.BonusSpiritMultiplier = (1 + statsTotal.BonusSpiritMultiplier) * (1.01f + 0.03f * character.MageTalents.StudentOfTheMind) - 1;
            }
            if (calculationOptions.EffectSpiritBonus > 0)
            {
                statsTotal.BonusSpiritMultiplier = (1 + statsTotal.BonusSpiritMultiplier) * (1f + calculationOptions.EffectSpiritBonus / 100f) - 1;
            }
            statsTotal.Strength = (float)Math.Floor(0.00001 + statsRaceStrength * (1 + statsTotal.BonusStrengthMultiplier)) + (float)Math.Floor(0.00001 + statsTotal.Strength * (1 + statsTotal.BonusStrengthMultiplier));
            statsTotal.Agility = (float)Math.Floor(0.00001 + statsRaceAgility * (1 + statsTotal.BonusAgilityMultiplier)) + (float)Math.Floor(0.00001 + statsTotal.Agility * (1 + statsTotal.BonusAgilityMultiplier));
            statsTotal.Intellect = (float)Math.Floor(0.00001 + Math.Floor(statsRaceIntellect * (1 + statsRaceBonusIntellectMultiplier)) * (1 + statsTotal.BonusIntellectMultiplier)) + (float)Math.Floor(0.00001 + statsTotal.Intellect * (1 + statsRaceBonusIntellectMultiplier) * (1 + statsTotal.BonusIntellectMultiplier));
            statsTotal.Stamina = (float)Math.Floor(0.00001 + statsRaceStamina * (1 + statsTotal.BonusStaminaMultiplier)) + (float)Math.Floor(0.00001 + statsTotal.Stamina * (1 + statsTotal.BonusStaminaMultiplier));
            statsTotal.Spirit = (float)Math.Floor(0.00001 + Math.Floor(statsRaceSpirit * (1 + statsRaceBonusSpiritMultiplier)) * (1 + statsTotal.BonusSpiritMultiplier)) + (float)Math.Floor(0.00001 + statsTotal.Spirit * (1 + statsRaceBonusSpiritMultiplier) * (1 + statsTotal.BonusSpiritMultiplier));

            statsTotal.Health = (float)Math.Round(((statsTotal.Health + statsRaceHealth + (statsTotal.Stamina * 10f)) * (character.Race == Character.CharacterRace.Tauren ? 1.05f : 1f)));
            statsTotal.Mana = (float)Math.Round(statsTotal.Mana + statsRaceMana + 15f * statsTotal.Intellect);
            statsTotal.Armor = (float)Math.Round(statsTotal.Armor + statsTotal.Agility * 2f + 0.5f * statsTotal.Intellect * character.MageTalents.ArcaneFortitude);

            if (character.Race == Character.CharacterRace.BloodElf)
            {
                statsTotal.Mp5 += 5 * 0.06f * statsTotal.Mana / 120;
            }

            float magicAbsorption = 0.5f * calculationOptions.PlayerLevel * character.MageTalents.MagicAbsorption;
            int frostWarding = character.MageTalents.FrostWarding;
            statsTotal.AllResist += magicAbsorption;

            if (statsTotal.MageIceArmor > 0)
            {
                statsTotal.Armor += (float)Math.Floor((calculationOptions.PlayerLevel < 79 ? 645 : 940) * (1 + 0.25f * frostWarding) * (1 + (character.MageTalents.GlyphOfIceArmor ? 0.2f : 0.0f)));
                statsTotal.FrostResistance += (float)Math.Floor((calculationOptions.PlayerLevel < 79 ? 18 : 40) * (1 + 0.25f * frostWarding) * (1 + (character.MageTalents.GlyphOfIceArmor ? 0.2f : 0.0f)));
            }
            if (statsTotal.MageMageArmor > 0)
            {
                if (calculationOptions.Mode31)
                {
                    statsTotal.SpellCombatManaRegeneration += 0.5f + (character.MageTalents.GlyphOfMageArmor ? 0.2f : 0.0f);
                }
                else
                {
                    statsTotal.SpellCombatManaRegeneration += 0.3f + (character.MageTalents.GlyphOfMageArmor ? 0.2f : 0.0f);
                }
                statsTotal.AllResist += (calculationOptions.PlayerLevel < 71 ? 18f : (calculationOptions.PlayerLevel < 79 ? 21f : 40f)) * (1 + character.MageTalents.ArcaneShielding * 0.25f);
            }
            if (statsTotal.MageMoltenArmor > 0)
            {
                if (calculationOptions.Mode31)
                {
                    statsTotal.CritRating += (0.35f + (character.MageTalents.GlyphOfMoltenArmor ? 0.2f : 0.0f)) * statsTotal.Spirit;
                }
                else
                {
                    statsTotal.SpellCrit += 0.03f + (character.MageTalents.GlyphOfMoltenArmor ? 0.02f : 0.0f);
                }
            }
            if (calculationOptions.EffectCritBonus > 0)
            {
                statsTotal.SpellCrit += calculationOptions.EffectCritBonus;
            }
            if (character.MageTalents.GlyphOfManaGem)
            {
                statsTotal.BonusManaGem += 0.4f;
            }

            if (calculationOptions.Mode31)
            {
                switch (character.MageTalents.ArcaneMeditation)
                {
                    case 1:
                        statsTotal.SpellCombatManaRegeneration += 0.17f;
                        break;
                    case 2:
                        statsTotal.SpellCombatManaRegeneration += 0.33f;
                        break;
                    case 3:
                        statsTotal.SpellCombatManaRegeneration += 0.5f;
                        break;
                }
                switch (character.MageTalents.Pyromaniac)
                {
                    case 1:
                        statsTotal.SpellCombatManaRegeneration += 0.17f;
                        break;
                    case 2:
                        statsTotal.SpellCombatManaRegeneration += 0.33f;
                        break;
                    case 3:
                        statsTotal.SpellCombatManaRegeneration += 0.5f;
                        break;
                }
            }
            else
            {
                statsTotal.SpellCombatManaRegeneration += 0.1f * character.MageTalents.ArcaneMeditation;
                statsTotal.SpellCombatManaRegeneration += 0.1f * character.MageTalents.Pyromaniac;
            }

            if (statsTotal.SpellCombatManaRegeneration > 1.0f) statsTotal.SpellCombatManaRegeneration = 1.0f;

            //statsTotal.Mp5 += calculationOptions.ShadowPriest;

            statsTotal.SpellDamageFromIntellectPercentage += 0.03f * character.MageTalents.MindMastery;

            statsTotal.AllResist += statsTotal.MageAllResist;

            statsTotal.SpellPower += statsTotal.BonusSpellPowerDemonicPactMultiplier * calculationOptions.WarlockSpellPower;
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
            statsTotal.Mp5 -= 5 * calculationOptions.EffectShadowManaDrain * calculationOptions.EffectShadowManaDrainFrequency * bossHitRate * (1 - StatConversion.GetAverageResistance(calculationOptions.TargetLevel, calculationOptions.PlayerLevel, statsTotal.ShadowResistance, 0));

            float fullResistRate = calculationOptions.EffectArcaneOtherBinary * (1 - bossHitRate * (1 - StatConversion.GetAverageResistance(calculationOptions.TargetLevel, calculationOptions.PlayerLevel, statsTotal.ArcaneResistance, 0)));
            fullResistRate += calculationOptions.EffectFireOtherBinary * (1 - bossHitRate * (1 - StatConversion.GetAverageResistance(calculationOptions.TargetLevel, calculationOptions.PlayerLevel, statsTotal.FireResistance, 0)));
            fullResistRate += calculationOptions.EffectFrostOtherBinary * (1 - bossHitRate * (1 - StatConversion.GetAverageResistance(calculationOptions.TargetLevel, calculationOptions.PlayerLevel, statsTotal.FrostResistance, 0)));
            fullResistRate += calculationOptions.EffectShadowOtherBinary * (1 - bossHitRate * (1 - StatConversion.GetAverageResistance(calculationOptions.TargetLevel, calculationOptions.PlayerLevel, statsTotal.ShadowResistance, 0)));
            fullResistRate += calculationOptions.EffectNatureOtherBinary * (1 - bossHitRate * (1 - StatConversion.GetAverageResistance(calculationOptions.TargetLevel, calculationOptions.PlayerLevel, statsTotal.NatureResistance, 0)));
            fullResistRate += calculationOptions.EffectHolyOtherBinary * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectArcaneOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectFireOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectFrostOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectShadowOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectNatureOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectHolyOther * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectShadowManaDrainFrequency * (1 - bossHitRate);
            fullResistRate += calculationOptions.EffectShadowSilenceFrequency * (1 - bossHitRate * StatConversion.GetAverageResistance(calculationOptions.TargetLevel, calculationOptions.PlayerLevel, statsTotal.ShadowResistance, 0));
            statsTotal.Mp5 += 5 * Math.Min(1f, fullResistRate) * 0.01f * character.MageTalents.MagicAbsorption * statsTotal.Mana;

            return statsTotal;
        }
         
        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            ComparisonCalculationBase comparison;
            float[] subPoints;

            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            MageTalents talents = character.MageTalents;
            CharacterCalculationsMage calculationResult = calculationOptions.Calculations;

            switch (chartName)
            {
                case "Item Budget":
                    return EvaluateItemBudget(character);
                case "Mana Sources":
                    _subPointNameColors = _subPointNameColorsMana;
                    foreach (KeyValuePair<string, float> kvp in calculationResult.ManaSources)
                    {
                        if (kvp.Value > 0)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = kvp.Key;
                            comparison.Equipped = false;
                            comparison.OverallPoints = kvp.Value;
                            subPoints = new float[] { kvp.Value };
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }
                    }
                    return comparisonList.ToArray();
                case "Mana Usage":
                    _subPointNameColors = _subPointNameColorsMana;
                    foreach (KeyValuePair<string, float> kvp in calculationResult.ManaUsage)
                    {
                        if (kvp.Value > 0)
                        {
                            comparison = CreateNewComparisonCalculation();
                            comparison.Name = kvp.Key;
                            comparison.Equipped = false;
                            comparison.OverallPoints = kvp.Value;
                            subPoints = new float[] { kvp.Value };
                            comparison.SubPoints = subPoints;

                            comparisonList.Add(comparison);
                        }
                    }
                    return comparisonList.ToArray();
                default:
                    return new ComparisonCalculationBase[0];
            }
        }

        private ComparisonCalculationBase[] EvaluateItemBudget(Character character)
        {
            Stats baseStats;
            return EvaluateItemBudget(character, new Stats(), false, out baseStats);
        }

        private ComparisonCalculationBase[] EvaluateItemBudget(Character character, Stats offset, bool forceIncrementalBaseRecalculation, out Stats baseStats)
        {
            List<ComparisonCalculationBase> comparisonList = new List<ComparisonCalculationBase>();
            CharacterCalculationsMage baseCalc, calc;
            ComparisonCalculationBase comparison;
            float[] subPoints;

            Item[] itemList = new Item[] {
                        new Item() { Stats = new Stats() { SpellPower = 11.7f } + offset },
                        new Item() { Stats = new Stats() { Mp5 = 4 } + offset },
                        new Item() { Stats = new Stats() { CritRating = 10 } + offset },
                        new Item() { Stats = new Stats() { HasteRating = 10 } + offset },
                        new Item() { Stats = new Stats() { HitRating = 10 } + offset },
                    };
            string[] statList = new string[] {
                        "11.7 Spell Power",
                        "4 Mana per 5 sec",
                        "10 Crit Rating",
                        "10 Haste Rating",
                        "10 Hit Rating",
                    };

            // offset might be very far from current position, so make sure to force incremental set recalc
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            baseCalc = GetCharacterCalculations(character, new Item() { Stats = offset }, forceIncrementalBaseRecalculation, false, calculationOptions.SmartOptimization, false) as CharacterCalculationsMage;
            baseStats = baseCalc.BaseStats;

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
                statsAtAdd = GetCharacterStats(character, new Item() { Stats = new Stats() { Intellect = intToAdd } + offset });
            }
            calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Intellect = intToAdd } + offset }) as CharacterCalculationsMage;

            Stats statsAtSubtract = baseCalc.BaseStats;
            float intToSubtract = 0f;
            while (baseInt == statsAtSubtract.Intellect && intToSubtract > -2)
            {
                intToSubtract -= 0.01f;
                statsAtSubtract = GetCharacterStats(character, new Item() { Stats = new Stats() { Intellect = intToSubtract } + offset });
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
                statsAtAdd = GetCharacterStats(character, new Item() { Stats = new Stats() { Spirit = spiToAdd } + offset });
            }
            calcAtAdd = GetCharacterCalculations(character, new Item() { Stats = new Stats() { Spirit = spiToAdd } + offset}) as CharacterCalculationsMage;

            statsAtSubtract = baseCalc.BaseStats;
            float spiToSubtract = 0f;
            while (baseSpi == statsAtSubtract.Spirit && spiToSubtract > -2)
            {
                spiToSubtract -= 0.01f;
                statsAtSubtract = GetCharacterStats(character, new Item() { Stats = new Stats() { Spirit = spiToSubtract } + offset });
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
        }

        public static string TimeFormat(double time)
        {
            TimeSpan span = new TimeSpan((long)(Math.Round(time, 2) / 0.0000001));
            return string.Format("{0:0}:{1:00}", span.Minutes, span.Seconds, span.Milliseconds);
        }

        public override void RenderCustomChart(Character character, string chartName, System.Drawing.Graphics g, int width, int height)
        {
            Rectangle rectSubPoint;
            System.Drawing.Drawing2D.LinearGradientBrush brushSubPointFill;
            System.Drawing.Drawing2D.ColorBlend blendSubPoint;

            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;

            Font fontLegend = new Font("Verdana", 10f, GraphicsUnit.Pixel);
            int legendY;

            Brush[] brushSubPoints;
            Color[] colorSubPointsA;
            Color[] colorSubPointsB;

            float graphStart;
            float graphWidth;
            float graphTop;
            float graphBottom;
            float graphHeight;
            float maxScale;
            float dpsScale;
            float graphEnd;
            float[] ticks;
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

            string[] statNames = new string[] { "11.7 Spell Power", "4 Mana per 5 sec", "10 Crit Rating", "10 Haste Rating", "10 Hit Rating", "10 Intellect", "10 Spirit" };
            Color[] statColors = new Color[] { Color.Red, Color.DarkBlue, Color.Orange, Color.Olive, Color.YellowGreen, Color.Aqua, Color.Blue };

            List<float> X = new List<float>();
            List<ComparisonCalculationBase[]> Y = new List<ComparisonCalculationBase[]>();
            Stats baseStats;

            float min;
            float max;

            height -= 2;
            switch (chartName)
            {
                case "Sequence Reconstruction":

                    if (calculationOptions.SequenceReconstruction == null)
                    {
                        g.DrawString("Sequence reconstruction data is not available.", fontLegend, Brushes.Black, 2, 2);
                    }
                    else
                    {
                        #region Legend
                        legendY = 2;

                        Cooldown[] cooldowns = new Cooldown[] { Cooldown.ArcanePower, Cooldown.IcyVeins, Cooldown.MoltenFury, Cooldown.Heroism, Cooldown.PotionOfWildMagic, Cooldown.PotionOfSpeed, Cooldown.FlameCap, Cooldown.Trinket1, Cooldown.Trinket2, Cooldown.Combustion, Cooldown.WaterElemental, Cooldown.ManaGemEffect, Cooldown.PowerInfusion };
						string[] cooldownNames = new string[] { "Arcane Power", "Icy Veins", "Molten Fury", "Heroism", "Potion of Wild Magic", "Potion of Speed", "Flame Cap", (character.Trinket1 != null) ? character.Trinket1.Item.Name : "Trinket 1", (character.Trinket2 != null) ? character.Trinket2.Item.Name : "Trinket 2", "Combustion", "Water Elemental", "Mana Gem Effect", "Power Infusion" };
						Color[] cooldownColors = new Color[] { Color.Azure, Color.DarkBlue, Color.Crimson, Color.Olive, Color.Purple, Color.LemonChiffon, Color.Orange, Color.Aqua, Color.Blue, Color.OrangeRed, Color.DarkCyan, Color.DarkGreen, Color.Yellow };
                        brushSubPoints = new Brush[cooldownColors.Length];
                        colorSubPointsA = new Color[cooldownColors.Length];
                        colorSubPointsB = new Color[cooldownColors.Length];
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

                        if (calculationOptions.AdviseAdvancedSolver)
                        {
                            g.DrawString("Sequence Reconstruction was not fully successful, it is recommended that you enable more options in advanced solver (segment cooldowns, integral mana consumables, advanced constraints options)!", fontLegend, Brushes.Black, new RectangleF(5 + maxWidth, 40, width - maxWidth - 10, 100));
                        }

                        g.DrawLine(Pens.Aqua, new Point(maxWidth + 40, 10), new Point(maxWidth + 80, 10));
                        g.DrawString("Mana", fontLegend, Brushes.Black, new Point(maxWidth + 90, 2));
                        g.DrawLine(Pens.Red, new Point(maxWidth + 40, 26), new Point(maxWidth + 80, 26));
                        g.DrawString("Dps", fontLegend, Brushes.Black, new Point(maxWidth + 90, 18));
                        #endregion

                        #region Graph Ticks
                        graphStart = 20f;
                        graphWidth = width - 40f;
                        graphTop = legendY;
                        graphBottom = height - 4 - 4 * cooldowns.Length;
                        graphHeight = graphBottom - graphTop - 40;
                        maxScale = calculationOptions.FightDuration;
                        graphEnd = graphStart + graphWidth;
                        ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};

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
                        float maxDps = 100;
                        for (int i = 0; i < sequence.Count; i++)
                        {
                            int index = sequence[i].Index;
                            VariableType type = sequence[i].VariableType;
                            float duration = (float)sequence[i].Duration;
                            Cycle cycle = sequence[i].Cycle;
                            if (cycle != null && cycle.DamagePerSecond > maxDps) maxDps = cycle.DamagePerSecond;
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
                                if (sequence[i].IsEvocation)
                                {
                                    switch (sequence[i].VariableType)
                                    {
                                        case VariableType.Evocation:
                                            mps = -(float)calculationOptions.Calculations.EvocationRegen;
                                            break;
                                        case VariableType.EvocationIV:
                                            mps = -(float)calculationOptions.Calculations.EvocationRegenIV;
                                            break;
                                        case VariableType.EvocationHero:
                                            mps = -(float)calculationOptions.Calculations.EvocationRegenHero;
                                            break;
                                        case VariableType.EvocationIVHero:
                                            mps = -(float)calculationOptions.Calculations.EvocationRegenIVHero;
                                            break;
                                    }
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
                            Cycle cycle = sequence[i].Cycle;
                            CastingState state = sequence[i].CastingState;
                            float mps = (float)sequence[i].Mps;
                            if (sequence[i].IsManaPotionOrGem) duration = 0;
                            float dps = 0;
                            if (cycle != null)
                            {
                                dps = cycle.DamagePerSecond;
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
                                if (time - timeOn > 0)
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
                    }
                    break;
                case "Scaling vs Spell Power":

                    #region Legend
                    legendY = 2;

                    brushSubPoints = new Brush[statColors.Length];
                    colorSubPointsA = new Color[statColors.Length];
                    colorSubPointsB = new Color[statColors.Length];
                    for (int i = 0; i < statColors.Length; i++)
                    {
                        Color baseColor = statColors[i];
                        brushSubPoints[i] = new SolidBrush(Color.FromArgb(baseColor.R / 2, baseColor.G / 2, baseColor.B / 2));
                        colorSubPointsA[i] = Color.FromArgb(baseColor.A / 2, baseColor.R / 2, baseColor.G / 2, baseColor.B / 2);
                        colorSubPointsB[i] = Color.FromArgb(baseColor.A / 2, baseColor);
                    }

                    for (int i = 0; i < statNames.Length; i++)
                    {
                        g.DrawLine(new Pen(statColors[i]), new Point(20, legendY + 7), new Point(50, legendY + 7));
                        g.DrawString(statNames[i], fontLegend, Brushes.Black, new Point(60, legendY));

                        legendY += 16;
                    }
                    #endregion

                    #region Graph Ticks
                    graphStart = 20f;
                    graphWidth = width - 40f;
                    graphTop = legendY;
                    graphBottom = height - 5;
                    graphHeight = graphBottom - graphTop - 40;
                    maxScale = 4000;
                    dpsScale = 20;
                    graphEnd = graphStart + graphWidth;
                    ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};

                    for (int i = 0; i <= 10; i++)
                    {
                        float h = (float)Math.Round(graphBottom - graphHeight * i / 10.0);
                        g.DrawLine(black25, graphStart - 4, h, graphEnd, h);
                        //g.DrawLine(black200, graphStart - 4, h, graphStart, h);

                        g.DrawString((i / 10.0 * dpsScale).ToString("0"), fontLegend, black200brush, graphStart - 15, h + 6, formatTick);
                    }

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

                    g.DrawString((0f).ToString("0"), fontLegend, black200brush, graphStart, graphTop + 36, formatTick);
                    g.DrawString((maxScale).ToString("0"), fontLegend, black200brush, graphEnd, graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.5f).ToString("0"), fontLegend, black200brush, ticks[0], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.75f).ToString("0"), fontLegend, black150brush, ticks[1], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.25f).ToString("0"), fontLegend, black150brush, ticks[2], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.125f).ToString("0"), fontLegend, black75brush, ticks[3], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.375f).ToString("0"), fontLegend, black75brush, ticks[4], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.625f).ToString("0"), fontLegend, black75brush, ticks[5], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.875f).ToString("0"), fontLegend, black75brush, ticks[6], graphTop + 36, formatTick);

                    #endregion

                    baseStats = GetCharacterStats(character);
                    g.DrawLine(black200, (float)Math.Round(graphStart + baseStats.SpellPower / maxScale * graphWidth), graphBottom, (float)Math.Round(graphStart + baseStats.SpellPower / maxScale * graphWidth), graphBottom - graphHeight);
                    min = -baseStats.SpellPower;
                    max = maxScale - baseStats.SpellPower;

                    for (float offset = min; offset <= max; offset += 100)
                    {
                        ComparisonCalculationBase[] result = EvaluateItemBudget(character, new Stats() { SpellPower = offset }, true, out baseStats);
                        if (baseStats.SpellPower >= 0 && baseStats.SpellPower <= maxScale)
                        {
                            X.Add(baseStats.SpellPower);
                            Y.Add(result);
                        }
                    }

					g.Clip = new Region(new RectangleF(graphStart, graphTop + 40, graphWidth, graphHeight));
					for (int i = 0; i < statColors.Length; i++)
                    {
                        List<PointF> list = new List<PointF>();
                        for (int j = 0; j < X.Count; j++)
                        {
                            list.Add(new PointF(graphStart + X[j] / maxScale * graphWidth, graphBottom - graphHeight * Y[j][i].OverallPoints / dpsScale));
                        }
                        if (list.Count > 0) g.DrawLines(new Pen(statColors[i]), list.ToArray());
                    }

                    // restore incremental base
                    if (((CalculationOptionsMage)character.CalculationOptions).IncrementalOptimizations)
                    {
                        GetCharacterCalculations(character, null, true, false, calculationOptions.SmartOptimization, false);
                    }

                    break;
                case "Scaling vs Crit Rating":

                    #region Legend
                    legendY = 2;

                    brushSubPoints = new Brush[statColors.Length];
                    colorSubPointsA = new Color[statColors.Length];
                    colorSubPointsB = new Color[statColors.Length];
                    for (int i = 0; i < statColors.Length; i++)
                    {
                        Color baseColor = statColors[i];
                        brushSubPoints[i] = new SolidBrush(Color.FromArgb(baseColor.R / 2, baseColor.G / 2, baseColor.B / 2));
                        colorSubPointsA[i] = Color.FromArgb(baseColor.A / 2, baseColor.R / 2, baseColor.G / 2, baseColor.B / 2);
                        colorSubPointsB[i] = Color.FromArgb(baseColor.A / 2, baseColor);
                    }

                    for (int i = 0; i < statNames.Length; i++)
                    {
                        g.DrawLine(new Pen(statColors[i]), new Point(20, legendY + 7), new Point(50, legendY + 7));
                        g.DrawString(statNames[i], fontLegend, Brushes.Black, new Point(60, legendY));

                        legendY += 16;
                    }
                    #endregion

                    #region Graph Ticks
                    graphStart = 20f;
                    graphWidth = width - 40f;
                    graphTop = legendY;
                    graphBottom = height - 5;
                    graphHeight = graphBottom - graphTop - 40;
                    maxScale = 500;
                    dpsScale = 20;
                    graphEnd = graphStart + graphWidth;
                    ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};

                    for (int i = 0; i <= 10; i++)
                    {
                        float h = (float)Math.Round(graphBottom - graphHeight * i / 10.0);
                        g.DrawLine(black25, graphStart - 4, h, graphEnd, h);
                        //g.DrawLine(black200, graphStart - 4, h, graphStart, h);

                        g.DrawString((i / 10.0 * dpsScale).ToString("0"), fontLegend, black200brush, graphStart - 15, h + 6, formatTick);
                    }

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

                    g.DrawString((0f).ToString("0"), fontLegend, black200brush, graphStart, graphTop + 36, formatTick);
                    g.DrawString((maxScale).ToString("0"), fontLegend, black200brush, graphEnd, graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.5f).ToString("0"), fontLegend, black200brush, ticks[0], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.75f).ToString("0"), fontLegend, black150brush, ticks[1], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.25f).ToString("0"), fontLegend, black150brush, ticks[2], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.125f).ToString("0"), fontLegend, black75brush, ticks[3], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.375f).ToString("0"), fontLegend, black75brush, ticks[4], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.625f).ToString("0"), fontLegend, black75brush, ticks[5], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.875f).ToString("0"), fontLegend, black75brush, ticks[6], graphTop + 36, formatTick);

                    #endregion

                    baseStats = GetCharacterStats(character);
                    g.DrawLine(black200, (float)Math.Round(graphStart + baseStats.CritRating / maxScale * graphWidth), graphBottom, (float)Math.Round(graphStart + baseStats.CritRating / maxScale * graphWidth), graphBottom - graphHeight);
                    min = -baseStats.CritRating;
                    max = maxScale - baseStats.CritRating;

                    for (float offset = min; offset <= max; offset += 50)
                    {
                        ComparisonCalculationBase[] result = EvaluateItemBudget(character, new Stats() { CritRating = offset }, true, out baseStats);
                        if (baseStats.CritRating >= 0 && baseStats.CritRating <= maxScale)
                        {
                            X.Add(baseStats.CritRating);
                            Y.Add(result);
                        }
                    }

					g.Clip = new Region(new RectangleF(graphStart, graphTop + 40, graphWidth, graphHeight));
					for (int i = 0; i < statColors.Length; i++)
                    {
                        List<PointF> list = new List<PointF>();
                        for (int j = 0; j < X.Count; j++)
                        {
                            list.Add(new PointF(graphStart + X[j] / maxScale * graphWidth, graphBottom - graphHeight * Y[j][i].OverallPoints / dpsScale));
                        }
                        if (list.Count > 0) g.DrawLines(new Pen(statColors[i]), list.ToArray());
                    }

                    // restore incremental base
                    if (((CalculationOptionsMage)character.CalculationOptions).IncrementalOptimizations)
                    {
                        GetCharacterCalculations(character, null, true, false, calculationOptions.SmartOptimization, false);
                    }

                    break;
                case "Scaling vs Haste Rating":

                    #region Legend
                    legendY = 2;

                    brushSubPoints = new Brush[statColors.Length];
                    colorSubPointsA = new Color[statColors.Length];
                    colorSubPointsB = new Color[statColors.Length];
                    for (int i = 0; i < statColors.Length; i++)
                    {
                        Color baseColor = statColors[i];
                        brushSubPoints[i] = new SolidBrush(Color.FromArgb(baseColor.R / 2, baseColor.G / 2, baseColor.B / 2));
                        colorSubPointsA[i] = Color.FromArgb(baseColor.A / 2, baseColor.R / 2, baseColor.G / 2, baseColor.B / 2);
                        colorSubPointsB[i] = Color.FromArgb(baseColor.A / 2, baseColor);
                    }

                    for (int i = 0; i < statNames.Length; i++)
                    {
                        g.DrawLine(new Pen(statColors[i]), new Point(20, legendY + 7), new Point(50, legendY + 7));
                        g.DrawString(statNames[i], fontLegend, Brushes.Black, new Point(60, legendY));

                        legendY += 16;
                    }
                    #endregion

                    #region Graph Ticks
                    graphStart = 20f;
                    graphWidth = width - 40f;
                    graphTop = legendY;
                    graphBottom = height - 5;
                    graphHeight = graphBottom - graphTop - 40;
                    maxScale = 1000;
                    dpsScale = 20;
                    graphEnd = graphStart + graphWidth;
                    ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};

                    for (int i = 0; i <= 10; i++)
                    {
                        float h = (float)Math.Round(graphBottom - graphHeight * i / 10.0);
                        g.DrawLine(black25, graphStart - 4, h, graphEnd, h);
                        //g.DrawLine(black200, graphStart - 4, h, graphStart, h);

                        g.DrawString((i / 10.0 * dpsScale).ToString("0"), fontLegend, black200brush, graphStart - 15, h + 6, formatTick);
                    }

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

                    g.DrawString((0f).ToString("0"), fontLegend, black200brush, graphStart, graphTop + 36, formatTick);
                    g.DrawString((maxScale).ToString("0"), fontLegend, black200brush, graphEnd, graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.5f).ToString("0"), fontLegend, black200brush, ticks[0], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.75f).ToString("0"), fontLegend, black150brush, ticks[1], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.25f).ToString("0"), fontLegend, black150brush, ticks[2], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.125f).ToString("0"), fontLegend, black75brush, ticks[3], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.375f).ToString("0"), fontLegend, black75brush, ticks[4], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.625f).ToString("0"), fontLegend, black75brush, ticks[5], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.875f).ToString("0"), fontLegend, black75brush, ticks[6], graphTop + 36, formatTick);

                    #endregion


                    baseStats = GetCharacterStats(character);
                    g.DrawLine(black200, (float)Math.Round(graphStart + baseStats.HasteRating / maxScale * graphWidth), graphBottom, (float)Math.Round(graphStart + baseStats.HasteRating / maxScale * graphWidth), graphBottom - graphHeight);
                    min = -baseStats.HasteRating;
                    max = maxScale - baseStats.HasteRating;

                    for (float offset = min; offset <= max; offset += 50)
                    {
                        ComparisonCalculationBase[] result = EvaluateItemBudget(character, new Stats() { HasteRating = offset }, true, out baseStats);
                        if (baseStats.HasteRating >= 0 && baseStats.HasteRating <= maxScale)
                        {
                            X.Add(baseStats.HasteRating);
                            Y.Add(result);
                        }
                    }

					g.Clip = new Region(new RectangleF(graphStart, graphTop + 40, graphWidth, graphHeight));
					for (int i = 0; i < statColors.Length; i++)
                    {
                        List<PointF> list = new List<PointF>();
                        for (int j = 0; j < X.Count; j++)
                        {
                            list.Add(new PointF(graphStart + X[j] / maxScale * graphWidth, graphBottom - graphHeight * Y[j][i].OverallPoints / dpsScale));
                        }
                        if (list.Count > 0) g.DrawLines(new Pen(statColors[i]), list.ToArray());
                    }

                    // restore incremental base
                    if (((CalculationOptionsMage)character.CalculationOptions).IncrementalOptimizations)
                    {
                        GetCharacterCalculations(character, null, true, false, calculationOptions.SmartOptimization, false);
                    }

                    break;
                case "Scaling vs Intellect":

                    #region Legend
                    legendY = 2;

                    brushSubPoints = new Brush[statColors.Length];
                    colorSubPointsA = new Color[statColors.Length];
                    colorSubPointsB = new Color[statColors.Length];
                    for (int i = 0; i < statColors.Length; i++)
                    {
                        Color baseColor = statColors[i];
                        brushSubPoints[i] = new SolidBrush(Color.FromArgb(baseColor.R / 2, baseColor.G / 2, baseColor.B / 2));
                        colorSubPointsA[i] = Color.FromArgb(baseColor.A / 2, baseColor.R / 2, baseColor.G / 2, baseColor.B / 2);
                        colorSubPointsB[i] = Color.FromArgb(baseColor.A / 2, baseColor);
                    }

                    for (int i = 0; i < statNames.Length; i++)
                    {
                        g.DrawLine(new Pen(statColors[i]), new Point(20, legendY + 7), new Point(50, legendY + 7));
                        g.DrawString(statNames[i], fontLegend, Brushes.Black, new Point(60, legendY));

                        legendY += 16;
                    }
                    #endregion

                    #region Graph Ticks
                    graphStart = 20f;
                    graphWidth = width - 40f;
                    graphTop = legendY;
                    graphBottom = height - 5;
                    graphHeight = graphBottom - graphTop - 40;
                    maxScale = 2000;
                    dpsScale = 20;
                    graphEnd = graphStart + graphWidth;
                    ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};

                    for (int i = 0; i <= 10; i++)
                    {
                        float h = (float)Math.Round(graphBottom - graphHeight * i / 10.0);
                        g.DrawLine(black25, graphStart - 4, h, graphEnd, h);
                        //g.DrawLine(black200, graphStart - 4, h, graphStart, h);

                        g.DrawString((i / 10.0 * dpsScale).ToString("0"), fontLegend, black200brush, graphStart - 15, h + 6, formatTick);
                    }

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

                    g.DrawString((0f).ToString("0"), fontLegend, black200brush, graphStart, graphTop + 36, formatTick);
                    g.DrawString((maxScale).ToString("0"), fontLegend, black200brush, graphEnd, graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.5f).ToString("0"), fontLegend, black200brush, ticks[0], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.75f).ToString("0"), fontLegend, black150brush, ticks[1], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.25f).ToString("0"), fontLegend, black150brush, ticks[2], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.125f).ToString("0"), fontLegend, black75brush, ticks[3], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.375f).ToString("0"), fontLegend, black75brush, ticks[4], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.625f).ToString("0"), fontLegend, black75brush, ticks[5], graphTop + 36, formatTick);
                    g.DrawString((maxScale * 0.875f).ToString("0"), fontLegend, black75brush, ticks[6], graphTop + 36, formatTick);

                    #endregion

                    baseStats = GetCharacterStats(character);
                    g.DrawLine(black200, (float)Math.Round(graphStart + baseStats.Intellect / maxScale * graphWidth), graphBottom, (float)Math.Round(graphStart + baseStats.Intellect / maxScale * graphWidth), graphBottom - graphHeight);
                    min = -baseStats.Intellect;
                    max = maxScale - baseStats.Intellect;

                    for (float offset = min; offset <= max; offset += 100)
                    {
                        ComparisonCalculationBase[] result = EvaluateItemBudget(character, new Stats() { Intellect = offset }, true, out baseStats);
                        if (baseStats.Intellect >= 0 && baseStats.Intellect <= maxScale)
                        {
                            X.Add(baseStats.Intellect);
                            Y.Add(result);
                        }
                    }

					g.Clip = new Region(new RectangleF(graphStart, graphTop + 40, graphWidth, graphHeight));
					for (int i = 0; i < statColors.Length; i++)
                    {
                        List<PointF> list = new List<PointF>();
                        for (int j = 0; j < X.Count; j++)
                        {
                            list.Add(new PointF(graphStart + X[j] / maxScale * graphWidth, graphBottom - graphHeight * Y[j][i].OverallPoints / dpsScale));
                        }
                        if (list.Count > 0) g.DrawLines(new Pen(statColors[i]), list.ToArray());
                    }

                    // restore incremental base
                    if (((CalculationOptionsMage)character.CalculationOptions).IncrementalOptimizations)
                    {
                        GetCharacterCalculations(character, null, true, false, calculationOptions.SmartOptimization, false);
                    }

                    break;
				case "Scaling vs Spirit":

					#region Legend
					legendY = 2;

					brushSubPoints = new Brush[statColors.Length];
					colorSubPointsA = new Color[statColors.Length];
					colorSubPointsB = new Color[statColors.Length];
					for (int i = 0; i < statColors.Length; i++)
					{
						Color baseColor = statColors[i];
						brushSubPoints[i] = new SolidBrush(Color.FromArgb(baseColor.R / 2, baseColor.G / 2, baseColor.B / 2));
						colorSubPointsA[i] = Color.FromArgb(baseColor.A / 2, baseColor.R / 2, baseColor.G / 2, baseColor.B / 2);
						colorSubPointsB[i] = Color.FromArgb(baseColor.A / 2, baseColor);
					}

					for (int i = 0; i < statNames.Length; i++)
					{
						g.DrawLine(new Pen(statColors[i]), new Point(20, legendY + 7), new Point(50, legendY + 7));
						g.DrawString(statNames[i], fontLegend, Brushes.Black, new Point(60, legendY));

						legendY += 16;
					}
					#endregion

					#region Graph Ticks
					graphStart = 20f;
					graphWidth = width - 40f;
					graphTop = legendY;
					graphBottom = height - 5;
					graphHeight = graphBottom - graphTop - 40;
					maxScale = 1000;
					dpsScale = 20;
					graphEnd = graphStart + graphWidth;
					ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};

					for (int i = 0; i <= 10; i++)
					{
						float h = (float)Math.Round(graphBottom - graphHeight * i / 10.0);
						g.DrawLine(black25, graphStart - 4, h, graphEnd, h);
						//g.DrawLine(black200, graphStart - 4, h, graphStart, h);

						g.DrawString((i / 10.0 * dpsScale).ToString("0"), fontLegend, black200brush, graphStart - 15, h + 6, formatTick);
					}

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

					g.DrawString((0f).ToString("0"), fontLegend, black200brush, graphStart, graphTop + 36, formatTick);
					g.DrawString((maxScale).ToString("0"), fontLegend, black200brush, graphEnd, graphTop + 36, formatTick);
					g.DrawString((maxScale * 0.5f).ToString("0"), fontLegend, black200brush, ticks[0], graphTop + 36, formatTick);
					g.DrawString((maxScale * 0.75f).ToString("0"), fontLegend, black150brush, ticks[1], graphTop + 36, formatTick);
					g.DrawString((maxScale * 0.25f).ToString("0"), fontLegend, black150brush, ticks[2], graphTop + 36, formatTick);
					g.DrawString((maxScale * 0.125f).ToString("0"), fontLegend, black75brush, ticks[3], graphTop + 36, formatTick);
					g.DrawString((maxScale * 0.375f).ToString("0"), fontLegend, black75brush, ticks[4], graphTop + 36, formatTick);
					g.DrawString((maxScale * 0.625f).ToString("0"), fontLegend, black75brush, ticks[5], graphTop + 36, formatTick);
					g.DrawString((maxScale * 0.875f).ToString("0"), fontLegend, black75brush, ticks[6], graphTop + 36, formatTick);

					#endregion


					baseStats = GetCharacterStats(character);
					g.DrawLine(black200, (float)Math.Round(graphStart + baseStats.Spirit / maxScale * graphWidth), graphBottom, (float)Math.Round(graphStart + baseStats.Spirit / maxScale * graphWidth), graphBottom - graphHeight);
					min = -baseStats.Spirit;
					max = maxScale - baseStats.Spirit;

					for (float offset = min; offset <= max; offset += 50)
					{
						ComparisonCalculationBase[] result = EvaluateItemBudget(character, new Stats() { Spirit = offset }, true, out baseStats);
						if (baseStats.Spirit >= 0 && baseStats.Spirit <= maxScale)
						{
							X.Add(baseStats.Spirit);
							Y.Add(result);
						}
					}

					g.Clip = new Region(new RectangleF(graphStart, graphTop + 40, graphWidth, graphHeight));
					for (int i = 0; i < statColors.Length; i++)
					{
						List<PointF> list = new List<PointF>();
						for (int j = 0; j < X.Count; j++)
						{
							list.Add(new PointF(graphStart + X[j] / maxScale * graphWidth, graphBottom - graphHeight * Y[j][i].OverallPoints / dpsScale));
						}
						if (list.Count > 0) g.DrawLines(new Pen(statColors[i]), list.ToArray());
					}

					// restore incremental base
					if (((CalculationOptionsMage)character.CalculationOptions).IncrementalOptimizations)
					{
                        GetCharacterCalculations(character, null, true, false, calculationOptions.SmartOptimization, false);
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
                    "Minimum Range",
                    "Threat Reduction",
                    "Arcane Nondps Talents",
                    "Fire Nondps Talents",
                    "Frost Nondps Talents",
                    "Partially Modeled Talents",
					};
                return _optimizableCalculationLabels;
            }
        }

        public override Stats GetRelevantStats(Stats stats)
        {
            Stats s = new Stats()
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
                //SpellPowerFor6SecOnCrit = stats.SpellPowerFor6SecOnCrit,
                EvocationExtension = stats.EvocationExtension,
                BonusMageNukeMultiplier = stats.BonusMageNukeMultiplier,
                LightningCapacitorProc = stats.LightningCapacitorProc,
                //SpellPowerFor20SecOnUse2Min = stats.SpellPowerFor20SecOnUse2Min,
                //HasteRatingFor20SecOnUse2Min = stats.HasteRatingFor20SecOnUse2Min,
                ManaRestoreFromBaseManaPerHit = stats.ManaRestoreFromBaseManaPerHit,
                BonusManaGem = stats.BonusManaGem,
                //SpellPowerFor10SecOnHit_10_45 = stats.SpellPowerFor10SecOnHit_10_45,
                SpellDamageFromIntellectPercentage = stats.SpellDamageFromIntellectPercentage,
                SpellDamageFromSpiritPercentage = stats.SpellDamageFromSpiritPercentage,
                //SpellPowerFor10SecOnResist = stats.SpellPowerFor10SecOnResist,
                //SpellPowerFor15SecOnCrit_20_45 = stats.SpellPowerFor15SecOnCrit_20_45,
                //SpellPowerFor15SecOnUse90Sec = stats.SpellPowerFor15SecOnUse90Sec,
                //SpellHasteFor5SecOnCrit_50 = stats.SpellHasteFor5SecOnCrit_50,
                //SpellHasteFor6SecOnCast_15_45 = stats.SpellHasteFor6SecOnCast_15_45,
                //SpellDamageFor10SecOnHit_5 = stats.SpellDamageFor10SecOnHit_5,
                //SpellHasteFor6SecOnHit_10_45 = stats.SpellHasteFor6SecOnHit_10_45,
                //SpellPowerFor10SecOnCrit_20_45 = stats.SpellPowerFor10SecOnCrit_20_45,
				//SpellPowerFor10SecOnCast_10_45 = stats.SpellPowerFor10SecOnCast_10_45,
				BonusManaPotion = stats.BonusManaPotion,
                ThreatIncreaseMultiplier = stats.ThreatIncreaseMultiplier,
                ThreatReductionMultiplier = stats.ThreatReductionMultiplier,
                //HasteRatingFor20SecOnUse5Min = stats.HasteRatingFor20SecOnUse5Min,
                AldorRegaliaInterruptProtection = stats.AldorRegaliaInterruptProtection,
                //SpellPowerFor15SecOnUse2Min = stats.SpellPowerFor15SecOnUse2Min,
                ShatteredSunAcumenProc = stats.ShatteredSunAcumenProc,
                //ManaRestoreOnCast_5_15 = stats.ManaRestoreOnCast_5_15,
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
                //SpellPowerFor10SecOnCast_15_45 = stats.SpellPowerFor10SecOnCast_15_45,
                //ManaRestoreOnCast_10_45 = stats.ManaRestoreOnCast_10_45,
                //SpellHasteFor10SecOnCast_10_45 = stats.SpellHasteFor10SecOnCast_10_45,
                //ManaRestoreOnCrit_25_45 = stats.ManaRestoreOnCrit_25_45,
                PendulumOfTelluricCurrentsProc = stats.PendulumOfTelluricCurrentsProc,
                ThunderCapacitorProc = stats.ThunderCapacitorProc,
                LightweaveEmbroideryProc = stats.LightweaveEmbroideryProc,
                //SpellPowerFor20SecOnUse5Min = stats.SpellPowerFor20SecOnUse5Min,
				CritBonusDamage = stats.CritBonusDamage,
				BonusDamageMultiplier = stats.BonusDamageMultiplier,
                //SpellPowerFor15SecOnCast_50_45 = stats.SpellPowerFor15SecOnCast_50_45,
                BonusSpellPowerDemonicPactMultiplier = stats.BonusSpellPowerDemonicPactMultiplier,
            };
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.MaxStack == 1)
                {
                    if (effect.Stats.SpellPower > 0)
                    {
                        if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.SpellMiss)
                        {
                            s.AddSpecialEffect(effect);
                            continue;
                        }
                    }
                    if (effect.Stats.HasteRating > 0)
                    {
                        if (effect.Trigger == Trigger.Use)
                        {
                            s.AddSpecialEffect(effect);
                            continue;
                        }
                        if (effect.Cooldown >= effect.Duration && (effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.DamageSpellCast))
                        {
                            s.AddSpecialEffect(effect);
                            continue;
                        }
                        if (effect.Cooldown == 0 && (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellCrit))
                        {
                            s.AddSpecialEffect(effect);
                            continue;
                        }
                    }
                    if (effect.Stats.ManaRestore > 0 || effect.Stats.Mp5 > 0)
                    {
                        if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit)
                        {
                            s.AddSpecialEffect(effect);
                            continue;
                        }
                    }
                    if (effect.Trigger == Trigger.ManaGem)
                    {
                        s.AddSpecialEffect(effect);
                        continue;
                    }
                }
                else if (effect.Chance == 1f && effect.Cooldown == 0f && (effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellHit))
                {
                    if (HasMageStats(effect.Stats))
                    {
                        s.AddSpecialEffect(effect);
                        continue;
                    }
                }
            }
            return s;
        }

        private bool HasMageStats(Stats stats)
        {
            float mageStats = stats.Intellect + stats.Spirit + stats.Mp5 + stats.CritRating + stats.SpellPower + stats.SpellFireDamageRating + stats.HasteRating + stats.HitRating + stats.BonusIntellectMultiplier + stats.BonusSpellCritMultiplier + stats.BonusSpiritMultiplier + stats.SpellFrostDamageRating + stats.SpellArcaneDamageRating + stats.SpellPenetration + stats.Mana + stats.SpellCombatManaRegeneration + stats.BonusArcaneDamageMultiplier + stats.BonusFireDamageMultiplier + stats.BonusFrostDamageMultiplier + stats.ArcaneBlastBonus + stats.EvocationExtension + stats.BonusMageNukeMultiplier + stats.LightningCapacitorProc + stats.ManaRestoreFromBaseManaPerHit + stats.BonusManaGem + stats.SpellDamageFromIntellectPercentage + stats.SpellDamageFromSpiritPercentage + stats.BonusManaPotion + stats.ThreatReductionMultiplier + stats.AllResist + stats.MageAllResist + stats.ArcaneResistance + stats.FireResistance + stats.FrostResistance + stats.NatureResistance + stats.ShadowResistance + stats.AldorRegaliaInterruptProtection + stats.ShatteredSunAcumenProc + stats.InterruptProtection + stats.ArcaneResistanceBuff + stats.FrostResistanceBuff + stats.FireResistanceBuff + stats.NatureResistanceBuff + stats.ShadowResistanceBuff + stats.PVPTrinket + stats.MovementSpeed + stats.Resilience + stats.MageIceArmor + stats.MageMageArmor + stats.MageMoltenArmor + stats.ManaRestoreFromMaxManaPerSecond + stats.SpellCrit + stats.SpellHit + stats.SpellHaste + stats.PendulumOfTelluricCurrentsProc + stats.ThunderCapacitorProc + stats.CritBonusDamage + stats.LightweaveEmbroideryProc + stats.BonusDamageMultiplier + stats.BonusSpellPowerDemonicPactMultiplier;
            return mageStats > 0;
        }

        public override bool HasRelevantStats(Stats stats)
        {
            bool mageStats = HasMageStats(stats);
            float ignoreStats = stats.Agility + stats.Strength + stats.AttackPower + + stats.DefenseRating + stats.Defense + stats.Dodge + stats.Parry + stats.DodgeRating + stats.ParryRating + stats.ExpertiseRating + stats.Expertise + stats.Block + stats.BlockRating + stats.BlockValue + stats.SpellShadowDamageRating + stats.SpellNatureDamageRating;
            foreach (SpecialEffect effect in stats.SpecialEffects())
            {
                if (effect.MaxStack == 1)
                {
                    if (effect.Stats.SpellPower > 0)
                    {
                        if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.SpellMiss)
                        {
                            return true;
                        }
                    }
                    if (effect.Stats.HasteRating > 0)
                    {
                        if (effect.Trigger == Trigger.Use)
                        {
                            return true;
                        }
                        if (effect.Cooldown >= effect.Duration && (effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.DamageSpellCast))
                        {
                            return true;
                        }
                        if (effect.Cooldown == 0 && (effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.DamageSpellCrit))
                        {
                            return true;
                        }
                    }
                    if (effect.Stats.ManaRestore > 0 || effect.Stats.Mp5 > 0)
                    {
                        if (effect.Trigger == Trigger.Use || effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.DamageSpellCrit || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellCrit || effect.Trigger == Trigger.SpellHit)
                        {
                            return true;
                        }
                    }
                    if (effect.Trigger == Trigger.ManaGem)
                    {
                        return true;
                    }
                }
                else if (effect.Chance == 1f && effect.Cooldown == 0f && (effect.Trigger == Trigger.DamageSpellCast || effect.Trigger == Trigger.DamageSpellHit || effect.Trigger == Trigger.SpellCast || effect.Trigger == Trigger.SpellHit))
                {
                    if (HasMageStats(effect.Stats))
                    {
                        return true;
                    }
                }
            }
            return (mageStats || ((stats.Health + stats.Stamina + stats.Armor) > 0 && ignoreStats == 0.0f));
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, Item.ItemSlot slot)
        {
            if (slot == Item.ItemSlot.OffHand || slot == Item.ItemSlot.Ranged) return false;
            return base.EnchantFitsInSlot(enchant, character, slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, Character.CharacterSlot slot)
        {
            if (slot == Character.CharacterSlot.OffHand && item.Slot == Item.ItemSlot.OneHand) return false;
            return base.ItemFitsInSlot(item, character, slot);
        }
    }
}
