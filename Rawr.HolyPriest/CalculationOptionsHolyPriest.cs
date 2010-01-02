using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.HolyPriest
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsHolyPriest : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsHolyPriest));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		private static readonly List<int> manaAmt = new List<int>() { 0, 1800, 2200, 2400, 4300 };
		public int ManaPot = 4;
		public int ManaAmt { get { return manaAmt[ManaPot]; } }
		public enum eRole
		{
			AUTO_Tank, AUTO_Raid, Greater_Heal, Flash_Heal, CoH_PoH, Holy_Tank, Holy_Raid,
			Disc_Tank_GH, Disc_Tank_FH, Disc_Raid, CUSTOM, Holy_Raid_Renew
		};
		public eRole Role = 0;
		public int Rotation = 0;    // LEGACY
		public float FSRRatio = 93f;
		public float FightLengthSeconds = 480f;
		public float Serendipity = 75f;
		public float Replenishment = 75f;
		public float Shadowfiend = 100f;
		public float Survivability = 2f;
		public float Rapture = 25f;
		public float TestOfFaith = 25f;
		public bool ModelProcs = true;

		public int FlashHealCast = 0;
		public int BindingHealCast = 0;
		public int GreaterHealCast = 0;
		public int PenanceCast = 0;
		public int RenewCast = 0;
		public int RenewTicks = 0;
		public int ProMCast = 0;
		public int ProMTicks = 0;
		public int PoHCast = 0;
		public int PWSCast = 0;
		public int CoHCast = 0;
		public int HolyNovaCast = 0;
		public int DivineHymnCast = 0;
		public int DispelCast = 0;
		public int MDCast = 0;
	}
}
