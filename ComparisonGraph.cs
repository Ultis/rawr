using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Rawr
{
    public partial class ComparisonGraph : UserControl
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
                if (_prerenderedGraph != null)
                    _prerenderedGraph.Dispose();
                _prerenderedGraph = null;
                this.Invalidate();
            }
        }

        public bool CustomRendered { get; set; }
        public string CustomRenderedChartName { get; set; }

        private Character _character;
        public Character Character
        {
            get
            {
                return _character;
            }
            set
            {
                if (_character != null)
                {
                    _character.AvailableItemsChanged -= new EventHandler(_character_AvailableItemsChanged);
                }
                _character = value;
                if (_character != null)
                {
                    _character.AvailableItemsChanged += new EventHandler(_character_AvailableItemsChanged);
                }
                if (_prerenderedGraph != null)
                    _prerenderedGraph.Dispose();
                _prerenderedGraph = null;
                this.Invalidate();
            }
        }

        void _character_AvailableItemsChanged(object sender, EventArgs e)
        {
            int scrollValue = -1;
            if (_scrollBar != null)
                scrollValue = _scrollBar.Value;
            if (_prerenderedGraph != null)
                _prerenderedGraph.Dispose();
            _prerenderedGraph = null;
            this.Invalidate();
            PrerenderedGraph.ToString(); //render it, so we can scroll back to the previous spot
            if (scrollValue > 0)
                _scrollBar.Value = scrollValue;
        }

        private bool _roundValues = true;
        public bool RoundValues
        {
            get
            {
                return _roundValues;
            }
            set
            {
                _roundValues = value;
            }
        }

        private ComparisonSort _sort = ComparisonSort.Overall;
        public ComparisonSort Sort
        {
            get
            {
                return _sort;
            }
            set
            {
                _sort = value;
                ItemCalculations = ItemCalculations;
            }
        }

        public enum GraphDisplayMode
        {
            Subpoints,
            Overall,
            CustomSubpoints,
        }

        public GraphDisplayMode DisplayMode
        {
            get;
            set;
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
                return b.SubPoints[(int) Sort].CompareTo(a.SubPoints[(int) Sort]);
        }

        private VScrollBar _scrollBar;
        public VScrollBar ScrollBar
        {
            get
            {
                return _scrollBar;
            }
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
					if (ItemCalculations == null) return new Bitmap(1, 1);
                    if (CustomRendered)
                    {
                        _prerenderedGraph = new Bitmap(Math.Min(32767, Math.Max(1, this.Width)), Math.Min(32767, Math.Max(1, this.Height)), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    }
                    else
                    {
                        _prerenderedGraph = new Bitmap(Math.Min(32767, Math.Max(1, this.Width)), Math.Min(32767, 40 + (ItemCalculations.Length * 36)), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    }
                    Graphics g = Graphics.FromImage(_prerenderedGraph);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
					g.FillRectangle(Brushes.White, 0, 0, _prerenderedGraph.Width, _prerenderedGraph.Height);
                    if (CustomRendered)
                    {
                        Calculations.RenderCustomChart(_character, CustomRenderedChartName, g, _prerenderedGraph.Width, _prerenderedGraph.Height);
                    }
                    else
                    {
                        if (ItemCalculations.Length > 0)
                        {
                            float maxOverallPoints = 2f;
                            foreach (ComparisonCalculationBase calc in ItemCalculations)
                            {
                                float points = calc.OverallPoints;
                                if (DisplayMode != GraphDisplayMode.Subpoints && Sort != ComparisonSort.Alphabetical && Sort != ComparisonSort.Overall)
                                {
                                    points = calc.SubPoints[(int)Sort];
                                }
                                if (!float.IsPositiveInfinity(points))
                                {
                                    maxOverallPoints = Math.Max(maxOverallPoints, points);
                                    maxOverallPoints = Math.Max(maxOverallPoints, -points);
                                }
                            }
                            maxOverallPoints = (float)Math.Ceiling(maxOverallPoints);
                            float maxScale = 10f;//(float)(Math.Ceiling(ItemCalculations[0].OverallPoints / 400) * 400f);
                            if (maxOverallPoints < 10)
                                maxScale = 2f * (float)Math.Ceiling(maxOverallPoints / 2f);
                            else if (maxOverallPoints < 100)
                                maxScale = 10f * (float)Math.Ceiling(maxOverallPoints / 10f);
                            else if (maxOverallPoints < 800)
                                maxScale = 100f * (float)Math.Ceiling(maxOverallPoints / 100f);
                            else
                            {
                                while (maxOverallPoints > maxScale)
                                    maxScale = (float)(Math.Ceiling((maxScale * 1.2f) / 800f) * 800f);
                            }

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

                            #region Legend
                            Rectangle rectSubPoint;
                            System.Drawing.Drawing2D.LinearGradientBrush brushSubPointFill;
                            System.Drawing.Drawing2D.ColorBlend blendSubPoint;

                            Font fontLegend = new Font(this.Font.FontFamily, 10f, GraphicsUnit.Pixel);
                            int legendX = 2;
                            if (DisplayMode == GraphDisplayMode.Subpoints)
                            {
                                for (int i = 0; i < subPointNames.Length; i++)
                                {
                                    string subPointName = subPointNames[i];
                                    int widthSubPoint = (int)Math.Ceiling(g.MeasureString(subPointName, fontLegend).Width + 2f);
                                    rectSubPoint = new Rectangle(legendX, 2, widthSubPoint, 16);
                                    blendSubPoint = new ColorBlend(3);
                                    blendSubPoint.Colors = new Color[] { colorSubPointsA[i], colorSubPointsB[i], colorSubPointsA[i] };
                                    blendSubPoint.Positions = new float[] { 0f, 0.5f, 1f };
                                    brushSubPointFill = new LinearGradientBrush(rectSubPoint, colorSubPointsA[i], colorSubPointsB[i], 67f);
                                    brushSubPointFill.InterpolationColors = blendSubPoint;

                                    g.FillRectangle(brushSubPointFill, rectSubPoint);
                                    g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                    g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                    g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);

                                    g.DrawString(subPointName, fontLegend, brushSubPoints[i], rectSubPoint, formatSubPoint);
                                    legendX += widthSubPoint;
                                }
                            }

                            legendX += 16;
                            Bitmap bmpDiamond = Rawr.Properties.Resources.Diamond;
                            Bitmap bmpDiamond2 = Rawr.Properties.Resources.Diamond2;
                            Bitmap bmpDiamond3 = Rawr.Properties.Resources.Diamond3;
                            Bitmap bmpDiamond4 = Rawr.Properties.Resources.Diamond4;
                            Bitmap bmpDiamondOutline = Rawr.Properties.Resources.DiamondOutline;

                            g.DrawImageUnscaled(bmpDiamond, legendX, 2);
                            g.DrawString("=", this.Font, new SolidBrush(this.ForeColor), legendX + 12, 3);
                            g.DrawString("Available for Optimizer", this.Font, new SolidBrush(this.ForeColor), legendX + 24, 4);
                            #endregion

                            #region Graph Ticks
                            float graphStart = 120f;
                            float graphWidth = this.Width - 160f;
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

                            g.DrawLine(black200, graphStart - 4, 40, graphEnd + 4, 40);
                            g.DrawLine(black200, graphStart, 36, graphStart, _prerenderedGraph.Height - 4);
                            g.DrawLine(black200, graphEnd, 36, graphEnd, 39);
                            g.DrawLine(black200, ticks[0], 36, ticks[0], 39);
                            g.DrawLine(black150, ticks[1], 36, ticks[1], 39);
                            g.DrawLine(black150, ticks[2], 36, ticks[2], 39);
                            g.DrawLine(black75, ticks[3], 36, ticks[3], 39);
                            g.DrawLine(black75, ticks[4], 36, ticks[4], 39);
                            g.DrawLine(black75, ticks[5], 36, ticks[5], 39);
                            g.DrawLine(black75, ticks[6], 36, ticks[6], 39);
                            g.DrawLine(black75, graphEnd, 41, graphEnd, _prerenderedGraph.Height - 4);
                            g.DrawLine(black75, ticks[0], 41, ticks[0], _prerenderedGraph.Height - 4);
                            g.DrawLine(black50, ticks[1], 41, ticks[1], _prerenderedGraph.Height - 4);
                            g.DrawLine(black50, ticks[2], 41, ticks[2], _prerenderedGraph.Height - 4);
                            g.DrawLine(black25, ticks[3], 41, ticks[3], _prerenderedGraph.Height - 4);
                            g.DrawLine(black25, ticks[4], 41, ticks[4], _prerenderedGraph.Height - 4);
                            g.DrawLine(black25, ticks[5], 41, ticks[5], _prerenderedGraph.Height - 4);
                            g.DrawLine(black25, ticks[6], 41, ticks[6], _prerenderedGraph.Height - 4);

                            g.DrawString((0f).ToString(), this.Font, black200brush, graphStart, 36, formatTick);
                            g.DrawString((maxScale).ToString(), this.Font, black200brush, graphEnd, 36, formatTick);
                            g.DrawString((maxScale * 0.5f).ToString(), this.Font, black200brush, ticks[0], 36, formatTick);
                            g.DrawString((maxScale * 0.75f).ToString(), this.Font, black150brush, ticks[1], 36, formatTick);
                            g.DrawString((maxScale * 0.25f).ToString(), this.Font, black150brush, ticks[2], 36, formatTick);
                            g.DrawString((maxScale * 0.125f).ToString(), this.Font, black75brush, ticks[3], 36, formatTick);
                            g.DrawString((maxScale * 0.375f).ToString(), this.Font, black75brush, ticks[4], 36, formatTick);
                            g.DrawString((maxScale * 0.625f).ToString(), this.Font, black75brush, ticks[5], 36, formatTick);
                            g.DrawString((maxScale * 0.875f).ToString(), this.Font, black75brush, ticks[6], 36, formatTick);
                            #endregion

                            for (int i = 0; i < ItemCalculations.Length; i++)
                            {
                                ComparisonCalculationBase item = ItemCalculations[i];
                                Rectangle rectItemName = new Rectangle(10, 44 + i * 36, (int)(graphStart - 14), 36);
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
                                if (item.Item != null && item.Item.Id != 0)
                                {
                                    switch (Character.GetItemAvailability(item.Item))
                                    {
                                        case Character.ItemAvailability.RegemmingAllowed:
                                            g.DrawImageUnscaled(bmpDiamond, 0, 55 + (i * 36));
                                            break;
                                        case Character.ItemAvailability.RegemmingAllowedWithEnchantRestrictions:
                                            g.DrawImageUnscaled(bmpDiamond3, 0, 55 + (i * 36));
                                            break;
                                        case Character.ItemAvailability.Available:
                                            g.DrawImageUnscaled(bmpDiamond2, 0, 55 + (i * 36));
                                            break;
                                        case Character.ItemAvailability.AvailableWithEnchantRestrictions:
                                            g.DrawImageUnscaled(bmpDiamond4, 0, 55 + (i * 36));
                                            break;
                                        case Character.ItemAvailability.NotAvailabe:
                                            g.DrawImageUnscaled(bmpDiamondOutline, 0, 55 + (i * 36));
                                            break;
                                    }
                                }

                                g.DrawString(item.Name, this.Font, brushItemNames, rectItemName, formatItemNames);

                                int overallWidth = (int)Math.Round((item.OverallPoints / maxScale) * graphWidth);
                                if (!RoundValues)
                                    overallWidth = (int)Math.Ceiling((item.OverallPoints / maxScale) * graphWidth);
                                if (float.IsPositiveInfinity(item.OverallPoints))
                                    overallWidth = (int)Math.Ceiling(graphWidth + 50f);
                                if (overallWidth > 0 && item.OverallPoints > 0.00001f || DisplayMode == GraphDisplayMode.Overall || DisplayMode == GraphDisplayMode.CustomSubpoints)
                                {
                                    int barStart = 0;
                                    if (DisplayMode == GraphDisplayMode.Subpoints)
                                    {
                                        for (int j = 0; j < item.SubPoints.Length && j < colorSubPointsA.Length; j++)
                                        {
                                            float subPoint = item.SubPoints[j];
                                            int barWidth = (int)Math.Round((subPoint / item.OverallPoints) * overallWidth);
                                            if (float.IsPositiveInfinity(subPoint))
                                                barWidth = overallWidth;

                                            rectSubPoint = new Rectangle((int)graphStart + 1 + barStart, 50 + i * 36, barWidth, 24);
                                            barStart += barWidth;

                                            brushSubPointFill = new System.Drawing.Drawing2D.LinearGradientBrush(
                                                new Rectangle((int)graphStart + 1, rectSubPoint.Y, overallWidth, 24), colorSubPointsA[j], colorSubPointsB[j],
                                                67f + (20f * (float.IsPositiveInfinity(item.OverallPoints) ? 1f : (item.OverallPoints / maxScale))));
                                            blendSubPoint = new System.Drawing.Drawing2D.ColorBlend(3);
                                            blendSubPoint.Colors = new Color[] { colorSubPointsA[j], colorSubPointsB[j], colorSubPointsA[j] };
                                            blendSubPoint.Positions = new float[] { 0f, 0.5f, 1f };
                                            brushSubPointFill.InterpolationColors = blendSubPoint;

                                            g.FillRectangle(brushSubPointFill, rectSubPoint);
                                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);

                                            //if (RoundValues && subPoint > 0)
                                            if (rectSubPoint.Width > 7)
                                                g.DrawString(subPoint.ToString("F"),
                                                    this.Font, brushSubPoints[j], rectSubPoint, formatSubPoint);


                                        }
                                        if (item.OverallPoints > 0)
                                            g.DrawString(RoundValues ? item.OverallPoints.ToString("F") :
                                                item.OverallPoints.ToString(), this.Font, brushOverall, new Rectangle((int)graphStart + barStart + 2, 50 + i * 36, 50, 24), formatOverall);

                                    }
                                    else if (DisplayMode == GraphDisplayMode.Overall || DisplayMode == GraphDisplayMode.CustomSubpoints)
                                    {
                                        float points = item.OverallPoints;
                                        Color colorA = Color.FromArgb(128, 64, 0, 64);
                                        Color colorB = Color.FromArgb(128, 128, 0, 128);
                                        if (Sort != ComparisonSort.Alphabetical && Sort != ComparisonSort.Overall)
                                        {
                                            points = item.SubPoints[(int)Sort];
                                            overallWidth = (int)Math.Round((points / maxScale) * graphWidth);
                                            if (DisplayMode != GraphDisplayMode.CustomSubpoints)
                                            {
                                                colorA = colorSubPointsA[(int)Sort];
                                                colorB = colorSubPointsB[(int)Sort];
                                            }
                                            if (overallWidth < 0)
                                            {
                                                overallWidth *= -1;
                                                colorA = Color.FromArgb(colorA.A, 255 - colorA.R, 255 - colorA.G, 255 - colorA.B);
                                                colorB = Color.FromArgb(colorB.A, 255 - colorB.R, 255 - colorB.G, 255 - colorB.B);
                                            }
                                        }
                                        int barWidth = overallWidth;

                                        if (barWidth > 0)
                                        {
                                            rectSubPoint = new Rectangle((int)graphStart + 1 + barStart, 50 + i * 36, barWidth, 24);
                                            barStart += barWidth;

                                            brushSubPointFill = new System.Drawing.Drawing2D.LinearGradientBrush(
                                                new Rectangle((int)graphStart + 1, rectSubPoint.Y, overallWidth, 24), colorA, colorB,
                                                67f + (20f * (float.IsPositiveInfinity(points) ? 1f : (points / maxScale))));
                                            blendSubPoint = new System.Drawing.Drawing2D.ColorBlend(3);
                                            blendSubPoint.Colors = new Color[] { colorA, colorB, colorA };
                                            blendSubPoint.Positions = new float[] { 0f, 0.5f, 1f };
                                            brushSubPointFill.InterpolationColors = blendSubPoint;

                                            g.FillRectangle(brushSubPointFill, rectSubPoint);
                                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);

                                            g.DrawString(RoundValues ? points.ToString("F") :
                                                points.ToString(), this.Font, brushOverall, new Rectangle((int)graphStart + barStart + 2, 50 + i * 36, 50, 24), formatOverall);


                                        }
                                    }

                                }
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

            Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);
        }

        private void ComparisonGraph_Resize(object sender, EventArgs e)
        {
            int scrollValue = -1;
            if (_scrollBar != null)
                scrollValue = _scrollBar.Value;
            if (_prerenderedGraph != null)
                _prerenderedGraph.Dispose();
            _prerenderedGraph = null;
            this.Invalidate();
            PrerenderedGraph.ToString(); //render it, so we can scroll back to the previous spot
            if (scrollValue > 0)
                _scrollBar.Value = scrollValue;
        }

        void Calculations_ModelChanged(object sender, EventArgs e)
        {
            _prerenderedGraph = null;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawImageUnscaled(PrerenderedGraph, 0, 0 - (_scrollBar != null ? _scrollBar.Value : 0));
        }

        public Character.CharacterSlot EquipSlot = Character.CharacterSlot.None;
        public SortedList<Item.ItemSlot, Character.CharacterSlot> SlotMap
        {
            get;
            set;
        }
        private void ComparisonGraph_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            if (e.X <= 106)
            {
                int itemIndex = (int) Math.Floor(((float) (e.Y - 44f + _scrollBar.Value)) / 36f);
                if (itemIndex >= 0 && itemIndex < ItemCalculations.Length && ItemCalculations[itemIndex].Item != null && ItemCalculations[itemIndex].Item.Id != 0)
                {
                    Item item = ItemCalculations[itemIndex].Item;
                    if (e.X < 10)
                    {
                        if (item != null && item.Id != 0)
                        {
                            if ((e.Button & MouseButtons.Right) != 0)
                            {
                                if (item.Id > 0 && !item.IsGem) ItemEnchantContextualMenu.Instance.Show(item);
                            }
                            else
                            {
                                Character.ToggleItemAvailability(item, (Control.ModifierKeys & Keys.Control) == 0);
                            }
                        }
                    }
                    else if (item.Id > 0)
                    {
                        Character.CharacterSlot slot = EquipSlot;
                        Item showItem = ItemCalculations[itemIndex].Item;
                        if (slot == Character.CharacterSlot.AutoSelect)
                        {
                            if (SlotMap != null && SlotMap.ContainsKey(showItem.Slot))
                            {
                                slot = SlotMap[showItem.Slot];
                            }
                            else
                            {
                                slot = Item.DefaultSlotMap[showItem.Slot];
                            }
                        }

                        ItemContextualMenu.Instance.Show(showItem, slot, ItemCalculations[itemIndex].Character, true);

                    }
                }
            }
            //}
        }

        private void ComparisonGraph_MouseLeave(object sender, EventArgs e)
        {
            HideTooltip();
        }

        private Point _lastPoint = Point.Empty;
        private void ComparisonGraph_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Location != _lastPoint)
            {
                _lastPoint = e.Location;
                Cursor cursor = Cursors.Default;
                if (e.X <= 118)
                {
                    int itemIndex = (int) Math.Floor(((float) (e.Y - 44f + _scrollBar.Value)) / 36f);
                    if (itemIndex >= 0 && itemIndex < ItemCalculations.Length)
                    {
                        Item item = ItemCalculations[itemIndex].Item;
                        Character itemCharacter = ItemCalculations[itemIndex].Character;
                        Enchant itemEnchant = ItemCalculations[itemIndex].Enchant;
                        if (e.X < 10 && item != null && item.Id != 0)
                            cursor = Cursors.Hand;

                        if (item != _tooltipItem)
                        {
                            int tipX = 118;
                            if (Parent.PointToScreen(Location).X + tipX + 249 > System.Windows.Forms.Screen.GetWorkingArea(this).Right)
                                tipX = -249;

                            ShowTooltip(item, itemCharacter, itemEnchant, new Point(tipX, 26 + (itemIndex * 36) - _scrollBar.Value));
                        }
                    }
                    else
                    {
                        HideTooltip();
                    }
                }
                else
                {
                    HideTooltip();
                    //ItemTooltip.Instance.HideTooltip();
                }
                if (Cursor != cursor)
                    Cursor = cursor;
            }
        }

        private void ShowTooltip(Item item, Character itemCharacter, Enchant itemEnchant, Point location)
        {
            if (_tooltipItem != item || _tooltipLocation != location)
            {
                _tooltipItem = item;
                _tooltipItemCharacter = itemCharacter;
                _tooltipItemEnchant = itemEnchant;
                _tooltipLocation = location;
                ShowHideTooltip();
            }
        }

        private void HideTooltip()
        {
            if (_tooltipItem != null)
            {
                _tooltipItem = null;
                _tooltipItemCharacter = null;
                _tooltipItemEnchant = null;
                ShowHideTooltip();
            }
        }

        private Item _tooltipItem = null;
        private Character _tooltipItemCharacter = null;
        private Enchant _tooltipItemEnchant = null;
        private Point _tooltipLocation = Point.Empty;
        private void ShowHideTooltip()
        {
            if (_tooltipItem != null && _tooltipLocation != Point.Empty)
            {
                ItemToolTip.Instance.Show(_tooltipItem, _tooltipItemCharacter, _tooltipItemEnchant, this, _tooltipLocation);
            }
            else
            {
                ItemToolTip.Instance.Hide(this);
            }
        }


    }
}
