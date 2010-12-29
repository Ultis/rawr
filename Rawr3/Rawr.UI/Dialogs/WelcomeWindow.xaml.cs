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
#if !SILVERLIGHT
            //this.Owner = MainPage.Instance.Parent as Window;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            this.WindowState = System.Windows.WindowState.Normal;
#endif
            //
            SetUpTips();
            SetUpFAQ();
            SetUpPatchNotes();
            SetUpKI();
        }

        #region Variables
        private Dictionary<string, string> TipStuff = new Dictionary<string, string>();
        private Dictionary<string, string> FAQStuff = new Dictionary<string, string>();
        private Dictionary<string, string> VNStuff = new Dictionary<string, string>();
        private Dictionary<string, string> KIStuff = new Dictionary<string, string>();
        #endregion

        // Set Up Information for display
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
"Where can I set the Stat Weights in Rawr?",
@"Rawr absolutely, flat out, completely, in no way uses Stat Weights (the idea of assigning a Stat a value and using that to score gear, Eg- STR = 1.2 so an item with 20 STR would be value 24). Instead, each Stat is calculated into a modeling process that includes all of the class' abilities with respect to the character as a whole. The gear comparison then becomes the total character change that item would provide. DPS classes see DPS changes, Tanks see Survivability changes, and Healers see HPS changes. This is what sets Rawr apart from most other modeling programs."
);

FAQStuff.Add(
"Why isn't the Optimizer keeping my meta gem active?",
@"You need to set Enforce Gem Requirements to be enabled. See Gemmings for more details."
);

FAQStuff.Add(
"Enforce Gem Requirements is active, so why isn't Rawr forcing the gems to match the socket colors?",
@"Enforce Gem Requirements only does two things: ensure that the meta gem is activated, and ensure that any Unique or Unique-Equipped gems are, in fact, unique. The gemmings recommended may or may not match the socket bonuses, irregardless of this option's state. Rawr does not have any options to set to force socket bonuses to be activated, and instead recommends the best gemming possible, whether it's with the socket bonus, or ignoring it. See Gemmings for more details."
);

FAQStuff.Add(
"Why does the Optimizer try and equip two of my weapon when I only have one?",
@"The item is not Unique, so the Optimizer assumes that you simply have access to the item, as it does not know how many of a given item you have. To restrict the Optimizer to consider only one copy of a weapon, right-click the item, select Edit, and mark the item as Unique. This will prevent the Optimizer from putting the item in both MH and OH slots, as if you had two copies of the item. The same process can be used for rings and trinkets."
);

FAQStuff.Add(
"Why does a Troll's Berserking and an Orc's Bloodfury not show up in the Buffs list?",
@"Racial buffs are automatically added to the character stats, based on your race selection in the Character Definition area of the Stats pane."
);

FAQStuff.Add(
"Why does the Optimizer sometimes lower my DPS?",
@"The Optimizer operates on a Random Seed method, which is why it works at all. Sometimes it can't use that random seed to find a set that is better than what you are currently wearing. This is a known issue that we wish to hopefully find a solution for in the future. In the meantime, you can help the Optimizer to find the optimal setup by following these steps:
- Limit the number of potential items that you mark as available to the Optimizer. As the number of pieces of gear / gems / enchants rise, the number of potential solutions (results) the Optimizer can create increases exponentially. Periodically clean up your list of items that you know will perform as well as other options.
- Increase the Optimizer thoroughness by moving the slider on the Optimizer window to the right. While the Optimizer will take longer to run, it will be checking through more and more potential setups the higher the thoroughness is set.
- Make absolutely sure that everything on your current character is marked as available to the Optimizer. This includes gear, enchants, and gems. This should be mostly covered by a check that is run when you start the Optimizer, though we are currently refining the warning dialogue to be more descriptive (to tell you exactly what you are currently wearing is not available to the Optimizer)."
);

FAQStuff.Add(
"Why does the Optimizer sometimes just rearrange my Gems?",
@"In the more modern versions of Rawr, this issue should no longer exist."
);

FAQStuff.Add(
"Why is my Crit value so low, compared to the in-game character panel?",
@"Boss level relative to your own affects your actual chance to deal a critical strike. Targets that are three levels higher than your own (83, or Boss level) exhibit a 4.8 crit depression / conversion, which affects both white and yellow damage. Rawr calculates and displays your actual chance to crit, while the in-game character panel reflects your stats against a same-level target."
);

