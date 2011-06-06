using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Xml.Serialization;
using System.Windows;

namespace Rawr.DPSWarr {
    [Rawr.Calculations.RawrModelInfo("DPSWarr", "Ability_Rogue_Ambush", CharacterClass.Warrior)]
    public class CalculationsDPSWarr : CalculationsBase {
        #region Variables and Properties

        #region Gemming Templates
        public override List<GemmingTemplate> DefaultGemmingTemplates
        {
            get
            {
                // Relevant Gem IDs for DPSWarrs
                //               common uncommon rare  jewel |  fills in gaps if it can
                // Red slots
                int[] red_str = { 52081, 52206, 00000, 52255 }; fixArray(red_str);
                int[] red_exp = { 52085, 52230, 00000, 52260 }; fixArray(red_exp);
                int[] red_hit = { 00000, 00000, 00000, 00000 }; fixArray(red_hit);
                int[] red_mst = { 00000, 00000, 00000, 00000 }; fixArray(red_mst);
                int[] red_crt = { 00000, 00000, 00000, 00000 }; fixArray(red_crt);
                int[] red_has = { 00000, 00000, 00000, 00000 }; fixArray(red_has);
                // Orange slots
                int[] org_str = { 52114, 52240, 00000, 00000 }; fixArray(org_str);
                int[] org_exp = { 52118, 52224, 00000, 00000 }; fixArray(org_exp);
                int[] org_hit = { 00000, 00000, 00000, 00000 }; fixArray(org_hit);
                int[] org_mst = { 52114, 52240, 00000, 00000 }; fixArray(org_mst);
                int[] org_crt = { 52108, 52222, 00000, 00000 }; fixArray(org_crt);
                int[] org_has = { 52111, 52214, 00000, 00000 }; fixArray(org_has);
                // Yellow slots
                int[] ylw_str = { 00000, 00000, 00000, 00000 }; fixArray(ylw_str);
                int[] ylw_exp = { 00000, 00000, 00000, 00000 }; fixArray(ylw_exp);
                int[] ylw_hit = { 00000, 00000, 00000, 00000 }; fixArray(ylw_hit);
                int[] ylw_mst = { 52094, 52219, 00000, 52269 }; fixArray(ylw_mst);
                int[] ylw_crt = { 52091, 52241, 00000, 52266 }; fixArray(ylw_crt);
                int[] ylw_has = { 52093, 52232, 00000, 52268 }; fixArray(ylw_has);
                // Green slots
                int[] grn_str = { 00000, 00000, 00000, 00000 }; fixArray(grn_str);
                int[] grn_exp = { 00000, 00000, 00000, 00000 }; fixArray(grn_exp);
                int[] grn_hit = { 52128, 52237, 00000, 00000 }; fixArray(grn_hit);
                int[] grn_mst = { 52126, 52231, 00000, 00000 }; fixArray(grn_mst);
                int[] grn_crt = { 52121, 52223, 00000, 00000 }; fixArray(grn_crt);
                int[] grn_has = { 52124, 52218, 00000, 00000 }; fixArray(grn_has);
                // Blue slots
                int[] blu_str = { 00000, 00000, 00000, 00000 }; fixArray(blu_str);
                int[] blu_exp = { 00000, 00000, 00000, 00000 }; fixArray(blu_exp);
                int[] blu_hit = { 52089, 52235, 00000, 52264 }; fixArray(blu_hit);
                int[] blu_mst = { 00000, 00000, 00000, 00000 }; fixArray(blu_mst);
                int[] blu_crt = { 00000, 00000, 00000, 00000 }; fixArray(blu_crt);
                int[] blu_has = { 00000, 00000, 00000, 00000 }; fixArray(blu_has);
                // Purple slots
                int[] ppl_str = { 52095, 52243, 00000, 00000 }; fixArray(ppl_str);
                int[] ppl_exp = { 52105, 52203, 00000, 00000 }; fixArray(ppl_exp);
                int[] ppl_hit = { 52101, 52213, 00000, 00000 }; fixArray(ppl_hit);
                int[] ppl_mst = { 00000, 00000, 00000, 00000 }; fixArray(ppl_mst);
                int[] ppl_crt = { 00000, 00000, 00000, 00000 }; fixArray(ppl_crt);
                int[] ppl_has = { 00000, 00000, 00000, 00000 }; fixArray(ppl_has);
                // Cogwheels
                int[] cog_exp = { 59489, 59489, 59489, 59489 }; fixArray(cog_exp);
                int[] cog_hit = { 59493, 59493, 59493, 59493 }; fixArray(cog_hit);
                int[] cog_mst = { 59480, 59480, 59480, 59480 }; fixArray(cog_mst);
                int[] cog_crt = { 59478, 59478, 59478, 59478 }; fixArray(cog_crt);
                int[] cog_has = { 59479, 59479, 59479, 59479 }; fixArray(cog_has);
                int[] cog_pry = { 59491, 59491, 59491, 59491 }; fixArray(cog_pry);
                int[] cog_ddg = { 59477, 59477, 59477, 59477 }; fixArray(cog_ddg);
                int[] cog_spr = { 59496, 59496, 59496, 59496 }; fixArray(cog_spr);

                string group; bool enabled;
                List<GemmingTemplate> templates = new List<GemmingTemplate>();

                #region Strength
                enabled = true;
                group = "Strength";
                // Straight
                AddTemplates(templates,
                    red_str, red_str, red_str,
                    red_str, red_str, red_str,
                    /*red_str,*/  group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_str, ylw_str, blu_str,
                    org_str, ppl_str, grn_str,
                    /*red_str,*/  group, enabled);
                #endregion

                #region Expertise
                group = "Expertise";
                enabled = true;
                // Straight
                AddTemplates(templates,
                    red_exp, red_exp, red_exp,
                    red_exp, red_exp, red_exp,
                    /*red_exp,*/ group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_exp, ylw_exp, blu_exp,
                    org_exp, ppl_exp, grn_exp,
                    /*red_exp,*/ group, enabled);
                #endregion

                #region Hit
                group = "Hit";
                enabled = true;
                // Straight
                AddTemplates(templates,
                    blu_hit, blu_hit, blu_hit,
                    blu_hit, blu_hit, blu_hit,
                    /*blu_hit,*/ group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_hit, ylw_hit, blu_hit,
                    org_hit, ppl_hit, grn_hit,
                    /*blu_hit,*/ group, enabled);
                #endregion

                #region Mastery
                enabled = true;
                group = "Mastery";
                // Straight
                AddTemplates(templates,
                    ylw_mst, ylw_mst, ylw_mst,
                    ylw_mst, ylw_mst, ylw_mst,
                    /*ylw_mst,*/ group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_mst, ylw_mst, blu_mst,
                    org_mst, ppl_mst, grn_mst,
                    /*ylw_mst,*/ group, enabled);
                #endregion

                #region Crit
                group = "Crit";
                enabled = false;
                // Straight
                AddTemplates(templates,
                    ylw_crt, ylw_crt, ylw_crt,
                    ylw_crt, ylw_crt, ylw_crt,
                    /*ylw_crt,*/ group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_crt, ylw_crt, blu_crt,
                    org_crt, ppl_crt, grn_crt,
                    /*red_crt,*/ group, enabled);
                #endregion

                #region Haste
                group = "Haste";
                enabled = false;
                // Straight
                AddTemplates(templates,
                    ylw_has, ylw_has, ylw_has,
                    ylw_has, ylw_has, ylw_has,
                    /*ylw_has,*/ group, enabled);
                // Socket Bonus
                AddTemplates(templates,
                    red_has, ylw_has, blu_has,
                    org_has, ppl_has, grn_has,
                    /*red_has,*/ group, enabled);
                #endregion

                #region Cogs
                templates.AddRange(new GemmingTemplate[] {
                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_hit[0], MetaId = 68779, },
                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_mst[0], MetaId = 68779, },
                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_crt[0], MetaId = 68779, },
                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_has[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_pry[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_ddg[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_exp[0], Cogwheel2Id = cog_spr[0], MetaId = 68779, },

                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_mst[0], MetaId = 68779, },
                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_crt[0], MetaId = 68779, },
                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_has[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_pry[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_ddg[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_hit[0], Cogwheel2Id = cog_spr[0], MetaId = 68779, },

                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_crt[0], MetaId = 68779, },
                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_has[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_pry[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_ddg[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_mst[0], Cogwheel2Id = cog_spr[0], MetaId = 68779, },

                        new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_has[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_pry[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_ddg[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_crt[0], Cogwheel2Id = cog_spr[0], MetaId = 68779, },

                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_has[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_ddg[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_pry[0], Cogwheel2Id = cog_spr[0], MetaId = 68779, },

                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_ddg[0], Cogwheel2Id = cog_has[0], MetaId = 68779, },
                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_ddg[0], Cogwheel2Id = cog_spr[0], MetaId = 68779, },

                        //new GemmingTemplate() { Model = "DPSWarr", Group = "Cogwheels", Enabled = false, CogwheelId = cog_spr[0], Cogwheel2Id = cog_has[0], MetaId = 68779, },
                    }
                );
                #endregion

                #region Sorting
                /* This stuff has all kinds of changes, the sort routine won't work the same way
                templates.Sort(new Comparison<GemmingTemplate>(
                    delegate(GemmingTemplate first, GemmingTemplate second) {
                        char[] splitters = {' '};
                        string[] group1 = first.Group.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);
                        string[] group2 = second.Group.Split(splitters, System.StringSplitOptions.RemoveEmptyEntries);
                        int temp = group1[0].CompareTo(group2[0]);
                        if (temp != 0) // they're not the same
                        {
                            if (group1[0] == "Uncommon") return -1; // r|e or r|j
                            if (group2[0] == "Rare" || group1[0] == "Rare") return 1;  // e|r or j|r
                            if (group1[0] == "Jewelcrafter") return 2; // e|j
                            return -1; // j|e
                        }
                        else // they're the same
                        {
                            temp = group1[1].CompareTo(group2[1]);
                            if (temp != 0) {
                                // str > arp > hit > exp > crit-capped
                                switch (group1[1]) {
                                    case "Strength": return -1;
                                    case "Mastery": return (group2[1] == "Strength" ? 1 : -1);
                                    case "Hit": return (group2[1] == "Mastery" ? 1 : -1);
                                    //case "Expertise": return (group2[1] != "Crit-capped" ? 1 : -1);
                                    default: return 1;
                                }
                            } else {
                                int val = first.RedId.CompareTo(second.RedId);
                                if (val != 0) return val;
                                val = first.YellowId.CompareTo(second.YellowId);
                                if (val != 0) return val;
                                val = first.BlueId.CompareTo(second.BlueId);
                                if (val != 0) return val;
                                return first.MetaId.CompareTo(second.MetaId);
                            }
                        }
                }));*/
                #endregion

                return templates;
            }
        }
        private static void fixArray(int[] thearray) {
            if (thearray[0] == 0) return; // Nothing to do, they are all 0
            if (thearray[1] == 0) thearray[1] = thearray[0]; // There was a Green Rarity, but no Blue Rarity
            if (thearray[2] == 0) thearray[2] = thearray[1]; // There was a Blue Rarity (or Green Rarity as set above), but no Purple Rarity
            if (thearray[3] == 0) thearray[3] = thearray[2]; // There was a Purple Rarity (or Blue Rarity/Green Rarity as set above), but no Jewel
        }
        private static void AddTemplates(List<GemmingTemplate> templates, int[] red, int[] ylw, int[] blu, int[] org, int[] prp, int[] grn, /*int[] pris,*/ string group, bool enabled)
        {
            // We are only creating these extra templates until the stupid more blues than reds gets reverted in a patch
            const int chaot = 52291; // 54 Crit, 3% crit dmg
            const int enigm = 52300; // 54 Crit, 10% Snare/Root dur reduc
            const int impsv = 52301; // 54 Crit, 10% Fear dur reduc
            const int fleet = 52289; // 54 Mastery, 8% move speed
            const int revrb = 68779; // 54 Strength, 3% crit dmg
            int[] metas = { chaot, enigm, impsv, fleet, revrb };

            const string groupFormat = "{0} {1}";
            string[] quality = new string[] { "Uncommon", "Rare", "Epic", "Jewelcrafter" };

            for (int m = 0; m < metas.Length; m++)
            {
                for (int j = 0; j < quality.Length; j++)
                {
                    // Check to make sure we're not adding the same gem template twice due to repeating JC gems
                    if (j != 3 || !(red[j] == red[j - 1] && blu[j] == blu[j - 1] && ylw[j] == ylw[j - 1]))
                    {
                        string groupStr = String.Format(groupFormat, quality[j], group);
                        templates.Add(new GemmingTemplate()
                        {
                            Model = "DPSWarr",
                            Group = groupStr,
                            RedId = red[j] != 0 ? red[j] : org[j] != 0 ? org[j] : prp[j],
                            YellowId = ylw[j] != 0 ? ylw[j] : org[j] != 0 ? org[j] : grn[j],
                            BlueId = blu[j] != 0 ? blu[j] : prp[j] != 0 ? prp[j] : grn[j],
                            PrismaticId = red[j] != 0 ? red[j] : ylw[j] != 0 ? ylw[j] : blu[j],
                            MetaId = metas[m],
                            Enabled = (enabled && j == 1)
                        });
                    }
                }
            }
        }
        #endregion

        public override string GetCharacterStatsString(Character character)
        {
            if (character == null) { return ""; }
            StringBuilder stats = new StringBuilder();
            stats.AppendFormat("Character:\t\t{0}@{1}-{2}\r\nRace:\t\t{3}",
                character.Name, character.Region, character.Realm, character.Race);

            char[] splits = {':','*'};
            Dictionary<string,string> dict = GetCharacterCalculations(character, null, false, false, true).GetAsynchronousCharacterDisplayCalculationValues();
            foreach (string s in CharacterDisplayCalculationLabels)
            {
                string[] label = s.Split(splits);
                if (dict.ContainsKey(label[1]))
                {
                    stats.AppendFormat("\r\n{0}:\t\t{1}", label[1], dict[label[1]].Split('*')[0]);
                }
            }
            
            return stats.ToString();
        }
        private string[] _characterDisplayCalculationLabels = null;
        public override string[] CharacterDisplayCalculationLabels {
            get {
                if (_characterDisplayCalculationLabels == null) {
                    _characterDisplayCalculationLabels = new string[] {
#region Base Stats
"Base Stats:Health and Stamina",
"Base Stats:Armor",
"Base Stats:Strength",
"Base Stats:Attack Power",
"Base Stats:Agility",
"Base Stats:Crit",
@"Base Stats:Hit*8.00% chance to miss base for Yellow Attacks (LVL 83 Targ)
Precision 0- 8%-0%=8%=264 Rating soft cap
Precision 1- 8%-1%=7%=230 Rating soft cap
Precision 2- 8%-2%=6%=197 Rating soft cap
Precision 3- 8%-3%=5%=164 Rating soft cap
NOTICE: These ratings numbers will be out of date for Cataclysm",
@"Base Stats:Expertise*Base 6.50% chance to be Dodged (LVL 83 Targ)
X Axis is Weapon Mastery
Y Axis is Strength of Arms
x>| 0  |  1  |  2
0 |213|180|147
1 |197|164|131
2 |180|147|115

0/2 in each the cap is 213 Rating
2/2 in each the cap is 115 Rating

Base 13.75% chance to be Parried (LVL 83 Targ)
Strength of Arms
0 |459
1 |443
2 |426

These numbers to do not include racial bonuses.
NOTICE: These ratings numbers will be out of date for Cataclysm",
"Base Stats:Haste",
"Base Stats:Armor Penetration*Cataclysm no longer has ArP Rating but you can still get ArP % from Talents and Abilities",
"Base Stats:Mastery",
#endregion
            
#region Fury
@"DPS Breakdown (Fury):Description 1*1st Number is per second or per tick
2nd Number is the average damage (factoring mitigation, hit/miss ratio and crits) per hit
3rd Number is number of times activated over fight duration",
"DPS Breakdown (Fury):Bloodsurge",
"DPS Breakdown (Fury):Bloodthirst",
"DPS Breakdown (Fury):Whirlwind",
"DPS Breakdown (Fury):Raging Blow",
#endregion

#region Arms
@"DPS Breakdown (Arms):Description 2*1st Number is per second or per tick
2nd Number is the average damage (factoring mitigation, hit/miss ratio and crits) per hit
3rd Number is number of times activated over fight duration",
"DPS Breakdown (Arms):Shattering Throw",
"DPS Breakdown (Arms):Bladestorm*Bladestorm only uses 1 GCD to activate but it is channeled for a total of 4 GCD's",
"DPS Breakdown (Arms):Mortal Strike",
"DPS Breakdown (Arms):Rend*The Blood and Thunder Talent can refresh Rend so you will only see approximately one activate but you'll still see the full Rend DPS here.",
"DPS Breakdown (Arms):Thunder Clap",
"DPS Breakdown (Arms):Taste for Blood*Perform an Overpower",
"DPS Breakdown (Arms):Overpower",
"DPS Breakdown (Arms):Slam*If this number is zero, it most likely means that your other abilities are proc'g often enough that you are rarely, if ever, having to resort to Slamming your target.",
"DPS Breakdown (Arms):Strikes Of Opportunity*The Arms Mastery Rating based ability",
#endregion

#region Shared
@"DPS Breakdown (Shared):Description 3*1st Number is per second or per tick
2nd Number is the average damage (factoring mitigation, hit/miss ratio and crits) per hit
3rd Number is number of times activated over fight duration",
"DPS Breakdown (Shared):Heroic Throw*" +
@"If you are Glyphed, this will apply your first Sunder Armor stack
If you have Moves set up in the Boss Handler that are more than
a GCD's length, you will use this while running back into place",
"DPS Breakdown (Shared):Colossus Smash*This ability provides the ArP % you see above",
"DPS Breakdown (Shared):Victory Rush*Slam will override this if it does more damage",
"DPS Breakdown (Shared):Heroic Strike",
"DPS Breakdown (Shared):Cleave",
"DPS Breakdown (Shared):Execute*<20% Spamming only",
"DPS Breakdown (Shared):Deep Wounds",
#endregion

#region General
"DPS Breakdown (General):White DPS",
"DPS Breakdown (General):Special DMG Procs*Such as Bandit's Insignia, Hand Mounted Pyro Rocket or Goblin Rocket Belt",
@"DPS Breakdown (General):Total DPS*1st number is total DPS
2nd number is total DMG over Duration",
#endregion

#region Rage Details
"Rage Details:Description 4",
"Rage Details:Rage Above 20%",
"Rage Details:Rage Below 20%",
#endregion

                    };
                }
                return _characterDisplayCalculationLabels;
            }
        }

        private string[] _optimizableCalculationLabels = null;
        public override string[] OptimizableCalculationLabels {
            get {
                if (_optimizableCalculationLabels == null)
                    _optimizableCalculationLabels = new string[] {
                        "Health",
                        "Armor",
                        "Strength",
                        "Attack Power",
                        "Agility",
                        "Crit %",
                        "Haste %",
                        "% Chance to Miss (White)",
                        "% Chance to Miss (Yellow)",
                        "% Chance to be Dodged",
                        "% Chance to be Parried",
                        "% Chance to be Avoided (Yellow/Dodge)",
                    };
                return _optimizableCalculationLabels;
            }
        }

        private Dictionary<string, Color> _subPointNameColors = null;
        public override Dictionary<string, Color> SubPointNameColors {
            get {
                if (_subPointNameColors == null) {
                    _subPointNameColors = new Dictionary<string, Color>();
                    _subPointNameColors.Add("DPS", Colors.Red);
                    _subPointNameColors.Add("Survivability", Colors.Green);
                }
                return _subPointNameColors;
            }
        }

        public override CharacterClass TargetClass { get { return CharacterClass.Warrior; } }
        public override ComparisonCalculationBase CreateNewComparisonCalculation() { return new ComparisonCalculationDPSWarr(); }
        public override CharacterCalculationsBase CreateNewCharacterCalculations() { return new CharacterCalculationsDPSWarr(); }
        public ICalculationOptionsPanel _calculationOptionsPanel = null;
        public override ICalculationOptionsPanel CalculationOptionsPanel { get { return _calculationOptionsPanel ?? (_calculationOptionsPanel = new CalculationOptionsPanelDPSWarr()); } }

        public override ICalculationOptionBase DeserializeDataObject(string xml) {
            CalculationOptionsDPSWarr calcOpts = null;
            StringReader sr = null;
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(CalculationOptionsDPSWarr));
                sr = new StringReader(xml);
                calcOpts = s.Deserialize(sr) as CalculationOptionsDPSWarr;
            }
            finally { sr.Dispose(); }
            return calcOpts;
        }

