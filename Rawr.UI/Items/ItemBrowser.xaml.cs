using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Rawr.UI
{
    public partial class ItemBrowser : ChildWindow
    {

        public ItemBrowser()
        {
            InitializeComponent();

            if (Rawr.Properties.GeneralSettings.Default.UseLargeViewItemBrowser) { BT_LargeView_Click(null, null); }
            if (Rawr.Properties.GeneralSettings.Default.UseRegexInItemBrowser) { CK_UseRegex.IsChecked = true; }

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            ItemCache.Instance.ItemsChanged += new EventHandler(Instance_ItemsChanged);
            UpdateItemList();
        }

        private void Instance_ItemsChanged(object sender, EventArgs e)
        {
            UpdateItemList();
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void UpdateItemList()
        {
            IEnumerable<Item> items = ItemCache.AllItems;
            if (!string.IsNullOrEmpty(NameText.Text)) {
                if (CK_UseRegex.IsChecked.GetValueOrDefault(false)) {
                    try {
                        Regex regex = new Regex(NameText.Text.ToLower());
                        items = items.Where(i =>
                            (regex.IsMatch(i.Name.ToLower())
                            || regex.IsMatch(i.Id.ToString().ToLower())
                            || regex.IsMatch(i.GetFullLocationDesc.ToLower())
                            )
                        );
                        NameText.BorderBrush = new SolidColorBrush(Colors.Gray);
                    }catch(Exception){
                        NameText.BorderBrush = new SolidColorBrush(Colors.Red);
                    }
                } else {
                    items = items.Where(i =>
                        (i.Name.ToLower().Contains(NameText.Text.ToLower())
                        || i.Id.ToString().ToLower().Contains(NameText.Text.ToLower())
                        || i.GetFullLocationDesc.ToLower().Contains(NameText.Text.ToLower())
                        )
                    );
                    NameText.BorderBrush = new SolidColorBrush(Colors.Gray);
                }
            } else {
                NameText.BorderBrush = new SolidColorBrush(Colors.Gray);
            }

            if (QualityList.SelectedItems.Count > 0)
            {
                List<ItemQuality> qualities = new List<ItemQuality>();
                foreach (ListBoxItem lbi in QualityList.SelectedItems)
                {
                    qualities.Add((ItemQuality)Enum.Parse(typeof(ItemQuality), lbi.Content.ToString(), false));
                    items = items.Where(i => qualities.Contains(i.Quality));
                }
            }

            if (TypeList.SelectedItems.Count > 0)
            {
                List<ItemType> types = new List<ItemType>();
                foreach (ListBoxItem lbi in TypeList.SelectedItems)
                {
                    types.Add((ItemType)Enum.Parse(typeof(ItemType), lbi.Content.ToString(), false));
                    items = items.Where(i => types.Contains(i.Type));
                }
            }

            if (BindList.SelectedItems.Count > 0)
            {
                List<BindsOn> binds = new List<BindsOn>();
                foreach (ListBoxItem lbi in BindList.SelectedItems)
                {
                    binds.Add((BindsOn)Enum.Parse(typeof(BindsOn), lbi.Content.ToString().Replace(" ", ""), false));
                    items = items.Where(i => binds.Contains(i.Bind));
                }
            }

            if (SlotList.SelectedItems.Count > 0)
            {
                List<ItemSlot> slots = new List<ItemSlot>();
                foreach (ListBoxItem lbi in SlotList.SelectedItems)
                {
                    slots.Add((ItemSlot)Enum.Parse(typeof(ItemSlot), lbi.Content.ToString().Replace(" ", ""), false));
                    items = items.Where(i => slots.Contains(i.Slot));
                }
            }

            if (ClassCombo.SelectedIndex > 0)
            {
                string reqClass = ((CharacterClass)ClassCombo.SelectedIndex).ToString();
                items = items.Where(i => string.IsNullOrEmpty(i.RequiredClasses)
                    || i.RequiredClasses.Split('|').Contains(reqClass));
            }

            if (!string.IsNullOrEmpty(MinLevelText.Text))
            {
                try
                {
                    int min = int.Parse(MinLevelText.Text);
                    items = items.Where(i => i.ItemLevel >= min);
                }
                catch { }
            }

            if (!string.IsNullOrEmpty(MaxLevelText.Text))
            {
                try
                {
                    int max = int.Parse(MaxLevelText.Text);
                    items = items.Where(i => i.ItemLevel <= max);
                }
                catch { }
            }

#if SILVERLIGHT
            PagedCollectionView itemsPCV = new PagedCollectionView(items);
            itemsPCV.GroupDescriptions.Add(new PropertyGroupDescription("Slot"));
            ItemGrid.ItemsSource = itemsPCV;            
#else
            // TODO: implement paging in WPF
            ItemGrid.ItemsSource = items;
#endif
            //if (MainPage.Instance != null && MainPage.Instance.Character != null) { MainPage.Instance.Character.OnCalculationsInvalidated(); }
            //ItemCache.OnItemsChanged();
        }

        #region Add Items
        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            EnterId EnterId = new EnterId();
            EnterId.Closed += new EventHandler(EnterId_Closed);
            EnterId.Show();
        }

        void EnterId_Closed(object sender, EventArgs e)
        {
            EnterId EnterId = sender as EnterId;
            if (EnterId.DialogResult.GetValueOrDefault(false))
            {
                //_changingItemCache = true;
                try
                {
                    //WebRequestWrapper.ResetFatalErrorIndicator();
                    if (EnterId.Values.Count > 0)
                    {
                        foreach (int i in EnterId.Values) {
                            if (i > 0) {
                                AddItemById(i, false, true, true && EnterId.CK_PTR.IsChecked.GetValueOrDefault(false));
                            }
                        }
                    }
                    else
                    {
                        int itemId = EnterId.Value;
                        bool useWowhead = EnterId.CK_WH.IsChecked.GetValueOrDefault(false);
                        if (itemId > 0)
                        {
                            AddItemById(EnterId.Value, !useWowhead, useWowhead, useWowhead && EnterId.CK_PTR.IsChecked.GetValueOrDefault(false));
                        }
                        else
                        {
                            AddItemByName(EnterId.ItemName, true/*!useWowhead*/, useWowhead, useWowhead && EnterId.CK_PTR.IsChecked.GetValueOrDefault(false));
                        }
                    }
                }
                finally
                {
                    //_changingItemCache = false;
                }
            }
        }

        private void AddItemByName(string name, bool useArmory, bool useWowhead, bool usePTR)
        {
            // ignore empty strings
            if (name.Length <= 0) return;
            else { AddItemByName(name, 0, useWowhead, usePTR); }
        }

        private void AddItemByName(string name, int armoryId, bool useWowhead, bool usePTR)
        {
            Item newItem = null;

            // try the armory (if requested)
            if (armoryId > 0)
            {
                newItem = Item.LoadFromId(armoryId, true, true, false, false && usePTR);
            }

            // try wowhead (if requested)
            // TODO add back when we add wowhead support
            /*if ((newItem == null) && useWowhead)
            {
                // make sure we don't get some bad input that is going to mess with our gem info passing
                if (!name.Contains("."))
                {
                    // need to add + where the spaces are
                    string wowhead_name = name.Replace(' ', '+');
                    // we can now pass it through the normal URI
                    newItem = Wowhead.GetItem(wowhead_name + ".0.0.0", true);
                    if (newItem != null) ItemCache.AddItem(newItem, true);
                }
            }*/

            if (newItem == null)
            {
                MessageBox.Show("Unable to load item: " + name + ".", "Item not found.", MessageBoxButton.OK);
            }
            else
            {
                UpdateItemList();
                // TODO this does not work, figure out how to select an item
                ItemGrid.SelectedItem = newItem;
            }
        }

        private void AddItemById(int id, bool useArmory, bool useWowhead, bool usePTR) { AddItemsById(new int[] { id }, useArmory, useWowhead, usePTR); }
        /// <summary>
        /// This override is down specifically for the load character from battle.net armory to ensure that missing items are loaded in
        /// </summary>
        public static void AddItemsById(int[] ids, bool useArmory, bool useWowhead)
        {
            foreach (int id in ids)
            {
                Item newItem = null;

                // try the armory (if requested)
                if (useArmory)
                {
                    newItem = Item.LoadFromId(id, true, true, false, false /*&& usePTR*/);
                }

                // try wowhead PTR
                /*if ((newItem == null) && useWowhead && usePTR)
                {
                    newItem = Item.LoadFromId(id, true, true, true, true);
                    if (newItem != null) ItemCache.AddItem(newItem, true);
                }*/
                // try wowhead if (armory failed) or (force wowhead) or (force wowhead and ptr failed)
                if ((newItem == null) /*&& useWowhead*/)
                {
                    newItem = Item.LoadFromId(id, true, true, true, false);
                    if (newItem != null) ItemCache.AddItem(newItem, true);
                }

                // We still can't find the item
                if (newItem == null)
                {
                    // TODO don't use message box
                    if (MessageBox.Show("Unable to load item " + id.ToString() + ". Would you like to create the item blank and type in the values yourself?", "Item not found. Create Blank?", MessageBoxButton.OK) == MessageBoxResult.OK)
                    {
                        newItem = new Item("New Item " + id.ToString(), ItemQuality.Epic, ItemType.None, id, "temp", ItemSlot.Head, string.Empty, false,
                            new Stats(), new Stats(), ItemSlot.None, ItemSlot.None, ItemSlot.None, 0, 0, ItemDamageType.Physical, 0f, string.Empty);
                        ItemCache.AddItem(newItem);
                        ItemCache.OnItemsChanged();
                    }
                }
                /*else
                {
                    UpdateItemList();
                    // TODO this does not work, figure out how to select an item
                    ItemGrid.SelectedItem = newItem;
                }*/
            }
        }

        /// <summary>
        /// This override is the normally used one
        /// </summary>
        public void AddItemsById(int[] ids, bool useArmory, bool useWowhead, bool usePTR)
        {
            foreach (int id in ids)
            {
                Item newItem = null;

                // try the armory (if requested)
                if (useArmory)
                {
                    newItem = Item.LoadFromId(id, true, true, false, false && usePTR);
                }

                // try wowhead PTR
                if ((newItem == null) && useWowhead && usePTR)
                {
                    newItem = Item.LoadFromId(id, true, true, true, true);
                    if (newItem != null) ItemCache.AddItem(newItem, true);
                }
                // try wowhead if (armory failed) or (force wowhead) or (force wowhead and ptr failed)
                if ((newItem == null) /*&& useWowhead*/)
                {
                    newItem = Item.LoadFromId(id, true, true, true, false);
                    if (newItem != null) ItemCache.AddItem(newItem, true);
                }

                // We still can't find the item
                if (newItem == null)
                {
                    // TODO don't use message box
                    if (MessageBox.Show("Unable to load item " + id.ToString() + ". Would you like to create the item blank and type in the values yourself?", "Item not found. Create Blank?", MessageBoxButton.OK) == MessageBoxResult.OK)
                    {
                        newItem = new Item("New Item " + id.ToString(), ItemQuality.Epic, ItemType.None, id, "temp", ItemSlot.Head, string.Empty, false,
                            new Stats(), new Stats(), ItemSlot.None, ItemSlot.None, ItemSlot.None, 0, 0, ItemDamageType.Physical, 0f, string.Empty);
                        ItemCache.AddItem(newItem);
                    }
                }
                else
                {
                    UpdateItemList();
                    // TODO this does not work, figure out how to select an item
                    ItemGrid.SelectedItem = newItem;
                    ItemCache.OnItemsChanged();
                }
            }
        }
        #endregion

        #region Edit Items
        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ItemEditor ItemEditor = new UI.ItemEditor();
            ItemEditor.Closed += new EventHandler(ItemEditor_Closed);
            Item i = ItemGrid.SelectedItem as Item;
            if (i != null) {
                ItemEditor.CurrentItem = i;
                ItemEditor.Show();
            }
        }

        private void ItemEditor_Closed(object sender, EventArgs e)
        {
            if ((sender as ItemEditor).DialogResult.GetValueOrDefault(false))
            {
                UpdateItemList();
            }
        }
        #endregion

        private void ItemGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Item i = ItemGrid.SelectedItem as Item;
            if (i == null)
            {
                EditButton.IsEnabled = false;
                RefreshButton.IsEnabled = false;
            }
            else
            {
                EditButton.IsEnabled = true;
                RefreshButton.IsEnabled = true;
            }
        }

        private void NameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateItemList();
        }

        private void RefreshButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Item i = ItemGrid.SelectedItem as Item;
            if (i != null)
            {
                Item.LoadFromId(i.Id, true, false, true, false);
            }
        }

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateItemList();
        }

        private void LevelChanged(object sender, TextChangedEventArgs e)
        {
            UpdateItemList();
        }

        private void CK_UseRegex_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Rawr.Properties.GeneralSettings.Default.UseRegexInItemBrowser = CK_UseRegex.IsChecked.GetValueOrDefault(false);
            UpdateItemList();
        }

        private void BT_LargeView_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 1000;
            this.Height = 750;
            BT_LargeView.Visibility = Visibility.Collapsed;
            BT_SmallView.Visibility = Visibility.Visible;
            // TODO: Find a way to re-center the window
            Rawr.Properties.GeneralSettings.Default.UseLargeViewItemBrowser = true;
        }

        private void BT_SmallView_Click(object sender, RoutedEventArgs e)
        {
            this.Width = 600;
            this.Height = 400;
            BT_LargeView.Visibility = Visibility.Visible;
            BT_SmallView.Visibility = Visibility.Collapsed;
            // TODO: Find a way to re-center the window
            Rawr.Properties.GeneralSettings.Default.UseLargeViewItemBrowser = false;
        }
    }
}

