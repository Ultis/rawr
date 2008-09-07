using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Rogue {
    public partial class TalentIcon : UserControl {
        private Bitmap _icon;
        private ToolTip _tt;
        private string _name;
        private GroupBox _gb;

        public TalentIcon() {
            InitializeComponent();
            panelIcon.Resize += new EventHandler(panelIcon_Resize);
            panelIcon.MouseHover += new EventHandler(panelIcon_MouseHover);

        }

        void panelIcon_MouseHover(object sender, EventArgs e) {
            if (_tt == null) {
                _tt = new ToolTip();
            }
            _tt.Show(_name, panelIcon);
        }

        public TalentIcon(TalentItem ti, Character.CharacterClass charClass, GroupBox gb)
            : this() {

            Talent = ti;
            _name = ti.Name;
            _gb = gb;
            CharClass = charClass;
            getIcon(ti, charClass);

            if (_icon == null) {
                _icon = new Bitmap(43, 45);
                for (int i = 0; i < 43; i++) {
                    _icon.SetPixel(i, i, Color.Red);
                    _icon.SetPixel(43 - i - 1, i, Color.Red);
                }
            }

            panelIcon.Width = _icon.Width;
            panelIcon.Height = _icon.Height;
        }

        void panelIcon_Resize(object sender, EventArgs e) {
            this.Width = panelIcon.Width + 2;
            this.Height = panelIcon.Height + 20;
        }

        public Character.CharacterClass CharClass {
            get;
            set;
        }

        public TalentItem Talent {
            get;
            set;
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            if (Talent != null) {
                panelIcon.BackgroundImage = _icon;
                labelPoints.Text = "(" + Talent.PointsInvested + "/" + Talent.Rank + ")";
            }
        }

        private void getIcon(TalentItem ti, Character.CharacterClass charclass) {
            string filePath = "";
            if (_icon == null) {
                WebRequestWrapper wrw = new WebRequestWrapper();
                try {
                    filePath = wrw.DownloadTalentIcon(charclass, ti.Tree.Replace(" ", ""), ti.Name.Replace(" ", ""));
                }
                catch (Exception e) {
                    MessageBox.Show("Error downloading talent icon for " + ti.Name + ": " + e.ToString());
                }
                if (!String.IsNullOrEmpty(filePath)) {
                    _icon = new Bitmap(filePath);
                }
            }


        }

        private void panelIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (Talent.PointsInvested < Talent.Rank) {
                    Talent.PointsInvested++;
                    this.Refresh();
                    refreshTree(true);
                }
            }
            else if (e.Button == MouseButtons.Right) {
                if (Talent.PointsInvested > 0) {
                    Talent.PointsInvested--;
                    this.Refresh();
                    refreshTree(false);
                }
            }
        }

        public Bitmap getIcon() {
            return _icon;
        }

        private void refreshTree(bool increase) {
            int points = Int32.Parse(_gb.Text.Substring(_gb.Text.IndexOf('(') + 1,
                                     _gb.Text.IndexOf(')') - 1 - _gb.Text.IndexOf('(')));
            string treeName = Talent.Tree;
            points = (increase ? points + 1 : points - 1);
            _gb.Text = treeName + " (" + points.ToString() + ")";
            _gb.Refresh();
        }
    }
}
