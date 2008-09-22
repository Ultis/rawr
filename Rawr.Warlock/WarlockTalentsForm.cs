using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Rawr.Warlock
{
    public partial class WarlockTalentsForm : Form
    {
        public WarlockTalentsForm(CalculationOptionsPanelWarlock basePanel)
        {
            this.basePanel = basePanel;
            InitializeComponent();
        }

        private CalculationOptionsPanelWarlock basePanel;

        public Character Character
        {
            get
            {
                return basePanel.Character;
            }
        }

        private void WarlockTalentsForm_Load(object sender, EventArgs e)
        {
            LoadCalculationOptions();
        }

        private bool calculationSuspended = false;

        public void LoadCalculationOptions()
        {
            CalculationOptionsWarlock calculationOptions = Character.CalculationOptions as CalculationOptionsWarlock;
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

                            string s = calculationOptions.GetTalentByName(talent).ToString();
                            cb.SelectedItem = calculationOptions.GetTalentByName(talent).ToString();
                        }
                    }
                }
            }
            calculationSuspended = false;
            ComputeTalentTotals();
        }

        private void ComputeTalentTotals()
        {
            CalculationOptionsWarlock calculationOptions = Character.CalculationOptions as CalculationOptionsWarlock;
            List<string> totals = new List<string>();
            foreach (Control c in Controls)
            {
                if (c is GroupBox)
                {
                    int total = 0;
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is ComboBox)
                        {
                            ComboBox cb = (ComboBox)cc;
                            string talent = cb.Name.Substring(8);
                            total += calculationOptions.GetTalentByName(talent);
                        }
                    }
                    totals.Add(total.ToString());
                }
            }
            totals.Reverse();
            Text = "Warlock Talents (" + string.Join("/", totals.ToArray()) + ")";
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock calculationOptions = Character.CalculationOptions as CalculationOptionsWarlock;
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Name.Substring(8);
            calculationOptions.SetTalentByName(talent, int.Parse(cb.SelectedItem.ToString()));
            if (!calculationSuspended)
            {
                Character.OnCalculationsInvalidated();
                ComputeTalentTotals();
            }
        }

        private void WarlockTalentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                basePanel.UpdateTalentOptions();
                Visible = false;
            }
        }

        private void buttonImportBlizzardCode_Click(object sender, EventArgs e)
        {
            string talentCode = textBoxBlizzardCode.Text;
            int index = talentCode.IndexOf('=');
            if (index >= 0) talentCode = talentCode.Substring(index + 1);

            CalculationsWarlock.LoadTalentCode(Character, talentCode);
            LoadCalculationOptions();
            Character.OnCalculationsInvalidated();
        }

        private void buttonImportTalentPreset_Click(object sender, EventArgs e)
        {
            CalculationsWarlock.LoadTalentSpec(Character, (string)comboBoxTalentPreset.SelectedItem);
            LoadCalculationOptions();
            Character.OnCalculationsInvalidated();
        }
    }
}