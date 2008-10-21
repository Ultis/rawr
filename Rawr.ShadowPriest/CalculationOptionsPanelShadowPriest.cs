using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.ShadowPriest
{
    public partial class CalculationOptionsPanelShadowPriest : CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelShadowPriest()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsShadowPriest();

            CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
            cbTargetLevel.SelectedIndex = calcOpts.TargetLevel;
            
            trkFightLength.Value = (int)calcOpts.FightLength;
            lblFightLength.Text = trkFightLength.Value + " minute fight.";

            trkShadowfiend.Value = (int)calcOpts.Shadowfiend;
            lblShadowfiend.Text = trkShadowfiend.Value + "% effect from Shadowfiend.";

            trkReplenishment.Value = (int)calcOpts.Replenishment;
            lblReplenishment.Text = trkReplenishment.Value + "% effect from Replenishment.";


            //cmbLength.Value = (decimal)calcOpts.FightDuration;
            cmbManaAmt.Text = calcOpts.ManaAmt.ToString();
            cmbManaTime.Value = (decimal)calcOpts.ManaTime;
            if (calcOpts.SpellPriority == null)
                calcOpts.SpellPriority = new List<string>(Spell.SpellList);
            lsSpellPriopity.Items.Clear();
            lsSpellPriopity.Items.AddRange(calcOpts.SpellPriority.ToArray());
            
            loading = false;
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbManaTime_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.ManaTime = (float)cmbManaTime.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void cmbManaAmt_TextUpdate(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                try
                {
                    calcOpts.ManaAmt = float.Parse(cmbManaAmt.Text);
                }
                catch { }
                Character.OnCalculationsInvalidated();
            }
        }
                
        private void bChangePriority_Click(object sender, EventArgs e)
        {
            CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
            SpellPriorityForm priority = new SpellPriorityForm(calcOpts.SpellPriority, lsSpellPriopity);
            priority.Show();
        }

        private void cbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.TargetLevel = cbTargetLevel.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkFightLength_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.FightLength = trkFightLength.Value;
                lblFightLength.Text = trkFightLength.Value + " minute fight.";
            }
        }

        private void trkShadowfiend_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.Shadowfiend = trkShadowfiend.Value;
                lblShadowfiend.Text = trkShadowfiend.Value + "% effect from Shadowfiend.";
            }
        }

        private void trkReplenishment_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.Replenishment = trkReplenishment.Value;
                lblReplenishment.Text = trkReplenishment.Value + "% effect from Replenishment.";
            }
        }
    }
    [Serializable]
	public class CalculationOptionsShadowPriest : ICalculationOptionBase
	{
        public int TargetLevel { get; set; }
        public float FightLength { get; set; }
        public float Shadowfiend { get; set; }
        public float Replenishment { get; set; }

        public List<string> SpellPriority { get; set; }

        public float ManaAmt { get; set; }
        public float ManaTime { get; set; }
        public float Spriest { get; set; }
        public float Lag { get; set; }
        public float WaitTime { get; set; }
        public float ISBUptime { get; set; }
        public int DrumsCount { get; set; }
        public bool UseYourDrum { get; set; }

        public CalculationOptionsShadowPriest()
        {
            TargetLevel = 3;
            FightLength = 6f;
            Shadowfiend = 80f;
            Replenishment = 100f;

            SpellPriority = null;
            ManaAmt = 2400;
            ManaTime = 60;
            Lag = 100;
            WaitTime = 50;
            ISBUptime = 30;
            DrumsCount = 0;
            UseYourDrum = false;
        }

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsShadowPriest));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
	}
}
