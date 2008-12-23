namespace Rawr.Rogue
{
    public class WhiteAttacks
    {
        public WhiteAttacks(RogueTalents talents, Stats stats, CombatFactors combatFactors)
        {
            _talents = talents;
            _stats = stats;
            _combatFactors = combatFactors;
        }

        private readonly RogueTalents _talents;
        private readonly Stats _stats;
        private readonly CombatFactors _combatFactors;

        public float MhHits
        {
            get { return MhSwingsPerSecond * (_combatFactors.ProbMhWhiteHit + _combatFactors.ProbMhCrit + _combatFactors.ProbGlancingHit); }
        }

        public float OhHits
        {
            get { return OhSwingsPerSecond * (_combatFactors.ProbOhWhiteHit + _combatFactors.ProbOhCrit + _combatFactors.ProbGlancingHit); }
        }

        public float CalcMhWhiteDPS()
        {
            var dps = _combatFactors.MhAvgDamage * 0.75f * _combatFactors.ProbGlancingHit;
            dps += _combatFactors.MhAvgDamage * _combatFactors.BaseCritMultiplier * _combatFactors.ProbMhCrit;
            dps += _combatFactors.MhAvgDamage * _combatFactors.ProbMhWhiteHit;
            return dps*MhSwingsPerSecond*_combatFactors.DamageReduction;
        }

        public float CalcOhWhiteDPS()
        {
            var dps = _combatFactors.OhAvgDamage * 0.75f * _combatFactors.ProbGlancingHit;
            dps += _combatFactors.OhAvgDamage * _combatFactors.BaseCritMultiplier * _combatFactors.ProbOhCrit;
            dps += _combatFactors.OhAvgDamage * _combatFactors.ProbOhWhiteHit;
            return dps*OhSwingsPerSecond*_combatFactors.DamageReduction;
        }

        private float MhSwingsPerSecond
        {
            get { return _combatFactors.TotalHaste / _combatFactors.MainHand.Speed; }
        }

        private float OhSwingsPerSecond
        {
            get { return _combatFactors.TotalHaste / _combatFactors.OffHand.Speed; }
        }
    }
}
