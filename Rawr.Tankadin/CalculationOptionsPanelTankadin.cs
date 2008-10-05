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
            nudPlayerLevel.Value = (decimal)calcOpts.PlayerLevel;
            nudTargetLevel.Value = (decimal)calcOpts.TargetLevel;
            trackBarBossAttackValue.Value = calcOpts.AverageHit;
            lblBossAttackValue.Text = calcOpts.AverageHit.ToString();
            trackBarMitigationScale.Value = calcOpts.MitigationScale;
            lblMitigationScaleValue.Text = calcOpts.MitigationScale.ToString();
            trackBarThreatScale.Value = calcOpts.ThreatScale;
            lblThreatScaleValue.Text = calcOpts.ThreatScale.ToString();
            _loadingCalculationOptions = false;

        }

        private void nudPlayerLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.PlayerLevel = (int)nudPlayerLevel.Value;
                Character.OnCalculationsInvalidated();
            }
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
                calcOpts.MitigationScale = trackBarMitigationScale.Value;
                lblMitigationScaleValue.Text = trackBarMitigationScale.Value.ToString();
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

        public bool EnforceMetagemRequirements = false;
        public int TargetLevel = 73;
        public int PlayerLevel = 70;
        public int AverageHit = 20000;
        public float AttackSpeed = 2;
        public int NumberAttackers = 1;
        public int TargetArmor = 6600;
        public int ThreatScale = 30;
        public int MitigationScale = 7000;
    }

}
