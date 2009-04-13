using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public abstract class Rotation
    {

        protected Skill cs;
        protected Skill judge;
        protected Skill ds;
        protected Skill exo;
        protected Skill how;
        protected Skill cons;
        protected Skill seal;
        protected White white;

        protected CombatStats Combats;

        public Rotation(CombatStats combats)
        {
            Combats = combats;
            cs = new CrusaderStrike(combats);
            ds = new DivineStorm(combats);
            exo = new Exorcism(combats);
            how = new HammerOfWrath(combats);
            cons = new Consecration(combats);
            white = new White(combats);

            if (combats.CalcOpts.Seal == Seal.Righteousness)
            {
                seal = new SealOfRighteousness(combats);
                judge = new JudgementOfRighteousness(combats);
            }
            else if (combats.CalcOpts.Seal == Seal.Command)
            {
                seal = new SealOfCommand(combats);
                judge = new JudgementOfCommand(combats);
            }
            else if (combats.CalcOpts.Seal == Seal.Blood)
            {
                seal = new SealOfBlood(combats);
                judge = new JudgementOfBlood(combats);
            }
            else if (combats.CalcOpts.Seal == Seal.Vengeance)
            {
                seal = new SealOfVengeance(combats);
                judge = new JudgementOfVengeance(combats);
            }
            else
            {
                seal = new None(combats);
                judge = new None(combats);
            }
        }

        public abstract void SetAbilityDPS(CharacterCalculationsRetribution calc);

        public void SetDPS(CharacterCalculationsRetribution calc)
        {
            calc.WhiteDPS = white.WhiteDPS();
            SetAbilityDPS(calc);

            calc.WhiteSkill = white;
            calc.SealSkill = seal;
            calc.JudgementSkill = judge;
            calc.DivineStormSkill = ds;
            calc.CrusaderStrikeSkill = cs;
            calc.ConsecrationSkill = cons;
            calc.ExorcismSkill = exo;
            calc.HammerOfWrathSkill = how;
            
            calc.DPSPoints =
                calc.WhiteDPS +
                calc.SealDPS +
                calc.JudgementDPS +
                calc.CrusaderStrikeDPS +
                calc.DivineStormDPS +
                calc.ExorcismDPS +
                calc.ConsecrationDPS +
                calc.HammerOfWrathDPS +
                calc.OtherDPS;
        }

        public float SealProcsPerSec()
        {
            if (seal.GetType() == typeof(SealOfCommand))
            {
                float procrate = (7f * (Combats.Talents.GlyphOfSealOfCommand ? 1.2f : 1f)) / 60f * Combats.AttackSpeed;
                float timeBetweenProcs = 1f + GetMeleeAttacksPerSec() / procrate;
                return 1f / timeBetweenProcs;
            }

            if (seal.GetType() == typeof(SealOfVengeance))
            {
                return 1f / 3f;
            }
            else
            {
                return GetMeleeAttacksPerSec();
            }
        }

        public abstract float GetMeleeAttacksPerSec();
        public abstract float GetPhysicalAttacksPerSec();
        public abstract float GetMeleeCritsPerSec();
        public abstract float GetPhysicalCritsPerSec();

    }

    public class Simulator : Rotation
    {

        public RotationSolution Solution { get; set; }

        public Simulator(CombatStats combats)
            : base(combats)
        {
            Solution = RotationSimulator.SimulateRotation(
                new RotationParameters(combats.CalcOpts.Priorities,
                    combats.CalcOpts.TimeUnder20,
                    combats.CalcOpts.Wait,
                    combats.CalcOpts.Delay,
                    combats.Stats.JudgementCDReduction > 0 ? true : false,
                    combats.Talents.GlyphOfConsecration)
            );
        }

        public override void SetAbilityDPS(CharacterCalculationsRetribution calc)
        {
            calc.Rotation = Solution;


            calc.SealDPS = seal.AverageDamage() * SealProcsPerSec();

            calc.JudgementDPS = judge.AverageDamage() * Solution.Judgement / Solution.FightLength;
            calc.CrusaderStrikeDPS = cs.AverageDamage() * Solution.CrusaderStrike / Solution.FightLength;
            calc.DivineStormDPS = ds.AverageDamage() * Solution.DivineStorm / Solution.FightLength;
            calc.ConsecrationDPS = cons.AverageDamage() * Solution.Consecration / Solution.FightLength;
            calc.ExorcismDPS = exo.AverageDamage() * Solution.Exorcism / Solution.FightLength;
            calc.HammerOfWrathDPS = how.AverageDamage() * Solution.HammerOfWrath / Solution.FightLength;
        }

        public override float GetMeleeAttacksPerSec()
        {
            return Solution.CrusaderStrike / Solution.FightLength * cs.ChanceToLand()
                + Solution.DivineStorm / Solution.FightLength * ds.ChanceToLand()
                + white.ChanceToLand() / Combats.AttackSpeed;
        }

        public override float GetMeleeCritsPerSec()
        {
            return Solution.CrusaderStrike * cs.ChanceToCrit() / Solution.FightLength
                + Solution.DivineStorm * ds.ChanceToCrit() / Solution.FightLength
                + 1f / Combats.AttackSpeed;
        }

        public override float GetPhysicalAttacksPerSec()
        {
            return GetMeleeAttacksPerSec()
                + Solution.Judgement * judge.ChanceToLand() / Solution.FightLength
                + Solution.HammerOfWrath* how.ChanceToLand() / Solution.FightLength;
        }

        public override float GetPhysicalCritsPerSec()
        {
            return GetMeleeAttacksPerSec()
                + Solution.Judgement * judge.ChanceToCrit() / Solution.FightLength
                + Solution.HammerOfWrath * how.ChanceToCrit() / Solution.FightLength;
        }

    }

    public class EffectiveCooldown : Rotation
    {

        private readonly CalculationOptionsRetribution _calcOpts;

        public EffectiveCooldown(CombatStats combats)
            : base(combats)
        {
            _calcOpts = combats.CalcOpts;
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

            calc.SealDPS = seal.AverageDamage() * SealProcsPerSec();

            calc.JudgementDPS = judge.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.JudgeCD + _calcOpts.TimeUnder20 / _calcOpts.JudgeCD20);
            calc.CrusaderStrikeDPS = cs.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.CSCD + _calcOpts.TimeUnder20 / _calcOpts.CSCD20);
            calc.DivineStormDPS = ds.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.DSCD + _calcOpts.TimeUnder20 / _calcOpts.DSCD20);
            calc.ConsecrationDPS = cons.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.ConsCD + _calcOpts.TimeUnder20 / _calcOpts.ConsCD20);
            calc.ExorcismDPS = exo.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.ExoCD + _calcOpts.TimeUnder20 / _calcOpts.ExoCD20);
            calc.HammerOfWrathDPS = how.AverageDamage() * (_calcOpts.TimeUnder20 / _calcOpts.HoWCD20);
        }

        public override float GetMeleeAttacksPerSec()
        {
            return white.ChanceToLand() / Combats.AttackSpeed
                + (cs.ChanceToLand() / _calcOpts.CSCD + ds.ChanceToLand() / _calcOpts.DSCD) * (1f - _calcOpts.TimeUnder20)
                + (cs.ChanceToLand() / _calcOpts.CSCD20 + ds.ChanceToLand() / _calcOpts.DSCD20) * _calcOpts.TimeUnder20;
        }

        public override float GetMeleeCritsPerSec()
        {
            return white.ChanceToCrit() / Combats.AttackSpeed
                + (judge.ChanceToCrit() / _calcOpts.CSCD + ds.ChanceToCrit() / _calcOpts.DSCD) * (1f - _calcOpts.TimeUnder20)
                + (judge.ChanceToCrit() / _calcOpts.CSCD20 + ds.ChanceToCrit() / _calcOpts.DSCD20) * _calcOpts.TimeUnder20;
        }

        public override float GetPhysicalAttacksPerSec()
        {
            return GetMeleeAttacksPerSec()
                + (judge.ChanceToLand() / _calcOpts.JudgeCD) * (1f - _calcOpts.TimeUnder20)
                + (judge.ChanceToLand() / _calcOpts.JudgeCD20 + how.ChanceToLand() / _calcOpts.HoWCD20) * _calcOpts.TimeUnder20;
        }

        public override float GetPhysicalCritsPerSec()
        {
            return GetMeleeAttacksPerSec()
                + (judge.ChanceToCrit() / _calcOpts.JudgeCD) * (1f - _calcOpts.TimeUnder20)
                + (judge.ChanceToCrit() / _calcOpts.JudgeCD20 + how.ChanceToCrit() / _calcOpts.HoWCD20) * _calcOpts.TimeUnder20;
        }

    }
}
