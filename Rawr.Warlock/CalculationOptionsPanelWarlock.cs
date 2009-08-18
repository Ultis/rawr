using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace Rawr.Warlock {
    public partial class CalculationOptionsPanelWarlock : CalculationOptionsPanelBase {
        public CalculationOptionsPanelWarlock() {
            InitializeComponent();
        }
        private bool loading;
        protected override void LoadCalculationOptions() {
            loading = true;
            if (Character.CalculationOptions == null) { Character.CalculationOptions = new CalculationOptionsWarlock(); }

            CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;

            // Adding this switch to handle target level transition from relative to actual
            switch(calcOpts.TargetLevel){
                case 0:case 1:case 2:case 3: { calcOpts.TargetLevel += 80; break; }
                default: { break; } // Do nothing if it's already transitioned
            }
            cbTargetLevel.Text = calcOpts.TargetLevel.ToString();

            trkFightLength.Value = (int)calcOpts.FightLength;
            lblFightLength.Text = trkFightLength.Value + " minute fight.";


            trkDelay.Value = (int)calcOpts.Delay;
            lblDelay.Text = trkDelay.Value + "ms Game/Brain Latency";

            trkReplenishment.Value = (int)calcOpts.Replenishment;
            lblReplenishment.Text = trkReplenishment.Value + "% effect from Replenishment.";

            trkJoW.Value = (int)calcOpts.JoW;
            lblJoW.Text = trkJoW.Value + "% effect from JoW.";

            cbManaAmt.SelectedIndex = calcOpts.ManaPot;

            if (calcOpts.SpellPriority == null) { calcOpts.SpellPriority = new List<string>() { "Shadow Bolt" }; }
            lsSpellPriopity.Items.Clear();
            lsSpellPriopity.Items.AddRange(calcOpts.SpellPriority.ToArray());

            tbAffEffects.Text = calcOpts.AffEffectsNumber.ToString();

            cbPet.SelectedItem = calcOpts.Pet;

            chbUseInfernal.Checked = calcOpts.UseInfernal;
            chbImmoAura.Checked = calcOpts.UseImmoAura;

            loading = false;
        }
        private void cmbManaAmt_SelectedIndexChanged(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.ManaPot = cbManaAmt.SelectedIndex;
                Character.OnCalculationsInvalidated();
            }
        }
        private void bChangePriority_Click(object sender, EventArgs e) {
            CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
            SpellPriorityForm priority = new SpellPriorityForm(calcOpts.SpellPriority, lsSpellPriopity, Character);
            priority.Show();
        }
        private void cbTargetLevel_SelectedIndexChanged(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.TargetLevel = int.Parse(cbTargetLevel.Text);
                Character.OnCalculationsInvalidated();
            }
        }
        private void trkFightLength_Scroll(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.FightLength = trkFightLength.Value;
                lblFightLength.Text = trkFightLength.Value + " minute fight.";
                Character.OnCalculationsInvalidated();
            }
        }
        private void trkDelay_Scroll(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Delay = trkDelay.Value;
                lblDelay.Text = trkDelay.Value + "ms Game/Brain Latency";
                Character.OnCalculationsInvalidated();
            }
        }
        private void trkReplenishment_Scroll(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Replenishment = trkReplenishment.Value;
                lblReplenishment.Text = trkReplenishment.Value + "% effect from Replenishment.";
                Character.OnCalculationsInvalidated();
            }
        }
        private void trkJoW_Scroll(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.JoW = trkJoW.Value;
                lblJoW.Text = trkJoW.Value + "% effect from JoW.";
                Character.OnCalculationsInvalidated();
            }
        }
        private void cbPet_SelectedIndexChanged(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Pet = (String)cbPet.SelectedItem;
                Character.OnCalculationsInvalidated();
            }
        }
        private void chbUseInfernal_CheckedChanged(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.UseInfernal = chbUseInfernal.Checked;
                Character.OnCalculationsInvalidated();
            }
        }
        private void tbAffEffects_Changed(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.AffEffectsNumber = System.Convert.ToInt32(tbAffEffects.Text);
                Character.OnCalculationsInvalidated();
            }
        }
        private void tbAffEffects_KeyPress(object sender, KeyPressEventArgs e) {
            if (!loading) {
                if (char.IsNumber(e.KeyChar) == false && char.IsControl(e.KeyChar) == false) {
                    e.Handled = true;
                }
            }
        }
        private void chbImmoAura_CheckedChanged(object sender, EventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.UseImmoAura = chbImmoAura.Checked;
                Character.OnCalculationsInvalidated();
            }
        }
        private void tbox1_dclick(object sender, MouseEventArgs e) {
            if (!loading) {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                textBox1.Text = calcOpts.castseq;
            }
        }
    }
    [Serializable]
	public class CalculationOptionsWarlock : ICalculationOptionBase {
        public bool GetGlyphByName(string name) {
            Type t = typeof(CalculationOptionsWarlock);
            return (bool)t.GetProperty(name).GetValue(this, null);
        }
        public void SetGlyphByName(string name, bool value) {
            Type t = typeof(CalculationOptionsWarlock);
            t.GetProperty(name).SetValue(this, value, null);
        }
        public String castseq { get; set; }

        public int TargetLevel { get; set; }
        public int AffEffectsNumber { get; set; }
        public float FightLength { get; set; }
        public float Delay { get; set; }
        public float Replenishment { get; set; }
        public float JoW { get; set; }
        public float LTUsePercent { get; set; }
        public String Pet { get; set; }
        public bool UseInfernal { get; set; }
        public bool UseImmoAura { get; set; }

        public List<string> SpellPriority { get; set; }

        private static readonly List<int> targetHit = new List<int>() {100 - 4, 100 - 5, 100 - 6, 100 - 17, 100 - 28, 100 - 39};
        public int TargetHit { get { return targetHit[TargetLevel-80]; } }

        private static readonly List<int> manaAmt = new List<int>() { 0, 1800, 2200, 2400, 4300 };
        public int ManaPot { get; set; }
        public int ManaAmt { get { return manaAmt[ManaPot]; } }

        public CalculationOptionsWarlock() {
            TargetLevel = 83;
            FightLength = 5f;
           
            Delay = 100f;
            Replenishment = 100f;
            JoW = 100f;
            Pet = "None";

            SpellPriority = null;
            ManaPot = 4;
        }
        public string GetXml() {
            XmlSerializer serializer = new XmlSerializer(typeof(CalculationOptionsWarlock));
            StringBuilder xml = new StringBuilder();
            StringWriter writer = new StringWriter(xml);
            serializer.Serialize(writer, this);
            return xml.ToString();
        }
	}
}