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
    public class App : Application
    {
#if !SILVERLIGHT
        public new static App Current
        {
            get
            {
                return Application.Current as App;
            }
        }

        public Grid RootVisual
        {
            get
            {
                return (Grid)MainWindow.Content;
            }
            set
            {
                MainWindow.Content = value;
            }
        }

        public bool IsRunningOutOfBrowser
        {
            get
            {
                return true;
            }
        }

        public static object GetFocusedElement()
        {
            return FocusManager.GetFocusedElement(Application.Current.MainWindow);
        }
#else
        public static object GetFocusedElement()
        {
            return FocusManager.GetFocusedElement();
        }
#endif
    }
}
