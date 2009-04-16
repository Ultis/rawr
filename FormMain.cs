using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using Rawr.Forms;
using System.IO;
using System.Threading;
using System.Drawing.Imaging;

namespace Rawr
{
	public partial class FormMain : Form, IFormItemSelectionProvider
	{
//        private const int INTRO_VERSION = 21;
//        private const string INTRO_TEXT =
//@"  Welcome to Rawr 2.2.0. Rawr now has a brand new gemming system, which 
//should greatly  ease the pains we've all had with gems in Rawr up til now.
//   
//Recent Changes:
//v2.2.0b5
// - PLEASE NOTE: This is a beta of Rawr 2.2. It has not received the same 
//level of testing that we normally put into releases, but we're releasing 
//it in its current form, due to the large number of changes. If you do run 
//into bugs, please post them on our Issue Tracker. Please use the current 
//release version, Rawr 2.1.9, if you encounter showstopping bugs in Rawr 
//2.2.0b5. Thanks!
// - Fixed a bug where relevant items and gemmings wouldn't be updated 
//immediately upon switching models.
// - Fix for the Direct Upgrades chart being broken in some models.
// - More performance improvements to the Optimizer
// - Added 'Load Possible Upgrades from Wowhead' feature. Check the 'Use 
//PTR Data' item inside of it to load upgrades from the PTR Wowhead, as 
//they're discovered on the PTR.
// - Models: Tons of model updates; see ReadMe.txt for details on the 
//changes and status of each model.
//
//If you are an experienced C# dev, a knowledgable theorycrafter, and 
//would like to help out, especially with the models which aren't fully 
//complete, please contact me at cnervig@hotmail.com. Thanks!";

        private string _storedCharacterPath;
        private bool _storedUnsavedChanged;
        private Character _storedCharacter;
        private BatchCharacter _batchCharacter;

        private FormSplash _splash = new FormSplash();
		private string _characterPath = "";
		private bool _unsavedChanges = false;
		private CharacterCalculationsBase _calculatedStats = null;
		private List<ToolStripMenuItem> _recentCharacterMenuItems = new List<ToolStripMenuItem>();
		private bool _loadingCharacter = false;
		private Character _character = null;
		private List<ToolStripMenuItem> _customChartMenuItems = new List<ToolStripMenuItem>();
		private Status _statusForm;
		private string _formatWindowTitle = "Rawr v{0}";
		private System.Threading.Timer _timerCheckForUpdates;

        private FormRelevantItemRefinement _itemRefinement;
        public FormRelevantItemRefinement ItemRefinement
        {
            get
            {
                if (_itemRefinement == null || _itemRefinement.IsDisposed)
                    _itemRefinement = new FormRelevantItemRefinement(null);
                return _itemRefinement;
            }
        }

        private FormItemFilter _formItemFilter;
        public FormItemFilter FormItemFilter
        {
            get
            {
                if (_formItemFilter == null || _formItemFilter.IsDisposed)
                    _formItemFilter = new FormItemFilter();
                return _formItemFilter;
            }
        }

		private FormItemSelection _formItemSelection;
		public FormItemSelection FormItemSelection
		{
			get
			{
				if (_formItemSelection == null || _formItemSelection.IsDisposed)
					_formItemSelection = new FormItemSelection();
				return _formItemSelection;
			}
		}

        public TalentPicker TalentPicker
        {
            get
            {
                return talentPicker1;
            }
        }

		private static FormMain _instance;
		public static FormMain Instance { get { return FormMain._instance; } }
		public FormMain()
		{
			_instance = this;
			_splash.Show();
			_statusForm = new Status();
			Application.DoEvents();

			Version version = System.Reflection.Assembly.GetCallingAssembly().GetName().Version;
			_formatWindowTitle = string.Format(_formatWindowTitle, version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString());

            asyncCalculationStart = new AsynchronousDisplayCalculationDelegate(AsyncCalculationStart);
            asyncCalculationCompleted = new SendOrPostCallback(AsyncCalculationCompleted);

			LoadModel(ConfigModel);
			InitializeComponent();
			if (Type.GetType("Mono.Runtime") != null)
				copyDataToClipboardToolStripMenuItem.Text += " (Doesn't work under Mono)";
			Application.DoEvents();

			Rectangle bounds = ConfigBounds;
			if (bounds.Width >= this.MinimumSize.Width && bounds.Height >= this.MinimumSize.Height)
			{
				this.StartPosition = FormStartPosition.Manual;
				this.Bounds = bounds;
			}
			
			Image icon = ItemIcons.GetItemIcon(Calculations.ModelIcons[ConfigModel], true);
			if (icon != null)
			{
				this.Icon = Icon.FromHandle((icon as Bitmap).GetHicon());
			}
			UpdateRecentCharacterMenuItems();

			//ToolStripMenuItem modelsToolStripMenuItem = new ToolStripMenuItem("Models");
			//menuStripMain.Items.Add(modelsToolStripMenuItem);
			//foreach (KeyValuePair<string, Type> kvp in Calculations.Models)
			//{
			//    ToolStripMenuItem modelToolStripMenuItem = new ToolStripMenuItem(kvp.Key);
			//    modelToolStripMenuItem.Click += new EventHandler(modelToolStripMenuItem_Click);
			//    modelToolStripMenuItem.Checked = kvp.Value == Calculations.Instance.GetType();
			//    modelToolStripMenuItem.Tag = kvp;
			//    modelsToolStripMenuItem.DropDownItems.Add(modelToolStripMenuItem);
			//}

			this.Shown += new EventHandler(FormMain_Shown);
			ItemCache.Instance.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
            Calculations.ModelChanging += new EventHandler(Calculations_ModelChanging);
			Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
            // at this point there is no character
            _character = new Character();
            _character.CurrentModel = ConfigModel;
            _character.Class = Calculations.ModelClasses[_character.CurrentModel];
            _characterPath = string.Empty;
            _unsavedChanges = false;
            // we didn't actually set up the character yet
            // model change will force it to reload and set up all needed events and everything
			Calculations_ModelChanged(null, null);

			//_loadingCharacter = true;
			sortToolStripMenuItem_Click(overallToolStripMenuItem, EventArgs.Empty);
			slotToolStripMenuItem_Click(headToolStripMenuItem, EventArgs.Empty);
			//_loadingCharacter = false;

            UpdateItemFilterDropDown();
		}

