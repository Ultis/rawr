using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Rogue {
    public partial class RogueTalents : Form {
        private CalculationOptionsPanelRogue basePanel;
        private bool calculationSuspended = false;

        public RogueTalents(CalculationOptionsPanelRogue panel) {
            this.basePanel = panel;
            InitializeComponent();
        }

        public Character Character {
            get { return basePanel.Character; }
        }

        public void LoadCalculationOptions() {
            CalculationOptionsRogue options = Character.CalculationOptions as CalculationOptionsRogue;
            calculationSuspended = true;
            string talent;

            foreach (Control c in Controls) {
                if (c is GroupBox) {
                    foreach (Control cc in c.Controls) {
                        if (cc is ComboBox) {
                            ComboBox cb = (ComboBox)cc;
                            talent = cb.Name.Substring(8);

                            cb.SelectedItem = options.GetTalentByName(talent).ToString();
                        }
                    }
                }
            }

            calculationSuspended = false;
        }

        private void buttonImportBlizzardCode_Click(object sender, EventArgs e) {
            // http://www.worldofwarcraft.com/info/classes/mage/talents.html?tal=2550050300230151333125100000000000000000000002030302010000000000000
            string talentCode = textBoxBlizzardCode.Text;
            int index = talentCode.IndexOf('=');
            if (index >= 0) talentCode = talentCode.Substring(index + 1);

            CalculationsRogue.LoadTalentCode(Character, talentCode);
            LoadCalculationOptions();
            Character.OnCalculationsInvalidated();
        }

        private void buttonImportTalentPreset_Click(object sender, EventArgs e) {
            CalculationsRogue.LoadTalentSpec(Character, (string)comboBoxTalentPreset.SelectedItem);
            LoadCalculationOptions();
            Character.OnCalculationsInvalidated();
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e) {
            CalculationOptionsRogue options = Character.CalculationOptions as CalculationOptionsRogue;
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Name.Substring(8);
            options.SetTalentByName(talent, int.Parse(cb.SelectedItem.ToString()));
            if (!calculationSuspended) {
                Character.OnCalculationsInvalidated();
            }
        }

        private void OnLoad(object sender, EventArgs e) {
            LoadCalculationOptions();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) {
                e.Cancel = true;
                Visible = false;
            }
        }
    }
}
