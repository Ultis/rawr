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
            if (Properties.Settings.Default.MaxThreads < 1)
            {
                Properties.Settings.Default.MaxThreads = 2;
                Properties.Settings.Default.Save();
            }
            numericUpDownMaxThreads.Value = Properties.Settings.Default.MaxThreads;
            checkBoxDebugCooldownSegmentation.Checked = Properties.Settings.Default.DebugSegmentation;
		}

        private bool loading = false;

        private static CooldownRestrictionsForm cooldownRestrictions;

		protected override void LoadCalculationOptions()
		{
            //if (Character.MageTalents == null) Character.MageTalents = new MageTalents();
            if (Character.CalculationOptions == null) Character.CalculationOptions = new CalculationOptionsMage(Character);
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            if (calculationOptions.ArcaneResist > 0 && calculationOptions.ArcaneResist <= 1)
            {
                if (calculationOptions.ArcaneResist == 1) calculationOptions.ArcaneResist = -1;
                else calculationOptions.ArcaneResist *= 400;
            }
            if (calculationOptions.FireResist > 0 && calculationOptions.FireResist <= 1)
            {
                if (calculationOptions.FireResist == 1) calculationOptions.FireResist = -1;
                else calculationOptions.FireResist *= 400;
            }
            if (calculationOptions.FrostResist > 0 && calculationOptions.FrostResist <= 1)
            {
                if (calculationOptions.FrostResist == 1) calculationOptions.FrostResist = -1;
                else calculationOptions.FrostResist *= 400;
            }
            if (calculationOptions.ShadowResist > 0 && calculationOptions.ShadowResist <= 1)
            {
                if (calculationOptions.ShadowResist == 1) calculationOptions.ShadowResist = -1;
                else calculationOptions.ShadowResist *= 400;
            }
            if (calculationOptions.NatureResist > 0 && calculationOptions.NatureResist <= 1)
            {
                if (calculationOptions.NatureResist == 1) calculationOptions.NatureResist = -1;
                else calculationOptions.NatureResist *= 400;
            }
            if (calculationOptions.HolyResist > 0 && calculationOptions.HolyResist <= 1)
            {
                if (calculationOptions.HolyResist == 1) calculationOptions.HolyResist = -1;
                else calculationOptions.HolyResist *= 400;
            }

            loading = true;
            calculationOptionsMageBindingSource.DataSource = calculationOptions;

            //BindingSource bs = calculationOptionsMageBindingSource;
            //bs.AddingNew += (s, ev) => System.Diagnostics.Trace.WriteLine("AddingNew");
            //bs.BindingComplete += (s, ev) => System.Diagnostics.Trace.WriteLine("BindingComplete");
            //bs.CurrentChanged += (s, ev) => System.Diagnostics.Trace.WriteLine("CurrentChanged");
            //bs.CurrentItemChanged += (s, ev) => System.Diagnostics.Trace.WriteLine("CurrentItemChanged");
            //bs.DataError += (s, ev) => System.Diagnostics.Trace.WriteLine("DataError");
            //bs.DataMemberChanged += (s, ev) => System.Diagnostics.Trace.WriteLine("DataMemberChanged");
            //bs.DataSourceChanged += (s, ev) => System.Diagnostics.Trace.WriteLine("DataSourceChanged");
            //bs.ListChanged += (s, ev) => System.Diagnostics.Trace.WriteLine("ListChanged");
            //bs.PositionChanged += (s, ev) => System.Diagnostics.Trace.WriteLine("PositionChanged");

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
            Properties.Settings.Default.DebugSegmentation = checkBoxDebugCooldownSegmentation.Checked;
            Properties.Settings.Default.Save();
            CharacterCalculationsMage.DebugCooldownSegmentation = checkBoxDebugCooldownSegmentation.Checked;
            if (Character != null) Character.OnCalculationsInvalidated();
        }

        private void buttonComputeOptimalArcaneCycles_Click(object sender, EventArgs e)
        {
            string armor = "Molten Armor";
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
            Solver solver = new Solver(Character, calculationOptions, false, false, 0, armor, false, false, false, false);
            Stats rawStats;
            Stats baseStats;
            CharacterCalculationsMage calculationResult = solver.InitializeCalculationResult(null, calculations, out rawStats, out baseStats);
            CastingState baseState = new CastingState(calculationResult, 0, false);
            CastingState apState = new CastingState(calculationResult, (int)StandardEffect.ArcanePower, false);

            Cycle wand = null;
            if (Character.Ranged != null)
            {
                wand = new WandTemplate(calculationResult, (MagicSchool)Character.Ranged.Item.DamageType, Character.Ranged.Item.MinDamage, Character.Ranged.Item.MaxDamage, Character.Ranged.Item.Speed).GetSpell(baseState);
            }


            /*StringBuilder sb = new StringBuilder();

            sb.AppendLine("Optimal Cycle Palette:");
            sb.AppendLine("");
            sb.AppendLine("Cycle Code Legend: 0 = AB, 1 = ABar, 2 = AM");
            sb.AppendLine(@"State Descriptions: ABx,ABary,MBz+-
x = number of AB stacks
y = remaining cooldown on Arcane Barrage
z = remaining time on Missile Barrage
+ = Missile Barrage proc visible
- = Missile Barrage proc not visible
");
    
            //sb.AppendLine("Base:");

            //ComputeOptimalCycles(baseState, sb);

            //sb.AppendLine("");
            //sb.AppendLine("Arcane Power:");

            //ComputeOptimalCycles(apState, sb);

            //MessageBox.Show(sb.ToString());

            //sb.Length = 0;
            //sb.AppendLine("Extended Model");
            sb.AppendLine("Base:");

            ArcaneCycleGenerator generator = new ArcaneCycleGenerator(baseState);

            sb.AppendLine("");
            for (int i = 0; i < generator.ControlOptions.Length; i++)
            {
                sb.AppendLine(i + ": " + generator.StateList[Array.IndexOf(generator.ControlIndex, i)]);
            }
            sb.AppendLine("");

            foreach (Cycle cycle in generator.Analyze(baseState, wand))
            {
                sb.Append(cycle.Name + ": " + cycle.DamagePerSecond + " dps, " + cycle.ManaPerSecond + " mps\r\n");
            }

            sb.AppendLine("");
            sb.AppendLine("Arcane Power:");

            generator = new ArcaneCycleGenerator(apState);

            sb.AppendLine("");
            for (int i = 0; i < generator.ControlOptions.Length; i++)
            {
                sb.AppendLine(i + ": " + generator.StateList[Array.IndexOf(generator.ControlIndex, i)]);
            }
            sb.AppendLine("");

            foreach (Cycle cycle in generator.Analyze(apState, wand))
            {
                sb.Append(cycle.Name + ": " + cycle.DamagePerSecond + " dps, " + cycle.ManaPerSecond + " mps\r\n");
            }

            MessageBox.Show(sb.ToString());*/

            ArcaneCycleGenerator generator = new ArcaneCycleGenerator(baseState);
            CycleAnalyzer analyzer = new CycleAnalyzer(baseState, generator, wand);
            analyzer.Show();
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

        private void numericUpDownMaxThreads_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MaxThreads = (int)numericUpDownMaxThreads.Value;
            Properties.Settings.Default.Save();
            ArrayPool.MaximumPoolSize = (int)numericUpDownMaxThreads.Value;
        }

        private void buttonEditTalentScores_Click(object sender, EventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            if (calculationOptions.TalentScore == null || calculationOptions.TalentScore.Length != Character.MageTalents.Data.Length)
            {
                calculationOptions.TalentScore = new float[Character.MageTalents.Data.Length];
            }
            TalentScoreForm form = new TalentScoreForm(calculationOptions.TalentScore);
            form.ShowDialog(this);
        }

        private void buttonComputeOptimalFrostCycles_Click(object sender, EventArgs e)
        {
            string armor = "Molten Armor";
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
            Solver solver = new Solver(Character, calculationOptions, false, false, 0, armor, false, false, false, false);
            Stats rawStats;
            Stats baseStats;
            CharacterCalculationsMage calculationResult = solver.InitializeCalculationResult(null, calculations, out rawStats, out baseStats);
            CastingState baseState = new CastingState(calculationResult, 0, false);

            Cycle wand = null;
            if (Character.Ranged != null)
            {
                wand = new WandTemplate(calculationResult, (MagicSchool)Character.Ranged.Item.DamageType, Character.Ranged.Item.MinDamage, Character.Ranged.Item.MaxDamage, Character.Ranged.Item.Speed).GetSpell(baseState);
            }


            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Optimal Cycle Palette:");
            sb.AppendLine("");
            sb.AppendLine("Cycle Code Legend: 0 = FrB, 1 = IL, 2 = FB");
            sb.AppendLine(@"State Descriptions: BFx+-,FOFy+-(z)
x = remaining time on Brain Freeze
+ = Brain Freeze proc visible
- = Brain Freeze proc not visible
y = visible count on Fingers of Frost
+ = ghost Fingers of Frost charge for instant available
- = ghost Fingers of Frost charge for instant not available
z = actual count on Fingers of Frost
");

            sb.AppendLine("Base:");

            FrostCycleGenerator generator = new FrostCycleGenerator(baseState);

            sb.AppendLine("");
            for (int i = 0; i < generator.ControlOptions.Length; i++)
            {
                sb.AppendLine(i + ": " + generator.StateList[Array.IndexOf(generator.ControlIndex, i)]);
            }
            sb.AppendLine("");

            foreach (Cycle cycle in generator.Analyze(baseState, wand))
            {
                sb.Append(cycle.Name + ": " + cycle.DamagePerSecond + " dps, " + cycle.ManaPerSecond + " mps\r\n");
            }

            MessageBox.Show(sb.ToString());
        }

        private void buttonHotStreakUtilization_Click(object sender, EventArgs e)
        {
            string armor = "Molten Armor";
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
            Solver solver = new Solver(Character, calculationOptions, false, false, 0, armor, false, false, false, false);
            Stats rawStats;
            Stats baseStats;
            CharacterCalculationsMage calculationResult = solver.InitializeCalculationResult(null, calculations, out rawStats, out baseStats);
            calculationResult.NeedsDisplayCalculations = true;
            CastingState baseState = new CastingState(calculationResult, 0, false);

            FireCycleGenerator generator = new FireCycleGenerator(baseState);            

            GenericCycle c1 = new GenericCycle("test", baseState, generator.StateList, true);
            Cycle c2 = baseState.GetCycle(CycleId.FBLBPyro);

            Dictionary<string, SpellContribution> dict1 = new Dictionary<string,SpellContribution>();
            Dictionary<string, SpellContribution> dict2 = new Dictionary<string,SpellContribution>();
            c1.AddDamageContribution(dict1, 1.0f);
            c2.AddDamageContribution(dict2, 1.0f);

            float predicted = dict2["Pyroblast"].Hits / dict2["Fireball"].Hits;
            float actual =  dict1["Pyroblast"].Hits / dict1["Fireball"].Hits;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Pyro/Nuke Ratio:");
            sb.AppendLine();
            sb.AppendLine("Approximation Model: " + predicted);
            sb.AppendLine("Exact Model: " + actual);
            sb.AppendLine();
            // predicted = raw * (1 - wastedold)
            // actual = raw * (1 - wasted)
            // wasted = 1 - actual / predicted * (1 - wastedold)
            sb.AppendLine("Predicted Wasted Hot Streaks: " + (1 - actual / predicted));

            MessageBox.Show(sb.ToString());
        }
	}
}