        #endregion

        #region Relevancy

        private List<ItemType> _relevantItemTypes = null;
        public override List<ItemType> RelevantItemTypes {
            get {
                if (_relevantItemTypes == null) {
                    _relevantItemTypes = new List<ItemType>(new[] {
                        ItemType.None,
                        ItemType.Plate,
                        ItemType.Bow, ItemType.Crossbow, ItemType.Gun, ItemType.Thrown,
                        ItemType.Dagger, ItemType.FistWeapon, ItemType.OneHandMace, ItemType.OneHandSword, ItemType.OneHandAxe,
                        ItemType.Polearm,
                        ItemType.TwoHandMace, ItemType.TwoHandSword, ItemType.TwoHandAxe,
                    });
                }
                return _relevantItemTypes;
            }
        }

        public override bool EnchantFitsInSlot(Enchant enchant, Character character, ItemSlot slot) {
            if (enchant == null) { return false; }
            // Hide the ranged weapon enchants. None of them apply to melee damage at all.
            if (enchant.Slot == ItemSlot.Ranged) { return false; }
            // Disallow Shield enchants, all shield enchants are ItemSlot.OffHand and nothing else is according to Astry
            if (enchant.Slot == ItemSlot.OffHand) { return false; }
            // Allow offhand Enchants for two-handers if toon has Titan's Grip
            // If not, turn off all enchants for the offhand
            if (character != null
                && character.WarriorTalents.TitansGrip > 0
                && enchant.Slot == ItemSlot.TwoHand
                && slot == ItemSlot.OffHand) {
                return true;
            } else  if (character != null
                && character.WarriorTalents.SingleMindedFury > 0
                && enchant.Slot == ItemSlot.OneHand
                && slot == ItemSlot.OffHand) {
                return true;
            } else if (character != null
                && character.WarriorTalents.TitansGrip == 0
                && (enchant.Slot == ItemSlot.TwoHand || enchant.Slot == ItemSlot.OneHand)
                && slot == ItemSlot.OffHand) {
                return false;
            }
            // If all the above is ok, return base version
            return enchant.FitsInSlot(slot);
        }

        public override bool ItemFitsInSlot(Item item, Character character, CharacterSlot slot, bool ignoreUnique) {
            // We need specialized handling due to Titan's Grip and SMT
            if (item == null || character == null) {
                return false;
            }

            // Covers all TG weapons
            if (character.WarriorTalents.TitansGrip > 0) {
                // Polearm can't go in OH, can't go in MH if there's an OH, but can go in MH if there's no OH
                if (item.Type == ItemType.Polearm) {
                    if (slot == CharacterSlot.OffHand || character.OffHand != null) return false;
                    if (slot == CharacterSlot.MainHand) return true;
                    return false;
                }
                // If there's a polearm in the MH, nothing can go in OH
                if (slot == CharacterSlot.OffHand && character.MainHand != null && character.MainHand.Type == ItemType.Polearm) {
                    return false;
                }
                // Else, if it's a 2h weapon it can go in OH or MH
                if (item.Slot == ItemSlot.TwoHand && (slot == CharacterSlot.OffHand || slot == CharacterSlot.MainHand)) return true;
            }

            if (character.WarriorTalents.SingleMindedFury > 0) {
                // If it's a 1h weapon it can go in OH or MH
                if (item.Slot == ItemSlot.OneHand && (slot == CharacterSlot.MainHand || slot == CharacterSlot.OffHand)) return true;
                // If it's a MH weapon it can go in MH only
                if (item.Slot == ItemSlot.MainHand && slot == CharacterSlot.MainHand) return true;
                // If it's an OH weapon it can go in OH only
                if (item.Slot == ItemSlot.OffHand && slot == CharacterSlot.OffHand) return true;
            }

            // Not TG, so can't dual-wield with a 2H in the MH
            if (slot == CharacterSlot.OffHand && character.MainHand != null && character.MainHand.Slot == ItemSlot.TwoHand) {
                return false;
            }

            return base.ItemFitsInSlot(item, character, slot, ignoreUnique);
        }

        private static List<string> _relevantGlyphs = null;
        public override List<string> GetRelevantGlyphs() {
            if (_relevantGlyphs == null) {
                _relevantGlyphs = new List<string>() {
                    #region Prime
                    "Glyph of Bladestorm", //@"Reduces the cooldown on Bladestorm by 15 sec.")]
                    "Glyph of Bloodthirst", //@"Increases the healing your recieve from Bloodthirst ability by 100%.")]
                    //"Glyph of Devastate", //@"Your Devastate ability now applies two stacks of Sunder Armor.")]
                    "Glyph of Mortal Strike", //@"Increases the damage of your Mortal Strike ability by 10%.")]
                    "Glyph of Overpower", //@"Adds a 100% chance to enable your Overpower when your attacks are parried.")]
                    "Glyph of Raging Blow", //@"Increases the critical strike chance of Raging Blow by 5%.")]
                    //"Glyph of Revenge", //@"After using Revenge, your next Heroic Strike costs no rage.")]
                    //"Glyph of Shield Slam", //@"Increases the damage of your Shield Slam by 10%.")]
                    "Glyph of Slam", //@"Increases the critical strike chance of Slam by 5%.")]
                    #endregion
                    #region Major
                    "Glyph of Cleaving", // Increases the number of targets your Cleave hits by 1.
                    "Glyph of Colossus Smash", // Your Colossus Smash refreshes the duration of Sunder Armor stacks on a target.
                    "Glyph of Death Wish", // Death Wish no longer increases damage taken.
                    "Glyph of Heroic Throw", // Your Heroic Throw applies a stack of Sunder Armor.
                    "Glyph of Intercept", // Increases the duration of your Intercept stun by 1 sec.
                    //"Glyph of Intervene", // Increases the number of attacks you intercept for your Intervene target by 1.
                    "Glyph of Long Charge", // Increases the range of your Charge ability by 5 yards.
                    "Glyph of Piercing Howl", // Increases the radius of Piercing Howl by 50%.
                    "Glyph of Rapid Charge", // Reduces the cooldown of your Charge ability by 7%.
                    "Glyph of Resonating Power", // Reduces the rage cost of your Thunder Clap ability by 5.
                    //"Glyph of Shield Wall", // Shield wall now reduces damage taken by 20%, but increases its cooldown by 2 min.
                    //"Glyph of Shockwave", // Reduces the cooldown on Shockwave by 3 sec.
                    //"Glyph of Spell Reflection", // Reduces the cooldown on Spell Reflection by 1 sec.
                    //"Glyph of Sunder Armor", // Your Sunder Armor ability effects a second nearby target.
                    "Glyph of Sweeping Strikes", // Reduces the rage cost of Sweeping Strikes ability by 100%.
                    "Glyph of Thunder Clap", // Increases the radius of your Thunder Clap ability by 2 yards.
                    "Glyph of Victory Rush", // Increases the total healing provided by your Victory Rush by 50%.
                    #endregion
                    #region Minor
                    "Glyph of Battle", //@"Increases the duration of your Battle Shout by 2 min.")]
                    "Glyph of Berserker Rage", //@"Berserker Rage generates 5 rage when used.")]
                    "Glyph of Bloody Healing", //@"Increases the healing your recieve from your Bloodthirst ability by 100%.")]
                    "Glyph of Command", //@"Increases the duration by 2 min and the area of effect by 50% of your Commanding Shout.")]
                    "Glyph of Demoralizing Shout", //@"Increases the duration by 15 sec and area of effect 50% of your Demoralizing Shout.")]
                    "Glyph of Enduring Victory", //@"Increases the window of opportunity in which you can use Victory Rush by 5 sec.")]
                    "Glyph of Furious Sundering", //@"Reduces the cost of Sunder Armor by 50%.")]
                    "Glyph of Intimidating Shout", //@"Targets of your Intimidating Shout no longer move faster when feared.")]
                    #endregion
                };
            }
            return _relevantGlyphs;
        }

        private static bool HidingBadStuff { get { return HidingBadStuff_Def || HidingBadStuff_Spl || HidingBadStuff_PvP; } }
        internal static bool HidingBadStuff_Def { get; set; }
        internal static bool HidingBadStuff_Spl { get; set; }
        internal static bool HidingBadStuff_PvP { get; set; }

        internal static List<Trigger> _RelevantTriggers = null;
        internal static List<Trigger> RelevantTriggers {
            get {
                return _RelevantTriggers ?? (_RelevantTriggers = new List<Trigger>() {
                    // General
                    Trigger.Use,
                    Trigger.MeleeAttack,
                    Trigger.MeleeCrit,
                    Trigger.MeleeHit,
                    Trigger.WhiteAttack,
                    Trigger.WhiteCrit,
                    Trigger.WhiteHit,
                    Trigger.PhysicalCrit,
                    Trigger.PhysicalHit,
                    Trigger.PhysicalAttack,
                    Trigger.DoTTick,
                    Trigger.DamageDone,
                    Trigger.DamageTaken,
                    Trigger.DamageAvoided,
                    Trigger.DamageOrHealingDone,
                    // Special
                    Trigger.DarkIntentCriticalPeriodicDamageOrHealing,
                    // Warrior Specific
                    Trigger.HSorSLHit,
                    Trigger.ColossusSmashHit,
                    Trigger.ExecuteHit,
                    Trigger.OPorRBAttack,
                    Trigger.MortalStrikeCrit,
                    Trigger.MortalStrikeHit,
                });
            }
        }

