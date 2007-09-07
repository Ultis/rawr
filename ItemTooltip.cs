using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class ItemTooltip : UserControl
	{
		public ItemTooltip()
		{
			InitializeComponent();
		}

		private void ItemTooltip_Load(object sender, EventArgs e)
		{

		}

		public void SetItem(Item item)
		{
			if (item != null)
			{
				labelName.Text = item.Name;
				labelArmor.Text = string.Format(labelArmor.Tag.ToString(), item.Stats.Armor);
				labelStamina.Text = string.Format(labelStamina.Tag.ToString(), item.Stats.Stamina);
				labelAgility.Text = string.Format(labelAgility.Tag.ToString(), item.Stats.Agility);
				labelDodge.Text = string.Format(labelDodge.Tag.ToString(), item.Stats.DodgeRating);
				labelDefense.Text = string.Format(labelDefense.Tag.ToString(), item.Stats.DefenseRating);
				labelResilience.Text = string.Format(labelResilience.Tag.ToString(), item.Stats.Resilience);

				if (item.Sockets.Color1 != Item.ItemSlot.None || item.Sockets.Color2 != Item.ItemSlot.None
					|| item.Sockets.Color3 != Item.ItemSlot.None)
				{
					this.Height = 115;
					this.labelSocketBonus.Text = string.Format(labelSocketBonus.Tag.ToString(),
						item.Sockets.Stats.ToString().Length == 0 ? "None" : item.Sockets.Stats.ToString());
					if (Item.GemMatchesSlot(item.Gem1, item.Sockets.Color1) &&
						Item.GemMatchesSlot(item.Gem2, item.Sockets.Color2) &&
						Item.GemMatchesSlot(item.Gem3, item.Sockets.Color3))
						labelSocketBonus.ForeColor = SystemColors.ControlText;
					else
						labelSocketBonus.ForeColor = SystemColors.GrayText;

					if (item.Sockets.Color1 != Item.ItemSlot.None)
					{
						panelGem1.Visible = true;
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

						if (item.Gem1 != null)
						{
							pictureBoxGem1.Visible = true;
							pictureBoxGem1.Image = ItemIcons.GetItemIcon(item.Gem1, true);

							string[] stats = item.Gem1.Stats.ToString().Split(',');
							if (stats.Length > 0)
							{
								labelGem1StatA.Visible = true;
								labelGem1StatA.Text = stats[0].Trim();
							}
							else
								labelGem1StatA.Visible = false;

							if (stats.Length > 1)
							{
								labelGem1StatB.Visible = true;
								labelGem1StatB.Text = stats[1].Trim();
							}
							else
								labelGem1StatB.Visible = false;

						}
						else
							pictureBoxGem1.Visible = labelGem1StatA.Visible = labelGem1StatB.Visible = false;
					}
					else
						panelGem1.Visible = labelGem1StatA.Visible = labelGem1StatB.Visible = false;

					if (item.Sockets.Color2 != Item.ItemSlot.None)
					{
						panelGem2.Visible = true;
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

						if (item.Gem2 != null)
						{
							pictureBoxGem2.Visible = true;
							pictureBoxGem2.Image = ItemIcons.GetItemIcon(item.Gem2, true);

							string[] stats = item.Gem2.Stats.ToString().Split(',');
							if (stats.Length > 0)
							{
								labelGem2StatA.Visible = true;
								labelGem2StatA.Text = stats[0].Trim();
							}
							else
								labelGem2StatA.Visible = false;

							if (stats.Length > 1)
							{
								labelGem2StatB.Visible = true;
								labelGem2StatB.Text = stats[1].Trim();
							}
							else
								labelGem2StatB.Visible = false;

						}
						else
							pictureBoxGem2.Visible = labelGem2StatA.Visible = labelGem2StatB.Visible = false;
					}
					else
						panelGem2.Visible = labelGem2StatA.Visible = labelGem2StatB.Visible = false;

					if (item.Sockets.Color3 != Item.ItemSlot.None)
					{
						panelGem3.Visible = true;
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

						if (item.Gem3 != null)
						{
							pictureBoxGem3.Visible = true;
							pictureBoxGem3.Image = ItemIcons.GetItemIcon(item.Gem3, true);

							string[] stats = item.Gem3.Stats.ToString().Split(',');
							if (stats.Length > 0)
							{
								labelGem3StatA.Visible = true;
								labelGem3StatA.Text = stats[0].Trim();
							}
							else
								labelGem3StatA.Visible = false;

							if (stats.Length > 1)
							{
								labelGem3StatB.Visible = true;
								labelGem3StatB.Text = stats[1].Trim();
							}
							else
								labelGem3StatB.Visible = false;

						}
						else
							pictureBoxGem3.Visible = labelGem3StatA.Visible = labelGem3StatB.Visible = false;
					}
					else
						panelGem3.Visible = labelGem3StatA.Visible = labelGem3StatB.Visible = false;
				}
				else
					this.Height = 57;

				if (this.Parent.ClientRectangle.Width < this.Right + 2)
				{
					int distanceToMove = this.Right - this.Parent.ClientRectangle.Width + 2;
					this.Left -= distanceToMove;
					this.Top += distanceToMove;
				}
				if (this.Parent.ClientRectangle.Height < this.Bottom + 2)
					this.Top -= this.Bottom - this.Parent.ClientRectangle.Height + 2;
				this.BringToFront();
				this.Visible = true;
			}
		}
	}
}
