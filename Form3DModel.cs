using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class Form3DModel : Form
    {
        public Form3DModel()
        {
            InitializeComponent();
        }

        public void ShowWowhead3DModelJava(Character character, bool maleModel)
        {
            bool missingDisplayID = false;
            webBrowser1.Navigate("about:blank");
            while (webBrowser1.Document.Body == null) Application.DoEvents(); // loop to ensure that wb loads blank document

            StringBuilder URL = new StringBuilder("<html><head><title>Wowhead 3D Character Model in Java</title></head><body>");
            URL.Append("<applet id=\"3dviewer-java\" code=\"org.jdesktop.applet.util.JNLPAppletLauncher\" ");
            URL.Append("width=\"600\" height=\"400\" ");
            URL.Append("archive=\"http://static.wowhead.com/modelviewer/applet-launcher.jar,");
            URL.Append("http://download.java.net/media/jogl/builds/archive/jsr-231-webstart-current/jogl.jar,");
            URL.Append("http://download.java.net/media/gluegen/webstart/gluegen-rt.jar,");
            URL.Append("http://download.java.net/media/java3d/webstart/release/vecmath/latest/vecmath.jar,");
            URL.Append("http://static.wowhead.com/modelviewer/ModelView510.jar\">");
            URL.Append("<param name=\"jnlp_href\" value=\"http://static.wowhead.com/modelviewer/ModelView.jnlp\">");
            URL.Append("<param name=\"codebase_lookup\" value=\"false\">");
            URL.Append("<param name=\"cache_option\" value=\"no\">");
            URL.Append("<param name=\"subapplet.classname\" value=\"modelview.ModelViewerApplet\">");
            URL.Append("<param name=\"subapplet.displayname\" value=\"Model Viewer Applet\">");
            URL.Append("<param name=\"progressbar\" value=\"true\">");
            URL.Append("<param name=\"jnlpNumExtensions\" value=\"1\">");
            URL.Append("<param name=\"jnlpExtension1\" value=\"http://download.java.net/media/jogl/builds/archive/jsr-231-webstart-current/jogl.jnlp\">");
            URL.Append("<param name=\"model\" value=\"");
            URL.Append(character.Race.ToString().ToLower());
            URL.Append(maleModel ? "male" : "female");
            URL.Append("\">");
            URL.Append("<param name=\"modelType\" value=\"16\">");
            URL.Append("<param name=\"equipList\" value=\"");
            foreach (ItemInstance item in character.GetItems())
            {
                if (item != null && (item.Slot != ItemSlot.Neck && item.Slot != ItemSlot.Shirt && item.Slot != ItemSlot.Tabard &&
                    item.Slot != ItemSlot.Trinket && item.Slot != ItemSlot.Finger && item.Slot != ItemSlot.Projectile && item.Slot != ItemSlot.ProjectileBag ||
                    (item.Slot == ItemSlot.Ranged && (item.Type == ItemType.Bow || item.Type == ItemType.Crossbow || item.Type == ItemType.Thrown || item.Type == ItemType.Wand))))
                {
                    if (item.DisplayId == 0 || item.DisplaySlot == 0)
                        missingDisplayID = true;
                    URL.Append(item.DisplaySlot + ",");
                    URL.Append(item.DisplayId + ",");
                }
            }
            URL.Remove(URL.Length - 1, 1); // removes trailing ,
            URL.Append("\">");
            URL.Append("<param name=\"bgColor\" value=\"#181818\">");
            URL.Append("</applet></body></html>");
            webBrowser1.Document.Write(URL.ToString());
            if (missingDisplayID)
                MessageBox.Show("One or more of your equipped items has a missing Display Information.\r\nPlease update your item cache from Wowhead to try to fix this problem.");
            else
                this.Show();
        }
    }
}
