using System;
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
    public partial class ComparisonGraphBar : UserControl
    {
        public Color Color
        {
            set { BaseRect.Fill = new SolidColorBrush(value); }
        }

        public string Title
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; (AToolTip.Content as TextBlock).Text = value; }
        }

        private TextBlock ToolTipText = null;
        private ToolTip _toolTip = null;
        public ToolTip AToolTip
        {
            get {
                if (_toolTip == null)
                {
                    _toolTip = new ToolTip();
                    ToolTipText = new TextBlock() { Text = "", };
                    _toolTip.Content = ToolTipText;
                    ToolTipService.SetToolTip(this, _toolTip);
                    ToolTipService.SetToolTip(BaseRect, _toolTip);
                    ToolTipService.SetToolTip(TextLabel, _toolTip);
                }
                return _toolTip;
            }
            set { _toolTip = value; }
        }

        public ComparisonGraphBar()
        {
            // Required to initialize variables
            InitializeComponent();
        }
    }
}
