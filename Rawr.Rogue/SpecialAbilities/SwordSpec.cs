using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue.SpecialAbilities
{
    public class SwordSpec
    {
        public float CalcDps(CalculationOptionsRogue calcOpts, CombatFactors combatFactors, WhiteAttacks whiteAttacks, CycleTime cycleTime)
        {
            var ssHits = 0f;
            if (combatFactors.MainHand.Type == Item.ItemType.OneHandSword)
            {
                //MH hits + CPG + finisher
                ssHits += whiteAttacks.MhHits * Talents.SwordSpecialization.Bonus;
                ssHits += calcOpts.CpGenerator.MhHitsNeeded(calcOpts.ComboPointsNeededForCycle()) * Talents.SwordSpecialization.Bonus * combatFactors.ProbMhWhiteHit;
                ssHits += 1f / cycleTime.Duration * Talents.SwordSpecialization.Bonus * combatFactors.ProbMhWhiteHit;
            }
            if (combatFactors.OffHand.Type == Item.ItemType.OneHandSword)
            {
                ssHits += whiteAttacks.OhHits * Talents.SwordSpecialization.Bonus;
            }

            var ssDPS = (ssHits * combatFactors.MhAvgDamage) * (1 - combatFactors.ProbMhCrit) + (ssHits * combatFactors.MhAvgDamage * combatFactors.BaseCritMultiplier) * combatFactors.ProbMhCrit;
            ssDPS *= combatFactors.DamageReduction;
            return ssDPS;
        }
    }
}