		private bool _checkForUpdatesEnabled = true;
		void _timerCheckForUpdates_Callback(object data)
		{
			if (_checkForUpdatesEnabled)
			{
				string latestVersion = new Rawr.WebRequestWrapper().GetBetaVersionString();
				if (!string.IsNullOrEmpty(latestVersion))
				{
					string currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
					if (currentVersion != latestVersion)
					{
						_checkForUpdatesEnabled = false;
						DialogResult result = MessageBox.Show(string.Format("A new version of Rawr has been released, version {0}! Would you like to go to the Rawr site to download the new version?",
							latestVersion), "New Version Released!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
						if (result == DialogResult.Yes)
						{
							Help.ShowHelp(null, "http://rawr.codeplex.com/");
						}
					}
				}
			}
		}

        void Calculations_ModelChanging(object sender, EventArgs e)
        {
            Character.SerializeCalculationOptions();
        }

		public Character Character
		{
			get
			{
				if (_character == null)
				{
					Character character = new Character();
					character.CurrentModel = ConfigModel;
					character.Class = Calculations.ModelClasses[character.CurrentModel];
					Character = character;
					_characterPath = string.Empty;
					_unsavedChanges = false;
				}
				return _character;
			}
			set
			{
				if (_character != null)
				{
					_character.ClassChanged -= new EventHandler(_character_ClassChanged);
					_character.CalculationsInvalidated -= new EventHandler(_character_ItemsChanged);
					_character.AvailableItemsChanged -= new EventHandler(_character_AvailableItemsChanged);
                }
				_character = value;
                if (_batchCharacter != null && _batchCharacter.Character != _character)
                {
                    // we're loading a character that is not a batch character
                    _batchCharacter = null;
                }
				if (_character != null)
				{
					this.Cursor = Cursors.WaitCursor;
                    _character.IsLoading = true; // we do not need ItemsChanged event triggering until we call OnItemsChanged at the end

					Character.CurrentModel = null;
					
					Calculations.CalculationOptionsPanel.Character = _character;
					ItemToolTip.Instance.Character = FormItemSelection.Character = talentPicker1.Character =
						ItemEnchantContextualMenu.Instance.Character = ItemContextualMenu.Instance.Character = buffSelector1.Character = itemComparison1.Character = 
						itemButtonBack.Character = itemButtonChest.Character = itemButtonFeet.Character =
						itemButtonFinger1.Character = itemButtonFinger2.Character = itemButtonHands.Character =
						itemButtonHead.Character = itemButtonRanged.Character = itemButtonLegs.Character =
						itemButtonNeck.Character = itemButtonShirt.Character = itemButtonShoulders.Character =
						itemButtonTabard.Character = itemButtonTrinket1.Character = itemButtonTrinket2.Character =
						itemButtonWaist.Character = itemButtonMainHand.Character = itemButtonOffHand.Character =
						itemButtonProjectile.Character = itemButtonProjectileBag.Character = itemButtonWrist.Character = _character;
					//Ahhh ahhh ahhh ahhh ahhh ahhh ahhh ahhh...

					_character.ClassChanged += new EventHandler(_character_ClassChanged);
					_character.CalculationsInvalidated += new EventHandler(_character_ItemsChanged);
					_character.AvailableItemsChanged += new EventHandler(_character_AvailableItemsChanged);
					_loadingCharacter = true;

					textBoxName.Text = Character.Name;
					textBoxRealm.Text = Character.Realm;
					comboBoxRegion.Text = Character.Region.ToString();
					comboBoxRace.Text = Character.Race.ToString();
					checkBoxEnforceGemRequirements.Checked = Character.EnforceGemRequirements;
                    checkBoxWaistBlacksmithingSocket.Checked = Character.WaistBlacksmithingSocketEnabled;
                    checkBoxWristBlacksmithingSocket.Checked = Character.WristBlacksmithingSocketEnabled;
                    checkBoxHandsBlacksmithingSocket.Checked = Character.HandsBlacksmithingSocketEnabled;
                    if (comboBoxClass.Text != Character.Class.ToString())
					{
						comboBoxClass.Text = Character.Class.ToString();
						_character_ClassChanged(null, null);
					}

					_loadingCharacter = false;
                    _character.IsLoading = false;
					//_character.OnCalculationsInvalidated(); nothing actually changed on the character, we just need calculations
                    _character_ItemsChanged(_character, EventArgs.Empty); // this way it won't cause extra invalidations for other listeners of the event
				}
            }
		}

		void _character_ClassChanged(object sender, EventArgs e)
		{
			_unsavedChanges = true;
            this.Cursor = Cursors.WaitCursor;
            string oldModel = (string)comboBoxModel.SelectedValue;
			if (string.IsNullOrEmpty(oldModel)) oldModel = ConfigModel;
			comboBoxModel.Items.Clear();
			List<string> items = new List<string>();
			foreach (KeyValuePair<string, Character.CharacterClass> kvp in Calculations.ModelClasses)
			{
				if (kvp.Value == _character.Class)
				{
					items.Add(kvp.Key);
				}
			}
			comboBoxModel.Items.AddRange(items.ToArray());
			if (items.Contains(oldModel)) comboBoxModel.SelectedIndex = items.IndexOf(oldModel);
			else if (comboBoxModel.Items.Count > 0) comboBoxModel.SelectedIndex = 0;
            this.Cursor = Cursors.Default;
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

		//private void ItemEnchantsChanged()
		//{
		//    _loadingCharacter = true;
		//    comboBoxEnchantBack.SelectedItem = Character.BackEnchant;
		//    comboBoxEnchantChest.SelectedItem = Character.ChestEnchant;
		//    comboBoxEnchantFeet.SelectedItem = Character.FeetEnchant;
		//    comboBoxEnchantFinger1.SelectedItem = Character.Finger1Enchant;
		//    comboBoxEnchantFinger2.SelectedItem = Character.Finger2Enchant;
		//    comboBoxEnchantHands.SelectedItem = Character.HandsEnchant;
		//    comboBoxEnchantHead.SelectedItem = Character.HeadEnchant;
		//    comboBoxEnchantLegs.SelectedItem = Character.LegsEnchant;
		//    comboBoxEnchantShoulders.SelectedItem = Character.ShouldersEnchant;
		//    comboBoxEnchantMainHand.SelectedItem = Character.MainHandEnchant;
		//    comboBoxEnchantOffHand.SelectedItem = Character.OffHandEnchant;
		//    comboBoxEnchantRanged.SelectedItem = Character.RangedEnchant;
		//    comboBoxEnchantWrists.SelectedItem = Character.WristEnchant;
		//    _loadingCharacter = false;
		//}

        private delegate void AsynchronousDisplayCalculationDelegate(CharacterCalculationsBase calculations, AsyncOperation asyncCalculation);

        private class AsyncCalculationResult
        {
            public CharacterCalculationsBase Calculations;
            public Dictionary<string, string> DisplayCalculationValues;
        }

        AsynchronousDisplayCalculationDelegate asyncCalculationStart;
        SendOrPostCallback asyncCalculationCompleted;
        AsyncOperation asyncCalculation;

        private void AsyncCalculationStart(CharacterCalculationsBase calculations, AsyncOperation asyncCalculation)
        {
            Dictionary<string, string> result = calculations.GetAsynchronousCharacterDisplayCalculationValues();
            asyncCalculation.PostOperationCompleted(asyncCalculationCompleted, new AsyncCalculationResult() { Calculations = calculations, DisplayCalculationValues = result });
        }

        private void AsyncCalculationCompleted(object arg)
        {
            AsyncCalculationResult result = (AsyncCalculationResult)arg;
            if (result.DisplayCalculationValues != null && result.Calculations == _calculatedStats)
            {
                UpdateDisplayCalculationValues(result.DisplayCalculationValues);
                // refresh chart if it's custom chart
                foreach (ToolStripItem item in toolStripDropDownButtonSlot.DropDownItems)
                {
                    if (item is ToolStripMenuItem && (item as ToolStripMenuItem).Checked && item.Tag != null)
                    {
                        itemComparison1.DisplayMode = ComparisonGraph.GraphDisplayMode.Subpoints;
                        string[] tag = item.Tag.ToString().Split('.');
                        switch (tag[0])
                        {
                            case "Custom":
                                itemComparison1.LoadCustomChart(tag[1]);
                                break;
                            case "CustomRendered":
                                itemComparison1.LoadCustomRenderedChart(tag[1]);
                                break;
                        }
                        break;
                    }
                }
                asyncCalculation = null;
            }
        }

		void _character_ItemsChanged(object sender, EventArgs e)
		{
            if (this.InvokeRequired)
            {
                Invoke((EventHandler)_character_ItemsChanged, sender, e);
                //InvokeHelper.Invoke(this, "_character_ItemsChanged", new object[] { sender, e });
                return;
            }
			this.Cursor = Cursors.WaitCursor;
            if (asyncCalculation != null)
            {
                CharacterCalculationsBase oldCalcs = _calculatedStats;
                _calculatedStats = null;
                oldCalcs.CancelAsynchronousCharacterDisplayCalculation();
                asyncCalculation = null;
            }			
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
				//ItemEnchantsChanged();
			}

			//and the clouds above move closer / looking so dissatisfied
			Calculations.ClearCache();
			CharacterCalculationsBase calcs = Calculations.GetCharacterCalculations(Character, null, true, true, true);
			_calculatedStats = calcs;

			FormItemSelection.CurrentCalculations = calcs;
            UpdateDisplayCalculationValues(calcs.GetCharacterDisplayCalculationValues());
            LoadComparisonData();
            if (calcs.RequiresAsynchronousDisplayCalculation)
            {
                asyncCalculation = AsyncOperationManager.CreateOperation(null);
                asyncCalculationStart.BeginInvoke(calcs, asyncCalculation, null, null);
            }

			this.Cursor = Cursors.Default;
			//and the ground below grew colder / as they put you down inside
		}

        public void UpdateDisplayCalculationValues(Dictionary<string, string> displayCalculationValues)
        {
            calculationDisplay1.SetCalculations(displayCalculationValues);
            string status;
            if (!displayCalculationValues.TryGetValue("Status", out status))
            {
                int i = 0;
                status = "Overall: " + Math.Round(_calculatedStats.OverallPoints);
                foreach (KeyValuePair<string, Color> kvp in Calculations.SubPointNameColors)
                {
                    status += ", " + kvp.Key + ": " + Math.Round(_calculatedStats.SubPoints[i]);
                    i++;
                }
                //status = "Rawr version " + typeof(Calculations).Assembly.GetName().Version.ToString();
            }
            toolStripStatusLabel.Text = status;
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
                Image icon = ItemIcons.GetItemIcon(Calculations.ModelIcons[displayName], true);
                if (icon != null)
                {
                    this.Icon = Icon.FromHandle((icon as Bitmap).GetHicon());
                }
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

		public Rectangle ConfigBounds
		{
			get
			{
				return new Rectangle(Properties.Recent.Default.WindowLocation,
					Properties.Recent.Default.WindowSize);
			}
			set 
			{ 
				Properties.Recent.Default.WindowLocation = value.Location; 
				Properties.Recent.Default.WindowSize = value.Size; 
			}
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

        private delegate void AddRecentCharacterDelegate(string character);
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
				fileToolStripMenuItem.DropDownItems.Insert(6, recentCharacterMenuItem);
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
            foreach (string chartName in Calculations.CustomRenderedChartNames)
            {
                ToolStripMenuItem customChartMenuItem = new ToolStripMenuItem(chartName);
                customChartMenuItem.Tag = "CustomRendered." + chartName;
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

	
        //private void modelToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem modelToolStripMenuItem = sender as ToolStripMenuItem;
        //    if (!modelToolStripMenuItem.Checked)
        //    {
        //        foreach (ToolStripMenuItem item in _customChartMenuItems)
        //            if (item.Checked)
        //                slotToolStripMenuItem_Click(toolStripDropDownButtonSlot.DropDownItems[1], null);

        //        foreach (ToolStripMenuItem item in (modelToolStripMenuItem.OwnerItem as ToolStripMenuItem).DropDownItems)
        //            item.Checked = item == modelToolStripMenuItem;
        //        KeyValuePair<string, Type> kvpModel = (KeyValuePair<string, Type>)modelToolStripMenuItem.Tag;
        //        Image icon = ItemIcons.GetItemIcon(Calculations.ModelIcons[kvpModel.Key], true);
        //        if (icon != null)
        //        {
        //            this.Icon = Icon.FromHandle((icon as Bitmap).GetHicon());
        //        }
        //        this.LoadModel(kvpModel.Key);
        //    }
        //}

		private void Calculations_ModelChanged(object sender, EventArgs e)
		{
            bool unsavedChanges = _unsavedChanges;

			Character.CurrentModel = null;

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

			Calculations.CalculationOptionsPanel.Dock = DockStyle.Fill;
			tabPageOptions.Controls.Clear();
			tabPageOptions.Controls.Add(Calculations.CalculationOptionsPanel);
            
			itemButtonProjectile.Visible = itemButtonProjectileBag.Visible = Calculations.CanUseAmmo;
            _loadingCharacter = true; // no need to load the comparison charts for this, it's done when reloading the character
            ItemRefinement.resetLists();
            ItemCache.OnItemsChanged();
            _loadingCharacter = false;
            Character = Character; //Reload the character
			_unsavedChanges = unsavedChanges;
		}

		void FormMain_Shown(object sender, EventArgs e)
		{
			_splash.Close();
			_splash.Dispose();
            SetTitle();

			//if (Properties.Recent.Default.SeenIntroVersion < INTRO_VERSION)
			//{
			//    Properties.Recent.Default.SeenIntroVersion = INTRO_VERSION;
			//    MessageBox.Show(INTRO_TEXT);
			//}
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			Character.ToString();//Load the saved character

			StatusMessaging.Ready = true;
			_timerCheckForUpdates = new System.Threading.Timer(new System.Threading.TimerCallback(_timerCheckForUpdates_Callback));
			_timerCheckForUpdates.Change(3000, 1000 * 60 * 60 * 8); //Check for updates 3 sec after the form loads, and then again every 8 hours

			if (Properties.Recent.Default.ShowStartPage)
				ShowStartPage();
		}

		private void ShowStartPage()
		{
			FormStart formStart = new FormStart(this);
			formStart.Left = this.Left + this.Width / 2 - formStart.Width / 2;
			formStart.Top = this.Top + this.Height / 2 - formStart.Height / 2;
			formStart.Show(this);
		}

		void ItemCache_ItemsChanged(object sender, EventArgs e)
		{
            if (this.InvokeRequired)
            {
                BeginInvoke((EventHandler)ItemCache_ItemsChanged, sender, e);
                //InvokeHelper.Invoke(this, "ItemCache_ItemsChanged", new object[2] { null, null });
            }
            else
            {
                Character.InvalidateItemInstances();
                if (!_loadingCharacter)
                {
                    LoadComparisonData();
                }
            }
		}

        void refineEquipmentParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            ItemRefinement.updateBoxes();
            if (ItemRefinement.ShowDialog(this) == DialogResult.OK)
            {
                ItemFilter.Save("Data" + System.IO.Path.DirectorySeparatorChar + "ItemFilter.xml");
            }
        }

		void defaultGemControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            FormGemmingTemplates GemControl = new FormGemmingTemplates();
            GemControl.ShowDialog(this);
            this.Cursor = Cursors.Default;
            if (GemControl.DialogResult.Equals(DialogResult.OK))
            {
                GemmingTemplate.SaveTemplates();
                ItemCache.OnItemsChanged();
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
			NewCharacter();
		}

		public bool NewCharacter()
		{
			bool ret = false;
			if (PromptToSaveBeforeClosing())
			{
                _characterPath = null;
				LoadCharacterIntoForm(new Character());
				ret = true;
			}
			return ret;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenCharacter();
		}

		public bool OpenCharacter()
		{
			bool ret = false;
			if (PromptToSaveBeforeClosing())
			{
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.DefaultExt = ".xml";
				dialog.Filter = "Rawr Xml Character Files | *.xml";
				dialog.Multiselect = false;
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					LoadSavedCharacter(dialog.FileName);
					ret = true;
				}
				dialog.Dispose();
			}
			return ret;
		}

        private void LoadCharacterIntoForm(Character character)
        {
            LoadCharacterIntoForm(character, false);
        }

        private void LoadCharacterIntoForm(Character character, bool unsavedChanges)
        {
			string characterModel = character.CurrentModel;
            Character = character;
            _unsavedChanges = unsavedChanges;
            SetTitle();
			LoadModel(characterModel);
			comboBoxModel.SelectedItem = characterModel;
        }

        public void BatchCharacterSaved(BatchCharacter character)
        {
            if (_batchCharacter == character)
            {
                _unsavedChanges = false;
                SetTitle();
            }
        }

        public void LoadBatchCharacter(BatchCharacter character)
        {
            if (character.Character != null)
            {
                if (_batchCharacter == null)
                {
                    _storedCharacter = _character;
                    _storedCharacterPath = _characterPath;
                    _storedUnsavedChanged = _unsavedChanges;
                }
                _batchCharacter = character;
                _characterPath = character.AbsulutePath;
                LoadCharacterIntoForm(character.Character, character.UnsavedChanges);
            }
        }

        public void UnloadBatchCharacter()
        {
            if (_batchCharacter != null)
            {
                _batchCharacter = null;
                _characterPath = _storedCharacterPath;
                LoadCharacterIntoForm(_storedCharacter, _storedUnsavedChanged);
                _storedCharacter = null;
            }
        }

        public void LoadSavedCharacter(string path)
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
            _loadingCharacter = true; // suppress item changed event
            Character character = Character.Load(e.Argument as string);
            _loadingCharacter = false;
            StatusMessaging.UpdateStatusFinished("Loading Character");
            if (character != null)
            {
                _loadingCharacter = true; // suppress item changed event
                this.EnsureItemsLoaded(character.GetAllEquippedAndAvailableGearIds());
                _loadingCharacter = false;
                _characterPath = e.Argument as string;
                Invoke((AddRecentCharacterDelegate)AddRecentCharacter, e.Argument);
                //InvokeHelper.Invoke(this, "AddRecentCharacter", new object[] { e.Argument});
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
			LoadCharacterFromArmory();
		}

		public bool LoadCharacterFromArmory()
		{
			bool ret = false;
			if (PromptToSaveBeforeClosing())
			{
				FormEnterNameRealm form = new FormEnterNameRealm();
				form.Icon = this.Icon;
				if (form.ShowDialog(this) == DialogResult.OK)
				{
					StartProcessing();
					BackgroundWorker bw = new BackgroundWorker();
					bw.DoWork += new DoWorkEventHandler(bw_ArmoryGetCharacter);
					bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_ArmoryGetCharacterComplete);
					bw.RunWorkerAsync(new string[] { form.CharacterName, form.Realm, form.ArmoryRegion.ToString() });
					ret = true;
				}
				form.Dispose();
			}
			return ret;
		}

        void bw_ArmoryGetCharacter(object sender, DoWorkEventArgs e)
        {
            string[] args = e.Argument as string[];
            //just accessing the UI elements from off thread is ok, its changing them thats bad.
            Character.CharacterRegion region = (Character.CharacterRegion)Enum.Parse(typeof(Character.CharacterRegion),args[2]);
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
            else if (e.Result != null)
            {
                Character character = e.Result as Character;
                LoadCharacterIntoForm(character);
                _unsavedChanges = true;
            }
            FinishedProcessing();
        }


        private void reloadCurrentCharacterFromArmoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
			Character.CharacterRegion region = (Character.CharacterRegion)Enum.Parse(typeof(Character.CharacterRegion), comboBoxRegion.SelectedItem.ToString());
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
                if (_batchCharacter != null)
                {
                    _batchCharacter.UnsavedChanges = false;
                }
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
			if (dialog.ShowDialog(this) == DialogResult.OK)
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
			ConfigBounds = this.Bounds;
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
			//if (!_loadingCharacter)
			//{   //If I was in World War II, they'd call me S-
			//    Character.BackEnchant = comboBoxEnchantBack.SelectedItem as Enchant;
			//    Character.ChestEnchant = comboBoxEnchantChest.SelectedItem as Enchant;
			//    Character.FeetEnchant = comboBoxEnchantFeet.SelectedItem as Enchant;
			//    Character.Finger1Enchant = comboBoxEnchantFinger1.SelectedItem as Enchant;
			//    Character.Finger2Enchant = comboBoxEnchantFinger2.SelectedItem as Enchant;
			//    Character.HandsEnchant = comboBoxEnchantHands.SelectedItem as Enchant;
			//    Character.HeadEnchant = comboBoxEnchantHead.SelectedItem as Enchant;
			//    Character.LegsEnchant = comboBoxEnchantLegs.SelectedItem as Enchant;
			//    Character.ShouldersEnchant = comboBoxEnchantShoulders.SelectedItem as Enchant;
			//    Character.MainHandEnchant = comboBoxEnchantMainHand.SelectedItem as Enchant;
			//    Character.OffHandEnchant = comboBoxEnchantOffHand.SelectedItem as Enchant;
			//    Character.RangedEnchant = comboBoxEnchantRanged.SelectedItem as Enchant;
			//    Character.WristEnchant = comboBoxEnchantWrists.SelectedItem as Enchant;
			//    Character.OnCalculationsInvalidated();
			//}   //...Fire!
		}

