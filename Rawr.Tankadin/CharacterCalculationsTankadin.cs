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
        public float Resilience {get; set; }
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
        public float ToCrit { get; set; }
        public float ToSpellCrit { get; set; }

        public float ShoRDamage { get; set; }
        public float ShoRThreat { get; set; }
        public float ShoRJBVDamage { get; set; }
        public float ShoRJBVThreat { get; set; }

        public float ASDamage { get; set; }
        public float ASThreat { get; set; }

        public float ConsDamage { get; set; }
        public float ConsTPS { get; set; }
        public float ConsDuration { get; set; }

        public float HotRDamage { get; set; }
        public float HotRThreat { get; set; }

        public float SoRDamage { get; set; }
        public float SoRThreat { get; set; }
        public float SoVDamage { get; set; }
        public float SoVTPS { get; set; }
        
        public float HSDamage { get; set; }
        public float HSThreat { get; set; }
        public float HSProcs { get; set; }

        public float JudgementCD { get; set; }
        
        public float JoRDamage { get; set; }
        public float JoRThreat { get; set; }
        public float JoVDamage { get; set; }
        public float JoVThreat { get; set; }

        public float WhiteDamage { get; set; }
        public float WhiteThreat { get; set; }

        public float Rot1TPS { get; set; }
		public float Rot2TPS { get; set; }
        
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

			dict.Add("Health", BasicStats.Health.ToString());
            dict.Add("Armor", string.Format("{0}*{1}% Damage Reduction", BasicStats.Armor, Math.Round(ArmorReduction * 100f, 2)));
			dict.Add("Stamina", BasicStats.Stamina.ToString());
			dict.Add("Strength", Math.Round(BasicStats.Strength).ToString());
			dict.Add("Agility", BasicStats.Agility.ToString());
            dict.Add("Defense", string.Format("{0}*{1} Defense Rating", Math.Floor(Defense + 400f), BasicStats.DefenseRating));
            dict.Add("Resilience", Math.Round(Resilience).ToString());
			dict.Add("Attack Power", BasicStats.AttackPower.ToString());
			dict.Add("Spell Damage", Math.Round(BasicStats.SpellPower).ToString());
            dict.Add("Expertise", BasicStats.Expertise.ToString());
            dict.Add("Block Value", BlockValue.ToString());
			dict.Add("Miss", string.Format("{0}%", Math.Round(Miss * 100f, 2)));
			dict.Add("Dodge", string.Format("{0}%", Math.Round(Dodge * 100f, 2)));
			dict.Add("Parry", string.Format("{0}%", Math.Round(Parry * 100f, 2)));
			dict.Add("Block", string.Format("{0}%", Math.Round(Block * 100f, 2)));
			dict.Add("Crit", string.Format("{0}%", Math.Round(Crit * 100f, 2)));
			dict.Add("Hit", string.Format("{0}%", Math.Round(Hit * 100f, 2)));
			dict.Add("Avoidance", string.Format("{0}%", Math.Round(Avoidance * 100f, 2)));
            dict.Add("Mitigation", string.Format("{0}%", Math.Round((1f - Mitigation) * 100f, 2)));
            dict.Add("Damage Taken", string.Format("{0}%", Math.Round(DamageTaken * 100f, 2)));
			dict.Add("Damage When Hit", string.Format("{0}*{1} on blocks\n{2} normal", 
				Math.Round(DamageWhenHit), Math.Round(DamagePerBlock), Math.Round(DamagePerHit)));
            if (.05f - CritAvoidance < 0)
            	if (Math.Abs(.05f - CritAvoidance)< .00008133f)
            		dict.Add("Chance to be Crit", string.Format("{0}%*Exactly enough defense rating/resilience to be uncrittable.",
                    	Math.Round((.05f - CritAvoidance) * 100f, 2)));
            	else
            		dict.Add("Chance to be Crit", string.Format("{0}%*Uncrittable. {1} defense rating or {2} resilience over the cap.",
            			Math.Round((.05f - CritAvoidance) * 100f, 2), 
            			Math.Floor(Math.Abs(.05f - CritAvoidance) * 100f * 25f * 4.918498039f),
            			Math.Floor(Math.Abs(.05f - CritAvoidance) * 100f * 81.97497559f)));
            else
            	dict.Add("Chance to be Crit", string.Format("{0}%*CRITTABLE! Short by {1} defense rating or {2} resilience to be uncrittable.",
                    Math.Round((.05f - CritAvoidance) * 100f, 2), 
                    Math.Ceiling(Math.Abs(.05f - CritAvoidance) * 100f * 25f * 4.918498039f),
                    Math.Ceiling(Math.Abs(.05f - CritAvoidance) * 100f * 81.97497559f)));
