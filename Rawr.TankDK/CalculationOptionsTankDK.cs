using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsTankDK : ICalculationOptionBase
	{
		public enum Presence { Blood, Frost, Unholy }

		public int TargetLevel = 83;
		public float ThreatWeight = 1.00f;
		public float SurvivalWeight = 1.00f;
		public uint IncomingDamage = 10000;
		public float PercentIncomingFromMagic = .0f;
		public float BossAttackSpeed = 2.5f;
		public float BossArmor = StatConversion.NPC_ARMOR[83 - 80];

		public bool Bloodlust = false;
		public float FightLength = 10f;
		public uint uNumberTargets = 1;
		public Rotation m_Rotation = new Rotation();
		public DeathKnightTalents talents;

		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTankDK));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

	}
}
