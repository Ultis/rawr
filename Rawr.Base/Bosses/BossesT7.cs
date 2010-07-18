using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr.Bosses {
    // ===== Naxxramas ================================
    // Spider Wing
    public class AnubRekhan : MultiDiffBoss
    {
        public AnubRekhan() {
            // If not listed here use values from defaults
            #region Info
            Name = "Anub'Rekhan";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2231200, 6763325f, 0, 0 };
            Max_Players = new int[] { 10, 25,  0,  0 };
            Min_Tanks   = new int[] {  2,  3,  0,  0 };
            Min_Healers = new int[] {  2,  4,  0,  0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                {
                    /* = Impale =
                     * Anub'Rekhan will target a random player and send a line of spikes out towards the
                     * player, hitting everyone in a straight line between him and his target. Players
                     * hit by these spikes will take 4,813 to 6,187 (Heroic: 5,688 to 7,312) physical
                     * damage and will be knocked into the air, suffering reduced fall damage when they land.
                     */
                    Attack a = new Attack {
                        Name = "Impale",
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = new float[] { (4813f + 6187f), (5688f + 7312f) }[i] / 2f,
                        MaxNumTargets = this[i].Max_Players,
                        AttackSpeed = 40.0f,
                        AttackType = ATTACK_TYPES.AT_AOE,
                        IgnoresMTank = true,
                    };
                    this[i].Attacks.Add(a);
                    // When he Impales, he turns around and faces the raid
                    // simming this by using the activates over fight and having him facing raid for 2 seconds
                    float time = (this[i].BerserkTimer / a.AttackSpeed) * 2f;
                    this[i].InBackPerc_Melee -= time / this[i].BerserkTimer;
                    this[i].InBackPerc_Ranged -= time / this[i].BerserkTimer;
                }
            }
            #endregion
            #endregion
            #region Impedances
            for (int i = 0; i < 2; i++)
            {
                Impedance M;
                /* = Locust Swarm =
                    * Every 80-120 seconds for 16 seconds you can't be on the target
                    * Note: Adding 4 seconds to the Duration for moving out before it
                    * starts and then back in after
                */
                this[i].Moves.Add(M = new Impedance()
                {
                    Frequency = (80f + 120f) / 2f,
                    Duration = (16f + 4f) * 1000f,
                    Chance = 1.00f,
                    Breakable = false, // Because he's being Kited and if you stay near him you die
                });
                // Every time he Locust Swarms he summons a Crypt Guard
                // Let's assume it's up for 10 seconds
                this[i].Targets.Add(new TargetGroup()
                {
                    Frequency = M.Frequency,
                    Duration = 10 * 1000f,
                    Chance = 1.00f,
                    NearBoss = false,
                    NumTargs = new float[] { 1 , 2 }[i],
                });
                // Every time he spawns a Crypt Guard and it dies, x seconds
                // after he summons 10 scarabs from it's body
                // Assuming they are up for 8 sec
                this[i].Targets.Add(new TargetGroup()
                {
                    Frequency = M.Frequency,
                    Duration = 8 * 1000f,
                    Chance = 1.00f,
                    NearBoss = false,
                    NumTargs = new float[] { 2, 4 }[i],
                });
            }
            #endregion
            /* TODO:
             * Damage from the Adds, minor but should still be there
             */
        }
    }
    public class GrandWidowFaerlina : MultiDiffBoss
    {
        public GrandWidowFaerlina() {
            // If not listed here use values from defaults
            #region Info
            Name = "Grand Widow Faerlina";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2231200f, 6763325f, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 1.00f, 1.00f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 3, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++) {
                this[i].Targets.Add(new TargetGroup { // Worshippers
                    Frequency = this[i].BerserkTimer - 1, // Once
                    Chance = 1.00f,
                    NearBoss = false,
                    Duration = new float[] { 4, 6 }[i] * 5 * 1000, // 4(+2) adds, 5 seconds each, used to derage her
                    NumTargs = 1
                });

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new DoT
                {
                    Name = "Poison Bolt Volley",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = new float[] { (2625f + 3375f) / 2.0f, (3755f + 4125f) / 2.0f }[i],
                    DamagePerTick = new float[] { BossHandler.CalcADotTick(1480f, 1720f, 8f, 2f), BossHandler.CalcADotTick(1900f, 2100f, 8f, 2f) }[i],
                    TickInterval = 2f,
                    NumTicks = 4,
                    MaxNumTargets = 3,
                    AttackSpeed = (7.0f + 15.0f) / 2.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
                {
                    Attack a = new Attack
                    {
                        Name = "Rain of Fire",
                        DamageType = ItemDamageType.Fire,
                        DamagePerHit = new float[] { BossHandler.CalcADotTick(1750f, 2750f, 6f, 2f), BossHandler.CalcADotTick(3700f, 4300f, 6f, 2f) }[i],
                        MaxNumTargets = this[i].Max_Players,
                        AttackSpeed = (6.0f + 18.0f) / 2.0f,
                        AttackType = ATTACK_TYPES.AT_AOE,
                    };
                    this[i].Attacks.Add(a);
                    // For each Rain of Fire she has to be moved (assuming 3 seconds to move)
                    this[i].Moves.Add(new Impedance()
                    {
                        Frequency = a.AttackSpeed,
                        Duration = 3f * 1000f,
                        Chance = 1f,
                        Breakable = false
                    });
                }
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Frenzy
             */
        }
    }
    public class Maexxna : MultiDiffBoss
    {
        public Maexxna() {
            // If not listed here use values from defaults
            #region Info
            Name = "Maexxna";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2510000f, 7600000f, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 1.00f, 1.00f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 1, 1, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                // 8 Adds every 40 seconds for 8 seconds (only 7300 HP each)
                this[i].Targets.Add(new TargetGroup
                {
                    Frequency = 40,
                    Chance = 1.00f,
                    Duration = 8 * 1000,
                    NumTargs = 8,
                    NearBoss = true,
                });

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                {
                    /* = Web Wrap =
                     * Cast 20 seconds after engaging, and every 40 seconds after that.
                     * Sends 1 (Heroic: 2) player straight to the western web wall, encasing
                     * them in a Web Wrap cocoon and incapacitating them. When encased, the
                     * player takes 2,475 to 3,025 Nature damage every 2 seconds. The cocoon
                     * can be destroyed from the outside, freeing the player and causing them
                     * to take minimal falling damage when they land.
                     */
                    Attack a = new Attack
                    {
                        Name = "Web Wrap",
                        DamageType = ItemDamageType.Nature,
                        DamagePerHit = new float[] { (2925f + 3575f) / 2f, (2925f + 3575f) / 2f }[i],
                        MaxNumTargets = new int[] { 1, 2 }[i],
                        AttackSpeed = 40.0f,
                        AttackType = ATTACK_TYPES.AT_RANGED,
                    };
                    this[i].Attacks.Add(a);
                    float initial = 20f;
                    float freq = a.AttackSpeed;
                    float chance = 1f + a.MaxNumTargets / (this[i].Max_Players - this[i].Min_Tanks);
                    this[i].Stuns.Add(new Impedance()
                    {
                        Frequency = freq * (this[i].BerserkTimer / (this[i].BerserkTimer - initial)) * chance,
                        Duration = 5f * 1000f,
                        Chance = 1f / (this[i].Max_Players - this[i].Min_Tanks),
                        Breakable = false
                    });
                }
                {
                    /* = Web Spray =
                     * Cast every 40 seconds, incapacitating everyone for 6 seconds, and
                     * dealing 1,750 to 2,250 (Heroic: 5,225 to 5,775) Nature damage. This
                     * ability cannot be resisted.
                     */
                    Attack a = new Attack
                    {
                        Name = "Web Spray",
                        DamageType = ItemDamageType.Nature,
                        DamagePerHit = new float[] { (2188f + 2812f) / 2f, (5225f + 5775f) / 2f }[i],
                        MaxNumTargets = this[i].Max_Players,
                        AttackSpeed = 40.0f,
                        AttackType = ATTACK_TYPES.AT_AOE,
                    };
                    this[i].Attacks.Add(a);
                    float initial = 0f;
                    float freq = a.AttackSpeed;
                    float chance = 1f + a.MaxNumTargets / this[i].Max_Players;
                    this[i].Stuns.Add(new Impedance()
                    {
                        Frequency = freq * (this[i].BerserkTimer / (this[i].BerserkTimer - initial)),
                        Duration = 6f * 1000f,
                        Chance = 1f,
                        Breakable = false
                    });
                }
                {
                    /* = Poison Shock =
                     * Does 3500 to 4500 (Heroic: 4,550 to 5,850) Nature damage in a 15
                     * yard frontal cone.
                     */
                    Attack a = new Attack
                    {
                        Name = "Poison Shock",
                        DamageType = ItemDamageType.Nature,
                        DamagePerHit = new float[] { (3500f + 4500f) / 2f, (4550f + 5850f) / 2f }[i],
                        MaxNumTargets = 1,
                        AttackSpeed = 40.0f,
                        AttackType = ATTACK_TYPES.AT_MELEE,
                    };
                    this[i].Attacks.Add(a);
                }
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Necrotic Poison - Poison effect that reduces healing taken by 90% for 30 seconds.
             * Frenzy - At 30% health, Maexxna will Frenzy, increasing attack speed and damage done by 50% (25 Player: damage by 75%).
             */
        }
    }
    // Plague Quarter
    public class NoththePlaguebringer : MultiDiffBoss {
        public NoththePlaguebringer() {
            // If not listed here use values from defaults
            #region Info
            Name = "Noth the Plaguebringer";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2789000f, 8436725f, 0, 0 };
            BerserkTimer = new int[] { (110 + 70) * 3, (110 + 70) * 3, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 1, 2, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 3, 3, 0, 0 };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                // Every 30 seconds 2 adds will spawn with 100k HP each, simming their life-time to 20 seconds
                this[i].Targets.Add(new TargetGroup
                {
                    Frequency = 30,
                    Chance = 1.00f,
                    Duration = 20 * 1000,
                    NumTargs = 2,
                    NearBoss = false,
                });
                //this[i].MultiTargsPerc = (this[i].BerserkTimer / 30f) * (20f) / this[i].BerserkTimer;

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Phase 2
             */
        }
    }
    public class HeigantheUnclean : MultiDiffBoss {
        public HeigantheUnclean() {
            // If not listed here use values from defaults
            #region Info
            Name = "Heigan the Unclean";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 3067900f, 9273425f, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.25f, 0.25f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 1, 1, 0, 0 };
            Min_Healers = new int[] { 3, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Decrepit Fever",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = new float[] { 3000f / 3f * 21f, 4500f / 3f * 21f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 30.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // We are assuming you are using the corner trick so you don't have
                // to dance as much in 10 man
                // Every 90 seconds for 45 seconds you must do the safety dance
                // If you are good you can stop 4 times for 5 seconds each and do
                // something to the boss
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 90f + 45f,
                    Duration = (45f - 4f * 5f) * 1000f,
                    Chance = 1f,
                    Breakable = false
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             */
        }
    }
    public class Loatheb : MultiDiffBoss {
        public Loatheb() {
            // If not listed here use values from defaults
            #region Info
            Name = "Loatheb";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 6693600f, 20220250f, 0, 0 };
            BerserkTimer = new int[] { 5 * 60, 5 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 1.00f, 1.00f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 1, 1, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Deathbloom",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = new float[] { (/*DoT*/200f / 1f * 6f) + (/*Bloom*/1200f), (/*DoT*/400f / 1f * 6f) + (/*Bloom*/1500f) }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 30.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Inevitable Doom",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { 4000f / 30f * 120f, 5000f / 30f * 120f }[i],
                    MaxNumTargets = 10,
                    AttackSpeed = 120.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // Initial 10 seconds to pop first Spore then every 3rd spore
                // after that (90 seconds respawn then 10 sec moving to/back)
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 90f,
                    Duration = 10f * 1000f,
                    Chance = 1f,
                    Breakable = false,
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Necrotic Aura
             * Fungal Creep
             */
        }
    }
    // Military Quarter
    public class InstructorRazuvious : MultiDiffBoss {
        public InstructorRazuvious() {
            // If not listed here use values from defaults
            #region Info
            Name = "Instructor Razuvious";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 3349000f, 10110125, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 4, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Disrupting Shout",
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = new float[] { (4275f + 4725f) / 2f, (7125f + 7825f) / 2f }[i],
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 15.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Jagged Knife",
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = new float[] { 5000 + (10000 / 5 * 5), 5000 + (10000 / 5 * 5) }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 10.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Unbalancing Strike
             * Using the Understudies
             */
        }
    }
    public class GothiktheHarvester : MultiDiffBoss {
        public GothiktheHarvester() {
            // If not listed here use values from defaults
            #region Info
            Name = "Gothik the Harvester";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 836700f, 2510100f, 0, 0 };
            BerserkTimer = new int[] { 19*60 - (4 * 60 + 34), 19*60 - (4 * 60 + 34), 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(new Attack
                {
                    Name = "Shadowbolt",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { (2880f + 3520f) / 2f, (4500f + 5500f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 1.0f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Phase 1 (Adds)
             * Harvest Soul
             */
        }
    }
    public class FourHorsemen : MultiDiffBoss {
        public FourHorsemen() {
            // If not listed here use values from defaults
            #region Info
            Name = "Four Horsemen";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 781000f * 4f, 2370650f * 4f, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.75f, 0.75f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 3, 3, 0, 0 }; // simming 3rd to show that 2 dps have to OT the back
            Min_Healers = new int[] { 3, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Korth'azz's Meteor",
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = new float[] { (13775f + 15225f) / 2f, (13775f + 15225f) / 2f }[i],
                    MaxNumTargets = 8,
                    AttackSpeed = 15.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Rivendare's Unholy Shadow",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { (2160f + 2640f) / 2f + (4800 / 2 * 4), (2160f + 2640f) / 2f + (4800 / 2 * 4) }[i],
                    MaxNumTargets = 8,
                    AttackSpeed = 15.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Blaumeux's Shadow Bolt",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { (2357f + 2643f) / 2f, (2357f + 2643f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 2.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Zeliek's Holy Bolt",
                    DamageType = ItemDamageType.Holy,
                    DamagePerHit = new float[] { (2357f + 2643f) / 2f, (2357f + 2643f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 2.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // Swap 1st 2 mobs once: 15
                // Get to the back once: 10
                // Bounce back and forth in the back: Every 30 sec for 10 sec but for only 40% of the fight
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 30f,
                    Duration = 10f * 1000f,
                    Chance = 0.40f,
                    Breakable = false
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Blaumeux's Void Zone
             */
        }
    }
    // Construct Quarter
    public class Patchwerk : MultiDiffBoss {
        public Patchwerk() {
            // If not listed here use values from defaults
            #region Info
            Name = "Patchwerk";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 4322950, 13038575, 0, 0 };
            BerserkTimer = new int[] { 6 * 60, 6 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 1.00f, 1.00f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 3, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack {
                    Name = "Hateful Strike",
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = new float[] { (19975f + 27025f) / 2f, (79000f + 81000f) / 2f }[i],
                    MaxNumTargets = 1f,
                    AttackSpeed = 1.0f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Frenzy
             */
        }
    }
    public class Grobbulus : MultiDiffBoss {
        public Grobbulus() {
            // If not listed here use values from defaults
            #region Info
            Name = "Grobbulus";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2928000f, 9552325, 0, 0 };
            BerserkTimer = new int[] { 12 * 60, 12 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // Every 8 seconds for 3 seconds Grob has to be kited to
                // avoid Poison Cloud Farts. This goes on the entire fight
                // Dropping the Dur to 1 sec for usability
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 8f,
                    Duration = 1f * 1000f,
                    Chance = 1f,
                    Breakable = false
                });
                // Every 20 seconds 1/10 chance to get hit with Mutating Injection
                // You have to run off for 10 seconds then run back for 4-5
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 20f,
                    Duration = (10f + (4f + 5f) / 2f) * 1000f,
                    Chance = 1f / (this[i].Max_Players - 1f),
                    Breakable = false
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Slime Spray
             * Occasional Poins Cloud Ticks that are unavoidable
             */
        }
    }
    public class Gluth : MultiDiffBoss {
        public Gluth() {
            // If not listed here use values from defaults
            #region Info
            Name = "Gluth";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2789000, 8436725, 0, 0 };
            BerserkTimer = new int[] { 8 * 60, 8 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 1.00f, 1.00f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 3, 0, 0 };
            Min_Healers = new int[] { 3, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Decimate
             * Enrage
             * Mortal Wound
             * Zombie Chows
             */
        }
    }
    public class Thaddius : MultiDiffBoss {
        public Thaddius() {
            // If not listed here use values from defaults
            #region Info
            Name = "Thaddius";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 3850000f + 838300f, 3834875 + 838300f, 0, 0 };// one player only deals with one of the add's total health + thadd's health
            BerserkTimer = new int[] { 6 * 60, 6 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.50f, 0.50f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Chain Lightning",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = new float[] { (3600f + 4400f) / 2f, (6938f + 8062f) / 2f }[i],
                    MaxNumTargets = 5f,
                    AttackSpeed = 15.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // Every 30 seconds, polarity shift, 3 sec move
                // 50% chance that your polarity will change
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 30f,
                    Duration = 3f * 1000f,
                    Chance = 0.50f,
                    Breakable = false
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Better handle of Feugen and Stalagg
             */
        }
    }
    // Frostwyrm Lair
    public class Sapphiron : MultiDiffBoss {
        public Sapphiron() {
            // If not listed here use values from defaults
            #region Info
            Name = "Sapphiron";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 4250000f, 4250000f, 0, 0 };
            BerserkTimer = new int[] { 15 * 60, 15 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 1, 1, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Frost Aura",
                    DamageType = ItemDamageType.Frost,
                    DamagePerHit = new float[]{1200,1600}[i],
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 2.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Life Drain",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[]{(((4376f + 5624f) / 2f) * 3f) * 4f,(((4376f + 5624f) / 2f) * 3f) * 4f}[i],
                    MaxNumTargets = 2,
                    AttackSpeed = 24.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // Every 45(+30) seconds for 30 seconds Sapph is in the air
                // He stops this at 10% hp
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 45f + 30f,
                    Duration = 30f * 1000f,
                    Chance = 0.90f,
                    Breakable = false
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Chill (The Blizzard)
             * Ice Bolt
             */
        }
    }
    public class KelThuzad : MultiDiffBoss {
        public KelThuzad() {
            // If not listed here use values from defaults
            #region Info
            Name = "Kel'Thuzad";
            Instance = "Naxxramas";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2230000f, 2500000f, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 6 * 60, 6 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 1.00f, 1.00f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 3, 3, 0, 0 };
            Min_Healers = new int[] { 3, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Frostbolt (Single)",
                    DamageType = ItemDamageType.Frost,
                    DamagePerHit = new float[] { (10063f + 12937f) / 2f, (29250f + 30750f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Frostbolt (Volley)",
                    DamageType = ItemDamageType.Frost,
                    DamagePerHit = new float[] { (4500f + 5500f) / 2f, (7200f + 8800f) / 2f }[i],
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // Phase 2 & 3, gotta move out of Shadow Fissures periodically
                // We're assuming they pop every 30 seconds and you have to be
                // moved for 6 seconds and there's a 1/10 chance he will select
                // you over someone else
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 30f,
                    Duration = 6f * 1000f,
                    Chance = 1f / this[i].Max_Players,
                    Breakable = false
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            // Phase 1, no damage to KT
            //TimeBossIsInvuln = 3f * 60f + 48f;
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * The Mobs in Phase 1
             */
        }
    }
    // ===== The Obsidian Sanctum =====================
    public class Shadron : MultiDiffBoss {
        public Shadron() {
            // If not listed here use values from defaults
            #region Info
            Name = "Shadron";
            Instance = "The Obsidian Sanctum";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 976150f, 2231200f, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 1, 2, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Shadow Fissure",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { (6188f + 8812f) / 2f, (6188f + 8812f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Shadow Breath",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { (6938f + 8062f) / 2f, (6938f + 8062f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // Every 60 seconds for 20 seconds dps has to jump into the portal and kill the add
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 60f + 20f,
                    Duration = 20f * 1000f,
                    Chance = 1f,
                    Breakable = false
                });
                // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
                // 1/10 chance he'll pick you
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = this[i].Attacks[1].AttackSpeed,
                    Duration = (5f + 1f) * 1000f,
                    Chance = 1f / this[i].Max_Players,
                    Breakable = false
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * The Acolyte Add
             */
        }
    }
    public class Tenebron : MultiDiffBoss {
        public Tenebron() {
            // If not listed here use values from defaults
            #region Info
            Name = "Tenebron";
            Instance = "The Obsidian Sanctum";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 976150f, 2231200, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Shadow Fissure",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { (6188f + 8812f) / 2f, (9488f + 13512f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Shadow Breath",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { (6938f + 8062f) / 2f, (8788f + 10212f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                {
                    // Every 30 seconds for 20 seconds DPS has to jump onto the 6 adds that spawn
                    this[i].Targets.Add(new TargetGroup()
                    {
                        Frequency = 40,
                        Chance = 1.00f,
                        Duration = 16 * 1000,
                        NumTargs = 6,
                        NearBoss = false,
                    });
                    this[i].Moves.Add(new Impedance()
                    {
                        Frequency = 30f + 20f,
                        Duration = (2 + 16 + 2) * 1000f,
                        Chance = 1f,
                        Breakable = false
                    });
                    //this[i].MultiTargsPerc += (this[i].BerserkTimer / (30f + 20f)) * (20f) / this[i].BerserkTimer;
                    //this[i].MaxNumTargets = 6 + 1;
                }
                {
                    // Every (Shadow Fissure Cd) seconds DPS has to move out for 5 seconds then back in for 1
                    // 1/10 chance he'll pick you
                    this[i].Moves.Add(new Impedance()
                    {
                        Frequency = this[i].Attacks[1].AttackSpeed,
                        Duration = (5f + 1f) * 1000f,
                        Chance = 1f / this[i].Max_Players,
                        Breakable = false
                    });
                }
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * The Adds' abilities
             */
        }
    }
    public class Vesperon : MultiDiffBoss {
        public Vesperon() {
            // If not listed here use values from defaults
            #region Info
            Name = "Vesperon";
            Instance = "The Obsidian Sanctum";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 976150f, 2231200f, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 1, 1, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Shadow Fissure",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { (6188f + 8812f) / 2f, (9488f + 13512f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
                this[i].Attacks.Add(new Attack
                {
                    Name = "Shadow Breath",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = new float[] { (6938f + 8062f) / 2f, (8788f + 10212f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
                // 1/25 chance he'll pick you
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = this[i].Attacks[1].AttackSpeed,
                    Duration = (5f + 1f) * 1000f,
                    Chance = 1f / this[i].Max_Players,
                    Breakable = false
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * The adds, which optimally you would ignore
             */
        }
    }
    public class Sartharion : MultiDiffBoss {
        public Sartharion() {
            // If not listed here use values from defaults
            #region Info
            Name = "Sartharion";
            Instance = "The Obsidian Sanctum";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2510100f, 7669750f, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks.Add(new Attack
                {
                    Name = "Fire Breath",
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = new float[] { (8750f + 11250f) / 2f, (10938f + 14062f) / 2f }[i],
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                });
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
            for (int i = 0; i < 2; i++)
            {
                //Moves;
                // Every 45 seconds for 10 seconds you gotta move for Lava Waves
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 45f,
                    Duration = 10f * 1000f,
                    Chance = 1f,
                    Breakable = false
                });
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             */
        }
    }
    // ===== The Vault of Archavon ====================
    public class ArchavonTheStoneWatcher : MultiDiffBoss {
        public ArchavonTheStoneWatcher() {
            // If not listed here use values from defaults
            #region Info
            Name = "Archavon The Stone Watcher";
            Instance = "The Vault of Archavon";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2300925f, 9970675f, 0, 0 };
            BerserkTimer = new int[] { 5 * 60, 5 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.75f, 0.75f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
            for (int i = 0; i < 2; i++)
            {
                // Every 30 seconds for 5 seconds you gotta catch up to him as he jumps around
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 30f,
                    Duration = 5f * 1000f,
                    Chance = 3f / this[i].Max_Players,
                    Breakable = false
                });
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             * Rock Shards
             * Crushing Leap
             * Stomp (this also stuns)
             * Impale (this also stuns)
             */
        }
    }
    // ===== The Eye of Eternity ======================
    public class Malygos : MultiDiffBoss {
        public Malygos() {
            // If not listed here use values from defaults
            #region Info
            Name = "Malygos";
            Instance = "The Eye of Eternity";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, BossHandler.TierLevels.T7_0, BossHandler.TierLevels.T7_5, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 2230000f, 19523000f, 0, 0 };
            BerserkTimer = new int[] { 19 * 60, 19 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 2, 4, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
            // Every 70-120 seconds for 16 seconds you can't be on the target
            // Adding 4 seconds to the Duration for moving out before starts and then back in after
            for (int i = 0; i < 2; i++) {
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = (70f + 120f) / 2f,
                    Duration = (16f + 4f) * 1000f,
                    Chance = 1f,
                    Breakable = false
                });
            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
            #endregion
            /* TODO:
             */
        }
    }
}