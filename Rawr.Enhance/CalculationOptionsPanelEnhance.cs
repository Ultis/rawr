using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class CalculationOptionsPanelCat : CalculationOptionsPanelBase
	{
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

		public CalculationOptionsPanelCat()
		{
			InitializeComponent();
			armorBosses.Add(3800, ": Shade of Aran");
			armorBosses.Add(4700, ": Roar");
			armorBosses.Add(5500, ": Netherspite");
			armorBosses.Add(6100, ": Julianne, Curator");
			armorBosses.Add(6200, ": Karathress, Vashj, Solarian, Kael'thas, Winterchill, Anetheron, Kaz'rogal, Azgalor, Archimonde, Teron, Shahraz");
			armorBosses.Add(6700, ": Maiden, Illhoof");
			armorBosses.Add(7300, ": Strawman");
			armorBosses.Add(7500, ": Attumen");
			armorBosses.Add(7600, ": Romulo, Nightbane, Malchezaar, Doomwalker");
			armorBosses.Add(7700, ": Hydross, Lurker, Leotheras, Tidewalker, Al'ar, Naj'entus, Supremus, Akama, Gurtogg");
			armorBosses.Add(8200, ": Midnight");
			armorBosses.Add(8800, ": Void Reaver");
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
			//if (!Character.CalculationOptions.ContainsKey("DrumsOfBattleUptime"))
			//    Character.CalculationOptions["DrumsOfBattleUptime"] = "25";
			//if (!Character.CalculationOptions.ContainsKey("DrumsOfWarUptime"))
			//    Character.CalculationOptions["DrumsOfWarUptime"] = "25";
			//if (!Character.CalculationOptions.ContainsKey("ShattrathFaction"))
			//    Character.CalculationOptions["ShattrathFaction"] = "Aldor";

			CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
			comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
			trackBarTargetArmor.Value = calcOpts.TargetArmor;
			trackBarExposeWeakness.Value = calcOpts.ExposeWeaknessAPValue;
			trackBarNumberOfFerociousInspirations.Value = calcOpts.NumberOfFerociousInspirations;
			trackBarBloodlustUptime.Value = (int)Math.Round(calcOpts.BloodlustUptime * 100);
			trackBarDrumsOfBattleUptime.Value = (int)Math.Round(calcOpts.DrumsOfBattleUptime * 100);
			trackBarDrumsOfWarUptime.Value = (int)Math.Round(calcOpts.DrumsOfWarUptime * 100);
            comboBoxMainhandImbue.SelectedItem = calcOpts.MainhandImbue;
            comboBoxOffhandImbue.SelectedItem = calcOpts.OffhandImbue;
			
			labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
			labelNumberOfFerociousInspirations.Text = trackBarNumberOfFerociousInspirations.Value.ToString();
			labelBloodlustUptime.Text = trackBarBloodlustUptime.Value.ToString() + "%";
			labelDrumsOfBattleUptime.Text = trackBarDrumsOfBattleUptime.Value.ToString() + "%";
			labelDrumsOfWarUptime.Text = trackBarDrumsOfWarUptime.Value.ToString() + "%";

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
				labelDrumsOfBattleUptime.Text = trackBarDrumsOfBattleUptime.Value.ToString() + "%";
				labelDrumsOfWarUptime.Text = trackBarDrumsOfWarUptime.Value.ToString() + "%";

				CalculationOptionsEnhance calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
				calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
				calcOpts.TargetArmor = trackBarTargetArmor.Value;
				calcOpts.NumberOfFerociousInspirations = trackBarNumberOfFerociousInspirations.Value;
				calcOpts.BloodlustUptime = (float)trackBarBloodlustUptime.Value / 100f;
				calcOpts.DrumsOfBattleUptime = (float)trackBarDrumsOfBattleUptime.Value / 100f;
				calcOpts.DrumsOfWarUptime = (float)trackBarDrumsOfWarUptime.Value / 100f;
				calcOpts.ExposeWeaknessAPValue = trackBarExposeWeakness.Value;
                calcOpts.MainhandImbue = (string) comboBoxMainhandImbue.SelectedItem;
                calcOpts.OffhandImbue = (string) comboBoxOffhandImbue.SelectedItem;
				calcOpts.ShattrathFaction = radioButtonAldor.Checked ? "Aldor" : "Scryer";

				Character.OnCalculationsInvalidated();
			}
		}

	}

	[Serializable]
	public class CalculationOptionsEnhance : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer = 
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsEnhance));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 73;
		public int TargetArmor = 7700;
		public int ExposeWeaknessAPValue = 200;
		public int NumberOfFerociousInspirations = 2;
		public float BloodlustUptime = 0.15f;
		public float DrumsOfBattleUptime = 0.25f;
		public float DrumsOfWarUptime = 0.25f;
		public string ShattrathFaction = "Aldor";
        public string MainhandImbue = "Windfury";
        public string OffhandImbue = "Flametongue";
	}
}
