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

namespace Rawr.UI
{
    public partial class RangeSlider : UserControl
    {
        public RangeSlider()
        {
            InitializeComponent();
            enabledBorderBrush = (LinearGradientBrush)TheLineHorizontal.BorderBrush;
            this.Loaded += new RoutedEventHandler(RangeSlider_Loaded);
            this.SizeChanged += new SizeChangedEventHandler(RangeSlider_SizeChanged);
            this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(RangeSlider_IsEnabledChanged);
            RangeSlider_OrientationChanged(null, null);

            LayoutRoot.DataContext = this;
        }

        private LinearGradientBrush enabledBorderBrush { get; set; }
        private LinearGradientBrush disabledBorderBrush = new LinearGradientBrush(new GradientStopCollection() {
                new GradientStop() { Color = Color.FromArgb(255, 236, 238, 241), Offset = 0.000 },
                new GradientStop() { Color = Color.FromArgb(255, 236, 238, 241), Offset = 0.400 },
                new GradientStop() { Color = Color.FromArgb(255, 223, 227, 230), Offset = 1.000 },
            }, 90);

        void RangeSlider_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled) {
                this.Foreground = new SolidColorBrush(Colors.Black);
                TheLineHorizontal.BorderBrush = enabledBorderBrush;
                TheLineVertical.BorderBrush = enabledBorderBrush;
                LowerSliderH.BorderBrush = enabledBorderBrush;
                LowerSliderV.BorderBrush = enabledBorderBrush;
                UpperSliderH.BorderBrush = enabledBorderBrush;
                UpperSliderV.BorderBrush = enabledBorderBrush;
            } else {
                this.Foreground = new SolidColorBrush(Colors.LightGray);
                TheLineHorizontal.BorderBrush = disabledBorderBrush;
                TheLineVertical.BorderBrush = disabledBorderBrush;
                LowerSliderH.BorderBrush = enabledBorderBrush;
                LowerSliderV.BorderBrush = enabledBorderBrush;
                UpperSliderH.BorderBrush = enabledBorderBrush;
                UpperSliderV.BorderBrush = enabledBorderBrush;
            }
        }

        void RangeSlider_OrientationChanged(object sender, EventArgs e) {
            // Hide All
            bottomTicksCanvas.Visibility = Visibility.Collapsed;
            TheLineHorizontal.Visibility = Visibility.Collapsed;
            LowerSliderH.Visibility = Visibility.Collapsed;
            UpperSliderH.Visibility = Visibility.Collapsed;
            rightTicksCanvas.Visibility = Visibility.Collapsed;
            TheLineVertical.Visibility = Visibility.Collapsed;
            LowerSliderV.Visibility = Visibility.Collapsed;
            UpperSliderV.Visibility = Visibility.Collapsed;
            // Show only what we want
            if (IsHorizontal) {
                bottomTicksCanvas.Visibility = Visibility.Visible;
                TheLineHorizontal.Visibility = Visibility.Visible;
                LowerSliderH.Visibility = Visibility.Visible;
                UpperSliderH.Visibility = Visibility.Visible;
            } else {
                rightTicksCanvas.Visibility = Visibility.Visible;
                TheLineVertical.Visibility = Visibility.Visible;
                LowerSliderV.Visibility = Visibility.Visible;
                UpperSliderV.Visibility = Visibility.Visible;
            }
            RangeSlider_SizeChanged(null, null);
        }

        // Using a DependencyProperty as the backing store for IsHorizontal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHorizontalProperty =
            DependencyProperty.Register("IsHorizontal", typeof(bool), typeof(RangeSlider), new PropertyMetadata(true));
        public bool IsHorizontal
        {
            get { return (bool)GetValue(IsHorizontalProperty); }
            set {
                // Negate the other one when you change it
                if (IsHorizontal != value) {
                    SetValue(IsHorizontalProperty, value);
                    RangeSlider_OrientationChanged(null, null);
                }
            }
        }

        #region Ticks
        // Using a DependencyProperty as the backing store for TickFreq.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickFreqProperty =
            DependencyProperty.Register("TickFrequency", typeof(double), typeof(RangeSlider), new PropertyMetadata(10d));
        public double TickFrequency
        {
            get { return (double)GetValue(TickFreqProperty); }
            set
            {
                SetValue(TickFreqProperty, value);
                RangeSlider_SizeChanged(null, null);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            bottomTicksCanvas = GetTemplateChild("BottomTicksCanvas") as Canvas;
        }

        // Using a DependencyProperty as the backing store for ShowLabels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowLabelsProperty =
            DependencyProperty.Register("ShowLabels", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
        public bool ShowLabels
        {
            get { return (bool)GetValue(ShowLabelsProperty); }
            set { SetValue(ShowLabelsProperty, value); RangeSlider_SizeChanged(null, null); }
        }
        // Using a DependencyProperty as the backing store for ShowTicks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowTicksProperty =
            DependencyProperty.Register("ShowTicks", typeof(bool), typeof(RangeSlider), new PropertyMetadata(false));
        public bool ShowTicks
        {
            get { return (bool)GetValue(ShowTicksProperty); }
            set { SetValue(ShowTicksProperty, value); RangeSlider_SizeChanged(null, null); }
        }

        void RangeSlider_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                bottomTicksCanvas.Children.Clear();
                rightTicksCanvas.Children.Clear();
                if (!ShowTicks) {
                    // don't create any ticks or labels and enforce our height to 20 px, we don't need the room for Labels
                    if (IsHorizontal) { this.MinHeight = 20; this.MinWidth = 0; } else { this.MinHeight = 0; this.MinWidth = 20; }
                    return;
                } else if (!ShowLabels) {
                    // create ticks but not labels and enforce our height to 20 px, we don't need the room for Labels
                    if (IsHorizontal) { this.MinHeight = 20; this.MinWidth = 0; } else { this.MinHeight = 0; this.MinWidth = 20; }
                } else {
                    // Create ticks AND labels and enforce our height to 30 px, we DO need the room for labels
                    if (IsHorizontal) { this.MinHeight = 30; this.MinWidth = 0; } else { this.MinHeight = 0; this.MinWidth = 30; }
                }
                int numberOfTicks = (int)((Maximum - Minimum) / TickFrequency);
                //numberOfTicks++;
                for (int i = 0; i <= numberOfTicks; i++) {
                    if (IsHorizontal) {
                        double x1 = 5 + ((i) * ((this.ActualWidth - 10) / numberOfTicks));
                        bottomTicksCanvas.Children.Add(CreateTick(new Point(x1, 0), new Point(x1, 5)));
                        if (!ShowLabels) { continue; } // jump to next if we don't want the labels
                        int value = (int)Minimum + (int)(Math.Round(((Maximum - Minimum) / numberOfTicks) * (i), 0));
                        bottomTicksCanvas.Children.Add(CreateLabel(value.ToString("0"), 6.0, x1));
                    } else {
                        double y1 = 5 + ((i) * ((this.ActualHeight - 10) / numberOfTicks));
                        rightTicksCanvas.Children.Add(CreateTick(new Point(0, y1), new Point(5, y1)));
                        if (!ShowLabels) { continue; } // jump to next if we don't want the labels
                        int value = (int)Minimum + (int)(Math.Round(((Maximum - Minimum) / numberOfTicks) * (i), 0));
                        rightTicksCanvas.Children.Add(CreateLabel(value.ToString("0"), y1, 7.0));
                    }
                }
            } catch (Exception ex) {
                new Base.ErrorBox() {
                    Title = "Error in Ranged Tick Slider Resize call",
                    Function = "RangeSlider_SizeChanged(...)",
                    TheException = ex,
                }.Show();
            }
        }
        TextBlock CreateLabel(string text, double top, double left)
        {
            TextBlock txt = new TextBlock();
            txt.Text = text;
            txt.SetValue(Canvas.TopProperty, (IsHorizontal ? top : top - txt.ActualHeight / 2));
            txt.SetValue(Canvas.LeftProperty, (IsHorizontal ? left - txt.ActualWidth / 2 : left));
            return txt;
        }
        // Using a DependencyProperty as the backing store for TickTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickTemplateProperty =
            DependencyProperty.Register("TickTemplate", typeof(DataTemplate), typeof(RangeSlider), null);
        public DataTemplate TickTemplate
        {
            get { return (DataTemplate)GetValue(TickTemplateProperty); }
            set { SetValue(TickTemplateProperty, value); }
        }
        FrameworkElement CreateTick(Point start, Point end)
        {
            if (TickTemplate == null) {
                Line ln = new Line();
                ln.SetBinding(Line.StrokeProperty, new System.Windows.Data.Binding("Foreground"));
                ln.StrokeThickness = 1.0;
                ln.X1 = start.X;
                ln.Y1 = start.Y;
                ln.X2 = end.X;
                ln.Y2 = end.Y;
                return ln;
            } else {
                ContentPresenter cp = new ContentPresenter();
                cp.Content = "a";
                cp.ContentTemplate = TickTemplate;
                cp.SetValue(Canvas.TopProperty, (IsHorizontal ? start.Y : start.Y - cp.ActualHeight / 2 ));
                cp.SetValue(Canvas.LeftProperty, (IsHorizontal ? start.X - cp.ActualWidth / 2 : start.X));
                return cp;
            }
            //return null;
        }
        #endregion

        #region Values
        void RangeSlider_Loaded(object sender, RoutedEventArgs e)
        {
            LowerSliderH.ValueChanged += new RoutedPropertyChangedEventHandler<double>(LowerSlider_ValueChanged);
            UpperSliderH.ValueChanged += new RoutedPropertyChangedEventHandler<double>(UpperSlider_ValueChanged);
            LowerSliderV.ValueChanged += new RoutedPropertyChangedEventHandler<double>(LowerSlider_ValueChanged);
            UpperSliderV.ValueChanged += new RoutedPropertyChangedEventHandler<double>(UpperSlider_ValueChanged);
        }

        void UpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LowerSliderH.Value = Math.Min(UpperSliderH.Value, LowerSliderH.Value);
            LowerSliderV.Value = Math.Min(UpperSliderV.Value, LowerSliderV.Value);
        }

        void LowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpperSliderH.Value = Math.Max(UpperSliderH.Value, LowerSliderH.Value);
            UpperSliderV.Value = Math.Max(UpperSliderV.Value, LowerSliderV.Value);
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(RangeSlider), new PropertyMetadata(0d));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(RangeSlider), new PropertyMetadata(100d));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LowerValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LowerValueProperty =
            DependencyProperty.Register("LowerValue", typeof(double), typeof(RangeSlider), new PropertyMetadata(0d));
        public double LowerValue
        {
            get { return (double)GetValue(LowerValueProperty); }
            set { SetValue(LowerValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UpperValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpperValueProperty =
            DependencyProperty.Register("UpperValue", typeof(double), typeof(RangeSlider), new PropertyMetadata(70d));
        public double UpperValue
        {
            get { return (double)GetValue(UpperValueProperty); }
            set { SetValue(UpperValueProperty, value); }
        }
        #endregion
    }
}
