using System;
using System.Xml.Serialization;

namespace Rawr.Rogue.Poisons
{
    [Serializable]
    [XmlInclude(typeof(NoPoison))]
    [XmlInclude(typeof(DeadlyPoison))]
    [XmlInclude(typeof(InstantPoison))]
    [XmlInclude(typeof(WoundPoison))]
    public abstract class PoisonBase
    {
        public abstract string Name { get; }
        public abstract bool IsDeadlyPoison { get; }
        public abstract float CalcPoisonDps(Stats stats, CalculationOptionsRogue calcOpts, CombatFactors combatFactors, float hits, float cycleTime);
    }
}