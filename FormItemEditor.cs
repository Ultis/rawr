using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rawr.Forms.Utilities;

namespace Rawr
{
	public partial class FormItemEditor : Form, IFormItemSelectionProvider
	{
		private bool _firstLoad = true;
        private bool _loadingItem = false;

		private FormItemSelection _formItemSelection;
		public FormItemSelection FormItemSelection
		{
			get
			{
				if (_formItemSelection == null || _formItemSelection.IsDisposed)
				{
					_formItemSelection = new FormItemSelection();
					_formItemSelection.Character = FormMain.Instance.FormItemSelection.Character;
					_formItemSelection.Items = ItemCache.RelevantItems;
				}
				return _formItemSelection;
			}
		}

		private ListViewItem _selectedItem;
        public ListViewItem SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				if (_selectedItem != null)
				{

					Item oldItem = _selectedItem.Tag as Item;
                    oldItem.InvalidateCachedData();
					_selectedItem.Text = oldItem.Name;
					oldItem.IdsChanged -= new EventHandler(Item_IdsChanged);
					string slot = oldItem.Slot.ToString();
					if (oldItem.Slot == Item.ItemSlot.Red || oldItem.Slot == Item.ItemSlot.Orange || oldItem.Slot == Item.ItemSlot.Yellow
						 || oldItem.Slot == Item.ItemSlot.Green || oldItem.Slot == Item.ItemSlot.Blue || oldItem.Slot == Item.ItemSlot.Purple
						 || oldItem.Slot == Item.ItemSlot.Prismatic || oldItem.Slot == Item.ItemSlot.Meta) slot = "Gems";
					_selectedItem.Group = listViewItems.Groups["listViewGroup" + slot];
					//_selectedItem.ImageKey = oldItem.IconPath;
					listViewItems.Sort();
				}

				_selectedItem = value;

				Item selectedItem = _selectedItem.Tag as Item;
                selectedItem.InvalidateCachedData();
                if (selectedItem.IsGem) ItemCache.InvalidateCachedStats();
				selectedItem.IdsChanged += new EventHandler(Item_IdsChanged);
				_equippedSlots = _character.GetEquippedSlots(selectedItem);
					
				textBoxName.DataBindings.Clear();
                textBoxSetName.DataBindings.Clear();
                textBoxIcon.DataBindings.Clear();
                textBoxNote.DataBindings.Clear();

				numericUpDownId.DataBindings.Clear();
				numericUpDownMin.DataBindings.Clear();
				numericUpDownMax.DataBindings.Clear();
				numericUpDownSpeed.DataBindings.Clear();
                numericUpDownBonus1.DataBindings.Clear();
                checkBoxUnique.DataBindings.Clear();
                comboBoxDamageType.DataBindings.Clear();
                comboBoxSlot.DataBindings.Clear();
                comboBoxQuality.DataBindings.Clear();
                comboBoxType.DataBindings.Clear();
				comboBoxSocket1.DataBindings.Clear();
				comboBoxSocket2.DataBindings.Clear();
				comboBoxSocket3.DataBindings.Clear();
				itemButtonGem1.DataBindings.Clear();
				itemButtonGem2.DataBindings.Clear();
				itemButtonGem3.DataBindings.Clear();


