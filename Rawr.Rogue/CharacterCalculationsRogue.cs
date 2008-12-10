using System;
using System.Collections.Generic;

namespace Rawr.Rogue
{
    public class CharacterCalculationsRogue : CharacterCalculationsBase
    {
        #region Points
        private float _overallPoints = 0f;
        public override float OverallPoints
        {
            get { return _overallPoints; }
            set { _overallPoints = value; }
        }

        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return _subPoints; }
            set { _subPoints = value; }
        }

        public float DPSPoints
        {
            get { return _subPoints[0]; }
            set { _subPoints[0] = value; }
        }

        private float _whiteDPS = 0f;
        public float WhiteDPS
        {
            get { return _whiteDPS; }
            set { _whiteDPS = value; }
        }

        private float _cpgDPS = 0f;
        public float CPGDPS
        {
            get { return _cpgDPS; }
            set { _cpgDPS = value; }
        }

        private float _finisherDPS = 0f;
        public float FinisherDPS
        {
            get { return _finisherDPS; }
            set { _finisherDPS = value; }
        }

        private float _windfuryDPS = 0f;
        public float WindfuryDPS
        {
            get { return _windfuryDPS; }
            set { _windfuryDPS = value; }
        }

        private float _swordspecDPS = 0f;
        public float SwordSpecDPS
        {
            get { return _swordspecDPS; }
            set { _swordspecDPS = value; }
        }

        private float _poisonDPS = 0f;
        public float PoisonDPS
        {
            get { return _poisonDPS; }
            set { _poisonDPS = value; }
        }
        #endregion

        #region Basic Stats
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
        #endregion

        #region Offensive Stats
        private float _crit;
        public float Crit
        {
            get { return _crit; }
            set { _crit = value; }
        }
        #endregion
        
        
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();

            float levelDifference = TargetLevel - 70;

            dictValues.Add("Health", BasicStats.Health.ToString());
            dictValues.Add("Stamina", BasicStats.Stamina.ToString());
            dictValues.Add("Strength", BasicStats.Strength.ToString());
            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Attack Power", BasicStats.AttackPower.ToString());
            dictValues.Add("Hit", Math.Round(BasicStats.HitRating * RogueConversions.HitRatingToHit + BasicStats.PhysicalHit, 2).ToString() + string.Format("%*Hit Rating {0}", BasicStats.HitRating));
            dictValues.Add("Expertise", (Math.Round(BasicStats.ExpertiseRating * RogueConversions.ExpertiseRatingToExpertise + BasicStats.Expertise)).ToString() + string.Format("%*Expertise Rating {0}", BasicStats.ExpertiseRating));
            dictValues.Add("Haste", Math.Round(BasicStats.HasteRating * RogueConversions.HasteRatingToHaste, 2).ToString() + string.Format("%*Haste Rating {0}", BasicStats.HasteRating));
            dictValues.Add("Armor Penetration", BasicStats.ArmorPenetration.ToString());
            dictValues.Add("Crit", Math.Round(BasicStats.CritRating * RogueConversions.CritRatingToCrit + BasicStats.PhysicalCrit, 2).ToString() + string.Format("%*Crit Rating {0}", BasicStats.CritRating));
            dictValues.Add("Weapon Damage", BasicStats.WeaponDamage.ToString());
            dictValues.Add("White DPS", _whiteDPS.ToString());
            dictValues.Add("CPG DPS", _cpgDPS.ToString());
            dictValues.Add("Finisher DPS", _finisherDPS.ToString());
            dictValues.Add("Windfury DPS", _windfuryDPS.ToString());
            dictValues.Add("Sword Spec DPS", _swordspecDPS.ToString());
            dictValues.Add("Poison DPS", _poisonDPS.ToString());
            dictValues.Add("Total DPS", _subPoints[0].ToString());

            return dictValues;
        }

        public override float GetOptimizableCalculationValue(string calculation)
        {
            /* "Health",
             * "Haste Rating",
             * "Expertise Rating",
             * "Hit Rating",
             * "Agility",
             * "Attack Power"
             */
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "Haste Rating": return BasicStats.HasteRating;
                case "Expertise Rating": return BasicStats.ExpertiseRating;
                case "Hit Rating": return BasicStats.HitRating;
                case "Agility": return BasicStats.Agility;
                case "Attack Power": return BasicStats.AttackPower;
            }

            return 0f;
        }
    }
}
