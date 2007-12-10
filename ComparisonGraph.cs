using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class ComparisonGraph : UserControl, IItemProvider
    {
        private ItemBuffEnchantCalculation[] _itemCalculations;

        public ItemBuffEnchantCalculation[] ItemCalculations
        {
            get { return _itemCalculations; }
            set
            {
                if (value == null)
                {
                    _itemCalculations = new ItemBuffEnchantCalculation[0];
                }
                else
                {
                    List<ItemBuffEnchantCalculation> calcs = new List<ItemBuffEnchantCalculation>(value);
                    calcs.Sort(new Comparison<ItemBuffEnchantCalculation>(CompareItemCalculations));
                    _itemCalculations = calcs.ToArray();
                }
                //if (_prerenderedGraph != null) 
                //   _prerenderedGraph.Dispose();
                _prerenderedGraph = null;
                Invalidate();
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
            Point p = (Parent.PointToScreen(Location)); //this.FindForm().PointToClient
            p.X += 2 + offset.X;
            p.Y += 2 + offset.Y;
            return p;
            //tooltip.Location = p;
            //}
            //tooltip.Visible = false;

            //return tooltip;
        }

        protected int CompareItemCalculations(ItemBuffEnchantCalculation a, ItemBuffEnchantCalculation b)
        {
            if (Sort == ComparisonSort.Mitigation)
                return b.MitigationPoints.CompareTo(a.MitigationPoints);
            else if (Sort == ComparisonSort.Survival)
                return b.SurvivalPoints.CompareTo(a.SurvivalPoints);
            else if (Sort == ComparisonSort.Alphabetical)
                return a.Name.CompareTo(b.Name);
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

        public enum ComparisonSort
        {
            Mitigation,
            Survival,
            Overall,
            Alphabetical
        }

        private void _scrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
        }

        private Bitmap _prerenderedGraph = null;

        public Bitmap PrerenderedGraph
        {
            get
            {
                if (_prerenderedGraph == null)
                {
                    _prerenderedGraph =
                        new Bitmap(Width, 20 + (ItemCalculations.Length*36), PixelFormat.Format32bppArgb);
                    Graphics g = Graphics.FromImage(_prerenderedGraph);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;

                    if (ItemCalculations.Length > 0)
                    {
                        float maxOverallPoints = 100f;
                        foreach (ItemBuffEnchantCalculation calc in ItemCalculations)
                            if (!float.IsPositiveInfinity(calc.OverallPoints))
                                maxOverallPoints = Math.Max(maxOverallPoints, calc.OverallPoints);
                        maxOverallPoints = (float) Math.Ceiling(maxOverallPoints);
                        float maxScale = 100f; //(float)(Math.Ceiling(ItemCalculations[0].OverallPoints / 400) * 400f);
                        while (maxOverallPoints > maxScale)
                            maxScale = (float) (Math.Ceiling((maxScale*1.2f)/800f)*800f);


                        StringFormat formatItemNames = new StringFormat();
                        formatItemNames.Alignment = StringAlignment.Far;
                        formatItemNames.LineAlignment = StringAlignment.Center;
                        StringFormat formatMitigationSurvival = new StringFormat();
                        formatMitigationSurvival.Alignment = StringAlignment.Center;
                        formatMitigationSurvival.LineAlignment = StringAlignment.Center;
                        StringFormat formatOverall = new StringFormat();
                        formatOverall.Alignment = StringAlignment.Near;
                        formatOverall.LineAlignment = StringAlignment.Center;
                        Brush brushItemNames = new SolidBrush(ForeColor);
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

                        LinearGradientBrush brushMitigationFill = new LinearGradientBrush(
                            new Rectangle(rectMitigation.X, rectMitigation.Y, 88, 24), colorMitigationA,
                            colorMitigationB,
                            67f + (20f*(1)));
                        ColorBlend blendMitigation = new ColorBlend(3);
                        blendMitigation.Colors = new Color[] {colorMitigationA, colorMitigationB, colorMitigationA};
                        blendMitigation.Positions = new float[] {0f, 0.5f, 1f};
                        brushMitigationFill.InterpolationColors = blendMitigation;
                        LinearGradientBrush brushSurvivalFill = new LinearGradientBrush(
                            new Rectangle(rectMitigation.X, rectMitigation.Y, 88, 24), colorSurvivalA, colorSurvivalB,
                            67f + (20f*(1)));
                        ColorBlend blendSurvival = new ColorBlend(3);
                        blendSurvival.Colors = new Color[] {colorSurvivalA, colorSurvivalB, colorSurvivalA};
                        blendSurvival.Positions = new float[] {0f, 0.5f, 1f};
                        brushSurvivalFill.InterpolationColors = blendSurvival;

                        g.FillRectangle(brushMitigationFill, rectMitigation);
                        g.FillRectangle(brushSurvivalFill, rectSurvival);

                        g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
                        g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
                        g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
                        g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
                        g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
                        g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);

                        g.DrawString("Mitigation", new Font(Font.FontFamily, 6.0f), brushMitigation, rectMitigation,
                                     formatMitigationSurvival);
                        g.DrawString("Survival", new Font(Font.FontFamily, 6.0f), brushSurvival, rectSurvival,
                                     formatMitigationSurvival);


                        float graphStart = 100f;
                        float graphWidth = Width - 140f;
                        float graphEnd = graphStart + graphWidth;
                        float[] ticks = new float[]
                            {
                                (float) Math.Round(graphStart + graphWidth*0.5f),
                                (float) Math.Round(graphStart + graphWidth*0.75f),
                                (float) Math.Round(graphStart + graphWidth*0.25f),
                                (float) Math.Round(graphStart + graphWidth*0.125f),
                                (float) Math.Round(graphStart + graphWidth*0.375f),
                                (float) Math.Round(graphStart + graphWidth*0.625f),
                                (float) Math.Round(graphStart + graphWidth*0.875f)
                            };
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
                        //Brush black50brush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
                        //Brush black25brush = new SolidBrush(Color.FromArgb(25, 0, 0, 0));

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

                        g.DrawString((0f).ToString(), Font, black200brush, graphStart, 16, formatTick);
                        g.DrawString((maxScale).ToString(), Font, black200brush, graphEnd, 16, formatTick);
                        g.DrawString((maxScale*0.5f).ToString(), Font, black200brush, ticks[0], 16, formatTick);
                        g.DrawString((maxScale*0.75f).ToString(), Font, black150brush, ticks[1], 16, formatTick);
                        g.DrawString((maxScale*0.25f).ToString(), Font, black150brush, ticks[2], 16, formatTick);
                        g.DrawString((maxScale*0.125f).ToString(), Font, black75brush, ticks[3], 16, formatTick);
                        g.DrawString((maxScale*0.375f).ToString(), Font, black75brush, ticks[4], 16, formatTick);
                        g.DrawString((maxScale*0.625f).ToString(), Font, black75brush, ticks[5], 16, formatTick);
                        g.DrawString((maxScale*0.875f).ToString(), Font, black75brush, ticks[6], 16, formatTick);

                        #endregion

                        for (int i = 0; i < ItemCalculations.Length; i++)
                        {
                            ItemBuffEnchantCalculation item = ItemCalculations[i];
                            Rectangle rectItemName = new Rectangle(0, 24 + i*36, 96, 36);
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
                                Rectangle rectBackground =
                                    new Rectangle(rectItemName.X, rectItemName.Y + 2, rectItemName.Width,
                                                  rectItemName.Height - 4);
                                g.FillRectangle(new SolidBrush(bgColor), rectBackground);
                                g.DrawRectangle(new Pen(bgColor), rectBackground);
                            }

                            g.DrawString(item.Name, Font, brushItemNames, rectItemName, formatItemNames);

                            int overallWidth = (int) Math.Round((item.OverallPoints/maxScale)*graphWidth);
                            if (!RoundValues)
                                overallWidth = (int) Math.Ceiling((item.OverallPoints/maxScale)*graphWidth);
                            if (float.IsPositiveInfinity(item.OverallPoints))
                                overallWidth = (int) Math.Ceiling(graphWidth + 50f);
                            if (overallWidth > 0)
                            {
                                int mitsurvWidth =
                                    (int)
                                    Math.Round((item.MitigationPoints/(item.MitigationPoints + item.SurvivalPoints))*
                                               overallWidth);
                                if (float.IsPositiveInfinity(item.MitigationPoints)) mitsurvWidth = overallWidth;

                                rectMitigation = new Rectangle((int) graphStart + 1, 30 + i*36, mitsurvWidth, 24);
                                rectSurvival =
                                    new Rectangle((int) graphStart + mitsurvWidth + 1, 30 + i*36,
                                                  overallWidth - mitsurvWidth, 24);

                                brushMitigationFill = new LinearGradientBrush(
                                    new Rectangle(rectMitigation.X, rectMitigation.Y, overallWidth, 24),
                                    colorMitigationA, colorMitigationB,
                                    67f +
                                    (20f*
                                     (float.IsPositiveInfinity(item.OverallPoints) ? 1f : (item.OverallPoints/maxScale))));
                                blendMitigation = new ColorBlend(3);
                                blendMitigation.Colors =
                                    new Color[] {colorMitigationA, colorMitigationB, colorMitigationA};
                                blendMitigation.Positions = new float[] {0f, 0.5f, 1f};
                                brushMitigationFill.InterpolationColors = blendMitigation;
                                brushSurvivalFill = new LinearGradientBrush(
                                    new Rectangle(rectMitigation.X, rectMitigation.Y, overallWidth, 24), colorSurvivalA,
                                    colorSurvivalB,
                                    67f + (20f*(item.OverallPoints/maxScale)));
                                blendSurvival = new ColorBlend(3);
                                blendSurvival.Colors = new Color[] {colorSurvivalA, colorSurvivalB, colorSurvivalA};
                                blendSurvival.Positions = new float[] {0f, 0.5f, 1f};
                                brushSurvivalFill.InterpolationColors = blendSurvival;

                                g.FillRectangle(brushMitigationFill, rectMitigation);
                                g.FillRectangle(brushSurvivalFill, rectSurvival);

                                g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
                                g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
                                g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
                                g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);
                                g.DrawRectangle(new Pen(brushMitigationFill), rectMitigation);
                                g.DrawRectangle(new Pen(brushSurvivalFill), rectSurvival);

                                if (RoundValues && item.MitigationPoints > 0)
                                    g.DrawString(Math.Round(item.MitigationPoints).ToString(),
                                                 Font, brushMitigation, rectMitigation, formatMitigationSurvival);
                                if (RoundValues && item.SurvivalPoints > 0)
                                    g.DrawString(Math.Round(item.SurvivalPoints).ToString(),
                                                 Font, brushSurvival, rectSurvival, formatMitigationSurvival);
                                if (item.OverallPoints > 0)
                                    g.DrawString(RoundValues
                                                     ? Math.Round(item.OverallPoints).ToString()
                                                     :
                                                 item.OverallPoints.ToString(), Font, brushOverall,
                                                 new Rectangle(rectSurvival.Right + 2, rectSurvival.Y, 50,
                                                               rectSurvival.Height), formatOverall);
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
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawImageUnscaled(PrerenderedGraph, 0, 0 - (_scrollBar != null ? _scrollBar.Value : 0));
        }

        public Character.CharacterSlot EquipSlot = Character.CharacterSlot.None;

        private void ComparisonGraph_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            if (e.X <= 96)
            {
                int itemIndex = (int) Math.Floor(((e.Y - 24f + _scrollBar.Value))/36f);
                if (itemIndex >= 0 && itemIndex < ItemCalculations.Length && ItemCalculations[itemIndex].Item != null &&
                    ItemCalculations[itemIndex].Item.Id != 0)
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
                    int itemIndex = (int) Math.Floor(((e.Y - 24f + _scrollBar.Value))/36f);
                    if (itemIndex >= 0 && itemIndex < ItemCalculations.Length)
                    {
                        if (ItemCalculations[itemIndex].Item != _tooltipItem)
                        {
                            int tipX = 98;
                            if (Parent.PointToScreen(Location).X + tipX + 249 > Screen.GetWorkingArea(this).Right)
                                tipX = -249;

                            ShowTooltip(ItemCalculations[itemIndex].Item,
                                        new Point(tipX, 26 + (itemIndex*36) - _scrollBar.Value));
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

        public Item GetItem()
        {
            return _tooltipItem;
        }

        private Item _tooltipItem = null;
        private Point _tooltipLocation = Point.Empty;

        private void ShowHideTooltip()
        {
            if (_tooltipItem != null && _tooltipLocation != Point.Empty)
            {
                ItemToolTip.Instance.Show("tooltip", this, _tooltipLocation);
            }
            else
            {
                ItemToolTip.Instance.Hide(this);
            }
        }
    }
}