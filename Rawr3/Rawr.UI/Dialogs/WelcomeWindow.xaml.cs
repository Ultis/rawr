using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rawr.UI
{
    public partial class WelcomeWindow : ChildWindow
    {
        public WelcomeWindow()
        {
            InitializeComponent();
            //
            SetUpTips();
            SetUpFAQ();
            SetUpPatchNotes();
            SetUpKI();
        }
        
        private Dictionary<string, string> TipStuff = new Dictionary<string, string>();
        private Dictionary<string, string> FAQStuff = new Dictionary<string, string>();
        private Dictionary<string, string> VNStuff = new Dictionary<string, string>();
        private Dictionary<string, string> KIStuff = new Dictionary<string, string>();

        private void SetUpTips()
        {
            TipStuff.Add(@"You can save talent builds by clicking the Save button, and then compare different builds in the Talent Specs chart.", "");
            TipStuff.Add(@"You can use the All Buffs chart to find the best food, elixir, or flask for your character.", "");
            TipStuff.Add(@"By marking the green diamond next to items, gems, and enchants, you indicate that you have them available to you. The Optimizer can use that information to find the best set of gear available to you, or to build a list of potential upgrades for you, and their upgrade values.", "");
            TipStuff.Add(@"When using the Optimizer, you can use the Additional Constraints feature to enforce requirements such as being uncrittable, being hit capped, maintaining a certain level of survivability, a minimum haste %, and much much more.", "");
            TipStuff.Add(@"You can use Batch Tools to figure out which spec of yours performs the best given your gear!", "");
            TipStuff.Add(@"Rawr Batch Tools can help you find gear that's an upgrade for more than one of your specs!", "");
            TipStuff.Add(@"The direct-upgrades chart will quickly show you where your current gear is lacking.", "");
            TipStuff.Add(@"If you can never remember which class gives which buff, you can set an option to display the source of each buff in Tools : Options : General Settings.", "");
            TipStuff.Add(@"You can select which language (English, French, German, Spanish, Russian) to view the item names in by selecting your language in Tools : Options : General Settings.", "");
            TipStuff.Add(@"In addition to the gems you mark available, the Optimizer will also use any gems included in any enabled Gemming Template. You can disable that in Tools : Options : Optimizer Settings, if you prefer to manually choose what gems are available.", "");
            TipStuff.Add(@"You can choose which gemming templates Rawr uses to gem items in the charts in Tools : Edit Gemming Templates.", "");
            TipStuff.Add(@"Jewelcrafters can tell Rawr to use their Dragon's Eye gems by ticking the Jeweler gems section in Tools : Edit Gemming Templates.", "");
            TipStuff.Add(@"You can force the Optimizer to use only a specific gemming of an item by CTRL-clicking the diamond in the gear list (it will turn blue).", "");
            TipStuff.Add(@"You can force the Optimizer to use only a specific enchanting of an item by right-clicking the diamond and selecting the enchant (it will show a red dot).", "");
            TipStuff.Add(@"Upgrade Lists provide a huge advantage over the simple comparison charts. The comparison charts will show you the values for individual items, while the Upgrade List will show you each item's upgrade including the optimal combination of items that you have available to go with that item. Use Tools : Optimizer : Build Upgrade List to get started!", "");
            TipStuff.Add(@"If Rawr is valuing items for your caster character which focus on mana regen higher than you'd expect, you may be running out of mana. Ensure that you have all the appropriate buffs checked, especially for mana regen.", "");
            TipStuff.Add(@"You can set an option in the Tools : Options menu to show or hide profession buffs that your character doesn't know.", "");
            TipStuff.Add(@"You can view your character in 3D on Wowhead.com by using the menu off the Tools menu.", "");

            CB_Tips.Items.Add((String)"All");
            String[] arr = new String[TipStuff.Keys.Count];
            TipStuff.Keys.CopyTo(arr, 0);
            foreach (String a in arr) { CB_Tips.Items.Add(a); }
            CB_Tips.SelectedIndex = 0;
            CB_Tips_SelectedIndexChanged(null, null);
        }
        private void SetUpFAQ()
        {
FAQStuff.Add(
"Why is the Mortal Strike talent shown with negative DPS in the Talent Comparison Pane? The ability is doing x DPS.",
@"When the standard rotation abilities for Arms are active (including Slam and Heroic Strike) the large rage consumption of the Mortal Strike Ability tends to overshadow the rage left-over for Heroic Strikes. Basically, if you were to Slam instead of Mortal Strike on every time you would have otherwise, there would be more rage left over to Heroic Strike. In some cases, Rawr sees this as a DPS gain and wants you to drop Mortal Strike. Fully 25 Man raid buffed, Mortal Strike should have a higher DPS value than the rage to Heroic Strikes would provide."
);
            
            CB_FAQ_Questions.Items.Add((String)"All");
            String[] arr = new String[FAQStuff.Keys.Count];
            FAQStuff.Keys.CopyTo(arr, 0);
            foreach (String a in arr) { CB_FAQ_Questions.Items.Add(a); }
            CB_FAQ_Questions.SelectedIndex = 0;
            CB_FAQ_Questions_SelectedIndexChanged(null, null);
        }
        private void SetUpPatchNotes()
        {
#region Rawr 4.0.11 (Unreleased, Planned as Dec 19-20, 2010) [last updated with r56250]
VNStuff.Add(
"Rawr 4.0.11 (Unreleased, Planned as Dec 19-20, 2010) [last updated with r56250]",
@"Cataclysm Release Beta

Rawr.Addon
- Version 0.07
- - Reworked the Character XML to be same as save from Rawr
- Version 0.08
- - Removed Equipped Block for equipped items in export
- - Removed alternate talent lists for other classes
- - Fixed extra line break at start of file
- - Added extra indent for available items for visual inspection
- - Refactor slot working
- - Dry coded blacksmithing socket check
- - Has sockets returns true for time being
- Version 0.09
- - Fixed some parsing issues to make it work
- - Export Class name as lowercase
- - Flag as v0.09 in TOC
- - Updated version check for Rawr AddOn to 0.09 and added version notes to the LUA
- - Added RawrBuild tag to export to allow Rawr to know what minimum build addon supports.

Rawr.Base:
- Created Import from Rawr AddOn process in program!
- - only Imports what you are wearing
- - Doesn't mark as available to the optimizer yet
- - Gems in sockets didn't work on test toon, need to investigate
- Fix for Issue 19269:  Enchant Cloak - Protection should give bonus armor - Changed to Bonus Armor
- Hid Add/Delete Custom Gemming menu options as they are non-functional and will be removed from program
- Changed ErrorBox to use a Silverlight ErrorWindow method instead of MessageBox so that we can keep it more internal and give a better window
- Armory Imports that fail with a not Found will state better messaging
- Enable Enchant Off-Hand - Superior Intellect.
- Potential fix for no saved file.
- Fix for bad merge.
- Fix for optimization without any available cogwheel/hydraulic.
- For the most part you should now be able to manually set sources for items. Please report bugs in the Issue Tracker on this.
- Fixed null checks
- More work for Item Sourcing, can now properly store and recall up to three sources per item and they show all of them on the Item Tooltips and should all be filterable against. If any one source is available from current filters it should show. Turn off all filters related to the sources to hide it.
- ilevel box on the Edit Item dialog was persisting between items. Changed handling so that it won't
- Same thing for Min/Max Weapon damage
- Added parsing for a trinket
- ItemFilters had a couple || in it, so those filters were matching everything and you couldn't properly hide a lot of items
- Fixed some bugs with Item Source editing
- More ItemSource fixes
- Fix for issue 19301: Copy Data to Clipboard on the Direct Upgrades->Gear chart only copies head-slot items - the list that Copy to Clipboard uses was only being Updated on item charts. Added checks to every chart type so they will all Export
- Fix for Issue 19292: Export to CSV generates FileSecurityState_OperationNotPermitted error - Tested that this cannot be performed in browser. When Installed Offline you can. Set a check to see if you were running Offline and if not will alert User that they need to install Offline before they can do this.
- Fix for Issue 19287: Editing/importing items seems to add them to the cache, but are unavailable to equip. or optimizer - I think a large part of this issue is users not switching to the right model or race beforehand so the Items are filtered out as irrelevant - Added a check to ensure the item shows in the chart if it is Relevant
- Fix for Issue 18697: Pawn export doesn't consider value of weapon DPS - Added Pawn Export function from Rawr2 - Added check for melee classes to add MeleeDPS1 to the string so that it gets a value
- Updated a few shaman races base stats
- New Rawr4 Welcome Window with a lot more noob friendliness
- More Welcome Window stuff
- Additional support for model based reforging restrictions

Rawr.Bear:
- Fix for Issue 19244: Ignoring Threat Rating Customization - Added ThreatScale to the ThreatPoints value

Rawr.DK:
- Fix for Defect 19241: Items not showing on list - This was due to bad gemming templates. So I shamelessly stole the template format from DPSWarr. It works great!
- Fix for possible rotation exception Found during new unit test.
- Fix for sorting issue in sorting by DPS/TPS
- Fix for BB weirdness.
- Fix bug w/ exception in rotation cost evaluation. Found during unittest.
- Fixed Tanking runes
- Fixed special effects handler
- Fixed Blood Parasite implementation
- Implemented basic RPP5 code
- Implemented combinedswing time for DW.
- Implemented Mitigation Subvalues like the survival subvalues.
- Fix for 19288: Hang when looking at trinkets.
- Unittests failing because of a config change.

Rawr.DPSWarr:
- Heroic Throw can no longer go nuts and take over your rotation
- Random work on stuff
- Worked on Heroic Strike/Cleave and how it interacts with the rotation. I'm liking the numbers I'm seeing and Rage is a lot smoother in interaction. This also greatly boosted Haste's relative value and it's not spikey in the test files like it was before.
- Changed the weighting of Normal v Exec phase so that they are based on percentage of time instead of direct average.

Rawr.Elemental:
- Activated the Gemming Templates so Items would show up
- Fixed a Null bug with Frost Shock
- It comes up without crashing now

Rawr.HealPriest:
- Fix for Issue 19280: Model not loading - Uncommented the RegisterModel command, now it loads

Rawr.Mage:
- Support for mastery procs (arcane only for now, probably slightly overestimates).
- Partial support for int procs (includes int=> spell power conversion, regen from max mana and negative impact on mana adept, does not include int=>crit conversion at the moment)
- Fix for int procs.
- Added Draenei hit bonus.

Rawr4.Moonkin:
* Fix double-counting base Druid crit.
* Fix for defect #19282: Items, talents, etc., have no value when no character is loaded.
- Fix for Issue 19317: SpellPower and Hit slightly off - User reported that base Intellect and Spirit don't factor into SpellPower and Hit fully. Implemented suggested fix

Rawr.ProtPaladin:
- Fix for Issue 19281: Items not loading - No Gemming Templates were enabled by default (due to there being no Epic Templates this early in Cataclysm). Set Rare templates to enable instead of Epic

Rawr.RestoSham:
- New gemming templates created and working, default set to use rares
- Fixed a parenthesis error

Rawr.ShadowPriest:
- Fix for Issue 19320: Mastery Rating doesn't show up for ShadowPriest Items - Added Mastery Rating as a Relevant Stat, though it will have no value

Rawr.TankDK:
- Fix for Runetap
- Fix for Mitigation values being Sky high - I was double adding the TotalDPS to PhysicalDPS from boss.
- Some other massaging
"
);
#endregion
#region Rawr 4.0.10 (Dec 06, 2010 23:53) [r56001]
VNStuff.Add(
"Rawr 4.0.10 (Dec 06, 2010 23:53) [r56001]",
@"Cataclysm Pre-Release Beta with first wave of Fixes

Rawr.Addon:
- Fixed drycoding errors

Rawr4.Base:
- Hid the Force Refresh box as we don't have caching on the server yet. 
- Fix for Issue 19240 - Tooltips for Cloaks were not showing their Additional Info strings (iLvl, Id, etc). - Cloaks used to be considered ItemType.Cloth but now they are ItemType.None. Needed to add a check to ensure the slot was still Back like we do for Necks, Rings and Trinkets
- Updated the Welcome Window with newer/better info and forced it to show always until we make a couple releases from this one.
- Fixed a null reference bug with Icons 

Rawr.Base.Items:
- Updated source information for all Reputation and Crafted Gear
- Added any Reputation/Crafted gear that were not in before
- Added Guild Reputation Heirloom heads and backs 
- Changed default on Enter ID form to Use Wowhead 
- Loading from BNet Armory now checks wowhead for items that are not in the database. This will be VERY slow when it occurs for a couple minutes, but then it will clear up when its done and when you close out those items are saved so you won't have to get them again.
- Removed the proxying process for communicating with wowhead. This should fix wowhead item loading for a lot of people 

Rawr.Mage:
- Make sure spell cost reduction can't make base spell cost negative."
);
#endregion
#region Rawr 4.0.09 (Dec 06, 2010 11:59) [r55987]
VNStuff.Add(
"Rawr 4.0.09 (Dec 06, 2010 11:59) [r55987]",
@"Cataclysm Pre-Release Beta"
);
#endregion

            CB_Version.Items.Add("All");
            String[] arr = new String[VNStuff.Keys.Count];
            VNStuff.Keys.CopyTo(arr, 0);
            foreach (String a in arr) { CB_Version.Items.Add(a); }
            CB_Version.SelectedIndex = 0;
            CB_Version_SelectedIndexChanged(null, null);
        }
        private void SetUpKI()
        {
KIStuff.Add(
"Many Models Don't work",
@"Due to the Cataclysm release, many models have not been fully coded as Cataclysm Ready.
Those that have been will most likely have several core bugs while we work out the specifics.
Note that some models are in fact ready, such as DPSWarr.Arms, Mage, Bear and Cat"
);

KIStuff.Add(
"19329 [RestoSham][Optimizer] Optimizer giving inconsistent returns",
"Please see the online issue for details"
);
KIStuff.Add(
"19318 [Optimizer] Giving odd suggestions, at least for Moonkin",
"Please see the online issue for details"
);
KIStuff.Add(
"19328 [Boss] GetDPSbyType always uses avoidance stats.",
"Please see the online issue for details"
);
KIStuff.Add(
"19313 [Base] Application stops working after a few seconds, shows only a white screen",
"Please see the online issue for details"
);
KIStuff.Add(
"19324 Resilience reforging",
"Please see the online issue for details"
);
KIStuff.Add(
"19264 [Base] Maximizing Offline Rawr 4 causes huge memory usage jump",
"Please see the online issue for details"
);
KIStuff.Add(
"19307 [Base] Base Stats are still level 80 for most Classes",
"Please see the online issue for details"
);
KIStuff.Add(
"19308 [Mage] Gale of Shadows Item not Modeled",
"Please see the online issue for details"
);
KIStuff.Add(
"19208 [Items] Unable to load in source info from Wowhead",
"Please see the online issue for details"
);
KIStuff.Add(
"19304 [ItemFilters] Some Parent Filters not Applied on Startup",
"Please see the online issue for details"
);
KIStuff.Add(
"17811 [Optimizer.Batch] Items not considered properly",
"Please see the online issue for details"
);
KIStuff.Add(
"17000 [Base] Fuzzy Text",
"Please see the online issue for details"
);
KIStuff.Add(
"18134 [Mac] Filter pull-down displays no options",
"Please see the online issue for details"
);
KIStuff.Add(
"18787 [Base] Tooltips clipped on the right",
"Please see the online issue for details"
);
KIStuff.Add(
"19270 [LoadCharacter] Unable to import character from Armory/Battle.net",
"Please see the online issue for details"
);
KIStuff.Add(
"19267 [DK.TankDK] Avoidance stats showing negative mitigation",
"Please see the online issue for details"
);
KIStuff.Add(
"18380 [Optimizer] Optimizer score not matching DPS value in main UI after optimizing",
"Please see the online issue for details"
);

            CB_Issues.Items.Add((String)"All");
            String[] arr = new String[KIStuff.Keys.Count];
            KIStuff.Keys.CopyTo(arr, 0);
            foreach (String a in arr) { CB_Issues.Items.Add(a); }
            CB_Issues.SelectedIndex = 0;
            CB_Issues_SelectedIndexChanged(null, null);
        }

        private void CB_Tips_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            //try {
            string text = "";
            if ((String)CB_Tips.SelectedItem == "All")
            {
                int Iter = 1;
                text += "== CONTENTS ==" + "\n";
                foreach (string s in TipStuff.Keys)
                {
                    text += Iter.ToString("00") + ". " + s + "\n"; // Tip
                    Iter++;
                } Iter = 1;
                /*text += "\n";
                text += "== READ ON ==" + "\n";
                foreach (string s in TipStuff.Keys)
                {
                    text += Iter.ToString("00") + ". " + s + "\n"; // Tip
                    text += "\n" + "\n";
                    Iter++;
                } Iter = 1;*/
                RTB_Tips.Text = text.Trim();
            }
            else
            {
                string s = (String)CB_Tips.SelectedItem;
                string a = "invalid";
                bool ver = TipStuff.TryGetValue(s, out a);
                text += s + "\n";
                text += "\n";
                text += (ver ? a : "An error occurred calling the string");
                RTB_Tips.Text = text.Trim();
            }
            /*} catch(Exception ex){
                ErrorBox eb = new ErrorBoxDPSWarr("Error in setting the Tip Item",
                    ex, "CB_Tip_SelectedIndexChanged");
                eb.show();
            }*/
        }
        private void CB_FAQ_Questions_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            //try {
            string text = "";
            if ((String)CB_FAQ_Questions.SelectedItem == "All")
            {
                int Iter = 1;
                text += "== CONTENTS ==" + "\n";
                foreach (string s in FAQStuff.Keys)
                {
                    text += Iter.ToString("00") + "Q. " + s + "\n"; // Question
                    Iter++;
                } Iter = 1;
                text += "\n";
                text += "== READ ON ==" + "\n";
                foreach (string s in FAQStuff.Keys)
                {
                    string a = "invalid";
                    text += Iter.ToString("00") + "Q. " + s + "\n"; // Question
                    bool ver = FAQStuff.TryGetValue(s, out a);
                    text += Iter.ToString("00") + "A. " + (ver ? a : "An error occurred calling the string") + "\n"; // Answer
                    text += "\n" + "\n";
                    Iter++;
                } Iter = 1;
                RTB_FAQ.Text = text.Trim();
            }
            else
            {
                string s = (String)CB_FAQ_Questions.SelectedItem;
                string a = "invalid";
                bool ver = FAQStuff.TryGetValue(s, out a);
                text += s + "\n";
                text += "\n";
                text += (ver ? a : "An error occurred calling the string");
                RTB_FAQ.Text = text.Trim();
            }
            /*} catch(Exception ex){
                ErrorBox eb = new ErrorBoxDPSWarr("Error in setting the FAQ Item",
                    ex, "CB_FAQ_Questions_SelectedIndexChanged");
                eb.show();
            }*/
        }
        private void CB_Version_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = "";
            if ((String)CB_Version.SelectedItem == "All")
            {
                int Iter = 1;
                text += "== CONTENTS ==" + "\r\n";
                foreach (string s in VNStuff.Keys)
                {
                    text += s + "\r\n";
                    Iter++;
                } Iter = 1;
                text += "\r\n";
                text += "== READ ON ==" + "\r\n";
                foreach (string s in VNStuff.Keys)
                {
                    string a = "invalid";
                    text += s + "\r\n";
                    bool ver = VNStuff.TryGetValue(s, out a);
                    text += (ver ? a : "An error occurred calling the string") + "\r\n";
                    text += "\r\n" + "\r\n";
                    Iter++;
                } Iter = 1;
                RTB_Version.Text = text.Trim();
            }
            else
            {
                string s = (String)CB_Version.SelectedItem;
                string a = "invalid";
                bool ver = VNStuff.TryGetValue(s, out a);
                text += s + "\r\n";
                text += "\r\n";
                text += (ver ? a : "An error occurred calling the string");
                RTB_Version.Text = text.Trim();
            }
        }
        private void CB_Issues_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            //try {
            string text = "";
            if ((String)CB_Issues.SelectedItem == "All")
            {
                int Iter = 1;
                text += "== CONTENTS ==" + "\n";
                foreach (string s in KIStuff.Keys)
                {
                    text += Iter.ToString("00") + "I. " + s + "\n"; // Issue
                    Iter++;
                } Iter = 1;
                text += "\n";
                text += "== READ ON ==" + "\n";
                foreach (string s in KIStuff.Keys)
                {
                    string a = "invalid";
                    text += Iter.ToString("00") + "I. " + s + "\n"; // Issue
                    bool ver = KIStuff.TryGetValue(s, out a);
                    text += Iter.ToString("00") + "W. " + (ver ? a : "An error occurred calling the string") + "\n"; // WorkAround
                    text += "\n" + "\n";
                    Iter++;
                } Iter = 1;
                RTB_Issues.Text = text.Trim();
            }
            else
            {
                string s = (String)CB_Issues.SelectedItem;
                string a = "invalid";
                bool ver = KIStuff.TryGetValue(s, out a);
                text += s + "\n";
                text += "\n";
                text += (ver ? a : "An error occurred calling the string");
                RTB_Issues.Text = text.Trim();
            }
            /*} catch(Exception ex){
                ErrorBox eb = new ErrorBoxDPSWarr("Error in setting the Known Issue Item",
                    ex, "CB_Issues_SelectedIndexChanged");
                eb.show();
            }*/
        }

        private void BT_CreateNew_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Instance.NewCharacter(null, null);
            this.DialogResult = true;
        }

        private void BT_LoadBNet_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Instance.LoadFromBNet(null, null);
            this.DialogResult = true;
        }

        private void BT_LoadRawrAddOn_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Instance.LoadFromRawrAddon(null, null);
            this.DialogResult = true;
        }

        private void BT_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Instance.OpenCharacter(null, null);
            this.DialogResult = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Rawr.Properties.GeneralSettings.Default.WelcomeScreenSeen = true;
            this.DialogResult = true;
        }

#if !SILVERLIGHT
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
#endif
    }
}

