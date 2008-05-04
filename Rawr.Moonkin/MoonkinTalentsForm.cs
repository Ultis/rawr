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
    public partial class MoonkinTalentsForm : Form
    {
        private CalculationOptionsPanelMoonkin basePanel;
        public MoonkinTalentsForm(CalculationOptionsPanelMoonkin basePanel)
        {
            this.basePanel = basePanel;
            InitializeComponent();
        }

        public Character Character
        {
            get
            {
                return basePanel.Character;
            }
        }

        private void MoonkinTalentsForm_Load(object sender, EventArgs e)
        {
            LoadCalculationOptions();
        }

        // Load talent points from a character's calculation options.
        public void LoadCalculationOptions()
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			cmbStarlightWrath.SelectedItem = calcOpts.StarlightWrath.ToString();
			cmbForceofNature.SelectedItem = calcOpts.ForceofNature.ToString();
			cmbWrathofCenarius.SelectedItem = calcOpts.WrathofCenarius.ToString();
			cmbImprovedFF.SelectedItem = calcOpts.ImprovedFF.ToString();
			cmbMoonkinForm.SelectedItem = calcOpts.MoonkinForm.ToString();
			cmbDreamstate.SelectedItem = calcOpts.Dreamstate.ToString();
			cmbBalanceofPower.SelectedItem = calcOpts.BalanceofPower.ToString();
			cmbMoonfury.SelectedItem = calcOpts.Moonfury.ToString();
			cmbMoonglow.SelectedItem = calcOpts.Moonglow.ToString();
			cmbNaturesGrace.SelectedItem = calcOpts.NaturesGrace.ToString();
			cmbLunarGuidance.SelectedItem = calcOpts.LunarGuidance.ToString();
			cmbCelestialFocus.SelectedItem = calcOpts.CelestialFocus.ToString();
			cmbVengeance.SelectedItem = calcOpts.Vengeance.ToString();
			cmbNaturesReach.SelectedItem = calcOpts.NaturesReach.ToString();
			cmbInsectSwarm.SelectedItem = calcOpts.InsectSwarm.ToString();
			cmbBrambles.SelectedItem = calcOpts.Brambles.ToString();
			cmbImpMoonfire.SelectedItem = calcOpts.ImpMoonfire.ToString();
			cmbFocusedStarlight.SelectedItem = calcOpts.FocusedStarlight.ToString();
			cmbControlofNature.SelectedItem = calcOpts.ControlofNature.ToString();
			cmbImpNaturesGrasp.SelectedItem = calcOpts.ImpNaturesGrasp.ToString();
			cmbNaturesGrasp.SelectedItem = calcOpts.NaturesGrasp.ToString();
			cmbFerocity.SelectedItem = calcOpts.Ferocity.ToString();
			cmbFeralAggression.SelectedItem = calcOpts.FeralAggression.ToString();
			cmbFeralInstinct.SelectedItem = calcOpts.FeralInstinct.ToString();
			cmbBrutalImpact.SelectedItem = calcOpts.BrutalImpact.ToString();
			cmbThickHide.SelectedItem = calcOpts.ThickHide.ToString();
			cmbFeralSwiftness.SelectedItem = calcOpts.FeralSwiftness.ToString();
			cmbFeralCharge.SelectedItem = calcOpts.FeralCharge.ToString();
			cmbSharpenedClaws.SelectedItem = calcOpts.SharpenedClaws.ToString();
			cmbShreddingAttacks.SelectedItem = calcOpts.ShreddingAttacks.ToString();
			cmbPredatoryStrikes.SelectedItem = calcOpts.PredatoryStrikes.ToString();
			cmbPrimalFury.SelectedItem = calcOpts.PrimalFury.ToString();
			cmbSavageFury.SelectedItem = calcOpts.SavageFury.ToString();
			cmbFeralFaerieFire.SelectedItem = calcOpts.FeralFaerieFire.ToString();
			cmbNurturingInstinct.SelectedItem = calcOpts.NurturingInstinct.ToString();
			cmbHotW.SelectedItem = calcOpts.HotW.ToString();
			cmbSotF.SelectedItem = calcOpts.SotF.ToString();
			cmbPrimalTenacity.SelectedItem = calcOpts.PrimalTenacity.ToString();
			cmbLotP.SelectedItem = calcOpts.LotP.ToString();
			cmbImprovedLotP.SelectedItem = calcOpts.ImprovedLotP.ToString();
			cmbMangle.SelectedItem = calcOpts.Mangle.ToString();
			cmbPredatoryInstincts.SelectedItem = calcOpts.PredatoryInstincts.ToString();
			cmbTreeofLife.SelectedItem = calcOpts.TreeofLife.ToString();
			cmbImprovedMotW.SelectedItem = calcOpts.ImprovedMotW.ToString();
			cmbEmpoweredRejuv.SelectedItem = calcOpts.EmpoweredRejuv.ToString();
			cmbFuror.SelectedItem = calcOpts.Furor.ToString();
			cmbNaturalPerfection.SelectedItem = calcOpts.NaturalPerfection.ToString();
			cmbNaturalist.SelectedItem = calcOpts.Naturalist.ToString();
			cmbSwiftmend.SelectedItem = calcOpts.Swiftmend.ToString();
			cmbNaturesFocus.SelectedItem = calcOpts.NaturesFocus.ToString();
			cmbLivingSpirit.SelectedItem = calcOpts.LivingSpirit.ToString();
			cmbNaturalShapeshifter.SelectedItem = calcOpts.NaturalShapeshifter.ToString();
			cmbImprovedRegrowth.SelectedItem = calcOpts.ImprovedRegrowth.ToString();
			cmbIntensity.SelectedItem = calcOpts.Intensity.ToString();
			cmbEmpoweredTouch.SelectedItem = calcOpts.EmpoweredTouch.ToString();
			cmbSubtlety.SelectedItem = calcOpts.Subtlety.ToString();
			cmbImpTranquility.SelectedItem = calcOpts.ImpTranquility.ToString();
			cmbOmenofClarity.SelectedItem = calcOpts.OmenofClarity.ToString();
			cmbGiftofNature.SelectedItem = calcOpts.GiftofNature.ToString();
			cmbTranquilSpirit.SelectedItem = calcOpts.TranquilSpirit.ToString();
			cmbNaturesSwiftness.SelectedItem = calcOpts.NaturesSwiftness.ToString();
			cmbImprovedRejuv.SelectedItem = calcOpts.ImprovedRejuv.ToString();



			//// Iterate through all controls on the form
			//foreach (Control c in Controls)
			//{
			//    // Iterate into group boxes only
			//    if (c is GroupBox)
			//    {
			//        // Iterate through all controls in the group box
			//        foreach (Control innerControl in c.Controls)
			//        {
			//            // Load calculation options into combo boxes only
			//            if (innerControl is ComboBox)
			//            {
			//                // Get the substring that is the actual talent name
			//                ComboBox cb = (ComboBox)innerControl;
			//                string talent = cb.Name.Substring(3);

			//                // If the talent is not in the calculation options, add it
			//                if (!Character.CalculationOptions.ContainsKey(talent))
			//                    Character.CalculationOptions[talent] = "0";

			//                // Load the value from the character into the combo box
			//                cb.SelectedItem = Character.CalculationOptions[talent];
			//            }
			//        }
			//    }
			//}
        }

        // Update character calculation options when a talent point is set
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsMoonkin calcOpts = Character.CalculationOptions as CalculationOptionsMoonkin;
			calcOpts.StarlightWrath = cmbStarlightWrath.SelectedIndex;
			calcOpts.ForceofNature = cmbForceofNature.SelectedIndex;
			calcOpts.WrathofCenarius = cmbWrathofCenarius.SelectedIndex;
			calcOpts.ImprovedFF = cmbImprovedFF.SelectedIndex;
			calcOpts.MoonkinForm = cmbMoonkinForm.SelectedIndex;
			calcOpts.Dreamstate = cmbDreamstate.SelectedIndex;
			calcOpts.BalanceofPower = cmbBalanceofPower.SelectedIndex;
			calcOpts.Moonfury = cmbMoonfury.SelectedIndex;
			calcOpts.Moonglow = cmbMoonglow.SelectedIndex;
			calcOpts.NaturesGrace = cmbNaturesGrace.SelectedIndex;
			calcOpts.LunarGuidance = cmbLunarGuidance.SelectedIndex;
			calcOpts.CelestialFocus = cmbCelestialFocus.SelectedIndex;
			calcOpts.Vengeance = cmbVengeance.SelectedIndex;
			calcOpts.NaturesReach = cmbNaturesReach.SelectedIndex;
			calcOpts.InsectSwarm = cmbInsectSwarm.SelectedIndex;
			calcOpts.Brambles = cmbBrambles.SelectedIndex;
			calcOpts.ImpMoonfire = cmbImpMoonfire.SelectedIndex;
			calcOpts.FocusedStarlight = cmbFocusedStarlight.SelectedIndex;
			calcOpts.ControlofNature = cmbControlofNature.SelectedIndex;
			calcOpts.ImpNaturesGrasp = cmbImpNaturesGrasp.SelectedIndex;
			calcOpts.NaturesGrasp = cmbNaturesGrasp.SelectedIndex;
			calcOpts.Ferocity = cmbFerocity.SelectedIndex;
			calcOpts.FeralAggression = cmbFeralAggression.SelectedIndex;
			calcOpts.FeralInstinct = cmbFeralInstinct.SelectedIndex;
			calcOpts.BrutalImpact = cmbBrutalImpact.SelectedIndex;
			calcOpts.ThickHide = cmbThickHide.SelectedIndex;
			calcOpts.FeralSwiftness = cmbFeralSwiftness.SelectedIndex;
			calcOpts.FeralCharge = cmbFeralCharge.SelectedIndex;
			calcOpts.SharpenedClaws = cmbSharpenedClaws.SelectedIndex;
			calcOpts.ShreddingAttacks = cmbShreddingAttacks.SelectedIndex;
			calcOpts.PredatoryStrikes = cmbPredatoryStrikes.SelectedIndex;
			calcOpts.PrimalFury = cmbPrimalFury.SelectedIndex;
			calcOpts.SavageFury = cmbSavageFury.SelectedIndex;
			calcOpts.FeralFaerieFire = cmbFeralFaerieFire.SelectedIndex;
			calcOpts.NurturingInstinct = cmbNurturingInstinct.SelectedIndex;
			calcOpts.HotW = cmbHotW.SelectedIndex;
			calcOpts.SotF = cmbSotF.SelectedIndex;
			calcOpts.PrimalTenacity = cmbPrimalTenacity.SelectedIndex;
			calcOpts.LotP = cmbLotP.SelectedIndex;
			calcOpts.ImprovedLotP = cmbImprovedLotP.SelectedIndex;
			calcOpts.Mangle = cmbMangle.SelectedIndex;
			calcOpts.PredatoryInstincts = cmbPredatoryInstincts.SelectedIndex;
			calcOpts.TreeofLife = cmbTreeofLife.SelectedIndex;
			calcOpts.ImprovedMotW = cmbImprovedMotW.SelectedIndex;
			calcOpts.EmpoweredRejuv = cmbEmpoweredRejuv.SelectedIndex;
			calcOpts.Furor = cmbFuror.SelectedIndex;
			calcOpts.NaturalPerfection = cmbNaturalPerfection.SelectedIndex;
			calcOpts.Naturalist = cmbNaturalist.SelectedIndex;
			calcOpts.Swiftmend = cmbSwiftmend.SelectedIndex;
			calcOpts.NaturesFocus = cmbNaturesFocus.SelectedIndex;
			calcOpts.LivingSpirit = cmbLivingSpirit.SelectedIndex;
			calcOpts.NaturalShapeshifter = cmbNaturalShapeshifter.SelectedIndex;
			calcOpts.ImprovedRegrowth = cmbImprovedRegrowth.SelectedIndex;
			calcOpts.Intensity = cmbIntensity.SelectedIndex;
			calcOpts.EmpoweredTouch = cmbEmpoweredTouch.SelectedIndex;
			calcOpts.Subtlety = cmbSubtlety.SelectedIndex;
			calcOpts.ImpTranquility = cmbImpTranquility.SelectedIndex;
			calcOpts.OmenofClarity = cmbOmenofClarity.SelectedIndex;
			calcOpts.GiftofNature = cmbGiftofNature.SelectedIndex;
			calcOpts.TranquilSpirit = cmbTranquilSpirit.SelectedIndex;
			calcOpts.NaturesSwiftness = cmbNaturesSwiftness.SelectedIndex;
			calcOpts.ImprovedRejuv = cmbImprovedRejuv.SelectedIndex;

			//ComboBox cb = (ComboBox)sender;
			//string talent = cb.Name.Substring(3);
			//Character.CalculationOptions[talent] = cb.SelectedItem.ToString();
			Character.OnItemsChanged();
        }

        // Do not close the form on close; merely hide it
        private void MoonkinTalentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Visible = false;
            }
        }
    }
}
