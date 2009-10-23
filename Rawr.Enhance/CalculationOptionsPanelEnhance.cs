using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rawr.Base;

namespace Rawr.Enhance
{
    public partial class CalculationOptionsPanelEnhance : CalculationOptionsPanelBase
    {
        /// <summary>This Model's local bosslist</summary>
        private BossList bosslist = null;
        CalculationOptionsEnhance _calcOpts;
        
        public CalculationOptionsPanelEnhance()
        {
            InitializeComponent();
            if (bosslist == null) { bosslist = new BossList(); }
            comboBoxBoss.Items.AddRange(bosslist.GetBetterBossNamesAsArray());
            comboBoxCalculationToGraph.Items.AddRange(Graph.GetCalculationNames());
        }

        protected override void LoadCalculationOptions()
        {
            _loadingCalculationOptions = true;
            if (Character.CalculationOptions == null)
                Character.CalculationOptions = new CalculationOptionsEnhance();

            _calcOpts = Character.CalculationOptions as CalculationOptionsEnhance;
            CB_TargLvl.Text = _calcOpts.TargetLevel.ToString();
            CB_TargArmor.Text = _calcOpts.TargetArmor.ToString();
            comboBoxBoss.Text = _calcOpts.BossName;
            CK_InBack.Checked = _calcOpts.InBack;
            CB_InBackPerc.Value = _calcOpts.InBackPerc;
            trackBarAverageLag.Value = _calcOpts.AverageLag;
            trackbarSRMana.Value = (int)_calcOpts.MinManaSR;
            trackbarSRMana.Enabled = _calcOpts.UseMana;
            labelAverageLag.Text = trackBarAverageLag.Value.ToString();
            labelSRMana.Text = trackbarSRMana.Value.ToString();
            cmbLength.Value = (decimal)_calcOpts.FightLength;
            comboBoxMainhandImbue.SelectedItem = _calcOpts.MainhandImbue;
            comboBoxOffhandImbue.SelectedItem = _calcOpts.OffhandImbue;
            comboBoxCalculationToGraph.SelectedItem = _calcOpts.CalculationToGraph;
            chbMagmaSearing.Checked = _calcOpts.Magma;
            chbMana.Checked = _calcOpts.UseMana;
            chbBaseStatOption.Checked = _calcOpts.BaseStatOption;
            chkStatsStrength.Checked = _calcOpts.StatsList[0];
            chkStatsAgility.Checked = _calcOpts.StatsList[1];
            chkStatsAP.Checked = _calcOpts.StatsList[2];
            chkStatsCrit.Checked = _calcOpts.StatsList[3];
            chkStatsHit.Checked = _calcOpts.StatsList[4];
            chkStatsExp.Checked = _calcOpts.StatsList[5];
            chkStatsHaste.Checked = _calcOpts.StatsList[6];
            chkStatsArP.Checked = _calcOpts.StatsList[7];
            chkStatsSP.Checked =_calcOpts.StatsList[8];
            LoadPriorities();


      //      labelTargetArmorDescription.Text = trackBarTargetArmor.Value.ToString() + (armorBosses.ContainsKey(trackBarTargetArmor.Value) ? armorBosses[trackBarTargetArmor.Value] : "");
            labelAverageLag.Text = trackBarAverageLag.Value.ToString();

            tbModuleNotes.Text = "The EnhSim export option exists for users that wish to have very detailed analysis of their stats. " +
                "For most users the standard model should be quite sufficient.\r\n\r\n" +
                "If you wish to use the EnhSim Simulator you will need to get the latest version from http://enhsim.codeplex.com\r\n\r\n" +
                "Once you have installed the simulator the easiest way to run it is to run EnhSimGUI and use the Clipboard copy functions.\r\n\r\n" +
                "Press the button above to copy your current Rawr.Enhance data to the clipboard then in EnhSimGUI click on the 'Import from Clipboard' " + 
                "button to replace the values in the EnhSimGUI with your Rawr values. Now all you need to do is click Simulate to get your results.\r\n\r\n" + 
                "Refer to the EnhSim website for more detailed instructions on how to use the sim and its various options";

            _loadingCalculationOptions = false;
        }

