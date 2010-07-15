using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr.Bosses {
    #region T9 (10) Content
    // ===== The Vault of Archavon ====================
    public class KoralonTheFlameWatcher_10 : BossHandler {
        public KoralonTheFlameWatcher_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Koralon the Flame Watcher";
            Content = TierLevels.T9_0;
            Instance = "The Vault of Archavon";
            Version = Versions.V_10N;
            Health = 4183500f;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
                IsTheDefaultMelee = true,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Fight Requirements
            Min_Tanks = 2;
            Min_Healers = 3;
            /* TODO:
             * I haven't done this fight yet so I can't really model it myself right now
             */
        }
    }
    #endregion
    #region T9 (10) H Content
    #endregion
    #region T9 (25) Content
    // ===== The Vault of Archavon ====================
    public class KoralonTheFlameWatcher_25 : KoralonTheFlameWatcher_10 {
        public KoralonTheFlameWatcher_25() {
            // If not listed here use values from defaults
            // Basics
            Content = TierLevels.T9_5;
            Version = Versions.V_25N;
            Health = 4183500f;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
                IsTheDefaultMelee = true,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            /* TODO:
             * I haven't done this fight yet so I can't really model it myself right now
             */
        }
    }
    #endregion
    #region T9 (25) H Content
    #endregion
}