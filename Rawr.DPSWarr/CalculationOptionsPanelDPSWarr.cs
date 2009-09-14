using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

/* Things to add:
 * 
 * Full Boss Handling System
 * Custom Rotation Priority
 * Target Damage
 * Target Attack Speed
 * Threat Value/Weight
 * Pot Usage (Needs to pull GCDs)
 * Healing Recieved
 * % of fight with mob under 20% HP (activates Execute Spamming)
 * Vigilance Threat pulling
 */

namespace Rawr.DPSWarr {
    public partial class CalculationOptionsPanelDPSWarr : CalculationOptionsPanelBase {
        private bool isLoading = false;
        private bool firstload = true;
        private BossList bosslist = null;
        //private readonly Dictionary<int, string> armorBosses = new Dictionary<int, string>();
        private Dictionary<string, string> FAQStuff = new Dictionary<string, string>();
        public CalculationOptionsPanelDPSWarr() {
            isLoading = true;
            InitializeComponent();
            SetUpFAQ();
            CTL_Maints.ExpandAll();

            if (bosslist == null) { bosslist = new BossList(); }
            if (CB_BossList.Items.Count < 1) { CB_BossList.Items.Add("Custom"); }
            if (CB_BossList.Items.Count < 2) { CB_BossList.Items.AddRange(bosslist.GetBetterBossNamesAsArray()); }

            if (CB_BL_FilterType.Text  == "") { CB_BL_FilterType.Text = "Content"; }

            if (CB_BL_Filter.Items.Count < 1) { CB_BL_Filter.Items.Add("All"); }
            bosslist.GenCalledList(BossList.FilterType.Content, CB_BL_Filter.Text);
            if (CB_BL_Filter.Items.Count < 2) { CB_BL_Filter.Items.AddRange(bosslist.GetFilterListAsArray((BossList.FilterType)(CB_BL_FilterType.SelectedIndex))); }

            if (CB_BossList.Items.Count > 0) { CB_BossList.Items.Clear(); }
            CB_BossList.Items.Add("Custom");
            CB_BossList.Items.AddRange(bosslist.GetBetterBossNamesAsArray());

            CB_Duration.Minimum = 0;
            CB_Duration.Maximum = 60 * 20; // 20 minutes
            CB_MoveTargsTime.Maximum = 60 * 20; // 20 minutes
            isLoading = false;
        }
        private void SetUpFAQ() {
FAQStuff.Add(
"Why is the Mortal Strike talent shown with negative DPS in the Talent Comparison Pane? The ability is doing x DPS.",
@"When the standard rotation abilities for Arms are active (including Slam and Heroic Strike) the large rage consumption of the Mortal Strike Ability tends to overshadow the rage left-over for Heroic Strikes. Basically, if you were to Slam instead of Mortal Strike on every time you would have otherwise, there would be more rage left over to Heroic Strike. In some cases, Rawr sees this as a DPS gain and wants you to drop Mortal Strike. Fully 25 Man raid buffed, Mortal Strike should have a higher DPS value than the rage to Heroic Strikes would provide."
);
FAQStuff.Add(
"Why does X talent/glyph not show any value in the comparison lists?",
@"Many talents cannot be valued by DPS gain or by Survivability Gain (which was recently added). Also, most Prot tree talents are not modeled as they are unnecessary, basically anything beyond 3rd tier. It's also possible that you do not have the Situation setting where the Talent/Glyph would have value. For example, If you are never Stunned, then Iron Will wouldn't have a value."
);
FAQStuff.Add(
"Why does X ability lower my DPS when I check it in the Ability Maintenance Tab?",
@"Abilities may not provide additional DPS, but they do absorb Global Cooldowns (GCDs). If the ability is replacing something that would otherwise cause damage and isn't providing at least a temporary buff, the total DPS will go down. This usually occurs when you check the Maintenance abilities. Commanding Shout doesn't add to DPS but takes GCDs to keep up."
);
FAQStuff.Add(
"Why is it when I run the Optimizer I don't end up hit capped and/or expertise capped? Shouldn't that be automatic?",
@"The optimizer, when run without any requirements, will attempt to find the highest possible Total DPS number. In many cases, this does not include being hit/expertise capped. This is an unfortunate calculational error that has been persistent throughout Rawr.DPSWarr's history. We have made great strides to correct this issue but a few points are still off. To ensure these caps are enforced, add the '% Chance to be Avoided <= 0' requirement before optimizing. You must also consider that in some cases, if another Hit Gem would put you over the cap by a large amount (you need 3 hit but gem would give you 8 so 5 wasted) the optimizer may find that leaving that 3 under the cap and giving an 8 STR gem in its place would be more beneficial overall."
);
FAQStuff.Add(
"Why does my toon do 0 DPS?",
@"There are a couple possible reasons this could occur.
1) You don't have a Main Hand Weapon, all DPS is tied to having a Main Hand Weapon.
2) Your Situational settings on the Fight Info tab are set such that you ave no ability to get any DPS out during the fight."
);
FAQStuff.Add(
"Why does the optimizer try and equip two of my weapon when I only have one?",
@"To restrict it to one item, right-click the item, select Edit then mark the Item as Unique. This will prevent it from putting the item in both MH and OH slots. The same goes for rings, trinkets, etc."
);
FAQStuff.Add(
"Why does the Optimizer sometimes lower my DPS?",
@"The Optimizer operates on a Random Seed method, which is why it works at all. Sometimes it can't use that random seed to find a set that is better than what you are currently wearing."
);
FAQStuff.Add(
"Why does the Optimizer sometimes just rearrange my Gems?",
@"This is a result of a the flaw of logic in the final push that the Optimizer uses, if your total DPS is the same and it was just the Gems that got swapped around, keep your existing set. Astrylian is working on an eventual solution to this problem."
);
FAQStuff.Add(
"Why is my Crit value so low compared to in-game?",
@"Boss level affects your Crit value. Level 83 has about a 4.8% drop, this is mentioned in the Crit Value tooltip."
);
FAQStuff.Add(
"What about <20% Target Health Execute Spamming?",
@"We don't model this yet, sorry."
);
FAQStuff.Add(
"Why do T9 items sometimes show as less value than T8 items (and subsequently T8 to T7)?",
@"Set Bonuses can have a serious impact on DPS, getting that 2nd or 4th piece can mean more or less for your character at specific gear-sets. It could also be a factor of Meta Gem Requirements if you have that active."
);
FAQStuff.Add(
"Why do Blood Frenzy/Savage Combat, Trauma/Mangle, Rampage/Leader of the Pack, Battle Shout/Blessing of Might, Commanding Shout/Blood Pact Buffs sometimes show 0 value or get cleared?",
@"One of the most repeated issue submissions for DPSWarr, this is actually intended functionality. When your character is Maintaining this Buff themselves, we disable the Buff Version so that the Talent can have value instead and we can get a better DPS calculation. We also disable the Buff version to prevent Double-Dipping (getting buff twice, once as Buff and once as Talent).
1) Blood Frenzy/Savage Combat: Disabled on having Blood Frenzy Talent (Arms)
2) Trauma/Mangle: Disabled on having Trauma Talent (Arms)
3) Rampage/Leader of the Pack: Disabled on having Rampage Talent (Fury)
4) Battle Shout/Blessing of Might: Disabled on Maintaining Ability
5) Commanding Shout/Blood Pact: Disabled on Maintaining Ability
6) Presently we do NOT model the following abilities this way: Sunder Armor, Thunder Clap, Demoralizing Shout, Hamstring. Sunder because of the stacking effect we have yet to model and the others because their Buffs are currently not relevant to DPSWarr."
);
FAQStuff.Add(
"Why aren't items with Resilience relevant?",
@"Rawr is for PvE, not PvP."
);
FAQStuff.Add(
"Why are the stats wrong for my x level (non-80) character when I load from Armory?",
@"Rawr is for end-game PvE, meaning you should be level 80. Rawr does not factor things for leveling as there is no point, you will replace the item in a couple of levels anyway and all your raiding gear will end up requiring level 80 to wear."
);
FAQStuff.Add(
"Why can't I select X weapon type or Y Armor Type?",
@"Some weapon types are pointless to factor in, Staves and one handed weapons definitely being the big part of this. Same for Armor, though we can wear cloth, cloth can't physically boost our DPS in any way compared to Plate. Leather and Mail at top end items have a chance to beat out your DPS plate in some circumstances. If you want to enable Leather and Mail you can by use of Refine Types of Items Listed from the Tools menu."
);
            CB_FAQ_Questions.Items.Add("All");
            string[] arr = new string[FAQStuff.Keys.Count];
            FAQStuff.Keys.CopyTo(arr,0);
            CB_FAQ_Questions.Items.AddRange(arr);
            CB_FAQ_Questions.SelectedIndex = 0;
            CB_FAQ_Questions_SelectedIndexChanged(null, null);
        }
        private void CB_FAQ_Questions_SelectedIndexChanged(object sender, EventArgs e) {
            string text = "";
            if (CB_FAQ_Questions.Text == "All") {
                int Iter = 1;
                text += "== CONTENTS ==" + "\r\n";
                foreach (string s in FAQStuff.Keys) {
                    text += Iter.ToString("00") + "Q. " + s + "\r\n"; // Question
                    Iter++;
                } Iter = 1;
                text += "\r\n";
                text += "== READ ON ==" + "\r\n";
                foreach (string s in FAQStuff.Keys) {
                    string a = "invalid";
                    text += Iter.ToString("00") + "Q. " + s + "\r\n"; // Question
                    bool ver = FAQStuff.TryGetValue(s, out a);
                    text += Iter.ToString("00") + "A. " + (ver ? a : "An error occurred calling the string") + "\r\n"; // Answer
                    text += "\r\n" + "\r\n";
                    Iter++;
                } Iter = 1;
                RTB_FAQ.Text = text;
            }else{
                string s = CB_FAQ_Questions.Text;
                string a = "invalid";
                bool ver = FAQStuff.TryGetValue(s, out a);
                text += s + "\r\n";
                text += "\r\n";
                text += (ver ? a : "An error occurred calling the string");
                RTB_FAQ.Text = text;
                RTB_FAQ.SelectAll();
                RTB_FAQ.SelectionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
                RTB_FAQ.Select(0, RTB_FAQ.Text.IndexOf('\n'));
                RTB_FAQ.SelectionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            }
        }
        protected override void LoadCalculationOptions() {
            isLoading = true;
            if (Character != null && Character.CalculationOptions == null) { 
                // If it's broke, make a new one with the defaults
                Character.CalculationOptions = new CalculationOptionsDPSWarr();
                isLoading = true;
            }
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
            CB_BossList.Text                = calcOpts.BossName;
            CB_TargLvl.Text                 = calcOpts.TargetLevel.ToString();
            CB_TargArmor.Text               = calcOpts.TargetArmor.ToString();
            CB_Duration.Value               = (decimal)calcOpts.Duration;
                CB_MoveTargsTime.Maximum    = CB_Duration.Value;
            RB_StanceArms.Checked           = !calcOpts.FuryStance;
            CK_PTRMode.Checked              =  calcOpts.PTRMode;
            CK_HideBadItems.Checked         =  calcOpts.HideBadItems;CalculationsDPSWarr.HidingBadStuff = calcOpts.HideBadItems;ItemCache.OnItemsChanged();
            NUD_SurvScale.Value             = (decimal)calcOpts.SurvScale;
            // Boss Selector
            // Save the new names
            CB_BL_FilterType.Text = calcOpts.FilterType;
            firstload = true;
            isLoading = false;  CB_BL_FilterType_SelectedIndexChanged(null, null); isLoading = true;
            CB_BL_Filter.Text = calcOpts.Filter;
            isLoading = false; CB_BL_Filter_SelectedIndexChanged(null, null); isLoading = true;
            CB_BossList.Text = calcOpts.BossName;
            isLoading = false; CB_BossList_SelectedIndexChanged(null, null); isLoading = true;
            firstload = false;
            // Rotational Changes
            CK_InBack.Checked               = calcOpts.InBack;
                LB_Perc5.Enabled            = calcOpts.InBack;
                CB_InBackPerc.Enabled       = calcOpts.InBack;
                CB_InBackPerc.Value         = calcOpts.InBackPerc;
            CK_MultiTargs.Checked           = calcOpts.MultipleTargets;
                LB_Max.Enabled              = calcOpts.MultipleTargets;
                LB_Perc1.Enabled            = calcOpts.MultipleTargets;
                CB_MultiTargsPerc.Enabled   = calcOpts.MultipleTargets;
                CB_MultiTargsMax.Enabled    = calcOpts.MultipleTargets;
                CB_MultiTargsPerc.Value     = calcOpts.MultipleTargetsPerc;
                CB_MultiTargsMax.Value      = (int)calcOpts.MultipleTargetsMax;
            CK_StunningTargs.Checked        = calcOpts.StunningTargets;
                NUD_StunFreq.Enabled        = calcOpts.StunningTargets;
                NUD_StunDur.Enabled         = calcOpts.StunningTargets;
                LB_Stun0.Enabled            = calcOpts.StunningTargets;
                LB_Stun1.Enabled            = calcOpts.StunningTargets;
                LB_Stun2.Enabled            = calcOpts.StunningTargets;
                NUD_StunFreq.Value          = (int)calcOpts.StunningTargetsFreq;
                NUD_StunDur.Value           = (int)calcOpts.StunningTargetsDur;
            CK_MovingTargs.Checked          = calcOpts.MovingTargets;
                CB_MoveTargsTime.Enabled    = calcOpts.MovingTargets;
                CB_MoveTargsPerc.Enabled    = calcOpts.MovingTargets;
                LB_MoveSec.Enabled          = calcOpts.MovingTargets;
                CB_MoveTargsTime.Value      = (int)calcOpts.MovingTargetsTime;
            // nonfunctional
            CK_DisarmTargs.Checked          = calcOpts.DisarmingTargets;
                CB_DisarmingTargsPerc.Value = calcOpts.DisarmingTargetsPerc;
            // Abilities to Maintain
            CK_Flooring.Checked = calcOpts.AllowFlooring;
            LoadAbilBools(calcOpts);
            // Latency
            CB_Lag.Value   = (int)calcOpts.Lag;
            CB_React.Value = (int)calcOpts.React;
            //
            calcOpts.FuryStance = (Character.WarriorTalents.TitansGrip > 0);
            RB_StanceFury.Checked = calcOpts.FuryStance;
            RB_StanceArms.Checked = !RB_StanceFury.Checked;
            //
            Character.OnCalculationsInvalidated();
            isLoading = false;
        }
        // Boss Handler
        private void CB_BL_FilterType_SelectedIndexChanged(object sender, EventArgs e) {
            if (!isLoading) {
                isLoading = true;
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                // Use Filter Type Box to adjust Filter Box
                if (CB_BL_Filter.Items.Count > 0) { CB_BL_Filter.Items.Clear(); }
                if (CB_BL_Filter.Items.Count < 1) { CB_BL_Filter.Items.Add("All"); }
                CB_BL_Filter.Text = "All";
                BossList.FilterType ftype = (BossList.FilterType)(CB_BL_FilterType.SelectedIndex);
                bosslist.GenCalledList(ftype, CB_BL_Filter.Text);
                CB_BL_Filter.Items.AddRange(bosslist.GetFilterListAsArray(ftype));
                CB_BL_Filter.Text = "All";
                // Now edit the Boss List to the new filtered list of bosses
                if (CB_BossList.Items.Count > 0) { CB_BossList.Items.Clear(); }
                CB_BossList.Items.Add("Custom");
                CB_BossList.Items.AddRange(bosslist.GetBetterBossNamesAsArray());
                CB_BossList.Text = "Custom";
                // Save the new names
                if (!firstload) {
                    calcOpts.FilterType = CB_BL_FilterType.Text;
                    calcOpts.Filter = CB_BL_Filter.Text;
                    calcOpts.BossName = CB_BossList.Text;
                }
                //
                Character.OnCalculationsInvalidated();
                isLoading = false;
            }
        }
        private void CB_BL_Filter_SelectedIndexChanged(object sender, EventArgs e) {
            if (!isLoading) {
                isLoading = true;
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                // Use Filter Type Box to adjust Filter Box
                BossList.FilterType ftype = (BossList.FilterType)(CB_BL_FilterType.SelectedIndex);
                bosslist.GenCalledList(ftype, CB_BL_Filter.Text);
                // Now edit the Boss List to the new filtered list of bosses
                if (CB_BossList.Items.Count > 0) { CB_BossList.Items.Clear(); }
                CB_BossList.Items.Add("Custom");
                CB_BossList.Items.AddRange(bosslist.GetBetterBossNamesAsArray());
                CB_BossList.Text = "Custom";
                // Save the new names
                if (!firstload) {
                    calcOpts.FilterType = CB_BL_FilterType.Text;
                    calcOpts.Filter = CB_BL_Filter.Text;
                    calcOpts.BossName = CB_BossList.Text;
                }
                //
                Character.OnCalculationsInvalidated();
                isLoading = false;
            }
        }
        private void CB_BossList_SelectedIndexChanged(object sender, EventArgs e) {
            if(!isLoading){
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                if (CB_BossList.Text != "Custom") {
                    isLoading = true;
                    // Get Values
                    BossHandler boss = bosslist.GetBossFromBetterName(CB_BossList.Text);
                    calcOpts.TargetLevel = boss.Level;
                    calcOpts.TargetArmor = (int)boss.Armor;
                    calcOpts.Duration = boss.BerserkTimer;
                    calcOpts.InBack = ((calcOpts.InBackPerc = (int)(boss.InBackPerc_Melee * 100f)) != 0);
                    calcOpts.MultipleTargets = ((calcOpts.MultipleTargetsPerc = (int)(boss.MultiTargsPerc * 100f)) > 0);
                    calcOpts.MultipleTargetsMax = Math.Min((float)CB_MultiTargsMax.Maximum, boss.MaxNumTargets);
                    calcOpts.StunningTargets = ((calcOpts.StunningTargetsFreq = (int)(boss.StunningTargsFreq)) <= calcOpts.Duration * 0.99f && calcOpts.StunningTargetsFreq != 0f);
                    calcOpts.StunningTargetsDur = boss.StunningTargsDur;
                    calcOpts.MovingTargets = ((calcOpts.MovingTargetsTime = (int)(boss.MovingTargsTime)) > 0);
                    calcOpts.DisarmingTargets = ((calcOpts.DisarmingTargetsPerc = (int)(boss.DisarmingTargsPerc * 100f)) > 0);

                    // Set Controls to those Values
                    CB_TargLvl.Text          = calcOpts.TargetLevel.ToString();
                    CB_TargArmor.Text        = calcOpts.TargetArmor.ToString();
                    CB_Duration.Value        = (int)calcOpts.Duration;
                    CB_MoveTargsTime.Maximum = CB_Duration.Value;

                    CK_InBack.Checked               = calcOpts.InBack;
                        LB_Perc5.Enabled            = calcOpts.InBack;
                        CB_InBackPerc.Enabled       = calcOpts.InBack;
                        CB_InBackPerc.Value         = calcOpts.InBackPerc;
                    CK_MultiTargs.Checked           = calcOpts.MultipleTargets;
                        LB_Max.Enabled              = calcOpts.MultipleTargets;
                        LB_Perc1.Enabled            = calcOpts.MultipleTargets;
                        CB_MultiTargsPerc.Enabled   = calcOpts.MultipleTargets;
                        CB_MultiTargsMax.Enabled    = calcOpts.MultipleTargets;
                        CB_MultiTargsPerc.Value     = calcOpts.MultipleTargetsPerc;
                        CB_MultiTargsMax.Value      = (int)calcOpts.MultipleTargetsMax;
                    CK_StunningTargs.Checked        = calcOpts.StunningTargets;
                        NUD_StunFreq.Enabled        = calcOpts.StunningTargets;
                        NUD_StunDur.Enabled         = calcOpts.StunningTargets;
                        LB_Stun0.Enabled            = calcOpts.StunningTargets;
                        LB_Stun1.Enabled            = calcOpts.StunningTargets;
                        LB_Stun2.Enabled            = calcOpts.StunningTargets;
                        NUD_StunFreq.Value          = (int)calcOpts.StunningTargetsFreq;
                        NUD_StunDur.Value           = (int)calcOpts.StunningTargetsDur;
                    CK_MovingTargs.Checked          = calcOpts.MovingTargets;
                        CB_MoveTargsTime.Enabled    = calcOpts.MovingTargets;
                        CB_MoveTargsPerc.Enabled    = calcOpts.MovingTargets;
                        LB_MoveSec.Enabled          = calcOpts.MovingTargets;
                        LB_MovePerc.Enabled         = calcOpts.MovingTargets;
                        CB_MoveTargsTime.Value      = (int)calcOpts.MovingTargetsTime;
                        CB_MoveTargsPerc.Value      = (decimal)Math.Floor(calcOpts.MovingTargetsTime / (float)CB_Duration.Value * 100f);
                    
                    TB_BossInfo.Text = boss.GenInfoString();
                    // Save the new names
                    if (!firstload) {
                        calcOpts.FilterType = CB_BL_FilterType.Text;
                        calcOpts.Filter = CB_BL_Filter.Text;
                        calcOpts.BossName = CB_BossList.Text;
                    }
                    isLoading = false;
                }else{
                    isLoading = true;
                    BossHandler boss = new BossHandler();
                    //
                    boss.Name = "Custom";
                    boss.Level = int.Parse(CB_TargLvl.Text);
                    boss.Armor = float.Parse(CB_TargArmor.Text);
                    boss.BerserkTimer = (int)CB_Duration.Value;
                    boss.InBackPerc_Melee = ((float)CB_InBackPerc.Value / 100f);
                    boss.MaxNumTargets = (float)CB_MultiTargsMax.Value;
                    boss.MultiTargsPerc = ((float)CB_MultiTargsPerc.Value / 100f);
                    boss.StunningTargsDur = (float)NUD_StunDur.Value;
                    boss.StunningTargsFreq = (float)NUD_StunFreq.Value;
                    boss.MovingTargsTime = (float)CB_MoveTargsTime.Value;
                    //
                    TB_BossInfo.Text = boss.GenInfoString();
                    isLoading = false;
                }
                Character.OnCalculationsInvalidated();
            }
            isLoading = false;
        }
        // Basics
        private void CK_HideBadItems_CheckedChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.HideBadItems = CK_HideBadItems.Checked;
                CalculationsDPSWarr.HidingBadStuff = calcOpts.HideBadItems;
                ItemCache.OnItemsChanged();
                Character.OnCalculationsInvalidated();
            }
        }
        private void CK_PTRMode_CheckedChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.PTRMode = CK_PTRMode.Checked;
                Character.OnCalculationsInvalidated();
            }
        }
        private void CB_ArmorBosses_SelectedIndexChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                int targetArmor = int.Parse(CB_TargArmor.Text);

                if (Character != null && Character.CalculationOptions != null) {
                    CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                    calcOpts.TargetArmor = targetArmor;
                    Character.OnCalculationsInvalidated();
                }
            }
        }
        private void CB_TargetLevel_SelectedIndexChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                if (Character != null && Character.CalculationOptions != null) {
                    CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                    calcOpts.TargetLevel = int.Parse(CB_TargLvl.Text);
                    Character.OnCalculationsInvalidated();
                }
            }
        }
        private void RB_StanceFury_CheckedChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.FuryStance = RB_StanceFury.Checked;
                CTL_Maints.Nodes[3].Nodes[0].Checked = calcOpts.FuryStance;
                CTL_Maints.Nodes[3].Nodes[1].Checked = !calcOpts.FuryStance;
                Character.OnCalculationsInvalidated();
            }
        }
        private void CB_Duration_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.Duration = (float)CB_Duration.Value;
                CB_MoveTargsTime.Maximum = CB_Duration.Value;
                CB_MoveTargsPerc_ValueChanged(null, null); // This adjusts Moving Time automatically based upon same Percentage
                Character.OnCalculationsInvalidated();
            }
        }
        // Rotational Changes
        private void RotChanges_InBack_ChecksChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                calcOpts.InBack = CK_InBack.Checked;
                CB_InBackPerc.Enabled = calcOpts.InBack;
                LB_Perc5.Enabled = calcOpts.InBack;
                //
                Character.OnCalculationsInvalidated();
            }
        }
        private void RotChanges_Multi_ChecksChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                calcOpts.MultipleTargets = CK_MultiTargs.Checked;
                CB_MultiTargsMax.Enabled = calcOpts.MultipleTargets;
                CB_MultiTargsPerc.Enabled = calcOpts.MultipleTargets;
                LB_Max.Enabled = calcOpts.MultipleTargets;
                LB_Perc1.Enabled = calcOpts.MultipleTargets;
                //
                Character.OnCalculationsInvalidated();
            }
        }
        private void RotChanges_Stun_ChecksChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                bool Checked             = CK_StunningTargs.Checked;
                calcOpts.StunningTargets = Checked;
                LB_Stun0.Enabled         = Checked;
                NUD_StunFreq.Enabled     = Checked;
                LB_Stun1.Enabled         = Checked;
                NUD_StunDur.Enabled      = Checked;
                LB_Stun2.Enabled         = Checked;
                //
                Character.OnCalculationsInvalidated();
            }
        }
        private void RotChanges_Move_ChecksChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                calcOpts.MovingTargets = CK_MovingTargs.Checked;
                CB_MoveTargsTime.Enabled = calcOpts.MovingTargets;
                CB_MoveTargsPerc.Enabled = calcOpts.MovingTargets;
                LB_MoveSec.Enabled = calcOpts.MovingTargets;
                LB_MovePerc.Enabled = calcOpts.MovingTargets;
                //
                Character.OnCalculationsInvalidated();
            }
        }
        private void RotChanges_Disarm_ChecksChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                calcOpts.DisarmingTargets = CK_DisarmTargs.Checked;
                CB_DisarmingTargsPerc.Enabled = calcOpts.DisarmingTargets;
                //
                Character.OnCalculationsInvalidated();
            }
        }
        private void RotChanges_InBack_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                calcOpts.InBackPerc = (int)CB_InBackPerc.Value;
                //
                Character.OnCalculationsInvalidated();
            }
        }
        private void RotChanges_Multi_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                calcOpts.MultipleTargetsPerc = (int)CB_MultiTargsPerc.Value;
                //
                Character.OnCalculationsInvalidated();
            }
        }
        private void RotChanges_MultiMax_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                calcOpts.MultipleTargetsMax = (float)CB_MultiTargsMax.Value;
                //
                Character.OnCalculationsInvalidated();
            }
        }
        private void NUD_StunFreq_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.StunningTargetsFreq = (int)NUD_StunFreq.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void NUD_StunDur_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.StunningTargetsDur = (float)NUD_StunDur.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        private void RotChanges_Move_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                isLoading = true;
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                calcOpts.MovingTargetsTime = (float)CB_MoveTargsTime.Value;
                CB_MoveTargsPerc.Value = (decimal)Math.Floor(calcOpts.MovingTargetsTime / (float)CB_Duration.Value * 100f);
                //
                Character.OnCalculationsInvalidated();
                isLoading = false;
            }
        }
        private void CB_MoveTargsPerc_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                isLoading = true;
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                CB_MoveTargsTime.Value = (decimal)Math.Floor(((float)CB_MoveTargsPerc.Value/100f) * (float)CB_Duration.Value);
                calcOpts.MovingTargetsTime = (float)CB_MoveTargsTime.Value;
                //
                Character.OnCalculationsInvalidated();
                isLoading = false;
            }
        }
        private void RotChanges_Disarm_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CB_BossList.Text = "Custom";
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                //
                calcOpts.DisarmingTargetsPerc = (int)CB_DisarmingTargsPerc.Value;
                //
                Character.OnCalculationsInvalidated();
            }
        }
        // Abilities to Maintain Changes
        private void CK_Flooring_CheckedChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.AllowFlooring = CK_Flooring.Checked;
                Character.OnCalculationsInvalidated();
            }
        }
        private void CTL_Maints_AfterCheck(object sender, TreeViewEventArgs e) {
            if (!isLoading) {
                CTL_Maints.AfterCheck -= new System.Windows.Forms.TreeViewEventHandler(CTL_Maints_AfterCheck);
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                // Work special changes for the tree
                switch (e.Node.Text) {
                    #region Rage Generators
                    case "Rage Generators": {
                        int currentNode = 0, subNode = 0;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked;
                        break;
                    }
                    case "Berserker Rage": {
                        int currentNode = 0;
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Bloodrage": {
                        int currentNode = 0;
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    #endregion
                    #region Maintenance
                    case "Maintenance": {
                        int currentNode = 1, subNode = 0;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[0].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[1].Checked = false; subNode++;// only one of these two can be active at a time
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked;
                        break;
                    }
                    case "Shout Selection": {
                        int currentNode = 1;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[2].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[3].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[4].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        // Handle it's children, Only one of these should ever be active since you can only maintain one shout
                        CTL_Maints.Nodes[currentNode].Nodes[0].Nodes[0].Checked = CTL_Maints.Nodes[currentNode].Nodes[0].Checked;
                        CTL_Maints.Nodes[currentNode].Nodes[0].Nodes[1].Checked = false;
                        break;
                    }
                    case "Battle Shout": {
                        int currentNode = 1, currentSubNode = 0;
                        // Can't have more than one of these two checked, so set the other as false
                        CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked = false;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked) {   // is BS checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked // is CS checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Commanding Shout": {
                        int currentNode = 1, currentSubNode = 0;
                        // Can't have more than one of these two checked, so set the other as false
                        CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked = false;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked) {   // is CS checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked // is BS checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Demoralizing Shout": {
                        int currentNode = 1;
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[2].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[3].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[4].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Sunder Armor": {
                        int currentNode = 1;
                        if (CTL_Maints.Nodes[currentNode].Nodes[2].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[0].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[3].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[4].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Thunder Clap": {
                        int currentNode = 1;
                        if (CTL_Maints.Nodes[currentNode].Nodes[3].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[2].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[0].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[4].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Hamstring": {
                        int currentNode = 1;
                        if (CTL_Maints.Nodes[currentNode].Nodes[4].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[2].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[3].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[0].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    #endregion
                    #region Periodics
                    case "Periodics": {
                        int currentNode = 2, subNode = 0;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked;
                        break;
                    }
                    case "Shattering Throw": {
                        int currentNode = 2;
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[2].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[3].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[4].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Sweeping Strikes": {
                        int currentNode = 2;
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[2].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[3].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[4].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Death Wish": {
                        int currentNode = 2;
                        if (CTL_Maints.Nodes[currentNode].Nodes[2].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[0].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[3].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[4].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Recklessness": {
                        int currentNode = 2;
                        if (CTL_Maints.Nodes[currentNode].Nodes[3].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[2].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[4].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[0].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Enraged Regeneration": {
                        int currentNode = 2;
                        if (CTL_Maints.Nodes[currentNode].Nodes[4].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[2].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[3].Checked &&
                                !CTL_Maints.Nodes[currentNode].Nodes[0].Checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    #endregion
                    #region Damage Dealers
                    case "Damage Dealers": {
                        int currentNode = 3, subNode = 0;
                        if (calcOpts.FuryStance) {
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[0].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[1].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[2].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = false;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[0].Checked = false;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[1].Checked = false;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[2].Checked = false;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[3].Checked = false;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[4].Checked = false;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[5].Checked = false;
                        }else{
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = false;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[0].Checked = false;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[1].Checked = false;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[2].Checked = false; subNode++;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[0].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[1].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[2].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[3].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[4].Checked = CTL_Maints.Nodes[currentNode].Checked;
                            CTL_Maints.Nodes[currentNode].Nodes[subNode].Nodes[5].Checked = CTL_Maints.Nodes[currentNode].Checked;
                        }
                        CTL_Maints.Nodes[currentNode].Nodes[2].Checked = true;
                        break;
                    }
                    #region Fury
                    case "Fury": {
                        int currentNode = 3;
                        // Can't have fury active if you are arms
                        if (!calcOpts.FuryStance) { CTL_Maints.Nodes[currentNode].Nodes[0].Checked = false; }
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        // Handle it's children
                        CTL_Maints.Nodes[currentNode].Nodes[0].Nodes[0].Checked = CTL_Maints.Nodes[currentNode].Nodes[0].Checked;
                        CTL_Maints.Nodes[currentNode].Nodes[0].Nodes[1].Checked = CTL_Maints.Nodes[currentNode].Nodes[0].Checked;
                        CTL_Maints.Nodes[currentNode].Nodes[0].Nodes[2].Checked = CTL_Maints.Nodes[currentNode].Nodes[0].Checked;
                        break;
                    }
                    case "Whirlwind": {
                        int currentNode = 3, currentSubNode = 0;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked) {     // is WW checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked && // is BT checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked    // is BS checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Bloodthirst": {
                        int currentNode = 3, currentSubNode = 0;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked) {     // is BT checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked && // is WW checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked    // is BS checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Bloodsurge": {
                        int currentNode = 3, currentSubNode = 0;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked) {     // is BS checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked && // is BT checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked    // is WW checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    #endregion
                    #region Arms
                    case "Arms": {
                        int currentNode = 3, currentSubNode = 1;
                        // Can't have arms active if you are fury
                        if (calcOpts.FuryStance) { CTL_Maints.Nodes[currentNode].Nodes[1].Checked = false; }
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        // Handle it's children
                        CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked = CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked;
                        CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked = CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked;
                        CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked = CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked;
                        CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[3].Checked = CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked;
                        CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[4].Checked = CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked;
                        CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[5].Checked = CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked;
                        break;
                    }
                    case "Bladestorm": {
                        int currentNode = 3, currentSubNode = 1;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked) {      // is BLS checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked && // is MS checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked && // is RD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[3].Checked && // is OP checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[4].Checked && // is SD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[5].Checked    // is SL checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Mortal Strike": {
                        int currentNode = 3, currentSubNode = 1;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked) {      // is MS checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked && // is BLS checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked && // is RD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[3].Checked && // is OP checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[4].Checked && // is SD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[5].Checked    // is SL checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Rend": {
                        int currentNode = 3, currentSubNode = 1;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked) {      // is RD checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked && // is MS checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked && // is BLS checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[3].Checked && // is OP checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[4].Checked && // is SD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[5].Checked    // is SL checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Overpower": {
                        int currentNode = 3, currentSubNode = 1;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[3].Checked) {      // is OP checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked && // is MS checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked && // is RD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked && // is BLS checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[4].Checked && // is SD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[5].Checked    // is SL checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Sudden Death": {
                        int currentNode = 3, currentSubNode = 1;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[4].Checked) {      // is SD checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked && // is MS checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked && // is RD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[3].Checked && // is OP checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked && // is BLS checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[5].Checked    // is SL checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Slam": {
                        int currentNode = 3, currentSubNode = 1;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[5].Checked) {      // is SL checked
                            CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[1].Checked && // is MS checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[2].Checked && // is RD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[3].Checked && // is OP checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[4].Checked && // is SD checked
                                !CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Nodes[0].Checked    // is BLS checked
                                ) {
                                CTL_Maints.Nodes[currentNode].Nodes[currentSubNode].Checked = false;
                            }
                        }
                        // Handle the parent's parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    #endregion
                    case "<20% Execute Spamming": {
                        int currentNode = 3;
                        // Handle the parent
                        if (CTL_Maints.Nodes[currentNode].Nodes[2].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked
                             && !CTL_Maints.Nodes[currentNode].Nodes[1].Checked)
                            {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    #endregion
                    #region Rage Dumps
                    case "Rage Dumps": {
                        int currentNode = 4, subNode = 0;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked; subNode++;
                        CTL_Maints.Nodes[currentNode].Nodes[subNode].Checked = CTL_Maints.Nodes[currentNode].Checked;
                        break;
                    }
                    case "Cleave": {
                        int currentNode = 4;
                        if (CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    case "Heroic Strike": {
                        int currentNode = 4;
                        if (CTL_Maints.Nodes[currentNode].Nodes[1].Checked) {
                            CTL_Maints.Nodes[currentNode].Checked = true;
                        }else{
                            if (!CTL_Maints.Nodes[currentNode].Nodes[0].Checked) {
                                CTL_Maints.Nodes[currentNode].Checked = false;
                            }
                        }
                        break;
                    }
                    #endregion
                    default: { break; }
                }
                // Assign the new values to the program
                setAbilBools();
                // Run a new dps calc
                Character.OnCalculationsInvalidated();
                CTL_Maints.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(CTL_Maints_AfterCheck);
            }
        }
        private void setAbilBools() {
            CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;

            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._RageGen__]        = CTL_Maints.Nodes[0].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.BerserkerRage_]    = CTL_Maints.Nodes[0].Nodes[0].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bloodrage_]        = CTL_Maints.Nodes[0].Nodes[1].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._Maintenance__]    = CTL_Maints.Nodes[1].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ShoutChoice_]      = CTL_Maints.Nodes[1].Nodes[0].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.BattleShout_]      = CTL_Maints.Nodes[1].Nodes[0].Nodes[0].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.CommandingShout_]  = CTL_Maints.Nodes[1].Nodes[0].Nodes[1].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.DemoralizingShout_]= CTL_Maints.Nodes[1].Nodes[1].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SunderArmor_]      = CTL_Maints.Nodes[1].Nodes[2].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ThunderClap_]      = CTL_Maints.Nodes[1].Nodes[3].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Hamstring_]        = CTL_Maints.Nodes[1].Nodes[4].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._Periodics__]      = CTL_Maints.Nodes[2].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ShatteringThrow_]  = CTL_Maints.Nodes[2].Nodes[0].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SweepingStrikes_]  = CTL_Maints.Nodes[2].Nodes[1].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.DeathWish_]        = CTL_Maints.Nodes[2].Nodes[2].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Recklessness_]     = CTL_Maints.Nodes[2].Nodes[3].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.EnragedRegeneration_]=CTL_Maints.Nodes[2].Nodes[4].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._DamageDealers__]  = CTL_Maints.Nodes[3].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Fury_]             = CTL_Maints.Nodes[3].Nodes[0].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Whirlwind_]        = CTL_Maints.Nodes[3].Nodes[0].Nodes[0].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bloodthirst_]      = CTL_Maints.Nodes[3].Nodes[0].Nodes[1].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bloodsurge_]       = CTL_Maints.Nodes[3].Nodes[0].Nodes[2].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Arms_]             = CTL_Maints.Nodes[3].Nodes[1].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bladestorm_]       = CTL_Maints.Nodes[3].Nodes[1].Nodes[0].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.MortalStrike_]     = CTL_Maints.Nodes[3].Nodes[1].Nodes[1].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_]             = CTL_Maints.Nodes[3].Nodes[1].Nodes[2].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Overpower_]        = CTL_Maints.Nodes[3].Nodes[1].Nodes[3].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SuddenDeath_]      = CTL_Maints.Nodes[3].Nodes[1].Nodes[4].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Slam_]             = CTL_Maints.Nodes[3].Nodes[1].Nodes[5].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_]      = CTL_Maints.Nodes[3].Nodes[2].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._RageDumps__]      = CTL_Maints.Nodes[4].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Cleave_]           = CTL_Maints.Nodes[4].Nodes[0].Checked;
            calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.HeroicStrike_]     = CTL_Maints.Nodes[4].Nodes[1].Checked;
        }
        private void LoadAbilBools(CalculationOptionsDPSWarr calcOpts) {
            CTL_Maints.AfterCheck -= new System.Windows.Forms.TreeViewEventHandler(CTL_Maints_AfterCheck);
            // Bounds Check
            if (calcOpts.Maintenance.GetUpperBound(0) != (int)Rawr.DPSWarr.CalculationOptionsDPSWarr.Maintenances.HeroicStrike_) {
                bool[] newArray = new bool[] {true, true, true, false, false, false, false, false, false, false, false, false, true,
                                              true, true, true, true, true, true, true, true, true, true, true, true, true, true,
                                              true, true, true, true,  true, true };
                calcOpts.Maintenance = newArray;
            }
            //
            CTL_Maints.Nodes[0].Checked                     = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._RageGen__];
            CTL_Maints.Nodes[0].Nodes[0].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.BerserkerRage_];
            CTL_Maints.Nodes[0].Nodes[1].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bloodrage_];
            CTL_Maints.Nodes[1].Checked                     = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._Maintenance__];
            CTL_Maints.Nodes[1].Nodes[0].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ShoutChoice_];
            CTL_Maints.Nodes[1].Nodes[0].Nodes[0].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.BattleShout_];
            CTL_Maints.Nodes[1].Nodes[0].Nodes[1].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.CommandingShout_];
            CTL_Maints.Nodes[1].Nodes[1].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.DemoralizingShout_];
            CTL_Maints.Nodes[1].Nodes[2].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SunderArmor_];
            CTL_Maints.Nodes[1].Nodes[3].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ThunderClap_];
            CTL_Maints.Nodes[1].Nodes[4].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Hamstring_];
            CTL_Maints.Nodes[2].Checked                     = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._Periodics__];
            CTL_Maints.Nodes[2].Nodes[0].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ShatteringThrow_];
            CTL_Maints.Nodes[2].Nodes[1].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SweepingStrikes_];
            CTL_Maints.Nodes[2].Nodes[2].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.DeathWish_];
            CTL_Maints.Nodes[2].Nodes[3].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Recklessness_];
            CTL_Maints.Nodes[2].Nodes[4].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.EnragedRegeneration_];
            CTL_Maints.Nodes[3].Checked                     = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._DamageDealers__];
            CTL_Maints.Nodes[3].Nodes[0].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Fury_];
            CTL_Maints.Nodes[3].Nodes[0].Nodes[0].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Whirlwind_];
            CTL_Maints.Nodes[3].Nodes[0].Nodes[1].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bloodthirst_];
            CTL_Maints.Nodes[3].Nodes[0].Nodes[2].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bloodsurge_];
            CTL_Maints.Nodes[3].Nodes[1].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Arms_];
            CTL_Maints.Nodes[3].Nodes[1].Nodes[0].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Bladestorm_];
            CTL_Maints.Nodes[3].Nodes[1].Nodes[1].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.MortalStrike_];
            CTL_Maints.Nodes[3].Nodes[1].Nodes[2].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Rend_];
            CTL_Maints.Nodes[3].Nodes[1].Nodes[3].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Overpower_];
            CTL_Maints.Nodes[3].Nodes[1].Nodes[4].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.SuddenDeath_];
            CTL_Maints.Nodes[3].Nodes[1].Nodes[5].Checked   = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Slam_];
            CTL_Maints.Nodes[3].Nodes[2].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.ExecuteSpam_];
            CTL_Maints.Nodes[4].Checked                     = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances._RageDumps__];
            CTL_Maints.Nodes[4].Nodes[0].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.Cleave_];
            CTL_Maints.Nodes[4].Nodes[1].Checked            = calcOpts.Maintenance[(int)CalculationOptionsDPSWarr.Maintenances.HeroicStrike_];
            //
            this.CTL_Maints.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.CTL_Maints_AfterCheck);
        }
        // Latency
        private void CB_Latency_ValueChanged(object sender, EventArgs e) {
            if (!isLoading)
            {
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.Lag = (int)CB_Lag.Value;
                calcOpts.React = (int)CB_React.Value;
                Character.OnCalculationsInvalidated();
            }
        }
        // Survival
        private void NUD_SurvScale_ValueChanged(object sender, EventArgs e) {
            if (!isLoading) {
                CalculationOptionsDPSWarr calcOpts = Character.CalculationOptions as CalculationOptionsDPSWarr;
                calcOpts.SurvScale = (float)NUD_SurvScale.Value;
                Character.OnCalculationsInvalidated();
            }
        }
    }
}
