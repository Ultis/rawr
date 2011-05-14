using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Healadin
{
    public class CalculationOptionsHealadin : ICalculationOptionBase, INotifyPropertyChanged
    {
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsHealadin));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public CalculationOptionsHealadin()
        {
            length = 7;
            activity = .85f;
            replenishment = .9f;
            divinePlea = 2f;
            boLUp = 1f;
            holyShock = .8f;
            holyPoints = 0.7f;
            lodtargets = 0.5f;
            judgementcasts = 0.8f;
            hrCasts = 0.5f;
            hrEff = 0.4f;
            burstScale = .4f;
            gHL_Targets = 1f;  // removing button for this
            infusionOfLight = true; // removing button for this
            ioLHolyLight = .9f;
            jotP = true; // removing button for this
            judgement = true;
            loHSelf = false;  // removing button for this
            hitIrrelevant = true;
        }

        private float length;
        public float Length
        {
            get { return length; }
            set { length = value; OnPropertyChanged("Length"); }
        }

        private float activity;
        public float Activity
        {
            get { return activity; }
            set { activity = value; OnPropertyChanged("Activity"); }
        }

        private float replenishment;
        public float Replenishment
        {
            get { return replenishment; }
            set { replenishment = value; OnPropertyChanged("Replenishment"); }
        }

        private float divinePlea;
        public float DivinePlea
        {
            get { return divinePlea; }
            set { divinePlea = value; OnPropertyChanged("DivinePlea"); }
        }

        private float boLUp;
        public float BoLUp
        {
            get { return boLUp; }
            set { boLUp = value; OnPropertyChanged("BoLUp"); }
        }

        private float holyShock;
        public float HolyShock
        {
            get { return holyShock; }
            set { holyShock = value; OnPropertyChanged("HolyShock"); }
        }

        private float holyPoints;
        public float HolyPoints
        {
            get { return holyPoints; }
            set { holyPoints = value; OnPropertyChanged("HolyPoints"); OnPropertyChanged("HolyPointsText"); }
        }

        public string HolyPointsText
        {
            get { return string.Format("{0:P0} LoD, {1:P0} WoG", HolyPoints, 1f - HolyPoints).Replace(" %", "%"); }
        }

        private float lodtargets;
        public float LoDTargets
        {
            get { return lodtargets; }
            set { lodtargets = value; OnPropertyChanged("LoDTargets"); }
        }

        private float judgementcasts;
        public float JudgementCasts
        {
            get { return judgementcasts; }
            set { judgementcasts = value; OnPropertyChanged("JudgementCasts"); }
        }


        private float hrEff;
        public float HREff
        {
            get { return hrEff; }
            set { hrEff = value; OnPropertyChanged("HREff"); }
        }

        private float hrCasts;
        public float HRCasts
        {
            get { return hrCasts; }
            set { hrCasts = value; OnPropertyChanged("HRCasts"); }
        }

        private float burstScale;
        public float BurstScale
        {
            get { return burstScale; }
            set {
                burstScale = value;
                OnPropertyChanged("BurstScale");
                OnPropertyChanged("BurstScaleText");
            }
        }

        public string BurstScaleText
        {
            get { return string.Format("{0:P0} Burst, {1:P0} Fight", BurstScale, 1f - BurstScale).Replace(" %", "%"); }
        }

        private float gHL_Targets;
        public float GHL_Targets
        {
            get { return gHL_Targets; }
            set { gHL_Targets = value; OnPropertyChanged("GHL_Targets"); }
        }

        private bool infusionOfLight;
        public bool InfusionOfLight
        {
            get { return infusionOfLight; }
            set { infusionOfLight = value; OnPropertyChanged("InfusionOfLight"); }
        }

        private float ioLHolyLight;
        public float IoLHolyLight
        {
            get { return ioLHolyLight; }
            set {
                ioLHolyLight = value;
                OnPropertyChanged("IoLHolyLight");
                OnPropertyChanged("IoLHolyLightText");
            }
        }

        private bool jotP;
        public bool JotP
        {
            get { return jotP; }
            set { jotP = value; OnPropertyChanged("JotP"); }
        }

        private bool judgement;
        public bool Judgement
        {
            get { return judgement; }
            set { judgement = value; OnPropertyChanged("Judgement"); }
        }

        private bool loHSelf;
        public bool LoHSelf
        {
            get { return loHSelf; }
            set { loHSelf = value; OnPropertyChanged("LoHSelf"); }
        }

        private bool hitIrrelevant;
        public bool HitIrrelevant
        {
            get { return hitIrrelevant; }
            set { hitIrrelevant = value; OnPropertyChanged("HitIrrelevant"); }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
