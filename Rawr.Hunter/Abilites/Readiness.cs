using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class Readiness : Ability
    {
        /// <summary>
        /// <b>Readiness</b>, Instant, 3 min Cd
        /// <para>When activated, this ability immediately finishes the cooldown on all Hunter abilities.</para>
        /// </summary>
        /// <TalentsAffecting></TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        public Readiness(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Readiness";
            Cd = 3f * 60f; // In Seconds
            Duration = 0f;
            UseHitTable = false;
            ReqTalent = true;
            Initialize();
        }
    }
}
