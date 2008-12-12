using System.Collections.Generic;

namespace Rawr.Rogue
{
    public class PoisonList : List<IPoison>
    {
        public PoisonList()
        {
            Add(new NoPoison());
            Add(new DeadlyPoison());
            Add(new InstantPoison());
        }

        public static IPoison Get(string poisonName)
        {
            foreach(var poison in new PoisonList())
            {
                if(poison.Name == poisonName)
                {
                    return poison;
                }
            }

            return new NoPoison();
        }
    }

    public interface IPoison
    {
        string Name { get; }
        bool IsDeadlyPoison { get; }
        float CalcPoisonDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits);
    }

    public class NoPoison : IPoison
    {
        public string Name { get { return "None"; } }

        public bool IsDeadlyPoison { get { return false; } }

        public float CalcPoisonDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return 0f;
        }
    }

    public class DeadlyPoison : IPoison
    {
        public string Name { get { return "Deadly Poison"; } }

        public bool IsDeadlyPoison { get { return true; } }

        public float CalcPoisonDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return 180f * VilePoison.DamageMultiplier(character) / 12f;
        }
    }

    public class InstantPoison : IPoison
    {
        public string Name { get { return "Instant Poison"; } }

        public bool IsDeadlyPoison { get { return false; } }

        public float CalcPoisonDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return hits*combatFactors.ProbPoisonHit*(300f + .1f*stats.AttackPower)*VilePoison.DamageMultiplier(character)*(.2f + ImprovedPoisons.IncreasedApplicationChance(character));
        }
    }

    public static class VilePoison
    {
        private static readonly float[] _multipliers = new[] { 1f, 1.07f, 1.14f, 1.2f };
        public static float DamageMultiplier(Character character)
        {
            return _multipliers[character.RogueTalents.VilePoisons];
        }
    }

    public static class ImprovedPoisons
    {
        public static float IncreasedApplicationChance(Character character)
        {
            return character.RogueTalents.ImprovedPoisons*.02f;
        }
    }
}