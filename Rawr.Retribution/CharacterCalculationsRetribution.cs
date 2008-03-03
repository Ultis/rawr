using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr.Retribution
{
    class CharacterCalculationsRetribution : CharacterCalculationsBase
    {

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
        private float _csDPSPoints;
        public float CSDPSPoints
        {
            get { return _csDPSPoints; }
            set { _csDPSPoints = value; }
        }
        private float _sealDPSPoints;
        public float SealDPSPoints
        {
            get { return _sealDPSPoints; }
            set { _sealDPSPoints = value; }
        }
        private float judgementDPSPoints;
        public float JudgementDPSPoints
        {
            get { return judgementDPSPoints; }
            set { judgementDPSPoints = value; }
        }
        private float _whiteDPSPoints;
        public float WhiteDPSPoints
        {
            get { return _whiteDPSPoints; }
            set { _whiteDPSPoints = value; }
        }
        private float _consDPSPoints;
        public float ConsDPSPoints
        {
            get { return _consDPSPoints; }
            set { _consDPSPoints = value; }
        }
        private float _exoDPSPoints;
        public float ExoDPSPoints
        {
            get { return _exoDPSPoints; }
            set { _exoDPSPoints= value; }
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

        private float _avoidedAttacks;
        public float AvoidedAttacks
        {
            get { return _avoidedAttacks; }
            set { _avoidedAttacks = value; }
        }

        private float _dodgedAttacks;
        public float DodgedAttacks
        {
            get { return _dodgedAttacks; }
            set { _dodgedAttacks = value; }
        }

        private float _missedAttacks;
        public float MissedAttacks
        {
            get { return _missedAttacks; }
            set { _missedAttacks = value; }
        }

        private float _whiteCrit;
        public float WhiteCrit
        {
            get { return _whiteCrit; }
            set { _whiteCrit = value; }
        }

        private float _yellowCrit;
        public float YellowCrit
        {
            get { return _yellowCrit; }
            set { _yellowCrit = value; }
        }

        private float _attackSpeed;
        public float AttackSpeed
        {
            get { return _attackSpeed; }
            set { _attackSpeed = value; }
        }

        private float _armorMitigation;
        public float ArmorMitigation
        {
            get { return _armorMitigation; }
            set { _armorMitigation = value; }
        }

        private float _cycleTime;
        public float CycleTime
        {
            get { return _cycleTime; }
            set { _cycleTime = value; }
        }

        private float _meleeDamage;
        public float MeleeDamage
        {
            get { return _meleeDamage; }
            set { _meleeDamage = value; }
        }
        
        public Character character { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            CalculationsRetribution cr = new CalculationsRetribution();

            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Health", BasicStats.Health.ToString());

            dictValues.Add("Agility", BasicStats.Agility.ToString());
            dictValues.Add("Strength", BasicStats.Strength.ToString());

            dictValues.Add("Attack Power", BasicStats.AttackPower.ToString());

            dictValues.Add("Hit Rating", (BasicStats.HitRating/15.6f).ToString());
            dictValues.Add("Crit Rating", (BasicStats.CritRating/22.08f).ToString());
            dictValues.Add("Expertise Rating", BasicStats.ExpertiseRating.ToString());
            dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString());
            dictValues.Add("Armor Penetration", BasicStats.ArmorPenetration.ToString());
            dictValues.Add("Total DPS", DPSPoints.ToString());
            dictValues.Add("Crusader Strike", CSDPSPoints.ToString());
            dictValues.Add("Seal", SealDPSPoints.ToString());
            dictValues.Add("White", WhiteDPSPoints.ToString());
            dictValues.Add("Judgement", JudgementDPSPoints.ToString());
            dictValues.Add("Consecration", ConsDPSPoints.ToString());
            dictValues.Add("Exorcism", ExoDPSPoints.ToString());

            return dictValues;
           
        }
    }
}
