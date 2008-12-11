namespace Rawr.Rogue
{
    public interface IFinisher
    {
        char Id { get; }
        float EnergyCost { get; }
        float CalcFinisherDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float cycleTime);
    }

    public static class Finishers
    {
        public static IFinisher Get(CalculationOptionsRogue calcOpts)
        {
            if (calcOpts.DPSCycle['r'] > 0)
            {
                return new Rupture();
            }
            
            if (calcOpts.DPSCycle['e'] > 0)
            {
                return new Evis();
            }

            return new NoFinisher();
        }

        public static IFinisher Get(char id)
        {
            if (id == 'r')
            {
                return new Rupture();
            }

            if (id == 'e')
            {
                return new Evis();
            }

            if (id == 's')
            {
                return new SnD();
            }

            return new NoFinisher();
        }
    }

    public class Rupture : IFinisher
    {
        public char Id { get { return 'r'; } }

        public float EnergyCost { get { return 25f; } }

        public float CalcFinisherDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float cycleTime)
        {
            float finisherDmg;
            switch (calcOpts.DPSCycle['r'])
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

            finisherDmg *= (1f + .1f * character.RogueTalents.SerratedBlades) * (1f + stats.BonusBleedDamageMultiplier);
            finisherDmg *= (1f - combatFactors.MissChance / 100f);
            if (character.RogueTalents.SurpriseAttacks < 1)
                finisherDmg *= (1f - combatFactors.MhDodgeChance / 100f);
            return finisherDmg / cycleTime;
        }
    }

    public class Evis : IFinisher
    {
        public char Id { get { return 'e'; } }

        public float EnergyCost { get { return 35f; } }

        public float CalcFinisherDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float cycleTime)
        {
            var evisMod = stats.AttackPower*calcOpts.DPSCycle['e']*.03f;
            var evisMin = 245f + (calcOpts.DPSCycle['e'] - 1f)*185f + evisMod;
            var evisMax = 365f + (calcOpts.DPSCycle['e'] - 1f)*185f + evisMod;

            var finisherDmg = (evisMin + evisMax)/2f;
            finisherDmg *= (1f + 0.05f*character.RogueTalents.ImprovedEviscerate);
            finisherDmg *= (1f + 0.02f*character.RogueTalents.Aggression);
            finisherDmg = finisherDmg * (1f - (combatFactors.MhCrit / 100f)) + (finisherDmg * 2f) * (combatFactors.MhCrit / 100f);
            finisherDmg *= (1f - (combatFactors.MissChance / 100f));
            if (character.RogueTalents.SurpriseAttacks < 1)
                finisherDmg *= (1f - (combatFactors.MhDodgeChance / 100f));

            finisherDmg *= combatFactors.DamageReduction;
            return finisherDmg / cycleTime;
        }
    }

    public class SnD : IFinisher
    {
        public char Id { get { return 's'; } }
        public float EnergyCost { get { return 25f; } }
        public float CalcFinisherDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float cycleTime)
        {
            return 0f;
        }
    }

    public class NoFinisher : IFinisher
    {
        public char Id { get { return 'z'; } }
        public float EnergyCost { get { return 0f; } }
        public float CalcFinisherDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float cycleTime)
        {
            return 0f;
        }
    }
}
