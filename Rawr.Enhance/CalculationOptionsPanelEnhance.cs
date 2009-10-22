using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

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
            chbMagmaSearing.Checked = _calcOpts.Magma;
            chbMana.Checked = _calcOpts.UseMana;
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
                labelSRMana.Text = trackbarSRMana.Value.ToString();

                _calcOpts.SetBoss(bosslist.GetBossFromBetterName(comboBoxBoss.Text));
                _calcOpts.FightLength = (float)cmbLength.Value;
                _calcOpts.MainhandImbue = (string)comboBoxMainhandImbue.SelectedItem;
                _calcOpts.OffhandImbue = (string)comboBoxOffhandImbue.SelectedItem;

                _calcOpts.BaseStatOption = chbBaseStatOption.Checked;
                _calcOpts.Magma = chbMagmaSearing.Checked;
                _calcOpts.UseMana = chbMana.Checked;

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

        private void btnStatsGraph_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CalculationsEnhance EnhanceCalc = new CalculationsEnhance();
            CharacterCalculationsEnhance baseCalc = EnhanceCalc.GetCharacterCalculations(Character) as CharacterCalculationsEnhance;
            Bitmap bitmap = new Bitmap(800,750);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            float graphHeight = 750f, graphOffset = 400f, graphStep = 3.5f;
            Color[] colors = new Color[] {
                Color.FromArgb(255,202,180,96), // Strength
                Color.FromArgb(255,101,225,240), // Agility
                Color.FromArgb(255,0,4,3), // Attack Power
                Color.FromArgb(255,238,238,30), // Crit Rating
                Color.FromArgb(255,45,112,63), // Hit Rating
                Color.FromArgb(255,121,72,210), //Expertise Rating
                Color.FromArgb(255,217,100,54), // Haste Rating
                Color.FromArgb(255,210,72,195), // Armor Penetration
                Color.FromArgb(255,206,189,191), // Spell Power
            };
            Stats[] statsList = _calcOpts.StatsList;
            float minDpsChange = 0f, maxDpsChange = 0f;
            Point[][] points = new Point[statsList.Length][];
            for (int index = 0; index < statsList.Length; index++)
            {
                Stats newStats = new Stats();
                points[index] = new Point[201];
                newStats.Accumulate(statsList[index], -101);
                for (int count = -100; count <= 100; count++)
                {
                    newStats.Accumulate(statsList[index]);

                    CharacterCalculationsEnhance currentCalc = EnhanceCalc.GetCharacterCalculations(Character, new Item() { Stats = newStats }) as CharacterCalculationsEnhance;
                    float dpsChange = currentCalc.DPSPoints - baseCalc.DPSPoints;
                    points[index][count + 100] = new Point(Convert.ToInt32(graphOffset + count * graphStep), Convert.ToInt32(dpsChange));
                    if (dpsChange < minDpsChange)
                        minDpsChange = dpsChange;
                    if (dpsChange > maxDpsChange)
                        maxDpsChange = dpsChange;
                }
            }
            float DpsVariance = maxDpsChange - minDpsChange;
            if (DpsVariance == 0)
                DpsVariance = 1;
            for (int index = 0; index < statsList.Length; index++)
            {
                for (int count = -100; count <= 100; count++)
                {
                    points[index][count + 100].Y = (int)((maxDpsChange - points[index][count + 100].Y) * (graphHeight-48) / DpsVariance) + 20;
                }
                Brush statBrush = new SolidBrush(colors[index]);
                g.DrawLines(new Pen(statBrush, 3), points[index]);
            }

            #region Graph X Ticks
            float graphStart = graphOffset - 100 * graphStep;
            float graphEnd = graphOffset + 100 * graphStep;
            float graphWidth = graphEnd - graphStart;
            
            float maxScale = 200f;
            float[] ticks = new float[] {(float)Math.Round(graphStart + graphWidth * 0.5f),
							(float)Math.Round(graphStart + graphWidth * 0.75f),
							(float)Math.Round(graphStart + graphWidth * 0.25f),
							(float)Math.Round(graphStart + graphWidth * 0.125f),
							(float)Math.Round(graphStart + graphWidth * 0.375f),
							(float)Math.Round(graphStart + graphWidth * 0.625f),
							(float)Math.Round(graphStart + graphWidth * 0.875f)};
            Pen black200 = new Pen(Color.FromArgb(200, 0, 0, 0));
            Pen black150 = new Pen(Color.FromArgb(150, 0, 0, 0));
            Pen black75 = new Pen(Color.FromArgb(75, 0, 0, 0));
            Pen black50 = new Pen(Color.FromArgb(50, 0, 0, 0));
            Pen black25 = new Pen(Color.FromArgb(25, 0, 0, 0));
            StringFormat formatTick = new StringFormat();
            formatTick.LineAlignment = StringAlignment.Far;
            formatTick.Alignment = StringAlignment.Center;
            Brush black200brush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
            Brush black150brush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
            Brush black75brush = new SolidBrush(Color.FromArgb(75, 0, 0, 0));
            Brush black50brush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
            Brush black25brush = new SolidBrush(Color.FromArgb(25, 0, 0, 0));

            g.DrawLine(black200, graphStart - 4, 20, graphEnd + 4, 20);
            g.DrawLine(black200, graphStart, 16, graphStart, bitmap.Height - 16);
            g.DrawLine(black200, graphEnd, 16, graphEnd, 19);
            g.DrawLine(black200, ticks[0], 16, ticks[0], 19);
            g.DrawLine(black150, ticks[1], 16, ticks[1], 19);
            g.DrawLine(black150, ticks[2], 16, ticks[2], 19);
            g.DrawLine(black75, ticks[3], 16, ticks[3], 19);
            g.DrawLine(black75, ticks[4], 16, ticks[4], 19);
            g.DrawLine(black75, ticks[5], 16, ticks[5], 19);
            g.DrawLine(black75, ticks[6], 16, ticks[6], 19);
            g.DrawLine(black75, graphEnd, 21, graphEnd, bitmap.Height - 16);
            g.DrawLine(black75, ticks[0], 21, ticks[0], bitmap.Height - 16);
            g.DrawLine(black50, ticks[1], 21, ticks[1], bitmap.Height - 16);
            g.DrawLine(black50, ticks[2], 21, ticks[2], bitmap.Height - 16);
            g.DrawLine(black25, ticks[3], 21, ticks[3], bitmap.Height - 16);
            g.DrawLine(black25, ticks[4], 21, ticks[4], bitmap.Height - 16);
            g.DrawLine(black25, ticks[5], 21, ticks[5], bitmap.Height - 16);
            g.DrawLine(black25, ticks[6], 21, ticks[6], bitmap.Height - 16);
            g.DrawLine(black200, graphStart - 4, bitmap.Height - 20, graphEnd + 4, bitmap.Height - 20);

            Font tickFont = new Font("Calibri", 11);
            g.DrawString((-100f).ToString(), tickFont, black200brush, graphStart, 16, formatTick);
            g.DrawString((maxScale - 100f).ToString(), tickFont, black200brush, graphEnd, 16, formatTick);
            g.DrawString((maxScale * 0.5f - 100f).ToString(), tickFont, black200brush, ticks[0], 16, formatTick);
            g.DrawString((maxScale * 0.75f - 100f).ToString(), tickFont, black150brush, ticks[1], 16, formatTick);
            g.DrawString((maxScale * 0.25f - 100f).ToString(), tickFont, black150brush, ticks[2], 16, formatTick);
            g.DrawString((maxScale * 0.125f - 100f).ToString(), tickFont, black75brush, ticks[3], 16, formatTick);
            g.DrawString((maxScale * 0.375f - 100f).ToString(), tickFont, black75brush, ticks[4], 16, formatTick);
            g.DrawString((maxScale * 0.625f - 100f).ToString(), tickFont, black75brush, ticks[5], 16, formatTick);
            g.DrawString((maxScale * 0.875f - 100f).ToString(), tickFont, black75brush, ticks[6], 16, formatTick);

            g.DrawString((-100f).ToString(), tickFont, black200brush, graphStart, bitmap.Height - 16, formatTick);
            g.DrawString((maxScale - 100f).ToString(), tickFont, black200brush, graphEnd, bitmap.Height - 16, formatTick);
            g.DrawString((maxScale * 0.5f - 100f).ToString(), tickFont, black200brush, ticks[0], bitmap.Height - 16, formatTick);
            g.DrawString((maxScale * 0.75f - 100f).ToString(), tickFont, black150brush, ticks[1], bitmap.Height - 16, formatTick);
            g.DrawString((maxScale * 0.25f - 100f).ToString(), tickFont, black150brush, ticks[2], bitmap.Height - 16, formatTick);
            g.DrawString((maxScale * 0.125f - 100f).ToString(), tickFont, black75brush, ticks[3], bitmap.Height - 16, formatTick);
            g.DrawString((maxScale * 0.375f - 100f).ToString(), tickFont, black75brush, ticks[4], bitmap.Height - 16, formatTick);
            g.DrawString((maxScale * 0.625f - 100f).ToString(), tickFont, black75brush, ticks[5], bitmap.Height - 16, formatTick);
            g.DrawString((maxScale * 0.875f - 100f).ToString(), tickFont, black75brush, ticks[6], bitmap.Height - 16, formatTick);
            g.DrawString("Stat Change", tickFont, black200brush, graphWidth / 2 + 50, bitmap.Height, formatTick);
            #endregion

            #region Graph Y ticks   
            Int32 zeroPoint = (int)(maxDpsChange * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black200, graphStart, zeroPoint, graphEnd, zeroPoint);
            formatTick.Alignment = StringAlignment.Near;
            g.DrawString("DPS  0", tickFont, black200brush, graphStart - 50, zeroPoint + 10, formatTick);
            g.DrawString(maxDpsChange.ToString("F1", CultureInfo.InvariantCulture), tickFont, black200brush, graphStart - 50, 30, formatTick);
            g.DrawString(minDpsChange.ToString("F1", CultureInfo.InvariantCulture), tickFont, black200brush, graphStart - 50, bitmap.Height - 12, formatTick);
            float pointY = (int)(maxDpsChange * .75f * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black75, graphStart, pointY, graphEnd, pointY);
            g.DrawString((maxDpsChange * .25f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black75brush, graphStart - 50, pointY + 12, formatTick);
            pointY = (int)(maxDpsChange * .5f * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black150, graphStart, pointY, graphEnd, pointY);
            g.DrawString((maxDpsChange * .5f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black150brush, graphStart - 50, pointY + 12, formatTick);
            pointY = (int)(maxDpsChange * .25f * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black75, graphStart, pointY, graphEnd, pointY);
            g.DrawString((maxDpsChange * .75f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black75brush, graphStart - 50, pointY + 12, formatTick);
            pointY = (int)((maxDpsChange - minDpsChange * .75f) * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black75, graphStart, pointY, graphEnd, pointY);
            g.DrawString((minDpsChange * .75f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black75brush, graphStart - 50, pointY + 12, formatTick);
            pointY = (int)((maxDpsChange - minDpsChange * .5f) * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black150, graphStart, pointY, graphEnd, pointY);
            g.DrawString((minDpsChange * .5f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black150brush, graphStart - 50, pointY + 12, formatTick);
            pointY = (int)((maxDpsChange - minDpsChange * .25f) * (graphHeight - 48) / DpsVariance) + 20;
            g.DrawLine(black75, graphStart, pointY, graphEnd, pointY);
            g.DrawString((minDpsChange * .25f).ToString("F1", CultureInfo.InvariantCulture), tickFont, black75brush, graphStart - 50, pointY + 12, formatTick);
            #endregion

            #region Line Names
            Font nameFont = new Font("Calibri", 14, FontStyle.Bold);
            int nameX = (int) (graphWidth * .6f + graphStart);
            for (int index = 0; index < statsList.Length; index++)
            {
                Brush nameBrush = new SolidBrush(colors[index]);
                int nameY = (int) (bitmap.Height * .6f) + index * 24;
                g.DrawString(statsList[index].ToString(), nameFont, nameBrush, new PointF(nameX, nameY));
            }
            #endregion

            Graph graph = new Graph(bitmap);
            graph.Show();
            Cursor.Current = Cursors.Default;
        }
    }
}
