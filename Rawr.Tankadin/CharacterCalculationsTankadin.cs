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

        private float _crushAvoidance;
        public float CrushAvoidance
        {
            get { return _crushAvoidance; }
            set { _crushAvoidance = value; }
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

        public float OverallTPS { get; set; }
        public float SoRTPS { get; set; }
        public float JoRTPS { get; set; }
        public float HolyShieldTPS { get; set; }
        public float ConsecrateTPS { get; set; }
        public float MiscTPS { get; set; }

        public float NatureSurvivalPoints { get; set; }
        public float FrostSurvivalPoints { get; set; }
        public float FireSurvivalPoints { get; set; }
        public float ShadowSurvivalPoints { get; set; }
        public float ArcaneSurvivalPoints { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            int armorCap = (int)Math.Ceiling((1402.5f * TargetLevel) - 66502.5f);
            float levelDifference = 0.2f * (TargetLevel - 70);

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Armor", BasicStats.Armor.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Defense", Defense.ToString());
            dictValues.Add("Miss", Miss.ToString() + "%");
            dictValues.Add("Dodge", Dodge.ToString() + "%");
            dictValues.Add("Parry", Parry.ToString() + "%");
            dictValues.Add("Block", Block.ToString() + "%");
            dictValues.Add("Block Value", BlockValue.ToString() + "%");
            dictValues.Add("Avoidance", Avoidance.ToString() + "%");
            dictValues.Add("Mitigation", Mitigation.ToString());
            dictValues.Add("Spell Damage", _basicStats.SpellDamageRating.ToString());
            dictValues.Add("Total Mitigation", TotalMitigation.ToString() + "%");
            if (CritAvoidance == (5f + levelDifference))
                dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritAvoidance).ToString()
                    + "%*Exactly enough defense rating/resilience to be uncrittable by bosses.");
            else if (CritAvoidance < (5f + levelDifference))
                dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritAvoidance).ToString()
                    + string.Format("%*CRITTABLE! Short by {0} defense rating or {1} resilience to be uncrittable by bosses.",
                    Math.Ceiling(((5f + levelDifference) - CritAvoidance) * 60f), Math.Ceiling(((5f + levelDifference) - CritAvoidance) * 39.423f)));
            else
                dictValues.Add("Chance to be Crit", ((5f + levelDifference) - CritAvoidance).ToString()
                    + string.Format("%*Uncrittable by bosses. {0} defense rating or {1} resilience over the crit cap.",
                    Math.Floor(((5f + levelDifference) - CritAvoidance) * -60f), Math.Floor(((5f + levelDifference) - CritAvoidance) * -39.423f)));
            dictValues.Add("Overall Points", OverallPoints.ToString());
            dictValues.Add("Mitigation Points", MitigationPoints.ToString());
            dictValues.Add("Survival Points", SurvivalPoints.ToString());
            dictValues.Add("Overall", Math.Round(OverallTPS) + " tps");
            dictValues.Add("Holy Shield", Math.Round(HolyShieldTPS) + " tps");
            dictValues.Add("Seal of Right", Math.Round(SoRTPS) + " tps");
            dictValues.Add("Judgement of Right", Math.Round(JoRTPS) + " tps");
            dictValues.Add("Consecrate", Math.Round(ConsecrateTPS) + " tps");
            dictValues.Add("Misc", Math.Round(MiscTPS) + " tps");

            return dictValues;
        }
    }
}
