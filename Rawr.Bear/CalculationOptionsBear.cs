using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Bear
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsBear : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsBear));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
		public float ThreatScale = 10f;
		public int TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
		public int SurvivalSoftCap = 160000;
		public int TargetDamage = 65000;
		public float TargetAttackSpeed = 2.0f;

		public bool? CustomUseMaul = null;
		public bool CustomUseMangle = false;
		public bool CustomUseSwipe = false;
		public bool CustomUseFaerieFire = false;
		public bool CustomUseLacerate = false;
	}
}
