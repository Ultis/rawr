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

namespace Rawr.Bear
{
	public partial class CalculationOptionsPanelBear : UserControl, ICalculationOptionsPanel
	{
		public CalculationOptionsPanelBear()
		{
			InitializeComponent();
            this.LayoutRoot.SelectAll();
            foreach (AccordionItem item in LayoutRoot.Items)
            {
                item.IsSelected = true;
            }
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
			if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsBear();
            this.DataContext = Character.CalculationOptions as CalculationOptionsBear;
            this.LayoutRoot.SelectAll();
			_loadingCalculationOptions = false;
		}
		#endregion
	}
}
