namespace Rawr.Rogue
{
    public class SwordSpec
    {
        public static float CalcDPS(RogueTalents talents, CombatFactors combatFactors, WhiteAttacks whiteAttacks, float numCPG, float cycleTime)
        {
            var ssHits = 0f;
            if (combatFactors.MainHand.Type == Item.ItemType.OneHandSword)
            {
                //MH hits + CPG + finisher
                ssHits += whiteAttacks.MhHits * 0.01f * talents.SwordSpecialization;
                ssHits += (numCPG / cycleTime) * 0.01f * talents.SwordSpecialization * combatFactors.ProbMhWhiteHit;
                ssHits += 1f / cycleTime * 0.01f * talents.SwordSpecialization * combatFactors.ProbMhWhiteHit;
            }
            if (combatFactors.OffHand.Type == Item.ItemType.OneHandSword)
            {
                ssHits += whiteAttacks.OhHits * 0.01f * talents.SwordSpecialization;
            }

            var ssDPS = (ssHits * combatFactors.MhAvgDamage) * (1 - combatFactors.ProbMhCrit) + (ssHits * combatFactors.MhAvgDamage * combatFactors.BaseCritMultiplier) * combatFactors.ProbMhCrit;
            ssDPS *= combatFactors.DamageReduction;
            return ssDPS;
        }
    }
}