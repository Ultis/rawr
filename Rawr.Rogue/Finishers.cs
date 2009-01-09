using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Rawr.Rogue
{
	[Serializable]
	[XmlInclude(typeof(NoFinisher))]
	[XmlInclude(typeof(SnD))]
	[XmlInclude(typeof(Rupture))]
	[XmlInclude(typeof(Evis))]
	public abstract class FinisherBase
    {
		public abstract char Id { get; }
		public abstract string Name { get; }
	    public abstract float EnergyCost(CombatFactors combatFactors);
		public abstract float CalcFinisherDPS(Stats stats, CombatFactors combatFactors, int rank, float cycleTime);
    }

	[Serializable]
	public class Finishers : List<FinisherBase>
    {
        public Finishers()
        {
            Add(new NoFinisher());
            Add(new SnD());
            Add(new Rupture());
            Add(new Evis());
        }

        public static FinisherBase Get(string name)
        {
            foreach (var finisher in new Finishers())
            {
                if (name == finisher.Name)
                {
                    return finisher;
                }
            }
            return new NoFinisher();
        }
    }

	[Serializable]
	public class Rupture : FinisherBase
    {
        public override char Id { get { return 'R'; } }

		public override string Name { get { return "Rupture"; } }

		public override float EnergyCost(CombatFactors combatFactors)
		{
            var baseCost = 25f;
            var missCost = baseCost * combatFactors.YellowMissChance * (1 - Talents.QuickRecovery.Bonus);
            var dodgeCost = baseCost * (Talents.SurpriseAttacks.HasPoints ? combatFactors.MhDodgeChance * (1 - Talents.QuickRecovery.Bonus) : 0);
            return baseCost + missCost + dodgeCost;
        }

		public override float CalcFinisherDPS(Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
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

            finisherDmg *= Talents.Add(Talents.SerratedBlades, Talents.BloodSpatter, Talents.FindWeakness);
		    finisherDmg *= Talents.DirtyDeeds.Multiplier;
            finisherDmg *= (1f + stats.BonusBleedDamageMultiplier);
            finisherDmg *= (1f - combatFactors.YellowMissChance);
            if (!Talents.SurpriseAttacks.HasPoints)
                finisherDmg *= (1f - combatFactors.MhDodgeChance);
            return finisherDmg / cycleTime;
        }
    }

	[Serializable]
	public class Evis : FinisherBase
    {
		public override char Id { get { return 'E'; } }

		public override string Name { get { return "Evis"; } }

		public override float EnergyCost(CombatFactors combatFactors)
		{
		    var baseCost = 35f;
            var missCost = baseCost * combatFactors.YellowMissChance * (1-Talents.QuickRecovery.Bonus);
            var dodgeCost = baseCost * (Talents.SurpriseAttacks.HasPoints ? combatFactors.MhDodgeChance * (1 - Talents.QuickRecovery.Bonus) : 0);
            return baseCost + missCost + dodgeCost;
		}

		public override float CalcFinisherDPS(Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
        {
            var evisMod = stats.AttackPower*rank*.03f;
            var evisMin = 245f + (rank - 1f)*185f + evisMod;
            var evisMax = 365f + (rank - 1f)*185f + evisMod;

            var finisherDmg = (evisMin + evisMax)/2f;
            finisherDmg *= Talents.ImprovedEviscerate.Multiplier;
            finisherDmg *= Talents.Aggression.Multiplier;
            finisherDmg = finisherDmg * (1f - combatFactors.ProbMhCrit) + (finisherDmg * 2f) * combatFactors.ProbMhCrit;
            finisherDmg *= (1f - (combatFactors.WhiteMissChance / 100f));

            if (!Talents.SurpriseAttacks.HasPoints)
                finisherDmg *= (1f - (combatFactors.MhDodgeChance / 100f));

            finisherDmg *= combatFactors.DamageReduction;
            return finisherDmg / cycleTime;
        }
    }

	[Serializable]
	public class SnD : FinisherBase
    {
		public override char Id { get { return 'S'; } }
		public override string Name { get { return "SnD"; } }
		public override float EnergyCost(CombatFactors combatFactors) { return 25f; }
		public override float CalcFinisherDPS(Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
        {
            return 0f;
        }
    }

	[Serializable]
	public class NoFinisher : FinisherBase
    {
		public override char Id { get { return 'Z'; } }
		public override string Name { get { return "None"; } }
		public override float EnergyCost(CombatFactors combatFactors) { return 0f; }
		public override float CalcFinisherDPS(Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
        {
            return 0f;
        }
    }
}
