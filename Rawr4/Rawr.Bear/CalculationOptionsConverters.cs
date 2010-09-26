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
using System.Windows.Data;

namespace Rawr.Bear
{
	public class ThreatValueConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			float threatValue = (float)value;
			if (threatValue == 0f) return "None";
			if (threatValue == 10f) return "MT";
			if (threatValue == 50f) return "OT";
			if (threatValue == 100f) return "Crazy About Threat";
			else return "Custom...";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string threatValue = (string)value;
			switch (threatValue)
			{
				case "None": return 0f;
				case "MT": return 10f;
				case "OT": return 50f;
				case "Crazy About Threat": return 100f;
			}
			return null;
		}

		#endregion
	}

	public class SurvivalSoftCapConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int survivalSoftCap = (int)value;
			if (survivalSoftCap == 900000) return "Normal Dungeons";
			if (survivalSoftCap == 1100000) return "Heroic Dungeons";
			else return "Custom...";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string survivalSoftCap = (string)value;
			switch (survivalSoftCap)
			{
				case "Normal Dungeons": return 900000;
				case "Heroic Dungeons": return 1100000;
			}
			return null;
		}

		#endregion
	}

	public class TargetDamageConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			int survivalSoftCap = (int)value;
			if (survivalSoftCap == 50000) return "Normal Dungeons";
			if (survivalSoftCap == 57000) return "Heroic Dungeons";
			else return "Custom...";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string survivalSoftCap = (string)value;
			switch (survivalSoftCap)
			{
				case "Normal Dungeons": return 50000;
				case "Heroic Dungeons": return 57000;
			}
			return null;
		}

		#endregion
	}
}
