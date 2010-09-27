/**********
 * Owner: Ebs
 **********/
using System;

namespace Rawr.DPSWarr.Skills
{
    #region Instants
    public class BloodThirst : Ability
    {
        public static new string SName { get { return "Bloodthirst"; } }
        public static new string SDesc { get { return "Instantly attack the target causing [AP*50/100] damage. In addition, the next 3 successful melee attacks will restore 1% health. This effect lasts 8 sec. Damage is based on your attack power."; } }
        public static new string SIcon { get { return "spell_nature_bloodlust"; } }
        /// <summary>
        /// Instantly attack the target causing [AP*50/100] damage. In addition, the next 3 successful melee
        /// attacks will restore 1% health. This effect lasts 8 sec. Damage is based on your attack power.
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
            //Targets += StatS.BonusTargets;
            Cd = 4f; // In Seconds
            //Duration = 8f;
            StanceOkFury = true;
            RageCost = 20f;
#if !RAWR4
            ReqTalent = true;
            Talent2ChksValue = Talents.Bloodthirst;
            RageCost -= (Talents.FocusedRage * 1f);
            DamageBonus = 1f + Talents.UnendingFury * 0.02f;
            BonusCritChance = StatS.BonusWarrior_T8_4P_MSBTCritIncrease;
#else
            BonusCritChance = StatS.BonusWarrior_T8_4P_MSBTCritIncrease + Talents.Cruelty * 0.05f;
#endif
            DamageBase = StatS.AttackPower * 50f / 100f;
            HealingBase = StatS.Health / 100.0f * 3f * (Talents.GlyphOfBloodthirst ? 2f : 1f);
            //HealingBonus = 1f;
            //
            Initialize();
        }
    }
    public class WhirlWind : Ability
    {
        public static new string SName { get { return "Whirlwind"; } }
        public static new string SDesc { get { return "In a whirlwind of steel you attack up to 4 enemies in 8 yards, causing weapon damage from both melee weapons to each enemy."; } }
        public static new string SIcon { get { return "ability_whirlwind"; } }
        /// <summary>
        /// In a whirlwind of steel you attack up to 4 enemies in 8 yards,
        /// causing weapon damage from both melee weapons to each enemy.
        /// <para>Talents: Improved Whirlwind [+(10*Pts)% Damage], Unending Fury [+(2*Pts)% Damage]</para>
        /// <para>Glyphs: Glyph of Whirlwind [-2 sec Cooldown]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public WhirlWind(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Whirlwind_;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            MaxRange = 8f; // In Yards
#if RAWR3 || RAWR4 || SILVERLIGHT
            Targets += (BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0 ? 3f : 0f);
#else
            Targets += (CalcOpts.MultipleTargets ? 3f : 0f);
#endif
#if !RAWR4
            Cd = 10f - (Talents.GlyphOfWhirlwind ? 2f : 0f); // In Seconds
            RageCost = 25f - (Talents.FocusedRage * 1f);
            DamageBonus = (1f + Talents.ImprovedWhirlwind * 0.10f) * (1f + Talents.UnendingFury * 0.02f);
#else
            Cd = 10f; // In Seconds
            RageCost = 25f;// -(Talents.FocusedRage * 1f);
            //DamageBonus = (1f + Talents.ImprovedWhirlwind * 0.10f) * (1f + Talents.UnendingFury * 0.02f);
#endif
            StanceOkFury = true;
            SwingsOffHand = true;
            //
            Initialize();
        }
        // Whirlwind while dual wielding executes two separate attacks; assume no offhand in base case
        public override float DamageOverride { get { return GetDamage(false) + GetDamage(true); } }
        /// <summary></summary>
        /// <param name="Override">When true, do not check for Bers Stance</param>
        /// <param name="isOffHand">When true, do calculations for off-hand damage instead of main-hand</param>
        /// <returns>Unmitigated damage of a single hit</returns>
        private float GetDamage(bool isOffHand) {
            float Damage;
            if (isOffHand) {
                Damage = combatFactors.NormalizedOhWeaponDmg;
            } else {
                Damage = combatFactors.NormalizedMhWeaponDmg;
            }
            return Damage * DamageBonus;
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
        public static new string SDesc { get { return "Your Heroic Strike, Bloodthirst and Whirlwind hits have a (7%/13%/20%) chance of making your next Slam instant for 5 sec."; } }
        public static new string SIcon { get { return "ability_warrior_bloodsurge"; } }
        /// <summary>
        /// Your Heroic Strike, Bloodthirst and Whirlwind hits have a (7%/13%/20%) chance of making your next Slam instant for 5 sec.
        /// <para>Talents: Bloodsurge (Requires Talent) [(7%/13%/20%) chance]</para>
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
            //Targets += StatS.BonusTargets;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
#if !RAWR4
            Duration = 5f; // In Seconds
            RageCost = 15f - (Talents.FocusedRage * 1f);
#else
            Duration = 10f; // In Seconds
            RageCost = 15f;// -(Talents.FocusedRage * 1f);
#endif
            StanceOkFury = true;
#if !RAWR4
            hsActivates = 0.0f;
#endif
            SL = slam;
            WW = whirlwind;
            BT = bloodthirst;
            UseReact = true;
            //
            Initialize();
        }
        #region Variables
