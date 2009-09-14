Rawr v2.2.16.0
------------
Welcome to Rawr 2.2.16.0.
   
Recent Changes:
 - Switched back to 3.2 ArPen rating conversion, since 3.2.2 still looks to be a bit out. Will release a new version as soon as 3.2.2 actually comes out.
 - Rawr.Warlock: Crashing fixed! Joy! Sorry that took so long.
 - Rawr.DPSWarr: Fixed a bug where dps values would come up as NaN in certain cituations. Improvements to Boss handling features. Updated default gemming templates.
 - Rawr.Mage: Update PTR mode to latest 3.2.2 mechanics. Fix for resilience relevancy.
 - Rawr.ProtWarr: Added support for 2T9, and a couple more trinkets. Added threat/damage from Deep Wouds
 - Rawr.TankDK: Fix for blood threat numbers being way off.
 - Rawr.DPSDK: Added support for 3.2.2 PTR changes.
 - Rawr.HolyPriest: Added support for 2T9 and 4T9.
 - Rawr.ShadowPriest: Added support for 2T9 and 4T9. Added 3.2.2 changes.
 - Rawr.Elemental: Fix for T9 bonuses.
 
TEASER: Work continues on Rawr v3, the next major version of Rawr, which will have both web-based, and desktop versions, and run natively on both Windows, and OSX Intel. If you'd like to beta test it (especially OSX users), please e-mail me at cnervig@hotmail.com.
 
Instructions
------------
There's no installer for Rawr. Just unzip the zip anywhere you like, that you have full permissions to (that means NOT Program Files on Vista+), and run Rawr.exe. (If you have any concern about Rawr doing anything malicious, the full source code is available at http://www.codeplex.com/Rawr/ for you to review and/or to build yourself)

Once you've got it running, you should see a basic character-screen-like layout of items. All slots will start out blank, so you can either start filling in items, or open an Armory profile. You'll probably want to open your own Armory profile, so you can get some familiar items. Goto File->Load from Armory..., and type in your character name and server (exactly, and choose a region if necessary), and hit OK. After a few sec, it should load your profile. You can mouse over an item to see the stats for it, and click on an item to get a dropdown of all of the other items available for that slot. It'll be missing your buffs, so fill those out on the main screen. If you'd like to edit the gems in an item, right click on it, hit edit, and change the gems.

Now that you have your current character fairly well defined, use the item comparison are on the right side of the main window. You can choose a slot and a sort method at the top. The ratings calculated in this graph will update as you make changes to your gear/enchants/buffs, to always be as accurate as possible.

~Astrylian on Suramar, cnervig@hotmail.com


FAQ
---
 Q: I get an error on load, "To run this application you must first install..." or "The application failed to initialize properly (0xc0000135)." How do I fix this?
 A: Install .NET Framework 2.0 from Microsoft. If it still doesn't work, uninstall .NET Framework completely, reinstall .NET Framework 2.0, and try Rawr again. Download link for .NET Framework 2.0 from Microsoft: http://go.microsoft.com/fwlink/?linkid=32168 

 Q: I get a "Cannot access disposed object." error. How can I fix that?
 A: There's a bug in the .NET Framework 2.0 which causes this on some machines. The only known fix right now is to uninstall it, and then reinstall the latest .NET Framework from Microsoft.

 Q: There's an item missing! Can you please add [Some Item]?
 A: No, Rawr is designed so that we wouldn't need to update it with new items every time a new item was found. You can add items to it yourself, very fast, and very easily. Look the item up on wowhead or thottbot, and remember the item ID # from the URL on wowhead or thottbot. Goto Tools > Edit Items, click Add, type that item ID # in, hit OK, and *poof*, you'll have the item. Another thing you can do, after loading your character from the Armory, is choose Tools > Load Possible Upgrades from Armory. This feature will take *a while*, like 5+ min, but will download all the items that Rawr and the Armory thinks are potential upgrades for you. It's a good idea to run this a few days after a major content patch. However, the Armory is commonly unstable immediately after a major content patch, so expect errors if you don't wait a few days.


Overview of Rawr
----------------
Rawr is a windows app, designed to help you create sets of gear for your WoW character. You can play with different items, enchants, and buffs, and see how they affect your stats and ratings. Based on your current stats, it will also display a graph of the value of known items for a selected slot, including multiple ratings relevant to your class/spec.

How Rawr Handles Items
----------------------
Of paramount importance in an app like this is how it handles items. Nobody wants to type in the stats of all their items, let alone the stats of all the other prospective items for each slot. If you want to customize items, or create new ones (to prepare for test server changes, for example), you still can type in stats, but you don't have to. There are two ways to load Rawr's item database with new items:

First, you can open an armory profile. Use File->Load from Armory..., type in a character name and server, and choose a region if necessary. It will load up and select all of the items used by that armory profile. Second, you can go to the item editor, choose add, and type in just the item id of an item you'd like to add. In both of these cases, the stats about each item is pulled from the Armory, so a web connection is required.

When loading a character from the armory, or starting a new blank character, all buffs are turned off, so be sure to go check off what buffs you typicaly tank with, to ensure you get accurate ratings.

Source Code
-----------
Rawr's source code is freely available at its website, http://www.codeplex.com/Rawr/ .

Rawr on Mac OS X / Linux
------------------------
NOTE: Rawr v3 is currently in development, and will run natively on OSX Intel. Until then...

You can run Rawr using Mono. Mono is a set of libraries that mimic the .NET Framework, which Rawr is built on, allowing it to run on OSX (and Linux). However, unfortunately, it's extremely buggy. If you want to give it a try, please do, it very well may work just fine for you. For most users, running Rawr via some form of emulation (Boot Camp, VMWare Fusion, Parallels, etc) will give you the best results, though. 

That said, I'm doing what I can to make Rawr available to Mac users, and that means officially supporting running Rawr on OSX, via Mono. 

How to Run Rawr on OSX:
 1) Install Mono (http://www.go-mono.com/mono-downloads/download.html) for Mac OS X. 
	1a) NOTE: At time of writing, Mono v2.4 is the latest, however, it's broken; use Mono v2.2 if you have issues with the latest version.
 2) Unzip Rawr. 
 3) Open the Terminal, navigate to where you unzipped Rawr (look for basic guides to the Terminal, if you need help with that), and type 'mono Rawr.exe', and hit enter. That should launch Rawr for you. 
 
Mono has some problems, so I appreciate your patience as I work to try to make Rawr stable under Mono. I strongly suggest saving often for now.

Known Issues:
 - Mono doesn't look perfect. It looks kinda ugly, and you'll see some weird graphical artifacts. I'll try to work around this as best I can, but it's going to take time.
 - Mono has some crashing problems. Things will be running fine, then all of a sudden, the whole app'll close. I can try to work around these bugs in Mono, but it's going to take time. 
 - Tooltips on labels don't work in Mono. I'll see if I can work around this for a later version. At least for stats, you can work around this by using the Copy Character Stats to Clipboard feature.
 - The clipboard doesn't work under Mono. In the mean time, when running on Mono, Copy Character Stats to Clipboard will save the stats as 'stats.txt' in the folder with Rawr.
 - More issues, I'm sure.




OLDER VERSION HISTORY
---------------------
v2.2.15.0
 - Updated rating calculations for 3.2.2 (if you want 3.2.0 calculations, stick with Rawr v2.2.14. We expect 3.2.2 to come out tomorrow, so are releasing this a day early)
 - Major improvements to Armory and Wowhead parsing; only differences now should be actual data differences between the sites.
 - Rawr will now load the faction-specificity of an item from the Armory, and will only show items available to your race's faction. Note that there are still a few ilvl258 items that aren't on the Armory, so there are still a few items in Rawr which don't have faction data, but the vast majority of items do.
 - Fixed disabled offhand items with gems counting toward metagem requirements.
 - Rawr.Cat: Improved handling of multiple temporary ArPen stats.
 - Rawr.Bear: Support for Paragon special effect.
 - Rawr.TankDK: Fix for ArPen rating being relevant.
 - Rawr.DPSWarr: Added stun handling feature. Improved Multi-Target features. Added Survivability calculations. Fixed a few minor bugs. Updated for 3.2.2. Improved rage calculations. Added preset bosses feature.
 - Rawr.DPSDK: Updated default rotations. Fixed bug with Frost Strike damage.
 - Rawr.ProtPaladin: Fix for attack crit damage calculations.
 - Rawr.Rogue: Fix for Lightning Reflexes calculation.
 - Rawr.Warlock: Fixed crashing bug. Implemented T9 set bonuses. Cleaned up Options tab.
 - Rawr.ProtWarr: Updated base stats to be consistent with 3.2.2. Updated Shield Slam damage, and a few minor proc effects.
 - Rawr.Moonkin: Fix for a damage calculation bug. Added/improved support for several trinkets.

v2.2.14.0
 - Improved Optimizer performance.
 - Improved parsing of Wowhead/Armory data (especially for Trial of Crusader items and item sources)
 - Improved Judgement of Wisdom calculations
 - Added feature to be able to manually remove an item from a built upgrade list.
 - Updated several more buffs/enchants for 3.2 changes.
 - Improved handling of several procs, especially new ones from ToC.
 - There are several more charts in the Buffs chart group, for subsets of buffs.
 - Improved the default set of ItemFilters. Note that the Alliance/Horde ToC filters are gone for the moment, but we will add them back as soon as we get data to support them (coming soon from Wowhead).
 - Rawr.Cat: Added support for offsetting trinkets (such as Grim Toll + Mjolnir Runestone).
 - Rawr.Bear: Updated presets on Options tab.
 - Rawr.Mage: Fixes to Flamestrike calculations/rotations. Added 3.2.2 mode.
 - Rawr.ProtPaladin: Updates to dodge/parry calculations for 3.2. Fixed Seal of Vengeance and Shield of Righteousness calculations. Fixed crit chance calculations.
 - Rawr.Hunter: Tons, and tons, and tons, of changes. Should be almost identical calculation logic to the spreadsheet now. 
 - Rawr.Tree: Updated Innervates calculations and talents for 3.2 changes. 
 - Rawr.DPSWarr: Slight tweaks to stat calculations to reflect WoW's rounding oddities. Fixes for several damage calculation issues. Added T9 set bonuses. Fixes to MultiTarget modes. Several glyph/talent updates for 3.2.
 - Rawr.Elemental: Improved Flameshock dot damage calculations. Improved handling of haste in rotations.
 - Rawr.Rogue: Lots of improvements to calculations, especially in talents.
 - Rawr.Moonkin: Fixed interaction of 4T9, Moonfury, Solar Eclipse, Earth and Moon, and Improved Insect Swarm (multiplicative vs additive).
 - Rawr.ProtWarr: Updated Devestate damage for 3.2. Slight fix to armor calculations.
 - Rawr.DPSDK: Added T9 set bonuses, and support for new Sigils. Fix for some crit chance calculations.
 - Rawr.Healadin: Updated default gemming tempaltes and gem handling for 3.2. Added option to not ignore items with spirit/hit.
 - Rawr.TankDK: Updated all calculations for 3.2. Fixed a few slight inaccuracies with health calculations.
 - Rawr.Warlock: Updated several calcultions for 3.2.
 - Rawr.Retribution: Improved support for new Librams and set bonuses.

