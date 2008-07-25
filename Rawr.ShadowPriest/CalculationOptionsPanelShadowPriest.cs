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
            cmbLength.Value = (decimal)calcOpts.FightDuration;
            cmbManaAmt.Text = calcOpts.ManaAmt.ToString();
            cmbManaTime.Value = (decimal)calcOpts.ManaTime;
            cmbSpriest.Value = (decimal)calcOpts.Spriest;
            lsSpellPriopity.Items.Clear();
            if (Character.Race != Character.CharacterRace.NightElf)
                calcOpts.SpellPriority.Remove("Starshards");
            else if (!calcOpts.SpellPriority.Contains("Starshards"))
                calcOpts.SpellPriority.Add("Starshards");
            if (Character.Race != Character.CharacterRace.Undead)
                calcOpts.SpellPriority.Remove("Devouring Plague");
            else if (!calcOpts.SpellPriority.Contains("Devouring Plague"))
                calcOpts.SpellPriority.Add("Devouring Plague");
            lsSpellPriopity.Items.AddRange(calcOpts.SpellPriority.ToArray());
            
            loading = false;
        }

        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.FightDuration = (float)cmbLength.Value;
                Character.OnItemsChanged();
            }
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
                Character.OnItemsChanged();
            }
        }

        private void cmbManaTime_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.ManaTime = (float)cmbManaTime.Value;
                Character.OnItemsChanged();
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
                Character.OnItemsChanged();
            }
        }
                
        private void cmbSpriest_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.Spriest = (float)cmbSpriest.Value;
                Character.OnItemsChanged();
            }
        }              

        private void bChangePriority_Click(object sender, EventArgs e)
        {
            CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
            SpellPriorityForm priority = new SpellPriorityForm(calcOpts.SpellPriority, lsSpellPriopity);
            priority.Show();
        }

        private void cmbDrums_ValueChanged(object sender, EventArgs e)
        {
            if (cmbDrums.Value == 0)
            {
                cbDrums.Checked = false;
                cbDrums.Enabled = false;
            }
            else
            {
                cbDrums.Enabled = true;
            }

            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.DrumsCount = (int)cmbDrums.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbLag_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.Lag = (float)cmbLag.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbWaitTime_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.WaitTime = (float)cmbWaitTime.Value;
                Character.OnItemsChanged();
            }
        }

        private void cmbISB_ValueChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.ISBUptime = (float)cmbISB.Value;
                Character.OnItemsChanged();
            }
        }

        private void cbDrums_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsShadowPriest calcOpts = Character.CalculationOptions as CalculationOptionsShadowPriest;
                calcOpts.UseYourDrum = cbDrums.Checked;
                Character.OnItemsChanged();
            }
        }        
    }
    [Serializable]
	public class CalculationOptionsShadowPriest : ICalculationOptionBase
	{
        public float FightDuration { get; set; }
        public List<string> SpellPriority { get; set; }
        
        public bool EnforceMetagemRequirements { get; set; }
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
            SpellPriority = new List<string>(Spell.SpellList);
            FightDuration = 600;
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
