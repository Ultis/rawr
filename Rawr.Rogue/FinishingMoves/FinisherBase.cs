using System;
using System.Xml.Serialization;

namespace Rawr.Rogue.FinishingMoves
{
    [Serializable]
    [XmlInclude(typeof(NoFinisher))]
    [XmlInclude(typeof(SnD))]
    [XmlInclude(typeof(Rupture))]
    [XmlInclude(typeof(Evis))]
    [XmlInclude(typeof(Envenom))]
    public abstract class FinisherBase
    {
        public abstract char Id { get; }
        public abstract string Name { get; }
        public abstract float EnergyCost(CombatFactors combatFactors, int rank);
        public abstract float CalcFinisherDPS( Stats stats, CombatFactors combatFactors, int rank, float cycleTime, WhiteAttacks whiteAttacks );
    }
}