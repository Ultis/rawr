using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class AimedShot : Ability
    {
        /// <summary>
        /// TODO Zhok: Careful Aim, Master Marksman, Sic 'Em
        /// <b>Aimed Shot</b>, 50 Focus, 5-40yd, Instant, 2.9 sec cast
        /// <para>A powerful aimed shot that deals 160% ranged 
        /// weapon damage plus (RAP * 0.724)+776.</para>
        /// </summary>
        /// <TalentsAffecting>Aimed Shot (Requires Spec)
        /// Careful Aim - Increases the critical strike chance of your Steady Shot, Cobra Shot and Aimed Shot by 30/60% on targets who are above 80% health.
        /// Master Marksman - You have a 20/40/60% chance when you Steady Shot to gain the Master Marksman effect, lasting 30 sec. After reaching 5 stacks, your next Aimed Shot's cast time and focus cost are reduced by 100% for 10 sec.
        /// Rapid Killing - After killing an opponent that yields experience or honor, your next Aimed Shot, Steady Shot or Cobra Shot causes 10/20% additional damage.  Lasts 20 sec.
        /// Sic 'Em! - When you critically hit with your Arcane Shot, Aimed Shot or Explosive Shot the focus cost of your Pet's next basic attack is reduced by 50/100% for 12 sec.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Aimed Shot - When you critically hit with Aimed Shot, you instantly gain 5 Focus.</GlyphsAffecting>
        public AimedShot(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Aimed Shot";
            ReqTalent = true;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            CastTime = 2.9f;
            FocusCost = 50f;
            DamageBase = (combatFactors.AvgRwWeaponDmgUnhasted + (StatS.RangedAttackPower * 0.724f) + 776) * 1.60f + 100;
            Consumes_Tier12_4pc = true;
            Initialize();
        }
    }
}
