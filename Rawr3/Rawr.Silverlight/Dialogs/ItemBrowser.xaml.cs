using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Data;

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

            ItemGrid.ItemsSource = items;
            ItemGrid.GroupDescriptions.Add(new PropertyGroupDescription("Slot"));
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
            if (i == null) EditButton.IsEnabled = false;
            else EditButton.IsEnabled = true;
        }

        private void NameText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateItemList();
        }
    }
}

