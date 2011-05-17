using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.ProtPaladin
{
    public partial class CalculationOptionsPanelProtPaladin : UserControl, ICalculationOptionsPanel
    {
        #region Initialization

        public CalculationOptionsPanelProtPaladin()
        {
            InitializeComponent();
        }

        #endregion

        #region ICalculationOptionsPanel
        public UserControl PanelControl { get { return this; } }

        private CalculationOptionsProtPaladin calcOpts;
        
        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsProtPaladin)
                    ((CalculationOptionsProtPaladin)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelProtPaladin_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                LayoutRoot.DataContext = calcOpts;
                // Add new event connections
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelProtPaladin_PropertyChanged);
                // Run it once for any special UI config checks
                CalculationOptionsPanelProtPaladin_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsProtPaladin();
            calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
            // Model Specific Code
            //
            _loadingCalculationOptions = false;
        }

        void CalculationOptionsPanelProtPaladin_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_loadingCalculationOptions) { return; }
            // This would handle any special changes, especially combobox assignments, but not when the pane is trying to load
            if (e.PropertyName == "SomeProperty")
            {
                // Do some code
            }
            //
            if (Character != null) { Character.OnCalculationsInvalidated(); }
        }
        #endregion

        #region Events
        private void cboRankingMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = CB_RankingMode.Items.IndexOf(e.AddedItems[0]);

            // Only enable threat scale for RankingModes other than 2
            /*if (btnResetThreatScale != null && silThreatScale != null)
                btnResetThreatScale.IsEnabled = silThreatScale.IsEnabled = (selectedIndex != 2);*/

            if (calcOpts == null) return; // can be null while loading xaml in WPF

            // Set the default ThreatScale
            if (selectedIndex == 2)
                calcOpts.ThreatScale = 0f;
            else
                calcOpts.ThreatScale = 10f;
        }
        private void btnResetHitsToSurvive_Click(object sender, RoutedEventArgs e) { calcOpts.HitsToSurvive = 3.5f; }
        private void btnResetBurstScale_Click(object sender, RoutedEventArgs e) { calcOpts.BurstScale = 3.0f; }
        private void btnResetThreatScale_Click(object sender, RoutedEventArgs e) { calcOpts.ThreatScale = 5f; }
        #endregion
    }
}
