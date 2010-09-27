/**********
 * Owner: Jothay
 **********/
using System;

namespace Rawr.DPSWarr.Skills
{
    public class DeepWounds : DoT
    {
        public static new string SName { get { return "Deep Wounds"; } }
        public static new string SDesc { get { return "Your critical strikes cause the opponent to bleed, dealing (16*Pts)% of your melee weapon's average damage over 6 sec."; } }
        public static new string SIcon { get { return "ability_backstab"; } }
        /// <summary>
        /// Your critical strikes cause the opponent to bleed, dealing (16*Pts)% of your 
        /// melee weapon's average damage over 6 sec.
        /// <para>Talents: Deep Wounds (Requires Talent) [(16*Pts)% damage dealt]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public DeepWounds(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            ReqTalent = true;
            Talent2ChksValue = Talents.DeepWounds;
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
                if (!Validated || (mhActivates + ohActivates) <= 0f) { return 0f; }

                // Doing it this way because Deep Wounds triggering off of a MH crit
                // and Deep Wounds triggering off of an OH crit do diff damage.
                // Damage stores the average damage of single deep wounds trigger
                float Damage = combatFactors.AvgMhWeaponDmgUnhasted * (0.16f * Talents.DeepWounds) * mhActivates / (mhActivates + ohActivates) +
                               combatFactors.AvgOhWeaponDmgUnhasted * (0.16f * Talents.DeepWounds) * ohActivates / (mhActivates + ohActivates);

                Damage *= (1f + StatS.BonusBleedDamageMultiplier);
                Damage *= combatFactors.DamageBonus;

                // Tick size
                Damage /= Duration * TimeBtwnTicks;

                // Because Deep Wounds is rolling, each tick is compounded by total number of times it's activated over its duration
                Damage *= (mhActivates + ohActivates) * Duration / FightDuration;

