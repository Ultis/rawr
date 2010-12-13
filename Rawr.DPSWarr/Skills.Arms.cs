/**********
 * Owner: Jothay
 **********/
using System;
using System.Collections.Generic;

namespace Rawr.DPSWarr.Skills
{
    // Melee
    public class MortalStrike : Ability
    {
        public static new string SName { get { return "Mortal Strike"; } }
        public static new string SDesc { get { return "A vicious strike that deals "+DamageMultiplier.ToString("0%")+" weapon damage plus 423 and wounds the target, reducing the effectiveness of any healing received by 10% for 10 sec."; } }
        public static new string SIcon { get { return "ability_warrior_savageblow"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 12294; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// A vicious strike that deals 150% weapon damage plus 423 and wounds the
        /// target, reducing the effectiveness of any healing received by 10% for 10 sec.
        /// <para>Talents: none</para>
        /// <para>Glyphs: Glyph of Mortal Strike [+10% Dmg]</para>
        /// <para>Sets: T8 4P [+Crit %]</para>
        /// </summary>
        public MortalStrike(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.MortalStrike_;
            ReqMeleeWeap = ReqMeleeRange = StanceOkArms = true;
            DamageBase = combatFactors.NormalizedMhWeaponDmg * DamageMultiplier + 423f;
            DamageBonus = (1f + StatS.BonusExecOPMSDamageMultiplier)
                        * (1f + (Talents.GlyphOfMortalStrike ? 0.10f : 0f))
                        * (1f + StatS.BonusWarrior_T11_2P_BTMSDmgMult);
            Cd = 4.5f; // In Seconds
            RageCost = 25f;
            BonusCritChance = Talents.Cruelty * 0.05f;
            //
            Initialize();
        }
        public static float DamageMultiplier = 1.50f;
        private float _JuggernautBonusCritChance = 0f;
        private float _BonusCritChance = 0f;
        /// <summary>Percent Based Crit chance, from 0% (0 returns +0%, you don't need to set 1f for 100%)</summary>
        public float JuggernautBonusCritChance { get { return _JuggernautBonusCritChance; } set { _JuggernautBonusCritChance = value; } }
        public override float BonusCritChance { get { return _BonusCritChance + JuggernautBonusCritChance; } set { _BonusCritChance = value; } }
    }
    public class ColossusSmash : Ability
    {
        public static new string SName { get { return "Colossus Smash"; } }
        public static new string SDesc { get { return "Smashes a target for "+DamageMultiplier.ToString("0%")+" weapon damage plus 120 and weakens their defenses, allowing your attacks to entirely bypass their armor for 6 sec."; } }
        public static new string SIcon { get { return "ability_warrior_colossussmash"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 86346; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Smashes a target for 150% weapon damage plus 120 and weakens their defenses,
        /// allowing your attacks to entirely bypass their armor for 6 sec.
        /// <para>Talents: Sudden Death [MeleeHits reset cooldown, (3*Pts)% Chance]</para>
        /// <para>Glyphs: Colossus Smash [Refreshes Sunder Armor on target]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public ColossusSmash(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.ColossusSmash_;
            ReqMeleeWeap = ReqMeleeRange = StanceOkArms = StanceOkFury = true;
            RageCost = 20f;
            Cd = 20f;
            DamageBase = combatFactors.AvgMhWeaponDmgUnhasted * DamageMultiplier + 120f;
            UseReact = true;
            //
            Initialize();
        }
        // Variables
        public static float DamageMultiplier = 1.50f;
        private static readonly SpecialEffect[/*Talents.SuddenDeath*/] _buff = {
            null,
            new SpecialEffect(Trigger.MeleeHit, null, 0f, 0f, 1 * 0.03f),
            new SpecialEffect(Trigger.MeleeHit, null, 0f, 0f, 2 * 0.03f),
        };
        protected SpecialEffect Buff { get { return _buff[Talents.SuddenDeath]; } }
        // Functions
        public float GetActivates(float landedatksoverdur, float mod)
        {
            if (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) { return 0f; }
            float actsUnderSD = Talents.SuddenDeath > 0 ? Buff.GetAverageProcsPerSecond((FightDuration * mod) / Math.Max(0, landedatksoverdur),
                1f, combatFactors._c_mhItemSpeed, (FightDuration * mod)) * (FightDuration * mod) : 0f;
            float min = (FightDuration * mod) / Cd; // If it follows it's cooldown, no SD procs
            float acts = Math.Max(actsUnderSD, min);
            //float acts = actsUnderSD + min;

            return acts * (1f - Whiteattacks.RageSlip((FightDuration * mod) / acts, RageCost));
        }
    }
    public class OverPower : Ability
    {
        public static new string SName { get { return "Overpower"; } }
        public static new string SDesc { get { return "Instantly overpower the enemy, causing "+DamageMultiplier.ToString("0%")+"weapon damage. Only useable after the target dodges. The Overpower cannot be blocked, dodged or parried."; } }
        public static new string SIcon { get { return "ability_meleedamage"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 7384; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Instantly overpower the enemy, causing 125% weapon damage. Only useable after the target dodges.
        /// The Overpower cannot be blocked, dodged or parried.
        /// <para>Talents: Improved Overpower [+(25*Pts)% Crit Chance]</para>
        /// <para>Glyphs: Glyph of Overpower [+10% DMG]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public OverPower(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Overpower_;
            ReqMeleeWeap = ReqMeleeRange = true;
            CanBeDodged = CanBeParried = CanBeBlocked = false;
            Cd = 1f;// In Seconds
            GCDTime = Cd;
            RageCost = 5f;
            //Targets += StatS.BonusTargets;
            StanceOkArms = true;
            DamageBase  = combatFactors.NormalizedMhWeaponDmg * DamageMultiplier;
            DamageBonus = (1f + StatS.BonusExecOPMSDamageMultiplier)
                        * (1f + (Talents.GlyphOfOverpower ? 0.10f : 0f));
            BonusCritChance = 0.20f * Talents.TasteForBlood;
            BonusCritDamage = 1f + Talents.Impale * 0.1f;
            UseReact = true; // can't plan for this
            //
            Initialize();
        }

        public static float DamageMultiplier = 1.25f;

        public float GetActivates(float AttacksThatDodgeOverDur, float sooActs)
        {
            if (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) { return 0f; }

            float acts = 0f;
            float LatentGCD = (1.5f + CalcOpts.Latency + (UseReact ? CalcOpts.React / 1000f : CalcOpts.AllowedReact));

            float dodge = Whiteattacks.MHAtkTable.Dodge;

            // Chance to activate: Dodges Only (No more Parry as of Cata
            if (dodge > 0f)
            {
                float WhtHitsOverDur = FightDuration / Whiteattacks.MhEffectiveSpeed
              + (combatFactors.useOH ? FightDuration / Whiteattacks.OhEffectiveSpeed : 0f)
              + sooActs;

                float dodgesoverDur = 0f
                    + WhtHitsOverDur * (dodge)
                    + (dodge > 0 ? AttacksThatDodgeOverDur : 0);

                //acts += Math.Max(0f, dodgesoverDur * (1f - Whiteattacks.AvoidanceStreak));
                acts += Math.Max(0f, dodgesoverDur * (1f - Whiteattacks.RageSlip(FightDuration / dodgesoverDur, RageCost)));
            }

            return acts;
        }
    }
    public class TasteForBlood : Ability
    {
        public static new string SName { get { return "Taste for Blood"; } }
        public static new string SDesc { get { return "Increases your Overpower critical strike chance by [20*Pts]%. In addition, whenever your Rend ability causes damage, you have a [100/3*Pts]% chance of allowing the use of Overpower for 9 sec. This effect will not occur more than once every 5 sec."; } }
        public static new string SIcon { get { return "ability_rogue_hungerforblood"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 56638; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Increases your Overpower critical strike chance by 60%. In addition, whenever your Rend ability causes
        /// damage, you have a 100% chance of allowing the use of Overpower for 9 sec.
        /// This effect will not occur more than once every 5 sec.
        /// <para>Talents: Improved Overpower [+(25*Pts)% Crit Chance], Unrelenting Assault [-(2*Pts) sec cooldown, +(10*Pts)% Damage.]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public TasteForBlood(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.TasteForBlood_;
            ReqTalent = true;
            Talent2ChksValue = Talents.TasteForBlood;
            ReqMeleeWeap = ReqMeleeRange = true;
            CanBeDodged = CanBeParried = CanBeBlocked = false;
            GCDTime = 1f;
            // Technically it has a 5 sec cd, but Rend only ticks every 3 sec
            // and it has to tick on every other tick so 6 sec
            Cd = 6f;// 5f; // In Seconds
            RageCost = 5f;
            StanceOkArms = true;
            DamageBase = combatFactors.NormalizedMhWeaponDmg * DamageMultiplier;
            DamageBonus = (1f + StatS.BonusExecOPMSDamageMultiplier) * (1f + (Talents.GlyphOfOverpower ? 0.10f : 0f));
            BonusCritChance = 0.20f * Talents.TasteForBlood;
            BonusCritDamage = 1f + Talents.Impale * 0.1f;
            //UseReact = true; // you can plan for it ahead of time, unlike SD and normal OP
            //
            Initialize();
        }

        public static float DamageMultiplier = 1.25f;

        protected override float ActivatesOverride
        {
            get
            {
                float acts = 0f;

                // Chance to activate Requires Rend
                if (CalcOpts.M_Rend)
                {
                    acts = base.ActivatesOverride * (1f / 3f * Talents.TasteForBlood);
                }

                return acts;
            }
        }
    }
    public class Execute : Ability
    {
        public static new string SName { get { return "Execute"; } }
        public static new string SDesc { get { return "Attempt to finish off a wounded foe, causing (10+AP*0.437) physical damage and consumes up to 20 additional rage to deal up to (AP*0.874-1) additional damage. Only usable on enemies that have less than 20% health."; } }
        public static new string SIcon { get { return "inv_sword_48"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 5308; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Attempt to finish off a wounded foe, causing (10+AP*0.437) physical damage and
        /// consumes up to 20 additional rage to deal up to (AP*0.874-1) additional damage.
        /// Only usable on enemies that have less than 20% health.
        /// <para>Talents: Executioner [Exec procs Haste], Sudden Death [Keep 5*Pts Rage After Use]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Execute(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_;
            ReqMeleeWeap = ReqMeleeRange = true;
            RageCost = 10f;
            DamageBonus = 1f + StatS.BonusExecOPMSDamageMultiplier;
            FreeRage = 0f;
            StanceOkFury = StanceOkArms = true;
            //
            Initialize();
        }
        private float FREERAGE;
        public float FreeRage { get { return FREERAGE; } set { FREERAGE = Math.Max(0f, value); } } // Must be above zero to prevent other calc problems
        public float UsedExtraRage { get { return Math.Max(0f, Math.Min(20f, ActivatesOverride == 0 ? 0f : FreeRage / (ActivatesOverride/* * (float)BossOpts.Under20Perc*/))); } }
        private float _DumbActivates = 0f;
        public float DumbActivates { get { return _DumbActivates; } set { _DumbActivates = value; } }
        protected override float ActivatesOverride { get { return DumbActivates; } }
        public override float DamageOverride {
            get {
                return ((10f + StatS.AttackPower * 0.437f) + ((UsedExtraRage / 20f) * (StatS.AttackPower * 0.874f - 1f)))
                       * DamageBonus /* * AvgTargets*/;
            }
        }
        public override float GetRageUseOverDur(float acts) {
            if (!Validated) { return 0f; }
            return acts * (RageCost + UsedExtraRage);
        }
        public override string GenTooltip(float acts, float dpsO20, float dpsU20, float ttldpsperc)
        {
            return base.GenTooltip(acts, dpsO20, dpsU20, ttldpsperc)
                + string.Format("\r\nExecute is boosted by {0}/20 Additional Rage", UsedExtraRage);
        }
    }
    public class Slam : Ability
    {
        public static new string SName { get { return "Slam"; } }
        public static new string SDesc { get { return "Slams the opponent, causing "+DamageMultiplier.ToString("0%")+" weapon damage plus (387*1.25)."; } }
        public static new string SIcon { get { return "ability_warrior_decisivestrike"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 1464; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Slams the opponent, causing 125% weapon damage plus (430*1.25).
        /// <para>Talents: Improved Slam [Reduces cast time of your Slam ability by (0.5/1) sec.]</para>
        /// <para>Glyphs: Slam [+5% Crit]</para>
        /// <para>Sets: T7 Deadnaught Battlegear 2 Pc [+10% Damage]</para>
        /// </summary>
        public Slam(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Slam_;
            ReqMeleeWeap = ReqMeleeRange = StanceOkArms = StanceOkDef = true;
            Cd = 1.5f;
            RageCost = 15f;
            DamageBase = (combatFactors.AvgMhWeaponDmgUnhasted + 387f) * DamageMultiplier;
            DamageBonus = 1f + Talents.WarAcademy * 0.05f;
            DamageBonus *= 1f + (!combatFactors.FuryStance ? (Talents.ImprovedSlam * 0.10f) : 0f);
            BonusCritDamage = 1f + Talents.Impale * 0.1f;
            BonusCritChance += Talents.GlyphOfSlam ? 0.05f : 0f;
            CastTime = (1.5f - (!combatFactors.FuryStance ? (Talents.ImprovedSlam * 0.5f) : 0f)); // In Seconds
            //
            Initialize();
        }
        public static float DamageMultiplier = 1.25f;
    }
    public class VictoryRush : Ability
    {
        public static new string SName { get { return "Victory Rush"; } }
        public static new string SDesc { get { return "Instantly attack the target causing (AP*56/100) damage and healing you for 20% of your maximum health. Can only be used within 20 sec after you kill an enemy that yields experience or honor."; } }
        public static new string SIcon { get { return "ability_warrior_devastate"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 34428; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Instant, No Cd, No Rage, Melee Range, (Any)
        /// Instantly attack the target causing (AP*56/100) damage and healing you for 20% of your
        /// maximum health. Can only be used within 20 sec after you kill an enemy that yields
        /// experience or honor.
        /// <para>Talents: </para>
        /// <para>Glyphs: 
        /// Glyph of Victory Rush [+30% Crit Chance @ targs >70% HP],
        /// Glyph of Enduring Victory [+5 sec to length before ability wears off]
        /// </para>
        /// <para>Sets: </para>
        /// </summary>
        public VictoryRush(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.VictoryRush_;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            RageCost = -1f;
            //
            DamageBase = StatS.AttackPower * 56f / 100f;
            DamageBonus = 1f + Talents.WarAcademy * 0.05f;
            HealingBase = StatS.Health * 0.20f * (1f + (Talents.GlyphOfVictoryRush ? 0.50f : 0f)); // 20% of Max Health Restored
            //
            Initialize();
            MHAtkTable = Whiteattacks.MHAtkTable;
        }
        protected override float ActivatesOverride { get { return (BossOpts.MultiTargs && BossOpts.Targets.Count > 0) ? FightDuration / BossOpts.MultiTargsFreq : 0f; } }
    }
    // Area of Effect
    public class Bladestorm : Ability
    {
        public static new string SName { get { return "Bladestorm"; } }
        public static new string SDesc { get { return "You become a whirling storm of destructive force, instantly striking all nearby targets for "+DamageMultiplier.ToString("0%")+" weapon damage and continuing to perform a whirlwind attack every 1 sec for 6 sec.  While under the effects of Bladestorm, you do not feel pity or remorse or fear and you cannot be stopped unless killed or disarmed, but you cannot perform any other abilities."; } }
        public static new string SIcon { get { return "ability_warrior_bladestorm"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 46924; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// You become a whirling storm of destructive force, instantly striking all nearby targets for
        /// 150% weapon damage and continuing to perform a whirlwind attack every 1 sec for 6 sec.
        /// While under the effects of Bladestorm, you do not feel pity or remorse or fear and you
        /// cannot be stopped unless killed or disarmed, but you cannot perform any other abilities.
        /// <para>Talents: Bladestorm [Requires Talent]</para>
        /// <para>Glyphs: Glyph of Bladestorm [-15 sec Cd]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Bladestorm(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Bladestorm_;
            ReqTalent = true;
            Talent2ChksValue = Talents.Bladestorm;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            MaxRange = 8; // In Yards
            Targets = 10; // Handled in WW
            DamageBase = combatFactors.NormalizedMhWeaponDmg * DamageMultiplier;
            Cd = 90f - (Talents.GlyphOfBladestorm ? 15f : 0f); // In Seconds
            RageCost = 25f;
            CastTime = 6f; // In Seconds // Channeled
            GCDTime = CastTime;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            SwingsOffHand = true;
            SwingsPerActivate = 7f;
            //
            Initialize();
        }
        // Variables
        public static float DamageMultiplier = 1.50f;
        // Functions
        public override float DamageOnUseOverride
        {
            get
            {
                //if (!Validated) { return 0f; }
                // ==== MAIN HAND ====
                float Damage = DamageBase * DamageBonus; // Base Damage
                Damage *= combatFactors.DamageBonus; // Global Damage Bonuses
                Damage *= combatFactors.DamageReduction; // Global Damage Penalties

                // Work the Attack Table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    //- MHAtkTable.Glance // glancing handled below
                    - MHAtkTable.Block  // blocked handled below
                    - MHAtkTable.Crit); // crits   handled below

                //float dmgGlance = Damage * MHAtkTable.Glance * combatFactors.ReducWhGlancedDmg;//Partial Damage when glancing, this doesn't actually do anything since glance is always 0
                float dmgBlock = Damage * MHAtkTable.Block * combatFactors.ReducYwBlockedDmg;//Partial damage when blocked
                float dmgCrit = Damage * MHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg);//Bonus   Damage when critting

                Damage *= dmgDrop;

                Damage += /*dmgGlance +*/ dmgBlock + dmgCrit;

                // ==== RESULT ====
                return Damage * AvgTargets * SwingsPerActivate;
            }
        }
    }
    // Ranged
    public class HeroicThrow : Ability
    {
        public static new string SName { get { return "Heroic Throw"; } }
        public static new string SDesc { get { return "Throws your weapon at the enemy causing 12+(AP*0.5) damage (based on attack power). This ability causes high threat."; } }
        public static new string SIcon { get { return "inv_axe_66"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 57755; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Instant, 1 min Cd, 30 yd, Melee Weapon (Any)
        /// <para>Throws your weapon at the enemy causing 12+(AP*0.5) damage (based on attack power). This ability causes high threat.</para>
        /// <para>Talents: Gag Order [(50*Pts)% Chance to Silence Target for 3 sec, -(15*Pts) sec Cd]</para>
        /// <para>Glyphs: Heroic Throw [Applies a Stack of Sunder Armor]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public HeroicThrow(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            //AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.HeroicThrow_;
            ReqMeleeWeap = ReqMeleeRange = StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = 12f + StatS.AttackPower * 0.50f;
            Cd = 60f - Talents.GagOrder * 15f; // In Seconds
            RageCost = -1f; // Free
            isMaint = true;
            //
            Initialize();
        }
        protected override float ActivatesOverride
        {
            get
            {
                float Cap = base.ActivatesOverride;
                float retVal = 0f;
                // Heroic Throw pops a Sunder Stack, so we get that + the damage from this which is better than just the Sunder
                if (Talents.GlyphOfHeroicThrow && CalcOpts.M_SunderArmor) {
                    retVal += 1;
                }
                // We want to use Heroic Throw whenever we are far away, because we get to spend a GCD doing damage rather than just running
                if (BossOpts.MovingTargs && BossOpts.Moves.Count > 0)
                {
                    // The move needs to be greater than a GCDs time away otherwise we are just using our Heroic Leap, Charge or Intercept to recover
                    // Need to test if we can press both the Movement ability AND Heroic Throw a the same time
                    float MoveSpeed = 7f * (1f + StatS.MovementSpeed);
                    float LatentGCD = 1.5f + CalcOpts.FullLatency;
                    foreach(Impedance m in BossOpts.Moves) {
                        if (!m.Validate) { continue; }// It's a bad one
                        if ((m.Duration / 1000f) < (MoveSpeed / LatentGCD)) { continue; } // it's not long enough
                        if (m.Frequency < Cd) { retVal += (m.Frequency / Cd) * FightDuration; continue; } // reduced rate because we can't hit every one
                        retVal += FightDuration / m.Frequency; // we get the full rate, every pop
                    }
                }
                // return result
                return Math.Min(retVal, Cap); //base.ActivatesOverride;
            }
        }
    }
    // Passive Melee
    public class StrikesOfOpportunity : Ability
    {
        public static new string SName { get { return "Strikes Of Opportunity"; } }
        public static new string SDesc { get { return "Grants a (16+2*Mastery)% chance for your melee attacks to instantly trigger an additional melee attack for "+DamageModifier.ToString("0%")+" normal damage. Each point of Mastery increases this chance by 2%."; } }
        public static new string SIcon { get { return "ability_backstab"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 76838; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Grants a (16+2*Mastery)% chance for your melee attacks to instantly trigger
        /// an additional melee attack for 100% normal damage. Each point of Mastery
        /// increases this chance by 2%.
        /// <para>Talents: Passive Arms Benefit</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public StrikesOfOpportunity(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            ReqMeleeRange = ReqMeleeWeap = true;
            StanceOkArms = true;
            DamageBase = combatFactors.AvgMhWeaponDmgUnhasted * DamageModifier;
            RageCost = -1f;
            GCDTime = 0f;
            UsesGCD = false;
            //
            Initialize();
        }

        public static readonly float DamageModifier = 1.00f;

        //private static Dictionary<float, SpecialEffect> _SE_StrikesOfOpportunity = new Dictionary<float,SpecialEffect>();

        public float GetActivates(float MeleeAttemptsOverDur, float perc)
        {
            /*if (!_SE_StrikesOfOpportunity.ContainsKey(StatS.MasteryRating)) {
                try {
                    _SE_StrikesOfOpportunity.Add(StatS.MasteryRating,
                        new SpecialEffect(Trigger.MeleeAttack, null, 0f, 0f,
                            (float)Math.Min(0.16f + (float)Math.Max(0f, 0.02f * StatConversion.GetMasteryFromRating(StatS.MasteryRating, CharacterClass.Warrior)), 1f)
                        ));
                } catch (Exception) { }
            }*/
            // This attack doesn't consume GCDs and doesn't affect the swing timer
            //float effectActs = _SE_StrikesOfOpportunity[StatS.MasteryRating].GetAverageProcsPerSecond(
            float effectActs = new SpecialEffect(Trigger.MeleeAttack, null, 0f, 0.5f,
                            (float)Math.Min(0.16f + (float)Math.Max(0f, 0.02f * StatConversion.GetMasteryFromRating(StatS.MasteryRating, CharacterClass.Warrior)), 1f)
                    ).GetAverageProcsPerSecond((FightDuration * perc) / MeleeAttemptsOverDur, 1f, combatFactors._c_mhItemSpeed, FightDuration * perc);
            effectActs *= FightDuration * perc;
            return effectActs;
        }
        public override string GenTooltip(float acts, float dpsO20, float dpsU20, float ttldpsperc)
        {
            //float Over20 = CalcOpts.M_ExecuteSpam ? 1f - (float)BossOpts.Under20Perc : 1f;
            //float Undr20 = CalcOpts.M_ExecuteSpam ? (float)BossOpts.Under20Perc : 1f;

            float misses = GetXActs(AttackTableSelector.Missed, acts), missesPerc = (acts == 0f ? 0f : misses / acts);
            float dodges = GetXActs(AttackTableSelector.Dodged, acts), dodgesPerc = (acts == 0f ? 0f : dodges / acts);
            float parrys = GetXActs(AttackTableSelector.Parried, acts), parrysPerc = (acts == 0f ? 0f : parrys / acts);
            float blocks = GetXActs(AttackTableSelector.Blocked, acts), blocksPerc = (acts == 0f ? 0f : blocks / acts);
            float glance = GetXActs(AttackTableSelector.Glance, acts), glancePerc = (acts == 0f ? 0f : glance / acts);
            float crits = GetXActs(AttackTableSelector.Crit, acts), critsPerc = (acts == 0f ? 0f : crits / acts);
            float hits = GetXActs(AttackTableSelector.Hit, acts), hitsPerc = (acts == 0f ? 0f : hits / acts);

            bool showmisss = misses > 0f;
            bool showdodge = CanBeDodged && dodges > 0f;
            bool showparry = CanBeParried && parrys > 0f;
            bool showblock = CanBeBlocked && blocks > 0f;
            bool showglance = false && glance > 0f;
            bool showcrits = CanCrit && crits > 0f;

            string tooltip = "*" + Name +
                Environment.NewLine + "Cast Time: " + (CastTime != -1 ? CastTime.ToString() : "Instant")
                                    + ", CD: " + (Cd != -1 ? Cd.ToString() : "None")
                                    + ", Rage Generated: " + (RageCost != -1 ? (-1f * RageCost).ToString() : "None") +
            Environment.NewLine + Environment.NewLine + acts.ToString("000.00") + " Activates over Attack Table:" +
            (showmisss ? Environment.NewLine + "- " + misses.ToString("000.00") + " : " + missesPerc.ToString("00.00%") + " : Missed "  : "") +
            (showdodge ? Environment.NewLine + "- " + dodges.ToString("000.00") + " : " + dodgesPerc.ToString("00.00%") + " : Dodged "  : "") +
            (showparry ? Environment.NewLine + "- " + parrys.ToString("000.00") + " : " + parrysPerc.ToString("00.00%") + " : Parried " : "") +
            (showblock ? Environment.NewLine + "- " + blocks.ToString("000.00") + " : " + blocksPerc.ToString("00.00%") + " : Blocked " : "") +
            (showglance? Environment.NewLine + "- " + glance.ToString("000.00") + " : " + glancePerc.ToString("00.00%") + " : Glanced " : "") +
            (showcrits ? Environment.NewLine + "- " + crits.ToString( "000.00") + " : " + critsPerc.ToString( "00.00%") + " : Crit "    : "") +
                         Environment.NewLine + "- " + hits.ToString(  "000.00") + " : " + hitsPerc.ToString(  "00.00%") + " : Hit " +
                Environment.NewLine +
                //Environment.NewLine + "Damage per Blocked|Hit|Crit: x|x|x" +
                Environment.NewLine + "Targets Hit: " + (Targets != -1 ? AvgTargets.ToString("0.00") : "None") +
                //Environment.NewLine + "DPS: " + (GetDPS(acts, Over20) > 0 ? GetDPS(acts, Over20).ToString("0.00") : "None") + "|" + (GetDPS(acts, Undr20) > 0 ? GetDPS(acts, Undr20).ToString("0.00") : "None") +
                Environment.NewLine + "DPS: " + (dpsO20 > 0 ? dpsO20.ToString("0.00") : "None") + "|" + (dpsU20 > 0 ? dpsU20.ToString("0.00") : "None") +
                Environment.NewLine + "Percentage of Total DPS: " + (ttldpsperc > 0 ? ttldpsperc.ToString("00.00%") : "None");

            return tooltip;
        }
    }
    // DoTs
    public class Rend : DoT
    {
        public static new string SName { get { return "Rend"; } }
        public static new string SDesc { get { return "Wounds the target causing them to bleed for 525 damage plus an additional (0.25*6*((MWB+mwb)/2+AP/14*MWS)) (based on weapon damage) over 15 sec."; } }
        public static new string SIcon { get { return "ability_gouge"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellID { get { return 772; } }
        public override int SpellID { get { return SSpellID; } }
        /// <summary>
        /// Wounds the target causing them to bleed for 525 damage plus an additional (0.25*6*((MWB+mwb)/2+AP/14*MWS)) (based on weapon damage) over 15 sec.
        /// <para>Talents: Improved Rend [+(10*Pts)% Bleed Damage], Trauma [+(15*Pts)% Bleed Damage]</para>
        /// <para>Glyphs: Glyph of Rending [+2 damage ticks]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Rend(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.Rend_;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            Duration = 15f; // In Seconds
            RageCost = 10f;
            Targets = Talents.BloodAndThunder > 0 ? Talents.BloodAndThunder * 0.50f * 10f : 1f;
            Cd = Duration + 3f;
            TimeBtwnTicks = 3f; // In Seconds
            StanceOkArms = StanceOkDef = true;
            DamageBase = 525f;
            //
            Initialize();
        }
        protected float addMisses;
        protected float addDodges;
        protected float addParrys;
        public float ThunderAppsO20 = 0f;
        public float ThunderAppsU20 = 0f;
        protected override float ActivatesOverride
        {
            get
            {
                // Since Rend has to be up in order to Taste for Blood
                // this override has the intention of taking the baseline
                // activates and forcing the char to use Rend again to ensure it's up
                // in the event that the attemtped activate didn't land (it Missed, was Dodged or Parried)
                // We're only doing the additional check once so it will at most Rend
                // twice in a row, may consider doing a settler
                //
                // HOWEVER, with this wonderful new Blood and Thunder talent in Cata, you only need to apply
                // Rend to your initial target, then every time you Thunder Clap, you not only reapply Rend
                // but you also apply it to every target your Thunder Clap hit
                float result = 0f;
                float Base = base.ActivatesOverride;
                if (Talents.BloodAndThunder > 0) { Base = 1f; } // Initial Application as the rest is applied by Thunder Clap
                addMisses = (MHAtkTable.Miss  > 0) ? Base * MHAtkTable.Miss  : 0f;
                addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                result = Base + addMisses + addDodges + addParrys;

                return result;
            }
        }
        /// <summary>Initial Damage of the Rend, used in PTR mode only right now</summary>
        public override float InitialDamage
        {
            get
            {
                if (!Validated && !(true/*CalcOpts.PTRMode*/)) { return 0f; }
                // Local Variables
                float initial = 0f;
                // Base work
                float DmgMod = (1f + StatS.BonusBleedDamageMultiplier)
                             * (1f + StatS.BonusDamageMultiplier)
                             * DamageBonus;
                initial = DamageBase * DmgMod;
                // Modify it by the attack table
                float dmgDrop = (1f
                    - MHAtkTable.Miss   // no damage when being missed
                    - MHAtkTable.Dodge  // no damage when being dodged
                    - MHAtkTable.Parry  // no damage when being parried
                    - MHAtkTable.Crit); // crits   handled below

                float dmgCrit = initial * MHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg) * BonusCritDamage; // Bonus Damage when critting

                initial *= dmgDrop;

                initial += dmgCrit;
                // return value
                return initial * AvgTargets;
            }
        }
        /// <summary>The Size of a Tick. In PTR Mode, this does not include the Base Damage</summary>
        public override float TickSize
        {
            get
            {
                if (!Validated) { return 0f; }

                float DmgBonusBase = (StatS.AttackPower * /*3.3f*/combatFactors._c_mhItemSpeed) / 14f
                                   + (combatFactors.MH.MaxDamage + combatFactors.MH.MinDamage) / 2f;
                DmgBonusBase *= 0.25f * 6f;
                float DmgMod = (1f + StatS.BonusBleedDamageMultiplier)
                             * (1f + StatS.BonusDamageMultiplier)
                             * DamageBonus;


                float usinginitial = (true/*CalcOpts.PTRMode*/ && Talents.BloodAndThunder > 0 ? DamageBase * (1f - Talents.BloodAndThunder * 0.50f)
                                      : (true/*CalcOpts.PTRMode*/ && Talents.BloodAndThunder == 0 ? 0f
                                      : DamageBase));
                float TickSize = ((usinginitial + DmgBonusBase) * DmgMod) / NumTicks;

                float dmgCrit = TickSize * MHAtkTable.Crit * (1f + combatFactors.BonusYellowCritDmg) * BonusCritDamage;

                TickSize *= 1f - MHAtkTable.Crit;

                TickSize += dmgCrit;

                return TickSize;
            }
        }
        public override float GetDPS(float acts, float perc)
        {
            return GetDPS(acts, 0, perc);
        }
        /// <summary>
        /// This is overriden so that Blood and Thunder's Thunder Clap refreshes loses a Tick,
        /// since you have to 
        /// </summary>
        public override float NumTicks { get { return base.NumTicks - (Talents.BloodAndThunder * 0.5f); } }
        /// <summary>
        /// Get the DPS for this ability, limiting the Fight Duration by a percentage (O20|U20)
        /// </summary>
        /// <param name="acts">The number of times Rend itself is used</param>
        /// <param name="thunderapps">The number of times Thunder Clap is used to refresh Rend's duration</param>
        /// <param name="perc">The Time modifier for Fight Duration</param>
        /// <returns></returns>
        public float GetDPS(float acts, float thunderapps, float perc)
        {
            if (Talents.BloodAndThunder == 0) { thunderapps = 0; } else if (Talents.BloodAndThunder == 1) { thunderapps *= 0.50f; }
            float dmgonuse = TickSize;
            float numticks = NumTicks * ((acts + (thunderapps * AvgTargets)) - addMisses - addDodges - addParrys);
            float result = GetDmgOverTickingTime(acts + (thunderapps * AvgTargets)) / (FightDuration * perc);
            if (true/*CalcOpts.PTRMode*/) { result += (InitialDamage * acts) / (FightDuration * perc); }
            return result;
        }
    }
}
