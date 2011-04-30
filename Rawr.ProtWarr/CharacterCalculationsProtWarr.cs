using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr
{
    public class CharacterCalculationsProtWarr : CharacterCalculationsBase
    {
        public Stats BasicStats { get; set; }
        public AbilityModelList Abilities { get; set; }

        public override float OverallPoints
        {
            get { return (_subPoints[0] + _subPoints[1] + _subPoints[2]); }
            set { }
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
            set { _subPoints[0] = Math.Min(value, float.MaxValue); }
        }

        public float MitigationPoints
        {
            get { return _subPoints[1]; }
            set { _subPoints[1] = Math.Min(value, float.MaxValue); }
        }

        public float ThreatPoints
        {
            get { return _subPoints[2]; }
            set { _subPoints[2] = Math.Min(value, float.MaxValue); }
        }

        public int TargetLevel { get; set; }
        public int RankingMode { get; set; }
        public float ThreatScale { get; set; }

        public float Dodge { get; set; }
        public float Parry { get; set; }
        public float Block { get; set; }
        public float CriticalBlock { get; set; }
        public float BaseBlock { get; set; }
        public float BuffedBlock { get; set; }
        public float Miss { get; set; }
        public float CritReduction { get; set; }
        public float CritVulnerability { get; set; }
        public float ArmorReduction { get; set; }
        public float GuaranteedReduction { get; set; }
        public float AnyMiss { get; set; }
        public float AnyAvoid { get; set; }
        public float BaseAnyAvoid { get; set; }
        public float BuffedAnyAvoid { get; set; }
        public float TotalMitigation { get; set; }
        public float BaseAttackerSpeed { get; set; }
        public float AttackerSpeed { get; set; }
        public float DamageTaken { get; set; }
        public float DamageTakenPerHit { get; set; }
        public float DamageTakenPerBlock { get; set; }
        public float DamageTakenPerCritBlock { get; set; }
        public float DamageTakenPerCrit { get; set; }

        public float TankPoints { get; set; }
        public float BurstTime { get; set; }

        public float NatureReduction { get; set; }
        public float FrostReduction { get; set; }
        public float FireReduction { get; set; }
        public float ShadowReduction { get; set; }
        public float ArcaneReduction { get; set; }
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
        
        public float ThreatPerSecond { get; set; }
        public float HeroicStrikeFrequency { get; set; }
        public string ThreatModel { get; set; }
        public string ThreatModelName { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Strength", BasicStats.Strength.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Armor", string.Format("{0}*Reduces physical damage taken by {1:0.00%}", BasicStats.Armor, ArmorReduction));
            dictValues.Add("Dodge", string.Format("{0:0.00%}*Dodge Rating {1}", Dodge, BasicStats.DodgeRating));
            dictValues.Add("Parry", string.Format("{0:0.00%}*Parry Rating {1}", Parry, BasicStats.ParryRating));
            dictValues.Add("Miss", string.Format("{0:0.00%}", Miss));

            if (Block != BuffedBlock)
                dictValues.Add("Block", string.Format("{0:0.00%}*Mastery Rating {1}" + Environment.NewLine +
                                                        "{2:0.00%} Normal Block Chance" + Environment.NewLine +
                                                        "{3:0.00%} Block Chance w/ Shield Block", Block, BasicStats.MasteryRating, BaseBlock, BuffedBlock));
            else
                dictValues.Add("Block", string.Format("{0:0.00%}*Mastery Rating {1}", Block, BasicStats.MasteryRating));

            if (Block > 0)
                dictValues.Add("Critical Block", string.Format("{0:0.00%} ({1:0.00%} actual)*Mastery Rating {2}", CriticalBlock / Block, CriticalBlock, BasicStats.MasteryRating));
            else
                dictValues.Add("Critical Block", string.Format("{0:0.00%}*Mastery Rating {1}", 0, BasicStats.MasteryRating));

            dictValues.Add("Guaranteed Reduction", string.Format("{0:0.00%}", GuaranteedReduction));

            if(AnyAvoid != BaseAnyAvoid)
                dictValues.Add("Avoidance", string.Format("{0:0.00%}*Avoidance: {0:0.00%}" + Environment.NewLine +
                                                            "+ Normal Block Chance: {1:0.00%}" + Environment.NewLine +
                                                            "+ Average Block Chance: {2:0.00%}" + Environment.NewLine +
                                                            "+ Block Chance w/ Shield Block: {3:0.00%}", AnyMiss, BaseAnyAvoid, AnyAvoid, BuffedAnyAvoid));
            else
                dictValues.Add("Avoidance", string.Format("{0:0.00%}*Avoidance: {0:0.00%}" + Environment.NewLine +
                                                           "Avoidance + Block: {1:0.00%}", AnyMiss, BaseAnyAvoid));

            dictValues.Add("Total Mitigation", string.Format("{0:0.00%}", TotalMitigation));
            
            if(AttackerSpeed == BaseAttackerSpeed)
                dictValues.Add("Attacker Speed", string.Format("{0:0.00}s", AttackerSpeed));
            else
                dictValues.Add("Attacker Speed", string.Format("{0:0.00}s*Base speed of {1:0.00}s (modified by haste)", AttackerSpeed, BaseAttackerSpeed));

            dictValues.Add("Damage Taken", 
                string.Format("{0:0.0} DPS*{1:0} damage per normal attack" + Environment.NewLine +
                                "{2:0} damage per blocked attack" + Environment.NewLine +
                                "{3:0} damage per critically blocked attack" + Environment.NewLine +
                                "{4:0} damage per critical attack", DamageTaken, DamageTakenPerHit, DamageTakenPerBlock, DamageTakenPerCritBlock, DamageTakenPerCrit));
            
            if (CritVulnerability > 0.0001f)
                dictValues.Add("Chance to be Crit", string.Format("{0:0.00%}*CRITTABLE!", CritVulnerability));
            else
                dictValues.Add("Chance to be Crit", string.Format("{0:0.00%}*Chance to crit reduced by {1:0.00%}", CritVulnerability, CritReduction));

            dictValues.Add("Nature Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.NatureResistance, NatureReduction));
            dictValues.Add("Arcane Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.ArcaneResistance, ArcaneReduction));
            dictValues.Add("Frost Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.FrostResistance, FrostReduction));
            dictValues.Add("Fire Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.FireResistance, FireReduction));
            dictValues.Add("Shadow Resist", string.Format("{0:0}*{1:0.00%} Total Reduction in Defensive Stance", BasicStats.ShadowResistance, ShadowReduction));
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
                                Math.Round(StatConversion.GetExpertiseFromRating(BasicStats.ExpertiseRating, CharacterClass.Warrior) + BasicStats.Expertise),
                                BasicStats.ExpertiseRating, Expertise));
            dictValues.Add("Haste", string.Format("{0:0.00%}*Haste Rating {1:0.00}", Haste, BasicStats.HasteRating));
            dictValues.Add("Armor Penetration", 
                string.Format("{0:0.00%}*" + /*"Armor Penetration Rating {1}" + Environment.NewLine +*/ "Armor Reduction {1}", 
                                ArmorPenetration, /*BasicStats.ArmorPenetrationRating,*/ BasicStats.ArmorPenetration));
            dictValues.Add("Crit", string.Format("{0:0.00%}*Crit Rating {1}", Crit, BasicStats.CritRating));
            dictValues.Add("Missed Attacks",
                string.Format("{0:0.00%}*Attacks Missed: {1:0.00%}" + Environment.NewLine + "Attacks Dodged: {2:0.00%}" + Environment.NewLine + 
                                "Attacks Parried: {3:0.00%}", AvoidedAttacks, MissedAttacks, DodgedAttacks, ParriedAttacks));
            dictValues.Add("Total Damage/sec", string.Format("{0:0.0}", TotalDamagePerSecond));
            dictValues.Add("Total Threat/sec", string.Format("{0:0.0}*Heroic Strike used every {1:0.0} seconds", ThreatPerSecond, 3.0f / HeroicStrikeFrequency));
            dictValues.Add("Rotation", ThreatModelName + "*" + ThreatModel);

            switch (RankingMode)
            {
                case 3: 
                    dictValues.Add("Ranking Mode", "Burst Time*The average amount of time between events which have a chance to result in a burst death");
                    dictValues.Add("Survival Points", string.Format("{0:0}*{1:0.00} seconds between events", SurvivalPoints, SurvivalPoints / 100.0f));
                    break;
                case 4:
                    dictValues.Add("Ranking Mode", "Damage Output*The average amount of DPS which can be produced");
                    dictValues.Add("Survival Points", string.Format("{0:0}*Survival is not weighted in this mode", SurvivalPoints, SurvivalPoints / 100.0f));
                    break;
                default: 
                    dictValues.Add("Ranking Mode", "Mitigation Scale*Default scale which compares mitigation, effective health, and threat.");
                    dictValues.Add("Survival Points", string.Format("{0:0}*Effective Health", SurvivalPoints));
                    break;
            }
            dictValues.Add("Overall Points", string.Format("{0:0}", OverallPoints));
            dictValues.Add("Mitigation Points", string.Format("{0:0}", MitigationPoints));
            dictValues.Add("Threat Points", string.Format("{0:0}", ThreatPoints));

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                
                case "% Total Mitigation": return TotalMitigation * 100.0f;
                case "% Guaranteed Reduction": return GuaranteedReduction * 100.0f;
                case "% Chance to be Crit": return ((float)Math.Round(CritVulnerability * 100.0f, 2));
                case "% Avoidance": return AnyMiss * 100.0f;
                case "% Avoidance+Block": return AnyAvoid * 100.0f;

                case "Threat/sec": return ThreatPerSecond;
                case "% Chance to be Avoided": return AvoidedAttacks * 100.0f;
                case "% Chance to be Parried": return ParriedAttacks * 100.0f;
                case "% Chance to be Dodged": return DodgedAttacks * 100.0f;
                case "% Chance to Miss": return MissedAttacks * 100.0f;
                
                case "Burst Time": return BurstTime;
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