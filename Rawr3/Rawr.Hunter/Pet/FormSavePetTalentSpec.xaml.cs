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

namespace Rawr.Hunter
{
    public partial class FormSavePetTalentSpec : ChildWindow
    {
        public FormSavePetTalentSpec()
        {
            InitializeComponent();
        }
        public FormSavePetTalentSpec(ICollection<SavedPetTalentSpec> specs)
        {
            InitializeComponent();
            CB_TalentSpecs.ItemsSource = specs;
        }

        public SavedPetTalentSpec PetTalentSpec()
        {
            return TB_NewSpecName.Text != "" ? (SavedPetTalentSpec)(object)TB_NewSpecName.Text : (SavedPetTalentSpec)CB_TalentSpecs.SelectedItem;
        }

        public string PetTalentSpecName()
        {
            return TB_NewSpecName.Text != "" ? TB_NewSpecName.Text : (string)CB_TalentSpecs.SelectedItem;
        }

        private void BT_OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void BT_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
