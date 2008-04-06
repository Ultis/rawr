using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Rawr
{
	public class ItemButton : Button
	{
		public Point GetTooltipLocation()
		{
			//get
			//{
				//ItemTooltip tooltip = ItemTooltip.Instance;
				//foreach (Control ctrl in this.FindForm().Controls)
				//    if (ctrl is ItemTooltip)
				//    {
				//        tooltip = ctrl as ItemTooltip;
				//        break;
				//    }
				//if (tooltip == null)
				//{
				//    tooltip = new ItemTooltip();
				//    this.FindForm().Controls.Add(tooltip);
				//}

				System.Drawing.Point p = (this.Parent.PointToScreen(this.Location)); //this.FindForm().PointToClient
				p.X += this.Width + 2;
				p.Y += 2;
				//tooltip.Location = p;
				//tooltip.Visible = false;

				//return tooltip;
			//}
				return p;
		}

		//private ContextMenuStrip menu = new ContextMenuStrip();
		public ItemButton()
		{
			this.Size = new System.Drawing.Size(70, 70);
			this.Text = string.Empty;
			this.MouseUp += new MouseEventHandler(ItemButton_MouseClick);
			this.MouseEnter += new EventHandler(ItemButton_MouseEnter);
			this.MouseLeave += new EventHandler(ItemButton_MouseLeave);

			ItemToolTip.Instance.SetToolTip(this, "");

			ItemCache.Instance.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
		}

		private bool _itemsChanging = false;
		void ItemCache_ItemsChanged(object sender, EventArgs e)
		{
			if (SelectedItem != null && !_itemsChanging)
			{
				_itemsChanging = true;
				SelectedItem = ItemCache.FindItemById(SelectedItem.GemmedId);
				_itemsChanging = false;
			}
		}

		void ItemButton_MouseLeave(object sender, EventArgs e)
		{
			ItemToolTip.Instance.Hide(this);
		}

		void ItemButton_MouseEnter(object sender, EventArgs e)
		{
			if (SelectedItem != null)
			{
				int tipX = this.Width;
				if (Parent.PointToScreen(Location).X + tipX + 249 > System.Windows.Forms.Screen.GetWorkingArea(this).Right)
					tipX = -249;
				ItemToolTip.Instance.Show(SelectedItem, this, new Point(tipX, 0));
			}
		}

		void ItemButton_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				(this.FindForm() as IFormItemSelectionProvider).FormItemSelection.Show(this, CharacterSlot);
			}
			else if (e.Button == MouseButtons.Right && SelectedItem != null)
			{
				ItemContextualMenu.Instance.Show(SelectedItem, Character.CharacterSlot.None, false);
			}
		}

		private Character.CharacterSlot _characterSlot = Character.CharacterSlot.Head;
		public Character.CharacterSlot CharacterSlot
		{
			get { return _characterSlot; }
			set
			{
				_characterSlot = value;
				//Items = Items;
			}
		}

		private Character _character;
		public Character Character
		{
			get { return _character; }
			set
			{
				if (_character != null)
				{
					_character.ItemsChanged -= new EventHandler(_character_ItemsChanged);
				}
				_character = value;
				if (_character != null)
				{
					_character.ItemsChanged += new EventHandler(_character_ItemsChanged);
					SelectedItem = _character[CharacterSlot];
				}
			}
		}

		void _character_ItemsChanged(object sender, EventArgs e)
		{
			if (CharacterSlot == Character.CharacterSlot.OffHand)
			{
				_dimIcon = Character[Character.CharacterSlot.MainHand] != null &&
					Character[Character.CharacterSlot.MainHand].Slot == Item.ItemSlot.TwoHand;
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

		public void UpdateSelectedItem()
		{
			if (Character != null)
			{
				_selectedItem = Character[CharacterSlot];
			}
			this.Text = string.Empty;
			this.ItemIcon = _selectedItem != null ? ItemIcons.GetItemIcon(_selectedItem) : null;
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
				if (Character != null)
					Character[CharacterSlot] = value;
				else
					_selectedItem = value;
				UpdateSelectedItem();
				//foreach (ToolStripMenuItem menuItem in menu.Items)
				//	menuItem.Checked = (menuItem.Tag == _selectedItem);

			}
		}

		private bool _dimIcon = false;
		private Image _itemIcon = null;
		public Image ItemIcon
		{
			get { return _itemIcon; }
			set
			{
				_itemIcon = value;
				if (_dimIcon && value != null)
					this.Image = GetDimIcon(value);
				else
					this.Image = value;
			}
		}

		private Image GetDimIcon(Image icon)
		{
			Bitmap bmp = new Bitmap(icon.Width, icon.Height);
			Graphics g = Graphics.FromImage(bmp);
			System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
			System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix();
			cm.Matrix33 = 0.5f;
			ia.SetColorMatrix(cm);
			g.DrawImage(icon, new Rectangle(0, 0, icon.Width, icon.Height), 0, 0, icon.Width, icon.Height, GraphicsUnit.Pixel, ia);
			g.Dispose();
			ia.Dispose();
			return bmp;
		}

		//public Item[] _sortedItems = new Item[0];
		//public Item[] _items = new Item[0];
		//public Item[] Items
		//{
		//    get
		//    {
		//        return _items;
		//    }
		//    set
		//    {
		//        _items = value;
		//        if (_items == null) _items = new Item[0];
				
		//        //_items = new List<Item>(sortedItems.Values).ToArray();
		//        SortedList<string, Item> sortedItems = new SortedList<string, Item>();
		//        foreach (Item item in _items)
		//            if (item.FitsInSlot(CharacterSlot))
		//                sortedItems.Add(item.Name + item.GemmedId + item.GetHashCode().ToString(), item);
		//        _sortedItems = new List<Item>(sortedItems.Values).ToArray();

		//        //menu.Items.Clear();
		//        //ToolStripMenuItem menuItem = new ToolStripMenuItem("None");
		//        //menuItem.Height = 36;
		//        //menuItem.Click += new EventHandler(menuItem_Click);
		//        //menuItem.Checked = (SelectedItem == null);
		//        //menu.Items.Add(menuItem);
		//        //foreach (Item item in sortedItems.Values)
		//        //{
		//        //    //menu.Items.Add(item.ToString(), ItemIcons.GetItemIcon(item, true));
		//        //    menuItem = new ToolStripMenuItem(item.ToString(), ItemIcons.GetItemIcon(item, true));
		//        //    menuItem.Height = 36;
		//        //    menuItem.ImageScaling = ToolStripItemImageScaling.None;
		//        //    menuItem.Click += new EventHandler(menuItem_Click);
		//        //    menuItem.Tag = item;
		//        //    menuItem.Checked = (SelectedItem == item);
		//        //    menu.Items.Add(menuItem);
		//        //}
		//        SelectedItem = _selectedItem;
		//    }
		//}

		//void menuItem_Click(object sender, EventArgs e)
		//{
		//    SelectedItem = (sender as ToolStripMenuItem).Tag as Item;
		//}
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
