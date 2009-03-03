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

            txtFightLength.Tag = new NumericField("FightLength", 1f, 20f, false);
            txtESInterval.Tag = new NumericField("ESInterval", 40f, 1000f, true);
            cboManaPotAmount.Tag = new NumericField("ManaPotAmount", 0f, 3000f, true);
            tbBurst.Tag = new NumericField("BurstPercentage", 0f, 100f, true);
            tbOverhealing.Tag = new NumericField("OverhealingPercentage", 0f, 100f, true);
        }

        protected override void LoadCalculationOptions()
        {
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsRestoSham();
            CalculationOptionsRestoSham options = Character.CalculationOptions as CalculationOptionsRestoSham;

            _bLoading = true;

            #region General tab page:
            txtFightLength.Text = options.FightLength.ToString();
            cboManaPotAmount.Text = options.ManaPotAmount.ToString();
            chkManaTide.Checked = options.ManaTideEveryCD;
            chkWaterShield.Checked = options.WaterShield;
            chkMT.Checked = options.TankHeal;
            txtESInterval.Text = options.ESInterval.ToString();
            cboHealingStyle.Text = options.HealingStyle.ToString();
            // The track bars
            tbBurst.Value = (Int32)options.BurstPercentage;
            UpdateTrackBarLabel(tbBurst);
            tbOverhealing.Value = (Int32)options.OverhealingPercentage;
            UpdateTrackBarLabel(tbOverhealing);
            #endregion
            #region Glyphs Page:
            chkELWGlyph.Checked = options.ELWGlyph;
            chkGlyphCH.Checked = options.GlyphCH;
            chkWaterShield2.Checked = options.WaterShield2;
            chkWaterShield3.Checked = options.WaterShield3;
            chkLHW.Checked = options.LHWPlus;
            chkManaTide2.Checked = options.ManaTidePlus;
            #endregion
            #region Relics Page:
            // Check for what totem is equipped and check the appropriate option on the totems page
            if (Character != null && Character.Ranged != null)
            {
                foreach (Control groupControl in this.RelicsPage.Controls)
                {
                    if (groupControl is GroupBox)
                    {
                        foreach (Control c in groupControl.Controls)
                        {
                            if (c is CheckBox && c.Tag != null)
                            {
                                if (Character.Ranged.Item.Name == ((CheckBox)c).Text)
                                {
                                    ((CheckBox)c).Checked = true;
                                    this[c.Tag.ToString()] = true;
                                }
                                else
                                {
                                    ((CheckBox)c).Checked = false;
                                    this[c.Tag.ToString()] = false;
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            _bLoading = false;
        }
        #region Text box handling
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
        #endregion

        #region Validation on Boxes and Text Handling
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
        #endregion

        #region CheckBox Handling
        private void chkManaTide_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["ManaTideEveryCD"] = chkManaTide.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkManaTide2_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["ManaTidePlus"] = chkManaTide2.Checked;
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

        private void chkWaterShield2_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["WaterShield2"] = chkWaterShield2.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkWaterShield3_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["WaterShield3"] = chkWaterShield3.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkLHW_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["LHWPlus"] = chkLHW.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkMT_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["TankHeal"] = chkMT.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkELWGlyph_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["ELWGlyph"] = chkELWGlyph.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGlyphCH_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["GlyphCH"] = chkGlyphCH.Checked;
                Character.OnCalculationsInvalidated();
            }
        }
        #endregion

        #region Trackbar Handling
        private void OnTrackBarScroll(object sender, EventArgs e)
        {
            TrackBar trackBar = sender as TrackBar;
            if (trackBar.Tag == null)
                return;

            // For now, only update the labels of track bars we think are percentages.
            if (trackBar.Minimum == 0 && trackBar.Maximum == 100)
                UpdateTrackBarLabel(trackBar);

            NumericField f = trackBar.Tag as NumericField;
            if (trackBar.Value == 0 && !f.CanBeZero)
            {
                errorRestoSham.SetError(sender as Control, "Value cannot be zero.");
                return;
            }

            if (trackBar.Value > f.MaxValue || trackBar.Value < f.MinValue)
            {
                string err = string.Format("Value must be between {0} and {1}.", f.MinValue, f.MaxValue);
                errorRestoSham.SetError(sender as Control, err);
                return;
            }

            this[f.PropertyName] = trackBar.Value;
            Character.OnCalculationsInvalidated();
        }

        private void UpdateTrackBarLabel(TrackBar trackBar)
        {
            Control[] sr = trackBar.Parent.Controls.Find(string.Format("{0}_Label", trackBar.Name), true);
            if (sr == null || sr.Length != 1)
                return;

            Label l = sr[0] as Label;
            if (l == null)
                return;

            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(
                "\\(\\d*%\\)",
                System.Text.RegularExpressions.RegexOptions.CultureInvariant
                | System.Text.RegularExpressions.RegexOptions.Compiled
            );

            l.Text = re.Replace(l.Text, string.Format("({0}%)", trackBar.Value));
        }
        #endregion

        #region Relic CheckBox Handling
        private void Relic_CheckedChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                if (sender is CheckBox)
                {
                    CheckBox cb = sender as CheckBox;
                    if (cb.Tag == null)
                        return;

                    string key = cb.Tag.ToString();
                    this[key] = cb.Checked;
                    
                    // Uncheck all other options
                    _bLoading = true;
                    foreach (Control groupControl in this.RelicsPage.Controls)
                    {
                        if (groupControl is GroupBox)
                        {
                            foreach (Control c in groupControl.Controls)
                            {
                                if (c is CheckBox)
                                {
                                    CheckBox cb2 = c as CheckBox;
                                    if (cb2.Checked && cb2.Tag != null && cb2.Name != cb.Name)
                                    {
                                        this[c.Tag.ToString()] = false;
                                        cb2.Checked = false;
                                    }
                                }
                            }
                        }
                    }
                    _bLoading = false;

                    Character.OnCalculationsInvalidated();
                }
            }
        }
        #endregion

        #region Combo Box Handling
        private void cboHealingStyle_TextChanged(object sender, EventArgs e)
        {
            if (!_bLoading)
            {
                this["HealingStyle"] = cboHealingStyle.Text;
                Character.OnCalculationsInvalidated();
            }
        }
        #endregion
    }

    #region Calculations, do not edit.
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
    #endregion

    [Serializable]
    public class CalculationOptionsRestoSham : ICalculationOptionBase
    {
        #region GetXml strings
        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                      new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRestoSham));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
        #endregion
        #region Defaults for variables
        #region Page One Defaults
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
        /// Whether we keep Water Shield up or not (could use Earth Shield during some fights).
        /// </summary>
        public bool TankHeal = true;

        /// <summary>
        /// Interval of time between Earth Shield casts, in seconds.  Minimum in Calculations of 32.
        /// </summary>
        public float ESInterval = 60f;

        /// <summary>
        /// Your style of healing.
        /// </summary>
        public string HealingStyle = "LHW Spam";

        /// <summary>
        /// The percentage of healing that is intended to be burst.
        /// </summary>
        public float BurstPercentage = 85f;

        /// <summary>
        /// The percentage of healing that is overhealing.
        /// </summary>
        public float OverhealingPercentage = 35f;
        #endregion
        #region Page Two Defaults
        /// <summary>
        /// Whether a Mana Tide totem is placed every time the cooldown is up.
        /// </summary>
        public bool ManaTidePlus = true;

        /// <summary>
        /// Whether we keep Water Shield up or not (could use Earth Shield during some fights).
        /// </summary>
        public bool WaterShield2 = true;

        /// <summary>
        /// Whether we keep Water Shield up or not (could use Earth Shield during some fights).
        /// </summary>
        public bool WaterShield3 = true;

        /// <summary>
        /// Whether we keep Water Shield up or not (could use Earth Shield during some fights).
        /// </summary>
        public bool GlyphCH = false;

        /// <summary>
        /// Whether we keep Water Shield up or not (could use Earth Shield during some fights).
        /// </summary>
        public bool ELWGlyph = false;

        /// <summary>
        /// Whether we keep Water Shield up or not (could use Earth Shield during some fights).
        /// </summary>
        public bool LHWPlus = true;
        #endregion
        #region Page Three Defaults
        /// <summary>
        /// Is the first healing wave totem in use.
        /// </summary>
        public bool TotemHW1 = false;

        /// <summary>
        /// Is the second healing wave totem in use.
        /// </summary>
        public bool TotemHW2 = false;

        /// <summary>
        /// Is the third healing wave totem in use.
        /// </summary>
        public bool TotemHW3 = false;

        /// <summary>
        /// Is the first chain heal totem in use.
        /// </summary>
        public bool TotemCH1 = false;

        /// <summary>
        /// Is the second chain heal totem in use.
        /// </summary>
        public bool TotemCH2 = false;

        /// <summary>
        /// Is the third chain heal totem in use.
        /// </summary>
        public bool TotemCH3 = false;

        /// <summary>
        /// Is the fourth chain heal totem in use.
        /// </summary>
        public bool TotemCH4 = false;

        /// <summary>
        /// Is the first lesser healing wave totem in use.
        /// </summary>
        public bool TotemLHW1 = false;

        /// <summary>
        /// Is the first water shield totem in use.
        /// </summary>
        public bool TotemWS1 = false;
        #endregion
        #endregion

        }

}
