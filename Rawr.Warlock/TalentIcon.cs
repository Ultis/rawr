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

        public TalentIcon()
        {
            InitializeComponent();
        }

        public TalentIcon(TalentItem ti, Character.CharacterClass charClass) : this()
        {
            Talent = ti;
            CharClass = charClass;
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
                panel1.BackgroundImage = new Bitmap(getIcon(Talent, CharClass));
                label1.Text = Talent.Name + "(" + Talent.PointsInvested + "/" + Talent.Rank + ")";
            }
        }

        private string getRemoteFilename(TalentItem ti, Character.CharacterClass charclass)
        {
            return string.Format(_baseAddress, charclass.ToString(), ti.Tree, ti.Name).ToLower();
        }

        private string getFilename(TalentItem ti, Character.CharacterClass charclass)
        {
            return charclass.ToString().ToLower() + ti.Tree.ToLower() + ti.Name.ToLower() + ".jpg";
            
        }

        private string getIcon(TalentItem ti, Character.CharacterClass charclass)
        {
            string fullfile = _basePath + getFilename(ti, charclass);
            if (!File.Exists(fullfile))
            {
                string remoteFile = getRemoteFilename(ti, charclass);
                WebClient Client = new WebClient();
                Client.DownloadFile(remoteFile, fullfile);
            }
            return fullfile;
        }
    }
}
