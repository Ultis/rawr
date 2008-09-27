using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Rawr.Forms.Utilities;

namespace Rawr
{
	public partial class FormItemSelection : Form
	{
		private Item[] _items;
		public Item[] Items
		{
			get { return _items; }
			set { _items = value; }
		}

		private Character _character;
		public Character Character
		{
			get { return _character; }
			set
			{
				_character = value;
				_characterSlot = Character.CharacterSlot.None;
			}
		}

		private CharacterCalculationsBase _currentCalculations;
		public CharacterCalculationsBase CurrentCalculations
		{
			get { return _currentCalculations; }
			set { _currentCalculations = value; }
		}

		private ComparisonCalculationBase[] _itemCalculations;
		public ComparisonCalculationBase[] ItemCalculations
		{
			get { return _itemCalculations; }
			set
			{
				if (value == null)
				{
					_itemCalculations = new ComparisonCalculationBase[0];
				}
				else
				{
					List<ComparisonCalculationBase> calcs = new List<ComparisonCalculationBase>(value);
					calcs.Sort(new System.Comparison<ComparisonCalculationBase>(CompareItemCalculations));
					_itemCalculations = calcs.ToArray();
				}
				RebuildItemList();
			}
		}

		private ComparisonGraph.ComparisonSort _sort = ComparisonGraph.ComparisonSort.Overall;
		public ComparisonGraph.ComparisonSort Sort
		{
			get { return _sort; }
			set
			{
				_sort = value;
				ItemCalculations = ItemCalculations;
			}
		}

		protected int CompareItemCalculations(ComparisonCalculationBase a, ComparisonCalculationBase b)
		{
			if (Sort == ComparisonGraph.ComparisonSort.Overall)
				return a.OverallPoints.CompareTo(b.OverallPoints);
			else if (Sort == ComparisonGraph.ComparisonSort.Alphabetical)
				return b.Name.CompareTo(a.Name);
			else
				return a.SubPoints[(int)Sort].CompareTo(b.SubPoints[(int)Sort]);
		}

		public FormItemSelection()
		{
			InitializeComponent();

			overallToolStripMenuItem.Tag = -1;
			alphabeticalToolStripMenuItem.Tag = -2;

			foreach (string name in Calculations.SubPointNameColors.Keys)
			{
				ToolStripMenuItem toolStripMenuItemSubPoint = new ToolStripMenuItem(name);
				toolStripMenuItemSubPoint.Tag = toolStripDropDownButtonSort.DropDownItems.Count - 2;
				toolStripMenuItemSubPoint.Click += new System.EventHandler(this.sortToolStripMenuItem_Click);
				toolStripDropDownButtonSort.DropDownItems.Add(toolStripMenuItemSubPoint);
			}

			this.Activated += new EventHandler(FormItemSelection_Activated);
            
            ItemCache.Instance.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
		}

		void ItemCache_ItemsChanged(object sender, EventArgs e)
		{
			Items = ItemCache.Instance.RelevantItems;
			Character.CharacterSlot characterSlot = _characterSlot;
			_characterSlot = Character.CharacterSlot.None;
			LoadItemsBySlot(characterSlot);
		}

		void FormItemSelection_Activated(object sender, EventArgs e)
		{
			panelItems.AutoScrollPosition = new Point(0, 0);
		}

		private void CheckToHide(object sender, EventArgs e)
		{
			if (Visible)
			{
                ItemToolTip.Instance.Hide(this);
                this.Hide();
			}
		}

		private void timerForceActivate_Tick(object sender, System.EventArgs e)
		{
			this.timerForceActivate.Enabled = false;
			this.Activate();
		}

		public void ForceShow()
		{
			this.timerForceActivate.Enabled = true;
		}

		public void Show(ItemButton button, Character.CharacterSlot slot)
		{
			_button = button;
			if (_buttonEnchant != null)
			{
				_characterSlot = Character.CharacterSlot.None;
				_buttonEnchant = null;
			}
			this.SetAutoLocation(button);
			this.LoadItemsBySlot(slot);
			base.Show();
		}

		public void Show(ItemButtonWithEnchant button, Character.CharacterSlot slot)
		{
			_buttonEnchant = button;
			if (_button != null)
			{
				_characterSlot = Character.CharacterSlot.None;
				_button = null;
			}
			this.SetAutoLocation(button);
			this.LoadEnchantsBySlot(slot);
			base.Show();
		}

		public void SetAutoLocation(Control ctrl)
		{
			Point ctrlScreen = ctrl.Parent.PointToScreen(ctrl.Location);
			Point location = new Point(ctrlScreen.X + ctrl.Width, ctrlScreen.Y);
			Rectangle workingArea = System.Windows.Forms.Screen.GetWorkingArea(ctrl.Parent);
			if (location.X < workingArea.Left)
				location.X = workingArea.Left;
			if (location.Y < workingArea.Top)
				location.Y = workingArea.Top;
			if (location.X + this.Width > workingArea.Right)
				location.X = ctrlScreen.X - this.Width;
			if (location.Y + this.Height > workingArea.Bottom)
				location.Y = workingArea.Bottom - this.Height;
			this.Location = location;
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
			this.Sort = sort;//(ComparisonGraph.ComparisonSort)Enum.Parse(typeof(ComparisonGraph.ComparisonSort), toolStripDropDownButtonSort.Text);
			this.Cursor = Cursors.Default;
		}

