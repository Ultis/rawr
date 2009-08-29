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

namespace Rawr.Cat
{
	public class CalculationOptionsCat : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsCat));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public CalculationOptionsCat()
		{
			TargetLevel = 83;
            TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
			CustomUseShred = false;
			CustomUseRip = false;
			CustomUseRake = false;
			CustomCPFerociousBite = 0;
			CustomCPSavageRoar = 2;
			Duration = 300;
		}

		public int TargetLevel = 83;
        public int TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
		public bool CustomUseShred = false;
		public bool CustomUseRip = false;
		public bool CustomUseRake = false;
		public int CustomCPFerociousBite = 0;
		public int CustomCPSavageRoar = 2;
		public int Duration = 300;
        public bool OffsetTrinkets = false;
	}
}
