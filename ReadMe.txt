Rawr Beta 13.1
--------------
 Welcome to Rawr b13. New in this beta are two major new features, the Optimizer, and the Retribution model, as well as a multitude of fixes and improvements to the app in general, and each model. You'll be able to mark items as available to your character, and then use the Optimizer to find the best possible set of gear from all available items, with a variety of additional optional requirements. The Retribute model, developed by Anarkii, will help all the Retadins out there build their gear set!

Anyway, as usual, thanks very much for testing Rawr. If you can make it crash, great. If it doesn't crash, but does something that looks wrong, great. If some calculations look wrong to you, that's cool too. Anything like that that goes wrong, or anything else that you find weird, or anything that you think would be more useful or easy to use if done differently, *let me know*! Please forgive me for writing so much in the readme, but please: *>*>*>at least read the FAQ below, and the Instructions section at the bottom<*<*<*.

FAQ
---
 Q: I get an error on load, "To run this application you must first install..." or "The application failed to initialize properly (0xc0000135)." How do I fix this?
 A: Install .NET Framework 2.0 from Microsoft. If it still doesn't work, uninstall .NET Framework completely, reinstall .NET Framework 2.0, and try Rawr again. Download link for .NET Framework 2.0 from Microsoft: http://go.microsoft.com/fwlink/?linkid=32168 

 Q: The Bear calculations don't match my character screen. Are they wrong?
 A: Most likely, no. First, the calculations in Rawr are for combat against raid boss mobs, which changes the chances to be missed, to dodge, to be crushed, and to be crit, so you'll see values off by 0.6% from what the character sheet says. Additionall, WoW sometimes rounds stats oddly, when many buffs are involved. I've gone to great lengths to make Rawr as accurate as possible, but it'll still calculate Stamina and Agility off by 1 in about 5 to 10% of cases. That in turn may make your health, dodge, and armor off by a tiny amount as well. Anyway, that's rare, and even when it does happen, the difference is *extremely* negligable, so don't worry about it. If you do find a situation where other calculations are off, or any of those are off significantly, please let me know. I want to ensure Rawr is as accurate as possible.

 Q: There's an item missing! Can you please add [Some Item]?
 A: No, I designed Rawr so that I wouldn't need to update it with new items every time a new tanking item was found. You can add items to it yourself, very fast, and very easily. Look the item up on wowhead or thottbot, and remember the item ID # from the URL on wowhead or thottbot. Goto Tools > Edit Items, click Add, type that item ID # in, hit OK, and *poof*, you'll have the item. Another thing you can do, after loading your character from the Armory, is choose Tools > Load Possible Upgrades from Armory. This feature will take *a while*, like 5+ min, but will download all the items that Rawr and the Armory thinks are potential upgrades for you. It's a good idea to run this a few days after a major content patch. However, the Armory is commonly unstable immediately after a major content patch, so expect errors if you don't wait a few days.
 
 Q: Can you make it, or does Rawr work for [insert class/spec here]?
 A: What you see in Rawr is what's currently developed to release quality. A few more models are in development. If you're a C# developer and would like to begin or help with another model, let me know!

 
Notes on Rawr.Cat
-----------------
 More calculation options are still coming. Toskk's been very busy lately, and is still working on these. As soon as we have a working version of these, we'll release them.

 
Notes on Rawr.Warlock
---------------------
 Rawr.Warlock is looking pretty good, but is missing several bits of functionality (all items which will be fixed in later versions). With these caveats in mind, it works though:

 - It does not take into account on use or on proc effects (trinkets, etc)
 - It does not take into account any set bonus's
 - It does not take into account any source of mana regen other than lifetap
 - ISB model is simplistic - assumes all charges are used by the caster


Notes on Rawr.Mage
------------------
Rawr.Mage is an optimizer. Given the stats, buffs, talents and fight information it will optimize spell selection and cooldown stacking to maximize total damage done.

When you load your character from the armory it will collect all data about gear, enchants and talents. After you do that you should open the Buffs tab and enable all the buffs that are applicable. Last go to the Options tab and set the fight parameters. At the very least select whether you're using Mage Armor or Molten Armor. You can control many characteristics of the fight, involving external mana regen from shadow priest, rate of interrupts, dps uptime, fight duration, whether there is any aoe involved and any real resistances on the boss.

At that point you are ready to check the Stats tab. At the top you have general stats information which should match your in-game paper doll. The most important part is Solution. Here you will see your expected dps and suggested use of spell cycles to achieve it. If you mouse over the * next to Spell Cycles you'll get a breakdown of all spells you should use and with which cooldowns to pair them.

Finally you can use the comparison charts to decide what improvements you can make. Besides the usual gear comparisons you have several mage specific charts. First is the mage armor comparison which will tell you whether to use Mage Armor or Molten Armor. Then you have per talent points comparison which shows you which talents in your build are the most essential and which you could get to improve your performance. Talent spec comparison gives you a quick glance at which talent spec is best suited to your gear and fight characteristics. You can quickly try out a different talent spec by going to Options tab and selecting a talent spec preset from the talent form. Last comparison is item budget comparison which shows you which stats you should generally be seeking on items.

