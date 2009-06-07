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
			TargetArmor = (int)StatConversion.NPC_BOSS_ARMOR;
			CustomUseShred = false;
			CustomUseRip = false;
			CustomUseRake = false;
			CustomUseFerociousBite = false;
			CustomCPSavageRoar = 2;
			Duration = 300;
		}

		public int TargetLevel { get; set; }
		public int TargetArmor { get; set; }
		public bool CustomUseShred { get; set; }
		public bool CustomUseRip { get; set; }
		public bool CustomUseRake { get; set; }
		public bool CustomUseFerociousBite { get; set; }
		public int CustomCPSavageRoar { get; set; }
		public int Duration { get; set; }
	}
}
