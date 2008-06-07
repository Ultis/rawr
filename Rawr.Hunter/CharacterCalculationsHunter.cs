using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Rawr.Hunter
{
    public class CharacterCalculationsHunter : CharacterCalculationsBase
    {

		private float _overallPoints = 0f;
		private float[] _subPoints = new float[] { 0f,0f };
		private Stats _basicStats;
		private Stats _petStats;
		private float _RAP;
		private float _baseAttackSpeed;

		public float BaseAttackSpeed
		{
			get { return _baseAttackSpeed; }
			set { _baseAttackSpeed = value; }
		}
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float HunterDpsPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float PetDpsPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }

		}

		public Stats BasicStats
		{
			get { return _basicStats; }
			set { _basicStats = value; }
		}

		public Stats PetStats
		{
			get { return _petStats;}
			set { _petStats = value; }
		}
		public List<string> ActiveBuffs { get; set; }

		public float RAP
		{
			get { return _RAP; }
			set { _RAP = value; }
		}

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			
			dictValues.Add("Agility", BasicStats.Agility.ToString("F0"));
			dictValues.Add("Crit Rating", BasicStats.CritRating.ToString());
			dictValues.Add("Hit Rating", BasicStats.HitRating.ToString());
			dictValues.Add("Intellect", BasicStats.Intellect.ToString("F0"));
			dictValues.Add("Stamina", BasicStats.Stamina.ToString("F0"));
			dictValues.Add("Armor", BasicStats.Armor.ToString());
			dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString("F0"));
			dictValues.Add("Armor Penetration", BasicStats.ArmorPenetration.ToString());
			dictValues.Add("MP5", BasicStats.Mp5.ToString());
			dictValues.Add("Mana", BasicStats.Mana.ToString());
			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Hit Percentage", BasicStats.Hit.ToString("P2"));
			dictValues.Add("Crit Percentage", BasicStats.Crit.ToString("P2"));
			dictValues.Add("Pet Attack Power", PetStats.AttackPower.ToString("F0"));
			dictValues.Add("Pet Hit Percentage", PetStats.Hit.ToString("P2"));
			dictValues.Add("Pet Crit Percentage", PetStats.Crit.ToString("P2"));
			dictValues.Add("Ranged AP", BasicStats.RangedAttackPower.ToString("F0"));
			dictValues.Add("Attack Speed", BaseAttackSpeed.ToString("F2"));
			dictValues.Add("Hunter Total DPS", HunterDpsPoints.ToString("F2"));
			dictValues.Add("Pet DPS", PetDpsPoints.ToString("F2"));
			dictValues.Add("Overall DPS", OverallPoints.ToString("F2"));
			return dictValues;
		}

    }
}
