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

            nudDelay.Value = (decimal)calcOpts.Delay;
            nudWait.Value = (decimal)calcOpts.Wait;

            nudJudge.Value = (decimal)calcOpts.JudgeCD;
            nudCS.Value = (decimal)calcOpts.CSCD;
            nudDS.Value = (decimal)calcOpts.DSCD;
            nudCons.Value = (decimal)calcOpts.ConsCD;
            nudExo.Value = (decimal)calcOpts.ExoCD;
            nudJudge20.Value = (decimal)calcOpts.JudgeCD20;
            nudCS20.Value = (decimal)calcOpts.CSCD20;
            nudDS20.Value = (decimal)calcOpts.DSCD20;
            nudCons20.Value = (decimal)calcOpts.ConsCD20;
            nudExo20.Value = (decimal)calcOpts.ExoCD20;
            nudHoW20.Value = (decimal)calcOpts.HoWCD20;

            chkGlyphJudgement.Checked = calcOpts.GlyphJudgement;
            chkGlyphConsecration.Checked = calcOpts.GlyphConsecration;
            chkGlyphSenseUndead.Checked = calcOpts.GlyphSenseUndead;
            chkGlyphExorcism.Checked = calcOpts.GlyphExorcism;

            UpdatePriority(calcOpts);

            if (calcOpts.SimulateRotation) radRotSim.Checked = true;
            else radEffectiveCD.Checked = true;
            SetSimulateRotation(calcOpts.SimulateRotation);

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
                calcOpts.TargetLevel = cmbLevel.SelectedIndex + 80;
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

        private void nudDelay_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Delay = (float)nudDelay.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void SetSimulateRotation(bool rot)
        {
            if (rot)
            {
                listUnlimitedPriority.Enabled = true;
                lblDelay.Enabled = true;
                lblWait.Enabled = true;
                butUnlimitedDown.Enabled = true;
                butUnlimitedUp.Enabled = true;
                nudDelay.Enabled = true;
                nudWait.Enabled = true;
                nudJudge.Enabled = false;
                nudJudge20.Enabled = false;
                nudCS.Enabled = false;
                nudCS20.Enabled = false;
                nudDS.Enabled = false;
                nudDS20.Enabled = false;
                nudCons.Enabled = false;
                nudCons20.Enabled = false;
                nudExo.Enabled = false;
                nudExo20.Enabled = false;
                nudHoW20.Enabled = false;
                lblPost20.Enabled = false;
                lblPre20.Enabled = false;
                lblJudge.Enabled = false;
                lblCS.Enabled = false;
                lblDS.Enabled = false;
                lblCons.Enabled = false;
                lblExo.Enabled = false;
                lblHoW.Enabled = false;
            }
            else
            {
                listUnlimitedPriority.Enabled = false;
                lblDelay.Enabled = false;
                lblWait.Enabled = false;
                butUnlimitedDown.Enabled = false;
                butUnlimitedUp.Enabled = false;
                nudDelay.Enabled = false;
                nudWait.Enabled = false;
                nudJudge.Enabled = true;
                nudJudge20.Enabled = true;
                nudCS.Enabled = true;
                nudCS20.Enabled = true;
                nudDS.Enabled = true;
                nudDS20.Enabled = true;
                nudCons.Enabled = true;
                nudCons20.Enabled = true;
                nudExo.Enabled = true;
                nudExo20.Enabled = true;
                nudHoW20.Enabled = true;
                lblPost20.Enabled = true;
                lblPre20.Enabled = true;
                lblJudge.Enabled = true;
                lblCS.Enabled = true;
                lblDS.Enabled = true;
                lblCons.Enabled = true;
                lblExo.Enabled = true;
                lblHoW.Enabled = true;
            }
        }

        private void radRotSim_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                SetSimulateRotation(radRotSim.Checked);
                calcOpts.SimulateRotation = radRotSim.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void txtJudge_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Delay = (float)nudDelay.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudJudge_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.JudgeCD = (float)nudJudge.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudJudge20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.JudgeCD20 = (float)nudJudge20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudCS_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.CSCD = (float)nudCS.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudCS20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.CSCD20 = (float)nudCS20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudDS_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.DSCD = (float)nudDS.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudDS20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.DSCD20 = (float)nudDS20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudCons_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.ConsCD = (float)nudCons.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudCons20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.ConsCD20 = (float)nudCons20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudExo_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.ExoCD = (float)nudExo.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudExo20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.ExoCD20 = (float)nudExo20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudHoW20_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.HoWCD20 = (float)nudHoW20.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void nudWait_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Wait = (float)nudWait.Value;
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
        public int MobType = 2;
        public float FightLength = 5f;
        public float TimeUnder20 = .18f;
        public float Delay = .05f;
        public float Wait = .05f;

        public bool SimulateRotation = true;

        private Rotation.Ability[] _order = { Rotation.Ability.CrusaderStrike, Rotation.Ability.HammerOfWrath, Rotation.Ability.Judgement,
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

        public float JudgeCD = 7.1f;
        public float CSCD = 7.1f;
        public float DSCD = 10.5f;
        public float ConsCD = 10.5f;
        public float ExoCD = 18f;

        public float JudgeCD20 = 7.1f;
        public float CSCD20 = 7.1f;
        public float DSCD20 = 12.5f;
        public float ConsCD20 = 12.5f;
        public float ExoCD20 = 25f;
        public float HoWCD20 = 6.4f;

        public CalculationOptionsRetribution Clone()
        {
            CalculationOptionsRetribution clone = new CalculationOptionsRetribution();

            clone.TargetLevel = TargetLevel;
            clone.MobType = MobType;
            clone.FightLength = FightLength;
            clone.TimeUnder20 = TimeUnder20;
            clone.Delay = Delay;
            clone.SimulateRotation = SimulateRotation;

            clone.JudgeCD = JudgeCD;
            clone.CSCD = CSCD;
            clone.DSCD = DSCD;
            clone.ConsCD = ConsCD;
            clone.ExoCD = ExoCD;

            clone.JudgeCD20 = JudgeCD20;
            clone.CSCD20 = CSCD20;
            clone.DSCD20 = DSCD20;
            clone.ConsCD20 = ConsCD20;
            clone.ExoCD20 = ExoCD20;
            clone.HoWCD20 = HoWCD20;

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
