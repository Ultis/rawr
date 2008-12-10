namespace Rawr.Rogue
{
    public interface IFinisher
    {
        float EnergyCost { get; }
        float CalcFinisherDamage(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors);
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
    }

    public class Rupture : IFinisher
    {
        public float EnergyCost { get { return 25f; } }
        public float CalcFinisherDamage(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors)
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
            return finisherDmg;
        }
    }

    public class Evis : IFinisher
    {
        public float EnergyCost { get { return 35f; } }

        public float CalcFinisherDamage(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors)
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
            return finisherDmg;
        }
    }

    public class NoFinisher : IFinisher
    {
        public float EnergyCost { get { return 0f; } }
        public float CalcFinisherDamage(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors)
        {
            return 0f;
        }
    }
}
