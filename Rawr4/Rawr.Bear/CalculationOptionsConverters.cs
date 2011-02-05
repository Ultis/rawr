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
			if (threatValue == 1f) return "Almost None";
			if (threatValue == 5f) return "MT";
			if (threatValue == 25f) return "OT";
			if (threatValue == 50f) return "Crazy About Threat";
			else return "Custom...";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string threatValue = (string)value;
			switch (threatValue)
			{
				case "Almost None": return 1f;
				case "MT": return 5f;
				case "OT": return 25f;
				case "Crazy About Threat": return 50f;
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
			if (survivalSoftCap == 50000*3) return "Normal Dungeons";
			if (survivalSoftCap == 80000*3) return "Heroic Dungeons";
			if (survivalSoftCap == 150000*3) return "Normal T11 Raids";
			if (survivalSoftCap == 175000*3) return "Heroic T11 Raids";
			else return "Custom...";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string survivalSoftCap = (string)value;
			switch (survivalSoftCap)
			{
				case "Normal Dungeons": return 50000*3;
				case "Heroic Dungeons": return 80000*3;
				case "Normal T11 Raids": return 150000*3;
				case "Heroic T11 Raids": return 175000*3;
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
			if (survivalSoftCap == 80000) return "Heroic Dungeons";
			if (survivalSoftCap == 150000) return "Normal T11 Raids";
			if (survivalSoftCap == 175000) return "Heroic T11 Raids";
			else return "Custom...";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string survivalSoftCap = (string)value;
			switch (survivalSoftCap)
			{
				case "Normal Dungeons": return 50000;
				case "Heroic Dungeons": return 80000;
				case "Normal T11 Raids": return 150000;
				case "Heroic T11 Raids": return 175000;
			}
			return null;
		}

		#endregion
	}
}
