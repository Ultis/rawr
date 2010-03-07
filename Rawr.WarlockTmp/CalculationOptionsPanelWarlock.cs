using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace Rawr.WarlockTmp {
    public partial class CalculationOptionsPanelWarlock
        : CalculationOptionsPanelBase {

        #region properties

        private CalculationOptionsWarlock _options;
        private bool _ignoreEvents;

        #endregion


        #region methods

        private void RefreshRotationPanel() {

            rotationSpellCombo.Items.Clear();
            foreach (String spell in Spell.ALL_SPELLS) {
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

            int itemCount = rotationList.Items.Count;
            int curIndex = rotationList.SelectedIndex;

            rotationAddButton.Enabled
                = rotationSpellCombo.Items.Contains(
                    rotationSpellCombo.Text);
            rotationUpButton.Enabled = curIndex > 0;
            rotationDownButton.Enabled
                = curIndex >= 0 && curIndex < itemCount - 1;
            rotationClearButton.Enabled = itemCount > 0;
        }

        private void RotationSwap(int swapWith) {

            int oldIndex = rotationList.SelectedIndex;
            int newIndex = oldIndex + swapWith;
            Utilities.SwapElements(_options.SpellPriority, oldIndex, newIndex);
            RefreshRotationPanel();
            rotationList.SelectedIndex = newIndex;
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
            fightLengthSpinner.Value = (decimal) _options.Duration;
            latencySpinner.Value = (decimal) _options.Latency * 1000;
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

            _options.Duration = (float) fightLengthSpinner.Value;
            Character.OnCalculationsInvalidated();
        }

        private void latencySpinner_ValueChanged(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            _options.Latency = (float) latencySpinner.Value / 1000f;
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

            int index = rotationList.SelectedIndex;
            _options.SpellPriority.RemoveAt(index);
            RefreshRotationPanel();
            int itemCount = rotationList.Items.Count;
            if (itemCount > 0) {
                rotationList.SelectedIndex = Math.Min(index, itemCount - 1);
            }
            Character.OnCalculationsInvalidated();
        }

        private void rotationUpButton_Click(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            RotationSwap(-1);
            Character.OnCalculationsInvalidated();
        }

        private void rotationDownButton_Click(object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            RotationSwap(1);
            Character.OnCalculationsInvalidated();
        }

        private void rotationClearButton_Click(object sender, EventArgs e) {

            _options.SpellPriority.Clear();
            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }

        private void rotationSpellCombo_SelectedIndexChanged(
            object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            RefreshRotationButtons();
            Character.OnCalculationsInvalidated();
        }

        private void rotationList_SelectedIndexChanged(
            object sender, EventArgs e) {

            if (_ignoreEvents) {
                return;
            }

            RefreshRotationButtons();
        }

        #endregion
    }
}
