using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Rawr.Base
{
    public partial class Graph : Form
    {
        private Bitmap bitGraph;
        private Graphics g;
        
        public Graph()
        {
            bitGraph = new Bitmap(800, 750);
            this.g = Graphics.FromImage(bitGraph);
            InitializeComponent();
        }
       
        private void Graph_Load(object sender, EventArgs e)
        {
            pictureBoxGraph.Image = bitGraph;
            pictureBoxGraph.SizeMode = PictureBoxSizeMode.Zoom;
        }

        public void SetupGraph(Character character, Stats[] statsList, string explanatoryText, string calculation)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.Text = "Graph of " + calculation;
            CharacterCalculationsBase baseCalc = Calculations.GetCharacterCalculations(character);
            float baseFigure = GetCalculationValue(baseCalc, calculation);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            float graphHeight = 750f, graphOffset = 400f, graphStep = 3.5f;
            Color[] colors = new Color[] {
                Color.FromArgb(255,202,180,96), 
                Color.FromArgb(255,101,225,240),
                Color.FromArgb(255,0,4,3), 
                Color.FromArgb(255,238,238,30),
                Color.FromArgb(255,45,112,63), 
                Color.FromArgb(255,121,72,210), 
                Color.FromArgb(255,217,100,54), 
                Color.FromArgb(255,210,72,195), 
                Color.FromArgb(255,206,189,191), 
            };
            if (statsList.Length == 0 || statsList.Length > 9) return; // more than 9 elements for the array would run out of colours
            float minDpsChange = 0f, maxDpsChange = 0f;
            Point[][] points = new Point[statsList.Length][];
            for (int index = 0; index < statsList.Length; index++)
            {
                Stats newStats = new Stats();
                points[index] = new Point[201];
                newStats.Accumulate(statsList[index], -101);
                for (int count = -100; count <= 100; count++)
                {
                    newStats.Accumulate(statsList[index]);

                    CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(character, new Item() { Stats = newStats }, false, false, false);
                    float currentFigure = GetCalculationValue(currentCalc, calculation);
                    float dpsChange = currentFigure - baseFigure;
                    points[index][count + 100] = new Point(Convert.ToInt32(graphOffset + count * graphStep), Convert.ToInt32(dpsChange));
                    if (dpsChange < minDpsChange)
                        minDpsChange = dpsChange;
                    if (dpsChange > maxDpsChange)
                        maxDpsChange = dpsChange;
                }
            }
            float DpsVariance = maxDpsChange - minDpsChange;
            if (DpsVariance == 0)
                DpsVariance = 1;
            for (int index = 0; index < statsList.Length; index++)
            {
                for (int count = -100; count <= 100; count++)
                {
                    points[index][count + 100].Y = (int)((maxDpsChange - points[index][count + 100].Y) * (graphHeight - 48) / DpsVariance) + 20;
                }
                Brush statBrush = new SolidBrush(colors[index]);
                g.DrawLines(new Pen(statBrush, 3), points[index]);
            }

            #region Graph X Ticks
            float graphStart = graphOffset - 100 * graphStep;
            float graphEnd = graphOffset + 100 * graphStep;
            float graphWidth = graphEnd - graphStart;

            float maxScale = 200f;
            float[] ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};
            Pen ZeroLine = new Pen(Color.FromArgb(100, 0, 0, 0), 3);
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
            g.DrawLine(black200, graphStart, 16, graphStart, graphHeight - 16);
            g.DrawLine(black200, graphEnd, 16, graphEnd, 19);
            g.DrawLine(ZeroLine, ticks[0], 16, ticks[0], 19);
            g.DrawLine(black150, ticks[1], 16, ticks[1], 19);
            g.DrawLine(black150, ticks[2], 16, ticks[2], 19);
            g.DrawLine(black75, ticks[3], 16, ticks[3], 19);
            g.DrawLine(black75, ticks[4], 16, ticks[4], 19);
            g.DrawLine(black75, ticks[5], 16, ticks[5], 19);
            g.DrawLine(black75, ticks[6], 16, ticks[6], 19);
            g.DrawLine(black75, graphEnd, 21, graphEnd, graphHeight - 16);
            g.DrawLine(ZeroLine, ticks[0], 21, ticks[0], graphHeight - 16);
            g.DrawLine(black50, ticks[1], 21, ticks[1], graphHeight - 16);
            g.DrawLine(black50, ticks[2], 21, ticks[2], graphHeight - 16);
            g.DrawLine(black25, ticks[3], 21, ticks[3], graphHeight - 16);
            g.DrawLine(black25, ticks[4], 21, ticks[4], graphHeight - 16);
            g.DrawLine(black25, ticks[5], 21, ticks[5], graphHeight - 16);
            g.DrawLine(black25, ticks[6], 21, ticks[6], graphHeight - 16);
            g.DrawLine(black200, graphStart - 4, graphHeight - 20, graphEnd + 4, graphHeight - 20);

            Font tickFont = new Font("Calibri", 11);
            g.DrawString((-100f).ToString(), tickFont, black200brush, graphStart, 16, formatTick);
            g.DrawString((maxScale - 100f).ToString(), tickFont, black200brush, graphEnd, 16, formatTick);
            g.DrawString((maxScale * 0.5f - 100f).ToString(), tickFont, black200brush, ticks[0], 16, formatTick);
            g.DrawString((maxScale * 0.75f - 100f).ToString(), tickFont, black150brush, ticks[1], 16, formatTick);
            g.DrawString((maxScale * 0.25f - 100f).ToString(), tickFont, black150brush, ticks[2], 16, formatTick);
            g.DrawString((maxScale * 0.125f - 100f).ToString(), tickFont, black75brush, ticks[3], 16, formatTick);
            g.DrawString((maxScale * 0.375f - 100f).ToString(), tickFont, black75brush, ticks[4], 16, formatTick);
            g.DrawString((maxScale * 0.625f - 100f).ToString(), tickFont, black75brush, ticks[5], 16, formatTick);
            g.DrawString((maxScale * 0.875f - 100f).ToString(), tickFont, black75brush, ticks[6], 16, formatTick);

            g.DrawString((-100f).ToString(), tickFont, black200brush, graphStart, graphHeight - 16, formatTick);
            g.DrawString((maxScale - 100f).ToString(), tickFont, black200brush, graphEnd, graphHeight - 16, formatTick);
            g.DrawString((maxScale * 0.5f - 100f).ToString(), tickFont, black200brush, ticks[0], graphHeight - 16, formatTick);
            g.DrawString((maxScale * 0.75f - 100f).ToString(), tickFont, black150brush, ticks[1], graphHeight - 16, formatTick);
            g.DrawString((maxScale * 0.25f - 100f).ToString(), tickFont, black150brush, ticks[2], graphHeight - 16, formatTick);
            g.DrawString((maxScale * 0.125f - 100f).ToString(), tickFont, black75brush, ticks[3], graphHeight - 16, formatTick);
            g.DrawString((maxScale * 0.375f - 100f).ToString(), tickFont, black75brush, ticks[4], graphHeight - 16, formatTick);
            g.DrawString((maxScale * 0.625f - 100f).ToString(), tickFont, black75brush, ticks[5], graphHeight - 16, formatTick);
            g.DrawString((maxScale * 0.875f - 100f).ToString(), tickFont, black75brush, ticks[6], graphHeight - 16, formatTick);
            g.DrawString("Stat Change", tickFont, black200brush, graphWidth / 2 + 50, graphHeight, formatTick);
            #endregion

            #region Graph Y ticks
            Int32 zeroPoint = (int)(maxDpsChange * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(ZeroLine, graphStart, zeroPoint, graphEnd, zeroPoint);
            formatTick.Alignment = StringAlignment.Near;
            StringFormat formatName = new StringFormat(StringFormatFlags.DirectionVertical);
            g.DrawString(calculation, tickFont, black200brush, graphStart - 50, graphHeight / 2, formatName);
            g.DrawString("0", tickFont, black200brush, graphStart - 20, zeroPoint + 10, formatTick);
            g.DrawString(maxDpsChange.ToString("F1", CultureInfo.InvariantCulture), tickFont, black200brush, graphStart - 50, 30, formatTick);
            g.DrawString(minDpsChange.ToString("F1", CultureInfo.InvariantCulture), tickFont, black200brush, graphStart - 50, graphHeight - 12, formatTick);
            float pointY = (int)(maxDpsChange * .75f * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black75, graphStart, pointY, graphEnd, pointY);
            g.DrawString((maxDpsChange * .25f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black75brush, graphStart - 50, pointY + 10, formatTick);
            pointY = (int)(maxDpsChange * .5f * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black150, graphStart, pointY, graphEnd, pointY);
            g.DrawString((maxDpsChange * .5f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black150brush, graphStart - 50, pointY + 10, formatTick);
            pointY = (int)(maxDpsChange * .25f * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black75, graphStart, pointY, graphEnd, pointY);
            g.DrawString((maxDpsChange * .75f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black75brush, graphStart - 50, pointY + 10, formatTick);
            pointY = (int)((maxDpsChange - minDpsChange * .75f) * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black75, graphStart, pointY, graphEnd, pointY);
            g.DrawString((minDpsChange * .75f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black75brush, graphStart - 50, pointY + 12, formatTick);
            pointY = (int)((maxDpsChange - minDpsChange * .5f) * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black150, graphStart, pointY, graphEnd, pointY);
            g.DrawString((minDpsChange * .5f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black150brush, graphStart - 50, pointY + 12, formatTick);
            pointY = (int)((maxDpsChange - minDpsChange * .25f) * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black75, graphStart, pointY, graphEnd, pointY);
            g.DrawString((minDpsChange * .25f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black75brush, graphStart - 50, pointY + 12, formatTick);
            #endregion

            #region Key Legend 
            Font nameFont = new Font("Calibri", 14, FontStyle.Bold);
            int nameX = (int)(graphWidth * .6f + graphStart);
            for (int index = 0; index < statsList.Length; index++)
            {
                Brush nameBrush = new SolidBrush(colors[index]);
                int nameY = (int)(graphHeight * .6f) + index * 24;
                g.DrawString(statsList[index].ToString(), nameFont, nameBrush, new PointF(nameX, nameY));
            }
            #endregion

            #region Explanatory Text
            g.DrawString(explanatoryText, tickFont, black150brush, graphWidth * .1f, graphHeight * .05f);
            #endregion
            Cursor.Current = Cursors.Default;
        }

        private float GetCalculationValue(CharacterCalculationsBase calcs, string calculation)
        {
            if (calculation == null || calculation == "Overall Rating")
                return calcs.OverallPoints;
            else
            {
                int index = 0;
                foreach (string subPoint in Calculations.SubPointNameColors.Keys)
                {
                    if (calculation.StartsWith(subPoint))
                        return calcs.SubPoints[index];
                    index++;
                }
                return 0f;
            }
        }

        public static string[] GetCalculationNames()
        {
            List<string> names = new List<string>();
            names.Add("Overall Rating");
            foreach (string subPoint in Calculations.SubPointNameColors.Keys)
				names.Add(subPoint + " Rating");
            return names.ToArray();
        }
    }
}
