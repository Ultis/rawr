using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class CalculationOptionsPanelEnhance : CalculationOptionsPanelBase
    {
        private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

        public CalculationOptionsPanelEnhance()
        {
            InitializeComponent();
        }

        protected override void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsEnhance();

            CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
            comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
            trackBarTargetArmor.Value = calcOpts.TargetArmor;
            trackBarBloodlustUptime.Value = (int)Math.Round(calcOpts.BloodlustUptime * 100);
            cmbLength.Value = (decimal) calcOpts.FightLength;
            comboBoxMainhandImbue.SelectedItem = calcOpts.MainhandImbue;
            comboBoxOffhandImbue.SelectedItem = calcOpts.OffhandImbue;

            chbBaseStatOption.Checked = calcOpts.BaseStatOption;

            labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
            labelBloodlustUptime.Text = trackBarBloodlustUptime.Value.ToString() + "%";

            tbModuleNotes.Text = "The EnhSim export option exists for users that wish to have very detailed analysis of their stats. " +
                "For most users the standard model should be quite sufficient.\r\n\r\n" +
                "If you wish to use the EnhSim Simulator you will need to get the latest version from http://enhsim.wikidot.com\r\n\r\n" +
                "Once you have installed the simulator the easiest way to run it is to run EnhSimGUI and use the Clipboard copy functions.\r\n\r\n" +
                "Press the button above to copy your current Rawr.Enhance data to the clipboard then in EnhSimGUI click on the 'Import from Clipboard' " + 
                "button to replace the values in the EnhSimGUI with your Rawr values. Now all you need to do is click Simulate to get your results.\r\n\r\n" + 
                "Refer to the EnhSim website for more detailed instructions on how to use the sim and its various options";

            _loadingCalculationOptions = false;
        }

        private bool _loadingCalculationOptions = false;
        private void calculationOptionControl_Changed(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                trackBarTargetArmor.Value = 100 * (trackBarTargetArmor.Value / 100);
                labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
                labelBloodlustUptime.Text = trackBarBloodlustUptime.Value.ToString() + "%";

                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
                calcOpts.TargetArmor = trackBarTargetArmor.Value;
                calcOpts.FightLength = (float)cmbLength.Value;
                calcOpts.BloodlustUptime = (float)trackBarBloodlustUptime.Value / 100f;
                calcOpts.MainhandImbue = (string)comboBoxMainhandImbue.SelectedItem;
                calcOpts.OffhandImbue = (string)comboBoxOffhandImbue.SelectedItem;
                calcOpts.ShattrathFaction = radioButtonAldor.Checked ? "Aldor" : "Scryer";

                calcOpts.BaseStatOption = chbBaseStatOption.Checked;
                calcOpts.Magma = chbMagmaSearing.Checked;

                Character.OnCalculationsInvalidated();
            }
        }

        private void btnEnhSim_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void chbBaseStatOption_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.BaseStatOption = chbBaseStatOption.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbMagmaSearing_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.Magma = chbMagmaSearing.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        public void Export()
        {
            if (!_loadingCalculationOptions)
            {
                Enhance.EnhSim simExport = new Enhance.EnhSim(Character);
                simExport.copyToClipboard();
            }
        }

        private void trackBarBloodlustUptime_Scroll(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                labelBloodlustUptime.Text = trackBarBloodlustUptime.Value.ToString() + "%";
                calcOpts.BloodlustUptime = (float)trackBarBloodlustUptime.Value / 100f;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.FightLength = (float)cmbLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }
    }

	[Serializable]
	public class CalculationOptionsEnhance : ICalculationOptionBase
	{
        public bool BaseStatOption { get; set; }
        public bool Magma { get; set; }
       
        public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer = 
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsEnhance));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
		public int TargetArmor = 10645;
		public int NumberOfFerociousInspirations = 2;
		public float BloodlustUptime = 0.15f;
		public string ShattrathFaction = "Aldor";
        public string MainhandImbue = "Windfury";
        public string OffhandImbue = "Flametongue";
        public float FightLength = 5.0f;
        
    }
}