/*			dict.Add("Overall Points", OverallPoints.ToString());
			dict.Add("Mitigation Points", MitigationPoints.ToString());
			dict.Add("Survival Points", SurvivalPoints.ToString());
			dict.Add("Threat Points", ThreatPoints.ToString());*/
            dict.Add("Chance to Hit", string.Format("{0}%*{1}% miss\n{2}% dodge\n{3}% parry", Math.Round(ToLand * 100f, 2), Math.Round(ToMiss * 100f, 2),
                Math.Round(ToDodge * 100, 2), Math.Round(ToParry * 100f, 2)));
            dict.Add("Chance to Miss", string.Format("{0}%", Math.Round(ToMiss * 100f, 2)));
            dict.Add("Chance to Dodge", string.Format("{0}%", Math.Round(ToDodge * 100f, 2)));
            dict.Add("Chance to Parry", string.Format("{0}%", Math.Round(ToParry * 100f, 2)));            
            dict.Add("Chance to Crit", string.Format("{0}%", Math.Round(ToCrit * 100f, 2)));
            dict.Add("Consecrate", string.Format("{0} threat*{1} damage per hit", Math.Round(ConsTPS), Math.Round(ConsDamage/ConsDuration)));
            dict.Add("ShoR", string.Format("{0} threat*{1} damage", Math.Round(ShoRThreat), Math.Round(ShoRDamage)));
            dict.Add("HotR", string.Format("{0} threat*{1} damage", Math.Round(HotRThreat), Math.Round(HotRDamage)));
            dict.Add("Avenger's Shield", string.Format("{0} threat*{1} damage", Math.Round(ASThreat), Math.Round(ASDamage)));
            dict.Add("Holy Shield", string.Format("{0} threat*{1} damage", Math.Round(HSThreat), Math.Round(HSDamage)));
            dict.Add("SoR", string.Format("{0} tps*{1} damage", Math.Round(SoRThreat), Math.Round(SoRDamage)));
            dict.Add("JoR", string.Format("{0} threat*{1} damage", Math.Round(JoRThreat), Math.Round(JoRDamage)));
            dict.Add("SoV", string.Format("{0} tps*{1} damage per tick", Math.Round(SoVTPS), Math.Round(SoVDamage)));
            dict.Add("JoV", string.Format("{0} threat*{1} damage", Math.Round(JoVThreat), Math.Round(JoVDamage)));
            dict.Add("White Damage", string.Format("{0} tps*{1} damage", Math.Round(WhiteThreat), Math.Round(WhiteDamage)));
            dict.Add("Total Threat (SoV)", string.Format("{0} tps", Math.Round(Rot1TPS)));
            dict.Add("Total Threat (SoR)", string.Format("{0} tps", Math.Round(Rot2TPS)));
            return dict;
        }
        
        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "% Chance to be Hit": return Hit * 100f;
                case "% Chance to be Crit": return (.05f - CritAvoidance) * 100f;
                case "Defense": return 400f + Defense;
                case "SoV TPS": return Rot1TPS;
                case "SoR TPS": return Rot2TPS;
                case "Block Value": return BlockValue;
                case "Expertise": return BasicStats.Expertise;
                case "% Chance to Hit": return ToLand * 100f;
                case "Avoidance": return Avoidance * 100f;
            }
            return 0.0f;
        }
    }
}