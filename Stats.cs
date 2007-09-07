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

        public Stats() { }
        public Stats(float armor, float health, float agility, float stamina, float dodgeRating, float defenseRating, float resilience)
        {
            _armor = armor;
            _health = health;
            _agility = agility;
            _stamina = stamina;
            _dodgeRating = dodgeRating;
            _defenseRating = defenseRating;
            _resilience = resilience;
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
            summary = summary.TrimEnd(' ', ',', ':');
            return summary;
        }
    }
}
