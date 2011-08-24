using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class SerpentSting : DoT
    {
        /// <summary>
        /// <b>Serpent Sting</b>, 25 Focus,5-40yd, Instant, No Cd
        /// <para>Causes (RAP * 0.4 + (460 * 15 sec / 3)) Nature damage over 15 sec.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// Chimera Shot - An instant shot that causes ranged weapon damage plus RAP*0.732+1620, refreshing the duration of  your Serpent Sting and healing you for 5% of your total health.
        /// Noxious Stings - Increases your damage done on targets afflicted by your Serpent Sting by 5/10%.
        /// Serpent Spread - Targets hit by your Multi-Shot are also afflicted by your Serpent Sting equal to 6/9 sec of its total duration.
        /// Toxicology - Increases the periodic critical damage of your Serpent Sting and Black Arrow by 50/100%.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Serpent Sting - Increases the periodic critical strike chance of your Serpent Sting by 6%.</GlyphsAffecting>
        public SerpentSting(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            Name = "Serpent Sting";
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            TimeBtwnTicks = 3f; // In Seconds
            Duration = 15f;
            FocusCost = 25f;
            DamageBase = (StatS.RangedAttackPower * 0.4f + (460f * 15f / 3f));
            BonusCritChance = 1f + (Talents.GlyphOfSerpentSting ? 0.06f : 0f) + (Talents.ImprovedSerpentSting * 0.05f);
            MinRange = 5f;
            MaxRange = 40f;
            CanCrit = true;
            StatS.BonusDamageMultiplier = (.05f * Talents.NoxiousStings);
            StatS.BonusCritDamageMultiplier = (.5f * Talents.Toxicology);
            // TODO zhok: Glyph of Serpant Sting ... 6% crit buff
            // Improved Serpent Sting
            // Noxious Stings
            //
            Initialize();
        }
        public override float TickSize
        {
            get
            {
                if (!Validated) { return 0f; }

                return getTotalDamage / NumTicks;
            }
        }
        public override float GetDPS(float acts)
        {
            float dmgonuse = TickSize;
            float numticks = NumTicks * acts;
            float result = (GetDmgOverTickingTime(acts) / FightDuration) + (getTotalDamage * (.15f * Talents.ImprovedSerpentSting));
            return result;
        }
        public float getTotalDamage
        {
            get
            {
                if (!Validated) { return 0f; }

                return (Damage * DamageBonus * (1f + StatS.BonusDamageMultiplier) * (1f + StatS.BonusNatureDamageMultiplier));
            }
        }
    }
}
