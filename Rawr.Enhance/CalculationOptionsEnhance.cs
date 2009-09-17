using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Enhance
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsEnhance : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsEnhance));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
		public int TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
		public int AverageLag = 250;
		public string MainhandImbue = "Windfury";
		public string OffhandImbue = "Flametongue";
		public float FightLength = 10.0f;
		public int TargetFireResistance = 0;
		public int TargetNatureResistance = 0;
		public bool Magma = true;
		public bool BaseStatOption = true;
	}
}
