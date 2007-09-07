using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr
{
	public partial class FormGemEditor : Form
    {
        public FormGemEditor()
        {
            InitializeComponent();
            foreach (Gem gem in ItemCache.Gems)
            {
                listBoxGems.Items.Add(gem);
            }
            if (listBoxGems.Items.Count > 0) listBoxGems.SelectedIndex = 0;
        }

        private void listBoxGems_SelectedIndexChanged(object sender, EventArgs e)
        {
            Gem selectedGem = listBoxGems.SelectedItem as Gem;
            textBoxName.DataBindings.Clear();
            textBoxIcon.DataBindings.Clear();
            numericUpDownAgility.DataBindings.Clear();
            numericUpDownDefense.DataBindings.Clear();
            numericUpDownDodge.DataBindings.Clear();
            numericUpDownId.DataBindings.Clear();
            numericUpDownResil.DataBindings.Clear();
            numericUpDownStamina.DataBindings.Clear();
            comboBoxColor.DataBindings.Clear();

            if (selectedGem != null)
            {
                textBoxName.DataBindings.Add("Text", selectedGem, "Name");
                textBoxIcon.DataBindings.Add("Text", selectedGem, "IconPath");
                numericUpDownId.DataBindings.Add("Value", selectedGem, "Id");
                numericUpDownAgility.DataBindings.Add("Value", selectedGem.Stats, "Agility");
                numericUpDownDefense.DataBindings.Add("Value", selectedGem.Stats, "DefenseRating");
                numericUpDownDodge.DataBindings.Add("Value", selectedGem.Stats, "DodgeRating");
                numericUpDownResil.DataBindings.Add("Value", selectedGem.Stats, "Resilience");
                numericUpDownStamina.DataBindings.Add("Value", selectedGem.Stats, "Stamina");
                comboBoxColor.DataBindings.Add("Text", selectedGem, "ColorString");
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormEnterId form = new FormEnterId();
            if (form.ShowDialog() == DialogResult.OK)
            {
                Gem oldGem = ItemCache.FindGemById(form.Value);
                if (oldGem != null)
                    listBoxGems.Items.Remove(oldGem);

                Gem newGem = new Gem("New Gem", form.Value, "/images/icons/temp.png", Item.ItemSlot.None, new Stats(0, 0, 0, 0, 0, 0, 0));
                ItemCache.AddGem(newGem);
                listBoxGems.Items.Add(newGem);
                listBoxGems.SelectedItem = newGem;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            ItemCache.DeleteGem(listBoxGems.SelectedItem as Gem);
            listBoxGems.Items.Remove(listBoxGems.SelectedItem);
            listBoxGems.SelectedIndex = 0;
        }
    }
}