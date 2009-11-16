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

namespace Rawr.TankDK
{
    public partial class CalculationOptionsPanelTankDK : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelTankDK()
        {
            InitializeComponent();
            this.LayoutRoot.SelectAll();
        }
        public UserControl PanelControl { get { return this; } }

        private Character character;
        public Character Character
        {
            get 
            { 
                return character; 
            }
            set
            {
                character = value;
                LoadCalculationOptions();

                ((CalculationOptionsTankDK)character.CalculationOptions).PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelTankDK_PropertyChanged);

            }
        }

        private bool _loadingCalculationOptions;
        public void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null)
            {
                Character.CalculationOptions = new CalculationOptionsTankDK();
            }
            DataContext = Character.CalculationOptions as CharacterCalculationsTankDK;
            _loadingCalculationOptions = false;
        }

        void ButtonClickHandler(object sender, RoutedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }

        void CalculationOptionsPanelTankDK_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
