using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public abstract class Rotation
    {

        public Skill CS;
        public Skill Judge;
        public Skill DS;
        public Skill Exo;
        public Skill HoW;
        public Skill Cons;
        public Skill Seal;
        public Skill SealDot;
        public White White;

        protected CombatStats Combats;

        public Rotation(CombatStats combats)
        {
            Combats = combats;
            CS = new CrusaderStrike(combats);
            DS = new DivineStorm(combats);
            Exo = new Exorcism(combats);
            HoW = new HammerOfWrath(combats);
            Cons = new Consecration(combats);
            White = new White(combats);

            if (combats.CalcOpts.Seal == SealOf.Righteousness)
            {
                Seal = new SealOfRighteousness(combats);
                Judge = new JudgementOfRighteousness(combats);
            }
            else if (combats.CalcOpts.Seal == SealOf.Command)
            {
                Seal = new SealOfCommand(combats);
                Judge = new JudgementOfCommand(combats);
            }
            else if (combats.CalcOpts.Seal == SealOf.Vengeance)
            {
                Seal = new SealOfVengeance(combats);
                SealDot = new SealOfVengeanceDoT(combats);
                Judge = new JudgementOfVengeance(combats);
            }
            else
            {
                Seal = new None(combats);
                Judge = new None(combats);
            }
        }

        public abstract void SetAbilityDPS(CharacterCalculationsRetribution calc);

        public void SetDPS(CharacterCalculationsRetribution calc)
        {
            calc.WhiteDPS = White.WhiteDPS();
            SetAbilityDPS(calc);

            calc.WhiteSkill = White;
            calc.SealSkill = Seal;
            calc.JudgementSkill = Judge;
            calc.DivineStormSkill = DS;
            calc.CrusaderStrikeSkill = CS;
            calc.ConsecrationSkill = Cons;
            calc.ExorcismSkill = Exo;
            calc.HammerOfWrathSkill = HoW;
            
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
            if (Seal.GetType() == typeof(SealOfVengeance))
            {
                return GetMeleeAttacksPerSec();
            }
            else
            {
                return GetMeleeAttacksPerSec() + GetJudgementsPerSec();
            }
        }

        public abstract float GetJudgementsPerSec();

        public abstract float GetMeleeAttacksPerSec();
        public abstract float GetPhysicalAttacksPerSec();
        public abstract float GetMeleeCritsPerSec();
        public abstract float GetPhysicalCritsPerSec();

        public abstract float GetJudgementCD();
        public abstract float GetCrusaderStrikeCD();

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
                    combats.Talents.ImprovedJudgements,
                    combats.Talents.GlyphOfConsecration)
            );
        }

        public override void SetAbilityDPS(CharacterCalculationsRetribution calc)
        {
            calc.Rotation = Solution;


            calc.SealDPS = Seal.AverageDamage() * SealProcsPerSec();
            if (SealDot != null) calc.SealDPS += SealDot.AverageDamage() / 3f;

            calc.JudgementDPS = Judge.AverageDamage() * Solution.Judgement / Solution.FightLength;
            calc.CrusaderStrikeDPS = CS.AverageDamage() * Solution.CrusaderStrike / Solution.FightLength;
            calc.DivineStormDPS = DS.AverageDamage() * Solution.DivineStorm / Solution.FightLength;
            calc.ConsecrationDPS = Cons.AverageDamage() * Solution.Consecration / Solution.FightLength;
            calc.ExorcismDPS = Exo.AverageDamage() * Solution.Exorcism / Solution.FightLength;
            calc.HammerOfWrathDPS = HoW.AverageDamage() * Solution.HammerOfWrath / Solution.FightLength;
        }

        public override float GetJudgementsPerSec()
        {
            return Solution.Judgement / Solution.FightLength * Judge.ChanceToLand() * Judge.Targets();
        }

        public override float GetMeleeAttacksPerSec()
        {
            return Solution.CrusaderStrike / Solution.FightLength * CS.ChanceToLand() * CS.Targets()
                + Solution.DivineStorm / Solution.FightLength * DS.ChanceToLand() * DS.Targets()
                + White.ChanceToLand() / Combats.AttackSpeed;
        }

        public override float GetMeleeCritsPerSec()
        {
            return Solution.CrusaderStrike * CS.ChanceToCrit() / Solution.FightLength * CS.Targets()
                + Solution.DivineStorm * DS.ChanceToCrit() / Solution.FightLength * DS.Targets()
                + 1f / Combats.AttackSpeed;
        }

        public override float GetPhysicalAttacksPerSec()
        {
            return GetMeleeAttacksPerSec()
                + Solution.Judgement * Judge.ChanceToLand() / Solution.FightLength * Judge.Targets()
                + Solution.HammerOfWrath * HoW.ChanceToLand() / Solution.FightLength * HoW.Targets();
        }

        public override float GetPhysicalCritsPerSec()
        {
            return GetMeleeAttacksPerSec()
                + Solution.Judgement * Judge.ChanceToCrit() / Solution.FightLength * Judge.Targets()
                + Solution.HammerOfWrath * HoW.ChanceToCrit() / Solution.FightLength * HoW.Targets();
        }

        public override float GetCrusaderStrikeCD()
        {
            return Solution.FightLength / Solution.CrusaderStrike;
        }

        public override float GetJudgementCD()
        {
            return Solution.FightLength / Solution.Judgement;
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

            calc.SealDPS = Seal.AverageDamage() * SealProcsPerSec();
            if (SealDot != null) calc.SealDPS += SealDot.AverageDamage() / 3f;

            calc.JudgementDPS = Judge.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.JudgeCD + _calcOpts.TimeUnder20 / _calcOpts.JudgeCD20);
            calc.CrusaderStrikeDPS = CS.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.CSCD + _calcOpts.TimeUnder20 / _calcOpts.CSCD20);
            calc.DivineStormDPS = DS.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.DSCD + _calcOpts.TimeUnder20 / _calcOpts.DSCD20);
            calc.ConsecrationDPS = Cons.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.ConsCD + _calcOpts.TimeUnder20 / _calcOpts.ConsCD20);
            calc.ExorcismDPS = Exo.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.ExoCD + _calcOpts.TimeUnder20 / _calcOpts.ExoCD20);
            calc.HammerOfWrathDPS = HoW.AverageDamage() * (_calcOpts.TimeUnder20 / _calcOpts.HoWCD20);
        }

        public override float GetJudgementsPerSec()
        {
            return Judge.ChanceToLand() / _calcOpts.JudgeCD * Judge.Targets() * (1f - _calcOpts.TimeUnder20)
                + Judge.ChanceToLand() / _calcOpts.JudgeCD20 * Judge.Targets() * _calcOpts.TimeUnder20;
        }

        public override float GetMeleeAttacksPerSec()
        {
            return White.ChanceToLand() / Combats.AttackSpeed
                + (CS.ChanceToLand() / _calcOpts.CSCD * CS.Targets()
                + DS.ChanceToLand() / _calcOpts.DSCD * DS.Targets()) * (1f - _calcOpts.TimeUnder20)
                + (CS.ChanceToLand() / _calcOpts.CSCD20 * CS.Targets()
                + DS.ChanceToLand() / _calcOpts.DSCD20 * DS.Targets()) * _calcOpts.TimeUnder20;
        }

        public override float GetMeleeCritsPerSec()
        {
            return White.ChanceToCrit() / Combats.AttackSpeed
                + (Judge.ChanceToCrit() / _calcOpts.CSCD * CS.Targets()
                + DS.ChanceToCrit() / _calcOpts.DSCD * DS.Targets()) * (1f - _calcOpts.TimeUnder20)
                + (Judge.ChanceToCrit() / _calcOpts.CSCD20 * CS.Targets()
                + DS.ChanceToCrit() / _calcOpts.DSCD20 * DS.Targets()) * _calcOpts.TimeUnder20;
        }

        public override float GetPhysicalAttacksPerSec()
        {
            return GetMeleeAttacksPerSec()
                + (Judge.ChanceToLand() / _calcOpts.JudgeCD * Judge.Targets()) * (1f - _calcOpts.TimeUnder20)
                + (Judge.ChanceToLand() / _calcOpts.JudgeCD20 * Judge.Targets()
                + HoW.ChanceToLand() / _calcOpts.HoWCD20 * HoW.Targets()) * _calcOpts.TimeUnder20;
        }

        public override float GetPhysicalCritsPerSec()
        {
            return GetMeleeAttacksPerSec()
                + (Judge.ChanceToCrit() / _calcOpts.JudgeCD * Judge.Targets()) * (1f - _calcOpts.TimeUnder20)
                + (Judge.ChanceToCrit() / _calcOpts.JudgeCD20 * Judge.Targets()
                + HoW.ChanceToCrit() / _calcOpts.HoWCD20 * HoW.Targets()) * _calcOpts.TimeUnder20;
        }

        public override float GetCrusaderStrikeCD()
        {
            return _calcOpts.CSCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.CSCD20 * _calcOpts.TimeUnder20;
        }

        public override float GetJudgementCD()
        {
            return _calcOpts.JudgeCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.JudgeCD20 * _calcOpts.TimeUnder20;
        }

    }
}
