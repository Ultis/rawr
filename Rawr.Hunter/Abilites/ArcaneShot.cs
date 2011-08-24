using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class ArcaneShot : Ability
    {

        private float _basefocuscost = 25f;
        /// <summary>
        /// TODO Zhok: Cobra Strike, Efficiency, Lock and Load, Sic 'Em, Thrill of the Hunt
        /// 
        /// <b>Arcane Shot</b>, 25 Focus, 5-40yd, Instant
        /// <para>An instant shot that causes 100% weapon damage 
        /// plus (RAP * 0.0483)+289 as Arcane damage.</para>
        /// </summary>
        /// <TalentsAffecting>Cobra Strikes - You have a 5/10/15% chance when you hit with Arcane Shot to cause your pet's next 2 Basic Attacks to critically hit.
        /// Efficiency - Reduces the focus cost of your Arcane Shot by 1/2/3, and your Explosive Shot and Chimera Shot by 2/4/6.
        /// Lock and Load - You have a 50/100% chance when you trap a target with Freezing Trap or Ice Trap to cause your next 2 Arcane Shot or Explosive Shot abilities to cost no focus and trigger no cooldown.
        /// Marked for Death - Your Arcane Shot and Chimera Shot have a 50/100% chance to automatically apply the Marked for Death effect.
        /// Sic 'Em! - When you critically hit with your Arcane Shot, Aimed Shot or Explosive Shot the focus cost of your Pet's next basic attack is reduced by 50/100% for 12 sec.
        /// Thrill of the Hunt - You have a 5/10/15% chance when you use Arcane Shot, Explosive Shot or Black Arrow to instantly regain 40% of the base focus cost of the shot.</TalentsAffecting>
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Arcane Shot [12% More DMG]</GlyphsAffecting>
        public ArcaneShot(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;

            Name = "Arcane Shot";

            ReqTalent = true;
            ReqRangedWeap = true;
            ReqSkillsRange = true;

            FocusCost = _basefocuscost - (Talents.Efficiency * 2f);

            DamageBase = cf.AvgRwWeaponDmgUnhasted + (StatS.RangedAttackPower * 0.0483f) + 289f;
            DamageBonus = (Talents.GlyphOfArcaneShot ? 0.12f : 0f);

            Consumes_Tier12_4pc = true;

            Initialize();
        }

        public float GainedThrilloftheHuntFocus() { return _basefocuscost * 0.40f; }
    }
}
