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
				comboBoxTwoHandedSpec.SelectedItem = calcOpts.TwoHandedSpec;
				comboBoxConviction.SelectedItem = calcOpts.Conviction;
                comboBoxCrusade.SelectedItem = calcOpts.Crusade;
                comboBoxDivineStrength.SelectedItem = calcOpts.DivineStrength;
                comboBoxFanaticism.SelectedItem = calcOpts.Fanaticism;
                comboBoxImprovedSanctityAura.SelectedItem = calcOpts.ImprovedSanctityAura;
                comboBoxPrecision.SelectedItem = calcOpts.Precision;
                comboBoxSanctityAura.SelectedItem = calcOpts.SanctityAura;
                comboBoxSanctifiedSeals.SelectedItem = calcOpts.SanctifiedSeals;
                comboBoxVengeance.SelectedItem = calcOpts.Vengeance;
                               
            }
        }
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Name.Substring(8);
			CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
            switch (talent)
			{
				case "TwoHandedSpec": calcOpts.TwoHandedSpec = int.Parse(cb.SelectedItem.ToString()); break;
				case "Conviction": calcOpts.Conviction = int.Parse(cb.SelectedItem.ToString()); break;
				case "Crusade": calcOpts.Crusade = int.Parse(cb.SelectedItem.ToString()); break;
				case "DivineStrength": calcOpts.DivineStrength = int.Parse(cb.SelectedItem.ToString()); break;
				case "Fanaticism": calcOpts.Fanaticism = int.Parse(cb.SelectedItem.ToString()); break;
				case "ImprovedSanctityAura": calcOpts.ImprovedSanctityAura = int.Parse(cb.SelectedItem.ToString()); break;
				case "Precision": calcOpts.Precision = int.Parse(cb.SelectedItem.ToString()); break;
				case "SanctityAura": calcOpts.SanctityAura = int.Parse(cb.SelectedItem.ToString()); break;
				case "SanctifiedSeals": calcOpts.SanctifiedSeals = int.Parse(cb.SelectedItem.ToString()); break;
				case "Vengeance": calcOpts.Vengeance = int.Parse(cb.SelectedItem.ToString()); break;
			}
            Character.OnItemsChanged();
			calcOpts.TalentsSaved = true;
        }
        
    }
}
