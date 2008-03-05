using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.CustomControls
{
    public partial class ExtendedToolTipLabel : Label
    {
        private ToolTip _ToolTip;
        private string _ToolTipText;

        public ExtendedToolTipLabel()
        {
            InitializeComponent();
            _ToolTip = new ToolTip();
            this.MouseEnter += new EventHandler(ExtendedToolTipLabel_MouseEnter);
            this.MouseLeave += new EventHandler(ExtendedToolTipLabel_MouseLeave);
        }

        public string ToolTipText
        {
            get { return _ToolTipText; }
            set { _ToolTipText = value; }
        }

        void ExtendedToolTipLabel_MouseLeave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_ToolTipText))
            {
                _ToolTip.Hide(this);
            }
        }

        void ExtendedToolTipLabel_MouseEnter(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(_ToolTipText))
            {
                int x = PointToClient(MousePosition).X + 10;
                _ToolTip.Show(_ToolTipText, this, new Point(x, -10));
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
