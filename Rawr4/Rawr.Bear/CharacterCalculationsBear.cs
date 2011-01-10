using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Text;

namespace Rawr.Bear
{
	/// <summary>
	/// Data container class for the results of calculations about a Character
	/// </summary>
	public class CharacterCalculationsBear : CharacterCalculationsBase
	{
		private float _overallPoints = 0f;
		public override float OverallPoints
		{
			get { return _overallPoints; }
			set { _overallPoints = value; }
		}

		private float[] _subPoints = new float[] { 0f, 0f, 0f };
		public override float[] SubPoints
		{
			get { return _subPoints; }
			set { _subPoints = value; }
		}

		public float MitigationPoints
		{
			get { return _subPoints[0]; }
			set { _subPoints[0] = value; }
		}

		public float SurvivalPoints
		{
			get { return _subPoints[1]; }
			set { _subPoints[1] = value; }
		}

		public float ThreatPoints
		{
			get { return _subPoints[2]; }
			set { _subPoints[2] = value; }
		}

		public StatsBear BasicStats { get; set; }
		public int TargetLevel { get; set; }
		public int CharacterLevel { get; set; }
		public float Dodge { get; set; }
		public float Miss { get; set; }
		public float DamageReductionFromArmor { get; set; }
		public float TotalConstantDamageReduction { get; set; }
		public float AvoidancePreDR { get; set; }
		public float AvoidancePostDR { get; set; }
		public float TotalMitigation { get; set; }
		public float SavageDefenseChance { get; set; }
		public float SavageDefenseValue { get; set; }
		public float SavageDefensePercent { get; set; }
		public float AverageVengeanceAP { get; set; }
		public float DamageTaken { get; set; }
		public float CritReduction { get; set; }
		public float CappedCritReduction { get; set; }
		public float SurvivalPointsRaw { get; set; }

		public float NatureSurvivalPoints { get; set; }
		public float FrostSurvivalPoints { get; set; }
		public float FireSurvivalPoints { get; set; }
		public float ShadowSurvivalPoints { get; set; }
		public float ArcaneSurvivalPoints { get; set; }

		public float AttackSpeed { get; set; }
		public float MangleCooldown { get; set; }

		public float MissedAttacks { get; set; }
		public float DodgedAttacks { get; set; }
		public float ParriedAttacks { get; set; }
		public float AvoidedAttacks { get { return MissedAttacks + DodgedAttacks + ParriedAttacks; } }

		public BearAbilityBuilder Abilities { get; set; }

		public BearRotationCalculator.BearRotationCalculation HighestDPSRotation { get; set; }
		public BearRotationCalculator.BearRotationCalculation HighestTPSRotation { get; set; }
		public BearRotationCalculator.BearRotationCalculation SwipeRotation { get; set; }
		public BearRotationCalculator.BearRotationCalculation CustomRotation { get; set; }