		private Character.CharacterSlot _characterSlot = Character.CharacterSlot.None;
		private ItemButton _button;
		private ItemButtonWithEnchant _buttonEnchant;
		public void LoadItemsBySlot(Character.CharacterSlot slot)
		{
			if (_characterSlot != slot)
			{
				_characterSlot = slot;
				List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
				if (this.Items != null && this.Character != null)
				{
					foreach (Item item in this.Items)
					{
						if (item.FitsInSlot(slot))
						{
							itemCalculations.Add(Calculations.GetItemCalculations(item, this.Character, slot));
						}
					}
				}
				itemCalculations.Sort(new System.Comparison<ComparisonCalculationBase>(CompareItemCalculations));
				ItemCalculations = itemCalculations.ToArray();
			}
		}

		private void LoadEnchantsBySlot(Character.CharacterSlot slot)
		{
			if (_characterSlot != slot)
			{
				_characterSlot = slot;
				List<ComparisonCalculationBase> itemCalculations = null;
				if (Character != null && CurrentCalculations != null)
				{
					itemCalculations = Calculations.GetEnchantCalculations(Item.GetItemSlotByCharacterSlot(slot), Character, CurrentCalculations);
				}
				itemCalculations.Sort(new System.Comparison<ComparisonCalculationBase>(CompareItemCalculations));
				ItemCalculations = itemCalculations.ToArray();
			}
		}

		private List<ItemSelectorItem> _itemSelectorItems = new List<ItemSelectorItem>();
		private void RebuildItemList()
		{
            if (this.InvokeRequired)
            {
                InvokeHelper.Invoke(this, "RebuildItemList", null);
                return;
            }
			panelItems.SuspendLayout();

			////Replaced this...
			//while (panelItems.Controls.Count < this.ItemCalculations.Length)
			//	panelItems.Controls.Add(new ItemSelectorItem());
			//while (panelItems.Controls.Count > this.ItemCalculations.Length)
			//	panelItems.Controls.RemoveAt(panelItems.Controls.Count - 1);

			////...with this, so as not to constantly create and remove lots of controls
			int currentItemLength = panelItems.Controls.Count;
			int targetItemLength = this.ItemCalculations.Length;
			if (currentItemLength < targetItemLength)
			{
				int itemSelectorsToCreate = targetItemLength - _itemSelectorItems.Count;
				for (int i = 0; i < itemSelectorsToCreate; i++)
					_itemSelectorItems.Add(new ItemSelectorItem());
				for (int i = currentItemLength; i < targetItemLength; i++)
					panelItems.Controls.Add(_itemSelectorItems[i]);
			}
			else if (currentItemLength > targetItemLength)
			{
				for (int i = currentItemLength; i > targetItemLength; i--)
					panelItems.Controls.RemoveAt(i - 1);
			}

			float maxRating = 0;
            for (int i = 0; i < this.ItemCalculations.Length; i++)
			{
				ItemSelectorItem ctrl = panelItems.Controls[i] as ItemSelectorItem;
                ComparisonCalculationBase calc = this.ItemCalculations[i];
				if (_button != null)
				{
					calc.Equipped = calc.Item == _button.SelectedItem;
					ctrl.IsEnchant = false;

				}
				if (_buttonEnchant != null)
				{
					calc.Equipped = Math.Abs(calc.Item.Id % 10000) == _buttonEnchant.SelectedEnchantId;
					ctrl.IsEnchant = true;
				}
				ctrl.ItemCalculation = calc;
				ctrl.Sort = this.Sort;
				ctrl.HideToolTip();
				bool visible = string.IsNullOrEmpty(this.toolStripTextBoxFilter.Text) || calc.Name.ToLower().Contains(this.toolStripTextBoxFilter.Text.ToLower());
				ctrl.Visible = visible;
				if (visible)
				{
					float calcRating;
                    if (Sort == ComparisonGraph.ComparisonSort.Overall || this.Sort == ComparisonGraph.ComparisonSort.Alphabetical)
						calcRating = calc.OverallPoints;
					else
                        calcRating = calc.SubPoints[(int)Sort];
					maxRating = Math.Max(maxRating, calcRating);
				}
			}
			panelItems.ResumeLayout(true);
			foreach (ItemSelectorItem ctrl in panelItems.Controls)
				ctrl.MaxRating = maxRating;
		}

		private void toolStripTextBoxFilter_TextChanged(object sender, EventArgs e)
		{
			RebuildItemList();
		}

		public void Select(Item item)
		{
			if (_button != null)
				_button.SelectedItem = item;
			if (_buttonEnchant != null)
				_buttonEnchant.SelectedEnchantId = Math.Abs(item.Id % 10000);
			_characterSlot = Character.CharacterSlot.None;
			ItemToolTip.Instance.Hide(this);
			this.Hide();
		}
	}

	public interface IFormItemSelectionProvider
	{
		FormItemSelection FormItemSelection { get; }
	}
}