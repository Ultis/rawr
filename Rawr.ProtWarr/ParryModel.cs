using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.ProtWarr {
    public class ParryModel {
        public ParryModel(Character character, Stats stats) {
            Character   = character;
            Stats       = stats;
            Options     = character.CalculationOptions as CalculationOptionsProtWarr;
            Talents     = character.WarriorTalents;
            AttackTable = new AttackTable(character, stats);
            DefendTable = new DefendTable(character, stats);

            Calculate();
        }
        #region Variables
        private Character Character;
        private CalculationOptionsProtWarr Options;
        private Stats Stats;
        private WarriorTalents Talents;

        private AttackTable AttackTable;
        private DefendTable DefendTable;

        public float BossAttackSpeed { get; private set; }
        public float WeaponSpeed { get; private set; }
        #endregion
        private void Calculate() {
            float baseBossAttackSpeed   = Options.BossAttackSpeed;
            float baseWeaponSpeed       = Lookup.WeaponSpeed(Character, Stats);
            float bossAttackHaste       = 0.0f;
            float weaponHaste           = 0.0f;

            BossAttackSpeed             = baseBossAttackSpeed;
            WeaponSpeed                 = baseWeaponSpeed;

            if (Options.UseParryHaste) {
                // Iterate on this a few times to get a 'stable' result
                for (int j=0;j<4;j++) {
                    weaponHaste = DefendTable.Parry * 0.24f * (WeaponSpeed / BossAttackSpeed);
                    // Unrelenting Assault 'Revenge Spam' builds have 1.0s GCD instead of 1.5s
                    bossAttackHaste = AttackTable.Parry * 0.24f * ((BossAttackSpeed / WeaponSpeed) + (BossAttackSpeed / (Talents.UnrelentingAssault == 2?1f:1.5f)));
                    
                    WeaponSpeed     = baseWeaponSpeed / (1.0f + weaponHaste);
                    BossAttackSpeed = baseBossAttackSpeed / (1.0f + bossAttackHaste);
                }
            }else{
                // Simple adjust to the defender's speed if the attacker isn't parry hasted
                WeaponSpeed /= (1.0f + (DefendTable.Parry * 0.24f * (WeaponSpeed / BossAttackSpeed)));
            }
        }
    }
}
