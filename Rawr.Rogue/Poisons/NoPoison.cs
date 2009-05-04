using System;

namespace Rawr.Rogue.Poisons
{
    [Serializable]
    public class NoPoison : PoisonBase
    {
        public override string Name { get { return "None"; } }

        public override bool IsDeadlyPoison { get { return false; } }

        public override float CalcPoisonDps(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits, float cycleTime)
        {
            return 0f;
        }
    }
}