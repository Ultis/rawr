/**********
 * Owner: Jothay
 **********/
using System;

namespace Rawr.DPSWarr {
    public partial class Skills {
        public class DeepWounds : DoT
        {
            // Constructors
            /// <summary>
            /// Your critical strikes cause the opponent to bleed, dealing (16*Pts)% of your melee weapon's
            /// average damage over 6 sec.
            /// </summary>
            /// <TalentsAffecting>Deep Wounds (Requires Talent) [(16*Pts)% damage dealt]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public DeepWounds(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Deep Wounds";
                ReqTalent = true;
                Talent2ChksValue = Talents.DeepWounds;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                CanCrit = false;
                Duration = 6f; // In Seconds
                TimeBtwnTicks = 1f; // In Seconds
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                mhActivates = ohActivates = 0f;
                //
                InitializeB();
            }
            private float mhActivates, ohActivates;
            public void SetAllAbilityActivates(float mh, float oh) { mhActivates = mh; ohActivates = oh; }
            protected override float ActivatesOverride { get { return mhActivates + ohActivates; } }
            public override float TickSize {
                get {
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
                    return Math.Max(0f, Damage);
                }
            }
            public override float GetDPS(float acts) { return TickSize; }
            public override float DPS { get { return TickSize; } }
        }
        #region BuffEffects
        public class BerserkerRage : BuffEffect
        {
            /// <summary>
            /// Instant, 30 sec Cd, Self, (Any)
            /// The warrior enters a berserker rage, becoming immune to Fear, Sap and Incapacitate effects
            /// and generating extra tage when taking damage. Lasts 10 sec.
            /// </summary>
            /// <TalentsAffecting>Improved Berserker Rage [+(10*Pts) Rage Generated]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public BerserkerRage(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Berserker Rage";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BerserkerRage_;
                Cd = 30f * (1f - 1f / 9f * Talents.IntensifyRage); // In Seconds
                RageCost = 0f + (Talents.ImprovedBerserkerRage * 10f); // This is actually reversed in the rotation
                StanceOkArms = StanceOkDef = StanceOkFury = true;
                //
                InitializeB();
            }
        }
        public class EnragedRegeneration : BuffEffect
        {
            /// <summary>
            /// Instant, 3 min Cd, 15 Rage, Self, (Any)
            /// You regenerate 30% of your total health over 10 sec. This ability requires an Enrage effect,
            /// consumes all Enrage effects and prevents any from affecting you for the full duration.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Enraged Regeneration [+10% to effect]</GlyphsAffecting>
            public EnragedRegeneration(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Enraged Regeneration";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.EnragedRegeneration_;
                Cd = 3f * 60f; // In Seconds
                RageCost = 15f;
                StanceOkArms = StanceOkDef = StanceOkFury = true;
                HealingBase = StatS.Health * (0.30f + (Talents.GlyphOfEnragedRegeneration ? 0.10f : 0f));
                //
                InitializeB();
            }
        }
        public class LastStand : BuffEffect
        {
            /// <summary>
            /// Instant, 5 min Cd, Self, (Def)
            /// 
            /// </summary>
            /// <TalentsAffecting>Last Stand [Requires Talent]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Last Stand [-1 min Cd]</GlyphsAffecting>
        }
        public class Bloodrage : BuffEffect
        {
            /// <summary>
            /// Instant, 1 min cd, Self (Any)
            /// Generates 10 rage at the cost of health and then generates an additional 10 rage over 10 sec.
            /// </summary>
            /// <TalentsAffecting>Improved Bloodrge [+(25*Pts)% Rage Generated], Intensify Rage [-(1/9*Pts]% Cooldown]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Bloodrage [-100% Health Cost]</GlyphsAffecting>
            public Bloodrage(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Bloodrage";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Bloodrage_;
                Cd = 60f * (1f - 1f / 9f * Talents.IntensifyRage); // In Seconds
                Duration = 10f; // In Seconds
                RageCost = 20f * (1f + Talents.ImprovedBloodrage * 0.25f); // This is actually reversed in the rotation
                StanceOkArms = StanceOkDef = StanceOkFury = true;
                Stats Base = BaseStats.GetBaseStats(Char.Level, CharacterClass.Warrior, Char.Race);
                float baseHealth = Base.Health + StatConversion.GetHealthFromStamina(Base.Stamina, CharacterClass.Warrior);
                HealingBase = -1f * (float)Math.Floor(baseHealth) * 0.16f;
                HealingBonus = (Talents.GlyphOfBloodrage ? 0f : 1f);
                /*Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { BonusRageGen = 1f * (1f + Talents.ImprovedBloodrage * 0.25f), },
                    Duration, Cd);*/
                //
                InitializeB();
            }
        }
        public class BattleShout : BuffEffect
        {
            // Constructors
            /// <summary>
            /// The warrior shouts, increasing attack power of all raid and party members within 20 yards by 548. Lasts 2 min.
            /// </summary>
            /// <TalentsAffecting>
            /// Booming Voice [+(25*Pts)% AoE and Duration],
            /// Commanding Presence [+(5*Pts)% to the AP Bonus]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Battle [+1 min duration]</GlyphsAffecting>
            public BattleShout(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Battle Shout";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.BattleShout_;
                MaxRange = 30f * (1f + Talents.BoomingVoice * 0.25f); // In Yards 
                Duration = (2f + (Talents.GlyphOfBattle ? 2f : 0f)) * 60f * (1f + Talents.BoomingVoice * 0.25f);
                Cd = Duration;
                RageCost = 10f;
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                /*Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = (548f*(1f+Talents.CommandingPresence*0.05f)), },
                    Duration, Cd);*/
                //
                InitializeB();
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
            // Constructors
            /// <summary>
            /// The warrior shouts, increasing the maximum health of all raid and party members within 20 yards by 2255. Lasts 2 min.
            /// </summary>
            /// <TalentsAffecting>
            /// Booming Voice [+(25*Pts)% AoE and Duration],
            /// Commanding Presence [+(5*Pts)% to the Health Bonus]
            /// </TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public CommandingShout(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Commanding Shout";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.CommandingShout_;
                MaxRange = 30f * (1f + Talents.BoomingVoice * 0.25f); // In Yards 
                Duration = (2f + (Talents.GlyphOfCommand ? 2f : 0f)) * 60f * (1f + Talents.BoomingVoice * 0.25f);
                Cd = Duration;
                RageCost = 10f;
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                /*Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { Health = (2255f*(1f+Talents.CommandingPresence*0.05f)), },
                    Duration, Cd);*/
                //
                InitializeB();
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
            // Constructors
            /// <summary>
            /// When activated you become enraged, increasing your physical damage by 20% but increasing
            /// all damage taken by 5%. Lasts 30 sec.
            /// </summary>
            /// <TalentsAffecting>Death Wish [Requires Talent], Intensify Rage [-(1/9*Pts)% Cooldown]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public DeathWish(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Death Wish";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DeathWish_;
                ReqTalent = true;
                Talent2ChksValue = Talents.DeathWish;
                Cd = 3f * 60f * (1f - 1f / 9f * Talents.IntensifyRage); // In Seconds
                Duration = 30f;
                RageCost = 10f;
                StanceOkArms = StanceOkFury = true;
                /*Effect = new SpecialEffect(Trigger.Use,
                        new Stats() { BonusDamageMultiplier = 0.20f, DamageTakenMultiplier = 0.05f, },
                        Duration, Cd);*/
                //
                InitializeB();
            }
        }
        public class Recklessness : BuffEffect {
            // Constructors
            /// <summary>
            /// Your next 3 special ability attacks have an additional 100% to critically hit
            /// but all damage taken is increased by 20%. Lasts 12 sec.
            /// </summary>
            /// <TalentsAffecting>Improved Disciplines [-(30*Pts) sec Cd]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Recklessness(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Recklessness";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Recklessness_;
                Cd = (5f * 60f - Talents.ImprovedDisciplines * 30f) * (1f - 1f / 9f * Talents.IntensifyRage); // In Seconds
                Duration = 12f; // In Seconds
                StanceOkFury = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats { PhysicalCrit = 1f, DamageTakenMultiplier = 0.20f, },
                    Duration, Cd);
                InitializeB();
            }
        }
        public class SweepingStrikes : BuffEffect
        {
            // Constructors
            /// <summary>
            /// Your next 5 melee attacks strike an additional nearby opponent.
            /// </summary>
            /// <TalentsAffecting>Sweeping Strikes [Requires Talent]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Sweeping Strikes [-100% Rage cost]</GlyphsAffecting>
            public SweepingStrikes(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Sweeping Strikes";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SweepingStrikes_;
                ReqTalent = true;
                Talent2ChksValue = Talents.SweepingStrikes;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                ReqMultiTargs = true;
                Cd = CalcOpts.MultipleTargetsPerc != 0 ? 30f / (CalcOpts.MultipleTargetsPerc / 100f) : FightDuration+(1.5f+CalcOpts.GetLatency()); // In Seconds
                Duration = 30f;
                RageCost = 30f - (Talents.FocusedRage * 1f);
                RageCost = (Talents.GlyphOfSweepingStrikes ? 0f : RageCost);
                StanceOkFury = StanceOkArms = true;
                //
                InitializeB();
            }
        }
        public class SecondWind : BuffEffect
        {
            // Constructors
            /// <summary>
            /// Whenever you are struck by a Stun of Immoblize effect you will generate
            /// 10*Pts Rage and (5*Pts)% of your total health over 10 sec.
            /// </summary>
            /// <TalentsAffecting>Sweeping Strikes [Requires Talent]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Sweeping Strikes [-100% Rage cost]</GlyphsAffecting>
            public SecondWind(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Second Wind";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SweepingStrikes_;
                ReqTalent = true;
                Talent2ChksValue = Talents.SecondWind;
                NumStunsOverDur = 1f;
                Duration = 10f; // Using 4 seconds to sim consume time
                Cd = 1.5f + CalcOpts.GetLatency();
                RageCost = 10f * Talents.SecondWind;
                StanceOkDef = StanceOkFury = StanceOkArms = true;
                //Effect = new SpecialEffect(Trigger.Use, new Stats() { BonusRageGen = 10f * Talents.SecondWind, }, Duration, Cd);
                HealingBase = StatS.Health * 0.05f * Talents.SecondWind;
                //
                InitializeB();
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
        public class ThunderClap : BuffEffect {
            /// <summary>
            /// Blasts nearby enemies increasing the time between their attacks by 10% for 30 sec
            /// and doing [300+AP*0.12] damage to them. Damage increased by attack power.
            /// This ability causes additional threat.
            /// </summary>
            /// <TalentsAffecting>
            /// Improved Thunder Clap [-(1/2/4) rage cost, +(10*Pts)% Damage, +(ROUNDUP(10-10/3*Pts))% Slowing Effect]
            /// Incite [+(5*Pts)% Critical Strike chance]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>
            /// Glyph of Thunder Clap [+2 yds MaxRange]
            /// Glyph of Resonating Power [-5 RageCost]
            /// </GlyphsAffecting>
            public ThunderClap(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Thunder Clap";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ThunderClap_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Targets += (CalcOpts.MultipleTargets ? (CalcOpts.MultipleTargetsMax - 1f) : 0f);
                MaxRange = 5f + (Talents.GlyphOfThunderClap ? 2f : 0f); // In Yards 
                Cd = 6f; // In Seconds
                Duration = 30f; // In Seconds
                float cost = 0f;
                switch (Talents.ImprovedThunderClap)
                {
                    case 1: { cost = 1f; break; }
                    case 2: { cost = 2f; break; }
                    case 3: { cost = 4f; break; }
                    default: { cost = 0f; break; }
                }
                RageCost = 20f - cost - (Talents.GlyphOfResonatingPower ? 5f : 0f) - (Talents.FocusedRage * 1f);
                StanceOkArms = StanceOkDef = true;
                DamageBase = 300f + StatS.AttackPower * 0.12f;
                DamageBonus = 1f + Talents.ImprovedThunderClap * 0.10f;
                BonusCritChance = Talents.Incite * 0.05f;
                //
                InitializeB();
            }
            protected override float ActivatesOverride {
                get {
                    // Since this has to be up according to Debuff Maintenance Rules
                    // this override has the intention of taking the baseline
                    // activates and forcing the char to use Rend again to ensure it's up
                    // in the event that the attemtped activate didn't land (it Missed, was Dodged or Parried)
                    // We're only doing the additional check once so it will at most do this
                    // twice in a row, may consider doing a settler
                    float result = 0f;
                    float Base = base.ActivatesOverride;
                    addMisses  = (MHAtkTable.Miss  > 0) ? Base * MHAtkTable.Miss  : 0f;
                    //addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                    //addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                    if (CalcOpts.AllowFlooring) {
                        addMisses = (float)Math.Ceiling(addMisses);
                        //addDodges = (float)Math.Ceiling(addDodges);
                        //addParrys = (float)Math.Ceiling(addParrys);
                    }

                    result = Base + addMisses + addDodges + addParrys;

                    return result;
                }
            }
        }
        public class SunderArmor : BuffEffect {
            // Constructors
            /// <summary></summary>
            /// <TalentsAffecting>Focused Rage [-(Pts) Rage Cost], Puncture [-(Pts) Rage Cost], </TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Sunder Armor [+1 Targets]</GlyphsAffecting>
            public SunderArmor(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Sunder Armor";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.SunderArmor_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 30f; // In Seconds
                Cd = 1.5f;
                CanCrit = false;
                RageCost = 15f - (Talents.FocusedRage * 1f) - (Talents.Puncture * 1f);
                Targets = 1f + (Talents.GlyphOfSunderArmor ? 1f : 0f);
                StanceOkFury = StanceOkArms = StanceOkDef = true;
                //
                InitializeB();
                //
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.04f, },
                    Duration, Cd, MHAtkTable.Hit, 5);
            }
            protected override float ActivatesOverride {
                get {
                    // Since this has to be up according to Debuff Maintenance Rules
                    // this override has the intention of taking the baseline
                    // activates and forcing the char to use Rend again to ensure it's up
                    // in the event that the attemtped activate didn't land (it Missed, was Dodged or Parried)
                    // We're only doing the additional check once so it will at most do this
                    // twice in a row, may consider doing a settler
                    float result = 0f;
                    float Base = base.ActivatesOverride;
                    addMisses = (MHAtkTable.Miss  > 0) ? Base * MHAtkTable.Miss  : 0f;
                    addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                    addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                    if (CalcOpts.AllowFlooring) {
                        addMisses = (float)Math.Ceiling(addMisses);
                        addDodges = (float)Math.Ceiling(addDodges);
                        addParrys = (float)Math.Ceiling(addParrys);
                    }

                    result = Base + addMisses + addDodges + addParrys;

                    return result;
                }
            }
        }
        public class ShatteringThrow : BuffEffect {
            /// <summary>
            /// Throws your weapon at the enemy causing (12+AP*0.50) damage (based on attack power),
            /// reducing the armor on the target by 20% for 10 sec or removing any invulnerabilities.
            /// </summary>
            public ShatteringThrow(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Shattering Throw";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.ShatteringThrow_;
                ReqMeleeWeap = true;
                ReqMeleeRange = false;
                MaxRange = 30f; // In Yards
                //Targets += StatS.BonusTargets;
                Cd = 5f * 60f; // In Seconds
                Duration = 10f;
                CastTime = 1.5f; // In Seconds
                RageCost = 25f - (Talents.FocusedRage * 1f);
                StanceOkArms = true;
                DamageBase = 12f + StatS.AttackPower * 0.50f;
                //
                InitializeB();
                //
                /*Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { ArmorPenetration = 0.20f, },
                    Duration, Cd,
                    MHAtkTable.Hit + MHAtkTable.Crit);*/
            }
        }
        public class DemoralizingShout : BuffEffect {
            // Constructors
            /// <summary>
            /// Reduces the melee attack power of all enemies within 10 yards by 411 for 30 sec.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public DemoralizingShout(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Demoralizing Shout";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DemoralizingShout_;
                ReqMeleeWeap = false;
                ReqMeleeRange = false;
                MaxRange = 10f; // In Yards 
                Duration = 30f;
                RageCost = 10f - (Talents.FocusedRage * 1f);
                StanceOkArms = StanceOkFury = true;
                //
                InitializeB();
                //
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { /*AttackPower = 411f,*/ }, // needs to be boss debuff
                    Duration, Duration,
                    MHAtkTable.Hit + MHAtkTable.Crit);
            }
            protected override float ActivatesOverride {
                get {
                    // Since this has to be up according to Debuff Maintenance Rules
                    // this override has the intention of taking the baseline
                    // activates and forcing the char to use Rend again to ensure it's up
                    // in the event that the attemtped activate didn't land (it Missed, was Dodged or Parried)
                    // We're only doing the additional check once so it will at most do this
                    // twice in a row, may consider doing a settler
                    float result = 0f;
                    float Base = base.ActivatesOverride;
                    addMisses = (MHAtkTable.Miss  > 0) ? Base * MHAtkTable.Miss  : 0f;
                    //addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                    //addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                    if (CalcOpts.AllowFlooring) {
                        addMisses = (float)Math.Ceiling(addMisses);
                        //addDodges = (float)Math.Ceiling(addDodges);
                        //addParrys = (float)Math.Ceiling(addParrys);
                    }

                    result = Base + addMisses + addDodges + addParrys;

                    return result;
                }
            }
        }
        public class Hamstring : BuffEffect {
            /// <summary>
            /// Instant, No cd, 10 Rage, Melee Range, Melee Weapon, (Battle/Zerker)
            /// Maims the enemy, reducing movement speed by 50% for 15 sec.
            /// </summary>
            /// <TalentsAffecting>Improved Hamstring [Gives a [5*Pts]% chance to immobilize the target for 5 sec.]</TalentsAffecting>
            /// <GlyphsAffecting>Glyph of Hamstring [Gives a 10% chance to immobilize the target for 5 sec.]</GlyphsAffecting>
            public Hamstring(Character c, Stats s, CombatFactors cf, WhiteAttacks wa) {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Hamstring";
                AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.Hamstring_;
                ReqMeleeWeap = true;
                ReqMeleeRange = true;
                Duration = 15f; // In Seconds
                RageCost = 10f - (Talents.FocusedRage * 1f);
                //Targets += StatS.BonusTargets;
                StanceOkFury = StanceOkArms = true;
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = 0f, /*TargetMoveSpeedReducPerc = 0.50f,*/ },
                    Duration, Duration);
                float Chance = Talents.ImprovedHamstring * 0.05f + (Talents.GlyphOfHamstring ? 0.10f : 0.00f);
                Effect2 = new SpecialEffect(Trigger.Use,
                    new Stats() { AttackPower = 0f, /*TargetStunned = 0.50f,*/ },
                    5f, Duration, Chance);
                //
                InitializeB();
            }
            protected override float ActivatesOverride {
                get {
                    // Since this has to be up according to Debuff Maintenance Rules
                    // this override has the intention of taking the baseline
                    // activates and forcing the char to use Rend again to ensure it's up
                    // in the event that the attemtped activate didn't land (it Missed, was Dodged or Parried)
                    // We're only doing the additional check once so it will at most do this
                    // twice in a row, may consider doing a settler
                    float result = 0f;
                    float Base = base.ActivatesOverride;
                    addMisses = (MHAtkTable.Miss  > 0) ? Base * MHAtkTable.Miss  : 0f;
                    addDodges = (MHAtkTable.Dodge > 0) ? Base * MHAtkTable.Dodge : 0f;
                    addParrys = (MHAtkTable.Parry > 0) ? Base * MHAtkTable.Parry : 0f;

                    if (CalcOpts.AllowFlooring) {
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
            /// <summary>
            /// Instant, 2 Min Cooldown, 0 Rage, Self (Any)
            /// Removes all movement impairing effects and all effects which cause loss of control of
            /// your character. This effect shares a cooldown with other similar effects.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public EveryManForHimself(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Every Man for Himself";
                if (c.Race != CharacterRace.Human) { return; }
                Cd = 2f * 60f;
                StanceOkArms = StanceOkFury = StanceOkDef = true;
                //
                InitializeB();
            }
        }
        public class HeroicFury : Ability
        {
            /// <summary>
            /// Instant, 45 sec Cooldown, 0 Rage, Self (Any)
            /// Removes any Immobilization effects and refreshes the cooldown of your Intercept ability.
            /// </summary>
            /// <TalentsAffecting></TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public HeroicFury(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Heroic Fury";
                ReqTalent = true;
                Talent2ChksValue = Talents.HeroicFury;
                Cd = 45f;
                StanceOkArms = StanceOkFury = StanceOkDef = true;
                //
                InitializeB();
            }
        }
        #endregion
        #region Movement Abilities
        public class Charge : Ability
        {
            /// <summary>
            /// Instant, 20 sec cd, 0 Rage, 8-25 yds, (Battle)
            /// Charge an enemy, generate 25 rage, and stun it for 1.50 sec. Cannot be used in combat.
            /// </summary>
            /// <TalentsAffecting>
            /// Warbringer [Usable in combat and any stance]
            /// Juggernaut [Usable in combat]
            /// Improved Charge [+(5*Pts) RageGen]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>
            /// Glyph of Rapid Charge [-20% Cd]
            /// Glyph of Charge [+5 yds MaxRange]
            /// </GlyphsAffecting>
            public Charge(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                MaxRange = 25f + (Talents.GlyphOfCharge ? 5f : 0f); // In Yards 
                Cd = (20f + Talents.Juggernaut * 5f) * (1f - (Talents.GlyphOfRapidCharge ? 0.20f : 0f)); // In Seconds
                Duration = 1.5f;
                RageCost = 25f + (Talents.ImprovedCharge * 5f);
                if (Talents.Warbringer == 1)
                {
                    StanceOkArms = StanceOkFury = StanceOkDef = true;
                }
                else if (Talents.Juggernaut == 1)
                {
                    StanceOkArms = true;
                }
                //
                InitializeB();
            }
        }
        public class Intercept : Ability
        {
            /// <summary>
            /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Zerker)
            /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
            /// </summary>
            /// <TalentsAffecting>
            /// Warbringer [Usable in any stance]
            /// Improved Intercept [-[5*Pts] sec Cd]
            /// </TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Intercept(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                MaxRange = 25f; // In Yards 
                Cd = 30f * (1f - (Talents.ImprovedIntercept * 5f)); // In Seconds
                Duration = 3f;
                RageCost = 10f - Talents.Precision * 1f;
                StanceOkFury = true; StanceOkArms = StanceOkDef = (Talents.Warbringer == 1);
                DamageBase = 380f;
                //
                InitializeB();
            }
        }
        public class Intervene : Ability
        {
            /// <summary>
            /// Instant, 30 sec Cd, 10 Rage, 8-25 yds, (Def)
            /// Charge an enemy, causing 380 damage (based on attack power) and stunning it for 3 sec.
            /// </summary>
            /// <TalentsAffecting>
            /// Warbringer [Usable in any stance]
            /// </TalentsAffecting>
            /// <GlyphsAffecting>
            /// Glyph of Intervene [Increases the number of attacks you intercept for your intervene target by 1.]
            /// </GlyphsAffecting>
            public Intervene(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                MaxRange = 25f; // In Yards 
                Cd = 30f * (1f - (Talents.ImprovedIntercept * 5f)); // In Seconds
                RageCost = 10f;
                StanceOkDef = true; StanceOkArms = StanceOkFury = (Talents.Warbringer == 1);
                //
                InitializeB();
            }
        }
        #endregion
        #region Other Abilities
        public class Retaliation : Ability
        {
            /// <summary>
            /// Instant, 5 Min Cd, No Rage, Melee Range, Melee Weapon, (Battle)
            /// Instantly counterattack any enemy that strikes you in melee for 12 sec. Melee attacks
            /// made from behind cannot be counterattacked. A maximum of 20 attacks will cause retaliation.
            /// </summary>
            /// <TalentsAffecting>Improved Disciplines [-(30*Pts) sec Cd]</TalentsAffecting>
            /// <GlyphsAffecting></GlyphsAffecting>
            public Retaliation(Character c, Stats s, CombatFactors cf, WhiteAttacks wa)
            {
                Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; InitializeA();
                //
                Name = "Retaliation";
                StanceOkArms = true;
                ReqMeleeRange = true;
                ReqMeleeWeap = true;
                //Targets += StatS.BonusTargets;
                Cd = 5f * 60f - Talents.ImprovedDisciplines * 30f;
                Duration = 12f;
                StackCap = 20f;
                //
                InitializeB();
            }
            public float StackCap;
        }
        #endregion
    }
}