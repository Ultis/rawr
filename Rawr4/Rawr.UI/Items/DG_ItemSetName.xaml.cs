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
    public partial class DG_ItemSetName : ChildWindow
    {
        public static void ShowDialog(Character character, ItemSet thenewItemSet, EventHandler callback)
        {
            DG_ItemSetName window = new DG_ItemSetName(character, thenewItemSet);
            if (callback != null) window.Closed += callback;
            window.Show();
        }
        
        public DG_ItemSetName(Character character, ItemSet thenewItemSet)
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
#endif

            Character = character;
            newItemSet = thenewItemSet;
        }

        public ItemSet newItemSet { get; set; }

        private Character _character;
        private Character Character {
            get { return _character; }
            set {
                _character = value;
                CB_Name.Items.Clear();
                foreach (ItemSet IS in _character.GetItemSetList())
                {
                    CB_Name.Items.Add(IS.Name);
                }
            }
        }

        public String NewSetName {
            get
            {
                string retVal = "";
                //
                if (TB_Name.Text != "") { retVal = TB_Name.Text; }
                else { retVal = (CB_Name.SelectedItem as String); }
                //
                return retVal;
            }
        }

        private bool isChanging = false;

        private void TB_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isChanging) return;
            isChanging = true;
            CB_Name.SelectedIndex = -1;
            OKButton.IsEnabled = (TB_Name.Text != "");
            isChanging = false;
        }

        private void CB_Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isChanging) return;
            isChanging = true;
            TB_Name.Text = "";
            OKButton.IsEnabled = (CB_Name.SelectedIndex != -1);
            isChanging = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

