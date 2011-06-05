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

namespace Rawr.DPSDK
{
    public partial class CalculationOptionsPanelDPSDK : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelDPSDK()
        {
            InitializeComponent();
        }

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
            if (_loadingCalculationOptions) { return; }
        }

        protected void BT_StatsGraph_Click(object sender, RoutedEventArgs e)
        {
            Stats[] statsList = BuildStatsList();
            StatGraphWindow gw = new StatGraphWindow();
            string explanatoryText = "This graph shows how adding or subtracting\nmultiples of a stat affects your dps.\n\nAt the Zero position is your current dps.\n" +
                         "To the right of the zero vertical is adding stats.\nTo the left of the zero vertical is subtracting stats.\n" +
                         "The vertical axis shows the amount of dps added or lost";
            gw.GetGraph.SetupStatsGraph(Character, statsList, calcOpts.StatsIncrement, explanatoryText, calcOpts.CalculationToGraph);
            gw.Show();
        }

        #region ICalculationOptionsPanel Members
        public UserControl PanelControl { get { return this; } }

        CalculationOptionsDPSDK calcOpts = null;

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsDPSDK)
                    calcOpts.PropertyChanged -= new PropertyChangedEventHandler(CalculationOptionsPanelDPSDK_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelDPSDK_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelDPSDK_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsDPSDK();
            calcOpts = Character.CalculationOptions as CalculationOptionsDPSDK;
            // Model Specific Code
            //
            // Bad Gear Hiding
            CalculationsDPSDK.HidingBadStuff_Def = calcOpts.HideBadItems_Def;
            CalculationsDPSDK.HidingBadStuff_Spl = calcOpts.HideBadItems_Spl;
            CalculationsDPSDK.HidingBadStuff_PvP = calcOpts.HideBadItems_PvP;
            ItemCache.OnItemsChanged();
            CalculationOptionsPanelDPSDK_PropertyChanged(null, null);
            _loadingCalculationOptions = false;
        }

        void CalculationOptionsPanelDPSDK_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }
            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            if (e.PropertyName.Contains("SG_") || e.PropertyName == "szRotReport")
            {
                // Don't want to invalidate Calcs just for the RotationReport:
                return;
            }
            //
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }

        void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }
        #endregion
    }
}
