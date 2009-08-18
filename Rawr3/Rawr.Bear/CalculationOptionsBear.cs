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
using System.Xml.Serialization;
using System.Text;

namespace Rawr.Bear
{
	public class CalculationOptionsBear : ICalculationOptionBase
	{
		public string GetXml()
		{
			XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsBear));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public CalculationOptionsBear()
		{
			TargetLevel = 83;
			ThreatScale = 10f;
            TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
			SurvivalSoftCap = 140000;
			TargetDamage = 50000;
			TargetAttackSpeed = 2.0f;
		}

		public int TargetLevel { get; set; }
		public float ThreatScale { get; set; }
		public int TargetArmor { get; set; }
		public int SurvivalSoftCap { get; set; }
		public int TargetDamage { get; set; }
		public float TargetAttackSpeed { get; set; }

		public bool? CustomUseMaul { get; set; }
		public bool CustomUseMangle { get; set; }
		public bool CustomUseSwipe { get; set; }
		public bool CustomUseFaerieFire { get; set; }
		public bool CustomUseLacerate { get; set; }
	}
}
