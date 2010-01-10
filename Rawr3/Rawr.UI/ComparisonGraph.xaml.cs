using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Rawr.Optimizer;

namespace Rawr.UI
{
	public partial class ComparisonGraph : UserControl
	{

        private CharacterSlot slot;
        public CharacterSlot Slot
        {
            get { return slot; }
            set
            {
                slot = value;
                ContextMenu.Slot = slot;
            }
        }

        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                character = value;
                ContextMenu.Character = character;
                if (comparisonItems != null)
                {
                    foreach (ComparisonGraphItem cgi in comparisonItems) cgi.Character = character;
                }
            }
        }

        private ComparisonSort sort;
        public ComparisonSort Sort
        {
            get { return sort; }
            set
            {
                sort = value;
                RefreshCalcs();
            }
        }

        private DisplayMode mode;
        public DisplayMode Mode
        {
            get { return mode; }
            set
            {
                mode = value;
                if (mode == DisplayMode.Overall)
                {
                    Dictionary<string, Color> overallLegend = new Dictionary<string, Color>();
                    overallLegend["Overall"] = Colors.Purple;
                    LegendItems = overallLegend;
                }
            }
        }

        private ComparisonCalculationBase[] currentCalcs;

        private float minScale = 0;
        private float maxScale = 2;

        private Dictionary<string, Color> legendItems;
        public Dictionary<string, Color> LegendItems
        {
            get { return legendItems; }
            set
            {
				if (legendItems != value)
				{
					legendItems = value;
					if (legendItems != null)
					{
						if (comparisonItems != null)
						{
							foreach (ComparisonGraphItem item in comparisonItems) item.SetColors(legendItems.Values);
						}
						LegendStack.Children.Clear();
						foreach (KeyValuePair<string, Color> kvp in legendItems)
						{
							LegendStack.Children.Add(new ComparisonGraphBar() { Color = kvp.Value, Title = kvp.Key });
						}
					}
				}
            }
        }

        private TextBlock[] AxisLabels;
        private Rectangle[] AxisLines;
		public ComparisonGraph()
		{
			// Required to initialize variables
			InitializeComponent();

            mode = DisplayMode.Subpoints;
            sort = ComparisonSort.Overall;

            AxisLabels = new TextBlock[9];
            AxisLabels[0] = AxisLabel1; AxisLabels[1] = AxisLabel2; AxisLabels[2] = AxisLabel3;
            AxisLabels[3] = AxisLabel4; AxisLabels[4] = AxisLabel5; AxisLabels[5] = AxisLabel6;
            AxisLabels[6] = AxisLabel7; AxisLabels[7] = AxisLabel8; AxisLabels[8] = AxisLabel9;

            AxisLines = new Rectangle[9];
            AxisLines[0] = AxisLine1; AxisLines[1] = AxisLine2; AxisLines[2] = AxisLine3;
            AxisLines[3] = AxisLine4; AxisLines[4] = AxisLine5; AxisLines[5] = AxisLine6;
            AxisLines[6] = AxisLine7; AxisLines[7] = AxisLine8; AxisLines[8] = AxisLine9;

		}

        private List<ComparisonGraphItem> comparisonItems;

        public void RefreshCalcs()
        {
            DisplayCalcs(currentCalcs);
        }

        public void DisplayCalcs(ComparisonCalculationBase[] calcs)
        {
            if (calcs == null) return;
            currentCalcs = calcs;
            if (comparisonItems == null) comparisonItems = new List<ComparisonGraphItem>();
			int i = 0;
#if DEBUG
			DateTime dtA = DateTime.Now;
#endif

            minScale = 0; maxScale = 0;
            foreach (ComparisonCalculationBase c in calcs)
            {
                if (c == null) continue;
                float min = 0f, max = 0f;
                if (Mode == DisplayMode.Overall)
                {
                    if (c.OverallPoints < 0) min += c.OverallPoints;
                    else max += c.OverallPoints;
                }
                else
                {
                    foreach (float f in c.SubPoints)
                    {
                        if (f < 0) min += f;
                        else max += f;
                    }
                }
                if (min < minScale) minScale = min;
                if (max > maxScale) maxScale = max;
            }
            if (minScale > -.01f) minScale = 0f;
            if (maxScale < .01f) maxScale = 0f;
            if (maxScale == 0f && minScale == 0f) maxScale = 2f;

            float totalScale = maxScale - minScale;

            float roundTo = 2f;
            if (totalScale >= 10) roundTo = (int)Math.Pow(10, Math.Floor(Math.Log10(totalScale) - .3f));
            totalScale = roundTo * (float)Math.Ceiling(totalScale / roundTo);

            minScale = -(float)Math.Ceiling(-minScale / totalScale * 8f) * (totalScale / 8f);
            maxScale = (float)Math.Ceiling(maxScale / totalScale * 8f) * (totalScale / 8f);
            if (maxScale - minScale > totalScale)
            {
                totalScale = maxScale - minScale; roundTo = 2f;
                if (totalScale >= 10) roundTo = (int)Math.Pow(10, Math.Floor(Math.Log10(totalScale) - .3f));
                totalScale = roundTo * (float)Math.Ceiling(totalScale / roundTo);

                minScale = -(float)Math.Ceiling(-minScale / totalScale * 8f) * (totalScale / 8f);
                maxScale = (float)Math.Ceiling(maxScale / totalScale * 8f) * (totalScale / 8f);
			}
#if DEBUG
			System.Diagnostics.Debug.WriteLine("DisplayCalcs Phase A: {0}ms", DateTime.Now.Subtract(dtA).TotalMilliseconds);
			DateTime dtB = DateTime.Now;
#endif

            ChangedSize(this, null);
            IOrderedEnumerable<ComparisonCalculationBase> orderedCalcs;
            if (Sort == ComparisonSort.Alphabetical) 
                orderedCalcs = calcs.OrderBy(calc => calc == null ? "" : calc.Name);
            else if (Sort == ComparisonSort.Overall)
                orderedCalcs = calcs.OrderByDescending(calc => calc == null ? 0 : calc.OverallPoints);
            else
				orderedCalcs = calcs.OrderByDescending(calc => calc == null ? 0 : calc.SubPoints[(int)Sort]);
#if DEBUG
			System.Diagnostics.Debug.WriteLine("DisplayCalcs Phase B: {0}ms", DateTime.Now.Subtract(dtB).TotalMilliseconds);
			DateTime dtC = DateTime.Now;
#endif
			foreach (ComparisonCalculationBase c in orderedCalcs)
            {
                if (c == null) continue;
                ComparisonGraphItem item;
                if (i >= comparisonItems.Count)
                {
                    item = new ComparisonGraphItem(LegendItems.Values);
                    item.Character = Character;
                    comparisonItems.Add(item);
                    ItemStack.Children.Add(item);
                    item.NameGrid.MouseEnter += new MouseEventHandler(NameGrid_MouseEnter);
                    item.NameGrid.MouseLeave += new MouseEventHandler(NameGrid_MouseLeave);
                    item.NameGrid.MouseLeftButtonDown += new MouseButtonEventHandler(NameGrid_MouseLeftButtonDown);
                }
                else item = comparisonItems[i];

                item.Title = c.Name;
                item.Equipped = c.Equipped;
                item.OtherItem = c.Item;
                item.ItemInstance = c.ItemInstance;
                item.MinScale = minScale;
                item.MaxScale = maxScale;

                item.NameGrid.Tag = c;

                if (Mode == DisplayMode.Overall) { item[0] = c.OverallPoints; }
                else { for (int j = 0; j < c.SubPoints.Length; j++) item[j] = c.SubPoints[j]; }
                
                item.Visibility = Visibility.Visible;
                i++;
			}
#if DEBUG
			System.Diagnostics.Debug.WriteLine("DisplayCalcs Phase C: {0}ms", DateTime.Now.Subtract(dtC).TotalMilliseconds);
			DateTime dtD = DateTime.Now;
#endif
			for (; i < comparisonItems.Count; i++) comparisonItems[i].Visibility = Visibility.Collapsed;
#if DEBUG
			System.Diagnostics.Debug.WriteLine("DisplayCalcs Phase D: {0}ms", DateTime.Now.Subtract(dtD).TotalMilliseconds);
			System.Diagnostics.Debug.WriteLine("Finished DisplayCalcs: Total {0}ms", DateTime.Now.Subtract(dtA).TotalMilliseconds);
#endif
        }

        private void NameGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid senderGrid = sender as Grid;
            ComparisonCalculationBase calc = senderGrid.Tag as ComparisonCalculationBase;
            if (calc.ItemInstance != null)
            {
                MainPage.Tooltip.Hide();
                Point mousePoint = e.GetPosition(senderGrid);
                ContextMenu.SelectedItemInstance = calc.ItemInstance;
                ContextMenu.Show(senderGrid, mousePoint.X + 4, mousePoint.Y + 4);
            }
            e.Handled = true;
        }

        private void NameGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            MainPage.Tooltip.Hide();
        }

        private void NameGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            ComparisonCalculationBase calc = ((Grid)sender).Tag as ComparisonCalculationBase;
            if ((calc.Description != null || calc.ItemInstance != null || calc.Item != null) && (!ContextMenu.IsShown))
            {
                if      (calc.ItemInstance != null) MainPage.Tooltip.ItemInstance = calc.ItemInstance;
                else if (calc.Item         != null) MainPage.Tooltip.Item = calc.Item;
                else MainPage.Tooltip.CurrentString = calc.Name + "|" + calc.Description;
                if (calc is ComparisonCalculationUpgrades)
                {
                    ComparisonCalculationUpgrades upgrades = calc as ComparisonCalculationUpgrades;
                    MainPage.Tooltip.CharacterItems = upgrades.CharacterItems;
                }
                MainPage.Tooltip.Show((Grid)sender, 143, 2);
            }
        }

        private void ChangedSize(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (ItemStack.ActualWidth > 100)
            {
                int negTicks = (int)(-minScale / (maxScale - minScale) * 8);
                int posTicks = (int)(maxScale / (maxScale - minScale) * 8);

                int j = 0;
                for (float i = minScale; i <= maxScale; i += (maxScale - minScale) / 8, j++)
                {
                    if (i == 0)
                    {
                        AxisLabels[j].Foreground = new SolidColorBrush(Colors.Black);
                        AxisLines[j].Fill = new SolidColorBrush(Colors.Black);
                    }
                    else if (Math.Abs(negTicks - j) % 2 == 1)
                    {
                        AxisLabels[j].Foreground = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0));
                        AxisLines[j].Fill = new SolidColorBrush(Color.FromArgb(25, 0, 0, 0));
                    }
                    else
                    {
                        AxisLabels[j].Foreground = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
                        AxisLines[j].Fill = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
                    }
                    AxisLabels[j].Text = Math.Round(i, 2).ToString();
                }
            }
        }

        public enum DisplayMode
        {
            Subpoints,
            Overall
        }

    }

    public enum ComparisonSort
    {
        //SubPoints will be their index, such as 0 or 1
        Overall = -1,
        Alphabetical = -2
    }
}