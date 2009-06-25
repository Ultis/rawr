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

namespace Rawr.Moonkin
{
    public partial class CalculationOptionsPanelMoonkin : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelMoonkin()
        {
            InitializeComponent();
        }

        #region ICalculationOptionsPanel Members

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
                    && character.CalculationOptions is CalculationOptionsMoonkin)
                    ((CalculationOptionsMoonkin)character.CalculationOptions).PropertyChanged
                        -= new System.ComponentModel.PropertyChangedEventHandler(calcOpts_PropertyChanged);

                character = value;
                if (character.CalculationOptions == null)
                    character.CalculationOptions = new CalculationOptionsMoonkin();

                CalculationOptionsMoonkin calcOpts = character.CalculationOptions as CalculationOptionsMoonkin;
                DataContext = calcOpts;
                calcOpts.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(calcOpts_PropertyChanged);
            }
        }

        public UserControl PanelControl
        {
            get { return this; }
        }

        private void calcOpts_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }

        #endregion
    }
}
