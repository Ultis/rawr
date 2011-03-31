using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.ShadowPriest
{
    /// <summary>
    /// The additional calculations options for the Shadow Priest module. 
    /// </summary>
    public class CalculationOptionsShadowPriest : ICalculationOptionBase, INotifyPropertyChanged
    {
        public CalculationOptionsShadowPriest()
        {
            _innerFire = true;
        }

        #region Fields

        [XmlIgnore] 
        public CharacterCalculationsShadowPriest calculatedStats;

        private float _latencyCast = .075f;
        private float _latencyGcd = .15f;
        private int _numberOfTargets = 1;
        private bool _innerFire;

        #endregion

        #region Properties

        public bool InnerFire
        {
            get { return _innerFire; }
            set
            {
                _innerFire = value;
                OnPropertyChanged(() => InnerFire);
            }
        }

        /// <summary>
        /// Gets or sets the latency of casts.
        /// </summary>
        /// <value>The latency cast.</value>
        public float LatencyCast
        {
            get { return _latencyCast; }
            set
            {
                _latencyCast = value;
                OnPropertyChanged(() => LatencyCast);
            }
        }

        /// <summary>
        /// Gets or sets the latency of the Global Cooldown.
        /// </summary>
        /// <value>The latency global cooldown.</value>
        public float LatencyGcd
        {
            get { return _latencyGcd; }
            set
            {
                _latencyGcd = value;
                OnPropertyChanged(() => LatencyGcd);
            }
        }

        /// <summary>
        /// Gets or sets the number of targets.
        /// </summary>
        /// <value>The number of targets.</value>
        public int NumberOfTargets
        {
            get { return _numberOfTargets; }
            set
            {
                _numberOfTargets = value;
                OnPropertyChanged(() => NumberOfTargets);
            }
        }

        #endregion

        #region Event Methods

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            MemberExpression memberExpression;
            if (property.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = (UnaryExpression) property.Body;
                memberExpression = (MemberExpression) unaryExpression.Operand;
            }
            else 
                memberExpression = (MemberExpression) property.Body;

            OnPropertyChanged(memberExpression.Member.Name);
        }

        #endregion

        #region ICalculationOptionBase Members

        /// <summary>
        /// Gets the XML serialization of the calculation options for use in the character file.
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter writer = new StringWriter(stringBuilder);

            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsShadowPriest));
            serializer.Serialize(writer, this);

            return stringBuilder.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}