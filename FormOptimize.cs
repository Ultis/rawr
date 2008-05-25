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

            radioButtonAllGemmings.Checked = Properties.Optimizer.Default.AllGemmings;
            radioButtonKnownGemmingsOnly.Checked = !Properties.Optimizer.Default.AllGemmings;
            trackBarThoroughness.Value = Properties.Optimizer.Default.Thoroughness;
            string calculationString = Properties.Optimizer.Default.CalculationToOptimize;
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

                if (!PopulateAvailableIds(itemCacheMain))
                {
                    e.Result = null;
                    e.Cancel = true;
                    return;
                }
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
					_character.Back = bestCharacter.Back == null ? null : ItemCache.FindItemById(bestCharacter.Back.GemmedId);
					_character.Chest = bestCharacter.Chest == null ? null : ItemCache.FindItemById(bestCharacter.Chest.GemmedId);
					_character.Feet = bestCharacter.Feet == null ? null : ItemCache.FindItemById(bestCharacter.Feet.GemmedId);
					_character.Finger1 = bestCharacter.Finger1 == null ? null : ItemCache.FindItemById(bestCharacter.Finger1.GemmedId);
					_character.Finger2 = bestCharacter.Finger2 == null ? null : ItemCache.FindItemById(bestCharacter.Finger2.GemmedId);
					_character.Hands = bestCharacter.Hands == null ? null : ItemCache.FindItemById(bestCharacter.Hands.GemmedId);
					_character.Head = bestCharacter.Head == null ? null : ItemCache.FindItemById(bestCharacter.Head.GemmedId);
					_character.Legs = bestCharacter.Legs == null ? null : ItemCache.FindItemById(bestCharacter.Legs.GemmedId);
					_character.MainHand = bestCharacter.MainHand == null ? null : ItemCache.FindItemById(bestCharacter.MainHand.GemmedId);
					_character.Neck = bestCharacter.Neck == null ? null : ItemCache.FindItemById(bestCharacter.Neck.GemmedId);
					_character.OffHand = bestCharacter.OffHand == null ? null : ItemCache.FindItemById(bestCharacter.OffHand.GemmedId);
					_character.Projectile = bestCharacter.Projectile == null ? null : ItemCache.FindItemById(bestCharacter.Projectile.GemmedId);
					_character.ProjectileBag = bestCharacter.ProjectileBag == null ? null : ItemCache.FindItemById(bestCharacter.ProjectileBag.GemmedId);
					_character.Ranged = bestCharacter.Ranged == null ? null : ItemCache.FindItemById(bestCharacter.Ranged.GemmedId);
					_character.Shoulders = bestCharacter.Shoulders == null ? null : ItemCache.FindItemById(bestCharacter.Shoulders.GemmedId);
					_character.Trinket1 = bestCharacter.Trinket1 == null ? null : ItemCache.FindItemById(bestCharacter.Trinket1.GemmedId);
					_character.Trinket2 = bestCharacter.Trinket2 == null ? null : ItemCache.FindItemById(bestCharacter.Trinket2.GemmedId);
					_character.Waist = bestCharacter.Waist == null ? null : ItemCache.FindItemById(bestCharacter.Waist.GemmedId);
					_character.Wrist = bestCharacter.Wrist == null ? null : ItemCache.FindItemById(bestCharacter.Wrist.GemmedId);
					_character.BackEnchant = bestCharacter.BackEnchant;
					_character.ChestEnchant = bestCharacter.ChestEnchant;
					_character.FeetEnchant = bestCharacter.FeetEnchant;
					_character.Finger1Enchant = bestCharacter.Finger1Enchant;
					_character.Finger2Enchant = bestCharacter.Finger2Enchant;
					_character.HandsEnchant = bestCharacter.HandsEnchant;
					_character.HeadEnchant = bestCharacter.HeadEnchant;
					_character.LegsEnchant = bestCharacter.LegsEnchant;
					_character.MainHandEnchant = bestCharacter.MainHandEnchant;
					_character.OffHandEnchant = bestCharacter.OffHandEnchant;
					_character.RangedEnchant = bestCharacter.RangedEnchant;
					_character.ShouldersEnchant = bestCharacter.ShouldersEnchant;
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
            bool savedAllGemmings = _allGemmings;
            try
            {
                ItemCacheInstance itemCacheOptimize = new ItemCacheInstance(itemCacheMain);
                ItemCache.Instance = itemCacheOptimize;
                if ((_allGemmings || _thoroughness > 100) && Properties.Optimizer.Default.WarningsEnabled)
                {
                    if (MessageBox.Show("The upgrade evaluations perform an optimization for each relevant item. With your settings this might take a long time. Consider using known gemmings only and lower thoroughness." + Environment.NewLine + Environment.NewLine + "Do you want to continue with upgrade evaluations?", "Optimizer Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                if (!PopulateAvailableIds(itemCacheMain))
                {
                    e.Cancel = true;
                    return;
                }
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
                            _allGemmings = true; // for the new added items check all gemmings
                            PopulateLockedItems(item);
                            _allGemmings = savedAllGemmings;
                            lockedSlot = slot;
                            if (lockedSlot == Character.CharacterSlot.Finger1 && item.Unique && _character.Finger2 != null && _character.Finger2.Id == item.Id)
                            {
                                lockedSlot = Character.CharacterSlot.Finger2;
                            }
                            if (lockedSlot == Character.CharacterSlot.Trinket1 && item.Unique && _character.Trinket2 != null && _character.Trinket2.Id == item.Id)
                            {
                                lockedSlot = Character.CharacterSlot.Trinket2;
                            }
                            _character = BuildSingleItemSwapCharacter(_character, lockedSlot, lockedItems[0]);
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
                                switch (lockedSlot)
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
                                    case Character.CharacterSlot.Finger2:
                                        item = bestCharacter.Finger2;
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
                                    case Character.CharacterSlot.Trinket2:
                                        item = bestCharacter.Trinket2;
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
                                /*itemCalc.OverallPoints = bestCalculations.OverallPoints - baseCalculations.OverallPoints;
                                float[] subPoints = new float[bestCalculations.SubPoints.Length];
                                for (int j = 0; j < bestCalculations.SubPoints.Length; j++)
                                {
                                    subPoints[j] = bestCalculations.SubPoints[j] - baseCalculations.SubPoints[j];
                                }
                                itemCalc.SubPoints = subPoints;*/
                                itemCalc.OverallPoints = best - baseValue;

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
                buttonUpgrades.Text = "Build Upgrade List";
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

        Item[] metaGemItems;
        Item[] gemItems;
        //SortedList<Item, bool> uniqueItems; not needed since we store the items themselves, we can just check Unique on the item
		Item[] headItems, neckItems, shouldersItems, backItems, chestItems, wristItems, handsItems, waistItems,
					legsItems, feetItems, fingerItems, trinketItems, mainHandItems, offHandItems, rangedItems, 
					projectileItems, projectileBagItems;
		Enchant[] backEnchants, chestEnchants, feetEnchants, fingerEnchants, handsEnchants, headEnchants,
			legsEnchants, shouldersEnchants, mainHandEnchants, offHandEnchants, rangedEnchants, wristEnchants;
        Item[] lockedItems;
        Character.CharacterSlot lockedSlot = Character.CharacterSlot.None;
		Random rand;

        private void PopulateLockedItems(Item item)
        {
            lockedItems = GetPossibleGemmedItemsForItem(item, gemItems, metaGemItems);
            //foreach (Item possibleGemmedItem in lockedItems)
            //    uniqueItems[possibleGemmedItem] = item.Unique;
        }

        private bool PopulateAvailableIds(ItemCacheInstance itemCacheMain)
        {
            Dictionary<int, Item> relevantItemMap = new Dictionary<int, Item>();
            foreach (Item relevantItem in itemCacheMain.RelevantItems)
            {
                relevantItemMap[relevantItem.Id] = relevantItem;
            }

            List<int> itemIds = new List<int>(_character.AvailableItems);
            List<int> removeIds = new List<int>();
            List<Item> metaGemItemList = new List<Item>();
            List<Item> gemItemList = new List<Item>();
            foreach (int id in _character.AvailableItems)
            {
                if (id > 0)
                {
                    Item availableItem;
                    relevantItemMap.TryGetValue(id, out availableItem);
                    if (availableItem != null)
                    {
                        switch (availableItem.Slot)
                        {
                            case Item.ItemSlot.Meta:
                                metaGemItemList.Add(availableItem);
                                removeIds.Add(availableItem.Id);
                                break;
                            case Item.ItemSlot.Red:
                            case Item.ItemSlot.Orange:
                            case Item.ItemSlot.Yellow:
                            case Item.ItemSlot.Green:
                            case Item.ItemSlot.Blue:
                            case Item.ItemSlot.Purple:
                            case Item.ItemSlot.Prismatic:
                                gemItemList.Add(availableItem);
                                removeIds.Add(availableItem.Id);
                                break;
                        }
                    }
                }
            }
            if (gemItemList.Count == 0) gemItemList.Add(null);
            if (metaGemItemList.Count == 0) metaGemItemList.Add(null);
            itemIds.RemoveAll(new Predicate<int>(delegate(int id) { return id < 0 || removeIds.Contains(id); }));

            metaGemItems = metaGemItemList.ToArray();
            gemItems = FilterList(gemItemList);

            List<Item> headItemList = new List<Item>();
            List<Item> neckItemList = new List<Item>();
            List<Item> shouldersItemList = new List<Item>();
            List<Item> backItemList = new List<Item>();
            List<Item> chestItemList = new List<Item>();
            List<Item> wristItemList = new List<Item>();
            List<Item> handsItemList = new List<Item>();
            List<Item> waistItemList = new List<Item>();
            List<Item> legsItemList = new List<Item>();
            List<Item> feetItemList = new List<Item>();
            List<Item> fingerItemList = new List<Item>();
            List<Item> trinketItemList = new List<Item>();
            List<Item> mainHandItemList = new List<Item>();
            List<Item> offHandItemList = new List<Item>();
            List<Item> rangedItemList = new List<Item>();
            List<Item> projectileItemList = new List<Item>();
            List<Item> projectileBagItemList = new List<Item>();

            List<Enchant> backEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Back, _character.AvailableItems))
                backEnchantList.Add(enchant);
            backEnchants = backEnchantList.ToArray();
            List<Enchant> chestEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Chest, _character.AvailableItems))
                chestEnchantList.Add(enchant);
            chestEnchants = chestEnchantList.ToArray();
            List<Enchant> feetEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Feet, _character.AvailableItems))
                feetEnchantList.Add(enchant);
            feetEnchants = feetEnchantList.ToArray();
            List<Enchant> fingerEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Finger, _character.AvailableItems))
                fingerEnchantList.Add(enchant);
            fingerEnchants = fingerEnchantList.ToArray();
            List<Enchant> handsEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Hands, _character.AvailableItems))
                handsEnchantList.Add(enchant);
            handsEnchants = handsEnchantList.ToArray();
            List<Enchant> headEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Head, _character.AvailableItems))
                headEnchantList.Add(enchant);
            headEnchants = headEnchantList.ToArray();
            List<Enchant> legsEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Legs, _character.AvailableItems))
                legsEnchantList.Add(enchant);
            legsEnchants = legsEnchantList.ToArray();
            List<Enchant> shouldersEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Shoulders, _character.AvailableItems))
                shouldersEnchantList.Add(enchant);
            shouldersEnchants = shouldersEnchantList.ToArray();
            List<Enchant> mainHandEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.MainHand, _character.AvailableItems))
                mainHandEnchantList.Add(enchant);
            mainHandEnchants = mainHandEnchantList.ToArray();
            List<Enchant> offHandEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.OffHand, _character.AvailableItems))
                offHandEnchantList.Add(enchant);
            offHandEnchants = offHandEnchantList.ToArray();
            List<Enchant> rangedEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Ranged, _character.AvailableItems))
                rangedEnchantList.Add(enchant);
            rangedEnchants = rangedEnchantList.ToArray();
            List<Enchant> wristEnchantList = new List<Enchant>();
            foreach (Enchant enchant in Enchant.FindEnchants(Item.ItemSlot.Wrist, _character.AvailableItems))
                wristEnchantList.Add(enchant);
            wristEnchants = wristEnchantList.ToArray();

            //Dictionary<Item, bool> uniqueDict = new Dictionary<Item, bool>();
            Item item = null;
            Item[] possibleGemmedItems = null;
            foreach (int itemId in itemIds)
            {
                relevantItemMap.TryGetValue(itemId, out item);

                if (item != null)
                {
                    possibleGemmedItems = GetPossibleGemmedItemsForItem(item, gemItems, metaGemItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Head)) headItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Neck)) neckItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Shoulders)) shouldersItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Back)) backItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Chest)) chestItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Wrist)) wristItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Hands)) handsItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Waist)) waistItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Legs)) legsItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Feet)) feetItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Finger1)) fingerItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Trinket1)) trinketItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.MainHand)) mainHandItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.OffHand)) offHandItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Ranged)) rangedItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.Projectile)) projectileItemList.AddRange(possibleGemmedItems);
                    if (item.FitsInSlot(Character.CharacterSlot.ProjectileBag)) projectileBagItemList.AddRange(possibleGemmedItems);

                    //foreach (Item possibleGemmedItem in possibleGemmedItems)
                    //    uniqueDict.Add(possibleGemmedItem, item.Unique);
                }
            }
            //uniqueItems = new SortedList<Item, bool>(uniqueDict);

            if (headItemList.Count == 0) headItemList.Add(null);
            if (neckItemList.Count == 0) neckItemList.Add(null);
            if (shouldersItemList.Count == 0) shouldersItemList.Add(null);
            if (backItemList.Count == 0) backItemList.Add(null);
            if (chestItemList.Count == 0) chestItemList.Add(null);
            if (wristItemList.Count == 0) wristItemList.Add(null);
            if (handsItemList.Count == 0) handsItemList.Add(null);
            if (waistItemList.Count == 0) waistItemList.Add(null);
            if (legsItemList.Count == 0) legsItemList.Add(null);
            if (feetItemList.Count == 0) feetItemList.Add(null);
            if (rangedItemList.Count == 0) rangedItemList.Add(null);
            if (projectileItemList.Count == 0) projectileItemList.Add(null);
            if (projectileBagItemList.Count == 0) projectileBagItemList.Add(null);
            fingerItemList.Add(null);
            trinketItemList.Add(null);
            mainHandItemList.Add(null);
            offHandItemList.Add(null);

            headItems = FilterList(headItemList);
            neckItems = FilterList(neckItemList);
            shouldersItems = FilterList(shouldersItemList);
            backItems = FilterList(backItemList);
            chestItems = FilterList(chestItemList);
            wristItems = FilterList(wristItemList);
            handsItems = FilterList(handsItemList);
            waistItems = FilterList(waistItemList);
            legsItems = FilterList(legsItemList);
            feetItems = FilterList(feetItemList);
            fingerItems = fingerItemList.ToArray(); //When one ring/trinket is completely better than another
            trinketItems = trinketItemList.ToArray(); //you may still want to use both, so don't filter
            mainHandItems = FilterList(mainHandItemList);
            offHandItems = FilterList(offHandItemList);
            rangedItems = FilterList(rangedItemList);
            projectileItems = FilterList(projectileItemList);
            projectileBagItems = FilterList(projectileBagItemList);

            if (Properties.Optimizer.Default.WarningsEnabled)
            {
                int gemLimit = 8;
                int itemLimit = 512;
                int enchantLimit = 8;

                List<string> emptyList = new List<string>();
                List<string> tooManyList = new List<string>();

                CalculateWarnings(gemItems, "Gems", emptyList, tooManyList, gemLimit);
                CalculateWarnings(metaGemItems, "Meta Gems", emptyList, tooManyList, gemLimit);

                CalculateWarnings(headItems, "Head Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(neckItems, "Neck Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(shouldersItems, "Shoulder Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(backItems, "Back Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(chestItems, "Chest Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(wristItems, "Wrist Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(handsItems, "Hands Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(waistItems, "Waist Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(legsItems, "Legs Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(feetItems, "Feet Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(fingerItems, "Finger Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(trinketItems, "Trinket Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(mainHandItems, "Main Hand Items", emptyList, tooManyList, itemLimit);
                CalculateWarnings(offHandItems, "Offhand Items", null, tooManyList, itemLimit);
                CalculateWarnings(rangedItems, "Ranged Items", null, tooManyList, itemLimit);
                CalculateWarnings(projectileItems, "Projectile Items", null, tooManyList, itemLimit);
                CalculateWarnings(projectileBagItems, "Projectile Bag Items", null, tooManyList, itemLimit);

                CalculateWarnings(backEnchants, "Back Enchants", emptyList, tooManyList, enchantLimit);
                CalculateWarnings(chestEnchants, "Chest Enchants", emptyList, tooManyList, enchantLimit);
                CalculateWarnings(feetEnchants, "Feet Enchants", emptyList, tooManyList, enchantLimit);
                CalculateWarnings(fingerEnchants, "Finger Enchants", null, tooManyList, enchantLimit);
                CalculateWarnings(handsEnchants, "Hands Enchants", emptyList, tooManyList, enchantLimit);
                CalculateWarnings(headEnchants, "Head Enchants", emptyList, tooManyList, enchantLimit);
                CalculateWarnings(legsEnchants, "Legs Enchants", emptyList, tooManyList, enchantLimit);
                CalculateWarnings(shouldersEnchants, "Shoulder Enchants", emptyList, tooManyList, enchantLimit);
                CalculateWarnings(mainHandEnchants, "Main Hand Enchants", emptyList, tooManyList, enchantLimit);
                CalculateWarnings(offHandEnchants, "Offhand Enchants", null, tooManyList, enchantLimit);
                CalculateWarnings(rangedEnchants, "Ranged Enchants", null, tooManyList, enchantLimit);
                CalculateWarnings(wristEnchants, "Wrist Enchants", emptyList, tooManyList, enchantLimit);

                if (emptyList.Count + tooManyList.Count > 0)
                {
                    if (emptyList.Count > 5)
                    {
                        emptyList.RemoveRange(5, emptyList.Count - 5);
                        emptyList.Add("...");
                    }
                    if (tooManyList.Count > 5)
                    {
                        tooManyList.RemoveRange(5, tooManyList.Count - 5);
                        tooManyList.Add("...");
                    }
                    if (tooManyList.Count == 0)
                    {
                        // good sizes but some are empty
                        return MessageBox.Show("You have not selected any of the following:" + Environment.NewLine + Environment.NewLine + "\t" + string.Join(Environment.NewLine + "\t", emptyList.ToArray()) + Environment.NewLine + Environment.NewLine + "Do you want to continue with the optimization?", "Optimizer Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                    }
                    else if (emptyList.Count == 0)
                    {
                        return MessageBox.Show("The following slots have a very large number of items selected :" + Environment.NewLine + Environment.NewLine + "\t" + string.Join(Environment.NewLine + "\t", tooManyList.ToArray()) + Environment.NewLine + Environment.NewLine + "Do you want to continue with the optimization?", "Optimizer Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                    }
                    else
                    {
                        return MessageBox.Show("You have not selected any of the following:" + Environment.NewLine + Environment.NewLine + "\t" + string.Join(Environment.NewLine + "\t", emptyList.ToArray()) + Environment.NewLine + Environment.NewLine + "The following slots have a very large number of items selected :" + Environment.NewLine + Environment.NewLine + "\t" + string.Join(Environment.NewLine + "\t", tooManyList.ToArray()) + Environment.NewLine + Environment.NewLine + "Do you want to continue with the optimization?", "Optimizer Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
                    }
                }
            }

            return true;

            /*ulong totalCombinations = (ulong)headItems.Length * (ulong)neckItems.Length * (ulong)shouldersItems.Length * (ulong)backItems.Length
                 * (ulong)chestItems.Length * (ulong)wristItems.Length * (ulong)handsItems.Length * (ulong)waistItems.Length * (ulong)legsItems.Length *
                 (ulong)feetItems.Length * (ulong)fingerItems.Length * (ulong)fingerItems.Length * (ulong)trinketItems.Length * (ulong)trinketItems.Length
                 * (ulong)mainHandItems.Length * (ulong)offHandItems.Length * (ulong)rangedItems.Length * (ulong)projectileItems.Length *
                 (ulong)projectileBagItems.Length * (ulong)headEnchants.Length * (ulong)shouldersEnchants.Length * (ulong)backEnchants.Length
                 * (ulong)chestEnchants.Length * (ulong)wristEnchants.Length * (ulong)handsEnchants.Length * (ulong)legsEnchants.Length *
                 (ulong)feetEnchants.Length * (ulong)fingerEnchants.Length * (ulong)fingerEnchants.Length * (ulong)mainHandItems.Length
                 * (ulong)offHandEnchants.Length * (ulong)rangedEnchants.Length;

            totalCombinations.ToString("f0");*/
        }

        private void CalculateWarnings(Array list, string group, List<string> emptyList, List<string> tooManyList, int tooManyLimit)
        {
            object el0 = (list.Length > 0) ? list.GetValue(0) : null;
            if (emptyList != null && (list.Length == 0 || (list.Length == 1 && (el0 == null || (el0 is Enchant && ((Enchant)el0).Id == 0))))) emptyList.Add(group);
            if (tooManyList != null && list.Length > tooManyLimit) tooManyList.Add(group);
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
                            if (rand.NextDouble() < 0.8 || !_allGemmings)
                            {
                                population[i] = BuildMutantCharacter(popCopy[i1]);
                            }
                            else if (rand.NextDouble() < 0.5)
                            {
                                population[i] = BuildReplaceGemMutantCharacter(popCopy[i1]);
                            }
                            else
                            {
                                population[i] = BuildSwapGemMutantCharacter(popCopy[i1]);
                            }
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
                    results = LookForDirectItemUpgrades(headItems, Character.CharacterSlot.Head, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(neckItems, Character.CharacterSlot.Neck, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(backItems, Character.CharacterSlot.Back, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(shouldersItems, Character.CharacterSlot.Shoulders, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(chestItems, Character.CharacterSlot.Chest, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(wristItems, Character.CharacterSlot.Wrist, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(handsItems, Character.CharacterSlot.Hands, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(waistItems, Character.CharacterSlot.Waist, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(legsItems, Character.CharacterSlot.Legs, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(feetItems, Character.CharacterSlot.Feet, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(fingerItems, Character.CharacterSlot.Finger1, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(fingerItems, Character.CharacterSlot.Finger2, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(trinketItems, Character.CharacterSlot.Trinket1, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(trinketItems, Character.CharacterSlot.Trinket2, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(mainHandItems, Character.CharacterSlot.MainHand, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(offHandItems, Character.CharacterSlot.OffHand, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(rangedItems, Character.CharacterSlot.Ranged, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(projectileItems, Character.CharacterSlot.Projectile, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectItemUpgrades(projectileBagItems, Character.CharacterSlot.ProjectileBag, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }

                    results = LookForDirectEnchantUpgrades(headEnchants, Character.CharacterSlot.Head, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(backEnchants, Character.CharacterSlot.Back, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(shouldersEnchants, Character.CharacterSlot.Shoulders, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(chestEnchants, Character.CharacterSlot.Chest, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(wristEnchants, Character.CharacterSlot.Wrist, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(handsEnchants, Character.CharacterSlot.Hands, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(legsEnchants, Character.CharacterSlot.Legs, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(feetEnchants, Character.CharacterSlot.Feet, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(fingerEnchants, Character.CharacterSlot.Finger1, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(fingerEnchants, Character.CharacterSlot.Finger2, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(mainHandEnchants, Character.CharacterSlot.MainHand, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(offHandEnchants, Character.CharacterSlot.OffHand, best, bestCharacter, out calculations);
                    if (results.Key > 0) { best = results.Key; bestCalculations = calculations; bestCharacter = results.Value; noImprove = 0; }
                    results = LookForDirectEnchantUpgrades(rangedEnchants, Character.CharacterSlot.Ranged, best, bestCharacter, out calculations);
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

		private KeyValuePair<float, Character> LookForDirectItemUpgrades(Item[] items, Character.CharacterSlot slot, float best, Character bestCharacter, out CharacterCalculationsBase bestCalculations)
		{
			Character charSwap;
            bestCalculations = null;
			float value;
			bool foundUpgrade = false;
            if (slot == lockedSlot) items = lockedItems;
			foreach (Item item in items)
			{
				if (item != null &&
					!(slot == Character.CharacterSlot.Finger1 && bestCharacter.Finger2 != null && bestCharacter.Finger2.Id == item.Id && item.Unique) &&
                    !(slot == Character.CharacterSlot.Finger2 && bestCharacter.Finger1 != null && bestCharacter.Finger1.Id == item.Id && item.Unique) &&
                    !(slot == Character.CharacterSlot.Trinket1 && bestCharacter.Trinket2 != null && bestCharacter.Trinket2.Id == item.Id && item.Unique) &&
                    !(slot == Character.CharacterSlot.Trinket2 && bestCharacter.Trinket1 != null && bestCharacter.Trinket1.Id == item.Id && item.Unique) &&
                    !(slot == Character.CharacterSlot.MainHand && bestCharacter.MainHand != null && bestCharacter.MainHand.Id == item.Id && item.Unique) &&
                    !(slot == Character.CharacterSlot.OffHand && bestCharacter.OffHand != null && bestCharacter.OffHand.Id == item.Id && item.Unique))
				{
					charSwap = BuildSingleItemSwapCharacter(bestCharacter, slot, item);
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

		private KeyValuePair<float, Character> LookForDirectEnchantUpgrades(Enchant[] enchants, Character.CharacterSlot slot, float best, Character bestCharacter, out CharacterCalculationsBase bestCalculations)
		{
			Character charSwap;
            bestCalculations = null;
			float value;
			float newBest = best;
			Character newBestCharacter = null;
			foreach (Enchant enchant in enchants)
			{
				charSwap = BuildSingleEnchantSwapCharacter(bestCharacter, slot, enchant);
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
			Item finger1Item = (lockedSlot == Character.CharacterSlot.Finger1) ? lockedItems[rand.Next(lockedItems.Length)] : fingerItems[rand.Next(fingerItems.Length)];
            Item finger2Item = fingerItems[rand.Next(fingerItems.Length)];
			while (finger1Item != null && finger2Item != null && finger1Item.Id == finger2Item.Id && finger1Item.Unique)
				finger2Item = fingerItems[rand.Next(fingerItems.Length)];

            Item trinket1Item = (lockedSlot == Character.CharacterSlot.Trinket1) ? lockedItems[rand.Next(lockedItems.Length)] : trinketItems[rand.Next(trinketItems.Length)];
            Item trinket2Item = trinketItems[rand.Next(trinketItems.Length)];
            while (trinket1Item != null && trinket2Item != null && trinket1Item.Id == trinket2Item.Id && trinket1Item.Unique)
				trinket2Item = trinketItems[rand.Next(trinketItems.Length)];

            Item mainHandItem = (lockedSlot == Character.CharacterSlot.MainHand) ? lockedItems[rand.Next(lockedItems.Length)] : mainHandItems[rand.Next(mainHandItems.Length)];
            Item offHandItem = (lockedSlot == Character.CharacterSlot.OffHand) ? lockedItems[rand.Next(lockedItems.Length)] : offHandItems[rand.Next(offHandItems.Length)];
			while (mainHandItem != null && offHandItem != null && mainHandItem.Id == offHandItem.Id && mainHandItem.Unique)
			{
                mainHandItem = (lockedSlot == Character.CharacterSlot.MainHand) ? lockedItems[rand.Next(lockedItems.Length)] : mainHandItems[rand.Next(mainHandItems.Length)];
                offHandItem = (lockedSlot == Character.CharacterSlot.OffHand) ? lockedItems[rand.Next(lockedItems.Length)] : offHandItems[rand.Next(offHandItems.Length)];
			}

			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
                        (lockedSlot == Character.CharacterSlot.Head) ? lockedItems[rand.Next(lockedItems.Length)] : headItems[rand.Next(headItems.Length)],
                        (lockedSlot == Character.CharacterSlot.Neck) ? lockedItems[rand.Next(lockedItems.Length)] : neckItems[rand.Next(neckItems.Length)],
                        (lockedSlot == Character.CharacterSlot.Shoulders) ? lockedItems[rand.Next(lockedItems.Length)] : shouldersItems[rand.Next(shouldersItems.Length)],
                        (lockedSlot == Character.CharacterSlot.Back) ? lockedItems[rand.Next(lockedItems.Length)] : backItems[rand.Next(backItems.Length)],
                        (lockedSlot == Character.CharacterSlot.Chest) ? lockedItems[rand.Next(lockedItems.Length)] : chestItems[rand.Next(chestItems.Length)],
                        null, null,
                        (lockedSlot == Character.CharacterSlot.Wrist) ? lockedItems[rand.Next(lockedItems.Length)] : wristItems[rand.Next(wristItems.Length)],
                        (lockedSlot == Character.CharacterSlot.Hands) ? lockedItems[rand.Next(lockedItems.Length)] : handsItems[rand.Next(handsItems.Length)],
                        (lockedSlot == Character.CharacterSlot.Waist) ? lockedItems[rand.Next(lockedItems.Length)] : waistItems[rand.Next(waistItems.Length)],
                        (lockedSlot == Character.CharacterSlot.Legs) ? lockedItems[rand.Next(lockedItems.Length)] : legsItems[rand.Next(legsItems.Length)],
                        (lockedSlot == Character.CharacterSlot.Feet) ? lockedItems[rand.Next(lockedItems.Length)] : feetItems[rand.Next(feetItems.Length)],
                        finger1Item, finger2Item, trinket1Item, trinket2Item, mainHandItem, offHandItem,
                        (lockedSlot == Character.CharacterSlot.Ranged) ? lockedItems[rand.Next(lockedItems.Length)] : rangedItems[rand.Next(rangedItems.Length)],
                        (lockedSlot == Character.CharacterSlot.Projectile) ? lockedItems[rand.Next(lockedItems.Length)] : projectileItems[rand.Next(projectileItems.Length)],
                        (lockedSlot == Character.CharacterSlot.ProjectileBag) ? lockedItems[rand.Next(lockedItems.Length)] : projectileBagItems[rand.Next(projectileBagItems.Length)],
						headEnchants[rand.Next(headEnchants.Length)], shouldersEnchants[rand.Next(shouldersEnchants.Length)],
						backEnchants[rand.Next(backEnchants.Length)], chestEnchants[rand.Next(chestEnchants.Length)],
						wristEnchants[rand.Next(wristEnchants.Length)], handsEnchants[rand.Next(handsEnchants.Length)],
						legsEnchants[rand.Next(legsEnchants.Length)], feetEnchants[rand.Next(feetEnchants.Length)],
						fingerEnchants[rand.Next(fingerEnchants.Length)], fingerEnchants[rand.Next(fingerEnchants.Length)],
						mainHandEnchants[rand.Next(mainHandEnchants.Length)], offHandEnchants[rand.Next(offHandEnchants.Length)],
                        rangedEnchants[rand.Next(rangedEnchants.Length)], _character.ActiveBuffs, false);
			//foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
			//	character.CalculationOptions.Add(kvp.Key, kvp.Value);
            character.CalculationOptions = _character.CalculationOptions;
			character.Class = _character.Class;
			character.Talents = _character.Talents;
            character.EnforceMetagemRequirements = _character.EnforceMetagemRequirements;
			//character.RecalculateSetBonuses();
			return character;
		}

		private Character BuildChildCharacter(Character father, Character mother)
		{
            Item finger1Item = rand.NextDouble() < 0.5d ? father.Finger1 : mother.Finger1;
            Item finger2Item = rand.NextDouble() < 0.5d ? father.Finger2 : mother.Finger2;
            while (finger1Item != null && finger2Item != null && finger1Item.Id == finger2Item.Id && finger1Item.Unique)
			{
				finger1Item = rand.NextDouble() < 0.5d ? father.Finger1 : mother.Finger1;
				finger2Item = rand.NextDouble() < 0.5d ? father.Finger2 : mother.Finger2;
			}

            Item trinket1Item = rand.NextDouble() < 0.5d ? father.Trinket1 : mother.Trinket1;
            Item trinket2Item = rand.NextDouble() < 0.5d ? father.Trinket2 : mother.Trinket2;
            while (trinket1Item != null && trinket2Item != null && trinket1Item.Id == trinket2Item.Id && trinket1Item.Unique)
			{
				trinket1Item = rand.NextDouble() < 0.5d ? father.Trinket1 : mother.Trinket1;
				trinket2Item = rand.NextDouble() < 0.5d ? father.Trinket2 : mother.Trinket2;
			}

            Item mainHandItem = rand.NextDouble() < 0.5d ? father.MainHand : mother.MainHand;
            Item offHandItem = rand.NextDouble() < 0.5d ? father.OffHand : mother.OffHand;
            while (mainHandItem != null && offHandItem != null && mainHandItem.Id == offHandItem.Id && mainHandItem.Unique)
			{
				mainHandItem = rand.NextDouble() < 0.5d ? father.MainHand : mother.MainHand;
				offHandItem = rand.NextDouble() < 0.5d ? father.OffHand : mother.OffHand;
			}

			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
				rand.NextDouble() < 0.5d ? father.Head : mother.Head,
				rand.NextDouble() < 0.5d ? father.Neck : mother.Neck,
				rand.NextDouble() < 0.5d ? father.Shoulders : mother.Shoulders,
				rand.NextDouble() < 0.5d ? father.Back : mother.Back,
				rand.NextDouble() < 0.5d ? father.Chest : mother.Chest,
				null, null,
				rand.NextDouble() < 0.5d ? father.Wrist : mother.Wrist,
				rand.NextDouble() < 0.5d ? father.Hands : mother.Hands,
				rand.NextDouble() < 0.5d ? father.Waist : mother.Waist,
				rand.NextDouble() < 0.5d ? father.Legs : mother.Legs,
				rand.NextDouble() < 0.5d ? father.Feet : mother.Feet,
				finger1Item, finger2Item, trinket1Item, trinket2Item, mainHandItem, offHandItem,
				rand.NextDouble() < 0.5d ? father.Ranged : mother.Ranged,
				rand.NextDouble() < 0.5d ? father.Projectile : mother.Projectile,
				rand.NextDouble() < 0.5d ? father.ProjectileBag : mother.ProjectileBag,
				rand.NextDouble() < 0.5d ? father.HeadEnchant : mother.HeadEnchant,
				rand.NextDouble() < 0.5d ? father.ShouldersEnchant : mother.ShouldersEnchant,
				rand.NextDouble() < 0.5d ? father.BackEnchant : mother.BackEnchant,
				rand.NextDouble() < 0.5d ? father.ChestEnchant : mother.ChestEnchant,
				rand.NextDouble() < 0.5d ? father.WristEnchant : mother.WristEnchant,
				rand.NextDouble() < 0.5d ? father.HandsEnchant : mother.HandsEnchant,
				rand.NextDouble() < 0.5d ? father.LegsEnchant : mother.LegsEnchant,
				rand.NextDouble() < 0.5d ? father.FeetEnchant : mother.FeetEnchant,
				rand.NextDouble() < 0.5d ? father.Finger1Enchant : mother.Finger1Enchant,
				rand.NextDouble() < 0.5d ? father.Finger2Enchant : mother.Finger2Enchant,
				rand.NextDouble() < 0.5d ? father.MainHandEnchant : mother.MainHandEnchant,
				rand.NextDouble() < 0.5d ? father.OffHandEnchant : mother.OffHandEnchant,
				rand.NextDouble() < 0.5d ? father.RangedEnchant : mother.RangedEnchant,
                _character.ActiveBuffs, false);
			//foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
			//	character.CalculationOptions.Add(kvp.Key, kvp.Value);
            character.CalculationOptions = _character.CalculationOptions;
            character.Class = _character.Class;
			character.Talents = _character.Talents;
            character.EnforceMetagemRequirements = _character.EnforceMetagemRequirements;
            //character.RecalculateSetBonuses();
			return character;
		}

        private Item ReplaceGem(Item item, int index, Item gem)
        {
            // alternatively construct gemmedid and retrieve from cache, trading memory footprint for dictionary access
            Item copy = new Item(item.Name, item.Quality, item.Type, item.Id, item.IconPath, item.Slot,
                item.SetName, item.Unique, item.Stats.Clone(), item.Sockets.Clone(), 0, 0, 0, item.MinDamage,
                item.MaxDamage, item.DamageType, item.Speed, item.RequiredClasses);
            copy.SetGemInternal(1, item.Gem1);
            copy.SetGemInternal(2, item.Gem2);
            copy.SetGemInternal(3, item.Gem3);
            copy.SetGemInternal(index, gem);
            return copy;
            /*string gemmedId = string.Format("{0}.{1}.{2}.{3}", item.Id, (index == 1) ? gem.Id : item.Gem1Id, (index == 2) ? gem.Id : item.Gem2Id, (index == 3) ? gem.Id : item.Gem3Id);
            return ItemCache.FindItemById(gemmedId, true, false);*/
        }

        private struct GemInformation
        {
            public Character.CharacterSlot Slot;
            public int Index;
            public Item Gem;
            public Item.ItemSlot Socket;
        }

        private Character BuildReplaceGemMutantCharacter(Character parent)
        {
            Dictionary<Character.CharacterSlot, Item> items = new Dictionary<Character.CharacterSlot, Item>();
            items[Character.CharacterSlot.Head] = parent[Character.CharacterSlot.Head];
            items[Character.CharacterSlot.Neck] = parent[Character.CharacterSlot.Neck];
            items[Character.CharacterSlot.Shoulders] = parent[Character.CharacterSlot.Shoulders];
            items[Character.CharacterSlot.Back] = parent[Character.CharacterSlot.Back];
            items[Character.CharacterSlot.Chest] = parent[Character.CharacterSlot.Chest];
            items[Character.CharacterSlot.Wrist] = parent[Character.CharacterSlot.Wrist];
            items[Character.CharacterSlot.Hands] = parent[Character.CharacterSlot.Hands];
            items[Character.CharacterSlot.Waist] = parent[Character.CharacterSlot.Waist];
            items[Character.CharacterSlot.Legs] = parent[Character.CharacterSlot.Legs];
            items[Character.CharacterSlot.Feet] = parent[Character.CharacterSlot.Feet];
            items[Character.CharacterSlot.Finger1] = parent[Character.CharacterSlot.Finger1];
            items[Character.CharacterSlot.Finger2] = parent[Character.CharacterSlot.Finger2];
            items[Character.CharacterSlot.Trinket1] = parent[Character.CharacterSlot.Trinket1];
            items[Character.CharacterSlot.Trinket2] = parent[Character.CharacterSlot.Trinket2];
            items[Character.CharacterSlot.MainHand] = parent[Character.CharacterSlot.MainHand];
            items[Character.CharacterSlot.OffHand] = parent[Character.CharacterSlot.OffHand];
            items[Character.CharacterSlot.Ranged] = parent[Character.CharacterSlot.Ranged];
            items[Character.CharacterSlot.Projectile] = parent[Character.CharacterSlot.Projectile];
            items[Character.CharacterSlot.ProjectileBag] = parent[Character.CharacterSlot.ProjectileBag];

            // do the work

            // build a list of possible mutation points
            List<GemInformation> locationList = new List<GemInformation>();
            foreach (KeyValuePair<Character.CharacterSlot, Item> pair in items)
            {
                if (pair.Value != null)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        Item gem = pair.Value.GetGem(i);
                        if (gem != null) locationList.Add(new GemInformation() { Slot = pair.Key, Index = i, Gem = gem, Socket = pair.Value.Sockets.GetColor(i) });
                    }
                }
            }

            if (locationList.Count > 0)
            {
                int numberMutations = rand.Next(1, 2);
                for (int i = 0; i < numberMutations; i++)
                {
                    // randomly select mutation point
                    int mutationIndex = rand.Next(locationList.Count);

                    // mutate
                    GemInformation mutation = locationList[mutationIndex];
                    Item newGem;
                    if (mutation.Socket == Item.ItemSlot.Meta)
                    {
                        newGem = metaGemItems[rand.Next(metaGemItems.Length)];
                    }
                    else
                    {
                        newGem = gemItems[rand.Next(gemItems.Length)];
                    }
                    items[mutation.Slot] = ReplaceGem(items[mutation.Slot], mutation.Index, newGem);
                }
            }

            // create character

            Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
                items[Character.CharacterSlot.Head],
                items[Character.CharacterSlot.Neck],
                items[Character.CharacterSlot.Shoulders],
                items[Character.CharacterSlot.Back],
                items[Character.CharacterSlot.Chest],
                null, null,
                items[Character.CharacterSlot.Wrist],
                items[Character.CharacterSlot.Hands],
                items[Character.CharacterSlot.Waist],
                items[Character.CharacterSlot.Legs],
                items[Character.CharacterSlot.Feet],
                items[Character.CharacterSlot.Finger1], items[Character.CharacterSlot.Finger2], items[Character.CharacterSlot.Trinket1], items[Character.CharacterSlot.Trinket2], items[Character.CharacterSlot.MainHand], items[Character.CharacterSlot.OffHand],
                items[Character.CharacterSlot.Ranged],
                items[Character.CharacterSlot.Projectile],
                items[Character.CharacterSlot.ProjectileBag],
                parent.HeadEnchant,
                parent.ShouldersEnchant,
                parent.BackEnchant,
                parent.ChestEnchant,
                parent.WristEnchant,
                parent.HandsEnchant,
                parent.LegsEnchant,
                parent.FeetEnchant,
                parent.Finger1Enchant,
                parent.Finger2Enchant,
                parent.MainHandEnchant,
                parent.OffHandEnchant,
                parent.RangedEnchant,
                _character.ActiveBuffs, false);
            //foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
            //	character.CalculationOptions.Add(kvp.Key, kvp.Value);
            character.CalculationOptions = _character.CalculationOptions;
            character.Class = _character.Class;
            character.Talents = _character.Talents;
            character.EnforceMetagemRequirements = _character.EnforceMetagemRequirements;
            //character.RecalculateSetBonuses();
            return character;
        }

        private Character BuildSwapGemMutantCharacter(Character parent)
        {
            Dictionary<Character.CharacterSlot, Item> items = new Dictionary<Character.CharacterSlot, Item>();
            items[Character.CharacterSlot.Head] = parent[Character.CharacterSlot.Head];
            items[Character.CharacterSlot.Neck] = parent[Character.CharacterSlot.Neck];
            items[Character.CharacterSlot.Shoulders] = parent[Character.CharacterSlot.Shoulders];
            items[Character.CharacterSlot.Back] = parent[Character.CharacterSlot.Back];
            items[Character.CharacterSlot.Chest] = parent[Character.CharacterSlot.Chest];
            items[Character.CharacterSlot.Wrist] = parent[Character.CharacterSlot.Wrist];
            items[Character.CharacterSlot.Hands] = parent[Character.CharacterSlot.Hands];
            items[Character.CharacterSlot.Waist] = parent[Character.CharacterSlot.Waist];
            items[Character.CharacterSlot.Legs] = parent[Character.CharacterSlot.Legs];
            items[Character.CharacterSlot.Feet] = parent[Character.CharacterSlot.Feet];
            items[Character.CharacterSlot.Finger1] = parent[Character.CharacterSlot.Finger1];
            items[Character.CharacterSlot.Finger2] = parent[Character.CharacterSlot.Finger2];
            items[Character.CharacterSlot.Trinket1] = parent[Character.CharacterSlot.Trinket1];
            items[Character.CharacterSlot.Trinket2] = parent[Character.CharacterSlot.Trinket2];
            items[Character.CharacterSlot.MainHand] = parent[Character.CharacterSlot.MainHand];
            items[Character.CharacterSlot.OffHand] = parent[Character.CharacterSlot.OffHand];
            items[Character.CharacterSlot.Ranged] = parent[Character.CharacterSlot.Ranged];
            items[Character.CharacterSlot.Projectile] = parent[Character.CharacterSlot.Projectile];
            items[Character.CharacterSlot.ProjectileBag] = parent[Character.CharacterSlot.ProjectileBag];

            // do the work

            // build a list of possible mutation points
            // make sure not to do meta gem swaps
            List<GemInformation> locationList = new List<GemInformation>();
            foreach (KeyValuePair<Character.CharacterSlot, Item> pair in items)
            {
                if (pair.Value != null)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        Item gem = pair.Value.GetGem(i);
                        if (gem != null && gem.Slot != Item.ItemSlot.Meta) locationList.Add(new GemInformation() { Slot = pair.Key, Index = i, Gem = gem, Socket = pair.Value.Sockets.GetColor(i) });
                    }
                }
            }

            if (locationList.Count > 1)
            {
                GemInformation mutation1;
                GemInformation mutation2;
                int tries = 0;
                // randomly select mutation point
                do
                {
                    int mutationIndex1 = rand.Next(locationList.Count);
                    int mutationIndex2 = rand.Next(locationList.Count);
                    mutation1 = locationList[mutationIndex1];
                    mutation2 = locationList[mutationIndex2];
                    tries++;
                } while (tries < 10 && mutation1.Gem == mutation2.Gem);

                // mutate
                items[mutation1.Slot] = ReplaceGem(items[mutation1.Slot], mutation1.Index, mutation2.Gem);
                items[mutation2.Slot] = ReplaceGem(items[mutation2.Slot], mutation2.Index, mutation1.Gem);
            }

            // create character

            Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
                items[Character.CharacterSlot.Head],
                items[Character.CharacterSlot.Neck],
                items[Character.CharacterSlot.Shoulders],
                items[Character.CharacterSlot.Back],
                items[Character.CharacterSlot.Chest],
                null, null,
                items[Character.CharacterSlot.Wrist],
                items[Character.CharacterSlot.Hands],
                items[Character.CharacterSlot.Waist],
                items[Character.CharacterSlot.Legs],
                items[Character.CharacterSlot.Feet],
                items[Character.CharacterSlot.Finger1], items[Character.CharacterSlot.Finger2], items[Character.CharacterSlot.Trinket1], items[Character.CharacterSlot.Trinket2], items[Character.CharacterSlot.MainHand], items[Character.CharacterSlot.OffHand],
                items[Character.CharacterSlot.Ranged],
                items[Character.CharacterSlot.Projectile],
                items[Character.CharacterSlot.ProjectileBag],
                parent.HeadEnchant,
                parent.ShouldersEnchant,
                parent.BackEnchant,
                parent.ChestEnchant,
                parent.WristEnchant,
                parent.HandsEnchant,
                parent.LegsEnchant,
                parent.FeetEnchant,
                parent.Finger1Enchant,
                parent.Finger2Enchant,
                parent.MainHandEnchant,
                parent.OffHandEnchant,
                parent.RangedEnchant,
                _character.ActiveBuffs, false);
            //foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
            //	character.CalculationOptions.Add(kvp.Key, kvp.Value);
            character.CalculationOptions = _character.CalculationOptions;
            character.Class = _character.Class;
            character.Talents = _character.Talents;
            character.EnforceMetagemRequirements = _character.EnforceMetagemRequirements;
            //character.RecalculateSetBonuses();
            return character;
        }

		private Character BuildMutantCharacter(Character parent)
		{
			int targetMutations = 2;
			while (targetMutations < 32 && rand.NextDouble() < 0.75d) targetMutations++;
			double mutationChance = (double)targetMutations / 32d;

            Item finger1Item = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Finger1) ? lockedItems[rand.Next(lockedItems.Length)] : fingerItems[rand.Next(fingerItems.Length)]) : parent.Finger1;
            Item finger2Item = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Finger2) ? lockedItems[rand.Next(lockedItems.Length)] : fingerItems[rand.Next(fingerItems.Length)]) : parent.Finger2;
            while (finger1Item != null && finger2Item != null && finger1Item.Id == finger2Item.Id && finger1Item.Unique)
			{
                finger1Item = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Finger1) ? lockedItems[rand.Next(lockedItems.Length)] : fingerItems[rand.Next(fingerItems.Length)]) : parent.Finger1;
                finger2Item = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Finger2) ? lockedItems[rand.Next(lockedItems.Length)] : fingerItems[rand.Next(fingerItems.Length)]) : parent.Finger2;
			}

            Item trinket1Item = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Trinket1) ? lockedItems[rand.Next(lockedItems.Length)] : trinketItems[rand.Next(trinketItems.Length)]) : parent.Trinket1;
            Item trinket2Item = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Trinket2) ? lockedItems[rand.Next(lockedItems.Length)] : trinketItems[rand.Next(trinketItems.Length)]) : parent.Trinket2;
            while (trinket1Item != null && trinket2Item != null && trinket1Item.Id == trinket2Item.Id && trinket1Item.Unique)
			{
                trinket1Item = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Trinket1) ? lockedItems[rand.Next(lockedItems.Length)] : trinketItems[rand.Next(trinketItems.Length)]) : parent.Trinket1;
                trinket2Item = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Trinket2) ? lockedItems[rand.Next(lockedItems.Length)] : trinketItems[rand.Next(trinketItems.Length)]) : parent.Trinket2;
			}

            Item mainHandItem = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.MainHand) ? lockedItems[rand.Next(lockedItems.Length)] : mainHandItems[rand.Next(mainHandItems.Length)]) : parent.MainHand;
            Item offHandItem = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.OffHand) ? lockedItems[rand.Next(lockedItems.Length)] : offHandItems[rand.Next(offHandItems.Length)]) : parent.OffHand;
            while (mainHandItem != null && offHandItem != null && mainHandItem.Id == offHandItem.Id && mainHandItem.Unique)
			{
                mainHandItem = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.MainHand) ? lockedItems[rand.Next(lockedItems.Length)] : mainHandItems[rand.Next(mainHandItems.Length)]) : parent.MainHand;
                offHandItem = rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.OffHand) ? lockedItems[rand.Next(lockedItems.Length)] : offHandItems[rand.Next(offHandItems.Length)]) : parent.OffHand;
			}

			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Head) ? lockedItems[rand.Next(lockedItems.Length)] : headItems[rand.Next(headItems.Length)]) : parent.Head,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Neck) ? lockedItems[rand.Next(lockedItems.Length)] : neckItems[rand.Next(neckItems.Length)]) : parent.Neck,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Shoulders) ? lockedItems[rand.Next(lockedItems.Length)] : shouldersItems[rand.Next(shouldersItems.Length)]) : parent.Shoulders,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Back) ? lockedItems[rand.Next(lockedItems.Length)] : backItems[rand.Next(backItems.Length)]) : parent.Back,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Chest) ? lockedItems[rand.Next(lockedItems.Length)] : chestItems[rand.Next(chestItems.Length)]) : parent.Chest, 
				null, null,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Wrist) ? lockedItems[rand.Next(lockedItems.Length)] : wristItems[rand.Next(wristItems.Length)]) : parent.Wrist,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Hands) ? lockedItems[rand.Next(lockedItems.Length)] : handsItems[rand.Next(handsItems.Length)]) : parent.Hands,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Waist) ? lockedItems[rand.Next(lockedItems.Length)] : waistItems[rand.Next(waistItems.Length)]) : parent.Waist,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Legs) ? lockedItems[rand.Next(lockedItems.Length)] : legsItems[rand.Next(legsItems.Length)]) : parent.Legs,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Feet) ? lockedItems[rand.Next(lockedItems.Length)] : feetItems[rand.Next(feetItems.Length)]) : parent.Feet,
				finger1Item, finger2Item, trinket1Item, trinket2Item, mainHandItem, offHandItem,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Ranged) ? lockedItems[rand.Next(lockedItems.Length)] : rangedItems[rand.Next(rangedItems.Length)]) : parent.Ranged,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.Projectile) ? lockedItems[rand.Next(lockedItems.Length)] : projectileItems[rand.Next(projectileItems.Length)]) : parent.Projectile,
                rand.NextDouble() < mutationChance ? ((lockedSlot == Character.CharacterSlot.ProjectileBag) ? lockedItems[rand.Next(lockedItems.Length)] : projectileBagItems[rand.Next(projectileBagItems.Length)]) : parent.ProjectileBag,
				rand.NextDouble() < mutationChance ? headEnchants[rand.Next(headEnchants.Length)] : parent.HeadEnchant, 
				rand.NextDouble() < mutationChance ? shouldersEnchants[rand.Next(shouldersEnchants.Length)] : parent.ShouldersEnchant,
				rand.NextDouble() < mutationChance ? backEnchants[rand.Next(backEnchants.Length)] : parent.BackEnchant,
				rand.NextDouble() < mutationChance ? chestEnchants[rand.Next(chestEnchants.Length)] : parent.ChestEnchant,
				rand.NextDouble() < mutationChance ? wristEnchants[rand.Next(wristEnchants.Length)] : parent.WristEnchant,
				rand.NextDouble() < mutationChance ? handsEnchants[rand.Next(handsEnchants.Length)] : parent.HandsEnchant,
				rand.NextDouble() < mutationChance ? legsEnchants[rand.Next(legsEnchants.Length)] : parent.LegsEnchant,
				rand.NextDouble() < mutationChance ? feetEnchants[rand.Next(feetEnchants.Length)] : parent.FeetEnchant,
				rand.NextDouble() < mutationChance ? fingerEnchants[rand.Next(fingerEnchants.Length)] : parent.Finger1Enchant,
				rand.NextDouble() < mutationChance ? fingerEnchants[rand.Next(fingerEnchants.Length)] : parent.Finger2Enchant,
				rand.NextDouble() < mutationChance ? mainHandEnchants[rand.Next(mainHandEnchants.Length)] : parent.MainHandEnchant,
				rand.NextDouble() < mutationChance ? offHandEnchants[rand.Next(offHandEnchants.Length)] : parent.OffHandEnchant,
				rand.NextDouble() < mutationChance ? rangedEnchants[rand.Next(rangedEnchants.Length)] : parent.RangedEnchant,
                _character.ActiveBuffs, false);
			//foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
			//	character.CalculationOptions.Add(kvp.Key, kvp.Value);
            character.CalculationOptions = _character.CalculationOptions;
            character.Class = _character.Class;
			character.Talents = _character.Talents;
            character.EnforceMetagemRequirements = _character.EnforceMetagemRequirements;
            //character.RecalculateSetBonuses();
			return character;

		}

		private Character BuildSingleItemSwapCharacter(Character baseCharacter, Character.CharacterSlot slot, Item item)
		{
			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
				slot == Character.CharacterSlot.Head ? item : baseCharacter.Head,
				slot == Character.CharacterSlot.Neck ? item : baseCharacter.Neck,
				slot == Character.CharacterSlot.Shoulders ? item : baseCharacter.Shoulders,
				slot == Character.CharacterSlot.Back ? item : baseCharacter.Back,
				slot == Character.CharacterSlot.Chest ? item : baseCharacter.Chest,
				null, null,
				slot == Character.CharacterSlot.Wrist ? item : baseCharacter.Wrist,
				slot == Character.CharacterSlot.Hands ? item : baseCharacter.Hands,
				slot == Character.CharacterSlot.Waist ? item : baseCharacter.Waist,
				slot == Character.CharacterSlot.Legs ? item : baseCharacter.Legs,
				slot == Character.CharacterSlot.Feet ? item : baseCharacter.Feet,
				slot == Character.CharacterSlot.Finger1 ? item : baseCharacter.Finger1,
				slot == Character.CharacterSlot.Finger2 ? item : baseCharacter.Finger2,
				slot == Character.CharacterSlot.Trinket1 ? item : baseCharacter.Trinket1,
				slot == Character.CharacterSlot.Trinket2 ? item : baseCharacter.Trinket2,
				slot == Character.CharacterSlot.MainHand ? item : baseCharacter.MainHand,
				slot == Character.CharacterSlot.OffHand ? item : baseCharacter.OffHand,
				slot == Character.CharacterSlot.Ranged ? item : baseCharacter.Ranged,
				slot == Character.CharacterSlot.Projectile ? item : baseCharacter.Projectile,
				slot == Character.CharacterSlot.ProjectileBag ? item : baseCharacter.ProjectileBag,
				baseCharacter.HeadEnchant,
				baseCharacter.ShouldersEnchant,
				baseCharacter.BackEnchant,
				baseCharacter.ChestEnchant,
				baseCharacter.WristEnchant,
				baseCharacter.HandsEnchant,
				baseCharacter.LegsEnchant,
				baseCharacter.FeetEnchant,
				baseCharacter.Finger1Enchant,
				baseCharacter.Finger2Enchant,
				baseCharacter.MainHandEnchant,
				baseCharacter.OffHandEnchant,
				baseCharacter.RangedEnchant,
                _character.ActiveBuffs, false);
			//foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
			//	character.CalculationOptions.Add(kvp.Key, kvp.Value);
            character.CalculationOptions = _character.CalculationOptions;
            character.Class = _character.Class;
			character.Talents = _character.Talents;
            character.EnforceMetagemRequirements = _character.EnforceMetagemRequirements;
            //character.RecalculateSetBonuses();
			return character;
		}

		private Character BuildSingleEnchantSwapCharacter(Character baseCharacter, Character.CharacterSlot slot, Enchant enchant)
		{
			Character character = new Character(_character.Name, _character.Realm, _character.Region, _character.Race,
				baseCharacter.Head,
				baseCharacter.Neck,
				baseCharacter.Shoulders,
				baseCharacter.Back,
				baseCharacter.Chest,
				null, null,
				baseCharacter.Wrist,
				baseCharacter.Hands,
				baseCharacter.Waist,
				baseCharacter.Legs,
				baseCharacter.Feet,
				baseCharacter.Finger1,
				baseCharacter.Finger2,
				baseCharacter.Trinket1,
				baseCharacter.Trinket2,
				baseCharacter.MainHand,
				baseCharacter.OffHand,
				baseCharacter.Ranged,
				baseCharacter.Projectile,
				baseCharacter.ProjectileBag,
				slot == Character.CharacterSlot.Head ? enchant : baseCharacter.HeadEnchant,
				slot == Character.CharacterSlot.Shoulders ? enchant : baseCharacter.ShouldersEnchant,
				slot == Character.CharacterSlot.Back ? enchant : baseCharacter.BackEnchant,
				slot == Character.CharacterSlot.Chest ? enchant : baseCharacter.ChestEnchant,
				slot == Character.CharacterSlot.Wrist ? enchant : baseCharacter.WristEnchant,
				slot == Character.CharacterSlot.Hands ? enchant : baseCharacter.HandsEnchant,
				slot == Character.CharacterSlot.Legs ? enchant : baseCharacter.LegsEnchant,
				slot == Character.CharacterSlot.Feet ? enchant : baseCharacter.FeetEnchant,
				slot == Character.CharacterSlot.Finger1 ? enchant : baseCharacter.Finger1Enchant,
				slot == Character.CharacterSlot.Finger2 ? enchant : baseCharacter.Finger2Enchant,
				slot == Character.CharacterSlot.MainHand ? enchant : baseCharacter.MainHandEnchant,
				slot == Character.CharacterSlot.OffHand ? enchant : baseCharacter.OffHandEnchant,
				slot == Character.CharacterSlot.Ranged ? enchant : baseCharacter.RangedEnchant,
                _character.ActiveBuffs, false);
			//foreach (KeyValuePair<string, string> kvp in _character.CalculationOptions)
			//	character.CalculationOptions.Add(kvp.Key, kvp.Value);
            character.CalculationOptions = _character.CalculationOptions;
            character.Class = _character.Class;
			character.Talents = _character.Talents;
            character.EnforceMetagemRequirements = _character.EnforceMetagemRequirements;
            //character.RecalculateSetBonuses();
			return character;
		}

        private Item[] GetPossibleGemmedItemsForItem(Item item, Item[] gemItems, Item[] metaGemItems)
		{
			List<Item> possibleGemmedItems = new List<Item>();
			if (!_allGemmings)
			{
				foreach (Item knownItem in ItemCache.RelevantItems)
					if (knownItem.Id == item.Id)
						possibleGemmedItems.Add(knownItem);
			}
			else
			{
                Item[] possibleGem1s, possibleGem2s, possibleGem3s = null;
			    switch (item.Sockets.Color1)
			    {
				    case Item.ItemSlot.Meta:
					    possibleGem1s = metaGemItems;
					    break;
				    case Item.ItemSlot.Red:
				    case Item.ItemSlot.Orange:
				    case Item.ItemSlot.Yellow:
				    case Item.ItemSlot.Green:
				    case Item.ItemSlot.Blue:
				    case Item.ItemSlot.Purple:
				    case Item.ItemSlot.Prismatic:
					    possibleGem1s = gemItems;
					    break;
				    default:
                        possibleGem1s = new Item[] { null };
					    break;
			    }
			    switch (item.Sockets.Color2)
			    {
				    case Item.ItemSlot.Meta:
					    possibleGem2s = metaGemItems;
					    break;
				    case Item.ItemSlot.Red:
				    case Item.ItemSlot.Orange:
				    case Item.ItemSlot.Yellow:
				    case Item.ItemSlot.Green:
				    case Item.ItemSlot.Blue:
				    case Item.ItemSlot.Purple:
				    case Item.ItemSlot.Prismatic:
					    possibleGem2s = gemItems;
					    break;
				    default:
                        possibleGem2s = new Item[] { null };
					    break;
			    }
			    switch (item.Sockets.Color3)
			    {
				    case Item.ItemSlot.Meta:
					    possibleGem3s = metaGemItems;
					    break;
				    case Item.ItemSlot.Red:
				    case Item.ItemSlot.Orange:
				    case Item.ItemSlot.Yellow:
				    case Item.ItemSlot.Green:
				    case Item.ItemSlot.Blue:
				    case Item.ItemSlot.Purple:
				    case Item.ItemSlot.Prismatic:
					    possibleGem3s = gemItems;
					    break;
				    default:
                        possibleGem3s = new Item[] { null };
					    break;
			    }

                foreach (Item gem1 in possibleGem1s)
                    foreach (Item gem2 in possibleGem2s)
                        foreach (Item gem3 in possibleGem3s)
                        {
                            //possibleGemmedItems.Add(ItemCache.Instance.FindItemById(string.Format("{0}.{1}.{2}.{3}", id0, id1, id2, id3), true, false, true));
                            // skip item cache, since we're creating new gemmings most likely they don't exist
                            // it will search through all item to find the item we already have and then clone it
                            // so skip all do that and do cloning ourselves
                            Item copy = new Item(item.Name, item.Quality, item.Type, item.Id, item.IconPath, item.Slot,
                                item.SetName, item.Unique, item.Stats.Clone(), item.Sockets.Clone(), 0, 0, 0, item.MinDamage,
                                item.MaxDamage, item.DamageType, item.Speed, item.RequiredClasses);
                            copy.SetGemInternal(1, gem1);
                            copy.SetGemInternal(2, gem2);
                            copy.SetGemInternal(3, gem3);
                            possibleGemmedItems.Add(copy);
                            //ItemCache.AddItem(copy, true, false);
                        }
			}

			return possibleGemmedItems.ToArray();
		}

		private Item[] FilterList(List<Item> unfilteredList)
		{
			List<Item> filteredList = new List<Item>();
			List<StatsColors> filteredStatsColors = new List<StatsColors>();
			foreach (Item gemmedItem in unfilteredList)
			{
				if (gemmedItem == null)
				{
					filteredList.Add(gemmedItem);
					continue;
				}
				int meta = 0, red = 0, yellow = 0, blue = 0;
                foreach (Item gem in new Item[] { gemmedItem.Gem1, gemmedItem.Gem2, gemmedItem.Gem3 })
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
					GemmedItem = gemmedItem,
                    SetName = gemmedItem.SetName,
                    Stats = gemmedItem.GetTotalStats(),
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
				filteredList.Add(statsColors.GemmedItem);
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
			public Item GemmedItem;
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
                return Stats.GetHashCode()^GemmedItem.GetHashCode();
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

        private void FormOptimize_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Optimizer.Default.AllGemmings = radioButtonAllGemmings.Checked;
            Properties.Optimizer.Default.Thoroughness = trackBarThoroughness.Value;
            Properties.Optimizer.Default.CalculationToOptimize = GetCalculationStringFromComboBox(comboBoxCalculationToOptimize);
            Properties.Optimizer.Default.Save();
        }
	}
}
