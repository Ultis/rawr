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

namespace Rawr.UI
{
    public partial class OptimizerResults : ChildWindow
    {
        public Character CurrentCharacter { get; private set; }
        public Character BestCharacter { get; private set; }
        public bool WeWantToStoreIt = false;

        public OptimizerResults(Character oldCharacter, Character newCharacter, bool cancelOptimization)
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            if (cancelOptimization)
            {
                CancelButton.Content = "Cancel";
                BT_StoreIt.Content = "Continue";
            }

            CurrentCharacter = oldCharacter;
            BestCharacter = newCharacter;

            int rows = 0;
            for (int i = 0; i < Character.OptimizableSlotCount; i++)
            {
                CharacterSlot slot = (CharacterSlot)i;
                if ((oldCharacter[slot] == null && newCharacter[slot] != null) ||
                    (oldCharacter[slot] != null  && !oldCharacter[slot].Equals(newCharacter[slot])))
                {
                    // Testing if the ring/trinket items were just swapped and not actually different
                    if (slot == CharacterSlot.Finger1 || slot == CharacterSlot.Finger2)
                    {
                        ItemInstance old1 = oldCharacter[CharacterSlot.Finger1];
                        ItemInstance old2 = oldCharacter[CharacterSlot.Finger2];
                        ItemInstance new1 = newCharacter[CharacterSlot.Finger1];
                        ItemInstance new2 = newCharacter[CharacterSlot.Finger2];
                        if (((old1 == null && new2 == null) || (old1 != null && old1.Equals(new2)))
                            && ((old2 == null && new1 == null) || (old2 != null && old2.Equals(new1)))) continue;
                    }
                    else if (slot == CharacterSlot.Trinket1 || slot == CharacterSlot.Trinket2)
                    {
                        ItemInstance old1 = oldCharacter[CharacterSlot.Trinket1];
                        ItemInstance old2 = oldCharacter[CharacterSlot.Trinket2];
                        ItemInstance new1 = newCharacter[CharacterSlot.Trinket1];
                        ItemInstance new2 = newCharacter[CharacterSlot.Trinket2];
                        if (((old1 == null && new2 == null) || (old1 != null && old1.Equals(new2)))
                            && ((old2 == null && new1 == null) || (old2 != null && old2.Equals(new1)))) continue;
                    }
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

            currentCalc = oldCharacter.CurrentCalculations.GetCharacterCalculations(oldCharacter, null, false, true, true);
            var oldValue = Optimizer.ItemInstanceOptimizer.GetOptimizationValue(oldCharacter, currentCalc);
            CurrentScoreLabel.Text = string.Format("Current: {0}", oldValue);
            CurrentCalculations.SetCalculations(currentCalc.GetCharacterDisplayCalculationValues());
            CurrentTalents.Character = oldCharacter; //CurrentTalents.IsEnabled = false;
            CurrentBuffs.Character = oldCharacter; //CurrentBuffs.IsEnabled = false;

            optimizedCalc = newCharacter.CurrentCalculations.GetCharacterCalculations(newCharacter, null, false, true, true);
            var newValue = Optimizer.ItemInstanceOptimizer.GetOptimizationValue(newCharacter, optimizedCalc);
            OptimizedScoreLabel.Text = string.Format("Optimized: {0} ({1:P} change)", newValue, (newValue - oldValue) / oldValue);
            OptimizedCalculations.SetCalculations(optimizedCalc.GetCharacterDisplayCalculationValues());
            OptimizedTalents.Character = newCharacter; //OptimizedTalents.IsEnabled = false;
            OptimizedBuffs.Character = newCharacter; //OptimizedBuffs.IsEnabled = false;
        }
        public CharacterCalculationsBase currentCalc;
        public CharacterCalculationsBase optimizedCalc;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void BT_StoreIt_Click(object sender, RoutedEventArgs e)
        {
            WeWantToStoreIt = true;
            this.DialogResult = true;
        }
    }
}

