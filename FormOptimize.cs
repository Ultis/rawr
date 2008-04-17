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
        private BackgroundWorker _worker;
        private BackgroundWorker _workerOptimize;
        private BackgroundWorker _workerUpgrades;
        private string _calculationToOptimize;
		private OptimizationRequirement[] _requirements;
		private int _thoroughness;
		private bool _allGemmings;

		public FormOptimize(Character character)
		{
			InitializeComponent();
			_character = character;
            _worker = _workerOptimize = new BackgroundWorker();
            _workerOptimize.WorkerReportsProgress = _workerOptimize.WorkerSupportsCancellation = true;
            _workerOptimize.DoWork += new DoWorkEventHandler(_worker_DoWork);
            _workerOptimize.ProgressChanged += new ProgressChangedEventHandler(_worker_ProgressChanged);
            _workerOptimize.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_worker_RunWorkerCompleted);

            _workerUpgrades = new BackgroundWorker();
            _workerUpgrades.WorkerReportsProgress = _workerUpgrades.WorkerSupportsCancellation = true;
            _workerUpgrades.DoWork += new DoWorkEventHandler(_workerUpgrades_DoWork);
            _workerUpgrades.ProgressChanged += new ProgressChangedEventHandler(_workerUpgrades_ProgressChanged);
            _workerUpgrades.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_workerUpgrades_RunWorkerCompleted);

			comboBoxCalculationToOptimize.Items.Add("Overall Rating");
			comboBoxCalculationToOptimize.Tag = Calculations.SubPointNameColors.Count;
			foreach (string subPoint in Calculations.SubPointNameColors.Keys)
				comboBoxCalculationToOptimize.Items.Add(subPoint + " Rating");
			comboBoxCalculationToOptimize.Items.AddRange(Calculations.OptimizableCalculationLabels);
			comboBoxCalculationToOptimize.SelectedIndex = 0;
		}

		private void FormOptimize_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = _worker.IsBusy;
		}

		private void buttonOptimize_Click(object sender, EventArgs e)
		{
			buttonOptimize.Text = "Optimizing...";
			buttonOptimize.Enabled = buttonUpgrades.Enabled = radioButtonAllGemmings.Enabled = radioButtonKnownGemmingsOnly.Enabled =
				trackBarThoroughness.Enabled = false;

			_allGemmings = radioButtonAllGemmings.Checked;
			_thoroughness = trackBarThoroughness.Value;
			_calculationToOptimize = GetCalculationStringFromComboBox(comboBoxCalculationToOptimize);
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
			_requirements = requirements.ToArray();

            _worker = _workerOptimize;
			_worker.RunWorkerAsync();
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
			if (_worker.IsBusy) _worker.CancelAsync();
		}

		void _worker_DoWork(object sender, DoWorkEventArgs e)
		{
            ItemCacheInstance itemCacheMain = ItemCache.Instance;
			try
			{
				ItemCacheInstance itemCacheOptimize = new ItemCacheInstance(itemCacheMain);
				ItemCache.Instance = itemCacheOptimize;

                PopulateAvailableIds(itemCacheMain);
			    e.Result = Optimize();
            }
            catch (Exception ex)
            {
                ex.ToString();
                e.Result = null;
            }
            finally
            {
                ItemCache.Instance = itemCacheMain;
            }

			if (!_worker.CancellationPending)
			{
				_worker.ReportProgress(100);
				System.Threading.Thread.Sleep(1000);
			}
			else
				e.Cancel = true;
		}

		void _worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.UserState != null)
				labelMax.Text = e.UserState.ToString();
			progressBarAlt.Value = e.ProgressPercentage;
			progressBarMain.Value = Math.Max(e.ProgressPercentage, progressBarMain.Value);


            Text = string.Format("{0}% Complete - Rawr Optimizer", progressBarMain.Value);
		}

		void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				labelMax.Text = string.Empty;
				buttonOptimize.Text = "Optimize";
				buttonOptimize.Enabled = buttonUpgrades.Enabled = radioButtonAllGemmings.Enabled = radioButtonKnownGemmingsOnly.Enabled =
				 trackBarThoroughness.Enabled = true;
				progressBarAlt.Value = progressBarMain.Value = 0;
			}
			else
			{
				progressBarAlt.Value = progressBarMain.Value = 100;
				Character bestCharacter = e.Result as Character;
				if (bestCharacter == null)
				{
					labelMax.Text = string.Empty;
					buttonOptimize.Text = "Optimize";
					buttonOptimize.Enabled = buttonUpgrades.Enabled = radioButtonAllGemmings.Enabled = radioButtonKnownGemmingsOnly.Enabled =
					 trackBarThoroughness.Enabled = true;
					progressBarAlt.Value = progressBarMain.Value = 0;
					MessageBox.Show("Sorry, Rawr was unable to find a gearset to meet your requirements.", "Rawr Optimizer Results");
				}

				if (_character == null || MessageBox.Show(string.Format("The Optimizer found a gearset with a score of {0}. " +
					"(Your currently equipped gear has a score of {1}) Would you like to equip the optimized gear?",
					GetCalculationsValue(Calculations.GetCharacterCalculations(bestCharacter)),
					GetCalculationsValue(Calculations.GetCharacterCalculations(_character))),
					"Rawr Optimizer Results", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
				_character.Back = bestCharacter.Back;
				_character.BackEnchant = bestCharacter.BackEnchant;
				_character.Chest = bestCharacter.Chest;
				_character.ChestEnchant = bestCharacter.ChestEnchant;
				_character.Feet = bestCharacter.Feet;
				_character.FeetEnchant = bestCharacter.FeetEnchant;
				_character.Finger1 = bestCharacter.Finger1;
				_character.Finger1Enchant = bestCharacter.Finger1Enchant;
				_character.Finger2 = bestCharacter.Finger2;
				_character.Finger2Enchant = bestCharacter.Finger2Enchant;
				_character.Hands = bestCharacter.Hands;
				_character.HandsEnchant = bestCharacter.HandsEnchant;
				_character.Head = bestCharacter.Head;
				_character.HeadEnchant = bestCharacter.HeadEnchant;
				_character.Legs = bestCharacter.Legs;
				_character.LegsEnchant = bestCharacter.LegsEnchant;
				_character.MainHand = bestCharacter.MainHand;
				_character.MainHandEnchant = bestCharacter.MainHandEnchant;
				_character.Neck = bestCharacter.Neck;
				_character.OffHand = bestCharacter.OffHand;
				_character.OffHandEnchant = bestCharacter.OffHandEnchant;
				_character.Projectile = bestCharacter.Projectile;
				_character.ProjectileBag = bestCharacter.ProjectileBag;
				_character.Ranged = bestCharacter.Ranged;
				_character.RangedEnchant = bestCharacter.RangedEnchant;
				_character.Shoulders = bestCharacter.Shoulders;
				_character.ShouldersEnchant = bestCharacter.ShouldersEnchant;
				_character.Trinket1 = bestCharacter.Trinket1;
				_character.Trinket2 = bestCharacter.Trinket2;
				_character.Waist = bestCharacter.Waist;
				_character.Wrist = bestCharacter.Wrist;
				_character.WristEnchant = bestCharacter.WristEnchant;
				_character.OnItemsChanged();
				Close();
			}
				else
				{
					labelMax.Text = string.Empty;
					buttonOptimize.Text = "Optimize";
                    buttonOptimize.Enabled = buttonUpgrades.Enabled = radioButtonAllGemmings.Enabled = radioButtonKnownGemmingsOnly.Enabled =
					 trackBarThoroughness.Enabled = true;
					progressBarAlt.Value = progressBarMain.Value = 0;
				}
			}
		}

        int itemProgress = 0;
        string currentItem = "";

        void _workerUpgrades_DoWork(object sender, DoWorkEventArgs e)
        {
            ItemCacheInstance itemCacheMain = ItemCache.Instance;
            Character saveCharacter = _character;
            try
            {
                ItemCacheInstance itemCacheOptimize = new ItemCacheInstance(itemCacheMain);
                ItemCache.Instance = itemCacheOptimize;

                PopulateAvailableIds(itemCacheMain);
                Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> result = new Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>>();

                Item[] items = itemCacheMain.RelevantItems;
                Character.CharacterSlot[] slots = new Character.CharacterSlot[] { Character.CharacterSlot.Back, Character.CharacterSlot.Chest, Character.CharacterSlot.Feet, Character.CharacterSlot.Finger1, Character.CharacterSlot.Hands, Character.CharacterSlot.Head, Character.CharacterSlot.Legs, Character.CharacterSlot.MainHand, Character.CharacterSlot.Neck, Character.CharacterSlot.OffHand, Character.CharacterSlot.Projectile, Character.CharacterSlot.ProjectileBag, Character.CharacterSlot.Ranged, Character.CharacterSlot.Shoulders, Character.CharacterSlot.Trinket1, Character.CharacterSlot.Waist, Character.CharacterSlot.Wrist };
                foreach (Character.CharacterSlot slot in slots)
                    result[slot] = new List<ComparisonCalculationBase>();

                CharacterCalculationsBase baseCalculations = Calculations.GetCharacterCalculations(_character);
                float baseValue = GetCalculationsValue(baseCalculations);
                Dictionary<int, Item> itemById = new Dictionary<int,Item>();
                foreach (Item item in items)
                {
                    itemById[item.Id] = item;
                }

                items = new List<Item>(itemById.Values).ToArray();
                _allGemmings = true; // for the new added items check all gemmings

                for (int i = 0; i < items.Length; i++)
                {
                    Item item = items[i];
                    currentItem = item.Name;
                    itemProgress = (int)Math.Round((float)i / ((float)items.Length / 100f));
                    if (_worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    _worker.ReportProgress(0);
                    foreach (Character.CharacterSlot slot in slots)
                    {
                        if (item.FitsInSlot(slot))
                        {
                            List<ComparisonCalculationBase> comparisons = result[slot];
                            PopulateLockedIds(item);
                            lockedSlot = slot;
                            _character = BuildSingleItemSwapCharacter(_character, slot, lockedIds[0]);
                            float best;
                            CharacterCalculationsBase bestCalculations;
                            Character bestCharacter;
                            if (_thoroughness > 1)
                            {
                                int saveThoroughness = _thoroughness;
                                _thoroughness = 1;
                                float injectValue;
                                Character inject = Optimize(null, 0, out injectValue, out bestCalculations);
                                _thoroughness = saveThoroughness;
                                bestCharacter = Optimize(inject, injectValue, out best, out bestCalculations);
                            }
                            else
                            {
                                bestCharacter = Optimize(null, 0, out best, out bestCalculations);
                            }
                            if (best > baseValue)
                            {
                                switch (slot)
                                {
                                    case Character.CharacterSlot.Back:
                                        item = bestCharacter.Back;
                                        break;
                                    case Character.CharacterSlot.Chest:
                                        item = bestCharacter.Chest;
                                        break;
                                    case Character.CharacterSlot.Feet:
                                        item = bestCharacter.Feet;
                                        break;
                                    case Character.CharacterSlot.Finger1:
                                        item = bestCharacter.Finger1;
                                        break;
                                    case Character.CharacterSlot.Hands:
                                        item = bestCharacter.Hands;
                                        break;
                                    case Character.CharacterSlot.Head:
                                        item = bestCharacter.Head;
                                        break;
                                    case Character.CharacterSlot.Legs:
                                        item = bestCharacter.Legs;
                                        break;
                                    case Character.CharacterSlot.MainHand:
                                        item = bestCharacter.MainHand;
                                        break;
                                    case Character.CharacterSlot.Neck:
                                        item = bestCharacter.Neck;
                                        break;
                                    case Character.CharacterSlot.OffHand:
                                        item = bestCharacter.OffHand;
                                        break;
                                    case Character.CharacterSlot.Projectile:
                                        item = bestCharacter.Projectile;
                                        break;
                                    case Character.CharacterSlot.ProjectileBag:
                                        item = bestCharacter.ProjectileBag;
                                        break;
                                    case Character.CharacterSlot.Ranged:
                                        item = bestCharacter.Ranged;
                                        break;
                                    case Character.CharacterSlot.Shoulders:
                                        item = bestCharacter.Shoulders;
                                        break;
                                    case Character.CharacterSlot.Trinket1:
                                        item = bestCharacter.Trinket1;
                                        break;
                                    case Character.CharacterSlot.Waist:
                                        item = bestCharacter.Waist;
                                        break;
                                    case Character.CharacterSlot.Wrist:
                                        item = bestCharacter.Wrist;
                                        break;
                                }
                                ComparisonCalculationBase itemCalc = Calculations.CreateNewComparisonCalculation();
                                itemCalc.Item = item;
                                itemCalc.Name = item.Name;
                                itemCalc.Equipped = false;
                                itemCalc.OverallPoints = bestCalculations.OverallPoints - baseCalculations.OverallPoints;
                                float[] subPoints = new float[bestCalculations.SubPoints.Length];
                                for (int j = 0; j < bestCalculations.SubPoints.Length; j++)
                                {
                                    subPoints[j] = bestCalculations.SubPoints[j] - baseCalculations.SubPoints[j];
                                }
                                itemCalc.SubPoints = subPoints;

                                comparisons.Add(itemCalc);
                            }
                            _character = saveCharacter;
                        }
                    }
                }

                e.Result = result;
            }
            catch (Exception ex)
            {
                ex.ToString();
                e.Result = null;
            }
            finally
            {
                _character = saveCharacter;
                ItemCache.Instance = itemCacheMain;
            }

            if (!_worker.CancellationPending)
            {
                _worker.ReportProgress(100);
                System.Threading.Thread.Sleep(1000);
            }
            else
                e.Cancel = true;
        }

        void _workerUpgrades_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelMax.Text = currentItem;
            progressBarAlt.Value = e.ProgressPercentage;
            progressBarMain.Value = itemProgress;

            Text = string.Format("{0}% Complete - Rawr Optimizer", progressBarMain.Value);
        }

        void _workerUpgrades_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled || e.Result == null)
            {
                labelMax.Text = string.Empty;
                buttonUpgrades.Text = "Upgrades";
                buttonOptimize.Enabled = buttonUpgrades.Enabled = radioButtonAllGemmings.Enabled = radioButtonKnownGemmingsOnly.Enabled =
                 trackBarThoroughness.Enabled = true;
                progressBarAlt.Value = progressBarMain.Value = 0;
            }
            else
            {
                progressBarAlt.Value = progressBarMain.Value = 100;
                Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>> result = e.Result as Dictionary<Character.CharacterSlot, List<ComparisonCalculationBase>>;
                FormUpgradeComparison.Instance.LoadData(_character, result);
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
			panelRequirement.BringToFront();
		}

		void buttonRemoveRequirement_Click(object sender, EventArgs e)
		{
			((Button)sender).Parent.Parent.Controls.Remove(((Button)sender).Parent);
			buttonAddRequirement.Top -= 29;
		}

        List<int> metaGemIds;
        List<int> gemIds;
        SortedList<string, bool> uniqueIds;
		string[] headIds, neckIds, shouldersIds, backIds, chestIds, wristIds, handsIds, waistIds,
					legsIds, feetIds, fingerIds, trinketIds, mainHandIds, offHandIds, rangedIds, 
					projectileIds, projectileBagIds;
		int[] backEnchantIds, chestEnchantIds, feetEnchantIds, fingerEnchantIds, handsEnchantIds, headEnchantIds,
			legsEnchantIds, shouldersEnchantIds, mainHandEnchantIds, offHandEnchantIds, rangedEnchantIds, wristEnchantIds;
        string[] lockedIds;
        Character.CharacterSlot lockedSlot = Character.CharacterSlot.None;
		Random rand;

        private void PopulateLockedIds(Item item)
        {
            lockedIds = GetPossibleGemmedIdsForItem(item, gemIds, metaGemIds);
            foreach (string possibleGemmedId in lockedIds)
                uniqueIds[possibleGemmedId] = item.Unique;
        }

        private void PopulateAvailableIds(ItemCacheInstance itemCacheMain)
        {
            metaGemIds = new List<int>();
            gemIds = new List<int>();
            List<int> itemIds = new List<int>(_character.AvailableItems.ToArray());
            foreach (int id in _character.AvailableItems)
            {
                if (id > 0)
                {
                    Item availableItem = itemCacheMain.FindItemById(id, false);
                    if (availableItem != null)
                    {
                        switch (availableItem.Slot)
                        {
                            case Item.ItemSlot.Meta:
                                metaGemIds.Add(id);
                                break;
                            case Item.ItemSlot.Red:
                            case Item.ItemSlot.Orange:
                            case Item.ItemSlot.Yellow:
                            case Item.ItemSlot.Green:
                            case Item.ItemSlot.Blue:
                            case Item.ItemSlot.Purple:
                            case Item.ItemSlot.Prismatic:
                                gemIds.Add(id);
                                break;
                        }
                    }
                }
            }
            if (gemIds.Count == 0) gemIds.Add(0);
            if (metaGemIds.Count == 0) metaGemIds.Add(0);
            itemIds.RemoveAll(new Predicate<int>(delegate(int id) { return id < 0 || gemIds.Contains(id) || metaGemIds.Contains(id); }));

            List<string> headIdList = new List<string>();
            List<string> neckIdList = new List<string>();
            List<string> shouldersIdList = new List<string>();
            List<string> backIdList = new List<string>();
            List<string> chestIdList = new List<string>();
            List<string> wristIdList = new List<string>();
            List<string> handsIdList = new List<string>();
            List<string> waistIdList = new List<string>();
            List<string> legsIdList = new List<string>();
            List<string> feetIdList = new List<string>();
            List<string> fingerIdList = new List<string>();
            List<string> trinketIdList = new List<string>();
            List<string> mainHandIdList = new List<string>();
            List<string> offHandIdList = new List<string>();
            List<string> rangedIdList = new List<string>();
            List<string> projectileIdList = new List<string>();
            List<string> projectileBagIdList = new List<string>();

            List<int> backEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Back, _character.AvailableItems))
                backEnchantIdList.Add(enchant.Id);
            backEnchantIds = backEnchantIdList.ToArray();
            List<int> chestEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Chest, _character.AvailableItems))
                chestEnchantIdList.Add(enchant.Id);
            chestEnchantIds = chestEnchantIdList.ToArray();
            List<int> feetEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Feet, _character.AvailableItems))
                feetEnchantIdList.Add(enchant.Id);
            feetEnchantIds = feetEnchantIdList.ToArray();
            List<int> fingerEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Finger, _character.AvailableItems))
                fingerEnchantIdList.Add(enchant.Id);
            fingerEnchantIds = fingerEnchantIdList.ToArray();
            List<int> handsEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Hands, _character.AvailableItems))
                handsEnchantIdList.Add(enchant.Id);
            handsEnchantIds = handsEnchantIdList.ToArray();
            List<int> headEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Head, _character.AvailableItems))
                headEnchantIdList.Add(enchant.Id);
            headEnchantIds = headEnchantIdList.ToArray();
            List<int> legsEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Legs, _character.AvailableItems))
                legsEnchantIdList.Add(enchant.Id);
            legsEnchantIds = legsEnchantIdList.ToArray();
            List<int> shouldersEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Shoulders, _character.AvailableItems))
                shouldersEnchantIdList.Add(enchant.Id);
            shouldersEnchantIds = shouldersEnchantIdList.ToArray();
            List<int> mainHandEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.MainHand, _character.AvailableItems))
                mainHandEnchantIdList.Add(enchant.Id);
            mainHandEnchantIds = mainHandEnchantIdList.ToArray();
            List<int> offHandEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.OffHand, _character.AvailableItems))
                offHandEnchantIdList.Add(enchant.Id);
            offHandEnchantIds = offHandEnchantIdList.ToArray();
            List<int> rangedEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Ranged, _character.AvailableItems))
                rangedEnchantIdList.Add(enchant.Id);
            rangedEnchantIds = rangedEnchantIdList.ToArray();
            List<int> wristEnchantIdList = new List<int>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Wrist, _character.AvailableItems))
                wristEnchantIdList.Add(enchant.Id);
            wristEnchantIds = wristEnchantIdList.ToArray();

            Dictionary<string, bool> uniqueDict = new Dictionary<string, bool>();
            Item item = null;
            string[] possibleGemmedIds = null;
            foreach (int itemId in itemIds)
            {
                item = ItemCache.FindItemById(itemId);

                if (item != null && Calculations.IsItemRelevant(item))
                {
                    possibleGemmedIds = GetPossibleGemmedIdsForItem(item, gemIds, metaGemIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Head)) headIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Neck)) neckIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Shoulders)) shouldersIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Back)) backIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Chest)) chestIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Wrist)) wristIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Hands)) handsIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Waist)) waistIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Legs)) legsIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Feet)) feetIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Finger1)) fingerIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Trinket1)) trinketIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.MainHand)) mainHandIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.OffHand)) offHandIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Ranged)) rangedIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.Projectile)) projectileIdList.AddRange(possibleGemmedIds);
                    if (item.FitsInSlot(Character.CharacterSlot.ProjectileBag)) projectileBagIdList.AddRange(possibleGemmedIds);

                    foreach (string possibleGemmedId in possibleGemmedIds)
                        uniqueDict.Add(possibleGemmedId, item.Unique);
                }
            }
            uniqueIds = new SortedList<string, bool>(uniqueDict);

            if (headIdList.Count == 0) headIdList.Add(null);
            if (neckIdList.Count == 0) neckIdList.Add(null);
            if (shouldersIdList.Count == 0) shouldersIdList.Add(null);
            if (backIdList.Count == 0) backIdList.Add(null);
            if (chestIdList.Count == 0) chestIdList.Add(null);
            if (wristIdList.Count == 0) wristIdList.Add(null);
            if (handsIdList.Count == 0) handsIdList.Add(null);
            if (waistIdList.Count == 0) waistIdList.Add(null);
            if (legsIdList.Count == 0) legsIdList.Add(null);
            if (feetIdList.Count == 0) feetIdList.Add(null);
            if (rangedIdList.Count == 0) rangedIdList.Add(null);
            if (projectileIdList.Count == 0) projectileIdList.Add(null);
            if (projectileBagIdList.Count == 0) projectileBagIdList.Add(null);
            fingerIdList.Add(null);
            trinketIdList.Add(null);
            mainHandIdList.Add(null);
            offHandIdList.Add(null);

            headIds = FilterList(headIdList);
            neckIds = FilterList(neckIdList);
            shouldersIds = FilterList(shouldersIdList);
            backIds = FilterList(backIdList);
            chestIds = FilterList(chestIdList);
            wristIds = FilterList(wristIdList);
            handsIds = FilterList(handsIdList);
            waistIds = FilterList(waistIdList);
            legsIds = FilterList(legsIdList);
            feetIds = FilterList(feetIdList);
            fingerIds = fingerIdList.ToArray(); //When one ring/trinket is completely better than another
            trinketIds = trinketIdList.ToArray(); //you may still want to use both, so don't filter
            mainHandIds = FilterList(mainHandIdList);
            offHandIds = FilterList(offHandIdList);
            rangedIds = FilterList(rangedIdList);
            projectileIds = FilterList(projectileIdList);
            projectileBagIds = FilterList(projectileBagIdList);

            ulong totalCombinations = (ulong)headIds.Length * (ulong)neckIds.Length * (ulong)shouldersIds.Length * (ulong)backIds.Length
                 * (ulong)chestIds.Length * (ulong)wristIds.Length * (ulong)handsIds.Length * (ulong)waistIds.Length * (ulong)legsIds.Length *
                 (ulong)feetIds.Length * (ulong)fingerIds.Length * (ulong)fingerIds.Length * (ulong)trinketIds.Length * (ulong)trinketIds.Length
                 * (ulong)mainHandIds.Length * (ulong)offHandIds.Length * (ulong)rangedIds.Length * (ulong)projectileIds.Length *
                 (ulong)projectileBagIds.Length * (ulong)headEnchantIds.Length * (ulong)shouldersEnchantIds.Length * (ulong)backEnchantIds.Length
                 * (ulong)chestEnchantIds.Length * (ulong)wristEnchantIds.Length * (ulong)handsEnchantIds.Length * (ulong)legsEnchantIds.Length *
                 (ulong)feetEnchantIds.Length * (ulong)fingerEnchantIds.Length * (ulong)fingerEnchantIds.Length * (ulong)mainHandIds.Length
                 * (ulong)offHandEnchantIds.Length * (ulong)rangedEnchantIds.Length;

            totalCombinations.ToString("f0");
        }

        private Character Optimize()
        {
            float best;
            CharacterCalculationsBase bestCalc;
            return Optimize(null, 0, out best, out bestCalc);
        }

		private Character Optimize(Character injectCharacter, float injectValue, out float best, out CharacterCalculationsBase bestCalculations)
		{
			//Begin Genetic
			int noImprove, i1, i2;
			best = -10000000;
            bestCalculations = null;
            bool injected = false;

			int popSize = _thoroughness;
			int cycleLimit = _thoroughness;
			Character[] population = new Character[popSize];
			Character[] popCopy = new Character[popSize];
			float[] values = new float[popSize];
			float[] share = new float[popSize];
			float s, sum, minv, maxv;
			Character bestCharacter = null;
			rand = new Random();

			if (_thoroughness > 1)
			{
			for (int i = 0; i < popSize; i++)
			{
				population[i] = BuildRandomCharacter();
			}
			}
			else
			{
				bestCharacter = _character;
			}

			noImprove = 0;
			while (noImprove < cycleLimit)
			{
				if (_thoroughness > 1)
				{
				    if (_worker.CancellationPending) return null;
					_worker.ReportProgress((int)Math.Round((float)noImprove / ((float)cycleLimit / 100f)), best);
    					
				    minv = 10000000;
				    maxv = -10000000;
				    for (int i = 0; i < popSize; i++)
				    {
                        CharacterCalculationsBase calculations;
                        values[i] = GetCalculationsValue(calculations = Calculations.GetCharacterCalculations(population[i]));
					    if (values[i] < minv) minv = values[i];
					    if (values[i] > maxv) maxv = values[i];
					    if (values[i] > best)
					    {
						    best = values[i];
                            bestCalculations = calculations;
						    bestCharacter = population[i];
						    noImprove = -1;
					    }
				    }
				    sum = 0;
				    for (int i = 0; i < popSize; i++)
					    sum += values[i] - minv + (maxv - minv) / 2;
				    for (int i = 0; i < popSize; i++)
					    share[i] = sum == 0 ? 1f / popSize : (values[i] - minv + (maxv - minv) / 2) / sum;
				}

				noImprove++;

				if (_thoroughness > 1 && noImprove == 0 || noImprove % Math.Max(1, cycleLimit / 2) != 0)
				{
					population.CopyTo(popCopy, 0);
					if (bestCharacter == null)
						population[0] = BuildRandomCharacter();
					else
						population[0] = bestCharacter;
					for (int i = 1; i < popSize; i++)
					{
						if (best == 0 || rand.NextDouble() < 0.1d)
						{
							//completely random
							population[i] = BuildRandomCharacter();
						}
						else if (rand.NextDouble() < 0.4d)
						{
							//crossover
							s = (float)rand.NextDouble();
							sum = 0;
							for (i1 = 0; i1 < popSize - 1; i1++)
							{
								sum += share[i1];
								if (sum >= s) break;
							}
							s = (float)rand.NextDouble();
							sum = 0;
							for (i2 = 0; i2 < popSize - 1; i2++)
							{
								sum += share[i2];
								if (sum >= s) break;
							}
							population[i] = BuildChildCharacter(popCopy[i1], popCopy[i2]);
						}
						else
						{
							//mutate
							s = (float)rand.NextDouble();
							sum = 0;
							for (i1 = 0; i1 < popSize - 1; i1++)
							{
								sum += share[i1];
								if (sum >= s) break;
							}
							population[i] = BuildMutantCharacter(popCopy[i1]);
						}
					}
				}
                else if (_thoroughness > 1 && injectCharacter != null && !injected && injectValue > best)
                {
                    population[popSize - 1] = injectCharacter;
                    noImprove = 0;
                    injected = true;
                }
                else
                {
                    //last try, look for single direct upgrades
                    KeyValuePair<float, Character> results;
                    CharacterCalculationsBase calculations;
                    results = LookForDirectItemUpgrades(headIds, Character.CharacterSlot.Head, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(neckIds, Character.CharacterSlot.Neck, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(backIds, Character.CharacterSlot.Back, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(shouldersIds, Character.CharacterSlot.Shoulders, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(chestIds, Character.CharacterSlot.Chest, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(wristIds, Character.CharacterSlot.Wrist, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(handsIds, Character.CharacterSlot.Hands, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(waistIds, Character.CharacterSlot.Waist, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(legsIds, Character.CharacterSlot.Legs, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(feetIds, Character.CharacterSlot.Feet, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(fingerIds, Character.CharacterSlot.Finger1, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(fingerIds, Character.CharacterSlot.Finger2, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(trinketIds, Character.CharacterSlot.Trinket1, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(trinketIds, Character.CharacterSlot.Trinket2, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(mainHandIds, Character.CharacterSlot.MainHand, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(offHandIds, Character.CharacterSlot.OffHand, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(rangedIds, Character.CharacterSlot.Ranged, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(projectileIds, Character.CharacterSlot.Projectile, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(projectileBagIds, Character.CharacterSlot.ProjectileBag, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }

                    results = LookForDirectEnchantUpgrades(headEnchantIds, Character.CharacterSlot.Head, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(backEnchantIds, Character.CharacterSlot.Back, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(shouldersEnchantIds, Character.CharacterSlot.Shoulders, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(chestEnchantIds, Character.CharacterSlot.Chest, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(wristEnchantIds, Character.CharacterSlot.Wrist, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(handsEnchantIds, Character.CharacterSlot.Hands, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(legsEnchantIds, Character.CharacterSlot.Legs, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(feetEnchantIds, Character.CharacterSlot.Feet, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(fingerEnchantIds, Character.CharacterSlot.Finger1, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(fingerEnchantIds, Character.CharacterSlot.Finger2, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(mainHandEnchantIds, Character.CharacterSlot.MainHand, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(offHandEnchantIds, Character.CharacterSlot.OffHand, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(rangedEnchantIds, Character.CharacterSlot.Ranged, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                }
			}

            if (best == 0)
            {
                bestCharacter = null;
                bestCalculations = null;
            }
            else
                ToString();
			return bestCharacter;

			//ulong msGenetic = (ulong)DateTime.Now.Subtract(startTime).TotalMilliseconds;
			//w.ToString();

			//startTime = DateTime.Now;

			//for (int i = 0; i < 10000; i++)
			//    Calculations.GetCharacterCalculations(_character);


			//ulong msManual10000 = (ulong)(DateTime.Now.Subtract(startTime).TotalMilliseconds);
			//ulong msManual = msManual10000 * (totalCombinations / (ulong)10000);

			//MessageBox.Show(string.Format("Genetic: {0} sec\r\nManual: {1} sec\r\nRelative Speed: {2}",
			//    msGenetic/1000, msManual/1000, msManual / msGenetic));
		}

		private KeyValuePair<float, Character> LookForDirectItemUpgrades(string[] ids, Character.CharacterSlot slot, float best, Character bestCharacter, out CharacterCalculationsBase bestCalculations)
		{
			Character charSwap;
            bestCalculations = null;
			float value;
			bool foundUpgrade = false;
            if (slot == lockedSlot) ids = lockedIds;
			foreach (string id in ids)
			{
				if (id != null &&
					!(slot == Character.CharacterSlot.Finger1 && bestCharacter._finger2 == id && uniqueIds[id]) &&
					!(slot == Character.CharacterSlot.Finger2 && bestCharacter._finger1 == id && uniqueIds[id]) &&
					!(slot == Character.CharacterSlot.Trinket1 && bestCharacter._trinket2 == id && uniqueIds[id]) &&
					!(slot == Character.CharacterSlot.Trinket2 && bestCharacter._trinket1 == id && uniqueIds[id]) &&
					!(slot == Character.CharacterSlot.MainHand && bestCharacter._mainHand == id && uniqueIds[id]) &&
					!(slot == Character.CharacterSlot.OffHand && bestCharacter._offHand == id && uniqueIds[id]))
				{
					charSwap = BuildSingleItemSwapCharacter(bestCharacter, slot, id);
                    CharacterCalculationsBase calculations;
					value = GetCalculationsValue(calculations = Calculations.GetCharacterCalculations(charSwap));
					if (value > best)
					{
						best = value;
                        bestCalculations = calculations;
						bestCharacter = charSwap;
						foundUpgrade = true;
					}
				}
			}
			if (foundUpgrade)
				return new KeyValuePair<float, Character>(best, bestCharacter);
			return new KeyValuePair<float, Character>(0, null);
		}

		private KeyValuePair<float, Character> LookForDirectEnchantUpgrades(int[] ids, Character.CharacterSlot slot, float best, Character bestCharacter, out CharacterCalculationsBase bestCalculations)
		{
			Character charSwap;
            bestCalculations = null;
			float value;
			float newBest = best;
			Character newBestCharacter = null;
			foreach (int id in ids)
			{
				charSwap = BuildSingleEnchantSwapCharacter(bestCharacter, slot, id);
                CharacterCalculationsBase calculations;
				value = GetCalculationsValue(calculations = Calculations.GetCharacterCalculations(charSwap));
				if (value > newBest)
				{
					newBest = value;
                    bestCalculations = calculations;
					newBestCharacter = charSwap;
				}
			}
			if (newBest > best)
				return new KeyValuePair<float, Character>(newBest, newBestCharacter);
			return new KeyValuePair<float,Character>(0, null);
		}
		

		private float GetCalculationsValue(CharacterCalculationsBase calcs)
		{
			float ret = 0;
			foreach (OptimizationRequirement requirement in _requirements)
			{
				float calcValue = GetCalculationValue(calcs, requirement.Calculation);
				if (requirement.LessThan)
				{
					if (!(calcValue <= requirement.Value))
						ret += requirement.Value - calcValue;
				}
				else
				{
					if (!(calcValue >= requirement.Value))
						ret += calcValue - requirement.Value;
				}
			}

			if (ret < 0) return ret;
			else return GetCalculationValue(calcs, _calculationToOptimize);
		}

		private float GetCalculationValue(CharacterCalculationsBase calcs, string calculation)
		{
			if (calculation == "[Overall]")
				return calcs.OverallPoints;
			else if (calculation.StartsWith("[SubPoint "))
				return calcs.SubPoints[int.Parse(calculation.Substring(10).TrimEnd(']'))];
			else
				return calcs.GetOptimizableCalculationValue(calculation);
		}

		private Character BuildRandomCharacter()
		{
			string finger1Id = (lockedSlot == Character.CharacterSlot.Finger1) ? lockedIds[rand.Next(lockedIds.Length)] : fingerIds[rand.Next(fingerIds.Length)];
			string finger2Id = fingerIds[rand.Next(fingerIds.Length)];
			while (finger1Id == finger2Id && finger1Id != null && uniqueIds[finger1Id])
				finger2Id = fingerIds[rand.Next(fingerIds.Length)];

            string trinket1Id = (lockedSlot == Character.CharacterSlot.Trinket1) ? lockedIds[rand.Next(lockedIds.Length)] : trinketIds[rand.Next(trinketIds.Length)];
			string trinket2Id = trinketIds[rand.Next(trinketIds.Length)];
			while (trinket1Id == trinket2Id && trinket1Id != null && uniqueIds[trinket1Id])
				trinket2Id = trinketIds[rand.Next(trinketIds.Length)];

            string mainHandId = (lockedSlot == Character.CharacterSlot.MainHand) ? lockedIds[rand.Next(lockedIds.Length)] : mainHandIds[rand.Next(mainHandIds.Length)];
            string offHandId = (lockedSlot == Character.CharacterSlot.OffHand) ? lockedIds[rand.Next(lockedIds.Length)] : offHandIds[rand.Next(offHandIds.Length)];
			while (mainHandId == offHandId && mainHandId != null && uniqueIds[mainHandId])
			{
                mainHandId = (lockedSlot == Character.CharacterSlot.MainHand) ? lockedIds[rand.Next(lockedIds.Length)] : mainHandIds[rand.Next(mainHandIds.Length)];
                offHandId = (lockedSlot == Character.CharacterSlot.OffHand) ? lockedIds[rand.Next(lockedIds.Length)] : offHandIds[rand.Next(offHandIds.Length)];
			}

			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
                        (lockedSlot == Character.CharacterSlot.Head) ? lockedIds[rand.Next(lockedIds.Length)] : headIds[rand.Next(headIds.Length)],
                        (lockedSlot == Character.CharacterSlot.Neck) ? lockedIds[rand.Next(lockedIds.Length)] : neckIds[rand.Next(neckIds.Length)],
                        (lockedSlot == Character.CharacterSlot.Shoulders) ? lockedIds[rand.Next(lockedIds.Length)] : shouldersIds[rand.Next(shouldersIds.Length)],
                        (lockedSlot == Character.CharacterSlot.Back) ? lockedIds[rand.Next(lockedIds.Length)] : backIds[rand.Next(backIds.Length)],
                        (lockedSlot == Character.CharacterSlot.Chest) ? lockedIds[rand.Next(lockedIds.Length)] : chestIds[rand.Next(chestIds.Length)],
                        null, null,
                        (lockedSlot == Character.CharacterSlot.Wrist) ? lockedIds[rand.Next(lockedIds.Length)] : wristIds[rand.Next(wristIds.Length)],
                        (lockedSlot == Character.CharacterSlot.Hands) ? lockedIds[rand.Next(lockedIds.Length)] : handsIds[rand.Next(handsIds.Length)],
                        (lockedSlot == Character.CharacterSlot.Waist) ? lockedIds[rand.Next(lockedIds.Length)] : waistIds[rand.Next(waistIds.Length)],
                        (lockedSlot == Character.CharacterSlot.Legs) ? lockedIds[rand.Next(lockedIds.Length)] : legsIds[rand.Next(legsIds.Length)],
                        (lockedSlot == Character.CharacterSlot.Feet) ? lockedIds[rand.Next(lockedIds.Length)] : feetIds[rand.Next(feetIds.Length)],
                        finger1Id, finger2Id, trinket1Id, trinket2Id, mainHandId, offHandId,
                        (lockedSlot == Character.CharacterSlot.Ranged) ? lockedIds[rand.Next(lockedIds.Length)] : rangedIds[rand.Next(rangedIds.Length)],
                        (lockedSlot == Character.CharacterSlot.Projectile) ? lockedIds[rand.Next(lockedIds.Length)] : projectileIds[rand.Next(projectileIds.Length)],
                        (lockedSlot == Character.CharacterSlot.ProjectileBag) ? lockedIds[rand.Next(lockedIds.Length)] : projectileBagIds[rand.Next(projectileBagIds.Length)],
						headEnchantIds[rand.Next(headEnchantIds.Length)], shouldersEnchantIds[rand.Next(shouldersEnchantIds.Length)],
						backEnchantIds[rand.Next(backEnchantIds.Length)], chestEnchantIds[rand.Next(chestEnchantIds.Length)],
						wristEnchantIds[rand.Next(wristEnchantIds.Length)], handsEnchantIds[rand.Next(handsEnchantIds.Length)],
						legsEnchantIds[rand.Next(legsEnchantIds.Length)], feetEnchantIds[rand.Next(feetEnchantIds.Length)],
						fingerEnchantIds[rand.Next(fingerEnchantIds.Length)], fingerEnchantIds[rand.Next(fingerEnchantIds.Length)],
						mainHandEnchantIds[rand.Next(mainHandEnchantIds.Length)], offHandEnchantIds[rand.Next(offHandEnchantIds.Length)],
						rangedEnchantIds[rand.Next(rangedEnchantIds.Length)]);
			character.ActiveBuffs.AddRange(_character.ActiveBuffs);
			foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
				character.CalculationOptions.Add(kvp.Key, kvp.Value);
			character.Class = _character.Class;
			character.Talents = _character.Talents;
			character.RecalculateSetBonuses();
			return character;
		}

		private Character BuildChildCharacter(Character father, Character mother)
		{
			string finger1Id = rand.NextDouble() < 0.5d ? father._finger1 : mother._finger1;
			string finger2Id = rand.NextDouble() < 0.5d ? father._finger2 : mother._finger2;
			while (finger1Id == finger2Id && finger1Id != null && uniqueIds[finger1Id])
			{
				finger1Id = rand.NextDouble() < 0.5d ? father._finger1 : mother._finger1;
				finger2Id = rand.NextDouble() < 0.5d ? father._finger2 : mother._finger2;
			}

			string trinket1Id = rand.NextDouble() < 0.5d ? father._trinket1 : mother._trinket1;
			string trinket2Id = rand.NextDouble() < 0.5d ? father._trinket2 : mother._trinket2;
			while (trinket1Id == trinket2Id && trinket1Id != null && uniqueIds[trinket1Id])
			{
				trinket1Id = rand.NextDouble() < 0.5d ? father._trinket1 : mother._trinket1;
				trinket2Id = rand.NextDouble() < 0.5d ? father._trinket2 : mother._trinket2;
			}

			string mainHandId = rand.NextDouble() < 0.5d ? father._mainHand : mother._mainHand;
			string offHandId = rand.NextDouble() < 0.5d ? father._offHand : mother._offHand;
			while (mainHandId == offHandId && mainHandId != null && uniqueIds[mainHandId])
			{
				mainHandId = rand.NextDouble() < 0.5d ? father._mainHand : mother._mainHand;
				offHandId = rand.NextDouble() < 0.5d ? father._offHand : mother._offHand;
			}

			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
				rand.NextDouble() < 0.5d ? father._head : mother._head,
				rand.NextDouble() < 0.5d ? father._neck : mother._neck,
				rand.NextDouble() < 0.5d ? father._shoulders : mother._shoulders,
				rand.NextDouble() < 0.5d ? father._back : mother._back,
				rand.NextDouble() < 0.5d ? father._chest : mother._chest,
				null, null,
				rand.NextDouble() < 0.5d ? father._wrist : mother._wrist,
				rand.NextDouble() < 0.5d ? father._hands : mother._hands,
				rand.NextDouble() < 0.5d ? father._waist : mother._waist,
				rand.NextDouble() < 0.5d ? father._legs : mother._legs,
				rand.NextDouble() < 0.5d ? father._feet : mother._feet,
				finger1Id, finger2Id, trinket1Id, trinket2Id, mainHandId, offHandId,
				rand.NextDouble() < 0.5d ? father._ranged : mother._ranged,
				rand.NextDouble() < 0.5d ? father._projectile : mother._projectile,
				rand.NextDouble() < 0.5d ? father._projectileBag : mother._projectileBag,
				rand.NextDouble() < 0.5d ? father._headEnchant : mother._headEnchant,
				rand.NextDouble() < 0.5d ? father._shouldersEnchant : mother._shouldersEnchant,
				rand.NextDouble() < 0.5d ? father._backEnchant : mother._backEnchant,
				rand.NextDouble() < 0.5d ? father._chestEnchant : mother._chestEnchant,
				rand.NextDouble() < 0.5d ? father._wristEnchant : mother._wristEnchant,
				rand.NextDouble() < 0.5d ? father._handsEnchant : mother._handsEnchant,
				rand.NextDouble() < 0.5d ? father._legsEnchant : mother._legsEnchant,
				rand.NextDouble() < 0.5d ? father._feetEnchant : mother._feetEnchant,
				rand.NextDouble() < 0.5d ? father._finger1Enchant : mother._finger1Enchant,
				rand.NextDouble() < 0.5d ? father._finger2Enchant : mother._finger2Enchant,
				rand.NextDouble() < 0.5d ? father._mainHandEnchant : mother._mainHandEnchant,
				rand.NextDouble() < 0.5d ? father._offHandEnchant : mother._offHandEnchant,
				rand.NextDouble() < 0.5d ? father._rangedEnchant : mother._rangedEnchant);
			character.ActiveBuffs.AddRange(_character.ActiveBuffs);
			foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
				character.CalculationOptions.Add(kvp.Key, kvp.Value);
			character.Class = _character.Class;
			character.Talents = _character.Talents;
			character.RecalculateSetBonuses();
			return character;
		}

		private Character BuildMutantCharacter(Character parent)
		{
			int targetMutations = 2;
			while (targetMutations < 32 && rand.NextDouble() < 0.75d) targetMutations++;
			double mutationChance = (double)targetMutations / 32d;

            string finger1Id = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Finger1) ? lockedIds[rand.Next(lockedIds.Length)] : fingerIds[rand.Next(fingerIds.Length)]) : parent._finger1;
            string finger2Id = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Finger2) ? lockedIds[rand.Next(lockedIds.Length)] : fingerIds[rand.Next(fingerIds.Length)]) : parent._finger2;
			while (finger1Id == finger2Id && finger1Id != null && uniqueIds[finger1Id])
			{
                finger1Id = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Finger1) ? lockedIds[rand.Next(lockedIds.Length)] : fingerIds[rand.Next(fingerIds.Length)]) : parent._finger1;
                finger2Id = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Finger2) ? lockedIds[rand.Next(lockedIds.Length)] : fingerIds[rand.Next(fingerIds.Length)]) : parent._finger2;
			}

            string trinket1Id = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Trinket1) ? lockedIds[rand.Next(lockedIds.Length)] : trinketIds[rand.Next(trinketIds.Length)]) : parent._trinket1;
            string trinket2Id = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Trinket2) ? lockedIds[rand.Next(lockedIds.Length)] : trinketIds[rand.Next(trinketIds.Length)]) : parent._trinket2;
			while (trinket1Id == trinket2Id && trinket1Id != null && uniqueIds[trinket1Id])
			{
                trinket1Id = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Trinket1) ? lockedIds[rand.Next(lockedIds.Length)] : trinketIds[rand.Next(trinketIds.Length)]) : parent._trinket1;
                trinket2Id = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Trinket2) ? lockedIds[rand.Next(lockedIds.Length)] : trinketIds[rand.Next(trinketIds.Length)]) : parent._trinket2;
			}

            string mainHandId = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.MainHand) ? lockedIds[rand.Next(lockedIds.Length)] : mainHandIds[rand.Next(mainHandIds.Length)]) : parent._mainHand;
            string offHandId = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.OffHand) ? lockedIds[rand.Next(lockedIds.Length)] : offHandIds[rand.Next(offHandIds.Length)]) : parent._offHand;
			while (mainHandId == offHandId && mainHandId != null && uniqueIds[mainHandId])
			{
                mainHandId = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.MainHand) ? lockedIds[rand.Next(lockedIds.Length)] : mainHandIds[rand.Next(mainHandIds.Length)]) : parent._mainHand;
                offHandId = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.OffHand) ? lockedIds[rand.Next(lockedIds.Length)] : offHandIds[rand.Next(offHandIds.Length)]) : parent._offHand;
			}

			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Head) ? lockedIds[rand.Next(lockedIds.Length)] : headIds[rand.Next(headIds.Length)]) : parent._head,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Neck) ? lockedIds[rand.Next(lockedIds.Length)] : neckIds[rand.Next(neckIds.Length)]) : parent._neck,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Shoulders) ? lockedIds[rand.Next(lockedIds.Length)] : shouldersIds[rand.Next(shouldersIds.Length)]) : parent._shoulders,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Back) ? lockedIds[rand.Next(lockedIds.Length)] : backIds[rand.Next(backIds.Length)]) : parent._back,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Chest) ? lockedIds[rand.Next(lockedIds.Length)] : chestIds[rand.Next(chestIds.Length)]) : parent._chest, 
				null, null,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Wrist) ? lockedIds[rand.Next(lockedIds.Length)] : wristIds[rand.Next(wristIds.Length)]) : parent._wrist,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Hands) ? lockedIds[rand.Next(lockedIds.Length)] : handsIds[rand.Next(handsIds.Length)]) : parent._hands,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Waist) ? lockedIds[rand.Next(lockedIds.Length)] : waistIds[rand.Next(waistIds.Length)]) : parent._waist,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Legs) ? lockedIds[rand.Next(lockedIds.Length)] : legsIds[rand.Next(legsIds.Length)]) : parent._legs,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Feet) ? lockedIds[rand.Next(lockedIds.Length)] : feetIds[rand.Next(feetIds.Length)]) : parent._feet,
				finger1Id, finger2Id, trinket1Id, trinket2Id, mainHandId, offHandId,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Ranged) ? lockedIds[rand.Next(lockedIds.Length)] : rangedIds[rand.Next(rangedIds.Length)]) : parent._ranged,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Projectile) ? lockedIds[rand.Next(lockedIds.Length)] : projectileIds[rand.Next(projectileIds.Length)]) : parent._projectile,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.ProjectileBag) ? lockedIds[rand.Next(lockedIds.Length)] : projectileBagIds[rand.Next(projectileBagIds.Length)]) : parent._projectileBag,
				rand.NextDouble() < mutationChance ? headEnchantIds[rand.Next(headEnchantIds.Length)] : parent._headEnchant, 
				rand.NextDouble() < mutationChance ? shouldersEnchantIds[rand.Next(shouldersEnchantIds.Length)] : parent._shouldersEnchant,
				rand.NextDouble() < mutationChance ? backEnchantIds[rand.Next(backEnchantIds.Length)] : parent._backEnchant,
				rand.NextDouble() < mutationChance ? chestEnchantIds[rand.Next(chestEnchantIds.Length)] : parent._chestEnchant,
				rand.NextDouble() < mutationChance ? wristEnchantIds[rand.Next(wristEnchantIds.Length)] : parent._wristEnchant,
				rand.NextDouble() < mutationChance ? handsEnchantIds[rand.Next(handsEnchantIds.Length)] : parent._handsEnchant,
				rand.NextDouble() < mutationChance ? legsEnchantIds[rand.Next(legsEnchantIds.Length)] : parent._legsEnchant,
				rand.NextDouble() < mutationChance ? feetEnchantIds[rand.Next(feetEnchantIds.Length)] : parent._feetEnchant,
				rand.NextDouble() < mutationChance ? fingerEnchantIds[rand.Next(fingerEnchantIds.Length)] : parent._finger1Enchant,
				rand.NextDouble() < mutationChance ? fingerEnchantIds[rand.Next(fingerEnchantIds.Length)] : parent._finger2Enchant,
				rand.NextDouble() < mutationChance ? mainHandEnchantIds[rand.Next(mainHandEnchantIds.Length)] : parent._mainHandEnchant,
				rand.NextDouble() < mutationChance ? offHandEnchantIds[rand.Next(offHandEnchantIds.Length)] : parent._offHandEnchant,
				rand.NextDouble() < mutationChance ? rangedEnchantIds[rand.Next(rangedEnchantIds.Length)] : parent._rangedEnchant);
			character.ActiveBuffs.AddRange(_character.ActiveBuffs);
			foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
				character.CalculationOptions.Add(kvp.Key, kvp.Value);
			character.Class = _character.Class;
			character.Talents = _character.Talents;
			character.RecalculateSetBonuses();
			return character;

		}

		private Character BuildSingleItemSwapCharacter(Character baseCharacter, Character.CharacterSlot slot, string id)
		{
			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
				slot == Character.CharacterSlot.Head ? id : baseCharacter._head,
				slot == Character.CharacterSlot.Neck ? id : baseCharacter._neck,
				slot == Character.CharacterSlot.Shoulders ? id : baseCharacter._shoulders,
				slot == Character.CharacterSlot.Back ? id : baseCharacter._back,
				slot == Character.CharacterSlot.Chest ? id : baseCharacter._chest,
				null, null,
				slot == Character.CharacterSlot.Wrist ? id : baseCharacter._wrist,
				slot == Character.CharacterSlot.Hands ? id : baseCharacter._hands,
				slot == Character.CharacterSlot.Waist ? id : baseCharacter._waist,
				slot == Character.CharacterSlot.Legs ? id : baseCharacter._legs,
				slot == Character.CharacterSlot.Feet ? id : baseCharacter._feet,
				slot == Character.CharacterSlot.Finger1 ? id : baseCharacter._finger1,
				slot == Character.CharacterSlot.Finger2 ? id : baseCharacter._finger2,
				slot == Character.CharacterSlot.Trinket1 ? id : baseCharacter._trinket1,
				slot == Character.CharacterSlot.Trinket2 ? id : baseCharacter._trinket2,
				slot == Character.CharacterSlot.MainHand ? id : baseCharacter._mainHand,
				slot == Character.CharacterSlot.OffHand ? id : baseCharacter._offHand,
				slot == Character.CharacterSlot.Ranged ? id : baseCharacter._ranged,
				slot == Character.CharacterSlot.Projectile ? id : baseCharacter._projectile,
				slot == Character.CharacterSlot.ProjectileBag ? id : baseCharacter._projectileBag,
				baseCharacter._headEnchant,
				baseCharacter._shouldersEnchant,
				baseCharacter._backEnchant,
				baseCharacter._chestEnchant,
				baseCharacter._wristEnchant,
				baseCharacter._handsEnchant,
				baseCharacter._legsEnchant,
				baseCharacter._feetEnchant,
				baseCharacter._finger1Enchant,
				baseCharacter._finger2Enchant,
				baseCharacter._mainHandEnchant,
				baseCharacter._offHandEnchant,
				baseCharacter._rangedEnchant);
			character.ActiveBuffs.AddRange(_character.ActiveBuffs);
			foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
				character.CalculationOptions.Add(kvp.Key, kvp.Value);
			character.Class = _character.Class;
			character.Talents = _character.Talents;
			character.RecalculateSetBonuses();
			return character;
		}

		private Character BuildSingleEnchantSwapCharacter(Character baseCharacter, Character.CharacterSlot slot, int id)
		{
			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
				baseCharacter._head,
				baseCharacter._neck,
				baseCharacter._shoulders,
				baseCharacter._back,
				baseCharacter._chest,
				null, null,
				baseCharacter._wrist,
				baseCharacter._hands,
				baseCharacter._waist,
				baseCharacter._legs,
				baseCharacter._feet,
				baseCharacter._finger1,
				baseCharacter._finger2,
				baseCharacter._trinket1,
				baseCharacter._trinket2,
				baseCharacter._mainHand,
				baseCharacter._offHand,
				baseCharacter._ranged,
				baseCharacter._projectile,
				baseCharacter._projectileBag,
				slot == Character.CharacterSlot.Head ? id : baseCharacter._headEnchant,
				slot == Character.CharacterSlot.Shoulders ? id : baseCharacter._shouldersEnchant,
				slot == Character.CharacterSlot.Back ? id : baseCharacter._backEnchant,
				slot == Character.CharacterSlot.Chest ? id : baseCharacter._chestEnchant,
				slot == Character.CharacterSlot.Wrist ? id : baseCharacter._wristEnchant,
				slot == Character.CharacterSlot.Hands ? id : baseCharacter._handsEnchant,
				slot == Character.CharacterSlot.Legs ? id : baseCharacter._legsEnchant,
				slot == Character.CharacterSlot.Feet ? id : baseCharacter._feetEnchant,
				slot == Character.CharacterSlot.Finger1 ? id : baseCharacter._finger1Enchant,
				slot == Character.CharacterSlot.Finger2 ? id : baseCharacter._finger2Enchant,
				slot == Character.CharacterSlot.MainHand ? id : baseCharacter._mainHandEnchant,
				slot == Character.CharacterSlot.OffHand ? id : baseCharacter._offHandEnchant,
				slot == Character.CharacterSlot.Ranged ? id : baseCharacter._rangedEnchant);
			character.ActiveBuffs.AddRange(_character.ActiveBuffs);
			foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
				character.CalculationOptions.Add(kvp.Key, kvp.Value);
			character.Class = _character.Class;
			character.Talents = _character.Talents;
			character.RecalculateSetBonuses();
			return character;
		}

		private string[] GetPossibleGemmedIdsForItem(Item item, List<int> gemIDs, List<int> metaGemIDs)
		{
			List<string> possibleGemmedIds = new List<string>();
			if (!_allGemmings)
			{
				foreach (Item knownItem in ItemCache.AllItems)
					if (knownItem.Id == item.Id)
						possibleGemmedIds.Add(knownItem.GemmedId);
			}
			else
			{
			List<int> possibleId1s, possibleId2s, possibleId3s = null;
			switch (item.Sockets.Color1)
			{
				case Item.ItemSlot.Meta:
					possibleId1s = metaGemIDs;
					break;
				case Item.ItemSlot.Red:
				case Item.ItemSlot.Orange:
				case Item.ItemSlot.Yellow:
				case Item.ItemSlot.Green:
				case Item.ItemSlot.Blue:
				case Item.ItemSlot.Purple:
				case Item.ItemSlot.Prismatic:
					possibleId1s = gemIDs;
					break;
				default:
					possibleId1s = new List<int>(new int[] { 0 });
					break;
			}
			switch (item.Sockets.Color2)
			{
				case Item.ItemSlot.Meta:
					possibleId2s = metaGemIDs;
					break;
				case Item.ItemSlot.Red:
				case Item.ItemSlot.Orange:
				case Item.ItemSlot.Yellow:
				case Item.ItemSlot.Green:
				case Item.ItemSlot.Blue:
				case Item.ItemSlot.Purple:
				case Item.ItemSlot.Prismatic:
					possibleId2s = gemIDs;
					break;
				default:
					possibleId2s = new List<int>(new int[] { 0 });
					break;
			}
			switch (item.Sockets.Color3)
			{
				case Item.ItemSlot.Meta:
					possibleId3s = metaGemIDs;
					break;
				case Item.ItemSlot.Red:
				case Item.ItemSlot.Orange:
				case Item.ItemSlot.Yellow:
				case Item.ItemSlot.Green:
				case Item.ItemSlot.Blue:
				case Item.ItemSlot.Purple:
				case Item.ItemSlot.Prismatic:
					possibleId3s = gemIDs;
					break;
				default:
					possibleId3s = new List<int>(new int[] { 0 });
					break;
			}

			int id0 = item.Id;
			foreach (int id1 in possibleId1s)
				foreach (int id2 in possibleId2s)
					foreach (int id3 in possibleId3s)
						possibleGemmedIds.Add(string.Format("{0}.{1}.{2}.{3}", id0, id1, id2, id3));
			}

			return possibleGemmedIds.ToArray();
		}

		private string[] FilterList(List<string> unfilteredList)
		{
			List<string> filteredList = new List<string>();
			List<StatsColors> filteredStatsColors = new List<StatsColors>();
			foreach (string gemmedId in unfilteredList)
			{
				if (gemmedId == null)
				{
					filteredList.Add(gemmedId);
					continue;
				}
				Item item = ItemCache.FindItemById(gemmedId);
				int meta = 0, red = 0, yellow = 0, blue = 0;
				foreach (Item gem in new Item[] { item.Gem1, item.Gem2, item.Gem3})
					if (gem != null)
						switch (gem.Slot)
						{
							case Item.ItemSlot.Meta:		meta++;						break;
							case Item.ItemSlot.Red:			red++;						break;
							case Item.ItemSlot.Orange:		red++; yellow++;			break;
							case Item.ItemSlot.Yellow:		yellow++;					break;
							case Item.ItemSlot.Green:		yellow++; blue++;			break;
							case Item.ItemSlot.Blue:		blue++;						break;
							case Item.ItemSlot.Purple:		blue++; red++;				break;
							case Item.ItemSlot.Prismatic:	red++; yellow++; blue++;	break;
						}

				StatsColors statsColorsA = new StatsColors()
				{
					GemmedId = gemmedId,
					SetName = item.SetName,
					Stats = item.GetTotalStats(),
					Meta = meta,
					Red = red,
					Yellow = yellow,
					Blue = blue
				};
				bool addItem = true;
				List<StatsColors> removeItems = new List<StatsColors>();
				foreach (StatsColors statsColorsB in filteredStatsColors)
				{
					ArrayUtils.CompareResult compare = statsColorsA.CompareTo(statsColorsB);
					if (compare == ArrayUtils.CompareResult.GreaterThan) //A>B
					{
						removeItems.Add(statsColorsB);
					}
					else if (compare == ArrayUtils.CompareResult.Equal || compare == ArrayUtils.CompareResult.LessThan)
					{
						addItem = false;
						break;
					}
				}
				foreach(StatsColors removeItem in removeItems)
					filteredStatsColors.Remove(removeItem);
				if (addItem) filteredStatsColors.Add(statsColorsA);
			}
			foreach (StatsColors statsColors in filteredStatsColors)
			{
				filteredList.Add(statsColors.GemmedId);
			}
			return filteredList.ToArray();
		}

		private class OptimizationRequirement
		{
			public string Calculation;
			public bool LessThan;
			public float Value;
		}

		private class StatsColors
		{
			public string GemmedId;
			public Stats Stats;
			public int Meta;
			public int Red;
			public int Yellow;
			public int Blue;
			public string SetName;

			public ArrayUtils.CompareResult CompareTo(StatsColors other)
			{
				if (this.SetName != other.SetName) return ArrayUtils.CompareResult.Unequal;

				int compare = Meta.CompareTo(other.Meta);
				bool haveLessThan = compare < 0;
				bool haveGreaterThan = compare > 0;
				if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;

				compare = Red.CompareTo(other.Red);
				haveLessThan |= compare < 0;
				haveGreaterThan |= compare > 0;
				if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;

				compare = Yellow.CompareTo(other.Yellow);
				haveLessThan |= compare < 0;
				haveGreaterThan |= compare > 0;
				if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;

				compare = Blue.CompareTo(other.Blue);
				haveLessThan |= compare < 0;
				haveGreaterThan |= compare > 0;
				if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;

				ArrayUtils.CompareResult compareResult = Stats.CompareTo(other.Stats);
				if (compareResult == ArrayUtils.CompareResult.Unequal) return ArrayUtils.CompareResult.Unequal;
				haveLessThan |= compareResult == ArrayUtils.CompareResult.LessThan;
				haveGreaterThan |= compareResult == ArrayUtils.CompareResult.GreaterThan;
				if (haveGreaterThan && haveLessThan) return ArrayUtils.CompareResult.Unequal;
				else if (haveGreaterThan) return ArrayUtils.CompareResult.GreaterThan;
				else if (haveLessThan) return ArrayUtils.CompareResult.LessThan;
				else return ArrayUtils.CompareResult.Equal;
			}
			public static bool operator ==(StatsColors x, StatsColors y)
			{
				if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
				return x.Meta == y.Meta && x.Red == y.Red && x.Yellow == y.Yellow 
					&& x.Blue == y.Blue && x.Stats == y.Stats;
			}
            public override int GetHashCode()
            {
                return Stats.GetHashCode()^GemmedId.GetHashCode();
            }
            public override bool Equals(object obj)
            {

                if(obj != null && obj.GetType() == this.GetType())
                {
                    return this == (obj as StatsColors);
                }
                return base.Equals(obj);
            }
			public static bool operator !=(StatsColors x, StatsColors y)
			{
				return !(x == y);
			}
		}

        private void buttonUpgrades_Click(object sender, EventArgs e)
        {
            buttonUpgrades.Text = "Calculating...";
            buttonOptimize.Enabled = buttonUpgrades.Enabled = radioButtonAllGemmings.Enabled = radioButtonKnownGemmingsOnly.Enabled =
                trackBarThoroughness.Enabled = false;

            _allGemmings = radioButtonAllGemmings.Checked;
            _thoroughness = trackBarThoroughness.Value;
            _calculationToOptimize = GetCalculationStringFromComboBox(comboBoxCalculationToOptimize);
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
            _requirements = requirements.ToArray();

            _worker = _workerUpgrades;
            _worker.RunWorkerAsync();
        }
	}
}
