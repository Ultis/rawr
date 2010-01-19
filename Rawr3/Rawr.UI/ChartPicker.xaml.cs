using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Rawr.UI
{
    public partial class ChartPicker : UserControl
    {
        public GraphDisplay GraphDisplay { get; set; }

        public List<ChartPickerItem> Items { get; set; }

        public static readonly DependencyProperty PrimaryItemProperty =
            DependencyProperty.Register("PrimaryItem", typeof(ChartPickerItem), typeof(ChartPicker), new PropertyMetadata(null));
        public ChartPickerItem PrimaryItem
        {
            get { return (ChartPickerItem)GetValue(PrimaryItemProperty); }
            set { SetValue(PrimaryItemProperty, value); }
        }


        public ChartPicker()
        {
            InitializeComponent();
            this.Items = new List<ChartPickerItem>(new ChartPickerItem[]
            {
                new ChartPickerItem("Gear", "Head", "Neck", "Shoulders", "Back", "Chest", "Wrist", "Hands", "Waist", "Legs", "Feet", "Finger 1", "Finger 2", "Trinket 1", "Trinket 2", "Main Hand", "Off Hand", "Ranged", "Projectile", "Projectile Bag"),
                new ChartPickerItem("Enchants", "Head", "Shoulders", "Back", "Chest", "Wrist", "Hands", "Legs", "Feet", "Finger 1", "Finger 2", "Main Hand", "Off Hand", "Ranged"),
                new ChartPickerItem("Gems", "All Normal", "Red", "Blue", "Yellow", "Meta"),
                new ChartPickerItem("Buffs", "All", "Food", "Flasks and Elixirs", "Scrolls", "Potions", "Raid Buffs", "Set Bonuses"),
                new ChartPickerItem("Talents and Glyphs", "Individual Talents", "Talent Specs", "Glyphs"),
                new ChartPickerItem("Equipped", "Gear", "Enchants", "Buff"),
                new ChartPickerItem("Available", "Gear", "Enchants"),
                new ChartPickerItem("Direct Upgrades", "Gear", "Enchants"),
                new ChartPickerItem("Stat Values", "Relative Stat Values"),
                //new ChartPickerItem("{0} Specific")
            });
            Calculations.ModelChanged += new EventHandler(Calculations_ModelChanged);

            this.DataContext = this;

			_timer = new DispatcherTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(150);
			_timer.Tick += new EventHandler(_timer_Tick);
        }

        void Calculations_ModelChanged(object sender, EventArgs e)
        {
            bool specificWasSelected = false;
            ChartPickerItem last = Items.Last();
            if (last.Header.EndsWith(" Specific"))
            {
                specificWasSelected = PrimaryItem == last;
                Items.Remove(last);
            }
            last = new ChartPickerItem(Calculations.Instance.Name + " Specific", Calculations.CustomChartNames);
            Items.Add(last);
            if (specificWasSelected)
            {
                PrimaryItem = last;
                SetCurrentGraph();
            }
        }

        private void Grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ChartPickerItem mousedOverItem = ((FrameworkElement)sender).DataContext as ChartPickerItem;
            _timer.Stop();
			_lastMousedOverItem = mousedOverItem;
			_timer.Start();
		}

		private void Grid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			ChartPickerItem mousedOverItem = ((FrameworkElement)sender).DataContext as ChartPickerItem;
			if (_lastMousedOverItem == mousedOverItem)
			{
				_lastMousedOverItem = null;
				_timer.Stop();
			}
		}

		private DispatcherTimer _timer;
		private ChartPickerItem _lastMousedOverItem;
		private void _timer_Tick(object sender, EventArgs e)
		{
			_timer.Stop();
			if (_lastMousedOverItem == null) return;

			PrimaryItem = _lastMousedOverItem;
			ListBoxSecondary.SelectedItem = PrimaryItem.SelectedItem;
		}

        private void Grid_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ChartPickerItem clickedItem = ((FrameworkElement)sender).DataContext as ChartPickerItem;
            if (ListBoxSecondary.Items.Contains(clickedItem))
                PrimaryItem.SelectedItem = clickedItem;
            SetCurrentGraph();
            Close();
        }

        private void Background_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void SetCurrentGraph()
        {
            TextBlockChartButtonPrimary.Text = PrimaryItem.Header;
            TextBlockChartButtonSecondary.Text = PrimaryItem.SelectedItem.Header;
            GraphDisplay.CurrentGraph = string.Format("{0}|{1}", PrimaryItem, PrimaryItem.SelectedItem);
        }

        private void Close()
        {
            PrimaryItem = Items.FirstOrDefault(chartPickerItem => chartPickerItem.Header == GraphDisplay.CurrentGraph.Split('|')[0]);
            PopupChartPicker.IsOpen = false;
        }

        #region WPF Popup Handling
