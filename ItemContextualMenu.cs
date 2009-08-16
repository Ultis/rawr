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

		private ItemInstance _item;
        private ItemInstance[] _characterItems;
		private CharacterSlot _equipSlot;
		private ToolStripMenuItem _menuItemName;
		private ToolStripMenuItem _menuItemEdit;
		private ToolStripMenuItem _menuItemWowhead;
		private ToolStripMenuItem _menuItemRefresh;
		private ToolStripMenuItem _menuItemRefreshWowhead;
		private ToolStripMenuItem _menuItemEquip;
		private ToolStripMenuItem _menuItemEquipAll;
		private ToolStripMenuItem _menuItemRemoveFromUpgradeList;
        private ToolStripMenuItem _menuItemDelete;
		private ToolStripMenuItem _menuItemDeleteDuplicates;
		//private ToolStripMenuItem _menuItemCreateBearGemmings;
		//private ToolStripMenuItem _menuItemCreateCatGemmings;
        private ToolStripMenuItem _menuItemEvaluateUpgrade;
        private ToolStripMenuItem _menuItemCustomizeItem;
        private ToolStripMenuItem _menuItemEquipCustomizedItem;
        public ItemContextualMenu()
		{
			_menuItemName = new ToolStripMenuItem();
			_menuItemName.Enabled = false;
			
			_menuItemEdit = new ToolStripMenuItem("Edit...");
			_menuItemEdit.Click += new EventHandler(_menuItemEdit_Click);

			_menuItemWowhead = new ToolStripMenuItem("Open in Wowhead");
			_menuItemWowhead.Click += new EventHandler(_menuItemWowhead_Click);

			_menuItemRefresh = new ToolStripMenuItem("Refresh Item Data from Armory");
			_menuItemRefresh.Click += new EventHandler(_menuItemRefresh_Click);

			_menuItemRefreshWowhead = new ToolStripMenuItem("Refresh Item Data from Wowhead");
			_menuItemRefreshWowhead.Click += new EventHandler(_menuItemRefreshWowhead_Click);

			_menuItemEquip = new ToolStripMenuItem("Equip");
			_menuItemEquip.Click += new EventHandler(_menuItemEquip_Click);

            _menuItemEquipAll = new ToolStripMenuItem("Equip All");
			_menuItemEquipAll.Click += new EventHandler(_menuItemEquipAll_Click);

			_menuItemRemoveFromUpgradeList = new ToolStripMenuItem("Remove from Upgrade List");
			_menuItemRemoveFromUpgradeList.Click += new EventHandler(_menuItemRemoveFromUpgradeList_Click);

			_menuItemDelete = new ToolStripMenuItem("Delete Custom Gemming");
			_menuItemDelete.Click += new EventHandler(_menuItemDelete_Click);

			_menuItemDeleteDuplicates = new ToolStripMenuItem("Delete Duplicates");
			_menuItemDeleteDuplicates.Click += new EventHandler(_menuItemDeleteDuplicates_Click);

            _menuItemEvaluateUpgrade = new ToolStripMenuItem("Evaluate Upgrade");
            _menuItemEvaluateUpgrade.Click += new EventHandler(_menuItemEvaluateUpgrade_Click);

            _menuItemCustomizeItem = new ToolStripMenuItem("Add Custom Gemming...");
            _menuItemCustomizeItem.Click += new EventHandler(_menuItemCustomizeItem_Click);

            _menuItemEquipCustomizedItem = new ToolStripMenuItem("Equip Custom Gemming...");
            _menuItemEquipCustomizedItem.Click += new EventHandler(_menuItemEquipCustomizedItem_Click);

			this.Items.Add(_menuItemName);
			this.Items.Add(new ToolStripSeparator());
			this.Items.Add(_menuItemEdit);
			this.Items.Add(_menuItemWowhead);
			this.Items.Add(_menuItemRefresh);
			this.Items.Add(_menuItemRefreshWowhead);
			this.Items.Add(_menuItemEquip);
			this.Items.Add(_menuItemEquipAll);
			this.Items.Add(_menuItemRemoveFromUpgradeList);
            this.Items.Add(_menuItemCustomizeItem);
            this.Items.Add(_menuItemEquipCustomizedItem);
            this.Items.Add(_menuItemDelete);
			//this.Items.Add(_menuItemDeleteDuplicates);
            this.Items.Add(_menuItemEvaluateUpgrade);
		}

        void _menuItemEquipCustomizedItem_Click(object sender, EventArgs e)
        {
            FormItemInstance form = new FormItemInstance();
            form.CharacterSlot = _equipSlot;
            form.ItemInstance = _item.Clone();
            if (form.ShowDialog(FormMain.Instance) == DialogResult.OK)
            {
                Character[_equipSlot] = form.ItemInstance == null ? null : form.ItemInstance.Clone();
            }
        }

        void _menuItemCustomizeItem_Click(object sender, EventArgs e)
        {
            FormItemInstance form = new FormItemInstance();
            form.CharacterSlot = _equipSlot;
            form.ItemInstance = _item.Clone();
            if (form.ShowDialog(FormMain.Instance) == DialogResult.OK)
            {
                ItemInstance itemInstance = form.ItemInstance.Clone();
                itemInstance.ForceDisplay = true;
                Character.CustomItemInstances.Add(itemInstance);
                Character.OnCalculationsInvalidated();
            }
        }

        void _menuItemEvaluateUpgrade_Click(object sender, EventArgs e)
        {
            FormOptimize optimize = new FormOptimize(Character);
            optimize.EvaluateUpgrades(_item.Item);
            optimize.ShowDialog(this);
            if (optimize.ShowUpgradeComparison)
            {
                FormUpgradeComparison.Instance.Show();
                FormUpgradeComparison.Instance.BringToFront();
            }
            optimize.Dispose();
        }

        public void Show(Character character, ItemInstance item, CharacterSlot equipSlot, bool allowDelete)
        {
            // TankConcrete 09.01.09 - Added a check to make sure the item being displayed
            // is really an item we can show a context menu for. Enchants, etc., won't work
            // properly so there's no reason to display the "Open in Wowhead" menu.
            // Items with ID > 0 are regular items. Below 0 are enchants and the like.
            if (item.Id > 0)
            {
                Show(character, item, equipSlot, null, allowDelete);
            }
        }

		public void Show(Character character, ItemInstance item, CharacterSlot equipSlot, ItemInstance[] characterItems, bool allowDelete)
		{
            Character = character;
			_item = item;
            _characterItems = characterItems;
            _menuItemEquipAll.Visible = _menuItemRemoveFromUpgradeList.Visible = (_characterItems != null);
			_equipSlot = equipSlot;
			_menuItemEquip.Enabled = (Character[equipSlot] != item);
            _menuItemEquip.Visible = _menuItemEvaluateUpgrade.Visible = _menuItemEquipCustomizedItem.Visible = equipSlot != CharacterSlot.None;
			_menuItemDelete.Enabled = allowDelete && _menuItemEquip.Enabled && Character.CustomItemInstances.Contains(item);
			_menuItemDeleteDuplicates.Enabled = allowDelete;
			_menuItemName.Text = item.Item.Name;

			this.Show(Control.MousePosition);
		}

		void _menuItemDelete_Click(object sender, EventArgs e)
		{
			//ItemCache.DeleteItem(_item);
            if (Character.CustomItemInstances.Contains(_item))
            {
                Character.CustomItemInstances.Remove(_item);
                Character.OnCalculationsInvalidated();
            }
		}

		void _menuItemDeleteDuplicates_Click(object sender, EventArgs e)
		{
			/*if (MessageBox.Show("Are you sure you want to delete all instances of " + _item.Item.Name + " except the selected one?", "Confirm Delete Duplicates", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
			}*/
		}

		void _menuItemCreateBearGemmings_Click(object sender, EventArgs e)
		{
			string gemmedAgi = _item.Id.ToString();
			string gemmedSocketAgi = _item.Id.ToString();
			string gemmedSocketStam = _item.Id.ToString();
			string gemmedStam = _item.Id.ToString();
			foreach(ItemSlot color in new ItemSlot[]
				{_item.Item.SocketColor1, _item.Item.SocketColor2, _item.Item.SocketColor3})
			{
				switch (color)
				{
					case ItemSlot.Red:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32194";
						gemmedSocketStam += ".32212";
						gemmedStam += ".32200";
						break;

					case ItemSlot.Yellow:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".30585";
						gemmedSocketStam += ".32223";
						gemmedStam += ".32200";
						break;
						
					case ItemSlot.Blue:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32212";
						gemmedSocketStam += ".32200";
						gemmedStam += ".32200";
						break;

					case ItemSlot.Meta:
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

            //ItemCache.FindItemById(gemmedAgi);
            //ItemCache.FindItemById(gemmedSocketAgi);
            //ItemCache.FindItemById(gemmedSocketStam);
            //ItemCache.FindItemById(gemmedStam);
		}

		void _menuItemCreateCatGemmings_Click(object sender, EventArgs e)
		{
			string gemmedAgi = _item.Id.ToString();
			string gemmedSocketAgi = _item.Id.ToString();
			foreach (ItemSlot color in new ItemSlot[] { _item.Item.SocketColor1, _item.Item.SocketColor2, _item.Item.SocketColor3 })
			{
				switch (color)
				{
					case ItemSlot.Red:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32194";
						break;

					case ItemSlot.Yellow:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32220";
						break;

					case ItemSlot.Blue:
						gemmedAgi += ".32194";
						gemmedSocketAgi += ".32212";
						break;

					case ItemSlot.Meta:
						gemmedAgi += ".32409";
						gemmedSocketAgi += ".32409";
						break;

					default:
						gemmedAgi += ".0";
						gemmedSocketAgi += ".0";
						break;
				}
			}

			//ItemCache.FindItemById(gemmedAgi);
			//ItemCache.FindItemById(gemmedSocketAgi);
		}

		void _menuItemEquip_Click(object sender, EventArgs e)
		{
            this.Character[_equipSlot] = _item == null ? null : _item.Clone();
		}

        void _menuItemEquipAll_Click(object sender, EventArgs e)
        {
            _character.SetItems(_characterItems);
		}

		void _menuItemRemoveFromUpgradeList_Click(object sender, EventArgs e)
		{
			FormUpgradeComparison.Instance.RemoveItem(_item);
		}

		void _menuItemRefresh_Click(object sender, EventArgs e)
		{
			//ItemCache.DeleteItem(_item);
			Item newItem = Item.LoadFromId(_item.Id, true, true, false);
			/*if (newItem == null)
			{
				MessageBox.Show("Unable to find item " + _item.Id + ". Reverting to previous data.");
				ItemCache.AddItem(_item, false);
			}*/
			ItemCache.OnItemsChanged();
		}

		void _menuItemRefreshWowhead_Click(object sender, EventArgs e)
		{
			//ItemCache.DeleteItem(_item);
			/* We don't have to use ptr for the moment... so we can use the generic method... and the locale informations is download.
			Item newItem = Wowhead.GetItem(FormMain.Instance.usePTRDataToolStripMenuItem.Checked ? "ptr" : "www", _item.Id.ToString(), false);
			if (newItem == null)
			{
				MessageBox.Show("Unable to find item " + _item.Id + ". Reverting to previous data.");
				//ItemCache.AddItem(_item, true, false);
			}
			else
			{
				ItemCache.AddItem(newItem, true);
			}*/
            Item newItem = Item.LoadFromId(_item.Id, true, true, true);
			ItemCache.OnItemsChanged();
		}

		void _menuItemWowhead_Click(object sender, EventArgs e)
		{
            string site = Properties.GeneralSettings.Default.Locale;
            if (site == "en")
            {
                if (FormMain.Instance.IsUsingPTR())
                    site = "ptr";
                else
                    site = "www";
            }
            Help.ShowHelp(null, "http://" + site + ".wowhead.com/?item=" + _item.Id);
		}

		void _menuItemEdit_Click(object sender, EventArgs e)
		{
			FormItemEditor editor = null;
			foreach (Form form in Application.OpenForms) if (form is FormItemEditor) editor = form as FormItemEditor;
			if (editor == null)
			{
				FormItemEditor itemEditor = new FormItemEditor(Character, _item.Item);
				//itemEditor.SelectItem(_item, true);
				itemEditor.ShowDialog(FormMain.Instance);
                itemEditor.Dispose();
				ItemCache.OnItemsChanged();
				//FormMain.Instance.OpenItemEditor(_item);
			}
			else
			{
				editor.SelectItem(_item.Item, true);
				editor.Focus();
			}
		}
	}
}
