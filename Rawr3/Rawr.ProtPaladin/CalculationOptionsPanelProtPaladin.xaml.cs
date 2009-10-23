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

namespace Rawr.ProtPaladin
{
	public partial class CalculationOptionsPanelProtPaladin : UserControl, ICalculationOptionsPanel
    {

        #region Initialization

        private static string[] TargetTypes = new string[]
        {
            "Unspecified",
            "Humanoid",
            "Undead",
            "Demon",
            "Elemental",
            "Giant",
            "Mechanical",
            "Beast",
            "Dragonkin"
        };

        private static string[] MagicDamageTypes = new string[]
        {
            "None",
            "Physical",
            "Holy",
            "Fire",
            "Nature",
            "Frost",
            "Frostfire",
            "Shadow",
            "Arcane",
            "Spellfire",
            "Naturefire",
        };

        private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

        public CalculationOptionsPanelProtPaladin()
		{
			InitializeComponent();

            // Setup TargetType combo box
            cboTargetType.ItemsSource = TargetTypes;

            // Setup MagicDamageType combo box
            cboMagicDamageType.ItemsSource = MagicDamageTypes;

            // Setup TargetArmor values
            armorBosses.Add((int)StatConversion.NPC_ARMOR[80 - 80], "Level 80 Creatures");
            armorBosses.Add((int)StatConversion.NPC_ARMOR[81 - 80], "Level 81 Creatures");
            armorBosses.Add((int)StatConversion.NPC_ARMOR[82 - 80], "Level 82 Creatures");
            armorBosses.Add((int)StatConversion.NPC_ARMOR[83 - 80], "Bosses & Level 83 Creatures");
        }

        #endregion

        #region ICalculationOptionsPanel

        public UserControl PanelControl { get { return this; } }

		private Character character;
        private CalculationOptionsProtPaladin calcOpts;
		public Character Character
		{
			get
			{
				return character;
			}
			set
			{
                if (character != null && character.CalculationOptions != null && character.CalculationOptions is CalculationOptionsProtPaladin)
                {
                    ((CalculationOptionsProtPaladin)character.CalculationOptions).PropertyChanged -= new PropertyChangedEventHandler(CalculationOptionsPanelProtPaladin_PropertyChanged);
                }

                character = value;
                if (character.CalculationOptions == null)
                {
                    character.CalculationOptions = new CalculationOptionsProtPaladin();
                }

                calcOpts = character.CalculationOptions as CalculationOptionsProtPaladin;

                DataContext = calcOpts;

                calcOpts.PropertyChanged += new PropertyChangedEventHandler(CalculationOptionsPanelProtPaladin_PropertyChanged);
            }
        }

        void CalculationOptionsPanelProtPaladin_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Character.OnCalculationsInvalidated();
        }

        #endregion

        #region Events

        private void sliTargetArmor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbTargetArmor != null)
                tbTargetArmor.Text = e.NewValue.ToString() + (armorBosses.ContainsKey((int)e.NewValue) ? ": " + armorBosses[(int)e.NewValue] : "");
        }

        private void btnResetTargetArmor_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.TargetArmor = (int)StatConversion.NPC_ARMOR[calcOpts.TargetLevel - 80];
        }

        private void btnResetBossAttackValue_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.BossAttackValue = 80000;
        }

        private void sliBossAttackSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbBossAttackSpeed != null)
                tbBossAttackSpeed.Text = string.Format("{0:N2} seconds", e.NewValue);
        }

        private void btnResetBossAttackSpeed_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.BossAttackSpeed = 1.0f;
        }

        private void btnResetBossAttackValueMagic_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.BossAttackValueMagic = 20000;
        }

        private void silBossAttackSpeedMagic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (tbBossAttackSpeedMagic != null)
                tbBossAttackSpeedMagic.Text = string.Format("{0:N2} seconds", e.NewValue);
        }

        private void btnResetBossAttackSpeedMagic_Click(object sender, RoutedEventArgs e)
        {
            calcOpts.BossAttackSpeedMagic = 2.0f;
        }

        #endregion

    }
}
