using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin {
    public class ParryModel {
        private Character Character;
        private CalculationOptionsProtPaladin CalcOpts;
        private Stats Stats;

        private AttackTable AttackTable;
        private DefendTable DefendTable;

        public float WeaponSpeed { get; private set; }

        private void Calculate() {
            float bossAttackSpeed = CalcOpts.BossAttackSpeed * (1f - Stats.BossAttackSpeedMultiplier);
            WeaponSpeed     = Lookup.WeaponSpeed(Character, Stats) / (1.0f + (DefendTable.Parry * 0.24f * (WeaponSpeed / bossAttackSpeed)));
        }

        public ParryModel(Character character, Stats stats, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts) {
            Character   = character;
            Stats       = stats;
            CalcOpts    = calcOpts;
            AttackTable = new AttackTable(character, stats, calcOpts, bossOpts);
            DefendTable = new DefendTable(character, stats, calcOpts, bossOpts);
            Calculate();
        }
    }
}
