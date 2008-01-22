using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class ComparisonGraph : UserControl, IItemProvider
	{
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
				if (_prerenderedGraph != null) _prerenderedGraph.Dispose();
				_prerenderedGraph = null;
				this.Invalidate();
			}
		}

		private bool _roundValues = true;
		public bool RoundValues
		{
			get { return _roundValues; }
			set { _roundValues = value; }
		}

		private ComparisonSort _sort = ComparisonSort.Overall;
		public ComparisonSort Sort
		{
			get { return _sort; }
			set
			{
				_sort = value;
				ItemCalculations = ItemCalculations;
			}
		}

		public Point GetTooltipLocation(Point offset)
		{
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

			//if (offset != Point.Empty)
			//{
			System.Drawing.Point p = (this.Parent.PointToScreen(this.Location)); //this.FindForm().PointToClient
			p.X += 2 + offset.X;
			p.Y += 2 + offset.Y;
			return p;
			//tooltip.Location = p;
			//}
			//tooltip.Visible = false;

			//return tooltip;
		}

		protected int CompareItemCalculations(ComparisonCalculationBase a, ComparisonCalculationBase b)
		{
			if (Sort == ComparisonSort.Overall)
				return b.OverallPoints.CompareTo(a.OverallPoints);
			else if (Sort == ComparisonSort.Alphabetical)
				return a.Name.CompareTo(b.Name);
			else
				return b.SubPoints[(int)Sort].CompareTo(a.SubPoints[(int)Sort]);
		}

		private VScrollBar _scrollBar;
		public VScrollBar ScrollBar
		{
			get { return _scrollBar; }
			set
			{
				_scrollBar = value;
				_scrollBar.Scroll += new ScrollEventHandler(_scrollBar_Scroll);
			}
		}

		public enum ComparisonSort
		{
			//SubPoints will be their index, such as 0 or 1
			Overall = -1, 
			Alphabetical = -2
		}

		void _scrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			this.Invalidate();
		}

		private Bitmap _prerenderedGraph = null;
		public Bitmap PrerenderedGraph
		{
			get
			{
				if (_prerenderedGraph == null)
				{
					_prerenderedGraph = new Bitmap(this.Width, 20 + (ItemCalculations.Length * 36), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
					Graphics g = Graphics.FromImage(_prerenderedGraph);
					g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

					if (ItemCalculations.Length > 0)
					{
						float maxOverallPoints = 100f;
						foreach (ComparisonCalculationBase calc in ItemCalculations)
							if (!float.IsPositiveInfinity(calc.OverallPoints))
								maxOverallPoints = Math.Max(maxOverallPoints, calc.OverallPoints);
						maxOverallPoints = (float)Math.Ceiling(maxOverallPoints);
						float maxScale = 100f;//(float)(Math.Ceiling(ItemCalculations[0].OverallPoints / 400) * 400f);
						while (maxOverallPoints > maxScale)
							maxScale = (float)(Math.Ceiling((maxScale * 1.2f) / 800f) * 800f);

						Dictionary<string, Color> subPointNameColors = Calculations.SubPointNameColors;
						string[] subPointNames = new string[subPointNameColors.Count];
						Color[] baseColors = new Color[subPointNameColors.Count];
						subPointNameColors.Keys.CopyTo(subPointNames, 0);
						subPointNameColors.Values.CopyTo(baseColors, 0);
						
						StringFormat formatItemNames = new StringFormat();
						formatItemNames.Alignment = StringAlignment.Far;
						formatItemNames.LineAlignment = StringAlignment.Center;
						StringFormat formatSubPoint = new StringFormat();
						formatSubPoint.Alignment = StringAlignment.Center;
						formatSubPoint.LineAlignment = StringAlignment.Center;
						StringFormat formatOverall = new StringFormat();
						formatOverall.Alignment = StringAlignment.Near;
						formatOverall.LineAlignment = StringAlignment.Center;
						Brush brushItemNames = new SolidBrush(this.ForeColor);
						Brush brushOverall = new SolidBrush(Color.FromArgb(128, 0, 128));

						Brush[] brushSubPoints = new Brush[baseColors.Length];
						Color[] colorSubPointsA = new Color[baseColors.Length];
						Color[] colorSubPointsB = new Color[baseColors.Length];
						for (int i = 0; i < baseColors.Length; i++)
						{
							Color baseColor = baseColors[i];
							brushSubPoints[i] = new SolidBrush(Color.FromArgb(baseColor.R / 2, baseColor.G / 2, baseColor.B / 2));
							colorSubPointsA[i] = Color.FromArgb(baseColor.A / 2, baseColor.R / 2, baseColor.G / 2, baseColor.B / 2);
							colorSubPointsB[i] = Color.FromArgb(baseColor.A / 2, baseColor);
						}

						//Brush brushMitigation = new SolidBrush(Color.FromArgb(128, 0, 0));
						//Brush brushSurvival = new SolidBrush(Color.FromArgb(0, 0, 128));
						//Color colorMitigationA = Color.FromArgb(128, 128, 0, 0);
						//Color colorMitigationB = Color.FromArgb(128, 255, 0, 0);
						//Color colorSurvivalA = Color.FromArgb(128, 0, 0, 128);
						//Color colorSurvivalB = Color.FromArgb(128, 0, 0, 255);

						#region TODO: Legend
						Rectangle rectSubPoint;
						System.Drawing.Drawing2D.LinearGradientBrush brushSubPointFill;
						System.Drawing.Drawing2D.ColorBlend blendSubPoint;
						//Rectangle rectMitigation = new Rectangle(2, 2, 44, 16);
						//Rectangle rectSurvival = new Rectangle(46, 2, 44, 16);

						//System.Drawing.Drawing2D.LinearGradientBrush brushMitigationFill = new System.Drawing.Drawing2D.LinearGradientBrush(
						//    new Rectangle(rectMitigation.X, rectMitigation.Y, 88, 24), colorMitigationA, colorMitigationB,
						//    67f + (20f * (1)));
						//System.Drawing.Drawing2D.ColorBlend blendMitigation = new System.Drawing.Drawing2D.ColorBlend(3);
						//blendMitigation.Colors = new Color[] { colorMitigationA, colorMitigationB, colorMitigationA };
						//blendMitigation.Positions = new float[] { 0f, 0.5f, 1f };
						//brushMitigationFill.InterpolationColors = blendMitigation;
						//System.Drawing.Drawing2D.LinearGradientBrush brushSurvivalFill = new System.Drawing.Drawing2D.LinearGradientBrush(
						//    new Rectangle(rectMitigation.X, rectMitigation.Y, 88, 24), colorSurvivalA, colorSurvivalB,
						//    67f + (20f * (1)));
						//System.Drawing.Drawing2D.ColorBlend blendSurvival = new System.Drawing.Drawing2D.ColorBlend(3);
						//blendSurvival.Colors = new Color[] { colorSurvivalA, colorSurvivalB, colorSurvivalA };
						//blendSurvival.Positions = new float[] { 0f, 0.5f, 1f };
						//brushSurvivalFill.InterpolationColors = blendSurvival;

						//g.FillRectangle(brushMitigationFill, rectMitigation);
						//g.FillRectangle(brushSurvivalFill, rectSurvival);

						//g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
						//g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
						//g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
						//g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
						//g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
						//g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);

						//g.DrawString("Mitigation", new Font(this.Font.FontFamily, 6.0f), brushMitigation, rectMitigation, formatMitigationSurvival);
						//g.DrawString("Survival", new Font(this.Font.FontFamily, 6.0f), brushSurvival, rectSurvival, formatMitigationSurvival);
						#endregion

						#region Graph Ticks
						float graphStart = 110f;
						float graphWidth = this.Width - 150f;
						float graphEnd = graphStart + graphWidth;
						float[] ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};
						Pen black200 = new Pen(Color.FromArgb(200, 0, 0, 0));
						Pen black150 = new Pen(Color.FromArgb(150, 0, 0, 0));
						Pen black75 = new Pen(Color.FromArgb(75, 0, 0, 0));
						Pen black50 = new Pen(Color.FromArgb(50, 0, 0, 0));
						Pen black25 = new Pen(Color.FromArgb(25, 0, 0, 0));
						StringFormat formatTick = new StringFormat();
						formatTick.LineAlignment = StringAlignment.Far;
						formatTick.Alignment = StringAlignment.Center;
						Brush black200brush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
						Brush black150brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
						Brush black75brush = new SolidBrush(Color.FromArgb(75, 0, 0, 0));
						Brush black50brush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
						Brush black25brush = new SolidBrush(Color.FromArgb(25, 0, 0, 0));

						g.DrawLine(black200, graphStart - 4, 20, graphEnd + 4, 20);
						g.DrawLine(black200, graphStart, 16, graphStart, _prerenderedGraph.Height - 4);
						g.DrawLine(black200, graphEnd, 16, graphEnd, 19);
						g.DrawLine(black200, ticks[0], 16, ticks[0], 19);
						g.DrawLine(black150, ticks[1], 16, ticks[1], 19);
						g.DrawLine(black150, ticks[2], 16, ticks[2], 19);
						g.DrawLine(black75, ticks[3], 16, ticks[3], 19);
						g.DrawLine(black75, ticks[4], 16, ticks[4], 19);
						g.DrawLine(black75, ticks[5], 16, ticks[5], 19);
						g.DrawLine(black75, ticks[6], 16, ticks[6], 19);
						g.DrawLine(black75, graphEnd, 21, graphEnd, _prerenderedGraph.Height - 4);
						g.DrawLine(black75, ticks[0], 21, ticks[0], _prerenderedGraph.Height - 4);
						g.DrawLine(black50, ticks[1], 21, ticks[1], _prerenderedGraph.Height - 4);
						g.DrawLine(black50, ticks[2], 21, ticks[2], _prerenderedGraph.Height - 4);
						g.DrawLine(black25, ticks[3], 21, ticks[3], _prerenderedGraph.Height - 4);
						g.DrawLine(black25, ticks[4], 21, ticks[4], _prerenderedGraph.Height - 4);
						g.DrawLine(black25, ticks[5], 21, ticks[5], _prerenderedGraph.Height - 4);
						g.DrawLine(black25, ticks[6], 21, ticks[6], _prerenderedGraph.Height - 4);

						g.DrawString((0f).ToString(), this.Font, black200brush, graphStart, 16, formatTick);
						g.DrawString((maxScale).ToString(), this.Font, black200brush, graphEnd, 16, formatTick);
						g.DrawString((maxScale * 0.5f).ToString(), this.Font, black200brush, ticks[0], 16, formatTick);
						g.DrawString((maxScale * 0.75f).ToString(), this.Font, black150brush, ticks[1], 16, formatTick);
						g.DrawString((maxScale * 0.25f).ToString(), this.Font, black150brush, ticks[2], 16, formatTick);
						g.DrawString((maxScale * 0.125f).ToString(), this.Font, black75brush, ticks[3], 16, formatTick);
						g.DrawString((maxScale * 0.375f).ToString(), this.Font, black75brush, ticks[4], 16, formatTick);
						g.DrawString((maxScale * 0.625f).ToString(), this.Font, black75brush, ticks[5], 16, formatTick);
						g.DrawString((maxScale * 0.875f).ToString(), this.Font, black75brush, ticks[6], 16, formatTick);
						#endregion

						for (int i = 0; i < ItemCalculations.Length; i++)
						{
							ComparisonCalculationBase item = ItemCalculations[i];
							Rectangle rectItemName = new Rectangle(0, 24 + i * 36, (int)(graphStart - 4), 36);
							Color bgColor = Color.Empty;
							if (item.Equipped)
							{
								bgColor = Color.FromArgb(64, 0, 255, 0);
							}
							if (item.Item != null)
								switch (item.Item.Slot)
								{
									case Item.ItemSlot.Red:
										bgColor = Color.FromArgb(64, Color.Red);
										break;
									case Item.ItemSlot.Orange:
										bgColor = Color.FromArgb(64, Color.Orange);
										break;
									case Item.ItemSlot.Yellow:
										bgColor = Color.FromArgb(64, Color.Yellow);
										break;
									case Item.ItemSlot.Green:
										bgColor = Color.FromArgb(64, Color.Green);
										break;
									case Item.ItemSlot.Blue:
										bgColor = Color.FromArgb(64, Color.Blue);
										break;
									case Item.ItemSlot.Purple:
										bgColor = Color.FromArgb(64, Color.Purple);
										break;
									case Item.ItemSlot.Meta:
										bgColor = Color.FromArgb(64, Color.Silver);
										break;
									case Item.ItemSlot.Prismatic:
										bgColor = Color.FromArgb(64, Color.DarkGray);
										break;
								}
							if (bgColor != Color.Empty)
							{
								Rectangle rectBackground = new Rectangle(rectItemName.X, rectItemName.Y + 2, rectItemName.Width, rectItemName.Height - 4);
								g.FillRectangle(new SolidBrush(bgColor), rectBackground);
								g.DrawRectangle(new Pen(bgColor), rectBackground);
							}

							g.DrawString(item.Name, this.Font, brushItemNames, rectItemName, formatItemNames);

							int overallWidth = (int)Math.Round((item.OverallPoints / maxScale) * graphWidth);
							if (!RoundValues) overallWidth = (int)Math.Ceiling((item.OverallPoints / maxScale) * graphWidth);
							if (float.IsPositiveInfinity(item.OverallPoints)) overallWidth = (int)Math.Ceiling(graphWidth + 50f);
							if (overallWidth > 0 && item.OverallPoints > 0.00001f)
							{
								int barStart = 0;
								for (int j = 0; j < item.SubPoints.Length; j++)
								{
									float subPoint = item.SubPoints[j];
									int barWidth = (int)Math.Round((subPoint / item.OverallPoints) * overallWidth);
									if (float.IsPositiveInfinity(subPoint)) barWidth = overallWidth;

									rectSubPoint = new Rectangle((int)graphStart + 1 + barStart, 30 + i * 36, barWidth, 24);
									barStart += barWidth;

									brushSubPointFill = new System.Drawing.Drawing2D.LinearGradientBrush(
										new Rectangle((int)graphStart + 1, rectSubPoint.Y, overallWidth, 24), colorSubPointsA[j], colorSubPointsB[j],
										67f + (20f * (float.IsPositiveInfinity(item.OverallPoints) ? 1f : (item.OverallPoints / maxScale))));
									blendSubPoint = new System.Drawing.Drawing2D.ColorBlend(3);
									blendSubPoint.Colors = new Color[] { colorSubPointsA[j], colorSubPointsB[j], colorSubPointsA[j]};
									blendSubPoint.Positions = new float[] { 0f, 0.5f, 1f };
									brushSubPointFill.InterpolationColors = blendSubPoint;

									g.FillRectangle(brushSubPointFill, rectSubPoint);
									g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
									g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
									g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);

									if (RoundValues && subPoint > 0) g.DrawString(Math.Round(subPoint).ToString(),
										this.Font, brushSubPoints[j], rectSubPoint, formatSubPoint);
								
								}

								//int mitsurvWidth = (int)Math.Round((item.MitigationPoints / (item.MitigationPoints + item.SurvivalPoints)) * overallWidth);
								//if (float.IsPositiveInfinity(item.MitigationPoints)) mitsurvWidth = overallWidth;							

								//rectMitigation = new Rectangle((int)graphStart + 1, 30 + i * 36, mitsurvWidth, 24);
								//rectSurvival = new Rectangle((int)graphStart + mitsurvWidth + 1, 30 + i * 36, overallWidth - mitsurvWidth, 24);

								//brushMitigationFill = new System.Drawing.Drawing2D.LinearGradientBrush(
								//    new Rectangle(rectMitigation.X, rectMitigation.Y, overallWidth, 24), colorMitigationA, colorMitigationB,
								//    67f + (20f * (float.IsPositiveInfinity(item.OverallPoints) ? 1f : (item.OverallPoints / maxScale))));
								//blendMitigation = new System.Drawing.Drawing2D.ColorBlend(3);
								//blendMitigation.Colors = new Color[] { colorMitigationA, colorMitigationB, colorMitigationA };
								//blendMitigation.Positions = new float[] { 0f, 0.5f, 1f };
								//brushMitigationFill.InterpolationColors = blendMitigation;
								//brushSurvivalFill = new System.Drawing.Drawing2D.LinearGradientBrush(
								//    new Rectangle(rectMitigation.X, rectMitigation.Y, overallWidth, 24), colorSurvivalA, colorSurvivalB,
								//    67f + (20f * (item.OverallPoints / maxScale)));
								//blendSurvival = new System.Drawing.Drawing2D.ColorBlend(3);
								//blendSurvival.Colors = new Color[] { colorSurvivalA, colorSurvivalB, colorSurvivalA };
								//blendSurvival.Positions = new float[] { 0f, 0.5f, 1f };
								//brushSurvivalFill.InterpolationColors = blendSurvival;

								//g.FillRectangle(brushMitigationFill, rectMitigation);
								//g.FillRectangle(brushSurvivalFill, rectSurvival);

								//g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
								//g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
								//g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
								//g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
								//g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
								//g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);

								//if (RoundValues && item.MitigationPoints > 0) g.DrawString(Math.Round(item.MitigationPoints).ToString(), 
								//	this.Font, brushMitigation, rectMitigation, formatSubPoint);
								//if (RoundValues && item.SurvivalPoints > 0) g.DrawString(Math.Round(item.SurvivalPoints).ToString(),
								//	this.Font, brushSurvival, rectSurvival, formatSubPoint);
								if (item.OverallPoints > 0) g.DrawString(RoundValues ? Math.Round(item.OverallPoints).ToString() :
									item.OverallPoints.ToString(), this.Font, brushOverall, new Rectangle((int)graphStart + barStart + 2, 30 + i * 36, 50, 24), formatOverall);
							}
						}
					}
					g.Dispose();
					_scrollBar.Maximum = _prerenderedGraph.Height;
					_scrollBar.Value = 0;
					_scrollBar.SmallChange = 32;
					_scrollBar.LargeChange = _scrollBar.Height;
				}
				return _prerenderedGraph;
			}
		}

		public ComparisonGraph()
		{
			this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			this.UpdateStyles();

			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//base.OnPaint(e);
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			e.Graphics.DrawImageUnscaled(PrerenderedGraph, 0, 0 - (_scrollBar != null ? _scrollBar.Value : 0));
		}

		public Character.CharacterSlot EquipSlot = Character.CharacterSlot.None;
		private void ComparisonGraph_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//if (e.Button == MouseButtons.Right)
			//{
				if (e.X <= 96)
				{
					int itemIndex = (int)Math.Floor(((float)(e.Y - 24f + _scrollBar.Value)) / 36f);
					if (itemIndex >= 0 && itemIndex < ItemCalculations.Length && ItemCalculations[itemIndex].Item != null && ItemCalculations[itemIndex].Item.Id != 0)
					{
						ItemContextualMenu.Instance.Show(ItemCalculations[itemIndex].Item, EquipSlot, true);
					}
				}
			//}
		}

		private void ComparisonGraph_MouseLeave(object sender, EventArgs e)
		{
			//ItemTooltip.Instance.HideTooltip();
			HideTooltip();
		}

		private Point _lastPoint = Point.Empty;
		//private Item _lastTooltipItem = null;
		private void ComparisonGraph_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Location != _lastPoint)
			{
				_lastPoint = e.Location;
				if (e.X <= 96)
				{
					int itemIndex = (int)Math.Floor(((float)(e.Y - 24f + _scrollBar.Value)) / 36f);
					if (itemIndex >= 0 && itemIndex < ItemCalculations.Length)
					{
						if (ItemCalculations[itemIndex].Item != _tooltipItem)
						{
							int tipX = 98;
							if (Parent.PointToScreen(Location).X + tipX + 249 > System.Windows.Forms.Screen.GetWorkingArea(this).Right)
								tipX = -249;

							ShowTooltip(ItemCalculations[itemIndex].Item, new Point(tipX, 26 + (itemIndex * 36) - _scrollBar.Value));
							//ItemTooltip.Instance.SetItem(ItemCalculations[itemIndex].Item, GetTooltipLocation(new Point(96, 24 + (itemIndex * 36) - _scrollBar.Value)));
						}
					}
					else
					{
						HideTooltip();
						//ItemTooltip.Instance.HideTooltip();
					}
				}
				else
				{
					HideTooltip();
					//ItemTooltip.Instance.HideTooltip();
				}
			}
		}

		private void ShowTooltip(Item item, Point location)
		{
			if (_tooltipItem != item || _tooltipLocation != location)
			{
				_tooltipItem = item;
				_tooltipLocation = location;
				ShowHideTooltip();
			}
		}

		private void HideTooltip()
		{
			if (_tooltipItem != null)
			{
				_tooltipItem = null;
				ShowHideTooltip();
			}
		}

		public Item GetItem() { return _tooltipItem; }
		private Item _tooltipItem = null;
		private Point _tooltipLocation = Point.Empty;
		private void ShowHideTooltip()
		{
			if (_tooltipItem != null && _tooltipLocation != Point.Empty)
			{
				ItemToolTip.Instance.Show(_tooltipItem.Name, this, _tooltipLocation);
			}
			else
			{
				ItemToolTip.Instance.Hide(this);
			}
		}
	}
}
