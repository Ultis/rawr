using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Retribution
{
    public class CharacterCalculationsRetribution : CharacterCalculationsBase
    {
        private float _overallPoints = 0f;
        public override float OverallPoints { get { return _overallPoints; } set { _overallPoints = value; } }
        private float[] _subPoints = new float[] { 0f };
        public override float[] SubPoints { get { return _subPoints; } set { _subPoints = value; } }
        public float DPSPoints { get { return _subPoints[0]; } set { _subPoints[0] = value; } }
        
        public float OtherDPS { get; set; }

        public Skill WhiteSkill { get; set; }
        public Skill SealSkill { get; set; }
        public Skill SealDotSkill { get; set; }
        public Skill CrusaderStrikeSkill { get; set; }
        public Skill TemplarsVerdictSkill { get; set; }
        public Skill CommandSkill { get; set; }
        public Skill JudgementSkill { get; set; }
        public Skill ConsecrationSkill { get; set; }
        public Skill ExorcismSkill { get; set; }
        public Skill HolyWrathSkill { get; set; }
        public Skill HammerOfWrathSkill { get; set; }
        
        public Stats BasicStats { get; set; }
        public Stats CombatStats { get; set; }
        public Character Character { get; set; }

        // Add calculated values to the values dictionary.
        // These values are then available for display via the CharacterDisplayCalculationLabels
        // member defined in CalculationsRetribution.cs
        // While possible, there's little reason to add values to the dictionary that are not being
        // used by the CharacterDisplayCalculationLabels.
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            // Basic stats
            dictValues["Health"] = string.Format("{0:N0}*Base Health: {1:N0}", CombatStats.Health, BasicStats.Health);
            dictValues["Mana"] = string.Format("{0:N0}*Base Mana: {1:N0}", CombatStats.Mana, BasicStats.Mana);
            dictValues["Strength"] = string.Format("{0:N0}*Base Strength: {1:N0}", CombatStats.Strength, BasicStats.Strength);
            dictValues["Agility"] = string.Format("{0:N0}*Base Agility: {1:N0}", CombatStats.Agility, BasicStats.Agility);
            dictValues["Attack Power"] = string.Format("{0:N0}*Base Attack Power: {1:N0}", CombatStats.AttackPower, BasicStats.AttackPower);
            dictValues["Melee Crit"] = string.Format("{0:P}*{1:0} Crit Rating ({2:P})", CombatStats.PhysicalCrit, BasicStats.CritRating, StatConversion.GetCritFromRating(BasicStats.CritRating, CharacterClass.Paladin));
            dictValues["Melee Haste"] = string.Format("{0:P}*{1:0} Haste Rating ({2:P})", CombatStats.PhysicalHaste, BasicStats.HasteRating, StatConversion.GetHasteFromRating(BasicStats.HasteRating, CharacterClass.Paladin));
            dictValues["Chance to Dodge"] = string.Format("{0:P}*{1:0} Expertise Rating ({2:F1})", ((BasePhysicalWhiteCombatTable)WhiteSkill.CT).ChanceToDodge, BasicStats.ExpertiseRating, BasicStats.Expertise);
            dictValues["Mastery"] = string.Format("{0:F2}*{1:0} Mastery Rating ({2:F1})\n{3:P} Hand of Light", (8f + StatConversion.GetMasteryFromRating(CombatStats.MasteryRating, CharacterClass.Paladin)),
                                                                                                               BasicStats.MasteryRating, StatConversion.GetMasteryFromRating(BasicStats.MasteryRating, CharacterClass.Paladin), 
                                                                                                               (8f + StatConversion.GetMasteryFromRating(BasicStats.MasteryRating, CharacterClass.Paladin)) * PaladinConstants.HOL_COEFF);
            dictValues["Miss Chance"] = string.Format("{0:P}*{1:0} Hit Rating ({2:P})", WhiteSkill.CT.ChanceToMiss, BasicStats.HitRating, StatConversion.GetHitFromRating(BasicStats.HitRating, CharacterClass.Paladin));
            dictValues["Spell Power"] = string.Format("{0:N0}*Base Spell Power: {1:N0}", CombatStats.SpellPower, BasicStats.SpellPower);
            dictValues["Spell Crit"] = string.Format("{0:P}*{1:0} Crit Rating ({2:P})", CombatStats.SpellCrit, BasicStats.CritRating, StatConversion.GetCritFromRating(BasicStats.CritRating, CharacterClass.Paladin));
            dictValues["Spell Haste"] = string.Format("{0:P}*{1:0} Haste Rating ({2:P})", CombatStats.SpellHaste, BasicStats.HasteRating, StatConversion.GetHasteFromRating(BasicStats.HasteRating, CharacterClass.Paladin));
            dictValues["Weapon Damage"] = string.Format("{0:F}*Base Weapon Damage: {1:F}", AbilityHelper.WeaponDamage(Character, CombatStats.AttackPower), AbilityHelper.WeaponDamage(Character, BasicStats.AttackPower));
            dictValues["Weapon Damage @3.3"] = string.Format("{0:F}*Base Weapon Damage: {1:F}", AbilityHelper.WeaponDamage(Character, CombatStats.AttackPower, true), AbilityHelper.WeaponDamage(Character, BasicStats.AttackPower, true));
            dictValues["Attack Speed"] = string.Format("{0:F2}*Base Attack Speed: {1:F2}", AbilityHelper.WeaponSpeed(Character, CombatStats.PhysicalHaste), AbilityHelper.WeaponSpeed(Character, BasicStats.PhysicalHaste));

            // DPS Breakdown
            dictValues["Total DPS"] = OverallPoints.ToString("N0");
            dictValues["White"] = string.Format("{0:N0}*" + WhiteSkill.ToString(), WhiteSkill.GetDPS());
            dictValues["Seal"] = string.Format("{0:N0}*" + SealSkill.ToString(), SealSkill.GetDPS());
            dictValues["Seal (Dot)"] = string.Format("{0:N0}*" + SealDotSkill.ToString(), SealDotSkill.GetDPS());
            dictValues["Seal of Command"] = string.Format("{0:N0}*" + CommandSkill.ToString(), CommandSkill.GetDPS());
            dictValues["Crusader Strike"] = string.Format("{0:N0}*" + CrusaderStrikeSkill.ToString(), CrusaderStrikeSkill.GetDPS());
            dictValues["Templars Verdict"] = string.Format("{0:N0}*" + TemplarsVerdictSkill.ToString(), TemplarsVerdictSkill.GetDPS());
            dictValues["Judgement"] = string.Format("{0:N0}*" + JudgementSkill.ToString(), JudgementSkill.GetDPS());
            dictValues["Consecration"] = string.Format("{0:N0}*" + ConsecrationSkill.ToString(), ConsecrationSkill.GetDPS());
            dictValues["Exorcism"] = string.Format("{0:N0}*" + ExorcismSkill.ToString(), ExorcismSkill.GetDPS());
            dictValues["Holy Wrath"] = string.Format("{0:N0}*" + HolyWrathSkill.ToString(), HolyWrathSkill.GetDPS());
            dictValues["Hammer of Wrath"] = string.Format("{0:N0}*" + HammerOfWrathSkill.ToString(), HammerOfWrathSkill.GetDPS());
            dictValues["Other"] = OtherDPS.ToString("N0");

            // Rotation Info:
            dictValues["Inqusition Uptime"] = CrusaderStrikeSkill.InqUptime.ToString("P2");
            dictValues["Crusader Strike Usage"] = (CrusaderStrikeSkill.UsagePerSec * Character.BossOptions.BerserkTimer).ToString("F2");
            dictValues["Templar's Verdict Usage"] = (TemplarsVerdictSkill.UsagePerSec * Character.BossOptions.BerserkTimer).ToString("F2");
            dictValues["Exorcism Usage"] = (ExorcismSkill.UsagePerSec * Character.BossOptions.BerserkTimer).ToString("F2");
            dictValues["Hammer of Wrath Usage"] = (HammerOfWrathSkill.UsagePerSec * Character.BossOptions.BerserkTimer).ToString("F2");
            dictValues["Judgement Usage"] = (JudgementSkill.UsagePerSec * Character.BossOptions.BerserkTimer).ToString("F2");
            dictValues["Holy Wrath Usage"] = (HolyWrathSkill.UsagePerSec * Character.BossOptions.BerserkTimer).ToString("F2");
            dictValues["Consecration Usage"] = (ConsecrationSkill.UsagePerSec * Character.BossOptions.BerserkTimer).ToString("F2");

            return dictValues;
        }

        /// <summary>
        /// Obtain optimizable values.
        /// </summary>
        /// <param name="calculation"></param>
        /// <returns></returns>
        /// The list of labels listed here needs to match with the list in OptimizableCalculationLabels override in CalculationsRetribution.cs
        public override float GetOptimizableCalculationValue(string calculation)
        {
            switch (calculation)
            {
                case "Health": return BasicStats.Health;
                case "% Chance to Miss (Melee)": return WhiteSkill.CT.ChanceToMiss * 100f;  // White and Melee hit for ret are identical since we can't dual wield.
                case "% Chance to Miss (Spells)": return ExorcismSkill.CT.ChanceToMiss * 100f;
                case "% Chance to be Dodged": return ((BasePhysicalWhiteCombatTable)WhiteSkill.CT).ChanceToDodge * 100f;
                case "% Chance to be Parried": return ((BasePhysicalWhiteCombatTable)WhiteSkill.CT).ChanceToParry * 100f;
                case "% Chance to be Avoided (Yellow/Dodge)": return (1f - ((BasePhysicalYellowCombatTable)CrusaderStrikeSkill.CT).ChanceToLand) * 100f;
            }
            return 0f;
        }
    }
}