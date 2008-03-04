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
    public partial class CalculationOptionsPanelRetribution : CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelRetribution()
        {
            InitializeComponent();
           
        }
        protected override void LoadCalculationOptions()
        {
            if (!Character.CalculationOptions.ContainsKey("TargetLevel"))
                Character.CalculationOptions["TargetLevel"] = "73";
            if (!Character.CalculationOptions.ContainsKey("BossArmor"))
                Character.CalculationOptions["BossArmor"] = "7700";
            if (!Character.CalculationOptions.ContainsKey("FightLength"))
                Character.CalculationOptions["FightLength"] = "10";
            if (!Character.CalculationOptions.ContainsKey("Exorcism"))
                Character.CalculationOptions["Exorcism"] = "0";
            if (!Character.CalculationOptions.ContainsKey("ConsecRank"))
                Character.CalculationOptions["ConsecRank"] = "0";
            if (!Character.CalculationOptions.ContainsKey("Seal"))
                Character.CalculationOptions["Seal"] = "1";
            if (!Character.CalculationOptions.ContainsKey("EnforceMetagemRequirements"))
                Character.CalculationOptions["EnforceMetagemRequirements"] = "No";

            comboBoxTargetLevel.SelectedItem = Character.CalculationOptions["TargetLevel"];
            txtArmor.Text = Character.CalculationOptions["BossArmor"];
            trackBarFightLength.Value = int.Parse(Character.CalculationOptions["FightLength"]);
            if (Character.CalculationOptions["Exorcism"] == "1")
            {
                checkBoxExorcism.Checked = true;
            }
            else
            {
                checkBoxExorcism.Checked = false;
            }
            if (Character.CalculationOptions["ConsecRank"] == "0")
            {
                checkBoxConsecration.Checked = false;
            }
            else
            {
                checkBoxConsecration.Checked = true;
                comboBoxConsRank.SelectedItem = "Rank " + Character.CalculationOptions["ConsecRank"];
            }
            if (Character.CalculationOptions["Seal"] == "1")
            {
                rbSoB.Checked = true;
            }
            else
            {
                rbSoC.Checked = true;
            }
            

        }
        

        private void rbSoC_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSoC.Checked)
            {
                Character.CalculationOptions["Seal"] = "0";
            }
            else
            {
                Character.CalculationOptions["Seal"] = "1";
            }
            Character.OnItemsChanged();
        }

        private void rbSoB_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSoB.Checked)
            {
                Character.CalculationOptions["Seal"] = "1";
            }
            else
            {
                Character.CalculationOptions["Seal"] = "0";
            }
            Character.OnItemsChanged();

        }

        private void checkBoxConsecration_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxConsecration.Checked)
            {
                comboBoxConsRank.Enabled = true;
                comboBoxConsRank.SelectedItem = "Rank 1";
                Character.CalculationOptions["ConsecRank"] = "1";
                
            }
            else
            {
                comboBoxConsRank.Enabled = false;
                Character.CalculationOptions["ConsecRank"] = "0";
            }
            Character.OnItemsChanged();
        }

        private void comboBoxConsRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["ConsecRank"] = comboBoxConsRank.SelectedItem.ToString().Substring(5, 1);
            Character.OnItemsChanged();
        }

        private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["TargetLevel"] = comboBoxTargetLevel.SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void trackBarFightLength_Scroll(object sender, EventArgs e)
        {
            Character.CalculationOptions["FightLength"] = trackBarFightLength.Value.ToString();
            Character.OnItemsChanged();
        }

        private void txtArmor_TextChanged(object sender, EventArgs e)
        {
            Character.CalculationOptions["BossArmor"] = txtArmor.Text;
            Character.OnItemsChanged();
        }

        private void checkBoxExorcism_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxExorcism.Checked)
            {
                Character.CalculationOptions["Exorcism"] = "1";
            }
            else
            {
                Character.CalculationOptions["Exorcism"] = "0";
            }
            Character.OnItemsChanged();
        }

        private void btnTalents_Click(object sender, EventArgs e)
        {
            Talents talents = new Talents(this);
            talents.Show();
        }
    }
}
