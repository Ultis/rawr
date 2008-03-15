using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class FormItemEditor : Form
	{
		private ListViewItem _selectedItem;

		public ListViewItem SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				if (_selectedItem != null)
				{

					Item oldItem = _selectedItem.Tag as Item;
					_selectedItem.Text = oldItem.Name;
					oldItem.IdsChanged -= new EventHandler(Item_IdsChanged);
					string slot = oldItem.Slot.ToString();
					if (oldItem.Slot == Item.ItemSlot.Red || oldItem.Slot == Item.ItemSlot.Orange || oldItem.Slot == Item.ItemSlot.Yellow
						 || oldItem.Slot == Item.ItemSlot.Green || oldItem.Slot == Item.ItemSlot.Blue || oldItem.Slot == Item.ItemSlot.Purple
						 || oldItem.Slot == Item.ItemSlot.Prismatic || oldItem.Slot == Item.ItemSlot.Meta) slot = "Gems";
					_selectedItem.Group = listViewItems.Groups["listViewGroup" + slot];
					_selectedItem.ImageKey = oldItem.IconPath;
					listViewItems.Sort();
				}

				_selectedItem = value;

				Item selectedItem = _selectedItem.Tag as Item;
                selectedItem.InvalidateCachedData();
                if (selectedItem.IsGem) ItemCache.InvalidateCachedStats();
				selectedItem.IdsChanged += new EventHandler(Item_IdsChanged);
				_equippedSlots = _character.GetEquippedSlots(selectedItem);
					
				textBoxName.DataBindings.Clear();
				textBoxIcon.DataBindings.Clear();

				numericUpDownId.DataBindings.Clear();
				numericUpDownMin.DataBindings.Clear();
				numericUpDownMax.DataBindings.Clear();
				numericUpDownSpeed.DataBindings.Clear();
                numericUpDownBonus1.DataBindings.Clear();
				comboBoxSlot.DataBindings.Clear();
				comboBoxType.DataBindings.Clear();
				comboBoxSocket1.DataBindings.Clear();
				comboBoxSocket2.DataBindings.Clear();
				comboBoxSocket3.DataBindings.Clear();
				itemButtonGem1.DataBindings.Clear();
				itemButtonGem2.DataBindings.Clear();
				itemButtonGem3.DataBindings.Clear();


				if (selectedItem != null)
				{
					textBoxName.DataBindings.Add("Text", selectedItem, "Name");
					textBoxIcon.DataBindings.Add("Text", selectedItem, "IconPath");
					numericUpDownId.DataBindings.Add("Value", selectedItem, "Id");
					numericUpDownMin.DataBindings.Add("Value", selectedItem, "MinDamage");
					numericUpDownMax.DataBindings.Add("Value", selectedItem, "MaxDamage");
					numericUpDownSpeed.DataBindings.Add("Value", selectedItem, "Speed");

					comboBoxSlot.DataBindings.Add("Text", selectedItem, "SlotString");
					comboBoxType.DataBindings.Add("Text", selectedItem, "TypeString");
					comboBoxSocket1.DataBindings.Add("Text", selectedItem.Sockets, "Color1String");
					comboBoxSocket2.DataBindings.Add("Text", selectedItem.Sockets, "Color2String");
					comboBoxSocket3.DataBindings.Add("Text", selectedItem.Sockets, "Color3String");
					itemButtonGem1.DataBindings.Add("SelectedItemId", selectedItem, "Gem1Id");
					itemButtonGem2.DataBindings.Add("SelectedItemId", selectedItem, "Gem2Id");
					itemButtonGem3.DataBindings.Add("SelectedItemId", selectedItem, "Gem3Id");

                    propertyGridStats.SelectedObject = selectedItem.Stats;

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
		public FormItemEditor(Character character)
		{
			InitializeComponent();
			_character = character;
            LoadItems();



            comboBoxBonus1.Tag = numericUpDownBonus1;
            comboBoxBonus1.Items.Add("None");
            comboBoxBonus1.Items.AddRange(Stats.StatNames);


 /*
           comboBoxBonus2.Tag = numericUpDownBonus2;
            comboBoxBonus2.Items.Add("None");
            comboBoxBonus2.Items.AddRange(Stats.StatNames);
*/

			ItemCache.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
			this.FormClosing += new FormClosingEventHandler(FormItemEditor_FormClosing);
		}

		void FormItemEditor_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.textBoxName.Focus(); //Force data changes to be applied through databinding...
			this.textBoxIcon.Focus(); //databinding doesn't seem to set the new value until focus has left the control
		}

		private bool _changingItemCache = false;
		private void ItemCache_ItemsChanged(object sender, EventArgs e)
		{
			if (!_changingItemCache && SelectedItem != null)
			{
				Item selectedItem = SelectedItem.Tag as Item;
				LoadItems();
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
					lvi.ImageKey = EnsureIconPath(item.IconPath);
					itemsToAdd.Add(lvi);
				}
			}
			listViewItems.Items.AddRange(itemsToAdd.ToArray());
			if (listViewItems.Items.Count > 0) listViewItems.SelectedIndices.Add(0);
		}

		private string EnsureIconPath(string iconPath)
		{
			if (!imageListItems.Images.ContainsKey(iconPath))
			{
				try
				{
					imageListItems.Images.Add(iconPath, ItemIcons.GetItemIcon(iconPath, true));
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
			}
		}

		private void FormItemEditor_Load(object sender, EventArgs e)
		{
			listViewItems.Sort();
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			FormEnterId form = new FormEnterId();
			if (form.ShowDialog() == DialogResult.OK)
			{
				_changingItemCache = true;
				try
				{
					WebRequestWrapper.ResetFatalErrorIndicator();
					AddItemById(form.Value);
				}
				finally
				{
					_changingItemCache = false;
				}
			}
		}

		private void AddItemById(int id) { AddItemsById(new int[] { id }); }
		private void AddItemsById(int[] ids)
		{
			foreach (int id in ids)
			{
				Item newItem = Item.LoadFromId(id, true, "Manually Added");
				if (newItem == null)
				{
					if (MessageBox.Show("Unable to load item " + id.ToString() + ". Would you like to create the item blank and type in the values yourself?", "Item not found. Create Blank?", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						newItem = new Item("New Item", Item.ItemQuality.Epic, Item.ItemType.None, id, "temp", Item.ItemSlot.Head, string.Empty, new Stats(), new Sockets(), 0, 0, 0, 0, 0, Item.ItemDamageType.Physical, 0f, string.Empty);
						ItemCache.AddItem(newItem);
					}
				}
                //Not an else, because we might have just created new Item manually.
                if (newItem != null)
				{
					ListViewItem newLvi = new ListViewItem(newItem.Name, 0, listViewItems.Groups["listViewGroup" + newItem.Slot.ToString()]);
					newLvi.Tag = newItem;
					newLvi.ImageKey = EnsureIconPath(newItem.IconPath);
					string slot = newItem.Slot.ToString();
					if (newItem.Slot == Item.ItemSlot.Red || newItem.Slot == Item.ItemSlot.Orange || newItem.Slot == Item.ItemSlot.Yellow
						 || newItem.Slot == Item.ItemSlot.Green || newItem.Slot == Item.ItemSlot.Blue || newItem.Slot == Item.ItemSlot.Purple
						 || newItem.Slot == Item.ItemSlot.Prismatic || newItem.Slot == Item.ItemSlot.Meta) slot = "Gems";
					newLvi.Group = listViewItems.Groups["listViewGroup" + slot];

                    listViewItems.Items.Add(newLvi);
                    listViewItems.Sort();
                    listViewItems.SelectedIndices.Clear();
                    //won't select the item graphically (box it) without selecting the control...*shrug
                    listViewItems.Select();
                    newLvi.Selected = true;
					newLvi.EnsureVisible();
                
                }
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
			if (form.ShowDialog() == DialogResult.OK)
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
		}

		private void buttonDuplicate_Click(object sender, EventArgs e)
		{
			Item item = listViewItems.SelectedItems[0].Tag as Item;
			Item copy = new Item(item.Name, item.Quality, item.Type, item.Id, item.IconPath, item.Slot, item.SetName, item.Stats.Clone(),
				item.Sockets.Clone(), 0, 0, 0, item.MinDamage, item.MaxDamage, item.DamageType, item.Speed, item.RequiredClasses);
			_changingItemCache = true;
			ItemCache.AddItem(copy);
			_changingItemCache = false;

			ListViewItem newLvi = new ListViewItem(copy.Name, 0, listViewItems.Groups["listViewGroup" + copy.Slot.ToString()]);
			newLvi.Tag = copy;
			newLvi.ImageKey = EnsureIconPath(copy.IconPath);
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
			Item selectedItem = SelectedItem.Tag as Item;
			LoadItems();
			SelectItem(selectedItem, false);
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

        }
	}
}
