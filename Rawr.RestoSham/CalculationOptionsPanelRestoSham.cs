using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml.Serialization;

namespace Rawr.RestoSham
{


    public partial class CalculationOptionsPanelRestoSham : CalculationOptionsPanelBase
    {
        private bool _bLoading = false;


        public CalculationOptionsPanelRestoSham()
        {
            InitializeComponent();

            txtFightLength.Tag = new NumericField("FightLength", 1f, 60f, false);
            txtOutsideFSR.Tag = new NumericField("OutsideFSRPct", 0f, 75f, true);
            txtESInterval.Tag = new NumericField("ESInterval", 40f, 1000f, true);
            cboManaPotAmount.Tag = new NumericField("ManaPotAmount", 0f, 3000f, true);

        }


        protected override void LoadCalculationOptions()
        {
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsRestoSham();
            CalculationOptionsRestoSham options = Character.CalculationOptions as CalculationOptionsRestoSham;

            _bLoading = true;

            // Init General tab page:

            txtFightLength.Text = options.FightLength.ToString();
            txtOutsideFSR.Text = Math.Round(100 * options.OutsideFSRPct, 0).ToString();
            cboManaPotAmount.Text = options.ManaPotAmount.ToString();
            chkManaTide.Checked = options.ManaTideEveryCD;
            chkWaterShield.Checked = options.WaterShield;
            txtESInterval.Text = options.ESInterval.ToString();

            _bLoading = false;
        }


        private bool _bInSetRankBoxes = false;
        private void SetRankBoxes(ComboBox cboMax, ComboBox cboMin, int? maxRank, int? minRank, int maxSpellRank)
        {
            if (_bInSetRankBoxes)
                return;
            _bInSetRankBoxes = true;

            // Bounds checking:

            int iMax = maxRank ?? maxSpellRank;
            int iMin = minRank ?? 1;

            if (iMax > maxSpellRank)
                iMax = maxSpellRank;
            if (iMax < 2)
                iMax = 2;
            if (iMin >= iMax)
                iMin = iMax - 1;

            // Set textbox values and adjust choices:

            if (maxRank.HasValue)
            {
                cboMax.Items.Clear();
                for (int i = maxSpellRank; i > iMin; i--)
                    cboMax.Items.Add(i);
                cboMax.SelectedItem = iMax;
            }
            if (minRank.HasValue)
            {
                cboMin.Items.Clear();
                for (int i = iMax - 1; i >= 1; i--)
                    cboMin.Items.Add(i);
                cboMin.SelectedItem = iMin;
            }

            _bInSetRankBoxes = false;
        }


        private void numericTextBox_Validated(object sender, EventArgs e)
        {
            if (_bLoading || Character == null)
                return;

            Control txtBox = sender as Control;
            if (txtBox.Tag == null)
                return;
            NumericField info = txtBox.Tag as NumericField;

            this[info.PropertyName] = float.Parse(txtBox.Text);
            Character.OnCalculationsInvalidated();
        }


        private void numericTextBox_Validating(object sender, CancelEventArgs e)
        {
            Control txtBox = sender as Control;
            if (txtBox.Tag == null)
                return;
            NumericField info = txtBox.Tag as NumericField;

            float f;
            string szError = string.Empty;
            if (!float.TryParse(txtBox.Text, out f))
                szError = "Please enter a numeric value";
            else
            {
                if (f < info.MinValue || f > info.MaxValue)
                    if (f == 0f && !info.CanBeZero)
                    {
                        if (info.MinValue == float.MinValue)
                        {
                            if (info.MaxValue == float.MaxValue)
                                szError = "Please enter a numeric value";
                            else
                                szError = "Please enter a number less than " + info.MaxValue.ToString();
                        }
                        else
                        {
                            if (info.MaxValue == float.MaxValue)
                                szError = "Please enter a number larger than " + info.MinValue.ToString();
                            else
                                szError = "Please enter a number between " + info.MinValue.ToString() + " and " +
                                          info.MaxValue.ToString();
                        }
                    }
            }

            if (!string.IsNullOrEmpty(szError))
            {
                e.Cancel = true;
                errorRestoSham.SetError(sender as Control, szError);
            }
        }


        public object this[string szFieldName]
        {
            get
            {
                CalculationOptionsRestoSham options = Character.CalculationOptions as CalculationOptionsRestoSham;
                Type t = options.GetType();
                FieldInfo field = t.GetField(szFieldName);
                if (field != null)
                    return field.GetValue(options);

                return null;
            }
            set
            {
                CalculationOptionsRestoSham options = Character.CalculationOptions as CalculationOptionsRestoSham;
                Type t = options.GetType();
                FieldInfo field = t.GetField(szFieldName);
                if (field != null)
                    field.SetValue(options, value);
            }
        }


        private void chkManaTide_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["ManaTideEveryCD"] = chkManaTide.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkWaterShield_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["WaterShield"] = chkWaterShield.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

    }


    class NumericField
    {
        public NumericField(string szName, float min, float max, bool bzero)
        {
            PropertyName = szName;
            MinValue = min;
            MaxValue = max;
            CanBeZero = bzero;
        }

        public string PropertyName = string.Empty;
        public float MinValue = float.MinValue;
        public float MaxValue = float.MaxValue;
        public bool CanBeZero = false;
    }


    [Serializable]
    public class CalculationOptionsRestoSham : ICalculationOptionBase
    {
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                      new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRestoSham));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }


        /// <summary>
        /// Fight length, in minutes.
        /// </summary>
        public float FightLength = 5.0f;

        /// <summary>
        /// Percentage of time during the fight spent outside the FSR.
        /// </summary>
        public float OutsideFSRPct = 0.0f;

        /// <summary>
        /// Average amount restored by a mana potion.
        /// </summary>
        public float ManaPotAmount = 2400f;

        /// <summary>
        /// Whether a Mana Tide totem is placed every time the cooldown is up.
        /// </summary>
        public bool ManaTideEveryCD = true;

        /// <summary>
        /// Whether we keep Water Shield up or not (could use Earth Shield during some fights).
        /// </summary>
        public bool WaterShield = true;

        /// <summary>
        /// Interval of time between Earth Shield casts, in seconds.  Minimum in Calculations of 32.
        /// </summary>
        public float ESInterval = 60f;

    }

}
