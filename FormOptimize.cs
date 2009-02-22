using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class FormOptimize : Form
	{
		private Character _character;
        private Optimizer _optimizer;
        private Item _itemToEvaluate;

        public void EvaluateUpgrades(Item itemToEvaluate)
        {
            _itemToEvaluate = itemToEvaluate;
            buttonUpgrades_Click(null, null);
            _itemToEvaluate = null;
        }

		public FormOptimize(Character character)
		{
			InitializeComponent();
			_character = character;
            _optimizer = new Optimizer();
            _optimizer.OptimizeCharacterProgressChanged += new OptimizeCharacterProgressChangedEventHandler(_optimizer_OptimizeCharacterProgressChanged);
            _optimizer.OptimizeCharacterCompleted += new OptimizeCharacterCompletedEventHandler(_optimizer_OptimizeCharacterCompleted);
            _optimizer.ComputeUpgradesProgressChanged += new ComputeUpgradesProgressChangedEventHandler(_optimizer_ComputeUpgradesProgressChanged);
            _optimizer.ComputeUpgradesCompleted += new ComputeUpgradesCompletedEventHandler(_optimizer_ComputeUpgradesCompleted);

			comboBoxCalculationToOptimize.Items.Add("Overall Rating");
			comboBoxCalculationToOptimize.Tag = Calculations.SubPointNameColors.Count;
			foreach (string subPoint in Calculations.SubPointNameColors.Keys)
				comboBoxCalculationToOptimize.Items.Add(subPoint + " Rating");
			comboBoxCalculationToOptimize.Items.AddRange(Calculations.OptimizableCalculationLabels);
			comboBoxCalculationToOptimize.SelectedIndex = 0;

            checkBoxOverrideRegem.Checked = Properties.Optimizer.Default.OverrideRegem;
            checkBoxOverrideReenchant.Checked = Properties.Optimizer.Default.OverrideReenchant;
            trackBarThoroughness.Value = Properties.Optimizer.Default.Thoroughness;
            string calculationString = character.CalculationToOptimize;
            if (string.IsNullOrEmpty(calculationString)) calculationString = Properties.Optimizer.Default.CalculationToOptimize;
            if (calculationString != null)
            {
                if (calculationString.StartsWith("[Overall]"))
                {
                    comboBoxCalculationToOptimize.SelectedIndex = 0;
                }
                else if (calculationString.StartsWith("[SubPoint "))
                {
                    calculationString = calculationString.Substring(10).TrimEnd(']');
                    int index = int.Parse(calculationString);
                    if (index < Calculations.SubPointNameColors.Count)
                    {
                        comboBoxCalculationToOptimize.SelectedIndex = index + 1;
                    }
                }
                else
                {
                    if (Array.IndexOf(Calculations.OptimizableCalculationLabels, calculationString) >= 0)
                    {
                        comboBoxCalculationToOptimize.SelectedItem = calculationString;
                    }
                }
            }
            if (character.OptimizationRequirements != null)
            {
                for (int i = 0; i < character.OptimizationRequirements.Count; i++) buttonAddRequirement_Click(null, null);
                int reqIndex = 0;
                foreach (Control ctrl in groupBoxRequirements.Controls)
                {
                    if (ctrl is Panel)
                    {
                        foreach (Control reqCtrl in ctrl.Controls)
                        {
                            switch (reqCtrl.Name)
                            {
                                case "comboBoxRequirementCalculation":
                                    ComboBox reqComboBox = (ComboBox)reqCtrl;
                                    calculationString = character.OptimizationRequirements[reqIndex].Calculation;
                                    if (calculationString.StartsWith("[Overall]"))
                                    {
                                        reqComboBox.SelectedIndex = 0;
                                    }
                                    else if (calculationString.StartsWith("[SubPoint "))
                                    {
                                        calculationString = calculationString.Substring(10).TrimEnd(']');
                                        int index = int.Parse(calculationString);
                                        if (index < Calculations.SubPointNameColors.Count)
                                        {
                                            reqComboBox.SelectedIndex = index + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (Array.IndexOf(Calculations.OptimizableCalculationLabels, calculationString) >= 0)
                                        {
                                            reqComboBox.SelectedItem = calculationString;
                                        }
                                    }
                                    break;

                                case "comboBoxRequirementGreaterLessThan":
                                    (reqCtrl as ComboBox).SelectedIndex = character.OptimizationRequirements[reqIndex].LessThan ? 1 : 0;
                                    break;

                                case "numericUpDownRequirementValue":
                                    (reqCtrl as NumericUpDown).Value = (decimal)character.OptimizationRequirements[reqIndex].Value;
                                    break;
                            }
                        }
                        reqIndex++;
                    }
                }
            }
		}

		private void FormOptimize_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = _optimizer.IsBusy;
		}

		private void buttonOptimize_Click(object sender, EventArgs e)
		{
            bool _overrideRegem = checkBoxOverrideRegem.Checked;
            bool _overrideReenchant = checkBoxOverrideReenchant.Checked;
            int _thoroughness = trackBarThoroughness.Value;
			string _calculationToOptimize = GetCalculationStringFromComboBox(comboBoxCalculationToOptimize);
            List<OptimizationRequirement> requirements = new List<OptimizationRequirement>();
			foreach (Control ctrl in groupBoxRequirements.Controls)
			{
				if (ctrl is Panel) 
				{
                    OptimizationRequirement requirement = new OptimizationRequirement();
					foreach (Control reqCtrl in ctrl.Controls)
					{
						switch (reqCtrl.Name)
						{
							case "comboBoxRequirementCalculation":
								requirement.Calculation = GetCalculationStringFromComboBox(reqCtrl as ComboBox);
								break;

							case "comboBoxRequirementGreaterLessThan":
								requirement.LessThan = (reqCtrl as ComboBox).SelectedIndex == 1;
								break;

							case "numericUpDownRequirementValue":
								requirement.Value = (float)((reqCtrl as NumericUpDown).Value);
								break;
						}
					}
					requirements.Add(requirement);
				}
			}
            OptimizationRequirement[] _requirements = requirements.ToArray();

            _optimizer.InitializeItemCache(_character, _character.AvailableItems, _overrideRegem, _overrideReenchant, Properties.Optimizer.Default.TemplateGemsEnabled, Calculations.Instance);
            if (Properties.Optimizer.Default.WarningsEnabled)
            {
                string prompt = _optimizer.GetWarningPromptIfNeeded();
                if (prompt != null)
                {
                    if (MessageBox.Show(prompt, "Optimizer Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                }
            }

            buttonOptimize.Text = "Optimizing...";
            buttonOptimize.Enabled = buttonUpgrades.Enabled = checkBoxOverrideRegem.Enabled = checkBoxOverrideReenchant.Enabled =
                trackBarThoroughness.Enabled = false;
            buttonCancel.DialogResult = DialogResult.None;

            Optimizer.OptimizationMethod = Properties.Optimizer.Default.OptimizationMethod;
            _optimizer.OptimizeCharacterAsync(_character, _calculationToOptimize, _requirements, _thoroughness, false);
		}

		private string GetCalculationStringFromComboBox(ComboBox comboBox)
		{
			if (comboBox.SelectedIndex == 0)
				return "[Overall]";
			else if (comboBox.SelectedIndex <= (int)comboBox.Tag)
				return string.Format("[SubPoint {0}]", comboBox.SelectedIndex - 1);
			else
				return comboBox.Text;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			if (_optimizer.IsBusy) _optimizer.CancelAsync();
		}

		void _optimizer_OptimizeCharacterProgressChanged(object sender, OptimizeCharacterProgressChangedEventArgs e)
		{
			labelMax.Text = e.BestValue.ToString();
			progressBarAlt.Value = e.ProgressPercentage;
			progressBarMain.Value = Math.Max(e.ProgressPercentage, progressBarMain.Value);

            Text = string.Format("{0}% Complete - Rawr Optimizer", progressBarMain.Value);
		}

        void _optimizer_OptimizeCharacterCompleted(object sender, OptimizeCharacterCompletedEventArgs e)
		{
            buttonCancel.DialogResult = DialogResult.Cancel;
			if (e.Cancelled)
			{
				labelMax.Text = string.Empty;
				buttonOptimize.Text = "Optimize";
                buttonOptimize.Enabled = buttonUpgrades.Enabled = checkBoxOverrideRegem.Enabled = checkBoxOverrideReenchant.Enabled =
				 trackBarThoroughness.Enabled = true;
				progressBarAlt.Value = progressBarMain.Value = 0;
			}
			else
			{
				progressBarAlt.Value = progressBarMain.Value = 100;
				Character bestCharacter = e.OptimizedCharacter;
				if (bestCharacter == null)
				{
					labelMax.Text = string.Empty;
					buttonOptimize.Text = "Optimize";
                    buttonOptimize.Enabled = buttonUpgrades.Enabled = checkBoxOverrideRegem.Enabled = checkBoxOverrideReenchant.Enabled =
					 trackBarThoroughness.Enabled = true;
					progressBarAlt.Value = progressBarMain.Value = 0;
					MessageBox.Show(this,"Sorry, Rawr was unable to find a gearset to meet your requirements.", "Rawr Optimizer Results");
				}

				if (_character != null && ((e.OptimizedCharacterValue >= 0 && MessageBox.Show(this,string.Format("The Optimizer found a gearset with a score of {0}. (Your currently equipped gear has a score of {1}) Would you like to equip the optimized gear?",
					e.OptimizedCharacterValue,
					e.CurrentCharacterValue),
                    "Rawr Optimizer Results", MessageBoxButtons.YesNo) == DialogResult.Yes) || (e.OptimizedCharacterValue < 0 && MessageBox.Show(this, "The Optimizer was not able to meet all the requirements. Would you like to equip the gear that is closest to meeting them?",
                    "Rawr Optimizer Results", MessageBoxButtons.YesNo) == DialogResult.Yes)))
				{
					//Loading new items while IsLoading==true causes properties to be reset to their previously cached values, 
					//so load all the items beforehand, then put them into the character all at once.
					ItemInstance itemBack = bestCharacter.Back == null ? null : bestCharacter.Back.Clone();
                    ItemInstance itemChest = bestCharacter.Chest == null ? null : bestCharacter.Chest.Clone();
                    ItemInstance itemFeet = bestCharacter.Feet == null ? null : bestCharacter.Feet.Clone();
                    ItemInstance itemFinger1 = bestCharacter.Finger1 == null ? null : bestCharacter.Finger1.Clone();
                    ItemInstance itemFinger2 = bestCharacter.Finger2 == null ? null : bestCharacter.Finger2.Clone();
                    ItemInstance itemHands = bestCharacter.Hands == null ? null : bestCharacter.Hands.Clone();
                    ItemInstance itemHead = bestCharacter.Head == null ? null : bestCharacter.Head.Clone();
                    ItemInstance itemLegs = bestCharacter.Legs == null ? null : bestCharacter.Legs.Clone();
                    ItemInstance itemMainHand = bestCharacter.MainHand == null ? null : bestCharacter.MainHand.Clone();
                    ItemInstance itemNeck = bestCharacter.Neck == null ? null : bestCharacter.Neck.Clone();
                    ItemInstance itemOffHand = bestCharacter.OffHand == null ? null : bestCharacter.OffHand.Clone();
                    ItemInstance itemProjectile = bestCharacter.Projectile == null ? null : bestCharacter.Projectile.Clone();
                    ItemInstance itemProjectileBag = bestCharacter.ProjectileBag == null ? null : bestCharacter.ProjectileBag.Clone();
                    ItemInstance itemRanged = bestCharacter.Ranged == null ? null : bestCharacter.Ranged.Clone();
                    ItemInstance itemShoulders = bestCharacter.Shoulders == null ? null : bestCharacter.Shoulders.Clone();
                    ItemInstance itemTrinket1 = bestCharacter.Trinket1 == null ? null : bestCharacter.Trinket1.Clone();
                    ItemInstance itemTrinket2 = bestCharacter.Trinket2 == null ? null : bestCharacter.Trinket2.Clone();
                    ItemInstance itemWaist = bestCharacter.Waist == null ? null : bestCharacter.Waist.Clone();
                    ItemInstance itemWrist = bestCharacter.Wrist == null ? null : bestCharacter.Wrist.Clone();
					
					_character.IsLoading = true;
					_character.Back = itemBack;
					_character.Chest = itemChest;
					_character.Feet = itemFeet;
					_character.Finger1 = itemFinger1;
					_character.Finger2 = itemFinger2;
					_character.Hands = itemHands;
					_character.Head = itemHead;
					_character.Legs = itemLegs;
					_character.MainHand = itemMainHand;
					_character.Neck = itemNeck;
					_character.OffHand = itemOffHand;
					_character.Projectile = itemProjectile;
					_character.ProjectileBag = itemProjectileBag;
					_character.Ranged = itemRanged;
					_character.Shoulders = itemShoulders;
					_character.Trinket1 = itemTrinket1;
					_character.Trinket2 = itemTrinket2;
					_character.Waist = itemWaist;
					_character.Wrist = itemWrist;
                    //_character.BackEnchant = bestCharacter.BackEnchant;
                    //_character.ChestEnchant = bestCharacter.ChestEnchant;
                    //_character.FeetEnchant = bestCharacter.FeetEnchant;
                    //_character.Finger1Enchant = bestCharacter.Finger1Enchant;
                    //_character.Finger2Enchant = bestCharacter.Finger2Enchant;
                    //_character.HandsEnchant = bestCharacter.HandsEnchant;
                    //_character.HeadEnchant = bestCharacter.HeadEnchant;
                    //_character.LegsEnchant = bestCharacter.LegsEnchant;
                    //_character.MainHandEnchant = bestCharacter.MainHandEnchant;
                    //_character.OffHandEnchant = bestCharacter.OffHandEnchant;
                    //_character.RangedEnchant = bestCharacter.RangedEnchant;
                    //_character.ShouldersEnchant = bestCharacter.ShouldersEnchant;
                    //_character.WristEnchant = bestCharacter.WristEnchant;
					_character.IsLoading = false;
					_character.OnCalculationsInvalidated();
					Close();
				}
				else
				{
					labelMax.Text = string.Empty;
					buttonOptimize.Text = "Optimize";
                    buttonOptimize.Enabled = buttonUpgrades.Enabled = checkBoxOverrideRegem.Enabled = checkBoxOverrideReenchant.Enabled =
					 trackBarThoroughness.Enabled = true;
					progressBarAlt.Value = progressBarMain.Value = 0;
				}
			}
		}

        void _optimizer_ComputeUpgradesProgressChanged(object sender, ComputeUpgradesProgressChangedEventArgs e)
        {
            labelMax.Text = e.CurrentItem;
            progressBarAlt.Value = e.ItemProgressPercentage;
            progressBarMain.Value = e.ProgressPercentage;

            Text = string.Format("{0}% Complete - Rawr Optimizer", progressBarMain.Value);
        }

        void _optimizer_ComputeUpgradesCompleted(object sender, ComputeUpgradesCompletedEventArgs e)
        {
            buttonCancel.DialogResult = DialogResult.Cancel;
            if (e.Cancelled)
            {
                labelMax.Text = string.Empty;
                buttonUpgrades.Text = "Build Upgrade List";
                buttonOptimize.Enabled = buttonUpgrades.Enabled = checkBoxOverrideRegem.Enabled = checkBoxOverrideReenchant.Enabled =
                 trackBarThoroughness.Enabled = true;
                progressBarAlt.Value = progressBarMain.Value = 0;
            }
            else
            {
                progressBarAlt.Value = progressBarMain.Value = 100;
                FormUpgradeComparison.Instance.LoadData(_character, e.Upgrades, null);
                FormUpgradeComparison.Instance.Show();
                Close();
                //FormUpgradeComparison.Instance.BringToFront();
            }
        }

		private void buttonAddRequirement_Click(object sender, EventArgs e)
		{
			buttonAddRequirement.Top += 29;

			Panel panelRequirement = new System.Windows.Forms.Panel();
			ComboBox comboBoxRequirementCalculation = new System.Windows.Forms.ComboBox();
			ComboBox comboBoxRequirementGreaterLessThan = new System.Windows.Forms.ComboBox();
			NumericUpDown numericUpDownRequirementValue = new System.Windows.Forms.NumericUpDown();
			Button buttonRemoveRequirement = new System.Windows.Forms.Button();
			panelRequirement.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(numericUpDownRequirementValue)).BeginInit();
			// 
			// panelRequirement
			// 
			panelRequirement.Controls.Add(numericUpDownRequirementValue);
			panelRequirement.Controls.Add(buttonRemoveRequirement);
			panelRequirement.Controls.Add(comboBoxRequirementGreaterLessThan);
			panelRequirement.Controls.Add(comboBoxRequirementCalculation);
			panelRequirement.Dock = System.Windows.Forms.DockStyle.Top;
			panelRequirement.Location = new System.Drawing.Point(3, 16);
			panelRequirement.Name = "panelRequirement";
			panelRequirement.Size = new System.Drawing.Size(294, 29);
			panelRequirement.TabIndex = 6;
			// 
			// comboBoxRequirementCalculation
			// 
			comboBoxRequirementCalculation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			comboBoxRequirementCalculation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxRequirementCalculation.FormattingEnabled = true;
			comboBoxRequirementCalculation.Location = new System.Drawing.Point(64, 4);
			comboBoxRequirementCalculation.Name = "comboBoxRequirementCalculation";
			comboBoxRequirementCalculation.Size = new System.Drawing.Size(125, 21);
			comboBoxRequirementCalculation.TabIndex = 3;
			// 
			// comboBoxRequirementGreaterLessThan
			// 
			comboBoxRequirementGreaterLessThan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			comboBoxRequirementGreaterLessThan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBoxRequirementGreaterLessThan.Items.AddRange(new object[] {
            "≥",
            "≤"});
			comboBoxRequirementGreaterLessThan.Location = new System.Drawing.Point(195, 4);
			comboBoxRequirementGreaterLessThan.Name = "comboBoxRequirementGreaterLessThan";
			comboBoxRequirementGreaterLessThan.Size = new System.Drawing.Size(30, 21);
			comboBoxRequirementGreaterLessThan.TabIndex = 3;
			// 
			// numericUpDownRequirementValue
			// 
			numericUpDownRequirementValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			numericUpDownRequirementValue.Location = new System.Drawing.Point(231, 5);
			numericUpDownRequirementValue.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			numericUpDownRequirementValue.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
			numericUpDownRequirementValue.Name = "numericUpDownRequirementValue";
			numericUpDownRequirementValue.Size = new System.Drawing.Size(60, 20);
			numericUpDownRequirementValue.TabIndex = 6;
			numericUpDownRequirementValue.ThousandsSeparator = true;
			// 
			// buttonRemoveRequirement
			// 
			buttonRemoveRequirement.Location = new System.Drawing.Point(3, 3);
			buttonRemoveRequirement.Name = "buttonRemoveRequirement";
			buttonRemoveRequirement.Size = new System.Drawing.Size(55, 23);
			buttonRemoveRequirement.TabIndex = 5;
			buttonRemoveRequirement.Text = "Remove";
			buttonRemoveRequirement.UseVisualStyleBackColor = true;
			buttonRemoveRequirement.Click += new EventHandler(buttonRemoveRequirement_Click);


			comboBoxRequirementCalculation.Items.Add("Overall Rating");
			comboBoxRequirementCalculation.Tag = Calculations.SubPointNameColors.Count;
			foreach (string subPoint in Calculations.SubPointNameColors.Keys)
				comboBoxRequirementCalculation.Items.Add(subPoint + " Rating");
			comboBoxRequirementCalculation.Items.AddRange(Calculations.OptimizableCalculationLabels);
			
			comboBoxRequirementCalculation.SelectedIndex = comboBoxRequirementGreaterLessThan.SelectedIndex = 0;
			groupBoxRequirements.Controls.Add(panelRequirement);
            ((System.ComponentModel.ISupportInitialize)(numericUpDownRequirementValue)).EndInit();
            panelRequirement.ResumeLayout();
            panelRequirement.BringToFront();
		}

		void buttonRemoveRequirement_Click(object sender, EventArgs e)
		{
			((Button)sender).Parent.Parent.Controls.Remove(((Button)sender).Parent);
			buttonAddRequirement.Top -= 29;
		}
		
        private void buttonUpgrades_Click(object sender, EventArgs e)
        {
            bool _overrideRegem = checkBoxOverrideRegem.Checked;
            bool _overrideReenchant = checkBoxOverrideReenchant.Checked;
            int _thoroughness = (int)Math.Ceiling((float)trackBarThoroughness.Value / 10f);
            string _calculationToOptimize = GetCalculationStringFromComboBox(comboBoxCalculationToOptimize);
            List<OptimizationRequirement> requirements = new List<OptimizationRequirement>();
            foreach (Control ctrl in groupBoxRequirements.Controls)
            {
                if (ctrl is Panel)
                {
                    OptimizationRequirement requirement = new OptimizationRequirement();
                    foreach (Control reqCtrl in ctrl.Controls)
                    {
                        switch (reqCtrl.Name)
                        {
                            case "comboBoxRequirementCalculation":
                                requirement.Calculation = GetCalculationStringFromComboBox(reqCtrl as ComboBox);
                                break;

                            case "comboBoxRequirementGreaterLessThan":
                                requirement.LessThan = (reqCtrl as ComboBox).SelectedIndex == 1;
                                break;

                            case "numericUpDownRequirementValue":
                                requirement.Value = (float)((reqCtrl as NumericUpDown).Value);
                                break;
                        }
                    }
                    requirements.Add(requirement);
                }
            }
            OptimizationRequirement[] _requirements = requirements.ToArray();

            if ((_overrideReenchant || _overrideRegem || _thoroughness > 100) && Properties.Optimizer.Default.WarningsEnabled)
            {
                if (MessageBox.Show("The upgrade evaluations perform an optimization for each relevant item. With your settings this might take a long time. Consider using lower thoroughness and no overriding of regem and reenchant options." + Environment.NewLine + Environment.NewLine + "Do you want to continue with upgrade evaluations?", "Optimizer Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
            }

            _optimizer.InitializeItemCache(_character, _character.AvailableItems, _overrideRegem, _overrideReenchant, Properties.Optimizer.Default.TemplateGemsEnabled, Calculations.Instance);
            if (Properties.Optimizer.Default.WarningsEnabled)
            {
                string prompt = _optimizer.GetWarningPromptIfNeeded();
                if (prompt != null)
                {
                    if (MessageBox.Show(prompt, "Optimizer Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
                }
            }

            buttonUpgrades.Text = "Calculating...";
            buttonOptimize.Enabled = buttonUpgrades.Enabled = checkBoxOverrideRegem.Enabled = checkBoxOverrideReenchant.Enabled =
                trackBarThoroughness.Enabled = false;
            buttonCancel.DialogResult = DialogResult.None;

            Optimizer.OptimizationMethod = Properties.Optimizer.Default.OptimizationMethod; 
            _optimizer.ComputeUpgradesAsync(_character, _calculationToOptimize, _requirements, _thoroughness, _itemToEvaluate);
        }

        private void FormOptimize_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Optimizer.Default.OverrideRegem = checkBoxOverrideRegem.Checked;
            Properties.Optimizer.Default.OverrideReenchant = checkBoxOverrideReenchant.Checked;
            Properties.Optimizer.Default.Thoroughness = trackBarThoroughness.Value;
            Properties.Optimizer.Default.CalculationToOptimize = GetCalculationStringFromComboBox(comboBoxCalculationToOptimize);
            Properties.Optimizer.Default.Save();

            _character.CalculationToOptimize = GetCalculationStringFromComboBox(comboBoxCalculationToOptimize);
            List<OptimizationRequirement> requirements = new List<OptimizationRequirement>();
            foreach (Control ctrl in groupBoxRequirements.Controls)
            {
                if (ctrl is Panel)
                {
                    OptimizationRequirement requirement = new OptimizationRequirement();
                    foreach (Control reqCtrl in ctrl.Controls)
                    {
                        switch (reqCtrl.Name)
                        {
                            case "comboBoxRequirementCalculation":
                                requirement.Calculation = GetCalculationStringFromComboBox(reqCtrl as ComboBox);
                                break;

                            case "comboBoxRequirementGreaterLessThan":
                                requirement.LessThan = (reqCtrl as ComboBox).SelectedIndex == 1;
                                break;

                            case "numericUpDownRequirementValue":
                                requirement.Value = (float)((reqCtrl as NumericUpDown).Value);
                                break;
                        }
                    }
                    requirements.Add(requirement);
                }
            }
            _character.OptimizationRequirements = requirements;
        }
	}
}
