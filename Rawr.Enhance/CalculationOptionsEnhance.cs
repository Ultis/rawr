using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Rawr.Enhance
{
#if !SILVERLIGHT
	[Serializable]
#endif
    public class CalculationOptionsEnhance : ICalculationOptionBase, INotifyPropertyChanged
	{
        // Boss parameters
        private BossHandler _boss = new BossHandler();
        private string _bossName = "Custom";
        private int _targetLevel = 83;
        private float _targetArmor = StatConversion.NPC_ARMOR[83 - 80];
        private bool _inBack = true;
        private int _inBackPerc = 100;

        // General parameters
        private int _averageLag = 250;
        private string _mainhandImbue = "Windfury";
        private string _offhandImbue = "Flametongue";
        private float _fightLength = 10.0f;
        private float _targetFireResistance = 0;
        private float _targetNatureResistance = 0;
        private float _minManaSR = 1250;
        private bool _magma = true;
        private bool _baseStatOption = true;
        private bool _useMana = true;
        private Stats[] _statsList = new Stats[] {
                new Stats() { Strength = 1 },
                new Stats() { Agility = 1 },
                new Stats() { AttackPower = 2 },
                new Stats() { CritRating = 1 },
                new Stats() { HitRating = 1 },
                new Stats() { ExpertiseRating = 1 },
                new Stats() { HasteRating = 1 },
                new Stats() { ArmorPenetrationRating = 1 },
                new Stats() { SpellPower = 1.15f }
            };
        private SerializableDictionary<EnhanceAbility, Priority> _priorityList = new SerializableDictionary<EnhanceAbility, Priority>();

        #region Getter/Setter
        public string BossName { get { return _bossName; } set { _bossName = value; OnPropertyChanged("BossName"); } }
        public int TargetLevel { get { return _targetLevel; } set { _targetLevel = value; OnPropertyChanged("TargetLevel"); } }
        public float TargetArmor { get { return _targetArmor; } set { _targetArmor = value; OnPropertyChanged("TargetArmor"); } }
        public bool InBack { get { return _inBack; } set { _inBack = value; OnPropertyChanged("InBack"); } }
        public int InBackPerc { get { return _inBackPerc; } set { _inBackPerc = value; OnPropertyChanged("InBackPerc"); } }
        public int AverageLag { get { return _averageLag; } set { _averageLag = value; OnPropertyChanged("AverageLag"); } }
        public float MinManaSR { get { return _minManaSR; } set { _minManaSR = value; OnPropertyChanged("MinManaSR"); } }
        public string MainhandImbue { get { return _mainhandImbue; } set { _mainhandImbue = value; OnPropertyChanged("MainhandImbue"); } }
        public string OffhandImbue { get { return _offhandImbue; } set { _offhandImbue = value; OnPropertyChanged("OffhandImbue"); } }
        public float FightLength { get { return _fightLength; } set { _fightLength = value; OnPropertyChanged("FightLength"); } }
        public float TargetFireResistance { get { return _targetFireResistance; } set { _targetFireResistance = value; OnPropertyChanged("TargetFireResistance"); } }
        public float TargetNatureResistance { get { return _targetNatureResistance; } set { _targetNatureResistance = value; OnPropertyChanged("TargetNatureResistance"); } }
        public bool Magma { get { return _magma; } set { _magma = value; OnPropertyChanged("Magma"); } }
        public bool BaseStatOption { get { return _baseStatOption; } set { _baseStatOption = value; OnPropertyChanged("BaseStatOption"); } }
        public bool UseMana { get { return _useMana; } set { _useMana = value; OnPropertyChanged("UseMana"); } }
        public Stats[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } } 
        public SerializableDictionary<EnhanceAbility, Priority> PriorityList { get { return _priorityList; } set { _priorityList = value; OnPropertyChanged("PriorityList"); } }
        
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

        public void SetBoss(BossHandler boss) 
        {
            _boss = boss;
            _bossName = boss.Name;
            _targetLevel = boss.Level;
            _targetArmor = (int)boss.Armor;
            _fightLength = boss.BerserkTimer / 60f;
            _inBack = ((_inBackPerc = (int)(boss.InBackPerc_Melee * 100f)) != 0);
            _targetFireResistance = boss.Resistance(ItemDamageType.Fire);
            _targetNatureResistance = boss.Resistance(ItemDamageType.Nature);
        }

        public bool PriorityInUse(EnhanceAbility abilityType)
        {
            Priority p = new Priority();
            _priorityList.TryGetValue(abilityType, out p);
            return p == null ? false : p.Checked;
        }

        public Priority GetAbilityPriority(EnhanceAbility abilityType)
        {
            Priority p = new Priority();
            _priorityList.TryGetValue(abilityType, out p);
            return p;
        }

        public int GetAbilityPriorityValue(EnhanceAbility abilityType)
        {
            Priority p = new Priority();
            _priorityList.TryGetValue(abilityType, out p);
            if (p == null)
                return -1;
            return p.Checked ? p.PriorityValue : 0;  // return 0 if priority not in use
        }

        public void SetAbilityPriority(EnhanceAbility abilityType, Priority priority)
        {
            Priority value = GetAbilityPriority(abilityType);
            if (value.AbilityType != EnhanceAbility.None)
                _priorityList.Remove(abilityType);
            if (priority.PriorityValue > 0)
                _priorityList.Add(abilityType, priority);
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

        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
	}
}
