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
    public partial class WarlockControl : UserControl
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

        public float CritPercent
        {
            get
            {
                float value = 0;
                float.TryParse(textBoxCrit.Text, out value);
                return value;
            }
            set
            {
                textBoxCrit.Text = value.ToString();
            }
        }

        public float SbCastTime
        {
            get
            {
                float value = 0;
                float.TryParse(textBoxSbCastTime.Text, out value);
                return value;
            }
            set
            {
                textBoxSbCastTime.Text = value.ToString();
            }
        }

        public float SbCastRatio
        {
            get
            {
                float value = 0;
                float.TryParse(textBoxSbCastRatio.Text, out value);
                return value;
            }
            set
            {
                textBoxSbCastRatio.Text = value.ToString();
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

        public WarlockControl()
        {
            InitializeComponent();
        }
    }
}
