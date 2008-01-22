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
		[System.Xml.Serialization.XmlElement("BonusStrengthMultiplier")]
		public float _bonusStrengthMultiplier;
		[System.Xml.Serialization.XmlElement("BonusStaminaMultiplier")]
		public float _bonusStaminaMultiplier;
		[System.Xml.Serialization.XmlElement("BonusArmorMultiplier")]
		public float _bonusArmorMultiplier;
		[System.Xml.Serialization.XmlElement("BonusAttackPowerMultiplier")]
		public float _bonusAttackPowerMultiplier;
		[System.Xml.Serialization.XmlElement("AttackPower")]
		public float _attackPower;
		[System.Xml.Serialization.XmlElement("Strength")]
		public float _strength;
		[System.Xml.Serialization.XmlElement("CritRating")]
		public float _critRating;
		[System.Xml.Serialization.XmlElement("HitRating")]
		public float _hitRating;
		[System.Xml.Serialization.XmlElement("WeaponDamage")]
		public float _weaponDamage;
		[System.Xml.Serialization.XmlElement("ExpertiseRating")]
		public float _expertiseRating;
		[System.Xml.Serialization.XmlElement("HasteRating")]
		public float _hasteRating;
		[System.Xml.Serialization.XmlElement("ArmorPenetration")]
		public float _armorPentration;
		[System.Xml.Serialization.XmlElement("BonusCritMultiplier")]
		public float _bonusCritMultiplier;
		[System.Xml.Serialization.XmlElement("BonusShredDamage")]
		public float _bonusShredDamage;
		[System.Xml.Serialization.XmlElement("BonusMangleDamage")]
		public float _bonusMangleDamage;
		[System.Xml.Serialization.XmlElement("BonusRipDamageMultiplier")]
		public float _bonusRipDamageMultiplier;
		[System.Xml.Serialization.XmlElement("MangleCostReduction")]
		public float _mangleCostReduction;
		[System.Xml.Serialization.XmlElement("BloodlustProc")]
		public float _bloodlustProc;
		[System.Xml.Serialization.XmlElement("TerrorProc")]
		public float _terrorProc;
				
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
		public float BonusStrengthMultiplier
		{
			get { return _bonusStrengthMultiplier; }
			set { _bonusStrengthMultiplier = value; }
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
		[System.Xml.Serialization.XmlIgnore]
		public float BonusAttackPowerMultiplier
		{
			get { return _bonusAttackPowerMultiplier; }
			set { _bonusAttackPowerMultiplier = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float AttackPower
		{
			get { return _attackPower; }
			set { _attackPower = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float Strength
		{
			get { return _strength; }
			set { _strength = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float CritRating
		{
			get { return _critRating; }
			set { _critRating = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float HitRating
		{
			get { return _hitRating; }
			set { _hitRating = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float WeaponDamage
		{
			get { return _weaponDamage; }
			set { _weaponDamage = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float ExpertiseRating
		{
			get { return _expertiseRating; }
			set { _expertiseRating = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float HasteRating
		{
			get { return _hasteRating; }
			set { _hasteRating = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float ArmorPenetration
		{
			get { return _armorPentration; }
			set { _armorPentration = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float BonusCritMultiplier
		{
			get { return _bonusCritMultiplier; }
			set { _bonusCritMultiplier = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float BonusShredDamage
		{
			get { return _bonusShredDamage; }
			set { _bonusShredDamage = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float BonusMangleDamage
		{
			get { return _bonusMangleDamage; }
			set { _bonusMangleDamage = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float BonusRipDamageMultiplier
		{
			get { return _bonusRipDamageMultiplier; }
			set { _bonusRipDamageMultiplier = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float MangleCostReduction
		{
			get { return _mangleCostReduction; }
			set { _mangleCostReduction = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float BloodlustProc
		{
			get { return _bloodlustProc; }
			set { _bloodlustProc = value; }
		}
		[System.Xml.Serialization.XmlIgnore]
		public float TerrorProc
		{
			get { return _terrorProc; }
			set { _terrorProc = value; }
		}
		
		//with hands high into the sky so blue
        public Stats() { }
		//public Stats(float armor, float health, float agility, float stamina, float dodgeRating, float defenseRating, float resilience)
		//    : this(armor, health, agility, stamina, dodgeRating, defenseRating, resilience, 0f, 0f, 0f, 0f) { }
		//public Stats(float armor, float health, float agility, float stamina, float dodgeRating, float defenseRating,
		//    float resilience, float miss, float bonusAgilityMultiplier, float bonusStaminaMultiplier, float bonusArmorMultiplier)
		//{
		//    _armor = armor;
		//    _health = health;
		//    _agility = agility;
		//    _stamina = stamina;
		//    _dodgeRating = dodgeRating;
		//    _defenseRating = defenseRating;
		//    _resilience = resilience;
		//    _miss = miss;
		//    _bonusAgilityMultiplier = bonusAgilityMultiplier;
		//    _bonusStaminaMultiplier = bonusStaminaMultiplier;
		//    _bonusArmorMultiplier = bonusArmorMultiplier;
		//}

		public Stats Clone()
		{
			return new Stats()
			{
				Armor = this.Armor,
				Agility = this.Agility,
				ArmorPenetration = this.ArmorPenetration,
				AttackPower = this.AttackPower,
				BloodlustProc = this.BloodlustProc,
				TerrorProc = this.TerrorProc,
				BonusAgilityMultiplier = this.BonusAgilityMultiplier,
				BonusStrengthMultiplier = this.BonusStrengthMultiplier,
				BonusArmorMultiplier = this.BonusArmorMultiplier,
				BonusAttackPowerMultiplier = this.BonusAttackPowerMultiplier,
				BonusCritMultiplier = this.BonusCritMultiplier,
				BonusRipDamageMultiplier = this.BonusRipDamageMultiplier,
				BonusStaminaMultiplier = this.BonusStaminaMultiplier,
				BonusMangleDamage = this.BonusMangleDamage,
				BonusShredDamage = this.BonusShredDamage,
				CritRating = this.CritRating,
				DefenseRating = this.DefenseRating,
				DodgeRating = this.DodgeRating,
				ExpertiseRating = this.ExpertiseRating,
				HasteRating = this.HasteRating,
				Health = this.Health,
				HitRating = this.HitRating,
				MangleCostReduction = this.MangleCostReduction,
				Miss = this.Miss,
				Resilience = this.Resilience,
				Stamina = this.Stamina,
				Strength = this.Strength,
				WeaponDamage = this.WeaponDamage
			};

		}

		//as the ocean opens up to swallow you
		public static Stats operator +(Stats a, Stats b)
		{
			return new Stats() {
				Armor = a.Armor + b.Armor,
				Agility = a.Agility + b.Agility,
				ArmorPenetration = a.ArmorPenetration + b.ArmorPenetration,
				AttackPower = a.AttackPower + b.AttackPower,
				BloodlustProc = a.BloodlustProc + b.BloodlustProc,
				TerrorProc = a.TerrorProc + b.TerrorProc,
				BonusAgilityMultiplier = ((1 + a.BonusAgilityMultiplier) * (1 + b.BonusAgilityMultiplier)) - 1,
				BonusStrengthMultiplier = ((1 + a.BonusStrengthMultiplier) * (1 + b.BonusStrengthMultiplier)) - 1,
				BonusArmorMultiplier = ((1 + a.BonusArmorMultiplier) * (1 + b.BonusArmorMultiplier)) - 1,
				BonusAttackPowerMultiplier = ((1 + a.BonusAttackPowerMultiplier) * (1 + b.BonusAttackPowerMultiplier)) - 1,
				BonusCritMultiplier = ((1 + a.BonusCritMultiplier) * (1 + b.BonusCritMultiplier)) - 1,
				BonusRipDamageMultiplier = ((1 + a.BonusRipDamageMultiplier) * (1 + b.BonusRipDamageMultiplier)) - 1,
				BonusStaminaMultiplier = ((1 + a.BonusStaminaMultiplier) * (1 + b.BonusStaminaMultiplier)) - 1,
				BonusMangleDamage = a.BonusMangleDamage + b.BonusMangleDamage,
				BonusShredDamage = a.BonusShredDamage + b.BonusShredDamage,
				CritRating = a.CritRating + b.CritRating,
				DefenseRating = a.DefenseRating + b.DefenseRating,
				DodgeRating = a.DodgeRating + b.DodgeRating,
				ExpertiseRating = a.ExpertiseRating + b.ExpertiseRating,
				HasteRating = a.HasteRating + b.HasteRating,
				Health = a.Health + b.Health,
				HitRating = a.HitRating + b.HitRating,
				MangleCostReduction = a.MangleCostReduction + b.MangleCostReduction,
				Miss = a.Miss + b.Miss,
				Resilience = a.Resilience + b.Resilience,
				Stamina = a.Stamina + b.Stamina,
				Strength = a.Strength + b.Strength,
				WeaponDamage = a.WeaponDamage + b.WeaponDamage
			};
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
			if (BonusStrengthMultiplier > 0) summary += (100f * BonusStrengthMultiplier).ToString() + "%Str, ";
			if (BonusStaminaMultiplier > 0) summary += (100f * BonusStaminaMultiplier).ToString() + "%Sta, ";
			if (BonusArmorMultiplier > 0) summary += (100f * BonusArmorMultiplier).ToString() + "%AC, ";
			if (BonusCritMultiplier > 0) summary += (100f * BonusCritMultiplier).ToString() + "%CritDmg, ";
			if (BonusRipDamageMultiplier > 0) summary += (100f * BonusRipDamageMultiplier).ToString() + "%RipDmg, ";
			if (AttackPower > 0) summary += AttackPower.ToString() + "AP, ";
			if (Strength > 0) summary += Strength.ToString() + "Str, ";
			if (CritRating > 0) summary += CritRating.ToString() + "Crit, ";
			if (HitRating > 0) summary += HitRating.ToString() + "Hit, ";
			if (WeaponDamage > 0) summary += WeaponDamage.ToString() + "Dmg, ";
			if (ExpertiseRating > 0) summary += ExpertiseRating.ToString() + "Exp, ";
			if (HasteRating > 0) summary += HasteRating.ToString() + "Haste, ";
			if (ArmorPenetration > 0) summary += ArmorPenetration.ToString() + "Pen, ";
			if (BonusShredDamage > 0) summary += BonusShredDamage.ToString() + "ShredDmg, ";
			if (BonusMangleDamage > 0) summary += BonusMangleDamage.ToString() + "MangleDmg, ";
			if (MangleCostReduction > 0) summary += MangleCostReduction.ToString() + "MangleCostReduction, ";
			if (BloodlustProc > 0) summary += "Bloodlust, ";
			if (TerrorProc > 0) summary += "Terror, ";
			summary = summary.TrimEnd(' ', ',', ':');
            return summary;
        }
    }
}
