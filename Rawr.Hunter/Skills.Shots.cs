using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    #region Shots
    public class ExplosiveShot : Ability
    {
        /// <summary>
        /// TODO Zhok: Add Efficiency, Lock and Load, Thrill of the Hunt
        /// <b>Explosive Shot</b>, 50 Focus, 5-40yd, Instant, 6 sec Cd
        /// <para>You fire an explosive charge into the enemy target, dealing
        /// [RAP * 0.273 + 453] - [RAP * 0.14 + 464] Fire Damage. The charge will
        /// blast the target every second for an additional 2 sec.</para>
        /// </summary>
        /// <TalentsAffecting>Explosive Shot (Requires Spec)</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Explosive Shot [+6% crit chance]</GlyphsAffecting>
        public ExplosiveShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Explosive Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
#if !RAWR4
            Talent2ChksValue = Talents.ExplosiveShot;
#endif
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            Cd = 6f; // In Seconds
            FocusCost = 50f;
            DamageBase = (StatS.RangedAttackPower * 0.273f + 453f);
            BonusCritChance = Talents.GlyphOfExplosiveShot ? 0.06f : 0f;
            //
            Initialize();
        }
    }
    public class ChimeraShot : Ability
    {
        /// <summary>
        /// TODO Zhok: Add Efficiency, Piercing Shots
        /// <b>Chimera Shot</b>, 50 Focus, 5-40yd, Instant, 10 sec Cd
        /// <para>An instant shot that causes ranged weapon damage 
        /// plus RAP*0.488+973, refreshing the duration of your 
        /// Serpent Sting and healing you for 5% of your total health.</para>
        /// </summary>
        /// <TalentsAffecting>Chimera Shot (Requires Talent)</TalentsAffecting>
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
            DamageBase = combatFactors.AvgRwWeaponDmgUnhasted + StatS.RangedAttackPower * 0.488f + 973;
            //
            Initialize();
        }
    }
    public class SteadyShot : Ability {
        /// <summary>
        /// TODO Zhok: Generate Focus! Careful Aim, Dazzled Prey, Sniper Training, Termination
        /// <b>Steady Shot</b>, 5% Base Mana, 5-40yd, 1.5 sec cast
        /// <para>A steady shot that causes 100% weapon damage 
        /// plus RAP*0.021+280. Generates 9 Focus.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// TODO Zhok:
        /// Improved Steady Shot [When you Steady Shot twice in a row, 
        /// your ranged attack speed will be increased by 15% for 8 sec.]
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Steady Shot [+10% DMG]</GlyphsAffecting>
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
        /// TODO Zhok:
        /// Improved Steady Shot [When you Steady Shot twice in a row, 
        /// your ranged attack speed will be increased by 15% for 8 sec.]
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Steady Shot [+10% DMG]</GlyphsAffecting>
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
            DamageBonus = 1f + (Talents.GlyphOfSteadyShot ? 0.10f : 0f);
            //
            Initialize();
        }
    }
    public class AimedShot : Ability {
        /// <summary>
        /// TODO Zhok: Careful Aim, Master Marksman, Sic 'Em
        /// <b>Aimed Shot</b>, 50 Focus, 5-40yd, Instant, 3 sec cast
        /// <para>A powerful aimed shot that deals 100% ranged 
        /// weapon damage plus (RAP * 0.48)+776.</para>
        /// </summary>
        /// <TalentsAffecting>Aimed Shot (Requires Spec)</TalentsAffecting>
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
            CastTime = 3f;
            FocusCost = 50f;
            DamageBase = combatFactors.NormalizedRwWeaponDmg + (StatS.RangedAttackPower * 0.48f) + 776;
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
        /// <TalentsAffecting></TalentsAffecting>
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
        /// <summary>
        /// TODO Zhok: Cobra Strike, Efficiency, Lock and Load, Sic 'Em, Thrill of the Hunt
        /// 
        /// <b>Arcane Shot</b>, 25 Focus, 5-40yd, Instant
        /// <para>An instant shot that causes 100% weapon damage 
        /// plus (RAP * 0.042)+252 as Arcane damage.</para>
        /// </summary>
        /// <GlyphsAffecting>Glyph of Arcane Shot [12% More DMG]</GlyphsAffecting>
        public ArcaneShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Arcane Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
#if !RAWR4
            Talent2ChksValue = Talents.AimedShot;
#endif
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            FocusCost = 25f;
            
            DamageBase = cf.AvgRwWeaponDmgUnhasted + (StatS.RangedAttackPower * 0.042f) + 252f;
            DamageBonus = 1f + (Talents.GlyphOfArcaneShot ? 0.12f : 0f);
            //
            Initialize();
        }
    }
    public class KillShot : Ability {
        /// <summary>
        /// TODO Zhok: Sniper Training
        /// <b>Kill Shot</b>, 45 Focus, 45yd, Instant, 10 sec Cd
        /// <para>You attempt to finish the wounded target off, firing a long range attack
        /// dealing 150% weapon damage plus RAP*0.30+362. </para>
        /// <para>Kill Shot can only be used on enemies that have 20% or less health.</para>
        /// </summary>
        /// <TalentsAffecting></TalentsAffecting>
        /// <GlyphsAffecting>TODO Zhok: Glyph of Kill Shot</GlyphsAffecting>
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
            Cd = 10f;
            FocusCost = 45f;
            DamageBase = cf.AvgRwWeaponDmgUnhasted + StatS.RangedAttackPower * 0.30f + 362f;
            //
            Initialize();
        }
    }
    
    #endregion

    #region DoTs
    public class BlackArrowDoT : DoT
    {
        /// <summary>
        /// TODO Zhok: Thrill of the Hunt, Toxicology, Trap Mastery
        /// <b>Black Arrow</b>, 35 Focus, 5-40yd, Instant, 30 sec Cd
        /// <para>Fires a Black Arrow at the target, dealing 2395 Shadow damage over 15 sec. 
        /// Black Arrow shares a cooldown with other Fire Trap spells.</para>
        /// </summary>
        /// <TalentsAffecting>Black Arrow (Requires Talent)</TalentsAffecting>
        /// <GlyphsAffecting></GlyphsAffecting>
        public BlackArrowDoT(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
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
            Duration = 15f;
            TimeBtwnTicks = 1f; // TODO Zhok: Haste?
            FocusCost = 35f;
            DamageBase = 2395f;
            //
            Initialize();
        }
        // TODO Zhok: ???
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
        /// <TalentsAffecting></TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Serpent Sting [+6 sec Dur]</GlyphsAffecting>
        public SerpentSting(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Serpent Sting";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            TimeBtwnTicks = 3f; // In Seconds
            Duration = 15f + (Talents.GlyphOfSerpentSting ? 6f : 0f);
            FocusCost = 25f;
            DamageBase = (StatS.RangedAttackPower * 0.4f + (460f * 15f / 3f));
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
    public class ChimeraShot_Serpent : Ability { }
    #endregion

    #region Traps
    public class ImmolationTrap : Ability { }
    public class ExplosiveTrap : Ability { }
    public class FreezingTrap : Ability { }
    public class FrostTrap : Ability { }

    #endregion

    #region Special Abilities
    public class Readiness : Ability { }
    #endregion

    #region Buff Effects
    public class BestialWrath : BuffEffect { }
    public class RapidFire : BuffEffect
    {
        /// <summary>
        /// <b>Rapid Fire</b>, 3% Base Mana, , , Instant, 5 min Cd
        /// <para>Increases ranged attack speed by 40% for 15 sec.</para>
        /// </summary>
        /// <TalentsAffecting></TalentsAffecting>
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
            //
            // TODO Zhok: Use this for Glyph and Talent.. but no Mana.. more focus ;)
            /*Effect = new SpecialEffect(Trigger.Use,
                    new Stats() { RangedHaste = 0.40f + (Talents.GlyphOfRapidFire ? 0.08f : 0f), },
                    Duration, Cd);
            if (Talents.RapidRecuperation > 0) {
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
