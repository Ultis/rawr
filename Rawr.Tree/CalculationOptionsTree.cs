using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Tree {
    //[Serializable]
    public class CalculationOptionsTree : ICalculationOptionBase, INotifyPropertyChanged {
        //[System.Xml.Serialization.XmlIgnore]
        private string[] _manaPotionsDesc = null;
        public string[] ManaPotionDesc {
            get {
                if (_manaPotionsDesc == null) {
                    _manaPotionsDesc = new string[] {
                        "(None) 0",
                        "(Major) 1350-2250, Avg 1800",
                        "(Mad) 1650-2750, Avg 2200",
                        "(Super) 1800-3000, Avg 2400",
                        "(Runic) 4200-4400, Avg 4300"};
                }
                return _manaPotionsDesc;}
        }
        //private int bSRatio; // goes from 0 to 100
        private int singleTarget;
        private int sustainedTarget;
        private int survValuePer100; // 100 Survival Points = 1 HPS (Survival Points = Health / (1-ArmorDamage Reduction)
        private int fightDuration; // In seconds
        private int manaPot;
        private int fSRRatio;
        private int replenishmentUptime;
        private int wildGrowthPerMinute;
        private int innervates;
        private int swiftmendPerMinute;
        private int idleCastTimePercent; // goes from 0 to 100
        private int avgRejuv, avgRegrowths, avgLifebloom, avgLifebloomStack;
        private int primaryHeal;
        private int lifebloomStackType;
        private int nourish1, nourish2, nourish3, nourish4;
        private int livingSeedEfficiency;
        private int singleTargetRotation;

        //private CharacterCalculationsTree calculatedStats = null;
        public CalculationOptionsTree() {
            SurvValuePer100 = 1; // 100 Survival Points = 1 HPS (Survival Points = Health / (1-ArmorDamage Reduction)

            singleTarget = 9000;
            sustainedTarget = 8500;
            
            FightDuration = 240; // 4 Minutes
            ManaPot = 4; // best pot
            FSRRatio = 100;
            ReplenishmentUptime = 90;
            Innervates = 1;

            IdleCastTimePercent = 0;

            AverageRejuv = 40;
            AverageRegrowths = 0;
            AverageLifebloom = 20;
            AverageLifebloomStack = 0;
            PrimaryHeal = 0;
            LifebloomStackType = 2;
            Nourish1 = 40;
            Nourish2 = 20;
            Nourish3 = 0;
            Nourish4 = 0;
            WildGrowthPerMinute = 2;
            SwiftmendPerMinute = 2;
            livingSeedEfficiency = 50;

            singleTargetRotation = 0;
        }
        public string GetXml() {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsTree));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        public int SingleTarget { get { return singleTarget; } set { singleTarget = value; OnPropertyChanged("SingleTarget"); } }
        public int SustainedTarget { get { return sustainedTarget; } set { sustainedTarget = value; OnPropertyChanged("SustainedTarget"); } }
        public int SurvValuePer100 { get { return survValuePer100; } set { survValuePer100 = value; OnPropertyChanged("SurvValuePer100"); } }
        public int FightDuration { get { return fightDuration; } set { fightDuration = value; OnPropertyChanged("FightDuration"); } }
        public int ManaPot             { get { return manaPot;             } set { manaPot             = value; OnPropertyChanged("ManaPot"             ); } }
        public int FSRRatio            { get { return fSRRatio;            } set { fSRRatio            = value; OnPropertyChanged("FSRRatio"            ); } }
        public int ReplenishmentUptime { get { return replenishmentUptime; } set { replenishmentUptime = value; OnPropertyChanged("ReplenishmentUptime" ); } }
        public int WildGrowthPerMinute { get { return wildGrowthPerMinute; } set { wildGrowthPerMinute = value; OnPropertyChanged("WildGrowthPerMinute" ); } }
        public int Innervates          { get { return innervates;          } set { innervates          = value; OnPropertyChanged("Innervates"          ); } }
        public int SwiftmendPerMinute  { get { return swiftmendPerMinute;  } set { swiftmendPerMinute  = value; OnPropertyChanged("SwiftmendPerMinute"  ); } }
        public int IdleCastTimePercent { get { return idleCastTimePercent; } set { idleCastTimePercent = value; OnPropertyChanged("IdleCastTimePercent"); } }

        public int AverageRejuv { get { return avgRejuv; } set { avgRejuv = value; OnPropertyChanged("AverageRejuv"); } }
        public int AverageRegrowths { get { return avgRegrowths; } set { avgRegrowths = value; OnPropertyChanged("AverageRegrowths"); } }
        public int AverageLifebloom { get { return avgLifebloom; } set { avgLifebloom = value; OnPropertyChanged("AverageLifebloom"); } }
        public int AverageLifebloomStack { get { return avgLifebloomStack; } set { avgLifebloomStack = value; OnPropertyChanged("AverageLifebloomStack"); } }
        public int PrimaryHeal { get { return primaryHeal; } set { primaryHeal = value; OnPropertyChanged("PrimaryHeal"); } }
        public int LifebloomStackType { get { return lifebloomStackType; } set { lifebloomStackType = value; OnPropertyChanged("LifebloomStackType"); } }

        public int SingleTargetRotation { get { return singleTargetRotation; } set { singleTargetRotation = value; OnPropertyChanged("SingleTargetRotation"); } }

        public int Nourish1 { get { return nourish1; } set { nourish1 = value; OnPropertyChanged("Nourish1"); } }
        public int Nourish2 { get { return nourish2; } set { nourish2 = value; OnPropertyChanged("Nourish2"); } }
        public int Nourish3 { get { return nourish3; } set { nourish3 = value; OnPropertyChanged("Nourish3"); } }
        public int Nourish4 { get { return nourish4; } set { nourish4 = value; OnPropertyChanged("Nourish4"); } }

        public int LivingSeedEfficiency { get { return livingSeedEfficiency; } set { livingSeedEfficiency = value; OnPropertyChanged("LivingSeedEfficiency"); } }

        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string name) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name)); }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
