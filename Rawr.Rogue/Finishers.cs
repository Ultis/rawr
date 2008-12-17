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
		public abstract float EnergyCost { get; }
		public abstract float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime);
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

		public override float EnergyCost { get { return 25f; } }

		public override float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
        {
            float finisherDmg;
            switch (rank)
            {
                case 5:
                    finisherDmg = 4f * (stats.AttackPower * .01f + 81f);
                    break;
                case 4:
                    finisherDmg = 5f * (stats.AttackPower * 0.02f + 92f);
                    break;
                case 3:
                    finisherDmg = 6f * (stats.AttackPower * 0.03f + 103f);
                    break;
                case 2:
                    finisherDmg = 7f * (stats.AttackPower * 0.03f + 114f);
                    break;
                default:
                    finisherDmg = 8f * (stats.AttackPower * 0.03f + 125f);
                    break;
            }

            finisherDmg *= (1f + .1f * talents.SerratedBlades) * (1f + stats.BonusBleedDamageMultiplier);
            finisherDmg *= (1f - combatFactors.WhiteMissChance / 100f);
            if (talents.SurpriseAttacks < 1)
                finisherDmg *= (1f - combatFactors.MhDodgeChance / 100f);
            return finisherDmg / cycleTime;
        }
    }

	[Serializable]
	public class Evis : FinisherBase
    {
		public override char Id { get { return 'E'; } }

		public override string Name { get { return "Evis"; } }

		public override float EnergyCost { get { return 35f; } }

		public override float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
        {
            var evisMod = stats.AttackPower*rank*.03f;
            var evisMin = 245f + (rank - 1f)*185f + evisMod;
            var evisMax = 365f + (rank - 1f)*185f + evisMod;

            var finisherDmg = (evisMin + evisMax)/2f;
            finisherDmg *= (1f + 0.05f*talents.ImprovedEviscerate);
            finisherDmg *= (1f + 0.02f*talents.Aggression);
            finisherDmg = finisherDmg * (1f - (combatFactors.MhCrit / 100f)) + (finisherDmg * 2f) * (combatFactors.MhCrit / 100f);
            finisherDmg *= (1f - (combatFactors.WhiteMissChance / 100f));
            if (talents.SurpriseAttacks < 1)
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
		public override float EnergyCost { get { return 25f; } }
		public override float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
        {
            return 0f;
        }
    }

	[Serializable]
	public class NoFinisher : FinisherBase
    {
		public override char Id { get { return 'Z'; } }
		public override string Name { get { return "None"; } }
		public override float EnergyCost { get { return 0f; } }
		public override float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
        {
            return 0f;
        }
    }
}
