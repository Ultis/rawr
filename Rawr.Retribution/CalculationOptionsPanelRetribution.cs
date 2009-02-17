using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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

            chkMode31.Checked = calcOpts.Mode31;
            chkGlyphJudgement.Checked = calcOpts.GlyphJudgement;
            chkGlyphConsecration.Checked = calcOpts.GlyphConsecration;
            chkGlyphSenseUndead.Checked = calcOpts.GlyphSenseUndead;

            UpdatePriorityDisplay(calcOpts);

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

        private void chk31Mode_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
                calcOpts.Mode31 = chkMode31.Checked;
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

        private void UpdatePriorityDisplay(CalculationOptionsRetribution calcOpts)
        {
            lblPriority1.Text = calcOpts.Priorities[0].ToString();
            lblPriority2.Text = calcOpts.Priorities[1].ToString();
            lblPriority3.Text = calcOpts.Priorities[2].ToString();
            lblPriority4.Text = calcOpts.Priorities[3].ToString();
            lblPriority5.Text = calcOpts.Priorities[4].ToString();
            lblPriority6.Text = calcOpts.Priorities[5].ToString();
        }

        private void PrioritySwitch1(object sender, EventArgs e)
        {
            CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
            calcOpts.Priorities = new Rotation.Ability[] { calcOpts.Priorities[1], calcOpts.Priorities[0], calcOpts.Priorities[2], 
                calcOpts.Priorities[3], calcOpts.Priorities[4], calcOpts.Priorities[5] };
            UpdatePriorityDisplay(calcOpts);
            Character.OnCalculationsInvalidated();
        }

        private void PrioritySwitch2(object sender, EventArgs e)
        {
            CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
            calcOpts.Priorities = new Rotation.Ability[] { calcOpts.Priorities[0], calcOpts.Priorities[2], calcOpts.Priorities[1], 
                calcOpts.Priorities[3], calcOpts.Priorities[4], calcOpts.Priorities[5] };
            UpdatePriorityDisplay(calcOpts);
            Character.OnCalculationsInvalidated();
        }

        private void PrioritySwitch3(object sender, EventArgs e)
        {
            CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
            calcOpts.Priorities = new Rotation.Ability[] { calcOpts.Priorities[0], calcOpts.Priorities[1], calcOpts.Priorities[3], 
                calcOpts.Priorities[2], calcOpts.Priorities[4], calcOpts.Priorities[5] };
            UpdatePriorityDisplay(calcOpts);
            Character.OnCalculationsInvalidated();
        }

        private void PrioritySwitch4(object sender, EventArgs e)
        {
            CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
            calcOpts.Priorities = new Rotation.Ability[] { calcOpts.Priorities[0], calcOpts.Priorities[1], calcOpts.Priorities[2], 
                calcOpts.Priorities[4], calcOpts.Priorities[3], calcOpts.Priorities[5] };
            UpdatePriorityDisplay(calcOpts);
            Character.OnCalculationsInvalidated();
        }

        private void PrioritySwitch5(object sender, EventArgs e)
        {
            CalculationOptionsRetribution calcOpts = Character.CalculationOptions as CalculationOptionsRetribution;
            calcOpts.Priorities = new Rotation.Ability[] { calcOpts.Priorities[0], calcOpts.Priorities[1], calcOpts.Priorities[2], 
                calcOpts.Priorities[3], calcOpts.Priorities[5], calcOpts.Priorities[4] };
            UpdatePriorityDisplay(calcOpts);
            Character.OnCalculationsInvalidated();
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
        public bool Mode31 = false;

        public Rotation.Ability[] Priorities = { Rotation.Ability.Judgement, Rotation.Ability.HammerOfWrath, Rotation.Ability.CrusaderStrike,
                                                   Rotation.Ability.DivineStorm, Rotation.Ability.Consecration, Rotation.Ability.Exorcism };

        public bool GlyphJudgement = true;
        public bool GlyphConsecration = true;
        public bool GlyphSenseUndead = true;

	}
}
