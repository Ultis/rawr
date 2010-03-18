using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rawr.Rogue
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsRogue : 
        ICalculationOptionBase,
        INotifyPropertyChanged,
        ICharacterCalculationOptions
    {
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        private int _targetLevel = 83;
        public int TargetLevel
        {
            get { return _targetLevel; }
            set { if (_targetLevel != value) { _targetLevel = value; OnPropertyChanged("TargetLevel"); } }
        }
        private int _targetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
        public int TargetArmor
        {
            get { return _targetArmor; }
            set { if (_targetArmor != value) { _targetArmor = value; OnPropertyChanged("TargetArmor"); } }
        }
        private bool _targetPoisonable = true;
        public bool TargetPoisonable
        {
            get { return _targetPoisonable; }
            set { if (_targetPoisonable != value) { _targetPoisonable = value; OnPropertyChanged("TargetPoisonable"); } }
        }
        private int _customCPG = 1;
        public int CustomCPG
        {
            get { return _customCPG; }
            set { if (_customCPG != value) { _customCPG = value; OnPropertyChanged("CustomCPG"); } }
        }
        private bool _customUseRupt = false;
        public bool CustomUseRupt
        {
            get { return _customUseRupt; }
            set { if (_customUseRupt != value) { _customUseRupt = value; OnPropertyChanged("CustomUseRupt"); } }
        }
        private bool _customUseExpose = false;
        public bool CustomUseExpose
        {
            get { return _customUseExpose; }
            set { if (_customUseExpose != value) { _customUseExpose = value; OnPropertyChanged("CustomUseExpose"); } }
        }
        private bool _customUseTotT = false;
        public bool CustomUseTotT
        {
            get { return _customUseTotT; }
            set { if (_customUseTotT != value) { _customUseTotT = value; OnPropertyChanged("CustomUseTotT"); } }
        }
        private bool _bleedIsUp = true;
        public bool BleedIsUp
        {
            get { return _bleedIsUp; }
            set { if (_bleedIsUp != value) { _bleedIsUp = value; OnPropertyChanged("BleedIsUp"); } }
        }
        private int _customFinisher = 1;
        public int CustomFinisher
        {
            get { return _customFinisher; }
            set { if (_customFinisher != value) { _customFinisher = value; OnPropertyChanged("CustomFinisher"); } }
        }
        private int _customCPFinisher = 5;
        public int CustomCPFinisher
        {
            get { return _customCPFinisher; }
            set { if (_customCPFinisher != value) { _customCPFinisher = value; OnPropertyChanged("CustomCPFinisher"); } }
        }
        private int _customCPSnD = 5;
        public int CustomCPSnD
        {
            get { return _customCPSnD; }
            set { if (_customCPSnD != value) { _customCPSnD = value; OnPropertyChanged("CustomCPSnD"); } }
        }
        private int _customMHPoison = 1;
        public int CustomMHPoison
        {
            get { return _customMHPoison; }
            set { if (_customMHPoison != value) { _customMHPoison = value; OnPropertyChanged("CustomMHPoison"); } }
        }
        private int _customOHPoison = 2;
        public int CustomOHPoison
        {
            get { return _customOHPoison; }
            set { if (_customOHPoison != value) { _customOHPoison = value; OnPropertyChanged("CustomOHPoison"); } }
        }
        private int _duration = 300;
        public int Duration
        {
            get { return _duration; }
            set { if (_duration != value) { _duration = value; OnPropertyChanged("Duration"); } }
        }
        private float _trinketOffset = 0f;
        public float TrinketOffset
        {
            get { return _trinketOffset; }
            set { if (_trinketOffset != value) { _trinketOffset = value; OnPropertyChanged("TrinketOffset"); } }
        }
        private bool _PTRMode = false;
        public bool PTRMode
        {
            get { return _PTRMode; }
            set { if (_PTRMode != value) { _PTRMode = value; OnPropertyChanged("PTRMode"); } }
        }
        private int _lagVariance = 200;
        public int LagVariance
        {
            get { return _lagVariance; }
            set { if (_lagVariance != value) { _lagVariance = value; OnPropertyChanged("LagVariance"); } }
        }

        private Character _character;
        [XmlIgnore]
        public Character Character
        {
            get
            {
                return _character;
            }
            set
            {
                _character = value;
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            if (Character != null)
            {
                Character.OnCalculationsInvalidated();
            }
        }
        #endregion
    }
}