using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Rawr.Forms.Controllers;
using Rawr.Forms.Utilities;

namespace Rawr
{
	public partial class FormItemSelection : Form
	{
        private ItemSelectionController _Controller;
        private ItemButton _button;
		
		public FormItemSelection()
		{
			InitializeComponent();
            _Controller = ItemSelectionController.Instance;

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
            
            //we shouldn't need this anymore since the form is getting created new each time
            //and the cache changing while it is open is highly unlikely.
			//ItemCache.Instance.ItemsChanged += new EventHandler(ItemCache_ItemsChanged);
		}

        //void ItemCache_ItemsChanged(object sender, EventArgs e)
        //{
        //    Character.CharacterSlot characterSlot = _characterSlot;
        //    _characterSlot = Character.CharacterSlot.None;
        //    LoadGearBySlot(characterSlot);
        //}

		void FormItemSelection_Activated(object sender, EventArgs e)
		{
			panelItems.AutoScrollPosition = new Point(0, 0);
		}

		private void CheckToHide(object sender, EventArgs e)
		{
			if (Visible)
			{
                ItemToolTip.Instance.Hide(this);
                //since we are creating a new form each time, we dispose of this one so the handle gets released properly.
                this.Dispose();
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
			_Controller.LoadGearBySlot(slot);
            RebuildItemList();
            Form main = FormHelper.GetMainForm();
            if (main != null)
            {
                base.Show(main);
            }
            else
            {
                base.Show();
            }
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
			_Controller.Sort = sort;//(ComparisonGraph.ComparisonSort)Enum.Parse(typeof(ComparisonGraph.ComparisonSort), toolStripDropDownButtonSort.Text);
			this.Cursor = Cursors.Default;
		}

		private void RebuildItemList()
		{
			panelItems.SuspendLayout();
			while (panelItems.Controls.Count < _Controller.ItemCalculations.Length)
				panelItems.Controls.Add(new ItemSelectorItem());
            while (panelItems.Controls.Count > _Controller.ItemCalculations.Length)
				panelItems.Controls.RemoveAt(panelItems.Controls.Count - 1);
			float maxRating = 0;
            for (int i = 0; i < _Controller.ItemCalculations.Length; i++)
			{
				ItemSelectorItem ctrl = panelItems.Controls[i] as ItemSelectorItem;
                ComparisonCalculationBase calc = _Controller.ItemCalculations[i];
				calc.Equipped = calc.Item == _button.SelectedItem;
				ctrl.ItemCalculation = calc;
				ctrl.Sort = _Controller.Sort;
				ctrl.HideToolTip();
				bool visible = string.IsNullOrEmpty(this.toolStripTextBoxFilter.Text) || calc.Name.ToLower().Contains(this.toolStripTextBoxFilter.Text.ToLower());
				ctrl.Visible = visible;
				if (visible)
				{
					float calcRating;
                    if (_Controller.Sort == ComparisonGraph.ComparisonSort.Overall || _Controller.Sort == ComparisonGraph.ComparisonSort.Alphabetical)
						calcRating = calc.OverallPoints;
					else
                        calcRating = calc.SubPoints[(int)_Controller.Sort];
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
			_Controller.CharacterSlot = Character.CharacterSlot.None;
            CheckToHide(null,null);
		}
	}
}