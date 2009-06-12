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

namespace Rawr.Silverlight
{

    public class ItemQualityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Item item = value as Item;
            Item.ItemQuality quality = item.Quality;
            switch (quality)
            {
                case Item.ItemQuality.Artifact:
                case Item.ItemQuality.Heirloom:
                    return new SolidColorBrush(Colors.Yellow);
                case Item.ItemQuality.Legendary:
                    return new SolidColorBrush(Colors.Orange);
                case Item.ItemQuality.Epic:
                    return new SolidColorBrush(Colors.Purple);
                case Item.ItemQuality.Rare:
                    return new SolidColorBrush(Colors.Blue);
                case Item.ItemQuality.Uncommon:
                    return new SolidColorBrush(Colors.Green);
                case Item.ItemQuality.Common:
                    return new SolidColorBrush(Colors.Gray);
                case Item.ItemQuality.Poor:
                    return new SolidColorBrush(Colors.DarkGray);
                case Item.ItemQuality.Temp:
                default:
                    return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
