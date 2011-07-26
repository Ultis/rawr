using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class ExplosiveShot : DoT
    {
        private float _basefocuscost = 50f;
        /// <summary>
        /// TODO Zhok: Add Efficiency, Lock and Load, Thrill of the Hunt
        /// <b>Explosive Shot</b>, 50 Focus, 5-40yd, Instant, 6 sec Cd
        /// <para>You fire an explosive charge into the enemy target, dealing
        /// [RAP * 0.232 + 320] - [RAP * 0.232 + 386] Fire Damage. The charge will
        /// blast the target every second for an additional 2 sec.</para>
        /// </summary>
        /// <TalentsAffecting>Explosive Shot (Requires Spec)
        /// Efficiency - Reduces the focus cost of your Arcane Shot by 1/2/3, and your Explosive Shot and Chimera Shot by 2/4/6.
        /// Lock and Load - You have a 50/100% chance when you trap a target with Freezing Trap or Ice Trap to cause your next 2 Arcane Shot or Explosive Shot abilities to cost no focus and trigger no cooldown.
        /// Sic 'Em! - When you critically hit with your Arcane Shot, Aimed Shot or Explosive Shot the focus cost of your Pet's next basic attack is reduced by 50/100% for 12 sec.
        /// Thrill of the Hunt - You have a 5/10/15% chance when you use Arcane Shot, Explosive Shot or Black Arrow to instantly regain 40% of the base focus cost of the shot.</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Explosive Shot [+6% crit chance]</GlyphsAffecting>
        public ExplosiveShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;

            Name = "Explosive Shot";
            ReqTalent = true;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            Cd = 6f; // In Seconds
            Duration = 2f;
            TimeBtwnTicks = 1f;
            FocusCost = _basefocuscost - (Talents.Efficiency * 2f);
            // 23.2% RAP + (320 + 386)/2 (Per tick) for 2 seconds
            DamageBase = ((StatS.RangedAttackPower * 0.232f) + (320 + 386) / 2f);
            BonusCritChance = Talents.GlyphOfExplosiveShot ? 0.06f : 0f;
            Initialize();
        }
        public override float TickSize
        {
            get
            {
                if (!Validated) { return 0f; }
                return DamageBase * (1f + StatS.BonusDamageMultiplier) * (1f + StatS.BonusFireDamageMultiplier) / NumTicks;
            }
        }
        public override float GetDPS(float acts)
        {
            float dmgonuse = TickSize;
            float numticks = NumTicks * (acts /*- addMisses - addDodges - addParrys*/);
            float result = GetDmgOverTickingTime(acts) / FightDuration;
            return result;
        }
        public float GainedThrilloftheHuntFocus() { return _basefocuscost * 0.40f; }
    }
}
