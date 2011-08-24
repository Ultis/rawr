using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class SteadyShot : Ability
    {
        /// <summary>
        /// TODO Zhok: Generate Focus! Careful Aim, Dazzled Prey, Sniper Training, Termination
        /// <b>Steady Shot</b>, 5-40yd, 1.5 sec cast
        /// <para>A steady shot that causes 100% weapon damage 
        /// plus RAP*0.021+280. Generates 9 Focus.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// Careful Aim - Increases the critical strike chance of your Steady Shot, Cobra Shot and Aimed Shot by 30/60% on targets who are above 80% health.
        /// Improved Steady Shot - When you Steady Shot twice in a row, your ranged attack speed will be increased by 5/10/15% for 8 sec.
        /// Master Marksman - You have a 20/40/60% chance when you Steady Shot to gain the Master Marksman effect, lasting 30 sec. After reaching 5 stacks, your next Aimed Shot's cast time and focus cost are reduced by 100% for 10 sec.
        /// Rapid Killing - After killing an opponent that yields experience or honor, your next Aimed Shot, Steady Shot or Cobra Shot causes 10/20% additional damage.  Lasts 20 sec.
        /// Sniper Training - Increases the critical strike chance of your Kill Shot ability by 5/10/15%, and after remaining stationary for 6 sec, your Steady Shot and Cobra Shot deal 2/4/6% more damage for 15 sec.
        /// Termination - Your Steady Shot and Cobra Shot abilities grant an additional 3/6 Focus when dealt on targets at or below 25% health.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Steady Shot [+10% DMG]
        /// Glyph of Dazzled Prey - Your Steady Shot generates an additional 2 Focus on targets afflicted by a daze effect.</GlyphsAffecting>
        public SteadyShot(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Steady Shot";
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            UsesGCD = true;
            CastTime = 1.5f;
            DamageBase = combatFactors.AvgRwWeaponDmgUnhasted + (StatS.RangedAttackPower * 0.021f) + 280f;
            DamageBonus = 1f + (Talents.GlyphOfSteadyShot ? 0.10f : 0f);
            //
            Initialize();
        }
    }
}
