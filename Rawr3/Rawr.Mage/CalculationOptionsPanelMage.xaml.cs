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

namespace Rawr.Mage
{
    public partial class CalculationOptionsPanelMage : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelMage()
        {
            InitializeComponent();
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
                if (character != null && character.CalculationOptions != null && character.CalculationOptions is CalculationOptionsMage)
                {
                    ((CalculationOptionsMage)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(CalculationOptionsPanelMage_PropertyChanged);
                }

                character = value;
                if (character.CalculationOptions == null) character.CalculationOptions = new CalculationOptionsMage(character);
                LayoutRoot.DataContext = Character.CalculationOptions;

                ((CalculationOptionsMage)character.CalculationOptions).PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelMage_PropertyChanged);

            }
        }

        void CalculationOptionsPanelMage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }
    }
}
