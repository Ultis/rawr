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
    }

    public interface IPoison
    {
        string Name { get; }
        float CalcPoisonDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits);
    }

    public class NoPoison : IPoison
    {
        public string Name
        {
            get { return "None"; }
        }

        public float CalcPoisonDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return 0f;
        }
    }

    public class DeadlyPoison : IPoison
    {
        public string Name
        {
            get { return "Deadly Poison"; }
        }

        public float CalcPoisonDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return 180f*(1f + character.RogueTalents.VilePoisons*.04f)/12f;
        }
    }

    public class InstantPoison : IPoison
    {
        public string Name
        {
            get { return "Instant Poison"; }
        }

        public float CalcPoisonDPS(Character character, Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return hits*combatFactors.ProbPoison*170f*(1f + character.RogueTalents.VilePoisons*0.04f);
        }
    }
}