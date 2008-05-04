using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Warlock
{
	public partial class CalculationOptionsPanelWarlock : CalculationOptionsPanelBase
	{
		public CalculationOptionsPanelWarlock()
		{
			InitializeComponent();
        }

		//private void setDefaultOption(string option, string value)
		//{
		//    if (!Character.CalculationOptions.ContainsKey(option))
		//        Character.CalculationOptions.Add(option, value);
		//}

		protected override void LoadCalculationOptions()
		{
			if (Character.CurrentCalculationOptions == null)
				Character.CurrentCalculationOptions = new CalculationOptionsWarlock();
			//setDefaultOption("TargetLevel", "73");
			//setDefaultOption("EnforceMetagemRequirements", "T");
			//setDefaultOption("Latency", "0.05");
			//setDefaultOption("Duration", "600");
			//setDefaultOption("Misery", "T");
			//setDefaultOption("ShadowWeaving", "T");
			//setDefaultOption("ShadowsBonus", "1.10");
			//setDefaultOption("ElementsBonus", "1.10");
			//setDefaultOption("SacraficedPet", "Succubus");
			//setDefaultOption("Curse", "CurseOfAgony");
			//setDefaultOption("Corruption", "F");
			//setDefaultOption("UnstableAffliction", "F");
			//setDefaultOption("SiphonLife", "F");
			//setDefaultOption("Immolate", "F");
			//setDefaultOption("FillerSpell", "Shadowbolt");
			//setDefaultOption("Pet", "Succubus");


			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			comboBoxTargetLevel.SelectedItem = calcOpts.TargetLevel.ToString();
			checkBoxEnforceMetagemRequirements.Checked = Character.EnforceMetagemRequirements;
            textBoxLatency.Text = calcOpts.Latency.ToString();
            textBoxDuration.Text = calcOpts.Duration.ToString();
            comboBoxPetSelection.SelectedItem = calcOpts.SacrificedPet;
            comboBoxCastCurse.SelectedItem = "CurseOfAgony";
            checkCorruption.Checked = false;
            checkUnstable.Checked = false;
            checkSiphonLife.Checked = false;
            checkSacraficed.Checked = true;
            checkImmolate.Checked = false;
            comboBoxFilterSpell.SelectedItem = "Shadowbolt";
        }
	
		private void comboBoxTargetLevel_SelectedIndexChanged(object sender, EventArgs e)
		{
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
			Character.OnItemsChanged();
		}

		private void checkBoxEnforceMetagemRequirements_CheckedChanged(object sender, EventArgs e)
		{
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			Character.EnforceMetagemRequirements = checkBoxEnforceMetagemRequirements.Checked;
			Character.OnItemsChanged();
		}

        

        private void textBoxLatency_TextChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			try
			{
			calcOpts.Latency = float.Parse(textBoxLatency.Text);
			}catch{}
            Character.OnItemsChanged();
        }

        private void comboBoxFilterSpell_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.FillerSpell = (sender as ComboBox).SelectedItem.ToString();
            if (calcOpts.FillerSpell.ToUpper() == "INCINERATE")
                checkImmolate.Checked = true;
            Character.OnItemsChanged();
        }

        private void comboBoxCastCurse_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.Curse = (sender as ComboBox).SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void checkImmolate_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.Immolate = (sender as CheckBox).Checked;
            Character.OnItemsChanged();
        }

        private void checkCorruption_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.Corruption = (sender as CheckBox).Checked;
            Character.OnItemsChanged();
        }

        private void checkSiphonLife_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.SiphonLife = (sender as CheckBox).Checked;
            Character.OnItemsChanged();
        }

        private void checkUnstable_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.UnstableAffliction = (sender as CheckBox).Checked;
            Character.OnItemsChanged();
        }

        private void comboBoxPetSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.Pet = (sender as ComboBox).SelectedItem.ToString();
            Character.OnItemsChanged();
        }

        private void checkSacraficed_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.SacrificedPet = (sender as CheckBox).Checked ? calcOpts.Pet : "";
            Character.OnItemsChanged();
        }

        private void checkScorch_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.Scorch = (sender as CheckBox).Checked;
            Character.OnItemsChanged();
        }

        private void checkShadowWeaving_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.ShadowWeaving = (sender as CheckBox).Checked;
            Character.OnItemsChanged();
        }

        private void checkMisery_CheckedChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			calcOpts.Misery = (sender as CheckBox).Checked;
            Character.OnItemsChanged();
        }

        private void comboBoxElements_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			try
			{
				calcOpts.ElementsBonus = float.Parse((sender as ComboBox).SelectedItem.ToString());
			}
			catch { }
            Character.OnItemsChanged();
        }

        private void comboBoxShadows_SelectedIndexChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			try
			{
			calcOpts.ShadowsBonus = float.Parse((sender as ComboBox).SelectedItem.ToString());
            }
			catch { }
            Character.OnItemsChanged();
        }

        private void textBoxISBUptime_TextChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			try
			{
				calcOpts.ISBUptime = float.Parse((sender as TextBox).Text);
			}
			catch { }
            Character.OnItemsChanged();
        }

        private void textBoxDuration_TextChanged(object sender, EventArgs e)
        {
			CalculationOptionsWarlock calcOpts = Character.CurrentCalculationOptions as CalculationOptionsWarlock;
			try
			{
				calcOpts.Duration = float.Parse((sender as TextBox).Text);
            }
			catch { }
            Character.OnItemsChanged();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Character.Talents != null)
            {
                TalentForm tf = new TalentForm();
                tf.SetParameters(Character.Talents, Character.Class);
                tf.Show();
            }
            else
            {
                MessageBox.Show("No talents found");
            }
        }

       

        

        
	}

	[Serializable]
	public class CalculationOptionsWarlock : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsWarlock));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 73;
		public bool EnforceMetagemRequirements = false;
		public float Latency = 0.05f;
		public float Duration = 600;
		public bool Misery = true;
		public bool ShadowWeaving = true;
		public float ShadowsBonus = 1.1f;
		public float ElementsBonus = 1.1f;
		public string SacrificedPet = "Succubus";
		public string Curse = "CurseOfAgony";
		public bool Corruption = false;
		public bool UnstableAffliction = false;
		public bool SiphonLife = false;
		public bool Scorch = false;
		public bool Immolate = false;
		public float ISBUptime = 0f;
		public string FillerSpell = "Shadowbolt";
		public string Pet = "Succubus";
	}
}
