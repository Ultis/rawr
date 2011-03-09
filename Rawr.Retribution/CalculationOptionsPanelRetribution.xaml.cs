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
                    ((CalculationOptionsRetribution)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsRetribution_PropertyChanged);
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

    }
}
