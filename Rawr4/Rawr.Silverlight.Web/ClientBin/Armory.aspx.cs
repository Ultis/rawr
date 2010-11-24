using System;
using System.Web;
using System.Net;
using System.Text;
using System.IO;

namespace Rawr.Silverlight.Web
{
    public partial class Armory : System.Web.UI.Page
    {
        private readonly string[] URL = new string[] { "http://{0}.wowhead.com/item={1}&xml" };

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string my = HttpContext.Current.Request.Url.ToString();
                string[] array = my.Substring(my.IndexOf('?') + 1).Split('*');
                object[] obj = new object[array.Length - 1];
                Array.Copy(array, 1, obj, 0, obj.Length);

                string url = string.Format(URL[int.Parse(array[0])], obj);

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
            catch
            {
            }
        }
    }
}
