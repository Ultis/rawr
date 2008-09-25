using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tankadin
{
    public class CharacterCalculationsTankadin : CharacterCalculationsBase
    {

        public override float OverallPoints { get; set; }

        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float MitigationPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float SurvivalPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float ThreatPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }

        public Stats BasicStats { get; set; }

        public float ArmorReduction { get; set; }
        public float Defense { get; set; }
        public float Dodge { get; set; }
        public float Miss { get; set; }
        public float Parry { get; set; }
        public float Block { get; set; }
        public float Hit { get; set; }
        public float Crit { get; set; }
        public float BlockValue { get; set; }
        public float Mitigation { get; set; }
        public float Avoidance { get; set; }
        public float DamageTaken { get; set; }
        public float CritAvoidance { get; set; }

        public float DamagePerHit { get; set; }
        public float DamagePerBlock { get; set; }
        public float DamageWhenHit { get; set; }

        public float ToMiss { get; set; }
        public float ToDodge { get; set; }
        public float ToParry { get; set; }
        public float ToResist { get; set; }
        public float ToLand { get; set; }


        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

			dict.Add("Health", BasicStats.Health.ToString());
			dict.Add("Armor", string.Format("{0}*{1}% Damage Reduction", BasicStats.Armor, Math.Round(ArmorReduction*100,2)));
			dict.Add("Stamina", BasicStats.Stamina.ToString());
			dict.Add("Agility", BasicStats.Agility.ToString());
			dict.Add("Defense", Defense.ToString());
			dict.Add("Attack Power", BasicStats.AttackPower.ToString());
            dict.Add("Spell Damage", BasicStats.SpellDamageRating.ToString());
            dict.Add("Block Value", BlockValue.ToString());
			dict.Add("Miss", string.Format("{0}%", Math.Round(Miss * 100, 2)));
			dict.Add("Dodge", string.Format("{0}%", Math.Round(Dodge * 100, 2)));
			dict.Add("Parry", string.Format("{0}%", Math.Round(Parry * 100, 2)));
			dict.Add("Block", string.Format("{0}%", Math.Round(Block * 100, 2)));
			dict.Add("Crit", string.Format("{0}%", Math.Round(Crit * 100, 2)));
			dict.Add("Hit", string.Format("{0}%", Math.Round(Hit * 100, 2)));
			dict.Add("Avoidance", string.Format("{0}%", Math.Round(Avoidance * 100, 2)));
            dict.Add("Mitigation", string.Format("{0}%", Math.Round((1f - Mitigation) * 100, 2)));
			dict.Add("Damage Taken", string.Format("{0}", Math.Round(DamageTaken, 2)));
			dict.Add("Damage When Hit", string.Format("{0}*{1} on blocks\n{2} normal", 
				Math.Round(DamageWhenHit), Math.Round(DamagePerBlock), Math.Round(DamagePerHit)));
            if (CritAvoidance == .05f)
                dict.Add("Chance to be Crit", string.Format("{0}%*Exactly enough defense rating/resilience to be uncrittable.",
                    Math.Round((.05f - CritAvoidance) * 100, 2)));
            else if (CritAvoidance < .05f)
                dict.Add("Chance to be Crit", string.Format("{0}%*CRITTABLE! Short by {1} defense rating or {2} resilience to be uncrittable.",
                    Math.Round((.05f - CritAvoidance) * 100, 2), "<nyi>", "<nyi>"));
            else
                dict.Add("Chance to be Crit", string.Format("{0}%*Uncrittable. {1} defense rating or {2} resilience over the cap.",
                    Math.Round((.05f - CritAvoidance) * 100, 2), "<nyi>", "<nyi>"));
			dict.Add("Overall Points", OverallPoints.ToString());
			dict.Add("Mitigation Points", MitigationPoints.ToString());
			dict.Add("Survival Points", SurvivalPoints.ToString());
			dict.Add("Threat Points", ThreatPoints.ToString());

            return dict;
        }
    }
}