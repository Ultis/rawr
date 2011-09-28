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
    public class Alizabal : MultiDiffBoss
    {
        public Alizabal()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Alizabal, Mistress of Hate";
            Instance = "Baradin Hold";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T13_10, BossHandler.TierLevels.T13_25, BossHandler.TierLevels.T13_10H, BossHandler.TierLevels.T13_25H };
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
            TimeBossIsInvuln = new float[] { 2f * 15f, 2f * 15f, 0, 0 };
            #endregion
            #region The Phases
            for (int i = 0; i < 2; i++)
            {
                Phase Phase1 = new Phase() { Name = "Phase 1: Soaking" };
                Phase Phase2 = new Phase() { Name = "Phase 2: Blade Dance" };

                #region MT and OT Melee Swapping
                // MT and OT tank swap
                // Each should take half of the total damage
                // does not melee during Firestorm
                Phase1.Attacks.Add(new Attack
                {
                    Name = "MT Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    IsTheDefaultMelee = true,
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    MaxNumTargets = 1f,
                    AttackSpeed = 5f,
                });
                Phase1.LastAttack.AffectsRole[PLAYER_ROLES.MainTank] = true;

                Phase1.Attacks.Add(new Attack
                {
                    Name = "OT Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    IsTheDefaultMelee = true,
                    DamageType = ItemDamageType.Physical,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    MaxNumTargets = 1f,
                    AttackSpeed = 5f,
                });
                Phase1.LastAttack.AffectsRole[PLAYER_ROLES.OffTank] = true;
                #endregion

                #region Skewer
                // Alizabal skewers her current target, stunning them. In addition, Skewer inflicts 20,000 damage 
                // every second and increases damage taken by 100%.
                // http://ptr.wowhead.com/spell=104936
                // Tanks switch every 10 seconds so minimize any extra damage the tank gets while stunned.
                // TODO: double check attack speed of the attack
                Phase1.Attacks.Add(new Attack
                {
                    Name = "Skewer [Main Tank]",
                    SpellID = 104936,
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerTick = (18000f + 22000f) / 2f,
                    IsDoT = true,
                    Duration = 8f,
                    TickInterval = 1f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 20f,
                });
                Phase1.LastAttack.SetUnavoidable();
                Phase1.LastAttack.AffectsRole[PLAYER_ROLES.MainTank] = true;
                Phase1.Stuns.Add(new Impedance
                {
                    Name = "Skewer [Main Tank] Stun",
                    Chance = 1f / Max_Players[i],
                    Duration = 8f * 1000f,
                    Frequency = Phase1.LastAttack.AttackSpeed,
                });
                Phase1.BuffStates.Add(new BuffState
                {
                    Name = "Skewer [Main Tank] Increased Damage Taken",
                    Chance = 1f / Min_Tanks[i],
                    Duration = 8f * 1000f,
                    Frequency = Phase1.LastAttack.AttackSpeed,
                    Breakable = false,
                    Stats = new Stats() { DamageTakenReductionMultiplier = -1f },
                });
                Phase1.LastBuffState.SetAffectsRoles_Tanks();

                Phase1.Attacks.Add(new Attack
                {
                    Name = "Skewer [Off Tank]",
                    SpellID = 104936,
                    DamageType = ItemDamageType.Physical,
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerTick = (18000f + 22000f) / 2f,
                    IsDoT = true,
                    Duration = 8f,
                    TickInterval = 1f,
                    MaxNumTargets = 1f,
                    AttackSpeed = 20f,
                });
                Phase1.LastAttack.SetUnavoidable();
                Phase1.LastAttack.AffectsRole[PLAYER_ROLES.OffTank] = true;
                Phase1.Stuns.Add(new Impedance
                {
                    Name = "Skewer [Off Tank] Stun",
                    Chance = 1f / Max_Players[i],
                    Duration = 8f * 1000f,
                    Frequency = Phase1.LastAttack.AttackSpeed,
                });
                Phase1.BuffStates.Add(new BuffState
                {
                    Name = "Skewer [Off Tank] Increased Damage Taken",
                    Chance = 1f / Min_Tanks[i],
                    Duration = 8f * 1000f,
                    Frequency = Phase1.LastAttack.AttackSpeed,
                    Breakable = false,
                    Stats = new Stats() { DamageTakenReductionMultiplier = -1f },
                });
                Phase1.LastBuffState.SetAffectsRoles_Tanks();
                #endregion

                #region Seething Hate
                // Alizabal incites Seething Hate on a random target causing 100,000 damage to all players within
                // 6 yards, splitting the damage between everyone hit.
                // http://ptr.wowhead.com/spell=105069
                // Attack is a dot that hits for every 3 seconds lasting 9 seconds.
                // Raid stacks up to spread the damage
                Phase1.Attacks.Add(new Attack
                {
                    Name = "Seething Hate",
                    SpellID = 105069,
                    IsDoT = true,
                    // From videos watched, it only appeares to affect range dps and healers.
                    AttackType = ATTACK_TYPES.AT_AOE,
                    DamageType = ItemDamageType.Shadow,
                    Duration = 9f,
                    TickInterval = 3f,
                    DamagePerTick = ((97500f + 102500f) / 2f) / Max_Players[i],
                    // Need verification on how many are hit.
                    MaxNumTargets = Max_Players[i],
                    // Need verification on timing, but there is a dummy effect with an 8 second cooldown
                    AttackSpeed = 8f,
                });
                Phase1.LastAttack.SetUnavoidable();
                Phase1.LastAttack.SetAffectsRoles_DPS();
                Phase1.LastAttack.SetAffectsRoles_Healers();
                #endregion

                #region Blade Dance
                // Alizabal enters a Blade Dance inflicting 10,000 physical damage every second to all players
                // withing 15 yards. In addition, during Blade Dance all incoming attacks are reflected. 
                // http://ptr.wowhead.com/spell=104994
                Phase2.Attacks.Add(new Attack
                {
                    Name = "Blade Dance",
                    SpellID = 104994f,
                    DamagePerHit = new float[] { 10000, 10000, 0, 0 }[i],
                    DamageType =  ItemDamageType.Physical,
                    AttackSpeed = 1f,
                    MaxNumTargets = Max_Players[i],
                    AttackType = ATTACK_TYPES.AT_AOE,
                    Duration = 15f * 1000f,
                });
                Phase2.LastAttack.SetAffectsRoles_All();

                // Everyone needs to move out
                Phase2.Moves.Add(new Impedance
                {
                    Name = "Blade Dance Move",
                    Chance = Phase2.LastAttack.MaxNumTargets / Max_Players[i],
                    Duration = 4f * 1000f,
                    Frequency = Phase2.LastAttack.Duration,
                });
                #endregion

                #region Apply Phases
                float phaseStartTime = 0;
                float phase1length = 2f * 60f;
                float phase2length = 15f;

                ClearPhase1Values(ref Phase1);
                ClearPhase1Values(ref Phase2);

                ApplyAPhasesValues(ref Phase1, i, 1, phaseStartTime, phase1length, this[i].BerserkTimer); phaseStartTime += phase1length;
                ApplyAPhasesValues(ref Phase2, i, 2, phaseStartTime, phase2length, this[i].BerserkTimer); phaseStartTime += phase2length;
                ApplyAPhasesValues(ref Phase1, i, 1, phaseStartTime, phase1length, this[i].BerserkTimer); phaseStartTime += phase1length;
                ApplyAPhasesValues(ref Phase2, i, 2, phaseStartTime, phase2length, this[i].BerserkTimer); phaseStartTime += phase2length;
                ApplyAPhasesValues(ref Phase1, i, 1, phaseStartTime, this[i].BerserkTimer - phaseStartTime, this[i].BerserkTimer);

                AddAPhase(Phase1, i);
                AddAPhase(Phase2, i);
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
        }
    }
    #endregion

    #region Dragon Soul
    // ===== Dragon Soul ======================
    public class Morchok : MultiDiffBoss
    {
        public Morchok()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Morchok";
            Instance = "Dragon Soul";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T13_10, BossHandler.TierLevels.T13_25, BossHandler.TierLevels.T13_10H, BossHandler.TierLevels.T13_25H, };
            #endregion
            #region Basics
            Health = new float[] { 17693752f, 53167148f, 27829008f, 83658808f };
            MobType = (int)MOB_TYPES.ELEMENTAL;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
            #endregion
            #region Offensive
            for (int i = 0; i < 4; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                Attack Melee = new Attack
                {
                    Name = "Default Melee",
                    AttackType = ATTACK_TYPES.AT_MELEE,
                    DamagePerHit = BossHandler.StandardMeleePerHit[(int)Content[i]],
                    AttackSpeed = 2.5f,
                };
                Melee.AffectsRole[PLAYER_ROLES.MainTank] = true;
                EntirePhase.Attacks.Add(Melee);
                Attack MeleeP2 = Melee.Clone();
                Melee.AffectsRole[PLAYER_ROLES.OffTank] = true;
                EntirePhase.Attacks.Add(MeleeP2);

                #region The Entire Phase
                /* Stomp - Morchok performs a massive stomp, splitting 500000 damage between all enemies within 20 yards. The two closest targets take a double share of the damage.
                 * Crush Armor - Morchok reduces his target's armor by 10% for 20 sec. Effect stacks up to 5 times.
                 * Twilight Orb - Morchok conjures an explosive crystal, which explodes after 12 sec. The explosion splits the damage between the five closest enemies. The total damage increases the further the enemy is from the crystal. Designer Note: 150000 damage base.
                 * The Earth Consumes You! - Morchok channels an intense spell, causing fragments to erupt to the surface and draw the Black Blood of the Earth to the surface. For 5 sec, he deals 5000 damage to all visible enemies within 4 yards.
                 *      Falling Fragments - Stalagmites erupt from the ground, dealing 15000 damage to enemies and creating obstacles which block spells.
                 *      Black Blood of the Earth - Morchok channels the blood of the earth and inflicts 5000 Nature damage to nearby enemies, increasing Nature damage taken by 10% every sec. Stacks up to 20 times.
                 */
                #endregion

                #region Apply Phases
                float p1duration = 0f;
                ApplyAPhasesValues(ref EntirePhase, i, 0, 0, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
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

    public class WarlordZonozz : MultiDiffBoss
    {
        public WarlordZonozz()
        {
            #region Info
            Name = "Warlord Zon'ozz";
            Instance = "Dragon Soul";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T13_10, BossHandler.TierLevels.T13_25, BossHandler.TierLevels.T13_10H, BossHandler.TierLevels.T13_25H, };
            #endregion
            #region Basics
            Health = new float[] { 13100001f, 40000000f, 20000000f, 60000004f };
            MobType = (int)MOB_TYPES.ELEMENTAL;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase EntirePhase = new Phase() { Name = "Entire Phase" };

                // He doesn't have a default melee attack during Obsidian Form (p1)
                // this[i].Attacks.Add(GenAStandardMelee(this[i].Content)); 

                #region Entire Phase
                /* Focused Anger -Every 5 seconds, Warlord Zon'ozz becomes increasingly focused on his primary target, increasing damage done by 5%.
                 * Psychic Drain - Warlord Zon'ozz channels a wave of psychic force in a cone in front of the himself, dealing 120000 Shadow damage, leeching life for ten times that amount.
                 * Disrupting Shadows - Warlord Zon'ozz curses random targets with Disrupting Shadows. Disrupting Shadows deal 30000 Shadow damage every 2 seconds. If removed, it will knock back all players within 10 yards.
                 * Void of the Unmaking - Warlord Zon'ozz summons a void of shadows. This void will travel forward until it encounters a soul to absorb. The void then diffuses, causing 200000 Shadow damage split evenly between nearby units. The force of this diffusion causes the void to richocet in the opposite direction, and the souls it absorbed increases the damage it deals by % per diffusion.
                 * A Necessary Distraction! - If the Void of the Unmaking collides with the Warlord Zon'ozz, it will cause a distracting shock to him, increasing the damage he takes by 5% for every stack of Void Diffusion that the void had on impact. This collision enrages Warlord Zon'ozz, causing him to awaken the Mouth of Iso'rath, but lose any stacks of Focused Anger he had acquired.
                 *      Claw of Iso'rath - These units will knock back nearby players and emit the Blood of the Old Gods.
                 *          Blood of the Old Gods - Deals 25000 Shadow damage every 200 second to all nearby enemies.
                 *      Eye of Iso'rath - These units will cast a Shadowy Gaze at a random enemy, dealing 20000 Shadow damage.
                 */
                #endregion

                #region Add Phases
                int phaseStartTime = 0;
                ApplyAPhasesValues(ref EntirePhase, i, 1, phaseStartTime, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntirePhase, i);
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
        }
    }

    public class Yorsahj : MultiDiffBoss
    {
        public Yorsahj()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Yor'sahj the Unsleeping";
            Instance = "Dragon Soul";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T13_10, BossHandler.TierLevels.T13_25, BossHandler.TierLevels.T13_10H, BossHandler.TierLevels.T13_25H, };
            #endregion
            #region Basics
            Health = new float[] { 38651400f, 115954200f, 71892800f, 197122144f }; // TODO: double check 25-man normal health pool
            MobType = (int)MOB_TYPES.ELEMENTAL;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 8 * 60, 8 * 60, 8 * 60, 8 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase EntireFight = new Phase() { Name = "Entire Fire" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                #region Entire Fight
                /* Void Bolt
                 *      Soulflayer Yor'sahj blasts his primary target with dark energy, dealing 75000 Shadow damage and reducing the amount of healing they take by 5%.
                 *
                 * Call Blood of the Old Gods
                 *      Soulflayer Yor'sahj calls to the might of the Old Gods, summoning a globule of their blood. These globules will path slowly towards Yor'sahj, infusing him with their powers if they reach him.
                 *
                 *      Fusing Vapors - When attacked, the blood globules emit a vapor that fuses with all other nearby globules, healing them for 5% of their maximum life
                 *      Glowing Blood of the Old Gods - Yor'sahj becomes Infused with the Glowing Blood of the Old Gods, causing Void Bolt to hit all nearby targets, reducing the cooldowns by half, and increasing attack speed by 50%.
                 *      Cobalt Blood of the Old Gods - Grants the ability: Mana Void. Mana voids will leech the mana from all casters and healers, storing it in the void. Destroying the mana void will return the sum of the mana leeched evenly to all units within 18 yards.
                 *      Crimson Blood of the Old Gods - Grants the ability: Searing Blood. Yor'sahj sears the blood of random enemies, dealing 25000 base Fire damage. The further the target stands from Yor'sahj, the more shock they receive from the blast.
                 *      Black Blood of the Old Gods - Causes the mouth of Iso'rath to become a bubbling cesspool of corruption, spawning Forgotten Ones periodically.
                 *          Forgotten One - These creatures fixate on a random target.
                 *              Psychic Slice - Channels a wave of psychic force in a cone in front of the caster, dealing 120000 Shadow damage.
                 *      Acidic Blood of the Old Gods - Causes the mouth of Iso'rath to bubble with Digestive Juices. Digestive acid periodically leaks from the stomach lining, dealing 35000 Nature damage to an enemy and its surrounding allies within 4 yards.
                 *      Shadowed Blood of the Old Gods - Grants the ability: Deep Corruption. Healing or absorbtion effects taken while under this effect will trigger a detonation at 5 stacks, dealing 50000 Shadow damage to all nearby allies.
                 */
                #endregion

                #region Add Phases
                int phaseStartTime = 0;
                ApplyAPhasesValues(ref EntireFight, i, 1, phaseStartTime, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntireFight, i);
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
             * All
             */
        }
    }

    public class Hagara : MultiDiffBoss
    {
        public Hagara()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Hagara the Stormbinder";
            Instance = "Dragon Soul";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T13_10, BossHandler.TierLevels.T13_25, BossHandler.TierLevels.T13_10H, BossHandler.TierLevels.T13_25H, };
            #endregion
            #region Basics
            Health = new float[] { (20442296f + (7901028f * 2f)), (61198050f + (25252248f * 2f)), 28619216f, 97100904f };
            MobType = (int)MOB_TYPES.HUMANOID;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 7 * 60, 7 * 60, 7 * 60, 7 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase EntireFight = new Phase() { Name = "Entire Fire" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                #region Entire Fight
                /* You pups don't stand a chance!
                 *      Hagara wields the power of ice to paralyze and destroy her enemies.
                 *
                 *      Ice Lance - Hagara conjures crystals of ice that fire lances towards melee attackers. The lances deal 15000 Frost damage in a 3 yard area around the first enemy struck.
                 *      Ice Tomb - Hagara traps an enemy in a tomb of ice. The tomb remains until destroyed.
                 *      Frost Orb - Launches a missile at a random enemy which lands 8 seconds later. Inflicts 53000 Frost damage and reduces the enemy's movement speed for 4 sec.
                 *      Focused Assault - Slices rapidly at the enemy for 5 sec.
                 *
                 * Frozen Tempest
                 *      Hagara repels all enemies to the outer ring of the Oculus, where they must flee a moving wave of ice.
                 *
                 *      Water Shield - While casting Frozen Tempest, Hagara protects herself with a shield of water, granting her invulnerability.
                 *      Binding Crystal - Four binding crystals protect Hagara while she channels Frozen Tempest. Destroying them removes her shield.
                 *
                 * Electrical Conduit
                 *      Hagara conjures a lightning storm to electrocute all nearby enemies.
                 *
                 *      Crystal Conductor - Hagara conjures crystal rods which protect her from the lightning storm she conjures. Crystal rods connected to the storm deal Nature damage to allies unfortunate enough to be close.
                 *      Bound Lightning Elementals - Hagara calls two elementals to aid her during the electrical storm. Upon death, they cause a conductor to overload.
                 */
                #endregion

                #region Add Phases
                int phaseStartTime = 0;
                ApplyAPhasesValues(ref EntireFight, i, 1, phaseStartTime, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntireFight, i);
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
             * Moves for Hurl Spear & Traps.
             */
        }
    }

    public class Ultraxion : MultiDiffBoss
    {
        public Ultraxion()
        {
            #region Info
            Name = "Ultraxion";
            Instance = "Dragon Soul";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T13_10, BossHandler.TierLevels.T13_25, BossHandler.TierLevels.T13_10H, BossHandler.TierLevels.T13_25H, };
            #endregion
            #region Basics
            Health = new float[] { 31565310f, 99978288f, 59428676f, 166239664f };
            MobType = (int)MOB_TYPES.ELEMENTAL;
            BerserkTimer = new int[] { 6 * 60, 6 * 60, 6 * 60, 6 * 60 };
            SpeedKillTimer = new int[] { 5 * 60, 5 * 60, 5 * 60, 5 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 2, 2, 2, 2 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase EntireFight = new Phase() { Name = "Entire Fire" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                #region Entire Fight
                #endregion

                #region Add Phases
                int phaseStartTime = 0;
                ApplyAPhasesValues(ref EntireFight, i, 1, phaseStartTime, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntireFight, i);
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
             * Heroic
             */
        }
    }

    public class WarmasterBlackhorn : MultiDiffBoss
    {
        public WarmasterBlackhorn()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Warmaster Blackhorn";
            Instance = "Dragon Soul";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T13_10, BossHandler.TierLevels.T13_25, BossHandler.TierLevels.T13_10H, BossHandler.TierLevels.T13_25H, };
            #endregion
            #region Basics
            Health = new float[] { 38221940f, 133927104f, 105990728f, 367274176f }; // TODO: Double check 25-man normal health value
            MobType = (int)MOB_TYPES.HUMANOID;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 5 * 60, 5 * 60, 5 * 60, 5 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 1, 1, 1, 1 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase EntireFight = new Phase() { Name = "Entire Fire" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                #region Entire Fight
                #endregion

                #region Add Phases
                int phaseStartTime = 0;
                ApplyAPhasesValues(ref EntireFight, i, 1, phaseStartTime, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntireFight, i);
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
             * all
             */
        }
    }

    public class SpineofDeathwing : MultiDiffBoss
    {
        public SpineofDeathwing()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Spine of Deathwing";
            Instance = "Dragon Soul";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T13_10, BossHandler.TierLevels.T13_25, BossHandler.TierLevels.T13_10H, BossHandler.TierLevels.T13_25H, };
            #endregion
            #region Basics
            Health = new float[] { 38221940f, 133927104f, 105990728f, 367274176f }; // TODO: Double check 25-man normal health value
            MobType = (int)MOB_TYPES.HUMANOID;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 5 * 60, 5 * 60, 5 * 60, 5 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 1, 1, 1, 1 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase EntireFight = new Phase() { Name = "Entire Fire" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                #region Entire Fight
                /* Barrel Roll
                        Throughout the encounter, Deathwing tries to throw players from his back when he senses that they are all standing on the same side of his spine. During a barrel roll, all players and other creatures not attached to his back are thrown off.

                   Elementium Reinforced Plates
                        The glancing strike from the Dragon Soul has destroyed one of the large armor plates on Deathwing's back and loosened another three. All of these plates must be removed if Thrall is to have a chance to get a clear shot.

                   Burning Tendons - Burning Tendons are exposed when one of Deathwing's armor plates is pried up. Destroying these fibrous strands is the key to removing an armor plate.
                        Seal Armor Breach - When an armor plate is pried up, the Burning Tendons rapidly pull the plate closed, protecting the tendons from further damage and preventing the plate from flying off.

                   Deathwing's Immune System
                        The creatures that comprise Deathwing's Immune System emerge from the holes in his back left by torn off armor plates. Deathwing defends himself with increasing intensity as more plates are torn off.

                        Grasping Tendrils - These tendrils sprout from holes in Deathwing's back that aren't currently occupied by Corruption tentacles. Players that wander too close will be grasped, reducing their movement speed by 50% and inflicting 15000 Fire damage every 0 sec. until they move away. This effect prevents players from being thrown off Deathwing's back.
                        Corruption - These tentacles are dramatic physical manifestations of the Old God Corruption that runs rampant through Deathwing's body. They are the first to emerge from the holes in his back left by armor plates, and they seal the wound completely, preventing any other creatures from emerging from it.
                            Searing Plasma - Coats the victim in the searing blood of Deathwing. The thick blood will absorb up to 300000 healing done to the target and will cause the victim to cough up blood, periodically inflicting 10000 Physical damage.
                            Fiery Grip - Grips victims with a whip-like cord of plasma, stunning them for up to 30 sec and inflicting 50000 Fire damage every 5 sec. The corruption must channel to maintain this effect, and it may be broken by inflicting significant damage to the tentacle. This effect prevents players from being thrown off Deathwing's back.
                        Hideous Amalgamation - These unstable creatures are a patchwork of Elementium Armor fragments and pieces of the former Dragon's body held together by his molten blood.
                            Absorb Blood - Absorbs nearby Corrupted Blood residue. The Amalgamation grows more unstable with each Residue absorbed, increasing its damage by 10% and attack speed by 10%. At 10 stacks it will become superheated.
                            Superheated Nucleus - The core of the Amalgamation becomes critically unstable, causing it to inflict 15000 Fire damage to all enemies every 3 sec. If it is destroyed in this state it will trigger a Nuclear Blast.
                            Nuclear Blast - The superheated nucleus of the Amalgamation triggers a massive explosion, inflicting 500000 Fire damage to targets within 10 yds. This blast is powerful enough to pry up one of Deathwing's Elementium plates if it is within range.
                            Degradation - Destroyed Amalgamations degrade the maximum health of all enemy targets by 6%. This effect occurs regardless of the state of the nucleus and stacks up to 15 times.
                            Blood Corruption: Death - Deathwing's corruption courses through your veins. This corruption will take hold if it is not removed quickly, infecting the player with the Blood of Deathwing. When dispelled this effect jumps to a new target and can mutate into Blood Corruption: Earth.
                                Blood of Deathwing - The blood of Deathwing explodes violently from your veins, inflicting 875000 Fire damage to you and several other players.
                            Blood Corruption: Earth - A shadow of Neltharion courses through your veins. This corruption will take hold if it is not removed quickly, infecting the player with the Blood of Neltharion. When dispelled this effect jumps to a new target and can mutate into Blood Corruption: Death.
                                Blood of Neltharion - The barest hint of a shadow of the former Earth-Warder suffuses your blood. Damage taken from all sources is reduced by 10%. This effect can stack up to 3 times.
                        Corrupted Blood - These living globs of Deathwing's blood appear fragile, but leave behind an indestructible residue when killed.
                            Burst - Corrupted Blood explodes when destroyed, inflicting 25000 Fire damage to nearby enemies.
                            Residue - This indestructible residue is left behind when a Corrupted Blood is destroyed. It slowly creeps toward the nearest hole in Deathwing's Back where it will be reconstituted into a new Corrupted Blood.
                 */
                #endregion

                #region Add Phases
                int phaseStartTime = 0;
                ApplyAPhasesValues(ref EntireFight, i, 1, phaseStartTime, this[i].BerserkTimer, this[i].BerserkTimer);
                AddAPhase(EntireFight, i);
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
             * all
             */
        }
    }

    public class MadnessofDeathwing : MultiDiffBoss
    {
        public MadnessofDeathwing()
        {
            // If not listed here use values from defaults
            #region Info
            Name = "Madness of Deathwing";
            Instance = "Dragon Soul";
            Content = new BossHandler.TierLevels[] { BossHandler.TierLevels.T13_10, BossHandler.TierLevels.T13_25, BossHandler.TierLevels.T13_10H, BossHandler.TierLevels.T13_25H, };
            #endregion
            #region Basics
            Health = new float[] { 38221940f, 133927104f, 105990728f, 367274176f }; // TODO: Double check 25-man normal health value
            MobType = (int)MOB_TYPES.HUMANOID;
            BerserkTimer = new int[] { 10 * 60, 10 * 60, 10 * 60, 10 * 60 };
            SpeedKillTimer = new int[] { 5 * 60, 5 * 60, 5 * 60, 5 * 60 };
            InBackPerc_Melee = new double[] { 0.95f, 0.95f, 0.95f, 0.95f };
            InBackPerc_Ranged = new double[] { 0.00f, 0.00f, 0.00f, 0.00f };
            Max_Players = new int[] { 10, 25, 10, 25 };
            Min_Tanks = new int[] { 1, 1, 1, 1 };
            Min_Healers = new int[] { 3, 5, 3, 5 };
            #endregion
            #region Offensive
            #region Attacks
            for (int i = 0; i < 4; i++)
            {
                Phase Stage1 = new Phase() { Name = "Stage One: The Final Assault" };
                Phase Stage2 = new Phase() { Name = "Stage Two: The Last Stand" };

                this[i].Attacks.Add(GenAStandardMelee(this[i].Content));

                #region Stage One: The Final Assault
                /* Deathwing assaults his enemies as long as he is attached to the platforms by his twisted limbs.

                   Assault Aspect - Deathwing begins to search for his next enemy to assault. Deathwing will assault the platform with the largest number of players present on it that one of his limbs is still grasping onto.
                   Cataclysm - Deathwing attempts to finish the job he started by bringing forth a second Cataclysm, inflicting 1500001 Fire damage to all enemies.
                   Elementium Meteor - Deathwing creates an Elementium Meteor, sending it to the target platform.
                        Elementium Meteor - If the Elementium Meteor reaches it's destination it will inflict 400000 Fire damage to all enemies the damage decreases the further from the target location.
                   Hemorrhage - Deathwing's tentacle begins to Hemorrhage, causing several Regenerative Blood to spawn at a nearby location.
                        Regenerative Blood - The Regenerative Blood of Deathwing forms. Regenerative Blood's gain ten energy every 1 sec. Regenerative Blood's attacks additionally cause Degenerative Bite.
                            Regnerative - The Regenerative Blood will heal to full health when reaching max energy.
                            Degenerative Bite - Attacks cause Degenerative Bite which inflicts 1500 Shadow damage every 1 sec for 10 sec. Stacks.
                   Limb Tentacle - Deathwing is grasping onto each platform with one of his Limbs, holding him up.
                        Burning Blood - The Burning Blood gushes from the Tentacle, inflicting Fire damage every 2 sec. The damage is increased the lower health the Limb Tentacle is.
                        Blistering Tentacle - Every 20% health the Limb Tentacle sprouts several Blistering Tentacles.
                            Blistering Heat - The Blistering Heat inflicts 4000 Fire damage every 2 sec. Blistering Heat increases damage by 0 every stack. Stacks.
                        Agonizing Pain - The pain from severing Deathwing's limb interrupts his concentration, stunning him and inflicting 20% of his health in damage.
                   Tail Tentacle - Deathwing's Tail Tentacle appears shortly after assaulting a platform.
                        Crush - The crushing weight of the Tail Tentacle inflicts 75000 Physical damage to enemies in a cone in front of the Tail Tentacle.
                        Impale - The Tail Tentacle impales it's target, inflicting 400000 Physical damage split among enemies within 6 yards of the Impale target.
                */
                #endregion

                #region Stage Two: The Last Stand
                /* At 20% health Deathwing falls forward onto the platform.

                   Elementium Fragment - Pieces of Deathwing's armor begin to chip off, forming an Elementium Fragment at nearby locations.
                        Shrapnel - The Fragment fires a piece of Shrapnel at the target, inflicting 200000 Physical damage, piercing through armor.
                   Elementium Terror - Pieces of Deathwing's armor begin to chip off, forming an Elementium Terror at nearby locations.
                        Tetanus - The Elementium Terror's attacks cause Tetnus, inflicting 150000 Physical damage and an additional 30000 Physical damage every 1 sec. Stacks.
                   Corrupted Blood - The Corrupted Blood gushes from Deathwing, inflicting Fire damage every 2 sec. The damage is increased the lower health Deathwing is.
                */
                #region Aspects
                /* The great aspects assist in the fight against Deathwing. Each aspect will channel Expose Weakness on their respective Limb Tentacle when Deathwing begins to cast Cataclysm, increasing the damage done to the Tentacle by 200%. Additionally each aspect will begin to channel Concentration after it's respective tentacle is destroyed, preventing them from assisting the players with their presence and their special powers. When Phase two begins all of the aspects will resume assisting the champions.

                   Alexstrasza
                        Alexstrasza's Presence - The Presence of the great aspect Alexstrasza increases max health by 30%.
                        Cauterize - Alexstrasza begins to Cauterize the Blistering Tentacles, inflicting lethal damage over 10 sec.
                   Nozdormu
                        Nozdormu's Presence - The Presence of the aspect Nozdormu increases Haste by 30%.
                        Time Zone - Nozdormu forms a Time Zone at the target location, causing the Elementium Meteor to lapse in time, decreasing its travel speed dramatically when entering the Time Zone. Additionally any enemy creatures within the Time Zone have their attack speed decreased by 50%.
                   Ysera
                        Ysera's Presence - The Presence of the great aspect Ysera increases healing done by 30%.
                        Dreaming - The Presence of the great aspect Ysera allows players to enter the Dream, decreasing damage taken by 50%.
                   Kalecgos
                        Kalecgos' Presence - The Presence of the great aspect Kalecgos increases damage dealt by 30%.
                        Spell-Weaving - The Presence of the great aspect Kalecgos causes attacks and abilities to cause Spellweaving. Spellweaving inflicts 100000 Arcane damage to enemies within 8 yards, excluding the target.
                   Thrall
                        Carrying Winds - The Carrying Winds will take players to the adjacent platform. The Carrying Winds increase your move speed by 60%. Stacks.
                 */
                #endregion
                #endregion

                #region Add Phases
                int phaseStartTime = 0;
                float phase1length = this[i].BerserkTimer * 0.8f;
                ApplyAPhasesValues(ref Stage1, i, 1, phaseStartTime, phase1length, this[i].BerserkTimer);
                AddAPhase(Stage1, i);
                ApplyAPhasesValues(ref Stage2, i, 1, phaseStartTime, this[i].BerserkTimer - phase1length, this[i].BerserkTimer);
                AddAPhase(Stage2, i);
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
             * all
             */
        }
    }
    #endregion
}
