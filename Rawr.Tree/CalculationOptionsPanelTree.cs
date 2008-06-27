using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace Rawr.Tree
{
    public partial class CalculationOptionsPanelTree : CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelTree()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsTree(Character);

            CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
            cmbLength.Value = (decimal)calcOpts.Length;
            cmbManaAmt.Text = calcOpts.ManaAmt.ToString();
            cmbManaTime.Value = (decimal)calcOpts.ManaTime;
            cmbSpriest.Value = (decimal)calcOpts.Spriest;
            upDownInnervate.Value = (decimal) calcOpts.InnervateDelay;

            cmbIntensity.Text = calcOpts.Intensity.ToString();
            cmbLivingSpirit.Text = calcOpts.LivingSpirit.ToString();
            cmbNaturalPerfection.Text = calcOpts.NaturalPerfection.ToString();
            cmbImprovedRejuvenation.Text = calcOpts.ImprovedRejuvenation.ToString();
            cmbEmpoweredRejuvenation.Text = calcOpts.EmpoweredRejuvenation.ToString();
            cmbGiftOfNature.Text = calcOpts.GiftOfNature.ToString();
            cmbImprovedRegrowth.Text = calcOpts.ImprovedRegrowth.ToString();
            cmbTreeOfLife.Text = calcOpts.TreeOfLife.ToString();

            trkMinHealth.Value = (int)calcOpts.TargetHealth;
            lblMinHealth.Text = trkMinHealth.Value.ToString();
            upDownSurvScalingAbove.Value = (decimal) calcOpts.SurvScalingAbove;
            upDownSurvScalingBelow.Value = (decimal)calcOpts.SurvScalingBelow;

            loading = false;
        }

        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.Length = (float)cmbLength.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbManaTime_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.ManaTime = (float)cmbManaTime.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbManaAmt_TextUpdate(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbSpriest_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.Spriest = (float)cmbSpriest.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbIntensity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.Intensity = int.Parse(cmbIntensity.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbLivingSpirit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.LivingSpirit = int.Parse(cmbLivingSpirit.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbNaturalPerfection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.NaturalPerfection = int.Parse(cmbNaturalPerfection.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbImprovedRejuvenation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.ImprovedRejuvenation = int.Parse(cmbImprovedRejuvenation.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbImprovedRegrowth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.ImprovedRegrowth = int.Parse(cmbImprovedRegrowth.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbGiftOfNature_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.GiftOfNature = int.Parse(cmbGiftOfNature.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbEmpoweredRejuvenation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.EmpoweredRejuvenation = int.Parse(cmbEmpoweredRejuvenation.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void cmbTreeOfLife_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                try
                {
                    calcOpts.TreeOfLife = int.Parse(cmbTreeOfLife.Text);
                }
                catch { }
                Character.OnItemsChanged();
            }
        }

        private void upDownSurvScalingAbove_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.SurvScalingAbove = (float)upDownSurvScalingAbove.Value;
                Character.OnItemsChanged();
            }
        }

        private void upDownSurvScalingBelow_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.SurvScalingBelow = (float)upDownSurvScalingBelow.Value;
                Character.OnItemsChanged();
            }
        }

        private void upDownInnervate_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                calcOpts.InnervateDelay = (float)upDownInnervate.Value;
                Character.OnItemsChanged();
            }
        }

        private void trkMinHealth_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsTree calcOpts = Character.CalculationOptions as CalculationOptionsTree;
                lblMinHealth.Text = trkMinHealth.Value.ToString();
                calcOpts.TargetHealth = trkMinHealth.Value;
                Character.OnItemsChanged();
            }
        }
    }
    [Serializable]
    public class CalculationOptionsTree : ICalculationOptionBase
    {
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTree));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public bool EnforceMetagemRequirements = false;
        public float Length = 5;
        public float ManaAmt = 2400;
        public float ManaTime = 2.5f;
        public float TargetHealth = 8750;
        public float SurvScalingAbove = 500;
        public float SurvScalingBelow = 20;
        public float Spriest = 0;
        public float InnervateDelay = 6.5f;

        public CalculationOptionsTree()
        {
        }

        public CalculationOptionsTree(Character character)
        {
            #region Druid Talents Import
            try
            {
                WebRequestWrapper wrw = new WebRequestWrapper();
                if (character.Class == Character.CharacterClass.Druid && character.Name != null && character.Realm != null)
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
                    OmenOfClarity = int.Parse(talentCode.Substring(49, 1));
                    TranquilSpirit = int.Parse(talentCode.Substring(50, 1));
                    ImprovedRejuvenation = int.Parse(talentCode.Substring(51, 1));
                    NaturesSwiftness = int.Parse(talentCode.Substring(52, 1));
                    GiftOfNature = int.Parse(talentCode.Substring(53, 1));
                    ImpTranquility = int.Parse(talentCode.Substring(54, 1));
                    EmpoweredTouch = int.Parse(talentCode.Substring(55, 1));
                    ImprovedRegrowth = int.Parse(talentCode.Substring(56, 1));
                    LivingSpirit = int.Parse(talentCode.Substring(57, 1));
                    Swiftmend = int.Parse(talentCode.Substring(58, 1));
                    NaturalPerfection = int.Parse(talentCode.Substring(59, 1));
                    EmpoweredRejuvenation = int.Parse(talentCode.Substring(60, 1));
                    TreeOfLife = int.Parse(talentCode.Substring(61, 1));
                }
            }
            catch
            {
            }
            #endregion
        }

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
		public int TreeOfLife;
		public int ImprovedMotW;
		public int EmpoweredRejuvenation;
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
		public int OmenOfClarity;
		public int GiftOfNature;
		public int TranquilSpirit;
		public int NaturesSwiftness;
		public int ImprovedRejuvenation;
    }
}
