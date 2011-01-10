using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rawr.Bosses
{
    #region Baradin Hold
    // ===== Baradin Hold ====================
    public class Argaloth : MultiDiffBoss
    {
        public Argaloth()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Pit Lord Argaloth";
            Instance = "Baradin Hold";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_0, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, };
            #endregion
            #region Basics
            Health = new float[] { 21473000f, 64419000f, 0, 0 };
            // 5 minute Berserk timer
            BerserkTimer = new int[] { 5 * 60, 5 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.60f, 0.60f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 3, 5, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                // Meteor Slash - Deals 200000/475000 Fire damage split between enemy targets within 65 yards in front of the caster.
                //     Increases Fire damage taken to all targets affected by 100%.
                //     Half the raid stacks on one side and takes the debuff for 1 stack
                this[i].Attacks.Add(new Attack
                {
                    Name = "Meteor Slash",
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    // Half the raid is getting hit with about 40k damage per attack.
                    DamagePerHit = new float[] { 200000, 475000, 0, 0 }[i] / (.5f * this[i].Max_Players),
                    MaxNumTargets = this[i].Max_Players / 2f,
                    // Berserk time minus Firestorm and Firestorm cast time (Firestorm (15 second spell + 3 second cast time; spell is cast 2 times in the fight) - 264 seconds
                    // Frequency is 16.5 seconds, with a 1.25 second cast time - 17.75 seconds
                    // Divide those two numbers to get the number of times in the fight that the spell is cast - 14.87324
                    // all dividing the Berserk timer (should come up with 20.17045 second attack speed)
                    AttackSpeed = this[i].BerserkTimer / ((this[i].BerserkTimer - ((15f + 3f) * 2f)) / (16.5f + 1.25f)),

                    Dodgable = false,
                    Missable = false,
                    Parryable = false,
                    Blockable = false,
                });

                // Consuming Darkness - The Shambling Doom inflicts 2,925 to 3,075 Shadow damage and additional Shadow damage every 0.5 sec for 15 sec. 100 yard range. Instant
                // Damage appears to increase additively as the dot progress:
                // .5s = 3k, 1s = 6k, 1.5s = 9k, ..., 15s = 90k
                // total damage for the full duration is 1,395,000
                // Can be dispelled
                this[i].Attacks.Add(new DoT
                {
                    Name = "Consuming Darkness",
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = (2925f / 3075f) / 2f,
                    NumTicks = 30f,
                    TickInterval = .5f,
                    DamagePerTick = 1395000f / 30f,
                    Interruptable = true,
                    MaxNumTargets = 1f,
                    // Berserk time minus Firestorm and Firestorm cast time (Firestorm (15 second spell + 3 second cast time; spell is cast 2 times in the fight) - 264 seconds
                    // Frequency is 22 seconds - 22 seconds
                    // Divide those two numbers to get the number of times in the fight that the spell is cast - 12
                    // all dividing the Berserk timer (should come up with 25 second attack speed)
                    AttackSpeed = this[i].BerserkTimer / ((this[i].BerserkTimer - ((15f + 3f) * 2f)) / (22f)),

                    Dodgable = false,
                    Missable = false,
                    Parryable = false,
                    Blockable = false,
                });

                // Fel Firestorm - Used at 66% and 33%. Argaloth shoots fireballs into the air which will fall down and leave a patch of flame on the ground.
                //        These patches deal 8,287 - 8,712 Fire damage as long as you are standing in them and they can appear (or will only appear?) directly under a players feet.
                //        Most of the room will be covered in flames at the end of this ability (think of Mimiron hardmode) before all flames will disappear and he will resume
                //        attacking you with his normal abilities. 50,000 yards range. 3 sec cast. Lasts 15 seconds. 
                this[i].Attacks.Add(new DoT
                {
                    Name = "Fel Firestorm",
                    DamageType = ItemDamageType.Fire,
                    TickInterval = 1f,
                    NumTicks = 15f,
                    DamagePerTick = (8287f + 8712f) / 2f,
                    MaxNumTargets = this[i].Max_Players,
                    AttackSpeed = 133f,

                    Dodgable = false,
                    Missable = false,
                    Parryable = false,
                    Blockable = false,
                });
                this[i].Moves.Add(new Impedance()
                {
                    Frequency = 133f,
                    // Fires are still around 3 seconds after he finishes channeling the spell and people need to get back to their groups
                    Duration = (15f + 3f) * 1000f,
                    Chance = 1f,
                    Breakable = false,
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
             */
        }
    }
    #endregion

    #region Blackwing Descent
    // ===== Blackwing Descent =========================
    public class Magmaw : MultiDiffBoss
    {
        public Magmaw()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Magmaw";
            Instance = "Blackwing Descent";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 33497880f, 105000000f, 46895800f, 120000000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             */
        }
    }

    public class OmnitronDefenseSystem : MultiDiffBoss
    {
        public OmnitronDefenseSystem()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Omnitron Defense System";
            Instance = "Blackwing Descent";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 32209000f, 98790000f, 45080000f, 138306000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             * "Magmatron, the fire golem"
             * Incineration Security Measure (10) / (25) - This ability is a raid-wide spell that is channeled and does 10k(10m)/15k(25m) fire damage every second
             *      for 4 seconds.
             *      
             * Acquiring Target -> Flamethrower (10) / (25) - Magmatron will randomly target a raid member and channel a beam on them for 4 seconds to acquire his target.
             *      After that time, Magmatron will use Flamethrower to that target and everyone directly behind him in a small cone, dealing 35k(10m)/50k(25m) damage
             *      every second for 4 seconds.
             *      
             * Barrier (10) / (25) - Magmatron "shield ability" in which he forms a barrier around himself, absorbing 300k(10m)/900k(25m) damage, and if broken, causes a
             *      Backdraft (10) / (25), which deals 75k(10m)/115k(25m) to all raid members.
             *      
             * "Arcanotron, the arcane golem"
             * Arcane Annihilator (10) / (25) - 1 second cast time spell used on a randomly targeted raid member, but is interruptable. It deals 40k(10m)/50k(25m) arcane
             *      damage if it does go through.
             *      
             * Power Generator (10) / (25) - Creates a whirling pool of energy underneath a golem that increases the damage of any raid member or boss mob standing in it
             *      by 50% and restores 250(10m)/500(25m) mana per every 0.5 second (Similar to Rune of Power from Iron Council). Lasts for 25 seconds (however it appears
             *      as if the buff lasts for 10-15 seconds after the Power Generator despawns).
             *      
             * Power Conversion - Arcanotron's "shield ability" in which he gains a stacking buff, Converted Power, when taking damage that increases his magic damage and
             *      cast speed by 10% per stack. Currently can be removed with a mage's Spellsteal.
             *      
             * "Toxitron, the poison golem"
             * Chemical Cloud (10) / (25) - Large raidus debuff that increases damage taken by 50% and deals 3k(10m)/4k(25m) damamge every 5 seconds.
             * 
             * Poison Protocol (10) / (25) - Channeled for 9 seconds, and spawns 3(10m)/6(25m) Poison Bombs, one every 3(10m)/1.5(25m) seconds. The Poison Bombs (10) / (25)
             *      have 78k(10m)/78k(25m) health and fixate on a target, then explode if they reach their target, dealing 90k(10m)/125k(25m) nature damage to players within
             *      the area and spawn a Slime Pool which deals additional damage to players that stand in it.
             *      
             * Poison Soaked Shell - Toxitron's "shield ability" which causes player that attacks him to gain a stacking debuff, Soaked in Poison (10) / (25), which causes
             *      that player to take 2k(10m)/5.5k(25m) nature damage every 2 seconds, however, they also will deal 10k additional nature damage to targets of their attacks.
             *      
             * "Electron, electricity golem"
             * Lightning Conductor (10) / (25) - A randomly targeted raid member will get this debuff which has a 10(10m)/15(25m) second duration and deals 25k damage to raid
             *      members with 8 yards every 2 seconds.
             *      
             * Electrical Discharge (10) / (25) - A chain lightning type ability which deals 30k(10m)40k(25m) nature damage to a target and then to up to 2 additional targets
             *      within 8 yards, damage increasing 20% each jump.
             *      
             * Unstable Shield - Electron's "shield ability" which causes at Static Shock at an attackers location, dealing 40k nature damage to raiders within 7 yards.
             */
        }
    }

    public class Maloriak : MultiDiffBoss
    {
        public Maloriak()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Maloriak";
            Instance = "Blackwing Descent";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 20603000f, 72150000f, 28844200f, 101010000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             * Melee - Hits relatively hard at 15k-30k(10m)/45k-65k(25m) after mitigation, and after the Green Vial, doubled because of Debilitating Slime for 15 seconds.
             * 
             * Release Aberrations - This is a 1.5 second summon used by the boss about every 30 seconds and can be interrupted. There will be 4 casts before each Green
             *      Vial phase, which each wave summoning 3 Aberration adds with 365k(10m)/1.3m(25m) health and melee for about 4k(10)/8k(25m) after mitigation and only
             *      1 stack of their buff, Growth Catalyst.
             *      
             * Arcane Storm (10) / (25) - This 6 second channeled spell is used often throughout the encounter and can be interrupted. It deals 8k(10m)/15k(25m) arcane
             *      damage to each raid member at 1 second intervals.
             *      
             * Remedy (10) / (25) - Maloriak occasionally uses this to buff himself, healing for 25k(10m)/75k(25m) and restoring 2k mana per second for 10 seconds. However,
             *      it can be removed with offensive magic dispels or can be taken via a mage's Spellsteal.
             *      
             * Berserk - Damage increased by 500%, attack and movement speed increased by 150%, and immune to Taunt effects. Triggered at 6 minutes into the fight.
             * 
             * "Red Vial"
             * Fire Imbued - This 40 second buff on Maloriak is triggered upon tossing a Red Vial into his cauldron. It allows the usage of his fire-based special abilities.
             * 
             * Consuming Flames (10) / (25) - This 10 second debuff during the Red Vial phase is placed on a random raid member and causes them to take 3k(10m)/6k(25m)
             *      fire damage per second, and additionally, causes the target to take additional damage equal to 25% of the damage taken from other magic sources.
             *      (Example: In 10-man, having this debuff and getting breathed on from a Scorching Blast that hits all 10 raid members, reducing the individual player's
             *      damage from Scorching Blast to 20k, will result in an additional 5k (which is 25% of 20,000) damage taken per second from Consuming Flames' secondary effect.)
             *      
             * Scorching Blast (10) / (25) - This ability is only used when Maloriak is Fire Imbued, and deals 200k(10m)/500k(25m) fire damage split between all players in a
             *      cone 60 yards in front of the boss.
             *      
             * "Blue Vial"
             * Frost Imbued - This 40 second buff on Maloriak is triggered upon tossing a Blue Vial into his cauldron. It allows the usage of his frost-based special abilities.
             * 
             * Biting Chill (10) / (25) - This debuff is placed on 1-2(10m)/3-5(25m) raid members that are within 10 yards of Maloriak. The debuff lasts 10 seconds and deals
             *      5k(10m)/7.5k(25m) frost damage per second to anyone within (6???) yards.
             *      
             * Flash Freeze (10) / (25) - This ability is used on a raid member at ranged while Maloriak is Frost Imbued, encasing them in a block of ice, also called a
             *      Flash Freeze, with 5k(10m)/17.5k(25m) health. Upon becoming frozen, that player will be take 50k(10m)/75k(25m) frost damage and be unable to move or use
             *      abilities. Additionally, other players within (10???) yards of the initial target will take the damage and be placed in Flash Freeze ice blocks. When any
             *      Flash Freeze ice block is destroyed that player will Shatter (10) / (25) dealing an additional 50k(10m)/75k(25m) frost damage to all players within (10???)
             *      yards and removing the Flash Freeze debuff on any other players within (10???) yards, freeing them as well.
             *      
             * "Green Vial"
             * Debilitating Slime (10) / (25) - When the Green Vial is tossed into the cauldron, it will knock Maloriak back 15 yards and cover the room in a green spray,
             *      debuffing all friendly and enemy targets in the room for 15 seconds which increases damage taken by 100%(10m)/150%(25m), additionally, it will temporarily
             *      remove the Growth Catalyst buff on any Aberrations or from Maloriak himself. This will make incoming tank damage very high, but will also drastically increase raid DPS.
             *      
             * "Release All"
             * Release All - When Maloriak reaches 20% health, he will cast this ability, summoning two Prime Subject and will summon 3 Aberrations for every Release Aberration
             *      cast that was interrupted throughout the fight. The Prime Subjects have 8.6million(10m)/30.1million(25m) health and melee much harder than the smaller adds,
             *      with each hit for about 20k-25k(10m)/30k-40k(25m) damage after mitigation. They will occasionally fixate a random target at attempt to kill it.
             *      
             * Magma Jets (10) / (25) - This ability is only used by Maloriak after he has used Release All. After a 2 second cast, the boss will charge toward a random raid
             *      member and deal 25k(10m)/50k(25m) fire damage to anyone in the path and knock them back 30 yards. It will also leave a path of fire that deals 5k(10m)/10k(25m)
             *      fire damage per second to anyone that gets within 3 yards. The trails dissipate after 20 seconds.
             *      
             * Absolute Zero (10) / (25) - This ability is only used by Maloriak after he has used Release All. This ability will form a circle near a random raid member that will
             *      spawn a floating ice sphere after 3 seconds. When the ball becomes active, it will float randomly around the room. If it comes into contact with a player, it
             *      will explode dealing 20k(10m)/40k(25m) frost damage and cause a 10 yard knock back to all nearby players.
             *      
             * Acid Nova (10) / (25) - This ability is only used by Maloriak after he has used Release All. This ability is as a blast that will place a 10 second debuff on everyone
             *      in the raid which deals 5k(10m)/7.5k(25m) nature damage per second.
             */
        }
    }

    public class Atramedes : MultiDiffBoss
    {
        public Atramedes()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Atramedes";
            Instance = "Blackwing Descent";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 32632000f, 97920000f, 45684800f, 137088000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             * "Ancient Dwarven Shield" - There are 8 of these (4 on each side of the room) and can be interacted with by any raid member. Using one
             *      causes a Resonating Clash which clears all sound meters and interrupts any spells being cast by Atramedes, as well as giving him a
             *      temporary feeling of Vertigo which causes a 5 second stun where he is vulnerable and takes 50% increased damage. Afterwards, he will
             *      immediately cast Sonic Flames to instantly destroy and melt the Ancient Dwarven Shield that was used.
             *      (*Note: The player that uses the Ancient Dwarven Shield will get an invisible trigger from the Resonating Clash, making them the defaut
             *      for any of Atramedes sound level based attacks until another player passes their sound level.
             *      
             * On the ground:
             * Melee - There is very little tank damage in this fight. Atramedes physical hits are for 18k-25k(10m)/25k-40k(25m) damage, however, there are
             *      a lot of breaks in his attacks such as when he is channeling Searing Flames or Sonic Breath, and the entire time he is in the air phase which
             *      does not need a tank.
             *      
             * Devastation - This ability is a nonstop blasting every 1.5 seconds of Devastation fireball attacks on a player that has become Noisy! which each deal 25k fire damage.
             * 
             * Modulation - This is an unavoidable raid-wide pulse that deals 20k Shadow Damage and increases sound level by 7.
             * 
             * Searing Flames - This is an 8 second channeled cast that can and should be interrupted via using and Ancient Dwarven Shield. Otherwise, every second
             *      the entire raid will take 10k fire damage and get a stacking debuff that increases further fire damage by 25% per stack for 4 seconds.
             *      Additionally, each tick of Searing Flame causes sound meters to increase by 10.
             *      
             * Sonic Breath - Atramedes will begin this ability by putting a Tracking debuff on the player in the raid (with the highest sound level???) and after a
             *      2 second cast, will begin to channel this Sonic Breath for 6 second in the direction of the tracked player. Anyone caught in the path will take 15k fire
             *      damage per second and have their sound level increased by 20 per tick. During this channel, Atramedes will spin to follow the path of the tracked target at a
             *      speed relative to that players sound level, moving faster is their sound meter is high, or slower if it is low.
             *      
             * Sonar Pulse - Three of these pulsating discs are emited from Atramedes an float straight in a random direction in the room. Players that are hit by these will
             *      have their sound meter increased by 5 every 0.5 seconds that they are in contact. They should always be avoided, even though they do no damage directly.
             *      (However, 6k arcane damage per tick in heroic)
             *      
             * In the Air:
             * Sonar Bomb - Atramedes marks several locations on the ground with what look like Sonar Pulses, and after 3 seconds that place will be bombed which hits everyone within
             *      (8???) yards and deals 10k arcane damage and increases sound level by 20. (30k damage and 30 sound level in heroic)
             *      
             * Sonic Fireball - To prevent the raid from clumping in 1 place, Atramedes will occasionally launch a few of these fireballs in the location of raid members,
             *      each dealing 30k fire damage to all players within (8??) yards.
             *      
             * Roaring Flame Breath - This ability is used throughout all of the air phase. He channels a Roaring Flame Breath dealing 10k fire damage that ticks every 0.5
             *      seconds and follows in the path of the chosen raid member (with the highest sound level???) and moves to follow it at a speed relative to that players sound
             *      level, moving faster is their sound meter is high, or slower if it is low. In the wake of this ability there will be patches of Roaring Flame which deal 15k
             *      fire damage and increase sound level by 10 to players that touch them. Additionally, players that stand in the patches too long will gain the debuff Roaring
             *      Flame which deals an additional 15k fire damage and then ticks every second for 4 seconds resulting in 8k fire damage and a sound meter increase by 5.
             *      The ground Roaring Flame patches despawn after 45 seconds.
             *      (*Note: During this phase if the chased player's sound level is getting high, someone use use an Ancient Dwarven Shield to reset all sound meters, stun
             *      Atramedes, and force him to start pursuit again at the slowest speed following whoever caused the Resonating Clash.
             */
        }
    }

    public class Chimaron : MultiDiffBoss
    {
        public Chimaron()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Chimaron";
            Instance = "Blackwing Descent";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 25940000f, 90620000f, 36316000f, 126868000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                // Chimaeron attack extremely slowly with 4-5 second between swings, however, hit hits are massive and deal 50k-65k(10m)/60k-75k(25m)
                //      after mitigation and before the damage taken debuff he can place on tanks.
                this[i].Attacks.Add(new Attack
                {
                    Name = "Melee",
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 2f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 4.0f,
                    AttackType = ATTACK_TYPES.AT_MELEE,

                    IgnoresMeleeDPS = true,
                    IgnoresRangedDPS = true,
                    IgnoresHealers = true,

                    IsTheDefaultMelee = true,
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
             * "Bile-O-Tron" - This is a minion of Finkle Einhorn (who is locked in a cage behind the boss) and plays a crucial role in this fight. It grants
             *      a buff, Finkle's Mixture, to the raid, however, will occasionally will go offline at which point the raid much change strategy.
             *      
             * Finkle's Mixture - This ability prevent Chimaeron's abilites, all of which are extremely powerful, from instantly killing a raid member.
             * 
             * Melee - Chimaeron attack extremely slowly with 4-5 second between swings, however, hit hits are massive and deal 50k-65k(10m)/60k-75k(25m)
             *      after mitigation and before the damage taken debuff he can place on tanks.
             *      
             * Caustic Slime (10) / (25) - This is a ability used often toward a random raid member which deals 280k(10m)/805k(25m) nature damage split between
             *      everyone within 6 yards and leaves a 4 second debuff that reduces chance to hit with spells or attacks by 75%. This can be survived as long
             *      as the target is either above 10k health with Finkle's Mixture, or if the majority of the raid is clumped on the target, however, they all will
             *      receive the debuff lowering raid damage.
             *      
             * Massacre - This ability will hit everyone in the raid for 999,999 physical damage, however, is not lethal as long as everyone has Finkle's Mixture
             *      from the Bile-O-Tron and is above 10k health.
             *      Tenative cd timer is 30 seconds.
             *      
             * Break - This is a debuff placed on the current tank that increases their physical damage taken by 25% and stacks. This debuff lasts 1 minute and is
             *      reapplied roughly every 18 seconds.
             *      
             * Double Attack - This ability will simply allow Chimaeron's to hit again immediately after his next attack, instead of there being a long swing timer.
             *      Because of Break, it is recommended that an offtank immediately taunts off the current tank to "soak" the Double Attack so that the first melee
             *      hit, which will almost definitely take a tank with several stacks of Break from near full heal to 1 with Finkle's Mixture, allowing them to be
             *      hit with a second rapid hit while under 10k health and dying. After the soaked hit, the main tank can retaunt to regain aggro.
             *      
             * Feud - This is a 30 second long channeled period after every 3rd Massacre (which knocks the Bile-O-Tron offline until it can Reroute Power). During this
             *      time, Chimaeron does no melee attacks, but will continue to cast Caustic Slime which must be soaked by the entire raid due to the temporarily loss
             *      of Finkle's Mixture.
             *      Tenative cd timer is 90 seconds.
             *      
             * Mortality - Places a debuff on the entire raid, reducing healing effects by 99% This ability is used at 21% and also applies a Mortality buff to Chimaeron
             *      that increases his size by 30%, increases his damage taken by 20%, and make him immune to Taunt effects.
             */
        }
    }

    public class Nefarian : MultiDiffBoss
    {
        public Nefarian()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Nefarian";
            Instance = "Blackwing Descent";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Onyxia = 6,441,000 / 22,500,000 / 9,017,400 / 31,500,000
            // Nefarion = 25,940,000 / 90,582,607 / 36,316,000 / 126,815,650
            Health = new float[] { ( 6441000f + 25940000f ), ( 22500000f + 90582607f ), ( 9017400f + 36316000f ), ( 31500000f + 126815650f ) };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            // 1 tank to tank Oyxia, 1 for kiting (though a hunter or kiting class can do this), and 1 for Nafarion
            Min_Tanks = new int[] { 3, 3, 3, 3 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             */
        }
    }

    #endregion

    #region The Bastion of Twilight
    // ===== The Bastion of Twilight =========================
    public class ValionaAndTheralion : MultiDiffBoss
    {
        public ValionaAndTheralion()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Valiona & Theralion";
            Instance = "The Bastion of Twilight";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Health is split between both Valiona and Theralion
            Health = new float[] { 32210000f, 97916880f, 45952000f, 137083632f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             * Taken from Tankspot - http://www.tankspot.com/showthread.php?71345-Preview-of-Cataclysm-Raid-Bosses
             * "Valiona, the purple dragon"
             * On the ground:
             * Melee - Hits for relatively weak physical damage on the tank, ranging from 15k-25k(10m)/17k-30k(25m), but more healing is needed for the raid damage from the other fight mechanics.
             * 
             * [Heroic] Twilight Shift - A heroic only ability used approximately every 20 seconds. It applies a debuff on the current highest threat target that stacks up to 5 times and has a 100
             *      second duration. At 5 stacks it will cause Shifting Reality which causes 35k shadow damage to all players within 8 yards and causes them to shift into the Twilight Realm.
             *      Additionally, you will deal (10%???) increased damage while in the Twilight Realm.
             *    
             * Blackout (10) / (25) - This is cast by Valiona on a random raid member at which point it can be removed by absorbing 20k(10m)/50k(25m) healing or by lasting the 15 second duration.
             *      It can also be dispelled on non-heroic. When it expires, it deals 350k(10m)/850k(25m) [Heroic: 400k(10m)/1.3mil(25m)] shadow damage split between everyone within (10-20???) yards.
             *      Tenative cd is 45 seconds. It cancels while in the air.
             * 
             * Devouring Flames (10) / (25) - Valiona will turn toward a random raid member and start this 2.5 second cast during which players in the way can attempt to run out of the way.
             *      Then for 5 seconds all player in front of Valiona will take 50k(10m)/80k(25m) [Heroic: 110k(10m)/(25m)] shadow damage per second, however, this damage can be lessened by being
             *      further away from the dragon.
             *      
             * In the air:
             * Twilight Meteorite (10) / (25) - This is cast by Valiona on a random raid member, marking them with an arrow. After 6 seconds, that player will take 150k(10m)/300k(25m)
             *      [Heroic: 190k(10m)/360k(25m)] shadow damage split between everyone within (10???) yards.
             * 
             * Deep Breath - Aerial special casted 3 times before descending from the air. Valiona travels to either the far east or west of the room and breathes Twilight Flames (10) / (25)
             *      along the ground straight across, dealing 15k(10m)/(25m) [Heroic: 25k(10m)/30k(25m)] shadow damage and sending anyone in them path into the Twilight Realm. The horizontal trail
             *      of Twilight Flames (10) / (25) remain on the ground and deals 6.5k(10m)/8.5k(25m) additional shadow damage every second to nearby players. This ability will cover a third of the
             *      room, either the north, south, or center (similar to Felmyst's ability in Sunwell Plateau).
             *      
             * "Theralion, the blue dragon"
             * On the ground:
             * Melee - Hits for relatively weak physical damage on the tank, ranging from 15k-25k(10m)/17k-30k(25m), but more healing is needed for the raid damage from the other fight mechanics.
             * [Heroic]Twilight Shift - A heroic only ability used approximately every 20 seconds. It applies a debuff on the current highest threat target that stacks up to 5 times and has a
             *      100 second duration. At 5 stacks it will cause Shifting Reality which causes 35k shadow damage to all players within 8 yards and causes them to shift into the Twilight Realm.
             *      Additionally, you will deal (10%???) increased damage while in the Twilight Realm.
             *      
             * Fabulous Flames (10) / (25) - This is cast by Theralion on a random raid member. Everyone within 15 yards will take 15.5k(10m)/25k(25m) shadow damage and then a Fabulous Flames
             *      patch (10) / (25) will spawn dealing an additional 15k(10)/(25m) [Heroic: 19k(10m)/(25m)] shadow damage per second to players still in it. These despawn after 45 seconds and
             *      are cast at 15 second intervals.
             *      
             * Engulfing Magic - This 20 second duration debuff is used on a random raid member [Heroic: 2 raid members] which increases the player's damage and healing by 100%, but deals damage
             *      to all friendly players within (8-10???) yards equivalent to the amount of healing or damage done. This damage does not harm the player debuffed with Engulfing Magic.
             *      Tenative cd is 37 seconds.It cancels while in the air.
             *      
             * In the air:
             * Twilight Blast (10) / (25) - Every 3.2 [Heroic: 2.0] seconds, one of these bombs are launched on a random raid member, dealing 30k(10m)/40k(25m) damage to all players within (6-8???) yards.
             * 
             * Dazzling Destruction (10) / (25) - Aerial special cast 3 times before descending from the air. Theralion will mark 3 swirling vortex locations and after 4 second, those spots will be bombed,
             *      dealing 15k(10m)/25k(25m) shadow damage to anyone within (12-15???) yards and sending them into the Twilight Realm.
             *      
             * "Twilight Realm"
             * Twilight Zone - All players inside of the Twilight Realm receive a stacking debuff every 2 seconds that deals 5k [Heroic: 7.5k] shadow damage and increases shadow damage taken by
             *      10% until clicking a portal near the perimeter of the room that causes the player to Leave Twilight Realm.
             *      
             * Unstable Twilight (10) / (25) - Floating spheres of unstable twilight energy exist in the Twilight Realm spread out approximately 10 yards from each other. Any player that touches an orb
             *      will cause it to explodes, dealing 40k [Heroic: 55k] shadow damage to all players within (8-10???) yards.
             *      
             * Dazzling Destruction - Players hit by this ability when already in the Twilight Realm will take 500k [Heroic: 750k] shadow damage.
             * 
             * Twilight Flames (10) / (25) - Players hit by the direct attack of this ability when already in the Twilight Realm will take 500k [Heroic: 750k] shadow damage.
             * 
             * Twilight Sentry - Several of these dragonkin NPCs exists in the Twilight Realm and each has ???k(10m)/???(25m) [Heroic: 309k(10m)/???k(25m)] health.
             *      On heroic mode, they will channel [Heroic]Twilight Rifts that exist in the normal realm. They use [Heroic]Rift Blast (10) / (25) attacks in the normal realm to shoot projectiles
             *      every 3 seconds that deal 10k(10m)/15k(25m) shadow damage to a random player.
             *      (*Note: May have to be killed on heroic in order to leave the Twilight Realm or to stop Rift Blast attacks in the normal realm???)
            */
        }
    }

    public class HalfusWyrmbreaker : MultiDiffBoss
    {
        public HalfusWyrmbreaker()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Halfus Wyrmbreaker";
            Instance = "The Bastion of Twilight";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Kill 3 of the 4 dragons plus Halfus, tanked on top of the boss
            // One of the dragons gives increased damage while stunning the boss
            // Dragons = 4,150,000 / 12,600,649 / 5,810,000 / 17,640,909
            // Halfus = 32,467,000 / 115,954,200 / 45,453,800 / 162,335,880
            Health = new float[] { 32467000f, 115954200f, 45453800f, 162335880f };
            BerserkTimer = new int[] { 6 * 60, 6 * 60, 6 * 60, 6 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             * Melee - Halfus Wyrmbreaker's melee attacks hit for around 12-18k(10m)/15-22k(25m) after mitigation.
             * 
             * Fire Breath(name???) - This raid-wide fire damage ability is cast by a dragon behind Halfus Wyrmbreaker. Deals roughly 900(10m)/2200(25m) damage per second for 8 seconds
             *      after resists. Also has a small single target Fire Blast attack. The dragon cannot be attacked and his attacks cannot be outranged or avoided via line of sight.
             *      
             * Frenzied Assault (10) / (25) - This ability is a buff that Halfus starts the fight with that increases his attack speed by 100%(10m)/120%(25m).
             * 
             * Violent Spin(name???) - At 30%, Halfus will begin to spin violently and do ~4k(10m)/7k(25m) damage per second to everyone within melee range while constantly reseting his
             *      threat table. Currently, he is not affected from the diminishing returns of Taunt during this phase.
             *      
             * Berserk - Damage increased by 500%, attack and movement speed increased by 150%, and immune to Taunt effects. Triggered at 6 minutes into the fight.
             * 
             * "Cage of Whelps"
             * This object can be triggered at any time to spawn ~20 Orphaned Emerald Whelps with 520k(10m)/1.8million(25m) health. These adds can then be killed to reduce raid damage
             *      from Dragon's Fire Breath and gives Halfus a debuff that increases his damage taken. They melee attack for 4-6k???(10m)/5-10k(25m) after mitigation.
             *      
             * Atrophic Aura - Triggers when these adds are spawned and reduces raid members' damage dealt by 2000.
             * 
             * "Nether Scion"
             * This object can be triggered at any time to spawn a Nether Scion with 6million???(10m)/12.7million(25m) health. This add can then be killed to remove the Frenzied Assault
             *      buff from Halfus Wyrmbreaker and gives Halfus a debuff that increases his damage taken. It's melee attacks are slow, but very hard, roughly 30-50k???(10m)/40-70k(25m) after mitigation.
             *      
             * Aura of Nether Blindness - Triggers when this add is spawned and reduces raid members' chance to hit, damage done, and attack speed by 25% until the Nether Scion is killed.
             * 
             * "Time Warden"
             * Another possible add.
             * 
             * Cyclonic Aura - Pulses an aura that interupts spell casts at a 0.75 second interval.
             * 
             * "Slate Dragon"
             * Another possible add.
             * (??? - When killed, gives a chance to stun Halfus???)
             * 
             * "Stone Drake"
             * Another possible add.
             * 
             * Malevolent Strikes (10) / (25)- Halfus gains an ability that allows each attack to reduce the target's healing received by 3%(10)/5%(25) with a 15 second duration and this ability stacks.
             * 
             * (Aura of Stone - Pulses an aura at a 25 second interval that has a chance to cause raid members to gain Paralysis, making them unable to move or attack for 10 seconds???)
             */
        }
    }

    public class TwilightAscendantCouncil : MultiDiffBoss
    {
        public TwilightAscendantCouncil()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Twilight Ascendant Council";
            Instance = "The Bastion of Twilight";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Arion = 4,724,000 / 14,600,000 / 6,613,600 (Guess) / 20,440,000 (Guess)
            // Terrastra = 4,724,000 / 14,600,000 / 6,613,600 (Guess) / 20,440,000 (Guess)
            // Ignacious = 6,871,000 / 21,900,000 / 9,619,400 (Guess) / 30,660,000 (Guess)
            // Feludious = 6,871,000 / 21,900,000 / 9,619,400 (Guess) / 30,660,000 (Guess)
            // Elemental Monstrocity = Total Health between all four his "parts."
            // His current health going into P3 is whatever is the remainder of what is currently on all mobs put together
            // So for P3, it's best to keep everyone at or close to 25% as possible going into each phase.
            Health = new float[] { ((4724000f * 2f) + (6871000f * 2f)), ((14600000f * 2f) + (21900000f * 2f)), ((6613600f * 2f) + (9619400f * 2f)), ((20440000f * 2f) + (30660000f * 2f)) };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             * "Ignacious, the fire ascendant"
             * Melee - Physical hits after mitigation can range anywhere from 15k-20k(10m)/25k-36k(25m), but can be increased via his Rising Flames buff.
             * 
             * Aegis of Flame (10) / (25) - This ability surrounds Ignacious in a protective shield that absorbs 300k(10m)/1million(25m) damage over the next 20 seconds.
             *      When this shield is applied, he will begin to cast Rising Flames.
             *      
             * Rising Flames - This 20 second channel pulses every 2 second, causing Rising Flames (10) / (25) which deals 5k(10m)/7k(25m) fire damage to everyone in the raid,
             *      however, it can and should be interrupted. Additionally, each gives Ignacious a stacking buff that increases his damage by 5% per stack for 15 seconds.
             *      The rate of application increases with each tick of damage.
             *      (Example: First tick grants 1 stack for a total of 5% damage increase. Second tick applies additional 2 stacks for a total of 3 stacks and 15% damage increase.
             *      Third tick applies additional stacks for a total of 6 stacks and 30% damage increase. The fourth tick applies 4 additional stacks for a total of 10 stacks and
             *      50% damage increase. And so on.)
             *      Tenative timer is 60 seconds.
             *      
             * Flame Torrent (10) / (25) - Used occasionally in the direction that he is facing, Ignacious will deal 20k(10m)/30k(25m) fire damage every second for 3 seconds to
             *      targets in a cone for 18 yards.
             *      
             * Inferno Leap (10) / (25) - Ignacious will leap through the air to the location of a random raid member, causing a 10 yard knock back to all targets within 10 yards
             *      of the impact point and dealing 15k(10m)/25k(25m) fire damage as well.
             *      
             * Inferno Rush - After landing from a Inferno Leap, Ignacious will charge back to the top target on his threat list, leaving a trail of fire behind. This trail of
             *      Inferno Rush fire (10) / (25) deals 5k(10m)/15k(25m) fire damage every 0.5 seconds to players within 3 yards.
             *      
             * Burning Blood - This debuff is causes the target to deal 2k fire damage every 2 seconds to all players within 10 yards, as well as causing them to become Flame Imbued
             *      which increases their damage against Feludius by causing physical attacks and spells to become empowered with fire. The empowerment lasts 30 seconds.
             *      Tenative timer is 30 seconds.
             *      
             * "Feludius, the water ascendant"
             * Melee - Physical hits after mitigation can range anywhere from 15k-20k(10m)/25k-36k(25m).
             * 
             * Glaciate (10) / (25) - Feludius will use this ability after a 3 second cast time. It will deal 200k(10m)/500k(25m) frost damage, however, the damage done decreases with
             *      distance from Feludius so every should attempt to be as far as possible to minimize damage. Additionally, players who had the Waterlogged debuff will be
             *      Frozen (10) / (25) stunning them and dealing 10k(10m)/20k(25m) frost damage every 2 seconds for 10 seconds.
             *      Tenative CD is 32 seconds
             *      
             * Water Bomb - This ability is used by Feludius and causes 10k damage to all players within 10 yards of where they land, as well as debuffing them with Waterlogged.
             *      Tenative cd is 32 seconds.
             * 
             * Waterlogged - This debuff is placed on raid members that are hit by Water Bombs. It reduces movement speed by 20% and causes vulnerability to Glaciate for 45 seconds,
             *      however, the flames of Ignacious' fire abilites will remove this debuff.
             *      
             * Hydro Lance (10) / (25) - This ability is used on a random raid member, dealing 50k(10m)/90k(25m) frost damage.
             * 
             * Heart of Ice - This debuff is causes the target to deal 2k frost damage every 2 seconds to all players within 10 yards, as well as causing them to become Frost Imbued
             *      which increases their damage against Ignacious by causing physical attacks and spells to become empowered with frost. The empowerment lasts 30 seconds.
             *      Tenative cd is 20 seconds.
             *      
             * "Arion, the wind ascendant"
             * Melee - Physical hits after mitigation can range anywhere from 15k-20k(10m)/25k-36k(25m).
             * 
             * Thundershock - Causes 80k nature damage to all players, however this damage is reduced if they are grounded. (150k damage on heroic)
             * 
             * Call Winds - A 1.5 second cast that summons several Lashing Winds (10) / (25) which have a cause a 10 yard knock back and 3k(10m)/5k(25m) nature damage to players within
             *      7 yards of them. They will then also debuff those players with Swirling Winds which causes a levitation effect for 2 minutes that is crucial to avoid maximum damage
             *      from Terrastra's Quake. This Swirling Winds debuff is removed by touching a Gravity Well and becoming Grounded.
             *      Tenative timer is 27 seconds.
             *      
             * Disperse - This ability will teleport Arion to another location in the room and will cause him to begin casting Lightning Blast.
             *      Tenative Timer is 30 seconds.
             * 
             * Lightning Blast - A 4 second cast used after Disperse which deals 80k damage to the highest target on Arion's treat table. (120k in heroic)
             * 
             * Lightning Rod - Arion places this debuff on 1(10m)/3(25m) target/s which lasts 15 seconds. After 10 second of initially becoming debuffed, Arion will cast Chain Lightning,
             *      which is a 2.5 second cast time spell that does 10k nature damage to each Lightning Rod and jumping to other players near them.
             *      Tenative timer is 15 seconds.
             *      
             * "Terrastra, the earth ascendant"
             * Melee - Physical hits after mitigation can range anywhere from 15k-20k(10m)/25k-36k(25m), but can be doubled via his Harden Skin buff.
             * 
             * Quake - This 3 second cast will do massive damage to anyone without the Swirling Winds buff from Arion's Lashing Winds. It deals 65k physical damage and cannot be avoided
             *      via jumping or a priest's Levitate. (150k in heroic)
             *      
             * Gravity Well - Spawns a sinkhole with a 20 second duration. This Gravity Well has a 7 yard radius and if a player gets near, it will deal 3k nature damage and will attempt
             *      to pull players toward it core with Magnetic Pull, as well as applying the Grounded debuff which is crucial to negate the damage of Arion's Thundershock for it's
             *      2 minute duration unless they are picked up by Swirling Winds which will remove Grounded.
             *      Tenative Timer is 17 seconds.
             *      
             * Gravity Core - Being sucked into the epicenter of a Gravity Well will cause the player to take 10k physical damage every 2 seconds and additionally they will be debuffed
             *      with Gravity Core which lasts 10 seconds and will slow movement speed of all nearby raid member by 50% and reduce attack and casting speed by 100%.
             *      
             * Eruption - A rumbling patch of earth will precursor this ability which will then impale all players within 4 yards of it's location, knocking them into the air by 12 yard
             *      and dealing 25k physical damage. (50k in heroic)
             *      
             * Harden Skin (10) / (25) - This self-buff of Terrastra is a 1 second cast that can be interrupted. If it is not interrupted, it will increase Terrastra's damage by 100% for
             *      30 seconds and will cause him to absorb 50% of all damage taken, up to a maximum of 500k(10m)/1.65million(25m) damage. It the maximum damage is absorbed, his Harden
             *      Skin barrier will break, causing him to Shatter and take massive (amount???) physical damage.
             *      
             * "Elementium Monstrosity"
             * Melee - Physical hits after mitigation can range anywhere from 20k-30k(10m)/30k-45k(25m).
             * 
             * Elemental Stasis - When either Arion or Terrastra reaches 25% health remaining, they will both despawn and all raid members will be stunned for 17 seconds and damage to them
             *      will be reduced by 99%. During this time, the four Twilight Ascendants will combine to form the Elementium Monstrosity.
             *      
             * Lava Seed - After a 2 second cast, these will be tossed all around the room approximately 10 yards apart. After (2-4???) more seconds, the seeds will explode into a Lava Plume
             *      which deals 40k damage to all raid members within 4 yards. (80k in heroic)
             *      
             * Liquid Ice - The Elementium Monstrosity is frigid and every 2.5 seconds, will form a patch of Liquid Ice underneath him which will grow larger if the Elementium Monstrosity is
             *      not moved out of it. If a player steps in it, it will deal 5k damage per second. (10k in heroic)
             *      
             * Electric Instability (10) / (25) - Throughout all of the final phase of the encounter, the Elementium Monstrosity will use this on raid members (at increasing frequency/targets???)
             *      and deal 5k(10m)/6k(25m) nature damage and will jump to other players within (6-8???) yards.
             *      
             * Gravity Crush - The Elementium Monstrosity will lift 1(10m)/3(25m) players roughly 35 yards into the air and cause the to be stunned and lose 5% of their maximum health every
             *      0.5 seconds for 6.5 seconds. After the effect wears off, those players will be dropped and will take additional fall damage. (By 8%(10m)/10%(25m) in heroic)
             */
        }
    }

    public class Chogall : MultiDiffBoss
    {
        public Chogall()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Cho'gall";
            Instance = "The Bastion of Twilight";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 33497000f, 101400000f, 46895800f, 141960000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             * "Corrupted Blood"
             * Corrupted Blood - Your blood is corrupted! Increases damage taken from Corruption of the Old God by 3%.
             * 
             * Dark Sickness - Your Corrupted Blood has made you sick, causing you to vomit and inflict 19500 to 20500 (39000 to 41000) Shadow damage to friends in a 5 yard cone
             *      in front of you, also causing Corrupted Blood.
             * 
             * Accelerated Corruption - At 25% Corrupted Blood you are afflicted with Corruption: Accelerated. You are becoming more corrupt by the second!
             * 
             * Absolute Corruption - At 100% Corrupted Blood you are afflicted with Corruption: Absolute. You receive 100% less from heals but inflict 100% additional damage and spells are cast instantly.
             * 
             * Summon Corrupting Adherent - Cho'gall summons Corrupted Adherents from the nearby portals to aid him!
             *      Tenative timer is 90 seconds.
             * 
             * "Cho'gall"
             * Melee - placeholder
             * 
             * Conversion - Cho'gall beckons to the weak of mind causing the target to channel the Worshipping spell on him.  Worshipping Cho'gall causes him to gain Twisted Devotion every 3 sec. Worshipping can be interrupted.
             * 
             * Twisted Devotion - The Worshipping has caused Cho'gall to become more zealous, increasing damage done by 10% for 20 sec.
             * 
             * Flame's Orders - Cho'gall Orders the Flame to come to his aid.
             * 
             * Flaming Destruction - Attacks cause a Flaming Destruction, inflicting 29250 to 30750 (39000 to 41000) additional Fire damage and causes a Blaze at a nearby location.
             * 
             * Blaze - Summons a Blaze that inflicts 9750 to 10250 (12675 to 13325) (19500 to 20500) Fire damage to enemies within 0 yards.
             * 
             * Shadow's Orders - Cho'gall Orders the Shadow to come to his aid.
             * 
             * Dark Sludge - placeholder
             * 
             * Dark Sludge - placeholder
             * 
             * Fury of Cho'gall - Cho'gall blasts the target, inflicting massive Shadow and Physical damage and increasing damage taken by Physical and Shadow.
             * 
             * Fury of Cho'gall - Cho'gall blasts the target, inflicting massive Shadow and Physical damage and increasing damage taken by Physical and Shadow.
             *      Applies both Cho's Blast and Gall's Blast
             * 
             * Cho's Blast - Cho blasts the target for 19500 to 20500 (29250 to 30750) (34125 to 35875) Physical damage, and increases Physical damage taken by 20%.
             * 
             * Gall's Blast - Gall blasts the target for 19500 to 20500 (29250 to 30750) (34125 to 35875) Shadow damage, and increases Shadow damage taken by 20%.
             * 
             * Fester Blood - Festers the blood of any Corrupting Adherents, causing Blood of the Old God to form from congealed blood and boils the blood of living Corrupting Adherents.
             *      Tenative timer is 90 seconds.
             * 
             * "Corrupting Adherent"
             * Melee - placeholder
             * 
             * Depravity - Inflicts 24375 to 25625 (39000 to 41000) Shadow damage to enemies within 0 yards and causes Corruption.
             * 
             * Corrupting Crash - Fires a shadow missile at a target, dealing 43875 to 46125 (73125 to 76875) Shadow damage to all enemies near the impact.  Targets hit by the Corrupting Crash have corruption applied to them.
             * 
             * Festering Blood - The Festered Blood is causing the Corrupting Adherent to bleed out and cause Sprayed Corruption.  Inflicts 34125 to 35875 (53625 to 56375) Shadow damage every 2 sec and causes Corrupted Blood.
             * 
             * Sprayed Corruption - Triggered by Festering Blood; Inflicts 34125 to 35875 (53625 to 56375) Shadow damage and applies Corrupted Blood.
             * 
             * Spilled Blood of the Old God - The slain Adherent to the old god spills its corrupted blood.  Inflicts 14625 to 15375 (21937 to 23062) Shadow damage to enemies within 0 yards and applies Corrupted Blood.
             * 
             * "Blood of the Old Gods"
             * Melee - placeholder
             * 
             * "Consume Blood of the Old God"
             * Consume Blood of the Old God - The Blood of the Old God is being consumed by Cho'gall, corrupting him even further!
             * 
             * Melee - placeholder
             * 
             * Corruption of the Old Gods - Cho'gall becomes completely corrupted by the Old Gods, inflicting 4875 to 5125 (6337 to 6662) Shadow damage every 2 sec. Causes Corrupted Blood.
             * 
             * Darkened Creations - Cho'gall summons Darkened Creations to aid him! Summons 8 in P2
             * 
             * "Darkened Creation"
             * Debilitating Beam - The Debilitating Beam reduces healing and damage done by 75% and inflicts 7800 to 8200 (10725 to 11275) Shadow damage every 1 sec for 10 sec. 
             */
        }
    }

    public class LadySinestra : MultiDiffBoss
    {
        public LadySinestra()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Lady Sinestra";
            Instance = "The Bastion of Twilight";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 46895800f, 141960000f, 0f, 0f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 3, 5, 0, 0 };
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
             */
        }
    }

    #endregion

    #region Throne of the Four Winds
    // ===== Throne of the Four Winds =========================
    public class ConclaveOfWind : MultiDiffBoss
    {
        public ConclaveOfWind()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Conclave Of Wind";
            Instance = "Throne of the Four Winds";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            // Rohash = 4,120,000 / 14,429,856 / 5,768,000 / 20,201,799
            // Anshal = 4,120,000 / 14,429,856 / 5,768,000 / 20,201,799
            // Nezir = 7,210,000 / 25,252,248 / 10,094,000 / 35,353,148
            Health = new float[] { (4120000f + 4120000f + 7210000f), (14429856f + 14429856f + 25252248f), (5768000f + 5768000f + 10094000f), (20201799f + 20201799f + 35353148f) };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             * "Nezir, Djinn of the North Wind"
             * Melee - 13k-22k(10m) / 22k-34k(25m) after mitigation at an average swing speed.
             * 
             * Wind Chill (10) / (25) - This AoE ability is pulsed on everyone on the North platform, dealing 4750 to 5250(10m)/7125 to 7875(25m) frost damage
             *      and a stack of a debuff increasing frost damage taken by 10% for 30 seconds.
             *      Tenative Timer is 11 seconds
             *      
             * Ice Patch - Nezir will create this on a large portion of the North platform. It causes anyone that stands on it to gain 1 stack
             *      every second of Ice Patch (10) / (25) which has a 3 second duration even after leaving the patch. Each stack slows movement
             *      speed by 10% and causes 4750 to 5250(10m)/7125 to 7875(25m)/9500 to 10500(25h) frost damage per second.
             *      
             * Permafrost (10) / (25) - This ability is cast on the current tank and deals 9500 to 10500(10m)/14250 to 15750(25m)/19000 to 21000(25h) frost damage per second in a 90%
             *      cone in front of Nezir for 3 seconds.
             *      Cast Timer = 2.5 seconds
             *      
             * Sleet Storm (10) / (25) - This is Nezir's full energy special attack. Used when at 90 energy, this spell is channeled for 15 seconds,
             *      during which, Nezir will deal 28500 to 31500(10m)/ 57000 to 63000(25m)/ 85500 to 94500(25h) frost damage per second, but the damage is divided between all raid member's on
             *      the North platform.
             *      
             * Chilling Winds (10) / (25) - Nezir's penalty ability, used if no one is on his platform for over 1 second. This debuff aura will be
             *      applied to everyone in the raid and will reduce haste by 500% and deal increasing frost damage, starting at 500(10m)/2000(25m).
             *      
             * "Rohash, Djinn of the East Wind"
             * Melee - Does not use melee attacks so a traditional tank is not required.
             * 
             * Slicing Gale (10) / (25) - This is used on a random player on the East platform near Rohash. Each hit deals 11875 to 13125(10m)/23750 to 26250(25m) nature
             *      damage and leaves a debuff that increases nature damage taken by 5% for the next 30 seconds and stacks.
             *      Tenative Timer is 30 seconds.
             *      
             * Wind Blast (10) / (25) - This is a 90% frontal cone attack that is channeled for 10 seconds as Rohash slowly spins in the center of
             *      his platform, dealing 9500 to 10500(10m)/ 23750 to 26250(25m)/ 19000 to 21000(10h)/ 47500 to 52500(25h) nature damage and a 15 yard knockback.
             *      
             * Summon Tornados - Rohash will create 3 Tornado (10) / (25) that whirl around his platform. These each have a 15 yard radius and will
             *      inflict 57000 to 63000(10m)/95000 to 105000(25m) nature damage and cause a 15 yard knock back.
             *      
             * Hurricane (10) / (25) - This is Rohash's full energy special attack. Used when at 90 energy, this spell is channeled for 15 seconds,
             *      during which, Rohash will lift anyone on his platform into the air and deal 2500(10m)/5000(25m)/3500(10h)/7500(25h) nature damage per second to them,
             *      and then drop them from the air, dealing fall damage.
             *      
             * Deafening Winds (10) / (25) - Rohash's penalty ability, used if no one is on his platform for over 1 second. This debuff aura will be
             *      applied to everyone in the raid and will silence and deal increasing nature damage, starting at 500(10m)/2000(25m).
             *      
             * "Anshal, Djinn of the West Wind"
             * Melee - 13k-22k(10m) / 22k-34k(25m) after mitigation at an average swing speed.
             * 
             * Soothing Breeze (10) / (25) - Places a green circle under Anshall with a 10 yard radius that lasts for 30 seconds. Raid members are
             *      silenced and unable to attack with physical attacks while in the circle, and Anshal and any adds that are within the aura are healed
             *      for 20000(10m)/40000(25m)/80000(25h) per second.
             *      
             * Nurture - 5 second channeled spell that summoned a seed every second. Shortly after these seeds spawn into Ravenous Creepers (Plant
             *      Lashers), which each melee for 4-7k(10m)/6k-9k(25m) after mitigaton. They also use an AoE attack in melee, Toxic Spores (10) / (25),
             *      and this attack can quickly stack to it's maximum of 25 stacks on everyone within melee range of the Ravenous Creepers at which
             *      point it will do significant damage. The debuff only has a 5 second duration, unless refreshed, but deals 500(10m)/1000(25m)/5000(25h) nature
             *      damage per stack every second.
             *      
             * Zephyr (10) / (25) - This is Anshal's full energy special attack. Used when at 90 energy, this spell is channeled for 15 seconds, during
             *      which, Anshal heals himself and any other adds left up on his platform for 25000(10m)/50000(25m)/100000(25h) per second, which equates to a total of
             *      375k(10m)/750k(25)/1,500k(25h) health, and they all get a stacking 15% damage buff which will then last for 15 seconds after the channel has ended,
             *      during which Anshal will have a total of 225% increased damage.
             *      
             * Withering Winds (10) / (25) - Anshal's penalty ability, used if no one is on his platform for over 1 second. This debuff aura will be applied
             *      to everyone in the raid and will reduce all healing received by 100% and deal increasing shadow damage, starting at 500(10m)/2000(25m).
             */
        }
    }

    public class AlAkir : MultiDiffBoss
    {
        public AlAkir()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Al'Akir";
            Instance = "Throne of the Four Winds";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_0, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_5, BossHandler.TierLevels.T11_9, };
            Version = new BossHandler.Versions[] { BossHandler.Versions.V_10N, BossHandler.Versions.V_25N, BossHandler.Versions.V_10H, BossHandler.Versions.V_25H, };
            #endregion
            #region Basics
            Health = new float[] { 30100000f, 105200000f, 42140000f, 147280000f };
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
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
             * "Fair Weather Phase"
             * Melee - Al'Akir's physical damage is moderate and pretty consistant with hits ranging from 20k-30k(10m)/40k-55k(25m) after mitigation.
             * 
             * Electrocute - This ability is used on the highest player on Al'Akir's threat table if that player is not in melee range. It is channeled
             *      and deals increasing nature damage over time. This may happen from knock backs via Wind Bursts in P1 or due to having to run for the
             *      opening in the Squall Line in P2.
             *      
             * Static Shock - A debuff placed on all players within (5-8???) yards of Al'Akir used approximately every 15 seconds. This debuff will then
             *      deal 1k nature damage and interrupt any spell being cast at every 1 second interval for 5 seconds.
             *      
             * Ice Storm - A slow moving cloud travels along Al'Akir's platform roughty 15 yards away from him just outside of his melee range. The falling
             *      Ice Storm deals 7500(10m)/15000(25m) frost damage per second and leaves a 50% movement speed reduction debuff on any player that get within 5 yards of
             *      it's current location or the path of ice it leaves behind.
             *      
             * Wind Burst - This 5 second cast time spell happens after an emote and deals 28275 to 31725(10m)/38000 to 42000(25m)/42412 to 47587(10h)/57000 to 63000(25h)
             *      nature damage and causes a 15 yard knock back to everyone in the room. It can knock off ranged DPS near the outer edge of Al'Akir's platform.
             *      
             * Lightning Strike - This ability is only used in the first phase and deals 9500 to 10500(10m)/19000 to 21000(25m) nature damage in a wide cone
             *      towards a random player, making it beneficial to not have the entire raid stacked in the same location.
             *      (*Note: This was working in some attempts, not sure if it is still in the encounter or being reworked)
             *      
             * "Bad Weather Phase"
             * Melee - Al'Akir's physical damage is moderate and pretty consistant with hits ranging from 20k-30k(10m)/40k-55k(25m) after mitigation.
             * 
             * Electrocute - This ability is used on the highest player on Al'Akir's threat table if that player is not in melee range. It is channeled and
             *      deals increasing nature damage over time. This may happen from knock backs via Wind Bursts in P1 or due to having to run for the opening
             *      in the Squall Line in P2.
             *      
             * Static Shock - A debuff placed on all players within (5-8???) yards of Al'Akir used approximately every 15 seconds. This debuff will then deal
             *      1k nature damage and interrupt any spell being cast at every 1 second interval for 5 seconds.
             *      
             * Squall Line (10) / (25) - Several tornadoes form a line radiating our from the inner to the outer edge of Al'Akir's platform. This wall will
             *      only have one open gap (similar to Sartharion's Flame Wall ability) which will allow players to not be hit by the Squall Line as it passes
             *      clockwise or counterclockwise along the platform. Getting hit will cause 10000(10m)/20000(25m)/40000(25h) nature damage and temporary loss
             *      of control while stunned.
             *      (*Note: In addition to the large obvious gap in the Squall Line, ranged DPS and healers that are standing on the very furthest points of the 
             *      platforrm's outside edge will also avoid the damage.)
             *      
             * Acid Rain - This debuff has a 17 second duriation, allowing it just enough time to keep refreshing and stacking every 20 seconds on every raid
             *      member for all of the second phase of the encounter. Each stack will deal 500(10m)/750(25m) nature damage per second.
             *      
             * Stormling - Al'Akir summons a Stormling lightning elemental that has 258k(10m)/901866(25m) health which melee for 2.5k(10m)/4000 to 4500(25m) physical damage
             *      as well as constantly pulse Stormling (10) / (25) which deals an additional 2375 to 2625(10m)/4750 to 5250(25m) nature damage players within (6???) yards.
             *      (heroic also 25% nature damage debuff)
             *      
             * Feedback - Whenever a Stormling dies, they will grant Al'Ashkir one stack of this debuff which increases his damage taken by 10% for 20 seconds.
             * 
             * "Electric Storm Phase"
             * Eye of the Storm - This buff is placed on all raid members in final phase of the fight. It grants flight and increases movement speed by 300%,
             *      however, removes the ability block, dodge, or parry.
             *      
             * Relentless Storm - Any player that is too far from Al'Akir will be temporarily stunned for 10 seconds as they are tossed around and launched back
             *      closer into the room.
             *      
             * Wind Burst - In the last phase, this ability no longer has a cast time and instead is much more dangerous being instant, dealing 50k nature damage,
             *      and causing a 10 yard knock back.
             *      
             * Chain Lightning - This is used very often by Al'Akir in the last phase, roughly every 2 seconds. It deals 14250 to 15750(10m)/28500 to 31500(25m)/20900 to
             *      23100(10h)/42750 to 47250(25h) nature damage to a random target and then jumping to new targets that are within (6-8???) yards, with each jump increasing
             *      the damage dealt by 30%.
             *      
             * Lightning Rod (10) / (25) - This ability is placed on 1 player at a time the last phase. A player will get this debuff and then, 5 seconds later,
             *      will begin to pulse and AoE for 5 seconds that deal 4750 to 5250(10m)/9500 to 10500(25m)/7125 to 7875(10h)/14250 to 15750(25h) nature damage every
             *      second to all players within 20 yards horizontally, and/or 5 yards vertically.
             *      
             * Lightning Cloud - This spawns a formation of clouds covering the entire altitude of where a randomly targeted raid member was located. After 5 seconds,
             *      those clouds will inflict 23750 to 26250(10m)/47500 to 52500(25m) nature damage to nearby players at the same altitude every second. The cloud formation will
             *      despawn after 30 seconds.
             */
        }
    }

    #endregion
}