using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class StatTooltip : UserControl
    {
        private string _header = "";
        /// <summary>The Header is show at the top of the tooltip in Bold. Nothing really uses this by default as it all goes to Message</summary>
        public string Header {
            get { return _header; }
            set {
                _header = value;
                if (!String.IsNullOrEmpty(_header)) {
                    LB_Header.Text = _header;
                    LB_Header.Visibility = Visibility.Visible;
                } else {
                    LB_Header.Text = "";
                    LB_Header.Visibility = Visibility.Collapsed;
                }
            }
        }
        private string _message = "";
        /// <summary>The Message is shown in the Bulk of the Tooltip. Everything uses this by default.</summary>
        public string Message
        {
            get { return _message; }
            set {
                _message = value;
                if (!String.IsNullOrEmpty(_message)) {
                    LB_Message.Text = _message;
                    LB_Message.Visibility = Visibility.Visible;
                } else {
                    LB_Message.Text = "";
                    LB_Message.Visibility = Visibility.Collapsed;
                }
            }
        }

        public void Show(UIElement relativeTo) { Show(relativeTo, 0, 0); }
        public void Show(UIElement relativeTo, double offsetX, double offsetY)
        {
#if SILVERLIGHT
            try
            {
                GeneralTransform gt = relativeTo.TransformToVisual((UIElement)this.Parent);
                Point offset = gt.Transform(new Point(offsetX, offsetY));
                StatPopup.VerticalOffset = offset.Y;
                StatPopup.HorizontalOffset = offset.X;
                StatPopup.IsOpen = true && (!String.IsNullOrEmpty(Message) || !String.IsNullOrEmpty(Header));

                StatGrid.Measure(App.Current.RootVisual.DesiredSize);

                GeneralTransform transform = relativeTo.TransformToVisual(App.Current.RootVisual);
                // Lets make sure that we don't clip from the bottom
                double distBetweenBottomOfPopupAndBottomOfWindow =
                    App.Current.RootVisual.RenderSize.Height - offsetY -
                    transform.Transform(new Point(0, StatGrid.DesiredSize.Height)).Y;
                if (distBetweenBottomOfPopupAndBottomOfWindow < 0)
                {
                    StatPopup.VerticalOffset += distBetweenBottomOfPopupAndBottomOfWindow;
                }
                // Lets make sure that we don't clip from the right side
                double distBetweenRightSideOfPopupAndBottomOfWindow =
                    App.Current.RootVisual.RenderSize.Width - offsetX -
                    transform.Transform(new Point(StatGrid.DesiredSize.Width, 0)).X;
                if (distBetweenRightSideOfPopupAndBottomOfWindow < 0)
                {
                    StatPopup.HorizontalOffset += distBetweenRightSideOfPopupAndBottomOfWindow;
                }
            }
            catch (ArgumentException)
            {
                // Value does not fall within the expected range
                // apparently happens if you call while it's still loading the visual tree or something
            }
#else
            StatPopup.PlacementTarget = relativeTo;
            StatPopup.PlacementRectangle = new Rect(0, offsetY, offsetX, relativeTo.RenderSize.Height);
            StatPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.Right;
            StatPopup.IsOpen = true;
#endif
        }

        public void Hide()
        {
            StatPopup.IsOpen = false;
        }

        public StatTooltip()
        {
            // Required to initialize variables
            InitializeComponent();
            LB_Header.Visibility = Visibility.Collapsed;
            LB_Message.Visibility = Visibility.Collapsed;
        }
    }
}
