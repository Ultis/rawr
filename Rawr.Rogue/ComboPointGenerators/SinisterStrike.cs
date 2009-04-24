using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    public class SinisterStrike : IComboPointGenerator
    {
        public string Name { get { return "Sinister Strike"; } }

        public float EnergyCost(CombatFactors combatFactors)
        {
			return 45f - Talents.ImprovedSinisterStrike.Bonus - (Crit(combatFactors) * Talents.FocusedAttacks.Bonus);
        }

        public float Crit(CombatFactors combatFactors)
        {
            return combatFactors.ProbMhCrit * combatFactors.ProbYellowHit;
		}

		public float ComboPointsGeneratedPerAttack
		{
			get { return 1f; }
		}

        public float CalcCpgDPS(Stats stats, CombatFactors combatFactors, CalculationOptionsRogue calcOpts, float numCpg, float cycleTime)
        {
            var baseDamage = BaseAttackDamage(combatFactors);
			baseDamage *= (1 + Talents.Add(Talents.FindWeakness, Talents.Aggression, Talents.BladeTwisting, Talents.SurpriseAttacks));
            baseDamage *= Talents.DirtyDeeds.Multiplier;
			baseDamage *= Talents.FindWeakness.Multiplier;
			baseDamage *= combatFactors.DamageReduction;

            var critDamage = baseDamage * CriticalDamageMultiplier(combatFactors) * Crit(combatFactors);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors), 0);

            return (critDamage + nonCritDamage) * numCpg / cycleTime;
        }

        private static float BaseAttackDamage(CombatFactors combatFactors)
        {
            var damage = combatFactors.MhNormalizedDamage;
            damage += 180f;
            return damage;
        }

        private static float CriticalDamageMultiplier(CombatFactors combatFactors)
        {
            return (combatFactors.BaseCritMultiplier + Talents.Lethality.Bonus);
        }
    }
}