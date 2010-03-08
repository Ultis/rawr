using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr.Bosses {
    #region T10 (Array) Content
    // ===== Icecrown Citadel =========================
    public class MultiDiffBoss : List<BossHandler>
    {
        public MultiDiffBoss() {
            // Initialize
            //this = new List<BossHandler>() { };
            this.Add(new BossHandler());
            this.Add(new BossHandler());
            this.Add(new BossHandler());
            this.Add(new BossHandler());
            // Basic Setups we don't want to repeat over and over again
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9 };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H };
            // Fight Requirements
            Min_Tanks   = new int[] {  2,  2,  2,  2 } ;
            Min_Healers = new int[] {  2,  5,  2,  5 } ;
        }
        #region Variable Convenience Overrides
        public string Name { get { return this[0].Name; } set { this[0].Name = value; } }
        public string Instance { get { return this[0].Instance; } set { this[0].Instance = value; } }
        public BossHandler.TierLevels[] Content
        {
            get
            {
                return new BossHandler.TierLevels[] {
                    this[0].Content,
                    this[1].Content,
                    this[2].Content,
                    this[3].Content,
                };
            }
            set
            {
                int i = 0;
                this[i].Content = value[i]; i++;
                this[i].Content = value[i]; i++;
                this[i].Content = value[i]; i++;
                this[i].Content = value[i];
            }
        }
        public BossHandler.Versions[] Version
        {
            get
            {
                return new BossHandler.Versions[] {
                    this[0].Version,
                    this[1].Version,
                    this[2].Version,
                    this[3].Version,
                };
            }
            set
            {
                int i = 0;
                this[i].Version = value[i]; i++;
                this[i].Version = value[i]; i++;
                this[i].Version = value[i]; i++;
                this[i].Version = value[i];
            }
        }
        public float[] Health
        {
            get
            {
                return new float[] {
                    this[0].Health,
                    this[1].Health,
                    this[2].Health,
                    this[3].Health,
                };
            }
            set
            {
                int i = 0;
                this[i].Health = value[i]; i++;
                this[i].Health = value[i]; i++;
                this[i].Health = value[i]; i++;
                this[i].Health = value[i];
            }
        }
        public int[] BerserkTimer
        {
            get
            {
                return new int[] {
                    this[0].BerserkTimer,
                    this[1].BerserkTimer,
                    this[2].BerserkTimer,
                    this[3].BerserkTimer,
                };
            }
            set
            {
                int i = 0;
                this[i].BerserkTimer = value[i]; i++;
                this[i].BerserkTimer = value[i]; i++;
                this[i].BerserkTimer = value[i]; i++;
                this[i].BerserkTimer = value[i];
            }
        }
        public int[] Max_Players
        {
            get
            {
                return new int[] {
                    this[0].Max_Players,
                    this[1].Max_Players,
                    this[2].Max_Players,
                    this[3].Max_Players,
                };
            }
            set
            {
                int i = 0;
                this[i].Max_Players = value[i]; i++;
                this[i].Max_Players = value[i]; i++;
                this[i].Max_Players = value[i]; i++;
                this[i].Max_Players = value[i];
            }
        }
        public int[] Min_Tanks
        {
            get
            {
                return new int[] {
                    this[0].Min_Tanks,
                    this[1].Min_Tanks,
                    this[2].Min_Tanks,
                    this[3].Min_Tanks,
                };
            }
            set
            {
                int i = 0;
                this[i].Min_Tanks = value[i]; i++;
                this[i].Min_Tanks = value[i]; i++;
                this[i].Min_Tanks = value[i]; i++;
                this[i].Min_Tanks = value[i];
            }
        }
        public int[] Min_Healers
        {
            get
            {
                return new int[] {
                    this[0].Min_Healers,
                    this[1].Min_Healers,
                    this[2].Min_Healers,
                    this[3].Min_Healers,
                };
            }
            set
            {
                int i = 0;
                this[i].Min_Healers = value[i]; i++;
                this[i].Min_Healers = value[i]; i++;
                this[i].Min_Healers = value[i]; i++;
                this[i].Min_Healers = value[i];
            }
        }
        public BossHandler BossByVersion(BossHandler.Versions v) { return this[(int)v]; }
        #endregion
    }
    // The Lower Spire
    public class LordMarrowgar : MultiDiffBoss
    {
        public LordMarrowgar()
        {
            // If not listed here use values from defaults
            // Basics
            Name = "Lord Marrowgar";
            Instance = "Icecrown Citadel";
            Health = new float[] { 6972500, 23706500, 10500000, 31400000 };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            // Fight Requirements
            Min_Tanks   = new int[] {  2,  3,  2,  3 } ;
            Min_Healers = new int[] {  2,  5,  3,  6 } ;
            // Resistance
            // Attacks
            int[] temps, temps2;
            float[] temps3;
            for (int i = 0; i < 4; i++) {
                this[i].Attacks.Add(new Attack {
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)this[i].Content],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2.0f,
                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,
                });
                // Saber Lash - Inflicts 200% weapon damage split evenly between the target and its 2 nearest allies.
                this[i].Attacks.Add(new Attack {
                    Name = "Saber Lash",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)this[i].Content],
                    MaxNumTargets = this[i].Min_Tanks,
                    AttackSpeed = 2.0f,
                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,
                });
                // Coldflame - Inflicts 6,000 Frost damage every 1 second for 3 seconds to anyone caught by the moving line of frost.
                temps = new int[] { 6000, 8000, 9000, 11000 };
                this[i].Attacks.Add(new DoT {
                    Name = "Coldflame",
                    DamageType = ItemDamageType.Frost,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    NumTicks = 3,
                    DamagePerTick = temps[i] / 3,
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 6.0f,
                });
                // Bone Spike Graveyard
                //   Hurls a spike at a random player, impaling all players between the boss and the target on Bone Spikes,
                //   inflicting 9,000 direct Physical damage, and additional 10% health as damage every 1 second until the
                //   spike is destroyed.
                // The player that is spiked is considered unbreakably stunned
                temps  = new int[] { 1, 3, 1, 3 };
                temps2 = new int[] { 9000, 10000, 11000, 12000 };
                temps3 = new float[] { 0.10f, 0.10f, 0.10f, 0.10f };
                int duration = 3; // Assuming you will be broken out within 3 sec
                this[i].Attacks.Add(new DoT {
                    Name = "Bone Spike Graveyard",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamagePerHit = 9000,
                    DamagePerTick = (temps3[i] * 0.10f * duration),
                    DamageIsPerc = true,
                    MaxNumTargets = temps[i],
                    AttackSpeed = 6.0f,
                    IgnoresMTank = true,
                    IgnoresOTank = true,
                    IgnoresTTank = true,
                });
                this[i].Stuns.Add(new Impedence() {
                    Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                    Duration = duration * 1000,
                    Chance = 1f / (this[i].Max_Players - this[i].Min_Tanks),
                    Breakable = false,
                });
                // Bone Storm
                //   Inflicts 6,000 Physical damage every 2 seconds to players caught in the Bone Storm.
                //   The entire storm lasts ~20 seconds.
                temps = new int[] { 6000, 8000, 10000, 12000 };
                this[i].Attacks.Add(new Attack {
                    Name = "Bone Spike Graveyard",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = temps[i] * 20 / 2 * 0.25f, // 6k per tick for 2 sec. 25% mod as people will be running away to take less damage
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 45f + 22f,
                });
                this[i].Moves.Add(new Impedence() {
                    Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                    Duration = (18f + 2f + 2f) * 1000f,
                    Chance = 1f,
                    Breakable = false,
                });
                // Situational Changes
                this[i].InBackPerc_Melee = 1.00f;
            }
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
             
        /*}
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
            Version = Versions.V_10N;
            Health = 6972500;
            BerserkTimer = 10 * 60;
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
             
        /*}
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
            Version = Versions.V_25N;
            Health = 23706500f;
            BerserkTimer = 10 * 60;
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
            BerserkTimer = 10 * 60;
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
            BerserkTimer = 10 * 60;
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