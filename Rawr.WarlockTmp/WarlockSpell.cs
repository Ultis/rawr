using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.WarlockTmp {

    public class WarlockSpell {

        // set via constructor (all numbers that vary between spell, but not
        // between casts of each spell)
        private String Name;
        private float ManaCost;
        private float BaseCastTime;
        private float CritChance;
        private float BaseDamage;
        private float Coefficient;
        private float BaseDamageMultiplier;
        private float BaseBonusCritMultiplier;
        public float WaitTimeBetweenCasts { get; private set; }

        // set via SetNumCasts()
        public float AvgCastTime { get; private set; }
        public float NumCasts { get; private set; }

        // set via SetDamageStats()
        private float AvgDirectDamage;
        private float AvgDirectCritDamage;
        public float AvgDamagePerCast { get; private set; }

        public WarlockSpell(
            String name,
            float percentBaseMana,
            float baseMana,
            float costMultiplier,
            float spellCastTime,
            float spellLowDamage,
            float spellHighDamage,
            float coefficient,
            float spellDamageMultiplier,
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
            Coefficient = coefficient;
            BaseDamageMultiplier = spellDamageMultiplier;
            BaseBonusCritMultiplier = spellBonusCritMultiplier;
            WaitTimeBetweenCasts = waitTimeBetweenCasts;
        }

        public virtual bool IsCastable(WarlockTalents talents) {

            return true;
        }

        public void SetNumCasts(
            float fightLength,
            float timeRemaining,
            float baseHasteDivisor,
            float delayPerSpell,
            Dictionary<String, WarlockSpell> alreadyCastSpells) {

            // TODO factor in estimated collisions
            AvgCastTime = BaseCastTime / baseHasteDivisor;
            float waitTime 
                = Math.Max(AvgCastTime + delayPerSpell, WaitTimeBetweenCasts);
            NumCasts = timeRemaining / waitTime;
        }

        public void SetDamageStats(
            float baseSpellPower,
            float hitChance,
            Dictionary<String, WarlockSpell> castSpells) {

            float multiplier = BaseDamageMultiplier;
            if (castSpells.ContainsKey("Metamorphosis")) {
                multiplier
                    += ((MetamorphosisSpell) castSpells["Metamorphosis"])
                        .GetAvgBonusDamageMultiplier();
            }

            AvgDirectDamage
                = (BaseDamage + Coefficient * baseSpellPower) * multiplier;
            AvgDirectCritDamage = AvgDirectDamage * BaseBonusCritMultiplier;
            AvgDamagePerCast
                = hitChance
                    * Utilities.GetWeightedSum(
                        AvgDirectCritDamage, 
                        CritChance,
                        AvgDirectDamage, 
                        1 - CritChance);
        }
    }

    public class MetamorphosisSpell : WarlockSpell {

        private float SpellDuration;
        private float FightDuration;

        public MetamorphosisSpell(
            WarlockTalents talents, float hasteDivisor, float fightDuration)
            : base(
                "Metamorphosis", // name
                0f, // percent base mana
                0f, // base mana
                1f, // cost multiplier
                1.5f, // cast time
                0f, // low damage
                0f, // high damage
                0f, // coefficient
                0f, // damage multiplier
                0f, // crit chance
                0f, // bonus crit
                180f * (1f - talents.Nemesis * .1f)) { // time between casts

            SpellDuration = 30f;
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
            float baseDamageMultiplier,
            float baseCritChance)
            : base(
                "Shadow Bolt", // name
                .17f, // percent base mana
                baseMana, // base mana
                1f, // cost multiplier
                3f - talents.Bane * .1f, // cast time
                690f, // low base
                770f, // high base
                .8571f + talents.ShadowAndFlame * .04f, // coefficient
                baseDamageMultiplier 
                    + talents.ShadowMastery * .03f
                    + talents.ImprovedShadowBolt * .01f, // damage multiplier
                baseCritChance
                    + talents.Devastation * .05f
                    + stats.Warlock4T8
                    + stats.Warlock2T10, // crit chance
                stats.BonusCritMultiplier + talents.Ruin * .1f, // bonus crit
                0f) { } // time between casts
    }
}
