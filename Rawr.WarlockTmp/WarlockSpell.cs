using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.WarlockTmp {

    public class WarlockSpell {

        public enum SpellTree { Affliction, Demonology, Destruction }

        // set via constructor (all numbers that vary between spell, but not
        // between casts of each spell)
        private string Name;
        private float ManaCost;
        private float BaseCastTime;
        private float CritChance;
        private float BaseDamage;
        private float DirectCoefficient;
        private float DirectDamageMultiplier;
        private float BaseTickDamage;
        private float NumTicks;
        private float TickCoefficient;
        private float BaseBonusCritMultiplier;
        private float TickDamageMultiplier;
        public float WaitTimeBetweenCasts { get; protected set; }

        // set via SetNumCasts()
        public float AvgCastTime { get; private set; }
        public float NumCasts { get; private set; }

        // set via SetDamageStats()
        private float AvgDirectDamage;
        private float AvgDirectCritDamage;
        private float AvgTickDamage;
        private float AvgTickCritDamage;
        public float AvgDamagePerCast { get; private set; }

        #region Constructor
        public WarlockSpell(
            WarlockTalents talents,
            string name,
            MagicSchool magicSchool,
            SpellTree spellTree,
            float percentBaseMana,
            float baseMana,
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
            float waitTimeBetweenCasts) {

            Name = name;
            // TODO factor in "mana cost reduction" proc trinket(s?)
            // TODO factor in mana restore procs (as cost reduction)
            ManaCost = baseMana * percentBaseMana * costMultiplier;
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
            WaitTimeBetweenCasts = waitTimeBetweenCasts;

            // apply talents that affect entire magic schools or spell trees
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

        public virtual bool IsCastable(WarlockTalents talents) {

            return true;
        }

        public virtual void SetCastingStats(
            float fightLength,
            float timeRemaining,
            float baseHasteDivisor,
            float delayPerSpell,
            Dictionary<string, WarlockSpell> alreadyCastSpells) {

            AvgCastTime = BaseCastTime / baseHasteDivisor;
            if (WaitTimeBetweenCasts > AvgCastTime) {
                float sumCastTimes = 0f;
                foreach (
                    KeyValuePair<String, WarlockSpell> pair
                    in alreadyCastSpells) {

                    sumCastTimes += pair.Value.AvgCastTime;
                }
                if (sumCastTimes > 0) {
                    float avgCastOfPrevSpells
                        = sumCastTimes / alreadyCastSpells.Count;
                    float chanceOfCollision
                        = (fightLength - timeRemaining) / timeRemaining;
                    float collisionDelay
                        = avgCastOfPrevSpells / 2f * chanceOfCollision;
                    WaitTimeBetweenCasts += collisionDelay;
                }
                NumCasts = timeRemaining / WaitTimeBetweenCasts;
            } else {
                NumCasts = timeRemaining / (AvgCastTime + delayPerSpell);
            }
        }

        public void SetDamageStats(
            float baseSpellPower,
            float hitChance,
            Dictionary<string, WarlockSpell> castSpells) {

            float directMultiplier = DirectDamageMultiplier;
            float tickMultiplier = TickDamageMultiplier;
            if (castSpells.ContainsKey("Metamorphosis")) {
                float morphBonus
                    = ((MetamorphosisSpell) castSpells["Metamorphosis"])
                        .GetAvgBonusDamageMultiplier();
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

    public class CorruptionSpell : WarlockSpell {

        private bool Hasted;

        public CorruptionSpell(
            WarlockTalents talents,
            Stats stats,
            float baseMana,
            float tickMultiplier,
            float baseCritChance)
            : base(
                talents, // talents
                "Corruption", // name
                MagicSchool.Shadow, // magic school
                SpellTree.Affliction, // spell tree
                .14f, // percent base mana
                baseMana, // base mana
                1f, // cost multiplier
                1.5f, // cast time
                0f, // low damage
                0f, // high damage
                0f, // direct coefficient
                0f, // direct multiplier
                1080f / 6f, // damage per tick
                6f, // num ticks
                (1.2f
                        + talents.EmpoweredCorruption * .12f
                        + talents.EverlastingAffliction * .01f)
                    / 6f, // tick coefficient
                tickMultiplier
                    + talents.ImprovedCorruption * .02f
                    + talents.Contagion * .01f, // tick multiplier
                (baseCritChance + talents.Malediction * .03f)
                    * talents.Pandemic, // crit chance
                stats.BonusCritMultiplier
                    + talents.Pandemic * .5f, // crit multiplier
                18f) { // time between casts

            Hasted = talents.GlyphQuickDecay;
        }

        public override void SetCastingStats(
            float fightLength,
            float timeRemaining,
            float baseHasteDivisor,
            float delayPerSpell,
            Dictionary<string, WarlockSpell> alreadyCastSpells) {

            if (Hasted) {
                WaitTimeBetweenCasts /= baseHasteDivisor;
            }
            base.SetCastingStats(
                fightLength,
                timeRemaining,
                baseHasteDivisor,
                delayPerSpell,
                alreadyCastSpells);
        }
    }

    public class MetamorphosisSpell : WarlockSpell {

        private float SpellDuration;
        private float FightDuration;

        public MetamorphosisSpell(
            WarlockTalents talents, float hasteDivisor, float fightDuration)
            : base(
                talents, // talents
                "Metamorphosis", // name
                0, // magic school
                SpellTree.Demonology, // spell tree
                0f, // percent base mana
                0f, // base mana
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
                180f * (1f - talents.Nemesis * .1f)) { // time between casts

            SpellDuration = 30f;
            if (talents.GlyphMetamorphosis) {
                SpellDuration += 6f;
            }
            FightDuration = fightDuration;
        }

        public override bool IsCastable(WarlockTalents talents) {

            return talents.Metamorphosis > 0;
        }

        public float GetAvgBonusDamageMultiplier() {

            float uprate = NumCasts * SpellDuration / FightDuration;
            return .2f * uprate;
        }
    }

    public class ShadowBoltSpell : WarlockSpell {

        public ShadowBoltSpell(
            WarlockTalents talents,
            Stats stats,
            float baseMana,
            float directMultiplier,
            float baseCritChance)
            : base(
                talents, // talents
                "Shadow Bolt", // name
                MagicSchool.Shadow, // magic school
                SpellTree.Destruction, // spell tree
                .17f, // percent base mana
                baseMana, // base mana
                1f, // cost multiplier
                3f - talents.Bane * .1f, // cast time
                690f, // low base
                770f, // high base
                .8571f + talents.ShadowAndFlame * .04f, // direct coefficient
                directMultiplier
                    + talents.ImprovedShadowBolt * .01f, // damage multiplier
                0f, // damage per tick
                0f, // num ticks
                0f, // tick coefficient
                0f, // tick multiplier
                baseCritChance
                    + stats.Warlock4T8
                    + stats.Warlock2T10, // crit chance
                stats.BonusCritMultiplier, // bonus crit
                0f) { } // time between casts
    }
}
