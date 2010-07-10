using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace Rawr.DPSDK
{
    public partial class CalculationOptionsPanelDPSDK : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelDPSDK()
        {
            InitializeComponent();
        }

        #region ICalculationOptionsPanel Members
        public UserControl PanelControl { get { return this; } }

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                // Kill any old event connections
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsDPSDK)
                    ((CalculationOptionsDPSDK)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(CalculationOptionsPanelDPSDK_PropertyChanged);
                // Apply the new character
                character = value;
                // Load the new CalcOpts
                LoadCalculationOptions();
                // Model Specific Code
                // Set the Data Context
                //LayoutRoot.DataContext = (Character.CalculationOptions as CalculationOptionsDPSDK);
                RotationTab.DataContext = (Character.CalculationOptions as CalculationOptionsDPSDK).rotation;
                OptionsTab.DataContext = (Character.CalculationOptions as CalculationOptionsDPSDK);
                // Add new event connections
                (Character.CalculationOptions as CalculationOptionsDPSDK).PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelDPSDK_PropertyChanged);
                // Run it once for any special UI config checks
                //CalculationOptionsPanelDPSDK_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsDPSDK();
            // Model Specific Code
            //
            _loadingCalculationOptions = false;
        }

        void CalculationOptionsPanelDPSDK_PropertyChanged(object sender, PropertyChangedEventArgs e)
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

        void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }
        #endregion
    }
}
