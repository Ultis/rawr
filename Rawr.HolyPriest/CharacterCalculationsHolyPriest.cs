using System;
using System.Collections.Generic;

namespace Rawr.HolyPriest
{

	public class CharacterCalculationsHolyPriest : CharacterCalculationsBase
	{

		public CharacterCalculationsHolyPriest()
		{
			_spells = new Dictionary<int, Spell>();
		}

		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f }; //, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float ThroughputPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float AvgHPS { get; set; }
		public float AvgHPM { get; set; }
		public float Healed { get; set; }

		private Stats _basicStats;
		public Stats BasicStats
		{
			get { return _basicStats; }
			set { _basicStats = value; Calculate(false); }
		}

		private readonly Dictionary<int, Spell> _spells;
		public Spell this[int i]
		{
			get { return _spells[i]; }
			set { _spells[i] = value; value.Calculate(_basicStats); }
		}

		public void Calculate() { Calculate(false); }
		public void Calculate(bool di)
		{
			foreach (KeyValuePair<int, Spell> kvp in _spells) { kvp.Value.Calculate(_basicStats, di); }
		}

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();

			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Stamina", BasicStats.Stamina.ToString());
			dictValues.Add("Mana", BasicStats.Mana.ToString());
			dictValues.Add("Intellect", BasicStats.Intellect.ToString());
			dictValues.Add("Spirit", BasicStats.Spirit.ToString());
			dictValues.Add("Healing", BasicStats.Healing.ToString());
			dictValues.Add("Mp5", BasicStats.Mp5.ToString());
			dictValues.Add("Spell Crit", string.Format("{0}%*{1} Spell Crit rating\nHoly Crit: {2}%", 
				Math.Round(_spells[0].SpellCrit * 100 - 5 - BasicStats.HoldyCrit, 2), BasicStats.SpellCritRating.ToString(),
				Math.Round(_spells[0].SpellCrit * 100, 2)));
			dictValues.Add("Spell Haste", string.Format("{0}%*{1} Spell Haste rating\nFoL Cast Time: {2} sec\nHL Cast Time: {3} sec",
				Math.Round(BasicStats.SpellHasteRating/15.7,2), BasicStats.SpellHasteRating.ToString(), Math.Round(_spells[0].CastTime,2), Math.Round(_spells[1].CastTime,2)));
			dictValues.Add("Total Healed", Math.Round(Healed).ToString());
			dictValues.Add("Average Hps", Math.Round(AvgHPS).ToString());
			dictValues.Add("Average Hpm", Math.Round(AvgHPM, 2).ToString());
			//dictValues.Add("Holy Light 4", string.Format("{0} hps*{1} hpm\n{2} average heal", Math.Round(_spells[8].Hps).ToString(), Math.Round(_spells[8].Hpm, 2).ToString(), Math.Round(_spells[8].AverageHeal).ToString()));
			return dictValues;
		}
	}
}
