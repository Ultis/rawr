using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Warlock
{
    public partial class CalculationOptionsPanelWarlock : CalculationOptionsPanelBase
    {
        public CalculationOptionsPanelWarlock()
        {
            InitializeComponent();
        }

        private bool loading;

        protected override void LoadCalculationOptions()
        {
            loading = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsWarlock();

            CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;

            cbTargetLevel.SelectedIndex = calcOpts.TargetLevel;

            trkFightLength.Value = (int)calcOpts.FightLength;
            lblFightLength.Text = trkFightLength.Value + " minute fight.";

            trkFSR.Value = (int)calcOpts.FSRRatio;
            lblFSR.Text = trkFSR.Value + "% time spent in FSR";

            trkDelay.Value = (int)calcOpts.Delay;
            lblDelay.Text = trkDelay.Value + "ms Game/Brain Latency";

            trkReplenishment.Value = (int)calcOpts.Replenishment;
            lblReplenishment.Text = trkReplenishment.Value + "% effect from Replenishment.";

            trkJoW.Value = (int)calcOpts.JoW;
            lblJoW.Text = trkJoW.Value + "% effect from JoW.";

            cbManaAmt.SelectedIndex = calcOpts.ManaPot;

            if (calcOpts.SpellPriority == null)
                calcOpts.SpellPriority = new List<string>() {"Shadow Bolt"};
            lsSpellPriopity.Items.Clear();
            lsSpellPriopity.Items.AddRange(calcOpts.SpellPriority.ToArray());

            tbAffEffects.Text = calcOpts.AffEffectsNumber.ToString();

            cbPet.SelectedItem = calcOpts.Pet;

            chbUseInfernal.Checked = calcOpts.UseInfernal;
            chbImmoAura.Checked = calcOpts.UseImmoAura;
            chbGlyphChaosBolt.Checked = calcOpts.GlyphChaosBolt;
            chbGlyphConflag.Checked = calcOpts.GlyphConflag;
            chbGlyphCorruption.Checked = calcOpts.GlyphCorruption;
            chbGlyphCoA.Checked = calcOpts.GlyphCoA;
            chbGlyphFelguard.Checked = calcOpts.GlyphFelguard;
            chbGlyphHaunt.Checked = calcOpts.GlyphHaunt;
            chbGlyphImmolate.Checked = calcOpts.GlyphImmolate;
            chbGlyphImp.Checked = calcOpts.GlyphImp;
            chbGlyphIncinerate.Checked = calcOpts.GlyphIncinerate;
            chbGlyphLifeTap.Checked = calcOpts.GlyphLifeTap;
            chbGlyphMetamorphosis.Checked = calcOpts.GlyphMetamorphosis;
            chbGlyphSearingPain.Checked = calcOpts.GlyphSearingPain;
            chbGlyphSB.Checked = calcOpts.GlyphSB;
            chbGlyphShadowburn.Checked = calcOpts.GlyphShadowburn;
            chbGlyphUA.Checked = calcOpts.GlyphUA;

            loading = false;
        }

        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.ManaPot = cbManaAmt.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private void bChangePriority_Click(object sender, EventArgs e)
        {
            CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
            SpellPriorityForm priority = new SpellPriorityForm(calcOpts.SpellPriority, lsSpellPriopity, Character);
            priority.Show();
        }

        private void cbTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.TargetLevel = cbTargetLevel.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkFightLength_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.FightLength = trkFightLength.Value;
                lblFightLength.Text = trkFightLength.Value + " minute fight.";
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkFSR_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.FSRRatio = trkFSR.Value;
                lblFSR.Text = trkFSR.Value + "% time spent in FSR";
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkDelay_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Delay = trkDelay.Value;
                lblDelay.Text = trkDelay.Value + "ms Game/Brain Latency";
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkReplenishment_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Replenishment = trkReplenishment.Value;
                lblReplenishment.Text = trkReplenishment.Value + "% effect from Replenishment.";
                Character.OnCalculationsInvalidated();
            }
        }

        private void trkJoW_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.JoW = trkJoW.Value;
                lblJoW.Text = trkJoW.Value + "% effect from JoW.";
                Character.OnCalculationsInvalidated();
            }
        }

        private void cbPet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Pet = (String)cbPet.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbUseInfernal_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.UseInfernal = chbUseInfernal.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbAffEffects_Changed(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.AffEffectsNumber = System.Convert.ToInt32(tbAffEffects.Text);
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbAffEffects_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!loading)
                if (char.IsNumber(e.KeyChar) == false && char.IsControl(e.KeyChar) == false)
                    e.Handled = true;
        }

        private void chbGlyphConflag_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphConflag = chbGlyphConflag.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphCorruption_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
            calcOpts.GlyphCorruption = chbGlyphCorruption.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void chbGlyphCoA_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphCoA = chbGlyphCoA.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphFelguard_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphFelguard = chbGlyphFelguard.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphImmolate_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphImmolate = chbGlyphImmolate.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphImp_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphImp = chbGlyphImp.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphSearingPain_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphSearingPain = chbGlyphSearingPain.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphSB_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphSB = chbGlyphSB.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphUA_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphUA = chbGlyphUA.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphChaosBolt_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphChaosBolt = chbGlyphChaosBolt.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphLifeTap_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphLifeTap = chbGlyphLifeTap.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphMetamorphosis_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphMetamorphosis = chbGlyphMetamorphosis.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphHaunt_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphHaunt = chbGlyphHaunt.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphIncinerate_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphIncinerate = chbGlyphIncinerate.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphShadowburn_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphShadowburn = chbGlyphShadowburn.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbImmoAura_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.UseImmoAura = chbImmoAura.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void tbox1_dclick(object sender, MouseEventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                textBox1.Text = calcOpts.castseq;
            }
        }
    }
    [Serializable]
	public class CalculationOptionsWarlock : ICalculationOptionBase
	{
        public bool GetGlyphByName(string name)
        {
            Type t = typeof(CalculationOptionsWarlock);
            return (bool)t.GetProperty(name).GetValue(this, null);
        }

        public void SetGlyphByName(string name, bool value)
        {
            Type t = typeof(CalculationOptionsWarlock);
            t.GetProperty(name).SetValue(this, value, null);
        }

        public String castseq { get; set; }

        public int TargetLevel { get; set; }
        public int AffEffectsNumber { get; set; }
        public float FightLength { get; set; }
        public float FSRRatio { get; set; }
        public float Delay { get; set; }
        public float Replenishment { get; set; }
        public float JoW { get; set; }
        public float LTUsePercent { get; set; }
        public String Pet { get; set; }
        public bool UseInfernal { get; set; }
        public bool UseImmoAura { get; set; }
        public bool GlyphChaosBolt { get; set; }
        public bool GlyphConflag { get; set; }
        public bool GlyphCorruption { get; set; }
        public bool GlyphCoA { get; set; }
        public bool GlyphFelguard { get; set; }
        public bool GlyphHaunt { get; set; }
        public bool GlyphImmolate { get; set; }
        public bool GlyphImp { get; set; }
        public bool GlyphIncinerate { get; set; }
        public bool GlyphLifeTap { get; set; }
        public bool GlyphMetamorphosis { get; set; }
        public bool GlyphSearingPain { get; set; }
        public bool GlyphSB { get; set; }
        public bool GlyphShadowburn { get; set; }
        public bool GlyphSiphonLife { get; set; }
        public bool GlyphUA { get; set; }

        public List<string> SpellPriority { get; set; }

        private static readonly List<int> targetHit = new List<int>() {100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39};
        public int TargetHit { get { return targetHit[TargetLevel]; } }

        private static readonly List<int> manaAmt = new List<int>() { 0, 1800, 2200, 2400, 4300 };
        public int ManaPot { get; set; }
        public int ManaAmt { get { return manaAmt[ManaPot]; } }

        public CalculationOptionsWarlock()
        {
            TargetLevel = 3;
            FightLength = 5f;
            FSRRatio = 100f;
            Delay = 100f;
            Replenishment = 100f;
            JoW = 100f;
            Pet = "None";

            SpellPriority = null;
            ManaPot = 4;
        }

        public string GetXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsWarlock));
            StringBuilder xml = new StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
	}
}