v2.2.13.0
 - Adjusted base stats for shamans to match 3.2.
 - Tweaks to CharacterProfiler import.
 - Updated the stats of several more buffs and enchants to match 3.2.
 - Rawr.Retribution: Fixed T9 set bonuses. Added Hand of Reckoning modeling. Added info about switching targets and SoV stacking. Adjustment to JoR procing 2x SoR, since that was hotfixed.
 - Rawr.Hunter: Tons and tons of more improvements and additions.
 - Rawr.Moonkin: Fixed T9 set bonuses.
 - Rawr.TankDK: Added BurstTime and ReactionTime values. Fixes for expertise rating, and for parry rating on weapons. Added support for more trinkets and sigils.
 - Rawr.Enhance: Fix for fist weapons in export to EnhSim. Added support for Totem of the Elemental Winds
 - Rawr.DPSDK: Fixes for Night of the Dead, Greatness procs, and Unholy Blight damage.
 - Rawr.DPSWarr: Updated base stats, talents, and glyphs for 3.2.
 - Rawr.Healadin: Support for a few more trinkets and librams.
 - Rawr.ProtWarr: Significant updates for 3.2.
 - Rawr.Elemental: Further tweaks to how haste affects rotations.

v2.2.12.0
 - NOTE! Everyone, please do NOT reuse your GemmingTemplates.xml file from a previous version of Rawr, due to all the changes in gems in 3.2.
 - Updated the default item cache with more 3.2 items, and updated data about existing items in 3.2. More data is still showing up on Wowhead/Armory daily, so be sure to try refreshing from Wowhead/Armory if you see any data that looks old/wrong. Also updated some of the enchants changed in 3.2.
 - Rawr.Enhance: Fix for Orcs with Axes. Fixes for default gemming templates.
 - Rawr.Hunter: Tons of more calculation improvements and fixes.
 - Rawr.Mage: Reverted to 3.1 hot streak model, due to hotfix that was just applied.
 - Rawr.DPSDK: Updated for 3.2.
 - Rawr.Warlock: Some more calculation improvements to Haunt and Fire & Brimstone.

v2.2.11.0
 - Updated base stats, stat conversions, and gemming rules for 3.2
 - Fixed a potential crash in Batch Tools
 - Rawr.Cat: Updated for 3.2.
 - Rawr.Mage: Updated for 3.2. Fix for FFB threat multiplier. Improved Hot Streak modeling.
 - Rawr.Retribution: Updated for 3.2.
 - Rawr.Enhance: Added support for Orc axe racial. Fix for weapon speed charts.
 - Rawr.DPSWarr: Rearranged the options panel. Fixed several calculation bugs/improvements.
 - Rawr.Warlock: Fixed several calculation bugs/improvements.
 - Rawr.RestoSham: Fixed several calculation bugs/improvements.
 - Rawr.Hunter: Fixed many calculation bugs/improvements.
 - Rawr.Moonkin: Now properly counts lost GCDs due to FF/Starfall/Treants.
 - Rawr.Elemental: Improved handling of special effects. Fixed several calculation bugs/improvements.
 - Rawr.Healadin: Updated for 3.2.
 - Rawr.RestoSham: Updated for 3.2.
 - Rawr.Tree: Updated for 3.2.

v2.2.10.0
 - Rawr.Tree: Added Survival as a 3rd rating
 - Rawr.Retribution: Slight fix to partial resist calculations
 - Rawr.Elemental: Fixed haste, damage, and talent calculations, implemented glyphs
 - Rawr.DPSWarr: Fixed broken Arms, Overpower, Deep Wounds, and Latency calculations, and fixed a crash. Now uses a Priority Queue for all specs. Improved stat display. 
 - Rawr.RestoSham: Improvements to calculations
 - Rawr.Healadin: Minor fix for crit rate calculations
 - Rawr.Rogue: Fixed several talent calculations
 - Rawr.DPSDK: Fixed crash, and minor calculation improvements
 - Rawr.Mage: Fixed special effect bonus haste/crit multipliers, and for Innervate
 
v2.2.9.0
 - Now supports the new armory data format Blizz just posted. Also fixed several Armory parsing errors
 - Rawr.Mage: Fix for procs affecting per-spell damage. Split ignite damage into its own line in the spell damage breakdown.
 - Rawr.DPSWarr: Fixed bug in Maintaining Debuffs and added Furry support. Many improvements to Arms calculations
 - Rawr.Healadin: Fix for Judgement GCD time.

v2.2.8.0
 - Improvements for handling locale-specific item names
 - Reenabled loading possible upgrades from Wowhead PTR
 - Fixed some bugs with loading items from Wowhead/Armory. NOTE: Armory *still* isn't returning socket bonus data, however if an item already has a socket bonus (ie, from Wowhead), reloading it from Armory will preserve the existing socket bonus
 - Rawr.Cat: Improved the calculation of Idol of the Corruptor
 - Rawr.Moonkin: Added a 3.2 mode, improved performance
 - Rawr.Retribution: Added a 3.2 mode
 - Rawr.RestoSham: Implemented all resto relics with special effects
 - Rawr.TankDK: Many calculation fixes and improvements
 - Rawr.DPSWarr: Fix for a wide variety of calculation issues
 - Rawr.Enhance: Improved GCD conflict calculations
 - Rawr.DPSDK: Added a 3.2 mode
 - Rawr.Mage: Added 3.2 mode
 - Rawr.Tree: Added 3.2 mode
 - Rawr.Healadin: Fix for minor rating calculation bug

v2.2.7.0
 - Reordered/revised alot of things in the readme
 - Improvements to alternate locale handling
 - Rawr.Cat: Improvements to Ferocious Bite and HighestStat calculations
 - Rawr.Bear: Fix for HighrstStat calculations
 - Rawr.Enhance: Lots of accuracy improvements all around 
 - Rawr.DPSWarr: Lots of accuracy improvements all around
 - Rawr.DPSDK: Lots of accuracy improvements all around
 - Rawr.TankDL: Initial pass at fixing all the calculations
 - Rawr.Hunter: Lots of accuracy improvements all around, but still a work in progress
 - Rawr.Healadin: Minor fix for burst ratings from procs. Initial implementation of 3.2 mode
 - Rawr.Retribution: Initial implementation of 3.2 mode

v2.2.6.0
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all 
the 3.1 modeling changes.
 - Fix for startup crash, also improved startup performance.
 - Several new advanced options in the Optimizer and Batch Tools. Fix for Optimizer crashes.
 - Rawr.Cat: Fix for FB damage calculations.
 - Rawr.Hunter: Significant improvements all over.
 - Rawr.Rogue: Further progress.
 - Rawr.Mage: Correction to DoT overlap calculations.
 - Rawr.DPSWarr: Further improvements to a variety of formulae and features.
 - Rawr.Moonkin: Fix for 4T8 proc rate.
 - Rawr.Healadin: Improvements to proc values. Fix for some base stats
 - Rawr.Retribution: Improvements to proc values. Fix for a couple talent calculations.
 - Rawr.ProtWarr: Improvements to proc values.
 - Rawr.RestoSham: Updates to glyphs, and added healing stream calculations.
 - Rawr.Elemental: Fix for haste valuing from buffs.
 - Rawr.HolyPriest: Haste fixes and tweaks.
 - Rawr.ShadowPriest: Haste fixes.
 
v2.2.6.0
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all 
the 3.1 modeling changes.
 - Fix for startup crash, also improved startup performance.
 - Several new advanced options in the Optimizer and Batch Tools. Fix for Optimizer crashes.
 - Rawr.Cat: Fix for FB damage calculations.
 - Rawr.Hunter: Significant improvements all over.
 - Rawr.Rogue: Further progress.
 - Rawr.Mage: Correction to DoT overlap calculations.
 - Rawr.DPSWarr: Further improvements to a variety of formulae and features.
 - Rawr.Moonkin: Fix for 4T8 proc rate.
 - Rawr.Healadin: Improvements to proc values. Fix for some base stats
 - Rawr.Retribution: Improvements to proc values. Fix for a couple talent calculations.
 - Rawr.ProtWarr: Improvements to proc values.
 - Rawr.RestoSham: Updates to glyphs, and added healing stream calculations.
 - Rawr.Elemental: Fix for haste valuing from buffs.
 - Rawr.HolyPriest: Haste fixes and tweaks.
 - Rawr.ShadowPriest: Haste fixes.

v2.2.5.0
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all 
the 3.1 modeling changes.
 - Capped ArPen from Rating at 100%
 - Improvements to the Wowhead parser, especially around Sigils
 - Improvements to the Optimizer's handling of gems
 - Enhanced filtering! You can now have subfilters to filters, and there are now default filters for Ulduar gear, broken down by raid size, difficulty, and boss.
 - There's a new Optimizer Results dialog. It's still a bit buggy at the moment, but we intend to improve it substantially as we get time. Please give us feedback on this new feature!
 - Rawr.Rogue: Further work on rogue support, but this model is still largely incomplete
 - Rawr.Mage: Better frost cycle handling
 - Rawr.DPSDK: Massive improvements to logic, across the board
 - Rawr.DPSWarr: Fix for agility miscalculation, better proc handling, better arms/fury rotation handling
 - Rawr.Enhance: Minor calculation improvements
 - Rawr.Moonkin: Fix for some proc handling
 - Rawr.HolyPriest: Calculation tweaks
 - Rawr.ShadowPriest: Improved haste calculations, and proc calculations
 - Rawr.Hunter: Starting to get up to date! Still largely incomplete, but progress is being made!
 - Rawr.Tree: Improved rotations, set bonuses.


