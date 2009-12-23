using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Tree {

    public class SpellProfile
    {
        private string name;
        private int fightDuration; // In seconds
        private int latency; // in milliseconds
        private int replenishmentUptime;
        private int wildGrowthPerMinute;
        private int innervates;
        private int swiftmendPerMinute;
        private int idleCastTimePercent; // goes from 0 to 100
        private int rejuvFrac, regrowthFrac, lifebloomFrac, nourishFrac;
        private int rejuvAmount, regrowthAmount, lifebloomStackAmount;
        private int lifebloomStackType;
        private int nourish1, nourish2, nourish3, nourish4;
        private int livingSeedEfficiency;
        private bool adjustRejuv, adjustRegrowth, adjustLifebloom, adjustNourish;
        private bool ignoreNaturesGrace, ignoreAllHasteEffects;
        private int revitalizePPM;
        private int reduceOOMRejuv, reduceOOMRegrowth, reduceOOMLifebloom, reduceOOMNourish, reduceOOMWildGrowth;
        private int reduceOOMRejuvOrder, reduceOOMRegrowthOrder, reduceOOMLifebloomOrder, reduceOOMNourishOrder, reduceOOMWildGrowthOrder;

        public SpellProfile()
        {
            name = "Custom";

            FightDuration = 300; // 5 Minutes
            Latency = 0; // milliseconds
            ReplenishmentUptime = 80;
            Innervates = 1;

            IdleCastTimePercent = 0;

            RejuvFrac = 50;
            RegrowthFrac = 0;
            LifebloomFrac = 10;
            NourishFrac = 40;
            LifebloomStackAmount = 0;
            RejuvAmount = 0;
            RegrowthAmount = 0;
            LifebloomStackType = 2;
            Nourish1 = 60;
            Nourish2 = 10;
            Nourish3 = 0;
            Nourish4 = 0;
            WildGrowthPerMinute = 4;
            SwiftmendPerMinute = 2;
            LivingSeedEfficiency = 40;
            AdjustRejuv = true;
            AdjustRegrowth = true;
            AdjustLifebloom = true;
            AdjustNourish = true;

            ReduceOOMRejuv = 40;
            ReduceOOMNourish = 80;
            ReduceOOMLifebloom = 100;
            ReduceOOMRegrowth = 100;
            ReduceOOMWildGrowth = 40;

            ReduceOOMNourishOrder = 0;
            ReduceOOMLifebloomOrder = 1;
            ReduceOOMRegrowthOrder = 2;
            ReduceOOMWildGrowthOrder = 3;
            ReduceOOMRejuvOrder = 4;

            IgnoreNaturesGrace = true;
            IgnoreAllHasteEffects = true;

            revitalizePPM = 5;
        }
        public SpellProfile Clone()
        {
            // Yes, dirty trick. Works.
            using (Stream objectStream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SpellProfile));
                serializer.Serialize(objectStream, this);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (SpellProfile)serializer.Deserialize(objectStream);
            }
        }
        public string GetXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsTree));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        public override string ToString()
        {
            return Name;
        }
        public int FightDuration { get { return fightDuration; } set { fightDuration = value; OnPropertyChanged("FightDuration"); } }
        public int Latency { get { return latency; } set { latency = value; OnPropertyChanged("Latency"); } }
        public int ReplenishmentUptime { get { return replenishmentUptime; } set { replenishmentUptime = value; OnPropertyChanged("ReplenishmentUptime" ); } }
        public int WildGrowthPerMinute { get { return wildGrowthPerMinute; } set { wildGrowthPerMinute = value; OnPropertyChanged("WildGrowthPerMinute" ); } }
        public int Innervates          { get { return innervates;          } set { innervates          = value; OnPropertyChanged("Innervates"          ); } }
        public int SwiftmendPerMinute  { get { return swiftmendPerMinute;  } set { swiftmendPerMinute  = value; OnPropertyChanged("SwiftmendPerMinute"  ); } }
        public int IdleCastTimePercent { get { return idleCastTimePercent; } set { idleCastTimePercent = value; OnPropertyChanged("IdleCastTimePercent"); } }

        public int RejuvFrac { get { return rejuvFrac; } set { rejuvFrac = value; OnPropertyChanged("RejuvFrac"); } }
        public int RegrowthFrac { get { return regrowthFrac; } set { regrowthFrac = value; OnPropertyChanged("RegrowthFrac"); } }
        public int LifebloomFrac { get { return lifebloomFrac; } set { lifebloomFrac = value; OnPropertyChanged("LifebloomFrac"); } }
        public int NourishFrac { get { return nourishFrac; } set { nourishFrac = value; OnPropertyChanged("NourishFrac"); } }
        public int LifebloomStackAmount { get { return lifebloomStackAmount; } set { lifebloomStackAmount = value; OnPropertyChanged("AverageLifebloomStack"); } }
        public int RejuvAmount { get { return rejuvAmount; } set { rejuvAmount = value; OnPropertyChanged("RejuvAmount"); } }
        public int RegrowthAmount { get { return regrowthAmount; } set { regrowthAmount = value; OnPropertyChanged("RegrowthAmount"); } }
        public int LifebloomStackType { get { return lifebloomStackType; } set { lifebloomStackType = value; OnPropertyChanged("LifebloomStackType"); } }

        public int Nourish1 { get { return nourish1; } set { nourish1 = value; OnPropertyChanged("Nourish1"); } }
        public int Nourish2 { get { return nourish2; } set { nourish2 = value; OnPropertyChanged("Nourish2"); } }
        public int Nourish3 { get { return nourish3; } set { nourish3 = value; OnPropertyChanged("Nourish3"); } }
        public int Nourish4 { get { return nourish4; } set { nourish4 = value; OnPropertyChanged("Nourish4"); } }

        public bool AdjustRejuv { get { return adjustRejuv; } set { adjustRejuv = value; OnPropertyChanged("AdjustRejuv"); } }
        public bool AdjustRegrowth { get { return adjustRegrowth; } set { adjustRegrowth = value; OnPropertyChanged("AdjustRegrowth"); } }
        public bool AdjustLifebloom { get { return adjustLifebloom; } set { adjustLifebloom = value; OnPropertyChanged("AdjustLifebloom"); } }
        public bool AdjustNourish { get { return adjustNourish; } set { adjustNourish = value; OnPropertyChanged("AdjustNourish"); } }

        public int ReduceOOMRejuv { get { return reduceOOMRejuv; } set { reduceOOMRejuv = value; OnPropertyChanged("ReduceOOMRejuv"); } }
        public int ReduceOOMRegrowth { get { return reduceOOMRegrowth; } set { reduceOOMRegrowth = value; OnPropertyChanged("ReduceOOMRegrowth"); } }
        public int ReduceOOMLifebloom { get { return reduceOOMLifebloom; } set { reduceOOMLifebloom = value; OnPropertyChanged("ReduceOOMLifebloom"); } }
        public int ReduceOOMNourish { get { return reduceOOMNourish; } set { reduceOOMNourish = value; OnPropertyChanged("ReduceOOMNourish"); } }
        public int ReduceOOMWildGrowth { get { return reduceOOMWildGrowth; } set { reduceOOMWildGrowth = value; OnPropertyChanged("ReduceOOMWildGrowth"); } }

        public int ReduceOOMRejuvOrder { get { return reduceOOMRejuvOrder; } set { reduceOOMRejuvOrder = value; OnPropertyChanged("ReduceOOMRejuvOrder"); } }
        public int ReduceOOMRegrowthOrder { get { return reduceOOMRegrowthOrder; } set { reduceOOMRegrowthOrder = value; OnPropertyChanged("ReduceOOMRegrowthOrder"); } }
        public int ReduceOOMLifebloomOrder { get { return reduceOOMLifebloomOrder; } set { reduceOOMLifebloomOrder = value; OnPropertyChanged("ReduceOOMLifebloomOrder"); } }
        public int ReduceOOMNourishOrder { get { return reduceOOMNourishOrder; } set { reduceOOMNourishOrder = value; OnPropertyChanged("ReduceOOMNourishOrder"); } }
        public int ReduceOOMWildGrowthOrder { get { return reduceOOMWildGrowthOrder; } set { reduceOOMWildGrowthOrder = value; OnPropertyChanged("ReduceOOMWildGrowthOrder"); } }

        public bool IgnoreNaturesGrace { get { return ignoreNaturesGrace; } set { ignoreNaturesGrace = value; OnPropertyChanged("IgnoreNaturesGrace"); } }
        public bool IgnoreAllHasteEffects { get { return ignoreAllHasteEffects; } set { ignoreAllHasteEffects = value; OnPropertyChanged("IgnoreAllHasteEffects"); } }

        public int LivingSeedEfficiency { get { return livingSeedEfficiency; } set { livingSeedEfficiency = value; OnPropertyChanged("LivingSeedEfficiency"); } }
        public int RevitalizePPM { get { return revitalizePPM; } set { revitalizePPM = value; OnPropertyChanged("RevitalizePPM"); } }
        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        
        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string name) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name)); }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }

    public class CalculationOptionsTree : ICalculationOptionBase, INotifyPropertyChanged {
        private int singleTarget;
        private int sustainedTarget;
        private int survValuePer100; // 100 Survival Points = 1 HPS (Survival Points = Health / (1-ArmorDamage Reduction)
        private int singleTargetRotation;
        private List<SpellProfile> profiles;
        private SpellProfile current;

        public CalculationOptionsTree() {
            SurvValuePer100 = 1; 

            singleTarget = 9000;
            sustainedTarget = 8500;
            
            singleTargetRotation = 0;

            current = new SpellProfile();
            profiles = new List<SpellProfile>();
            profiles.Add(current);
            current.Name = "Default";
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
        [XmlArray("Profiles")]
        public List<SpellProfile> Profiles { get { return profiles; } set { profiles = value; OnPropertyChanged("Profiles"); } }
        public SpellProfile Current { get { return current; } set { current = value; OnPropertyChanged("Current"); } }

        public int SingleTargetRotation { get { return singleTargetRotation; } set { singleTargetRotation = value; OnPropertyChanged("SingleTargetRotation"); } }

        #region INotifyPropertyChanged Members
        private void OnPropertyChanged(string name) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name)); }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