		public override Dictionary<string, string> GetCharacterDisplayCalculationValues()
		{
			Dictionary<string, string> dictValues = new Dictionary<string, string>();

			int levelDifference = TargetLevel - CharacterLevel;
			float baseMiss = StatConversion.WHITE_MISS_CHANCE_CAP[levelDifference] - BasicStats.PhysicalHit;
			float baseDodge = StatConversion.WHITE_DODGE_CHANCE_CAP[levelDifference] - StatConversion.GetDodgeParryReducFromExpertise(BasicStats.Expertise);
			float baseParry = StatConversion.WHITE_PARRY_CHANCE_CAP[levelDifference] - StatConversion.GetDodgeParryReducFromExpertise(BasicStats.Expertise);
			float capMiss = (float)Math.Ceiling(baseMiss * StatConversion.RATING_PER_PHYSICALHIT);
			float capDodge = (float)Math.Ceiling(baseDodge * 400f * StatConversion.RATING_PER_EXPERTISE);
			float capParry = (float)Math.Ceiling(baseParry * 400f * StatConversion.RATING_PER_EXPERTISE);

			string tipMiss = string.Empty;
			if (BasicStats.HitRating > capMiss)
				tipMiss = string.Format("*Over the cap by {0} Hit Rating", BasicStats.HitRating - capMiss);
			else if (BasicStats.HitRating < capMiss)
				tipMiss = string.Format("*Under the cap by {0} Hit Rating", capMiss - BasicStats.HitRating);
			else
				tipMiss = "*Exactly at the cap";

			string tipDodgeParry = string.Empty;
			if (BasicStats.ExpertiseRating > capDodge)
				tipDodgeParry = string.Format("*Over the dodge cap by {0} Expertise Rating\r\n", BasicStats.ExpertiseRating - capDodge);
			else if (BasicStats.ExpertiseRating < capDodge)
				tipDodgeParry = string.Format("*Under the dodge cap by {0} Expertise Rating\r\n", capDodge - BasicStats.ExpertiseRating);
			else
				tipDodgeParry = "*Exactly at the dodge cap";

			if (BasicStats.ExpertiseRating > capParry)
				tipDodgeParry += string.Format("Over the parry cap by {0} Expertise Rating", BasicStats.ExpertiseRating - capParry);
			else if (BasicStats.ExpertiseRating < capParry)
				tipDodgeParry += string.Format("Under the parry cap by {0} Expertise Rating", capParry - BasicStats.ExpertiseRating);
			else
				tipDodgeParry += "Exactly at the parry cap";




			int armorCap = (int)Math.Ceiling(6502.5f * TargetLevel - 474502.5f);
			float levelDifferenceAvoidance = 0.002f * levelDifference;
			float targetCritReduction = StatConversion.NPC_LEVEL_CRIT_MOD[levelDifference];
			/*int defToCap = 0, resToCap = 0;
			if (CritReduction < targetCritReduction)
			{
				//while (((float)Math.Floor((BasicStats.DefenseRating + defToCap) / (123f / 52f)) * 0.04f)
				//+ BasicStats.Resilience / (2050f / 52f) + BasicStats.CritChanceReduction < targetCritReduction)
				//    defToCap++;
				//while (((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04f)
				//+ (BasicStats.Resilience + resToCap) / (2050f / 52f) + BasicStats.CritChanceReduction < targetCritReduction)
				//    resToCap++;
				while (((float)Math.Floor(StatConversion.GetDefenseFromRating(BasicStats.DefenseRating + defToCap)) * 0.0004f)
				                            + StatConversion.GetCritReductionFromResilience(BasicStats.Resilience)
                                            + BasicStats.CritChanceReduction < targetCritReduction)
					defToCap++;
				while (((float)Math.Floor(StatConversion.GetDefenseFromRating(BasicStats.DefenseRating)) * 0.0004f)
				                            + StatConversion.GetCritReductionFromResilience(BasicStats.Resilience + resToCap)
                                            + BasicStats.CritChanceReduction < targetCritReduction)
					resToCap++;
			}
			else if (CritReduction > targetCritReduction)
			{
				//while (((float)Math.Floor((BasicStats.DefenseRating + defToCap) / (123f / 52f)) * 0.04f)
				//+ BasicStats.Resilience / (2050f / 52f) + BasicStats.CritChanceReduction > targetCritReduction)
				//    defToCap--;
				//while (((float)Math.Floor(BasicStats.DefenseRating / (123f / 52f)) * 0.04f)
				//+ (BasicStats.Resilience + resToCap) / (2050f / 52f) + BasicStats.CritChanceReduction > targetCritReduction)
				//    resToCap--;

				while (((float)Math.Floor(StatConversion.GetDefenseFromRating(BasicStats.DefenseRating + defToCap)) * 0.0004f)
				+ StatConversion.GetCritReductionFromResilience(BasicStats.Resilience) + BasicStats.CritChanceReduction > targetCritReduction)
					defToCap--;
				while (((float)Math.Floor(StatConversion.GetDefenseFromRating(BasicStats.DefenseRating)) * 0.0004f)
				+ StatConversion.GetCritReductionFromResilience(BasicStats.Resilience + resToCap) + BasicStats.CritChanceReduction > targetCritReduction)
					resToCap--;
				defToCap++;
				resToCap++;
			}*/

			// Changed to not just give a resist rating, but a breakdown of the resulting resist values in the tooltip
			string tipResist = string.Empty;
			tipResist = StatConversion.GetResistanceTableString(TargetLevel, CharacterLevel, BasicStats.NatureResistance, 0);
			dictValues.Add("Nature Resist", BasicStats.NatureResistance.ToString() + "*" + tipResist);
			tipResist = StatConversion.GetResistanceTableString(TargetLevel, CharacterLevel, BasicStats.ArcaneResistance, 0);
			dictValues.Add("Arcane Resist", BasicStats.ArcaneResistance.ToString() + "*" + tipResist);
			tipResist = StatConversion.GetResistanceTableString(TargetLevel, CharacterLevel, BasicStats.FrostResistance, 0);
			dictValues.Add("Frost Resist", BasicStats.FrostResistance.ToString() + "*" + tipResist);
			tipResist = StatConversion.GetResistanceTableString(TargetLevel, CharacterLevel, BasicStats.FireResistance, 0);
			dictValues.Add("Fire Resist", BasicStats.FireResistance.ToString() + "*" + tipResist);
			tipResist = StatConversion.GetResistanceTableString(TargetLevel, CharacterLevel, BasicStats.ShadowResistance, 0);
			dictValues.Add("Shadow Resist", BasicStats.ShadowResistance.ToString() + "*" + tipResist);

			dictValues.Add("Health", BasicStats.Health.ToString());
			dictValues.Add("Agility", BasicStats.Agility.ToString());
			dictValues.Add("Armor", BasicStats.Armor.ToString());
			dictValues.Add("Stamina", BasicStats.Stamina.ToString());
			dictValues.Add("Dodge Rating", BasicStats.DodgeRating.ToString());
			dictValues.Add("Mastery", string.Format("{0}*{1} Mastery Rating", 
				StatConversion.GetMasteryFromRating(BasicStats.MasteryRating) + 8f,
				BasicStats.MasteryRating.ToString()));
			dictValues.Add("Resilience", BasicStats.Resilience.ToString());
			dictValues.Add("Dodge", Dodge.ToString("0.000%"));
			dictValues.Add("Miss", Miss.ToString("0.000%"));
			if (BasicStats.Armor == armorCap)
				dictValues.Add("Armor Damage Reduction", DamageReductionFromArmor.ToString("0.000%")
					+ string.Format("*Exactly at the armor cap against level {0} mobs.", TargetLevel));
			else if (BasicStats.Armor > armorCap)
				dictValues.Add("Armor Damage Reduction", DamageReductionFromArmor.ToString("0.000%")
					+ string.Format("*Over the armor cap by {0} armor.", BasicStats.Armor - armorCap));
			else
				dictValues.Add("Armor Damage Reduction", DamageReductionFromArmor.ToString("0.000%")
					+ string.Format("*Short of the armor cap by {0} armor.", armorCap - BasicStats.Armor));
			dictValues.Add("Total Damage Reduction", TotalConstantDamageReduction.ToString("0.000%"));
			dictValues.Add("Avoidance PreDR", AvoidancePreDR.ToString("0.000%"));
			dictValues.Add("Avoidance PostDR", AvoidancePostDR.ToString("0.000%"));
			dictValues.Add("Total Mitigation", TotalMitigation.ToString("0.000%"));
			dictValues.Add("Damage Taken", DamageTaken.ToString("0.000%"));
			dictValues.Add("Savage Defense", string.Format(
				"{0} ~ {1}*{0} chance to absorb incoming hit\r\n{1} absorbed per hit\r\n{2} of incoming damage absorbed",
				SavageDefenseChance.ToString("0.000%"), SavageDefenseValue, SavageDefensePercent.ToString("0.000%")));
			dictValues.Add("Chance to be Crit", ((0.05f + levelDifferenceAvoidance) - CritReduction).ToString("0.000%"));
			dictValues.Add("Overall Points", OverallPoints.ToString());
			dictValues.Add("Mitigation Points", MitigationPoints.ToString());
			dictValues.Add("Survival Points", string.Format("{0}*{1} Before Soft Cap", SurvivalPoints.ToString(), SurvivalPointsRaw.ToString()));
			dictValues.Add("Threat Points", ThreatPoints.ToString());

			dictValues["Nature Survival"] = NatureSurvivalPoints.ToString();
			dictValues["Frost Survival"] = FrostSurvivalPoints.ToString();
			dictValues["Fire Survival"] = FireSurvivalPoints.ToString();
			dictValues["Shadow Survival"] = ShadowSurvivalPoints.ToString();
			dictValues["Arcane Survival"] = ArcaneSurvivalPoints.ToString();


			dictValues["Strength"] = BasicStats.Strength.ToString();
			dictValues["Attack Power"] = string.Format("{0}*{1} with Vengeance", (BasicStats.AttackPower - AverageVengeanceAP), BasicStats.AttackPower);
			dictValues["Average Vengeance AP"] = AverageVengeanceAP.ToString("N1");
			dictValues["Crit Rating"] = BasicStats.CritRating.ToString();
			dictValues["Hit Rating"] = BasicStats.HitRating.ToString() + tipMiss;
			dictValues["Expertise Rating"] = BasicStats.ExpertiseRating.ToString() + tipDodgeParry;
			dictValues["Haste Rating"] = string.Format("{0}*{1}sec Attack Speed", BasicStats.HasteRating, AttackSpeed.ToString("0.000"));
			//dictValues["Armor Penetration Rating"] = BasicStats.ArmorPenetrationRating.ToString();

			dictValues["Avoided Attacks"] = String.Format("{0}*{1} Missed\r\n{2} Dodged\r\n{3} Parried",
				AvoidedAttacks.ToString("0.000%"), MissedAttacks.ToString("0.000%"),
				DodgedAttacks.ToString("0.000%"), ParriedAttacks.ToString("0.000%"));

			dictValues["Highest DPS Rotation"] = HighestDPSRotation.Name;
			dictValues["Highest TPS Rotation"] = HighestTPSRotation.Name;
			dictValues["Swipe Rotation"] = "";
			dictValues["Custom Rotation"] = "";
			//string rotationFormat = "{0} DPS, {1} TPS*{2}";
			//dictValues["Highest DPS Rotation"] = String.Format(rotationFormat, Math.Round(HighestDPSRotation.DPS), Math.Round(HighestDPSRotation.TPS), GetRotationTooltip(HighestDPSRotation.Name));
			//dictValues["Highest TPS Rotation"] = String.Format(rotationFormat, Math.Round(HighestTPSRotation.DPS), Math.Round(HighestTPSRotation.TPS), GetRotationTooltip(HighestTPSRotation.Name));
			//dictValues["Swipe Rotation"] = String.Format(rotationFormat, Math.Round(SwipeRotation.DPS), Math.Round(SwipeRotation.TPS), GetRotationTooltip(SwipeRotation.Name));
			//dictValues["Custom Rotation"] = String.Format(rotationFormat, Math.Round(CustomRotation.DPS), Math.Round(CustomRotation.TPS), GetRotationTooltip(CustomRotation.Name));

			dictValues["Melee"] = Abilities.MeleeStats.ToString();
			dictValues["Maul"] = Abilities.MaulStats.ToString();
			dictValues["Mangle"] = Abilities.MangleStats.ToString();
			dictValues["Lacerate"] = Abilities.LacerateStats.ToString();
			dictValues["Pulverize"] = Abilities.PulverizeStats.ToString();
			dictValues["Swipe"] = Abilities.SwipeStats.ToString();
			dictValues["Thrash"] = Abilities.ThrashStats.ToString();
			dictValues["Faerie Fire"] = Abilities.FaerieFireStats.ToString();
			dictValues["Thorns"] = Abilities.ThornsStats.ToString();
			//string attackFormat = "{0} Dmg, {1} Threat*Per Hit: {0} Damage, {1} Threat\r\nPer Average Swing: {2} Damage, {3} Threat";
			//string attackFormatWithRage = attackFormat + "\r\nThreat Per Rage: {4}\r\nDamage Per Rage: {5}";
			//dictValues["Melee"] = String.Format(attackFormat, MeleeDamageRaw, MeleeThreatRaw, MeleeDamageAverage, MeleeThreatAverage);
			//dictValues["Maul"] = String.Format(attackFormatWithRage, MaulDamageRaw, MaulThreatRaw, MaulDamageAverage, MaulThreatAverage, MaulTPR, MaulDPR);
			//dictValues["Mangle"] = String.Format(attackFormatWithRage, MangleDamageRaw, MangleThreatRaw, MangleDamageAverage, MangleThreatAverage, MangleTPR, MangleDPR);
			//dictValues["Swipe"] = String.Format(attackFormatWithRage, SwipeDamageRaw, SwipeThreatRaw, SwipeDamageAverage, SwipeThreatAverage, SwipeTPR, SwipeDPR);
			//dictValues["Faerie Fire"] = String.Format(attackFormat, FaerieFireDamageRaw, FaerieFireThreatRaw, FaerieFireDamageAverage, FaerieFireThreatAverage);
			//dictValues["Lacerate"] = String.Format(attackFormatWithRage, LacerateDamageRaw, LacerateThreatRaw, LacerateDamageAverage, LacerateThreatAverage, LacerateTPR, LacerateDPR);
			//dictValues["Lacerate DoT Tick"] = String.Format(attackFormat, LacerateDotDamageRaw, LacerateDotThreatRaw, LacerateDotDamageAverage, LacerateDotThreatAverage).Replace("Swing", "Tick");

			return dictValues;
		}

