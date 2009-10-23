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
        public UserControl PanelControl { get { return this; } }

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                if (character != null && character.CalculationOptions != null && character.CalculationOptions is CalculationOptionsDPSDK)
                {
               //     ((CalculationOptionsDPSDK)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(CalculationOptionsPanelDPSDK_PropertyChanged);
                }

                character = value;
                //if (character.CalculationOptions == null) character.CalculationOptions = new CalculationOptionsDPSDK();
                //LayoutRoot.DataContext = Character.CalculationOptions;
                LoadCalculationOptions();

                RotationTab.DataContext = (Character.CalculationOptions as CalculationOptionsDPSDK).rotation;

                ((CalculationOptionsDPSDK)character.CalculationOptions).PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelDPSDK_PropertyChanged);

            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsDPSDK();

            _loadingCalculationOptions = false;
        }

        void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }

        void CalculationOptionsPanelDPSDK_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "talents" && e.PropertyName != "Presence")
            {
                MessageBox.Show(e.PropertyName + " ; " + e.ToString());
                character.OnCalculationsInvalidated();
            }
           //Character.OnCalculationsInvalidated();
        }
    }
}
