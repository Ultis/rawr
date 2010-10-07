using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.DPSDK
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsDPSDK : ICalculationOptionBase, INotifyPropertyChanged
	{
		public string GetXml()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsDPSDK));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public enum Presence
		{
			None = 0,
            Blood, 
            Unholy, 
            Frost
		}
		
		private float _GhoulUptime = 1f;
		public float GhoulUptime
		{
			get { return _GhoulUptime; }
			set { _GhoulUptime = value; OnPropertyChanged("GhoulUptime"); }
		}

        private Presence _Presence = Presence.None;
        public Presence CurrentPresence
        {
            get { return _Presence; }
            set { _Presence = value; }
        }
		
		private float _KMProcUsage = 1f;
		public float KMProcUsage
		{
			get { return _KMProcUsage; }
			set { _KMProcUsage = value; OnPropertyChanged("KMProcUsage"); }
		}
		
		private float _BloodwormsUptime = 0.25f;
		public float BloodwormsUptime
		{
			get { return _BloodwormsUptime; }
			set { _BloodwormsUptime = value; OnPropertyChanged("BloodwormsUptime"); }
		}
		
		private bool _Ghoul = true;
		public bool Ghoul
		{
			get { return _Ghoul; }
			set { _Ghoul = value; OnPropertyChanged("Ghoul"); }
		}
		
        private bool _getRefreshForReferenceCalcs = true;
        public bool GetRefreshForReferenceCalcs
        {
            get { return _getRefreshForReferenceCalcs; }
            set { _getRefreshForReferenceCalcs = value; }
        }

        private bool _getRefreshForDisplayCalcs = true;
        public bool GetRefreshForDisplayCalcs
        {
            get { return _getRefreshForDisplayCalcs; }
            set { _getRefreshForDisplayCalcs = value; }
        }

        private bool _getRefreshForSignificantChange = false;
        public bool GetRefreshForSignificantChange
        {
            get { return _getRefreshForSignificantChange; }
            set { _getRefreshForSignificantChange = value; }
        }

        private bool _m_bExperimental = false;
        public bool m_bExperimental
        {
            get { return _m_bExperimental; }
            set { _m_bExperimental = value; OnPropertyChanged("m_bExperimental"); }
        }

		private Rotation _rotation = null;
		public Rotation rotation
		{
            get { if (_rotation == null) _rotation = new Rotation(); return _rotation; }
            set
            {
                if (_rotation == null) 
                    _rotation = new Rotation();
                _rotation = value; OnPropertyChanged("rotation"); 
            }
		}
		/*
		private bool _TalentsSaved = false;
		public bool TalentsSaved
		{
			get { return _TalentsSaved; }
			set { _TalentsSaved = value; OnPropertyChanged("TalentsSaved"); }
		}

        private double _weightScale = .01;
        public double WeightScale
        {
            get { return _weightScale; }
            set { _weightScale = value; }
        }
        */
		#region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        }
        #endregion
	}
}
