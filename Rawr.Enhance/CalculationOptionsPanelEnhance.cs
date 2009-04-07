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
            trackBarNumberOfFerociousInspirations.Value = calcOpts.NumberOfFerociousInspirations;
            trackBarBloodlustUptime.Value = (int)Math.Round(calcOpts.BloodlustUptime * 100);
            comboBoxMainhandImbue.SelectedItem = calcOpts.MainhandImbue;
            comboBoxOffhandImbue.SelectedItem = calcOpts.OffhandImbue;

            chbBaseStatOption.Checked = calcOpts.BaseStatOption;

            labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
            labelNumberOfFerociousInspirations.Text = trackBarNumberOfFerociousInspirations.Value.ToString();
            labelBloodlustUptime.Text = trackBarBloodlustUptime.Value.ToString() + "%";

            tbModuleNotes.Text = "The EnhSim export option exists for users that wish to have very detailed analysis of their stats. " +
                "For most users the standard model should be quite sufficient.\r\n\r\n" +
                "If you wish to use the EnhSim Simulator you will need to get the latest version from http://enhsim.wikidot.com\r\n\r\n" +
                "Once you have installed the simulator to run it you need to edit the config file in a decent text editor (ie: NOT notepad).\r\n\r\n" +
                "With the config file open in your text editor scroll down to the last section that says you can replace everything below with " +
                "your paper doll stats. Delete that section in the config file.\r\n\r\nNow press the button above to copy your " +
                "current Rawr.Enhance data to the clipboard then in your text editor, paste the config values into the end of the file.\r\n\r\n" +
                "If you now save that config file eg: as a CharacterName.txt file you are then ready to run the sim executable or the sim gui." +
                "Remember you must select the config file you just saved as the one you want to use.\r\n\r\nRefer to the EnhSim website for " +
                "instructions on how to use the sim and its options";

            _loadingCalculationOptions = false;
        }

        private bool _loadingCalculationOptions = false;
        private void calculationOptionControl_Changed(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                trackBarTargetArmor.Value = 100 * (trackBarTargetArmor.Value / 100);
                labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
                labelNumberOfFerociousInspirations.Text = trackBarNumberOfFerociousInspirations.Value.ToString();
                labelBloodlustUptime.Text = trackBarBloodlustUptime.Value.ToString() + "%";

                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
                calcOpts.TargetArmor = trackBarTargetArmor.Value;
                calcOpts.NumberOfFerociousInspirations = trackBarNumberOfFerociousInspirations.Value;
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
        
    }
}
