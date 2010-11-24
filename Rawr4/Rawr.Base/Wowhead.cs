using System;
using System.Net;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Browser;
using System.ComponentModel;
using System.Windows.Threading;
using System.IO;

namespace Rawr
{
    public class WowheadService
    {
        // ==============================================
        //
        // COPIED FROM Armory.cs (should probably be removed)
        //
        // ==============================================

        public static void GetItem(int id, Action<Item> callback)
        {
            new ItemRequest(id, callback);
        }

        /* This is commented out until request by name can be set up
        public static void GetItemIdByName(string itemName, Action<int> callback)
        {
            new ItemIdRequest(itemName, callback);
        }*/

        // ==============================================
        //
        // COPIED FROM ElitistArmoryService.cs (most should probably be removed)
        //
        // ==============================================

        private const string URL_ITEM = "http://{0}.wowhead.com/item={1}&xml";
        private WebClient _webClient;

        public WowheadService()
        {
            _webClient = new WebClient();
            _webClient.Encoding = Encoding.UTF8; // wowhead xml pages use UTF8 encoding
            _webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_webClient_DownloadStringCompleted);
            _queueTimer.Tick += new EventHandler(CheckQueueAsync);
        }

        public event EventHandler<EventArgs<string>> ProgressChanged;
        private string _progress = "Requesting Item...";
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

        private bool _canceled = false;
        public void CancelAsync()
        {
            _webClient.CancelAsync();
            _canceled = true;
        }

        private void _webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                XDocument xdoc;
                try
                {
                    using (StringReader sr = new StringReader(e.Result))
                    {
                        xdoc = XDocument.Load(sr);
                    }
                }
                catch (TargetInvocationException ex) {
                    Progress = "Did not download file correctly";
                    return;
                }

