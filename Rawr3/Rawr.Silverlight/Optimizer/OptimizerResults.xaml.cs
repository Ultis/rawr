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

namespace Rawr.Silverlight
{
    public partial class OptimizerResults : ChildWindow
    {
        public Character CurrentCharacter { get; private set; }
        public Character BestCharacter { get; private set; }

        public OptimizerResults(Character oldCharacter, Character newCharacter)
        {
            InitializeComponent();

            CurrentCharacter = oldCharacter;
            BestCharacter = newCharacter;

            int rows = 0;
            foreach (Character.CharacterSlot slot in EnumHelper.GetValues<Character.CharacterSlot>())
            {
                //if (!oldCharacter[slot].Equals(newCharacter[slot]))
                if (oldCharacter[slot] != null || newCharacter[slot] != null) {
                    ItemGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                    if (oldCharacter[slot] != null)
                    {
                        ItemDisplay oldItem = new ItemDisplay(oldCharacter[slot]);
                        oldItem.Style = Resources["ItemDisplayStyle"] as Style;
                        ItemGrid.Children.Add(oldItem);
                        Grid.SetRow(oldItem, rows);
                        Grid.SetColumn(oldItem, 0);
                    }

                    if (newCharacter[slot] != null)
                    {
                        ItemDisplay newItem = new ItemDisplay(newCharacter[slot]);
                        newItem.Style = Resources["ItemDisplayStyle"] as Style;
                        ItemGrid.Children.Add(newItem);
                        Grid.SetRow(newItem, rows);
                        Grid.SetColumn(newItem, 1);
                    }

                    rows++;
                }
            }

            CharacterCalculationsBase currentCalc = Calculations.GetCharacterCalculations(oldCharacter);
            CurrentScoreLabel.Text = string.Format("Current: {0}", currentCalc.OverallPoints);
            CurrentCalculations.SetCalculations(currentCalc.GetCharacterDisplayCalculationValues());

            CharacterCalculationsBase optimizedCalc = Calculations.GetCharacterCalculations(newCharacter);
            OptimizedScoreLabel.Text = string.Format("Optimized: {0}", optimizedCalc.OverallPoints);
            OptimizedCalculations.SetCalculations(optimizedCalc.GetCharacterDisplayCalculationValues());
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

