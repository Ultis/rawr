using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class ItemButtonWithEnchant : UserControl
	{
		public ItemButtonWithEnchant()
		{
			InitializeComponent();
			buttonEnchant.MouseEnter += new EventHandler(buttonEnchant_MouseEnter);
			buttonEnchant.MouseLeave += new EventHandler(buttonEnchant_MouseLeave);

			ItemToolTip.Instance.SetToolTip(this, "");
		}

		public override string Text { get { return itemButtonItem.Text; } set { itemButtonItem.Text = value; } }
		public Character.CharacterSlot CharacterSlot { get { return itemButtonItem.CharacterSlot; } set { itemButtonItem.CharacterSlot = value; } }
		public Item SelectedItem { get { return itemButtonItem.SelectedItem; } set { itemButtonItem.SelectedItem = value; } }
		public int SelectedItemId { get { return itemButtonItem.SelectedItemId; } set { itemButtonItem.SelectedItemId = value; } }
		public Image ItemIcon { get { return itemButtonItem.ItemIcon; } set { itemButtonItem.ItemIcon = value; } }
		public bool UseVisualStyleBackColor { get { return itemButtonItem.UseVisualStyleBackColor; } set { itemButtonItem.UseVisualStyleBackColor = value; } }
		
		private Character _character;
		public Character Character
		{
			get { return _character; }
			set
			{
				if (_character != null)
				{
					_character.CalculationsInvalidated -= new EventHandler(_character_CalculationsInvalidated);
				}
				_character = itemButtonItem.Character = value;
				if (_character != null)
				{
					_character.CalculationsInvalidated += new EventHandler(_character_CalculationsInvalidated);
					SelectedEnchant = _character.GetEnchantBySlot(CharacterSlot);
				}
			}
		}

		void _character_CalculationsInvalidated(object sender, EventArgs e)
		{
			UpdateSelectedItem();
		}
		public int SelectedEnchantId
		{
			get
			{
				if (SelectedEnchant == null) return 0;
				else return SelectedEnchant.Id;
			}
			set
			{
				if (value == 0) SelectedEnchant = null;
				else SelectedEnchant = Enchant.FindEnchant(value, Item.GetItemSlotByCharacterSlot(CharacterSlot), Character);
			}
		}
		private Enchant _selectedEnchant;
		public Enchant SelectedEnchant
		{
			get
			{
				return _selectedEnchant;
			}
			set
			{
				if (Character != null)
					Character.SetEnchantBySlot(CharacterSlot, value);
				else
					_selectedEnchant = value;
				UpdateSelectedItem();
			}
		}

		public void UpdateSelectedItem()
		{
			if (Character != null)
			{
				_selectedEnchant = Character.GetEnchantBySlot(CharacterSlot);
			}
            if (SelectedEnchant != null)
            {
                buttonEnchant.Text = SelectedEnchant.ShortName;
            }
            itemButtonItem.UpdateSelectedItem();
		}

		private void buttonEnchant_Click(object sender, EventArgs e)
		{
			(this.FindForm() as IFormItemSelectionProvider).FormItemSelection.Show(this, CharacterSlot);
		}

		void buttonEnchant_MouseLeave(object sender, EventArgs e)
		{
			ItemToolTip.Instance.Hide(this);
		}

		void buttonEnchant_MouseEnter(object sender, EventArgs e)
		{
			if (SelectedItem != null)
			{
				int tipX = this.Width;
				Item itemEnchant = new Item(SelectedEnchant.Name, Item.ItemQuality.Temp, Item.ItemType.None,
					-1 * (SelectedEnchant.Id + (10000 * (int)SelectedEnchant.Slot)), null, Item.ItemSlot.None, null, false, SelectedEnchant.Stats, new Sockets(), 0, 0, 0, 0, 0,
					Item.ItemDamageType.Physical, 0, null);
				
				if (Parent.PointToScreen(Location).X + tipX + 249 > System.Windows.Forms.Screen.GetWorkingArea(this).Right)
					tipX = -249;
				ItemToolTip.Instance.Show(itemEnchant, this, new Point(tipX, itemButtonItem.Height));
			}
		}
	}
}
