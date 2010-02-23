using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public abstract class Rotation
    {

        public static IEnumerable<Ability[]> GetAllRotations()
        {
            Ability[] abilities = new Ability[(int)Ability.Last + 1];

            for (int ability = 0; ability <= (int)Ability.Last; ability++)
                abilities[ability] = (Ability)ability;

            return Utilities.GetDifferentElementPermutations(abilities);
        }


        protected Rotation(CombatStats combats)
        {
            if (combats == null)
                throw new ArgumentNullException("combats");

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
                SealDot = new NullSealDoT(combats);
                Judge = new JudgementOfRighteousness(combats);
            }
            else if (combats.CalcOpts.Seal == SealOf.Command)
            {
                Seal = new SealOfCommand(combats);
                SealDot = new NullSealDoT(combats);
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
                Seal = new NullSeal(combats);
                SealDot = new NullSealDoT(combats);
                Judge = new NullJudgement(combats);
            }
        }


        public Skill CS { get; private set; }
        public Skill Judge { get; private set; }
        public Skill DS { get; private set; }
        public Skill Exo { get; private set; }
        public Skill HoW { get; private set; }
        public Skill Cons { get; private set; }
        public Skill Seal { get; private set; }
        public Skill SealDot { get; private set; }
        public Skill HoR { get; private set; }
        public White White { get; private set; }
        public CombatStats Combats { get; private set; }


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
                + SealDPS(new SealOfRighteousness(Combats), new NullSealDoT(Combats));

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
            return sealdot.AverageDamage() / 3f + seal.AverageDamage() * SealProcsPerSec(seal);
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

        private static RotationParameters Parameters(
            CombatStats combats,
            Ability[] rotation,
            decimal simulationTime,
            bool under20PercentHealth,
            bool bloodlust)
        {
            const float bloodlustHaste = 0.3f;

            // Remove Hammer of Wrath from rotation if the target is not under 20% health
            if (!under20PercentHealth)
            {
                List<Ability> abilities = new List<Ability>(rotation);
                abilities.Remove(Ability.HammerOfWrath);
                rotation = abilities.ToArray();
            }

            return new RotationParameters(
                rotation,
                combats.CalcOpts.Wait,
                combats.CalcOpts.Delay,
                combats.Stats.JudgementCDReduction > 0,
                combats.Talents.ImprovedJudgements,
                combats.Talents.GlyphOfConsecration,
                combats.Stats.DivineStormRefresh > 0 ?
                    bloodlust ? 
                        combats.BaseWeaponSpeed / (1 + combats.Stats.PhysicalHaste) / (1 + bloodlustHaste) :
                        combats.BaseWeaponSpeed / (1 + combats.Stats.PhysicalHaste) :
                    0f,
                bloodlust ?
                    (1 + combats.Stats.SpellHaste) * (1 + bloodlustHaste) - 1 :
                    combats.Stats.SpellHaste,
                simulationTime);
        }


        public RotationSolution Solution { get; set; }
        public Ability[] Rotation { get; set; }


        public Simulator(CombatStats combats, Ability[] rotation, decimal simulationTime)
            : base(combats)
        {
            const float bloodlustDuration = 40f;
            const float secondsPerMinute = 60f;

            if (rotation == null)
                throw new ArgumentNullException("rotation");

            Rotation = rotation;

            // in seconds
            float fightLengthWithBloodlust = combats.CalcOpts.Bloodlust ?
                Math.Min(combats.CalcOpts.FightLength * secondsPerMinute, bloodlustDuration) :
                0;
            // in seconds
            float fightLengthWithoutBloodlust =
                Math.Max(0, combats.CalcOpts.FightLength * secondsPerMinute - fightLengthWithBloodlust);

            Solution = RotationSolution.Combine(
                () => RotationSolution.Combine(
                    () => RotationSimulator.SimulateRotation(
                        Parameters(combats, rotation, simulationTime, false, false)),
                    fightLengthWithoutBloodlust,
                    () => RotationSimulator.SimulateRotation(
                        Parameters(combats, rotation, simulationTime, false, true)),
                    fightLengthWithBloodlust),
                1 - combats.CalcOpts.TimeUnder20,
                () => RotationSolution.Combine(
                    () => RotationSimulator.SimulateRotation(
                        Parameters(combats, rotation, simulationTime, true, false)),
                    fightLengthWithoutBloodlust,
                    () => RotationSimulator.SimulateRotation(
                        Parameters(combats, rotation, simulationTime, true, true)),
                    fightLengthWithBloodlust),
                combats.CalcOpts.TimeUnder20);
        }


        public override void SetCharacterCalculations(CharacterCalculationsRetribution calc)
        {
            calc.Solution = Solution;
            calc.Rotation = Rotation;
        }

        public override float GetAbilityUsagePerSecond(Skill skill)
        {
            return Solution.GetAbilityUsagePerSecond(skill.RotationAbility.Value);
        }

    }

    public class EffectiveCooldown : Rotation
    {

        public EffectiveCooldown(CombatStats combats) 
            : base(combats) 
        { 
        }


        public override void SetCharacterCalculations(CharacterCalculationsRetribution calc)
        {
            calc.Solution = new RotationSolution();

            foreach (Skill skill in new[] { Judge, CS, DS, Cons, Exo, HoW })
                calc.Solution.SetAbilityEffectiveCooldown(
                    skill.RotationAbility.Value,
                    skill.UsableBefore20PercentHealth ? 
                       Combats.CalcOpts.GetEffectiveAbilityCooldown(skill.RotationAbility.Value) *
                                (1f - Combats.CalcOpts.TimeUnder20) +
                            Combats.CalcOpts.GetEffectiveAbilityCooldownAfter20PercentHealth(
                                    skill.RotationAbility.Value) *
                                Combats.CalcOpts.TimeUnder20 :
                        Combats.CalcOpts.GetEffectiveAbilityCooldownAfter20PercentHealth(
                            skill.RotationAbility.Value));

            calc.Rotation = null;
        }

        public override float GetAbilityUsagePerSecond(Skill skill)
        {
            return (skill.UsableBefore20PercentHealth ?
                (1 - Combats.CalcOpts.TimeUnder20)
                    / Combats.CalcOpts.GetEffectiveAbilityCooldown(skill.RotationAbility.Value) :
                0) +
            Combats.CalcOpts.TimeUnder20
                / Combats.CalcOpts.GetEffectiveAbilityCooldownAfter20PercentHealth(skill.RotationAbility.Value);
        }

    }
}
