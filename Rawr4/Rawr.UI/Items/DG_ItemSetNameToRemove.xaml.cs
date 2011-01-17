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
    public partial class DG_ItemSetNameToRemove : ChildWindow
    {
        public static void ShowDialog(Character character, EventHandler callback)
        {
            DG_ItemSetNameToRemove window = new DG_ItemSetNameToRemove(character);
            if (callback != null) window.Closed += callback;
            window.Show();
        }


        public DG_ItemSetNameToRemove(Character character)
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
#endif

            Character = character;
            CB_Name_SelectionChanged(null, null);
        }

        public String SetNameToRemove
        {
            get
            {
                string retVal = "";
                //
                retVal = (CB_Name.SelectedItem as String);
                //
                return retVal;
            }
        }


        private Character _character;
        private Character Character
        {
            get { return _character; }
            set
            {
                _character = value;
                CB_Name.Items.Clear();
                foreach(ItemSet IS in _character.GetItemSetList()){
                    CB_Name.Items.Add(IS.Name);
                }
            }
        }

        private void CB_Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OKButton.IsEnabled = (CB_Name.SelectedIndex != -1);
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