Features:

 - 5 second rule model
 - AOE model
 - Mana Potion vs Destruction Potion tradeoffs
 - Mana Gem vs Flame Cap tradeoffs
 - Evocation vs dps tradeoffs
 - Cooldown stacking optimization 

Known issues:

 - Sometimes you might notice that the cooldowns are stacked in an impossible way. Most of the issues are either edge cases or effects of gradual tradeoffs. If you find something that looks strange let me know and I'll do my best to try to find a fix. 

Notes on Rawr.Moonkin
---------------------
 You may notice some things that aren't quite right with Rawr.Moonkin, but it should be pretty close to the spreadsheets. We'll be making improvements in upcoming versions!

Version History
---------------
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


Files
-----
Attached to this e-mail, you will find a .zip containing the following:

ReadMe.txt - This ReadMe
Rawr.exe - The Rawr application
Source - This folder contains the Visual Studio 2005 C# project for Rawr. This is all of the source code, so that you can build it yourself, and peruse the source code if you are concerned about it containing anything malicious (it doesn't).
ItemCache.xml - This is the file that Rawr saves its item database to. I've included my current one, which contains a fair number of items for each slot.
Images - This folder containts the images of all of the items that Rawr has in its item database. Whenever Rawr needs an item/gem's icon, it'll look here first. If you don't have the correct icon, it'll download it from the Armory. 

After running Rawr, you'll also see:

Rawr.log - This file contains a log of what you downloaded from the armory, and why. If you have any bugs, I may ask for you to send me this log; it'll help me in solving the bug.
BuffCache.xml - This is the file that Rawr saves its buff database to. This should be complete as of the time of writing (2.3.3). If new enchants come out, you can edit the xml and relaunch Rawr.
EnchantCache.xml - Same as BuffCache, but for enchants.

Overview of Rawr
----------------
Rawr is a little windows app, designed to help you design sets of gear for your Bear & Cat. You can play with different items, enchants, and buffs, and see how they affect your tanking stats & dps stats. Based on your current stats, it will also display a graph of the value of known items for a selected slot, displaying a mitigation rating, a survival rating, and an overall rating for Bear, and a dps rating for Cat. The Bear ratings, along with most of the other Bear calculations in the app, were originally based on the fine spreadsheet created by HugeHoss. Most of the Cat calculations are based on Toskk's Cat Calculator and other hard work by Toskk. Huge thanks to Hoss & Toskk for the help with this project. They calculations for both Cat & Bear have grown, since then, and are now as accurate as possible.


How Rawr Handles Items
----------------------
Of paramount importance in an app like this is how it handles items. Nobody wants to type in the stats of all their items, let alone the stats of all the other prospective items for each slot. If you want to customize items, or create new ones (to prepare for test server changes, for example), you still can type in stats, but you don't have to. There are two ways to load Rawr's item database with new items:

First, you can open an armory profile. Use File->Load from Armory..., type in a character name and server, and choose a region if necessary. It will load up and select all of the items used by that armory profile. Second, you can go to the item editor, choose add, and type in just the item id of an item you'd like to add. In both of these cases, the stats about each item is pulled from the Armory, so a web connection is required.

When loading a character from the armory, or starting a new blank character, all buffs are turned off, so be sure to go check off what buffs you typicaly tank with, to ensure you get accurate ratings.


Instructions
------------
There's no installer for Rawr (at least, not yet). Just unzip the zip anywhere you like, and run Rawr.exe. (If you have any concern about Rawr doing anything malicious, the full source code is included for you to review and/or build yourself)

Once you've got it running, you should see a basic character-screen-esque layout of items. All slots will start out blank, so you can either start filling in items, or open an armory profile. You'll probably want to open your own armory profile, so you can get some familiar items. Goto File->Load from Armory..., and type in your character name and server (exactly, and choose a region if necessary), and hit OK. After a few sec, it should load your profile. You can mouse over an item to see the stats for it, and click on an item to get a dropdown of all of the other items available for that slot. It'll be missing your buffs, so fill those out on the main screen. If any items aren't gemmed (mouse over them all and see if any show a socket, a solid square of color, with no gem in it) right click on it, hit edit, and fill in some gems, or goto Tool->Item Editor, then click Fill Sockets, choose some gems you like, and hit OK, OK.

Now that you have your current character fairly well defined, use the item comparison are on the right side of the main window. You can choose a slot and a sort method at the top. The ratings calculated in this graph will update as you make changes to your gear/enchants/buffs, to always be as accurate as possible.



That's about it, let me know how it works (or doesn't) for you! Thanks!
~Astrylian on Kilrogg (soon transferring to Whisperwind), cnervig@hotmail.com


LICENSE
-------

   Copyright 2007 Chadd Nervig, "HugeHoss", and "Toskk"

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.