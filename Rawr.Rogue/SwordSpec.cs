namespace Rawr.Rogue
{
    public class SwordSpec
    {
        public float CalcDPS(CombatFactors combatFactors, WhiteAttacks whiteAttacks, float numCPG, float cycleTime)
        {
            var ssHits = 0f;
            if (combatFactors.MainHand.Type == Item.ItemType.OneHandSword)
            {
                //MH hits + CPG + finisher
                ssHits += whiteAttacks.MhHits * Talents.SwordSpecialization.Bonus;
                ssHits += (numCPG / cycleTime) * Talents.SwordSpecialization.Bonus * combatFactors.ProbMhWhiteHit;
                ssHits += 1f / cycleTime * Talents.SwordSpecialization.Bonus * combatFactors.ProbMhWhiteHit;
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