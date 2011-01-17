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
            TheList.Add(s);
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
    }
}

