using System;
using System.Globalization;
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
using System.Collections.Generic;

namespace Rawr.UI
{
    // Code inspired by http://charlass.wordpress.com/2009/07/29/binding-enums-to-a-combobbox-in-silverlight/
    /// <summary>
    /// Caches the "enum objects" for the lifetime of the application.
    /// </summary>
    internal static class EnumValueCache
    {
        private static readonly IDictionary<Type, object[]> Cache = new Dictionary<Type, object[]>();

        public static object[] GetValues(Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("Type '" + type.Name + "' is not an enum");

            List<object> ltemp = new List<object>();
            object[] values = ltemp.ToArray();
            if (!Cache.TryGetValue(type, out values))
            {
                System.Reflection.FieldInfo[] temp = type.GetFields();
                foreach (System.Reflection.FieldInfo f in temp)
                {
                    if (f.IsLiteral)
                        ltemp.Add(f.GetValue(null));
                }
                Cache[type] = values = ltemp.ToArray();
            }
            return values;
        }
    }

    /// <summary>
    /// Enum => EnumValues
    /// </summary>
    public class EnumValuesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            else
                return EnumValueCache.GetValues(value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
