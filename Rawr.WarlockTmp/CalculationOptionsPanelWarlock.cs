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
        private int _ignoreCount;

        #endregion


        #region methods

        private void RefreshRotationPanel() {

            ++_ignoreCount;

            rotationCombo.Items.Clear();
            foreach (string rotation in _options.Rotations.Keys) {
                rotationCombo.Items.Add(rotation);
            }
            rotationCombo.SelectedItem = _options.ActiveRotation.Name;

            rotationMenu.Items.Clear();
            foreach (string spell in Spell.ALL_SPELLS) {
                if (!GetActivePriorities().Contains(spell)
                    && !fillerCombo.Items.Contains(spell)) {

                    rotationMenu.Items.Add(spell);
                }
            }

            rotationList.Items.Clear();
            foreach (string spell in GetActivePriorities()) {
                rotationList.Items.Add(spell);
            }

            fillerCombo.SelectedItem = _options.ActiveRotation.Filler;

            RefreshRotationButtons();

            --_ignoreCount;
        }

        private void RefreshRotationButtons() {

            int itemCount = rotationList.Items.Count;
            int curIndex = rotationList.SelectedIndex;

            rotationAddButton.Enabled = rotationMenu.SelectedIndex >= 0;
            rotationUpButton.Enabled = curIndex > 0;
            rotationDownButton.Enabled
                = curIndex >= 0 && curIndex < itemCount - 1;
            rotationClearButton.Enabled = itemCount > 0;

            rotationErrorLabel.Text = _options.ActiveRotation.GetError();
        }

        private void RotationSwap(int swapWith) {

            int oldIndex = rotationList.SelectedIndex;
            int newIndex = oldIndex + swapWith;
            Utilities.SwapElements(GetActivePriorities(), oldIndex, newIndex);
            RefreshRotationPanel();
            rotationList.SelectedIndex = newIndex;
        }

        private List<string> GetActivePriorities() {

            return _options.ActiveRotation.SpellPriority;
        }

        private string PromptForRotationName(string title, string start) {

            string error = null;
            while (true) {
                string message = "Choose a name:";
                if (error != null) {
                    message = error + message;
                }
                string name
                    = TextInputDialog.Show(title, message, start);
                if (name == null) {
                    return null;
                }
                if (name.Length == 0) {
                    error = "The name cannot be blank.  ";
                } else if (_options.Rotations.ContainsKey(name)) {
                    error = "That name already exists.  ";
                } else {
                    return name;
                }
            }
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
            ++_ignoreCount;

            targetLevelCombo.Text = _options.TargetLevel.ToString();
            fightLengthSpinner.Value = (decimal) _options.Duration;
            latencySpinner.Value = (decimal) _options.Latency * 1000;
            RefreshRotationPanel();

            --_ignoreCount;
        }

        #endregion


        #region event handlers

        private void petCombo_SelectedIndexChanged(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }


            Character.OnCalculationsInvalidated();
        }

        private void infernalCheck_CheckedChanged(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }
            

            Character.OnCalculationsInvalidated();
        }

        private void targetLevelCombo_SelectedIndexChanged(
            object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            _options.TargetLevel = Convert.ToInt32(targetLevelCombo.Text);
            Character.OnCalculationsInvalidated();
        }

        private void fightLengthSpinner_ValueChanged(
            object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            _options.Duration = (float) fightLengthSpinner.Value;
            Character.OnCalculationsInvalidated();
        }

        private void latencySpinner_ValueChanged(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            _options.Latency = (float) latencySpinner.Value / 1000f;
            Character.OnCalculationsInvalidated();
        }

        private void rotationAddButton_Click(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            ++_ignoreCount;
            GetActivePriorities().Add((string) rotationMenu.SelectedItem);
            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
            --_ignoreCount;
        }

        private void rotationRemoveButton_Click(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            int index = rotationList.SelectedIndex;
            GetActivePriorities().RemoveAt(index);
            RefreshRotationPanel();
            int itemCount = rotationList.Items.Count;
            if (itemCount > 0) {
                rotationList.SelectedIndex = Math.Min(index, itemCount - 1);
            }
            Character.OnCalculationsInvalidated();
        }

        private void rotationUpButton_Click(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            RotationSwap(-1);
            Character.OnCalculationsInvalidated();
        }

        private void rotationDownButton_Click(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            RotationSwap(1);
            Character.OnCalculationsInvalidated();
        }

        private void rotationClearButton_Click(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            GetActivePriorities().Clear();
            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }

        private void rotationMenu_SelectedIndexChanged(
            object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            RefreshRotationButtons();
        }

        private void rotationList_SelectedIndexChanged(
            object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            RefreshRotationButtons();
        }

        private void fillerCombo_SelectedIndexChanged(
            object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            _options.ActiveRotation.Filler = (string) fillerCombo.SelectedItem;
            Character.OnCalculationsInvalidated();
        }

        private void procCheckbox_CheckedChanged(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            _options.NoProcs = ProcCheckbox.Checked;
            Character.OnCalculationsInvalidated();
        }

        private void rotationCombo_SelectedIndexChanged(
            object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            _options.ActiveRotation
                = _options.Rotations[(string) rotationCombo.SelectedItem];
            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }

        private void newRotationButton_Click(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            string name = PromptForRotationName("New Rotation", "");
            if (name == null) {
                return;
            }
            _options.ActiveRotation = new Rotation(name, "Shadow Bolt");

            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }

        private void rotationRenameButton_Click(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            Rotation rotation = _options.ActiveRotation;
            string name
                = PromptForRotationName("Rename Rotation", rotation.Name);
            if (name == null) {
                return;
            }
            _options.RemoveActiveRotation();
            rotation.Name = name;
            _options.ActiveRotation = rotation;

            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }

        private void deleteRotationButton_Click(object sender, EventArgs e) {

            if (_ignoreCount > 0) {
                return;
            }

            _options.RemoveActiveRotation();
            if (_options.Rotations.Count == 0) {
                _options.ActiveRotation
                    = new Rotation("New Rotation", "Shadow Bolt");
            }

            RefreshRotationPanel();
            Character.OnCalculationsInvalidated();
        }

        #endregion
    }
}