#if !RAWR4
        public float hsActivates;
#endif
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
            bool is4T10 = (StatS.BonusWarrior_T10_4P_BSSDProcChange != 0f);

            float bonusProcChance = 0f;
            
            // We can squeeze in a single slam from 7-8 if we get a proc
            if (is4T10)
            {
                bonusProcChance = 1f -
                                  (1f - actMod * BT.Activates / 2f * procChance * BT.MHAtkTable.AnyLand * 0.2f) * 
                                  (1f - actMod * hsActivates * 3f / 8f * procChance * MHAtkTable.AnyLand * 0.2f); // 1proc left from this
            }

            // Only WW
            chanceWeDontProc *= (1f - actMod * WW.Activates * procChance * WW.MHAtkTable.AnyLand) *
                                (1f - actMod * WW.Activates * procChance * WW.OHAtkTable.AnyLand);
            // Second BT
            chanceWeDontProc *= (1f - actMod * BT.Activates / 2f * procChance * BT.MHAtkTable.AnyLand);
            // Other 5/8s of the HSes
            chanceWeDontProc *= (1f - actMod * hsActivates * 5f / 8f * procChance * MHAtkTable.AnyLand);

            // chanceWeDontProc% of the time, we don't proc. But bonusProcChance% of the time, we can use the leftover here
            float numProcs = (1f + chanceWeDontProc) * bonusProcChance;
            numProcs += (1f - chanceWeDontProc) * (is4T10 ? 1.2f : 1f);
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
#if RAWR4
                float chance = Talents.Bloodsurge * 0.10f;
#else
                float chance = Talents.Bloodsurge * 0.20f / 3f;
#endif
                float chanceMhHitLands = (1f - MHAtkTable.Miss - MHAtkTable.Dodge);
                float chanceOhHitLands = (1f - OHAtkTable.Miss - OHAtkTable.Dodge);

#if !RAWR4
                float procs3 = BasicFuryRotation(chanceMhHitLands, chanceOhHitLands, hsActivates, chance);
#else
                float procs3 = BasicFuryRotation(chanceMhHitLands, chanceOhHitLands, 0, chance);
#endif

                procs3 = (maintainActs > procs3) ? 0f : procs3 - maintainActs;

                //return procs3; // *(1f - Whiteattacks.AvoidanceStreak); // Not using AvoidanceStreak, as it's already accounted by the other ability's activates
                return procs3 * (1f - Whiteattacks.RageSlip(FightDuration / procs3, RageCost));
            }
        }
        public override float DamageOverride { get { return SL.DamageOverride; } }
        #endregion
    }
