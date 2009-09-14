using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsProtPaladin : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsProtPaladin));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
		public int TargetArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
		public int BossAttackValue = 50000;
		public float BossAttackSpeed = 2.0f;

		public int BossAttackValueMagic = 8000;
		public float BossAttackSpeedMagic = 1.0f;
		public bool UseParryHaste = false;
		public float ThreatScale = 10.0f;
		public float MitigationScale = 17000f;
		public int RankingMode = 1;
		public bool UseHolyShield = true;
		public string SealChoice = "Seal of Vengeance";
		public string TargetType = "Unspecified";
		public string MagicDamageType = "None";
		public string TrinketOnUseHandling = "Ignore";
		public PaladinTalents talents = null;
		public bool PTRMode = false;
	}
}
