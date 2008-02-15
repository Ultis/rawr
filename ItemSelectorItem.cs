using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class ItemSelectorItem : UserControl
	{
		public ItemSelectorItem()
		{
			InitializeComponent();
			this.Dock = DockStyle.Top;

			//Ugh, this is sloppy, but I'm tired at the moment.
			foreach (Control a in this.Controls)
			{
				a.MouseMove += new MouseEventHandler(ItemSelectorItem_MouseEnterLeave);
				a.MouseLeave += new EventHandler(ItemSelectorItem_MouseEnterLeave);
				a.MouseClick += new MouseEventHandler(ItemSelectorItem_MouseClick);
				foreach (Control b in a.Controls)
			    {
					b.MouseMove += new MouseEventHandler(ItemSelectorItem_MouseEnterLeave);
					b.MouseLeave += new EventHandler(ItemSelectorItem_MouseEnterLeave);
					b.MouseClick += new MouseEventHandler(ItemSelectorItem_MouseClick);
					foreach (Control c in b.Controls)
			        {
						c.MouseMove += new MouseEventHandler(ItemSelectorItem_MouseEnterLeave);
						c.MouseLeave += new EventHandler(ItemSelectorItem_MouseEnterLeave);
						c.MouseClick += new MouseEventHandler(ItemSelectorItem_MouseClick);
					}
			    }
			}

			Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
			CreateSubPointPanels();
		}

		void Calculations_ModelChanged(object sender, EventArgs e)
		{
			CreateSubPointPanels();
		}

		void ItemSelectorItem_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				(FindForm() as FormItemSelection).Select(_itemCalculation.Item);
			}
			else if (e.Button == MouseButtons.Right)
			{
				ItemContextualMenu.Instance.Show(ItemCalculation.Item, Character.CharacterSlot.None, !ItemCalculation.Equipped);
			}
		}

		private bool _tooltipShown = false;
		Rectangle rectMouseTest;
		void ItemSelectorItem_MouseEnterLeave(object sender, MouseEventArgs e) { ItemSelectorItem_MouseEnterLeave(sender, (EventArgs)e); }
		void ItemSelectorItem_MouseEnterLeave(object sender, EventArgs e)
		{
			rectMouseTest = new Rectangle(2, 2, this.Width - 4, this.Height - 4);
			if (rectMouseTest.Contains(this.PointToClient(Control.MousePosition)))
				ShowToolTip();
			else
				HideToolTip();
		}

		public void HideToolTip()
		{
			if (_tooltipShown && !IsDisposed)
			{
				_tooltipShown = false;
				UpdateBackColors();
			}
		}

		public void ShowToolTip()
		{
			if (!_tooltipShown && !IsDisposed)
			{
				_tooltipShown = true;
				int tipX = this.Width + 20;
				if (Parent.PointToScreen(Location).X + tipX + 249 > System.Windows.Forms.Screen.GetWorkingArea(this).Right)
					tipX = -249;
				ItemToolTip.Instance.Show(_itemCalculation.Item, this, new Point(tipX, 0));

				UpdateBackColors();
				
				foreach (ItemSelectorItem ctrl in Parent.Controls)
					if (ctrl != this && !ctrl.IsDisposed)
						ctrl.HideToolTip();
			}
		}

		private void UpdateBackColors()
		{
			if (_tooltipShown)
			{
				panelBorder.BackColor = Color.FromArgb(128, 128, 255);
				panelLeft.BackColor = panelCenter.BackColor = panelRight.BackColor =
					panelBottom.BackColor = Color.FromArgb(192, 192, 255);
			}
			else
			{
				panelBorder.BackColor = panelLeft.BackColor = panelCenter.BackColor = panelRight.BackColor =
					panelBottom.BackColor = (_itemCalculation.Equipped ? Color.FromArgb(212, 212, 255) : SystemColors.Control);
			}
		}

		public Character.CharacterSlot CharacterSlot;
		private ComparisonCalculationBase _itemCalculation;
		public ComparisonCalculationBase ItemCalculation
		{
			get { return _itemCalculation; }
			set
			{
				if (_itemCalculation != value)
				{
					_itemCalculation = value;
					UpdateBackColors();
					_sort = (ComparisonGraph.ComparisonSort)(-1);
					Item item = _itemCalculation.Item;
					pictureBoxIcon.Image = ItemIcons.GetItemIcon(item.IconPath);
					labelName.Text = item.Name;
					panelRight.Visible = item.Gem1 != null || item.Gem2 != null || item.Gem3 != null;
					panelRight.Width = (item.Gem3 != null ? 98 : (item.Gem2 != null ? 67 : 36));
					switch (item.Sockets.Color1)
					{
						case Item.ItemSlot.Meta:
							panelGem1.BackColor = Color.Silver;
							break;
						case Item.ItemSlot.Red:
							panelGem1.BackColor = Color.Red;
							break;
						case Item.ItemSlot.Yellow:
							panelGem1.BackColor = Color.Yellow;
							break;
						case Item.ItemSlot.Blue:
							panelGem1.BackColor = Color.Blue;
							break;
					}
					switch (item.Sockets.Color2)
					{
						case Item.ItemSlot.Meta:
							panelGem2.BackColor = Color.Silver;
							break;
						case Item.ItemSlot.Red:
							panelGem2.BackColor = Color.Red;
							break;
						case Item.ItemSlot.Yellow:
							panelGem2.BackColor = Color.Yellow;
							break;
						case Item.ItemSlot.Blue:
							panelGem2.BackColor = Color.Blue;
							break;
					}
					switch (item.Sockets.Color3)
					{
						case Item.ItemSlot.Meta:
							panelGem3.BackColor = Color.Silver;
							break;
						case Item.ItemSlot.Red:
							panelGem3.BackColor = Color.Red;
							break;
						case Item.ItemSlot.Yellow:
							panelGem3.BackColor = Color.Yellow;
							break;
						case Item.ItemSlot.Blue:
							panelGem3.BackColor = Color.Blue;
							break;
					}
					if (item.Gem1 != null) pictureBoxGem1.Image = ItemIcons.GetItemIcon(item.Gem1);
					if (item.Gem2 != null) pictureBoxGem2.Image = ItemIcons.GetItemIcon(item.Gem2);
					if (item.Gem3 != null) pictureBoxGem3.Image = ItemIcons.GetItemIcon(item.Gem3);
				}
			}
		}

		private ComparisonGraph.ComparisonSort _sort = (ComparisonGraph.ComparisonSort)(-1);
		public ComparisonGraph.ComparisonSort Sort
		{
			get { return _sort; }
			set
			{
				if (_sort != value)
				{
					_sort = value;
					foreach (Panel panelSubPoint in panelBottom.Controls)
					{
						panelSubPoint.Visible = (int)_sort == (int)panelSubPoint.Tag || _sort == ComparisonGraph.ComparisonSort.Overall || _sort == ComparisonGraph.ComparisonSort.Alphabetical;
					}
				}
			}
		}

		private void CreateSubPointPanels()
		{
			panelBottom.SuspendLayout();
			panelBottom.Controls.Clear();
			foreach (KeyValuePair<string, Color> subPointNameColors in Calculations.SubPointNameColors)
			{
				Panel panelSubPoint = new Panel();
				panelSubPoint.BackColor = subPointNameColors.Value;
				panelSubPoint.Tag = panelBottom.Controls.Count;
				panelSubPoint.Dock = DockStyle.Left;
				panelBottom.Controls.Add(panelSubPoint);
				panelSubPoint.BringToFront();
			}
			panelBottom.ResumeLayout();
			SetMaxRating(_lastMaxRating);
		}

		private float _lastMaxRating = 100f;
		public void SetMaxRating(float maxRating)
		{
			_lastMaxRating = maxRating;
			foreach (Panel panelSubPoint in panelBottom.Controls)
			{
				try
				{
					panelSubPoint.Width = (int)Math.Floor((float)(panelBottom.Width - panelBottom.Padding.Horizontal) * (_itemCalculation.SubPoints[(int)panelSubPoint.Tag] / maxRating));
				}
				catch { }
			}
		}
	}
}
