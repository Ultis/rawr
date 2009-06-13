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

namespace Rawr.Healadin
{
    public partial class CalculationOptionsPanelHealadin : UserControl, ICalculationOptionsPanel

    {
        public CalculationOptionsPanelHealadin()
        {
            InitializeComponent();
        }

        #region ICalculationOptionsPanel Members
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
                if (character != null && character.CalculationOptions != null
                    && character.CalculationOptions is CalculationOptionsHealadin)
                    ((CalculationOptionsHealadin)character.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(calcOpts_PropertyChanged);

                character = value;
                if (character.CalculationOptions == null)
                    character.CalculationOptions = new CalculationOptionsHealadin();

                CalculationOptionsHealadin calcOpts = character.CalculationOptions as CalculationOptionsHealadin;
                DataContext = calcOpts;
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(calcOpts_PropertyChanged);
            }
        }

        private void calcOpts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }
        #endregion
    }
}
