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

namespace Rawr.ProtPaladin
{
	public partial class CalculationOptionsPanelProtPaladin : UserControl, ICalculationOptionsPanel
	{
		public CalculationOptionsPanelProtPaladin()
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
                if (character != null && character.CalculationOptions != null && character.CalculationOptions is CalculationOptionsProtPaladin)
                {
                    ((CalculationOptionsProtPaladin)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(CalculationOptionsPanelProtPaladin_PropertyChanged);
                }

                character = value;
                if (character.CalculationOptions == null)
                    character.CalculationOptions = new CalculationOptionsProtPaladin();

                LayoutRoot.DataContext = Character.CalculationOptions;

                CalculationOptionsProtPaladin calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

                DataContext = calcOpts;

                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelProtPaladin_PropertyChanged);
            }
        }

        void CalculationOptionsPanelProtPaladin_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }
	}
}
