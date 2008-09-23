using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Tankadin
{
    public class CharacterCalculationsTankadin : CharacterCalculationsBase
    {
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

        private Stats _basicStats;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }

        private int _targetLevel;
        public int TargetLevel
        {
            get { return _targetLevel; }
            set { _targetLevel = value; }
        }

        public float Armor { get; set; }
        public float ArmorReduction { get; set; }
        public float DamageReduction { get; set; }
        public float Defense { get; set; }
        public float Dodge { get; set; }
        public float Miss { get; set; }
        public float Parry { get; set; }
        public float Block { get; set; }
        public float BlockValue { get; set; }
        public float Mitigation { get; set; }
        public float Avoidance { get; set; }
        public float DamageTaken { get; set; }
        public float CritAvoidance { get; set; }

        public float ToMiss { get; set; }
        public float ToDodge { get; set; }
        public float ToParry { get; set; }
        public float ToResist { get; set; }
        public float ToLand { get; set; }


        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Chance to be Crit", (5f - CritAvoidance).ToString()
                    + string.Format("%*CRITTABLE! Short by {0} defense rating or {1} resilience to be uncrittable by bosses.",
                    Math.Ceiling((5f - CritAvoidance) * 60f), Math.Ceiling((5f - CritAvoidance) * 39.423f)));

            return dict;
        }
    }
}
