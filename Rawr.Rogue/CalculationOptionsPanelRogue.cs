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
        private bool m_bLoading;

        public CalculationOptionsPanelRogue()
        {
            InitializeComponent();

            _armorBosses.Add((int)StatConversion.NPC_ARMOR[80 - 80], "Level 80 Mob");
            _armorBosses.Add((int)StatConversion.NPC_ARMOR[81 - 80], "Level 81 Mob");
            _armorBosses.Add((int)StatConversion.NPC_ARMOR[82 - 80], "Level 82 Mob");
            _armorBosses.Add((int)StatConversion.NPC_ARMOR[83 - 80], "Ulduar Bosses");

            comboBoxArmorBosses.DisplayMember = "Key";
            comboBoxArmorBosses.DataSource = new BindingSource(_armorBosses, null);

            comboBoxTargetLevel.DataSource = new[] {83, 82, 81, 80};

            comboBoxComboPoints1.DataSource = new[] { 0, 1, 2, 3, 4, 5 };
            comboBoxComboPoints2.DataSource = new[] { 0, 1, 2, 3, 4, 5 };
            comboBoxComboPoints3.DataSource = new[] { 0, 1, 2, 3, 4, 5 };

            comboBoxFinisher1.DisplayMember = "Name";
            comboBoxFinisher1.ValueMember = "Id";
            comboBoxFinisher1.DataSource = new Finishers();
            comboBoxComboPoints1.SelectedIndex = 4;
            comboBoxFinisher1.SelectedIndex = comboBoxFinisher1.FindString(SnD.NAME);

            comboBoxFinisher2.DisplayMember = "Name";
            comboBoxFinisher2.ValueMember = "Id";
            comboBoxFinisher2.DataSource = new Finishers();
            comboBoxComboPoints2.SelectedIndex = 5;
            comboBoxFinisher2.SelectedIndex = comboBoxFinisher2.FindString(Rupture.NAME);

            comboBoxFinisher3.DisplayMember = "Name";
            comboBoxFinisher3.ValueMember = "Id";
            comboBoxFinisher3.DataSource = new Finishers();

            comboBoxMHPoison.DisplayMember = "Name"; 
            comboBoxMHPoison.DataSource = new PoisonList();

            comboBoxOHPoison.DisplayMember = "Name";
            comboBoxOHPoison.DataSource = new PoisonList();

            ComboBoxCpGenerator.DisplayMember = "Name";
            ComboBoxCpGenerator.DataSource = new ComboPointGeneratorList();
        }

        protected override void LoadCalculationOptions()
        {
            m_bLoading = true;

            if (Character.CalculationOptions == null)
            {
                Character.CalculationOptions = _calcOpts;
            }
            else
            {
                CalculationOptionsRogue loadOpts = Character.CalculationOptions as CalculationOptionsRogue;

                _calcOpts.TargetLevel = loadOpts.TargetLevel;
                _calcOpts.TargetArmor = loadOpts.TargetArmor;
                _calcOpts.CpGenerator = loadOpts.CpGenerator;
                _calcOpts.DpsCycle = loadOpts.DpsCycle;
                _calcOpts.TempMainHandEnchant = loadOpts.TempMainHandEnchant;
                _calcOpts.TempOffHandEnchant = loadOpts.TempOffHandEnchant;

                _calcOpts.Feint = loadOpts.Feint;
                _calcOpts.TurnTheTablesUptime = loadOpts.TurnTheTablesUptime;
                _calcOpts.TargetIsValidForMurder = loadOpts.TargetIsValidForMurder;

                comboBoxTargetLevel_SelectedIndexChanged(this, null);
                comboBoxArmorBosses_SelectedIndexChanged(this, null);
                ComboBoxCpGenerator_SelectedIndexChanged(this, null);
                CycleChanged(this, null);
                OnMHPoisonChanged(this, null);
                OnOHPoisonChanged(this, null);
                Feint_CheckedChanged(this, null);
                UseTurnTheTables_CheckedChanged(this, null);
                MurderTalentCheckBox_CheckedChanged(this, null);

                UpdateCalculations();
        }

            m_bLoading = false;
        }

        private void OnMHPoisonChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            _calcOpts.TempMainHandEnchant = PoisonList.Get(((ComboBox)sender).Text);
            UpdateCalculations();
        }
            else
            {
                comboBoxMHPoison.Text = _calcOpts.TempMainHandEnchant.Name;
            }
        }

        private void OnOHPoisonChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            _calcOpts.TempOffHandEnchant = PoisonList.Get(((ComboBox)sender).Text);
            UpdateCalculations();
        }
            else
            {
                comboBoxOHPoison.Text = _calcOpts.TempOffHandEnchant.Name;
            }
        }

        private void comboBoxArmorBosses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            var targetArmor = int.Parse(comboBoxArmorBosses.Text);
            labelTargetArmorDescription.Text = _armorBosses[targetArmor];
            _calcOpts.TargetArmor = targetArmor;
            UpdateCalculations();
        }
            else
            {
                comboBoxArmorBosses.Text = _calcOpts.TargetArmor.ToString();
                labelTargetArmorDescription.Text = _armorBosses[_calcOpts.TargetArmor];
            }
        }

        private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            _calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.Text);
            UpdateCalculations();
        }
            else
            {
                comboBoxTargetLevel.Text = _calcOpts.TargetLevel.ToString();
            }
        }

        private void UpdateCalculations()
        {
            if (Character != null && Character.CalculationOptions != null)
            {
                Character.CalculationOptions = _calcOpts;
                Character.OnCalculationsInvalidated();
            }
        }

        private void CycleChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            var cycle = new Cycle();
            cycle.Components.Add(GetCycleComponent(comboBoxComboPoints1, comboBoxFinisher1));
            cycle.Components.Add(GetCycleComponent(comboBoxComboPoints2, comboBoxFinisher2));
            cycle.Components.Add(GetCycleComponent(comboBoxComboPoints3, comboBoxFinisher3));
            _calcOpts.DpsCycle = cycle;
            UpdateCalculations();
        }
            else
            {
                comboBoxComboPoints1.Text   = _calcOpts.DpsCycle.Components[0].Rank.ToString();
                comboBoxFinisher1.Text      = _calcOpts.DpsCycle.Components[0].Finisher.Name;
                comboBoxComboPoints2.Text   = _calcOpts.DpsCycle.Components[1].Rank.ToString();
                comboBoxFinisher2.Text      = _calcOpts.DpsCycle.Components[1].Finisher.Name;
                comboBoxComboPoints3.Text   = _calcOpts.DpsCycle.Components[2].Rank.ToString();
                comboBoxFinisher3.Text      = _calcOpts.DpsCycle.Components[2].Finisher.Name;
            }
        }

        private static CycleComponent GetCycleComponent(ComboBox comboBoxComboPoints, ComboBox comboBoxFinisher)
        {

            var component = new CycleComponent
                                {
                                    Rank = comboBoxComboPoints.Text == "" ? 0 : int.Parse(comboBoxComboPoints.Text), 
                                    Finisher = Finishers.Get(comboBoxFinisher.Text)
                                };
            return component;
        }

        private void ComboBoxCpGenerator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
                if (ComboBoxCpGenerator.Text == new HonorAmongThieves(0f, 0f).Name)
                {
                SetHatVisibility(true);
                HatStepper_ValueChanged(sender, e);
                return;
            }

            SetHatVisibility(false);

            _calcOpts.CpGenerator = ComboPointGeneratorList.Get(ComboBoxCpGenerator.Text);
            UpdateCalculations();
        }
            else
            {
                ComboBoxCpGenerator.Text = _calcOpts.CpGenerator.Name;

                if (ComboBoxCpGenerator.Text == new HonorAmongThieves().Name)
                {
                    SetHatVisibility(true);

                    //  HatStepper_ValueChanged
                    //  SetHatVisibility
                }
                else
                {
                    SetHatVisibility(false);
                }
            }
        }

        private void HatStepper_ValueChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
                _calcOpts.CpGenerator = new HonorAmongThieves((float)HatStepper.Value, (float)HemoPerCycle.Value);
            UpdateCalculations();
        }
        }

        private void SetHatVisibility(bool visible)
        {
            HemoPerCycleLabel.Visible = visible;
            CpPerSecLabel.Visible = visible;
            HatStepper.Visible = visible;
            HemoPerCycle.Visible = visible;
        }

        private void Feint_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            FeintDelayStepper.Visible = FeintDelayLabel.Visible = Feint.Checked;
            _calcOpts.Feint = Feint.Checked ? new Feint((float)FeintDelayStepper.Value) : new Feint(0);
            UpdateCalculations();
        }
            else
            {
                Feint.Checked = _calcOpts.Feint.IsNeedFeint();

                FeintDelayStepper.Visible   = Feint.Checked;
                FeintDelayLabel.Visible     = Feint.Checked;

                FeintDelayStepper_ValueChanged(sender, e);
            }
        }

        private void FeintDelayStepper_ValueChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            _calcOpts.Feint = Feint.Checked ? new Feint((float)FeintDelayStepper.Value) : new Feint(0);
            UpdateCalculations();
        }
            else
            {
                FeintDelayStepper.Text = _calcOpts.Feint.Delay().ToString();
            }
        }

        private void UseTurnTheTables_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            TurnTheTablesUptimePercent.Visible = TurnTheTablesNumericStepperLabel.Visible = UseTurnTheTables.Checked;
                _calcOpts.TurnTheTablesUptime = UseTurnTheTables.Checked ? (float)TurnTheTablesUptimePercent.Value / 100f : 0f;
            UpdateCalculations();
        }
            else
            {
                UseTurnTheTables.Checked = (_calcOpts.TurnTheTablesUptime > 0) ? true : false;

                TurnTheTablesUptimePercent.Visible          = UseTurnTheTables.Checked;
                TurnTheTablesNumericStepperLabel.Visible    = UseTurnTheTables.Checked;

                TurnTheTablesUptimePercent_ValueChanged(sender, e);
            }
        }

        private void TurnTheTablesUptimePercent_ValueChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
                _calcOpts.TurnTheTablesUptime = (float)TurnTheTablesUptimePercent.Value / 100f;
            UpdateCalculations();
        }
            else
            {
                int value = (int)(_calcOpts.TurnTheTablesUptime * 100.0f);
                
                TurnTheTablesUptimePercent.Text = value.ToString();
            }
        }

        private void MurderTalentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            _calcOpts.TargetIsValidForMurder = MurderTalentCheckBox.Checked;
            UpdateCalculations();
        }
            else
            {
                MurderTalentCheckBox.Checked = _calcOpts.TargetIsValidForMurder;
            }
        }
    }
}