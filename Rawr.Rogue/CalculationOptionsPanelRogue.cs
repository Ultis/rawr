using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Rogue
{
    public partial class CalculationOptionsPanelRogue : CalculationOptionsPanelBase
    {
        private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

        public CalculationOptionsPanelRogue()
        {
            InitializeComponent();

            armorBosses.Add(10000, "Patchwerk");
            armorBosses.Add(11000, "Grobbulus");

            comboBoxArmorBosses.DataSource = new BindingSource(armorBosses, null);
            comboBoxArmorBosses.DisplayMember = "Key";

            comboBoxTargetLevel.DataSource = new[] {83, 82, 81, 80};

            comboBoxMHPoison.DataSource = new PoisonList();
            comboBoxMHPoison.DisplayMember = "Name";

            comboBoxOHPoison.DataSource = new PoisonList();
            comboBoxOHPoison.DisplayMember = "Name";
        }

        private bool _loadingCalculationOptions = false;

        protected override void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;

            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsRogue(Character);

            var calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
            comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();

            _loadingCalculationOptions = false;
        }


        private void OnCheckedChanged(object sender, EventArgs e)
        {
        }

        private void OnMHPoisonChanged(object sender, EventArgs e)
        {
            if (Character != null && Character.CalculationOptions != null)
            {
                var calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                calcOpts.TempMainHandEnchant = ((ComboBox)sender).Text;
                Character.OnCalculationsInvalidated();
            }
        }

        private void OnOHPoisonChanged(object sender, EventArgs e)
        {
            if (Character != null && Character.CalculationOptions != null)
            {
                var calcOpts = Character.CalculationOptions as CalculationOptionsRogue;
                calcOpts.TempOffHandEnchant = ((ComboBox)sender).Text;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxArmorBosses_SelectedIndexChanged(object sender, EventArgs e)
        {
            var targetArmor = int.Parse(comboBoxArmorBosses.Text);
            labelTargetArmorDescription.Text = "Bosses: " + armorBosses[targetArmor];

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

    [Serializable]
    public class CalculationOptionsRogue : ICalculationOptionBase
    {
        public string GetXml()
        {
            var s = new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRogue));
            var xml = new StringBuilder();
            var sw = new System.IO.StringWriter(xml);
            s.Serialize(sw, this);
            return xml.ToString();
        }

        public CalculationOptionsRogue() { }
        public CalculationOptionsRogue(Character character) : this()
        {
            DPSCycle = new Cycle("4s5r");
        }

        public int TargetLevel = 83;
        public int TargetArmor = 10000;
        public Cycle DPSCycle;
        public string TempMainHandEnchant;
        public string TempOffHandEnchant;
    }
}