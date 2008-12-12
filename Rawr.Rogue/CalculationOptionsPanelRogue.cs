using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rawr.Rogue
{
    public partial class CalculationOptionsPanelRogue : CalculationOptionsPanelBase
    {
        private readonly Dictionary<int, string> armorBosses = new Dictionary<int, string>();

        public CalculationOptionsPanelRogue()
        {
            InitializeComponent();

            armorBosses.Add(10000, "Patchwerk");
            armorBosses.Add(11000, "Grobbulus");

            comboBoxArmorBosses.DisplayMember = "Key";
            comboBoxArmorBosses.DataSource = new BindingSource(armorBosses, null);

            comboBoxTargetLevel.DataSource = new[] {83, 82, 81, 80};

            comboBoxMHPoison.DisplayMember = "Name"; 
            comboBoxMHPoison.DataSource = new PoisonList();

            comboBoxOHPoison.DisplayMember = "Name";
            comboBoxOHPoison.DataSource = new PoisonList();
        }

        protected override void LoadCalculationOptions()
        {
            if (Character.CalculationOptions == null)
            {
                Character.CalculationOptions = new CalculationOptionsRogue();
            }
        }

        private void OnMHPoisonChanged(object sender, EventArgs e)
        {
            if (Character != null && Character.CalculationOptions != null)
            {
                var calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                calcOpts.TempMainHandEnchant = PoisonList.Get(((ComboBox)sender).Text);
                Character.OnCalculationsInvalidated();
            }
        }

        private void OnOHPoisonChanged(object sender, EventArgs e)
        {
            if (Character != null && Character.CalculationOptions != null)
            {
                var calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                calcOpts.TempOffHandEnchant = PoisonList.Get(((ComboBox)sender).Text);
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxArmorBosses_SelectedIndexChanged(object sender, EventArgs e)
        {
            var targetArmor = int.Parse(comboBoxArmorBosses.Text);
            labelTargetArmorDescription.Text = armorBosses[targetArmor];

            if (Character != null && Character.CalculationOptions != null)
            {
                var calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                calcOpts.TargetArmor = targetArmor;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Character != null && Character.CalculationOptions != null)
            {
                var calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.Text);
                Character.OnCalculationsInvalidated();
            }
        }
    }
}