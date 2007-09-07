using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public class ItemButton : Button
	{
		public ItemTooltip Tooltip
		{
			get
			{
				ItemTooltip tooltip = null;
				foreach (Control ctrl in this.FindForm().Controls)
					if (ctrl is ItemTooltip)
					{
						tooltip = ctrl as ItemTooltip;
						break;
					}
				if (tooltip == null)
				{
					tooltip = new ItemTooltip();
					this.FindForm().Controls.Add(tooltip);
				}

				System.Drawing.Point p = this.FindForm().PointToClient(this.Parent.PointToScreen(this.Location));
				p.X += this.Width + 2;
				p.Y += 2;
				tooltip.Location = p;
				tooltip.Visible = false;

				return tooltip;
			}
		}

		private ContextMenuStrip menu = new ContextMenuStrip();
		public ItemButton()
		{
			this.Size = new System.Drawing.Size(70, 70);
			this.Text = string.Empty;
			this.Click += new EventHandler(ItemButton_Click);
			this.MouseEnter += new EventHandler(ItemButton_MouseEnter);
			this.MouseLeave += new EventHandler(ItemButton_MouseLeave);
		}

		void ItemButton_MouseLeave(object sender, EventArgs e)
		{
			Tooltip.Visible = false;
		}

		void ItemButton_MouseEnter(object sender, EventArgs e)
		{
			Tooltip.SetItem(SelectedItem);
		}

		void ItemButton_Click(object sender, EventArgs e)
		{
			menu.Show(this, this.Width, 0);
		}

		private Character.CharacterSlot _characterSlot = Character.CharacterSlot.Head;
		public Character.CharacterSlot CharacterSlot
		{
			get { return _characterSlot; }
			set
			{
				_characterSlot = value;
				Items = Items;
			}
		}

		private Character _character;

		public Character Character
		{
			get { return _character; }
			set
			{
				_character = value;
				if (_character != null)
					SelectedItem = _character[CharacterSlot];
			}
		}

		public int SelectedItemId
		{
			get
			{
				if (SelectedItem == null) return 0;
				else return SelectedItem.Id;
			}
			set
			{
				if (value == 0) SelectedItem = null;
				else SelectedItem = ItemCache.FindItemById(value);
			}
		}

		private Item _selectedItem;
		public Item SelectedItem
		{
			get
			{
				return _selectedItem;
			}
			set
			{
				_selectedItem = value;
				this.Text = string.Empty;
				this.Image = _selectedItem != null ? ItemIcons.GetItemIcon(_selectedItem) : null;
				foreach (ToolStripMenuItem menuItem in menu.Items)
					menuItem.Checked = (menuItem.Tag == _selectedItem);

				if (Character != null)
					Character[CharacterSlot] = SelectedItem;
			}
		}

		public Item[] _items = new Item[0];
		public Item[] Items
		{
			get
			{
				return _items;
			}
			set
			{
				_items = value;
				if (_items == null) _items = new Item[0];

				//_items = new List<Item>(sortedItems.Values).ToArray();
				SortedList<string, Item> sortedItems = new SortedList<string, Item>();
				foreach (Item item in _items)
					if (item.FitsInSlot(CharacterSlot))
						sortedItems.Add(item.Name + item.GemmedId + item.GetHashCode().ToString(), item);

				menu.Items.Clear();
				ToolStripMenuItem menuItem = new ToolStripMenuItem("None");
				menuItem.Height = 36;
				menuItem.Click += new EventHandler(menuItem_Click);
				menuItem.Checked = (SelectedItem == null);
				menu.Items.Add(menuItem);
				foreach (Item item in sortedItems.Values)
				{
					//menu.Items.Add(item.ToString(), ItemIcons.GetItemIcon(item, true));
					menuItem = new ToolStripMenuItem(item.ToString(), ItemIcons.GetItemIcon(item, true));
					menuItem.Height = 36;
					menuItem.ImageScaling = ToolStripItemImageScaling.None;
					menuItem.Click += new EventHandler(menuItem_Click);
					menuItem.Tag = item;
					menuItem.Checked = (SelectedItem == item);
					menu.Items.Add(menuItem);
				}
				SelectedItem = _selectedItem;
			}
		}

		void menuItem_Click(object sender, EventArgs e)
		{
			SelectedItem = (sender as ToolStripMenuItem).Tag as Item;
		}
	}


	//public class GemButton : Button
	//{
	//    ToolTip tip = new ToolTip();
	//    ContextMenuStrip menu = new ContextMenuStrip();
	//    public GemButton()
	//    {
	//        this.Size = new System.Drawing.Size(70, 70);
	//        this.Text = string.Empty;
	//        this.Click += new EventHandler(GemButton_Click);
	//        tip.AutomaticDelay = 0;
	//        tip.InitialDelay = 0;
	//        tip.ReshowDelay = 0;
	//        tip.AutoPopDelay = 0;
	//        tip.UseAnimation = false;
	//        tip.UseFading = false;
	//    }

	//    void GemButton_Click(object sender, EventArgs e)
	//    {
	//        menu.Show(this, this.Width, 0);
	//    }

	//    private Gem _selectedGem;
	//    public Gem SelectedGem
	//    {
	//        get
	//        {
	//            return _selectedGem;
	//        }
	//        set
	//        {
	//            _selectedGem = value;
	//            this.Text = string.Empty;
	//            if (_selectedGem != null)
	//            {
	//                tip.SetToolTip(this, _selectedGem.ToString());
	//                this.Image = ItemIcons.GetItemIcon(_selectedGem);
	//                foreach (ToolStripMenuItem menuItem in menu.Items)
	//                    menuItem.Checked = (menuItem.Tag == _selectedGem);
	//            }
	//        }
	//    }

	//    public Gem[] _gems = new Gem[0];
	//    public Gem[] Gems
	//    {
	//        get
	//        {
	//            return _gems;
	//        }
	//        set
	//        {
	//            _gems = value;
	//            if (_gems == null) _gems = new Gem[0];

	//            menu.Items.Clear();
	//            foreach (Gem gem in _gems)
	//            {
	//                menu.Items.Add(gem.ToString(), ItemIcons.GetItemIcon(gem, true));
	//                ToolStripMenuItem menuItem = menu.Items[menu.Items.Count - 1] as ToolStripMenuItem;
	//                menuItem.Height = 36;
	//                menuItem.ImageScaling = ToolStripItemImageScaling.None;
	//                menuItem.Click += new EventHandler(menuItem_Click);
	//                menuItem.Tag = gem;
	//                menuItem.Checked = (gem == SelectedGem);
	//            }
	//        }
	//    }

	//    void menuItem_Click(object sender, EventArgs e)
	//    {
	//        SelectedGem = (sender as ToolStripMenuItem).Tag as Gem;
	//    }
	//}
}
