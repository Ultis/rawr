using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Warlock
{
    public partial class ShadowPriestControl : UserControl
    {
        public float HitPercent
        {
            get
            {
                float value = 0;
                float.TryParse(textBoxHit.Text, out value);
                return value;
            }
            set
            {
                textBoxHit.Text = value.ToString();
            }
        }

        public float MbFrequency
        {
            get
            {
                float value = 0;
                float.TryParse(textBoxMbFrequency.Text, out value);
                return value;
            }
            set
            {
                textBoxMbFrequency.Text = value.ToString();
            }
        }

        public float ShadowDps
        {
            get
            {
                float value = 0;
                float.TryParse(textBoxShadowDps.Text, out value);
                return value;
            }
            set
            {
                textBoxShadowDps.Text = value.ToString();
            }
        }

        public ShadowPriestControl()
        {
            InitializeComponent();
        }
    }
}
