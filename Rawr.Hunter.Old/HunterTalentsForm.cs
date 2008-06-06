using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Hunter
{
    public partial class HunterTalentsForm : Form
    {
        #region Instance Variables

        private CalculationOptionsPanelHunter basePanel;
        private bool calculationSuspended = false;

        #endregion

        #region Constructors

        public HunterTalentsForm(CalculationOptionsPanelHunter basePanel)
        {
            this.basePanel = basePanel;
            InitializeComponent();
        }

        #endregion
        
        #region Properties

        public Character Character
        {
            get { return basePanel.Character; }
        }

        #endregion

        #region Event Handlers

        private void HunterTalentsForm_Load(object sender, EventArgs e)
        {
            LoadCalculationOptions();
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string talent = cb.Name.Substring(8);
            Character.CalculationOptions[talent] = cb.SelectedItem.ToString();
            if (!calculationSuspended) 
                Character.OnItemsChanged();
        }

        private void HunterTalentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Visible = false;
            }
        }

        #endregion

        #region Instance Methods

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

        #endregion
    }
}
