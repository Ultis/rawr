using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    public class Mutilate : IComboPointGenerator
    {
        public string Name { get { return "Mutilate"; } }
        
		public float EnergyCost(CombatFactors combatFactors)
		{ 
			//Assume Mutiliate can only crit once, so Focused Attacks can only return 2 energy, and
			//not 2 energy for a MH crit, and another 2 energy (total 4) for a MH and OH crit
            return 60f * combatFactors.Tier7FourPieceEnergyCostReduction
                - (Crit(combatFactors) * Talents.FocusedAttacks.Bonus); 
		}

        public float Crit(CombatFactors combatFactors)
        {
            return combatFactors.ProbMhCrit + Talents.PuncturingWounds.Mutilate.Bonus;
		}

		public float ComboPointsGeneratedPerAttack
		{
			get { return 2f; }
		}

        public float CalcCpgDPS(Stats stats, CombatFactors combatFactors, CalculationOptionsRogue calcOpts, float numCpg, float cycleTime)
        {
            var baseDamage = BaseAttackDamage(combatFactors);
            baseDamage *= TalentBonusDamage();
            baseDamage *= BonusIfTargetIsPoisoned(calcOpts);
            baseDamage *= Talents.DirtyDeeds.Multiplier;
			baseDamage *= Talents.FindWeakness.Multiplier;
            baseDamage *= combatFactors.DamageReduction;

            var critDamage = baseDamage * CriticalDamageMultiplier(combatFactors) * Crit(combatFactors);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors), 0);

            return (critDamage + nonCritDamage)*(numCpg/2f)/cycleTime;
        }

        private static float BaseAttackDamage(CombatFactors combatFactors)
        {
            var damage = combatFactors.MhNormalizedDamage + 181;
            damage += combatFactors.OhNormalizedDamage + (181 * 2 *combatFactors.OffHandDamagePenalty);
            return damage;
        }

        private static float TalentBonusDamage()
        {
            return 1f + Talents.Add(Talents.FindWeakness, Talents.Opportunity);
        }

        private static float BonusIfTargetIsPoisoned(CalculationOptionsRogue calcOpts)
        {
            if (calcOpts.TempMainHandEnchant.IsDeadlyPoison
                || calcOpts.TempOffHandEnchant.IsDeadlyPoison
                || calcOpts.TempMainHandEnchant.Name == new WoundPoison().Name
                || calcOpts.TempOffHandEnchant.Name == new WoundPoison().Name)
            {
                return 1.5f;
            }

            return 1f;
        }

        private static float CriticalDamageMultiplier(CombatFactors combatFactors)
        {
            return combatFactors.BaseCritMultiplier + Talents.Lethality.Bonus;
        }
    }
}