v2.2.4.0
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all the 3.1 modeling changes.
 - Fix for crashing bug when editing gemming templates and custom gemmings
 - Fix for the PPMs of several weapon enchants
 - Rawr.Retribution: Updated to 3.1 boss armor values
 - Rawr.Cat: Fix for stacking of temporary ArPen effects, with different uptimes.
 - Rawr.HolyPriest: Better support for a few trinkets.

v2.2.3.0
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all the 3.1 modeling changes.
 - Batch Optimizer supports using multiple models
 - Optimizer doesn't report changes where it just swapped ring or trinket slots.
 - Rawr.Cat: Fixes and improvements to rotations
 - Rawr.Enhance: Lots of fixes for calculations
 - Rawr.ProtPaladin: Support for more procs and effects
 - Rawr.Retribution: Added some optimizable values, support for more procs and effects, and a Consecration effectiveness option
 - Rawr.Healadin: Added some optimizable values
 - Rawr.Warlock: Support for glyphs, and fix for Supression double dipping, lots of haste fixes
 - Rawr.DPSWarr: Many calculation improvements
 - Rawr.HolyPriest: Support for 2T8, new default talents
 - Rawr.ShadowPriest: Support for 2T8, gemmings fixed
 - Rawr.Mage: Improvements to the advanced solver, updated 4T8 to 25% chance, and added support for Frost Warding as a mana regen source
 - Rawr.ProtWarr: Added support for Vigilance threat and glyph.
 - Rawr.Moonkin: Updated Spirit->SpellPower conversion for 3.1.2

