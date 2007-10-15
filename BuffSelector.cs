using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class BuffSelector : UserControl
	{
		public BuffSelector()
		{
			InitializeComponent();
		}

		private bool _loadingCharacter = false;
		private Character _character = null;
		public Character Character
		{
			get { return _character; }
			set
			{
				_character = value;
				_loadingCharacter = true;

				if (Character != null)
				{
					checkBoxAgilityFood.Checked = Character.Buffs.AgilityFood;
					checkBoxBlessingOfKings.Checked = Character.Buffs.BlessingOfKings;
					checkBoxBloodPact.Checked = Character.Buffs.BloodPact;
					checkBoxCommandingShout.Checked = Character.Buffs.CommandingShout;
					checkBoxDevotionAura.Checked = Character.Buffs.DevotionAura;
					checkBoxElixirOfIronskin.Checked = Character.Buffs.ElixirOfIronskin;
					checkBoxElixirOfMajorAgility.Checked = Character.Buffs.ElixirOfMajorAgility;
					checkBoxElixirOfMajorDefense.Checked = Character.Buffs.ElixirOfMajorDefense;
					checkBoxElixirOfMastery.Checked = Character.Buffs.ElixirOfMastery;
					checkBoxElixirOfMajorFortitude.Checked = Character.Buffs.ElixirOfMajorFortitude;
					checkBoxFlaskOfFortification.Checked = Character.Buffs.FlaskOfFortification;
					checkBoxFlaskOfChromaticWonder.Checked = Character.Buffs.FlaskOfChromaticWonder;
					checkBoxGraceOfAirTotem.Checked = Character.Buffs.GraceOfAirTotem;
					checkBoxImprovedBloodPact.Checked = Character.Buffs.ImprovedBloodPact;
					checkBoxImprovedCommandingShout.Checked = Character.Buffs.ImprovedCommandingShout;
					checkBoxImprovedDevotionAura.Checked = Character.Buffs.ImprovedDevotionAura;
					checkBoxImprovedGraceOfAirTotem.Checked = Character.Buffs.ImprovedGraceOfAirTotem;
					checkBoxImprovedMarkOfTheWild.Checked = Character.Buffs.ImprovedMarkOfTheWild;
					checkBoxImprovedPowerWordFortitude.Checked = Character.Buffs.ImprovedPowerWordFortitude;
					checkBoxInsectSwarm.Checked = Character.Buffs.InsectSwarm;
					checkBoxMarkOfTheWild.Checked = Character.Buffs.MarkOfTheWild;
					checkBoxPowerWordFortitude.Checked = Character.Buffs.PowerWordFortitude;
					checkBoxScorpidSting.Checked = Character.Buffs.ScorpidSting;
					checkBoxStaminaFood.Checked = Character.Buffs.StaminaFood;
					checkBoxMalorne4PieceBonus.Checked = Character.Buffs.Malorne4PieceBonus;
					checkBoxScrollOfProtection.Checked = Character.Buffs.ScrollOfProtection;
					checkBoxScrollOfAgility.Checked = Character.Buffs.ScrollOfAgility;
					checkBoxShadowEmbrace.Checked = Character.Buffs.ShadowEmbrace;
					checkBoxGladiatorResilience.Checked = Character.Buffs.GladiatorResilience;
					checkBoxDualWieldingMob.Checked = Character.Buffs.DualWieldingMob;
					checkBoxBadgeOfTenacity.Checked = Character.Buffs.BadgeOfTenacity;
					checkBoxMoroesLuckyPocketWatch.Checked = Character.Buffs.MoroesLuckyPocketWatch;
					checkBoxIdolOfTerror.Checked = Character.Buffs.IdolOfTerror;
					checkBoxAncestralFortitude.Checked = Character.Buffs.AncestralFortitude;
					checkBoxImprovedLayOnHands.Checked = Character.Buffs.ImprovedLayOnHands;
					checkBoxHeroicPotion.Checked = Character.Buffs.HeroicPotion;
					checkBoxIronshieldPotion.Checked = Character.Buffs.IronshieldPotion;
					checkBoxNightmareSeed.Checked = Character.Buffs.NightmareSeed;
					checkBoxHeroicHPTrinket.Checked = Character.Buffs.Heroic1750HealthTrinket;
					checkBoxSeason3ResilienceRelic.Checked = Character.Buffs.Season3ResilienceRelic;
					checkBoxMoongladeRejuvination.Checked = Character.Buffs.MoongladeRejuvination;
					checkBoxLivingRootOfTheWildheart.Checked = Character.Buffs.LivingRootOfTheWildheart;
					checkBoxArgussianCompass.Checked = Character.Buffs.ArgussianCompass;
					checkBoxDawnstoneCrab.Checked = Character.Buffs.DawnstoneCrab;
					checkBoxAdamantiteFigurine.Checked = Character.Buffs.AdamantiteFigurine;
					checkBoxBroochOfTheImmortalKing.Checked = Character.Buffs.BroochOfTheImmortalKing;
				}

				UpdateEnabledStates();
				_loadingCharacter = false;
			}
		}

		private void checkBoxBuff_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter && Character != null)
			{
				UpdateEnabledStates();
				UpdateCharacterBuffs();
				Character.OnItemsChanged();
			}
		}

		private void UpdateCharacterBuffs()
		{
			Character.Buffs.AgilityFood = checkBoxAgilityFood.Checked && checkBoxAgilityFood.Enabled;
			Character.Buffs.BlessingOfKings = checkBoxBlessingOfKings.Checked && checkBoxBlessingOfKings.Enabled;
			Character.Buffs.BloodPact = checkBoxBloodPact.Checked && checkBoxBloodPact.Enabled;
			Character.Buffs.CommandingShout = checkBoxCommandingShout.Checked && checkBoxCommandingShout.Enabled;
			Character.Buffs.DevotionAura = checkBoxDevotionAura.Checked && checkBoxDevotionAura.Enabled;
			Character.Buffs.ElixirOfIronskin = checkBoxElixirOfIronskin.Checked && checkBoxElixirOfIronskin.Enabled;
			Character.Buffs.ElixirOfMajorAgility = checkBoxElixirOfMajorAgility.Checked && checkBoxElixirOfMajorAgility.Enabled;
			Character.Buffs.ElixirOfMajorDefense = checkBoxElixirOfMajorDefense.Checked && checkBoxElixirOfMajorDefense.Enabled;
			Character.Buffs.ElixirOfMastery = checkBoxElixirOfMastery.Checked && checkBoxElixirOfMastery.Enabled;
			Character.Buffs.ElixirOfMajorFortitude = checkBoxElixirOfMajorFortitude.Checked && checkBoxElixirOfMajorFortitude.Enabled;
			Character.Buffs.FlaskOfFortification = checkBoxFlaskOfFortification.Checked && checkBoxFlaskOfFortification.Enabled;
			Character.Buffs.FlaskOfChromaticWonder = checkBoxFlaskOfChromaticWonder.Checked && checkBoxFlaskOfChromaticWonder.Enabled;
			Character.Buffs.GraceOfAirTotem = checkBoxGraceOfAirTotem.Checked && checkBoxGraceOfAirTotem.Enabled;
			Character.Buffs.ImprovedBloodPact = checkBoxImprovedBloodPact.Checked && checkBoxImprovedBloodPact.Enabled;
			Character.Buffs.ImprovedCommandingShout = checkBoxImprovedCommandingShout.Checked && checkBoxImprovedCommandingShout.Enabled;
			Character.Buffs.ImprovedDevotionAura = checkBoxImprovedDevotionAura.Checked && checkBoxImprovedDevotionAura.Enabled;
			Character.Buffs.ImprovedGraceOfAirTotem = checkBoxImprovedGraceOfAirTotem.Checked && checkBoxImprovedGraceOfAirTotem.Enabled;
			Character.Buffs.ImprovedMarkOfTheWild = checkBoxImprovedMarkOfTheWild.Checked && checkBoxImprovedMarkOfTheWild.Enabled;
			Character.Buffs.ImprovedPowerWordFortitude = checkBoxImprovedPowerWordFortitude.Checked && checkBoxImprovedPowerWordFortitude.Enabled;
			Character.Buffs.InsectSwarm = checkBoxInsectSwarm.Checked && checkBoxInsectSwarm.Enabled;
			Character.Buffs.MarkOfTheWild = checkBoxMarkOfTheWild.Checked && checkBoxMarkOfTheWild.Enabled;
			Character.Buffs.PowerWordFortitude = checkBoxPowerWordFortitude.Checked && checkBoxPowerWordFortitude.Enabled;
			Character.Buffs.ScorpidSting = checkBoxScorpidSting.Checked && checkBoxScorpidSting.Enabled;
			Character.Buffs.StaminaFood = checkBoxStaminaFood.Checked && checkBoxStaminaFood.Enabled;
			Character.Buffs.Malorne4PieceBonus = checkBoxMalorne4PieceBonus.Checked && checkBoxMalorne4PieceBonus.Enabled;
			Character.Buffs.ScrollOfProtection = checkBoxScrollOfProtection.Checked && checkBoxScrollOfProtection.Enabled;
			Character.Buffs.ScrollOfAgility = checkBoxScrollOfAgility.Checked && checkBoxScrollOfAgility.Enabled;
			Character.Buffs.ShadowEmbrace = checkBoxShadowEmbrace.Checked && checkBoxShadowEmbrace.Enabled;
			Character.Buffs.GladiatorResilience = checkBoxGladiatorResilience.Checked && checkBoxGladiatorResilience.Enabled;
			Character.Buffs.DualWieldingMob = checkBoxDualWieldingMob.Checked && checkBoxDualWieldingMob.Enabled;
			Character.Buffs.BadgeOfTenacity = checkBoxBadgeOfTenacity.Checked && checkBoxBadgeOfTenacity.Enabled;
			Character.Buffs.MoroesLuckyPocketWatch = checkBoxMoroesLuckyPocketWatch.Checked && checkBoxMoroesLuckyPocketWatch.Enabled;
			Character.Buffs.IdolOfTerror = checkBoxIdolOfTerror.Checked && checkBoxIdolOfTerror.Enabled;
			Character.Buffs.AncestralFortitude = checkBoxAncestralFortitude.Checked && checkBoxAncestralFortitude.Enabled;
			Character.Buffs.ImprovedLayOnHands = checkBoxImprovedLayOnHands.Checked && checkBoxImprovedLayOnHands.Enabled;
			Character.Buffs.HeroicPotion = checkBoxHeroicPotion.Checked && checkBoxHeroicPotion.Enabled;
			Character.Buffs.IronshieldPotion = checkBoxIronshieldPotion.Checked && checkBoxIronshieldPotion.Enabled;
			Character.Buffs.NightmareSeed = checkBoxNightmareSeed.Checked && checkBoxNightmareSeed.Enabled;
			Character.Buffs.Heroic1750HealthTrinket = checkBoxHeroicHPTrinket.Checked && checkBoxHeroicHPTrinket.Enabled;
			Character.Buffs.Season3ResilienceRelic = checkBoxSeason3ResilienceRelic.Checked && checkBoxSeason3ResilienceRelic.Enabled;
			Character.Buffs.MoongladeRejuvination = checkBoxMoongladeRejuvination.Checked && checkBoxMoongladeRejuvination.Enabled;
			Character.Buffs.LivingRootOfTheWildheart = checkBoxLivingRootOfTheWildheart.Checked && checkBoxLivingRootOfTheWildheart.Enabled;
			Character.Buffs.ArgussianCompass = checkBoxArgussianCompass.Checked && checkBoxArgussianCompass.Enabled;
			Character.Buffs.DawnstoneCrab = checkBoxDawnstoneCrab.Checked && checkBoxDawnstoneCrab.Enabled;
			Character.Buffs.AdamantiteFigurine = checkBoxAdamantiteFigurine.Checked && checkBoxAdamantiteFigurine.Enabled;
			Character.Buffs.BroochOfTheImmortalKing = checkBoxBroochOfTheImmortalKing.Checked && checkBoxBroochOfTheImmortalKing.Enabled;
		}

		private void UpdateEnabledStates()
		{
			//If I was in World War II, they'd call me Spitfire!
			checkBoxImprovedDevotionAura.Enabled = checkBoxDevotionAura.Checked;
			checkBoxImprovedPowerWordFortitude.Enabled = checkBoxPowerWordFortitude.Checked;
			checkBoxImprovedMarkOfTheWild.Enabled = checkBoxMarkOfTheWild.Checked;
			checkBoxImprovedBloodPact.Enabled = checkBoxBloodPact.Checked;
			checkBoxImprovedCommandingShout.Enabled = checkBoxCommandingShout.Checked;
			checkBoxImprovedGraceOfAirTotem.Enabled = checkBoxGraceOfAirTotem.Checked;

			checkBoxAgilityFood.Enabled = !checkBoxStaminaFood.Checked;
			checkBoxStaminaFood.Enabled = !checkBoxAgilityFood.Checked;

			checkBoxHeroicPotion.Enabled = !checkBoxIronshieldPotion.Checked;
			checkBoxIronshieldPotion.Enabled = !checkBoxHeroicPotion.Checked;

			checkBoxElixirOfMajorAgility.Enabled = !checkBoxFlaskOfFortification.Checked &&
				!checkBoxFlaskOfChromaticWonder.Checked && !checkBoxElixirOfMastery.Checked;
			checkBoxElixirOfMastery.Enabled = !checkBoxFlaskOfFortification.Checked &&
				!checkBoxFlaskOfChromaticWonder.Checked && !checkBoxElixirOfMajorAgility.Checked;
			checkBoxElixirOfMajorDefense.Enabled = !checkBoxFlaskOfFortification.Checked && !checkBoxElixirOfIronskin.Checked &&
				!checkBoxFlaskOfChromaticWonder.Checked && !checkBoxElixirOfMajorFortitude.Checked;
			checkBoxElixirOfIronskin.Enabled = !checkBoxFlaskOfFortification.Checked && !checkBoxElixirOfMajorDefense.Checked &&
				!checkBoxFlaskOfChromaticWonder.Checked && !checkBoxElixirOfMajorFortitude.Checked;
			checkBoxElixirOfMajorFortitude.Enabled = !checkBoxFlaskOfFortification.Checked && !checkBoxElixirOfMajorDefense.Checked &&
				!checkBoxFlaskOfChromaticWonder.Checked && !checkBoxElixirOfIronskin.Checked;
			checkBoxFlaskOfFortification.Enabled = !checkBoxElixirOfMajorAgility.Checked && !checkBoxElixirOfMajorDefense.Checked &&
				!checkBoxFlaskOfChromaticWonder.Checked && !checkBoxElixirOfIronskin.Checked && !checkBoxElixirOfMastery.Checked && !checkBoxElixirOfMajorFortitude.Checked;
			checkBoxFlaskOfChromaticWonder.Enabled = !checkBoxElixirOfMajorAgility.Checked && !checkBoxElixirOfMajorDefense.Checked &&
				!checkBoxFlaskOfFortification.Checked && !checkBoxElixirOfIronskin.Checked && !checkBoxElixirOfMastery.Checked && !checkBoxElixirOfMajorFortitude.Checked;
		}
	}
}
