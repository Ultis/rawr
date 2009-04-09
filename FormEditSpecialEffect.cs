using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
    public partial class FormEditSpecialEffect : Form
    {

        public Stats Stats { get; set; }
        public Trigger Trigger { get; set; }
        public float Duration { get; set; }
        public float Cooldown { get; set; }
        public bool UsesPPM { get; set; }
        public float Chance { get; set; }
        public int Stacks { get; set; }

        private bool loading;

        public FormEditSpecialEffect()
            : this(new Stats(), Trigger.MeleeHit, 20f, 120f, 1f, 1)
        {
            Text = "Add Special Effect";
        }

        public FormEditSpecialEffect(Stats stats, Trigger trigger, float duration, float cooldown, float chance, int stacks)
        {
            loading = true;
            Stats = stats.Clone();
            Trigger = trigger;
            Duration = duration;
            Cooldown = cooldown;
            Stacks = stacks;

            if (chance < 0)
            {
                Chance = -chance;
                UsesPPM = true;
            }
            else
            {
                Chance = chance * 100f;
                UsesPPM = false;
            }
            
            InitializeComponent();

            propertyGridStats.SelectedObject = Stats;
            cmbTrigger.DataSource = Enum.GetNames(typeof(Trigger));
            cmbTrigger.SelectedIndex = (int)Trigger;
            
            nudDuration.DataBindings.Add("Value", this, "Duration");
            nudCooldown.DataBindings.Add("Value", this, "Cooldown");
            nudChance.DataBindings.Add("Value", this, "Chance");
            nudStacks.DataBindings.Add("Value", this, "Stacks");

            if (UsesPPM) cmbPPM.SelectedIndex = 1;
            else cmbPPM.SelectedIndex = 0;

            loading = false;
        }

        private void butOkay_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmbTrigger_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                Trigger = (Trigger)Enum.Parse(typeof(Trigger), cmbTrigger.Text);
            }
        }

        private void cmbPPM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                if (cmbPPM.Text == "PPM") UsesPPM = true;
                else UsesPPM = false;
            }
        }

    }
}
