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
        public Talents(CalculationOptionsPanelRetribution retCalcOpts)
        {
            this.calcOptions = retCalcOpts;
            InitializeComponent();            
        }
        private CalculationOptionsPanelRetribution calcOptions;

        public Character Character
        {
            get
            {
                return calcOptions.Character;
            }
        }

        private void Talents_Load(object sender, EventArgs e)
        {
            if (Character.CalculationOptions.ContainsKey("TalentsSaved") && Character.CalculationOptions["TalentsSaved"] == "1")
            {
                comboBoxTwoHandedSpec.SelectedItem = Character.CalculationOptions["TwoHandedSpec"];
                comboBoxConviction.SelectedItem =Character.CalculationOptions["Conviction"];
                comboBoxCrusade.SelectedItem =Character.CalculationOptions["Crusade"];
                comboBoxDivineStrength.SelectedItem = Character.CalculationOptions["DivineStrength"];
                comboBoxFanaticism.SelectedItem = Character.CalculationOptions["Fanaticism"];
                comboBoxImprovedSanctityAura.SelectedItem = Character.CalculationOptions["ImprovedSanctityAura"];
                comboBoxPrecision.SelectedItem = Character.CalculationOptions["Precision"];
                comboBoxSanctityAura.SelectedItem = Character.CalculationOptions["SanctityAura"];
                comboBoxSanctifiedSeals.SelectedItem = Character.CalculationOptions["SanctifiedSeals"];
                comboBoxVengeance.SelectedItem = Character.CalculationOptions["Vengeance"];
                
            }
        }
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Name.Substring(8);
            Character.CalculationOptions[talent] = cb.SelectedItem.ToString();
            Character.OnItemsChanged();
        }
        
    }
}
