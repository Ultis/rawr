using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Rawr.Rogue.ComboPointGenerators;
using Rawr.Rogue.FinishingMoves;
using Rawr.Rogue.Poisons;
using Rawr.Rogue.SpecialAbilities;

namespace Rawr.Rogue
{
    public partial class CalculationOptionsPanelRogue : CalculationOptionsPanelBase
    {
        private readonly Dictionary<int, string> _armorBosses = new Dictionary<int, string>();
        private static readonly CalculationOptionsRogue _calcOpts = new CalculationOptionsRogue();

        public CalculationOptionsPanelRogue()
        {
            InitializeComponent();

            _armorBosses.Add((int)StatConversion.NPC_ARMOR[80 - 80], "Level 80 Mob");
            _armorBosses.Add((int)StatConversion.NPC_ARMOR[81 - 80], "Level 81 Mob");
            _armorBosses.Add((int)StatConversion.NPC_ARMOR[82 - 80], "Level 82 Mob");
            _armorBosses.Add((int)StatConversion.NPC_ARMOR[83 - 80], "Ulduar Bosses");

            CB_PoisonMH.DisplayMember = "Name"; 
            CB_PoisonMH.DataSource = new PoisonList();

            CB_PoisonOH.DisplayMember = "Name";
            CB_PoisonOH.DataSource = new PoisonList();
        }

        protected override void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsRogue();

            CalculationOptionsRogue calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
            comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
            numericUpDownTargetArmor.Value = calcOpts.TargetArmor;
            checkBoxPoisonable.Checked = calcOpts.TargetPoisonable;
            numericUpDownDuration.Value = calcOpts.Duration;
            numericUpDownLagVariance.Value = calcOpts.LagVariance;
            checkBoxRupt.Checked = calcOpts.CustomUseRupt;
            checkBoxExpose.Checked = calcOpts.CustomUseExpose;
            comboBoxCPG.SelectedIndex = calcOpts.CustomCPG;
            comboBoxSnD.SelectedItem = calcOpts.CustomCPSnD.ToString();
            comboBoxFinisher.SelectedIndex = calcOpts.CustomFinisher;
            comboBoxCPFinisher.SelectedItem = calcOpts.CustomCPFinisher.ToString();
            comboBoxMHPoison.SelectedIndex = calcOpts.CustomMHPoison;
            comboBoxOHPoison.SelectedIndex = calcOpts.CustomOHPoison;
            trackBarTrinketOffset.Value = (int)(calcOpts.TrinketOffset * 2);

            labelTrinketOffset.Text = string.Format(labelTrinketOffset.Tag.ToString(), calcOpts.TrinketOffset);

            _loadingCalculationOptions = false;
        }

        private void UpdateCalculations()
        {
            if (Character != null && Character.CalculationOptions != null)
            {
                Character.CalculationOptions = _calcOpts;
                Character.OnCalculationsInvalidated();
            }
        }

        private bool _loadingCalculationOptions = false;
        private void calculationOptionControl_Changed(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsRogue calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
                calcOpts.TargetArmor = (int)numericUpDownTargetArmor.Value;
                calcOpts.TargetPoisonable = checkBoxPoisonable.Checked;
                calcOpts.Duration = (int)numericUpDownDuration.Value;
                calcOpts.LagVariance = (int)numericUpDownLagVariance.Value;
                calcOpts.CustomUseRupt = checkBoxRupt.Checked;
                calcOpts.CustomUseExpose = checkBoxExpose.Checked;
                calcOpts.CustomCPG = comboBoxCPG.SelectedIndex;
                calcOpts.CustomCPSnD = int.Parse(comboBoxSnD.SelectedItem.ToString());
                calcOpts.CustomFinisher = comboBoxFinisher.SelectedIndex;
                calcOpts.CustomCPFinisher = int.Parse(comboBoxCPFinisher.SelectedItem.ToString());
                calcOpts.CustomMHPoison = comboBoxMHPoison.SelectedIndex;
                calcOpts.CustomOHPoison = comboBoxOHPoison.SelectedIndex;
                calcOpts.TrinketOffset = (float)trackBarTrinketOffset.Value / 2f;

                labelTrinketOffset.Text = string.Format(labelTrinketOffset.Tag.ToString(), calcOpts.TrinketOffset);

                Character.OnCalculationsInvalidated();
            }
        }
    }
}