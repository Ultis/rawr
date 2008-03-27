using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Rawr
{
    public enum ItemSource
    {
        Unknown,
        Vendor,
        Faction,
        PVP,
        Crafted,
        WorldDrop,
        StaticDrop,
        Quest,
        Container,
    }

    public enum ReputationLevel
    {
        Hated,
        Hostile,
        Unfriendly,
        Neutral,
        Friendly,
        Honored,
        Revered,
        Exalted,
    };

    public enum BindsOn
    {
        None,
        BoP,
        BoE,
    };

    delegate ItemLocation Construct();

    [Serializable]
    public class ItemLocation
    {
        public ItemSource Source{get;set;}

        private string _description;
        [XmlIgnore]
        public virtual string Description
        {
            get{return _description;}
        }
        public ItemLocation():this("Unknown location")
        {            
        }
        public ItemLocation(string desc)
        {
            Source = ItemSource.Unknown;
            _description = desc;
        }
        public virtual ItemLocation Fill(XmlNode node, string itemId)
        {
            return this;
        }
        public static ItemLocation Construct()
        {
            return new ItemLocation("");
        }

    }
  
    [Serializable]
    public class VendorItem : ItemLocation
    {
        
        public string Token{get;set;}
        public int Count{get;set;}

        
        [XmlIgnore]
        public string CostString
        {
            get
            {
                int total = Cost;
                int gold = total / 10000;
                total -= gold * 10000;
                int silver = total / 100;
                total -= silver * 100;

                if (gold > 0)
                {
                    return string.Format("{0}g{1}s{2}c", gold, silver, total);
                }
                else if (silver > 0)
                {
                    return string.Format("{1}s{2}c", gold, silver, total);
                }
                else if (total > 0)
                {
                    return string.Format("{2}c", gold, silver, total);
                }
                return "";
            }

        }

        public int Cost{get;set;}


        public VendorItem()
        {
            Source = ItemSource.Vendor;
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return string.Format("Purchasable with {0} [{1}]{2}{3}", Count, Token, Cost>0?" and":"", CostString);
            }
        }


        public override ItemLocation Fill(XmlNode node, string itemId)
        {
            WebRequestWrapper wrw = new WebRequestWrapper();
            XmlDocument doc = wrw.DownloadItemInformation(int.Parse(itemId));

            XmlNode subNode = doc.SelectSingleNode("/page/itemInfo/item/cost/token");
            if(subNode!= null)
            {

                string tokenId = subNode.Attributes["id"].Value;
                int count = int.Parse(subNode.Attributes["count"].Value);

                string Boss = null;
                string Area = null;

                if (!_idToBossMap.ContainsKey(tokenId))
                {
                    doc = wrw.DownloadItemInformation(int.Parse(tokenId));

                    XmlNodeList list = doc.SelectNodes("/page/itemInfo/item/dropCreatures/creature");

                    subNode = list[0];
                    if (list.Count == 1)
                    {
                        Boss = subNode.Attributes["name"].Value;
                        Area = subNode.Attributes["area"].Value;
                        _idToBossMap[tokenId] = Boss;
                        _bossToAreaMap[Boss] = Area;
                    }
                    else
                    {
                        Boss = subNode.SelectSingleNode("/page/itemInfo/item/@name").InnerText;
                        Area = "*";
                    }

                    _idToBossMap[tokenId] = Boss;
                    _bossToAreaMap[Boss] = Area;
                }
                else
                {
                    Boss = _idToBossMap[tokenId];
                    Area = _bossToAreaMap[Boss];
                }
                if (Area != "*")
                {
                    return new StaticDrop()
                    {
                        Area = Area,
                        Boss = Boss,
                        Heroic = false
                    };
                }

                Count = count;
                Token = Boss;

            }
            else
            {
                return new ItemLocation("Vendor");
            }

            return this;
        }
        public static new ItemLocation Construct()
        {
            VendorItem item = new VendorItem();
            return item;
        }

        static SortedList<string, string> _idToBossMap = new SortedList<string, string>();
        static SortedList<string, string> _bossToAreaMap = new SortedList<string, string>();
    }
  
    [Serializable]
    public class FactionItem : ItemLocation
    {

        public string FactionName {get;set;}
        public ReputationLevel Level{get;set;}

        [XmlIgnore]
        public string CostString
        {
            get
            {
                int total = Cost;
                int gold = total / 10000;
                total -= gold * 10000;
                int silver = total / 100;
                total -= silver * 100;

                if (gold > 0)
                {
                    return string.Format("{0}g{1}s{2}c", gold, silver, total);
                }
                else if (silver > 0)
                {
                    return string.Format("{1}s{2}c", gold, silver, total);
                }
                return string.Format("{2}c", gold, silver, total);
            }

        }

        public int Cost{get;set;}


        static SortedList<string, string> tokenIDMap = new SortedList<string, string>();

        private SerializableDictionary<string, int> _tokenMap = new SerializableDictionary<string, int>();
        public SerializableDictionary<string, int> TokenMap
        {
            get
            {
                return _tokenMap;
            }
            set
            {
                _tokenMap = value;
            }
        }
        public FactionItem()
        {
            Source = ItemSource.Faction;
        }

        public override string Description
        {
            get
            {
                if (_tokenMap.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string key in _tokenMap.Keys)
                    {
                        sb.AppendFormat("{0} [{1}] ", _tokenMap[key], key);
                    }

                    if(Cost>0)
                    {
                        return string.Format("Purchasable for {3}{2} and requires {0} - {1}", FactionName, Level.ToString(), CostString, sb.ToString());

                    }
                    else
                    {
                        return string.Format("Purchasable for {2} and requires {0} - {1}", FactionName, Level.ToString(), sb.ToString());

                    }
                }
                else if (Cost > 0)
                {
                    return string.Format("Purchasable for {2} and requires {0} - {1}", FactionName, Level.ToString(), CostString);
                }
                else
                {
                    return string.Format("Purchasable and requires {0} - {1}", FactionName, Level.ToString());
                }

            }
        }

        public override ItemLocation Fill(XmlNode node, string itemId)
        {
            WebRequestWrapper wrw = new WebRequestWrapper();
            XmlDocument doc = wrw.DownloadItemInformation(int.Parse(itemId));

            XmlNode subNode = doc.SelectSingleNode("/page/itemInfo/item/rewardFromQuests/quest[1]");

            if (subNode != null)
            {
                return QuestItem.Construct().Fill(node, itemId);
            }
                
                
                
            subNode = doc.SelectSingleNode("/page/itemInfo/item/cost/@buyPrice");

            if (subNode != null)
            {
                Cost = int.Parse(subNode.InnerText);
            }
            else
            {
                Cost = 0;
            }

            foreach (XmlNode token in doc.SelectNodes("/page/itemInfo/item/cost/token"))
            {
                int Count = int.Parse(token.Attributes["count"].Value);
                string id = token.Attributes["id"].Value;
                if(!tokenIDMap.ContainsKey(id))
                {
                    WebRequestWrapper wrw2 = new WebRequestWrapper();
                    XmlDocument doc2 = wrw.DownloadItemInformation(int.Parse(id));

                    tokenIDMap[id] =doc2.SelectSingleNode("/page/itemInfo/item/@name").InnerText;
                }

                _tokenMap[tokenIDMap[id]] = Count;
            }


            subNode = node.SelectSingleNode("/page/itemTooltips/itemTooltip/requiredFaction");
            if(subNode != null)
            {
                FactionName = subNode.Attributes["name"].Value;
                Level = (ReputationLevel)System.Enum.Parse(typeof(ReputationLevel), subNode.Attributes["rep"].Value);
            }

            return this;
        }
        public static new ItemLocation Construct()
        {
            FactionItem item = new FactionItem();
            return item;
        }
    }
   
    [Serializable]
    public class PvpItem : ItemLocation
    {
        public string PointType{get;set;}
        public int Points{get;set;}
        public string TokenType{get;set;}
        public int TokenCount{get;set;}

        public PvpItem()
        {
            Source = ItemSource.PVP;
        }

        [XmlIgnore]
        public override string Description
        {
            get
            {
                string points = string.Format("Purchasable for {0} {1} Points", Points, PointType);
                if (TokenCount > 0)
                {
                    return string.Format("{0} and {1} [{2}]", points, TokenCount, TokenType);
                }
                return points;
            }
        }

        public override ItemLocation Fill(XmlNode node, string itemId)
        {
            WebRequestWrapper wrw = new WebRequestWrapper();
            XmlDocument doc = wrw.DownloadItemInformation(int.Parse(itemId));

            string TokenId;
            XmlNode subNode = doc.SelectSingleNode("/page/itemInfo/item/cost/@honor");

            if (subNode != null)
            {
                Points = int.Parse(subNode.InnerText);
                PointType = "Honor";
                subNode = doc.SelectSingleNode("/page/itemInfo/item/cost/token/@count");
                if(subNode != null)
                {
                    
                    TokenCount = int.Parse(subNode.InnerText);
                    TokenId = doc.SelectSingleNode("/page/itemInfo/item/cost/token").Attributes["id"].Value;

                    if (_tokenMap.ContainsKey(TokenId))
                    {
                        TokenType = _tokenMap[TokenId];
                    }
                    else
                    {
                        wrw = new WebRequestWrapper();
                        doc = wrw.DownloadItemInformation(int.Parse(TokenId));

                        TokenType = doc.SelectSingleNode("/page/itemInfo/item").Attributes["name"].Value;

                        _tokenMap[TokenId] = TokenType;
                    }
                }
            }
            else
            {
                subNode = doc.SelectSingleNode("/page/itemInfo/item/cost/@arena");
                if(subNode != null)
                {
                    Points = int.Parse(subNode.InnerText);
                    PointType = "Arena";
                }
            }

            return this;
        }
        public static new ItemLocation Construct()
        {
            PvpItem item = new PvpItem();
            return item;
        }
        static SortedList<string, string> _tokenMap = new SortedList<string, string>();
    }
   
    [Serializable]
    public class StaticDrop : ItemLocation
    {
        public string Area{get;set;}
        public bool Heroic{get;set;}
        public string Boss{get;set;}

        [XmlIgnore]
        public override string Description
        {
            get
            {
                return string.Format("Drops from {0} in {1}{2}", Boss, Heroic ? "Heroic " : "", Area);
            }
        }
        public StaticDrop()
        {
            Source = ItemSource.StaticDrop;
        }

        public override ItemLocation Fill(XmlNode node, string itemId)
        {
            XmlNode subNode = node.SelectSingleNode("page/itemTooltips/itemTooltip/itemSource");
            Area = subNode.Attributes["areaName"].Value;
            Heroic = ("h" == subNode.Attributes["difficulty"].Value);
            Boss = subNode.Attributes["creatureName"].Value;
            return this;
        }
        public static new ItemLocation Construct()
        {
            StaticDrop item = new StaticDrop();
            return item;
        }
    }
   
    [Serializable]
    public class WorldDrop : ItemLocation
    {
        private WorldDrop()
        {
            Source = ItemSource.WorldDrop;
        }


        string Location{get;set;}

        [XmlIgnore]
        public override string Description
        {
            get
            {
                if(Location != null)
                {
                    return string.Format("Trash drop in {0}", Location);
                }
                return "World Drop";
            }
        }
        public override ItemLocation Fill(XmlNode node, string itemId)
        {


            WebRequestWrapper wrw = new WebRequestWrapper();
            XmlDocument doc = wrw.DownloadItemInformation(int.Parse(itemId));

            XmlNode subNode = doc.SelectSingleNode("/page/itemInfo/item/dropCreatures/creature[1]/@area");
            if(subNode != null)
            {
                Location = subNode.InnerText;
            }

            return this;
        }
        public static new ItemLocation Construct()
        {
            WorldDrop item = new WorldDrop();
            return item;
        }

    }

    [Serializable]
    public class CraftedItem : ItemLocation
    {
        private CraftedItem()
        {
            Source = ItemSource.Crafted;
        }
        [XmlIgnore]
        public override string Description
        {
            get
            {
                if(Skill == "Unknown")
                {
                    return "Crafted";
                }
                StringBuilder basic = new StringBuilder();

                if (Bind != BindsOn.None)
                {
                    basic.AppendFormat("{0} crafted {1}({2}) ", Bind.ToString(), Skill, Level);
                }
                else
                {
                    basic.AppendFormat("Crafted {1}({2})", Bind.ToString(), Skill, Level);
                }
                if (BopMats.Count > 0)
                {
                    basic.Append(" using ");
                    foreach (string key in BopMats.Keys)
                    {
                        basic.AppendFormat("{0} {1}", BopMats[key], key);
                    }
                }
                return basic.ToString();
            }
        }


        public string Skill {get;set;}
        public int Level {get;set;}
        public BindsOn Bind {get;set;}


        public SerializableDictionary<string, int> BopMats
        {
            get
            {
                return _bopMats;
            }
        }

        private SerializableDictionary<string, int> _bopMats = new SerializableDictionary<string, int>();
        static SortedList<string, BindsOn> _materialBindMap = new SortedList<string, BindsOn>();
        public override ItemLocation Fill(XmlNode node, string itemId)
        {
            WebRequestWrapper wrw = new WebRequestWrapper();
            XmlDocument doc = wrw.DownloadItemInformation(int.Parse(itemId));



            XmlNode subNode = doc.SelectSingleNode("/page/itemInfo/item/createdBy/spell/item");

            if (subNode != null)
            {
                Bind = (BindsOn) Enum.Parse(typeof(BindsOn), node.SelectSingleNode("/page/itemTooltips/itemTooltip/bonding").InnerText);
            }
            else
            {
                subNode = doc.SelectSingleNode("/page/itemInfo/item");
                if (subNode.Attributes["requiredSkill"]!= null)
                {
                    Bind = BindsOn.BoP;
                }
                else
                {
                    Bind = BindsOn.BoE;
                }
            }
            if (subNode.Attributes["requiredSkill"] != null)
            {
                Skill = subNode.Attributes["requiredSkill"].Value;
                Level = int.Parse(subNode.Attributes["requiredSkillRank"].Value);
            }
            else 
            {
                Skill = "Unknown";
            }



            foreach(XmlNode reagent in doc.SelectNodes("/page/itemInfo/item/createdBy/spell/reagent"))
            {
                string name = reagent.Attributes["name"].Value;
                int count = int.Parse(reagent.Attributes["count"].Value);
                if (!_materialBindMap.ContainsKey(name))
                {
                    wrw = new WebRequestWrapper();
                    doc = wrw.DownloadItemToolTipSheet(reagent.Attributes["id"].Value);

                    _materialBindMap[name] = (BindsOn) Enum.Parse(typeof(BindsOn), doc.SelectSingleNode("/page/itemTooltips/itemTooltip/bonding").InnerText);
                }

                if (_materialBindMap[name] == BindsOn.BoP)
                {
                    BopMats[name] = count;
                }

            }

            return this;
        }
        public static new ItemLocation Construct()
        {
            CraftedItem item = new CraftedItem();
            return item;
        }
    }

    [Serializable]
    public class QuestItem : ItemLocation
    {
        private QuestItem()
        {
            Source = ItemSource.Quest;
        }
        [XmlIgnore]
        public override string Description
        {
            get
            {
                return string.Format("Reward from [{0}{1}{2}] {3} in {4}", MinLevel, Party > 0 ? "g" : "", Party > 0 ? Party.ToString() : "", Quest, Area);                
            }
        }


        String Area {get;set;}
        String Quest {get;set;}
        int MinLevel {get;set;}
        int Party {get;set;}

        public override ItemLocation Fill(XmlNode node, string itemId)
        {
            WebRequestWrapper wrw = new WebRequestWrapper();
            XmlDocument doc = wrw.DownloadItemInformation(int.Parse(itemId));

            XmlNode subNode = doc.SelectSingleNode("/page/itemInfo/item/rewardFromQuests/quest[1]");

            Area = subNode.Attributes["area"].Value;
            Quest = subNode.Attributes["name"].Value;
            MinLevel = int.Parse(subNode.Attributes["reqMinLevel"].Value);
            Party = int.Parse(subNode.Attributes["suggestedPartySize"].Value);
            return this;
        }
        public static new ItemLocation Construct()
        {
            QuestItem item = new QuestItem();
            return item;
        }
    }

    [Serializable]
    public class ContainerItem : ItemLocation
    {
        private ContainerItem()
        {
            Source = ItemSource.Container;
        }
        [XmlIgnore]
        public override string Description
        {
            get
            {
                return string.Format("Found in {0} in {1}", Container, Area);
            }
        }


        String Area { get; set; }
        String Container { get; set; }
        int MinLevel { get; set; }
        int Party { get; set; }

        public override ItemLocation Fill(XmlNode node, string itemId)
        {
            WebRequestWrapper wrw = new WebRequestWrapper();
            XmlDocument doc = wrw.DownloadItemInformation(int.Parse(itemId));

            XmlNode subNode = doc.SelectSingleNode("/page/itemInfo/item/containerObjects/object[1]");

            Area = subNode.Attributes["area"].Value;
            Container = subNode.Attributes["name"].Value;
            return this;
        }
        public static new ItemLocation Construct()
        {
            ContainerItem item = new ContainerItem();
            return item;
        }
    }
    public static class LocationFactory
    {
        static LocationFactory()
        {
            _LocationFactory = new SortedList<string, Construct>();

            _LocationFactory["sourceType.worldDrop"] = WorldDrop.Construct;
            _LocationFactory["sourceType.creatureDrop"] = StaticDrop.Construct;
            _LocationFactory["sourceType.pvpReward"] = PvpItem.Construct;
            _LocationFactory["sourceType.factionReward"] = FactionItem.Construct;
            _LocationFactory["sourceType.vendor"] = VendorItem.Construct;
            _LocationFactory["sourceType.createdByPlans"] = CraftedItem.Construct;
            _LocationFactory["sourceType.createdBySpell"] = CraftedItem.Construct;
            _LocationFactory["sourceType.questReward"] = QuestItem.Construct;
            _LocationFactory["sourceType.gameObjectDrop"] = ContainerItem.Construct;
            _LocationFactory["sourceType.none"] = ItemLocation.Construct;
        }

        public static ItemLocation Create(XmlNode node, string itemId)
        {
            ItemLocation item = null;
            try
            {
                string sourceType = node.SelectSingleNode("page/itemTooltips/itemTooltip/itemSource").Attributes["value"].Value;
                if (_LocationFactory.ContainsKey(sourceType))
                {
                    item = _LocationFactory[sourceType]();
                    item = item.Fill(node, itemId);
                }
                else
                {
                    throw new Exception("Unrecognized item source " + sourceType);
                }
            }
            catch (System.Exception e)
            {
                item = new ItemLocation("Failed - " + e.Message);
            }

            return item;
        }

        static SortedList<string, Construct> _LocationFactory = null;
    }
}
