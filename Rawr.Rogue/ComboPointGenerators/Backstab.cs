using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    [Serializable]
    public class Backstab : ComboPointGenerator
    {
        public override string Name { get { return "Backstab"; } }

        protected override float EnergyCost(CombatFactors combatFactors)
        {
            return 60f * combatFactors.Tier7FourPieceEnergyCostReduction
                - Talents.SlaughterFromTheShadows.BackstabAndAmbushEnergyCost.Bonus 
                - (Crit(combatFactors) * Talents.FocusedAttacks.Bonus);  
        }

        public override float Crit(CombatFactors combatFactors)
        {
            return (combatFactors.ProbMhCrit + Talents.PuncturingWounds.Backstab.Bonus) * combatFactors.ProbYellowHit;
        }

        public override float CalcCpgDPS(Stats stats, CombatFactors combatFactors, CalculationOptionsRogue calcOpts, float numCpg, float cycleTime)
        {
            var baseDamage = BaseAttackDamage(combatFactors);
			baseDamage *= (1 + Talents.Add(Talents.FindWeakness, Talents.Aggression, Talents.BladeTwisting, Talents.SurpriseAttacks, Talents.Opportunity));
            baseDamage *= Talents.DirtyDeeds.Multiplier;
    		baseDamage *= Talents.FindWeakness.Multiplier;
            baseDamage *= combatFactors.DamageReduction;

            var critDamage = baseDamage*CriticalDamageMultiplier(combatFactors)*Crit(combatFactors);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors), 0);

            return (critDamage + nonCritDamage)* numCpg / cycleTime;
        }

        private static float BaseAttackDamage(CombatFactors combatFactors)
        {
            var attackDamage = combatFactors.MhNormalizedDamage;
            attackDamage += 465;
            attackDamage *= (1.5f + Talents.SinisterCalling.HemoAndBackstab.Bonus);
            return attackDamage;
        }

        private static float CriticalDamageMultiplier(CombatFactors combatFactors)
        {
            return (combatFactors.BaseCritMultiplier + Talents.Lethality.Bonus);
        }
    }
}