        public override Stats GetRelevantStats(Stats stats) {
            if (stats == null) { return new Stats(); }
            Stats relevantStats = new Stats() {
                #region Wanted Stats
                // Base Stats
                Strength = stats.Strength,
                Agility = stats.Agility,
                AttackPower = stats.AttackPower,
                // Ratings
                CritRating = stats.CritRating,
                HitRating = stats.HitRating,
                HasteRating = stats.HasteRating,
                ExpertiseRating = stats.ExpertiseRating,
                MasteryRating = stats.MasteryRating,
                // Bonuses
                WeaponDamage = stats.WeaponDamage,
                ArmorPenetration = stats.ArmorPenetration,
                TargetArmorReduction = stats.TargetArmorReduction,
                PhysicalCrit = stats.PhysicalCrit,
                PhysicalHaste = stats.PhysicalHaste,
                PhysicalHit = stats.PhysicalHit,
                SpellHit = stats.SpellHit,
                // Boss Handler
                MovementSpeed = stats.MovementSpeed,
                StunDurReduc = stats.StunDurReduc,
                SnareRootDurReduc = stats.SnareRootDurReduc,
                FearDurReduc = stats.FearDurReduc,
                DisarmDurReduc = stats.DisarmDurReduc,
                // Target Debuffs
                BossAttackSpeedReductionMultiplier = stats.BossAttackSpeedReductionMultiplier,
                // Procs
                DarkmoonCardDeathProc = stats.DarkmoonCardDeathProc,
                HighestStat = stats.HighestStat,
                HighestSecondaryStat = stats.HighestSecondaryStat,
                Paragon = stats.Paragon,
                ManaorEquivRestore = stats.ManaorEquivRestore,
                // Damage Procs
                ShadowDamage = stats.ShadowDamage,
                ArcaneDamage = stats.ArcaneDamage,
                HolyDamage = stats.HolyDamage,
                NatureDamage = stats.NatureDamage,
                FrostDamage = stats.FrostDamage,
                FireDamage = stats.FireDamage,
                BonusShadowDamageMultiplier = stats.BonusShadowDamageMultiplier,
                BonusArcaneDamageMultiplier = stats.BonusArcaneDamageMultiplier,
                BonusHolyDamageMultiplier = stats.BonusHolyDamageMultiplier,
                BonusNatureDamageMultiplier = stats.BonusNatureDamageMultiplier,
                BonusFrostDamageMultiplier = stats.BonusFrostDamageMultiplier,
                BonusFireDamageMultiplier = stats.BonusFireDamageMultiplier,
                // Multipliers
                BonusAgilityMultiplier = stats.BonusAgilityMultiplier,
                BonusStrengthMultiplier = stats.BonusStrengthMultiplier,
                BonusAttackPowerMultiplier = stats.BonusAttackPowerMultiplier,
                BonusBleedDamageMultiplier = stats.BonusBleedDamageMultiplier,
                BonusDamageMultiplier = stats.BonusDamageMultiplier,
                BonusWhiteDamageMultiplier = stats.BonusWhiteDamageMultiplier,
                BonusPhysicalDamageMultiplier = stats.BonusPhysicalDamageMultiplier,
                BonusCritDamageMultiplier = stats.BonusCritDamageMultiplier,
                BonusCritChance = stats.BonusCritChance,
                BonusPeriodicDamageMultiplier = stats.BonusPeriodicDamageMultiplier,
                // Set Bonuses
                // Special
                BonusRageGen = stats.BonusRageGen,
                #endregion
                #region Survivability Stats
                Stamina = stats.Stamina,
                Health = stats.Health,
                BonusStaminaMultiplier = stats.BonusStaminaMultiplier,
                BonusHealthMultiplier = stats.BonusHealthMultiplier,
                HealthRestore = stats.HealthRestore,
                HealthRestoreFromMaxHealth = stats.HealthRestoreFromMaxHealth,
                Armor = stats.Armor,
                BonusArmor = stats.BonusArmor,
                BaseArmorMultiplier = stats.BaseArmorMultiplier,
                BonusArmorMultiplier = stats.BonusArmorMultiplier,
                DamageTakenReductionMultiplier = stats.DamageTakenReductionMultiplier,
                BossPhysicalDamageDealtReductionMultiplier = stats.BossPhysicalDamageDealtReductionMultiplier,
                BonusHealingDoneMultiplier = stats.BonusHealingDoneMultiplier, // not really rel but want it if it's available on something else
                #endregion
            };
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (RelevantTriggers.Contains(effect.Trigger) && (HasRelevantStats(effect.Stats) || HasSurvivabilityStats(effect.Stats)))
                {
                    relevantStats.AddSpecialEffect(effect);
                }
            }
            return relevantStats;
        }
        public override bool HasRelevantStats(Stats stats) {
            bool relevant = HasWantedStats(stats) && !HasIgnoreStats(stats);
            return relevant;
        }

        private bool HasWantedStats(Stats stats) {
            // Base Stats
            if (stats.Strength != 0) { return true; }
            if (stats.Agility != 0) { return true; }
            if (stats.AttackPower != 0) { return true; }
            // Ratings
            if (stats.CritRating != 0) { return true; }
            if (stats.HitRating != 0) { return true; }
            if (stats.HasteRating != 0) { return true; }
            if (stats.ExpertiseRating != 0) { return true; }
            if (stats.MasteryRating != 0) { return true; }
            // Bonuses
            if (stats.WeaponDamage != 0) { return true; }
            if (stats.ArmorPenetration != 0) { return true; }
            if (stats.TargetArmorReduction != 0) { return true; }
            if (stats.PhysicalCrit != 0) { return true; }
            if (stats.PhysicalHaste != 0) { return true; }
            if (stats.PhysicalHit != 0) { return true; }
            if (stats.SpellHit != 0) { return true; } // used for TClap/Demo Shout maintenance
            // Boss Handler
            if (stats.MovementSpeed != 0) { return true; }
            if (stats.StunDurReduc != 0) { return true; }
            if (stats.SnareRootDurReduc != 0) { return true; }
            if (stats.FearDurReduc != 0) { return true; }
            if (stats.DisarmDurReduc != 0) { return true; }
            // Target Debuffs
            if (stats.BossAttackSpeedReductionMultiplier != 0) { return true; }
            // Procs
            if (stats.DarkmoonCardDeathProc != 0) { return true; }
            if (stats.HighestStat != 0) { return true; }
            if (stats.HighestSecondaryStat != 0) { return true; }
            if (stats.Paragon != 0) { return true; }
            if (stats.ManaorEquivRestore != 0) { return true; }
            // Damage Procs
            if (stats.ShadowDamage != 0) { return true; }
            if (stats.ArcaneDamage != 0) { return true; }
            if (stats.HolyDamage != 0) { return true; }
            if (stats.NatureDamage != 0) { return true; }
            if (stats.FrostDamage != 0) { return true; }
            if (stats.FireDamage != 0) { return true; }
            if (stats.BonusShadowDamageMultiplier != 0) { return true; }
            if (stats.BonusArcaneDamageMultiplier != 0) { return true; }
            if (stats.BonusHolyDamageMultiplier != 0) { return true; }
            if (stats.BonusNatureDamageMultiplier != 0) { return true; }
            if (stats.BonusFrostDamageMultiplier != 0) { return true; }
            if (stats.BonusFireDamageMultiplier != 0) { return true; }
            // Multipliers
            if (stats.BonusAgilityMultiplier != 0) { return true; }
            if (stats.BonusStrengthMultiplier != 0) { return true; }
            if (stats.BonusAttackPowerMultiplier != 0) { return true; }
            if (stats.BonusBleedDamageMultiplier != 0) { return true; }
            if (stats.BonusDamageMultiplier != 0) { return true; }
            if (stats.BonusWhiteDamageMultiplier != 0) { return true; }
            if (stats.BonusPhysicalDamageMultiplier != 0) { return true; }
            if (stats.BonusCritDamageMultiplier != 0) { return true; }
            if (stats.BonusCritChance != 0) { return true; }
            if (stats.BonusPeriodicDamageMultiplier != 0) { return true; }
            // Set Bonuses
            // Special
            if (stats.BonusRageGen != 0) { return true; }
            // Special Effects
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (RelevantTriggers.Contains(effect.Trigger) && HasRelevantStats(effect.Stats)) {
                    return true;
                }
            }
            return false;
        }

        private bool HasSurvivabilityStats(Stats stats) {
            // Health Base
            if (stats.Stamina != 0) { return true; }
            if (stats.Health != 0) { return true; }
            if (stats.BonusStaminaMultiplier != 0) { return true; }
            if (stats.BonusHealthMultiplier != 0) { return true; }
            // Health Regen
            if (stats.HealthRestore != 0) { return true; }
            if (stats.HealthRestoreFromMaxHealth != 0) { return true; }
            // Armor
            if (stats.Armor != 0) { return true; }
            if (stats.BonusArmor != 0) { return true; }
            if (stats.BaseArmorMultiplier != 0) { return true; }
            if (stats.BonusArmorMultiplier != 0) { return true; }
            // Multipliers
            if (stats.DamageTakenReductionMultiplier != 0) { return true; }
            // Target Debuffs
            if (stats.BossPhysicalDamageDealtReductionMultiplier != 0) { return true; }
            // Special Effects
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                if (RelevantTriggers.Contains(effect.Trigger) && HasSurvivabilityStats(effect.Stats)) {
                    return true;
                }
            }
            return false;
        }

        private bool HasIgnoreStats(Stats stats) {
            if (!HidingBadStuff) { return false; }

            // Remove Spellcasting Stuff
            if (HidingBadStuff_Spl) {
                if (stats.Mp5 != 0) { return true; }
                if (stats.SpellPower != 0) { return true; }
                if (stats.Mana != 0) { return true; }
                if (stats.ManaRestore != 0) { return true; }
                if (stats.Spirit != 0) { return true; }
                if (stats.Intellect != 0) { return true; }
                if (stats.BonusSpiritMultiplier != 0) { return true; }
                if (stats.BonusIntellectMultiplier != 0) { return true; }
                if (stats.SpellPenetration != 0) { return true; }
                if (stats.BonusManaMultiplier != 0) { return true; }
            }
            // Remove Defensive Stuff
            if (HidingBadStuff_Def) {
                if (stats.Dodge != 0) { return true; }
                if (stats.Parry != 0) { return true; }
                if (stats.DodgeRating != 0) { return true; }
                if (stats.ParryRating != 0) { return true; }
                if (stats.BlockRating != 0) { return true; }
                if (stats.Block != 0) { return true; }
                if (stats.SpellReflectChance != 0) { return true; }
            }
            // Remove PvP Items
            if (HidingBadStuff_PvP) {
                if (stats.Resilience != 0) { return true; }
            }
            // Special Effects
            foreach (SpecialEffect effect in stats.SpecialEffects()) {
                // The effect doesn't have a relevant trigger
                if (!RelevantTriggers.Contains(effect.Trigger)) { return true; }
                // The Effect has Ignore Stats
                if (HasIgnoreStats(effect.Stats)) { return true; }
            }
            return false;
        }

        public override bool IsItemRelevant(Item item) {
            if (item == null) { return false; }
            /*if ( // Manual override for +X to all Stats gems
                   item.Id == 49110 // Nightmare Tear
                || item.Id == 42702 // Enchanted Tear
                || item.Id == 42701 // Enchanted Pearl
                || item.Id == 34143 // Chromatic Sphere
                ) {
                return true;
                //}else if (item.Type == ItemType.Polearm && 
            } else*/ {
                Stats stats = item.Stats;
                bool wantedStats = HasWantedStats(stats);
                bool survstats = HasSurvivabilityStats(stats);
                bool ignoreStats = HasIgnoreStats(stats);
                return (wantedStats || survstats) && !ignoreStats && base.IsItemRelevant(item);
            }
        }

        public override bool IsEnchantRelevant(Enchant enchant, Character character) {
            if (enchant == null || character == null) { return false; }
            return 
                IsEnchantAllowedForClass(enchant, character.Class) && 
                IsProfEnchantRelevant(enchant, character) && 
                (HasWantedStats(enchant.Stats) || 
                    (HasSurvivabilityStats(enchant.Stats) && !HasIgnoreStats(enchant.Stats)));
        }

        public override bool IsBuffRelevant(Buff buff, Character character) {
            if (buff == null) { return false; }
            // Force some buffs to active
            if (buff.SpellId == 53909 // Wild Magic Potion
                || buff.SpellId == 28494 // Insane Strength Potion
                || buff.SpellId == 22738 // Disarm Duration Reduction (Old PvP Set Bonuses)
                || buff.SpellId == 90293  // T11 2P
                || buff.SpellId == 90295) // T11 4P
            { return true; }
            // Force some buffs to go away
            else if (!buff.AllowedClasses.Contains(CharacterClass.Warrior))
            { return false; }
            else if (character != null && Rawr.Properties.GeneralSettings.Default.HideProfEnchants && !character.HasProfession(buff.Professions))
            { return false; }
            //
            bool haswantedStats = HasWantedStats(buff.Stats);
            bool hassurvStats = HasSurvivabilityStats(buff.Stats);
            bool hasbadstats = HasIgnoreStats(buff.Stats);
            bool retVal = haswantedStats || (hassurvStats && !hasbadstats);
            return retVal;
        }
        public Base.StatsWarrior GetBuffsStats(DPSWarrCharacter dpswarchar)
        {
            List<Buff> removedBuffs = new List<Buff>();
            List<Buff> addedBuffs = new List<Buff>();

            List<Buff> buffGroup = new List<Buff>();
            #region Maintenance Auto-Fixing
            // Removes the Sunder Armor if you are maintaining it yourself
            // Also removes Acid Spit and Expose Armor
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (dpswarchar.CalcOpts.M_SunderArmor) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Sunder Armor"));
                buffGroup.Add(Buff.GetBuffByName("Expose Armor"));
                buffGroup.Add(Buff.GetBuffByName("Faerie Fire"));
                buffGroup.Add(Buff.GetBuffByName("Corrosive Spit"));
                buffGroup.Add(Buff.GetBuffByName("Tear Armor"));
                MaintBuffHelper(buffGroup, dpswarchar.Char, removedBuffs);
            }

            // Removes the Shattering Throw Buff if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (dpswarchar.CalcOpts.M_ShatteringThrow) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Shattering Throw"));
                MaintBuffHelper(buffGroup, dpswarchar.Char, removedBuffs);
            }

            // Removes the Thunder Clap & Improved Buffs if you are maintaining it yourself
            // Also removes Judgements of the Just, Infected Wounds, Frost Fever, Improved Icy Touch
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (dpswarchar.CalcOpts.M_ThunderClap) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Thunder Clap"));
                buffGroup.Add(Buff.GetBuffByName("Frost Fever"));
                buffGroup.Add(Buff.GetBuffByName("Judgements of the Just"));
                buffGroup.Add(Buff.GetBuffByName("Infected Wounds"));
                MaintBuffHelper(buffGroup, dpswarchar.Char, removedBuffs);
            }

            // Removes the Demoralizing Shout & Improved Buffs if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (dpswarchar.CalcOpts.M_DemoralizingShout) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Demoralizing Shout"));
                buffGroup.Add(Buff.GetBuffByName("Improved Demoralizing Shout"));
                MaintBuffHelper(buffGroup, dpswarchar.Char, removedBuffs);
            }

            // Removes the Battle Shout & Commanding Presence Buffs if you are maintaining it yourself
            // Also removes their equivalent of Blessing of Might (+Improved)
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (dpswarchar.CalcOpts.M_BattleShout) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Battle Shout"));
                buffGroup.Add(Buff.GetBuffByName("Strength of Earth Totem"));
                buffGroup.Add(Buff.GetBuffByName("Horn of Winter"));
                buffGroup.Add(Buff.GetBuffByName("Roar of Courage"));
                MaintBuffHelper(buffGroup, dpswarchar.Char, removedBuffs);
            }

            // Removes the Commanding Shout & Commanding Presence Buffs if you are maintaining it yourself
            // Also removes their equivalent of Blood Pact (+Improved Imp)
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (dpswarchar.CalcOpts.M_CommandingShout) {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Commanding Shout"));
                buffGroup.Add(Buff.GetBuffByName("Power Word: Fortitude"));
                buffGroup.Add(Buff.GetBuffByName("Quiraji Fortitude"));
                buffGroup.Add(Buff.GetBuffByName("Blood Pact"));
                MaintBuffHelper(buffGroup, dpswarchar.Char, removedBuffs);
            }
            #endregion

            #region Passive Ability Auto-Fixing
            // Removes the Blood Frenzy Buff and it's equivalent of Savage Combat if you are maintaining it yourself
            // Cata also has BF giving what Trauma used to
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (dpswarchar.Char.WarriorTalents.BloodFrenzy > 0)
            {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Trauma"));
                buffGroup.Add(Buff.GetBuffByName("Mangle"));
                buffGroup.Add(Buff.GetBuffByName("Hemorrhage"));
                buffGroup.Add(Buff.GetBuffByName("Tendon Rip"));
                buffGroup.Add(Buff.GetBuffByName("Gore"));
                buffGroup.Add(Buff.GetBuffByName("Stampede"));
                //
                buffGroup.Add(Buff.GetBuffByName("Blood Frenzy"));
                buffGroup.Add(Buff.GetBuffByName("Savage Combat"));
                buffGroup.Add(Buff.GetBuffByName("Brittle Bones"));
                buffGroup.Add(Buff.GetBuffByName("Ravage"));
                buffGroup.Add(Buff.GetBuffByName("Acid Spit"));
                MaintBuffHelper(buffGroup, dpswarchar.Char, removedBuffs);
            }

            // Removes the Rampage Buff and it's equivalent of Leader of the Pack if you are maintaining it yourself
            // We are now calculating this internally for better accuracy and to provide value to relevant talents
            if (dpswarchar.Char.WarriorTalents.Rampage > 0 && dpswarchar.CombatFactors.FuryStance)
            {
                buffGroup.Clear();
                buffGroup.Add(Buff.GetBuffByName("Rampage"));
                buffGroup.Add(Buff.GetBuffByName("Leader of the Pack"));
                buffGroup.Add(Buff.GetBuffByName("Honor Among Thieves"));
                buffGroup.Add(Buff.GetBuffByName("Elemental Oath"));
                buffGroup.Add(Buff.GetBuffByName("Furious Howl"));
                buffGroup.Add(Buff.GetBuffByName("Terrifying Roar"));
                MaintBuffHelper(buffGroup, dpswarchar.Char, removedBuffs);
            }
            #endregion

            Base.StatsWarrior statsBuffs = new Base.StatsWarrior();
            statsBuffs.Accumulate(GetBuffsStats(dpswarchar.Char.ActiveBuffs));
            AccumulateSetBonusStats(statsBuffs, dpswarchar.Char.SetBonusCount);

            if (dpswarchar.Char.ActiveBuffs.Find<Buff>(x => x.SpellId == 22738) != null) {
                statsBuffs.BonusWarrior_PvP_4P_InterceptCDReduc = 5f;
            }
            /*if (dpswarchar.Char.ActiveBuffs.Find<Buff>(x => x.SpellId == 70854) != null) {
                statsBuffs.BonusWarrior_PvP_4P_InterceptCDReduc = 5f;
            }
            if (dpswarchar.Char.ActiveBuffs.Find<Buff>(x => x.SpellId == 70847) != null) {
                statsBuffs.BonusWarrior_PvP_4P_InterceptCDReduc = 5f;
            }*/
            int T11count;
            dpswarchar.Char.SetBonusCount.TryGetValue("Earthen Warplate", out T11count);
            if (T11count >= 2) {//dpswarchar.Char.ActiveBuffs.Find<Buff>(x => x.SpellId == 90293) != null) {
                statsBuffs.BonusMortalStrikeDamageMultiplier = 0.05f;
                statsBuffs.BonusBloodthirstDamageMultiplier = 0.05f;
            }
            if (T11count >= 4) {//dpswarchar.Char.ActiveBuffs.Find<Buff>(x => x.SpellId == 90295) != null) {
                statsBuffs.AddSpecialEffect(new SpecialEffect(Trigger.OPorRBAttack,
                    new Stats() { BonusAttackPowerMultiplier = 0.01f, },
                    30, 0, 1f, 3));
            }
            
            foreach (Buff b in removedBuffs) { dpswarchar.Char.ActiveBuffsAdd(b); }
            foreach (Buff b in addedBuffs) { dpswarchar.Char.ActiveBuffs.Remove(b); }

            return statsBuffs;
        }
        private static void MaintBuffHelper(List<Buff> buffGroup, Character character, List<Buff> removedBuffs)
        {
            foreach (Buff b in buffGroup) {
                if (character.ActiveBuffs.Remove(b)) { removedBuffs.Add(b); }
            }
        }

        public override void SetDefaults(Character character) {
            // Need to be behind boss
            character.BossOptions.InBack = true;
            character.BossOptions.InBackPerc_Melee = 1.00d;
        }

        public override bool IncludeOffHandInCalculations(Character character) {
            if (character == null || character.OffHand == null) { return false; }
            WarriorTalents talents = (WarriorTalents)character.CurrentTalents;
            if (talents.TitansGrip > 0 || talents.SingleMindedFury > 0) {
                return true;
            } else { // if (character.MainHand.Slot != ItemSlot.TwoHand)
                return base.IncludeOffHandInCalculations(character);
            }
            //return false;
        }

        #endregion

        #region Special Comparison Charts
        private string[] _customChartNames = null;
        public override string[] CustomChartNames {
            get {
                if (_customChartNames == null) {
                    _customChartNames = new string[] {
                        "Ability DPS",
                        "Ability Damage per GCD",
                        "Rage Cost per Damage",
                        "Execute Spam",
                        "Ability Maintenance Changes",
#if DEBUG
                        "PTR Testing", // This is Devs only
#endif
                    };
                }
                return _customChartNames;
            }
        }

        float getDPS(DPSWarrCharacter dpswarchar, int Iter, bool with)
        {
            bool orig = dpswarchar.CalcOpts.MaintenanceTree[Iter];
            dpswarchar.CalcOpts.MaintenanceTree[Iter] = with;
            CharacterCalculationsDPSWarr calculations = GetCharacterCalculations(dpswarchar.Char.Clone()) as CharacterCalculationsDPSWarr;
            dpswarchar.CalcOpts.MaintenanceTree[Iter] = orig;
            return calculations.TotalDPS;
        }
        float getSurv(DPSWarrCharacter dpswarchar, int Iter, bool with)
        {
            bool orig = dpswarchar.CalcOpts.MaintenanceTree[Iter];
            dpswarchar.CalcOpts.MaintenanceTree[Iter] = with;
            CharacterCalculationsDPSWarr calculations = GetCharacterCalculations(dpswarchar.Char.Clone()) as CharacterCalculationsDPSWarr;
            dpswarchar.CalcOpts.MaintenanceTree[Iter] = orig;
            return calculations.TotalHPS;
        }

        ComparisonCalculationDPSWarr getComp(DPSWarrCharacter dpswarchar, string Name, int Iter) {
            ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
            comparison.Name = Name;
            comparison.Equipped = dpswarchar.CalcOpts.MaintenanceTree[Iter] == true;
            float with = getDPS(dpswarchar, Iter, true);
            float without = getDPS(dpswarchar, Iter, false);
            comparison.DPSPoints = with - without;
            with = getSurv(dpswarchar, Iter, true) * dpswarchar.CalcOpts.SurvScale;
            without = getSurv(dpswarchar, Iter, false) * dpswarchar.CalcOpts.SurvScale;
            comparison.SurvPoints = with - without;
            //comparison.ImageSource = aw.ability.Icon; // TODO
            //comparison.Description = aw.ability.Desc; // TODO
            return comparison;
        }

        public override ComparisonCalculationBase[] GetCustomChartData(Character character, string chartName) {
            if (character == null) { return null;}
            Character zeOriginal = character.Clone();
            Character zeClone = character.Clone();
            CharacterCalculationsDPSWarr calculations = GetCharacterCalculations(zeOriginal) as CharacterCalculationsDPSWarr;
            CalculationOptionsDPSWarr calcOpts = zeOriginal.CalculationOptions as CalculationOptionsDPSWarr;
            ((CalculationOptionsPanelDPSWarr)CalculationOptionsPanel).IsLoadingCalculationOptions = true;
            bool[] origMaints = (bool[])calcOpts.MaintenanceTree.Clone();
            DPSWarrCharacter dpswarchar = new DPSWarrCharacter() {
                Char = zeOriginal,
                CalcOpts = (CalculationOptionsDPSWarr)zeOriginal.CalculationOptions,
                BossOpts = zeOriginal.BossOptions,
                CombatFactors = null,
                Rot = null,
            };

            switch (chartName) {
                #region Ability DPS
                case "Ability DPS": {
                    List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                    foreach (AbilityWrapper aw in calculations.Rot.TheAbilityList)
                    {
                        if (aw.Ability.DamageOnUse == 0) { continue; }
                        ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                        comparison.Name = aw.Ability.Name;
                        comparison.Description = aw.Ability.Desc;
                        if (aw.Ability is Skills.Rend) {
                            comparison.DPSPoints = (aw.Ability as Skills.Rend).GetDPS(aw.NumActivatesO20, calculations.Rot.GetWrapper<Skills.ThunderClap>().NumActivatesO20, calculations.Rot.TimeOver20Perc) * calculations.Rot.TimeOver20Perc
                                                 + (aw.Ability as Skills.Rend).GetDPS(aw.NumActivatesU20, calculations.Rot.GetWrapper<Skills.ThunderClap>().NumActivatesU20, calculations.Rot.TimeUndr20Perc) * calculations.Rot.TimeUndr20Perc;
                        } else {
                            comparison.DPSPoints = aw.AllDPS;
                        }
                        comparison.SurvPoints = aw.AllHPS;
                        comparison.ImageSource = aw.Ability.Icon;
                        comparisons.Add(comparison);
                    }
                    foreach (ComparisonCalculationDPSWarr comp in comparisons) {
                        comp.OverallPoints = comp.DPSPoints + comp.SurvPoints;
                    }
                    calcOpts.MaintenanceTree = origMaints;
                    ((CalculationOptionsPanelDPSWarr)CalculationOptionsPanel).IsLoadingCalculationOptions = false;
                    return comparisons.ToArray();
                }
                #endregion
                #region Ability Damage per GCD
                case "Ability Damage per GCD":
                    {
                        List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                        foreach (AbilityWrapper aw in calculations.Rot.TheAbilityList)
                        {
                            if (aw.Ability.DamageOnUse == 0) { continue; }
                            ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                            comparison.Name = aw.Ability.Name;
                            comparison.Description = aw.Ability.Desc;
                            comparison.DPSPoints = (
                                ((aw.Ability is Skills.Rend) ? ((aw.Ability as Skills.Rend).TickSize * (aw.Ability as Skills.Rend).NumTicks)
                                : aw.Ability.DamageOnUse)) / (aw.Ability.GCDTime == 0 ? 1f : (aw.Ability.GCDTime / calculations.Rot.LatentGCD));
                            comparison.SurvPoints = aw.Ability.GetAvgHealingOnUse(aw.AllNumActivates);
                            comparison.ImageSource = aw.Ability.Icon;
                            comparisons.Add(comparison);
                        }
                        foreach (ComparisonCalculationDPSWarr comp in comparisons)
                        {
                            comp.OverallPoints = comp.DPSPoints + comp.SurvPoints;
                        }
                        calcOpts.MaintenanceTree = origMaints;
                        ((CalculationOptionsPanelDPSWarr)CalculationOptionsPanel).IsLoadingCalculationOptions = false;
                        return comparisons.ToArray();
                    }
                #endregion
                #region Ability Maintenance Changes
                case "Ability Maintenance Changes": {
                    List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                    #region Rage Generators
                    comparisons.Add(getComp(dpswarchar, "Berserker Rage", (int)Maintenance.BerserkerRage));
                    comparisons.Add(getComp(dpswarchar, "Deadly Calm", (int)Maintenance.DeadlyCalm));
                    #endregion
                    #region Maintenance
                    comparisons.Add(getComp(dpswarchar, "Battle Shout", (int)Maintenance.BattleShout));
                    comparisons.Add(getComp(dpswarchar, "Commanding Shout", (int)Maintenance.CommandingShout));
                    comparisons.Add(getComp(dpswarchar, "Demoralizing Shout", (int)Maintenance.DemoralizingShout));
                    comparisons.Add(getComp(dpswarchar, "Sunder Armor", (int)Maintenance.SunderArmor));
                    comparisons.Add(getComp(dpswarchar, "Thunder Clap", (int)Maintenance.ThunderClap));
                    comparisons.Add(getComp(dpswarchar, "Hamstring", (int)Maintenance.Hamstring));
                    #endregion
                    #region Periodics
                    comparisons.Add(getComp(dpswarchar, "Shattering Throw", (int)Maintenance.ShatteringThrow));
                    comparisons.Add(getComp(dpswarchar, "Sweeping Strikes", (int)Maintenance.SweepingStrikes));
                    comparisons.Add(getComp(dpswarchar, "Death Wish", (int)Maintenance.DeathWish));
                    comparisons.Add(getComp(dpswarchar, "Recklessness", (int)Maintenance.Recklessness));
                    comparisons.Add(getComp(dpswarchar, "Enraged Regeneration", (int)Maintenance.EnragedRegeneration));
                    #endregion
                    #region Damage Dealers
                    if (calculations.Rot.GetType() == typeof(FuryRotation)) {
                        #region Fury
                        comparisons.Add(getComp(dpswarchar, "Bloodsurge", (int)Maintenance.Bloodsurge));
                        comparisons.Add(getComp(dpswarchar, "Bloodthirst", (int)Maintenance.Bloodthirst));
//                        comparisons.Add(getComp(dpswarchar, "Whirlwind", (int)Maintenance.Whirlwind));
                        comparisons.Add(getComp(dpswarchar, "Raging Blow", (int)Maintenance.RagingBlow));
                        #endregion
                    } else if (calculations.Rot.GetType() == typeof(ArmsRotation)) {
                        #region Arms
                        comparisons.Add(getComp(dpswarchar, "Bladestorm", (int)Maintenance.Bladestorm));
                        comparisons.Add(getComp(dpswarchar, "Mortal Strike", (int)Maintenance.MortalStrike));
                        comparisons.Add(getComp(dpswarchar, "Rend", (int)Maintenance.Rend));
                        comparisons.Add(getComp(dpswarchar, "Overpower", (int)Maintenance.Overpower));
                        comparisons.Add(getComp(dpswarchar, "Taste for Blood", (int)Maintenance.TasteForBlood));
                        comparisons.Add(getComp(dpswarchar, "Colossus Smash", (int)Maintenance.ColossusSmash));
                        comparisons.Add(getComp(dpswarchar, "Slam", (int)Maintenance.Slam));
                        #endregion
                    }
                    comparisons.Add(getComp(dpswarchar, "<20% Execute Spamming", (int)Maintenance.ExecuteSpam));
                    #endregion
                    #region Rage Dumps
                    comparisons.Add(getComp(dpswarchar, "Heroic Strike", (int)Maintenance.HeroicStrike));
                    comparisons.Add(getComp(dpswarchar, "Cleave", (int)Maintenance.Cleave));
                    comparisons.Add(getComp(dpswarchar, "Inner Rage", (int)Maintenance.InnerRage));
                    #endregion
                    foreach (ComparisonCalculationDPSWarr comp in comparisons) {
                        comp.OverallPoints = comp.DPSPoints + comp.SurvPoints;
                    }
                    calcOpts.MaintenanceTree = origMaints;
                    ((CalculationOptionsPanelDPSWarr)CalculationOptionsPanel).IsLoadingCalculationOptions = false;
                    return comparisons.ToArray();
                }
                #endregion
                #region Rage Cost per Damage
                case "Rage Cost per Damage": {
                    List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                    float DeepWoundsDamage = calculations.Rot.DW.TickSize * 6f;

                    foreach (AbilityWrapper aw in calculations.Rot.TheAbilityList)
                    {
                        if (aw.Ability.DamageOnUse == 0 || aw.Ability.RageCost == -1f) { continue; }
                        ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                        comparison.Name = aw.Ability.Name;
                        comparison.Description = string.Format("Costs {0} Rage\r\n{1}", aw.Ability.RageCost, aw.Ability.Desc);
                        float extraRage = (aw.Ability is Skills.Execute) ? (aw.Ability as Skills.Execute).UsedExtraRage : 0f;
                        float val = (((aw.Ability.RageCost + extraRage) != 0) ? (aw.Ability.RageCost < -1f ? aw.Ability.RageCost * -1f : aw.Ability.RageCost) + extraRage : 1f);
                        if (aw.Ability is Skills.Rend) {
                            comparison.SubPoints[0] = ((aw.Ability as Skills.Rend).TickSize * (aw.Ability as Skills.Rend).NumTicks) / val;
                        } else {
                            comparison.SubPoints[0] = aw.Ability.DamageOnUse / val;
                        }
                        comparison.SubPoints[1] = (aw.Ability.MHAtkTable.Crit * DeepWoundsDamage) / val;
                        comparison.ImageSource = aw.Ability.Icon;
                        comparisons.Add(comparison);
                    }
                    foreach (ComparisonCalculationDPSWarr comp in comparisons) {
                        comp.OverallPoints = comp.SubPoints[0] + comp.SubPoints[1];
                    }
                    calcOpts.MaintenanceTree = origMaints;
                    ((CalculationOptionsPanelDPSWarr)CalculationOptionsPanel).IsLoadingCalculationOptions = false;
                    return comparisons.ToArray();
                }
                #endregion
                #region Execute Spam
                case "Execute Spam": {
                    List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                    {
                        bool orig = ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpam;
                        bool orig2 = ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpamStage2;
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpam = false;
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpamStage2 = false;
                        //CharacterCalculationsDPSWarr bah = GetCharacterCalculations(zeClone) as CharacterCalculationsDPSWarr;
                        ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                        comparison.Name = "Without Execute Spam";
                        comparison.Description = "Turning <20% Execute Spam off on the options pane will change your DPS to this";
                        comparison.SubPoints[0] = GetCharacterCalculations(zeClone).SubPoints[0];
                        comparison.SubPoints[1] = GetCharacterCalculations(zeClone).SubPoints[1];
                        comparison.Equipped = orig == false;
                        comparison.ImageSource = Skills.MortalStrike.SIcon;
                        comparisons.Add(comparison);
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpam = orig;
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpamStage2 = orig2;
                    }
                    {
                        bool orig = ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpam;
                        bool orig2 = ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpamStage2;
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpam = true;
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpamStage2 = false;
                        //CharacterCalculationsDPSWarr bah = GetCharacterCalculations(zeClone) as CharacterCalculationsDPSWarr;
                        ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                        comparison.Name = "With Execute Spam";
                        comparison.Description = "Turning <20% Execute Spam on on the options pane will change your DPS to this";
                        comparison.SubPoints[0] = GetCharacterCalculations(zeClone).SubPoints[0];
                        comparison.SubPoints[1] = GetCharacterCalculations(zeClone).SubPoints[1];
                        comparison.Equipped = orig == true && orig2 == false;
                        comparison.ImageSource = Skills.Execute.SIcon;
                        comparisons.Add(comparison);
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpam = orig;
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpamStage2 = orig2;
                    }
                    {
                        bool orig = ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpam;
                        bool orig2 = ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpamStage2;
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpam = true;
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpamStage2 = true;
                        CharacterCalculationsDPSWarr bah = GetCharacterCalculations(zeClone) as CharacterCalculationsDPSWarr;
                        ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                        comparison.Name = "With Execute Spam Stage 2";
                        comparison.Description = "Turning <20% Execute Spam on on the options pane AND Enforcing Taste for Blood will change your DPS to this";
                        comparison.SubPoints[0] = GetCharacterCalculations(zeClone).SubPoints[0];
                        comparison.SubPoints[1] = GetCharacterCalculations(zeClone).SubPoints[1];
                        comparison.Equipped = orig == true && orig2 == true;
                        comparison.ImageSource = Skills.TasteForBlood.SIcon;
                        comparisons.Add(comparison);
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpam = orig;
                        ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).M_ExecuteSpamStage2 = orig2;
                    }
                    foreach (ComparisonCalculationDPSWarr comp in comparisons)
                    {
                        comp.OverallPoints = comp.SubPoints[0] + comp.SubPoints[1];
                    }
                    calcOpts.MaintenanceTree = origMaints;
                    ((CalculationOptionsPanelDPSWarr)CalculationOptionsPanel).IsLoadingCalculationOptions = false;
                    return comparisons.ToArray();
                }
                #endregion
                #region PTR Testing
                case "PTR Testing":
                    {
                        List<ComparisonCalculationBase> comparisons = new List<ComparisonCalculationBase>();
                        {
                            bool orig = ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).PtrMode;
                            ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).PtrMode = false;
                            CharacterCalculationsDPSWarr bah = GetCharacterCalculations(zeClone) as CharacterCalculationsDPSWarr;
                            ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                            comparison.Name = "Live Mode";
                            comparison.Description = "This makes Thunderclap apply Rend's Initial Damage along with its own.";
                            comparison.SubPoints[0] = GetCharacterCalculations(zeClone).SubPoints[0];
                            comparison.SubPoints[1] = GetCharacterCalculations(zeClone).SubPoints[1];
                            comparison.Equipped = orig == false;
                            comparison.ImageSource = "spell_nature_callstorm";
                            comparisons.Add(comparison);
                            ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).PtrMode = orig;
                        }
                        {
                            bool orig = ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).PtrMode;
                            ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).PtrMode = true;
                            CharacterCalculationsDPSWarr bah = GetCharacterCalculations(zeClone) as CharacterCalculationsDPSWarr;
                            ComparisonCalculationDPSWarr comparison = new ComparisonCalculationDPSWarr();
                            comparison.Name = "PTR Mode";
                            comparison.Description = "This makes Thunderclap NOT apply Rend's Initial Damage along with its own.";
                            comparison.SubPoints[0] = GetCharacterCalculations(zeClone).SubPoints[0];
                            comparison.SubPoints[1] = GetCharacterCalculations(zeClone).SubPoints[1];
                            comparison.Equipped = orig == true;
                            comparison.ImageSource = Skills.Rend.SIcon;
                            comparisons.Add(comparison);
                            ((CalculationOptionsDPSWarr)zeClone.CalculationOptions).PtrMode = orig;
                        }
                        foreach (ComparisonCalculationDPSWarr comp in comparisons)
                        {
                            comp.OverallPoints = comp.SubPoints[0] + comp.SubPoints[1];
                        }
                        calcOpts.MaintenanceTree = origMaints;
                        ((CalculationOptionsPanelDPSWarr)CalculationOptionsPanel).IsLoadingCalculationOptions = false;
                        return comparisons.ToArray();
                    }
                #endregion
                default: { calcOpts.MaintenanceTree = origMaints; return new ComparisonCalculationBase[0]; }
            }
        }
        #endregion

        #region Character Calcs

