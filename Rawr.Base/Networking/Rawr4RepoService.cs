using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Reflection;
#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Web;
#endif
using System.Text;

/*
 * This site pulls a regular character.xml file. Astrylian is owner and writer
 * of the site so he's just giving us data exactly like we need it
*/
namespace Rawr
{
    public class Rawr4RepoService
    {
        private const string URL_CHAR_REQ = "http://www.rawr4.com/~{0}{1}";
        private WebClient _webClient;
        private string _lastIdentifier;

        public Rawr4RepoService()
        {
            _webClient = new WebClient();
            _webClient.Encoding = Encoding.UTF8; // rawr4 uses UTF8 encoding
            _webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_webClient_DownloadStringCompleted);
            _webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(_webClient_UploadStringCompleted);
        }

        public event EventHandler<EventArgs<string>> ProgressChanged;
        public string Progress { get { return _progress; } set { _progress = value; if (ProgressChanged != null) { ProgressChanged(this, new EventArgs<string>(value)); } } }
        private string _progress = "Requesting Character...";

        public void CancelAsync() { _webClient.CancelAsync(); }

        private void _webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) { return; }
                if (e.Error != null) {
                    if (e.Error.Message.Contains("NotFound")) {
                        new Base.ErrorBox("Problem Getting Character from Rawr4 Repository",
                            "Your character file was not found on the server.",
                            "Try checking the name entered to look for").Show();
                    } else {
                        new Base.ErrorBox()
                        {
                            Title = "Problem Getting Character from Rawr4 Repository",
                            Function = "_webClient_DownloadStringCompleted(string input)",
                            TheException = e.Error,
                        }.Show();
                    }
                    return;
                }
                XDocument xdoc;
                using (StringReader sr = new StringReader(e.Result))
                {
                    xdoc = XDocument.Load(sr);
                }

                if (xdoc.Root.Name == "Character")
                {
                    Progress = "Parsing Character Data...";
                    Character character = Character.LoadFromXml(xdoc.Document.ToString());
                    Progress = "Complete!";
                    if (this.GetCharacterCompleted != null)
                        this.GetCharacterCompleted(this, new EventArgs<Character>(character));
                }
            } catch (Exception ex) {
                if (ex.Message.Contains("NotFound")) {
                    new Base.ErrorBox("Error Getting Character from the Rawr4 Repository",
                        "The Rawr4 parsing page was not able to load the character correctly",
                        "").Show();
                } else {
                    new Base.ErrorBox() {
                        Title = "Problem Getting Character from the Rawr4 Repository",
                        TheException = ex,
                    }.Show();
                }
            }
        }
        private void _webClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Cancelled) { return; }
            if (e.Error != null)
            {
                if (e.Error.Message.Contains("Not Found"))
                {
                    new Base.ErrorBox("Problem Saving Character to Rawr4 Repository",
                        "The name of the save appears to be causing a conflict.",
                        "Try replacing any periods with spaces or +").Show();
                }
                else
                {
                    new Base.ErrorBox()
                    {
                        Title = "Problem Saving Character to Rawr4 Repository",
                        Function = "_webClient_UploadStringCompleted(string input)",
                        TheException = e.Error,
                    }.Show();
                }
                return;
            }
            Progress = "Complete!";
            if (this.SetCharacterCompleted != null)
                this.SetCharacterCompleted(this, new EventArgs<string>(e.Result));
        }

        private void bwParse_ProgressChanged(object sender, ProgressChangedEventArgs e) { Progress = e.UserState.ToString(); }

        public event EventHandler<EventArgs<Character>> GetCharacterCompleted;
        public event EventHandler<EventArgs<string>> SetCharacterCompleted;
        public void GetCharacterAsync(string identifier)
        {
            _lastIdentifier = identifier;
            string url = string.Format(URL_CHAR_REQ, identifier.Replace("+", "%20").Replace(" ", "%20").Replace(".", "%20"), "");
            _webClient.DownloadStringAsync(new Uri(url));
            this.Progress = "Downloading Character Data...";
        }
        public void SetCharacterAsync(string identifier, string pw, string charXmlData)
        {
            _lastIdentifier = identifier;
            string url = string.Format(URL_CHAR_REQ, identifier.Replace("+", "%20").Replace(" ", "%20").Replace(".", "%20"), pw != "" ? "~" + pw : "~");
            _webClient.Encoding = Encoding.UTF8;
            _webClient.UploadStringAsync(new Uri(url), "POST", charXmlData);
            this.Progress = "Uploading Character Data...";
        }
    }
}
