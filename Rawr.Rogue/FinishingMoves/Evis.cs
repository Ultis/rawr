using System;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue.FinishingMoves
{
    [Serializable]
    public class Evis : FinisherBase
    {
        public const string NAME = "Evis";

        public override char Id { get { return 'E'; } }

        public override string Name { get { return NAME; } }

        public override float EnergyCost(CombatFactors combatFactors, int rank)
        {
            var baseCost = 35f - Talents.RelentlessStrikes.Bonus;
            var missCost = baseCost * combatFactors.YellowMissChance * (1-Talents.QuickRecovery.Bonus);
            var dodgeCost = baseCost * (Talents.SurpriseAttacks.HasPoints ? combatFactors.MhDodgeChance * (1 - Talents.QuickRecovery.Bonus) : 0);
            return baseCost + missCost + dodgeCost;
        }

        public override float CalcFinisherDPS( CalculationOptionsRogue calcOpts, Stats stats, CombatFactors combatFactors, int rank, CycleTime cycleTime, WhiteAttacks whiteAttacks, CharacterCalculationsRogue displayValues )
        {
            var evisMod = stats.AttackPower*rank*.03f;
            var evisMin = 245f + (rank - 1f)*185f + evisMod;
            var evisMax = 365f + (rank - 1f)*185f + evisMod;

            var baseFinisherDmg = (evisMin + evisMax)/2f;
            baseFinisherDmg *= Talents.Add(Talents.ImprovedEviscerate, Talents.FindWeakness, Talents.Murder, Talents.Aggression, Talents.HungerForBlood.Damage).Multiplier;

            var critDamage = (baseFinisherDmg * 2f * CritChance(combatFactors, calcOpts));
            var nonCritDamage = baseFinisherDmg * ( 1f - CritChance(combatFactors, calcOpts) );

            var finisherDmg = critDamage + nonCritDamage;
            finisherDmg *= (1f - (combatFactors.WhiteMissChance / 100f));

            if (!Talents.SurpriseAttacks.HasPoints)
                finisherDmg *= (1f - (combatFactors.MhDodgeChance / 100f));

            finisherDmg *= combatFactors.DamageReduction;
            return finisherDmg / cycleTime.Duration;
        }

        private float CritChance(CombatFactors combatFactors, CalculationOptionsRogue calcOpts)
        {
            return combatFactors.ProbMhCrit + GlyphOfEviscerateBonus + CritBonusFromTurnTheTables(calcOpts);
        }

        private static float GlyphOfEviscerateBonus
        {
            get { return Glyphs.GlyphOfEviscerate ? .1f : 0f; }
        }
    }
}