#if DEBUG
        /*public static string GetConstructionCounts() {
            string retVal = "";
            //
            foreach (string c in ConstructionCounts.Keys) {
                retVal += string.Format("{0:000000}/{1:00000.00}: {2}\n",
                    ConstructionCounts[c] < 5 ? 0 : ConstructionCounts[c] - 5,
                    (float)(ConstructionCounts[c] < 5 ? 0 : ConstructionCounts[c] - 5) / (float)(ConstructionCounts["GetCharacterCalculations"] < 5 ? 1 : ConstructionCounts["GetCharacterCalculations"] - 5),
                    c);
            }
            //
            return retVal + "\n";
        }
        public static Dictionary<string, int> ConstructionCounts = new Dictionary<string,int>() {
            { "GetCharacterCalculations", 0 },
            { "GetCharacterStats", 0 },
            { "GetCharacterStats_Inner", 0 },
            { "GetCharacterStats_Buffed", 0 },
            { "GetCharacterStats_Override", 0 },
            { "CalculateTriggers", 0 },
            { "IterativeSpecialEffectsStats", 0 },
            { "ApplySpecialEffect", 0 },
            { "UpdateStatsAndAdd", 0 },
            { "DoSpecialEffects", 0 },
            { "Rotation Base", 0 },
            { "Rotation Arms", 0 },
            { "Rotation Fury", 0 },
            { "CombatFactors", 0 },
        };*/
