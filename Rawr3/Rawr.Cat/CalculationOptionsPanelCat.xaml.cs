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

namespace Rawr.Cat
{
	public partial class CalculationOptionsPanelCat : UserControl, ICalculationOptionsPanel
	{
		public CalculationOptionsPanelCat()
		{
			InitializeComponent();
			DataContext = this;
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
				character = value;
				LoadCalculationOptions();
			}
		}

		private bool _loadingCalculationOptions;
		public void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsCat();

			_loadingCalculationOptions = false;
		}
		#endregion
	}
}
