using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Warlock
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsWarlock : ICalculationOptionBase
	{
        /// <summary>
        /// The boss target level - should always be 83.
        /// </summary>
		public int TargetLevel { get; set; }
        /// <summary>
        /// The number of affliction effects (excluding your own) on the target.
        /// </summary>
		public int AffEffectsNumber { get; set; }
        /// <summary>
        /// The duration (in seconds) of the combat simulation.
        /// </summary>
		public int Duration { get; set; }
        /// <summary>
        /// The latency (in milliseconds) to be applied to spell casting during the combat simulation.
        /// </summary>
		public float Latency { get; set; }
        /// <summary>
        /// The expected uptime (percentage) that Replenishment will be active during the combat simulation.
        /// </summary>
		public float Replenishment { get; set; }
        /// <summary>
        /// The expected uptime (percentage) that Judgement Of Wisdom will be active during the combat simulation.
        /// </summary>
		public float JoW { get; set; }
        /// <summary>
        /// The pet that will be active during combat.
        /// </summary>
		public String Pet { get; set; }
        public float LTUsePercent { get; set; }
		public bool UseInfernal { get; set; }
		public bool UseImmoAura { get; set; }
        public bool UseDecimation { get; set; }
        /// <summary>
        /// Allows the user to test changes from the PTR.
        /// </summary>
        public bool PTR { get; set; }
        /// <summary>
        /// The prioritized list of spells to be used during combat.
        /// </summary>
        public List<string> SpellPriority { get; set; }
        
        [XmlIgnore]
        public List<string> WarlockSpells = new List<string> { 
            "Chaos Bolt" ,
            "Corruption", 
            "Conflagrate",
            "Curse of Agony", 
            "Curse of Doom", 
            "Death Coil", 
            "Drain Life", 
            "Drain Soul", 
            "Haunt", 
            "Hellfire", 
            "Incinerate", 
            "Immolate", 
            "Rain of Fire", 
            "Searing Pain", 
            "Seed of Corruption", 
            "Shadow Bolt", 
            "Shadowburn", 
            "Shadowflame", 
            "Shadowfury", 
            "Soul Fire", 
            "Unstable Affliction", 
        };

        [XmlIgnore]
        public String castseq { get; set; }

		private static readonly List<int> targetHit = new List<int>() { 100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39 };
		public int TargetHit { get { return targetHit[TargetLevel - 80]; } }

		private static readonly List<int> manaAmt = new List<int>() { 0, 1800, 2200, 2400, 4300 };
		public int ManaPot { get; set; }
		public int ManaAmt { get { return manaAmt[ManaPot]; } }

		public CalculationOptionsWarlock()
		{
			TargetLevel = 83;
			Duration = 300;
			Latency = 100;
            Replenishment = 100f;
            JoW = 100f;

			Pet = "None";

			ManaPot = 4;
		}

		public string GetXml()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsWarlock));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml, System.Globalization.CultureInfo.InvariantCulture);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}
	}
}
