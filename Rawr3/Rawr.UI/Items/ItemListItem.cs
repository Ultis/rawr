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
using System.Collections.Generic;

namespace Rawr.UI
{
    public class ItemListItem
    {
        private ComparisonCalculationBase _calc;
        private double _maxValue;
        private double _maxWidth;

        public ItemListItem(ComparisonCalculationBase calc, double maxValue, double maxWidth)
        {
            _calc = calc;
            _maxValue = maxValue;
            _maxWidth = maxWidth;
        }

        public bool Equipped { get { return _calc.Equipped; } }
        public ItemInstance ItemInstance { get { return _calc.ItemInstance; } }
        public Item Item { get { return _calc.Item; } }
        public string Name { get { return _calc.Name; } }
        public float OverallRating { get { return _calc.OverallPoints; } }

        public int EnchantId
        {
            get
            {
                if (_calc.Item == null) return 0;
                return Math.Abs(_calc.Item.Id % 10000);
            }
        }

        public int ReforgeId
        {
            get
            {
                if (_calc.Item == null) return 0;
                return -_calc.Item.Id - 1000000;
            }
        }

        public ImageSource Icon
        {
            get
            {
                if (_calc.ItemInstance != null && _calc.ItemInstance.Item != null)
                    return Icons.AnIcon(_calc.ItemInstance.Item.IconPath);
                else
                    return null;
            }
        }

#if false
        public void ItemImage_ImageFailed(object o, ExceptionRoutedEventArgs e)
        {
            ItemImage.ImageFailed -= new EventHandler<ExceptionRoutedEventArgs>(ItemImage_ImageFailed);
#if DEBUG
            //TalentImage_ImageFailed2(o, e); // Tell me what happened
            //TalentImage.ImageFailed += new EventHandler<ExceptionRoutedEventArgs>(TalentImage_ImageFailed2);
#endif
            // Getting the Image from the Armory failed, lets try another source
            if (itemInstance != null)
            {
                ItemImage.Source = Icons.AnIcon(ItemInstance.Item.IconPath);
            }
            else if (NonItemImageSource != null)
            {
                ItemImage.Source = Icons.AnIcon(NonItemImageSource);
            }
        }

        public void ItemImage_ImageFailed2(object o, ExceptionRoutedEventArgs e)
        {
            ItemImage.ImageFailed -= new EventHandler<ExceptionRoutedEventArgs>(ItemImage_ImageFailed2);
            // Getting the Image from the Armory & Wowhead failed, tell me why
            /*string infoString = string.Format("Talent Name: {0}\r\nClass: {1}\r\nTree Name: {2}\r\nTalent Icon: {3}\r\nSource String: {4}",
                talentData.Name, TalentTree.Class, TalentTree.TreeName, talentData.Icon, (TalentImage.Source as BitmapImage).UriSource);
            /*Base.ErrorBox eb = new Base.ErrorBox("Error getting the talent image", e.ErrorException, "Talent Image Update()", infoString);
            eb.Show();*/
        }
#endif

        public ItemListItemRating[] Ratings
        {
            get
            {
                ItemListItemRating[] ratings = new ItemListItemRating[_calc.SubPoints.Length];
                Color[] colors = new Color[ratings.Length];
                int i = 0;
                foreach (var kvp in Calculations.SubPointNameColors)
                    colors[i++] = kvp.Value;
                for (i = 0; i < ratings.Length; i++)
                    ratings[i] = new ItemListItemRating()
                    {
                        Brush = new SolidColorBrush(colors[i]),
                        Width = Math.Max(0d, _maxWidth * ((double)_calc.SubPoints[i] / _maxValue))
                    };
                return ratings;
            }
        }

        public ItemListItemGem[] Gems
        {
            get
            {
                if (_calc.ItemInstance == null || _calc.ItemInstance.Item == null)
                    return new ItemListItemGem[0];

                List<ItemListItemGem> gems = new List<ItemListItemGem>();

                if (_calc.ItemInstance.Gem1 != null)
                    gems.Add(new ItemListItemGem()
                    {
                        GemIcon = Icons.AnIcon(_calc.ItemInstance.Gem1.IconPath),
                        SocketBrush = GetBrushForSlot(_calc.ItemInstance.Item.SocketColor1)
                    });
                if (_calc.ItemInstance.Gem2 != null)
                    gems.Add(new ItemListItemGem()
                    {
                        GemIcon = Icons.AnIcon(_calc.ItemInstance.Gem2.IconPath),
                        SocketBrush = GetBrushForSlot(_calc.ItemInstance.Item.SocketColor2)
                    });
                if (_calc.ItemInstance.Gem3 != null)
                    gems.Add(new ItemListItemGem()
                    {
                        GemIcon = Icons.AnIcon(_calc.ItemInstance.Gem3.IconPath),
                        SocketBrush = GetBrushForSlot(_calc.ItemInstance.Item.SocketColor3)
                    });

                return gems.ToArray();
            }
        }

        private Brush GetBrushForSlot(ItemSlot slot)
        {
            switch (slot)
            {
                case ItemSlot.Red: return new SolidColorBrush(Colors.Red);
                case ItemSlot.Yellow: return new SolidColorBrush(Colors.Yellow);
                case ItemSlot.Blue: return new SolidColorBrush(Colors.Blue);
                case ItemSlot.Cogwheel: return new SolidColorBrush(Colors.Black);
                case ItemSlot.Hydraulic: return new SolidColorBrush(Colors.White);
                default: return new SolidColorBrush(Colors.Gray);
            }
        }

        public class ItemListItemRating
        {
            public double Width { get; set; }
            public Brush Brush { get; set; }
            public Visibility Visibility { get; set; }
        }

        public class ItemListItemGem
        {
            public ImageSource GemIcon { get; set; }
            public Brush SocketBrush { get; set; }
        }
    }
}
