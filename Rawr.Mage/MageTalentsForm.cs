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

            LoadTalentCode(talentCode);
        }

        private void buttonImportTalentPreset_Click(object sender, EventArgs e)
        {
            string talentCode = null;
            switch ((string)comboBoxTalentPreset.SelectedItem)
            {
                case "Fire (2/48/11)":
                    talentCode = "2000000000000000000000050520201230333115312510532000010000000000000";
                    break;
                case "Fire/Cold Snap (0/40/21)":
                    talentCode = "0000000000000000000000050510200230233005112500535000310030010000000";
                    break;
                case "Frost (10/0/51)":
                    talentCode = "2300050000000000000000000000000000000000000000535020313235010051551";
                    break;
                case "Arcane (48/0/13)":
                    talentCode = "2550050300230150333125000000000000000000000000534000010000000000000";
                    break;
                case "Arcane/Fire (40/18/3)":
                    talentCode = "2500050300230150330125050520001230000000000000030000000000000000000";
                    break;
                case "Arcane/Frost (40/0/21)":
                    talentCode = "2520050300030150330125000000000000000000000000535020310010010000000";
                    break;
            }

            LoadTalentCode(talentCode);
        }

        private void LoadTalentCode(string talentCode)
        {
            if (talentCode == null || talentCode.Length < 66) return;

            Character.CalculationOptions["ArcaneSubtlety"] = talentCode.Substring(0, 1);
            Character.CalculationOptions["ArcaneFocus"] = talentCode.Substring(1, 1);
            Character.CalculationOptions["ImprovedArcaneMissiles"] = talentCode.Substring(2, 1);
            Character.CalculationOptions["WandSpecialization"] = talentCode.Substring(3, 1);
            Character.CalculationOptions["MagicAbsorption"] = talentCode.Substring(4, 1);
            Character.CalculationOptions["ArcaneConcentration"] = talentCode.Substring(5, 1);
            Character.CalculationOptions["MagicAttunement"] = talentCode.Substring(6, 1);
            Character.CalculationOptions["ArcaneImpact"] = talentCode.Substring(7, 1);
            Character.CalculationOptions["ArcaneFortitude"] = talentCode.Substring(8, 1);
            Character.CalculationOptions["ImprovedManaShield"] = talentCode.Substring(9, 1);
            Character.CalculationOptions["ImprovedCounterspell"] = talentCode.Substring(10, 1);
            Character.CalculationOptions["ArcaneMeditation"] = talentCode.Substring(11, 1);
            Character.CalculationOptions["ImprovedBlink"] = talentCode.Substring(12, 1);
            Character.CalculationOptions["PresenceOfMind"] = talentCode.Substring(13, 1);
            Character.CalculationOptions["ArcaneMind"] = talentCode.Substring(14, 1);
            Character.CalculationOptions["PrismaticCloak"] = talentCode.Substring(15, 1);
            Character.CalculationOptions["ArcaneInstability"] = talentCode.Substring(16, 1);
            Character.CalculationOptions["ArcanePotency"] = talentCode.Substring(17, 1);
            Character.CalculationOptions["EmpoweredArcaneMissiles"] = talentCode.Substring(18, 1);
            Character.CalculationOptions["ArcanePower"] = talentCode.Substring(19, 1);
            Character.CalculationOptions["SpellPower"] = talentCode.Substring(20, 1);
            Character.CalculationOptions["MindMastery"] = talentCode.Substring(21, 1);
            Character.CalculationOptions["Slow"] = talentCode.Substring(22, 1);
            Character.CalculationOptions["ImprovedFireball"] = talentCode.Substring(23, 1);
            Character.CalculationOptions["Impact"] = talentCode.Substring(24, 1);
            Character.CalculationOptions["Ignite"] = talentCode.Substring(25, 1);
            Character.CalculationOptions["FlameThrowing"] = talentCode.Substring(26, 1);
            Character.CalculationOptions["ImprovedFireBlast"] = talentCode.Substring(27, 1);
            Character.CalculationOptions["Incinerate"] = talentCode.Substring(28, 1);
            Character.CalculationOptions["ImprovedFlamestrike"] = talentCode.Substring(29, 1);
            Character.CalculationOptions["Pyroblast"] = talentCode.Substring(30, 1);
            Character.CalculationOptions["BurningSoul"] = talentCode.Substring(31, 1);
            Character.CalculationOptions["ImprovedScorch"] = talentCode.Substring(32, 1);
            Character.CalculationOptions["ImprovedFireWard"] = talentCode.Substring(33, 1);
            Character.CalculationOptions["MasterOfElements"] = talentCode.Substring(34, 1);
            Character.CalculationOptions["PlayingWithFire"] = talentCode.Substring(35, 1);
            Character.CalculationOptions["CriticalMass"] = talentCode.Substring(36, 1);
            Character.CalculationOptions["BlastWave"] = talentCode.Substring(37, 1);
            Character.CalculationOptions["BlazingSpeed"] = talentCode.Substring(38, 1);
            Character.CalculationOptions["FirePower"] = talentCode.Substring(39, 1);
            Character.CalculationOptions["Pyromaniac"] = talentCode.Substring(40, 1);
            Character.CalculationOptions["Combustion"] = talentCode.Substring(41, 1);
            Character.CalculationOptions["MoltenFury"] = talentCode.Substring(42, 1);
            Character.CalculationOptions["EmpoweredFireball"] = talentCode.Substring(43, 1);
            Character.CalculationOptions["DragonsBreath"] = talentCode.Substring(44, 1);
            Character.CalculationOptions["FrostWarding"] = talentCode.Substring(45, 1);
            Character.CalculationOptions["ImprovedFrostbolt"] = talentCode.Substring(46, 1);
            Character.CalculationOptions["ElementalPrecision"] = talentCode.Substring(47, 1);
            Character.CalculationOptions["IceShards"] = talentCode.Substring(48, 1);
            Character.CalculationOptions["Frostbite"] = talentCode.Substring(49, 1);
            Character.CalculationOptions["ImprovedFrostNova"] = talentCode.Substring(50, 1);
            Character.CalculationOptions["Permafrost"] = talentCode.Substring(51, 1);
            Character.CalculationOptions["PiercingIce"] = talentCode.Substring(52, 1);
            Character.CalculationOptions["IcyVeins"] = talentCode.Substring(53, 1);
            Character.CalculationOptions["ImprovedBlizzard"] = talentCode.Substring(54, 1);
            Character.CalculationOptions["ArcticReach"] = talentCode.Substring(55, 1);
            Character.CalculationOptions["FrostChanneling"] = talentCode.Substring(56, 1);
            Character.CalculationOptions["Shatter"] = talentCode.Substring(57, 1);
            Character.CalculationOptions["FrozenCore"] = talentCode.Substring(58, 1);
            Character.CalculationOptions["ColdSnap"] = talentCode.Substring(59, 1);
            Character.CalculationOptions["ImprovedConeOfCold"] = talentCode.Substring(60, 1);
            Character.CalculationOptions["IceFloes"] = talentCode.Substring(61, 1);
            Character.CalculationOptions["WintersChill"] = talentCode.Substring(62, 1);
            Character.CalculationOptions["IceBarrier"] = talentCode.Substring(63, 1);
            Character.CalculationOptions["ArcticWinds"] = talentCode.Substring(64, 1);
            Character.CalculationOptions["EmpoweredFrostbolt"] = talentCode.Substring(65, 1);
            Character.CalculationOptions["SummonWaterElemental"] = talentCode.Substring(66, 1);

            LoadCalculationOptions();
            Character.OnItemsChanged();
        }
    }
}