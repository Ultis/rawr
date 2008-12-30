using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    public class Backstab : IComboPointGenerator
    {
        public Backstab(RogueTalents talents)
        {
            _talents = talents;
        }

        private readonly RogueTalents _talents;

        public string Name { get { return "Backstab"; } }

        public float EnergyCost
        {
            get { return 60f - 3f * _talents.SlaughterFromTheShadows; }
        }

        public float Crit(CombatFactors combatFactors)
        {
            return (combatFactors.ProbMhCrit + .1f * _talents.PuncturingWounds) * combatFactors.ProbYellowHit;
        }

        public float CalcCpgDPS(Stats stats, CombatFactors combatFactors, CalculationOptionsRogue calcOpts, float numCPG, float cycleTime)
        {
            var baseDamage = BaseAttackDamage(stats, combatFactors);
            baseDamage *= TalentBonusDamage();
            baseDamage *= (1f + .35f * 0.1f * _talents.DirtyDeeds);
            baseDamage *= combatFactors.DamageReduction;

            var critDamage = baseDamage*CriticalDamageMultiplier(combatFactors)*Crit(combatFactors);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors), 0);

            return (critDamage + nonCritDamage)* numCPG / cycleTime;
        }

        private float BaseAttackDamage(Stats stats, CombatFactors combatFactors)
        {
            var attackDamage = combatFactors.MhNormalizedDamage;
            attackDamage += 465;
            attackDamage *= (1.5f + 0.01f * _talents.SinisterCalling);
            return attackDamage;
        }

        private float CriticalDamageMultiplier(CombatFactors combatFactors)
        {
            return (combatFactors.BaseCritMultiplier + .06f * _talents.Lethality);
        }

        private float TalentBonusDamage()
        {
            var talentBonuses = 1f;
            talentBonuses += 0.02f*_talents.FindWeakness;
            talentBonuses += 0.03f*_talents.Aggression;
            talentBonuses += 0.05f*_talents.BladeTwisting;
            talentBonuses += 0.1f*_talents.SurpriseAttacks;
            talentBonuses += 0.1f*_talents.Opportunity;
            return talentBonuses;
        }
    }
}