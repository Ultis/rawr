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

		private ItemCalculation[] _itemCalculations;
		public ItemCalculation[] ItemCalculations
		{
			get
			{
				return _itemCalculations;
			}
			set
			{
				if (value == null)
				{
					_itemCalculations = new ItemCalculation[0];
				}
				else
				{
					List<ItemCalculation> calcs = new List<ItemCalculation>(value);
					calcs.Sort(new System.Comparison<ItemCalculation>(CompareItemCalculations));
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

		protected int CompareItemCalculations(ItemCalculation a, ItemCalculation b)
		{
			if (Sort == ComparisonGraph.ComparisonSort.Mitigation)
				return a.MitigationPoints.CompareTo(b.MitigationPoints);
			else if (Sort == ComparisonGraph.ComparisonSort.Survival)
				return a.SurvivalPoints.CompareTo(b.SurvivalPoints);
			else if (Sort == ComparisonGraph.ComparisonSort.Alphabetical)
				return b.ItemName.CompareTo(a.ItemName);
			else
				return a.OverallPoints.CompareTo(b.OverallPoints);
		}

		public FormItemSelection()
		{
			InitializeComponent();
			this.Activated += new EventHandler(FormItemSelection_Activated);
		}

		void FormItemSelection_Activated(object sender, EventArgs e)
		{
			panelItems.AutoScrollPosition = new Point(0, 0);
		}

		private void CheckToHide(object sender, EventArgs e)
		{
			this.Hide();
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
			if (location.X < System.Windows.Forms.SystemInformation.WorkingArea.Left)
				location.X = System.Windows.Forms.SystemInformation.WorkingArea.Left;
			if (location.Y < System.Windows.Forms.SystemInformation.WorkingArea.Top)
				location.Y = System.Windows.Forms.SystemInformation.WorkingArea.Top;
			if (location.X + this.Width > System.Windows.Forms.SystemInformation.WorkingArea.Right)
				location.X = ctrlScreen.X - this.Width;
			if (location.Y + this.Height > System.Windows.Forms.SystemInformation.WorkingArea.Bottom)
				location.Y = ctrlScreen.Y + ctrl.Height - this.Height;
			this.Location = location;
		}

		private void sortToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			foreach (ToolStripItem item in toolStripDropDownButtonSort.DropDownItems)
			{
				if (item is ToolStripMenuItem)
				{
					(item as ToolStripMenuItem).Checked = item == sender;
					if ((item as ToolStripMenuItem).Checked)
					{
						toolStripDropDownButtonSort.Text = item.Text;
					}
				}
			}
			this.Sort = (ComparisonGraph.ComparisonSort)Enum.Parse(typeof(ComparisonGraph.ComparisonSort), toolStripDropDownButtonSort.Text);
			this.Cursor = Cursors.Default;
		}

		private Character.CharacterSlot _characterSlot = (Character.CharacterSlot)(-1);
		private ItemButton _button;
		public void LoadGearBySlot(Character.CharacterSlot slot)
		{
			if (slot != _characterSlot)
			{
				_characterSlot = slot;
				List<ItemCalculation> itemCalculations = new List<ItemCalculation>();
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
				ItemCalculation calc = ItemCalculations[i];
				calc.Equipped = calc.Item == _button.SelectedItem;
				ctrl.ItemCalculation = calc;
				ctrl.Sort = this.Sort;
				ctrl.HideToolTip();
				bool visible = string.IsNullOrEmpty(this.toolStripTextBoxFilter.Text) || calc.ItemName.ToLower().Contains(this.toolStripTextBoxFilter.Text.ToLower());
				ctrl.Visible = visible;
				if (visible)
					maxRating = Math.Max(maxRating,
						(Sort == ComparisonGraph.ComparisonSort.Mitigation ? calc.MitigationPoints :
						(Sort == ComparisonGraph.ComparisonSort.Survival ? calc.SurvivalPoints :
						calc.OverallPoints)));
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
			_characterSlot = (Character.CharacterSlot)(-1);
			this.Hide();
		}
	}
}