using System;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue.ComboPointGenerators
{
#if (SILVERLIGHT == false)
    [Serializable]
#endif

    public class SinisterStrike : ComboPointGenerator
    {
        public override string Name { get { return "Sinister Strike"; } }

        public override float EnergyCost(CombatFactors combatFactors, CalculationOptionsRogue calcOpts)
        {
            return 45f * combatFactors.Tier7FourPieceEnergyCostReduction
                - Talents.ImprovedSinisterStrike.Bonus 
                - (Crit(combatFactors, calcOpts) * Talents.FocusedAttacks.Bonus);
        }

        public override float Crit( CombatFactors combatFactors, CalculationOptionsRogue calcOpts )
        {
            return ( combatFactors.ProbMhCrit + CritBonusFromTurnTheTables(calcOpts) ) * combatFactors.ProbYellowHit;
		}

        public override float CalcCpgDps(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, Stats stats, CycleTime cycleTime)
        {
            var baseDamage = BaseAttackDamage(combatFactors);
            baseDamage *= Talents.Add(Talents.FindWeakness, Talents.Murder, Talents.Aggression, Talents.BladeTwisting, Talents.SurpriseAttacks, Talents.DirtyDeeds, Talents.HungerForBlood.Damage).Multiplier;
			baseDamage *= combatFactors.DamageReduction;
            var critDamage = baseDamage * CriticalDamageMultiplier(combatFactors) * Crit(combatFactors, calcOpts);
            var nonCritDamage = baseDamage * Math.Max(combatFactors.ProbYellowHit - Crit(combatFactors, calcOpts), 0);

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

        protected override float ComboPointsGeneratedPerAttack(CombatFactors combatFactors, CalculationOptionsRogue calcOpts)
        {
            return base.ComboPointsGeneratedPerAttack(combatFactors, calcOpts) + (Glyphs.GlyphOfSinisterStrike ? .5f : 0f);
        }
    }
}