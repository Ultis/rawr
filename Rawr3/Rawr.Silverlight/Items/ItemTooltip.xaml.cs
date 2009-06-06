using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Rawr.Silverlight
{
	public partial class ItemTooltip : UserControl
	{

        private ItemInstance itemInstance; 
        public ItemInstance ItemInstance {
            get { return itemInstance; }
            set
            {
                itemInstance = value;
                item = null;
                UpdateTooltip();
            }
        }

        private Item item;
        public Item Item
        {
            get { return item; }
            set
            {
                itemInstance = null;
                item = value;
                UpdateTooltip();
            }
        }

        private static Color ColorForGem(Item.ItemSlot gemType)
        {
            switch (gemType)
            {
                case Item.ItemSlot.Red:
                    return Colors.Red;
                case Item.ItemSlot.Yellow:
                    return Colors.Yellow;
                case Item.ItemSlot.Blue:
                    return Colors.Blue;
                case Item.ItemSlot.Orange:
                    return Colors.Orange;
                case Item.ItemSlot.Purple:
                    return Colors.Purple;
                case Item.ItemSlot.Green:
                    return Colors.Green;
                case Item.ItemSlot.Prismatic:
                    return Colors.LightGray;
                case Item.ItemSlot.Meta:
                    return Colors.Gray;
                default:
                    return Colors.Transparent;
            }
        }

        private static Color ColorForQuality(Item.ItemQuality quality)
        {
            switch (quality)
            {
                case Item.ItemQuality.Artifact:
                case Item.ItemQuality.Heirloom:
                    return Colors.Yellow;
                case Item.ItemQuality.Legendary:
                    return Colors.Orange;
                case Item.ItemQuality.Epic:
                    return Colors.Purple;
                case Item.ItemQuality.Rare:
                    return Colors.Blue;
                case Item.ItemQuality.Uncommon:
                    return Colors.Green;
                case Item.ItemQuality.Common:
                    return Colors.Gray;
                case Item.ItemQuality.Poor:
                    return Colors.DarkGray;
                case Item.ItemQuality.Temp:
                default:
                    return Colors.Black;
            }
        }

        public void UpdateTooltip()
        {
            Item actualItem = null;
            if (ItemInstance != null)
            {
                actualItem = ItemInstance.Item;
            }
            else if (Item != null)
            {
                actualItem = Item;
            }
            else
            {
                RootLayout.Visibility = Visibility.Collapsed;
                return;
            }
            RootLayout.Visibility = Visibility.Visible;

            ItemName.Text = actualItem.Name;
            ItemName.Foreground = new SolidColorBrush(ColorForQuality(actualItem.Quality));
            if (actualItem.ItemLevel > 0) ItemLevel.Text = "[" + actualItem.ItemLevel + "]";
            else ItemLevel.Text = "";

            #region Displaying Item Stats
            List<string> statsList = new List<string>();

            Stats relevantStats = Calculations.GetRelevantStats(actualItem.Stats);
            var positiveStats = relevantStats.Values(x => x != 0);
            foreach (System.Reflection.PropertyInfo info in positiveStats.Keys)
            {
                float value = positiveStats[info];
                if (Stats.IsPercentage(info)) value *= 100;
                value = (float)Math.Round(value * 100f) / 100f;
                string text = string.Format("{0}{1}", value, Extensions.DisplayName(info));
                statsList.Add(text);
            }
            if (actualItem.DPS > 0)
            {
                float dps = (float)Math.Round(actualItem.DPS * 100f) / 100f;
                string text = dps + " DPS";
                statsList.Add(text);
                text = actualItem.Speed + " Speed";
                statsList.Add(text);
            }
            foreach (SpecialEffect effect in relevantStats.SpecialEffects())
            {
                string text = effect.ToString();
                statsList.Add(text);
            }
            StatPanel.Children.Clear();
            if (statsList.Count == 0) StatPanel.Visibility = Visibility.Collapsed;
            else
            {
                StatPanel.Visibility = Visibility.Visible;
                foreach (string s in statsList)
                {
                    TextBlock text = new TextBlock();
                    text.Margin = new Thickness(0, 0, 16, 0);
                    text.HorizontalAlignment = HorizontalAlignment.Left;
                    text.VerticalAlignment = VerticalAlignment.Top;
                    text.Text = s;
                    StatPanel.Children.Add(text);
                }
            }
            #endregion

            #region Setting Up Gems
            bool hasGems = false;
            if (actualItem.SocketColor1 == Item.ItemSlot.None)
            {
                GemColor1.Visibility = Visibility.Collapsed;
                GemImage1.Visibility = Visibility.Collapsed;
                GemStat1.Visibility = Visibility.Collapsed;
            }
            else
            {
                GemColor1.Visibility = Visibility.Visible;
                GemColor1.Background = new SolidColorBrush(ColorForGem(actualItem.SocketColor1));

                GemImage1.Visibility = Visibility.Visible;
                GemImage1.Source = null;

                GemStat1.Visibility = Visibility.Visible;
                GemStat1.Children.Clear();
                hasGems = true;
            }
            if (actualItem.SocketColor2 == Item.ItemSlot.None)
            {
                GemColor2.Visibility = Visibility.Collapsed;
                GemImage2.Visibility = Visibility.Collapsed;
                GemStat2.Visibility = Visibility.Collapsed;
            }
            else
            {
                GemColor2.Visibility = Visibility.Visible;
                GemColor2.Background = new SolidColorBrush(ColorForGem(actualItem.SocketColor2));

                GemImage2.Visibility = Visibility.Visible;
                GemImage2.Source = null;

                GemStat2.Visibility = Visibility.Visible;
                GemStat2.Children.Clear();
                hasGems = true;
            }
            if (actualItem.SocketColor3 == Item.ItemSlot.None)
            {
                GemColor3.Visibility = Visibility.Collapsed;
                GemImage3.Visibility = Visibility.Collapsed;
                GemStat3.Visibility = Visibility.Collapsed;
            }
            else
            {
                GemColor3.Visibility = Visibility.Visible;
                GemColor3.Background = new SolidColorBrush(ColorForGem(actualItem.SocketColor3));

                GemImage3.Visibility = Visibility.Visible;
                GemImage3.Source = null;

                GemStat3.Visibility = Visibility.Visible;
                GemStat3.Children.Clear();
                hasGems = true;
            }
            if (hasGems)
            {
                SocketBonusLabel.Visibility = Visibility.Visible;
                SocketBonusLabel.Text = "Socket Bonus: " + (actualItem.SocketBonus.ToString().Length == 0 ? "None" : actualItem.SocketBonus.ToString());
            }
            else SocketBonusLabel.Visibility = Visibility.Collapsed;
            #endregion

            EnchantLabel.Visibility = Visibility.Collapsed;

            if (ItemInstance != null)
            {
                if (ItemInstance.EnchantId > 0 && ItemInstance.Enchant != null)
                {
                    EnchantLabel.Text = ItemInstance.Enchant.Name + ": "
                        + Calculations.GetRelevantStats(ItemInstance.Enchant.Stats).ToString();
                    EnchantLabel.Visibility = Visibility.Visible;
                }
                if (ItemInstance.Gem1 != null)
                {
                    if (actualItem.SocketColor1 == Item.ItemSlot.None)
                    {
                        GemColor1.Visibility = Visibility.Visible;
                        GemColor1.Background = new SolidColorBrush(ColorForGem(Item.ItemSlot.Prismatic));
                        GemImage1.Visibility = Visibility.Visible;
                        GemImage1.Source = null;
                        GemStat1.Visibility = Visibility.Visible;
                        GemStat1.Children.Clear();
                        hasGems = true;
                    }
                    GemImage1.Source = Icons.ItemIcon(ItemInstance.Gem1.IconPath);
                    foreach (string s in Calculations.GetRelevantStats(ItemInstance.Gem1.Stats).ToString().Split(','))
                    {
                        TextBlock t = new TextBlock();
                        t.Text = s.StartsWith(" ") ? s.Substring(1) : s;
                        GemStat1.Children.Add(t);
                    }
                }
                if (ItemInstance.Gem2 != null)
                {
                    if (actualItem.SocketColor2 == Item.ItemSlot.None)
                    {
                        GemColor2.Visibility = Visibility.Visible;
                        GemColor2.Background = new SolidColorBrush(ColorForGem(Item.ItemSlot.Prismatic));
                        GemImage2.Visibility = Visibility.Visible;
                        GemImage2.Source = null;
                        GemStat2.Visibility = Visibility.Visible;
                        GemStat2.Children.Clear();
                        hasGems = true;
                    }
                    GemImage2.Source = Icons.ItemIcon(ItemInstance.Gem2.IconPath);
                    foreach (string s in Calculations.GetRelevantStats(ItemInstance.Gem2.Stats).ToString().Split(','))
                    {
                        TextBlock t = new TextBlock();
                        t.Text = s.StartsWith(" ") ? s.Substring(1) : s;
                        GemStat2.Children.Add(t);
                    }
                }
                if (ItemInstance.Gem3 != null)
                {
                    if (actualItem.SocketColor3 == Item.ItemSlot.None)
                    {
                        GemColor3.Visibility = Visibility.Visible;
                        GemColor3.Background = new SolidColorBrush(ColorForGem(Item.ItemSlot.Prismatic));
                        GemImage3.Visibility = Visibility.Visible;
                        GemImage3.Source = null;
                        GemStat3.Visibility = Visibility.Visible;
                        GemStat3.Children.Clear();
                        hasGems = true;
                    }
                    GemImage3.Source = Icons.ItemIcon(ItemInstance.Gem3.IconPath);
                    foreach (string s in Calculations.GetRelevantStats(ItemInstance.Gem3.Stats).ToString().Split(','))
                    {
                        TextBlock t = new TextBlock();
                        t.Text = s.StartsWith(" ") ? s.Substring(1) : s;
                        GemStat3.Children.Add(t);
                    }
                }
                if (!Item.GemMatchesSlot(itemInstance.Gem1, actualItem.SocketColor1) ||
                                    !Item.GemMatchesSlot(itemInstance.Gem2, actualItem.SocketColor2) ||
                                    !Item.GemMatchesSlot(itemInstance.Gem3, actualItem.SocketColor3))
                    SocketBonusLabel.Foreground = new SolidColorBrush(Colors.Gray);
                else SocketBonusLabel.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        public void Show(UIElement relativeTo) { Show(relativeTo, 0, 0); }
        public void Show(UIElement relativeTo, double offsetX, double offsetY)
        {
            GeneralTransform gt = relativeTo.TransformToVisual((UIElement)this.Parent);
            Point offset = gt.Transform(new Point(offsetX, offsetY));
            ItemPopup.VerticalOffset = offset.Y;
            ItemPopup.HorizontalOffset = offset.X;
            ItemPopup.IsOpen = true;
        }

        public void Hide()
        {
            ItemPopup.IsOpen = false;
        }

		public ItemTooltip()
		{
			// Required to initialize variables
			InitializeComponent();
        }
	}
}