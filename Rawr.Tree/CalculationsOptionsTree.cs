using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using System.Xml.Serialization;


namespace Rawr.Tree
{
//    [Serializable]
    public class CalculationOptionsTree : ICalculationOptionBase, INotifyPropertyChanged
    {
//        [System.Xml.Serialization.XmlIgnore]

        private string[] _manaPotionsDesc = null;
        public string[] ManaPotionDesc
        {
            get 
            {
                if (_manaPotionsDesc == null)
                    _manaPotionsDesc = new string[] {
                        "(None) 0",
                        "(Major) 1350-2250, Avg 1800",
                        "(Mad) 1650-2750, Avg 2200",
                        "(Super) 1800-3000, Avg 2400",
                        "(Runic) 4200-4400, Avg 4300"};

                return _manaPotionsDesc;}
        }

        private int bSRatio; // goes from 0 to 100
        private int survValuePer100; // 100 Survival Points = 1 HPS (Survival Points = Health / (1-ArmorDamage Reduction)

        private int fightDuration; // In seconds
        private int rotation;
        private int manaPot;
        private int fSRRatio;
        private int replenishmentUptime;
        private int wildGrowthPerMinute;
        private int innervates;

        private int swiftmendPerMinute;

        private bool patch3_2;

        //        public int MainSpellFraction = 60;
        public RotationSettings customRotationSettings;
//        private CharacterCalculationsTree calculatedStats = null;

        public CalculationOptionsTree()
        {
            BSRatio = 75; // goes from 0 to 100
            SurvValuePer100 = 1; // 100 Survival Points = 1 HPS (Survival Points = Health / (1-ArmorDamage Reduction)
            
            FightDuration = 240; // 4 Minutes
            Rotation = 4; // default: heal 2 tanks using nourish
            ManaPot = 4; // best pot
            FSRRatio = 100;
            ReplenishmentUptime = 70;
            WildGrowthPerMinute = 3;
            //        public int MainSpellFraction = 60;
            Innervates = 1;

            SwiftmendPerMinute = 0;
            patch3_2 = true;
        }

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTree));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public int BSRatio
        {
            get { return bSRatio; }
            set { bSRatio = value; OnPropertyChanged("BSRatio"); }
        }
        public int SurvValuePer100
        {
            get { return survValuePer100; }
            set { survValuePer100 = value; OnPropertyChanged("SurvValuePer100"); }
        }


        public int FightDuration
        {
            get { return fightDuration; }
            set { fightDuration = value; OnPropertyChanged("FightDuration"); }
        }

        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; OnPropertyChanged("Rotation"); }
        }

        public int ManaPot
        {
            get { return manaPot; }
            set { manaPot = value; OnPropertyChanged("ManaPot"); }
        }

        public int FSRRatio
        {
            get { return fSRRatio; }
            set { fSRRatio = value; OnPropertyChanged("FSRRatio"); }
        }

        public int ReplenishmentUptime
        {
            get { return replenishmentUptime; }
            set { replenishmentUptime = value; OnPropertyChanged("ReplenishmentUptime"); }
        }

        public int WildGrowthPerMinute
        {
            get { return wildGrowthPerMinute; }
            set { wildGrowthPerMinute = value; OnPropertyChanged("WildGrowthPerMinute"); }
        }

        public int Innervates
        {
            get { return innervates; }
            set { innervates = value; OnPropertyChanged("Innervates"); }
        }

        public int SwiftmendPerMinute
        {
            get { return swiftmendPerMinute; }
            set { swiftmendPerMinute = value; OnPropertyChanged("SwiftmendPerMinute"); }
        }

        public bool Patch3_2
        {
            get { return patch3_2; }
            set { patch3_2 = value; OnPropertyChanged("Patch3_2"); }
        }


        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
