using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class ComparisonGraph : UserControl
	{
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
				if(_prerenderedGraph != null) _prerenderedGraph.Dispose();
				_prerenderedGraph = null;
				this.Invalidate();
			}
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

		public ItemTooltip MoveTooltip(Point offset)
		{
			ItemTooltip tooltip = null;
			foreach (Control ctrl in this.FindForm().Controls)
				if (ctrl is ItemTooltip)
				{
					tooltip = ctrl as ItemTooltip;
					break;
				}
			if (tooltip == null)
			{
				tooltip = new ItemTooltip();
				this.FindForm().Controls.Add(tooltip);
			}

			if (offset != Point.Empty)
			{
				System.Drawing.Point p = this.FindForm().PointToClient(this.Parent.PointToScreen(this.Location));
				p.X += 2 + offset.X;
				p.Y += 2 + offset.Y;
				tooltip.Location = p;
			}
			tooltip.Visible = false;

			return tooltip;
		}

		protected int CompareItemCalculations(ItemCalculation a, ItemCalculation b)
		{
			if (Sort == ComparisonSort.Mitigation)
				return b.MitigationPoints.CompareTo(a.MitigationPoints);
			else if (Sort == ComparisonSort.Survival)
				return b.SurvivalPoints.CompareTo(a.SurvivalPoints);
			else
				return b.OverallPoints.CompareTo(a.OverallPoints);
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

		public enum ComparisonSort { Mitigation, Survival, Overall }

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
						float maxOverallPoints = (float)Math.Ceiling(ItemCalculations[0].OverallPoints);
						float maxScale = 100f;//(float)(Math.Ceiling(ItemCalculations[0].OverallPoints / 400) * 400f);
						while (maxOverallPoints > maxScale)
							maxScale = (float)(Math.Ceiling((maxScale * 1.2f) / 800f) * 800f);


						StringFormat formatItemNames = new StringFormat();
						formatItemNames.Alignment = StringAlignment.Far;
						formatItemNames.LineAlignment = StringAlignment.Center;
						StringFormat formatMitigationSurvival = new StringFormat();
						formatMitigationSurvival.Alignment = StringAlignment.Center;
						formatMitigationSurvival.LineAlignment = StringAlignment.Center;
						StringFormat formatOverall = new StringFormat();
						formatOverall.Alignment = StringAlignment.Near;
						formatOverall.LineAlignment = StringAlignment.Center;
						Brush brushItemNames = new SolidBrush(this.ForeColor);
						Brush brushMitigation = new SolidBrush(Color.FromArgb(128, 0, 0));
						Brush brushSurvival = new SolidBrush(Color.FromArgb(0, 0, 128));
						Brush brushOverall = new SolidBrush(Color.FromArgb(128, 0, 128));
						Color colorMitigationA = Color.FromArgb(128, 128, 0, 0);
						Color colorMitigationB = Color.FromArgb(128, 255, 0, 0);
						Color colorSurvivalA = Color.FromArgb(128, 0, 0, 128);
						Color colorSurvivalB = Color.FromArgb(128, 0, 0, 255);

						#region Graph Ticks

						Rectangle rectMitigation = new Rectangle(2, 2, 44, 16);
						Rectangle rectSurvival = new Rectangle(46, 2, 44, 16);

						System.Drawing.Drawing2D.LinearGradientBrush brushMitigationFill = new System.Drawing.Drawing2D.LinearGradientBrush(
							new Rectangle(rectMitigation.X, rectMitigation.Y, 88, 24), colorMitigationA, colorMitigationB,
							67f + (20f * (1)));
						System.Drawing.Drawing2D.ColorBlend blendMitigation = new System.Drawing.Drawing2D.ColorBlend(3);
						blendMitigation.Colors = new Color[] { colorMitigationA, colorMitigationB, colorMitigationA };
						blendMitigation.Positions = new float[] { 0f, 0.5f, 1f };
						brushMitigationFill.InterpolationColors = blendMitigation;
						System.Drawing.Drawing2D.LinearGradientBrush brushSurvivalFill = new System.Drawing.Drawing2D.LinearGradientBrush(
							new Rectangle(rectMitigation.X, rectMitigation.Y, 88, 24), colorSurvivalA, colorSurvivalB,
							67f + (20f * (1)));
						System.Drawing.Drawing2D.ColorBlend blendSurvival = new System.Drawing.Drawing2D.ColorBlend(3);
						blendSurvival.Colors = new Color[] { colorSurvivalA, colorSurvivalB, colorSurvivalA };
						blendSurvival.Positions = new float[] { 0f, 0.5f, 1f };
						brushSurvivalFill.InterpolationColors = blendSurvival;

						g.FillRectangle(brushMitigationFill, rectMitigation);
						g.FillRectangle(brushSurvivalFill, rectSurvival);

						g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
						g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
						g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
						g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
						g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
						g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);

						g.DrawString("Mitigation", new Font(this.Font.FontFamily, 6.0f), brushMitigation, rectMitigation, formatMitigationSurvival);
						g.DrawString("Survival", new Font(this.Font.FontFamily, 6.0f), brushSurvival, rectSurvival, formatMitigationSurvival);
								


						float graphStart = 100f;
						float graphWidth = this.Width - 140f;
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
						
						g.DrawLine(black200, 96, 20, graphEnd + 4, 20);
						g.DrawLine(black200, 100, 16, 100, _prerenderedGraph.Height - 4);
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
							ItemCalculation item = ItemCalculations[i];
							Rectangle rectItemName = new Rectangle(0, 24 + i * 36, 96, 36);
							if (item.Equipped)
							{
								g.FillRectangle(new SolidBrush(Color.FromArgb(64, 0, 255, 0)), rectItemName);
								g.DrawRectangle(new Pen(Color.FromArgb(64, 0, 255, 0)), rectItemName);
							}
							g.DrawString(item.ItemName, this.Font, brushItemNames, rectItemName, formatItemNames);

							int OverallWidth = (int)Math.Round((item.OverallPoints / maxScale) * graphWidth);
							if (OverallWidth > 0)
							{
								int mitsurvWidth = (int)Math.Round((item.MitigationPoints / (item.MitigationPoints + item.SurvivalPoints)) * OverallWidth);

								rectMitigation = new Rectangle((int)graphStart + 1, 30 + i * 36, mitsurvWidth, 24);
								rectSurvival = new Rectangle((int)graphStart + mitsurvWidth + 1, 30 + i * 36, OverallWidth - mitsurvWidth, 24);

								brushMitigationFill = new System.Drawing.Drawing2D.LinearGradientBrush(
									new Rectangle(rectMitigation.X, rectMitigation.Y, OverallWidth, 24), colorMitigationA, colorMitigationB,
									67f + (20f * (item.OverallPoints / maxScale)));
								blendMitigation = new System.Drawing.Drawing2D.ColorBlend(3);
								blendMitigation.Colors = new Color[] { colorMitigationA, colorMitigationB, colorMitigationA };
								blendMitigation.Positions = new float[] { 0f, 0.5f, 1f };
								brushMitigationFill.InterpolationColors = blendMitigation;
								brushSurvivalFill = new System.Drawing.Drawing2D.LinearGradientBrush(
									new Rectangle(rectMitigation.X, rectMitigation.Y, OverallWidth, 24), colorSurvivalA, colorSurvivalB,
									67f + (20f * (item.OverallPoints / maxScale)));
								blendSurvival = new System.Drawing.Drawing2D.ColorBlend(3);
								blendSurvival.Colors = new Color[] { colorSurvivalA, colorSurvivalB, colorSurvivalA };
								blendSurvival.Positions = new float[] { 0f, 0.5f, 1f };
								brushSurvivalFill.InterpolationColors = blendSurvival;

								g.FillRectangle(brushMitigationFill, rectMitigation);
								g.FillRectangle(brushSurvivalFill, rectSurvival);

								g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
								g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
								g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
								g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
								g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
								g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);

								if (item.MitigationPoints > 0) g.DrawString(Math.Round(item.MitigationPoints).ToString(), this.Font, brushMitigation, rectMitigation, formatMitigationSurvival);
								if (item.SurvivalPoints > 0) g.DrawString(Math.Round(item.SurvivalPoints).ToString(), this.Font, brushSurvival, rectSurvival, formatMitigationSurvival);
								if (item.OverallPoints > 0) g.DrawString(Math.Round(item.OverallPoints).ToString(), this.Font, brushOverall, new Rectangle(rectSurvival.Right + 2, rectSurvival.Y, 50, rectSurvival.Height), formatOverall);
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

		private void ComparisonGraph_MouseLeave(object sender, EventArgs e)
		{
			MoveTooltip(Point.Empty).Visible = false;
			_lastTooltipItem = null;
		}

		private Point _lastPoint = Point.Empty;
		private Item _lastTooltipItem = null;
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
						if (ItemCalculations[itemIndex].Item != _lastTooltipItem)
						{
							_lastTooltipItem = ItemCalculations[itemIndex].Item;
							MoveTooltip(new Point(96, 24 + (itemIndex*36) - _scrollBar.Value)).SetItem(ItemCalculations[itemIndex].Item);
						}
					}
					else
					{
						_lastTooltipItem = null;
						MoveTooltip(Point.Empty).Visible = false;
					}
				}
				else
				{
					_lastTooltipItem = null;
					MoveTooltip(Point.Empty).Visible = false;
				}
			}
		}
	}
}
