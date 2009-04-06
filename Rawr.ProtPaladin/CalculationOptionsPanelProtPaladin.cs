using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Rawr.CustomControls;

namespace Rawr.ProtPaladin
{
	public partial class CalculationOptionsPanelProtPaladin : CalculationOptionsPanelBase
	{
		private Dictionary<int, string> armorBosses = new Dictionary<int, string>();

		public CalculationOptionsPanelProtPaladin()
		{
			InitializeComponent();
            armorBosses.Add(13100, ": Tier 7 Bosses"); // armorBosses.Add(13083, ":Wrath Bosses");
		}

		protected override void LoadCalculationOptions()
		{
			_loadingCalculationOptions = true;
			if (Character.CalculationOptions == null)
				Character.CalculationOptions = new CalculationOptionsProtPaladin();

			CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
            PaladinTalents Talents = Character.PaladinTalents;

            // Attacker Stats
            comboBoxTargetType.SelectedItem = calcOpts.TargetType.ToString();
            numericUpDownTargetLevel.Value = calcOpts.TargetLevel;
			trackBarTargetArmor.Value = calcOpts.TargetArmor;
            trackBarBossAttackValue.Value = calcOpts.BossAttackValue;
            trackBarBossAttackSpeed.Value = (int)(calcOpts.BossAttackSpeed / 0.25f);
            checkBoxUseParryHaste.Checked = calcOpts.UseParryHaste;
            // Stupid hack since you can't put in newlines into the VS editor properties
            extendedToolTipUseParryHaste.ToolTipText =
                extendedToolTipUseParryHaste.ToolTipText.Replace("May not", Environment.NewLine + "May not");

            // Ranking System
            if (calcOpts.ThreatScale > 24.0f) // Old scale value being saved, reset to default
                calcOpts.ThreatScale = 8.0f;
            trackBarThreatScale.Value = Convert.ToInt32(calcOpts.ThreatScale / 8.0f / 0.1f);
            if (calcOpts.MitigationScale > 1.0f) // Old scale value being saved, reset to default
                calcOpts.MitigationScale = (1.0f / 8.0f);
            trackBarMitigationScale.Value = Convert.ToInt32((calcOpts.MitigationScale * 8.0f / 0.1f));
            radioButtonMitigationScale.Checked = (calcOpts.RankingMode == 1);
            radioButtonTankPoints.Checked = (calcOpts.RankingMode == 2);
            radioButtonBurstTime.Checked = (calcOpts.RankingMode == 3);
            radioButtonDamageOutput.Checked = (calcOpts.RankingMode == 4);
            trackBarThreatScale.Enabled = labelThreatScale.Enabled = (calcOpts.RankingMode != 4);
            trackBarMitigationScale.Enabled = labelMitigationScale.Enabled = (calcOpts.RankingMode == 1);

            // Seal Choice
            radioButtonSoR.Checked = (calcOpts.SealChoice == "Seal of Righteousness");
            radioButtonSoV.Checked = (calcOpts.SealChoice == "Seal of Vengeance");

            // Glyphs
            //checkBoxGlyphOfJudgement.Checked  = Talents.GlyphOfJudgement;
            //checkBoxGlyphOfSealOfVengeance.Checked = Talents.GlyphOfSealOfVengeance;
            //checkBoxGlyphOfExorcism.Checked = Talents.GlyphOfExorcism;
            //checkBoxGlyphOfDivinePlea.Checked = Talents.GlyphOfDivinePlea;
            //checkBoxGlyphOfSenseUndead.Checked = Talents.GlyphOfSenseUndead;

            calcOpts.UseHolyShield = checkBoxUseHolyShield.Checked;
			
			labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
            labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
            labelBossAttackSpeed.Text = String.Format("{0:0.00}s", ((float)(trackBarBossAttackSpeed.Value) * 0.25f));
            labelThreatScale.Text = String.Format("{0:0.0}", ((float)(trackBarThreatScale.Value) * 0.1f));
            labelMitigationScale.Text = String.Format("{0:0.0}", ((float)(trackBarMitigationScale.Value) * 0.1f));

			_loadingCalculationOptions = false;
		}

