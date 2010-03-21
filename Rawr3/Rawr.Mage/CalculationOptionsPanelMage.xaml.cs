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
using System.ComponentModel;
using System.Text;

namespace Rawr.Mage
{
    public partial class CalculationOptionsPanelMage : UserControl, ICalculationOptionsPanel
    {
        public CalculationOptionsPanelMage()
        {
            InitializeComponent();
        }

        public UserControl PanelControl { get { return this; } }

        private Character character;
        public Character Character
        {
            get
            {
                return character;
            }
            set
            {
                if (character != null && character.CalculationOptions != null && character.CalculationOptions is CalculationOptionsMage)
                {
                    ((CalculationOptionsMage)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(CalculationOptionsPanelMage_PropertyChanged);
                }

                character = value;
                if (character.CalculationOptions == null) character.CalculationOptions = new CalculationOptionsMage(character);
                LayoutRoot.DataContext = Character.CalculationOptions;

                ((CalculationOptionsMage)character.CalculationOptions).PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelMage_PropertyChanged);

            }
        }

        void CalculationOptionsPanelMage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }

        private void CooldownEditor_Click(object sender, RoutedEventArgs e)
        {
            //if (cooldownRestrictions == null || cooldownRestrictions.IsDisposed)
            //{
            //    cooldownRestrictions = new CooldownRestrictionsForm();
            //    cooldownRestrictions.Character = Character;
            //    //cooldownRestrictions.bindingSourceCalculationOptions.DataSource = calculationOptions;
            //}
            //cooldownRestrictions.Show();
            //cooldownRestrictions.BringToFront();
        }

        private void AdvancedSolverLog_Click(object sender, RoutedEventArgs e)
        {
            //SolverLogForm.Instance.Show();
            //SolverLogForm.Instance.BringToFront();
        }

        private void HotStreakUtilization_Click(object sender, RoutedEventArgs e)
        {
            string armor = "Molten Armor";
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
            Solver solver = new Solver(Character, calculationOptions, false, false, 0, armor, false, false, false, false);
            Stats rawStats;
            Stats baseStats;
            CharacterCalculationsMage calculationResult = solver.InitializeCalculationResult(null, calculations, out rawStats, out baseStats);
            calculationResult.NeedsDisplayCalculations = true;
            CastingState baseState = new CastingState(calculationResult, 0, false, 0);

            FireCycleGenerator generator = new FireCycleGenerator(baseState);

            GenericCycle c1 = new GenericCycle("test", baseState, generator.StateList, true);
            Cycle c2 = baseState.GetCycle(CycleId.FBLBPyro);

            Dictionary<string, SpellContribution> dict1 = new Dictionary<string, SpellContribution>();
            Dictionary<string, SpellContribution> dict2 = new Dictionary<string, SpellContribution>();
            c1.AddDamageContribution(dict1, 1.0f);
            c2.AddDamageContribution(dict2, 1.0f);

            float predicted = dict2["Pyroblast"].Hits / dict2["Fireball"].Hits;
            float actual = dict1["Pyroblast"].Hits / dict1["Fireball"].Hits;

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

        private void CalculationTiming_Click(object sender, RoutedEventArgs e)
        {
            CalculationsMage calculations = (CalculationsMage)Calculations.Instance;
            Character character = Character;
#if SILVERLIGHT
            DateTime start = DateTime.Now;
#else
            System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();
            clock.Start();
#endif
            for (int i = 0; i < 10000; i++)
            {
                calculations.GetCharacterCalculations(character);
            }
#if SILVERLIGHT
            MessageBox.Show("Calculating 10000 characters takes " + DateTime.Now.Subtract(start).TotalSeconds + " seconds.");
#else
            clock.Stop();
            MessageBox.Show("Calculating 10000 characters takes " + clock.Elapsed.TotalSeconds + " seconds.");
#endif
        }

        private void CycleAnalyzer_Click(object sender, RoutedEventArgs e)
        {
            //CycleAnalyzer analyzer = new CycleAnalyzer(Character);
            //analyzer.Show();
        }

        private void CustomSpellMix_Click(object sender, RoutedEventArgs e)
        {
            CalculationOptionsMage calculationOptions = Character.CalculationOptions as CalculationOptionsMage;
            if (calculationOptions.CustomSpellMix == null) calculationOptions.CustomSpellMix = new List<SpellWeight>();
            CustomSpellMixDialog dialog = new CustomSpellMixDialog(calculationOptions.CustomSpellMix);
            dialog.Show();
        }
    }
}
