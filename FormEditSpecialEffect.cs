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
        public float Chance { get; set; }
        public int Stacks { get; set; }

        private bool loading;

        public FormEditSpecialEffect() : this(new Stats(), Trigger.MeleeHit, 20f, 120f, 1f, 1) { }

        public FormEditSpecialEffect(Stats stats, Trigger trigger, float duration, float cooldown, float chance, int stacks)
        {
            loading = true;
            Stats = stats;
            Trigger = trigger;
            Duration = duration;
            Cooldown = cooldown;
            Chance = chance;
            Stacks = stacks;
            
            InitializeComponent();

            propertyGridStats.SelectedObject = Stats;
            cmbTrigger.DataSource = Enum.GetNames(typeof(Trigger));
            cmbTrigger.SelectedIndex = (int)Trigger;
            
            nudDuration.DataBindings.Add("Value", this, "Duration");
            nudCooldown.DataBindings.Add("Value", this, "Cooldown");
            nudChance.DataBindings.Add("Value", this, "Chance");
            nudStacks.DataBindings.Add("Value", this, "Stacks");
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
    }
}
