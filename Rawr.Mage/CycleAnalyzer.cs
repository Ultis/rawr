using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Mage
{
    public partial class CycleAnalyzer : Form
    {
        CycleGenerator generator;
        CastingState castingState;
        Cycle wand;

        public CycleAnalyzer(CastingState castingState, CycleGenerator generator, Cycle wand)
        {
            InitializeComponent();

            this.castingState = castingState;
            this.generator = generator;
            this.wand = wand;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(generator.StateDescription);

            sb.AppendLine("");
            for (int i = 0; i < generator.ControlOptions.Length; i++)
            {
                sb.AppendLine(i + ": " + generator.StateList[Array.IndexOf(generator.ControlIndex, i)]);
            }

            textBoxDescription.Text = sb.ToString();
            textBoxControlString.Text = new string('0', generator.ControlOptions.Length);

            //textBoxControlString.SelectAll();
            textBoxControlString.Focus();

            buttonCalculate_Click(null, EventArgs.Empty);
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            string name = textBoxControlString.Text;
            if (name.Length != generator.ControlOptions.Length) return;

            for (int i = 0; i < generator.ControlOptions.Length; i++)
            {
                generator.ControlValue[i] = int.Parse(name[i].ToString());
            }

            GenericCycle generic = new GenericCycle(name, castingState, generator.StateList, true);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(generic.DamagePerSecond + " Dps");
            sb.AppendLine(generic.ManaPerSecond + " Mps");
            sb.AppendLine(generic.ThreatPerSecond + " Tps");

            sb.AppendLine();

            sb.AppendLine(generic.SpellDistribution);

            textBoxResult.Text = sb.ToString();
        }

        private void buttonOptimal_Click(object sender, EventArgs e)
        {
            if (buttonOptimal.Text == "Cancel")
            {
                backgroundWorker.CancelAsync();
            }
            else
            {
                buttonOptimal.Text = "Cancel";
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = generator.Analyze(castingState, wand, backgroundWorker);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Cycle cycle in (List<Cycle>)e.Result)
                {
                    sb.Append(cycle.Name + ": " + cycle.DamagePerSecond + " dps, " + cycle.ManaPerSecond + " mps\r\n");
                }
                textBoxResult.Text = sb.ToString();
            }
            else
            {
                textBoxResult.Text = "";
            }
            buttonOptimal.Text = "Optimal";
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusLabel.Text = (string)e.UserState;
            statusProgressBar.Value = e.ProgressPercentage;
        }
    }
}
