using System;
using System.Collections.Generic;
using System.Text;

namespace Rawr
{
	public class Buffs
	{
		//Class Buffs
		public bool PowerWordFortitude = false;
		public bool ImprovedPowerWordFortitude = false;
		public bool MarkOfTheWild = false;
		public bool ImprovedMarkOfTheWild = false;
		public bool BloodPact = false;
		public bool ImprovedBloodPact = false;
		public bool CommandingShout = false;
		public bool ImprovedCommandingShout = false;
		public bool DevotionAura = false;
		public bool ImprovedDevotionAura = false;
		public bool GraceOfAirTotem = false;
		public bool ImprovedGraceOfAirTotem = false;
		public bool BlessingOfKings = false;

		//Elixirs and Flasks
		public bool ElixirOfMajorDefense = false;
		public bool ElixirOfIronskin = false;
		public bool ElixirOfMajorFortitude = false;
		public bool ElixirOfMastery = false;
		public bool ElixirOfMajorAgility = false;
		public bool FlaskOfFortification = false;
		public bool FlaskOfChromaticWonder = false;

		//Other Consumables
		public bool AgilityFood = false;
		public bool StaminaFood = false;
		public bool ScrollOfAgility = false;
		public bool ScrollOfProtection = false;
		
		//Debuffs
		public bool ScorpidSting = false;
		public bool InsectSwarm = false;
		public bool ShadowEmbrace = false;
		public bool DualWieldingMob = false;

		//Set Bonuses
		public bool Malorne4PieceBonus = false;
		public bool GladiatorResilience = false;

		//Temporary Buffs
		public bool BadgeOfTenacity = false; 
		public bool MoroesLuckyPocketWatch = false; 
		public bool IdolOfTerror = false; 
		public bool AncestralFortitude = false;
		public bool ImprovedLayOnHands = false;
		public bool HeroicPotion = false; 
		public bool IronshieldPotion = false;
		public bool NightmareSeed = false; 
		public bool Heroic1750HealthTrinket = false; 
		public bool Season3ResilienceRelic = false;
		public bool MoongladeRejuvination = false;
		public bool LivingRootOfTheWildheart = false; 
		public bool ArgussianCompass = false;
		public bool DawnstoneCrab = false;
		public bool AdamantiteFigurine = false; 
		public bool BroochOfTheImmortalKing = false;

		public enum BuffCatagory
		{
			AllBuffs,
			AllLongDurationBuffsNoDW,
			CurrentBuffs
		}

		public static string[] AllLongDurationBuffsNoDW = new string[] { "Power Word: Fortitude", "Improved Power Word: Fortitude", "Mark of the Wild",
			"Improved Mark of the Wild", "Blood Pact", "Improved Blood Pact", "30 Stamina Food", "20 Agility Food", "Commanding Shout",
			"Improved Commanding Shout", "Devotion Aura", "Improved Devotion Aura", "Grace of Air Totem", "Improved Grace of Air Totem",
			"Blessing of Kings", "Elixir of Ironskin", "Elixir of Major Agility", "Elixir of Mastery", "Scorpid Sting", "Insect Swarm",
			"Elixir of Major Defense", "Elixir of Major Fortitude", "Flask of Fortification", "Flask of Chromatic Wonder", "Malorne 4 Piece Bonus", "Scroll of Protection",
			"Scroll of Agility", "Shadow Embrace", "Gladiator 2 Piece Bonus" };

		public static string[] AllBuffs = new string[] { "Power Word: Fortitude", "Improved Power Word: Fortitude", "Mark of the Wild",
			"Improved Mark of the Wild", "Blood Pact", "Improved Blood Pact", "30 Stamina Food", "20 Agility Food", "Commanding Shout",
			"Improved Commanding Shout", "Devotion Aura", "Improved Devotion Aura", "Grace of Air Totem", "Improved Grace of Air Totem",
			"Blessing of Kings", "Elixir of Ironskin", "Elixir of Major Agility", "Elixir of Mastery", "Scorpid Sting", "Insect Swarm",
			"Elixir of Major Defense", "Elixir of Major Fortitude", "Flask of Fortification", "Flask of Chromatic Wonder", "Malorne 4 Piece Bonus", "Scroll of Protection",
			"Scroll of Agility", "Shadow Embrace", "Gladiator 2 Piece Bonus", "Dual Wielding Mob", "Badge of Tenacity",
			"Moroes' Lucky Pocket Watch", "Idol of Terror", "Ancestral Fortitude / Inspiration", "Improved Lay On Hands", "Heroic Potion",
			"Ironshield Potion", "Nightmare Seed", "Heroic 1750 Health Trinket", "Season 3 Resilience Relic", "Moonglade Rejuvination",
			"Living Root of the Wildheart", "Argussian Compass", "Dawnstone Crab", "Adamantite Figurine", "Brooch of the Immortal King" };

		public Buffs() { }

