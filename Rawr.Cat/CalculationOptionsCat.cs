using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Cat
{
#if !SILVERLIGHT
	[Serializable]
#endif
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

		public int TargetLevel = 83;
		public int TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
		public bool CustomUseShred = false;
		public bool CustomUseRip = false;
		public bool CustomUseRake = false;
		public int CustomCPFerociousBite = 0;
		public int CustomCPSavageRoar = 2;
		public int Duration = 300;
		public float TrinketOffset = 0f;
		public int LagVariance = 200;
	}
}
