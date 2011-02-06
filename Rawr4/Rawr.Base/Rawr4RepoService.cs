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

        public Rawr4RepoService()
        {
            _webClient = new WebClient();
            _webClient.Encoding = Encoding.UTF8; // rawr4 uses UTF8 encoding
            _webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_webClient_DownloadStringCompleted);
            _webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(_webClient_UploadStringCompleted);
            _queueTimer.Tick += new EventHandler(CheckQueueAsync);
        }

        public event EventHandler<EventArgs<string>> ProgressChanged;
        private string _progress = "Requesting Character...";
        public string Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                if (ProgressChanged != null)
                {
                    ProgressChanged(this, new EventArgs<string>(value));
                }
            }
        }

        //private bool _canceled = false;
        public void CancelAsync()
        {
            _webClient.CancelAsync();
            //_canceled = true;
        }

        private void _webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) { return; }
                if (e.Error != null)
                {
                    if (e.Error.Message.Contains("NotFound"))
                    {
                        new Base.ErrorBox("Problem Getting Character from Rawr4 Repository",
                            "Your character file was not found on the server.",
                            "");
                    }
                    else
                    {
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

                /*if (xdoc.Root.Name == "queue")
                {
                    Progress = "Queued (Position: " + xdoc.Root.Attribute("position").Value + ")";
                    _queueTimer.Start();
                }
                else*/
                if (xdoc.Root.Name == "Character")
                {
                    Progress = "Parsing Character Data...";
                    Character character = Character.LoadFromXml(xdoc.Document.ToString());
                    Progress = "Complete!";
                    if (this.GetCharacterCompleted != null)
                        this.GetCharacterCompleted(this, new EventArgs<Character>(character));
                    //BackgroundWorker bwParseCharacter = new BackgroundWorker();
                    //bwParseCharacter.WorkerReportsProgress = true;
                    //bwParseCharacter.DoWork += new DoWorkEventHandler(bwParseCharacter_DoWork);
                    //bwParseCharacter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwParseCharacter_RunWorkerCompleted);
                    //bwParseCharacter.ProgressChanged += new ProgressChangedEventHandler(bwParse_ProgressChanged);
                    //bwParseCharacter.RunWorkerAsync(xdoc);
                }
                /*else if (xdoc.Root.Name == "itemData")
                {
                    Progress = "Parsing Item Data...";
                    BackgroundWorker bwParseItem = new BackgroundWorker();
                    bwParseItem.WorkerReportsProgress = true;
                    bwParseItem.DoWork += new DoWorkEventHandler(bwParseItem_DoWork);
                    bwParseItem.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwParseItem_RunWorkerCompleted);
                    bwParseItem.ProgressChanged += new ProgressChangedEventHandler(bwParse_ProgressChanged);
                    bwParseItem.RunWorkerAsync(xdoc);
                }*/
            } catch (Exception ex) {
                if (ex.Message.Contains("NotFound"))
                {
                    new Base.ErrorBox("Error Getting Character from the Rawr4 Repository",
                        "The Rawr4 parsing page was not able to load the character correctly",
                        "").Show();
                }
                else
                {
                    new Base.ErrorBox()
                    {
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
                new Base.ErrorBox()
                {
                    Title = "Problem Saving Character to Rawr4 Repository",
                    Function = "_webClient_UploadStringCompleted(string input)",
                    TheException = e.Error,
                }.Show();
                return;
            }
            Progress = "Complete!";
            if (this.SetCharacterCompleted != null)
                this.SetCharacterCompleted(this, new EventArgs<string>(e.Result));
        }


        private void bwParse_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.UserState.ToString();
        }

        private DispatcherTimer _queueTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
        private string _lastIdentifier;
        private void CheckQueueAsync(object sender, EventArgs e)
        {
            /*_queueTimer.Stop();
            if (!_canceled)
            {
                /*if (_lastRequestWasItem)
                {
                    _webClient.DownloadStringAsync(new Uri(string.Format(URL_ITEM, _lastItemId)));
                    this.Progress = "Downloading Item Data...";
                }
                else*//*
                {
                    _webClient.DownloadStringAsync(new Uri(string.Format(URL_CHAR_QUEUE,
                        _lastName.ToLower(), _lastRegion.ToString().ToLower(), _lastRealm.ToLower())));
                    this.Progress = "Downloading Character Data...";
                }
            }*/
        }

        #region Characters
        public event EventHandler<EventArgs<Character>> GetCharacterCompleted;
        public event EventHandler<EventArgs<string>> SetCharacterCompleted;
        public void GetCharacterAsync(string identifier)
        {
            _lastIdentifier = identifier;
            //_canceled = false;
            //_lastRequestWasItem = false;
            string url = string.Format(URL_CHAR_REQ, identifier, "");
            _webClient.DownloadStringAsync(new Uri(url));
            this.Progress = "Downloading Character Data...";
        }
        public void SetCharacterAsync(string identifier, string pw, string charXmlData)
        {
            _lastIdentifier = identifier;
            //_canceled = false;
            //_lastRequestWasItem = false;
            string url = string.Format(URL_CHAR_REQ, identifier, pw != "" ? "~" + pw : "~");
            _webClient.Encoding = Encoding.UTF8;
            _webClient.UploadStringAsync(new Uri(url), charXmlData);
            this.Progress = "Uploading Character Data...";
        }

        private string UrlEncode(string text)
        {
            // Rawr4.com expects space to be encoded as %20
            #if SILVERLIGHT
            return HttpUtility.UrlEncode(text).Replace("+", "%20");
            #else
            return Utilities.UrlEncode(text).Replace("+", "%20");
            #endif
        }
        #endregion
    }
}
