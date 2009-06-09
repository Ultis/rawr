using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;

namespace Rawr.DPSWarr
{

    public class CalculationOptionsDPSWarr : ICalculationOptionBase
    {
        public int TargetLevel = 83;
        public int TargetArmor = 10643;
        public float Duration = 300;
        public bool FuryStance = true;
        public bool MultipleTargets = false; public int MultipleTargetsPerc = 0;
        public bool MovingTargets = false; public int MovingTargetsPerc = 0;
        public bool StunningTargets = false; public int StunningTargetsPerc = 0;
		public bool DisarmingTargets = false; public int DisarmingTargetsPerc = 0;
		public bool Mntn_Thunder = false;
		public bool Mntn_Sunder = false;
		public bool Mntn_Battle = false;
		public bool Mntn_Demo = false;
		public bool Mntn_Hamstring = false;
        public WarriorTalents talents = null;
        public string GetXml()
        {
            var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSWarr));
            var xml = new StringBuilder();
            var sw = new System.IO.StringWriter(xml);
            s.Serialize(sw, this);
            return xml.ToString();
        }
    }
}