/**********
 * Owner: Jothay
 **********/
using System;

namespace Rawr.DPSWarr.Skills
{
    #region Instants
    public class MortalStrike : Ability
    {
        public static new string SName { get { return "Mortal Strike"; } }
        public static new string SDesc { get { return "A vicious strike that deals weapon damage plus 380 and wounds the target, reducing the effectiveness of any healing by 50% for 10 sec."; } }
        public static new string SIcon { get { return "ability_warrior_savageblow"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// A vicious strike that deals weapon damage plus 380 and wounds the target, reducing
        /// the effectiveness of any healing by 50% for 10 sec.
        /// <para>Talents: Mortal Strike (Requires Talent), Improved Mortal Strike [+(10-ROUNDUP(10/3*Pts))% Dmg, -(1/3*Pts) Cd]</para>
        /// <para>Glyphs: Glyph of Mortal Strike [+10% Dmg]</para>
        /// <para>Sets: T8 4P [+Crit %]</para>
        /// </summary>
        public MortalStrike(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)CalculationOptionsDPSWarr.Maintenances.MortalStrike_;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            //Targets += StatS.BonusTargets;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = combatFactors.NormalizedMhWeaponDmg + 380f;
            DamageBonus *= 1f + StatS.BonusExecOPMSDamageMultiplier;
            Cd = 6f;// -(Talents.ImprovedMortalStrike / 3f); // In Seconds
            RageCost = 30f;// -(Talents.FocusedRage * 1f);
            DamageBonus = /*(1f + Talents.ImprovedMortalStrike / 3f * 0.1f) * */ (1f + (Talents.GlyphOfMortalStrike ? 0.1f : 0f));
            BonusCritChance = StatS.BonusWarrior_T8_4P_MSBTCritIncrease;
            //
            Initialize();
        }
    }
    public class ColossusSmash : Ability
    {
        public static new string SName { get { return "Colossus Smash"; } }
        public static new string SDesc { get { return "Your melee hits have a (3*Pts)% chance of allowing the use of Execute regardless of the target's Health state. This Execute only uses up to 30 total rage. In addition, you keep at least (3/7/10) rage after using Execute."; } }
        public static new string SIcon { get { return "ability_smash"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Smashes a target for weapon damage plus 120 and weakens their defenses,
        /// allowing your attacks to entirely bypass their armor for 6 sec.
        /// <para>Talents: Sudden Death (Requires Talent) [(5*Pts)% chance on melee hit to reset the cd, keep 5*Pts rage after Exec]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public ColossusSmash(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ColossusSmash_;
            ReqMeleeWeap = ReqMeleeRange = StanceOkArms = StanceOkFury = true;
            RageCost = 30;
            Cd = 20;
            DamageBase = combatFactors.NormalizedMhWeaponDmg + 120f;
            UseReact = true;
            //
            Initialize();
        }
        // Variables
        private static readonly SpecialEffect[/*Talents.SuddenDeath*/] _buff = {
            new SpecialEffect(Trigger.MeleeHit, null, 0f, 0f, 0 * 0.05f),
            new SpecialEffect(Trigger.MeleeHit, null, 0f, 0f, 1 * 0.05f),
            new SpecialEffect(Trigger.MeleeHit, null, 0f, 0f, 2 * 0.05f),
        };
        protected SpecialEffect Buff { get { return _buff[Talents.SuddenDeath]; } }
        // Functions
        public float GetActivates(float landedatksoverdur)
        {
            if (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) { return 0f; }
            float acts = 0f;
            float actsUnderSD = Buff.GetAverageProcsPerSecond(landedatksoverdur / FightDuration, 1f/*MHAtkTable.AnyLand*/, combatFactors._c_mhItemSpeed, FightDuration);
            float actsNormal = FightDuration / Cd;
            acts = Math.Min(actsUnderSD, actsNormal);

            return acts * (1f - Whiteattacks.RageSlip(FightDuration / acts, RageCost));
        }
    }
    public class OverPower : Ability
    {
        public static new string SName { get { return "Overpower"; } }
        public static new string SDesc { get { return "Instantly overpower the enemy, causing weapon damage plus 125. Only usable after the target dodges. The Overpower cannot be blocked, dodged or parried."; } }
        public static new string SIcon { get { return "ability_meleedamage"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Instantly overpower the enemy, causing weapon damage plus 125. Only usable after the target dodges.
        /// The Overpower cannot be blocked, dodged or parried.
        /// <para>Talents: Improved Overpower [+(25*Pts)% Crit Chance],
        /// Unrelenting Assault [-(2*Pts) sec cooldown, +(10*Pts)% Damage.]</para>
        /// <para>Glyphs: Glyph of Overpower [Can proc when parried]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public OverPower(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Overpower_;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            CanBeDodged = false;
            CanBeParried = false;
            CanBeBlocked = false;
            Cd = 5f;// -(2f * Talents.UnrelentingAssault); // In Seconds
            GCDTime = Math.Min(1.5f, Cd);
            RageCost = 5f;// -(Talents.FocusedRage * 1f);
            //Targets += StatS.BonusTargets;
            StanceOkArms = true;
            DamageBase = combatFactors.NormalizedMhWeaponDmg;
            DamageBonus *= 1f + StatS.BonusExecOPMSDamageMultiplier;
            //DamageBonus = 1f +(0.1f * Talents.UnrelentingAssault);
            BonusCritChance = 0.20f * Talents.TasteForBlood;
            BonusCritDamage = 1f + Talents.Impale * 0.1f;
            UseReact = true; // can't plan for this
            //
            Initialize();
        }
        public float GetActivates(float YellowAttacksThatDodgeOverDur, float YellowAttacksThatParryOverDur)
        {
            if (AbilIterater != -1 && !CalcOpts.Maintenance[AbilIterater]) { return 0f; }

            float acts = 0f;
            float LatentGCD = (1.5f + CalcOpts.Latency + (UseReact ? CalcOpts.React / 1000f : CalcOpts.AllowedReact));

            float dodge = Whiteattacks.MHAtkTable.Dodge;
            float parry = (Talents.GlyphOfOverpower ? Whiteattacks.MHAtkTable.Parry : 0f);

            // Chance to activate: Dodges + (if glyphed) Parries
            if (dodge + parry > 0f)
            {
                float WhtHitsOverDur = FightDuration / Whiteattacks.MhEffectiveSpeed
              + (combatFactors.useOH ? FightDuration / Whiteattacks.OhEffectiveSpeed : 0f);

                float dodgesoverDur = 0f
                    + WhtHitsOverDur * (dodge + parry)
                    + (dodge > 0 ? YellowAttacksThatDodgeOverDur : 0)
                    + (parry > 0 ? YellowAttacksThatParryOverDur : 0);

                //acts += Math.Max(0f, dodgesoverDur * (1f - Whiteattacks.AvoidanceStreak));
                acts += Math.Max(0f, dodgesoverDur * (1f - Whiteattacks.RageSlip(FightDuration / dodgesoverDur, RageCost)));
            }

            return acts;
        }
    }
    public class TasteForBlood : Ability
    {
        public static new string SName { get { return "Taste for Blood"; } }
        public static new string SDesc { get { return "Instantly overpower the enemy, causing weapon damage plus 125. Only usable after the target takes Rend Damage. The Overpower cannot be blocked, dodged or parried."; } }
        public static new string SIcon { get { return "ability_rogue_hungerforblood"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Instantly overpower the enemy, causing weapon damage. Only usable after the target takes Rend Damage.
        /// The Overpower cannot be blocked, dodged or parried.
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
            GCDTime = Math.Min(1.5f, 5f /*- (2f * Talents.UnrelentingAssault)*/);
            Cd = 6f; // In Seconds
            RageCost = 5f;// -(Talents.FocusedRage * 1f);
            StanceOkArms = true;
            DamageBase = combatFactors.NormalizedMhWeaponDmg;
            DamageBonus *= 1f + StatS.BonusExecOPMSDamageMultiplier;
            //DamageBonus = 1f + (0.1f * Talents.UnrelentingAssault);
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
        public static new string SDesc { get { return "Instantly Whirlwind up to 4 nearby targets and for the next 6 sec you will perform a whirlwind attack every 1 sec. While under the effects of Bladestorm, you can move but cannot perform any other abilities but you do not feel pity or remorse or fear and you cannot be stopped unless killed."; } }
        public static new string SIcon { get { return "ability_warrior_bladestorm"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Instantly Whirlwind up to 4 nearby targets and for the next 6 sec you will
        /// perform a whirlwind attack every 1 sec. While under the effects of Bladestorm, you can move but cannot
        /// perform any other abilities but you do not feel pity or remorse or fear and you cannot be stopped
        /// unless killed.
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
            DamageBase = WW.DamageBase;
            Cd = 60f - (Talents.GlyphOfBladestorm ? 15f : 0f); // In Seconds
            RageCost = 25f;// -(Talents.FocusedRage * 1f);
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
        public override float DamageOnUseOverride
        {
            get
            {
                if (!Validated) { return 0f; }
                float Damage = WW.DamageOnUseOverride; // WW.DamageOnUseOverride;
                return Damage * 7f; // it WW's 7 times
            }
        }
    }
    public class Execute : Ability
    {
        public static new string SName { get { return "Execute"; } }
        public static new string SDesc { get { return "Attempt to finish off a wounded foe, causing (1456+AP*0.2) damage and converting each extra point of rage into 38 additional damage. Only usable on enemies that have less than 20% health."; } }
        public static new string SIcon { get { return "inv_sword_48"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Attempt to finish off a wounded foe, causing (1456+AP*0.2) damage and converting each
        /// extra point of rage into 38 additional damage. Only usable on enemies that have less
        /// than 20% health.
        /// </summary>
        /// <para>Talents: Improved Execute [Reduces the rage cost of your Execute ability by (2.5/5).]</para>
        /// <para>Glyphs: Glyph of Execute [Your Execute ability acts as if it has 10 additional rage.]</para>
        public Execute(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            Cd = 1.5f;
            RageCost = 15f;// -(Talents.ImprovedExecute * 2.5f);// -(Talents.FocusedRage * 1f);
            DamageBonus *= 1f + StatS.BonusExecOPMSDamageMultiplier;
            FreeRage = 0f;
            StanceOkFury = StanceOkArms = true;
            PercTimeUnder20 = 0.17f;
            //
            Initialize();
        }
        public bool GetReqMeleeWeap() { return this.ReqMeleeWeap; }
        public bool GetReqMeleeRange() { return this.ReqMeleeRange; }
        private float FREERAGE;
        public float FreeRage
        {
            get { return FREERAGE; }
            set
            {
                FREERAGE = value;
                UsedExtraRage = Math.Max(0f, Math.Min(30f, FreeRage));
            }
        }
        public float UsedExtraRage;
        public float PercTimeUnder20;
        protected override float ActivatesOverride { get { return base.ActivatesOverride * PercTimeUnder20; } }
        public override float DamageOverride
        {
            get
            {
                //UsedExtraRage = Math.Max(0f, Math.Min(30f, FreeRage));
                float executeRage = UsedExtraRage;

                float Damage = 1456f + StatS.AttackPower * 0.2f + executeRage * 38f;

                return Damage * AvgTargets;
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
        public static new string SDesc { get { return "Slams the opponent, causing weapon damage plus 250."; } }
        public static new string SIcon { get { return "ability_warrior_decisivestrike"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Slams the opponent, causing weapon damage plus 250.
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
            RageCost = 15f;// -(Talents.FocusedRage * 1f);
            DamageBonus  = 1f + StatS.BonusWarrior_T7_2P_SlamDamage;
            DamageBonus *= 1f + Talents.WarAcademy * 0.05f;
            DamageBonus *= 1f + Talents.ImprovedSlam * 0.10f;
            BonusCritDamage = 1f + Talents.Impale * 0.1f;
            BonusCritChance += Talents.GlyphOfSlam ? 0.05f : 0f;
            CastTime = (1.5f - (Talents.ImprovedSlam * 0.5f)); // In Seconds
            DamageBase = combatFactors.AvgMhWeaponDmgUnhasted + 250f;
            //
            Initialize();
        }
    }
    public class VictoryRush : Ability
    {
        public static new string SName { get { return "Victory Rush"; } }
        public static new string SDesc { get { return "Instantly attack the target causing (AP * 45 / 100) damage and healing you for 20% of your maximum health.  Can only be used within 20 sec after you kill an enemy that yields experience or honor."; } }
        public static new string SIcon { get { return "ability_warrior_devastate"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Instant, No Cd, No Rage, Melee Range, (Battle, Zerker)
        /// Instant attack the target causing 1424 damage. Can only be used within 25 sec after you
        /// kill an enemy that yields experience or honor. Damage is based on your attack power.
        /// </summary>
        /// <para>Talents: </para>
        /// <para>Glyphs: 
        /// Glyph of Victory Rush [+30% Crit Chance @ targs >70% HP]
        /// Glyph of Enduring Victory [+5 sec to length before ability wears off]
        /// </para>
        public VictoryRush(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.VictoryRush_;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            //
            DamageBase = StatS.AttackPower * 45f / 100f;
            DamageBonus = 1f + Talents.WarAcademy * 0.05f;
            HealingBase = StatS.Health * 0.20f; // 20% of Max Health Restored
            //
            Initialize();
        }
        protected override float ActivatesOverride
        {
            get
            {
                float retVal = 0f;
                if (BossOpts.MultiTargs) {
                    float freq = BossOpts.MultiTargsFreq;
                    float acts = FightDuration / freq;
                    retVal = acts;
                }
                return retVal;
            }
        }
    }
    #endregion

    public class Rend : DoT
    {
        public static new string SName { get { return "Rend"; } }
        public static new string SDesc { get { return "Wounds the target causing them to bleed for 380 damage plus an additional (0.2*5*MWB+mwb/2+AP/14*MWS) (based on weapon damage) over 15 sec. If used while your target is above 75% health, Rend does 35% more damage."; } }
        public static new string SIcon { get { return "ability_gouge"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        /// <summary>
        /// Wounds the target causing them to bleed for 380 damage plus an additional
        /// (0.2*5*MWB+mwb/2+AP/14*MWS) (based on weapon damage) over 15 sec. If used while your
        /// target is above 75% health, Rend does 35% more damage.
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
            DamageBase = 380f;
            //
            Initialize();
        }
        protected float addMisses;
        protected float addDodges;
        protected float addParrys;
        public float ThunderApps = 0f;
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
#if RAWR4
                // HOWEVER, with this wonderful new Blood and Thunder talent in Cata, you only need to apply
                // Rend to your initial target, then every time you Thunder Clap, you not only reapply Rend
                // but you also apply it to every target your Thunder Clap hit
#endif
                float result = 0f;
                float Base = base.ActivatesOverride;
#if RAWR4
                if (Talents.BloodAndThunder > 0) {
                    Base = 1f; // Initial Application as the rest is applied by Thunder Clap
                }
#endif
                addMisses = (MHAtkTable.Miss > 0) ? Base * MHAtkTable.Miss : 0f;
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
                float DmgBonusU75 = 0.75f * 1.00f;
                float DmgBonusO75 = 0.25f * 1.35f;
                float DmgMod = (1f + StatS.BonusBleedDamageMultiplier)
                             * (1f + StatS.BonusDamageMultiplier)
                             * DamageBonus;
#if !RAWR4
                float GlyphMOD = Talents.GlyphOfRending ? 7f / 5f : 1f;
#else
                float GlyphMOD = 1f;
#endif

                float damageUnder75 = (DamageBase + DmgBonusBase) * DmgBonusU75;
                float damageOver75 = (DamageBase + DmgBonusBase) * DmgBonusO75;

                float TheDamage = (damageUnder75 + damageOver75) * DmgMod;

                float TickSize = (TheDamage * GlyphMOD) / NumTicks;

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
