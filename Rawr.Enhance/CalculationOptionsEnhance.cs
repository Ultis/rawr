using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Enhance
{
#if !SILVERLIGHT
	[Serializable]
#endif
    public class CalculationOptionsEnhance : ICalculationOptionBase, INotifyPropertyChanged
	{
        // General parameters
        private int _averageLag = 250;
        private int _reactionTime = 250;
        private string _mainhandImbue = "Windfury";
        private string _offhandImbue = "Flametongue";
        private float _minManaSR = 1250;
        private bool _baseStatOption = true;
        private bool _useMana = false;
        private bool _showExportMessageBox = true;
        private bool[] _statsList = new bool[] { true, true, true, true, true, true, true, true, true, true };
        private int _statsIncrement = 120;
        private string _calculationToGraph = "DPS Rating";
        private SerializableDictionary<EnhanceAbility, Priority> _priorityList;
        // this can be read from multiple threads, make sure all modifications are only done from
        // when no calculations are going on so we can avoid locking
        // make sure it is updated whenever changes to _priorityList are made
        private List<KeyValuePair<EnhanceAbility, Priority>> _sortedList;

        public CalculationOptionsEnhance()
        {
            _priorityList = SetPriorityDefaults();
            SortPriorities();
        }

        #region Getter/Setter
        public int AverageLag { get { return _averageLag; } set { _averageLag = value; OnPropertyChanged("AverageLag"); } }
        public int ReactionTime { get { return _reactionTime; } set { _reactionTime = value; OnPropertyChanged("ReactionTime"); } }
        public float MinManaSR { get { return _minManaSR; } set { _minManaSR = value; OnPropertyChanged("MinManaSR"); } }
        public string MainhandImbue { get { return _mainhandImbue; } set { _mainhandImbue = value; OnPropertyChanged("MainhandImbue"); } }
        public string OffhandImbue { get { return _offhandImbue; } set { _offhandImbue = value; OnPropertyChanged("OffhandImbue"); } }
        public bool Magma { get { return PriorityInUse(EnhanceAbility.MagmaTotem); } }
        public bool Searing { get { return PriorityInUse(EnhanceAbility.SearingTotem); } }
        public bool FireElemental { get { return PriorityInUse(EnhanceAbility.FireElemental); } }
        public bool BaseStatOption { get { return _baseStatOption; } set { _baseStatOption = value; OnPropertyChanged("BaseStatOption"); } }
        public bool UseMana { get { return _useMana; } set { _useMana = value; OnPropertyChanged("UseMana"); } }
        public bool ShowExportMessageBox { get { return _showExportMessageBox; } set { _showExportMessageBox = value; OnPropertyChanged("ShowExportMessageBox"); } }
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        public int StatsIncrement { get { return _statsIncrement; } set { _statsIncrement = value; OnPropertyChanged("StatsIncrement"); } }
        public string CalculationToGraph { get { return _calculationToGraph; } set { _calculationToGraph = value; OnPropertyChanged("CalculationToGraph"); } }
        public SerializableDictionary<EnhanceAbility, Priority> PriorityList { get { return _priorityList; } set { _priorityList = value; SortPriorities(); OnPropertyChanged("PriorityList"); } }
        
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsEnhance));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion

        public bool PriorityInUse(EnhanceAbility abilityType)
        {
            Priority p;
            if (_priorityList.TryGetValue(abilityType, out p))
            {
                return p.Checked;
            }
            return false;
        }

        public Priority GetAbilityPriority(EnhanceAbility abilityType)
        {
            return _priorityList[abilityType];
        }

        public int GetAbilityPriorityValue(EnhanceAbility abilityType)
        {
            int priority = 0;
            foreach (KeyValuePair<EnhanceAbility, Priority> kvp in _sortedList)
            {
                Priority p = kvp.Value;
                if (p.Checked && p.PriorityValue > 0) 
                    priority++;
                if (p.AbilityType == abilityType)
                    return p.Checked ? priority : 0;
            }
            return -1;
        }

        public void SetAbilityPriority(EnhanceAbility abilityType, Priority priority)
        {
            Priority value = GetAbilityPriority(abilityType);
            if (value.AbilityType != EnhanceAbility.None)
                _priorityList.Remove(abilityType);
            if (priority.PriorityValue > 0)
                _priorityList.Add(abilityType, priority);
            // update the sorted list
            SortPriorities();
        }

        public int ActivePriorities()
        {
            int activePriorities = 0;
            foreach (Priority p in _priorityList.Values)
            {
                if (p.Checked && p.PriorityValue > 0) activePriorities++;
            }
            return activePriorities;
        }

        // make sure not to call this from calculations, this should be setup before calculations start
        public void SortPriorities()
        {
            _sortedList = new List<KeyValuePair<EnhanceAbility, Priority>>(_priorityList);
            _sortedList.Sort(
                delegate(KeyValuePair<EnhanceAbility, Priority> firstPair, KeyValuePair<EnhanceAbility, Priority> nextPair)
                {
                    return firstPair.Value.PriorityValue.CompareTo(nextPair.Value.PriorityValue);
                }
            );
        }

        public static SerializableDictionary<EnhanceAbility, Priority> SetPriorityDefaults()
        {
            SerializableDictionary<EnhanceAbility, Priority> priorityList = new SerializableDictionary<EnhanceAbility, Priority>();
            if (priorityList.Count == 0)
            {
                int priority = 0;
                priorityList.Add(EnhanceAbility.FeralSpirits, new Priority("Feral Spirits", EnhanceAbility.FeralSpirits, "Use Feral Sprirts", true, ++priority, "SW"));
                priorityList.Add(EnhanceAbility.StormStrike, new Priority("Stormstrike", EnhanceAbility.StormStrike, "Use Stormstrike", true, ++priority, "SS"));
                priorityList.Add(EnhanceAbility.LavaLash, new Priority("Lava Lash", EnhanceAbility.LavaLash, "Use Lava Lash", true, ++priority, "LL"));
                priorityList.Add(EnhanceAbility.FlameShock, new Priority("Flame Shock", EnhanceAbility.FlameShock, "Use Flame Shock if no Flame Shock debuff on target", true, ++priority, "FS"));
                priorityList.Add(EnhanceAbility.UnleashElements, new Priority("Unleash Elements", EnhanceAbility.UnleashElements, "Use Unleash Elements", true, ++priority, "UE"));
                priorityList.Add(EnhanceAbility.LightningBolt, new Priority("Lightning Bolt on 5 stacks of MW", EnhanceAbility.LightningBolt, "Use Lightning Bolt when you have 5 stacks of Maelstrom Weapon", true, ++priority, "MW5_LB"));
                priorityList.Add(EnhanceAbility.EarthShock, new Priority("Earth Shock", EnhanceAbility.EarthShock, "Use Earth Shock", true, ++priority, "ES"));
                priorityList.Add(EnhanceAbility.FireElemental, new Priority("Fire Elemental", EnhanceAbility.FireElemental, "Drop Fire Elemental Totem", true, ++priority, "FE"));
                priorityList.Add(EnhanceAbility.SearingTotem, new Priority("Searing Totem", EnhanceAbility.SearingTotem, "Refresh Searing Totem", true, ++priority, "ST"));
                priorityList.Add(EnhanceAbility.ShamanisticRage, new Priority("Shamanistic Rage", EnhanceAbility.ShamanisticRage, "Use Shamanistic Rage", true, ++priority, "SR"));
                priorityList.Add(EnhanceAbility.LightningShield, new Priority("Lightning Shield", EnhanceAbility.LightningShield, "Refresh Lightning Shield", true, ++priority, "LS"));
                priorityList.Add(EnhanceAbility.RefreshTotems, new Priority("Refresh Totems", EnhanceAbility.RefreshTotems, "Refresh Totems", true, ++priority, ""));
            }
            return priorityList;
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
	}
}
