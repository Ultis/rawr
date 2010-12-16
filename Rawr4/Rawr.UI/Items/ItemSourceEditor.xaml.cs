using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class ItemSourceEditor : ChildWindow
    {
        public ItemSourceEditor(Item itemToEdit)
        {
            InitializeComponent();
            ItemToEdit = itemToEdit;
            GenerateList();
        }

        private Item _ItemToEdit = null;
        private Item ItemToEdit {
            get { return _ItemToEdit; }
            set {
                _ItemToEdit = value;
                LB_ItemName.Text = ItemToEdit.Name;
                ItemSources = new ItemLocationList(ItemToEdit.LocationInfo);
            }
        }

        private ItemLocationList _ItemSources;
        public ItemLocationList ItemSources
        {
            get { return _ItemSources; }
            set {
                _ItemSources = value;
                GenerateList();
            }
        }

        private ItemSourceEditorChild childwindow = null;

        private void BT_Add_Click(object sender, RoutedEventArgs e)
        {
            childwindow = new ItemSourceEditorChild();
            childwindow.Closed += new EventHandler(BT_Add_Closed);
            childwindow.Show();
        }
        private void BT_Add_Closed(object sender, EventArgs e)
        {
            if (childwindow.DialogResult.GetValueOrDefault(false)) {
                ItemSources.Add(childwindow.NewSource);
            }
            childwindow = null;
            GenerateList();
            EnableDisableButtons();
        }

        private void BT_Edit_Click(object sender, RoutedEventArgs e)
        {
            childwindow = new ItemSourceEditorChild(TheListBox.SelectedItem as ItemLocation);
            childwindow.Closed += new EventHandler(BT_Edit_Closed);
            childwindow.Show();
        }
        private void BT_Edit_Closed(object sender, EventArgs e)
        {
            if (childwindow.DialogResult.GetValueOrDefault(false))
            {
                ItemSources[TheListBox.SelectedIndex] = childwindow.NewSource;
            }
            childwindow = null;
            GenerateList();
            EnableDisableButtons();
        }

        private void BT_Delete_Click(object sender, RoutedEventArgs e)
        {
            int index = TheListBox.SelectedIndex;
            if (index == -1) { EnableDisableButtons(); return; }
            TheListBox.Items.RemoveAt(index);
            ItemSources.RemoveAt(index); // push change back into the list
            EnableDisableButtons();
        }

        private void GenerateList() {
            TheListBox.Items.Clear();
            foreach (ItemLocation il in ItemSources)
            {
                if (il == null) { continue; }
                TheListBox.Items.Add(il);
            }
            //
            EnableDisableButtons();
        }

        private void EnableDisableButtons() {
            if (TheListBox.SelectedIndex != -1) {
                BT_Edit.IsEnabled = true;
                BT_Delete.IsEnabled = true;
            } else {
                BT_Edit.IsEnabled = false;
                BT_Delete.IsEnabled = false;
            }
            BT_Add.IsEnabled = TheListBox.Items.Count < 3;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) { this.DialogResult = true; }
        private void CancelButton_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; }

        private void TheListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //
            EnableDisableButtons();
        }
    }
}

