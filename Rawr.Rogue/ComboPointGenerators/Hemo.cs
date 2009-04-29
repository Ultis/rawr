using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    [Serializable]
    public class Hemo : ComboPointGenerator
    {
        public override string Name { get { return "Hemo"; } }

        protected override float EnergyCost(CombatFactors combatFactors)
        {
            return 35f * combatFactors.Tier7FourPieceEnergyCostReduction
                - Talents.SlaughterFromTheShadows.HemoEnergyCost.Bonus 
                - (Crit(combatFactors) * Talents.FocusedAttacks.Bonus); 
        }

        public override float Crit(CombatFactors combatFactors)
        {
            return combatFactors.ProbMhCrit * combatFactors.ProbYellowHit;
		}

        public override float CalcCpgDPS(Stats stats, CombatFactors combatFactors, CalculationOptionsRogue calcOpts, float numCPG, float cycleTime)
        {
            var baseDamage = BaseAttackDamage(combatFactors);
			baseDamage *= (1 + Talents.Add(Talents.FindWeakness, Talents.SurpriseAttacks));
            baseDamage *= Talents.DirtyDeeds.Multiplier;
			baseDamage *= Talents.FindWeakness.Multiplier;
            baseDamage *= combatFactors.DamageReduction;

            var critDamage = baseDamage * CriticalDamageMultiplier(combatFactors) * Crit(combatFactors);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors), 0);

            return (critDamage + nonCritDamage) * numCPG / cycleTime;
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