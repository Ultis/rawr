using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Rawr.Silverlight
{
	public partial class ComparisonGraphItem : UserControl
	{

        public string Title
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }

        public bool Equipped
        {
            get { return EquippedRect.Visibility == Visibility.Visible; }
            set { EquippedRect.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public float MinScale { get; set; }
        public float MaxScale { get; set; }

        private List<ComparisonGraphBar> rects;
        private List<float> values;
        public float this[int i]
        {
            get { return values[i]; }
            set
            {
                values[i] = value;
                rects[i].Title = Math.Round(value, 2).ToString();
                TotalLabel.Text = Math.Round(values.Sum(), 2).ToString();
                ChangedSize(this, null);
            }
        }

        public void SetColors(IEnumerable<Color> colors)
        {
            rects = new List<ComparisonGraphBar>();
            values = new List<float>(rects.Count);

            foreach (Color c in colors)
            {
                ComparisonGraphBar r = new ComparisonGraphBar() { Color = c };
                r.Height = 30;
                r.Margin = new Thickness(0, 4, 0, 4);
                rects.Add(r);
                values.Add(0f);
            }
        }

        public ComparisonGraphItem(IEnumerable<Color> colors) : this() { SetColors(colors); }
		public ComparisonGraphItem()
		{
			// Required to initialize variables
			InitializeComponent();
		}

		private void ChangedSize(object sender, System.Windows.SizeChangedEventArgs e)
		{
            if (ActualWidth > 150)
            {
                PositiveStack.Children.Clear();
                NegativeStack.Children.Clear();

                int minTick = (int)(-MinScale / (MaxScale - MinScale) * 8);
                int maxTick = (int)(MaxScale / (MaxScale - MinScale) * 8);

                if (minTick == 0) NegativeStack.Visibility = Visibility.Collapsed;
                else
                {
                    Grid.SetColumnSpan(NegativeStack, minTick);
                    NegativeStack.Visibility = Visibility.Visible;
                }
                if (maxTick == 0) PositiveStack.Visibility = Visibility.Collapsed;
                else
                {
                    Grid.SetColumn(PositiveStack, minTick + 1);
                    Grid.SetColumnSpan(PositiveStack, maxTick);
                    PositiveStack.Visibility = Visibility.Visible;
                }
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i] > 0)
                    {
                        rects[i].Width = (ActualWidth - 151) * (values[i] / (MaxScale - MinScale));
                        PositiveStack.Children.Add(rects[i]);
                    }
                    else
                    {
                        rects[i].Width = (ActualWidth - 151) * (-values[i] / (MaxScale - MinScale));
                        NegativeStack.Children.Add(rects[i]);
                    }
                }
                PositiveStack.Children.Add(TotalLabel);
            }
		}
	}
}