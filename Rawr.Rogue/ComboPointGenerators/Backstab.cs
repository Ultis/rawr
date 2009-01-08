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
            var baseDamage = BaseAttackDamage(combatFactors);
            baseDamage *= TalentBonusDamage();
            baseDamage *= Talents.DirtyDeeds.Multiplier;
            baseDamage *= combatFactors.DamageReduction;

            var critDamage = baseDamage*CriticalDamageMultiplier(combatFactors)*Crit(combatFactors);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors), 0);

            return (critDamage + nonCritDamage)* numCPG / cycleTime;
        }

        private static float BaseAttackDamage(CombatFactors combatFactors)
        {
            var attackDamage = combatFactors.MhNormalizedDamage;
            attackDamage += 465;
            attackDamage *= (1.5f + Talents.SinisterCalling.Bonus);
            return attackDamage;
        }

        private static float CriticalDamageMultiplier(CombatFactors combatFactors)
        {
            return (combatFactors.BaseCritMultiplier + Talents.Lethality.Bonus);
        }

        private static float TalentBonusDamage()
        {
            return Talents.Add(Talents.FindWeakness,
                                    Talents.Aggression,
                                    Talents.BladeTwisting,
                                    Talents.SurpriseAttacks,
                                    Talents.Opportunity);
        }
    }
}