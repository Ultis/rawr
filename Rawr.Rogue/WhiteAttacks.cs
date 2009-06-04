using Rawr.Rogue.ClassAbilities;

namespace Rawr.Rogue
{
    public class WhiteAttacks
    {
        public WhiteAttacks(CombatFactors combatFactors)
        {
            _combatFactors = combatFactors;
        }

        private readonly CombatFactors _combatFactors;

        public float MhHits
        {
            get { return MhSwingsPerSecond * (_combatFactors.ProbMhWhiteHit + _combatFactors.ProbMhCrit + _combatFactors.ProbGlancingHit); }
        }

        public float OhHits
        {
            get { return OhSwingsPerSecond * (_combatFactors.ProbOhWhiteHit + _combatFactors.ProbOhCrit + _combatFactors.ProbGlancingHit); }
        }

        public float MhCrits
        {
            get { return MhSwingsPerSecond * _combatFactors.ProbMhCrit; }
        }

        public float OhCrits
        {
            get { return OhSwingsPerSecond * _combatFactors.ProbOhCrit; }
        }

        public float CalcMhWhiteDps()
        {
            var dps = _combatFactors.MhAvgDamage * 0.75f * _combatFactors.ProbGlancingHit;
            dps += _combatFactors.MhAvgDamage * _combatFactors.BaseCritMultiplier * _combatFactors.ProbMhCrit;
            dps += _combatFactors.MhAvgDamage * _combatFactors.ProbMhWhiteHit;
            var mhDps = dps * MhSwingsPerSecond * _combatFactors.DamageReduction;
            return mhDps * Talents.Add(Talents.FindWeakness, Talents.Murder, Talents.HungerForBlood.Damage).Multiplier;
        }

        public float CalcOhWhiteDps()
        {
            var dps = _combatFactors.OhAvgDamage * 0.75f * _combatFactors.ProbGlancingHit;
            dps += _combatFactors.OhAvgDamage * _combatFactors.BaseCritMultiplier * _combatFactors.ProbOhCrit;
            dps += _combatFactors.OhAvgDamage * _combatFactors.ProbOhWhiteHit;
            var ohDps = dps * OhSwingsPerSecond * _combatFactors.DamageReduction;
            return ohDps * Talents.Add(Talents.FindWeakness, Talents.Murder, Talents.HungerForBlood.Damage).Multiplier;
        }

        private float MhSwingsPerSecond
        {
			get { return (1f / _combatFactors.MainHand.Speed) * _combatFactors.BaseHaste; }
        }

        private float OhSwingsPerSecond
        {
			get { return (1f / _combatFactors.OffHand.Speed) * _combatFactors.BaseHaste; }
        }
    }
}
