using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.DPSWarr
{
    public partial class DG_BossSitChanges : Form
    {
        #region Constructors
        public DG_BossSitChanges() { InitializeComponent(); }
        public DG_BossSitChanges(List<Stun> list)
        {
            InitializeComponent();
            Flag = Flags.Stun;
            StunList.AddRange((Stun[])(list.ToArray().Clone()));
            this.Text = this.Text.Replace("SitChanges", "Stuns");
            SetListBox();
        }
        public DG_BossSitChanges(List<Move> list)
        {
            InitializeComponent();
            Flag = Flags.Move;
            MoveList = list;
            this.Text = this.Text.Replace("SitChanges", "Moves");
            SetListBox();
        }
        public DG_BossSitChanges(List<Root> list)
        {
            InitializeComponent();
            Flag = Flags.Root;
            RootList.AddRange((Root[])(list.ToArray().Clone()));
            this.Text = this.Text.Replace("SitChanges", "Roots");
            SetListBox();
        }
        public DG_BossSitChanges(List<Fear> list)
        {
            InitializeComponent();
            Flag = Flags.Fear;
            FearList = list;
            this.Text = this.Text.Replace("SitChanges", "Fears");
            SetListBox();
        }
        public DG_BossSitChanges(List<Disarm> list)
        {
            InitializeComponent();
            Flag = Flags.Disarm;
            DisarmList = list;
            this.Text = this.Text.Replace("SitChanges", "Disarms");
            SetListBox();
        }
        #endregion
        #region Variables
        protected enum Flags {
            Stun = 0,
            Move,
            Root,
            Fear,
            Disarm,
        }
        Flags Flag = Flags.Stun;
        protected List<Stun> _StunList = null;
        public List<Stun> StunList
        {
            get { return _StunList ?? (_StunList = new List<Stun>()); }
            set { _StunList = value; }
        }
        protected List<Move> _MoveList = null;
        public List<Move> MoveList
        {
            get { return _MoveList ?? (_MoveList = new List<Move>()); }
            set { _MoveList = value; }
        }
        protected List<Root> _RootList = null;
        public List<Root> RootList
        {
            get { return _RootList ?? (_RootList = new List<Root>()); }
            set { _RootList = value; }
        }
        protected List<Fear> _FearList = null;
        public List<Fear> FearList
        {
            get { return _FearList ?? (_FearList = new List<Fear>()); }
            set { _FearList = value; }
        }
        protected List<Disarm> _DisarmList = null;
        public List<Disarm> DisarmList
        {
            get { return _DisarmList ?? (_DisarmList = new List<Disarm>()); }
            set { _DisarmList = value; }
        }
        #endregion
        #region Functions
        private void SetListBox()
        {
            LB_TheList.Items.Clear();
            switch (Flag) {
                case Flags.Move: {
                    foreach (Move s in MoveList)
                    {
                        string str = s.ToString();
                        LB_TheList.Items.Add(str);
                    }
                    break;
                }
                case Flags.Fear:
                    {
                        foreach (Fear s in FearList)
                        {
                            string str = s.ToString();
                            LB_TheList.Items.Add(str);
                        }
                        break;
                    }
                case Flags.Root:
                    {
                        foreach (Root s in RootList)
                        {
                            string str = s.ToString();
                            LB_TheList.Items.Add(str);
                        }
                        break;
                    }
                case Flags.Disarm:
                    {
                        foreach (Disarm s in DisarmList)
                        {
                            string str = s.ToString();
                            LB_TheList.Items.Add(str);
                        }
                        break;
                    }
                default:
                    {
                    foreach (Stun s in StunList)
                    {
                        string str = s.ToString();
                        LB_TheList.Items.Add(str);
                    }
                    break;
                }
            }
        }
        private void BT_Delete_Click(object sender, EventArgs e) {
            int index = LB_TheList.SelectedIndex;
            if (index == -1) { return; }
            switch (Flag) {
                case Flags.Move: { MoveList.RemoveAt(index); break; }
                case Flags.Fear: { FearList.RemoveAt(index); break; }
                case Flags.Root: { RootList.RemoveAt(index); break; }
                case Flags.Disarm: { DisarmList.RemoveAt(index); break; }
                default: { StunList.RemoveAt(index); break; }
            }
            SetListBox();
        }
        private void BT_Add_Click(object sender, EventArgs e) {
            switch (Flag)
            {
                case Flags.Move: {
                    Move s = new Move()
                    {
                        Frequency = (float)NUD_Freq.Value,
                        Duration = (float)NUD_Dur.Value,
                        Chance = ((float)NUD_Chance.Value) / 100f,
                        Breakable = CK_Breakable.Checked,
                    };
                    MoveList.Add(s);
                    break;
                }
                case Flags.Fear: {
                    Fear s = new Fear()
                    {
                        Frequency = (float)NUD_Freq.Value,
                        Duration = (float)NUD_Dur.Value,
                        Chance = ((float)NUD_Chance.Value) / 100f,
                        Breakable = CK_Breakable.Checked,
                    };
                    FearList.Add(s);
                    break;
                }
                case Flags.Root: {
                    Root s = new Root()
                    {
                        Frequency = (float)NUD_Freq.Value,
                        Duration = (float)NUD_Dur.Value,
                        Chance = ((float)NUD_Chance.Value) / 100f,
                        Breakable = CK_Breakable.Checked,
                    };
                    RootList.Add(s);
                    break;
                }
                case Flags.Disarm: {
                    Disarm s = new Disarm()
                    {
                        Frequency = (float)NUD_Freq.Value,
                        Duration = (float)NUD_Dur.Value,
                        Chance = ((float)NUD_Chance.Value) / 100f,
                        Breakable = CK_Breakable.Checked,
                    };
                    DisarmList.Add(s);
                    break;
                }
                default: {
                    Stun s = new Stun()
                    {
                        Frequency = (float)NUD_Freq.Value,
                        Duration = (float)NUD_Dur.Value,
                        Chance = ((float)NUD_Chance.Value) / 100f,
                        Breakable = CK_Breakable.Checked,
                    };
                    StunList.Add(s);
                    break;
                }
            }
            SetListBox();
        }
        #endregion
    }
}
