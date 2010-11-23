using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.ProtWarr
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalculationOptionsProtWarr : ICalculationOptionBase, INotifyPropertyChanged
    {
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsProtWarr));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

		public CalculationOptionsProtWarr()
		{
			BossAttackValue = 80000;
			BossAttackSpeed = 2.0f;
            ThreatScale = 8.0f;
			MitigationScale = 0.125f;
			RankingMode = 1;
            HeroicStrikeFrequency = 0.9f;
        }

        private int _bossAttackValue;
        public int BossAttackValue
        {
            get { return _bossAttackValue; }
            set { _bossAttackValue = value; OnPropertyChanged("BossAttackValue"); }
        }

        private float _bossAttackSpeed;
        public float BossAttackSpeed
        {
            get { return _bossAttackSpeed; }
            set { _bossAttackSpeed = value; OnPropertyChanged("BossAttackSpeed"); }
        }

        private float _threatScale;
        public float ThreatScale
        {
            get { return _threatScale; }
            set { _threatScale = value; OnPropertyChanged("ThreatScale"); }
        }

        private float _mitigationScale;
        public float MitigationScale
        {
            get { return _mitigationScale; }
            set { _mitigationScale = value; OnPropertyChanged("MitigationScale"); }
        }

        private int _rankingMode;
        public int RankingMode
        {
            get { return _rankingMode; }
            set { _rankingMode = value; OnPropertyChanged("RankingMode"); }
        }

        private bool _useVigilance;
        public bool UseVigilance
        {
            get { return _useVigilance; }
            set { _useVigilance = value; OnPropertyChanged("UseVigilance"); }
        }

        private int _vigilanceValue;
        public int VigilanceValue
        {
            get { return _vigilanceValue; }
            set { _vigilanceValue = value; OnPropertyChanged("VigilanceValue"); }
        }

        private float _heroicStrikeFrequency;
        public float HeroicStrikeFrequency
        {
            get { return _heroicStrikeFrequency; }
            set { _heroicStrikeFrequency = value; OnPropertyChanged("HeroicStrikeFrequency"); }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        #endregion

    }
}