#if !SILVERLIGHT
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured == this && e.OriginalSource == this)
            {
                Close();
            }
        }

        private bool IsDescendant(DependencyObject reference, DependencyObject node)
        {
            DependencyObject o = node;
            while (o != null)
            {
                if (o == reference)
                {
                    return true;
                }
                Popup popup = o as Popup;
                if (popup != null)
                {
                    o = popup.Parent;
                    if (o == null)
                    {
                        o = popup.PlacementTarget;
                    }
                }
                else
                {
                    //handle content elements separately
                    ContentElement contentElement = o as ContentElement;
                    if (contentElement != null)
                    {
                        DependencyObject parent = ContentOperations.GetParent(contentElement);
                        if (parent != null)
                        {
                            o = parent;
                        }
                        FrameworkContentElement fce = contentElement as FrameworkContentElement;
                        if (fce != null)
                        {
                            o = fce.Parent;
                        }
                        else
                        {
                            o = null;
                        }
                    }
                    else
                    {
                        //also try searching for parent in framework elements (such as DockPanel, etc)
                        FrameworkElement frameworkElement = o as FrameworkElement;
                        if (frameworkElement != null)
                        {
                            DependencyObject parent = frameworkElement.Parent;
                            if (parent != null)
                            {
                                o = parent;
                                continue;
                            }
                        }

                        //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
                        o = VisualTreeHelper.GetParent(o);
                    }
                }
            }
            return false;
        }

        private void UserControl_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured != this)
            {
                if (e.OriginalSource == Toggle && Mouse.Captured == null)
                {
                    // reclicking on button, if you close the click will cause toggle => open
                    return;
                }
                if (e.OriginalSource == this)
                {
                    // If capture is null or it's not below the combobox, close. 
                    if (Mouse.Captured == null || !IsDescendant(this, Mouse.Captured as DependencyObject))
                    {
                        Close();
                    }
                }
                else
                {
                    if (IsDescendant(this, e.OriginalSource as DependencyObject))
                    {
                        // Take capture if one of our children gave up capture
                        if (PopupChartPicker.IsOpen && Mouse.Captured == null)
                        {
                            Mouse.Capture(this, CaptureMode.SubTree);
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }

        private void PopupChartPicker_Opened(object sender, EventArgs e)
        {
            Mouse.Capture(this, CaptureMode.SubTree);
        }

        private void PopupChartPicker_Closed(object sender, EventArgs e)
        {
            if (Mouse.Captured == this)
            {
                Mouse.Capture(null);
            }
        }
#endif
        #endregion

        public class ChartPickerItem : DependencyObject
        {
            public static readonly DependencyProperty ItemsProperty =
                DependencyProperty.Register("Items", typeof(ChartPickerItem[]), typeof(ChartPickerItem), new PropertyMetadata(null));
            public ChartPickerItem[] Items
            {
                get { return (ChartPickerItem[])GetValue(ItemsProperty); }
                set { SetValue(ItemsProperty, value); }
            }

            public static readonly DependencyProperty HeaderProperty =
                DependencyProperty.Register("Header", typeof(string), typeof(ChartPickerItem), new PropertyMetadata(null));
            public string Header
            {
                get { return (string)GetValue(HeaderProperty); }
                set { SetValue(HeaderProperty, value); }
            }

            public static readonly DependencyProperty SelectedItemProperty =
                DependencyProperty.Register("SelectedItem", typeof(ChartPickerItem), typeof(ChartPickerItem), new PropertyMetadata(null));
            public ChartPickerItem SelectedItem
            {
                get { return (ChartPickerItem)GetValue(SelectedItemProperty); }
                set { SetValue(SelectedItemProperty, value); }
            }

            public ChartPickerItem() { }
            public ChartPickerItem(string header, params string[] children)
            {
                Header = header;
                Items = children.Select(child => new ChartPickerItem(child)).ToArray();
                SelectedItem = Items.FirstOrDefault();
            }

            public override string ToString()
            {
                return Header;
            }
        }
    }
}
