using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace Rawr.Warlock
{
    public partial class TalentIcon : UserControl
    {
        //0 = class, 1=tree, 2=talentname - all lowercase
        private string _baseAddress = @"http://www.worldofwarcraft.com/shared/global/talents/{0}/images/{1}/{2}.jpg";

        private string _basePath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + @"\talentIcons\";
        private Bitmap _icon;
        private ToolTip _tt;
        private string _name;
        public TalentIcon()
        {
            InitializeComponent();
            panel1.Resize += new EventHandler(panel1_Resize);
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

        public TalentIcon(TalentItem ti, Character.CharacterClass charClass) : this()
        {
            
            Talent = ti;
            _name = ti.Name;
            CharClass = charClass;
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
            getIcon(ti, charClass);
            panel1.Width = _icon.Width;
            panel1.Height = _icon.Height;
            
        }

        void panel1_Resize(object sender, EventArgs e)
        {
            this.Width = panel1.Width + 2;
            this.Height = panel1.Height + 20;
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

        private string getRemoteFilename(TalentItem ti, Character.CharacterClass charclass)
        {
            return string.Format(_baseAddress, charclass.ToString(), ti.Tree, ti.Name).ToLower().Replace(" ", "");
        }

        private string getFilename(TalentItem ti, Character.CharacterClass charclass)
        {
            return (charclass.ToString().ToLower() + ti.Tree.ToLower() + ti.Name.ToLower() + ".jpg").Replace(" ", "");
            
        }

        private void getIcon(TalentItem ti, Character.CharacterClass charclass)
        {
            if (_icon == null)
            {
                string fullfile = _basePath + getFilename(ti, charclass);
                if (!File.Exists(fullfile))
                {
                    string remoteFile = getRemoteFilename(ti, charclass);
					WebRequestWrapper wrw = new WebRequestWrapper();
                    wrw.DownloadIcon(remoteFile, fullfile);
                }
                _icon = new Bitmap(fullfile);
            }

            
        }
    }
}