		public Buffs(bool powerWordFortitude, bool improvedPowerWordFortitude, bool markOfTheWild,
			bool improvedMarkOfTheWild, bool bloodPact, bool improvedBloodPact, bool staminaFood, bool agilityFood, bool commandingShout,
			bool improvedCommandingShout, bool devotionAura, bool improvedDevotionAura, bool graceOfAirTotem, bool improvedGraceOfAirTotem,
			bool blessingOfKings, bool elixirOfIronskin, bool elixirOfMajorAgility, bool elixirOfMastery, bool scorpidSting, bool insectSwarm,
			bool elixirOfMajorDefense, bool elixirOfMajorFortitude, bool flaskOfFortitude, bool flaskOfChromaticWonder, bool malorne4PieceBonus, bool scrollOfProtection,
			bool scrollOfAgility, bool shadowEmbrace, bool gladiatorResilience, bool dualWieldingMob, bool badgeOfTenacity,
			bool moroesLuckyPocketWatch, bool idolOfTerror, bool ancestralFortitude, bool improvedLayOnHands, bool heroicPotion,
			bool ironshieldPotion, bool nightmareSeed, bool heroic1750HealthTrinket, bool season3ResilienceRelic, bool moongladeRejuvination,
			bool livingRootOfTheWildheart, bool argussianCompass, bool dawnstoneCrab, bool adamantiteFigurine, bool broochOfTheImmortalKing)
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
			DualWieldingMob = dualWieldingMob;
			BadgeOfTenacity = badgeOfTenacity;
			MoroesLuckyPocketWatch = moroesLuckyPocketWatch;
			IdolOfTerror = idolOfTerror;
			AncestralFortitude = ancestralFortitude;
			ImprovedLayOnHands = improvedLayOnHands;
			HeroicPotion = heroicPotion;
			IronshieldPotion = ironshieldPotion;
			NightmareSeed = nightmareSeed;
			Heroic1750HealthTrinket = heroic1750HealthTrinket;
			Season3ResilienceRelic = season3ResilienceRelic;
			MoongladeRejuvination = moongladeRejuvination;
			LivingRootOfTheWildheart = livingRootOfTheWildheart;
			ArgussianCompass = argussianCompass;
			DawnstoneCrab = dawnstoneCrab;
			AdamantiteFigurine = adamantiteFigurine;
			BroochOfTheImmortalKing = broochOfTheImmortalKing;
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
					case "Dual Wielding Mob": return DualWieldingMob;
					case "Badge of Tenacity": return BadgeOfTenacity;
					case "Moroes' Lucky Pocket Watch": return MoroesLuckyPocketWatch;
					case "Idol of Terror": return IdolOfTerror;
					case "Ancestral Fortitude / Inspiration": return AncestralFortitude;
					case "Improved Lay On Hands": return ImprovedLayOnHands;
					case "Heroic Potion": return HeroicPotion;
					case "Ironshield Potion": return IronshieldPotion;
					case "Nightmare Seed": return NightmareSeed;
					case "Heroic 1750 Health Trinket": return Heroic1750HealthTrinket;
					case "Season 3 Resilience Relic": return Season3ResilienceRelic;
					case "Moonglade Rejuvination": return MoongladeRejuvination;
					case "Living Root of the Wildheart": return LivingRootOfTheWildheart;
					case "Argussian Compass": return ArgussianCompass;
					case "Dawnstone Crab": return DawnstoneCrab;
					case "Adamantite Figurine": return AdamantiteFigurine;
					case "Brooch of the Immortal King": return BroochOfTheImmortalKing;
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
					case "Dual Wielding Mob": DualWieldingMob = value; break;
					case "Badge of Tenacity": BadgeOfTenacity = value; break;
					case "Moroes' Lucky Pocket Watch": MoroesLuckyPocketWatch = value; break;
					case "Idol of Terror": IdolOfTerror = value; break;
					case "Ancestral Fortitude / Inspiration": AncestralFortitude = value; break;
					case "Improved Lay On Hands": ImprovedLayOnHands = value; break;
					case "Heroic Potion": HeroicPotion = value; break;
					case "Ironshield Potion": IronshieldPotion = value; break;
					case "Nightmare Seed": NightmareSeed = value; break;
					case "Heroic 1750 Health Trinket": Heroic1750HealthTrinket = value; break;
					case "Season 3 Resilience Relic": Season3ResilienceRelic = value; break;
					case "Moonglade Rejuvination": MoongladeRejuvination = value; break;
					case "Living Root of the Wildheart": LivingRootOfTheWildheart = value; break;
					case "Argussian Compass": ArgussianCompass = value; break;
					case "Dawnstone Crab": DawnstoneCrab = value; break;
					case "Adamantite Figurine": AdamantiteFigurine = value; break;
					case "Brooch of the Immortal King": BroochOfTheImmortalKing = value; break;
				}
			}
		}
	}
}