		private bool _loadingCalculationOptions = false;
		private void calculationOptionControl_Changed(object sender, EventArgs e)
		{
			if (!_loadingCalculationOptions)
			{
				CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
				// Attacker Stats
                trackBarTargetArmor.Value = 100 * (trackBarTargetArmor.Value / 100);
				labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
                trackBarBossAttackValue.Value = 500 * (trackBarBossAttackValue.Value / 500);
                labelBossAttackValue.Text = trackBarBossAttackValue.Value.ToString();
                labelBossAttackSpeed.Text = String.Format("{0:0.00}s", ((float)(trackBarBossAttackSpeed.Value) * 0.25f));
				// Ranking System
                labelThreatScale.Text = String.Format("{0:0.0}", ((float)(trackBarThreatScale.Value) * 0.1f));
				labelMitigationScale.Text = String.Format("{0:0.0}", ((float)(trackBarMitigationScale.Value) * 0.1f));

				//c alcOpts.TargetLevel = int.Parse(comboBoxTargetLevel.SelectedItem.ToString());
                calcOpts.TargetLevel = (int)numericUpDownTargetLevel.Value;
				calcOpts.TargetArmor = trackBarTargetArmor.Value;
                calcOpts.BossAttackValue = trackBarBossAttackValue.Value;
                calcOpts.BossAttackSpeed = ((float)(trackBarBossAttackSpeed.Value) * 0.25f);
                calcOpts.ThreatScale = ((float)(trackBarThreatScale.Value) * 0.1f * 8.0f);
                calcOpts.MitigationScale = ((float)(trackBarMitigationScale.Value) * 0.1f / 8.0f);

				Character.OnCalculationsInvalidated();
			}
		}

        private void checkBoxUseParryHaste_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.UseParryHaste = checkBoxUseParryHaste.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                if (radioButtonTankPoints.Checked)
                {
                    calcOpts.RankingMode = 2;
                    trackBarThreatScale.Value = 10;
                }
                else if (radioButtonBurstTime.Checked)
                {
                    calcOpts.RankingMode = 3;
                    trackBarThreatScale.Value = 0;
                }
                else if (radioButtonDamageOutput.Checked)
                {
                    calcOpts.RankingMode = 4;
                    trackBarThreatScale.Value = 10;
                }
                else
                {
                    calcOpts.RankingMode = 1;
                    trackBarThreatScale.Value = 10;
                }
                trackBarThreatScale.Enabled = labelThreatScale.Enabled = (calcOpts.RankingMode != 4);
                trackBarMitigationScale.Enabled = labelMitigationScale.Enabled = (calcOpts.RankingMode == 1);

