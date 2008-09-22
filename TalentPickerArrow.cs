using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Rawr
{
	public class TalentPickerArrow : System.Windows.Forms.PictureBox
	{
		public TalentPickerArrow() { }
		public TalentPickerArrow(TalentPickerItem item, TalentPickerItem prerequisiteItem)
		{
			this.BackColor = Color.Transparent;
			Item = item;
			PrerequisiteItem = prerequisiteItem;
			UpdateArrow();
		}

		public TalentPickerItem Item { get; set; }
		public TalentPickerItem PrerequisiteItem { get; set; }
		
		private PictureBox _itemArrow = null;
		public PictureBox ItemArrow
		{
			get
			{
				if (_itemArrow == null)
				{
					_itemArrow = new PictureBox();
					_itemArrow.BackColor = Color.Transparent;
					Item.OverlayPanel.Controls.Add(_itemArrow);
				}
				return _itemArrow;
			}
		}
		private PictureBox _prerequisiteItemArrow = null;
		public PictureBox PrerequisiteItemArrow
		{
			get
			{
				if (_prerequisiteItemArrow == null)
				{
					_prerequisiteItemArrow = new PictureBox();
					_prerequisiteItemArrow.BackColor = Color.Transparent;
					PrerequisiteItem.OverlayPanel.Controls.Add(_prerequisiteItemArrow);
				}
				return _prerequisiteItemArrow;
			}
		}
		
		public void UpdateArrow()
		{
			int offsetX = Item.Column - PrerequisiteItem.Column;
			int offsetY = Item.Row - PrerequisiteItem.Row;
			int color = -1; //-1=Grey, 0=Green, 1=Yellow
			if (Item.Available)
			{
				if (Item.CurrentRank == 0) color = 0;
				else color = 1;
			}

			if (offsetX == 0)
			{
				if (offsetY == 1)
				{
					if (color == -1) LoadImage(Properties.Resources.down_1_grey);
					else if (color == 0) LoadImage(Properties.Resources.down_1_green);
					else if (color == 1) LoadImage(Properties.Resources.down_1_yellow);
				}
				else if (offsetY == 2)
				{
					if (color == -1) LoadImage(Properties.Resources.down_2_grey);
					else if (color == 0) LoadImage(Properties.Resources.down_2_green);
					else if (color == 1) LoadImage(Properties.Resources.down_2_yellow);
				}
				else if (offsetY == 3)
				{
					if (color == -1) LoadImage(Properties.Resources.down_3_grey);
					else if (color == 0) LoadImage(Properties.Resources.down_3_green);
					else if (color == 1) LoadImage(Properties.Resources.down_3_yellow);
				}
				else if (offsetY == 4)
				{
					if (color == -1) LoadImage(Properties.Resources.down_4_grey);
					else if (color == 0) LoadImage(Properties.Resources.down_4_green);
					else if (color == 1) LoadImage(Properties.Resources.down_4_yellow);
				}
				SetLocation(13, 47, offsetX, offsetY);
			}
			else if (offsetX == 1)
			{
				if (offsetY == 0)
				{
					if (color == -1) LoadImage(Properties.Resources.across_right_grey);
					else if (color == 0) LoadImage(Properties.Resources.across_right_green);
					else if (color == 1) LoadImage(Properties.Resources.across_right_yellow);
					SetLocation(45, 17, offsetX, offsetY);
				}
				else if (offsetY == 1)
				{
					if (color == -1) LoadImage(Properties.Resources.down_right_grey);
					else if (color == 0) LoadImage(Properties.Resources.down_right_green);
					else if (color == 1) LoadImage(Properties.Resources.down_right_yellow);
					SetLocation(45, 20, offsetX, offsetY);
				}
				else if (offsetY == 2)
				{
					if (color == -1) LoadImage(Properties.Resources.down_2_right_grey);
					else if (color == 0) LoadImage(Properties.Resources.down_2_right_green);
					else if (color == 1) LoadImage(Properties.Resources.down_2_right_yellow);
					SetLocation(45, 20, offsetX, offsetY);
				}
			}
			else if (offsetX == -1)
			{
				if (offsetY == 0)
				{
					if (color == -1) LoadImage(Properties.Resources.across_left_grey);
					else if (color == 0) LoadImage(Properties.Resources.across_left_green);
					else if (color == 1) LoadImage(Properties.Resources.across_left_yellow);
					SetLocation(-22, 14, offsetX, offsetY);
				}
				else if (offsetY == 1)
				{
					if (color == -1) LoadImage(Properties.Resources.down_left_grey);
					else if (color == 0) LoadImage(Properties.Resources.down_left_green);
					else if (color == 1) LoadImage(Properties.Resources.down_left_yellow);
					SetLocation(-51, 20, offsetX, offsetY);
				}
				else if (offsetY == 2)
				{
					if (color == -1) LoadImage(Properties.Resources.down_2_left_grey);
					else if (color == 0) LoadImage(Properties.Resources.down_2_left_green);
					else if (color == 1) LoadImage(Properties.Resources.down_2_left_yellow);
					SetLocation(-51, 20, offsetX, offsetY);
				}
			}

		}

		private void SetLocation(int x, int y, int offsetX, int offsetY)
		{
			this.Location = new Point(PrerequisiteItem.Left + x, PrerequisiteItem.Top + y);
			this.PrerequisiteItemArrow.Location = new Point(x, y);
			this.ItemArrow.Location = new Point(x - (offsetX * 63), y - (offsetY * 65));
		}

		private void LoadImage(Image img)
		{
			if (this.Image != img)
			{
				this.Size = this.ItemArrow.Size = this.PrerequisiteItemArrow.Size = img.Size;
				this.Image = this.ItemArrow.Image = this.PrerequisiteItemArrow.Image = img;
			}
		}
	}
}
