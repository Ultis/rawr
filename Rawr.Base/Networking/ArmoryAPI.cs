using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
#if SILVERLIGHT
using System.Windows.Browser;
#else
using System.Web;
#endif
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Rawr
{
	public class ArmoryAPI
	{
        private const string URL_CHAR_REQ = "http://{0}.battle.net/api/wow/character/{1}/{2}?fields=items,talents,pets,professions{3}";
        private WebClient _webClient;
        
        public ArmoryAPI()
        {
            _webClient = new WebClient();
            _webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_webClient_DownloadStringCompleted);
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
        }

        private void _webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) { return; }
                #region Handle Errors
                if (e.Error != null)
                {
                    if (e.Error.Message.Contains("NotFound"))
                    {
                        new Base.ErrorBox("Problem Getting Character from Battle.Net Armory",
                            "Your character was not found on the server.",
                            "This could be due to a change on Battle.Net as these are happening often right now and can easily break the parsing."
                            + " You do not need to create a new Issue for this as we have a monitoring system in place which alerts us to Armories that don't parse.");
                    }
                    else
                    {
                        new Base.ErrorBox()
                        {
                            Title = "Problem Getting Character from Battle.Net Armory",
                            Function = "_webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)",
                            TheException = e.Error,
                        }.Show();
                    }
                    return;
                }
                /*
                if (e.Result != null && e.Result.ToLower().Contains("server is overloaded"))
                {
                    new Base.ErrorBox("Problem Getting Character from Battle.Net Armory",
                        "The server is down for High Volume Issues",
                        "Due to high volume of traffic, Blizzard has taken their World of Warcraft and Starcraft II Community sites offline to"
                        + " allow as many users as possible to claim keys, upgrade accounts and manage subscriptions."
                        + "\r\nAs soon as website load is back to normal they will open these sites again."
                        + "\r\nThank you for your patience!"
                        + "\r\nThe Rawr team suggests using the Rawr Addon instead to load your character for now.").Show();
                    Progress = "Error!";
                    if (this.GetCharacterErrored != null)
                        this.GetCharacterErrored(this, new EventArgs<String>(e.Result.Replace("<Error>", "").Replace("</Error>", "")));
                    return;
                }
                else if (e.Result != null && e.Result.ToLower().Contains("server is down for maintenance"))
                {
                    new Base.ErrorBox("Problem Getting Character from Battle.Net Armory",
                        "The server is down for Maintenance",
                        "The Rawr team suggests using the Rawr Addon instead to load your character for now.").Show();
                    Progress = "Error!";
                    if (this.GetCharacterErrored != null)
                        this.GetCharacterErrored(this, new EventArgs<String>(e.Result.Replace("<Error>", "").Replace("</Error>", "")));
                    return;
                }
                else if (e.Result != null && e.Result.ToLower().Contains("<error>"))
                {
                    new Base.ErrorBox("Problem Getting Character from Battle.Net Armory",
                        e.Result.Replace("<Error>", "").Replace("</Error>", ""),
                        "The Rawr team suggests using the Rawr Addon instead to load your character for now.").Show();
                    Progress = "Error!";
                    if (this.GetCharacterErrored != null)
                        this.GetCharacterErrored(this, new EventArgs<String>(e.Result.Replace("<Error>", "").Replace("</Error>", "")));
                    return;
                }*/
                #endregion

                XDocument xdoc;
                //using (StringReader sr = new StringReader(e.Result))
                {
                   // xdoc = XDocument.Load(sr);
                }
                /*
                if (xdoc.Root.Name == "Error")
                {
                    new Base.ErrorBox("Problem Getting Character from Battle.Net Armory",
                        xdoc.Root.Value,
                        "The Rawr team suggests using the Rawr Addon instead to load your character for now.").Show();
                    Progress = "Error!";
                    if (this.GetCharacterErrored != null)
                        this.GetCharacterErrored(this, new EventArgs<String>(e.Result.Replace("<Error>", "").Replace("</Error>", "")));
                    return;
                }
                else if (xdoc.Root.Name == "Character")
                {
                    Progress = "Parsing Character Data...";
                    Character character = Character.LoadFromXml(xdoc.Document.ToString());
                    character.Realm = character.Realm.Replace("-", " ");
                    Calculations.GetModel(character.CurrentModel).SetDefaults(character);
                    Progress = "Complete!";
                    if (this.GetCharacterCompleted != null)
                        this.GetCharacterCompleted(this, new EventArgs<Character>(character));
                }*/
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("NotFound"))
                {
                    new Base.ErrorBox("Error Getting Character from Battle.Net Armory",
                        "The Rawr4 parsing page was not able to load the character correctly",
                        "This could be due to a change on Battle.Net as these are happening often right now and can easily break the parsing."
                        + " You do not need to create a new Issue for this as we have a monitoring system in place which alerts us to Armories that don't parse.").Show();
                }
                else
                {
                    new Base.ErrorBox()
                    {
                        Title = "Problem Getting Character from Battle.Net Armory",
                        TheException = ex,
                    }.Show();
                }
            }
        }

        #region Characters
        public void GetCharacterAsync(CharacterRegion region, string realm, string name, bool forceRefresh)
        {
            string url = string.Format(URL_CHAR_REQ, region.ToString().ToLower(), realm, name, forceRefresh ? "!" : "");
            _webClient.DownloadStringAsync(new Uri(url));
            this.Progress = "Downloading Character Data...";
        }
        #endregion
    }
}
