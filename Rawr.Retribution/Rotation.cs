using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public abstract class Rotation
    {

        protected Ability cs;
        protected Ability judge;
        protected Ability ds;
        protected Ability exo;
        protected Ability how;
        protected Ability cons;
        protected Ability seal;
        protected White white;

        public Rotation()
        {
            cs = new CrusaderStrike();
            judge = new Judgement();
            ds = new DivineStorm();
            exo = new Exorcism();
            how = new HammerOfWrath();
            cons = new Consecration();
            seal = new SealOfBlood();
            white = new White();
        }

        public abstract void SetAbilityDPS(CharacterCalculationsRetribution calc);

        public void SetDPS(CharacterCalculationsRetribution calc)
        {
            calc.WhiteDPS = white.WhiteDPS();
            SetAbilityDPS(calc);
            
            calc.DPSPoints =
                calc.WhiteDPS +
                calc.SealDPS +
                calc.JudgementDPS +
                calc.CrusaderStrikeDPS +
                calc.DivineStormDPS +
                calc.ExorcismDPS +
                calc.ConsecrationDPS +
                calc.HammerOfWrathDPS;
        }

        public abstract float GetMeleeAttacksPerSec();
        public abstract float GetPhysicalAttacksPerSec();

        public float GetMeleeCritsPerSec()
        {
            return GetMeleeAttacksPerSec() * Ability.Stats.PhysicalCrit * Ability.GetMeleeAvoid();
        }

        public float GetPhysicalCritsPerSec()
        {
            return GetPhysicalAttacksPerSec() * Ability.Stats.PhysicalCrit * Ability.GetMeleeAvoid();
        }

    }

    public class Simulator : Rotation
    {

        public RotationSolution Solution { get; set; }

        public Simulator(RotationParameters rot)
            : base()
        {
            Solution = RotationSimulator.SimulateRotation(rot);
        }

        public override void SetAbilityDPS(CharacterCalculationsRetribution calc)
        {
            calc.Rotation = Solution;

            calc.SealDPS = seal.AverageDamage() * GetMeleeAttacksPerSec() * Ability.GetMeleeAvoid();
            calc.JudgementDPS = judge.AverageDamage() * Solution.Judgement / Solution.FightLength;
            calc.CrusaderStrikeDPS = cs.AverageDamage() * Solution.CrusaderStrike / Solution.FightLength;
            calc.DivineStormDPS = ds.AverageDamage() * Solution.DivineStorm / Solution.FightLength;
            calc.ConsecrationDPS = cons.AverageDamage() * Solution.Consecration / Solution.FightLength;
            calc.ExorcismDPS = exo.AverageDamage() * Solution.Exorcism / Solution.FightLength;
            calc.HammerOfWrathDPS = how.AverageDamage() * Solution.HammerOfWrath / Solution.FightLength;
        }

        public override float GetMeleeAttacksPerSec()
        {
            return (Solution.CrusaderStrike + Solution.DivineStorm) / Solution.FightLength + 1f / Ability.AttackSpeed;
        }

        public override float GetPhysicalAttacksPerSec()
        {
            return GetMeleeAttacksPerSec() + (Solution.Judgement + Solution.HammerOfWrath) / Solution.FightLength;
        }

    }

    public class EffectiveCooldown : Rotation
    {

        private readonly CalculationOptionsRetribution _calcOpts;

        public EffectiveCooldown(CalculationOptionsRetribution calcOpts)
            : base()
        {
            _calcOpts = calcOpts;
        }

        public override void SetAbilityDPS(CharacterCalculationsRetribution calc)
        {

            calc.Rotation = new RotationSolution();
            calc.Rotation.JudgementCD = _calcOpts.JudgeCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.JudgeCD20 * _calcOpts.TimeUnder20;
            calc.Rotation.CrusaderStrikeCD = _calcOpts.CSCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.CSCD20 * _calcOpts.TimeUnder20;
            calc.Rotation.DivineStormCD = _calcOpts.DSCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.DSCD20 * _calcOpts.TimeUnder20;
            calc.Rotation.ConsecrationCD = _calcOpts.ConsCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.ConsCD20 * _calcOpts.TimeUnder20;
            calc.Rotation.ExorcismCD = _calcOpts.ExoCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.ExoCD20 * _calcOpts.TimeUnder20;
            calc.Rotation.HammerOfWrathCD = _calcOpts.HoWCD20;

            calc.SealDPS = seal.AverageDamage() * GetMeleeAttacksPerSec() * Ability.GetMeleeAvoid();
            calc.JudgementDPS = judge.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.JudgeCD + _calcOpts.TimeUnder20 / _calcOpts.JudgeCD20);
            calc.CrusaderStrikeDPS = cs.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.CSCD + _calcOpts.TimeUnder20 / _calcOpts.CSCD20);
            calc.DivineStormDPS = ds.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.DSCD + _calcOpts.TimeUnder20 / _calcOpts.DSCD20);
            calc.ConsecrationDPS = cons.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.ConsCD + _calcOpts.TimeUnder20 / _calcOpts.ConsCD20);
            calc.ExorcismDPS = exo.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.ExoCD + _calcOpts.TimeUnder20 / _calcOpts.ExoCD20);
            calc.HammerOfWrathDPS = how.AverageDamage() * (_calcOpts.TimeUnder20 / _calcOpts.HoWCD20);
        }

        public override float GetMeleeAttacksPerSec()
        {
            return 1f / Ability.AttackSpeed
                + (1f / _calcOpts.CSCD + 1f / _calcOpts.DSCD) * (1f - _calcOpts.TimeUnder20)
                + (1f / _calcOpts.CSCD20 + 1f / _calcOpts.DSCD20) * _calcOpts.TimeUnder20;
        }

        public override float GetPhysicalAttacksPerSec()
        {
            return GetMeleeAttacksPerSec()
                + (1f / _calcOpts.JudgeCD) * (1f - _calcOpts.TimeUnder20)
                + (1f / _calcOpts.JudgeCD20 + 1f / _calcOpts.HoWCD20) * _calcOpts.TimeUnder20;
        }

    }
}
