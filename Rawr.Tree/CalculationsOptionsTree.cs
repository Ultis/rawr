using System;
using System.Collections.Generic;
using System.Text;

using System.Xml.Serialization;


namespace Rawr.Tree
{
//    [Serializable]
    public class CalculationOptionsTree : ICalculationOptionBase
    {
        //I want the calculated Stats in the SpellrotationsEditor .. so I trade them over the Options
//        [System.Xml.Serialization.XmlIgnore]
        public CharacterCalculationsTree calculatedStats = null;

        public int BSRatio = 75; // goes from 0 to 100

        public int FightDuration = 240; // 4 Minutes
        public int Rotation = 4; // default: heal 2 tanks using nourish
        public int ManaPot = 4; // best pot
        public int FSRRatio = 100;
        public int ReplenishmentUptime = 70;
        public int WildGrowthPerMinute = 3;
        //        public int MainSpellFraction = 60;
        public int Innervates = 1;

        public int SwiftmendPerMinute = 0;

        public bool PenalizeEverything = false;

        public bool glyphOfHealingTouch = false;
        public bool glyphOfRegrowth = false;
        public bool glyphOfRejuvenation = false;

        public bool glyphOfLifebloom = true;
        public bool glyphOfInnervate = true;
        public bool glyphOfSwiftmend = true;

        public bool patch3_2 = false;

        public RotationSettings customRotationSettings;

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTree));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
    }
}
