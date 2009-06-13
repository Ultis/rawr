using System;
using System.Globalization;
using System.Windows.Data;
using System.Reflection;

namespace Rawr.Silverlight
{

    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(float)) return (float)value * 100.0f;
            return (float)value * 100.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(float)) return (float)((double)value / 100);
            return (double)value / 100;
        }
    }

}
