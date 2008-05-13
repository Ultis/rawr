using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Rawr.CustomControls;

namespace Rawr.ProtWarr
{
	public partial class CalculationOptionsPanelProtWarr : CalculationOptionsPanelBase
	{
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

		public CalculationOptionsPanelProtWarr()
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
				Character.CalculationOptions = new CalculationOptionsProtWarr();

			CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;
			comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
			trackBarTargetArmor.Value = calcOpts.TargetArmor;
            trackBarBossAttackValue.Value = calcOpts.BossAttackValue;
            trackBarThreatScale.Value = (int)calcOpts.ThreatScale;
            trackBarMitigationScale.Value = calcOpts.MitigationScale;
            trackBarShieldBlockUptime.Value = (int)calcOpts.ShieldBlockUptime;
            checkBoxUseShieldBlock.Checked = calcOpts.UseShieldBlock;
			checkBoxEnforceMetagemRequirements.Checked = Character.EnforceMetagemRequirements;
			radioButtonAldor.Checked = calcOpts.ShattrathFaction == "Aldor";
			radioButtonScryer.Checked = calcOpts.ShattrathFaction == "Scryer";
			
			labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
            labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
            labelThreatScale.Text = trackBarThreatScale.Value.ToString();
			labelMitigationScale.Text = trackBarMitigationScale.Value.ToString();
            labelShieldBlockUptime.Text = trackBarShieldBlockUptime.Value.ToString() + "%";

			_loadingCalculationOptions = false;
		}

		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;
				trackBarTargetArmor.Value = 100 * (trackBarTargetArmor.Value / 100);
				labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
				labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
				labelThreatScale.Text = trackBarThreatScale.Value.ToString();
				labelMitigationScale.Text = trackBarMitigationScale.Value.ToString();
                labelShieldBlockUptime.Text = trackBarShieldBlockUptime.Value.ToString() + "%";

				calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
				calcOpts.TargetArmor = trackBarTargetArmor.Value;
                calcOpts.BossAttackValue = trackBarBossAttackValue.Value;
                calcOpts.ThreatScale = trackBarThreatScale.Value;
                calcOpts.MitigationScale = trackBarMitigationScale.Value;
                calcOpts.ShieldBlockUptime = trackBarShieldBlockUptime.Value;
				calcOpts.ShattrathFaction = radioButtonAldor.Checked ? "Aldor" : "Scryer";

				Character.OnItemsChanged();
			}
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;
			Character.EnforceMetagemRequirements = checkBoxEnforceMetagemRequirements.Checked;
			Character.OnItemsChanged();
		}

        private void checkBoxUseShieldBlock_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsProtWarr calcOpts = Character.CalculationOptions as CalculationOptionsProtWarr;
			calcOpts.UseShieldBlock = checkBoxUseShieldBlock.Checked;
            Character.OnItemsChanged();
        }

        private void buttonTalents_Click(object sender, EventArgs e)
        {
            if ((Character.Talents != null) &&
                (Character.Talents.Trees != null) &&
                (Character.Talents.Trees.Count > 0))
            {
                TalentForm tf = new TalentForm();
                tf.SetParameters(Character);
                tf.Show();
            }
            else
            {
                MessageBox.Show("No talents found. Please reload from armory");
            }
        }
	}

	[Serializable]
	public class CalculationOptionsProtWarr : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsProtWarr));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 73;
		public int TargetArmor = 7700;
		public int BossAttackValue = 20000;
		public float ThreatScale = 20;
		public int MitigationScale = 3750;
		public bool EnforceMetagemRequirements = false;
		public float ShieldBlockUptime = 100;
		public bool UseShieldBlock = false;
		public string ShattrathFaction = "Scryer";
	}
}
