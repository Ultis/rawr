using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr {
    #region T7 Content
    // ===== Naxxramas ================================
    // Spider Wing
    public class AnubRekhan_10 : BossHandler {
        public AnubRekhan_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Anub'Rekhan";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2230000f;
            // ==== Fight Requirements ====
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
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
                    DamagePerHit = (4813f + 6187f) / 2f,
                    MaxNumTargets = Max_Players,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                };
                Attacks.Add(a);
                // When he Impales, he turns around and faces the raid
                // simming this by using the activates over fight and having him facing raid for 2 seconds
                float time = (BerserkTimer / a.AttackSpeed) * 2f;
                InBackPerc_Melee = 1f - time / BerserkTimer;
            }
            // ==== Situational Changes ====
            {
                /* = Locust Swarm =
                 * Every 80-120 seconds for 16 seconds you can't be on the target
                 * Note: Adding 4 seconds to the Duration for moving out before it
                 * starts and then back in after
                */
                Moves.Add(new Move() {
                    Frequency = (80f + 120f / 2f),
                    Duration = (16 + 4) * 1000,
                    Chance = 1f,
                    Breakable = false
                });
            }
            // Every time he Locust Swarms he summons a Crypt Guard
            // Let's assume it's up for 10 seconds
            float mtime = (BerserkTimer / 60f) * 10f;
            // Every time he spawns a Crypt Guard and it dies, x seconds
            // after he summons 10 scarabs from it's body
            // Assuming they are up for 8 sec
            mtime += ((BerserkTimer - 20f) / 60f) * 8f;
            MaxNumTargets = 10f;
            MultiTargsPerc = mtime / BerserkTimer;
            // TODO: Adds
        }
    }
    public class GrandWidowFaerlina_10 : BossHandler {
        public GrandWidowFaerlina_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Grand Widow Faerlina";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2231200f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  3;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Poison Bolt Volley",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (/*Initial*/(2625f + 3375f) / 2.0f) + (/*Dot*/((1480f+1720f)/2.0f)*8f/2f),
                MaxNumTargets = 3,
                AttackSpeed = (7.0f+15.0f)/2.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            {
                Attack a = new Attack {
                    Name = "Rain of Fire",
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = (/*Dot*/((1750f + 2750f) / 2.0f) * 6f / 2f),
                    MaxNumTargets = Max_Players,
                    AttackSpeed = (6.0f + 18.0f) / 2.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                };
                Attacks.Add(a);
                // For each Rain of Fire she has to be moved (assuming 3 seconds to move)
                Moves.Add(new Move() {
                    Frequency = a.AttackSpeed,
                    Duration = 3 * 1000,
                    Chance = 1f,
                    Breakable = false
                });
            }
            // Situational Changes
            /* TODO:
             * Frenzy
             * Worshippers
             */
        }
    }
    public class Maexxna_10 : BossHandler {
        public Maexxna_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Maexxna";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2510000f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks = 1;
            Min_Healers = 2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            {
                /* = Web Wrap =
                 * Cast 20 seconds after engaging, and every 40 seconds after that.
                 * Sends 1 (Heroic: 2) player straight to the western web wall, encasing
                 * them in a Web Wrap cocoon and incapacitating them. When encased, the
                 * player takes 2,475 to 3,025 Nature damage every 2 seconds. The cocoon
                 * can be destroyed from the outside, freeing the player and causing them
                 * to take minimal falling damage when they land.
                 */
                Attack a = new Attack {
                    Name = "Web Wrap",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = (2925f + 3575f) / 2f,
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                };
                Attacks.Add(a);
                float initial = 20f;
                float freq = a.AttackSpeed;
                float chance = 1f + a.MaxNumTargets / (Max_Players - Min_Tanks);
                Stuns.Add(new Stun() {
                    Frequency = freq * (BerserkTimer / (BerserkTimer - initial)) * chance,
                    Duration = 5 * 1000,
                    Breakable = false
                });
            }
            {
                /* = Web Spray =
                 * Cast every 40 seconds, incapacitating everyone for 6 seconds, and
                 * dealing 1,750 to 2,250 (Heroic: 5,225 to 5,775) Nature damage. This
                 * ability cannot be resisted.
                 */
                Attack a = new Attack {
                    Name = "Web Spray",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = (2188f + 2812f) / 2f,
                    MaxNumTargets = Max_Players,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                };
                Attacks.Add(a);
                float initial = 0f;
                float freq = a.AttackSpeed;
                float chance = 1f + a.MaxNumTargets / Max_Players;
                Stuns.Add(new Stun() {
                    Frequency = freq * (BerserkTimer / (BerserkTimer - initial)),
                    Duration = 6 * 1000,
                    Breakable = false
                });
            }
            {
                /* = Poison Shock =
                 * Does 3500 to 4500 (Heroic: 4,550 to 5,850) Nature damage in a 15
                 * yard frontal cone.
                 */
                Attack a = new Attack {
                    Name = "Poison Shock",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = (3500f + 4500f) / 2f,
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                };
                Attacks.Add(a);
            }
            // 8 Adds every 40 seconds for 8 seconds (only 7300 HP each)
            MultiTargsPerc = ((BerserkTimer / 40f) * 8f) / BerserkTimer;
            MaxNumTargets = 8;
            /* TODO:
             * Necrotic Poison
             * Frenzy
             */
        }
    }
    // Plague Quarter
    public class NoththePlaguebringer_10 : BossHandler {
        public NoththePlaguebringer_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Noth the Plaguebringer";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2500000f;
            BerserkTimer = (110 + 70) * 3; // He enrages after 3rd iteration of Phase 2
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  1;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 30 seconds 2 adds will spawn with 100k HP each, simming their life-time to 20 seconds
            MultiTargsPerc = (BerserkTimer / 30f) * (20f) / BerserkTimer;
            /* TODO:
             * Phase 2
             */
        }
    }
    public class HeigantheUnclean_10 : BossHandler {
        public HeigantheUnclean_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Heigan the Unclean";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 3067900f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  1;
            Min_Healers =  3;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Decrepit Fever",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = 3000f / 3f * 21f,
                MaxNumTargets = 1,
                AttackSpeed = 30.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            // Situational Changes
            InBackPerc_Melee = 0.25f;
            // We are assuming you are using the corner trick so you don't have
            // to dance as much in 10 man
            // Every 90 seconds for 45 seconds you must do the safety dance
            // If you are good you can stop 4 times for 5 seconds each and do
            // something to the boss
            Moves.Add(new Move() {
                Frequency = 90f+45f,
                Duration = (45f-4f*5f) * 1000,
                Chance = 1f,
                Breakable = false
            });
            /* TODO:
             */
        }
    }
    public class Loatheb_10 : BossHandler {
        public Loatheb_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Loatheb";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 6693600f;
            BerserkTimer = 5 * 60; // Inevitable Doom starts to get spammed every 15 seconds
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks = 1;
            Min_Healers = 3;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Deathbloom",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (/*DoT*/200f / 1f * 6f) + (/*Bloom*/1200f),
                MaxNumTargets = 1,
                AttackSpeed = 30.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            Attacks.Add(new Attack {
                Name = "Inevitable Doom",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = 4000 / 30 * 120,
                MaxNumTargets = 10,
                AttackSpeed = 120.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Initial 10 seconds to pop first Spore then every 3rd spore
            // after that (90 seconds respawn then 10 sec moving to/back)
            Moves.Add(new Move() {
                Frequency = 90,
                Duration = 10 * 1000,
                Chance = 1f,
                Breakable = false,
            });
            /* TODO:
             * Necrotic Aura
             * Fungal Creep
             */
        }
    }
    // Military Quarter
    public class InstructorRazuvious_10 : BossHandler {
        public InstructorRazuvious_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Instructor Razuvious";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 3349000f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Disrupting Shout",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (4275f + 4725f) / 2f,
                MaxNumTargets = Max_Players,
                AttackSpeed = 15.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            Attacks.Add(new Attack {
                Name = "Jagged Knife",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 5000 + (10000 / 5 * 5),
                MaxNumTargets = 1,
                AttackSpeed = 10.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            /* TODO:
             * Unbalancing Strike
             * Using the Understudies
             */
        }
    }
    public class GothiktheHarvester_10 : BossHandler {
        public GothiktheHarvester_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Gothik the Harvester";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 836700f;
            BerserkTimer = BerserkTimer - (4 * 60 + 34);
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Shadowbolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2880f + 3520f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 1.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            /* TODO:
             * Phase 1 (Adds)
             * Harvest Soul
             */
        }
    }
    public class FourHorsemen_10 : BossHandler {
        public FourHorsemen_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Four Horsemen";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 781000f * 4f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  3; // simming 3rd to show that 2 dps have to OT the back
            Min_Healers =  3;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Korth'azz's Meteor",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (13775f + 15225f) / 2f,
                MaxNumTargets = 8,
                AttackSpeed = 15.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            Attacks.Add(new Attack {
                Name = "Rivendare's Unholy Shadow",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2160f + 2640f) / 2f + (4800/2*4),
                MaxNumTargets = 8,
                AttackSpeed = 15.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            Attacks.Add(new Attack {
                Name = "Blaumeux's Shadow Bolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2357f + 2643f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            Attacks.Add(new Attack {
                Name = "Zeliek's Holy Bolt",
                DamageType = ItemDamageType.Holy,
                DamagePerHit = (2357f + 2643f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Swap 1st 2 mobs once: 15
            // Get to the back once: 10
            // Bounce back and forth in the back: Every 30 sec for 10 sec but for only 40% of the fight
            Moves.Add(new Move() {
                Frequency = 30f,
                Duration = 10 * 1000,
                Chance = 0.40f,
                Breakable = false
            });
            /* TODO:
             * Blaumeux's Void Zone
             */
        }
    }
    // Construct Quarter
    public class Patchwerk_10 : BossHandler {
        public Patchwerk_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Patchwerk";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            BerserkTimer = 6 * 60;
            Health = 4322950;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Hateful Strike",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (19975f + 27025f) / 2f,
                MaxNumTargets = 1f,
                AttackSpeed = 1.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            /* TODO:
             * Frenzy
             */
        }
    }
    public class Grobbulus_10 : BossHandler {
        public Grobbulus_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Grobbulus";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2928000f;
            BerserkTimer = 12 * 60;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 8 seconds for 3 seconds Grob has to be kited to
            // avoid Poison Cloud Farts. This goes on the entire fight
            // Dropping the Dur to 1 sec for usability
            Moves.Add(new Move() {
                Frequency = 8f,
                Duration = 1 * 1000,
                Chance = 1f,
                Breakable = false
            });
            // Every 20 seconds 1/10 chance to get hit with Mutating Injection
            // You have to run off for 10 seconds then run back for 4-5
            Moves.Add(new Move() {
                Frequency = 20f,
                Duration = (10f+(4f+5f)/2f) * 1000f,
                Chance = 1f / (Max_Players - 1f),
                Breakable = false
            });
            /* TODO:
             * Slime Spray
             * Occasional Poins Cloud Ticks that are unavoidable
             */
        }
    }
    public class Gluth_10 : BossHandler {
        public Gluth_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Gluth";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2789000;
            BerserkTimer = 8 * 60;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  3;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 40000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            /* TODO:
             * Decimate
             * Enrage
             * Mortal Wound
             * Zombie Chows
             */
        }
    }
    public class Thaddius_10 : BossHandler {
        public Thaddius_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Thaddius";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 3850000f + 838300f; // one player only deals with one of the add's total health + thadd's health
            BerserkTimer = 6 * 60; // Need to verify if starts at beg. of combat or beg. of Thadd
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Chain Lightning",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (3600f+4400f)/2f,
                MaxNumTargets = 5f,
                AttackSpeed = 15.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.50f;
            // Every 30 seconds, polarity shift, 3 sec move
            // 50% chance that your polarity will change
            Moves.Add(new Move() {
                Frequency = 30f,
                Duration = 3 * 1000,
                Chance = 0.50f,
                Breakable = false
            });
            /* TODO:
             * Better handle of Feugen and Stalagg
             */
        }
    }
    // Frostwyrm Lair
    public class Sapphiron_10 : BossHandler {
        public Sapphiron_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Sapphiron";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 4250000f;
            BerserkTimer = 15 * 60;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  1;
            Min_Healers =  3;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Frost Aura",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = 1200f,
                MaxNumTargets = Max_Players,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            Attacks.Add(new Attack {
                Name = "Life Drain",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (((4376f+5624f)/2f) * 3f) * 4f,
                MaxNumTargets = 2,
                AttackSpeed = 24.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 45(+30) seconds for 30 seconds Sapph is in the air
            // He stops this at 10% hp
            Moves.Add(new Move() {
                Frequency = 45f + 30f,
                Duration = 30 * 1000,
                Chance = 0.90f,
                Breakable = false
            });
            /* TODO:
             * Chill (The Blizzard)
             * Ice Bolt
             */
        }
    }
    public class KelThuzad_10 : BossHandler {
        public KelThuzad_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Kel'Thuzad";
            Content = "T7";
            Instance = "Naxxramas";
            Version = "10 Man";
            Health = 2230000f;
            BerserkTimer = 19 * 60;
            SpeedKillTimer = 6 * 60;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  3;
            Min_Healers =  3;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Frostbolt (Single)",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = (10063f + 12937f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            Attacks.Add(new Attack {
                Name = "Frostbolt (Volley)",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = (4500f + 5500f) / 2f,
                MaxNumTargets = Max_Players,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Phase 1, no damage to KT
            TimeBossIsInvuln = 3f * 60f + 48f;
            // Phase 2 & 3, gotta move out of Shadow Fissures periodically
            // We're assuming they pop every 30 seconds and you have to be
            // moved for 6 seconds and there's a 1/10 chance he will select
            // you over someone else
            Moves.Add(new Move() {
                Frequency = 30f,
                Duration = 6 * 1000,
                Chance = 1f / Max_Players,
                Breakable = false
            });
            /* TODO:
             * The Mobs in Phase 1
             */
        }
    }
    // ===== The Obsidian Sanctum =====================
    public class Shadron_10 : BossHandler {
        public Shadron_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Shadron";
            Content = "T7";
            Instance = "The Obsidian Sanctum";
            Version = "10 Man";
            Health = 976150f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  1;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 60 seconds for 20 seconds dps has to jump into the portal and kill the add
            Moves.Add(new Move() {
                Frequency = 60f + 20f,
                Duration = 20 * 1000,
                Chance = 1f,
                Breakable = false
            });
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/10 chance he'll pick you
            Moves.Add(new Move() {
                Frequency = Attacks[1].AttackSpeed,
                Duration = (5f + 1f) * 1000f,
                Chance = 1 / Max_Players,
                Breakable = false
            });
            /* TODO:
             * The Acolyte Add
             */
        }
    }
    public class Tenebron_10 : BossHandler {
        public Tenebron_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Tenebron";
            Content = "T7";
            Instance = "The Obsidian Sanctum";
            Version = "10 Man";
            Health = 976150f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            {
                // Every 30 seconds for 20 seconds dps has to jump onto the 6 adds that spawn
                Moves.Add(new Move() {
                    Frequency = 30f + 20f,
                    Duration = 20 * 1000,
                    Chance = 1f,
                    Breakable = false
                });
                MultiTargsPerc = (BerserkTimer / (30f + 20f)) * (20f) / BerserkTimer;
                MaxNumTargets = 6f + 1f;
            }
            {
                // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
                // 1/10 chance he'll pick you
                Moves.Add(new Move() {
                    Frequency = Attacks[1].AttackSpeed,
                    Duration = (5f + 1f) * 1000,
                    Chance = 1f / Max_Players,
                    Breakable = false
                });
            }
            /* TODO:
             * The Adds' abilities
             */
        }
    }
    public class Vesperon_10 : BossHandler {
        public Vesperon_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Vesperon";
            Content = "T7";
            Instance = "The Obsidian Sanctum";
            Version = "10 Man";
            Health = 976150f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  1;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/10 chance he'll pick you
            Moves.Add(new Move() {
                Frequency = Attacks[1].AttackSpeed,
                Duration = (5f + 1f) * 1000,
                Chance = 1f / Max_Players,
                Breakable = false
            });
            /* TODO:
             * The adds, which optimally you would ignore
             */
        }
    }
    public class Sartharion_10 : BossHandler {
        public Sartharion_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Sartharion";
            Content = "T7";
            Instance = "The Obsidian Sanctum";
            Version = "10 Man";
            Health = 2510100f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Fire Breath",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (8750f + 11250f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 45 seconds for 10 seconds you gotta move for Lava Waves
            Moves.Add(new Move() {
                Frequency = 45f,
                Duration = 10f * 1000f,
                Chance = 1f,
                Breakable = false
            });
            /* TODO:
             */
        }
    }
    // ===== The Vault of Archavon ====================
    public class ArchavonTheStoneWatcher_10 : BossHandler {
        public ArchavonTheStoneWatcher_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Archavon The Stone Watcher";
            Content = "T7";
            Instance = "The Vault of Archavon";
            Version = "10 Man";
            Health = 2300925f;
            BerserkTimer = 5 * 60;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Every 30 seconds for 5 seconds you gotta catch up to him as he jumps around
            Moves.Add(new Move() {
                Frequency = 30f,
                Duration = 5 * 1000,
                Chance = 1f,
                Breakable = false
            });
            /* TODO:
             * Rock Shards
             * Crushing Leap
             * Stomp (this also stuns)
             * Impale (this also stuns)
             */
        }
    }
    // ===== The Eye of Eternity ======================
    public class Malygos_10 : BossHandler {
        public Malygos_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Malygos";
            Content = "T7";
            Instance = "The Eye of Eternity";
            Version = "10 Man";
            Health = 2230000f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2; // you really only need 1 but adding 2nd for the adds phase and sparks in 1st phase
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 70-120 seconds for 16 seconds you can't be on the target
            // Adding 4 seconds to the Duration for moving out before starts and then back in after
            Moves.Add(new Move() {
                Frequency = (70f + 120f) / 2f,
                Duration = (16f+4f) * 1000f,
                Chance = 1f,
                Breakable = false
            });
            /* TODO:
             */
        }
    }
    #endregion
    #region T7.5 Content
    // ===== Naxxramas ================================
    // Spider Wing
    public class AnubRekhan_25 : BossHandler {
        public AnubRekhan_25() {
            // If not listed here use values from 10 man version
            // Basics
            Name = "Anub'Rekhan";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 6763325f;
            // ==== Fight Requirements ====
            Max_Players = 25;
            Min_Tanks   =  3;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
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
                    DamagePerHit = (5688f + 7312f) / 2f,
                    MaxNumTargets = Max_Players,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                };
                Attacks.Add(a);
                // When he Impales, he turns around and faces the raid
                // simming this by using the activates over fight and having him facing raid for 2 seconds
                float time = (BerserkTimer / a.AttackSpeed) * 2f;
                InBackPerc_Melee = 1f - time / BerserkTimer;
            }
            // ==== Situational Changes ====
            {
                /* = Locust Swarm =
                 * Every 80-120 seconds for 16 seconds you can't be on the target
                 * Note: Adding 4 seconds to the Duration for moving out before it
                 * starts and then back in after
                */
                Moves.Add(new Move() {
                    Frequency = (80f + 120f) / 2f,
                    Duration = (16 + 4) * 1000,
                    Chance = 1f,
                    Breakable = false
                });
            }
            // Every time he Locust Swarms he summons 2 Crypt Guards
            // Let's assume it's up for 10 seconds
            float mtime = (BerserkTimer / 60f) * 10f;
            // Every time he spawns a Crypt Guard and it dies, x seconds
            // after he summons 10 scarabs from each's body
            // Assuming they are up for 8 sec
            mtime += ((BerserkTimer - 20f) / 60f) * 8f;
            MaxNumTargets = 20f;
            MultiTargsPerc = mtime / BerserkTimer;
            // TODO: Adds
        }
    }
    public class GrandWidowFaerlina_25 : BossHandler {
        public GrandWidowFaerlina_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Grand Widow Faerlina";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 6763325;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Poison Bolt Volley",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (/*Initial*/(3755f + 4125f) / 2.0f) + (/*Dot*/((1900f+2100f)/2.0f)*8f/2f),
                MaxNumTargets = 3,
                AttackSpeed = (7.0f+15.0f)/2.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            {
                Attack a = new Attack {
                    Name = "Rain of Fire",
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = (/*Dot*/((3700f + 4300f) / 2.0f) * 6f / 2f),
                    MaxNumTargets = Max_Players,
                    AttackSpeed = (6.0f + 18.0f) / 2.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                };
                Attacks.Add(a);
                // For each Rain of Fire she has to be moved (assuming 3 seconds to move)
                Moves.Add(new Move() {
                    Frequency = a.AttackSpeed,
                    Duration = 3 * 1000,
                    Chance = 1f,
                    Breakable = false
                });
            }
            // Situational Changes
            /* TODO:
             * Frenzy
             * Worshippers
             */
        }
    }
    public class Maexxna_25 : BossHandler {
        public Maexxna_25() {
            // If not listed here use values from 10 man version
            // Basics
            Name = "Maexxna";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 7600000f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 1;
            Min_Healers = 4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            {
                /* = Web Wrap =
                 * Cast 20 seconds after engaging, and every 40 seconds after that.
                 * Sends 1 (Heroic: 2) player straight to the western web wall, encasing
                 * them in a Web Wrap cocoon and incapacitating them. When encased, the
                 * player takes 2,475 to 3,025 Nature damage every 2 seconds. The cocoon
                 * can be destroyed from the outside, freeing the player and causing them
                 * to take minimal falling damage when they land.
                 */
                Attack a = new Attack {
                    Name = "Web Wrap",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = (2925f + 3575f) / 2f,
                    MaxNumTargets = 2,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                };
                Attacks.Add(a);
                float initial = 20f;
                float freq = a.AttackSpeed;
                float chance = a.MaxNumTargets / (Max_Players - Min_Tanks);
                Stuns.Add(new Stun() {
                    Frequency = freq * (BerserkTimer / (BerserkTimer - initial)) / chance,
                    Duration = 5 * 1000,
                    Breakable = false
                });
            }
            {
                /* = Web Spray =
                 * Cast every 40 seconds, incapacitating everyone for 6 seconds, and
                 * dealing 1,750 to 2,250 (Heroic: 5,225 to 5,775) Nature damage. This
                 * ability cannot be resisted.
                 */
                Attack a = new Attack {
                    Name = "Web Spray",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = (5225f + 5775f) / 2f,
                    MaxNumTargets = Max_Players,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                };
                Attacks.Add(a);
                float initial = 0f;
                float freq = a.AttackSpeed;
                float chance = a.MaxNumTargets / Max_Players;
                Stuns.Add(new Stun() {
                    Frequency = freq * (BerserkTimer / (BerserkTimer - initial)) / chance,
                    Duration = 6 * 1000,
                    Breakable = false
                });
            }
            {
                /* = Poison Shock =
                 * Does 3500 to 4500 (Heroic: 4,550 to 5,850) Nature damage in a 15
                 * yard frontal cone.
                 */
                Attack a = new Attack {
                    Name = "Poison Shock",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = (4550f + 5850f) / 2f,
                    MaxNumTargets = 1,
                    AttackSpeed = 40.0f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                };
                Attacks.Add(a);
            }
            // 8 Adds every 40 seconds for 10 seconds, starting at 30 sec in (only 14000 HP each)
            MultiTargsPerc = ((BerserkTimer / 40f) * 10f) / BerserkTimer;
            MaxNumTargets = 8;
            /* TODO:
             * Necrotic Poison
             * Frenzy
             */
        }
    }
    // Plague Quarter
    public class NoththePlaguebringer_25 : BossHandler {
        public NoththePlaguebringer_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Noth the Plaguebringer";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 2500000f;
            BerserkTimer = (110 + 70) * 3; // He enrages after 3rd iteration of Phase 2
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 30 seconds 2 adds will spawn with 100k HP each, simming their life-time to 20 seconds
            MultiTargsPerc = (BerserkTimer / 30f) * (20f) / BerserkTimer;
            /* TODO:
             * Phase 2
             */
        }
    }
    public class HeigantheUnclean_25 : BossHandler {
        public HeigantheUnclean_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Heigan the Unclean";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 9273425f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  1;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Decrepit Fever",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = 3000f / 3f * 21f,
                MaxNumTargets = 1,
                AttackSpeed = 30.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            // Situational Changes
            InBackPerc_Melee = 0.25f;
            // Every 90 seconds for 45 seconds you must do the safety dance
            Moves.Add(new Move() {
                Frequency = 90f + 45f,
                Duration = (45f-4f*5f) * 1000f,
                Chance = 1f,
                Breakable = false
            });
            /* TODO:
             */
        }
    }
    public class Loatheb_25 : BossHandler {
        public Loatheb_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Loatheb";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 20220250f;
            BerserkTimer = 5 * 60; // Inevitable Doom starts to get spammed every 15 seconds
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 1;
            Min_Healers = 4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Deathbloom",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (/*DoT*/400f / 1f * 6f) + (/*Bloom*/1500f),
                MaxNumTargets = 1f,
                AttackSpeed = 30.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            Attacks.Add(new Attack {
                Name = "Inevitable Doom",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = 5000 / 30 * 120,
                MaxNumTargets = Max_Players,
                AttackSpeed = 120.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Initial 10 seconds to pop first Spore then every 3rd spore
            // after that (90 seconds respawn then 10 sec moving to/back)
            Moves.Add(new Move() {
                Frequency = 90,
                Duration = 10 * 1000,
                Chance = 1f,
                Breakable = false,
            });
            /* TODO:
             * Necrotic Aura
             * Fungal Creep
             */
        }
    }
    // Military Quarter
    public class InstructorRazuvious_25 : BossHandler {
        public InstructorRazuvious_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Instructor Razuvious";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 10110125;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  4;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 240000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            Attacks.Add(new Attack {
                Name = "Disrupting Shout",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (7125f + 7825f) / 2f,
                MaxNumTargets = Max_Players,
                AttackSpeed = 15.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            Attacks.Add(new Attack {
                Name = "Jagged Knife",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 5000 + (10000 / 5 * 5),
                MaxNumTargets = 1,
                AttackSpeed = 10.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            /* TODO:
             * Unbalancing Strike
             * Using the Understudies
             */
        }
    }
    public class GothiktheHarvester_25 : BossHandler {
        public GothiktheHarvester_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Gothik the Harvester";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 2510100f;
            //BerserkTimer = (8 * 60) - (4 * 60 + 34);
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Shadowbolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2880f + 3520f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 1.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            /* TODO:
             * Phase 1 (adds)
             * Harvest Soul
             */
        }
    }
    public class FourHorsemen_25 : BossHandler {
        public FourHorsemen_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Four Horsemen";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 2370650f * 4f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  3; // simming 3rd to show that 2 dps have to OT the back
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            Attacks.Add(new Attack {
                Name = "Korth'azz's Meteor",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (13775f + 15225f) / 2f,
                MaxNumTargets = 8,
                AttackSpeed = 15.0f,
            });
            Attacks.Add(new Attack {
                Name = "Rivendare's Unholy Shadow",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2160f + 2640f) / 2f + (4800/2*4),
                MaxNumTargets = 8,
                AttackSpeed = 15.0f,
            });
            Attacks.Add(new Attack {
                Name = "Blaumeux's Shadow Bolt",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (2357f + 2643f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
            });
            Attacks.Add(new Attack {
                Name = "Zeliek's Holy Bolt",
                DamageType = ItemDamageType.Holy,
                DamagePerHit = (2357f + 2643f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Swap 1st 2 mobs once: 15
            // Get to the back once: 10
            // Bounce back and forth in the back: Every 30 sec for 10 sec but for only 40% of the fight
            Moves.Add(new Move() {
                Frequency = 30f,
                Duration = 10 * 1000,
                Chance = 0.40f,
                Breakable = false
            });
            /* TODO:
             * Blaumeux's Void Zone
             */
        }
    }
    // Construct Quarter
    public class Patchwerk_25 : BossHandler {
        public Patchwerk_25() {
            // If not listed here use values from 10 man version
            // Basics
            Name = "Patchwerk";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 13038575;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  3;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Hateful Strike",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = (79000f + 81000f) / 2f,
                MaxNumTargets = 1f,
                AttackSpeed = 1.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            /* TODO:
             * Frenzy
             */
        }
    }
    public class Grobbulus_25 : BossHandler {
        public Grobbulus_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Grobbulus";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 9552325;
            BerserkTimer = 12 * 60;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks = 2;
            Min_Healers = 4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 8 seconds for 3 seconds Grob has to be kited to
            // avoid Poison Cloud Farts. This goes on the entire fight
            Moves.Add(new Move() {
                Frequency = 8f,
                Duration = 1 * 1000,
                Chance = 1f,
                Breakable = false
            });
            // Every 20 seconds 1/10 chance to get hit with Mutating Injection
            // You have to run off for 10 seconds then run back for 4-5
            Moves.Add(new Move() {
                Frequency = 20f,
                Duration = (10f+(4f+5f)/2f) * 1000f,
                Chance = 1f / (Max_Players - 1f),
                Breakable = false
            });
            /* TODO:
             * Slime Spray
             * Occasional Poins Cloud Ticks that are unavoidable
             */
        }
    }
    public class Gluth_25 : BossHandler {
        public Gluth_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Gluth";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 8436725;
            BerserkTimer = 8 * 60;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  3;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 80000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            /* TODO:
             * Decimate
             * Enrage
             * Mortal Wound
             * Zombie Chows
             */
        }
    }
    public class Thaddius_25 : BossHandler {
        public Thaddius_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Thaddius";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 3834875 + 30400100; // one player only deals with one of the add's total health + thadd's health
            BerserkTimer = 6 * 60; // Starts at beg. of Thadd, not begin of fight
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            Attacks.Add(new Attack {
                Name = "Chain Lightning",
                DamageType = ItemDamageType.Nature,
                DamagePerHit = (6938f+8062f)/2f,
                MaxNumTargets = 5f,
                AttackSpeed = 15.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.50f;
            // Every 30 seconds, polarity shift, 3 sec move
            // 50% chance that your polarity will change
            Moves.Add(new Move() {
                Frequency = 30f,
                Duration = 3 * 1000,
                Chance = 0.50f,
                Breakable = false
            });
            /* TODO:
             * Better handle of Feugen and Stalagg
             */
        }
    }
    // Frostwyrm Lair
    public class Sapphiron_25 : BossHandler {
        public Sapphiron_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Sapphiron";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 4250000f;
            BerserkTimer = 15 * 60;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  1;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Frost Aura",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = 1600f,
                MaxNumTargets = Max_Players,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            Attacks.Add(new Attack {
                Name = "Life Drain",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (((4376f+5624f)/2f) * 3f) * 4f,
                MaxNumTargets = 2,
                AttackSpeed = 24.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 45(+30) seconds for 30 seconds Sapph is in the air
            // He stops this at 10% hp
            Moves.Add(new Move() {
                Frequency = 45f + 30f,
                Duration = 30 * 1000,
                Chance = 0.90f,
                Breakable = false
            });
            /* TODO:
             * Chill (The Blizzard)
             * Ice Bolt
             */
        }
    }
    public class KelThuzad_25 : BossHandler {
        public KelThuzad_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Kel'Thuzad";
            Content = "T7.5";
            Instance = "Naxxramas";
            Version = "25 Man";
            Health = 2500000;
            BerserkTimer = 19 * 60;
            SpeedKillTimer = 6 * 60;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  3;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Frostbolt (Single)",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = (29250f + 30750f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_RANGED,
            });
            Attacks.Add(new Attack {
                Name = "Frostbolt (Volley)",
                DamageType = ItemDamageType.Frost,
                DamagePerHit = (7200f + 8800f) / 2f,
                MaxNumTargets = Max_Players,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Phase 1, no damage to KT
            TimeBossIsInvuln = 3f * 60f + 48f;
            // Phase 2 & 3, gotta move out of Shadow Fissures periodically
            // We're assuming they pop every 30 seconds and you have to be
            // moved for 6 seconds and there's a 1/10 chance he will select
            // you over someone else
            Moves.Add(new Move() {
                Frequency = 30f,
                Duration = 6 * 1000,
                Chance = 1f / Max_Players,
                Breakable = false
            });
            /* TODO:
             * The Mobs in Phase 1
             */
        }
    }
    // ===== The Obsidian Sanctum =====================
    public class Shadron_25 : BossHandler {
        public Shadron_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Shadron";
            Content = "T7.5";
            Instance = "The Obsidian Sanctum";
            Version = "25 Man";
            Health = 2231200f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6188f + 8812f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (6938f + 8062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 60 seconds for 20 seconds dps has to jump into the portal and kill the add
            Moves.Add(new Move() {
                Frequency = 60f + 20f,
                Duration = 20 * 1000,
                Chance = 1f,
                Breakable = false
            });
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/10 chance he'll pick you
            Moves.Add(new Move() {
                Frequency = Attacks[1].AttackSpeed,
                Duration = (5f + 1f) * 1000f,
                Chance = 1f / (Max_Players),
                Breakable = false
            });
            /* TODO:
             * The Acolyte Add
             */
        }
    }
    public class Tenebron_25 : BossHandler {
        public Tenebron_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Tenebron";
            Content = "T7.5";
            Version = "25 Man";
            Instance = "The Obsidian Sanctum";
            Health = 2231200;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (9488f + 13512f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (8788f + 10212f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            {
                // Every 30 seconds for 20 seconds dps has to jump onto the 6 adds that spawn
                Moves.Add(new Move() {
                    Frequency = 30f + 20f,
                    Duration = 20 * 1000,
                    Chance = 1f,
                    Breakable = false
                });
                MultiTargsPerc = (BerserkTimer / (30f + 20f)) * (20f) / BerserkTimer;
                MaxNumTargets = 6f + 1f;
            }
            {
                // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
                // 1/10 chance he'll pick you
                Moves.Add(new Move() {
                    Frequency = Attacks[1].AttackSpeed,
                    Duration = (5f + 1f) * 1000,
                    Chance = 1f / Max_Players,
                    Breakable = false
                });
            }
            /* TODO:
             * The Adds' abilities
             */
        }
    }
    public class Vesperon_25 : BossHandler {
        public Vesperon_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Vesperon";
            Content = "T7.5";
            Instance = "The Obsidian Sanctum";
            Version = "25 Man";
            Health = 2231200f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  1;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
                AttackType = ATTACK_TYPES.AT_MELEE,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Fissure",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (9488f + 13512f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            Attacks.Add(new Attack {
                Name = "Shadow Breath",
                DamageType = ItemDamageType.Shadow,
                DamagePerHit = (8788f + 10212f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
                AttackType = ATTACK_TYPES.AT_AOE,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every (Shadow Fissure Cd) seconds dps has to move out for 5 seconds then back in for 1
            // 1/25 chance he'll pick you
            Moves.Add(new Move() {
                Frequency = Attacks[1].AttackSpeed,
                Duration = (5f + 1f) * 1000,
                Chance = 1f / Max_Players,
                Breakable = false
            });
            /* TODO:
             * The adds, which optimally you would ignore
             */
        }
    }
    public class Sartharion_25 : BossHandler {
        public Sartharion_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Sartharion";
            Content = "T7.5";
            Instance = "The Obsidian Sanctum";
            Version = "25 Man";
            Health = 7669750f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            Attacks.Add(new Attack {
                Name = "Fire Breath",
                DamageType = ItemDamageType.Fire,
                DamagePerHit = (10938f + 14062f) / 2f,
                MaxNumTargets = 1,
                AttackSpeed = 40.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 45 seconds for 10 seconds you gotta move for Lava Waves
            Moves.Add(new Move() {
                Frequency = 45f,
                Duration = 10f * 1000f,
                Chance = 1f,
                Breakable = false
            });
            /* TODO:
             */
        }
    }
    // ===== The Vault of Archavon ====================
    public class ArchavonTheStoneWatcher_25 : BossHandler {
        public ArchavonTheStoneWatcher_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Archavon The Stone Watcher";
            Content = "T7.5";
            Instance = "The Vault of Archavon";
            Version = "25 Man";
            Health = 9970675f;
            BerserkTimer = 5 * 60;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.75f;
            // Every 30 seconds for 5 seconds you gotta catch up to him as he jumps around
            Moves.Add(new Move() {
                Frequency = 30f,
                Duration = 5 * 1000,
                Chance = 3f / Max_Players,
                Breakable = false
            });
            /* TODO:
             * Rock Shards
             * Crushing Leap
             * Stomp (this also stuns)
             * Impale (this also stuns)
             */
        }
    }
    // ===== The Eye of Eternity ======================
    public class Malygos_25 : Malygos_10 {
        public Malygos_25() {
            // If not listed here use values from defaults
            // Basics
            Content = "T7.5";
            Version = "25 Man";
            Health = 2230000f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2; // you really only need 1 but adding 2nd for the adds phase and sparks in 1st phase
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.95f;
            // Every 70-120 seconds for 16 seconds you can't be on the target
            // Adding 4 seconds to the Duration for moving out before starts and then back in after
            Moves.Add(new Move() {
                Frequency = (70f + 120f) / 2f,
                Duration = (16f+4f) * 1000f,
                Chance = 1f,
                Breakable = false
            });
            /* TODO:
             */
        }
    }
    #endregion
    #region T8 Content
    // ===== The Vault of Archavon ====================
    public class EmalonTheStormWatcher_10 : BossHandler {
        public EmalonTheStormWatcher_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Emalon the Storm Watcher";
            Content = "T8";
            Instance = "The Vault of Archavon";
            Version = "10 Man";
            Health = 2789000f;
            BerserkTimer = 6 * 60;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  3;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 90000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Every 45 seconds for 18 seconds dps has to be on the overcharged add (it wipes the raid at 20 sec)
            // Adding 5 seconds to the Duration for moving out before starts and then 5 for back in after
            Moves.Add(new Move() {
                Frequency = 45f + 18f,
                Duration = (18f + 5f + 5f) * 1000f,
                Chance = 1f,
                Breakable = false,
            });
            // Lightning Nova, usually happens a few seconds after the overcharged add dies
            // (right when most melee reaches the boss again) Simming 4 to run out and 4 to get back
            Moves.Add(new Move() {
                Frequency = 45f + 18f,
                Duration = (4f + 4f) * 1000f,
                Chance = 1f,
                Breakable = false,
            });
            /* TODO:
             * Adds Damage
             * Chain Lightning Damage
             * Lightning Nova Damage
             */
        }
    }
    // ===== Ulduar ===================================
    // The Siege
        // TODO: Flame Leviathan
        // TODO: Ignis the Furnace Master
        // TODO: Razorscale
        // TODO: XT-002 Deconstructor
    // The Antechamber
        // TODO: Assembly of Iron
        // TODO: Kologarn
    public class Auriaya_10 : BossHandler {
        public Auriaya_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Auriaya";
            Content = "T8";
            Instance = "Ulduar";
            Version = "10 Man";
            BerserkTimer = 10 * 60;
            Health = 3137625f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  2;
            Min_Healers =  2;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 90000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.00f; // This is a boss where you CANNOT be behind her or she Fubar's the raid
            // She summons extra targets a lot, most of the time, they are within melee range of persons on the boss
            // Guardian Swarm: Marks a player and summons a pack of 10 Swarming Guardians with low health around them soon after.
            // Feral Defender: If you leave him alone, he's up about 90% of the fight
            MultiTargsPerc = 0.90f; // need to sim this out
            MaxNumTargets  = 10f; // need to drop this down to only when the swarm is up
            // Terrifying Screech: Raid-wide fear for 5 seconds. Magic effect.
            // Going to assume the CD is 45 sec for now (cuz I know she doesnt do it every 8 sec)
            FearingTargsFreq = 45f;
            FearingTargsDur = 5f * 1000f;
            // Fight Requirements
            /* TODO:
             */
        }
    }
    // The Keepers
        // TODO: Mimiron
        // TODO: Freya
        // TODO: Thorim
    public class Hodir_10 : BossHandler {
        public Hodir_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Hodir";
            Content = "T8";
            Instance = "Ulduar";
            Version = "10 Man";
            BerserkTimer = 8 * 60;
            Health = 8115990f;
            // Fight Requirements
            Max_Players = 10;
            Min_Tanks   =  1;
            Min_Healers =  3;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 90000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.75f; // he moves A LOT so it's hard to stay behind him at all times
            // Freeze: Inflicts 5,550 to 6,450 Frost damage to players within 10 yards. Also roots
            // the targets in place for 10 seconds. The rooting component of the spell is a magic debuff.
            // Going to assume the CD is 45 sec for now
            RootingTargsFreq = 45f;
            RootingTargsDur = 10f * 1000f;
            /* TODO:
             */
        }
    }
    // The Descent into Madness
        // TODO: General Vezax
        // TODO: Yogg-Saron
    // Supermassive
    // TODO: Algalon the Observer
    #endregion
    #region T8.5 Content
    // ===== The Vault of Archavon ====================
    public class EmalonTheStormWatcher_25 : BossHandler {
        public EmalonTheStormWatcher_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Emalon the Storm Watcher";
            Content = "T8.5";
            Instance = "The Vault of Archavon";
            Version = "25 Man";
            Health = 11156000f;
            BerserkTimer = 6 * 60;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  5;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 180000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 1.00f;
            // Every 45 seconds for 18 seconds dps has to be on the overcharged add (it wipes the raid at 20 sec)
            // Adding 5 seconds to the Duration for moving out before starts and then 5 for back in after
            Moves.Add(new Move() {
                Frequency = 45f + 18f,
                Duration = (18f + 5f + 5f) * 1000f,
                Chance = 1f,
                Breakable = false,
            });
            // Lightning Nova, usually happens a few seconds after the overcharged add dies
            // (right when most melee reaches the boss again) Simming 4 to run out and 4 to get back
            Moves.Add(new Move() {
                Frequency = 45f + 18f,
                Duration = (4f + 4f) * 1000f,
                Chance = 1f,
                Breakable = false,
            });
            /* TODO:
             * Adds Damage
             * Chain Lightning Damage
             * Lightning Nova Damage
             */
        }
    }
    // ===== Ulduar ===================================
    // The Siege
        // TODO: Flame Leviathan
        // TODO: Ignis the Furnace Master
        // TODO: Razorscale
        // TODO: XT-002 Deconstructor
    // The Antechamber
        // TODO: Assembly of Iron
        // TODO: Kologarn
    public class Auriaya_25 : BossHandler {
        public Auriaya_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Auriaya";
            Content = "T8.5";
            Instance = "Ulduar";
            Version = "25 Man";
            BerserkTimer = 10 * 60;
            Health = 16734000f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  2;
            Min_Healers =  4;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 180000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.00f; // This is a boss where you CANNOT be behind her or she Fubar's the raid
            // She summons extra targets a lot, most of the time, they are within melee range of persons on the boss
            // Guardian Swarm: Marks a player and summons a pack of 10 Swarming Guardians with low health around them soon after.
            // Feral Defender: If you leave him alone, he's up about 90% of the fight
            MultiTargsPerc = 0.90f; // need to sim this out
            MaxNumTargets  = 10f; // need to drop this down to only when the swarm is up
            // Terrifying Screech: Raid-wide fear for 5 seconds. Magic effect.
            // Going to assume the CD is 45 sec for now (cuz I know she doesnt do it every 8 sec)
            FearingTargsFreq = 45f;
            FearingTargsDur = 5f * 1000f;
            /* TODO:
             */
        }
    }
    // The Keepers
        // TODO: Mimiron
        // TODO: Freya
        // TODO: Thorim
    public class Hodir_25 : BossHandler {
        public Hodir_25() {
            // If not listed here use values from defaults
            // Basics
            Name = "Hodir";
            Content = "T8.5";
            Instance = "Ulduar";
            Version = "25 Man";
            BerserkTimer = 8 * 60;
            Health = 32477904f;
            // Fight Requirements
            Max_Players = 25;
            Min_Tanks   =  1;
            Min_Healers =  5;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 180000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
            });
            // Situational Changes
            InBackPerc_Melee = 0.75f; // he moves A LOT so it's hard to stay behind him at all times
            // Freeze: Inflicts 5,550 to 6,450 Frost damage to players within 10 yards. Also roots
            // the targets in place for 10 seconds. The rooting component of the spell is a magic debuff.
            // Going to assume the CD is 45 sec for now
            RootingTargsFreq = 45f;
            RootingTargsDur = 10f * 1000f;
            /* TODO:
             */
        }
    }
    // The Descent into Madness
        // TODO: General Vezax
        // TODO: Yogg-Saron
    // Supermassive
    // TODO: Algalon the Observer
    #endregion
    #region T9 (10) Content
    // ===== The Vault of Archavon ====================
    public class KoralonTheFlameWatcher_10 : BossHandler {
        public KoralonTheFlameWatcher_10() {
            // If not listed here use values from defaults
            // Basics
            Name = "Koralon the Flame Watcher";
            Content = "T9";
            Instance = "The Vault of Archavon";
            Version = "10 Man";
            Health = 4183500f;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 60000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
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
            Content = "T9.5";
            Version = "25 Man";
            Health = 4183500f;
            // Resistance
            // Attacks
            Attacks.Add(new Attack {
                Name = "Melee",
                DamageType = ItemDamageType.Physical,
                DamagePerHit = 120000f,
                MaxNumTargets = 1f,
                AttackSpeed = 2.0f,
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