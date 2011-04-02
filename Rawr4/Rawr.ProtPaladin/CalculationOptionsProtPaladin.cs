using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Data;
using System.Xml.Serialization;

namespace Rawr.ProtPaladin
{
    public class ThreatValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            float threatValue = (float)value;
            if (threatValue == 1f) return "Almost None";
            if (threatValue == 5f) return "MT";
            if (threatValue == 25f) return "OT";
            if (threatValue == 50f) return "Crazy About Threat";
            else return "Custom...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string threatValue = (string)value;
            switch (threatValue)
            {
                case "Almost None": return 1f;
                case "MT": return 5f;
                case "OT": return 25f;
                case "Crazy About Threat": return 50f;
            }
            return null;
        }

        #endregion
    }
    public class SurvivalSoftCapConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int survivalSoftCap = (int)value;
            if (survivalSoftCap == 50000 * 3 * 1.25) return "Normal Dungeons";
            if (survivalSoftCap == 80000 * 3 * 1.25) return "Heroic Dungeons";
            if (survivalSoftCap == 150000 * 3 * 1.25) return "Normal T11 Raids";
            if (survivalSoftCap == 175000 * 3 * 1.25) return "Heroic T11 Raids";
            else return "Custom...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string survivalSoftCap = (string)value;
            switch (survivalSoftCap)
            {
                case "Normal Dungeons": return 50000 * 3 * 1.25;
                case "Heroic Dungeons": return 80000 * 3 * 1.25;
                case "Normal T11 Raids": return 150000 * 3 * 1.25;
                case "Heroic T11 Raids": return 175000 * 3 * 1.25;
            }
            return null;
        }

        #endregion
    }


#if !SILVERLIGHT
    [Serializable]
#endif
    public class CalculationOptionsProtPaladin : ICalculationOptionBase, INotifyPropertyChanged {

        private float _ThreatScale = 5.0f;
        public float ThreatScale
        {
            get { return _ThreatScale; }
            set { _ThreatScale = value; OnPropertyChanged("ThreatScale"); }
        }

        private float _MitigationScale = 0.125f;
        public float MitigationScale
        {
            get { return _MitigationScale; }
            set { _MitigationScale = value; OnPropertyChanged("MitigationScale"); }
        }

        private int _RankingMode = 0;
        public int RankingMode
        {
            get { return _RankingMode; }
            set { _RankingMode = value; OnPropertyChanged("RankingMode"); }
        }

        private string _SealChoice = "Seal of Truth";
        public string SealChoice
        {
            get { return _SealChoice; }
            set { _SealChoice = value; OnPropertyChanged("SealChoice"); }
        }

        private string _MagicDamageType = "None";
        public string MagicDamageType
        {
            get { return _MagicDamageType; }
            set { _MagicDamageType = value; OnPropertyChanged("MagicDamageType"); }
        }

        private string _TrinketOnUseHandling = "Ignore";
        public string TrinketOnUseHandling
        {
            get { return _TrinketOnUseHandling; }
            set { _TrinketOnUseHandling = value; OnPropertyChanged("TrinketOnUseHandling"); }
        }

        private PaladinTalents _talents = null;
        public PaladinTalents talents
        {
            get { return _talents; }
            set { _talents = value; OnPropertyChanged("talents"); }
        }

        private bool _PTRMode = false;
        public bool PTRMode
        {
            get { return _PTRMode; }
            set { _PTRMode = value; OnPropertyChanged("PTRMode"); }
        }

        private int _survivalSoftCap = 300000;
        public int SurvivalSoftCap
        {
            get { return _survivalSoftCap; }
            set { _survivalSoftCap = value; OnPropertyChanged("SurvivalSoftCap"); }
        }

        private string _mainAttack = "Crusader Strike";
        public string MainAttack
        {
            get { return _mainAttack; }
            set { _mainAttack = value; OnPropertyChanged("MainAttack"); }
        }

        private string _priority = "AS > HW";
        public string Priority
        {
            get { return _priority; }
            set { _priority = value; OnPropertyChanged("Priority"); }
        }

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
