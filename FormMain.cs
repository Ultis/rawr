using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Rawr.Forms;
using Rawr.Forms.Utilities;
using System.IO;

namespace Rawr
{
	public partial class FormMain : Form, IFormItemSelectionProvider
	{
        private FormSplash _spash = new FormSplash();
		private string _characterPath = "";
		private bool _unsavedChanges = false;
		private CharacterCalculationsBase _calculatedStats = null;
		private List<ToolStripMenuItem> _recentCharacterMenuItems = new List<ToolStripMenuItem>();
		private bool _loadingCharacter = false;
		private Character _character = null;
		private List<ToolStripMenuItem> _customChartMenuItems = new List<ToolStripMenuItem>();
		private Status _statusForm;
		private string _formatWindowTitle = "Rawr (Beta {0})";


		private FormItemSelection _formItemSelection;
		public FormItemSelection FormItemSelection
		{
			get
			{
				if (_formItemSelection == null)
					_formItemSelection = new FormItemSelection();
				return _formItemSelection;
			}
		}

		private static FormMain _instance;
		public static FormMain Instance { get { return FormMain._instance; } }
		public FormMain()
		{
			_instance = this;
			_spash.Show();
			_statusForm = new Status();
			Application.DoEvents();

			Version version = System.Reflection.Assembly.GetCallingAssembly().GetName().Version;
			_formatWindowTitle = string.Format(_formatWindowTitle, version.Minor.ToString() + "." + version.Build.ToString());

			LoadModel(ConfigModel);
			InitializeComponent();
			Application.DoEvents();
			
			Image icon = ItemIcons.GetItemIcon(Calculations.ModelIcons[ConfigModel], true);
			if (icon != null)
			{
				this.Icon = Icon.FromHandle((icon as Bitmap).GetHicon());
			}
			UpdateRecentCharacterMenuItems();

			ToolStripMenuItem modelsToolStripMenuItem = new ToolStripMenuItem("Models");
			menuStripMain.Items.Add(modelsToolStripMenuItem);
			foreach (KeyValuePair<string, Type> kvp in Calculations.Models)
			{
				ToolStripMenuItem modelToolStripMenuItem = new ToolStripMenuItem(kvp.Key);
				modelToolStripMenuItem.Click += new EventHandler(modelToolStripMenuItem_Click);
				modelToolStripMenuItem.Checked = kvp.Value == Calculations.Instance.GetType();
				modelToolStripMenuItem.Tag = kvp;
				modelsToolStripMenuItem.DropDownItems.Add(modelToolStripMenuItem);
			}

			this.Shown += new EventHandler(FormMain_Shown);
			ItemCache.Instance.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
			Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
			Calculations_ModelChanged(null, null);

			sortToolStripMenuItem_Click(overallToolStripMenuItem, EventArgs.Empty);
			slotToolStripMenuItem_Click(headToolStripMenuItem, EventArgs.Empty);
		}

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
					_character.AvailableItemsChanged -= new EventHandler(_character_AvailableItemsChanged);
                }
				_character = value; 
				if (_character != null)
				{
					this.Cursor = Cursors.WaitCursor;
                    _character.IsLoading = true; // we do not need ItemsChanged event triggering until we call OnItemsChanged at the end
					Calculations.CalculationOptionsPanel.Character = _character;
					ItemToolTip.Instance.Character = FormItemSelection.Character = 
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
					_character.AvailableItemsChanged += new EventHandler(_character_AvailableItemsChanged);
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
                    _character.IsLoading = false;
					_character.OnItemsChanged();
				}
			}
		}

        private void SetTitle()
        {
            StringBuilder sb = new StringBuilder(_formatWindowTitle);
            if (_character != null && !String.IsNullOrEmpty(_character.Name))
            {
                sb.Append(" - ");
                sb.Append(_character.Name);
            }
            if (!String.IsNullOrEmpty(_characterPath))
            {
                sb.Append(" - ");
                sb.Append(Path.GetFileName(_characterPath));
                if (_unsavedChanges)
                {
                    sb.Append("*");
                }
            }
            this.Text = sb.ToString();
        }

		void _character_AvailableItemsChanged(object sender, EventArgs e)
		{
			_unsavedChanges = true;
		}

		void _character_ItemsChanged(object sender, EventArgs e)
		{
            if (this.InvokeRequired)
            {
                InvokeHelper.Invoke(this, "_character_ItemsChanged", new object[] { sender, e });
                return;
            }
			this.Cursor = Cursors.WaitCursor;
			_unsavedChanges = true;

			//itemButtonOffHand.Enabled = _character.MainHand == null || _character.MainHand.Slot != Item.ItemSlot.TwoHand;
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
            SetTitle();
		}

		public void LoadModel(string displayName)
		{
			try
			{
				Calculations.LoadModel(Calculations.Models[displayName]);
			}
			finally
			{
				this.ConfigModel = displayName;
			}
		}

		public string ConfigModel
		{
			get
			{
				return Calculations.ValidModel(Properties.Recent.Default.RecentModel);
			}
			set { Properties.Recent.Default.RecentModel = value; }
		}

		public string[] ConfigRecentCharacters
		{
			get
			{
				string recentCharacters = Properties.Recent.Default.RecentFiles;
				if (string.IsNullOrEmpty(recentCharacters))
				{
					return new string[0];
				}
				else
				{
					return recentCharacters.Split(';');
				}
			}
			set { Properties.Recent.Default.RecentFiles = string.Join(";", value); }
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
                LoadSavedCharacter((sender as ToolStripMenuItem).Tag.ToString());
			}
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
				KeyValuePair<string, Type> kvpModel = (KeyValuePair<string, Type>)modelToolStripMenuItem.Tag;
				Image icon = ItemIcons.GetItemIcon(Calculations.ModelIcons[kvpModel.Key], true);
				if (icon != null)
				{
					this.Icon = Icon.FromHandle((icon as Bitmap).GetHicon());
				}
                this.LoadModel(kvpModel.Key);
			}
		}

		private void Calculations_ModelChanged(object sender, EventArgs e)
		{
            Buff.InvalidateCachedData();
			bool unsavedChanges = _unsavedChanges;

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
			comboBoxEnchantOffHand.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.OffHand).ToArray());
			comboBoxEnchantRanged.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Ranged).ToArray());
			comboBoxEnchantWrists.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Wrist).ToArray());

			Calculations.CalculationOptionsPanel.Dock = DockStyle.Fill;
			tabPageOptions.Controls.Clear();
			tabPageOptions.Controls.Add(Calculations.CalculationOptionsPanel);
            if (Calculations.CanUseAmmo)
            {
                itemButtonProjectile.Visible = true;
                itemButtonProjectileBag.Visible = true;
            }
            else
            {
                itemButtonProjectile.Visible = false;
                itemButtonProjectileBag.Visible = false;
            }
			Character = Character;

			ItemCache.OnItemsChanged();
			Character.OnItemsChanged();
			_unsavedChanges = unsavedChanges;
		}

		void FormMain_Shown(object sender, EventArgs e)
		{
			_spash.Close();
			_spash.Dispose();
            SetTitle();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			Character.ToString();//Load the saved character

			StatusMessaging.Ready = true;
		}

		void ItemCache_ItemsChanged(object sender, EventArgs e)
		{
            if (this.InvokeRequired)
            {
                InvokeHelper.Invoke(this, "ItemCache_ItemsChanged", new object[2] { null, null });
            }
            else
            {
                Item[] items = ItemCache.RelevantItems;
                itemComparison1.Items = items;
                LoadComparisonData();
            }
		}

		private void editItemsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormItemEditor itemEditor = new FormItemEditor(Character);
			itemEditor.ShowDialog(this);
			ItemCache.OnItemsChanged();
		}
		//{
		//    OpenItemEditor();
		//}

		//public void OpenItemEditor() { OpenItemEditor(null); }
		//public void OpenItemEditor(Item selectedItem)
		//{
		//    this.Invoke(new OpenItemEditorDel(_openItemEditor), selectedItem);
		//}

		//private delegate void OpenItemEditorDel(Item selectedItem);
		//private void _openItemEditor(Item selectedItem)
		//{
		//    FormItemEditor itemEditor = new FormItemEditor(Character);
		//    if (selectedItem != null) itemEditor.SelectItem(selectedItem, true);
		//    itemEditor.ShowDialog(this);
		//    ItemCache.OnItemsChanged();
		//}

		#region File Commands
		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (PromptToSaveBeforeClosing())
			{
                _characterPath = null;
				LoadCharacterIntoForm(new Character());
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
					LoadSavedCharacter(dialog.FileName);
				}
                dialog.Dispose();
			}
		}

        private void LoadCharacterIntoForm(Character character)
        {
            Character = character;
            _unsavedChanges = false;
            SetTitle();
        }

        private void LoadSavedCharacter(string path)
        {
            StartProcessing();
            BackgroundWorker bw = new BackgroundWorker();
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_LoadSavedCharacterComplete);
            bw.DoWork += new DoWorkEventHandler(bw_LoadSavedCharacter);
            bw.RunWorkerAsync(path);
        }

        void bw_LoadSavedCharacter(object sender, DoWorkEventArgs e)
        {
            WebRequestWrapper.ResetFatalErrorIndicator();
            StatusMessaging.UpdateStatus("Loading Character", "Loading Saved Character");
            StatusMessaging.UpdateStatus("Update Item Cache", "Queued");
            StatusMessaging.UpdateStatus("Cache Item Icons", "Queued");
            Character character = Character.Load(e.Argument as string);
            StatusMessaging.UpdateStatusFinished("Loading Character");
            if (character != null)
            {
                this.EnsureItemsLoaded(character.GetAllEquipedGearIds());
                _characterPath = e.Argument as string;
                InvokeHelper.Invoke(this, "AddRecentCharacter", new object[] { e.Argument});
                e.Result = character;
            }
        }

        void bw_LoadSavedCharacterComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                //Load Character into UI
                LoadCharacterIntoForm(e.Result as Character);
                FinishedProcessing();
            }
        }

		private void loadFromArmoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (PromptToSaveBeforeClosing())
			{
				FormEnterNameRealm form = new FormEnterNameRealm();
				if (form.ShowDialog() == DialogResult.OK)
				{
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
                    StartProcessing();
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new DoWorkEventHandler(bw_ArmoryGetCharacter);
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_ArmoryGetCharacterComplete);
                    bw.RunWorkerAsync(new string[] {form.CharacterName, form.Realm, form.ArmoryRegion.ToString()});
                    //LoadCharacter(Armory.GetCharacter(form.ArmoryRegion, form.Realm, form.CharacterName), string.Empty);
				}
                form.Dispose();
			}
		}

        void bw_ArmoryGetCharacter(object sender, DoWorkEventArgs e)
        {
            string[] args = e.Argument as string[];
            //just accessing the UI elements from off thread is ok, its changing them thats bad.
            Character.CharacterRegion region = (args[2] == Rawr.Character.CharacterRegion.US.ToString()) ? Rawr.Character.CharacterRegion.US : Rawr.Character.CharacterRegion.EU;
            e.Result = this.GetCharacterFromArmory(args[1], args[0], region);
            _characterPath = "";  
        }

        void bw_ArmoryGetCharacterComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // TODO: log this to the status screen.
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                Character character = e.Result as Character;
                LoadCharacterIntoForm(character);
                _unsavedChanges = true;
            }
            FinishedProcessing();
        }


        private void reloadCurrentCharacterFromArmoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Character.CharacterRegion region = radioButtonRegionUS.Checked ? Rawr.Character.CharacterRegion.US : Rawr.Character.CharacterRegion.EU;  
            if (String.IsNullOrEmpty(Character.Name) || String.IsNullOrEmpty(Character.Realm))
            {
                MessageBox.Show("A valid character has not been loaded, unable to reload.","No Character Loaded",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else if (MessageBox.Show("Confirm reloading " + textBoxName.Text + " from the " + textBoxRealm.Text + "@" + region + " realm ", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                StartProcessing();
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new DoWorkEventHandler(bw_ArmoryReloadCharacter);
                bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_ArmoryGetCharacterReloadComplete);
                bw.RunWorkerAsync(Character);
            }
        }

        void bw_ArmoryReloadCharacter(object sender, DoWorkEventArgs e)
        {
            Character character = e.Argument as Character;
            e.Result = this.ReloadCharacterFromArmory(character);
        }

        void bw_ArmoryGetCharacterReloadComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // TODO: log this to the status screen.
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                Character character = e.Result as Character;
                this.Character = character;
                _unsavedChanges = true;
            }
            FinishedProcessing();
        }

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(_characterPath))
			{
				this.Cursor = Cursors.WaitCursor;
				Character.Save(_characterPath);
				_unsavedChanges = false;
				AddRecentCharacter(_characterPath);
                SetTitle();
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
                SetTitle();
                this.Cursor = Cursors.Default;
            }
            dialog.Dispose();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = !PromptToSaveBeforeClosing();
			Properties.Recent.Default.Save();
			ItemCache.Save();
			
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
            StartProcessing();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_GetArmoryUpgrades);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_StatusCompleted);
            bw.RunWorkerAsync(Character);
		}

        void bw_GetArmoryUpgrades(object sender, DoWorkEventArgs e)
        {
            this.GetArmoryUpgrades(e.Argument as Character);
        }

        private void StartProcessing()
        {
            Cursor = Cursors.WaitCursor;
            if (_statusForm == null || _statusForm.IsDisposed)
            {
                _statusForm = new Status();
            }
            _statusForm.Show(this);
            menuStripMain.Enabled = false;
            ItemContextualMenu.Instance.Enabled = false;
            FormItemSelection.Enabled = false;
        }

        private void FinishedProcessing()
        {
            if (_statusForm != null && !_statusForm.IsDisposed)
            {
                _statusForm.AllowedToClose = true;
                if (_statusForm.HasErrors)
                {
                    _statusForm.SwitchToErrorTab();
                }else
                {
                    _statusForm.Close();
                    _statusForm.Dispose();  
                }
            }
            ItemContextualMenu.Instance.Enabled = true;
            menuStripMain.Enabled = true;
            FormItemSelection.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private void updateAllItemsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            StartProcessing();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_UpdateAllCachedItems);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_StatusCompleted);
            bw.RunWorkerAsync();
        }

        void bw_UpdateAllCachedItems(object sender, DoWorkEventArgs e)
        {
            this.UpdateAllCachedItems();
        }

        void bw_StatusCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error processing request: " + e.Error.Message);
            }
            FinishedProcessing();
        }

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Options options = new Options();
			options.ShowDialog(this);
            options.Dispose();
		}

		private void optimizeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormOptimize optimize = new FormOptimize(Character);
			optimize.ShowDialog(this);
            optimize.Dispose();
		}

		public void UpdateAllCachedItems()
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			StatusMessaging.UpdateStatus("Update All Items", "Beginning Update");
			StatusMessaging.UpdateStatus("Cache Item Icons", "Not Started");
			for (int i = 0; i < ItemCache.AllItems.Length; i++)
			{
				Item item = ItemCache.AllItems[i];
				StatusMessaging.UpdateStatus("Update All Items", "Updating " + i + " of " + ItemCache.AllItems.Length + " items");
				if (item.Id < 90000)
				{
					Item.LoadFromId(item.GemmedId, true, "Refreshing", false);
				}
			}
			StatusMessaging.UpdateStatusFinished("Update All Items");
			ItemIcons.CacheAllIcons(ItemCache.AllItems);
			ItemCache.OnItemsChanged();
		}

		public void GetArmoryUpgrades(Character currentCharacter)
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			StatusMessaging.UpdateStatus("GetArmoryUpgrades", "Getting Armory Updates");
			Armory.LoadUpgradesFromArmory(currentCharacter);
			ItemCache.OnItemsChanged();
			StatusMessaging.UpdateStatusFinished("GetArmoryUpgrades");
		}


		public Character ReloadCharacterFromArmory(Character character)
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			Character reload = GetCharacterFromArmory(character.Realm, character.Name, character.Region);
			if (reload != null)
			{
				//load values for gear from armory into original character
				foreach (Character.CharacterSlot slot in Enum.GetValues(typeof(Character.CharacterSlot)))
				{
					character[slot] = reload[slot];
				}
			}
			return character;
		}

		public Character GetCharacterFromArmory(string realm, string name, Character.CharacterRegion region)
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			StatusMessaging.UpdateStatus("Get Character From Armory", " Downloading Character Definition");
            StatusMessaging.UpdateStatus("Update Item Cache", "Queued");
            StatusMessaging.UpdateStatus("Cache Item Icons", "Queued");
			string[] itemsOnChar;
			Character character = Armory.GetCharacter(region, realm, name, out itemsOnChar);
            StatusMessaging.UpdateStatusFinished("Get Character From Armory");
            if (itemsOnChar != null)
            {
                EnsureItemsLoaded(itemsOnChar);
            }
            else
            {
                StatusMessaging.UpdateStatusFinished("Update Item Cache");
                StatusMessaging.UpdateStatusFinished("Cache Item Icons");

            }
			return character;
		}

        private void EnsureItemsLoaded(string[] ids)
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < ids.Length; i++)
            {
                StatusMessaging.UpdateStatus("Update Item Cache", string.Format("Checking Item Cache for Definitions - {0} of {1}", i, ids.Length));
                Item newItem = Item.LoadFromId(ids[i], false, "Character from Armory", false);
                if (newItem != null)
                {
                    items.Add(newItem);
                }
            }
            StatusMessaging.UpdateStatusFinished("Update Item Cache");
            ItemIcons.CacheAllIcons(items.ToArray());
            ItemCache.OnItemsChanged();
        }
	}
}