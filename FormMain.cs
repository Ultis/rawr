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
		private const int INTRO_VERSION = 13;
		private const string INTRO_TEXT =
@" Welcome to Rawr 2.1.6. Rawr is now designed for use with WoW 3.0, primarily for characters up to level 80. Some things to note:
   •We're not done. We've included only the models that have been updated for WoW 3.0; older models are available via our source control only, since they're not of much use until they get updated.
   •To help you stay updated with the latest changes, Rawr will now check for new available updates, notify you if there's a newer version, and offer to open Rawr's website for you.
   •We now have support for loading item data from Wowhead. Please note that the Wowhead parsing is brand new, so there are bound to be bugs. Please report any bugs you find, and we'll try to get them fixed asap.

Recent Changes:
v2.1.6:
	Fixes for several Armory/Wowhead parsing errors.
	Fixes for stats on a couple buffs.
	Fix for a crash when choosing No Enchant from the enchant dropdowns.
	Rawr.Hunter/Bear/Cat: Adjusted miss chance to 8%.
	Rawr.TankDK: Initial release. Not fully complete yet, but included in this release of Rawr so that you can see how we're progressing. We still advise using Rawr.TankDK in conjunction with other theorycrafting tools.

v2.1.5: 
	Items now show their item level in their tooltips. 
	Armor and Bonus Armor are now handled separately. 
	Performance and crash fixes to the Optimizer. 
	Added mousewheel support for the charts. 
	Added buffs for Mixology. 
	Added support for Blacksmithing sockets (reload character from armory, or load character from 2.1.4 and remove extra gem from items). 
	Added a context menu item to quickly evaluate the upgrade value of the clicked item.
	Added support for updating the entire item cache from Wowhead.
	Added parsing for Greatness cards.
	Rawr.Bear: Now includes correct calcultions for armor in patch 3.0.8. Fix for damage reduction calculations for different level targets. Added support for Mongoose. Fixed calculations for Idol of Terror. Added support for soft-capping Survivability.
	Rawr.Cat: Fix for damage reduction calculations for different level targets. Added support for Mongoose, Berserking, Trauma, and Mangle from another feral. Fixed calculations for Idol of Terror.
	Rawr.HolyPriest: Added Arcane Torrent for blood elves. Support for more set bonuses.
	Rawr.Mage: Added custom graphs for stats scaling. Supports extra crit rate from encounter effects (ie, Loatheb). Fix for new cycles ignoring flame caps. Support for reconjuring mana gems.
	Rawr.ProtWarr: Adjusted miss rate from 9% to 8%, and dodge rate from 6.5% to 6.4%.
	Rawr.ShadowPriest Added Arcane Torrent for blood elves. Fixed display issue with Misery/Faerie Fire hit. Support for more trinket effects. Support for more set bonuses.
	Rawr.Tankadin: Fix for block rating conversion.
	Rawr.DPSDK: Fixes for a few calculations. Fixes for item relevancy to show DK set items, not paladin ones.
	Rawr.RestoSham: Updated a variety of calculations. Not fully updated yet, but included in this release of Rawr so that you can see how we're progressing. We still advise using Rawr.RestoSham in conjunction with other theorycrafting tools.

