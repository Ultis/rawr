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
using System.Globalization;

namespace Rawr.TankDK
{
    public class GraphTypeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CalculationType cType = (CalculationType)value;
            if (cType == CalculationType.SMT) return "Default";
            if (cType == CalculationType.Burst) return "Burst Time";
            else return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string survivalSoftCap = (string)value;
            switch (survivalSoftCap)
            {
                case "Default": return CalculationType.SMT;
                case "Burst Time": return CalculationType.Burst;
            }
            return null;
        }

        #endregion
    }

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

    public class SurvivalSoftCapConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int survivalSoftCap = (int)value;
            if (survivalSoftCap == 100000) return "Normal Dungeons";
            if (survivalSoftCap == 175000) return "Heroic Dungeons";
            if (survivalSoftCap == 250000) return "Normal T11 Raids";
            if (survivalSoftCap == 300000) return "Heroic T11 Raids";
            else return "Custom...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string survivalSoftCap = (string)value;
            switch (survivalSoftCap)
            {
                case "Normal Dungeons" : return 100000;
                case "Heroic Dungeons" : return 175000;
                case "Normal T11 Raids": return 250000;
                case "Heroic T11 Raids": return 300000;
            }
            return null;
        }

        #endregion
    }
}