		private void comboBoxRace_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter)
			{
				Character.Race = (Character.CharacterRace)Enum.Parse(typeof(Character.CharacterRace), comboBoxRace.Text);
				Character.OnCalculationsInvalidated();
			}
		}

		private void comboBoxRegion_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter)
			{
				Character.Region = (Character.CharacterRegion)Enum.Parse(typeof(Character.CharacterRegion), comboBoxRegion.Text);
				Character.OnCalculationsInvalidated();
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
                ItemInstance item = Character[slot];
                if (item != null)
                {
                    sb.AppendFormat("{0}\t{1}", slot,item.Item.Name);
                    sb.AppendLine();
                }
            }

            sb.AppendLine(Calculations.GetCharacterStatsString(Character));

			try
			{
				Clipboard.SetText(sb.ToString(), TextDataFormat.Text);
			}
			catch { }
			if (Type.GetType("Mono.Runtime") != null)
			{
				//Clipboard isn't working
				System.IO.File.WriteAllText("stats.txt", sb.ToString());
				MessageBox.Show("Mono doesn't support modifying the clipboard, so stats have been saved to a 'stats.txt' file instead.");
			}
		}

		private void slotToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;

			ToolStripMenuItem clickedMenuItem = (ToolStripMenuItem)sender;
			string selectedTag = clickedMenuItem.Tag.ToString();

			if (gearToolStripMenuItem.DropDownItems.Contains(clickedMenuItem) ||
				toolStripDropDownButtonSubSlotGear.DropDownItems.Contains(clickedMenuItem) ||
				clickedMenuItem == gearToolStripMenuItem)
			{
				gearToolStripMenuItem.Tag = selectedTag;
				if (clickedMenuItem != gearToolStripMenuItem)
				{
					gearToolStripMenuItem.Text = "Gear > " + clickedMenuItem.Text;
					toolStripDropDownButtonSubSlotGear.Text = "> " + clickedMenuItem.Text;
				}

				toolStripDropDownButtonSlot.Text = "Slot: Gear";
				toolStripDropDownButtonSlot.ShowDropDownArrow = false;
				toolStripDropDownButtonSubSlotGear.Visible = true;
				toolStripDropDownButtonSubSlotEnchants.Visible = toolStripDropDownButtonSubSlotGems.Visible =
					toolStripDropDownButtonSubSlotBuffs.Visible = false;
			}
			else if (enchantsToolStripMenuItem.DropDownItems.Contains(clickedMenuItem) ||
				toolStripDropDownButtonSubSlotEnchants.DropDownItems.Contains(clickedMenuItem) ||
				clickedMenuItem == enchantsToolStripMenuItem)
			{
				enchantsToolStripMenuItem.Tag = selectedTag;
				if (clickedMenuItem != enchantsToolStripMenuItem)
				{
					enchantsToolStripMenuItem.Text = "Enchants > " + clickedMenuItem.Text;
					toolStripDropDownButtonSubSlotEnchants.Text = "> " + clickedMenuItem.Text;
				}

				toolStripDropDownButtonSlot.Text = "Slot: Enchants";
				toolStripDropDownButtonSlot.ShowDropDownArrow = false;
				toolStripDropDownButtonSubSlotEnchants.Visible = true;
				toolStripDropDownButtonSubSlotGear.Visible = toolStripDropDownButtonSubSlotGems.Visible =
					toolStripDropDownButtonSubSlotBuffs.Visible = false;
			}
			else if (gemsToolStripMenuItem.DropDownItems.Contains(clickedMenuItem) ||
				toolStripDropDownButtonSubSlotGems.DropDownItems.Contains(clickedMenuItem) ||
				clickedMenuItem == gemsToolStripMenuItem)
			{
				gemsToolStripMenuItem.Tag = selectedTag;
				if (clickedMenuItem != gemsToolStripMenuItem)
				{
					gemsToolStripMenuItem.Text = "Gems > " + clickedMenuItem.Text;
					toolStripDropDownButtonSubSlotGems.Text = "> " + clickedMenuItem.Text;
				}

				toolStripDropDownButtonSlot.Text = "Slot: Gems";
				toolStripDropDownButtonSlot.ShowDropDownArrow = false;
				toolStripDropDownButtonSubSlotGems.Visible = true;
				toolStripDropDownButtonSubSlotGear.Visible = toolStripDropDownButtonSubSlotEnchants.Visible =
					toolStripDropDownButtonSubSlotBuffs.Visible = false;
			}
			else if (buffsToolStripMenuItem.DropDownItems.Contains(clickedMenuItem) ||
				toolStripDropDownButtonSubSlotBuffs.DropDownItems.Contains(clickedMenuItem) ||
				clickedMenuItem == buffsToolStripMenuItem)
			{
				buffsToolStripMenuItem.Tag = selectedTag;
				if (clickedMenuItem != buffsToolStripMenuItem)
				{
					buffsToolStripMenuItem.Text = "Buffs > " + clickedMenuItem.Text;
					toolStripDropDownButtonSubSlotBuffs.Text = "> " + clickedMenuItem.Text;
				}

				toolStripDropDownButtonSlot.Text = "Slot: Buffs";
				toolStripDropDownButtonSlot.ShowDropDownArrow = false;
				toolStripDropDownButtonSubSlotBuffs.Visible = true;
				toolStripDropDownButtonSubSlotGear.Visible = toolStripDropDownButtonSubSlotEnchants.Visible =
					toolStripDropDownButtonSubSlotGems.Visible = false;
			}
			else
			{
				toolStripDropDownButtonSlot.Text = "Slot: " + clickedMenuItem.Text;
				toolStripDropDownButtonSlot.ShowDropDownArrow = true;
				toolStripDropDownButtonSubSlotGear.Visible = toolStripDropDownButtonSubSlotEnchants.Visible = 
					toolStripDropDownButtonSubSlotGems.Visible = toolStripDropDownButtonSubSlotBuffs.Visible = false;
			}

			foreach (ToolStripItem item in toolStripDropDownButtonSlot.DropDownItems)
			{
				ToolStripMenuItem menuItem = item as ToolStripMenuItem;
				if (menuItem != null)
				{
					menuItem.Checked = (string)menuItem.Tag == selectedTag;
					//if (menuItem.Checked)
					//{
					//    toolStripDropDownButtonSlot.Text = "Slot: " + menuItem.Text;
					//}
				}
			}
			foreach (ToolStripMenuItem menuItem in gearToolStripMenuItem.DropDownItems)
				menuItem.Checked = (string)menuItem.Tag == selectedTag;
			foreach (ToolStripMenuItem menuItem in enchantsToolStripMenuItem.DropDownItems)
				menuItem.Checked = (string)menuItem.Tag == selectedTag;
			foreach (ToolStripMenuItem menuItem in gemsToolStripMenuItem.DropDownItems)
				menuItem.Checked = (string)menuItem.Tag == selectedTag;
			foreach (ToolStripMenuItem menuItem in buffsToolStripMenuItem.DropDownItems)
				menuItem.Checked = (string)menuItem.Tag == selectedTag;
			foreach (ToolStripMenuItem menuItem in toolStripDropDownButtonSubSlotGear.DropDownItems)
				menuItem.Checked = (string)menuItem.Tag == selectedTag;
			foreach (ToolStripMenuItem menuItem in toolStripDropDownButtonSubSlotEnchants.DropDownItems)
				menuItem.Checked = (string)menuItem.Tag == selectedTag;
			foreach (ToolStripMenuItem menuItem in toolStripDropDownButtonSubSlotGems.DropDownItems)
				menuItem.Checked = (string)menuItem.Tag == selectedTag;
			foreach (ToolStripMenuItem menuItem in toolStripDropDownButtonSubSlotBuffs.DropDownItems)
				menuItem.Checked = (string)menuItem.Tag == selectedTag;

			toolStripDropDownButtonSlot.DropDown.Close(ToolStripDropDownCloseReason.ItemClicked);

			if (!_loadingCharacter)
				LoadComparisonData(selectedTag);
			this.Cursor = Cursors.Default;
		}

		private string _currentChartTag = "Gear.Head";
		private void LoadComparisonData() { LoadComparisonData(_currentChartTag); }
		private void LoadComparisonData(string chartTag)
		{
			_currentChartTag = chartTag;
			itemComparison1.DisplayMode = ComparisonGraph.GraphDisplayMode.Subpoints;
			string[] tag = chartTag.Split('.');
			copyPawnStringToClipboardToolStripMenuItem.Visible = viewUpgradesOnLootRankToolStripMenuItem.Visible =
				viewUpgradesOnWowheadToolStripMenuItem.Visible = labelRelativeStatValuesWarning.Visible =
				tag[0] == "Relative Stat Values";
            copyEnhSimConfigToClipboardToolStripMenuItem.Visible = _character.CurrentModel == "Enhance";
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
					itemComparison1.LoadBuffs(_calculatedStats, tag[1] == "Current");
					break;

				case "Current Gear/Enchants/Buffs":
					itemComparison1.LoadCurrentGearEnchantsBuffs(_calculatedStats);
					break;

				case "Direct Upgrades":
					itemComparison1.LoadAvailableGear(_calculatedStats);
					break;

				case "Talent Specs":
					itemComparison1.LoadTalentSpecs(talentPicker1);
					break;

				case "Talents":
					itemComparison1.LoadTalents();
					break;

                case "Glyphs":
                    itemComparison1.LoadGlyphs();
                    break;

				case "Relative Stat Values":
					itemComparison1.LoadRelativeStatValues();
					break;

				case "Custom":
					itemComparison1.LoadCustomChart(tag[1]);
					break;

				case "CustomRendered":
					itemComparison1.LoadCustomRenderedChart(tag[1]);
					break;
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
						toolStripDropDownButtonSort.Text = "Sort: " + item.Text;
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

        private void loadPossibleUpgradesFromWowheadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartProcessing();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_GetWowheadUpgrades);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_StatusCompleted);
            bw.RunWorkerAsync(Character);
        }

        void bw_GetWowheadUpgrades(object sender, DoWorkEventArgs e)
        {
            this.GetWowheadUpgrades(e.Argument as Character, usePTRDataToolStripMenuItem.Checked);
        }

        public bool IsUsingPTR()
        {
            return usePTRDataToolStripMenuItem.Checked;
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
                }
				else
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

        private void updateItemCacheArmoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
			if (ConfirmUpdateItemCache())
			{
				StartProcessing();
				BackgroundWorker bw = new BackgroundWorker();
				bw.DoWork += new DoWorkEventHandler(bw_UpdateItemCacheArmory);
				bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_StatusCompleted);
				bw.RunWorkerAsync();
			}
        }

		private bool ConfirmUpdateItemCache()
		{
			return MessageBox.Show("Are you sure you would like to update the item cache? This process takes significant time, and the default item cache is fully updated as of the time of release. This does not add any new items, it only updates the data about items already in your itemcache.",
				"Update Item Cache?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
		}

        void bw_UpdateItemCacheArmory(object sender, DoWorkEventArgs e)
        {
            this.UpdateItemCacheArmory();
        }

		private void updateItemCacheWowheadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ConfirmUpdateItemCache())
			{
				StartProcessing();
				BackgroundWorker bw = new BackgroundWorker();
				bw.DoWork += new DoWorkEventHandler(bw_UpdateItemCacheWowhead);
				bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_StatusCompleted);
				bw.RunWorkerAsync();
			}
		}

		void bw_UpdateItemCacheWowhead(object sender, DoWorkEventArgs e)
		{
			this.UpdateItemCacheWowhead();
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
            if (optimize.ShowUpgradeComparison)
            {
                FormUpgradeComparison.Instance.Show();
                FormUpgradeComparison.Instance.BringToFront();
            }
            optimize.Dispose();
		}

		public void UpdateItemCacheArmory()
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			StatusMessaging.UpdateStatus("Update All Items from Armory", "Beginning Update");
			StatusMessaging.UpdateStatus("Cache Item Icons", "Not Started");
			for (int i = 0; i < ItemCache.AllItems.Length; i++)
			{
				Item item = ItemCache.AllItems[i];
				StatusMessaging.UpdateStatus("Update All Items from Armory", "Updating " + i + " of " + ItemCache.AllItems.Length + " items");
				if (item.Id < 90000)
				{
					Item.LoadFromId(item.Id, true, false, false);
				}
			}
			StatusMessaging.UpdateStatusFinished("Update All Items");
			ItemIcons.CacheAllIcons(ItemCache.AllItems);
			ItemCache.OnItemsChanged();
            _character.InvalidateItemInstances();
		}

		public void UpdateItemCacheWowhead()
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			StatusMessaging.UpdateStatus("Update All Items from Wowhead", "Beginning Update");
			StatusMessaging.UpdateStatus("Cache Item Icons", "Not Started");
			for (int i = 0; i < ItemCache.AllItems.Length; i++)
			{
				Item item = ItemCache.AllItems[i];
                StatusMessaging.UpdateStatus("Update All Items from Wowhead", "Updating " + i + " of " + ItemCache.AllItems.Length + " items");
				if (item.Id < 90000)
				{
					try
					{
                        Item.LoadFromId(item.Id, true, false, true, Rawr.Properties.GeneralSettings.Default.Locale, usePTRDataToolStripMenuItem.Checked ? "ptr" : "www");
					}
					catch (Exception ex)
					{
						MessageBox.Show(string.Format("Unable to update '{0}' due to an error: {1}\r\n\r\n{2}", item.Name, ex.Message, ex.StackTrace));
					}
				}
			}
			StatusMessaging.UpdateStatusFinished("Update All Items");
			ItemIcons.CacheAllIcons(ItemCache.AllItems);
			ItemCache.OnItemsChanged();
            _character.InvalidateItemInstances();
        }

		public void GetArmoryUpgrades(Character currentCharacter)
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			StatusMessaging.UpdateStatus("GetArmoryUpgrades", "Getting Armory Updates");
			Armory.LoadUpgradesFromArmory(currentCharacter);
			ItemCache.OnItemsChanged();
			StatusMessaging.UpdateStatusFinished("GetArmoryUpgrades");
		}

        public void GetWowheadUpgrades(Character currentCharacter, bool usePTR)
        {
            WebRequestWrapper.ResetFatalErrorIndicator();
            StatusMessaging.UpdateStatus("GetWowheadUpgrades", "Getting Wowhead Updates");
            Wowhead.LoadUpgradesFromWowhead(currentCharacter, usePTR);
            ItemCache.OnItemsChanged();
            StatusMessaging.UpdateStatusFinished("GetWowheadUpgrades");
        }

        public Character ReloadCharacterFromArmory(Character character)
		{
			WebRequestWrapper.ResetFatalErrorIndicator();
			Character reload = GetCharacterFromArmory(character.Realm, character.Name, character.Region);
			if (reload != null)
			{
                this.Invoke(new ReloadCharacterFromArmoryUpdateDelegate(this.ReloadCharacterFromCharacterProfilerUpdate), character, reload);
			}
			return character;
		}

        public delegate void ReloadCharacterFromArmoryUpdateDelegate(Character character, Character reload);
        public void ReloadCharacterFromArmoryUpdate(Character character, Character reload)
        {
            //load values for gear from armory into original character
            character.SetItems(reload, true);
            character.AssignAllTalentsFromCharacter(reload, false);
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
                string id = ids[i];
                if (id != null)
                {
                    if (id.IndexOf('.') < 0 && ItemCache.ContainsItemId(int.Parse(id))) continue;
                    string[] s = id.Split('.');
                    Item newItem = Item.LoadFromId(int.Parse(s[0]), false, false, false);
                    if (s.Length >= 4)
                    {
                        Item gem;
                        if (s[1] != "*" && s[1] != "0") gem = Item.LoadFromId(int.Parse(s[1]), false, false, false);
                        if (s[2] != "*" && s[2] != "0") gem = Item.LoadFromId(int.Parse(s[2]), false, false, false);
                        if (s[3] != "*" && s[3] != "0") gem = Item.LoadFromId(int.Parse(s[3]), false, false, false);
                    }
                    if (newItem != null)
                    {
                        items.Add(newItem);
                    }
                }
            }
            StatusMessaging.UpdateStatusFinished("Update Item Cache");
            ItemIcons.CacheAllIcons(items.ToArray());
            ItemCache.OnItemsChanged();
        }

        private void BatchToolsToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            FormBatchTools form = new FormBatchTools(this);
            form.Show();            
        }

        // Charinna
        private void loadFromCharacterProfilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PromptToSaveBeforeClosing())
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".lua";
                dialog.Filter = "Character Profiler Saved Variables Files | *.lua";
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        CharacterProfilerData characterList = new CharacterProfilerData(dialog.FileName);

                        FormChooseCharacter form = new FormChooseCharacter(characterList);

                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            StartProcessing();
                            BackgroundWorker bw = new BackgroundWorker();
                            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_LoadCharacterProfilerComplete);
                            bw.DoWork += new DoWorkEventHandler(bw_LoadCharacterProfiler);
                            bw.RunWorkerAsync(form.Character);
                        }

                        form.Dispose();
                    }
                    catch (InvalidDataException ex)
                    {
                        MessageBox.Show("Unable to parse saved variable file: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading saved variable file: " + ex.Message);
                    }
                }
                dialog.Dispose();
            }
        }

        // Charinna
        void bw_LoadCharacterProfiler(object sender, DoWorkEventArgs e)
        {
            WebRequestWrapper.ResetFatalErrorIndicator();
            StatusMessaging.UpdateStatus("Loading Character", "Loading Saved Character");
            StatusMessaging.UpdateStatus("Update Item Cache", "Queued");
            StatusMessaging.UpdateStatus("Cache Item Icons", "Queued");
            CharacterProfilerCharacter characterProfilerChoice = e.Argument as CharacterProfilerCharacter;
            StatusMessaging.UpdateStatusFinished("Loading Character");
            if (characterProfilerChoice != null)
            {
                this.EnsureItemsLoaded(characterProfilerChoice.Character.GetAllEquippedAndAvailableGearIds());
                _characterPath = null;
                e.Result = characterProfilerChoice.Character;
            }
        }

        // Charinna
        void bw_LoadCharacterProfilerComplete(object sender, RunWorkerCompletedEventArgs e)
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

		private void comboBoxModel_SelectedIndexChanged(object sender, EventArgs e)
		{
            this.Cursor = Cursors.WaitCursor;
            LoadModel((string)comboBoxModel.SelectedItem);
            this.Cursor = Cursors.Default;
		}

		private void comboBoxClass_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter && _character != null)
			{
                this.Cursor = Cursors.WaitCursor;
                Character.Class = (Character.CharacterClass)Enum.Parse(typeof(Character.CharacterClass), comboBoxClass.Text);
                this.Cursor = Cursors.Default;
			}
		}

		private void checkBoxEnforceGemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter && _character != null)
			{
				Character.EnforceGemRequirements = checkBoxEnforceGemRequirements.Checked;
				Character.OnCalculationsInvalidated();
			}
		}

        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ToolStripMenuItem menuItem = ((ToolStripMenuItem)sender);
            menuItem.Checked = !menuItem.Checked;
            if (menuItem == toolStripMenuItemFilterOther)
            {
                ItemFilter.OtherRegexEnabled = menuItem.Checked;
            }
            else
            {
                ((ItemFilterRegex)menuItem.Tag).Enabled = menuItem.Checked;
            }
            ItemCache.OnItemsChanged();
            this.Cursor = Cursors.Default;
        }

        private void UpdateItemFilterDropDown()
        {
            toolStripDropDownButtonFilter.DropDownItems.Clear();
            foreach (ItemFilterRegex regex in ItemFilter.RegexList)
            {
                ToolStripMenuItem toolStripMenuItemFilter = new ToolStripMenuItem(regex.Name);
                toolStripMenuItemFilter.Tag = regex;
                toolStripMenuItemFilter.Checked = regex.Enabled;
                toolStripMenuItemFilter.Click += new System.EventHandler(this.filterToolStripMenuItem_Click);
                toolStripDropDownButtonFilter.DropDownItems.Add(toolStripMenuItemFilter);
            }
            toolStripMenuItemFilterOther.Checked = ItemFilter.OtherRegexEnabled;
            toolStripDropDownButtonFilter.DropDownItems.Add(toolStripMenuItemFilterOther);
        }

        private void toolStripMenuItemItemFilterEditor_Click(object sender, EventArgs e)
        {
            // in order to preserve which filters are enabled we have to save the filters before initiating the edit
            ItemFilter.Save("Data" + System.IO.Path.DirectorySeparatorChar + "ItemFilter.xml");
            FormItemFilter.bindingSourceItemFilter.DataSource = ItemFilter.RegexList;
            if (FormItemFilter.ShowDialog(this) == DialogResult.OK)
            {
				ItemFilter.Save("Data" + System.IO.Path.DirectorySeparatorChar + "ItemFilter.xml");
                UpdateItemFilterDropDown();
                ItemCache.OnItemsChanged();
            }
            else
            {
				ItemFilter.Load("Data" + System.IO.Path.DirectorySeparatorChar + "ItemFilter.xml");
                UpdateItemFilterDropDown(); // you have to rebuild dropdown, because reloading item filters from file makes Tags on drop down menu invalid
            }
        }

		private void FormMain_KeyDown(object sender, KeyEventArgs e)
		{
#if DEBUG
			if (e.KeyCode == (Keys)192 && e.Modifiers == Keys.Alt)
			{
				ToString(); //Breakpoint Here


				int[] ids = new int[] { 45912, 45853, 46172, 46197, 46135, 46140, 46139, 46137, 46136, 46188, 46163, 46132, 46129, 46133, 46134, 46130, 46195, 46165, 46170, 46168, 46193, 46190, 45391, 45419, 45417, 45420, 45421, 45422, 45387, 45392, 46131, 45365, 45367, 45369, 45368, 45388, 45393, 45394, 45395, 45389, 45390, 46191, 46189, 46158, 46183, 46161, 46184, 46185, 46160, 46196, 46159, 46186, 46157, 46187, 46192, 46194, 46123, 46124, 46125, 46126, 46127, 46313, 45351, 45355, 45345, 45356, 45346, 45347, 45357, 45352, 45358, 45348, 45359, 45349, 45353, 45354, 45396, 45397, 45398, 45399, 45400, 46142, 46143, 46144, 46145, 46141, 46205, 46212, 46207, 46200, 46199, 46206, 46201, 46209, 46210, 46202, 46203, 46211, 46204, 46198, 46208, 45360, 45361, 45362, 45363, 45364, 45413, 45412, 45406, 45414, 45401, 45411, 45402, 45408, 45409, 45403, 45415, 45410, 45404, 45405, 45416, 46154, 46173, 46175, 46155, 46179, 46181, 46174, 46180, 46156, 46176, 46153, 46177, 46152, 46182, 46178, 46113, 46119, 46121, 46116, 46122, 46117, 46146, 46162, 46148, 46166, 46164, 46151, 46169, 46150, 46167, 46149, 45375, 45381, 45382, 45376, 45370, 45371, 45383, 45372, 45377, 45384, 45379, 45385, 45380, 45373, 45374, 45340, 45335, 45336, 45341, 45337, 45342, 45338, 45343, 45339, 45344, 45429, 45430, 45431, 45432, 45433, 45635, 45636, 45637, 45641, 45642, 45643, 45644, 45645, 45646, 45647, 45648, 45656, 45657, 45658, 46308, 45165, 45892, 45685, 45852, 45851, 45466, 45675, 45297, 45469, 45694, 45927, 45894, 45185, 45325, 45893, 45677, 45686, 45467, 45679, 45687, 45300, 45680, 45445, 45712, 45676, 45481, 45295, 45472, 45682, 45817, 45659, 45660, 45661, 45287, 45132, 45292, 45286, 45285, 45133, 45291, 45289, 45136, 45504, 45288, 45283, 45134, 46029, 45872, 45327, 45972, 45876, 45315, 45437, 45886, 45511, 46104, 45873, 45810, 45811, 45850, 45866, 45812, 45813, 45438, 45874, 45808, 45809, 45508, 45464, 45435, 45832, 45865, 45441, 45864, 45709, 45439, 45440, 45711, 45888, 45320, 45887, 45707, 45877, 45436, 46108, 45649, 45650, 45651, 45652, 45190, 45195, 45199, 45201, 45200, 45194, 45196, 45197, 45202, 45198, 44601, 44817, 44599, 44854, 44791, 44855, 46098, 46069, 46106, 46070, 45049, 46086, 46087, 46088, 46083, 46085, 46081, 46084, 46082, 45480, 45994, 45483, 45186, 46079, 46080, 46065, 46066, 46076, 46077, 46078, 46064, 46063, 46062, 45052, 46059, 46061, 46060, 46073, 46074, 46075, 45161, 45140, 46058, 46057, 46071, 46072, 45169, 44834, 44835, 44852, 44853, 44843, 45654, 44839, 46114, 44837, 44840, 44836, 44838, 44822, 44820, 46101, 46099, 46100, 45171, 45457, 45170, 45461, 45496, 45485, 45443, 45459, 45995, 45451, 45168, 45566, 45386, 45162, 45462, 45482, 45453, 45460, 45454, 45164, 45187, 45452, 45542, 45487, 45594, 45167, 45510, 45096, 45095, 45098, 45099, 45103, 45093, 45050, 44978, 45815, 45924, 45653, 45655, 44987, 46023, 44998, 44983, 45942, 45137, 45147, 45442, 45142, 45110, 45256, 45246, 45086, 45257, 45319, 45138, 45115, 45158, 45148, 45116, 45250, 45113, 45326, 45247, 45106, 45112, 45150, 45117, 45558, 45119, 45253, 45258, 45557, 45146, 45260, 45149, 45108, 45141, 45151, 45249, 45109, 45259, 45143, 45118, 45166, 45248, 45139, 45228, 45252, 45107, 45111, 45251, 45334, 45424, 45425, 45426, 45427, 45428, 45450, 45145, 45114, 45255, 45144, 45254, 45100, 45094, 45101, 45097, 45104, 45102, 45105, 45089, 45088, 45092, 45090, 45091, 46006, 45038, 45276, 45277, 45006, 45007, 45008, 45009, 45932, 45279, 46026, 45458, 45261, 45282, 45498, 45266, 45700, 45233, 45516, 45234, 45861, 45284, 45449, 45177, 45176, 45179, 45178, 45085, 45991, 45992, 45858, 45862, 45879, 45883, 45880, 45881, 45882, 45987, 45280, 45224, 45271, 45854, 45849, 45518, 45507, 45263, 45193, 45262, 45236, 45503, 45235, 45515, 45998, 45275, 45273, 45514, 45238, 45240, 45272, 45567, 45512, 45237, 45232, 45265, 45513, 45227, 45274, 45239, 45505, 45241, 45501, 45264, 45268, 45330, 45502, 45267, 45225, 45509, 45270, 45907, 45909, 45714, 45715, 45716, 45717, 45500, 45127, 45719, 45720, 45721, 45718, 45722, 45723, 45328, 45905, 45903, 45083, 45082, 45902, 46004, 46005, 45323, 45904, 45034, 45796, 45798, 45039, 45986, 45506, 45857, 46007, 45724, 45984, 45981, 45977, 45978, 45979, 45980, 46001, 46003, 46000, 46002, 45999, 45022, 45790, 45794, 45797, 45792, 45795, 45793, 45741, 45745, 45743, 45742, 45746, 45747, 45744, 45625, 45731, 45733, 45732, 45735, 45734, 45769, 45908, 45766, 45761, 45762, 45768, 45764, 45767, 45753, 45755, 45758, 45757, 45760, 45756, 45799, 45804, 45805, 45806, 45800, 45803, 45775, 45771, 45777, 45772, 45778, 45770, 45776, 45738, 45736, 45740, 45737, 45739, 45781, 45782, 45779, 45785, 45780, 45783, 45789, 45623, 45601, 45622, 45603, 45604, 45602, 45063, 45047, 37490, 45061, 45037, 45820, 45822, 45823, 45821, 45819, 45848, 45831, 45840, 45830, 45829, 45838, 45839, 45846, 45847, 45827, 45837, 45836, 45844, 45845, 45828, 45824, 45833, 45835, 45834, 45826, 45843, 45841, 45825, 45842, 45087, 45632, 45633, 45634, 45638, 45639, 45640, 45773, 45749, 45759, 45748, 45754, 45750, 45730, 45752, 45729, 45751, 45728, 45670, 45668, 45671, 45666, 45672, 45669, 45664, 45667, 45673, 45674, 45579, 45580, 45578, 45577, 45581, 45582, 45585, 45574, 45584, 45583, 45688, 45689, 45690, 45774, 45624, 45706, 45725, 45606, 45590, 45592, 45593, 45591, 45597, 45586, 45595, 45596, 45589 };

				foreach (int id in ids)
				{
					Item item = Wowhead.GetItem(id);
					if (item != null)
						ItemCache.AddItem(item);
				}

				ItemCache.OnItemsChanged();
				
				ToString();
			}
#endif
		}

        public class LoadCharacterProfileArguments
        {
            public Character character;
            public CharacterProfilerCharacter characterProfilerCharacter;
        }

        private void reloadInvetoryFromCharacterProfilerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Character.CharacterRegion region = (Character.CharacterRegion)Enum.Parse(typeof(Character.CharacterRegion), comboBoxRegion.SelectedItem.ToString());
            if (String.IsNullOrEmpty(Character.Name) || String.IsNullOrEmpty(Character.Realm))
            {
                MessageBox.Show("A valid character has not been loaded, unable to reload.", "No Character Loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".lua";
                dialog.Filter = "Character Profiler Saved Variables Files | *.lua";
                dialog.Multiselect = false;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        CharacterProfilerData characterList = new CharacterProfilerData(dialog.FileName);

                        bool found_character = false;
                        for (int r=0; r<characterList.Realms.Count; r++)
                        {
                            if (characterList.Realms[r].Name == this.Character._realm)
                            {
                                for (int c=0; c<characterList.Realms[r].Characters.Count; c++)
                                {
                                    if (characterList.Realms[r].Characters[c].Name == this.Character._name)
                                    {
                                        StartProcessing();
                                        BackgroundWorker bw = new BackgroundWorker();
                                        bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_ReloadCharacterProfilerComplete);
                                        bw.DoWork += new DoWorkEventHandler(bw_ReloadCharacterProfiler);
                                        LoadCharacterProfileArguments args = new LoadCharacterProfileArguments();
                                        args.character = Character;
                                        args.characterProfilerCharacter = characterList.Realms[r].Characters[c];
                                        bw.RunWorkerAsync(args);
                                        // we found the character stop searching
                                        found_character = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!found_character)
                        {
                            string error_msg = string.Format("{0} of {1} was not found in the Character Profiler Data.", this.Character._name, this.Character._realm);
                            MessageBox.Show(error_msg, "Character Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (InvalidDataException ex)
                    {
                        MessageBox.Show("Unable to parse saved variable file: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading saved variable file: " + ex.Message);
                    }
                }
                dialog.Dispose();
            }
        }

        void bw_ReloadCharacterProfiler(object sender, DoWorkEventArgs e)
        {
            LoadCharacterProfileArguments args = e.Argument as LoadCharacterProfileArguments;
            e.Result = this.ReloadCharacterFromCharacterProfiler(args.character, args.characterProfilerCharacter);
        }

        void bw_ReloadCharacterProfilerComplete(object sender, RunWorkerCompletedEventArgs e)
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

        public Character ReloadCharacterFromCharacterProfiler(Character character, CharacterProfilerCharacter characterProfilerCharacter)
        {
            WebRequestWrapper.ResetFatalErrorIndicator();
            Character reload = GetCharacterFromCharacterProfiler(characterProfilerCharacter);
            if (reload != null)
            {
                this.Invoke(new ReloadCharacterFromCharacterProfilerUpdateDelegate(this.ReloadCharacterFromCharacterProfilerUpdate), character, reload);
            }
            return character;
        }

        public delegate void ReloadCharacterFromCharacterProfilerUpdateDelegate(Character character, Character reload);
        public void ReloadCharacterFromCharacterProfilerUpdate(Character character, Character reload)
        {
            //load values for gear from armory into original character
			character.IsLoading = true;
            character.SetItems(reload, true);
			foreach (string existingAvailableItem in character.AvailableItems)
			{
				string itemId = existingAvailableItem.Split('.')[0];
				if (reload.AvailableItems.Contains(itemId)) reload.AvailableItems.Remove(itemId);
			}
            character.AvailableItems.AddRange(reload.AvailableItems);
            character.AssignAllTalentsFromCharacter(reload, false);
			character.IsLoading = false;
			character.OnCalculationsInvalidated();
        }

        public Character GetCharacterFromCharacterProfiler(CharacterProfilerCharacter characterProfilerCharacter)
        {
            WebRequestWrapper.ResetFatalErrorIndicator();
            StatusMessaging.UpdateStatus("Load Character", " Loading Character");
            StatusMessaging.UpdateStatus("Update Item Cache", "Queued");
            StatusMessaging.UpdateStatus("Cache Item Icons", "Queued");
            Character character = characterProfilerCharacter.Character;
            StatusMessaging.UpdateStatusFinished("Load Character");
            if (characterProfilerCharacter.Character != null)
            {
                EnsureItemsLoaded(characterProfilerCharacter.Character.GetAllEquippedAndAvailableGearIds());
            }
            else
            {
                StatusMessaging.UpdateStatusFinished("Update Item Cache");
                StatusMessaging.UpdateStatusFinished("Cache Item Icons");

            }
            return character;
        }

        private void checkBoxWristBlacksmithingSocket_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCharacter && _character != null)
            {
                Character.WristBlacksmithingSocketEnabled = checkBoxWristBlacksmithingSocket.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void checkBoxHandsBlacksmithingSocket_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCharacter && _character != null)
            {
                Character.HandsBlacksmithingSocketEnabled = checkBoxHandsBlacksmithingSocket.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void checkBoxWaistBlacksmithingSocket_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCharacter && _character != null)
            {
                Character.WaistBlacksmithingSocketEnabled = checkBoxWaistBlacksmithingSocket.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

		private void rawrHelpPageToolStripMenuItem_Click(object sender, EventArgs e)
		{ Help.ShowHelp(null, "http://www.codeplex.com/Rawr/Wiki/View.aspx?title=Help"); }

		private void tourOfRawrToolStripMenuItem_Click(object sender, EventArgs e)
		{ Help.ShowHelp(null, "http://www.youtube.com/watch?v=OjRM5SUoOoQ"); }

		private void gemmingsToolStripMenuItem_Click(object sender, EventArgs e)
		{ Help.ShowHelp(null, "http://www.codeplex.com/Rawr/Wiki/View.aspx?title=Gemmings"); }

		private void gearOptimizationToolStripMenuItem_Click(object sender, EventArgs e)
		{ Help.ShowHelp(null, "http://www.codeplex.com/Rawr/Wiki/View.aspx?title=GearOptimization"); }

		private void batchToolsToolStripMenuItem_Click(object sender, EventArgs e)
		{ Help.ShowHelp(null, "http://www.codeplex.com/Rawr/Wiki/View.aspx?title=BatchTools"); }

		private void itemFilteringToolStripMenuItem_Click(object sender, EventArgs e)
		{ Help.ShowHelp(null, "http://www.codeplex.com/Rawr/Wiki/View.aspx?title=ItemFiltering"); }

		private void rawrWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
		{ Help.ShowHelp(null, "http://rawr.codeplex.com/"); }

		private void donateToolStripMenuItem_Click(object sender, EventArgs e)
		{ Help.ShowHelp(null, "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=2451163"); }

        private void upgradeListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.DefaultExt = ".xml";
			dialog.Filter = "Rawr Upgrade List Files | *.xml";
			dialog.Multiselect = false;
			if (dialog.ShowDialog(this) == DialogResult.OK && 
				FormUpgradeComparison.Instance.LoadFile(dialog.FileName))
			{
				FormUpgradeComparison.Instance.Show();
				FormUpgradeComparison.Instance.BringToFront();
			}
			dialog.Dispose();
        }

        #region Exports
        private void viewUpgradesOnWowheadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_loadingCharacter && _character != null)
                Help.ShowHelp(null, Wowhead.GetWowheadWeightedReportURL(_character));
        }

        private void copyPawnStringToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_loadingCharacter && _character != null)
                Exports.CopyPawnString(_character);
        }

        private void viewUpgradesOnLootRankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_loadingCharacter && _character != null)
                Help.ShowHelp(null, Exports.GetLootRankURL(_character));
		}

		private string GetChartDataCSV()
		{
			StringBuilder sb = new StringBuilder("Name,Equipped,Slot,Gem1,Gem2,Gem3,Enchant,Source,ItemId,GemmedId,Overall");
			foreach (string subPointName in Calculations.SubPointNameColors.Keys)
			{
				sb.AppendFormat(",{0}", subPointName);
			}
			sb.AppendLine();
			foreach (ComparisonCalculationBase comparisonCalculation in itemComparison1.ComparisonGraph.ItemCalculations)
			{
				ItemInstance itemInstance = comparisonCalculation.ItemInstance;
				Item item = comparisonCalculation.Item;
				if (itemInstance != null)
				{
					sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
						itemInstance.Item.Name.Replace(',', ';'),
						comparisonCalculation.Equipped,
						itemInstance.Slot,
						itemInstance.Gem1 != null ? itemInstance.Gem1.Name : null,
						itemInstance.Gem2 != null ? itemInstance.Gem2.Name : null,
						itemInstance.Gem3 != null ? itemInstance.Gem3.Name : null,
						itemInstance.Enchant.Name,
						itemInstance.Item.LocationInfo.Description.Split(',')[0],
						itemInstance.Id,
						itemInstance.GemmedId,
						comparisonCalculation.OverallPoints);
					foreach (float subPoint in comparisonCalculation.SubPoints)
						sb.AppendFormat(",{0}", subPoint);
					sb.AppendLine();
				}
				else if (item != null)
				{
					sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
						item.Name.Replace(',', ';'),
						comparisonCalculation.Equipped,
						item.Slot,
						null,
						null,
						null,
						null,
						item.LocationInfo.Description.Split(',')[0],
						item.Id,
						null,
						comparisonCalculation.OverallPoints);
					foreach (float subPoint in comparisonCalculation.SubPoints)
						sb.AppendFormat(",{0}", subPoint);
					sb.AppendLine();
				}
				else
				{
					sb.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
						comparisonCalculation.Name.Replace(',', ';'),
						comparisonCalculation.Equipped,
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						comparisonCalculation.OverallPoints);
					foreach (float subPoint in comparisonCalculation.SubPoints)
						sb.AppendFormat(",{0}", subPoint);
					sb.AppendLine();
				}
			}

			return sb.ToString();
		}

		private void copyDataToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				Clipboard.SetText(GetChartDataCSV(), TextDataFormat.Text);
			}
			catch { }
		}

		private void exportToImageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.DefaultExt = ".png";
			dialog.Filter = "PNG|*.png|GIF|*.gif|JPG|*.jpg|BMP|*.bmp";
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					ImageFormat imgFormat = ImageFormat.Bmp;
					if (dialog.FileName.EndsWith(".png")) imgFormat = ImageFormat.Png;
					else if (dialog.FileName.EndsWith(".gif")) imgFormat = ImageFormat.Gif;
					else if (dialog.FileName.EndsWith(".jpg") || dialog.FileName.EndsWith(".jpeg")) imgFormat = ImageFormat.Jpeg;
					itemComparison1.ComparisonGraph.PrerenderedGraph.Save(dialog.FileName, imgFormat);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			dialog.Dispose();
		}

		private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.DefaultExt = ".csv";
			dialog.Filter = "Comma Separated Values | *.csv";
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					using (StreamWriter writer = System.IO.File.CreateText(dialog.FileName))
					{
						writer.Write(GetChartDataCSV());
						writer.Flush();
						writer.Close();
						writer.Dispose();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			dialog.Dispose();
		}

        private void copyEnhSimConfigToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Type formOptionsPanel = Calculations.CalculationOptionsPanel.GetType();
            if (formOptionsPanel.FullName == "Rawr.CalculationOptionsPanelEnhance")
            {
                MethodInfo exportMethod = formOptionsPanel.GetMethod("Export");
                if (exportMethod != null)
                {
                    exportMethod.Invoke(Calculations.CalculationOptionsPanel, null);
                }
            }
        }

        #endregion

		private void startPageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowStartPage();
		}

        private void toolStripDropDownButtonSort_Click(object sender, EventArgs e)
        {

        }
    }
}
