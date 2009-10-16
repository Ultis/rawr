using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Rawr.Enhance
{
    public partial class CalculationOptionsPanelEnhance : CalculationOptionsPanelBase
    {
        /// <summary>This Model's local bosslist</summary>
        private BossList bosslist = null;
        CalculationOptionsEnhance _calcOpts;
        PriorityDisplay _pd;
        
        public CalculationOptionsPanelEnhance()
        {
            InitializeComponent();
            if (bosslist == null) { bosslist = new BossList(); }
            comboBoxBoss.Items.AddRange(bosslist.GetBetterBossNamesAsArray());
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
            cmbLength.Value = (decimal) _calcOpts.FightLength;
            comboBoxMainhandImbue.SelectedItem = _calcOpts.MainhandImbue;
            comboBoxOffhandImbue.SelectedItem = _calcOpts.OffhandImbue;
            chbMagmaSearing.Checked = _calcOpts.Magma;
            chbBaseStatOption.Checked = _calcOpts.BaseStatOption;
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

                _calcOpts.SetBoss(bosslist.GetBossFromBetterName(comboBoxBoss.Text));
                _calcOpts.FightLength = (float)cmbLength.Value;
                _calcOpts.MainhandImbue = (string)comboBoxMainhandImbue.SelectedItem;
                _calcOpts.OffhandImbue = (string)comboBoxOffhandImbue.SelectedItem;

                _calcOpts.BaseStatOption = chbBaseStatOption.Checked;
                _calcOpts.Magma = chbMagmaSearing.Checked;

                Character.OnCalculationsInvalidated();
            }
        }

        private void LoadPriorities()
        {
            _pd = new PriorityDisplay(_calcOpts.Magma);
            CLBPriorities.Items.Clear();
            foreach (PriorityDisplay.Priority p in _pd.getPriorities())
            {
                CLBPriorities.Items.Add(p.PriorityName, p.Checked);
                _calcOpts.SetAbilityPriority(p.AbilityType, CLBPriorities.Items.Count);
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
                // one of the priorities changed. TODO Update the Priority Display object with new status
                List<PriorityDisplay.Priority> priorities = _pd.getPriorities();
                PriorityDisplay.Priority selected = priorities[e.Index];
                selected.SetChecked(e.NewValue);
                if (selected.Checked)
                    _calcOpts.SetAbilityPriority(selected.AbilityType, e.Index + 1); // index is zero based priorities are 1 based
                else
                    _calcOpts.SetAbilityPriority(selected.AbilityType, 0);
                if (selected.AbilityType == EnhanceAbility.MagmaTotem)
                {  // toggle status of fire totems - TODO needs to toggle status of checkbox
                    _calcOpts.SetAbilityPriority(EnhanceAbility.SearingTotem, 0);
                }
                if (selected.AbilityType == EnhanceAbility.SearingTotem)
                {
                    _calcOpts.SetAbilityPriority(EnhanceAbility.MagmaTotem, 0);
                }
                Character.OnCalculationsInvalidated();
            }
        }
    }

    public class PriorityDisplay
    {
        private List<Priority> priorities;

        public PriorityDisplay(bool magma)
        {
            priorities = new List<Priority>();
            priorities.Add(new Priority("Shamanistic Rage", EnhanceAbility.ShamanisticRage, "Use Shamanistic Rage", true));
            priorities.Add(new Priority("Feral Spirits", EnhanceAbility.FeralSpirits, "Use Feral Sprirts", true));
            priorities.Add(new Priority("Lightning Bolt on 5 stacks of MW", EnhanceAbility.LightningBolt, "Use Lightning Bolt when you have 5 stacks of Maelstrom Weapon", true));
            priorities.Add(new Priority("Flame Shock", EnhanceAbility.FlameShock, "Use Flame Shock if no Flame Shock debuff on target", true));
     //       priorities.Add(new Priority("Earth Shock if SS debuff", EnhanceAbility.EarthShock, "Use Earth Shock if Stormstrike debuff on target", true));
     //       priorities.Add(new Priority("Lava Lash if Quaking Earth", EnhanceAbility.LavaLash, "Use Lava Lash if Volcanic Fury buff about to run out", false));
            priorities.Add(new Priority("Stormstrike", EnhanceAbility.StormStrike, "Use Stormstrike", true));
            priorities.Add(new Priority("Earth Shock", EnhanceAbility.EarthShock, "Use Earth Shock", true));
            priorities.Add(new Priority("Lava Lash", EnhanceAbility.LavaLash, "Use Stormstrike", true));
            priorities.Add(new Priority("Magma Totem", EnhanceAbility.MagmaTotem, "Refresh Magma Totem", magma));
            priorities.Add(new Priority("Searing Totem", EnhanceAbility.SearingTotem, "Refresh Searing Totem", ! magma));
            priorities.Add(new Priority("Lightning Shield", EnhanceAbility.LightningShield, "Refresh Lightning Shield", true));
        }

        public List<Priority> getPriorities()
        {
            return priorities;
        }

        public class Priority : TextBox
        {
            private EnhanceAbility _abilityType;
            private string _priorityName;
            private string _description;
            private bool _inUse;

            public Priority (string priorityName, EnhanceAbility abilityType, string description, bool onByDefault)
            {
                _abilityType = abilityType;
                _priorityName = priorityName;
                _description = description;
                _inUse = onByDefault;
                this.Text = priorityName;
            }

            public string PriorityName { get { return _priorityName; } }
            public EnhanceAbility  AbilityType { get { return _abilityType; } }
            public string Description { get { return _description; } }
            public bool Checked { get { return _inUse; } } 

            public void SetChecked(CheckState isChecked)
            {
                _inUse = isChecked == CheckState.Checked;
            }
        }
    }
}
