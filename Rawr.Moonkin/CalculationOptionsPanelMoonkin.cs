using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
			if (Character.CurrentCalculationOptions == null)
				Character.CurrentCalculationOptions = new CalculationOptionsMoonkin();
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

			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
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
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.TargetLevel = int.Parse(cmbTargetLevel.SelectedItem.ToString());
            Character.OnItemsChanged();
        }

        private void txtLatency_TextChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.Latency = float.Parse(txtLatency.Text);
            Character.OnItemsChanged();
        }

        private void chkMetagem_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			Character.EnforceMetagemRequirements = chkMetagem.Checked;
            Character.OnItemsChanged();
        }

        private void txtFightLength_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.FightLength = float.Parse(txtFightLength.Text);
            Character.OnItemsChanged();
        }

        private void chkInnervate_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.Innervate = chkInnervate.Checked;
            txtInnervateDelay.Enabled = chkInnervate.Checked;
            Character.OnItemsChanged();
        }

        private void txtShadowPriest_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.ShadowPriest = float.Parse(txtShadowPriest.Text);
            Character.OnItemsChanged();
        }

        private void chkManaPots_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.ManaPots = chkManaPots.Checked;
            cmbPotType.Enabled = chkManaPots.Checked;
            txtManaPotDelay.Enabled = chkManaPots.Checked;
            Character.OnItemsChanged();
        }

        private void cmbPotType_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.ManaPotType = cmbPotType.SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void txtInnervateDelay_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.InnervateDelay = float.Parse(txtInnervateDelay.Text);
            Character.OnItemsChanged();
        }

        private void txtManaPotDelay_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.ManaPotDelay = float.Parse(txtManaPotDelay.Text);
            Character.OnItemsChanged();
        }

        private void txtInnervateWeaponInt_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.InnervateWeaponInt = float.Parse(txtInnervateWeaponInt.Text);
            Character.OnItemsChanged();
        }

        private void txtInnervateWeaponSpi_Leave(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.InnervateWeaponSpi = float.Parse(txtInnervateWeaponSpi.Text);
            Character.OnItemsChanged();
        }

        private void chkInnervateWeapon_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.InnervateWeapon = chkInnervateWeapon.Checked;
            txtInnervateWeaponInt.Enabled = chkInnervateWeapon.Checked;
            txtInnervateWeaponSpi.Enabled = chkInnervateWeapon.Checked;
            Character.OnItemsChanged();
        }

        private void rdbScryer_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CurrentCalculationOptions as CalculationOptionsMoonkin;
			calcOpts.AldorScryer = rdbScryer.Checked ? "Scryer" : "Aldor";
            Character.OnItemsChanged();
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
