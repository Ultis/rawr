using System;
using System.Windows.Data;
using System.Globalization;

namespace Rawr.ShadowPriest
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
            if (targetType == typeof(float)) return System.Convert.ToSingle(value) / 100.0f;
            if (targetType == typeof(double)) return System.Convert.ToDouble(value) / 100.0d;
            return System.Convert.ToDouble(value) / 100.0d;
        }
    }
    public class MillisecConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(float)) return System.Convert.ToSingle(value) * 1000.0f;
            if (targetType == typeof(double)) return System.Convert.ToDouble(value) * 1000.0d;
            return System.Convert.ToDouble(value) * 1000.0d;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(float)) return System.Convert.ToSingle(value) / 1000.0f;
            if (targetType == typeof(double)) return System.Convert.ToDouble(value) / 1000.0d;
            return System.Convert.ToDouble(value) / 1000.0d;
        }
    }
}

