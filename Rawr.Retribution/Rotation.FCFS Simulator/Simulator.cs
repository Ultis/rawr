using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class Simulator : Rotation
    {

        private static Ability[] RemoveHammerOfWrathFromRotation(Ability[] rotation)
        {
            List<Ability> abilities = new List<Ability>(rotation);
            abilities.Remove(Ability.HammerOfWrath);
            return abilities.ToArray();
        }

        private static Ability[] RemoveUnavailableAbilitiesFromRotation(
            Ability[] rotation,
            CombatStats combats)
        {
            Ability[] result = rotation;

            if (combats.Talents.DivineStorm == 0)
            {
                List<Ability> abilities = new List<Ability>(result);
                abilities.Remove(Ability.DivineStorm);
                result = abilities.ToArray();
            }
            return result;
        }

        /// <summary>
        /// Calculates the solution with concrete parameters
        /// </summary>
        private static RotationSolution GetSolution(
            CombatStats combats,
            Ability[] rotation,
            decimal simulationTime,
            float spellHaste)
        {
            return SimulatorEngine.SimulateRotation(new SimulatorParameters(
                rotation,
                combats.CalcOpts.Wait,
                combats.CalcOpts.Delay,
                combats.Talents.GlyphOfConsecration,
                spellHaste,
                simulationTime));
        }

        /// <summary>
        /// Calculates the solution by combining subsolutions with the boss above and under 20% health
        /// </summary>
        private static RotationSolution GetCombinedSolutionWithUnder20PercentHealth(
            CombatStats combats,
            Ability[] rotation,
            decimal simulationTime,
            float spellHaste)
        {
            return RotationSolution.Combine(
                () => GetSolution(
                    combats,
                    RemoveHammerOfWrathFromRotation(rotation),
                    simulationTime,
                    spellHaste),
                1 - combats.CalcOpts.TimeUnder20,
                () => GetSolution(
                    combats,
                    rotation,
                    simulationTime,
                    spellHaste),
                combats.CalcOpts.TimeUnder20);
        }

        /// <summary>
        /// Calculates the solution by combining subsolutions with and withou Bloodlust
        /// </summary>
        private static RotationSolution GetCombinedSolutionWithBloodlust(
            CombatStats combats,
            Ability[] rotation,
            decimal simulationTime)
        {
            const float bloodlustDuration = 40f;
            const float secondsPerMinute = 60f;
            const float bloodlustHaste = 0.3f;

            // in seconds
            float fightLengthWithBloodlust = combats.CalcOpts.Bloodlust ?
                Math.Min(combats.CalcOpts.FightLength * secondsPerMinute, bloodlustDuration) :
                0;
            // in seconds
            float fightLengthWithoutBloodlust =
                Math.Max(0, combats.CalcOpts.FightLength * secondsPerMinute - fightLengthWithBloodlust);

            float bloodlustSpellHaste = (1 + combats.Stats.SpellHaste) * (1 + bloodlustHaste) - 1;

            float normalSwingTime = combats.BaseWeaponSpeed / (1 + combats.Stats.PhysicalHaste);
            float bloodlustSwingTime = normalSwingTime / (1 + bloodlustHaste);

            return RotationSolution.Combine(
                    () => GetCombinedSolutionWithUnder20PercentHealth(
                        combats,
                        rotation,
                        simulationTime,
                        combats.Stats.SpellHaste),
                    fightLengthWithoutBloodlust,
                    () => GetCombinedSolutionWithUnder20PercentHealth(
                        combats,
                        rotation,
                        simulationTime,
                        bloodlustSpellHaste),
                    fightLengthWithBloodlust);
        }
        
        public RotationSolution Solution { get; set; }
        public Ability[] Rotation { get; set; }

        public Simulator(CombatStats combats, Ability[] rotation, decimal simulationTime)
            : base(combats)
        {
            if (rotation == null)
                throw new ArgumentNullException("rotation");

            Ability[] effectiveRotation = RemoveUnavailableAbilitiesFromRotation(rotation, combats);

            Rotation = effectiveRotation;
            Solution = GetCombinedSolutionWithBloodlust(combats, effectiveRotation, simulationTime);
        }

        public override void SetCharacterCalculations(CharacterCalculationsRetribution calc)
        {
            calc.Solution = Solution;
            calc.Rotation = Rotation;
        }

        public override float GetAbilityUsagePerSecond(Skill skill)
        {
            if ((skill == null) || (Solution == null))
                return 0f;
            else
                return Solution.GetAbilityUsagePerSecond(skill.RotationAbility.Value);
        }
    }
}
