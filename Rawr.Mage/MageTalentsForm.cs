using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Mage
{
    public partial class MageTalentsForm : Form
    {
        public MageTalentsForm(CalculationOptionsPanelMage basePanel)
        {
            this.basePanel = basePanel;
            InitializeComponent();
        }

        private CalculationOptionsPanelMage basePanel;

        public Character Character
        {
            get
            {
                return basePanel.Character;
            }
        }

        private void MageTalentsForm_Load(object sender, EventArgs e)
        {
            LoadCalculationOptions();
        }

        private bool calculationSuspended = false;

        public void LoadCalculationOptions()
        {
            calculationSuspended = true;
            foreach (Control c in Controls)
            {
                if (c is GroupBox)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is ComboBox)
                        {
                            ComboBox cb = (ComboBox)cc;
                            string talent = cb.Name.Substring(8);

                            if (!Character.CalculationOptions.ContainsKey(talent))
                                Character.CalculationOptions[talent] = "0";

                            cb.SelectedItem = Character.CalculationOptions[talent];
                        }
                    }
                }
            }
            calculationSuspended = false;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Name.Substring(8);
            Character.CalculationOptions[talent] = cb.SelectedItem.ToString();
            if (!calculationSuspended) Character.OnItemsChanged();
        }

        private void MageTalentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Visible = false;
            }
        }

        private void buttonImportBlizzardCode_Click(object sender, EventArgs e)
        {
            // http://www.worldofwarcraft.com/info/classes/mage/talents.html?2550050300230151333125100000000000000000000002030302010000000000000
            string talentCode = textBoxBlizzardCode.Text;
            int index = talentCode.IndexOf('?');
            if (index >= 0) talentCode = talentCode.Substring(index + 1);

            CalculationsMage.LoadTalentCode(Character, talentCode);
            LoadCalculationOptions();
            Character.OnItemsChanged();
        }

        private void buttonImportTalentPreset_Click(object sender, EventArgs e)
        {
            CalculationsMage.LoadTalentSpec(Character, (string)comboBoxTalentPreset.SelectedItem);
            LoadCalculationOptions();
            Character.OnItemsChanged();
        }
    }
}