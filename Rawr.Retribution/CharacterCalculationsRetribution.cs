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

        private float _whiteDPS;
        public float WhiteDPS
        {
            get { return _whiteDPS; }
            set { _whiteDPS = value; }
        }

        private float _sealDPS;
        public float SealDPS
        {
            get { return _sealDPS; }
            set { _sealDPS = value; }
        }

        private float _windfuryDPS;
        public float WindfuryDPS
        {
            get { return _windfuryDPS; }
            set { _windfuryDPS = value; }
        }

        private float _crusaderDPS;
        public float CrusaderDPS
        {
            get { return _crusaderDPS; }
            set { _crusaderDPS = value; }
        }

        private float judgementDPS;
        public float JudgementDPS
        {
            get { return judgementDPS; }
            set { judgementDPS = value; }
        }

        private float _consecrationDPS;
        public float ConsecrationDPS
        {
            get { return _consecrationDPS; }
            set { _consecrationDPS = value; }
        }

        private float _exorcismDPS;
        public float ExorcismDPS
        {
            get { return _exorcismDPS; }
            set { _exorcismDPS= value; }
        }

        private int _targetLevel;
        public int TargetLevel
        {
            get { return _targetLevel; }
            set { _targetLevel = value; }
        }

        private float _weaponDamage;
        public float WeaponDamage
        {
            get { return _weaponDamage; }
            set { _weaponDamage = value; }
        }

        private float _attackSpeed;
        public float AttackSpeed
        {
            get { return _attackSpeed; }
            set { _attackSpeed = value; }
        }

        private float _critChance;
        public float CritChance
        {
            get { return _critChance; }
            set { _critChance = value; }
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

        private float _enemyMitigation;
        public float EnemyMitigation
        {
            get { return _enemyMitigation; }
            set { _enemyMitigation = value; }
        }

        private Stats _basicStats;
        public Stats BasicStats
        {
            get { return _basicStats; }
            set { _basicStats = value; }
        }

        public List<Buff> ActiveBuffs { get; set; }

        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            float critRating = BasicStats.CritRating;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Judgement of the Crusade")))
                critRating -= 3444f / 52f;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Leader of the Pack")))
                critRating -= 22.08f * 5;

            float hitRating = BasicStats.HitRating;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Faerie Fire")))
                hitRating -= 47.3077f;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Heroic Presence")))
                hitRating -= 15.769f;

            float armorPenetration = BasicStats.ArmorPenetration;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Faerie Fire")))
                armorPenetration -= 610f;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Sunder Armor (x5)")))
                armorPenetration -= 2600f;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Curse of Recklessness")))
                armorPenetration -= 800f;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Expose Armor (5cp)")))
                armorPenetration -= 2050f;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Expose Armor (5cp)")))
                armorPenetration -= 1025f;

            float attackPower = BasicStats.AttackPower;
            if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Hunter's Mark")))
                attackPower -= 110f * (1f + BasicStats.BonusAttackPowerMultiplier);

            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Health", BasicStats.Health.ToString("N2"));
            dictValues.Add("Strength", BasicStats.Strength.ToString("N2"));
            dictValues.Add("Agility", BasicStats.Agility.ToString("N2"));
            dictValues.Add("Attack Power", attackPower.ToString("N2"));
            dictValues.Add("Crit Rating", critRating.ToString("N2"));
            dictValues.Add("Hit Rating", hitRating.ToString("N2"));
            dictValues.Add("Expertise", BasicStats.Expertise.ToString("N2"));
            dictValues.Add("Haste Rating", BasicStats.HasteRating.ToString("N2"));
            dictValues.Add("Armor Penetration", armorPenetration.ToString());

            dictValues.Add("Weapon Damage", WeaponDamage.ToString("N2"));
            dictValues.Add("Attack Speed", AttackSpeed.ToString("N2"));
            dictValues.Add("Crit Chance", CritChance.ToString("N2") + "%");
            dictValues.Add("Avoided Attacks", AvoidedAttacks.ToString("N2") + "%");
            dictValues.Add("Enemy Mitigation", EnemyMitigation.ToString("N2") + "%");

            dictValues.Add("White", WhiteDPS.ToString("N2"));
            dictValues.Add("Seal", SealDPS.ToString("N2"));
            dictValues.Add("Windfury", WindfuryDPS.ToString("N2"));
            dictValues.Add("Crusader Strike", CrusaderDPS.ToString("N2"));
            dictValues.Add("Judgement", JudgementDPS.ToString("N2"));
            dictValues.Add("Consecration", ConsecrationDPS.ToString("N2"));
            dictValues.Add("Exorcism", ExorcismDPS.ToString("N2"));
            dictValues.Add("Total DPS", DPSPoints.ToString("N2"));

            return dictValues;
           
        }
    }
}
