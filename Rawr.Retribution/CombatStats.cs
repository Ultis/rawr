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

        protected Character _character = new Character();
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
        public float NormalWeaponDamage;

        public float AvengingWrathMulti = 1f;
        public float ArmorReduction = 1f;
        public readonly float PartialResist = 0.953f;

        protected static readonly float[] DodgeChance = { 0.05f, 0.055f, 0.06f, 0.065f };
        protected static readonly float[] MissChance = { 0.05f, 0.052f, 0.054f, 0.08f };
        protected static readonly float[] ResistChance = { 0.04f, 0.05f, 0.06f, 0.17f };
        public static float GetMissChance(float hit, int targetLevel) { return (float)Math.Max(MissChance[targetLevel - 80] - hit, 0f); }
        public static float GetDodgeChance(float expertise, int targetLevel) { return (float)Math.Max(DodgeChance[targetLevel - 80] - expertise * .0025f, 0f); }
        public static float GetResistChance(float spellHit, int targetLevel) { return (float)Math.Max(ResistChance[targetLevel - 80] - spellHit, 0f); }
        public float GetMeleeAvoid()
        {
            return 1f - GetMissChance(_stats.PhysicalHit, _calcOpts.TargetLevel) - GetDodgeChance(_stats.Expertise, _calcOpts.TargetLevel);
        }
        public float GetRangeAvoid()
        {
            return 1f - GetMissChance(Stats.PhysicalHit, _calcOpts.TargetLevel);
        }
        public float GetSpellAvoid()
        {
            return 1f - GetResistChance(Stats.SpellHit, _calcOpts.TargetLevel);
        }

        public void UpdateCalcs()
        {
            float fightLength = _calcOpts.FightLength * 60f;

            float bloodlustUptime = ((float)Math.Floor(fightLength / 300f) * 40f + (float)Math.Min(fightLength % 300f, 40f)) / fightLength;
            float bloodlustHaste = 1f + (bloodlustUptime * .3f);

            float awTimes = (float)Math.Ceiling((fightLength - 20f) / (180f - _talents.SanctifiedWrath * 30f));
            AvengingWrathMulti = 1f + ((awTimes * 20f) / fightLength) * .2f;

            ArmorReduction = 1f - ArmorCalculations.GetDamageReduction(Character.Level, 10645f, Stats.ArmorPenetration, Stats.ArmorPenetrationRating);
            BaseWeaponSpeed = _character.MainHand == null ? 3.5f : _character.MainHand.Speed;
            float baseWeaponDamage = _character.MainHand == null ? 371.5f : (_character.MainHand.MinDamage + _character.MainHand.MaxDamage) / 2f;
            AttackSpeed = BaseWeaponSpeed / ((1f + _stats.PhysicalHaste) * bloodlustHaste);
            WeaponDamage = baseWeaponDamage + _stats.AttackPower * BaseWeaponSpeed / 14f;
            NormalWeaponDamage = WeaponDamage * 3.3f / BaseWeaponSpeed;
        }
    }
}
