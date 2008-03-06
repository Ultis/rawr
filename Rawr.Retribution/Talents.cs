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
                comboBoxTwoHandedSpec.SelectedItem =(Character.CalculationOptions.ContainsKey("TwoHandedSpec") ? Character.CalculationOptions["TwoHandedSpec"] : "0");
                comboBoxConviction.SelectedItem = (Character.CalculationOptions.ContainsKey("Conviction") ? Character.CalculationOptions["Conviction"] : "0");
                comboBoxCrusade.SelectedItem = (Character.CalculationOptions.ContainsKey("Crusade") ? Character.CalculationOptions["Crusade"] : "0");
                comboBoxDivineStrength.SelectedItem = (Character.CalculationOptions.ContainsKey("DivineStrength") ? Character.CalculationOptions["DivineStrength"] : "0");
                comboBoxFanaticism.SelectedItem = (Character.CalculationOptions.ContainsKey("Fanaticism") ? Character.CalculationOptions["Fanaticism"] : "0");
                comboBoxImprovedSanctityAura.SelectedItem = (Character.CalculationOptions.ContainsKey("ImprovedSanctityAura") ? Character.CalculationOptions["ImprovedSanctityAura"] : "0");
                comboBoxPrecision.SelectedItem = (Character.CalculationOptions.ContainsKey("Precision") ? Character.CalculationOptions["Precision"] : "0");
                comboBoxSanctityAura.SelectedItem = (Character.CalculationOptions.ContainsKey("SanctityAura") ? Character.CalculationOptions["SanctityAura"] : "0");
                comboBoxSanctifiedSeals.SelectedItem = (Character.CalculationOptions.ContainsKey("SanctifiedSeals") ? Character.CalculationOptions["SanctifiedSeals"] : "0");
                comboBoxVengeance.SelectedItem = (Character.CalculationOptions.ContainsKey("Vengeance") ? Character.CalculationOptions["Vengeance"] : "0");
                               
            }
        }
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Name.Substring(8);
            Character.CalculationOptions[talent] = cb.SelectedItem.ToString();
            Character.OnItemsChanged();
            Character.CalculationOptions["TalentsSaved"] = "1";
        }
        
    }
}
