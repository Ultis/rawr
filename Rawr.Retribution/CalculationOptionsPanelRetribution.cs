using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Rawr.Retribution
{
    public partial class CalculationOptionsPanelRetribution : CalculationOptionsPanelBase
    {

        public CalculationOptionsPanelRetribution()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsRetribution();

            CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;

            cmbMobType.SelectedIndex = calcOpts.MobType;
            cmbLevel.SelectedIndex = calcOpts.TargetLevel - 80;
            cmbLength.Value = (decimal)calcOpts.FightLength;

            trkTime20.Value = (int)(calcOpts.TimeUnder20 * 100);
            lblTime20.Text = trkTime20.Value + "%";

            chkGlyphJudgement.Checked = calcOpts.GlyphJudgement;
            chkGlyphConsecration.Checked = calcOpts.GlyphConsecration;
            chkGlyphSenseUndead.Checked = calcOpts.GlyphSenseUndead;

            chkSimulateMana.Checked = calcOpts.SimulateMana;

            UpdateUnlimitedPriority(calcOpts);
            UpdateLimitedPriority(calcOpts);

            loading = false;
        }

        private void cmbMobType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.MobType = cmbMobType.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGlyphJudgement_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.GlyphJudgement = chkGlyphJudgement.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.TargetLevel = cmbMobType.SelectedIndex + 80;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.FightLength = (float)cmbLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkTime20_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.TimeUnder20 = trkTime20.Value / 100f;
                lblTime20.Text = trkTime20.Value + "%";
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGlyphConsecration_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.GlyphConsecration = chkGlyphConsecration.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGlyphSenseUndead_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.GlyphSenseUndead = chkGlyphSenseUndead.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void UpdateUnlimitedPriority(CalculationOptionsRetribution calcOpts)
        {
            bool wasLoading = loading;
            loading = true;

            listUnlimitedPriority.Items.Clear();
            listUnlimitedPriority.Items.AddRange(new string[] { Rotation.AbilityString(calcOpts.UnlimitedOrder[0]), Rotation.AbilityString(calcOpts.UnlimitedOrder[1]),
                 Rotation.AbilityString(calcOpts.UnlimitedOrder[2]), Rotation.AbilityString(calcOpts.UnlimitedOrder[3]),
                  Rotation.AbilityString(calcOpts.UnlimitedOrder[4]), Rotation.AbilityString(calcOpts.UnlimitedOrder[5])});

            for (int i = 0; i < 6; i++) listUnlimitedPriority.SetItemChecked(i, calcOpts.UnlimitedSelected[i]);

            loading = wasLoading;
        }

        private void butUnlimitedUp_Click(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                int sel = listUnlimitedPriority.SelectedIndex;
                if (sel > 0 && sel <= 5)
                {
                    Rotation.Ability temp1 = calcOpts.UnlimitedOrder[sel - 1];
                    calcOpts.UnlimitedOrder[sel - 1] = calcOpts.UnlimitedOrder[sel];
                    calcOpts.UnlimitedOrder[sel] = temp1;

                    bool temp2 = calcOpts.UnlimitedSelected[sel - 1];
                    calcOpts.UnlimitedSelected[sel - 1] = calcOpts.UnlimitedSelected[sel];
                    calcOpts.UnlimitedSelected[sel] = temp2;

                    UpdateUnlimitedPriority(calcOpts);
                    listUnlimitedPriority.SelectedIndex = sel - 1;
                    Character.OnCalculationsInvalidated();
                }
            }
        }

        private void butUnlimitedDown_Click(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                int sel = listUnlimitedPriority.SelectedIndex;
                if (sel >= 0 && sel < 5)
                {
                    Rotation.Ability temp1 = calcOpts.UnlimitedOrder[sel + 1];
                    calcOpts.UnlimitedOrder[sel + 1] = calcOpts.UnlimitedOrder[sel];
                    calcOpts.UnlimitedOrder[sel] = temp1;

                    bool temp2 = calcOpts.UnlimitedSelected[sel + 1];
                    calcOpts.UnlimitedSelected[sel + 1] = calcOpts.UnlimitedSelected[sel];
                    calcOpts.UnlimitedSelected[sel] = temp2;

                    UpdateUnlimitedPriority(calcOpts);
                    listUnlimitedPriority.SelectedIndex = sel + 1;
                    Character.OnCalculationsInvalidated();
                }
            }
        }

        private void listUnlimitedPriority_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                
                if (e.NewValue == CheckState.Unchecked) calcOpts.UnlimitedSelected[e.Index] = false;
                else calcOpts.UnlimitedSelected[e.Index] = true;

                Character.OnCalculationsInvalidated();
            }
        }

        private void UpdateLimitedPriority(CalculationOptionsRetribution calcOpts)
        {
            bool wasLoading = loading;
            loading = true;

            listLimitedPriority.Items.Clear();
            listLimitedPriority.Items.AddRange(new string[] { Rotation.AbilityString(calcOpts.LimitedOrder[0]), Rotation.AbilityString(calcOpts.LimitedOrder[1]),
                 Rotation.AbilityString(calcOpts.LimitedOrder[2]), Rotation.AbilityString(calcOpts.LimitedOrder[3]),
                  Rotation.AbilityString(calcOpts.LimitedOrder[4]), Rotation.AbilityString(calcOpts.LimitedOrder[5])});

            for (int i = 0; i < 6; i++) listLimitedPriority.SetItemChecked(i, calcOpts.LimitedSelected[i]);

            loading = wasLoading;
        }

        private void butLimitedUp_Click(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                int sel = listLimitedPriority.SelectedIndex;
                if (sel > 0 && sel <= 5)
                {
                    Rotation.Ability temp1 = calcOpts.LimitedOrder[sel - 1];
                    calcOpts.LimitedOrder[sel - 1] = calcOpts.LimitedOrder[sel];
                    calcOpts.LimitedOrder[sel] = temp1;

                    bool temp2 = calcOpts.LimitedSelected[sel - 1];
                    calcOpts.LimitedSelected[sel - 1] = calcOpts.LimitedSelected[sel];
                    calcOpts.LimitedSelected[sel] = temp2;

                    UpdateLimitedPriority(calcOpts);
                    listLimitedPriority.SelectedIndex = sel - 1;
                    Character.OnCalculationsInvalidated();
                }
            }
        }

        private void butLimitedDown_Click(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                int sel = listLimitedPriority.SelectedIndex;
                if (sel >= 0 && sel < 5)
                {
                    Rotation.Ability temp1 = calcOpts.LimitedOrder[sel + 1];
                    calcOpts.LimitedOrder[sel + 1] = calcOpts.LimitedOrder[sel];
                    calcOpts.LimitedOrder[sel] = temp1;

                    bool temp2 = calcOpts.LimitedSelected[sel + 1];
                    calcOpts.LimitedSelected[sel + 1] = calcOpts.LimitedSelected[sel];
                    calcOpts.LimitedSelected[sel] = temp2;

                    UpdateLimitedPriority(calcOpts);
                    listLimitedPriority.SelectedIndex = sel + 1;
                    Character.OnCalculationsInvalidated();
                }
            }
        }

        private void listLimitedPriority_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                
                if (e.NewValue == CheckState.Unchecked) calcOpts.LimitedSelected[e.Index] = false;
                else calcOpts.LimitedSelected[e.Index] = true;

                Character.OnCalculationsInvalidated();
            }
        }

        private void chkSimulateMana_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.SimulateMana = chkSimulateMana.Checked;
                Character.OnCalculationsInvalidated();               
            }
        }


    }

	[Serializable]
	public class CalculationOptionsRetribution : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsRetribution));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
        public int MobType = 0;
        public float FightLength = 5f;
        public float TimeUnder20 = .18f;

        public bool SimulateMana = false;

        private Rotation.Ability[] _unlimitedOrder = { Rotation.Ability.Judgement, Rotation.Ability.HammerOfWrath, Rotation.Ability.CrusaderStrike,
                                                   Rotation.Ability.DivineStorm, Rotation.Ability.Consecration, Rotation.Ability.Exorcism };
        public Rotation.Ability[] UnlimitedOrder {
            get { _unlimitedCache = null; return _unlimitedOrder; }
            set { _unlimitedCache = null; _unlimitedOrder = value; }
        }

        private Rotation.Ability[] _limitedOrder = { Rotation.Ability.Judgement, Rotation.Ability.HammerOfWrath, Rotation.Ability.CrusaderStrike,
                                                   Rotation.Ability.DivineStorm, Rotation.Ability.Consecration, Rotation.Ability.Exorcism };
        public Rotation.Ability[] LimitedOrder
        {
            get { _limitedCache = null; return _limitedOrder; }
            set { _limitedCache = null; _limitedOrder = value; }
        }

        private bool[] _unlimitedSelected = { true, true, true, true, true, true };
        public bool[] UnlimitedSelected
        {
            get { _unlimitedCache = null; return _unlimitedSelected; }
            set { _unlimitedCache = null; _unlimitedSelected = value; }
        }

        private bool[] _limitedSelected = { true, true, true, true, false, false };
        public bool[] LimitedSelected
        {
            get { _limitedCache = null; return _limitedSelected; }
            set { _limitedCache = null; _limitedSelected = value; }
        }

        private Rotation.Ability[] _unlimitedCache = null;

        [XmlIgnore]        
        public Rotation.Ability[] UnlimitedPriorities
        {
            get
            {
                if (_unlimitedCache == null)
                {
                    int count = 0;
                    foreach (bool b in _unlimitedSelected) { if (b) count++; }
                    _unlimitedCache = new Rotation.Ability[count];

                    int sel = 0;
                    for (int i = 0; i < _unlimitedOrder.Length; i++)
                    {
                        if (_unlimitedSelected[i])
                        {
                            _unlimitedCache[sel] = _unlimitedOrder[i];
                            sel++;
                        }
                    }
                }
                return _unlimitedCache;
            }
        }

        private Rotation.Ability[] _limitedCache;

        [XmlIgnore]
        public Rotation.Ability[] LimitedPriorities
        {
            get
            {
                if (_limitedCache == null)
                {
                    int count = 0;
                    foreach (bool b in _limitedSelected) { if (b) count++; }
                    _limitedCache = new Rotation.Ability[count];

                    int sel = 0;
                    for (int i = 0; i < _limitedOrder.Length; i++)
                    {
                        if (_limitedSelected[i])
                        {
                            _limitedCache[sel] = _limitedOrder[i];
                            sel++;
                        }
                    }
                }
                return _limitedCache;
            }
        }
        

        public bool GlyphJudgement = true;
        public bool GlyphConsecration = true;
        public bool GlyphSenseUndead = true;

	}
}
