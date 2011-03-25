using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class CombatStats
    {
        public CombatStats(Character character, Stats stats)
        {
            _stats = stats;
            _character = character;
            _calcOpts = _character.CalculationOptions as CalculationOptionsRetribution;
            _talents = _character.PaladinTalents;
            UpdateCalcs();
        }

        protected Character _character = new Character() { IsLoading = false };
        public Character Character { get { return _character; } }

        protected Stats _stats = new Stats();
        public Stats Stats { get { return _stats; } }

        protected CalculationOptionsRetribution _calcOpts = new CalculationOptionsRetribution();
        public CalculationOptionsRetribution CalcOpts { get { return _calcOpts; } }

        protected PaladinTalents _talents = new PaladinTalents();
        public PaladinTalents Talents { get { return _talents; } }

        public float WeaponDamage;
        public float BaseWeaponSpeed;
        public float AttackSpeed;
        public float WeaponDamageNormalized;

        public float AvengingWrathMulti = 1f;
        public float ArmorReduction = 1f;

        private float BloodlustHaste = 0;
        private float BaseWeaponDamage = 0;

        public float GetMasteryTotalPercent()
        {
            return PaladinConstants.HOL_BASE + StatConversion.GetMasteryFromRating(_stats.MasteryRating, CharacterClass.Paladin) * PaladinConstants.HOL_COEFF;
        }

        public float GetAttackSpeed(float BaseHaste)
        {
            return BaseWeaponSpeed / ((1f + BaseHaste) * BloodlustHaste);
        }

        public float GetAttackSpeed()
        {
            return GetAttackSpeed(_stats.PhysicalHaste);
        }

        public float GetWeaponDamageNormalized(float Attackpower)
        {
            return BaseWeaponDamage + Attackpower * 3.3f / 14f;
        }

        public float GetWeaponDamage(float Attackpower)
        {
            return BaseWeaponDamage + Attackpower * BaseWeaponSpeed / 14f;
        }

        public float GetWeaponDamage()
        {
            return GetWeaponDamage(_stats.AttackPower);
        }

        public void UpdateCalcs()
        {
            float fightLength = _character.BossOptions.BerserkTimer;

            float bloodlustUptime = ((float)Math.Floor(fightLength / 600f) * 40f + (float)Math.Min(fightLength % 600f, 40f)) / fightLength;
            BloodlustHaste = 1f + (Character.ActiveBuffsContains("Heroism/Bloodlust")
                                   || Character.ActiveBuffsContains("Time Warp")
                                   || Character.ActiveBuffsContains("Ancient Hysteria")
                                   ? (bloodlustUptime * .3f) : 0f);

            float awUptime = (float)Math.Ceiling((fightLength - 20f) / (180f - _talents.SanctifiedWrath * 30f)) * 20f / fightLength;
            AvengingWrathMulti = 1f + awUptime * .2f;

            ArmorReduction = 1f - StatConversion.GetArmorDamageReduction(Character.Level, _character.BossOptions.Armor, Stats.TargetArmorReduction, Stats.ArmorPenetration); ;

            BaseWeaponSpeed = (_character.MainHand == null || _character.MainHand.Speed == 0.0f) ? 3.5f : _character.MainHand.Speed; // NOTE by Kavan: added a check against speed == 0, it can happen when item data is still being downloaded
            BaseWeaponDamage = _character.MainHand == null ? 371.5f : (_character.MainHand.MinDamage + _character.MainHand.MaxDamage) / 2f;
            AttackSpeed = GetAttackSpeed();
            WeaponDamage = BaseWeaponDamage + _stats.AttackPower * BaseWeaponSpeed / 14f;
            WeaponDamageNormalized = BaseWeaponDamage + _stats.AttackPower * 3.3f / 14f;
        }
    }
}
