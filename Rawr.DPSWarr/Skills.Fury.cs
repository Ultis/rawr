/**********
 * Owner: Ebs
 **********/
using System;

namespace Rawr.DPSWarr.Skills
{
    #region Instants
    public class BloodThirst : Ability
    {
        // Constructors
        /// <summary>
        /// Instantly attack the target causing [AP*50/100] damage. In addition, the next 3 successful melee
        /// attacks will restore 1% health. This effect lasts 8 sec. Damage is based on your attack power.
        /// </summary>
        /// <TalentsAffecting>Bloodthirst (Requires talent), Unending Fury [+(2*Pts)% Damage]</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Bloodthirst [+100% from healing effect]</GlyphsAffecting>
        public BloodThirst(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            Name = "Bloodthirst";
            Description = "Instantly attack the target causing [AP*50/100] damage. In addition, the next 3 successful melee attacks will restore 1% health. This effect lasts 8 sec. Damage is based on your attack power.";
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
        /// <summary>
        /// In a whirlwind of steel you attack up to 4 enemies in 8 yards,
        /// causing weapon damage from both melee weapons to each enemy.
        /// </summary>
        /// <TalentsAffecting>Improved Whirlwind [+(10*Pts)% Damage], Unending Fury [+(2*Pts)% Damage]</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Whirlwind [-2 sec Cooldown]</GlyphsAffecting>
        public WhirlWind(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            Name = "Whirlwind";
            Description = "In a whirlwind of steel you attack up to 4 enemies in 8 yards, causing weapon damage from both melee weapons to each enemy.";
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
        // Constructors
        /// <summary>
        /// Your Heroic Strike, Bloodthirst and Whirlwind hits have a (7%/13%/20%)
        /// chance of making your next Slam instant for 5 sec.
        /// </summary>
        /// <TalentsAffecting>Bloodsurge (Requires Talent) [(7%/13%/20%) chance]</TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        public BloodSurge(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo,
            Ability slam, Ability whirlwind, Ability bloodthirst)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            Name = "Bloodsurge";
            Description = "Your Heroic Strike, Bloodthirst and Whirlwind hits have a (7%/13%/20%) chance of making your next Slam instant for 5 sec.";
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
        /// <summary>
        /// A mighty blow that deals 100% weapon damage from both melee weapons. Can only be used while Enraged.
        /// </summary>
        /// <TalentsAffecting></TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        public RagingBlow(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            Name = "Raging Blow";
            Description = "A mighty blow that deals 100% weapon damage from both melee weapons. Can only be used while Enraged.";
            ReqTalent = true;
            Talent2ChksValue = Talents.RagingBlow;
            //AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Whirlwind_;
            ReqMeleeWeap = ReqMeleeRange = true;
            Cd = 6f; // In Seconds
            RageCost = 20f;
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
        /// <summary>
        /// A strong attack that increases melee damage by 495 and causes a high amount of
        /// threat. Causes 173.25 additional damage against Dazed targets.
        /// </summary>
        /// <TalentsAffecting>Improved Heroic Strike [-(1*Pts) rage cost], Incite [+(5*Pts)% crit chance]</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Heroic Strike [+10 rage on crits]</GlyphsAffecting>
        public HeroicStrike(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            Name = "Heroic Strike";
            Description = "A strong attack that increases melee damage by 495 and causes a high amount of threat. Causes 173.25 additional damage against Dazed targets.";
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
        /// <summary>
        /// A sweeping attack that does your weapon damage plus 222 to the target and his nearest ally.
        /// </summary>
        /// <TalentsAffecting>Improved Cleave [+(40*Pts)% Damage], Incite [+(5*Pts)% Crit Perc]</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Cleaving [+1 targets hit]</GlyphsAffecting>
        public Cleave(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            Name = "Cleave";
            Description = "A sweeping attack that does your weapon damage plus 222 to the target and his nearest ally.";
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
#else
            DamageBase = Whiteattacks.MhDamage + (222f/* * (1f + Talents.ImprovedCleave * 0.40f)*/);
            DamageBonus = Talents.WarAcademy * 0.05f;
#endif
            //DamageBonus = 1f + Talents.ImprovedCleave * 0.40f; // Imp Cleave is only the "Bonus Damage", and not the whole attack
            BonusCritChance = Talents.Incite * 0.05f;
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
        /// <TalentsAffecting></TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        ///  - (Talents.FocusedRage * 1f)
        ///  RageCost = RageCost * (1f - Talents.DrumsOfWar * 0.50f); // Drums of War negates rage cost
    }
    #endregion
}