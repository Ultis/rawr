using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class CobraShot : Ability
    {
        /// <summary>
        /// TODO Zhok: Generate Focus! Careful Aim, Sniper Training, Termination
        /// <b>Cobra Shot</b>, 5-40yd, 1.5 sec cast
        /// <para>Deals weapon damage plus (276 + (RAP * 0.017)) in the form of Nature damage 
        /// and increases the duration of your Serpent Sting on 
        /// the target by 6 sec.</para>
        /// <para>Generates 9 Focus.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// Careful Aim - Increases the critical strike chance of your Steady Shot, Cobra Shot and Aimed Shot by 30/60% on targets who are above 80% health.
        /// Rapid Killing - After killing an opponent that yields experience or honor, your next Aimed Shot, Steady Shot or Cobra Shot causes 10/20% additional damage.  Lasts 20 sec.
        /// Sniper Training - Increases the critical strike chance of your Kill Shot ability by 5/10/15%, and after remaining stationary for 6 sec, your Steady Shot and Cobra Shot deal 2/4/6% more damage for 15 sec.
        /// Termination - Your Steady Shot and Cobra Shot abilities grant an additional 3/6 Focus when dealt on targets at or below 25% health.
        /// </TalentsAffecting>
        /// <Note>Cobra Shot is a replacement to [Steady Shot] for beastmaster and survival hunters due to its ability 
        /// (and beastmaster and survival hunters otherwise lack of ability) to increase the duration of [Serpent Sting] without recasting it. </Note>
        public CobraShot(Character c, StatsHunter s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Cobra Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            CastTime = 2f;
            //Targets += StatS.BonusTargets;
            DamageBase = combatFactors.AvgRwWeaponDmgUnhasted + (StatS.RangedAttackPower * 0.017f) + 277.21f;
            Initialize();
        }
    }
}
