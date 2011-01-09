/**********
 * Owner: Jothay
 **********/
using System;

namespace Rawr.DPSWarr.Skills
{
    public sealed class DeepWounds : DoT
    {
        public static new string SName { get { return "Deep Wounds"; } }
        public static new string SDesc { get { return "Your critical strikes cause the opponent to bleed, dealing (16*Pts)% of your melee weapon's average damage over 6 sec."; } }
        public static new string SIcon { get { return "ability_backstab"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 12867; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Your critical strikes cause the opponent to bleed, dealing (16*Pts)% of your 
        /// melee weapon's average damage over 6 sec.
        /// <para>DPSWarrChar.Talents: Deep Wounds (Requires Talent) [(16*Pts)% damage dealt]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public DeepWounds(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            ReqTalent = true;
            Talent2ChksValue = DPSWarrChar.Talents.DeepWounds;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            CanCrit = false;
            Duration = 6f; // In Seconds
            TimeBtwnTicks = 1f; // In Seconds
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            mhActivates = ohActivates = 0f;
            UseHitTable = false;
            UsesGCD = false;
            //
            Initialize();
        }
        private float mhActivates, ohActivates;
        public void SetAllAbilityActivates(float mh, float oh) { mhActivates = mh; ohActivates = oh; }
        protected override float ActivatesOverride { get { return mhActivates + ohActivates; } }
        public override float TickSize
        {
            get
            {
                if (!Validated || ActivatesOverride <= 0f) { return 0f; }

                // Doing it this way because Deep Wounds triggering off of a MH crit
                // and Deep Wounds triggering off of an OH crit do diff damage.
                // Damage stores the average damage of single deep wounds trigger
                float Damage = DPSWarrChar.CombatFactors.NormalizedMHWeaponDmg * (0.16f * DPSWarrChar.Talents.DeepWounds) * mhActivates / ActivatesOverride +
                               DPSWarrChar.CombatFactors.NormalizedOHWeaponDmg * (0.16f * DPSWarrChar.Talents.DeepWounds) * ohActivates / ActivatesOverride;

                Damage *= (1f + DPSWarrChar.StatS.BonusBleedDamageMultiplier);
                Damage *= DPSWarrChar.CombatFactors.DamageBonus;

                // Tick size
                Damage /= Duration * TimeBtwnTicks;

                // Because Deep Wounds is rolling, each tick is compounded by total number of times it's activated over its duration
                Damage *= ActivatesOverride * Duration / FightDuration;

                // Ensure we're not doing negative damage
                return Damage;
            }
        }
        public override float GetDPS(float acts, float perc) { return TickSize; }
        //public override float DPS { get { return TickSize; } }
    }
    #region BuffEffects
    public sealed class BerserkerRage : BuffEffect
    {
        public static new string SName { get { return "Berserker Rage"; } }
        public static new string SDesc { get { return "The warrior enters a berserker rage, becoming immune to Fear, Sap and Incapacitate effects and generating extra tage when taking damage. Lasts 10 sec."; } }
        public static new string SIcon { get { return "spell_nature_ancestralguardian"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 18499; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 30 sec Cd, Self, Any Stance,
        /// Enter a berserker rage, removing and granting immunity to Fear, Sap and Incapacitate effects
        /// and generating extra rage when taking damage. Lasts 10 sec.
        /// <para>DPSWarrChar.Talents: Intensify Rage [-(10*Pts)% Cd]</para>
        /// <para>Glyphs: Glyph of Berserer Rage [+5 Rage Gen]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public BerserkerRage(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            CD = 30f * (1f - 0.10f * DPSWarrChar.Talents.IntensifyRage); // In Seconds
            Duration = 10f;
            RageCost = (DPSWarrChar.Talents.GlyphOfBerserkerRage ? -5f : -1f); // This is reversed in the rotation
            AbilIterater = (int)Maintenance.BerserkerRage;
            StanceOkArms = StanceOkDef = StanceOkFury = true;
            UseHitTable = false;
            Targets = -1;
            UseReact = true;
            UsesGCD = false; // Tested 11/27/2010 with 4.0.3a
            //
            Initialize();
        }
        public new float ActivatesOverride { get { return base.ActivatesOverride; } }
    }
    public sealed class EnragedRegeneration : BuffEffect
    {
        public static new string SName { get { return "Enraged Regeneration"; } }
        public static new string SDesc { get { return "You regenerate 30% of your total health over 10 sec. Can only be used while Enraged."; } }
        public static new string SIcon { get { return "ability_warrior_focusedrage"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 55694; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 3 min Cd, 15 Rage, Self, Any Stance,
        /// You regenerate 30% of your total health over 10 sec. Can only be used while Enraged.
        /// <para>DPSWarrChar.Talents: Field Dressing [+(10*Pts)% Healed]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public EnragedRegeneration(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.EnragedRegeneration;
            CD = 3f * 60f; // In Seconds
            RageCost = 15f;
            Targets = -1;
            StanceOkArms = StanceOkDef = StanceOkFury = true;
            HealingBase = DPSWarrChar.StatS.Health * 0.30f;
            HealingBonus = 1f + DPSWarrChar.Talents.FieldDressing * 0.10f;
            UseHitTable = false;
            UseReact = true;
            //
            Initialize();
        }
    }
    public sealed class LastStand : BuffEffect
    {
        public static new string SName { get { return "Last Stand"; } }
        public static new string SDesc { get { return "Temporarily grants you 30% of your maximum health for 20 sec. After the effect expires, the health is lost."; } }
        public static new string SIcon { get { return "spell_holy_ashestoashes"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 12975; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 5 min Cd, Self, (Def)
        /// Temporarily grants you 30% of your maximum health for 20 sec.  After the effect expires, the health is lost.
        /// <para>DPSWarrChar.Talents: Last Stand [Requires Talent]</para>
        /// <para>Glyphs: Glyph of Last Stand [-1 min Cd]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public LastStand() { Targets = -1; }
    }
    public sealed class BattleShout : BuffEffect
    {
        public static new string SName { get { return "Battle Shout"; } }
        public static new string SDesc { get { return "The warrior shouts, increasing the Strength and Agility of all raid and party members within 30 yards by 549 and gaining 20 rage. Lasts 2 min."; } }
        public static new string SIcon { get { return "ability_warrior_battleshout"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 6673; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// The warrior shouts, increasing attack power of all raid and party members within 20 yards by 549 and gaining 20 rage. Lasts 2 min.
        /// <para>DPSWarrChar.Talents: Booming Voice [-15*Pts Cd, +5*Pts Rage Gen]</para>
        /// <para>Glyphs: Glyph of Battle [+1*Pts min duration, +(25*Pts)% AoE]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public BattleShout(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.BattleShout;
            MaxRange = 30f; // In Yards 
            Duration = (2f + (DPSWarrChar.Talents.GlyphOfBattle ? 2f : 0f)) * 60f;
            RageCost = -1f * (20f + DPSWarrChar.Talents.BoomingVoice * 5f);
            CD = 60f - DPSWarrChar.Talents.BoomingVoice * 15f;
            Targets = -1;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            UseHitTable = false;
            //
            Initialize();
        }
    }
    public sealed class CommandingShout : BuffEffect
    {
        public static new string SName { get { return "Commanding Shout"; } }
        public static new string SDesc { get { return "Increases Stamina of all party and raid members within 30 yards by 584 and gaining 20 rage. Lasts 2 min."; } }
        public static new string SIcon { get { return "ability_warrior_rallyingcry"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 469; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Increases Stamina of all party and raid members within 30 yards by 584 and gaining 20 rage. Lasts 2 min.
        /// <para>DPSWarrChar.Talents: Booming Voice [+(25*Pts)% AoE and Duration], Commanding Presence [+(5*Pts)% to the Health Bonus]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public CommandingShout(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.CommandingShout;
            MaxRange = 30f * (1f + (DPSWarrChar.Talents.GlyphOfCommand ? 0.50f : 0f)); // In Yards 
            Duration = (2f + (DPSWarrChar.Talents.GlyphOfCommand ? 2f : 0f)) * 60f;
            RageCost = -1f * (20f + DPSWarrChar.Talents.BoomingVoice * 5f);
            CD = 60f - DPSWarrChar.Talents.BoomingVoice * 15f;
            Targets = -1;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            UseHitTable = false;
            //
            Initialize();
        }
    }
    public sealed class DeathWish : BuffEffect
    {
        public static new string SName { get { return "Death Wish"; } }
        public static new string SDesc { get { return "When activated you become enraged, increasing your physical damage by 20% but increasing all damage taken by 5%. Lasts 30 sec."; } }
        public static new string SIcon { get { return "spell_shadow_deathpact"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 12292; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// When activated you become enraged, increasing your physical damage by 20% but increasing
        /// all damage taken by 5%. Lasts 30 sec.
        /// <para>DPSWarrChar.Talents: Death Wish [Requires Talent], Intensify Rage [-(10*Pts)% CD]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public DeathWish(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.DeathWish;
            ReqTalent = true;
            Talent2ChksValue = DPSWarrChar.Talents.DeathWish;
            CD = 3f * 60f * (1f - 0.10f * DPSWarrChar.Talents.IntensifyRage); // In Seconds
            Duration = 30f;
            Targets = -1;
            RageCost = 10f;
            StanceOkFury = true;
            UseHitTable = false;
            //
            Initialize();
        }
    }
    public sealed class Recklessness : BuffEffect
    {
        public static new string SName { get { return "Recklessness"; } }
        public static new string SDesc { get { return "Your next 3 special ability attacks have an additional 100% chance to critically hit but all damage taken is increased by 20%. Lasts 12 sec."; } }
        public static new string SIcon { get { return "ability_criticalstrike"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 1719; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Your next 3 special ability attacks have an additional 100% chance to critically hit
        /// but all damage taken is increased by 20%. Lasts 12 sec.
        /// <para>DPSWarrChar.Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Recklessness(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.Recklessness;
            CD = (5f * 60f) * (1f - 0.10f * DPSWarrChar.Talents.IntensifyRage); // In Seconds
            Duration = 12f; // In Seconds
            StanceOkFury = true;
            Targets = -1;
            //Effect = new SpecialEffect(Trigger.Use, new Stats { PhysicalCrit = 1f, DamageTakenMultiplier = 0.20f, }, Duration, Cd);
            UseHitTable = false;
            Initialize();
        }
    }
    public sealed class SweepingStrikes : BuffEffect
    {
        public static new string SName { get { return "Sweeping Strikes"; } }
        public static new string SDesc { get { return "Your melee attacks strike an additional nearby opponent. Lasts 10 sec."; } }
        public static new string SIcon { get { return "ability_rogue_slicedice"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 12328; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Your melee attacks strike an additional nearby opponent. Lasts 10 sec.
        /// <para>DPSWarrChar.Talents: Sweeping Strikes [Requires Talent]</para>
        /// <para>Glyphs: Glyph of Sweeping Strikes [-100% RageCost]</para>
        /// <para>Sets: none</para>
        /// <remarks>
        /// Notes from wowhead site:
        /// * Sweeping Strikes hits cannot be dodged, missed or parried.
        /// * SS is off the GCD, so you can macro it into pretty much every skill to have 5 free hits every 30
        ///   seconds. Just make a macro, name it something, and dump /cast sweeping strikes into it somewhere,
        ///   along with whatever other ability you would normally use
        /// * Sweeping Strikes does not trigger the GCD.
        /// </remarks>
        /// </summary>
        public SweepingStrikes(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.SweepingStrikes;
            ReqTalent = true; Talent2ChksValue = DPSWarrChar.Talents.SweepingStrikes;
            ReqMeleeWeap = ReqMeleeRange = true;
            StanceOkArms = true;
            ReqMultiTargs = true;
            Targets = -1;
            CD = 60f;
            Duration = 10f;
            RageCost = (DPSWarrChar.Talents.GlyphOfSweepingStrikes ? -1f : 30f);
            UseHitTable = false;
            UsesGCD = false;
            //
            Initialize();
        }
        protected override float ActivatesOverride
        {
            get {
                // We should only be activating this when MultiTargets occurs
                if (!DPSWarrChar.BossOpts.MultiTargs || DPSWarrChar.BossOpts.Targets.Count < 1) { return 0f; }
                // First, let's determine our cap
                float result = 0f, cap = base.ActivatesOverride;
                // Next, let's figure out how often we see MultiTargs
                foreach (TargetGroup tg in DPSWarrChar.BossOpts.Targets)
                {
                    if (tg.Validated) {
                        result += FightDuration / tg.Frequency;
                    }
                }
                // Finally, let's return the result, limited by the cap
                return Math.Min(cap, result);
            }
        }
    }
    public sealed class SecondWind : BuffEffect
    {
        public static new string SName { get { return "Second Wind"; } }
        public static new string SDesc { get { return "Whenever you are struck by a Stun of Immoblize effect you will generate 10*Pts Rage and (5*Pts)% of your total health over 10 sec."; } }
        public static new string SIcon { get { return "ability_hunter_harass"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 29838; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Whenever you are struck by a Stun of Immoblize effect you will generate
        /// 10*Pts Rage and (5*Pts)% of your total health over 10 sec.
        /// <para>DPSWarrChar.Talents: Sweeping Strikes [Requires Talent]</para>
        /// <para>Glyphs: Glyph of Sweeping Strikes [-100% Rage cost]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public SecondWind(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            //AbilIterater = -1f;
            ReqTalent = true;
            Talent2ChksValue = DPSWarrChar.Talents.SecondWind;
            CD = -1f;
            Targets = -1;
            NumStunsOverDur = 0f;
            Duration = 10f; // Using 4 seconds to sim consume time
            RageCost = -10f * DPSWarrChar.Talents.SecondWind;
            StanceOkDef = StanceOkFury = StanceOkArms = true;
            HealingBase = DPSWarrChar.StatS.Health * 0.025f * DPSWarrChar.Talents.SecondWind;
            HealingBonus = 1f + DPSWarrChar.Talents.FieldDressing * 0.10f;
            UseHitTable = false;
            UsesGCD = false;
            //
            Initialize();
        }
        private float _numStunsOverDur;
        public float NumStunsOverDur
        {
            get { return _numStunsOverDur; }
            set
            {
                _numStunsOverDur = value;
                CD = (FightDuration / _numStunsOverDur);
                if (Effect != null) { Effect.Cooldown = CD; }
            }
        }
    }
    public sealed class DeadlyCalm : BuffEffect
    {
        public static new string SName { get { return "Deadly Calm"; } }
        public static new string SDesc { get { return "For the next 10 sec, none of your abilities cost rage, but you continue to generate rage. Cannot be used during Inner Rage."; } }
        public static new string SIcon { get { return "achievement_boss_kingymiron"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 85730; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 2 min Cd, Self, Any Stance,
        /// For the next 10 sec, none of your abilities cost rage, but you
        /// continue to generate rage. Cannot be used during Inner Rage.
        /// <para>DPSWarrChar.Talents: Deadly Calm [Requires Talent]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public DeadlyCalm(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            CD = 2 * 60f; // In Seconds
            Duration = 10f;
            RageCost = -1f;
            AbilIterater = (int)Maintenance.DeadlyCalm;
            ReqTalent = true; Talent2ChksValue = DPSWarrChar.Talents.DeadlyCalm;
            StanceOkArms = StanceOkDef = StanceOkFury = true;
            UseHitTable = false;
            Targets = -1;
            UseReact = true;
            IsMaint = false; // Intentional so we can handle it against Inner Rage
            UsesGCD = false; // Tested 11/27/2010 with 4.0.3a
            //
            Initialize();
        }
    }
    public sealed class InnerRage : BuffEffect
    {
        public static new string SName { get { return "Inner Rage"; } }
        public static new string SDesc { get { return "Usable when you have over 75 Rage to increase all damage you deal by 15% but increase the rage cost of all abilities by 50%. Lasts 15 sec."; } }
        public static new string SIcon { get { return "warrior_talent_icon_innerrage"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 1134; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 1.5 sec Cd
        /// Usable when you have over 75 Rage to increase all damage you deal by 15%
        /// but increase the rage cost of all abilities by 50%. Lasts 15 sec.
        /// <para>DPSWarrChar.Talents: Deadly Calm [Requires Talent]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public InnerRage(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            CD = 1.50f; // In Seconds
            Duration = 15f;
            RageCost = -1f;
            AbilIterater = (int)Maintenance.InnerRage;
            StanceOkArms = StanceOkDef = StanceOkFury = true;
            UseHitTable = false;
            Targets = -1;
            UseReact = true;
            IsMaint = false; // Intentional so we can handle it against Deadly Calm and how often we have tons of rage
            UsesGCD = false;
            Effect = effect;
            //
            Initialize();
        }
        // Trigger isn't really MortalStrikeHit, it's RageGoesAbove75
        private static SpecialEffect effect = new SpecialEffect(Trigger.MortalStrikeHit, new Base.StatsWarrior() { BonusDamageMultiplier = 0.15f, RageCostMultiplier = 0.50f }, 15, 1.5f);
        private float FREERAGEO20 = -1f, FREERAGEU20 = -1f;
        public float FreeRageO20 { get { return FREERAGEO20; } set { FREERAGEO20 = Math.Max(0f, value); } } // Must be above zero to prevent other calc problems
        public float FreeRageU20 { get { return FREERAGEU20; } set { FREERAGEU20 = Math.Max(0f, value); } } // Must be above zero to prevent other calc problems
        public float GetActivates(float repassAvailRage, float perc) {
            if (repassAvailRage < 0) { return 0f; }
            float avgFreeRagePerSec = repassAvailRage / (FightDuration * perc);
            //float origprocs = 0f;
            float procs = Math.Min((FightDuration * perc) / Duration,
                                        /*origprocs = */(Effect.GetAverageProcsPerSecond(CD, Math.Max(0f, Math.Min(1f, avgFreeRagePerSec * 0.025f)), 3, (FightDuration * perc)) * (FightDuration * perc)));

            /*// Over 20
            float avgFreeRageO20 = FreeRageO20 / FightDurationO20;
            float origprocsO20 = 0f;
            float procsO20 = avgFreeRageO20 > 0
                            ? Math.Min(FightDurationO20 / Duration,
                                        origprocsO20 = (Effect.GetAverageProcsPerSecond(Cd, Math.Max(0f, Math.Min(1f, avgFreeRageO20 * 0.25f)), 3, FightDurationO20) * FightDurationO20))
                            : 0f;
            // Under 20 (Guess based on O20 if we don't have it yet)
            float origprocsU20 = 0f;
            float guessAvgFreeRageU20 = FreeRageU20 == -1f ? (avgFreeRageO20 / (FightDurationO20 / FightDuration)) * (FightDurationU20 / FightDuration) : 0f;
            float avgFreeRageU20 = (CalcOpts.M_ExecuteSpam && FreeRageU20 != 0) ? (FreeRageU20 == -1 ? guessAvgFreeRageU20 : FreeRageU20 / FightDurationU20) : 0f;
            float procsU20 = CalcOpts.M_ExecuteSpam && avgFreeRageU20 > 0
                            ? Math.Min(FightDurationU20 / Duration,
                                        origprocsU20 = (Effect.GetAverageProcsPerSecond(Cd, Math.Max(0f, Math.Min(1f, avgFreeRageU20 * 0.25f)), 3, FightDurationU20) * FightDurationU20))
                            : 0f;*/
            // Results
            return procs/*procsO20 /*+ procsU20*/;
        }
        protected override float ActivatesOverride {
            get {
                // This ability can only be activated at >75 Rage so we need to limit its chance to activate to that
                if (FreeRageO20 > 0f || FreeRageU20 > 0f) {
                    // Over 20
                    float avgFreeRageO20 = FreeRageO20 / FightDurationO20;
                    //float origprocsO20 = 0f;
                    float procsO20 = avgFreeRageO20 > 0
                                    ? Math.Min(FightDurationO20 / Duration,
                                                /*origprocsO20 =*/ (Effect.GetAverageProcsPerSecond(CD, Math.Max(0f, Math.Min(1f, avgFreeRageO20 * 0.25f)), 3, FightDurationO20) * FightDurationO20))
                                    : 0f;
                    // Under 20 (Guess based on O20 if we don't have it yet)
                    //float origprocsU20 = 0f;
                    float guessAvgFreeRageU20 = FreeRageU20 == -1f ? (avgFreeRageO20 / (FightDurationO20 / FightDuration)) * (FightDurationU20 / FightDuration) : 0f;
                    float avgFreeRageU20 = (DPSWarrChar.CalcOpts.M_ExecuteSpam && FreeRageU20 != 0) ? (FreeRageU20 == -1 ? guessAvgFreeRageU20 : FreeRageU20 / FightDurationU20) : 0f;
                    float procsU20 = DPSWarrChar.CalcOpts.M_ExecuteSpam && avgFreeRageU20 > 0
                                    ? Math.Min(FightDurationU20 / Duration,
                                                /*origprocsU20 =*/ (Effect.GetAverageProcsPerSecond(CD, Math.Max(0f, Math.Min(1f, avgFreeRageU20 * 0.25f)), 3, FightDurationU20) * FightDurationU20))
                                    : 0f;
                    // Results
                    return procsO20 + procsU20;
                } else { return 0; }
            }
        }
        public float GetUptime(float acts) {
            float retVal = 0f;
            if (acts > FightDuration / Duration)
            {
                // We have 100% uptime
                retVal = 1f;
            }
            else if (acts > 0f)
            {
                // we have less than 100% uptime
                retVal = (acts * Duration) / FightDuration;
            }
            return retVal;
        }
    }
    #endregion
    #region DeBuff Abilities
    public sealed class ThunderClap : BuffEffect
    {
        public static new string SName { get { return "Thunder Clap"; } }
        public static new string SDesc { get { return "Blasts enemies within 8 yards for 303 damage, and increases the time between their attacks by 20% for 30 sec. Damage increased by attack power."; } }
        public static new string SIcon { get { return "spell_nature_thunderclap"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 6343; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Blasts enemies within 8 yards for 303 damage, and increases the time between
        /// their attacks by 20% for 30 sec. Damage increased by attack power.
        /// <para>TODO: BonusCritDamage to 2x instead of 1.5x as it's now considered a ranged attack (3.3.3 notes) other parts implemented already</para>
        /// <para>DPSWarrChar.Talents: none</para>
        /// <para>Glyphs: Glyph of Thunder Clap [+2 yds MaxRange], Glyph of Resonating Power [-5 RageCost]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public ThunderClap(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.ThunderClap;
            ReqMeleeRange = StanceOkArms = StanceOkDef = true;
            IsMaint = false;
            Targets = 10;
            MaxRange = 5f + (DPSWarrChar.Talents.GlyphOfThunderClap ? 2f : 0f); // In Yards 
            CD = 6f; // In Seconds
            Duration = 30f; // In Seconds
            RageCost = 20f - (DPSWarrChar.Talents.GlyphOfResonatingPower ? 5f : 0f);
            DamageBase = 303f + DPSWarrChar.StatS.AttackPower * 0.12f;
            UseRangedHit = true;
            CanBeDodged = CanBeParried = false;
            //
            Initialize();
        }
        protected override float ActivatesOverride
        {
            get
            {
                /* We have 3 different things that would cause us to want to Thunderclap
                 * - Maintaining the Debuff, must be refreshed every 30 seconds
                 * - Multiple Mobs showing up, so we can damage all of them at once, based on BossHandler
                 * - Blood and Thunder Talent lets us refresh Rend duration with Thunderclap (YAY!), must
                 *   be done every 15-1=14 seconds. If it falls off then it's no good.
                 * Basically, we'll just take the total of them up to the most that's possible,
                 * which is no more than every 6 sec.
                 */

                float cap = 0, fromBuff = 0, fromMultiTargs = 0, fromBnT = 0;
                float LatentGCD = 1.5f + DPSWarrChar.CalcOpts.Latency + DPSWarrChar.CalcOpts.AllowedReact;
                float GCDPerc = LatentGCD / (CD + DPSWarrChar.CalcOpts.Latency + DPSWarrChar.CalcOpts.AllowedReact);
                //float maxTCpersec = 0f;

                {
                    // First, let's see our Cap
                    float result = 0f;
                    //
                    result = Math.Max(0f, FightDuration / (LatentGCD / GCDPerc));
                    //
                    cap = result;
                    //maxTCpersec = cap / FightDuration;
                }

                {
                    // Second, let's do Blood n Thunder
                    // But remember, we're already doing the Buff Maintenance and
                    // MultiMobs, so we may not need to do this all that much
                    // Oh, and we need the addMisses again, because we need to force it up
                    float result = 0f;
                    //
                    GCDPerc = LatentGCD / (14f + DPSWarrChar.CalcOpts.Latency + DPSWarrChar.CalcOpts.AllowedReact); // 15-1=14
                    result = Math.Max(0f, FightDuration / (LatentGCD / GCDPerc));
                    addMisses = (MHAtkTable.Miss > 0) ? result * MHAtkTable.Miss : 0f;
                    result += addMisses;
                    //
                    fromBnT = result;
                }

                {
                    // Third, lets do Buff Maintenance
                    /* Since this has to be up according to Debuff Maintenance Rules
                     * this override has the intention of taking the baseline
                     * activates and forcing the char to use TC again to ensure it's up
                     * in the event that the attempted activate didn't land (it Missed)
                     * We're only doing the additional check once so it will at most do this
                     * twice in a row, may consider doing a settler*/
                    float result = 0f;
                    //
                    float Base = base.ActivatesOverride;
                    addMisses = (MHAtkTable.Miss > 0) ? Base * MHAtkTable.Miss : 0f;
                    result = Base + addMisses;
                    //
                    fromBuff = Math.Max(0f, result - fromBnT);
                }

                {
                    // Fourth, let's do MultiMobs
                    float result = 0f;
                    //
                    foreach (TargetGroup g in DPSWarrChar.BossOpts.Targets)
                    {
                        if (g.NumTargs > 0 && g.Frequency != -1 && g.Chance > 0f && g.Duration > 0) { // Validate it first
                            result += (g.Duration / 1000f) / (LatentGCD / GCDPerc) * (FightDuration / g.Frequency);
                        }
                    }
                    //
                    fromMultiTargs = Math.Max(0f, result - fromBnT - fromBuff);
                }

                /*float composite = 0;
                {
                    // Now, obviously these activates are gonna all impact each other, but how?
                    float freqFromBuff = FightDuration / fromBuff;
                    float freqFromBnT = FightDuration / fromBnT;
                    float freqFromMultiTargs = FightDuration / fromMultiTargs;

                    float perSecFromBuff = freqFromBuff / FightDuration;
                    float perSecFromBnT = fromBnT / FightDuration;
                    float perSecFromMultiTargs = fromMultiTargs / FightDuration;

                    composite = perSecFromBuff * perSecFromBnT * perSecFromMultiTargs;
                }*/

                GCDPerc = LatentGCD / (CD + DPSWarrChar.CalcOpts.Latency + DPSWarrChar.CalcOpts.AllowedReact);
                return Math.Min(cap, fromBuff + fromBnT + fromMultiTargs) * (1f - DPSWarrChar.Whiteattacks.RageSlip(LatentGCD / GCDPerc, RageCost));
            }
        }
    }
    public sealed class SunderArmor : BuffEffect
    {
        public static new string SName { get { return "Sunder Armor"; } }
        public static new string SDesc { get { return "Sunders the target's armor, reducing it by 4% per Sunder Armor and causes a high amount of threat. Threat increased by attack power. Can be applied up to 5 times. Lasts 30 sec."; } }
        public static new string SIcon { get { return "ability_warrior_sunder"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 7386; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Sunders the target's armor, reducing it by 4% per Sunder Armor and causes a high amount of threat.
        /// Threat increased by attack power. Can be applied up to 5 times. Lasts 30 sec.
        /// <para>DPSWarrChar.Talents: Focused Rage [-(Pts) Rage Cost], Puncture [-(Pts) Rage Cost], </para>
        /// <para>Glyphs: Glyph of Sunder Armor [+1 Targets]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public SunderArmor(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.SunderArmor;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            Duration = 30f; // In Seconds
            CD = 1.5f;
            CanCrit = false;
            RageCost = 15f * (1f - (DPSWarrChar.Talents.GlyphOfFuriousSundering ? 0.50f : 0f));
            Targets = 1f + (DPSWarrChar.Talents.GlyphOfSunderArmor ? 1f : 0f);
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            //
            Initialize();
        }
        protected override float ActivatesOverride
        {
            get
            {
                // Since this has to be up according to Debuff Maintenance Rules
                // this override has the intention of taking the baseline
                // activates and forcing the char to use Rend again to ensure it's up
                // in the event that the attemtped activate didn't land (it Missed, was Dodged or Parried)
                // We're only doing the additional check once so it will at most do this
                // twice in a row, may consider doing a settler
                float result = 0f;
                float Base = base.ActivatesOverride;

                // Glyph of Colossus Smash makes that ability refresh Sunder, so we no longer need to
                // spend tons of GCDs Maintaining Sunder Armor, just the initial 3 to stack it up
                if (DPSWarrChar.Talents.GlyphOfColossusSmash && DPSWarrChar.CalcOpts.M_ColossusSmash) { Base = 3f; }

                // If we aren't using Colossus Smash to refresh Sunder, then we can keep using Heroic Throw, so lets discount more
                if (!DPSWarrChar.Talents.GlyphOfColossusSmash && DPSWarrChar.Talents.GlyphOfHeroicThrow) {
                    Base -= FightDuration / (60f - DPSWarrChar.Talents.GagOrder * 15f);
                }
                // Glyph of Heroic Throw lets Heroic Throw apply a stack of Sunder Armor, so we can spend one less with Sunder itself
                else if (DPSWarrChar.Talents.GlyphOfHeroicThrow && Base >= 1f) { Base -= 1f; }

                // Make sure we didn't accidentally go negative
                Base = Math.Max(0f, Base);

                addMisses = (MHAtkTable.Miss > 0) ? Base * MHAtkTable.Miss : 0f;
                addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                result = Base + addMisses + addDodges + addParrys;

                return result;
            }
        }
    }
    public sealed class ShatteringThrow : BuffEffect
    {
        public static new string SName { get { return "Shattering Throw"; } }
        public static new string SDesc { get { return "Throws your weapon at the enemy causing (12+AP*0.50) damage (based on attack power), reducing the armor on the target by 20% for 10 sec or removing any invulnerabilities."; } }
        public static new string SIcon { get { return "ability_warrior_shatteringthrow"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 64382; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Throws your weapon at the enemy causing (12+AP*0.50) damage (based on attack power),
        /// reducing the armor on the target by 20% for 10 sec or removing any invulnerabilities.
        /// </summary>
        public ShatteringThrow(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.ShatteringThrow;
            ReqMeleeWeap = true;
            ReqMeleeRange = false;
            MaxRange = 30f; // In Yards
            //Targets += DPSWarrChar.StatS.BonusTargets;
            CD = 5f * 60f; // In Seconds
            Duration = 10f;
            CastTime = 1.5f; // In Seconds
            RageCost = 25f;
            StanceOkArms = true;
            DamageBase = 12f + DPSWarrChar.StatS.AttackPower * 0.50f;
            UseRangedHit = true;
            //
            Initialize();
        }
    }
    public sealed class DemoralizingShout : BuffEffect
    {
        public static new string SName { get { return "Demoralizing Shout"; } }
        public static new string SDesc { get { return "Reduces the physical damage caused by all enemies within 10 yards by 10% for 30 sec."; } }
        public static new string SIcon { get { return "ability_warrior_warcry"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 1160; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Reduces the physical damage caused by all enemies within 10 yards by 10% for 30 sec.
        /// <para>DPSWarrChar.Talents: Drums of War [-50% Rage Cost/Pt]</para>
        /// <para>Glyphs: Demo Shout [+15s Dur, +50% AoE]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public DemoralizingShout(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.DemoralizingShout;
            ReqMeleeWeap = ReqMeleeRange = false;
            MaxRange = 10f * (1f + (DPSWarrChar.Talents.GlyphOfDemoralizingShout ? 0.50f : 0f)); // In Yards 
            Duration = 30f + (DPSWarrChar.Talents.GlyphOfDemoralizingShout ? 15f : 0f); // Booming Voice doesn't boost this shout
            RageCost = TalentsAsSpecialEffects.DrumsOfWarRageCosts[DPSWarrChar.Talents.DrumsOfWar]; // Drums of War negates rage cost
            StanceOkArms = StanceOkFury = true;
            UseSpellHit = true;
            //
            Initialize();
        }
        protected override float ActivatesOverride
        {
            get
            {
                // Since this has to be up according to Debuff Maintenance Rules
                // this override has the intention of taking the baseline
                // activates and forcing the char to use Demo Shout again to ensure it's up
                // in the event that the attemtped activate didn't land (it Missed)
                // We're only doing the additional check once so it will at most do this
                // twice in a row, may consider doing a settler
                float Base = base.ActivatesOverride;

                addMisses = (MHAtkTable.Miss > 0) ? Base * MHAtkTable.Miss : 0f;

                float result = Base + addMisses + addDodges + addParrys;

                return result;
            }
        }
    }
    public sealed class Hamstring : BuffEffect
    {
        public static new string SName { get { return "Hamstring"; } }
        public static new string SDesc { get { return "Instant, No cd, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker) Maims the enemy, reducing movement speed by 50% for 15 sec."; } }
        public static new string SIcon { get { return "ability_shockwave"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 1715; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, No cd, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker)
        /// Maims the enemy, reducing movement speed by 50% for 15 sec.
        /// <para>DPSWarrChar.Talents: Improved Hamstring [Gives a [5*Pts]% chance to immobilize the target for 5 sec.]</para>
        /// <para>Glyphs: Glyph of Hamstring [Gives a 10% chance to immobilize the target for 5 sec.]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Hamstring(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Maintenance.Hamstring;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            Duration = 15f; // In Seconds
            RageCost = 10f;// -(DPSWarrChar.Talents.FocusedRage * 1f);
            //Targets += DPSWarrChar.StatS.BonusTargets;
            StanceOkFury = StanceOkArms = true;
            //Effect = new SpecialEffect(Trigger.Use, new Stats() { AttackPower = 0f, /*TargetMoveSpeedReducPerc = 0.50f,*/ }, Duration, Duration);
            //float Chance = DPSWarrChar.Talents.ImprovedHamstring * 0.05f + (DPSWarrChar.Talents.GlyphOfHamstring ? 0.10f : 0.00f);
            //Effect2 = new SpecialEffect(Trigger.Use, new Stats() { AttackPower = 0f, /*TargetStunned = 0.50f,*/ }, 5f, Duration, Chance);
            //
            Initialize();
        }
        protected override float ActivatesOverride
        {
            get
            {
                // Since this has to be up according to Debuff Maintenance Rules
                // this override has the intention of taking the baseline
                // activates and forcing the char to use Rend again to ensure it's up
                // in the event that the attemtped activate didn't land (it Missed, was Dodged or Parried)
                // We're only doing the additional check once so it will at most do this
                // twice in a row, may consider doing a settler
                float result = 0f;
                float Base = base.ActivatesOverride;
                addMisses = (MHAtkTable.Miss > 0) ? Base * MHAtkTable.Miss : 0f;
                addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                result = Base + addMisses + addDodges + addParrys;

                return result;
            }
        }
    }
    #endregion
    #region Anti-Debuff Abilities
    public sealed class EveryManForHimself : Ability
    {
        public static new string SName { get { return "Every Man for Himself"; } }
        public static new string SDesc { get { return "Removes all movement impairing effects and all effects which cause loss of control of your character. This effect shares a cooldown with other similar effects."; } }
        public static new string SIcon { get { return "spell_shadow_charm"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 59752; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 2 Min Cooldown, 0 Rage, Self (Any)
        /// Removes all movement impairing effects and all effects which cause loss of control of
        /// your character. This effect shares a cooldown with other similar effects.
        /// <para>DPSWarrChar.Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public EveryManForHimself(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            CD = 2f * 60f;
            StanceOkArms = StanceOkFury = StanceOkDef = true;
            UseHitTable = false;
            UseReact = true;
            UsesGCD = false; // Comments say no, but I need to verify in game
            Targets = -1;
            //
            Initialize();
        }
    }
    public sealed class EscapeArtist : Ability
    {
        public static new string SName { get { return "Escape Artist"; } }
        public static new string SDesc { get { return "Escape the effects of any immobilization or movement speed reduction effect."; } }
        public static new string SIcon { get { return "ability_rogue_trip"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 20589; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 1.75 Min Cooldown, 0 Rage, Self (Any)
        /// Escape the effects of any immobilization or movement speed reduction effect.
        /// <para>DPSWarrChar.Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public EscapeArtist(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            CD = 105f;
            StanceOkArms = StanceOkFury = StanceOkDef = true;
            UseHitTable = false;
            UseReact = true;
            UsesGCD = false; // Comments say no, but I need to verify in game
            Targets = -1;
            //
            Initialize();
        }
    }
    public sealed class WillOfTheForsaken : Ability
    {
        public static new string SName { get { return "Will of the Forsaken"; } }
        public static new string SDesc { get { return "Removes any Charm, Fear and Sleep effect. This effect shares a 30 sec cooldown with other similar effects."; } }
        public static new string SIcon { get { return "spell_shadow_raisedead"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 20589; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 2 Min Cooldown, 0 Rage, Self (Any)
        /// Removes any Charm, Fear and Sleep effect. This effect shares a 30 sec cooldown with other similar effects.
        /// <para>DPSWarrChar.Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public WillOfTheForsaken(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            CD = 2f * 60f;
            StanceOkArms = StanceOkFury = StanceOkDef = true;
            UseHitTable = false;
            UseReact = true;
            UsesGCD = false; // Comments say no, but I need to verify in game
            Targets = -1;
            //
            Initialize();
        }
    }
    public sealed class HeroicFury : Ability
    {
        public static new string SName { get { return "Heroic Fury"; } }
        public static new string SDesc { get { return "Removes any Immobilization effects and refreshes the cooldown of your Intercept ability."; } }
        public static new string SIcon { get { return "warrior_talent_icon_deadlycalm"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 60970; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 30 sec Cd, 0 Rage, Self (Any)
        /// Removes any Immobilization effects and refreshes the cooldown of your Intercept ability.
        /// </summary>
        /// <para>DPSWarrChar.Talents: </para>
        /// <para>Glyphs: </para>
        public HeroicFury(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            ReqTalent = true;
            Talent2ChksValue = DPSWarrChar.Talents.HeroicFury;
            CD = 30f;
            Targets = -1;
            StanceOkArms = StanceOkFury = StanceOkDef = true;
            UseHitTable = false;
            UseReact = true;
            UsesGCD = false;
            //
            Initialize();
        }
    }
    #endregion
    #region Movement Abilities
    public sealed class Charge : Ability
    {
        public static new string SName { get { return "Charge"; } }
        public static new string SDesc { get { return "Charge an enemy, generate 15 rage, and stun it for 1 sec. Cannot be used in combat."; } }
        public static new string SIcon { get { return "ability_warrior_charge"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 100; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 15 sec cd, 0 Rage, 8-25 yds, (Battle)
        /// Charge an enemy, generate 15 rage, and stun it for 1 sec. Cannot be used in combat.
        /// <para>DPSWarrChar.Talents: Blitz [+5 RageGen], Warbringer [Usable in combat and any stance], Juggernaut [Usable in combat]</para>
        /// <para>Glyphs: Glyph of Rapid Charge [-7% Cd], Glyph of Charge [+5 yds MaxRange]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Charge(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            MinRange = 8f;
            MaxRange = 25f + (DPSWarrChar.Talents.GlyphOfLongCharge ? 5f : 0f); // In Yards 
            RageCost = -(15f + (DPSWarrChar.Talents.Blitz * 5f));
            float cdi = DPSWarrChar.Talents.Juggernaut * 5f;
            if (DPSWarrChar.CalcOpts.PtrMode) { cdi = 0f; }
            CD = (15f + cdi); // In Seconds
            if (DPSWarrChar.CalcOpts.PtrMode && DPSWarrChar.Talents.GlyphOfRapidCharge) {
                CD -= 1f;
            } else if (DPSWarrChar.Talents.GlyphOfRapidCharge) {
                CD *= 1f - (DPSWarrChar.Talents.GlyphOfRapidCharge ? 0.07f : 0f);
            }
            float di = 0f;
            if (DPSWarrChar.CalcOpts.PtrMode && DPSWarrChar.Talents.Juggernaut > 0) { di = 2f; }
            Duration = 1.5f + di;
            Targets = -1;
            if (DPSWarrChar.Talents.Warbringer == 1) {
                StanceOkArms = StanceOkFury = StanceOkDef = true;
            } else if (DPSWarrChar.Talents.Juggernaut == 1) {
                StanceOkArms = true;
            }
            //
            Initialize();
        }
    }
    public sealed class Intercept : Ability
    {
        public static new string SName { get { return "Intercept"; } }
        public static new string SDesc { get { return "Charge an enemy, causing (AP*0.12) damage (based on attack power) and stunning it for 3 sec."; } }
        public static new string SIcon { get { return "ability_rogue_sprint"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 20252; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Zerker)
        /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
        /// <para>DPSWarrChar.Talents: Warbringer [Usable in any stance], Improved Intercept [-[5*Pts] sec Cd]</para>
        /// <para>Glyphs: Intercept [+1s Stun Dur]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Intercept(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            MinRange = 8f;
            MaxRange = 25f; // In Yards 
            CD = 30f - (DPSWarrChar.CombatFactors.FuryStance ? DPSWarrChar.Talents.Skirmisher * 5f : 0f) - (DPSWarrChar.StatS != null ? DPSWarrChar.StatS.BonusWarrior_PvP_4P_InterceptCDReduc : 0); // In Seconds
            RageCost = 10f;
            Targets = -1;
            Duration = 3f + (DPSWarrChar.Talents.GlyphOfIntercept ? 1f : 0f);
            StanceOkFury = true; StanceOkArms = StanceOkDef = (DPSWarrChar.Talents.Warbringer == 1);
            DamageBase = (DPSWarrChar.StatS != null ? DPSWarrChar.StatS.AttackPower * 0.12f : 0);
            //
            Initialize();
        }
    }
    public sealed class Intervene : Ability
    {
        public static new string SName { get { return "Intervene"; } }
        public static new string SDesc { get { return "Run at high speed towards a party member, intercepting the next melee or ranged attack made against them as well as reducing their total threat by 10%."; } }
        public static new string SIcon { get { return "ability_warrior_victoryrush"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 3411; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 30 sec Cd, 10 Rage, 0-25 yds, (Def)
        /// Run at high speed towards a party member, intercepting the next melee or ranged attack made against them as well as reducing their total threat by 10%.
        /// <para>DPSWarrChar.Talents: Warbringer [Usable in any stance], Safeguard [Target threat reduced by (15*Pts)% instead of 10%]</para>
        /// <para>Glyphs: Glyph of Intervene [Increases the number of attacks you intercept for your intervene target by 1.]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Intervene(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            MinRange = 0f;
            MaxRange = 25f; // In Yards 
            CD = 30f; // In Seconds
            Targets = -1;
            RageCost = 10f;
            StanceOkDef = true; StanceOkArms = StanceOkFury = (DPSWarrChar.Talents.Warbringer == 1);
            UseHitTable = false;
            //
            Initialize();
        }
    }
    public sealed class HeroicLeap : Ability
    {
        public static new string SName { get { return "Heroic Leap"; } }
        public static new string SDesc { get { return "Leap through the air towards a targeted location, slamming down with destructive force to deal (1+AP*0.5) damage to all enemies within 8 yards."; } }
        public static new string SIcon { get { return "ability_heroicleap"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 6544; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 60 sec Cd, 8-40 yds
        /// Leap through the air towards a targeted location, slamming down with destructive force to deal (1+AP*0.5) damage to all enemies within 8 yards.
        /// <para>DPSWarrChar.Talents: Skirmisher [-[10*Pts] sec CD, Max: 2]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public HeroicLeap(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            MinRange = 8f;
            MaxRange = 40f;
            CD = 60f - (DPSWarrChar.CombatFactors.FuryStance ? (10f * DPSWarrChar.Talents.Skirmisher) : 0f);
            RageCost = -1f;
            StanceOkArms = StanceOkDef = StanceOkFury = true;
            DamageBase = 1f + (DPSWarrChar.StatS != null ? DPSWarrChar.StatS.AttackPower * 0.50f : 0f);
            Targets = 10;
            //
            Initialize();
        }
    }
    #endregion
    #region Other Abilities
    public sealed class Retaliation : Ability
    {
        public static new string SName { get { return "Retaliation"; } }
        public static new string SDesc { get { return "Instantly counterattack any enemy that strikes you in melee for 12 sec. Melee attacks made from behind cannot be counterattacked. A maximum of 20 attacks will cause retaliation."; } }
        public static new string SIcon { get { return "ability_warrior_challange"; } }
        public override string Name { get { return SName; } }
        public override string Desc { get { return SDesc; } }
        public override string Icon { get { return SIcon; } }
        public static new int SSpellId { get { return 20230; } }
        public override int SpellId { get { return SSpellId; } }
        /// <summary>
        /// Instant, 5 Min Cd, No Rage, Melee Range, Melee Weapon, (Battle)
        /// Instantly counterattack any enemy that strikes you in melee for 12 sec. Melee attacks
        /// made from behind cannot be counterattacked. A maximum of 20 attacks will cause retaliation.
        /// <para>DPSWarrChar.Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Retaliation(DPSWarrCharacter dpswarrchar/*Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo*/)
        {
            DPSWarrChar = dpswarrchar; //Char = c; DPSWarrChar.StatS = s; CombatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            StanceOkArms = true;
            ReqMeleeRange = true;
            ReqMeleeWeap = true;
            CD = 5f * 60f;
            Duration = 12f;
            StackCap = 20f;
            UseHitTable = false;
            //
            Initialize();
        }
        public float StackCap;
    }
    #endregion
}