        private bool _loadingCalculationOptions = false;
        private void calculationOptionControl_Changed(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                labelAverageLag.Text = trackBarAverageLag.Value.ToString();
                labelSRMana.Text = trackbarSRMana.Value.ToString();

                _calcOpts.SetBoss(bosslist.GetBossFromBetterName(comboBoxBoss.Text));
                _calcOpts.FightLength = (float)cmbLength.Value;
                _calcOpts.MainhandImbue = (string)comboBoxMainhandImbue.SelectedItem;
                _calcOpts.OffhandImbue = (string)comboBoxOffhandImbue.SelectedItem;
                _calcOpts.CalculationToGraph = (string)comboBoxCalculationToGraph.SelectedItem;
                _calcOpts.BaseStatOption = chbBaseStatOption.Checked;
                _calcOpts.Magma = chbMagmaSearing.Checked;
                _calcOpts.UseMana = chbMana.Checked;
                _calcOpts.StatsList[0] = chkStatsStrength.Checked;
                _calcOpts.StatsList[1] = chkStatsAgility.Checked;
                _calcOpts.StatsList[2] = chkStatsAP.Checked;
                _calcOpts.StatsList[3] = chkStatsCrit.Checked;
                _calcOpts.StatsList[4] = chkStatsHit.Checked;
                _calcOpts.StatsList[5] = chkStatsExp.Checked;
                _calcOpts.StatsList[6] = chkStatsHaste.Checked;
                _calcOpts.StatsList[7] = chkStatsArP.Checked;
                _calcOpts.StatsList[8] = chkStatsSP.Checked;
                SavePriorities();
                Character.OnCalculationsInvalidated();
            }
        }

        private void LoadPriorities()
        {
            SetPriorityDefaults(_calcOpts.PriorityList);
            CLBPriorities.Items.Clear();
            foreach (Priority p in _calcOpts.PriorityList.Values)
            {
                CLBPriorities.Items.Add(p, p.Checked);
            }
        }

        private void SavePriorities()
        {
            for (int i = 0; i < CLBPriorities.Items.Count; i++)
            {
                Priority p = (Priority)CLBPriorities.Items[i];
                _calcOpts.SetAbilityPriority(p.AbilityType, p);
            }
        }

        private void btnEnhSim_Click(object sender, EventArgs e)
        {
            Export();
        }

