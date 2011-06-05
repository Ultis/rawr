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

namespace Rawr.Retribution
{
    public partial class CalculationOptionsPanelRetribution : ICalculationOptionsPanel
    {
        public bool _loadingCalculationOptions = false;

        public UserControl PanelControl { get { return this; } }

        #region ICalculationOptionsPanel Members
        CalculationOptionsRetribution calcOpts = null;

        private Character character;
        public Character Character
        {
            get { return character; }
            set {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsRetribution)
                    ((CalculationOptionsRetribution)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(CalculationOptionsRetribution_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsRetribution_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsRetribution_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        public CalculationOptionsPanelRetribution() {
            _loadingCalculationOptions = true;
            try {
                InitializeComponent();
                // Model Specific Code
                // n/a
            } catch (Exception ex) {
                new Base.ErrorBox() {
                    Title = "Error in creating the Retribution Options Pane",
                    TheException = ex,
                    Function = "CalculationOptionsPanelRetribution()",
                }.Show();
            }
            _loadingCalculationOptions = false;
        }
        public void LoadCalculationOptions()
        {
            string info = "";
            _loadingCalculationOptions = true;
            try {
                if (Character != null && Character.CalculationOptions == null)
                {
                    // If it's broke, make a new one with the defaults
                    Character.CalculationOptions = new CalculationOptionsRetribution();
                    _loadingCalculationOptions = true;
                }
                else if (Character == null) { return; }
                calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                // == Model Specific Code ==
            } catch (Exception ex) {
                new Base.ErrorBox() {
                    Title = "Error in loading the Retribution Options Pane",
                    Function = "LoadCalculationOptions()",
                    TheException = ex,
                    Info = info,
                }.Show();
            }
            _loadingCalculationOptions = false;
        }
        void CalculationOptionsRetribution_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }
            if (e == null) { e = new PropertyChangedEventArgs(""); }
            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            if (e.PropertyName.Contains("SG_")
                || e.PropertyName == "StatsList"
                || e.PropertyName == "StatsIncrement"
                || e.PropertyName == "CalculationToGraph")
            {
                // These fields shouldn't be forcing new calcs on each click
                return;
            }
            //
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        #endregion

        #region Stats Graph
        protected Stats[] BuildStatsList()
        {
            List<Stats> statsList = new List<Stats>();
            if (CK_Stats_0.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Strength = 1f }); }
            if (CK_Stats_1.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Agility = 1f }); }
            if (CK_Stats_2.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { AttackPower = 2f }); }
            if (CK_Stats_3.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { CritRating = 1f }); }
            if (CK_Stats_4.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { HitRating = 1f }); }
            if (CK_Stats_5.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { ExpertiseRating = 1f }); }
            if (CK_Stats_6.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { HasteRating = 1f }); }
            if (CK_Stats_7.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { MasteryRating = 1f }); }
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