#endif

        public override CharacterCalculationsBase GetCharacterCalculations(Character character, Item additionalItem, bool referenceCalculation, bool significantChange, bool needsDisplayCalculations)
        {
#if DEBUG
            //ConstructionCounts["GetCharacterCalculations"]++;
#endif
            CharacterCalculationsDPSWarr calc = new CharacterCalculationsDPSWarr();
            try {
                #region Object Creation
                // First things first, we need to ensure that we aren't using bad data
                if (character == null) { return calc; }
                CalculationOptionsDPSWarr calcOpts = character.CalculationOptions as CalculationOptionsDPSWarr;
                if (calcOpts == null) { return calc; }
                //
                BossOptions bossOpts = character.BossOptions;
                
                CombatFactors combatFactors;
                Skills.WhiteAttacks whiteAttacks;
                Rotation Rot;

                Base.StatsWarrior statsRace = null;
                Base.StatsWarrior stats = GetCharacterStats(character, additionalItem, StatType.Average, calcOpts, bossOpts, out statsRace, out combatFactors, out whiteAttacks, out Rot);

                DPSWarrCharacter charStruct = new DPSWarrCharacter() {
                    CalcOpts = calcOpts,
                    BossOpts = bossOpts,
                    Rot = Rot,
                    CombatFactors = combatFactors,
                    Char = character,
                    Talents = character.WarriorTalents
                };

                /*if (calcOpts.UseMarkov) {
                    if (combatFactors.FuryStance) {
                        //Markov.StateSpaceGeneratorFuryTest b = new Markov.StateSpaceGeneratorFuryTest();
                        //Markov.StateSpaceGeneratorFuryTest.StateSpaceGeneratorFuryTest1(character, stats, combatFactors, whiteAttacks, calcOpts, bossOpts, needsDisplayCalculations);
                    } else {
                        //Markov.StateSpaceGeneratorArmsTest b = new Markov.StateSpaceGeneratorArmsTest();
                        //Markov.StateSpaceGeneratorArmsTest.StateSpaceGeneratorArmsTest1(character, stats, combatFactors, whiteAttacks, calcOpts, bossOpts, needsDisplayCalculations);
                    }
                }*/
                #endregion

                calc.Duration = bossOpts.BerserkTimer;
                calc.PTRMode = calcOpts.PtrMode;

                calc.AverageStats = stats;
                if (needsDisplayCalculations) {
                    calc.UnbuffedStats = GetCharacterStats(character, additionalItem, StatType.Unbuffed, calcOpts, bossOpts, out statsRace);
                    calc.BuffedStats = GetCharacterStats(character, additionalItem, StatType.Buffed, calcOpts, bossOpts, out statsRace);
                    calc.BuffsStats = GetBuffsStats(charStruct);
                    calc.MaximumStats = GetCharacterStats(character, additionalItem, StatType.Maximum, calcOpts, bossOpts, out statsRace);
                }
                
                calc.CombatFactors = combatFactors;
                calc.Rot = Rot;
                calc.TargetLevel = bossOpts.Level;
                calc.BaseHealth = statsRace.Health; 
                {// == Attack Table ==
                    // Miss
                    calc.Miss = stats.Miss;
                    calc.HitRating = stats.HitRating;
                    calc.ExpertiseRating = stats.ExpertiseRating;
                    calc.Expertise = StatConversion.GetExpertiseFromRating(stats.ExpertiseRating, CharacterClass.Warrior) + stats.Expertise;
                    calc.MHExpertise = combatFactors.CMHexpertise;
                    calc.OHExpertise = combatFactors.COhexpertise;
                    calc.AgilityCritBonus = StatConversion.GetCritFromAgility(stats.Agility, CharacterClass.Warrior);
                    calc.CritRating = stats.CritRating;
                    calc.CritPercent = stats.PhysicalCrit;
                    calc.MHCrit = combatFactors.CMHycrit;
                    calc.OHCrit = combatFactors.COhycrit;
                } 
                // Offensive
                //calc.ArmorPenetrationRating = stats.ArmorPenetrationRating;
                //calc.ArmorPenetrationRating2Perc = StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating);
                //calc.ArmorPenetration = Math.Min(1f, calc.ArmorPenetrationRating2Perc);
                calc.HasteRating = stats.HasteRating;
                calc.HastePercent = stats.PhysicalHaste;
                calc.MasteryVal = StatConversion.GetMasteryFromRating(stats.MasteryRating, CharacterClass.Warrior);
                
                // DPS
                Rot.Initialize(calc);

                calc.PlateSpecValid = HelperFunctions.ValidatePlateSpec(charStruct);
                
                // Neutral
                // Defensive
                calc.Armor = stats.Armor; 

                Rot.MakeRotationandDoDPS(true, needsDisplayCalculations);

                #region Special Damage Procs, like Bandit's Insignia or Hand-mounted Pyro Rockets
                calc.SpecProcDPS = calc.SpecProcDmgPerHit = calc.SpecProcActs = 0f;
                if (stats._rawSpecialEffectData != null && character.MainHand != null) {
                    bool runIt = false;
                    foreach (SpecialEffect s in stats.SpecialEffects()) {
                        if (s.Stats == null) { continue; }
                        if (s.Stats.ShadowDamage != 0) { runIt = true; break; }
                        if (s.Stats.FireDamage   != 0) { runIt = true; break; }
                        if (s.Stats.HolyDamage   != 0) { runIt = true; break; }
                        if (s.Stats.FrostDamage  != 0) { runIt = true; break; }
                        if (s.Stats.NatureDamage != 0) { runIt = true; break; }
                        if (s.Stats.ArcaneDamage != 0) { runIt = true; break; }
                    }
                    if (runIt) {
                        DamageProcs.SpecialDamageProcs SDP;
                        Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
                        Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();
                        CalculateTriggers(charStruct, triggerIntervals, triggerChances);
                        if (character.Race == CharacterRace.Goblin && statsRace._rawSpecialEffectData.Length > 0) {
                            // Fix the damage for Goblin Rockets
                            foreach (SpecialEffect s in stats.SpecialEffects()) {
                                if (s.Stats != null && s.Stats.FireDamage == (1f + character.Level * 2)) {
                                    s.Stats.FireDamage += stats.AttackPower * 0.25f   // AP Bonus
                                                        + stats.Intellect * 0.50193f; // Int Bonus
                                }
                            }
                        }
                        SDP = new Rawr.DamageProcs.SpecialDamageProcs(character, stats, calc.TargetLevel - character.Level,
                            new List<SpecialEffect>(stats.SpecialEffects()),
                            triggerIntervals, triggerChances,
                            bossOpts.BerserkTimer,
                            combatFactors.DamageReduction);

                        calc.SpecProcDPS = SDP.CalculateAll();
                        calc.SpecProcDmgPerHit = SDP.GetDamagePerHit;
                        calc.SpecProcActs = SDP.GetTotalNumProcs;
                    }
                }
                calc.TotalDPS += calc.SpecProcDPS;
                #endregion

                #region Survivability
                List<Attack> Attacks;
                if (calcOpts.SurvScale != 0f && (Attacks = bossOpts.Attacks.FindAll(a => a.AffectsRole[PLAYER_ROLES.MeleeDPS])).Count > 0) {
                    Dictionary<ItemDamageType, float> countsDmg = new Dictionary<ItemDamageType, float>() {
                        { ItemDamageType.Physical, 0f },
                        { ItemDamageType.Arcane, 0f },
                        { ItemDamageType.Fire, 0f },
                        { ItemDamageType.Frost, 0f },
                        { ItemDamageType.Holy, 0f },
                        { ItemDamageType.Nature, 0f },
                        { ItemDamageType.Shadow, 0f },
                    };
                    Dictionary<ItemDamageType, float> percDmg = new Dictionary<ItemDamageType, float>() {
                        { ItemDamageType.Physical, 0f },
                        { ItemDamageType.Arcane, 0f },
                        { ItemDamageType.Fire, 0f },
                        { ItemDamageType.Frost, 0f },
                        { ItemDamageType.Holy, 0f },
                        { ItemDamageType.Nature, 0f },
                        { ItemDamageType.Shadow, 0f },
                    };
                    Dictionary<ItemDamageType, float> highestDmg = new Dictionary<ItemDamageType, float>() {
                        { ItemDamageType.Physical, 0f },
                        { ItemDamageType.Arcane, 0f },
                        { ItemDamageType.Fire, 0f },
                        { ItemDamageType.Frost, 0f },
                        { ItemDamageType.Holy, 0f },
                        { ItemDamageType.Nature, 0f },
                        { ItemDamageType.Shadow, 0f },
                    };
                    int totalCount = 0;
                    foreach (Attack a in Attacks) {
                        countsDmg[a.DamageType] += 1; totalCount++;
                        if ((a.DamagePerHit + a.DamagePerTick) > highestDmg[a.DamageType]) {
                            highestDmg[a.DamageType] = (a.DamagePerHit + a.DamagePerTick);
                        }
                    }
                    foreach (ItemDamageType t in countsDmg.Keys) { percDmg[t] = countsDmg[t] / (float)totalCount; }
                    float TotalConstantDamageReduction = 1f - (1f - (stats.Armor > 0 ? StatConversion.GetArmorDamageReduction(bossOpts.Level, character.Level, stats.Armor, 0f, 0f) : 0))
                                                            * (1f - stats.DamageTakenReductionMultiplier)
                                                            * (1f - stats.BossPhysicalDamageDealtReductionMultiplier);
                    Dictionary<ItemDamageType, float> SurvivabilityPointsRaw = new Dictionary<ItemDamageType, float>() {
                        { ItemDamageType.Physical, stats.Health / (1f - TotalConstantDamageReduction) },
                        { ItemDamageType.Arcane,   stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.ArcaneResistance, 0)) * (1f - stats.DamageTakenReductionMultiplier)) },
                        { ItemDamageType.Fire,     stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.FireResistance,   0)) * (1f - stats.DamageTakenReductionMultiplier)) },
                        { ItemDamageType.Frost,    stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.FrostResistance,  0)) * (1f - stats.DamageTakenReductionMultiplier)) },
                        { ItemDamageType.Holy,     stats.Health / ((1f)                                                                                                   * (1f - stats.DamageTakenReductionMultiplier)) },
                        { ItemDamageType.Nature,   stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.NatureResistance, 0)) * (1f - stats.DamageTakenReductionMultiplier)) },
                        { ItemDamageType.Shadow,   stats.Health / ((1f - StatConversion.GetMinimumResistance(bossOpts.Level, character.Level, stats.ShadowResistance, 0)) * (1f - stats.DamageTakenReductionMultiplier)) },
                    };
                    Dictionary<ItemDamageType, float> SurvivabilityPoints = SoftCapSurvivals(charStruct, highestDmg, SurvivabilityPointsRaw);

                    if (stats.HealthRestoreFromMaxHealth > 0) {
                        stats.HealthRestore += stats.HealthRestoreFromMaxHealth * stats.Health * bossOpts.BerserkTimer;
                    }

                    float survs = 0f;
                    foreach (ItemDamageType t in SurvivabilityPoints.Keys) { survs += SurvivabilityPoints[t] * percDmg[t]; }
                    float HealthRest2Surv = survs / 100f + Rot._HPS_TTL + stats.HealthRestore / 100f;
                    calc.TotalHPS = Rot._HPS_TTL;
                    calc.Survivability = calcOpts.SurvScale * HealthRest2Surv;
                } else {
                    // No point in running all those calcs just to zero them out after
                    calc.TotalHPS = Rot._HPS_TTL;
                    calc.Survivability = 0f;
                }
                #endregion

                calc.OverallPoints = calc.TotalDPS + calc.Survivability;

                #region Old ArP cap calc, keeping in case another cap has to be implemented
                //calculatedStats.UnbuffedStats = GetCharacterStats(character, additionalItem, StatType.Unbuffed, calcOpts, bossOpts);
                /*if (needsDisplayCalculations)
                {
                    calc.BuffedStats = GetCharacterStats(character, additionalItem, StatType.Buffed, calcOpts, bossOpts, out statsRace);
                    //calculatedStats.MaximumStats = GetCharacterStats(character, additionalItem, StatType.Maximum, calcOpts, bossOpts);

                    float maxArp = calc.BuffedStats.ArmorPenetrationRating;
                    foreach (SpecialEffect effect in calc.BuffedStats.SpecialEffects(s => s.Stats.ArmorPenetrationRating > 0f))
                    {
                        maxArp += effect.Stats.ArmorPenetrationRating;
                    }
                    calc.MaxArmorPenetration = StatConversion.GetArmorPenetrationFromRating(maxArp);
                }*/
                #endregion

#if DEBUG
                /*foreach (string c in Rotation.ConstructionCounts.Keys) {
                    if (!ConstructionCounts.ContainsKey(c)) { ConstructionCounts.Add(c, 0); }
                    ConstructionCounts[c] = Rotation.ConstructionCounts[c];
                }
                foreach (string c in CombatFactors.ConstructionCounts.Keys) {
                    if (!ConstructionCounts.ContainsKey(c)) { ConstructionCounts.Add(c, 0); }
                    ConstructionCounts[c] = CombatFactors.ConstructionCounts[c];
                }
                string counts = GetConstructionCounts();
                Clipboard.SetText(counts);
                //System.Windows.MessageBox.Show(counts, "DPSWarr Counts", System.Windows.MessageBoxButton.OK);*/