		private string GetRotationTooltip(string name)
		{
			StringBuilder tooltip = new StringBuilder("Ability Priority: \r\n");
			if (name.Contains("Maul")) tooltip.Append("Maul>");
			if (name.Contains("Mangle")) tooltip.Append("Mangle>");
			if (name.Contains("Faerie")) tooltip.Append("Faerie Fire>");
			if (name.Contains("Swipe") && name.Contains("Lacerate")) tooltip.Append("Lacerate(+)>Swipe\r\n(+) means to only stack and refresh the debuff.");
			else if (name.Contains("Swipe")) tooltip.Append("Swipe");
			else if (name.Contains("Lacerate")) tooltip.Append("Lacerate");

			return tooltip.ToString();
		}

		public override float GetOptimizableCalculationValue(string calculation)
		{
			switch (calculation)
			{
				case "Health": return BasicStats.Health;
				case "Avoided Attacks %": return AvoidedAttacks * 100f;
				case "Avoided Interrupts %": return MissedAttacks * 100f;
				case "Mitigation % from Armor": return TotalConstantDamageReduction * 100f;
				case "Avoidance %": return AvoidancePostDR * 100f;
				case "% Chance to be Crit": return ((5f + (0.2f * (TargetLevel - 85))) - CritReduction * 100f);
				case "Nature Survival": return NatureSurvivalPoints;
				case "Fire Survival": return FireSurvivalPoints;
				case "Frost Survival": return FrostSurvivalPoints;
				case "Shadow Survival": return ShadowSurvivalPoints;
				case "Arcane Survival": return ArcaneSurvivalPoints;
				case "Nature Resist": return BasicStats.NatureResistance;
				case "Fire Resist": return BasicStats.FireResistance;
				case "Frost Resist": return BasicStats.FrostResistance;
				case "Shadow Resist": return BasicStats.ShadowResistance;
				case "Arcane Resist": return BasicStats.ArcaneResistance;
				case "Highest DPS": return HighestDPSRotation.DPS;
				case "Highest TPS": return HighestTPSRotation.TPS;
				case "Swipe DPS": return SwipeRotation.DPS;
				case "Swipe TPS": return SwipeRotation.TPS;
				case "Custom Rotation DPS": return CustomRotation.DPS;
				case "Custom Rotation TPS": return CustomRotation.TPS;
			}
			return 0f;
		}
	}
}
