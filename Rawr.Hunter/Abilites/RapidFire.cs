using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class RapidFire : BuffEffect
    {
        /// <summary>
        /// <b>Rapid Fire</b>, Instant, 5 min Cd
        /// <para>Increases ranged attack speed by 40% for 15 sec.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// Posthaste - Reduces the cooldown of your Rapid Fire by 1/2 min, and your movement speed is increased by 15/30% for 4 sec after you use Disengage.
        /// Rapid Recuperation - You gain 6/12 focus every 3 sec while under the effect of Rapid Fire, and you gain 50 focus instantly when you gain Rapid Killing.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Rapid Fire [+10% Haste Bonus]</GlyphsAffecting>
        public RapidFire(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Rapid Fire";
            //AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DeathWish_;
            Cd = (5f - Talents.Posthaste) * 60f; // In Seconds
            Duration = 15f;
            UseHitTable = false;
            Effect = new SpecialEffect(Trigger.Use,
                new Stats() { RangedHaste = 0.40f + (Talents.GlyphOfRapidFire ? 0.10f : 0f), },
                Duration, Cd);
            //
            // TODO Zhok: Use this for Glyph and Talent.. but no Mana.. more focus ;)
            /*if (Talents.RapidRecuperation > 0) {
                Effect2 = new SpecialEffect(Trigger.Use,
                        new Stats() { ManaRestore = StatS.Mana * (0.02f * Talents.RapidRecuperation) * Duration / 3f, },
                        Duration, Cd);
            } else { Effect2 = null; }*/
            //
            Initialize();
        }
    }
}
