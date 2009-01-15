namespace Rawr.DPSWarr
{
    public class WhiteAttacks
    {
        public WhiteAttacks(WarriorTalents talents, Stats stats, CombatFactors combatFactors)
        {
            _talents = talents;
            _stats = stats;
            _combatFactors = combatFactors;
        }

        private readonly WarriorTalents _talents;
        private readonly Stats _stats;
        private readonly CombatFactors _combatFactors;

        public float CalcMhWhiteDPS()
        {
            float wepSpeed = _combatFactors.MainHandSpeed;
            if (_combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand && _talents.TitansGrip != 1)
                wepSpeed += (1.5f - (0.5f * _talents.ImprovedSlam)) / 5;
            var mhWhiteDPS = _combatFactors.AvgMhWeaponDmg / wepSpeed;
            mhWhiteDPS *= (1 + _combatFactors.MhCrit * _combatFactors.BonusWhiteCritDmg
                            - (1 - _combatFactors.ProbMhWhiteHit) - (0.25f * 0.35f));
            mhWhiteDPS *= _combatFactors.DamageReduction;
            return mhWhiteDPS;
        }

        public float CalcOhWhiteDPS()
        {
            var ohWhiteDPS = _combatFactors.AvgOhWeaponDmg / _combatFactors.OffHandSpeed;
            ohWhiteDPS *= (1 + _combatFactors.OhCrit * _combatFactors.BonusWhiteCritDmg
                            - (1 - _combatFactors.ProbOhWhiteHit) - (0.25f * 0.35f));
            ohWhiteDPS *= _combatFactors.DamageReduction;
            if (_combatFactors.OffHand.DPS > 0 && (_combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || _talents.TitansGrip == 1))
                return ohWhiteDPS;
            else
                return 0f;
        }

        public float whiteRageGen()
        {
            float constant = 320.6f;
            float whiteRage = 15 * (CalcOhWhiteDPS() + CalcMhWhiteDPS()) / 4 / constant;

            /*whiteRage += ((14/8.0f*(1+_combatFactors.MhCrit-_combatFactors.WhiteMissChance-_combatFactors.MhDodgeChance)+
                         7/8.0f*(1+_combatFactors.OhCrit-_combatFactors.WhiteMissChance-_combatFactors.OhDodgeChance))*
                         (1/_combatFactors.TotalHaste));*/

            return whiteRage;
        }
    }
}
