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
		private Character.CharacterSlot _equipSlot;
		private ToolStripMenuItem _menuItemName;
		private ToolStripMenuItem _menuItemEdit;
		private ToolStripMenuItem _menuItemWowhead;
		private ToolStripMenuItem _menuItemRefresh;
		private ToolStripMenuItem _menuItemEquip;
		private ToolStripMenuItem _menuItemDelete;
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

			_menuItemDelete = new ToolStripMenuItem("Delete");
			_menuItemDelete.Click += new EventHandler(_menuItemDelete_Click);

			this.Items.Add(_menuItemName);
			this.Items.Add(new ToolStripSeparator());
			this.Items.Add(_menuItemEdit);
			this.Items.Add(_menuItemWowhead);
			this.Items.Add(_menuItemRefresh);
			this.Items.Add(_menuItemEquip);
			this.Items.Add(_menuItemDelete);
		}

		public void Show(Item item, Character.CharacterSlot equipSlot, bool allowDelete)
		{
			_item = item;
			_equipSlot = equipSlot;
			_menuItemEquip.Enabled = (this.Character[equipSlot] != item);
			_menuItemEquip.Visible = equipSlot != Character.CharacterSlot.None;
			_menuItemDelete.Enabled = allowDelete && _menuItemEquip.Enabled;
			_menuItemName.Text = item.Name;

			this.Show(Control.MousePosition);
		}

		void _menuItemDelete_Click(object sender, EventArgs e)
		{
			ItemCache.DeleteItem(_item);
		}

		void _menuItemEquip_Click(object sender, EventArgs e)
		{
			this.Character[_equipSlot] = _item;
		}

		void _menuItemRefresh_Click(object sender, EventArgs e)
		{
			//ItemCache.DeleteItem(_item);
			Item newItem = Item.LoadFromId(_item.GemmedId, true, "Refreshing");
			if (newItem == null)
			{
				MessageBox.Show("Unable to find item " + _item.Id + ". Reverting to previous data.");
				ItemCache.AddItem(_item, true, false);
			}
			ItemCache.OnItemsChanged();
		}

		void _menuItemWowhead_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.wowhead.com/?item=" + _item.Id);
		}

		void _menuItemEdit_Click(object sender, EventArgs e)
		{
			FormItemEditor editor = null;
			foreach (Form form in Application.OpenForms) if (form is FormItemEditor) editor = form as FormItemEditor;
			if (editor == null)
			{
				editor = new FormItemEditor(Character);
				editor.SelectItem(_item);
				FormMain formMain = null;
				foreach (Form form in Application.OpenForms)
					if (form is FormMain)
						formMain = form as FormMain;
				if (formMain != null)
					editor.ShowDialog(formMain);
				else
					editor.ShowDialog();
				ItemCache.OnItemsChanged();
			}
			else
			{
				editor.SelectItem(_item);
				editor.Focus();
			}
		}
	}
}
