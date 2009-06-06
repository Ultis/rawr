using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;

namespace Rawr.Silverlight.Web
{
    public partial class Armory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string my = HttpContext.Current.Request.Url.ToString();
            string url = string.Format("http://www.wowarmory.com/{0}", my.Substring(my.IndexOf('?')+1).Replace('*','?'));

            HttpWebRequest loHttp = (HttpWebRequest)WebRequest.Create(url);
            loHttp.Timeout = 10000;
            loHttp.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
            HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
            Encoding enc = Encoding.GetEncoding(1252);  // Windows default Code Page
            StreamReader loResponseStream = new StreamReader(loWebResponse.GetResponseStream(), enc);
            string lcHtml = loResponseStream.ReadToEnd();
            loWebResponse.Close();
            loResponseStream.Close();

            MainLiteral.Text = lcHtml;
        }
    }
}
