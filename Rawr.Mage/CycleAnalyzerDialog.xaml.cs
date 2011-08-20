using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text;
using System.ComponentModel;

namespace Rawr.Mage
{
    public partial class CycleAnalyzerDialog : ChildWindow
    {
        public CycleAnalyzerDialog(Character character)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            InitializeComponent();

            this.character = character;
            CycleGeneratorComboBox.SelectedIndex = 0;
        }

        Character character;
        CycleGenerator generator;
        CastingState castingState;
        Cycle wand;
        BackgroundWorker backgroundWorker;

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = generator.Analyze(castingState, wand, backgroundWorker);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                StringBuilder sb = new StringBuilder();
                Cycle lastCycle = null;
                foreach (Cycle cycle in (List<Cycle>)e.Result)
                {
                    sb.Append(generator.ConvertCycleNameInternalToEasy(cycle.Name) + ": " + cycle.DamagePerSecond + " dps, " + cycle.ManaPerSecond + " mps");
                    if (lastCycle != null)
                    {
                        sb.Append(", " + (cycle.DamagePerSecond - lastCycle.DamagePerSecond) / (cycle.ManaPerSecond - lastCycle.ManaPerSecond) + " dpm tradeoff");
                    }
                    sb.AppendLine();
                    lastCycle = cycle;
                }
                Result.Text = sb.ToString();
            }
            else
            {
                Result.Text = "";
            }
            Optimal.Content = "Optimal";
            ProgressLabel.Text = "";
            ProgressBar.Value = 0;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressLabel.Text = (string)e.UserState;
            ProgressBar.Value = e.ProgressPercentage;
        }
 
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (castingState == null || generator == null)
            {
                return;
            }
            string name = generator.ConvertCycleNameEasyToInternal(ControlString.Text);
            if (name.Length != generator.ControlOptions.Length) return;

            for (int i = 0; i < generator.ControlOptions.Length; i++)
            {
                generator.ControlValue[i] = int.Parse(name[i].ToString());
            }

            try
            {
                GenericCycle generic = new GenericCycle(name, castingState, generator.StateList, true);

                StringBuilder sb = new StringBuilder();

                sb.AppendLine(generic.DamagePerSecond + " Dps");
                sb.AppendLine(generic.ManaPerSecond + " Mps");
                sb.AppendLine(generic.ThreatPerSecond + " Tps");
                sb.AppendLine(generic.DpsPerSpellPower + " Dps per Spell Power");
                sb.AppendLine(generic.DpsPerMastery + " Dps per Mastery");
                sb.AppendLine((generic.DpsPerCrit / 100) + " Dps per Crit");
                sb.AppendLine((generic.CastProcs / generic.CastTime) + " Cast Procs / Sec");
                sb.AppendLine((generic.HitProcs / generic.CastTime) + " Hit Procs / Sec");
                sb.AppendLine((generic.CritProcs / generic.CastTime) + " Crit Procs / Sec");
                sb.AppendLine((generic.DamageProcs / generic.CastTime) + " Damage Procs / Sec");
                sb.AppendLine((generic.DotProcs / generic.CastTime) + " Dot Procs / Sec");

                sb.AppendLine();

                sb.AppendLine(generic.SpellDistribution);

                Result.Text = sb.ToString();
            }
            catch (OutOfMemoryException /*ex*/)
            {
                Result.Text = "State Space too complex to solve, please select a different cycle solver.";
            }
        }

        private void Optimal_Click(object sender, RoutedEventArgs e)
        {
            if (castingState == null || generator == null)
            {
                return;
            }
            if ((string)Optimal.Content == "Cancel")
            {
                backgroundWorker.CancelAsync();
            }
            else
            {
                Optimal.Content = "Cancel";
                backgroundWorker.RunWorkerAsync();
            }
        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                backgroundWorker.DoWork -= new DoWorkEventHandler(backgroundWorker_DoWork);
                backgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                backgroundWorker.ProgressChanged -= new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            }        
        }

        private void CycleGeneratorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string armor;
            Solver solver;
            CalculationOptionsMage calculationOptions = character.CalculationOptions as CalculationOptionsMage;
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;

            switch ((string)((ComboBoxItem)CycleGeneratorComboBox.SelectedItem).Content)
            {
                case "Arcane":
                default:
                    armor = "Mage Armor";
                    solver = new Solver(character, calculationOptions, false, false, false, 0, armor, false, false, false, false, true, false, false);
                    solver.Initialize(null);
                    castingState = new CastingState(solver, 0, false, 0);
                    generator = new ArcaneCycleGeneratorBeta(castingState, true, false, false, false);
                    break;
                case "Arcane Hyper Regen":
                    armor = "Mage Armor";
                    solver = new Solver(character, calculationOptions, false, false, false, 0, armor, false, false, false, false, true, false, false);
                    solver.Initialize(null);
                    castingState = new CastingState(solver, 0, false, 0);
                    generator = new ArcaneCycleGeneratorBeta(castingState, true, false, false, true);
                    break;
                case "Arcane AOE":
                    armor = "Mage Armor";
                    solver = new Solver(character, calculationOptions, false, false, false, 0, armor, false, false, false, false, true, false, false);
                    solver.Initialize(null);
                    castingState = new CastingState(solver, 0, false, 0);
                    generator = new ArcaneAOECycleGenerator(castingState, true, false, false);
                    break;
                case "Frost":
                    armor = "Molten Armor";
                    solver = new Solver(character, calculationOptions, false, false, false, 0, armor, false, false, false, false, true, false, false);
                    solver.Initialize(null);
                    castingState = new CastingState(solver, 0, false, 0);
                    generator = new FrostCycleGeneratorBeta(castingState, false, 0.0f, false, 0.0f);
                    break;
                case "Frost+Deep Freeze":
                    armor = "Molten Armor";
                    solver = new Solver(character, calculationOptions, false, false, false, 0, armor, false, false, false, false, true, false, false);
                    solver.Initialize(null);
                    castingState = new CastingState(solver, 0, false, 0);
                    generator = new FrostCycleGeneratorBeta(castingState, true, 30.0f, false, 0.0f);
                    break;
                case "Frost+Freeze":
                    armor = "Molten Armor";
                    solver = new Solver(character, calculationOptions, false, false, false, 0, armor, false, false, false, false, true, false, false);
                    solver.Initialize(null);
                    castingState = new CastingState(solver, 0, false, 0);
                    generator = new FrostCycleGeneratorBeta(castingState, false, 0.0f, true, 25.0f);
                    break;
                case "Frost+Freeze+Deep Freeze":
                    armor = "Molten Armor";
                    solver = new Solver(character, calculationOptions, false, false, false, 0, armor, false, false, false, false, true, false, false);
                    solver.Initialize(null);
                    castingState = new CastingState(solver, 0, false, 0);
                    generator = new FrostCycleGeneratorBeta(castingState, true, 30.0f, true, 25.0f);
                    break;
            }

            if (castingState == null || generator == null)
            {
                return;
            }

            if (character.Ranged != null)
            {
                wand = new WandTemplate(solver, (MagicSchool)character.Ranged.Item.DamageType, character.Ranged.Item.MinDamage, character.Ranged.Item.MaxDamage, character.Ranged.Item.Speed).GetSpell(castingState);
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(generator.StateDescription);

            sb.AppendLine();
            for (int i = 0; i < generator.ControlOptions.Length; i++)
            {
                sb.Append(i);
                sb.Append(": ");
                sb.Append(generator.StateList[Array.IndexOf(generator.ControlIndex, i)]);
                sb.Append(": ");
                List<int> keys = new List<int>();
                foreach (var kvp in generator.SpellMap[i])
                {
                    keys.Add(generator.SpellList.IndexOf(kvp.Key));
                }
                keys.Sort();
                foreach (var key in keys)
                {
                    sb.Append(key);
                    sb.Append("=");
                    sb.Append(generator.SpellList[key]);
                    sb.Append("  ");
                }
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.AppendLine("Transitions:");
            sb.AppendLine();

            for (int i = 0; i < generator.ControlOptions.Length; i++)
            {
                foreach (var kvp in generator.SpellMap[i])
                {
                    sb.Append(i);
                    sb.Append(": ");
                    sb.Append(kvp.Key);
                    sb.Append(" => ");

                    List<int> list = new List<int>();
                    for (int s = 0; s < generator.ControlIndex.Length; s++)
                    {
                        if (generator.ControlIndex[s] == i)
                        {
                            foreach (CycleControlledStateTransition transition in generator.StateList[s].Transitions)
                            {
                                string n;
                                if (transition.Spell != null)
                                {
                                    n = transition.Spell.Name;
                                }
                                else
                                {
                                    n = "Pause";
                                }
                                if (n == kvp.Key)
                                {
                                    int target = generator.ControlIndex[transition.TargetState.Index];
                                    if (!list.Contains(target))
                                    {
                                        list.Add(target);
                                    }
                                }
                            }
                        }
                    }

                    list.Sort();
                    sb.Append(string.Join(",", list));

                    sb.AppendLine();
                }
            }


            Description.Text = sb.ToString();
            ControlString.Text = generator.ConvertCycleNameInternalToEasy(new string('0', generator.ControlOptions.Length));

            //ControlString.SelectAll();
            ControlString.Focus();

            Calculate_Click(null, null);        
        }
    }
}

