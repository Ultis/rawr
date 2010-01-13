using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.RestoSham
{
	#region Calculations, do not edit.
	class NumericField
	{
		public NumericField(string szName, float min, float max, bool bzero)
		{
			PropertyName = szName;
			MinValue = min;
			MaxValue = max;
			CanBeZero = bzero;
		}

		public string PropertyName = string.Empty;
		public float MinValue = float.MinValue;
		public float MaxValue = float.MaxValue;
		public bool CanBeZero = false;
	}
	#endregion

#if !SILVERLIGHT
	[Serializable]
#endif
	public class CalculationOptionsRestoSham : ICalculationOptionBase
	{
		#region GetXml strings
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
					  new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRestoSham));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}
		#endregion
		#region Defaults for variables
		/// <summary>
		/// Fight length, in minutes.
		/// </summary>
		public float FightLength = 5.0f;

		/// <summary>
		/// Whether a Mana Tide totem is placed every time the cooldown is up.
		/// </summary>
		public bool ManaTideEveryCD = true;

        /// <summary>
        /// Count of Innervates you get.
        /// </summary>
        public float Innervates = 0f;

        /// <summary>
		/// Whether we keep Water Shield up or not (could use Earth Shield during some fights).
		/// </summary>
		public bool WaterShield = true;

		/// <summary>
		/// Earth shield use.
		/// </summary>
		public bool EarthShield = true;

		/// <summary>
		/// Your style of healing.
		/// </summary>
		public string BurstStyle = "RT+HW";

		/// <summary>
		/// Your style of healing.
		/// </summary>
		public string SustStyle = "RT+CH";

        /// <summary>
        /// Targets.
        /// </summary>
        public string Targets = "Raid";

        /// <summary>
		/// Will you or someone else use heroism?
		/// </summary>
		public string Heroism = "Me";

		/// <summary>
		/// The percentage of time that Replenishment is "up".
		/// </summary>
        public float ReplenishmentPercentage = 90f;

		/// <summary>
		/// The number of times you use Cleanse Spirit.
		/// </summary>
		public float Decurse = 0f;

        /// <summary>
        /// The latency in milliseconds.
        /// </summary>
        public float Latency = 60f;

		/// <summary>
		/// The percentage of usefulness survival is to you.
		/// </summary>
		public float SurvivalPerc = 2f;
		#endregion

	}
}
