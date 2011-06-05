using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Rawr.HealPriest
{
    public partial class CalculationOptionsPanelHealPriest : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelHealPriest()
        {
            InitializeComponent();
        }

        #region ICalculationOptionsPanel Members
        public UserControl PanelControl { get { return this; } }

        CalculationOptionsHealPriest calcOpts = null;

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsHealPriest)
                    ((CalculationOptionsHealPriest)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelHealPriest_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelHealPriest_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelHealPriest_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsHealPriest();
            calcOpts = Character.CalculationOptions as CalculationOptionsHealPriest;
            // Model Specific Code
            //
            cbRotation.ItemsSource = PriestModels.Models;
            cbRotation.SelectedItem = calcOpts.Model;
            _loadingCalculationOptions = false;
            CalculationOptionsPanelHealPriest_PropertyChanged(null, new PropertyChangedEventArgs("Model"));
        }

        void CalculationOptionsPanelHealPriest_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }
            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            if (e.PropertyName == "Model") {
                // Do some code
                bool isCustom = false; //calcOpts. == eRole.CUSTOM;
                numFlashHealCast.IsEnabled = isCustom;
                numBindingHealCast.IsEnabled = isCustom;
                numGreaterHealCast.IsEnabled = isCustom;
                numPenanceCast.IsEnabled = isCustom;
                numRenewCast.IsEnabled = isCustom; numRenewTicks.IsEnabled = isCustom;
                numProMCast.IsEnabled = isCustom; numProMTicks.IsEnabled = isCustom;
                numPoHCast.IsEnabled = isCustom;
                numPWSCast.IsEnabled = isCustom;
                numCoHCast.IsEnabled = isCustom;
                numHolyNovaCast.IsEnabled = isCustom;
                numDivineHymnCast.IsEnabled = isCustom;
                numDispelCast.IsEnabled = isCustom;
                numMDCast.IsEnabled = isCustom;
            }
            //
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        #endregion

        #region Stats Graph
        protected Stats[] BuildStatsList()
        {
            List<Stats> statsList = new List<Stats>();
            if (CK_Stats_0.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Intellect = 1f }); }
            if (CK_Stats_1.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Spirit = 1f }); }
            if (CK_Stats_2.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { SpellPower = 2f }); }
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
            if (_loadingCalculationOptions || calcOpts == null) { return; }
            calcOpts.CalculationToGraph = (string)CB_CalculationToGraph.SelectedItem;
        }

        protected void BT_StatsGraph_Click(object sender, RoutedEventArgs e)
        {
            Stats[] statsList = BuildStatsList();
            StatGraphWindow gw = new StatGraphWindow();
            string explanatoryText = "This graph shows how adding or subtracting\nmultiples of a stat affects your score.\n\nAt the Zero position is your current score.\n" +
                         "To the right of the zero vertical is adding stats.\nTo the left of the zero vertical is subtracting stats.\n" +
                         "The vertical axis shows the amount of score added or lost";
            gw.GetGraph.SetupStatsGraph(Character, statsList, calcOpts.StatsIncrement, explanatoryText, calcOpts.CalculationToGraph);
            gw.Show();
        }
        #endregion
    }
}
