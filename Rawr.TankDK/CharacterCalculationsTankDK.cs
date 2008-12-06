using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.TankDK
{
    class CharacterCalculationsTankDK : CharacterCalculationsBase
    {

        public override float OverallPoints {
            get { return Mitigation + Avoidance; }
            set {   } 
        }

        public Stats BasicStats { get; set; }
        public int TargetLevel { get; set; }

        public float Dodge { get; set; }
        public float Miss { get; set; }
        public float Parry { get; set; }

        public float Mitigation { get; set; }
        public float Avoidance { get; set; }

        public float AvoidancePreDR { get; set; }
        public float AvoidancePostDR { get; set; }
        public float TotalMitigation { get; set; }
        public float DamageTaken { get; set; }
        public float CritReduction { get; set; }
        public float CappedCritReduction { get; set; }

        public float Armor { get; set; }

        public float Crit { get; set; }
        public float Defense { get; set; }
        public float DefenseRating { get; set; }
        public float DefenseRatingNeeded { get; set; }

        private float[] _subPoints = new float[] { 0f, 0f };
        public override float[] SubPoints
        {
            get { return new float[] { Avoidance, Mitigation }; }
            set { _subPoints = value; }
        }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            
            dict["Miss"] = Miss.ToString("F2");
            dict["Dodge"] = Dodge.ToString("F2");
            dict["Parry"] = Parry.ToString("F2");

            dict["Health"] = BasicStats.Health.ToString("F0");
            dict["Armor"] = BasicStats.Armor.ToString("F0");
            dict["Strength"] = BasicStats.Strength.ToString("F0");
            dict["Agility"] = BasicStats.Agility.ToString("F0");

            dict["TTL"] = (Mitigation + Avoidance).ToString("F1");
            dict["TTL Avoidance"] = Avoidance.ToString("F1");
            dict["TTL Mitigation"] = Mitigation.ToString("F1");

            dict["Crit"] = Crit.ToString("F2");
            dict["Defense"] = Defense.ToString("F0");
            dict["Defense Rating"] = DefenseRating.ToString("F0");
            dict["Defense Rating needed"] = DefenseRatingNeeded.ToString("F0");

            return dict;
        }

    }
}
