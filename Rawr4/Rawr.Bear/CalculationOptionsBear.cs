using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Bear
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsBear : ICalculationOptionBase, INotifyPropertyChanged
	{
		public string GetXml()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsBear));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		#region Rating Customization
		private float _threatScale = 5f;
		public float ThreatScale
		{
			get { return _threatScale; }
			set { if (_threatScale != value) { _threatScale = value; OnPropertyChanged("ThreatScale"); } }
		}
		private int _survivalSoftCap = 450000;
		public int SurvivalSoftCap
		{
			get { return _survivalSoftCap; }
			set { if (_survivalSoftCap != value) { _survivalSoftCap = value; OnPropertyChanged("SurvivalSoftCap"); } }
		}
		private float _temporarySurvivalScale = 1f;
		public float TemporarySurvivalScale
		{
			get { return _temporarySurvivalScale; }
			set { if (_temporarySurvivalScale != value) { _temporarySurvivalScale = value; OnPropertyChanged("TemporarySurvivalScale"); } }
		}
		#endregion

		#region Target Parameters
		private int _targetDamage = 150000;
		public int TargetDamage
		{
			get { return _targetDamage; }
			set { if (_targetDamage != value) { _targetDamage = value; OnPropertyChanged("TargetDamage"); } }
		}
		private float _targetAttackSpeed = 2.0f;
		public float TargetAttackSpeed
		{
			get { return _targetAttackSpeed; }
			set { if (_targetAttackSpeed != value) { _targetAttackSpeed = value; OnPropertyChanged("TargetAttackSpeed"); } }
		}
		#endregion

		#region INotifyPropertyChanged Members
		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
		}
		#endregion
	}
}
