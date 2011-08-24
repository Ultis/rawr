using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class BestialWrath : BuffEffect
    {
        /// <summary>
        /// <b>Bestial Wrath</b>, Instant, 2 min cooldown
        /// <para>Send your pet into a rage causing 20% additional damage for 10 sec.  The beast does not feel pity or remorse or fear and it cannot be stopped unless killed.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// Longevity - Reduces the cooldown of your Bestial Wrath, Intimidation and Pet Special Abilities by 10/20/30%.
        /// The Beast Within - While your pet is under the effects of Bestial Wrath, you also go into a rage causing 10% additional damage and reducing the focus cost of all shots and abilities by 50% for 10 sec.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Bestial Wrath - Decreases the cooldown of Bestial Wrath by 20 sec.</GlyphsAffecting>
        public BestialWrath(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Bestial Wrath";
            Cd = ((2f * 60f) * (1f - Talents.Longevity)) - (Talents.GlyphOfBestialWrath ? 20f : 0f); // In Seconds
            Duration = 10f;
            UseHitTable = false;
            ReqTalent = true;
            // TODO: Move these to static SEs.
            Effect = new SpecialEffect(Trigger.Use,
                new Stats() { BonusPetDamageMultiplier = 0.20f }, Duration, Cd);
            if (Talents.TheBeastWithin > 0f)
            {
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { BonusDamageMultiplier = 0.10f }, Duration, Cd);
            }
            Initialize();
        }

        
    }
}
