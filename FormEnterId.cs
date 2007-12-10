using System;
using System.Windows.Forms;

namespace Rawr
{
    public partial class FormEnterId : Form
    {
        public int Value
        {
            get { return (int) numericUpDownValue.Value; }
        }

        public FormEnterId()
        {
            InitializeComponent();
        }

        private void FormEnterId_Load(object sender, EventArgs e)
        {
            numericUpDownValue.Focus();
            numericUpDownValue.Select();
            numericUpDownValue.Select(0, 1);
        }
    }
}