using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rawr.Warlock {
    public partial class SpellPriorityForm : Form 
    {
        private readonly ListBox _spellPriority;
        private readonly List<string> _warlockSpells;

        public SpellPriorityForm(List<string> warlockSpells, ListBox spellPriority) 
        {
            InitializeComponent();

            _warlockSpells = warlockSpells;
            _spellPriority = spellPriority;

            //populate the listbox with the spells to be used for combat
            lsSpellPriority.Items.AddRange(spellPriority.Items);

            //populate the combobox with all other spells known by the warlock
            foreach (string spell in warlockSpells)
            {
                if (!spellPriority.Items.Contains(spell))
                {
                    cmbSpells.Items.Add(spell);
                }
            }
        }

        private void bAdd_Click(object sender, EventArgs e) 
        {
            if (cmbSpells.SelectedItem == null) { return; }

            lsSpellPriority.Items.Add(cmbSpells.SelectedItem);
            cmbSpells.Items.RemoveAt(cmbSpells.SelectedIndex);
            cmbSpells.SelectedText = "";
        }

        private void bUp_Click(object sender, EventArgs e) 
        {
            if (lsSpellPriority.SelectedItem == null || lsSpellPriority.SelectedIndex == 0) { return; }

            lsSpellPriority.Items.Insert(lsSpellPriority.SelectedIndex - 1, lsSpellPriority.SelectedItem);
            lsSpellPriority.Items.RemoveAt(lsSpellPriority.SelectedIndex);
        }

        private void bDown_Click(object sender, EventArgs e) 
        {
            if (lsSpellPriority.SelectedItem == null || lsSpellPriority.SelectedIndex == lsSpellPriority.Items.Count - 1) { return; }

            lsSpellPriority.Items.Insert(lsSpellPriority.SelectedIndex + 2, lsSpellPriority.SelectedItem);
            lsSpellPriority.Items.RemoveAt(lsSpellPriority.SelectedIndex);
        }

        private void bRemove_Click(object sender, EventArgs e) 
        {
            if (lsSpellPriority.SelectedItem == null) { return; }

            cmbSpells.Items.Add(lsSpellPriority.SelectedItem);
            lsSpellPriority.Items.RemoveAt(lsSpellPriority.SelectedIndex);
        }

        private void bClear_Click(object sender, EventArgs e) 
        {
            lsSpellPriority.Items.Clear();
            cmbSpells.Items.Clear();
            cmbSpells.Items.AddRange(_warlockSpells.ToArray());
        }

        private void bSave_Click(object sender, EventArgs e) 
        {
            _spellPriority.Items.Clear();
            _spellPriority.Items.AddRange(lsSpellPriority.Items);
            Hide();
        }
    }
}