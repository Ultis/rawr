using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Configuration;

namespace Rawr
{
	public partial class FormMain : Form
	{
		private FormSplash _spash = new FormSplash();
		private string _characterPath = "";
		private bool _unsavedChanges = false;
		private CharacterCalculationsBase _calculatedStats = null;

		private bool _loadingCharacter = false;
		private Character _character = null;
		public Character Character
		{
			get
			{
				if (_character == null)
				{
					Character = new Character();
					_characterPath = string.Empty;
					_unsavedChanges = false;
				}
				return _character;
			}
			set
			{
				if (_character != null)
				{
					_character.ItemsChanged -= new EventHandler(_character_ItemsChanged);
				}
				_character = value; 
				if (_character != null)
				{
					this.Cursor = Cursors.WaitCursor;
					Calculations.CalculationOptionsPanel.Character = _character;
					ItemToolTip.Instance.Character = FormItemSelection.Instance.Character = 
						ItemContextualMenu.Instance.Character = buffSelector1.Character = itemComparison1.Character = 
						itemButtonBack.Character = itemButtonChest.Character = itemButtonFeet.Character =
						itemButtonFinger1.Character = itemButtonFinger2.Character = itemButtonHands.Character =
						itemButtonHead.Character = itemButtonRanged.Character = itemButtonLegs.Character =
						itemButtonNeck.Character = itemButtonShirt.Character = itemButtonShoulders.Character =
						itemButtonTabard.Character = itemButtonTrinket1.Character = itemButtonTrinket2.Character =
						itemButtonWaist.Character = itemButtonMainHand.Character = itemButtonOffHand.Character =
						itemButtonProjectile.Character = itemButtonProjectileBag.Character = 
						itemButtonWrist.Character = _character;
					//Ahhh ahhh ahhh ahhh ahhh ahhh ahhh ahhh...

					_character.ItemsChanged += new EventHandler(_character_ItemsChanged);
					_loadingCharacter = true;

					comboBoxEnchantBack.SelectedItem = Character.BackEnchant;
					comboBoxEnchantChest.SelectedItem = Character.ChestEnchant;
					comboBoxEnchantFeet.SelectedItem = Character.FeetEnchant;
					comboBoxEnchantFinger1.SelectedItem = Character.Finger1Enchant;
					comboBoxEnchantFinger2.SelectedItem = Character.Finger2Enchant;
					comboBoxEnchantHands.SelectedItem = Character.HandsEnchant;
					comboBoxEnchantHead.SelectedItem = Character.HeadEnchant;
					comboBoxEnchantLegs.SelectedItem = Character.LegsEnchant;
					comboBoxEnchantShoulders.SelectedItem = Character.ShouldersEnchant;
					comboBoxEnchantMainHand.SelectedItem = Character.MainHandEnchant;
					comboBoxEnchantOffHand.SelectedItem = Character.OffHandEnchant;
					comboBoxEnchantRanged.SelectedItem = Character.RangedEnchant;
					comboBoxEnchantWrists.SelectedItem = Character.WristEnchant;

					textBoxName.Text = Character.Name;
					textBoxRealm.Text = Character.Realm;
					radioButtonRegionUS.Checked = Character.Region == Character.CharacterRegion.US;
					radioButtonRegionEU.Checked = Character.Region == Character.CharacterRegion.EU;
					comboBoxRace.Text = Character.Race.ToString();
					
					_loadingCharacter = false;
					Character.OnItemsChanged();
				}
			}
		}

		void _character_ItemsChanged(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			_unsavedChanges = true;

			itemButtonOffHand.Enabled = _character.MainHand == null || _character.MainHand.Slot != Item.ItemSlot.TwoHand;
			if (!_loadingCharacter)
			{
				itemButtonBack.UpdateSelectedItem(); itemButtonChest.UpdateSelectedItem(); itemButtonFeet.UpdateSelectedItem();
				itemButtonFinger1.UpdateSelectedItem(); itemButtonFinger2.UpdateSelectedItem(); itemButtonHands.UpdateSelectedItem();
				itemButtonHead.UpdateSelectedItem(); itemButtonRanged.UpdateSelectedItem(); itemButtonLegs.UpdateSelectedItem();
				itemButtonNeck.UpdateSelectedItem(); itemButtonShirt.UpdateSelectedItem(); itemButtonShoulders.UpdateSelectedItem();
				itemButtonTabard.UpdateSelectedItem(); itemButtonTrinket1.UpdateSelectedItem(); itemButtonTrinket2.UpdateSelectedItem();
				itemButtonWaist.UpdateSelectedItem(); itemButtonMainHand.UpdateSelectedItem(); itemButtonOffHand.UpdateSelectedItem();
				itemButtonProjectile.UpdateSelectedItem(); itemButtonProjectileBag.UpdateSelectedItem(); itemButtonWrist.UpdateSelectedItem();
			}
			//and the clouds above move closer / looking so dissatisfied
			Calculations.ClearCache();
			CharacterCalculationsBase calcs = Calculations.GetCharacterCalculations(Character);
			_calculatedStats = calcs;

			LoadComparisonData();

			calculationDisplay1.SetCalculations(calcs);

			this.Cursor = Cursors.Default;
			//and the ground below grew colder / as they put you down inside
		}

