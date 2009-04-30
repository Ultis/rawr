using System;

namespace Rawr.Rogue.Poisons
{
    [Serializable]
    public class NoPoison : PoisonBase
    {
        public override string Name { get { return "None"; } }

        public override bool IsDeadlyPoison { get { return false; } }

        public override float CalcPoisonDPS(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits)
        {
            return 0f;
        }
    }
}