#endif
            } catch (Exception ex) {
                new Base.ErrorBox() {
                    Title = "Error in creating Stat Pane Calculations",
                    Function = "GetCharacterCalculations()",
                    TheException = ex,
                }.Show();
            }
            return calc;
        }

        static double fourToTheNegativeFourThirds = Math.Pow(4d, -4d / 3d);
        static double topRight = Math.Pow(fourToTheNegativeFourThirds, 1d / 4d);
        public float SoftCapSurv(float raw, float damage) {
            if (damage <= 0f) { return 0f; }
            double survivalCap = damage * 2.0f / 1000d;
            double survivalRaw = raw / 1000d;

            //Implement Survival Soft Cap
            if (survivalRaw <= survivalCap) {
                return 1000f * (float)survivalRaw;
            } else {
                double x = survivalRaw;
                double cap = survivalCap;
                double topLeft = Math.Pow(((x - cap) / cap) + fourToTheNegativeFourThirds, 1d / 4d);
                double fracTop = topLeft - topRight;
                double fraction = fracTop / 2d;
                double y = (cap * fraction + cap);
                return 1000f * (float)y;
            }
        }

        public Dictionary<ItemDamageType, float> SoftCapSurvivals(DPSWarrCharacter dpswarchar, Dictionary<ItemDamageType, float> highestDmg, Dictionary<ItemDamageType, float> survivalRaws) {
            Dictionary<ItemDamageType, float> cappedValues = new Dictionary<ItemDamageType, float>() { };
            //
            foreach (ItemDamageType t in highestDmg.Keys) {
                cappedValues[t] = SoftCapSurv(survivalRaws[t], highestDmg[t]);
            }
            //
            return cappedValues;
        }

        public override Stats GetCharacterStats(Character character, Item additionalItem) {
#if DEBUG
            //ConstructionCounts["GetCharacterStats_Override"]++;
#endif
            if (character == null) { return new Stats(); }
            try {
                Base.StatsWarrior statsRace = null;
                return GetCharacterStats(character, additionalItem, StatType.Average, (CalculationOptionsDPSWarr)character.CalculationOptions, character.BossOptions, out statsRace);
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error in getting Character Stats",
                    Function = "GetCharacterStats()",
                    TheException = ex,
                }.Show();
            }
            return new Stats() { };
        }

        private Base.StatsWarrior GetCharacterStats_Buffed(DPSWarrCharacter dpswarchar, Item additionalItem, bool isBuffed, out Base.StatsWarrior statsRace)
        {
#if DEBUG
            //ConstructionCounts["GetCharacterStats_Buffed"]++;
#endif
            if (dpswarchar.CalcOpts == null) { dpswarchar.CalcOpts = dpswarchar.Char.CalculationOptions as CalculationOptionsDPSWarr; }
            if (dpswarchar.BossOpts == null) { dpswarchar.BossOpts = dpswarchar.Char.BossOptions; }
            if (dpswarchar.CombatFactors == null) { dpswarchar.CombatFactors = new CombatFactors(dpswarchar.Char, new Base.StatsWarrior(), dpswarchar.CalcOpts, dpswarchar.BossOpts); }
            WarriorTalents talents = dpswarchar.Char.WarriorTalents;

            #region From Race
            statsRace = new Base.StatsWarrior();
            statsRace.Accumulate(BaseStats.GetBaseStats(dpswarchar.Char.Level, CharacterClass.Warrior, dpswarchar.Char.Race));
            #endregion
            #region From Gear/Buffs
            Base.StatsWarrior statsBuffs = (isBuffed ? GetBuffsStats(dpswarchar) : new Base.StatsWarrior());
            Base.StatsWarrior statsItems = new Base.StatsWarrior();
            statsItems.Accumulate(GetItemStats(dpswarchar.Char, additionalItem));
            #endregion
            #region From Options
            Base.StatsWarrior statsOptionsPanel = new Base.StatsWarrior()
            {
                //BonusStrengthMultiplier = (dpswarchar.combatFactors.FuryStance ? talents.ImprovedBerserkerStance * 0.04f : 0f),
                //PhysicalCrit = (dpswarchar.combatFactors.FuryStance ? 0.03f + statsBuffs.BonusWarrior_T9_2P_Crit : 0f),
                // Stance Related Damage Given/Taken mods
                /*
                  Battle Stance
                        A balanced combat stance.  Increases damage done by 5%.  Decreases damage taken by 5%.
                  Berserker Stance
                        An aggressive combat stance.  Increases damage done by 10%.
                */
                DamageTakenReductionMultiplier = (!dpswarchar.CombatFactors.FuryStance ? -0.05f : 0f),
                BonusDamageMultiplier = (!dpswarchar.CombatFactors.FuryStance ? 0.05f : 0.10f),

                // Battle Shout
                Strength = (dpswarchar.CalcOpts.M_BattleShout ? 549f : 0f),
                Agility = (dpswarchar.CalcOpts.M_BattleShout ? 549f : 0f),
                // Commanding Shout
                Stamina = (dpswarchar.CalcOpts.M_CommandingShout ? 585f : 0f),
                // Demo Shout
                BossPhysicalDamageDealtReductionMultiplier = (dpswarchar.CalcOpts.M_DemoralizingShout ? 0.10f : 0f),
                // Sunder Armor
                ArmorPenetration = (dpswarchar.CalcOpts.M_SunderArmor ? 0.04f * 3f : 0f),
                // Thunder Clap
                BossAttackSpeedReductionMultiplier = (dpswarchar.CalcOpts.M_ThunderClap ? 0.20f : 0f),
            };
            if (dpswarchar.CalcOpts.M_ColossusSmash) { statsOptionsPanel.AddSpecialEffect(TalentsAsSpecialEffects.ColossusSmash); }
            #endregion
            #region From Talents
            Base.StatsWarrior statsTalents = new Base.StatsWarrior()
            {
                // Offensive
                BonusDamageMultiplier = ((!dpswarchar.CombatFactors.FuryStance
                                           && dpswarchar.Char.MainHand != null
                                           && dpswarchar.Char.MainHand.Slot == ItemSlot.TwoHand
                                         ? 1.20f : 1.00f)
                                         * (dpswarchar.CombatFactors.FuryStance
                                            && talents.SingleMindedFury > 0
                                            && HelperFunctions.ValidateSMTBonus(dpswarchar)
                                         ? 1.20f : 1.00f))
                                         - 1f,
                BonusPhysicalDamageMultiplier = (dpswarchar.CalcOpts.M_Rend // Have Rend up
                                                 || talents.DeepWounds > 0 // Have Deep Wounds
                                                ? talents.BloodFrenzy * 0.02f : 0f),
                BonusBleedDamageMultiplier = (dpswarchar.CalcOpts.M_Rend // Have Rend up
                                                 || talents.DeepWounds > 0 // Have Deep Wounds
                                                ? talents.BloodFrenzy * 0.15f : 0f),
                PhysicalCrit = (talents.Rampage > 0 && dpswarchar.CombatFactors.FuryStance && isBuffed ? 0.05f + 0.02f : 0f), // Cata has a new +2% on self (group gets 5%, self gets total 7%)
                PhysicalHit = (dpswarchar.CombatFactors.FuryStance ? 0.03f : 0f), // Fury Spec has passive 3% Hit
                BonusWhiteDamageMultiplier = (dpswarchar.CombatFactors.FuryStance ? 0.40f : 0f), // Fury Spec has passive 40% Bonus White Damage Mult
                // Defensive
                BaseArmorMultiplier = talents.Toughness * 0.10f / 3f,
                BonusHealingReceived = talents.FieldDressing * 0.03f,
                BonusStrengthMultiplier = HelperFunctions.ValidatePlateSpec(dpswarchar) ? 0.05f : 0f,
                // Specific Abilities
                BonusMortalStrikeDamageMultiplier = (1f + (dpswarchar.Talents.GlyphOfMortalStrike ? 0.10f : 0f))
                                                  * (1f + dpswarchar.Talents.WarAcademy * 0.05f)
                                                  - 1f,
                BonusRagingBlowDamageMultiplier = dpswarchar.Talents.WarAcademy * 0.05f,
                BonusOverpowerDamageMultiplier = (dpswarchar.Talents.GlyphOfOverpower ? 0.10f : 0f),
                BonusSlamDamageMultiplier = (1f + dpswarchar.Talents.ImprovedSlam * 0.20f)
                                          * (1f + dpswarchar.Talents.WarAcademy * 0.05f)
                                          - 1f,
                BonusVictoryRushDamageMultiplier = dpswarchar.Talents.WarAcademy * 0.05f,
                BonusBloodthirstDamageMultiplier = (dpswarchar.Talents.GlyphOfBloodthirst ? 0.10f : 0f),
            };
            // Add Talents that give SpecialEffects
            if (talents.WreckingCrew > 0 && dpswarchar.Char.MainHand != null) { statsTalents.AddSpecialEffect(TalentsAsSpecialEffects.WreckingCrew[talents.WreckingCrew]); }
            if (talents.LambsToTheSlaughter > 0 && dpswarchar.CalcOpts.M_MortalStrike)
            {
                statsTalents.AddSpecialEffect(TalentsAsSpecialEffects.LambsToTheSlaughter[talents.LambsToTheSlaughter]);
                // TODO: This should also refresh rend in 4.1.0
            }
            if (talents.BloodCraze > 0) { statsTalents.AddSpecialEffect(TalentsAsSpecialEffects.BloodCraze[talents.BloodCraze]); }
            if (talents.Executioner > 0 && dpswarchar.CalcOpts.M_ExecuteSpam) { statsTalents.AddSpecialEffect(TalentsAsSpecialEffects.Executioner[talents.Executioner]); }
            if (talents.BloodFrenzy > 0) { statsTalents.AddSpecialEffect(TalentsAsSpecialEffects.BloodFrenzy[talents.BloodFrenzy]); }
            if (talents.MeatCleaver > 0 && (dpswarchar.CalcOpts.M_Whirlwind || dpswarchar.CalcOpts.M_Cleave)) { statsTalents.AddSpecialEffect(TalentsAsSpecialEffects.MeatCleaver[talents.MeatCleaver]); }
            #endregion

            Base.StatsWarrior statsTotal = new Base.StatsWarrior();
            statsTotal.Accumulate(statsRace);
            statsTotal.Accumulate(statsItems);
            statsTotal.Accumulate(statsBuffs);
            statsTotal.Accumulate(statsTalents);
            statsTotal.Accumulate(statsOptionsPanel);
            statsTotal = UpdateStatsAndAdd(statsTotal, null, dpswarchar.Char);
            float multiplier = 0.0560f;
            float masteryBonusVal = (2f*0.056f + multiplier * StatConversion.GetMasteryFromRating(statsTotal.MasteryRating, CharacterClass.Warrior));
            if (talents.DeathWish > 0 && dpswarchar.CalcOpts.M_DeathWish && dpswarchar.CombatFactors.FuryStance) {
                statsTotal.AddSpecialEffect(TalentsAsSpecialEffects.GetDeathWishWithMastery(masteryBonusVal, dpswarchar));
            }
            if (talents.Enrage > 0 && dpswarchar.CombatFactors.FuryStance) {
                statsTotal.AddSpecialEffect(TalentsAsSpecialEffects.GetEnragedRegenerationWithMastery(masteryBonusVal, dpswarchar));
            }
            //Stats statsProcs = new Stats();

            // Dodge (your dodging incoming attacks)
            statsTotal.Dodge += StatConversion.GetDodgeFromAgility(statsTotal.Agility, dpswarchar.Char.Class);
            statsTotal.Dodge += StatConversion.GetDodgeFromRating(statsTotal.DodgeRating, dpswarchar.Char.Class);

            // Parry (your parrying incoming attacks)
            statsTotal.Parry += StatConversion.GetParryFromRating(statsTotal.ParryRating, dpswarchar.Char.Class);

            return statsTotal;
        }

        private Base.StatsWarrior GetCharacterStats(Character character, Item additionalItem, StatType statType, CalculationOptionsDPSWarr calcOpts, BossOptions bossOpts, out Base.StatsWarrior statsRace)
        {
#if DEBUG
            //ConstructionCounts["GetCharacterStats"]++;
#endif
            CombatFactors temp; Skills.WhiteAttacks temp2; Rotation temp3;
            return GetCharacterStats(character, additionalItem, statType, calcOpts, bossOpts, out statsRace, out temp, out temp2, out temp3);
        }
        private Base.StatsWarrior GetCharacterStats(Character character, Item additionalItem, StatType statType, CalculationOptionsDPSWarr calcOpts, BossOptions bossOpts,
            out Base.StatsWarrior statsRace, out CombatFactors combatFactors, out Skills.WhiteAttacks whiteAttacks, out Rotation Rot)
        {
#if DEBUG
            //ConstructionCounts["GetCharacterStats_Inner"]++;
#endif
            DPSWarrCharacter dpswarchar = new DPSWarrCharacter { Char = character, CalcOpts = calcOpts, BossOpts = bossOpts, Talents = character.WarriorTalents, CombatFactors = null, Rot = null };
            Base.StatsWarrior statsTotal = GetCharacterStats_Buffed(dpswarchar, additionalItem, statType != StatType.Unbuffed, out statsRace);
            dpswarchar.StatS = statsTotal;
            combatFactors = new CombatFactors(character, statsTotal, calcOpts, bossOpts); // we have to regenerate it here
            dpswarchar.CombatFactors = combatFactors;
            whiteAttacks = new Skills.WhiteAttacks(dpswarchar);
            dpswarchar.Whiteattacks = whiteAttacks;
            if (combatFactors.FuryStance) Rot = new FuryRotation(dpswarchar);
            else Rot = new ArmsRotation(dpswarchar);
            dpswarchar.Rot = Rot;
            
            if (statType == (StatType.Buffed | StatType.Unbuffed))
            {
                return statsTotal;
            }
            // SpecialEffects: Supposed to handle all procs such as Berserking, Mirror of Truth, Grim Toll, etc.
            Rot.Initialize();
            Rot.MakeRotationandDoDPS(false, false);
            Rot.AddValidatedSpecialEffects(statsTotal, character.WarriorTalents);

            DPSWarrCharacter charStruct = new DPSWarrCharacter(){
                CalcOpts = calcOpts,
                BossOpts = bossOpts,
                Char = character,
                CombatFactors = combatFactors,
                Rot = Rot,
                Talents = character.WarriorTalents,
                StatS = statsTotal,
                Whiteattacks = whiteAttacks,
            };

            float fightDuration = bossOpts.BerserkTimer;

            List<SpecialEffect> bersMainHand = new List<SpecialEffect>();
            List<SpecialEffect> bersOffHand = new List<SpecialEffect>();

            if (character.MainHandEnchant != null/* && character.MainHandEnchant.Id == 3789*/) { // 3789 = Berserker Enchant ID, but now supporting other proc effects as well
                Stats.SpecialEffectEnumerator mhEffects = character.MainHandEnchant.Stats.SpecialEffects();
                if (mhEffects.MoveNext()) {
                    bersMainHand.Add(mhEffects.Current); 
                }
            }
            if (character.MainHand != null && character.MainHand.Item.Stats._rawSpecialEffectData != null) {
                Stats.SpecialEffectEnumerator mhEffects = character.MainHand.Item.Stats.SpecialEffects();
                if (mhEffects.MoveNext()) { bersMainHand.Add(mhEffects.Current); }
            }
            if (combatFactors.useOH && character.OffHandEnchant != null /*&& character.OffHandEnchant.Id == 3789*/) {
                Stats.SpecialEffectEnumerator ohEffects = character.OffHandEnchant.Stats.SpecialEffects();
                if (ohEffects.MoveNext()) { bersOffHand.Add(ohEffects.Current); }
            }
            if (character.OffHand != null && character.OffHand.Item.Stats._rawSpecialEffectData != null) {
                Stats.SpecialEffectEnumerator ohEffects = character.OffHand.Item.Stats.SpecialEffects();
                if (ohEffects.MoveNext()) { bersOffHand.Add(ohEffects.Current); }
            }
            if (statType == StatType.Average) {
                DoSpecialEffects(charStruct, bersMainHand, bersOffHand, statsTotal);
            }
            else // if (statType == StatType.Maximum)
            {
                Base.StatsWarrior maxSpecEffects = new Base.StatsWarrior();
                foreach (SpecialEffect effect in statsTotal.SpecialEffects()) maxSpecEffects.Accumulate(effect.Stats);
                return UpdateStatsAndAdd(maxSpecEffects as Base.StatsWarrior, combatFactors.StatS, character);
            }
            //UpdateStatsAndAdd(statsProcs, statsTotal, character); // Already done in GetSpecialEffectStats

            // special case for dual wielding w/ berserker enchant on one/both weapons, as they act independently
            //combatFactors.StatS = statsTotal;
            Base.StatsWarrior bersStats = new Base.StatsWarrior();
            foreach (SpecialEffect e in bersMainHand) {
                if (e.Duration == 0) {
                    bersStats.ShadowDamage = e.GetAverageProcsPerSecond(fightDuration / Rot.AttemptedAtksOverDurMH, Rot.LandedAtksOverDurMH / Rot.AttemptedAtksOverDurMH, combatFactors.CMHItemSpeed, calcOpts.SE_UseDur ? fightDuration : 0);
                } else {
                    // berserker enchant id
                    float f = e.GetAverageUptime(fightDuration / Rot.AttemptedAtksOverDurMH, Rot.LandedAtksOverDurMH / Rot.AttemptedAtksOverDurMH, combatFactors.CMHItemSpeed, calcOpts.SE_UseDur ? fightDuration : 0);
                    bersStats.Accumulate(e.Stats, f);
                }
            }
            foreach (SpecialEffect e in bersOffHand) {
                if (e.Duration == 0) {
                    bersStats.ShadowDamage += e.GetAverageProcsPerSecond(fightDuration / Rot.AttemptedAtksOverDurOH, Rot.LandedAtksOverDurOH / Rot.AttemptedAtksOverDurOH, combatFactors.COHItemSpeed, calcOpts.SE_UseDur ? fightDuration : 0);
                } else {
                    float f = e.GetAverageUptime(fightDuration / Rot.AttemptedAtksOverDurOH, Rot.LandedAtksOverDurOH / Rot.AttemptedAtksOverDurOH, combatFactors.COHItemSpeed, calcOpts.SE_UseDur ? fightDuration : 0);
                    bersStats.Accumulate(e.Stats, f);
                }
            }
            combatFactors.StatS = UpdateStatsAndAdd(bersStats, combatFactors.StatS, character);
            combatFactors.InvalidateCache();
            return combatFactors.StatS;
        }

        private void DoSpecialEffects(DPSWarrCharacter charStruct, List<SpecialEffect> bersMainHand, List<SpecialEffect> bersOffHand, Base.StatsWarrior statsTotal)
        {
#if DEBUG
            //ConstructionCounts["DoSpecialEffects"]++;
#endif
            #region Initialize Triggers
            Dictionary<Trigger, float> triggerIntervals = new Dictionary<Trigger, float>();
            Dictionary<Trigger, float> triggerChances = new Dictionary<Trigger, float>();

            CalculateTriggers(charStruct, triggerIntervals, triggerChances);
            #endregion

            #region ArPen Lists
            /*List<float> tempArPenRatings = new List<float>();
            List<float> tempArPenRatingsCapLimited = new List<float>();
            List<float> tempArPenRatingUptimes = new List<float>();
            List<SpecialEffect> tempArPenEffects = new List<SpecialEffect>();
            List<float> tempArPenEffectIntervals = new List<float>();
            List<float> tempArPenEffectChances = new List<float>();
            List<float> tempArPenEffectScales = new List<float>();*/
            #endregion

            // First Let's add InnerRage in, because that affects other calcs
            if (charStruct.CalcOpts.M_InnerRage) {
                AbilityWrapper ir = charStruct.Rot.GetWrapper<Skills.InnerRage>();
                statsTotal.Accumulate(((ir.Ability as Skills.InnerRage).Effect.Stats as Base.StatsWarrior), (ir.Ability as Skills.InnerRage).GetUptime(ir.AllNumActivates));
            }

            List<SpecialEffect> critEffects = new List<SpecialEffect>();

            List<SpecialEffect> firstPass = new List<SpecialEffect>();
            List<SpecialEffect> secondPass = new List<SpecialEffect>();
            List<SpecialEffect> thirdPass = new List<SpecialEffect>();
            //bool doubleExecutioner = false;
            foreach (SpecialEffect effect in statsTotal.SpecialEffects())
            {
                effect.Stats.GenerateSparseData();

                if (!triggerIntervals.ContainsKey(effect.Trigger)) continue;
                else if (effect.Stats.CritRating > 0f)
                {
                    critEffects.Add(effect);
                }
                #region ArP Proc (which don't exist anymore)
                /*else if (effect.Stats.ArmorPenetrationRating > 0f)
                {
                    if (doubleExecutioner) continue;
                    Trigger realTrigger;
                    if (bersMainHand.Contains(effect))
                    {
                        bersMainHand.Remove(effect);
                        doubleExecutioner = true;
                        if (bersOffHand.Contains(effect))
                        {
                            bersOffHand.Remove(effect);
                            realTrigger = Trigger.MeleeHit;
                        }
                        else realTrigger = Trigger.MainHandHit;
                    }
                    else if (bersOffHand.Contains(effect))
                    {
                        bersOffHand.Remove(effect);
                        realTrigger = Trigger.OffHandHit;
                    }
                    else realTrigger = effect.Trigger;
                    tempArPenEffects.Add(effect);
                    tempArPenEffectIntervals.Add(triggerIntervals[realTrigger]);
                    tempArPenEffectChances.Add(triggerChances[realTrigger]);
                    tempArPenEffectScales.Add(1f);
                }*/
                #endregion
                else if (!bersMainHand.Contains(effect) && !bersOffHand.Contains(effect) &&
                   (effect.Stats.Agility > 0f ||
                    effect.Stats.HasteRating > 0f ||
                    effect.Stats.HitRating > 0f ||
                    effect.Stats.CritRating > 0f ||
                    effect.Stats.MasteryRating > 0f ||
                    effect.Stats.PhysicalHaste > 0f ||
                    effect.Stats.PhysicalCrit > 0f ||
                    effect.Stats.PhysicalHit > 0f))
                {
                    // These procs affect rotation
                    firstPass.Add(effect);
                }
                else if (!bersMainHand.Contains(effect) && !bersOffHand.Contains(effect) &&
                   (effect.Stats.FireDamage > 0 ||
                    effect.Stats.FireDamage > 0f ||
                    effect.Stats.NatureDamage > 0f ||
                    effect.Stats.ShadowDamage > 0f ||
                    effect.Stats.HolyDamage > 0f ||
                    effect.Stats.ArcaneDamage > 0f))
                {
                    // It's a Special Damage Proc
                    thirdPass.Add(effect);
                }
                else if (!bersMainHand.Contains(effect) && !bersOffHand.Contains(effect))
                {
                    // Any other stat proc
                    secondPass.Add(effect);
                }
            }

            #region ArP Proc Cap handling
            /*if (tempArPenEffects.Count == 0)
            {
                //tempArPenRatings.Add(0.0f);
                //tempArPenRatingUptimes.Add(1.0f);
            }
            else if (tempArPenEffects.Count == 1)
            { //Only one, add it to
                SpecialEffect effect = tempArPenEffects[0];
                float uptime = effect.GetAverageStackSize(tempArPenEffectIntervals[0], tempArPenEffectChances[0], charStruct.combatFactors.CmhItemSpeed,
                    (charStruct.calcOpts.SE_UseDur ? charStruct.bossOpts.BerserkTimer : 0f)) * tempArPenEffectScales[0];
                tempArPenRatings.Add(effect.Stats.ArmorPenetrationRating);
                tempArPenRatingUptimes.Add(uptime);
                tempArPenRatings.Add(0.0f);
                tempArPenRatingUptimes.Add(1.0f - uptime);
            }
            else if (tempArPenEffects.Count > 1)
            {
                //if (tempArPenEffects.Count >= 2)
                //{
                //    offset[0] = calcOpts.TrinketOffset;
                //}
                WeightedStat[] arPenWeights = SpecialEffect.GetAverageCombinedUptimeCombinations(tempArPenEffects.ToArray(), tempArPenEffectIntervals.ToArray(), tempArPenEffectChances.ToArray(), new float[tempArPenEffectChances.Count], tempArPenEffectScales.ToArray(), charStruct.combatFactors.CmhItemSpeed,
                    charStruct.bossOpts.BerserkTimer,
                    AdditiveStat.ArmorPenetrationRating);
                for (int i = 0; i < arPenWeights.Length; i++)
                {
                    tempArPenRatings.Add(arPenWeights[i].Value);
                    tempArPenRatingUptimes.Add(arPenWeights[i].Chance);
                }
            }*/
            // Get the average Armor Pen Rating across all procs
            /*if (tempArPenRatings.Count > 0f)
            {
                Stats originalStats = charStruct.combatFactors.StatS;
                int LevelDif = charStruct.bossOpts.Level- charStruct.Char.Level;

                float arpenBuffs = 0.0f;

                //float OriginalArmorReduction = StatConversion.GetArmorDamageReduction(charStruct.Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif], originalStats.TargetArmorReduction, arpenBuffs, Math.Max(0f, originalStats.ArmorPenetrationRating));
                float AverageArmorPenRatingsFromProcs = 0f;
                float tempCap = StatConversion.RATING_PER_ARMORPENETRATION - StatConversion.GetRatingFromArmorPenetration(arpenBuffs);

                for (int i = 0; i < tempArPenRatings.Count; i++)
                {
                    tempArPenRatingsCapLimited.Add(Math.Max(0f, Math.Min(tempCap - originalStats.ArmorPenetrationRating, tempArPenRatings[i])));
                    //float bah = StatConversion.GetArmorDamageReduction(charStruct.Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif], originalStats.TargetArmorReduction, arpenBuffs, Math.Max(0f, originalStats.ArmorPenetrationRating + tempArPenRatingsCapLimited[i]));
                    AverageArmorPenRatingsFromProcs += tempArPenRatingUptimes[i] * tempArPenRatingsCapLimited[i];
                }
                Stats dummyStats = new Stats();
                
                //float procArp = StatConversion.GetRatingFromArmorReduction(charStruct.Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif], originalStats.TargetArmorReduction, arpenBuffs, ProccedArmorReduction);
                originalStats.ArmorPenetrationRating += AverageArmorPenRatingsFromProcs;//(procArp - originalStats.ArmorPenetrationRating);                
            }*/
            #endregion

            IterativeSpecialEffectsStats(charStruct, firstPass,  critEffects, triggerIntervals, triggerChances, 0f, true, new Base.StatsWarrior(), charStruct.CombatFactors.StatS);
            IterativeSpecialEffectsStats(charStruct, secondPass, critEffects, triggerIntervals, triggerChances, 0f, false, null, charStruct.CombatFactors.StatS);
            IterativeSpecialEffectsStats(charStruct, thirdPass,  critEffects, triggerIntervals, triggerChances, 0f, false, null, charStruct.CombatFactors.StatS);
        }

        private static void CalculateTriggers(DPSWarrCharacter charStruct, Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances)
        {
#if DEBUG
            //ConstructionCounts["CalculateTriggers"]++;
            string addInfo = "No Additional Info";
#endif
            try
            {
                float fightDuration = charStruct.BossOpts.BerserkTimer;
                float fightDurationO20 = charStruct.Rot.FightDurationO20;
                float fightDurationU20 = charStruct.Rot.FightDurationU20;
#if DEBUG
                addInfo = "FightDur Passed";
#endif
                //float fightDuration2Pass = charStruct.calcOpts.SE_UseDur ? fightDuration : 0;

                float attemptedMH = charStruct.Rot.AttemptedAtksOverDurMH;
                float attemptedOH = charStruct.Rot.AttemptedAtksOverDurOH;
                float attempted = attemptedMH + attemptedOH;

                float landMH = charStruct.Rot.LandedAtksOverDurMH;
                float landOH = charStruct.Rot.LandedAtksOverDurOH;
                float land = landMH + landOH;

                float crit = charStruct.Rot.CriticalAtksOverDur;

                float avoidedAttacks = charStruct.CombatFactors.StatS.Dodge + charStruct.CombatFactors.StatS.Parry;

                float dwbleed = 0;
#if DEBUG
                addInfo += "\r\nBig if started";
#endif
                if (charStruct.Char.WarriorTalents.DeepWounds > 0 && crit != 0f)
                {
                    float dwTicks = 1f;
                    foreach (AbilityWrapper aw in charStruct.Rot.DamagingAbilities)
                    {
                        if (aw.AllNumActivates > 0f && aw.Ability.CanCrit)
                        {
                            if (aw.Ability.SwingsOffHand)
                            {

                                dwTicks *= (float)Math.Pow(1f - aw.Ability.MHAtkTable.Crit * (1f - aw.Ability.OHAtkTable.Crit), aw.AllNumActivates / fightDuration);
                                dwTicks *= (float)Math.Pow(1f - aw.Ability.OHAtkTable.Crit, aw.AllNumActivates / fightDuration);
                            }
                            else
                            {
                                // to fix this breaking apart when you're close to yellow crit cap for these abilities, namely OP
                                if (aw.Ability is Skills.Overpower || aw.Ability is Skills.TasteForBlood)
                                    dwTicks *= (1f - aw.AllNumActivates / fightDuration * aw.Ability.MHAtkTable.Crit);
                                else dwTicks *= (float)Math.Pow(1f - aw.Ability.MHAtkTable.Crit, aw.AllNumActivates / fightDuration);
                            }
                        }
                    }
                    dwbleed = fightDuration * dwTicks;
                }
#if DEBUG
                addInfo += "\r\nBuncha Floats started";
#endif
                float bleed = dwbleed + fightDuration * (charStruct.CombatFactors.FuryStance || !charStruct.CalcOpts.MaintenanceTree[(int)Maintenance.Rend] ? 0f : 1f / 3f);

                float bleedHitInterval = fightDuration / bleed;
                float dwbleedHitInterval = fightDuration / dwbleed;
                float attemptedAtkInterval = fightDuration / attempted;
                float attemptedAtksIntervalMH = fightDuration / attemptedMH;
                float attemptedAtksIntervalOH = fightDuration / attemptedOH;
                //float landedAtksInterval = fightDuration / land;
                float dmgDoneInterval = fightDuration / (land + bleed);
                float dmgTakenInterval = 0f;
                {
                    List<Attack> attacks = charStruct.BossOpts.Attacks.FindAll(a => a.AffectsRole[PLAYER_ROLES.MeleeDPS] && a.Validate);
                    float frequency = 0;
                    foreach (Attack a in attacks) {
                        frequency += 1f / a.AttackSpeed;
                    }
                    dmgTakenInterval = 1f / frequency;
                }
                
#if DEBUG
                addInfo += "\r\nAoETargsFreq Passed";
#endif
                float hitRate = 1, hitRateMH = 1, hitRateOH = 1, critRate = 1;
                if (attempted != 0f)
                {
                    hitRate = land / attempted;
                    critRate = crit / attempted;
                }
                if (attemptedMH != 0f)
                    hitRateMH = landMH / attemptedMH;
                if (attemptedOH != 0f)
                    hitRateOH = landOH / attemptedOH;

#if DEBUG
                addInfo += "\r\nTriggers started";
#endif
                triggerIntervals[Trigger.Use] = 0f;
                triggerChances[Trigger.Use] = 1f;

                triggerIntervals[Trigger.MeleeAttack] = attemptedAtkInterval;
                triggerChances[Trigger.MeleeAttack] = 1f;

                float mhWhites = charStruct.Rot.DPSWarrChar.Whiteattacks.MHActivatesAll;
                triggerIntervals[Trigger.WhiteAttack] = fightDuration / (mhWhites != 0 ? mhWhites : 1f);
                triggerChances[Trigger.WhiteAttack] = 1f;

                triggerIntervals[Trigger.WhiteCrit] = fightDuration / (mhWhites != 0 ? mhWhites : 1f);
                triggerChances[Trigger.WhiteCrit] = charStruct.Rot.DPSWarrChar.Whiteattacks.MHAtkTable.Crit;

                triggerIntervals[Trigger.WhiteHit] = fightDuration / (mhWhites != 0 ? mhWhites : 1f);
                triggerChances[Trigger.WhiteHit] = charStruct.Rot.DPSWarrChar.Whiteattacks.MHAtkTable.AnyLand;

                triggerIntervals[Trigger.MeleeHit] = triggerIntervals[Trigger.PhysicalHit] = triggerIntervals[Trigger.PhysicalAttack] = attemptedAtkInterval;
                triggerChances[Trigger.MeleeHit] = triggerChances[Trigger.PhysicalHit] = hitRate;
                triggerChances[Trigger.PhysicalAttack] = 1f;

                triggerIntervals[Trigger.PhysicalCrit] = triggerIntervals[Trigger.MeleeCrit] = attemptedAtkInterval;
                triggerChances[Trigger.PhysicalCrit] = triggerChances[Trigger.MeleeCrit] = critRate;

                triggerIntervals[Trigger.MainHandHit] = attemptedAtksIntervalMH;
                triggerChances[Trigger.MainHandHit] = hitRateMH;
                triggerIntervals[Trigger.OffHandHit] = attemptedAtksIntervalOH;
                triggerChances[Trigger.OffHandHit] = hitRateOH;

                triggerIntervals[Trigger.DoTTick] = bleedHitInterval;
                triggerChances[Trigger.DoTTick] = 1f;

                triggerIntervals[Trigger.DamageDone] = dmgDoneInterval;
                triggerChances[Trigger.DamageDone] = 1f;

                triggerIntervals[Trigger.DamageOrHealingDone] = dmgDoneInterval; // Need to add Self Heals
                triggerChances[Trigger.DamageOrHealingDone] = 1f;

                triggerIntervals[Trigger.DamageTaken] = dmgTakenInterval;
                triggerChances[Trigger.DamageTaken] = 1f;

                triggerIntervals[Trigger.DamageAvoided] = dmgTakenInterval;
                triggerChances[Trigger.DamageAvoided] = avoidedAttacks;

                triggerIntervals[Trigger.HSorSLHit] = fightDurationO20 / charStruct.Rot.CritHSSlamOverDur;
                triggerChances[Trigger.HSorSLHit] = 1f;

                triggerIntervals[Trigger.ExecuteHit] = fightDurationU20 / charStruct.Rot.GetWrapper<Skills.Execute>().NumActivatesU20;
                triggerChances[Trigger.ExecuteHit] = charStruct.Rot.GetWrapper<Skills.Execute>().Ability.MHAtkTable.AnyLand;

                triggerIntervals[Trigger.DeepWoundsTick] = dwbleedHitInterval;
                triggerChances[Trigger.DeepWoundsTick] = 1f;

                triggerIntervals[Trigger.DarkIntentCriticalPeriodicDamageOrHealing] = (float)charStruct.CalcOpts.DarkIntentDotTickInterval;
                triggerChances[Trigger.DarkIntentCriticalPeriodicDamageOrHealing] = (float)charStruct.CalcOpts.DarkIntentDotTickCritChance / 100f;

                triggerIntervals[Trigger.WWorCleaveHit] = fightDuration / (charStruct.Rot.GetWrapper<Skills.Whirlwind>().AllNumActivates + charStruct.Rot.GetWrapper<Skills.Cleave>().AllNumActivates);
                triggerChances[Trigger.WWorCleaveHit] = 1f;

                if (charStruct.Rot.GetWrapper<Skills.MortalStrike>().NumActivatesO20 > 0 && charStruct.Rot.GetWrapper<Skills.MortalStrike>().NumActivatesU20 > 0) {
                    triggerIntervals[Trigger.MortalStrikeCrit] =
                        (fightDurationO20 / charStruct.Rot.GetWrapper<Skills.MortalStrike>().NumActivatesO20) * (fightDurationO20 / fightDuration) +
                        (fightDurationU20 / charStruct.Rot.GetWrapper<Skills.MortalStrike>().NumActivatesU20) * (fightDurationU20 / fightDuration);
                } else if (charStruct.Rot.GetWrapper<Skills.MortalStrike>().NumActivatesU20 > 0) {
                    triggerIntervals[Trigger.MortalStrikeCrit] = fightDurationU20 / charStruct.Rot.GetWrapper<Skills.MortalStrike>().NumActivatesU20;
                } else if (charStruct.Rot.GetWrapper<Skills.MortalStrike>().NumActivatesO20 > 0) {
                    triggerIntervals[Trigger.MortalStrikeCrit] = fightDurationO20 / charStruct.Rot.GetWrapper<Skills.MortalStrike>().NumActivatesO20;
                } else {
                    triggerIntervals[Trigger.MortalStrikeCrit] = 0f;
                }
                triggerChances[Trigger.MortalStrikeCrit] = charStruct.Rot.GetWrapper<Skills.MortalStrike>().Ability.MHAtkTable.Crit;

                triggerIntervals[Trigger.MortalStrikeHit] = fightDurationO20 / charStruct.Rot.GetWrapper<Skills.MortalStrike>().NumActivatesO20;
                triggerChances[Trigger.MortalStrikeHit] = charStruct.Rot.GetWrapper<Skills.MortalStrike>().Ability.MHAtkTable.AnyLand;

                triggerIntervals[Trigger.ColossusSmashHit] = fightDuration / charStruct.Rot.GetWrapper<Skills.ColossusSmash>().AllNumActivates;
                triggerChances[Trigger.ColossusSmashHit] = charStruct.Rot.GetWrapper<Skills.ColossusSmash>().Ability.MHAtkTable.AnyLand;

                float opActs = charStruct.Rot.GetWrapper<Skills.Overpower>().AllNumActivates
                             + charStruct.Rot.GetWrapper<Skills.TasteForBlood>().AllNumActivates;
                float rbActs = charStruct.Rot.GetWrapper<Skills.RagingBlow>().AllNumActivates;
                triggerIntervals[Trigger.OPorRBAttack] = (((opActs + rbActs) > 0f) ? (fightDuration / (opActs + rbActs)) : 0f);
                triggerChances[Trigger.OPorRBAttack]   = 1f;

#if DEBUG
                addInfo += "\r\nFinished";
#endif
            } catch (Exception ex) {
                new Base.ErrorBox() {
                    Title = "Error Calculating Triggers",
                    Function = "CalculateTriggers()",
#if DEBUG
                    Info = addInfo,
#endif
                    TheException = ex,
                }.Show();
            }
        }

        private Stats IterativeSpecialEffectsStats(DPSWarrCharacter charStruct, List<SpecialEffect> specialEffects, List<SpecialEffect> critEffects,
            Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances, float oldFlurryUptime,
            bool iterate, Base.StatsWarrior iterateOld, Base.StatsWarrior originalStats)
        {
#if DEBUG
            //ConstructionCounts["IterativeSpecialEffectsStats"]++;
#endif
            WarriorTalents talents = charStruct.Char.WarriorTalents;
            float fightDuration = charStruct.BossOpts.BerserkTimer;
            Base.StatsWarrior statsProcs = new Base.StatsWarrior();
            try {
                float dmgTakenInterval = fightDuration / charStruct.BossOpts.AoETargsFreq;

                //float attempted = charStruct.Rot.AttemptedAtksOverDur;
                //float land = charStruct.Rot.LandedAtksOverDur;
                //float crit = charStruct.Rot.CriticalAtksOverDur;

                //int LevelDif = charStruct.bossOpts.Level - charStruct.Char.Level;
                List<Trigger> critTriggers = new List<Trigger>();
                List<float> critWeights = new List<float>();
                bool needsHitTableReset = false;
                foreach (SpecialEffect effect in critEffects) {
                    needsHitTableReset = true;
                    critTriggers.Add(effect.Trigger);
                    critWeights.Add(1f);
                }
                foreach (SpecialEffect effect in specialEffects) {
                    #region old arp code
                    /*if (effect.Stats.ArmorPenetrationRating > 0) {
                        float arpenBuffs =
                            ((combatFactors.CmhItemType == ItemType.TwoHandMace) ? talents.MaceSpecialization * 0.03f : 0.00f) +
                            (!calcOpts.FuryStance ? (0.10f + originalStats.BonusWarrior_T9_2P_ArP) : 0.0f);

                        float OriginalArmorReduction = StatConversion.GetArmorDamageReduction(Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif],
                            originalStats.ArmorPenetration, arpenBuffs, originalStats.ArmorPenetrationRating);
                        float ProccedArmorReduction = StatConversion.GetArmorDamageReduction(Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif],
                            originalStats.ArmorPenetration + effect.Stats.ArmorPenetration, arpenBuffs, originalStats.ArmorPenetrationRating + effect.Stats.ArmorPenetrationRating);

                        Stats dummyStats = new Stats();
                        float procUptime = ApplySpecialEffect(effect, Char, Rot, combatFactors, calcOpts, originalStats.Dodge + originalStats.Parry, ref dummyStats);

                        float targetReduction = ProccedArmorReduction * procUptime + OriginalArmorReduction * (1f - procUptime);
                        //float arpDiff = OriginalArmorReduction - targetReduction;
                        float procArp = StatConversion.GetRatingFromArmorReduction(Char.Level, (int)StatConversion.NPC_ARMOR[LevelDif],
                            originalStats.ArmorPenetration, arpenBuffs, targetReduction);
                        statsProcs.ArmorPenetrationRating += (procArp - originalStats.ArmorPenetrationRating);
                    } 
                    else */
                    #endregion
                    
                    float numProcs = 0;
                    if (effect.Stats.ManaorEquivRestore > 0f && effect.Stats.HealthRestoreFromMaxHealth > 0f) {
                        // effect.Duration = 0, so GetAverageStats won't work
                        float value1 = effect.Stats.ManaorEquivRestore;
                        float value2 = effect.Stats.HealthRestoreFromMaxHealth;
                        SpecialEffect dummy = new SpecialEffect(effect.Trigger, new Stats() { ManaorEquivRestore = value1, HealthRestoreFromMaxHealth = value2 }, effect.Duration, effect.Cooldown, effect.Chance);
                        numProcs = dummy.GetAverageProcsPerSecond(dmgTakenInterval, originalStats.Dodge + originalStats.Parry, 0f, 0f) * fightDuration;
                        statsProcs.ManaorEquivRestore += dummy.Stats.ManaorEquivRestore * numProcs;
                        dummy.Stats.ManaorEquivRestore = 0f;
                        //numProcs = effect.GetAverageProcsPerSecond(triggerIntervals[Trigger.PhysicalCrit], triggerChances[Trigger.PhysicalCrit], 0f, 0f) * fightDuration;
                        //statsProcs.HealthRestoreFromMaxHealth += effect.Stats.HealthRestoreFromMaxHealth * numProcs;
                        ApplySpecialEffect(dummy, charStruct, triggerIntervals, triggerChances, ref statsProcs);
                    } else if (effect.Stats.ManaorEquivRestore > 0f) {
                        // effect.Duration = 0, so GetAverageStats won't work
                        numProcs = effect.GetAverageProcsPerSecond(dmgTakenInterval, originalStats.Dodge + originalStats.Parry, 0f, 0f) * fightDuration;
                        statsProcs.ManaorEquivRestore += effect.Stats.ManaorEquivRestore * numProcs;
                    } else if (effect.Stats.HealthRestoreFromMaxHealth > 0f) {
                        // effect.Duration = 0, so GetAverageStats won't work
                        numProcs = effect.GetAverageProcsPerSecond(dmgTakenInterval, originalStats.Dodge + originalStats.Parry, 0f, 0f) * fightDuration;
                        statsProcs.HealthRestoreFromMaxHealth += effect.Stats.HealthRestoreFromMaxHealth * numProcs;
                    } else {
                        ApplySpecialEffect(effect, charStruct, triggerIntervals, triggerChances, ref statsProcs);
                    }
                }

                WeightedStat[] critProcs;
                if (critEffects.Count == 0) {
                    critProcs = new WeightedStat[] { new WeightedStat() { Value = 0f, Chance = 1f } };
                } else if (critEffects.Count == 1) {
                    float interval = triggerIntervals[critEffects[0].Trigger];
                    float chance = triggerChances[critEffects[0].Trigger];
                    float upTime = critEffects[0].GetAverageStackSize(interval, chance, charStruct.CombatFactors.CMHItemSpeed,
                        (charStruct.CalcOpts.SE_UseDur ? charStruct.BossOpts.BerserkTimer : 0f));
                    upTime *= critWeights[0];
                    critProcs = new WeightedStat[] { new WeightedStat() { Value = critEffects[0].Stats.CritRating, Chance = upTime },
                                                     new WeightedStat() { Value = 0f, Chance = 1f - upTime } };
                } else {
                    float[] intervals = new float[critEffects.Count];
                    float[] chances = new float[critEffects.Count];
                    float[] offset = new float[critEffects.Count];
                    for (int i = 0; i < critEffects.Count; i++) {
                        intervals[i] = triggerIntervals[critEffects[i].Trigger];
                        chances[i] = triggerChances[critEffects[i].Trigger];
                    }

                    critProcs = SpecialEffect.GetAverageCombinedUptimeCombinations(critEffects.ToArray(), intervals, chances, offset, critWeights.ToArray(), charStruct.CombatFactors.CMHItemSpeed,
                        charStruct.BossOpts.BerserkTimer, AdditiveStat.CritRating);
                }
                charStruct.CombatFactors.CritProcs = critProcs;
                float flurryUptime = 0f;
                if (iterate && talents.Flurry > 0f && charStruct.CombatFactors.FuryStance && charStruct.Char.MainHand != null && charStruct.Char.MainHand.Item != null)
                {
                    float numFlurryHits = 3f; // default
                    float mhPerc = 1f; // 100% by default
                    float flurryHaste = 0.25f / 3f * talents.Flurry;
                    bool useOffHand = false;
                    
                    float flurryHitsPerSec = charStruct.CombatFactors.TotalHaste * (1f + flurryHaste) / (1f + flurryHaste * oldFlurryUptime);
                    float temp = 1f / charStruct.Char.MainHand.Item.Speed;
                    if (charStruct.Char.OffHand != null && charStruct.Char.OffHand.Item != null) {
                        useOffHand = true;
                        temp += 1f / charStruct.Char.OffHand.Item.Speed;
                        mhPerc = (charStruct.Char.MainHand.Speed / charStruct.Char.OffHand.Speed) / (1f + charStruct.Char.MainHand.Speed / charStruct.Char.OffHand.Speed);
                        if (charStruct.Char.OffHand.Speed == charStruct.Char.MainHand.Speed) numFlurryHits = 4f;
                    }
                    
                    flurryHitsPerSec *= temp;
                    float flurryDuration = numFlurryHits / flurryHitsPerSec;
                    flurryUptime = 1f;
                    foreach (AbilityWrapper aw in charStruct.Rot.DamagingAbilities) {
                        if (aw.Ability.CanCrit && aw.AllNumActivates > 0f)
                        {
                            float tempFactor = (float) Math.Pow(1f - aw.Ability.MHAtkTable.Crit,
                                                                flurryDuration*
                                                                (aw.AllNumActivates*aw.Ability.SwingsPerActivate*
                                                                 aw.Ability.AvgTargets/fightDuration));
                            flurryUptime *= tempFactor;
                            if (aw.Ability.SwingsOffHand && useOffHand)
                            {
                                flurryUptime *= (float) Math.Pow(1f - aw.Ability.OHAtkTable.Crit,
                                                                 flurryDuration*
                                                                 (aw.AllNumActivates*aw.Ability.SwingsPerActivate*
                                                                  aw.Ability.AvgTargets/fightDuration));
                            }
                        }
                    }
                    flurryUptime *= (float)Math.Pow(1f - charStruct.Rot.DPSWarrChar.Whiteattacks.MHAtkTable.Crit, numFlurryHits * mhPerc);
                    flurryUptime *= (float)Math.Pow(1f - charStruct.Rot.DPSWarrChar.Whiteattacks.OHAtkTable.Crit, numFlurryHits * (1f - mhPerc));
                    flurryUptime = 1f - flurryUptime;
                    statsProcs.PhysicalHaste = (1f + statsProcs.PhysicalHaste) * (1f + flurryHaste * flurryUptime) - 1f;
                }

                charStruct.CombatFactors.StatS = UpdateStatsAndAdd(statsProcs, originalStats, charStruct.Char);
                charStruct.CombatFactors.InvalidateCache();
                //Rot.InvalidateCache();
                if (iterate) {
                    const float precisionWhole = 0.01f;
                    const float precisionDec = 0.0001f;
                    if (statsProcs.Agility - iterateOld.Agility > precisionWhole ||
                        statsProcs.HasteRating - iterateOld.HasteRating > precisionWhole ||
                        statsProcs.HitRating - iterateOld.HitRating > precisionWhole ||
                        statsProcs.CritRating - iterateOld.CritRating > precisionWhole ||
                        statsProcs.PhysicalHaste - iterateOld.PhysicalHaste > precisionDec ||
                        statsProcs.PhysicalCrit - iterateOld.PhysicalCrit > precisionDec ||
                        statsProcs.PhysicalHit - iterateOld.PhysicalHit > precisionDec)
                    {
                        if (needsHitTableReset) charStruct.Rot.ResetHitTables();
                        charStruct.Rot.DoIterations();
                        CalculateTriggers(charStruct, triggerIntervals, triggerChances);
                        return IterativeSpecialEffectsStats(charStruct,
                            specialEffects, critEffects, triggerIntervals, triggerChances, flurryUptime, true, statsProcs, originalStats);
                    } else { /*int j = 0;*/ }
                }

                return statsProcs;
            } catch (Exception ex) {
                new Base.ErrorBox()
                {
                    Title = "Error in creating SpecialEffects Stats",
                    Function = "GetSpecialEffectsStats()",
                    TheException = ex,
                }.Show();
                return new Stats();
            }
        }

        private float ApplySpecialEffect(SpecialEffect effect, DPSWarrCharacter charStruct, Dictionary<Trigger, float> triggerIntervals, Dictionary<Trigger, float> triggerChances, ref Base.StatsWarrior applyTo)
        {
#if DEBUG
            //ConstructionCounts["ApplySpecialEffect"]++;
#endif
            float fightDuration = charStruct.BossOpts.BerserkTimer;
            float fightDuration2Pass = charStruct.CalcOpts.SE_UseDur ? fightDuration : 0;

            Stats effectStats = effect.Stats;

            float upTime = 0f;
            //float avgStack = 1f;
            /*if (effect.Stats.TargetArmorReduction > 0f || effect.Stats.ArmorPenetrationRating > 0f) {
                //int j = 0;
            }*/
            if (effect.Trigger == Trigger.Use) {
                if (effect.Stats._rawSpecialEffectDataSize == 1) {
                    upTime = effect.GetAverageUptime(0f, 1f, charStruct.CombatFactors.CMHItemSpeed, fightDuration2Pass);
                    //float uptime =  (effect.Cooldown / fightDuration);
                    List<SpecialEffect> nestedEffect = new List<SpecialEffect>();
                    nestedEffect.Add(effect.Stats._rawSpecialEffectData[0]);
                    Base.StatsWarrior _stats2 = new Base.StatsWarrior();
                    ApplySpecialEffect(effect.Stats._rawSpecialEffectData[0], charStruct, triggerIntervals, triggerChances, ref _stats2);
                    effectStats = _stats2;
                } else {
                    upTime = effect.GetAverageStackSize(0f, 1f, charStruct.CombatFactors.CMHItemSpeed, fightDuration2Pass); 
                }
            } else if (effect.Duration == 0f && triggerIntervals.ContainsKey(effect.Trigger) && !float.IsInfinity(triggerIntervals[effect.Trigger])) {
                upTime = effect.GetAverageProcsPerSecond(triggerIntervals[effect.Trigger], 
                                                         triggerChances[effect.Trigger],
                                                         charStruct.CombatFactors.CMHItemSpeed,
                                                         fightDuration2Pass);
            } else if (effect.Trigger == Trigger.ExecuteHit) {
                upTime = effect.GetAverageStackSize(triggerIntervals[effect.Trigger], 
                                                         triggerChances[effect.Trigger],
                                                         charStruct.CombatFactors.CMHItemSpeed,
                                                         fightDuration2Pass * (float)charStruct.BossOpts.Under20Perc);
            } else if (triggerIntervals.ContainsKey(effect.Trigger) && !float.IsInfinity(triggerIntervals[effect.Trigger])) {
                upTime = effect.GetAverageStackSize(triggerIntervals[effect.Trigger], 
                                                         triggerChances[effect.Trigger],
                                                         charStruct.CombatFactors.CMHItemSpeed,
                                                         fightDuration2Pass);
            }

            if (upTime > 0f) {
                if (effect.Duration == 0f) {
                    applyTo.Accumulate(effectStats, upTime * fightDuration);
                } else if (upTime <= effect.MaxStack) {
                    applyTo.Accumulate(effectStats, upTime);
                }
                return upTime;
            }
            return 0f;
        }

        private static Base.StatsWarrior UpdateStatsAndAdd(Base.StatsWarrior statsToAdd, Base.StatsWarrior baseStats, Character character)
        {
#if DEBUG
            //ConstructionCounts["UpdateStatsAndAdd"]++;
#endif
            Base.StatsWarrior retVal;
            float newStaMult = 1f + statsToAdd.BonusStaminaMultiplier;
            float newStrMult = 1f + statsToAdd.BonusStrengthMultiplier;
            float newAgiMult = 1f + statsToAdd.BonusAgilityMultiplier;
            float newArmMult = 1f + statsToAdd.BonusArmorMultiplier;
            float newBaseArmMult = 1f + statsToAdd.BaseArmorMultiplier;
            float newAtkMult = 1f + statsToAdd.BonusAttackPowerMultiplier;
            float newHealthMult = 1f + statsToAdd.BonusHealthMultiplier;
            if (baseStats != null) {
                retVal = baseStats.Clone();
                
                newStaMult *= (1f + retVal.BonusStaminaMultiplier);
                newStrMult *= (1f + retVal.BonusStrengthMultiplier);
                newAgiMult *= (1f + retVal.BonusAgilityMultiplier);
                newArmMult *= (1f + retVal.BonusArmorMultiplier);
                newBaseArmMult *= (1f + retVal.BaseArmorMultiplier);
                newAtkMult *= (1f + retVal.BonusAttackPowerMultiplier);
                newHealthMult *= (1f + retVal.BonusHealthMultiplier);

                // normalize retVal with its old base stat values, since we're updating them below
                // This essentially reverses what gets done to statsToAdd, but only things that
                // are affected by multipliers (like base stats, armor, AP, etc)
                
                retVal.Health -= retVal.Stamina * 10f; // Stamina is affected by a multiplier

                // Since AP is set to (RawAP + 2*STR + A2T + BonusAP)*APMult, and Str/A2T are affected by mults too,
                // we need to rewind the Str and Armor components out.  We will add them after we've updated Str/Armor, below
                retVal.AttackPower /= 1f + retVal.BonusAttackPowerMultiplier;
                retVal.AttackPower -= (retVal.Strength * 2f);

                    // This is reversing the Armor = (Armor*BaseMult + Bonus)*BonusMult
                retVal.Armor /= 1f + retVal.BonusArmorMultiplier;
                retVal.Armor -= retVal.BonusArmor;
                retVal.Armor /= 1f + retVal.BaseArmorMultiplier;
                //retVal.BonusArmor -= retVal.Agility * 2f;
                
                // Agi is multed, remove it from PhysicalCrit for now
                retVal.PhysicalCrit -= StatConversion.GetCritFromAgility(retVal.Agility, character.Class);
            } else { retVal = null; }

            //statsToAdd.DeathbringerProc = 0f;

            #region Base Stats
            statsToAdd.Stamina  *= newStaMult;
            statsToAdd.Strength *= newStrMult;
            statsToAdd.Agility *= newAgiMult;

            if (retVal != null)
            {
                // change retvals to use the new mults. Combines Stat/=oldMult; Stat*=newMult
                retVal.Stamina *= newStaMult / (1f + retVal.BonusStaminaMultiplier);
                retVal.Strength *= newStrMult / (1f + retVal.BonusStrengthMultiplier);
                retVal.Agility *= newAgiMult / (1f + retVal.BonusAgilityMultiplier);
            }
            #endregion

            #region Health
            statsToAdd.Health *= newHealthMult;
            statsToAdd.Health += (statsToAdd.Stamina * 10f);
            if (retVal != null)
            {
                // Combines rollback of oldmult and addition of newmult
                retVal.Health *= newHealthMult / (1f + retVal.BonusHealthMultiplier);
                retVal.Health += retVal.Stamina * 10f;
            }
            #endregion

            #region Armor
            //statsToAdd.BonusArmor += statsToAdd.Agility * 2f;
            statsToAdd.Armor = (statsToAdd.Armor * newBaseArmMult + statsToAdd.BonusArmor) * newArmMult;
            if (retVal != null)
            {
                //retVal.BonusArmor += retVal.Agility * 2f;
                retVal.Armor = (retVal.Armor * newBaseArmMult + retVal.BonusArmor) * newArmMult;
            }
            #endregion

            #region Attack Power
            // stats to add
            statsToAdd.AttackPower += (statsToAdd.Strength * 2f);
            statsToAdd.AttackPower *= newAtkMult;
            // reset retval
            if (retVal != null) {
                // already rolled back AP's oldmult, so not combining
                retVal.AttackPower += (retVal.Strength * 2f);
                retVal.AttackPower *= newAtkMult;
            }
            #endregion

            // Crit
            statsToAdd.PhysicalCrit += StatConversion.GetCritFromAgility(statsToAdd.Agility, character.Class);
            statsToAdd.PhysicalCrit += StatConversion.GetCritFromRating(statsToAdd.CritRating, character.Class);
            if (retVal != null) {
                retVal.PhysicalCrit += StatConversion.GetCritFromAgility(retVal.Agility, character.Class);
            }

            // Haste
            statsToAdd.PhysicalHaste = (1f + statsToAdd.PhysicalHaste)
                                     * (1f + StatConversion.GetPhysicalHasteFromRating(Math.Max(0, statsToAdd.HasteRating), character.Class))
                                     - 1f;

            // If we're adding two, then return the .Accumulate
            if (retVal != null) {
                retVal.Accumulate(statsToAdd);

                // Paragon and its friends
                if (retVal.Paragon > 0f || retVal.HighestStat > 0f) {
                    float paragonValue = retVal.Paragon + retVal.HighestStat; // how much paragon to add
                    retVal.Paragon = retVal.HighestStat = 0f; // remove Paragon stat, since it's not needed
                    if (retVal.Strength > retVal.Agility) { // Now that we've added the two stats, we run UpdateStatsAndAdd again for paragon {
                        return UpdateStatsAndAdd(new Base.StatsWarrior { Strength = paragonValue }, retVal, character);
                    } else {
                        return UpdateStatsAndAdd(new Base.StatsWarrior { Agility = paragonValue }, retVal, character);
                    }
                } else if (retVal.HighestSecondaryStat > 0) {
                    float paragonValue = retVal.HighestSecondaryStat; // how much paragon to add
                    retVal.HighestSecondaryStat = 0f; // remove Paragon stat, since it's not needed
                    if (retVal.CritRating > retVal.HasteRating && retVal.CritRating > retVal.MasteryRating) {
                        return UpdateStatsAndAdd(new Base.StatsWarrior { CritRating = paragonValue }, retVal, character);
                    } else if (retVal.HasteRating > retVal.CritRating && retVal.HasteRating > retVal.MasteryRating) {
                        return UpdateStatsAndAdd(new Base.StatsWarrior { HasteRating = paragonValue }, retVal, character);
                    } else /*if (retVal.MasteryRating > retVal.CritRating && retVal.MasteryRating > retVal.HasteRating)*/ {
                        return UpdateStatsAndAdd(new Base.StatsWarrior { MasteryRating = paragonValue }, retVal, character);
                    }
                } else { return retVal; }
            } else { return statsToAdd; } // Just processing one, not adding two
        }

        #endregion
    }
}
