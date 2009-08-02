using System;
using System.Web;
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
            string[] array = my.Substring(my.IndexOf('?') + 1).Split('*');

            string url = "";

            if (array.Length == 2 || array.Length == 3)
            {
                if (array.Length == 2) url = string.Format("http://www.wowarmory.com/{0}?{1}", array[0], array[1]);
                else url = string.Format("http://{0}.wowarmory.com/{1}?{2}", array[0], array[1], array[2]);

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
}
