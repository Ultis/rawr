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
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
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
#region Rawr 4.1.00 (Mar 10, 2011) [r58642]
VNStuff.Add("Rawr 4.1.00 (Mar 10, 2011) [r58642]",
@"Cataclysm Release Beta

Rawr.Addon:
- Partial support for export to Rawr Addon from upgrade list

Rawr.Base:
- Turning off ElevatedPermissions requirement for OOB mode. This may fix automated updating for OOB installs
- Silverlight and WPF versions both have new update protocols to check in addition to what they already have. This should better alert users when new versions come out
- Fix for crash when loading a character that has items equipped which are not in the database
- Refactored BossHandler functions, ordering and adding of Default Values to reduce XML file clutter
- When generating a new BossOptions because it was null, will automatically add a Default Melee Attack (helps Tanking models with their default values, especially Bear)
- Refactored Character class, ordering and adding of Default Values to reduce XML File Clutter and added comments
- This commit has an overall reduction of Character files with minimal settings changes by approximately 25 lines
- Migrated Rawr.Base to Root
- Rawr2 unused file removals from Root
- WPF: Better version check handling
- Fix for random suffix items crash
- Fix for random suffix items crash in batch tools
- Fix for Item Cloning
- Organized the files into folders
- Migrated Rawr.UI to Root, since there was no Rawr2 version of this directory or its children, there was nothing to delete prior

Rawr.BossHandler:
- Added Mob Types variable to the Boss Handler (Defaults to Humanoid)
- Added Default setups for MyModelSupports this against MobType
- The three Special Bosses now determine what mob type they are by counting. If the highest count is same number as Humanoid, will revert to Humanoid

Rawr.Buffs:
- Fix for Issue 19984: Flask Mixology and Double Pot Trick mutually exclusive - Added Conflicting Buffs statements for Double Pot Tricks, I swear if I have to change this stuff again.

Rawr.Enchants:
- Fix for Issue 20067: Avalanche & Hurricane - Hurricane already done, Updated Avalanche to DamageDone trigger from MeleeHit

Rawr.Items:
- Updated Mercury-Coated Hood
- Added Hornet-Sting Band
- Added Throne of the Four Winds items in preparation for new suffix update
- Added Tier 12 ilvl uniqueness
- Added early support for new Tier 12 trinkets
- Added support for Nature and Holy Mana Cost reduction
- Added support for Highest Secondary Stat
- TODO: several new trinkets do not have any ICD or duration information at this time
- Task 19987 Completed: Alchemist Stones need to be mutually exclusive - Added exclusivity between 50400, 50386, 50399, 50378
- Fix for Issue 19810: Darkmoon Card: Hurricane proc is Incorrect - Changed the value to a 5% proc for now
- Fix for Issue 19390: Engineering cogwheels not working properly - Added another Cog selector to gemming templates. When the Templates in each model get set up correctly (or custom added by user) they will show up correctly
- Fix for Issue 19975: PVP Set parsing incorrectly - Added a Replace to kill any instance of the word Vicious from set names
- Fix for Issue 19530: Quest rewards not flagged as unique - By popular demand (aka bitching) Quest Reward items will be marked unique upon refresh
- Updated all raid item sources in BoT, BD, and Tot4W
- Updated all Tier token item locations
- Fixed Bell of Enraged Resonance proc
- Fixed all Cata Alchemy Stones
- Updated all 325 - 333 quest items (so that they are showing as Unique Equip)
- Updated Suffix information for all Random Enchant items found in Tot4W
- Updated Cata Jewelcrafting gems and armor with Source information and Binding Type
- Added Suffix information for JC rings and Necks
- Updated Source information and Binding Type for Cogwheels
- Updated source and binding type for all Blacksmithing crafted items

Rawr.ItemFilters:
- Fix for Issue 20034: Filter by Profession hides non-profession items - Shirts/Tabards are no longer profession filtered by that check. The lances don't matter and no real item would have this problem.

Rawr.Optimizer:
- Fix for crash with random suffix items
- Results should display score depending on what you optimize for
Rawr.Optimizer.UL:
- Fix for Issue 20110: Upgrade list not thorough enough, missing item(s) - Implemented 10x Ctrl Click for Upgrade List too

Rawr.Server:
- Fix for glyph parsing

Rawr.Tinkerings:
- Fix for Issue 20100: Incorrect duration for Synapse Springs - 4.0.6 Update for Synapse Springs

Rawr.Bear:
- Fix for Issue 20043: Armor Multiplier, Survival Soft Caps - Fixed Armor Multiplier
- Upped Soft Caps by 25% of current values
- Updated Gemming Templates to include proper Cogwheel templates

Rawr.Cat:
- Updated Gemming Templates to include proper Cogwheel templates

Rawr.DK:
- Fix for 20091: DamageModifier problem causing ability value inflation causing HUGE values in most abilities
- Removing old files for Rawr4 migration/reintegration back to trunk
- Update to new Cogs format

Rawr.DPSDK:
- XML optimization & adding in some defaults
- Migrated files to Root
- Fix a null reference problem on clean characters
- Fix for Issue 20007: Rune of Fallen Crusader wasn't working or being excluded properly
- Implement CinderGlacier
- Fix RazorIce
- Implement Rime (it's super hacky right now)

Rawr.TankDK:
- Adding an XMLIgnore in there to help shrink the XMLs a bit
- Fix for 19864: Default over-healing should not be 0%
- Migrated files to Root
- Fix issue 19851: Lack of Haste value in RSV because HasteRating wasn't being properly handled

Rawr.DPSWarr:
- Migrated DPSWarr files to Root
- Added several DefaultValue flags to Calcopts, reduces saved file size when defaults aren't changed
- Updated Gemming Templates to include proper Cogwheel templates

Rawr.Enhance:
- Fix for display issue on options panel
- Reimplemented Stats Graph
- Fix for Issue 20070: 1% spell hit difference between WoW and Rawr - Display issue only, added Draenei hit check to the tooltip and value
- Migrated to Root
- Further work on Export to EnhSim
- Remove an unused file

Rawr.Healadin:
- Task 19775 Completed: Option to not display spirit items should be removed - Removed
- Working Issue 20187: Spirit not a relevant stat - Added Spirit as a relevant stat

Rawr.HealPriest:
- Update the available glyphs's for priests

Rawr.Hunter:
- Updated basic Pet stats (Health, AP, Armor, Crit should be correct)
- Sgen for Hunter project
- Removed the PetBuffSeletor UI and back end. This was removed for Cata
- More updates to Pet stats
- More work on calculations
- Updated Gemming template with Cogwheel info

Rawr.Mage:
- Changing arcane light setting to default enabled
- Fix for by spell breakdown
- Improved quadratic solver support for int and mastery procs
- No dot ignites in ptr mode (you should manually lower ignite munching factor)
- Implemented PTR changes, arcane aoe cycle solver
- Fixing improved ae, changing gcd latency to 0.01 default

Rawr.Moonkin:
- Fix treant hit calculations, now that I've resolved the issue with my in-game testing
- Remove the Treant Hit display, as it is mostly irrelevant now
- Delete Rawr2/Rawr3 code in preparation for migration of Rawr4 code to root folder
- Migrate code to base
- Fix cogwheel gemming templates
- Don't need to add the cogwheel templates to every tier of the gemming templates
- Fix for Issue 20175: Fixed the treant hit mistake

Rawr.ProtPaladin:
- Mongoose should not be adding armor
- Fix for Issue 20039: Agility provides Armor - Agility was still increasing armor, removed. Also removed from HealPriest and ShadowPriest
- Fixed bug with Seal of Truth being way too high on threat

Rawr.Retribution:
- First Changeset, mostly adaption of new skills
- Gems copied from DPSWar, DPS Breakdown changes
- Old Code removed and attack table fix
- Removal of old code and some bugfixes
- Rearranged Additive / Multiplicative
- Added T11 support
- Damage values mostly done, some old code removed
- HighestStat support, Crusader Glyph and a bugfix of additive and multiplicative stats rearrangement from yesterday
- Cleared up the simulation for better performance, some damage fixes
- First raw draft of rotation
- Inqusition handling, reorganizing some code
- Removal of old code
- Migrated to Root
- Further removal of old code and some improvements
- Unused option removed
- Fixed Ret's options pane constructions back to the standard for all models, the DataContext stuff wasn't being assigned correctly
- More options removed
- Removed the Bloodlust checkbox from the Options Pane, tied the Ret back end to trigger this from a user actually selecting the buff (or it's equivalent)
- Bosshandler useage
- Updated Gemming Templates to include Cogs
- Forgot to add the spell crit buff as default
- Crit fix

Rawr.Rogue:
- To speed up optimizing all mobs are assumed to be poisonable. Assorted fixes for Assassination. First work on Combat
- Ported a fix Poison fix made in Ass to Combat and Subt. Forgot to remove a line added only for testing
- More work on Combat. Switched glyphs in Prime and Major glyph categories. Added glyphs of Blind and Vanish. Added saved talent specs for Assassination and Combat (glyphs don't load yet).
- Fixed glyph numbering. Glyphs now load when picking a saved talent spec
- Small updates for Ass / Combat and first pass at Subt. HoT is overvalued (assumed to proc on CD) and Ambush / Shadow Dance aren't (fully) in yet

Rawr.ShadowPriest:
- Update the available glyphs's for priests
- Verification of the Shadow Priest Model Stats Calculations
- Ensure basic stat calculations are correct for all priest races /w standard talent set/untalented and three item sets
- Cleanup
- Update gemming templates
- Remove unused files
- Migration to Root

Rawr.Tree:
- Modified spell durations, coefficients and mana costs to 4.0 values from Restocalcs.xls
- Some talents updated
- Something still looked weird on spell crit values
- Mana regen not yet touched
- Started work on a hopefully more elegant way to reduce copy+paste code in Solver.cs
- Most talents now implemented (Tree of Life doesn't model different spell behaviour)
- Fixed issue with crit percentages
- Mastery now implemented at the spell level and used in New custom graphs
- New custom graphs include effects of Revitalize (not replenishment) and lifebloom refresh from Emp Touch
- Top level simulation and mana regen still need work
- Fix for Issue 20104: Spellpower from Spirit - The model was no longer using it but the line was still there. Commented out the line to prevent confusion
- Commented it out from HealPriest as well

Rawr.Warlock:
- Fix for issue 20112: Mark the Rare gem templates as enabled by default
- Fix Cogwheel templates
");
#endregion
#region Rawr 4.0.20 (Feb 20, 2011) [r58200]
VNStuff.Add("Rawr 4.0.20 (Feb 20, 2011) [r58200]",
@"Cataclysm Release Beta

Rawr.Addon:
- Professions export with None instead of empty XML
- Mage talent bug not fixed in client 4.0.6 - export checks for 4.0.6 now

Rawr.Base:
- Replaced the caches with all same encodings
- Fix for LocationInfos being null
- Fix for Pawn string to export Mastery Rating
- Fix for Filter side-bar starting on Drop Rates instead of Sources
- Support for random suffix items. Still needs to be done: wowhead parsing, armory import, rawr addon import/export. Needs a lot of testing
- Fix for random suffix display in optimizer results
- Fixing some bugs introduced with random suffixes
- Fix for Issue 19978: Scaling on random charts too large - In some cases, the rounding method can cause the step scaling to go nuts. Added a loop to reduce the step and bring it back in line in the event that it does this
- Updated Base Stats per Chardev for all races/classes. There may be still some info that is a little off but we're far closer than we were
- Feature 18505 Implemented: Refresh Only Items Currently Worn - Added a function to do this to the Import menu
- Feature 15546 Implemented: Bars showing relative to equipped item - Added setting to the Options dialog to enable this - NOTE: It is NOT recommended to turn this on as it makes the charts harder to read
- Feature 15606 Implemented: Equip Option within Item Edit - Added Equip button to Item Editor
- Feature 9028 Implemented: Character Comparison - Added the ability to compare the currently loaded character to another xml file using the optimizer results window. Select this from the Tools menu (under item sets)
- Fix for Issue 19747: Tweaks for WPF popup placement
- UI Work for Talent Spec importing, but it's not done yet as it needs a back end
- Updated Wowhead url loading of PTR items (Rawr was still using the Beta's cata.wowhead, instead of ptr.wowhead)
- Handling of random suffix items not present in item cache, performance improvements

Rawr.BossHandler:
- Task 18222 Completed: Create a method for Editing Attacks, Impedances, etc - Selecting things in their respective lists will populate the UI so they can be changed. Use the Add/Edit button to push any changes to the list

Rawr.Buffs:
- Added Dark Intent

Rawr.Items:
- Fixed Incorrect stats for Feral Druid Tier shoulders
- Fix for parsing PvP token costs on item sources
- Fix for Zone names going bad on source parsing
- Updated Item Cache with a lot of fixed sources. Especially PvP, Justice Points and Relic Items
- Updated Alchemist Stones data
- Fix for Gladiator's Regalia 4 Piece Bonus

Rawr.ItemFilters:
- Implemented Filter by Bind Type UI
- Removed 'Disable by Bind Type' filters from the Tree
- Implemented Filter by Professions UI
- Removed 'Disable BoP Crafted' filters from the Tree
- Implemented Filter by Drop Rate UI
- Removed 'Disable by Drop Rate' filters from the Tree
- Fixes for Issue 20000: Filtering Less than Optimal - Multiple Source Data with valids + not founds [FIXED]. The Item Cache will automatically purge itself of these scenarios on load
- BoP Crafted filters with new UI method not working [FIXED]. It was checking for BoA instead of BoP... stupid bug
- Cleaned up filters of old currenies no long in game and made them less messy to work with current parsing info
- Fix for Gem filter not working as intended
- Combined 'Wildhammer Clan/Dragonmaw Clan' as they are faction specific vendors

Rawr.Optimizer:
- Added notice next to the Optimize button about Ctrl+Click for 10x thoroughness
- Better optimizer support for random suffixes, xaml sync

Rawr.UpgradeLists:
- Feature 17387 Implemented: Remove Item from Upgrade List Option - Added ability to do this from the context menu. Removing an item cannot be undone
- Feature 15396 Implemented: Build Upgrade List for a Slot - Added 'Evaluate Upgrades for Slot...' to the item context menu, verified it worked on processing only items in Head slot (checked other slots too)
- Feature 19524 Implemented: Export Upgrade List to CSV format - Added Export options to UL, CSV in clipboard and saving CSV to file (saving to file requires install offline mode)

Rawr.Bear:
- Fix for Issue 19976: Thick Hide in 4.0.6 changed - Updated Bonus Armor Multiplier values for Thick Hide to 4.0.6
- Implemented DamageAbsorbed stat, which only shows how crappy the stat is for Bear

Rawr.DPSDK:
- Fix for Issue 20005: Implement filters like DPSWarr for gear
- Associate for 19726: Some additional fleshing out of ghoul data
- Adjustments to the Unholy rotation. It's really screwy, but far better than it was

Rawr.DPSWarr:
- Updated Bloodthirst stats with 4.0.6 values
- Single-Minded Fury bonus has been increased to 20%, up from 15%
- Bloodsurge procs add 20% damage to instant slams
- Slam hits with both weapons for characters with SMF talent
- Don't use WW in rotation by default
- Raging blow deals 120% weapon damage up from 110%
- Battle Trance talent now affects fury calculations
- HS and CL don't use GCD as it was in old model
- Fixed execute phase calculations
- SMF-specced warrior calculations use dual-wield miss chances instead of one-handed miss chances
- Updated Raging Blow values for 4.0.6a patch
- Fixed base mastery value
- Fixed stances damage modifiers
- Fixed infinite rage and 'hit doesn't matter' problem - 'Rage Details' still shows that many rage is unused, but rotation uses other 'source' until common solution for arms and fury is implemented
- Code cleanup
- Added 18 Feb hotfix for arms(Two-Handed Weapon Specialization (Arms passive) now gives 20% bonus damage with two-handed weapons, up from 10%)

Rawr.Enhance:
- Gemming templates updated
- Fix for mana regen
- Initial Major Display revamp (many things don't have any values beside them, but the values that do exist are the ones that were there before)
- Clean up of display in-case next release is pushed before my next check-in. 
- Removing reference to a temp working file (woops) 

Rawr.Hunter:
- Updated Talents and Glyph information for hunters
- Numbers pass on all shots. Still need more work with integration using Focus 
- Tons of Refractoring
- Added gemming templates
- Mastery and Specialization have rudimentory settings set up
- Cleaned up several Basic stats (RAP and Health should be correct or within a few points of live)
- Continued work on shot information
- Started work on pet basic stats

Rawr.Mage:
- Applying arcane hotfix changes
- Shard of Woe tweak
- Support for Dark Intent

Rawr.Moonkin:
- Further refinement to the model of Glyph of SS/Glyph of SFall with Starfall Lunar Only rotations
- Change Wild Mushroom cast time to 3 * 1 second global cooldown
- Add information to display about Starfall and Wild Mushroom usage in the rotations
- Update the coefficient on Wild Mushroom
- I had the wrong damage multiplier for Moonfury in the first place. Changed to the hotfixed 4.0.6a value
- Fix another mistake where I was double-counting the baseline 8 points of Mastery
- Update the calculations for Moonfire and Nature's Grace to better match the spreadsheet
- I also believe I improved upon that last by providing an actual calculation for GoSF and NG, rather than just plumbing in a guess of 50%
- Performance minded change, should improve performance by ~25%. Unsure as to the full ramifications of the change
- Implemented Mastery flooring as per Elitist Jerks. Note that this totally messes up the Relative Stat Values chart
- Add a button to display the stats graph. This will show the haste breakpoints and the mastery stepping function

Rawr.ProtPaladin:
- Now modeling Holy Shield. Thought that was already in there...

Rawr.RestoSham:
- Removed level 80 option, fixed HW and GHW calcs to better reflect real world

Rawr.Rogue:
- Implemented Honor Among Thieves. First steps in splitting the solvers, the output is complete bogus now
- More work on splitting the solvers, still useless output
- Fix for Defect 19962: Errors when compiling the SL version
- Changes to Assassination solver, Expertise and Hit calculations, gemming templates and display tooltips
- Implemented Energy regen from Haste
- Assassination: Made some change (forgot what) but the stat values seem to be right now, the overall DPS is just low. Once that's fixed I'll go on to Combat
- Fixed damage reduction from boss armor
- Potent Poisons and Vile Poisons stack additively. Rupture dmg is shown on the overview. SnD increases melee speed
- To speed up the optimizer SnD use is only considered with 4 or 5 CP. Split off Venemous Wounds damage from Rupture damage
- Fix for Poisons double dipping Mastery

Rawr.Tree:
- Fix for Issue 19674: Intellect not increasing Spellpower - Applied it at 1:1

Rawr.Warlock:
- Updated gemming templates to use Burning Shadowspirit Diamond; removed Wrath templates
- Updated stat relevancy to include Power Torrent, Hurricane, and offhand intellect
- Updated mastery bonuses for Demo and Destr for patch 4.0.6
- Updated effect of Inferno talent
");
#endregion
#region Rawr 4.0.19 (Feb 06, 2011) [r57865]
VNStuff.Add("Rawr 4.0.19 (Feb 06, 2011) [r57865]",
@"Cataclysm Release Beta

Rawr.Base:
- Fix for Charts not starting on 0.0
- Fix for Boss Handler's Average Boss averaging very slow melee attacks with normal melee (skewing the results)
- Fixed base physical crit and spell crit for paladins 

Rawr.MultipleModels:
- Support for WoW 4.0.6

Rawr.WPF:
- Removed System.Web dependency

Rawr.Buffs:
- Added 10% and 15% options for Luck of the Draw buff

Rawr.Items:
- Added Love is in the Air 346 ilvl neck drops 
- Withered Dream Belt having haste instead of mastery

Rawr.LoadCharacter:
- Fixes for loading characters with Russian text
- Removed TW and CN regions from loading from Battle.Net as they don't work anyways and won't work for a while. Added a notice to the dialog to this effect
- UI for Force Refresh option on loading character from Battle.Net
- Added UI for saving character files to repository and recalling them

Rawr.Cat:
- Switched to using BossHandler
- Fix for Leather Specialization

Rawr.Bear:
- Switched to using BossHandler

Rawr.Enhance:
- The BonusWhiteDamageMultiplier stat application wasn't set correctly

Rawr.Mage:
- Changing Improved Mana Gem to 15 sec, fix for 4T11
- Added AB4ABar1234AM and AB3ABar123AM cycles, added a note for mana neutral mix showing a mix of what it is
- Cooldown restrictions editor
- Fix for sequence reconstruction chart in WPF
- Numerical stability improvements for advanced solver

Rawr.ProtWarr:
- Support for all patch changes to abilities and talents
- Fix for Improved Revenge not granting enough bonus damage
- Support for Shield Block uptime calculations (now displayed in Block/Total Avoidance tooltips)
- Support for Heavy Repercussions (if Shield Block is enabled)
- Lowered default threat weight to 10% and increased default avoidance weight value
- Proper fix for Hold the Line threat values
- Support for additional Mastery and Parry gemming templates 

Rawr.ProtPaladin:
- Touched by the Light and Plate specialization now modeled
- Fixed relevant glyphs
- Fixed mastery calculation, block should be reading right now
- Touched by the light fully modeled
- Fixed spell power calc 
- Added 0.25 stam to 1 parry conversion
- Fix to toughness and bonus armor mult calculations 
- Fixed attack power calculation 
- Fixed a couple issues with Hammer of the Righteous 

Rawr.Moonkin:
- Update math to latest WrathCalcs spreadsheet:
- Move Starfall, Treants, Wild Mushroom calculations into the Rotation calculations.
- Add rotations with Starfall cast mode and Wild Mushroom cast mode.
- Remove rotations with Starsurge cast mode, is always on cooldown now.
- Remove Once and Unused rotations from dot cast modes.
- Corrected relevant NG and Eclipse calculations 
- Supposed to fix the issue with Sorrowsong where the value isn't right sub 35%. 
- Fix to Wild Mushroom calculations
- Make the glyph of Starsurge do something when the selected or burst rotations are set to Starfall Lunar Only
- Better modeling of the effects of the Starfall/Starsurge glyphs on Starfall Lunar Only rotations.
- Fix Starsurge glyph on rotations that are not Starfall Lunar Only.
- Fix Glyph of Focus. 

Rawr.Tree:
- Updates to include mastery and spell power. Cleaning up some bad logic
");
#endregion
#region Rawr 4.0.18 (Feb 03, 2011) [r57766]
VNStuff.Add(
"Rawr 4.0.18 (Feb 03, 2011) [r57766]",
@"Cataclysm Release Beta

Rawr.Addon:
- Fix for issue 19861 - exporting Rawr addon data wasn't working if item had a location with quotes in it
- Update version number in export to latest commit

Rawr.Base:
- Fix for Issue 19849: Exception on using the Add Item button twice - The dialog being reused in Silverlight worked fine but for WPF it causes problems
- Removed the Reusage method and now it generates a new one instead
- Fix for resetting interpolation cache when editing special effects
- Support for Triggers that only proc at sub 35% target health. Models need to add the uptime modifiers themselves

Rawr.BossHandler:
- Updated heroic 25 Nefarian Health based on Paragon's video
- More updated Boss health pools
- Small update to Boss Handler

Rawr.Buffs:
- Fixed for overvaluing Resistance bonus from Mark and Kings
- Fixed working item 19865: Stealskin Mixology bonus being undervalued

Rawr.Items:
- Updated trinkets that use the 35% triggers
- Corrected Tanking trinkets incorrectly using the target's execute trigger

Rawr.LoadCharacter:
- Fix for Issue 19848: Error Loading 'character.xml' Files - Character files without the Name property set come in as null charcater name, so when it's loading in and hitting the new escaped characters check, it's throwing a null exception - Added a fix to ignore empty character Names and Realms
- Fix for Issue 19859: Can't load character from EU-Голдринн Added Голдринн to EU Server List
- Fix for Issue 19746: Striping out hyphens from the server names that have it when making a character request
- Fix for Issue 19739: Load from Battle.net/Rawr Addon doesn't mark Ring Enchants as available - The same problem that was happening before with having 2 of the same item was happening to ring enchants. Broke it out and overrode the check to allow it to mark them. Verified with the example character

Rawr.Optimizer:
- Fix for Issue 19831: Listing Items as Changed When They are Not - Changed the visibility options for the parts of the display, now everything is shown side by side so it's obvious what has changed and what hasn't

Rawr.MultipleModels:
- Task 19371 Completed: Melee Modules need support for BonusWhiteDamageMultiplier - Added Relevancy, Get and usage of BonusWhiteDamageMultiplier to Cat, Bear, ProtWarr, Retribution, ProtPaladin and Enhance

Rawr.Cat:
- Fix for Issue 19832: Unheeded Warning proc not working - Added WeaponDamage stat usage in CatAbilityBuilder constructor. Relevancy and Get were already in

Rawr.DK:
- Adjust some additional talent tweaks based on in-game values
- Implement Rage of Rivendare
- Fix for Defect 19598: validate White swing code

Rawr.DPSDK:
- Basic Ghoul code started. Not plugged in yet

Rawr.Enhance:
- Fix for Issue 19581
- Initial work at display revamp
- Fix for Issue 19890: Ignoring custom Boss Level for hit/expertise caps - Enhance was set up to replicate a lot of variables from BossOpts over to duplicate variables in CalcOpts. Removed (almost) all of that and replaced it with actual BossOpts calls. Fire/Nature Resist and Target Groups are still using the bad method and should be fixed

Rawr.Mage:
- Fix for hasted evocation
- Fix for Issue 19858: Arg_NullReferenceException when trying to run optimizer
- Support for execute phase special effect triggers
- Fix for numerical instability in arcane solver

Rawr.Moonkin:
- Should fix the issue where it was crashing during optimization. I don't know what possessed me to write it the way I did
- Add a user option to enable or disable reforging Spirit to Hit rating, now that Hit rating has a slight tangible advantage over Spirit
- Fix for issue where pots were showing 0 DPS value

Rawr.Rogue:
- Enable default gemming templates. Fix some casts
- Refactoring. Implemented Ambidexteriry, Executioner, Improved Ambush, Ambush damage, Initiative, Sanguinary Veins, Preparation and Serrated Blades. Prevented bleed debuff double dipping. Removed Evis resetting SnD when spec'd in Master Poisoner

Rawr.TankDK:
- Fix for Defects 19769 & 19812 - though there is still some stats normalization that I need to do. Dodge & Parry are now correct and validated
- Fix for Defect 19863: DRW was screwing up my RP math. Not perfect, but it's alot better. Refactored Blood rotation
");
#endregion
#region Rawr 4.0.17 (Jan 28, 2011) [r57608]
VNStuff.Add(
"Rawr 4.0.17 (Jan 28, 2011) [r57608]",
@"Cataclysm Release Beta

Rawr.Addon:
- Display character frame & paperdoll frame on typing /rawr import
- Add check to comparison values to ensure not nil
- Tested fixes for import
- Changed to have 3 ranks of sounds
- Fixed issue with checking item ids with nil itemstrings
- Looting mobs tested - bug fixes
- Added warning frame and warning frame options
- Export to Rawr Addon now includes location string so tooltip can show where to get the item
- Added item location info from Rawr requires Rawr 4.0.17
- Allow Changes button to show if data previously imported
- Fix for Issue 19819: Import does not mark all items available - The toggler that sets it as available turns it on, then the second instance of the item turns it back off. Wrote a check to see if the item id has already been processed and ignored it if it was

Rawr.Base:
- Added a generic Armor Specialization check to the Character Class
- Base Stats for draenei shaman updated
- 30% boost to melee haste for shamans removed
- A proper fix for calculations trying to get called when character is not ready for it
- Don't show Jewelcrafters gems on gemming lists if character isn't a Jewelcrafter
- Jewelcrafter gems only excluded if Enforce Professions selected
- Loading addon data now loads model defaults
- Changed default reforging options to include reforging from nonrelevant stats
- Fix for defect 19823: Hunter model status was not on home splash page
- Fix for Specials Character in names issues from Patch 8190 submitted by yury2808
- Fix for Issue 19729: Enchant icons not showing - The cached copies of all the enchants didn't have the Icon Sources set. Forced it to rebuild the cache with new data. - Updated the XML and DefaultDataFiles.zip with a new default cache

Rawr.BossHandler:
- Updated Lady Sinestra's Boss Handler Information. Most spells and frequency are in place. Damages are estimates based on Paragon's video. All info is based on that video. Phase two is estimated at lasting 1 minute
- Updated Maloriak's enrage timers

Rawr.Buffs:
- Fix for Issue 19685: Enabling a Scroll does not disable Mixology bonuses - The check that would turn it off was being skipped due to the professions enforcement. Fixed to support both
- Fix for Issue 19731: Can only select one Mixology when selecting Elixirs - Verified after fix that you can select a Battle Elixir with it's Mixology and a Guardian Elixir with it's Mixology all at the same time

Rawr.ItemFilters:
- Disable By Item iLevel filters removed from Filter Tree
- Activated the Filters by Item Level Accordion at the bottom
- Filled out the Accordion with two option sets: Checkboxes which when checked will show that ilevel group and a Custom Slider where you can manually set a range you want to see
- Added Reset and Uncheck All buttons to the Accordion
- These filter values save to the Character object so they will be consistent with each save file managed
- The Sidebar will now remember how wide it was from last program launch
- Updated the separation of WotLK top end Cata low end to 284 from 277

Rawr.Items:
- Add Lady Sinestra's Cache drop information to filter system
- Updated Lady Sinestra's loot with Cache information
- More Source updates
- Updated Earthen Handguard normal & Heroic stats
- Updated 11.5 Token drop locations
- Added support for 4.0.6 change to Unheeded Warning

Rawr.Optimizer:
- Fix for issue 19834: Cogwheels/Hydraulics not being used by Optimizer

Rawr.DK:
- Talents.XML: Add some default specs. Some are missing glyphs, but should provide some additional guidance

Rawr.DPSDK:
- Pulling out unused Files
- project update from earlier check in
- Fix for 19773: Nerves of Cold Steel was not implemented despite what my comments had suggested
- Null checking in the fix for NoCS

Rawr.DPSWarr:
- Updated Meta List to add the new Strength Meta to the templates. Will remove other now useless templates once 4.0.6 hits

Rawr.Enhance:
- Initial work at cleaning up the code
- Export to Enhsim is now working for WPF version. Numbers may be slightly off
- Removal of the 30% boost to hybrid haste
- Added glove tinkers to EnhSim Export
- Further work on getting Enhsim export accurate
- New Export to EnhSim for Silverlight is here!!

Rawr.Mage:
- Updated mana cost based on hot fix notes
- Fix for mirror images in averaged mode
- Fix for Combustion cooldown segmentation
- Changing Mirror Image default to averaged
- Setting for ignite munching, PTR mode (hopefully I got all the changes, could use some review)

Rawr.Moonkin:
- PTR changes:
* Remove armor modifier from Moonkin form
* Add 30% damage buff to Wild Mushroom calculations
* Also changed the Wild Mushroom calculations to 650-786 base damage per mushroom, rather than split across all 3 mushrooms. I have no idea if this is correct
- Switch the Wild Mushroom calculations to match the spreadsheet:
* Reduce coefficient to the spreadsheet value. (Needs confirmation)
* Re-reduce damage to 650-786 across all 3 mushroom
- DEFAULT_GEMMING_TIER changed to 1 - Array was zero based not 1 based so default of 2 was epic instead of rare. Epic gems of course don't exist yet in Cataclysm.
- Updates to treants:
* Treants do not benefit from raid-wide auras such as 10% AP, 5% crit, 10% haste
* Treants benefit from Heroism and 4% physical damage debuff
* Add a display value to show the amount of hit treants have, as well as hit rating to cap
- Correct display typo: StatConversion.YELLOW_MISS_CHANCE_CAP[PlayerLevel - TargetLevel] which always gives a negative and out of bounds exception should have been StatConversion.YELLOW_MISS_CHANCE_CAP[TargetLevel - PlayerLevel]

Rawr.RestoSham:
- Applied patch 8175 to fix non-beta model's GHW calcs

Rawr.Retribution:
- Split out active and passive abilities into separate classes
- removed conflicting abilities class 
- Fixed stats for Tauren, blood elf and human paladin
- Still need stam and spirit for dwarf and draenei
- Updated more Paladin Base Stats 

Rawr.Rogue:
- Rupt dmg updated. Energy costs updated. The optimizer will no longer use finishers with less than 3 CPs. Rupture and Recuperate are no longer assumed to be used with 5CP if used at all. Agi now gives 2 AP per point. Updated base AP and base Belf stats. Implemented Leather Specialization. OH dmg was penalized twice. Lots of talent/glyph updates
- More refactoring. Updated Assassination talents. Fixed base stat calculations
- Added Faerie Fire to the armor debuffs to consider when deciding about Expose Armor
- First pass at better DP calcs
- Removed an unused variable
- Fixed Rupt dmg

Rawr.TankDK:
- Implementation of some combat data metrics as requested in the discussion forums. This should help us figure out why the mastery numbers are so far off. Includes a tweak to the DS & BShield methodology. Which, while making things worse for Mastery valuation, is much more accurate. Looking at the avoidance numbers next
- Some Cleanup. Additional NoCS tweak
- Fixed Missing Dodge from Agi.
- Fix for Improved DeathStrike properly buffing Healing done
- Adjustment of Survival math to include all static damage reduction, not just armor
");
#endregion
#region Rawr 4.0.16 (Jan 16, 2011) [r57235]
VNStuff.Add(
"Rawr 4.0.16 (Jan 16, 2011) [r57235]",
@"Cataclysm Release Beta

Rawr.Addon:
- Drycoded adding options to play sounds on looting items that are upgrades. NOTE: NOT TESTED IN GAME YET!! ie: probably typos crashes etc ALPHA quality
- Update packaging to include LibSharedMedia-3.0
- Fix typos in packaging
- Add LibSharedMedia to embeds.xml
- Fix locales
- Add options to select sounds to play if an upgrade is seen
- Fix issue if slot in direct upgrades isn't loaded from cache yet
- Fixed shift clicking of Rawr slots or upgrade lists puts item links in chat
- Fixed ctrl clicking of Rawr slots or upgrade lists shows items in dressing room
- Added some default test sounds
- Tweaks to looting from looting window (dry coded)
- Force check to load media once initialized
- Changed to have 3 ranks of sounds
- Fixed issue with checking item ids with nil itemstrings
- Looting mobs tested - bug fixes
- Added warning frame and warning frame options
- Upgrade shows percentage of upgrade
- Tested Warning frame seems to all be working now
- Added various default sounds
- Added Loot Upgrade Check when Need/Greed roll window pops up
- Release as Version 0.62
- Added command line /rawr import as per website description

Rawr.Base:
- Updated EU and Korean server listings with missing server names (There are a few EU and several Korean server names that appear to be no longer listed on Blizzard's server status pages)
- Added new PTR enchants to the global PTR mode
- Added new Meta gem requirements for the new Agil, Strength, and Intellect metas
- Updated PTR information of Rune of Swordshattering and Rune of Swordbreaking with their reduction of 60% disarm effect
- Updated Welcome Window with Moonkin's Fully Ready status
- Fixed Tinkering showing up in Enchant listing
- Updated Racials with PTR info
- Added a generic Armor Specialization check to the Character Class
- Fixed the meta references for the three new metas, Hina had entered the JC spell to create them, not the metas themselves
- Updated the Armor Damage Reduction formula based on multiple sources. This appears correct though someone else should probably take a look at it to be sure
- Fixed Spirit proccing from Lightweave Embroidery (version 2)
- Updated Spell Crit Reduction from Boss level mobs from 2.1% to 1.8% based on EJ testing
- Adding some of the missing Racials

Rawr.BossHandler:
- Updated the Armor values list to what SimCraft is using
- Disabled the Armor value box and tied it's selected index to the Level value. This means if you select 87, it will auto-select 87's matching Armor
- Adjusted Dynamic Attack so that the attackspeed was the total attack interval rather than the interval between just 1 each attack on the list.

Rawr.Buffs:
- Fix for Issue 19643: Elixir of the Master (Mixology) missing - The improved was typo'd as 'Deep Earth' instead of 'the Master'
- Fix for Issue 19565: Inscription scrolls not correctly limiting - Changed the ConflictingBuffs lists again
- Fix for Issue 19656: Indestructible Potion Duration incorrect - Updated to Cata value
- Updated Synapse Springs, Darkglow Embroidery (Rank 2), and Weapon Chains with PTR information based on Blizzard's updated PTR notes
- Fix for Issue 19685: Enabling a Scroll does not disable Mixology bonuses - The check that would turn it off was being skipped due to the professions enforcement. Fixed to support both

Rawr.Items:
- Updated Left and Right Eyes of Rajh's chance to proc in PTR mode (50% chance to proc on Physical/Melee Crit)
- Added support for Mandala of Stirring Patterns's PTR On Equip effect
- Added PTR's PvP helm and shoulder enchants
- Small update to Profression created items
- Started work on adding abilities to Blackwing Descent Bosses for BossHandler

Rawr.ItemFilters:
- More adjustments
- Corrected a few errors with the use of the '(The\s|)' and the '.*' in '.*Vortex Pinnacle', just to keep it simple and added a few more filters. Combined Baradin's Wardens/Hellscream's Reach as they are just faction vendors with the same items just in different places. Also included a special curency tab for Tol Borad Commendation
- Corrected Error in The Bastion of Twilight filtering
- Feature 19675 Completed: Added Item Level filtering for Normal and Heroic Dungeons and Raids
- Corrected Error in 'Disable 346-358 Cata Dungeons (H)'
- Disable By Item iLevel filters removed from Filter Tree
- Activated the Filters by Item Level Accordion at the bottom
- Filled out the Accordion with two option sets: Checkboxes which when checked will show that ilevel group and a Custom Slider where you can manually set a range you want to see
- Added Reset and Uncheck All buttons to the Accordion
- These filter values save to the Character object so they will be consistent with each save file managed
- The Sidebar will now remember how wide it was from last program launch

Rawr.LoadCharacter:
- Fox for Issue 19657: Reload Character Crash - Was trying to remove items from a collection that was being iterated over

Rawr.Optimizer:
- Fix for optimizer

Rawr.Bear:
- Improvements to Vengeance calculations in Bear

Rawr.DPSDK:
- Fix for Issue 19648: XAML parse error on the options pane
- Implement BonusWhiteDamageMultiplier

Rawr.DPSWarr:
- Updated Meta List to add the new Strength Meta to the templates. Will remove other now useless templates once 4.0.6 hits

Rawr.Enhance:
- Daggers aren't relevant items

Rawr.HealPriest:
- Updated Priest Base Stats

Rawr.Mage:
- Fixes, support for Heart of Ignacius, support for mastery procs for fire and frost specialization

Rawr.Moonkin:
- Add a PTR mode switch and plumbing
- Add Moonfire/Sunfire mana cost reduction
- Add Eclipse Mastery buff
- I'm pretty sure I've implemented all the mechanics. Updating to Fully Ready

Rawr.ProtWarr:
- Some fixes against the StatsWarrior class and handling effect procs

Rawr.Retribution:
- Added a couple of exploritory classes for setting up a state manager
- Added a static constants class for a central location for coefficients, etc, for easy maintenance

Rawr.Rogue:
- Updated attack damage values to level 85. Removed Anesthetic Poison

Rawr.ShadowPriest:
- Updated Priest Base Stats

Rawr.TankDK:
- Fix for BoneShield & Vengeance based on dynamic attack info
- Implement BonusWhiteDamageMultiplier

Rawr.Warlock:
- Added Stat Graph to Info tab on Options pane
");
#endregion
#region Rawr 4.0.15 (Jan 09, 2011) [r57038]
VNStuff.Add(
"Rawr 4.0.15 (Jan 09, 2011) [r57038]",
@"Cataclysm Release Beta

Rawr.Addon
- Dry coded changes to import subpoints
- Fixed a missing comma on export
- Move bank items into savedvariable
- Reworked Gem exports to use gem ids
- Reworked export to Addon to include equipped score data 
- Import now shows DPS subpoints on tooltip on import paperdoll frame
- Fix color display of tooltips
- duCalcs are now passed to the export
- Prep work to use ItemSets for writing character & loaded character
- Prep work for exporting direct upgrades
- Refactored export to use ItemSets
- Changed Direct Upgrades button to show hide changes - doesn't hide at present
- Reworked Import to use new loaded/character import
- Release as Version 0.40
- Import now shows differences between what was loaded (from addon or Battle.net) and what was displayed when doing export - This means you can load up your character do some tweaks/optimisations load it back into Addon and see changes in game
- Tweak for dataloaded always being false on reloadUI
- Changed Tooltip to use custom tooltip
- Update minimum build & addon versions
- Refactored export to addon - introduced upgrades - needs subpoint values
- Added SlotId to Item - Needs testing with addon v0.41
- Added wait cursor to Export to Addon menu click.
- Only export upgrades for regular slots
- Fixes to DU data
- Added comparison tooltip - now shows difference between loaded and exported
- Fixed Bank Export
- Added Output on scanning bank
- Added GemId to enchantID routine - fixes display of gems IF user has seen gems in itemcache
- Added text to comparison tooltips to identify which is which
- Initial coding of upgrades frame - kinda right position does nothing at present - Have now got Icon buttons set and text objects reads, Layout looking a lot better too. Still no actual data populates the frame
- Added fix for professions being localised
- Update minimum addon version to import
- Implemented CheckBoxes on Import form for Direct Upgrades Processing
- Implemented CheckBoxes to select Display Upgrades Filter
- Implemented Select All/Clear All buttons
- Build Upgrade List now works on checked/unchecked items
- Direct Upgrades now scrolls and displays overall upgrade score.
- Icon textures not yet working though
- Added ItemIdToEnchantId.lua file should really be in Rawr4
- Now shows tooltip for Direct Upgrade items 
- Direct Upgrades now show tooltips and comparison tooltips
- Direct Upgrade scrolling can now also be done by mousewheel
- Release as Version 0.51
- Fix issue with first time use of addon
- Convert ItemIdToEnchantId.lua to a XML version for Rawr4.
- Added ItemIdToEnchantID files to TFS project
- Added GemIDtoEnchantId convertor changed Item.ToItemString to use EnchantId.
- Release as Version 0.52
- Changed Import to use GemEnchantId and not GemId
- Changed version on export to ensure 4.0.15 release works with addon.
- Added version check of Rawr data on import
- Release as Version 0.53
- Direct Upgrade values are rounded to two decimal places
- Tooltip values are rounded to two decimal places
- Added fix for Blizzard bug on Mage talents in patch 4.0.3
- Mage Talent bug is in spurious 15th talent in Arcane tree. 

Rawr.Base:
- Fix for issue 19503: Weapons capped at 2000 max damage - Was a type with Maximum Damage in the Item Browser. Set it to 10000 instead.
- Fix for Chaotic Skyflare requirements
- Fix for Issue 19418: Overwrite save set does not overwrite - Changed the way it checks to see if the set is in the list
- Adjusted the size of the Reforging, Enchanting, Tinkering and Blacksmith socket boxes to save some room on the UI for smaller screens.
- ItemSets are now saving/loading from character xmls, yay!
- Removing level-based partial resists
- Agi no longer provides armor
- Added a RetryCount check to the number of times it tries to load the caches. If it fails more than 4 times in a row, it will just throw an error and stop trying. The error will provide more instrucitons to the User about deleting their Silverlight Cache to try and get new ones.
- Fix for Issue 19542: EU-Malorne missing in Server List - Added
- Fix for Issue 19519: Tooltips with Large Amounts of Text Disappear - Swapped Tooltip from base to a Popup that stays until mouse moves away. Also added a Header usage as an option for a Bold line at the top
- Added GetItemSetByName to Character for Addon
- Added GetDirectUpgradesGearCalcs to GraphDisplay for Addon
- Calling GetItemSetByName('Current') will return the character's equipped items (null if they are null)
- Loading in a character from the Addon or Armory will automatically assign an ItemSet of the gear at that time - This can be used to compare all changes since last load and will be passed into the Rawr Addon to show comparisons
- Added Uldum to EU Server List
- Fix for Issue 19600: Wrong value for the Titanium Plating - Titanium Plating has only 26 Parry and it has 50% Disarm Dur Reduc. Updated
- Canceling a new, open or load character so you can save your file first should now result in the action actually being cancelled
- Fully removed several things from Rawr that are no longer in game:
- - Armor Penetration Rating and related value to rating and armor reduc from rating, etc functions. Armor Penetration as a Percentage still exists because of talents and abilities in some models, like Colossus Smash.
- - Defense and Defense Rating and related rating to value, etc functions. Defense has been completely removed from the game and all items and effects providing it have been converted to other stats like dodge and parry
- - Block Value, this stat no longer exists as all blocks result in a 30% value (modified by talents and abilities)
- - Crit Reduction from Resilience, this was removed from game
- - Armor from Agility, this was removed from game
- Stamina to Health conversion is now 14 from 10 per Cata changes
- Changed behavior of save file check before getting another file
- Fix for Issue 19370: Can't 'Open in Wowhead' when installed locally - This is a Silverlight issue, found a workaround and implemented. Tested fine
- Added a global PTR Mode to Options
- Tied 4.0.6 Meta Gem requirements to the PTR mode
- Corrected 4.0.3a Meta Gem Requirements for all Chaotic and Relentless Metas
- Fixed Tooltip Widths of several UI Elements on Options dialog by inserting character returns
- Added even more status info into Rawr to try and cut down on duplicate issues for inoperable models
- Description in Wowhead was incorrect with what Lightweave Embroidery procced. Procs 580 Intellect instead of Spell Power. Fixed enchant.
- Fixed an issue with array xml storage from a previous commit 
- Added a popup to the Install Offline button when you are already installed to alert the user to what they are doing and what they should be doing
- Enabled Reload Character from Battle.Net and Rawr Addon

Rawr.BossHandler:
- Task 18226 Completed: Add a flag for time periods where Boss takes Bonus Damage - Added BuffStates as a list on BH. Just like other Impedance and Attack lists it uses the Freq, Chance, Dur, system and also includes a Stats object to reflect bonuses or penalties to players. UI created as well

Rawr.Buffs:
- Added the new Cataclysm Elixirs
- Added Shamans to the Select Buffs By Raid Members Dialog
- Added Druids, Paladins, Rogues and Warlocks to the Select Buffs By Raid Members Dialog
- Buffs by Raid Comp should be fully functional now
- Fix for Issue 19565: Inscription scrolls not correctly limiting. Added the respective Guardian and Battle Elixir conflicts to Scroll Buffs
- Fix for Issue 19508: Values for Resistance Buffs are too high - Updated those buffs to 195 from 1105 (an old PTR value)
- Arcane Brilliance (SP) was incorrectly marked as coming from Shamans

Rawr.Items:
- Feature 15466 Implemented: Add Item Drop Rate to the Tooltip - Implemented Drop Rates for static drop items, will pull info on wowhead refresh or you can manually populate it with the Item Source Editor
- Fixed source parsing for Crafted Items. The skill type value was stored as an integer, not string so it wasn't converting properly
- Changed Crafted Items Desription function to not show the '(0)' if it's 0.
- Fix for Crafted Source UI editor, wasn't fixing nulls 

Rawr.ItemFilters
- Minor fixes
- You can now filter by Drop Rates. A default set of Disable filters was added but not sure if thats what we will decide to keep

Rawr.Bear:
- Added Avoided Interrupts % optimizable value

Rawr.Cat:
- Fix for Issue 19611: T11 2P Bonus wasn't relevant to Cat - Added BonusRakeTickDamageMultiplier to the HasRelevantStats function (was already in GetRelevantStats) 
- Added Avoided Interrupts % optimizable value

Rawr.DK:
- Tweak the GetSpec() function
- Add Mastery to Paperdoll output on TankDK
- Add SpellDamageTakenMultiplier so that Effluent Meta is properly evaluated
- Update Gem Template slightly
- Pull out dead function
- Adding in initial rough of DRW
- Adding in rough of T11 set bonuses
- Implmenting some of the 4.0.6 changes
- Fixed RuneTap implementation SE
Rawr.DPSDK:
- Work for Issue 19414: Models using old crit reductions - Updated DPSDK to pass BossOptions around so Target Level could be pulled from it and used for calcs.
Rawr.TankDK:
- Updating one of the BossHandler GetDPS functions to include Attack Speed adjustments.
- Adding Initial Impedence handling in the mitigation section. Things like Run Speed increases & Stun reductions should now have values when using a Boss that includes those kinds of impedences. You will see this in the tooltip of Mitigation values.
- Fixed/added some gemming templates

Rawr.DPSWarr:
- Stat Graph settings for Str and Agi checkboxes weren't assigning to the UI correctly, fixed
- Fixed the Heroic Strike and Cleave usage against Rage Cost variables, they needed to be inverted
- Implemented 4.0.6 patch changes for DPSWarr and tied them to the PTRMode check on the model Options Pane
- Both Warrior models are now using a StatsWarrior class instead of Stats. This reduces the overall size of the Base Stats class. Ours is fully implemented for all accumulate functions, etc. 
- Migrated several things to the new StatsWarrior object

Rawr.Enhance:
- Work for Issue 19414: Models using old crit reductions - Ensured no hardcoded level 85 or 80 numbers for character level were left, all now use the Character.Level at some point or another
- Fix for Issue 19511: Lava Lash CD appears to be too fast - Searched all references for Lava Lash and verified set cooldowns to 10s from 6s
- Work for Issue 19581: Elemental Precision not being factored. Added the necessary mod to the tooltip

Rawr.Hunter:
- HunterEnums Updated, new spells added, old spells removed. Update all references
- Refactor ManaCost -> FocusCost
- Update Base DMG / Cooldown of existing Spells
- Added Cobra 2 Shots
- Remove Some Mana Infos From Display
- Some refactoring MPS -> focus per secound

Rawr.Mage:
- New Arcane Light option - simplified arcane model using only mana neutral cycle mix and AB spam
- Fix for Flashburn
- Added T11 set bonuses
- Support for Gale of Shadows
- Updated base stats
- Fix for Pyroblast coefficient
- Updated Pyro dot uptime model
- Updated fire cycles
- More fixes and rework of Combustion (not finished yet)
- Heuristic adjustments to Pyro dot uptime and Combustion, switching to latest T3 HS formula

Rawr.Moonkin:
- Add an option to prefer either Hit or Spirit for reforging on gear
- Reducing the Wild Mushroom damage until I'm sure I know how hard the things hit
- Correct the math for Treants
- Redo the combat table to reflect new testing
- Redo swing speed, base AP, base weapon DPS
- Re-add Target Armor Reduction as a relevant stat and implement it. Still to do: Figure out what crit/haste buffs apply to the Treants and apply them
- Fix treant base DPS
- Correct the formula used to scale treant hit to treant expertise
- Add crit/haste raid buffs and Heroism to treants

Rawr.ProtPaladin:
- Work for Issue 19414: Models using old crit reductions - Ensured no hardcoded level 85 or 80 numbers for character level were left, all now use the Character.Level at some point or another

Rawr.ProtWarr:
- Fix for Work Item 19599: gemming templates now have the correct Austere meta gem and no longer default to JC gems turned on
- Both models are now using a StatsWarrior class instead of Stats
- This reduces the overall size of the Base Stats class
- Ours is fully implemented for all accumulate functions, etc

Rawr.RestoSham:
- Restored front panel look of resto sham stats now that beta model is disabled partially
- Mastery fix, wrong number used originally
- First fix for issue 19553
- First fix in place for 19409
- More work on the new model
- Fixed a bug with Water Shield in the current model
- Various fixes

Rawr.Retribution:
- The model still doesn't work but it at least doesn't crash on load character now

Rawr.Tree:
- Removed unnecessary fields to clean UI
- Traced variables and commented on usefulness in my tweaks of the solver
- Healing simulation updated

Rawr.Warlock:
- Fixed several array bugs and nullcheck errors
- Improve fix for issues 19488, 19391
- clean up item relevancy rules; fix bug causing GCDs < 1 sec
");
#endregion
#region Rawr 4.0.14 (Jan 01, 2011) [r56705]
VNStuff.Add(
"Rawr 4.0.14 (Jan 01, 2011) [r56705]",
@"Cataclysm Release Beta

Rawr.Addon:
- Added export of empty tinkered items until Blizzard adds API call for checking tinkers.
- Fix paperdoll display scaling issue
- Fix export of empty profession
- Lock frame to UIParent and use its native scaling

Rawr.Base:
- Added Tinkerings as a listing to the Equipped charts (under All and its own header).
- Fix for blue diamond toggle.
- Fix for default values of buffs.
- Updated support for single changes direct upgrades optimization method.
- Changed the Block% meta gem to only give a 1% bonus rather than the stated 5% bonus to match in-game testing
- Improved comparison charts for gem selection.
- Fix for gem selection when socket is empty.

Rawr.BossHandler:
- Adding some handling code adjusting what Jothay had put in. To deal w/ physical v. Magical attacks. And updated the one T11 boss implemented to match.

Rawr.Items:
- Major update to trinket proc modeling
- A few adjustments to a few melee enchants
- Updated Filter with several missing bosses
- Slight adjustment to Heart of Ignacious and Jar of Ancient Remedies procs

Rawr.LoadCharacter:
- Fix for dashes being allowed in the server name, and showing up as defaults instead of spaces.
- Restricted server names to the valid list of values, to prevent mistakes.

Rawr.Bear:
- Fix for broken Stamina multiplier from HotW.
- Fix for broken Health multiplier from Stamina.
- Improved attack power calculations.
- Fixed base attack power.
- Fixed Glyphs showing up, but only Mangle is modeled currently.

Rawr.Mage:
- Fix for quadratic solver.
- Fix for DotTick trigger proc.

Rawr.ProtWarr:
- Adjusted the base damage/threat and coefficients of a number of abilities
- White damage is no longer reduced by Heroic Strike usage
- Fixed Devastate having no value in the talent point view when using Sword and Board
- Added initial support for Vengeance--slider added to the options pane, defaulting at 60% stack
- Added support for Cataclysm gemming templates
- Adjusted yellow critical hits to use two-roll mechanics
- Added support for BonusBlockValueMultiplier stat
- Fixed Mastery base value double-dipping issue causing Block% to be too high

Rawr.DK:
- Fix issue w/ Base damage valuation for Spell v. Physical hit.
- Rotations: Provide pre-set rotations to help until solver is handled.
- Fix issue w/ rotation math coming up w/ weird values for rotation duration.
- Implement initial Scent of Blood work.
- Gem Templates for TankDK using new gems.
- Updated Relevant stats for DPSDK to exclude defensive stats.
- White damage wasn't properly included in the rotation outputs.
- Fixed base stats... they'll need some further tweaking.
- Fix for melee/spell special counts
- Use Pre-made blood rotation in DPSDK when in a Blood Spec.
- Update discription of TankDK DPS & Threat values to include max Vengeance.
- Fix for 16078: Display the rotation on the options tab and it's working w/o going crazy.
- Solver is now actually doing some work. The rotations don't always make alot of sense and it's way big on the DPS numbers, but it's now a working set.
- Setting TankDK as 'Mostly' since Survival and Mitigation values are looking reasonable. And threat is OK as long as it's using the pre-set rotation. DPSDK on the other hand still needs work. It's work will dial in the Threat on TankDK.
");
            #endregion
#region Rawr 4.0.13 (Dec 28, 2010) [r56600]
VNStuff.Add(
"Rawr 4.0.13 (Dec 28, 2010) [r56608]",
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
- Vastly improved default and preset values on the Options tab.
- Improvements to Vengeance calculations

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
        private void BT_LoadRawrRepo_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Instance.LoadFromRawr4Repository(null, null);
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