        private void chbBaseStatOption_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                _calcOpts.BaseStatOption = chbBaseStatOption.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbMagmaSearing_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                _calcOpts.Magma = chbMagmaSearing.Checked;
                Character.OnCalculationsInvalidated();
            }
        }

        private void chbMana_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                _calcOpts.UseMana = chbMana.Checked;
                trackbarSRMana.Enabled = _calcOpts.UseMana;
                Character.OnCalculationsInvalidated();
            }
        }

        public void Export()
        {
            if (!_loadingCalculationOptions)
            {
                Enhance.EnhSim simExport = new Enhance.EnhSim(Character, _calcOpts);
                simExport.copyToClipboard();
            }
        }

        private void cmbLength_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                _calcOpts.FightLength = (float)cmbLength.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trackBarAverageLag_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                labelAverageLag.Text = trackBarAverageLag.Value.ToString();
                _calcOpts.AverageLag = trackBarAverageLag.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void trackbarSRMana_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                labelSRMana.Text = trackbarSRMana.Value.ToString();
                _calcOpts.MinManaSR = trackbarSRMana.Value;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxBoss_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                _loadingCalculationOptions = true;
                CalculationsEnhance calcs = new CalculationsEnhance();
                BossHandler boss = bosslist.GetBossFromBetterName(comboBoxBoss.Text);
                _calcOpts.SetBoss(boss);
                CB_TargLvl.Text = _calcOpts.TargetLevel.ToString();
                CB_TargArmor.Text = _calcOpts.TargetArmor.ToString();
                cmbLength.Value = (int)_calcOpts.FightLength;
                CK_InBack.Checked = _calcOpts.InBack;
                CB_InBackPerc.Value = _calcOpts.InBackPerc;

                Stats stats = calcs.GetCharacterStats(Character, null);
                TB_BossInfo.Text = boss.GenInfoString(
                    0, // The Boss' Damage bonuses against you (meaning YOU are debuffed)
                    StatConversion.GetArmorDamageReduction(_calcOpts.TargetLevel, stats.Armor, 0, 0, 0), // Your Armor's resulting Damage Reduction
                    StatConversion.GetDRAvoidanceChance(Character, stats, HitResult.Miss, _calcOpts.TargetLevel), // Your chance for Boss to Miss you
                    StatConversion.GetDRAvoidanceChance(Character, stats, HitResult.Dodge, _calcOpts.TargetLevel), // Your chance Dodge
                    StatConversion.GetDRAvoidanceChance(Character, stats, HitResult.Parry, _calcOpts.TargetLevel), // Your chance Parry
                    0,  // Your Chance to Block
                    0); // How much you Block when you Block
                // Save the new names

                _loadingCalculationOptions = false;
                Character.OnCalculationsInvalidated();
            }
        }

        private void comboBoxMainhandImbue_SelectedIndexChanged(object sender, EventArgs e)
        {
            _calcOpts.MainhandImbue = (string)comboBoxMainhandImbue.SelectedItem;
            Character.OnCalculationsInvalidated();
        }

        private void CK_InBack_CheckedChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                _calcOpts.InBack = CK_InBack.Checked;
                CB_InBackPerc.Enabled = _calcOpts.InBack;
                comboBoxBoss.Text = "Custom";
                Character.OnCalculationsInvalidated();
            }
        }

        private void CB_InBackPerc_ValueChanged(object sender, EventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                _calcOpts.InBackPerc = (int)CB_InBackPerc.Value;
                comboBoxBoss.Text = "Custom";
                Character.OnCalculationsInvalidated();
            }
        }

        private void CLBPriorities_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!_loadingCalculationOptions)
            {
                Priority p = (Priority)CLBPriorities.Items[e.Index];
                if (e.NewValue == CheckState.Checked)
                {
                    p.Checked = true;
                    if (p.AbilityType == EnhanceAbility.MagmaTotem)
                        SetAbilityChecked(EnhanceAbility.SearingTotem, false);
                    if (p.AbilityType == EnhanceAbility.SearingTotem)
                        SetAbilityChecked(EnhanceAbility.MagmaTotem, false);
                }
                else
                {
                    p.Checked = false;
                }
                SavePriorities();
                Character.OnCalculationsInvalidated();
            }
        }

        private void SetAbilityChecked(EnhanceAbility abilityType, bool checkState)
        {
            for (int i = 0; i < CLBPriorities.Items.Count; i++)
            {
                Priority p = (Priority)CLBPriorities.Items[i];
                if (p.AbilityType == abilityType)
                {
                    _loadingCalculationOptions = true;
                    p.Checked = checkState;
                    _calcOpts.SetAbilityPriority(p.AbilityType, p);
                    CLBPriorities.SetItemChecked(i, checkState);
                    _loadingCalculationOptions = false;
                    break;
                }
            }
        }

        private void SetPriorityDefaults(SerializableDictionary<EnhanceAbility, Priority> priorityList)
        {
            if (priorityList.Count == 0)
            {
                int priority = 0;
                priorityList.Add(EnhanceAbility.ShamanisticRage, new Priority("Shamanistic Rage", EnhanceAbility.ShamanisticRage, "Use Shamanistic Rage", true, ++priority, "SR"));
                priorityList.Add(EnhanceAbility.FeralSpirits, new Priority("Feral Spirits", EnhanceAbility.FeralSpirits, "Use Feral Sprirts", true, ++priority, "SW"));
                priorityList.Add(EnhanceAbility.LightningBolt, new Priority("Lightning Bolt on 5 stacks of MW", EnhanceAbility.LightningBolt, "Use Lightning Bolt when you have 5 stacks of Maelstrom Weapon", true, ++priority, "MW5_LB"));
                priorityList.Add(EnhanceAbility.FlameShock, new Priority("Flame Shock", EnhanceAbility.FlameShock, "Use Flame Shock if no Flame Shock debuff on target", true, ++priority, "FS"));
                //       priorityList.Add(new Priority("Earth Shock if SS debuff", EnhanceAbility.EarthShock, "Use Earth Shock if Stormstrike debuff on target", true, ++priority, "ES_SS"));
                //       priorityList.Add(new Priority("Lava Lash if Quaking Earth", EnhanceAbility.LavaLash, "Use Lava Lash if Volcanic Fury buff about to run out", false, ++priority, "LL_QE"));
                priorityList.Add(EnhanceAbility.StormStrike, new Priority("Stormstrike", EnhanceAbility.StormStrike, "Use Stormstrike", true, ++priority, "SS"));
                priorityList.Add(EnhanceAbility.EarthShock, new Priority("Earth Shock", EnhanceAbility.EarthShock, "Use Earth Shock", true, ++priority, "ES"));
                priorityList.Add(EnhanceAbility.LavaLash, new Priority("Lava Lash", EnhanceAbility.LavaLash, "Use Lava Lash", true, ++priority, "LL"));
                priorityList.Add(EnhanceAbility.MagmaTotem, new Priority("Magma Totem", EnhanceAbility.MagmaTotem, "Refresh Magma Totem", true, ++priority, "MT"));
                priorityList.Add(EnhanceAbility.SearingTotem, new Priority("Searing Totem", EnhanceAbility.SearingTotem, "Refresh Searing Totem", false, ++priority, "ST"));
                priorityList.Add(EnhanceAbility.LightningShield, new Priority("Lightning Shield", EnhanceAbility.LightningShield, "Refresh Lightning Shield", true, ++priority, "LS"));
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (CLBPriorities.SelectedIndex > 0)
            {
                _loadingCalculationOptions = true;
                Priority current = (Priority)CLBPriorities.Items[CLBPriorities.SelectedIndex];
                Priority above = (Priority)CLBPriorities.Items[CLBPriorities.SelectedIndex - 1];
                int currentPriority = current.PriorityValue;
                current.PriorityValue = above.PriorityValue;
                above.PriorityValue = currentPriority;
                CLBPriorities.Items[CLBPriorities.SelectedIndex] = above;
                CLBPriorities.Items[CLBPriorities.SelectedIndex - 1] = current;
                CLBPriorities.SetItemChecked(CLBPriorities.SelectedIndex, above.Checked);
                CLBPriorities.SetItemChecked(CLBPriorities.SelectedIndex - 1, current.Checked);
                CLBPriorities.SelectedIndex--;
                _loadingCalculationOptions = false; 
                Character.OnCalculationsInvalidated();
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _loadingCalculationOptions = true;
            Priority current = (Priority)CLBPriorities.Items[CLBPriorities.SelectedIndex];
            Priority below = (Priority)CLBPriorities.Items[CLBPriorities.SelectedIndex + 1];
            int currentPriority = current.PriorityValue;
            current.PriorityValue = below.PriorityValue;
            below.PriorityValue = currentPriority;
            CLBPriorities.Items[CLBPriorities.SelectedIndex] = below;
            CLBPriorities.Items[CLBPriorities.SelectedIndex + 1] = current;
            CLBPriorities.SetItemChecked(CLBPriorities.SelectedIndex, below.Checked);
            CLBPriorities.SetItemChecked(CLBPriorities.SelectedIndex + 1, current.Checked);
            CLBPriorities.SelectedIndex++;
            _loadingCalculationOptions = false;
            Character.OnCalculationsInvalidated();
        }

        private void CLBPriorities_SelectedIndexChanged(object sender, EventArgs e)
        {
            // set butons enabled or disabled based on 
            btnUp.Enabled = CLBPriorities.SelectedIndex > 0;
            btnDown.Enabled = CLBPriorities.SelectedIndex < CLBPriorities.Items.Count - 1;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _loadingCalculationOptions = true;
            btnUp.Enabled = false;
            btnDown.Enabled = false;
            _calcOpts.PriorityList = new SerializableDictionary<EnhanceAbility, Priority>();
            LoadPriorities();
            _loadingCalculationOptions = false;
            Character.OnCalculationsInvalidated();
        }

        private Stats[] BuildStatsList()
        {
            List<Stats> statsList = new List<Stats>();
            if (chkStatsStrength.Checked)
                statsList.Add(new Stats() { Strength = 1 });
            if (chkStatsAgility.Checked)
                statsList.Add(new Stats() { Agility = 1 });
            if (chkStatsAP.Checked)
                statsList.Add(new Stats() { AttackPower = 2 });
            if (chkStatsCrit.Checked)
                statsList.Add(new Stats() { CritRating = 1 });
            if (chkStatsHit.Checked)
                statsList.Add(new Stats() { HitRating = 1 });
            if (chkStatsExp.Checked)
                statsList.Add(new Stats() { ExpertiseRating = 1 });
            if (chkStatsHaste.Checked)
                statsList.Add(new Stats() { HasteRating = 1 });
            if (chkStatsArP.Checked)
                statsList.Add(new Stats() { ArmorPenetrationRating = 1 });
            if (chkStatsSP.Checked)
                statsList.Add(new Stats() { SpellPower = 1.15f });

            return statsList.ToArray();
        }

        private void btnStatsGraph_Click(object sender, EventArgs e)
        {
            Stats[] statsList = BuildStatsList();
            Graph graph = new Graph();
            string explanatoryText = "This graph shows how adding or subtracting\nmultiples of a stat affects your dps.\n\nAt the Zero position is your current dps.\n" +
                         "To the right of the zero vertical is adding stats.\nTo the left of the zero vertical is subtracting stats.\n" +
                         "The vertical axis shows the amount of dps added or lost";
            graph.SetupGraph(Character, statsList, explanatoryText, "DPS", _calcOpts.CalculationToGraph);
            graph.Show();
        }

        private void chkStatsStrength_CheckedChanged(object sender, EventArgs e)
        {
            _calcOpts.StatsList[0] = chkStatsStrength.Checked;
        }

        private void chkStatsAgility_CheckedChanged(object sender, EventArgs e)
        {
            _calcOpts.StatsList[1] = chkStatsAgility.Checked;
        }

        private void chkStatsAP_CheckedChanged(object sender, EventArgs e)
        {
            _calcOpts.StatsList[2] = chkStatsAP.Checked;
        }

        private void chkStatsCrit_CheckedChanged(object sender, EventArgs e)
        {
            _calcOpts.StatsList[3] = chkStatsCrit.Checked;
        }

        private void chkStatsHit_CheckedChanged(object sender, EventArgs e)
        {
            _calcOpts.StatsList[4] = chkStatsHit.Checked;
        }

        private void chkStatsExp_CheckedChanged(object sender, EventArgs e)
        {
            _calcOpts.StatsList[5] = chkStatsExp.Checked;
        }

        private void chkStatsHaste_CheckedChanged(object sender, EventArgs e)
        {
            _calcOpts.StatsList[6] = chkStatsHaste.Checked;
        }

        private void chkStatsArP_CheckedChanged(object sender, EventArgs e)
        {
            _calcOpts.StatsList[7] = chkStatsArP.Checked;
        }

        private void chkStatsSP_CheckedChanged(object sender, EventArgs e)
        {
            _calcOpts.StatsList[8] = chkStatsSP.Checked;
        }

        private void comboBoxCalculationToGraph_SelectedIndexChanged(object sender, EventArgs e)
        {
            _calcOpts.CalculationToGraph = (string)comboBoxCalculationToGraph.SelectedItem;
        }
    }
}
