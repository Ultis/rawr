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

        public float OhHits()
        {
            return (_combatFactors.TotalHaste/_combatFactors.OffHand.Speed)*_combatFactors.ProbOhWhiteHit;
        }

        public float MhHits()
        {
            return (_combatFactors.TotalHaste / _combatFactors.MainHand.Speed) * _combatFactors.ProbMhWhiteHit;
        }

        public float MhAvgDamage()
        {
            return _combatFactors.AvgMhWeaponDmg + (_stats.AttackPower / 14.0f) * _combatFactors.MainHand.Speed;
        }

        public float CalcMhWhiteDPS()
        {
            var mhWhiteDPS = MhAvgDamage()*MhHits();
            mhWhiteDPS = (1f - _combatFactors.MhCrit/100f)*mhWhiteDPS + (_combatFactors.MhCrit/100f)*(mhWhiteDPS*(2f*_combatFactors.BonusWhiteCritDmg));
            mhWhiteDPS *= _combatFactors.DamageReduction;
            return mhWhiteDPS;
        }

        public float CalcOhWhiteDPS()
        {
            var avgOHDmg = _combatFactors.AvgOhWeaponDmg + (_stats.AttackPower/14.0f)*_combatFactors.OffHand.Speed;
            avgOHDmg *= (0.25f + _talents.DualWieldSpecialization * 0.1f);

            var ohWhite = avgOHDmg*OhHits();
            ohWhite = (1f - _combatFactors.OhCrit/100f)*ohWhite + (_combatFactors.OhCrit/100f)*(ohWhite*(2f*_combatFactors.BonusWhiteCritDmg));
            ohWhite *= _combatFactors.DamageReduction;
            return ohWhite;
        }
    }
}
