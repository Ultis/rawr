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

        private float _defense;
        public float Defense
        {
            get { return _defense; }
            set { _defense = value; }
        }

        private float _dodge;
        public float Dodge
        {
            get { return _dodge; }
            set { _dodge = value; }
        }

        private float _miss;
        public float Miss
        {
            get { return _miss; }
            set { _miss = value; }
        }

        private float _parry;
        public float Parry
        {
            get { return _parry; }
            set { _parry = value; }
        }

        private float _block;
        public float Block
        {
            get { return _block; }
            set { _block = value; }
        }

        private float _blockvalue;
        public float BlockValue
        {
            get { return _blockvalue; }
            set { _blockvalue = value; }
        }
        
        private float _mitigation;
        public float Mitigation
        {
            get { return _mitigation; }
            set { _mitigation = value; }
        }

        private float _avoidance;
        public float Avoidance
        {
            get { return _avoidance; }
            set { _avoidance = value; }
        }

        private float _totalMitigation;
        public float TotalMitigation
        {
            get { return _totalMitigation; }
            set { _totalMitigation = value; }
        }

        private float _damageTaken;
        public float DamageTaken
        {
            get { return _damageTaken; }
            set { _damageTaken = value; }
        }

        private float _critAvoidance;
        public float CritAvoidance
        {
            get { return _critAvoidance; }
            set { _critAvoidance = value; }
        }



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
