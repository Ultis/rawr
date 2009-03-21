using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Rawr.Mage
{
	public partial class CalculationOptionsPanelMage : CalculationOptionsPanelBase
	{
		public CalculationOptionsPanelMage()
		{
			InitializeComponent();
            foreach (MIPMethod value in Enum.GetValues(typeof(MIPMethod)))
            {
                comboBoxMIPMethod.Items.Add(value);
            }
		}

        private bool loading = false;

        private static CooldownRestrictionsForm cooldownRestrictions;

		protected override void LoadCalculationOptions()
		{
            //if (Character.MageTalents == null) Character.MageTalents = new MageTalents();
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsMage(Character);
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;

            loading = true;
            calculationOptionsMageBindingSource.DataSource = calculationOptions;
            if (cooldownRestrictions != null && !cooldownRestrictions.IsDisposed)
            {
                cooldownRestrictions.Character = Character;
                //cooldownRestrictions.bindingSourceCalculationOptions.DataSource = calculationOptions;
            }

            loading = false;

            SolverLogForm.Instance.ToString(); // force create it on main thread
        }

        private void checkBoxSMP_CheckedChanged(object sender, EventArgs e)
        {
            //DisplaySMPWarning();
        }

        private void checkBoxSMPDisplay_CheckedChanged(object sender, EventArgs e)
        {
            //DisplaySMPWarning();
        }

        private void DisplaySMPWarning()
        {
            if (!Properties.Settings.Default.DisplayedSMPWarning)
            {
                MessageBox.Show("Rawr detected that this is the first time you are using Segmented Multi-Pass (SMP) algorithm or Sequence Reconstruction." + Environment.NewLine + Environment.NewLine + "Sequence Reconstruction will perform best in combination with SMP algorithm. Since SMP algorithm can be computationally very expensive it is recommended to use SMP for display only and not for item comparisons. In some situations with many available cooldowns the SMP algorithm might have difficulties finding a solution. In such a case it will indicate it exceeded its computation limit and display the last working solution. Sequence Reconstruction will attempt to convert the optimum spell cycles into a timed sequence, but at the moment it will not consider timing dependencies between mana consumables and cooldowns, which means that in certain situations the reconstructions provided might be of low quality.", "Rawr.Mage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Properties.Settings.Default.DisplayedSMPWarning = true;
                Properties.Settings.Default.Save();
            }
        }

        private void calculationOptionsMageBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            if (!loading) Character.OnCalculationsInvalidated();
        }

        private void buttonCustomSpellMix_Click(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            if (calculationOptions.CustomSpellMix == null) calculationOptions.CustomSpellMix = new List<SpellWeight>();
            CustomSpellMixForm form = new CustomSpellMixForm(calculationOptions.CustomSpellMix);
            form.ShowDialog(this);
        }

        private void checkBoxDebugCooldownSegmentation_CheckedChanged(object sender, EventArgs e)
        {
            CharacterCalculationsMage.DebugCooldownSegmentation = checkBoxDebugCooldownSegmentation.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void buttonComputeOptimalArcaneCycles_Click(object sender, EventArgs e)
        {
            string armor = "Molten Armor";
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
            Stats rawStats = calculations.GetRawStats(Character, null, calculationOptions, new List<Buff>(), armor);
            Stats characterStats = calculations.GetCharacterStats(Character, null, rawStats, calculationOptions);
            CharacterCalculationsMage calculationResult = new CharacterCalculationsMage();
            calculationResult.Calculations = calculations;
            calculationResult.BaseStats = characterStats;
            calculationResult.Character = Character;
            calculationResult.CalculationOptions = calculationOptions;
            CastingState baseState = new CastingState(calculationResult, characterStats, calculationOptions, armor, Character, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
            CastingState apState = new CastingState(calculationResult, characterStats, calculationOptions, armor, Character, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Optimal Cycle Palette:");
            sb.AppendLine("");
            sb.AppendLine("Cycle Code Legend:");
            sb.AppendLine(@"1) at 0 stack with ABar not on cooldown and you don't see MB do: 0 = AB, 1 = ABar, 2 = AM
2) at 1 stack if you don't see MB do: 0 = AB, 1 = ABar, 2 = AM
3) at 0 stack with ABar on cooldown if you don't see MB do: 0 = AB, 1 = AM
4) at 0 stack with ABar on cooldown if you see MB do: 0 = AB, 1 = AM
5) at 1 stack if you see MB do: 0 = AB, 1 = ABar, 2 = AM
6) at 2 stack if you don't see MB do: 0 = AB, 1 = ABar, 2 = AM
7) at 2 stack if you see MB do: 0 = AB, 1 = ABar, 2 = AM
8) at 3 stack if you don't see MB do: 0 = AB, 1 = ABar, 2 = AM
9) at 3 stack if you see MB do: 0 = AB, 1 = ABar, 2 = AM
10) at 0 stack with ABar not on cooldown and you see MB do: 0 = AB, 1 = ABar, 2 = AM
");
    
            sb.AppendLine("Base:");

            ComputeOptimalCycles(baseState, sb);

            sb.AppendLine("");
            sb.AppendLine("Arcane Power:");

            ComputeOptimalCycles(apState, sb);

            MessageBox.Show(sb.ToString());


            /*foreach (GenericArcane cycle in cycleDict.Values)
            {
                System.Diagnostics.Debug.Write(cycle.Name + ":\r\n" + cycle.DamagePerSecond + " dps\r\n" + cycle.ManaPerSecond + " mps\r\n" + cycle.SpellDistribution + "\r\n");
            }*/
        }

        private static StringBuilder ComputeOptimalCycles(CastingState baseState, StringBuilder sb)
        {
            Dictionary<string, GenericArcane> cycleDict = new Dictionary<string, GenericArcane>();
            double[] spellControl = new double[46];
            for (int control0 = 0; control0 < 3; control0++)
                for (int control1 = 0; control1 < 3; control1++)
                    for (int control2 = 0; control2 < 2; control2++)
                        for (int control3 = 0; control3 < 2; control3++)
                            for (int control4 = 0; control4 < 3; control4++)
                                for (int control5 = 0; control5 < 3; control5++)
                                    for (int control6 = 0; control6 < 3; control6++)
                                        for (int control7 = 0; control7 < 3; control7++)
                                            for (int control8 = 0; control8 < 3; control8++)
                                                for (int control9 = 0; control9 < 3; control9++)
                                                {
                                                    for (int i = 0; i < 28; i++) spellControl[i] = 0;
                                                    spellControl[0 + control0] = 1;
                                                    spellControl[3 + control1] = 1;
                                                    spellControl[6 + control2] = 1;
                                                    spellControl[8 + control3] = 1;
                                                    spellControl[10 + control4] = 1;
                                                    spellControl[13 + control5] = 1;
                                                    spellControl[16 + control6] = 1;
                                                    spellControl[19 + control7] = 1;
                                                    spellControl[22 + control8] = 1;
                                                    spellControl[25 + control9] = 1;
                                                    GenericArcane generic = new GenericArcane(control0.ToString() + control1.ToString() + control2.ToString() + control3.ToString() + control4.ToString() + control5.ToString() + control6.ToString() + control7.ToString() + control8.ToString() + control9.ToString(), baseState, spellControl[0], spellControl[1], spellControl[2], spellControl[3], spellControl[4], spellControl[5], spellControl[6], spellControl[7], spellControl[8], spellControl[9], spellControl[10], spellControl[11], spellControl[12], spellControl[13], spellControl[14], spellControl[15], spellControl[16], spellControl[17], spellControl[18], spellControl[19], spellControl[20], spellControl[21], spellControl[22], spellControl[23], spellControl[24], spellControl[25], spellControl[26], spellControl[27]);
                                                    if (!cycleDict.ContainsKey(generic.SpellDistribution))
                                                    {
                                                        cycleDict.Add(generic.SpellDistribution, generic);
                                                    }
                                                }

            List<GenericArcane> cyclePalette = new List<GenericArcane>();

            double maxdps = 0;
            GenericArcane maxdpsCycle = null;
            foreach (GenericArcane cycle in cycleDict.Values)
            {
                if (cycle.DamagePerSecond > maxdps)
                {
                    maxdpsCycle = cycle;
                    maxdps = cycle.DamagePerSecond;
                }
            }

            cyclePalette.Add(maxdpsCycle);

            GenericArcane mindpmCycle;
            do
            {
                GenericArcane highdpsCycle = cyclePalette[cyclePalette.Count - 1];
            RESTART:
                mindpmCycle = null;
                double mindpm = double.PositiveInfinity;
                foreach (GenericArcane cycle in cycleDict.Values)
                {
                    double dpm = (cycle.DamagePerSecond - highdpsCycle.DamagePerSecond) / (cycle.ManaPerSecond - highdpsCycle.ManaPerSecond);
                    if (dpm > 0 && dpm < mindpm && cycle.ManaPerSecond < highdpsCycle.ManaPerSecond)
                    {
                        mindpm = dpm;
                        mindpmCycle = cycle;
                    }
                }
                if (mindpmCycle != null)
                {
                    // validate cycle pair theory
                    foreach (GenericArcane cycle in cycleDict.Values)
                    {
                        double dpm = (cycle.DamagePerSecond - mindpmCycle.DamagePerSecond) / (cycle.ManaPerSecond - mindpmCycle.ManaPerSecond);
                        if (cycle != highdpsCycle && cycle.DamagePerSecond > mindpmCycle.DamagePerSecond && dpm > mindpm + 0.000001)
                        {
                            highdpsCycle = cycle;
                            goto RESTART;
                        }
                    }
                    cyclePalette.Add(mindpmCycle);
                }
            } while (mindpmCycle != null);


            foreach (GenericArcane cycle in cyclePalette)
            {
                sb.Append(cycle.Name + ": " + cycle.DamagePerSecond + " dps, " + cycle.ManaPerSecond + " mps\r\n");
                //System.Diagnostics.Debug.Write(cycle.Name + ": " + cycle.DamagePerSecond + " dps, " + cycle.ManaPerSecond + " mps\r\n");
            }
            return sb;
        }

        private void buttonCooldownRestrictionsEditor_Click(object sender, EventArgs e)
        {
            if (cooldownRestrictions == null || cooldownRestrictions.IsDisposed)
            {
                cooldownRestrictions = new CooldownRestrictionsForm();
                cooldownRestrictions.Character = Character;
                //cooldownRestrictions.bindingSourceCalculationOptions.DataSource = calculationOptions;
            }
            cooldownRestrictions.Show();
            cooldownRestrictions.BringToFront();
        }

        private void buttonAdvancedSolverLog_Click(object sender, EventArgs e)
        {
            SolverLogForm.Instance.Show();
            SolverLogForm.Instance.BringToFront();
        }
	}
}
