using System;

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

            var finisherDmg = (evisMin + evisMax)/2f;
            finisherDmg *= Talents.ImprovedEviscerate.Multiplier;
            finisherDmg *= Talents.FindWeakness.Multiplier;
            finisherDmg *= Talents.Aggression.Multiplier;
            finisherDmg = finisherDmg * (1f - combatFactors.ProbMhCrit) + (finisherDmg * 2f) * (combatFactors.ProbMhCrit + GlyphOfEviscerateBonus);
            finisherDmg *= (1f - (combatFactors.WhiteMissChance / 100f));

            if (!Talents.SurpriseAttacks.HasPoints)
                finisherDmg *= (1f - (combatFactors.MhDodgeChance / 100f));

            finisherDmg *= combatFactors.DamageReduction;
            return finisherDmg / cycleTime.Duration;
        }

        private static float GlyphOfEviscerateBonus
        {
            get { return Talents.Glyphs.Eviscerate ? .1f : 0f; }
        }
    }
}