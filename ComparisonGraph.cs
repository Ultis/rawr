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

		public void LoadItemCalculationsPreSorted(ComparisonCalculationBase[] itemCalculations)
		{
			_itemCalculations = itemCalculations;
			if (_prerenderedGraph != null)
				_prerenderedGraph.Dispose();
			_prerenderedGraph = null;
			this.Invalidate();
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

        public int CompareItemCalculations(ComparisonCalculationBase a, ComparisonCalculationBase b)
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

        private string Shorten(float number)
        {
            string sign = number < 0 ? "-" : "";
            number = (float)Math.Abs(number);
            if (number < 100) return sign + Math.Round(number, 2).ToString();
            else if (number < 1000) return sign + Math.Round(number).ToString();
            else if (number < 100000) return sign + Math.Round(number / 1000, 1).ToString() + "k";
            else if (number < 1000000) return sign + Math.Round(number / 1000).ToString() + "k";
            else return sign + Math.Round(number / 1000000, 2).ToString() + "m";
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
                    else if (ItemCalculations.Length > 0)
                    {

                        float minOverallPoints = 0f;
                        float maxOverallPoints = 0f;
                        foreach (ComparisonCalculationBase calc in ItemCalculations)
                        {
                            if (DisplayMode == GraphDisplayMode.Overall)
                            {
                                maxOverallPoints = Math.Max(maxOverallPoints, calc.OverallPoints);
                                minOverallPoints = Math.Min(minOverallPoints, calc.OverallPoints);
                            }
                            else if (DisplayMode != GraphDisplayMode.Subpoints && Sort != ComparisonSort.Alphabetical && Sort != ComparisonSort.Overall)
                            {
                                maxOverallPoints = Math.Max(maxOverallPoints, calc.SubPoints[(int)Sort]);
                                minOverallPoints = Math.Min(minOverallPoints, calc.SubPoints[(int)Sort]);
                            }
                            else
                            {
                                float pos = 0, neg = 0;
                                foreach (float f in calc.SubPoints)
                                {
                                    if (f < 0) neg += f;
                                    else pos += f;
                                }
                                maxOverallPoints = Math.Max(maxOverallPoints, pos);
                                minOverallPoints = Math.Min(minOverallPoints, neg);
                            }
                        }
                        if (maxOverallPoints == 0f && minOverallPoints == 0f) maxOverallPoints = 2f;
                        maxOverallPoints = (float)Math.Ceiling(Math.Round(maxOverallPoints, 2));
                        float maxScale = 10f;
                        float maxRoundTo = 2f;
                        if (maxOverallPoints >= 10) maxRoundTo = (int)Math.Pow(10, Math.Floor(Math.Log10(maxOverallPoints) - .3f));
                        maxScale = maxRoundTo * (float)Math.Ceiling(maxOverallPoints / maxRoundTo);

                        minOverallPoints = (float)Math.Floor(Math.Round(minOverallPoints, 2));
                        float minScale = 0f;
                        float minRoundTo = 2f;
                        if (minOverallPoints <= -10) minRoundTo = (int)Math.Pow(10, Math.Floor(Math.Log10(-minOverallPoints) - .3f));
                        minScale = minRoundTo * (float)Math.Floor(minOverallPoints / minRoundTo);

                        float totalScale = maxScale - minScale;

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
                        #endregion

                        #region Graph Ticks
                        float graphStart = 120f;
                        float graphWidth = this.Width - 160f;
                        float graphEnd = graphStart + graphWidth;

                        Dictionary<float, float> ticks = new Dictionary<float, float>();
                        float graphZero = (float)Math.Round(graphStart - minScale / totalScale * graphWidth);
                        ticks[0] = graphZero;

                        int maxTicks = (int)Math.Floor(maxScale / totalScale * 8f);
                        int minTicks = (int)Math.Floor(-minScale / totalScale * 8f);

                        float tickInc = (float)(Math.Floor(totalScale / Math.Max(maxRoundTo, minRoundTo)) * Math.Max(maxRoundTo, minRoundTo) / 8f);

                        for (int i = 0; i < maxTicks; i++)
                        {
                            ticks[(i + 1) * tickInc] = (float)Math.Round(graphZero + (i + 1) * tickInc / totalScale * graphWidth);
                        }

                        for (int i = 0; i < minTicks; i++)
                        {
                            ticks[(i + 1) * -tickInc] = (float)Math.Round(graphZero + (i + 1) * -tickInc / totalScale * graphWidth);
                        }

                        #region Pens
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
                        #endregion

                        bool posAlt = false, negAlt = false;
                        foreach (KeyValuePair<float, float> kvp in ticks)
                        {
                            Pen p1;
                            Pen p2;
                            Brush b;
                            if (kvp.Key == 0)
                            {
                                p1 = black200;
                                p2 = black200;
                                b = black200brush;
                            }
                            else if ((kvp.Key < 0 && negAlt) || (kvp.Key > 0 && posAlt))
                            {
                                p1 = black150;
                                p2 = black50;
                                b = black150brush;
                            }
                            else
                            {
                                p1 = black75;
                                p2 = black25;
                                b = black75brush;
                            }
                            g.DrawLine(p1, kvp.Value, 36, kvp.Value, 39);
                            g.DrawLine(p2, kvp.Value, 41, kvp.Value, _prerenderedGraph.Height);
                            g.DrawString(Shorten(kvp.Key), this.Font, b, kvp.Value, 36, formatTick);
                            if (kvp.Key < 0) negAlt = !negAlt;
                            else if (kvp.Key > 0) posAlt = !posAlt;
                        }
                        g.DrawLine(black200, graphStart - 4, 40, graphEnd + 4, 40);
                        #endregion

                        bool hasItemAvailabilty = false;

                        for (int itemNumber = 0; itemNumber < ItemCalculations.Length; itemNumber++)
                        {
                            ComparisonCalculationBase item = ItemCalculations[itemNumber];

                            #region Item Name
                            Rectangle rectItemName = new Rectangle(10, 44 + itemNumber * 36, (int)(graphStart - 14), 36);
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
                                hasItemAvailabilty = true;
                                Character.ItemAvailability itemAvailability;
                                if (item.ItemInstance != null)
                                {
                                    itemAvailability = Character.GetItemAvailability(item.ItemInstance);
                                }
                                else
                                {
                                    itemAvailability = Character.GetItemAvailability(item.Item);
                                }
                                switch (itemAvailability)
                                {
                                    case Character.ItemAvailability.RegemmingAllowed:
                                        g.DrawImageUnscaled(bmpDiamond, 0, 55 + (itemNumber * 36));
                                        break;
                                    case Character.ItemAvailability.RegemmingAllowedWithEnchantRestrictions:
                                        g.DrawImageUnscaled(bmpDiamond3, 0, 55 + (itemNumber * 36));
                                        break;
                                    case Character.ItemAvailability.Available:
                                        g.DrawImageUnscaled(bmpDiamond2, 0, 55 + (itemNumber * 36));
                                        break;
                                    case Character.ItemAvailability.AvailableWithEnchantRestrictions:
                                        g.DrawImageUnscaled(bmpDiamond4, 0, 55 + (itemNumber * 36));
                                        break;
                                    case Character.ItemAvailability.NotAvailable:
                                        g.DrawImageUnscaled(bmpDiamondOutline, 0, 55 + (itemNumber * 36));
                                        break;
                                }
                            }

                            g.DrawString(item.Name, this.Font, brushItemNames, rectItemName, formatItemNames);
                            #endregion

                            if (item.OverallPoints < 0.00001f || item.OverallPoints > 0.00001f || DisplayMode == GraphDisplayMode.Overall || DisplayMode == GraphDisplayMode.CustomSubpoints)
                            {
                                int posStart = (int)graphZero + 1;
                                int negStart = (int)graphZero - 1;
                                int barWidth = 0;
                                float scale = 1;

                                #region Sub Point Display Mode
                                if (DisplayMode == GraphDisplayMode.Subpoints)
                                {
                                    for (int subNumber = 0; subNumber < item.SubPoints.Length && subNumber < colorSubPointsA.Length; subNumber++)
                                    {
                                        float subPoint = item.SubPoints[subNumber];

                                        if (subPoint < 0)
                                        {
                                            barWidth = (int)Math.Round(subPoint / minScale * (graphZero - graphStart));
                                            rectSubPoint = new Rectangle(negStart - barWidth, 50 + itemNumber * 36, barWidth, 24);
                                            negStart -= barWidth;
                                            scale = minScale;
                                        }
                                        else if (subPoint > 0)
                                        {
                                            barWidth = (int)Math.Round(subPoint / maxScale * (graphEnd - graphZero));
                                            rectSubPoint = new Rectangle(posStart, 50 + itemNumber * 36, barWidth, 24);
                                            posStart += barWidth;
                                            scale = maxScale;
                                        }
                                        else
                                        {
                                            rectSubPoint = new Rectangle(posStart, 50 + itemNumber * 36, 0, 24);
                                        }

                                        if (barWidth > 0)
                                        {
                                            brushSubPointFill = new System.Drawing.Drawing2D.LinearGradientBrush(rectSubPoint, colorSubPointsA[subNumber], colorSubPointsB[subNumber],
                                                67f + (20f * subPoint / scale));
                                            blendSubPoint = new System.Drawing.Drawing2D.ColorBlend(3);
                                            blendSubPoint.Colors = new Color[] { colorSubPointsA[subNumber], colorSubPointsB[subNumber], colorSubPointsA[subNumber] };
                                            blendSubPoint.Positions = new float[] { 0f, 0.5f, 1f };
                                            brushSubPointFill.InterpolationColors = blendSubPoint;

                                            g.FillRectangle(brushSubPointFill, rectSubPoint);
                                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                            g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);

                                            if (Math.Abs(rectSubPoint.Width) > 7)
                                                g.DrawString(subPoint.ToString("F"),
                                                    this.Font, brushSubPoints[subNumber], rectSubPoint, formatSubPoint);
                                        }

                                    }
                                    g.DrawString(RoundValues ? item.OverallPoints.ToString("F") :
                                            item.OverallPoints.ToString(), this.Font, brushOverall, new Rectangle(posStart + 2, 50 + itemNumber * 36, 50, 24), formatOverall);
                                }
                                #endregion

                                #region Overall Display Mode
                                else if (DisplayMode == GraphDisplayMode.Overall || DisplayMode == GraphDisplayMode.CustomSubpoints)
                                {
                                    float points = item.OverallPoints;
                                    Color colorA = Color.FromArgb(128, 64, 0, 64);
                                    Color colorB = Color.FromArgb(128, 128, 0, 128);
                                    if (DisplayMode != GraphDisplayMode.Overall &&Sort != ComparisonSort.Alphabetical && Sort != ComparisonSort.Overall)
                                    {
                                        points = item.SubPoints[(int)Sort];

                                        if (DisplayMode != GraphDisplayMode.CustomSubpoints)
                                        {
                                            colorA = colorSubPointsA[(int)Sort];
                                            colorB = colorSubPointsB[(int)Sort];
                                        }
                                    }
                                    if (points < 0)
                                    {
                                        barWidth = (int)Math.Round(points / minScale * (graphZero - graphStart));
                                        rectSubPoint = new Rectangle(negStart - barWidth, 50 + itemNumber * 36, barWidth, 24);
                                        scale = minScale;
                                    }
                                    else if (points > 0)
                                    {
                                        barWidth = (int)Math.Round(points / maxScale * (graphEnd - graphZero));
                                        rectSubPoint = new Rectangle(posStart, 50 + itemNumber * 36, barWidth, 24);
                                        scale = maxScale;
                                    }
                                    else
                                    {
                                        rectSubPoint = new Rectangle(posStart, 50 + itemNumber * 36, barWidth, 24);
                                    }

                                    if (barWidth > 0)
                                    {
                                        brushSubPointFill = new System.Drawing.Drawing2D.LinearGradientBrush(rectSubPoint, colorA, colorB,
                                            67f + (20f * points / scale));
                                        blendSubPoint = new System.Drawing.Drawing2D.ColorBlend(3);
                                        blendSubPoint.Colors = new Color[] { colorA, colorB, colorA };
                                        blendSubPoint.Positions = new float[] { 0f, 0.5f, 1f };
                                        brushSubPointFill.InterpolationColors = blendSubPoint;

                                        g.FillRectangle(brushSubPointFill, rectSubPoint);
                                        g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                        g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);
                                        g.DrawRectangle(new Pen(brushSubPointFill), rectSubPoint);

                                        g.DrawString(RoundValues ? points.ToString("F") :
                                            points.ToString(), this.Font, brushOverall, new Rectangle(posStart + barWidth + 2, 50 + itemNumber * 36, 50, 24), formatOverall);
                                    }
                                }
                                #endregion

                            }
                        }
                        if (hasItemAvailabilty)
                        {
                            g.DrawImageUnscaled(bmpDiamond, legendX, 2);
                            g.DrawString("=", this.Font, new SolidBrush(this.ForeColor), legendX + 12, 3);
                            g.DrawString("Available for Optimizer", this.Font, new SolidBrush(this.ForeColor), legendX + 24, 4);
                        }
                    }
                    g.Dispose();
                    if (CustomRendered)
                    {
                        _scrollBar.Visible = false;
                    }
                    else
                    {
                        _scrollBar.Visible = true;
                    }
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
			this.MouseWheel += new MouseEventHandler(ComparisonGraph_MouseWheel);
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
                    ItemInstance itemInstance = ItemCalculations[itemIndex].ItemInstance;
                    Item item = ItemCalculations[itemIndex].Item;
					if (item != null && itemInstance == null) itemInstance = GemmingTemplate.CurrentTemplates[0].GetItemInstance(item, null, false);
                    if (e.X < 10)
                    {
                        if (itemInstance != null && itemInstance.Id != 0)
                        {
                            if ((e.Button & MouseButtons.Right) != 0)
                            {
                                if (itemInstance.Id > 0) ItemEnchantContextualMenu.Instance.Show(itemInstance);
                            }
                            else
                            {
                                Character.ToggleItemAvailability(itemInstance, (Control.ModifierKeys & Keys.Control) == 0);
                            }
                        }
                        else if (item != null && item.Id != 0)
                        {
                            if ((e.Button & MouseButtons.Right) != 0)
                            {
                                //if (item.Id > 0) ItemEnchantContextualMenu.Instance.Show(itemInstance);
                            }
                            else
                            {
                                Character.ToggleItemAvailability(item, (Control.ModifierKeys & Keys.Control) == 0);
                            }
                        }
                    }
                    else if (itemInstance != null && itemInstance.Id > 0)
                    {
                        Character.CharacterSlot slot = EquipSlot;
                        //ItemInstance showItem = ItemCalculations[itemIndex].ItemInstance;
                        if (slot == Character.CharacterSlot.AutoSelect)
                        {
                            if (SlotMap != null && SlotMap.ContainsKey(itemInstance.Slot))
                            {
								slot = SlotMap[itemInstance.Slot];
                            }
                            else
                            {
								slot = Item.DefaultSlotMap[itemInstance.Slot];
                            }
                        }

						ItemContextualMenu.Instance.Show(itemInstance, slot, ItemCalculations[itemIndex].CharacterItems, true);

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
                        ItemInstance itemInstance = ItemCalculations[itemIndex].ItemInstance;
                        ItemInstance[] characterItems = ItemCalculations[itemIndex].CharacterItems;
                        Enchant itemEnchant = null;
                        if (ItemCalculations[itemIndex].ItemInstance != null) itemEnchant = ItemCalculations[itemIndex].ItemInstance.Enchant;
                        if (e.X < 10 && item != null && item.Id != 0)
                            cursor = Cursors.Hand;

                        if (item != _tooltipItem)
                        {
                            int tipX = 118;
                            if (Parent.PointToScreen(Location).X + tipX + 249 > System.Windows.Forms.Screen.GetWorkingArea(this).Right)
                                tipX = -249;
                            if (itemInstance != null)
                            {
                                ShowTooltip(itemInstance, characterItems, new Point(tipX, 26 + (itemIndex * 36) - _scrollBar.Value));
                            }
                            else
                            {
                                ShowTooltip(item, new Point(tipX, 26 + (itemIndex * 36) - _scrollBar.Value));
                            }
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

        private void ShowTooltip(ItemInstance item, ItemInstance[] characterItems, Point location)
        {
            if (_tooltipItemInstance != item || _tooltipLocation != location)
            {
                _tooltipItem = null;
                _tooltipItemInstance = item;
                _tooltipCharacterItems = characterItems;
                _tooltipLocation = location;
                ShowHideTooltip();
            }
        }

        private void ShowTooltip(Item item, Point location)
        {
            if (_tooltipItem != item || _tooltipLocation != location)
            {
                _tooltipItem = item;
                _tooltipItemInstance = null;
                _tooltipCharacterItems = null;
                _tooltipLocation = location;
                ShowHideTooltip();
            }
        }

        private void HideTooltip()
        {
            if (_tooltipItem != null || _tooltipItemInstance != null)
            {
                _tooltipItem = null;
                _tooltipItemInstance = null;
                _tooltipCharacterItems = null;
                ShowHideTooltip();
            }
        }

        private ItemInstance _tooltipItemInstance = null;
        private Item _tooltipItem = null;
        private ItemInstance[] _tooltipCharacterItems = null;
        private Point _tooltipLocation = Point.Empty;
        private void ShowHideTooltip()
        {
            if (_tooltipItem != null && _tooltipLocation != Point.Empty)
			{
				ItemToolTip.Instance.Show(_tooltipItem, _tooltipCharacterItems, EquipSlot, this, _tooltipLocation);
            }
            else if (_tooltipItemInstance != null && _tooltipLocation != Point.Empty)
            {
                ItemToolTip.Instance.Show(_tooltipItemInstance, _tooltipCharacterItems, EquipSlot, this, _tooltipLocation);
            }
            else
            {
                ItemToolTip.Instance.Hide(this);
            }
        }

		private void ComparisonGraph_MouseWheel(object sender, MouseEventArgs e)
		{
			ScrollBar.Value = Math.Max(ScrollBar.Minimum, Math.Min(ScrollBar.Maximum - ScrollBar.LargeChange, ScrollBar.Value - e.Delta));
            _scrollBar_Scroll(this, null);
		}
    }
}
