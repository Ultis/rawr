using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Rawr
{
    public class Armory
    {
        private static Armory _instance = new Armory();

        public static Armory Instance
        {
            get { return _instance; }
        }

        private bool _proxyRequiresAuthentication = false;

        private XmlDocument DownloadXml(string url)
        {
            try
                //can we get away...
            {
                //far away...
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                if (_proxyRequiresAuthentication)
                {
                    request.Proxy = HttpWebRequest.DefaultWebProxy;
                    request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                }
                request.UserAgent =
                    "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
                string xml = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                Application.DoEvents();
                return doc;
            }
            catch (Exception ex)
                //say goodnight... to gravity...
            {
                //the passing stars light the way...
                if (!_proxyRequiresAuthentication && ex.Message.Contains("Proxy Authentication Required"))
                {
                    _proxyRequiresAuthentication = true;
                    return DownloadXml(url);
                }
            }
            return null; //lets go for a ride...
        }

        public Character GetCharacter(Character.CharacterRegion region, string realm, string name)
        {
            XmlDocument docCharacter = null;
            try
            {
                Log.Write("Getting Character from Armory: " + name + "@" + region.ToString() + "-" + realm);
                //Tell me how he died.
                string armoryDomain = region == Character.CharacterRegion.US ? "www" : "eu";
                string characterSheetPath = string.Format("http://{0}.wowarmory.com/character-sheet.xml?r={1}&n={2}",
                                                          armoryDomain, realm, name);
                docCharacter = DownloadXml(characterSheetPath);

                Character.CharacterRace race =
                    docCharacter.SelectSingleNode("page/characterInfo/character").Attributes["race"].Value ==
                    "Night Elf"
                        ?
                    Character.CharacterRace.NightElf
                        : Character.CharacterRace.Tauren;
                Dictionary<Character.CharacterSlot, string> items = new Dictionary<Character.CharacterSlot, string>();
                Dictionary<Character.CharacterSlot, int> enchants = new Dictionary<Character.CharacterSlot, int>();

                foreach (XmlNode itemNode in docCharacter.SelectNodes("page/characterInfo/characterTab/items/item"))
                {
                    int slot = int.Parse(itemNode.Attributes["slot"].Value);
                    items[(Character.CharacterSlot) slot] =
                        string.Format("{0}.{1}.{2}.{3}", itemNode.Attributes["id"].Value,
                                      itemNode.Attributes["gem0Id"].Value, itemNode.Attributes["gem1Id"].Value,
                                      itemNode.Attributes["gem2Id"].Value);
                    enchants[(Character.CharacterSlot) slot] = int.Parse(itemNode.Attributes["permanentenchant"].Value);
                }

                Character character = new Character(name, realm, region, race,
                                                    items.ContainsKey(Character.CharacterSlot.Head)
                                                        ? items[Character.CharacterSlot.Head]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Neck)
                                                        ? items[Character.CharacterSlot.Neck]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Shoulders)
                                                        ? items[Character.CharacterSlot.Shoulders]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Back)
                                                        ? items[Character.CharacterSlot.Back]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Chest)
                                                        ? items[Character.CharacterSlot.Chest]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Shirt)
                                                        ? items[Character.CharacterSlot.Shirt]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Tabard)
                                                        ? items[Character.CharacterSlot.Tabard]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Wrist)
                                                        ? items[Character.CharacterSlot.Wrist]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Hands)
                                                        ? items[Character.CharacterSlot.Hands]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Waist)
                                                        ? items[Character.CharacterSlot.Waist]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Legs)
                                                        ? items[Character.CharacterSlot.Legs]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Feet)
                                                        ? items[Character.CharacterSlot.Feet]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Finger1)
                                                        ? items[Character.CharacterSlot.Finger1]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Finger2)
                                                        ? items[Character.CharacterSlot.Finger2]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Trinket1)
                                                        ? items[Character.CharacterSlot.Trinket1]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Trinket2)
                                                        ? items[Character.CharacterSlot.Trinket2]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Weapon)
                                                        ? items[Character.CharacterSlot.Weapon]
                                                        : null,
                                                    items.ContainsKey(Character.CharacterSlot.Idol)
                                                        ? items[Character.CharacterSlot.Idol]
                                                        : null,
                                                    enchants.ContainsKey(Character.CharacterSlot.Head)
                                                        ? enchants[Character.CharacterSlot.Head]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Shoulders)
                                                        ? enchants[Character.CharacterSlot.Shoulders]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Back)
                                                        ? enchants[Character.CharacterSlot.Back]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Chest)
                                                        ? enchants[Character.CharacterSlot.Chest]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Wrist)
                                                        ? enchants[Character.CharacterSlot.Wrist]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Hands)
                                                        ? enchants[Character.CharacterSlot.Hands]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Legs)
                                                        ? enchants[Character.CharacterSlot.Legs]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Feet)
                                                        ? enchants[Character.CharacterSlot.Feet]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Finger1)
                                                        ? enchants[Character.CharacterSlot.Finger1]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Finger2)
                                                        ? enchants[Character.CharacterSlot.Finger2]
                                                        : 0,
                                                    enchants.ContainsKey(Character.CharacterSlot.Weapon)
                                                        ? enchants[Character.CharacterSlot.Weapon]
                                                        : 0
                    );

                //I will tell you how he lived.
                return character;
            }
            catch (Exception ex)
            {
                if (docCharacter == null || docCharacter.InnerXml.Length == 0)
                {
                    MessageBox.Show(string.Format("Rawr encountered an error getting Character " +
                                                  "from Armory: {0}@{1}-{2}. Please check to make sure you've spelled the character name and realm" +
                                                  " exactly right, and chosen the correct Region. Rawr recieved no response to its query for character" +
                                                  " data, so if the character name/region/realm are correct, please check to make sure that no firewall " +
                                                  "or proxy software is blocking Rawr. If you still encounter this error, please copy and" +
                                                  " paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {3}\r\n\r\n\r\n{4}\r\n\r\n{5}",
                                                  name, region.ToString(), realm, "null", ex.Message, ex.StackTrace));
                }
                else
                {
                    MessageBox.Show(string.Format("Rawr encountered an error getting Character " +
                                                  "from Armory: {0}@{1}-{2}. Please check to make sure you've spelled the character name and realm" +
                                                  " exactly right, and chosen the correct Region. If you still encounter this error, please copy and" +
                                                  " paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {3}\r\n\r\n\r\n{4}\r\n\r\n{5}",
                                                  name, region.ToString(), realm, docCharacter.OuterXml, ex.Message,
                                                  ex.StackTrace));
                }
                return null;
            }
        }

        public Item GetItem(string gemmedId, string logReason)
        {
            //Just close your eyes
            XmlDocument docItem = null;
            try
            {
                int retry = 0;
                while (retry < 3)
                {
                    try
                    {
                        string id = gemmedId.Split('.')[0];
                        Log.Write("Getting Item from Armory: " + id + "   Reason: " + logReason);

                        string itemTooltipPath = string.Format("http://www.wowarmory.com/item-tooltip.xml?i={0}", id);
                        docItem = DownloadXml(itemTooltipPath);

                        Quality quality = Quality.Common;
                        string name = string.Empty;
                        string iconPath = string.Empty;
                        Item.ItemSlot slot = Item.ItemSlot.None;
                        Stats stats = new Stats();
                        Sockets sockets = new Sockets();

                        foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/name"))
                        {
                            name = node.InnerText;
                        }
                        foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/icon"))
                        {
                            iconPath = node.InnerText;
                        }
                        foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/overallQualityId"))
                        {
                            quality = (Quality) Enum.Parse(typeof (Quality), node.InnerText);
                        }
                        foreach (
                            XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/equipData/inventoryType")
                            )
                        {
                            slot = (Item.ItemSlot) int.Parse(node.InnerText);
                        }

                        foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusAgility"))
                        {
                            stats.Agility = int.Parse(node.InnerText);
                        }
                        foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/armor"))
                        {
                            stats.Armor = int.Parse(node.InnerText);
                        }
                        foreach (
                            XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusDefenseSkillRating")
                            )
                        {
                            stats.DefenseRating = int.Parse(node.InnerText);
                        }
                        foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusDodgeRating"))
                        {
                            stats.DodgeRating = int.Parse(node.InnerText);
                        }
                        foreach (
                            XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusResilienceRating"))
                        {
                            stats.Resilience = int.Parse(node.InnerText);
                        }
                        foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/bonusStamina"))
                        {
                            stats.Stamina = int.Parse(node.InnerText);
                        }

                        XmlNodeList socketNodes = docItem.SelectNodes("page/itemTooltips/itemTooltip/socketData/socket");
                        if (socketNodes.Count > 0) sockets.Color1String = socketNodes[0].Attributes["color"].Value;
                        if (socketNodes.Count > 1) sockets.Color2String = socketNodes[1].Attributes["color"].Value;
                        if (socketNodes.Count > 2) sockets.Color3String = socketNodes[2].Attributes["color"].Value;
                        string socketBonus = string.Empty;
                        foreach (
                            XmlNode node in
                                docItem.SelectNodes("page/itemTooltips/itemTooltip/socketData/socketMatchEnchant"))
                        {
                            socketBonus = node.InnerText.Trim('+');
                        }
                        if (!string.IsNullOrEmpty(socketBonus))
                        {
                            try
                            {
                                int socketBonusValue = int.Parse(socketBonus.Substring(0, socketBonus.IndexOf(' ')));
                                switch (socketBonus.Substring(socketBonus.IndexOf(' ') + 1))
                                {
                                    case "Agility":
                                        sockets.Stats.Agility = socketBonusValue;
                                        break;
                                    case "Stamina":
                                        sockets.Stats.Stamina = socketBonusValue;
                                        break;
                                    case "Dodge Rating":
                                        sockets.Stats.DodgeRating = socketBonusValue;
                                        break;
                                    case "Defense Rating":
                                        sockets.Stats.DefenseRating = socketBonusValue;
                                        break;
                                    case "Resilience":
                                    case "Resilience Rating":
                                        sockets.Stats.Resilience = socketBonusValue;
                                        break;
                                }
                            }
                            catch
                            {
                            }
                        }
                        foreach (
                            XmlNode nodeGemProperties in
                                docItem.SelectNodes("page/itemTooltips/itemTooltip/gemProperties"))
                        {
                            string[] gemBonuses =
                                nodeGemProperties.InnerText.Split(new string[] {" and ", " & "}, StringSplitOptions.None);
                            foreach (string gemBonus in gemBonuses)
                            {
                                try
                                {
                                    int gemBonusValue =
                                        int.Parse(gemBonus.Substring(0, gemBonus.IndexOf(' ')).Trim('+'));
                                    switch (gemBonus.Substring(gemBonus.IndexOf(' ') + 1))
                                    {
                                        case "Agility":
                                            stats.Agility = gemBonusValue;
                                            break;
                                        case "Stamina":
                                            stats.Stamina = gemBonusValue;
                                            break;
                                        case "Dodge Rating":
                                            stats.DodgeRating = gemBonusValue;
                                            break;
                                        case "Defense Rating":
                                            stats.DefenseRating = gemBonusValue;
                                            break;
                                        case "Resilience":
                                        case "Resilience Rating":
                                            stats.Resilience = gemBonusValue;
                                            break;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                        string desc = string.Empty;
                        foreach (XmlNode node in docItem.SelectNodes("page/itemTooltips/itemTooltip/desc"))
                        {
                            desc = node.InnerText;
                        }
                        if (desc.Contains("Matches a "))
                        {
                            bool red = desc.Contains("Red");
                            bool blue = desc.Contains("Blue");
                            bool yellow = desc.Contains("Yellow");
                            slot = red && blue && yellow
                                       ? Item.ItemSlot.Prismatic
                                       :
                                   red && blue
                                       ? Item.ItemSlot.Purple
                                       :
                                   blue && yellow
                                       ? Item.ItemSlot.Green
                                       :
                                   red && yellow
                                       ? Item.ItemSlot.Orange
                                       :
                                   red
                                       ? Item.ItemSlot.Red
                                       :
                                   blue
                                       ? Item.ItemSlot.Blue
                                       :
                                   yellow
                                       ? Item.ItemSlot.Yellow
                                       :
                                   Item.ItemSlot.None;
                        }
                        else if (desc.Contains("meta gem slot"))
                            slot = Item.ItemSlot.Meta;

                        string[] ids = gemmedId.Split('.');
                        int gem1Id = ids.Length == 4 ? int.Parse(ids[1]) : 0;
                        int gem2Id = ids.Length == 4 ? int.Parse(ids[2]) : 0;
                        int gem3Id = ids.Length == 4 ? int.Parse(ids[3]) : 0;
                        Item item =
                            new Item(name, quality, int.Parse(id), iconPath, slot, stats, sockets, gem1Id, gem2Id,
                                     gem3Id);
                        return item;
                    }
                    catch
                    {
                        retry++;
                        if (retry == 3) throw;
                    }
                    //And all will be revealed
                }
                return null;
            }
            catch (Exception ex)
            {
                if (docItem == null || docItem.InnerXml.Length == 0)
                {
                    MessageBox.Show(string.Format("Rawr encountered an error getting Item " +
                                                  "from Armory: {0}. Rawr recieved no response to its query for item" +
                                                  " data, so please check to make sure that no firewall " +
                                                  "or proxy software is blocking Rawr. If you still encounter this error, please copy and" +
                                                  " paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
                                                  gemmedId, "null", ex.Message, ex.StackTrace));
                }
                else
                {
                    MessageBox.Show(string.Format("Rawr encountered an error getting Item " +
                                                  "from Armory: {0}. If you still encounter this error, please copy and" +
                                                  " paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
                                                  gemmedId, docItem.OuterXml, ex.Message, ex.StackTrace));
                }
                return null;
            }
        }

        public void LoadUpgradesFromArmory(Character character)
        {
            if (!string.IsNullOrEmpty(character.Realm) && !string.IsNullOrEmpty(character.Name))
            {
                List<ItemBuffEnchantCalculation> gemCalculations = new List<ItemBuffEnchantCalculation>();
                foreach (Item item in ItemCache.GetItemsArray())
                {
                    if (item.Slot == Item.ItemSlot.Blue || item.Slot == Item.ItemSlot.Green ||
                        item.Slot == Item.ItemSlot.Meta
                        || item.Slot == Item.ItemSlot.Orange || item.Slot == Item.ItemSlot.Prismatic ||
                        item.Slot == Item.ItemSlot.Purple
                        || item.Slot == Item.ItemSlot.Red || item.Slot == Item.ItemSlot.Yellow)
                    {
                        gemCalculations.Add(
                            Calculations.GetItemCalculations(item, character,
                                                             item.Slot == Item.ItemSlot.Meta
                                                                 ? Character.CharacterSlot.Metas
                                                                 : Character.CharacterSlot.Gems));
                    }
                }

                ItemBuffEnchantCalculation idealRed = null, idealBlue = null, idealYellow = null, idealMeta = null;
                foreach (ItemBuffEnchantCalculation calc in gemCalculations)
                {
                    if (Item.GemMatchesSlot(calc.Item, Item.ItemSlot.Meta) &&
                        (idealMeta == null || idealMeta.OverallPoints < calc.OverallPoints))
                        idealMeta = calc;
                    if (Item.GemMatchesSlot(calc.Item, Item.ItemSlot.Red) &&
                        (idealRed == null || idealRed.OverallPoints < calc.OverallPoints))
                        idealRed = calc;
                    if (Item.GemMatchesSlot(calc.Item, Item.ItemSlot.Blue) &&
                        (idealBlue == null || idealBlue.OverallPoints < calc.OverallPoints))
                        idealBlue = calc;
                    if (Item.GemMatchesSlot(calc.Item, Item.ItemSlot.Yellow) &&
                        (idealYellow == null || idealYellow.OverallPoints < calc.OverallPoints))
                        idealYellow = calc;
                }
                Dictionary<Item.ItemSlot, int> idealGems = new Dictionary<Item.ItemSlot, int>();
                idealGems.Add(Item.ItemSlot.Meta, idealMeta == null ? 0 : idealMeta.Item.Id);
                idealGems.Add(Item.ItemSlot.Red, idealRed == null ? 0 : idealRed.Item.Id);
                idealGems.Add(Item.ItemSlot.Blue, idealBlue == null ? 0 : idealBlue.Item.Id);
                idealGems.Add(Item.ItemSlot.Yellow, idealYellow == null ? 0 : idealYellow.Item.Id);
                idealGems.Add(Item.ItemSlot.None, 0);

                _character = character;
                _idealGems = idealGems;
                Thread t = new Thread(new ThreadStart(LoadUpgrades));
                t.Start();
            }
            else
            {
                MessageBox.Show(
                    "This feature requires your character name, realm, and region. Please fill these fields out, and try again.");
            }
        }

        private Character _character = new Character();
        private Dictionary<Item.ItemSlot, int> _idealGems;

        public void LoadUpgrades()
        {
            Application.DoEvents();

            LoadUpgradesForSlot(Character.CharacterSlot.Back);
            LoadUpgradesForSlot(Character.CharacterSlot.Chest);
            LoadUpgradesForSlot(Character.CharacterSlot.Feet);
            LoadUpgradesForSlot(Character.CharacterSlot.Finger1);
            LoadUpgradesForSlot(Character.CharacterSlot.Finger2);
            LoadUpgradesForSlot(Character.CharacterSlot.Hands);
            LoadUpgradesForSlot(Character.CharacterSlot.Head);
            LoadUpgradesForSlot(Character.CharacterSlot.Legs);
            LoadUpgradesForSlot(Character.CharacterSlot.Neck);
            LoadUpgradesForSlot(Character.CharacterSlot.Shoulders);
            LoadUpgradesForSlot(Character.CharacterSlot.Trinket1);
            LoadUpgradesForSlot(Character.CharacterSlot.Trinket2);
            LoadUpgradesForSlot(Character.CharacterSlot.Waist);
            LoadUpgradesForSlot(Character.CharacterSlot.Weapon);

            FormMain.Instance.ResetItemSlotText();
            FormMain.Instance.ResetProgressBar();
            FormMain.Instance.ResetStatusText();
        }

        private void LoadUpgradesForSlot(Character.CharacterSlot slot)
        {
            XmlDocument docUpgradeSearch = null;
            try
            {
                FormMain.Instance.SetItemSlotText(slot);
                Item itemToUpgrade = _character[slot];
                if (itemToUpgrade != null)
                {
                    string armoryDomain = _character.Region == Character.CharacterRegion.US ? "www" : "eu";
                    string upgradeSearchPath =
                        string.Format("http://{0}.wowarmory.com/search.xml?searchType=items&pr={1}&pn={2}&pi={3}",
                                      armoryDomain, _character.Realm, _character.Name, itemToUpgrade.Id);

                    FormMain.Instance.SetStatusText("Connecting to " + upgradeSearchPath);
                    Application.DoEvents();
                    
                    docUpgradeSearch = DownloadXml(upgradeSearchPath);

                    ItemBuffEnchantCalculation currentCalculation =
                        Calculations.GetItemCalculations(itemToUpgrade, _character, slot);

                    FormMain.Instance.SetStatusText("Processing nodes...");
                    FormMain.Instance.ResetProgressBar();

                    double i = 0;
                    double count = 0;
                    if (docUpgradeSearch.SelectNodes("page/armorySearch/searchResults/items/item") != null)
                        count = docUpgradeSearch.SelectNodes("page/armorySearch/searchResults/items/item").Count;

                    foreach (XmlNode node in docUpgradeSearch.SelectNodes("page/armorySearch/searchResults/items/item"))
                    {
                        i++;
                        FormMain.Instance.SetProgressBar((i / count) * 100);
                        Application.DoEvents();
                        string id = node.Attributes["id"].Value + ".0.0.0";
                        FormMain.Instance.SetStatusText("Retrieving Item " + id);
                        Application.DoEvents();
                        Item idealItem = GetItem(id, "Loading Upgrades");
                        idealItem._gem1Id = _idealGems[idealItem.Sockets.Color1];
                        idealItem._gem2Id = _idealGems[idealItem.Sockets.Color2];
                        idealItem._gem3Id = _idealGems[idealItem.Sockets.Color3];

                        if (!ItemCache.Items.ContainsKey(idealItem.GemmedId))
                        {
                            FormMain.Instance.SetStatusText("Calculating Item " + idealItem);
                            Application.DoEvents();
                            ItemBuffEnchantCalculation upgradeCalculation =
                                Calculations.GetItemCalculations(idealItem, _character, slot);

                            if (upgradeCalculation.OverallPoints > (currentCalculation.OverallPoints*.8f)) // ||
                                //upgradeCalculation.MitigationPoints > (currentCalculation.MitigationPoints * .8f) ||
                                //upgradeCalculation.SurvivalPoints > (currentCalculation.SurvivalPoints * .8f) )
                            {
                                ItemCache.AddItem(idealItem);
                                FormMain.Instance.SetStatusText("Loading Item " + idealItem);
                                Application.DoEvents();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (docUpgradeSearch == null || docUpgradeSearch.InnerXml.Length == 0)
                {
                    MessageBox.Show(string.Format("Rawr encountered an error getting Upgrades " +
                                                  "from Armory: {0}. Rawr recieved no response to its query for upgrade" +
                                                  " data, so please check to make sure that no firewall " +
                                                  "or proxy software is blocking Rawr. If you still encounter this error, please copy and" +
                                                  " paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
                                                  slot.ToString(), "null", ex.Message, ex.StackTrace));
                }
                else
                {
                    MessageBox.Show(string.Format("Rawr encountered an error getting Upgrades " +
                                                  "from Armory: {0}. If you still encounter this error, please copy and" +
                                                  " paste this into an e-mail to cnervig@hotmail.com. Thanks!\r\n\r\nResponse: {1}\r\n\r\n\r\n{2}\r\n\r\n{3}",
                                                  slot.ToString(), docUpgradeSearch.OuterXml, ex.Message, ex.StackTrace));
                }
            }
        }
    }
}