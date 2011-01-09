/**********
 * Owner: Ebs (Though he left)
 **********/
using System;

namespace Rawr.DPSWarr.Skills
{
    #region Instants
    public sealed class Bloodthirst : Ability
    {
        public static new string SName { get { return "Bloodthirst"; } }
        public static new string SDesc { get { return "Instantly attack the target causing [AP*0.62] damage. In addition, the next 3 successful melee attacks will restore 1% health. This effect lasts 8 sec. Damage is based on your attack power."; } }
        public static new string SIcon { get { return "spell_nature_bloodlust"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 23881; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instantly attack the target causing [AP*62/100] damage. In addition, the next 3 successful melee
        /// attacks will restore 0.5% health. This effect lasts 8 sec. Damage is based on your attack power.
        /// <para>DPSWarrChar.Talents: Bloodthirst (Requires talent), Unending Fury [+(2*Pts)% Damage]</para>
        /// <para>Glyphs: Glyph of Bloodthirst [+100% from healing effect]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Bloodthirst(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.Bloodthirst;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = true;
            CD = 3f; // In Seconds
            //Duration = 8f;
            RageCost        = 20f;
            BonusCritChance = DPSWarrChar.Talents.Cruelty * 0.05f;
            DamageBase      = DPSWarrChar.StatS.AttackPower * 0.62f;
            DamageBonus     = (1f + (DPSWarrChar.Talents.GlyphOfBloodthirst ? 0.10f : 0f))
                            * (1f + DPSWarrChar.StatS.BonusBloodthirstDamageMultiplier);
            HealingBase     = DPSWarrChar.StatS.Health * 0.005f * 3f;
            HealingBonus    = 1f + (DPSWarrChar.Talents.GlyphOfBloodyHealing ? 1f : 0f);
            //
            Initialize();
        }
    }
    public sealed class Whirlwind : Ability
    {
        public static new string SName { get { return "Whirlwind"; } }
        public static new string SDesc { get { return string.Format("In a whirlwind of steel you attack all enemies within 8 yards, causing {0:0%} weapon damage from both melee weapons to each enemy.", DamageMultiplier); } }
        public static new string SIcon { get { return "ability_whirlwind"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 1680; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// In a whirlwind of steel you attack all enemies within 8 yards,
        /// causing 65% weapon damage from both melee weapons to each enemy.
        /// <para>DPSWarrChar.Talents: Improved Whirlwind [+(10*Pts)% Damage], Unending Fury [+(2*Pts)% Damage]</para>
        /// <para>Glyphs: Glyph of Whirlwind [-2 sec Cooldown]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Whirlwind(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.Whirlwind;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = true;
            SwingsOffHand = true;
            MaxRange = 8f; // In Yards
            Targets = 10f;
            CD = 10f; // In Seconds
            RageCost = 25f;
            DamageBase = (DPSWarrChar.CombatFactors.NormalizedMHWeaponDmg + DPSWarrChar.CombatFactors.NormalizedOHWeaponDmg) * DamageMultiplier;
            DamageBonus = 1f + DPSWarrChar.StatS.BonusWhirlwindDamageMultiplier;
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
            return (isOffHand ? DPSWarrChar.CombatFactors.NormalizedOHWeaponDmg : DPSWarrChar.CombatFactors.NormalizedMHWeaponDmg)
                * DamageMultiplier * DamageBonus;
        }
        public override float DamageOnUseOverride
        {
            get
            {
                // ==== MAIN HAND ====
                float DamageMH = GetDamage(false); // Base Damage
                DamageMH *= DPSWarrChar.CombatFactors.DamageBonus; // Global Damage Bonuses
                DamageMH *= DPSWarrChar.CombatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    - MHAtkTable.Glance // glancing handled below
                    - MHAtkTable.Block  // blocked handled below
                    - MHAtkTable.Crit); // crits   handled below

                float dmgGlance = DamageMH * MHAtkTable.Glance * CombatFactors.ReducWHGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                float dmgBlock = DamageMH * MHAtkTable.Block * CombatFactors.ReducYWBlockedDmg;//Partial damage when blocked
                float dmgCrit = DamageMH * MHAtkTable.Crit * (1f + DPSWarrChar.CombatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                DamageMH *= dmgDrop;

                DamageMH += dmgGlance + dmgBlock + dmgCrit;

                // ==== OFF HAND ====
                float DamageOH = GetDamage(true); // Base Damage
                DamageOH *= DPSWarrChar.CombatFactors.DamageBonus; // Global Damage Bonuses
                DamageOH *= DPSWarrChar.CombatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                dmgDrop = (1f
                    - OHAtkTable.Miss   // no damage when being missed
                    - OHAtkTable.Dodge  // no damage when being dodged
                    - OHAtkTable.Parry  // no damage when being parried
                    - OHAtkTable.Glance // glancing handled below
                    - OHAtkTable.Block  // blocked handled below
                    - OHAtkTable.Crit); // crits   handled below

                dmgGlance = DamageOH * OHAtkTable.Glance * CombatFactors.ReducWHGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                dmgBlock = DamageOH * OHAtkTable.Block * CombatFactors.ReducYWBlockedDmg;//Partial damage when blocked
                dmgCrit = DamageOH * OHAtkTable.Crit * (1f + DPSWarrChar.CombatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                DamageOH *= dmgDrop;

                DamageOH += dmgGlance + dmgBlock + dmgCrit;

                // ==== RESULT ====
                float Damage = DamageMH + DamageOH;
                return Damage * AvgTargets;
            }
        }
    }
    public sealed class BloodSurge : Ability
    {
        public static new string SName { get { return "Bloodsurge"; } }
        public static new string SDesc { get { return "Your Heroic Strike, Bloodthirst and Whirlwind hits have a (10*Pts)% chance of making your next Slam instant for 5 sec."; } }
        public static new string SIcon { get { return "ability_warrior_bloodsurge"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 46915; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Your Bloodthirst hits have a 30% chance of making your next Slam instant and free for 10 sec.
        /// <para>DPSWarrChar.Talents: Bloodsurge (Requires Talent) [(10*Pts)% chance]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public BloodSurge(DPSWarrCharacter dpswarrchar, //Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo,
            Ability slam/*, Ability whirlwind*/, Ability bloodthirst)
        {
            DPSWarrChar = dpswarrchar; //Char = c; StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.Bloodsurge;
            ReqTalent = true;
            Talent2ChksValue = DPSWarrChar.Talents.Bloodsurge;
            ReqMeleeWeap = ReqMeleeRange = true;
            Duration = 10f; // In Seconds
            RageCost = -1f;
            StanceOkFury = true;
            SL = slam;
            //WW = whirlwind;
            BT = bloodthirst;
            UseReact = true;
            //
            Initialize();
        }
        #region Variables
        //public float maintainActs;
        public Ability SL;
        //public Ability WW;
        public Ability BT;
        #endregion
        #region Functions
        public float GetActivates(float btActs, float perc) {
            float retVal = 0f;
            retVal = TalentsAsSpecialEffects.Bloodsurge[DPSWarrChar.Talents.Bloodsurge].GetAverageProcsPerSecond((FightDuration * perc) / btActs, BT.MHAtkTable.AnyLand, 3.3f, FightDuration * perc);
            return retVal * (FightDuration * perc);
        }
        /*private float BasicFuryRotation(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance)
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

        }*/
        //private float CalcSlamProcs(float chanceMHhit, float chanceOHhit, float hsActivates, float procChance) { return 0f; }
        protected override float ActivatesOverride
        {
            get
            {
                return 0f;
                /*float chance = DPSWarrChar.Talents.Bloodsurge * 0.10f;
                float chanceMhHitLands = (1f - MHAtkTable.Miss - MHAtkTable.Dodge);
                float chanceOhHitLands = (1f - OHAtkTable.Miss - OHAtkTable.Dodge);

                float procs3 = BasicFuryRotation(chanceMhHitLands, chanceOhHitLands, 0, chance);

                procs3 = (maintainActs > procs3) ? 0f : procs3 - maintainActs;

                //return procs3; // *(1f - Whiteattacks.AvoidanceStreak); // Not using AvoidanceStreak, as it's already accounted by the other ability's activates
                return procs3 * (1f - Whiteattacks.RageSlip(FightDuration / procs3, RageCost));*/
            }
        }
        public override float DamageOverride { get { return SL.DamageOverride; } }
        #endregion
    }
    public sealed class RagingBlow : Ability
    {
        public static new string SName { get { return "Raging Blow"; } }
        public static new string SDesc { get { return string.Format("A mighty blow that deals {0:0%} weapon damage from both melee weapons. Can only be used while Enraged.", DamageMultiplier); } }
        public static new string SIcon { get { return "ability_hunter_swiftstrike"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 85288; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// A mighty blow that deals 110% weapon damage from both melee weapons. Can only be used while Enraged.
        /// <para>DPSWarrChar.Talents: none</para>
        /// <para>Glyphs: RagingBlow [+5% Crit Chance]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public RagingBlow(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            ReqTalent = true;
            Talent2ChksValue = DPSWarrChar.Talents.RagingBlow;
            AbilIterater = (int)Maintenance.RagingBlow;
            ReqMeleeWeap = ReqMeleeRange = true;
            CD = 6f; // In Seconds
            RageCost = 20f;
            StanceOkFury = true;
            SwingsOffHand = true;
            BonusCritChance = DPSWarrChar.Talents.GlyphOfRagingBlow ? 0.05f : 0f;
            if (DPSWarrChar.CalcOpts.PtrMode) {
                DamageBonus = 1f + (0.376f + 0.0560f * StatConversion.GetMasteryFromRating(DPSWarrChar.StatS.MasteryRating, CharacterClass.Warrior));
                DamageBonus *= 1f + DPSWarrChar.Talents.WarAcademy * 0.05f;
            } else {
                DamageBonus = 1f + (0.376f + 0.0470f * StatConversion.GetMasteryFromRating(DPSWarrChar.StatS.MasteryRating, CharacterClass.Warrior));
            }
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
        private float GetDamage(bool isOffHand) { return (isOffHand ? DPSWarrChar.CombatFactors.NormalizedOHWeaponDmg : DPSWarrChar.CombatFactors.NormalizedMHWeaponDmg) * DamageBonus; }
        public override float DamageOnUseOverride
        {
            get
            {
                // ==== MAIN HAND ====
                float DamageMH = GetDamage(false); // Base Damage
                DamageMH *= DPSWarrChar.CombatFactors.DamageBonus; // Global Damage Bonuses
                DamageMH *= DPSWarrChar.CombatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    - MHAtkTable.Glance // glancing handled below
                    - MHAtkTable.Block  // blocked handled below
                    - MHAtkTable.Crit); // crits   handled below

                float dmgGlance = DamageMH * MHAtkTable.Glance * CombatFactors.ReducWHGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                float dmgBlock = DamageMH * MHAtkTable.Block * CombatFactors.ReducYWBlockedDmg;//Partial damage when blocked
                float dmgCrit = DamageMH * MHAtkTable.Crit * (1f + DPSWarrChar.CombatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                DamageMH *= dmgDrop;

                DamageMH += dmgGlance + dmgBlock + dmgCrit;

                // ==== OFF HAND ====
                float DamageOH = GetDamage(true); // Base Damage
                DamageOH *= DPSWarrChar.CombatFactors.DamageBonus; // Global Damage Bonuses
                DamageOH *= DPSWarrChar.CombatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                dmgDrop = (1f
                    - OHAtkTable.Miss   // no damage when being missed
                    - OHAtkTable.Dodge  // no damage when being dodged
                    - OHAtkTable.Parry  // no damage when being parried
                    - OHAtkTable.Glance // glancing handled below
                    - OHAtkTable.Block  // blocked handled below
                    - OHAtkTable.Crit); // crits   handled below

                dmgGlance = DamageOH * OHAtkTable.Glance * CombatFactors.ReducWHGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                dmgBlock = DamageOH * OHAtkTable.Block * CombatFactors.ReducYWBlockedDmg;//Partial damage when blocked
                dmgCrit = DamageOH * OHAtkTable.Crit * (1f + DPSWarrChar.CombatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

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
    public sealed class HeroicStrike : Ability
    {
        public static new string SName { get { return "Heroic Strike"; } }
        public static new string SDesc { get { return "An attack that instantly deals (8+AP*0.75) physical damage. A good attack for moments of excess rage."; } }
        public static new string SIcon { get { return "ability_rogue_ambush"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 78; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// An attack that instantly deals (8+AP*0.75) physical damage. A good attack for moments of excess rage.
        /// <para>DPSWarrChar.Talents: Improved Heroic Strike [-(1*Pts) rage cost], Incite [+(5*Pts)% crit chance]</para>
        /// <para>Glyphs: Glyph of Heroic Strike [+10 rage on crits]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public HeroicStrike(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.HeroicStrike;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            CD = 3f; // In Seconds
            RageCost = 30f;
            DamageBase = 8f + DPSWarrChar.StatS.AttackPower * 0.75f;
            if (DPSWarrChar.CalcOpts.PtrMode) {
                DamageBonus = 1f;
            } else {
                DamageBonus = 1f + DPSWarrChar.Talents.WarAcademy * 0.05f;
            }
            BonusCritChance = DPSWarrChar.Talents.Incite * 0.05f;
            UsesGCD = false;
            //
            Initialize();
        }
        private float storedInciteBonusCrits = 0f;
        private float storedActs = 0f;
        public float InciteBonusCrits(float acts) {
            if (DPSWarrChar.Talents.Incite == 0) { storedActs = 0f; storedInciteBonusCrits = 0f; return 0f; }
            storedActs = acts;
            // Factor that we don't HS in Exec phase
            float fightDur = (!DPSWarrChar.CombatFactors.FuryStance && DPSWarrChar.CalcOpts.M_ExecuteSpam) ? FightDurationO20 : FightDuration;
            storedInciteBonusCrits = TalentsAsSpecialEffects.Incite[DPSWarrChar.Talents.Incite].GetAverageProcsPerSecond(fightDur / acts, MHAtkTable.Crit, DPSWarrChar.CombatFactors.MHSpeedHasted, fightDur) * fightDur;
            return storedInciteBonusCrits;
        }
        public override float DamageOnUseOverride {
            get {
                float dmg = Damage; // Base Damage
                dmg *= DPSWarrChar.CombatFactors.DamageBonus; // Global Damage Bonuses
                dmg *= DPSWarrChar.CombatFactors.DamageReduction; // Global Damage Penalties

                float bonusForcedCritsPerc = (DPSWarrChar.Talents.Incite > 0 && storedInciteBonusCrits > 0f && storedActs > 0f)
                                             ? storedInciteBonusCrits / storedActs : 0;

                // Work the Attack Table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    - MHAtkTable.Block  // blocked handled below
                    - (MHAtkTable.Crit + bonusForcedCritsPerc)); // crits   handled below

                float dmgBlock = dmg * MHAtkTable.Block * CombatFactors.ReducYWBlockedDmg;//Partial damage when blocked
                float dmgCrit = dmg * (MHAtkTable.Crit + bonusForcedCritsPerc) * (1f + DPSWarrChar.CombatFactors.BonusYellowCritDmg) * BonusCritDamage;//Bonus Damage when critting

                dmg *= dmgDrop;

                dmg += dmgBlock + dmgCrit;
                
                dmg *= AvgTargets;

                if (DPSWarrChar.CalcOpts.PtrMode) { dmg *= (1f - 0.20f); } // 4.0.6: Heroic Strike damage has been reduced by 20%.

                return dmg;
            }
        }
    }
    public sealed class Cleave : Ability
    {
        public static new string SName { get { return "Cleave"; } }
        public static new string SDesc { get { return "A sweeping attack that strikes the target and a nearby ally, dealing (6 + AP * 0.562) physical damage."; } }
        public static new string SIcon { get { return "ability_warrior_cleave"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 845; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// A sweeping attack that strikes the target and a nearby ally, dealing (6 + $ap * 0.45) physical damage.
        /// <para>DPSWarrChar.Talents: Improved Cleave [+(40*Pts)% Damage], Incite [+(5*Pts)% Crit Perc]</para>
        /// <para>Glyphs: Glyph of Cleaving [+1 targets hit]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Cleave(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.Cleave;
            ReqMeleeWeap = ReqMeleeRange = ReqMultiTargs = true;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            CD = 3f; // In Seconds
            RageCost = 30f;
            Targets = 2f + (DPSWarrChar.Talents.GlyphOfCleaving ? 1f : 0f);
            DamageBase = 6f + DPSWarrChar.StatS.AttackPower * 0.562f;
            if (DPSWarrChar.CalcOpts.PtrMode) {
                DamageBonus = 1f + DPSWarrChar.StatS.BonusCleaveDamageMultiplier;
            } else {
                DamageBonus = 1f + DPSWarrChar.Talents.WarAcademy * 0.05f + DPSWarrChar.StatS.BonusCleaveDamageMultiplier;
            }
            DamageBonus = 1f + DPSWarrChar.Talents.WarAcademy * 0.05f + DPSWarrChar.StatS.BonusCleaveDamageMultiplier;
            UsesGCD = false;
            //
            Initialize();
        }
        public override float DamageOnUseOverride
        {
            get
            {
                float dmg = base.DamageOnUseOverride;
                if (DPSWarrChar.CalcOpts.PtrMode) { dmg *= (1f - 0.20f); } // 4.0.6: Cleave damage has been reduced by 20%.
                return dmg;
            }
        }
    }
    #endregion
    #region Unused
    public sealed class Pummel : Ability
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
        /// <para>DPSWarrChar.Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// <para>TODO: Damage Increase proc from Rude Interruption Talent</para>
        /// </summary>
        public Pummel(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            //AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Pummel_;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkArms = StanceOkFury = true;
            CD = 10f; // In Seconds
            Duration = 4f;
            RageCost = TalentsAsSpecialEffects.DrumsOfWarRageCosts[DPSWarrChar.Talents.DrumsOfWar];
            DamageBase = 0;
            //
            Initialize();
        }
    }
    #endregion
}
