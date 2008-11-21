using System;
using System.Windows.Forms;

namespace Rawr.Tree
{
    public partial class SpellRotationEditor : Form
    {
        public Character character = null;

        private string[] spellComboBoxItems = new string[] 
        {
            "Healing Touch",
            "Regrowth",
            "Rejuvenation",
            "Lifebloom",
            "Nourish",
            "Wild Growth",
            //"Nothing"
        };

        public SpellRotationEditor()
        {
            InitializeComponent();
        }

        public void generateEditorContent(Character character)
        {
            if (this.character != character)
                this.character = character;

            generateListBoxItems();

            generatePanelContent();

            displayStatsofRota();
        }

        private void generateListBoxItems()
        {
            int selectedIndex = lbRotations.SelectedIndex;
            lbRotations.Items.Clear();

            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);

            int rotacount = 0;
            foreach (Spellrotation rota in calcOpts.Spellrotations)
            {
                if (rota.Name == "")
                    rota.Name = rotacount.ToString();

                string newItem = rota.Name;

                lbRotations.Items.Add(newItem);

                rotacount++;
            }

            if (selectedIndex == -1 && lbRotations.Items.Count > 0)
                lbRotations.SelectedIndex = 0;

            if (lbRotations.Items.Count > 0)
                lbRotations.SelectedIndex = selectedIndex; 
        }

        private void generatePanelContent()
        {
            panel1.Controls.Clear();

            if (lbRotations.Items.Count == 0 || lbRotations.SelectedIndex == -1)
                return;

            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);

            int rowcount = 0;
            float cycletime = 0f;
            Spellrotation currentRota = calcOpts.Spellrotations[lbRotations.SelectedIndex];
            foreach (Spellcast spellcast in currentRota.spells)
            {
                cycletime += currentRota.getSpellByName(spellcast.spellname, calcOpts.calculatedStats).CastTime;
 
                panel1.Controls.Add(newSelectButton(rowcount));
                panel1.Controls.Add(newUpButton(rowcount));
                panel1.Controls.Add(newDownButton(rowcount));
                panel1.Controls.Add(newSpellComboBox(rowcount, spellcast.spellname));
                panel1.Controls.Add(newTargetTextBox(rowcount, spellcast.target));
                panel1.Controls.Add(newTimeLabel(rowcount, cycletime));
                panel1.Controls.Add(newDurationLabel(rowcount));
                panel1.Controls.Add(newDeleteButton(rowcount));
                rowcount++;
            }
            panel1.Controls.Add(newAddButton(rowcount));

