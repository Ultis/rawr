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
    public partial class CalculationOptionsPanelRetribution : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelRetribution()
        {
            InitializeComponent();
        }

        #region ICalculationOptionsPanel Members
        public UserControl PanelControl { get { return this; } }

        private CalculationOptionsRetribution CalcOpts
        {
            get { return DataContext as CalculationOptionsRetribution; }
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
                    ((CalculationOptionsRetribution)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(calcOpts_PropertyChanged);
                character = value;
                if (character.CalculationOptions == null)
                    character.CalculationOptions = new CalculationOptionsRetribution();

                CalculationOptionsRetribution calcOpts = character.CalculationOptions as CalculationOptionsRetribution;
                
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
