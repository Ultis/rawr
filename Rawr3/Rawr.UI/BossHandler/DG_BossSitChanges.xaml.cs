using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public partial class DG_BossSitChanges : ChildWindow
    {
        static DG_BossSitChanges()
        {
        }

        public DG_BossSitChanges()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif
        }

        public DG_BossSitChanges(List<Impedance> list, Flags f)
        {
            InitializeComponent();
            Flag = f;
            TheList = list;
            string s = "";
            switch (f)
            {
                case Flags.Stun: { s = "Stuns"; break; }
                case Flags.Move: { s = "Moves"; break; }
                case Flags.Fear: { s = "Fears"; break; }
                case Flags.Root: { s = "Roots"; break; }
                case Flags.Silence: { s = "Silences"; break; }
                default: { s = "Disarms"; break; } // Disarm
            }
            this.Title = ((string)(this.Title)).Replace("SitChanges", s);
            SetListBox();
        }

        #region Variables
        public enum Flags
        {
            Stun = 0,
            Move,
            Root,
            Fear,
            Silence,
            Disarm,
        }
        public Flags Flag = Flags.Stun;
        protected List<Impedance> _TheList = null;
        public List<Impedance> TheList
        {
            get { return _TheList ?? (_TheList = new List<Impedance>()); }
            set { _TheList = value; }
        }
        #endregion

        private void SetListBox()
        {
            LB_TheList.Items.Clear();
            foreach (Impedance s in TheList)
            {
                LB_TheList.Items.Add(s);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void BT_Add_Click(object sender, RoutedEventArgs e)
        {
            Impedance s = new Impedance()
            {
                Frequency = (float)NUD_Freq.Value,
                Duration = (float)NUD_Dur.Value,
                Chance = ((float)NUD_Chance.Value) / 100f,
                Breakable = (bool)CK_Breakable.IsChecked,
            };
            if (isEditing) {
                // Affect your changes to the currently selected one
                isEditing = false;
                int index = LB_TheList.SelectedIndex;
                TheList.RemoveAt(LB_TheList.SelectedIndex);
                TheList.Insert(index, s);
            } else { TheList.Add(s); }
            SetListBox();
        }

        private void BT_Delete_Click(object sender, RoutedEventArgs e)
        {
            int index = LB_TheList.SelectedIndex;
            if (index == -1) { return; }
            TheList.RemoveAt(index);
            SetListBox();
        }

        bool isEditing = false;
        private void LB_TheList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LB_TheList.SelectedIndex != -1) {
                Impedance selected = LB_TheList.SelectedItem as Impedance;
                //
                NUD_Freq.Value = selected.Frequency;
                NUD_Dur.Value = selected.Duration;
                NUD_Chance.Value = selected.Chance * 100f;
                CK_Breakable.IsChecked = selected.Breakable;
                //
                isEditing = true;
            } else {
                // Reset the UI to a blank buff state
                NUD_Freq.Value = 45;
                NUD_Dur.Value = 10 * 1000;
                NUD_Chance.Value = 100f;
                CK_Breakable.IsChecked = false;
                //
                isEditing = false;
            }
        }
    }
}