                if (xdoc.Root.FirstAttribute.Name == "error")
                {
                    Progress = xdoc.Root.FirstAttribute.Value;
                    CancelAsync();
                }
                else if (xdoc.Root.FirstAttribute.Name == "item")
                {
                    Progress = "Parsing Item Data...";
                    BackgroundWorker bwParseItem = new BackgroundWorker();
                    bwParseItem.WorkerReportsProgress = true;
                    bwParseItem.DoWork += new DoWorkEventHandler(bwParseItem_DoWork);
                    bwParseItem.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwParseItem_RunWorkerCompleted);
                    bwParseItem.ProgressChanged += new ProgressChangedEventHandler(bwParse_ProgressChanged);
                    bwParseItem.RunWorkerAsync(xdoc);
                }
            }
        }

        private void bwParse_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.UserState.ToString();
        }

        private DispatcherTimer _queueTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
        private int _lastItemId;
        private void CheckQueueAsync(object sender, EventArgs e)
        {
            _queueTimer.Stop();
            if (!_canceled)
            {
                _webClient.DownloadStringAsync(new Uri(string.Format(URL_ITEM, UsePTR ? "cata" : "www", _lastItemId)));
                this.Progress = "Downloading Item Data...";
            }
        }

        private string UrlEncode(string text)
        {
            // elitistarmory expect space to be encoded as %20
            return HttpUtility.UrlEncode(text).Replace("+", "%20");
        }

        bool UsePTR = false;

        #region Items
        public event EventHandler<EventArgs<Item>> GetItemCompleted;
        public void GetItemAsync(int itemId, bool usePTR)
        {
            _lastItemId = itemId;
            UsePTR = usePTR;
            _webClient.DownloadStringAsync(new Uri(string.Format(URL_ITEM, UsePTR ? "cata" : "www", itemId)));
            this.Progress = "Downloading Item Data...";
        }

        void bwParseItem_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Progress = "Complete!";
                if (this.GetItemCompleted != null)
                    this.GetItemCompleted(this, new EventArgs<Item>(e.Result as Item));
            }
        }

        private void bwParseItem_DoWork(object sender, DoWorkEventArgs e)
        {
            XDocument xdoc = e.Argument as XDocument;
            int id = 0;
            try
            {
                #region Wowhead Parsing
                if (xdoc == null || xdoc.Root.Value.Contains("Item not found!")) { e.Result = null; return; }
                // the id from above can now be a name as well as the item number, so we regrab it from the data wowhead returned
                foreach (XElement node in xdoc.SelectNodes("wowhead/item")) { id = int.Parse(node.Attribute("id").Value); }
                Item item = new Item() { Id = id, Stats = new Stats() };
                string htmlTooltip = string.Empty;
                string json1s = string.Empty;
                string json2s = string.Empty;
                string repSource = string.Empty;
                string repLevel = string.Empty;

                #region Set Initial Data (Name, Quality, Unique, etc) and record the Tooltip, json & jsonequip sections
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/name")) { item.Name = node.Value; }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/quality")) { item.Quality = (ItemQuality)int.Parse(node.Attribute("id").Value); }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/icon")) { item.IconPath = node.Value; }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/htmlTooltip")) { htmlTooltip = node.Value; }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/json")) { json1s = node.Value; }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/jsonEquip")) { json2s = node.Value; }
                if (htmlTooltip.Contains("Unique")) item.Unique = true;
                #endregion

                // On Load items from Wowhead Filter, we don't want any
                // items that aren't at least Epic quality
                //if (filter && (int)item.Quality < 2) { return null; }

                #region Item Binding
                // Bind status check
                if (htmlTooltip.Contains("Binds when picked up"))       item.Bind = BindsOn.BoP;
                else if (htmlTooltip.Contains("Binds when equipped"))   item.Bind = BindsOn.BoE;
                else if (htmlTooltip.Contains("Binds to account"))      item.Bind = BindsOn.BoA;
                else if (htmlTooltip.Contains("Binds when used"))       item.Bind = BindsOn.BoU;
                #endregion

                Dictionary<string, object> json;

                try {
                    json = JsonParser.Merge(JsonParser.Parse(json1s), JsonParser.Parse(json2s));
                } catch {
                    e.Result = null;
                    return;
                }

                #region Process json & jsonequip
                object tmp;
                // Pull Faction Info
                //,reqfaction:1073,reqrep:6
                if (json.TryGetValue("reqfaction", out tmp))
                {
                    repSource = tmp.ToString();
                }
                if (json.TryGetValue("reqrep", out tmp))
                {
                    repLevel = tmp.ToString();
                }
                if (json.TryGetValue("displayid", out tmp)) //A 3d display ID# for each icon
                {
                    item.DisplayId = (int)tmp;
                }
                if (json.TryGetValue("slotbak", out tmp)) //A couple slots actually have two possible slots... ie vests and robes both fit in chest. slotbak distinguishes vests from robes. We don't care for Rawr, so ignored.
                {
                    item.DisplaySlot = (int)tmp; // it is also used for the 3d display slot id
                }
                if (json.TryGetValue("level", out tmp)) //Rawr now handles item levels
                {
                    item.ItemLevel = (int)tmp;
                }
                if (json.TryGetValue("slot", out tmp))
                {
                    int slot = (int)tmp;
                    if (slot != 0)
                    {
                        item.Slot = GetItemSlot(slot);
                    }
                }
                if (json.TryGetValue("classs", out tmp))
                {
                    string c = tmp.ToString();
                    if (json.TryGetValue("subclass", out tmp))
                    {
                        c = c + "." + tmp.ToString();
                    }
                    if (c.StartsWith("1.") || c.StartsWith("12."))
                    {
                    }
                    else if (c.StartsWith("3."))
                    {
                        item.Type = ItemType.None;
                        switch (c)
                        {
                            case "3.0": item.Slot = ItemSlot.Red; break;
                            case "3.1": item.Slot = ItemSlot.Blue; break;
                            case "3.2": item.Slot = ItemSlot.Yellow; break;
                            case "3.3": item.Slot = ItemSlot.Purple; break;
                            case "3.4": item.Slot = ItemSlot.Green; break;
                            case "3.5": item.Slot = ItemSlot.Orange; break;
                            case "3.6": item.Slot = ItemSlot.Meta; break;
                            case "3.8": item.Slot = ItemSlot.Prismatic; break;
                        }
                    }
                    else
                    {
                        item.Type = GetItemType(c);
                    }
                }
                if (json.TryGetValue("speed", out tmp))
                {
                    item.Speed = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("dmgmin1", out tmp))
                {
                    item.MinDamage = (int)Math.Floor(Convert.ToSingle(tmp));
                }
                if (json.TryGetValue("dmgmax1", out tmp))
                {
                    item.MaxDamage = (int)Math.Ceiling(Convert.ToSingle(tmp));
                }
                if (json.TryGetValue("dmgtype1", out tmp))
                {
                    item.DamageType = (ItemDamageType)(int)tmp;
                }
                if (json.TryGetValue("armor", out tmp))
                {
                    item.Stats.Armor = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("armorbonus", out tmp))
                {
                    item.Stats.Armor -= Convert.ToSingle(tmp);
                    item.Stats.BonusArmor = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("healthrgn", out tmp))
                {
                    item.Stats.Hp5 += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("manargn", out tmp))
                {
                    item.Stats.Mp5 += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("health", out tmp))
                {
                    item.Stats.Health += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("agi", out tmp))
                {
                    item.Stats.Agility += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("int", out tmp))
                {
                    item.Stats.Intellect += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("spi", out tmp))
                {
                    item.Stats.Spirit += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("sta", out tmp))
                {
                    item.Stats.Stamina += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("str", out tmp))
                {
                    item.Stats.Strength += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("hastertng", out tmp) || json.TryGetValue("mlehastertng", out tmp) || json.TryGetValue("rgdhastertng", out tmp) || json.TryGetValue("splhastertng", out tmp))
                {
                    item.Stats.HasteRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("mastrtng", out tmp))
                {
                    item.Stats.MasteryRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("splpwr", out tmp) || json.TryGetValue("splheal", out tmp) || json.TryGetValue("spldmg", out tmp))
                {
                    item.Stats.SpellPower += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("critstrkrtng", out tmp) || json.TryGetValue("mlecritstrkrtng", out tmp) || json.TryGetValue("rgdcritstrkrtng", out tmp) || json.TryGetValue("splcritstrkrtng", out tmp))
                {
                    item.Stats.CritRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("firres", out tmp))
                {
                    item.Stats.FireResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("natres", out tmp))
                {
                    item.Stats.NatureResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("frores", out tmp))
                {
                    item.Stats.FrostResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("shares", out tmp))
                {
                    item.Stats.ShadowResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("arcres", out tmp))
                {
                    item.Stats.ArcaneResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("hitrtng", out tmp) || json.TryGetValue("mlehitrtng", out tmp) || json.TryGetValue("rgdhitrtng", out tmp) || json.TryGetValue("splhitrtng", out tmp))
                {
                    item.Stats.HitRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("atkpwr", out tmp) || json.TryGetValue("mleatkpwr", out tmp))
                {
                    item.Stats.AttackPower += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("rgdatkpwr", out tmp))
                {
                    if (item.Stats.AttackPower != Convert.ToSingle(tmp))
                    {
                        item.Stats.RangedAttackPower = Convert.ToSingle(tmp);
                    }
                }
                if (json.TryGetValue("block", out tmp))
                {
                    item.Stats.BlockValue += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("exprtng", out tmp))
                {
                    item.Stats.ExpertiseRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("defrtng", out tmp))
                {
                    item.Stats.DefenseRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("dodgertng", out tmp))
                {
                    item.Stats.DodgeRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("blockrtng", out tmp))
                {
                    item.Stats.BlockRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("socket1", out tmp))
                {
                    item.SocketColor1 = GetSocketType(tmp.ToString());
                }
                if (json.TryGetValue("socket2", out tmp))
                {
                    item.SocketColor2 = GetSocketType(tmp.ToString());
                }
                if (json.TryGetValue("socket3", out tmp))
                {
                    item.SocketColor3 = GetSocketType(tmp.ToString());
                }
                if (json.TryGetValue("socketbonus", out tmp))
                {
                    item.SocketBonus = GetSocketBonus(tmp.ToString());
                }
                if (json.TryGetValue("parryrtng", out tmp))
                {
                    item.Stats.ParryRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("classes", out tmp))
                {
                    List<string> requiredClasses = new List<string>();
                    int classbitfield = (int)tmp;
                    if ((classbitfield & 1) > 0)
                        requiredClasses.Add("Warrior");
                    if ((classbitfield & 2) > 0)
                        requiredClasses.Add("Paladin");
                    if ((classbitfield & 4) > 0)
                        requiredClasses.Add("Hunter");
                    if ((classbitfield & 8) > 0)
                        requiredClasses.Add("Rogue");
                    if ((classbitfield & 16) > 0)
                        requiredClasses.Add("Priest");
                    if ((classbitfield & 32) > 0)
                        requiredClasses.Add("Death Knight");
                    if ((classbitfield & 64) > 0)
                        requiredClasses.Add("Shaman");
                    if ((classbitfield & 128) > 0)
                        requiredClasses.Add("Mage");
                    if ((classbitfield & 256) > 0)
                        requiredClasses.Add("Warlock");
                    //if ((classbitfield & 512) > 0) ; // Only seems to occur in PvP gear, along with another huge value
                    //    requiredClasses.Add("");
                    if ((classbitfield & 1024) > 0)
                        requiredClasses.Add("Druid");
                    item.RequiredClasses = string.Join("|", requiredClasses.ToArray());
                }
                if (json.TryGetValue("resirtng", out tmp))
                {
                    item.Stats.Resilience += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("armorpen", out tmp) || json.TryGetValue("armorpenrtng", out tmp))
                {
                    item.Stats.ArmorPenetrationRating = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("splpen", out tmp))
                {
                    item.Stats.SpellPenetration = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("mana", out tmp))
                {
                    item.Stats.Mana = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("dmg", out tmp))
                {
                    item.Stats.WeaponDamage = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("frospldmg", out tmp))
                {
                    item.Stats.SpellFrostDamageRating = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("shaspldmg", out tmp))
                {
                    item.Stats.SpellShadowDamageRating = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("firspldmg", out tmp))
                {
                    item.Stats.SpellFireDamageRating = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("arcspldmg", out tmp))
                {
                    item.Stats.SpellArcaneDamageRating = Convert.ToSingle(tmp);
                }
                #endregion

                // We don't need to process any more data if it's not a slottable item (eg not Gear/Gem)
                if (item.Slot == ItemSlot.None) { e.Result = null; return; }

                #region Item Source
                if (json.TryGetValue("source", out tmp))
                {
                    object[] sourceArr = (object[])tmp;
                    object[] sourcemoreArr = null;
                    if (json.TryGetValue("sourcemore", out tmp))
                    {
                        sourcemoreArr = (object[])tmp;
                    }

                    #region We have Source Data

                    // most mobs that have a vendor bought alternative will give more information through the vendor than the mob
                    // this is specially case for vault of archavon drops
                    int source = (int)sourceArr[0];
                    Dictionary<string, object> sourcemore = null;
                    if (sourcemoreArr != null && sourcemoreArr.Length > 0)
                    {
                        sourcemore = (Dictionary<string, object>)sourcemoreArr[0];
                    }

                    int vendorIndex = Array.IndexOf(sourceArr, 5);
                    if (vendorIndex >= 0)
                    {
                        source = (int)sourceArr[vendorIndex];
                        if (sourcemoreArr != null && sourcemoreArr.Length > vendorIndex)
                        {
                            sourcemore = (Dictionary<string, object>)sourcemoreArr[vendorIndex];
                        }
                    }

                    string n = string.Empty;
                    if (sourcemore != null && sourcemore.TryGetValue("n", out tmp))
                    {
                        n = tmp.ToString();
                    }

                    string itemId = item.Id.ToString();
                    if (source == 2 && sourcemore == null)
                    {
                        #region Didn't work
                        /*// We don't have SourceMore data so let's see if the web page has what we need
                        // If not, call it a World Drop
                        string name = null;
                        string boss = null;
                        string area = null;
                        bool heroic = false;
                        bool container = false;
                        try {
                            //if (!_UnkDropMap.ContainsKey(itemId)) {
                                XmlDocument docToken = wrw.DownloadItemHtmlWowhead(itemId);

                                string tokenJson = docToken.SelectSingleNode("wowhead/item/json").InnerText;

                                string tokenSource = string.Empty;
                                if (tokenJson.Contains("source:[")) {
                                    tokenSource = tokenJson.Substring(tokenJson.IndexOf("source:[") + "source:[".Length);
                                    tokenSource = tokenSource.Substring(0, tokenSource.IndexOf("]"));
                                }

                                string tokenSourcemore = string.Empty;
                                if (tokenJson.Contains("sourcemore:[{")) {
                                    tokenSourcemore = tokenJson.Substring(tokenJson.IndexOf("sourcemore:[{") + "sourcemore:[{".Length);
                                    tokenSourcemore = tokenSourcemore.Substring(0, tokenSourcemore.IndexOf("}]"));
                                }

                                if (!string.IsNullOrEmpty(tokenSource) && !string.IsNullOrEmpty(tokenSourcemore)) {
                                    string[] tokenSourceKeys = tokenSource.Split(',');
                                    string[] tokenSourcemoreKeys = tokenSourcemore.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

                                    // for tokens we prefer loot info, we don't care if it can be bought with badges
                                    tokenSource = tokenSourceKeys[0];
                                    tokenSourcemore = tokenSourcemoreKeys[0];

                                    int dropIndex = Array.IndexOf(tokenSourceKeys, "2");
                                    if (dropIndex >= 0) {
                                        tokenSource = tokenSourceKeys[dropIndex];
                                        tokenSourcemore = tokenSourcemoreKeys[dropIndex];
                                    }

                                    if (tokenSource == "2") {
                                        foreach (string kv in tokenSourcemore.Split(',')) {
                                            if (!string.IsNullOrEmpty(kv)) {
                                                string[] keyvalsplit = kv.Split(':');
                                                string key = keyvalsplit[0];
                                                string val = keyvalsplit[1];
                                                switch (key) {
                                                    case "t":
                                                        container = val == "2" || val == "3";
                                                        break;
                                                    case "n":       // NPC 'Name'
                                                        boss = val.Replace("\\'", "'").Trim('\'');
                                                        break;
                                                    case "z":       // Zone
                                                        area = GetZoneName(val);
                                                        break;
                                                    case "dd":      // Dungeon Difficulty (1 = Normal, 2 = Heroic)
                                                        heroic = val == "2";
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (boss == null) { boss = "Unknown Boss (Wowhead lacks data)"; }
                                _UnkDropMap[itemId] = new UnkDropInfo() { Boss = boss, Area = area, Heroic = heroic, Name = itemId, Container = container };
                                if (area != null) {
                                    #region This is a Dropped Item, so assign it to where it drops from
                                    if (container) {
                                        ItemLocation locInfo = new ContainerItem()
                                        {
                                            Area = area,
                                            Container = boss,
                                            Heroic = heroic
                                        };
                                        LocationFactory.Add(item.Id.ToString(), locInfo);
                                    } else {
                                        ItemLocation locInfo = new StaticDrop()
                                        {
                                            Area = area,
                                            Boss = boss,
                                            Heroic = heroic
                                        };
                                        LocationFactory.Add(item.Id.ToString(), locInfo);
                                    }
                                    #endregion
                                } else {
                                    // World Drop dat crap, until we can find a better solution
                                    WorldDrop locInfo = new WorldDrop();
                                    LocationFactory.Add(item.Id.ToString(), locInfo);
                                }
                            /*} else {
                                UnkDropInfo info = _UnkDropMap[itemId];
                                boss = info.Boss;
                                area = info.Area;
                                heroic = info.Heroic;
                                name = info.Name;
                                container = info.Container;
                                if (area != null) {
                                    #region This is a Dropped Item, so assign it to where it drops from
                                    if (container) {
                                        ItemLocation locInfo = new ContainerItem()
                                        {
                                            Area = area,
                                            Container = boss,
                                            Heroic = heroic
                                        };
                                        LocationFactory.Add(item.Id.ToString(), locInfo);
                                    } else {
                                        ItemLocation locInfo = new StaticDrop()
                                        {
                                            Area = area,
                                            Boss = boss,
                                            Heroic = heroic
                                        };
                                        LocationFactory.Add(item.Id.ToString(), locInfo);
                                    }
                                    #endregion
                                } else {
                                    // World Drop dat crap, until we can find a better solution
                                    WorldDrop locInfo = new WorldDrop();
                                    LocationFactory.Add(item.Id.ToString(), locInfo);
                                }
                            }*/
                        #endregion
                        /*} catch (Exception ex) {
                            Rawr.Base.ErrorBox eb = new Rawr.Base.ErrorBox("Error getting drop data from wowhead",
                                ex.Message, "GetItem(...)", "", ex.StackTrace);*/
                        // World Drop dat crap, until we can find a better solution
                        WorldDrop locInfo = new WorldDrop();
                        LocationFactory.Add(item.Id.ToString(), locInfo);
                        /*}*/
                    }
                    else if (source == 5)
                    {
                        // if we only have vendor information then we will want to download
                        // the normal html page and scrape the currency information
                        // and in case it is a token, link it to the boss/zone where the token drops
                        #region Vendor Related
                        string[] tokenIds = { null, null };
                        int[] tokenCounts = { 0, 0 };
                        string[] tokenNames = { null, null };
                        int cost = 0;
                        #region Try to get the Token Names and individual costs
                        try
                        {
                            /*XDocument rawHtmlDoc = wrw.DownloadItemHtmlWowhead(query);
                            if (rawHtmlDoc != null)
                            {
                                int startpos = rawHtmlDoc.InnerXml.IndexOf("new Listview({template: 'npc', id: 'sold-by'");
                                if (startpos > 1)
                                {
                                    int endpos = rawHtmlDoc.InnerXml.IndexOf(";", startpos);
                                    string text = rawHtmlDoc.InnerXml.Substring(startpos, endpos - startpos);
                                    // we are looking for something like cost:[0,0,0,[[40633,1]]]

                                    int costIndex = text.IndexOf("cost:[");
                                    if (costIndex >= 0)
                                    {
                                        string costtext = text.Substring(costIndex + 6);
                                        // get the monetary cost
                                        cost = int.Parse(costtext.Substring(0, costtext.IndexOfAny(new char[] { ',', ']' }, 0)));
                                        // get the token and count out
                                        int tokenIndex = costtext.IndexOf("[[", 0);
                                        if (tokenIndex >= 0)
                                        {
                                            int tokenEnd = costtext.IndexOf("]]", tokenIndex);
                                            string tokentext = costtext.Substring(tokenIndex + 1, tokenEnd - tokenIndex);
                                            if (tokentext.Contains("],["))
                                            {
                                                string[] tokens = tokentext.Split(',');
                                                tokenIds[0] = tokens[0].Trim('[');
                                                tokenCounts[0] = int.Parse(tokens[1].Trim(']'));

                                                tokenIds[1] = tokens[2].Trim('[');
                                                tokenCounts[1] = int.Parse(tokens[3].Trim(']'));
                                                /*string[] token1 = tokentext.Substring(1, 8).Split(',');
                                                string[] token2 = tokentext.Substring(12, 8).Split(',');
                                                tokenIds[0] = token1[0]; tokenCounts[0] = int.Parse(token1[1].Trim('[').Trim(']'));
                                                tokenIds[1] = token2[0]; tokenCounts[1] = int.Parse(token2[1].Trim('[').Trim(']'));*/
                                            /*}
                                            else
                                            {
                                                string[] token = costtext.Substring(tokenIndex + 2, tokenEnd - tokenIndex - 2).Split(',');
                                                tokenIds[0] = token[0];
                                                tokenCounts[0] = int.Parse(token[1]);
                                            }
                                        }
                                    }
                                }
                            }*/
                        }
                        catch { }
                        #endregion
                        VendorItem vendorItem = new VendorItem()
                        {
                            Cost = cost,
                        };
                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 1 && tokenIds[i] == null) { continue; } // break out if we're one 2 and there's only 1 or for some reason there's 0
                            if (tokenIds[i] != null && _pvpTokenMap.TryGetValue(tokenIds[i], out tokenNames[i]))
                            {
                                #region It's a PvP Token: Mark of Honor/Venture Coin
                                ItemLocation locInfo = new PvpItem()
                                {
                                    TokenCount = tokenCounts[i],
                                    TokenType = tokenNames[i]
                                };
                                LocationFactory.Add(item.Id.ToString(), locInfo);
                                vendorItem = null;
                                break;
                                #endregion
                            }
                            else if (tokenIds[i] != null && _vendorTokenMap.TryGetValue(tokenIds[i], out tokenNames[i]))
                            {
                                #region It's a PvE Token that we've seen before and it's from a Vendor
                                vendorItem.TokenMap[tokenNames[i]] = tokenCounts[i];
                                #endregion
                            }
                            else if (tokenIds[i] != null)
                            {
                                #region It's a PvE Token that is not from a Vendor
                                // ok now let's see what info we can get about this token
                                string boss = null;
                                string area = null;
                                bool heroic = false;
                                bool container = false;
                                if (!_tokenDropMap.ContainsKey(tokenIds[i]))
                                {
                                    #region We *really* haven't seen this before so we need to pull the data
                                    /*XDocument docToken = wrw.DownloadItemWowhead(site, tokenIds[i]);
                                    if (docToken != null)
                                    {
                                        tokenNames[i] = docToken.SelectSingleNode("wowhead/item/name").Value;

                                        // we don't want token => boss propagation anymore, otherwise you get weird stuff like 277 gloves dropping from Toravon
                                        /*string tokenJson = docToken.SelectSingleNode("wowhead/item/json").InnerText;

                                        string tokenSource = string.Empty;
                                        if (tokenJson.Contains("\"source\":["))
                                        {
                                            tokenSource = tokenJson.Substring(tokenJson.IndexOf("\"source\":[") + "\"source\":[".Length);
                                            tokenSource = tokenSource.Substring(0, tokenSource.IndexOf("]"));
                                        }

                                        string tokenSourcemore = string.Empty;
                                        if (tokenJson.Contains("\"sourcemore\":[{"))
                                        {
                                            tokenSourcemore = tokenJson.Substring(tokenJson.IndexOf("\"sourcemore\":[{") + "\"sourcemore\":[{".Length);
                                            tokenSourcemore = tokenSourcemore.Substring(0, tokenSourcemore.IndexOf("}]"));
                                        }

                                        if (!string.IsNullOrEmpty(tokenSource) && !string.IsNullOrEmpty(tokenSourcemore))
                                        {
                                            string[] tokenSourceKeys = tokenSource.Split(',');
                                            string[] tokenSourcemoreKeys = tokenSourcemore.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

                                            // for tokens we prefer loot info, we don't care if it can be bought with badges
                                            tokenSource = tokenSourceKeys[0];
                                            tokenSourcemore = tokenSourcemoreKeys[0];

                                            int dropIndex = Array.IndexOf(tokenSourceKeys, "2");
                                            if (dropIndex >= 0)
                                            {
                                                tokenSource = tokenSourceKeys[dropIndex];
                                                tokenSourcemore = tokenSourcemoreKeys[dropIndex];
                                            }

                                            if (tokenSource == "2")
                                            {
                                                foreach (string kv in tokenSourcemore.Split(','))
                                                {
                                                    if (!string.IsNullOrEmpty(kv))
                                                    {
                                                        string[] keyvalsplit = kv.Split(':');
                                                        string key = keyvalsplit[0];
                                                        string val = keyvalsplit[1];
                                                        switch (key.Trim('"'))
                                                        {
                                                            case "t":
                                                                container = val == "2" || val == "3";
                                                                break;
                                                            case "n":       // NPC 'Name'
                                                                boss = val.Replace("\\'", "'").Trim('"');
                                                                break;
                                                            case "z":       // Zone
                                                                area = GetZoneName(val);
                                                                break;
                                                            case "dd":      // Dungeon Difficulty (1 = Normal, 2 = Heroic)
                                                                heroic = val == "2";
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (boss == null) 
                                        { 
                                            //boss = "Unknown Boss (Wowhead lacks data)";
                                            area = null; // if boss is null prefer treating this as pve token
                                        }*//*
                                        if (tokenNames[i] != null)
                                        {
                                            _tokenDropMap[tokenIds[i]] = new TokenDropInfo() { Boss = boss, Area = area, Heroic = heroic, Name = tokenNames[i], Container = container };
                                        }
                                    }*/
                                    #endregion
                                }
                                else
                                {
                                    #region We've seen this before so just use that data
                                    TokenDropInfo info = _tokenDropMap[tokenIds[i]];
                                    boss = info.Boss;
                                    area = info.Area;
                                    heroic = info.Heroic;
                                    tokenNames[i] = info.Name;
                                    container = info.Container;
                                    #endregion
                                }
                                if (area != null)
                                {
                                    #region This is a Dropped Token, so assign it to where it drops from
                                    if (container)
                                    {
                                        ItemLocation locInfo = new ContainerItem()
                                        {
                                            Area = area,
                                            Container = boss,
                                            Heroic = heroic
                                        };
                                        LocationFactory.Add(item.Id.ToString(), locInfo);
                                        vendorItem = null;
                                        break;
                                    }
                                    else
                                    {
                                        ItemLocation locInfo = new StaticDrop()
                                        {
                                            Area = area,
                                            Boss = boss,
                                            Heroic = heroic
                                        };
                                        LocationFactory.Add(item.Id.ToString(), locInfo);
                                        vendorItem = null;
                                        break;
                                    }
                                    #endregion
                                }
                                else if (tokenNames[i] != null)
                                {
                                    #region This is NOT a Dropped Token, so treat it as a normal vendor item and include token info
                                    vendorItem.TokenMap[tokenNames[i]] = tokenCounts[i];
                                    #endregion
                                }
                                else /*if (tokenNames[i] == null)*/
                                {
                                    // there was an error pulling token data from web
                                    // ignore source information 
                                    vendorItem = null;
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                #region There is no token so this is a normal vendor item
                                if (!string.IsNullOrEmpty(repSource) && !string.IsNullOrEmpty(repLevel))
                                {
                                    string[] repInfo = GetItemFactionVendorInfo(repSource, repLevel);
                                    FactionItem locInfo = new FactionItem()
                                    {
                                        FactionName = repInfo[0],
                                        Level = (ReputationLevel)int.Parse(repLevel), // repInfo[3]
                                        Cost = cost,
                                    };
                                    LocationFactory.Add(item.Id.ToString(), locInfo);
                                    vendorItem = null;
                                    break;
                                }
                                else
                                {
                                    VendorItem locInfo = new VendorItem()
                                    {
                                        Cost = cost,
                                    };
                                    if (!string.IsNullOrEmpty(n)) locInfo.VendorName = n;
                                    if (sourcemore != null && sourcemore.TryGetValue("z", out tmp))
                                    {
                                        locInfo.VendorArea = GetZoneName(tmp.ToString());
                                    }
                                    LocationFactory.Add(item.Id.ToString(), locInfo);
                                    vendorItem = null;
                                    break;
                                }
                                #endregion
                            }
                        }
                        if (vendorItem != null)
                        {
                            if (!string.IsNullOrEmpty(n)) vendorItem.VendorName = n;
                            if (sourcemore != null && sourcemore.TryGetValue("z", out tmp))
                            {
                                vendorItem.VendorArea = GetZoneName(tmp.ToString());
                            }
                            LocationFactory.Add(item.Id.ToString(), vendorItem);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Process any other Sources
                        if (sourcemore != null)
                        {
                            if (sourcemore.TryGetValue("t", out tmp))
                            {
                                /*
                                //#define CTYPE_NPC            1
                                //#define CTYPE_OBJECT         2
                                //#define CTYPE_ITEM           3
                                //#define CTYPE_ITEMSET        4
                                //#define CTYPE_QUEST          5
                                //#define CTYPE_SPELL          6
                                //#define CTYPE_ZONE           7
                                //#define CTYPE_FACTION        8
                                //#define CTYPE_PET            9
                                //#define CTYPE_ACHIEVEMENT    10
                                */
                                switch ((int)tmp)
                                {
                                    case 1: //Dropped by a mob...
                                        StaticDrop staticDrop = StaticDrop.Construct() as StaticDrop;
                                        if (sourcemore.TryGetValue("n", out tmp))
                                        {
                                            staticDrop.Boss = tmp.ToString();
                                        }
                                        if (sourcemore.TryGetValue("z", out tmp) || sourcemore.TryGetValue("c", out tmp))
                                        {
                                            staticDrop.Area = GetZoneName(tmp.ToString());
                                        }
                                        if (sourcemore.TryGetValue("dd", out tmp))
                                        {
                                            string value = tmp.ToString();
                                            staticDrop.Heroic = (value == "-2" || value == "3" || value == "4");
                                            staticDrop.Area += (value == "1" || value == "3") ? " (10)" : ((value == "2" || value == "4") ? " (25)" : string.Empty);
                                        }
                                        LocationFactory.Add(item.Id.ToString(), staticDrop);
                                        break;

                                    case 2: //Found in a container object
                                        ContainerItem containerItem = ContainerItem.Construct() as ContainerItem;
                                        if (sourcemore.TryGetValue("n", out tmp))
                                        {
                                            containerItem.Container = tmp.ToString();
                                        }
                                        if (sourcemore.TryGetValue("z", out tmp) || sourcemore.TryGetValue("c", out tmp))
                                        {
                                            containerItem.Area = GetZoneName(tmp.ToString());
                                        }
                                        if (sourcemore.TryGetValue("dd", out tmp))
                                        {
                                            string value = tmp.ToString();
                                            containerItem.Heroic = (value == "-2" || value == "3" || value == "4");
                                            containerItem.Area += (value == "1" || value == "3") ? " (10)" : ((value == "2" || value == "4") ? " (25)" : string.Empty);
                                        }
                                        LocationFactory.Add(item.Id.ToString(), containerItem);
                                        break;

                                    case 5: //Rewarded from a quest...
                                        QuestItem questName = QuestItem.Construct() as QuestItem;
                                        if (sourcemore.TryGetValue("ti", out tmp))
                                        {
                                            /*WebRequestWrapper wrwti = new WebRequestWrapper();
                                            string questItem = wrwti.DownloadQuestWowhead(tmp.ToString());
                                            if (questItem != null && !questItem.Contains("This quest doesn't exist or is not yet in the database."))
                                            {
                                                int levelStart = questItem.IndexOf("<div>Required level: ") + 21;
                                                if (levelStart == 20)
                                                {
                                                    levelStart = questItem.IndexOf("<div>Requires level ") + 20;
                                                }
                                                if (levelStart > 19)
                                                {
                                                    int levelEnd = questItem.IndexOf("</div>", levelStart);
                                                    string level = questItem.Substring(levelStart, levelEnd - levelStart);
                                                    if (level == "??")
                                                    {
                                                        levelStart = questItem.IndexOf("<div>Level: ") + 12;
                                                        levelEnd = questItem.IndexOf("</div>", levelStart);
                                                        questName.MinLevel = int.Parse(questItem.Substring(levelStart, levelEnd - levelStart));
                                                    }
                                                    else
                                                    {
                                                        questName.MinLevel = int.Parse(level);
                                                    }
                                                }

                                                int typeStart = questItem.IndexOf("<div>Type: ") + 11;
                                                if (typeStart > 10)
                                                {
                                                    int typeEnd = questItem.IndexOf("</div>", typeStart);
                                                    switch (questItem.Substring(typeStart, typeEnd - typeStart))
                                                    {
                                                        case "Group":
                                                            int partyStart = questItem.IndexOf("Suggested Players [") + 19;
                                                            if (partyStart > 18)
                                                            {
                                                                int partyEnd = questItem.IndexOf("]", partyStart);
                                                                questName.Party = int.Parse(questItem.Substring(partyStart, partyEnd - partyStart));
                                                            }
                                                            break;

                                                        case "Dungeon": questName.Type = "d"; break;
                                                        case "Raid": questName.Type = "r"; break;
                                                        default: questName.Type = ""; break;
                                                    }
                                                }
                                            }*/
                                        }
                                        if (sourcemore.TryGetValue("n", out tmp))
                                        {
                                            questName.Quest = tmp.ToString();
                                        }
                                        if (sourcemore.TryGetValue("z", out tmp) || sourcemore.TryGetValue("c", out tmp))
                                        {
                                            questName.Area = GetZoneName(tmp.ToString());
                                        }
                                        LocationFactory.Add(item.Id.ToString(), questName);
                                        break;

                                    case 6: //Crafted by a profession...
                                        CraftedItem craftedItem = CraftedItem.Construct() as CraftedItem;
                                        if (sourcemore.TryGetValue("n", out tmp))
                                        {
                                            craftedItem.SpellName = tmp.ToString();
                                        }
                                        if (sourcemore.TryGetValue("z", out tmp))
                                        {
                                            craftedItem.Skill = GetZoneName(tmp.ToString());
                                        }
                                        if (sourcemore.TryGetValue("s", out tmp))
                                        {
                                            string profession = "";
                                            switch (Rawr.Properties.GeneralSettings.Default.Locale)
                                            {
                                                case "fr":
                                                    switch (tmp.ToString())
                                                    {
                                                        case "171": profession = "Alchimie"; break;
                                                        case "164": profession = "Forge"; break;
                                                        case "333": profession = "Enchantement"; break;
                                                        case "202": profession = "Ingénierie"; break;
                                                        case "182": profession = "Herboristerie"; break;
                                                        case "773": profession = "Calligraphie"; break;
                                                        case "755": profession = "Joaillerie"; break;
                                                        case "165": profession = "Travail du cuir"; break;
                                                        case "186": profession = "Minage"; break;
                                                        case "393": profession = "Dépeçage"; break;
                                                        case "197": profession = "Couture"; break;

                                                        default:
                                                            "".ToString();
                                                            break;
                                                    }
                                                    break;

                                                default:
                                                    switch (tmp.ToString())
                                                    {
                                                        case "171": profession = "Alchemy"; break;
                                                        case "164": profession = "Blacksmithing"; break;
                                                        case "333": profession = "Enchanting"; break;
                                                        case "202": profession = "Engineering"; break;
                                                        case "182": profession = "Herbalism"; break;
                                                        case "773": profession = "Inscription"; break;
                                                        case "755": profession = "Jewelcrafting"; break;
                                                        case "165": profession = "Leatherworking"; break;
                                                        case "186": profession = "Mining"; break;
                                                        case "393": profession = "Skinning"; break;
                                                        case "197": profession = "Tailoring"; break;

                                                        default:
                                                            "".ToString();
                                                            break;
                                                    }
                                                    break;
                                            }
                                            if (!string.IsNullOrEmpty(profession)) craftedItem.Skill = profession;
                                        }
                                        LocationFactory.Add(item.Id.ToString(), craftedItem);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            if (sourcemore.TryGetValue("p", out tmp))
                            {
                                LocationFactory.Add(item.Id.ToString(), PvpItem.Construct());
                                (item.LocationInfo[0] as PvpItem).Points = 0;
                                (item.LocationInfo[0] as PvpItem).PointType = "PvP";
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (item.Stats.Resilience > 0)
                {
                    // We DON'T have Source Data, BUT the item has resilience on it, so it's a pvp item
                    PvpItem locInfo = new PvpItem();
                    //locInfo.
                    LocationFactory.Add(item.Id.ToString(), locInfo);
                }
                else
                {
                    // We DON'T have Source Data
                    // Since we are doing nothing, the ItemSource cache doesn't change
                    // Therefore the original ItemSource persists, if it's there
                }
                #endregion

                #region Meta Gem Effects
                if (item.Slot == ItemSlot.Meta)
                {
                    if (htmlTooltip.Contains("<span class=\"q1\">") && htmlTooltip.Contains("</span>"))
                    {
                        string line = htmlTooltip.Substring(htmlTooltip.IndexOf("<span class=\"q1\">") + "<span class=\"q1\">".Length);
                        line = line.Substring(0, line.IndexOf("</span>"));
                        if (line.Contains("<a"))
                        {
                            {
                                int start = line.IndexOf("<a");
                                int end = line.IndexOf(">", start + 1);
                                line = line.Remove(start, end - start + 1);
                            }
                            {
                                int start = line.IndexOf("</a>");
                                line = line.Remove(start, 4);
                            }
                        }
                        SpecialEffects.ProcessMetaGem(line, item.Stats, false);
                    }
                    else throw (new Exception("Unhandled Metagem:\r\n" + item.Name));
                }
                #endregion

                // If it's Craftable and Bings on Pickup, mark it as such
                if (item.LocationInfo[0] is CraftedItem && (item.Bind == BindsOn.BoP))
                {
                    (item.LocationInfo[0] as CraftedItem).Bind = BindsOn.BoP;
                }

                #region Special Effects
                List<string> useLines = new List<string>();
                List<string> equipLines = new List<string>();
                while (htmlTooltip.Contains("<span class=\"q2\">") && htmlTooltip.Contains("</span>"))
                {
                    htmlTooltip = htmlTooltip.Substring(htmlTooltip.IndexOf("<span class=\"q2\">") + "<span class=\"q2\">".Length);
                    string line = htmlTooltip.Substring(0, htmlTooltip.IndexOf("</span>"));

                    // Remove Comments
                    while (line.Contains("<!--"))
                    {
                        int start = line.IndexOf("<!--");
                        int end = line.IndexOf("-->");
                        string toRemove = line.Substring(start, end - start + 3);
                        line = line.Replace(toRemove, "");
                    }
                    // Swap out to real spaces
                    while (line.Contains("&nbsp;")) { line = line.Replace("&nbsp;", " "); }
                    // Remove the Spell Links
                    // Later we will instead USE the spell links but we aren't set up for that right now
                    while (line.Contains("<a"))
                    {
                        int start = line.IndexOf("<a");
                        int end = line.IndexOf(">");
                        string toRemove = line.Substring(start, end - start + 1);
                        line = line.Replace(toRemove, "");
                    }
                    while (line.Contains("</a>")) { line = line.Replace("</a>", ""); }
                    // Remove the Small tags, we don't use those
                    while (line.Contains("<small"))
                    {
                        int start = line.IndexOf("<small>");
                        int end = line.IndexOf("</small>");
                        string toRemove = line.Substring(start, end - start + "</small>".Length);
                        line = line.Replace(toRemove, "");
                    }
                    // Remove double spaces
                    while (line.Contains("  ")) { line = line.Replace("  ", " "); }
                    // Swap out "sec." with "sec" as sometimes they
                    // do and sometimes they don't, regex for both is annoying
                    while (line.Contains("sec.")) { line = line.Replace("sec.", "sec"); }

                    // Now Process it
                    if (line.StartsWith("Equip: "))
                    {
                        string equipLine = line.Substring("Equip: ".Length);
                        equipLines.Add(equipLine);
                    }
                    else if (line.StartsWith("Chance on hit: "))
                    {
                        string chanceLine = line.Substring("Chance on hit: ".Length);
                        equipLines.Add(chanceLine);
                    }
                    else if (line.StartsWith("Use: "))
                    {
                        string useLine = line.Substring("Use: ".Length);
                        useLines.Add(useLine);
                    }
                    htmlTooltip = htmlTooltip.Substring(line.Length + "</span>".Length);
                }
                foreach (string useLine in useLines) SpecialEffects.ProcessUseLine(useLine, item.Stats, false, item.Id);
                foreach (string equipLine in equipLines) SpecialEffects.ProcessEquipLine(equipLine, item.Stats, false, item.ItemLevel, item.Id);
                #endregion

                #region Armor vs Bonus Armor Fixes
                if (item.Slot == ItemSlot.Finger ||
                    item.Slot == ItemSlot.MainHand ||
                    item.Slot == ItemSlot.Neck ||
                    (item.Slot == ItemSlot.OffHand && item.Type != ItemType.Shield) ||
                    item.Slot == ItemSlot.OneHand ||
                    item.Slot == ItemSlot.Trinket ||
                    item.Slot == ItemSlot.TwoHand)
                {
                    item.Stats.BonusArmor += item.Stats.Armor;
                    item.Stats.Armor = 0f;
                }
                else if (item.Stats.Armor + item.Stats.BonusArmor == 0f)
                { //Fix for wowhead bug where guns/bows/crossbows show up with 0 total armor, but 24.5 (or some such) bonus armor (they really have no armor at all)
                    item.Stats.Armor = 0;
                    item.Stats.BonusArmor = 0;
                }
                #endregion

                #region Belongs to a Set
                if (htmlTooltip.Contains(" (0/"))
                {
                    htmlTooltip = htmlTooltip.Substring(0, htmlTooltip.IndexOf("</a> (0/"));
                    htmlTooltip = htmlTooltip.Substring(htmlTooltip.LastIndexOf(">") + 1);
                    htmlTooltip = htmlTooltip.Replace("Wrathful ", "").Replace("Relentless ", "").Replace("Furious ", "").Replace("Deadly ", "").Replace("Hateful ", "").Replace("Savage ", "")
                        .Replace("Brutal ", "").Replace("Vengeful ", "").Replace("Merciless ", "").Replace("Valorous ", "")
                        .Replace("Heroes' ", "").Replace("Conqueror's ", "").Replace("Totally ", "").Replace("Triumphant ", "").Replace("Kirin'dor", "Kirin Tor").Replace("Regaila", "Regalia").Replace("Sanctified ", "");

                    if (htmlTooltip.Contains("Sunstrider's") || htmlTooltip.Contains("Zabra's") ||
                        htmlTooltip.Contains("Gul'dan's") || htmlTooltip.Contains("Garona's") ||
                        htmlTooltip.Contains("Runetotem's") || htmlTooltip.Contains("Windrunner's Pursuit") ||
                        htmlTooltip.Contains("Thrall's") || htmlTooltip.Contains("Liadrin's") ||
                        htmlTooltip.Contains("Hellscream's") || htmlTooltip.Contains("Kolitra's") || htmlTooltip.Contains("Koltira's"))
                    {
                        item.Faction = ItemFaction.Horde;
                    }
                    else if (htmlTooltip.Contains("Khadgar's") || htmlTooltip.Contains("Velen's") ||
                        htmlTooltip.Contains("Kel'Thuzad's") || htmlTooltip.Contains("VanCleef's") ||
                        htmlTooltip.Contains("Malfurion's") || htmlTooltip.Contains("Windrunner's Battlegear") ||
                        htmlTooltip.Contains("Nobundo's") || htmlTooltip.Contains("Turalyon's") ||
                        htmlTooltip.Contains("Wrynn's") || htmlTooltip.Contains("Thassarian's"))
                    {
                        item.Faction = ItemFaction.Alliance;
                    }

                    // normalize alliance/horde set names
                    htmlTooltip = htmlTooltip.Replace("Sunstrider's", "Khadgar's")   // Mage T9
                                             .Replace("Zabra's", "Velen's") // Priest T9
                                             .Replace("Gul'dan's", "Kel'Thuzad's") // Warlock T9
                                             .Replace("Garona's", "VanCleef's") // Rogue T9
                                             .Replace("Runetotem's", "Malfurion's") // Druid T9
                                             .Replace("Windrunner's Pursuit", "Windrunner's Battlegear") // Hunter T9
                                             .Replace("Thrall's", "Nobundo's") // Shaman T9
                                             .Replace("Liadrin's", "Turalyon's") // Paladin T9
                                             .Replace("Hellscream's", "Wrynn's") // Warrior T9
                                             .Replace("Koltira's", "Thassarian's")  // Death Knight T9
                                             .Replace("Kolitra's", "Thassarian's"); // Death Knight T9
                    item.SetName = htmlTooltip.Trim();
                }
                #endregion

                // Filter out random suffix greens
                /*if (filter
                    && item.Quality == ItemQuality.Uncommon
                    && item.Stats <= new Stats() { Armor = 99999, AttackPower = 99999, SpellPower = 99999, BlockValue = 99999 })
                { e.Result = null; return; }*/
                #endregion
            } catch (Exception ex) {
                (sender as BackgroundWorker).ReportProgress(0, ex.Message + "|" + ex.StackTrace);
            }
        }

        private static ItemType GetItemType(string subclassName, int inventoryType, int classId)
        {
            switch (subclassName)
            {
                case "Cloth": return ItemType.Cloth;
                case "Leather": return ItemType.Leather;
                case "Mail": return ItemType.Mail;
                case "Plate": return ItemType.Plate;
                case "Dagger": return ItemType.Dagger;
                case "Fist Weapon": return ItemType.FistWeapon;
                case "Axe": return (inventoryType == 17 ? ItemType.TwoHandAxe : ItemType.OneHandAxe);
                case "Mace": return (inventoryType == 17 ? ItemType.TwoHandMace : ItemType.OneHandMace);
                case "Sword": return (inventoryType == 17 ? ItemType.TwoHandSword : ItemType.OneHandSword);
                case "Polearm": return ItemType.Polearm;
                case "Staff": return ItemType.Staff;
                case "Shield": return ItemType.Shield;
                case "Bow": return ItemType.Bow;
                case "Crossbow": return ItemType.Crossbow;
                case "Gun": return ItemType.Gun;
                case "Wand": return ItemType.Wand;
                case "Thrown": return ItemType.Thrown;
                case "Idol": return ItemType.Idol;
                case "Libram": return ItemType.Libram;
                case "Totem": return ItemType.Totem;
                case "Arrow": return ItemType.Arrow;
                case "Bullet": return ItemType.Bullet;
                case "Quiver": return ItemType.Quiver;
                case "Ammo Pouch": return ItemType.AmmoPouch;
                case "Sigil": return ItemType.Sigil;
                default: return ItemType.None;
            }
        }

        private static ItemSlot GetItemSlot(int inventoryType, int classId)
        {
            switch (classId)
            {
                case 6: return ItemSlot.Projectile;
                case 11: return ItemSlot.ProjectileBag;
            }
            switch (inventoryType)
            {
                case 1: return ItemSlot.Head;
                case 2: return ItemSlot.Neck;
                case 3: return ItemSlot.Shoulders;
                case 16: return ItemSlot.Back;
                case 5:
                case 20: return ItemSlot.Chest;
                case 4: return ItemSlot.Shirt;
                case 19: return ItemSlot.Tabard;
                case 9: return ItemSlot.Wrist;
                case 10: return ItemSlot.Hands;
                case 6: return ItemSlot.Waist;
                case 7: return ItemSlot.Legs;
                case 8: return ItemSlot.Feet;
                case 11: return ItemSlot.Finger;
                case 12: return ItemSlot.Trinket;
                case 13: return ItemSlot.OneHand;
                case 17: return ItemSlot.TwoHand;
                case 21: return ItemSlot.MainHand;
                case 14:
                case 22:
                case 23: return ItemSlot.OffHand;
                case 15:
                case 25:
                case 26:
                case 28: return ItemSlot.Ranged;
                case 24: return ItemSlot.Projectile;
                case 27: return ItemSlot.ProjectileBag;
                default: return ItemSlot.None;
            }
        }
        #endregion

        // ==============================================
        //
        // COPIED FROM Rawr2
        //
        // ==============================================

        public static Item ProcessItem(string itemData)
        {
            string data = itemData.Replace("[,{", "[{");
            string id = data.Substring(0, data.IndexOf(','));
            data = data.Substring(data.IndexOf(',') + 1);
            string name = string.Empty;
            if (data.StartsWith("\""))
            {
                name = data.Substring(1, data.IndexOf("\",") - 1);
            }
            else
            {
                name = data.Substring(0, data.IndexOf(','));
            }
            data = data.Replace(name, "").TrimStart('"').TrimStart(',');
            string quality = data.Substring(0, data.IndexOf(','));
            data = data.Substring(data.IndexOf(',') + 2);

            string json1 = data.Substring(0, data.IndexOf("\",")).Replace(",subclass:", ".").Replace(",armor:", ",armorDUPE:");
            data = data.Substring(data.IndexOf("\",") + 2);
            string json2 = data.Trim('"');//string.IsNullOrEmpty(data) ? string.Empty : data.Substring(1, data.IndexOf('"'));

            string source = string.Empty;
            if (json1.Contains("source:["))
            {
                source = json1.Substring(json1.IndexOf("source:[") + "source:[".Length);
                source = source.Substring(0, source.IndexOf("]"));
                json1 = json1.Replace(string.Format("source:[{0}]", source), "source:[SOURCE]");
            }

            string sourcemore = string.Empty;
            if (json1.Contains("sourcemore:[{"))
            {
                sourcemore = json1.Substring(json1.IndexOf("sourcemore:[{") + "sourcemore:[{".Length);
                sourcemore = sourcemore.Substring(0, sourcemore.IndexOf("}]"));
                json1 = json1.Replace(sourcemore, "SOURCEMORE");
            }

            Item item = new Item()
            {
                Id = int.Parse(id),
                Name = name,
                Quality = (ItemQuality)int.Parse(quality),
                IconPath = string.Empty,
                Stats = new Stats()
            };
            //item.Stats += GetAdditionalItemEffect(item._id);

            if ((int)item.Quality < 2) return null;

            foreach (string keyval in (json1 + "," + json2).Split(','))
            {
                if (!string.IsNullOrEmpty(keyval))
                {
                    string[] keyvalsplit = keyval.Split(':');
                    string key = keyvalsplit[0];
                    string val = keyvalsplit[1];
                    if (ProcessKeyValue(item, key, val) && !json1.Contains("classs:3."))
                        return null;
                }
            }
            if (item.Slot == ItemSlot.None) return null;
            if (!string.IsNullOrEmpty(source)) ProcessKeyValue(item, "source", source);
            if (!string.IsNullOrEmpty(sourcemore))
            {
                string n = string.Empty;
                if (sourcemore.Contains("n:'"))
                {
                    n = sourcemore.Substring(sourcemore.IndexOf("n:'") + "n:'".Length);
                    n = n.Substring(0, n.IndexOf("'"));
                    if (!string.IsNullOrEmpty(n))
                        sourcemore = sourcemore.Replace(n, "N");
                }

                foreach (string keyval in sourcemore.Replace("},", ",").Replace(",{", ",").Split(','))
                {
                    if (!string.IsNullOrEmpty(keyval))
                    {
                        string[] keyvalsplit = keyval.Split(':');
                        string key = keyvalsplit[0];
                        string val = keyvalsplit[1];
                        ProcessKeyValue(item, key, val);
                    }
                }
                if (!string.IsNullOrEmpty(n)) ProcessKeyValue(item, "n", n);
            }
            if (item.Quality == ItemQuality.Uncommon && item.Stats <= new Stats() { Armor = 99999, AttackPower = 99999, SpellPower = 99999, BlockValue = 99999 }) return null; //Filter out random suffix greens
            return item;
        }

        private class TokenDropInfo
        {
            public string Boss;
            public string Area;
            public bool Heroic;
            public bool Container;
            public string Name;
        }
        /*private class UnkDropInfo
        {
            public string Boss;
            public string Area;
            public bool Heroic;
            public bool Container;
            public string Name;
        }*/

        static Dictionary<string, TokenDropInfo> _tokenDropMap = new Dictionary<string, TokenDropInfo>();
        //static Dictionary<string, UnkDropInfo> _UnkDropMap = new Dictionary<string, UnkDropInfo>();
        static Dictionary<string, string> _pvpTokenMap = new Dictionary<string, string>();
        static Dictionary<string, string> _vendorTokenMap = new Dictionary<string, string>();

        static WowheadService()
        {
            switch (Rawr.Properties.GeneralSettings.Default.Locale)
            {
                case "fr":
                    _pvpTokenMap["20560"] = "Marque d'honneur de la vallée d'Alterac";
                    _pvpTokenMap["20559"] = "Marque d'honneur du bassin d'Arathi";
                    _pvpTokenMap["20558"] = "Marque d'honneur du goulet des Chanteguerres";
                    _pvpTokenMap["29024"] = "Marque d'honneur de l'Oeil du cyclone";
                    _pvpTokenMap["37836"] = "Pièce de la KapitalRisk";
                    _pvpTokenMap["42425"] = "Marque d'honneur du rivage des Anciens";
                    _pvpTokenMap["43589"] = "Marque d'honneur de Joug-d'hiver";

                    _vendorTokenMap["44990"] = "Sceau de champion";
                    _vendorTokenMap["40752"] = "Emblème d'héroïsme";
                    _vendorTokenMap["40753"] = "Emblème de vaillance";
                    _vendorTokenMap["45624"] = "Emblème de conquête";
                    break;

                default:
                    _pvpTokenMap["20560"] = "Alterac Valley Mark of Honor";
                    _pvpTokenMap["20559"] = "Arathi Basin Mark of Honor";
                    _pvpTokenMap["20558"] = "Warsong Gulch Mark of Honor";
                    _pvpTokenMap["29024"] = "Eye of the Storm Mark of Honor";
                    _pvpTokenMap["37836"] = "Venture Coin";
                    _pvpTokenMap["42425"] = "Strand of the Ancients Mark of Honor";
                    _pvpTokenMap["43589"] = "Wintergrasp Mark of Honor";

                    _vendorTokenMap["44990"] = "Champion's Seal";
                    _vendorTokenMap["40752"] = "Emblem of Heroism";
                    _vendorTokenMap["40753"] = "Emblem of Valor";
                    _vendorTokenMap["45624"] = "Emblem of Conquest";
                    _vendorTokenMap["47241"] = "Emblem of Triumph";
                    _vendorTokenMap["47242"] = "Trophy of the Crusade";
                    _vendorTokenMap["49426"] = "Emblem of Frost";
                    break;
            }
        }
        
        private static List<string> _unhandledKeys = new List<string>();
        private static List<string> _unhandledSocketBonus = new List<string>();
        private static bool ProcessKeyValue(Item item, string key, string value)
        {
            switch (key.Trim('"'))
            {
                #region Item Info/Stat Keys
                case "id": //ID's are parsed out of the main data, not the json
                case "name": //Item names are parsed out of the main data, not the json
                case "subclass": //subclass is combined with class
                case "subsubclass": //Only used for Battle vs Guardian Elixirs
                case "buyprice": //Rawr doesn't care about buy...
                case "sellprice": //...and sell prices
                case "reqlevel": //Rawr assumes that you meet all level requirements
                case "dps": //Rawr calculates weapon dps based on min/max and speed
                case "maxcount": //Rawr doesn't deal with stack sizes
                case "dura": //durability isn't handled
                case "nsockets": //Rawr figures this out itself, Smart program.
                case "races": //Not worried about race restrictions
                case "source": //Handled below by individual keyvals
                case "sourcemore": //Handled below by individual keyvals
                case "nslots": //Don't care about bag sizes...
                case "avgmoney": //For containers, average amount of money inside
                case "glyph": //1=Major, 2=Minor
                    break;
                case "displayid": //A 3d display ID# for each icon
                    item.DisplayId = int.Parse(value);
                    break;
                case "slotbak": //A couple slots actually have two possible slots... ie vests and robes both fit in chest. slotbak distinguishes vests from robes. We don't care for Rawr, so ignored.
                    item.DisplaySlot = int.Parse(value); // it is also used for the 3d display slot id
                    break;
                case "level": //Rawr now handles item levels
                    item.ItemLevel = int.Parse(value);
                    break;
                //TODO:
                case "cooldown": //Not handled yet
                case "skill": //Related to skill requirements
                case "reqskill": //Related to skill requirements
                case "reqskillrank": //Related to skill requirements
                case "reqrep": //reqrep=6: heroic 5 man, reqrep=1: heroic raid, reqrep=5: Arena, reqrep=4: Faction Friendly, reqrep=5: Faction Honored, etc
                case "reqfaction": //Currently faction & reputation is not handled.
                case "itemset": //Contains the itemset id... May want to parse this,
                case "reqspell": //Profession specialization requirements, like weaponcrafting, armorsmithing, etc
                    break;

                case "slot":
                    int slot = int.Parse(value);
                    if (slot == 0) return true; //Not Equippable
                    item.Slot = GetItemSlot(slot);
                    break;

                case "classs":
                    if (value.StartsWith("1.") || value.StartsWith("12.")) return true; //Container and Quest
                    if (value.StartsWith("3."))
                    {
                        item.Type = ItemType.None;
                        switch (value)
                        {
                            case "3.0": item.Slot = ItemSlot.Red; break;
                            case "3.1": item.Slot = ItemSlot.Blue; break;
                            case "3.2": item.Slot = ItemSlot.Yellow; break;
                            case "3.3": item.Slot = ItemSlot.Purple; break;
                            case "3.4": item.Slot = ItemSlot.Green; break;
                            case "3.5": item.Slot = ItemSlot.Orange; break;
                            case "3.6": item.Slot = ItemSlot.Meta; break;
                            case "3.8": item.Slot = ItemSlot.Prismatic; break;
                        }
                    }
                    else
                    {
                        item.Type = GetItemType(value);
                    }
                    break;

                case "speed":
                    item.Speed = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "dmgmin1":
                    item.MinDamage += (int)float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "dmgmax1":
                    item.MaxDamage += (int)float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "dmgtype1":
                    item.DamageType = (ItemDamageType)int.Parse(value);
                    break;

                case "armor":
                    item.Stats.Armor = int.Parse(value);
                    break;

                case "armorbonus":
                    item.Stats.Armor -= float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    item.Stats.BonusArmor = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "healthrgn":
                    item.Stats.Hp5 += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "manargn":
                    item.Stats.Mp5 += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "health":
                    item.Stats.Health += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "agi":
                    item.Stats.Agility += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "int":
                    item.Stats.Intellect += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "spi":
                    item.Stats.Spirit += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "sta":
                    item.Stats.Stamina += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "str":
                    item.Stats.Strength += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "hastertng":
                case "mlehastertng":
                case "rgdhastertng":
                case "splhastertng":
                    item.Stats.HasteRating = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "splpwr":
                case "splheal":
                case "spldmg":
                    item.Stats.SpellPower = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "critstrkrtng":
                case "mlecritstrkrtng":
                case "rgdcritstrkrtng":
                case "splcritstrkrtng":
                    item.Stats.CritRating = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "holres":
                    break;

                case "firres":
                    item.Stats.FireResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "natres":
                    item.Stats.NatureResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "frores":
                    item.Stats.FrostResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "shares":
                    item.Stats.ShadowResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "arcres":
                    item.Stats.ArcaneResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "hitrtng":
                case "mlehitrtng":
                case "rgdhitrtng":
                case "splhitrtng":
                    item.Stats.HitRating = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "atkpwr":
                case "mleatkpwr":
                    //case "feratkpwr":
                    item.Stats.AttackPower += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "rgdatkpwr":
                    if (item.Stats.AttackPower != float.Parse(value, System.Globalization.CultureInfo.InvariantCulture))
                        item.Stats.RangedAttackPower = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "block":
                    item.Stats.BlockValue += int.Parse(value);
                    break;

                case "exprtng":
                    item.Stats.ExpertiseRating += int.Parse(value);
                    break;

                case "defrtng":
                    item.Stats.DefenseRating += int.Parse(value);
                    break;

                case "dodgertng":
                    item.Stats.DodgeRating += int.Parse(value);
                    break;

                case "blockrtng":
                    item.Stats.BlockRating += int.Parse(value);
                    break;

                case "socket1":
                    item.SocketColor1 = GetSocketType(value);
                    break;

                case "socket2":
                    item.SocketColor2 = GetSocketType(value);
                    break;

                case "socket3":
                    item.SocketColor3 = GetSocketType(value);
                    break;

                case "socketbonus":
                    item.SocketBonus = GetSocketBonus(value);
                    break;

                case "parryrtng":
                    item.Stats.ParryRating += int.Parse(value);
                    break;

                case "classes":
                    List<string> requiredClasses = new List<string>();
                    int classbitfield = int.Parse(value);
                    if ((classbitfield & 1) > 0)
                        requiredClasses.Add("Warrior");
                    if ((classbitfield & 2) > 0)
                        requiredClasses.Add("Paladin");
                    if ((classbitfield & 4) > 0)
                        requiredClasses.Add("Hunter");
                    if ((classbitfield & 8) > 0)
                        requiredClasses.Add("Rogue");
                    if ((classbitfield & 16) > 0)
                        requiredClasses.Add("Priest");
                    if ((classbitfield & 32) > 0)
                        requiredClasses.Add("Death Knight");
                    if ((classbitfield & 64) > 0)
                        requiredClasses.Add("Shaman");
                    if ((classbitfield & 128) > 0)
                        requiredClasses.Add("Mage");
                    if ((classbitfield & 256) > 0)
                        requiredClasses.Add("Warlock");
                    //if ((classbitfield & 512) > 0) ; // Only seems to occur in PvP gear, along with another huge value
                    //    requiredClasses.Add("");
                    if ((classbitfield & 1024) > 0)
                        requiredClasses.Add("Druid");
                    item.RequiredClasses = string.Join("|", requiredClasses.ToArray());
                    break;

                case "resirtng":
                    item.Stats.Resilience += int.Parse(value);
                    break;

                case "armorpen":
                case "armorpenrtng":
                    item.Stats.ArmorPenetrationRating = int.Parse(value);
                    break;

                case "splpen":
                    item.Stats.SpellPenetration = int.Parse(value);
                    break;

                case "mana":
                    item.Stats.Mana = int.Parse(value);
                    break;

                case "dmg":
                    item.Stats.WeaponDamage = int.Parse(value);
                    break;

                case "frospldmg":
                    item.Stats.SpellFrostDamageRating = int.Parse(value);
                    break;

                case "shaspldmg":
                    item.Stats.SpellShadowDamageRating = int.Parse(value);
                    break;

                case "firspldmg":
                    item.Stats.SpellFireDamageRating = int.Parse(value);
                    break;

                case "arcspldmg":
                    item.Stats.SpellArcaneDamageRating = int.Parse(value);
                    break;
                #endregion
                #region Source Keys
                case "t":   //Source Type
                    /*
                    //#define CTYPE_NPC            1
                    //#define CTYPE_OBJECT         2
                    //#define CTYPE_ITEM           3
                    //#define CTYPE_ITEMSET        4
                    //#define CTYPE_QUEST          5
                    //#define CTYPE_SPELL          6
                    //#define CTYPE_ZONE           7
                    //#define CTYPE_FACTION        8
                    //#define CTYPE_PET            9
                    //#define CTYPE_ACHIEVEMENT    10
                    */
                    switch (value)
                    {
                        case "1": //Dropped by a mob...
                            LocationFactory.Add(item.Id.ToString(), StaticDrop.Construct());
                            break;

                        case "2": //Found in a container object
                            LocationFactory.Add(item.Id.ToString(), ContainerItem.Construct());
                            break;

                        case "3": //Found in a container item
                            LocationFactory.Add(item.Id.ToString(), ContainerItem.Construct());
                            break;

                        case "5": //Rewarded from a quest...
                            LocationFactory.Add(item.Id.ToString(), QuestItem.Construct());
                            break;

                        case "6": //Crafted by a profession...
                            LocationFactory.Add(item.Id.ToString(), CraftedItem.Construct());
                            break;

                        default:
                            "".ToString();
                            break;
                    }
                    break;

                case "ti":      // NPC ID that drops/gives... We use the name, so ignoring this
                    ItemLocation questName = item.LocationInfo[0];
                    /*if (questName is QuestItem)
                    {
                        WebRequestWrapper wrw = new WebRequestWrapper();
                        string questItem = wrw.DownloadQuestWowhead(value);
                        if (questItem != null && !questItem.Contains("This quest doesn't exist or is not yet in the database."))
                        {
                            int levelStart = questItem.IndexOf("<div>Required level: ") + 21;
                            if (levelStart == 20)
                            {
                                levelStart = questItem.IndexOf("<div>Requires level ") + 20;
                            }
                            if (levelStart > 19)
                            {
                                int levelEnd = questItem.IndexOf("</div>", levelStart);
                                string level = questItem.Substring(levelStart, levelEnd - levelStart);
                                if (level == "??")
                                {
                                    levelStart = questItem.IndexOf("<div>Level: ") + 12;
                                    levelEnd = questItem.IndexOf("</div>", levelStart);
                                    (questName as QuestItem).MinLevel = int.Parse(questItem.Substring(levelStart, levelEnd - levelStart));
                                }
                                else
                                {
                                    (questName as QuestItem).MinLevel = int.Parse(level);
                                }
                            }

                            int typeStart = questItem.IndexOf("<div>Type: ") + 11;
                            if (typeStart > 10)
                            {
                                int typeEnd = questItem.IndexOf("</div>", typeStart);
                                switch (questItem.Substring(typeStart, typeEnd - typeStart))
                                {
                                    case "Group":
                                        int partyStart = questItem.IndexOf("Suggested Players [") + 19;
                                        if (partyStart > 18)
                                        {
                                            int partyEnd = questItem.IndexOf("]", partyStart);
                                            (questName as QuestItem).Party = int.Parse(questItem.Substring(partyStart, partyEnd - partyStart));
                                        }
                                        break;

                                    case "Dungeon": (questName as QuestItem).Type = "d"; break;
                                    case "Raid": (questName as QuestItem).Type = "r"; break;
                                    default: (questName as QuestItem).Type = ""; break;
                                }
                            }
                        }
                    }*/
                    break;

                case "n":       // NPC 'Name'
                    ItemLocation locationName = item.LocationInfo[0];
                    if (locationName is StaticDrop) (locationName as StaticDrop).Boss = value;
                    if (locationName is ContainerItem) (locationName as ContainerItem).Container = value;
                    if (locationName is QuestItem) (locationName as QuestItem).Quest = value;
                    if (locationName is CraftedItem) (locationName as CraftedItem).SpellName = value;
                    break;

                case "z":       // Zone
                    string zonename = GetZoneName(value);
                    ItemLocation locationZone = item.LocationInfo[0];
                    if (locationZone is StaticDrop) (locationZone as StaticDrop).Area = zonename;
                    else if (locationZone is ContainerItem) (locationZone as ContainerItem).Area = zonename;
                    else if (locationZone is QuestItem) (locationZone as QuestItem).Area = zonename;
                    else if (locationZone is CraftedItem) (locationZone as CraftedItem).Skill = value;
                    else if (locationZone is WorldDrop) (locationZone as WorldDrop).Location = zonename;
                    break;

                case "c": //Zone again, used for quests
                    string continentname = GetZoneName(value);
                    ItemLocation locationContinent = item.LocationInfo[0];
                    if (locationContinent is StaticDrop) (locationContinent as StaticDrop).Area = continentname;
                    else if (locationContinent is ContainerItem) (locationContinent as ContainerItem).Area = continentname;
                    else if (locationContinent is QuestItem) (locationContinent as QuestItem).Area = continentname;
                    break;

                case "c2": //Don't care about continent
                    break;

                case "dd":      // Dungeon Difficulty 
                    // -1 = Normal Dungeon,  -2 = Heroic Dungeon
                    //  1 = Normal Raid (10), 2 = Normal Raid (25)
                    //  3 = Heroic Raid (10), 4 = Heroic Raid (25)
                    //hasdd = true;
                    //ddheroic = (value == "-2" || value == "3" || value == "4");
                    //ddarea = (value == "1" || value == "3" ) ? " (10)" :
                    //	( (value == "2" || value == "4") ? " (25)" : string.Empty  );
                    break;

                case "s":
                    if (item.LocationInfo[0] is CraftedItem)
                    {
                        string profession = "";
                        switch (Rawr.Properties.GeneralSettings.Default.Locale)
                        {
                            case "fr":
                                switch (value)
                                {
                                    case "171": profession = "Alchimie"; break;
                                    case "164": profession = "Forge"; break;
                                    case "333": profession = "Enchantement"; break;
                                    case "202": profession = "Ingénierie"; break;
                                    case "182": profession = "Herboristerie"; break;
                                    case "773": profession = "Calligraphie"; break;
                                    case "755": profession = "Joaillerie"; break;
                                    case "165": profession = "Travail du cuir"; break;
                                    case "186": profession = "Minage"; break;
                                    case "393": profession = "Dépeçage"; break;
                                    case "197": profession = "Couture"; break;

                                    default:
                                        "".ToString();
                                        break;
                                }
                                break;

                            default:
                                switch (value)
                                {
                                    case "171": profession = "Alchemy"; break;
                                    case "164": profession = "Blacksmithing"; break;
                                    case "333": profession = "Enchanting"; break;
                                    case "202": profession = "Engineering"; break;
                                    case "182": profession = "Herbalism"; break;
                                    case "773": profession = "Inscription"; break;
                                    case "755": profession = "Jewelcrafting"; break;
                                    case "165": profession = "Leatherworking"; break;
                                    case "186": profession = "Mining"; break;
                                    case "393": profession = "Skinning"; break;
                                    case "197": profession = "Tailoring"; break;

                                    default:
                                        "".ToString();
                                        break;
                                }
                                break;
                        }
                        if (!string.IsNullOrEmpty(profession)) (item.LocationInfo[0] as CraftedItem).Skill = profession;
                    }
                    "".ToString();
                    break;

                case "q":
                    "".ToString();
                    break;

                case "p": //PvP
                    LocationFactory.Add(item.Id.ToString(), PvpItem.Construct());
                    (item.LocationInfo[0] as PvpItem).Points = 0;
                    (item.LocationInfo[0] as PvpItem).PointType = "PvP";
                    "".ToString();
                    break;
                #endregion

                default:
                    if (!_unhandledKeys.Contains(key))
                        _unhandledKeys.Add(key);
                    break;
            }
            return false;
        }

        private static string GetZoneName(string zoneId)
        {
            switch (Rawr.Properties.GeneralSettings.Default.Locale)
            {
                #region French Translations
                case "fr":
                    {
                        switch (zoneId)
                        {
                            case "65": return "Désolation des dragons";
                            case "66": return "Zul'Drak";
                            case "67": return "Les pics Foudroyés";
                            case "206": return "Donjon d'Utgarde";
                            case "210": return "La Couronne de glace";
                            case "394": return "Les Grisonnes";
                            case "495": return "Fjord Hurlant";
                            case "1196": return "Cime d'Utgarde";
                            case "2817": return "Forêt du Chant de cristal";
                            case "2917": return "Hall of Legends"; // Can't find the translation.
                            case "2918": return "Champion's Hall"; // Can't find the translation.
                            case "3456": return "Naxxramas";
                            case "3477": return "Azjol-Nérub";
                            case "3537": return "Toundra Boréenne";
                            case "3711": return "Bassin de Sholazar";
                            case "4100": return "L'Épuration de Stratholme";
                            case "4120": return "Le Nexus";
                            case "4196": return "Donjon de Drak'Tharon";
                            case "4197": return "Joug-d'hiver";
                            case "4228": return "L'Oculus";
                            case "4264": return "Les salles de Pierre";
                            case "4272": return "Les salles de Foudre";
                            case "4298": return "l'enclave Écarlate";
                            case "4375": return "Gundrak";
                            case "4395": return "Dalaran";
                            case "4415": return "Le fort Pourpre";
                            case "4493": return "Le sanctum Obsidien";
                            case "4494": return "Ahn'kahet";
                            case "4500": return "L'Œil de l'éternité";
                            case "4603": return "Caveau d'Archavon";
                            case "3520": return "Vallée d'Ombrelune";
                            case "4075": return "Plateau du Puits de soleil";
                            case "3959": return "Temple noir";
                            case "3606": return "Sommet d'Hyjal";
                            case "3607": return "Caverne du sanctuaire du Serpent";
                            case "3618": return "Repaire de Gruul";
                            case "3836": return "Le repaire de Magtheridon";
                            case "2562": return "Karazhan";
                            case "3842": return "L'Oeil";
                            case "3805": return "Zul'Aman";
                            case "4273": return "Ulduar";
                            default: return "Inconnue - " + zoneId;
                        }
                    }
                #endregion
                default:
                    {
                        switch (zoneId)
                        {
                            #region Old World
                            case "-364": return "Darkmoon Faire";
                            case "12": return "Elwynn Forest";
                            case "38": return "Loch Modan";
                            case "40": return "Westfall";
                            case "133": return "Gnomeregan";
                            case "209": return "Shadowfang Keep";
                            case "491": return "Razorfen Kraul";
                            case "717": return "The Stockade";
                            case "718": return "Wailing Caverns";
                            case "719": return "Blackfathom Deeps";
                            case "722": return "Razorfen Downs";
                            case "796": return "Scarlet Monastery";
                            case "978": return "Zul'Farrak";
                            case "1337": return "Uldaman";
                            case "1417": return "Sunken Temple";
                            case "1519": return "Stormwind City";
                            case "1581": return "The Deadmines";
                            case "1583": return "Blackrock Spire";
                            case "1584": return "Blackrock Depths";
                            case "2017": return "Stratholme";
                            case "2057": return "Scholomance";
                            case "2100": return "Maraudon";
                            case "2159": return "Onyxia's Lair";
                            case "2437": return "Ragefire Chasm";
                            case "2557": return "Dire Maul";
                            case "2597": return "Alterac Valley";
                            case "2677": return "Blackwing Lair";
                            case "2918": return "Champion's Hall";
                            #endregion
                            #region TBC
                            case "2366": return "The Black Morass";
                            case "2367": return "Old Hillsbrad Foothills";
                            case "2562": return "Karazhan";
                            case "3520": return "Shadowmoon Valley";
                            case "3523": return "Netherstorm";
                            case "3562": return "Hellfire Ramparts";
                            case "3606": return "Hyjal Summit";
                            case "3607": return "Serpentshrine Cavern";
                            case "3618": return "Gruul's Lair";
                            case "3703": return "Shattrath City";
                            case "3713": return "The Blood Furnace";
                            case "3714": return "The Shattered Halls";
                            case "3715": return "The Steamvault";
                            case "3716": return "The Underbog";
                            case "3717": return "The Slave Pens";
                            case "3789": return "Shadow Labyrinth";
                            case "3790": return "Auchenai Crypts";
                            case "3791": return "Sethekk Halls";
                            case "3792": return "Mana-Tombs";
                            case "3805": return "Zul'Aman";
                            case "3836": return "Magtheridon's Lair";
                            case "3842": return "The Eye";
                            case "3846": return "The Arcatraz";
                            case "3847": return "The Botanica";
                            case "3849": return "The Mechanar";
                            case "3959": return "Black Temple";
                            case "4075": return "Sunwell Plateau";
                            case "4080": return "Isle of Quel'Danas";
                            case "4095": return "Magisters' Terrace";
                            #endregion
                            #region WotLK
                            case "65": return "Dragonblight";
                            case "66": return "Zul'Drak";
                            case "67": return "The Storm Peaks";
                            case "206": return "Utgarde Keep";
                            case "210": return "Icecrown";
                            case "394": return "Grizzly Hills";
                            case "495": return "Howling Fjord";
                            case "1196": return "Utgarde Pinnacle";
                            case "2817": return "Crystalsong Forest";
                            case "2917": return "Hall of Legends";
                            case "3456": return "Naxxramas";
                            case "3477": return "Azjol-Nerub";
                            case "3537": return "Borean Tundra";
                            case "3711": return "Sholazar Basin";
                            case "4100": return "The Culling of Stratholme";
                            case "4120": return "The Nexus";
                            case "4196": return "Drak'Tharon Keep";
                            case "4197": return "Wintergrasp";
                            case "4228": return "The Oculus";
                            case "4264": return "Halls of Stone";
                            case "4272": return "Halls of Lightning";
                            case "4273": return "Ulduar";
                            case "4298": return "The Scarlet Enclave";
                            case "4375": return "Gundrak";
                            case "4395": return "Dalaran";
                            case "4415": return "The Violet Hold";
                            case "4493": return "The Obsidian Sanctum";
                            case "4494": return "Ahn'kahet: The Old Kingdom";
                            case "4500": return "The Eye of Eternity";
                            case "4603": return "Vault of Archavon";
                            case "4722": return "Trial of the Crusader";
                            case "4723": return "Trial of the Champion";
                            case "4809": return "Forge of Souls";
                            case "4812": return "Icecrown Citadel";
                            case "4813": return "Pit of Saron";
                            case "4820": return "Halls of Reflection";
                            case "4987": return "Ruby Sanctum";
                            #endregion
                            #region Cataclysm
                            // Don't have these yet
                            //case "65": return "Dragonblight";
                            #endregion
                            default: return "Unknown - " + zoneId;
                        }
                    }
            }
        }

        private static ItemSlot GetSocketType(string socket)
        {
            switch (socket)
            {
                case "1": return ItemSlot.Meta;
                case "2": return ItemSlot.Red;
                case "4": return ItemSlot.Yellow;
                case "6": return ItemSlot.Orange;
                case "8": return ItemSlot.Blue;
                case "10": return ItemSlot.Purple;
                case "12": return ItemSlot.Green;
                case "14": return ItemSlot.Prismatic;
                case "32": return ItemSlot.Cogwheel;
                // Dont have this id yet, but assuming 33 for now
                case "33": return ItemSlot.Hydraulic;
                default:
                    throw (new Exception("Unknown Slot Type :" + socket));
            }
        }

        private static Stats GetSocketBonus(string socketbonus)
        {
            Stats stats = new Stats();
            switch (socketbonus)
            {
                #region Spell Power
                case "2900": stats.SpellPower += 4; break;
                case "2872":
                case "2889":
                case "3198":
                case "3596":
                case "3752": stats.SpellPower += 5; break;
                case "428":
                case "2770":
                case "3602": stats.SpellPower += 7; break;
                case "430":
                case "440":
                case "2314":
                case "3753": stats.SpellPower += 9; break;
                #endregion
                #region Attack Power
                case "3114": stats.AttackPower += 4; break;
                case "2936": stats.AttackPower += 8; break;
                case "1587":
                case "3356":
                case "3764": stats.AttackPower += 12; break;
                case "3877":
                case "1589": stats.AttackPower += 16; break;
                case "1597": stats.AttackPower += 32; break;
                #endregion
                #region Stamina
                case "2895": stats.Stamina += 4; break;
                case "2868":
                case "2882": stats.Stamina += 6; break;
                case "1886":
                case "3307": stats.Stamina += 9; break;
                case "3354":
                case "3305":
                case "3766": stats.Stamina += 12; break;
                #endregion
                #region Mp5
                case "2367":
                case "2865":
                case "3306": stats.Mp5 += 2; break;
                case "2370":
                case "2854": stats.Mp5 += 3; break;
                case "2371": stats.Mp5 += 4; break;
                #endregion
                #region Hit Rating
                case "2873":
                case "2908": stats.HitRating += 4; break;
                case "3351": stats.HitRating += 6; break;
                case "2767":
                case "2844": stats.HitRating += 8; break;
                #endregion
                #region Crit Rating
                case "2887":
                case "3204": stats.CritRating += 3; break;
                case "2864":
                case "2874":
                case "2951":
                case "2952":
                case "3263": stats.CritRating += 4; break;
                case "3301":
                case "3316": stats.CritRating += 6; break;
                case "2771":
                case "2787":
                case "2843":
                case "3314": stats.CritRating += 8; break;
                #endregion
                #region Spirit
                case "2890": stats.Spirit += 4; break;
                case "3311": stats.Spirit += 6; break;
                case "2842":
                case "3352": stats.Spirit += 8; break;
                #endregion
                #region Intellect
                case "2869": stats.Intellect += 4; break;
                case "3310": stats.Intellect += 6; break;
                case "3353": stats.Intellect += 8; break;
                #endregion
                #region Dodge Rating
                case "2871": stats.DodgeRating += 4; break;
                case "3358": stats.DodgeRating += 6; break;
                case "3304": stats.DodgeRating += 8; break;
                #endregion
                #region Agility
                case "3149": stats.Agility += 2; break;
                case "2877": stats.Agility += 4; break;
                case "3355": stats.Agility += 6; break;
                case "3313": stats.Agility += 8; break;
                #endregion
                #region Resilience
                case "2878": stats.Resilience += 4; break;
                case "3600": stats.Resilience += 6; break;
                case "3821": stats.Resilience += 8; break;
                #endregion
                #region Strength
                case "2892": stats.Strength += 4; break;
                case "2927": stats.Strength += 4; break;
                case "3312": stats.Strength += 8; break;
                case "3357": stats.Strength += 6; break;
                #endregion
                #region Block Rating
                case "2972": stats.BlockRating += 4; break;
                case "3361": stats.BlockRating += 6; break;
                #endregion
                #region Block Value
                case "2888": stats.BlockValue += 6; break;
                case "3363": stats.BlockValue += 9; break;
                #endregion
                #region Defense Rating
                case "2932": stats.DefenseRating += 4; break;
                case "3751":
                case "3857": stats.DefenseRating += 6; break;
                case "3302": stats.DefenseRating += 8; break;
                #endregion
                #region Haste Rating
                case "3267":
                case "3308": stats.HasteRating += 4; break;
                case "3309": stats.HasteRating += 6; break;
                case "2963":
                case "3303": stats.HasteRating += 8; break;
                #endregion
                #region Expertise Rating
                case "3094": stats.ExpertiseRating += 4; break;
                case "3362": stats.ExpertiseRating += 6; break;
                case "3778": stats.ExpertiseRating += 8; break;
                #endregion
                #region Parry Rating
                case "3359": stats.ParryRating += 4; break;
                case "3871": stats.ParryRating += 6; break;
                case "3360": stats.ParryRating += 8; break;
                #endregion
                #region ArP Rating
                case "3765":
                case "3880": stats.ArmorPenetrationRating += 4; break;
                case "3882": stats.ArmorPenetrationRating += 8; break;
                #endregion
                default:
                    if (!_unhandledSocketBonus.Contains(socketbonus))
                        _unhandledSocketBonus.Add(socketbonus);
                    break;
            }
            return stats;
        }

        private static ItemType GetItemType(string classSubclass)
        {
            switch (classSubclass)
            {
                case "4.1": return ItemType.Cloth;
                case "4.2": return ItemType.Leather;
                case "4.3": return ItemType.Mail;
                case "4.4": return ItemType.Plate;
                case "2.15": return ItemType.Dagger;
                case "2.13": return ItemType.FistWeapon;
                case "2.1": return ItemType.TwoHandAxe;
                case "2.0": return ItemType.OneHandAxe;
                case "2.5": return ItemType.TwoHandMace;
                case "2.4": return ItemType.OneHandMace;
                case "2.8": return ItemType.TwoHandSword;
                case "2.7": return ItemType.OneHandSword;
                case "2.6": return ItemType.Polearm;
                case "2.10": return ItemType.Staff;
                case "4.6": return ItemType.Shield;
                case "2.2": return ItemType.Bow;
                case "2.18": return ItemType.Crossbow;
                case "2.3": return ItemType.Gun;
                case "2.19": return ItemType.Wand;
                case "2.16": return ItemType.Thrown;
                case "4.8": return ItemType.Idol;
                case "4.7": return ItemType.Libram;
                case "4.9": return ItemType.Totem;
                case "6.2": return ItemType.Arrow;
                case "6.3": return ItemType.Bullet;
                case "11.2": return ItemType.Quiver;
                case "11.3": return ItemType.AmmoPouch;
                case "4.10": return ItemType.Sigil;
                default: return ItemType.None;
            }
        }

        private static ItemSlot GetItemSlot(int slotId)
        {
            switch (slotId)
            {
                case  1: return ItemSlot.Head;
                case  2: return ItemSlot.Neck;
                case  3: return ItemSlot.Shoulders;
                case 16: return ItemSlot.Back;
                case  5: case 20: return ItemSlot.Chest;
                case  4: return ItemSlot.Shirt;
                case 19: return ItemSlot.Tabard;
                case  9: return ItemSlot.Wrist;
                case 10: return ItemSlot.Hands;
                case  6: return ItemSlot.Waist;
                case  7: return ItemSlot.Legs;
                case  8: return ItemSlot.Feet;
                case 11: return ItemSlot.Finger;
                case 12: return ItemSlot.Trinket;
                case 13: return ItemSlot.OneHand;
                case 17: return ItemSlot.TwoHand;
                case 21: return ItemSlot.MainHand;
                case 14: case 22: case 23: return ItemSlot.OffHand;
                case 15: case 25: case 26: case 28: return ItemSlot.Ranged;
                case 24: return ItemSlot.Projectile;
                case 18: case 27: return ItemSlot.ProjectileBag;
                default: return ItemSlot.None;
            }
        }

        private static string[] GetItemFactionVendorInfo(string repReqdId, string repReqdLevel)
        {
            string[] retVal = new string[] { "Unknown Faction", "Unknown Vendor", "Unknown Zone", "Unknown Level" };

            switch (repReqdId)
            {
                case "1037": retVal[0] = "Alliance Vanguard"; retVal[1] = ""; retVal[2] = ""; break;
                case "1106": retVal[0] = "Argent Crusade"; retVal[1] = ""; retVal[2] = ""; break;
                case "529": retVal[0] = "Argent Dawn"; retVal[1] = ""; retVal[2] = ""; break;
                case "1012": retVal[0] = "Ashtongue Deathsworn"; retVal[1] = ""; retVal[2] = ""; break;
                case "87": retVal[0] = "Bloodsail Buccaneers"; retVal[1] = ""; retVal[2] = ""; break;
                case "21": retVal[0] = "Booty Bay"; retVal[1] = ""; retVal[2] = ""; break;
                case "910": retVal[0] = "Brood of Nozdormu"; retVal[1] = ""; retVal[2] = ""; break;
                case "609": retVal[0] = "Cenarion Circle"; retVal[1] = ""; retVal[2] = ""; break;
                case "942": retVal[0] = "Cenarion Expedition"; retVal[1] = ""; retVal[2] = ""; break;
                case "909": retVal[0] = "Darkmoon Faire"; retVal[1] = ""; retVal[2] = ""; break;
                case "530": retVal[0] = "Darkspear Trolls"; retVal[1] = ""; retVal[2] = ""; break;
                case "69": retVal[0] = "Darnassus"; retVal[1] = ""; retVal[2] = ""; break;
                case "577": retVal[0] = "Everlook"; retVal[1] = ""; retVal[2] = ""; break;
                case "930": retVal[0] = "Exodar"; retVal[1] = ""; retVal[2] = ""; break;
                case "1068": retVal[0] = "Explorers' League"; retVal[1] = ""; retVal[2] = ""; break;
                case "1104": retVal[0] = "Frenzyheart Tribe"; retVal[1] = ""; retVal[2] = ""; break;
                case "729": retVal[0] = "Frostwolf Clan"; retVal[1] = ""; retVal[2] = ""; break;
                case "369": retVal[0] = "Gadgetzan"; retVal[1] = ""; retVal[2] = ""; break;
                case "92": retVal[0] = "Gelkis Clan Centaur"; retVal[1] = ""; retVal[2] = ""; break;
                case "54": retVal[0] = "Gnomeregan Exiles"; retVal[1] = ""; retVal[2] = ""; break;
                case "946": retVal[0] = "Honor Hold"; retVal[1] = ""; retVal[2] = ""; break;
                case "1052": retVal[0] = "Horde Expedition"; retVal[1] = ""; retVal[2] = ""; break;
                case "749": retVal[0] = "Hydraxian Waterlords"; retVal[1] = ""; retVal[2] = ""; break;
                case "47": retVal[0] = "Ironforge"; retVal[1] = ""; retVal[2] = ""; break;
                case "989": retVal[0] = "Keepers of Time"; retVal[1] = ""; retVal[2] = ""; break;
                case "1090": retVal[0] = "Kirin Tor"; retVal[1] = ""; retVal[2] = ""; break;
                case "1098": retVal[0] = "Knights of the Ebon Blade"; retVal[1] = ""; retVal[2] = ""; break;
                case "978": retVal[0] = "Kurenai"; retVal[1] = ""; retVal[2] = ""; break;
                case "1011": retVal[0] = "Lower City"; retVal[1] = ""; retVal[2] = ""; break;
                case "93": retVal[0] = "Magram Clan Centaur"; retVal[1] = ""; retVal[2] = ""; break;
                case "1015": retVal[0] = "Netherwing"; retVal[1] = ""; retVal[2] = ""; break;
                case "1038": retVal[0] = "Ogri'la"; retVal[1] = ""; retVal[2] = ""; break;
                case "76": retVal[0] = "Orgrimmar"; retVal[1] = ""; retVal[2] = ""; break;
                case "470": retVal[0] = "Ratchet"; retVal[1] = ""; retVal[2] = ""; break;
                case "349": retVal[0] = "Ravenholdt"; retVal[1] = ""; retVal[2] = ""; break;
                case "1031": retVal[0] = "Sha'tari Skyguard"; retVal[1] = ""; retVal[2] = ""; break;
                case "1077": retVal[0] = "Shattered Sun Offensive"; retVal[1] = ""; retVal[2] = ""; break;
                case "809": retVal[0] = "Shen'dralar"; retVal[1] = ""; retVal[2] = ""; break;
                case "911": retVal[0] = "Silvermoon City"; retVal[1] = ""; retVal[2] = ""; break;
                case "890": retVal[0] = "Silverwing Sentinels"; retVal[1] = ""; retVal[2] = ""; break;
                case "970": retVal[0] = "Sporeggar"; retVal[1] = ""; retVal[2] = ""; break;
                case "730": retVal[0] = "Stormpike Guard"; retVal[1] = ""; retVal[2] = ""; break;
                case "72": retVal[0] = "Stormwind"; retVal[1] = ""; retVal[2] = ""; break;
                case "70": retVal[0] = "Syndicate"; retVal[1] = ""; retVal[2] = ""; break;
                case "932": retVal[0] = "The Aldor"; retVal[1] = ""; retVal[2] = ""; break;
                case "1156": retVal[0] = "The Ashen Verdict"; retVal[1] = ""; retVal[2] = ""; break;
                case "933": retVal[0] = "The Consortium"; retVal[1] = ""; retVal[2] = ""; break;
                case "510": retVal[0] = "The Defilers"; retVal[1] = ""; retVal[2] = ""; break;
                case "1126": retVal[0] = "The Frostborn"; retVal[1] = ""; retVal[2] = ""; break;
                case "1067": retVal[0] = "The Hand of Vengeance"; retVal[1] = ""; retVal[2] = ""; break;
                case "1073": retVal[0] = "The Kalu'ak"; retVal[1] = ""; retVal[2] = ""; break;
                case "509": retVal[0] = "The League of Arathor"; retVal[1] = ""; retVal[2] = ""; break;
                case "941": retVal[0] = "The Mag'har"; retVal[1] = ""; retVal[2] = ""; break;
                case "1105": retVal[0] = "The Oracles"; retVal[1] = ""; retVal[2] = ""; break;
                case "990": retVal[0] = "The Scale of the Sands"; retVal[1] = ""; retVal[2] = ""; break;
                case "934": retVal[0] = "The Scryers"; retVal[1] = ""; retVal[2] = ""; break;
                case "935": retVal[0] = "The Sha'tar"; retVal[1] = ""; retVal[2] = ""; break;
                case "1094": retVal[0] = "The Silver Covenant"; retVal[1] = ""; retVal[2] = ""; break;
                case "1119": retVal[0] = "The Sons of Hodir"; retVal[1] = ""; retVal[2] = ""; break;
                case "1124": retVal[0] = "The Sunreavers"; retVal[1] = ""; retVal[2] = ""; break;
                case "1064": retVal[0] = "The Taunka"; retVal[1] = ""; retVal[2] = ""; break;
                case "967": retVal[0] = "The Violet Eye"; retVal[1] = ""; retVal[2] = ""; break;
                case "1091": retVal[0] = "The Wyrmrest Accord"; retVal[1] = ""; retVal[2] = ""; break;
                case "59": retVal[0] = "Thorium Brotherhood"; retVal[1] = ""; retVal[2] = ""; break;
                case "947": retVal[0] = "Thrallmar"; retVal[1] = ""; retVal[2] = ""; break;
                case "81": retVal[0] = "Thunder Bluff"; retVal[1] = ""; retVal[2] = ""; break;
                case "576": retVal[0] = "Timbermaw Hold"; retVal[1] = ""; retVal[2] = ""; break;
                case "922": retVal[0] = "Tranquillien"; retVal[1] = ""; retVal[2] = ""; break;
                case "68": retVal[0] = "Undercity"; retVal[1] = ""; retVal[2] = ""; break;
                case "1050": retVal[0] = "Valiance Expedition"; retVal[1] = ""; retVal[2] = ""; break;
                case "1085": retVal[0] = "Warsong Offensive"; retVal[1] = ""; retVal[2] = ""; break;
                case "889": retVal[0] = "Warsong Outriders"; retVal[1] = ""; retVal[2] = ""; break;
                case "589": retVal[0] = "Wintersaber Trainers"; retVal[1] = ""; retVal[2] = ""; break;
                case "270": retVal[0] = "Zandalar Tribe"; retVal[1] = ""; retVal[2] = ""; break;
                default: break;
            }
            switch (repReqdLevel)
            {
                case "4": retVal[3] = "Friendly"; break;
                case "5": retVal[3] = "Honored"; break;
                case "6": retVal[3] = "Revered"; break;
                case "7": retVal[3] = "Exalted"; break;
                default: break;
            }

            if (retVal[0] == "") { retVal[0] = "Unknown Faction"; }
            if (retVal[1] == "") { retVal[1] = "Unknown Vendor"; }
            if (retVal[2] == "") { retVal[2] = "Unknown Zone"; }
            if (retVal[3] == "") { retVal[3] = "0"; }

            return retVal;
        }

        public delegate bool UpgradeCancelCheck();
        /*
        public static void LoadUpgradesFromWowhead(Character character, CharacterSlot slot, bool usePTR, UpgradeCancelCheck cancel)
        {
            if (!string.IsNullOrEmpty(character.Name))
            {
                //WebRequestWrapper.ResetFatalErrorIndicator();
                List<ComparisonCalculationBase> gemCalculations = new List<ComparisonCalculationBase>();
                foreach (Item item in ItemCache.AllItems)
                {
                    if (item.Slot == ItemSlot.Blue || item.Slot == ItemSlot.Green || item.Slot == ItemSlot.Meta
                         || item.Slot == ItemSlot.Orange || item.Slot == ItemSlot.Prismatic || item.Slot == ItemSlot.Purple
                         || item.Slot == ItemSlot.Red || item.Slot == ItemSlot.Yellow)
                    {
                        gemCalculations.Add(Calculations.GetItemCalculations(item, character, item.Slot == ItemSlot.Meta ? CharacterSlot.Metas : CharacterSlot.Gems));
                    }
                }

                ComparisonCalculationBase idealRed = null, idealBlue = null, idealYellow = null, idealMeta = null;
                foreach (ComparisonCalculationBase calc in gemCalculations)
                {
                    if (Item.GemMatchesSlot(calc.Item, ItemSlot.Meta) && (idealMeta == null || idealMeta.OverallPoints < calc.OverallPoints))
                        idealMeta = calc;
                    if (Item.GemMatchesSlot(calc.Item, ItemSlot.Red) && (idealRed == null || idealRed.OverallPoints < calc.OverallPoints))
                        idealRed = calc;
                    if (Item.GemMatchesSlot(calc.Item, ItemSlot.Blue) && (idealBlue == null || idealBlue.OverallPoints < calc.OverallPoints))
                        idealBlue = calc;
                    if (Item.GemMatchesSlot(calc.Item, ItemSlot.Yellow) && (idealYellow == null || idealYellow.OverallPoints < calc.OverallPoints))
                        idealYellow = calc;
                }
                Dictionary<ItemSlot, int> idealGems = new Dictionary<ItemSlot, int>();
                idealGems.Add(ItemSlot.Meta, idealMeta == null ? 0 : idealMeta.Item.Id);
                idealGems.Add(ItemSlot.Red, idealRed == null ? 0 : idealRed.Item.Id);
                idealGems.Add(ItemSlot.Blue, idealBlue == null ? 0 : idealBlue.Item.Id);
                idealGems.Add(ItemSlot.Yellow, idealYellow == null ? 0 : idealYellow.Item.Id);
                idealGems.Add(ItemSlot.None, 0);

                #region status queuing

                if (slot != CharacterSlot.None)
                {
                    StatusMessaging.UpdateStatus(slot.ToString(), "Queued");
                }
                else
                {
                    StatusMessaging.UpdateStatus(CharacterSlot.Head.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Neck.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Shoulders.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Back.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Chest.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Wrist.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Hands.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Waist.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Legs.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Feet.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Finger1.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Finger2.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Trinket1.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Trinket2.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.MainHand.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.OffHand.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Ranged.ToString(), "Queued");
                }

                #endregion

                if (slot != CharacterSlot.None)
                {
                    LoadUpgradesForSlot(character, slot, idealGems, usePTR, cancel);
                }
                else
                {
                    LoadUpgradesForSlot(character, CharacterSlot.Head, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Neck, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Shoulders, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Back, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Chest, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Wrist, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Hands, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Waist, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Legs, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Feet, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Finger1, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Finger2, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Trinket1, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Trinket2, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.MainHand, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.OffHand, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Ranged, idealGems, usePTR, cancel);
                }
            }
            else
            {
                Base.ErrorBox eb = new Base.ErrorBox("", "You need to have a character loaded for Rawr to find Wowhead upgrades.");
                eb.Show();
            }
        }

        public static void ImportItemsFromWowhead(string filter) { ImportItemsFromWowhead(filter, false); }
        public static void ImportItemsFromWowhead(string filter, bool usePTR)
        {
            //WebRequestWrapper.ResetFatalErrorIndicator();

            string docUpgradeSearch = null;
            try
            {
                string site = usePTR ? "ptr" : "www";
                StatusMessaging.UpdateStatus("ImportWowheadFilter", "Downloading Item List");
                WebRequestWrapper wrw = new WebRequestWrapper();
                docUpgradeSearch = wrw.DownloadUpgradesWowhead(site, filter);
                if (docUpgradeSearch != null)
                {
                    // at this stage have an HTML doc that has upgrades in a <div class="listview-void"> block
                    // need to get the itemID list out and then load them and add to cache
                    int startpos = docUpgradeSearch.IndexOf("<div class=\"listview-void\">");
                    if (startpos > 1)
                    {
                        int endpos = docUpgradeSearch.IndexOf("</div>", startpos);
                        XDocument doc = new XDocument();
                        doc.InnerXml = docUpgradeSearch.Substring(startpos, endpos - startpos + 6);
                        List<XElement> nodeList = new List<XElement>(doc.SelectNodes("//a/@href"));

                        Regex toMatch = new Regex("(\\d{5})");
                        Match match;

                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            StatusMessaging.UpdateStatus("ImportWowheadFilter",
                                string.Format("Downloading definition {0} of {1} items", i, nodeList.Count));
                            //string id = nodeList[i].Value.Substring(7);
                            // This new code will let it find the item id without worrying
                            // about wowhead messing with the specifics of the string
                            match = toMatch.Match(nodeList[i].Value);
                            string id = match.Value;
                            {
                                Item item = GetItem(site, id, true);
                                if (item != null)
                                {
                                    ItemCache.AddItem(item, false);
                                }
                            }
                        }
                    }
                }
                else
                {
                    StatusMessaging.ReportError("ImportWowheadFilter", null, "No response returned from Wowhead");
                }
            }
            catch (Exception ex)
            {
                StatusMessaging.ReportError("ImportWowheadFilter", ex, "Error interpreting the data returned from Wowhead");
            }
        }

        private static void LoadUpgradesForSlot(Character character, CharacterSlot slot, Dictionary<ItemSlot, int> idealGems, bool usePTR, UpgradeCancelCheck cancel)
        {
            if (cancel != null && cancel())
                return;

            string docUpgradeSearch = null;
            try
            {
                string site = usePTR ? "ptr" : "www";
                StatusMessaging.UpdateStatus(slot.ToString(), "Downloading Upgrade List");
                ItemInstance itemToUpgrade = character[slot];
                if ((object)itemToUpgrade != null)
                {
                    WebRequestWrapper wrw = new WebRequestWrapper();
                    string minLevel = "minle=" + itemToUpgrade.Item.ItemLevel.ToString() + ";";
                    string filter = getWowheadSlotFilter(slot) + minLevel + getWowheadClassFilter(character.Class) +
                                    getWowheadWeightFilter(character);
                    docUpgradeSearch = wrw.DownloadUpgradesWowhead(site, filter);
                    ComparisonCalculationBase currentCalculation = Calculations.GetItemCalculations(itemToUpgrade, character, slot);
                    if (docUpgradeSearch != null)
                    {
                        // at this stage have an HTML doc that has upgrades in a <div class="listview-void"> block
                        // need to get the itemID list out and then load them and add to cache if better than itemToUpgrade
                        int startpos = docUpgradeSearch.IndexOf("<div class=\"listview-void\">");
                        if (startpos > 1)
                        {
                            int endpos = docUpgradeSearch.IndexOf("</div>", startpos);
                            XDocument doc = new XDocument();
                            doc.InnerXml = docUpgradeSearch.Substring(startpos, endpos - startpos + 6);
                            List<XElement> nodeList = new List<XElement>(doc.SelectNodes("//a/@href"));

                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                if (cancel != null && cancel())
                                    break;

                                StatusMessaging.UpdateStatus(slot.ToString(), string.Format("Downloading definition {0} of {1} possible upgrades", i, nodeList.Count));
                                string id = nodeList[i].Value.Substring(7);
                                if (!ItemCache.Instance.ContainsItemId(int.Parse(id)))
                                {
                                    Item idealItem = GetItem(site, id, true);
                                    if (idealItem != null)
                                    {
                                        ItemInstance idealGemmedItem = new ItemInstance(int.Parse(id), idealGems[idealItem.SocketColor1], idealGems[idealItem.SocketColor2], idealGems[idealItem.SocketColor3], itemToUpgrade.EnchantId);

                                        Item newItem = ItemCache.AddItem(idealItem, false);

                                        //This is calling OnItemsChanged and ItemCache.Add further down the call stack so if we add it to the cache first, 
                                        // then do the compare and remove it if we don't want it, we can avoid that constant event trigger
                                        ComparisonCalculationBase upgradeCalculation = Calculations.GetItemCalculations(idealGemmedItem, character, slot);

                                        if (upgradeCalculation.OverallPoints < (currentCalculation.OverallPoints * .8f))
                                            ItemCache.DeleteItem(newItem, false);
                                    }
                                }
                            }
                        }
                    } else {
                        StatusMessaging.ReportError(slot.ToString(), null, "No response returned from Wowhead");
                    }
                }
                StatusMessaging.UpdateStatusFinished(slot.ToString());
            } catch (Exception ex) {
                StatusMessaging.ReportError(slot.ToString(), ex, "Error interpreting the data returned from Wowhead");
            }
        }
        */
        public static String GetWowheadWeightedReportURL(Character character)
        {
            return "http://www.wowhead.com/?items&filter=minrl=" + character.Level + ";" + getWowheadClassFilter(character.Class) + getWowheadWeightFilter(character);
        }

        private static string getWowheadClassFilter(CharacterClass className)
        {
            switch (className)
            {
                case CharacterClass.DeathKnight:
                    return "ub=6;";
                case CharacterClass.Druid:
                    return "ub=11;";
                case CharacterClass.Hunter:
                    return "ub=3;";
                case CharacterClass.Mage:
                    return "ub=8;";
                case CharacterClass.Paladin:
                    return "ub=2;";
                case CharacterClass.Priest:
                    return "ub=5;";
                case CharacterClass.Rogue:
                    return "ub=4;";
                case CharacterClass.Shaman:
                    return "ub=7;";
                case CharacterClass.Warlock:
                    return "ub=9;";
                case CharacterClass.Warrior:
                    return "ub=1;";
            }
            return string.Empty;
        }

        private static string getWowheadSlotFilter(CharacterSlot slot)
        {
            switch (slot)
            {
                case CharacterSlot.Back:
                    return "sl=16;";
                case CharacterSlot.Chest:
                    return "sl=5;";
                case CharacterSlot.Feet:
                    return "sl=8;";
                case CharacterSlot.Finger1:
                case CharacterSlot.Finger2:
                    return "sl=11;";
                case CharacterSlot.Hands:
                    return "sl=10;";
                case CharacterSlot.Head:
                    return "sl=1;";
                case CharacterSlot.Legs:
                    return "sl=7;";
                case CharacterSlot.MainHand:
                    return "sl=21:13:17;";
                case CharacterSlot.Neck:
                    return "sl=2;";
                case CharacterSlot.OffHand:
                    return "sl=13:14:22:23;";
                case CharacterSlot.Ranged:
                    return "sl=15:28:25;";
                case CharacterSlot.Shoulders:
                    return "sl=3;";
                case CharacterSlot.Trinket1:
                case CharacterSlot.Trinket2:
                    return "sl=12;";
                case CharacterSlot.Waist:
                    return "sl=6;";
                case CharacterSlot.Wrist:
                    return "sl=9;";
            }
            return string.Empty;
        }

        private static string getWowheadWeightFilter(Character character)
        {
            StringBuilder wt = new StringBuilder("wt=");
            StringBuilder wtv = new StringBuilder(";wtv=");
            if (character.CurrentModel == "Enhance") // force weapon dps ep value 6 to fix caster weapon display issue
            {
                wt.Append("134:");
                wtv.Append("6:");
            }
            ComparisonCalculationBase[] statValues = CalculationsBase.GetRelativeStatValues(character);
            Array.Sort(statValues, StatValueSorter);
            foreach (ComparisonCalculationBase ccb in statValues)
            {
                string stat = getWowHeadStatID(ccb.Name);
                if (!stat.Equals(string.Empty))
                {
                    wt.Append(stat);
                    wtv.Append(ccb.OverallPoints.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
                    wt.Append(":");
                    wtv.Append(":");
                }
            }
            if (wt.Equals("wt="))
                return string.Empty;
            else
                return wt.ToString().Substring(0, wt.Length - 1) + wtv.ToString().Substring(0, wtv.Length - 1);
        }

        private static string getWowHeadStatID(string Name)
        {
            switch (Name)
            {
                case " Strength": return "20";
                case " Agility": return "21";
                case " Stamina": return "22";
                case " Intellect": return "23";
                case " Spirit": return "24";

                case " Health": return "115";
                case " Mana": return "116";
                case " Health per 5 sec": return "60";
                case " Mana per 5 sec": return "61";

                case " Armor": return "41";
                case " Defense Rating": return "42";
                case " Block Value": return "43";
                case " Block Rating": return "44";
                case " Dodge Rating": return "45";
                case " Parry Rating": return "46";
                case " Bonus Armor": return "109";
                case " Resilience": return "79";

                case " Attack Power": return "77";
                case " Spell Power": return "123";
                case " Expertise Rating": return "117";
                case " Hit Rating": return "119";
                case " Crit Rating": return "96";
                case " Haste Rating": return "103";
                case " Melee Crit": return "84";

                case " Feral Attack Power": return "97";
                case " Spell Crit Rating": return "49";
                case " Spell Arcane Damage": return "52";
                case " Spell Fire Damage": return "53";
                case " Spell Nature Damage": return "56";
                case " Spell Shadow Damage": return "57";
                case " Armor Penetration Rating": return "114";
            }
            return string.Empty;
        }

        private static int StatValueSorter(ComparisonCalculationBase x, ComparisonCalculationBase y)
        {
            if (x.OverallPoints > y.OverallPoints)
                return -1;
            else if (x.OverallPoints < y.OverallPoints)
                return 1;
            else
                return 0;
        }
    }

    /*public class ItemIdRequest
    {
        private readonly string itemName;
        public string ItemName { get { return itemName; } }

        private readonly Action<int> callback;
        public Action<int> Callback { get { return callback; } }

        public XDocument ItemSearch { get; set; }

        public int Result { get; set; }

        public void Invoke()
        {
            callback(Result);
        }

        public ItemIdRequest(string itemName, Action<int> callback)
        {
            this.itemName = itemName;
            this.callback = callback;
            GetItemId();
        }
        public void GetItemId()
        {
            new NetworkUtils(new EventHandler(ItemSearchReady)).DownloadItemSearch(ItemName);
        }

        private void ItemSearchReady(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;
            ItemSearch = network.Result;
            try
            {
                List<XElement> items_nodes = new List<XElement>(ItemSearch.SelectNodes("/wowhead/searchResults/items/item"));
                // we only want a single match, even if its not exact
                if (items_nodes.Count == 1)
                {
                    int id = Int32.Parse(items_nodes[0].Attribute("id").Value);
                    Result = id;
                }
                else
                {
                    // choose an exact match if it exists
                    foreach (XElement node in items_nodes)
                    {
                        if (node.Attribute("name").Value == itemName)
                        {
                            int id = Int32.Parse(items_nodes[0].Attribute("id").Value);
                            Result = id;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessaging.ReportError("Get Item", ex, "Rawr encountered an error getting Item Id from Armory: " + ItemName);
            }
            Invoke();
        }
    }*/

    public class ItemRequest
    {
        private readonly int id;
        public int Id { get { return id; } }

        private readonly Action<Item> callback;
        public Action<Item> Callback { get { return callback; } }

        public XDocument Tooltip { get; set; }
        public XDocument ItemInfo { get; set; }

        public Item Result { get; set; }

        public void Invoke()
        {
            callback(Result);
        }

        public ItemRequest(int id, Action<Item> callback)
        {
            this.id = id;
            this.callback = callback;
            GetItem();
        }
        public void GetItem()
        {
            new NetworkUtils(new EventHandler(ItemTooltipReady)).DownloadItemToolTipSheet(Id);
        }

        private void ItemTooltipReady(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;

            Tooltip = network.Result;
            network.DocumentReady -= new EventHandler(ItemTooltipReady);
            network.DocumentReady += new EventHandler(ItemInformationReady);
            network.DownloadItemInformation(Id);
        }

        /// <summary>
        /// This actually parses data, but it's set up for reading Armory right now and not Wowhead data
        /// Need to make it run the JsonParser class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemInformationReady(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;
            ItemInfo = network.Result;
            try
            {
                ItemLocation location = LocationFactory.Create(/*Tooltip,*/ ItemInfo, Id.ToString());

                if (Tooltip == null || Tooltip.SelectSingleNode("/wowhead/itemTooltips/htmlTooltip") == null)
                {
                    StatusMessaging.ReportError("Get Item", null, "No item returned from Wowhead for " + Id);
                    return;
                }

                #region Set Up Variables for Parsing
                ItemQuality quality = ItemQuality.Common;
                ItemType type = ItemType.None;
                ItemSlot socketColor1 = ItemSlot.None;
                ItemSlot socketColor2 = ItemSlot.None;
                ItemSlot socketColor3 = ItemSlot.None;
                Stats socketStats = new Stats();
                string name = string.Empty;
                string iconPath = string.Empty;
                string setName = string.Empty;
                ItemSlot slot = ItemSlot.None;
                Stats stats = new Stats();
                int inventoryType = -1;
                int classId = -1;
                string subclassName = string.Empty;
                int minDamage = 0;
                int maxDamage = 0;
                ItemDamageType damageType = ItemDamageType.Physical;
                float speed = 0f;
                List<string> requiredClasses = new List<string>();
                bool unique = false;
                int itemLevel = 0;
                #endregion

                #region Basic Item Info (Name, Type, etc)
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/name")) { name = node.Value; }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/icon")) { iconPath = node.Value; }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/maxCount")) { unique = node.Value == "1"; }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/overallQualityId")) { quality = (ItemQuality)Enum.Parse(typeof(ItemQuality), node.Value, false); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/classId")) { classId = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/equipData/inventoryType")) { inventoryType = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/equipData/subclassName")) { subclassName = node.Value; }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/min")) { minDamage = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/max")) { maxDamage = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/type")) { damageType = (ItemDamageType)Enum.Parse(typeof(ItemDamageType), node.Value, false); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/speed")) { speed = float.Parse(node.Value, System.Globalization.CultureInfo.InvariantCulture); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/setData/name")) { setName = node.Value; }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/allowableClasses/class")) { requiredClasses.Add(node.Value); }

                foreach (XAttribute attr in ItemInfo.SelectNodes("page/itemInfo/item").Attributes("level")) { itemLevel = int.Parse(attr.Value); }

                if (inventoryType >= 0)
                    slot = GetItemSlot(inventoryType, classId);
                if (!string.IsNullOrEmpty(subclassName))
                    type = GetItemType(subclassName, inventoryType, classId);
                #endregion

                #region Item Class Restriction Fixes
                // fix class restrictions on BOP items that can only be made by certain classes
                switch (Id)
                {
                    case 35181:
                    case 32495:
                        requiredClasses.Add("Priest");
                        break;
                    case 32476:
                    case 35184:
                    case 32475:
                    case 34355:
                        requiredClasses.Add("Shaman");
                        break;
                    case 32474:
                    case 34356:
                        requiredClasses.Add("Hunter");
                        break;
                    case 46106:
                    case 32479:
                    case 32480:
                    case 46109:
                        requiredClasses.Add("Druid");
                        break;
                    case 32478:
                    case 34353:
                        requiredClasses.Add("Druid");
                        requiredClasses.Add("Rogue");
                        break;
                }
                #endregion

                #region Item Stats
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusAgility")) { stats.Agility = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusAttackPower")) { stats.AttackPower = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/armor")) { stats.Armor = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusDefenseSkillRating")) { stats.DefenseRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusDodgeRating")) { stats.DodgeRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusParryRating")) { stats.ParryRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusBlockRating")) { stats.BlockRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusBlockValue")) { stats.BlockValue = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/blockValue")) { stats.BlockValue = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusResilienceRating")) { stats.Resilience = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusStamina")) { stats.Stamina = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusIntellect")) { stats.Intellect = int.Parse(node.Value); }

                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusStrength")) { stats.Strength = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHitRating")) { stats.HitRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteRating")) { stats.HasteRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusCritRating")) { stats.CritRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusExpertiseRating")) { stats.ExpertiseRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusArmorPenetration")) { stats.ArmorPenetrationRating = int.Parse(node.Value); }

                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/arcaneResist")) { stats.ArcaneResistance = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/fireResist")) { stats.FireResistance = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/frostResist")) { stats.FrostResistance = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/natureResist")) { stats.NatureResistance = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/shadowResist")) { stats.ShadowResistance = int.Parse(node.Value); }

                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusCritSpellRating")) { stats.CritRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHitSpellRating")) { stats.HitRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteSpellRating")) { stats.HasteRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusSpellPower")) { stats.SpellPower = int.Parse(node.Value); }

                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusMana")) { stats.Mana = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusSpirit")) { stats.Spirit = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusManaRegen")) { stats.Mp5 = int.Parse(node.Value); }
                #endregion

                #region Item Armor vs Bonus Armor fix
                if (slot == ItemSlot.Finger ||
                    slot == ItemSlot.MainHand ||
                    slot == ItemSlot.Neck ||
                    (slot == ItemSlot.OffHand && type != ItemType.Shield) ||
                    slot == ItemSlot.OneHand ||
                    slot == ItemSlot.Trinket ||
                    slot == ItemSlot.TwoHand)
                {
                    stats.BonusArmor += stats.Armor;
                    stats.Armor = 0f;
                }

                if (slot == ItemSlot.Back)
                {
                    float baseArmor = 0;
                    switch (quality)
                    {
                        case ItemQuality.Temp:
                        case ItemQuality.Poor:
                        case ItemQuality.Common:
                        case ItemQuality.Uncommon:
                            baseArmor = (float)itemLevel * 1.19f + 5.1f;
                            break;

                        case ItemQuality.Rare:
                            baseArmor = ((float)itemLevel + 26.6f) * 16f / 25f;
                            break;

                        case ItemQuality.Epic:
                        case ItemQuality.Legendary:
                        case ItemQuality.Artifact:
                        case ItemQuality.Heirloom:
                            baseArmor = ((float)itemLevel + 358f) * 7f / 26f;
                            break;
                    }

                    baseArmor = (float)Math.Floor(baseArmor);
                    stats.BonusArmor = stats.Armor - baseArmor;
                    stats.Armor = baseArmor;
                }
                #endregion

                #region Item Special Effects
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/spellData/spell"))
                {
                    bool isEquip = false;
                    bool isUse = false;
                    string spellDesc = null;
                    foreach (XElement childNode in node.Elements())
                    {
                        if (childNode.Name == "trigger")
                        {
                            isEquip = childNode.Value == "1";
                            isUse = childNode.Value == "0";
                        }
                        if (childNode.Name == "desc")
                            spellDesc = childNode.Value;
                    }

                    //parse Use/Equip lines
                    if (isUse) SpecialEffects.ProcessUseLine(spellDesc, stats, true, Id);
                    if (isEquip) SpecialEffects.ProcessEquipLine(spellDesc, stats, true, itemLevel, Id);
                }
                #endregion

                #region Item Socket Info and Socket Bonus Stats
                List<XElement> socketNodes = new List<XElement>(Tooltip.SelectNodes("page/itemTooltips/itemTooltip/socketData/socket"));
                if (socketNodes.Count > 0) socketColor1 = (ItemSlot)Enum.Parse(typeof(ItemSlot), socketNodes[0].Attribute("color").Value, false);
                if (socketNodes.Count > 1) socketColor2 = (ItemSlot)Enum.Parse(typeof(ItemSlot), socketNodes[1].Attribute("color").Value, false);
                if (socketNodes.Count > 2) socketColor3 = (ItemSlot)Enum.Parse(typeof(ItemSlot), socketNodes[2].Attribute("color").Value, false);
                string socketBonusesString = string.Empty;
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/socketData/socketMatchEnchant")) { socketBonusesString = node.Value.Trim('+'); }
                if (!string.IsNullOrEmpty(socketBonusesString))
                {
                    try
                    {
                        List<string> socketBonuses = new List<string>();
                        string[] socketBonusStrings = socketBonusesString.Split(new string[] { " and ", " & ", ", " }, StringSplitOptions.None);
                        foreach (string socketBonusString in socketBonusStrings)
                        {
                            if (socketBonusString.LastIndexOf('+') > 2 && socketBonusString.LastIndexOf('+') < socketBonusString.Length - 3)
                            {
                                socketBonuses.Add(socketBonusString.Substring(0, socketBonusString.IndexOf(" +")));
                                socketBonuses.Add(socketBonusString.Substring(socketBonusString.IndexOf(" +") + 1));
                            }
                            else
                                socketBonuses.Add(socketBonusString);
                        }
                        foreach (string socketBonus in socketBonuses)
                        {
                            int socketBonusValue = 0;
                            if (socketBonus.IndexOf(' ') > 0) socketBonusValue = int.Parse(socketBonus.Substring(0, socketBonus.IndexOf(' ')));
                            switch (socketBonus.Substring(socketBonus.IndexOf(' ') + 1))
                            {
                                case "Agility":
                                    socketStats.Agility = socketBonusValue;
                                    break;
                                case "Stamina":
                                    socketStats.Stamina = socketBonusValue;
                                    break;
                                case "Dodge Rating":
                                    socketStats.DodgeRating = socketBonusValue;
                                    break;
                                case "Parry Rating":
                                    socketStats.ParryRating = socketBonusValue;
                                    break;
                                case "Block Rating":
                                    socketStats.BlockRating = socketBonusValue;
                                    break;
                                case "Block Value":
                                    socketStats.BlockValue = socketBonusValue;
                                    break;
                                case "Defense Rating":
                                    socketStats.DefenseRating = socketBonusValue;
                                    break;
                                case "Hit Rating":
                                    socketStats.HitRating = socketBonusValue;
                                    break;
                                case "Haste Rating":
                                    socketStats.HasteRating = socketBonusValue;
                                    break;
                                case "Expertise Rating":
                                    socketStats.ExpertiseRating = socketBonusValue;
                                    break;
                                case "Armor Penetration Rating":
                                    socketStats.ArmorPenetrationRating = socketBonusValue;
                                    break;
                                case "Strength":
                                    socketStats.Strength = socketBonusValue;
                                    break;
                                case "Healing":
                                    //case "Healing +4 Spell Damage":
                                    //case "Healing +3 Spell Damage":
                                    //case "Healing +2 Spell Damage":
                                    //case "Healing +1 Spell Damage":
                                    //case "Healing and +4 Spell Damage":
                                    //case "Healing and +3 Spell Damage":
                                    //case "Healing and +2 Spell Damage":
                                    //case "Healing and +1 Spell Damage":
                                    if (socketBonusValue == 0)
                                        socketStats.SpellPower = (float)Math.Round(int.Parse(socketBonuses[0].Substring(0, socketBonuses[0].IndexOf(' '))) / 1.88f);
                                    else
                                        socketStats.SpellPower = (float)Math.Round(socketBonusValue / 1.88f);
                                    break;
                                case "Spell Damage":
                                    // Only update Spell Damage if its not already set (Incase its an old heal bonus)
                                    if (socketStats.SpellPower == 0)
                                        socketStats.SpellPower = socketBonusValue;
                                    //sockets.Stats.Healing = socketBonusValue;
                                    break;
                                case "Spell Power":
                                    socketStats.SpellPower = socketBonusValue;
                                    break;
                                case "Crit Rating":
                                case "Crit Strike Rating":
                                case "Critical Rating":
                                case "Critical Strike Rating":
                                    socketStats.CritRating = socketBonusValue;
                                    break;
                                case "Attack Power":
                                    socketStats.AttackPower = socketBonusValue;
                                    break;
                                case "Weapon Damage":
                                    socketStats.WeaponDamage = socketBonusValue;
                                    break;
                                case "Resilience":
                                case "Resilience Rating":
                                    socketStats.Resilience = socketBonusValue;
                                    break;
                                //case "Spell Damage and Healing":
                                //    sockets.Stats.SpellDamageRating = socketBonusValue;
                                //    sockets.Stats.Healing = socketBonusValue;
                                //    break;
                                case "Spell Hit Rating":
                                    socketStats.HitRating = socketBonusValue;
                                    break;
                                case "Intellect":
                                    socketStats.Intellect = socketBonusValue;
                                    break;
                                case "Spell Crit":
                                case "Spell Crit Rating":
                                case "Spell Critical":
                                case "Spell Critical Rating":
                                case "Spell Critical Strike Rating":
                                    socketStats.CritRating = socketBonusValue;
                                    break;
                                case "Spell Haste Rating":
                                    socketStats.HasteRating = socketBonusValue;
                                    break;
                                case "Spirit":
                                    socketStats.Spirit = socketBonusValue;
                                    break;
                                case "Mana every 5 seconds":
                                case "Mana ever 5 Sec":
                                case "mana per 5 sec":
                                case "mana per 5 sec.":
                                case "Mana per 5 sec.":
                                case "Mana per 5 Seconds":
                                    socketStats.Mp5 = socketBonusValue;
                                    break;
                            }
                        }
                    }
                    catch { }
                }
                #endregion

                #region Gem Stats
                foreach (XElement nodeGemProperties in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/gemProperties"))
                {
                    List<string> gemBonuses = new List<string>();
                    string[] gemBonusStrings = nodeGemProperties.Value.Split(new string[] { " and ", " & ", ", " }, StringSplitOptions.None);
                    foreach (string gemBonusString in gemBonusStrings)
                    {
                        if (gemBonusString.IndexOf('+') != gemBonusString.LastIndexOf('+'))
                        {
                            gemBonuses.Add(gemBonusString.Substring(0, gemBonusString.IndexOf(" +")));
                            gemBonuses.Add(gemBonusString.Substring(gemBonusString.IndexOf(" +") + 1));
                        }
                        else
                            gemBonuses.Add(gemBonusString);
                    }
                    foreach (string gemBonus in gemBonuses)
                    {
                        if (gemBonus == "Spell Damage +6")
                        {
                            stats.SpellPower = 6.0f;
                        }
                        else if (gemBonus == "2% Increased Armor Value from Items")
                        {
                            stats.BaseArmorMultiplier = 0.02f;
                        }
                        else if (gemBonus == "Stamina +6")
                        {
                            stats.Stamina = 6.0f;
                        }
                        else if (gemBonus == "Chance to restore mana on spellcast")
                        {
                            stats.ManaRestoreOnCast_5_15 = 300; // IED
                        }
                        else if (gemBonus == "Chance on spellcast - next spell cast in half time" || gemBonus == "Chance to Increase Spell Cast Speed")
                        {
                            stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = 320 }, 6, 45, 0.15f));
                        }
                        else if (gemBonus == "+10% Shield Block Value")
                        {
                            stats.BonusBlockValueMultiplier = 0.1f;
                        }
                        else if (gemBonus == "+2% Intellect")
                        {
                            stats.BonusIntellectMultiplier = 0.02f;
                        }
                        else if (gemBonus == "2% Reduced Threat")
                        {
                            stats.ThreatReductionMultiplier = 0.02f;
                        }
                        else if (gemBonus == "3% Increased Critical Healing Effect")
                        {
                            stats.BonusCritHealMultiplier = 0.03f;
                        }
                        else
                        {
                            try
                            {
                                int gemBonusValue = int.Parse(gemBonus.Substring(0, gemBonus.IndexOf(' ')).Trim('+').Trim('%'));
                                switch (gemBonus.Substring(gemBonus.IndexOf(' ') + 1).Trim())
                                {
                                    case "to All Stats":
                                    case "All Stats":
                                        stats.Agility = gemBonusValue;
                                        stats.Strength = gemBonusValue;
                                        stats.Stamina = gemBonusValue;
                                        stats.Intellect = gemBonusValue;
                                        stats.Spirit = gemBonusValue;
                                        break;
                                    case "Resist All":
                                        stats.ArcaneResistance = gemBonusValue;
                                        stats.FireResistance = gemBonusValue;
                                        stats.FrostResistance = gemBonusValue;
                                        stats.NatureResistance = gemBonusValue;
                                        stats.ShadowResistance = gemBonusValue;
                                        break;
                                    case "Increased Critical Damage":
                                        stats.BonusCritMultiplier = (float)gemBonusValue / 100f;
                                        stats.BonusSpellCritMultiplier = (float)gemBonusValue / 100f; // both melee and spell crit use the same text, would have to disambiguate based on other stats
                                        break;
                                    case "Agility":
                                        stats.Agility = gemBonusValue;
                                        break;
                                    case "Stamina":
                                        stats.Stamina = gemBonusValue;
                                        break;
                                    case "Dodge Rating":
                                        stats.DodgeRating = gemBonusValue;
                                        break;
                                    case "Parry Rating":
                                        stats.ParryRating = gemBonusValue;
                                        break;
                                    case "Block Rating":
                                        stats.BlockRating = gemBonusValue;
                                        break;
                                    case "Defense Rating":
                                        stats.DefenseRating = gemBonusValue;
                                        break;
                                    case "Hit Rating":
                                        stats.HitRating = gemBonusValue;
                                        break;
                                    case "Haste Rating":
                                        stats.HasteRating = gemBonusValue;
                                        break;
                                    case "Expertise Rating":
                                        stats.ExpertiseRating = gemBonusValue;
                                        break;
                                    case "Armor Penetration Rating":
                                        stats.ArmorPenetrationRating = gemBonusValue;
                                        break;
                                    case "Strength":
                                        stats.Strength = gemBonusValue;
                                        break;
                                    case "Crit Rating":
                                    case "Crit Strike Rating":
                                    case "Critical Rating":
                                    case "Critical Strike Rating":
                                        stats.CritRating = gemBonusValue;
                                        break;
                                    case "Attack Power":
                                        stats.AttackPower = gemBonusValue;
                                        break;
                                    case "Weapon Damage":
                                        stats.WeaponDamage = gemBonusValue;
                                        break;
                                    case "Resilience":
                                    case "Resilience Rating":
                                        stats.Resilience = gemBonusValue;
                                        break;
                                    case "Spell Hit Rating":
                                        stats.HitRating = gemBonusValue;
                                        break;
                                    case "Spell Haste Rating":
                                        stats.HasteRating = gemBonusValue;
                                        break;
                                    case "Spell Damage":
                                        // Ignore spell damage from gem if Healing has already been applied, as it might be a "9 Healing 3 Spell" gem. 
                                        if (stats.SpellPower == 0)
                                            stats.SpellPower = gemBonusValue;
                                        break;
                                    case "Spell Damage and Healing":
                                        stats.SpellPower = gemBonusValue;
                                        break;
                                    case "Healing":
                                        stats.SpellPower = (float)Math.Round(gemBonusValue / 1.88f);
                                        break;
                                    case "Spell Power":
                                        stats.SpellPower = gemBonusValue;
                                        break;
                                    case "Spell Crit":
                                    case "Spell Crit Rating":
                                    case "Spell Critical":
                                    case "Spell Critical Rating":
                                        stats.CritRating = gemBonusValue;
                                        break;
                                    case "Mana every 5 seconds":
                                    case "Mana ever 5 Sec":
                                    case "mana per 5 sec":
                                    case "mana per 5 sec.":
                                    case "Mana per 5 Seconds":
                                        stats.Mp5 = gemBonusValue;
                                        break;
                                    case "Intellect":
                                        stats.Intellect = gemBonusValue;
                                        break;
                                    case "Spirit":
                                        stats.Spirit = gemBonusValue;
                                        break;
                                }
                            }
                            catch { }
                        }
                    }
                }
                #endregion

                #region Gem Socket Color
                string desc = string.Empty;
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/desc")) { desc = node.Value; }
                if (desc.Contains("Matches any socket"))
                {
                    slot = ItemSlot.Prismatic;
                }
                else if (desc.ToLower().Contains("cogwheel"))
                {
                    slot = ItemSlot.Cogwheel;
                }
                else if (desc.ToLower().Contains("hydraulic"))
                {
                    slot = ItemSlot.Hydraulic;
                }
                else if (desc.Contains("Matches a "))
                {
                    bool red = desc.Contains("Red");
                    bool blue = desc.Contains("Blue");
                    bool yellow = desc.Contains("Yellow");
                    slot = red && blue && yellow ? ItemSlot.Prismatic :
                        red && blue ? ItemSlot.Purple :
                        blue && yellow ? ItemSlot.Green :
                        red && yellow ? ItemSlot.Orange :
                        red ? ItemSlot.Red :
                        blue ? ItemSlot.Blue :
                        yellow ? ItemSlot.Yellow :
                        ItemSlot.None;
                }
                else if (desc.Contains("meta gem slot"))
                    slot = ItemSlot.Meta;
                #endregion

                #region Produce the Item
                Item item = new Item()
                {
                    Id = Id,
                    Name = name,
                    Quality = quality,
                    Type = type,
                    IconPath = iconPath,
                    Slot = slot,
                    SetName = setName,
                    Stats = stats,
                    SocketColor1 = socketColor1,
                    SocketColor2 = socketColor2,
                    SocketColor3 = socketColor3,
                    SocketBonus = socketStats,
                    MinDamage = minDamage,
                    MaxDamage = maxDamage,
                    DamageType = damageType,
                    Speed = speed,
                    RequiredClasses = string.Join("|", requiredClasses.ToArray()),
                    Unique = unique,
                    ItemLevel = itemLevel,
                };
                Result = item;
                #endregion
            } catch (Exception ex) {
                StatusMessaging.ReportError("Get Item", ex,
                    string.Format("Rawr encountered an error getting Item from Wowhead: {0}", Id));
            }
            Invoke();
        }

        private static ItemType GetItemType(string subclassName, int inventoryType, int classId)
        {
            switch (subclassName)
            {
                case "Cloth":       return ItemType.Cloth;
                case "Leather":     return ItemType.Leather;
                case "Mail":        return ItemType.Mail;
                case "Plate":       return ItemType.Plate;
                case "Dagger":      return ItemType.Dagger;
                case "Fist Weapon": return ItemType.FistWeapon;
                case "Axe":         return (inventoryType == 17 ? ItemType.TwoHandAxe   : ItemType.OneHandAxe  );
                case "Mace":        return (inventoryType == 17 ? ItemType.TwoHandMace  : ItemType.OneHandMace );
                case "Sword":       return (inventoryType == 17 ? ItemType.TwoHandSword : ItemType.OneHandSword);
                case "Polearm":     return ItemType.Polearm;
                case "Staff":       return ItemType.Staff;
                case "Shield":      return ItemType.Shield;
                case "Bow":         return ItemType.Bow;
                case "Crossbow":    return ItemType.Crossbow;
                case "Gun":         return ItemType.Gun;
                case "Wand":        return ItemType.Wand;
                case "Thrown":      return ItemType.Thrown;
                case "Idol":        return ItemType.Idol;
                case "Libram":      return ItemType.Libram;
                case "Totem":       return ItemType.Totem;
                case "Arrow":       return ItemType.Arrow;
                case "Bullet":      return ItemType.Bullet;
                case "Quiver":      return ItemType.Quiver;
                case "Ammo Pouch":  return ItemType.AmmoPouch;
                case "Sigil":       return ItemType.Sigil;
                default:            return ItemType.None;
            }
        }

        private static ItemSlot GetItemSlot(int inventoryType, int classId)
        {
            switch (classId) {
                case 6: return ItemSlot.Projectile;
                case 11: return ItemSlot.ProjectileBag;
            }
            switch (inventoryType) {
                case  1: return ItemSlot.Head;
                case  2: return ItemSlot.Neck;
                case  3: return ItemSlot.Shoulders;
                case 16: return ItemSlot.Back;
                case  5: case 20: return ItemSlot.Chest;
                case  4: return ItemSlot.Shirt;
                case 19: return ItemSlot.Tabard;
                case  9: return ItemSlot.Wrist;
                case 10: return ItemSlot.Hands;
                case  6: return ItemSlot.Waist;
                case  7: return ItemSlot.Legs;
                case  8: return ItemSlot.Feet;
                case 11: return ItemSlot.Finger;
                case 12: return ItemSlot.Trinket;
                case 13: return ItemSlot.OneHand;
                case 17: return ItemSlot.TwoHand;
                case 21: return ItemSlot.MainHand;
                case 14: case 22: case 23: return ItemSlot.OffHand;
                case 15: case 25: case 26: case 28: return ItemSlot.Ranged;
                case 24: return ItemSlot.Projectile;
                case 27: return ItemSlot.ProjectileBag;
                default: return ItemSlot.None;
            }
        }
    }
}
