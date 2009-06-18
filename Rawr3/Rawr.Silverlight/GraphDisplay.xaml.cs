using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Reflection;

namespace Rawr.Silverlight
{
    public partial class GraphDisplay : UserControl
    {

        public enum Graph : int
        {   Gear = 0,
            Enchants = 1,
            Gems = 2,
            Buffs = 3,
            TalentSpecs = 5,
            Talents = 6,
            Glyphs = 7,
            RelativeStatValue = 8,
            Custom = 10
        }

        #region Getters/Setters
        private Graph currentGraph;
        public Graph CurrentGraph
        {
            get { return currentGraph; }
            set
            {
                currentGraph = value;
                UpdateGraph();
            }
        }

        private Character.CharacterSlot gear;
        public Character.CharacterSlot Gear
        {
            get { return gear; }
            set
            {
                gear = value;
                UpdateGraph();
            }
        }

        private Item.ItemSlot enchant;
        public Item.ItemSlot Enchant
        {
            get { return enchant; }
            set
            {
                enchant = value;
                UpdateGraph();
            }
        }

        private Character.CharacterSlot gem;
        public Character.CharacterSlot Gem
        {
            get { return gem; }
            set
            {
                gem = value;
                UpdateGraph();
            }
        }

        private bool allBuffs;
        public bool AllBuffs
        {
            get { return allBuffs; }
            set
            {
                allBuffs = value;
                UpdateGraph();
            }
        }

        private int custom;
        public int Custom
        {
            get { return custom; }
            set
            {
                custom = value;
                UpdateGraph();
            }
        }

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                if (character != null)
                {
                    character.CalculationsInvalidated -= new EventHandler(character_CalculationsInvalidated);
                }
                character = value;
                character.CalculationsInvalidated += new EventHandler(character_CalculationsInvalidated);
                ComparisonGraph.Character = character;
				CustomCombo.ItemsSource = new List<string>(Calculations.CustomChartNames);
				CustomCombo.SelectedIndex = Calculations.CustomChartNames.Length > 0 ? 0 : -1;

