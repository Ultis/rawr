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

namespace Rawr.Moonkin
{
    public class CalculationOptionsMoonkin : ICalculationOptionBase, System.ComponentModel.INotifyPropertyChanged
    {
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMoonkin));
            System.Text.StringBuilder xml = new System.Text.StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public CalculationOptionsMoonkin()
        {
        }

        public CalculationOptionsMoonkin(Character character)
        {
        }

        private int targetLevel = 83;
        public int TargetLevel
        {
            get { return targetLevel; }
            set { targetLevel = value; OnPropertyChanged("TargetLevel"); }
        }

        private float latency = 0.2f;
		public float Latency
		{
			get { return latency; }
			set { latency = value; OnPropertyChanged("Latency"); }
		}
        public float ExternalLatency
        {
            get { return latency * 1000f; }
            set { latency = value / 1000f; OnPropertyChanged("Latency"); }
        }

        private float fightLength = 5;
        public float FightLength
        {
            get { return fightLength; }
            set { fightLength = value; OnPropertyChanged("FightLength"); }
        }

        private bool innervate = false;
        public bool Innervate
        {
            get { return innervate; }
            set { innervate = value; OnPropertyChanged("Innervate"); }
        }

        private float innervateDelay = 1;
        public float InnervateDelay
        {
            get { return innervateDelay; }
            set { innervateDelay = value; OnPropertyChanged("InnervateDelay"); }
        }

        private bool manaPots = false;
        public bool ManaPots
        {
            get { return manaPots; }
            set { manaPots = value; OnPropertyChanged("ManaPots"); }
        }

        private string manaPotType = "Runic Mana Potion";
        public string ManaPotType
        {
            get { return manaPotType; }
            set { manaPotType = value; OnPropertyChanged("ManaPotType"); }
        }

        private string aldorScryer = "Aldor";
        public string AldorScryer
        {
            get { return aldorScryer; }
            set { aldorScryer = value; OnPropertyChanged("AldorScryer"); }
        }

        private float replenishmentUptime = 1.0f;
        public float ReplenishmentUptime
        {
            get { return replenishmentUptime; }
            set { replenishmentUptime = value; OnPropertyChanged("ReplenishmentUptime"); }
        }

        private float treantLifespan = 1.0f;
        public float TreantLifespan
        {
            get { return treantLifespan; }
            set { treantLifespan = value; OnPropertyChanged("TreantLifespan"); }
        }

        private bool lunarEclipse = true;
        public bool LunarEclipse
        {
            get { return lunarEclipse; }
            set { lunarEclipse = value; OnPropertyChanged("LunarEclipse"); }
        }

        private bool moonfireAlways = true;
        public bool MoonfireAlways
        {
            get { return moonfireAlways; }
            set { moonfireAlways = value; OnPropertyChanged("MoonfireAlways"); }
        }

        private string userRotation = "None";
        public string UserRotation
        {
            get { return userRotation; }
            set { userRotation = value; OnPropertyChanged("UserRotation"); }
        }


        #region INotifyPropertyChanged Members

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(property));
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
