using System.Diagnostics;
using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue {
    public class WhiteAttacks {
        public WhiteAttacks(CombatFactors combatFactors) {
            _combatFactors = combatFactors;
        }
        private readonly CombatFactors _combatFactors;
        public float MhHits { get { return MhSwingsPerSecond * (_combatFactors.ProbMhWhiteHit + _combatFactors.ProbMhCrit + _combatFactors.ProbGlancingHit); } }
        public float OhHits { get { return OhSwingsPerSecond * (_combatFactors.ProbOhWhiteHit + _combatFactors.ProbOhCrit + _combatFactors.ProbGlancingHit); } }
        public float MhCrits { get { return MhSwingsPerSecond * _combatFactors.ProbMhCrit; } }
        public float OhCrits { get { return OhSwingsPerSecond * _combatFactors.ProbOhCrit; } }
        public float CalcMhWhiteDps() {
            float dps  = _combatFactors.MhAvgDamage * 0.70f * _combatFactors.ProbGlancingHit;
                  dps += _combatFactors.MhAvgDamage * _combatFactors.BaseCritMultiplier * _combatFactors.ProbMhCrit;
                  dps += _combatFactors.MhAvgDamage * _combatFactors.ProbMhWhiteHit;
            float mhDps = dps * MhSwingsPerSecond * _combatFactors.MhDamageReduction;
            return mhDps * Talents.Add(Talents.FindWeakness, Talents.Murder, Talents.HungerForBlood.Damage).Multiplier;
        }
        public float CalcOhWhiteDps() {
            float dps  = _combatFactors.OhAvgDamage * 0.70f * _combatFactors.ProbGlancingHit;
                  dps += _combatFactors.OhAvgDamage * _combatFactors.BaseCritMultiplier * _combatFactors.ProbOhCrit;
                  dps += _combatFactors.OhAvgDamage * _combatFactors.ProbOhWhiteHit;
            float ohDps = dps * OhSwingsPerSecond * _combatFactors.OhDamageReduction;
            return ohDps * Talents.Add(Talents.FindWeakness, Talents.Murder, Talents.HungerForBlood.Damage).Multiplier;
        }
        public float MhSwingsPerSecond { get { return _combatFactors.MH.Speed == 0 ? 0 : (1f / _combatFactors.MHSpeed); } }
        public float OhSwingsPerSecond { get { return _combatFactors.OH.Speed == 0 ? 0 : (1f / _combatFactors.OHSpeed); } }
    }
}
