using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr.Bosses
{
    #region Icecrown Citadel
    // ===== Icecrown Citadel =========================
    #region The Lower Spire
    public class LordMarrowgar : MultiDiffBoss
    {
        public LordMarrowgar()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Lord Marrowgar";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 6972500, 23706500, 10500000, 31400000 };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 3, 2, 3 };
            Min_Healers = new int[] { 2, 5, 3, 6 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                // Melee
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                // Saber Lash - Inflicts 200% weapon damage split evenly between the target and its 2 nearest allies.
                this[i].Attacks.Add(new Attack
                {
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
                this[i].Attacks.Add(new DoT
                {
                    Name = "Coldflame",
                    DamageType = ItemDamageType.Frost,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    NumTicks = 3,
                    DamagePerTick = new int[] { 6000, 8000, 9000, 11000 }[i] / 3,
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 6.0f,
                });
                // Bone Spike Graveyard
                //   Hurls a spike at a random player, impaling all players between the boss and the target on Bone Spikes,
                //   inflicting 9,000 direct Physical damage, and additional 10% health as damage every 1 second until the
                //   spike is destroyed.
                // The player that is spiked is considered unbreakably stunned
                int duration = 3; // Assuming you will be broken out within 3 sec
                this[i].Attacks.Add(new DoT
                {
                    Name = "Bone Spike Graveyard",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamagePerHit = 9000f,
                    DamagePerTick = (new float[] { 0.10f, 0.10f, 0.10f, 0.10f }[i] * 0.10f * duration),
                    DamageIsPerc = true,
                    MaxNumTargets = new int[] { 1, 3, 1, 3 }[i],
                    AttackSpeed = 6.0f,
                    IgnoresMTank = true,
                    IgnoresOTank = true,
                    IgnoresTTank = true,
                });
                this[i].Stuns.Add(new Impedance()
                {
                    Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                    Duration = duration * 1000,
                    Chance = 1f / (this[i].Max_Players - this[i].Min_Tanks),
                    Breakable = false,
                });
                // Bone Storm
                //   Inflicts 6,000 Physical damage every 2 seconds to players caught in the Bone Storm.
                //   The entire storm lasts ~20 seconds.
                this[i].Attacks.Add(new Attack
                {
                    Name = "Bone Storm",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = new int[] { 6000, 12000, 12000, 14000 }[i] * new int[] { 20, 30, 20, 30 }[i] / 2 * 0.25f, // 6k per tick for 2 sec. 25% mod as people will be running away to take less damage
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 45f + 22f,
                });
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                    Duration = (18f + 2f + 2f) * 1000f,
                    Chance = 1f,
                    Breakable = false,
                });
                // Situational Changes
                this[i].InBackPerc_Melee = 1.00f;
            }
            #endregion
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0.00f, 0.00f, 0, 0 };
            Resist_Frost = new double[] { 0.00f, 0.00f, 0, 0 };
            Resist_Fire = new double[] { 0.00f, 0.00f, 0, 0 };
            Resist_Nature = new double[] { 0.00f, 0.00f, 0, 0 };
            Resist_Arcane = new double[] { 0.00f, 0.00f, 0, 0 };
            Resist_Shadow = new double[] { 0.00f, 0.00f, 0, 0 };
            Resist_Holy = new double[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    public class LadyDeathwhisper : MultiDiffBoss
    {
        public LadyDeathwhisper()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Lady Deathwhisper";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 3346800f + 13992000f, 3346800f + 13992000f, 3346800f + 13992000f, 3346800f + 13992000f };// Mana Barrier, have to destroy mana before dps'g boss
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 3, 2, 3 };
            Min_Healers = new int[] { 2, 5, 3, 6 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 2, 2, 2, 2 };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                // Adds come 3 at a time, either 2 Adherents 1 Fanatic or 1 Adherent 2 Fanatics
                // They will also randomly respawn as Reanimated or Mutated (? forgot word used)
                this[i].MultiTargsPerc = 0.00f;
                this[i].MultiTargsPerc -= 0.30f; // Phase 2 has no adds, assuming 30% of fight in Phase 2
                // Heroic version spawns adds (1 add on 10 man, 3 adds on 25 man); 50-60% of fight in Phase 2
                float uptime = (this[i].BerserkTimer / 60f) * 35f; // Every 60 seconds and up for 35 sec before back on boss
                this[i].MultiTargsPerc -= (1f - uptime) * 0.70f; // Phase 1 has adds, marking the downtime instead of uptime

                // Melee attacks only in Phase 2
                this[i].Attacks.Add(new Attack
                {
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // hits for about 11k in Normal 25 
                    DamagePerHit = ( ( BossHandler.StandardMeleePerHit[(int)this[i].Content] / 1.3f ) / 1.3f ),
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f, 
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    Dodgable = true,
                    Missable = true,
                    Parryable = true,
                    Blockable = true,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,
                });
                
                // Shadow Bolt - Inflicts 7,438 to 9,562 Shadow damage to the current target.
                this[i].Attacks.Add(new Attack()
                {
                    Name = "Shadow Bolt",
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = new int[] { (7438 + 9562), (9188 + 11812), (9188 + 11812), (11375 + 14625) }[i] / 2f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f + 2.0f,
                });
                // Frostbolt - Inflicts 40,950/44,850/50,700/58,500 to 43,050/47,150/53,330/61,500 Frost damage
                //           to the current target. Can be interrupted. 4 second Cast
                //      Only cast during Phase 2
                this[i].Attacks.Add(new Attack()
                {
                    Name = "Frostbolt",
                    DamageType = ItemDamageType.Frost,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = new int[] { (40950 + 43050), (50700 + 53330), (44850 + 47150), (58500 + 61500) }[i] / 2f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 4.0f,
                    Interruptable = true,
                    
                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,
                });
                // Frostbolt Volley - Inflicts 8550/10800/14400 to 10450/13200/17600 Frost damage to nearby enemies, reducing their movement speed by 30% for 4 sec.
                //      Only cast during Phase 2
                this[i].Attacks.Add(new Attack()
                {
                    Name = "Frostbolt Volley",
                    DamageType = ItemDamageType.Frost,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = new int[] { (8550 + 10450), (10800 + 13200), (10800 + 13200), (14400 + 17600) }[i] / 2f,
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 20f,
                });
                // Death and Decay - Inflicts 4,500 Shadow damage every 1 second to enemies within the area of the spell. 
                //          The entire spell lasts 10 seconds.
                this[i].Attacks.Add(new DoT()
                {
                    Name = "Death and Decay",
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    NumTicks = 10f,
                    TickInterval = 1f,
                    DamagePerTick = new int[] { 4500, 4500, 6000, 6000 }[i] * 0.25f, // 4.5k/6k per tick for 1 sec. 25% mod as people will be running away to take less damage
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 45f,
                });

                // TODO: Dark Empowerment - Empowers a random Adherent or Fanatic, causing them to deal area damage with spells. Also makes the target immune to interrupts.
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            // Dominate Mind - 1 target in 10 man heroic, and 25 normal; 3 targets in 25 heroic. Lasts 12 seconds; 40 second Cooldown
            for (int i = 0; i < 4; i++)
            {
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 40f,
                    Duration = 12f,
                    Chance = new int[] { 0, (1 / (this[i].Max_Players - this[i].Min_Tanks)), (1 / (this[i].Max_Players - this[i].Min_Tanks)), (3 / (this[i].Max_Players - this[i].Min_Tanks)) }[i],
                    Breakable = false,
                });
            }
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    // - Gunship Event
    public class DeathbringerSaurfang : MultiDiffBoss
    {
        public DeathbringerSaurfang()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Deathbringer Saurfang";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 8785000f, 31860000f, 12300000f, 43930000f };
            BerserkTimer = new int[] { 8 * 60, 8 * 60, 8 * 60, 8 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 3, 6 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
                // Melee
                this[i].Attacks.Add(new Attack
                {
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 1f,
                    AttackType = ATTACK_TYPES.AT_MELEE,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
                // TO DO: Blood Power - Increases damage and size by 1%; Stacks up to 100%
                // TO DO: Frenzy  - Increases Saurfang's attack speed by 30% and size by 20%.
                //       Used when Saurfang's health reaches 30%.
                // Boiling Blood - Inflicts 5,000 Physical damage every 3 seconds for 24 seconds. 
                //       Used on a random target. (Same amount in all three versions)
                this[i].Attacks.Add(new DoT() {
                   Name = "Boiling Blood",
                   DamageType = ItemDamageType.Physical,
                   AttackType = ATTACK_TYPES.AT_RANGED,
                   TickInterval = 3f,
                   NumTicks = 8f,
                   DamagePerTick = 5000f,
                   MaxNumTargets = new int[] { 1, 3, 1, 3 }[i],
                   AttackSpeed = 15.5f,
                });
                // Blood Nova - Inflicts 7,600/9,500 to 8,400/10,500 Physical damage 
                //       to a random target and all players within 12 yards.
                this[i].Attacks.Add(new Attack {
                    Name = "Blood Nova",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamagePerHit =  new int[] { (7600 + 8400), (9500 + 10500), (7600 + 8400), (9500 + 10500) }[i] / 2f, // 1 person only assuming everyone is more than 12 yards apart
                    MaxNumTargets = 1f,
                    AttackSpeed = 20f,
                });
                // TO DO: Rune of Blood - Marks a target with Rune of Blood, causing Saurfang's melee attacks
                //       against that target to leech 5,100/5,950 to 6,900/8,050 health from them, and heal the 
                //       Deathbringer for 10 times that amount. Lasts 20 seconds.
            }
           #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            #region BossAdds
            /*
             * Name = Blood Beast (Summoned 2/5 mobs every 30 seconds)
             * Health = new float[] { 100000f, 120000f, 90997f, 140134f };
             * Resistant Skin - The skin of this creature is highly resistant. Damage from area of effect attacks is 
             *          reduced by 95% and damage from Diseases is reduced by 70%.
             * Scent of Blood - Saurfang buffs the Blood Beasts, causing them to reduce the movement speed of all nearby players by 80%,
             *          while increasing the Beasts' damage by 300%. Lasts 10 seconds. (Heroic 10 and 25 only)
             */
            #endregion
            /* TODO:
             */
        }
    }
    #endregion

    #region The Plagueworks
    public class Festergut : MultiDiffBoss
    {
        public Festergut()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Festergut";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 9412000f, 40440000f, 13700000f, 52200000f };
            BerserkTimer = new int[] { 5 * 60, 5 * 60, 5 * 60, 5 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 3, 6 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            // Inhale rotation consists of 4 x 30 second Gaseous Blight rotations, 3 x 3.5 second Inhale cast times, and 1 x 3 second Pungent Blight cast time
            float inhalerotationlength = ( 30f * 4f ) + ( 3.5f * 3f ) + 3f;
            for (int i = 0; i < 4; i++) {
                // Melee
                // 0 Inhales
                this[i].Attacks.Add( new Attack
                {
                    Name = "Melee with 0 Inhales",
                    DamageType = ItemDamageType.Physical,
                    // hits for about 11k in 25 man
                    DamagePerHit =  ( ( BossHandler.StandardMeleePerHit[ (int)this[i].Content ] / 1.3f ) / 1.3f ) ,
                    MaxNumTargets = 1f,
                    AttackSpeed = ( inhalerotationlength / ( 30f / ( 2f / 1.1f ) ) ), // hits every 1.8 seconds for 30 seconds
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    Dodgable = true,
                    Missable = true,
                    Parryable = true,
                    Blockable = true,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,                   
                });
                // 1 Inhale
                this[i].Attacks.Add(new Attack
                {
                    Name = "Melee with 1 Inhales",
                    DamageType = ItemDamageType.Physical,
                    // hits for about 14k in 25 man
                    DamagePerHit = ( BossHandler.StandardMeleePerHit[ (int)this[i].Content ] / 1.3f ),
                    MaxNumTargets = 1f,
                    AttackSpeed = ( inhalerotationlength / ( 30f / ( 2f / 1.4f ) ), // hits every 1.45 seconds for 30 seconds
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    Dodgable = true,
                    Missable = true,
                    Parryable = true,
                    Blockable = true,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,
                });
                // 2 inhales
                this[i].Attacks.Add(new Attack
                {
                    Name = "Melee with 2 Inhales",
                    DamageType = ItemDamageType.Physical,
                    // hits for about 19k in 25 man
                    DamagePerHit = BossHandler.StandardMeleePerHit[ (int)this[i].Content  ],
                    MaxNumTargets = 1f,
                    AttackSpeed = ( inhalerotationlength / ( 30f / ( 2f / 1.7f ) ),  // hits 1.17 seconds for 30 seconds
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    Dodgable = true,
                    Missable = true,
                    Parryable = true,
                    Blockable = true,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,
                });
                // 3 inhales
                this[i].Attacks.Add(new Attack
                {
                    Name = "Melee with 3 Inhales",
                    DamageType = ItemDamageType.Physical,
                    // hits for about 25k in 25 man
                    DamagePerHit = BossHandler.StandardMeleePerHit[ (int)this[i].Content ] * 1.3f,
                    MaxNumTargets = 1f,
                    AttackSpeed = ( inhalerotationlength / 30f ),  // hits every second for 30 seconds
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    Dodgable = true,
                    Missable = true,
                    Parryable = true,
                    Blockable = true,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,
                });
                // Gaseous Blight—The Gaseous Plague inflicts 4,388 to 4,612 Shadow damage to all nearby players.
                //              (lasts 30 seconds + 1st inhale cast time [3.5 seconds])
                this[i].Attacks.Add(new DoT()
                {
                    Name = "Gaseous Blight with 0 inhales",
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    TickInterval = 1f, 
                    NumTicks = 33.5f, //lasts 30 seconds + 1st inhale cast time [3.5 seconds]
                    DamagePerTick = new int[] { (2925 + 3075), (4388 + 4612), (4388 + 4612), (6338 + 6662) }[i] / 2f ,
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = inhalerotationlength,
                });
                // Gaseous Blight—(After 1 inhale) The Gaseous Plague inflicts 2,925 to 3,075 Shadow damage to all nearby players.
                //             (lasts 30 seconds + 2st inhale cast time [3.5 seconds]) (lasts 30 seconds + 2st inhale cast time [3.5 seconds])
                this[i].Attacks.Add(new DoT()
                {
                    Name = "Gaseous Blight with 1 inhales",
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    TickInterval = 1f,
                    NumTicks = 33.5f, //lasts 30 seconds + 2st inhale cast time [3.5 seconds]
                    DamagePerTick = (new int[] { (1950 + 2050), (2925 + 3075), (2925 + 3075), (4388 + 4612) }[i] / 2f) * .25f, // assume 25% shadow reduction
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = inhalerotationlength,
                });
                // Gaseous Blight—(After 2 inhale ) The Gaseous Plague inflicts 1,463 to 1,537 Shadow damage to all nearby players. 
                //             (lasts 30 seconds + Pungent Blight cast time [3 seconds]) (assume 50% shadow reduction)
                this[i].Attacks.Add(new DoT()
                {
                    Name = "Gaseous Blight with 2 inhales",
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    TickInterval = 1f,
                    NumTicks = 33f, //lasts 30 seconds + Pungent Blight cast time [3 seconds]
                    DamagePerTick = (new int[] { (1463 + 1537), (1950 + 2050), (1950 + 2050), (2925 + 3075) }[i] / 2f) * .5f, // assume 50% shadow reduction
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = inhalerotationlength,
                });
                // Pungent Blight—Violently releases the Gaseous Blight, dealing 48,750 to 51,250 Shadow damage to all enemy players, 
                //                 releasing the deadly Blight back into the room. ( inhalerotationlength cd ) (assume 75% shadow reduction)
                this[i].Attacks.Add(new Attack
                {
                    Name = "Pungent Blight",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = (new int[] { (42900 + 45100), (63375 + 66625), (48750 + 51250), (73125 + 76875) }[i] / 2f) * .75f, // assume 75% shadow reduction
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = inhalerotationlength,
                });
                // Vile Gas—Inflicts a Vile plague in targeted area, inflicting 4,875 to 5,125 damage every 2 seconds for 6 seconds. The plague causes the infected targets to vomit uncontrollably, inflicting 3,900 to 4,100 damage to nearby allies. Does not affect melee if there are at least 3 ranged players (2 on 10-man; 3 on 25-man).
                this[i].Attacks.Add(new DoT()
                {
                    Name = "Vile Gas",
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    TickInterval = 2f,
                    NumTicks = 3f, //damage every 2 seconds for 6 seconds
                    DamagePerTick = new int[] { (3900 + 4100), (4875 + 5125), (4875 + 5125), (6338 + 6662) }[i] / 2f, // assume everyone is spread out 8 yards and doesn't get hit with aoe from attack
                    MaxNumTargets = new float[] { 2f, 3f, 2f, 3f }[i],
                    AttackSpeed = 20f,
                });
                this[i].Stuns.Add(new Impedance()
                {
                    Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                    Duration = 6f,
                    Chance = 1f / (this[i].Max_Players - this[i].Min_Tanks),
                    Breakable = false,
                });
                // Gas Spore - inflicting 1,950 to 2,050 Shadow damage every 1 second for 6 seconds to any players within 8 yards range. 
                //                      Players who survive the DoT will become Inoculated, which decreases Shadow damage taken by 25%, 
                //                      stacking up to 3 times. Pungent Blight resets the stack of Inoculated.
                this[i].Attacks.Add(new DoT
                {
                    Name = "Gas Spore",
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    TickInterval = 1f,
                    NumTicks = 6f, //damage every 1 second for 6 seconds
                    DamagePerTick = new int[] { (1950 + 2050), (1950 + 2050), (1950 + 2050), (1950 + 2050) }[i] / 2f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 40f, // 40 second cooldown, first spore after 12 seconds
                });
                // Some people will have to move, Tanks don't and some will not (aka melee).
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                    Duration = 3f,
                    Chance = 1f / (this[i].Max_Players - this[i].Min_Tanks),
                    Breakable = true,
                });
                // Gastric Bloat—Inflicts 9,750 to 10,250 Nature damage to the target. Damage done by affected increased by 
                //               10% per application. Causes Gastric Explosion, immediately killing the player, if it reaches 10 stacks.
                this[i].Attacks.Add(new Attack
                {
                    Name = "Gastric Bloat",
                    DamageType = ItemDamageType.Nature,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = new int[] { (9750 + 10250), (12188 + 12812), (12188 + 12812), (14625 + 15375) }[i] / 2f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 11f,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,
                });
                // Gastric Explosion—Immediately kills the affected player, and does 29,250 to 30,750 shadow damage to all players
                //                    in a 10 yard range. 
                this[i].Attacks.Add(new Attack
                {
                    Name = "Gastric Explosion on Tank",
                    DamageType = ItemDamageType.Nature,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = 1f,
                    DamageIsPerc = true,
                    MaxNumTargets = 1f,
                    AttackSpeed = 100f,
                    Interruptable = true,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,
                });
                // Assuming the explosion does go off, it would most likely explode near melee
                this[i].Attacks.Add(new Attack
                {
                    Name = "Gastric Explosion on Melee",
                    DamageType = ItemDamageType.Nature,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = new int[] { (29250 + 30750), (29250 + 30750), (48750 + 51250), (48750 + 51250) }[i] / 2f,
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 100f,
                    Interruptable = true,
                });
                // Malleable Goo - (Heroic Only) Inflicts 14625/19500 to 15375/20500 damage in a 5 yard radius and slows casting
                //                              and attack speed by 250% for 20 sec.
                this[i].Attacks.Add(new Attack
                {
                    Name = "Malleable Goo",
                    DamageType = ItemDamageType.Nature,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = new int[] { 0, (14625 + 15375), 0, (19500 + 20500) }[i] / 2f,
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 10f,
                    Interruptable = true,
                });
                // run away if Malleable Goo targets the spot/melee that your in; estimate 2 seconds to move away 5 yards
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                    Duration = 2f,
                    Chance = 1f / this[i].Max_Players,
                    Breakable = false,
                });
            }
           #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    public class Rotface : MultiDiffBoss
    {
        public Rotface()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Rotface";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 7320000f, 36257000f, 10458000f, 47413000f };
            // Soft enrage is about 7 minutes in, Hard enrage is after 10 minutes (as shown in the DK soloing video)
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 3, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
             this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    public class ProfessorPutricide : MultiDiffBoss
    {
        public ProfessorPutricide()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Professor Putricide";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 9761500f, 42000000f, 13670000f, 50200000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 3, 2, 3 };
            Min_Healers = new int[] { 2, 5, 3, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
             this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    #endregion

    #region The Crimson Hall
    public class BloodPrinceCouncil : MultiDiffBoss
    {
        public BloodPrinceCouncil()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Blood Prince Council";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Health is shared between the three bosses
            Health = new float[] { 5620000f, 22500000f, 7624000f, 30497000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 3, 2, 3 };
            Min_Healers = new int[] { 2, 5, 3, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
             this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    public class BloodQueenLanathel : MultiDiffBoss
    {
        public BloodQueenLanathel()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Blood-Queen Lana'thel";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 14200000f, 5940000f, 18900000f, 71300000f };
            // 5 minute, 20 second fight
            BerserkTimer = new int[] { 320, 320, 320, 320 };
            Min_Tanks = new int[] { 2, 3, 2, 3 };
            Min_Healers = new int[] { 2, 5, 3, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
             this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    #endregion

    #region The Frostwing Hall
    public class ValathriaDreamwalker : MultiDiffBoss
    {
        public ValathriaDreamwalker()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Valathria Dreamwalker";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Health is half her total health, which you HEAL, not dps
            Health = new float[] { 12000000f / 2f, 35999000f / 2f, 12000000f / 2f, 35999000f / 2f };
            BerserkTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 4 }; // 1-2 Tertiary Tanks to kite the Blistering Zombie on Heroic 25
            Min_Healers = new int[] { 2, 5, 3, 6 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
                    // Valathria does not actually fight you, you fight a crap load of adds while healers heal Valathria back to full health
                    // Heroic, Valathria does take damage
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    public class Sindragosa : MultiDiffBoss
    {
        public Sindragosa()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Sindragosa";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 11156000f, 38348000f, 13950000f, 45950000f };
            // Soft enrage is about 7 minutes in, Hard enrage is after 10 minutes (as shown in the DK soloing video)
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 3, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
            this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    #endregion

    #region The Frozen Throne
    public class TheLichKing : MultiDiffBoss
    {
        public TheLichKing()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "The Lich King";
            Instance = "Icecrown Citadel";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // You have 15 minutes to bring him down to 10%. After which point it's a free kill with no berserker time.
            Health = new float[] { 17400000f * .9f, 61300000f * .9f, 29500000f * .9f, 103200000f * .9f };
            BerserkTimer = new int[] { 15 * 60, 15 * 60, 15 * 60, 15 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 3, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
            this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    #endregion
    #endregion

    #region Ruby Sanctum
    // =====Ruby Sanctum =========================
    public class Baltharus : MultiDiffBoss
    {
        public Baltharus()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Baltharus";
            Instance = "Ruby Sanctum";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Normal and Heroic setting provide no difference to his health or abilities
            Health = new float[] { 3486000f, 11156000f, 3486000f, 11156000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 2, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
            this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    
                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    public class SavianaRagefire : MultiDiffBoss
    {
        public SavianaRagefire()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Saviana Ragefire";
            Instance = "Ruby Sanctum";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Normal and Heroic setting provide no difference to her health or abilities
            Health = new float[] { 4183000f, 13945000f, 4183000f, 13945000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 2, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
            this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    
                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    public class GeneralZarithrian : MultiDiffBoss
    {
        public GeneralZarithrian()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "General Zarithrian";
            Instance = "Ruby Sanctum";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Normal and Heroic setting provide no difference to his health or abilities
            Health = new float[] { 11156000f, 43500000f, 15339900f, 58600000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 2, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
            this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    
                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    public class Halion : MultiDiffBoss
    {
        public Halion()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Halion";
            Instance = "Ruby Sanctum";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T10_0, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_5, BossHandler.TierLevels.T10_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Fight is split into 3 phases, first two everyone is DPSing him together, Phase 3 DPS is split up between live and shadow realm
            Health = new float[] { 4141000f, 14098000f, 4141000f, 14098000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 2, 5, 2, 5 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
            this[i].Attacks.Add(new Attack
            {
                    // Melee Attacks
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    // Hits for about 18k on normal 25
                    DamagePerHit = BossHandler.StandardMeleePerHit[ ( (int)this[i].Content ],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    
                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
                });
            }
            #endregion
            #endregion
            #region Impedances
            //Moves;
            //Stuns;
            //Fears;
            //Roots;
            //Disarms;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f };
            #endregion
            /* TODO:
             */
        }
    }
    #endregion
}