				UpdateGraph();
            }
        }
        #endregion

        public void character_CalculationsInvalidated(object sender, EventArgs e)
        {
            UpdateGraph();
        }

        private bool loading;
        public GraphDisplay()
        {
            loading = true;
            // Required to initialize variables
            InitializeComponent();
            GraphCombo.SelectedIndex = (int)Graph.RelativeStatValue;
            GearCombo.SelectedIndex = (int)Character.CharacterSlot.Head;
            EnchantCombo.SelectedIndex = (int)Character.CharacterSlot.Head;
            GemCombo.SelectedIndex = 0;
            BuffCombo.SelectedIndex = 0;
            CustomCombo.SelectedIndex = -1;
            loading = false;
            UpdateGraph();
        }

        public void UpdateGraph()
        {
            if (loading || Calculations.Instance == null || Character == null) return;

            GearCombo.Visibility = Visibility.Collapsed;
            EnchantCombo.Visibility = Visibility.Collapsed;
            GemCombo.Visibility = Visibility.Collapsed;
            BuffCombo.Visibility = Visibility.Collapsed;
            CustomCombo.Visibility = Visibility.Collapsed;

            if (CurrentGraph == Graph.Gear)
            {
                GearCombo.Visibility = Visibility.Visible;
                bool seenEquippedItem = (Character[Gear] == null);

                Calculations.ClearCache();
                List<ItemInstance> relevantItemInstances = Character.GetRelevantItemInstances(Gear);
                ComparisonCalculationBase[] itemCalculations = new ComparisonCalculationBase[relevantItemInstances.Count + 1];

                int index = 0;
                if (relevantItemInstances.Count > 0)
                {
                    foreach (ItemInstance item in relevantItemInstances)
                    {
                        if (!seenEquippedItem && Character[Gear].Equals(item)) seenEquippedItem = true;
                        itemCalculations[index++] = Calculations.GetItemCalculations(item, Character, Gear);
                    }
                }
                if (!seenEquippedItem) itemCalculations[index++] = Calculations.GetItemCalculations(Character[Gear], Character, Gear);

                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.DisplayCalcs(itemCalculations);
            }
            else if (CurrentGraph == Graph.Enchants)
            {
                EnchantCombo.Visibility = Visibility.Visible;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.DisplayCalcs(Calculations.GetEnchantCalculations(Enchant, Character, Calculations.GetCharacterCalculations(Character)).ToArray());
            }
            else if (CurrentGraph == Graph.Gems)
            {
                GemCombo.Visibility = Visibility.Visible;

                Calculations.ClearCache();
                List<Item> relevantItems = Character.GetRelevantItems(Gem);
                ComparisonCalculationBase[] itemCalculations = new ComparisonCalculationBase[relevantItems.Count];

                int index = 0;
                if (relevantItems.Count > 0)
                {
                    foreach (Item item in relevantItems)
                    {
                        itemCalculations[index++] = Calculations.GetItemCalculations(item, Character, Gem);
                    }
                }

                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.DisplayCalcs(itemCalculations);
            }
            else if (CurrentGraph == Graph.Buffs)
            {
                BuffCombo.Visibility = Visibility.Visible;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.DisplayCalcs(Calculations.GetBuffCalculations(Character, Calculations.GetCharacterCalculations(Character), !AllBuffs).ToArray());
            }
            else if (CurrentGraph == Graph.Talents)
            {
                List<ComparisonCalculationBase> talentCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();
                CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false);;
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;

                foreach (PropertyInfo pi in Character.CurrentTalents.GetType().GetProperties())
                {
                    TalentDataAttribute[] talentDatas = pi.GetCustomAttributes(typeof(TalentDataAttribute), true) as TalentDataAttribute[];
                    int orig;
                    if (talentDatas.Length > 0)
                    {
                        TalentDataAttribute talentData = talentDatas[0];
                        orig = Character.CurrentTalents.Data[talentData.Index];
                        if (talentData.MaxPoints == (int)pi.GetValue(Character.CurrentTalents, null))
                        {
                            newChar.CurrentTalents.Data[talentData.Index]--;
                            newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                            compare = Calculations.GetCharacterComparisonCalculations(newCalc, currentCalc, talentData.Name, talentData.MaxPoints == orig);
                        }
                        else
                        {
                            newChar.CurrentTalents.Data[talentData.Index]++;
                            newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                            compare = Calculations.GetCharacterComparisonCalculations(currentCalc, newCalc, talentData.Name, talentData.MaxPoints == orig);
                        }
                        compare.Item = null;
                        talentCalculations.Add(compare);
                        newChar.CurrentTalents.Data[talentData.Index] = orig;
                    }
                }
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.DisplayCalcs(talentCalculations.ToArray());
            }
            else if (CurrentGraph == Graph.TalentSpecs)
            {
                List<ComparisonCalculationBase> talentCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();

                TalentsBase nothing = Character.CurrentTalents.Clone();
                for (int i = 0; i < nothing.Data.Length; i++) nothing.Data[i] = 0;
                for (int i = 0; i < nothing.GlyphData.Length; i++) nothing.GlyphData[i] = false;
                newChar.CurrentTalents = nothing;

                CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, true);
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;

                bool same, found = false;
                foreach (SavedTalentSpec sts in SavedTalentSpec.SpecsFor(Character.Class))
                {
                    same = false;
                    if (sts.Equals(Character.CurrentTalents)) same = true;
                    newChar.CurrentTalents = sts.TalentSpec();
                    newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, true);
                    compare = Calculations.GetCharacterComparisonCalculations(baseCalc, newCalc, sts.Name, same);
                    compare.Item = null;
                    talentCalculations.Add(compare);
                    found = found || same;
                }
                if (!found)
                {
                    newCalc = Calculations.GetCharacterCalculations(Character, null, false, true, true);
                    compare = Calculations.GetCharacterComparisonCalculations(baseCalc, newCalc, "Custom", true);
                    talentCalculations.Add(compare);
                }
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.DisplayCalcs(talentCalculations.ToArray());
            }
            else if (CurrentGraph == Graph.Glyphs)
            {
                List<ComparisonCalculationBase> glyphCalculations = new List<ComparisonCalculationBase>();
                Character newChar = Character.Clone();
                CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(Character, null, false, true, false);
                CharacterCalculationsBase newCalc;
                ComparisonCalculationBase compare;
                List<string> relevant = Calculations.GetModel(Character.CurrentModel).GetRelevantGlyphs();

                foreach (PropertyInfo pi in Character.CurrentTalents.GetType().GetProperties())
                {
                    GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                    bool orig;
                    if (glyphDatas.Length > 0)
                    {
                        GlyphDataAttribute glyphData = glyphDatas[0];
                        if (relevant == null || relevant.Contains(glyphData.Name))
                        {
                            orig = Character.CurrentTalents.GlyphData[glyphData.Index];
                            if (orig)
                            {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = false;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(newCalc, currentCalc, glyphData.Name, orig);
                            }
                            else
                            {
                                newChar.CurrentTalents.GlyphData[glyphData.Index] = true;
                                newCalc = Calculations.GetCharacterCalculations(newChar, null, false, true, false);
                                compare = Calculations.GetCharacterComparisonCalculations(currentCalc, newCalc, glyphData.Name, orig);
                            }
                            compare.Item = null;
                            glyphCalculations.Add(compare);
                            newChar.CurrentTalents.GlyphData[glyphData.Index] = orig;
                        }
                    }
                }
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.DisplayCalcs(glyphCalculations.ToArray());
            }
            else if (CurrentGraph == Graph.RelativeStatValue)
            {
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                ComparisonGraph.DisplayCalcs(CalculationsBase.GetRelativeStatValues(Character));
            }
            else if (CurrentGraph == Graph.Custom)
            {
                CustomCombo.Visibility = Visibility.Visible;
                ComparisonGraph.LegendItems = Calculations.SubPointNameColors;
                if (Custom < 0) ComparisonGraph.DisplayCalcs(new ComparisonCalculationBase[0]);
                else ComparisonGraph.DisplayCalcs(Calculations.GetCustomChartData(Character, Calculations.CustomChartNames[Custom]));
            }
        }

        private void SetCustomSubpoints(IEnumerable<string> subpoints)
        {

        }

        private void SortChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortCombo != null)
                ComparisonGraph.Sort = (ComparisonSort)(SortCombo.SelectedIndex - 2);
        }

        private void GraphChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CurrentGraph = (Graph)GraphCombo.SelectedIndex;
        }

        private void GearChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Gear = (Character.CharacterSlot)GearCombo.SelectedIndex;
        }

        private void EnchantChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Enchant = (Item.ItemSlot)EnchantCombo.SelectedIndex;
        }

        private void GemChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Gem = GemCombo.SelectedIndex == 0 ? Character.CharacterSlot.Metas : Character.CharacterSlot.Gems;
        }

        private void BuffChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            AllBuffs = BuffCombo.SelectedIndex == 0;
        }

        private void CustomChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Custom = CustomCombo.SelectedIndex;
        }

    }
}