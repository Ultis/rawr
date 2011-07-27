using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    #region Shots
    public class MultiShot : Ability
    {
        /// <summary>
        /// TODO Zhok: Bombardment, Serpent Spread
        /// <b>Multi-Shot</b>, 40 Focus, 5-35yd
        /// <para>Fires several missiles, hitting your current target 
        /// and all enemies within 0 yards of that target for 55% of weapon damage.</para>
        /// </summary>
        /// <TalentsAffecting>Bombardment - When you critically hit with your Multi-Shot your next Multi-Shot's focus cost will be reduced by 25/50%.
        /// Concussive Barrage - Your successful Chimera Shot and Multi-Shot attacks have a 50/100% chance to daze the target for 4 sec.
        /// Serpent Spread - Targets hit by your Multi-Shot are also afflicted by your Serpent Sting equal to 6/9 sec of its total duration.
        /// </TalentsAffecting>
        public MultiShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Multi-Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            Targets = 10f; // TODO Zhok: 10?
            FocusCost = 40f;
            DamageBase = cf.AvgRwWeaponDmgUnhasted * 1.20f;
            //
            Initialize();
        }
    }
   #endregion

    #region DoTs
    public class BlackArrowDoT : DoT
    { }

    public class BlackArrowBuff : BuffEffect
    {
        /// <summary>
        /// TODO Zhok: Thrill of the Hunt, Toxicology, Trap Mastery
        /// <b>Black Arrow</b>, 35 Focus, 5-40yd, Instant, 30 sec Cd
        /// <para>Fires a Black Arrow at the target, dealing 2395 Shadow damage over 15 sec. 
        /// Black Arrow shares a cooldown with other Fire Trap spells.</para>
        /// </summary>
        /// <TalentsAffecting>Black Arrow (Requires Talent)</TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        public BlackArrowBuff(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Black Arrow";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
            Talent2ChksValue = Talents.BlackArrow;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            Cd = 30f; //TODO Zhok: Resourcefulness ... - (2f * Talents.Resourcefulness;
            FocusCost = 35f;
            //
            Initialize();
        }
    }
    #endregion

    #region Stings
    public class ChimeraShot_Serpent : Ability { }
    #endregion

    #region Traps
    public class ImmolationTrap : Ability { }
    public class ExplosiveTrap : Ability { }
    public class FreezingTrap : Ability { }
    public class FrostTrap : Ability { }

    #endregion

    #region Special Abilities
    #endregion

    #region Buff Effects
    #endregion
}
