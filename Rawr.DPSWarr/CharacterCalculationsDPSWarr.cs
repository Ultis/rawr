using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.DPSWarr
{
    class CharacterCalculationsDPSWarr : CharacterCalculationsBase
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
        private float _msDPSPoints;
        public float MSDPSPoints
        {
            get { return _msDPSPoints; }
            set { _msDPSPoints = value; }
        }
        private float _wwDPSPoints;
        public float WWDPSPoints
        {
            get { return _wwDPSPoints; }
            set { _wwDPSPoints = value; }
        }
        private float slamDPSPoints;
        public float SlamDPSPoints
        {
            get { return slamDPSPoints; }
            set { slamDPSPoints = value; }
        }
        private float _whiteDPSPoints;
        public float WhiteDPSPoints
        {
            get { return _whiteDPSPoints; }
            set { _whiteDPSPoints = value; }
        }
        private float _wfDPSPoints;
        public float WFDPSPoints
        {
            get { return _wfDPSPoints; }
            set { _wfDPSPoints = value; }
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
        private float _hastedSpeed;
        public float HastedSpeed
        {
            get { return _hastedSpeed; }
            set { _hastedSpeed = value; }
        }        
        public Character character { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            CalculationsDPSWarr cr = new CalculationsDPSWarr();

            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Health", BasicStats.Health.ToString("N2"));

            dictValues.Add("Agility", BasicStats.Agility.ToString("N2"));
            dictValues.Add("Strength", BasicStats.Strength.ToString("N2"));

            dictValues.Add("Attack Power", BasicStats.AttackPower.ToString("N2"));

            dictValues.Add("Hit", BasicStats.HitRating.ToString() + " (" + (BasicStats.HitRating / 15.76f).ToString("N2") +"% )");
            dictValues.Add("Crit", BasicStats.CritRating.ToString() + " (" + (BasicStats.CritRating /22.08f).ToString("N2") +"% )");
            dictValues.Add("Expertise Rating", BasicStats.ExpertiseRating.ToString("N2")+" ("+(Math.Floor(BasicStats.ExpertiseRating/3.89f)).ToString("N2")+" )");
            dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString("N2"));
            dictValues.Add("Armor Penetration", BasicStats.ArmorPenetration.ToString());
            dictValues.Add("Hasted Speed", HastedSpeed.ToString("N2"));
            dictValues.Add("Total DPS", DPSPoints.ToString("N2"));
            dictValues.Add("Mortal Strike", MSDPSPoints.ToString("N2"));
            dictValues.Add("Slam", SlamDPSPoints.ToString("N2"));
            dictValues.Add("White", WhiteDPSPoints.ToString("N2"));
            dictValues.Add("Whirlwind", WWDPSPoints.ToString("N2"));
            dictValues.Add("Windfury", WFDPSPoints.ToString("N2"));
            dictValues.Add("Weapon Damage", BasicStats.WeaponDamage.ToString("N2"));

            return dictValues;
           
        }
    }
}
