using System.Collections.Generic;

namespace Rawr.Rogue
{
    public interface IFinisher
    {
        char Id { get; }
        string Name { get; }
        float EnergyCost { get; }
        float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime);
    }

    public class Finishers : List<IFinisher>
    {
        public Finishers()
        {
            Add(new NoFinisher());
            Add(new SnD());
            Add(new Rupture());
            Add(new Evis());
        }

        public static IFinisher Get(char id)
        {
            foreach(var finisher in new Finishers())
            {
                if(id == finisher.Id)
                {
                    return finisher;
                }
            }
            return new NoFinisher();
        }

        public static IFinisher Get(string name)
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

    public class Rupture : IFinisher
    {
        public char Id { get { return 'R'; } }

        public string Name { get { return "Rupture"; } }

        public float EnergyCost { get { return 25f; } }

        public float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
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

    public class Evis : IFinisher
    {
        public char Id { get { return 'E'; } }

        public string Name { get { return "Evis"; } }

        public float EnergyCost { get { return 35f; } }

        public float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
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

    public class SnD : IFinisher
    {
        public char Id { get { return 'S'; } }
        public string Name { get { return "SnD"; } }
        public float EnergyCost { get { return 25f; } }
        public float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
        {
            return 0f;
        }
    }

    public class NoFinisher : IFinisher
    {
        public char Id { get { return 'Z'; } }
        public string Name { get { return "None"; } }
        public float EnergyCost { get { return 0f; } }
        public float CalcFinisherDPS(RogueTalents talents, Stats stats, CombatFactors combatFactors, int rank, float cycleTime)
        {
            return 0f;
        }
    }
}
