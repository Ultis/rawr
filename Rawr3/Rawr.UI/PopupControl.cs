using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Rawr.UI
{
    [TemplatePart(Name = "PART_Toggle", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_PopupControl", Type = typeof(Control))]
    public class PopupControl : Control
    {
        static PopupControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopupControl), new FrameworkPropertyMetadata(typeof(PopupControl)));
        }

        private ToggleButton toggle;
        private Popup popup;
        private Control popupControl;

        public override void OnApplyTemplate()
        {
            toggle = Template.FindName("PART_Toggle", this) as ToggleButton;
            popup = Template.FindName("PART_Popup", this) as Popup;

#if !SILVERLIGHT
            MouseDown += PopupControl_MouseDown;
            LostMouseCapture += PopupControl_LostMouseCapture;

            if (popup != null)
            {
                popup.Opened += Popup_Opened;
                popup.Closed += Popup_Closed;
            }
#endif
        }

        public virtual void Close()
        {
            popup.IsOpen = false;
        }

        #region WPF Popup Handling
#if !SILVERLIGHT
        private void PopupControl_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void PopupControl_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured != this)
            {
                if (e.OriginalSource == toggle && Mouse.Captured == null)
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
                        if (popup.IsOpen && Mouse.Captured == null)
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

        private void Popup_Opened(object sender, EventArgs e)
        {
            Mouse.Capture(this, CaptureMode.SubTree);
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            if (Mouse.Captured == this)
            {
                Mouse.Capture(null);
            }
        }
#endif
        #endregion
    }
}
