using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr.Bosses {
    // ===== Icecrown Citadel =========================
    // The Lower Spire
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
            Min_Tanks   = new int[] {  2,  3,  2,  3 };
            Min_Healers = new int[] {  2,  5,  3,  6 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 1, 1, 1, 1 };
            MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++) {
                // Melee
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
                this[i].Attacks.Add(new DoT {
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
                this[i].Attacks.Add(new DoT {
                    Name = "Bone Spike Graveyard",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamagePerHit = new int[] { 9000, 10000, 11000, 12000 }[i],
                    DamagePerTick = (new float[] { 0.10f, 0.10f, 0.10f, 0.10f }[i] * 0.10f * duration),
                    DamageIsPerc = true,
                    MaxNumTargets = new int[] { 1, 3, 1, 3 }[i],
                    AttackSpeed = 6.0f,
                    IgnoresMTank = true,
                    IgnoresOTank = true,
                    IgnoresTTank = true,
                });
                this[i].Stuns.Add(new Impedance() {
                    Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                    Duration = duration * 1000,
                    Chance = 1f / (this[i].Max_Players - this[i].Min_Tanks),
                    Breakable = false,
                });
                // Bone Storm
                //   Inflicts 6,000 Physical damage every 2 seconds to players caught in the Bone Storm.
                //   The entire storm lasts ~20 seconds.
                this[i].Attacks.Add(new Attack {
                    Name = "Bone Spike Graveyard",
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = new int[] { 6000, 8000, 10000, 12000 }[i] * 20 / 2 * 0.25f, // 6k per tick for 2 sec. 25% mod as people will be running away to take less damage
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 45f + 22f,
                });
                this[i].Moves.Add(new Impedance() {
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
            Min_Tanks   = new int[] {  2,  3,  2,  3 };
            Min_Healers = new int[] {  2,  5,  3,  6 };
            #endregion
            #region Offensive
            MaxNumTargets = new double[] { 2, 2, 2, 2 };
            #region Attacks
            for (int i = 0; i < 4; i++) {
                // Adds come 3 at a time, either 2 Adherents 1 Fanatic or 1 Adherent 2 Fanatics
                // They will also randomly respawn as Reanimated or Mutated (? forgot word used)
                this[i].MultiTargsPerc = 0.00f;
                this[i].MultiTargsPerc -= 0.30f; // Phase 2 has no adds, assuming 30% of fight in Phase 2
                float uptime = (this[i].BerserkTimer / 60f) * 35f; // Every 60 seconds and up for 35 sec before back on boss
                this[i].MultiTargsPerc -= (1f - uptime) * 0.70f; // Phase 1 has adds, marking the downtime instead of uptime

                // Shadow Bolt - Inflicts 7,438 to 9,562 Shadow damage to the current target.
                this[i].Attacks.Add(new Attack() {
                    Name = "Shadow Bolt",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = (7438 + 9562) / 2f,
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 2.0f,
                });
                // TODO: Death and Decay - Inflicts 4,500 Shadow damage every 1 second to enemies within the area of the spell. The entire spell lasts 10 seconds.
                // TODO: Dark Empowerment - Empowers a random Adherent or Fanatic, causing them to deal area damage with spells. Also makes the target immune to interrupts.
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
}