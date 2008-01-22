using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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

		private static FormItemSelection _instance = null;
		public static FormItemSelection Instance
		{
			get
			{
				if (_instance == null)
					_instance = new FormItemSelection();
				return _instance;
			}
		}

		private Character _character;
		public Character Character
		{
			get { return _character; }
			set { _character = value; }
		}

		private ComparisonCalculationBase[] _itemCalculations;
		public ComparisonCalculationBase[] ItemCalculations
		{
			get
			{
				return _itemCalculations;
			}
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

			ItemCache.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
		}

		void ItemCache_ItemsChanged(object sender, EventArgs e)
		{
			Character.CharacterSlot characterSlot = _characterSlot;
			_characterSlot = Character.CharacterSlot.None;
			LoadGearBySlot(characterSlot);
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
			this.SetAutoLocation(button);
			this.LoadGearBySlot(slot);
			base.Show();
		}

		public void SetAutoLocation(Control ctrl)
		{
			Point ctrlScreen = ctrl.Parent.PointToScreen(ctrl.Location);
			Point location = new Point(ctrlScreen.X + ctrl.Width, ctrlScreen.Y);
			Rectangle workingArea = System.Windows.Forms.Screen.GetWorkingArea(this);
			if (location.X < workingArea.Left)
				location.X = workingArea.Left;
			if (location.Y < workingArea.Top)
				location.Y = workingArea.Top;
			if (location.X + this.Width > workingArea.Right)
				location.X = ctrlScreen.X - this.Width;
			if (location.Y + this.Height > workingArea.Bottom)
				location.Y = ctrlScreen.Y + ctrl.Height - this.Height;
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
		public void LoadGearBySlot(Character.CharacterSlot slot)
		{
			if (slot != _characterSlot)
			{
				_characterSlot = slot;
				List<ComparisonCalculationBase> itemCalculations = new List<ComparisonCalculationBase>();
				if (Items != null && Character != null)
				{
					foreach (Item item in Items)
					{
						if (item.FitsInSlot(slot))
						{
							itemCalculations.Add(Calculations.GetItemCalculations(item, Character, slot));
						}
					}
				}

				this.ItemCalculations = itemCalculations.ToArray();
			}
		}

		private void RebuildItemList()
		{
			panelItems.SuspendLayout();
			while (panelItems.Controls.Count < ItemCalculations.Length)
				panelItems.Controls.Add(new ItemSelectorItem());
			while (panelItems.Controls.Count > ItemCalculations.Length)
				panelItems.Controls.RemoveAt(panelItems.Controls.Count - 1);
			float maxRating = 0;
			for (int i = 0; i < ItemCalculations.Length; i++)
			{
				ItemSelectorItem ctrl = panelItems.Controls[i] as ItemSelectorItem;
				ComparisonCalculationBase calc = ItemCalculations[i];
				calc.Equipped = calc.Item == _button.SelectedItem;
				ctrl.ItemCalculation = calc;
				ctrl.Sort = this.Sort;
				ctrl.HideToolTip();
				bool visible = string.IsNullOrEmpty(this.toolStripTextBoxFilter.Text) || calc.Name.ToLower().Contains(this.toolStripTextBoxFilter.Text.ToLower());
				ctrl.Visible = visible;
				if (visible)
				{
					float calcRating;
					if (Sort == ComparisonGraph.ComparisonSort.Overall || Sort == ComparisonGraph.ComparisonSort.Alphabetical)
						calcRating = calc.OverallPoints;
					else
						calcRating = calc.SubPoints[(int)Sort];
					maxRating = Math.Max(maxRating, calcRating);
				}
			}
			panelItems.ResumeLayout(true);
			foreach (ItemSelectorItem ctrl in panelItems.Controls)
				ctrl.SetMaxRating(maxRating);
		}

		private void toolStripTextBoxFilter_TextChanged(object sender, EventArgs e)
		{
			RebuildItemList();
		}

		public void Select(Item item)
		{
			_button.SelectedItem = item;
			_characterSlot = Character.CharacterSlot.None;
			ItemToolTip.Instance.Hide(this);
			this.Hide();
		}
	}
}