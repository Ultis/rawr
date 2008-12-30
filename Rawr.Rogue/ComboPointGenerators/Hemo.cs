using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    public class Hemo : IComboPointGenerator
    {
        public Hemo(RogueTalents talents)
        {
            _talents = talents;
        }

        private readonly RogueTalents _talents;

        public string Name { get { return "Hemo"; } }

        public float EnergyCost
        {
            get { return 35f - _talents.SlaughterFromTheShadows; }
        }

        public float Crit(CombatFactors combatFactors)
        {
            return combatFactors.ProbMhCrit * combatFactors.ProbYellowHit;
        }

        public float CalcCpgDPS(Stats stats, CombatFactors combatFactors, CalculationOptionsRogue calcOpts, float numCPG, float cycleTime)
        {
            var baseDamage = BaseAttackDamage(stats, combatFactors);
            baseDamage *= TalentBonusDamage();
            baseDamage *= (1f + .35f * 0.1f * _talents.DirtyDeeds);
            baseDamage *= combatFactors.DamageReduction;

            var critDamage = baseDamage * CriticalDamageMultiplier(combatFactors) * Crit(combatFactors);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors), 0);

            return (critDamage + nonCritDamage) * numCPG / cycleTime;
        }

        private float BaseAttackDamage(Stats stats, CombatFactors combatFactors)
        {
            var damage = combatFactors.MhNormalizedDamage;
            damage *= (1.1f + 0.01f * _talents.SinisterCalling);
            return damage;
        }

        private float CriticalDamageMultiplier(CombatFactors combatFactors)
        {
            return (combatFactors.BaseCritMultiplier + .06f * _talents.Lethality);
        }

        private float TalentBonusDamage()
        {
            var talentBonuses = 1f;
            talentBonuses += 0.02f * _talents.FindWeakness;
            talentBonuses += 0.1f * _talents.SurpriseAttacks;
            return talentBonuses;
        }
    }
}