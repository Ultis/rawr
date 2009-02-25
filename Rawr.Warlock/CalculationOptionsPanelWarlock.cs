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

            trkSurvivability.Value = (int)calcOpts.Survivability;
            lblSurvivability.Text = trkSurvivability.Value + "% Focus on Survivability.";
            
            cbManaAmt.SelectedIndex = calcOpts.ManaPot;

            if (calcOpts.SpellPriority == null)
                calcOpts.SpellPriority = new List<string>() {"Shadow Bolt"};
            lsSpellPriopity.Items.Clear();
            lsSpellPriopity.Items.AddRange(calcOpts.SpellPriority.ToArray());
            
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.Survivability = trkSurvivability.Value;
                lblSurvivability.Text = trkSurvivability.Value + "% Focus on Survivability.";
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

        private void chbUseDoomguard_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.UseDoomguard = chbUseDoomguard.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbPetSacrificed_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.PetSacrificed = chbPetSacrificed.Checked;
                calcOpts.cleanBreak = chbPetSacrificed.Checked;
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

        private void chbGlyphShadowburn_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphShadowburn = chbGlyphShadowburn.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbGlyphSiphonLife_CheckedChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                CalculationOptionsWarlock calcOpts = Character.CalculationOptions as CalculationOptionsWarlock;
                calcOpts.GlyphSiphonLife = chbGlyphSiphonLife.Checked;
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
    }
    [Serializable]
	public class CalculationOptionsWarlock : ICalculationOptionBase
	{
        public int TargetLevel { get; set; }
        public int AffEffectsNumber { get; set; }
        public bool cleanBreak { get; set; }
        public float FightLength { get; set; }
        public float FSRRatio { get; set; }
        public float Delay { get; set; }
        public float Replenishment { get; set; }
        public float JoW { get; set; }
        public float LTUsePercent { get; set; }
        public float Survivability { get; set; }
        public String Pet { get; set; }
        public bool PetSacrificed { get; set; }
        public bool UseDoomguard { get; set; }
        public bool GlyphConflag { get; set; }
        public bool GlyphCorruption { get; set; }
        public bool GlyphCoA { get; set; }
        public bool GlyphFelguard { get; set; }
        public bool GlyphImmolate { get; set; }
        public bool GlyphImp { get; set; }
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
            Survivability = 2f;

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

/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Warlock
{
	public partial class CalculationOptionsPanelWarlock : CalculationOptionsPanelBase
	{
        private bool calculationSuspended = false;

		public CalculationOptionsPanelWarlock()
		{
			InitializeComponent();
        }

		protected override void LoadCalculationOptions()
		{
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsWarlock(Character);
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;

            calculationSuspended = true;

            textBoxLatency.Text = options.Latency.ToString();
            comboBoxTargetLevel.SelectedItem = options.TargetLevel.ToString();
            textBoxFightDuration.Text = options.FightDuration.ToString();
            textBoxDotGap.Text = options.DotGap.ToString();
            textBoxAfflictionDebuffs.Text = options.AfflictionDebuffs.ToString();
            comboBoxFillerSpell.SelectedIndex = (int)options.FillerSpell;
            comboBoxCastedCurse.SelectedIndex = (int)options.CastedCurse;
            checkBoxCastImmolate.Checked = options.CastImmolate;
            checkBoxCastCorruption.Checked = options.CastCorruption;
            checkBoxCastUnstableAffliction.Enabled = options.UnstableAffliction == 1;
            checkBoxCastUnstableAffliction.Checked = checkBoxCastUnstableAffliction.Enabled && options.CastUnstableAffliction;
            checkBoxCastSiphonLife.Enabled = options.SiphonLife == 1;
            checkBoxCastSiphonLife.Checked = checkBoxCastSiphonLife.Enabled && options.CastSiphonLife;
            checkBoxCastShadowburn.Enabled = options.Shadowburn == 1;
            checkBoxCastShadowburn.Checked = checkBoxCastShadowburn.Enabled && options.CastShadowburn;
            checkBoxCastConflagrate.Enabled = (options.Conflagrate == 1 && checkBoxCastImmolate.Checked);
            checkBoxCastConflagrate.Checked = checkBoxCastConflagrate.Enabled && options.CastConflagrate;
            if (options.SummonFelguard == 1 && !comboBoxPet.Items.Contains("Felguard"))
                comboBoxPet.Items.Add("Felguard");
            else if (options.SummonFelguard != 1 && comboBoxPet.Items.Contains("Felguard"))
                comboBoxPet.Items.Remove("Felguard");
            comboBoxPet.SelectedIndex = (int)options.Pet;
            checkBoxPetSacrificed.Enabled = options.DemonicSacrifice == 1;
            checkBoxPetSacrificed.Checked = checkBoxPetSacrificed.Enabled && options.PetSacrificed;

            calculationSuspended = false;
        }

        public void UpdateTalentOptions()
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;

            calculationSuspended = true;

            checkBoxCastUnstableAffliction.Enabled = options.UnstableAffliction == 1;
            checkBoxCastUnstableAffliction.Checked = checkBoxCastUnstableAffliction.Enabled && options.CastUnstableAffliction;

            checkBoxCastSiphonLife.Enabled = options.SiphonLife == 1;
            checkBoxCastSiphonLife.Checked = checkBoxCastSiphonLife.Enabled && options.CastSiphonLife;

            checkBoxCastShadowburn.Enabled = options.Shadowburn == 1;
            checkBoxCastShadowburn.Checked = checkBoxCastShadowburn.Enabled && options.CastShadowburn;

            checkBoxCastConflagrate.Enabled = options.Conflagrate == 1 && checkBoxCastImmolate.Checked;
            checkBoxCastConflagrate.Checked = checkBoxCastConflagrate.Enabled && options.CastConflagrate;

            checkBoxCastCorruption.Checked = options.CastCorruption;
            checkBoxCastImmolate.Checked = options.CastImmolate;
            comboBoxFillerSpell.SelectedIndex = (int)options.FillerSpell;

            if (options.SummonFelguard == 1 && !comboBoxPet.Items.Contains("Felguard"))
                comboBoxPet.Items.Add("Felguard");
            else if (options.SummonFelguard != 1 && comboBoxPet.Items.Contains("Felguard"))
                comboBoxPet.Items.Remove("Felguard");
            comboBoxPet.SelectedIndex = (int)options.Pet;

            checkBoxPetSacrificed.Enabled = options.DemonicSacrifice == 1;
            checkBoxPetSacrificed.Checked = checkBoxPetSacrificed.Enabled && options.PetSacrificed;

            calculationSuspended = false;
            Character.OnCalculationsInvalidated();
        }

        private void textBoxLatency_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if (float.TryParse(textBoxLatency.Text, out value))
            {
                options.Latency = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }   
	
		private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
                    CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
                    options.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
                    if (!calculationSuspended) Character.OnCalculationsInvalidated();
		}

        private void textBoxFightDuration_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if (float.TryParse(textBoxFightDuration.Text, out value))
            {
                options.FightDuration = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }

        private void textBoxDotGap_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            float value;
            if (float.TryParse(textBoxDotGap.Text, out value))
            {
                options.DotGap = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }

        private void textBoxAfflictionDebuffs_Leave(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            int value;
            if (int.TryParse(textBoxAfflictionDebuffs.Text, out value))
            {
                options.AfflictionDebuffs = value;
                if (!calculationSuspended) Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxFillerSpell_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.FillerSpell = (FillerSpell)(comboBoxFillerSpell.SelectedIndex);
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void comboBoxCastedCurse_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastedCurse = (CastedCurse)(comboBoxCastedCurse.SelectedIndex);
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastImmolate_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastImmolate = checkBoxCastImmolate.Checked;
            if (!options.CastImmolate)
                checkBoxCastConflagrate.Enabled = checkBoxCastConflagrate.Checked = false;
            else if(options.Conflagrate == 1)
                checkBoxCastConflagrate.Enabled = true;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastCorruption_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastCorruption = checkBoxCastCorruption.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastUnstableAffliction_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastUnstableAffliction = checkBoxCastUnstableAffliction.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastSiphonLife_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastSiphonLife = checkBoxCastSiphonLife.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastShadowburn_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastShadowburn = checkBoxCastShadowburn.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxCastConflagrate_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.CastConflagrate = checkBoxCastConflagrate.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void comboBoxPet_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.Pet = (Pet)(comboBoxPet.SelectedIndex);
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

        private void checkBoxPetSacrificed_CheckedChanged(object sender, EventArgs e)
        {
            CalculationOptionsWarlock options = Character.CalculationOptions as CalculationOptionsWarlock;
            options.PetSacrificed = checkBoxPetSacrificed.Checked;
            if (!calculationSuspended) Character.OnCalculationsInvalidated();
        }

	}
}
*/