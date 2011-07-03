using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Bosses
{
    #region Baradin Hold
    // ===== Baradin Hold ====================
    public class Occuthar : MultiDiffBoss
    {
        public Occuthar()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Occu'thar";
            Instance = "Baradin Hold";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H };
            #endregion
            #region Basics
            Health = new float[] { 53946000f, 80900000f, 0, 0 };
            MobType = (int)MOB_TYPES.DEMON;
            // 5 minute Berserk timer
            BerserkTimer = new int[] { 5 * 60, 5 * 60, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0 };
            InBackPerc_Melee = new double[] { 0.60f, 0.60f, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0 };
            Min_Healers = new int[] { 3, 5, 0, 0 };
            #endregion
            #region The Phases
            for (int i = 0; i < 2; i++)
            {
                Phase EntireFight = new Phase() { Name = "Entire Fight" };

                #region MT and OT Melee Swapping
                // MT and OT tank swap
                // Each should take half of the total damage
                // does not melee during Firestorm
                EntireFight.Attacks.Add(new Attack
                {
                    Name = "MT Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    IsTheDefaultMelee = true,
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2.0f,
                });
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.MainTank] = true;

                EntireFight.Attacks.Add(new Attack
                {
                    Name = "OT Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    IsTheDefaultMelee = true,
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2.0f,
                });
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.OffTank] = true;
                #endregion

                #region Searing Shadows
                // Occu'thar inflicts 102374 to 107625 Shadow damage to players in a 60 degree cone in front of
                // him, and increases Shadow damage taken by 100% for 30 sec.
                /* 10 man - http://ptr.wowhead.com/spell=96913
                   25 man - http://ptr.wowhead.com/spell=101007 */
                // Tanks switch every 15 seconds so that each one does not get hit with with more than one stack.
                EntireFight.Attacks.Add(new Attack
                {
                    Name = "Searing Shadows [Main Tank]",
                    SpellID = new float[] { 96913, 101007, 0, 0 }[i],
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = ( 102375f + 107625f) / 2f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 22f,
                });
                EntireFight.LastAttack.SetUnavoidable();
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.MainTank] = true;

                EntireFight.Attacks.Add(new Attack
                {
                    Name = "Searing Shadows [Off Tank]",
                    SpellID = new float[] { 96913, 101007, 0, 0 }[i],
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = (102375f + 107625f) / 2f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 22f,
                });
                EntireFight.LastAttack.SetUnavoidable();
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.OffTank] = true;
                #endregion

                #region Focused Fire
                // Occu'thar sets his gaze on a random location, inflicting 34125 to 35875 Fire damage every 1
                // seconds to players within 12 yards.
                /* 10 man - http://ptr.wowhead.com/spell=96883
                   25 man - http://ptr.wowhead.com/spell=101004 */
                // Raid spreads out so that it's less likely for more than 2-3 people get hit by this. Assume 1
                // person is hit at this time and it takes 2 seconds to get out. Location lasts for 20 seconds as per 
                // this spell: http://ptr.wowhead.com/spell=96882
                EntireFight.Attacks.Add(new Attack
                {
                    Name = "Focused Fire",
                    SpellID = new float[] { 96883, 101004, 0, 0 }[i],
                    IsDoT = true,
                    // From videos watched, it only appeares to affect range dps and healers.
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamageType = ItemDamageType.Shadow,
                    // Assume it takes 2 seconds to move out.
                    Duration = 2f, //15f,
                    TickInterval = 1f,
                    DamagePerTick = (34125f + 35875f) / 2f,
                    // Need verification on how many are hit.
                    MaxNumTargets = new float[] { 3, 3 }[i],
                    // Initial release, one video I saw showed 15 seconds between each cast with the zone lasting
                    // about 5 seconds into the next zone's time frame.
                    AttackSpeed = 15.5f,
                });
                EntireFight.LastAttack.SetUnavoidable();
                EntireFight.LastAttack.SetAffectsRoles_DPS();
                EntireFight.LastAttack.SetAffectsRoles_Healers();
                EntireFight.Moves.Add(new Impedance()
                {
                    Name = "Focused Fire (Get Out)",
                    Frequency = EntireFight.LastAttack.AttackSpeed,
                    Duration = EntireFight.LastAttack.Duration * 1000f,
                    Chance = EntireFight.LastAttack.MaxNumTargets / (Max_Players[i] - Min_Tanks[i]),
                    Breakable = true, // Movement is always breakable
                    AffectsRole = EntireFight.LastAttack.AffectsRole,
                });
                #endregion

                #region Eyes of Occu'thar
                // Occu'thar forms an Eye of Occu'thar for each player which will bore into them for 10 sec or 
                // until killed. (these are considered adds)
                // 10 man - http://db.mmo-champion.com/c/52389/eye-of-occuthar/
                // 25 man - http://db.mmo-champion.com/c/52428/eye-of-occuthar/
                EntireFight.Targets.Add(new TargetGroup
                {
                    Name = "Eyes of Occu'thar",
                    TargetID = new float[] { 52389, 52428, 0, 0 }[i],
                    // Initial release, assume 15 seconds
                    Frequency = 60f,
                    // Need verification on how many are released in 25 man
                    // Does not appear to target tanks
                    NumTargs = new float[] { 5, 20, 0, 0 }[i],
                    Chance = new float[] { 5, 20, 0, 0 }[i] / Max_Players[i],
                    Duration = 10f * 1000f,
                    LevelOfTargets = 87,
                    NearBoss = true,
                });
                EntireFight.LastTarget.SetAffectsRoles_DPS();
                EntireFight.LastTarget.SetAffectsRoles_Healers();

                // Assume 2 seconds to clump up together to AOE down the adds, the 2 seconds to run back out after all have been killed
                EntireFight.Moves.Add(new Impedance
                {
                    Name = "Eyes of Occu'thar",
                    Chance = EntireFight.LastTarget.Chance,
                    Duration = 4f * 1000f,
                    Frequency = EntireFight.LastTarget.Frequency,
                });

                /* Gaze of Occu'thar
                 * The eye of Occu'thar fires a beam at its target. Inflicting 5850 to 6150 Shadow damage every
                 * 1 sec for 10 sec.
                 * After 10 sec, the Eye of Occu'thar is able to fully bore into the target and causes Occu'thar's
                 * Destruction, inflicting 24375 to 25625 Shadow damage to every player.*/
                // 10 man - http://ptr.wowhead.com/spell=96920
                // 25 man - http://ptr.wowhead.com/spell=101006
                EntireFight.Attacks.Add(new Attack
                {
                    Name = "Gaze of Occu'thar",
                    SpellID = new float[] { 96920, 101006, 0, 0 }[i],
                    IsDoT = true,
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    // Assume it takes 9 of the 10 seconds to kill the mobs
                    Duration = 9f,
                    TickInterval = 1f,
                    DamagePerTick = (5850f + 6150f ) / 2f,
                    MaxNumTargets = EntireFight.LastTarget.NumTargs,
                    AttackSpeed = EntireFight.LastTarget.Frequency,
                    IsFromAnAdd = true,
                });

                /* Occu'thar's Destruction
                 * After 10 sec, the Eye of Occu'thar is able to fully bore into the target and causes Occu'thar's
                 * Destruction, inflicting 24375 to 25625 Shadow damage to every player.*/
                // 10 man - http://ptr.wowhead.com/spell=96968
                // 25 man - http://ptr.wowhead.com/spell=101008
                EntireFight.Attacks.Add(new Attack
                {
                    Name = "Occu'thar's Destruction",
                    SpellID = new float[] { 96968, 101008, 0, 0 }[i],
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = ((24375f + 25625f) / 2f) * EntireFight.LastTarget.NumTargs,
                    MaxNumTargets = Max_Players[i],
                    AttackSpeed = EntireFight.LastTarget.Frequency,
                    Interruptable = true,
                    IsFromAnAdd = true,
                });
                #endregion

                #region Apply Phases
                int phaseStartTime = 0;
                ApplyAPhasesValues(ref EntireFight, i, 1, phaseStartTime, this[i].BerserkTimer, this[i].BerserkTimer); phaseStartTime += this[i].BerserkTimer;
                AddAPhase(EntireFight, i);
                #endregion
            }
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0 };
        }
    }
    #endregion

    #region Firelands
    // ===== Firelands ======================
    public class Bethtilac : MultiDiffBoss
    {
        public Bethtilac()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Beth'tilac";
            Instance = "Firelands";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, };
            #endregion
            #region Basics
            Health = new float[] { 20871756f, 62615268f, 29220458f, 87661375f };
            MobType = (int)MOB_TYPES.BEAST;
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
            for (int i = 0; i < 4; i++)
            {

                Phase TheCinderweb = new Phase() { Name = "The Cinderweb" };
                Phase TheFrenzy = new Phase() { Name = "The Frenzy!" };
                
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                #region The Cinderweb
                // Beth'tilac retreats to her web at the beginning of the battle. Beneath her web
                // scurry her brood.

                /* Fire Energy
                 * Beth'tilac will slowly lose Fire Energy over time. In addition, Cinderweb Drones will siphon
                 * some of her energy when they are depleted. Whenever Beth'tilac runs out of Fire Energy, she will
                 * set herself ablaze, causing Smoldering Devastation. */

                /* Ember Flare
                 * Intense heat burns enemies near Beth'tilac dealing 18500 to 21500 Fire damage to those on the
                 * same side of the web as she is. */
                // 8 different ids for this, using the "tighter" band of damage for now but listing the alt id beside
                // it until further notice.
                // 10 man - http://ptr.wowhead.com/spell=98934; alt - http://ptr.wowhead.com/spell=99859
                // 25 man - http://ptr.wowhead.com/spell=100648; alt - http://ptr.wowhead.com/spell=100649
                // 10 man heroic - http://ptr.wowhead.com/spell=100834; alt - http://ptr.wowhead.com/spell=100935
                // 25 man heroic - http://ptr.wowhead.com/spell=100835; alt - http://ptr.wowhead.com/spell=100936

                /* Meteor Burn
                 * Meteors crash down onto the web, dealing 37000 to 43000 Fire damage to those who stand beneath
                 * them. Additionally, they burn a hole in the web through which players may fall. */
                // http://ptr.wowhead.com/spell=99076

                /* Consume
                 * Beth'tilac consumes Cinderweb Spiderlings healing for 10% of her life. */
                // 2 spell ids with similar wording, providing both
                // http://ptr.wowhead.com/spell=99332; alt - http://ptr.wowhead.com/spell=99857

                /* Smoldering Devastation
                 * When Beth'tilac is depleted of Fire Energy she will set herself ablaze, obliterating those who
                 * are not shielded by her web. */
                // http://ptr.wowhead.com/spell=99052

                #region Cinderweb Spinner
                /* These spiders dangle from the web above. Using Taunt or a similar ability on them will cause
                 * them to drop to the ground. Once killed, their filaments remain allowing players to climb up
                 * to the Cinderweb. */
                // http://ptr.wowhead.com/npc=52981; Beast

                /* Burning Acid
                 * The Cinderweb Spinner spits burning venom at a random enemy, dealing 19016 to 21316 Fire damage. */
                // 12 different version with layering damage ranges posting the most probable with alts
                // 10 man - http://ptr.wowhead.com/spell=99934; alt - http://ptr.wowhead.com/spell=98471
                // 25 man - http://ptr.wowhead.com/spell=100829; alt - http://ptr.wowhead.com/spell=100826
                // 10 man heroic - http://ptr.wowhead.com/spell=100831; alt - http://ptr.wowhead.com/spell=100828
                // 25 man heroic - http://ptr.wowhead.com/spell=100125
                /* Others
                 * http://ptr.wowhead.com/spell=100830; http://ptr.wowhead.com/spell=100827
                 * http://ptr.wowhead.com/spell=99647; http://ptr.wowhead.com/spell=99974; http://ptr.wowhead.com/spell=100127 */

                /* Fiery Web Spin [Heroic Only]
                 * The Cinderweb Spinner channels a web onto a random player, stunning them.*/
                // http://ptr.wowhead.com/spell=97202
                #endregion

                #region Cinderweb Drone
                /* These large spiders climb out of caves below the Cinderweb. When they are depleted of Fire
                 * Endergy, they will climb up to Beth'tilac and siphon Fire Energy from her. */
                // http://db.mmo-champion.com/c/52581/cinderweb-drone/

                /* Consume
                 * Cinderweb Drones consume Spinderweb Spiderlings for 20% of their maximum life and provide
                 * them additional movement and attack speed.*/
                // http://ptr.wowhead.com/spell=99304

                /* Boiling Splatter
                 * The Cinderweb Drone spits burning venom at enemies in a 60 degree cone, dealing 58968 to
                 * 68531 Fire damage.*/

                /* Burning Acid
                 * The Cinderweb Drone spits burning venom at a random enemy, dealing 19016 to 21316 Fire damage. */
                // 12 different version with layering damage ranges posting the most probable with alts
                // 10 man - http://ptr.wowhead.com/spell=99934; alt - http://ptr.wowhead.com/spell=98471
                // 25 man - http://ptr.wowhead.com/spell=100829; alt - http://ptr.wowhead.com/spell=100826
                // 10 man heroic - http://ptr.wowhead.com/spell=100831; alt - http://ptr.wowhead.com/spell=100828
                // 25 man heroic - http://ptr.wowhead.com/spell=100125
                /* Others
                 * http://ptr.wowhead.com/spell=100830; http://ptr.wowhead.com/spell=100827
                 * http://ptr.wowhead.com/spell=99647; http://ptr.wowhead.com/spell=99974; http://ptr.wowhead.com/spell=100127 */

                /* Fixate [Heroic Only]
                 * The Cinderweb Drone fixates on a random player, ignoring all others.*/
                // Two different ids used
                // http://ptr.wowhead.com/spell=99526
                // http://ptr.wowhead.com/spell=99559
                #endregion

                #region Cinderweb Spiderling
                /* These tiny spiders climb out of caves below the Cinderweb. They instinctively move towards
                 * Cinderweb Drones for protection. Cinderweb Spiderlings can be consumed by larger spiders in order
                 * to restore some of their health.*/
                // http://ptr.wowhead.com/npc=53765

                /* Seeping venom
                 * The Cinderweb Spiderling leaps onto a random enemy within 5 yards, injecting them with venom,
                 * which sears them for 6937 to 8062 Fire damage every 2 seconds for 10 sec.*/
                // http://ptr.wowhead.com/spell=97079
                #endregion

                #region Cinderweb Broodling [Heroic Only]
                // These unstable spiders fixate on a random player and explode when they reach their target.

                /* Volatile Burst [Heroic Only]
                 * Upon contact with any enemy, Cinderweb Broodling explode dealing 55500 to 64500 Fire damage
                 * to all enemies within 6 yards.*/
                // http://ptr.wowhead.com/spell=99990; alt - http://ptr.wowhead.com/spell=100838 (60125 to 69875)
                #endregion
                #endregion

                #region The Frenzy!
                /* After she has performed Smouldering Devastation three times, Beth'tilac becomes frenzied. She
                 * emerges from the safety of her Cinderweb canopy and no longer calls for aid from her brood.*/

                /* Frenzy
                 * Parsing Error */
                // Two possible ids
                // http://ptr.wowhead.com/spell=99497 (10% increase damage)
                // http://ptr.wowhead.com/spell=100522 (30% increase damage)

                /* The Widow's Kiss
                 * Beth'tilac's deadly kiss boils the blood of her current target, reducing the amount that they
                 * can be healed by 2% every 2 seconds for 20 sec. If also causes the target to deal growing Fire
                 * damage to their surrounding allies within 10 yards.*/
                // Two possible ids
                // http://ptr.wowhead.com/spell=99476
                // http://ptr.wowhead.com/spell=99506

                /* Ember Flare
                * Intense heat burns enemies near Beth'tilac dealing 18500 to 21500 Fire damage to those on the
                * same side of the web as she is. */
                // 8 different ids for this, using the "tighter" band of damage for now but listing the alt id beside
                // it until further notice.
                // 10 man - http://ptr.wowhead.com/spell=98934; alt - http://ptr.wowhead.com/spell=99859
                // 25 man - http://ptr.wowhead.com/spell=100648; alt - http://ptr.wowhead.com/spell=100649
                // 10 man heroic - http://ptr.wowhead.com/spell=100834; alt - http://ptr.wowhead.com/spell=100935
                // 25 man heroic - http://ptr.wowhead.com/spell=100835; alt - http://ptr.wowhead.com/spell=100936

                /* Consume
                 * Beth'tilac consumes Cinderweb Spiderlings healing for 10% of her life. */
                // 2 spell ids with similar wording, providing both
                // http://ptr.wowhead.com/spell=99332; alt - http://ptr.wowhead.com/spell=99857
                #endregion
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

    public class LordRhyolith : MultiDiffBoss
    {
        public LordRhyolith()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Lord Rhyolith";
            Instance = "Firelands";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, };
            #endregion
            #region Basics
            // Lord Rhyolith has three parts, Left Leg, Right Leg, Chest
            // Legs health are tied together
            // All three have low health, however they start out with an 80% damage
            // reduction applied to them.
            Health = new float[] { (15500000f + 7750000f), (47000000f + 23500000f), (21700000f + 15500000f), (65800000f + 32900000f) };
            MobType = (int)MOB_TYPES.ELEMENTAL;
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
            for (int i = 0; i < 4; i++)
            {
                Phase ObsidianForm = new Phase() { Name = "Obsidian Form" };
                Phase LiquidForm = new Phase() { Name = "Liquid Form" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                #region Obsidian Form
                /* Lord Rhyolith ignores players while his armor is intact, but they can attack his feet to
                 * control his movement.*/

                #region Obsidian Armor
                /* Lord Rhyolish awakens encrusted in a thick coat of Obsidian Armor, which reduces all damage
                 * he takes by 80%. Lord Rhyolith's Obsidian Armor is reduced every time he steps on an active
                 * volcano.
                 * 
                 * *Warning* When damaged, Lord Rhyolith's armor comes to life. If not killed, it will restore
                 * 1% of his armor.*/
                // http://ptr.wowhead.com/spell=98632

                #region Liquid Obsidian
                /* The Liquid Obsidian attempts to move within 5 yards of Lord Rhyolith, then use the Fuse
                 * ability on him.*/
                // http://db.mmo-champion.com/c/52619/liquid-obsidian/ (Elemental)

                /* Fuse
                 * The Liquid Obsidian fuses to Lord Rhyolith granting him an additional 1% of damage reduction.*/
                // http://ptr.wowhead.com/spell=99875
                #endregion
                #endregion

                /* Concussive Stomp
                 * Lord Rhyolith smashes the ground, dealing 32375 to 37625 Fire damage to all players and
                 * knocking away targets within 20 yards. Each stomp creates two to three volcanoes.*/
                // 10 man - http://ptr.wowhead.com/spell=97282
                // 25 man - http://ptr.wowhead.com/spell=100411
                // 10 man heroic - http://ptr.wowhead.com/spell=100968
                // 25 man heroic - http://ptr.wowhead.com/spell=100969

                /* Drink Magma
                 * If Lord Rhyolith is ever permitted to reach the edge of his plateau, he will drink from the
                 * liquid magma, then deal 35000 fire damage to all players every second for 4 sec.*/
                // Drink = http://ptr.wowhead.com/spell=98034
                // Spit = http://ptr.wowhead.com/spell=99867

                #region Thermal Ignition
                /* Lord Rhyolith releases a jet of cinders, which deals 15000 Fire damage to players within 7 yards
                 * and forms part of himself into an elemental. Lord Rhyolith alternates between bringing multiple
                 * Fragments of Lord Rhyolith and single Sparks of Rhyolith to life.*/

                /* Fragment of Rhyolith
                 * Fragments of Rhyolith have low health. If not slain withing 30 sec, they will deal their current
                 * health to a random player.*/
                // http://db.mmo-champion.com/c/52620/fragment-of-rhyolith/

                #region Spark of Rhyolith
                /* Sparks of Rhyolith deal 8078 to 8929 Fire damage to all players within 12 yards. Sparks should
                 * be pulled away from the raid as long as possible, then quickly destroyed.*/
                // http://db.mmo-champion.com/c/53211/spark-of-rhyolith/

                /* Infernal Rage
                 * Sparks of Rhyolith increase their damage dealt by 10% and damage taken by 10% every 5 seconds.*/
                // http://ptr.wowhead.com/spell=98596
                #endregion
                #endregion

                /* Volcano
                 * Lord Rhyolith creates volcanoes when he stomps. Periodically, Lord Rhyolith will bring a
                 * volcano to life, causing it to deal 6000 Fire damage to 3 players every 1 sec. When struck,
                 * the player takes 6000s2% additional Fire damage for 20 sec. Stacks up to 20 times.
                 * 
                 * *Warning* In 25 player raids, the Volcano does damage to 6 players.*/
                // http://ptr.wowhead.com/npc=52582

                /* Crater
                 * Lord Rhyolith creates a crater when he stops on an active volcano. Occasionally, Lord
                 * Rhyolith will cause streams of lava to flow from a crater. The streams will deal 75000*2]
                 * Fire damage to anyone who stands on them for too long.*/
                // Crater - http://db.mmo-champion.com/c/52866/crater/
                #endregion

                #region Liquid Form
                /* When Lord Rhyolith reaches 25% health, his armor shatters. He becomes attackable and
                 * no longer ignores players.*/

                /* Immolation
                 * Lord Rhyolith's fiery presence deals 7003 to 9004 Fire damage to all players every second.*/
                // http://ptr.wowhead.com/spell=99845

                /* Unleashed Flame
                 * Lord Rhyolithunleashes beams of fire which pursue random players, dealing 10000 Fire damage
                 * to all players within 5 yards.*/
                // http://ptr.wowhead.com/spell=100974
                #endregion
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

    public class Alysrazor : MultiDiffBoss
    {
        public Alysrazor()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Alysrazor";
            Instance = "Firelands";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, };
            #endregion
            #region Basics
            Health = new float[] { 51352000f, 154605600f, 71892800f, 216447840f };
            MobType = (int)MOB_TYPES.ELEMENTAL;
            // 3 full phases or 15 minute enrage timer.
            BerserkTimer = new int[] { 15 * 60, 15 * 60, 15 * 60, 15 * 60 };
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
            for (int i = 0; i < 4; i++)
            {
                Phase FlightofFlames = new Phase() { Name = "Flight of Flames" };
                Phase UltimateFirepower = new Phase() { Name = "Ultimate Firepower" };
                Phase Burnout = new Phase() { Name = "Burnout" };
                Phase ReIgnition = new Phase() { Name = "Re-Ignition" };
                
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                /* Firestorm
                 * At the beginning of the battle, Alysrazor ascends into the sky dealing 30000 Fire damage to all
                 * enemies and knocking them back. In addition, Alysrazor will continue to deal 10000 Fire damage
                 * to all enemies every 1 seconds for 10 sec.*/
                // Initial damage - http://ptr.wowhead.com/spell=99605
                // Addition damage - http://ptr.wowhead.com/spell=99606

                /* Volcanic Fire
                 * A massive eruption creates patches of Fire which block escape from Alysrazor's domain. Volcanic
                 * Fire patches deal 92500 to 107500 Fire damage to enemies within 6 yards every 1 seconds.*/
                // http://ptr.wowhead.com/spell=98463

                #region Flight of Flames
                /* Alysrazor flies around the area, allowing her minions to corner her foes far below. She will
                 * periodically fly through the center of the arena to claw at floes as well.*/

                /* Firestorm [Heroic Only]
                 * Alysrazor faces the center of the arena and kicks up a powerful, fiery wind. After 5 seconds,
                 * the arena is bathed in flames, dealing 100000 Fire damage every 1 seconds to all enemies within
                 * line of sight for 10 sec.*/
                // http://ptr.wowhead.com/spell=100745
                // Personal Note - Stopping a Molten Meteor which is summoned by a Herald of the Burning End allows
                // people to Line of Sight the Firestorm damage.

                #region Molting
                // Alysrazor begins to mold, creating Molten Feathers nearby.

                /* Molten Feather [DPS Note]
                 * Molten Feathers can be picked up by players, up to a maximum of three. While holding a
                 * Molten Feather, all spells can be cast while moving and movement speed is increased by 190%
                 * per feather. Once three feathers have been obtained, the player gains Wings of Flame.*/
                // http://ptr.wowhead.com/spell=97128

                /* Wings of Flame
                 * Allows the player to fly for 20 sec.*/
                // http://ptr.wowhead.com/spell=98630
                #endregion

                #region Flying
                // Players in flight using Wings of Flame contend with additional elements of the battle.

                /* Blazing Power
                 * While flying, Alysrazor periodically gives off rings of fire, which last for 3 seconds.
                 * Enemies that pass through the ring gain Blazing Power, wich increases haste by 4% and
                 * stacks up to 25 times. In addition, each stack of Blazing Power restores mana, rage,
                 * energy, runic power, and holy power, and refreshes the duration of Wings of Flame.*/
                // http://ptr.wowhead.com/spell=99461
                // Restores 5% of mana, rage, energy, runic power, and holy power.

                /* Alysra's Razor
                 * If a player gains 25 stacks of Blazing Power, they gain Alysra's Razor, which increases
                 * critical strike chance by 50% for 30 sec.*/
                // http://ptr.wowhead.com/spell=100029

                /* Incendiary Cloud
                 * While flying, Alysrazor periodically gives off between one and three Incendiary Clouds,
                 * which last for 3 seconds. Enemies that pass through the cloud suffer 27750 to 32250 Fire
                 * damage every 1.50 sec.
                 * 
                 * *Warning* In Heroic Difficulty, Alysrazor always creates three Incindiary Clouds.*/
                // 10 man - http://ptr.wowhead.com/spell=99427
                // 25 man - http://ptr.wowhead.com/spell=100729
                // 10 man heroic - http://ptr.wowhead.com/spell=100730
                // 25 man heroic - http://ptr.wowhead.com/spell=100731
                #endregion

                #region Blazing Talon Initiate
                /* Blazing Talon Initiates will periodically fly in from the Firelands to assist Alysrazor
                 * in defeating enemy forces on the ground.*/
                // http://db.mmo-champion.com/c/53897/blazing-talon-initiate/

                /* Brushfire
                 * The Blazing Talon Initiate conjures a fiery ball that moves across the arena, dealing
                 * 27750 to 32250 damage every 1 sec to enemies within 0 yards.*/
                // Original cast - http://ptr.wowhead.com/spell=98868
                // Summons - http://db.mmo-champion.com/c/53372
                // 10 man - http://ptr.wowhead.com/spell=98885
                // 25 man - http://ptr.wowhead.com/spell=100715
                // 10 man heroic - http://ptr.wowhead.com/spell=100716
                // 25 man heroic - http://ptr.wowhead.com/spell=100717
                #endregion

                #region Voracious Hatchling [Tank Note]
                /* Early in Stage 1, two Blazing Broodmothers drop off two Molten Eggs. After several seconds,
                 * the eggs hatch into Voracious Hatchlings. Voracious Hatchlings are indeed voracious and will
                 * throw a Tantrum if not fed Plump Lava Worms.*/
                // http://db.mmo-champion.com/c/53898/voracious-hatchling/

                /* Imprinted
                 * Upon hatchling, Voracious Hatchlings become imprinted on the nearest enemy. The hatchling
                 * will only attack that target, but the target gains 1000% additional damage against the
                 * hatchling.*/
                // http://ptr.wowhead.com/spell=99389; alt - http://ptr.wowhead.com/spell=100359


                /* Satiated
                 * The Voracious Hatchling will not throw a Tantrum when Satiated, which lasts for 15 sec.
                 * Voracious Hatchlings hatch fully Satiated, and can become Satiated again if they are fed
                 * Lava Worms.*/
                // 15 seconds - http://ptr.wowhead.com/spell=99359; alt - http://ptr.wowhead.com/spell=100850
                // 20 seconds - http://ptr.wowhead.com/spell=100852; alt - http://ptr.wowhead.com/spell=100851

                /* Hungry
                 * A Voracious Hatchling that is no longer Satiated becomes Hungry. When Hungry, hatchling
                 * have a 20% chance on hit to throw a Tantrum.*/
                // http://ptr.wowhead.com/spell=99361

                /* Tantrum
                 * The Voracious Hatchling throws a Tantrum, increasing damage by 50% and haste by 50%.*/
                // http://ptr.wowhead.com/spell=99362

                /* Gushing Wound [Healer Note]
                 * The Voracious Hatchling strikes all targets within a 6-yard cone, causing them to bleed for
                 * 3000 Physical damage every 0.20 seconds or until the target's health falls below 50% of
                 * their maximum health.*/
                // 10 man - http://ptr.wowhead.com/spell=100024; alt - http://ptr.wowhead.com/spell=99308
                // 25 man - http://ptr.wowhead.com/spell=100721; alt - http://ptr.wowhead.com/spell=100718
                // 10 man heroic - http://ptr.wowhead.com/spell=100719; alt - http://ptr.wowhead.com/spell=100722
                // 25 man heroic - http://ptr.wowhead.com/spell=100723; alt - http://ptr.wowhead.com/spell=100720
                #endregion

                #region Plump Lava Worm [Tank Note]
                /* During Stage 1, two sets of four Lava Worms will erupt from the molten ground. Lava Worms
                 * cannot be attacked by players. Voracious Hatchlings that are near a Lava Worm will rush
                 * to devour it, becoming temporarily Satiated.*/
                // this means the tank tanking the Hatchlings need to move to position the hatchlings near the worms
                // when they need to be satiated.

                /* Lava Spew
                 * Plump Lava Worms spew a molten cone of fire dealing 27750 to 32250 damage every 1 sec
                 * to all enemies within a 14-yard cone.*/
                // 10 man - http://ptr.wowhead.com/spell=99336
                // 25 man - http://ptr.wowhead.com/spell=100725
                // 10 man heroic - http://ptr.wowhead.com/spell=100726
                // 25 man heroic - http://ptr.wowhead.com/spell=100727
                #endregion

                #region Herald of the Burning End [Heroic Only]
                /* During Stage 1, a Herald of the Burning End will periodically appear and begin casting
                 * Cataclysm. The Herald is immune to all damage, but will die once Cataclysm is cast.*/

                #region Cataclysm [Heroic Only]
                /* The Herald of the Burning End summons a powerful Molten Meteor, dealing 462500 to
                 * 537500 Flamestrike damage to enemies within 0 yards.*/
                // NPC - http://db.mmo-champion.com/c/53489

                /* Molten Meteor [Heroic Only]
                 * After being summoned by the Herald of the Burning End, a Molten Meteor will roll in one
                 * of 8 random directions, dealing 462500 to 537500 Flamestrike damage to enemies within
                 * 0 yards every 1.50 sec.
                 * 
                 * If the meteor reaches a wall, it will break apart into three Molten Boulders, which
                 * ricochet back to the opposite direction. If it is destroyed before it reaches a wall, 
                 * the Molten Meteor becomes temporarily stationary and block line of sight.*/
                // Explosion - http://ptr.wowhead.com/spell=99274
                // Personal note, this is what is used to negate the damage from the 100,000 damage every second
                // from Firestorm.

                /* Molten Boulder [Heroic Only]
                 * Three Molten Boulders form when a Molten Meteor hits a wall and breaks apart. Molten Boulder
                 * deals 29600 to 34400 Flamestrike damage to enemies within 2 yards every 1.50 sec and knock
                 * them back.*/
                // NPC - http://db.mmo-champion.com/c/53496/
                // Damage - http://ptr.wowhead.com/spell=99275
                #endregion
                #endregion
                #endregion

                #region Ultimate Firepower
                /* Alysrazor flies in a tight circle, removing Wings of Flame from all players after 5 seconds,
                 * and begins her ultimate attack.*/

                /* Fiery Vortex
                 * A Fiery Vortex appears in the middle of the arena, dealing 100000 Fire damage every 0.50
                 * seconds to enemies within 0 yards.*/
                // http://ptr.wowhead.com/spell=99794

                /* Fiery Tornado
                 * Fiery Tornadoes erupt from the Fiery Vortex and begin moving rapidly around Alysrazor's arena,
                 * dealing 25000 Fire damage every 1 sec to enemies within 0 yards.*/
                // 10 man - http://ptr.wowhead.com/spell=99816
                // 25 man - http://ptr.wowhead.com/spell=100733
                // 10 man heroic - http://ptr.wowhead.com/spell=100734
                // 25 man heroic - http://ptr.wowhead.com/spell=100735

                /* Blazing Power
                 * Alysrazor continues to give off rings of fire, which appear on the ground of the arena and
                 * lasts for 3 seconds. Players who pass through the ring gain Blazing Power, which increase haste
                 * by  4% and stacks up to 25 times. In addition, each stack of Blazing Power restores mana, rage,
                 * energy, runic power, and holy power.*/
                // http://ptr.wowhead.com/spell=99461
                // Restores 5% of mana, rage, energy, runic power, and holy power.
                #endregion

                #region Burnout [DPS Note]
                /* Alysrazor crashes to the ground and becomes vulnerable, with 0 Molten Power. This stage lasts
                 * until Alyrazor's energy bar reaches 50 Molten Power.*/

                #region Burnout
                /* Alysrazor's fire burns out, causing her to become immobile and increasing damage taken by 100%.
                 * In addition, when struck with a harmful spell, Alysrazor emits Essence of the Green.*/
                // http://ptr.wowhead.com/spell=99432

                /* Essence of the Green [Healer Note]
                 * During Burnout, if Alysrazor is struck by a harmful spell, she emits Essence of the Green
                 * restoring 10% of maximum mana to players.*/
                // http://ptr.wowhead.com/spell=99433
                #endregion

                /* Spark
                 * A bright spark continues to burn within the heart of Alyrazor, restoring 3 Molten Power
                 * every 2 seconds.*/
                // http://ptr.wowhead.com/spell=99921

                #region Blazing Talon Clawshaper
                /* At the start of stage 2, two Blazing Talon Clawshapers will fly in and begin to re-energize
                 * Alysrazor.*/
                // http://db.mmo-champion.com/c/53735/

                /* Ignition
                 * Blazing Talon Clawshapers channel molten energy into Alysrazor, restoring 1 Molten Power
                 * every 1 seconds.*/
                // http://ptr.wowhead.com/spell=99919
                #endregion
                // Personal note - Between the Spark and Clawshapers, this stage lasts for 15 seconds if nothing
                // is done to the Clawshapers, 34 seconds if the Clawshapers are interupted.
                #endregion

                #region Re-Ignition
                /* Alysrazor's fire becomes re-ignited at 50 Molten Power. This stage lasts until Alysrazor
                 * reaches 100 Molten Power.*/

                /* Ignited
                 * Alysrazor's fiery core begins to combust once again, rapidly restoring Molten Power. Restores
                 * 3 Molten Power every 1 seconds.*/
                // http://ptr.wowhead.com/spell=99922
                // Personal Note - This means that the phase will last 17 seconds

                /* Blazing Buffet
                 * Alysrazor's Fiery core emits powerful bursts of flame, dealing 9250 to 10750 Fire damage to all
                 * enemies every 1 seconds for as long as Alysrazor remains ignited.*/
                // 10 man - http://ptr.wowhead.com/spell=99757
                // 25 man - http://ptr.wowhead.com/spell=100739
                // 10 man heroic - http://ptr.wowhead.com/spell=100740
                // 25 man heroic - http://ptr.wowhead.com/spell=100741

                /* Blazing Claw [Tank Note]
                 * Alysrazor claws her enemies, dealing 92500 to 107500 Physical damage to enemies in a 25-yard cone
                 * every 1.50 seconds. In addition, each swipe increases the Fire and Physical damage taken by the
                 * target by 10% for 15 sec.*/
                // http://ptr.wowhead.com/spell=99844
                // Personal Note, this means that there needs to be a tank swap at the 5-6 stack mark

                /* Full Power [Healer Note]
                 * When Alysrazor reaches 100 Molten Power, she is at Full Power, which deals 50000 Fire damage
                 * to all enemies and knocks them back. Once she reaches Full Power, Alysrazor will begin her
                 * Stage 1 activities once again.*/
                // 10 man - http://ptr.wowhead.com/spell=99925
                // 25 man - http://ptr.wowhead.com/spell=100736
                // 10 man heroic - http://ptr.wowhead.com/spell=100737
                // 25 man heroic - http://ptr.wowhead.com/spell=100738
                #endregion

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

    public class Shannox : MultiDiffBoss
    {
        public Shannox()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Shannox";
            Instance = "Firelands";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, };
            #endregion
            #region Basics
            // Baleroc's health consists of both himself and his dogs Ripgut and Rageface
            Health = new float[] { (24049760f + (9619904f * 2f)), (81597400f + (33669664f * 2f)), (33669664f + (13467866f * 2f)), (114236360f + (47137530f * 2f)) };
            MobType = (int)MOB_TYPES.HUMANOID;
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
            for (int i = 0; i < 4; i++)
            {
                Phase EntireFight = new Phase() { Name = "Entire Fight" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                /* Immolation Trap
                 * Shannox launches a fiery trap at the feet of a player. The trap takes 2 seconds to arm, and 
                 * triggers when stepped on therafter, dealing 64374 to 64375 Fire damage immediately and
                 * 24462 to 27038 Fire damage every 3 sec. and increasing all damage taken by 40%, for 9 sec.*/
                // http://ptr.wowhead.com/spell=99838

                /* Crystal Prison Trap
                 * Shannox launches a prison trap at the feet of a player. The trap takes 2 seconds to arm, and
                 * triggers when stepped on therafter, encasing the target in a block of magma crystal, preventing all
                 * movement or other actions. Only by destroying the crystal prison can a trapped player be freed.*/
                // Spell - http://ptr.wowhead.com/spell=99836
                // NPC - http://db.mmo-champion.com/c/53713/

                #region Arcing Slash
                /* Shannox causes 125% of normal melee damage in a wide arc up to 1 yards in front of him, and
                 * inflicts Jagged Tear on those he strikes.*/
                // http://ptr.wowhead.com/spell=99931

                /* Jagged Tear
                 * Shannox's Arcing Slash leaves a Jagged Tear that deals 3000 physical damage every 3 sec for 30
                 * sec. Stacks.*/
                // http://ptr.wowhead.com/spell=99937
                #endregion

                /* Hurl Spear
                 * Shannox hurls his spear in the direction of his hound, Riplimb. The spear deals 117000 to
                 * 123000 physical damage to anyone it strikes directly as well as 59546 to 69204 Fire damage to
                 * all enemies within 50 yards. The spear strike also triggers a cascade of molten eruptions around the
                 * impact point which deal 61156 to 67594 Fire damage to enemies that are caught in them.
                 * 
                 * Riplimb will then break off from combat, fetch the spear, and return it to Shannox.*/
                // http://ptr.wowhead.com/spell=100002

                /* Frenzy
                 * When both of Shannox's hounds are defeated, he goes into a frenzy, increasing physical damage
                 * and attack speed by 30. Shannox no longer uses Hurl Spear after this point and instead drives his
                 * spear directly into the ground to trigger the same cascade of molten eruptions around the impact
                 * point, which deal 61156 to 67594 Fire damage to enemies that are stand in them.*/
                // http://ptr.wowhead.com/spell=100522

                #region Riplimb
                /* Shannox has two hounds. Riplimb will attack the target with the most threat.
                 * 
                 * *Warning* In Heroic Difficulty, Riplimb cannot be permanently slain while his master lives. When
                 * his healther reaches zero, he will collapse for up to 30 seconds, and the reanimate at full health to
                 * resume fighting.*/
                // http://db.mmo-champion.com/c/53694/

                /* Limb Rip
                 * Riplimb bites savagely, dealing 175% of normal melee damage to an enemy and inflicting
                 * Jagged Tear on those he strikes.*/
                // http://ptr.wowhead.com/spell=99832

                /* Jagged Tear
                 * Riplimb's Limb Rip leaves a Jagged Tear that deals 3000 physical damage every 3 sec for 30
                 * sec. Stacks.*/
                // http://ptr.wowhead.com/spell=99937

                /* Frenzied Devotion
                 * Riplimb goes into an unstoppable rage if he is alive to sitness Shannox's health reach 30%.
                 * This effect increases damage dealth by 200% and attack and movement speed by 100%.*/
                // http://ptr.wowhead.com/spell=100064

                /* Feeding Frenzy [Heroic Only]
                 * Riplimb's successful melee attacks grant a stacking 10% bonus to physical damage dealt
                 * for 20sec.*/
                // http://ptr.wowhead.com/spell=100656
                #endregion

                #region Rageface
                /* Shannox has two hounds, Rageface cannot be controlled, and will dart about from enemy to 
                 * enemy, changing targets periodically.*/
                // http://db.mmo-champion.com/c/53695/

                /* Face Rage
                 * Rageface leaps at a random target, stunning and knockign them to the ground, and bgins to
                 * viciously claw at them. This mauling initially deals 8000 physical damage every 0.50 sec, but
                 * the damage dealt increases over time. While so occupied, Rageface is 50% more susceptible to
                 * critical strikes.
                 * Rageface will continue until his target is dead, or he receives a single attack that deals at least
                 * 40000 damage.*/
                // Attack - http://ptr.wowhead.com/spell=99947
                // Get off me - http://ptr.wowhead.com/spell=100129

                /* Feeding Frenzy [Heroic Only]
                 * Rageface's successful melee attacks grant a stacking 10% bonus to physical damage dealt
                 * for 20sec.*/
                // http://ptr.wowhead.com/spell=100656

                /* Frenzied Devotion
                 * Rageface goes into an unstoppable rage if he is alive to sitness Shannox's health reach 30%.
                 * This effect increases damage dealth by 200% and attack and movement speed by 100%.*/
                // http://ptr.wowhead.com/spell=100064
                #endregion
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

    public class Baleroc : MultiDiffBoss
    {
        // The tank and healing check of the raid instance
        // ALMOST a Patchwork fight
        public Baleroc()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Baleroc, the Gatekeeper";
            Instance = "Firelands";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, };
            #endregion
            #region Basics
            Health = new float[] { 42087080f, 133304384f, 58921912f, 186626138f };
            MobType = (int)MOB_TYPES.ELEMENTAL;
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
            for (int i = 0; i < 4; i++)
            {
                Phase EntireFight = new Phase() { Name = "Entire Fight" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                /* Blaze of Glory
                 * Baleroc's assault periodically awakens a burning spark within his primary target, increasing the
                 * target's physical damage taken by 20%, but also raising their maximum health by 20%.
                 * 
                 * Every time Baleroc applies Blaze of Glory, he gains an application of Incendiary Soul, increasing Fire
                 * damage done by 20%.*/
                // http://ptr.wowhead.com/spell=99252

                /* Incendiary Soul
                 * Every time Baleroc applies Blaze of Glory, he gains an application of Incendiary Soul, increasing Fire
                 * damage done by 20%.*/
                // http://ptr.wowhead.com/spell=99369

                #region Shards of Torment
                /* Baleroc summons *warning* two chrystals *end warning* amonst his foes, which continually channel
                 * a shadowy beam on the player that is nearest to them.*/
                // Summon - http://ptr.wowhead.com/spell=99260
                // NPC - http://db.mmo-champion.com/c/53495/

                /* Torment
                 * Deals 3500 Shadow damage per application to the nearest player, stacking once per second.*/
                // 10 man - http://ptr.wowhead.com/spell=99256
                // 25 man - http://ptr.wowhead.com/spell=100230
                // 10 man heroic - http://ptr.wowhead.com/spell=100231
                // 25 man heroic - http://ptr.wowhead.com/spell=100232

                /* Tormented
                 * When Torment fades from a player, they are afflicted by the Tormented effect, which increases
                 * Shadow damage taken by 250% and reduces healing done by 75%, for 40 sec.
                 * *Warning* Direct melee contact with any other player will apply a fresh copy of the Tormented effect
                 * to that player.*/
                // 10 man - http://ptr.wowhead.com/spell=99257
                // 25 man - http://ptr.wowhead.com/spell=99402
                // 10 man heroic - http://ptr.wowhead.com/spell=99403
                // 25 man heroic - http://ptr.wowhead.com/spell=99404

                /* Wave of Torment
                 * If there are no players within 15 yards of a Shard of Torment, the Shard pulses this effect, dealing
                 * 14250 to 15750 Shadow damage each second to all players.*/
                // http://ptr.wowhead.com/spell=99261

                /* Vital Spark
                 * If a player casts a direct heal on someone who is being damaged by Torment, the healer gains an
                 * application of Vital Spark for each stack of Torment on the target. Casting a single-target
                 * direct heal on a target affected by Blaze of Glory will trigger Vital Flame, increasing healing
                 * done on such targets by 2% per stack of vital Spark, lasting for 15 sec.*/
                // http://ptr.wowhead.com/spell=99262

                /* Vital Flame
                 * Increasing healing done to targets affected by Blaze of Glory by 2% per stack of Vital Spark
                 * consumed to create this effect, lasting for 15 sec. When Vital Flame expires, it restores the
                 * Vital Spark stacks that were consumed to create the effect.*/
                // http://ptr.wowhead.com/spell=99263
                #endregion

                #region Blades of Baleroc
                /*Baleroc will periodically empower one of his blades with Shadow or Fire energy, and wield it
                 * alone for 15 sec.*/

                /* Decimation Blade
                 * Baleroc's melee strikes deal Shadow damage equal to 90% of the target's maximum health,
                 * instead of their normal physical damage. This damage cannot be resisted or mitigated by
                 * normal means.*/
                // http://ptr.wowhead.com/spell=99352
                // http://ptr.wowhead.com/spell=99353

                /* Inferno Blade
                 * Baleroc's melee strikes deal 102999 to 103000 Fire damage to the target, instead of their
                 * normal physical damage, while this effect is active.*/
                // http://ptr.wowhead.com/spell=99350
                // http://ptr.wowhead.com/spell=99351
                #endregion

                /* Countdown [Heroic Only]
                 * Baleroc links two players to each other for 8 sec. If the chosen players move within 3 yards of each
                 * other, the effect will dissipate harmlessly, but if the effect runs its full course, both players will
                 * explode, dealing 128749 to 128750 Fire damage to all allies within 45 yards.*/
                // http://ptr.wowhead.com/spell=99516
                // possible explosion id: http://ptr.wowhead.com/spell=99518
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

    public class MajordomoStaghelm : MultiDiffBoss
    {
        public MajordomoStaghelm()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "MajordomoStaghelm";
            Instance = "Firelands";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, };
            #endregion
            #region Basics
            Health = new float[] { 28344360f, 99205264f, 39682104f, 138887370f };
            MobType = (int)MOB_TYPES.HUMANOID;
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
            for (int i = 0; i < 4; i++)
            {
                Phase RageoftheFirelands = new Phase() { Name = "Behold the Rage of the Firelands!" };
                Phase BlazeofGlory = new Phase() { Name = "Blaze of Glory!" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                /* Concentration [Heroic Only]
                 * Each player who engages Fandral on heroic difficulty is granted a Concentration power bar.
                 * This bar fills over time, increasing damage and healing done by 25% for every 25 Concentration
                 * up to 100. Players hit by a damaging attack or spell will lose all currently accumulated
                 * Concentration.*/
                // http://ptr.wowhead.com/spell=98229
                // Uncommon (25%) - http://ptr.wowhead.com/spell=98254
                // Rare (50%) - http://ptr.wowhead.com/spell=98254
                // Epic (75%) - http://ptr.wowhead.com/spell=98252
                // Legendary (100%) - http://ptr.wowhead.com/spell=98245

                #region Behold the Rage of the Firelands!
                /* Fandral transforms into a Cat when his enemies are not clustered together or into a Scorpion when
                 * 7 or more of his enemies are clustered together.*/

                #region Cat Form
                // Fandral transforms into a Cat when his enemies are not clustered together.
                // http://db.mmo-champion.com/c/53145/

                /* Leaping Flames
                 * Fandral leaps at an enemy, inflicting 36404 to 40846 Fire damage in a small area and
                 * creating a Spirit of the Flame. This attack costs 100 energy.*/
                // 10 man - http://ptr.wowhead.com/spell=98535
                // 25 man - http://ptr.wowhead.com/spell=100207
                // 10 man heroic - http://ptr.wowhead.com/spell=100206
                // 25 man heroic - http://ptr.wowhead.com/spell=100208

                /* Spirit of the Flame
                 * These small burning cats attack enemeis until defeated.*/
                // http://db.mmo-champion.com/c/52593/
                // On Heroic, Fandral summons 3 spirits after each leap.

                /* Adrenaline
                 * Fandral gains a stack of Adrenaline each time he performs Leaping Flames. Adrenaline
                 * increases his energy regeneration rate by 1% per application. Fandral loses all stacks of
                 * Adrenaline when he switches form.*/
                // http://ptr.wowhead.com/spell=97238

                /* Fury
                 * Fandral gains a stack of Fury each time he transforms into a Cat or Scorpion,
                 * permanently increasing the Physical damage he deals by 10%.*/
                // http://ptr.wowhead.com/spell=97235
                #endregion

                #region Scorpion Form
                // Fandral transforms into a Scorpion when 7 or more of his enemies are clustered together.

                /* Flame Scythe
                 * Parsing Error*/
                // Damage needs to be split between all players
                // 10 man - http://ptr.wowhead.com/spell=98474
                // 25 man - http://ptr.wowhead.com/spell=100212
                // 10 man heroic - http://ptr.wowhead.com/spell=100213
                // 25 man heroic - http://ptr.wowhead.com/spell=100214

                /* Adrenaline
                 * Fandral gains a stack of Adrenaline each time he performs Flame Scythe. Adrenaline
                 * increases his energy regeneration rate by 1% per application. Fandral loses all stacks of
                 * Adrenaline when he switches form.*/
                // http://ptr.wowhead.com/spell=97238

                /* Fury
                 * Fandral gains a stack of Fury each time he transforms into a Cat or Scorpion,
                 * permanently increasing the Physical damage he deals by 10%.*/
                // http://ptr.wowhead.com/spell=97235
                #endregion
                #endregion

                #region Blaze of Glory!
                /* Fandral continues to transform into a Cat or Scorpion.
                 * 
                 * On every third transform, Fandral pauses in human form to briefly envelop his enemies in a Fiery
                 * Cyclone and cast an additional spell. When he is switching from Cat form to Scorpion form, Fandral
                 * unleashes Searing Seeds. When he is switching from Scorpion form to Cat form, Fandral unleashes
                 * Burning Orbs.*/

                /* Fiery Cyclone
                 * On every third transform, Fandral pauses in human form to briefly envelop his enemies in a
                 * Fiery Cyclone.
                 * 
                 * The Fiery Cyclone tosses all enemy targets into the air, preventing all action but making them
                 * invulnerable for 3 sec.*/

                /* Searing Seeds
                 * When Fandral switches from Cat form to Scorpion form, he unleashes Searing Seeds.
                 * 
                 * Searing Seeds implants fiery seeds in Fandral's enemies. Each seed grows at a different rate.
                 * When fully grown, the seeds explode, inflicting 51499 to 51500 Fire damage to targets within
                 * 10 yards.*/

                /* Burning Orbs
                 * When Fandral switches from Scorpion form to Cat form, he unleashes Burning Orbs.
                 * 
                 * Fandral summons several orbs around the room. Each orb attacks the player nearest to it,
                 * burning them for 7276 to 8174 Fire damage every 2 sec. Stacks.*/
                #endregion
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

    public class Ragnaros : MultiDiffBoss
    {
        public Ragnaros()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Ragnaros";
            Instance = "Firelands";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, };
            #endregion
            #region Basics
            // Rag "dies" at 10% on normal (goes back under the lava).
            // need double check if he dies at 0% on heroic.
            // Initial assumption that he dies at 0% on heroic
            Health = new float[] { 57142855f * 0.9f, 200987280f * 0.9f, 80000000f, 281382192f };
            MobType = (int)MOB_TYPES.ELEMENTAL;
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
            for (int i = 0; i < 4; i++)
            {
                Phase ByFirebePurged = new Phase() { Name = "By Fire be Purged!" };
                Phase Intermission1 = new Phase() { Name = "Intermission: Minions of Fire!" };
                Phase SulfuaswillbeYourEnd = new Phase() { Name = "Sulfuras will be Your End!" };
                Phase Intermission2 = new Phase() { Name = "Intermission: Denizens of Flame!" };
                Phase BegoneFrommyRealm = new Phase() { Name = "Begone From my Realm!" };
                Phase TheTruePoweroftheFireLord = new Phase() { Name = "The True Power of the Fire Lord!" }; // Heroic Only

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                #region By Fire be Purged!
                #region Sulfuras Smash
                /*Ragnarose faces a random player and begins prepares to smash Sulfuras on the platform. The
                 * impact creates several Lava Waves which move out in several directions from the point of
                 * impact.*/
                // 10 man - http://ptr.wowhead.com/spell=98708
                // 25 man - http://ptr.wowhead.com/spell=100256
                // 10 man heroic - http://ptr.wowhead.com/spell=100257
                // 25 man heroic - http://ptr.wowhead.com/spell=100258

                /* Lava Wave
                 * A Lava Wave inflicts 87871 to 92379 Fire damage and knocks back all players it passes
                 * through. Targets who are knocked back suffer an additional 25106 to 26394 Fire damage
                 * every 1 sec for 5 sec.*/
                // 10 man - http://ptr.wowhead.com/spell=98928
                // 25 man - http://ptr.wowhead.com/spell=100292
                // 10 man heroic - http://ptr.wowhead.com/spell=100293
                // 25 man heroic - http://ptr.wowhead.com/spell=100294
                #endregion

                /* Wrath of Ragnaros
                 * Ragnaros targets a player, inflicting 75318 to 79182 Fire damage to all players within 6 yards
                 * and knocking them back.
                 * 
                 * *Warning* In 25 player raids, Ragnaros targets three players.*/
                // 10 man - http://ptr.wowhead.com/spell=98263
                // 25 man - http://ptr.wowhead.com/spell=100114
                // 10 man heroic - http://ptr.wowhead.com/spell=100113
                // 25 man heroic - http://ptr.wowhead.com/spell=100115

                /* Hand of Ragnaros
                 * Ragnaros inflicts 37659 to 39591 Fire damage to all enemies within 50 yards, knocking them
                 * back.*/
                // 10 man - http://ptr.wowhead.com/spell=98237
                // 25 man - http://ptr.wowhead.com/spell=100383
                // 10 man heroic - http://ptr.wowhead.com/spell=100384
                // 25 man heroic - http://ptr.wowhead.com/spell=100387

                #region Magma Trap
                /*Ragnaros periodically forms a Magmaw Trap at a random player's location. The Magma Trap
                 * persists for the duration of the battle, and will trigger when stepped on, causing a Magma Trap
                 * Eruption.*/
                // http://ptr.wowhead.com/spell=98164
                // damage - http://ptr.wowhead.com/spell=98170
                // summons after damage - http://db.mmo-champion.com/c/53086/

                /* Magma Trap Eruption
                 * When triggered, a Magma Trap erupts for 75318 to 79182 Fire damage to all enemies
                 * within the Firelands, and violently knocking the player who tripped the Magma Trap into the air.
                 * 
                 * *Warning* An enemy that triggers a Magma Trap will take 50 additional damage from the Magma
                 * Trap Eruption for 30 sec. Stacks.*/
                /* Eruption
                 * 10 man - http://ptr.wowhead.com/spell=98175
                 * 25 man - http://ptr.wowhead.com/spell=100106
                 * 10 man heroic - http://ptr.wowhead.com/spell=100107
                 * 25 man heroic - http://ptr.wowhead.com/spell=100108*/
                /* Vulnerability
                 * normal - http://ptr.wowhead.com/spell=100238
                 * heroic - http://ptr.wowhead.com/spell=100239*/
                #endregion
                #endregion

                #region Intermission: Minions of Fire!
                /* At 70% health, Ragnaros will cast Splitting Blow wedging Sulfuras into the platform and creating
                 * Sons of Flame across the platform. Ragnaros will stay submerged for 45 seconds or until all of the
                 * Sons of Flame are destroyed.*/

                /* Splitting Blow
                 * Ragnaros buries Sulfuras within the platform, creating Sons of Flame that attempt to reach the
                 * mighty hammer.*/
                /* 12 different ids all with the same descriptions:
                 * http://ptr.wowhead.com/spell=98951; http://ptr.wowhead.com/spell=98952; http://ptr.wowhead.com/spell=98953
                 * http://ptr.wowhead.com/spell=100877; http://ptr.wowhead.com/spell=100878; http://ptr.wowhead.com/spell=100879
                 * http://ptr.wowhead.com/spell=100880; http://ptr.wowhead.com/spell=100881; http://ptr.wowhead.com/spell=100882
                 * http://ptr.wowhead.com/spell=100883; http://ptr.wowhead.com/spell=100884; http://ptr.wowhead.com/spell=100885*/
                // initial Summon Sons trigger - http://ptr.wowhead.com/spell=99012
                // Summon Cast - http://ptr.wowhead.com/spell=99056
                // Number of summons - http://ptr.wowhead.com/spell=99054
                // This means that 10 Sons are summoned that need to be slowed and killed within 45 seconds.

                #region Son of Flame
                /*Sons of Flame will cross the platform and attempt to reform with Sulfuras, causing a
                 * Supernova if they are able to reach the mighty weapon.*/
                // http://db.mmo-champion.com/c/53140/

                /* Burning Speed
                 * Sons of Flame move faster as their blaze burns hotter. Their movement speed is
                 * increased by an amount equal to every 1% health they have above 50% health.*/
                // 10 man - http://ptr.wowhead.com/spell=99414
                // 25 man - http://ptr.wowhead.com/spell=100306
                // 10 man heroic - http://ptr.wowhead.com/spell=100307
                // 25 man heroic - http://ptr.wowhead.com/spell=100308

                /* Supernova
                 * If a Son of Flame reaches Sulfuras, the elemental will explode in a Supernova,
                 * inflicting 112978 to 118772 Fire damage to all players within the Firelands.*/
                // 10 man - http://ptr.wowhead.com/spell=99112
                // 25 man - http://ptr.wowhead.com/spell=100259
                // 10 man heroic - http://ptr.wowhead.com/spell=100260
                // 25 man heroic - http://ptr.wowhead.com/spell=100261
                #endregion
                #endregion

                #region Sulfuras will be Your End!
                #region Sulfuras Smash
                /*Ragnarose faces a random player and begins prepares to smash Sulfuras on the platform. The
                 * impact creates several Lava Waves which move out in several directions from the point of
                 * impact.*/
                // 10 man - http://ptr.wowhead.com/spell=98708
                // 25 man - http://ptr.wowhead.com/spell=100256
                // 10 man heroic - http://ptr.wowhead.com/spell=100257
                // 25 man heroic - http://ptr.wowhead.com/spell=100258

                /* Lava Wave
                 * A Lava Wave inflicts 87871 to 92379 Fire damage and knocks back all players it passes
                 * through. Targets who are knocked back suffer an additional 25106 to 26394 Fire damage
                 * every 1 sec for 5 sec.*/
                // 10 man - http://ptr.wowhead.com/spell=98928
                // 25 man - http://ptr.wowhead.com/spell=100292
                // 10 man heroic - http://ptr.wowhead.com/spell=100293
                // 25 man heroic - http://ptr.wowhead.com/spell=100294
                #endregion

                /* World of Flames [Heroic Only]
                 * Ragnaros periodically engulfs one third of the platform in flame every 2.60 sec for 13 sec.
                 * Inflicting 87871 to 92379 Fire damage to players caught in the conflagration.*/
                // cast - http://ptr.wowhead.com/spell=100171
                // spell - http://ptr.wowhead.com/spell=99171

                #region Molten Seed
                /*Ragnaros forms a Molten Seed at the location of 10 random players, inflicing 69042 to 72583
                 * Fire damage to all players withing 6 yards. After 10 sec the Molten Seed will burst in a Molten
                 * Inferno.
                 * 
                 * *Warning* In 25 player raids, Ragnaros targets 20 players.*/
                // 10 man - http://ptr.wowhead.com/spell=98498
                // 25 man - http://ptr.wowhead.com/spell=100579
                // 10 man heroic - http://ptr.wowhead.com/spell=100580
                // 25 man heroic - http://ptr.wowhead.com/spell=100581

                /* Molten Inferno
                 * When Molten Seed burst, they cause a Molten Inferno that inflicts 128749 to 128750
                 * Fire damage to all players who are near the Molten Seed. The damage decreases the
                 * farther away the target is.
                 * 
                 * Following the Molten inferno, a Molten Elemental is created at the location of the Seed.*/
                // 10 man - http://ptr.wowhead.com/spell=98518
                // 25 man - http://ptr.wowhead.com/spell=100252
                // 10 man heroic - http://ptr.wowhead.com/spell=100253
                // 25 man heroic - http://ptr.wowhead.com/spell=100254

                #region Molten Elemental
                // The Molten Elemental is spawned from a Molten Inferno. It will attack and fixate one random player.
                // http://db.mmo-champion.com/c/53189/

                /* Molten Power [Heroic Only]
                 * The Molten Elemental is empowered by nearby Molten Elementals, increasing its
                 * damage by 25% and causing it to be immune to Snare effects.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100158
                // 25 man heroic - http://ptr.wowhead.com/spell=100302
                #endregion
                #endregion
                #endregion

                #region Intermission: Denizens of Flame!
                /* At 40% health, Ragnaros will cast Splitting Blow, wedging Sulfuras into the platform and creating
                 * Sons of Flame. Ragnaros will stay submerged for 45 seconds or until all of the Sons of Flame are
                 * destroyed.*/

                /* Splitting Blow
                 * Ragnaros buries Sulfuras within the platform, creating Sons of Flame that attempt to reach the
                 * mighty hammer.*/
                /* 12 different ids all with the same descriptions:
                 * http://ptr.wowhead.com/spell=98951; http://ptr.wowhead.com/spell=98952; http://ptr.wowhead.com/spell=98953
                 * http://ptr.wowhead.com/spell=100877; http://ptr.wowhead.com/spell=100878; http://ptr.wowhead.com/spell=100879
                 * http://ptr.wowhead.com/spell=100880; http://ptr.wowhead.com/spell=100881; http://ptr.wowhead.com/spell=100882
                 * http://ptr.wowhead.com/spell=100883; http://ptr.wowhead.com/spell=100884; http://ptr.wowhead.com/spell=100885*/
                // initial Summon Sons trigger - http://ptr.wowhead.com/spell=99012
                // Summon Cast - http://ptr.wowhead.com/spell=99056
                // Number of summons - http://ptr.wowhead.com/spell=99054
                // This means that 10 Sons are summoned that need to be slowed and killed within 45 seconds.

                #region Son of Flame
                /*Sons of Flame will cross the platform and attempt to reform with Sulfuras, causing a
                 * Supernova if they are able to reach the mighty weapon.*/
                // http://db.mmo-champion.com/c/53140/

                /* Burning Speed
                 * Sons of Flame move faster as their blaze burns hotter. Their movement speed is
                 * increased by an amount equal to every 1% health they have above 50% health.*/
                // 10 man - http://ptr.wowhead.com/spell=99414
                // 25 man - http://ptr.wowhead.com/spell=100306
                // 10 man heroic - http://ptr.wowhead.com/spell=100307
                // 25 man heroic - http://ptr.wowhead.com/spell=100308

                /* Supernova
                 * If a Son of Flame reaches Sulfuras, the elemental will explode in a Supernova,
                 * inflicting 112978 to 118772 Fire damage to all players within the Firelands.*/
                // 10 man - http://ptr.wowhead.com/spell=99112
                // 25 man - http://ptr.wowhead.com/spell=100259
                // 10 man heroic - http://ptr.wowhead.com/spell=100260
                // 25 man heroic - http://ptr.wowhead.com/spell=100261
                #endregion

                #region Lava Scion
                // One Lava Scion will form on each side of the platform.
                // http://db.mmo-champion.com/c/53231/

                /* Blazing Heat
                 * The Lava Scion inflicts a random target with Blazing Heat, causing them to create a trail of
                 * Blazing Heat in their wake. Blazing Heat inflicts 62765 to 65985 Fire damage every 1
                 * sec, and heals Sons of Flame and Lava Scions for 10% every 1 sec.*/
                /* Damaging Part
                 * 10 man - http://ptr.wowhead.com/spell=99144
                 * 25 man - http://ptr.wowhead.com/spell=100303
                 * 10 man heroic - http://ptr.wowhead.com/spell=100304
                 * 25 man heroic - http://ptr.wowhead.com/spell=100305*/
                // Healing Part - http://ptr.wowhead.com/spell=99145
                #endregion
                #endregion

                #region Begone From my Realm!
                #region Sulfuras Smash
                /*Ragnarose faces a random player and begins prepares to smash Sulfuras on the platform. The
                 * impact creates several Lava Waves which move out in several directions from the point of
                 * impact.*/
                // 10 man - http://ptr.wowhead.com/spell=98708
                // 25 man - http://ptr.wowhead.com/spell=100256
                // 10 man heroic - http://ptr.wowhead.com/spell=100257
                // 25 man heroic - http://ptr.wowhead.com/spell=100258

                /* Lava Wave
                 * A Lava Wave inflicts 87871 to 92379 Fire damage and knocks back all players it passes
                 * through. Targets who are knocked back suffer an additional 25106 to 26394 Fire damage
                 * every 1 sec for 5 sec.*/
                // 10 man - http://ptr.wowhead.com/spell=98928
                // 25 man - http://ptr.wowhead.com/spell=100292
                // 10 man heroic - http://ptr.wowhead.com/spell=100293
                // 25 man heroic - http://ptr.wowhead.com/spell=100294
                #endregion

                /* World of Flames [Heroic Only]
                 * Ragnaros periodically engulfs one third of the platform in flame every 2.60 sec for 13 sec.
                 * Inflicting 87871 to 92379 Fire damage to players caught in the conflagration.*/
                // cast - http://ptr.wowhead.com/spell=100171
                // spell - http://ptr.wowhead.com/spell=99171

                #region Summon Living Meteor
                /* Ragnaros calls down an increasing number of Living Meteors over time, inclicting 81595 to
                 * 85780 Fire damage to players within 5 yards of the location.*/
                // http://ptr.wowhead.com/spell=99268

                #region Living Meteor
                /* The Living Meteor will fixate on a random target and chase them. A player that gets
                 * within 4 yards of the Living Meteor will trigger a Metoer Impact, inflicting 627656 to
                 * 659844 Fire damage to enemies within 8 yards.*/
                // http://db.mmo-champion.com/c/53500/
                // 10 man - http://ptr.wowhead.com/spell=99317
                // 25 man - http://ptr.wowhead.com/spell=100989
                // 10 man heroic - http://ptr.wowhead.com/spell=100990
                // 25 man heroic - http://ptr.wowhead.com/spell=100991

                /* Meteor Impact
                 * A player that gets within 4 yards of the Living Meteor will trigger a Metoer Impact,
                 * inflicting 627656 to 659844 Fire damage to enemies within 8 yards.*/
                // 10 man - http://ptr.wowhead.com/spell=99287
                // 25 man - http://ptr.wowhead.com/spell=100299
                // 10 man heroic - http://ptr.wowhead.com/spell=100300
                // 25 man heroic - http://ptr.wowhead.com/spell=100301

                #region Combustible
                /* The Living Meteor is highly Combustible. When attacked, it will cause Combustion,
                 * knocking it back several yards away from the enemy that hit it. Combustible is
                 * removed for several seconds after Combustion is triggered.*/

                /* Combustion
                 * While combustible is active, the Living Metoer is knocked back several yards
                 * from the enemy that hit it.*/
                // 10 man - http://ptr.wowhead.com/spell=99296
                // 25 man - http://ptr.wowhead.com/spell=100282
                // 10 man heroic - http://ptr.wowhead.com/spell=100283
                // 25 man heroic - http://ptr.wowhead.com/spell=100284

                // Personal note, Living Meteors can be knocked into the target by way of combustion
                // http://ptr.wowhead.com/spell=100911

                #endregion

                // On Heroic Living Meteors have a 10% additional run speed associated with them
                // 10 man heroic - http://ptr.wowhead.com/spell=100277
                // 25 man heroic - http://ptr.wowhead.com/spell=100286

                // Living Meteors MAY have a 99% damage reduction associated with them
                // http://ptr.wowhead.com/spell=100904
 
                #endregion
                #endregion
                #endregion

                #region The True Power of the Fire Lord [Heroic Only]
                /* The Fire lord unleashes his full power and is able to move freely around the platform. Players have
                 * the aid of powerful heroes of Azeroth to support them.*/

                /* Superheated [Heroic Only]
                 * Ragnaros is at his full power and is now Superheated, inflicting 2510 to 2640 Fire
                 * damage every 1, increasing damage taken from Superheated by 10%. Stacks.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100594
                // 25 man heroic - http://ptr.wowhead.com/spell=100915

                #region Empower Sulfuras [Heroic Only]
                /*Ragnaros begins to empower Sulfuras. After 5 sec, Sulfuras becomes Empowered and attacks
                 * made by Ragnaros cause Flames of Sulfuras, inflicting 627656 to 659844 Fire damage to all
                 * enemies within the Firelands.*/
                /* Cast
                 * 10 man heroic - http://ptr.wowhead.com/spell=100604
                 * 25 man heroic - http://ptr.wowhead.com/spell=100997 */
                // Damage - http://ptr.wowhead.com/spell=100628
                
                /* Flames of Sulfuras [Heroic Only]
                 * When Sulfuras is Empowered, attacks made by Ragnaros cause Flames of Sulfuras,
                 * inflicting 627656 to 659844 Fire damage to all enemies within the Firelands.*/
                // http://ptr.wowhead.com/spell=100630
                #endregion

                /* Dreadflame [Heroic Only]
                 * Sulfuras creates a Dreadflame at two nearby locations. The Dreadflame multiplies rapidly
                 * and spreads across the platform. Dreadflame inflicts 43935 to 46190 Fire damage and an
                 * additional 4506 to 4507 Fire damage every 1 sec for 30 sec.
                 * 
                 * *Warning* In 25 player raids, Dreadflame will strike five locations at once.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100941
                // 25 man heroic - http://ptr.wowhead.com/spell=100998

                /* Magma Geyser [Heroic Only]
                 * Ragnaros will target a Magma Geyser whenever he notices four players in a cluster together. The
                 * Magma Geyser inflicts 69042 to 72583 Fire damage every 1 sec and destroy any nearby
                 * Breadth of Frost.
                 * 
                 * *Warning* In 25 player raids, Ragnaros will use Magma Geyser on clusters of 10 players.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100861
                // 25 man heroic - http://ptr.wowhead.com/spell=100999

                #region Cenarius [Heroic Only]
                /* Cenarius is a demigod, the son of Malorne and Elune, and the patron of all of Azeroth's druids.
                 * 
                 * Cenarius will support the raid by freezing Living Meteors and reducing the damage caused by
                 * Superheated.*/
                // http://db.mmo-champion.com/c/53872/

                /* Breadth of Frost [Heroic Only]
                 * Cenarius forms a Breadth of Frost at a nearby location. Any Living Meteors that enter the
                 * Breadth of Frost are Frozen and take 15000% additional damage. Additionally, players who
                 * stand within the Breadth of Frost are immune to Superheated damage and have the
                 * Superheaded debuff removed from them.*/
                // Living Meteor - http://ptr.wowhead.com/spell=100567
                // Shield from Superheated - http://ptr.wowhead.com/spell=100503
                #endregion

                #region Arch Druid Hamuul Runetotem [Heroic Only]
                /* Humuul Runetotem is a tauren druid and leads the druids of Thunder Bluff. In Mount Hyjal, he
                 * assists Ysera in protecting Nordrassil from Ragnaros.
                 * 
                 * The arch druid will support the raid by entrapping Ragnaros.*/
                // http://db.mmo-champion.com/c/53913/

                /* Entrapping Roots [Heroic Only]
                 * Arch Druid Humuul Runetotem forms Entrapping Roots at a nearby location. If
                 * Ragnaros enters the area of the Entrapping Roots, he will become stunned for 10 sec and
                 * take 50% additional damage while stunned.*/
                // Cast - http://ptr.wowhead.com/spell=100646
                // Stun - http://ptr.wowhead.com/spell=100653
                #endregion

                #region Malfurion Stormrage [Heroic Only]
                /* Malfurion Stormrage is an ancient and powerful night elf druid. He leads the army of Cenarius in
                 * the Defense of Mount Hyjal against the forces of Ragnaros.
                 * 
                 * Malfurion will support the raid by protecting players from Dreadflame.*/
                // http://db.mmo-champion.com/c/52135/

                #region Cloudburst [Heroic Only]
                /* Malfurion forms a Cloudburst. The players who interact with the Cloudburst will be
                 * surrounded with a Deluge. Deluge makes the player immune to Dreadflame damage, and
                 * also allows the player to extinguish any nearby Dreadflame.
                 * 
                 * *Warning* In 25 player raids, up to three players can use a single Cloudburst.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100758
                // 25 man heroic - http://ptr.wowhead.com/spell=100766

                /* Deluge [Heroic Only]
                 * Makes the player immune to Dreadflame damage, and also allows the player to
                 * extinguish any nearby Dreadflame.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100713
                // 25 man heroic - http://ptr.wowhead.com/spell=101015
                // Spell that extinguishes the Dreadflame - http://ptr.wowhead.com/spell=100757
                #endregion
                #endregion
                #endregion
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

}
