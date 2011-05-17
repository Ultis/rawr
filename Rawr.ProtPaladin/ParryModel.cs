using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin {
    public class ParryModel {
        private Character Character;
        private CalculationOptionsProtPaladin CalcOpts;
        private BossOptions BossOpts;
        private Stats Stats;

        private AttackTable AttackTable;
        private DefendTable DefendTable;

        public float WeaponSpeed { get; private set; }

        private void Calculate() {
            float bossAttackSpeed = BossOpts.DefaultMeleeAttack.AttackSpeed / (1f - Stats.BossAttackSpeedReductionMultiplier);
            WeaponSpeed     = Lookup.WeaponSpeed(Character, Stats) / (1.0f + (DefendTable.Parry * 0.24f * (WeaponSpeed / bossAttackSpeed)));
        }

        public ParryModel(Character character, Stats stats, CalculationOptionsProtPaladin calcOpts, BossOptions bossOpts) {
            Character   = character;
            Stats       = stats;
            CalcOpts    = calcOpts;
            BossOpts    = bossOpts;
            AttackTable = new AttackTable(character, stats, calcOpts, bossOpts);
            DefendTable = new DefendTable(character, stats, calcOpts, bossOpts);
            Calculate();
        }
    }
}
