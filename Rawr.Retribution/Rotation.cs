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
        public Skill HoR;
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
            HoR = new HandOfReckoning(combats);

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
                float stack = AverageSoVStackSize();
                Seal = new SealOfVengeance(combats, stack);
                SealDot = new SealOfVengeanceDoT(combats, stack);
                Judge = new JudgementOfVengeance(combats, stack);
            }
            else
            {
                Seal = new None(combats);
                Judge = new None(combats);
            }
        }

        public abstract void SetCharacterCalculations(CharacterCalculationsRetribution calc);

        public void SetDPS(CharacterCalculationsRetribution calc)
        {
            SetCharacterCalculations(calc);

            calc.AverageSoVStack = AverageSoVStackSize();
            calc.SoVOvertake = SoVOvertakeTime();

            calc.WhiteDPS = White.WhiteDPS();
            calc.SealDPS = SealDPS(Seal, SealDot);
            calc.JudgementDPS = JudgementDPS(Judge);
            calc.DivineStormDPS = DivineStormDPS(DS);
            calc.CrusaderStrikeDPS = CrusaderStrikeDPS(CS);
            calc.ConsecrationDPS = ConsecrationDPS(Cons);
            calc.ExorcismDPS = ExorcismDPS(Exo);
            calc.HammerOfWrathDPS = HammerOfWrathDPS(HoW);
            calc.HandOfReckoningDPS = HandOfReckoningDPS(HoR);

            calc.WhiteSkill = White;
            calc.SealSkill = Seal;
            calc.JudgementSkill = Judge;
            calc.DivineStormSkill = DS;
            calc.CrusaderStrikeSkill = CS;
            calc.ConsecrationSkill = Cons;
            calc.ExorcismSkill = Exo;
            calc.HammerOfWrathSkill = HoW;
            calc.HandOfReckoningSkill = HoR;

            calc.DPSPoints =
                calc.WhiteDPS +
                calc.SealDPS +
                calc.JudgementDPS +
                calc.CrusaderStrikeDPS +
                calc.DivineStormDPS +
                calc.ExorcismDPS +
                calc.HandOfReckoningDPS +
                calc.ConsecrationDPS +
                calc.HammerOfWrathDPS +
                calc.OtherDPS;
        }

        public float DPS()
        {
            return White.WhiteDPS() + SealDPS(Seal, SealDot)+ JudgementDPS(Judge) + DivineStormDPS(DS)
                + CrusaderStrikeDPS(CS) + ConsecrationDPS(Cons) + ExorcismDPS(Exo) + HammerOfWrathDPS(HoW)
                + HandOfReckoningDPS(HoR);
        }

        public float SealProcsPerSec(Skill seal)
        {
            if (seal.GetType() == typeof(SealOfVengeance))
            {
                return GetMeleeAttacksPerSec();
            //}else if (seal.GetType() == typeof(SealOfRighteousness)){
            //    return GetMeleeAttacksPerSec() + GetJudgementsPerSec() + GetJudgementsPerSec();
            //}
            }
            else
            {
                return GetMeleeAttacksPerSec() + GetJudgementsPerSec();
            }
        }

        public float SoVOvertakeTime()
        {
            float sov0dps = JudgementDPS(new JudgementOfVengeance(Combats, 0));
            float sov5dps = JudgementDPS(new JudgementOfVengeance(Combats, 5))
                + SealDPS(new SealOfVengeance(Combats, 5), new SealOfVengeanceDoT(Combats, 5));
            float sordps = JudgementDPS(new JudgementOfRighteousness(Combats))
                + SealDPS(new SealOfRighteousness(Combats), null);

            if (sordps > sov0dps)
            {
                float averageStack = (sordps - sov0dps) / (sov5dps - sov0dps) * 5f;
                float timeToMaxStack = Combats.AttackSpeed * 4f;
                return 2.5f * timeToMaxStack / (5f - averageStack);
            }
            else { return 0; }
        }

        public float AverageSoVStackSize()
        {
            float averageTimeOnMob = Combats.CalcOpts.FightLength * 60f / (Combats.CalcOpts.TargetSwitches + 1);
            float timeToMaxStack = Combats.AttackSpeed * 4f;
            if (averageTimeOnMob > timeToMaxStack)
            {
                return (2.5f * timeToMaxStack + 5f * (averageTimeOnMob - timeToMaxStack)) / averageTimeOnMob;
            }
            else
            {
                return 2.5f * averageTimeOnMob / timeToMaxStack;
            }
        }

        public virtual float SealDPS(Skill seal, Skill sealdot)
        {
            if (sealdot == null) return seal.AverageDamage() * SealProcsPerSec(seal);
            else return sealdot.AverageDamage() / 3f + seal.AverageDamage() * SealProcsPerSec(seal);
        }

        public virtual float HandOfReckoningDPS(Skill hor) { return hor.AverageDamage() / 8f * Combats.CalcOpts.HoREff; }

        public abstract float JudgementDPS(Skill judge);
        public abstract float CrusaderStrikeDPS(Skill cs);
        public abstract float DivineStormDPS(Skill ds);
        public abstract float ConsecrationDPS(Skill cons);
        public abstract float ExorcismDPS(Skill exo);
        public abstract float HammerOfWrathDPS(Skill how);

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
        public Ability[] Rotation { get; set; }
        public int RotationIndex { get; set; }

        public Simulator(CombatStats combats, int rotation) : base(combats)
        {
            Solution = RotationSimulator.SimulateRotation(Parameters(combats.CalcOpts.Rotations[rotation]));
        }

        public Simulator(CombatStats combats)
            : base(combats)
        {
            Rotation = null;
            RotationIndex = -1;

            RotationSolution maxSolution = null;
            float maxDPS = 0, currentDPS;

            if (combats.CalcOpts.ForceRotation >= 0)
            {
                Solution = RotationSimulator.SimulateRotation(Parameters(combats.CalcOpts.Rotations[combats.CalcOpts.ForceRotation]));
            }
            else if (combats.CalcOpts.Rotations.Count > 0)
            {
                for (int i = 0; i < combats.CalcOpts.Rotations.Count; i++)
                {
                    Solution = RotationSimulator.SimulateRotation(Parameters(combats.CalcOpts.Rotations[i]));
                    currentDPS = DPS();
                    if (currentDPS > maxDPS)
                    {
                        maxDPS = currentDPS;
                        maxSolution = Solution;
                        Rotation = combats.CalcOpts.Rotations[i];
                        RotationIndex = i;
                    }
                }
                Solution = maxSolution;
            }
            else
            {
                Solution = RotationSimulator.SimulateRotation(Parameters(RotationParameters.DefaultRotation()));
            }
        }

        private RotationParameters Parameters(Ability[] rotation)
        {
            const float bloodlustDuration = 40f;

            return new RotationParameters(
                rotation,
                Combats.CalcOpts.TimeUnder20,
                Combats.CalcOpts.Wait,
                Combats.CalcOpts.Delay,
                Combats.Stats.JudgementCDReduction > 0,
                Combats.Talents.ImprovedJudgements,
                Combats.Talents.GlyphOfConsecration,
                Combats.Stats.DivineStormRefresh > 0 ? 
                    Combats.BaseWeaponSpeed / (1 + Combats.Stats.PhysicalHaste) : 
                    0f,
                Combats.Stats.SpellHaste,
                Combats.CalcOpts.Bloodlust && (Combats.Stats.Bloodlust == 0) ?
                    Math.Min(1f, Combats.CalcOpts.FightLength / bloodlustDuration) : 
                    0);
        }

        public override void SetCharacterCalculations(CharacterCalculationsRetribution calc)
        {
            calc.Solution = Solution;
            calc.Rotation = Rotation;
            calc.RotationIndex = RotationIndex;
        }

        public override float JudgementDPS(Skill judge) { return judge.AverageDamage() * Solution.Judgement / Solution.FightLength; }

        public override float CrusaderStrikeDPS(Skill cs) { return cs.AverageDamage() * Solution.CrusaderStrike / Solution.FightLength; }

        public override float DivineStormDPS(Skill ds) { return ds.AverageDamage() * Solution.DivineStorm / Solution.FightLength; }

        public override float ConsecrationDPS(Skill cons) { return cons.AverageDamage() * Solution.Consecration / Solution.FightLength; }

        public override float ExorcismDPS(Skill exo) { return exo.AverageDamage() * Solution.Exorcism / Solution.FightLength; }

        public override float HammerOfWrathDPS(Skill how) { return how.AverageDamage() * Solution.HammerOfWrath / Solution.FightLength; }

        public override float GetJudgementsPerSec() { return Solution.Judgement / Solution.FightLength * Judge.ChanceToLand() * Judge.Targets(); }

        public override float GetMeleeAttacksPerSec()
        {
            // Melee hit procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
            // - Weapon swing hits
            // - Tiny Abomination in a Jar releasing attack hits
            // (2 multiplier needs to be moved to another place)

            return 
                Solution.CrusaderStrike / Solution.FightLength * CS.ChanceToLand() * CS.Targets() + 
                Solution.DivineStorm / Solution.FightLength * DS.ChanceToLand() * DS.Targets() + 
                White.ChanceToLand() / Combats.AttackSpeed + 
                Combats.Stats.MoteOfAnger * 2 * White.ChanceToLand();
        }

        public override float GetMeleeCritsPerSec()
        {
            // Melee crit procs can be triggered by:
            // - Crusader Strike crits
            // - Divine Storm crits on each target
            // - Weapon swing crits
            // - Tiny Abomination in a Jar releasing attack crits
            // (2 multiplier needs to be moved to another place)

            return 
                Solution.CrusaderStrike * CS.ChanceToCrit() / Solution.FightLength * CS.Targets() + 
                Solution.DivineStorm * DS.ChanceToCrit() / Solution.FightLength * DS.Targets() + 
                White.ChanceToCrit() / Combats.AttackSpeed + 
                Combats.Stats.MoteOfAnger * 2 * White.ChanceToCrit();
        }

        public override float GetPhysicalAttacksPerSec()
        {
            // Physical hit procs, damage done procs and damage or healing done procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
            // - Weapon swing hits
            // - Tiny Abomination in a Jar releasing attack hits
            // (2 multiplier needs to be moved to another place)
            // - Judgement hits
            // - Hammer of Wrath hits

            return GetMeleeAttacksPerSec() + 
                Solution.Judgement * Judge.ChanceToLand() / Solution.FightLength * Judge.Targets() + 
                Solution.HammerOfWrath * HoW.ChanceToLand() / Solution.FightLength * HoW.Targets();
        }

        public override float GetPhysicalCritsPerSec()
        {
            // Physical crit procs can be triggered by:
            // - Crusader Strike crits
            // - Divine Storm crits on each target
            // - Weapon swing crits
            // - Tiny Abomination in a Jar releasing attack crits
            // (2 multiplier needs to be moved to another place)
            // - Judgement crits
            // - Hammer of Wrath crits

            return GetMeleeCritsPerSec() + 
                Solution.Judgement * Judge.ChanceToCrit() / Solution.FightLength * Judge.Targets() + 
                Solution.HammerOfWrath * HoW.ChanceToCrit() / Solution.FightLength * HoW.Targets();
        }

        public override float GetCrusaderStrikeCD() { return Solution.FightLength / Solution.CrusaderStrike; }

        public override float GetJudgementCD() { return Solution.FightLength / Solution.Judgement; }
    }

    public class EffectiveCooldown : Rotation
    {
        private readonly CalculationOptionsRetribution _calcOpts;

        public EffectiveCooldown(CombatStats combats) : base(combats) { _calcOpts = combats.CalcOpts; }

        public override void SetCharacterCalculations(CharacterCalculationsRetribution calc)
        {
            calc.Solution = new RotationSolution();
            calc.Solution.JudgementCD = _calcOpts.JudgeCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.JudgeCD20 * _calcOpts.TimeUnder20;
            calc.Solution.CrusaderStrikeCD = _calcOpts.CSCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.CSCD20 * _calcOpts.TimeUnder20;
            calc.Solution.DivineStormCD = _calcOpts.DSCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.DSCD20 * _calcOpts.TimeUnder20;
            calc.Solution.ConsecrationCD = _calcOpts.ConsCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.ConsCD20 * _calcOpts.TimeUnder20;
            calc.Solution.ExorcismCD = _calcOpts.ExoCD * (1f - _calcOpts.TimeUnder20) + _calcOpts.ExoCD20 * _calcOpts.TimeUnder20;
            calc.Solution.HammerOfWrathCD = _calcOpts.HoWCD20;

            calc.Rotation = null;
            calc.RotationIndex = -1;
        }

        public override float JudgementDPS(Skill judge)
        {
            return judge.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.JudgeCD + _calcOpts.TimeUnder20 / _calcOpts.JudgeCD20);
        }

        public override float CrusaderStrikeDPS(Skill cs)
        {
            return cs.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.CSCD + _calcOpts.TimeUnder20 / _calcOpts.CSCD20);
        }

        public override float DivineStormDPS(Skill ds)
        {
            return ds.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.DSCD + _calcOpts.TimeUnder20 / _calcOpts.DSCD20);
        }

        public override float ConsecrationDPS(Skill cons)
        {
            return cons.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.ConsCD + _calcOpts.TimeUnder20 / _calcOpts.ConsCD20);
        }

        public override float ExorcismDPS(Skill exo)
        {
            return exo.AverageDamage() * ((1f - _calcOpts.TimeUnder20) / _calcOpts.ExoCD + _calcOpts.TimeUnder20 / _calcOpts.ExoCD20);
        }

        public override float HammerOfWrathDPS(Skill how)
        {
            return how.AverageDamage() * (_calcOpts.TimeUnder20 / _calcOpts.HoWCD20);
        }

        public override float GetJudgementsPerSec()
        {
            return Judge.ChanceToLand() / _calcOpts.JudgeCD * Judge.Targets() * (1f - _calcOpts.TimeUnder20)
                + Judge.ChanceToLand() / _calcOpts.JudgeCD20 * Judge.Targets() * _calcOpts.TimeUnder20;
        }

        public override float GetMeleeAttacksPerSec()
        {
            // Melee hit procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
            // - Weapon swing hits
            // - Tiny Abomination in a Jar releasing attack hits
            // (2 multiplier needs to be moved to another place)

            return 
                White.ChanceToLand() / Combats.AttackSpeed + 
                Combats.Stats.MoteOfAnger * 2 * White.ChanceToLand() + 
                (CS.ChanceToLand() / _calcOpts.CSCD * CS.Targets() + 
                    DS.ChanceToLand() / _calcOpts.DSCD * DS.Targets()) * (1f - _calcOpts.TimeUnder20) + 
                (CS.ChanceToLand() / _calcOpts.CSCD20 * CS.Targets() + 
                    DS.ChanceToLand() / _calcOpts.DSCD20 * DS.Targets()) * _calcOpts.TimeUnder20;
        }

        public override float GetMeleeCritsPerSec()
        {
            // Melee crit procs can be triggered by:
            // - Crusader Strike crits
            // - Divine Storm crits on each target
            // - Weapon swing crits
            // - Tiny Abomination in a Jar releasing attack crits
            // (2 multiplier needs to be moved to another place)

            return 
                White.ChanceToCrit() / Combats.AttackSpeed + 
                Combats.Stats.MoteOfAnger * 2f * White.ChanceToCrit() + 
                (Judge.ChanceToCrit() / _calcOpts.CSCD * CS.Targets() + 
                    DS.ChanceToCrit() / _calcOpts.DSCD * DS.Targets()) * (1f - _calcOpts.TimeUnder20) + 
                (Judge.ChanceToCrit() / _calcOpts.CSCD20 * CS.Targets() + 
                    DS.ChanceToCrit() / _calcOpts.DSCD20 * DS.Targets()) * _calcOpts.TimeUnder20;
        }

        public override float GetPhysicalAttacksPerSec()
        {
            // Physical hit procs, damage done procs and damage or healing done procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
            // - Weapon swing hits
            // - Tiny Abomination in a Jar releasing attack hits
            // (2 multiplier needs to be moved to another place)
            // - Judgement hits
            // - Hammer of Wrath hits

            return GetMeleeAttacksPerSec() + 
                (Judge.ChanceToLand() / _calcOpts.JudgeCD * Judge.Targets()) * (1f - _calcOpts.TimeUnder20) + 
                (Judge.ChanceToLand() / _calcOpts.JudgeCD20 * Judge.Targets() + 
                    HoW.ChanceToLand() / _calcOpts.HoWCD20 * HoW.Targets()) * _calcOpts.TimeUnder20;
        }

        public override float GetPhysicalCritsPerSec()
        {
            // Physical crit procs can be triggered by:
            // - Crusader Strike crits
            // - Divine Storm crits on each target
            // - Weapon swing crits
            // - Tiny Abomination in a Jar releasing attack crits
            // (2 multiplier needs to be moved to another place)
            // - Judgement crits
            // - Hammer of Wrath crits

            return GetMeleeCritsPerSec() + 
                (Judge.ChanceToCrit() / _calcOpts.JudgeCD * Judge.Targets()) * (1f - _calcOpts.TimeUnder20) + 
                (Judge.ChanceToCrit() / _calcOpts.JudgeCD20 * Judge.Targets() + 
                    HoW.ChanceToCrit() / _calcOpts.HoWCD20 * HoW.Targets()) * _calcOpts.TimeUnder20;
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