v2.2.2.0
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all the 3.1 modeling changes.
 - Fix for a couple armory/wowhead parsing bugs (currently, Armory STILL doesn't return socket bonus info; use Wowhead to get socket bonus data)
 - Chart items with negative values will now be displayed properly
 - Rawr.Healadin: Fix for Icewalker not being relevant
 - Rawr.Tree: Added support for spell mana reduction. Added Wild Growth healing. More trinket support
 - Rawr.Mage: Further updates for 3.1 changes
 - Rawr.Enhance: Further calculation improvements
 - Rawr.DPSDK: More 3.1 updates
 - Rawr.ShadowPriest: Updated to 3.1
 - Rawr.HolyPriest: Updated built-in rotations
 - Rawr.Retribution: Better proc support
 - Rawr.Moonkin: Updated set bonuses, glyphs, innervate calculations, added support for more trinkets 
 - Rawr.ProtPaladin: Support for a few more buffs, calculation improvements
 - Rawr.DPSWarr: Added support for Mace Specialization

v2.2.1.0
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all the 3.1 modeling changes.
 - Updated to the new ArPen model for 3.1
 - New Load from Wowhead Filter feature in the Tools menu
 - Fix for a few bugs with the Optimizer
 - Fix for parsing several items
 - Fix for CharacterProfiler support in foreign languages
 - Rawr.Cat: More 3.1 updates
 - Rawr.Bear: More 3.1 updates
 - Rawr.Moonkin: More 3.1 updates
 - Rawr.HolyPriest: More 3.1 updates
 - Rawr.ShadowPriest: More 3.1 updates
 - Rawr.Mage: More 3.1 updates
 - Rawr.EnhSham: More 3.1 updates
 - Rawr.Tree: More 3.1 updates
 - Rawr.Retribution: More 3.1 updates
 - Rawr.ProtPaladin: More 3.1 updates
 - Rawr.DPSWarr: More 3.1 updates
 - Rawr.DPSDK: More 3.1 updates

v2.2.0.9
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all the 3.1 modeling changes.
 - All talent trees and tooltips updated to 3.1
 - Option to display gem names in tooltips
 - Fix for tooltip rendering
 - Rawr.ProtPaladin: calculation fixes
 - Rawr.TankDK: updated for 3.1
 - Rawr.HolyPriest, Rawr.ShadowPriest: glyph updates
 - Rawr.Tree: Nature's Bounty, Revitalize
  
v2.2.0.8
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all the 3.1 modeling changes.
 - Fix for armory parsing, glyph parsing
 - Fix for optimizer enchant filtering
 - Talent tree updates in some models
 - Rawr.DPSWarr: Update Improved Berserker Stance to Str instead of AP.
 - Rawr.Moonkin: More support for special effects.
 - Rawr.EnhSham: Display enhancements.
 - Rawr.RestoSham: Updates for new glyph system.
 - Rawr.Bear: Fix for lacerate always being able to crit.
 - Rawr.Cat: Added special effects handling.

v2.2.0.7
 - Not all models are completely ready for final release. Specifically in some models the trinket effects might be missing. If that is the case please manually edit the items and give them average stats until we make everything work. We have decided that even not being completely ready we should make a release so that you can work with all the 3.1 modeling changes.
 - There have been many many changes since 2.1.9. For a full list of changes check the ReadMe.txt. The following are the most important changes and some recent changes since 2.2.0.b6.
 - Gemming Revamp! Rawr will now automatically handle gems, just as you'd expect. See Help > Gemmings for details.
 - Multithreading! Rawr will now better utilize your processor, resulting in a 40% to 100% speed boost for rendering most charts, and optimizing.
 - Special Effects Revamp! Rawr now uses a new system for describing special effects that most trinkets have. You can edit the effects in the item editor. The old custom stats are still in because not all models have transitioned to the new system, but we will be phasing them out in new versions.
 - Talent Optimizer! You can now use optimizer to optimize talents. Not all models have talent optimization constraints available, so in those cases it might be of limited value since it will otherwise tend to skip nonmodeled but otherwise important talents.
 - Glyphs are now part of talent specs! Glyphs now get saved together with the talent spec and there is a glyph chart available now for all models.
 - Default buffs/glyphs! When loading character from armory the models can now automatically enable common buffs and glyphs.
 - NOTE: Rawr 2.2 is not backwards compatible. Please do not copy data files from previous versions of Rawr. Your existing character files should load into Rawr 2.2 just fine, except you'll have to reselect enchants on your gear and glyphs.
 - Rawr.Healadin: More 3.1 changes.
 - Rawr.EnhSham: More 3.1 changes. Updated EnhSim export.
 - Rawr.Retribution: More 3.1 changes.
 - Rawr.Mage: More 3.1 changes. Performance Improvements.
 - Rawr.ProtPaladin: More 3.1 changes and fixes.
 - Rawr.Cat: More 3.1 changes. Updated combo point model.
 - Rawr.Bear: More 3.1 changes.
 - Rawr.Warlock: More 3.1 changes.
 - Rawr.Tree: More 3.1 changes.
 - Rawr.RestoSham: More 3.1 changes.
 - Rawr.TankDK and Rawr.Hunter are not updated for 3.1
 
v2.2.0b6
 - PLEASE NOTE: This is a beta of Rawr 2.2. It has not received the same level of testing that we normally put into releases, but we're releasing it in its current form, due to the large number of changes. If you do run into bugs, please post them on our Issue Tracker. Please use the current release version, Rawr 2.1.9, if you encounter showstopping bugs in Rawr 2.2.0b6. Thanks!
 - Start Page! There's now a start page when you launch Rawr, which helps you get started. We're still filling in some of the content on this page, but we're looking for lots of feedback on how you like it, and what else you might want on it.
 - Chart Selection Interface! There is now a more streamlined interface for choosing charts!
 - Chart Exports! There's now an Exports menu above the charts, which allow you to export the data in the current chart to either Clipboard, CSV, or Image.
 - Multilingual Support! In the options, you'll find a locale setting. After setting that, items loaded from wowhead will load their foreign names.
 - Food/Flask/Elixirs Optimization! There are now options to optimize food/flask/elixirs in the Optimizer dialog.
 - On the Relative Stat Values chart, there are 3 additional exports: Copy a Pawn string to Clipboard, View weighted upgrades on Wowhead, and View weighted upgrades on Lootrank.
 - You can now load an individual item from PTR Wowhead by pasting in the PTR Wowhead link to it on the Add Item dialog.
 - There's now a setting in the Options dialog to add the source class/spec to buffs on the Buffs tab.
 - Rawr.Tree: More 3.1 changes. More optimizer additional requirements.
 - Rawr.Cat: More 3.1 changes.
 - Rawr.Bear: More 3.1 changes.
 - Rawr.Mage: Added Mana Source/Usage charts. More 3.1 changes. Improved advanced calculations. Incanter's Absorbtion is now (simply) modeled.
 - Rawr.Moonkin: More 3.1 changes.
 - Rawr.RestoSham: Couple bugfixes.
 - Rawr.Retribution: Added Glyph chart. More 3.1 changes.
 - Rawr.Healadin: Added Glyph chart. More 3.1 changes.
 - Rawr.EnhSham: Coupled bugfixes. More 3.1 changes.
 - Rawr.ProtWarr: More 3.1 changes.
 - Rawr.Warlock: More 3.1 changes. Improvements to talent/spell support.

v2.2.0b5
 - PLEASE NOTE: This is a beta of Rawr 2.2. It has not received the same level of testing that we normally put into releases, but we're releasing it in its current form, due to the large number of changes. If you do run into bugs, please post them on our Issue Tracker. Please use the current release version, Rawr 2.1.9, if you encounter showstopping bugs in Rawr 2.2.0b5. Thanks!
 - Fixed a bug where relevant items and gemmings wouldn't be updated immediately upon switching models.
 - Fix for the Direct Upgrades chart being broken in some models.
 - More performance improvements to the Optimizer
 - Added 'Load Possible Upgrades from Wowhead' feature. Check the 'Use PTR Data' item inside of it to load upgrades from the PTR Wowhead, as they're discovered on the PTR.
 - Rawr.Bear: Fix for a minor issue with Savage Defense. Support for more 3.1 changes.
 - Rawr.Cat: Support for more 3.1 changes.
 - Rawr.Moonkin: Support for more 3.1 changes.
 - Rawr.Mage: Support for more 3.1 changes.
 - Rawr.HolyPriest: New Custom Role feature (please test this!)
 - Rawr.Retribution: Support for more 3.1 changes.
 - Rawr.Healadin: Support for more 3.1 changes.
 - Rawr.Tankadin: Support for more 3.1 changes.
 - Rawr.Warlock: Fixes for several more talents, pets, and glyphs.
 - Rawr.RestoSham: Support for more 3.1 changes and some bugfixes.
 - Rawr.Hunter: Fix for a display bug with AP.
 - Rawr.ProtWarr: Support for more 3.1 changes.

v2.2.0b4
 - PLEASE NOTE: This is a beta of Rawr 2.2. It has not received the same level of testing that we normally put into releases, but we're releasing it in its current form, due to the large number of changes. If you do run into bugs, please post them on our Issue Tracker. Please use the current release version, Rawr 2.1.9, if you encounter showstopping bugs in Rawr 2.2.0b4. Thanks!
 - Multithreading! Rawr will now better utilize your processor, resulting in a 40% to 100% speed boost for rendering most charts, and optimizing. There is potential for hangs from this, so please test as much as you can, and report if you can make it hang, along with very explicitly telling us what you were doing when it hung, and including the character file. NOTE: If you encounter frequent hangs in b4, you can turn off Multithreading in the Tools > Options dialog. If you can't get to that dialog before it hangs, you can edit the config file at /Data/Rawr.Base.dll.config. PLEASE report any hangs you experience!
 - Addition to the new dynamic gemming feature: You can now choose to display the Top X gemmings for an item. Check it out on the Tools > Edit Gemming Templates dialog.
 - Fixes for a few crashes and minor bugs.
 - Batch tools now include a batch optimizer.
 - You can now save, load, and export Upgrade Lists.
 - Rawr.Bear: Support for more 3.1 changes, and presets for some options.
 - Rawr.Cat: Support for more 3.1 changes.
 - Rawr.Moonkin: Fix for a few minor bugs. Support for 3.1 changes.
 - Rawr.Tankadin: Improved base stat accuracy, and support for several more librams, set bonuses, and trinkets. Minor fixes to the effects of several stats.
 - Rawr.Enhance: Minor fixes to the effects of several stats. Improved Flametongue calculations. Support for more buffs, trinkets, and totems, and set bonuses. Several bug fixes. Support for 3.1 changes.
 - Rawr.RestoSham: Support for selecting a healing style. Support for more totems. Added more optimizable stats.
 - Rawr.Elemental: Updated to use calculations from Binkenstein's latest spreadsheet. Added support for custom rotations.
 - Rawr.Retribution: Fix for rounding on a few stats. Support for WoW 3.1 changes.
 - Rawr.Tree: Support for WoW 3.1 changes.
 - Rawr.Mage: Improvements to the advanced rotation solver. Now supports hasted Evocations and Power Infusion. Updates to a few racial base stats.
 - Rawr.DPSWarr: Added Mail armor, and fixes for a few racials.
 - Rawr.ProtWarr: Fixes for a few racial base stats.
 - Rawr.Warlock: Support for 3.1 changes, glyphs, and some more stats and talents.

v2.2.0b3
 - b3 is just a fix for b2 being a bad build. Sorry about that.

v2.2.0b2
 - PLEASE NOTE: This is a beta of Rawr 2.2. It has not received the same level of testing that we normally put into releases, but we're releasing it in its current form, due to the large number of changes. If you do run into bugs, please post them on our Issue Tracker. Please use the current release version, Rawr 2.1.9, if you encounter showstopping bugs in Rawr 2.2.0b2. Thanks!
 - Gemming Revamp! Rawr will now automatically handle gems, just as you'd expect. See Help > Gemmings for details. NOTE: Rawr 2.2 is not backawards compatible most of your existing data files. Please do not copy data files from previous versions of Rawr. Your existing character files should load into Rawr 2.2 just fine, except you'll have to reselect enchants on your gear.
 - Armor Penetration has been adjusted, for all Rawr models, to match new 3.1 Armor Penetration mechanics. Note that Rawr assumes the bugs with ArPen calculations on the current PTR are fixed, and ArPenRating is multiplicative with ArPen debuffs.
 - Fix a crash in Optimizer.
 - Rawr.Cat: Support for new 3.1 changes.
 - Rawr.Bear: Support for new 3.1 changes.
 - Rawr.Mage: Major solver changes.
 - Rawr.Elemental: Bug fixes and added more glyphs.
 - Rawr.Tankadin: Bug fixes and new optimizable stats.
 - Rawr.Enhance: Bug fixes and support for many more trinkets and totems.
 - Rawr.Warlock: Major additions and bug fixes.
 - Rawr.Moonkin: Fix for Skull of Gul'dan.
 - Rawr.ProtWarr: Bug fixes.
 - Rawr.RestoSham: Bug fixes.

v2.2.0b1
 - PLEASE NOTE: This is a beta of Rawr 2.2. It has not received the same level of testing that we normally put into releases, but we're releasing it in its current form, due to the large number of changes. If you do run into bugs, please post them on our Issue Tracker. Please use the current release version, Rawr 2.1.9, if you encounter showstopping bugs in Rawr 2.2.0b1. Thanks!
 - Gemming Revamp! Rawr will now automatically handle gems, just as you'd expect. See Help > Gemmings for details. NOTE: Rawr 2.2 is not backawards compatible most of your existing data files. Please do not copy data files from previous versions of Rawr. Your existing character files should load into Rawr 2.2 just fine, except you'll have to reselect enchants on your gear.
 - Rawr.Bear: Support for Savage Defense.
 - Rawr.Cat: Fixes for hit calculations.
 - Rawr.Mage: Support for 3.0.9 changes. Fixes for a few obscure calcultion bugs.
 - Rawr.Moonkin: Brand new spell calculation engine, powered by WrathCalcs.
 - Rawr.ProtWarr: Significant improvements to accuracy. Added additional rating choices, and additional customization to existing rating ratios. See the Options tab for details. Added support for parry hasting
 - Rawr.HolyPriest: Fix for Build Upgrade List, several calculation improvements. Support for 3.1 Mana Regen model.
 - Rawr.ShadowPriest: Fix for Build Upgrade List, several calculation improvements.
 - Rawr.Tree: Overhaul of casting system, should provide much more useful results.
 - Rawr.Elemental: Significant fixes and new features.
 - Rawr.RestoSham: Now includes Activity, Overhealing, and Burst Healing. Many calculation fixes/improvements.
 - Rawr.Enhance: Lots of calculation improvements.
 - Rawr.TankDK: Wide variety of calculation fixes, UI improvements.
 - Rawr.DPSDK: Many calculation fixes and new features.
 - Rawr.Tankadin: Fixes for armor calculations.
 - Rawr.Healadin: Support for 3.0.9 changes.
 - Rawr.Hunter: Fix for several calculation bugs.
 - Rawr.Retribution: Many calculation changes, new features, improvements, and bug fixes.
 - Rawr.Warlock: Initial release. Not fully complete yet, but included in this release of Rawr so that you can see how we're progressing. We still advise using Rawr.Warlock in conjunction with other theorycrafting tools. In particular, pets are not modeled yet, so Demonology specs will be significantly undervalued.

Here's a quick rundown of the status of each model:
   •Rawr.Base: Fully functional for 3.0 & level 80.
   •Rawr.Bear: Fully functional for 3.0 & level 80.
   •Rawr.Cat: Fully functional for 3.0 & level 80.
   •Rawr.DPSDK: Fully functional for 3.0 & level 80.
   •Rawr.DPSWarr: Partially functional for 3.0 & level 80.
   •Rawr.Elemental: Partially functional for 3.0 & level 80.
   •Rawr.Enhance: Partially functional for 3.0 & level 80.
   •Rawr.Healadin: Fully functional for 3.0 & level 80.
   •Rawr.HolyPriest: Fully functional for 3.0 & level 80.
   •Rawr.Hunter: Partially functional for 3.0 & level 80.
   •Rawr.Mage: Fully functional for 3.0 & level 80.
   •Rawr.Moonkin: Fully functional for 3.0 & level 80.
   •Rawr.ProtWarr: Partially functional for 3.0 & level 80.
   •Rawr.RestoSham: Partially functional for 3.0 & level 80.
   •Rawr.Retribution: Fully functional for 3.0 & level 80.
   •Rawr.Rogue: Not functional for 3.0.
   •Rawr.ShadowPriest: Fully functional for 3.0 & level 80.
   •Rawr.TankDK: Partially functional for 3.0 & level 80.
   •Rawr.Tankadin: Fully functional for 3.0 & level 80.
   •Rawr.Tree: Fully functional for 3.0 & level 80.
   •Rawr.Warlock: Partially functional for 3.0 & level 80.
    
    
 As you can see, we still have alot of work ahead of us, but we're actively working on it. If you are an experienced C# dev, a knowledgable theorycrafter, and would like to help out, especially with the models which we haven't begun updating for 3.0, please contact me at cnervig@hotmail.com. Thanks, and look forward to frequent updates!

v2.1.9:
	Rawr is now accepting donations. Please use Tools > Donate, or goto https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=2451163 if you'd like to donate, to help accelerate the development of Rawr and its models. Thank you!
	Updated parsing for a large number of trinkets/gems whose wording changed in WoW 3.0.8.
	Support for (or more accurate support for) a variety of additional trinkets, in many models.
	Fixed the data for a few profession bonuses which changed.
	Updated the tooltips on all talents.
	Tweaked the Relative Stat Values chart a bit to avoid some anomalies with rounding and stats which provide multiple benefits (ie, both Crit and AP from Agi for Cats).
	Rawr.Cat/Bear: Fix for crit chance reduction against bosses, and fix for display of crit chance debuffs.
	Rawr.Tree: Fixes for haste calculations
	Rawr.Mage: Updates for WoW 3.0.8/hotfix changes. Added Scaling vs Spirit chart.
	Rawr.RestoSham: Support for some glyphs, and more customizable healing rotations. Also enabled cloth/leather.
	Rawr.ProtWarr: Major updates. Threat should be accurate, and a variety of minor fixes and additional options
	Rawr.TankDK/DPSDK: Updated for 3.0.8 changes. A wide variety of major fixes and improvements.
	Rawr.HolyPriest/ShadowPriest: Added optimizable values that you can create requirements for, or optimize for.
	Rawr.Elemental: Initial release. Not fully complete yet, but included in this release of Rawr so that you can see how we're progressing. We still advise using Rawr.Elemental in conjunction with other theorycrafting tools.

v2.1.8:
	Fixed a bug in the Optimizer that would stop it from equipping the optimized gearset when any item gemmings to be equipped weren't in the itemcache already.
	Fixed several bugs with Wowhead parsing, and updated the default itemcache with the new 3.0.8 data from Wowhead.
	Added the two new enchants from 3.0.8.
	Rawr.Healadin: Removed profession benefits from Options since they are now all correctly modelled in there respective places. Support for Judgements missing, and effect of Hit rating. Fixed bug that capped HL too high incorrectly. Added support for FoL Glyph. Updated 2T6 and 4T6 bonuses.
	Rawr.HolyPriest: Raid & CoH spam rotation changes for 3.0.8. Major tweak in Holy/Disc Priest mana consumption, should now actually become saturated at a point. Fiddled more with Trinkets.
	Rawr.Tankadin: Adjusted calculations for new Sacred Duty.
	Rawr.Retribution: Fix for loading the saved glyphs.
	Rawr.Tree: New custom chart for Spell Rotations. New feature to choose the ratio of burst vs sustained healing used in Overall ratings. 
	Rawr.Mage: Added Arcane Scorch cycles.
	Rawr.Cat: Added 4T7 calculations.
	Rawr.ProtWarr: Tweak to the Vitality calculations to match WoW's rounding.
	
v2.1.7:
	All models now contain a Relative Stat Values chart, which dynamically shows the value of various common stats to your current character. Some models had charts similar to this, and most of those still remain, in case they show any additional stats that this universal chart doesn't, but they will be removed in the next major version. As before, remember that these are only outputs from Rawr's calculations, not used internally by Rawr. Also, still remember that they will never be 100% accurate, and to always update whatever you use them for, whenver your gear/enchant/buffs change, in order to maintain as close to accurate as you can.
	Fixed a couple more bugs with Wowhead parsing, and made it more resilient to errors (won't break your whole item cache update if it does hit an error).
	Fixed a bug that would clear your available items on reload character from Armory.
	You can now create filters that apply only to gems, or only to gear. (Not used by default filters yet, though)
	Added support for several more trinket proc/effects.
	Equipped items will always be shown in item listings, even if they would otherwise be filtered out.
	Equipped items will now be marked as available, when re/loading from the Armory
	Rawr.Cat/Bear: Adjusted calculations for the new FeralAP system. Weapon DPS is now counted as attack power, and items no longer show innate feral attack power. I've manually updated all feral weapons to have their new DPS values, but the actual stats on the items are still from 3.0.3 (ie, there's still Str on them, instead of plain AP). Please refresh the data on these items, once Wowhead/Armory update to 3.0.8.
	Rawr.Tree: Major improvements all around. Still not fully complete.
	Rawr.RestoSham: Major improvements all around. Still not fully complete.
	Rawr.Retribution: Base miss chance lowered to 8%.
	Rawr.Tankadin: Base miss chance lowered to 8%.
	Rawr.ProtWarr: Block Value formula changed. Updates to armor calculations to account for Base vs Bonus Armor. Base racial stats updated. Support for the Armored to the Teeth talent.
	Rawr.HolyPriest/ShadowPriest: Initial Glyph support.
	Rawr.Mage: Significant updates to 3.0.8 support and to Arcane cycles. Added Rune of Razorice and Lightweave Embroidery. Enhanced hit rating tooltip. Fix for FBScPyro never being chosen when Global Optimizations were on. Added shatter combo cycles for Frost.
	Rawr.DPSWarr: Initial draft of 3.0 version. Not fully complete yet, but included in this release of Rawr so that you can see how we're progressing. We still advise using Rawr.DPSWarr in conjunction with other theorycrafting tools.
	Rawr.TankDK: Fixed expertise calculations.
	Rawr.Rogue/Warlock: Nothing new, yet, but wanted to mention that we have a ton of active development on these models lately, and new versions are coming soon.

v2.1.6:
	Fixes for several Armory/Wowhead parsing errors.
	Fixes for stats on a couple buffs.
	Fix for a crash when choosing No Enchant from the enchant dropdowns.
	Rawr.Hunter/Bear/Cat: Adjusted miss chance to 8%.
	Rawr.TankDK: Initial release. Not fully complete yet, but included in this release of Rawr so that you can see how we're progressing. We still advise using Rawr.TankDK in conjunction with other theorycrafting tools.

v2.1.5: 
	Items now show their item level in their tooltips. 
	Armor and Bonus Armor are now handled separately. 
	Performance and crash fixes to the Optimizer. 
	Added mousewheel support for the charts. 
	Added buffs for Mixology. 
	Added support for Blacksmithing sockets (reload character from armory, or load character from 2.1.4 and remove extra gem from items). 
	Added a context menu item to quickly evaluate the upgrade value of the clicked item.
	Added support for updating the entire item cache from Wowhead.
	Added parsing for Greatness cards.
	Rawr.Bear: Now includes correct calcultions for armor in patch 3.0.8. Fix for damage reduction calculations for different level targets. Added support for Mongoose. Fixed calculations for Idol of Terror. Added support for soft-capping Survivability.
	Rawr.Cat: Fix for damage reduction calculations for different level targets. Added support for Mongoose, Berserking, Trauma, and Mangle from another feral. Fixed calculations for Idol of Terror.
	Rawr.HolyPriest: Added Arcane Torrent for blood elves. Support for more set bonuses.
	Rawr.Mage: Added custom graphs for stats scaling. Supports extra crit rate from encounter effects (ie, Loatheb). Fix for new cycles ignoring flame caps. Support for reconjuring mana gems.
	Rawr.ProtWarr: Adjusted miss rate from 9% to 8%, and dodge rate from 6.5% to 6.4%.
	Rawr.ShadowPriest Added Arcane Torrent for blood elves. Fixed display issue with Misery/Faerie Fire hit. Support for more trinket effects. Support for more set bonuses.
	Rawr.Tankadin: Fix for block rating conversion.
	Rawr.DPSDK: Fixes for a few calculations. Fixes for item relevancy to show DK set items, not paladin ones.
	Rawr.RestoSham: Updated a variety of calculations. Not fully updated yet, but included in this release of Rawr so that you can see how we're progressing. We still advise using Rawr.RestoSham in conjunction with other theorycrafting tools.
	Rawr.Enhance: Updated the default and available target levels from 70-73 to 80-83, and raised the maximum target armor.
		
		
v2.1.4: Added tracking of item levels, and filtering by item level and a few other item properties. Added support for several additional buffs and enchants. Fixed Wowhead and Armory parsing for a few stats. Fixed CharacterProfiler support. ShadowPriest: Fixes for Optimization, and support for a few more buffs. HolyPriest: Fixes for Optimization and Haste calculations. Moonkin: Added support for a couple trinkets, allowed fist weapons, fixes to Starfire glyph calculations, fix for double application of Kings, and added support for user-defined rotations. Healadin: Added Burst Healing rating, support for Divine Favor and Divine Illumination. ProtWarr: Updated with correct base stats for all races at 80. Mage: Added a 3.0.8 calculation mode. Tree: Fixed haste and crit calculations. Cat: Adjusted base miss rate to 8%, added support for total % damage increases. Bear: Fixed a minor bug in DR calculations.

v2.1.3: Improvements to CharacterProfiler support. Can now load items by exact name, as well as load from Armory and fail over to Wowhead if not found. Support for several new set bonuses and proc/use effects. Fixed base stat calculations in Retribution and Tankadin, and a few more calculation fixes in Retribution. Fixes for a few buff/enchant stats.

v2.1.2: Improvements to launch time, and item editor performance. Support for more set bonuses and abnormal item stats. Fix for crash in Priest models. Updates to base stats and rating conversions for several models. Fixes for requirements and optimization of several gems. Added ghost hit for Frostfire bolt in Mage. Fixed the formula for PW: Shield in HolyPriest. Improved calculations for Cat and Bear, and made Cat results more descriptive.

v2.1.1: Improved calculations for level 80 combat for several models (Cat, Bear, HolyPriest, ShadowPriest, Mage, Moonkin, Tree, Healadin). Added Leatherworking and Inscription self-enchants. Fixed some bugs with mass gem replacement and the optimizer.

v2.1: Updated for level 80 content. Removed models which haven't yet been updated for WoW 3.0. Added two brand new models: Enhance and DPSDK! Welcome to Rawr, Enhancement Shamans and DPS Death Knights!

v2.0.0
 - First release version targeting WoW 3.0

Beta 16.1:
 - The beginnings of user-controlled item filtering is in. Let me stress that this is just the beginning. Currently it's hidden away in a menu item (Tools>Refine Relevant Items...), and only supports the most basic filtering by item type (Leather vs Mail, Maces vs Swords, etc.). In the future, we expect to expand upon this greatly, by allowing all kinds of filters (such as by source, by item level, etc), and will be making the UI to control this more intuitive. Please post on our development site, let us know what sort of filters you'd like to be able to create
 - More improvements to the performance and accuracy of the Optimizer
 - The Add Item dialog now accepts whole links to wowhead or thottbot
 - Added Flame Armor Kits, Arcane Armor Kits, and Lesser Arcanums of Resilience to the default enchant list
 - Rawr.Mage:
   - New custom chart to visualize sequence reconstruction
   - Couple fixes for sequence reconstruction, especially around handling threat
 - Rawr.ProtWarr
   - Blessing of Kings now correctly affects Agility

Beta 16:
 - Significantly improved the performance and accuracy of the Optimizer
 - Added support for importing a character from a Character Profiler saved data file
 - Added Sapphiron enchants to the default EnchantCache
 - Added support for loading characters from the CN Armory
 - Fixed a few crashing bugs in the Optimizer
 - Fixed a potential crash when running Rawr under Mono on OSX
 - Rawr.Bear:
   - Adjusted the Parry cap to 13.75% reflect latest testing
 - Rawr.Mage:
   - Improvements to Sequence Reconstruction 
   - Added support for Serpent-Coil Braid
   - Added breakdown of total damage by spells including damage done, % total damage and number of hits
 - Rawr.Tree:
   - Fixed a few crashing bugs
   - Added support for more talents
   - Added support for HT in rotations
 - Rawr.RestoSham:
   - Added Cloth and Leather as relevant item types
 - Rawr.HolyPriest:
   - Added support for Bangle and Blue Dragon
 - Rawr.ProtWarr:
   - Updated the calculations for PPM enchants (like Mongoose)
   - Adjusted the Parry cap to 13.75% reflect latest testing

Beta 15.1:
 - Fixed Resistances in several models
 - Added several resistance enchants, including glyphs, armor kits, shoulder inscription, and cloak enchants
 - Fixed a bug with the cached values in the popup item selector not being reset on character loads
 - Reduced the scale of thoroughness for the Build Upgrade List feature, in order to give results in reasonable times, compared to Optimze
 - Added class restrictions to engineering goggles in armory lookups
 - The Batch features now allows you to choose a different model for each character file
 - Added a significant amount of (relevantly gemmed) items to the default itemcache for the new models
 - Rawr.Bear:
   - Fix for edge conditions with anti-crit and avoidance affecting damage taken
 - Rawr.DPSWarr:
   - Added importing talents, and fixed loading of talents in the Talents window
   - Fixed weapon DPS calculations
 - Rawr.RestoSham:
   - Fixed value of haste
 - Rawr.Healadin:
   - Fixed value of haste
   - Increased the maximum fight duration to 60min from 20min
 - Rawr.HolyPriest:
   - Increased the maximum fight duration to 60min from 20min
   - Fixed some calculations that were slightly off with basic stats
 - Rawr.Warlock:
   - Fixed title on Shadow Priests box in Raid ISB
   - Enabled the Enforce Metagem Requirements checkbox
 - Rawr.Hunter:
   - Added ranged weapon enchants (scopes)
   - Cloth and throwing items are no longer considered relevant
 - Rawr.Tree:
   - Implemented Bangle, Blue Dragon, Prayerbook, and on-use +healing effects
   - Introduced a scaling factor for average/temporary +healing
   - Implemented IED cooldown and fixed a bug in the calculations for it
   - Items with just spellcrit or health are no longer considered relevant
 - Rawr.Mage:
   - Parsing for Vengeance of the Illidari and fix for Shifting Naaru Sliver cooldown
   - Stability and performance improvements for SMP

Beta 15:
 - Added the Tree, RestoSham, DPSWarr (Arms-only for this release), Hunter (BM-only for this release), Tankadin, and HolyPriest models! These are our first versions of these models, so please report any bugs you find with them!
 - Revamp of item availability mechanics: Items can be marked to allow regemming and restrict which enchants are available. Left click on diamond as before marks item to be available withot restrictions (regemming and all enchants), CTRL+click marks the item to be available for this specific gemming. By right clicking on the diamond it is possible to restrict which enchants are available (if there are any restrictions active this is indicated by a dot next to diamond). Optimizer has an option to override whether items that aren't specifically marked for regemming/reenchanting can be regemmed or reenchanted.
 - The optimizer upgrade comparison chart will now display all slots used to achieve the displayed upgrade value in its tooltip, and has a contextual menu item on click to equip all items
 - Significant performance improvements all around for calculations, especially in the Optimizer
 - Fix for properly handling gems and socket bonuses with spell damage and healing, and for display gems with three stats
 - Fixed a bug with encoding for armory lookups, which was breaking some KR character loads
 - Fixed a bug with item selection popups where the highest rating is 0
 - Now handles broken images on the armory gracefully
 - Added Batch Tools. These are still a work-in-progress, and are unstable, but the end goal is to be able to optimize a collection of characters (ie, your guild) at once. Feel free to poke around, but consider this feature incomplete at the moment, and be sure to save before using it.
 - Rawr.Cat
   - Fix for Braxxis' Staff not being relevant
 - Rawr.Bear
   - Fix for Braxxis' Staff not being relevant
   - Fixes to threat generation, and included Swipe rotations
 - Rawr.ProtWarr:
   - Fixed Combat Table and Crushless Mob debuff
 - Rawr.Moonkin:
   - Fix for 4T5 incorrectly applying to Starfire Spam spell rotation
   - Better judgement of when to clip DoTs and when to let them tick off
 - Rawr.Mage:
   - Massive improvements to sequence reconstruction
   - Added non-debuffed AB to spell info display
   - Many more options on the Options tab, in a new UI
   - Fix for IED meta gem having internal cooldown
   - Fixed the base damage of Arcane Missiles
   - Added an option to set a custom spell mix
   - Fix for Fireball-Fire Blast cycle to take advantage of haste in certain ranges
 - Rawr.Warlock:
   - Pretty much rewrote the whole thing. It's much better now. Feedback and bugreports are definitely welcome!
 - Rawr.Retribution:
   - Mongoose and Executioner reworked
   - Armor Mitigation and Haste reworked
   - Minor fixes to Judgement, Exorcism, Crusader Strike, and Consecration calculations
   - Talents and Options will now save and load properly
   - Implemented more accurate WF and SoC calculations
 - Rawr.Healadin:
   - Added Cloth/Leather/Mail to the relevant item types
   
Beta 14.1:
 - Possible fix for the 'Unable to access a disposed object' error. I still haven't been able to reproduce this, so am not sure if this will solve it; please let me know asap if you still encounter this issue.
 - Fix for the optimizer swapping gems around inappropriately when using Known Gemmings Only.
 - Fixed the tab order on the Load Character From Armory dialog.
 - Added temporary buffs for the SSO Tanking neck
 - Fixed the lack of spellcrit on Improved Judgement of the Crusader
 - Added Trueshot Aura, and setbonuses for Primalstrike, Clefthoof, and Fel Leather sets.
 - Made the item selection dropdown perform much better.
 - Rawr.Bear:
   - Fix for crossbows showing up
   - Fix for Unleashed Rage not showing up
   - Added Resists as optimizable values
 - Rawr.Cat:
   - Fix for crossbows showing up
   - Non-caster items/gems/enchants with stamina will now show up as well (ie, stam gems, stam enchants, etc)
   - Added Resists as optimizable values
 - Rawr.Mage:
   - Fix for display issue in spell cycle solution
   - Fix for a bug that would make your dps skyrocket under certain specific circumstances
   - Fix for SMP overrestricting cooldowns
   - Added parsing for Shifting Naaru Sliver and Shattered Sun Pendant of Acumen
   - Options should now only cause refreshes when you leave the field, not on every keypress
   - Improved calculations for partial water elementals
 - Rawr.ProtWarr:
   - Added Unleashed Rage as a buff.
   - Fixed Eternal Earthstorm Diamond gem requirements.
   - Added weapon damage to white attacks and devastates and fixed a bug where white attacks were normalized and fixed so white damage can crit and glance.
   - Added Windfury as a buff and calculate the TPS from windfury (only from the damage, since Rage Per Second isn't being used yet).
   - Renamed Relative Stat Value chart to Item Budget, since thats what it really was doing, and added strength, haste rating, expertise rating, and hit rating to it.
   - Removed the limitation on level 73 targets only, probably still some issues with hit/expertise caps on trash mobs/bosses.
   - Fixed Improved Def Stance for the resist survival points.
   - Changed the default Mitigation Scale to 3750.
   - Changed the order of stats on the character tab, and removed a lot of the useless ones or moved them to tooltips (Ex. Added hit % and moved hit rating to the tooltip for hit %). Now they are: Base Stats, Defensive Stats, Offensive Stats, Resistance, Complex Stats (for the Survival Points, Mitigation Points, Threat Points, and all the Resistance Points)
   - Added a breakdown on threat values for: white tps, shield slam tps, revenge tps, devastate tps, and heroic strike tps to the threat tooltips.
   - Changed the default threat points to be an average of the unlimited and limited threat values.
   - Fixed crit reduction not taking talents into account.
   - Fixed bug where Vitality was being counted twice for the strength bonus in the threat section.
   - Fixed devastate as a normalized attack.
   - Fixed base crit chance, this should match the in game values (except the 0.6% crit you lose because it's a boss).
   - Corrected chance to be parried and dodged to match the values discussed on the EJ forum. Still need testing to confirm boss parry values and to add support for trash mobs. 
 - Rawr.Warlock:
   - Added 4T6 set bonus
   - Added Shadow Weaving buff
 - Rawr.Retribution:
   - Updated with new WF+SoC interaction logic
   - Uptime fields on the Options tab have been replaced with fields for # of uses

Beta 14:
 - Added the ProtWarr and Healadin models! These are our first versions of these models, so please report any bugs you find with them!
 - Mac support! See the readme for details.
 - Major improvements to the intelligence of the Optimizer
 - Added a new feature in the Optimizer: Build Upgrade List. This feature will take longer than the normal Optimizer (so you'll want to run it at a lower thoroughness, most likely), but will produce a chart of how much value you could gain if you had each item that you don't already have available.
 - Added a Direct Upgrades chart, which shows what the biggest upgrades for you would be, considering just direct upgrades, no other gear swaps. For a comprehensive upgrades evaluation, use the new Build Upgrade List feature of the optimizer.
 - Significantly improved performance all across Rawr, especially in the Optimizer
 - Added a Delete Duplicates function. By right clicking on an item or using the Item Editor, you can Delete Duplicates for an item, which will delete all gemmings of the item except for the one you selected, and any that are equipped.
 - Reworked how Rawr stores calculation options. This means that when you load your characters created with previous versions of Rawr, the Options tab will be reset to default, please be sure to fill that out again.
 - Rawr now correctly handles mainhand and offhand enchants, as appropriate by equipped items.
 - Improved the loading performance of the Item Editor on successive loads. The first time you open it, it should be faster than before, and the second+ time should be nearly instant.
 - Fixed a bug that made the Fill Sockets functionality of the Item Editor not work.
 - Fixed a bug that made the chart render as a big red X occasionally.
 - Made some changes that may help with the UI for users of high-DPI resolutions. High DPI is still not supported (as Windows' support for it is retard and broken), but this should help somewhat.
 - Fixed a leak of control handles in the item selector, should improve performance and prevent crashes related to this.
 - Fixed the Armor Penetration values of Imp/Expose Armor.
 - Reloading the current character from the armory will now load enchants and talents, in addition to items.
 - The Item Selector should no longer extend off the screen when there's room available, on very low resolution displays.
 - Added support for loading characters from KR and TW realms.
 - Load Upgrades from Armory will no longer create duplicates.
 - Added some warnings to the Optimizer to help people use it properly (such as pointing out when you've forgotten to mark any choices in a slot as available). These can be disabled in the Options.
 - Rawr.Bear changes:
    - Added Threat rating
    - Made the Defense Rating to Defense Skill conversion stepwise, so that partial Defense Skill don't count. Adjusted the amount needed to be uncrittable to match (previously, Rawr.Bear may have told you that you'd need 1def rating more than you really did, that's fixed by this).
    - Added several new relevant buffs and debuffs 
 - Rawr.Cat changes:
    - Made the calculations of the SSO Exalted Scryer neck accurate.
    - Added a Survivability rating
    - Added several new relevant buffs and debuffs 
 - Rawr.Mage changes:
    - Added value to labels in item budget comparison
    - Added innervate option
    - Added mana tide totem option
    - Added options for optimization requirements based on stamina and resistances
    - Fix for Arctic Winds talent
    - Added dps fragmentation option
    - Added Mind Quickening Gem and Wrath of Cenarius parsing
    - Improvements in performance and stability of LP solver
    - Added sequence reconstruction
    - Added SMP solver
    - Added additional information to spell tooltips and when copying stats to clipboard
    - Added 2T4 set bonus
    - Added survivability rating 
 - Rawr.Retribution changes:
    - Include more customizable buff options
 - Rawr.Moonkin changes:
    - Calculation model overhauled. Should give much more accurate results now.    

Beta 13.1:
 - Fix for several Optimizer crashes. If you can get the Optimizer to crash still, please e-mail me your character file (cnervig@hotmail.com).
 - Fix for Rawr.Mage requiring .NET 3.5.

Beta 13:
 - Rawr now includes a Retribution model, built by Anarkii! Since it's our first Ret release, it may have some bugs, so please report anything you find wrong with it, and welcome to Rawr, Retadins!
 - Added an Optimizer feature. The Optimizer works by looking at different combinations of gear that you have available (mark items as available by clicking the diamond next to the item in the charts), and finding the best possible set of gear. There's lots to see in the Optimizer, so give it a try, and let us know how it works. One known issue with the optimizer: Unique gems may be used multiple times; best to just not mark any unique gems as Available for now.
 - Added a Status dialog that will show the status of all pending network operations (like loading items from the Armory). Note that there's still no way to cancel these operations while they're in-progress, but we hope to have that in a future update, soon.
 - Improved the performance of loading and saving cached items, and performing some common item stat calculations, and fixed several bugs with the item editor.
 - Items with class restrictions will not show up for other classes.
 - Items will now display a Location, of where to get them. The data for this is still vague or non-existent in some cases, since it's loaded from the Armory, but for the most part it should be useful.
 - All DPS models' DPS rating is calculated in terms of plain DPS (as opposed to DPS*100);
 - Downloads from the Armory should be more resilient now, retrying whenever random failures occur, and correctly supporting more types of proxy servers where needed.
 - The current character name and filename will appear in the title bar.
 - Tooltips are now shown for buffs on the buff selection tab.
 - Buffs and Enchants will now combine the built-in lists with what's in your itemcache, so that you can preserve customizations between versions.
 - Rawr.Bear: Added all missing 2.4 items/enchants to the default ItemCache, resist enchants are now correctly counted, there are now debuffs for Sunwell Radiance and mobs that don't crush.
 - Rawr.Mage: Added more AB-AM cycles, now shows a tradeoff coefficient for all spells, improved rotation and trinket optimization, improved performance, now tracks threat, auto selects Mage/Molten armor,
 - Rawr.Moonkin: Removed the Mana Efficiency rating, since it was already contained in the total damage rating, improved the accuracy of several calculations, added set bonuses, updated mechanics for 2.4, now shows several different damage-related calculations similar to the spreadsheet.

Beta 12.1:
 - Fixed a bug that would cause "Input string not in correct format" errors when Rawr was run on copies of Windows for a region where a period is not the decimal separator (ie european countries where it would be "1,23" instead of "1.23").
 - For proxies that filter by user-agent, the user-agent used by Rawr is now customizable in the proxy options.
 - Config settings (like recent files) are now user specific.
 - Fix for occasionally not loading icons immediately even though a network connection is available. Also fixed an error when there was no network connection.
 - Rawr.Mage: Added new AB-AM cycle and new AB cycles with Frostbolt/Scorch filler, added a fix for the item budget comparison, added scrolling on the options screen, added Drums of Battle stacking, 
 - Rawr.Warlock: Fixed issue when altering spell cycle
 
Beta 12:
 - Rawr now includes models for Moonkins, Mages, and Warlocks!
 - Many more stats are tracked about items, for use in new and upcoming models.
 - Fixed a bug in the options for Cat, so that it updates the label for the value of expose weakness.
 - Icons now pull from the more reliable wowarmory.com instead of the old flakey armory.worldofwarcraft.com.
 - You can now safely run multiple copies of Rawr at the same time by duplicating Rawr's folder. Each running copy of Rawr must be in its own folder, with its own cache xml files.
 - Fixed some bugs that would cause Rawr to crash.
 - The Splash screen has finally been redone! It now has a background image, and an icon for each model that is loaded, along with the version #s of each model, Rawr's UI, and Base.
 - Tooltips on the calculations should really last a long time now, for everyone, I hope.
 - The ItemCache should save faster, load faster, and take up alot less space, per item.
 - There's a menu item now, to reload the character data from the Armory for the current character.
 - The Projectile and Projectile Bag slots will now be hidden if they don't apply to the current model.
 - Improved how Rawr handles requests for data from the Armory.
 - When unable to access the Armory will now handle it much more gracefully. If you see blank icons, anywhere, that's probably the case.
 - There's now an options dialog. More will come in there, but for now, there's customizable proxy settings. If you were previously unable to use Rawr due to being behind a proxy that was different from your IE settings, or required different authentication, give this a try.

Beta 11:
 - You can now change between Bear and Cat mode, and any other models, on the fly, using the new Model menu.
 - All stats are now editable in the Item Editor, not just bear stats.
 - Rawr has been updated for the 2.4 PTR, as of 2/15. Most of the new items have been added, and the idols have been changed to their new stats. Note that you shouldn't try to Refresh Item Data on a new item, as it'll just fail to find the item and timeout.
 - Added accurate calculations for all the decent cat idols.
 - There's now a checkbox on the Options tab to enable Metagem requirement detection. If you run into a situation where one item is ranked higher than another, and you think it should be the other way around, see if the first one helps you meet the metagem requirements, and the second one doesn't. Turn that option off if you like handling metagem requirements yourself, and it'll always count the metagem.
 - Buffs now have tooltips in the charts.
 - The most recently used model will be selected at startup. No more dialog asking what model you want to startup in.
 - There's now a list of recently opened files in the File menu, so you can open your different gear sets and characters faster.
 - The model system has been changed so that each model is its own DLL, meaning that models can be updated/added/removed seperately of Rawr itself.
 - Fixed a few bugs on dual-monitor setups.
 - Items can now have weapon properties (min/max damage, speed)
 - In order to prepare Rawr for a wider variety of models, the item type/slot system has been revamped. You'll now notice that there are 2 more buttons on the main form, for Projectile and Projectile Bag, though nothing will fit in them for ferals. Same with the offhand.
 - Added support for all races, though Rawr.Cat and Rawr.Bear only have stats for NE/Tauren, of course. This is in preparation for Rawr being used by other classes.
 - Items with no stats relevant to the current model will be hidden from charts/dropdowns
 - There are now several new buffs for cats: ExposeWeakness (which has a slider on the Options tab to control its AP value), and Bloodlust, Drums of Battle, and Drums of War (which have sliders on the Options tab to control their uptime %).
 - Models can now define their own custom charts, which are now used by Cat to include Combat Table (White), Combat Table (Yellow) and Relative Stat Values, and by Bear to include Combat Table and Relative Stat Values. Note that due to rounding abormalities when working with small values of certain stats that get multiplies, the two Relative Stat Values charts aren't as informative as I'd like, yet; take their values with a large grain of salt, consider them alpha quality for now.
 - You can resize the main form to give you more chart space, and more space for the tab control with calculation display, buffs, options, enchants.

Beta 10:
 - Cat Support! When Rawr opens, you'll be given a choice of loading the Bear of Cat calculation model. Choose Cat to start using Rawr for optimizing your dps! Most of the Cat calculations are based on Toskk's cat calculator. Huge thanks to Toskk for this!
 - Rawr automatically will detect and activate set bonuses as appropriate.
 - Cat stats are now loaded from the armory, in addition to Bear stats. Only stats relevant to your current mode will be visible.
 - Procs and Uses are now averaged out for Cat, but remain off with optional buffs for Bear.
 - The calculation models are now completely modular. Rawr is very extensible now. It will be much easier to add another model now. If anyone's interested in writing Rawr.Tree, or Rawr.Moonkin, or Rawr.Afflliction, or Rawr.Mutilation or any other class/spec, let me know.
 - There's now an Options tab which contains additional options for the calculations, for both Cat and Bear. For Bear, there's only Target Level. There's much more for Cat though (although most of the Cat options aren't done yet). Currently you can change the Target Armor and choose how often you Powershift.
 - The legend on the comparison graph is gone. I hope to re-implement that at some point, but not a high priority.
 - Improved rendering of tooltips slightly.
 - Fixed a bug with the Filter box on the Item Editor.
 - Probably a dozen other little things that I can't remember cause I've been working on this for so long.
 

Beta 9a:
 - Fixed a bug that made the new tooltips disappear really fast
 - Fixed a bug that made Load Upgrades From Armory skip waist items
 - Added better error messages for all Armory interactions, to indicate whether or not any response was received at all, what that response was, and if no response was received, that you should check your firewall/proxy settings first.

Beta 9:
 - Fixed several crashing bugs introduced in Beta 8
 - Buffs and Enchants are now dynamically loaded from xml files in the Rawr folder. While there is no way to edit them in Rawr, yet, you can at least edit buffs and enchants now, by editing the BuffCache.xml and EnchantCache.xml files, and relaunching Rawr.
 - Added mouseovers giving more info for Overall/Mitigation/Survival Points, and Armor Mitigation value.
 - Minor bugfixes and code cleanups in preparation for generalizing Rawr so that many modules can be made, like Rawr.Cat for kitties.

Beta 8:
 - Rawr now records and displays item quality (rare, epic, etc). If you reuse your ItemCache.xml from a previous version, you'll need to refresh the item data for each item you want to see the quality of. Thanks to Wicks for this functionality!
 - Added support for Finger enchants.
 - Gems in the comparison graph now show up highlighted in their color.
 - Left clicking on an item in the comparison graph will popup the context menu for it, not just right clicking.
 - When loading items from the Armory, Rawr will now retry 3 times if it fails. This should solve the errors caused by intermittant Armory problems. Additionally, if it still fails, it won't close Rawr.
 - Fixed some display problems in the item editor. Also, if you add an item with an ID that doesn't exist, it'll offer to create the item as a blank item, for you to fill in.
 - Clarified the tooltip for anticrit when you're uncrittable. (Defense over the cap isn't totally wasted)
 - Attempting to refresh the data of an item with an item id that doesn't really exist will no longer delete that item. It will alert you to the fact that it could not find item data for that item, and revert to the current data.

Beta 7:
 -Added Contextual Menus for items. You can now right click on an item on the main screen, in the popup item selector, to get a menu of things to do on that item:
	-Edit: Opens the item editor to that item.
	-Open in Wowhead: Opens a new web browser window and navigates to the item in wowhead.
	-Refresh Item Data: Refreshes the data about the item from the armory.
	-Equip: Equips the item.
	-Delete: Deletes the item from Rawr's database.
 -Implemented a ton of new temporary buffs. These buffs are typically from trinkets, or other procs, and most can't really be relied upon to base your gear selections around, but it is interesting to see your stats with these activated.
 -There are now 3 different buff comparison graphs, which let you choose which subset of all the buffs to show: All Buffs, All Long-Duration Buffs (No Dual Wield), and Current Buffs.
 -Made the buff selection tab less cramped, by splitting it into categories, and making it scroll.
 -Added a tooltip to the Chance to be Crit % on the main screen, which will display how much you're over, or under the anti-crit cap, in terms of defense rating or resilience.
 -Added a comparison graph for the Combat Table. This will display the chances of each possible combat swing outcome. If you sort by alphabetical, it will be displayed in the correct order of the combat table.
 -Added another comparison graph, which will include all of your currently equipped gear, enchants, and buffs. While not particularly useful in making gear decisions, this is interesting to look at to see where your mitigation and survival come from.
 -May have fixed the problem with tooltips appearing in the wrong spot on computers with multiple monitors.
 -Enchants in the comparison graph will be highlighted if they are the currently selected enchant for that slot.
 -Buffs will now be highlighted in the comparison graph if they are active.
 -Tooltips will now show up for enchants in the comparison graph.
 -Added a Filter box on the item editor, to make finding specific items in the list faster.
 -Fixed a bug where resilience rating wasn't properly detected on items in a few cases, mostly on gems or socket bonuses.
 -Made the comparison graph understand the concept of 'Infinity'. It will no longer bug out and be blank if any item has a mitigation or overall rating of infinity. This can occur when an item pushes you over 100% dodge.
 -Capped dodge at 100-Miss. Functionally, this doesn't really matter, since it doesn't matter how much over 100% your Miss+Dodge is, if it is over 100%, but I made the change so that it the actual chances of each occurance are correct.
 -Fixed the tab order on the item editor.
 -Fixed some display issues with item tooltip content.
 -Cleaned up the code all over the place.
 -Minor bugfixes here and there.

Beta 6a:
 -Fix for editing gems

Beta 6: (private release)
 -Updated a few buffs for Patch 2.2
 -Implemented new ToolTips for items. While not a highly requested change (and functionally there have been no changes), it was necessary in order to implement the next item...
 -Implemented new item selection dropdowns. Instead of clicking on an item slot and getting a dropdown menu, there's now a much more feature rich popup to choose items. I fully intend to flesh this out with more features, but for now you can see what gems are in each item, see the tooltip for it, see the ratings for it, and have the list sorted by ratings/alpha. You can also type a part of the item name in the Filter box, to filter to items containing that text.
 -Added Alphabetical sorting to the graph
 -Fixed a nasty bug with the calculations for Blessing of Kings. Sorry about that. :(
 -Ugh, prolly a few other things I can't remember right now since it's been so long.

Beta 5a:
 -Fix for the Back slot being broken. Oops.

Beta 5:
 -Finally fixed the bug that made items come back after you delete them. Big thanks to Imagy for help with this one. This also happened to fix the bug that prevented you from adding a second copy of an item that you already have an ungemmed copy of.
 -Implemented Sardrimm's calculations for agi and sta to make Rawr calculate your agi and sta from gear even more accurately. Should be exactly correct 95% of the time now, and only off by 1 otherwise.
 -Made the Name/Realm/Region of a character exposed and editable on the main screen.
 -Added a "Load Possible Upgrades from Armory" feature. Character Name/Realm/Region are required to use this feature. Choose Tools > Load Possible Upgrades from Armory, and the loading screen will come back up and Rawr will think for a while. Quite a while, actually. You might want to go get a sandwich or something, it can take like 5+ minutes. If you think it's hung and it's not doing anything, it is, just let it think for a while longer. :)  Eventually, it'll finish downloading and processing a ton of data from the Armory, and *poof*, your dataset will be filled with a multitude of potential upgrades for every slot. I say potential because not all items it finds will be upgrades. Specifically, it will download all items that the Armory suggests as upgrades (see the Find an Upgrade feature in the Armory), fill it with the best gems you know about for the gem slot color, and run Rawr's calculations on it, and save it if its overall rating is at least 80% of your current item's overall rating. 
 -Made Rawr detect whether a copy of Rawr is already running when it launches, and not launch if there's already a copy open. This will prevent all those errors where it doesn't look like it's loading, and a user tries to launch it again.
 -Added some code that should detect when you're behind a proxy server that requires authentication, and pass your default credentials to it if needed. I have no way to test this, so if you've previously reported a Proxy Requires Authentication error to me, please give Beta 5 a try, and let me know if it works for you.
 -Added a FAQ to the readme.
 -Fixed a bug that prevented Rawr from properly detecting Socket Bonuses of resilience.
 -Made a few error messages more user friendly, and describe how to fix it.

Beta 4b:
 -Updated to use the new armory domains, www.wowarmory.com and eu.wowarmory.com. Won't matter unless you want to armory a character on a realm with a space in its name (gg armory redirect double-encoding).

Beta 4a:
 -Fixed a bug with rating calculations for improvable buffs

Beta 4:
 -All calculations in Rawr should be almost perfectly accurate, with 2 exceptions. I haven't gotten the chance to implement the logic to predict how WoW rounds Stamina and Agility, so these numbers may be off by 1, and any calculations based off those may be off by the value of 1 sta/agi. If anyone finds any occurrances of calculations being off by more than this, PLEASE let me know.
 -One calculation change was significant, so I wanted to call it out specifically. The Total Damage Taken calculation (and subsequently Mitigation Points), did not account for crushing blows or crits. Now it does. For example, if in some hypothetical situation, you had 85% dodge+miss, and 0 armor, you'd receive 15% of the mob's attacks, but they'd all be crushes. Previously, Rawr would consider that as 15% of total damage taken. Now it accounts for those being crushes, so it more accurately says that you'll take 22.5% of the raw incoming damage. Compared to the previous version, the value of agility increases slower as you gain agility, until you reach 85% dodge+miss, then the rate of increase goes way up. In practical terms, you'll see your damage taken score go up from the previous version.
 -I tweaked the scale of Mitigation Points a bit, to make it more even with Survival Points. See my post in the US Druid forums, "Theorycrafting: Method for Rating Bear Gear", for details.
 -The Comparison graph now has slot choices for each Enchant slot, and for Buffs. Can't decide between 6stats and 150hp? Or between 4stats and 12sta? A flask or two elixirs? And which elixirs? Check out the graphs!
 -Rawr will now create a Rawr.log file in the same folder as itself. When Rawr is closed, it will write a log of what you loaded from the armory, and *why*. This will help me track down problems where items get deleted, then magically reappear.
 -Massively improved the performance of the calculations. Depending on the calculations being performed, you should experience a big difference. Calculations should go at least 1.4 times faster, most likely 30-40 times faster, even up to 80 times faster in some cases. You probably won't even notice calculation lag anymore.
 -Rawr's 'HossPoints' really are quite different from the overall points that Hugehoss used in his spreadsheet, so to avoid confusion, I've renamed it to Overall Points.
 -Added Elixir of Mastery. Useful in some situations.
 -Added Major Armor to cloak enchant. No, you shouldn't be using this, but I included it so that you could see on the graph just how bad it is, compared to 12agi. The only time 120armor is more valuable than 12agi is when you're mostly naked. I also included Dodge to cloak, so you can see that there's never a time when it's better than 12agi, in any way. 
 -Added a giant sign with huge red letters that [almost] pops out of your monitor and slaps Emposter in the face.
 -Fixed a bug with meta gems being put in meta slots, and completing socket bonuses
 -Added a "Copy Character Stats To Clipboard" item in the Tools menu, which will copy the calcualted character stats, so that you can post it on forums, or save a history of your stats in a text file or something.
 -Added a Mit/Surv legend to the graph
 -Removed the racial base dodge rating from what shows up in the character stats, to avoid confusion. Now you'll just see the dodge rating from gear, just like in WoW
 -Added a Duplicate button on the Item Editor, which will duplicate the currently selected item.
 -More minor bugs, features, and tweaks, that I've probably missed or just forgotten I added. :)

Beta 3:
 -Characters saved with Beta 2 will not be compatible with this version, so please start from loading your armory again.
 -Massively improved the armory loader, to load enchants by IDs, correctly detect which gems you have and download data for those gems, rather than creating a new gem with the detected stats of the gems you're wearing.
 -All character/item/gem/image data is downloaded from the Armory now, rather than partially from the Armory, partially from Allakhazam, and partially made up from scratch.
 -The Gem Editor has been merged into the Item Editor. Gems now appear as their own heading in the list on the left, and there is a new item in the Slot dropdown for each gem color. When you hit Add Item, and type in the ID of a gem, it will not properly download all the stats and data about that gem, unlike before.
 -The Item Comparison graph on the right side of the main window has had some improvements. Mousing over an item name will now display the tooltip for that item, so that you can now distiguish which gem selection it is. Additionally, the currently equipped item is highlighted in green. There are now two new items in the Slot dropdown, Gems and Metas, which display the regular gems and meta gems in your dataset. While ratings for normal items are calculated by taking the difference between the overall ratings of your character with that item equipped, and with that slot empty, calculatings for gems are the difference between your current character ratings, and your character ratings if you had a buff with the stats of that gem.
 -Added the Elixir of Major Fortitude as a potential buff.
 -Fixed a multitude of crashing bugs and made many minor fixes/improvements.

Beta 2b:
 -Fixed a bug where, when loading an armory, items that Rawr had not seen before would be loaded with their sockets empty, instead of whatever gems you have.

Beta 2a:
 -Fixed a bug in how Rawr adds new gems to its library when it first sees them, that would hang your machine. Sorry!
 
Beta 2:
 -Initial Public Release



LICENSE
-------

   Copyright 2008 Chadd Nervig, "HugeHoss", "Toskk", and the Rawr Developers.

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.