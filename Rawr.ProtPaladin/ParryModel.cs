using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtPaladin {
    public class ParryModel {
        private Character Character;
        private CalculationOptionsProtPaladin Options;
        private Stats Stats;

        private AttackTable AttackTable;
        private DefendTable DefendTable;

        public float BossAttackSpeed { get; private set; }
        public float WeaponSpeed { get; private set; }

        private void Calculate() {
            float baseBossAttackSpeed   = Options.BossAttackSpeed * (1f - Stats.BossAttackSpeedMultiplier);
            float baseWeaponSpeed       = Lookup.WeaponSpeed(Character, Stats);
            float bossAttackHaste       = 0.0f;
            float weaponHaste           = 0.0f;

            BossAttackSpeed             = baseBossAttackSpeed ;
            WeaponSpeed                 = baseWeaponSpeed;

            if (Options.UseParryHaste) {
                // Iterate on this a few times to get a 'stable' result
                for (int j = 0; j < 4; j++) {
                    bossAttackHaste = AttackTable.Parry * 0.24f * ((BossAttackSpeed / WeaponSpeed) + (BossAttackSpeed / 1.5f));
                    weaponHaste     = DefendTable.Parry * 0.24f * (WeaponSpeed / BossAttackSpeed);

                    BossAttackSpeed = baseBossAttackSpeed / (1.0f + bossAttackHaste);
                    WeaponSpeed     = baseWeaponSpeed / (1.0f + weaponHaste);
                }
            }else{
                // Simple adjust to the defender's speed if the attacker isn't parry hasted
                WeaponSpeed /= (1.0f + (DefendTable.Parry * 0.24f * (WeaponSpeed / BossAttackSpeed)));
                BossAttackSpeed = baseBossAttackSpeed;
            }
        }

        public ParryModel(Character character, Stats stats, CalculationOptionsProtPaladin options) {
            Character   = character;
            Stats       = stats;
            Options     = options;
            AttackTable = new AttackTable(character, stats, options);
            DefendTable = new DefendTable(character, stats, options);

            Calculate();
        }
    }
}
