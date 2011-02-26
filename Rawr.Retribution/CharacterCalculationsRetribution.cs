using System;
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

        public RotationSolution Solution { get; set; } // TODO: Remove dependancy, This should be obtained out of the base Rotation class, not the Sim specific solution.
        public Ability[] Rotation { get; set; }

        public float WhiteDPS { get; set; }
        public float SealDPS { get; set; }
        public float CrusaderStrikeDPS { get; set; }
        public float TemplarsVerdictDPS { get; set; }
        public float CommandDPS { get; set; }
        public float JudgementDPS { get; set; }
        public float ConsecrationDPS { get; set; }
        public float ExorcismDPS { get; set; }
        public float HolyWrathDPS { get; set; }
        public float HammerOfWrathDPS { get; set; }
        public float OtherDPS { get; set; }

        public Skill WhiteSkill { get; set; }
        public Skill SealSkill { get; set; }
        public Skill CrusaderStrikeSkill { get; set; }
        public Skill TemplarsVerdictSkill { get; set; }
        public Skill CommandSkill { get; set; }
        public Skill JudgementSkill { get; set; }
        public Skill ConsecrationSkill { get; set; }
        public Skill ExorcismSkill { get; set; }
        public Skill HolyWrathSkill { get; set; }
        public Skill HammerOfWrathSkill { get; set; }
        
        public float AverageSoVStack { get; set; }
        
        public Stats BasicStats { get; set; }
        public CombatStats Combatstats { get; set; }

        // Add calculated values to the values dictionary.
        // These values are then available for display via the CharacterDisplayCalculationLabels
        // member defined in CalculationsRetribution.cs
        // While possible, there's little reason to add values to the dictionary that are not being
        // used by the CharacterDisplayCalculationLabels.
        public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
        {
            Dictionary<string, string> dictValues = new Dictionary<string, string>();
            // Basic stats
            dictValues["Health"] = string.Format("{0:N0}*Base Health: {1:N0}", Combatstats.Stats.Health, BasicStats.Health);
            dictValues["Mana"] = string.Format("{0:N0}*Base Mana: {1:N0}", Combatstats.Stats.Mana, BasicStats.Mana);
            dictValues["Strength"] = string.Format("{0:N0}*Base Strength: {1:N0}", Combatstats.Stats.Strength, BasicStats.Strength);
            dictValues["Agility"] = string.Format("{0:N0}*Base Agility: {1:N0}", Combatstats.Stats.Agility, BasicStats.Agility);
            dictValues["Attack Power"] = string.Format("{0:N0}*Base Attack Power: {1:N0}", Combatstats.Stats.AttackPower, BasicStats.AttackPower);
            dictValues["Crit Chance"] = string.Format("{0:P}*{1:0} Crit Rating ({2:P})", Combatstats.Stats.PhysicalCrit, BasicStats.CritRating, StatConversion.GetCritFromRating(BasicStats.CritRating, CharacterClass.Paladin));
            dictValues["Miss Chance"] = string.Format("{0:P}*{1:0} Hit Rating ({2:P})", Combatstats.GetMeleeMissChance(), BasicStats.HitRating, StatConversion.GetHitFromRating(BasicStats.HitRating, CharacterClass.Paladin));
            dictValues["Dodge Chance"] = string.Format("{0:P}*{1:0} Expertise Rating ({2:F1})", Combatstats.GetToBeDodgedChance(), BasicStats.ExpertiseRating, BasicStats.Expertise);
            dictValues["Mastery"] = string.Format("{0:F2}*{1:0} Mastery Rating ({2:F1})\n{3:P} Hand of Light", (8f + StatConversion.GetMasteryFromRating(Combatstats.Stats.MasteryRating, CharacterClass.Paladin)), BasicStats.MasteryRating, (8f + StatConversion.GetMasteryFromRating(BasicStats.MasteryRating, CharacterClass.Paladin)), Combatstats.GetMasteryTotalPercent());
            dictValues["Melee Haste"] = string.Format("{0:P}*{1:0} Haste Rating ({2:P})", Combatstats.Stats.PhysicalHaste, BasicStats.HasteRating, StatConversion.GetHasteFromRating(BasicStats.HasteRating, CharacterClass.Paladin));
            dictValues["Weapon Damage"] = string.Format("{0:F}*Base Weapon Damage: {1:F}", Combatstats.WeaponDamage.ToString("N2"), Combatstats.GetWeaponDamage(BasicStats.AttackPower));
            dictValues["Attack Speed"] = string.Format("{0:F2}*Base Attack Speed: {1:F2}", Combatstats.AttackSpeed.ToString("N2"), Combatstats.GetAttackSpeed(BasicStats.PhysicalHaste));

            // DPS Breakdown
            dictValues["Total DPS"] = OverallPoints.ToString("N0");
            dictValues["White"] = string.Format("{0}*{1}", WhiteDPS.ToString("N0"), WhiteSkill.ToString());
            dictValues["Seal"] = string.Format("{0}*{1}", SealDPS.ToString("N0"), SealSkill.ToString());
            dictValues["Seal of Command"] = string.Format("{0}*{1}", CommandDPS.ToString("N0"), CommandSkill.ToString());
            dictValues["Crusader Strike"] = string.Format("{0}*{1}", CrusaderStrikeDPS.ToString("N0"), CrusaderStrikeSkill.ToString());
            dictValues["Templars Verdict"] = string.Format("{0}*{1}", TemplarsVerdictDPS.ToString("N0"), TemplarsVerdictSkill.ToString());
            dictValues["Judgement"] = string.Format("{0}*{1}", JudgementDPS.ToString("N0"), JudgementSkill.ToString());
            dictValues["Consecration"] = string.Format("{0}*{1}", ConsecrationDPS.ToString("N0"), ConsecrationSkill.ToString());
            dictValues["Exorcism"] = string.Format("{0}*{1}", ExorcismDPS.ToString("N0"), ExorcismSkill.ToString());
            dictValues["Holy Wrath"] = string.Format("{0}*{1}", HolyWrathDPS.ToString("N0"), HolyWrathSkill.ToString());
            dictValues["Hammer of Wrath"] = string.Format("{0}*{1}", HammerOfWrathDPS.ToString("N0"), HammerOfWrathSkill.ToString());
            dictValues["Other"] = OtherDPS.ToString("N0");

            // Rotation Info:
            dictValues["Chosen Rotation"] = Rotation == null ? 
                "n/a" :
                SimulatorParameters.ShortRotationString(Rotation);  // TODO: Remove dependancy on SimulatorParameters.
            dictValues["Average SoV Stack"] = AverageSoVStack.ToString("N2");
            dictValues["Crusader Strike CD"] = Solution.GetAbilityEffectiveCooldown(Ability.CrusaderStrike).ToString("N2");
            dictValues["Judgement CD"] = Solution.GetAbilityEffectiveCooldown(Ability.Judgement).ToString("N2");
            dictValues["Consecration CD"] = Solution.GetAbilityEffectiveCooldown(Ability.Consecration).ToString("N2");
            dictValues["Exorcism CD"] = Solution.GetAbilityEffectiveCooldown(Ability.Exorcism).ToString("N2");
            dictValues["Hammer of Wrath CD"] = Solution.GetAbilityEffectiveCooldown(Ability.HammerOfWrath).ToString("N2");

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
                case "% Chance to Miss (Melee)": return Combatstats.GetMeleeMissChance() * 100f;  // White and Melee hit for ret are identical since we can't dual wield.
                case "% Chance to Miss (Spells)": return Combatstats.GetSpellMissChance() * 100f;
                case "% Chance to be Dodged": return Combatstats.GetToBeDodgedChance() * 100f;
                case "% Chance to be Parried": return Combatstats.GetToBeParriedChance() * 100f;
                case "% Chance to be Avoided (Yellow/Dodge)": return (Combatstats.GetMeleeMissChance() + Combatstats.GetToBeDodgedChance()) * 100f;
            }
            return 0f;
        }
    }
}