Here's a quick rundown of the status of each model:
   •Rawr.Base: Fully functional. Still want to implement a global interface for glyphs, but they're left up to each model for now.
   •Rawr.Bear: Fully functional for level 80.
   •Rawr.Cat: Fully functional for level 80.
   •Rawr.DPSDK: Fully functional for level 80, but still has a few problems since it's brand new.
   •Rawr.DPSWarr: Not functional for 3.0.
   •Rawr.Enhance: Fully functional for level 80, but still has a few problems since it's brand new.
   •Rawr.Healadin: Fully functional for level 80.
   •Rawr.HolyPriest: Fully functional for level 80.
   •Rawr.Hunter: Fully functional for level 80, but still has a few problems since it's brand new.
   •Rawr.Mage: Fully functional for level 80.
   •Rawr.Moonkin: Fully functional for level 80.
   •Rawr.ProtWarr: Partially functional for 3.0 & level 80.
   •Rawr.RestoSham: Partially functional for 3.0.
   •Rawr.Retribution: Fully functional for level 80.
   •Rawr.Rogue: Not functional for 3.0.
   •Rawr.ShadowPriest: Fully functional for level 80.
   •Rawr.Tankadin: Fully functional for level 80.
   •Rawr.TankDK: Partially functional for 3.0.
   •Rawr.Tree: Partially functional for level 80.
   •Rawr.Warlock: Not functional for 3.0.
    
    
 As you can see, we still have alot of work ahead of us, but we're actively working on it. If you are an experienced C# dev, a knowledgable theorycrafter, and would like to help out, especially with the models which we haven't begun updating for 3.0, please contact me at cnervig@hotmail.com. Thanks, and look forward to frequent updates!";
	

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
        private FormMassGemReplacement _gemControl;

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

        public FormMassGemReplacement GemControl
        {
            get
            {
                if (_gemControl == null || _gemControl.IsDisposed)
                    _gemControl = new FormMassGemReplacement();
                return _gemControl;
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

			LoadModel(ConfigModel);
			InitializeComponent();
			Application.DoEvents();
			
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
			Calculations_ModelChanged(null, null); // this will trigger items changed event which triggers loading of item cache

			sortToolStripMenuItem_Click(overallToolStripMenuItem, EventArgs.Empty);
			slotToolStripMenuItem_Click(headToolStripMenuItem, EventArgs.Empty);

            // items are already loaded on model change, see above
            //ItemCache.Load(); // make sure item filters are loaded before creating drop down
            UpdateItemFilterDropDown();
		}

		private bool _checkForUpdatesEnabled = true;
		void _timerCheckForUpdates_Callback(object data)
		{
			if (_checkForUpdatesEnabled)
			{
				string latestVersion = new Rawr.WebRequestWrapper().GetLatestVersionString();
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
							Help.ShowHelp(null, "http://www.codeplex.com/Rawr");
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
						itemButtonProjectile.Character = itemButtonProjectileBag.Character = 
						itemButtonExtraHandsSocket.Character = itemButtonExtraWaistSocket.Character =
						itemButtonExtraWristSocket.Character = itemButtonWrist.Character = _character;
					//Ahhh ahhh ahhh ahhh ahhh ahhh ahhh ahhh...

					_character.ClassChanged += new EventHandler(_character_ClassChanged);
					_character.CalculationsInvalidated += new EventHandler(_character_ItemsChanged);
					_character.AvailableItemsChanged += new EventHandler(_character_AvailableItemsChanged);
					_loadingCharacter = true;

					textBoxName.Text = Character.Name;
					textBoxRealm.Text = Character.Realm;
					comboBoxRegion.Text = Character.Region.ToString();
					comboBoxRace.Text = Character.Race.ToString();
					checkBoxEnforceMetagemRequirements.Checked = Character.EnforceMetagemRequirements;
					if (comboBoxClass.Text != Character.Class.ToString())
					{
						comboBoxClass.Text = Character.Class.ToString();
						_character_ClassChanged(null, null);
					}

					_loadingCharacter = false;
                    _character.IsLoading = false;
					_character.OnCalculationsInvalidated();
				}
			}
		}

		void _character_ClassChanged(object sender, EventArgs e)
		{
			_unsavedChanges = true;
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
				itemButtonExtraHandsSocket.UpdateSelectedItem(); itemButtonExtraWaistSocket.UpdateSelectedItem(); itemButtonExtraWristSocket.UpdateSelectedItem();
				//ItemEnchantsChanged();
			}
			//and the clouds above move closer / looking so dissatisfied
			Calculations.ClearCache();
			CharacterCalculationsBase calcs = Calculations.GetCharacterCalculations(Character);
			_calculatedStats = calcs;

			LoadComparisonData();
			FormItemSelection.CurrentCalculations = calcs;
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
				fileToolStripMenuItem.DropDownItems.Insert(5, recentCharacterMenuItem);
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
            Character = Character; //Reload the character

            ItemRefinement.resetLists();
			ItemCache.OnItemsChanged();
			//Character.OnItemsChanged(); already called when setting Character
			_unsavedChanges = unsavedChanges;
		}

		void FormMain_Shown(object sender, EventArgs e)
		{
			_splash.Close();
			_splash.Dispose();
            SetTitle();

			if (Properties.Recent.Default.SeenIntroVersion < INTRO_VERSION)
			{
				Properties.Recent.Default.SeenIntroVersion = INTRO_VERSION;
				MessageBox.Show(INTRO_TEXT);
			}
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			Character.ToString();//Load the saved character

			StatusMessaging.Ready = true;
			_timerCheckForUpdates = new System.Threading.Timer(new System.Threading.TimerCallback(_timerCheckForUpdates_Callback));
			_timerCheckForUpdates.Change(3000, 1000 * 60 * 60 * 8); //Check for updates 3 sec after the form loads, and then again every 8 hours
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

            FormMassGemReplacement GemControl = new FormMassGemReplacement();
            GemControl.ShowDialog(this);
            if(GemControl.DialogResult.Equals(DialogResult.OK))
                ItemCache.OnItemsChanged();
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
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					LoadSavedCharacter(dialog.FileName);
				}
                dialog.Dispose();
			}
		}

        private void LoadCharacterIntoForm(Character character)
        {
            LoadCharacterIntoForm(character, false);
        }

        private void LoadCharacterIntoForm(Character character, bool unsavedChanges)
        {
            Character = character;
            _unsavedChanges = unsavedChanges;
            initializeAvailableItemList(Character);
            SetTitle();
        }

        /// <summary>
        /// 09.01.01 - TankConcrete
        /// Sets up the initial list of gear that the toon has available. Method looks the
        /// gear that is currently equipped and adds it to the "AvailableItems" list if it
        /// is not already there.
        /// 
        /// If a new item is added to the "AvailableItems" list, the "unsaved" changes flag
        /// will be updated so the user knows to save.
        /// </summary>
        /// <param name="currentChar"></param>
        private void initializeAvailableItemList(Character currentChar)
        {
            // Get the current list of items that we know about.
            string[] equippedItems = currentChar.GetAllEquipedAndAvailableGearIds();
            string itemId = string.Empty;

            // Loop through the list of known and equipped items
            foreach (string item in equippedItems)
            {
                itemId = item;
                // Check to see if this is a compound item id.
                if (item.IndexOf('.') > 0)
                {
                    // We're only concerned with the base item ID, so take out the .*.*.*
                    itemId = item.Substring(0, item.IndexOf('.'));
                }

                // Check the list of available items to see if this item is in the list
                if (!currentChar.AvailableItems.Contains(itemId))
                {
                    // Add this item to our list
                    currentChar.AvailableItems.Add(itemId);

                    // We have unsaved changes - let the user know.
                    this._unsavedChanges = true;
                }
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
                else
                {
                    _batchCharacter.UnsavedChanges = _unsavedChanges;
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
                _batchCharacter.UnsavedChanges = _unsavedChanges;
                _batchCharacter = null;
                _characterPath = _storedCharacterPath;
                LoadCharacterIntoForm(_storedCharacter, _storedUnsavedChanged);
                _storedCharacter = null;
            }
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
                this.EnsureItemsLoaded(character.GetAllEquipedAndAvailableGearIds());
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
                form.Icon = this.Icon;
				if (form.ShowDialog(this) == DialogResult.OK)
				{
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
                Item item = Character[slot];
                if (item != null)
                {
                    sb.AppendFormat("{0}\t{1}", slot,item.Name);
                    sb.AppendLine();
                }
            }

            sb.AppendLine(Calculations.GetCharacterStatsString(Character));

			Clipboard.SetText(sb.ToString(), TextDataFormat.Text);
			if (Type.GetType("Mono.Runtime") != null)
			{
				//Clipboard isn't working
				System.IO.File.WriteAllText("stats.txt", sb.ToString());
			}
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
                        if (tag[0] == "CustomRendered") tag[0] = "Custom";
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
                    itemComparison1.DisplayMode = ComparisonGraph.GraphDisplayMode.Subpoints;
					string[] tag = item.Tag.ToString().Split('.');
					switch (tag[0])
					{
						case "Gear":
						case "Gems":
                            //Character._availableItems.Add(
							itemComparison1.LoadGearBySlot((Character.CharacterSlot)Enum.Parse(typeof(Character.CharacterSlot), tag[1]));
							break;

						case "Enchants":
							itemComparison1.LoadEnchantsBySlot((Item.ItemSlot)Enum.Parse(typeof(Item.ItemSlot), tag[1]), _calculatedStats);
							break;

						case "Buffs":
							string buffType = tag[1];
							bool activeOnly = buffType.EndsWith("+");
							buffType = buffType.Replace("+", "");
							itemComparison1.LoadBuffs(_calculatedStats, activeOnly);
							break;

						case "Current Gear/Enchants/Buffs":
							itemComparison1.LoadCurrentGearEnchantsBuffs(_calculatedStats);
							break;

						case "Direct Upgrades":
							itemComparison1.LoadAvailableGear(_calculatedStats);
							break;

						case "TalentSpecs":
							itemComparison1.LoadTalentSpecs(talentPicker1);
							break;

						case "Talents":
							itemComparison1.LoadTalents();
							break;

						case "Custom":
							itemComparison1.LoadCustomChart(tag[1]);
							break;

                        case "CustomRendered":
                            itemComparison1.LoadCustomRenderedChart(tag[1]);
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
            StartProcessing();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += new DoWorkEventHandler(bw_UpdateItemCacheArmory);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_StatusCompleted);
            bw.RunWorkerAsync();
        }

        void bw_UpdateItemCacheArmory(object sender, DoWorkEventArgs e)
        {
            this.UpdateItemCacheArmory();
        }

		private void updateItemCacheWowheadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StartProcessing();
			BackgroundWorker bw = new BackgroundWorker();
			bw.DoWork += new DoWorkEventHandler(bw_UpdateItemCacheWowhead);
			bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_StatusCompleted);
			bw.RunWorkerAsync();
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
					Item.LoadFromId(item.GemmedId, true, "Refreshing", false);
				}
			}
			StatusMessaging.UpdateStatusFinished("Update All Items");
			ItemIcons.CacheAllIcons(ItemCache.AllItems);
			ItemCache.OnItemsChanged();
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
					Item.LoadFromId(item.GemmedId, true, "Refreshing", false, true);
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
                this.Invoke(new ReloadCharacterFromArmoryUpdateDelegate(this.ReloadCharacterFromCharacterProfilerUpdate), character, reload);
			}
			return character;
		}

        public delegate void ReloadCharacterFromArmoryUpdateDelegate(Character character, Character reload);
        public void ReloadCharacterFromArmoryUpdate(Character character, Character reload)
        {
            //load values for gear from armory into original character
            foreach (Character.CharacterSlot slot in Character.CharacterSlots)
            {
                character[slot] = reload[slot];
            }
            foreach (Character.CharacterSlot slot in Character.CharacterSlots)
            {
                character.SetEnchantBySlot(slot, reload.GetEnchantBySlot(slot));
            }
            character.AssignAllTalentsFromCharacter(reload);
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
                    Item newItem = Item.LoadFromId(id, false, "Character from Armory", false);
                    if (newItem != null)
                    {
                        // force load all gems also
                        Item gem;
                        gem = newItem.Gem1;
                        gem = newItem.Gem2;
                        gem = newItem.Gem3;
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
                this.EnsureItemsLoaded(characterProfilerChoice.Character.GetAllEquipedAndAvailableGearIds());
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
			LoadModel((string)comboBoxModel.SelectedItem);
		}

		private void comboBoxClass_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter && _character != null)
			{
				Character.Class = (Character.CharacterClass)Enum.Parse(typeof(Character.CharacterClass), comboBoxClass.Text);
			}
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter && _character != null)
			{
				Character.EnforceMetagemRequirements = checkBoxEnforceMetagemRequirements.Checked;
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
            }
        }

		private void FormMain_KeyDown(object sender, KeyEventArgs e)
		{
#if DEBUG
			if (e.KeyCode == (Keys)192 && e.Modifiers == Keys.Alt)
			{
				ToString(); //Breakpoint Here


				foreach (Item item in ItemCache.AllItems)
				{
					if (item.Sockets.Color1 == Item.ItemSlot.None && item.Gem1Id != 0) item.Gem1Id = 0;
					else if (item.Sockets.Color2 == Item.ItemSlot.None && item.Gem2Id != 0) item.Gem2Id = 0;
					else if (item.Sockets.Color3 == Item.ItemSlot.None && item.Gem3Id != 0) item.Gem3Id = 0;
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
            foreach (Character.CharacterSlot slot in Character.CharacterSlots)
            {
                character[slot] = reload[slot];
            }
            foreach (Character.CharacterSlot slot in Character.CharacterSlots)
            {
                character.SetEnchantBySlot(slot, reload.GetEnchantBySlot(slot));
            }
            character.AvailableItems = reload.AvailableItems;
            character.AssignAllTalentsFromCharacter(reload);
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
                EnsureItemsLoaded(characterProfilerCharacter.Character.GetAllEquipedAndAvailableGearIds());
            }
            else
            {
                StatusMessaging.UpdateStatusFinished("Update Item Cache");
                StatusMessaging.UpdateStatusFinished("Cache Item Icons");

            }
            return character;
        }


        //private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
		//{
		//    FormItemEditor itemEditor = new FormItemEditor(Character);
		//    itemEditor.ShowDialog(this);
		//    ItemCache.OnItemsChanged();
		//}

		//private void buffsToolStripMenuItem1_Click(object sender, EventArgs e)
		//{
		//    EditBuffs edit = new EditBuffs(Buff.AllBuffs);
		//    if (edit.ShowDialog() == DialogResult.OK)
		//    {
		//        //copy edited buffs to buff cache
		//    }
		//    edit.Dispose();
		//}

		//private void enchantsToolStripMenuItem1_Click(object sender, EventArgs e)
		//{
		//    EditEnchants edit = new EditEnchants(Enchant.AllEnchants);
		//    if (edit.ShowDialog() == DialogResult.OK)
		//    {
		//        //copy edited enchants to enchant cache.
		//    }
		//    edit.Dispose();
		//}
	}
}
