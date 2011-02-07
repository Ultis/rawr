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
    public partial class DG_BossBuffStates : ChildWindow
    {
        static DG_BossBuffStates()
        {
        }

        public DG_BossBuffStates()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            statControl.CurrentStats = new Stats();
        }

        public DG_BossBuffStates(List<BuffState> list)
        {
            InitializeComponent();
            TheList = list;
            statControl.CurrentStats = new Stats();
            SetListBox();
        }

        #region Variables
        protected List<BuffState> _TheList = null;
        public List<BuffState> TheList
        {
            get { return _TheList ?? (_TheList = new List<BuffState>()); }
            set { _TheList = value; }
        }
        #endregion

        private void SetListBox()
        {
            LB_TheList.Items.Clear();
            foreach (BuffState s in TheList)
            {
                string str = s.ToString();
                LB_TheList.Items.Add(str);
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
            BuffState s = new BuffState() {
                Name = TB_Name.Text,
                Frequency = (float)NUD_Freq.Value,
                Duration = (float)NUD_Dur.Value,
                Chance = ((float)NUD_Chance.Value) / 100f,
                Stats = statControl.CurrentStats,
            };
            if (isEditing) {
                // Affect your changes to the currently selected one
                isEditing = false;
                int index = LB_TheList.SelectedIndex;
                TheList.RemoveAt(LB_TheList.SelectedIndex);
                TheList.Insert(index, s);
            } else { TheList.Add(s); }
            SetListBox();
            statControl.CurrentStats = new Stats();
            statControl.StatsStack.Children.Clear();
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
                BuffState selected = LB_TheList.SelectedItem as BuffState;
                //
                TB_Name.Text = selected.Name; 
                NUD_Freq.Value = selected.Frequency;
                NUD_Dur.Value = selected.Duration;
                NUD_Chance.Value = selected.Chance * 100f;
                statControl.CurrentStats = selected.Stats;
                //
                isEditing = true;
            } else {
                // Reset the UI to a blank buff state
                TB_Name.Text = "";
                NUD_Freq.Value = 45;
                NUD_Dur.Value = 10 * 1000;
                NUD_Chance.Value = 100f;
                statControl.CurrentStats = new Stats();
                //
                isEditing = false;
            }
        }
    }
}

