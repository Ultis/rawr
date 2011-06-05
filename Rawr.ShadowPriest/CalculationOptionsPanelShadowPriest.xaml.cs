using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Rawr.ShadowPriest.Spells;

namespace Rawr.ShadowPriest
{
    /// <summary>
    /// The Options Panel for the shadow priest model.
    /// </summary>
    public partial class CalculationOptionsPanelShadowPriest : UserControl, ICalculationOptionsPanel
    {
        private readonly CalculationOptionsShadowPriest _calculationOptions;

        public CalculationOptionsPanelShadowPriest(CalculationOptionsShadowPriest calculationOptions)
        {
            _calculationOptions = calculationOptions;

            InitializeComponent();

                        // Set about text
            tbModuleNotes.Text =
                "Notes:\r\n" +
               "Shadow Priest Notes";
            /*
            SpellBox spellBox = new SpellBox();

            
            foreach (Spell s in spellBox.Spells)
            {
                lsSpellPriority.Items.Add(s.name);
            }
            */
        }
        public UserControl PanelControl { get { return this; } }

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {

                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsShadowPriest)
                    ((CalculationOptionsShadowPriest)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsShadowPriest_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = _calculationOptions;
                // Add new event connections
                _calculationOptions.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsShadowPriest_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsShadowPriest_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;

        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            
            if (Character.CalculationOptions == null) 
                Character.CalculationOptions = _calculationOptions;
            
            _loadingCalculationOptions = false;
        }

        void CalculationOptionsShadowPriest_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }
            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            if (e.PropertyName.Contains("SP_")) { return; } // Don't trigger recalc for these settings changes
            //
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        /*
        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSpells.SelectedItem == null)
                return;

            lsSpellPriority.Items.Add(cmbSpells.SelectedItem);
            cmbSpells.Items.RemoveAt(cmbSpells.SelectedIndex);
            cmbSpells.SelectedItem = "";
        }

        private void bUp_Click(object sender, RoutedEventArgs e)
        {
            if (lsSpellPriority.SelectedItem == null || lsSpellPriority.SelectedIndex == 0)
                return;

            lsSpellPriority.Items.Insert(lsSpellPriority.SelectedIndex - 1, lsSpellPriority.SelectedItem);
            lsSpellPriority.Items.RemoveAt(lsSpellPriority.SelectedIndex);
        }

        private void bDown_Click(object sender, RoutedEventArgs e)
        {
            if (lsSpellPriority.SelectedItem == null || lsSpellPriority.SelectedIndex == lsSpellPriority.Items.Count - 1)
                return;

            lsSpellPriority.Items.Insert(lsSpellPriority.SelectedIndex + 2, lsSpellPriority.SelectedItem);
            lsSpellPriority.Items.RemoveAt(lsSpellPriority.SelectedIndex);
        }

        private void bRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lsSpellPriority.SelectedItem == null)
                return;

            cmbSpells.Items.Add(lsSpellPriority.SelectedItem);
            lsSpellPriority.Items.RemoveAt(lsSpellPriority.SelectedIndex);
        }

        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            if (lsSpellPriority.Items.Count == 0)
                return;

            foreach (Object o in lsSpellPriority.Items)
                cmbSpells.Items.Add(o);

            lsSpellPriority.Items.Clear();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            character.OnCalculationsInvalidated();
        }
        */

        #region Stats Graph
        protected Stats[] BuildStatsList()
        {
            List<Stats> statsList = new List<Stats>();
            if (CK_Stats_0.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { SpellPower = 1f }); }
            if (CK_Stats_1.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Intellect = 1f }); }
            if (CK_Stats_2.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Spirit = 1f }); }
            if (CK_Stats_3.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { CritRating = 1f }); }
            if (CK_Stats_4.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { HitRating = 1f }); }
            if (CK_Stats_5.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { ExpertiseRating = 1f }); }
            if (CK_Stats_6.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { HasteRating = 1f }); }
            if (CK_Stats_7.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { MasteryRating = 1f }); }
            if (CK_Stats_8.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { SpellPenetration = 1f }); }
            return statsList.ToArray();
        }
        private void CB_CalculationToGraph_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_loadingCalculationOptions || _calculationOptions == null) { return; }
            _calculationOptions.CalculationToGraph = (string)CB_CalculationToGraph.SelectedItem;
        }

        protected void BT_StatsGraph_Click(object sender, RoutedEventArgs e)
        {
            Stats[] statsList = BuildStatsList();
            StatGraphWindow gw = new StatGraphWindow();
            string explanatoryText = "This graph shows how adding or subtracting\nmultiples of a stat affects your score.\n\nAt the Zero position is your current score.\n" +
                         "To the right of the zero vertical is adding stats.\nTo the left of the zero vertical is subtracting stats.\n" +
                         "The vertical axis shows the amount of score added or lost";
            gw.GetGraph.SetupStatsGraph(Character, statsList, _calculationOptions.StatsIncrement, explanatoryText, _calculationOptions.CalculationToGraph);
            gw.Show();
        }
        #endregion
    }
}
