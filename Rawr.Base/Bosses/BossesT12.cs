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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, BossHandler.TierLevels.T12_LFR };
            #endregion
            #region Basics
            Health = new float[] { 53946000f, 80900000f, 0, 0, 0 };
            MobType = (int)MOB_TYPES.DEMON;
            // 5 minute Berserk timer
            BerserkTimer = new int[] { 5 * 60, 5 * 60, 0, 0, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 0, 0, 0 };
            InBackPerc_Melee = new double[] { 0.60f, 0.60f, 0, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0, 0 };
            Min_Healers = new int[] { 3, 5, 0, 0, 0 };
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
                    AttackSpeed = 5f,
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
                    AttackSpeed = 5f,
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
                    SpellID = new float[] { 96913, 101007, 0, 0, 0 }[i],
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = ( 102375f + 107625f) / 2f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 24f * 2f,
                });
                EntireFight.LastAttack.SetUnavoidable();
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.MainTank] = true;

                EntireFight.Attacks.Add(new Attack
                {
                    Name = "Searing Shadows [Off Tank]",
                    SpellID = new float[] { 96913, 101007, 0, 0, 0 }[i],
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = (102375f + 107625f) / 2f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 24f * 2f,
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
                    SpellID = new float[] { 96883, 101004, 0, 0, 0 }[i],
                    IsDoT = true,
                    // From videos watched, it only appeares to affect range dps and healers.
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamageType = ItemDamageType.Shadow,
                    // Assume it takes 2 seconds to move out.
                    Duration = 2f, //15f,
                    TickInterval = 1f,
                    DamagePerTick = (34125f + 35875f) / 2f,
                    // Need verification on how many are hit.
                    MaxNumTargets = new float[] { 3, 3, 0, 0, 0 }[i],
                    // Initial release, one video I saw showed 15 seconds between each cast with the zone lasting
                    // about 5 seconds into the next zone's time frame.
                    AttackSpeed = 15.7f,
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
                    Frequency = 58f,
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
                    Name = "Eyes of Occu'thar Movement",
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
                EntireFight.LastAttack.SetAffectsRoles_Tanks(); // will affect the tank not currently tanking.
                EntireFight.LastAttack.SetAffectsRoles_DPS();
                EntireFight.LastAttack.SetAffectsRoles_Healers();

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
                EntireFight.LastAttack.SetAffectsRoles_Tanks(); // will affect the tank not currently tanking.
                EntireFight.LastAttack.SetAffectsRoles_DPS();
                EntireFight.LastAttack.SetAffectsRoles_Healers();
                #endregion

                #region Apply Phases
                int phaseStartTime = 0;
                ApplyAPhasesValues( EntireFight, i, 1, phaseStartTime, BerserkTimer[i], BerserkTimer[i]); phaseStartTime += BerserkTimer[i];
                AddAPhase(EntireFight, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Frost = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Fire = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Nature = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Arcane = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Shadow = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Holy = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0, 0 };
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, BossHandler.TierLevels.T12_LFR };
            Comment = "Main Tank is tanking Beth'tilac, Off Tank is tanking adds during Cinderweb phase.";
            #endregion
            #region Basics
            Health = new float[] { 17693752f, 53167148f, 27829008f, 83658808f, 0 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            TimeBossIsInvuln = new float[] { 8f * 3f, 8f * 3f, 8f * 3f, 8f * 3f, 0 };
            #endregion
            #region Offensive
            for (int i = 0; i < 4; i++)
            {
                Phase TheCinderweb = new Phase() { Name = "The Cinderweb" };
                Phase TheFrenzy = new Phase() { Name = "The Frenzy!" };
                Phase Devastation = new Phase() { Name = "Devastation" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 2.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                Devastation.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                MeleeP2.AttackSpeed = 5f;
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                TheFrenzy.Attacks.Add(MeleeP2);

                #region The Cinderweb
                #region Boss Effects
                // Beth'tilac retreats to her web at the beginning of the battle. Beneath her web
                // scurry her brood.

                /* Fire Energy
                 * Beth'tilac will slowly lose Fire Energy over time. In addition, Cinderweb Drones will siphon
                 * some of her energy when they are depleted. Whenever Beth'tilac runs out of Fire Energy, she will
                 * set herself ablaze, causing Smoldering Devastation. */

                #region Phase 1 Ember Flare
                /* Ember Flare
                 * Intense heat burns enemies near Beth'tilac dealing 18500 to 21500 Fire damage to those on the
                 * same side of the web as she is. */
                // 10 man - http://ptr.wowhead.com/spell=98934 (Upstairs); alt - http://ptr.wowhead.com/spell=99859 (on ground)
                // 25 man - http://ptr.wowhead.com/spell=100648; alt - http://ptr.wowhead.com/spell=100649
                // 10 man heroic - http://ptr.wowhead.com/spell=100834; alt - http://ptr.wowhead.com/spell=100935
                // 25 man heroic - http://ptr.wowhead.com/spell=100835; alt - http://ptr.wowhead.com/spell=100936
                Attack EmberFlare = new Attack
                {
                    Name = "Ember Flare",
                    DamagePerHit = new float[]{ (14152f + 16447f), (15725f + 18275f), (20229f + 23510f), (25858f + 30051f), 0f }[i] / 2f, // Heroic values are a guess.
                    AttackSpeed = 6f,
                    MaxNumTargets = new float[]{ 3f, 5f, 3f, 5f, 0f }[i],
                };
                EmberFlare.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EmberFlare.AffectsRole[PLAYER_ROLES.MainTankHealer] = true;
                EmberFlare.AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                Devastation.Attacks.Add(EmberFlare);
                #endregion

                #region Meteor Burn
                /* Meteor Burn
                 * Meteors crash down onto the web, dealing 37000 to 43000 Fire damage to those who stand beneath
                 * them. Additionally, they burn a hole in the web through which players may fall. */
                // http://ptr.wowhead.com/spell=99076
                // Players should not be hit by these
                Impedance Move_MeteorBurn = new Impedance
                {
                    Chance = new float[]{ 3f, 5f, 3f, 5f, 0f }[i] / Max_Players[i],
                    Name = "Meteor Burn",
                    Duration = 2f * 1000f,
                    Frequency = 15, // Guess
                    Breakable = false,
                };
                Move_MeteorBurn.AffectsRole[PLAYER_ROLES.MainTank] = true;
                Move_MeteorBurn.AffectsRole[PLAYER_ROLES.MainTankHealer] = true;
                Move_MeteorBurn.AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                Devastation.Moves.Add(Move_MeteorBurn);
                #endregion

                #region Consume
                /* Consume
                 * Beth'tilac consumes Cinderweb Spiderlings healing for 10% of her life. */
                // 2 spell ids with similar wording, providing both
                // http://ptr.wowhead.com/spell=99332; alt - http://ptr.wowhead.com/spell=99857
                // This should not happen
                #endregion

                #region Smoldering Devastation
                /* Smoldering Devastation
                 * When Beth'tilac is depleted of Fire Energy she will set herself ablaze, obliterating those who
                 * are not shielded by her web. */
                // http://ptr.wowhead.com/spell=99052
                Impedance Move_SmolderingDevastion = new Impedance
                {
                    Chance = Move_MeteorBurn.Chance,
                    Name = "Smoldering Devastation",
                    Duration = 8f * 1000f, // 5 seconds to jump down, and 3 second to get back up (about 7 seconds to taunt)
                    Frequency = 90,
                    Breakable = false,
                };
                Move_SmolderingDevastion.AffectsRole[PLAYER_ROLES.MainTank] = true;
                Move_SmolderingDevastion.AffectsRole[PLAYER_ROLES.MainTankHealer] = true;
                Move_SmolderingDevastion.AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                Devastation.Moves.Add(Move_SmolderingDevastion);
                #endregion
                #endregion

                #region Cinderweb Spinner
                /* These spiders dangle from the web above. Using Taunt or a similar ability on them will cause
                 * them to drop to the ground. Once killed, their filaments remain allowing players to climb up
                 * to the Cinderweb. */
                // http://ptr.wowhead.com/npc=52981; Beast
                TargetGroup CinderwebSpinner = new TargetGroup
                {
                    Name = "Cinderweb Spinner",
                    NearBoss = false,
                    NumTargs = new float[] { 2, 5, 2, 5, 0}[i],
                    // 3 groups drop after each devastation
                    Frequency = (Move_SmolderingDevastion.Frequency / 3f),
                    Duration = 10f * 1000f, // Assuming they last for only a few seconds since it's a taunt, and a couple swings.
                    TargetID = 52524,
                    LevelOfTargets = 85,
                };
                CinderwebSpinner.SetAffectsRoles_All();
                Devastation.Targets.Add(CinderwebSpinner);

                #region Cinderweb Spinner Burning Acid
                /* Burning Acid
                 * The Cinderweb Spinner spits burning venom at a random enemy, dealing 19016 to 21316 Fire damage. */
                // 12 different version with layering damage ranges posting the most probable with alts
                // 10 man - http://ptr.wowhead.com/spell=98471
                // 25 man - http://ptr.wowhead.com/spell=100826
                // 10 man heroic - http://ptr.wowhead.com/spell=100827
                // 25 man heroic - http://ptr.wowhead.com/spell=100828
                Attack BurningAcid = new Attack
                {
                    Name = "Cinderweb Spinner: Burning Acid",
                    DamagePerHit = new float[]{ (14420f + 16179f), (16022f + 17977f), (20612f + 23127f), (26347f + 29562f), 0f }[i],
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    // assume only one casts is cast per add before being taunted down
                    AttackSpeed = (Move_SmolderingDevastion.Frequency / 3f) / CinderwebSpinner.NumTargs,
                    IsFromAnAdd = true,
                    MaxNumTargets = CinderwebSpinner.NumTargs,
                    Dodgable = false,
                    Parryable = false,
                    Blockable = false,
                    Missable = false,
                };
                BurningAcid.SetAffectsRoles_All();
                Devastation.Attacks.Add(BurningAcid);
                #endregion

                #region Fiery Web Spin
                /* Fiery Web Spin [Heroic Only]
                 * The Cinderweb Spinner channels a web onto a random player, stunning them.*/
                // http://ptr.wowhead.com/spell=97202
                // This should never happen
                if (i > 2) 
                {
                    /*Impedance WebSpin = new Impedance
                    {
                        Name = "Web Spin",
                        Breakable = false,
                        Duration = 25,
                        Frequency = 15,
                    };
                    WebSpin.SetAffectsRoles_All();
                    WebSpin.AffectsRole[PLAYER_ROLES.MainTank] = false;
                    WebSpin.AffectsRole[PLAYER_ROLES.OffTank] = false;
                    TheCinderweb.Stuns.Add(WebSpin);
                     */
                }
                #endregion
                #endregion

                #region Cinderweb Drone
                /* These large spiders climb out of caves below the Cinderweb. When they are depleted of Fire
                 * Energy, they will climb up to Beth'tilac and siphon Fire Energy from her. */
                // http://ptr.wowhead.com/npc=52581#abilities
                TargetGroup CinderwebDrone = new TargetGroup
                {
                    Name = "Cinderweb Drone",
                    NearBoss = false,
                    NumTargs = 1,
                    Duration = 85f * 1000f, // Max duration
                    TargetID = 52581,
                    Frequency = 60,
                    LevelOfTargets = 85,
                };
                CinderwebDrone.AffectsRole[PLAYER_ROLES.OffTank] = true;
                CinderwebDrone.AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                CinderwebDrone.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                TheCinderweb.Targets.Add(CinderwebDrone);

                #region Cinderweb Melee
                Attack DroneMelee = new Attack
                {
                    Name = "Melee from Drones",
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] / 2f,
                    AttackSpeed = 2.125f,
                    IsFromAnAdd = true,
                    MaxNumTargets = 1,
                };
                DroneMelee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                TheCinderweb.Attacks.Add(DroneMelee);
                #endregion

                #region Cinderweb Drone Consume
                /* Consume
                 * Cinderweb Drones consume Spinderweb Spiderlings for 20% of their maximum life and provide
                 * them additional movement and attack speed.*/
                // http://ptr.wowhead.com/spell=99304
                // this should never happen
                #endregion

                #region Boiling Splatter
                /* Boiling Splatter
                 * The Cinderweb Drone spits burning venom at enemies in a 60 degree cone, dealing 58968 to
                 * 68531 Fire damage.*/
                // 10 man - http://ptr.wowhead.com/spell=99463
                // 25 man - http://ptr.wowhead.com/spell=100121
                // 10 man heroic - http://ptr.wowhead.com/spell=100832
                // 25 man heroic - http://ptr.wowhead.com/spell=100833
                // This should only be hit by the off-tank tanking the drones
                Attack BoilingSplatter = new Attack
                {
                    Name = "Boiling Splatter",
                    DamagePerHit = new float[] { (50122f + 58251f), (58968f + 68531f), (64472f + 74927f), (76658f + 89089f), 0f }[i] / 2f,
                    DamageType = ItemDamageType.Fire,
                    AttackSpeed = 15f,
                    IsFromAnAdd = true,
                    MaxNumTargets = 1,
                    Dodgable = false,
                    Parryable = false,
                    Blockable = false,
                    Missable = false,
                    SpellID = new float[] { 99463f, 100121f, 100832f, 100833f, 0f }[i],
                };
                BoilingSplatter.AffectsRole[PLAYER_ROLES.OffTank] = true;
                TheCinderweb.Attacks.Add(BoilingSplatter);
                #endregion

                #region Cinderweb Drone Burning Acid
                /* Burning Acid
                 * The Cinderweb Drone spits burning venom at a random enemy, dealing 19016 to 21316 Fire damage. */
                // 12 different version with layering damage ranges posting the most probable with alts
                // 10 man - http://ptr.wowhead.com/spell=99934
                // 25 man - http://ptr.wowhead.com/spell=100829
                // 10 man heroic - http://ptr.wowhead.com/spell=100830
                // 25 man heroic - http://ptr.wowhead.com/spell=100831
                Attack DroneBurningAcid = new Attack
                {
                    Name = "Cinderweb Drone: Burning Acid",
                    DamagePerHit = new float[] { (14419f + 16179f), (16022f + 17976f), (20612f + 23126f), (26347f + 29561f), 0f }[i],
                    DamageType = ItemDamageType.Fire,
                    AttackSpeed = 8f,
                    IsFromAnAdd = true,
                    MaxNumTargets = 1,
                    Dodgable = false,
                    Parryable = false,
                    Blockable = false,
                    Missable = false,
                    SpellID = new float[] { 99934f, 100829f, 100830f, 100831f, 0f }[i],
                };
                DroneBurningAcid.SetAffectsRoles_DPS();
                DroneBurningAcid.SetAffectsRoles_Healers();
                DroneBurningAcid.AffectsRole[PLAYER_ROLES.OffTank] = true;
                TheCinderweb.Attacks.Add(DroneBurningAcid);
                #endregion

                #region Fixate
                /* Fixate [Heroic Only]
                 * The Cinderweb Drone fixates on a random player, ignoring all others.*/
                // Two different ids used
                // http://www.wowhead.com/spell=99526
                if (i > 1)
                {
                    Attack DroneFixate = DroneMelee.Clone();
                    DroneFixate.DamagePerHit *= .25f;
                    DroneFixate.Duration = 10f;
                    DroneFixate.SpellID = 99526f;
                    DroneFixate.SetAffectsRoles_DPS();
                    DroneFixate.SetAffectsRoles_Healers();
                    TheCinderweb.Attacks.Add(DroneFixate);
                }
                #endregion
                #endregion

                #region Cinderweb Spiderling
                /* These tiny spiders climb out of caves below the Cinderweb. They instinctively move towards
                 * Cinderweb Drones for protection. Cinderweb Spiderlings can be consumed by larger spiders in order
                 * to restore some of their health.*/
                // http://www.wowhead.com/npc=52447
                TargetGroup CinderwebSpiderling = new TargetGroup
                {
                    Name = "Cinderweb Spiderling",
                    NearBoss = false,
                    NumTargs = 10f,
                    Duration = 15f * 1000f,
                    TargetID = 52447f,
                    Frequency = 20f,
                    LevelOfTargets = 85,
                };
                CinderwebSpiderling.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                TheCinderweb.Targets.Add(CinderwebSpiderling);

                #region Seeping Venom
                /* Seeping venom
                 * The Cinderweb Spiderling leaps onto a random enemy within 5 yards, injecting them with venom,
                 * which sears them for 6937 to 8062 Fire damage every 2 seconds for 10 sec.*/
                // http://www.wowhead.com/spell=97079
                Attack SeepingVenom = new Attack
                {
                    Name = "Seeping Venom",
                    DamagePerTick = (6937f + 8062f)/ 2f,
                    Duration = 10f,
                    IsDoT = true,
                    TickInterval = 2f,
                    DamageType = ItemDamageType.Nature,
                    AttackSpeed = 20f,
                    IsFromAnAdd = true,
                    MaxNumTargets = 1,
                    Dodgable = false,
                    Parryable = false,
                    Blockable = false,
                    Missable = false,
                    SpellID = 97079,
                };
                SeepingVenom.SetAffectsRoles_DPS();
                SeepingVenom.SetAffectsRoles_Healers();
                TheCinderweb.Attacks.Add(SeepingVenom);
                #endregion

                #endregion

                #region Cinderweb Broodling [Heroic Only]
                // These unstable spiders fixate on a random player and explode when they reach their target.
                if (i > 1)
                {
                    TargetGroup EngorgedBroodling = new TargetGroup
                    {
                        // These die when touched so they should not be up for more than 3 seconds each
                        Name = "Engorged Broodling",
                        NearBoss = false,
                        NumTargs = new float[] { 0, 0, 1, 3, 0 }[i],
                        Duration = 2f * 1000f,
                        LevelOfTargets = 85,
                        TargetID = new float[] { 0, 0, 53743, 53753, 0 }[i],
                        Frequency = 2f,
                    };
                    EngorgedBroodling.SetAffectsRoles_All();
                    TheCinderweb.Targets.Add(EngorgedBroodling);
                }

                #region Volatile Burst
                /* Volatile Burst [Heroic Only]
                 * Upon contact with any enemy, Cinderweb Broodling explode dealing 55500 to 64500 Fire damage
                 * to all enemies within 6 yards.*/
                // 10-man Heroic - http://ptr.wowhead.com/spell=99990
                // 25-man Heroic - http://ptr.wowhead.com/spell=100838
                Attack VolatileBurst = new Attack
                {
                    Name = "Volatile Burst",
                    DamagePerHit = new float[] { 0f, 0f, (38918f + 45229f), (43243f + 50256f), 0f }[i] / 2f, // Heroic values are a guess.
                    AttackSpeed = 2f,
                    MaxNumTargets = new float[] { 0f, 0f, 1f, 3f, 0f }[i],
                    SpellID = new float[] { 0f, 0f, 99990f, 100838f, 0f }[i],
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Nature,
                    IsFromAnAdd = true,
                };
                VolatileBurst.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                VolatileBurst.AffectsRole[PLAYER_ROLES.OffAndTertTankHealer] = true;
                VolatileBurst.AffectsRole[PLAYER_ROLES.RaidHealer] = true;
                TheCinderweb.Attacks.Add(VolatileBurst);
                #endregion
                #endregion
                #endregion

                #region The Frenzy!
                /* After she has performed Smouldering Devastation three times, Beth'tilac becomes frenzied. She
                 * emerges from the safety of her Cinderweb canopy and no longer calls for aid from her brood.*/

                #region Frenzy
                /* Frenzy
                 * a stacking buff which increases Beth'tilac's damage done by 5% per stack. A stack is added 
                 * every 5 seconds. It acts as a soft enrage timer and this is the reason you want to have 
                 * the boss as low on health as possible when entering this phase. */
                // http://www.wowhead.com/spell=99497
                SpecialEffect FrenzySpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { DamageTakenReductionMultiplier = -0.05f, }, 4.983f * 60f, 5f, 1f, 50);
                Stats FrenzyStats = new Stats();
                FrenzyStats.AddSpecialEffect(FrenzySpecialEffect);
                BuffState Frenzy = new BuffState
                {
                    Name = "Frenzy",
                    Breakable = false,
                    Frequency = BerserkTimer[i] - 1f,
                    Duration = BerserkTimer[i] * 1000f,
                    Stats = FrenzyStats,
                    Chance = 1f,
                };
                Frenzy.SetAffectsRoles_All();
                TheFrenzy.BuffStates.Add(Frenzy);
                #endregion

                #region The Widow's Kiss
                /* The Widow's Kiss
                 * Beth'tilac's deadly kiss boils the blood of her current target, reducing the amount that they
                 * can be healed by 10% every 2 seconds for 20 sec. If also causes the target to deal growing Fire
                 * damage to their surrounding allies within 10 yards.*/
                // Tanks should not be standing next to each other to be taking the fire damage
                // http://ptr.wowhead.com/spell=99506 (10%)
                SpecialEffect widowsKissSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { HealingReceivedMultiplier = -0.10f, }, 5f, 2f, 1f, 10);
                Stats widowsKissStats = new Stats();
                widowsKissStats.AddSpecialEffect(widowsKissSpecialEffect);
                widowsKissSpecialEffect = new SpecialEffect(Trigger.Use, widowsKissStats, 25f, 60f, 1f);
                widowsKissStats = new Stats();
                widowsKissStats.AddSpecialEffect(widowsKissSpecialEffect);
                BuffState WidowsKiss = new BuffState
                {
                    Name = "The Widow's Kiss",
                    Breakable = false,
                    Frequency = 30f * 2f,
                    Duration = 20f * 1000f,
                    Stats = widowsKissStats,
                };
                WidowsKiss.SetAffectsRoles_Tanks();
                TheFrenzy.BuffStates.Add(WidowsKiss);
                #endregion

                #region Phase 2 Ember Flare
                /* Ember Flare
                * Intense heat burns enemies near Beth'tilac dealing 18500 to 21500 Fire damage to those on the
                * same side of the web as she is. */
                // 10 man - http://ptr.wowhead.com/spell=99859
                // 25 man - http://ptr.wowhead.com/spell=100649
                // 10 man heroic - http://ptr.wowhead.com/spell=100935
                // 25 man heroic - http://ptr.wowhead.com/spell=100936
                Attack EmberFlarep2 = EmberFlare.Clone();
                EmberFlarep2.Name = "Ember Flare Phase 2";
                EmberFlarep2.DamagePerHit = new float[] { (15660f + 20340f), (17400f + 22600f), (23809f + 27670f), (30421f + 35355f), 0f }[i] / 2f;
                EmberFlarep2.MaxNumTargets = Max_Players[i];
                EmberFlarep2.SetAffectsRoles_All();
                TheFrenzy.Attacks.Add(EmberFlarep2);
                #endregion

                #region Consume
                /* Consume
                 * Beth'tilac consumes Cinderweb Spiderlings healing for 10% of her life. */
                // 2 spell ids with similar wording, providing both
                // http://ptr.wowhead.com/spell=99332; alt - http://ptr.wowhead.com/spell=99857
                // This should never happen
                #endregion

                #endregion

                #region Apply Phases
                float p1duration = Move_SmolderingDevastion.Frequency;
                float phaseStart = 0f;
                InnerPhase InnerPhaseDevasation;
                for (int j = 0; j < 3; j++)
                {
                    InnerPhaseDevasation = new InnerPhase(Devastation, i, 1, phaseStart, p1duration, BerserkTimer[i]);
                    phaseStart += p1duration;
                    TheCinderweb.InnerPhases.Add(InnerPhaseDevasation);
                }
                ApplyAPhasesValues(TheCinderweb, i, 1, 0, p1duration * 3f, BerserkTimer[i]);
                AddAPhase(TheCinderweb, i);
                ApplyAPhasesValues(TheFrenzy, i, 2, (p1duration * 3f), (BerserkTimer[i] - (p1duration * 3f)), BerserkTimer[i]);
                AddAPhase(TheFrenzy, i);
                #endregion
            }
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Frost = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Fire = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Nature = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Arcane = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Shadow = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Holy = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, BossHandler.TierLevels.T12_LFR };
            #endregion
            #region Basics
            // Lord Rhyolith has three parts, Left Leg, Right Leg, Chest
            // Legs health are tied together
            // All three have low health, however they start out with an 80% damage
            // reduction applied to them.
            Health = new float[] { 13100001f, 40000000f, 20000000f, 60000004f, 0 };
            MobType = (int)MOB_TYPES.ELEMENTAL;
            // Assume people can withstand being in Superheaded for 1 minute
            // Superheated happens after 6 minutes on normal and 5 minutes on heroic
            BerserkTimer = new int[] { 480, 480, 400, 400, 0 };
            SpeedKillTimer = new int[] { 5 * 60, 5 * 60, 5 * 60, 5 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 1, 1, 2, 2, 0 }; // Most guilds will only use 1 tank for the adds on normal, 2 on heroic.
            Min_Healers = new int[] { 2, 5, 3, 5, 0 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase ObsidianForm = new Phase() { Name = "Stage One: Nuisances, Nuisances!" };
                Phase LiquidForm = new Phase() { Name = "Stage Two: Now you will BURN!" };

                // He doesn't have a default melee attack during Obsidian Form (p1)

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
                Stats ObsidianArmor = new Stats();
                ObsidianArmor.BonusDamageMultiplier = -0.80f;
                BuffState ObsidianArmorBuffState;
                float superheated = 0f;
                if (i < 2)
                {
                    superheated = 6f * 60f;
                    int StacksNormal = 80 / 16;
                    SpecialEffect ObsidianArmorSpecialEffectNormal = new SpecialEffect(Trigger.Use, new Stats() { BonusDamageMultiplier = 0.16f, }, superheated, superheated / StacksNormal, 1f, StacksNormal);
                    ObsidianArmor.AddSpecialEffect(ObsidianArmorSpecialEffectNormal);
                }
                else
                {
                    superheated = 5f * 60f;
                    int StacksHeroic = 80 / 10;
                    SpecialEffect ObsidianArmorSpecialEffectHeroic = new SpecialEffect(Trigger.Use, new Stats() { BonusDamageMultiplier = 0.10f, }, superheated, superheated / StacksHeroic, 1f, StacksHeroic);
                    ObsidianArmor.AddSpecialEffect(ObsidianArmorSpecialEffectHeroic);
                }
                ObsidianArmorBuffState = new BuffState
                {
                    Name = "Obsidian Armor",
                    Chance = 1f,
                    Duration = superheated * 1000f,
                    Frequency = BerserkTimer[i] - 1f,
                    Breakable = false,
                    Stats = ObsidianArmor,
                };
                ObsidianArmorBuffState.SetAffectsRoles_All();
                ObsidianForm.BuffStates.Add(ObsidianArmorBuffState);
                #endregion

                #region Liquid Obsidian
                /* The Liquid Obsidian attempts to move within 5 yards of Lord Rhyolith, then use the Fuse
                 * ability on him.*/
                // http://db.mmo-champion.com/c/52619/liquid-obsidian/ (Elemental)
                // Only happens on heroic
                if (i > 1)
                {
                    TargetGroup LiquidObsidian = new TargetGroup
                    {
                        Name = "Liquid Obsidian",
                        NumTargs = 5,
                        LevelOfTargets = 85,
                        Frequency = (5f * 60f) / 8f, // all 40 should be released by the 5 minute mark
                        Duration = 20f * 1000f,
                    };
                    LiquidObsidian.SetAffectsRoles_DPS();
                    ObsidianForm.Targets.Add(LiquidObsidian);

                    #region Fuse
                    /* Fuse
                     * The Liquid Obsidian fuses to Lord Rhyolith granting him an additional 1% of damage reduction.*/
                    // http://ptr.wowhead.com/spell=99875
                    // This should not happen
                    #endregion
                }
                #endregion
                
                #region Concussive Stomp
                /* Concussive Stomp
                 * Lord Rhyolith smashes the ground, dealing 32375 to 37625 Fire damage to all players and
                 * knocking away targets within 20 yards. Each stomp creates two to three volcanoes.*/
                // 10 man - http://ptr.wowhead.com/spell=102306
                // 25 man - http://ptr.wowhead.com/spell=102307
                // 10 man heroic - http://ptr.wowhead.com/spell=102308
                // 25 man heroic - http://ptr.wowhead.com/spell=102309
                Attack ConcussiveStomp = new Attack
                {
                    Name = "Concussive Stomp",
                    DamagePerHit = new float[] { (32375f + 37625f), (32375f + 37625f), (42087f + 48912f), (42087f + 48912f), 0 }[i] / 2f,
                    AttackSpeed = 30,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    SpellID = new float[] { 102306, 102307, 102308, 102309, 0 }[i],
                };
                ConcussiveStomp.SetUnavoidable();
                ConcussiveStomp.SetAffectsRoles_All();
                ObsidianForm.Attacks.Add(ConcussiveStomp);
                #endregion

                #region Drink Magma
                /* Drink Magma
                 * If Lord Rhyolith is ever permitted to reach the edge of his plateau, he will drink from the
                 * liquid magma, then deal 35000 fire damage to all players every second for 4 sec.*/
                // Drink = http://ptr.wowhead.com/spell=98034
                // Spit = http://ptr.wowhead.com/spell=99867
                // This should never happen
                #endregion

                #region Volcano
                /* Volcano
                 * Lord Rhyolith creates volcanoes when he stomps. Periodically, Lord Rhyolith will bring a
                 * volcano to life, causing it to deal 12000 Fire damage to 3 players every 2 sec. When struck,
                 * the player takes 5% additional Fire damage for 20 sec. Stacks up to 20 times.
                 * 
                 * *Warning* In 25 player raids, the Volcano does damage to 6 players.*/
                // http://ptr.wowhead.com/npc=52582

                #region Eruption
                // http://ptr.wowhead.com/spell=98492
                // You should only have 1 active volcano up at any given time
                // Assume the drivers can hit the volcanoes within 20 seconds
                Attack EruptionAttack = new Attack
                {
                    Name = "Eruption",
                    DamagePerTick = 12000f,
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    MaxNumTargets = new float[] { 3, 6, 3, 6, 0 }[i],
                    IsDoT = true,
                    TickInterval = 2f,
                    Duration = 25f * 1000f,
                    SpellID = 98492f,
                    AttackSpeed = ((i < 2) ? 40f : 25.5f),
                };
                EruptionAttack.SetUnavoidable();
                EruptionAttack.SetAffectsRoles_All();
                ObsidianForm.Attacks.Add(EruptionAttack);

                Stats EruptionStats = new Stats();
                SpecialEffect EruptionSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { FireDamageTakenMultiplier = 0.05f }, 15f, 2f, EruptionAttack.MaxNumTargets / Max_Players[i], 20);
                EruptionStats.AddSpecialEffect(EruptionSpecialEffect);
                BuffState EruptionBuffState = new BuffState
                {
                    Name = "Eruption Debuff",
                    Frequency = EruptionAttack.AttackSpeed,
                    Duration = EruptionAttack.Duration,
                    Chance = EruptionAttack.MaxNumTargets / Max_Players[i],
                    Breakable = false,
                    Stats = EruptionStats,
                };
                EruptionBuffState.SetAffectsRoles_All();
                ObsidianForm.BuffStates.Add(EruptionBuffState);
                #endregion
                #endregion

                #region Crater
                /* Crater
                 * Lord Rhyolith creates a crater when he stops on an active volcano. Occasionally, Lord
                 * Rhyolith will cause streams of lava to flow from a crater. The streams will deal 75000*2]
                 * Fire damage to anyone who stands on them for too long.*/
                // Crater - http://ptr.wowhead.com/npc=52866

                #region Magma Flow
                // You need to move way from the lines that come from the craters
                // 2 on 10-man, 3 on 25-man, volcanoes spawn appoximately every 25 seconds
                // Assume half on 10-man, 2/3rds on 25-man get stomped causing craters
                Impedance Move_MagmaFlow = new Impedance
                {
                    Chance = 0.50f,
                    Name = "Meteor Burn",
                    Duration = 2f * 1000f,
                    Frequency = 25f / ((i < 2) ? (1f / 2f) : (2f / 3f)),
                };
                Move_MagmaFlow.SetAffectsRoles_All();
                ObsidianForm.Moves.Add(Move_MagmaFlow);
                #endregion
                #endregion

                #region Thermal Ignition
                /* Lord Rhyolith releases a jet of cinders, which deals 15000 Fire damage to players within 7 yards
                 * and forms part of himself into an elemental. Lord Rhyolith alternates between bringing multiple
                 * Fragments of Lord Rhyolith and single Sparks of Rhyolith to life.*/

                // Summon Fragment of Rhyolith - http://ptr.wowhead.com/spell=100392
                // Summon Spark of Rhyolith - http://ptr.wowhead.com/spell=98552
                Attack SummonAdds = new Attack
                {
                    Name = "Summon Fragment or Spark",
                    DamagePerHit = new float[] { 15000, 15000, 15000, 15000, 0 }[i],
                    AttackSpeed = 22.5f,
                    IsDoT = false,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    IsFromAnAdd = true,
                };
                SummonAdds.SetUnavoidable();
                SummonAdds.SetAffectsRoles_All();
                ObsidianForm.Attacks.Add(SummonAdds);
                
                #region Fragments of Rhyolith
                // Fragments of Rhyolith have low Heath. If not slain within 30 sec, they inflict
                // damage equal to their current health to a random player.
                // In 25 person raids, they deal damage equal to half their current health to a 
                // random player.
                
                // Fragments should not be damaging anyone besides the tank tanking them.
                // Meaning they should be killed within the 30 second timeframe.
                // 5 x Fragments to be tanked & DPS'd down
                TargetGroup Fragments = new TargetGroup
                {
                    Name = "Fragments of Rhyolith",
                    NumTargs = 5,
                    LevelOfTargets = 85,
                    Frequency = 45f,
                    Duration = 30f * 1000f,
                    TargetID = 52620,
                    NearBoss = true,
                };
                Fragments.SetAffectsRoles_DPS();
                Fragments.SetAffectsRoles_Tanks();
                ObsidianForm.Targets.Add(Fragments);

                Attack FragmentMelee = new Attack
                {
                    Name = "Melee from Fragment Adds",
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 0.1f,
                    AttackSpeed = 2.5f / 5f, // 5 mobs hitting every 2.5 secs.
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamageType = ItemDamageType.Physical,
                    IsFromAnAdd = true,
                };
                FragmentMelee.SetAffectsRoles_Tanks();
                ObsidianForm.Attacks.Add(FragmentMelee);
                #endregion

                #region Spark of Rhyolith
                // 1 x Spark to be tanked & DPS'd down.
                // Sparks should not be up with more than 10 stacks of Infernal Rage
                TargetGroup Spark = new TargetGroup
                {
                    Name = "Spark of Rhyolith",
                    NumTargs = 1,
                    LevelOfTargets = 85,
                    Frequency = 70f,
                    Duration = 5f * 10f * 1000f,
                    TargetID = 53211,
                    NearBoss = false,
                };
                Spark.SetAffectsRoles_DPS();
                Spark.SetAffectsRoles_Tanks();
                ObsidianForm.Targets.Add(Spark);

                #region Spark Melee
                Attack SparkMelee = new Attack
                {
                    Name = "Melee from Spark Add",
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 0.35f,
                    AttackSpeed = 2.5f, 
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamageType = ItemDamageType.Physical,
                    Dodgable = true,
                    Missable = true,
                    Parryable = true,
                    Blockable =  true,
                };
                SparkMelee.SetAffectsRoles_Tanks();
                ObsidianForm.Attacks.Add(SparkMelee);
                #endregion

                #region Immolation
                // 10-man - http://ptr.wowhead.com/spell=98598
                // 25-man - http://ptr.wowhead.com/spell=100414
                // 10-man Heroic - http://ptr.wowhead.com/spell=98598
                // 25-man Heroic - http://ptr.wowhead.com/spell=100414
                Attack SparkImmolation = new Attack
                {
                    Name = "Immolation from Spark",
                    DamagePerTick = (6840f + 7560f) / 2f,
                    IsDoT = true,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    TickInterval = 1,
                    Duration = Spark.Duration, // Max length is how long it takes to drop him.  Let's assume just as long until the next add pack.
                    Missable = false,
                    Dodgable = false,
                    Parryable = false,
                    Blockable = false,
                    SpellID = new float[] { 98598f, 100414, 98598, 100414, 0 }[i],
                };
                SparkImmolation.SetAffectsRoles_Tanks();
                SparkImmolation.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                ObsidianForm.Attacks.Add(SparkImmolation);
                #endregion

                #region Infernal Rage
                // Sparks of Rhyolith increase their damage dealt by 10% and damage taken
                // by 10% every 5 seconds. This effect stacks up to 20 times.
                // This should not stack up more than 10 stacks
                Stats InfernalRageStats = new Stats();
                // all adds disappear going into Phase 2, so have the debuff last until superheated should be appropriat
                SpecialEffect InfernalRageSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { BonusDamageMultiplier = 0.10f, DamageTakenReductionMultiplier = -0.10f, }, superheated, 5f, 1f, 20);
                InfernalRageStats.AddSpecialEffect(InfernalRageSpecialEffect);
                InfernalRageSpecialEffect = new SpecialEffect(Trigger.Use, InfernalRageStats, Spark.Duration / 1000f, Spark.Frequency, 1f);
                InfernalRageStats = new Stats();
                InfernalRageStats.AddSpecialEffect(InfernalRageSpecialEffect);
                BuffState InfernalRage = new BuffState
                {
                    Name = "Infernal Rage",
                    Duration = Spark.Duration,
                    Chance = 1f,
                    Frequency = Spark.Frequency,
                    Breakable = false,
                    Stats = InfernalRageStats,
                };
                InfernalRage.SetAffectsRoles_Tanks();
                ObsidianForm.BuffStates.Add(InfernalRage);
                #endregion
                #endregion
                #endregion
                #endregion

                #region Liquid Form
                /* When Lord Rhyolith reaches 25% health, his armor shatters. He becomes attackable and
                 * no longer ignores players.*/
                #region Melee Attack
                Attack BasicMeleeMT = new Attack
                {
                    Name = "Default Melee",
                    AttackSpeed = 5f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    Missable = true,
                    Dodgable = true,
                    Parryable = true,
                    Blockable = true,
                };
                BasicMeleeMT.AffectsRole[PLAYER_ROLES.MainTank] = true;
                LiquidForm.Attacks.Add(BasicMeleeMT);
                #endregion

                #region Immolation
                /* Immolation
                 * Lord Rhyolith's fiery presence deals 7003 to 9004 Fire damage to all players every second.*/
                // http://ptr.wowhead.com/spell=99845
                Attack Immolation = new Attack
                {
                    Name = "Phase 2 Immolation",
                    DamagePerTick = (7000f + 9000f) / 2f,
                    IsDoT = true,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    TickInterval = 1,
                    Duration = BerserkTimer[i] * .25f * 1000f, // for the duration of this phase.
                    SpellID = 99845f,
                };
                Immolation.SetUnavoidable();
                Immolation.SetAffectsRoles_All();
                LiquidForm.Attacks.Add(Immolation);
                #endregion

                #region Concussive Stomp
                //Concussive Stomp
                Attack ConcussiveStopPhase2 = ConcussiveStomp.Clone();
                ConcussiveStopPhase2.AttackSpeed = 10f;
                LiquidForm.Attacks.Add(ConcussiveStopPhase2);
                #endregion

                #region Unleashed Flame
                /* Unleashed Flame
                 * Lord Rhyolithunleashes beams of fire which pursue random players, dealing 10000 Fire damage
                 * to all players within 5 yards.*/
                // http://ptr.wowhead.com/spell=100974
                if (i > 1)
                {
                    Attack UnleashedFlame = new Attack
                    {
                        Name = "Unleashed Flame",
                        DamagePerTick = 10000f,
                        MaxNumTargets = Max_Players[i],
                        IsDoT = true,
                        TickInterval = 1f,
                        Duration = Immolation.Duration,
                        AttackType = ATTACK_TYPES.AT_AOE,
                        DamageType = ItemDamageType.Fire,
                        SpellID = 100974f,
                    };
                    UnleashedFlame.SetUnavoidable();
                    UnleashedFlame.SetAffectsRoles_All();
                    LiquidForm.Attacks.Add(UnleashedFlame);
                }
                #endregion
                #endregion

                #region Add Phases
                int phaseStartTime = 0;
                int phaseDuration = (int)((float)BerserkTimer[i] * (1 - .25f));  // Phase change at 25% health when the armor is gone.
                ApplyAPhasesValues( ObsidianForm, i, 1, phaseStartTime, phaseDuration, BerserkTimer[i]);
                AddAPhase(ObsidianForm, i);
                phaseStartTime = phaseStartTime + phaseDuration;
                phaseDuration = BerserkTimer[i] - phaseDuration;
                ApplyAPhasesValues( LiquidForm, i, 1, phaseStartTime, phaseDuration, BerserkTimer[i]);
                AddAPhase(LiquidForm, i);
                #endregion
            }
            #endregion
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Frost = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Fire = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Nature = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Arcane = new double[] { 0.00f, 0.00f, 0, 0, 0};
            Resist_Shadow = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Holy = new double[] { 0.00f, 0.00f, 0, 0, 0 };
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
            /* TODO:
             * Impedances.  This is a rediculous movement fight and I'm not sure how to model that yet.
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, BossHandler.TierLevels.T12_LFR };
            Comment = "Not modeled in detail yet.";
            #endregion
            #region Basics
            Health = new float[] { 38651400f, 115954200f, 71892800f, 197122144f, 0 }; // TODO: double check 25-man normal and 10-man heroic health pool
            MobType = (int)MOB_TYPES.ELEMENTAL;
            // 3 full phases on Normal; 2 full phases on Heroic; or 15 minute enrage timer.
            BerserkTimer = new int[] { 15 * 60, 15 * 60, 15 * 60, 15 * 60, 0 };
            SpeedKillTimer = new int[] { 8 * 60, 8 * 60, 8 * 60, 8 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase InitialPull = new Phase() { Name = "Initial Pull" };
                Phase FlightofFlames = new Phase() { Name = "Flight of Flames" };
                Phase FlightofFlamesPart2 = new Phase() { Name = "Flight of Flames" };
                Phase UltimateFirepower = new Phase() { Name = "Ultimate Firepower" };
                Phase Burnout = new Phase() { Name = "Burnout" };
                Phase ReIgnition = new Phase() { Name = "Re-Ignition" };

                //Mini-Phase variables
                Phase HatchlingPhase = new Phase() { Name = "Hatchlings" };
                Phase FireStormPhase = new Phase() { Name = "Firestorm Phase" };
                InnerPhase InnerPhaseHatchling, InnerPhaseFirestorm;

                float FlightofFlamesPhaseTime = new float[] { 190f, 190f, 250f, 250f, 0f }[i];
                float FirestormCD = new float[] { 0, 0, 83f, 83f, 0 }[i];
                float MeteorCD = new float[] { 0, 0, 37, 37, 0 }[i];
                float FieryTornadoDuration = 35f;
                float BurnoutDuration = 33f;
                float ReIgniteDuration = 50f / 3f;

                #region Initial Pull
                #region Initial Pull Firestorm
                /* Firestorm
                 * At the beginning of the battle, Alysrazor ascends into the sky dealing 30000 Fire damage to all
                 * enemies and knocking them back. In addition, Alysrazor will continue to deal 10000 Fire damage
                 * to all enemies every 1 seconds for 10 sec.*/
                // Initial damage
                // 10-man - http://ptr.wowhead.com/spell=99605
                // 25-man - http://ptr.wowhead.com/spell=101658
                // 10-man Heroic - http://ptr.wowhead.com/spell=101659
                // 25-man Heroic - http://ptr.wowhead.com/spell=101660
                // Addition damage - http://ptr.wowhead.com/spell=99606
                Attack InitialFirestorm = new Attack
                {
                    Name = "Initial Pull Firestorm",
                    AttackSpeed = BerserkTimer[i] - 1f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    Interruptable = false,
                    MaxNumTargets = Max_Players[i],
                    DamagePerHit = new float[] { 20000f, 20000f, 25000f, 25000f, 0 }[i],
                    DamagePerTick = 8000f,
                    IsDoT = true,
                    Duration = 10f * 1000f,
                    TickInterval = 1f,
                    SpellID = new float[] { 99605, 101658, 101659, 101660, 0 }[i],
                };
                InitialFirestorm.SetAffectsRoles_All();
                InitialPull.Attacks.Add(InitialFirestorm);
                #endregion

                #region Volcanic Fire
                /* Volcanic Fire
                 * A massive eruption creates patches of Fire which block escape from Alysrazor's domain. Volcanic
                 * Fire patches deal 92500 to 107500 Fire damage to enemies within 6 yards every 1 seconds.*/
                // http://ptr.wowhead.com/spell=98463
                // This should never be hit
                #endregion
                #endregion

                #region Flight of Flames
                /* Alysrazor flies around the area, allowing her minions to corner her foes far below. She will
                 * periodically fly through the center of the arena to claw at floes as well.*/

                #region Firestorm [Heroic Only]
                /* Alysrazor faces the center of the arena and kicks up a powerful, fiery wind. After 5 seconds,
                 * the arena is bathed in flames, dealing 100000 Fire damage every 1 seconds to all enemies within
                 * line of sight for 5 sec.*/
                // Initicial cast - http://ptr.wowhead.com/spell=100744
                // Actual Damage - http://ptr.wowhead.com/spell=100745
                // Personal Note - Stopping a Molten Meteor which is summoned by a Herald of the Burning End allows
                // people to Line of Sight the Firestorm damage, thus negating the damage (aka interuptable).
                // This should never be hit, but people should move on the cast part
                if (i > 1)
                {
                    /*Attack Firestorm = new Attack
                    {
                        Name = "Firestorm",
                        DamagePerTick = 50000f,
                        IsDoT = true,
                        TickInterval = 1f,
                        Duration = 5f,
                        DamageType = ItemDamageType.Fire,
                        AttackSpeed = FirestormCD,
                    };
                    Firestorm.SetAffectsRoles_All();
                    Firestorm.AffectsRole[PLAYER_ROLES.AlysrazorAirGroup] = true;
                     */

                    Impedance Firestorm_move = new Impedance
                    {
                        Name = "Move to Hide from Firestorm",
                        Chance = 1f,
                        Breakable = false,
                        Duration = 5f * 1000f,
                        Frequency = 10f,
                    };
                    Firestorm_move.SetAffectsRoles_All();
                    Firestorm_move.AffectsRole[PLAYER_ROLES.AlysrazorAirGroup] = true;
                    FireStormPhase.Moves.Add(Firestorm_move);
                }
                #endregion

                #region Molting
                // Alysrazor begins to mold, creating Molten Feathers nearby.
                // She channels this for 9 seconds.
                // 10-man/Heroic - http://ptr.wowhead.com/spell=99464, she drops every second
                // 25-man/Heroic - http://ptr.wowhead.com/spell=100698, every 400 milliseconds
                float feathers = 9f / new float[] { 1f, 0.4f, 1f, 0.4f, 0f }[i];

                #region Molten Feather [DPS Note]
                /* Molten Feathers can be picked up by players, up to a maximum of three. While holding a
                 * Molten Feather, all spells can be cast while moving and movement speed is increased by 30%
                 * per feather. Once three feathers have been obtained, the player gains Wings of Flame.*/
                // http://ptr.wowhead.com/spell=97128
                float AirGroupSize = new float[] { 1f, 3f, 1f, 3f, 0f }[i];
                BuffState MoltenFeatherAirGroup = new BuffState
                {
                    Name = "Molten Feather Air Group",
                    Chance = AirGroupSize / (Max_Players[i] - Min_Tanks[i] - Min_Healers[i]),
                    // Assume that it does not fall off until the end of Burnout
                    Duration = FlightofFlamesPhaseTime * 1000f,
                    Frequency = FlightofFlamesPhaseTime,
                    Breakable = false,
                    Stats = new Stats() { MovementSpeed = (0.30f * 3f) },
                };
                MoltenFeatherAirGroup.AffectsRole[PLAYER_ROLES.AlysrazorAirGroup] = true;

                // On heroic air group needs to grab 1 feather each after each Firestorm to keep their flying buff up.
                float remainingfeathers = (feathers - (AirGroupSize * 3f)) + (feathers - ((i > 1) ? AirGroupSize : 0f)) * 2f;
                BuffState MoltenFeatherGroundGroup = new BuffState
                {
                    Name = "Molten Feather Ground Group",
                    Chance = (Max_Players[i] - AirGroupSize) / Max_Players[i],
                    Duration = MoltenFeatherAirGroup.Duration,
                    Frequency = MoltenFeatherAirGroup.Frequency,
                    Breakable = false,
                    Stats = new Stats() { MovementSpeed = (0.30f * (3f * (remainingfeathers / (feathers * 3f)))) },
                };
                MoltenFeatherGroundGroup.SetAffectsRoles_All();
                FlightofFlames.BuffStates.Add(MoltenFeatherGroundGroup);
                FlightofFlamesPart2.BuffStates.Add(MoltenFeatherGroundGroup);
                // Since people are getting feathers, casters can cast while moving, thus eliminating most movement impedances
                #endregion
                
                #region Wings of Flame
                // Allows the player to fly for 20 sec.*/
                // http://ptr.wowhead.com/spell=98630
                MoltenFeatherAirGroup.Stats.MovementSpeed = 2.25f;
                FlightofFlames.BuffStates.Add(MoltenFeatherAirGroup);
                FlightofFlamesPart2.BuffStates.Add(MoltenFeatherAirGroup);
                #endregion
                #endregion

                #region Flying
                // Players in flight using Wings of Flame contend with additional elements of the battle.

                #region Blazing Power
                /* While flying, Alysrazor periodically gives off rings of fire, which last for 3 seconds.
                 * Enemies that pass through the ring gain Blazing Power, wich increases haste by 4% and
                 * stacks up to 25 times. In addition, each stack of Blazing Power restores mana, rage,
                 * energy, runic power, and holy power, and refreshes the duration of Wings of Flame.*/
                // http://ptr.wowhead.com/spell=99461
                // Restores 5% of mana, rage, energy, runic power, and holy power.
                Stats BlazingPowerStats = new Stats();
                SpecialEffect BlazingPowerSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { ManaorEquivRestore = 0.05f, PhysicalHaste = 0.08f, SpellHaste = 0.08f }, 40f, 5f, 1f, 25);
                BlazingPowerStats.AddSpecialEffect(BlazingPowerSpecialEffect);
                BuffState BlazingPower = new BuffState
                {
                    Name = "Blazing Power",
                    Frequency = MoltenFeatherAirGroup.Frequency,
                    Duration = MoltenFeatherAirGroup.Duration,
                    Chance = 1f,
                    Breakable = false,
                    Stats = BlazingPowerStats,
                };
                BlazingPower.AffectsRole[PLAYER_ROLES.AlysrazorAirGroup] = true;
                FlightofFlames.BuffStates.Add(BlazingPower);
                FlightofFlamesPart2.BuffStates.Add(BlazingPower);
                #endregion

                #region Alysra's Razor
                /* If a player gains 25 stacks of Blazing Power, they gain Alysra's Razor, which increases
                 * critical strike chance by 50% for 30 sec.*/
                // http://ptr.wowhead.com/spell=100029
                // Normal there is no break in the timing on normal,
                // Flyer should have 25 stacks during the second Firestorm, so only 10 seconds while not refreshing the stack
                float AlysrasRazorStartTime = (i < 2 ? 125f : 135f);
                BuffState AlysrasRazor = new BuffState
                {
                    Name = "Alysra's Razor",
                    Frequency = MoltenFeatherAirGroup.Frequency,
                    Duration = MoltenFeatherAirGroup.Duration - (AlysrasRazorStartTime * 1000f),
                    Chance = 1f,
                    Breakable = false,
                    Stats = new Stats() { SpellCrit = 0.75f, PhysicalCrit = 0.75f },
                };
                AlysrasRazor.AffectsRole[PLAYER_ROLES.AlysrazorAirGroup] = true;
                FlightofFlames.BuffStates.Add(AlysrasRazor);
                FlightofFlamesPart2.BuffStates.Add(AlysrasRazor);
                #endregion

                #region Incendiary Cloud
                /* While flying, Alysrazor periodically gives off between one and three Incendiary Clouds,
                 * which last for 3 seconds. Enemies that pass through the cloud suffer 27750 to 32250 Fire
                 * damage every 1.50 sec.
                 * 
                 * *Warning* In Heroic Difficulty, Alysrazor always creates three Incindiary Clouds.*/
                // 10 man - http://ptr.wowhead.com/spell=99427
                // 25 man - http://ptr.wowhead.com/spell=100729
                // 10 man heroic - http://ptr.wowhead.com/spell=100730
                // 25 man heroic - http://ptr.wowhead.com/spell=100731
                // These should NEVER be hit by the Air group
                Attack IncendiaryCloud = new Attack
                {
                    Name = "Incendiary Cloud",
                    DamagePerTick = new float[] { (27750f + 32250f), (27750f + 32250f), (55500f + 64500f), (55500f + 64500f), 0 }[i] / 2f,
                    IsDoT = true,
                    TickInterval = 1.5f,
                    Duration = 3f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    AttackSpeed = 5f,
                    DamageType = ItemDamageType.Fire,
                    SpellID = new float[] { 99427, 100729, 100730, 100731, 0 }[i],
                    // Normal has 2 clouds, Heroic has 3 clouds
                    MaxNumTargets = new float[] { 2, 2, 3, 3, 0 }[i],
                    // By flying around the clouds, you can "interrupt" the attacks
                    Interruptable = true,
                };
                IncendiaryCloud.AffectsRole[PLAYER_ROLES.AlysrazorAirGroup] = true;
                FlightofFlames.Attacks.Add(IncendiaryCloud);
                FlightofFlamesPart2.Attacks.Add(IncendiaryCloud);
                #endregion

                #endregion

                #region Blazing Talon Initiate
                /* Blazing Talon Initiates will periodically fly in from the Firelands to assist Alysrazor
                 * in defeating enemy forces on the ground.*/
                // West-side - http://ptr.wowhead.com/npc=53896
                // East-side - http://ptr.wowhead.com/npc=53369
                // Timer - Both - Normal - 17; Heroic - 17
                //         Both - Normal - 31; heroic - 22
                //         East - Normal - 21; Heroic - 53
                //         West - Normal - 21; Heroic - 21
                //         East - Normal - 21; Heroic - 21
                //         West - Normal - 21; Heroic - 40
                float WestBlazingTalonInitiateFrequence = (i > 1 ? (17f + 22f + (63f + 21f) + (21f + 40f)) : (17f + 31f + (31f + 21f) + (21f + 21f))) / 4f;
                float EastBlazingTalonInitiateFrequence = (i > 1 ? (17f + 22f + 63f + (21f + 21f)) : (17f + 31f + 31f + (21f + 21f))) / 4f;
                TargetGroup BlazingTalonInitiateWest = new TargetGroup
                {
                    Name = "Blazing Talon Initiate West",
                    Chance = 1f,
                    Frequency = WestBlazingTalonInitiateFrequence,
                    Duration = WestBlazingTalonInitiateFrequence * 1000f,
                    LevelOfTargets = 87,
                    NearBoss = false,
                    NumTargs = 1f,
                    TargetID = 53896,
                };
                BlazingTalonInitiateWest.SetAffectsRoles_DPS();
                HatchlingPhase.Targets.Add(BlazingTalonInitiateWest);
                FlightofFlames.Targets.Add(BlazingTalonInitiateWest);
                FlightofFlamesPart2.Targets.Add(BlazingTalonInitiateWest);

                TargetGroup BlazingTalonInitiateEast = new TargetGroup
                {
                    Name = "Blazing Talon Initiate East",
                    Chance = 1f,
                    Frequency = EastBlazingTalonInitiateFrequence,
                    Duration = EastBlazingTalonInitiateFrequence * 1000f,
                    LevelOfTargets = 87,
                    NearBoss = false,
                    NumTargs = 1f,
                    TargetID = 53369,
                };
                BlazingTalonInitiateEast.SetAffectsRoles_DPS();
                HatchlingPhase.Targets.Add(BlazingTalonInitiateEast);
                FlightofFlames.Targets.Add(BlazingTalonInitiateEast);
                FlightofFlamesPart2.Targets.Add(BlazingTalonInitiateEast);

                #region Brushfire
                /* The Blazing Talon Initiate conjures a fiery ball that moves across the arena, dealing
                 * 27750 to 32250 damage every 1 sec to enemies within 0 yards.*/
                // Initiates are now stunnable so these can be "interupted" now
                // Original cast - http://ptr.wowhead.com/spell=98868
                // Summons - http://ptr.wowhead.com/npc=53372
                // 10 man - http://ptr.wowhead.com/spell=98885
                // 25 man - http://ptr.wowhead.com/spell=100715
                // 10 man heroic - http://ptr.wowhead.com/spell=100716
                // 25 man heroic - http://ptr.wowhead.com/spell=100717
                Attack Brushfire = new Attack
                {
                    Name = "Brushfire",
                    DamagePerHit = new float[] { (27750f + 32250f), (27750f + 32250f), (55500f + 64500f), (55500f + 64500f), 0f }[i],
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    MaxNumTargets = 1f,
                    Interruptable = true,
                    IsFromAnAdd = true,
                    SpellID = new float[] { 98885, 100715, 100716, 100717, 0 }[i],
                    AttackSpeed = new float[] { 4f, 4f, 8f, 8f, 0f }[i],
                };
                Brushfire.SetAffectsRoles_All();
                FlightofFlames.Attacks.Add(Brushfire);
                FlightofFlamesPart2.Attacks.Add(Brushfire);
                #endregion

                #region Fieroblast
                /* The Blazing Talon Initiate hurls a fiery boulder at an enemy, inflicting 27750 to 32250 Fire
                 * damage and 10000 Fire damage every 3 seconds for 12 sec.*/
                // After the September 20, 2011 nerf to Firelands in general, this is only cast on heroic
                // Cast after every Brushfire (Brushfire/Fieroblast/Burshfire/Fieroblast/etc)
                // These are interuptable
                // 10 man Heroic - http://ptr.wowhead.com/spell=101295
                // 25 man Heroic - http://ptr.wowhead.com/spell=101296
                if (i > 1 )
                {
                    Attack Fieroblast = new Attack
                    {
                        Name = "Fieroblast",
                        DamagePerHit = new float[] { 0, 0, (37000f + 43000f), (37000f + 43000f), 0f }[i],
                        DamagePerTick = new float[] { 0, 0, 20000, 20000, 0 }[i],
                        IsDoT = true,
                        TickInterval = 3f,
                        Duration = 12f,
                        AttackSpeed = new float[] { 0, 0, 8f, 8f, 0f }[i],
                        AttackType = ATTACK_TYPES.AT_DOT,
                        DamageType = ItemDamageType.Fire,
                        Interruptable = true,
                        IsFromAnAdd = true,
                        MaxNumTargets = 1f,
                    };
                    Fieroblast.SetAffectsRoles_All();
                    Fieroblast.AffectsRole[PLAYER_ROLES.AlysrazorAirGroup] = true;
                    FlightofFlames.Attacks.Add(Fieroblast);
                    FlightofFlamesPart2.Attacks.Add(Fieroblast);
                }
                #endregion
                #endregion

                #region Voracious Hatchling [Tank Note]
                /* Early in Stage 1, two Blazing Broodmothers drop off two Molten Eggs. After several seconds,
                 * the eggs hatch into Voracious Hatchlings. Voracious Hatchlings are indeed voracious and will
                 * throw a Tantrum if not fed Plump Lava Worms.*/
                // East Hatchling - http://ptr.wowhead.com/npc=53898
                // West hatchling - http://ptr.wowhead.com/npc=53509
                TargetGroup WestVoraciousHatchling = new TargetGroup
                {
                    Name = "West Voracious Hatchling",
                    Chance = 1f,
                    Frequency = (i > 1 ? FirestormCD : FlightofFlamesPhaseTime) - 1f,
                    Duration = ((i > 1 ? FirestormCD : FlightofFlamesPhaseTime) - 12f) * 1000f,
                    NearBoss = false,
                    NumTargs = 1f,
                    TargetID = 53509,
                    LevelOfTargets = 87,
                };
                WestVoraciousHatchling.AffectsRole[PLAYER_ROLES.MainTank] = true;
                if (i < 2)
                {
                    FlightofFlames.Targets.Add(WestVoraciousHatchling);
                    FlightofFlamesPart2.Targets.Add(WestVoraciousHatchling);
                }
                else
                    FireStormPhase.Targets.Add(WestVoraciousHatchling);

                TargetGroup EastVoraciousHatchling = WestVoraciousHatchling.Clone();
                EastVoraciousHatchling.Name = "East Voracious Hatchling";
                EastVoraciousHatchling.TargetID = 53509;
                EastVoraciousHatchling.AffectsRole[PLAYER_ROLES.MainTank] = false;
                EastVoraciousHatchling.AffectsRole[PLAYER_ROLES.OffTank] = true;
                if (i < 2)
                {
                    FlightofFlames.Targets.Add(EastVoraciousHatchling);
                    FlightofFlamesPart2.Targets.Add(EastVoraciousHatchling);
                }
                else
                    FireStormPhase.Targets.Add(EastVoraciousHatchling);

                Attack VoraciousHatchlingMelee = new Attack
                {
                    Name = "Voracious hatchling Melee",
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 1f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamageType = ItemDamageType.Physical,
                    IsFromAnAdd = true,
                    MaxNumTargets = 1f,
                    Missable = true,
                    Parryable = true,
                    Dodgable = true,
                    Blockable = true,
                    IsTheDefaultMelee = true,
                };
                VoraciousHatchlingMelee.SetAffectsRoles_Tanks();
                if (i < 2)
                {
                    FlightofFlames.Attacks.Add(VoraciousHatchlingMelee);
                    FlightofFlamesPart2.Attacks.Add(VoraciousHatchlingMelee);
                }
                else
                    FireStormPhase.Attacks.Add(VoraciousHatchlingMelee);

                #region Imprinted
                /* Upon hatchling, Voracious Hatchlings become imprinted on the nearest enemy. The hatchling
                 * will only attack that target, but the target gains 1000% additional damage against the
                 * hatchling.*/
                // http://ptr.wowhead.com/spell=99389; alt - http://ptr.wowhead.com/spell=100359
                WestVoraciousHatchling.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EastVoraciousHatchling.AffectsRole[PLAYER_ROLES.OffTank] = true;
                BuffState Imprint = new BuffState
                {
                    Name = "Imprint",
                    Frequency = WestVoraciousHatchling.Frequency,
                    Duration = WestVoraciousHatchling.Duration,
                    Chance = 1f,
                    Breakable = false,
                    Stats = new Stats() { BonusDamageMultiplier = 10f },
                };
                Imprint.SetAffectsRoles_Tanks();
                if (i < 2)
                {
                    FlightofFlames.BuffStates.Add(Imprint);
                    FlightofFlamesPart2.BuffStates.Add(Imprint);
                }
                else
                    FireStormPhase.BuffStates.Add(Imprint);
                #endregion

                #region Satiated
                /* The Voracious Hatchling will not throw a Tantrum when Satiated, which lasts for 15 sec.
                 * Voracious Hatchlings hatch fully Satiated, and can become Satiated again if they are fed
                 * Lava Worms.*/
                // 15 seconds
                // 10-man - http://ptr.wowhead.com/spell=99359
                // 25-man - http://ptr.wowhead.com/spell=100850
                // 10 seconds
                // 10-man heroic - http://ptr.wowhead.com/spell=100851
                // 25-man heroic - http://ptr.wowhead.com/spell=100852
                #endregion

                #region Hungry
                /* A Voracious Hatchling that is no longer Satiated becomes Hungry. When Hungry, hatchling
                 * have a 20% chance on hit to throw a Tantrum.*/
                // http://ptr.wowhead.com/spell=99361
                #endregion

                #region Tantrum
                // The Voracious Hatchling throws a Tantrum, increasing damage by 50% and haste by 50%.
                // http://ptr.wowhead.com/spell=99362
                // This should never happen on heroic
                Stats TantrumStats = new Stats();
                SpecialEffect TantrumSpecialEffect = new SpecialEffect(Trigger.DamageTaken, new Stats() { DamageTakenReductionMultiplier = -0.50f }, 10f, (i < 2 ? 35 : 25 ), 0.20f);
                TantrumStats.AddSpecialEffect(TantrumSpecialEffect);
                BuffState Tantrum = new BuffState
                {
                    Name = "Tantrum",
                    Chance = 1f,
                    Breakable = true,
                    Frequency = (i < 2 ? 30 : 20 ),
                    Duration = 10f * 1000f,
                    Stats = TantrumStats,
                };
                Tantrum.SetAffectsRoles_Tanks();
                if (i < 2)
                {
                    FlightofFlames.BuffStates.Add(Tantrum);
                    FlightofFlamesPart2.BuffStates.Add(Tantrum);
                }
                else
                    FireStormPhase.BuffStates.Add(Tantrum);
                #endregion

                #region Gushing Wound [Healer Note]
                /* The Voracious Hatchling strikes all targets within a 6-yard cone, causing them to bleed for
                 * 3000 Physical damage every 0.20 seconds or until the target's health falls below 50% of
                 * their maximum health.*/
                // 10 man - http://ptr.wowhead.com/spell=100024; alt - http://ptr.wowhead.com/spell=99308
                // 25 man - http://ptr.wowhead.com/spell=100721; alt - http://ptr.wowhead.com/spell=100718
                // 10 man heroic - http://ptr.wowhead.com/spell=100719; alt - http://ptr.wowhead.com/spell=100722
                // 25 man heroic - http://ptr.wowhead.com/spell=100723; alt - http://ptr.wowhead.com/spell=100720
                Attack GushingWound = new Attack
                {
                    Name = "Gushing Wound",
                    DamagePerTick = new float[] { 3000, 4000, 5000, 6000, 0 }[i],
                    TickInterval = 0.20f,
                    DamageType = ItemDamageType.Physical,
                    Duration = 60f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    AttackSpeed = 60f,
                    IsDoT = true,
                    IsFromAnAdd = true,
                    MaxNumTargets = 1f,
                };
                GushingWound.SetAffectsRoles_Tanks();
                if (i < 2)
                {
                    FlightofFlames.Attacks.Add(GushingWound);
                    FlightofFlamesPart2.Attacks.Add(GushingWound);
                }
                else
                    FireStormPhase.Attacks.Add(GushingWound);
                #endregion
                #endregion

                #region Plump Lava Worm [Tank Note]
                /* During Stage 1, two sets of four Lava Worms will erupt from the molten ground. Lava Worms
                 * cannot be attacked by players. Voracious Hatchlings that are near a Lava Worm will rush
                 * to devour it, becoming temporarily Satiated.*/
                // this means the tank tanking the Hatchlings need to move to position the hatchlings near the worms
                // when they need to be satiated.
                // http://ptr.wowhead.com/npc=53520
                TargetGroup PlumpLavaWorm = new TargetGroup
                {
                    Name = "Plump Lava Worm",
                    NumTargs = (i < 2f ? 8f : 4f),
                    // four worms spawn at any given time
                    // two worm will last for the full duration, while the first two will last half that
                    // so an average of 75% of the full duration for all of the worms.
                    Duration = WestVoraciousHatchling.Duration * 0.75f,
                    Frequency = WestVoraciousHatchling.Frequency,
                    Chance = 1f,
                    LevelOfTargets = 85,
                    NearBoss = false,
                    TargetID = 53520,
                };
                PlumpLavaWorm.SetAffectsRoles_Tanks();
                if (i < 2)
                {
                    FlightofFlames.Targets.Add(PlumpLavaWorm);
                    FlightofFlamesPart2.Targets.Add(PlumpLavaWorm);
                }
                else
                    FireStormPhase.Targets.Add(PlumpLavaWorm);

                #region Lava Spew
                /* Plump Lava Worms spew a molten cone of fire dealing 27750 to 32250 damage every 1 sec
                 * to all enemies within a 14-yard cone.*/
                // 10 man - http://ptr.wowhead.com/spell=99336
                // 25 man - http://ptr.wowhead.com/spell=100725
                // 10 man heroic - http://ptr.wowhead.com/spell=100726
                // 25 man heroic - http://ptr.wowhead.com/spell=100727
                Attack LavaSpew = new Attack
                {
                    Name = "Lava Spew",
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = new float[] { (27750f + 32250f), (27750f + 32250f), (55500f + 64500f), (55500f + 64500f), 0f }[i] / 2f,
                    AttackSpeed = 1f,
                    Interruptable = true,
                    IsFromAnAdd = true,
                    MaxNumTargets = Max_Players[i],
                    SpellID = new float[] { 99336, 100725, 100726, 100727, 0 }[i],
                };
                LavaSpew.SetAffectsRoles_All();
                if (i < 2)
                {
                    FlightofFlames.Attacks.Add(LavaSpew);
                    FlightofFlamesPart2.Attacks.Add(LavaSpew);
                }
                else
                    FireStormPhase.Attacks.Add(LavaSpew);
                #endregion
                #endregion

                #region Herald of the Burning End [Heroic Only]
                if (i > 1)
                {
                    /* During Stage 1, a Herald of the Burning End will periodically appear and begin casting
                     * Cataclysm. The Herald is immune to all damage, but will die once Cataclysm is cast.*/

                    #region Cataclysm [Heroic Only]
                    /* The Herald of the Burning End summons a powerful Molten Meteor, dealing 462500 to
                     * 537500 Flamestrike damage to enemies within 0 yards.*/
                    // NPC - http://ptr.wowhead.com/npc=53375
                    // You cannot target this harm this npc since this npc will deal a 50,000 AOE damage to anyone
                    // near him and is immune to all attacks. He disappears after casting Cataclysm.

                    #region Molten Meteor [Heroic Only]
                    /* After being summoned by the Herald of the Burning End, a Molten Meteor will roll in one
                     * of 8 random directions, dealing 462500 to 537500 Flamestrike damage to enemies within
                     * 0 yards every 1.50 sec.
                     * 10-man heroic - http://ptr.wowhead.com/npc=53489
                     * 25-man heroic - http://ptr.wowhead.com/npc=54563
                     * This should NEVER happen*/
                    TargetGroup MoltenMeteor = new TargetGroup
                    {
                        Name = "Molten Meteor",
                        // meteors lasts at most 9 seconds before they explode, at least one of theses needs to
                        // survive before a firestorm
                        Duration = 9f * 1000f,
                        Chance = 1f,
                        LevelOfTargets = 87,
                        TargetID = new float[] { 0, 0, 53489, 54563, 0 }[i],
                        NumTargs = 1f,
                        NearBoss = false,
                        Frequency = FirestormCD / 2f,
                    };
                    MoltenMeteor.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                    FireStormPhase.Targets.Add(MoltenMeteor);

                    /* If the meteor reaches a wall, it will break apart into three Molten Boulders, which
                     * ricochet back to the opposite direction. If it is destroyed before it reaches a wall, 
                     * the Molten Meteor becomes temporarily stationary and block line of sight.*/
                    // Explosion - http://ptr.wowhead.com/spell=99274
                    // Personal note, this is what is used to negate the damage from the 100,000 damage every second
                    // from Firestorm.
                    // Players should NEVER get hit by this.
                    #endregion

                    #region Molten Boulder [Heroic Only]
                    /* Three Molten Boulders form when a Molten Meteor hits a wall and breaks apart. Molten Boulder
                     * deals 29600 to 34400 Flamestrike damage to enemies within 2 yards every 1.50 sec and knock
                     * them back.*/
                    // NPC - http://db.mmo-champion.com/c/53496/
                    // Damage - http://ptr.wowhead.com/spell=99275
                    Impedance MoltenBoulder_Move = new Impedance
                    {
                        Name = "Molten Boulder",
                        Duration = 2f * 1000f,
                        Chance = 1f,
                        Frequency = MoltenMeteor.Frequency,
                        Breakable = false,
                    };
                    MoltenBoulder_Move.SetAffectsRoles_All();
                    FireStormPhase.Moves.Add(MoltenBoulder_Move);
                    #endregion
                }
                #endregion
                #endregion
                #endregion

                #region Ultimate Firepower
                /* Alysrazor flies in a tight circle, removing Wings of Flame from all players after 5 seconds,
                 * and begins her ultimate attack.*/

                // This lasts for 35 seconds

                #region Fiery Vortex
                /* A Fiery Vortex appears in the middle of the arena, dealing 100000 Fire damage every 0.50
                 * seconds to enemies within 0 yards.*/
                // http://ptr.wowhead.com/spell=99794
                // This should NEVER be hit
                #endregion

                #region Fiery Tornado
                /* Fiery Tornadoes erupt from the Fiery Vortex and begin moving rapidly around Alysrazor's arena,
                 * dealing 25000 Fire damage every 1 sec to enemies within 0 yards.*/
                // 10 man - http://ptr.wowhead.com/spell=99816
                // 25 man - http://ptr.wowhead.com/spell=100733
                // 10 man heroic - http://ptr.wowhead.com/spell=100734
                // 25 man heroic - http://ptr.wowhead.com/spell=100735
                // These should never be hit
                /*Attack FieryTornado = new Attack
                {
                    Name = "Fiery Tornado",
                    DamagePerHit = new float[] { 20000, 25000, 60000, 70000, 0 }[i],
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    AttackSpeed = 1f,
                    MaxNumTargets = 1f,
                    Interruptable = true,
                    SpellID = new float[] { 99816, 100733, 100734, 100735, 0 }[i],
                };
                FieryTornado.SetAffectsRoles_All();
                FieryTornado.AffectsRole[PLAYER_ROLES.AlysrazorAirGroup] = true;
                 */
                #endregion

                #region Blazing Power
                /* Alysrazor continues to give off rings of fire, which appear on the ground of the arena and
                 * lasts for 3 seconds. Players who pass through the ring gain Blazing Power, which increase haste
                 * by 8% and stacks up to 25 times. In addition, each stack of Blazing Power restores mana, rage,
                 * energy, runic power, and holy power.*/
                // http://ptr.wowhead.com/spell=99461
                // Restores 5% of mana, rage, energy, runic power, and holy power.
                BuffState BlazingPowerPhase2 = new BuffState
                {
                    Name = "Blazing Power Phase 2",
                    Frequency = FieryTornadoDuration,
                    Duration = FieryTornadoDuration * 1000f,
                    Chance = 1f,
                    Breakable = false,
                    Stats = BlazingPowerStats,
                };
                BlazingPowerPhase2.SetAffectsRoles_All();
                UltimateFirepower.BuffStates.Add(BlazingPowerPhase2);
                
                // Assume the air group has 25 stacks at this point and still refreshing during Tornadoes
                BuffState BlazingPowerPhase2AirGroup = new BuffState
                {
                    Name = "Blazing Power Phase 2 Air Group",
                    Frequency = FieryTornadoDuration,
                    Duration = FieryTornadoDuration * 1000f,
                    Chance = 1f,
                    Breakable = false,
                    Stats = new Stats() { SpellHaste = 2f, PhysicalHaste = 2f, CritRating = 0.75f },
                };
                BlazingPowerPhase2.AffectsRole[PLAYER_ROLES.AlysrazorAirGroup] = true;
                UltimateFirepower.BuffStates.Add(BlazingPowerPhase2AirGroup);
                #endregion
                #endregion

                #region Burnout [DPS Note]
                /* Alysrazor crashes to the ground and becomes vulnerable, with 0 Molten Power. This stage lasts
                 * until Alyrazor's energy bar reaches 50 Molten Power.*/
                #region Blazing Power
                BuffState BlazingPowerPhase3 = new BuffState
                {
                    Name = "Blazing Power Phase 3",
                    Frequency = BurnoutDuration,
                    Duration = BurnoutDuration * 1000f,
                    Chance = 1f,
                    Breakable = false,
                    Stats = new Stats() { SpellHaste = 0.08f * 7f, PhysicalHaste = 0.08f * 7f },
                };
                BlazingPowerPhase3.SetAffectsRoles_All();
                Burnout.BuffStates.Add(BlazingPowerPhase3);
                
                BuffState BlazingPowerPhase3AirGroup = BlazingPowerPhase2AirGroup.Clone();
                BlazingPowerPhase3AirGroup.Name = "Blazing Power Phase 3 Air Group";
                BlazingPowerPhase3AirGroup.Frequency = BurnoutDuration;
                BlazingPowerPhase3AirGroup.Duration = BurnoutDuration * 1000f;
                Burnout.BuffStates.Add(BlazingPowerPhase3AirGroup);
                #endregion

                #region Burnout
                /* Alysrazor's fire burns out, causing her to become immobile and increasing damage taken by 100%.
                 * In addition, when struck with a harmful spell, Alysrazor emits Essence of the Green.*/
                // http://ptr.wowhead.com/spell=99432
                BuffState BurnoutBuffState = new BuffState
                {
                    Name = "Burnout DeBuff",
                    Breakable = false,
                    Chance = 1f,
                    Duration = BurnoutDuration * 1000f,
                    Frequency = BurnoutDuration,
                    Stats = new Stats() { BonusDamageMultiplier = 1f },
                };
                BurnoutBuffState.SetAffectsRoles_DPS();
                BurnoutBuffState.SetAffectsRoles_Healers();
                Burnout.BuffStates.Add(BurnoutBuffState);
                #endregion

                #region Essence of the Green [Healer Note]
                /* During Burnout, if Alysrazor is struck by a harmful spell, she emits Essence of the Green
                 * restoring 10% of maximum mana to players.*/
                // http://ptr.wowhead.com/spell=99433
                Stats EssenceoftheGreenStats = new Stats();
                SpecialEffect EssenceoftheGreenSpecialEffect = new SpecialEffect(Trigger.DamageSpellHit, new Stats() { ManaRestoreFromMaxManaPerSecond = 0.10f }, 0, 0, 1f);
                EssenceoftheGreenStats.AddSpecialEffect(EssenceoftheGreenSpecialEffect);
                BuffState EssenceoftheGreen = new BuffState
                {
                    Name = "Essence of the Green",
                    Breakable = false,
                    Chance = 1f,
                    Duration = BurnoutDuration * 1000f,
                    Frequency = BurnoutDuration,
                    Stats = EssenceoftheGreenStats,
                };
                EssenceoftheGreen.SetAffectsRoles_DPS();
                EssenceoftheGreen.SetAffectsRoles_Healers();
                Burnout.BuffStates.Add(EssenceoftheGreen);
                #endregion

                #region Spark
                /* A bright spark continues to burn within the heart of Alyrazor, restoring 3 Molten Power
                 * every 2 seconds.*/
                // http://ptr.wowhead.com/spell=99921
                #endregion

                #region Blazing Talon Clawshaper
                /* At the start of stage 2, two Blazing Talon Clawshapers will fly in and begin to re-energize
                 * Alysrazor.*/
                // http://ptr.wowhead.com/npc=53734
                TargetGroup BlazingTalonClawshaper = new TargetGroup
                {
                    Name = "Blazing Talon Clawshaper",
                    Chance = 1f,
                    Duration = BurnoutDuration * 1000f,
                    Frequency = BurnoutDuration,
                    // Originally they are not near the boss but they can be pulled/kited to next to the boss,
                    NearBoss = true,
                    NumTargs = 2f,
                    TargetID = 53734,
                    LevelOfTargets = 86,
                };
                BlazingTalonClawshaper.SetAffectsRoles_Tanks();
                Burnout.Targets.Add(BlazingTalonClawshaper);

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

                #region Ignited
                /* Alysrazor's fiery core begins to combust once again, rapidly restoring Molten Power. Restores
                 * 3 Molten Power every 1 seconds.*/
                // http://ptr.wowhead.com/spell=99922
                // Personal Note - This means that the phase will last 17 seconds
                #endregion

                #region Blazing Claw [Tank Note]
                /* Alysrazor claws her enemies, dealing 92500 to 107500 Physical damage to enemies in a 25-yard cone
                 * every 1.50 seconds. In addition, each swipe increases the Fire and Physical damage taken by the
                 * target by 10% for 15 sec.*/
                // 10-man - http://ptr.wowhead.com/spell=99844
                // 25-man - http://ptr.wowhead.com/spell=101729
                // 10-man Heroic - http://ptr.wowhead.com/spell=101730
                // 25-man Heroic - http://ptr.wowhead.com/spell=101731
                // Personal Note, this means that there needs to be a tank swap at the 5-6 stack mark
                Attack BlazingClaw = new Attack
                {
                    Name = "Blazing Claw",
                    DamagePerHit = new float[] { (69375f + 80625f), (83250f + 96750f), (92500f + 107500f), (115625f + 134375f), 0 }[i] / 2f,
                    AttackSpeed = 1.5f * 2,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamageType = ItemDamageType.Physical,
                    MaxNumTargets = 1f,
                    SpellID = new float[] { 99844, 101729, 101730, 101731, 0 }[i],
                };
                BlazingClaw.SetUnavoidable();
                BlazingClaw.SetAffectsRoles_Tanks();
                ReIgnition.Attacks.Add(BlazingClaw);

                Stats BlazingClawStats = new Stats();
                SpecialEffect BlazingClawSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { BossPhysicalDamageDealtReductionMultiplier = -0.10f, FireDamageTakenMultiplier = 0.10f }, 15, 1.5f, 0.5f, 11);
                BlazingClawStats.AddSpecialEffect(BlazingClawSpecialEffect);
                BuffState BlazingClawBuffState = new BuffState
                {
                    Name = "Blazing Claw Debuff",
                    Frequency = ReIgniteDuration,
                    Duration = ReIgniteDuration * 1000f,
                    Chance = 1f,
                    Breakable = false,
                    Stats = BlazingClawStats,
                };
                BlazingClawBuffState.SetAffectsRoles_Tanks();
                ReIgnition.BuffStates.Add(BlazingClawBuffState);
                #endregion

                #region Blazing Buffet
                /* Alysrazor's Fiery core emits powerful bursts of flame, dealing 9250 to 10750 Fire damage to all
                 * enemies every 1 seconds for as long as Alysrazor remains ignited.*/
                // 10 man - http://ptr.wowhead.com/spell=99757
                // 25 man - http://ptr.wowhead.com/spell=100739
                // 10 man heroic - http://ptr.wowhead.com/spell=100740
                // 25 man heroic - http://ptr.wowhead.com/spell=100741
                Attack BlazingBuffet = new Attack
                {
                    Name = "Blazing Buffet",
                    DamagePerHit = new float[] { (6937f + 8062f), (10406f + 12093f), (11793f + 13706f), (11793f + 13706f), 0 }[i] / 2f,
                    AttackSpeed = 1f,
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    MaxNumTargets = Max_Players[i],
                    SpellID = new float[] { 99757, 100739, 100740, 100741, 0 }[i],
                };
                BlazingBuffet.SetAffectsRoles_All();
                ReIgnition.Attacks.Add(BlazingBuffet);
                #endregion

                #region Full Power [Healer Note]
                /* When Alysrazor reaches 100 Molten Power, she is at Full Power, which deals 50000 Fire damage
                 * to all enemies and knocks them back. Once she reaches Full Power, Alysrazor will begin her
                 * Stage 1 activities once again.*/
                // 10 man - http://ptr.wowhead.com/spell=99925
                // 25 man - http://ptr.wowhead.com/spell=100736
                // 10 man heroic - http://ptr.wowhead.com/spell=100737
                // 25 man heroic - http://ptr.wowhead.com/spell=100738
                Attack FullPower = new Attack
                {
                    Name = "Full Power",
                    DamagePerHit = new float[] { 37500, 45000, 60000, 70000, 0 }[i],
                    AttackSpeed = ReIgniteDuration - 1,
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    MaxNumTargets = Max_Players[i],
                    SpellID = new float[] { 99925, 100736, 100737, 100738, 0 }[i],
                };
                FullPower.SetAffectsRoles_All();
                ReIgnition.Attacks.Add(FullPower);
                #endregion
                #endregion

                #region Apply Phases
                // Pull then at 30 Sec, Empower Blade w/ 50/50 chance for which blade type.
                // 15 secs of empowered blade
                // Return to normal mode.
                float phasestart = 0;
                float HeroicFofStart = 0;
                float HeroicFoFLength = 0;
                if (i < 2)
                {
                    ApplyAPhasesValues(InitialPull, i, 1, 0, 10f, BerserkTimer[i]); phasestart += 10f;

                    ApplyAPhasesValues(FlightofFlames, i, 2, phasestart, FlightofFlamesPhaseTime, BerserkTimer[i]); phasestart += FlightofFlamesPhaseTime;
                    ApplyAPhasesValues(UltimateFirepower, i, 3, phasestart, FieryTornadoDuration, BerserkTimer[i]); phasestart += FieryTornadoDuration;
                    ApplyAPhasesValues(Burnout, i, 4, phasestart, BurnoutDuration, BerserkTimer[i]); phasestart += BurnoutDuration;
                    ApplyAPhasesValues(ReIgnition, i, 5, phasestart, ReIgniteDuration, BerserkTimer[i]); phasestart += ReIgniteDuration;

                    ApplyAPhasesValues(FlightofFlames, i, 6, phasestart, FlightofFlamesPhaseTime, BerserkTimer[i]);
                    ApplyAPhasesValues(UltimateFirepower, i, 7, phasestart, FieryTornadoDuration, BerserkTimer[i]); phasestart += FieryTornadoDuration;
                    ApplyAPhasesValues(Burnout, i, 8, phasestart, BurnoutDuration, BerserkTimer[i]); phasestart += BurnoutDuration;
                    ApplyAPhasesValues(ReIgnition, i, 9, phasestart, ReIgniteDuration, BerserkTimer[i]); phasestart += ReIgniteDuration;

                    ApplyAPhasesValues(FlightofFlames, i, 10, phasestart, FlightofFlamesPhaseTime, BerserkTimer[i]); phasestart += FlightofFlamesPhaseTime;
                    ApplyAPhasesValues(UltimateFirepower, i, 11, phasestart, FieryTornadoDuration, BerserkTimer[i]); phasestart += FieryTornadoDuration;
                    ApplyAPhasesValues(Burnout, i, 12, phasestart, BurnoutDuration, BerserkTimer[i]); phasestart += BurnoutDuration;
                    ApplyAPhasesValues(ReIgnition, i, 13, phasestart, ReIgniteDuration, BerserkTimer[i]); // She will stay in this phase until the raid wipes;
                }
                else
                {
                    ApplyAPhasesValues(InitialPull, i, 1, 0, 10f, BerserkTimer[i]); phasestart += 10f;

                    HeroicFofStart = phasestart;
                    InnerPhaseHatchling = new InnerPhase(HatchlingPhase, i, 1, phasestart, FirestormCD, BerserkTimer[i]); phasestart += FirestormCD; HeroicFoFLength += FirestormCD;
                    FlightofFlames.InnerPhases.Add(InnerPhaseHatchling);
                    InnerPhaseFirestorm = new InnerPhase(FireStormPhase, i, 1, phasestart, 10f, BerserkTimer[i]); phasestart += 10f; HeroicFoFLength += 10f;
                    FlightofFlames.InnerPhases.Add(InnerPhaseFirestorm);
                    InnerPhaseHatchling = new InnerPhase(HatchlingPhase, i, 1, phasestart, FirestormCD, BerserkTimer[i]); phasestart += FirestormCD; HeroicFoFLength += FirestormCD;
                    FlightofFlames.InnerPhases.Add(InnerPhaseHatchling);
                    InnerPhaseFirestorm = new InnerPhase(FireStormPhase, i, 1, phasestart, 10f, BerserkTimer[i]); phasestart += 10f; HeroicFoFLength += 10f;
                    FlightofFlames.InnerPhases.Add(InnerPhaseFirestorm);
                    InnerPhaseHatchling = new InnerPhase(HatchlingPhase, i, 1, phasestart, FlightofFlamesPhaseTime - HeroicFoFLength, BerserkTimer[i]); phasestart += (FlightofFlamesPhaseTime - HeroicFoFLength); HeroicFoFLength = 0;
                    FlightofFlames.InnerPhases.Add(InnerPhaseHatchling);
                    ApplyAPhasesValues(FlightofFlames, i, 2, HeroicFofStart, FlightofFlamesPhaseTime, BerserkTimer[i]); phasestart += FlightofFlamesPhaseTime;
                    ApplyAPhasesValues(UltimateFirepower, i, 3, phasestart, FieryTornadoDuration, BerserkTimer[i]); phasestart += FieryTornadoDuration;
                    ApplyAPhasesValues(Burnout, i, 4, phasestart, BurnoutDuration, BerserkTimer[i]); phasestart += BurnoutDuration;
                    ApplyAPhasesValues(ReIgnition, i, 5, phasestart, ReIgniteDuration, BerserkTimer[i]); phasestart += ReIgniteDuration;

                    HeroicFofStart = phasestart;
                    InnerPhaseHatchling = new InnerPhase(HatchlingPhase, i, 6, phasestart, FirestormCD, BerserkTimer[i]); phasestart += FirestormCD; HeroicFoFLength += FirestormCD;
                    FlightofFlamesPart2.InnerPhases.Add(InnerPhaseHatchling);
                    InnerPhaseFirestorm = new InnerPhase(FireStormPhase, i, 6, phasestart, 10f, BerserkTimer[i]); phasestart += 10f; HeroicFoFLength += 10f;
                    FlightofFlamesPart2.InnerPhases.Add(InnerPhaseFirestorm);
                    InnerPhaseHatchling = new InnerPhase(HatchlingPhase, i, 6, phasestart, FirestormCD, BerserkTimer[i]); phasestart += FirestormCD; HeroicFoFLength += FirestormCD;
                    FlightofFlamesPart2.InnerPhases.Add(InnerPhaseHatchling);
                    InnerPhaseFirestorm = new InnerPhase(FireStormPhase, i, 6, phasestart, 10f, BerserkTimer[i]); phasestart += 10f; HeroicFoFLength += 10f;
                    FlightofFlamesPart2.InnerPhases.Add(InnerPhaseFirestorm);
                    InnerPhaseHatchling = new InnerPhase(HatchlingPhase, i, 6, phasestart, FlightofFlamesPhaseTime - HeroicFoFLength, BerserkTimer[i]); phasestart += (FlightofFlamesPhaseTime - HeroicFoFLength); HeroicFoFLength = 0;
                    FlightofFlamesPart2.InnerPhases.Add(InnerPhaseHatchling);
                    ApplyAPhasesValues(FlightofFlamesPart2, i, 6, HeroicFofStart, FlightofFlamesPhaseTime, BerserkTimer[i]); phasestart += FlightofFlamesPhaseTime;
                    ApplyAPhasesValues(UltimateFirepower, i, 7, phasestart, FieryTornadoDuration, BerserkTimer[i]); phasestart += FieryTornadoDuration;
                    ApplyAPhasesValues(Burnout, i, 8, phasestart, BurnoutDuration, BerserkTimer[i]); phasestart += BurnoutDuration;
                    ApplyAPhasesValues(ReIgnition, i, 9, phasestart, BerserkTimer[i] - phasestart, BerserkTimer[i]); // She will stay in this phase until the raid wipes;
                }
                AddAPhase(InitialPull, i);
                AddAPhase(FlightofFlames, i);
                AddAPhase(UltimateFirepower, i);
                AddAPhase(Burnout, i);
                AddAPhase(ReIgnition, i);
                #endregion
            }
            #endregion
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Frost = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Fire = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Nature = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Arcane = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Shadow = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Holy = new double[] { 0.00f, 0.00f, 0, 0, 0 };
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
            /* TODO:
             * All
             */
        }
    }

    public class Shannox : MultiDiffBoss
    {
        public Shannox()
        {
            /* Shannox can be done with 1 tank on Normal with the main tank tanking Riplimb at the same time as
             * Shannox. However for consistency between Normal and Heroic, the Boss Handler will assume the use
             * of the two tank strat.*/
            #region Info
            Name = "Shannox";
            Instance = "Firelands";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, BossHandler.TierLevels.T12_LFR };
            #endregion
            #region Basics
            // Baleroc's health consists of both himself and his dogs Ripgut and Rageface
            // On heroic mode, you do not kill the dogs. Instead you burn Shannox instead
            Health = new float[] { (20442296f + (7901028f * 2f)), (61198050f + (25252248f * 2f)), 28619216f, 97100904f, 0 };
            MobType = (int)MOB_TYPES.HUMANOID;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase EntireFight = new Phase() { Name = "Entire Fight" };
                Phase TwoDogsUp = new Phase() { Name = "Two Dogs Up" };
                Phase OneDogUp = new Phase() { Name = "One Dog Up" };
                Phase Before30Pct = new Phase() { Name = "Before Shannox reaches 30% Health" };
                Phase After30Pct = new Phase() { Name = "After Shannox reaches 30% Health" };

                #region Shannox
                Attack BasicMeleeMT = new Attack
                {
                    Name = "Melee Shannox",
                    AttackSpeed = 1.4f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * (i < 2 ? .5f : 1f),
                    Missable = true,
                    Dodgable = true,
                    Parryable = true,
                    Blockable = true,
                    IsTheDefaultMelee=true,
                };
                BasicMeleeMT.AffectsRole[PLAYER_ROLES.MainTank] = true;
                TwoDogsUp.Attacks.Add(BasicMeleeMT);
                
                // With just one dog up, his attack damage goes up by 15%
                Attack BasicMeleeMT1DU = BasicMeleeMT.Clone();
                BasicMeleeMT1DU.DamagePerHit *= 1.15f;
                OneDogUp.Attacks.Add(BasicMeleeMT1DU);

                #region Frenzy
                /* When both of Shannox's hounds are defeated, he goes into a frenzy, increasing physical damage
                 * and attack speed by 30. Shannox no longer uses Hurl Spear after this point and instead drives his
                 * spear directly into the ground to trigger the same cascade of molten eruptions around the impact
                 * point, which deal 61156 to 67594 Fire damage to enemies that are stand in them.*/
                // http://ptr.wowhead.com/spell=100522

                // If both of his dogs die, his damage goes up by 30%
                // Does not happen on Heroic
                Attack BasicMeleeMT0DU = BasicMeleeMT.Clone();
                BasicMeleeMT0DU.DamagePerHit *= 1.30f;
                if (i < 2)
                    After30Pct.Attacks.Add(BasicMeleeMT0DU);
                else
                    After30Pct.Attacks.Add(BasicMeleeMT);
                #endregion

                #region Arcing Slash
                /* Shannox causes 125% of normal melee damage in a wide arc up to 1 yards in front of him, and
                 * inflicts Jagged Tear on those he strikes.*/
                // http://ptr.wowhead.com/spell=99931
                Attack ArcingSlash = new Attack
                {
                    Name = "Arcing Slash",
                    AttackSpeed = 12.5f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BasicMeleeMT.DamagePerHit * 1.25f,
                    SpellID = 99931,
                };
                ArcingSlash.AffectsRole[PLAYER_ROLES.MainTank] = true;
                TwoDogsUp.Attacks.Add(ArcingSlash);

                Attack ArcingSlash1DU = ArcingSlash.Clone();
                ArcingSlash1DU.DamagePerHit = BasicMeleeMT1DU.DamagePerHit * 1.25f;
                OneDogUp.Attacks.Add(ArcingSlash1DU);

                Attack ArcingSlash0DU = ArcingSlash.Clone();
                ArcingSlash0DU.DamagePerHit = BasicMeleeMT0DU.DamagePerHit * 1.25f;
                if (i < 2)
                    After30Pct.Attacks.Add(ArcingSlash0DU);
                else
                    After30Pct.Attacks.Add(ArcingSlash);

                #region Jagged Tear
                /* Shannox's Arcing Slash leaves a Jagged Tear that deals 3000 physical damage every 3 sec for 30
                 * sec. Stacks.*/
                // http://ptr.wowhead.com/spell=99937
                Attack JaggedTear = new Attack
                {
                    Name = "Jagged Tear MT",
                    AttackSpeed = 10f,
                    AttackType = ATTACK_TYPES.AT_DOT,
                    IsDoT = true,
                    SpellID = 99937,
                    DamagePerTick = 3000,
                    TickInterval = 3,
                    Duration = 30,
                };
                JaggedTear.AffectsRole[PLAYER_ROLES.MainTank] = true;
                JaggedTear.AffectsRole[PLAYER_ROLES.OffTank] = false;
                Before30Pct.Attacks.Add(JaggedTear);
                After30Pct.Attacks.Add(JaggedTear);
                #endregion
                #endregion

                #region Hurl Spear
                /* Shannox hurls his spear in the direction of his hound, Riplimb. The spear deals 117000 to
                 * 123000 physical damage to anyone it strikes directly as well as 59546 to 69204 Fire damage to
                 * all enemies within 50 yards. The spear strike also triggers a cascade of molten eruptions around the
                 * impact point which deal 61156 to 67594 Fire damage to enemies that are caught in them.
                 * 
                 * Riplimb will then break off from combat, fetch the spear, and return it to Shannox.*/
                // http://ptr.wowhead.com/spell=100002
                // Should NEVER get hit by this, one should move away from the landing spot
                /*Attack HurlSpear = new Attack
                {
                    Name = "Hurl Spear",
                    AttackSpeed = 25f, // Timing is a rough estimate currently.
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = (99450f + 104550f) / 2f,
                    MaxNumTargets = Max_Players[i],
                    DamageType = ItemDamageType.Physical,
                    SpellID = 100002,
                };*/
                Impedance HurlSpear_Move = new Impedance
                {
                    Name = "Hurl Spear Move",
                    Breakable = false,
                    Chance = 1f,
                    Frequency = 20f,
                    Duration = 2f * 1000f,
                };
                HurlSpear_Move.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                HurlSpear_Move.SetAffectsRoles_Healers();
                HurlSpear_Move.AffectsRole[PLAYER_ROLES.OffTank] = true;
                Before30Pct.Moves.Add(HurlSpear_Move);

                #region Magma Flare
                // The Initial 40% increase to fire damage, as well as the initial damage should never be aplied, but the residual damage should
                // 10-man - http://ptr.wowhead.com/spell=99842
                // 25-man - http://ptr.wowhead.com/spell=101205
                // 10-man Heroic - http://ptr.wowhead.com/spell=101206
                // 25-man heroic - http://ptr.wowhead.com/spell=101207
                /*Attack MagmaRapture = new Attack
                {
                    Name = "Magma Rapture",
                    AttackSpeed = 15f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = new float[] { (64600f + 71400f), (64600f + 71400f), (100937f + 111562f), (100937f + 111562f), 0f }[i],
                    DamageType = ItemDamageType.Fire,
                    MaxNumTargets = 1f,
                    SpellID = new float[] { 99842, 101205, 101206, 101207, 0 }[i],
                };
                 */
                // http://ptr.wowhead.com/spell=100495
                Attack MagmaFlare = new Attack
                {
                    Name = "Magma Flare",
                    AttackSpeed = 45f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = (39312f + 45687f) / 2f,
                    DamageType = ItemDamageType.Fire,
                    MaxNumTargets = Max_Players[i],
                    SpellID = 100495,
                };
                MagmaFlare.SetUnavoidable();
                MagmaFlare.SetAffectsRoles_All();
                Before30Pct.Attacks.Add(MagmaFlare);
                After30Pct.Attacks.Add(MagmaFlare);
                #endregion
                #endregion
                #endregion

                #region Riplimb
                /* Shannox has two hounds. Riplimb will attack the target with the most threat.
                 * *Warning* In Heroic Difficulty, Riplimb cannot be permanently slain while his master lives. When
                 * his health reaches zero, he will collapse for up to 30 seconds, and then reanimate at full health to
                 * resume fighting.*/
                // http://db.mmo-champion.com/c/53694/
                TargetGroup Riplimb = new TargetGroup 
                { 
                    Name= "Riplimb",
                    // On normal, Riplimb should be the second dog to die, not the first
                    // Heroic, Riplimb stays alive the entire fight
                    Duration = BerserkTimer[i] * (i < 2 ? 0.7f : 1f) * 1000f,
                    Frequency = 1,
                    LevelOfTargets = 88,
                    NearBoss = false,
                    NumTargs = 1,
                    TargetID = 53694,
                };
                Riplimb.AffectsRole[PLAYER_ROLES.OffTank] = true;
                Before30Pct.Targets.Add(Riplimb);
                if (i > 1)
                    After30Pct.Targets.Add(Riplimb);

                #region Melee
                Attack BasicMeleeOT = new Attack
                {
                    Name = "Melee Riplimb",
                    AttackSpeed = 1.4f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 0.30f,
                    Missable = true,
                    Dodgable = true,
                    Parryable = true,
                    Blockable = true,
                };
                BasicMeleeOT.AffectsRole[PLAYER_ROLES.MainTank] = false;
                BasicMeleeOT.AffectsRole[PLAYER_ROLES.OffTank] = true;
                Before30Pct.Attacks.Add(BasicMeleeOT);
                if (i > 1)
                    After30Pct.Attacks.Add(BasicMeleeOT);
                #endregion

                #region Limb Rip
                /* Riplimb bites savagely, dealing 175% of normal melee damage to an enemy and inflicting
                 * Jagged Tear on those he strikes.*/
                // http://ptr.wowhead.com/spell=99832
                Attack LimbRipOT = new Attack
                {
                    Name = "Limb Rip",
                    AttackSpeed = 10f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BasicMeleeOT.DamagePerHit * 1.75f, // Heroic values are unkonwn at this point.
                    Missable = false,
                    Dodgable = false,
                    Parryable = false,
                    Blockable = false,
                };
                LimbRipOT.AffectsRole[PLAYER_ROLES.MainTank] = false;
                LimbRipOT.AffectsRole[PLAYER_ROLES.OffTank] = true;
                Before30Pct.Attacks.Add(LimbRipOT);
                if (i > 1)
                    After30Pct.Attacks.Add(LimbRipOT);
                #endregion

                #region Jagged Tear
                /* Riplimb's Limb Rip leaves a Jagged Tear that deals 3000 physical damage every 3 sec for 30
                 * sec. Stacks.*/
                // http://ptr.wowhead.com/spell=99937
                Attack JaggedTearRL = JaggedTear.Clone();
                JaggedTearRL.Name = "Jagged Tear OT";
                JaggedTearRL.AttackSpeed = LimbRipOT.AttackSpeed;
                JaggedTearRL.AffectsRole[PLAYER_ROLES.MainTank] = false;
                JaggedTearRL.AffectsRole[PLAYER_ROLES.OffTank] = true;
                Before30Pct.Attacks.Add(JaggedTearRL);
                if (i > 1)
                    After30Pct.Attacks.Add(JaggedTearRL);
                #endregion

                #region Frenzied Devotion
                /* Riplimb goes into an unstoppable rage if he is alive to sitness Shannox's health reach 30%.
                 * This effect increases damage dealth by 200% and attack and movement speed by 100%.*/
                // http://ptr.wowhead.com/spell=100064
                // Only happens on Normal
                // Never should happen
                #endregion

                #region Feeding Frenzy [Heroic Only]
                /* Riplimb's successful melee attacks grant a stacking 10% bonus to physical damage dealt
                 * for 20sec.*/
                // http://ptr.wowhead.com/spell=100656
                // Never should last more than 1 minute stacks
                Stats FeedingFrenzyStats = new Stats();
                SpecialEffect FeedingFrenzySpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { PhysicalDamageTakenReductionMultiplier = -0.1f }, 20f, 1.2f, 1, 200);
                FeedingFrenzyStats.AddSpecialEffect(FeedingFrenzySpecialEffect);
                FeedingFrenzySpecialEffect = new SpecialEffect(Trigger.Use, FeedingFrenzyStats, 60f, 60f);
                FeedingFrenzyStats = new Stats();
                FeedingFrenzyStats.AddSpecialEffect(FeedingFrenzySpecialEffect);
                if (i > 1) // Heroic only
                {
                    BuffState FeedingFrenzyRiplimb = new BuffState
                    {
                        Name = "Feeding Frenzy Riplimb",
                        Breakable = false,
                        Chance = 1,
                        Duration = 20 * 1000,
                        Frequency = BasicMeleeOT.AttackSpeed,
                        Stats = FeedingFrenzyStats,
                    };
                    FeedingFrenzyRiplimb.AffectsRole[PLAYER_ROLES.OffTank] = true;
                    Before30Pct.BuffStates.Add(FeedingFrenzyRiplimb);
                    After30Pct.BuffStates.Add(FeedingFrenzyRiplimb);
                }
                #endregion
                #endregion

                #region Rageface
                /* Shannox has two hounds, Rageface cannot be controlled, and will dart about from enemy to 
                 * enemy, changing targets periodically.*/
                // http://db.mmo-champion.com/c/53695/

                TargetGroup Rageface = new TargetGroup
                {
                    Name = "Rageface",
                    // One normal, Rageface should be the first dog to die
                    // Heroic Rageface stays alive the entire fight
                    Duration = BerserkTimer[i] * (i < 2 ? .35f : 1f) * 1000f,
                    Frequency = 1,
                    LevelOfTargets = 88,
                    NearBoss = false,
                    NumTargs = 1,
                    TargetID = 53695,
                };
                Rageface.SetAffectsRoles_DPS();
                Rageface.SetAffectsRoles_Healers();
                if (i < 2)
                    TwoDogsUp.Targets.Add(Rageface);
                else
                {
                    Before30Pct.Targets.Add(Rageface);
                    After30Pct.Targets.Add(Rageface);
                }

                #region Melee
                Attack RagefaceMelee = new Attack
                {
                    Name = "Rageface Melee",
                    AttackSpeed = 1.2f,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * 0.1f,
                    DamageType = ItemDamageType.Physical,
                    MaxNumTargets = 1f,
                    IsFromAnAdd = true,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                };
                RagefaceMelee.SetAffectsRoles_DPS();
                RagefaceMelee.SetAffectsRoles_Healers();
                if (i < 2)
                    TwoDogsUp.Attacks.Add(RagefaceMelee);
                else
                {
                    Before30Pct.Attacks.Add(RagefaceMelee);
                    After30Pct.Attacks.Add(RagefaceMelee);
                }
                #endregion

                #region Face Rage
                /* Rageface leaps at a random target, stunning and knockign them to the ground, and bgins to
                 * viciously claw at them. This mauling initially deals 8000 physical damage every 0.50 sec, but
                 * the damage dealt increases over time. While so occupied, Rageface is 1000% more susceptible to
                 * critical strikes.
                 * Rageface will continue until his target is dead, or he receives a single attack that deals at least
                 * 40000 damage.*/
                // Attack - http://ptr.wowhead.com/spell=99947
                // Get off me - http://ptr.wowhead.com/spell=100129
                Attack FaceRage = new Attack
                {
                    Name = "Face Rage",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamageType = ItemDamageType.Physical,
                    AttackSpeed = 15f,
                    DamagePerHit = 30000f,
                    DamagePerTick = 8000f,
                    IsDoT = true,
                    TickInterval = 0.5f,
                    Duration = 30f,
                    Interruptable = true,
                };
                FaceRage.SetAffectsRoles_DPS();
                FaceRage.SetAffectsRoles_Healers();
                if (i < 2)
                    TwoDogsUp.Attacks.Add(FaceRage);
                else
                {
                    Before30Pct.Attacks.Add(FaceRage);
                    After30Pct.Attacks.Add(FaceRage);
                }

                BuffState FaceRageBuff = new BuffState
                {
                    Name = "Face Rage Crit Buff",
                    Frequency = 15f,
                    Duration = 30f * 1000f,
                    Chance = 1f,
                    Breakable = true,
                    Stats = new Stats() { SpellCrit = 10f, PhysicalCrit = 10f },
                };
                FaceRageBuff.SetAffectsRoles_DPS();
                if (i < 2)
                    TwoDogsUp.BuffStates.Add(FaceRageBuff);
                else
                {
                    Before30Pct.BuffStates.Add(FaceRageBuff);
                    After30Pct.BuffStates.Add(FaceRageBuff);
                }
                #endregion

                #region Feeding Frenzy [Heroic Only]
                /* Rageface's successful melee attacks grant a stacking 10% bonus to physical damage dealt
                 * for 20sec.*/
                // http://ptr.wowhead.com/spell=100656
                if (i > 1)
                {
                    BuffState FeedingFrenzyRageface = new BuffState
                    {
                        Name = "Feeding Frenzy Rageface",
                        Breakable = false,
                        Chance = 1,
                        Duration = 20 * 1000,
                        Frequency = RagefaceMelee.AttackSpeed,
                        Stats = FeedingFrenzyStats,
                    };
                    FeedingFrenzyRageface.SetAffectsRoles_DPS();
                    FeedingFrenzyRageface.SetAffectsRoles_Healers();
                    Before30Pct.BuffStates.Add(FeedingFrenzyRageface);
                    After30Pct.BuffStates.Add(FeedingFrenzyRageface);
                }
                #endregion

                #region Frenzied Devotion
                /* Rageface goes into an unstoppable rage if he is alive to sitness Shannox's health reach 30%.
                 * This effect increases damage dealth by 200% and attack and movement speed by 100%.*/
                // http://ptr.wowhead.com/spell=100064
                // Only happens on normal
                // Should NEVER happen
                #endregion
                #endregion

                #region Apply Phases
                float phaseStartTime = 0;
                float NormalPhaseOneStartTime = 0;
                float phaseDuration = BerserkTimer[i] * (1 - .3f);
                NormalPhaseOneStartTime = phaseDuration / 2f;
                InnerPhase DogsUp;
                if (i < 2)
                {
                    DogsUp = new InnerPhase(TwoDogsUp, i, 1, 0, NormalPhaseOneStartTime, BerserkTimer[i]);
                    Before30Pct.InnerPhases.Add(DogsUp);
                    DogsUp = new InnerPhase(OneDogUp, i, 1, NormalPhaseOneStartTime, NormalPhaseOneStartTime, BerserkTimer[i]);
                    Before30Pct.InnerPhases.Add( DogsUp );
                }
                ApplyAPhasesValues( Before30Pct, i, 1, phaseStartTime, phaseDuration, BerserkTimer[i] );
                phaseStartTime += phaseDuration;
                phaseDuration = BerserkTimer[i] - phaseDuration;
                ApplyAPhasesValues( After30Pct, i, 1, phaseStartTime, phaseDuration, BerserkTimer[i] );
                AddAPhase(Before30Pct, i);
                AddAPhase(After30Pct, i);
                #endregion
            }
            #endregion
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Frost = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Fire = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Nature = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Arcane = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Shadow = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Holy = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
            #region Impedances
            for (int i = 0; i < 2; i++)
            {
                //Moves;

                // TODO: OT Always moves for Hurl Spear.
                // DPS Moves for Hurl Spear
                
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

            }
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
            /* TODO:
             * Moves for Hurl Spear & Traps.
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, BossHandler.TierLevels.T12_LFR };
            #endregion
            #region Basics
            Health = new float[] { 31565310f, 99978288f, 59428676f, 166239664f, 0 };
            MobType = (int)MOB_TYPES.ELEMENTAL;
            BerserkTimer = new int[] { 6 * 60, 6 * 60, 6 * 60, 6 * 60, 0 };
            SpeedKillTimer = new int[] { 5 * 60, 5 * 60, 5 * 60, 5 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase Normal = new Phase() { Name = "Normal Phase" };
                Phase DecimationBlade = new Phase() { Name = "Decimation Blade" };
                Phase InfernoBlade = new Phase() { Name = "Inferno Blade" };
                Phase BothPhases = new Phase() { Name = "Both Phases" };

                Attack Melee = GenAStandardMelee(this[i].Content);
                Melee.IsDualWielding = true;
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                Normal.Attacks.Add(Melee);

                #region Blaze of Glory
                /* Baleroc's assault periodically awakens a burning spark within his primary target, increasing the
                 * target's physical damage taken by 20%, but also raising their maximum health by 20%.
                 * 
                 * Every time Baleroc applies Blaze of Glory, he gains an application of Incendiary Soul, increasing Fire
                 * damage done by 20%.*/
                // http://ptr.wowhead.com/spell=99252
                // He averages out to applying this once every 12 seconds
                Stats BlazeofGloryStats = new Stats();
                SpecialEffect BlazeofGlorySpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { BonusHealthMultiplier = 0.2f, DamageTakenReductionMultiplier = -0.20f }, BerserkTimer[i], 12f, 1f, 99);
                BlazeofGloryStats.AddSpecialEffect(BlazeofGlorySpecialEffect);
                BuffState BlazeofGlory = new BuffState
                {
                    Name = "Blaze of Glory",
                    Chance = 1,
                    Frequency = BerserkTimer[i],
                    Stats = BlazeofGloryStats,
                    Breakable = false,
                    Duration = BerserkTimer[i] * 1000f,
                };
                BlazeofGlory.SetAffectsRoles_Tanks();
                BothPhases.BuffStates.Add(BlazeofGlory);

                /* Incendiary Soul
                 * Every time Baleroc applies Blaze of Glory, he gains an application of Incendiary Soul, increasing Fire
                 * damage done by 20%.*/
                // http://ptr.wowhead.com/spell=99369
                Stats IncendiarySoulStat = new Stats();
                SpecialEffect IncendiarySoulSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { FireDamageTakenMultiplier = 0.20f }, BerserkTimer[i], 12f, 1f, 99);
                IncendiarySoulStat.AddSpecialEffect(IncendiarySoulSpecialEffect);
                BuffState IncendiarySoul = new BuffState
                {
                    Name = "Incendiary Soul",
                    Chance = 1,
                    Frequency = BerserkTimer[i],
                    Stats = IncendiarySoulStat,
                    Breakable = false,
                    Duration = BerserkTimer[i] * 1000f,
                };
                IncendiarySoul.SetAffectsRoles_Tanks();
                BothPhases.BuffStates.Add(IncendiarySoul);
                #endregion

                #region Shards of Torment
                /* Baleroc summons *warning* two chrystals *end warning* amonst his foes, which continually channel
                 * a shadowy beam on the player that is nearest to them.*/
                // Summon - http://ptr.wowhead.com/spell=99260
                // NPC - http://db.mmo-champion.com/c/53495/
                float ShardofTormentDebuff = new float[] { 40, 60, 40, 60 }[i];

                #region Torment
                // Deals 3500 Shadow damage per application to the nearest player, stacking once per second.
                // 10 man - http://ptr.wowhead.com/spell=99256
                // 25 man - http://ptr.wowhead.com/spell=100230
                // 10 man heroic - http://ptr.wowhead.com/spell=100231
                // 25 man heroic - http://ptr.wowhead.com/spell=100232
                // At most people should be taking 12 stacks of Torment
                float TormentDamageMultiplier = (1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11) / 12f;
                float Affected = ((i == 0) || (i == 2 ) ? 1f : 2f );
                Attack Torment = new Attack
                {
                    Name = "Torment",
                    AttackSpeed = 34f,
                    DamagePerTick = new float[] { 3000, 3000, 4250, 4250, 0 }[i] * TormentDamageMultiplier,
                    IsDoT = true,
                    Duration = 12f,
                    TickInterval = 1f,
                    MaxNumTargets = Affected * 2f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Shadow,
                    SpellID = new float[] { 99256, 100230, 100231, 100232, 0 }[i],
                };
                Torment.SetAffectsRoles_DPS();
                BothPhases.Attacks.Add(Torment);
                #endregion

                #region Tormented
                /* When Torment fades from a player, they are afflicted by the Tormented effect, which increases
                 * Shadow damage taken by 250% and reduces healing done by 75%, for 40 sec.
                 * *Warning* Direct melee contact with any other player will apply a fresh copy of the Tormented effect
                 * to that player. [Heroic ONLY]*/
                // 10 man - http://ptr.wowhead.com/spell=99257
                // 25 man - http://ptr.wowhead.com/spell=99402
                // 10 man heroic - http://ptr.wowhead.com/spell=99403
                // 25 man heroic - http://ptr.wowhead.com/spell=99404
                // This should NEVER get placed on a Healer
                float TormentedDamageIncrease = new float[] { 2.5f, 2.5f, 5f, 5f, 0f }[i];
                BuffState Tormented = new BuffState
                {
                    Name = "Tormented",
                    Chance = Affected / (Max_Players[i] - Min_Healers[i] - Min_Tanks[i]),
                    Breakable = false,
                    Duration = ShardofTormentDebuff * 1000f,
                    Frequency = 34f,
                    Stats = new Stats() { SpellDamageTakenReductionMultiplier = -TormentedDamageIncrease, BonusHealingDoneMultiplier = -0.50f },
                };
                Tormented.SetAffectsRoles_DPS();
                BothPhases.BuffStates.Add(Tormented);
                #endregion

                #region Wave of Torment
                /* If there are no players within 15 yards of a Shard of Torment, the Shard pulses this effect, dealing
                 * 14250 to 15750 Shadow damage each second to all players.*/
                // 10 man - http://ptr.wowhead.com/spell=99261
                // 25 man - http://www.wowhead.com/spell=101636
                // 10 man heroic - http://www.wowhead.com/spell=101637
                // 25 man heroic - http://www.wowhead.com/spell=101638
                // This should NEVER happen
                #endregion

                #region Vital Spark
                /* If a player casts a direct heal on someone who is being damaged by Torment, the healer gains an
                 * application of Vital Spark for each (three/five [Normal/Heroic]) stacks of Torment on the target.
                 * Casting a single-target direct heal on a target affected by Blaze of Glory will trigger Vital Flame,
                 * increasing healing done on such targets by 5% per stack of vital Spark, lasting for 15 sec.*/
                // http://ptr.wowhead.com/spell=99262
                float VitalSparkBaseCD = new float[] { 3, 3, 5, 5, 0 }[i];
                float VitalSparkCD = BerserkTimer[i] / ((BerserkTimer[i] * ((Torment.Duration * 2f) / Torment.AttackSpeed)) / VitalSparkBaseCD);
                Stats VitalSparkStats = new Stats();
                // Assume half the healers get the buff at any given time
                SpecialEffect VitalSparkSpecialEffect = new SpecialEffect(Trigger.HealingSpellHit, new Stats() { BonusHealingDoneMultiplier = 0.05f }, 60f, VitalSparkCD, 0.5f, 999);
                VitalSparkStats.AddSpecialEffect(VitalSparkSpecialEffect);
                BuffState VitalSpark = new BuffState
                {
                    Name = "Vital Spark",
                    Breakable = false,
                    Chance = 1f,
                    Duration = BerserkTimer[i] * 1000f,
                    Frequency = BerserkTimer[i] - 1f,
                    Stats = VitalSparkStats,
                };
                VitalSpark.SetAffectsRoles_Healers();
                BothPhases.BuffStates.Add(VitalSpark);
                #endregion

                #region Vital Flame
                /* Increasing healing done to targets affected by Blaze of Glory by 5% per stack of Vital Spark
                 * consumed to create this effect, lasting for 15 sec. When Vital Flame expires, it restores the
                 * Vital Spark stacks that were consumed to create the effect.*/
                // http://ptr.wowhead.com/spell=99263
                Stats VitalFlameStats = new Stats();
                SpecialEffect VitalFlameSpecialEffect = new SpecialEffect(Trigger.Use, new Stats() { HealingReceivedMultiplier = 0.05f }, 60f, VitalSparkCD, 1f, 999);
                VitalFlameStats.AddSpecialEffect(VitalFlameSpecialEffect);
                BuffState VitalFlame = new BuffState
                {
                    Name = "Vital Flame",
                    Breakable = false,
                    Chance = 1f,
                    Duration = BerserkTimer[i] * 1000f,
                    Frequency = BerserkTimer[i] - 1f,
                    Stats = VitalFlameStats,
                };
                VitalFlame.SetAffectsRoles_Tanks();
                BothPhases.BuffStates.Add(VitalFlame);
                #endregion
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
                Attack DecimatingStrike = new Attack
                {
                    // Decimation Blades' Attack speed was reduced for 5 second per swing to 6.25 on Sep 19, 2011
                    Name = "Decimating Strike",
                    AttackSpeed = 6.25f,
                    DamagePerHit = .9f,
                    DamageType = ItemDamageType.Shadow,
                    DamageIsPerc = true,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    SpellID = 99353,
                    Blockable = false,
                    Dodgable = true,
                    Parryable = true,
                    Missable = true,
                    IsDualWielding = false,
                };
                DecimatingStrike.AffectsRole[PLAYER_ROLES.OffTank] = true;
                DecimationBlade.Attacks.Add(DecimatingStrike);

                /* Inferno Blade
                 * Baleroc's melee strikes deal 102999 to 103000 Fire damage to the target, instead of their
                 * normal physical damage, while this effect is active.*/
                // Initial Cast - http://ptr.wowhead.com/spell=99350
                // 10 man - http://ptr.wowhead.com/spell=99351
                // 25 man - http://ptr.wowhead.com/spell=101000
                // 10 man heroic - http://ptr.wowhead.com/spell=101001
                // 25 man heroic - http://ptr.wowhead.com/spell=101002
                Attack InfernoStrike = new Attack
                {
                    Name = "Inferno Strike",
                    AttackSpeed = 4f,
                    DamagePerHit = new float[] { (73125f + 76875f), (124312f + 130687f), (116025f + 121975f), (197242f + 207357f), 0f }[i] / 2f,
                    DamageType = ItemDamageType.Fire,
                    DamageIsPerc = false,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    SpellID = new float[] { 99351, 10100, 101001, 101002, 0 }[i],
                    Blockable = true,
                    Dodgable = true,
                    Parryable = true,
                    Missable = true,
                    IsDualWielding = false,
                };
                InfernoStrike.AffectsRole[PLAYER_ROLES.MainTank] = true;
                InfernoBlade.Attacks.Add(InfernoStrike);

                #endregion

                #region Countdown [Heroic Only]
                /* Baleroc links two players to each other for 8 sec. If the chosen players move within 3 yards of each
                 * other, the effect will dissipate harmlessly, but if the effect runs its full course, both players will
                 * explode, dealing 125000 Fire damage to all allies within 45 yards.*/
                // http://ptr.wowhead.com/spell=99516
                // possible explosion id: http://ptr.wowhead.com/spell=99518
                // This should NEVER go off, so one has to move at most 7 of the 8 seconds
                if (i > 1)
                {
                    Impedance Countdown_Move = new Impedance
                    {
                        Name = "Countdown Move",
                        Duration = 7f * 1000f,
                        Chance = 2f / (Max_Players[i] - Min_Tanks[i]),
                        Breakable = false,
                        Frequency = 45f,
                    };
                    Countdown_Move.SetAffectsRoles_DPS();
                    Countdown_Move.SetAffectsRoles_Healers();
                    BothPhases.Moves.Add(Countdown_Move);
               }
                #endregion

                #region Apply Phases
                // Pull then at 30 Sec, Empower Blade w/ 50/50 chance for which blade type.
                // 15 secs of empowered blade
                // Return to normal mode.
                int phasestart = 0;
                int EBdur = 15;
                int NormalDur = 30;
                InnerPhase InnerPhaseNormal, InnerPhaseEmpoweredBlade;
                InnerPhaseNormal = new InnerPhase(Normal, i, 1, phasestart, NormalDur, BerserkTimer[i]); phasestart += NormalDur; // 30 seconds;  OT builds stacks
                BothPhases.InnerPhases.Add(InnerPhaseNormal);
                InnerPhaseEmpoweredBlade = new InnerPhase(InfernoBlade, i, 1, phasestart, EBdur, BerserkTimer[i]); phasestart += EBdur; // 45 seconds
                BothPhases.InnerPhases.Add(InnerPhaseEmpoweredBlade);
                InnerPhaseNormal = new InnerPhase(Normal, i, 1, phasestart, NormalDur, BerserkTimer[i]); phasestart += NormalDur; // 1 minute 15 seconds
                BothPhases.InnerPhases.Add(InnerPhaseNormal);
                InnerPhaseEmpoweredBlade = new InnerPhase(DecimationBlade, i, 1, phasestart, EBdur, BerserkTimer[i]); phasestart += EBdur; // 1 minute 30 seconds
                BothPhases.InnerPhases.Add(InnerPhaseEmpoweredBlade);
                InnerPhaseNormal = new InnerPhase(Normal, i, 1, phasestart, NormalDur, BerserkTimer[i]); phasestart += NormalDur; // 2 minutes 0 seconds
                BothPhases.InnerPhases.Add(InnerPhaseNormal);
                InnerPhaseEmpoweredBlade = new InnerPhase(InfernoBlade, i, 1, phasestart, EBdur, BerserkTimer[i]); phasestart += EBdur; // 2 minutes 15 seconds
                BothPhases.InnerPhases.Add(InnerPhaseEmpoweredBlade);
                InnerPhaseNormal = new InnerPhase(Normal, i, 1, phasestart, NormalDur, BerserkTimer[i]); phasestart += NormalDur; // 2 minute 45 seconds
                BothPhases.InnerPhases.Add(InnerPhaseNormal);
                InnerPhaseEmpoweredBlade = new InnerPhase(DecimationBlade, i, 1, phasestart, EBdur, BerserkTimer[i]); phasestart += EBdur; // 3 minute 0 seconds
                BothPhases.InnerPhases.Add(InnerPhaseEmpoweredBlade);
                InnerPhaseNormal = new InnerPhase(Normal, i, 1, phasestart, NormalDur, BerserkTimer[i]); phasestart += NormalDur; // 3 minutes 30 seconds
                BothPhases.InnerPhases.Add(InnerPhaseNormal);
                InnerPhaseEmpoweredBlade = new InnerPhase(InfernoBlade, i, 1, phasestart, EBdur, BerserkTimer[i]); phasestart += EBdur; // 3 minutes 45 seconds
                BothPhases.InnerPhases.Add(InnerPhaseEmpoweredBlade);
                InnerPhaseNormal = new InnerPhase(Normal, i, 1, phasestart, NormalDur, BerserkTimer[i]); phasestart += NormalDur; // 4 minute 15 seconds
                BothPhases.InnerPhases.Add(InnerPhaseNormal);
                InnerPhaseEmpoweredBlade = new InnerPhase(DecimationBlade, i, 1, phasestart, EBdur, BerserkTimer[i]); phasestart += EBdur; // 4 minute 30 seconds
                BothPhases.InnerPhases.Add(InnerPhaseEmpoweredBlade);
                InnerPhaseNormal = new InnerPhase(Normal, i, 1, phasestart, NormalDur, BerserkTimer[i]); phasestart += NormalDur; // 5 minutes 0 seconds
                BothPhases.InnerPhases.Add(InnerPhaseNormal);
                InnerPhaseEmpoweredBlade = new InnerPhase(InfernoBlade, i, 1, phasestart, EBdur, BerserkTimer[i]); phasestart += EBdur; // 5 minutes 15 seconds
                BothPhases.InnerPhases.Add(InnerPhaseEmpoweredBlade);
                InnerPhaseNormal = new InnerPhase(Normal, i, 1, phasestart, NormalDur, BerserkTimer[i]); phasestart += NormalDur; // 5 minute 45 seconds
                BothPhases.InnerPhases.Add(InnerPhaseNormal);
                InnerPhaseEmpoweredBlade = new InnerPhase(DecimationBlade, i, 1, phasestart, EBdur, BerserkTimer[i]); phasestart += EBdur; // 6 minute 0 seconds
                BothPhases.InnerPhases.Add(InnerPhaseEmpoweredBlade);
                ApplyAPhasesValues(BothPhases, i, 1, 0, BerserkTimer[i], BerserkTimer[i]);
                AddAPhase(BothPhases, i);
                #endregion
            }
            #endregion
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Frost = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Fire = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Nature = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Arcane = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Shadow = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Holy = new double[] { 0.00f, 0.00f, 0, 0, 0 };
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
            /* TODO:
             * Heroic
             */
        }
    }

    public class MajordomoStaghelm : MultiDiffBoss
    {
        public MajordomoStaghelm()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Majordomo Staghelm";
            Instance = "Firelands";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, BossHandler.TierLevels.T12_LFR };
            Comment = "Not modeled in detail yet.";
            #endregion
            #region Basics
            Health = new float[] { 38221940f, 133927104f, 105990728f, 367274176f, 0 }; // TODO: Double check 25-man normal health value
            MobType = (int)MOB_TYPES.HUMANOID;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 5 * 60, 5 * 60, 5 * 60, 5 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 1, 1, 1, 1, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase NEPhase = new Phase() { Name = "Night Elf Phase" };
                Phase ShapeshiftingPhase1 = new Phase() { Name = "Shapeshifting Phase One" };
                Phase ShapeshiftingPhase2 = new Phase() { Name = "Shapeshifting Phase Two" };
                Phase ShapeshiftingPhase3 = new Phase() { Name = "Shapeshifting Phase Three" };
                Phase ShapeshiftingPhase4 = new Phase() { Name = "Shapeshifting Phase Four" };
                Phase ShapeshiftingPhase5 = new Phase() { Name = "Shapeshifting Phase Five" };
                Phase ShapeshiftingPhase6 = new Phase() { Name = "Shapeshifting Phase Six" };
                Phase ScorpionPhase1 = new Phase() { Name = "Scorpion Phase One" };
                Phase CatPhase1 = new Phase() { Name = "Cat Phase One" };
                Phase ScorpionPhase2 = new Phase() { Name = "Scorpion Phase Two" };
                Phase CatPhase2 = new Phase() { Name = "Cat Phase Two" };
                Phase ScorpionPhase3 = new Phase() { Name = "Scorpion Phase Three" };
                Phase CatPhase3 = new Phase() { Name = "Cat Phase Three" };
                Phase ScorpionPhase4 = new Phase() { Name = "Scorpion Phase Four" };
                Phase CatPhase4 = new Phase() { Name = "Cat Phase Four" };
                Phase ScorpionPhase5 = new Phase() { Name = "Scorpion Phase Five" };
                Phase CatPhase5 = new Phase() { Name = "Cat Phase Five" };
                Phase ScorpionPhase6 = new Phase() { Name = "Scorpion Phase Six" };
                Phase CatPhase6 = new Phase() { Name = "Cat Phase Six" };

                float SevenStackLength = 17.3f + 13.4f + 11f + 8.6f + 7.4f + 7.4f + 6.1f + 6.1f;
                float SevenStackAverageLength = SevenStackLength / 8f;
                float FiveStackLength = 17.3f + 13.4f + 11f + 8.6f + 7.4f + 7.4f;
                float FiveStackAverageLength = FiveStackLength / 6f;
                float ThreeStackLength = 17.3f + 13.4f + 11f + 8.6f;
                // Spread once the bar reaches 70%
                float ZeroStackLength = 17.3f * 0.7f;

                #region Main Tank Damage
                Attack MTMelee0Stacks = GenAStandardMelee(Content[i]);
                MTMelee0Stacks.Name = "Main Taink Melee - Zero Stacks";
                MTMelee0Stacks.DamagePerHit *= 0.80f;
                MTMelee0Stacks.AffectsRole[PLAYER_ROLES.OffTank] = false;
                MTMelee0Stacks.AffectsRole[PLAYER_ROLES.MainTank] = true;
                ScorpionPhase1.Attacks.Add(MTMelee0Stacks);

                Attack MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - One Stack";
                MTMeleeTemp.DamagePerHit *= 1.10f;
                CatPhase1.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Two Stacks";
                MTMeleeTemp.DamagePerHit *= 1.20f;
                ScorpionPhase2.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Three Stacks";
                MTMeleeTemp.DamagePerHit *= 1.30f;
                CatPhase2.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Four Stacks";
                MTMeleeTemp.DamagePerHit *= 1.40f;
                ScorpionPhase3.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Five Stacks";
                MTMeleeTemp.DamagePerHit *= 1.50f;
                CatPhase3.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Six Stacks";
                MTMeleeTemp.DamagePerHit *= 1.60f;
                ScorpionPhase4.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Seven Stacks";
                MTMeleeTemp.DamagePerHit *= 1.70f;
                CatPhase4.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Eight Stacks";
                MTMeleeTemp.DamagePerHit *= 1.80f;
                ScorpionPhase5.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Nine Stacks";
                MTMeleeTemp.DamagePerHit *= 1.90f;
                CatPhase5.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Ten Stacks";
                MTMeleeTemp.DamagePerHit *= 2.00f;
                ScorpionPhase6.Attacks.Add(MTMeleeTemp);

                MTMeleeTemp = MTMelee0Stacks.Clone();
                MTMeleeTemp.Name = "Main Taink Melee - Eleven Stacks";
                MTMeleeTemp.DamagePerHit *= 2.10f;
                CatPhase6.Attacks.Add(MTMeleeTemp);
                #endregion

                #region Concentration [Heroic Only]
                /* Each player who engages Fandral on heroic difficulty is granted a Concentration power bar.
                 * This bar fills over time, increasing damage and healing done by 25% for every 25 Concentration
                 * up to 100. Players hit by a damaging attack or spell will lose all currently accumulated
                 * Concentration.*/
                // http://ptr.wowhead.com/spell=98229
                // Uncommon (25%) - http://ptr.wowhead.com/spell=98254
                // Rare (50%) - http://ptr.wowhead.com/spell=98254
                // Epic (75%) - http://ptr.wowhead.com/spell=98252
                // Legendary (100%) - http://ptr.wowhead.com/spell=98245
                Stats ConcentrationStat = new Stats();
                SpecialEffect ConcentrationSpecialEffect = new SpecialEffect ( Trigger.Use, new Stats() { BonusDamageMultiplier = 0.25f, BonusHealingDoneMultiplier = 0.25f }, BerserkTimer[i], 5f, 1, 4 );
                ConcentrationStat.AddSpecialEffect(ConcentrationSpecialEffect);
                if (i > 1)
                {
                    BuffState Concentration = new BuffState
                    {
                        Name = "Concentration",
                        Frequency = 1f,
                        Duration = BerserkTimer[i] * 1000f,
                        Chance = 1f,
                        Breakable = false,
                        Stats = ConcentrationStat,
                    };
                    // Do to the fact that this only triggers when NOT getting hit, the tank will not be able to gain any stacks
                    Concentration.SetAffectsRoles_DPS();
                    Concentration.SetAffectsRoles_Healers();
                    ShapeshiftingPhase1.BuffStates.Add(Concentration);
                    ShapeshiftingPhase2.BuffStates.Add(Concentration);
                    ShapeshiftingPhase3.BuffStates.Add(Concentration);
                    ShapeshiftingPhase4.BuffStates.Add(Concentration);
                    ShapeshiftingPhase5.BuffStates.Add(Concentration);
                    ShapeshiftingPhase6.BuffStates.Add(Concentration);
                }
                #endregion

                #region Behold the Rage of the Firelands!
                /* Fandral transforms into a Cat when his enemies are not clustered together or into a Scorpion when
                 * 7 or more of his enemies are clustered together.*/

                #region Cat Form
                // Fandral transforms into a Cat when his enemies are not clustered together.
                // http://db.mmo-champion.com/c/53145/

                #region Leaping Flames
                /* Fandral leaps at an enemy, inflicting 36404 to 40846 Fire damage in a small area and
                 * creating a Spirit of the Flame. This attack costs 100 energy.*/
                // 10 man - http://ptr.wowhead.com/spell=98535
                // 25 man - http://ptr.wowhead.com/spell=100206
                // 10 man heroic - http://ptr.wowhead.com/spell=100207
                // 25 man heroic - http://ptr.wowhead.com/spell=100208
                // Move but assume the person get hit with four ticks of the AOE
                Impedance LeapingFlames_Move = new Impedance
                {
                    Name = "Leaping Flames Movement",
                    Duration = 2f * 1000f,
                    Frequency = SevenStackAverageLength,
                    Chance = 1f / Max_Players[i],
                    Breakable = false,
                };
                LeapingFlames_Move.SetAffectsRoles_Healers();
                LeapingFlames_Move.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                CatPhase1.Moves.Add(LeapingFlames_Move);
                CatPhase2.Moves.Add(LeapingFlames_Move);
                CatPhase3.Moves.Add(LeapingFlames_Move);
                CatPhase4.Moves.Add(LeapingFlames_Move);
                CatPhase5.Moves.Add(LeapingFlames_Move);
                CatPhase6.Moves.Add(LeapingFlames_Move);

                Attack LeapingFlames = new Attack
                {
                    Name = "Leaping Flames",
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerTick = new float[] { (17671f + 19828f), (17671f + 19828f), (26036f + 29213f), (26036f + 29213f), 0f }[i] / 2f,
                    Duration = 2f,
                    TickInterval = 0.5f,
                    IsDoT = true,
                    SpellID = new float[] { 98535, 100206, 100207, 100208, 0 }[i],
                };
                LeapingFlames.SetAffectsRoles_Healers();
                LeapingFlames.AffectsRole[PLAYER_ROLES.RangedDPS] = true;

                Attack LeapingFlames_temp = LeapingFlames.Clone();
                LeapingFlames_temp.DamagePerTick *= 1.10f;
                CatPhase1.Attacks.Add(LeapingFlames_temp);

                LeapingFlames_temp = LeapingFlames.Clone();
                LeapingFlames_temp.DamagePerTick *= 1.30f;
                CatPhase2.Attacks.Add(LeapingFlames_temp);

                LeapingFlames_temp = LeapingFlames.Clone();
                LeapingFlames_temp.DamagePerTick *= 1.50f;
                CatPhase3.Attacks.Add(LeapingFlames_temp);

                LeapingFlames_temp = LeapingFlames.Clone();
                LeapingFlames_temp.DamagePerTick *= 1.70f;
                CatPhase4.Attacks.Add(LeapingFlames_temp);

                LeapingFlames_temp = LeapingFlames.Clone();
                LeapingFlames_temp.DamagePerTick *= 1.90f;
                CatPhase5.Attacks.Add(LeapingFlames_temp);

                LeapingFlames_temp = LeapingFlames.Clone();
                LeapingFlames_temp.DamagePerTick *= 2.10f;
                CatPhase6.Attacks.Add(LeapingFlames_temp);
                #endregion

                #region Spirit of the Flame
                // These small burning cats attack enemeis until defeated.
                // http://ptr.wowhead.com/npc=52593
                TargetGroup SpiritOfTheFlame = new TargetGroup
                {
                    Name = "Spirit of the Flame",
                    LevelOfTargets = 85,
                    Chance = 1f,
                    NearBoss = true,
                    NumTargs = 1f,
                    TargetID = 52593,
                    Duration = SevenStackAverageLength * 1000f,
                    Frequency = SevenStackAverageLength,
                };
                SpiritOfTheFlame.SetAffectsRoles_All();
                CatPhase1.Targets.Add(SpiritOfTheFlame);
                CatPhase2.Targets.Add(SpiritOfTheFlame);
                CatPhase3.Targets.Add(SpiritOfTheFlame);
                CatPhase4.Targets.Add(SpiritOfTheFlame);
                CatPhase5.Targets.Add(SpiritOfTheFlame);
                CatPhase6.Targets.Add(SpiritOfTheFlame);

                Attack SpiritOfTheFlameMelee = new Attack
                {
                    Name = "Spirit Of The Flame Melee",
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] * .666f,
                    DamageType = ItemDamageType.Physical,
                    IsFromAnAdd = true,
                    MaxNumTargets = 1f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    AttackSpeed = 1.8f,
                    Blockable = true,
                    Dodgable = true,
                    Missable = true,
                    Parryable = true,
                };
                SpiritOfTheFlameMelee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                CatPhase1.Attacks.Add(SpiritOfTheFlameMelee);
                CatPhase2.Attacks.Add(SpiritOfTheFlameMelee);
                CatPhase3.Attacks.Add(SpiritOfTheFlameMelee);
                CatPhase4.Attacks.Add(SpiritOfTheFlameMelee);
                CatPhase5.Attacks.Add(SpiritOfTheFlameMelee);
                CatPhase6.Attacks.Add(SpiritOfTheFlameMelee);
                #endregion

                #region Adrenaline
                /* Fandral gains a stack of Adrenaline each time he performs Leaping Flames. Adrenaline
                 * increases his energy regeneration rate by 1% per application. Fandral loses all stacks of
                 * Adrenaline when he switches form.*/
                // http://ptr.wowhead.com/spell=97238
                #endregion

                #region Fury
                /* Fandral gains a stack of Fury each time he transforms into a Cat or Scorpion,
                 * permanently increasing the Physical damage he deals by 10%.*/
                // http://ptr.wowhead.com/spell=97235
                #endregion
                #endregion

                #region Scorpion Form
                // Fandral transforms into a Scorpion when 7 or more of his enemies are clustered together.

                #region Flame Scythe
                // Fandral inflicts 562000 Fire damage to enemies in front of him. Damage is split equally
                // among targets hit. This attack costs 100 energy. 
                // Damage needs to be split between all players
                // 10 man - http://ptr.wowhead.com/spell=98474
                // 25 man - http://ptr.wowhead.com/spell=100212
                // 10 man heroic - http://ptr.wowhead.com/spell=100213
                // 25 man heroic - http://ptr.wowhead.com/spell=100214
                Attack FlameSythe = new Attack
                {
                    Name = "Flame Sythe",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = new float[] { 562000, 1687000, 765000, 2295000, 0 }[i] / Max_Players[i],
                    AttackSpeed = (i < 2 ? SevenStackAverageLength : FiveStackAverageLength),
                    SpellID = new float[] { 98474, 100212, 100213, 100214, 0 }[i],
                    MaxNumTargets = Max_Players[i],
                };
                FlameSythe.SetAffectsRoles_All();
                ScorpionPhase1.Attacks.Add(FlameSythe);

                Attack FlameSythe_temp = FlameSythe.Clone();
                FlameSythe_temp.DamagePerHit *= 1.2f;
                if (i < 2)
                    ScorpionPhase2.Attacks.Add(FlameSythe_temp);

                FlameSythe_temp = FlameSythe.Clone();
                FlameSythe_temp.DamagePerHit *= 1.4f;
                if (i < 2)
                    ScorpionPhase3.Attacks.Add(FlameSythe_temp);

                FlameSythe_temp = FlameSythe.Clone();
                FlameSythe_temp.DamagePerHit *= 1.6f;
                if (i < 2)
                    ScorpionPhase4.Attacks.Add(FlameSythe_temp);

                FlameSythe_temp = FlameSythe.Clone();
                FlameSythe_temp.DamagePerHit *= 1.8f;
                if (i < 2)
                    ScorpionPhase5.Attacks.Add(FlameSythe_temp);

                FlameSythe_temp = FlameSythe.Clone();
                FlameSythe_temp.DamagePerHit *= 2.0f;
                if (i < 2)
                    ScorpionPhase6.Attacks.Add(FlameSythe_temp);
                #endregion

                #region Adrenaline
                /* Fandral gains a stack of Adrenaline each time he performs Flame Scythe. Adrenaline
                 * increases his energy regeneration rate by 1% per application. Fandral loses all stacks of
                 * Adrenaline when he switches form.*/
                // http://ptr.wowhead.com/spell=97238
                #endregion

                #region Fury
                /* Fandral gains a stack of Fury each time he transforms into a Cat or Scorpion,
                 * permanently increasing the Physical damage he deals by 10%.*/
                // http://ptr.wowhead.com/spell=97235
                #endregion
                #endregion
                #endregion

                #region Blaze of Glory!
                /* Fandral continues to transform into a Cat or Scorpion.
                 * 
                 * On every third transform, Fandral pauses in human form to briefly envelop his enemies in a Fiery
                 * Cyclone and cast an additional spell. When he is switching from Cat form to Scorpion form, Fandral
                 * unleashes Searing Seeds. When he is switching from Scorpion form to Cat form, Fandral unleashes
                 * Burning Orbs.*/

                #region Fiery Cyclone
                /* On every third transform, Fandral pauses in human form to briefly envelop his enemies in a
                 * Fiery Cyclone.
                 * 
                 * The Fiery Cyclone tosses all enemy targets into the air, preventing all action but making them
                 * invulnerable for 3 sec.*/
                Impedance FieryCyclone = new Impedance
                {
                    Name = "Fiery Cyclone",
                    Frequency = 1f,
                    Duration = 3f * 1000f,
                    Chance = 1f,
                    Breakable = false,
                };
                FieryCyclone.SetAffectsRoles_All();
                NEPhase.Stuns.Add(FieryCyclone);
                #endregion

                #region Searing Seeds
                /* When Fandral switches from Cat form to Scorpion form, he unleashes Searing Seeds.
                 * 
                 * Searing Seeds implants fiery seeds in Fandral's enemies. Each seed grows at a different rate.
                 * When fully grown, the seeds explode, inflicting 51499 to 51500 Fire damage to targets within
                 * 10 yards.*/
                // 10-man - http://ptr.wowhead.com/spell=98620
                // 25-man - http://ptr.wowhead.com/spell=100215
                // 10-man Heroic - http://ptr.wowhead.com/spell=100216
                // 25-man Heroic - http://ptr.wowhead.com/spell=100217
                Attack SearingSeed = new Attack
                {
                    Name = "Searing Seed",
                    DamagePerHit = new float[] { 45000, 45000, 63750, 63750, 0 }[i],
                    AttackSpeed = (SevenStackLength * (i < 2 ? 2f : 1f))  - 1f,
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    MaxNumTargets = 1f,
                    SpellID = new float[] { 98620, 100215, 100216, 100217, 0 }[i],
                };
                SearingSeed.SetAffectsRoles_All();
                SearingSeed.AffectsRole[PLAYER_ROLES.MainTank] = false;
                ShapeshiftingPhase2.Attacks.Add(SearingSeed);
                ShapeshiftingPhase4.Attacks.Add(SearingSeed);
                ShapeshiftingPhase6.Attacks.Add(SearingSeed);
                #endregion

                #region Burning Orbs
                /* When Fandral switches from Scorpion form to Cat form, he unleashes Burning Orbs.
                 * 
                 * Fandral summons several orbs around the room. Each orb attacks the player nearest to it,
                 * burning them for 7276 to 8174 Fire damage every 2 sec. Stacks.*/
                // No more than 4 stacks
                // Meaning to get the average amount after 4 stacks, you multiply by 2.5
                // 10-man - http://ptr.wowhead.com/spell=98584
                // 25-man - http://ptr.wowhead.com/spell=100209
                // 10-man Heroic - http://ptr.wowhead.com/spell=100210
                // 25-man Heroic - http://ptr.wowhead.com/spell=100211
                Attack BurningOrbs = new Attack
                {
                    Name = "Burning Orb",
                    DamagePerHit = new float[] { 3750, 3750, 7650, 7650, 0 }[i] * 10f,
                    DamagePerTick = new float[] { 3750, 3750, 7650, 7650, 0 }[i] * 2.5f,
                    IsDoT = true,
                    Duration = 6f,
                    TickInterval = 2f,
                    AttackSpeed = 14f,
                    MaxNumTargets = new float[] { 2, 5, 2, 5, 0 }[i],
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    SpellID = new float[] { 98584, 100209, 100210, 100211, 0 }[i],
                };
                BurningOrbs.SetAffectsRoles_All();
                ShapeshiftingPhase3.Attacks.Add(BurningOrbs);
                ShapeshiftingPhase5.Attacks.Add(BurningOrbs);
 

                #endregion
                #endregion

                #region Apply Phases
                float phasestart = 0;
                float ShapshiftingStart = 0;
                InnerPhase InnerPhaseShapeShifting;
                // On Normal we are running a 7 stack each phase. That leaves only a 3 stack during the final Cat phase for Berserk
                if (i < 2)
                {
                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase1, i, 1, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase1.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase1, i, 1, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase1.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase1, i, 1, phasestart, ShapshiftingStart, BerserkTimer[i]); phasestart += ShapshiftingStart;
                    ApplyAPhasesValues(NEPhase, i, 2, phasestart, 3f, BerserkTimer[i]); phasestart += 3f; ShapshiftingStart = 0f;
                    AddAPhase(ShapeshiftingPhase1, i);

                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase2, i, 3, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase2.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase2, i, 3, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase2.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase2, i, 3, phasestart, ShapshiftingStart, BerserkTimer[i]); phasestart += ShapshiftingStart;
                    ApplyAPhasesValues(NEPhase, i, 4, phasestart, 3f, BerserkTimer[i]); phasestart += 3f; ShapshiftingStart = 0f;
                    AddAPhase(ShapeshiftingPhase2, i);

                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase3, i, 5, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase3.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase3, i, 5, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase3.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase3, i, 5, phasestart, ShapshiftingStart, BerserkTimer[i]); phasestart += ShapshiftingStart;
                    ApplyAPhasesValues(NEPhase, i, 6, phasestart, 3f, BerserkTimer[i]); phasestart += 3f; ShapshiftingStart = 0f;
                    AddAPhase(ShapeshiftingPhase3, i);
                    AddAPhase(NEPhase, i);

                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase4, i, 7, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase4.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase4, i, 7, ShapshiftingStart, ThreeStackLength, BerserkTimer[i]); ShapshiftingStart += ThreeStackLength;
                    ShapeshiftingPhase4.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase4, i, 7, phasestart, ShapshiftingStart, BerserkTimer[i]);
                    AddAPhase(ShapeshiftingPhase4, i);
                }
                // On Heroic We are running a 5 Stack on the first Scorpion
                // Then a 7 Stack on Cat and 0 stack [70% before the first stack goes off] on the Scorpion phase
                else
                {
                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase1, i, 1, ShapshiftingStart, FiveStackLength, BerserkTimer[i]); ShapshiftingStart += FiveStackLength;
                    ShapeshiftingPhase1.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase1, i, 1, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase1.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase1, i, 1, phasestart, ShapshiftingStart, BerserkTimer[i]); phasestart += ShapshiftingStart;
                    ApplyAPhasesValues(NEPhase, i, 2, phasestart, 3f, BerserkTimer[i]); phasestart += 3f; ShapshiftingStart = 0f;
                    AddAPhase(ShapeshiftingPhase1, i);

                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase2, i, 3, ShapshiftingStart, ZeroStackLength, BerserkTimer[i]); ShapshiftingStart += ZeroStackLength;
                    ShapeshiftingPhase2.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase2, i, 3, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase2.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase2, i, 3, phasestart, ShapshiftingStart, BerserkTimer[i]); phasestart += ShapshiftingStart;
                    ApplyAPhasesValues(NEPhase, i, 4, phasestart, 3f, BerserkTimer[i]); phasestart += 3f; ShapshiftingStart = 0f;
                    AddAPhase(ShapeshiftingPhase2, i);

                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase3, i, 5, ShapshiftingStart, ZeroStackLength, BerserkTimer[i]); ShapshiftingStart += ZeroStackLength;
                    ShapeshiftingPhase3.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase3, i, 5, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase3.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase3, i, 5, phasestart, ShapshiftingStart, BerserkTimer[i]); phasestart += ShapshiftingStart;
                    ApplyAPhasesValues(NEPhase, i, 6, phasestart, 3f, BerserkTimer[i]); phasestart += 3f; ShapshiftingStart = 0f;
                    AddAPhase(ShapeshiftingPhase3, i);

                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase4, i, 7, ShapshiftingStart, ZeroStackLength, BerserkTimer[i]); ShapshiftingStart += ZeroStackLength;
                    ShapeshiftingPhase4.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase4, i, 7, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase4.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase4, i, 7, phasestart, ShapshiftingStart, BerserkTimer[i]); phasestart += ShapshiftingStart;
                    ApplyAPhasesValues(NEPhase, i, 8, phasestart, 3f, BerserkTimer[i]); phasestart += 3f; ShapshiftingStart = 0f;
                    AddAPhase(ShapeshiftingPhase3, i);

                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase5, i, 9, ShapshiftingStart, ZeroStackLength, BerserkTimer[i]); ShapshiftingStart += ZeroStackLength;
                    ShapeshiftingPhase5.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase5, i, 9, ShapshiftingStart, SevenStackLength, BerserkTimer[i]); ShapshiftingStart += SevenStackLength;
                    ShapeshiftingPhase5.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase5, i, 9, phasestart, ShapshiftingStart, BerserkTimer[i]); phasestart += ShapshiftingStart;
                    ApplyAPhasesValues(NEPhase, i, 10, phasestart, 3f, BerserkTimer[i]); phasestart += 3f; ShapshiftingStart = 0f;
                    AddAPhase(ShapeshiftingPhase3, i);
                    AddAPhase(NEPhase, i);

                    InnerPhaseShapeShifting = new InnerPhase(ScorpionPhase6, i, 11, ShapshiftingStart, ZeroStackLength, BerserkTimer[i]); ShapshiftingStart += ZeroStackLength;
                    ShapeshiftingPhase6.InnerPhases.Add(InnerPhaseShapeShifting);
                    InnerPhaseShapeShifting = new InnerPhase(CatPhase6, i, 11, ShapshiftingStart, SevenStackLength - 4.45f, BerserkTimer[i]); ShapshiftingStart += (SevenStackLength - 4.45f);
                    ShapeshiftingPhase6.InnerPhases.Add(InnerPhaseShapeShifting);
                    ApplyAPhasesValues(ShapeshiftingPhase6, i, 11, phasestart, ShapshiftingStart, BerserkTimer[i]);
                    AddAPhase(ShapeshiftingPhase3, i);
                }
                #endregion
            }
            #endregion
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Frost = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Fire = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Nature = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Arcane = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Shadow = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Holy = new double[] { 0.00f, 0.00f, 0, 0, 0 };
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
            /* TODO:
             * all
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T12_10, BossHandler.TierLevels.T12_25, BossHandler.TierLevels.T12_10H, BossHandler.TierLevels.T12_25H, BossHandler.TierLevels.T12_LFR };
            Comment = "Not modeled in detail yet.";
            #endregion
            #region Basics
            // Rag "dies" at 10% on normal (goes back under the lava).
            // Apparently Rag heals to about 50% once he hits phase 4 and starts moving around the platform
            // So for heroic his health is 90% to phase 4 and 50% while in Phase 4 so 140% health
            Health = new float[] { 50246820f * 0.9f, 150740464f * 0.9f, 74200000f * 1.40f, 246910064f * 1.40f, 0 }; // TODO: Double check Heroic 10-man health
            MobType = (int)MOB_TYPES.ELEMENTAL;
            BerserkTimer = new int[] { 18 * 60, 18 * 60, 18 * 60, 18 * 60, 0 };
            SpeedKillTimer = new int[] { 5 * 60, 8 * 60, 12 * 60, 12 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 0 };
            Min_Healers = new int[] { 2, 5, 2, 3, 0 };
            TimeBossIsInvuln = new float[] { 45f * 2f, 45f * 2f, (45 * 2f) + 14f, (45 * 2f) + 14f, 0 }; // Assume 45 seconds during each phase transition, and 14 seconds going into phase 4
            Under35Perc = new double[] { 0.166666667, 0.166666667, 0.214285714, 0.214285714, 0 };
            Under20Perc = new double[] { 0.122222222, 0.122222222, 0.221428571, 0.221428571, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase ByFirebePurged = new Phase() { Name = "By Fire be Purged!" }; // Phase 1
                Phase Intermission1 = new Phase() { Name = "Intermission: Minions of Fire!" }; // Intermission 1
                Phase SulfuaswillbeYourEnd = new Phase() { Name = "Sulfuras will be Your End!" }; // Phase 2
                Phase Intermission2 = new Phase() { Name = "Intermission: Denizens of Flame!" }; // Intermission 2
                Phase BegoneFrommyRealm = new Phase() { Name = "Begone From my Realm!" }; // Phase 3
                Phase Intermission3 = new Phase() { Name = "Ah! Outsiders! This is not your Realm!" }; // Intermission 3
                Phase TheTruePoweroftheFireLord = new Phase() { Name = "The True Power of the Fire Lord!" }; // Heroic Only

                float IntermissionLength = 45f;
                float Intermission3Length = 14f;
                float phase1to3Length = (BerserkTimer[i] - (45f * 2) - (i > 1 ? Intermission3Length : 0f)) * (i < 2 ? (1f / 3f) : (3f / 14f));
                float phase4Length = (BerserkTimer[i] - (45f * 2) - Intermission3Length) * (5f / 14f);

                Attack melee = GenAStandardMelee(this[i].Content);
                melee.DamagePerHit *= 1.22f;
                ByFirebePurged.Attacks.Add(melee);
                SulfuaswillbeYourEnd.Attacks.Add(melee);
                BegoneFrommyRealm.Attacks.Add(melee);
                TheTruePoweroftheFireLord.Attacks.Add(melee);

                #region By Fire be Purged!
                #region Sulfuras Smash
                /*Ragnarose faces a random player and begins prepares to smash Sulfuras on the platform. The
                 * impact creates several Lava Waves which move out in several directions from the point of
                 * impact.*/
                // 10 man - http://ptr.wowhead.com/spell=98708
                // 25 man - http://ptr.wowhead.com/spell=100256
                // 10 man heroic - http://ptr.wowhead.com/spell=100257
                // 25 man heroic - http://ptr.wowhead.com/spell=100258
                // Melee should need to worry about this since they are positioned behind the attack zone thus missing the attack
                // This should never be hit by the player

                #region Lava Wave
                /* A Lava Wave inflicts 87871 to 92379 Fire damage and knocks back all players it passes
                 * through. Targets who are knocked back suffer an additional 25106 to 26394 Fire damage
                 * every 1 sec for 5 sec.*/
                // 10 man - http://ptr.wowhead.com/spell=98928
                // 25 man - http://ptr.wowhead.com/spell=100292
                // 10 man heroic - http://ptr.wowhead.com/spell=100293
                // 25 man heroic - http://ptr.wowhead.com/spell=100294
                // This SHOULD NEVER happen
                // Iv anything it should be a movement to move around the attack
                Impedance LavaWave_Move = new Impedance
                {
                    Name = "Lava Wave Move",
                    Duration = 2f * 1000f,
                    Chance = 0.20f, // Five possible spots it can hit, thus 20% chance it will hit in your area thus having to move
                    Frequency = 30f,
                    Breakable = false,
                };
                LavaWave_Move.SetAffectsRoles_Healers();
                LavaWave_Move.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                ByFirebePurged.Moves.Add(LavaWave_Move);
                
                
                #endregion
                #endregion

                #region Wrath of Ragnaros
                /* Ragnaros targets a player, inflicting 75318 to 79182 Fire damage to all players within 6 yards
                 * and knocking them back.
                 * 
                 * *Warning* In 25 player raids, Ragnaros targets three players.*/
                // 10 man - http://ptr.wowhead.com/spell=98263
                // 25 man - http://ptr.wowhead.com/spell=100113
                // 10 man heroic - http://ptr.wowhead.com/spell=100114
                // 25 man heroic - http://ptr.wowhead.com/spell=100115
                Attack WrathofRagnaros = new Attack
                {
                    Name = "Wrath of Ragnaros",
                    AttackSpeed = 30f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = new float[] { (43875f + 46125f), (43875f + 46125f), (62156f + 65343f), (62156f + 65343f), 0f }[i] / 2f,
                    MaxNumTargets = new float[] { 1f, 3f, 1f, 3f, 0f }[i],
                    SpellID = new float[] { 98263, 100113, 100114, 100115, 0 }[i],
                };
                WrathofRagnaros.SetAffectsRoles_Healers();
                WrathofRagnaros.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                ByFirebePurged.Attacks.Add(WrathofRagnaros);
                #endregion

                #region Hand of Ragnaros
                /* Ragnaros inflicts 37659 to 39591 Fire damage to all enemies within 55 yards, knocking them
                 * back.*/
                // 10 man - http://ptr.wowhead.com/spell=98237
                // 25 man - http://ptr.wowhead.com/spell=100383
                // 10 man heroic - http://ptr.wowhead.com/spell=100384
                // 25 man heroic - http://ptr.wowhead.com/spell=100387
                // Only affects Melee and Tanks
                Attack HandofRagnaros = new Attack
                {
                    Name = "Hand of Ragnaros",
                    AttackSpeed = 25f,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = new float[] { (21937f + 23062f), (21937f + 23062f), (33150f + 34850f), (33150f + 34850f), 0f }[i] / 2f,
                    MaxNumTargets = Max_Players[i],
                    SpellID = new float[] { 98237, 100383, 100384, 100387, 0 }[i],
                };
                HandofRagnaros.AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                HandofRagnaros.SetAffectsRoles_Tanks();
                ByFirebePurged.Attacks.Add(HandofRagnaros);
                SulfuaswillbeYourEnd.Attacks.Add(HandofRagnaros);
                BegoneFrommyRealm.Attacks.Add(HandofRagnaros);
                #endregion

                #region Magma Trap
                /*Ragnaros periodically forms a Magmaw Trap at a random player's location. The Magma Trap
                 * persists for the duration of the battle, and will trigger when stepped on, causing a Magma Trap
                 * Eruption.*/
                // http://ptr.wowhead.com/spell=98164
                // damage - http://ptr.wowhead.com/spell=98170
                // summons after damage - http://ptr.wowhead.com/npc=53086
                // Normal we are popping them immediatly on spawn
                // On Heroic we need to wait 45 seconds before popping the next trap.
                // That means on the bear minimum, we need to pop 4 traps in Phase 1 and 3 traps in phase 2
                
                #region Magma Trap Eruption
                /* When triggered, a Magma Trap erupts for 75318 to 79182 Fire damage to all enemies
                 * within the Firelands, and violently knocking the player who tripped the Magma Trap into the air.
                 * 
                 * *Heroic* An enemy that triggers a Magma Trap will take 50% additional fire damage from the Magma
                 * Trap Eruption for 45 sec. Stacks.*/
                // This means we cannot pop the next trap until the debuf wears off or 45 second cooldown
                // This debuff ONLY increases the damage of your next Magma Trap damage, not overall fire damage
                /* Eruption
                 * 10 man - http://ptr.wowhead.com/spell=98175
                 * 25 man - http://ptr.wowhead.com/spell=100106
                 * 10 man heroic - http://ptr.wowhead.com/spell=100107
                 * 25 man heroic - http://ptr.wowhead.com/spell=100108 */
                // Timers are adjusted by 16 seconds
                Attack MagmaTrapEruption = new Attack
                {
                    Name = "Magma Trap Eruption",
                    AttackSpeed = (phase1to3Length/ (phase1to3Length - 16f)) * (i < 2 ? 25f : 45f),
                    DamagePerHit = new float[] { (58500f + 61500f), (58500f + 61500f), (82875f + 87125f), (97500f + 102500f), 0f }[i] / 2f,
                    MaxNumTargets = Max_Players[i],
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    SpellID = new float[] { 98175, 100106, 100107, 100108, 0 }[i],
                };
                MagmaTrapEruption.SetAffectsRoles_All();
                ByFirebePurged.Attacks.Add(MagmaTrapEruption);

                Attack MagmaTrapEruptionPhase2 = MagmaTrapEruption.Clone();
                // At most 3 traps should be up during phase 2 on Heroic
                MagmaTrapEruptionPhase2.AttackSpeed = phase1to3Length / 3f;
                if (i > 1) { SulfuaswillbeYourEnd.Attacks.Add(MagmaTrapEruptionPhase2); }
                /* Vulnerability
                 * 10-man Heroic - http://ptr.wowhead.com/spell=100238
                 * 25-man heroic - http://ptr.wowhead.com/spell=100239 */
                // Not going to model this part since I'm factoring in the 45 second debuff into my attack speed
                #endregion
                #endregion

                #region Magma Blast
                /* Ragnaros blasts magma at his current target if he cannot reach and melee them, 
                 * inflicting 73125 to 76875 Fire damage and increasing Fire damage taken by 50% for 6 sec. */
                // http://ptr.wowhead.com/spell=98313
                // This should NEVER happen
                #endregion

                #region Burning Wound
                /* The attacks from Ragnaros open a Burning Wound, inflicting 3656 to 3843 Fire damage every 2 sec
                 * for 20 sec.  The wound causes the target's attacks to become infused with fire, causing a
                 * Burning Blast.*/
                // 10-man - http://ptr.wowhead.com/spell=99399
                // 25-man - http://ptr.wowhead.com/spell=101238
                // 10-man Heroic - http://ptr.wowhead.com/spell=101239
                // 25-man Heroic - http://ptr.wowhead.com/spell=101240
                // 5 second cooldown on application
                // Only allow 4 stacks to be applied
                float BurningWoundDamage = new float[] { (3656f + 3843f), (3656f + 3843f), (5386f + 5663f), (5386f + 5663f), 0f }[i] / 2f;
                Attack BurningWound = new Attack
                {
                    Name = "Burning Wound",
                    AttackSpeed = 40f,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamageType = ItemDamageType.Fire,
                    // Average damage after the stacking equates to ABOUT 1.3 times the base damage
                    DamagePerTick = BurningWoundDamage * 1.3f,
                    TickInterval = 2f,
                    Duration = 20f + 20f, // 20 seconds to stack the debuff, 20 seconds to let it drop
                    IsDoT = true,
                    SpellID = new float[] { 99399, 101238, 101239, 101240, 0 }[i],
                };
                BurningWound.SetAffectsRoles_Tanks();
                ByFirebePurged.Attacks.Add(BurningWound);

                #region Burning Blast
                /* Inflicts 1950 to 2050 Fire damage, each stack of Burning Wound increases the damage of
                 * Burning Blast.*/
                // 10-man - http://ptr.wowhead.com/spell=99400
                // 25-man - http://ptr.wowhead.com/spell=101241
                // 10-man Heroic - http://ptr.wowhead.com/spell=101242
                // 25-man Heroic - http://ptr.wowhead.com/spell=101243
                float BurningBlastDamaage = (1950f + 2050f) / 2f;
                Stats BurningBlastStat = new Stats();
                SpecialEffect BurningBlastSpecialEffect = new SpecialEffect(Trigger.PhysicalAttack, new Stats() { FireDamage = BurningBlastDamaage }, 20f, 5f, 1f, 4);
                BurningBlastStat.AddSpecialEffect(BurningBlastSpecialEffect);
                BurningBlastSpecialEffect = new SpecialEffect(Trigger.Use, BurningBlastStat, 40f, 40f, 1f);
                BurningBlastStat = new Stats();
                BurningBlastStat.AddSpecialEffect(BurningBlastSpecialEffect);
                BuffState BurningBlast = new BuffState
                {
                    Name = "Burning Blast",
                    Frequency = 40f,
                    Duration = 40f * 1000f,
                    Chance = 1f,
                    Breakable = false,
                    Stats = BurningBlastStat,
                };
                BurningBlast.SetAffectsRoles_Tanks();
                ByFirebePurged.BuffStates.Add(BurningBlast);
                #endregion
                #endregion
                #endregion

                #region Intermission: Minions of Fire!
                /* At 70% health, Ragnaros will cast Splitting Blow wedging Sulfuras into the platform and creating
                 * Sons of Flame across the platform. Ragnaros will stay submerged for 45 seconds or until all of the
                 * Sons of Flame are destroyed.*/

                #region Splitting Blow
                /* Ragnaros buries Sulfuras within the platform, creating Sons of Flame that attempt to reach the
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
                // The damage from Splitting Blow should NEVER hit a player
                #endregion

                #region Son of Flame
                /*Sons of Flame will cross the platform and attempt to reform with Sulfuras, causing a
                 * Supernova if they are able to reach the mighty weapon.*/
                // http://ptr.wowhead.com/npc=53140
                TargetGroup SonsofFlame = new TargetGroup
                {
                    Name = "Sons of Flame",
                    Chance = 1f,
                    Frequency = 1f,
                    Duration = IntermissionLength * 1000f,
                    LevelOfTargets = 87,
                    NearBoss = false,
                    NumTargs = 10f,
                    TargetID = 53140f,
                };
                SonsofFlame.SetAffectsRoles_All();
                Intermission1.Targets.Add(SonsofFlame);

                #region Burning Speed
                /* Sons of Flame move faster as their blaze burns hotter. Their movement speed is
                 * increased by an amount equal to every 1% health they have above 50% health.*/
                // 10 man - http://ptr.wowhead.com/spell=99414
                // 25 man - http://ptr.wowhead.com/spell=100306
                // 10 man heroic - http://ptr.wowhead.com/spell=100307
                // 25 man heroic - http://ptr.wowhead.com/spell=100308
                // Don't Need to model
                #endregion

                #region Supernova
                /* If a Son of Flame reaches Sulfuras, the elemental will explode in a Supernova,
                 * inflicting 112978 to 118772 Fire damage to all players within the Firelands.*/
                // 10 man - http://ptr.wowhead.com/spell=99112
                // 25 man - http://ptr.wowhead.com/spell=100259
                // 10 man heroic - http://ptr.wowhead.com/spell=100260
                // 25 man heroic - http://ptr.wowhead.com/spell=100261
                // Since if this happens at ANY time, it's a wipe, so assuming that this does not take place
                #endregion
                #endregion

                #region Lava Bolt
                /* While Ragnaros lies submerged underneath the lava, bolts of hot magma fall on four random players every 4 seconds.
                 * A Lava Bolt inflicts 43875 to 46125 Fire Damage.
                 * 
                 * In 25 player raids, bolts of hot magma fall on ten players.*/
                // 10-man - http://ptr.wowhead.com/spell=98981
                // 25-man - http://ptr.wowhead.com/spell=100289
                // 10-man Heroic - http://ptr.wowhead.com/spell=100290
                // 25-man Heroic - http://ptr.wowhead.com/spell=100291
                Attack LavaBolt = new Attack
                {
                    Name = "Lava Bolt",
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    AttackSpeed = 4f,
                    DamagePerHit = new float[] { (43875f + 46125f), (43875f + 46125f), (68250f + 71750f), (68250f + 71750f), 0 }[i] / 2f,
                    MaxNumTargets = new float[] { 4, 10, 4, 10, 0 }[i],
                    SpellID = new float[] { 98981, 100289, 100290, 100291, 0 }[i],
                };
                LavaBolt.SetAffectsRoles_All();
                Intermission1.Attacks.Add(LavaBolt);
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

                #region Lava Wave
                /* A Lava Wave inflicts 87871 to 92379 Fire damage and knocks back all players it passes
                 * through. Targets who are knocked back suffer an additional 25106 to 26394 Fire damage
                 * every 1 sec for 5 sec.*/
                // 10 man - http://ptr.wowhead.com/spell=98928
                // 25 man - http://ptr.wowhead.com/spell=100292
                // 10 man heroic - http://ptr.wowhead.com/spell=100293
                // 25 man heroic - http://ptr.wowhead.com/spell=100294
                SulfuaswillbeYourEnd.Moves.Add(LavaWave_Move);
                #endregion
                #endregion

                #region World of Flames [Heroic Only]
                /* Ragnaros periodically engulfs one third of the platform in flame every 2.60 sec for 13 sec.
                 * Inflicting 87871 to 92379 Fire damage to players caught in the conflagration.*/
                // cast - http://ptr.wowhead.com/spell=100171
                // spell - http://ptr.wowhead.com/spell=99171
                #endregion

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

                #region Magma Blast
                /* Ragnaros blasts magma at his current target if he cannot reach and melee them, 
                 * inflicting 73125 to 76875 Fire damage and increasing Fire damage taken by 50% for 6 sec. */
                // http://ptr.wowhead.com/spell=98313
                // This should NEVER happen
                #endregion

                #region Burning Wound
                /* The attacks from Ragnaros open a Burning Wound, inflicting 3656 to 3843 Fire damage every 2 sec
                 * for 20 sec.  The wound causes the target's attacks to become infused with fire, causing a
                 * Burning Blast.*/
                // 10-man - http://ptr.wowhead.com/spell=99399
                // 25-man - http://ptr.wowhead.com/spell=101238
                // 10-man Heroic - http://ptr.wowhead.com/spell=101239
                // 25-man Heroic - http://ptr.wowhead.com/spell=101240
                // 5 second cooldown on application
                // Only allow 4 stacks to be applied
                SulfuaswillbeYourEnd.Attacks.Add(BurningWound);

                #region Burning Blast
                /* Inflicts 1950 to 2050 Fire damage, each stack of Burning Wound increases the damage of
                 * Burning Blast.*/
                // 10-man - http://ptr.wowhead.com/spell=99400
                // 25-man - http://ptr.wowhead.com/spell=101241
                // 10-man Heroic - http://ptr.wowhead.com/spell=101242
                // 25-man Heroic - http://ptr.wowhead.com/spell=101243
                SulfuaswillbeYourEnd.BuffStates.Add(BurningBlast);
                #endregion
                #endregion
                #endregion

                #region Intermission: Denizens of Flame!
                /* At 40% health, Ragnaros will cast Splitting Blow, wedging Sulfuras into the platform and creating
                 * Sons of Flame. Ragnaros will stay submerged for 45 seconds or until all of the Sons of Flame are
                 * destroyed.*/

                #region Splitting Blow
                /* Ragnaros buries Sulfuras within the platform, creating Sons of Flame that attempt to reach the
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
                // The damage from Splitting Blow should NEVER hit a player
                #endregion

                #region Son of Flame
                /*Sons of Flame will cross the platform and attempt to reform with Sulfuras, causing a
                 * Supernova if they are able to reach the mighty weapon.*/
                // http://ptr.wowhead.com/npc=53140
                Intermission2.Targets.Add(SonsofFlame);

                #region Burning Speed
                /* Sons of Flame move faster as their blaze burns hotter. Their movement speed is
                 * increased by an amount equal to every 1% health they have above 50% health.*/
                // 10 man - http://ptr.wowhead.com/spell=99414
                // 25 man - http://ptr.wowhead.com/spell=100306
                // 10 man heroic - http://ptr.wowhead.com/spell=100307
                // 25 man heroic - http://ptr.wowhead.com/spell=100308
                // Don't need to model
                #endregion

                #region Supernova
                /* If a Son of Flame reaches Sulfuras, the elemental will explode in a Supernova,
                 * inflicting 112978 to 118772 Fire damage to all players within the Firelands.*/
                // 10 man - http://ptr.wowhead.com/spell=99112
                // 25 man - http://ptr.wowhead.com/spell=100259
                // 10 man heroic - http://ptr.wowhead.com/spell=100260
                // 25 man heroic - http://ptr.wowhead.com/spell=100261
                // Since if this happens at ANY time, it's a wipe, so assuming that this does not take place
                #endregion
                #endregion

                #region Lava Scion
                // One Lava Scion will form on each side of the platform.
                // http://ptr.wowhead.com/npc=53231
                TargetGroup LavaScion = new TargetGroup
                {
                    Name = "Lava Scion",
                    Chance = 1f,
                    Duration = 60f * 1000f,
                    Frequency = 1f,
                    LevelOfTargets = 87,
                    NearBoss = true,
                    NumTargs = 2f,
                    TargetID = 53231,
                };
                LavaScion.SetAffectsRoles_All();
                Intermission2.Targets.Add(LavaScion);

                #region Blazing Heat
                /* The Lava Scion inflicts a random target with Blazing Heat, causing them to create a trail of
                 * Blazing Heat in their wake. Blazing Heat inflicts 62765 to 65985 Fire damage every 1
                 * sec, and heals Sons of Flame and Lava Scions for 10% every 1 sec.*/
                /* Damaging Part
                 * 10 man - http://ptr.wowhead.com/spell=99144
                 * 25 man - http://ptr.wowhead.com/spell=100303
                 * 10 man heroic - http://ptr.wowhead.com/spell=100304
                 * 25 man heroic - http://ptr.wowhead.com/spell=100305*/
                // Healing Part - http://ptr.wowhead.com/spell=99145
                // Just assume 2 people get chosen with the debuff and have to move for the entire duration
                // Blazing heat has a 3 second cast and a 9 second duration. So, 12 seconds movement
                // Not going to model the damaing part or the Healing Part
                Impedance BlazingHeat_Move = new Impedance
                {
                    Name = "Blazing Heat Move",
                    Breakable = false,
                    Chance = 2f / (Max_Players[i] - Min_Tanks[i]),
                    Duration = 12f * 1000f,
                    Frequency = IntermissionLength - 12f,
                };
                BlazingHeat_Move.SetAffectsRoles_DPS();
                BlazingHeat_Move.SetAffectsRoles_Healers();
                Intermission2.Moves.Add(BlazingHeat_Move);
                #endregion
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

                #region Lava Wave
                /* A Lava Wave inflicts 87871 to 92379 Fire damage and knocks back all players it passes
                 * through. Targets who are knocked back suffer an additional 25106 to 26394 Fire damage
                 * every 1 sec for 5 sec.*/
                // 10 man - http://ptr.wowhead.com/spell=98928
                // 25 man - http://ptr.wowhead.com/spell=100292
                // 10 man heroic - http://ptr.wowhead.com/spell=100293
                // 25 man heroic - http://ptr.wowhead.com/spell=100294
                BegoneFrommyRealm.Moves.Add(LavaWave_Move);
                #endregion
                #endregion

                #region World of Flames [Heroic Only]
                /* Ragnaros periodically engulfs one third of the platform in flame every 2.60 sec for 13 sec.
                 * Inflicting 87871 to 92379 Fire damage to players caught in the conflagration.*/
                // cast - http://ptr.wowhead.com/spell=100171
                // spell - http://ptr.wowhead.com/spell=99171
                #endregion

                #region Summon Living Meteor
                /* Ragnaros calls down an increasing number of Living Meteors over time, inclicting 81595 to
                 * 85780 Fire damage to players within 5 yards of the location.*/
                // http://ptr.wowhead.com/spell=99268

                #region Living Meteor
                /* The Living Meteor will fixate on a random target and chase them. A player that gets
                 * within 4 yards of the Living Meteor will trigger a Metoer Impact, inflicting 627656 to
                 * 659844 Fire damage to enemies within 8 yards.*/
                // http://ptr.wowhead.com/npc=53500
                // 10 man - http://ptr.wowhead.com/spell=99317
                // 25 man - http://ptr.wowhead.com/spell=100989
                // 10 man heroic - http://ptr.wowhead.com/spell=100990
                // 25 man heroic - http://ptr.wowhead.com/spell=100991
                TargetGroup LivingMeteor = new TargetGroup
                {
                    Name = "Living Meteor",
                    Chance = 1f,
                    Duration = phase1to3Length * 1000f,
                    Frequency = 45f,
                    LevelOfTargets = 87,
                    NearBoss = false,
                    NumTargs = 1f,
                    TargetID = 53500,
                };
                LivingMeteor.SetAffectsRoles_All();
                BegoneFrommyRealm.Targets.Add(LivingMeteor);

                #region Meteor Impact
                /* A player that gets within 4 yards of the Living Meteor will trigger a Metoer Impact,
                 * inflicting 627656 to 659844 Fire damage to enemies within 8 yards.*/
                // 10 man - http://ptr.wowhead.com/spell=99287
                // 25 man - http://ptr.wowhead.com/spell=100299
                // 10 man heroic - http://ptr.wowhead.com/spell=100300
                // 25 man heroic - http://ptr.wowhead.com/spell=100301
                // THIS SHOULD NEVER HAPPEN!!!!
                #endregion

                #region Combustible
                /* The Living Meteor is highly Combustible. When attacked, it will cause Combustion,
                 * knocking it back several yards away from the enemy that hit it. Combustible is
                 * removed for several seconds after Combustion is triggered.*/

                #region Combustion
                /* While combustible is active, the Living Meteor is knocked back several yards
                 * from the enemy that hit it.*/
                // 10 man - http://ptr.wowhead.com/spell=99296
                // 25 man - http://ptr.wowhead.com/spell=100282
                // 10 man heroic - http://ptr.wowhead.com/spell=100283
                // 25 man heroic - http://ptr.wowhead.com/spell=100284
                #endregion

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

                #region Magma Blast
                /* Ragnaros blasts magma at his current target if he cannot reach and melee them, 
                 * inflicting 73125 to 76875 Fire damage and increasing Fire damage taken by 50% for 6 sec. */
                // http://ptr.wowhead.com/spell=98313
                // This should NEVER happen
                #endregion

                #region Burning Wound
                /* The attacks from Ragnaros open a Burning Wound, inflicting 3656 to 3843 Fire damage every 2 sec
                 * for 20 sec.  The wound causes the target's attacks to become infused with fire, causing a
                 * Burning Blast.*/
                // 10-man - http://ptr.wowhead.com/spell=99399
                // 25-man - http://ptr.wowhead.com/spell=101238
                // 10-man Heroic - http://ptr.wowhead.com/spell=101239
                // 25-man Heroic - http://ptr.wowhead.com/spell=101240
                // 5 second cooldown on application
                // Only allow 4 stacks to be applied
                BegoneFrommyRealm.Attacks.Add(BurningWound);

                #region Burning Blast
                /* Inflicts 1950 to 2050 Fire damage, each stack of Burning Wound increases the damage of
                 * Burning Blast.*/
                // 10-man - http://ptr.wowhead.com/spell=99400
                // 25-man - http://ptr.wowhead.com/spell=101241
                // 10-man Heroic - http://ptr.wowhead.com/spell=101242
                // 25-man Heroic - http://ptr.wowhead.com/spell=101243
                BegoneFrommyRealm.BuffStates.Add(BurningBlast);
                #endregion
                #endregion
                #endregion

                #region The True Power of the Fire Lord [Heroic Only]
                /* The Fire lord unleashes his full power and is able to move freely around the platform. Players have
                 * the aid of powerful heroes of Azeroth to support them.*/

                #region Superheated [Heroic Only]
                /* Ragnaros is at his full power and is now Superheated, inflicting 2510 to 2640 Fire
                 * damage every 1, increasing damage taken from Superheated by 10%. Stacks.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100594
                // 25 man heroic - http://ptr.wowhead.com/spell=100915
                #endregion

                #region Empower Sulfuras [Heroic Only]
                /*Ragnaros begins to empower Sulfuras. After 5 sec, Sulfuras becomes Empowered and attacks
                 * made by Ragnaros cause Flames of Sulfuras, inflicting 627656 to 659844 Fire damage to all
                 * enemies within the Firelands.*/
                /* Cast
                 * 10 man heroic - http://ptr.wowhead.com/spell=100604
                 * 25 man heroic - http://ptr.wowhead.com/spell=100997 */
                // Damage - http://ptr.wowhead.com/spell=100628
                
                #region Flames of Sulfuras [Heroic Only]
                /* When Sulfuras is Empowered, attacks made by Ragnaros cause Flames of Sulfuras,
                 * inflicting 627656 to 659844 Fire damage to all enemies within the Firelands.*/
                // http://ptr.wowhead.com/spell=100630
                #endregion
                #endregion

                #region Dreadflame [Heroic Only]
                /* Sulfuras creates a Dreadflame at two nearby locations. The Dreadflame multiplies rapidly
                 * and spreads across the platform. Dreadflame inflicts 43935 to 46190 Fire damage and an
                 * additional 4506 to 4507 Fire damage every 1 sec for 30 sec.
                 * 
                 * *Warning* In 25 player raids, Dreadflame will strike five locations at once.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100941
                // 25 man heroic - http://ptr.wowhead.com/spell=100998
                #endregion

                #region Magma Geyser [Heroic Only]
                /* Ragnaros will target a Magma Geyser whenever he notices four players in a cluster together. The
                 * Magma Geyser inflicts 69042 to 72583 Fire damage every 1 sec and destroy any nearby
                 * Breadth of Frost.
                 * 
                 * *Warning* In 25 player raids, Ragnaros will use Magma Geyser on clusters of 10 players.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100861
                // 25 man heroic - http://ptr.wowhead.com/spell=100999
                #endregion

                #region Cenarius [Heroic Only]
                /* Cenarius is a demigod, the son of Malorne and Elune, and the patron of all of Azeroth's druids.
                 * 
                 * Cenarius will support the raid by freezing Living Meteors and reducing the damage caused by
                 * Superheated.*/
                // http://ptr.wowhead.com/npc=53872

                #region Breadth of Frost [Heroic Only]
                /* Cenarius forms a Breadth of Frost at a nearby location. Any Living Meteors that enter the
                 * Breadth of Frost are Frozen and take 15000% additional damage. Additionally, players who
                 * stand within the Breadth of Frost are immune to Superheated damage and have the
                 * Superheaded debuff removed from them.*/
                // Living Meteor - http://ptr.wowhead.com/spell=100567
                // Shield from Superheated - http://ptr.wowhead.com/spell=100503
                #endregion
                #endregion

                #region Arch Druid Hamuul Runetotem [Heroic Only]
                /* Humuul Runetotem is a tauren druid and leads the druids of Thunder Bluff. In Mount Hyjal, he
                 * assists Ysera in protecting Nordrassil from Ragnaros.
                 * 
                 * The arch druid will support the raid by entrapping Ragnaros.*/
                // http://ptr.wowhead.com/npc=53913

                #region Entrapping Roots [Heroic Only]
                /* Arch Druid Humuul Runetotem forms Entrapping Roots at a nearby location. If
                 * Ragnaros enters the area of the Entrapping Roots, he will become stunned for 10 sec and
                 * take 50% additional damage while stunned.*/
                // Cast - http://ptr.wowhead.com/spell=100646
                // Stun - http://ptr.wowhead.com/spell=100653
                #endregion
                #endregion

                #region Malfurion Stormrage [Heroic Only]
                /* Malfurion Stormrage is an ancient and powerful night elf druid. He leads the army of Cenarius in
                 * the Defense of Mount Hyjal against the forces of Ragnaros.
                 * 
                 * Malfurion will support the raid by protecting players from Dreadflame.*/
                // http://ptr.wowhead.com/npc=52135

                #region Cloudburst [Heroic Only]
                /* Malfurion forms a Cloudburst. The players who interact with the Cloudburst will be
                 * surrounded with a Deluge. Deluge makes the player immune to Dreadflame damage, and
                 * also allows the player to extinguish any nearby Dreadflame.
                 * 
                 * *Warning* In 25 player raids, up to three players can use a single Cloudburst.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100758
                // 25 man heroic - http://ptr.wowhead.com/spell=100766

                #region Deluge [Heroic Only]
                /* Makes the player immune to Dreadflame damage, and also allows the player to
                 * extinguish any nearby Dreadflame.*/
                // 10 man heroic - http://ptr.wowhead.com/spell=100713
                // 25 man heroic - http://ptr.wowhead.com/spell=101015
                // Spell that extinguishes the Dreadflame - http://ptr.wowhead.com/spell=100757
                #endregion
                #endregion
                #endregion
                #endregion

                #region Apply Phases
                // Pull then at 30 Sec, Empower Blade w/ 50/50 chance for which blade type.
                // 15 secs of empowered blade
                // Return to normal mode.
                float phasestart = 0;
                ApplyAPhasesValues(ByFirebePurged, i, 1, phasestart, phase1to3Length, BerserkTimer[i]); phasestart += phase1to3Length;
                ApplyAPhasesValues(Intermission1, i, 2, phasestart, IntermissionLength, BerserkTimer[i]); phasestart += IntermissionLength;
                ApplyAPhasesValues(SulfuaswillbeYourEnd, i, 3, phasestart, phase1to3Length, BerserkTimer[i]); phasestart += phase1to3Length;
                ApplyAPhasesValues(Intermission2, i, 4, phasestart, IntermissionLength, BerserkTimer[i]); phasestart += IntermissionLength;
                ApplyAPhasesValues(BegoneFrommyRealm, i, 5, phasestart, phase1to3Length, BerserkTimer[i]); phasestart += phase1to3Length;
                if (i > 1)
                {
                    ApplyAPhasesValues(Intermission3, i, 6, phasestart, Intermission3Length, BerserkTimer[i]); phasestart += Intermission3Length;
                    ApplyAPhasesValues(TheTruePoweroftheFireLord, i, 7, phasestart, phase4Length, BerserkTimer[i]);
                }
                AddAPhase(ByFirebePurged, i);
                AddAPhase(Intermission1, i);
                AddAPhase(SulfuaswillbeYourEnd, i);
                AddAPhase(Intermission2, i);
                AddAPhase(BegoneFrommyRealm, i);
                if (i > 1)
                {
                    AddAPhase(Intermission3, i);
                    AddAPhase(TheTruePoweroftheFireLord, i);
                }
                #endregion

            }
            #endregion
            #endregion
            #region Defensive
            Resist_Physical = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Frost = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Fire = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Nature = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Arcane = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Shadow = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Resist_Holy = new double[] { 0.00f, 0.00f, 0, 0, 0 };
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
            /* TODO:
             */
        }
    }
    #endregion

}
