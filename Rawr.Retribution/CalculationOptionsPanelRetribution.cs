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
            chkGlyphExorcism.Checked = calcOpts.GlyphExorcism;

            UpdatePriority(calcOpts);

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

        private void UpdatePriority(CalculationOptionsRetribution calcOpts)
        {
            bool wasLoading = loading;
            loading = true;

            listUnlimitedPriority.Items.Clear();
            listUnlimitedPriority.Items.AddRange(new string[] { Rotation.AbilityString(calcOpts.Order[0]), Rotation.AbilityString(calcOpts.Order[1]),
                 Rotation.AbilityString(calcOpts.Order[2]), Rotation.AbilityString(calcOpts.Order[3]),
                  Rotation.AbilityString(calcOpts.Order[4]), Rotation.AbilityString(calcOpts.Order[5])});

            for (int i = 0; i < 6; i++) listUnlimitedPriority.SetItemChecked(i, calcOpts.Selected[i]);

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
                    Rotation.Ability temp1 = calcOpts.Order[sel - 1];
                    calcOpts.Order[sel - 1] = calcOpts.Order[sel];
                    calcOpts.Order[sel] = temp1;

                    bool temp2 = calcOpts.Selected[sel - 1];
                    calcOpts.Selected[sel - 1] = calcOpts.Selected[sel];
                    calcOpts.Selected[sel] = temp2;

                    UpdatePriority(calcOpts);
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
                    Rotation.Ability temp1 = calcOpts.Order[sel + 1];
                    calcOpts.Order[sel + 1] = calcOpts.Order[sel];
                    calcOpts.Order[sel] = temp1;

                    bool temp2 = calcOpts.Selected[sel + 1];
                    calcOpts.Selected[sel + 1] = calcOpts.Selected[sel];
                    calcOpts.Selected[sel] = temp2;

                    UpdatePriority(calcOpts);
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
                
                if (e.NewValue == CheckState.Unchecked) calcOpts.Selected[e.Index] = false;
                else calcOpts.Selected[e.Index] = true;

                Character.OnCalculationsInvalidated();
            }
        }

        private void chkGlyphExorcism_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.GlyphExorcism = chkGlyphExorcism.Checked;
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

        private Rotation.Ability[] _order = { Rotation.Ability.Judgement, Rotation.Ability.HammerOfWrath, Rotation.Ability.CrusaderStrike,
                                                   Rotation.Ability.DivineStorm, Rotation.Ability.Consecration, Rotation.Ability.Exorcism };
        public Rotation.Ability[] Order {
            get { _cache = null; return _order; }
            set { _cache = null; _order = value; }
        }

        private bool[] _selected = { true, true, true, true, true, true };
        public bool[] Selected
        {
            get { _cache = null; return _selected; }
            set { _cache = null; _selected = value; }
        }

        private Rotation.Ability[] _cache = null;

        [XmlIgnore]        
        public Rotation.Ability[] Priorities
        {
            get
            {
                if (_cache == null)
                {
                    int count = 0;
                    foreach (bool b in _selected) { if (b) count++; }
                    _cache = new Rotation.Ability[count];

                    int sel = 0;
                    for (int i = 0; i < _order.Length; i++)
                    {
                        if (_selected[i])
                        {
                            _cache[sel] = _order[i];
                            sel++;
                        }
                    }
                }
                return _cache;
            }
        }        

        public bool GlyphJudgement = true;
        public bool GlyphConsecration = true;
        public bool GlyphSenseUndead = true;
        public bool GlyphExorcism = true;

        public CalculationOptionsRetribution Clone()
        {
            CalculationOptionsRetribution clone = new CalculationOptionsRetribution();

            clone.TargetLevel = TargetLevel;
            clone.MobType = MobType;
            clone.FightLength = FightLength;
            clone.TimeUnder20 = TimeUnder20;
            clone.SimulateMana = SimulateMana;

            clone._order = (Rotation.Ability[])_order.Clone();
            clone._selected = (bool[])_selected.Clone();

            clone.GlyphJudgement = GlyphJudgement;
            clone.GlyphConsecration = GlyphConsecration;
            clone.GlyphSenseUndead = GlyphSenseUndead;
            clone.GlyphExorcism = GlyphExorcism;

            return clone;
        }

    }
}