#if RAWR4
    public class RagingBlow : Ability
    {
        public static new string SName { get { return "Raging Blow"; } }
        public static new string SDesc { get { return "A mighty blow that deals 100% weapon damage from both melee weapons. Can only be used while Enraged."; } }
        public static new string SIcon { get { return "ability_hunter_swiftstrike"; } }
        /// <summary>
        /// A mighty blow that deals 100% weapon damage from both melee weapons. Can only be used while Enraged.
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
            //AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Whirlwind_;
            ReqMeleeWeap = ReqMeleeRange = true;
            Cd = 6f; // In Seconds
            RageCost = 20f;
            StanceOkFury = true;
            SwingsOffHand = true;
            BonusCritChance = Talents.GlyphOfRagingBlow ? 0.05f : 0f;
            //
            Initialize();
        }
        // Whirlwind while dual wielding executes two separate attacks; assume no offhand in base case
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
                return Damage * AvgTargets;
            }
        }
    }
#endif
    #endregion
    #region OnAttacks
    public class HeroicStrike :
#if !RAWR4
        OnAttack
#else
        Ability
#endif
    {
        public static new string SName { get { return "Heroic Strike"; } }
        public static new string SDesc { get { return "A strong attack that increases melee damage by 495 and causes a high amount of threat. Causes 173.25 additional damage against Dazed targets."; } }
        public static new string SIcon { get { return "ability_rogue_ambush"; } }
        /// <summary>
        /// A strong attack that increases melee damage by 495 and causes a high amount of
        /// threat. Causes 173.25 additional damage against Dazed targets.
        /// <para>Talents: Improved Heroic Strike [-(1*Pts) rage cost], Incite [+(5*Pts)% crit chance]</para>
        /// <para>Glyphs: Glyph of Heroic Strike [+10 rage on crits]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public HeroicStrike(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
#if !RAWR4
            Cd = /*0f*/(Char.MainHand != null ? Whiteattacks.MhEffectiveSpeed : 0f); // In Seconds
            RageCost = 15f - (Talents.ImprovedHeroicStrike * 1f) - (Talents.FocusedRage * 1f);
#else
            Cd = 3f; // In Seconds
            RageCost = 30f;
#endif
            CastTime = 0f; // In Seconds // Replaces a white hit
            GCDTime = 0f;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = Whiteattacks.MhDamage + 495f;
#if RAWR4
            // 8 + AP*0.6 Base Damage in Cata?
            DamageBonus = Talents.WarAcademy * 0.05f;
#endif
            BonusCritChance = Talents.Incite * 0.05f + StatS.BonusWarrior_T9_4P_SLHSCritIncrease;
            //
            Initialize();
        }
        private SpecialEffect[] _SE_Incite = new SpecialEffect[] {
            null,
            new SpecialEffect(Trigger.Use, null, 0, 6, 1f / 3f * 1f),
            new SpecialEffect(Trigger.Use, null, 0, 6, 1f / 3f * 2f),
            new SpecialEffect(Trigger.Use, null, 0, 6, 1f / 3f * 3f),
        };
        public float storedInciteBonusCrits = 0f;
        public float storedActs = 0f;
        public float InciteBonusCrits(float acts) {
            if (Talents.Incite == 0) { storedActs = 0f; storedInciteBonusCrits = 0f; return 0f; }
            storedActs = acts;
            storedInciteBonusCrits = _SE_Incite[Talents.Incite].GetAverageProcsPerSecond(acts, MHAtkTable.Crit, combatFactors.MHSpeed, CalcOpts.SE_UseDur ? FightDuration : 0f) * FightDuration;
            return storedInciteBonusCrits;
        }
        public override float DamageOnUseOverride
        {
            get
            {
                float dmg = Damage; // Base Damage
                dmg *= combatFactors.DamageBonus; // Global Damage Bonuses
                dmg *= combatFactors.DamageReduction; // Global Damage Penalties

#if RAWR4
                float bonusForcedCritsPerc = Talents.Incite > 0 ? storedInciteBonusCrits / storedActs : 0;
#else
                float bonusForcedCritsPerc = 0f;
#endif

                // Work the Attack Table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    - MHAtkTable.Glance // glancing handled below
                    - MHAtkTable.Block  // blocked handled below
                    - (MHAtkTable.Crit + bonusForcedCritsPerc)); // crits   handled below

                float dmgGlance = dmg * MHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                float dmgBlock = dmg * MHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                float dmgCrit = dmg * (MHAtkTable.Crit + bonusForcedCritsPerc) * (1f + combatFactors.BonusYellowCritDmg) * BonusCritDamage;//Bonus   Damage when critting

                dmg *= dmgDrop;

                dmg += /*dmgGlance +*/ dmgBlock + dmgCrit;

                return dmg;
            }
        }
