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
        private MoonkinTalentsForm talents;
        public CalculationOptionsPanelMoonkin()
        {
            talents = new MoonkinTalentsForm(this);
            InitializeComponent();
        }

        // Display the talents form when the button is clicked
        private void btnTalents_Click(object sender, EventArgs e)
        {
            talents.Show();
        }

        // Load the options into the form
        protected override void LoadCalculationOptions()
        {
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsMoonkin(Character);
			//if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
			//    calcOpts.TargetLevel = "73";
			//if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
			//    character.EnforceMetagemRequirements = "No";
			//if (!Character.CalculationOptions.ContainsKey("Latency"))
			//    calcOpts.Latency = "0.4";
			//if (!Character.CalculationOptions.ContainsKey("FightLength"))
			//    calcOpts.FightLength = "5";
			//if (!Character.CalculationOptions.ContainsKey("Innervate"))
			//    calcOpts.Innervate = "No";
			//if (!Character.CalculationOptions.ContainsKey("InnervateDelay"))
			//    calcOpts.InnervateDelay = "1";
			//if (!Character.CalculationOptions.ContainsKey("ShadowPriest"))
			//    calcOpts.ShadowPriest = "0";
			//if (!Character.CalculationOptions.ContainsKey("ManaPots"))
			//    calcOpts.ManaPots = "No";
			//if (!Character.CalculationOptions.ContainsKey("ManaPotDelay"))
			//    calcOpts.ManaPotDelay = "1.5";
			//if (!Character.CalculationOptions.ContainsKey("ManaPotType"))
			//    calcOpts.ManaPotType = "Super Mana Potion";
			//if (!Character.CalculationOptions.ContainsKey("InnervateWeapon"))
			//    calcOpts.InnervateWeapon = "No";
			//if (!Character.CalculationOptions.ContainsKey("InnervateWeaponInt"))
			//    calcOpts.InnervateWeaponInt = "0";
			//if (!Character.CalculationOptions.ContainsKey("InnervateWeaponSpi"))
			//    calcOpts.InnervateWeaponSpi = "0";
			//if (!Character.CalculationOptions.ContainsKey("AldorScryer"))
			//    calcOpts.AldorScryer = "Aldor";

			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
            cmbTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
			chkMetagem.Checked = Character.EnforceMetagemRequirements;
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

            if (talents != null) talents.LoadCalculationOptions();
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

        private void chkMetagem_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			Character.EnforceMetagemRequirements = chkMetagem.Checked;
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
                    StarlightWrath = int.Parse(talentCode.Substring(0, 1));
                    NaturesGrasp = int.Parse(talentCode.Substring(1, 1));
                    ImpNaturesGrasp = int.Parse(talentCode.Substring(2, 1));
                    ControlofNature = int.Parse(talentCode.Substring(3, 1));
                    FocusedStarlight = int.Parse(talentCode.Substring(4, 1));
                    ImpMoonfire = int.Parse(talentCode.Substring(5, 1));
                    Brambles = int.Parse(talentCode.Substring(6, 1));
                    InsectSwarm = int.Parse(talentCode.Substring(7, 1));
                    NaturesReach = int.Parse(talentCode.Substring(8, 1));
                    Vengeance = int.Parse(talentCode.Substring(9, 1));
                    CelestialFocus = int.Parse(talentCode.Substring(10, 1));
                    LunarGuidance = int.Parse(talentCode.Substring(11, 1));
                    NaturesGrace = int.Parse(talentCode.Substring(12, 1));
                    Moonglow = int.Parse(talentCode.Substring(13, 1));
                    Moonfury = int.Parse(talentCode.Substring(14, 1));
                    BalanceofPower = int.Parse(talentCode.Substring(15, 1));
                    Dreamstate = int.Parse(talentCode.Substring(16, 1));
                    MoonkinForm = int.Parse(talentCode.Substring(17, 1));
                    ImprovedFF = int.Parse(talentCode.Substring(18, 1));
                    WrathofCenarius = int.Parse(talentCode.Substring(19, 1));
                    ForceofNature = int.Parse(talentCode.Substring(20, 1));
                    Ferocity = int.Parse(talentCode.Substring(21, 1));
                    FeralAggression = int.Parse(talentCode.Substring(22, 1));
                    FeralInstinct = int.Parse(talentCode.Substring(23, 1));
                    BrutalImpact = int.Parse(talentCode.Substring(24, 1));
                    ThickHide = int.Parse(talentCode.Substring(25, 1));
                    FeralSwiftness = int.Parse(talentCode.Substring(26, 1));
                    FeralCharge = int.Parse(talentCode.Substring(27, 1));
                    SharpenedClaws = int.Parse(talentCode.Substring(28, 1));
                    ShreddingAttacks = int.Parse(talentCode.Substring(29, 1));
                    PredatoryStrikes = int.Parse(talentCode.Substring(30, 1));
                    PrimalFury = int.Parse(talentCode.Substring(31, 1));
                    SavageFury = int.Parse(talentCode.Substring(32, 1));
                    FeralFaerieFire = int.Parse(talentCode.Substring(33, 1));
                    NurturingInstinct = int.Parse(talentCode.Substring(34, 1));
                    HotW = int.Parse(talentCode.Substring(35, 1));
                    SotF = int.Parse(talentCode.Substring(36, 1));
                    PrimalTenacity = int.Parse(talentCode.Substring(37, 1));
                    LotP = int.Parse(talentCode.Substring(38, 1));
                    ImprovedLotP = int.Parse(talentCode.Substring(39, 1));
                    PredatoryInstincts = int.Parse(talentCode.Substring(40, 1));
                    Mangle = int.Parse(talentCode.Substring(41, 1));
                    ImprovedMotW = int.Parse(talentCode.Substring(42, 1));
                    Furor = int.Parse(talentCode.Substring(43, 1));
                    Naturalist = int.Parse(talentCode.Substring(44, 1));
                    NaturesFocus = int.Parse(talentCode.Substring(45, 1));
                    NaturalShapeshifter = int.Parse(talentCode.Substring(46, 1));
                    Intensity = int.Parse(talentCode.Substring(47, 1));
                    Subtlety = int.Parse(talentCode.Substring(48, 1));
                    OmenofClarity = int.Parse(talentCode.Substring(49, 1));
                    TranquilSpirit = int.Parse(talentCode.Substring(50, 1));
                    ImprovedRejuv = int.Parse(talentCode.Substring(51, 1));
                    NaturesSwiftness = int.Parse(talentCode.Substring(52, 1));
                    GiftofNature = int.Parse(talentCode.Substring(53, 1));
                    ImpTranquility = int.Parse(talentCode.Substring(54, 1));
                    EmpoweredTouch = int.Parse(talentCode.Substring(55, 1));
                    ImprovedRegrowth = int.Parse(talentCode.Substring(56, 1));
                    LivingSpirit = int.Parse(talentCode.Substring(57, 1));
                    Swiftmend = int.Parse(talentCode.Substring(58, 1));
                    NaturalPerfection = int.Parse(talentCode.Substring(59, 1));
                    EmpoweredRejuv = int.Parse(talentCode.Substring(60, 1));
                    TreeofLife = int.Parse(talentCode.Substring(61, 1));
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
        }

		public int TargetLevel = 73;
		public bool EnforceMetagemRequirements = false;
		public float Latency = 0.4f;
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

		public int StarlightWrath;
		public int ForceofNature;
		public int WrathofCenarius;
		public int ImprovedFF;
		public int MoonkinForm;
		public int Dreamstate;
		public int BalanceofPower;
		public int Moonfury;
		public int Moonglow;
		public int NaturesGrace;
		public int LunarGuidance;
		public int CelestialFocus;
		public int Vengeance;
		public int NaturesReach;
		public int InsectSwarm;
		public int Brambles;
		public int ImpMoonfire;
		public int FocusedStarlight;
		public int ControlofNature;
		public int ImpNaturesGrasp;
		public int NaturesGrasp;
		public int Ferocity;
		public int FeralAggression;
		public int FeralInstinct;
		public int BrutalImpact;
		public int ThickHide;
		public int FeralSwiftness;
		public int FeralCharge;
		public int SharpenedClaws;
		public int ShreddingAttacks;
		public int PredatoryStrikes;
		public int PrimalFury;
		public int SavageFury;
		public int FeralFaerieFire;
		public int NurturingInstinct;
		public int HotW;
		public int SotF;
		public int PrimalTenacity;
		public int LotP;
		public int ImprovedLotP;
		public int Mangle;
		public int PredatoryInstincts;
		public int TreeofLife;
		public int ImprovedMotW;
		public int EmpoweredRejuv;
		public int Furor;
		public int NaturalPerfection;
		public int Naturalist;
		public int Swiftmend;
		public int NaturesFocus;
		public int LivingSpirit;
		public int NaturalShapeshifter;
		public int ImprovedRegrowth;
		public int Intensity;
		public int EmpoweredTouch;
		public int Subtlety;
		public int ImpTranquility;
		public int OmenofClarity;
		public int GiftofNature;
		public int TranquilSpirit;
		public int NaturesSwiftness;
		public int ImprovedRejuv;
	}
}