                Character.OnCalculationsInvalidated();
            }
        }
        
        private void extendedToolTipMitigtionScale_Click(object sender, EventArgs e)
        {
            if (!radioButtonMitigationScale.Checked)
                radioButtonMitigationScale.Checked = true;
        }

        private void extendedToolTipTankPoints_Click(object sender, EventArgs e)
        {
            if (!radioButtonTankPoints.Checked)
                radioButtonTankPoints.Checked = true;
        }

        private void extendedToolTipBurstTime_Click(object sender, EventArgs e)
        {
            if (!radioButtonBurstTime.Checked)
                radioButtonBurstTime.Checked = true;
        }

        private void extendedToolTipDamageOutput_Click(object sender, EventArgs e)
        {
            if (!radioButtonDamageOutput.Checked)
                radioButtonDamageOutput.Checked = true;
        }

        private void radioButtonSealChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                if (radioButtonSoR.Checked)
                {
                    calcOpts.SealChoice = "Seal of Righteousness";
                }
                else
                {
                    calcOpts.SealChoice = "Seal of Vengeance";
                }
                Character.OnCalculationsInvalidated();
            }
        }

        private void checkBoxUseHolyShield_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.UseHolyShield = checkBoxUseHolyShield.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void numericUpDownTargetLevel_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.TargetLevel = (int)numericUpDownTargetLevel.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
                calcOpts.TargetType = comboBoxTargetType.SelectedItem.ToString();
                Character.OnCalculationsInvalidated();
            }
        }

        //#region Glyphs

        //private void checkBoxGlyphOfJudgement_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!_loadingCalculationOptions)
        //    {
        //        CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
        //        PaladinTalents Talents = Character.PaladinTalents;
        //        Talents.GlyphOfJudgement = checkBoxGlyphOfJudgement.Checked;
        //        Character.OnCalculationsInvalidated();
        //    }
        //}

        //private void checkBoxGlyphOfSealOfVengeance_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!_loadingCalculationOptions)
        //    {
        //        CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
        //        PaladinTalents Talents = Character.PaladinTalents;
        //        Talents.GlyphOfSealOfVengeance = checkBoxGlyphOfSealOfVengeance.Checked;
        //        Character.OnCalculationsInvalidated();
        //    }
        //}

        //private void checkBoxGlyphOfExorcism_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!_loadingCalculationOptions)
        //    {
        //        CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
        //        PaladinTalents Talents = Character.PaladinTalents;
        //        Talents.GlyphOfExorcism = checkBoxGlyphOfExorcism.Checked;
        //        Character.OnCalculationsInvalidated();
        //    }
        //}

        //private void checkBoxGlyphOfDivinePlea_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!_loadingCalculationOptions)
        //    {
        //        CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
        //        PaladinTalents Talents = Character.PaladinTalents;
        //        Talents.GlyphOfDivinePlea = checkBoxGlyphOfDivinePlea.Checked;
        //        Character.OnCalculationsInvalidated();
        //    }
        //}

        //private void checkBoxGlyphOfSenseUndead_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!_loadingCalculationOptions)
        //    {
        //        CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
        //        PaladinTalents Talents = Character.PaladinTalents;
        //        Talents.GlyphOfSenseUndead = checkBoxGlyphOfSenseUndead.Checked;
        //        Character.OnCalculationsInvalidated();
        //    }
        //}

        //private void checkBoxGlyphOfSealOfRighteousness_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!_loadingCalculationOptions)
        //    {
        //        CalculationOptionsProtPaladin calcOpts = Character.CalculationOptions as CalculationOptionsProtPaladin;
        //        PaladinTalents Talents = Character.PaladinTalents;
        //        Talents.GlyphOfSealOfRighteousness = checkBoxGlyphOfSealOfRighteousness.Checked;
        //        Character.OnCalculationsInvalidated();
        //    }
        //}

        //#endregion
    }

	[Serializable]
	public class CalculationOptionsProtPaladin : ICalculationOptionBase
	{
		public string GetXml()
		{
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(CalculationOptionsProtPaladin));
			StringBuilder xml = new StringBuilder();
			System.IO.StringWriter writer = new System.IO.StringWriter(xml);
			serializer.Serialize(writer, this);
			return xml.ToString();
		}

		public int TargetLevel = 83;
		public int TargetArmor = 13100;
		public int BossAttackValue = 25000;
        public float BossAttackSpeed = 2.0f;
        public bool UseParryHaste = false;
		public float ThreatScale = 8.0f;
        public float MitigationScale = 0.125f;
        public int RankingMode = 1;
        /*
        public bool GlyphSealVengeance = false;
        public bool GlyphSealRighteousness = false;
        public bool GlyphJudgement = false;
        public bool GlyphExorcism = false;
        public bool GlyphDivinePlea = false;
        public bool GlyphSenseUndead = false;
        */
        public bool UseHolyShield = true;
        public string SealChoice = "Seal of Vengeance";
        public string TargetType = "Unspecified";
        public PaladinTalents talents = null;
	}
}
