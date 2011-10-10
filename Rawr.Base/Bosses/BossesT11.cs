using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Rawr.Bosses
{
    /* Phase timings work kind of like this:
    SpecialEffect primary = new SpecialEffect(Trigger.Use, new Stats(), 90, 30);
    SpecialEffect secondary = new SpecialEffect(Trigger.Use, new Stats() { FireDamage = 50000f }, 0, 15);
    primary.Stats.AddSpecialEffect(secondary);*/

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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            Health = new float[] { 21473000f, 64419000f, 0, 0, 0 };
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
                // He won't cast other abilities while channelling Fel Firestorm
                Phase EntireFight = new Phase() { Name = "While not casting Firestorm" };
                Phase FelFirestorm = new Phase() { Name = "During Fel Firestorm" };

                #region MT and OT Melee Swapping
                // MT and OT tank swap
                // Each should take half of the total damage
                // does not melee during Firestorm
                EntireFight.Attacks.Add(new Attack {
                    Name = "MT Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE, IsTheDefaultMelee = true,
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2.0f,
                });
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.MainTank] = true;

                EntireFight.Attacks.Add(new Attack {
                    Name = "OT Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE, IsTheDefaultMelee = true,
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    MaxNumTargets = 1f,
                    AttackSpeed = 2.0f,
                });
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.OffTank] = true;
                #endregion

                #region Meteor Slash
                // Meteor Slash - Deals 200000/475000 Fire damage split between enemy targets within 65 yards in front of the caster.
                //     Increases Fire damage taken to all targets affected by 100%.
                //     Half the raid stacks on one side and takes the debuff for 1 stack
                /* 10 man - http://www.wowhead.com/spell=88942
                   25 man - http://www.wowhead.com/spell=95172 */
                // TODO: Add Fire taken damage debuff
                EntireFight.Attacks.Add(new Attack {
                    Name = "Meteor Slash [Group 1]",
                    SpellID = new float[] { 88942, 95172, 0, 0 }[i],
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    // Half the raid is getting hit with about 40k damage per attack.
                    DamagePerHit = new float[] { 200000, 475000, }[i] / (Max_Players[i] / 2),
                    MaxNumTargets = Max_Players[i] / 2f,
                    // Frequency is 16.5 seconds, with a 1.25 second cast time - 17.75 seconds, Verified DBM v4.74-r5279
                    // Bigwigs is registering a 17 second frequency - release # 8327
                    AttackSpeed = 16.5f + 1.25f,
                });
                EntireFight.LastAttack.SetUnavoidable();
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.TertiaryTank] = true;
                EntireFight.LastAttack.SetAffectsRoles_DPS();
                EntireFight.LastAttack.SetAffectsRoles_Healers();

                EntireFight.Attacks.Add(new Attack {
                    Name = "Meteor Slash [Group 2]",
                    SpellID = new float[] { 88942, 95172, 0, 0 }[i],
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    // Half the raid is getting hit with about 40k damage per attack.
                    DamagePerHit = new float[] { 200000, 475000, }[i] / (Max_Players[i] / 2),
                    MaxNumTargets = Max_Players[i] / 2f,
                    // Frequency is 16.5 seconds, with a 1.25 second cast time - 17.75 seconds, Verified DBM v4.74-r5279
                    // Bigwigs is registering a 17 second frequency - release # 8327
                    AttackSpeed = 16.5f + 1.25f,
                });
                EntireFight.LastAttack.SetUnavoidable();
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntireFight.LastAttack.AffectsRole[PLAYER_ROLES.TertiaryTank] = true;
                EntireFight.LastAttack.SetAffectsRoles_DPS();
                EntireFight.LastAttack.SetAffectsRoles_Healers();
                #endregion

                #region Consuming Darkness
                // Consuming Darkness - The Shambling Doom inflicts 2,925 to 3,075 Shadow damage and additional Shadow damage every 0.5 sec for 15 sec. 100 yard range. Instant
                // Damage appears to increase additively as the dot progress:
                // .5s = 3k, 1s = 6k, 1.5s = 9k, ..., 15s = 90k
                // total damage for the full duration is 1,395,000
                // Can be dispelled
                /* 10 man - http://www.wowhead.com/spell=88954
                   25 man - http://www.wowhead.com/spell=95173 */
                EntireFight.Attacks.Add(new Attack
                {
                    Name = "Consuming Darkness",
                    SpellID = new float[] { 88954, 95173, 0, 0 }[i],
                    IsDoT = true,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamageType = ItemDamageType.Shadow,
                    //DamagePerHit = (3900f + 4100f) / 2f,
                    // Assume it takes 5 seconds to remove all.
                    Duration = 5f, //15f,
                    TickInterval = .5f,
                    //DamagePerTick = 165000f / 10f,
                    DamagePerTick = (3900f + 4100f) / 2f,
                    //Interruptable = true,
                    MaxNumTargets = new float[] { 3, 7, }[i],
                    // Berserk time minus Firestorm and Firestorm cast time (Firestorm (15 second spell + 3 second cast time; spell is cast 2 times in the fight) - 264 seconds
                    // Frequency is 22 seconds - 22 seconds
                    // Divide those two numbers to get the number of times in the fight that the spell is cast - 12
                    // all dividing the Berserk timer (should come up with 25 second attack speed)
                    // Deadly Boss mods is reporting a 24 second cooldown - Release # 5621
                    AttackSpeed = 24f,
                });
                EntireFight.LastAttack.SetUnavoidable();
                EntireFight.LastAttack.SetAffectsRoles_DPS();
                EntireFight.LastAttack.SetAffectsRoles_Healers();
                #endregion

                #region Fel Firestorm
                // Fel Firestorm - Used at 66% and 33%. Argaloth shoots fireballs into the air which will fall down and leave a patch of flame on the ground.
                //        These patches deal 8,287 - 8,712 Fire damage as long as you are standing in them and they can appear (or will only appear?) directly under a players feet.
                //        Most of the room will be covered in flames at the end of this ability (think of Mimiron hardmode) before all flames will disappear and he will resume
                //        attacking you with his normal abilities. 50,000 yards range. 3 sec cast. Lasts 15 seconds.
                // http://www.wowhead.com/spell=88972
                // Places Fel Flames on the ground every 3 seconds
                // 10 man - http://www.wowhead.com/spell=89000
                // 25 man - http://www.wowhead.com/spell=95177
                FelFirestorm.Attacks.Add(new Attack
                {
                    Name = "Fel Firestorm",
                    SpellID = new float[] { 89000, 95177, 0, 0 }[i],
                    IsDoT = true,
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    TickInterval = 1f,
                    Duration = 15f,
                    // Adjusting damage to take into account for the 100% fire damage debuff from Meteor Strike Debuff
                    DamagePerTick = ((11700f + 12300f) / 2f) * 2,
                    MaxNumTargets = Max_Players[i],
                    AttackSpeed = 15+3,
                });
                FelFirestorm.LastAttack.SetUnavoidable();
                FelFirestorm.LastAttack.SetAffectsRoles_All();
                FelFirestorm.Moves.Add(new Impedance() {
                    Name = "Fel Firestorm (Getting Away)",
                    Frequency = 15 + 3,
                    // Fires are still around 3 seconds after he finishes channeling the spell and people need to get back to their groups
                    Duration = (15f + 3f) * 1000f,
                    Chance = 1f,
                    Breakable = true, // Movement is always breakable
                    AffectsRole = FelFirestorm.LastAttack.AffectsRole,
                });
                #endregion

                #region Apply Phases
                // 5 minute Berserk Timer with Firestorm only last 18 seconds twice.
                // Pushing distro of normal fighting to more in the first two to account for executes
                int phaseStartTime = 0;
                ClearPhase1Values( FelFirestorm);
                ApplyAPhasesValues( EntireFight,  i, 1, phaseStartTime, 92, this[i].BerserkTimer); phaseStartTime += 92;
                ApplyAPhasesValues( FelFirestorm, i, 2, phaseStartTime, 18, this[i].BerserkTimer); phaseStartTime += 18;
                ApplyAPhasesValues( EntireFight,  i, 3, phaseStartTime, 92, this[i].BerserkTimer); phaseStartTime += 92;
                ApplyAPhasesValues( FelFirestorm, i, 4, phaseStartTime, 18, this[i].BerserkTimer); phaseStartTime += 18;
                ApplyAPhasesValues( EntireFight,  i, 5, phaseStartTime, 80, this[i].BerserkTimer);
                AddAPhase(EntireFight, i);
                AddAPhase(FelFirestorm, i);
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Health values were lowered on Normal by 20% in 4.2
            Health = new float[] { 33497880f * 0.80f, 81082048f, 46895800f, 120016400f, 0 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 6 * 60, 6 * 60, 6 * 60, 6 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.70f, 0.70f, 0.70f, 0.70f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 3, 3, 0 };
            Min_Healers = new int[] { 3, 5, 3, 6, 0 };
            #endregion
            #region Phase Info
            for (int i = 0; i < 4; i++)
            {
                Phase NotBurnExposedHeadNormal = new Phase() { Name = "Not Burning Exposed Head", };
                Phase NotBurnExposedHeadHeroicAbove30 = new Phase() { Name = "Not Burning Exposed Head (Above 30%)", };
                Phase NotBurnExposedHeadHeroicUnder30 = new Phase() { Name = "Not Burning Exposed Head (Under 30%)", };
                Phase BurnExposedHead = new Phase() { Name = "Burning Exposed Head", };

                // JOTHAY TODO: Review phase times to ensure accuracy based on new data from DBM

                #region Not Burning Exposed Head
                // Adding Melee as constant for the whole fight until I read otherwise
                NotBurnExposedHeadNormal.Attacks.Add(GenAStandardMelee(this[i].Content));
                NotBurnExposedHeadNormal.LastAttack.DamagePerHit *= 1.3f;
                NotBurnExposedHeadNormal.LastAttack.AttackSpeed = 3f;
                NotBurnExposedHeadHeroicAbove30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                NotBurnExposedHeadHeroicUnder30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                #region Magma Spit
                // Magmaw will constantly cast Magma Spit at random players, as well as Lava Spew. This must be healed through
                NotBurnExposedHeadNormal.Attacks.Add(new Attack {
                    Name = "Magma Spit",
                    SpellID = new float[] { 78068, 91917, 91927, 91928 }[i],
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = new float[] { (24500f + 31500f), (24500f + 31500f), (35000f + 45000f), (39375f + 50625f) }[i] / 2f,
                    MaxNumTargets = new float[] { 2f, 8f, 2f, 8f }[i],
                    AttackSpeed = 10f,
                });
                NotBurnExposedHeadNormal.LastAttack.AffectsRole[PLAYER_ROLES.TertiaryTank] = true;
                NotBurnExposedHeadNormal.LastAttack.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                NotBurnExposedHeadNormal.LastAttack.SetAffectsRoles_Healers();
                NotBurnExposedHeadHeroicAbove30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                NotBurnExposedHeadHeroicUnder30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                #endregion
                #region Lava Spew
                // Magmaw will constantly cast Magma Spit at random players, as well as Lava Spew. This must be healed through
                NotBurnExposedHeadNormal.Attacks.Add(new Attack {
                    Name = "Lava Spew",
                    SpellID = new float[] { 77690, 91919, 91931, 91932 }[i],
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = new float[] { (14800f + 17200f), (14800f + 17200f), (20812f + 24187f), (27750f + 32250f) }[i] / 2f,
                    MaxNumTargets = Max_Players[i],
                    AttackSpeed = 26, // Verified DBM 4.74-r5279
                });
                NotBurnExposedHeadNormal.LastAttack.SetAffectsRoles_All();
                NotBurnExposedHeadHeroicAbove30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                NotBurnExposedHeadHeroicUnder30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                #endregion
                #region Pillar of Flame
                // Magmaw will cast Pillar of Flame, marking a location and giving you four seconds to get out of
                // the area. If you are too slow to move, you will be thrown into the air and take massive fall
                // damage, most likely leading to death.
                NotBurnExposedHeadNormal.Attacks.Add(new Attack {
                    Name = "Pillar of Flame",
                    SpellID = 78006,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = (24375f + 25625f) / 2f,
                    MaxNumTargets = Max_Players[i],
                    AttackSpeed = 32.5f,  // Verified DBM 4.74-r5279. The cooldown resets at 30s, bosses uses between 30s and 40s. Most commonly 32.5s
                });
                NotBurnExposedHeadNormal.LastAttack.SetAffectsRoles_Healers();
                NotBurnExposedHeadNormal.LastAttack.AffectsRole[PLAYER_ROLES.TertiaryTank] = true;
                NotBurnExposedHeadNormal.LastAttack.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                NotBurnExposedHeadHeroicAbove30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                NotBurnExposedHeadHeroicUnder30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                NotBurnExposedHeadNormal.Moves.Add(new Impedance {
                    Name = "Pillar of Flame (Avoiding)",
                    Frequency = NotBurnExposedHeadNormal.LastAttack.AttackSpeed,
                    Duration = 3f * 1000f, // takes about 3 seconds to move out of the Pillar's mark
                    // Max Players - 2 players for the MT and OT and assume 1/3rd of the remainder is melee
                    Chance = ((Max_Players[i] - Min_Tanks[i]) * 2f / 3f) / Max_Players[i],
                    Breakable = true,
                    AffectsRole = NotBurnExposedHeadNormal.LastAttack.AffectsRole,
                });
                NotBurnExposedHeadHeroicAbove30.Moves.Add(NotBurnExposedHeadNormal.LastMove.Clone());
                NotBurnExposedHeadHeroicUnder30.Moves.Add(NotBurnExposedHeadNormal.LastMove.Clone());
                #region Parasites (spawned by Pillar of Flame
                // Spawns 9 Parasites
                // killed within 30 seconds on normal; kited and killed on the go between each slump on heroic.
                NotBurnExposedHeadNormal.Targets.Add(new TargetGroup {
                    Name = "Parasites",
                    LevelOfTargets = 85,
                    NumTargs = 9f,
                    Chance = 1.00f,
                    Duration = new float[] { 30f, 30f, 90f, 90f }[i] * 1000f,
                    Frequency = NotBurnExposedHeadNormal.LastAttack.AttackSpeed,
                    NearBoss = false,
                    AffectsRole = NotBurnExposedHeadNormal.LastAttack.AffectsRole,
                });
                NotBurnExposedHeadHeroicAbove30.Targets.Add(NotBurnExposedHeadNormal.LastTarget.Clone());
                NotBurnExposedHeadHeroicUnder30.Targets.Add(NotBurnExposedHeadNormal.LastTarget.Clone());
                // If player touches a Parasite they get a dot that does damage
                NotBurnExposedHeadNormal.Attacks.Add(new Attack {
                    Name = "Parasitic Infection",
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = (12025f + 13975f) / 2f,
                    Duration = 10f,
                    TickInterval = 2f,
                    MaxNumTargets = NotBurnExposedHeadNormal.LastAttack.MaxNumTargets,
                    AttackSpeed = 30f,
                    AffectsRole = NotBurnExposedHeadNormal.LastAttack.AffectsRole,
                });
                NotBurnExposedHeadHeroicAbove30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                NotBurnExposedHeadHeroicUnder30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                // This should not affect Melee and MT/OT only range and healers
                NotBurnExposedHeadNormal.Moves.Add(new Impedance {
                    Name = "Parasitic Infection (Avoidance)",
                    Frequency = NotBurnExposedHeadNormal.LastAttack.AttackSpeed,
                    Chance = 1f,
                    Duration = 3f * 1000f,
                    Breakable = true, // movement is always breakable
                    AffectsRole = NotBurnExposedHeadNormal.LastAttack.AffectsRole,
                });
                NotBurnExposedHeadHeroicAbove30.Moves.Add(NotBurnExposedHeadNormal.LastMove.Clone());
                NotBurnExposedHeadHeroicUnder30.Moves.Add(NotBurnExposedHeadNormal.LastMove.Clone());
                if (i == 2 || i == 3) {
                    // disable the last move and replace it with this other one for TT on Heroic
                    NotBurnExposedHeadHeroicAbove30.LastMove.AffectsRole[PLAYER_ROLES.TertiaryTank] = false;
                    NotBurnExposedHeadHeroicUnder30.LastMove.AffectsRole[PLAYER_ROLES.TertiaryTank] = false;
                    // Kiting Tank is moving all the time on heroic
                    NotBurnExposedHeadHeroicAbove30.Moves.Add(new Impedance {
                        Name = "Kiting Tank Movement",
                        Chance = new float[] { 0f, 0f, 1f, 1f }[i] / Max_Players[i],
                        Duration = new float[] { 0f, 0f, 3, 3 }[i] * 1000f,
                        Frequency = new float[] { 0f, 0f, 5, 5 }[i],
                        Breakable = true, // movement is always breakable
                    });
                    NotBurnExposedHeadHeroicAbove30.LastMove.AffectsRole[PLAYER_ROLES.TertiaryTank] = true;
                    NotBurnExposedHeadHeroicUnder30.Moves.Add(NotBurnExposedHeadHeroicAbove30.LastMove.Clone());
                }
                #endregion
                #endregion
                #region Mangle
                NotBurnExposedHeadNormal.Attacks.Add(new Attack {
                    Name = "Mangle",
                    SpellID = new float[] { 89773, 91912, 94616, 94617 }[i],
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerTick = new float[] { (110464f + 128377f), (132557f + 154052f), (132557f + 154052f), (154649f + 179728f) }[i] / 2f,
                    DamageType = ItemDamageType.Physical,
                    IsDoT = true,
                    Duration = 5f, // should not last more than 5 seconds
                    TickInterval = 5f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 95f, // Verified DBM 4.74-r5279 - 95s cooldown, 30s uptime
                });
                NotBurnExposedHeadNormal.LastAttack.AffectsRole[PLAYER_ROLES.MainTank] = true;
                NotBurnExposedHeadNormal.LastAttack.AffectsRole[PLAYER_ROLES.OffTank] = true;
                NotBurnExposedHeadHeroicAbove30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                NotBurnExposedHeadHeroicUnder30.Attacks.Add(NotBurnExposedHeadNormal.LastAttack.Clone());
                #endregion
                // Mangle Triggers Phase 2, Ignition is handled as part of Phase 2
                #region Blazing Inferno (Heroic Modes Above 30% Magmaw HP Only)
                if (i == 2 || i == 3) {
                    // In Heroic Mode, Blazing Infernos are spawned by Nefarian, but only above 30%
                    #region Inferno Itself
                    NotBurnExposedHeadHeroicAbove30.Attacks.Add(new Attack {
                        Name = "Blazing Inferno",
                        AttackType = ATTACK_TYPES.AT_RANGED,
                        DamageType = ItemDamageType.Fire,
                        DamagePerHit = new float[] { 0f, 0f, (50875f + 59125f), (50875f + 59125f) }[i] / 2f,
                        MaxNumTargets = Max_Players[i],
                        AttackSpeed = new float[] { 0f, 0f, 35f, 35f }[i], // Verified DBM 4.74-r5279
                    });
                    NotBurnExposedHeadHeroicAbove30.LastAttack.SetAffectsRoles_Healers();
                    NotBurnExposedHeadHeroicAbove30.LastAttack.AffectsRole[PLAYER_ROLES.TertiaryTank] = true;
                    NotBurnExposedHeadHeroicAbove30.LastAttack.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                    NotBurnExposedHeadHeroicAbove30.Moves.Add(new Impedance {
                        Name = "Blazing Inferno (Avoiding)",
                        Frequency = NotBurnExposedHeadHeroicAbove30.LastAttack.AttackSpeed,
                        Chance = ((Max_Players[i] - Min_Tanks[i]) * 2f / 3f) / Max_Players[i], // Max Players - 2 players for the MT and OT and assume 1/3rd of the remainder is melee
                        Duration = 2f * 1000f, // takes about 2 seconds to move out of the Pillar
                        Breakable = true, // Movement is always breakable
                        AffectsRole = NotBurnExposedHeadHeroicAbove30.LastAttack.AffectsRole,
                    });
                    #endregion
                    #region Blazing Bone Constructs (spawned from Inferno)
                    NotBurnExposedHeadHeroicAbove30.Targets.Add(new TargetGroup {
                        Name = "Blazing Bone Constructs",
                        LevelOfTargets = 85,
                        NearBoss = true,
                        NumTargs = 1f,
                        Chance = new float[] { 0f, 0f, 1f, 1f }[i],
                        Frequency = new float[] { 0f, 0f, 30f, 30f }[i],
                        Duration = new float[] { 0f, 0f, 30f, 30f }[i] * 1000f,
                    });
                    NotBurnExposedHeadHeroicAbove30.LastTarget.AffectsRole[PLAYER_ROLES.OffTank] = true;
                    NotBurnExposedHeadHeroicAbove30.LastTarget.SetAffectsRoles_DPS();

                    NotBurnExposedHeadHeroicAbove30.Attacks.Add(new Attack {
                        Name = "Blazing Bone Constructs (Melee)", IsFromAnAdd = true,
                        AttackType = ATTACK_TYPES.AT_MELEE,
                        DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]] / 2f,
                        MaxNumTargets = new float[] { 0f, 0f, 1f, 1f }[i],
                        AttackSpeed = 2.0f,
                        Blockable = true,
                        Dodgable = true,
                        Parryable = true,
                        Missable = true,
                    });
                    NotBurnExposedHeadHeroicAbove30.LastAttack.AffectsRole[PLAYER_ROLES.OffTank] = true;

                    // At 20% HP these start casting an 8 second cast that if not killed, will wipe the raid
                    NotBurnExposedHeadHeroicAbove30.Attacks.Add(new Attack {
                        Name = "Armageddon",
                        AttackType = ATTACK_TYPES.AT_AOE,
                        DamageType = ItemDamageType.Fire,
                        DamagePerHit = new float[] { 0f, 0f, (118750f + 131250f), (118750f + 131250f) }[i] / 2f,
                        MaxNumTargets = Max_Players[i],
                        AttackSpeed = 30f,
                    });
                    NotBurnExposedHeadHeroicAbove30.LastAttack.SetAffectsRoles_All(); // everyone has to burn them
                    #endregion
                }
                #endregion
                #region Shadowflame Barrage (Heroic Modes Under 30% Magmaw HP Only)
                if (i == 2 || i == 3) {
                    NotBurnExposedHeadHeroicUnder30.Attacks.Add(new Attack {
                        Name = "Shadowflame Barrage",
                        AttackType = ATTACK_TYPES.AT_AOE,
                        DamageType = ItemDamageType.Shadow,
                        DamagePerHit = new float[] { 0, 0, (22500f + 27500f), (36000f + 44000f) }[i] / 2f,
                        MaxNumTargets = new float[] { 0f, 0f, 2f, 2f }[i],
                        // attacks every second in P3 (at 30%)
                        AttackSpeed = new float[] { 0f, 0f, 1, 1 }[i],
                    });
                    NotBurnExposedHeadHeroicUnder30.LastAttack.SetAffectsRoles_All();
                }
                #endregion
                #endregion
                #region Burning Exposed Head
                // Mangle is handled as part of Phase 1
                #region Mangle Debuff: Tanks loses 50% armor
                // Tanks gain a 50% armor reduction after being affected by a Mangle
                BurnExposedHead.BuffStates.Add(new BuffState {
                    Name = "Sweltering Armor",
                    Breakable = false,
                    Chance = 1f / 2f, // it's a 50/50 chance you will be the tank that gets it
                    Duration = 90f * 1000f,
                    Frequency = 90f,
                    Stats = new Stats() { ArmorReductionMultiplier = 0.50f },
                });
                BurnExposedHead.LastBuffState.AffectsRole[PLAYER_ROLES.MainTank] = true;
                BurnExposedHead.LastBuffState.AffectsRole[PLAYER_ROLES.OffTank] = true;
                #endregion
                #region Exposed Head Debuff: you deal 100% extra damage
                BurnExposedHead.BuffStates.Add(new BuffState {
                    Name = "Exposed Head",
                    Breakable = false,
                    Chance = 1f,
                    Duration = 30f * 1000f,
                    Frequency = 30,
                    Stats = new Stats() { BonusDamageMultiplier = 1f },
                });
                BurnExposedHead.LastBuffState.SetAffectsRoles_All();
                #endregion
                #region Ignition
                // While Mangle is used, half the room is hit with Ignition that affects everyone
                BurnExposedHead.Attacks.Add(new Attack {
                    Name = "Ignition",
                    AttackType = ATTACK_TYPES.AT_AOE, IsDoT = true,
                    DamagePerTick = (23125f + 26875f) / 2f,
                    DamageType = ItemDamageType.Physical,
                    Duration = 10f, // should not last more than 10 seconds
                    TickInterval = 1f,
                    MaxNumTargets = Max_Players[i],
                    AttackSpeed = 90f,
                });
                BurnExposedHead.LastAttack.SetAffectsRoles_All();
                BurnExposedHead.Moves.Add(new Impedance {
                    Name = "Ignition (Avoiding)",
                    Chance = 1f,
                    Duration = 3f * 1000f, // Assume 3 seconds to get out
                    Breakable = true, // Movement is always breakable
                    Frequency = BurnExposedHead.LastAttack.AttackSpeed,
                    AffectsRole = BurnExposedHead.LastAttack.AffectsRole,
                });
                #endregion
                #endregion

                #region Apply Phases
                // Burn Phases should last about 30 seconds
                int phaseStartTime = 0;
                ClearPhase1Values( BurnExposedHead);
                if (i == 0 || i == 1) {
                    // 10 minute Berserk Timer 95+30=125 600/125=4.8. At most 10 phases (5 each)
                    ApplyAPhasesValues( NotBurnExposedHeadNormal, i, 1, phaseStartTime, 90, this[i].BerserkTimer); phaseStartTime += 90;
                    ApplyAPhasesValues( BurnExposedHead,          i, 2, phaseStartTime, 30, this[i].BerserkTimer); phaseStartTime += 30;
                    ApplyAPhasesValues( NotBurnExposedHeadNormal, i, 3, phaseStartTime, 90, this[i].BerserkTimer); phaseStartTime += 90;
                    ApplyAPhasesValues( BurnExposedHead,          i, 4, phaseStartTime, 30, this[i].BerserkTimer); phaseStartTime += 30;
                    ApplyAPhasesValues( NotBurnExposedHeadNormal, i, 5, phaseStartTime, 90, this[i].BerserkTimer); phaseStartTime += 90;
                    ApplyAPhasesValues( BurnExposedHead,          i, 6, phaseStartTime, 30, this[i].BerserkTimer); phaseStartTime += 30;
                    ApplyAPhasesValues( NotBurnExposedHeadNormal, i, 7, phaseStartTime, 90, this[i].BerserkTimer); phaseStartTime += 90;
                    ApplyAPhasesValues( BurnExposedHead,          i, 8, phaseStartTime, 30, this[i].BerserkTimer); phaseStartTime += 30;
                    ApplyAPhasesValues( NotBurnExposedHeadNormal, i, 9, phaseStartTime, 90, this[i].BerserkTimer); phaseStartTime += 90;
                    ApplyAPhasesValues( BurnExposedHead,          i,10, phaseStartTime, 30, this[i].BerserkTimer);
                    AddAPhase(NotBurnExposedHeadNormal, i);
                } else if (i == 2 || i == 3) {
                    // Heroic has a Phase 3 which replace the normal phase 1 at 30% HP, start it at 600-180=420
                    // Kavan: exposed head starts about 100-115 seconds into the fight, then it comes about every 90-100 seconds
                    // 10 minute Berserk Timer 500/95=5.3. At most 12 phases (6 each), swap last two Ph1 with a Ph3
                    ClearPhase1Values( NotBurnExposedHeadHeroicUnder30);
                    ApplyAPhasesValues( NotBurnExposedHeadHeroicAbove30, i, 1, phaseStartTime, 100, this[i].BerserkTimer); phaseStartTime += 100; // 100
                    ApplyAPhasesValues( BurnExposedHead,                 i, 2, phaseStartTime, 30, this[i].BerserkTimer); phaseStartTime += 30; //130
                    ApplyAPhasesValues( NotBurnExposedHeadHeroicAbove30, i, 3, phaseStartTime, 65, this[i].BerserkTimer); phaseStartTime += 65; //195
                    ApplyAPhasesValues( BurnExposedHead,                 i, 4, phaseStartTime, 30, this[i].BerserkTimer); phaseStartTime += 30; //225
                    ApplyAPhasesValues( NotBurnExposedHeadHeroicAbove30, i, 5, phaseStartTime, 65, this[i].BerserkTimer); phaseStartTime += 65; //290
                    ApplyAPhasesValues( BurnExposedHead,                 i, 6, phaseStartTime, 30, this[i].BerserkTimer); phaseStartTime += 30; //320
                    ApplyAPhasesValues( NotBurnExposedHeadHeroicAbove30, i, 7, phaseStartTime, 65, this[i].BerserkTimer); phaseStartTime += 65; //385
                    ApplyAPhasesValues( BurnExposedHead,                 i, 8, phaseStartTime, 30, this[i].BerserkTimer); phaseStartTime += 30; //415
                    ApplyAPhasesValues( NotBurnExposedHeadHeroicUnder30, i, 9, phaseStartTime, 65, this[i].BerserkTimer); phaseStartTime += 65; //480
                    ApplyAPhasesValues( BurnExposedHead,                 i, 10, phaseStartTime, 30, this[i].BerserkTimer); phaseStartTime += 30; //510
                    ApplyAPhasesValues( NotBurnExposedHeadHeroicUnder30, i, 11, phaseStartTime, 65, this[i].BerserkTimer); phaseStartTime += 65; //575
                    ApplyAPhasesValues( BurnExposedHead,                 i, 12, phaseStartTime, 25, this[i].BerserkTimer); phaseStartTime += 25; //600
                    AddAPhase(NotBurnExposedHeadHeroicAbove30, i);
                    AddAPhase(NotBurnExposedHeadHeroicUnder30, i);
                }
                AddAPhase(BurnExposedHead, i);
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
        }
    }

    public class OmnotronDefenseSystem : MultiDiffBoss
    {
        public OmnotronDefenseSystem()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Omnotron Defense System";
            Instance = "Blackwing Descent";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Health values were lowered on Normal by 20% in 4.2
            Health = new float[] { 32209000f * 0.80f, 79364208f, 45080000f, 126261240f, 0 };
            MobType = (int)MOB_TYPES.MECHANICAL;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            #endregion
            #region Phase Info
            for (int i = 0; i < 4; i++)
            {
                Phase Magtron = new Phase() { Name = "Magmatron (Alone)" };
                Phase Arctron = new Phase() { Name = "Arcanotron (Alone)" };
                Phase Toxtron = new Phase() { Name = "Toxitron (Alone)" };
                Phase Elctron = new Phase() { Name = "Electron (Alone)" };
#if FALSE
                Phase MagtronArctron = new Phase() { Name = "Magmatron and Arcanotron" };
                Phase MagtronToxtron = new Phase() { Name = "Magmatron and Toxitron" };
                Phase MagtronElctron = new Phase() { Name = "Magmatron and Electron" };
                Phase ArctronToxtron = new Phase() { Name = "Arcanotron and Toxitron" };
                Phase ArctronElctron = new Phase() { Name = "Arcanotron and Electron" };
                Phase ToxtronElctron = new Phase() { Name = "Toxitron and Electron" };

                Phase MagtronArctronToxtron = new Phase() { Name = "Magmatron, Arcanotron and Toxitron" };
                Phase MagtronArctronElctron = new Phase() { Name = "Magmatron, Arcanotron and Electron" };
                Phase MagtronToxtronElctron = new Phase() { Name = "Magmatron, Toxitron and Electron" };
                Phase ArctronToxtronElctron = new Phase() { Name = "Arcanotron, Toxitron and Electron" };
#endif

                #region "Magmatron, the fire golem"
                #region Melee
                Magtron.Attacks.Add(GenAStandardMelee(this[i].Content));
                #endregion
                #region Barrier - Broken
                //  Barrier (10) / (25) - Magmatron "shield ability" in which he forms a barrier around himself, absorbing 300k(10m)/900k(25m) damage, and if broken, causes a
                //      Backdraft (10) / (25), which deals 75k(10m)/115k(25m) to all raid members.
                //      Basically, when he casts this, stop attacking Magmatron
                Magtron.Attacks.Add(new Attack
                {
                    Name = "Barrier - Broken",
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = (73125 + 76875) / 2f,
                    MaxNumTargets = Max_Players[i],
                    AttackSpeed = 10+1.5f, // In reality you don't want this ability to proc, so you "interrupt" it. NOTE: Can't set this to 0 or it doesn't show up right in the UI
                    Interruptable = true, // Interrupted by Stopping DPS
                });
                Magtron.LastAttack.SetAffectsRoles_All();
                #endregion
                #region Incineration Security Measure
                // Incineration Security Measure (10) / (25) - This ability is a raid-wide spell that is channeled and does 10k(10m)/15k(25m) fire damage every second
                //      for 4 seconds.
                // Has a 1.5 second cast time
                Magtron.Attacks.Add(new Attack {
                    Name = "Incineration Security Measure",
                    AttackType = ATTACK_TYPES.AT_AOE,
                    IsDoT = true,
                    DamageType = ItemDamageType.Fire,
                    DamagePerTick = (14625 + 15375) / 2f,
                    Duration = 4f,
                    TickInterval = 1f,
                    MaxNumTargets = Max_Players[i],
                    AttackSpeed = 26.5f, // Verified DBM 4.74 --Timer Series, 10, 27, 32 (on normal) from activate til shutdown.
                });
                Magtron.LastAttack.SetAffectsRoles_All();
                #endregion
                #region Acquiring Target - Flamethrower
                // Acquiring Target -> Flamethrower (10) / (25) - Magmatron will randomly target a raid member and channel a beam on them for 4 seconds to acquire his target.
                //      After that time, Magmatron will use Flamethrower to that target and everyone directly behind him in a small cone, dealing 35k(10m)/50k(25m) damage
                //      every second for 4 seconds.
                Magtron.Attacks.Add(new Attack
                {
                    Name = "Acquiring Target - Flamethrower",
                    IsDoT = true,
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamagePerTick = (34125 + 35875) / 2f,
                    Duration = 4f,
                    TickInterval = 1f,
                    MaxNumTargets = 1f, // You really only want one person be hit by this. Assume it does not target a tank
                    AttackSpeed = 40f, // Verified DBM 4.74
                });
                Magtron.LastAttack.SetAffectsRoles_DPS();
                Magtron.LastAttack.SetAffectsRoles_Healers();
                Magtron.Moves.Add(new Impedance() {
                    Name = "Acquiring Target - Flamethrower (Avoidance)",
                    Frequency = Magtron.LastAttack.AttackSpeed,
                    Duration = 4f * 1000f, // You have 4 seconds to get out of the way
                    Chance = 1f / Max_Players[i], // One random member of the raid is the one that gets targetted
                    Breakable = true, // Movement is always breakable
                    AffectsRole = Magtron.LastAttack.AffectsRole,
                });
                #endregion
                #endregion

                #region "Arcanotron, the arcane golem"
                #region Melee
                Arctron.Attacks.Add(GenAStandardMelee(this[i].Content));
                #endregion
                #region Arcane Annihilator
                /* Arcane Annihilator (10) / (25) - 1 second cast time spell used on a randomly targeted raid member, but is interruptable. It deals 40k(10m)/50k(25m) arcane
                 *      damage if it does go through. */
                // DBM CD is 8 after overcharge is cast
                #endregion
                #region Power Generator
                /* Power Generator (10) / (25) - Creates a whirling pool of energy underneath a golem that increases the damage of any raid member or boss mob standing in it
                 *      by 50% and restores 250(10m)/500(25m) mana per every 0.5 second (Similar to Rune of Power from Iron Council). Lasts for 25 seconds (however it appears
                 *      as if the buff lasts for 10-15 seconds after the Power Generator despawns). */
                // DBM CD is 30s
                #endregion
                #region Power Conversion
                /* Power Conversion - Arcanotron's "shield ability" in which he gains a stacking buff, Converted Power, when taking damage that increases his magic damage and
                 *      cast speed by 10% per stack. Currently can be removed with a mage's Spellsteal. */
                // DBM CD is 10+1.5
                #endregion
                #endregion

                #region "Toxitron, the poison golem"
                #region Melee
                Toxtron.Attacks.Add(GenAStandardMelee(this[i].Content));
                #endregion
                /* Chemical Cloud (10) / (25) - Large raidus debuff that increases damage taken by 50% and deals 3k(10m)/4k(25m) damamge every 5 seconds. */
                // DBM CD is 30: Timer Series, 11, 30, 36 (on normal) from activate til shutdown.
                /* Poison Protocol (10) / (25) - Channeled for 9 seconds, and spawns 3(10m)/6(25m) Poison Bombs, one every 3(10m)/1.5(25m) seconds. The Poison Bombs (10) / (25)
                 *      have 78k(10m)/78k(25m) health and fixate on a target, then explode if they reach their target, dealing 90k(10m)/125k(25m) nature damage to players within
                 *      the area and spawn a Slime Pool which deals additional damage to players that stand in it. */
                // DBM CD is 45
                /* Poison Soaked Shell - Toxitron's "shield ability" which causes player that attacks him to gain a stacking debuff, Soaked in Poison (10) / (25), which causes
                 *      that player to take 2k(10m)/5.5k(25m) nature damage every 2 seconds, however, they also will deal 10k additional nature damage to targets of their attacks.*/
                // DBM CD is 30 with 10+1.5 uptime
                #endregion

                #region "Electron, the electricity golem"
                #region Melee
                Elctron.Attacks.Add(GenAStandardMelee(this[i].Content));
                #endregion
                /* TODO: Lightning Conductor (10) / (25) - A randomly targeted raid member will get this debuff which has a 10(10m)/15(25m) second duration and deals 25k damage to raid
                 *      members with 8 yards every 2 seconds. */
                // DBM CD is 25
                /* TODO: Electrical Discharge (10) / (25) - A chain lightning type ability which deals 30k(10m)40k(25m) nature damage to a target and then to up to 2 additional targets
                 *      within 8 yards, damage increasing 20% each jump. */
                // TODO: DBM CD is unknown
                /* TODO: Unstable Shield - Electron's "shield ability" which causes at Static Shock at an attackers location, dealing 40k nature damage to raiders within 7 yards.*/
                // DBM CD is 10+1.5
                #endregion

                #region Apply Phases
                // For now just doing Magmatron
                int phaseStartTime = 0;
                //ClearPhase1Values( FelFirestorm);
                ApplyAPhasesValues( Magtron,  i, 1, phaseStartTime, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(Magtron, i);
                //AddAPhase(FelFirestorm, i);
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Health values were lowered on Normla by 80% in 4.2
            Health = new float[] { 24700000f * 0.80f, 68713600f, 34631000f, 121310000f, 0 };
            MobType = (int)MOB_TYPES.DRAGONKIN;
            BerserkTimer = new int[] { 7 * 60, 7 * 60, 12 * 60, 12 * 60, 0 }; // Source: http://us.battle.net/wow/en/blog/1232869
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 3, 3, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            #endregion
            #region Offensive
            #region Multiple Targets
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #endregion
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase RedVial = new Phase() { Name = "Red Vial (Group Up!)" };
                Phase BlueVial = new Phase() { Name = "Blue Vial (Spread Out!)" };
                Phase GreenVial = new Phase() { Name = "Green Vial (Adds!)" };
                Phase DarkVial = new Phase() { Name = "Dark Vial (Heroic!)" };
                Phase Under25 = new Phase() { Name = "Under 25% HP" };

                // TODO: Aberration and Prime Subject Damage

                #region Phase 1
                #region Common to all Vial Phases
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
                this[i].Attacks[this[i].Attacks.Count - 1].SetPhaseValue(1, 0, BerserkTimer[i], BerserkTimer[i]);
                #region Arcane Storm
                /* Arcane Storm (10) / (25) - This 6 second channeled spell is used often throughout the encounter and can be interrupted. It deals 8k(10m)/15k(25m) arcane
                 *      damage to each raid member at 1 second intervals. */
                /* 10 man - http://www.wowhead.com/spell=77908
                    25 man - http://www.wowhead.com/spell=92961
                    10 man heroic - http://www.wowhead.com/spell=92962
                    25 man heroic - http://www.wowhead.com/spell=92963 */
                RedVial.Attacks.Add(new Attack
                {
                    Name = "Arcane Storm",
                    SpellID = new float[] { 77908, 92961, 92962, 92963 }[i],
                    AttackType = ATTACK_TYPES.AT_AOE, IsDoT = true,
                    DamageType = ItemDamageType.Arcane,
                    DamagePerTick = new float[] { (14137 + 15862), (14137 + 15862), (47125 + 52875), (47125 + 52875) }[i] / 2f,
                    Duration = 2, // Assuming players interupt him at 2s
                    TickInterval = 1,
                    // Deadly Boss Mods is quoting 14 seconds as of April 23rd, 2011; release version 5621.
                    // BigWigs does not show a timer for this
                    AttackSpeed = 14,
                    MaxNumTargets = Max_Players[i], 
                    Interruptable = true,
                });
                RedVial.LastAttack.SetUnavoidable();
                RedVial.LastAttack.SetAffectsRoles_All();
                BlueVial.Attacks.Add(RedVial.LastAttack.Clone());
                GreenVial.Attacks.Add(RedVial.LastAttack.Clone());
                #endregion
                #region Release Aberrations
                /* Release Aberrations - This is a 1.5 second summon used by the boss about every 30 seconds and can be interrupted. There will be 4 casts before each Green
                 *      Vial phase, with each wave summoning 3 Aberration adds with 365k(10m)/1.3m(25m) health and melee for about 4k(10)/8k(25m) after mitigation and only
                 *      1 stack of their buff, Growth Catalyst. */
                // http://www.wowhead.com/spell=77569
                RedVial.Targets.Add(new TargetGroup {
                    Name = "Release Aberrations",
                    LevelOfTargets = 85,
                    NumTargs = 3,
                    NearBoss = false,
                    // Deadly Boss Mods is quoting 15 seconds as of April 23rd, 2011; release version 5621.
                    // BigWigs does not show a timer for this
                    Frequency = 15,
                    // Each Red and Blue Phase lasts for 47 seconds as posted by BigWigs as of April 23rd, 2011; release version 8327.
                    // Deadly Boss Mods is quoting 49 seconds as of April 23rd, 2011; release version 5621. Will average them out.
                    Duration = (48f * 1000f * 2f) + (15f * 1000f), // TODO: Should be tied to two phases, you build them up to nine, then AoE during a Green Vial Phase + 15 seconds to AoE them down
                    Chance = 1.00f,
                });
                RedVial.LastTarget.AffectsRole[PLAYER_ROLES.OffTank] = true;
                RedVial.LastTarget.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                BlueVial.Targets.Add(RedVial.LastTarget.Clone());
                GreenVial.Targets.Add(RedVial.LastTarget.Clone());
                #endregion
                /* TODO: Remedy (10) / (25) - Maloriak occasionally uses this to buff himself, healing for 25k(10m)/75k(25m) and restoring 2k mana per second for 10 seconds. However,
                 *      it can be removed with offensive magic dispels or can be taken via a mage's Spellsteal. */
                /* 10 man - http://www.wowhead.com/spell=77912
                    25 man - http://www.wowhead.com/spell=92965
                    10 man heroic - http://www.wowhead.com/spell=92966
                    25 man heroic - http://www.wowhead.com/spell=92967 */
                #endregion
                #region Red Vial (Group Up!)
                // Fire Imbued - This 40 second buff on Maloriak is triggered upon tossing a Red Vial into his cauldron. It allows the usage of his fire-based special abilities.
                // NOTE: This has no bearing in modelling, it simply makes the other attacks active
                // http://www.wowhead.com/spell=78896
                #region Consuming Frames
                /* Consuming Flames (10) / (25) - This 10 second debuff during the Red Vial phase is placed on a random raid member and causes them to take 3k(10m)/6k(25m)
                 *      fire damage per second, and additionally, causes the target to take additional damage equal to 50% of the damage taken from other magic sources.
                 *      (Example: In 10-man, having this debuff and getting breathed on from a Scorching Blast that hits all 10 raid members, reducing the individual player's
                 *      damage from Scorching Blast to 20k, will result in an additional 5k (which is 25% of 20,000) damage taken per second from Consuming Flames' secondary effect.) */
                /* 10 man - http://www.wowhead.com/spell=77786
                    25 man - http://www.wowhead.com/spell=92972
                    10 man heroic - http://www.wowhead.com/spell=92971
                    25 man heroic - http://www.wowhead.com/spell=92973 */
                RedVial.Attacks.Add(new Attack {
                    Name = "Consuming Flames",
                    AttackType = ATTACK_TYPES.AT_RANGED, IsDoT = true,
                    DamageType = ItemDamageType.Fire,
                    DamagePerTick = new float[] { 4500, 7500, 9000, 13500 }[i],
                    Duration = 10,
                    TickInterval = 1,
                    MaxNumTargets = 1,
                    // Cast 4 times during Red Vial phase according to logs
                    // 2nd is 12 seconds after 1st cast, 3rd is 7 seconds after, 4th is 12 seconds after
                    // Neither BigWigs or DBM show a timer for this.
                    AttackSpeed = 10.3f,
                });
                RedVial.LastAttack.SetAffectsRoles_All();
                RedVial.BuffStates.Add(new BuffState {
                    Name = RedVial.LastAttack.Name,
                    Frequency = RedVial.LastAttack.AttackSpeed,
                    Duration = RedVial.LastAttack.Duration * 1000f,
                    AffectsRole = RedVial.LastAttack.AffectsRole,
                    Chance = RedVial.LastAttack.MaxNumTargets / Max_Players[i],
                    Stats = { FireDamageTakenMultiplier = 0.50f },
                    Breakable = true, // Cloak of Shadows and similar abilities can break this debuff
                });
                #endregion
                #region Scorching Blast
                /* Scorching Blast (10) / (25) - This ability is only used when Maloriak is Fire Imbued, and deals 200k(10m)/500k(25m)
                   fire damage split between all players in a cone 60 yards in front of the boss. */
                /* 10 man - http://www.wowhead.com/spell=77679
                   25 man - http://www.wowhead.com/spell=92968
                   10 man heroic - http://www.wowhead.com/spell=92969
                   25 man heroic - http://www.wowhead.com/spell=92970 */
                RedVial.Attacks.Add(new Attack {
                    Name = "Scorching Blast",
                    AttackType = ATTACK_TYPES.AT_AOE, IsDoT = true,
                    DamageType = ItemDamageType.Fire,
                    DamagePerTick = new float[] { 500000f, 1250000f, (1000000f + 1250000f) / 2f, (2500000f + 3125000f) / 2f }[i] / Max_Players[i],
                    Duration = 4f,
                    TickInterval = 1f,
                    MaxNumTargets = Max_Players[i],
                    // Both DBM and BigWigs is saying 10 second cooldown as of April 23rd, 2011; release version 5621 and 8327 respectively.
                    // Logs are showing 19 seconds between each cast
                    AttackSpeed = 19f,
                });
                RedVial.LastAttack.SetAffectsRoles_All();
                #endregion
                #endregion
                #region Blue Vial (Spread Out!)
                // Frost Imbued - This 40 second buff on Maloriak is triggered upon tossing a Blue Vial into his cauldron. It allows the usage of his frost-based special abilities
                // NOTE: This has no bearing in modelling, it simply makes the other attacks active
                #region Biting Chill
                /* Biting Chill (10) / (25) - This debuff is placed on 1-2(10m)/3-5(25m) raid members that are within 10 yards of Maloriak. The debuff lasts 10 seconds and deals
                 *      5k(10m)/7.5k(25m) frost damage per second to anyone within (6???) yards. */
                /* 10 man - http://www.wowhead.com/spell=77763
                   25 man - http://www.wowhead.com/spell=92975
                   10 man heroic - http://www.wowhead.com/spell=92976
                   25 man heroic - http://www.wowhead.com/spell=92977 */
                BlueVial.Attacks.Add(new Attack {
                    Name = "Biting Chill",
                    AttackType = ATTACK_TYPES.AT_AOE,
                    IsDoT = true,
                    DamageType = ItemDamageType.Frost,
                    // spell description shows 5000 damage for each version, however logs are showing 10,000 damage for heroics (10 and 25)
                    DamagePerTick = new float[] { 5000, 5000, 10000, 10000 }[i],
                    Duration = 10f,
                    TickInterval = 1f,
                    MaxNumTargets = new float[] { 1+2, 3+5, 1+2, 3+5 }[i] / 2f,
                    // DBM is reporting a 10 second cooldown as of April 23rd, 2011; release version 5621
                    AttackSpeed = 10f,
                });
                BlueVial.LastAttack.SetUnavoidable();
                BlueVial.LastAttack.SetAffectsRoles_Tanks();
                BlueVial.LastAttack.AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                #endregion
                #region Flash Freeze
                /* Flash Freeze (10) / (25) - This ability is used on a raid member at ranged while Maloriak is Frost Imbued, encasing them in a block of ice, also called a
                 *      Flash Freeze, with 5k(10m)/17.5k(25m) health. Upon becoming frozen, that player will take 50k(10m)/75k(25m) frost damage and be unable to move or use
                 *      abilities. Additionally, other players within (10???) yards of the initial target will take the damage and be placed in Flash Freeze ice blocks. When any
                 *      Flash Freeze ice block is destroyed that player will Shatter (10) / (25) dealing an additional 50k(10m)/75k(25m) frost damage to all players within (10???)
                 *      yards and removing the Flash Freeze debuff on any other players within (10???) yards, freeing them as well. */
                /* 10 man - http://www.wowhead.com/spell=77699
                   25 man - http://www.wowhead.com/spell=92978
                   10 man heroic - http://www.wowhead.com/spell=92979
                   25 man heroic - http://www.wowhead.com/spell=92980 */
                BlueVial.Attacks.Add(new Attack {
                    Name = "Flash Freeze",
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Frost,
                    // Everyone should be spread out so that any extra damage is non-existant. The damage here is applied after the iceblock is "killed"
                    // Wowhead does have the damage range just it is not reflected in the tooltip
                    DamagePerHit = new float[] { (70687 + 79312), (70687 + 79312), (84825 + 95175), (117812 + 132187) }[i] / 2f,
                    MaxNumTargets = 1f,
                    // DBM is reporting a 14 second cooldown as of April 23rd, 2011; release version 5621
                    // BigWigs is reporting a 15 second cooldown as of April 23rd, 2011; release version 8327; using a medium of the two
                    AttackSpeed = 14.5f,
                });
                BlueVial.LastAttack.SetUnavoidable();
                BlueVial.LastAttack.SetAffectsRoles_All();
                BlueVial.Targets.Add(new TargetGroup {
                    Name = "Flash Freeze",
                    AffectsRole = BlueVial.LastAttack.AffectsRole,
                    Duration = 4f * 1000f,
                    Frequency = BlueVial.LastAttack.AttackSpeed,
                    NearBoss = false,
                    NumTargs = 1, // Only one player gets frozen at a given time.
                    Chance = 1f / Max_Players[i], // 2 DPS need to work them down after healers top off the encased player's HP
                    LevelOfTargets = 85,
                });
                #endregion
                #endregion
                #region Green Vial (Adds!)
                #region Debilitating Slime
                /* Debilitating Slime (10) / (25) - When the Green Vial is tossed into the cauldron, it will knock Maloriak back 15 yards and cover the room in a green spray,
                 *  debuffing all friendly and enemy targets in the room for 15 seconds which increases damage taken by 100%(10m)/150%(25m), additionally, it will temporarily remove
                 *  the Growth Catalyst buff on any Aberrations or from Maloriak himself. This will make incoming tank damage very high, but will also drastically increase raid DPS. */
                // http://www.wowhead.com/spell=77615
                GreenVial.BuffStates.Add(new BuffState {
                    Name = "Debilitating Slime (Buff, Debuff)",
                    Frequency = 45,
                    Duration = 15f * 1000f,
                    Chance = 1.00f,
                    Stats = {
                        DamageTakenReductionMultiplier = new float[] { -1.00f, -1.00f, -1.50f, -1.50f }[i],
                        BonusDamageMultiplier          = new float[] { -1.00f, -1.00f, -1.50f, -1.50f }[i]
                    },
                });
                GreenVial.LastBuffState.SetAffectsRoles_All();
                #endregion
                #endregion
                #region Dark Vial (Heroic Only)
                // TODO: Dark Sludge
                #region Vile Swill
                // http://www.wowhead.com/npc=49811
                DarkVial.Targets.Add(new TargetGroup {
                    Name = "Vile Swill",
                    Frequency = BerserkTimer[i],
                    Chance = 1.00f,
                    // Both DBM and BigWigs is reporting a 100 second Dark Phase as of April 23rd, 2011; release version 5621 and 8327 respectively
                    Duration = 100 * 1000f, // It usually takes the full length of the phase to kill them
                    NearBoss =  false, // They are kiting around the outside of the room
                    LevelOfTargets = 88,
                    NumTargs = new float[] { 0f, 0f, 3f, 5f }[i] // TODO: Confirm 10 man number of adds, 25 man has 5 spawns
                });
                DarkVial.LastTarget.SetAffectsRoles_All();
                #endregion
                #region Engulfing Darkness
                // http://www.wowhead.com/spell=92983
                DarkVial.Attacks.Add(new Attack {
                    Name = "Engulfing Darkness",
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamagePerHit = 25000,
                    // Deadly Boss Mods is quoting 12 seconds as of April 23rd, 2011; release version 5621.
                    // BigWigs does not show a timer for this
                    AttackSpeed = 12,
                    MaxNumTargets = 1, // Only one person should ever be hit by it, the main tank
                });
                DarkVial.LastAttack.SetUnavoidable();
                DarkVial.LastAttack.AffectsRole[PLAYER_ROLES.MainTank] = true;
                DarkVial.BuffStates.Add(new BuffState {
                    Name = "Engulfing Darkness (Debuff)",
                    Breakable = false,
                    Chance = 1.00f, // Frontal cone, guaranteed to hit the tank
                    Duration = 8f * 1000f,
                    Frequency = DarkVial.LastAttack.AttackSpeed,
                    Stats = { HealingReceivedMultiplier = -1.00f, },
                    AffectsRole = DarkVial.LastAttack.AffectsRole,
                });
                #endregion
                #endregion
                #endregion
                #region Phase 2 (at 25% HP)
                #region Release All
                /* "Release All"
                 * Release All - When Maloriak reaches 25% health, he will cast this ability, summoning two Prime Subject and will summon 3 Aberrations for every Release Aberration
                 *      cast that was interrupted throughout the fight. The Prime Subjects have 8.6million(10m)/30.1million(25m) health and melee much harder than the smaller adds,
                 *      with each hit for about 20k-25k(10m)/30k-40k(25m) damage after mitigation. They will occasionally fixate a random target and attempt to kill it. */
                // http://www.wowhead.com/spell=77991
                Under25.Targets.Add(new TargetGroup {
                    Name = "Release All (Aberrations)",
                    LevelOfTargets = 85,
                    NumTargs = 3 * 3, // Assuming 3 were interupted
                    NearBoss = false,
                    Frequency = BerserkTimer[i]-1,
                    Duration = 15f * 1000f,
                    Chance = 1.00f,
                });
                Under25.LastTarget.AffectsRole[PLAYER_ROLES.OffTank] = true;
                Under25.LastTarget.AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                Under25.LastTarget.AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                // http://www.wowhead.com/npc=41841
                Under25.Targets.Add(new TargetGroup {
                    Name = "Release All (Prime Subjects)",
                    LevelOfTargets = 88,
                    NumTargs = 2,
                    NearBoss = Under25.LastTarget.NearBoss,
                    Frequency = Under25.LastTarget.Frequency,
                    // these are tanked for as much as 132 seconds on normal and 232 on heroic
                    Duration = new float[] { 132, 132, 232, 232}[i] * 1000f,
                    Chance = Under25.LastTarget.Chance,
                    AffectsRole = Under25.LastTarget.AffectsRole,
                });
                #endregion
                #region Magma Jets
                /* Magma Jets (10) / (25) - This ability is only used by Maloriak after he has used Release All. After a 2 second cast, the boss will charge toward a random raid
                 *      member and deal 25k(10m)/50k(25m) fire damage to anyone in the path and knock them back 30 yards. It will also leave a path of fire that deals 5k(10m)/10k(25m)
                 *      fire damage per second to anyone that gets within 3 yards. The trails dissipate after 20 seconds. */
                /* 10 man - http://www.wowhead.com/spell=78095
                   25 man - http://www.wowhead.com/spell=93014
                   10 man heroic - http://www.wowhead.com/spell=93015
                   25 man heroic - http://www.wowhead.com/spell=93016 */
                // TODO: Tank can move away from the attack
                // TODO: Add 30 yard knockback to main tank
                Under25.Attacks.Add(new Attack {
                    Name = "Magma Jets",
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = new float[] { (38000 + 42000), (37700 + 42300), (70687 + 79312), (70687 + 79312) }[i] / 2f,
                    AttackSpeed = 12.5f,
                    MaxNumTargets = Max_Players[i],
                });
                // It targets the MT but anyone is able to walk into the fire after it is up.
                Under25.LastAttack.SetUnavoidable();
                Under25.LastAttack.SetAffectsRoles_All();
                #endregion
                #region Absolute Zero
                /* Absolute Zero (10) / (25) - This ability is only used by Maloriak after he has used Release All. This ability will form a circle near a random raid member that will
                 *      spawn a floating ice sphere after 3 seconds. When the ball becomes active, it will float randomly around the room. If it comes into contact with a player, it
                 *      will explode dealing 20k(10m)/40k(25m) frost damage and cause a 10 yard knock back to all nearby players. */
                /* 10 man - http://www.wowhead.com/spell=78208
                   25 man - http://www.wowhead.com/spell=93041
                   10 man heroic - http://www.wowhead.com/spell=93042
                   25 man heroic - http://www.wowhead.com/spell=93043
                   NPC - http://www.wowhead.com/npc=41961 */ 
                Under25.Moves.Add(new Impedance {
                    Name = "Absolute Zero Spheres (Avoiding)",
                    // According to logs, it is cast every 7 seconds
                    // DBM and BigWigs do not show any cooldown
                    Frequency = 7,
                    Duration = 3f * 1000f, // Assuming takes 3s to move out of the way
                    Chance = 1f / Max_Players[i],
                    Breakable = true, // movement is always breakable
                });
                Under25.LastMove.SetAffectsRoles_All();
                #endregion
                #region Acid Nova
                /* Acid Nova (10) / (25) - This ability is only used by Maloriak after he has used Release All. This ability is as a blast that will place a 10 second debuff on everyone
                 *      in the raid which deals 5k(10m)/7.5k(25m) nature damage per second. */
                /* 10 man - http://www.wowhead.com/spell=78225
                   25 man - http://www.wowhead.com/spell=93011
                   10 man heroic - http://www.wowhead.com/spell=93012
                   25 man heroic - http://www.wowhead.com/spell=93013 */
                Under25.Attacks.Add(new Attack {
                    Name = "Acid Nova",
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Nature, IsDoT = true,
                    DamagePerTick = new float[] { 7500, 7500, 15000, 15000 }[i],
                    Duration = 10,
                    TickInterval = 1,
                    // Logs are showing a 30 second cooldown
                    // Both DBM and BigWigs do not show a timer for this
                    AttackSpeed = 30,
                    MaxNumTargets = Max_Players[i],
                });
                Under25.LastAttack.SetUnavoidable();
                Under25.LastAttack.SetAffectsRoles_All();
                #endregion
                #endregion
                #region Apply Phases
                // 5 minute Berserk Timer with Firestorm only last 18 seconds twice.
                // Pushing distro of normal fighting to more in the first two to account for executes
                int phaseStartTime = 0;
                ClearPhase1Values( BlueVial);
                ClearPhase1Values( GreenVial);
                ClearPhase1Values( Under25);
                // DBM is reporting a 49 second phase length for Red, Blue, and Green phases as of April 23rd, 2011; release version 5621
                // BigWigs is reporting a 47 second phase length for Red, Blue, and Green phases as of April 23rd, 2011; release version 8327. Using a medium of the two
                // DBM and BigWigs is reporting a 100 second Black phase as of April 23rd, 2011; release version 5621 and 8327 respectively
                // Under 25% needs to hit after the second Green Vial phase, especially on heroic or raids will hit the enrage timer
                if (i == 0 || i == 1) {
                    ApplyAPhasesValues( RedVial,   i, 1, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( BlueVial,  i, 2, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( GreenVial, i, 3, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( RedVial,   i, 4, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( BlueVial,  i, 5, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( GreenVial, i, 6, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( Under25,   i, 7, phaseStartTime, 132, this[i].BerserkTimer);
                }
                else if (i == 2 || i == 3)
                {
                    ApplyAPhasesValues( DarkVial,  i, 1, phaseStartTime, 100, this[i].BerserkTimer); phaseStartTime += 100;
                    ApplyAPhasesValues( RedVial,   i, 2, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( BlueVial,  i, 3, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( GreenVial, i, 4, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( DarkVial,  i, 5, phaseStartTime, 100, this[i].BerserkTimer); phaseStartTime += 100;
                    ApplyAPhasesValues( RedVial,   i, 6, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( BlueVial,  i, 7, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
                    ApplyAPhasesValues( GreenVial, i, 8, phaseStartTime, 48, this[i].BerserkTimer); phaseStartTime += 48;
//                    ApplyAPhasesValues( DarkVial,  i, 9, phaseStartTime, 60, this[i].BerserkTimer); phaseStartTime += 60;
//                    ApplyAPhasesValues( RedVial,   i,10, phaseStartTime, 60, this[i].BerserkTimer); phaseStartTime += 60;
//                    ApplyAPhasesValues( BlueVial,  i,11, phaseStartTime, 60, this[i].BerserkTimer); phaseStartTime += 60;
                    ApplyAPhasesValues( Under25,   i,9, phaseStartTime, 232, this[i].BerserkTimer);

                    AddAPhase(DarkVial, i);
                }
                AddAPhase(RedVial, i);
                AddAPhase(BlueVial, i);
                AddAPhase(GreenVial, i);
                AddAPhase(Under25, i);
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
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            Health = new float[] { 32632000f * 0.80f, 78333504f, 45684800f, 103070400f, 0 };
            MobType = (int)MOB_TYPES.DRAGONKIN;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 1, 1, 1, 1, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            #endregion
            #region Offensive
            #region Multiple Targets
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #endregion
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                #region Ground Phase (80 seconds)
                Phase GroundPhase = new Phase() { Name = "Ground Phase" };
                /* Melee - There is very little tank damage in this fight. Atramedes physical hits are for 18k-25k(10m)/25k-40k(25m) damage, however, there are
                 *      a lot of breaks in his attacks such as when he is channeling Searing Flames or Sonic Breath, and the entire time he is in the air phase which
                 *      does not need a tank. */
                GroundPhase.Attacks.Add(GenAStandardMelee(this[i].Content));
                GroundPhase.LastAttack.AttackSpeed = 3f;
                GroundPhase.LastAttack.DamagePerHit *= 1.3f; // simming the low damage as 85% normal. TODO: Analyze WoL to find proper values
                /* Devastation - This ability is a nonstop blasting every 1.5 seconds of Devastation fireball attacks on a player that has become Noisy! which each deal 25k fire damage.
                 * NOTE: This attack shouldn't be modelled as it's never supposed to happen */
                // Modulation - This is an unavoidable raid-wide pulse that deals 20k Shadow Damage and increases sound level by 7.
                GroundPhase.Attacks.Add(new Attack {
                    Name = "Modulation",
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Shadow,
                    DamagePerHit = 20000,
                    AttackSpeed = 45,
                    MaxNumTargets = Max_Players[i],
                });
                GroundPhase.LastAttack.SetUnavoidable();
                GroundPhase.LastAttack.SetAffectsRoles_All();
                /* Searing Flames - This is an 8 second channeled cast that can and should be interrupted by using an Ancient Dwarven Shield. Otherwise, every second
                 *      the entire raid will take 10k fire damage and get a stacking debuff that increases further fire damage by 25% per stack for 4 seconds.
                 *      Additionally, each tick of Searing Flame causes sound meters to increase by 10. */
                GroundPhase.Attacks.Add(new Attack {
                    Name = "Searing Flames",
                    AttackType = ATTACK_TYPES.AT_AOE, IsDoT = true,
                    DamageType = ItemDamageType.Fire,
                    DamagePerTick = 10000,
                    TickInterval = 1,
                    Duration = 8,
                    AttackSpeed = 45,
                    MaxNumTargets = Max_Players[i],
                });
                GroundPhase.LastAttack.SetUnavoidable();
                GroundPhase.LastAttack.SetAffectsRoles_All();
                GroundPhase.BuffStates.Add(new BuffState {
                     Name = "Searing Flames (Debuff)",
                     Frequency = GroundPhase.LastAttack.AttackSpeed,
                     Duration = GroundPhase.LastAttack.Duration * 1000f,
                     Chance = 1f,
                     Breakable = true,
                     Stats = { },
                     AffectsRole = GroundPhase.LastAttack.AffectsRole,
                });
                GroundPhase.LastBuffState.Stats.AddSpecialEffect(new SpecialEffect(Trigger.Use, new Stats() { FireDamageTakenMultiplier = 0.05f, }, 8, 1, 1f, 8));
                /* Sonic Breath - Atramedes will begin this ability by putting a Tracking debuff on the player in the raid (with the highest sound level???) and after a
                 *      2 second cast, will begin to channel this Sonic Breath for 6 second in the direction of the tracked player. Anyone caught in the path will take 15k fire
                 *      damage per second and have their sound level increased by 20 per tick. During this channel, Atramedes will spin to follow the path of the tracked target at a
                 *      speed relative to that players sound level, moving faster if their sound meter is high, or slower if it is low. */
                GroundPhase.Attacks.Add(new Attack {
                    Name = "Sonic Breath",
                    AttackType = ATTACK_TYPES.AT_RANGED, IsDoT = true,
                    DamageType = ItemDamageType.Fire,
                    DamagePerTick = 15000,
                    TickInterval = 1,
                    Duration = 6,
                    AttackSpeed = 45,
                    MaxNumTargets = 3, // Assuming no more than 3 would get hit at once
                });
                GroundPhase.LastAttack.SetUnavoidable();
                GroundPhase.LastAttack.SetAffectsRoles_All();
                if (i == 2 || i == 3) {
                    /* Sonar Pulse - Three of these pulsating discs are emitted from Atramedes and float straight in a random direction in the room. Players that are hit by these will
                     *      have their sound meter increased by 5 every 0.5 seconds that they are in contact. They should always be avoided, even though they do no damage directly.
                     *      (However, 6k arcane damage per tick in heroic) */
                    GroundPhase.Attacks.Add(new Attack {
                        Name = "Sonic Breath",
                        AttackType = ATTACK_TYPES.AT_RANGED, IsDoT = true,
                        DamageType = ItemDamageType.Arcane,
                        DamagePerTick = 6000,
                        TickInterval = 1,
                        Duration = 6,
                        AttackSpeed = 45,
                        MaxNumTargets = 2, // Assuming no more than 2 would get hit at once
                    });
                    GroundPhase.LastAttack.SetUnavoidable();
                    GroundPhase.LastAttack.SetAffectsRoles_All();
                    /* Obnoxious Fiends - Two will spawn per ground phase—one in the middle and one towards the end. These attach themselves to a random raid member and must be burned
                     *      down immediately by the raid. They have minimal health but cast Obnoxious, an cast which gives the attached player 10 additional sound if not interrupted. */
                    // Only appear on Heroic
                    if ((i == 2) || (i == 3)) {
                        GroundPhase.Targets.Add(new TargetGroup {
                            Name = "Obnoxious Fiends",
                            LevelOfTargets = (int)POSSIBLE_LEVELS.LVLP0,
                            NearBoss = false,
                            NumTargs = 2,
                            Frequency = 45,
                            Duration = 10 * 1000,
                            Chance = 2f / (float)Max_Players[i],
                        });
                        GroundPhase.LastTarget.SetAffectsRoles_All();
                    }
                }
                #endregion
                #region Air Phase (40 seconds)
                // In the Air:
                Phase AirPhase = new Phase() { Name = "Air Phase" };
                /* Sonar Bomb - Atramedes marks several locations on the ground with what look like Sonar Pulses, and after 3 seconds that place will be bombed which hits everyone within
                 *      (8???) yards and deals 10k arcane damage and increases sound level by 20. (30k damage and 30 sound level in heroic)
                 *      NOTE: Not modelling this one as you shouldn't get hit by them */
                /* Sonic Fireball - To prevent the raid from clumping in 1 place, Atramedes will occasionally launch a few of these fireballs in the location of raid members,
                 *      each dealing 30k fire damage to all players within (8??) yards */
                AirPhase.Attacks.Add(new Attack {
                    Name = "Sonic Fireball",
                    AttackType = ATTACK_TYPES.AT_RANGED,
                    DamageType = ItemDamageType.Fire,
                    DamagePerHit = 30000,
                    AttackSpeed = 45,
                    MaxNumTargets = (float)Max_Players[i] / 3f, // Assuming 2/3 of the players aren't in range of someone that gets blasted
                });
                AirPhase.LastAttack.SetUnavoidable();
                AirPhase.LastAttack.SetAffectsRoles_All();
                AirPhase.Moves.Add(new Impedance {
                     Name = "Sonic Fireball (Avoiding)",
                     AffectsRole = AirPhase.LastAttack.AffectsRole,
                     Frequency = AirPhase.LastAttack.AttackSpeed,
                     Duration = 2f * 1000f,
                     Chance = ((float)Max_Players[i] - 3) / (float)Max_Players[i], // All but 3 raid members need to run
                     Breakable = true,
                });
                /* Roaring Flame Breath - This ability is used throughout all of the air phase. He channels a Roaring Flame Breath dealing 10k fire damage that ticks every 0.5
                 *      seconds and follows in the path of the chosen raid member (with the highest sound level???) and moves to follow it at a speed relative to that players sound
                 *      level, moving faster if their sound meter is high, or slower if it is low. In the wake of this ability there will be patches of Roaring Flame which deal 15k
                 *      fire damage and increase sound level by 10 to players that touch them. Additionally, players that stand in the patches too long will gain the debuff Roaring
                 *      Flame which deals an additional 15k fire damage and then ticks every second for 4 seconds resulting in 8k fire damage and a sound meter increase by 5.
                 *      The ground Roaring Flame patches despawn after 45 seconds.
                 *      (*Note: During this phase if the chased player's sound level is getting high, someone use use an Ancient Dwarven Shield to reset all sound meters, stun
                 *      Atramedes, and force him to start pursuit again at the slowest speed following whoever caused the Resonating Clash.
                 *  TODO: Holding off on modelling this just yet */
                #endregion
                #region Apply Phases
                // 10 minute Berserk Timer 80+40=120 600/120=5. At most 10 phases (5 each)
                int phaseStartTime = 0;
                ClearPhase1Values( AirPhase);
                ApplyAPhasesValues( GroundPhase,  i, 1, phaseStartTime, 80, this[i].BerserkTimer); phaseStartTime += 80;
                ApplyAPhasesValues( AirPhase,     i, 2, phaseStartTime, 40, this[i].BerserkTimer); phaseStartTime += 40;
                ApplyAPhasesValues( GroundPhase,  i, 3, phaseStartTime, 80, this[i].BerserkTimer); phaseStartTime += 80;
                ApplyAPhasesValues( AirPhase,     i, 4, phaseStartTime, 40, this[i].BerserkTimer); phaseStartTime += 40;
                ApplyAPhasesValues( GroundPhase,  i, 5, phaseStartTime, 80, this[i].BerserkTimer); phaseStartTime += 80;
                ApplyAPhasesValues( AirPhase,     i, 6, phaseStartTime, 40, this[i].BerserkTimer); phaseStartTime += 40;
                ApplyAPhasesValues( GroundPhase,  i, 7, phaseStartTime, 80, this[i].BerserkTimer); phaseStartTime += 80;
                ApplyAPhasesValues( AirPhase,     i, 8, phaseStartTime, 40, this[i].BerserkTimer); phaseStartTime += 40;
                ApplyAPhasesValues( GroundPhase,  i, 9, phaseStartTime, 80, this[i].BerserkTimer); phaseStartTime += 80;
                ApplyAPhasesValues( AirPhase,     i,10, phaseStartTime, 40, this[i].BerserkTimer);
                AddAPhase(GroundPhase, i);
                AddAPhase(AirPhase, i);
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
            /*for (int i = 0; i < 2; i++)
            {
                //Moves;
                //Stuns;
                //Fears;
                //Roots;
                //Disarms;
            }*/
            TimeBossIsInvuln = new float[] { 0.00f, 0.00f, 0, 0, 0 };
            #endregion
            /* TODO: "Ancient Dwarven Shield" - There are 8 of these (4 on each side of the room) and can be interacted with by any raid member. Using one
             *      causes a Resonating Clash which clears all sound meters and interrupts any spells being cast by Atramedes, as well as giving him a
             *      temporary feeling of Vertigo which causes a 5 second stun where he is vulnerable and takes 50% increased damage. Afterwards, he will
             *      immediately cast Sonic Flames to instantly destroy and melt the Ancient Dwarven Shield that was used.
             *      (*Note: The player that uses the Ancient Dwarven Shield will get an invisible trigger from the Resonating Clash, making them the defaut
             *      for any of Atramedes sound level based attacks until another player passes their sound level. */
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Health values were lowered on Normal by 20% in 4.2
            Health = new float[] { 25939000f * 0.80f, 90616064f * 0.80f, 36246000f, 126776592f, 0 };
            MobType = (int)MOB_TYPES.BEAST;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 450, 450, 0 };
            SpeedKillTimer = new int[] { 6 * 60, 6 * 60, 6 * 60, 6 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 3, 2, 3, 0 };
            Min_Healers = new int[] { 3, 6, 3, 6, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
                for (int i = 0; i < 4; i++)
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

                    IsTheDefaultMelee = true,
                });
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                    = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                    = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                    = true;

                this[i].Attacks.Add(new Attack
                {
                    // Assume that players are staying above the 10k mark for bile
                    Name = "Caustic Slime in P1",
                    DamageType = ItemDamageType.Nature,
                    DamagePerHit = 0.99f, DamageIsPerc = true,
                    MaxNumTargets = new float[] { 1, 3, 1, 3 }[i],
                    AttackType = ATTACK_TYPES.AT_AOE,
                    // Only happens in P1 and until 23% where he stops casting it.
                    // Happens about 6 times per minute, based on
                    // http://www.worldoflogs.com/reports/rt-jjgnyvc6orry0sq0/log/?s=6131&e=6471
                    AttackSpeed = 10f,
                });
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer] = true;
                this[i].BuffStates.Add(new BuffState
                {
                    Name =  "Caustic Slime in P1",
                    Stats = new Stats() { PhysicalHit = -0.75f, SpellHit = -0.75f },
                    Duration = 3f * 1000f,
                    Frequency = 10f,
                    Chance = new float[] { 1, 3, 1, 3 }[i] / (this[i].Max_Players - this[i].Min_Tanks),
                    Breakable = false,
                    AffectsRole = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole,
                });
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Onyxia = 6,600,000 / 24,000,000 / 9,017,400 / 31,500,000
            // Nefarion = 28,500,000 / 98000000 / 36,316,000 / 126,815,650
            // Health values were lowered on Normal by 20% in 4.2
            Health = new float[] { (5582980f + 22761380f), (19755160f + 79707776f), (9240000f + 36316000f), (34786260f + 179342496f), 0 };
            MobType = (int)MOB_TYPES.DRAGONKIN;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.65f, 0.65f, 0.65f, 0.65f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            // 1 tank to tank Oyxia, 1 for kiting (though a hunter or kiting class can do this), and 1 for Nafarion
            Min_Tanks = new int[] { 3, 3, 3, 3, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
                for (int i = 0; i < 4; i++)
                {
                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Nefarian's Melee",
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = BossHandler.StandardMeleePerHit[(int)this[i].Content],
                        MaxNumTargets = 1f,
                        AttackSpeed = 2.0f,
                        AttackType = ATTACK_TYPES.AT_MELEE,
                        IsTheDefaultMelee = true,

                        Dodgable = true,
                        Missable = true,
                        Parryable = true,
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                        = true;

                    #region Onyxia
                    // Onyxia is only up for 2 minutes
                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Onyxia's Melee",
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = BossHandler.StandardMeleePerHit[(int)this[i].Content],
                        MaxNumTargets = 1f,
                        AttackSpeed = BerserkTimer[i] / ( ( BerserkTimer[i] * 0.2f ) / 2.0f ),
                        AttackType = ATTACK_TYPES.AT_MELEE,

                        Dodgable = true,
                        Missable = true,
                        Parryable = true,
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                        = true;

                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Onxyia's Shadowflame Breath",
                        DamageType = ItemDamageType.Shadow,
                        DamagePerTick = 35000f,
                        IsDoT = true,
                        Duration = 1.5f,
                        TickInterval = 0.5f,
                        // Should only hit the tank
                        MaxNumTargets = 1f,
                        AttackType = ATTACK_TYPES.AT_MELEE,
                        AttackSpeed = BerserkTimer[i] / ((BerserkTimer[i] * 0.2f) / 25.0f)
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                        = true;

                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Lightning Discharge",
                        DamageType = ItemDamageType.Nature,
                        DamagePerHit = new float[] { (23400f + 24600f), (23400f + 24600f), (31200f + 32800f), (31200f + 32800f) }[i] / 2f,
                        MaxNumTargets = Max_Players[i],
                        AttackSpeed = BerserkTimer[i] / ((BerserkTimer[i] * 0.2f) / 30.0f),
                        AttackType = ATTACK_TYPES.AT_AOE,
                    });
                    // Only people who should be getting hit by this is range and healers
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                        = true;

                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Onyia's Tail Lash",
                        DamageType = ItemDamageType.Physical,
                        DamagePerHit = (43750f + 56250f) / 2f,
                        MaxNumTargets = Max_Players[i],
                        AttackSpeed = BerserkTimer[i] / ((BerserkTimer[i] * 0.2f) / 30.0f),
                        AttackType = ATTACK_TYPES.AT_AOE,
                    });
                    // happens about the same time as Lightning Discharge
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                        = true;
                    // Stuns users for 2 seconds
                    this[i].Stuns.Add(new Impedance
                    {
                        Chance = 1f,
                        Duration = 2 * 1000f,
                        Breakable = false,
                        Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                    });
                    this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                        = this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                        = this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                        = this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                        = this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                        = true;

                    // Melee needs to move from Onyxia to Nefarian
                    this[i].Moves.Add(new Impedance
                    {
                        Chance = 1f,
                        Frequency = BerserkTimer[i] - 1f,
                        Duration = 8f * 1000f,
                        Breakable = false,
                    });
                    this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                  #endregion
                    #region Animated Bone Warrior
                    // these are up for about half of the fight
                    this[i].Targets.Add(new TargetGroup
                    {
                        NumTargs = 9f,
                        NearBoss = false,
                        Frequency = BerserkTimer[i] - 1f,
                        Chance = 1f,
                        Duration = BerserkTimer[i] * 0.5f * 1000f,
                    });
                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Animated Bone Warrior Melee", IsFromAnAdd = true,
                        DamageType = ItemDamageType.Physical,
                        // these hit for about 10k damage
                        DamagePerHit = (BossHandler.StandardMeleePerHit[(int)this[i].Content]) * 0.1f,
                        MaxNumTargets = 1f,
                        // Need to finalize the attack speed in relation to 9 adds hitting at the same time
                        AttackSpeed = 3f, //BerserkTimer[i] / ( ( BerserkTimer[i] * 0.5f ) / 2.0f ),
                        AttackType = ATTACK_TYPES.AT_MELEE,
                        IsTheDefaultMelee = true,

                        Dodgable = true,
                        Missable = true,
                        Parryable = true,
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                        = true;

                    // Off tank tanks these for half the time
                    // Tertiary tank tanks for 70% of the time
                    this[i].Moves.Add(new Impedance
                    {
                        Duration = (BerserkTimer[i] * 0.5f) * 1000f,
                        Chance = 1f,
                        Frequency = BerserkTimer[i] - 1f,
                        Breakable = false,
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                        = true;
                    this[i].Moves.Add(new Impedance
                    {
                        Duration = (BerserkTimer[i] * 0.7f) * 1000f,
                        Chance = 1f,
                        Frequency = BerserkTimer[i] - 1f,
                        Breakable = false,
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                        = true;

                    #endregion
                    #region Chromatic Prototype
                    // Once Onxyia dies, allow for 8 seconds for everyone to get to their pillers
                    // and another to get set up P3
                    this[i].Moves.Add(new Impedance
                    {
                        Chance = 1f,
                        Frequency = (BerserkTimer[i] * 0.5f) - 1f,
                        Duration = 8f * 1000f,
                        Breakable = false,
                    });
                    this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                        = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                        = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                        = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                        = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                        = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                        = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                        = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                        = true;

                    // Only appear in P2
                    // P2 lasts for 2 minutes
                    this[i].Targets.Add(new TargetGroup
                    {
                        Chance = 1f,
                        Duration = (2f * 60f) * 1000f,
                        Frequency = BerserkTimer[i] - 1f,
                        NearBoss = false,
                        NumTargs = 3f,
                    });
                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Blast Nova",
                        DamageType = ItemDamageType.Fire,
                        DamagePerHit = new float[] { (34800f + 45200f), (34800f + 45200f), (60900f + 79100f), (60900f + 79100f) }[i] / 2f,
                        AttackSpeed = BerserkTimer[i] / ((BerserkTimer[i] * 0.2f) / 8.0f),
                        MaxNumTargets = Max_Players[i],
                        AttackType = ATTACK_TYPES.AT_AOE,
                        Interruptable = true,
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                        = true;
                    #endregion
                    #region Crackle
                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Electrocute",
                        DamageType = ItemDamageType.Nature,
                        DamagePerHit = new float[] { (103950f + 106050f), (103950f + 106050f), (128700f + 131300f), (128700f + 131300f) }[i] / 2f,
                        MaxNumTargets = Max_Players[i],
                        AttackType = ATTACK_TYPES.AT_AOE,
                        // Cast every 10% on Nefarian, so it's cast 9 times throughout the fight
                        AttackSpeed = BerserkTimer[i] / 9f,
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                        = true;
                    #endregion
                    #region Nefarian
                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Nefarian's Shadowflame Breath",
                        DamageType = ItemDamageType.Shadow,
                        DamagePerTick = 35000f,
                        IsDoT = true,
                        TickInterval = 0.5f,
                        Duration = 1.5f,
                        // Should only hit the tank
                        MaxNumTargets = 1f,
                        AttackType = ATTACK_TYPES.AT_MELEE,
                        AttackSpeed = BerserkTimer[i] / ((BerserkTimer[i] * 0.8f) / 25.0f)
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                        = true;

                    // Only casts this during Phase 2
                    this[i].Attacks.Add(new Attack
                    {
                        Name = "Shadowflame Barrage",
                        // Does fire and shadow damage
                        DamageType = ItemDamageType.Shadow,
                        DamagePerHit = (22500f + 27500f) / 2f,
                        AttackType = ATTACK_TYPES.AT_AOE,
                        MaxNumTargets = new float[] { 4f, 10f, 4f, 10f }[i],
                        AttackSpeed = BerserkTimer[i] / ((BerserkTimer[i] * 0.2f) / 3.0f)
                    });
                    this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                        = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                        = true;
                    #endregion

                    #region Heroic Only
                    if ( i == 2 || i == 3 )
                    {
                        // Mind Controlled part of
                        // Free Your Mind - Focus your will to break Nefarian's dominion over your actions.
                        // Stolen Power - Damage and healing of your next spell or ability is increased by 15%.
                        // each stolen Power generates compounting stacking buff
                        // 1, 2, 4, 7, 11, 16, 22, 29, 37, 46
                        // Once at 46 stacks, use "Free Your Mind" to remove the mind control
                        // Basically allow 10 seconds of MC for each person.
                        // Only happens in P1 and P3
                        // First one does not happen until a minute into the fight
                        // each one happens every 18 seconds
                        // 4 MCs in 25 man, possible 2 in 10 man
                        // Does not target MT or OT
                        // Gain a 15% damage bonus for 15 seconds
                        // Does not affect Rip or Bane of Doom
                        // DPS should take the full duration
                        float timer = BerserkTimer[i] / ((BerserkTimer[i] * .7f) / 30f);
                        this[i].Stuns.Add(new Impedance
                        {
                            Breakable = false,
                            Chance = new float[] { 0f, 0f, 3f, 5f }[i] / (Max_Players[i] - Min_Tanks[i]),
                            Duration = 10f * 1000f,
                            Frequency = new float[] { 0f, 0f, timer, timer }[i],
                        });
                        this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                            = this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                            = true;
                        this[i].BuffStates.Add(new BuffState
                        {
                            Name = "Stolen Power",
                            Duration = this[i].Stuns[this[i].Stuns.Count - 1].Duration,
                            Frequency = this[i].Stuns[this[i].Stuns.Count - 1].Frequency,
                            Breakable = this[i].Stuns[this[i].Stuns.Count - 1].Breakable,
                            Chance = this[i].Stuns[this[i].Stuns.Count - 1].Chance,
                            Stats = new Stats() { BonusDamageMultiplier = (0.05f * 46f) },
                        });
                        this[i].BuffStates[this[i].BuffStates.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                            = this[i].BuffStates[this[i].BuffStates.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                            = true;

                        // Healers should break out of it immediately and not generate any stolen Power
                        this[i].Stuns.Add(new Impedance
                        {
                            Breakable = true,
                            Chance = new float[] { 0f, 0f, 3f, 5f }[i] / (Max_Players[i] - Min_Tanks[i]),
                            Duration = 1f * 1000f,
                            Frequency = new float[] { 0f, 0f, timer, timer }[i],
                        });
                        this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                            = this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                            = this[i].Stuns[this[i].Stuns.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                            = true;

                        // Heroic Only
                        // Explosive Cinders - Burns several enemies at random, coating them in explosive residue that inflicts periodic fire damage. 
                        //     The residue detonates after 8 sec, inflicting 42750 to 47250 Fire damage in a small area.
                        // Only happens in P2
                        // Basically the player needs to jump into the lava and let the explosion explode outside the piller group so as not to
                        //      spread the explosion to the rest of the piller group
                        // 3 people are targeted each time.
                        // Targeted players will also have to deal with lava damage (4k damage every second) [assume 5 seconds of in the lava]
                        timer = BerserkTimer[i] / ((BerserkTimer[i] * .3f) / 25f);
                        this[i].Attacks.Add(new Attack
                        {
                            Name = "Explosive Cinders",
                            DamageType = ItemDamageType.Fire,
                            DamagePerHit = new float[] { 0f, 0f, (42750f + 47250f), (42750f + 47250f) }[i] / 2f,
                            IsDoT = true,
                            DamagePerTick = new float[] { 0f, 0f, 2000f, 2000f }[i],
                            TickInterval = 2f,
                            Duration = 8f,
                            MaxNumTargets = new float[] { 0f, 0f, 1f, 3f }[i],
                            AttackType = ATTACK_TYPES.AT_AOE,
                            AttackSpeed = new float[] { 0f, 0f, timer, timer }[i],
                        });
                        this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                            = true;
                       
                        // People have to jump in the lava and take lava damage if they are targeted by explosive cinders
                        this[i].Attacks.Add(new Attack
                        {
                            Name = "Lava Damage",
                            DamageType = ItemDamageType.Fire,
                            DamagePerHit = 0f,
                            IsDoT = true,
                            DamagePerTick = new float[] { 0f, 0f, 4000f, 4000f }[i],
                            TickInterval = 1f,
                            Duration = 5f,
                            MaxNumTargets = new float[] { 0f, 0f, 1f, 3f }[i],
                            AttackType = ATTACK_TYPES.AT_AOE,
                            AttackSpeed = new float[] { 0f, 0f, timer, timer }[i],
                        });
                        this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                            = this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                            = true;

                        this[i].Moves.Add(new Impedance
                        {
                            Breakable = false,
                            Chance = this[i].Attacks[this[i].Attacks.Count - 1].MaxNumTargets / Max_Players[i],
                            Frequency = this[i].Attacks[this[i].Attacks.Count - 1].AttackSpeed,
                            Duration = this[i].Attacks[this[i].Attacks.Count - 1].Duration * 1000f,
                        });
                        this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.MainTank]
                            = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer]
                            = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS]
                            = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer]
                            = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.OffTank]
                            = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer]
                            = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS]
                            = this[i].Moves[this[i].Moves.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank]
                            = true;
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

    #region The Bastion of Twilight
    // ===== The Bastion of Twilight =========================
    public class HalfusWyrmbreaker : MultiDiffBoss
    {
        public HalfusWyrmbreaker()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Halfus Wyrmbreaker";
            Instance = "The Bastion of Twilight";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Kill 3 of the 4 dragons plus Halfus, tanked on top of the boss
            // One of the dragons gives increased damage while stunning the boss
            // Dragons = 4,150,000 / 12,600,649 / 5,810,000 / 17,640,909
            // Halfus = 32,467,000 / 115,954,200 / 45,453,800 / 162,335,880
            // Health values were lowered on normal by 20% in 4.2
            Health = new float[] { 25939384f, 115954200f * 0.80f, 51535200f, 184667808f, 0 };
            MobType = (int)MOB_TYPES.HUMANOID;
            BerserkTimer = new int[] { 6 * 60, 6 * 60, 6 * 60, 6 * 60, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 3, 4, 0 };
            Min_Healers = new int[] { 3, 5, 3, 6, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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

    public class ValionaAndTheralion : MultiDiffBoss
    {
        public ValionaAndTheralion()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Valiona & Theralion";
            Instance = "The Bastion of Twilight";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Health is split between both Valiona and Theralion
            // Health values was lowered on normal by 20% in 4.2
            Health = new float[] { 25767600f, 97916880f * 0.80f, 48099520f, 164912640f, 0 };
            MobType = (int)MOB_TYPES.DRAGONKIN;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.97f, 0.97f, 0.97f, 0.97f, 0 };
            InBackPerc_Ranged = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 1, 1, 2, 2, 0 };
            Min_Healers = new int[] { 2, 6, 3, 6, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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

    public class TwilightAscendantCouncil : MultiDiffBoss
    {
        public TwilightAscendantCouncil()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Twilight Ascendant Council";
            Instance = "The Bastion of Twilight";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Arion = 4,724,000 / 14,600,000 / 6,613,600 (Guess) / 20,440,000 (Guess)
            // Terrastra = 4,724,000 / 14,600,000 / 6,613,600 (Guess) / 20,440,000 (Guess)
            // Ignacious = 6,871,000 / 21,900,000 / 9,619,400 (Guess) / 30,660,000 (Guess)
            // Feludious = 6,871,000 / 21,900,000 / 9,619,400 (Guess) / 30,660,000 (Guess)
            // Elemental Monstrocity = Total Health between all four his "parts."
            // His current health going into P3 is whatever is the remainder of what is currently on all mobs put together
            // So for P3, it's best to keep everyone at or close to 25% as possible going into each phase.
            // Health values were lowered on normal by 20% in patch 4.2
            Health = new float[] { ((4724060f * 2f) + (6871060f * 2f)), ((14600000f * 2f) + (21900000f * 2f)), ((12850854f * 2f) + (8374470f * 2f)), ((24600000f * 2f) + (38000000f * 2f)), 0 };
            MobType = (int)MOB_TYPES.ELEMENTAL;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 0 };
            Min_Healers = new int[] { 3, 5, 3, 5, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Health values on Normal were lowered by 20% in 4.2
            Health = new float[] { 26798304f, 101352560f * 0.80f, 47000000f, 146000000f, 0 };
            MobType = (int)MOB_TYPES.HUMANOID;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 3, 3, 0 };
            Min_Healers = new int[] { 3, 5, 3, 6, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
            // She is only available in the Heroic version
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Her health starts at 60%
            // you bring her to 30% to start P2
            // At the end of P2, she restors her health to 100%
            Health = new float[] { 42946000f * 1.3f, 128838000f * 1.3f, 0f, 0f, 0f };
            MobType = (int)MOB_TYPES.DRAGONKIN;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 0, 0, 0 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 0, 0, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0, 0, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0, 0, 0 };
            Max_Players = new int[] { 10, 25, 0, 0, 0 };
            Min_Tanks = new int[] { 2, 2, 0, 0, 0 };
            Min_Healers = new int[] { 3, 5, 0, 0, 0 };
            Under35Perc = new double[] { 0.166666667, 0.166666667, 0, 0, 0 };
            Under20Perc = new double[] { 0.151515152, 0.151515152, 0, 0, 0 };

            // Assume it takes 60 seconds to get through P2.
            // Technically she isn't invulneratble, it's just while in P2, she heals herself fairly quickly
            // so whatever damage you do to her is healed immediately. But for all intense and purpose, she is invulnerable.
            TimeBossIsInvuln = new float[] { 60, 60, 0, 0, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 2; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                // Flame Breath
                this[i].Attacks.Add(new Attack
                {
                    Name = "Flame Breath",
                    // only used during P1 and P3 with a 20 second cd
                    // Assume 1 minute in P2
                    AttackSpeed = (float)this[i].BerserkTimer / (((float)this[i].BerserkTimer - 60f) / 20f ),
                    DamageType = ItemDamageType.Fire,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamagePerHit = 66000,
                    MaxNumTargets = Max_Players[i],

                    Blockable = false,
                    Parryable = false,
                    Interruptable = false,
                    Missable = false,
                    Dodgable = false,
                });

                // Shadow Orb
                this[i].Attacks.Add (new Attack
                {
                    Name = "Shadow Orb",
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = 75000f,
                    MaxNumTargets = 2f,
                    // Assume it only hits the two targets and no more.
                    AttackSpeed = (float)this[i].BerserkTimer / (((float)this[i].BerserkTimer - 60f) / 30f ),

                    Blockable = false,
                    Parryable = false,
                    Interruptable = false,
                    Missable = false,
                    Dodgable = false,
                });
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer] = true;
                this[i].Attacks.Add(new Attack
                {
                    Name = "Wrack",
                    IsDoT = true,
                    DamageType = ItemDamageType.Shadow,
                    AttackType = ATTACK_TYPES.AT_AOE,
                    TickInterval = 2f,
                    Duration = 30,
                    // Starts out as 2k damage a tick, goes up to 60k damage a tick after 30 ticks.
                    // Once dispelled you spread the debuff to 2 other players
                    // Those players get the remaining time left but start from 0 damage
                    // Preferably around the 30 second mark so that the remaining players don't get hit too hard by the debuff
                    // Total damage after 30 seconds = 240,000 = 
                    // 2k + 4k + 6k + 8k + 10k + 12k + 14k + 16k + 18k + 20k + 22k + 24k + 26k + 28k + 30k
                    DamagePerTick = 240000f / 15f,
                    Interruptable = false,
                    AttackSpeed = (float)this[i].BerserkTimer / (((float)this[i].BerserkTimer - 60f) / 60f ),
                    MaxNumTargets = 3f,

                    Dodgable = false,
                    Parryable = false,
                    Blockable = false,
                    Missable = false,
                });
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.TertiaryTank] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MeleeDPS] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RangedDPS] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.MainTankHealer] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.OffAndTertTankHealer] = true;
                this[i].Attacks[this[i].Attacks.Count - 1].AffectsRole[PLAYER_ROLES.RaidHealer] = true;
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
                this[i].Moves.Add(new Impedance
                { // Shadow Orb
                    Breakable = false,
                    Chance = 2f / (this[i].Max_Players - this[i].Min_Tanks),
                    Duration = 15f * 1000f,
                    Frequency = (float)this[i].BerserkTimer / (((float)this[i].BerserkTimer - 60f) / 30f )
                });
                // Buff State
                this[i].BuffStates.Add(new BuffState
                {
                    Chance = 1f,
                    // Lasts for 3 minutes
                    Duration = 3f * 60f * 1000f,
                    Name = "Essence of the Red",
                    Breakable = false, //Why break a 100% haste buff
                    // Provides a 100% haste buff
                    Stats = new Stats() { PhysicalHaste = 1f, RangedHaste = 1f, SpellHaste = 1f, ManaorEquivRestore = .05f },
                    Frequency = (float)this[i].BerserkTimer - (3f * 60f), // Only happens once a fight
                });
                // Adds
                this[i].Targets.Add(new TargetGroup
                {
                    // Pulsing Twilight Egg
                    // Allow 1 minute to kill both targets
                    Duration = 60f * 1000f,
                    Chance = 1f,
                    Frequency =  (float)this[i].BerserkTimer - (60f), // Only happens once a fight
                    NearBoss = false,
                    NumTargs = 2f,
                });
                this[i].Targets.Add(new TargetGroup
                {
                    // Twilight Spitecaller
                    Duration = 60f * 1000f,
                    Chance = 1f,
                    Frequency = (float)this[i].BerserkTimer - (60f), // Only happens once a fight
                    NearBoss = false,
                    NumTargs = 4f,
                });
                this[i].Targets.Add(new TargetGroup
                {
                    // Twilight Drake
                    Duration = 60f * 1000f,
                    Chance = 1f,
                    Frequency = (float)this[i].BerserkTimer - (60f), // Only happens once a fight
                    NearBoss = false,
                    NumTargs = 1f,
                });
                this[i].Targets.Add(new TargetGroup
                {
                    // Twilight Whelps
                    Duration = 60f * 1000f,
                    Chance = 1f,
                    // Spawns once a minute during P1 and P2
                    Frequency = (float)this[i].BerserkTimer / (((float)this[i].BerserkTimer - (60f)) / 60f),
                    NearBoss = false,
                    NumTargs = 5f,
                });
            }
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Rohash = 4,120,000 / 14,429,856 / 5,768,000 / 20,201,799
            // Anshal = 4,120,000 / 14,429,856 / 5,768,000 / 20,201,799
            // Nezir = 7,210,000 / 25,252,248 / 10,094,000 / 35,353,148
            // Health values were lowered on normal by 20% in 4.2
            Health = new float[] { (4294600f + 4294600f + 7300820f) * 0.80f, (14601640f + 14601640f + 25252248f) * 0.80f, (5768000f + 5768000f + 10094000f), (23362624f + 23362624f + 41228160f), 0 };
            MobType = (int)MOB_TYPES.ELEMENTAL;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 0 };
            Min_Healers = new int[] { 3, 5, 3, 6, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T11_10, BossHandler.TierLevels.T11_25, BossHandler.TierLevels.T11_10H, BossHandler.TierLevels.T11_25H, BossHandler.TierLevels.T11_LFR };
            #endregion
            #region Basics
            // Health values were lowered on Normal by 20% in 4.2
            Health = new float[] { 24049760f, 84174160f, 48099520f, 168348320f, 0 };
            MobType = (int)MOB_TYPES.ELEMENTAL;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60, 0 };
            SpeedKillTimer = new int[] { 3 * 60, 3 * 60, 3 * 60, 3 * 60, 0 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f, 0 };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f, 0 };
            Max_Players = new int[] { 10, 25, 10, 25, 0 };
            Min_Tanks = new int[] { 2, 2, 2, 2, 0 };
            Min_Healers = new int[] { 3, 5, 3, 6, 0 };
            #endregion
            #region Offensive
            //MaxNumTargets = new double[] { 1, 1, 0, 0 };
            //MultiTargsPerc = new double[] { 0.00d, 0.00d, 0.00d, 0.00d };
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));
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
