using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.TankDK
{
    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(float)) return System.Convert.ToSingle(value) * 100.0f;
            if (targetType == typeof(double)) return System.Convert.ToDouble(value) * 100.0d;
            return System.Convert.ToDouble(value) * 100.0d;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(float)) return System.Convert.ToSingle(value) / 100f;
            if (targetType == typeof(double)) return System.Convert.ToDouble(value) / 100d;
            return System.Convert.ToDouble(value) / 100d;
        }
    }
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
