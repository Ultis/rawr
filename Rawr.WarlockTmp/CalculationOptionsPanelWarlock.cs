using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.WarlockTmp {
    public partial class CalculationOptionsPanelWarlock
        : CalculationOptionsPanelBase {

        #region constants
        private static String[] ALL_SPELLS = { "Shadow Bolt" };
        #endregion


        #region properties

        private CalculationOptionsWarlock _options;
        private bool _ignoreEvents;

        #endregion


        #region methods

        private void RefreshRotationPanel() {

            rotationSpellCombo.Items.Clear();
            foreach (String spell in ALL_SPELLS) {
                if (!_options.SpellPriority.Contains(spell)) {
                    rotationSpellCombo.Items.Add(spell);
                }
            }

            rotationList.Items.Clear();
            foreach (String spell in _options.SpellPriority) {
                rotationList.Items.Add(spell);
            }

            RefreshRotationButtons();
        }

        private void RefreshRotationButtons() {

            rotationAddButton.Enabled
                = rotationSpellCombo.Items.Contains(
                    rotationSpellCombo.Text);
            rotationClearButton.Enabled = rotationList.Items.Count > 0;
        }

        #endregion


        #region initialization

        public CalculationOptionsPanelWarlock() {

            InitializeComponent();
        }

        protected override void LoadCalculationOptions() {

            if (Character.CalculationOptions == null) {
                Character.CalculationOptions = new CalculationOptionsWarlock();
            }
            _options = (CalculationOptionsWarlock) Character.CalculationOptions;
            _ignoreEvents = true;

            targetLevelCombo.Text = _options.TargetLevel.ToString();
            RefreshRotationPanel();

            _ignoreEvents = false;
        }

        #endregion


        #region event handlers

        private void petCombo_SelectedIndexChanged(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void infernalCheck_CheckedChanged(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            

            Character.OnCalculationsInvalidated();
        }

        private void targetLevelCombo_SelectedIndexChanged(
            object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            _options.TargetLevel = Convert.ToInt32(targetLevelCombo.Text);

            Character.OnCalculationsInvalidated();
        }

        private void fightLengthSpinner_ValueChanged(
            object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            

            Character.OnCalculationsInvalidated();
        }

        private void latencySpinner_ValueChanged(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void manaPotionCombo_SelectedIndexChanged(
            object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void replenishmentSpinner_ValueChanged(
            object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void newRotationButton_Click(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void rotationRenameButton_Click(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void deleteRotationButton_Click(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void rotationAddButton_Click(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            _ignoreEvents = true;
            _options.SpellPriority.Add(rotationSpellCombo.Text);
            //rotationSpellCombo.Text = string.Empty;
            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
            _ignoreEvents = false;
        }

        private void rotationRemoveButton_Click(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void rotationUpButton_Click(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void rotationDownButton_Click(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }



            Character.OnCalculationsInvalidated();
        }

        private void rotationClearButton_Click(object sender, EventArgs e) {

            _options.SpellPriority.Clear();
            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }

        private void rotationSpellCombo_SelectedIndexChanged(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            RefreshRotationButtons();
            Character.OnCalculationsInvalidated();
        }

        #endregion
    }
}
