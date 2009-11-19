using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Hunter.Skills
{
    public class ExplosiveShot : Ability
    {
        /// <summary>
        /// <b>Explosive Shot</b>, 10% Base Mana, 5-35yd, Instant, 6 sec Cd
        /// <para>You fire an explosive charge into the enemy target, dealing
        /// [RAP * 0.14 + 386] - [RAP * 0.14 + 464] Fire Damage. The charge will
        /// blast the target every second for an additional 2 sec.</para>
        /// </summary>
        /// <TalentsAffecting>Explosive Shot (Requires Talent)</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Explosive Shot [+4% crit chance]</GlyphsAffecting>
        public ExplosiveShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Explosive Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
            Talent2ChksValue = Talents.ExplosiveShot;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            Cd = 6f; // In Seconds
            ManaCostisPerc = true;
            ManaCost = 0.10f; // 10% of base mana, but higher shots are 7% so we need to verify
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = StatS.RangedAttackPower * 0.14f + ((386f + 464f) / 2f);
            BonusCritChance = Talents.GlyphOfExplosiveShot ? 0.04f : 0f;
            //
            Initialize();
        }
    }
    public class ChimeraShot : Ability
    {
        /// <summary>
        /// <b>Chimera Shot</b>, 12% Base Mana, 5-35yd, Instant, 10 sec Cd
        /// <para>You deal 125% weapon damage, refreshing the current Sting on your
        /// target and triggering an effect:</para>
        /// <para>Serpent Sting - Instantly deals 40% of the Damage done by your Serpent Sting.</para>
        /// <para>Viper Sting - Instatly restores mana to you equal to 60% of the total amount drained by your Viper Sting.</para>
        /// <para>Scorpid Sting - Attempts to Disarm the target for 10 sec. This affect cannot occur more than once per 1 minute.</para>
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
            ManaCostisPerc = true;
            ManaCost = 0.12f; // 12% of base mana
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = combatFactors.AvgRwWeaponDmgUnhasted * 1.25f;
            //
            Initialize();
        }
    }
    public class SteadyShot : Ability {
        /// <summary>
        /// <b>Steady Shot</b>, 5% Base Mana, 5-35yd, 1.5 sec cast, 10 sec Cd
        /// <para>A steady shot that causes unmodified weapon damage, plus ammo,
        /// plus [RAP * 0.1 + 252]. Causes an additional 175 against Dazed targets.</para>
        /// </summary>
        /// <TalentsAffecting>
        /// Improved Steady Shot [+3*5% chance to increase damage of next Aimed/Arcane/Chim
        /// shot by 15% and reduc Mana cost of next Aimed/Arcane/Chim shot by 20%]
        /// </TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Steady Shot [+10% DMG w/ Serpent Sting active]</GlyphsAffecting>
        public SteadyShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Steady Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            ManaCostisPerc = true;
            ManaCost = 0.05f; // 5% of base mana
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = combatFactors.AvgRwWeaponDmgUnhasted
                       + (Char.Projectile.Item.MinDamage + Char.Projectile.Item.MaxDamage) / 2f
                       + StatS.RangedAttackPower * 0.1f + 252f;
            DamageBonus = 1f + (true ? 0.10f : 0f); // this should be a check for Serpent Sting on the target
            //
            Initialize();
        }
    }
    public class AimedShot : Ability {
        /// <summary>
        /// <b>Aimed Shot</b>, 8% Base Mana, 5-35yd, Instant, 10 sec Cd
        /// <para>An Aimed show that increases ranged damage by 408 and reduces
        /// healing done to that target by 50%. Lasts 10 sec.</para>
        /// </summary>
        /// <TalentsAffecting>Aimed Shot (Requires Talent)</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Aimed Shot [-2 sec Cd]</GlyphsAffecting>
        public AimedShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Aimed Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
            Talent2ChksValue = Talents.AimedShot;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            Cd = 10f - (Talents.GlyphOfAimedShot ? 2f : 0f); // In Seconds
            Duration = 10f;
            ManaCostisPerc = true;
            ManaCost = 0.08f; // 8% of base mana
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = combatFactors.NormalizedRwWeaponDmg + 408;
            //
            Initialize();
        }
    }
    public class MultiShot : Ability {
        /// <summary>
        /// <b>Multi-Shot</b>, 9% Base Mana, 5-35yd, Cast Time: Attack Speed, 10 sec Cd
        /// <para>Fires several missiles, hitting 3 targets for an additioal 408 damage.</para>
        /// </summary>
        /// <TalentsAffecting></TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Multi-Shot [-1 sec Cd]</GlyphsAffecting>
        public MultiShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Multi-Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            Targets = 3f;
            Cd = 10f - (Talents.GlyphOfMultiShot ? 1f : 0f); // In Seconds
            CastTime = cf.RWSpeed;
            ManaCostisPerc = true;
            ManaCost = 0.09f; // 9% of base mana
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = cf.AvgRwWeaponDmgUnhasted + 408f;
            //
            Initialize();
        }
    }
    public class ArcaneShot : Ability {
        /// <summary>
        /// <b>Arcane Shot</b>, 5% Base Mana, 5-35yd, Instant, 6 sec Cd
        /// <para>An Instant shot that causes [RAP * 0.15 + 492] Arcane Damage</para>
        /// </summary>
        /// <TalentsAffecting>Aimed Shot (Requires Talent)</TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Arcane Shot [20% ManaCost refunded if a sting is active]</GlyphsAffecting>
        public ArcaneShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Arcane Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
            Talent2ChksValue = Talents.AimedShot;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            Cd = 6f; // In Seconds
            Duration = 10f;
            ManaCostisPerc = true;
            ManaCost = 0.05f; // 5% of base mana
            // -20% with a sting active
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = StatS.RangedAttackPower * 0.15f + 492f;
            DamageBonus = 1f + Talents.ImprovedArcaneShot * 0.05f;
            //
            Initialize();
        }
    }
    public class KillShot : Ability {
        /// <summary>
        /// <b>Kill Shot</b>, 7% Base Mana, 45yd, Instant, 15 sec Cd
        /// <para>You attempt to finish the wounded target off, firing a long
        /// range attack dealing 200% weapon damage plus [RAP * 0.40 + 650].
        /// Kill Shot can only be used on enemies that have 20% or less health.</para>
        /// </summary>
        /// <TalentsAffecting></TalentsAffecting>
        /// <GlyphsAffecting>Glyph of Kill Shot [-6 sec Cd]</GlyphsAffecting>
        public KillShot(Character c, Stats s, CombatFactors cf, WhiteAttacks wa, CalculationOptionsHunter co)
        {
            Char = c; StatS = s; combatFactors = cf; Whiteattacks = wa; CalcOpts = co;
            //
            Name = "Kill Shot";
            //AbilIterater = (int)CalculationOptionsHunter.Maintenances.MortalStrike_;
            ReqTalent = true;
            Talent2ChksValue = Talents.AimedShot;
            ReqRangedWeap = true;
            ReqSkillsRange = true;
            //Targets += StatS.BonusTargets;
            Cd = 15f - (Talents.GlyphOfKillShot ? 6f : 0f); // In Seconds
            ManaCostisPerc = true;
            ManaCost = 0.07f; // 7% of base mana
            StanceOkFury = StanceOkArms = StanceOkDef = true;
            DamageBase = cf.AvgRwWeaponDmgUnhasted + StatS.RangedAttackPower * 0.40f + 650f;
            //
            Initialize();
        }
    }

    public class BlackArrow : Ability { }
    public class SilencingShot : Ability { }
    public class ScorpidSting : Ability { }
    public class ViperSting : Ability { }
    public class RapidFire : Ability { }
    public class ImmolationTrap : Ability { }
    public class Readiness : Ability { }
    public class BeastialWrath : Ability { }
    public class BloodFury : Ability { }
    public class Berserk : Ability { }

    public class ChimeraShot_Serpent : Ability { }
    public class SerpentSting : Ability { }
}
