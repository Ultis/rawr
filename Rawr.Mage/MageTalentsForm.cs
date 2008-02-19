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

        public void LoadCalculationOptions()
        {
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