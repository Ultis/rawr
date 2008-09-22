using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Retribution
{
    public partial class Talents : Form
    {
        public Talents(CalculationOptionsPanelRetribution retCalcOptsPanel)
        {
            this.calcOptionsPanel = retCalcOptsPanel;
            InitializeComponent();            
        }
        private CalculationOptionsPanelRetribution calcOptionsPanel;

        public Character Character
        {
            get
            {
                return calcOptionsPanel.Character;
            }
        }

        private void Talents_Load(object sender, EventArgs e)
        {
			CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
            if (calcOpts.TalentsSaved)
            {
				comboBoxTwoHandedSpec.SelectedItem = calcOpts.TwoHandedSpec.ToString();
                comboBoxConviction.SelectedItem = calcOpts.Conviction.ToString();
                comboBoxCrusade.SelectedItem = calcOpts.Crusade.ToString();
                comboBoxDivineStrength.SelectedItem = calcOpts.DivineStrength.ToString();
                comboBoxFanaticism.SelectedItem = calcOpts.Fanaticism.ToString();
                comboBoxImprovedSanctityAura.SelectedItem = calcOpts.ImprovedSanctityAura.ToString();
                comboBoxPrecision.SelectedItem = calcOpts.Precision.ToString();
                comboBoxSanctityAura.SelectedItem = calcOpts.SanctityAura.ToString();
                comboBoxSanctifiedSeals.SelectedItem = calcOpts.SanctifiedSeals.ToString();
                comboBoxVengeance.SelectedItem = calcOpts.Vengeance.ToString();
                               
            }
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Name.Substring(8);
			CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
            switch (talent)
			{
                case "TwoHandedSpec": calcOpts.TwoHandedSpec = cb.SelectedIndex; break;// int.Parse(cb.SelectedItem.ToString()); break;
                case "Conviction": calcOpts.Conviction = cb.SelectedIndex; break;
                case "Crusade": calcOpts.Crusade = cb.SelectedIndex; break;
                case "DivineStrength": calcOpts.DivineStrength = cb.SelectedIndex; break;
                case "Fanaticism": calcOpts.Fanaticism = cb.SelectedIndex; break;
                case "ImprovedSanctityAura": calcOpts.ImprovedSanctityAura = cb.SelectedIndex; break;
                case "Precision": calcOpts.Precision = cb.SelectedIndex; break;
                case "SanctityAura": calcOpts.SanctityAura = cb.SelectedIndex; break;
                case "SanctifiedSeals": calcOpts.SanctifiedSeals = cb.SelectedIndex; break;
                case "Vengeance": calcOpts.Vengeance = cb.SelectedIndex; break;
			}
            Character.OnCalculationsInvalidated();
			calcOpts.TalentsSaved = true;
        }
    }
}
