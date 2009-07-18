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

namespace Rawr.Silverlight
{
    public partial class ItemBrowser : ChildWindow
    {

        private ItemEditor itemEditor;
        public ItemEditor ItemEditor
        {
            get
            {
                if (itemEditor == null) itemEditor = new ItemEditor();
                return itemEditor;
            }
        }

        public ItemBrowser()
        {
            InitializeComponent();
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
            if (!string.IsNullOrEmpty(NameText.Text)) items = items.Where(i =>
                i.Name.ToLower().Contains(NameText.Text.ToLower()));

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

			PagedCollectionView itemsPCV = new PagedCollectionView(items);
			itemsPCV.GroupDescriptions.Add(new PropertyGroupDescription("Slot"));
            ItemGrid.ItemsSource = itemsPCV;
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Item i = ItemGrid.SelectedItem as Item;
            if (i != null)
            {
                ItemEditor.CurrentItem = i;
                ItemEditor.Show();
                ItemEditor.Closed += new EventHandler(ItemEditor_Closed);
            }
        }

        private void ItemEditor_Closed(object sender, EventArgs e)
        {
            if (((ChildWindow)sender).DialogResult.GetValueOrDefault(false))
            {
                UpdateItemList();
            }
        }

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
                Item.LoadFromId(i.Id, true, false, false);
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
    }
}

