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

namespace Rawr.TankDK
{
    public partial class CalculationOptionsPanelTankDK : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelTankDK()
        {
            InitializeComponent();
        }

        public UserControl PanelControl { get { return this; } }

        CalculationOptionsTankDK calcOpts = null;

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsTankDK)
                    ((CalculationOptionsTankDK)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelTankDK_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelTankDK_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelTankDK_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsTankDK();
            calcOpts = Character.CalculationOptions as CalculationOptionsTankDK;
            // Model Specific Code
            //
            _loadingCalculationOptions = false;
        }

        void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }

        void CalculationOptionsPanelTankDK_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }
            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            if (e.PropertyName == "szRotReport" || e.PropertyName == "SG_")
            {
                // Don't want to invalidate Calcs just for the RotationReport:
                return;
            }
            //
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        private void BT_HitsToSurvive_Click(object sender, RoutedEventArgs e) { calcOpts.HitsToSurvive = 3.5f; }
        private void BT_BurstScale_Click(object sender, RoutedEventArgs e) { calcOpts.BurstWeight = 3.0f; }
        private void BT_ThreatScale_Click(object sender, RoutedEventArgs e) { calcOpts.ThreatWeight = 1.0f; }
        private void BT_VengeanceScale_Click(object sender, RoutedEventArgs e) { calcOpts.VengeanceWeight = 1.0f; }
        private void BT_Overhealing_Click(object sender, RoutedEventArgs e) { calcOpts.pOverHealing = 0.25f; }

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
            if (CK_Stats_8.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { DodgeRating = 1f }); }
            if (CK_Stats_9.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { ParryRating = 1f }); }
            if (CK_Stats_10.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Armor = 1f }); }
            if (CK_Stats_11.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { BonusArmor = 1f }); }
            if (CK_Stats_12.IsChecked.GetValueOrDefault(true)) { statsList.Add(new Stats() { Stamina = 1f }); }
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
    }
}
