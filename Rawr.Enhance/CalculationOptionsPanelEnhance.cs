using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class CalculationOptionsPanelEnhance : CalculationOptionsPanelBase
	{
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();
        private int _glyphCount = 0;

		public CalculationOptionsPanelEnhance()
		{
			InitializeComponent();
            //			armorBosses.Add(3800, ": Shade of Aran");
            //			armorBosses.Add(4700, ": Roar");
            //			armorBosses.Add(5500, ": Netherspite");
            //			armorBosses.Add(6100, ": Julianne, Curator");
            //			armorBosses.Add(6200, ": Karathress, Vashj, Solarian, Kael'thas, Winterchill, Anetheron, Kaz'rogal, Azgalor, Archimonde, Teron, Shahraz");
            //			armorBosses.Add(6700, ": Maiden, Illhoof");
            //			armorBosses.Add(7300, ": Strawman");
            //			armorBosses.Add(7500, ": Attumen");
            //			armorBosses.Add(7600, ": Romulo, Nightbane, Malchezaar, Doomwalker");
            //			armorBosses.Add(7700, ": Hydross, Lurker, Leotheras, Tidewalker, Al'ar, Naj'entus, Supremus, Akama, Gurtogg");
            //			armorBosses.Add(8200, ": Midnight");
            //			armorBosses.Add(8800, ": Void Reaver");
		}

		protected override void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsEnhance();
			//if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
			//    Character.CalculationOptions["TargetLevel"] = "73";
			//if (!Character.CalculationOptions.ContainsKey("TargetArmor"))
			//    Character.CalculationOptions["TargetArmor"] = "7700";
			//if (!Character.CalculationOptions.ContainsKey("ExposeWeaknessAPValue"))
			//    Character.CalculationOptions["ExposeWeaknessAPValue"] = "200";
			//if (!Character.CalculationOptions.ContainsKey("Powershift"))
			//    Character.CalculationOptions["Powershift"] = "4";
			//if (!Character.CalculationOptions.ContainsKey("PrimaryAttack"))
			//    Character.CalculationOptions["PrimaryAttack"] = "Both";
			//if (!Character.CalculationOptions.ContainsKey("Finisher"))
			//    Character.CalculationOptions["Finisher"] = "Rip";
			//if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
			//    Character.CalculationOptions["EnforceMetagemRequirements"] = "No";
			//if (!Character.CalculationOptions.ContainsKey("BloodlustUptime"))
			//    Character.CalculationOptions["BloodlustUptime"] = "15";
			//if (!Character.CalculationOptions.ContainsKey("ShattrathFaction"))
			//    Character.CalculationOptions["ShattrathFaction"] = "Aldor";

			CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
			comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
			trackBarTargetArmor.Value = calcOpts.TargetArmor;
			trackBarExposeWeakness.Value = calcOpts.ExposeWeaknessAPValue;
			trackBarNumberOfFerociousInspirations.Value = calcOpts.NumberOfFerociousInspirations;
			trackBarBloodlustUptime.Value = (int)Math.Round(calcOpts.BloodlustUptime * 100);
            comboBoxMainhandImbue.SelectedItem = calcOpts.MainhandImbue;
            comboBoxOffhandImbue.SelectedItem = calcOpts.OffhandImbue;

            chbGlyphFT.Checked = calcOpts.GlyphFT;
            chbGlyphLL.Checked = calcOpts.GlyphLL;
            chbGlyphLB.Checked = calcOpts.GlyphLB;
            chbGlyphLS.Checked = calcOpts.GlyphLS;
            chbGlyphShocking.Checked = calcOpts.GlyphShocking;
            chbGlyphSS.Checked = calcOpts.GlyphSS;
            chbGlyphWF.Checked = calcOpts.GlyphWF;

            _glyphCount = (chbGlyphFT.Checked ? 1 : 0) + (chbGlyphLL.Checked ? 1 : 0) + (chbGlyphLB.Checked ? 1 : 0) +
                          (chbGlyphLS.Checked ? 1 : 0) + (chbGlyphSS.Checked ? 1 : 0) + (chbGlyphWF.Checked ? 1 : 0) +
                          (chbGlyphShocking.Checked ? 1 : 0);
            
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
				labelExposeWeakness.Text = trackBarExposeWeakness.Value.ToString();
				labelNumberOfFerociousInspirations.Text = trackBarNumberOfFerociousInspirations.Value.ToString();
				labelBloodlustUptime.Text = trackBarBloodlustUptime.Value.ToString() + "%";

				CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
				calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
				calcOpts.TargetArmor = trackBarTargetArmor.Value;
				calcOpts.NumberOfFerociousInspirations = trackBarNumberOfFerociousInspirations.Value;
				calcOpts.BloodlustUptime = (float)trackBarBloodlustUptime.Value / 100f;
				calcOpts.ExposeWeaknessAPValue = trackBarExposeWeakness.Value;
                calcOpts.MainhandImbue = (string) comboBoxMainhandImbue.SelectedItem;
                calcOpts.OffhandImbue = (string) comboBoxOffhandImbue.SelectedItem;
				calcOpts.ShattrathFaction = radioButtonAldor.Checked ? "Aldor" : "Scryer";

                calcOpts.GlyphFT = chbGlyphFT.Checked;
                calcOpts.GlyphLL = chbGlyphLL.Checked;
                calcOpts.GlyphLB = chbGlyphLB.Checked;
                calcOpts.GlyphLS = chbGlyphLS.Checked;
                calcOpts.GlyphShocking = chbGlyphShocking.Checked;
                calcOpts.GlyphSS = chbGlyphSS.Checked;
                calcOpts.GlyphWF = chbGlyphWF.Checked;

				Character.OnCalculationsInvalidated();
			}
		}

        private bool checkMaxGlyphs(CheckBox glyph)
        {
            _glyphCount += glyph.Checked ? 1 : -1;
            if (glyph.Checked & _glyphCount == 4)
            {
                System.Windows.Forms.MessageBox.Show("You can enable a maximum of three glyphs",
                                                     "Enhance Module", System.Windows.Forms.MessageBoxButtons.OK);
                glyph.Checked = false;
            }
            else
                Character.OnCalculationsInvalidated();
            return glyph.Checked;
        }

        private void chbGlyphFT_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.GlyphFT = checkMaxGlyphs(chbGlyphFT);
            }
        }

        private void chbGlyphLL_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.GlyphLL = checkMaxGlyphs(chbGlyphLL);
            }
        }

        private void chbGlyphLB_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.GlyphLB = checkMaxGlyphs(chbGlyphLB);
            }
        }

        private void chbGlyphLS_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.GlyphLS = checkMaxGlyphs(chbGlyphLS);
            }
        }

        private void chbGlyphShocking_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.GlyphShocking = checkMaxGlyphs(chbGlyphShocking);
            }
        }

        private void chbGlyphSS_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.GlyphSS = checkMaxGlyphs(chbGlyphSS);
            }
        }

        private void chbGlyphWF_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.GlyphWF = checkMaxGlyphs(chbGlyphWF);
            }
        }

        private void btnEnhSim_Click(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                Enhance.EnhSim simExport = new Enhance.EnhSim(Character);
                simExport.copyToClipboard();
            }
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

        private void chbPatch3_1_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.Patch3_1 = chbPatch3_1.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphFS_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
                calcOpts.GlyphFS = checkMaxGlyphs(chbGlyphFS);
            }
        }
    }

	[Serializable]
	public class CalculationOptionsEnhance : ICalculationOptionBase
	{
        public bool GlyphFS { get; set; }
        public bool GlyphFT { get; set; }
        public bool GlyphLL { get; set; }
        public bool GlyphLB { get; set; }
        public bool GlyphLS { get; set; }
        public bool GlyphShocking { get; set; }
        public bool GlyphSS { get; set; }
        public bool GlyphWF { get; set; }
        public bool BaseStatOption { get; set; }
       
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
		public int TargetArmor = 13083;
		public int ExposeWeaknessAPValue = 200;
		public int NumberOfFerociousInspirations = 2;
		public float BloodlustUptime = 0.15f;
		public string ShattrathFaction = "Aldor";
        public string MainhandImbue = "Windfury";
        public string OffhandImbue = "Flametongue";
        public bool Patch3_1 = true;
       
        
    }
}
