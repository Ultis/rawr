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
            length = 7;            // fight length in minutes
            activity = .85f;       // percent of fight time active
            replenishment = .9f;   // percent of fight lenght with this buff
            divinePlea = 2f;       // time in minutes between Divine Plea casts
            cleanse = 10f;         // number of cleanse casts during fight
            melee = 0.25f;         // percent of instant cast time total spent doing melee
            boLUp = 1f;
            holyShock = 7.5f;
            holyPoints = 0.75f;
            lodtargets = 0.6f;
            judgementcasts = 10f;
            hrCasts = 60f;
            hrEff = 0.5f;          
            burstScale = .3f;    // FoL spam vs total fight HPS
            judgement = true;
            hitIrrelevant = true;
            meleevsHL = false;
            userdelay = 0.1f;
            ihEff = 1f;         // todo:  get rid of these
            critoverheals = 0f; // todo:  get rid of these
         //   gHL_Targets = 1f;  // removing button for this
         //   infusionOfLight = true; // removing button for this
         //   ioLHolyLight = .9f;
         //   jotP = true; // removing button for this
         //   loHSelf = false;  // removing button for this
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

        private float critoverheals;
        public float CritOverheals
        {
            get { return critoverheals; }
            set { critoverheals = value; OnPropertyChanged("CritOverheals"); }
        }

        private float melee;
        public float Melee
        {
            get { return melee; }
            set { melee = value; OnPropertyChanged("Melee"); }
        }

        private float cleanse;
        public float Cleanse
        {
            get { return cleanse; }
            set { cleanse = value; OnPropertyChanged("Cleanse"); }
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
            set { holyShock = value; OnPropertyChanged("HolyShock"); /* OnPropertyChanged("HolyShockText"); */ }
        }


        private float holyPoints; // this tracks how they spend holy points, WoG vs LoD
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
            set { judgementcasts = value; OnPropertyChanged("JudgementCasts"); /* OnPropertyChanged("JudgementCastsText"); */ }
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
            set { hrCasts = value; OnPropertyChanged("HRCasts"); /* OnPropertyChanged("HRCastsText"); */ }
        }

        public string HRCastsText
        {
            get { return string.Format("Cast HR every {0} seconds", (30f / HRCasts).ToString("N01")); }
        }

        private float ihEff;
        public float IHEff
        {
            get { return ihEff; }
            set { ihEff = value; OnPropertyChanged("IHEff"); }
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
            get { return string.Format("{0:P0} Burst, {1:P0} Total Fight", BurstScale, 1f - BurstScale).Replace(" %", "%"); }
        }

        private float userdelay;
        public float Userdelay
        {
            get { return userdelay; }
            set { userdelay = value; OnPropertyChanged("Userdelay"); }
        }


        private bool judgement;
        public bool Judgement
        {
            get { return judgement; }
            set { judgement = value; OnPropertyChanged("Judgement"); }
        }


        private bool hitIrrelevant;
        public bool HitIrrelevant
        {
            get { return hitIrrelevant; }
            set { hitIrrelevant = value; OnPropertyChanged("HitIrrelevant"); }
        }

        private bool meleevsHL;
        public bool MeleevsHL
        {
            get { return meleevsHL; }
            set { meleevsHL = value; OnPropertyChanged("MeleevsHL"); }
        }

        /* stuff I want to delete, putting in comment block in case I still need it
         * Molotok 5/27/11
         * 
         *  
        private bool jotP;
        public bool JotP
        {
            get { return jotP; }
            set { jotP = value; OnPropertyChanged("JotP"); }
        }
        private bool loHSelf;
        public bool LoHSelf
        {
            get { return loHSelf; }
            set { loHSelf = value; OnPropertyChanged("LoHSelf"); }
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
        public string JudgementCastsText
        {
            get { return string.Format("Cast Judgement every {0} seconds", (8f / JudgementCasts).ToString("N01")); }
        }
        
        private float gHL_Targets;
        public float GHL_Targets
        {
            get { return gHL_Targets; }
            set { gHL_Targets = value; OnPropertyChanged("GHL_Targets"); }
        }
        
        public string HolyShockText
        {
            get { return string.Format("Cast HS every {0} seconds", (6f / HolyShock).ToString("N01")); }
        }
         * 
         * 
         * 
         * 
         */


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
