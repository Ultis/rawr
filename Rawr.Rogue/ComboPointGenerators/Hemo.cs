using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    public class Hemo : IComboPointGenerator
    {
        public string Name { get { return "Hemo"; } }

        public float EnergyCost
        {
            get { return 35f - Talents.SlaughterFromTheShadows.Bonus; }
        }

        public float Crit(CombatFactors combatFactors)
        {
            return combatFactors.ProbMhCrit * combatFactors.ProbYellowHit;
        }

        public float CalcCpgDPS(Stats stats, CombatFactors combatFactors, CalculationOptionsRogue calcOpts, float numCPG, float cycleTime)
        {
            var baseDamage = BaseAttackDamage(combatFactors);
            baseDamage *= TalentBonusDamage();
            baseDamage *= Talents.DirtyDeeds.Multiplier;
            baseDamage *= combatFactors.DamageReduction;

            var critDamage = baseDamage * CriticalDamageMultiplier(combatFactors) * Crit(combatFactors);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors), 0);

            return (critDamage + nonCritDamage) * numCPG / cycleTime;
        }

        private static float BaseAttackDamage(CombatFactors combatFactors)
        {
            var damage = combatFactors.MhNormalizedDamage;
            damage *= (1.1f + Talents.SinisterCalling.Bonus);
            return damage;
        }

        private static float CriticalDamageMultiplier(CombatFactors combatFactors)
        {
            return (combatFactors.BaseCritMultiplier + Talents.Lethality.Bonus);
        }

        private static float TalentBonusDamage()
        {
            return Talents.Add(Talents.FindWeakness, Talents.SurpriseAttacks);
        }
    }
}