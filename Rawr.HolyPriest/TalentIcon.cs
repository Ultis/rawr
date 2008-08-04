using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace Rawr.HolyPriest
{
    public partial class TalentIcon : UserControl
    {
        private Bitmap _icon;
        private ToolTip _tt;
        private string _name;
        private TalentForm parentForm;

        public TalentIcon(TalentForm parentForm)
        {
            this.parentForm = parentForm;
            InitializeComponent();
            panel1.Resize += new EventHandler(panel1_Resize);
            panel1.Click += new EventHandler(panel1_Click);
            panel1.MouseHover += new EventHandler(panel1_MouseHover);
     
        }

        void panel1_MouseHover(object sender, EventArgs e)
        {
            if (_tt == null)
            {
                _tt = new ToolTip();
            }
            _tt.Show(_name, panel1);
        }

        public TalentIcon(TalentForm parentForm, TalentItem ti, Character.CharacterClass charClass)
            : this(parentForm)
        {
            
            Talent = ti;
            _name = ti.Name;
            CharClass = charClass;
            getIcon(ti, charClass);
            panel1.Width = _icon.Width;
            panel1.Height = _icon.Height;
            
        }

        void panel1_Resize(object sender, EventArgs e)
        {
            this.Width = panel1.Width + 2;
            this.Height = panel1.Height + 20;
        }

        void panel1_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                if (Talent.PointsInvested < Talent.Rank)
                {
                    Talent.PointsInvested += 1;
                    parentForm.Character.OnItemsChanged();
                    Refresh();
                }
            }
            else
            {
                if (Talent.PointsInvested > 0)
                {
                    Talent.PointsInvested -= 1;
                    parentForm.Character.OnItemsChanged();
                    Refresh();
                }
            }
        }

        public Character.CharacterClass CharClass
        {
            get;
            set;
        }

        public TalentItem Talent
        {
            get;
            set;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Talent != null && CharClass != null)
            {
                panel1.BackgroundImage = _icon;
                label2.Text = "(" + Talent.PointsInvested + "/" + Talent.Rank + ")";
            }
        }

        private void getIcon(TalentItem ti, Character.CharacterClass charclass)
        {
            if (_icon == null)
            {
				WebRequestWrapper wrw = new WebRequestWrapper();
				string filePath = wrw.DownloadTalentIcon(charclass, ti.Tree.Replace(" ", ""), ti.Name.Replace(" ", "").Replace(":", ""));
				if (!String.IsNullOrEmpty(filePath))
				{
					_icon = new Bitmap(filePath);
				}
            }

            
        }
    }
}
