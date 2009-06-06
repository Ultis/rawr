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

namespace Rawr.Retribution
{
    public partial class CalculationOptionsPanelRetribution : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelRetribution()
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
                character = value;
            }
        }

        public void LoadCalculationOptions()
        {
            ;
        }

        #endregion
    }
}
