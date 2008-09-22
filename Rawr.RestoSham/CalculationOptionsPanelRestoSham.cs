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
    public enum Faction
      {
        None,
        Aldor,
        Scryers
      }
    
    
    public partial class CalculationOptionsPanelRestoSham : CalculationOptionsPanelBase
      {
        private bool _bLoading = false;
        
        
        public CalculationOptionsPanelRestoSham()
          {
            InitializeComponent();
            
            txtFightLength.Tag = new NumericField("FightLength", 1f, 60f, false);
            txtOutsideFSR.Tag = new NumericField("OutsideFSRPct", 0f, 75f, true);
            txtSPriestMp5.Tag = new NumericField("SPriestMP5", 0f, 500f, true);
            txtManaPotInterval.Tag = new NumericField("ManaPotTime", 2f, float.MaxValue, true);
            txtHWRatio.Tag = new NumericField("HWRatio", 0f, 100f, true);
            txtLHWRatio.Tag = new NumericField("LHWRatio", 0f, 100f , true);
            txtCHRatio.Tag = new NumericField("CHRatio", 0f, 100f, true);
            txtESInterval.Tag = new NumericField("ESInterval", 40f, 1000f, true);
            cboManaPotAmount.Tag = new NumericField("ManaPotAmount", 0f, 3000f, true);
            
            cboNumCHTargets.Items.Add(1);
            cboNumCHTargets.Items.Add(2);
            cboNumCHTargets.Items.Add(3);

            cboESRank.Items.Add(1);
            cboESRank.Items.Add(2);
            cboESRank.Items.Add(3);

            cboHWTotem.Items.Add(new TotemRelic());
            cboLHWTotem.Items.Add(new TotemRelic());
            cboCHTotem.Items.Add(new TotemRelic());
            cboHWTotem.Tag = HealSpells.HealingWave;
            cboLHWTotem.Tag = HealSpells.LesserHealingWave;
            cboCHTotem.Tag = HealSpells.ChainHeal;

            foreach (TotemRelic totem in TotemRelic.TotemList)
              {
                if (totem.AppliesTo == HealSpells.HealingWave)
                  cboHWTotem.Items.Add(totem);
                else if (totem.AppliesTo == HealSpells.LesserHealingWave)
                  cboLHWTotem.Items.Add(totem);
                else
                  cboCHTotem.Items.Add(totem);
              }
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
            txtSPriestMp5.Text = options.SPriestMP5.ToString();
            cboManaPotAmount.Text = options.ManaPotAmount.ToString();
            txtManaPotInterval.Text = options.ManaPotTime.ToString();
            chkManaTide.Checked = options.ManaTideEveryCD;
            chkWaterShield.Checked = options.WaterShield;
            radioAldor.Checked = (options.ExaltedFaction == Faction.Aldor);
            radioScryers.Checked = (options.ExaltedFaction == Faction.Scryers);
            chkExalted.Checked = (options.ExaltedFaction != Faction.None);

            // Init Spells tab page:
            
            txtHWRatio.Text = options.HWRatio.ToString();
            txtLHWRatio.Text = options.LHWRatio.ToString();
            txtCHRatio.Text = options.CHRatio.ToString();
            txtESInterval.Text = options.ESInterval.ToString();
            cboESRank.SelectedIndex = options.ESRank - 1;
            cboNumCHTargets.SelectedIndex = options.NumCHTargets - 1;
            
            trkHW.Value = 100 - options.HWDownrank.Ratio;
            trkCH.Value = 100 - options.CHDownrank.Ratio;
            lblHWMaxPct.Text = String.Format("{0}%", 100 - trkHW.Value);
            lblHWMinPct.Text = String.Format("{0}%", trkHW.Value);
            lblCHMaxPct.Text = String.Format("{0}%", 100 - trkCH.Value);
            lblCHMinPct.Text = String.Format("{0}%", trkCH.Value);
            SetRankBoxes(cboHWMaxRank, cboHWMinRank, options.HWDownrank.MaxRank, options.HWDownrank.MinRank, 12);
            SetRankBoxes(cboCHMaxRank, cboCHMinRank, options.CHDownrank.MaxRank, options.CHDownrank.MinRank, 5);

            // Init Totems tab page:

            cboHWTotem.SelectedIndex = FindIndex(cboHWTotem, options.Totems[HealSpells.HealingWave]);
            cboLHWTotem.SelectedIndex = FindIndex(cboLHWTotem, options.Totems[HealSpells.LesserHealingWave]);
            cboCHTotem.SelectedIndex = FindIndex(cboCHTotem, options.Totems[HealSpells.ChainHeal]);
            chkEquipTotems.Checked = options.EquipTotemsDuringFight;
            
            _bLoading = false;
          }

        private int FindIndex(ComboBox cbox, TotemRelic totem)
          {
            foreach (TotemRelic t in cbox.Items)
              if (t.Equals(totem))
                return cbox.Items.IndexOf(t);
                
            return -1;
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
          
          
        private void cboLHWTotem_SelectedIndexChanged(object sender, EventArgs e)
          {
            TotemRelic totem = cboLHWTotem.SelectedItem as TotemRelic;
            lblLHWTotemDesc.Text = totem.Description;
            
            if (!_bLoading)
              {
                if (totem.ID != 0 && !chkEquipTotems.Checked)
                  {
                    cboHWTotem.SelectedIndex = 0;
                    cboCHTotem.SelectedIndex = 0;
                  }
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).Totems[HealSpells.LesserHealingWave] = totem;
                Character.OnCalculationsInvalidated();
              }
          }

        private void cboHWTotem_SelectedIndexChanged(object sender, EventArgs e)
          {
            TotemRelic totem = cboHWTotem.SelectedItem as TotemRelic;
            lblHWTotemDesc.Text = totem.Description;

            if (!_bLoading)
              {
                if (totem.ID != 0 && !chkEquipTotems.Checked)
                  {
                    cboLHWTotem.SelectedIndex = 0;
                    cboCHTotem.SelectedIndex = 0;
                  }
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).Totems[HealSpells.HealingWave] = totem;
                Character.OnCalculationsInvalidated();
              }
          }

        private void cboCHTotem_SelectedIndexChanged(object sender, EventArgs e)
          {
            TotemRelic totem = cboCHTotem.SelectedItem as TotemRelic;
            lblCHTotemDesc.Text = totem.Description;

            if (!_bLoading)
              {
                if (totem.ID != 0 && !chkEquipTotems.Checked)
                  {
                    cboLHWTotem.SelectedIndex = 0;
                    cboHWTotem.SelectedIndex = 0;
                  }
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).Totems[HealSpells.ChainHeal] = totem;
                Character.OnCalculationsInvalidated();
              }
          }

        private void chkEquipTotems_CheckedChanged(object sender, EventArgs e)
          {
            if (!_bLoading)
              {
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).EquipTotemsDuringFight = chkEquipTotems.Checked;
                if (!chkEquipTotems.Checked)
                  {
                    if (cboHWTotem.SelectedIndex != 0)
                      {
                        cboLHWTotem.SelectedIndex = 0;
                        cboCHTotem.SelectedIndex = 0;
                      }
                    else if (cboLHWTotem.SelectedIndex != 0)
                      cboCHTotem.SelectedIndex = 0;
                  }
                Character.OnCalculationsInvalidated();
              }
          }

        private void SetHWComboBoxes()
          {
            SetRankBoxes(cboHWMaxRank, cboHWMinRank, (int?)cboHWMaxRank.SelectedItem, (int?)cboHWMinRank.SelectedItem, 12);
          }

        private void cboHWMaxRank_SelectedIndexChanged(object sender, EventArgs e)
          {
            if (Character != null && !_bLoading)
              {
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).HWDownrank.MaxRank = (int)cboHWMaxRank.SelectedItem;
                Character.OnCalculationsInvalidated();
              }
            SetHWComboBoxes();
          }

        private void cboHWMinRank_SelectedIndexChanged(object sender, EventArgs e)
          {
            if (Character != null && !_bLoading)
              {
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).HWDownrank.MinRank = (int)cboHWMinRank.SelectedItem;
                Character.OnCalculationsInvalidated();
              }
            SetHWComboBoxes();
          }

        private void SetCHComboBoxes()
          {
            SetRankBoxes(cboCHMaxRank, cboCHMinRank, (int?)cboCHMaxRank.SelectedItem, (int?)cboCHMinRank.SelectedItem, 5);
          }

        private void cboCHMaxRank_SelectedIndexChanged(object sender, EventArgs e)
          {
            if (Character != null && !_bLoading)
              {
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).CHDownrank.MaxRank = (int)cboCHMaxRank.SelectedItem;
                Character.OnCalculationsInvalidated();
              }
            SetCHComboBoxes();
          }

        private void cboCHMinRank_SelectedIndexChanged(object sender, EventArgs e)
          {
            if (Character != null && !_bLoading)
              {
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).CHDownrank.MinRank = (int)cboCHMinRank.SelectedItem;
                Character.OnCalculationsInvalidated();
              }
            SetCHComboBoxes();
          }

        private void trkHW_ValueChanged(object sender, EventArgs e)
          {
            if (!_bLoading)
              {
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).HWDownrank.Ratio = 100 - trkHW.Value;
                Character.OnCalculationsInvalidated();
              }
            lblHWMaxPct.Text = String.Format("{0}%", 100 - trkHW.Value);
            lblHWMinPct.Text = String.Format("{0}%", trkHW.Value);
          }

        private void trkCH_ValueChanged(object sender, EventArgs e)
          {
            if (!_bLoading)
              {
                ((CalculationOptionsRestoSham)(Character.CalculationOptions)).CHDownrank.Ratio = 100 - trkCH.Value;
                Character.OnCalculationsInvalidated();
              }
            lblCHMaxPct.Text = String.Format("{0}%", 100 - trkCH.Value);
            lblCHMinPct.Text = String.Format("{0}%", trkCH.Value);
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
            
            float  f;
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

        private void cboNumCHTargets_SelectedIndexChanged(object sender, EventArgs e)
          {
            if (!_bLoading)
              {
                this["NumCHTargets"] = cboNumCHTargets.SelectedIndex + 1;
                Character.OnCalculationsInvalidated();
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

        private void cboESRank_SelectedIndexChanged(object sender, EventArgs e)
          {
            if (!_bLoading)
              {
                this["ESRank"] = cboESRank.SelectedIndex + 1;
                Character.OnCalculationsInvalidated();
              }
          }

        private void radioAldor_CheckedChanged(object sender, EventArgs e)
          {
            RadioButton btn = sender as RadioButton;
            if (!btn.Checked)
              return;
              
            Faction faction = (btn.Name == "radioAldor" ? Faction.Aldor : Faction.Scryers);
            RadioButton otherBtn = (faction == Faction.Aldor ? radioScryers : radioAldor);
            
            otherBtn.Checked = false;
            if (!_bLoading)
              {
                this["ExaltedFaction"] = faction;
                Character.OnCalculationsInvalidated();
              }
          }

        private void chkExalted_CheckedChanged(object sender, EventArgs e)
          {
            radioAldor.Enabled = chkExalted.Checked;
            radioScryers.Enabled = chkExalted.Checked;
            
            if (!chkExalted.Checked)
              {
                this["ExaltedFaction"] = Faction.None;
                Character.OnCalculationsInvalidated();
                return;
              }
            
            if (!radioAldor.Checked && !radioScryers.Checked)
              radioAldor.Checked = true;
            else if (!_bLoading)
              {
                this["ExaltedFaction"] = (radioAldor.Checked ? Faction.Aldor : Faction.Scryers);
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
        public float  MinValue = float.MinValue;
        public float  MaxValue = float.MaxValue;
        public bool   CanBeZero = false;
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
        /// Interval of time between mana potion uses, in minutes.
        /// </summary>
        public float ManaPotTime = 2.25f;
        
        /// <summary>
        /// Mp5 received from Shadow Priest(s).
        /// </summary>
        public float SPriestMP5 = 0f;
        
        /// <summary>
        /// Whether a Mana Tide totem is placed every time the cooldown is up.
        /// </summary>
        public bool ManaTideEveryCD = true;
        
        /// <summary>
        /// Whether we keep Water Shield up or not (could use Earth Shield during some fights).
        /// </summary>
        public bool WaterShield = true;
        
        /// <summary>
        /// Used (with LHWRatio and CHRatio) to determine the ratio of HW : LHW : CH cast during the fight.
        /// </summary>
        public float HWRatio = 5.0f;
        
        /// <summary>
        /// Used (with HWRatio and CHRatio) to determine the ratio of HW : LHW : CH cast during the fight.
        /// </summary>
        public float LHWRatio = 5.0f;
        
        /// <summary>
        /// Used (with HWRatio and LHWRatio) to determine the ratio of HW : LHW : CH cast during the fight.
        /// </summary>
        public float CHRatio = 85.0f;
        
        /// <summary>
        /// Interval of time between Earth Shield casts, in seconds.
        /// </summary>
        public float ESInterval = 60f;
        
        /// <summary>
        /// Rank of Earth Shield that is used.
        /// </summary>
        public int ESRank = 3;
        
        /// <summary>
        /// Average number of targets healed by Chain Heal.
        /// </summary>
        public int NumCHTargets = 2;
        
        /// <summary>
        /// Information about downranking of Healing Wave during the fight.
        /// </summary>
        public DownrankInfo HWDownrank = new DownrankInfo(12, 8, 100);
        
        /// <summary>
        /// Information about downranking of Chain Heal during the fight.
        /// </summary>
        public DownrankInfo CHDownrank = new DownrankInfo(5, 3, 100);
        
        /// <summary>
        /// A dictionary that contains the totem used during the fight for each spell.
        /// </summary>
        public SerializableDictionary<HealSpells, TotemRelic> Totems;
        
        /// <summary>
        /// Whether or not to equip different totems during the fight.
        /// </summary>
        public bool EquipTotemsDuringFight = true;
        
        /// <summary>
        /// Which faction (Aldor, Scryers) we are exalted with (if any).
        /// </summary>
        public Faction ExaltedFaction = Faction.None;
        
        
        [XmlIgnore]
        public float HWWeight
          {
            get { return (HWRatio / (HWRatio + LHWRatio + CHRatio)); }
          }
          
        [XmlIgnore]
        public float LHWWeight
          {
            get { return (LHWRatio / (HWRatio + LHWRatio + CHRatio)); }
          }
          
        [XmlIgnore]
        public float CHWeight
          {
            get { return (CHRatio / (HWRatio + LHWRatio + CHRatio)); }
          }
        
        
        public CalculationOptionsRestoSham()
          {
            Totems = new SerializableDictionary<HealSpells,TotemRelic>();
            Totems.Add(HealSpells.HealingWave, new TotemRelic());
            Totems.Add(HealSpells.LesserHealingWave, new TotemRelic());
            Totems.Add(HealSpells.ChainHeal, new TotemRelic());
          }
      }

    
    [Serializable]
    public class DownrankInfo
      {
        public DownrankInfo()
          {
          }
          
        public DownrankInfo(int max, int min, int ratio)
          {
            MaxRank = max;
            MinRank = min;
            Ratio = ratio;
          }
        
        /// <summary>
        /// Higher rank cast.
        /// </summary>
        public int MaxRank;
        
        /// <summary>
        /// Lower rank cast.
        /// </summary>
        public int MinRank;
        
        /// <summary>
        /// Ratio of MaxRank to MinRank spells cast.
        /// </summary>
        public int Ratio;
      }
  }