		private Configuration _config = null;
		public Configuration Config
		{
			get
			{
				if (_config == null)
				{
					_config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
					if (_config.AppSettings.Settings["Model"] == null)
						_config.AppSettings.Settings.Add("Model", "Bear");
					if (_config.AppSettings.Settings["RecentCharacters"] == null)
						_config.AppSettings.Settings.Add("RecentCharacters", "");
				}
				return _config;
			}
		}

		public string ConfigModel
		{
			get { return Config.AppSettings.Settings["Model"].Value; }
			set { Config.AppSettings.Settings["Model"].Value = value; }
		}

		public string[] ConfigRecentCharacters
		{
			get
			{
				string recentCharacters = Config.AppSettings.Settings["RecentCharacters"].Value;
				if (string.IsNullOrEmpty(recentCharacters))
					return new string[0];
				else
					return recentCharacters.Split(';'); 
			}
			set { Config.AppSettings.Settings["RecentCharacters"].Value = string.Join(";", value); }
		}

		public void AddRecentCharacter(string character)
		{
			List<string> recentCharacters = new List<string>(ConfigRecentCharacters);
			recentCharacters.Remove(character);
			recentCharacters.Add(character);
			while (recentCharacters.Count > 8)
				recentCharacters.RemoveRange(0, recentCharacters.Count - 8);
			ConfigRecentCharacters = recentCharacters.ToArray();
			UpdateRecentCharacterMenuItems();
		}

		private List<ToolStripMenuItem> _recentCharacterMenuItems = new List<ToolStripMenuItem>();
		public void UpdateRecentCharacterMenuItems()
		{
			foreach (ToolStripMenuItem item in _recentCharacterMenuItems)
			{
				fileToolStripMenuItem.DropDownItems.Remove(item);
				item.Dispose();
			}
			_recentCharacterMenuItems.Clear();
			foreach (string recentCharacter in ConfigRecentCharacters)
			{
				string fileName = System.IO.Path.GetFileName(recentCharacter);
				ToolStripMenuItem recentCharacterMenuItem = new ToolStripMenuItem(fileName);
				recentCharacterMenuItem.Tag = recentCharacter;
				recentCharacterMenuItem.Click += new EventHandler(recentCharacterMenuItem_Click);
				_recentCharacterMenuItems.Add(recentCharacterMenuItem);
				fileToolStripMenuItem.DropDownItems.Insert(4, recentCharacterMenuItem);
			}
		}

		private List<ToolStripMenuItem> _customChartMenuItems = new List<ToolStripMenuItem>();
		public void UpdateCustomChartMenuItems()
		{
			foreach (ToolStripMenuItem item in _customChartMenuItems)
			{
				toolStripDropDownButtonSlot.DropDownItems.Remove(item);
				item.Dispose();
			}
			_customChartMenuItems.Clear();
			foreach (string chartName in Calculations.CustomChartNames)
			{
				ToolStripMenuItem customChartMenuItem = new ToolStripMenuItem(chartName);
				customChartMenuItem.Tag = "Custom." + chartName;
				customChartMenuItem.Click += new EventHandler(slotToolStripMenuItem_Click);
				_customChartMenuItems.Add(customChartMenuItem);
				toolStripDropDownButtonSlot.DropDownItems.Add(customChartMenuItem);
			}
		}

		void recentCharacterMenuItem_Click(object sender, EventArgs e)
		{
			if (PromptToSaveBeforeClosing())
			{
				LoadCharacter((sender as ToolStripMenuItem).Tag.ToString());
			}
		}

