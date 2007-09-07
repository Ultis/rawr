using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Rawr
{
	public partial class FormMain : Form
	{
		private FormSplash _spash = new FormSplash();
		private string _characterPath = "";
		private bool _unsavedChanges = false;
		private CalculatedStats _calculatedStats = null;

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
					itemComparison1.Character = itemButtonBack.Character = itemButtonChest.Character = itemButtonFeet.Character =
						itemButtonFinger1.Character = itemButtonFinger2.Character = itemButtonHands.Character =
						itemButtonHead.Character = itemButtonIdol.Character = itemButtonLegs.Character =
						itemButtonNeck.Character = itemButtonShirt.Character = itemButtonShoulders.Character =
						itemButtonTabard.Character = itemButtonTrinket1.Character = itemButtonTrinket2.Character =
						itemButtonWaist.Character = itemButtonWeapon.Character = itemButtonWrist.Character = _character;
					//Ahhh ahhh ahhh ahhh ahhh ahhh ahhh ahhh...

					_character.ItemsChanged += new EventHandler(_character_ItemsChanged);
					_loadingCharacter = true;

					checkBoxBuffsFoodAgility.Checked = Character.Buffs.AgilityFood;
					checkBoxBuffsKings.Checked = Character.Buffs.BlessingOfKings;
					checkBoxBuffsPact.Checked = Character.Buffs.BloodPact;
					checkBoxBuffsShout.Checked = Character.Buffs.CommandingShout;
					checkBoxBuffsDevo.Checked = Character.Buffs.DevotionAura;
					checkBoxBuffsIronskin.Checked = Character.Buffs.ElixirOfIronskin;
					checkBoxBuffsAgilityElixir.Checked = Character.Buffs.ElixirOfMajorAgility;
					checkBoxBuffsMajorDefense.Checked = Character.Buffs.ElixirOfMajorDefense;
					checkBoxBuffsMastery.Checked = Character.Buffs.ElixirOfMastery;
					checkBoxBuffsMajorFortitude.Checked = Character.Buffs.ElixirOfMajorFortitude;
					checkBoxBuffsFlask.Checked = Character.Buffs.FlaskOfFortification;
					checkBoxBuffsGrace.Checked = Character.Buffs.GraceOfAirTotem;
					checkBoxBuffsImpPact.Checked = Character.Buffs.ImprovedBloodPact;
					checkBoxBuffsImpShout.Checked = Character.Buffs.ImprovedCommandingShout;
					checkBoxBuffsImpDevo.Checked = Character.Buffs.ImprovedDevotionAura;
					checkBoxBuffsImpGrace.Checked = Character.Buffs.ImprovedGraceOfAirTotem;
					checkBoxBuffsImpMark.Checked = Character.Buffs.ImprovedMarkOfTheWild;
					checkBoxBuffsImpFort.Checked = Character.Buffs.ImprovedPowerWordFortitude;
					checkBoxBuffsInsectSwarm.Checked = Character.Buffs.InsectSwarm;
					checkBoxBuffsMark.Checked = Character.Buffs.MarkOfTheWild;
					checkBoxBuffsFort.Checked = Character.Buffs.PowerWordFortitude;
					checkBoxBuffsScorpidSting.Checked = Character.Buffs.ScorpidSting;
					checkBoxBuffsFoodStamina.Checked = Character.Buffs.StaminaFood;
					checkBoxBuffsMalorne.Checked = Character.Buffs.Malorne4PieceBonus;
					checkBoxBuffsScrollOfProtection.Checked = Character.Buffs.ScrollOfProtection;
					checkBoxBuffsScrollOfAgility.Checked = Character.Buffs.ScrollOfAgility;
					checkBoxBuffsShadowEmbrace.Checked = Character.Buffs.ShadowEmbrace;
					checkBoxBuffsGladiatorResilience.Checked = Character.Buffs.GladiatorResilience;

					comboBoxEnchantBack.SelectedItem = Character.BackEnchant;
					comboBoxEnchantChest.SelectedItem = Character.ChestEnchant;
					comboBoxEnchantFeet.SelectedItem = Character.FeetEnchant;
					comboBoxEnchantHands.SelectedItem = Character.HandsEnchant;
					comboBoxEnchantHead.SelectedItem = Character.HeadEnchant;
					comboBoxEnchantLegs.SelectedItem = Character.LegsEnchant;
					comboBoxEnchantShoulders.SelectedItem = Character.ShouldersEnchant;
					comboBoxEnchantWeapon.SelectedItem = Character.WeaponEnchant;
					comboBoxEnchantWrists.SelectedItem = Character.WristEnchant;

					textBoxName.Text = Character.Name;
					textBoxRealm.Text = Character.Realm;
					radioButtonRegionUS.Checked = Character.Region == Character.CharacterRegion.US;
					radioButtonRegionEU.Checked = Character.Region == Character.CharacterRegion.EU;
					radioButtonRaceNightElf.Checked = Character.Race == Character.CharacterRace.NightElf;
					radioButtonRaceTauren.Checked = Character.Race == Character.CharacterRace.Tauren;

					_loadingCharacter = false;
					_character_ItemsChanged(null, null);
				}
			}
		}

		void _character_ItemsChanged(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			_unsavedChanges = true;
			//and the clouds above move closer / looking so dissatisfied
			Calculations.ClearCache();
			CalculatedStats calcs = Calculations.CalculateStats(Character);
			_calculatedStats = calcs;

			//ItemCalculations itemCalc = Calculations.GetItemCalculations(Character.Finger1, calcStats);
			//MessageBox.Show(string.Format("{0}H, {1}M, {2}S", itemCalc.HossPoints, itemCalc.MitigationPoints, itemCalc.SurvivalPoints));
			LoadComparisonData();

			labelHealth.Text = calcs.BasicStats.Health.ToString();
			labelArmor.Text = calcs.BasicStats.Armor.ToString();
			labelAgility.Text = calcs.BasicStats.Agility.ToString();
			labelStamina.Text = calcs.BasicStats.Stamina.ToString();
			labelDefenseRating.Text = calcs.BasicStats.DefenseRating.ToString();
			if (radioButtonRaceNightElf.Checked)
				labelDodgeRating.Text = (calcs.BasicStats.DodgeRating - 59).ToString();
			else
				labelDodgeRating.Text = (calcs.BasicStats.DodgeRating - 40).ToString();
			labelResilience.Text = calcs.BasicStats.Resilience.ToString();

			labelDodge.Text = calcs.Dodge.ToString() + "%";
			labelMiss.Text = calcs.Miss.ToString() + "%";
			labelMitigation.Text = calcs.Mitigation.ToString() + "%";
			labelDodgePlusMiss.Text = calcs.DodgePlusMiss.ToString() + "%";
			labelTotalMitigation.Text = calcs.TotalMitigation.ToString() + "%";
			labelDamageTaken.Text = calcs.DamageTaken.ToString() + "%";
			labelCritReduction.Text = (2.6f - calcs.CritReduction).ToString() + "%";
			labelOverallPoints.Text = calcs.OverallPoints.ToString();
			labelMitigationPoints.Text = calcs.MitigationPoints.ToString();
			labelSurvivalPoints.Text = calcs.SurvivalPoints.ToString();

			
			this.Cursor = Cursors.Default;
			//and the ground below grew colder / as they put you down inside
		}

		public FormMain()
		{
			_spash.Show();
			Application.DoEvents();
			InitializeComponent();
			this.Shown += new EventHandler(FormMain_Shown);
			ItemCache.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
			ItemCache_ItemsChanged(null, null);

			comboBoxEnchantBack.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Back).ToArray());
			comboBoxEnchantChest.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Chest).ToArray());
			comboBoxEnchantFeet.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Feet).ToArray());
			comboBoxEnchantHands.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Hands).ToArray());
			comboBoxEnchantHead.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Head).ToArray());
			comboBoxEnchantLegs.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Legs).ToArray());
			comboBoxEnchantShoulders.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Shoulders).ToArray());
			comboBoxEnchantWeapon.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Weapon).ToArray());
			comboBoxEnchantWrists.Items.AddRange(Enchant.FindEnchants(Item.ItemSlot.Wrist).ToArray());

			sortToolStripMenuItem_Click(overallToolStripMenuItem, EventArgs.Empty);
			slotToolStripMenuItem_Click(headToolStripMenuItem, EventArgs.Empty);
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
			Item[] items = ItemCache.GetItemsArray();
			ItemIcons.CacheAllIcons(items);
			itemComparison1.Items = items;
			itemButtonBack.Items = itemButtonChest.Items = itemButtonFeet.Items = itemButtonFinger1.Items =
					itemButtonFinger2.Items = itemButtonHands.Items = itemButtonHead.Items = itemButtonIdol.Items =
					itemButtonLegs.Items = itemButtonNeck.Items = itemButtonShirt.Items = itemButtonShoulders.Items =
					itemButtonTabard.Items = itemButtonTrinket1.Items = itemButtonTrinket2.Items = itemButtonWaist.Items =
					itemButtonWeapon.Items = itemButtonWrist.Items = ItemCache.GetItemsArray();
			this.Cursor = Cursors.Default;
		}

		private void editItemsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormItemEditor itemEditor = new FormItemEditor();
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
				Character = new Character();
				_characterPath = string.Empty;
				_unsavedChanges = false;
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
					this.Cursor = Cursors.WaitCursor;
					Character = Character.Load(dialog.FileName);
					_characterPath = dialog.FileName;
					_unsavedChanges = false;
					this.Cursor = Cursors.Default;
				}
			}
		}

		private void loadFromArmoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			///Code to Remove Dupes. Run until it doesn't remove any.
			//List<Item> itemsToRemove = new List<Item>();
			//foreach (Item itemA in ItemCache.Items)
			//    foreach (Item itemB in ItemCache.Items)
			//        if (itemA != itemB && itemA.GemmedId == itemB.GemmedId)
			//            itemsToRemove.Add(itemB);
			//List<string> idsRemoved = new List<string>();
			//foreach (Item item in itemsToRemove)
			//    if (ItemCache.Items.Contains(item) && !idsRemoved.Contains(item.GemmedId))
			//    {
			//        idsRemoved.Add(item.GemmedId);
			//        ItemCache.Items.Remove(item);
			//    }

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
					//Character = Character.LoadFromArmory(form.ArmoryRegion, form.Realm, form.CharacterName);
					Character = Armory.GetCharacter(form.ArmoryRegion, form.Realm, form.CharacterName);
					_characterPath = string.Empty;
					_unsavedChanges = true;
					this.Cursor = Cursors.Default;
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
				this.Cursor = Cursors.Default;
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
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
						break;
					case DialogResult.No:
						return true;
						break;
					default:
						return false;
						break;
				}
			}
			else
				return true;
		}
		#endregion

		private bool _loadingCharacter = false;
		private void BuffsControl_CheckedChanged(object sender, EventArgs e)
		{
			//If I was in World War II, they'd call me Spitfire!
			checkBoxBuffsImpDevo.Enabled = checkBoxBuffsDevo.Checked;
			checkBoxBuffsImpFort.Enabled = checkBoxBuffsFort.Checked;
			checkBoxBuffsImpMark.Enabled = checkBoxBuffsMark.Checked;
			checkBoxBuffsImpPact.Enabled = checkBoxBuffsPact.Checked;
			checkBoxBuffsImpShout.Enabled = checkBoxBuffsShout.Checked;
			checkBoxBuffsImpGrace.Enabled = checkBoxBuffsGrace.Checked;
			checkBoxBuffsFoodAgility.Enabled = !checkBoxBuffsFoodStamina.Checked;
			checkBoxBuffsFoodStamina.Enabled = !checkBoxBuffsFoodAgility.Checked;
			checkBoxBuffsAgilityElixir.Enabled = !checkBoxBuffsFlask.Checked && !checkBoxBuffsMastery.Checked;
			checkBoxBuffsMastery.Enabled = !checkBoxBuffsFlask.Checked && !checkBoxBuffsAgilityElixir.Checked;
			checkBoxBuffsMajorDefense.Enabled = !checkBoxBuffsFlask.Checked && !checkBoxBuffsIronskin.Checked && !checkBoxBuffsMajorFortitude.Checked;
			checkBoxBuffsIronskin.Enabled = !checkBoxBuffsFlask.Checked && !checkBoxBuffsMajorDefense.Checked && !checkBoxBuffsMajorFortitude.Checked;
			checkBoxBuffsMajorFortitude.Enabled = !checkBoxBuffsFlask.Checked && !checkBoxBuffsMajorDefense.Checked && !checkBoxBuffsIronskin.Checked;
			checkBoxBuffsFlask.Enabled = !checkBoxBuffsAgilityElixir.Checked && !checkBoxBuffsMajorDefense.Checked && !checkBoxBuffsIronskin.Checked && !checkBoxBuffsMastery.Checked && !checkBoxBuffsMajorFortitude.Checked;

			if (!_loadingCharacter)
			{
				Character.Buffs.AgilityFood = checkBoxBuffsFoodAgility.Checked && checkBoxBuffsFoodAgility.Enabled;
				Character.Buffs.BlessingOfKings = checkBoxBuffsKings.Checked && checkBoxBuffsKings.Enabled;
				Character.Buffs.BloodPact = checkBoxBuffsPact.Checked && checkBoxBuffsPact.Enabled;
				Character.Buffs.CommandingShout = checkBoxBuffsShout.Checked && checkBoxBuffsShout.Enabled;
				Character.Buffs.DevotionAura = checkBoxBuffsDevo.Checked && checkBoxBuffsDevo.Enabled;
				Character.Buffs.ElixirOfIronskin = checkBoxBuffsIronskin.Checked && checkBoxBuffsIronskin.Enabled;
				Character.Buffs.ElixirOfMastery = checkBoxBuffsMastery.Checked && checkBoxBuffsMastery.Enabled;
				Character.Buffs.ElixirOfMajorAgility = checkBoxBuffsAgilityElixir.Checked && checkBoxBuffsAgilityElixir.Enabled;
				Character.Buffs.ElixirOfMajorDefense = checkBoxBuffsMajorDefense.Checked && checkBoxBuffsMajorDefense.Enabled;
				Character.Buffs.ElixirOfMajorFortitude = checkBoxBuffsMajorFortitude.Checked && checkBoxBuffsMajorFortitude.Enabled;
				Character.Buffs.FlaskOfFortification = checkBoxBuffsFlask.Checked && checkBoxBuffsFlask.Enabled;
				Character.Buffs.GraceOfAirTotem = checkBoxBuffsGrace.Checked && checkBoxBuffsGrace.Enabled;
				Character.Buffs.ImprovedBloodPact = checkBoxBuffsImpPact.Checked && checkBoxBuffsImpPact.Enabled;
				Character.Buffs.ImprovedCommandingShout = checkBoxBuffsImpShout.Checked && checkBoxBuffsImpShout.Enabled;
				Character.Buffs.ImprovedDevotionAura = checkBoxBuffsImpDevo.Checked && checkBoxBuffsImpDevo.Enabled;
				Character.Buffs.ImprovedGraceOfAirTotem = checkBoxBuffsImpGrace.Checked && checkBoxBuffsImpGrace.Enabled;
				Character.Buffs.ImprovedMarkOfTheWild = checkBoxBuffsImpMark.Checked && checkBoxBuffsImpMark.Enabled;
				Character.Buffs.ImprovedPowerWordFortitude = checkBoxBuffsImpFort.Checked && checkBoxBuffsImpFort.Enabled;
				Character.Buffs.InsectSwarm = checkBoxBuffsInsectSwarm.Checked && checkBoxBuffsInsectSwarm.Enabled;
				Character.Buffs.MarkOfTheWild = checkBoxBuffsMark.Checked && checkBoxBuffsMark.Enabled;
				Character.Buffs.PowerWordFortitude = checkBoxBuffsFort.Checked && checkBoxBuffsFort.Enabled;
				Character.Buffs.ScorpidSting = checkBoxBuffsScorpidSting.Checked && checkBoxBuffsScorpidSting.Enabled;
				Character.Buffs.StaminaFood = checkBoxBuffsFoodStamina.Checked && checkBoxBuffsFoodStamina.Enabled;
				Character.Buffs.Malorne4PieceBonus = checkBoxBuffsMalorne.Checked && checkBoxBuffsMalorne.Enabled;
				Character.Buffs.ScrollOfProtection = checkBoxBuffsScrollOfProtection.Checked && checkBoxBuffsScrollOfProtection.Enabled;
				Character.Buffs.ScrollOfAgility = checkBoxBuffsScrollOfAgility.Checked && checkBoxBuffsScrollOfAgility.Enabled;
				Character.Buffs.ShadowEmbrace = checkBoxBuffsShadowEmbrace.Checked && checkBoxBuffsShadowEmbrace.Enabled;
				Character.Buffs.GladiatorResilience = checkBoxBuffsGladiatorResilience.Checked && checkBoxBuffsGladiatorResilience.Enabled;
				_character_ItemsChanged(null, null);
			}
		}

		private void comboBoxEnchant_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter)
			{   //If I was in World War II, they'd call me S-
				Character.BackEnchant = comboBoxEnchantBack.SelectedItem as Enchant;
				Character.ChestEnchant = comboBoxEnchantChest.SelectedItem as Enchant;
				Character.FeetEnchant = comboBoxEnchantFeet.SelectedItem as Enchant;
				Character.HandsEnchant = comboBoxEnchantHands.SelectedItem as Enchant;
				Character.HeadEnchant = comboBoxEnchantHead.SelectedItem as Enchant;
				Character.LegsEnchant = comboBoxEnchantLegs.SelectedItem as Enchant;
				Character.ShouldersEnchant = comboBoxEnchantShoulders.SelectedItem as Enchant;
				Character.WeaponEnchant = comboBoxEnchantWeapon.SelectedItem as Enchant;
				Character.WristEnchant = comboBoxEnchantWrists.SelectedItem as Enchant;
				_character_ItemsChanged(null, null);
			}   //...Fire!
		}

		private void radioButtonRace_CheckedChanged(object sender, EventArgs e)
		{
			if (!_loadingCharacter)
			{
				Character.Race = radioButtonRaceNightElf.Checked ? Character.CharacterRace.NightElf : Character.CharacterRace.Tauren;
				_character_ItemsChanged(null, null);
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

		private void trackBarValueOfArmor_Scroll(object sender, EventArgs e)
		{
			//Calculations.ValueOfArmor = ((float)trackBarValueOfArmor.Value) / 100f;
			_character_ItemsChanged(null, null);
		}

		private void groupBox3_Enter(object sender, EventArgs e)
		{

		}

		private void tabPageBuffs_Click(object sender, EventArgs e)
		{

		}

		private void copyCharacterStatsToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string stats = string.Format("Character:\t\t{18}@{19}-{20}\r\nRace:\t\t{0}\r\nHealth:\t\t{1}\r\nArmor:\t\t{2}\r\nAgility:\t\t{3}\r\nStamina:\t\t{4}\r\nDefense Rating:\t{5}\r\nDodge Rating:\t{6}\r\nResilience:\t{7}\r\nDodge:\t\t{8}\r\nMiss:\t\t{9}\r\nArmor Mitigation:\t{10}\r\nDodge+Miss:\t{11}\r\nTotal Mitigation:\t{12}\r\nDamage Taken:\t{13}\r\nChance to be Crit:\t{14}\r\nOverall Points:\t{15}\r\nMitigation Points:\t{16}\r\nSurvival Points:\t{17}",
				Character.Race, labelHealth.Text, labelArmor.Text, labelAgility.Text, labelStamina.Text, labelDefenseRating.Text, 
				labelDodgeRating.Text, labelResilience.Text, labelDodge.Text, labelMiss.Text, labelMitigation.Text, 
				labelDodgePlusMiss.Text, labelTotalMitigation.Text, labelDamageTaken.Text, labelCritReduction.Text, 
				labelOverallPoints.Text, labelMitigationPoints.Text, labelSurvivalPoints.Text, textBoxName.Text,
				Character.Region.ToString(), textBoxRealm.Text);
			Clipboard.SetText(stats, TextDataFormat.Text);
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
							itemComparison1.LoadBuffs(_calculatedStats);
							break;
					}
				}
			}
		}

		private void sortToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			foreach (ToolStripItem item in toolStripDropDownButtonSort.DropDownItems)
			{
				if (item is ToolStripMenuItem)
				{
					(item as ToolStripMenuItem).Checked = item == sender;
					if ((item as ToolStripMenuItem).Checked)
					{
						toolStripDropDownButtonSort.Text = item.Text;
					}
				}
			}
			itemComparison1.Sort = (ComparisonGraph.ComparisonSort)Enum.Parse(typeof(ComparisonGraph.ComparisonSort), toolStripDropDownButtonSort.Text);
			this.Cursor = Cursors.Default;
		}

		private void tabPageStats_Click(object sender, EventArgs e)
		{

		}

		private void loadPossibleUpgradesFromArmoryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			_spash = new FormSplash();
			_spash.Show();
			Application.DoEvents();
			Armory.LoadUpgradesFromArmory(Character);
			_spash.Hide();
			_spash.Dispose();
			this.Cursor = Cursors.Default;
		}
	}
}