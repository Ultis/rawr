using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Rawr.Base;

namespace Rawr.Hunter {
	public partial class CalculationOptionsPanelHunter : ICalculationOptionsPanel {
        private bool isLoading = false;
        private bool firstload = true;
        /// <summary>This Model's local bosslist</summary>
        private BossList bosslist = null;
        private Dictionary<string, string> FAQStuff = new Dictionary<string, string>();
        private Dictionary<string, string> PNStuff = new Dictionary<string, string>();
        public UserControl PanelControl { get { return this; } }
        private Character _char;
        public Character Character
        {
            get { return _char; }
            set {
                if (_char != null && _char.CalculationOptions != null
                    && _char.CalculationOptions is CalculationOptionsHunter)
                    ((CalculationOptionsHunter)_char.CalculationOptions).PropertyChanged
                        -= new PropertyChangedEventHandler(calcOpts_PropertyChanged);

                _char = value;
                if (_char.CalculationOptions == null)
                    _char.CalculationOptions = new CalculationOptionsHunter();

                CalculationOptionsHunter calcOpts = _char.CalculationOptions as CalculationOptionsHunter;
                DataContext = calcOpts;
                calcOpts.PropertyChanged += new PropertyChangedEventHandler(calcOpts_PropertyChanged);
            }
        }
        public CalculationOptionsPanelHunter() {
            isLoading = true;
            try {
			    InitializeComponent();
                SetUpFAQ();
                SetUpPatchNotes();
                SetUpOther();
            } catch (Exception ex) {
                new ErrorBox("Error in creating the Hunter Options Pane",
                    ex.Message, "CalculationOptionsPanelHunter()",
                    ex.InnerException.Message, ex.StackTrace);
            }
            isLoading = false;
        }
        public void LoadCalculationOptions()
        {
            string info = "";
            isLoading = true;
            try {
                CalculationOptionsHunter calcOpts;
                if (Character != null && Character.CalculationOptions == null)
                {
                    // If it's broke, make a new one with the defaults
                    Character.CalculationOptions = new CalculationOptionsHunter();
                    isLoading = true;
                }
                else if (Character == null) { return; }
                calcOpts = Character.CalculationOptions as CalculationOptionsHunter;
                //CB_BossList.Text = calcOpts.BossName; line = 6; info = calcOpts.TargetLevel.ToString();
                //CB_TargLvl.SelectedItem = calcOpts.TargetLevel.ToString();
                //CB_TargArmor.SelectedItem = calcOpts.TargetArmor.ToString();
                CalculationsHunter.HidingBadStuff_Spl = calcOpts.HideBadItems_Spl;
                CalculationsHunter.HidingBadStuff_PvP = calcOpts.HideBadItems_PvP;
                ItemCache.OnItemsChanged();
                //
                calcOpts_PropertyChanged(null, null);
            } catch (Exception ex) {
                new ErrorBox("Error in loading the Hunter Options Pane",
                    ex.Message, "LoadCalculationOptions()", info, ex.StackTrace);
            }
            isLoading = false;
        }
        private void SetUpFAQ()
        {
            FAQStuff.Add(
            "Why does my toon do 0 DPS?",
            @"There are a couple possible reasons this could occur.
1) You don't have a Ranged Weapon, all DPS is tied to having a Ranged Weapon.
2) Your Situational settings on the Fight Info tab are set such that you ave no ability to get any DPS out during the fight."
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
            /*//CB_FAQ_Questions.Items.Add("All");
            string[] arr = new string[FAQStuff.Keys.Count];
            FAQStuff.Keys.CopyTo(arr,0);
            //CB_FAQ_Questions.Items.AddRange(arr);
            //CB_FAQ_Questions.SelectedIndex = 0;
            CB_FAQ_Questions_SelectedIndexChanged(null, null);*/
        }
        private void SetUpPatchNotes()
        {
            PNStuff.Add(
            "v2.2.28 (Unreleased)",
            @"- Fix for issue 14739: Optimize Requirement '% Chance to Miss' error (calcOpts wasn't being set before the call to it in the requirement, added set before call)
- Fix for bug with Hit Rating where more of it would hurt SpecialEffects
- Applied Patch 4398:
  * Adding tab to show shot rotation details
  * NOTE: This patch was applied but it presently doesn't have a function (it's just visual). Will add function later
- Fixed Strength stat from Gear pulling, also made it add to attack power like it should
- Added Profession Handling for Buffs, will auto-activate buff bonuses if you have the related Profession selected.
- Added Automated Buff Anti-Double-Dipping for Trueshot Aura. If you are maintaining it yourself, then it ignores the Buff version and uses your Talents instead. (This needs more verification)
- Fixed Crit Rating Procs so they will actually add to your Crit %
- Moved all references to the QuickShots effect to GetCharacterStats as a simple PhysicalHaste SpecialEffect (will add aspect downtime to it later)
- Moved all references to Call of the Wild to GetCharacterStats
- Moved all References to Trueshot Aura to GetCharacterStats
- Changed the RAP tooltip to just show base, agi, str bonuses (will add gear later)
- Fixed the activator that would turn off Buff version Trueshot aura when you have the talent yourself
- Added Hunter's Mark to Buff disabling when you have the relevant talents/glyphs (this still isn't working the way that I want)
- Moved Aspect of the Hawk and Dragonhawk's AP handling to GetCharacterStats
- Moved all references to Hunter vs Wild AP to GetCharacterStats
- Moved all references to Careful Aim AP to GetCharacterStats
- Fixed a Ranged Haste on Procs bug
- Redid hunter T8 4 pc to use a different trigger and method (and removed the old method stat placeholder from Stats)
- Moved all references to Expose Weakness AP to GetCharacterStats
- Moved all references to Furious Howl AP to GetCharacterStats
- Added a couple of files to begin work on converting shots to classes (will make them easier to manage and prevent some things from not affecting others)
- Fixed the Details tab resizing
- Aligned more of the back end for the new Ability system
- Started adding in shots using the Ability class method from DPSWarr (will not do any rotation work until all of this is settled first)
- Finally fixed the Buffs that Double-Dip. I had added the function that does the special handling but GetCharacterStats wasn't using it, D'OH!
- More shot data setups
- Moved TFB above OP in the priority queue, because OP's use is limited by TFB
- Corrected a bug in Arms where Slam would try to use more rage than you had available, which led to negative DPS while the rotation tried to correct itself
- All models using Prof enchant hiding have been changed to use the global sets on the Stats Pane and Options > General Settings > Hide enchants based on professions. Models that were handling this manually have been edited to the new method, models that didn't have it are now on it as the back end changed
- Profession bonus Buffs (Toughness and the like) are now updated when you update your professions in all models. Models that were handling this manually have been edited to the new method, models that didn't have it are now on it as the back end changed
- Added a new Stats for global Mana Cost Reducs by Percentage (for the Beast Within talent)
- Changed BonusDamageMultiplier usage in Hunter to not be restrictive (which was just dumb). It can now handle sources beyond just Buffs and be used in more places
- Changed handling of The Beast Within and Bestial Wrath. Now these are handled using SpecialEffects and better modifiers, though I do need to add more of it into Pet damage calculation. Also fixed teh Duration from 18 to 10 sec
- Changed the Armor Damage Reduction Formula for Pets to use the global one (just without ArP Rating)
- Added some Traps and Volley as selectable in your rotations (started out as a way to give value to several glyphs that were not being used and just kinda blew up from there)
- Fixed a bug I accidentally made with Armor Ignorance on Pets
- Removed the emulate Spreadsheet Bugs option
- Added a PTR Mode to enable 3.3 changes. Most affect stuff Hunter isn't set up to handle, but CullingTheHerd has been added to the Pet Talents so it can be selected

Completely reworked the Pet Talents storage and interface (these steps will lead towards storing Pet Specs alongside Hunter Specs and also makes it a lot easier for me to maintain)
- Interface on the options pane now looks more like the actual talent trees, though I still have some more tweaking to do
- Back end stores the talent info as a string, just like normal talents. Way better than the monstrously sized PetTalents section in the XML files before

- Added support for T10 2PC & 4PC, the values are in there but the uptimes are wrong, won't be correct until new rotation setup is created
- Fixed a couple missing relevant Triggers for Hunter Special Effects
- Removed Pot selection from Options Pane, migrating this to use Buff version of pots instead
- Added Nature Damage as a relevant stat

- Removed option to 'calculate uptimes like the spreadsheet' since the back end for it no longer exists
- Added Multiple Targets % to control Volley usage and reworked Volley's activation to enforce it's channeling
- Moved Traps to a separate section on the Stats Pane from Shots
- Removed Racials from the Stats Pane (back end is there)
- Adjusted the display of some of the Mana Regen stats
- Added the RAP boost coeff to Volley
- Added a failure reason to Volley for not having Multiple Targets active and at >0%
- Fixed a potential bug with certain gear setups that would break Mana Regen (causing it to go to Infinity)
- Made the Randomize Procs on Rotation Test option gray out if not using Shot Rotation Test
- Added the PetBuffSelector directly to the Options Pane instead of using a back end function to add it on load (previous builder didn't know how to add it)
- Moved Pet stuff to a subfolder
");
            PNStuff.Add(
            "v2.2.27 (Nov 10, 2009 03:45)",
            @"- Fix for broken Trolls and Dwaves.");
            PNStuff.Add(
            "v2.2.26 (Nov 09, 2009 01:53)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.25 (Nov 08, 2009 20:15)",
            @"Please note that this Versions's notes will be difficult to read due to the nature of the commits occurring here.

- Applied Patch 4276 (by Drizz): Here is the small patches I made to the hunter module. I have tried to put a comment '// <date> Drizz: ...' at each of the places where I have been doing changes or adding something. List
Have tested from a MM spec perspective. So BM and Surv spec talents and shots (e.g. Black Arrow, Exploding Shot have not yet been tested/corrected)
CalculationsHunter.cs
* Added display of Piercingshot element in stats view.
* Trinket: Added handling of Heroic version of Death's Choice/Verdict
* Added handling of Talent Piercingshots
* Trinket: Added handling of Banner of Victory
* Corrected the WildQuiver DPS to include the ViperUptime penalty
* Aligned Calculation with Spreadsheet on CritAdjust for several shots
* Moved the armorReduction to be multiplied in later
* Changed Buff check to check for 'Windrunner's Battlegear' instead of 'Windrunner's Pursuit' for the 2 Piece T9 bonus. The Buff's name don't seem to be 'changed'
* Aligned the calculations of the 2pT9 effect.
* Rearranged calculations for Chimera Shot.
RotationTest.cs
* Updated to wait for Rapid Fire if close to cooldown in Rotationtest
CalculationOptionsPanelHunter.cs
* Added an option to be able to set the CooldownCutoff from the GUI
CalculationOptionsHunter.cs
* Changed the default cooldownCutoff to 30 (from 15) to align wiht spreadsheet
ShotPriority.cs
* Updated the calculation of critSpecialShotPerSecond to align
PetCalculations.cs
* Updated the focusRegen calculations to align with Spreadsheet.
* Added armor effect to pet calculations
* Updated name of Windrunner Buff for the 4pT9 effect. (Battlegear not Pursuit)
* Corrected Kill Command calculations
CharacterCalculationsHunter.cs
* set the initiation of shots to set Serpent sting to be able to crit.
HunterEnums.cs
* Updated some static constants to align with numbers in spreadsheet (including new armor pen value=
* Created a new constant for the Pet Cirt damage multiplier
CalculationsOptionsPanelHunter.cs
* made GUI of the update cooldownCutoff
- Applied Patch 3887: Updated Hunter Gemming to default to Epic gems instead of Rare gems
- Applied Patch 4304:
  RotationTest.cs
   * Changed a couple of initiation values to align with the spreadsheet
   * Removed a check for GCD to align with spreadsheet
   * Added special case for currentTime == 0
   * Changed to using LastAutoShotCheck value instead of the currentTime (align with spreadsheet)
   * Fixed a boundary error of the use of the rand.Next function
  CalculationsHunter.cs
   * Adding the Mangle/Trauma buff effect to the Piercing Shots
  CalculationOptionPanelHunter.cs
   * Added a checkbox for Randomizing Procs on Rotation Test
Rawr.HunterSE: Adding this model so Devs can do a verification for 2.2.25 release (will replace current Hunter model).
Rawr.HunterSE:
- Updated the Optimization Requirements to use Percentages instead of Ratings and added a couple more options
- Added requirement to Buff Relevance that it must be obtainable by a Hunter (E.g.- a DK only buff won't show up in the set bonuses)
Rawr.HunterSE:
- Cleaned out the PvP set bonuses (there is only one since they all share)
- Set the Buffs to filter out if not available for this class
- Fixed a Crit from AGI tooltip
- Migrated a couple of back end items to use SpecialEffects for Uptime instead of a private Hunter function (which is now deleted)

Rawr.Hunter:
- Turned the Debug Shot Rotation setup off (kills performance)

Rawr.HunterSE:
- Added Hide Bad Items setup (similar to DPSWarr). Can hide Spell gear (based on Spell Power and Spell Penetration) and/or PvP gear (Based on Resilience)
- Added Hide Enchants Based on Profession (non-engineers won't see Hyperspeed Accelerators, etc)
- Renamed a lot of the Interface Objects on the Options Pane for better naming convention
- Cleaned up the Interface scaling on several sections of the Interface
- Added Piercing Shots talent work by Drizz (was supposed to be in last commit but apparently I missed it)

Rawr.HunterSE:
- Refactored Pet Talents into a new class and changed all references to that
- Fixed window being too small so interface items are hidden issue (added scrollbars where applicable)

Rawr.HunterSE:
- Adjustments to reduce DPS differences between Hunter and HunterSE
- Reformatted Damage for shots on the Stats pane to 'Damage : DPS'

Rawr.HunterSE:
- Added Freq to the stats pane for shots
- Chimera shot's primary issues fixed (couple of the multipliers were bad)

Rawr.HunterSE:
- Fixed Auto-Shot DPS (haste wasn't set correctly)

Rawr.HunterSE:
- Steady Shot Damage fixed (DPS still off)

Rawr.HunterSE:
- Fixed Steady Shot DPS (Haste was set wrong, have now checked everything that involves haste to fix all those breaks)

Rawr.HunterSE:
- Some more float conversion cleanups
- Resorted and added summaries to the Hunter Glyphs (users may need to reselect their glyphs)

***** OCT 31st, 2009: Removing old Hunter, Hunter SE is now Hunter *****

- Rawr3: Fix for compile issue (function doesn't have same call in Silverlight)
- Couple more changes to prevent compile errors
");
            PNStuff.Add(
            "v2.2.24 (Oct 24, 2009 18:00)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.23 (Oct 15, 2009 03:15)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.22 (Oct 13, 2009 05:16)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.21 (Oct 07, 2009 20:18)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.20 (Oct 06, 2009 19:22)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.19 (Sep 23, 2009 06:22)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.18 (Sep 23, 2009 04:36)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.17 (Sep 23, 2009 04:36)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.16 (Sep 13, 2009 21:51)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.15 (Sep 07, 2009 08:25)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.14 (Aug 30, 2009 07:18)",
            @"- use Readiness in rapid fire cooldown calculation
- make Readiness trigger a GCD
- fixed effect of Readiness on Kill Command cooldown
- implemented heroism effect and added heroism usage option
- the options pane now scrolls when needed
- fixed tab ordering on all options panes
- made items/enchants with use-effects appear correctly
- implemented the 5 proc haste trinkets/enchants on the spreadsheet
- implemented the 2 proc crit trinkets on the spreadsheet
- implemented improved steady shot (mana reduction & damage adjustment)
- fixed default pet rotation for pets without two of dive/dash/charge/swoop
- calculate pet specials crit frequency soon enough for Ferocious Inspiraion calcs
- show more pet crit detail in stats pane
- use heroism for pets too
- don't let the pet dodge effect be beneficial
- calculate mana regen from roar of recovery
- calculate mana regen from invigoration
- adjust pet ap for animal handler
- calculate the t7 pet bonus correctly
- calculate the beastial wrath effect for pets
- show full (correct) haste calculations in stats pane
- correctly adjust mana for glyph of arcane shot
- calculate mana and damage effects from improved steady shot
- calculate mana regen when viper is up 100%
- added cobra strikes calculation
- made the utility calculation functions static
- implemented t7 2-piece bonus
- implemented t9 2-piece & 4-piece bonus
- correctly display set bonuses on the buffs tabs
- apply Aspect of the Beast AP bonus correctly to pet
- apply the correct number of Rake dots (based on frequency)
- implemented rabid effect
- implemented pet glancing blows
- implemented feeding frenzy
- use debuff calculations everywhere they're needed
- show AP from debuffs in tooltip
- apply AP from call of the wild correctly to the hunter
- calculate aspect uptimes correctly when using AotB during BW
- rounding tweak for serpent sting damage
- implemented the missing +AP proc trinkets
- implemented the missing +ArPen proc trinkets
- implemented the missing +Damage proc trinkets/enchants
- implemented the missing +Agi proc trinkets
- hunters are interested in direct-damage fire effects
- tweaked troll berserk and orc blood fury effects
- spirit does not affect our dps
- melee-triggered special effects are useless to hunters
- no DK runes pls
- use MP5 from stats and gear
- implemented Darkmoon Card: Crusade
- updated base AP from Swordguard Embroidery
- mjolnir trinket now 665 ArP
- implemented trinkets with simple +haste use effects
- removed emulations for many spreadsheet bugs fixed in 91b:
* wrong trinkets being counted as alchemist stones
* dust cloud, snatch and tendon rip can no longer trigger crit procs
* spore cloud debuff uses the correct freq
* acid spit debuff uses the correct freq
* monstrous bite proc uses the correct freq
* scorpid poison uses correct AP scaling
* spirit strike uses correct AP scaling
* poison spit multiplies damage correctly by number of dots
* scorpid poison damage uses the correct freq
- now compatible with spreadsheet 91b
- beastial wrath correctly reduce the mana cost of kill command
- Bestial Wrath can only be used if you have the talent
- PriorityRotation uses the options object directly - moved some constants in there
- implemented Scorpid Sting
- implemented Viper Sting
- implemented Immolation Trap
- added a rotation test (disabled by default)
- calculate static haste before dynamic haste
- modified priorityRotation to correctly calculate specials pet second and the crit ratio sum
- don't calculate shot frequencies the old way when we're using the rotation test
- make sure we initialize the timings (time_used & rotation_cooldown) regardless of rotation test (and after we know duration!)
- always calculate crits_ratio for every shot, even when its ratio is 0
- correctly recalculate crit ratios in rotation test mode, once we know steady shot cast time
- readme update
- replaced the old custom chart with 4 useful ones
- added rotation DPS and MPS charts and cleaned up the others a little
- most buffs now working correctly (hunter & pet)
- removed second dodge adjustment from pet white damage
- crit tooltip tweak
- tweaked which buffs to offer
- recalculate armor, agility & base crit
- implemented JoW
- corrected damage adjustment for BA (it's not physical)
- corrected intellect base calculation
- calculated JoW correctly given that it's stored as a 1% restore
- hopefully the last JoW change for a while
- added an options tab for pet buffs
- removed pet buffs from hunter buffs filter
- added PetBuffSelector control
- fixed LotP/Rampage spreadsheet bug for pets
- store pet buffs in options xml in the same way the main xml stores character buffs
- pet dodge uses the hunter's non-rounded hit
- hide worthless foods
- added a note about possible MP5 stacking bug (posted on EJ)
- emulate the spreadsheet Mp5 stacking issue
- updated spreadsheet emulation to 91c
- remove untalented silencing shot correctly in rotation-test mode
- leave untalented silencing shot in frequency mode when emulating bugs
- added LALProcChance as an options (not set-able yet)
- rotation test is now on by default
- special shots per second should use final_freq rather than freq - refreshed shots count as shots
- shot better tooltips for refreshed shots
- emulate Serpent/Chimera spreadsheet rounding bug
- updated to match spreadsheet 91c(r2)
- another bug that got fixed in 91c(r2)
- show mana usage/regen details in stats output
- show a lot more detail for pet ap/hit/hit in stats output
- compatibility with spreadsheet 91e
- fix for rotation test - don't wait for non-gcd abilities
- automatically set frequency of (some) zero focus pet skills to their cooldown
- sting debuff uses real frequency for uptime calculation
- roar of recovery can use frequency in any mode since it now equals cooldown
- call of the wild can use frequency in any mode since it now equals cooldown
- updated partial resist formula for pet to match hunter
- troll berserking uses real frequency
- updated calculation for crit from racial to match bug in spreadsheet
- calculate pet timings before debuffs, else sting doesn't get factored in correctly
- changed T9 2-piece bonus crit damage for serpent sting
- fixed killShot sub 20 calcs to factor in viper for each shot instead of on the delta");
            PNStuff.Add(
            "v2.2.13 (Aug 12, 2009 02:28)",
            @"- Correctly calculate autos per sec, specials per sec and crittable specials per sec
- Correctly calculate crit bonus from master tactician
- Calculate cumulative percentage bonuses correctly
- Added agility bonus from Hunting Party
- Include crit chances in shot data
- Calculate composite crit ratios
- Show AP from expose weakness in tooltip
- Correctly compound static and dynamic haste
- Calculate Expose Weakness AP gain correctly, using critting shots per second & crit composite sum
- Calculate base agility correctly
- AP from Hunter vs Wild needs to be rounded down
- Fixed meta gem crit boost - was previously getting 0 and then compounding it incorrectly
- fixed wild quiver calculation
- split out wild quiver from autoshot dps
- cleaned up displayed stats list (and added placeholders)
- Mana regen overhaul
- General refactoring continues
- show AP from procs on tooltip
- show crit talent breakdown in tooltip
- recalculate frequencies before calculating shots per second (since we have final haste & steady shot speed)
- round crit correctly (or at least so that we agree with the spreadsheet)
- calculate correct noxious sting bonus for non-sting shots
- adjust serpent sting damage correctly and apply correct number of ticks
- adjust black arrow damage correctly
- calculate stamina from gear/race correctly
- Calculate LAL procs as soon as we know frequencies (frequencies will not need correcting at the end)
- Fixed expose weakness calculation (0.33,0.66, not 1/3,2/3)
- Added a few test trinkets (mine, of course ;)
- Updated hunting party to 3.2 values (2% per second, not 2.5%)
- replenishment buffs override hunting party
- calculate & display bonus damage from kill shot at sub-20% HP
- added all of the armor set bonuses (tiers 4-9, seasons 1-6)
- don't crash when character is not wearing trinkets or back enchant
- updated Thrill of the Hunt mana adjustment for 3.2
- removed Thrill of the Hunt mana adjustment from black arrow
- round MPS in the same way the spreadsheet does
- break out mps calculation from dps, so we can determine aspect uptime for dps
- added several new options (main aspect, viper usage, mana pots, phase timings and more)
- calculate and display aspect uptimes (based on mana rotation and needed viper)
- corrected aspect of the hawk AP calculation
- tweaked base mana calculation to match spreadsheet
- rearranged the option panels
- correctly save and load fight duration option
- removed some debugging
- explicitly mark regions where we're copying bugs from the spreadsheet (w/ a global toggle)
- fixed thread safety
- removed more cruft
- implemented quick shots (improved aspect of the hawk proc)
- Started overhauling the pet calculations
- Added options to emulate spreadsheet bugs & oddities
- Lots of cleanup and minor calculation adjustments
- Show more detail for hit and haste in tooltips
- Silenced all the warnings
- implemented first pet skill
- micro rounding adjustments to match spreadsheet
- my profile is now within 0.00007 DPS of the spreadsheet
- more pet skills implemented (bat-hyena)
- started on target debuff effects - armor debuffs from pet are working
- fixed bug with pet AP rounding
- calculate savage rend effects
- more pet skills implemented (bat-spore bat)
- emulate all spreadsheet KC crit bugs (some non-damaing abilities are counted as critting)
- implemented sting and acid spit armor debuffs
- implemented correct furious howl ap effect
- emulate spreadsheet cooldown bugs in acid spit and monstrous bite uptime calcs
- implemented every focus-using rotation pet skill that's in the spreadsheet (thunderstomp is missing)
- clear the pet family list before re-populating it (so it doesn't grow with each profile load)
- include the full/correct skill list for pet rotation pick-lists
- correctly load pet skill priority instead of overriding it with the default
- allow setting up to 7 pet skills in the priority list (up from 4)
- renamed the ferocity talent to 'Charge/Swoop'
- Fixed incorrect base health for 3 of the 7 hunter races
- added 3 more pet priority slots
- added Frenzy effects
- added Ferocious Inspiraion effects
- added ability to set pet type to none
- added cooldowns & durations for all the unimplemented hunter skills (so rotation timing is correct)
- calculate spell crit (used for trinkets)
- added AP from aspect mastery
- implemented some more trinkets: Grim Toll & Bandit's Insignia
- correctly calculate Beast Within effects
- added t8-2 piece bonus for serpent sting
- fixed aimed shot damage when weapon isn't 2.8
- calculate base armor correctly
- calculate base health correctly
- updated sources/credits block
- fixed hunter T7 2-piece bonus
- better formatting for some hunter stats");
            PNStuff.Add(
            "v2.2.12 (Aug 06, 2009 05:01)",
            @"- Replaced old broken rotation code with priority-based shot rotation
- Use a 2-stage rotation calculator to first get frequencies for uptimes, LAL procs, etc
- Show shot details in tooltips, with frequencies, DPS and reasons shots are skipped
- Deal with chimera shot refreshing serpent & viper stings
- Calculate proper LAL frequencies
- Added debugger for shot rotation table
- Removed all of the old unused shot results from the CharacterCalculation class
- Added 'bad uptime calculation' mode to mirror the spreadsheet
- Correctly calculate rapid fire & black arrow uptimes based on rotation frequency
- Set critProc & useGCD correctly when creating the ShotData objects
- Added ShotData model for Rapid Fire
- Use latency from options in frequency calculations
- Fixed possible divide-by-zero error in uptime calculations
- Added all the unimplemented shots/traps/effects
- Cleaned up the displayed calculations list
- Correctly apply rotation options when choosing a preset rotation
- Allow setting of every pet talent
");
            PNStuff.Add(
            "v2.2.11 (Aug 05, 2009 05:40)",
            @"- Better formatting for haste
- WotLK boss armor is 10643, not 13100
- Show 2dp for haste calculations.
- Rapid fire base haste is 40%, not 50%
- Corrected base mana and hp calculations
- Updated autoshot dps calculations
- Break autoshot dps down into base autoshot and wild quiver
- Removed a couple of unused/redundant regions
- Corrected steady shot damage calculation
- Corrected serpent sting dps
- Fixed aimed shot calculation
- Started refactoring of damage, crit chance and crit damage adjustments
- Corrected calculations for explosive shot
- Corrected calculations for chimera shot
- Corrected calculations for Arcane shot
- Corrected calculations for multi shot, kill shot and black arrow");
            PNStuff.Add(
            "v2.2.10 (Jul 17, 2009 22:27)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.09 (Jul 02, 2009 03:02)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.08 (Jun 30, 2009 11:29)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.07 (Jun 21, 2009 22:54)",
            @"- Updated Hunter Calculations: Approximately 80% complete, gives accurate numbers for dps and stat values. Mork work to come soon
- Rotations in Hunter finished, DPS for both hunter and pet is 95%-98% accurate
- Updates to the Hunter module for the move to Silverlight
- Couple of minor changes as Rawr.Hunter readies for Silverlight
- Took out ammo damage in Hunter for now until we can sort out a null value issue");
            PNStuff.Add(
            "v2.2.06 (Jun 06, 2009 01:34)",
            @"- No commits for this release");
            PNStuff.Add(
            "v2.2.05 (May 25, 2009 16:25)",
@"- First step to get Hunter module caught up to 3.x:
* Adding a txt file to track my todos and give the current state of expected support for the module.
* Added Glyphs based on what I could find on wowhead.
* Adjusted the relative stat value graph to be Agility based.
* Started adding pet talents.
-  	Hunter Pet DPS section updated to 3.1.2
- File missing from previous build, PetSkills.cs now present");
            /*CB_Version.Items.Add("All");
            string[] arr = new string[PNStuff.Keys.Count];
            PNStuff.Keys.CopyTo(arr, 0);
            CB_Version.Items.AddRange(arr);
            CB_Version.SelectedIndex = 0;
            CB_PatchNotes_SelectedIndexChanged(null, null);*/
        }
        private void SetUpOther()
        {
            RTB_Welcome.Text = @"Welcome to Rawr.DPSWarr!

This module is designed for Warriors hoping to fulfill the DPS role in a raid, either as Fury or Arms Specialized.

To begin, assuming you have already imported your character from either Character Profiler or the WoW Armory, select your talent specialization above as either Fury or Arms if it did not do so automatically. If you would like to check your numbers against information from the next patch (at present, this is 3.2.2) you can check that item.

Next, jump over to the Fight Info Tab to set some background rules for the fight you want to measure against.
- Lag is the average Latency reported in your WoW client. Many with broadband connections usually see a value between 100 ms and 200 ms. Those with slower connection types such as dial-up will see much larger numbers.
- Reaction is the average amount of time it takes for you (the player) to react to a button that becomes available. For example, when an opponent dodges and the Overpower ability procs, how long does it take you to process this mentally and command your finger to push the hotkey for Overpower. The WoW client gives 250 ms (1/4 second) allowance for this before your reactions count against you. Most players fall under this 250 ms rule.
NOTE: Lag and Reaction are combined into a single calculable value. Small adjustments to these numbers yield *very* small adjustments in your DPS.
- The Boss Selector is a new method of using defined Presets for your 'Situational' settings. Selecting a specific boss will tailor the Duration, in back time, multiple targets etc to what is necessary for that fight. Please note that as this is a new method, many of the values for the presets still need to be fine-tuned.
- Target Level can be changed from 80 to 83. 83 is the numeric representation of all Raid Bosses (who show themselves as Level '??')
- Target Armor is currently defaulted to 10,643 for all Level 83 Bosses. This is the currently accepted rule of thumb and there is little reason to change off of this.
- Fight Duration is the length of the fight in seconds. A value of 600 is 10 minutes. Most boss fights take 6 minutes (value 360) or less but we left a high upper value for those wanting to see total damage for a greater period of time. The maximum for this box is 20 minutes, just above KT's Enrage Timer, which is a value of 1200.
- The Situational boxes provide the basic situation you will normally be fighting in. The default setting should be all disabled except 'Standing in Back' at 100%. See the Advanced Instructions (the next tab over from this one) for more info on these settings.

Finally, go to the Ability Maintenance Tab and choose the abilities you will be maintaining during your battles. Note that changing one or the other can have serious effects on your total DPS output, and some abilities act differently if you are in different situations. For example, Bladestorm will have a much larger DPS number if there are multiple targets throughout the fight.
NOTE: If you have Flooring active, turn it off unless you really want to see what it does. The methods behind it have not been refined and it is presently not as accurate as having it disabled.";
            RTB_Advanced.Text = @"This section is for advanced users only, most players do not need to concern themselves with these settings.

Since you have gotten your feet wet, looked at your gear, maybe even run an optimization or two, now you must be hungry for more. Fear not, there's plenty more you can tweak with your character.

The Fight Info Tab

We will be adding functionality to maintain damage taken, for survivability and for the additional rage generated from damage taken at some point, but time takes time.

The Situational Boxes on The Fight Info Tab

This tab holds information regarding how often in a fight your toon is in that particular situation. Presently, there are five options with individual settings for each:
- Standing in Back: You spend at least x% of the fight standing behind the target. Mobs are unable to parry or block attacks from behind so the ~13% of attacks that could normally be parried are no longer on the table. If you are not standing behind the mob during for any portion of the fight, Expertise will have additional value due to it preventing Parries.
- Multiple Targets: Your encounter has additional mobs within melee striking distance for x% amount of time. This provides usefulness for abilities such as Cleave, Bladestorm, Whirlwind, Sweeping Strikes to start 'doing their thing' and allowing you to hit the additional targets for a greater overall DPS. Boss fights like Patchwerk however, do not have additional targets. There is a cap for the number of targets placed so that abilities such as Whirlwind do not go for 4 targets worth of damage when there are only 2 targets.
- Moving Targets: Your encounter has a target that moves out of melee striking distance for x% amount of time. This provides usefulness for abilities such as Charge and Intercept and talents which enable these abilities. This also provides effectiveness for Move Speed Enchants like Cat's Swiftness. A good example of this situation is 'Archavon the Stone Watcher' in the 'Vault of Archavon (VoA)'. [Currently this functionality is only active for Arms, not Fury, Charge, etc abilities are not yet modeled]
- Stunning Targets: Your encounter has a target that either stuns just yourself or your entire raid  x times over the fight duration for y milliseconds (1000 = 1 second). This provides usefulness for abilities such as Every man for Himself (Humans) and the talent Iron Will. You can change the values of both boxes by changing one of them. E.g.- Set Percentage to 25% and it will change the seconds box to match and vice-versa. [Currently this functionality is only active for Arms, not Fury]
- Disarming Targets: Your encounter has a target that disarms your characters' weapon periodically in combat. This provides usefulness for things like Titanium Weapon Chain and the talent Weapon Mastery. Most bosses do not do this, but there are several groups of trash (namely in Karazhan) that will disarm players. [Currently, this functionality has not been implemented, though it will be coming soon.]
Additional Situations to manage will be coming soon.

The Ability Maintenance Tab

Select additional abilities to watch how they affect your DPS. Thunder Clap applies a debuff to bosses as do Sunder Armor, Demoralizing Shout, Shattering Throw, etc.";
        }
        private void CB_FAQ_Questions_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            /*try {
                string text = "";
                if (true /*CB_FAQ_Questions.Text == "All"*/
            /*) {
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
} else {
string s = "";//CB_FAQ_Questions.Text;
string a = "invalid";
bool ver = FAQStuff.TryGetValue(s, out a);
text += s + "\r\n";
text += "\r\n";
text += (ver ? a : "An error occurred calling the string");
RTB_FAQ.Text = text;
RTB_FAQ.SelectAll();
//RTB_FAQ.SelectionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
RTB_FAQ.Select(0, RTB_FAQ.Text.IndexOf('\n'));
//RTB_FAQ.SelectionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
}
} catch(Exception ex){
new ErrorBoxDPSWarr("Error in setting the FAQ Item",
ex.Message, "CB_FAQ_Questions_SelectedIndexChanged");
}*/
        }
        private void CB_Version_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            /*string text = "";
            if (CB_Version.Text == "All")
            {
                int Iter = 1;
                text += "== CONTENTS ==" + "\r\n";
                foreach (string s in PNStuff.Keys)
                {
                    text += s + "\r\n";
                    Iter++;
                } Iter = 1;
                text += "\r\n";
                text += "== READ ON ==" + "\r\n";
                foreach (string s in PNStuff.Keys)
                {
                    string a = "invalid";
                    text += s + "\r\n";
                    bool ver = PNStuff.TryGetValue(s, out a);
                    text += (ver ? a : "An error occurred calling the string") + "\r\n";
                    text += "\r\n" + "\r\n";
                    Iter++;
                } Iter = 1;
                RTB_Version.Text = text;
            }
            else
            {
                string s = CB_Version.Text;
                string a = "invalid";
                bool ver = PNStuff.TryGetValue(s, out a);
                text += s + "\r\n";
                text += "\r\n";
                text += (ver ? a : "An error occurred calling the string");
                RTB_Version.Text = text;
                RTB_Version.SelectAll();
                RTB_Version.SelectionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);
                RTB_Version.Select(0, RTB_Version.Text.IndexOf('\n'));
                RTB_Version.SelectionFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            }*/
        }
        //
        public void calcOpts_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CalculationOptionsHunter calcOpts = Character.CalculationOptions as CalculationOptionsHunter;
            // Target Armor/Level
            /*if (!isLoading && CB_TargLvl.SelectedIndex == -1) { CB_TargLvl.SelectedIndex = 0; }
            if (!isLoading && CB_TargArmor.SelectedIndex == -1) { CB_TargArmor.SelectedIndex = 0; }
            // Fix the enables
            LB_InBehindPerc.IsEnabled = calcOpts.InBack;
            CB_InBackPerc.IsEnabled = calcOpts.InBack;
            LB_Max.IsEnabled = calcOpts.MultipleTargets;
            LB_MultiTargsPerc.IsEnabled = calcOpts.MultipleTargets;
            CB_MultiTargsPerc.IsEnabled = calcOpts.MultipleTargets;
            CB_MultiTargsMax.IsEnabled = calcOpts.MultipleTargets;
            NUD_MoveFreq.IsEnabled = calcOpts.MovingTargets;
            NUD_MoveDur.IsEnabled = calcOpts.MovingTargets;
            NUD_StunFreq.IsEnabled = calcOpts.StunningTargets;
            NUD_StunDur.IsEnabled = calcOpts.StunningTargets;
            NUD_FearFreq.IsEnabled = calcOpts.FearingTargets;
            NUD_FearDur.IsEnabled = calcOpts.FearingTargets;
            NUD_RootFreq.IsEnabled = calcOpts.RootingTargets;
            NUD_RootDur.IsEnabled = calcOpts.RootingTargets;
            NUD_DisarmFreq.IsEnabled = calcOpts.DisarmingTargets;
            NUD_DisarmDur.IsEnabled = calcOpts.DisarmingTargets;
            NUD_AoEFreq.IsEnabled = calcOpts.AoETargets;
            NUD_AoEDMG.IsEnabled = calcOpts.AoETargets;
            // Change abilities if stance changes
            if (e.PropertyName == "FuryStance")
            {
                bool Checked = calcOpts.FuryStance;
                // Fury
                CK_M_F_WW.IsChecked = Checked;
                CK_M_F_BS.IsChecked = Checked;
                CK_M_F_BT.IsChecked = Checked;
                // Fury Special
                CK_M_F_DW.IsChecked = calcOpts.M_DeathWish && Checked;
                CK_M_F_RK.IsChecked = calcOpts.M_Recklessness && Checked;
                // Arms
                CK_M_A_BLS.IsChecked = !Checked;
                CK_M_A_MS.IsChecked = !Checked;
                CK_M_A_RD.IsChecked = !Checked;
                CK_M_A_OP.IsChecked = !Checked;
                CK_M_A_TB.IsChecked = !Checked;
                CK_M_A_SD.IsChecked = !Checked;
                CK_M_A_SL.IsChecked = !Checked;
                // Arms Special
                CK_M_A_TH.IsChecked = calcOpts.M_ThunderClap && !Checked;
                CK_M_A_ST.IsChecked = calcOpts.M_ShatteringThrow && !Checked;
                CK_M_A_SW.IsChecked = calcOpts.M_SweepingStrikes && !Checked;
            }*/
            //
            Character.OnCalculationsInvalidated();
        }
    }
}
