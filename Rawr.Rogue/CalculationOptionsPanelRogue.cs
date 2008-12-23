using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rawr.Rogue
{
    public partial class CalculationOptionsPanelRogue : CalculationOptionsPanelBase
    {
        private readonly Dictionary<int, string> armorBosses = new Dictionary<int, string>();
        private static readonly CalculationOptionsRogue _calcOpts = new CalculationOptionsRogue();

        public CalculationOptionsPanelRogue()
        {
            InitializeComponent();

            armorBosses.Add(13083, "All Level 83 Bosses");
            armorBosses.Add(10338, "Generic 82 Elite");

            comboBoxArmorBosses.DisplayMember = "Key";
            comboBoxArmorBosses.DataSource = new BindingSource(armorBosses, null);

            comboBoxTargetLevel.DataSource = new[] {83, 82, 81, 80};

            comboBoxComboPoints1.DataSource = new[] { 0, 1, 2, 3, 4, 5 };
            comboBoxComboPoints2.DataSource = new[] { 0, 1, 2, 3, 4, 5 };
            comboBoxComboPoints3.DataSource = new[] { 0, 1, 2, 3, 4, 5 };

            comboBoxFinisher1.DisplayMember = "Name";
            comboBoxFinisher1.ValueMember = "Id";
            comboBoxFinisher1.DataSource = new Finishers();
            comboBoxComboPoints1.SelectedIndex = 4;
            comboBoxFinisher1.SelectedIndex = comboBoxFinisher1.FindString(new SnD().Name);

            comboBoxFinisher2.DisplayMember = "Name";
            comboBoxFinisher2.ValueMember = "Id";
            comboBoxFinisher2.DataSource = new Finishers();
            comboBoxComboPoints2.SelectedIndex = 5;
            comboBoxFinisher2.SelectedIndex = comboBoxFinisher2.FindString(new Rupture().Name);

            comboBoxFinisher3.DisplayMember = "Name";
            comboBoxFinisher3.ValueMember = "Id";
            comboBoxFinisher3.DataSource = new Finishers();

            comboBoxMHPoison.DisplayMember = "Name"; 
            comboBoxMHPoison.DataSource = new PoisonList();

            comboBoxOHPoison.DisplayMember = "Name";
            comboBoxOHPoison.DataSource = new PoisonList();
        }

        protected override void LoadCalculationOptions()
        {
            if (Character.CalculationOptions == null)
            {
                Character.CalculationOptions = _calcOpts;
            }
        }

        private void OnMHPoisonChanged(object sender, EventArgs e)
        {
            _calcOpts.TempMainHandEnchant = PoisonList.Get(((ComboBox)sender).Text);
            UpdateCalculations();
        }

        private void OnOHPoisonChanged(object sender, EventArgs e)
        {
            _calcOpts.TempOffHandEnchant = PoisonList.Get(((ComboBox)sender).Text);
            UpdateCalculations();
        }

        private void comboBoxArmorBosses_SelectedIndexChanged(object sender, EventArgs e)
        {
            var targetArmor = int.Parse(comboBoxArmorBosses.Text);
            labelTargetArmorDescription.Text = armorBosses[targetArmor];
            _calcOpts.TargetArmor = targetArmor;
            UpdateCalculations();
        }

        private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            _calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.Text);
            UpdateCalculations();
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
            var cycle = new Cycle();
            cycle.Components.Add(GetCycleComponent(comboBoxComboPoints1, comboBoxFinisher1));
            cycle.Components.Add(GetCycleComponent(comboBoxComboPoints2, comboBoxFinisher2));
            cycle.Components.Add(GetCycleComponent(comboBoxComboPoints3, comboBoxFinisher3));
            _calcOpts.DPSCycle = cycle;
            UpdateCalculations();
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
    }
}