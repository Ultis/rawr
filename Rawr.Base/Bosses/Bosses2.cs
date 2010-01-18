using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr.Bosses {
    #region T10 (10) Content
    // ===== Icecrown Citadel =========================
    // The Lower Spire
    public class LordMarrowgar_10 : BossHandler {
        public LordMarrowgar_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Lord Marrowgar";
            Content = TierLevels.T10_0;
            Instance = "Icecrown Citadel";
            Version = Versions.V_10;
            Health = 6972500;
            BerserkTimer = 19 * 60;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks = 2;
            Min_Healers = 2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack
            {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Saber Lash - Inflicts 200% weapon damage split evenly between the target and its 2 nearest allies.
            Attacks.Add(new Attack
            {
                Name = "Saber Lash",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 2f,
                AttackSpeed = 2.0f,
            });
            // Coldflame - Inflicts 6,000 Frost damage every 1 second for 3 seconds to anyone caught by the moving line of frost.
            Attacks.Add(new Attack
            {
                Name = "Coldflame",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = 6000,
                MaxNumTargets = Max_Players,
                AttackSpeed = 6.0f,
            });
            // Bone Spike Graveyard
            //   Hurls a spike at a random player, impaling all players between the boss and the target on Bone Spikes,
            //   inflicting 9,000 direct Physical damage, and additional 10% health as damage every 1 second until the
            //   spike is destroyed.
            // The player that is spiked is considered unbreakably stunned
            Attacks.Add(new Attack
            {
                Name = "Bone Spike Graveyard",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 9000 + (20000 * 0.10f * 3), // 9k initial + 20k avg dps hp * 10% * 3 sec to break it
                MaxNumTargets = Max_Players,
                AttackSpeed = 6.0f,
            });
            Stuns.Add(new Impedence() {
                Frequency = Attacks[Attacks.Count-1].AttackSpeed,
                Duration = 3000,
                Chance = 1f / (Max_Players - Min_Tanks),
                Breakable = false,
            });
            // Bone Storm
            //   Inflicts 6,000 Physical damage every 2 seconds to players caught in the Bone Storm.
            //   The entire storm lasts ~20 seconds.
            Attacks.Add(new Attack
            {
                Name = "Bone Spike Graveyard",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 6000 * 20 / 2 * 0.25f, // 6k per tick for 2 sec. 25% mod as people will be running away to take less damage
                MaxNumTargets = Max_Players,
                AttackSpeed = 45f + 22f,
            });
            Moves.Add(new Impedence()
            {
                Frequency = 45f + 22f,
                Duration = (18f + 2f + 2f) * 1000f,
                Chance = 1f,
                Breakable = false,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            /* TODO:
             */
        }
    }
    // - Lady Deathwhisper
    /*public class LadyDeathwhisper_10 : BossHandler
    {
        public LadyDeathwhisper_10()
        {
            // If not listed here use values from defaults
            // Basics
            Name = "Lady Deathwhisper";
            Content = TierLevels.T10_0;
            Instance = "Icecrown Citadel";
            Version = Versions.V_10;
            Health = 3346800f + 13992000f; // Mana Barrier, have to destroy mana before dps'g boss
            BerserkTimer = 10 * 60;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks = 2;
            Min_Healers = 2;
            // Resistance
            // Attacks
            // Shadow Bolt - Inflicts 7,438 to 9,562 Shadow damage to the current target.
            Attacks.Add(new Attack() {
                Name = "Shadow Bolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (7438 + 9562) / 2f,
                MaxNumTargets = Max_Players,
                AttackSpeed = 2.0f,
            });
            // TODO: Death and Decay - Inflicts 4,500 Shadow damage every 1 second to enemies within the area of the spell. The entire spell lasts 10 seconds.
            // TODO: Dark Empowerment - Empowers a random Adherent or Fanatic, causing them to deal area damage with spells. Also makes the target immune to interrupts.
            // Adds
            // Adds come 3 at a time, either 2 Adherents 1 Fanatic or 1 Adherent 2 Fanatics
            // They will also randomly respawn as Reanimated or Mutated (? forgot word used)
            MultiTargsPerc  = 0.00f;
            MultiTargsPerc -= 0.30f; // Phase 2 has no adds, assuming 30% of fight in Phase 2
            float uptime = (BerserkTimer / 60f) * 35f; // Every 60 seconds and up for 35 sec before back on boss
            MultiTargsPerc -= (1f - uptime) * 0.70f; // Phase 1 has adds, marking the downtime instead of uptime
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // TODO:
             
        }
    }*/
    // - Gunship Event
    // - Deathbringer Saurfang
    // The Plagueworks
    // - Festergut
    // - Rotface
    // - Professor Putricide
    // The Crimson Hall
    // - Blood Prince Council
    // - Blood-Queen Lana'thel
    // The Frostwing Hall
    // - Valathria Dreamwalker
    // - Sindragosa
    // The Frozen Throne
    // - The Lich King
    #endregion
    #region T10 (25) Content
    // ===== Icecrown Citadel =========================
    // The Lower Spire
    public class LordMarrowgar_25 : BossHandler
    {
        public LordMarrowgar_25()
        {
            // If not listed here use values from defaults
            // Basics
            Name = "Lord Marrowgar";
            Content = TierLevels.T10_5;
            Instance = "Icecrown Citadel";
            Version = Versions.V_25;
            Health = 23706500f;
            BerserkTimer = 19 * 60;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 3;
            Min_Healers = 4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack
            {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Saber Lash - Inflicts 200% weapon damage split evenly between the target and its 2 nearest allies.
            Attacks.Add(new Attack
            {
                Name = "Saber Lash",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 3f,
                AttackSpeed = 2.0f,
            });
            // Coldflame - Inflicts 6,000 Frost damage every 1 second for 3 seconds to anyone caught by the moving line of frost.
            Attacks.Add(new Attack
            {
                Name = "Coldflame",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = 8000,
                MaxNumTargets = Max_Players,
                AttackSpeed = 6.0f,
            });
            // Bone Spike Graveyard
            //   Hurls a spike at a random player, impaling all players between the boss and the target on Bone Spikes,
            //   inflicting 9,000 direct Physical damage, and additional 10% health as damage every 1 second until the
            //   spike is destroyed.
            // The player that is spiked is considered unbreakably stunned
            Attacks.Add(new Attack
            {
                Name = "Bone Spike Graveyard",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 9000 + (20000 * 0.10f * 3), // 9k initial + 20k avg dps hp * 10% * 3 sec to break it
                MaxNumTargets = Max_Players,
                AttackSpeed = 6.0f,
            });
            Stuns.Add(new Impedence()
            {
                Frequency = Attacks[Attacks.Count - 1].AttackSpeed,
                Duration = 3000,
                Chance = 1f / (Max_Players - Min_Tanks),
                Breakable = false,
            });
            // Bone Storm
            //   Inflicts 6,000 Physical damage every 2 seconds to players caught in the Bone Storm.
            //   The entire storm lasts ~20 seconds.
            Attacks.Add(new Attack
            {
                Name = "Bone Spike Graveyard",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 12000 * 20 / 2 * 0.25f, // 6k per tick for 2 sec. 25% mod as people will be running away to take less damage
                MaxNumTargets = Max_Players,
                AttackSpeed = 45f + 22f,
            });
            Moves.Add(new Impedence()
            {
                Frequency = 45f + 22f,
                Duration = (18f + 2f + 2f) * 1000f,
                Chance = 1f,
                Breakable = false,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            /* TODO:
             */
        }
    }
    // - Lady Deathwhisper
    // - Gunship Event
    // - Deathbringer Saurfang
    // The Plagueworks
    // - Festergut
    // - Rotface
    // - Professor Putricide
    // The Crimson Hall
    // - Blood Prince Council
    // - Blood-Queen Lana'thel
    // The Frostwing Hall
    // - Valathria Dreamwalker
    // - Sindragosa
    // The Frozen Throne
    // - The Lich King
    #endregion
    #region T10 (10) H Content
    // ===== Icecrown Citadel =========================
    // The Lower Spire
    public class LordMarrowgar_10H : BossHandler
    {
        public LordMarrowgar_10H()
        {
            // If not listed here use values from defaults
            // Basics
            Name = "Lord Marrowgar";
            Content = TierLevels.T10_5;
            Instance = "Icecrown Citadel";
            Version = Versions.V_10H;
            Health = 6972500;
            BerserkTimer = 19 * 60;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks = 2;
            Min_Healers = 2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack
            {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Saber Lash - Inflicts 200% weapon damage split evenly between the target and its 2 nearest allies.
            Attacks.Add(new Attack
            {
                Name = "Saber Lash",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 2f,
                AttackSpeed = 2.0f,
            });
            // Coldflame - Inflicts 6,000 Frost damage every 1 second for 3 seconds to anyone caught by the moving line of frost.
            Attacks.Add(new Attack
            {
                Name = "Coldflame",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = 6000,
                MaxNumTargets = Max_Players,
                AttackSpeed = 6.0f,
            });
            // Bone Spike Graveyard
            //   Hurls a spike at a random player, impaling all players between the boss and the target on Bone Spikes,
            //   inflicting 9,000 direct Physical damage, and additional 10% health as damage every 1 second until the
            //   spike is destroyed.
            // The player that is spiked is considered unbreakably stunned
            Attacks.Add(new Attack
            {
                Name = "Bone Spike Graveyard",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 9000 + (20000 * 0.10f * 3), // 9k initial + 20k avg dps hp * 10% * 3 sec to break it
                MaxNumTargets = Max_Players,
                AttackSpeed = 6.0f,
            });
            Stuns.Add(new Impedence()
            {
                Frequency = Attacks[Attacks.Count - 1].AttackSpeed,
                Duration = 3000,
                Chance = 1f / (Max_Players - Min_Tanks),
                Breakable = false,
            });
            // Bone Storm
            //   Inflicts 6,000 Physical damage every 2 seconds to players caught in the Bone Storm.
            //   The entire storm lasts ~20 seconds.
            Attacks.Add(new Attack
            {
                Name = "Bone Spike Graveyard",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 12000 * 20 / 2 * 0.25f, // 6k per tick for 2 sec. 25% mod as people will be running away to take less damage
                MaxNumTargets = Max_Players,
                AttackSpeed = 45f + 22f,
            });
            Moves.Add(new Impedence()
            {
                Frequency = 45f + 22f,
                Duration = (18f + 2f + 2f) * 1000f,
                Chance = 1f,
                Breakable = false,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            /* TODO:
             */
        }
    }
    // - Lady Deathwhisper
    // - Gunship Event
    // - Deathbringer Saurfang
    // The Plagueworks
    // - Festergut
    // - Rotface
    // - Professor Putricide
    // The Crimson Hall
    // - Blood Prince Council
    // - Blood-Queen Lana'thel
    // The Frostwing Hall
    // - Valathria Dreamwalker
    // - Sindragosa
    // The Frozen Throne
    // - The Lich King
    #endregion
    #region T10 (25) H Content
    // ===== Icecrown Citadel =========================
    // The Lower Spire
    public class LordMarrowgar_25H : BossHandler
    {
        public LordMarrowgar_25H()
        {
            // If not listed here use values from defaults
            // Basics
            Name = "Lord Marrowgar";
            Content = TierLevels.T10_9;
            Instance = "Icecrown Citadel";
            Version = Versions.V_25H;
            Health = 23706500f;
            BerserkTimer = 19 * 60;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 3;
            Min_Healers = 4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack
            {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Saber Lash - Inflicts 200% weapon damage split evenly between the target and its 2 nearest allies.
            Attacks.Add(new Attack
            {
                Name = "Saber Lash",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = StandardMeleePerHit[(int)Content],
                MaxNumTargets = 3f,
                AttackSpeed = 2.0f,
            });
            // Coldflame - Inflicts 6,000 Frost damage every 1 second for 3 seconds to anyone caught by the moving line of frost.
            Attacks.Add(new Attack
            {
                Name = "Coldflame",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = 11000,
                MaxNumTargets = Max_Players,
                AttackSpeed = 6.0f,
            });
            // Bone Spike Graveyard
            //   Hurls a spike at a random player, impaling all players between the boss and the target on Bone Spikes,
            //   inflicting 9,000 direct Physical damage, and additional 10% health as damage every 1 second until the
            //   spike is destroyed.
            // The player that is spiked is considered unbreakably stunned
            Attacks.Add(new Attack
            {
                Name = "Bone Spike Graveyard",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 9000 + (20000 * 0.10f * 3), // 9k initial + 20k avg dps hp * 10% * 3 sec to break it
                MaxNumTargets = Max_Players,
                AttackSpeed = 6.0f,
            });
            Stuns.Add(new Impedence()
            {
                Frequency = Attacks[Attacks.Count - 1].AttackSpeed,
                Duration = 3000,
                Chance = 1f / (Max_Players - Min_Tanks),
                Breakable = false,
            });
            // Bone Storm
            //   Inflicts 6,000 Physical damage every 2 seconds to players caught in the Bone Storm.
            //   The entire storm lasts ~20 seconds.
            Attacks.Add(new Attack
            {
                Name = "Bone Spike Graveyard",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 14000 * 20 / 2 * 0.25f, // 6k per tick for 2 sec. 25% mod as people will be running away to take less damage
                MaxNumTargets = Max_Players,
                AttackSpeed = 45f + 22f,
            });
            Moves.Add(new Impedence()
            {
                Frequency = 45f + 22f,
                Duration = (18f + 2f + 2f) * 1000f,
                Chance = 1f,
                Breakable = false,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            /* TODO:
             */
        }
    }
    // - Lady Deathwhisper
    // - Gunship Event
    // - Deathbringer Saurfang
    // The Plagueworks
    // - Festergut
    // - Rotface
    // - Professor Putricide
    // The Crimson Hall
    // - Blood Prince Council
    // - Blood-Queen Lana'thel
    // The Frostwing Hall
    // - Valathria Dreamwalker
    // - Sindragosa
    // The Frozen Throne
    // - The Lich King
    #endregion
}