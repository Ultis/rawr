using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    class CharacterCalculationsTankDK : CharacterCalculationsBase
    {

        public override float OverallPoints {
            get { return Survival * SurvivalWeight + Mitigation + Threat * ThreatWeight; }
            set {   } 
        }

        public Stats BasicStats { get; set; }
        public int TargetLevel { get; set; }

        public float Dodge { get; set; }
        public float Miss { get; set; }
        public float Parry { get; set; }

        public float Survival { get; set; }
        public float Mitigation { get; set; }
        public float Threat { get; set; }

        public float SurvivalWeight { get; set; }
        public float ThreatWeight { get; set; }

        public float AvoidancePreDR { get; set; }
        public float AvoidancePostDR { get; set; }
        public float TotalMitigation { get; set; }
        public float DamageTaken { get; set; }
        public float CritReduction { get; set; }
        public float CappedCritReduction { get; set; }

        public float ArmorDamageReduction { get; set; }

        public float DRDefense { get; set; }
        public float DRParry { get; set; }
        public float DRDodge { get; set; }

        public float Armor { get; set; }

        public float Crit { get; set; }
        public float Defense { get; set; }
        public float DefenseRating { get; set; }
        public float DefenseRatingNeeded { get; set; }

        public float TargetMiss { get; set; }
        public float TargetDodge { get; set; }
        public float TargetParry { get; set; }

        public float Expertise { get; set; }

        private float[] _subPoints = new float[] { 0f, 0f, 0f };
        public override float[] SubPoints
        {
            get { return new float[] { Mitigation, Survival * SurvivalWeight, Threat * ThreatWeight }; }
            set { _subPoints = value; }
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Crit Reduction": return Crit;
                case "Avoidance": return Miss + Parry + Dodge;
                case "Target Miss": return TargetMiss * 100.0f;
                case "Target Parry": return TargetParry * 100.0f;
                case "Target Dodge": return TargetDodge * 100.0f;
                case "Armor Damage Reduction": return ArmorDamageReduction * 100.0f;
                case "Armor": return Armor;
                default:
                    return 0.0f;
            }
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict["DR Defense"] = DRDefense.ToString("F2") + "%";
            dict["DR Parry"] = DRParry.ToString("F2") + "%";
            dict["DR Dodge"] = DRDodge.ToString("F2") + "%";

            dict["Miss"] = Miss.ToString("F2");
            dict["Dodge"] = Dodge.ToString("F2");
            dict["Parry"] = Parry.ToString("F2");
            dict["Armor Damage Reduction"] = (ArmorDamageReduction * 100.0f).ToString("F2") + "%";

            dict["Total Avoidance"] = (Miss + Parry + Dodge).ToString("F2");

            dict["Health"] = BasicStats.Health.ToString("F0");
            dict["Armor"] = BasicStats.Armor.ToString("F0");
            dict["Strength"] = BasicStats.Strength.ToString("F0");
            dict["Agility"] = BasicStats.Agility.ToString("F0");
            dict["Stamina"] = BasicStats.Stamina.ToString("F0");
            dict["Hit Rating"] = BasicStats.HitRating.ToString("F0");
            dict["Haste Rating"] = BasicStats.HasteRating.ToString("F0");
            dict["Crit Rating"] = BasicStats.CritRating.ToString("F0");
            dict["Expertise"] = Expertise.ToString("F0");
            dict["Attack Power"] = BasicStats.AttackPower.ToString("F0");

            dict["Overall Points"] = String.Format("{0:0,0}", (Mitigation + Survival));
            dict["Mitigation Points"] = String.Format("{0:0,0}", Mitigation);
            dict["Survival Points"] = String.Format("{0:0,0}", Survival);

            dict["Crit"] = Crit.ToString("F2");
            dict["Defense"] = Defense.ToString("F0");
            dict["Defense Rating"] = DefenseRating.ToString("F0");
            dict["Defense Rating needed"] = DefenseRatingNeeded.ToString("F0");

            dict["Target Miss"] = (TargetMiss * 100.0f).ToString("F1") + "%";
            dict["Target Dodge"] = (TargetDodge * 100.0f).ToString("F1") + "%";
            dict["Target Parry"] = (TargetParry * 100.0f).ToString("F1") + "%";

            dict["Threat"] = Threat.ToString("F1");
            dict["Overall"] = OverallPoints.ToString("F1");
            dict["Modified Survival"] = (Survival * SurvivalWeight).ToString("F1");
            dict["Modified Mitigation"] = (Mitigation).ToString("F1");
            dict["Modified Threat"] = (Threat * ThreatWeight).ToString("F1");

            return dict;
        }

    }
}