				if (selectedItem != null)
				{
                    _loadingItem = true;
                    textBoxName.DataBindings.Add("Text", selectedItem, "Name");
                    textBoxSetName.DataBindings.Add("Text", selectedItem, "SetName");
                    textBoxIcon.DataBindings.Add("Text", selectedItem, "IconPath");
					numericUpDownId.DataBindings.Add("Value", selectedItem, "Id");
					numericUpDownMin.DataBindings.Add("Value", selectedItem, "MinDamage");
					numericUpDownMax.DataBindings.Add("Value", selectedItem, "MaxDamage");
					numericUpDownSpeed.DataBindings.Add("Value", selectedItem, "Speed");

                    checkBoxUnique.DataBindings.Add("Checked", selectedItem, "Unique");
                    comboBoxQuality.DataBindings.Add("Text", selectedItem, "Quality");
                    comboBoxSlot.DataBindings.Add("Text", selectedItem, "SlotString");
					comboBoxType.DataBindings.Add("Text", selectedItem, "TypeString");
                    comboBoxDamageType.DataBindings.Add("Text", selectedItem, "DamageType");
                    comboBoxSocket1.DataBindings.Add("Text", selectedItem.Sockets, "Color1String");
					comboBoxSocket2.DataBindings.Add("Text", selectedItem.Sockets, "Color2String");
					comboBoxSocket3.DataBindings.Add("Text", selectedItem.Sockets, "Color3String");
					itemButtonGem1.DataBindings.Add("SelectedItemId", selectedItem, "Gem1Id");
					itemButtonGem2.DataBindings.Add("SelectedItemId", selectedItem, "Gem2Id");
					itemButtonGem3.DataBindings.Add("SelectedItemId", selectedItem, "Gem3Id");

                    textBoxSource.Text = selectedItem.LocationInfo.Description;
                    textBoxNote.DataBindings.Add("Text", selectedItem.LocationInfo, "Note");

                    propertyGridStats.SelectedObject = selectedItem.Stats;

                    string requiredClassesString = "";
                    if (selectedItem.RequiredClasses != null) requiredClassesString = selectedItem.RequiredClasses;
                    string[] requiredClasses = requiredClassesString.Split('|');
                    for (int i = 0; i < checkedListBoxRequiredClasses.Items.Count; i++)
                    {
                        checkedListBoxRequiredClasses.SetItemChecked(i, Array.IndexOf<string>(requiredClasses, (string)checkedListBoxRequiredClasses.Items[i]) >= 0);
                    }

                    var socketBonuses = selectedItem.Sockets.Stats.Values(x=> x > 0).GetEnumerator();
                    if (!socketBonuses.MoveNext())
                    {
                        comboBoxBonus1.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBoxBonus1.SelectedIndex = comboBoxBonus1.Items.IndexOf(Extensions.DisplayName(socketBonuses.Current.Key));
                        numericUpDownBonus1.Value = (decimal)socketBonuses.Current.Value;

                        for (int i = 0; i < numericUpDownBonus1.DataBindings.Count; i++)
                        {
                            numericUpDownBonus1.DataBindings[i].WriteValue();
                        }

/*
 // if there is ever more than one socket
                        if (!socketBonuses.MoveNext())
                        {
                            comboBoxBonus2.SelectedIndex = 0;
                        }
                        else
                        {

                            comboBoxBonus2.SelectedIndex = comboBoxBonus1.Items.IndexOf(Extensions.SpaceCamel(socketBonuses.Current.Key.Name));
                            numericUpDownBonus2.Value = (decimal)socketBonuses.Current.Value;
                        }
*/
                    }
                    _loadingItem = false;
				}
			}
		}

		void Item_IdsChanged(object sender, EventArgs e)
		{
			Item item = (sender as Item);
			foreach (Character.CharacterSlot slot in _equippedSlots)
				_character[slot] = item;
		}

		private Character.CharacterSlot[] _equippedSlots;

		private Character _character;
        public Character Character
        {
            get
            {
                return _character;
            }
            set
            {
                _character = value;
            }
        }

        public FormItemEditor(Character character)
		{
			InitializeComponent();
			_character = character;
			//listViewItems.SmallImageList = ItemIcons.SmallIcons;
            LoadItems();



            comboBoxBonus1.Tag = numericUpDownBonus1;
            comboBoxBonus1.Items.Add("None");
            comboBoxBonus1.Items.AddRange(Stats.StatNames);


 /*
           comboBoxBonus2.Tag = numericUpDownBonus2;
            comboBoxBonus2.Items.Add("None");
            comboBoxBonus2.Items.AddRange(Stats.StatNames);
*/

			ItemCache.Instance.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
			this.FormClosing += new FormClosingEventHandler(FormItemEditor_FormClosing);
		}

		public FormItemEditor(Character character, Item initialSelection)
		{
			InitializeComponent();
			_character = character;
			//listViewItems.SmallImageList = ItemIcons.SmallIcons;
			_loadingItem = true;
			textBoxFilter.Text = initialSelection.Name;
			_loadingItem = false;
			LoadItems();
			SelectItem(initialSelection, true);


			comboBoxBonus1.Tag = numericUpDownBonus1;
			comboBoxBonus1.Items.Add("None");
			comboBoxBonus1.Items.AddRange(Stats.StatNames);


			/*
					  comboBoxBonus2.Tag = numericUpDownBonus2;
					   comboBoxBonus2.Items.Add("None");
					   comboBoxBonus2.Items.AddRange(Stats.StatNames);
		   */

			ItemCache.Instance.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
			this.FormClosing += new FormClosingEventHandler(FormItemEditor_FormClosing);
		}

		void FormItemEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.textBoxName.Focus(); //Force data changes to be applied through databinding...
			this.textBoxIcon.Focus(); //databinding doesn't seem to set the new value until focus has left the control
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
		}

		private bool _changingItemCache = false;
		private void ItemCache_ItemsChanged(object sender, EventArgs e)
		{
			if (!_changingItemCache && SelectedItem != null)
			{
				Item selectedItem = SelectedItem.Tag as Item;
				//LoadItems();
				SelectItem(selectedItem, true);
			}
		}

		void comboBoxSocket_TextChanged(object sender, EventArgs e)
		{
			itemButtonGem1.CharacterSlot = comboBoxSocket1.Text == Item.ItemSlot.Meta.ToString() ? Character.CharacterSlot.Metas : Character.CharacterSlot.Gems;
			itemButtonGem2.CharacterSlot = comboBoxSocket2.Text == Item.ItemSlot.Meta.ToString() ? Character.CharacterSlot.Metas : Character.CharacterSlot.Gems;
			itemButtonGem3.CharacterSlot = comboBoxSocket3.Text == Item.ItemSlot.Meta.ToString() ? Character.CharacterSlot.Metas : Character.CharacterSlot.Gems;
		}

		private void LoadItems() { LoadItems(ItemCache.AllItems); }
		private void LoadItems(Item[] items)
		{
			listViewItems.Items.Clear();
			List<ListViewItem> itemsToAdd = new List<ListViewItem>();
			foreach (Item item in items)
			{
				if (string.IsNullOrEmpty(textBoxFilter.Text) || item.Name.ToLower().Contains(textBoxFilter.Text.ToLower()))
				{
					string slot = item.Slot.ToString();
					if (item.Slot == Item.ItemSlot.Red || item.Slot == Item.ItemSlot.Orange || item.Slot == Item.ItemSlot.Yellow
						 || item.Slot == Item.ItemSlot.Green || item.Slot == Item.ItemSlot.Blue || item.Slot == Item.ItemSlot.Purple
						 || item.Slot == Item.ItemSlot.Prismatic || item.Slot == Item.ItemSlot.Meta) slot = "Gems";
					ListViewItem lvi = new ListViewItem(item.Name, listViewItems.Groups["listViewGroup" + slot]);
					lvi.Tag = item;
					//lvi.ImageKey = EnsureIconPath(item.IconPath);
					itemsToAdd.Add(lvi);
				}
			}
			listViewItems.Items.AddRange(itemsToAdd.ToArray());
            listViewItems.Sort();
		}

		private string EnsureIconPath(string iconPath)
		{
			if (!ItemIcons.SmallIcons.Images.ContainsKey(iconPath))
			{
				try
				{
					/*imageListItems.Images.Add(iconPath, */ItemIcons.GetItemIcon(iconPath, true)/*)*/;
				}
				catch { }
			}
			return iconPath;
		}

		private void listViewItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listViewItems.SelectedIndices.Count > 0)
			{
				SelectedItem = listViewItems.SelectedItems[0];
                SelectedItem.EnsureVisible();
			}
		}

		private void FormItemEditor_Load(object sender, EventArgs e)
		{
            listViewItems.Sort();
            //force a paint, without this, the sorting doesn't render correctly 
            //until the user causes a paint event over the moved items (scrolling off the screen or clicking on the affected ones.
            //have to do the DoEvents to force it to process too otherwise the EnsureVisible won't work correctly in the activation event 
            //(I couldn't get it to won't work here at all)
            listViewItems.Invalidate(true);
            Application.DoEvents();
            if (listViewItems.Items.Count > 0 && listViewItems.SelectedIndices.Count == 0)
            {
                ListViewItem item = FindFirstItem();
                if (item != null)
                {
                    item.Selected = true;
                }
                else
                {
                    listViewItems.Items[0].Selected = true;
                }
            }
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			FormEnterId form = new FormEnterId();
			if (form.ShowDialog(this) == DialogResult.OK)
			{
				_changingItemCache = true;
				try
				{
					WebRequestWrapper.ResetFatalErrorIndicator();
                    int itemId = form.Value;
                    if (itemId > 0)
					    AddItemById(form.Value, form.UseArmory, form.UseWowhead);
                    else
                        AddItemByName(form.Name, form.UseArmory, form.UseWowhead);
				}
				finally
				{
					_changingItemCache = false;
				}
			}
            form.Dispose();
		}

        private void AddItemByName(string name, bool useArmory, bool useWowhead)
        {
            Item newItem = null;

            // try the armory (if requested)
            if (useArmory && !useWowhead)
            {
                MessageBox.Show("Unable to load from the Armory by name.  Please try Wowhead.", "Item not found.", MessageBoxButtons.OK);                
            }

            // try wowhead (if requested)
            if ((newItem == null) && useWowhead)
            {
                // make sure we don't get some bad input that is going to mess with our gem info passing
                if (!name.Contains("."))
                {
                    // need to add + where the spaces are
                    string wowhead_name = name.Replace(' ', '+');
                    // we can now pass it through the normal URI
                    newItem = Wowhead.GetItem(wowhead_name + ".0.0.0");
                    if (newItem != null) ItemCache.AddItem(newItem, true, true);
                }
            }

            if (newItem == null)
            {
                MessageBox.Show("Unable to load item: " + name + ".", "Item not found.", MessageBoxButtons.OK);
            }
            else
            {
                AddNewItemToListView(newItem);
            }
        }

		private void AddItemById(int id, bool useArmory, bool useWowhead) { AddItemsById(new int[] { id }, useArmory, useWowhead); }
        private void AddItemsById(int[] ids, bool useArmory, bool useWowhead)
		{
			foreach (int id in ids)
            {
                Item newItem = null;

                // try the armory (if requested)
                if (useArmory)
                {
                    newItem = Item.LoadFromId(id, true, "Manually Added", true);
                }

                // try wowhead (if requested)
                if ((newItem == null) && useWowhead)
                {
                    newItem = Wowhead.GetItem(id.ToString() + ".0.0.0");
                    if (newItem != null) ItemCache.AddItem(newItem, true, true);
                }

                if (newItem == null)
                {
                    if (MessageBox.Show("Unable to load item " + id.ToString() + ". Would you like to create the item blank and type in the values yourself?", "Item not found. Create Blank?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        newItem = new Item("New Item", Item.ItemQuality.Epic, Item.ItemType.None, id, "temp", Item.ItemSlot.Head, string.Empty, false, new Stats(), new Sockets(), 0, 0, 0, 0, 0, Item.ItemDamageType.Physical, 0f, string.Empty);
                        ItemCache.AddItem(newItem);
                    }
                }
                {
                    AddNewItemToListView(newItem);
                }
            }
		}

        private void AddNewItemToListView(Item newItem)
        {
            // show the item we just added (if we added one)
            if (newItem != null)
            {
                ListViewItem newLvi = new ListViewItem(newItem.Name, 0, listViewItems.Groups["listViewGroup" + newItem.Slot.ToString()]);
                newLvi.Tag = newItem;
                //newLvi.ImageKey = EnsureIconPath(newItem.IconPath);
                string slot = newItem.Slot.ToString();
                if (newItem.Slot == Item.ItemSlot.Red || newItem.Slot == Item.ItemSlot.Orange || newItem.Slot == Item.ItemSlot.Yellow
                     || newItem.Slot == Item.ItemSlot.Green || newItem.Slot == Item.ItemSlot.Blue || newItem.Slot == Item.ItemSlot.Purple
                     || newItem.Slot == Item.ItemSlot.Prismatic || newItem.Slot == Item.ItemSlot.Meta) slot = "Gems";
                newLvi.Group = listViewItems.Groups["listViewGroup" + slot];

                listViewItems.Items.Add(newLvi);
                listViewItems.Sort();
                listViewItems.SelectedIndices.Clear();
                newLvi.Selected = true;
                newLvi.EnsureVisible();

            }
        }

		private void buttonDelete_Click(object sender, EventArgs e)
		{
			
            if (listViewItems.SelectedItems.Count > 0)
            {
                Item item = listViewItems.SelectedItems[0].Tag as Item;
                if (MessageBox.Show("Are you sure you want to delete this instance of the " + item.Name, "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _changingItemCache = true;
                    ItemCache.DeleteItem(item);
                    _changingItemCache = false;
                    listViewItems.Items.Remove(listViewItems.SelectedItems[0]);
                    if (listViewItems.Items.Count > 0)
                    {
                        listViewItems.SelectedIndices.Add(0);
                    }
                }
            }
			
		}

		private void buttonFillSockets_Click(object sender, EventArgs e)
		{
			FormFillSockets form = new FormFillSockets();
			if (form.ShowDialog(this) == DialogResult.OK)
			{
				foreach (Item item in ItemCache.AllItems)
				{
					if (item.Sockets.Color1 != Item.ItemSlot.None && (!form.FillEmptySockets || item.Gem1Id == 0))
					{
						switch (item.Sockets.Color1)
						{
							case Item.ItemSlot.Red:
								item.Gem1 = form.GemRed;
								break;

							case Item.ItemSlot.Blue:
								item.Gem1 = form.GemBlue;
								break;

							case Item.ItemSlot.Yellow:
								item.Gem1 = form.GemYellow;
								break;

							case Item.ItemSlot.Meta:
								item.Gem1 = form.GemMeta;
								break;
						}
					}
					if (item.Sockets.Color2 != Item.ItemSlot.None && (!form.FillEmptySockets || item.Gem2Id == 0))
					{
						switch (item.Sockets.Color2)
						{
							case Item.ItemSlot.Red:
								item.Gem2 = form.GemRed;
								break;

							case Item.ItemSlot.Blue:
								item.Gem2 = form.GemBlue;
								break;

							case Item.ItemSlot.Yellow:
								item.Gem2 = form.GemYellow;
								break;

							case Item.ItemSlot.Meta:
								item.Gem2 = form.GemMeta;
								break;
						}
					}
					if (item.Sockets.Color3 != Item.ItemSlot.None && (!form.FillEmptySockets || item.Gem3Id == 0))
					{
						switch (item.Sockets.Color3)
						{
							case Item.ItemSlot.Red:
								item.Gem3 = form.GemRed;
								break;

							case Item.ItemSlot.Blue:
								item.Gem3 = form.GemBlue;
								break;

							case Item.ItemSlot.Yellow:
								item.Gem3 = form.GemYellow;
								break;

							case Item.ItemSlot.Meta:
								item.Gem3 = form.GemMeta;
								break;
						}
					}
				}
			}
            form.Dispose();
		}

		private void buttonDuplicate_Click(object sender, EventArgs e)
		{
			Item item = listViewItems.SelectedItems[0].Tag as Item;
			Item copy = new Item(item.Name, item.Quality, item.Type, item.Id, item.IconPath, item.Slot, item.SetName, item.Unique, item.Stats.Clone(),
				item.Sockets.Clone(), 0, 0, 0, item.MinDamage, item.MaxDamage, item.DamageType, item.Speed, item.RequiredClasses);
			_changingItemCache = true;
			ItemCache.AddItem(copy, false, true);
			_changingItemCache = false;

			ListViewItem newLvi = new ListViewItem(copy.Name, 0, listViewItems.Groups["listViewGroup" + copy.Slot.ToString()]);
			newLvi.Tag = copy;
			//newLvi.ImageKey = EnsureIconPath(copy.IconPath);
			string slot = copy.Slot.ToString();
			if (copy.Slot == Item.ItemSlot.Red || copy.Slot == Item.ItemSlot.Orange || copy.Slot == Item.ItemSlot.Yellow
				 || copy.Slot == Item.ItemSlot.Green || copy.Slot == Item.ItemSlot.Blue || copy.Slot == Item.ItemSlot.Purple
				 || copy.Slot == Item.ItemSlot.Prismatic || copy.Slot == Item.ItemSlot.Meta) slot = "Gems";
			newLvi.Group = listViewItems.Groups["listViewGroup" + slot];

			listViewItems.Items.Add(newLvi);
			newLvi.Selected = true;
			listViewItems.Sort();
			newLvi.EnsureVisible();
		}

		internal void SelectItem(Item item, bool force) 
		{
			bool found = false;
			foreach (ListViewItem lvi in listViewItems.Items)
				if (lvi.Tag == item)
				{
					lvi.Selected = true;
					lvi.EnsureVisible();
					found = true;
				}
			if (!found && force)
			{
				string filter = textBoxFilter.Text;
				textBoxFilter.Text = "";
				foreach (ListViewItem lvi in listViewItems.Items)
					if (lvi.Tag == item)
					{
						lvi.Selected = true;
						lvi.EnsureVisible();
						found = true;
					}
                if (!found)
                {
                    textBoxFilter.Text = filter;
                }
			}
		}

		private void textBoxFilter_TextChanged(object sender, EventArgs e)
		{
			if (!_loadingItem)
			{
				Item selectedItem = null;
				if (SelectedItem != null) selectedItem = SelectedItem.Tag as Item;
				LoadItems();
				if (selectedItem != null) SelectItem(selectedItem, false);
			}
		}

        private void comboBoxBonus_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item selectedItem = SelectedItem.Tag as Item;
            UpdateStatDataBindings(sender as ComboBox, selectedItem.Sockets.Stats, true);
        }

        private void comboBoxStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item selectedItem = SelectedItem.Tag as Item;
            UpdateStatDataBindings(sender as ComboBox, selectedItem.Stats, false);
        }

        private void UpdateStatDataBindings(ComboBox combo, Stats boundStats, bool clearExistingValue)
        {
            if (null != combo)
            {
                NumericUpDown ud = (combo.Tag as NumericUpDown);
                if (ud != null)
                {
                    decimal value = ud.Value;
                    if(clearExistingValue)
                    {
                        ud.Value = 0;

                        for (int i = 0; i < ud.DataBindings.Count; i++)
                        {
                            ud.DataBindings[i].WriteValue();
                        }
                        ud.DataBindings.Clear();
                        ud.Enabled = (combo.SelectedIndex > 0);
                    }
                    
                    if (ud.Enabled)
                    {
                        string v = Extensions.UnDisplayName(combo.Items[combo.SelectedIndex].ToString()).Name;
                        ud.DataBindings.Add("Value", boundStats, v);
                       
                        if(clearExistingValue)
                        {
                            ud.Value = value;
                            for (int i = 0; i < ud.DataBindings.Count; i++)
                            {
                                ud.DataBindings[i].WriteValue();
                            }
                        }
                    }
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (SelectedItem != null)
            {
                Item selectedItem = SelectedItem.Tag as Item;
                selectedItem.InvalidateCachedData();
            }
        }

        private void FormItemEditor_Activated(object sender, EventArgs e)
        {
            if ( _firstLoad && listViewItems.SelectedIndices.Count > 0)
            {
                //EnsureVisible works correctly in Load, except that the list is then reordered by the sort, and doesn't shift
                //the scroll correctly if redone in Load, it only seemed to work in another event handle.
                listViewItems.Items[listViewItems.SelectedIndices[0]].EnsureVisible();
            }
        }

        // Being lazy with this since its a known case and probably won't change
        private ListViewItem FindFirstItem()
        {
            ListViewItem ret = null;
            if (listViewItems.Groups.Count > 0 && listViewItems.Groups[0].Items.Count > 0)
            {
                ret = listViewItems.Groups[0].Items[0];
                for (int i = 1; i < listViewItems.Groups[0].Items.Count; i++)
                {
                    if (string.Compare(ret.Text, listViewItems.Groups[0].Items[i].Text) > 0)
                    {
                        ret = listViewItems.Groups[0].Items[i];
                    }
                }
            }
            return ret;
        }

		private void buttonDeleteDuplicates_Click(object sender, EventArgs e)
		{
			if (listViewItems.SelectedItems.Count > 0)
			{
				Item itemToSave = listViewItems.SelectedItems[0].Tag as Item;
				if (MessageBox.Show("Are you sure you want to delete all instances of " + itemToSave.Name + " except the selected one?", "Confirm Delete Duplicates", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					_changingItemCache = true;
					Cursor = Cursors.WaitCursor;
					List<Item> itemsToDelete = new List<Item>(ItemCache.Instance.FindAllItemsById(itemToSave.Id));
					Item itemUngemmed = ItemCache.FindItemById(itemToSave.Id.ToString() + ".0.0.0", false, false);
					if (itemUngemmed != null) itemsToDelete.Add(itemUngemmed);
					if (itemsToDelete.Contains(itemToSave)) itemsToDelete.Remove(itemToSave);
					foreach (Item itemToDelete in itemsToDelete)
						ItemCache.DeleteItem(itemToDelete);
					foreach (ListViewItem lvi in listViewItems.Items)
						if (itemsToDelete.Contains(lvi.Tag as Item))
							listViewItems.Items.Remove(lvi);
					Cursor = Cursors.Default;
					_changingItemCache = false;
				}
			}
        }

        private void checkedListBoxRequiredClasses_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_loadingItem)
            {
                Item selectedItem = _selectedItem.Tag as Item;
                List<string> requiredClasses = new List<string>();
                foreach (string requiredClass in checkedListBoxRequiredClasses.CheckedItems)
                {
                    requiredClasses.Add(requiredClass);
                }
                if (e.NewValue == CheckState.Checked)
                {
                    requiredClasses.Add((string)checkedListBoxRequiredClasses.Items[e.Index]);
                }
                else
                {
                    requiredClasses.Remove((string)checkedListBoxRequiredClasses.Items[e.Index]);
                }
                if (selectedItem != null)
                {
                    selectedItem.RequiredClasses = string.Join("|", requiredClasses.ToArray());
                }
            }
        }
    }
}
