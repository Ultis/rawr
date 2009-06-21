using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using Rawr.Properties;

namespace Rawr.Silverlight
{
    public partial class OptionsDialog : ChildWindow
    {
        public OptionsDialog()
        {
            InitializeComponent();

            WarningsCheck.IsChecked = OptimizerSettings.Default.WarningsEnabled;
            TemplateGemsCheck.IsChecked = OptimizerSettings.Default.TemplateGemsEnabled;
            switch (OptimizerSettings.Default.OptimizationMethod)
            {
                case OptimizationMethod.GeneticAlgorithm:
                    OptimizationMethodCombo.SelectedIndex = 0;
                    break;
                case OptimizationMethod.SimulatedAnnealing:
                    OptimizationMethodCombo.SelectedIndex = 1;
                    break;
            }
            switch (OptimizerSettings.Default.GreedyOptimizationMethod)
            {
                case GreedyOptimizationMethod.AllCombinations:
                    GreedyMethodCombo.SelectedIndex = 0;
                    break;
                case GreedyOptimizationMethod.SingleChanges:
                    GreedyMethodCombo.SelectedIndex = 1;
                    break;
                case GreedyOptimizationMethod.GreedyBest:
                    GreedyMethodCombo.SelectedIndex = 2;
                    break;
            }

            ProcEffectModeCombo.SelectedIndex = GeneralSettings.Default.ProcEffectMode;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            OptimizerSettings.Default.TemplateGemsEnabled = TemplateGemsCheck.IsChecked.GetValueOrDefault();
            OptimizerSettings.Default.WarningsEnabled = WarningsCheck.IsChecked.GetValueOrDefault();
            switch (OptimizationMethodCombo.SelectedIndex)
            {
                case 0:
                    OptimizerSettings.Default.OptimizationMethod = OptimizationMethod.GeneticAlgorithm;
                    break;
                case 1:
                    OptimizerSettings.Default.OptimizationMethod = OptimizationMethod.SimulatedAnnealing;
                    break;
            }
            switch (GreedyMethodCombo.SelectedIndex)
            {
                case 0:
                    OptimizerSettings.Default.GreedyOptimizationMethod = GreedyOptimizationMethod.AllCombinations;
                    break;
                case 1:
                    OptimizerSettings.Default.GreedyOptimizationMethod = GreedyOptimizationMethod.SingleChanges;
                    break;
                case 2:
                    OptimizerSettings.Default.GreedyOptimizationMethod = GreedyOptimizationMethod.GreedyBest;
                    break;
            }
            GeneralSettings.Default.ProcEffectMode = ProcEffectModeCombo.SelectedIndex;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

