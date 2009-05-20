using System;

namespace Rawr.Rogue.ComboPointGenerators
{
    [Serializable]
    public class SinisterStrike : ComboPointGenerator
    {
        public override string Name { get { return "Sinister Strike"; } }

        public override float EnergyCost(CombatFactors combatFactors)
        {
            return 45f * combatFactors.Tier7FourPieceEnergyCostReduction
                - Talents.ImprovedSinisterStrike.Bonus 
                - (Crit(combatFactors) * Talents.FocusedAttacks.Bonus);
        }

        public override float Crit(CombatFactors combatFactors)
        {
            return combatFactors.ProbMhCrit * combatFactors.ProbYellowHit;
		}

        public override float CalcCpgDPS(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, Stats stats, CycleTime cycleTime)
        {
            var baseDamage = BaseAttackDamage(combatFactors);
			baseDamage *= (1 + Talents.Add(Talents.FindWeakness, Talents.Aggression, Talents.BladeTwisting, Talents.SurpriseAttacks));
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
            damage += 180f;
            return damage;
        }

        private static float CriticalDamageMultiplier(CombatFactors combatFactors)
        {
            return (combatFactors.BaseCritMultiplier + Talents.Lethality.Bonus);
        }

        protected override float ComboPointsGeneratedPerAttack
        {
            get
            {
                return 1 + (Talents.Glyphs.GlyphOfSinisterStrike ? .5f : 0f);
            }
        }
    }
}