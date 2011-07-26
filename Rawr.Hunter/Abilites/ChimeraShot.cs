using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class ChimeraShot : Ability
    {
        /// <summary>
        /// TODO Zhok: Add Efficiency, Piercing Shots
        /// <b>Chimera Shot</b>, 50 Focus, 5-40yd, Instant, 10 sec Cd
        /// <para>An instant shot that causes ranged weapon damage 
        /// plus RAP*0.732+1620, refreshing the duration of your 
        /// Serpent Sting and healing you for 5% of your total health.</para>
        /// </summary>
        /// <TalentsAffecting>Chimera Shot (Requires Talent)
        /// Concussive Barrage - Your successful Chimera Shot and Multi-Shot attacks have a 50/100% chance to daze the target for 4 sec.
        /// Efficiency - Reduces the focus cost of your Arcane Shot by 1/2/3, and your Explosive Shot and Chimera Shot by 2/4/6.
        /// Marked for Death - Your Arcane Shot and Chimera Shot have a 50/100% chance to automatically apply the Marked for Death effect.
        /// Piercing Shots - Your critical Aimed, Steady and Chimera Shots cause the target to bleed for 10/20/30% of the damage dealt over 8 sec.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Chimera Shot [-1 sec Cd]</GlyphsAffecting>
        public ChimeraShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Chimera Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
            Talent2ChksValue = Talents.ChimeraShot;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            Cd = 10f - (Talents.GlyphOfChimeraShot ? 1f : 0f); // In Seconds
            FocusCost = 50f - (Talents.Efficiency * 2f);
            DamageBase = combatFactors.AvgRwWeaponDmgUnhasted + StatS.RangedAttackPower * 0.732f + 1620;
            RefreshesSS = true;
            //
            Initialize();
        }

        public float piercingShots_TickSize()
        {
            float damage = Damage * Talents.PiercingShots * 0.10f;
            float NumTicks = 8f;
            return damage * DamageBonus * (1f + StatS.BonusDamageMultiplier) * (1f + StatS.BonusNatureDamageMultiplier) / NumTicks;
        }

        public float piercingShots_GetDPS(float acts)
        {
            float dmgonuse = piercingShots_TickSize();
            float numticks = 8f * acts;
            float result = (dmgonuse * numticks) / FightDuration;
            return result;
        }
    }
}
