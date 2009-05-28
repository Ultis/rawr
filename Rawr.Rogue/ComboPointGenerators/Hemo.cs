using System;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue.ComboPointGenerators
{
    [Serializable]
    public class Hemo : ComboPointGenerator
    {
        public override string Name { get { return "Hemo"; } }

        public override float EnergyCost(CombatFactors combatFactors)
        {
            return 35f * combatFactors.Tier7FourPieceEnergyCostReduction
                - Talents.SlaughterFromTheShadows.HemoEnergyCost.Bonus 
                - (Crit(combatFactors) * Talents.FocusedAttacks.Bonus); 
        }

        public override float Crit(CombatFactors combatFactors)
        {
            return combatFactors.ProbMhCrit * combatFactors.ProbYellowHit;
		}

        public override float CalcCpgDPS(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, Stats stats, CycleTime cycleTime)
        {
            var baseDamage = BaseAttackDamage(combatFactors);
			baseDamage *= (1 + Talents.Add(Talents.FindWeakness, Talents.SurpriseAttacks));
            baseDamage *= Talents.DirtyDeeds.Multiplier;
			baseDamage *= Talents.FindWeakness.Multiplier;
            baseDamage *= combatFactors.DamageReduction;

            var critDamage = baseDamage * CriticalDamageMultiplier(combatFactors) * Crit(combatFactors);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors), 0);

            return (critDamage + nonCritDamage) * calcOpts.ComboPointsNeededForCycle() / cycleTime.Duration;
        }

        private static float BaseAttackDamage(CombatFactors combatFactors)
        {
            var damage = combatFactors.MhNormalizedDamage;
            damage *= (1.1f + Talents.SinisterCalling.HemoAndBackstab.Bonus);
            return damage;
        }

        private static float CriticalDamageMultiplier(CombatFactors combatFactors)
        {
            return (combatFactors.BaseCritMultiplier + Talents.Lethality.Bonus);
        }
    }
}