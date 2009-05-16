namespace Rawr.DPSWarr {
    public class WhiteAttacks {
        public WhiteAttacks(WarriorTalents talents, Stats stats, CombatFactors combatFactors, Character character) {
            _talents = talents;
            _stats = stats;
            _combatFactors = combatFactors;
            _character = character;
        }
        private readonly WarriorTalents _talents;
        private readonly Stats _stats;
        private readonly CombatFactors _combatFactors;
        private readonly Character _character;
        public float CalcMhWhiteDPS() {
            float wepSpeed = _combatFactors.MainHandSpeed;
            if (_combatFactors.MainHand.Slot == Item.ItemSlot.TwoHand && _talents.TitansGrip != 1) {
                wepSpeed += (1.5f - (0.5f * _talents.ImprovedSlam)) / 5;
            }
            float mhWhiteDPS = _combatFactors.AvgMhWeaponDmg * _combatFactors.ProbMhWhiteHit;
            mhWhiteDPS += _combatFactors.AvgMhWeaponDmg * _combatFactors.MhCrit * (1+_combatFactors.BonusWhiteCritDmg);
            mhWhiteDPS += _combatFactors.AvgMhWeaponDmg * _combatFactors.GlanceChance * 0.7f;
            mhWhiteDPS /= wepSpeed;
            //mhWhiteDPS *= (1 + _combatFactors.MhCrit * _combatFactors.BonusWhiteCritDmg - (1 - _combatFactors.ProbMhWhiteHit) - (_combatFactors.GlanceChance/* - (0.24f * 0.35f)*/); // ebs: WTF?!?
            mhWhiteDPS *= _combatFactors.DamageReduction;
            return mhWhiteDPS;
        }
        public float CalcOhWhiteDPS() {
            float ohWhiteDPS = _combatFactors.AvgOhWeaponDmg * _combatFactors.ProbOhWhiteHit;
            ohWhiteDPS += _combatFactors.AvgOhWeaponDmg * _combatFactors.OhCrit * (1 + _combatFactors.BonusWhiteCritDmg);
            ohWhiteDPS += _combatFactors.AvgOhWeaponDmg * _combatFactors.GlanceChance * 0.7f;
            ohWhiteDPS /= _combatFactors.OffHandSpeed;
            if (_combatFactors.OffHand.DPS > 0 && (_combatFactors.MainHand.Slot != Item.ItemSlot.TwoHand || _talents.TitansGrip == 1)) {
                return ohWhiteDPS;
            } else {
                return 0f;
            }
        }

        public float GetMHSwingRage()
        {
            // d = damage amt
            // c = rage conversion value
            // s = weapon speed
            // f = hit factor

            float c, d, s, f, rage;
            
            rage = 0.0f;
            c = 0.0091107836f * _character.Level * _character.Level + 3.225598133f * _character.Level + 4.2652911f;
            s = _combatFactors.MainHand.Speed;

            // regular hit
            d = _combatFactors.AvgMhWeaponDmg * _combatFactors.DamageBonus * _combatFactors.DamageReduction;
            f = 3.5f;
            rage += RageFormula(d, c, s, f) * _combatFactors.ProbMhWhiteHit;

            // crit
            d *= _combatFactors.BonusWhiteCritDmg;
            f = 7.0f;
            rage += RageFormula(d, c, s, f) * _combatFactors.MhCrit;

            // glance
            d = d / _combatFactors.MhCrit * 0.7f;
            f = 3.5f;
            rage += RageFormula(d, c, s, f) * _combatFactors.GlanceChance;

            // UW rage per swing
            rage += (_combatFactors.MainHand.Speed * (3f * _talents.UnbridledWrath) / 60.0f) * (1.0f - _combatFactors.WhiteMissChance);
            rage *= (1.0f + 0.25f * _talents.EndlessRage);
            return rage;
        }

        public float GetOHSwingRage() {
            // d = damage amt
            // c = rage conversion value
            // s = weapon speed
            // f = hit factor

            float c, d, s, f, rage; //StatConversion.LEVEL_80_COMBATRATING_MODIFIER;

            rage = 0.0f;
            c = 0.0091107836f * _character.Level * _character.Level + 3.225598133f * _character.Level + 4.2652911f;
            s = _combatFactors.OffHand.Speed;

            // regular hit
            d = _combatFactors.AvgOhWeaponDmg * _combatFactors.DamageBonus * _combatFactors.DamageReduction;
            f = 1.75f;
            rage += RageFormula(d, c, s, f) * _combatFactors.ProbOhWhiteHit;

            // crit
            d *= _combatFactors.BonusWhiteCritDmg;
            f = 3.5f;
            rage += RageFormula(d, c, s, f) * _combatFactors.OhCrit;

            // glance
            d = d / _combatFactors.OhCrit * 0.7f;
            f = 1.75f;
            rage += RageFormula(d, c, s, f) * _combatFactors.GlanceChance;

            // UW rage per swing
            rage += (_combatFactors.OffHand.Speed * (3f * _talents.UnbridledWrath) / 60.0f) * (1.0f - _combatFactors.WhiteMissChance);
            
            // Endless rage
            rage *= (1.0f + 0.25f * _talents.EndlessRage);
            
            return rage;
        }
        // Rage generated per second
        public float whiteRageGenPerSec() {

            float MHRage = (_character.MainHand != null && _character.MainHand.MaxDamage > 0 ? GetMHSwingRage() : 0f);
            float OHRage = (_character.OffHand  != null && _character.OffHand.MaxDamage  > 0 ? GetOHSwingRage() : 0f);

            // Rage per Second
            MHRage /= _combatFactors.MainHandSpeed;
            OHRage /= _combatFactors.OffHandSpeed;

            float rage = MHRage + OHRage;
            
            return rage;
            //float constant = 320.6f;
            
            //float whiteRage = 15 * (CalcOhWhiteDPS() + CalcMhWhiteDPS()) / 4 / constant;
            //whiteRage *= 1 + _talents.EndlessRage * 0.25f;
            //whiteRage += 1 * _talents.UnbridledWrath * 0.20f; //assuming a 20% chance to proc per talent point, no evidence to back this up tho

            /*whiteRage += ((14/8.0f*(1+_combatFactors.MhCrit-_combatFactors.WhiteMissChance-_combatFactors.MhDodgeChance)+
                         7/8.0f*(1+_combatFactors.OhCrit-_combatFactors.WhiteMissChance-_combatFactors.OhDodgeChance))*
                         (1/_combatFactors.TotalHaste));*/

            //return whiteRage;
        }
        public float RageFormula(float d, float c, float s, float f)
        {
            return 15.0f * d / 4.0f / c + f * s / 2.0f;
        }
    }
}
