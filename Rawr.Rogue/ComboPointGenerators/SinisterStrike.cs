using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    public class SinisterStrike : IComboPointGenerator
    {
        public SinisterStrike(RogueTalents talents)
        {
            _talents = talents;
        }

        private readonly RogueTalents _talents;
        
        public string Name { get { return "Sinister Strike"; } }

        public float EnergyCost
        {
            get
            {
                switch (_talents.ImprovedSinisterStrike)
                {
                    case 2: return 40f;
                    case 1: return 42f;
                    default: return 45f;
                }
            }
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

        private static float BaseAttackDamage(Stats stats, CombatFactors combatFactors)
        {
            var damage = combatFactors.MhNormalizedDamage;
            damage += 180f;
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
            talentBonuses += 0.03f * _talents.Aggression;
            talentBonuses += 0.05f * _talents.BladeTwisting;
            talentBonuses += 0.1f * _talents.SurpriseAttacks;
            return talentBonuses;
        }
    }
}