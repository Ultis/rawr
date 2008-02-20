using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Moonkin
{
    public partial class MoonkinTalentsForm : Form
    {
        private CalculationOptionsPanelMoonkin basePanel;
        public MoonkinTalentsForm(CalculationOptionsPanelMoonkin basePanel)
        {
            this.basePanel = basePanel;
            InitializeComponent();
        }

        public Character Character
        {
            get
            {
                return basePanel.Character;
            }
        }

        private void MoonkinTalentsForm_Load(object sender, EventArgs e)
        {
            LoadCalculationOptions();
        }

        // Load talent points from a character's calculation options.
        public void LoadCalculationOptions()
        {
            // Iterate through all controls on the form
            foreach (Control c in Controls)
            {
                // Iterate into group boxes only
                if (c is GroupBox)
                {
                    // Iterate through all controls in the group box
                    foreach (Control innerControl in c.Controls)
                    {
                        // Load calculation options into combo boxes only
                        if (innerControl is ComboBox)
                        {
                            // Get the substring that is the actual talent name
                            ComboBox cb = (ComboBox)innerControl;
                            string talent = cb.Name.Substring(3);

                            // If the talent is not in the calculation options, add it
                            if (!Character.CalculationOptions.ContainsKey(talent))
                                Character.CalculationOptions[talent] = "0";

                            // Load the value from the character into the combo box
                            cb.SelectedItem = Character.CalculationOptions[talent];
                        }
                    }
                }
            }
        }

        // Update character calculation options when a talent point is set
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Name.Substring(3);
            Character.CalculationOptions[talent] = cb.SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        // Do not close the form on close; merely hide it
        private void MoonkinTalentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Visible = false;
            }
        }
    }
}
