using System;
using System.Collections.Generic;
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

        private float _sealOfBloodDPS;
        public float SealOfBloodDPS
        {
            get { return _sealOfBloodDPS; }
            set { _sealOfBloodDPS = value; }
        }

        private float _sealOfCommandDPS;
        public float SealOfCommandDPS
        {
            get { return _sealOfCommandDPS; }
            set { _sealOfCommandDPS = value; }
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

        private float _divineStormDPS;
        public float DivineStormDPS
        {
            get { return _divineStormDPS; }
            set { _divineStormDPS = value; }
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
            //float critRating = BasicStats.CritRating;
            //if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Judgement of the Crusade")))
            //    critRating -= 3444f / 52f;
            //if (ActiveBuffs.Contains(Buff.GetBuffByName("Leader of the Pack")))
            //    critRating -= 22.08f * 5;

            //float hitRating = BasicStats.HitRating;
            //if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Faerie Fire")))
            //    hitRating -= 47.3077f;
            //if (ActiveBuffs.Contains(Buff.GetBuffByName("Heroic Presence")))
            //    hitRating -= 15.769f;

            //float armorPenetration = BasicStats.ArmorPenetration;
			//if (ActiveBuffs.Contains(Buff.GetBuffByName("Faerie Fire")))
			//    armorPenetration -= 610f;
			//if (ActiveBuffs.Contains(Buff.GetBuffByName("Sunder Armor (x5)")))
			//    armorPenetration -= 2600f;
			//if (ActiveBuffs.Contains(Buff.GetBuffByName("Curse of Recklessness")))
			//    armorPenetration -= 800f;
			//if (ActiveBuffs.Contains(Buff.GetBuffByName("Expose Armor (5cp)")))
			//    armorPenetration -= 2050f;
			//if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Expose Armor (5cp)")))
			//    armorPenetration -= 1025f;

            //float attackPower = BasicStats.AttackPower;
            //if (ActiveBuffs.Contains(Buff.GetBuffByName("Improved Hunter's Mark")))
            //    attackPower -= 110f * (1f + BasicStats.BonusAttackPowerMultiplier);

            //float effectiveArmor = 10557.5f / ((1f / EnemyMitigation) - 1f);

            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            dictValues.Add("Health", BasicStats.Health.ToString("N0"));
            dictValues.Add("Strength", BasicStats.Strength.ToString("N0"));
            dictValues.Add("Agility", string.Format("{0:0}*Provides {1:P} crit chance", BasicStats.Agility, (BasicStats.Agility / 6250f)));
			dictValues.Add("Attack Power", BasicStats.AttackPower.ToString("N0"));
			dictValues.Add("Crit Rating", string.Format("{0:0}*Provides {1:P} crit chance", BasicStats.CritRating, (BasicStats.CritRating / 4591f)));
			dictValues.Add("Hit Rating", string.Format("{0:0}*Negates {1:P} miss chance and {2:P} spell resist chance", BasicStats.HitRating, (BasicStats.HitRating / 3279f), (BasicStats.HitRating / 2623f)));
			dictValues.Add("Expertise", string.Format("{0:0}*Negates {1:P} dodge chance", BasicStats.Expertise, (BasicStats.Expertise / 400f)));
			dictValues.Add("Haste Rating", string.Format("{0:0}*Increases attack speed by {1:P}", BasicStats.HasteRating, (BasicStats.HasteRating / 1576f)));
			dictValues.Add("Armor Penetration", BasicStats.ArmorPenetration.ToString("N0"));
			dictValues.Add("Armor Penetration Rating", string.Format("{0:0}*Reduces target armor by {1:P}", BasicStats.ArmorPenetrationRating, (BasicStats.ArmorPenetrationRating / 1540f)));

            dictValues.Add("Weapon Damage", WeaponDamage.ToString("N2"));
            dictValues.Add("Attack Speed", AttackSpeed.ToString("N2"));
            dictValues.Add("Crit Chance", string.Format("{0:P}", CritChance));
            dictValues.Add("Avoided Attacks", string.Format("{0:P}*{1:P} Dodged, {2:P} Missed", AvoidedAttacks, DodgedAttacks, MissedAttacks));
            dictValues.Add("Enemy Mitigation", string.Format("{0:P}", EnemyMitigation));

            dictValues.Add("White", WhiteDPS.ToString("N2"));
            dictValues.Add("Seal", SealDPS.ToString("N2"));
            dictValues.Add("Windfury", WindfuryDPS.ToString("N2"));
            dictValues.Add("Crusader Strike", CrusaderDPS.ToString("N2"));
            dictValues.Add("Judgement", JudgementDPS.ToString("N2"));
            dictValues.Add("Consecration", ConsecrationDPS.ToString("N2"));
            dictValues.Add("Exorcism", ExorcismDPS.ToString("N2"));
            dictValues.Add("Divine Storm", DivineStormDPS.ToString("N2"));
            dictValues.Add("Total DPS", DPSPoints.ToString("N2"));

            return dictValues;
        }
    }
}
