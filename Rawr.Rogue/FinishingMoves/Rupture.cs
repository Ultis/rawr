using System;

namespace Rawr.Rogue.FinishingMoves
{
    [Serializable]
    public class Rupture : FinisherBase
    {
        public override char Id { get { return 'R'; } }

        public override string Name { get { return "Rupture"; } }

        public override float EnergyCost(CombatFactors combatFactors, int rank)
        {
            var baseCost = 25f - Talents.RelentlessStrikes.Bonus;
            var missCost = baseCost * combatFactors.YellowMissChance * (1 - Talents.QuickRecovery.Bonus);
            var dodgeCost = baseCost * (Talents.SurpriseAttacks.HasPoints ? combatFactors.MhDodgeChance * (1 - Talents.QuickRecovery.Bonus) : 0);
            return baseCost + missCost + dodgeCost;
        }

        public override float CalcFinisherDPS( Stats stats, CombatFactors combatFactors, int rank, float cycleTime, WhiteAttacks whiteAttacks )
        {
            float finisherDmg;
            switch (rank)
            {
                case 5:
                    finisherDmg = 8f * (stats.AttackPower * 0.0375f + 217f); 
                    break;
                case 4:
                    finisherDmg = 7f * (stats.AttackPower * 0.03428571f + 199f); 
                    break;
                case 3:
                    finisherDmg = 6f * (stats.AttackPower * 0.03f + 181f);
                    break;
                case 2:
                    finisherDmg = 5f * (stats.AttackPower * 0.024f + 163f);
                    break;
                default:
                    finisherDmg = 4f * (stats.AttackPower * .015f + 145f);
                    break;
            }

            //Note:  Shadowstep isn't modeled because the energy cost isn't modeled
            //Note:  Bonus Physical Damage isn't modeled yet  (e.g. Savage Combat/Blood Frenzy) 
            //Note:  Need to Model Tier 7 2-Piece Bonus;

            finisherDmg *= ( 1f + Talents.Add(Talents.SerratedBlades.Rupture, Talents.BloodSpatter, Talents.FindWeakness) );
            finisherDmg *= Talents.DirtyDeeds.Multiplier;
            finisherDmg *= Talents.FindWeakness.Multiplier;
            finisherDmg *= ( 1f + stats.BonusBleedDamageMultiplier );
            finisherDmg *= ( 1f - combatFactors.YellowMissChance );
            finisherDmg *= combatFactors.Tier7TwoPieceRuptureBonusDamage;
            finisherDmg *= combatFactors.Tier8FourPieceRuptureCrit;
            if (!Talents.SurpriseAttacks.HasPoints)
                finisherDmg *= (1f - combatFactors.MhDodgeChance);
            return finisherDmg / cycleTime;
        }
    }
}