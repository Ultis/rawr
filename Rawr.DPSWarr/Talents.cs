using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.DPSWarr
{
    public partial class Talents : Form
    {
        public Talents(CalculationOptionsPanelDPSWarr retCalcOpts)
        {
            this.calcOptions = retCalcOpts;
            InitializeComponent();            
        }
        private CalculationOptionsPanelDPSWarr calcOptions;

        public Character Character
        {
            get
            {
                return calcOptions.Character;
            }
        }

        private void Talents_Load(object sender, EventArgs e)
        {
			CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
			if (calcOpts.TalentsSaved)
            {
				_loading = true;
				comboBoxTwoHandedSpec.SelectedItem = calcOpts.TwoHandedSpec.ToString();
				comboBoxDeathWish.SelectedItem = calcOpts.DeathWish.ToString();
				comboBoxImpale.SelectedItem = calcOpts.Impale.ToString(); 
				comboBoxDeepWounds.SelectedItem = calcOpts.DeepWounds.ToString(); 
				comboBoxMortalStrike.SelectedItem = calcOpts.MortalStrike.ToString(); 
				comboBoxCruelty.SelectedItem = calcOpts.Cruelty.ToString();
				comboBoxFlurry.SelectedItem = calcOpts.Flurry.ToString(); 
				comboBoxWeaponMastery.SelectedItem = calcOpts.WeaponMastery.ToString();
				comboBoxImpSlam.SelectedItem = calcOpts.ImpSlam.ToString();
				_loading = false;
            }
        }
		private bool _loading = false;
        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (!_loading)
			{
				CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
				//ComboBox cb = (ComboBox)sender;
				//string talent = cb.Name.Substring(8);
				//Character.CalculationOptions[talent] = cb.SelectedItem.ToString();

				calcOpts.TwoHandedSpec = int.Parse((comboBoxTwoHandedSpec.SelectedItem ?? "0").ToString());
				calcOpts.DeathWish = int.Parse((comboBoxDeathWish.SelectedItem ?? "0").ToString());
				calcOpts.Impale = int.Parse((comboBoxImpale.SelectedItem ?? "0").ToString());
				calcOpts.DeepWounds = int.Parse((comboBoxDeepWounds.SelectedItem ?? "0").ToString());
				calcOpts.MortalStrike = int.Parse((comboBoxMortalStrike.SelectedItem ?? "0").ToString());
				calcOpts.Cruelty = int.Parse((comboBoxCruelty.SelectedItem ?? "0").ToString());
				calcOpts.Flurry = int.Parse((comboBoxFlurry.SelectedItem ?? "0").ToString());
				calcOpts.WeaponMastery = int.Parse((comboBoxWeaponMastery.SelectedItem ?? "0").ToString());
				calcOpts.ImpSlam = int.Parse((comboBoxImpSlam.SelectedItem ?? "0").ToString());

				calcOpts.TalentsSaved = true;
				Character.OnCalculationsInvalidated();
			}
        }
        
    }
}