FAQStuff.Add(
"Why does X talent/glyph not show any value in the comparison lists?",
@"Many talents cannot be valued by DPS gain or by Survivability Gain. It's also possible that you do not have the Situation setting where the Talent/Glyph would have value. For example, If you are never Stunned, then a Warrior's Iron Will wouldn't have a value."
);

FAQStuff.Add(
"Why is it when I run the Optimizer I don't end up hit and/or expertise capped?",
@"The optimizer, when run without any requirements, will attempt to find the highest possible total DPS. In many cases, this does not include being hit/expertise capped, either by a small amount, or sometimes even by a larger amount. This is usually a correct recommendation in terms of optimizing you DPS / tanking performance. However, sometimes one may need to ensure that an interrupt or a Taunt does not miss. To ensure that an avoidance cap is enforced, add the '% Chance to be Missed <= 0' requirement before optimizing. Similar parameters are available (model-dependent) for Dodged, Parried, and Avoided, the latter taking into account all types of target avoidance."
);

FAQStuff.Add(
"Why do higher tier items sometimes show less value than lower tier items?",
@"Set Bonuses can have a serious impact on DPS; getting that 2nd or 4th piece can mean more or less for your character in specific gear-sets. It could impact your meta gem requirements, depending on your setup."
);

FAQStuff.Add(
"Why aren't items with Resilience considered relevant?",
@"Rawr is for PvE, not PvP. Modeling for PvP settings is an exercise in futility, due to the wide range of fight settings, high mobility, the extreme variability of damage intake and damage output requirements, and the sheer number of potential targets that are presented in PvP situations. Some models allow for comparison of PvP-sourced items, despite the wasted item budget, and some filter out those items with Resilience. Check the Options panel for your particular model for more information."
);

FAQStuff.Add(
"Why are the stats wrong for my X level (sub-80) character when I load from Armory?",
@"Rawr is for end-game PvE, meaning you should be level 80. Rawr does not factor things for leveling as there is no point, you will replace the item in a couple of levels anyway and all your raiding gear will end up requiring level 80 to wear."
);

FAQStuff.Add(
"Why can't I select X weapon type?",
@"Some weapon types are pointless to factor in depending on your model. For example, Fury Warriors and Enhance Shaman will never use staves, and Arms Warriors will never use one-handed weapons. Rawr intentionally does not consider such futile, sub-optimal situations."
);

