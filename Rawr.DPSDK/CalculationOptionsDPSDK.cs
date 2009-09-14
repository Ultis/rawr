using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSDK
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsDPSDK : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsDPSDK));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public enum Presence
		{
			Blood, Unholy
		}

		public int TargetLevel = 83;
		public int BossArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
		public int FightLength = 10;
		public bool EnforceMetagemRequirements = false;
		public bool Bloodlust = false;
		public bool DrumsOfBattle = false;
		public bool DrumsOfWar = false;
		public int FerociousInspiration = 1;
		public bool Ghoul = false;
		public float BloodwormsUptime = 0.25f;
		public float KMProcUsage = 1f;
		public float GhoulUptime = 1f;
		public Presence presence = Presence.Blood;

		public DeathKnightTalents talents = null;
		public Rotation rotation;

		public bool TalentsSaved = false;
	}
}
