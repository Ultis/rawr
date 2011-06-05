using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.ShadowPriest
{
    /// <summary>
    /// Holds the options the user selects from the options tab.
    /// </summary>
    public class CalculationOptionsShadowPriest : ICalculationOptionBase, INotifyPropertyChanged
    {
        /// <summary>Maintain Inner Fire myself</summary>
        [DefaultValue(true)]
        public bool InnerFire { get { return _innerFire; } set { _innerFire = value; OnPropertyChanged("InnerFire"); } }
        private bool _innerFire = true;

        /// <summary>Gets or sets the latency of casts</summary>
        /// <value>The latency cast</value>
        [DefaultValue(0.075f)]
        public float LatencyCast { get { return _latencyCast; } set { _latencyCast = value; OnPropertyChanged("LatencyCast"); } }
        private float _latencyCast = 0.075f;

        /// <summary>Gets or sets the latency of the Global Cooldown</summary>
        /// <value>The latency global cooldown.</value>
        [DefaultValue(0.15f)]
        public float LatencyGcd { get { return _latencyGcd; } set { _latencyGcd = value; OnPropertyChanged("LatencyGcd"); } }
        private float _latencyGcd = 0.15f;

        /// <summary>Gets or sets the number of targets</summary>
        /// <value>The number of targets.</value>
        [Obsolete("Number Of Targets should be pulled from BossHandler instead of stored in CalcOpts")]
        [DefaultValue(1)]
        public int NumberOfTargets { get { return _numberOfTargets; } set { _numberOfTargets = value; OnPropertyChanged("NumberOfTargets"); } }
        private int _numberOfTargets = 1;

        #region Stat Graph
        [DefaultValue(new bool[] { true, true, true, true, true, /**/ true, true, true, true, })]
        public bool[] StatsList { get { return _statsList; } set { _statsList = value; OnPropertyChanged("StatsList"); } }
        private bool[] _statsList = new bool[] { true, true, true, true, true, /**/ true, true, true, true, };
        [DefaultValue(100)]
        public int StatsIncrement { get { return _StatsIncrement; } set { _StatsIncrement = value; OnPropertyChanged("StatsIncrement"); } }
        private int _StatsIncrement = 100;
        [DefaultValue("Overall Rating")]
        public string CalculationToGraph { get { return _calculationToGraph; } set { _calculationToGraph = value; OnPropertyChanged("CalculationToGraph"); } }
        private string _calculationToGraph = "Overall Rating";
        [XmlIgnore]
        public bool SG_SP { get { return StatsList[0]; } set { StatsList[0] = value; OnPropertyChanged("SG_SP"); } }
        [XmlIgnore]
        public bool SG_Int { get { return StatsList[1]; } set { StatsList[1] = value; OnPropertyChanged("SG_Int"); } }
        [XmlIgnore]
        public bool SG_Spi { get { return StatsList[2]; } set { StatsList[2] = value; OnPropertyChanged("SG_Spi"); } }
        [XmlIgnore]
        public bool SG_Crit { get { return StatsList[3]; } set { StatsList[3] = value; OnPropertyChanged("SG_Crit"); } }
        [XmlIgnore]
        public bool SG_Hit { get { return StatsList[4]; } set { StatsList[4] = value; OnPropertyChanged("SG_Hit"); } }
        [XmlIgnore]
        public bool SG_Exp { get { return StatsList[5]; } set { StatsList[5] = value; OnPropertyChanged("SG_Exp"); } }
        [XmlIgnore]
        public bool SG_Haste { get { return StatsList[6]; } set { StatsList[6] = value; OnPropertyChanged("SG_Haste"); } }
        [XmlIgnore]
        public bool SG_Mstr { get { return StatsList[7]; } set { StatsList[7] = value; OnPropertyChanged("SG_Mstr"); } }
        [XmlIgnore]
        public bool SG_SpPen { get { return StatsList[8]; } set { StatsList[8] = value; OnPropertyChanged("SG_SpPen"); } }
        #endregion

        #region ICalculationOptionBase Members
        /// <summary>
        /// Gets the XML serialization of the calculation options for use in the character file.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsShadowPriest));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion
        #region INotifyPropertyChanged Members
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) { if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); } }
        /*private void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> property) {
            MemberExpression memberExpression;
            if (property.Body is UnaryExpression) {
                UnaryExpression unaryExpression = (UnaryExpression) property.Body;
                memberExpression = (MemberExpression) unaryExpression.Operand;
            } else { memberExpression = (MemberExpression) property.Body; }

            OnPropertyChanged(memberExpression.Member.Name);
        }*/
        #endregion
    }
}