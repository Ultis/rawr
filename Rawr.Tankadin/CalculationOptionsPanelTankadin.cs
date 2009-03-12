using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Tankadin
{
    public partial class CalculationOptionsPanelTankadin  : CalculationOptionsPanelBase
	{

        private bool _loadingCalculationOptions;

		public CalculationOptionsPanelTankadin()
		{
			InitializeComponent();
		}

        protected override void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsTankadin();

            CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
            nudTargetLevel.Value = (decimal)calcOpts.TargetLevel;
            trackBarBossAttackValue.Value = calcOpts.AverageHit;
            lblBossAttackValue.Text = calcOpts.AverageHit.ToString();
            trackBarMitigationScale.Value = calcOpts.MitigationScalePercent;
            lblMitigationScaleValue.Text = calcOpts.MitigationScalePercent.ToString() + "%";
            trackBarThreatScale.Value = calcOpts.ThreatScale;
            lblThreatScaleValue.Text = calcOpts.ThreatScale.ToString();
            _loadingCalculationOptions = false;
            
            // Seal Choice
            radioButtonSoR.Checked = (calcOpts.ThreatRotationChoice == 1);
            radioButtonSoV.Checked = (calcOpts.ThreatRotationChoice == 2);

            // Glyphs
            checkBoxGlyphOfJudgement.Checked = calcOpts.GlyphJudgement;
            checkBoxGlyphOfSealOfVengeance.Checked = calcOpts.GlyphSealVengeance;
        }

        private void nudTargetLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.TargetLevel = (int)nudTargetLevel.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trackBarBossAttackValue_Scroll(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.AverageHit = trackBarBossAttackValue.Value;
                lblBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
                Character.OnCalculationsInvalidated();
            }
        }

        private void trackBarThreatScale_Scroll(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.ThreatScale = trackBarThreatScale.Value;
                lblThreatScaleValue.Text = trackBarThreatScale.Value.ToString();
                Character.OnCalculationsInvalidated();
            }
        }

        private void trackBarMitigationScale_Scroll(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.MitigationScalePercent = trackBarMitigationScale.Value;
                lblMitigationScaleValue.Text = trackBarMitigationScale.Value.ToString() + "%";
                Character.OnCalculationsInvalidated();
            }
        }


        
        void RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                if (radioButtonSoR.Checked)
                {
                	calcOpts.ThreatRotationChoice = 2;
                }
                else
                {
                	calcOpts.ThreatRotationChoice = 1;
                }
                Character.OnCalculationsInvalidated();
            }
        }
        
        void CheckBoxGlyphOfJudgementCheckedChanged(object sender, EventArgs e)
        {
        	if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.GlyphJudgement = checkBoxGlyphOfJudgement.Checked;
                Character.OnCalculationsInvalidated();
            }
        }
        
        void CheckBoxGlyphOfSealOfVengeanceCheckedChanged(object sender, EventArgs e)
        {
        	if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.GlyphSealVengeance = checkBoxGlyphOfSealOfVengeance.Checked;
                Character.OnCalculationsInvalidated();
            }        	
        }
    }

    [Serializable]
    public class CalculationOptionsTankadin : ICalculationOptionBase
    {
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsTankadin));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }

        public int TargetLevel = 83;
        public int AverageHit = 30000;
        public float AttackSpeed = 2;
        public int NumberAttackers = 1;
        public int TargetArmor = 10000;
        public int ThreatScale = 10;
        public int MitigationScalePercent = 100;
        public int ThreatRotationChoice = 1;
        public bool GlyphSealVengeance = false;
        public bool GlyphJudgement = false;
    }

}