                // Ensure we're not doing negative damage
                return Damage;
            }
        }
        public override float GetDPS(float acts) { return TickSize; }
        public override float DPS { get { return TickSize; } }
    }
    #region BuffEffects
    public class BerserkerRage : BuffEffect
    {
        public static new string SName { get { return "Berserker Rage"; } }
        public static new string SDesc { get { return "The warrior enters a berserker rage, becoming immune to Fear, Sap and Incapacitate effects and generating extra tage when taking damage. Lasts 10 sec."; } }
        public static new string SIcon { get { return "spell_nature_ancestralguardian"; } }
#if RAWR4
        /// <summary>
        /// Instant, 30 sec Cd, Self, Any Stance,
        /// Enter a berserker rage, removing and granting immunity to Fear, Sap and Incapacitate effects
        /// and generating extra rage when taking damage. Lasts 10 sec.
        /// <para>Talents: Intensify Rage [-(10*Pts)% Cd]</para>
        /// <para>Glyphs: Glyph of Berserer Rage [+5 Rage Gen]</para>
        /// <para>Sets: none</para>
        /// </summary>
#else
        /// <summary>
        /// Instant, 30 sec Cd, Self, (Any)
        /// The warrior enters a berserker rage, becoming immune to Fear, Sap and Incapacitate effects
        /// and generating extra tage when taking damage. Lasts 10 sec.
        /// <para>Talents: Improved Berserker Rage [+(10*Pts) Rage Gen], Intensify Rage [-(10*Pts)% Cd]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
#endif
        public BerserkerRage(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
#if !RAWR4
            Cd = 30f * (1f - 1f / 9f * Talents.IntensifyRage); // In Seconds
            RageCost = 0f - (Talents.ImprovedBerserkerRage * 10f); // This is actually reversed in the rotation
#else
            Cd = 30f * (1f - 0.10f * Talents.IntensifyRage); // In Seconds
            RageCost = 0f - (Talents.GlyphOfBerserkerRage ? 5f : 0f); // This is actually reversed in the rotation
#endif
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BerserkerRage_;
            StanceOkArms = StanceOkDef = StanceOkFury = true;
            UseHitTable = false;
            UseReact = true;
            //
            Initialize();
        }
        public new float ActivatesOverride { get { return base.ActivatesOverride; } }
    }
    public class EnragedRegeneration : BuffEffect
    {
        public static new string SName { get { return "Enraged Regeneration"; } }
#if RAWR4
        public static new string SDesc { get { return "You regenerate 30% of your total health over 10 sec. Can only be used while Enraged."; } }
#else
        public static new string SDesc { get { return "You regenerate 30% of your total health over 10 sec. This ability requires an Enrage effect, consumes all Enrage effects and prevents any from affecting you for the full duration."; } }
#endif
        public static new string SIcon { get { return "ability_warrior_focusedrage"; } }
#if RAWR4
        /// <summary>
        /// Instant, 3 min Cd, 15 Rage, Self, Any Stance,
        /// You regenerate 30% of your total health over 10 sec. Can only be used while Enraged.
        /// <para>Talents: Field Dressing [+(10*Pts)% Healed]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
#else
        /// <summary>
        /// Instant, 3 min Cd, 15 Rage, Self, (Any)
        /// You regenerate 30% of your total health over 10 sec. This ability requires an Enrage effect,
        /// consumes all Enrage effects and prevents any from affecting you for the full duration.
        /// <para>Talents: none</para>
        /// <para>Glyphs: Glyph of Enraged Regeneration [+10% to effect]</para>
        /// <para>Sets: none</para>
        /// </summary>
#endif
        public EnragedRegeneration(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.EnragedRegeneration_;
            Cd = 3f * 60f; // In Seconds
            RageCost = 15f;
            StanceOkArms = StanceOkDef = StanceOkFury = true;
#if !RAWR4
            HealingBase = StatS.Health * (0.30f + (Talents.GlyphOfEnragedRegeneration ? 0.10f : 0f));
#else
            HealingBase = StatS.Health * 0.30f;
            HealingBonus = 1f + Talents.FieldDressing * 0.10f;
#endif
            UseHitTable = false;
            UseReact = true;
            //
            Initialize();
        }
    }
    public class LastStand : BuffEffect
    {
        public static new string SName { get { return "Last Stand"; } }
        public static new string SDesc { get { return "Temporarily grants you 30% of your maximum health for 20 sec. After the effect expires, the health is lost."; } }
        public static new string SIcon { get { return "spell_holy_ashestoashes"; } }
        /// <summary>
        /// Instant, 5 min Cd, Self, (Def)
        /// Temporarily grants you 30% of your maximum health for 20 sec.  After the effect expires, the health is lost.
        /// <para>Talents: Last Stand [Requires Talent]</para>
        /// <para>Glyphs: Glyph of Last Stand [-1 min Cd]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public LastStand() { }
    }
    public class Bloodrage : BuffEffect
    {
        public static new string SName { get { return "Bloodrage"; } }
        public static new string SDesc { get { return "Generates 10 rage at the cost of health and then generates an additional 10 rage over 10 sec."; } }
        public static new string SIcon { get { return "ability_racial_bloodrage"; } }
        /// <summary>
        /// Instant, 1 min cd, Self (Any)
        /// Generates 10 rage at the cost of health and then generates an additional 10 rage over 10 sec.
        /// <para>Talents: Improved Bloodrge [+(25*Pts)% Rage Generated], Intensify Rage [-(1/9*Pts]% Cooldown]</para>
        /// <para>Glyphs: Glyph of Bloodrage [-100% Health Cost]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Bloodrage(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Bloodrage_;
#if RAWR4
            Cd = 60f * (1f - 0.10f * Talents.IntensifyRage); // In Seconds
#else
            Cd = 60f * (1f - 1f/9f * Talents.IntensifyRage); // In Seconds
#endif
            Duration = 10f; // In Seconds
            // Rage is actually reversed in the rotation
            RageCost = -(20f // Base
                        + 10f) // Over Time
#if !RAWR4
                       * (1f + Talents.ImprovedBloodrage * 0.25f); // Talent Bonus
#else
                        ;
#endif
            StanceOkArms = StanceOkDef = StanceOkFury = true;
            Stats Base = BaseStats.GetBaseStats(Char.Level, CharacterClass.Warrior, Char.Race);
            float baseHealth = Base.Health + StatConversion.GetHealthFromStamina(Base.Stamina, CharacterClass.Warrior);
            HealingBase = -1f * (float)Math.Floor(baseHealth) * 0.16f;
#if !RAWR4
            HealingBonus = (Talents.GlyphOfBloodrage ? 0f : 1f);
#endif
            UseHitTable = false;
            UsesGCD = false;
            UseReact = true;
            //
            Initialize();
        }
    }
    public class BattleShout : BuffEffect
    {
        public static new string SName { get { return "Battle Shout"; } }
        public static new string SDesc { get { return "The warrior shouts, increasing attack power of all raid and party members within 20 yards by 548. Lasts 2 min."; } }
        public static new string SIcon { get { return "ability_warrior_battleshout"; } }
        /// <summary>
        /// The warrior shouts, increasing attack power of all raid and party members within 20 yards by 548. Lasts 2 min.
        /// <para>Talents: Booming Voice [+(25*Pts)% AoE and Duration], Commanding Presence [+(5*Pts)% to the AP Bonus]</para>
        /// <para>Glyphs: Glyph of Battle [+1 min duration]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public BattleShout(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BattleShout_;
#if RAWR4
            MaxRange = 30f; // In Yards 
            Duration = (2f + (Talents.GlyphOfBattle ? 2f : 0f)) * 60f/* * (1f + Talents.BoomingVoice * 0.25f)*/;
            RageCost = -1f * (20f + Talents.BoomingVoice * 5f);
#else
            MaxRange = 30f * (1f + Talents.BoomingVoice * 0.25f); // In Yards 
            Duration = (2f + (Talents.GlyphOfBattle ? 2f : 0f)) * 60f * (1f + Talents.BoomingVoice * 0.25f);
            RageCost = 10f;
#endif
            Cd = Duration;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            UseHitTable = false;
            //
            Initialize();
        }
        public override Stats AverageStats
        {
            get
            {
                if (!Validated) { return new Stats(); }
                Stats bonus = Effect.GetAverageStats(1f);
                return bonus;
            }
        }
    }
    public class CommandingShout : BuffEffect
    {
        public static new string SName { get { return "Commanding Shout"; } }
        public static new string SDesc { get { return "The warrior shouts, increasing the maximum health of all raid and party members within 20 yards by 2255. Lasts 2 min."; } }
        public static new string SIcon { get { return "ability_warrior_rallyingcry"; } }
        /// <summary>
        /// The warrior shouts, increasing the maximum health of all raid and party members within 20 yards by 2255. Lasts 2 min.
        /// <para>Talents: Booming Voice [+(25*Pts)% AoE and Duration], Commanding Presence [+(5*Pts)% to the Health Bonus]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public CommandingShout(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.CommandingShout_;
#if RAWR4
            MaxRange = 30f/* * (1f + Talents.BoomingVoice * 0.25f)*/; // In Yards 
            Duration = (2f + (Talents.GlyphOfCommand ? 2f : 0f)) * 60f/* * (1f + Talents.BoomingVoice * 0.25f)*/;
            RageCost = -1f * (20f + Talents.BoomingVoice * 5f);
#else
            MaxRange = 30f * (1f + Talents.BoomingVoice * 0.25f); // In Yards 
            Duration = (2f + (Talents.GlyphOfCommand ? 2f : 0f)) * 60f * (1f + Talents.BoomingVoice * 0.25f);
            RageCost = 10f;
#endif
            Cd = Duration;
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            UseHitTable = false;
            //
            Initialize();
        }
        public override Stats AverageStats
        {
            get
            {
                if (!Validated) { return new Stats(); }
                Stats bonus = Effect.GetAverageStats(1f);
                return bonus;
            }
        }
    }
    public class DeathWish : BuffEffect
    {
        public static new string SName { get { return "Death Wish"; } }
        public static new string SDesc { get { return "When activated you become enraged, increasing your physical damage by 20% but increasing all damage taken by 5%. Lasts 30 sec."; } }
        public static new string SIcon { get { return "spell_shadow_deathpact"; } }
        /// <summary>
        /// When activated you become enraged, increasing your physical damage by 20% but increasing
        /// all damage taken by 5%. Lasts 30 sec.
        /// <para>Talents: Death Wish [Requires Talent], Intensify Rage [-(1/9*Pts)% Cooldown]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public DeathWish(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DeathWish_;
            ReqTalent = true;
            Talent2ChksValue = Talents.DeathWish;
#if RAWR4
            Cd = 3f * 60f * (1f - 0.10f * Talents.IntensifyRage); // In Seconds
#else
            Cd = 3f * 60f * (1f - 1f/9f * Talents.IntensifyRage); // In Seconds
#endif
            Duration = 30f;
            RageCost = 10f;
            StanceOkArms = StanceOkFury = true;
            UseHitTable = false;
            //
            Initialize();
        }
    }
    public class Recklessness : BuffEffect
    {
        public static new string SName { get { return "Recklessness"; } }
        public static new string SDesc { get { return "Your next 3 special ability attacks have an additional 100% chance to critically hit but all damage taken is increased by 20%. Lasts 12 sec."; } }
        public static new string SIcon { get { return "ability_criticalstrike"; } }
        /// <summary>
        /// Your next 3 special ability attacks have an additional 100% chance to critically hit
        /// but all damage taken is increased by 20%. Lasts 12 sec.
        /// <para>Talents: Improved Disciplines [-(30*Pts) sec Cd]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Recklessness(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Recklessness_;
#if !RAWR4
            Cd = (5f * 60f - Talents.ImprovedDisciplines * 30f) * (1f - 1f / 9f * Talents.IntensifyRage); // In Seconds
#else
            Cd = (5f * 60f /*- Talents.ImprovedDisciplines * 30f*/) * (1f - 0.10f * Talents.IntensifyRage); // In Seconds
#endif
            Duration = 12f; // In Seconds
            StanceOkFury = true;
            //Effect = new SpecialEffect(Trigger.Use, new Stats { PhysicalCrit = 1f, DamageTakenMultiplier = 0.20f, }, Duration, Cd);
            UseHitTable = false;
            Initialize();
        }
    }
    public class SweepingStrikes : BuffEffect
    {
        public static new string SName { get { return "Sweeping Strikes"; } }
        public static new string SDesc { get { return "Your next 5 melee attacks strike an additional nearby opponent."; } }
        public static new string SIcon { get { return "ability_rogue_slicedice"; } }
        /// <summary>
        /// Your next 5 melee attacks strike an additional nearby opponent.
        /// <para>Talents: Sweeping Strikes [Requires Talent]</para>
        /// <para>Glyphs: Glyph of Sweeping Strikes [-100% Rage cost]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public SweepingStrikes(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SweepingStrikes_;
            ReqTalent = true;
            Talent2ChksValue = Talents.SweepingStrikes;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            ReqMultiTargs = true;
#if RAWR3 || RAWR4 || SILVERLIGHT
            Cd = FightDuration + (1.5f + CalcOpts.Latency + (UseReact ? CalcOpts.React / 1000f : CalcOpts.AllowedReact));
                //BossOpts.MultiTargsPerc != 0 ? 30f / ((float)BossOpts.MultiTargsPerc/* / 100f*/) : FightDuration + (1.5f + CalcOpts.Latency + (UseReact ? CalcOpts.React / 1000f : CalcOpts.AllowedReact)); // In Seconds
#else
            Cd = CalcOpts.MultipleTargetsPerc != 0 ? 30f / (CalcOpts.MultipleTargetsPerc / 100f) : FightDuration + (1.5f + CalcOpts.Latency + (UseReact ? CalcOpts.React / 1000f : CalcOpts.AllowedReact)); // In Seconds
#endif
#if !RAWR4
            RageCost = 30f - (Talents.FocusedRage * 1f);
            Duration = 30f;
#else
            RageCost = 30f;
            Duration = 10f;
            if (Cd < 60) Cd = 60;
#endif
            RageCost = (Talents.GlyphOfSweepingStrikes ? 0f : RageCost);
            StanceOkFury = StanceOkArms = true;
            UseHitTable = false;
            //
            Initialize();
        }
    }
    public class SecondWind : BuffEffect
    {
        public static new string SName { get { return "Second Wind"; } }
        public static new string SDesc { get { return "Whenever you are struck by a Stun of Immoblize effect you will generate 10*Pts Rage and (5*Pts)% of your total health over 10 sec."; } }
        public static new string SIcon { get { return "ability_hunter_harass"; } }
        /// <summary>
        /// Whenever you are struck by a Stun of Immoblize effect you will generate
        /// 10*Pts Rage and (5*Pts)% of your total health over 10 sec.
        /// <para>Talents: Sweeping Strikes [Requires Talent]</para>
        /// <para>Glyphs: Glyph of Sweeping Strikes [-100% Rage cost]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public SecondWind(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            //AbilIterater = -1f;
            ReqTalent = true;
            Talent2ChksValue = Talents.SecondWind;
            Cd = -1f;
            NumStunsOverDur = 0f;
            Duration = 10f; // Using 4 seconds to sim consume time
            RageCost = -10f * Talents.SecondWind;
            StanceOkDef = StanceOkFury = StanceOkArms = true;
            HealingBase = StatS.Health * 0.05f * Talents.SecondWind;
#if RAWR4
            HealingBonus = 1f + Talents.FieldDressing * 0.10f;
#endif
            UseHitTable = false;
            UsesGCD = false;
            //
            Initialize();
        }
        private float NUMSTUNSOVERDUR;
        public float NumStunsOverDur
        {
            get { return NUMSTUNSOVERDUR; }
            set
            {
                NUMSTUNSOVERDUR = value;
                Cd = (FightDuration / NUMSTUNSOVERDUR);
                if (Effect != null) { Effect.Cooldown = Cd; }
            }
        }
    }
    #endregion
    #region DeBuff Abilities
    public class ThunderClap : BuffEffect
    {
        public static new string SName { get { return "Thunder Clap"; } }
        public static new string SDesc { get { return "Blasts nearby enemies increasing the time between their attacks by 10% for 30 sec and doing [300+AP*0.12] damage to them. Damage increased by attack power. This ability causes additional threat."; } }
        public static new string SIcon { get { return "spell_nature_thunderclap"; } }
        /// <summary>
        /// Blasts nearby enemies increasing the time between their attacks by 10% for 30 sec
        /// and doing [300+AP*0.12] damage to them. Damage increased by attack power.
        /// This ability causes additional threat.
        /// <para>TODO: BonusCritDamage to 2x instead of 1.5x as it's now considered a ranged attack (3.3.3 notes) other parts implemented already</para>
        /// <para>Talents: Improved Thunder Clap [-(1/2/4) rage cost, +(10*Pts)% Damage, +(ROUNDUP(10-10/3*Pts))% Slowing Effect], Incite [+(5*Pts)% Critical Strike chance]</para>
        /// <para>Glyphs: Glyph of Thunder Clap [+2 yds MaxRange], Glyph of Resonating Power [-5 RageCost]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public ThunderClap(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ThunderClap_;
            ReqMeleeRange = true;
#if RAWR4
            isMaint = false;
#endif
#if RAWR3 || RAWR4 || SILVERLIGHT
            {
                float value = 0;
                foreach (TargetGroup tg in BossOpts.Targets) {
                    if (tg.Frequency <= 0 || tg.Chance <= 0) continue; // bad one, skip it
                    float upTime = tg.Frequency / BossOpts.BerserkTimer * tg.Duration * tg.Chance;
                    value += (Math.Max(10, tg.NumTargs - (tg.NearBoss ? 0 : 1))) * upTime;
                }
                Targets += value;
            }
#else
            Targets += (CalcOpts.MultipleTargets ? (CalcOpts.MultipleTargetsMax - 1f) : 0f);
#endif
            MaxRange = 5f + (Talents.GlyphOfThunderClap ? 2f : 0f); // In Yards 
            Cd = 6f; // In Seconds
            Duration = 30f; // In Seconds
            RageCost = 20f - (Talents.GlyphOfResonatingPower ? 5f : 0f);
#if !RAWR4
            float cost = 0f;
            switch (Talents.ImprovedThunderClap) {
                case 1: { cost = 1f; break; }
                case 2: { cost = 2f; break; }
                case 3: { cost = 4f; break; }
                default: { cost = 0f; break; }
            }
            RageCost -= cost;
            RageCost -= (Talents.FocusedRage * 1f);
            DamageBonus = 1f + Talents.ImprovedThunderClap * 0.10f;
            BonusCritChance = Talents.Incite * 0.05f;
#endif
            StanceOkArms = StanceOkDef = true;
            DamageBase = 300f + StatS.AttackPower * 0.12f;
            UseSpellHit = true;
            CanBeDodged = CanBeParried = false;
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
                //addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                //addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                if (CalcOpts.AllowFlooring)
                {
                    addMisses = (float)Math.Ceiling(addMisses);
                    //addDodges = (float)Math.Ceiling(addDodges);
                    //addParrys = (float)Math.Ceiling(addParrys);
                }

                result = Base + addMisses + addDodges + addParrys;

                return result;
            }
        }
    }
    public class SunderArmor : BuffEffect
    {
        public static new string SName { get { return "Sunder Armor"; } }
        public static new string SDesc { get { return "Sunders the target's armor, reducing it by 4% per Sunder Armor and causes a high amount of threat. Threat increased by attack power. Can be applied up to 5 times. Lasts 30 sec."; } }
        public static new string SIcon { get { return "ability_warrior_sunder"; } }
        /// <summary>
        /// Sunders the target's armor, reducing it by 4% per Sunder Armor and causes a high amount of threat.
        /// Threat increased by attack power. Can be applied up to 5 times. Lasts 30 sec.
        /// <para>Talents: Focused Rage [-(Pts) Rage Cost], Puncture [-(Pts) Rage Cost], </para>
        /// <para>Glyphs: Glyph of Sunder Armor [+1 Targets]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public SunderArmor(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SunderArmor_;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            Duration = 30f; // In Seconds
            Cd = 1.5f;
            CanCrit = false;
            RageCost = 15f
#if !RAWR4
                - (Talents.FocusedRage * 1f) - (Talents.Puncture * 1f);
#else
                * (1f - (Talents.GlyphOfFuriousSundering ? 0.50f : 0f));
#endif
            Targets = 1f + (Talents.GlyphOfSunderArmor ? 1f : 0f);
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
                addMisses = (MHAtkTable.Miss > 0) ? Base * MHAtkTable.Miss : 0f;
                addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                if (CalcOpts.AllowFlooring)
                {
                    addMisses = (float)Math.Ceiling(addMisses);
                    addDodges = (float)Math.Ceiling(addDodges);
                    addParrys = (float)Math.Ceiling(addParrys);
                }

                result = Base + addMisses + addDodges + addParrys;

                return result;
            }
        }
    }
    public class ShatteringThrow : BuffEffect
    {
        public static new string SName { get { return "Shattering Throw"; } }
        public static new string SDesc { get { return "Throws your weapon at the enemy causing (12+AP*0.50) damage (based on attack power), reducing the armor on the target by 20% for 10 sec or removing any invulnerabilities."; } }
        public static new string SIcon { get { return "ability_warrior_shatteringthrow"; } }
        /// <summary>
        /// Throws your weapon at the enemy causing (12+AP*0.50) damage (based on attack power),
        /// reducing the armor on the target by 20% for 10 sec or removing any invulnerabilities.
        /// </summary>
        public ShatteringThrow(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ShatteringThrow_;
            ReqMeleeWeap = true;
            ReqMeleeRange = false;
            MaxRange = 30f; // In Yards
            //Targets += StatS.BonusTargets;
            Cd = 5f * 60f; // In Seconds
            Duration = 10f;
            CastTime = 1.5f; // In Seconds
#if !RAWR4
            RageCost = 25f - (Talents.FocusedRage * 1f);
#else
            RageCost = 25f;// -(Talents.FocusedRage * 1f);
#endif
            StanceOkArms = true;
            DamageBase = 12f + StatS.AttackPower * 0.50f;
            //
            Initialize();
        }
    }
    public class DemoralizingShout : BuffEffect
    {
        public static new string SName { get { return "Demoralizing Shout"; } }
        public static new string SDesc { get { return "Reduces the melee attack power of all enemies within 10 yards by 411 for 30 sec."; } }
        public static new string SIcon { get { return "ability_warrior_warcry"; } }
        /// <summary>
        /// Reduces the melee attack power of all enemies within 10 yards by 411 for 30 sec.
        /// <para>Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public DemoralizingShout(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DemoralizingShout_;
            ReqMeleeWeap = false;
            ReqMeleeRange = false;
            MaxRange = 10f; // In Yards 
            Duration = 30f * (1f + 0.05f * Talents.BoomingVoice);
#if !RAWR4
            RageCost = 10f - (Talents.FocusedRage * 1f);
#else
            RageCost = 10f;
            RageCost = RageCost * (1f - Talents.DrumsOfWar * 0.50f); // Drums of War negates rage cost
#endif
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
                // activates and forcing the char to use Rend again to ensure it's up
                // in the event that the attemtped activate didn't land (it Missed, was Dodged or Parried)
                // We're only doing the additional check once so it will at most do this
                // twice in a row, may consider doing a settler
                float result = 0f;
                float Base = base.ActivatesOverride;
                addMisses = (MHAtkTable.Miss > 0) ? Base * MHAtkTable.Miss : 0f;
                //addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                //addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                if (CalcOpts.AllowFlooring)
                {
                    addMisses = (float)Math.Ceiling(addMisses);
                    //addDodges = (float)Math.Ceiling(addDodges);
                    //addParrys = (float)Math.Ceiling(addParrys);
                }

                result = Base + addMisses + addDodges + addParrys;

                return result;
            }
        }
    }
    public class Hamstring : BuffEffect
    {
        public static new string SName { get { return "Hamstring"; } }
        public static new string SDesc { get { return "Instant, No cd, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker) Maims the enemy, reducing movement speed by 50% for 15 sec."; } }
        public static new string SIcon { get { return "ability_shockwave"; } }
        /// <summary>
        /// Instant, No cd, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker)
        /// Maims the enemy, reducing movement speed by 50% for 15 sec.
        /// <para>Talents: Improved Hamstring [Gives a [5*Pts]% chance to immobilize the target for 5 sec.]</para>
        /// <para>Glyphs: Glyph of Hamstring [Gives a 10% chance to immobilize the target for 5 sec.]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Hamstring(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Hamstring_;
            ReqMeleeWeap = true;
            ReqMeleeRange = true;
            Duration = 15f; // In Seconds
#if !RAWR4
            RageCost = 10f - (Talents.FocusedRage * 1f);
#else
            RageCost = 10f;// -(Talents.FocusedRage * 1f);
#endif
            //Targets += StatS.BonusTargets;
            StanceOkFury = StanceOkArms = true;
            //Effect = new SpecialEffect(Trigger.Use, new Stats() { AttackPower = 0f, /*TargetMoveSpeedReducPerc = 0.50f,*/ }, Duration, Duration);
            //float Chance = Talents.ImprovedHamstring * 0.05f + (Talents.GlyphOfHamstring ? 0.10f : 0.00f);
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

                if (CalcOpts.AllowFlooring)
                {
                    addMisses = (float)Math.Ceiling(addMisses);
                    addDodges = (float)Math.Ceiling(addDodges);
                    addParrys = (float)Math.Ceiling(addParrys);
                }

                result = Base + addMisses + addDodges + addParrys;

                return result;
            }
        }
    }
    #endregion
    #region Anti-Debuff Abilities
    public class EveryManForHimself : Ability
    {
        public static new string SName { get { return "Every Man for Himself"; } }
        public static new string SDesc { get { return "Removes all movement impairing effects and all effects which cause loss of control of your character. This effect shares a cooldown with other similar effects."; } }
        public static new string Icon { get { return "spell_shadow_charm"; } }
        /// <summary>
        /// Instant, 2 Min Cooldown, 0 Rage, Self (Any)
        /// Removes all movement impairing effects and all effects which cause loss of control of
        /// your character. This effect shares a cooldown with other similar effects.
        /// <para>Talents: none</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public EveryManForHimself(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            Cd = 2f * 60f;
            StanceOkArms = StanceOkFury = StanceOkDef = true;
            UseHitTable = false;
            UseReact = true;
            //
            Initialize();
        }
    }
    public class HeroicFury : Ability
    {
        public static new string SName { get { return "Heroic Fury"; } }
        public static new string SDesc { get { return "Removes any Immobilization effects and refreshes the cooldown of your Intercept ability."; } }
        public static new string SIcon { get { return "warrior_talent_icon_deadlycalm"; } }
        /// <summary>
        /// Instant, 45 sec Cooldown, 0 Rage, Self (Any)
        /// Removes any Immobilization effects and refreshes the cooldown of your Intercept ability.
        /// </summary>
        /// <para>Talents: </para>
        /// <para>Glyphs: </para>
        public HeroicFury(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            ReqTalent = true;
            Talent2ChksValue = Talents.HeroicFury;
            Cd = 45f;
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
    public class Charge : Ability
    {
        public static new string SName { get { return "Charge"; } }
        public static new string SDesc { get { return "Charge an enemy, generate 15 rage, and stun it for 1.50 sec. Cannot be used in combat."; } }
        public static new string SIcon { get { return "ability_warrior_charge"; } }
        /// <summary>
        /// Instant, 15 sec cd, 0 Rage, 8-25 yds, (Battle)
        /// Charge an enemy, generate 15 rage, and stun it for 1.50 sec. Cannot be used in combat.
        /// <para>Talents: Warbringer [Usable in combat and any stance], Juggernaut [Usable in combat], Improved Charge [+(5*Pts) RageGen]</para>
        /// <para>Glyphs: Glyph of Rapid Charge [-7% Cd], Glyph of Charge [+5 yds MaxRange]</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Charge(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            MinRange = 8f;
#if !RAWR4
            MaxRange = 25f + (Talents.GlyphOfCharge ? 5f : 0f); // In Yards 
            RageCost = -(15f + (Talents.ImprovedCharge * 5f));
#else
            MaxRange = 25f + (Talents.GlyphOfLongCharge ? 5f : 0f); // In Yards 
            RageCost = -(15f + (Talents.Blitz * 5f));
#endif
            Cd = (15f + Talents.Juggernaut * 5f) * (1f - (Talents.GlyphOfRapidCharge ? 0.07f : 0f)); // In Seconds
            Duration = 1.5f;
            if (Talents.Warbringer == 1)
            {
                StanceOkArms = StanceOkFury = StanceOkDef = true;
            } else if (Talents.Juggernaut == 1) {
                StanceOkArms = true;
            }
            //
            Initialize();
        }
    }
    public class Intercept : Ability
    {
        public static new string SName { get { return "Intercept"; } }
        public static new string SDesc { get { return "Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec."; } }
        public static new string SIcon { get { return "ability_rogue_sprint"; } }
        /// <summary>
        /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Zerker)
        /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
        /// <para>Talents: Warbringer [Usable in any stance], Improved Intercept [-[5*Pts] sec Cd]</para>
        /// <para>Glyphs: none</para>
        /// <para>Sets: none</para>
        /// </summary>
        public Intercept(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            MinRange = 8f;
            MaxRange = 25f; // In Yards 
#if !RAWR4
            Cd = 30f - (Talents.ImprovedIntercept * 5f) - StatS.BonusWarrior_PvP_4P_InterceptCDReduc; // In Seconds
            RageCost = 10f - Talents.Precision * 1f;
#else
            Cd = 30f /*- (Talents.ImprovedIntercept * 5f)*/ - StatS.BonusWarrior_PvP_4P_InterceptCDReduc; // In Seconds
            RageCost = 10f;// -Talents.Precision * 1f;
#endif
            Duration = 3f;
            StanceOkFury = true; StanceOkArms = StanceOkDef = (Talents.Warbringer == 1);
            DamageBase = 380f;
            //
            Initialize();
        }
    }
    public class Intervene : Ability
    {
        public static new string SName { get { return "Intervene"; } }
        public static new string SDesc { get { return "Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec."; } }
        public static new string SIcon { get { return "ability_warrior_victoryrush"; } }
        /// <summary>
        /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Def)
        /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
        /// <para>Talents: Warbringer [Usable in any stance]</para>
        /// <para>Glyphs: Glyph of Intervene [Increases the number of attacks you intercept for your intervene target by 1.]</para>
        /// </summary>
        public Intervene(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            MinRange = 8f;
            MaxRange = 25f; // In Yards 
#if !RAWR4
            Cd = 30f * (1f - (Talents.ImprovedIntercept * 5f)); // In Seconds
#else
            Cd = 30f;// * (1f - (Talents.ImprovedIntercept * 5f)); // In Seconds
#endif
            RageCost = 10f;
            StanceOkDef = true; StanceOkArms = StanceOkFury = (Talents.Warbringer == 1);
            UseHitTable = false;
            //
            Initialize();
        }
    }
    #endregion
    #region Other Abilities
    public class Retaliation : Ability
    {
        public static new string SName { get { return "Retaliation"; } }
        public static new string SDesc { get { return "Instantly counterattack any enemy that strikes you in melee for 12 sec. Melee attacks made from behind cannot be counterattacked. A maximum of 20 attacks will cause retaliation."; } }
        public static new string SIcon { get { return "ability_warrior_challange"; } }
        /// <summary>
        /// Instant, 5 Min Cd, No Rage, Melee Range, Melee Weapon, (Battle)
        /// Instantly counterattack any enemy that strikes you in melee for 12 sec. Melee attacks
        /// made from behind cannot be counterattacked. A maximum of 20 attacks will cause retaliation.
        /// </summary>
        /// <para>Talents: Improved Disciplines [-(30*Pts) sec Cd]</para>
        /// <para>Glyphs: </para>
        public Retaliation(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsDPSWarr co, BossOptions bo)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co; BossOpts = bo;
            //
            StanceOkArms = true;
            ReqMeleeRange = true;
            ReqMeleeWeap = true;
            //Targets += StatS.BonusTargets;
#if !RAWR4
            Cd = 5f * 60f - Talents.ImprovedDisciplines * 30f;
#else
            Cd = 5f * 60f;// -Talents.ImprovedDisciplines * 30f;
#endif
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