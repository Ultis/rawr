using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public class ItemContextualMenu : ContextMenuStrip
	{
		private static ItemContextualMenu _instance = null;
		public static ItemContextualMenu Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ItemContextualMenu();
				}
				return _instance;
			}
		}

		private Character _character;
		public Character Character
		{
			get { return _character; }
			set { _character = value; }
		}

		private Item _item;
        private Character _itemCharacter;
		private Character.CharacterSlot _equipSlot;
		private ToolStripMenuItem _menuItemName;
		private ToolStripMenuItem _menuItemEdit;
		private ToolStripMenuItem _menuItemWowhead;
		private ToolStripMenuItem _menuItemRefresh;
		private ToolStripMenuItem _menuItemEquip;
        private ToolStripMenuItem _menuItemEquipAll;
        private ToolStripMenuItem _menuItemDelete;
		private ToolStripMenuItem _menuItemDeleteDuplicates;
		private ToolStripMenuItem _menuItemCreateBearGemmings;
		private ToolStripMenuItem _menuItemCreateCatGemmings;
		public ItemContextualMenu()
		{
			_menuItemName = new ToolStripMenuItem();
			_menuItemName.Enabled = false;
			
			_menuItemEdit = new ToolStripMenuItem("Edit...");
			_menuItemEdit.Click += new EventHandler(_menuItemEdit_Click);

			_menuItemWowhead = new ToolStripMenuItem("Open in Wowhead");
			_menuItemWowhead.Click += new EventHandler(_menuItemWowhead_Click);

			_menuItemRefresh = new ToolStripMenuItem("Refresh Item Data");
			_menuItemRefresh.Click += new EventHandler(_menuItemRefresh_Click);

			_menuItemEquip = new ToolStripMenuItem("Equip");
			_menuItemEquip.Click += new EventHandler(_menuItemEquip_Click);

            _menuItemEquipAll = new ToolStripMenuItem("Equip All");
            _menuItemEquipAll.Click += new EventHandler(_menuItemEquipAll_Click);

			_menuItemDelete = new ToolStripMenuItem("Delete");
			_menuItemDelete.Click += new EventHandler(_menuItemDelete_Click);

			_menuItemDeleteDuplicates = new ToolStripMenuItem("Delete Duplicates");
			_menuItemDeleteDuplicates.Click += new EventHandler(_menuItemDeleteDuplicates_Click);

			//TODO: Implement this for all models.
			_menuItemCreateBearGemmings = new ToolStripMenuItem("Create Bear Gemmings");
			_menuItemCreateBearGemmings.Click += new EventHandler(_menuItemCreateBearGemmings_Click);
			_menuItemCreateCatGemmings = new ToolStripMenuItem("Create Cat Gemmings");
			_menuItemCreateCatGemmings.Click += new EventHandler(_menuItemCreateCatGemmings_Click);

			this.Items.Add(_menuItemName);
			this.Items.Add(new ToolStripSeparator());
			this.Items.Add(_menuItemEdit);
			this.Items.Add(_menuItemWowhead);
			this.Items.Add(_menuItemRefresh);
			this.Items.Add(_menuItemEquip);
            this.Items.Add(_menuItemEquipAll);
            this.Items.Add(_menuItemDelete);
			this.Items.Add(_menuItemDeleteDuplicates);

			//this.Items.Add(_menuItemCreateBearGemmings);
			//this.Items.Add(_menuItemCreateCatGemmings);
		}

        public void Show(Item item, Character.CharacterSlot equipSlot, bool allowDelete)
        {
            Show(item, equipSlot, null, allowDelete);
        }

		public void Show(Item item, Character.CharacterSlot equipSlot, Character itemCharacter, bool allowDelete)
		{
			_item = item;
            _itemCharacter = itemCharacter;
            _menuItemEquipAll.Visible = (_itemCharacter != null);
			_equipSlot = equipSlot;
			_menuItemEquip.Enabled = (this.Character[equipSlot] != item);
			_menuItemEquip.Visible = equipSlot != Character.CharacterSlot.None;
			_menuItemDelete.Enabled = allowDelete && _menuItemEquip.Enabled;
			_menuItemDeleteDuplicates.Enabled = allowDelete;
			_menuItemName.Text = item.Name;

			this.Show(Control.MousePosition);
		}

		void _menuItemDelete_Click(object sender, EventArgs e)
		{
			ItemCache.DeleteItem(_item);
		}

		void _menuItemDeleteDuplicates_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete all instances of " + _item.Name + " except the selected one?", "Confirm Delete Duplicates", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				Cursor = Cursors.WaitCursor;
				List<Item> itemsToDelete = new List<Item>(ItemCache.Instance.FindAllItemsById(_item.Id));
				Item itemUngemmed = ItemCache.FindItemById(_item.Id.ToString() + ".0.0.0", false, false);
				if (itemUngemmed != null) itemsToDelete.Add(itemUngemmed);
				if (itemsToDelete.Contains(_item)) itemsToDelete.Remove(_item);
				if (itemsToDelete.Contains(Character[_equipSlot])) itemsToDelete.Remove(Character[_equipSlot]);
				foreach (Item itemToDelete in itemsToDelete)
					ItemCache.DeleteItem(itemToDelete);
				Cursor = Cursors.Default;
			}
		}

		void _menuItemCreateBearGemmings_Click(object sender, EventArgs e)
		{
			string gemmedAgi = _item.Id.ToString();
			string gemmedSocketAgi = _item.Id.ToString();
			string gemmedSocketStam = _item.Id.ToString();
			string gemmedStam = _item.Id.ToString();
			foreach(Item.ItemSlot color in new Item.ItemSlot[]
				{_item.Sockets.Color1, _item.Sockets.Color2, _item.Sockets.Color3})
			{
				switch (color)
				{
					case Item.ItemSlot.Red:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32194";
						gemmedSocketStam += ".32212";
						gemmedStam += ".32200";
						break;

					case Item.ItemSlot.Yellow:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".30585";
						gemmedSocketStam += ".32223";
						gemmedStam += ".32200";
						break;
						
					case Item.ItemSlot.Blue:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32212";
						gemmedSocketStam += ".32200";
						gemmedStam += ".32200";
						break;

					case Item.ItemSlot.Meta:
						gemmedAgi += ".32409";
						gemmedSocketAgi += ".32409";
						gemmedSocketStam += ".25896";
						gemmedStam += ".25896";
						break;

					default:
						gemmedAgi += ".0";
						gemmedSocketAgi += ".0";
						gemmedSocketStam += ".0";
						gemmedStam += ".0";
						break;
				}
			}

			ItemCache.FindItemById(gemmedAgi);
			ItemCache.FindItemById(gemmedSocketAgi);
			ItemCache.FindItemById(gemmedSocketStam);
			ItemCache.FindItemById(gemmedStam);
		}

		void _menuItemCreateCatGemmings_Click(object sender, EventArgs e)
		{
			string gemmedAgi = _item.Id.ToString();
			string gemmedSocketAgi = _item.Id.ToString();
			foreach (Item.ItemSlot color in new Item.ItemSlot[] { _item.Sockets.Color1, _item.Sockets.Color2, _item.Sockets.Color3 })
			{
				switch (color)
				{
					case Item.ItemSlot.Red:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32194";
						break;

					case Item.ItemSlot.Yellow:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32220";
						break;

					case Item.ItemSlot.Blue:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32212";
						break;

					case Item.ItemSlot.Meta:
						gemmedAgi += ".32409";
						gemmedSocketAgi += ".32409";
						break;

					default:
						gemmedAgi += ".0";
						gemmedSocketAgi += ".0";
						break;
				}
			}

			ItemCache.FindItemById(gemmedAgi);
			ItemCache.FindItemById(gemmedSocketAgi);
		}

		void _menuItemEquip_Click(object sender, EventArgs e)
		{
            this.Character[_equipSlot] = _item == null ? null : ItemCache.FindItemById(_item.GemmedId);
		}

        void _menuItemEquipAll_Click(object sender, EventArgs e)
        {
            _character.IsLoading = true;
            _character.Back = _itemCharacter.Back == null ? null : ItemCache.FindItemById(_itemCharacter.Back.GemmedId);
            _character.Chest = _itemCharacter.Chest == null ? null : ItemCache.FindItemById(_itemCharacter.Chest.GemmedId);
            _character.Feet = _itemCharacter.Feet == null ? null : ItemCache.FindItemById(_itemCharacter.Feet.GemmedId);
            _character.Finger1 = _itemCharacter.Finger1 == null ? null : ItemCache.FindItemById(_itemCharacter.Finger1.GemmedId);
            _character.Finger2 = _itemCharacter.Finger2 == null ? null : ItemCache.FindItemById(_itemCharacter.Finger2.GemmedId);
            _character.Hands = _itemCharacter.Hands == null ? null : ItemCache.FindItemById(_itemCharacter.Hands.GemmedId);
            _character.Head = _itemCharacter.Head == null ? null : ItemCache.FindItemById(_itemCharacter.Head.GemmedId);
            _character.Legs = _itemCharacter.Legs == null ? null : ItemCache.FindItemById(_itemCharacter.Legs.GemmedId);
            _character.MainHand = _itemCharacter.MainHand == null ? null : ItemCache.FindItemById(_itemCharacter.MainHand.GemmedId);
            _character.Neck = _itemCharacter.Neck == null ? null : ItemCache.FindItemById(_itemCharacter.Neck.GemmedId);
            _character.OffHand = _itemCharacter.OffHand == null ? null : ItemCache.FindItemById(_itemCharacter.OffHand.GemmedId);
            _character.Projectile = _itemCharacter.Projectile == null ? null : ItemCache.FindItemById(_itemCharacter.Projectile.GemmedId);
            _character.ProjectileBag = _itemCharacter.ProjectileBag == null ? null : ItemCache.FindItemById(_itemCharacter.ProjectileBag.GemmedId);
            _character.Ranged = _itemCharacter.Ranged == null ? null : ItemCache.FindItemById(_itemCharacter.Ranged.GemmedId);
            _character.Shoulders = _itemCharacter.Shoulders == null ? null : ItemCache.FindItemById(_itemCharacter.Shoulders.GemmedId);
            _character.Trinket1 = _itemCharacter.Trinket1 == null ? null : ItemCache.FindItemById(_itemCharacter.Trinket1.GemmedId);
            _character.Trinket2 = _itemCharacter.Trinket2 == null ? null : ItemCache.FindItemById(_itemCharacter.Trinket2.GemmedId);
            _character.Waist = _itemCharacter.Waist == null ? null : ItemCache.FindItemById(_itemCharacter.Waist.GemmedId);
            _character.Wrist = _itemCharacter.Wrist == null ? null : ItemCache.FindItemById(_itemCharacter.Wrist.GemmedId);
            _character.BackEnchant = _itemCharacter.BackEnchant;
            _character.ChestEnchant = _itemCharacter.ChestEnchant;
            _character.FeetEnchant = _itemCharacter.FeetEnchant;
            _character.Finger1Enchant = _itemCharacter.Finger1Enchant;
            _character.Finger2Enchant = _itemCharacter.Finger2Enchant;
            _character.HandsEnchant = _itemCharacter.HandsEnchant;
            _character.HeadEnchant = _itemCharacter.HeadEnchant;
            _character.LegsEnchant = _itemCharacter.LegsEnchant;
            _character.MainHandEnchant = _itemCharacter.MainHandEnchant;
            _character.OffHandEnchant = _itemCharacter.OffHandEnchant;
            _character.RangedEnchant = _itemCharacter.RangedEnchant;
            _character.ShouldersEnchant = _itemCharacter.ShouldersEnchant;
            _character.WristEnchant = _itemCharacter.WristEnchant;
            _character.IsLoading = false;
            _character.OnItemsChanged();
        }

		void _menuItemRefresh_Click(object sender, EventArgs e)
		{
			ItemCache.DeleteItem(_item);
			Item newItem = Item.LoadFromId(_item.GemmedId, true, "Refreshing",false);
			if (newItem == null)
			{
				MessageBox.Show("Unable to find item " + _item.Id + ". Reverting to previous data.");
				ItemCache.AddItem(_item, true, false);
			}
			ItemCache.OnItemsChanged();
		}

		void _menuItemWowhead_Click(object sender, EventArgs e)
		{
			//System.Diagnostics.Process.Start("http://www.wowhead.com/?item=" + _item.Id);
            Help.ShowHelp(null, "http://www.wowhead.com/?item=" + _item.Id);
		}

		void _menuItemEdit_Click(object sender, EventArgs e)
		{
			FormItemEditor editor = null;
			foreach (Form form in Application.OpenForms) if (form is FormItemEditor) editor = form as FormItemEditor;
			if (editor == null)
			{
				FormItemEditor itemEditor = new FormItemEditor(Character);
				itemEditor.SelectItem(_item, true);
				itemEditor.ShowDialog(FormMain.Instance);
                itemEditor.Dispose();
				ItemCache.OnItemsChanged();
				//FormMain.Instance.OpenItemEditor(_item);
			}
			else
			{
				editor.SelectItem(_item, true);
				editor.Focus();
			}
		}
	}
}