		public FormMain()
		{
			_spash.Show();

			Calculations.LoadModel(Config.AppSettings.Settings["Model"].Value);
			Application.DoEvents();
			InitializeComponent();
			UpdateRecentCharacterMenuItems();

			ToolStripMenuItem modelsToolStripMenuItem = new ToolStripMenuItem("Models");
			menuStripMain.Items.Add(modelsToolStripMenuItem);
			foreach (KeyValuePair<string, Type> kvp in Calculations.Models)
			{
				ToolStripMenuItem modelToolStripMenuItem = new ToolStripMenuItem(kvp.Key);
				modelToolStripMenuItem.Click += new EventHandler(modelToolStripMenuItem_Click);
				modelToolStripMenuItem.Checked = kvp.Value == Calculations.Instance.GetType();
				modelToolStripMenuItem.Tag = kvp.Value;
				modelsToolStripMenuItem.DropDownItems.Add(modelToolStripMenuItem);					
			}

			this.Shown += new EventHandler(FormMain_Shown);
			ItemCache.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
			Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
			Calculations_ModelChanged(null, null);
			
			sortToolStripMenuItem_Click(overallToolStripMenuItem, EventArgs.Empty);
			slotToolStripMenuItem_Click(headToolStripMenuItem, EventArgs.Empty);
		}

		private void modelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem modelToolStripMenuItem = sender as ToolStripMenuItem;
			if (!modelToolStripMenuItem.Checked)
			{
				foreach (ToolStripMenuItem item in _customChartMenuItems)
					if (item.Checked)
						slotToolStripMenuItem_Click(toolStripDropDownButtonSlot.DropDownItems[1], null);

				foreach (ToolStripMenuItem item in (modelToolStripMenuItem.OwnerItem as ToolStripMenuItem).DropDownItems)
					item.Checked = item == modelToolStripMenuItem;
				Calculations.LoadModel(modelToolStripMenuItem.Tag as Type);
				ConfigModel = modelToolStripMenuItem.Text;
			}
		}

