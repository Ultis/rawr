using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    #region Shots
    public class ExplosiveShot : Ability
    {
        private float basefocuscost = 50f;
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
            FocusCost = basefocuscost - (Talents.Efficiency * 2f);
            DamageBase = (StatS.RangedAttackPower * 0.232f + 386f);
            
            BonusCritChance = Talents.GlyphOfExplosiveShot ? 0.06f : 0f;
            //
            Initialize();
        }
        public void Mastery(int tree, float mastery)
        {
            // if tree = 2 or survival tree, Mastery adds a damage bonus
            if (tree == 2) { DamageBonus = 1f + mastery; }
        }
        public float GainedThrilloftheHuntFocus() { return basefocuscost * 0.40f; }
    }
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
            FocusCost = 50f;
            DamageBase = combatFactors.AvgRwWeaponDmgUnhasted + StatS.RangedAttackPower * 0.732f + 1620;
            //
            Initialize();
        }
    }
    public class SteadyShot : Ability {
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
        public SteadyShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Steady Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            CastTime = 1.5f;
            //Targets += StatS.BonusTargets;
            DamageBase = combatFactors.AvgRwWeaponDmgUnhasted
                       + StatS.RangedAttackPower * 0.021f + 280f;
            DamageBonus = 1f + (Talents.GlyphOfSteadyShot ? 0.10f : 0f);
            //
            Initialize();
        }
    }
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
        public CobraShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Steady Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            CastTime = 1.5f;
            //Targets += StatS.BonusTargets;
            DamageBase = combatFactors.AvgRwWeaponDmgUnhasted
                       + (276 + (StatS.RangedAttackPower * 0.017f));
            Initialize();
        }
        public void Mastery(int tree, float mastery)
        {
            // if tree = 2 or survival tree, Mastery adds a damage bonus
            if (tree == 2) { DamageBonus = 1f + mastery; }
        }
    }
    public class AimedShot : Ability {
        /// <summary>
        /// TODO Zhok: Careful Aim, Master Marksman, Sic 'Em
        /// <b>Aimed Shot</b>, 50 Focus, 5-40yd, Instant, 2.9 sec cast
        /// <para>A powerful aimed shot that deals 200% ranged 
        /// weapon damage plus (RAP * 0.724)+776.</para>
        /// </summary>
        /// <TalentsAffecting>Aimed Shot (Requires Spec)
        /// Careful Aim - Increases the critical strike chance of your Steady Shot, Cobra Shot and Aimed Shot by 30/60% on targets who are above 80% health.
        /// Master Marksman - You have a 20/40/60% chance when you Steady Shot to gain the Master Marksman effect, lasting 30 sec. After reaching 5 stacks, your next Aimed Shot's cast time and focus cost are reduced by 100% for 10 sec.
        /// Rapid Killing - After killing an opponent that yields experience or honor, your next Aimed Shot, Steady Shot or Cobra Shot causes 10/20% additional damage.  Lasts 20 sec.
        /// Sic 'Em! - When you critically hit with your Arcane Shot, Aimed Shot or Explosive Shot the focus cost of your Pet's next basic attack is reduced by 50/100% for 12 sec.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Aimed Shot - When you critically hit with Aimed Shot, you instantly gain 5 Focus.</GlyphsAffecting>
        public AimedShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Aimed Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
#if !RAWR4
            Talent2ChksValue = Talents.AimedShot;
#endif
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            CastTime = 2.9f;
            FocusCost = 50f;
            DamageBase = (combatFactors.NormalizedRwWeaponDmg * 2.00f) + (StatS.RangedAttackPower * 0.724f) + 776;
            //
            Initialize();
        }
    }
    public class MultiShot : Ability {
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
            DamageBase = cf.AvgRwWeaponDmgUnhasted * 0.55f;
            //
            Initialize();
        }
    }
    public class ArcaneShot : Ability {

        private float basefocuscost = 25f;
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
        public ArcaneShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;

            Name = "Arcane Shot";

            ReqTalent = true;
#if !RAWR4
            Talent2ChksValue = Talents.AimedShot;
#endif
            ReqRangedWeap = true;
            ReqSkillsRange = true;

            FocusCost = basefocuscost - (Talents.Efficiency * 2f);

            DamageBase = cf.AvgRwWeaponDmgUnhasted + (StatS.RangedAttackPower * 0.0483f) + 289f;
            DamageBonus = 1f + (Talents.GlyphOfArcaneShot ? 0.12f : 0f);

            Initialize();
        }
        public void Mastery(int tree, float mastery)
        {
            // if tree = 2 or survival tree, Mastery adds a damage bonus
            if (tree == 2) { DamageBonus = 1f + mastery; }
        }
        public float GainedThrilloftheHuntFocus() { return basefocuscost * 0.40f; }
    }
    public class KillShot : Ability {
        /// <summary>
        /// TODO Zhok: Sniper Training
        /// <b>Kill Shot</b>, 45 Focus, 45yd, Instant, 10 sec Cd
        /// <para>You attempt to finish the wounded target off, firing a long range attack
        /// dealing 150% weapon damage plus RAP*0.30+543. Kill Shot can only be used on
        /// enemies that have 20% or less health.</para>
        /// <para>Kill Shot can only be used on enemies that have 20% or less health.</para>
        /// </summary>
        /// <TalentsAffecting>Sniper Training - Increases the critical strike chance of your Kill Shot ability by 5/10/15%, and after remaining stationary for 6 sec, your Steady Shot and Cobra Shot deal 2/4/6% more damage for 15 sec.</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Kill Shot - If the damage from your Kill Shot fails to kill a target at or below 20% health, your Kill Shot's cooldown is instantly reset. This effect has a 6 sec cooldown.</GlyphsAffecting>
        public KillShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Kill Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
#if !RAWR4
            Talent2ChksValue = Talents.AimedShot;
#endif
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            // In terms of modeling, the Glyph of Kill Shot is basically a 4 second cooldown reduction.
            Cd = 10 - (Talents.GlyphOfKillShot ? 4f : 0f);
            FocusCost = 45f;
            DamageBase = cf.AvgRwWeaponDmgUnhasted + StatS.RangedAttackPower * 0.30f + 543f;
            //
            Initialize();
        }
    }
    
    #endregion

    #region DoTs
    public class BlackArrowDoT : DoT
    {
        private float basefocuscost = 35f;
        /// <summary>
        /// TODO Zhok: Thrill of the Hunt, Toxicology, Trap Mastery
        /// <b>Black Arrow</b>, 35 Focus, 5-40yd, Instant, 30 sec Cd
        /// <para>Fires a Black Arrow at the target, dealing 2035 Shadow damage over 15 sec. 
        /// Black Arrow shares a cooldown with other Fire Trap spells.</para>
        /// </summary>
        /// <TalentsAffecting>Black Arrow (Requires Talent)
        /// Resourcefulness - Reduces the cooldown of all traps and Black Arrow by 2/4/6 sec.
        /// T.N.T - When you deal periodic damage with your Immolation Trap, Explosive Trap or Black Arrow you have a 6/12% chance to trigger Lock and Load.
        /// Thrill of the Hunt - You have a 5/10/15% chance when you use Arcane Shot, Explosive Shot or Black Arrow to instantly regain 40% of the base focus cost of the shot.
        /// Toxicology - Increases the periodic critical damage of your Serpent Sting and Black Arrow by 50/100%.
        /// Trap Mastery - Immolation Trap, Explosive Trap and Black Arrow - Increases the periodic damage done by 10/20/30%.
        /// </TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        public BlackArrowDoT(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;

            Name = "Black Arrow";

            ReqTalent = true;
            Talent2ChksValue = Talents.BlackArrow;
            ReqRangedWeap = true;
            ReqSkillsRange = true;

            Cd = 30f - (Talents.Resourcefulness * 2f);
            Duration = 15f;
            TimeBtwnTicks = 1f; // TODO Zhok: Haste?
            FocusCost = basefocuscost;
            DamageBase = 2035f;
            DamageBonus = 1f + (Talents.TrapMastery * .10f);

            Initialize();
        }
        public override float TickSize {
            get {
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
        public void Mastery(int tree, float mastery)
        {
            // if tree = 2 or survival tree, Mastery adds a damage bonus
            if (tree == 2) { DamageBonus = 1f + mastery; }
        }
        public float GainedThrilloftheHuntFocus() { return basefocuscost * 0.40f; }
    }
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
    #endregion

    #region Stings
    public class SerpentSting : DoT {
        /// <summary>
        /// <b>Serpent Sting</b>, 25 Focus,5-40yd, Instant, No Cd
        /// <para>Causes (RAP * 0.4 + (460 * 15 sec / 3)) Nature damage over 15 sec.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// Chimera Shot - An instant shot that causes ranged weapon damage plus RAP*0.732+1620, refreshing the duration of  your Serpent Sting and healing you for 5% of your total health.
        /// Noxious Stings - Increases your damage done on targets afflicted by your Serpent Sting by 5/10%.
        /// Serpent Spread - Targets hit by your Multi-Shot are also afflicted by your Serpent Sting equal to 6/9 sec of its total duration.
        /// Toxicology - Increases the periodic critical damage of your Serpent Sting and Black Arrow by 50/100%.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Serpent Sting - Increases the periodic critical strike chance of your Serpent Sting by 6%.</GlyphsAffecting>
        public SerpentSting(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            Name = "Serpent Sting";
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            TimeBtwnTicks = 3f; // In Seconds
            Duration = 15f;
            FocusCost = 25f;
            DamageBase = (StatS.RangedAttackPower * 0.4f + (460f * 15f / 3f));
            BonusCritChance = 1f + (Talents.GlyphOfSerpentSting ? 0.06f : 0f) + (Talents.ImprovedSerpentSting * 0.05f);
            MinRange = 5f;
            MaxRange = 40f;
            CanCrit = true;
            StatS.BonusDamageMultiplier = (.05f * Talents.NoxiousStings);
            StatS.BonusCritMultiplier = (.5f * Talents.Toxicology);
            // TODO zhok: Glyph of Serpant Sting ... 6% crit buff
            // Improved Serpent Sting
            // Noxious Stings
            //
            Initialize();
        }
        public override float TickSize
        {
            get
            {
                if (!Validated) { return 0f; }

                return getTotalDamage / NumTicks;
            }
        }
        public override float GetDPS(float acts)
        {
            float dmgonuse = TickSize;
            float numticks = NumTicks * acts;
            float result = (GetDmgOverTickingTime(acts) / FightDuration) + (getTotalDamage * (.15f * Talents.ImprovedSerpentSting));
            return result;
        }
        public float getTotalDamage
        {
            get
            {
                if (!Validated) { return 0f; }

                return (Damage * DamageBonus * (1f + StatS.BonusDamageMultiplier) * (1f + StatS.BonusNatureDamageMultiplier));
            }
        }
        public void Mastery(int tree, float mastery)
        {
            // if tree = 2 or survival tree, Mastery adds a damage bonus
            if (tree == 2) { DamageBonus *= 1f + mastery; }
        }
    }
    public class ChimeraShot_Serpent : Ability { }
    #endregion

    #region Traps
    public class ImmolationTrap : Ability { }
    public class ExplosiveTrap : Ability { }
    public class FreezingTrap : Ability { }
    public class FrostTrap : Ability { }

    #endregion

    #region Special Abilities
    public class Readiness : Ability
    {
        /// <summary>
        /// <b>Readiness</b>, Instant, 3 min Cd
        /// <para>When activated, this ability immediately finishes the cooldown on all Hunter abilities.</para>
        /// </summary>
        /// <TalentsAffecting></TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        public Readiness(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
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
    #endregion

    #region Buff Effects
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
        public BestialWrath(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Bestial Wrath";
            Cd = ((2f * 60f) * (1f - Talents.Longevity)) - (Talents.GlyphOfBestialWrath ? 20f : 0f); // In Seconds
            Duration = 10f;
            UseHitTable = false;
            ReqTalent = true;
            Effect = new SpecialEffect(Trigger.Use,
                new Stats() { BonusPetDamageMultiplier = 0.20f }, Duration, Cd);
            if (Talents.TheBeastWithin > 0f)
            {
                Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { BonusDamageMultiplier = 0.10f }, Duration, Cd);
            }
            //
            // TODO Zhok: Use this for Glyph and Talent.. but no Mana.. more focus ;)
            /*if (Talents.RapidRecuperation > 0) {
                Effect2 = new SpecialEffect(Trigger.Use,
                        new Stats() { ManaRestore = StatS.Mana * (0.02f * Talents.RapidRecuperation) * Duration / 3f, },
                        Duration, Cd);
            } else { Effect2 = null; }*/
            //
            Initialize();
        }
    }
    public class RapidFire : BuffEffect
    {
        /// <summary>
        /// <b>Rapid Fire</b>, Instant, 5 min Cd
        /// <para>Increases ranged attack speed by 40% for 15 sec.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// Posthaste - Reduces the cooldown of your Rapid Fire by 1/2 min, and your movement speed is increased by 15/30% for 4 sec after you use Disengage.
        /// Rapid Recuperation - You gain 6/12 focus every 3 sec while under the effect of Rapid Fire, and you gain 50 focus instantly when you gain Rapid Killing.
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Rapid Fire [+10% Haste Bonus]</GlyphsAffecting>
        public RapidFire(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Rapid Fire";
            //AbilIterater = (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.DeathWish_;
            Cd = (5f - Talents.Posthaste) * 60f; // In Seconds
            Duration = 15f;
            UseHitTable = false;
            Effect = new SpecialEffect(Trigger.Use,
                new Stats() { RangedHaste = 0.40f + (Talents.GlyphOfRapidFire ? 0.10f : 0f), },
                Duration, Cd);
            //
            // TODO Zhok: Use this for Glyph and Talent.. but no Mana.. more focus ;)
            /*if (Talents.RapidRecuperation > 0) {
                Effect2 = new SpecialEffect(Trigger.Use,
                        new Stats() { ManaRestore = StatS.Mana * (0.02f * Talents.RapidRecuperation) * Duration / 3f, },
                        Duration, Cd);
            } else { Effect2 = null; }*/
            //
            Initialize();
        }
    }
    #endregion
}
