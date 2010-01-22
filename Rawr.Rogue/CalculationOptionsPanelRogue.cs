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

            CB_TargArmor.DisplayMember = "Key";
            CB_TargArmor.DataSource = new BindingSource(_armorBosses, null);

            CB_TargLevel.DataSource = new[] {83, 82, 81, 80};

            CB_ComboPoints1.DataSource = new[] { 0, 1, 2, 3, 4, 5 };
            CB_ComboPoints2.DataSource = new[] { 0, 1, 2, 3, 4, 5 };
            CB_ComboPoints3.DataSource = new[] { 0, 1, 2, 3, 4, 5 };

            CB_Finisher1.DisplayMember = "Name";
            CB_Finisher1.ValueMember = "Id";
            CB_Finisher1.DataSource = new Finishers();
            CB_ComboPoints1.SelectedIndex = 4;
            CB_Finisher1.SelectedIndex = CB_Finisher1.FindString(SnD.NAME);

            CB_Finisher2.DisplayMember = "Name";
            CB_Finisher2.ValueMember = "Id";
            CB_Finisher2.DataSource = new Finishers();
            CB_ComboPoints2.SelectedIndex = 5;
            CB_Finisher2.SelectedIndex = CB_Finisher2.FindString(Rupture.NAME);

            CB_Finisher3.DisplayMember = "Name";
            CB_Finisher3.ValueMember = "Id";
            CB_Finisher3.DataSource = new Finishers();

            CB_PoisonMH.DisplayMember = "Name"; 
            CB_PoisonMH.DataSource = new PoisonList();

            CB_PoisonOH.DisplayMember = "Name";
            CB_PoisonOH.DataSource = new PoisonList();

            CB_CpGenerator.DisplayMember = "Name";
            CB_CpGenerator.DataSource = new ComboPointGeneratorList();
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
                _calcOpts.Duration   = loadOpts.Duration;
                _calcOpts.CpGenerator = loadOpts.CpGenerator;
                _calcOpts.DpsCycle = loadOpts.DpsCycle;
                _calcOpts.TempMainHandEnchant = loadOpts.TempMainHandEnchant;
                _calcOpts.TempOffHandEnchant = loadOpts.TempOffHandEnchant;

                _calcOpts.Feint = loadOpts.Feint;
                _calcOpts.TurnTheTablesUptime = loadOpts.TurnTheTablesUptime;

                CB_TargLevel_SelectedIndexChanged(this, null);
                CB_TargArmor_SelectedIndexChanged(this, null);
                NUD_Duration_ValueChanged(this, null);
                CB_CpGenerator_SelectedIndexChanged(this, null);
                CycleChanged(this, null);
                OnMHPoisonChanged(this, null);
                OnOHPoisonChanged(this, null);
                Feint_CheckedChanged(this, null);
                UseTurnTheTables_CheckedChanged(this, null);

                UpdateCalculations();
            }

            m_bLoading = false;
        }

        private void OnMHPoisonChanged(object sender, EventArgs e) {
            if (!m_bLoading) {
                _calcOpts.TempMainHandEnchant = PoisonList.Get(((ComboBox)sender).Text);
                UpdateCalculations();
            } else {
                CB_PoisonMH.Text = _calcOpts.TempMainHandEnchant.Name;
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
                CB_PoisonOH.Text = _calcOpts.TempOffHandEnchant.Name;
            }
        }

        private void CB_TargArmor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
                var targetArmor = int.Parse(CB_TargArmor.Text);
                _calcOpts.TargetArmor = targetArmor;
                UpdateCalculations();
            }
            else
            {
                CB_TargArmor.Text = _calcOpts.TargetArmor.ToString();
            }
        }

        private void CB_TargLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
                _calcOpts.TargetLevel = int.Parse(CB_TargLevel.Text);
                UpdateCalculations();
            }
            else
            {
                CB_TargLevel.Text = _calcOpts.TargetLevel.ToString();
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
            cycle.Components.Add(GetCycleComponent(CB_ComboPoints1, CB_Finisher1));
            cycle.Components.Add(GetCycleComponent(CB_ComboPoints2, CB_Finisher2));
            cycle.Components.Add(GetCycleComponent(CB_ComboPoints3, CB_Finisher3));
            _calcOpts.DpsCycle = cycle;
            UpdateCalculations();
        }
            else
            {
                CB_ComboPoints1.Text   = _calcOpts.DpsCycle.Components[0].Rank.ToString();
                CB_Finisher1.Text      = _calcOpts.DpsCycle.Components[0].Finisher.Name;
                CB_ComboPoints2.Text   = _calcOpts.DpsCycle.Components[1].Rank.ToString();
                CB_Finisher2.Text      = _calcOpts.DpsCycle.Components[1].Finisher.Name;
                CB_ComboPoints3.Text   = _calcOpts.DpsCycle.Components[2].Rank.ToString();
                CB_Finisher3.Text      = _calcOpts.DpsCycle.Components[2].Finisher.Name;
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

        private void CB_CpGenerator_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
                if (CB_CpGenerator.Text == new HonorAmongThieves(0f, 0f).Name)
                {
                SetHatVisibility(true);
                HatStepper_ValueChanged(sender, e);
                return;
            }

            SetHatVisibility(false);

            _calcOpts.CpGenerator = ComboPointGeneratorList.Get(CB_CpGenerator.Text);
            UpdateCalculations();
        }
            else
            {
                CB_CpGenerator.Text = _calcOpts.CpGenerator.Name;

                if (CB_CpGenerator.Text == new HonorAmongThieves().Name)
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
                _calcOpts.CpGenerator = new HonorAmongThieves((float)NUD_Hat.Value, (float)NUD_HemoPerCycle.Value);
            UpdateCalculations();
        }
        }

        private void SetHatVisibility(bool visible)
        {
            LB_HemoPerCycle.Visible = visible;
            LB_CPperSec.Visible = visible;
            NUD_Hat.Visible = visible;
            NUD_HemoPerCycle.Visible = visible;
        }

        private void Feint_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            NUD_FeintDelay.Visible = LB_FeintDelay.Visible = CK_UseFeint.Checked;
            _calcOpts.Feint = CK_UseFeint.Checked ? new Feint((float)NUD_FeintDelay.Value) : new Feint(0);
            UpdateCalculations();
        }
            else
            {
                CK_UseFeint.Checked = _calcOpts.Feint.IsNeedFeint();

                NUD_FeintDelay.Visible   = CK_UseFeint.Checked;
                LB_FeintDelay.Visible     = CK_UseFeint.Checked;

                FeintDelayStepper_ValueChanged(sender, e);
            }
        }

        private void FeintDelayStepper_ValueChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            _calcOpts.Feint = CK_UseFeint.Checked ? new Feint((float)NUD_FeintDelay.Value) : new Feint(0);
            UpdateCalculations();
        }
            else
            {
                NUD_FeintDelay.Text = _calcOpts.Feint.Delay().ToString();
            }
        }

        private void UseTurnTheTables_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
            NUD_TurnTheTablesUptimePerc.Visible = LB_TurnTheTables.Visible = CK_UseTurnTheTables.Checked;
                _calcOpts.TurnTheTablesUptime = CK_UseTurnTheTables.Checked ? (float)NUD_TurnTheTablesUptimePerc.Value / 100f : 0f;
            UpdateCalculations();
        }
            else
            {
                CK_UseTurnTheTables.Checked = (_calcOpts.TurnTheTablesUptime > 0) ? true : false;

                NUD_TurnTheTablesUptimePerc.Visible          = CK_UseTurnTheTables.Checked;
                LB_TurnTheTables.Visible    = CK_UseTurnTheTables.Checked;

                TurnTheTablesUptimePercent_ValueChanged(sender, e);
            }
        }

        private void TurnTheTablesUptimePercent_ValueChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
                _calcOpts.TurnTheTablesUptime = (float)NUD_TurnTheTablesUptimePerc.Value / 100f;
            UpdateCalculations();
        }
            else
            {
                int value = (int)(_calcOpts.TurnTheTablesUptime * 100.0f);
                
                NUD_TurnTheTablesUptimePerc.Text = value.ToString();
            }
        }

        private void NUD_Duration_ValueChanged(object sender, EventArgs e)
        {
            if (!m_bLoading)
            {
                _calcOpts.Duration = (float)NUD_Duration.Value;
                UpdateCalculations();
            }
            else
            {
                NUD_Duration.Value = (int)_calcOpts.Duration;
            }
        }
    }
}