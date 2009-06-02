using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    // Reminder: This is the character totals based on all gear and talents.  Apply the weights here.
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

        public float TotalMitigation { get; set; }  // What's the difference between this and Mitigation above?
        public float DamageTaken { get; set; }

        public float ArmorDamageReduction { get; set; }
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
                case "Chance to be Crit": return Crit; // Def cap chance to be critted by boss.  For optimization this needs to be  <= 0
                case "Avoidance %": return (Miss + Parry + Dodge); // Another duplicat math location?
                case "Target Miss %": return TargetMiss * 100.0f; 
                case "Target Parry %": return TargetParry * 100.0f; // Expertise related.
                case "Target Dodge %": return TargetDodge * 100.0f; // Expertise related.
                case "Damage Reduction %": return ArmorDamageReduction * 100.0f; // % Damage reduction by Armor
                case "Armor": return Armor; // Raw Armor
                default:
                    return 0.0f;
            }
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict["Miss"] = Miss.ToString("F2") + "%";
            dict["Dodge"] = Dodge.ToString("F2") + "%";
            dict["Parry"] = Parry.ToString("F2") + "%";
            dict["Armor Damage Reduction"] = (ArmorDamageReduction * 100.0f).ToString("F2") + "%";

            dict["Total Avoidance"] = (Miss + Parry + Dodge).ToString("F2") + "%"; // Another duplicate math location.

            dict["Health"] = BasicStats.Health.ToString("F0");
            dict["Armor"] = BasicStats.Armor.ToString("F0");
            dict["Strength"] = BasicStats.Strength.ToString("F0");
            dict["Agility"] = BasicStats.Agility.ToString("F0");
            dict["Stamina"] = BasicStats.Stamina.ToString("F0");
            dict["Hit Rating"] = BasicStats.HitRating.ToString("F0");
            dict["Haste Rating"] = BasicStats.HasteRating.ToString("F0");
            dict["Crit Rating"] = BasicStats.CritRating.ToString("F0");
            dict["Physical Crit"] = (BasicStats.PhysicalCrit * 100f).ToString("F2");
            dict["Expertise"] = Expertise.ToString("F0");
            dict["Attack Power"] = BasicStats.AttackPower.ToString("F0");
            dict["Armor Penetration"] = BasicStats.ArmorPenetration.ToString("F0");
            dict["Armor Penetration Rating"] = BasicStats.ArmorPenetrationRating.ToString("F0");

            dict["Overall Points"] = String.Format("{0:0.0}", (Mitigation + Survival));
            // Modify above to:
            //dict["Overall Points"] = OverallPoints.ToString("F1"); 
            dict["Mitigation Points"] = String.Format("{0:0.0}", Mitigation); // Unmodified Mitigation.
            dict["Survival Points"] = String.Format("{0:0.0}", Survival); // Unmodified Survival

            dict["Crit"] = Crit.ToString("F2");
            dict["Defense"] = Defense.ToString("F0");
            dict["Defense Rating"] = DefenseRating.ToString("F0");
            dict["Defense Rating needed"] = DefenseRatingNeeded.ToString("F0");

            dict["Target Miss"] = (TargetMiss * 100.0f).ToString("F1") + "%";
            dict["Target Dodge"] = (TargetDodge * 100.0f).ToString("F1") + "%";
            dict["Target Parry"] = (TargetParry * 100.0f).ToString("F1") + "%";

            dict["Threat"] = Threat.ToString("F1"); // Unmodified Threat.
            dict["Overall"] = OverallPoints.ToString("F1");  
            dict["Modified Survival"] = (Survival * SurvivalWeight).ToString("F1"); // another place of duplicate math.
            dict["Modified Mitigation"] = (Mitigation).ToString("F1");
            dict["Modified Threat"] = (Threat * ThreatWeight).ToString("F1"); // another place of duplicate math.

            return dict;
        }

    }
}
