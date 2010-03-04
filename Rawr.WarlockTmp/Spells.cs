using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.WarlockTmp {

    public class Spell {

        public enum SpellTree { Affliction, Demonology, Destruction }

        // set via constructor (all numbers that vary between spell, but not
        // between casts of each spell)
        public CharacterCalculationsWarlock Mommy { get; protected set; }
        public float ManaCost { get; protected set; }
        public float BaseCastTime { get; protected set; }
        public float CritChance { get; protected set; }
        public float BaseDamage { get; protected set; }
        public float DirectCoefficient { get; protected set; }
        public float DirectDamageMultiplier { get; protected set; }
        public float BaseTickDamage { get; protected set; }
        public float NumTicks { get; protected set; }
        public float TickCoefficient { get; protected set; }
        public float BaseBonusCritMultiplier { get; protected set; }
        public float TickDamageMultiplier { get; protected set; }
        public float Cooldown { get; protected set; }

        // set via SetNumCasts()
        public float AvgCastTime { get; protected set; }
        public float NumCasts { get; protected set; }

        // set via SetDamageStats()
        public float AvgDirectDamage { get; protected set; }
        public float AvgDirectCritDamage { get; protected set; }
        public float AvgTickDamage { get; protected set; }
        public float AvgTickCritDamage { get; protected set; }
        public float AvgDamagePerCast { get; protected set; }

        #region Constructor
        public Spell(
            CharacterCalculationsWarlock mommy,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float costMultiplier,
            float spellCastTime,
            float spellLowDamage,
            float spellHighDamage,
            float directCoefficient,
            float directMultiplier,
            float baseTickDamage,
            float numTicks,
            float tickCoefficient,
            float tickMultiplier,
            float spellCritChance,
            float spellBonusCritMultiplier,
            float cooldown) {

            Mommy = mommy;
            // TODO factor in "mana cost reduction" proc trinket(s?)
            // TODO factor in mana restore procs (as cost reduction)
            ManaCost = mommy.BaseMana * percentBaseMana * costMultiplier;
            BaseCastTime = spellCastTime;
            CritChance = spellCritChance;
            BaseDamage = (spellLowDamage + spellHighDamage) / 2f;
            DirectCoefficient = directCoefficient;
            DirectDamageMultiplier = directMultiplier;
            BaseTickDamage = baseTickDamage;
            NumTicks = numTicks;
            TickCoefficient = tickCoefficient;
            TickDamageMultiplier = tickMultiplier;
            BaseBonusCritMultiplier = spellBonusCritMultiplier;
            Cooldown = cooldown;

            // apply talents that affect entire magic schools or spell trees
            WarlockTalents talents = mommy.Talents;
            if (magicSchool == MagicSchool.Shadow) {
                DirectDamageMultiplier += talents.ShadowMastery * .03f;
                TickDamageMultiplier += talents.ShadowMastery * .03f;
            }
            if (spellTree == SpellTree.Destruction) {
                BaseBonusCritMultiplier += talents.Ruin * .2f;
                CritChance += talents.Devastation * .05f;
            }
        }
        #endregion

        public virtual bool IsCastable(
            WarlockTalents talents, Dictionary<string, Spell> castSpells) {

            return true;
        }

        public virtual void SetCastingStats(
            float manaRemaining,
            float timeRemaining,
            float baseHasteDivisor,
            Dictionary<string, Spell> alreadyCast) {

            AvgCastTime = Math.Max(1f, BaseCastTime / baseHasteDivisor);

            if (Cooldown > 0) {

                // This spell is on a cooldown.
                
                // There is *LOT* of math, complexity and computation time that
                // can go into calculating collisionDelay, but I suspect this
                // is a decent approximation.
                float collisionDelay
                    = (Mommy.Options.Duration - timeRemaining)
                        / Mommy.Options.Duration;
                float avgSpellCastTime = Math.Max(1f, 2.2f / baseHasteDivisor);
                float effectiveCooldown
                    = Cooldown
                        + collisionDelay
                        + (avgSpellCastTime + Mommy.Options.Latency) / 2;
                NumCasts = Mommy.Options.Duration / effectiveCooldown;
            } else {

                // This spell is spammable.
                LifeTapSpell lt = Mommy.GetLifeTapStats();
                lt.AddCastsForRegen(
                    timeRemaining, manaRemaining, baseHasteDivisor, this);
                timeRemaining -= lt.NumCasts * lt.AvgCastTime;
                NumCasts
                    = timeRemaining / (AvgCastTime + Mommy.Options.Latency);
            }
        }

        public void SetDamageStats(
            float baseSpellPower,
            float hitChance,
            Dictionary<string, Spell> castSpells) {

            float directMultiplier = DirectDamageMultiplier;
            float tickMultiplier = TickDamageMultiplier;
            if (castSpells.ContainsKey("Metamorphosis")) {
                float morphBonus
                    = ((MetamorphosisSpell) castSpells["Metamorphosis"])
                        .GetAvgDamageMultiplier();
                directMultiplier += morphBonus;
                tickMultiplier += morphBonus;
            }

            AvgDirectDamage
                = (BaseDamage + DirectCoefficient * baseSpellPower)
                    * directMultiplier;
            AvgDirectCritDamage
                = AvgDirectDamage * (1.5f + .5f * BaseBonusCritMultiplier);

            AvgTickDamage
                = (BaseTickDamage + TickCoefficient * baseSpellPower)
                    * tickMultiplier;
            AvgTickCritDamage
                = AvgTickDamage * (1.5f + .5f * BaseBonusCritMultiplier);

            float directDamage
                = Utilities.GetWeightedSum(
                    AvgDirectCritDamage,
                    CritChance,
                    AvgDirectDamage,
                    1 - CritChance);
            float tickDamage
                = Utilities.GetWeightedSum(
                    AvgTickCritDamage,
                    CritChance,
                    AvgTickDamage,
                    1 - CritChance);
            AvgDamagePerCast
                = hitChance * (directDamage + NumTicks * tickDamage);
        }

        public String GetToolTip() {

            string toolTip
                = String.Format(
                    "{0:0}*{1:0.00}s\tAverage Cast Time\r\n",
                    AvgDamagePerCast,
                    AvgCastTime);
            if (AvgDirectDamage > 0) {
                toolTip
                    += String.Format(
                        "{0:0}\tAverage Hit\r\n"
                            + "{1:0}\tAverage Crit\r\n",
                        AvgDirectDamage,
                        AvgDirectCritDamage);
            }
            if (AvgTickDamage > 0) {
                toolTip
                    += String.Format(
                        "{0:0}\tAverage Tick\r\n"
                            + "{1:0}\tAverage Tick Crit\r\n",
                        AvgTickDamage,
                        AvgTickCritDamage);
            }
            toolTip += String.Format("{0:0.0}\tCasts", NumCasts);
            return toolTip;
        }
    }

    public class CorruptionSpell : Spell {

        private float TicksPerSec;

        public CorruptionSpell(
           CharacterCalculationsWarlock mommy,
            float tickMultiplier,
            float baseCritChance)
            : base(
                mommy,
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                .14f, // percent base mana
                1f, // cost multiplier
                1.5f, // cast time
                0f, // low damage
                0f, // high damage
                0f, // direct coefficient
                0f, // direct multiplier
                1080f / 6f, // damage per tick
                6f, // num ticks
                (1.2f
                        + mommy.Talents.EmpoweredCorruption * .12f
                        + mommy.Talents.EverlastingAffliction * .01f)
                    / 6f, // tick coefficient
                tickMultiplier
                    + mommy.Talents.ImprovedCorruption * .02f
                    + mommy.Talents.Contagion * .01f, // tick multiplier
                (baseCritChance + mommy.Talents.Malediction * .03f)
                    * mommy.Talents.Pandemic, // crit chance
                mommy.Stats.BonusCritMultiplier
                    + mommy.Talents.Pandemic * .5f, // crit multiplier
                18f) { } // "cooldown"

        public override void SetCastingStats(
            float manaRemaining,
            float timeRemaining,
            float baseHasteDivisor,
            Dictionary<string, Spell> castSpells) {

            if (Mommy.Talents.GlyphQuickDecay) {
                Cooldown /= baseHasteDivisor;
            }
            base.SetCastingStats(
                manaRemaining, timeRemaining, baseHasteDivisor, castSpells);
            TicksPerSec = 3f / baseHasteDivisor;
        }
    }

    public class LifeTapSpell : Spell {

        // This exists as a spell object only for the case when we would have
        // enough mana to never life tap, but still need to keep up the gyph. So
        // make it's cooldown 37s (40s duration of the glyph minus just about
        // enough time to make cooldown collisions & such don't let it drop
        // off).

        public LifeTapSpell(CharacterCalculationsWarlock mommy)
            : base(
                mommy,
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                0f, // percent base mana (overwritten below)
                0f, // cost multiplier
                1.5f, // cast time
                0f, // low damage
                0f, // high damage
                0f, // direct coefficient
                0f, // direct multiplier
                0f, // base tick damage
                0f, // num ticks
                0f, // tick coefficient
                0f, // tick multiplier
                0f, // crit chance
                0f, // bonus crit multiplier
                37f) { // cooldown

            ManaCost = -1490f - mommy.Stats.Spirit * 3f;
        }

        public void AddCastsForRegen(
            float timeRemaining,
            float manaRemaining,
            float baseHasteDivisor,
            Spell spammedSpell) {

            if (AvgCastTime == 0) {
                AvgCastTime = Math.Max(1f, 1.5f / baseHasteDivisor);
            }

            // The number of needed lifetaps is obtained by solving this
            // system of equations:
            //
            // spamCasts * spamMana + ltCasts * ltMana = manaRemaining
            // spamCasts * spamCast + ltCasts * ltCast = timeRemaining

            float latency = Mommy.Options.Latency;
            float a = spammedSpell.ManaCost;
            float b = ManaCost;
            float c = manaRemaining;
            float d = spammedSpell.AvgCastTime + latency;
            float e = AvgCastTime + latency;
            float f = timeRemaining;
            NumCasts += Math.Max(0f, (c * d - a * f) / (b * d - a * e));
        }
    }

    public class MetamorphosisSpell : Spell {

        public MetamorphosisSpell(CharacterCalculationsWarlock mommy)
            : base(
                mommy, // options
                0, // magic school
                SpellTree.Demonology, // spell tree
                0f, // percent base mana
                1f, // cost multiplier
                1.5f, // cast time
                0f, // low damage
                0f, // high damage
                0f, // direct coefficient
                0f, // direct multiplier
                0f, // damage per tick
                0f, // num ticks
                0f, // tick coefficient
                0f, // tick multiplier
                0f, // crit chance
                0f, // bonus crit
                180f
                    * (1f
                        - mommy.Talents.Nemesis * .1f)) { } // cooldown

        public override bool IsCastable(
            WarlockTalents talents, Dictionary<string, Spell> castSpells) {

            return talents.Metamorphosis > 0;
        }

        public override void SetCastingStats(
            float manaRemaining,
            float timeRemaining,
            float baseHasteDivisor,
            Dictionary<string, Spell> castSpells) {

            base.SetCastingStats(
                manaRemaining, timeRemaining, baseHasteDivisor, castSpells);

            // Discretize NumCasts.  This makes sense becasue of this spell's
            // long cooldown, so that it's (correctly) modelled as more
            // valuable in a 4 minute fight than in a 5 minute fight.

            float maxUprate = GetSpellDuration() / Cooldown;
            float wholeCasts = (float) Math.Floor(NumCasts);
            float partialCast = NumCasts - wholeCasts;
            NumCasts = wholeCasts + Math.Min(1f, partialCast / maxUprate);
        }

        public float GetAvgDamageMultiplier() {

            float uprate
                = NumCasts * GetSpellDuration() / Mommy.Options.Duration;
            return .2f * uprate;
        }

        private float GetSpellDuration() {

            if (Mommy.Talents.GlyphMetamorphosis) {
                return 36f;
            } else {
                return 30f;
            }
        }
    }

    public class ShadowBoltSpell : Spell {

        public ShadowBoltSpell(
            CharacterCalculationsWarlock mommy,
            float directMultiplier,
            float baseCritChance)
            : base(
                mommy, // options
                MagicSchool.Shadow, // magic school
                SpellTree.Destruction, // spell tree
                .17f, // percent base mana
                1f, // cost multiplier
                3f - mommy.Talents.Bane * .1f, // cast time
                690f, // low base
                770f, // high base
                .8571f
                    + mommy.Talents.ShadowAndFlame * .04f, // direct coefficient
                directMultiplier
                    + mommy.Talents.ImprovedShadowBolt
                        * .01f, // damage multiplier
                0f, // damage per tick
                0f, // num ticks
                0f, // tick coefficient
                0f, // tick multiplier
                baseCritChance
                    + mommy.Stats.Warlock4T8
                    + mommy.Stats.Warlock2T10, // crit chance
                mommy.Stats.BonusCritMultiplier, // bonus crit
                0f) { } // cooldown
    }

    public class InstantShadowBoltSpell : ShadowBoltSpell {

        public InstantShadowBoltSpell(
            CharacterCalculationsWarlock mommy,
            float directMultiplier,
            float baseCritChance)
            : base(
                mommy,
                directMultiplier,
                baseCritChance) {

            BaseCastTime = 1.5f;
        }

        public override bool IsCastable(
            WarlockTalents talents, Dictionary<string, Spell> castSpells) {

            return castSpells.ContainsKey("Corruption")
                && (Mommy.Talents.GlyphCorruption
                    || Mommy.Talents.Nightfall > 0);
        }

        public override void SetCastingStats(
            float manaRemaining,
            float timeRemaining,
            float baseHasteDivisor,
            Dictionary<string, Spell> castSpells) {

            // Currently modeled as a spell on a cooldown equal to the
            // average time between procs.  This lengthens the time between
            // casts according to the rules for cooldown collision, which is not
            // completely accurate, but close enough.  To be accurate it
            // should instead model the probability that it will proc twice (or
            // more) during the casting of higher priority spells.
            float procChance = Mommy.Talents.Nightfall * .02f;
            if (Mommy.Talents.GlyphCorruption) {
                procChance += .04f;
            }
            Spell corruption = castSpells["Corruption"];
            float numProcs
                = procChance * corruption.NumCasts * corruption.NumTicks;
            Cooldown = Mommy.Options.Duration / numProcs;
            base.SetCastingStats(
                manaRemaining, timeRemaining, baseHasteDivisor, castSpells);
        }
    }
}