FAQStuff.Add(
"Why can't I select X armor type?",
@"Each model typically only shows those items that are specifically designed for use with a certain class, but limiting the armor shown to that of the highest armor class that is wearable. However, there are certainly a number of situations in which downarmoring provides similar, only slightly lower, or sometimes even better performance for a given character. Some examples might be a Hunter considering Leather armor, or a Resto Shaman considering Cloth. To enable such considerations, simply pull up Tools > Refine Types of Items Listed, and enable / disable what items you would like to consider as applicable."
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
#region Rawr 4.0.13 (Dec 28, 2010) [r56600]
VNStuff.Add(
"Rawr 4.0.13 (Dec 28, 2010) [r56600]",
@"Cataclysm Release Beta

Rawr.Addon:
- Fix case of class name
- Release as v0.10
- Class Name for Death Knights should be DeathKnight capital K
- Fix for Night Elf & Blood Elf names needing no spaces and correct capitalisation
- Typo on elseif
- Fix for races
- add isValidVersion for more reliable version parsing in AddonLoadDialog
- Needs thorough testing for various invalids
- Import - build needs maxvalue to check if parse worked
- Added TODO comment about build number
- Add support files for Curseforge Localisations
- TFS project hadn't added locale files to project files
- Change warning on addon load text to indicate invalid data as a possible cause of bad import
- Release as Version 0.20
- Added button to Character Panel
- Added frame to display imported Rawr Data
- Update TFS to use Frame files
- Dry coded import button functionality needs testing
- Added import buttons (don't do anything yet)
- Added import.lua code file
- Changed icon to have lowercase lettering
- Export was missing trailing commas
- Change first line of export to named import function
- Tweak output format for import to addon
- Import now accepts data from Rawr
- Updated minimum version numbers on addon/Rawr interfacing with each other
- Now actually displays icons of stuff imported
- Missing comma on export
- Added UI for Rawr Export to AddOn
- Release as Version 0.21
- Added display of items on import frame with working tooltips
- Highlight item borders to show slot item rarity
- Selecting Equip Item from the context menu on the Direct Upgrades charts will now try to equip the item to the related slots, not just the Head slot.
- Fix crash if missing a profession
- Release as Version 0.22
- Fix crash if missing a profession
- Fix issue with non English locales exporting race name
- Moved buttons on Paper doll frame a bit
- Small tweak to button sizes

Rawr.Base:
- Added Item Set Comparisons!!! You can now use the Tools Menu to Add/Equip/Remove Saved Sets for comparison.
- Addon Importing now filters out items from your bags/bank that aren't relevant to the model. So if you have tank gear in your bags and you are wearing your healing gear, the model loads into the healing model and doesn't mark the tank gear available (as well as junk gear link Green Winter Hat)
- You can now save an Optimized GearSet to the ItemSets list. This does not save changes in the optimizer other than the items themselves (meaning no buff or talent changes)
- Started work on adding parsings for Cata trinkets. Currently have most 'On Use' effects in place.
- Cleaned up the 'On Use' special effects section so that procs are grouped up and duplicate lookups are removed.
- Added a BonusWhiteDamage stat so that Unheeded Warning can be modeled. Melee devs need to add this stat as a relevant stat.
- Updated all items that have been changed by Blizzard through their hotfixes
- Updated items and filter with Rare Mob drops information (MMO Champ missed a few rare mobs in their article)
- Started work on adding more import information from Wowhead.
- I stand corrected MMO Champ did have them all and I was the one who didn't have them all. Filters are updated
- Fix for Issue 19369: Unit Race doesn't seem to be working for Worgen - Worgen and Goblin didn't have the correct Race ID's set yet and the Race Selector on the Main Page wasn't set up right. Fixed both
- Item Editor no longer persists between usings, stops all issues with specific fields persisting
- Removed the duplicate 'basics2' reference from all the xaml's in Rawr.UI 
- Undid previous removal of basics2 reference, Kavan says it behaves differently for WPF 
- Updated all caches to clear out errors and post new settings and stuff 
- Fix for Issue 18787: Tooltips Clipped on Right - Added handling to move the tooltip to the left if its going off the right edge 
- Hid the option to Refresh Item from Armory since we aren't using that process
- Updated Welcome Window with FAQ entries
- ItemCache failover should now check to see if the ItemCache loaded was empty (this occurs when a previously bad item cache from an older version was accidentally saved as empty).
- Replaced the ErrorBox handling (again, sorry) to prevent the mishaps of things going to the wrong place on the resulting dialog.
- Fixed Character class wasn't copying CogwheelId & HydraulicId from gemming templates therefore wasn't using them at all
- Add a spell crit depression array. The only values I'm 100% sure of are Level+0 and Level+3 - the other two are just guesstimates. Could use some testing.
- Add a Rawr.Addon Save dialog in preparation for being able to export optimised character to Rawr.addon
- Save to addon implemented now exports data of equipped items to Addon from Rawr
- Fixed some level 83 references that should be 88 now across several models
- Added a StatConversion.GetRatingFromSpellHit funtion to complement StatConversion.GetRatingFromHit which was Physical Hit only
- Fix for issue 19447: float.Parse() does not work as expected for some culture settings - Implemented InvariantInfo setting on the version number check and added ',' as a possible delimiter in the RegEx so it will capture properly. 
- Added an option for people with low res monitors to hide the shirt & tabard icons and move the gloves to the left side, basically saves you a row of paper doll selectors
- Changed the Stats Pane to have the character basic info stuf moved into an accordion thats default collapsed. The info in the boxes is now tied to a label at the top. This saved you some space on the Stats pane
- Removed the usage of LocationFactory as a separate cache, there is now no need for ItemSource.xml
- Resorted the Wowhead.cs file so its easier to find stuff (had gotten rather haphazard)
- Working on getting Update Item Cache from Wowhead working. Initial code placed but throws constant InvalidCrossThreadAccess errors
- Moved several import/export dialogs to a new Character folder (instead of cluttering up the Dialogs folder)
- Polished up the Item Editor some more with Group Boxes
- Added Binds On selector to the Item Editor
- Added the ability to Compare an Item Set to your currently equipped items using the Optimizer Results window. You can also equip the comparing set from that window.
- The tooltips on the Item Sets chart entries now display as an Item List (like the bottom of the Build Upgrade List's tooltips) instead of a giant wall of text.
- Fix for Issue 19411: Error pop up on sorting and changing charts - Needed the special sort check in the new chart switch sorting
- Task 19069 Work: Add support for Engineering tinkers 
- Tinkerings for Waist slot fixed
- Selecting other Tinkerings works now, NOTE: This commit should mean full support of Tinkerings now
- Added better warning info to the model status in the status bar
- Colorized the model status label
- Fix for Non-ItemTooltips having a nullcheck crash
- Disabled Update Item Cache from Wowhead function in release builds as it doesn't work yet

Rawr.Buffs:
- The Buffs By raid Members dialog has been updated to Cataclysm for the four classes that I had previously set up (Priest, Warrior, Mage and Hunter). Other classes coming soon
- Added Death Knights to that dialog 
- Fix for Issue 16401: Buff.AllowedClasses wasn't being consistently enforced - Changed the implementation in Base to enforce allowed classes and professions. Charts, the Buff Control and the affected models should now all work properly

Rawr.BossHandler:
- Raid Boss armour confirmed at 11977 not 10643
- Issue 19328 Work: GetDPSbyType always uses avoidance stats - Added some if statements into the calc that would be for formulating against other attack types. They are non-functional at this time, just a placeholde

Rawr.ItemFilters:
- Vendor Item Source now supports multiple tokens, the second one can be typed in.
- Slight adjustments to the Rare mobs listing
- Ensured the ItemFilters were fixed and in the DefaultDataFiles.zip
- Basic cleanup of test data and un-needed references.
- More cleanup and errors fixed. Updated DefaultDataFiles.zip 

Rawr.Items:
- Change Darkmoon Card: Volcano to use two different special effects for the two different parts of the proc.
- Fix for Chimera gems uniqueness
- Fix for issue 19344: Anhuur's Hymnal proc not counted - Added parsing for its proc
- Fixed BonusWhiteDamageMultiplier with a DefaultValueAtribute(0)
- Added ToItemString function to ItemInstance for Rawr.Addon
- Fix ToItemString - Rawr uses ReforgeIds-56 needed to add that back to itemstring export to get in game to work
- A bunch of the Source parsing from Wowhead is now fixed. Still more work to do
- Parsing source from Wowhead is about 95% functional now. Only a few items won't get source data (not counting the ones that simply don't have source data on wowhead yet).
- More Parsing Fixes:
- - Now sets costs for Justice and Valor points if the item has them
- - Now sets both the purchase currency+cost and where the currency drops (namely the armor token drops) if its available

Rawr.LoadCharacter:
- Importing your toon from the Rawr AddOn or the Armory will now mark the items (and their enchants if equipped) with Green Diamonds

Rawr.Bear:
- Fix for Issue 19426: Proc AGI is still providing armor - Took off the Agility to Armor bonus from procs 

Rawr.DPSWarr:
- Overhauled using warnings from Code Analysis
- Fix for Issue 19372: SMT with 1H Weapons doesn't work
- - This was Three separate things
- - You need to go to Filters by Item Type and turn on 1h Weapons then click Apply. DPSWarr by default doesn't show them but does leave you the option to turn them on
- - SMT wasn't handled correctly. I've fixed this
- - Having a non-1h in the OffHand is throwing a NaN on that chart, so it ends up not knowing what to do and crashes. Added a handler to check for NaN in that core file so no models can make that break anymore.
- Modelled BonusWhiteDamageMultiplier (some new trinkets use this)
- More overhauling
- Fixed a bug that would make all the Buff Selectors disappear
- More overhauling
- Added a notice that Fury doesnt work yet, invalidated its calcs
- Gemming Templates now generate for 4 metas instead of just Chaotic

Rawr.Enhance:
- Initial work at implementing T11
- Removed T7-T9
- Fix for issue 19428 - hit rating required was using physical not spell hit 

Rawr.Mage:
- Added average mode for mirror images, default to disabled
- Performance improvement for incremental optimization with advanced mana segment constraints. 

Rawr.Moonkin:
- Add Silences to the boss options handling code in Moonkin
- Removed a lot of else's that should enable handling of single SpecialEffects with multiple stats that fall into separate handler categories
- Enable trinket procs that apply DoT effects.
- Fix mana proc handling. In particular, Hymn of Hope / Mana Tide Totem should now give sustained damage benefit.
- Add cogwheel support to gemming templates
- Add sparkling cogwheel for spirit = hit so both hit & spirit cogwheels can be used together
- Implement spell crit depression
- Undo an old optimization change that turned out to break a lot of calculations for non-standard rotations. Woopsy
- Fix a mistake with calculating mastery procs. Theralion's Mirror should now show the proper bonus
- Fix a display issue where the hit cap is not displayed properly for non-raid boss targets
- Got the numbers backwards on the display function

Rawr.ProtPaladin:
- Fix for Issue 19434: Crash on Character Load - Consecrate Glyph wasn't playing well with number of ticks count. Fixed the array size and the starting point so it doesn't crash anymore

Rawr.ProtWarr:
- Fix for Issue 19365: Agility should no longer grant armor - Removed the calc that adds Armor from Agility
- Task 19364 Completed: Plate Specialization is missing - Added ValidatePlateSpec function from DPSWarr, applied as BonusStaminaMultiplier
- Fix for Issue 19366: [Enchant Shield - Blocking] missing - ProtWarr didn't have Block Rating relevant and wasn't pulling the stat. Fixed
- Fixed a cross thread access issue

Rawr.RestoSham:
- Fix for buffs, gemming templates, and changes to stats area to keep confusion down while Alpineman continues work on new model
- Disabled the beta model for now
- Beta model changes

Rawr.ShadowPriest:
- Fix for Issue 19425: Twisted Faith is giving hit on base spirit - Added a subtractor to make it ignore base spirit 

Rawr.Warlock:
- Fix for Issue 19391
");
#endregion
#region Rawr 4.0.12 (Dec 19, 2010) [r56280]
VNStuff.Add(
"Rawr 4.0.12 (Dec 19, 2010) [r56280]",
@"Cataclysm Release Beta

Rawr.Base:
- Monolithically huge performance fix!
- Catch for IsolatedStorageExceptions to alert the user they need to allow Rawr in Silverlight permissions instead of Deny it.
- Second Catch for failed Armory Imports to try and get better messaging in case it failed in a different spot
- Attempted to make a call that would download new caches automatically if they failed to load. Astrylian will investigate this further.
- Task 19071 Completed: Support Item Source from Achievements - You can now manually set item source to Achievements 
- Task 18891 Completed: Cat T10 4P Change - Updated the Set Bonus
- Fix for ItemCache failover reloading

Rawr.Moonkin:
- Fix broken null reference exception in the reforging code. 
");
#endregion
#region Rawr 4.0.11 (Dec 18, 2010) [r56269]
VNStuff.Add(
"Rawr 4.0.11 (Dec 18, 2010) [r56269]",
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
- Redid the Welcome Window some to give the Tabs more room (so you can read them more easily)
- Populated the Version History with Rawr4 stuff instead of Rawr2 from Nov 2009
- Fixed the Tips display
- Populated the Known Issues list with Open Issues that haven't already been resolved for .11
- Fixed the annoying scrollbar for no reason issue on the main page
- Fixed Windwalk's Enchant Id
- GetCritReductionFromResilience now always returns 0. Resilience has not reduced crit strike chance since 4.0.1
- Moved crit chance reduction via resilence removal to the right spot. I didn't realize at first that GetCritReductionFromResilience still calculates crit damage reduction.
- More ItemSource fixes
- Performance fixes

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
- Fix for issue where reforging was showing nonsensical combinations
- Add Intellect and Mastery trinket procs

Rawr.ProtPaladin:
- Fix for Issue 19281: Items not loading - No Gemming Templates were enabled by default (due to there being no Epic Templates this early in Cataclysm). Set Rare templates to enable instead of Epic
- Fixed crit vulnerability calculations
- Fixed display issue when crit immune
- Updated consecration probability calculations
- Added Mastery Rating to the item budget table
- Added custom rotation, fixed some miscellaneous bugs, started work on fixing the scales
- Bar colors now match other tank models 

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
#region Rawr 4.0.10 (Dec 06, 2010) [r56001]
VNStuff.Add(
"Rawr 4.0.10 (Dec 06, 2010) [r56001]",
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
#region Rawr 4.0.09 (Dec 06, 2010) [r55987]
VNStuff.Add(
"Rawr 4.0.09 (Dec 06, 2010) [r55987]",
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

            CB_Issues.Items.Add((String)"All");
            String[] arr = new String[KIStuff.Keys.Count];
            KIStuff.Keys.CopyTo(arr, 0);
            foreach (String a in arr) { CB_Issues.Items.Add(a); }
            CB_Issues.SelectedIndex = 0;
            CB_Issues_SelectedIndexChanged(null, null);
        }

        #region Info Operations
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
        #endregion

        #region Character Operations
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
        #endregion

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
