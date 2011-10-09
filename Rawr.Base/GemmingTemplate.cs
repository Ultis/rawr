using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Rawr
{
    public class GemmingTemplate : INotifyPropertyChanged
    {
        [XmlElement("Id")]
        public string _id;
        [XmlIgnore]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [XmlElement("Model")]
        public string _model;
        [XmlIgnore]
        public string Model
        {
            get { return _model; }
            set { _model = value; }
        }

        [XmlElement("Enabled")]
        public bool _enabled;
        [XmlIgnore]
        public bool Enabled
        {
            get { return _enabled; }
            set 
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    OnPropertyChanged("Enabled");
                }
            }
        }

        [XmlElement("Group")]
        public string _group;
        [XmlIgnore]
        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        [XmlElement("RedId")]
        public int _redId;
        [XmlIgnore]
        public int RedId
        {
            get { return _redId; }
            set { _redId = value; }
        }

        [XmlIgnore]
        private Item _redGem;
        [XmlIgnore]
        public Item RedGem
        {
            get
            {
                if (RedId == 0) return null;
                if (_redGem == null || _redGem.Id != RedId)
                {
                    _redGem = Item.LoadFromId(RedId, false, true, true, false);
                }
                return _redGem;
            }
            set
            {
                if (value == null)
                    RedId = 0;
                else
                    RedId = value.Id;
                _redGem = value;
                OnPropertyChanged("RedGem");
            }
        }

        [XmlElement("YellowId")]
        public int _yellowId;
        [XmlIgnore]
        public int YellowId
        {
            get { return _yellowId; }
            set { _yellowId = value; }
        }

        [XmlIgnore]
        private Item _yellowGem;
        [XmlIgnore]
        public Item YellowGem
        {
            get
            {
                if (YellowId == 0) return null;
                if (_yellowGem == null || _yellowGem.Id != YellowId)
                {
                    _yellowGem = Item.LoadFromId(YellowId, false, true, true, false);
                }
                return _yellowGem;
            }
            set
            {
                if (value == null)
                    YellowId = 0;
                else
                    YellowId = value.Id;
                _yellowGem = value;
                OnPropertyChanged("YellowGem");
            }
        }

        [XmlElement("BlueId")]
        public int _blueId;
        [XmlIgnore]
        public int BlueId
        {
            get { return _blueId; }
            set { _blueId = value; }
        }

        [XmlIgnore]
        private Item _blueGem;
        [XmlIgnore]
        public Item BlueGem
        {
            get
            {
                if (BlueId == 0) return null;
                if (_blueGem == null || _blueGem.Id != BlueId)
                {
                    _blueGem = Item.LoadFromId(BlueId, false, true, true, false);
                }
                return _blueGem;
            }
            set
            {
                if (value == null)
                    BlueId = 0;
                else
                    BlueId = value.Id;
                _blueGem = value;
                OnPropertyChanged("BlueGem");
            }
        }

        [XmlElement("CogwheelId")]
        public int _cogwheelId;
        [XmlIgnore]
        public int CogwheelId
        {
            get { return _cogwheelId; }
            set { _cogwheelId = value; }
        }

        [XmlElement("Cogwheel2Id")]
        public int _cogwheel2Id;
        [XmlIgnore]
        public int Cogwheel2Id
        {
            get { return _cogwheel2Id; }
            set { _cogwheel2Id = value; }
        }

        [XmlIgnore]
        private Item _cogwheel;
        [XmlIgnore]
        public Item Cogwheel
        {
            get
            {
                if (CogwheelId == 0) return null;
                if (_cogwheel == null || _cogwheel.Id != CogwheelId)
                {
                    _cogwheel = Item.LoadFromId(CogwheelId, false, true, true, false);
                }
                return _cogwheel;
            }
            set
            {
                if (value == null)
                    CogwheelId = 0;
                else
                    CogwheelId = value.Id;
                _cogwheel = value;
                OnPropertyChanged("Cogwheel");
            }
        }

        [XmlIgnore]
        private Item _cogwheel2;
        [XmlIgnore]
        public Item Cogwheel2
        {
            get
            {
                if (Cogwheel2Id == 0) return null;
                if (_cogwheel2 == null || _cogwheel2.Id != Cogwheel2Id)
                {
                    _cogwheel2 = Item.LoadFromId(Cogwheel2Id, false, true, true, false);
                }
                return _cogwheel2;
            }
            set
            {
                if (value == null)
                    Cogwheel2Id = 0;
                else
                    Cogwheel2Id = value.Id;
                _cogwheel2 = value;
                OnPropertyChanged("Cogwheel2");
            }
        }

        [XmlElement("HydraulicId")]
        public int _hydraulicId;
        [XmlIgnore]
        public int HydraulicId
        {
            get { return _hydraulicId; }
            set { _hydraulicId = value; }
        }

        [XmlIgnore]
        private Item _hydraulic;
        [XmlIgnore]
        public Item Hydraulic
        {
            get
            {
                if (HydraulicId == 0) return null;
                if (_hydraulic == null || _hydraulic.Id != BlueId)
                {
                    _hydraulic = Item.LoadFromId(HydraulicId, false, true, true, false);
                }
                return _hydraulic;
            }
            set
            {
                if (value == null)
                    HydraulicId = 0;
                else
                    HydraulicId = value.Id;
                _hydraulic = value;
                OnPropertyChanged("Hydraulic");
            }
        }
        
        [XmlElement("MetaId")]
        public int _metaId;
        [XmlIgnore]
        public int MetaId
        {
            get { return _metaId; }
            set { _metaId = value; }
        }

        [XmlIgnore]
        private Item _metaGem;
        [XmlIgnore]
        public Item MetaGem
        {
            get
            {
                if (MetaId == 0) return null;
                if (_metaGem == null || _metaGem.Id != MetaId)
                {
                    _metaGem = Item.LoadFromId(MetaId, false, true, true, false);
                }
                return _metaGem;
            }
            set
            {
                if (value == null)
                    MetaId = 0;
                else
                    MetaId = value.Id;
                _metaGem = value;
                OnPropertyChanged("MetaGem");
            }
        }

        [XmlElement("PrismaticId")]
        public int _prismaticId;
        [XmlIgnore]
        public int PrismaticId
        {
            get { return _prismaticId; }
            set { _prismaticId = value; }
        }

        [XmlIgnore]
        private Item _prismaticGem;
        [XmlIgnore]
        public Item PrismaticGem
        {
            get
            {
                if (PrismaticId == 0) return null;
                if (_prismaticGem == null || _prismaticGem.Id != PrismaticId)
                {
                    _prismaticGem = Item.LoadFromId(PrismaticId, false, true, true, false);
                }
                return _prismaticGem;
            }
            set
            {
                if (value == null)
                    PrismaticId = 0;
                else
                    PrismaticId = value.Id;
                _prismaticGem = value;
                OnPropertyChanged("PrismaticGem");
            }
        }

        public ItemInstance GetItemInstance(Item item, int randomSuffixId, Enchant enchant, bool blacksmithingSocket)
        {
            return GetItemInstance(item, randomSuffixId, enchant, null, null, blacksmithingSocket);
        }

        public ItemInstance GetItemInstance(Item item, int randomSuffixId, Reforging reforging, bool blacksmithingSocket)
        {
            return GetItemInstance(item, randomSuffixId, null, reforging, null, blacksmithingSocket);
        }

        public ItemInstance GetItemInstance(Item item, int randomSuffixId, Tinkering tinkering, bool blacksmithingSocket)
        {
            return GetItemInstance(item, randomSuffixId, null, null, tinkering, blacksmithingSocket);
        }

        public ItemInstance GetItemInstance(Item item, int randomSuffixId, Enchant enchant, Reforging reforging, Tinkering tinkering, bool blacksmithingSocket)
        {
            if (item == null) return null;
            Item gem1 = null;
            Item gem2 = null;
            Item gem3 = null;
            bool cog1used = false;
            switch (item.SocketColor1)
            {
                case ItemSlot.Meta: gem1 = MetaGem; break;
                case ItemSlot.Red: gem1 = RedGem; break;
                case ItemSlot.Yellow: gem1 = YellowGem; break;
                case ItemSlot.Blue: gem1 = BlueGem; break;
                case ItemSlot.Prismatic: gem1 = PrismaticGem; break;
                case ItemSlot.Cogwheel: gem1 = Cogwheel; cog1used = true; break;
                case ItemSlot.Hydraulic: gem1 = Hydraulic; break;
                case ItemSlot.None: 
                    if (blacksmithingSocket)
                    {
                        gem1 = PrismaticGem;
                        blacksmithingSocket = false;
                    }
                    break;
            }
            switch (item.SocketColor2)
            {
                case ItemSlot.Meta: gem2 = MetaGem; break;
                case ItemSlot.Red: gem2 = RedGem; break;
                case ItemSlot.Yellow: gem2 = YellowGem; break;
                case ItemSlot.Blue: gem2 = BlueGem; break;
                case ItemSlot.Prismatic: gem2 = PrismaticGem; break;
                case ItemSlot.Cogwheel: if (cog1used) { gem2 = Cogwheel2; } else { gem2 = Cogwheel; cog1used = true; } break;
                case ItemSlot.Hydraulic: gem2 = Hydraulic; break;
                case ItemSlot.None:
                    if (blacksmithingSocket)
                    {
                        gem2 = PrismaticGem;
                        blacksmithingSocket = false;
                    }
                    break;
            }
            switch (item.SocketColor3)
            {
                case ItemSlot.Meta: gem3 = MetaGem; break;
                case ItemSlot.Red: gem3 = RedGem; break;
                case ItemSlot.Yellow: gem3 = YellowGem; break;
                case ItemSlot.Blue: gem3 = BlueGem; break;
                case ItemSlot.Prismatic: gem3 = PrismaticGem; break;
                case ItemSlot.Cogwheel: if (cog1used) { gem3 = Cogwheel2; } else { gem3 = Cogwheel; cog1used = true; } break;
                case ItemSlot.Hydraulic: gem3 = Hydraulic; break;
                case ItemSlot.None:
                    if (blacksmithingSocket)
                    {
                        gem3 = PrismaticGem;
                        blacksmithingSocket = false;
                    }
                    break;
            }
            return new ItemInstance(item, randomSuffixId, gem1, gem2, gem3, enchant, reforging, tinkering);
        }

        public override string ToString()
        {
            return string.Format("{0} # {1}: ({2}) R:{3} Y:{4} B:{5} C1:{6} C2:{7} H:{8} M:{9} P:{10}",
                Group, Id, Enabled ? "On" : "Off", RedId, YellowId, BlueId, CogwheelId, Cogwheel2Id, HydraulicId, MetaId, PrismaticId);
        }

        private static Dictionary<string, List<GemmingTemplate>> _allTemplates = new Dictionary<string, List<GemmingTemplate>>();
        public static Dictionary<string, List<GemmingTemplate>> AllTemplates
        {
            get { return _allTemplates; }
        }

        public static List<GemmingTemplate> CurrentTemplates
        {
            get
            {
                List<GemmingTemplate> list;
                if (!AllTemplates.TryGetValue(Calculations.Instance.Name, out list))
                {
                    list = new List<GemmingTemplate>(Calculations.Instance.DefaultGemmingTemplates);
                    AllTemplates[Calculations.Instance.Name] = list;
                }
                return list;
            }
        }

        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
