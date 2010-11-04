/**********
 * Owner: Jothay
 **********/
using System;
using System.Collections.Generic;

namespace Rawr.DPSWarr.Skills
{
    #region Instants
    public class MortalStrike : Ability
    {
        public static new string SName { get { return "Mortal Strike"; } }
        public static new string SDesc { get { return "A vicious strike that deals 185% weapon damage plus 423 and wounds the target, reducing the effectiveness of any healing received by 10% for 10 sec."; } }
        public static new string SIcon { get { return "ability_warrior_savageblow"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// A vicious strike that deals 185% weapon damage plus 423 and wounds the
        /// target, reducing the effectiveness of any healing received by 10% for 10 sec.
        /// <para>Talents: Mortal Strike (Requires Talent), Improved Mortal Strike [+(10-ROUNDUP(10/3*Pts))% Dmg, -(1/3*Pts) Cd]</para>
        /// <para>Glyphs: Glyph of Mortal Strike [+10% Dmg]</para>
        /// <para>Sets: T8 4P [+Crit %]</para>
        /// </summary>
        public MortalStrike(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.MortalStrike_;
            ReqMeleeWeap = ReqMeleeRange = StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = combatFactors.AvgMhWeaponDmgUnhasted * 1.85f + 423f;
            DamageBonus = (1f + StatS.BonusExecOPMSDamageMultiplier) * (1f + (Talents.GlyphOfMortalStrike ? 0.10f : 0f));
            Cd = 4.5f; // In Seconds
            RageCost = 25f;
            BonusCritChance = StatS.BonusWarrior_T8_4P_MSBTCritIncrease + Talents.Cruelty * 0.05f;
            //
            Initialize();
        }
    }
    public class ColossusSmash : Ability
    {
        public static new string SName { get { return "Colossus Smash"; } }
        public static new string SDesc { get { return "Smashes a target for 150% weapon damage plus 120 and weakens their defenses, allowing your attacks to entirely bypass their armor for 6 sec."; } }
        public static new string SIcon { get { return "ability_warrior_colossussmash"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Smashes a target for 150% weapon damage plus 120 and weakens their defenses,
        /// allowing your attacks to entirely bypass their armor for 6 sec.
        /// <para>Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public ColossusSmash(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ColossusSmash_;
            ReqMeleeWeap = ReqMeleeRange = StanceOkArms = StanceOkFury = true;
            RageCost = 20f;
            Cd = 20f;
            DamageBase = combatFactors.AvgMhWeaponDmgUnhasted * 1.50f + 120f;
            UseReact = true;
            //
            Initialize();
        }
        // Variables
        private static readonly SpecialEffect[/*Talents.SuddenDeath*/] _buff = {
            null,
            new SpecialEffect(Trigger.MeleeHit, null, 0f, 0f, 1 * 0.03f),
            new SpecialEffect(Trigger.MeleeHit, null, 0f, 0f, 2 * 0.03f),
        };
        protected SpecialEffect Buff { get { return _buff[Talents.SuddenDeath]; } }
        // Functions
        public float GetActivates(float landedatksoverdur)
        {
            if (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) { return 0f; }
            float actsUnderSD = Talents.SuddenDeath > 0 ? Buff.GetAverageProcsPerSecond(landedatksoverdur / FightDuration, 1f, combatFactors._c_mhItemSpeed, FightDuration) * FightDuration : 0f;
            float min = FightDuration / Cd; // If it follows it's cooldown, no SD procs
            float acts = Math.Max(actsUnderSD, min);

            return acts * (1f - Whiteattacks.RageSlip(FightDuration / acts, RageCost));
        }
    }
    public class OverPower : Ability
    {
        public static new string SName { get { return "Overpower"; } }
        public static new string SDesc { get { return "Instantly overpower the enemy, causing weapon damage.  Only useable after the target dodges.  The Overpower cannot be blocked, dodged or parried."; } }
        public static new string SIcon { get { return "ability_meleedamage"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Instantly overpower the enemy, causing weapon damage. Only useable after the target dodges.
        /// The Overpower cannot be blocked, dodged or parried.
        /// <para>Talents: Improved Overpower [+(25*Pts)% Crit Chance]</para>
        /// <para>Glyphs: Glyph of Overpower [+10% DMG]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public OverPower(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Overpower_;
            ReqMeleeWeap = ReqMeleeRange = true;
            CanBeDodged = CanBeParried = CanBeBlocked = false;
            Cd = 1f;// In Seconds
            GCDTime = Cd;
            RageCost = 5f;
            //Targets += StatS.BonusTargets;
            StanceOkArms = true;
            DamageBase = combatFactors.AvgMhWeaponDmgUnhasted;
            DamageBonus = (1f + StatS.BonusExecOPMSDamageMultiplier) * (1f + (Talents.GlyphOfOverpower ? 0.10f : 0f));
            BonusCritChance = 0.20f * Talents.TasteForBlood;
            BonusCritDamage = 1f + Talents.Impale * 0.1f;
            UseReact = true; // can't plan for this
            //
            Initialize();
        }
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
                    + WhtHitsOverDur * (dodge )
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
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.TasteForBlood_;
            ReqTalent = true;
            Talent2ChksValue = Talents.TasteForBlood;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            CanBeDodged = false;
            CanBeParried = false;
            CanBeBlocked = false;
            GCDTime = 1f;
            Cd = 5f; // In Seconds
            RageCost = 5f;
            StanceOkArms = true;
            DamageBase = combatFactors.AvgMhWeaponDmgUnhasted;
            DamageBonus = (1f + StatS.BonusExecOPMSDamageMultiplier) * (1f + (Talents.GlyphOfOverpower ? 0.10f : 0f));
            BonusCritChance = 0.20f * Talents.TasteForBlood;
            BonusCritDamage = 1f + Talents.Impale * 0.1f;
            //UseReact = true; // you can plan for it ahead of time, unlike SD and normal OP
            //
            Initialize();
        }
        protected override float ActivatesOverride
        {
            get
            {
                float acts = 0f;

                // Chance to activate Requires Rend
                if (CalcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_])
                {
                    acts = base.ActivatesOverride * (1f / 3f * Talents.TasteForBlood);
                }

                return acts;
            }
        }
    }
    public class Bladestorm : Ability
    {
        public static new string SName { get { return "Bladestorm"; } }
        public static new string SDesc { get { return "You become a whirling storm of destructive force, instantly striking all nearby targets for 150% weapon damage and continuing to perform a whirlwind attack every 1 sec for 6 sec.  While under the effects of Bladestorm, you do not feel pity or remorse or fear and you cannot be stopped unless killed or disarmed, but you cannot perform any other abilities."; } }
        public static new string SIcon { get { return "ability_warrior_bladestorm"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// You become a whirling storm of destructive force, instantly striking all nearby targets for
        /// 150% weapon damage and continuing to perform a whirlwind attack every 1 sec for 6 sec.
        /// While under the effects of Bladestorm, you do not feel pity or remorse or fear and you
        /// cannot be stopped unless killed or disarmed, but you cannot perform any other abilities.
        /// <para>Talents: Bladestorm [Requires Talent]</para>
        /// <para>Glyphs: Glyph of Bladestorm [-15 sec Cd]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Bladestorm(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo, Ability ww)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            WW = ww;
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Bladestorm_;
            ReqTalent = true;
            Talent2ChksValue = Talents.Bladestorm;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            MaxRange = WW.MaxRange; // In Yards
            Targets = WW.Targets; // Handled in WW
            DamageBase = combatFactors.NormalizedMhWeaponDmg * 1.50f;
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
        public Ability WW;
        // Functions
        public override float DamageOnUseOverride {
            get {
                if (!Validated) { return 0f; }
                float Damage = (WW.DamageOnUseOverride / 0.75f) * 1.50f; // Negate WW's 75% penalty then give it BS's 150%
                return Damage * 7f; // it WW's 7 times
            }
        }
    }
    public class Execute : Ability
    {
        public static new string SName { get { return "Execute"; } }
        public static new string SDesc { get { return "Attempt to finish off a wounded foe, causing (10+AP*0.25) physical damage and consumes up to 20 additional rage to deal up to (AP*0.5-1) additional damage. Only usable on enemies that have less than 20% health."; } }
        public static new string SIcon { get { return "inv_sword_48"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Attempt to finish off a wounded foe, causing (10+AP*0.25) physical damage and
        /// consumes up to 20 additional rage to deal up to (AP*0.5-1) additional damage.
        /// Only usable on enemies that have less than 20% health.
        /// <para>Talents: Improved Execute [Reduces the rage cost of your Execute ability by (2.5/5).]</para>
        /// <para>Glyphs: Glyph of Execute [Your Execute ability acts as if it has 10 additional rage.]</para>
        /// </summary>
        public Execute(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_;
            ReqMeleeWeap = ReqMeleeRange = true;
            RageCost = 10f - (Talents.SuddenDeath * 5f);
            DamageBonus = 1f + StatS.BonusExecOPMSDamageMultiplier;
            FreeRage = 0f;
            StanceOkFury = StanceOkArms = true;
            //
            Initialize();
        }
        public bool GetReqMeleeWeap() { return this.ReqMeleeWeap; }
        public bool GetReqMeleeRange() { return this.ReqMeleeRange; }
        private float FREERAGE;
        public float FreeRage { get { return FREERAGE; } set { FREERAGE = Math.Max(0f, value); } } // Must be above zero to prevent other calc problems
        public float UsedExtraRage { get { return Math.Min(20f, FreeRage / (ActivatesOverride * (float)BossOpts.Under20Perc)); } }
        public override float DamageOverride {
            get {
                float damageBase = (10f + StatS.AttackPower * 0.25f) + (UsedExtraRage * (StatS.AttackPower * 0.5f - 1f));
                return damageBase * DamageBonus * AvgTargets;
            }
        }
        public override float GetRageUseOverDur(float acts)
        {
            if (!Validated) { return 0f; }
            return acts * (RageCost + UsedExtraRage);
        }
    }
    public class Slam : Ability
    {
        public static new string SName { get { return "Slam"; } }
        public static new string SDesc { get { return "Slams the opponent, causing 150% weapon damage plus 277."; } }
        public static new string SIcon { get { return "ability_warrior_decisivestrike"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Slams the opponent, causing 150% weapon damage plus 277.
        /// <para>Talents: Improved Slam [Reduces cast time of your Slam ability by (0.5/1) sec.]</para>
        /// <para>Glyphs: Slam [+5% Crit]</para>
        /// <para>Sets: T7 Deadnaught Battlegear 2 Pc [+10% Damage]</para>
        /// </summary>
        public Slam(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Slam_;
            ReqMeleeWeap = ReqMeleeRange = StanceOkArms = StanceOkDef = true;
            Cd = 1.5f;
            BonusCritChance = StatS.BonusWarrior_T9_4P_SLHSCritIncrease;
            RageCost = 15f;
            DamageBase = combatFactors.AvgMhWeaponDmgUnhasted * 1.50f + 277f;
            DamageBonus = 1f + StatS.BonusWarrior_T7_2P_SlamDamage;
            DamageBonus *= 1f + Talents.WarAcademy * 0.05f;
            DamageBonus *= 1f + Talents.ImprovedSlam * 0.10f;
            BonusCritDamage = 1f + Talents.Impale * 0.1f;
            BonusCritChance += Talents.GlyphOfSlam ? 0.05f : 0f;
            CastTime = (1.5f - (Talents.ImprovedSlam * 0.5f)); // In Seconds
            //
            Initialize();
        }
    }
    public class VictoryRush : Ability
    {
        public static new string SName { get { return "Victory Rush"; } }
        public static new string SDesc { get { return "Instantly attack the target causing (AP*45/100) damage and healing you for 20% of your maximum health. Can only be used within 20 sec after you kill an enemy that yields experience or honor."; } }
        public static new string SIcon { get { return "ability_warrior_devastate"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Instant, No Cd, No Rage, Melee Range, (Any)
        /// Instantly attack the target causing (AP*45/100) damage and healing you for 20% of your
        /// maximum health. Can only be used within 20 sec after you kill an enemy that yields
        /// experience or honor.
        /// <para>Talents: </para>
        /// <para>Glyphs: 
        /// Glyph of Victory Rush [+30% Crit Chance @ targs >70% HP]
        /// Glyph of Enduring Victory [+5 sec to length before ability wears off]
        /// </para>
        /// <para>Sets: </para>
        /// </summary>
        public VictoryRush(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.VictoryRush_;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            //
            DamageBase = StatS.AttackPower * 56f / 100f;
            DamageBonus = 1f + Talents.WarAcademy * 0.05f;
            HealingBase = StatS.Health * 0.20f * (1f + (Talents.GlyphOfVictoryRush ? 0.50f : 0f)); // 20% of Max Health Restored
            //
            Initialize();
        }
        protected override float ActivatesOverride { get { return BossOpts.MultiTargs ? FightDuration / BossOpts.MultiTargsFreq : 0f; } }
    }
    public class StrikesOfOpportunity : Ability
    {
        public static new string SName { get { return "Strikes Of Opportunity"; } }
        public static new string SDesc { get { return "Gives a (16+2*Mastery)% chance to get an extra attack on the same target dealing 115% of normal damage."; } }
        public static new string SIcon { get { return "inv_sword_27"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Gives a (16+2*Mastery)% chance to get an extra attack on the same target dealing 115% of normal damage.
        /// </summary>
        /// <para>Talents: </para>
        /// <para>Glyphs: </para>
        public StrikesOfOpportunity(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            //ReqTalent = true;
            //Targets += StatS.BonusTargets;
            ReqMeleeRange = ReqMeleeWeap = true;
            //Cd = 6f; // In Seconds
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = combatFactors.AvgMhWeaponDmgUnhasted * 1.15f; // 115% normal damage
            RageCost = -Whiteattacks.MHSwingRage; // This supposedly makes it generate rage, but I don't know if that's true. It could be that this thing is a Yellow
            UsesGCD = false;
            //
            Initialize();
            MHAtkTable = Whiteattacks.MHAtkTable;
        }

        private static Dictionary<float, SpecialEffect> _SE_StrikesOfOpportunity = new Dictionary<float,SpecialEffect>();

        public float GetActivates(float YellowsThatLandOverDur)
        {
            if (!_SE_StrikesOfOpportunity.ContainsKey(StatS.MasteryRating)) {
                try {
                    _SE_StrikesOfOpportunity.Add(StatS.MasteryRating,
                        new SpecialEffect(Trigger.MeleeAttack, null, 0f, 0f,
                            (float)Math.Min(0.16f + (float)Math.Max(0f, 0.02f * StatConversion.GetMasteryFromRating(StatS.MasteryRating, CharacterClass.Warrior)), 1f)
                        ));
                } catch (Exception) { }
            }
            // This attack doesn't consume GCDs and doesn't affect the swing timer
            float rawActs = (YellowsThatLandOverDur + Whiteattacks.LandedAtksOverDur) / FightDuration;
            float effectActs = _SE_StrikesOfOpportunity[StatS.MasteryRating].GetAverageProcsPerSecond(rawActs, 1f, combatFactors._c_mhItemSpeed, FightDuration);
            effectActs *= FightDuration;
            return effectActs;
        }
        public override string GenTooltip(float acts, float ttldpsperc)
        {
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
            bool showglance = true && glance > 0f;
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
                Environment.NewLine + "DPS: " + (GetDPS(acts) > 0 ? GetDPS(acts).ToString("0.00") : "None") +
                Environment.NewLine + "Percentage of Total DPS: " + (ttldpsperc > 0 ? ttldpsperc.ToString("00.00%") : "None");

            return tooltip;
        }
    }
    #endregion

    public class Rend : DoT
    {
        public static new string SName { get { return "Rend"; } }
        public static new string SDesc { get { return "Wounds the target causing them to bleed for 635 damage plus an additional (0.2*6*((MWB+mwb)/2+AP/14*MWS)) (based on weapon damage) over 15 sec."; } }
        public static new string SIcon { get { return "ability_gouge"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Wounds the target causing them to bleed for 635 damage plus an additional (0.2*6*((MWB+mwb)/2+AP/14*MWS)) (based on weapon damage) over 15 sec.
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
            CanCrit = false;
            Duration = 15f; // In Seconds
            RageCost = 10f;
            Cd = Duration + 3f;
            TimeBtwnTicks = 3f; // In Seconds
            StanceOkArms = StanceOkDef = true;
            DamageBase = 635f;
            //
            Initialize();
        }
        protected float addMisses;
        protected float addDodges;
        protected float addParrys;
        public float ThunderApps = 0f;
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
        public override float TickSize
        {
            get
            {
                if (!Validated) { return 0f; }

                float DmgBonusBase = (StatS.AttackPower * combatFactors._c_mhItemSpeed) / 14f
                                   + (combatFactors.MH.MaxDamage + combatFactors.MH.MinDamage) / 2f;
                DmgBonusBase *= 0.2f * 6f; // Not sure where the 0.2 * 5 (now 6) was so adding it in now
                float DmgMod = (1f + StatS.BonusBleedDamageMultiplier)
                             * (1f + StatS.BonusDamageMultiplier)
                             * DamageBonus;

                float TickSize = ((DamageBase + DmgBonusBase) * DmgMod) / NumTicks;

                return TickSize;
            }
        }
        public override float GetDPS(float acts)
        {
            float dmgonuse = TickSize;
            float numticks = NumTicks * ((acts + (ThunderApps * AvgTargets)) - addMisses - addDodges - addParrys);
            float result = GetDmgOverTickingTime(acts + ThunderApps) / FightDuration;
            return result;
        }
    }

    public class FakeWhite : Ability
    {
        public static new string SName { get { return "MH White Swing"; } }
        public static new string SDesc { get { return "White Damage"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public FakeWhite(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            Cd = Whiteattacks.MhEffectiveSpeed;
            RageCost = Whiteattacks.MHSwingRage;
            StanceOkArms = StanceOkFury = StanceOkDef = true;
            DamageBase = Whiteattacks.MhDamageOnUse;
            //DamageBonus = (1f + Talents.UnendingFury * 0.02f) * (1f + StatS.BonusWarrior_T7_2P_SlamDamage);
            //BonusCritChance = StatS.BonusWarrior_T9_4P_SLHSCritIncrease;
            //
            Initialize();
            MHAtkTable = Whiteattacks.MHAtkTable;
        }
    }
}