#if !RAWR4
        public override float FullRageCost
        {
            get
            {
                //float glyphback = (Talents.GlyphOfHeroicStrike ? 10.0f * ContainCritValue(true) : 0f);
                return base.FullRageCost - (Talents.GlyphOfHeroicStrike ? 10.0f * MHAtkTable.Crit : 0f);
            }
        }
#endif
    }
    public class Cleave : 
#if !RAWR4
        OnAttack
#else
        Ability
#endif
    {
        public static new string SName { get { return "Cleave"; } }
        public static new string SDesc { get { return "A sweeping attack that does your weapon damage plus 222 to the target and his nearest ally."; } }
        public static new string SIcon { get { return "ability_warrior_cleave"; } }
        /// <summary>
        /// A sweeping attack that does your weapon damage plus 222 to the target and his nearest ally.
        /// <para>Talents: Improved Cleave [+(40*Pts)% Damage], Incite [+(5*Pts)% Crit Perc]</para>
        /// <para>Glyphs: Glyph of Cleaving [+1 targets hit]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Cleave(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Cleave_;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
#if !RAWR4
            RageCost = 20f - (Talents.FocusedRage * 1f);
#else
            RageCost = 20f;// -(Talents.FocusedRage * 1f);
#endif
#if RAWR3 || RAWR4 || SILVERLIGHT
            Targets += (BossOpts.MultiTargs && BossOpts.Targets != null && BossOpts.Targets.Count > 0 ? 1f + (Talents.GlyphOfCleaving ? 1f : 0f) : 0f);
#else
            Targets += (CalcOpts.MultipleTargets ? 1f + (Talents.GlyphOfCleaving ? 1f : 0f) : 0f);
#endif
            CastTime = 0f; // In Seconds // Replaces a white hit
            GCDTime = 0f;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
#if !RAWR4
            DamageBase = Whiteattacks.MhDamage + (222f * (1f + Talents.ImprovedCleave * 0.40f));
            BonusCritChance = Talents.Incite * 0.05f;
#else
            DamageBase = Whiteattacks.MhDamage + (222f/* * (1f + Talents.ImprovedCleave * 0.40f)*/);
            DamageBonus = Talents.WarAcademy * 0.05f;
#endif
            //DamageBonus = 1f + Talents.ImprovedCleave * 0.40f; // Imp Cleave is only the "Bonus Damage", and not the whole attack
            //
            Initialize();
        }
    }
    #endregion
    #region Unused
    public class Pummel : Ability
    {
        /// <summary>
        /// Instant, 10 sec Cd, 10 Rage, Melee Range, (Zerker)
        /// Pummel the target, interupting spellcasting and preventing any spell in that school
        /// from being cast for 4 sec.
        /// </summary>
        /// <para>Talents: </para>
        /// <para>Glyphs: </para>
        ///  - (Talents.FocusedRage * 1f)
        ///  RageCost = RageCost * (1f - Talents.DrumsOfWar * 0.50f); // Drums of War negates rage cost
        public static new string Icon { get { return "inv_gauntlets_04"; } }
    }
    #endregion
}