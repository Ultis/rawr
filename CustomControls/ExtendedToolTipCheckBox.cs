using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.CustomControls
{
    public partial class ExtendedToolTipCheckBox : CheckBox
    {
        private ToolTip _ToolTip;
        private string _ToolTipText;

        public ExtendedToolTipCheckBox()
        {
            InitializeComponent();
            _ToolTip = new ToolTip();
            this.MouseEnter += new EventHandler(ExtendedToolTipCheckBox_MouseEnter);
            this.MouseLeave += new EventHandler(ExtendedToolTipCheckBox_MouseLeave);
        }

        public void ForceShowToolTip()
        {
            ExtendedToolTipCheckBox_MouseEnter(null, null);
        }


        public void ForceHideToolTip()
        {
            ExtendedToolTipCheckBox_MouseLeave(null, null);
        }

        public string ToolTipText
        {
            get { return _ToolTipText; }
            set { _ToolTipText = value; }
        }

        private void ExtendedToolTipCheckBox_MouseLeave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_ToolTipText))
            {
                _ToolTip.Hide(this);
            }
        }

        private void ExtendedToolTipCheckBox_MouseEnter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_ToolTipText))
            {
                int x = PointToClient(MousePosition).X + 10;
                _ToolTip.Show(_ToolTipText, this, new Point(x, -10));
            }
        }
    }
}
