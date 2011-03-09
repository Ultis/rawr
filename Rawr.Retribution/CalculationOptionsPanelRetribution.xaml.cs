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
        public CalculationOptionsPanelRetribution()
        {
            InitializeComponent();
        }

        public UserControl PanelControl { get { return this; } }

        #region ICalculationOptionsPanel Members
        private CalculationOptionsRetribution CalcOpts
        {
            get { return (CalculationOptionsRetribution) character.CalculationOptions; }
        }

        private Character character;
        public Character Character
        {
            get
            {
                return character;
            }
            set
            {
                if (character != null && character.CalculationOptions != null && character.CalculationOptions is CalculationOptionsRetribution)
                    ((CalculationOptionsRetribution)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(CalculationOptionsRetribution_PropertyChanged);
                character = value;
                if (character.CalculationOptions == null)
                    character.CalculationOptions = new CalculationOptionsRetribution();

                CalcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsRetribution_PropertyChanged);
                CalculationOptionsRetribution_PropertyChanged(null, new PropertyChangedEventArgs(""));
            }
        }

        void CalculationOptionsRetribution_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }

        private void btnResetBelow20_Click(object sender, RoutedEventArgs e)
        {
            CalcOpts.TimeUnder20 = .18f;
        }
        #endregion

    }
}
