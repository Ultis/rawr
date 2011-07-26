using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class PiercingShots : DoT
    {
        /// <summary>
        /// <b>Piercing Shots</b>
        /// <para>Your critical Aimed, Steady and Chimera Shots cause the
        /// target to bleed for [Pts*10]% of the damage dealt over 8 sec.</para>
        /// </summary>
        /// <TalentsAffecting>Piercing Shots (Requires Talent)</TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        public PiercingShots(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Piercing Shots";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
            Talent2ChksValue = Talents.PiercingShots;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            //Cd = 30f; // In Seconds
            Duration = 8f;
            TimeBtwnTicks = 1f; // In Seconds
            DamageBase = StatS.RangedAttackPower * 0.10f + 2765f;
            //
            Initialize();
        }
        public override float TickSize
        {
            get
            {
                if (!Validated) { return 0f; }

                float damage = Damage;
                /*float DmgBonusBase = (StatS.AttackPower * combatFactors._c_rwItemSpeed) / 14f
                                   + (combatFactors.RW.MaxDamage + combatFactors.RW.MinDamage) / 2f;
                float DmgBonusU75 = 0.75f * 1.00f;
                float DmgBonusO75 = 0.25f * 1.35f;
                float DmgMod = (1f + StatS.BonusBleedDamageMultiplier)
                             * (1f + StatS.BonusDamageMultiplier)
                             * DamageBonus;
                float GlyphMOD = Talents.GlyphOfRending ? 7f / 5f : 1f;

                float damageUnder75 = (DamageBase + DmgBonusBase) * DmgBonusU75;
                float damageOver75 = (DamageBase + DmgBonusBase) * DmgBonusO75;

                float TheDamage = (damageUnder75 + damageOver75) * DmgMod;

                float TickSize = (TheDamage * GlyphMOD) / NumTicks;*/

                return Damage * DamageBonus * (1f + StatS.BonusDamageMultiplier) * (1f + StatS.BonusShadowDamageMultiplier) / NumTicks;
            }
        }
        public override float GetDPS(float acts)
        {
            float dmgonuse = TickSize;
            float numticks = NumTicks * (acts /*- addMisses - addDodges - addParrys*/);
            float result = GetDmgOverTickingTime(acts) / FightDuration;
            return result;
        }
    }
}
