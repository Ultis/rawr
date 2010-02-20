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
            calc.JudgementDPS = GetAbilityDps(Judge);
            calc.DivineStormDPS = GetAbilityDps(DS);
            calc.CrusaderStrikeDPS = GetAbilityDps(CS);
            calc.ConsecrationDPS = GetAbilityDps(Cons);
            calc.ExorcismDPS = GetAbilityDps(Exo);
            calc.HammerOfWrathDPS = GetAbilityDps(HoW);
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
            return 
                White.WhiteDPS() + 
                SealDPS(Seal, SealDot)+ 
                GetAbilityDps(Judge) + 
                GetAbilityDps(DS) + 
                GetAbilityDps(CS) + 
                GetAbilityDps(Cons) + 
                GetAbilityDps(Exo) + 
                GetAbilityDps(HoW) + 
                HandOfReckoningDPS(HoR);
        }

        public float SealProcsPerSec(Skill seal)
        {
            if (seal.GetType() == typeof(SealOfVengeance))
                return GetMeleeAttacksPerSec();
            else
                return GetMeleeAttacksPerSec() + GetAbilityHitsPerSecond(Judge);
        }

        public float SoVOvertakeTime()
        {
            float sov0dps = GetAbilityDps(new JudgementOfVengeance(Combats, 0));
            float sov5dps = GetAbilityDps(new JudgementOfVengeance(Combats, 5))
                + SealDPS(new SealOfVengeance(Combats, 5), new SealOfVengeanceDoT(Combats, 5));
            float sordps = GetAbilityDps(new JudgementOfRighteousness(Combats))
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

        public virtual float HandOfReckoningDPS(Skill hor) 
        { 
            return hor.AverageDamage() / 8f * Combats.CalcOpts.HoREff; 
        }

        public float GetAbilityDps(Skill skill)
        {
            return skill.AverageDamage() * GetAbilityUsagePerSecond(skill);
        }

        public float GetAbilityHitsPerSecond(Skill skill)
        {
            return 
                GetAbilityUsagePerSecond(skill) *
                skill.ChanceToLand() * 
                skill.Targets() * 
                skill.TickCount();
        }

        public float GetAbilityCritsPerSecond(Skill skill)
        {
            return 
                GetAbilityUsagePerSecond(skill) *
                skill.ChanceToCrit() * 
                skill.Targets() * 
                skill.TickCount();
        }

        public float GetCrusaderStrikeCD()
        {
            return 1 / GetAbilityUsagePerSecond(CS);
        }

        public float GetJudgementCD()
        {
            return 1 / GetAbilityUsagePerSecond(Judge);
        }

        public abstract float GetAbilityUsagePerSecond(Skill skill);

        public float GetMeleeAttacksPerSec()
        {
            // Melee hit procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
            // - Weapon swing hits
            // - Tiny Abomination in a Jar releasing attack hits
            // (2 multiplier needs to be moved to another place)

            return
                GetAbilityHitsPerSecond(CS) +
                GetAbilityHitsPerSecond(DS) +
                White.ChanceToLand() / Combats.AttackSpeed +
                Combats.Stats.MoteOfAnger * 2 * White.ChanceToLand();
        }

        public float GetPhysicalAttacksPerSec()
        {
            // Physical hit procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
            // - Weapon swing hits
            // - Tiny Abomination in a Jar releasing attack hits
            // (2 multiplier needs to be moved to another place)
            // - Judgement hits
            // - Hammer of Wrath hits

            return
                GetAbilityHitsPerSecond(CS) +
                GetAbilityHitsPerSecond(DS) +
                White.ChanceToLand() / Combats.AttackSpeed +
                Combats.Stats.MoteOfAnger * 2 * White.ChanceToLand() +
                GetAbilityHitsPerSecond(Judge) +
                GetAbilityHitsPerSecond(HoW);
        }

        public float GetMeleeCritsPerSec()
        {
            // Melee crit procs can be triggered by:
            // - Crusader Strike crits
            // - Divine Storm crits on each target
            // - Weapon swing crits
            // - Tiny Abomination in a Jar releasing attack crits
            // (2 multiplier needs to be moved to another place)

            return
                GetAbilityCritsPerSecond(CS) +
                GetAbilityCritsPerSecond(DS) +
                White.ChanceToCrit() / Combats.AttackSpeed +
                Combats.Stats.MoteOfAnger * 2 * White.ChanceToCrit();
        }

        public float GetPhysicalCritsPerSec()
        {
            // Physical crit procs can be triggered by:
            // - Crusader Strike crits
            // - Divine Storm crits on each target
            // - Weapon swing crits
            // - Tiny Abomination in a Jar releasing attack crits
            // (2 multiplier needs to be moved to another place)
            // - Judgement crits
            // - Hammer of Wrath crits

            return
                GetAbilityCritsPerSecond(CS) +
                GetAbilityCritsPerSecond(DS) +
                White.ChanceToCrit() / Combats.AttackSpeed +
                Combats.Stats.MoteOfAnger * 2 * White.ChanceToCrit() +
                GetAbilityCritsPerSecond(Judge) +
                GetAbilityCritsPerSecond(HoW);
        }

        public float GetAttacksPerSec()
        {
            // Damage done procs and damage or healing done procs can be triggered by:
            // - Crusader Strike hits
            // - Divine Storm hits
            // - Weapon swing hits
            // - Tiny Abomination in a Jar releasing attack hits
            // (2 multiplier needs to be moved to another place)
            // - Judgement hits
            // - Hammer of Wrath hits
            // - Consecration damage ticks
            // - Exorcism hits

            return
                GetAbilityHitsPerSecond(CS) +
                GetAbilityHitsPerSecond(DS) +
                White.ChanceToLand() / Combats.AttackSpeed +
                Combats.Stats.MoteOfAnger * 2 * White.ChanceToLand() +
                GetAbilityHitsPerSecond(Judge) +
                GetAbilityHitsPerSecond(HoW) +
                GetAbilityHitsPerSecond(Cons) +
                GetAbilityHitsPerSecond(Exo);
        }

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

        public override float GetAbilityUsagePerSecond(Skill skill)
        {
            return Solution.GetAbilityUsagePerSecond(skill.RotationAbility.Value);
        }

    }

    public class EffectiveCooldown : Rotation
    {
        private readonly CalculationOptionsRetribution _calcOpts;

        public EffectiveCooldown(CombatStats combats) : base(combats) { _calcOpts = combats.CalcOpts; }

        public override void SetCharacterCalculations(CharacterCalculationsRetribution calc)
        {
            calc.Solution = new RotationSolution();

            foreach (Skill skill in new[] { Judge, CS, DS, Cons, Exo, HoW })
                calc.Solution.SetAbilityEffectiveCooldown(
                    skill.RotationAbility.Value,
                    skill.UsableBefore20PercentHealth ? 
                        _calcOpts.GetEffectiveAbilityCooldown(skill.RotationAbility.Value) * 
                                (1f - _calcOpts.TimeUnder20) +
                            _calcOpts.GetEffectiveAbilityCooldownAfter20PercentHealth(
                                    skill.RotationAbility.Value) *
                                _calcOpts.TimeUnder20 :
                        _calcOpts.GetEffectiveAbilityCooldownAfter20PercentHealth(skill.RotationAbility.Value));

            calc.Rotation = null;
            calc.RotationIndex = -1;
        }

        public override float GetAbilityUsagePerSecond(Skill skill)
        {
            return (skill.UsableBefore20PercentHealth ?
                (1 - _calcOpts.TimeUnder20)
                    / _calcOpts.GetEffectiveAbilityCooldown(skill.RotationAbility.Value) :
                0) +
            _calcOpts.TimeUnder20
                / _calcOpts.GetEffectiveAbilityCooldownAfter20PercentHealth(skill.RotationAbility.Value);
        }

    }
}
