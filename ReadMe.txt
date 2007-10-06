Rawr Beta 6
------------
Thanks for helping test out Rawr. Since Beta 5, I've had alot of time, but been very busy. Honestly there aren't a ton of new features in Beta 6, but it took me a *ton* of time to get the new tooltips and dropdown menus working properly. Beta 7 should have all the fancy new features I promised, and I really hope to have Beta 7 out much quicker than Beta 6 took. Anyway, as usual, if you can make it crash, great. If it doesn't crash, but does something that looks wrong, great. If some calculations look wrong to you, that's cool too. Anything like that that goes wrong, or anything else that you find weird, or anything that you think would be more useful or easy to use if done differently, *let me know*! Please forgive me for writing so much in the readme, but please: *>*>*>at least read the FAQ below, and the Instructions section at the bottom<*<*<*.

FAQ
---
 Q: I launched Rawr, but I just see a black window with some red text on it. Has it hung?
 A: No, just let it go for a min. On the first launch of Rawr, it will download all the icons for items it knows about, from the Armory. This can take 1-2 minutes, possibly more on slow connections. If anything goes wrong, it'll display an error message. Just be patient, please. :)

 Q: I get an error on load, "To run this application you must first install..." or "The application failed to initialize properly (0xc0000135)." How do I fix this?
 A: Install .NET Framework 2.0 from Microsoft. If it still doesn't work, uninstall .NET Framework completely, reinstall .NET Framework 2.0, and try Rawr again. Download link for .NET Framework 2.0 from Microsoft: http://go.microsoft.com/fwlink/?linkid=32168 

 Q: There's an item missing! Can you please add [Some Item]?
 A: No, I designed Rawr so that I wouldn't need to update it with new items every time a new tanking item was found. You can add items to it yourself, very fast, and very easily. Look the item up on wowhead or thottbot, and remember the item ID # from the URL on wowhead or thottbot. Goto Tools > Edit Items, click Add, type that item ID # in, hit OK, and *poof*, you'll have the item. Another thing you can do, after loading your character from the Armory, is choose Tools > Load Possible Upgrades from Armory. This feature will take *a while*, like 5+ min, but will download all the items that the Armory thinks are potential upgrades for you, and Rawr agrees are potentially upgrades. It's a good idea to run this a few days after a major content patch.
 
 Q: Can you make it, or does Rawr work for Cats, Moonkin, or Trees?
 A: Right now, no, my focus is on Bears. Once I'm fully satisfied with Rawr's support for Bears, I'll probably start on Cats.

Version History
---------------
Beta 6:
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


After running Rawr, you'll see this folder in the Rawr folder as well:

Images - This folder containts the images of all of the items that Rawr has in its item database. Whenever Rawr needs an item/gem's icon, it'll look here first. If you don't have the correct icon, it'll download it from the Armory. Because of that behavior, I haven't included my images folder.

And this file:

Rawr.log - This file contains a log of what you downloaded from the armory, and why. If you have any bugs, I may ask for you to send me this log; it'll help me in solving the bug.

Overview of Rawr
----------------
Rawr is a little windows app, designed to help you design sets of gear for your Bear. You can play with different items, enchants, and buffs, and see how they affect your tanking stats. Based on your current stats, it will also display a graph of the value of known items for a selected slot, displaying a mitigation rating, a survival rating, and an overall rating. These ratings, along with most of the other calculations in the app, are based on the fine spreadsheet created by HugeHoss. Huge thanks to Hoss for the help with this project. 


How Rawr Handles Items
----------------------
Of paramount importance in an app like this is how it handles items. Nobody wants to type in the stats of all their items, let alone the stats of all the other prospective items for each slot. If you want to customize items, or create new ones (to prepare for test server changes, for example), you still can type in stats, but you don't have to. There are two ways to load Rawr's item database with new items:

First, you can open an armory profile. Use File->Load from Armory..., type in a character name and server, and choose a region if necessary. It will load up and select all of the items used by that armory profile. Second, you can go to the item editor, choose add, and type in just the item id of an item you'd like to add. In both of these cases, the stats about each item is pulled from the Armory, so a web connection is required.

When loading a character from the armory, or starting a new blank character, all buffs are turned off, so be sure to go check off what buffs you typicaly tank with, to ensure you get accurate ratings.


Instructions
------------
There's no installer for Rawr (at least, not yet). Just unzip the zip anywhere you like, and run Rawr.exe.

Once you've got it running, you should see a basic character-screen-esque layout of items. All slots will start out blank, so you can either start filling in items, or open an armory profile. You'll probably want to open your own armory profile, so you can get some familiar items. Goto File->Load from Armory..., and type in your character name and server (exactly, and choose a region if necessary), and hit OK. After a few sec, it should load your profile. You can mouse over an item to see the stats for it, and click on an item to get a dropdown of all of the other items available for that slot. It'll be missing your buffs, so fill those out on the main screen. If any items aren't gemmed (mouse over them all and see if any show a socket, a solid square of color, with no gem in it) goto Tool->Item Editor, then click Fill Sockets, choose some gems you like, and hit OK, OK.

Now that you have your current character fairly well defined, use the item comparison are on the right side of the main window. You can choose a slot and a sort method at the top. The ratings calculated in this graph are based on HugeHoss' excellent spreadsheet, and will update as you make changes to your gear/enchants/buffs.


TODO
----
Stuff I need to do...

-Splash Graphic
-Popup Item Editor
-Some way to show all 3 tabs if you have room
-Tooltips on buffs
-Links to Wowhead
-Eventually track more data about items (str, ap, hit, etc)
-Make the level modifier for calculations customizable
-Eventually track dps too



That's about it, let me know how it works (or doesn't) for you! Thanks!
~Astrylian on Kilrogg, cnervig@hotmail.com


LICENSE
-------

   Copyright 2007 Chadd Nervig

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.