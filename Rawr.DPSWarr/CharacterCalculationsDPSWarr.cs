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
        private float _btDPSPoints;
        public float BTDPSPoints
        {
            get { return _btDPSPoints; }
            set { _btDPSPoints = value; }
        }
        private float _hsDPSPoints;
        public float HSDPSPoints
        {
            get { return _hsDPSPoints; }
            set { _hsDPSPoints = value; }
        }

        private float _deepWoundsDPSPoints;
        public float DeepWoundsDPSPoints
        {
            get { return _deepWoundsDPSPoints; }
            set { _deepWoundsDPSPoints = value; }
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
        private float _whiteHits;
        public float WhiteHits
        {
            get { return _whiteHits; }
            set { _whiteHits = value; }
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
        private float _hastedOffSpeed;
        public float HastedOffSpeed
        {
            get { return _hastedOffSpeed; }
            set { _hastedOffSpeed = value; }
        }
        private float _hastedMainSpeed;
        public float HastedMainSpeed
        {
            get { return _hastedMainSpeed; }
            set { _hastedMainSpeed = value; }
        }
        private float _flurryUptime;
        public float FlurryUptime
        {
            get { return _flurryUptime; }
            set { _flurryUptime = value; }
        }

        public Character character { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            CalculationsDPSWarr cr = new CalculationsDPSWarr();

            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Health", BasicStats.Health.ToString("N2"));
            dictValues.Add("Armor", BasicStats.Armor.ToString("N2"));
            dictValues.Add("Agility", BasicStats.Agility.ToString("N2"));
            dictValues.Add("Strength", BasicStats.Strength.ToString("N2"));
            dictValues.Add("Attack Power", BasicStats.AttackPower.ToString("N2"));

            dictValues.Add("Hit", BasicStats.HitRating.ToString() + " (" + (BasicStats.HitRating / CalculationsDPSWarr.fHitRatingPerPercent).ToString("N2") + "%/8%)");
            dictValues.Add("Crit", BasicStats.CritRating.ToString() + " (" + (BasicStats.CritRating / CalculationsDPSWarr.fCritRatingPerPercent).ToString("N2") + "% )");
            dictValues.Add("Expertise Rating", BasicStats.ExpertiseRating.ToString("N2") + " (" + 
                (BasicStats.ExpertiseRating / CalculationsDPSWarr.fExpertiseRatingPerPercent / 4.0f).ToString("N2") + "%/6.5%)");
            
            dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString("N2"));
            dictValues.Add("Armor Mitigation", ArmorMitigation.ToString("N2") + "%");
            dictValues.Add("Armor Penetration Rating", BasicStats.ArmorPenetrationRating.ToString() + "/" +
                (BasicStats.ArmorPenetrationRating / CalculationsDPSWarr.fArmorPen).ToString("N2") + "%");

            dictValues.Add("Flurry Uptime", FlurryUptime.ToString("N2") + "%");

            dictValues.Add("Bloodthirst DPS", BTDPSPoints.ToString("N2") + " / " + (BTDPSPoints / DPSPoints * 100.0f).ToString("N2") + "%");
            dictValues.Add("Heroic Strike DPS", HSDPSPoints.ToString("N2") + " / " + (HSDPSPoints / DPSPoints * 100.0f).ToString("N2") + "%");
            dictValues.Add("Whirlwind DPS", WWDPSPoints.ToString("N2") + " / " + (WWDPSPoints / DPSPoints * 100.0f).ToString("N2") + "%");
            dictValues.Add("White DPS", WhiteDPSPoints.ToString("N2") + " / " + (WhiteDPSPoints / DPSPoints * 100.0f).ToString("N2") + "%");
            dictValues.Add("Deep Wounds DPS", DeepWoundsDPSPoints.ToString("N2")  + " / " + (DeepWoundsDPSPoints / DPSPoints * 100.0f).ToString("N2") + "%");
            dictValues.Add("Slam DPS", SlamDPSPoints.ToString("N2") + " / " + (SlamDPSPoints / DPSPoints * 100.0f).ToString("N2") + "%");
            dictValues.Add("Total DPS", DPSPoints.ToString("N2"));


            float nTotalHits = WhiteHits + WhiteCrit + DodgedAttacks + MissedAttacks;
            dictValues.Add("White Hits", WhiteHits.ToString("N2") + " / " + ((float)WhiteHits / nTotalHits * 100.0f).ToString("N2") + "%" );
            dictValues.Add("White Crits", WhiteCrit.ToString("N2") + " / " + ((float)WhiteCrit / nTotalHits * 100.0f).ToString("N2") + "%");
            dictValues.Add("White Dodges", DodgedAttacks.ToString("N2") + " / " + ((float)DodgedAttacks / nTotalHits * 100.0f).ToString("N2") + "%");
            dictValues.Add("White Misses", MissedAttacks.ToString("N2") + " / " + ((float)MissedAttacks / nTotalHits * 100.0f).ToString("N2") + "%");

            return dictValues;
           
        }
    }
}
