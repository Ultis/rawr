using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class CharacterCalculationsProtWarr : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }
        public List<Buff> ActiveBuffs { get; set; }
        public AbilityModelList Abilities { get; set; }

        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float SurvivalPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        public float MitigationPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = value; }
        }

        public float ThreatPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = value; }
        }

        public int TargetLevel { get; set; }
        public float ThreatScale { get; set; }

        public float Defense { get; set; } 
        public float Dodge { get; set; }
        public float Parry { get; set; }
        public float Block { get; set; }
        public float BlockValue { get; set; }
        public float Miss { get; set; }
        public float CritReduction { get; set; }
        public float CritVulnerability { get; set; }
        public float ArmorReduction { get; set; }
        public float GuaranteedReduction { get; set; }
        public float DodgePlusMissPlusParry { get; set; }
        public float DodgePlusMissPlusParryPlusBlock { get; set; }
        public float TotalMitigation { get; set; }
        public float BaseAttackerSpeed { get; set; }
        public float AttackerSpeed { get; set; }
        public float DamageTaken { get; set; }
        public float DamageTakenPerHit { get; set; }
        public float DamageTakenPerBlock { get; set; }
        public float DamageTakenPerCrit { get; set; }
        public float TankPoints { get; set; }

        public float NatureSurvivalPoints { get; set; }
        public float FrostSurvivalPoints { get; set; }
        public float FireSurvivalPoints { get; set; }
        public float ShadowSurvivalPoints { get; set; }
        public float ArcaneSurvivalPoints { get; set; }

        public float MissedAttacks { get; set; }
        public float AvoidedAttacks { get; set; }
        public float DodgedAttacks { get; set; }
        public float ParriedAttacks { get; set; }
        public float BlockedAttacks { get; set; }
        public float Hit { get; set; }
        public float Crit { get; set; }
        public float Expertise { get; set; }
        public float Haste { get; set; }
        public float ArmorPenetration { get; set; }
        public float WeaponSpeed { get; set; }
        public float TotalDamagePerSecond { get; set; }
        
        public float LimitedThreat { get; set; }
        public float UnlimitedThreat { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Strength", BasicStats.Strength.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Armor", string.Format("{0}*Reduces physical damage taken by {1:0.00%}", BasicStats.Armor, ArmorReduction));
            dictValues.Add("Defense", Defense.ToString() + string.Format("*Defense Rating {0}", BasicStats.DefenseRating));
            dictValues.Add("Dodge", string.Format("{0:0.00%}*Dodge Rating {1}", Dodge, BasicStats.DodgeRating));
            dictValues.Add("Parry", string.Format("{0:0.00%}*Parry Rating {1}", Parry, BasicStats.ParryRating));
            dictValues.Add("Block", string.Format("{0:0.00%}*Block Rating {1}", Block, BasicStats.BlockRating));
            dictValues.Add("Miss", string.Format("{0:0.00%}", Miss));
            dictValues.Add("Block Value", string.Format("{0}", BlockValue));
            dictValues.Add("Guaranteed Reduction", string.Format("{0:0.00%}", GuaranteedReduction));
            dictValues.Add("Avoidance", string.Format("{0:0.00%}", DodgePlusMissPlusParry));
            dictValues.Add("Avoidance + Block", string.Format("{0:0.00%}", DodgePlusMissPlusParryPlusBlock));
            dictValues.Add("Total Mitigation", string.Format("{0:0.00%}", TotalMitigation));
            
            if(AttackerSpeed == BaseAttackerSpeed)
                dictValues.Add("Attacker Speed", string.Format("{0:0.00}s", AttackerSpeed));
            else
                dictValues.Add("Attacker Speed", string.Format("{0:0.00}s*Base speed of {1:0.00}s (reduced by parry haste)", AttackerSpeed, BaseAttackerSpeed));
            
            dictValues.Add("Damage Taken", 
                string.Format("{0:0.0} DPS*{1:0} damage per normal attack" + Environment.NewLine +
                                "{2:0} damage per blocked attack" + Environment.NewLine +
                                "{3:0} damage per critical attack", DamageTaken, DamageTakenPerHit, DamageTakenPerBlock, DamageTakenPerCrit));
            
            dictValues.Add("Resilience",
                string.Format(@"{0}*Reduces periodic damage and chance to be critically hit by {1}%." + Environment.NewLine +
                                "Reduces the effect of mana-drains and the damage of critical strikes by {2}%.",
                                BasicStats.Resilience,
                                BasicStats.Resilience * ProtWarr.ResilienceRatingToCritReduction,
                                BasicStats.Resilience * ProtWarr.ResilienceRatingToCritReduction * 2));
            
            if (CritVulnerability > 0.0001f)
            {
                double defenseNeeded = Math.Ceiling((CritVulnerability / (ProtWarr.DefenseToCritReduction / 100.0f)) / ProtWarr.DefenseRatingToDefense);
                double resilienceNeeded = Math.Ceiling(CritVulnerability / (ProtWarr.ResilienceRatingToCritReduction / 100.0f));
                dictValues.Add("Chance to be Crit",
                    string.Format("{0:0.00%}*CRITTABLE! Short by {1:0} defense or {2:0} resilience to be uncrittable.",
                                    CritVulnerability, defenseNeeded, resilienceNeeded));
            }
            else
                dictValues.Add("Chance to be Crit", string.Format("{0:0.00%}*Chance to crit reduced by {1:0.00%}", CritVulnerability, CritReduction));

            dictValues["Nature Resist"] = (BasicStats.NatureResistance + BasicStats.AllResist).ToString();
            dictValues["Arcane Resist"] = (BasicStats.ArcaneResistance + BasicStats.AllResist).ToString();
            dictValues["Frost Resist"] = (BasicStats.FrostResistance + BasicStats.AllResist).ToString();
            dictValues["Fire Resist"] = (BasicStats.FireResistance + BasicStats.AllResist).ToString();
            dictValues["Shadow Resist"] = (BasicStats.ShadowResistance + BasicStats.AllResist).ToString();
            dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
            dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
            dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
            dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
            dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString();

            dictValues.Add("Weapon Speed", string.Format("{0:0.00}*{1:0.00%} Haste", WeaponSpeed, Haste));
            dictValues.Add("Attack Power", string.Format("{0}", BasicStats.AttackPower));
            dictValues.Add("Hit", string.Format("{0:0.00%}*Hit Rating {1}", Hit, BasicStats.HitRating));
            dictValues.Add("Expertise", 
                string.Format("{0}*Expertise Rating {1}" + Environment.NewLine + "Reduces chance to be dodged or parried by {2:0.00%}.", 
                                Math.Round(BasicStats.ExpertiseRating * ProtWarr.ExpertiseRatingToExpertise + BasicStats.Expertise),
                                BasicStats.ExpertiseRating, Expertise));
            dictValues.Add("Haste", string.Format("{0:0.00%}*Haste Rating {1:0.00}", Haste, BasicStats.HasteRating));
            dictValues.Add("Armor Penetration", 
                string.Format("{0:0.00%}*Armor Penetration Rating {1}" + Environment.NewLine + "Armor Reduction {2}", 
                                ArmorPenetration, BasicStats.ArmorPenetrationRating, BasicStats.ArmorPenetration));
            dictValues.Add("Crit", string.Format("{0:0.00%}*Crit Rating {1}", Crit, BasicStats.CritRating));
            dictValues.Add("Weapon Damage", string.Format("{0}", BasicStats.WeaponDamage));
            dictValues.Add("Missed Attacks",
                string.Format("{0:0.00%}*Attacks Missed: {1:0.00%}" + Environment.NewLine + "Attacks Dodged: {2:0.00%}" + Environment.NewLine + 
                                "Attacks Parried: {3:0.00%}", AvoidedAttacks, MissedAttacks, DodgedAttacks, ParriedAttacks));
            dictValues.Add("Total Damage/sec", string.Format("{0:0.0}", TotalDamagePerSecond));
            dictValues.Add("Limited Threat/sec", string.Format("{0:0.0}",LimitedThreat));
            dictValues.Add("Unlimited Threat/sec", string.Format("{0:0.0}", UnlimitedThreat));

            dictValues.Add("Tank Points", string.Format("{0:0}", TankPoints));
            dictValues.Add("Overall Points", string.Format("{0:0}", OverallPoints));
            dictValues.Add("Mitigation Points", string.Format("{0:0}", MitigationPoints));
            dictValues.Add("Survival Points", string.Format("{0:0}", SurvivalPoints));
            dictValues.Add("Threat Points", string.Format("{0:0}", ThreatPoints));

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "Unlimited TPS": return UnlimitedThreat;
                case "Limited TPS": return LimitedThreat;
                case "% Guaranteed Reduction": return GuaranteedReduction * 100.0f;
                case "% Chance to Avoid Attacks": return DodgePlusMissPlusParry * 100.0f;
                case "% Chance to be Crit": return ((float)Math.Round(CritVulnerability * 100.0f, 2));
                case "% Chance to be Avoided": return AvoidedAttacks;
                case "Nature Survival": return NatureSurvivalPoints;
                case "Fire Survival": return FireSurvivalPoints;
                case "Frost Survival": return FrostSurvivalPoints;
                case "Shadow Survival": return ShadowSurvivalPoints;
                case "Arcane Survival": return ArcaneSurvivalPoints;
            }
            return 0.0f;
        }
    }
}