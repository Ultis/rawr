using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.Properties
{
	public class GeneralSettings
	{

		static GeneralSettings()
		{
			_default = new GeneralSettings();

			_default.CountGemmingsShown = 3;
			_default.UseMultithreading = true;
			_default.Locale = "en";
			_default.DisplayBuffSource = false;
			_default.DisplayGemNames = false;
			_default.DisplayItemIds = false;
			_default.ProcEffectMode = 3;
			_default.CombinationEffectMode = 1;

		}

		private static GeneralSettings _default;
		public static GeneralSettings Default { get { return _default; } set { _default = value; } }

		public int CountGemmingsShown { get; set; }
		public bool UseMultithreading { get; set; }
		public string Locale { get; set; }
		public bool DisplayBuffSource { get; set; }
		public bool DisplayGemNames { get; set; }
		public int ProcEffectMode { get; set; }
		public bool DisplayItemIds { get; set; }
        public bool DisplayItemType { get; set; }
        public bool HideProfEnchants { get; set; }
		public int CombinationEffectMode { get; set; }
	}
}