            recalulateTimes(currentRota);
        }

        private void recalulateTimes(Spellrotation currentRota)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);

            Label tmp;

            int rowcount = 0;
            float cycletime = 0f;
            foreach (Spellcast spellcast in currentRota.spells)
            {
                cycletime += currentRota.getSpellByName(spellcast.spellname, calcOpts.calculatedStats).CastTime;

                tmp = (Label)findControl(panel1, "Time" + rowcount);
                if (tmp != null)
                    tmp.Text = cycletime.ToString();

                rowcount++;
            }
        }

        private Control findControl(Panel panel1, string p)
        {
            foreach (Control c in panel1.Controls)
            {
                if (c.Name == p)
                    return c;
            }
            return null;
        }

        private Control newDeleteButton(int rowcount)
        {
            Button tmp = new Button();
            tmp.Size = new System.Drawing.Size(26, 23);
            tmp.Location = new System.Drawing.Point(380, 1 + rowcount * 29);
            tmp.Name = "Add";
            tmp.Text = "X";
            tmp.Tag = rowcount;
            tmp.Click += new EventHandler(genericDeleteButtonClickEventHandler);
            return tmp;
        }

        void genericDeleteButtonClickEventHandler(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);
            int rowcount = (int)((Button)sender).Tag;

            calcOpts.Spellrotations[lbRotations.SelectedIndex].spells.RemoveAt(rowcount);

            generatePanelContent();

            refreshTimer.Enabled = true;
            refreshTimer.Interval = 5000;
        }

        private Button newAddButton(int rowcount)
        {
            Button tmp = new Button();
            tmp.Size = new System.Drawing.Size(43, 23);
            tmp.Location = new System.Drawing.Point(131, 1 + rowcount * 29);
            tmp.Name = "Add";
            tmp.Text = "+";
            tmp.Tag = rowcount;
            tmp.Click += new EventHandler(genericAddButtonClickEventHandler);
            return tmp;
        }

        private void genericAddButtonClickEventHandler(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);
            Button bSender = (Button)sender;

            Spellcast tmp = new Spellcast();
            tmp.spellname = "Healing Touch";
            tmp.target = "1";

            calcOpts.Spellrotations[lbRotations.SelectedIndex].spells.Add(tmp);

            generatePanelContent();

            refreshTimer.Enabled = true;
            refreshTimer.Interval = 5000;
        }

        private Label newDurationLabel(int rowcount)
        {
            Label tmp = new Label();
            tmp.Size = new System.Drawing.Size(45, 13);
            tmp.Location = new System.Drawing.Point(329, 6 + rowcount * 29);
            tmp.Name = "Duration" + rowcount;
            tmp.Text = "NYI";
            tmp.Tag = rowcount;
            tmp.Visible = false;
            return tmp;
        }

        private Label newTimeLabel(int rowcount, float cycletime)
        {
            Label tmp = new Label();
            tmp.Size = new System.Drawing.Size(39, 13);
            tmp.Location = new System.Drawing.Point(278, 6 + rowcount * 29);
            tmp.Name = "Time" + rowcount;
            tmp.Text = Math.Round(cycletime, 1) + "sec";
            tmp.Tag = rowcount;
            return tmp;
        }

        private TextBox newTargetTextBox(int rowcount, string target)
        {
            TextBox tmp = new TextBox();
            tmp.Size = new System.Drawing.Size(35, 20);
            tmp.Location = new System.Drawing.Point(237, 3 + rowcount * 29);
            tmp.Name = "Target" + rowcount;
            tmp.Text = target;
            tmp.Tag = rowcount;
            tmp.TextChanged += new EventHandler(genericTargetTextBoxEventHandler);
            return tmp;
        }

        private void genericTargetTextBoxEventHandler(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);
            TextBox tbSender = (TextBox)sender;

            calcOpts.Spellrotations[lbRotations.SelectedIndex].spells[(int)tbSender.Tag].target = tbSender.Text;

            refreshTimer.Enabled = true;
            refreshTimer.Interval = 5000;
        }

        private ComboBox newSpellComboBox(int rowcount, string selectedSpell)
        {
            ComboBox tmp = new ComboBox();
            tmp.Size = new System.Drawing.Size(134, 21);
            tmp.Location = new System.Drawing.Point(97, 3 + rowcount * 29);
            tmp.Name = "Spell" + rowcount;
            tmp.Tag = rowcount;
            foreach (string str in spellComboBoxItems)
                tmp.Items.Add(str);
            tmp.SelectedIndex = tmp.Items.IndexOf(selectedSpell);
            //TODO: Spell-Items
            tmp.SelectedIndexChanged += new EventHandler(genericSpellComboBoxIndexChangedEventHandler);
            return tmp;
        }

        private void genericSpellComboBoxIndexChangedEventHandler(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);
            ComboBox cbSender = (ComboBox)sender;
            calcOpts.Spellrotations[lbRotations.SelectedIndex].spells[(int)cbSender.Tag].spellname = (string)cbSender.Items[cbSender.SelectedIndex];
            recalulateTimes(calcOpts.Spellrotations[lbRotations.SelectedIndex]);

            refreshTimer.Enabled = true;
            refreshTimer.Interval = 5000;
        }

        private Button newDownButton(int rowcount)
        {
            Button tmp = new Button();
            tmp.Size = new System.Drawing.Size(26, 23);
            tmp.Location = new System.Drawing.Point(65, 1 + rowcount * 29);
            tmp.Name = "Down" + rowcount;
            tmp.Text = " V";
            tmp.Tag = rowcount;
            tmp.Click += new EventHandler(genericDownButtonClickEventHandler);
            return tmp;
        }

        private void genericDownButtonClickEventHandler(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);
            int rowcount = (int)((Button)sender).Tag;

            if (rowcount < calcOpts.Spellrotations[lbRotations.SelectedIndex].spells.Count - 1)
            {
                Spellcast tmp = calcOpts.Spellrotations[lbRotations.SelectedIndex].spells[rowcount];
                calcOpts.Spellrotations[lbRotations.SelectedIndex].spells.Remove(tmp);
                calcOpts.Spellrotations[lbRotations.SelectedIndex].spells.Insert(rowcount+1, tmp);
            }
            generatePanelContent();

            refreshTimer.Enabled = true;
            refreshTimer.Interval = 5000;
        }

        private Button newUpButton(int rowcount)
        {
            Button tmp = new Button();
            tmp.Size = new System.Drawing.Size(26, 23);
            tmp.Location = new System.Drawing.Point(33, 1 + rowcount * 29);
            tmp.Name = "Up" + rowcount;
            tmp.Text = " Λ";
            tmp.Tag = rowcount;
            tmp.Click += new EventHandler(genericUpButtonEventHandler);
            return tmp;
        }

        private void genericUpButtonEventHandler(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);
            int rowcount = (int)((Button)sender).Tag;

            if (rowcount > 0)
            {
                Spellcast tmp = calcOpts.Spellrotations[lbRotations.SelectedIndex].spells[rowcount];
                calcOpts.Spellrotations[lbRotations.SelectedIndex].spells.Remove(tmp);
                calcOpts.Spellrotations[lbRotations.SelectedIndex].spells.Insert(rowcount - 1, tmp);
            }

            generatePanelContent();

            refreshTimer.Enabled = true;
            refreshTimer.Interval = 5000;
        }

        private Control newSelectButton(int rowcount)
        {
            Button tmp = new Button();
            tmp.Size = new System.Drawing.Size(24, 23);
            tmp.Location = new System.Drawing.Point(3, 1 + rowcount * 29);
            tmp.Name = "Select" + rowcount;
            tmp.Text = ">";
            tmp.Tag = rowcount;
            tmp.Click += new EventHandler(genericSelectButtonClickHandlerEvent);
            tmp.Visible = false;
            return tmp;
        }

        private void genericSelectButtonClickHandlerEvent(object sender, EventArgs e)
        {

        }

        private void SpellRotationEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void lbRotations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbRotations.SelectedIndex > -1)
            {
                tbNameEditor.Text = lbRotations.Items[lbRotations.SelectedIndex].ToString();
                generatePanelContent();
            }
        }

        private void bRotaUp_Click(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);

            if (lbRotations.SelectedIndex == 0)
                return;

            Spellrotation tmp = calcOpts.Spellrotations[lbRotations.SelectedIndex];
            calcOpts.Spellrotations.Remove(tmp);
            calcOpts.Spellrotations.Insert(lbRotations.SelectedIndex - 1, tmp);

            generateListBoxItems();

            refreshTimer.Enabled = true;
        }

        private void bRotaDown_Click(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);

            if (lbRotations.SelectedIndex == lbRotations.Items.Count - 1)
                return;

            Spellrotation tmp = calcOpts.Spellrotations[lbRotations.SelectedIndex];
            calcOpts.Spellrotations.Remove(tmp);
            calcOpts.Spellrotations.Insert(lbRotations.SelectedIndex, tmp);

            generateListBoxItems();
        }

        private void bRotaNew_Click(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);

            Spellrotation tmp = new Spellrotation();
            tmp.Name = lbRotations.Items.Count.ToString();
            calcOpts.Spellrotations.Add(tmp);
            lbRotations.SelectedIndex = lbRotations.Items.Count - 1;

            generateListBoxItems();
        }

        private void bRotaDelete_Click(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);

            if (lbRotations.SelectedIndex == 0)
                return;

            calcOpts.Spellrotations.RemoveAt(lbRotations.SelectedIndex);

            generateListBoxItems();

            refreshTimer.Enabled = true;
            refreshTimer.Interval = 5000;
        }

        private void tbNameEditor_TextChanged(object sender, EventArgs e)
        {
            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);

            calcOpts.Spellrotations[lbRotations.SelectedIndex].Name = tbNameEditor.Text;

            generateListBoxItems();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            if (lbRotations.SelectedIndex == 0)
                character.OnCalculationsInvalidated();
                
            refreshTimer.Enabled = false;
            displayStatsofRota();
        }

        private void displayStatsofRota()
        {
            if (lbRotations.Items.Count == 0 || lbRotations.SelectedIndex == -1)
                return;

            CalculationOptionsTree calcOpts = ((CalculationOptionsTree)character.CalculationOptions);

            Spellrotation currentRota = calcOpts.Spellrotations[lbRotations.SelectedIndex];
            currentRota.CalculateSpellRotaion(calcOpts.calculatedStats);

            lHPS.Text = Math.Round(currentRota.HPS, 1).ToString();
            lTime2OOM.Text = Math.Round(calcOpts.calculatedStats.BasicStats.Mana * 5 / (currentRota.manaPerSecond * 5 - calcOpts.calculatedStats.ManaRegInFSR), 1) + "sec";
        }
    }
}
