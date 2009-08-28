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

namespace Rawr.TankDK
{
    public class CalculationOptionsTankDK : ICalculationOptionBase
    {
        public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTankDK));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

        public enum Presence
        {
            Blood, Unholy
        }

        // Tank DK values.
        public float BossAttackSpeed = 2.5f;
        public float IncomingDamage = 10000f;
        public float PercentIncomingFromMagic = 0f;
        public float SurvivalWeight = 1f;
        public float ThreatWeight = 1f;
        public Rotation m_Rotation = new Rotation();
        public uint uNumberTargets = 1;

        public int TargetLevel = 83;
        public int BossArmor = (int)StatConversion.NPC_ARMOR[83 - 80];
		public float FightLength = 10;
		public bool EnforceMetagemRequirements = false;
        public bool Bloodlust = false;
        public bool DrumsOfBattle = false;
        public bool DrumsOfWar = false;
        public int FerociousInspiration = 1;
        public bool HammerOfWrath = false;
        public bool Ghoul = false;
        public float BloodwormsUptime = 0.25f;
        public float GhoulUptime = 1f;
        public Presence presence = Presence.Blood;

	    public DeathKnightTalents talents = null;
	    public Rotation rotation;

		public bool TalentsSaved = false;
	}
}