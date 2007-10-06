using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
	public class Buffs
	{
		public bool PowerWordFortitude = false;
		public bool ImprovedPowerWordFortitude = false;
		public bool MarkOfTheWild = false;
		public bool ImprovedMarkOfTheWild = false;
		public bool BloodPact = false;
		public bool ImprovedBloodPact = false;
		public bool StaminaFood = false;
		public bool AgilityFood = false;
		public bool CommandingShout = false;
		public bool ImprovedCommandingShout = false;
		public bool DevotionAura = false;
		public bool ImprovedDevotionAura = false;
		public bool GraceOfAirTotem = false;
		public bool ImprovedGraceOfAirTotem = false;
		public bool BlessingOfKings = false;
		public bool ElixirOfIronskin = false;
		public bool ElixirOfMajorAgility = false;
		public bool ElixirOfMastery = false;
		public bool ScorpidSting = false;
		public bool InsectSwarm = false;
		public bool ElixirOfMajorDefense = false;
		public bool ElixirOfMajorFortitude = false;
		public bool FlaskOfFortification = false;
		public bool FlaskOfChromaticWonder = false;
		public bool Malorne4PieceBonus = false;
		public bool ScrollOfProtection = false;
		public bool ScrollOfAgility = false;
		public bool ShadowEmbrace = false;
		public bool GladiatorResilience = false;

		public static string[] AllBuffs = new string[] { "Power Word: Fortitude", "Improved Power Word: Fortitude", "Mark of the Wild",
			"Improved Mark of the Wild", "Blood Pact", "Improved Blood Pact", "30 Stamina Food", "20 Agility Food", "Commanding Shout",
			"Improved Commanding Shout", "Devotion Aura", "Improved Devotion Aura", "Grace of Air Totem", "Improved Grace of Air Totem",
			"Blessing of Kings", "Elixir of Ironskin", "Elixir of Major Agility", "Elixir of Mastery", "Scorpid Sting", "Insect Swarm",
			"Elixir of Major Defense", "Elixir of Major Fortitude", "Flask of Fortification", "Flask of Chromatic Wonder", "Malorne 4 Piece Bonus", "Scroll of Protection",
			"Scroll of Agility", "Shadow Embrace", "Gladiator 2 Piece Bonus"};

		public Buffs() { }

		public Buffs(bool powerWordFortitude, bool improvedPowerWordFortitude, bool markOfTheWild,
			bool improvedMarkOfTheWild, bool bloodPact, bool improvedBloodPact, bool staminaFood, bool agilityFood, bool commandingShout,
			bool improvedCommandingShout, bool devotionAura, bool improvedDevotionAura, bool graceOfAirTotem, bool improvedGraceOfAirTotem,
			bool blessingOfKings, bool elixirOfIronskin, bool elixirOfMajorAgility, bool elixirOfMastery, bool scorpidSting, bool insectSwarm,
			bool elixirOfMajorDefense, bool elixirOfMajorFortitude, bool flaskOfFortitude, bool flaskOfChromaticWonder, bool malorne4PieceBonus, bool scrollOfProtection,
			bool scrollOfAgility, bool shadowEmbrace, bool gladiatorResilience)
		{
			PowerWordFortitude = powerWordFortitude;
			ImprovedPowerWordFortitude = improvedPowerWordFortitude;
			MarkOfTheWild = markOfTheWild;
			ImprovedMarkOfTheWild = improvedMarkOfTheWild;
			BloodPact = bloodPact;
			ImprovedBloodPact = improvedBloodPact;
			StaminaFood = staminaFood;
			AgilityFood = agilityFood;
			CommandingShout = commandingShout;
			ImprovedCommandingShout = improvedCommandingShout;
			DevotionAura = devotionAura;
			ImprovedDevotionAura = improvedDevotionAura;
			GraceOfAirTotem = graceOfAirTotem;
			ImprovedGraceOfAirTotem = improvedGraceOfAirTotem;
			BlessingOfKings = blessingOfKings;
			ElixirOfIronskin = elixirOfIronskin;
			ElixirOfMajorAgility = elixirOfMajorAgility;
			ElixirOfMastery = elixirOfMastery;
			ScorpidSting = scorpidSting;
			InsectSwarm = insectSwarm;
			ElixirOfMajorDefense = elixirOfMajorDefense;
			ElixirOfMajorFortitude = elixirOfMajorFortitude;
			FlaskOfFortification = flaskOfFortitude;
			FlaskOfChromaticWonder = flaskOfChromaticWonder;
			Malorne4PieceBonus = malorne4PieceBonus;
			ScrollOfProtection = scrollOfProtection;
			ScrollOfAgility = scrollOfAgility;
			ShadowEmbrace = shadowEmbrace;
			GladiatorResilience = gladiatorResilience;
		}

		public bool this[string name]
		{
			get
			{
				switch (name)
				{
					case "Power Word: Fortitude": return PowerWordFortitude;
					case "Improved Power Word: Fortitude": return ImprovedPowerWordFortitude;
					case "Mark of the Wild": return MarkOfTheWild;
					case "Improved Mark of the Wild": return ImprovedMarkOfTheWild;
					case "Blood Pact": return BloodPact;
					case "Improved Blood Pact": return ImprovedBloodPact;
					case "30 Stamina Food": return StaminaFood;
					case "20 Agility Food": return AgilityFood;
					case "Commanding Shout": return CommandingShout;
					case "Improved Commanding Shout": return ImprovedCommandingShout;
					case "Devotion Aura": return DevotionAura;
					case "Improved Devotion Aura": return ImprovedDevotionAura;
					case "Grace of Air Totem": return GraceOfAirTotem;
					case "Improved Grace of Air Totem": return ImprovedGraceOfAirTotem;
					case "Blessing of Kings": return BlessingOfKings;
					case "Elixir of Ironskin": return ElixirOfIronskin;
					case "Elixir of Major Agility": return ElixirOfMajorAgility;
					case "Elixir of Mastery": return ElixirOfMastery;
					case "Scorpid Sting": return ScorpidSting;
					case "Insect Swarm": return InsectSwarm;
					case "Elixir of Major Defense": return ElixirOfMajorDefense;
					case "Elixir of Major Fortitude": return ElixirOfMajorFortitude;
					case "Flask of Fortification": return FlaskOfFortification;
					case "Flask of Chromatic Wonder": return FlaskOfChromaticWonder;
					case "Malorne 4 Piece Bonus": return Malorne4PieceBonus;
					case "Scroll of Protection": return ScrollOfProtection;
					case "Scroll of Agility": return ScrollOfAgility;
					case "Shadow Embrace": return ShadowEmbrace;
					case "Gladiator 2 Piece Bonus": return GladiatorResilience;
					default: return false;
				}
			}
			set
			{
				switch (name)
				{
					case "Power Word: Fortitude": PowerWordFortitude = value; break;
					case "Improved Power Word: Fortitude": ImprovedPowerWordFortitude = value; break;
					case "Mark of the Wild": MarkOfTheWild = value; break;
					case "Improved Mark of the Wild": ImprovedMarkOfTheWild = value; break;
					case "Blood Pact": BloodPact = value; break;
					case "Improved Blood Pact": ImprovedBloodPact = value; break;
					case "30 Stamina Food": StaminaFood = value; break;
					case "20 Agility Food": AgilityFood = value; break;
					case "Commanding Shout": CommandingShout = value; break;
					case "Improved Commanding Shout": ImprovedCommandingShout = value; break;
					case "Devotion Aura": DevotionAura = value; break;
					case "Improved Devotion Aura": ImprovedDevotionAura = value; break;
					case "Grace of Air Totem": GraceOfAirTotem = value; break;
					case "Improved Grace of Air Totem": ImprovedGraceOfAirTotem = value; break;
					case "Blessing of Kings": BlessingOfKings = value; break;
					case "Elixir of Ironskin": ElixirOfIronskin = value; break;
					case "Elixir of Major Agility": ElixirOfMajorAgility = value; break;
					case "Elixir of Mastery": ElixirOfMastery = value; break;
					case "Scorpid Sting": ScorpidSting = value; break;
					case "Insect Swarm": InsectSwarm = value; break;
					case "Elixir of Major Defense": ElixirOfMajorDefense = value; break;
					case "Elixir of Major Fortitude": ElixirOfMajorFortitude = value; break;
					case "Flask of Fortification": FlaskOfFortification = value; break;
					case "Flask of Chromatic Wonder": FlaskOfChromaticWonder = value; break;
					case "Malorne 4 Piece Bonus": Malorne4PieceBonus = value; break;
					case "Scroll of Protection": ScrollOfProtection = value; break;
					case "Scroll of Agility": ScrollOfAgility = value; break;
					case "Shadow Embrace": ShadowEmbrace = value; break;
					case "Gladiator 2 Piece Bonus": GladiatorResilience = value; break;
				}
			}
		}
	}
}
