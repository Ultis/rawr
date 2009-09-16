using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Warlock
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsWarlock : ICalculationOptionBase
	{
		public bool GetGlyphByName(string name)
		{
			Type t = typeof(CalculationOptionsWarlock);
			return (bool)t.GetProperty(name).GetValue(this, null);
		}

		public void SetGlyphByName(string name, bool value)
		{
			Type t = typeof(CalculationOptionsWarlock);
			t.GetProperty(name).SetValue(this, value, null);
		}

		public String castseq { get; set; }

		public int TargetLevel { get; set; }
		public int AffEffectsNumber { get; set; }
		public float FightLength { get; set; }
		public float Delay { get; set; }
		public float Replenishment { get; set; }
		public float JoW { get; set; }
		public float LTUsePercent { get; set; }
        public float Health35Perc { get; set; }
		public String Pet { get; set; }
		public bool UseInfernal { get; set; }
		public bool UseImmoAura { get; set; }
        public bool UseDecimation { get; set; }

		public List<string> SpellPriority { get; set; }

		private static readonly List<int> targetHit = new List<int>() { 100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39 };
		public int TargetHit { get { return targetHit[TargetLevel - 80]; } }

		private static readonly List<int> manaAmt = new List<int>() { 0, 1800, 2200, 2400, 4300 };
		public int ManaPot { get; set; }
		public int ManaAmt { get { return manaAmt[ManaPot]; } }

		public CalculationOptionsWarlock()
		{
			TargetLevel = 83;
			FightLength = 5f;

			Delay = 100f;
			Replenishment = 100f;
			JoW = 100f;
            Health35Perc = 35f;
			Pet = "None";

			SpellPriority = null;
			ManaPot = 4;
		}

		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsWarlock));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml, System.Globalization.CultureInfo.InvariantCulture);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}
	}
}
