using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
    [Serializable]
    public class Stats
    {
        [System.Xml.Serialization.XmlElement("Armor")]
        public float _armor;
        [System.Xml.Serialization.XmlElement("Health")]
        public float _health;
        [System.Xml.Serialization.XmlElement("Agility")]
        public float _agility;
        [System.Xml.Serialization.XmlElement("Stamina")]
        public float _stamina;
        [System.Xml.Serialization.XmlElement("DodgeRating")]
        public float _dodgeRating;
        [System.Xml.Serialization.XmlElement("DefenseRating")]
        public float _defenseRating;
        [System.Xml.Serialization.XmlElement("Resilience")]
        public float _resilience;
		[System.Xml.Serialization.XmlElement("Miss")]
		public float _miss;
		[System.Xml.Serialization.XmlElement("BonusAgilityMultiplier")]
		public float _bonusAgilityMultiplier;
		[System.Xml.Serialization.XmlElement("BonusStaminaMultiplier")]
		public float _bonusStaminaMultiplier;
		[System.Xml.Serialization.XmlElement("BonusArmorMultiplier")]
		public float _bonusArmorMultiplier;
        
        [System.Xml.Serialization.XmlIgnore]
        public float Armor
        {
            get { return _armor; }
            set { _armor = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public float Agility
        {
            get { return _agility; }
            set { _agility = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public float Stamina
        {
            get { return _stamina; }
            set { _stamina = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public float DodgeRating
        {
            get { return _dodgeRating; }
            set { _dodgeRating = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public float DefenseRating
        {
            get { return _defenseRating; }
            set { _defenseRating = value; }
        }
        [System.Xml.Serialization.XmlIgnore]
        public float Resilience
        {
            get { return _resilience; }
            set { _resilience = value; }
        }
		[System.Xml.Serialization.XmlIgnore]
		public float Miss
		{
			get { return _miss; }
			set { _miss = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float BonusAgilityMultiplier
		{
			get { return _bonusAgilityMultiplier; }
			set { _bonusAgilityMultiplier = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float BonusStaminaMultiplier
		{
			get { return _bonusStaminaMultiplier; }
			set { _bonusStaminaMultiplier = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float BonusArmorMultiplier
		{
			get { return _bonusArmorMultiplier; }
			set { _bonusArmorMultiplier = value; }
		}
        
		//with hands high into the sky so blue
        public Stats() { }
		public Stats(float armor, float health, float agility, float stamina, float dodgeRating, float defenseRating, float resilience)
			: this(armor, health, agility, stamina, dodgeRating, defenseRating, resilience, 0f, 0f, 0f, 0f) { }
		public Stats(float armor, float health, float agility, float stamina, float dodgeRating, float defenseRating,
			float resilience, float miss, float bonusAgilityMultiplier, float bonusStaminaMultiplier, float bonusArmorMultiplier)
        {
            _armor = armor;
            _health = health;
            _agility = agility;
            _stamina = stamina;
            _dodgeRating = dodgeRating;
            _defenseRating = defenseRating;
            _resilience = resilience;
			_miss = miss;
			_bonusAgilityMultiplier = bonusAgilityMultiplier;
			_bonusStaminaMultiplier = bonusStaminaMultiplier;
			_bonusArmorMultiplier = bonusArmorMultiplier;
        }

		//as the ocean opens up to swallow you
		public static Stats operator +(Stats a, Stats b)
		{
			return new Stats(a.Armor + b.Armor, a.Health + b.Health, a.Agility + b.Agility, a.Stamina + b.Stamina, a.DodgeRating + b.DodgeRating,
				a.DefenseRating + b.DefenseRating, a.Resilience + b.Resilience, a.Miss + b.Miss,
				((1 + a.BonusAgilityMultiplier) * (1 + b.BonusAgilityMultiplier)) - 1,
				((1 + a.BonusStaminaMultiplier) * (1 + b.BonusStaminaMultiplier)) - 1,
				((1 + a.BonusArmorMultiplier) * (1 + b.BonusArmorMultiplier)) - 1);
		}

        public override string ToString()
        {
            string summary = string.Empty;
            if (Armor > 0) summary += Armor.ToString() + "AC, ";
            if (Health > 0) summary += Health.ToString() + "HP, ";
            if (Agility > 0) summary += Agility.ToString() + "Agi, ";
            if (Stamina > 0) summary += Stamina.ToString() + "Sta, ";
            if (DodgeRating > 0) summary += DodgeRating.ToString() + "Dodge, ";
            if (DefenseRating > 0) summary += DefenseRating.ToString() + "Def, ";
            if (Resilience > 0) summary += Resilience.ToString() + "Res, ";
			if (Miss > 0) summary += Miss.ToString() + "%Miss, ";
			if (BonusAgilityMultiplier > 0) summary += (100f * BonusAgilityMultiplier).ToString() + "%Agi, ";
			if (BonusStaminaMultiplier > 0) summary += (100f * BonusStaminaMultiplier).ToString() + "%Sta, ";
			if (BonusArmorMultiplier > 0) summary += (100f * BonusArmorMultiplier).ToString() + "%AC, ";
			summary = summary.TrimEnd(' ', ',', ':');
            return summary;
        }
    }
}
