using System;
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
}
