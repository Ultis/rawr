using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class ItemTooltip : UserControl
    {
        /// <summary>This is used by the Non-Item charts</summary>
        public string CurrentString
        {
            get { return currentString; }
            set
            {
                itemInstance = null;
                item = null;
                //itemSet = null;
                characterItems = null;
                currentString = value;
                UpdateTooltip();
            }
        }
        private string currentString;

        private ItemInstance itemInstance; 
        public ItemInstance ItemInstance {
            get { return itemInstance; }
            set
            {
                itemInstance = value;
                item = null;
                itemSet = null;
                characterItems = null;
                currentString = null;
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
                itemSet = null;
                characterItems = null;
                currentString = null;
                UpdateTooltip();
            }
        }

        /// <summary>This is used by the Build Upgrade List chart</summary>
        public ItemInstance[] CharacterItems
        {
            get { return characterItems; }
            set
            {
                characterItems = value;
                itemSet = null;
                UpdateTooltip();
            }
        }
        private ItemInstance[] characterItems;

        /// <summary>This is used by the Item Sets chart</summary>
        public ItemSet ItemSet {
            get { return itemSet; }
            set {
                itemSet = value;
                item = null;
                itemInstance = null;
                characterItems = null;
                //UpdateTooltip();
            }
        }
        private ItemSet itemSet = null;

        private static Color ColorForGem(ItemSlot gemType)
        {
            switch (gemType)
            {
                case ItemSlot.Red:
                    return Colors.Red;
                case ItemSlot.Yellow:
                    return Colors.Yellow;
                case ItemSlot.Blue:
                    return Colors.Blue;
                case ItemSlot.Orange:
                    return Colors.Orange;
                case ItemSlot.Purple:
                    return Colors.Purple;
                case ItemSlot.Green:
                    return Colors.Green;
                case ItemSlot.Prismatic:
                    return Colors.LightGray;
                case ItemSlot.Meta:
                    return Colors.Gray;
                case ItemSlot.Cogwheel:
                    return Colors.Black;
                case ItemSlot.Hydraulic:
                    return Colors.White;
                default:
                    return Colors.Transparent;
            }
        }

        private static Color ColorForQuality(ItemQuality quality)
        {
            switch (quality)
            {
                case ItemQuality.Artifact:
                case ItemQuality.Heirloom:
                    return Colors.Yellow;
                case ItemQuality.Legendary:
                    return Colors.Orange;
                case ItemQuality.Epic:
                    return Colors.Purple;
                case ItemQuality.Rare:
                    return Colors.Blue;
                case ItemQuality.Uncommon:
                    return Colors.Green;
                case ItemQuality.Common:
                    return Colors.Gray;
                case ItemQuality.Poor:
                    return Colors.DarkGray;
                case ItemQuality.Temp:
                default:
                    return Colors.Black;
            }
        }

        private void NonItemTooltip() {
            RootLayout.Visibility = Visibility.Visible;
            try {
                string Title = CurrentString.Split('|')[0].Trim();
                string Desc = CurrentString.Split('|')[1].Trim();

                ItemName.Text = Title;
                ItemName.Foreground = new SolidColorBrush(Colors.Purple);

                // Hide Item related fields
                TypesPanel.Visibility = Visibility.Collapsed;
                StatPanel.Visibility = Visibility.Collapsed;
                GemStack.Visibility = Visibility.Collapsed;
                GemNamesLabel1.Visibility = Visibility.Collapsed;
                GemNamesLabel2.Visibility = Visibility.Collapsed;
                GemNamesLabel3.Visibility = Visibility.Collapsed;
                SocketBonusLabel.Visibility = Visibility.Collapsed;
                EnchantLabel.Visibility = Visibility.Collapsed;
                ReforgingLabel.Visibility = Visibility.Collapsed;
                SetLabel.Visibility = Visibility.Collapsed;
                UnusedStatPanel.Visibility = Visibility.Collapsed;

                LocationLabel.Visibility = Visibility.Visible;
                LocationLabel.Text = Desc;

                if (ItemSet != null && ItemSet.Count > 0) {
                    ItemsGrid.Visibility = Visibility.Visible;
                    #region Additional Items, like in Build Upgrade List
                    ItemsGrid.Children.Clear();
                    ItemsGrid.RowDefinitions.Clear();
                    int row = 0;
                    foreach (ItemInstance setItem in ItemSet)
                    {
                        ItemsGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                        if (setItem == null) continue;

                        Image iconImage = new Image();
                        iconImage.Style = Resources["SmallIconStyle"] as Style;
                        iconImage.Source = Icons.AnIcon(setItem.Item.IconPath);
                        Grid.SetColumn(iconImage, 0);
                        Grid.SetRow(iconImage, row);
                        ItemsGrid.Children.Add(iconImage);

                        if (setItem.Gem1Id > 0)
                        {
                            Image gem1Image = new Image();
                            gem1Image.Style = Resources["SmallIconStyle"] as Style;
                            gem1Image.Source = Icons.AnIcon(setItem.Gem1.IconPath);
                            Grid.SetColumn(gem1Image, 1);
                            Grid.SetRow(gem1Image, row);
                            ItemsGrid.Children.Add(gem1Image);
                        }

                        if (setItem.Gem2Id > 0)
                        {
                            Image gem2Image = new Image();
                            gem2Image.Style = Resources["SmallIconStyle"] as Style;
                            gem2Image.Source = Icons.AnIcon(setItem.Gem2.IconPath);
                            Grid.SetColumn(gem2Image, 2);
                            Grid.SetRow(gem2Image, row);
                            ItemsGrid.Children.Add(gem2Image);
                        }

                        if (setItem.Gem3Id > 0)
                        {
                            Image gem3Image = new Image();
                            gem3Image.Style = Resources["SmallIconStyle"] as Style;
                            gem3Image.Source = Icons.AnIcon(setItem.Gem3.IconPath);
                            Grid.SetColumn(gem3Image, 3);
                            Grid.SetRow(gem3Image, row);
                            ItemsGrid.Children.Add(gem3Image);
                        }

                        TextBlock nameText = new TextBlock();
                        if (setItem.EnchantId > 0) nameText.Text = string.Format("{0} ({1})", setItem.Item.Name, setItem.Enchant.Name);
                        else nameText.Text = string.Format("{0}", setItem.Item.Name);
                        nameText.Foreground = new SolidColorBrush(ColorForQuality(setItem.Item.Quality));
                        Grid.SetColumn(nameText, 4);
                        Grid.SetRow(nameText, row);
                        ItemsGrid.Children.Add(nameText);

                        row++;
                    }
                    #endregion
                } else {
                    ItemsGrid.Visibility = Visibility.Collapsed;
                }
            } catch (Exception ex) {
                new Base.ErrorBox() {
                    Title = "Error setting up a Non-Item Tooltip",
                    Function = "NonItemTooltip()",
                    TheException = ex,
                }.Show();
            }
        }

        public static void List2Panel(WrapPanel p, IList<string> li, Brush fore, bool allowWrap)
        {
            // reset
            p.Children.Clear();

            // show
            p.Visibility = li.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

            // fill
            foreach (var s in li)
            {
                var c = new TextBlock {
                    Margin = new Thickness(0, 0, 8, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Text = s,
                    TextWrapping = allowWrap ? TextWrapping.Wrap : TextWrapping.NoWrap,
                };

                if (fore != null)
                    c.Foreground = fore;
                p.Children.Add(c);
            }
        }

        public void UpdateTooltip()
        {
            Item actualItem = null;
            if (ItemInstance != null) {
                actualItem = ItemInstance.Item;
            } else if (Item != null) {
                actualItem = Item;
            } else if (ItemSet != null) {
                NonItemTooltip();
                return;
            } else if (CurrentString != null && CurrentString != "") {
                NonItemTooltip();
                return;
            } else {
                RootLayout.Visibility = Visibility.Collapsed;
                return;
            }
            RootLayout.Visibility = Visibility.Visible;

            ItemName.Text = ItemInstance != null ? ItemInstance.Name : (actualItem != null ? actualItem.Name : "");
            ItemName.Foreground = new SolidColorBrush(ColorForQuality(actualItem != null ? actualItem.Quality : ItemQuality.Common));

            #region Displaying Item Types
            var liTypes = new List<string>();

            if (actualItem != null && (actualItem.Type != ItemType.None || (actualItem.Type == ItemType.None
                && (actualItem.Slot == ItemSlot.Back || actualItem.Slot == ItemSlot.Neck || actualItem.Slot == ItemSlot.Finger || actualItem.Slot == ItemSlot.Trinket))))
            {
                //if (Properties.GeneralSettings.Default.DisplayExtraItemInfo) {
                if (actualItem.ItemLevel > 0)
                    liTypes.Add(string.Format("[{0}]", actualItem.ItemLevel));

                liTypes.Add(string.Format("[{0}]", actualItem.Id));

                if (actualItem.Bind != BindsOn.None)
                    liTypes.Add(string.Format("[{0}]", actualItem.Bind));

                if (!string.IsNullOrEmpty(actualItem.SlotString))
                    liTypes.Add(string.Format("[{0}]", actualItem.SlotString));

                if (actualItem.Type != ItemType.None)
                    liTypes.Add(string.Format("[{0}]", actualItem.Type));
                //}
            }
            List2Panel(TypesPanel, liTypes, new SolidColorBrush(Colors.Gray) , false);
            
            #endregion // Displaying Item Types

            #region Displaying Item Stats
            List<string> statsList = new List<string>();
            List<string> unusedStatsList = new List<string>();
            if (Rawr.Properties.GeneralSettings.Default.DisplayUnusedStats) {
                statsList.Add("Used Stats:");
                unusedStatsList.Add("Unused Stats:");
            }

            if (actualItem != null) {
                // Pull the Actual Stats, including Random Suffix if it has one
                Stats stats = actualItem.Stats;
                if (ItemInstance != null && ItemInstance.RandomSuffixId != 0) {
                    stats = stats.Clone();
                    RandomSuffix.AccumulateStats(stats, actualItem, ItemInstance.RandomSuffixId);
                }
                // Relevant Stats
                Stats relevantStats = Calculations.GetRelevantStats(stats);
                var nonzeroStats = relevantStats.Values(x => x != 0);
                foreach (System.Reflection.PropertyInfo info in nonzeroStats.Keys) {
                    float value = nonzeroStats[info];
                    if (Stats.IsPercentage(info)) value *= 100;
                    value = (float)Math.Round(value * 100f) / 100f;
                    string text = string.Format("{0}{1}", value, Extensions.DisplayName(info));
                    statsList.Add(text);
                }
                if (actualItem.DPS > 0) {
                    float dps = (float)Math.Round(actualItem.DPS * 100f) / 100f;
                    string text = dps + " DPS";
                    statsList.Add(text);
                    text = actualItem.Speed + " Speed";
                    statsList.Add(text);
                }
                foreach (SpecialEffect effect in relevantStats.SpecialEffects()) {
                    string text = effect.ToString();
                    statsList.Add(text);
                }
                // Irrelevant Stats
                Stats unusedStats = stats - relevantStats;
                var unusedNonzeroStats = unusedStats.Values(x => x != 0);
                foreach (System.Reflection.PropertyInfo info in unusedNonzeroStats.Keys) {
                    float value = unusedNonzeroStats[info];
                    if (Stats.IsPercentage(info)) value *= 100;
                    value = (float)Math.Round(value * 100f) / 100f;
                    string text = string.Format("{0}{1}", value, Extensions.DisplayName(info));
                    unusedStatsList.Add(text);
                }
                foreach (SpecialEffect effect in unusedStats.SpecialEffects()) {
                    string text = effect.ToString();
                    unusedStatsList.Add(text);
                }
            }
            if (statsList.Count > 1 || !Rawr.Properties.GeneralSettings.Default.DisplayUnusedStats)
            {
                if (unusedStatsList.Count <= 1 && Rawr.Properties.GeneralSettings.Default.DisplayUnusedStats)
                {
                    // Take off the "Used Stats" statement, we dont need to differentiate
                    statsList.RemoveAt(0);
                }
                List2Panel(StatPanel, statsList, null, true);
            } else { StatPanel.Children.Clear(); }
            if (Rawr.Properties.GeneralSettings.Default.DisplayUnusedStats && unusedStatsList.Count > 1)
            { List2Panel(UnusedStatPanel, unusedStatsList, new SolidColorBrush(Colors.Gray), true); }
            else { UnusedStatPanel.Children.Clear(); }
            #endregion

            #region Displaying Item Sets

            if (actualItem != null && actualItem.SetName != null && actualItem.SetName != "")
            {
                SetLabel.Text = string.Format("Set: {0}", actualItem.SetName);
                SetLabel.Visibility = Visibility.Visible;
            }
            else
            {
                SetLabel.Text = "";
                SetLabel.Visibility = Visibility.Collapsed;
            }

            #endregion

            #region Setting Up Gems
            bool hasGems = false;
            if (actualItem == null || actualItem.SocketColor1 == ItemSlot.None)
            {
                GemColor1.Visibility = Visibility.Collapsed;
                GemImage1.Visibility = Visibility.Collapsed;
                GemStat1.Visibility = Visibility.Collapsed;
                GemNamesLabel1.Visibility = Visibility.Collapsed;
            }
            else
            {
                GemColor1.Visibility = Visibility.Visible;
                GemColor1.Background = new SolidColorBrush(ColorForGem(actualItem.SocketColor1));

                GemImage1.Visibility = Visibility.Visible;
                GemImage1.Source = null;

                GemStat1.Visibility = Visibility.Visible;
                GemStat1.Children.Clear();
                GemNamesLabel1.Visibility = Visibility.Visible;
                hasGems = true;
            }
            if (actualItem == null || actualItem.SocketColor2 == ItemSlot.None)
            {
                GemColor2.Visibility = Visibility.Collapsed;
                GemImage2.Visibility = Visibility.Collapsed;
                GemStat2.Visibility = Visibility.Collapsed;
                GemNamesLabel2.Visibility = Visibility.Collapsed;
            }
            else
            {
                GemColor2.Visibility = Visibility.Visible;
                GemColor2.Background = new SolidColorBrush(ColorForGem(actualItem.SocketColor2));

                GemImage2.Visibility = Visibility.Visible;
                GemImage2.Source = null;

                GemStat2.Visibility = Visibility.Visible;
                GemStat2.Children.Clear();
                GemNamesLabel2.Visibility = Visibility.Visible;
                hasGems = true;
            }
            if (actualItem == null || actualItem.SocketColor3 == ItemSlot.None)
            {
                GemColor3.Visibility = Visibility.Collapsed;
                GemImage3.Visibility = Visibility.Collapsed;
                GemStat3.Visibility = Visibility.Collapsed;
                GemNamesLabel3.Visibility = Visibility.Collapsed;
            }
            else
            {
                GemColor3.Visibility = Visibility.Visible;
                GemColor3.Background = new SolidColorBrush(ColorForGem(actualItem.SocketColor3));

                GemImage3.Visibility = Visibility.Visible;
                GemImage3.Source = null;

                GemStat3.Visibility = Visibility.Visible;
                GemStat3.Children.Clear();
                GemNamesLabel3.Visibility = Visibility.Visible;
                hasGems = true;
            }
            if (hasGems)
            {
                GemStack.Visibility = Visibility.Visible;
                SocketBonusLabel.Visibility = Visibility.Visible;
                SocketBonusLabel.Text = "Socket Bonus: " + (actualItem.SocketBonus.ToString().Length == 0 ? "None" : actualItem.SocketBonus.ToString());
            }
            else SocketBonusLabel.Visibility = Visibility.Collapsed;
            #endregion

            #region Gem Sockets
            EnchantLabel.Visibility = Visibility.Collapsed;

            if (ItemInstance != null)
            {
                if (ItemInstance.EnchantId > 0 && ItemInstance.Enchant != null)
                {
                    EnchantLabel.Text = /*ItemInstance.Enchant.Name + ": "
                        + */Calculations.GetRelevantStats(ItemInstance.Enchant.Stats).ToString();
                    EnchantLabel.Visibility = Visibility.Visible;
                }
                if (ItemInstance.Gem1 != null)
                {
                    if (actualItem.SocketColor1 == ItemSlot.None)
                    {
                        GemColor1.Visibility = Visibility.Visible;
                        GemColor1.Background = new SolidColorBrush(ColorForGem(ItemSlot.Prismatic));
                        GemImage1.Visibility = Visibility.Visible;
                        GemImage1.Source = null;
                        GemStat1.Visibility = Visibility.Visible;
                        GemStat1.Children.Clear();
                        hasGems = true;
                    }
                    GemImage1.Source = Icons.AnIcon(ItemInstance.Gem1.IconPath);
                    foreach (string s in Calculations.GetRelevantStats(ItemInstance.Gem1.Stats).ToString().Split(','))
                    {
                        string r = s.Contains("Armor Penetration") ? s.Replace("Armor Penetration", "ArP") : s;
                        TextBlock t = new TextBlock();
                        t.Text = r.Trim();
                        GemStat1.Children.Add(t);
                    }
                    GemNamesLabel1.Text = "- " + ItemInstance.Gem1.Name;
                }
                if (ItemInstance.Gem2 != null)
                {
                    if (actualItem.SocketColor2 == ItemSlot.None)
                    {
                        GemColor2.Visibility = Visibility.Visible;
                        GemColor2.Background = new SolidColorBrush(ColorForGem(ItemSlot.Prismatic));
                        GemImage2.Visibility = Visibility.Visible;
                        GemImage2.Source = null;
                        GemStat2.Visibility = Visibility.Visible;
                        GemStat2.Children.Clear();
                        hasGems = true;
                    }
                    GemImage2.Source = Icons.AnIcon(ItemInstance.Gem2.IconPath);
                    foreach (string s in Calculations.GetRelevantStats(ItemInstance.Gem2.Stats).ToString().Split(','))
                    {
                        string r = s.Contains("Armor Penetration") ? s.Replace("Armor Penetration", "ArP") : s;
                        TextBlock t = new TextBlock();
                        t.Text = r.Trim();
                        GemStat2.Children.Add(t);
                    }
                    GemNamesLabel2.Text = "- " + ItemInstance.Gem2.Name;
                }
                if (ItemInstance.Gem3 != null)
                {
                    if (actualItem.SocketColor3 == ItemSlot.None)
                    {
                        GemColor3.Visibility = Visibility.Visible;
                        GemColor3.Background = new SolidColorBrush(ColorForGem(ItemSlot.Prismatic));
                        GemImage3.Visibility = Visibility.Visible;
                        GemImage3.Source = null;
                        GemStat3.Visibility = Visibility.Visible;
                        GemStat3.Children.Clear();
                        hasGems = true;
                    }
                    GemImage3.Source = Icons.AnIcon(ItemInstance.Gem3.IconPath);
                    foreach (string s in Calculations.GetRelevantStats(ItemInstance.Gem3.Stats).ToString().Split(','))
                    {
                        string r = s.Contains("Armor Penetration") ? s.Replace("Armor Penetration", "ArP") : s;
                        TextBlock t = new TextBlock();
                        t.Text = r.Trim();
                        GemStat3.Children.Add(t);
                    }
                    GemNamesLabel3.Text = "- " + ItemInstance.Gem3.Name;
                }
                if (actualItem != null && (!Item.GemMatchesSlot(itemInstance.Gem1, actualItem.SocketColor1) ||
                                    !Item.GemMatchesSlot(itemInstance.Gem2, actualItem.SocketColor2) ||
                                    !Item.GemMatchesSlot(itemInstance.Gem3, actualItem.SocketColor3)))
                    SocketBonusLabel.Foreground = new SolidColorBrush(Colors.Gray);
                else SocketBonusLabel.Foreground = new SolidColorBrush(Colors.Black);
            }
            #endregion

            #region Reforging
#if RAWR4
            if (ItemInstance != null && ItemInstance.Reforging != null && ItemInstance.Reforging.ReforgeAmount > 0)
            {
                ReforgingLabel.Text = string.Format("Reforge {0} {1} → {2}", ItemInstance.Reforging.ReforgeAmount, Extensions.SpaceCamel(ItemInstance.Reforging.ReforgeFrom.ToString()), Extensions.SpaceCamel(ItemInstance.Reforging.ReforgeTo.ToString()));
                ReforgingLabel.Visibility = Visibility.Visible;
            }
            else
            {
                ReforgingLabel.Visibility = Visibility.Collapsed;
            }
#endif
            #endregion

            #region Location Section
            // the actualItem.Id check to make sure it's an item, not a reforging or something
            if (actualItem != null && actualItem.Id > 0 && actualItem.Id < (int)AvailableItemIDModifiers.Reforges) {
                if (/*actualItem.LocationInfo != null &&*/
                    /*actualItem.LocationInfo.Count > 0 &&*/
                    ((LocationLabel.Text = actualItem.GetFullLocationDesc) != ""))
                {
                    LocationLabel.Visibility = Visibility.Visible;
                } else { LocationLabel.Visibility = Visibility.Collapsed; }
            } else { LocationLabel.Visibility = Visibility.Collapsed; }
            #endregion

            #region Additional Items, like in Build Upgrade List
            ItemsGrid.Children.Clear();
            ItemsGrid.RowDefinitions.Clear();
            if (CharacterItems == null || CharacterItems.Length == 0) ItemsGrid.Visibility = Visibility.Collapsed;
            else
            {
                ItemsGrid.Visibility = Visibility.Visible;
                int row = 0;
                foreach (ItemInstance characterItem in CharacterItems)
                {
                    ItemsGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                    if (characterItem == null) continue;
                    Image iconImage = new Image();
                    iconImage.Style = Resources["SmallIconStyle"] as Style;
                    iconImage.Source = Icons.AnIcon(characterItem.Item.IconPath);
                    Grid.SetColumn(iconImage, 0);
                    Grid.SetRow(iconImage, row);
                    ItemsGrid.Children.Add(iconImage);

                    if (characterItem.Gem1Id > 0)
                    {
                        Image gem1Image = new Image();
                        gem1Image.Style = Resources["SmallIconStyle"] as Style;
                        gem1Image.Source = Icons.AnIcon(characterItem.Gem1.IconPath);
                        Grid.SetColumn(gem1Image, 1);
                        Grid.SetRow(gem1Image, row);
                        ItemsGrid.Children.Add(gem1Image);
                    }

                    if (characterItem.Gem2Id > 0)
                    {
                        Image gem2Image = new Image();
                        gem2Image.Style = Resources["SmallIconStyle"] as Style;
                        gem2Image.Source = Icons.AnIcon(characterItem.Gem2.IconPath);
                        Grid.SetColumn(gem2Image, 2);
                        Grid.SetRow(gem2Image, row);
                        ItemsGrid.Children.Add(gem2Image);
                    }

                    if (characterItem.Gem3Id > 0)
                    {
                        Image gem3Image = new Image();
                        gem3Image.Style = Resources["SmallIconStyle"] as Style;
                        gem3Image.Source = Icons.AnIcon(characterItem.Gem3.IconPath);
                        Grid.SetColumn(gem3Image, 3);
                        Grid.SetRow(gem3Image, row);
                        ItemsGrid.Children.Add(gem3Image);
                    }

                    TextBlock nameText = new TextBlock();
                    if (characterItem.EnchantId > 0) nameText.Text = string.Format("{0} ({1})", characterItem.Item.Name, characterItem.Enchant.Name);
                    else nameText.Text = string.Format("{0}", characterItem.Item.Name);
                    nameText.Foreground = new SolidColorBrush(ColorForQuality(characterItem.Item.Quality));
                    Grid.SetColumn(nameText, 4);
                    Grid.SetRow(nameText, row);
                    ItemsGrid.Children.Add(nameText);

                    row++;
                }
            }
            #endregion
        }

        public void Show(UIElement relativeTo) { Show(relativeTo, 0, 0); }
        public void Show(UIElement relativeTo, double offsetX, double offsetY)
        {
#if SILVERLIGHT
            try
            {
                GeneralTransform gt = relativeTo.TransformToVisual((UIElement)this.Parent);
                Point offset = gt.Transform(new Point(offsetX, offsetY));
                ItemPopup.VerticalOffset = offset.Y;
                ItemPopup.HorizontalOffset = offset.X;
                ItemPopup.IsOpen = true;

                ItemGrid.Measure(App.Current.RootVisual.DesiredSize);

                GeneralTransform transform = relativeTo.TransformToVisual(App.Current.RootVisual);
                // Lets make sure that we don't clip from the bottom
                double distBetweenBottomOfPopupAndBottomOfWindow =
                    App.Current.RootVisual.RenderSize.Height - offsetY -
                    transform.Transform(new Point(0, ItemGrid.DesiredSize.Height)).Y;
                if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
                {
                    ItemPopup.VerticalOffset += distBetweenBottomOfPopupAndBottomOfWindow;
                }
                // Lets make sure that we don't clip from the right side
                double distBetweenRightSideOfPopupAndBottomOfWindow =
                    App.Current.RootVisual.RenderSize.Width - offsetX -
                    transform.Transform(new Point(ItemGrid.DesiredSize.Width, 0)).X;
                if (distBetweenRightSideOfPopupAndBottomOfWindow < 0)
                {
                    ItemPopup.HorizontalOffset += distBetweenRightSideOfPopupAndBottomOfWindow;
                }
            }
            catch (ArgumentException)
            {
                // Value does not fall within the expected range
                // apparently happens if you call while it's still loading the visual tree or something
            }
#else
            ItemPopup.PlacementTarget = relativeTo;
            ItemPopup.PlacementRectangle = new Rect(0, offsetY, offsetX, relativeTo.RenderSize.Height);
            ItemPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.Right;
            ItemPopup.IsOpen = true;
#endif
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
