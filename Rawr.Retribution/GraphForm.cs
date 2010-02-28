using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Retribution
{
    public partial class GraphForm : Form
    {
        private Graphics gfx;
        private Bitmap bmp = null;

        public GraphForm(Character character)
        {
            InitializeComponent();

            // Create ARGB bitmap matching the size of the picturebox and associate with picturebox
            bmp = new Bitmap(pictureBoxGraph.Size.Width, pictureBoxGraph.Size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            pictureBoxGraph.Image = bmp;

            // Greate & initialise GDI+ graphics object via bitmap
            gfx = Graphics.FromImage(bmp);
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            
            #region Coordinates and scaling.
            /* 
             *   +-----------------------------------------------------+-rcBmp  ---
             *   |                                                     |         |EndY
             *   |    ^                                                |        ---
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    |                                                |
             *   |    +--------------------------------------------->  | ---
             *   |                                                     |  |  OrgY
             *   +-----------------------------------------------------+ ---
             *   
             *   |----|OrgX                                         |--|EndX
             * 
             * 
             * 
             */

            // Apply a translateion transformation. We'll move the 0,0 to the 0,0 of the chart we want to display
            // This will make most of the rest of the charting easier since all translation are then done by GDI+
            // It also makes working with 'y' a bit easier as it's just flipped in sign rather than translated also.
            int OrgX = 50;  // Leftmost edge of char
            int OrgY = 50;  // Bottom edge of char
            int EndX = 15;  // Margin right of chart
            int EndY = 15;  // Margin top of chart
            gfx.TranslateTransform(OrgX, pictureBoxGraph.Size.Height - OrgY);

            // Bitmap coordinates translated back into transform
            Rectangle rcBmp = new Rectangle(0, 0, pictureBoxGraph.Size.Width, pictureBoxGraph.Size.Height);
            rcBmp.Offset(-OrgX, -(pictureBoxGraph.Size.Height - OrgY));

            // rcChart is image rectangle, watch out, Y has positive value !
            Rectangle rcChart = new Rectangle(0, 0, pictureBoxGraph.Size.Width - OrgX - EndX, pictureBoxGraph.Size.Height - OrgY - EndY);
            int YAxisValueX = - 5; // Right alignment of values on the Y axis
            int XAxisValueY = + 5; // Top margin of values on the X axis
            int YChartMargin = 5;
            #endregion

            #region Fonts, Pens, Brushes and other helper variables
            Font fntValue = new Font("Arial", 8);
            Font fntTitle = new Font("Arial", 16);

            Pen penBlack = new Pen(Color.FromArgb(255, 0, 0, 0));
            Pen penRed = new Pen(Color.FromArgb(255, 255, 0, 0));

            Brush brBlack = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            Brush brWhite = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            Brush brRed = new SolidBrush(Color.FromArgb(255, 255, 0, 0));

            StringFormat fmtRAlignCenter = new StringFormat();
            fmtRAlignCenter.Alignment = StringAlignment.Far;
            fmtRAlignCenter.LineAlignment = StringAlignment.Center;

            StringFormat fmtRAlignBottom = new StringFormat();
            fmtRAlignBottom.Alignment = StringAlignment.Far;
            fmtRAlignBottom.LineAlignment = StringAlignment.Far;

            StringFormat fmtLAlignTop = new StringFormat();
            fmtLAlignTop.Alignment = StringAlignment.Near;
            fmtLAlignTop.LineAlignment = StringAlignment.Near;

            StringFormat fmtCAlignTop = new StringFormat();
            fmtCAlignTop.Alignment = StringAlignment.Center;
            fmtCAlignTop.LineAlignment = StringAlignment.Near;
            #endregion

            // Clear bitmap to all white.
            gfx.FillRectangle(brWhite, rcBmp);

            #region Draw Axis and titles
            // Y Axis
            gfx.DrawLine(penBlack, 0, 0, 0, -rcChart.Height);
            gfx.DrawString("DPS", fntTitle, brBlack, 5, -(rcChart.Height - 5), fmtLAlignTop);

            // X-axis
            gfx.DrawLine(penBlack, 0, 0, rcChart.Width, 0); 
            gfx.DrawString("iLevel", fntTitle, brBlack, rcChart.Width - 5, -5, fmtRAlignBottom);

            // Center Y Axis.
            gfx.DrawLine(penBlack, rcChart.Width / 2, 0, rcChart.Width / 2, -rcChart.Height);
            gfx.DrawString("0", fntValue, brBlack, XAxisValueY, rcChart.Width / 2, fmtCAlignTop);
            
            // "Experimental" text
            gfx.DrawString("Experimental", fntValue, brRed, rcChart.Width-5, OrgY, fmtRAlignBottom);
            #endregion

            // Define charts
            ChartData[] aCharts = new ChartData[] {
                new ChartData(rcChart.Width, Color.FromArgb(255, 255, 0, 0), "Haste", new Stats() { HasteRating = 1 }),
                //new ChartData(rcChart.Width, Color.FromArgb(255, 0, 0, 255), "Strength", new Stats() { Strength = 1 }),
            };

            // Calculate charts
            CalculationsRetribution Calc = new CalculationsRetribution();
            float DpsMax = 0;
            float DpsMin = 999999999;
            foreach (ChartData cd in aCharts)
            {
                for (int count = 0; count < rcChart.Width; count++)
                {
                    Stats chartstats = cd.stats.Clone();
                    chartstats *= count - (rcChart.Width / 2);

                    CharacterCalculationsRetribution chartCalc = Calc.GetCharacterCalculations(character, new Item() { Stats = chartstats }) as CharacterCalculationsRetribution;
                    float Dps = chartCalc.DPSPoints;

                    if (Dps > DpsMax)
                        DpsMax = Dps;
                    if (Dps < DpsMin)
                        DpsMin = Dps;

                    cd.dps[count] = Dps;
                }
            }
            float DpsScaling = (rcChart.Height - 2*YChartMargin/* some extra margin*/) / (DpsMax - DpsMin);

            // Draw charts
            foreach (ChartData cd in aCharts)
            {
                for (int count = 0; count < rcChart.Width - 1; count++)
                    gfx.DrawLine(cd.pen, count, -(cd.dps[count] - DpsMin) * DpsScaling - YChartMargin, count + 1, -(cd.dps[count + 1] - DpsMin) * DpsScaling - YChartMargin);
            }
            Invalidate();
            Update();
        }
    }

    /// <summary>
    /// Helper class for charting
    /// </summary>
    public class ChartData
    {
        public ChartData(int points, Color color, string name, Stats stats)
        { 
            dps = new float[points];
            pen = new Pen(color);
            brush = new SolidBrush(color);
            this.name = name;
            this.stats = stats;
        }

        public float[] dps;
        public Pen pen;
        public Brush brush;
        public string name;
        public Stats stats;
    };

}
