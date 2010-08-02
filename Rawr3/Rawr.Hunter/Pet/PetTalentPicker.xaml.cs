using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace Rawr.Hunter
{
    public partial class PetTalentPicker : UserControl
    {
        private Character character;
        public Character Character
        {
            get { return character; }
            set
            {
                if (character != null) { character.ClassChanged -= new EventHandler(character_ClassChanged); }
                character = value;
                if (character != null)
                {
                    character.ClassChanged += new EventHandler(character_ClassChanged);
                    character_ClassChanged(this, EventArgs.Empty);
                }
            }
        }

        public void RefreshSpec() { character_ClassChanged(this, EventArgs.Empty); }
        private void character_ClassChanged(object sender, EventArgs e)
        {
            if (Character.Class != CharacterClass.Hunter) return;
            CalculationOptionsHunter calcOpts = Character.CalculationOptions as CalculationOptionsHunter;
            Tree1.Talents = calcOpts.PetTalents;
            TreeTab1.Header = Tree1.TreeName;
            Tree2.Talents = calcOpts.PetTalents;
            TreeTab2.Header = Tree2.TreeName;
            Tree3.Talents = calcOpts.PetTalents;
            TreeTab3.Header = Tree3.TreeName;
            
            //UpdateSavedSpecs();
        }

        public PetTalentPicker()
        {
            // Required to initialize variables
            InitializeComponent();
            Tree1.Tree = 0;
            Tree2.Tree = 1;
            Tree3.Tree = 2;

            Scroll1.SetIsMouseWheelScrollingEnabled(true);
            Scroll2.SetIsMouseWheelScrollingEnabled(true);
            Scroll3.SetIsMouseWheelScrollingEnabled(true);

            Tree1.TalentsChanged += new EventHandler(TalentsChanged);
            Tree2.TalentsChanged += new EventHandler(TalentsChanged);
            Tree3.TalentsChanged += new EventHandler(TalentsChanged);
        }

        public void TalentsChanged(object sender, EventArgs e)
        {
            //UpdateSavedSpecs();
            Character.OnCalculationsInvalidated();
        }

        // BELOW IS NOT IN USE UNTIL I MIGRATE THE CODE OVER FROM THE OPTIONS PANE
        #region

        public bool HasCustomSpec { get; private set; }

        private bool updating;
        private void UpdateSavedSpecs()
        {
            SavedTalentSpecList savedSpecs = SavedTalentSpec.SpecsFor(Character.Class);
            SavedTalentSpec current = null;
            updating = true;
            foreach (SavedTalentSpec sts in savedSpecs)
            {
                if (sts.Equals(Character.CurrentTalents))
                {
                    current = sts;
                    break;
                }
            }

            if (current != null)
            {
                HasCustomSpec = false;
                SavedCombo.ItemsSource = savedSpecs;
                SavedCombo.SelectedItem = current;
                SaveDeleteButton.Content = "Delete";
            }
            else
            {
                HasCustomSpec = true;
                current = new SavedTalentSpec("Custom", Character.CurrentTalents, Tree1.Points(), Tree2.Points(), Tree3.Points());
                SavedTalentSpecList currentList = new SavedTalentSpecList();
                currentList.AddRange(savedSpecs);
                currentList.Add(current);
                SavedCombo.ItemsSource = null;
                SavedCombo.ItemsSource = currentList;
                SavedCombo.SelectedItem = current;
                SaveDeleteButton.Content = "Save";
            }
            updating = false;
        }

        private void SaveDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SavedTalentSpec currentSpec = SavedCombo.SelectedItem as SavedTalentSpec;
            if (HasCustomSpec)
            {
                List<SavedPetTalentSpec> classTalents = new List<SavedPetTalentSpec>();
                FormSavePetTalentSpec dialog = new FormSavePetTalentSpec(classTalents);
                dialog.Closed += new EventHandler(dialog_Closed);
                dialog.Show();
            }
            else
            {
                SavedTalentSpec.AllSpecs.Remove(currentSpec);
                UpdateSavedSpecs();
            }
        }

        private void dialog_Closed(object sender, EventArgs e)
        {
            if (((ChildWindow)sender).DialogResult.GetValueOrDefault(false))
            {
                UpdateSavedSpecs();
            }
        }

        private void SavedCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!updating)
            {
                SavedPetTalentSpec newSpec = SavedCombo.SelectedItem as SavedPetTalentSpec;
                //Character.CurrentTalents = newSpec.TalentSpec(); // TODO
                character_ClassChanged(this, EventArgs.Empty);
                Character.OnCalculationsInvalidated();
            }
        }
        #endregion

    }
}