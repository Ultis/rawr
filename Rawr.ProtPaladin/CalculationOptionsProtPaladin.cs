using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Xml.Serialization;

namespace Rawr.ProtPaladin
{
    public class CalculationOptionsProtPaladin : ICalculationOptionBase, INotifyPropertyChanged
    {

        #region Weighting Adjustments
        /// <summary>
        /// Change ranking mode between Mitigation Scale (the standard across all tanks) and
        /// Burst Time which is a scale of how long before you die in a Burst scenario
        /// </summary>
        [DefaultValue(0)]
        public int RankingMode { get { return _RankingMode; } set { _RankingMode = value; OnPropertyChanged("RankingMode"); } }
        private int _RankingMode = 0;
        /// <summary>
        /// The number of Hits to Survive, which determines your soft cap
        /// </summary>
        [DefaultValue(3.5d)]
        public double HitsToSurvive { get { return _hitsToSurvive; } set { _hitsToSurvive = value; OnPropertyChanged("HitsToSurvive"); } }
        private double _hitsToSurvive = 3.5d;
        /// <summary>
        /// The Scale of Burst's value
        /// </summary>
        [DefaultValue(3.0f)]
        public float BurstScale { get { return _burstScale; } set { _burstScale = value; OnPropertyChanged("BurstScale"); } }
        private float _burstScale = 3.0f;
        /// <summary>
        /// Threat Scaling determines how much higher you value threat over base
        /// </summary>
        [DefaultValue(5.0f)]
        public float ThreatScale { get { return _ThreatScale; } set { _ThreatScale = value; OnPropertyChanged("ThreatScale"); } }
        private float _ThreatScale = 5.0f;
        #endregion

        #region Trinket Handling
        /// <summary>
        /// How Rawr should calculate On Use effects. This should be obsoleted and replaced with a Burst Scale value
        /// </summary>
        [ObsoleteAttribute("This should be replaced with a Burst value calc. The code in place gives you what you need to determine it")]
        [DefaultValue("Ignore")]
        public string TrinketOnUseHandling { get { return _TrinketOnUseHandling; } set { _TrinketOnUseHandling = value; OnPropertyChanged("TrinketOnUseHandling"); } }
        private string _TrinketOnUseHandling = "Ignore";
        #endregion
        
        #region Seal Choice
        /// <summary>
        /// The active seal used in a fight
        /// </summary>
        [DefaultValue("Seal of Truth")]
        public string SealChoice { get { return _SealChoice; } set { _SealChoice = value; OnPropertyChanged("SealChoice"); } }
        private string _SealChoice = "Seal of Truth";
        #endregion

        #region Customize Rotation
        /// <summary>
        /// The Primary Attack in the 9-3-9 sequence
        /// </summary>
        [DefaultValue("Crusader Strike")]
        public string MainAttack { get { return _mainAttack; } set { _mainAttack = value; OnPropertyChanged("MainAttack"); } }
        private string _mainAttack = "Crusader Strike";
        /// <summary>
        /// The Priority order for the 3rd place in the 9-3-9 sequence
        /// </summary>
        [DefaultValue("AS > HW")]
        public string Priority { get { return _priority; } set { _priority = value; OnPropertyChanged("Priority"); } }
        private string _priority = "AS > HW";
        #endregion

        #region PTR Mode
        /// <summary>
        /// When PTR Notes are posted, tie changes occurring in the next patch to this flag for users
        /// </summary>
        [DefaultValue(false)]
        public bool PTRMode { get { return _PTRMode; } set { _PTRMode = value; OnPropertyChanged("PTRMode"); } }
        private bool _PTRMode = false;
        #endregion

        #region ICalculationOptionBase members
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsProtPaladin));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
