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

    public class GemColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Item item = value as Item;
            Item.ItemSlot gemType;
            if (parameter.ToString() == "1") gemType = item.SocketColor1;
            else if (parameter.ToString() == "2") gemType = item.SocketColor2;
            else if (parameter.ToString() == "3") gemType = item.SocketColor3;
            else gemType = Item.ItemSlot.None;
            
            switch (gemType)
            {
                case Item.ItemSlot.Red:
                    return new SolidColorBrush(Colors.Red);
                case Item.ItemSlot.Yellow:
                    return new SolidColorBrush(Colors.Yellow);
                case Item.ItemSlot.Blue:
                    return new SolidColorBrush(Colors.Blue);
                case Item.ItemSlot.Orange:
                    return new SolidColorBrush(Colors.Orange);
                case Item.ItemSlot.Purple:
                    return new SolidColorBrush(Colors.Purple);
                case Item.ItemSlot.Green:
                    return new SolidColorBrush(Colors.Green);
                case Item.ItemSlot.Prismatic:
                    return new SolidColorBrush(Colors.LightGray);
                case Item.ItemSlot.Meta:
                    return new SolidColorBrush(Colors.Gray);
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

}