		private void Calculations_ModelChanged(object sender, EventArgs e)
		{
			bool unsavedChanges = _unsavedChanges;
			if (Calculations.CalculationOptionsPanel.Icon != null)
			{
				Icon = Calculations.CalculationOptionsPanel.Icon;
			}

			UpdateCustomChartMenuItems();
			toolStripDropDownButtonSort.DropDownItems.Clear();
			toolStripDropDownButtonSort.DropDownItems.Add(overallToolStripMenuItem);
			toolStripDropDownButtonSort.DropDownItems.Add(alphabeticalToolStripMenuItem);
			foreach (string name in Calculations.SubPointNameColors.Keys)
			{
				ToolStripMenuItem toolStripMenuItemSubPoint = new ToolStripMenuItem(name);
				toolStripMenuItemSubPoint.Tag = toolStripDropDownButtonSort.DropDownItems.Count - 2;
				toolStripMenuItemSubPoint.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
				toolStripDropDownButtonSort.DropDownItems.Add(toolStripMenuItemSubPoint);
			}

			comboBoxEnchantBack.Items.Clear();
			comboBoxEnchantChest.Items.Clear();
			comboBoxEnchantFeet.Items.Clear();
			comboBoxEnchantFinger1.Items.Clear();
			comboBoxEnchantFinger2.Items.Clear();
			comboBoxEnchantHands.Items.Clear();
			comboBoxEnchantHead.Items.Clear();
			comboBoxEnchantLegs.Items.Clear();
			comboBoxEnchantShoulders.Items.Clear();
			comboBoxEnchantMainHand.Items.Clear();
			comboBoxEnchantOffHand.Items.Clear();
			comboBoxEnchantRanged.Items.Clear();
			comboBoxEnchantWrists.Items.Clear();
			comboBoxEnchantBack.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Back).ToArray());
			comboBoxEnchantChest.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Chest).ToArray());
			comboBoxEnchantFeet.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Feet).ToArray());
			comboBoxEnchantFinger1.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Finger).ToArray());
			comboBoxEnchantFinger2.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Finger).ToArray());
			comboBoxEnchantHands.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Hands).ToArray());
			comboBoxEnchantHead.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Head).ToArray());
			comboBoxEnchantLegs.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Legs).ToArray());
			comboBoxEnchantShoulders.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Shoulders).ToArray());
			comboBoxEnchantMainHand.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.MainHand).ToArray());
			comboBoxEnchantOffHand.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.MainHand).ToArray());
			comboBoxEnchantRanged.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Ranged).ToArray());
			comboBoxEnchantWrists.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Wrist).ToArray());

			Calculations.CalculationOptionsPanel.Dock = DockStyle.Fill;
			tabPageOptions.Controls.Clear();
			tabPageOptions.Controls.Add(Calculations.CalculationOptionsPanel);
			Character = Character;

			ItemCache.OnItemsChanged();
			Character.OnItemsChanged();
			_unsavedChanges = unsavedChanges;
		}

		void FormMain_Shown(object sender, EventArgs e)
		{
			_spash.Close();
			_spash.Dispose();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			Character.ToString(); //Load the saved character
		}

		void ItemCache_ItemsChanged(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			Item[] items = ItemCache.RelevantItems;
			ItemIcons.CacheAllIcons(items);
			itemComparison1.Items = items;
			LoadComparisonData();
			FormItemSelection.Instance.Items = items;
			this.Cursor = Cursors.Default;
		}

		private void editItemsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormItemEditor itemEditor = new FormItemEditor(Character);
			itemEditor.ShowDialog();
			ItemCache.OnItemsChanged();
		}

		private void editGemsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//FormGemEditor gemEditor = new FormGemEditor();
			//gemEditor.ShowDialog();
			ItemCache.OnItemsChanged();
		}

		#region File Commands
		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (PromptToSaveBeforeClosing())
			{
				LoadCharacter(new Character(), string.Empty);
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (PromptToSaveBeforeClosing())
			{
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.DefaultExt = ".xml";
				dialog.Filter = "Rawr Xml Character Files | *.xml";
				dialog.Multiselect = false;
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					LoadCharacter(dialog.FileName);
				}
			}
		}

		private void LoadCharacter(string characterPath) { LoadCharacter(Character.Load(characterPath), characterPath); }
		private void LoadCharacter(Character character, string characterPath)
		{
			this.Cursor = Cursors.WaitCursor;
			Character = character;
			_characterPath = characterPath;
			_unsavedChanges = false;
			if (!string.IsNullOrEmpty(characterPath))
				AddRecentCharacter(characterPath);
			this.Cursor = Cursors.Default;
		}

		private void loadFromArmoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (PromptToSaveBeforeClosing())
			{
				FormEnterNameRealm form = new FormEnterNameRealm();
				if (form.ShowDialog() == DialogResult.OK)
				{
					this.Cursor = Cursors.WaitCursor;

					if (form.ArmoryRegion == Character.CharacterRegion.US && form.Realm == "Dragonmaw" && form.CharacterName == "Emposter")
					{
						Form formForEmposter = new Form();
						Label labelForEmposter = new Label();
						labelForEmposter.Font = new Font(labelForEmposter.Font.FontFamily, 42);
						labelForEmposter.Dock = DockStyle.Fill;
						labelForEmposter.ForeColor = System.Drawing.Color.Red;
						labelForEmposter.Text = "HEY EMPOSTER!\r\n*SLAP SLAP*";
						labelForEmposter.TextAlign = ContentAlignment.MiddleCenter;
						formForEmposter.Controls.Add(labelForEmposter);
						formForEmposter.Width += 100;
						formForEmposter.StartPosition = FormStartPosition.CenterParent;
						formForEmposter.Show(this);
						Application.DoEvents();
					}

					LoadCharacter(Armory.GetCharacter(form.ArmoryRegion, form.Realm, form.CharacterName), string.Empty);
					_unsavedChanges = true;
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(_characterPath))
			{
				this.Cursor = Cursors.WaitCursor;
				Character.Save(_characterPath);
				_unsavedChanges = false;
				AddRecentCharacter(_characterPath);
				this.Cursor = Cursors.Default;
			}
			else
			{
				saveAsToolStripMenuItem_Click(null, null);
			}
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.DefaultExt = ".xml";
			dialog.Filter = "Rawr Xml Character Files | *.xml";
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				this.Cursor = Cursors.WaitCursor;
				Character.Save(dialog.FileName);
				_characterPath = dialog.FileName;
				_unsavedChanges = false;
				AddRecentCharacter(_characterPath);
				this.Cursor = Cursors.Default;
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Config.Save();
			ItemCache.Save();
			e.Cancel = !PromptToSaveBeforeClosing();
		}

		private bool PromptToSaveBeforeClosing()
		{
			if (_unsavedChanges)
			{
				DialogResult result = MessageBox.Show("Would you like to save the current character before closing it?", "Rawr - Save?", MessageBoxButtons.YesNoCancel);
				switch (result)
				{
					case DialogResult.Yes:
						saveToolStripMenuItem_Click(null, null);
						return !string.IsNullOrEmpty(_characterPath);
					case DialogResult.No:
						return true;
					default:
						return false;
				}
			}
			else
				return true;
		}
		#endregion

		private void comboBoxEnchant_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter)
			{   //If I was in World War II, they'd call me S-
				Character.BackEnchant = comboBoxEnchantBack.SelectedItem as Enchant;
				Character.ChestEnchant = comboBoxEnchantChest.SelectedItem as Enchant;
				Character.FeetEnchant = comboBoxEnchantFeet.SelectedItem as Enchant;
				Character.Finger1Enchant = comboBoxEnchantFinger1.SelectedItem as Enchant;
				Character.Finger2Enchant = comboBoxEnchantFinger2.SelectedItem as Enchant;
				Character.HandsEnchant = comboBoxEnchantHands.SelectedItem as Enchant;
				Character.HeadEnchant = comboBoxEnchantHead.SelectedItem as Enchant;
				Character.LegsEnchant = comboBoxEnchantLegs.SelectedItem as Enchant;
				Character.ShouldersEnchant = comboBoxEnchantShoulders.SelectedItem as Enchant;
				Character.MainHandEnchant = comboBoxEnchantMainHand.SelectedItem as Enchant;
				Character.OffHandEnchant = comboBoxEnchantOffHand.SelectedItem as Enchant;
				Character.RangedEnchant = comboBoxEnchantRanged.SelectedItem as Enchant;
				Character.WristEnchant = comboBoxEnchantWrists.SelectedItem as Enchant;
				Character.OnItemsChanged();
			}   //...Fire!
		}

		private void comboBoxRace_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter)
			{
				Character.Race = (Character.CharacterRace)Enum.Parse(typeof(Character.CharacterRace), comboBoxRace.Text);
				Character.OnItemsChanged();
			}
		}

		private void radioButtonRegion_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter)
			{
				Character.Region = radioButtonRegionUS.Checked ? Character.CharacterRegion.US : Character.CharacterRegion.EU;
				_unsavedChanges = true;
			}
		}

		private void textBoxName_TextChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter)
			{
				Character.Name = textBoxName.Text;
				_unsavedChanges = true;
			}
		}

		private void textBoxRealm_TextChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter)
			{
				Character.Realm = textBoxRealm.Text;
				_unsavedChanges = true;
			}
		}

		private void copyCharacterStatsToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
            foreach (int slotNum in Enum.GetValues(typeof(Character.CharacterSlot)))
            {
                Character.CharacterSlot slot = (Character.CharacterSlot)(slotNum);
                Item item = Character[slot];
                if (item != null)
                {
                    sb.AppendFormat("{0}\t{1}", slot,item.Name);
                    sb.AppendLine();
                }
            }

            sb.AppendLine(Calculations.GetCharacterStatsString(Character));

			Clipboard.SetText(sb.ToString(), TextDataFormat.Text);
		}

		private void slotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			foreach (ToolStripItem item in toolStripDropDownButtonSlot.DropDownItems)
			{
				if (item is ToolStripMenuItem)
				{
					(item as ToolStripMenuItem).Checked = item == sender;
					if ((item as ToolStripMenuItem).Checked)
					{
						string[] tag = item.Tag.ToString().Split('.');
						toolStripDropDownButtonSlot.Text = tag[0];
						if (tag.Length > 1) toolStripDropDownButtonSlot.Text += " > " + item.Text;
					}
				}
			}
			LoadComparisonData();
			this.Cursor = Cursors.Default;
		}

		private void LoadComparisonData()
		{
			foreach (ToolStripItem item in toolStripDropDownButtonSlot.DropDownItems)
			{
				if (item is ToolStripMenuItem && (item as ToolStripMenuItem).Checked && item.Tag != null)
				{
					string[] tag = item.Tag.ToString().Split('.');
					switch (tag[0])
					{
						case "Gear":
						case "Gems":
							itemComparison1.LoadGearBySlot((Character.CharacterSlot)Enum.Parse(typeof(Character.CharacterSlot), tag[1]));
							break;

						case "Enchants":
							itemComparison1.LoadEnchantsBySlot((Item.ItemSlot)Enum.Parse(typeof(Item.ItemSlot), tag[1]), _calculatedStats);
							break;

						case "Buffs":
							string buffType = tag[1];
							bool activeOnly = buffType.EndsWith("+");
							buffType = buffType.Replace("+", "");
							itemComparison1.LoadBuffs(_calculatedStats, (Buff.BuffType)Enum.Parse(typeof(Buff.BuffType), buffType), activeOnly);
							break;

						case "Current Gear/Enchants/Buffs":
							itemComparison1.LoadCurrentGearEnchantsBuffs(_calculatedStats);
							break;

						case "Custom":
							itemComparison1.LoadCustomChart(tag[1]);
							break;
					}
				}
			}
		}

		private void sortToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			ComparisonGraph.ComparisonSort sort = ComparisonGraph.ComparisonSort.Overall;
			foreach (ToolStripItem item in toolStripDropDownButtonSort.DropDownItems)
			{
				if (item is ToolStripMenuItem)
				{
					(item as ToolStripMenuItem).Checked = item == sender;
					if ((item as ToolStripMenuItem).Checked)
					{
						toolStripDropDownButtonSort.Text = item.Text;
						sort = (ComparisonGraph.ComparisonSort)((int)item.Tag);
					}
				}
			}
			itemComparison1.Sort = sort;
			this.Cursor = Cursors.Default;
		}

		private void loadPossibleUpgradesFromArmoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			_spash = new FormSplash();
			_spash.Show();
			Application.DoEvents();
			Armory.LoadUpgradesFromArmory(Character);
			ItemCache.OnItemsChanged();
			_spash.Hide();
			_spash.Dispose();
			this.Cursor = Cursors.Default;
		}

        private void updateAllItemsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
			foreach (Item item in ItemCache.AllItems)
			{
				if (item.Id < 90000)
				{
					Item newItem = Item.LoadFromId(item.GemmedId, true, "Refreshing");
					if (newItem == null)
					{
						MessageBox.Show("Unable to find item " + item.Id + ". Reverting to previous data.");
						ItemCache.AddItem(item, true, false);
					}
				}
			}
			ItemCache.OnItemsChanged();
			//AddPTRItems();
        }

		private void AddPTRItems()
		{
			Item[] ptrItems = new Item[]
			{
				//new Item() { Name = "Hard Khorium Choker", Id = 90001, Slot = Item.ItemSlot.Neck, IconPath = "temp", Stats = new Stats()
				//{ Stamina = 42, HasteRating = 29, AttackPower = 58, ArmorPenetration = 150 }},
				//new Item() { Name = "Hard Khorium Band", Id = 90002, Slot = Item.ItemSlot.Finger, IconPath = "temp", Stats = new Stats()
				//{ Agility = 30, Stamina = 42, HasteRating = 28, AttackPower = 58 }},
				//new Item() { Name = "Bladed Chaos Tunic", Id = 90003, Slot = Item.ItemSlot.Chest, IconPath = "temp", Stats = new Stats()
				//{ Armor = 474, Agility = 42, Stamina = 45, CritRating = 38, AttackPower = 120, ArmorPenetration = 210 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Blue, Color2 = Item.ItemSlot.Yellow, Color3 = Item.ItemSlot.Red,
				//    Stats = new Stats() { AttackPower = 8 }}},
				//new Item() { Name = "Gloves of the Forest Drifter", Id = 90004, Slot = Item.ItemSlot.Hands, IconPath = "temp", Stats = new Stats()
				//{ Armor = 539, Strength = 34, Agility = 34, Stamina = 45, ArmorPenetration = 140 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red, Color2 = Item.ItemSlot.Blue,
				//    Stats = new Stats() { Agility = 3 }}},
				//new Item() { Name = "Harness of Carnal Instinct", Id = 90005, Slot = Item.ItemSlot.Chest, IconPath = "temp", Stats = new Stats()
				//{ Armor = 727, Strength = 52, Agility = 44, Stamina = 64, ArmorPenetration = 196 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red, Color2 = Item.ItemSlot.Yellow, Color3 = Item.ItemSlot.Blue,
				//    Stats = new Stats() { Agility = 4 }}},
				//new Item() { Name = "Shadowed Gauntlets of Paroxysm", Id = 90006, Slot = Item.ItemSlot.Hands, IconPath = "temp", Stats = new Stats()
				//{ Armor = 252, Agility = 41, Stamina = 33, HasteRating = 30, AttackPower = 82 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red, Color2 = Item.ItemSlot.Blue,
				//    Stats = new Stats() { Agility = 3 }}},
				//new Item() { Name = "Demontooth Shoulderpads", Id = 90007, Slot = Item.ItemSlot.Shoulders, IconPath = "temp", Stats = new Stats()
				//{ Armor = 484, Strength = 38, Agility = 38, Stamina = 38, CritRating = 20, ArmorPenetration = 105 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red, Color2 = Item.ItemSlot.Blue,
				//    Stats = new Stats() { Agility = 3 }}},
				//new Item() { Name = "Shoulderpads of Vehemence", Id = 90008, Slot = Item.ItemSlot.Shoulders, IconPath = "temp", Stats = new Stats()
				//{ Armor = 333, Agility = 33, Stamina = 45, HitRating = 26, HasteRating = 30, AttackPower = 90 }},
				//new Item() { Name = "Leggings of the Immortal Beast", Id = 90009, Slot = Item.ItemSlot.Legs, IconPath = "temp", Stats = new Stats()
				//{ Armor = 765, Strength = 44, Agility = 46, Stamina = 66, ArmorPenetration = 169 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red, Color2 = Item.ItemSlot.Red, Color3 = Item.ItemSlot.Blue,
				//    Stats = new Stats() { Agility = 4 }}},
				//new Item() { Name = "Leggings of the Immortal Night", Id = 90010, Slot = Item.ItemSlot.Shoulders, IconPath = "temp", Stats = new Stats()
				//{ Armor = 380, Agility = 41, Stamina = 48, HitRating = 32, AttackPower = 124, ArmorPenetration = 224 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red, Color2 = Item.ItemSlot.Red, Color3 = Item.ItemSlot.Red,
				//    Stats = new Stats() { Agility = 4 }}},
				//new Item() { Name = "Mask of the Furry Hunter", Id = 90011, Slot = Item.ItemSlot.Head, IconPath = "temp", Stats = new Stats()
				//{ Armor = 611, Strength = 50, Agility = 50, Stamina = 58, CritRating = 30 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red, Color2 = Item.ItemSlot.Meta,
				//    Stats = new Stats() { Stamina = 6 }}},
				//new Item() { Name = "Duplicitous Guise", Id = 90012, Slot = Item.ItemSlot.Head, IconPath = "temp", Stats = new Stats()
				//{ Armor = 373, Agility = 43, Stamina = 57, HitRating = 30, HasteRating = 34, AttackPower = 126 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Meta, Color2 = Item.ItemSlot.Red,
				//    Stats = new Stats() { HitRating = 4 }}},
				//new Item() { Name = "Gloves of Immortal Dusk", Id = 90013, Slot = Item.ItemSlot.Hands, IconPath = "temp", Stats = new Stats()
				//{ Armor = 252, Agility = 30, Stamina = 33, CritRating = 30, AttackPower = 90, ArmorPenetration = 154 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red, Color2 = Item.ItemSlot.Red,
				//    Stats = new Stats() { CritRating = 3 }}},
				//new Item() { Name = "Carapace of Sun and Shadow", Id = 90014, Slot = Item.ItemSlot.Chest, IconPath = "temp", Stats = new Stats()
				//{ Armor = 474, Agility = 42, Stamina = 45, HasteRating = 38, HitRating = 30, AttackPower = 120 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Yellow, Color2 = Item.ItemSlot.Red, Color3 = Item.ItemSlot.Red,
				//    Stats = new Stats() { AttackPower = 8 }}},
				//new Item() { Name = "Quad Deathblow X44 Goggles", Id = 90015, Slot = Item.ItemSlot.Head, IconPath = "temp", Stats = new Stats()
				//{ Armor = 326, Agility = 61, Stamina = 47, HitRating = 24, AttackPower = 104 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Meta, Color2 = Item.ItemSlot.Blue,
				//    Stats = new Stats() { Agility = 4 }}},
				//new Item() { Name = "Thunderheart Treads", Id = 90016, Slot = Item.ItemSlot.Feet, SetName = "Thunderheart Harness", IconPath = "temp", Stats = new Stats()
				//{ Armor = 515, Strength = 35, Agility = 35, Stamina = 54, ExpertiseRating = 20 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red,
				//    Stats = new Stats() { Stamina = 3 }}},
				//new Item() { Name = "Thunderheart Waistguard", Id = 90017, Slot = Item.ItemSlot.Waist, SetName = "Thunderheart Harness", IconPath = "temp", Stats = new Stats()
				//{ Armor = 319, Strength = 38, Agility = 40, Stamina = 34, HitRating = 22 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red,
				//    Stats = new Stats() { Stamina = 3 }}},
				//new Item() { Name = "Thunderheart Wristguards", Id = 90018, Slot = Item.ItemSlot.Wrist, SetName = "Thunderheart Harness", IconPath = "temp", Stats = new Stats()
				//{ Armor = 264, Strength = 28, Agility = 28, Stamina = 39, ArmorPenetration = 91 },
				//Sockets = new Sockets() { Color1 = Item.ItemSlot.Red,
				//    Stats = new Stats() { Stamina = 3 }}},
				//new Item() { Name = "Stanchion of Primal Instinct", Id = 90019, Slot = Item.ItemSlot.TwoHand, IconPath = "temp", Stats = new Stats()
				//{ Stamina = 50, Strength = 75, AttackPower = 1197, Agility = 47, ArmorPenetration = 350 }},
				//new Item() { Name = "Staff of the Forest Lord", Id = 90020, Slot = Item.ItemSlot.TwoHand, IconPath = "temp", Stats = new Stats()
				//{ Stamina = 78, Strength = 50, AttackPower = 1110, Agility = 52 }}
				
				new Item() { Name = "Sunwell Badge Loot - Melee Ring", Id = 90020, Slot = Item.ItemSlot.Finger, IconPath = "temp", Stats = new Stats()
				{ Agility = 29, Stamina = 28, AttackPower = 58, ArmorPenetration = 126 }},
				new Item() { Name = "Sunwell Badge Loot - Tank Ring", Id = 90021, Slot = Item.ItemSlot.Finger, IconPath = "temp", Stats = new Stats()
				{ Armor = 392, Stamina = 45, DodgeRating = 28 }},
				new Item() { Name = "Embrace of Everlasting Prowess", Id = 90022, Slot = Item.ItemSlot.Chest, IconPath = "temp", Stats = new Stats()
				{ Armor = 711, Strength = 40, Agility = 49, Stamina = 52, HasteRating = 20 },
				Sockets = new Sockets() { Color1 = Item.ItemSlot.Red,
				    Stats = new Stats() { Stamina = 3 }}},
				new Item() { Name = "Tameless Breeches", Id = 90023, Slot = Item.ItemSlot.Legs, IconPath = "temp", Stats = new Stats()
				{ Armor = 667, Strength = 39, Agility = 45, Stamina = 52, HasteRating = 17 },
				Sockets = new Sockets() { Color1 = Item.ItemSlot.Red, Color2 = Item.ItemSlot.Yellow,
				    Stats = new Stats() { Stamina = 4 }}},
				new Item() { Name = "Belt of the Silent Path", Id = 90024, Slot = Item.ItemSlot.Waist, IconPath = "temp", Stats = new Stats()
				{ Armor = 205, AttackPower = 78, Agility = 34, Stamina = 33, HitRating = 23 },
				Sockets = new Sockets() { Color1 = Item.ItemSlot.Yellow,
				    Stats = new Stats() { Agility = 2 }}},
				new Item() { Name = "Tunic of the Dark Hour", Id = 90025, Slot = Item.ItemSlot.Chest, IconPath = "temp", Stats = new Stats()
				{ Armor = 474, AttackPower = 102, Agility = 44, Stamina = 51, HitRating = 34 },
				Sockets = new Sockets() { Color1 = Item.ItemSlot.Red,
				    Stats = new Stats() { Stamina = 3 }}},
				new Item() { Name = "Trousers of the Scryers' Retainer", Id = 90026, Slot = Item.ItemSlot.Legs, IconPath = "temp", Stats = new Stats()
				{ Armor = 380, AttackPower = 104, Agility = 43, Stamina = 45, HitRating = 30 },
				Sockets = new Sockets() { Color1 = Item.ItemSlot.Yellow, Color2 = Item.ItemSlot.Blue,
				    Stats = new Stats() { HitRating = 3 }}},
				new Item() { Name = "Handwraps of the Aggressor", Id = 90027, Slot = Item.ItemSlot.Hands, IconPath = "temp", Stats = new Stats()
				{ Armor = 448, Strength = 30, Agility = 35, Stamina = 36, HasteRating = 13 },
				Sockets = new Sockets() { Color1 = Item.ItemSlot.Yellow,
				    Stats = new Stats() { Agility = 2 }}}
			};

			foreach (Item ptrItem in ptrItems)
			{
				ItemCache.AddItem(ptrItem, true, false);
			}
			ItemCache.OnItemsChanged();
		}
	}
}