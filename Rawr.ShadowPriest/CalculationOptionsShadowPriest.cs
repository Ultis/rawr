using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ShadowPriest
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsShadowPriest : ICalculationOptionBase
	{
		public int TargetLevel { get; set; }
		public float FightLength { get; set; }
		public float FSRRatio { get; set; }
		public float Delay { get; set; }
		public float Shadowfiend { get; set; }
		public float Replenishment { get; set; }
		public float JoW { get; set; }
		public float Survivability { get; set; }

		public List<string> SpellPriority { get; set; }

		private static readonly List<int> targetHit = new List<int>() { 100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39 };
		public int TargetHit { get { return targetHit[TargetLevel]; } }

		private static readonly List<int> manaAmt = new List<int>() { 0, 1800, 2200, 2400, 4300 };
		public int ManaPot { get; set; }
		public int ManaAmt { get { return manaAmt[ManaPot]; } }

		public CalculationOptionsShadowPriest()
		{
			TargetLevel = 3;
			FightLength = 6f;
			FSRRatio = 100f;
			Delay = 350f;
			Shadowfiend = 100f;
			Replenishment = 100f;
			JoW = 100f;
			Survivability = 2f;

			SpellPriority = null;
			ManaPot = 4;
		}

		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsShadowPriest));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}
	}
}
