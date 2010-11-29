/**********
 * Owner: Ebs (Though he left)
 **********/
using System;

namespace Rawr.DPSWarr.Skills
{
    #region Instants
    public class BloodThirst : Ability
    {
        public static new string SName { get { return "Bloodthirst"; } }
        public static new string SDesc { get { return "Instantly attack the target causing [AP*62/100] damage. In addition, the next 3 successful melee attacks will restore 1% health. This effect lasts 8 sec. Damage is based on your attack power."; } }
        public static new string SIcon { get { return "spell_nature_bloodlust"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 23881; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Instantly attack the target causing [AP*62/100] damage. In addition, the next 3 successful melee
        /// attacks will restore 0.5% health. This effect lasts 8 sec. Damage is based on your attack power.
        /// <para>Talents: Bloodthirst (Requires talent), Unending Fury [+(2*Pts)% Damage]</para>
        /// <para>Glyphs: Glyph of Bloodthirst [+100% from healing effect]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public BloodThirst(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Bloodthirst_;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = true;
            Cd = 3f; // In Seconds
            //Duration = 8f;
            RageCost        = 20f;
            BonusCritChance = Talents.Cruelty * 0.05f;
            DamageBase      = StatS.AttackPower * 62f / 100f;
            DamageBonus     = (1f + (Talents.GlyphOfBloodthirst ? 0.10f : 0f))
                            * (1f + StatS.BonusWarrior_T11_2P_BTMSDmgMult);
            HealingBase     = StatS.Health * 0.005f * 3f;
            HealingBonus    = 1f + (Talents.GlyphOfBloodyHealing ? 1f : 0f);
            //
            Initialize();
        }
    }
    public class WhirlWind : Ability
    {
        public static new string SName { get { return "Whirlwind"; } }
        public static new string SDesc { get { return "In a whirlwind of steel you attack all enemies within 8 yards, causing "+DamageMultiplier.ToString("0%")+" weapon damage from both melee weapons to each enemy."; } }
        public static new string SIcon { get { return "ability_whirlwind"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 1680; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// In a whirlwind of steel you attack all enemies within 8 yards,
        /// causing 65% weapon damage from both melee weapons to each enemy.
        /// <para>Talents: Improved Whirlwind [+(10*Pts)% Damage], Unending Fury [+(2*Pts)% Damage]</para>
        /// <para>Glyphs: Glyph of Whirlwind [-2 sec Cooldown]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public WhirlWind(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Whirlwind_;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = true;
            SwingsOffHand = true;
            MaxRange = 8f; // In Yards
            Targets = 10f;
            Cd = 10f; // In Seconds
            RageCost = 25f;
            DamageBase = (combatFactors.NormalizedMhWeaponDmg + combatFactors.NormalizedOhWeaponDmg) * DamageMultiplier;
            DamageBonus = 1f + StatS.BonusCleaveWWDamageMultiplier;
            //
            Initialize();
        }
        public static readonly float DamageMultiplier = 0.65f;
        // Whirlwind while dual wielding executes two separate attacks; assume no offhand in base case
        public override float DamageOverride { get { return GetDamage(false) + GetDamage(true); } }
        /// <summary></summary>
        /// <param name="Override">When true, do not check for Bers Stance</param>
        /// <param name="isOffHand">When true, do calculations for off-hand damage instead of main-hand</param>
        /// <returns>Unmitigated damage of a single hit</returns>
        private float GetDamage(bool isOffHand) {
            return (isOffHand ? combatFactors.NormalizedOhWeaponDmg : combatFactors.NormalizedMhWeaponDmg)
                * DamageMultiplier * DamageBonus;
        }
        public override float DamageOnUseOverride
        {
            get
            {
                // ==== MAIN HAND ====
                float DamageMH = GetDamage(false); // Base Damage
                DamageMH *= combatFactors.DamageBonus; // Global Damage Bonuses
                DamageMH *= combatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    - MHAtkTable.Glance // glancing handled below
                    - MHAtkTable.Block  // blocked handled below
                    - MHAtkTable.Crit); // crits   handled below

                float dmgGlance = DamageMH * MHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                float dmgBlock = DamageMH * MHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                float dmgCrit = DamageMH * MHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                DamageMH *= dmgDrop;

                DamageMH += dmgGlance + dmgBlock + dmgCrit;

                // ==== OFF HAND ====
                float DamageOH = GetDamage(true); // Base Damage
                DamageOH *= combatFactors.DamageBonus; // Global Damage Bonuses
                DamageOH *= combatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                dmgDrop = (1f
                    - OHAtkTable.Miss   // no damage when being missed
                    - OHAtkTable.Dodge  // no damage when being dodged
                    - OHAtkTable.Parry  // no damage when being parried
                    - OHAtkTable.Glance // glancing handled below
                    - OHAtkTable.Block  // blocked handled below
                    - OHAtkTable.Crit); // crits   handled below

                dmgGlance = DamageOH * OHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                dmgBlock = DamageOH * OHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                dmgCrit = DamageOH * OHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                DamageOH *= dmgDrop;

                DamageOH += dmgGlance + dmgBlock + dmgCrit;

                // ==== RESULT ====
                float Damage = DamageMH + DamageOH;
                return Damage * AvgTargets;
            }
        }
    }
    public class BloodSurge : Ability
    {
        public static new string SName { get { return "Bloodsurge"; } }
        public static new string SDesc { get { return "Your Heroic Strike, Bloodthirst and Whirlwind hits have a (10*Pts)% chance of making your next Slam instant for 5 sec."; } }
        public static new string SIcon { get { return "ability_warrior_bloodsurge"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 46915; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Your Heroic Strike, Bloodthirst and Whirlwind hits have a (10*Pts)% chance of making your next Slam instant for 5 sec.
        /// <para>Talents: Bloodsurge (Requires Talent) [(10*Pts)% chance]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public BloodSurge(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo,
            Ability slam, Ability whirlwind, Ability bloodthirst)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Bloodsurge_;
            ReqTalent = true;
            Talent2ChksValue = Talents.Bloodsurge;
            ReqMeleeWeap = ReqMeleeRange = true;
            Duration = 10f; // In Seconds
            RageCost = 15f;
            StanceOkFury = true;
            SL = slam;
            WW = whirlwind;
            BT = bloodthirst;
            UseReact = true;
            //
            Initialize();
        }
        #region Variables
        public float maintainActs;
        public Ability SL;
        public Ability WW;
        public Ability BT;
        #endregion
        #region Functions
        private float BasicFuryRotation(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance)
        {
            // Assumes one slot to slam every 8 seconds: WW/BT/Slam/BT repeat. Not optimal, but easy to do
            float chanceWeDontProc = 1f;
            float actMod = 8f / FightDuration; // since we're assuming an 8sec rotation

            float bonusProcChance = 0f;
            
            // Only WW
            chanceWeDontProc *= (1f - actMod * WW.Activates * procChance * WW.MHAtkTable.AnyLand) *
                                (1f - actMod * WW.Activates * procChance * WW.OHAtkTable.AnyLand);
            // Second BT
            chanceWeDontProc *= (1f - actMod * BT.Activates / 2f * procChance * BT.MHAtkTable.AnyLand);
            // Other 5/8s of the HSes
            chanceWeDontProc *= (1f - actMod * hsActivates * 5f / 8f * procChance * MHAtkTable.AnyLand);

            // chanceWeDontProc% of the time, we don't proc. But bonusProcChance% of the time, we can use the leftover here
            float numProcs = (1f + chanceWeDontProc) * bonusProcChance;
            numProcs += (1f - chanceWeDontProc);
            return (numProcs / actMod);

        }
        private float CalcSlamProcs(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance)
        {
            return 0f;
        }
        protected override float ActivatesOverride
        {
            get
            {
                float chance = Talents.Bloodsurge * 0.10f;
                float chanceMhHitLands = (1f - MHAtkTable.Miss - MHAtkTable.Dodge);
                float chanceOhHitLands = (1f - OHAtkTable.Miss - OHAtkTable.Dodge);

                float procs3 = BasicFuryRotation(chanceMhHitLands, chanceOhHitLands, 0, chance);

                procs3 = (maintainActs > procs3) ? 0f : procs3 - maintainActs;

                //return procs3; // *(1f - Whiteattacks.AvoidanceStreak); // Not using AvoidanceStreak, as it's already accounted by the other ability's activates
                return procs3 * (1f - Whiteattacks.RageSlip(FightDuration / procs3, RageCost));
            }
        }
        public override float DamageOverride { get { return SL.DamageOverride; } }
        #endregion
    }
    public class RagingBlow : Ability
    {
        public static new string SName { get { return "Raging Blow"; } }
        public static new string SDesc { get { return "A mighty blow that deals " + DamageMultiplier.ToString("0%") + " weapon damage from both melee weapons. Can only be used while Enraged."; } }
        public static new string SIcon { get { return "ability_hunter_swiftstrike"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 85288; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// A mighty blow that deals 110% weapon damage from both melee weapons. Can only be used while Enraged.
        /// <para>Talents: none</para>
        /// <para>Glyphs: RagingBlow [+5% Crit Chance]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public RagingBlow(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            ReqTalent = true;
            Talent2ChksValue = Talents.RagingBlow;
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.RagingBlow_;
            ReqMeleeWeap = ReqMeleeRange = true;
            Cd = 6f; // In Seconds
            RageCost = 20f;
            StanceOkFury = true;
            SwingsOffHand = true;
            BonusCritChance = Talents.GlyphOfRagingBlow ? 0.05f : 0f;
            //
            Initialize();
        }
        public static readonly float DamageMultiplier = 1.10f;
        // Raging Blow while dual wielding executes two separate attacks; assume no offhand in base case
        public override float DamageOverride { get { return GetDamage(false) + GetDamage(true); } }
        /// <summary></summary>
        /// <param name="Override">When true, do not check for Bers Stance</param>
        /// <param name="isOffHand">When true, do calculations for off-hand damage instead of main-hand</param>
        /// <returns>Unmitigated damage of a single hit</returns>
        private float GetDamage(bool isOffHand) { return (isOffHand ? combatFactors.NormalizedOhWeaponDmg : combatFactors.NormalizedMhWeaponDmg) * DamageBonus; }
        public override float DamageOnUseOverride
        {
            get
            {
                // ==== MAIN HAND ====
                float DamageMH = GetDamage(false); // Base Damage
                DamageMH *= combatFactors.DamageBonus; // Global Damage Bonuses
                DamageMH *= combatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    - MHAtkTable.Glance // glancing handled below
                    - MHAtkTable.Block  // blocked handled below
                    - MHAtkTable.Crit); // crits   handled below

                float dmgGlance = DamageMH * MHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                float dmgBlock = DamageMH * MHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                float dmgCrit = DamageMH * MHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                DamageMH *= dmgDrop;

                DamageMH += dmgGlance + dmgBlock + dmgCrit;

                // ==== OFF HAND ====
                float DamageOH = GetDamage(true); // Base Damage
                DamageOH *= combatFactors.DamageBonus; // Global Damage Bonuses
                DamageOH *= combatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                dmgDrop = (1f
                    - OHAtkTable.Miss   // no damage when being missed
                    - OHAtkTable.Dodge  // no damage when being dodged
                    - OHAtkTable.Parry  // no damage when being parried
                    - OHAtkTable.Glance // glancing handled below
                    - OHAtkTable.Block  // blocked handled below
                    - OHAtkTable.Crit); // crits   handled below

                dmgGlance = DamageOH * OHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                dmgBlock = DamageOH * OHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                dmgCrit = DamageOH * OHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                DamageOH *= dmgDrop;

                DamageOH += dmgGlance + dmgBlock + dmgCrit;

                // ==== RESULT ====
                float Damage = DamageMH + DamageOH;
                return Damage * AvgTargets * DamageMultiplier;
            }
        }
    }
    #endregion
    #region Rage Dumps (used to be OnAttacks)
    public class HeroicStrike : Ability
    {
        public static new string SName { get { return "Heroic Strike"; } }
        public static new string SDesc { get { return "An attack that instantly deals (8+AP*0.75) physical damage. A good attack for moments of excess rage."; } }
        public static new string SIcon { get { return "ability_rogue_ambush"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 78; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// An attack that instantly deals (8+AP*0.75) physical damage. A good attack for moments of excess rage.
        /// <para>Talents: Improved Heroic Strike [-(1*Pts) rage cost], Incite [+(5*Pts)% crit chance]</para>
        /// <para>Glyphs: Glyph of Heroic Strike [+10 rage on crits]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public HeroicStrike(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            Cd = 3f; // In Seconds
            RageCost = 30f;
            DamageBase = 8f + StatS.AttackPower * 0.75f;
            DamageBonus = 1f + Talents.WarAcademy * 0.05f;
            BonusCritChance = Talents.Incite * 0.05f;
            //
            Initialize();
        }
        private SpecialEffect[] _SE_Incite = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.HSorSLHit, null, 0, 6, 1f / 3f * 1f), // actual trigger is HS Crit but no need to make one
            new SpecialEffect(Trigger.HSorSLHit, null, 0, 6, 1f / 3f * 2f),
            new SpecialEffect(Trigger.HSorSLHit, null, 0, 6, 1f / 3f * 3f),
        };
        private float storedInciteBonusCrits = 0f;
        private float storedActs = 0f;
        public float InciteBonusCrits(float acts) {
            if (Talents.Incite == 0) { storedActs = 0f; storedInciteBonusCrits = 0f; return 0f; }
            storedActs = acts;
            // Factor that we don't HS in Exec phase
            float fightDur = (!combatFactors.FuryStance && CalcOpts.M_ExecuteSpam) ? FightDurationO20 : FightDuration;
            storedInciteBonusCrits = _SE_Incite[Talents.Incite].GetAverageProcsPerSecond(fightDur / acts, MHAtkTable.Crit, combatFactors.MHSpeed, fightDur) * fightDur;
            return storedInciteBonusCrits;
        }
        public override float DamageOnUseOverride {
            get {
                float dmg = Damage; // Base Damage
                dmg *= combatFactors.DamageBonus; // Global Damage Bonuses
                dmg *= combatFactors.DamageReduction; // Global Damage Penalties

                float bonusForcedCritsPerc = (Talents.Incite > 0 && storedInciteBonusCrits > 0f && storedActs > 0f)
                                             ? storedInciteBonusCrits / storedActs : 0;

                // Work the Attack Table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    - MHAtkTable.Block  // blocked handled below
                    - (MHAtkTable.Crit + bonusForcedCritsPerc)); // crits   handled below

                float dmgBlock = dmg * MHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                float dmgCrit = dmg * (MHAtkTable.Crit + bonusForcedCritsPerc) * (1f + combatFactors.BonusYellowCritDmg) * BonusCritDamage;//Bonus Damage when critting

                dmg *= dmgDrop;

                dmg += dmgBlock + dmgCrit;

                return dmg * AvgTargets;
            }
        }
    }
    public class Cleave : Ability
    {
        public static new string SName { get { return "Cleave"; } }
        public static new string SDesc { get { return "A sweeping attack that strikes the target and a nearby ally, dealing (6 + AP * 0.562) physical damage."; } }
        public static new string SIcon { get { return "ability_warrior_cleave"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 845; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// A sweeping attack that strikes the target and a nearby ally, dealing (6 + $ap * 0.45) physical damage.
        /// <para>Talents: Improved Cleave [+(40*Pts)% Damage], Incite [+(5*Pts)% Crit Perc]</para>
        /// <para>Glyphs: Glyph of Cleaving [+1 targets hit]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Cleave(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            Cd = 3f; // In Seconds
            RageCost = 30f;
            Targets = 2f + (Talents.GlyphOfCleaving ? 1f : 0f);
            DamageBase = 6f + StatS.AttackPower * 0.562f;
            DamageBonus = 1f + Talents.WarAcademy * 0.05f + StatS.BonusCleaveWWDamageMultiplier;
            //
            Initialize();
        }
    }
    #endregion
    #region Unused
    public class Pummel : Ability
    {
        public static new string SName { get { return "Pummel"; } }
        public static new string SDesc { get { return "This is not a built class in Rawr4"; } }
        public static new string SIcon { get { return "inv_gauntlets_04"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Instant, 10 sec Cd, 10 Rage, Melee Range, (Battle, Berserker)
        /// Pummel the target, interrupting spell-casting and preventing any spell in that school
        /// from being cast for 4 sec.
        /// <para>Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// <para>TODO: Damage Increase proc from Rude Interruption Talent</para>
        /// </summary>
        public Pummel(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            //AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Pummel_;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkArms = StanceOkFury = true;
            Cd = 10f; // In Seconds
            Duration = 4f;
            RageCost = DrumsOfWarRageCosts[Talents.DrumsOfWar];
            DamageBase = 0;
            //
            Initialize();
        }
        public static readonly float[] DrumsOfWarRageCosts = new float[] { 10, 5, -1 };
    }
    #endregion
}
