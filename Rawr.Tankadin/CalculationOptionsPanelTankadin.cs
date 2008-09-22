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
            cmbTargetLevel.SelectedIndex = calcOpts.TargetLevel-70;
            nubAtkSpeed.Value = (decimal)calcOpts.AttackSpeed;
            nubAttackers.Value = (decimal)calcOpts.NumberAttackers;
            trackBarBossAttackValue.Value = calcOpts.AverageHit;
            trackBarMitigationScale.Value = calcOpts.MitigationScale;
            trackBarTargetArmor.Value = calcOpts.TargetArmor;
            trackBarThreatScale.Value = calcOpts.ThreatScale;
            labelBossAttackValue.Text = calcOpts.AverageHit.ToString();
            labelMitigationScale.Text = calcOpts.MitigationScale.ToString();
            labelTargetArmor.Text = calcOpts.TargetArmor.ToString();
            labelThreatScale.Text = calcOpts.ThreatScale.ToString();
            _loadingCalculationOptions = false;

        }

        private void cmbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.TargetLevel = int.Parse(cmbTargetLevel.SelectedItem.ToString());
                Character.OnCalculationsInvalidated();
            }
        }

        private void nubAttackers_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.NumberAttackers = (int)nubAttackers.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nubAtkSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.AttackSpeed = (float)nubAtkSpeed.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trackBarTargetArmor_Scroll(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.TargetArmor = trackBarTargetArmor.Value;
                labelTargetArmor.Text = trackBarTargetArmor.Value.ToString();
                Character.OnCalculationsInvalidated();
            }
        }

        private void trackBarThreatScale_Scroll(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.ThreatScale = trackBarThreatScale.Value;
                labelThreatScale.Text = trackBarThreatScale.Value.ToString();
                Character.OnCalculationsInvalidated();
            }
        }

        private void trackBarMitigationScale_Scroll(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.MitigationScale = trackBarMitigationScale.Value;
                labelMitigationScale.Text = trackBarMitigationScale.Value.ToString();
                Character.OnCalculationsInvalidated();
            }
        }

        private void trackBarBossAttackValue_Scroll(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsTankadin calcOpts = Character.CalculationOptions as CalculationOptionsTankadin;
                calcOpts.AverageHit = trackBarBossAttackValue.Value;
                labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
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
        public int AverageHit = 20000;
        public float AttackSpeed = 2;
        public int NumberAttackers = 1;
        public int TargetArmor = 6600;
        public int ThreatScale = 100;
        public int MitigationScale = 4000;
    }

}
