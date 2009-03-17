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
        private float _steadySpeed;
		private double _PetBaseDPS;
		private double _PetSpecialDPS;
		private double _PetKillCommandDPS;
        private double _autoshotDPS;
        private double _steadySpamDPS;
        private double _arcane3xSteadyDPS;
        private double _arcane2xSteadyDPS;
        private double _serpASSteadyDPS;
        private double _expSteadySerpDPS;
        private double _chimASSteadyDPS;
        private double _customDPS;
        private String _customRotation; 

		public float BaseAttackSpeed
		{
			get { return _baseAttackSpeed; }
			set { _baseAttackSpeed = value; }
		}

        public float SteadySpeed
        {
            get { return _steadySpeed; }
            set { _steadySpeed = value; }
        }

        public double AutoshotDPS
        {
            get { return _autoshotDPS; }
            set { _autoshotDPS = value; }
        }

        public double SteadySpamDPS
        {
            get { return _steadySpamDPS; }
            set { _steadySpamDPS = value; }
        }

        public double Arcane3xSteadyDPS
        {
            get { return _arcane3xSteadyDPS; }
            set { _arcane3xSteadyDPS = value; }
        }

        public double Arcane2xSteadyDPS
        {
            get { return _arcane2xSteadyDPS; }
            set { _arcane2xSteadyDPS = value; }
        }

        public double SerpASSteadyDPS
        {
            get { return _serpASSteadyDPS; }
            set { _serpASSteadyDPS = value; }
        }

        public double ExpSteadySerpDPS
        {
            get { return _expSteadySerpDPS; }
            set { _expSteadySerpDPS = value; }
        }

        public double ChimASSteadyDPS
        {
            get { return _chimASSteadyDPS; }
            set { _chimASSteadyDPS = value; }
        }

        public double CustomDPS
        {
            get { return _customDPS; }
            set { _customDPS = value; }
        }

        public String CustomRotation
        {
            get { return _customRotation; }
            set { _customRotation = value; }
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

		public double PetBaseDPS
		{
			get { return _PetBaseDPS; }
			set { _PetBaseDPS = value; }
		}

		public double PetSpecialDPS
		{
			get { return _PetSpecialDPS; }
			set { _PetSpecialDPS = value; }
		}

		public double PetKillCommandDPS
		{
			get { return _PetKillCommandDPS; }
			set { _PetKillCommandDPS = value; }
		}


		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();
			
			dictValues.Add("Agility", BasicStats.Agility.ToString("F0"));
			dictValues.Add("Crit Rating", BasicStats.CritRating.ToString("F0"));
			dictValues.Add("Hit Rating", BasicStats.HitRating.ToString("F0"));
			dictValues.Add("Intellect", BasicStats.Intellect.ToString("F0"));
			dictValues.Add("Stamina", BasicStats.Stamina.ToString("F0"));
			dictValues.Add("Armor", BasicStats.Armor.ToString("F0"));
			dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString("F0"));
			dictValues.Add("Armor Penetration", BasicStats.ArmorPenetrationRating.ToString());
			dictValues.Add("MP5", BasicStats.Mp5.ToString("F0"));
			dictValues.Add("Mana", BasicStats.Mana.ToString("F0"));
			dictValues.Add("Health", BasicStats.Health.ToString("F0"));
			dictValues.Add("Hit Percentage", BasicStats.PhysicalHit.ToString("P2"));
			dictValues.Add("Crit Percentage", BasicStats.PhysicalCrit.ToString("P2"));
			dictValues.Add("Pet Attack Power", PetStats.AttackPower.ToString("F0"));
			dictValues.Add("Pet Hit Percentage", PetStats.PhysicalHit.ToString("P2"));
			dictValues.Add("Pet Crit Percentage", PetStats.PhysicalCrit.ToString("P2"));
			dictValues.Add("Pet Base DPS", PetBaseDPS.ToString("F2"));
			dictValues.Add("Pet Special DPS", PetSpecialDPS.ToString("F2"));
			dictValues.Add("Pet KC DPS", PetKillCommandDPS.ToString("F2"));
			dictValues.Add("Ranged AP", RAP.ToString("F0") + "*includes expected Expose Weakness uptime (if applicable)\n"+BasicStats.RangedAttackPower.ToString("F0") + " before talents");
			dictValues.Add("Attack Speed", BaseAttackSpeed.ToString("F2"));
            dictValues.Add("Steady Speed", SteadySpeed.ToString("F2"));
			dictValues.Add("Hunter Total DPS", HunterDpsPoints.ToString("F2"));
			dictValues.Add("Pet DPS", PetDpsPoints.ToString("F2"));
			dictValues.Add("Overall DPS", OverallPoints.ToString("F2"));

            dictValues.Add("Autoshot DPS", AutoshotDPS.ToString("F2"));
            dictValues.Add("Steady Spam", SteadySpamDPS.ToString("F2"));
            dictValues.Add("AS 3xSteady", Arcane3xSteadyDPS.ToString("F2"));
            dictValues.Add("AS 2xSteady", Arcane2xSteadyDPS.ToString("F2"));
            dictValues.Add("Custom Rotation", CustomDPS.ToString("F2") + CustomRotation);

            String serpRota = @"*Serpent Sting
Arcane Shot
Steady Shot
Steady Shot
Steady Shot
Arcane Shot
Steady Shot
Steady Shot
Steady Shot";
            dictValues.Add("SerpASSteady", SerpASSteadyDPS.ToString("F2") + serpRota);

            String expRota = @"*Explosive Shot
Serpent Sting
Steady Shot
Steady Shot
Explosive Shot
Steady Shot
Steady Shot
Steady Shot";
            dictValues.Add("ExpSteadySerp", ExpSteadySerpDPS.ToString("F2") + expRota);

            String chimRota = @"*Chimera Shot
Arcane Shot
Steady Shot
Steady Shot
Steady Shot
Steady Shot";

            dictValues.Add("ChimASSteady", ChimASSteadyDPS.ToString("F2") + chimRota);
			return dictValues;
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health":
					return BasicStats.Health;
				case "Crit Rating":
					return BasicStats.CritRating;
				case "Hit Rating":
					return BasicStats.HitRating;
				case "Mana":
					return BasicStats.Mana;
			}
			return 0;
		}
    }
}


