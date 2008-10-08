using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Moonkin
{
    public partial class CalculationOptionsPanelMoonkin : CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelMoonkin()
        {
            InitializeComponent();
        }

        // Load the options into the form
        protected override void LoadCalculationOptions()
        {
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsMoonkin(Character);

			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
            cmbTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
			txtLatency.Text = calcOpts.Latency.ToString();
            txtFightLength.Text = calcOpts.FightLength.ToString();
            txtShadowPriest.Text = calcOpts.ShadowPriest.ToString();
            chkInnervate.Checked = calcOpts.Innervate;
            chkManaPots.Checked = calcOpts.ManaPots;
            cmbPotType.SelectedItem = calcOpts.ManaPotType;
            cmbPotType.Enabled = chkManaPots.Checked;
            txtInnervateDelay.Text = calcOpts.InnervateDelay.ToString();
            txtInnervateDelay.Enabled = chkInnervate.Checked;
            txtManaPotDelay.Text = calcOpts.ManaPotDelay.ToString();
            txtManaPotDelay.Enabled = chkManaPots.Checked;
            chkInnervateWeapon.Checked = calcOpts.InnervateWeapon;
            txtInnervateWeaponInt.Enabled = chkInnervateWeapon.Checked;
            txtInnervateWeaponInt.Text = calcOpts.InnervateWeaponInt.ToString();
            txtInnervateWeaponSpi.Enabled = chkInnervateWeapon.Checked;
            txtInnervateWeaponSpi.Text = calcOpts.InnervateWeaponSpi.ToString();
            rdbAldor.Checked = calcOpts.AldorScryer == "Aldor";
            rdbScryer.Checked = calcOpts.AldorScryer == "Scryer";
        }

        private void cmbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.TargetLevel = int.Parse(cmbTargetLevel.SelectedItem.ToString());
            Character.OnCalculationsInvalidated();
        }

        private void txtLatency_TextChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.Latency = float.Parse(txtLatency.Text);
            Character.OnCalculationsInvalidated();
        }

        private void txtFightLength_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.FightLength = float.Parse(txtFightLength.Text);
            Character.OnCalculationsInvalidated();
        }

        private void chkInnervate_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.Innervate = chkInnervate.Checked;
            txtInnervateDelay.Enabled = chkInnervate.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void txtShadowPriest_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.ShadowPriest = float.Parse(txtShadowPriest.Text);
            Character.OnCalculationsInvalidated();
        }

        private void chkManaPots_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.ManaPots = chkManaPots.Checked;
            cmbPotType.Enabled = chkManaPots.Checked;
            txtManaPotDelay.Enabled = chkManaPots.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void cmbPotType_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.ManaPotType = cmbPotType.SelectedItem.ToString();
            Character.OnCalculationsInvalidated();
        }

        private void txtInnervateDelay_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.InnervateDelay = float.Parse(txtInnervateDelay.Text);
            Character.OnCalculationsInvalidated();
        }

        private void txtManaPotDelay_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.ManaPotDelay = float.Parse(txtManaPotDelay.Text);
            Character.OnCalculationsInvalidated();
        }

        private void txtInnervateWeaponInt_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.InnervateWeaponInt = float.Parse(txtInnervateWeaponInt.Text);
            Character.OnCalculationsInvalidated();
        }

        private void txtInnervateWeaponSpi_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.InnervateWeaponSpi = float.Parse(txtInnervateWeaponSpi.Text);
            Character.OnCalculationsInvalidated();
        }

        private void chkInnervateWeapon_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.InnervateWeapon = chkInnervateWeapon.Checked;
            txtInnervateWeaponInt.Enabled = chkInnervateWeapon.Checked;
            txtInnervateWeaponSpi.Enabled = chkInnervateWeapon.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void rdbScryer_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.AldorScryer = rdbScryer.Checked ? "Scryer" : "Aldor";
            Character.OnCalculationsInvalidated();
        }
    }

	[Serializable]
	public class CalculationOptionsMoonkin : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsMoonkin));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

        public CalculationOptionsMoonkin()
        {
        }

        public CalculationOptionsMoonkin(Character character)
        {
            #region Druid Talents Import
            try
            {
                WebRequestWrapper wrw = new WebRequestWrapper();
                if (character.Class == Character.CharacterClass.Druid && character.Name != null && character.Region != null && character.Realm != null)
                {
                    XmlDocument docTalents = wrw.DownloadCharacterTalentTree(character.Name, character.Region, character.Realm);

                    //<talentTab>
                    //  <talentTree value="50002201050313523105100000000000000530000000000300001000030300"/>
                    //</talentTab>
                    string talentCode = docTalents.SelectSingleNode("page/characterInfo/talentTab/talentTree").Attributes["value"].Value;
                    character.DruidTalents.StarlightWrath = int.Parse(talentCode.Substring(0, 1));
                    character.DruidTalents.Genesis = int.Parse(talentCode.Substring(1, 1));
                    character.DruidTalents.Moonglow = int.Parse(talentCode.Substring(2, 1));
                    character.DruidTalents.NaturesMastery = int.Parse(talentCode.Substring(3, 1));
                    character.DruidTalents.ImprovedMoonfire = int.Parse(talentCode.Substring(4, 1));
                    character.DruidTalents.Brambles = int.Parse(talentCode.Substring(5, 1));
                    character.DruidTalents.NaturesGrace = int.Parse(talentCode.Substring(6, 1));
                    character.DruidTalents.NaturesSplendor = int.Parse(talentCode.Substring(7, 1));
                    character.DruidTalents.NaturesReach = int.Parse(talentCode.Substring(8, 1));
                    character.DruidTalents.Vengeance = int.Parse(talentCode.Substring(9, 1));
                    character.DruidTalents.CelestialFocus = int.Parse(talentCode.Substring(10, 1));
                    character.DruidTalents.LunarGuidance = int.Parse(talentCode.Substring(11, 1));
                    character.DruidTalents.InsectSwarm = int.Parse(talentCode.Substring(12, 1));
                    character.DruidTalents.ImprovedInsectSwarm = int.Parse(talentCode.Substring(13, 1));
                    character.DruidTalents.Dreamstate = int.Parse(talentCode.Substring(14, 1));
                    character.DruidTalents.Moonfury = int.Parse(talentCode.Substring(15, 1));
                    character.DruidTalents.BalanceOfPower = int.Parse(talentCode.Substring(16, 1));
                    character.DruidTalents.MoonkinForm = int.Parse(talentCode.Substring(17, 1));
                    character.DruidTalents.ImprovedMoonkinForm = int.Parse(talentCode.Substring(18, 1));
                    character.DruidTalents.ImprovedFaerieFire = int.Parse(talentCode.Substring(19, 1));
                    character.DruidTalents.OwlkinFrenzy = int.Parse(talentCode.Substring(20, 1));
                    character.DruidTalents.WrathOfCenarius = int.Parse(talentCode.Substring(21, 1));
                    character.DruidTalents.Eclipse = int.Parse(talentCode.Substring(22, 1));
                    character.DruidTalents.Typhoon = int.Parse(talentCode.Substring(23, 1));
                    character.DruidTalents.ForceOfNature = int.Parse(talentCode.Substring(24, 1));
                    character.DruidTalents.GaleWinds = int.Parse(talentCode.Substring(25, 1));
                    character.DruidTalents.EarthAndMoon = int.Parse(talentCode.Substring(26, 1));
                    character.DruidTalents.Starfall = int.Parse(talentCode.Substring(27, 1));
                    character.DruidTalents.Ferocity = int.Parse(talentCode.Substring(28, 1));
                    character.DruidTalents.FeralAggression = int.Parse(talentCode.Substring(29, 1));
                    character.DruidTalents.FeralInstinct = int.Parse(talentCode.Substring(30, 1));
                    character.DruidTalents.SavageFury = int.Parse(talentCode.Substring(31, 1));
                    character.DruidTalents.ThickHide = int.Parse(talentCode.Substring(32, 1));
                    character.DruidTalents.FeralSwiftness = int.Parse(talentCode.Substring(33, 1));
                    character.DruidTalents.SurvivalInstincts = int.Parse(talentCode.Substring(34, 1));
                    character.DruidTalents.SharpenedClaws = int.Parse(talentCode.Substring(35, 1));
                    character.DruidTalents.ShreddingAttacks = int.Parse(talentCode.Substring(36, 1));
                    character.DruidTalents.PredatoryStrikes = int.Parse(talentCode.Substring(37, 1));
                    character.DruidTalents.PrimalFury = int.Parse(talentCode.Substring(38, 1));
                    character.DruidTalents.PrimalPrecision = int.Parse(talentCode.Substring(39, 1));
                    character.DruidTalents.BrutalImpact = int.Parse(talentCode.Substring(40, 1));
                    character.DruidTalents.FeralCharge = int.Parse(talentCode.Substring(41, 1));
                    character.DruidTalents.NurturingInstinct = int.Parse(talentCode.Substring(42, 1));
                    character.DruidTalents.NaturalReaction = int.Parse(talentCode.Substring(43, 1));
                    character.DruidTalents.HeartOfTheWild = int.Parse(talentCode.Substring(44, 1));
                    character.DruidTalents.SurvivalOfTheFittest = int.Parse(talentCode.Substring(45, 1));
                    character.DruidTalents.LeaderOfThePack = int.Parse(talentCode.Substring(46, 1));
                    character.DruidTalents.ImprovedLeaderOfThePack = int.Parse(talentCode.Substring(47, 1));
                    character.DruidTalents.PrimalTenacity = int.Parse(talentCode.Substring(48, 1));
                    character.DruidTalents.MotherBear = int.Parse(talentCode.Substring(49, 1));
                    character.DruidTalents.PredatoryInstincts = int.Parse(talentCode.Substring(50, 1));
                    character.DruidTalents.InfectedWounds = int.Parse(talentCode.Substring(51, 1));
                    character.DruidTalents.KingOfTheJungle = int.Parse(talentCode.Substring(52, 1));
                    character.DruidTalents.Mangle = int.Parse(talentCode.Substring(53, 1));
                    character.DruidTalents.ImprovedMangle = int.Parse(talentCode.Substring(54, 1));
                    character.DruidTalents.RendAndTear = int.Parse(talentCode.Substring(55, 1));
                    character.DruidTalents.Berserk = int.Parse(talentCode.Substring(56, 1));
                    character.DruidTalents.ImprovedMarkOfTheWild = int.Parse(talentCode.Substring(57, 1));
                    character.DruidTalents.NaturesFocus = int.Parse(talentCode.Substring(58, 1));
                    character.DruidTalents.Furor = int.Parse(talentCode.Substring(59, 1));
                    character.DruidTalents.Naturalist = int.Parse(talentCode.Substring(60, 1));
                    character.DruidTalents.Subtlety = int.Parse(talentCode.Substring(61, 1));
                    character.DruidTalents.NaturalShapeshifter = int.Parse(talentCode.Substring(62, 1));
                    character.DruidTalents.Intensity = int.Parse(talentCode.Substring(63, 1));
                    character.DruidTalents.OmenOfClarity = int.Parse(talentCode.Substring(64, 1));
                    character.DruidTalents.MasterShapeshifter = int.Parse(talentCode.Substring(65, 1));
                    character.DruidTalents.TranquilSpirit = int.Parse(talentCode.Substring(66, 1));
                    character.DruidTalents.ImprovedRejuvenation = int.Parse(talentCode.Substring(67, 1));
                    character.DruidTalents.NaturesSwiftness = int.Parse(talentCode.Substring(68, 1));
                    character.DruidTalents.GiftOfNature = int.Parse(talentCode.Substring(69, 1));
                    character.DruidTalents.ImprovedTranquility = int.Parse(talentCode.Substring(70, 1));
                    character.DruidTalents.EmpoweredTouch = int.Parse(talentCode.Substring(71, 1));
                    character.DruidTalents.ImprovedRegrowth = int.Parse(talentCode.Substring(72, 1));
                    character.DruidTalents.LivingSpirit = int.Parse(talentCode.Substring(73, 1));
                    character.DruidTalents.Swiftmend = int.Parse(talentCode.Substring(74, 1));
                    character.DruidTalents.NaturalPerfection = int.Parse(talentCode.Substring(75, 1));
                    character.DruidTalents.EmpoweredRejuvenation = int.Parse(talentCode.Substring(76, 1));
                    character.DruidTalents.LivingSeed = int.Parse(talentCode.Substring(77, 1));
                    character.DruidTalents.Replenish = int.Parse(talentCode.Substring(78, 1));
                    character.DruidTalents.TreeOfLife = int.Parse(talentCode.Substring(79, 1));
                    character.DruidTalents.ImprovedTreeOfLife = int.Parse(talentCode.Substring(80, 1));
                    character.DruidTalents.GiftOfTheEarthmother = int.Parse(talentCode.Substring(81, 1));
                    character.DruidTalents.WildGrowth = int.Parse(talentCode.Substring(82, 1));
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
        }

		public int TargetLevel = 73;
		public float Latency = 0.2f;
		public float FightLength = 5;
		public bool Innervate = false;
		public float InnervateDelay = 1;
		public float ShadowPriest = 0;
		public bool ManaPots = false;
		public float ManaPotDelay = 1.5f;
		public string ManaPotType = "Super Mana Potion";
		public bool InnervateWeapon = false;
		public float InnervateWeaponInt = 0;
		public float InnervateWeaponSpi = 0;
		public string AldorScryer = "Aldor";
	}
}
