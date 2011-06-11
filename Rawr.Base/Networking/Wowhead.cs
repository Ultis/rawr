using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
#if SILVERLIGHT
using System.Windows.Browser;
#endif
using System.Windows.Threading;
using System.Xml.Linq;

namespace Rawr
{
    public class WowheadService
    {
        public WowheadService()
        {
            _webClient = new WebClient();
            _webClient.Encoding = Encoding.UTF8; // wowhead xml pages use UTF8 encoding
            _webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_webClient_DownloadStringCompleted);
            _queueTimer.Tick += new EventHandler(CheckQueueAsync);

            _webClient_nonxml = new WebClient();
            _webClient_nonxml.Encoding = Encoding.UTF8; // wowhead pages use UTF8 encoding
            _webClient_nonxml.DownloadStringCompleted += new DownloadStringCompletedEventHandler(_webClient_nonxml_DownloadStringCompleted);
        }
        static WowheadService()
        {
            switch (Rawr.Properties.GeneralSettings.Default.Locale)
            {
                #region German
                case "de":
                    _pvpTokenMap["20560"] = "Ehrenabzeichen des Alteractals"; 
                    _pvpTokenMap["20559"] = "Ehrenabzeichen des Arathibeckens";
                    _pvpTokenMap["20558"] = "Ehrenabzeichen der Kriegshymnenschlucht";
                    _pvpTokenMap["29024"] = "Ehrenabzeichen vom Auge des Sturms";
                    _pvpTokenMap["37836"] = "Münze der Venture Co.";
                    _pvpTokenMap["42425"] = "Ehrenabzeichen vom Strand der Uralten";
                    _pvpTokenMap["43589"] = "Ehrenabzeichen von Tausendwinter";

                    _vendorTokenMap["44990"] = "Siegel des Champions";
                    _vendorTokenMap["40752"] = "Emblem des Heldentums";
                    _vendorTokenMap["40753"] = "Emblem der Ehre";
                    _vendorTokenMap["45624"] = "Emblem der Eroberung";
                    _vendorTokenMap["47241"] = "Emblem des Triumphs";
                    _vendorTokenMap["47242"] = "Trophäe des Kreuzzugs";
                    _vendorTokenMap["49426"] = "Emblem des Frosts";

                    _vendorTokenMap["62898"] = "Belobigungsabzeichen von Tol Barad";
                    break;
                #endregion
                #region Spanish
                case "es":
                    _pvpTokenMap["20560"] = "Marca de Honor del Valle de Alterac"; 
                    _pvpTokenMap["20559"] = "Marca de Honor de la Cuenca de Arathi";
                    _pvpTokenMap["20558"] = "Marca de Honor de la Garganta Grito de Guerra";
                    _pvpTokenMap["29024"] = "Marca de Honor del Ojo de la Tormenta";
                    _pvpTokenMap["37836"] = "Moneda de Ventura";
                    _pvpTokenMap["42425"] = "Marca de Honor de la Playa de los Ancestros";
                    _pvpTokenMap["43589"] = "Marca de Honor de Conquista del Invierno";

                    _vendorTokenMap["44990"] = "Sello de Campeón";
                    _vendorTokenMap["40752"] = "Emblema de heroísmo";
                    _vendorTokenMap["40753"] = "Emblema de valor";
                    _vendorTokenMap["45624"] = "Emblema de conquista";
                    _vendorTokenMap["47241"] = "Emblema de triunfo";
                    _vendorTokenMap["47242"] = "Trofeo de la Cruzada";
                    _vendorTokenMap["49426"] = "Emblema de escarcha";

                    _vendorTokenMap["62898"] = "Mención de honor de Tol Barad";
                    break;
                #endregion
                #region French
                case "fr":
                    _pvpTokenMap["20560"] = "Marque d'honneur de la vallée d'Alterac"; 
                    _pvpTokenMap["20559"] = "Marque d'honneur du bassin d'Arathi";
                    _pvpTokenMap["20558"] = "Marque d'honneur du goulet des Chanteguerres";
                    _pvpTokenMap["29024"] = "Marque d'honneur de l'Oeil du cyclone";
                    _pvpTokenMap["37836"] = "Pièce de la KapitalRisk";
                    _pvpTokenMap["42425"] = "Marque d'honneur du rivage des Anciens";
                    _pvpTokenMap["43589"] = "Marque d'honneur de Joug-d'hiver";

                    _vendorTokenMap["44990"] = "Sceau de champion";
                    _vendorTokenMap["40752"] = "Emblème d'héroïsme";
                    _vendorTokenMap["40753"] = "Emblème de vaillance";
                    _vendorTokenMap["45624"] = "Emblème de conquête";
                    _vendorTokenMap["47241"] = "Emblème de triomphe";
                    _vendorTokenMap["47242"] = "TTrophée de la croisade";
                    _vendorTokenMap["49426"] = "Emblème de givre";

                    _vendorTokenMap["62898"] = "Recommandation de Tol Barad";
                    break;
                #endregion
                #region Russian
                case "ru":
                    _pvpTokenMap["20560"] = "Почетный знак Альтеракской долины"; 
                    _pvpTokenMap["20559"] = "Почетный знак Низины Арати";
                    _pvpTokenMap["20558"] = "Почетный знак Ущелья Песни Войны";
                    _pvpTokenMap["29024"] = "Почетный знак Ока Бури";
                    _pvpTokenMap["37836"] = "Монета Торговой Компании";
                    _pvpTokenMap["42425"] = "Почетный знак Берега Древних";
                    _pvpTokenMap["43589"] = "Почетный знак Озера Ледяных Оков";

                    _vendorTokenMap["44990"] = "Печать чемпиона";
                    _vendorTokenMap["40752"] = "Эмблема героизма";
                    _vendorTokenMap["40753"] = "Эмблема доблести";
                    _vendorTokenMap["45624"] = "Эмблема завоевания";
                    _vendorTokenMap["47241"] = "Эмблема триумфа";
                    _vendorTokenMap["47242"] = "Трофей Авангарда";
                    _vendorTokenMap["49426"] = "Эмблема льда";

                    _vendorTokenMap["62898"] = "Рекомендательный значок Тол Барада";
                    break;
                #endregion
                default:
                    _pvpTokenMap["20560"] = "Alterac Valley Mark of Honor"; //This item is no longer available within the game.
                    _pvpTokenMap["20559"] = "Arathi Basin Mark of Honor"; //This item is no longer available within the game.
                    _pvpTokenMap["20558"] = "Warsong Gulch Mark of Honor"; //This item is no longer available within the game.
                    _pvpTokenMap["29024"] = "Eye of the Storm Mark of Honor"; //This item is no longer available within the game.
                    _pvpTokenMap["37836"] = "Venture Coin";
                    _pvpTokenMap["42425"] = "Strand of the Ancients Mark of Honor"; //This item is no longer available within the game.
                    _pvpTokenMap["43589"] = "Wintergrasp Mark of Honor";
                    _pvpTokenMap["390"] = "Conquest Points";
                    _pvpTokenMap["392"] = "Honor Points";
                    _pvpTokenMap["391"] = "Tol Barad Commendation";
                    _pvpTokenMap["62898"] = "Tol Barad Commendation";


                    //_vendorTokenMap["44990"] = "Champion's Seal";
                    //_vendorTokenMap["40752"] = "Emblem of Heroism";//This item is no longer available within the game.
                    //_vendorTokenMap["40753"] = "Emblem of Valor";//This item is no longer available within the game.
                    //_vendorTokenMap["45624"] = "Emblem of Conquest";//This item is no longer available within the game.
                    //_vendorTokenMap["47241"] = "Emblem of Triumph";//This item is no longer available within the game.
                    //_vendorTokenMap["47242"] = "Trophy of the Crusade";//This item is no longer available within the game.
                    //_vendorTokenMap["49426"] = "Emblem of Frost";//This item is no longer available within the game.

                    _tokenDropMap["395"] = new TokenDropInfo() { Name = "Justice Points", Boss = "Magatha Silverton", Area = "Stormwind City" };
                    _tokenDropMap["396"] = new TokenDropInfo() { Name = "Valor Points", Boss = "Faldren Tillsdale", Area = "Stormwind City" };
                    _tokenDropMap["241"] = new TokenDropInfo() { Name = "Champion's Seal", };
                    _tokenDropMap["402"] = new TokenDropInfo() { Name = "Chef's Award" };
                    _tokenDropMap[ "81"] = new TokenDropInfo() { Name = "Dalaran Cooking Award" };
                    _tokenDropMap[ "61"] = new TokenDropInfo() { Name = "Dalaran Jewelcrafter's Token" };
                    _tokenDropMap["398"] = new TokenDropInfo() { Name = "Draenei Archaeology Fragment" };
                    _tokenDropMap["384"] = new TokenDropInfo() { Name = "Dwarf Archaeology Fragment" };
                    _tokenDropMap["393"] = new TokenDropInfo() { Name = "Fossil Archaeology Fragment" };
                    _tokenDropMap["361"] = new TokenDropInfo() { Name = "Illustrious Jewelcrafter's Token" };
                    _tokenDropMap["400"] = new TokenDropInfo() { Name = "Nerubian Archaeology Fragment" };
                    _tokenDropMap["394"] = new TokenDropInfo() { Name = "Night Elf Archaeology Fragment" };
                    _tokenDropMap["397"] = new TokenDropInfo() { Name = "Orc Archaeology Fragment" };
                    _tokenDropMap["401"] = new TokenDropInfo() { Name = "Tol'vir Archaeology Fragment" };
                    _tokenDropMap["385"] = new TokenDropInfo() { Name = "Troll Archaeology Fragment" };
                    _tokenDropMap["399"] = new TokenDropInfo() { Name = "Vrykul Archaeology Fragment" };

                    // T11: Paladin, Priest, Warlock
                    _tokenDropMap["63683"] = new TokenDropInfo() { Name = "Helm of the Forlorn Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Nefarian" };
                    _tokenDropMap["64315"] = new TokenDropInfo() { Name = "Mantle of the Forlorn Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Boss = "Cho'gall" };
                    // T11: Rogue, Death Knight, Mage, Druid
                    _tokenDropMap["63682"] = new TokenDropInfo() { Name = "Helm of the Forlorn Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Nefarian" };
                    _tokenDropMap["64314"] = new TokenDropInfo() { Name = "Mantle of the Forlorn Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Boss = "Cho'gall" };
                    // T11: Warrior, Hunter, Shaman
                    _tokenDropMap["63684"] = new TokenDropInfo() { Name = "Helm of the Forlorn Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Nefarian" };
                    _tokenDropMap["64316"] = new TokenDropInfo() { Name = "Mantle of the Forlorn Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Boss = "Cho'gall" };
                    // T11.5
                    _tokenDropMap["66998"] = new TokenDropInfo() { Name = "Essence of the Forlorn", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Container = true, Boss = "Cache of the Broodmother", Heroic = true };
                    // T11.5: Paladin, Priest, Warlock
                    _tokenDropMap["65001"] = new TokenDropInfo() { Name = "Crown of the Forlorn Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Nefarian", Heroic = true };
                    _tokenDropMap["65088"] = new TokenDropInfo() { Name = "Shoulders of the Forlorn Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Boss = "Cho'gall", Heroic = true };
                    _tokenDropMap["67423"] = new TokenDropInfo() { Name = "Chest of the Forlorn Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Boss = "Halfus Wyrmbreaker", Heroic = true };
                    _tokenDropMap["67429"] = new TokenDropInfo() { Name = "Gauntlets of the Forlorn Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Magmaw", Heroic = true };
                    _tokenDropMap["67428"] = new TokenDropInfo() { Name = "Leggings of the Forlorn Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Maloriak", Heroic = true };
                    // T11.5: Rogue, Death Knight, Mage, Druid
                    _tokenDropMap["65002"] = new TokenDropInfo() { Name = "Crown of the Forlorn Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Nefarian", Heroic = true };
                    _tokenDropMap["65089"] = new TokenDropInfo() { Name = "Shoulders of the Forlorn Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Boss = "Cho'gall", Heroic = true };
                    _tokenDropMap["67425"] = new TokenDropInfo() { Name = "Chest of the Forlorn Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Boss = "Halfus Wyrmbreaker", Heroic = true };
                    _tokenDropMap["67431"] = new TokenDropInfo() { Name = "Gauntlets of the Forlorn Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Magmaw", Heroic = true };
                    _tokenDropMap["67426"] = new TokenDropInfo() { Name = "Leggings of the Forlorn Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Maloriak", Heroic = true };
                    // T11.5: Warrior, Hunter, Shaman
                    _tokenDropMap["65000"] = new TokenDropInfo() { Name = "Crown of the Forlorn Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Nefarian", Heroic = true };
                    _tokenDropMap["65087"] = new TokenDropInfo() { Name = "Shoulders of the Forlorn Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Boss = "Cho'gall", Heroic = true };
                    _tokenDropMap["67424"] = new TokenDropInfo() { Name = "Chest of the Forlorn Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight", Boss = "Halfus Wyrmbreaker", Heroic = true };
                    _tokenDropMap["67430"] = new TokenDropInfo() { Name = "Gauntlets of the Forlorn Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Magmaw", Heroic = true };
                    _tokenDropMap["67427"] = new TokenDropInfo() { Name = "Leggings of the Forlorn Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent", Boss = "Maloriak", Heroic = true };

                    // Unknown at this time where each of the tokens drops from
                    // T12: Paladin, Priest, Warlock
                    _tokenDropMap["71675"] = new TokenDropInfo() { Name = "Helm of the Fiery Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Firelands" /*, Boss = "Nefarian"*/ };
                    _tokenDropMap["71681"] = new TokenDropInfo() { Name = "Mantle of the Fiery Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Firelands" /*, Boss = "Cho'gall"*/ };
                    // T12: Rogue, Death Knight, Mage, Druid
                    _tokenDropMap["71668"] = new TokenDropInfo() { Name = "Helm of the Fiery Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Firelands"/*, Boss = "Nefarian"*/ };
                    _tokenDropMap["71674"] = new TokenDropInfo() { Name = "Mantle of the Fiery Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Firelands"/*, Boss = "Cho'gall"*/ };
                    // T12: Warrior, Hunter, Shaman
                    _tokenDropMap["71682"] = new TokenDropInfo() { Name = "Helm of the Fiery Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Firelands"/*, Boss = "Nefarian"*/ };
                    _tokenDropMap["71688"] = new TokenDropInfo() { Name = "Mantle of the Fiery Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Firelands"/*, Boss = "Cho'gall"*/ };
                    // T12.5: Paladin, Priest, Warlock
                    _tokenDropMap["71677"] = new TokenDropInfo() { Name = "Crown of the Fiery Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent"/*, Boss = "Nefarian"*/, Heroic = true };
                    _tokenDropMap["71680"] = new TokenDropInfo() { Name = "Shoulders of the Fiery Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight"/*, Boss = "Cho'gall"*/, Heroic = true };
                    _tokenDropMap["71679"] = new TokenDropInfo() { Name = "Chest of the Fiery Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight"/*, Boss = "Halfus Wyrmbreaker"*/, Heroic = true };
                    _tokenDropMap["71676"] = new TokenDropInfo() { Name = "Gauntlets of the Fiery Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent"/*, Boss = "Magmaw"*/, Heroic = true };
                    _tokenDropMap["71678"] = new TokenDropInfo() { Name = "Leggings of the Fiery Conqueror", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent"/*, Boss = "Maloriak"*/, Heroic = true };
                    // T12.5: Rogue, Death Knight, Mage, Druid
                    _tokenDropMap["71670"] = new TokenDropInfo() { Name = "Crown of the Fiery Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent"/*, Boss = "Nefarian"*/, Heroic = true };
                    _tokenDropMap["71673"] = new TokenDropInfo() { Name = "Shoulders of the Fiery Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight"/*, Boss = "Cho'gall"*/, Heroic = true };
                    _tokenDropMap["71672"] = new TokenDropInfo() { Name = "Chest of the Fiery Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight"/*, Boss = "Halfus Wyrmbreaker"*/, Heroic = true };
                    _tokenDropMap["71669"] = new TokenDropInfo() { Name = "Gauntlets of the Fiery Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent"/*, Boss = "Magmaw"*/, Heroic = true };
                    _tokenDropMap["71671"] = new TokenDropInfo() { Name = "Leggings of the Fiery Vanquisher", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent"/*, Boss = "Maloriak"*/, Heroic = true };
                    // T12.5: Warrior, Hunter, Shaman
                    _tokenDropMap["71684"] = new TokenDropInfo() { Name = "Crown of the Fiery Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent"/*, Boss = "Nefarian"*/, Heroic = true };
                    _tokenDropMap["71687"] = new TokenDropInfo() { Name = "Shoulders of the Fiery Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight"/*, Boss = "Cho'gall"*/, Heroic = true };
                    _tokenDropMap["71686"] = new TokenDropInfo() { Name = "Chest of the Fiery Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "The Bastion of Twilight"/*, Boss = "Halfus Wyrmbreaker"*/, Heroic = true };
                    _tokenDropMap["71683"] = new TokenDropInfo() { Name = "Gauntlets of the Fiery Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent"/*, Boss = "Magmaw"*/, Heroic = true };
                    _tokenDropMap["71685"] = new TokenDropInfo() { Name = "Leggings of the Fiery Protector", Vendor = "Faldren Tillsdale", VendorArea = "Stormwind City", Area = "Blackwing Descent"/*, Boss = "Maloriak"*/, Heroic = true };
                    // T12.5: Non-Tier Items - these drop from all heroic bosses and are used to upgrade non-tier items to their heroic version (similar to Sunwell's Sunmotes)
                    _tokenDropMap["71617"] = new TokenDropInfo() { Name = "Crystallized Firestone", Vendor = "Lurah Wrathvine", VendorArea = "Mount Hyjal", Area = "Firelands", Boss = "Beth'tilac", Heroic = true };



                    // Holiday Satchels are going to be treated as quest rewards
                    // Valentines Day -  Reward is a Heart-Shaped Box http://www.wowhead.com/item=54537
                    _questRewardMap["49715"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Hummel", MinLevel = 80, Party = 5, Source = ItemSource.Container }; // Forever-Lovely Rose
                    _questRewardMap["50741"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Hummel", MinLevel = 80, Party = 5, Source = ItemSource.Container }; // Vile Furnigator's Mask

                    // Midsummer Fire Festival - Reward is a Satchel of Chilled Goods http://www.wowhead.com/item=54536
                    // The cloaks do not drop from the daily loot, instead drops from the Ice Chest that Ahune reveals after killing him http://www.wowhead.com/object=187892
                    _questRewardMap["54806"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Ahune", MinLevel = 80, Party = 5, Source = ItemSource.Container }; // 232 ilvl Frostscythe of Lord Ahune
                    _questRewardMap["69771"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Ahune", MinLevel = 80, Party = 5, Source = ItemSource.Container }; // 353 ilvl Frostscythe of Lord Ahune

                    // Brewfest - Reward is a Keg-Shaped Treasure Chest http://ptr.wowhead.com/item=54535
                    // The trinkets drop from Coren Direbrew himself http://www.wowhead.com/npc=23872
                    _questRewardMap["49120"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Coren Direbrew", MinLevel = 80, Party = 5, Source = ItemSource.Container }; // 200 ilvl Direbrew's Bloody Shanker
                    _questRewardMap["48663"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Coren Direbrew", MinLevel = 80, Party = 5, Source = ItemSource.Container }; // 226 ilvl Tankard O' Terror
                    _questRewardMap["71331"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Coren Direbrew", MinLevel = 85, Party = 5, Source = ItemSource.Container }; // 365 ilvl Direbrew's Bloodied Shanker
                    _questRewardMap["71332"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Coren Direbrew", MinLevel = 85, Party = 5, Source = ItemSource.Container }; // 365 ilvl Tremendous Tankard O' Terror

                    // Headless Horseman - Reward is Loot-Filled Pumpkin http://ptr.wowhead.com/item=54516
                    // Rings drop directly from the Headless Horseman http://www.wowhead.com/npc=23682
                    _questRewardMap["33292"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Headless Horseman", MinLevel = 80, Party = 5, Source = ItemSource.Container }; // Hallowed Helm
                    _questRewardMap["49128"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Headless Horseman", MinLevel = 80, Party = 5, Source = ItemSource.Container }; // 200 ilvl The Horseman's Baleful Blade
                    _questRewardMap["49126"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Headless Horseman", MinLevel = 80, Party = 5, Source = ItemSource.Container }; // 200 ilvl The Horseman's Horrific Helm
                    _questRewardMap["71325"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Headless Horseman", MinLevel = 85, Party = 5, Source = ItemSource.Container }; // 365 ilvl The Horseman's Sinister Saber
                    _questRewardMap["71326"] = new QuestItem() { Area = "Dungeon Finder", Quest = "World Event Dungeon - Headless Horseman", MinLevel = 85, Party = 5, Source = ItemSource.Container }; // 365 ilvl The Horseman's Horrific Helmet
                    break;
            }
        }

        #region Variables
        private const string URL_ITEM = "http://{0}.wowhead.com/item={1}&xml";
        private const string URL_ITEM_NONXML = "http://{0}.wowhead.com/item={1}";
        private WebClient _webClient;
        private WebClient _webClient_nonxml;
        bool UsePTR = false;
        bool needsNewSourceData = true;
        private bool _canceled = false;
        public event EventHandler<EventArgs<Item>> GetItemCompleted;
        public event EventHandler<EventArgs<string>> ProgressChanged;
        private string _progress = "Requesting Item...";
        public string Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                if (ProgressChanged != null)
                {
                    ProgressChanged(this, new EventArgs<string>(value));
                }
            }
        }
        private DispatcherTimer _queueTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(5) };
        private int _lastItemId;
        #endregion

        public void CancelAsync()
        {
            _webClient.CancelAsync();
            _webClient_nonxml.CancelAsync();
            _canceled = true;
        }

        private void CheckQueueAsync(object sender, EventArgs e)
        {
            _queueTimer.Stop();
            if (!_canceled)
            {
                string url = string.Format(URL_ITEM, false/*UsePTR*/ ? "ptr" : "www", _lastItemId);
                _webClient.DownloadStringAsync(new Uri(url));
                this.Progress = "Downloading Item Data...";
            }
        }

        public static void GetItem(int id, Action<Item> callback) { new ItemRequest(id, callback); }
        // This is commented out until request by name can be set up
        //public static void GetItemIdByName(string itemName, Action<int> callback) { new ItemIdRequest(itemName, callback); }
        public void GetItemAsync(int itemId, bool usePTR)
        {
            _lastItemId = itemId;
            UsePTR = usePTR;
            string url = string.Format(URL_ITEM, UsePTR ? "ptr" : "www", _lastItemId);
            _webClient.DownloadStringAsync(new Uri(url));
            this.Progress = "Downloading Item Data...";
        }
        public void GetItemSourceAsync(int itemId, bool usePTR)
        {
            _lastItemId = itemId;
            UsePTR = usePTR;
            string url = string.Format(URL_ITEM_NONXML, UsePTR ? "ptr" : "www", _lastItemId);
            _webClient_nonxml.DownloadStringAsync(new Uri(url));
            this.Progress = "Downloading Item Source Data...";
        }

        private void _webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                XDocument xdoc;
                try
                {
                    using (StringReader sr = new StringReader(e.Result))
                    {
                        xdoc = XDocument.Load(sr);
                    }
                }
                catch (TargetInvocationException /*ex*/) {
                    Progress = "Did not download file correctly";
                    return;
                }

                /*if (xdoc.Root.FirstAttribute.Name == "error")
                {
                    Progress = xdoc.Root.FirstAttribute.Value;
                    CancelAsync();
                }
                else if (xdoc.Root.FirstAttribute.Name == "item")*/
                {
                    Progress = "Parsing Item Data...";
                    BackgroundWorker bwParseItem = new BackgroundWorker();
                    bwParseItem.WorkerReportsProgress = true;
                    bwParseItem.DoWork += new DoWorkEventHandler(bwParseItem_DoWork);
                    bwParseItem.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwParseItem_RunWorkerCompleted);
                    bwParseItem.ProgressChanged += new ProgressChangedEventHandler(bwParse_ProgressChanged);
                    bwParseItem.RunWorkerAsync(xdoc);
                }
            }
        }
        private void _webClient_nonxml_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                string hdoc;
                try
                {
                    using (StringReader sr = new StringReader(e.Result))
                    {
                        hdoc = sr.ReadToEnd();
                    }
                }
                catch (TargetInvocationException /*ex*/)
                {
                    Progress = "Did not download item source file correctly";
                    return;
                }

                /*if (xdoc.Root.FirstAttribute.Name == "error")
                {
                    Progress = xdoc.Root.FirstAttribute.Value;
                    CancelAsync();
                }
                else if (xdoc.Root.FirstAttribute.Name == "item")*/
                {
                    Progress = "Parsing Item Data...";
                    BackgroundWorker bwParseItemSource = new BackgroundWorker();
                    bwParseItemSource.WorkerReportsProgress = false;
                    bwParseItemSource.DoWork += new DoWorkEventHandler(bwParseItemSource_DoWork);
                    //bwParseItemSource.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwParseItemSource_RunWorkerCompleted);
                    //bwParseItemSource.ProgressChanged += new ProgressChangedEventHandler(bwParse_ProgressChanged);
                    bwParseItemSource.RunWorkerAsync(hdoc);
                }
            }
        }

        private void bwParseItem_DoWork(object sender, DoWorkEventArgs e)
        {
            XDocument xdoc = e.Argument as XDocument;
            int id = 0;
            try
            {
                #region Wowhead Parsing
                if (xdoc == null || xdoc.Root.Value.Contains("Item not found!")) { e.Result = null; return; }
                // the id from above can now be a name as well as the item number, so we regrab it from the data wowhead returned
                foreach (XElement node in xdoc.SelectNodes("wowhead/item")) { id = int.Parse(node.Attribute("id").Value); }
                Item item = new Item() { Id = id, Stats = new Stats() };
                e.Result = item;

                string htmlTooltip = string.Empty;
                string json1s = string.Empty;
                string json2s = string.Empty;
                string repSource = string.Empty;
                string repLevel = string.Empty;

                #region Set Initial Data (Name, Quality, Unique, etc) and record the Tooltip, json & jsonequip sections
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/name")) { item.Name = node.Value; }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/quality")) { item.Quality = (ItemQuality)int.Parse(node.Attribute("id").Value); }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/icon")) { item.IconPath = node.Value; }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/htmlTooltip")) { htmlTooltip = node.Value; }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/json")) { json1s = node.Value; }
                foreach (XElement node in xdoc.SelectNodes("wowhead/item/jsonEquip")) { json2s = node.Value; }
                if (htmlTooltip.Contains("Unique")) item.Unique = true;
                #endregion

                // On Load items from Wowhead Filter, we don't want any
                // items that aren't at least Epic quality
                //if (filter && (int)item.Quality < 2) { return null; }

                #region Item Binding
                // Bind status check
                if (htmlTooltip.Contains("Binds when picked up"))       item.Bind = BindsOn.BoP;
                else if (htmlTooltip.Contains("Binds when equipped"))   item.Bind = BindsOn.BoE;
                else if (htmlTooltip.Contains("Binds to account"))      item.Bind = BindsOn.BoA;
                else if (htmlTooltip.Contains("Binds when used"))       item.Bind = BindsOn.BoU;
                #endregion

                Dictionary<string, object> json;

                try {
                    json = JsonParser.Merge(JsonParser.Parse(json1s), JsonParser.Parse(json2s));
                } catch { e.Result = null; return; }

                #region Process json & jsonequip
                object tmp;
                // Pull Faction Info
                //,reqfaction:1073,reqrep:6
                if (json.TryGetValue("reqfaction", out tmp) || json.TryGetValue("requiredFaction", out tmp))
                {
                    repSource = tmp.ToString();
                }
                if (json.TryGetValue("reqrep", out tmp) || json.TryGetValue("requiredRep", out tmp))
                {
                    repLevel = tmp.ToString();
                }
                if (json.TryGetValue("displayid", out tmp)) //A 3d display ID# for each icon
                {
                    item.DisplayId = (int)tmp;
                }
                if (json.TryGetValue("slotbak", out tmp)) //A couple slots actually have two possible slots... ie vests and robes both fit in chest. slotbak distinguishes vests from robes. We don't care for Rawr, so ignored.
                {
                    item.DisplaySlot = (int)tmp; // it is also used for the 3d display slot id
                }
                if (json.TryGetValue("level", out tmp)) //Rawr now handles item levels
                {
                    item.ItemLevel = (int)tmp;
                }
                if (json.TryGetValue("slot", out tmp))
                {
                    int slot = (int)tmp;
                    if (slot != 0)
                    {
                        item.Slot = GetItemSlot(slot);
                    }
                }
                if (json.TryGetValue("classs", out tmp))
                {
                    string c = tmp.ToString();
                    if (json.TryGetValue("subclass", out tmp))
                    {
                        c = c + "." + tmp.ToString();
                    }
                    if (c.StartsWith("1.") || c.StartsWith("12."))
                    {
                    }
                    else if (c.StartsWith("3."))
                    {
                        item.Type = ItemType.None;
                        switch (c)
                        {
                            case "3.0": item.Slot = ItemSlot.Red; break;
                            case "3.1": item.Slot = ItemSlot.Blue; break;
                            case "3.2": item.Slot = ItemSlot.Yellow; break;
                            case "3.3": item.Slot = ItemSlot.Purple; break;
                            case "3.4": item.Slot = ItemSlot.Green; break;
                            case "3.5": item.Slot = ItemSlot.Orange; break;
                            case "3.6": item.Slot = ItemSlot.Meta; break;
                            case "3.8": item.Slot = ItemSlot.Prismatic; break;
                            case "3.10": item.Slot = ItemSlot.Cogwheel; break;
                            case "3.11": item.Slot = ItemSlot.Hydraulic; break; // need to verify 11
                        }
                    }
                    else
                    {
                        item.Type = GetItemType(c);
                    }
                }
                if (json.TryGetValue("speed", out tmp))
                {
                    item.Speed = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("dmgmin1", out tmp) || json.TryGetValue("dmgMin", out tmp))
                {
                    item.MinDamage = (int)Math.Floor(Convert.ToSingle(tmp));
                }
                if (json.TryGetValue("dmgmax1", out tmp) || json.TryGetValue("dmgMax", out tmp))
                {
                    item.MaxDamage = (int)Math.Ceiling(Convert.ToSingle(tmp));
                }
                if (json.TryGetValue("dmgtype1", out tmp) || json.TryGetValue("dmgType", out tmp))
                {
                    item.DamageType = (ItemDamageType)(int)tmp;
                }
                if (json.TryGetValue("armor", out tmp))
                {
                    item.Stats.Armor = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("armorbonus", out tmp))
                {
                    item.Stats.Armor -= Convert.ToSingle(tmp);
                    item.Stats.BonusArmor = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("healthrgn", out tmp) || json.TryGetValue("health5Combat", out tmp))
                {
                    item.Stats.Hp5 += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("manargn", out tmp) || json.TryGetValue("mana5Combat", out tmp))
                {
                    item.Stats.Mp5 += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("health", out tmp))
                {
                    item.Stats.Health += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("agi", out tmp))
                {
                    item.Stats.Agility += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("int", out tmp))
                {
                    item.Stats.Intellect += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("spi", out tmp) || json.TryGetValue("spr", out tmp))
                {
                    item.Stats.Spirit += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("sta", out tmp))
                {
                    item.Stats.Stamina += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("str", out tmp))
                {
                    item.Stats.Strength += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("hastertng", out tmp) || json.TryGetValue("mlehastertng", out tmp) || json.TryGetValue("rgdhastertng", out tmp) || json.TryGetValue("splhastertng", out tmp) || json.TryGetValue("hasteRating", out tmp))
                {
                    item.Stats.HasteRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("mastrtng", out tmp) || json.TryGetValue("masteryRating", out tmp))
                {
                    item.Stats.MasteryRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("splpwr", out tmp) || json.TryGetValue("splheal", out tmp) || json.TryGetValue("spldmg", out tmp) || json.TryGetValue("spellPower", out tmp))
                {
                    item.Stats.SpellPower += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("critstrkrtng", out tmp) || json.TryGetValue("mlecritstrkrtng", out tmp) || json.TryGetValue("rgdcritstrkrtng", out tmp) || json.TryGetValue("splcritstrkrtng", out tmp) || json.TryGetValue("critRating", out tmp))
                {
                    item.Stats.CritRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("firres", out tmp) || json.TryGetValue("fireResistance", out tmp))
                {
                    item.Stats.FireResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("natres", out tmp) || json.TryGetValue("natureResistance", out tmp))
                {
                    item.Stats.NatureResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("frores", out tmp) || json.TryGetValue("frostResistance", out tmp))
                {
                    item.Stats.FrostResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("shares", out tmp) || json.TryGetValue("shadowResistance", out tmp))
                {
                    item.Stats.ShadowResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("arcres", out tmp) || json.TryGetValue("arcaneResistance", out tmp))
                {
                    item.Stats.ArcaneResistance += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("hitrtng", out tmp) || json.TryGetValue("mlehitrtng", out tmp) || json.TryGetValue("rgdhitrtng", out tmp) || json.TryGetValue("splhitrtng", out tmp) || json.TryGetValue("hitRating", out tmp))
                {
                    item.Stats.HitRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("atkpwr", out tmp) || json.TryGetValue("mleatkpwr", out tmp) || json.TryGetValue("attackPower", out tmp))
                {
                    item.Stats.AttackPower += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("rgdatkpwr", out tmp))
                {
                    if (item.Stats.AttackPower != Convert.ToSingle(tmp))
                    {
                        item.Stats.RangedAttackPower = Convert.ToSingle(tmp);
                    }
                }
                if (json.TryGetValue("exprtng", out tmp) || json.TryGetValue("expertiseRating", out tmp))
                {
                    item.Stats.ExpertiseRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("dodgertng", out tmp) || json.TryGetValue("dodgeRating", out tmp))
                {
                    item.Stats.DodgeRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("blockrtng", out tmp) || json.TryGetValue("blockRating", out tmp))
                {
                    item.Stats.BlockRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("socket1", out tmp))
                {
                    item.SocketColor1 = GetSocketType(tmp.ToString());
                }
                if (json.TryGetValue("socket2", out tmp))
                {
                    item.SocketColor2 = GetSocketType(tmp.ToString());
                }
                if (json.TryGetValue("socket3", out tmp))
                {
                    item.SocketColor3 = GetSocketType(tmp.ToString());
                }
                if (json.TryGetValue("socketbonus", out tmp) || json.TryGetValue("socketBonus", out tmp))
                {
                    item.SocketBonus = GetSocketBonus(tmp.ToString());
                }
                if (json.TryGetValue("parryrtng", out tmp) || json.TryGetValue("parryRating", out tmp))
                {
                    item.Stats.ParryRating += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("classes", out tmp))
                {
                    List<string> requiredClasses = new List<string>();
                    int classbitfield = (int)tmp;
                    if ((classbitfield & 1) > 0)
                        requiredClasses.Add("Warrior");
                    if ((classbitfield & 2) > 0)
                        requiredClasses.Add("Paladin");
                    if ((classbitfield & 4) > 0)
                        requiredClasses.Add("Hunter");
                    if ((classbitfield & 8) > 0)
                        requiredClasses.Add("Rogue");
                    if ((classbitfield & 16) > 0)
                        requiredClasses.Add("Priest");
                    if ((classbitfield & 32) > 0)
                        requiredClasses.Add("Death Knight");
                    if ((classbitfield & 64) > 0)
                        requiredClasses.Add("Shaman");
                    if ((classbitfield & 128) > 0)
                        requiredClasses.Add("Mage");
                    if ((classbitfield & 256) > 0)
                        requiredClasses.Add("Warlock");
                    //if ((classbitfield & 512) > 0) ; // Only seems to occur in PvP gear, along with another huge value
                    //    requiredClasses.Add("");
                    if ((classbitfield & 1024) > 0)
                        requiredClasses.Add("Druid");
                    item.RequiredClasses = string.Join("|", requiredClasses.ToArray());
                }
                if (json.TryGetValue("resirtng", out tmp) || json.TryGetValue("resilRating", out tmp))
                {
                    item.Stats.Resilience += Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("splpen", out tmp) || json.TryGetValue("spellPen", out tmp))
                {
                    item.Stats.SpellPenetration = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("mana", out tmp))
                {
                    item.Stats.Mana = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("dmg", out tmp))
                {
                    item.Stats.WeaponDamage = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("frospldmg", out tmp))
                {
                    item.Stats.SpellFrostDamageRating = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("shaspldmg", out tmp))
                {
                    item.Stats.SpellShadowDamageRating = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("firspldmg", out tmp))
                {
                    item.Stats.SpellFireDamageRating = Convert.ToSingle(tmp);
                }
                if (json.TryGetValue("arcspldmg", out tmp))
                {
                    item.Stats.SpellArcaneDamageRating = Convert.ToSingle(tmp);
                }
                #endregion

                // We don't need to process any more data if it's not a slottable item (eg not Gear/Gem)
                if (item.Slot == ItemSlot.None) { e.Result = null; return; }

                #region Item Source
                // NOTE: What we are doing here is just giving it a preliminary source data
                // that is blank but of the correct type if possible
                // Once the item gets added to the database (if tis valid) then we will call
                // source data separately that will do the full source stuff
                // In some cases, we may not need to make the extra call, so added a flag to tell it to NOT do it
                if (json.TryGetValue("source", out tmp)) {
                    object[] sourceArr = (object[])tmp;
                    object[] sourcemoreArr = null;
                    if (json.TryGetValue("sourcemore", out tmp)) { sourcemoreArr = (object[])tmp; }

                    #region We have some Source Data

                    // most mobs that have a vendor bought alternative will give more information through the vendor than the mob
                    // this is especially case for vault of archavon drops
                    int source = (int)sourceArr[0];
                    Dictionary<string, object> sourcemore = null;
                    if (sourcemoreArr != null && sourcemoreArr.Length > 0)
                    {
                        sourcemore = (Dictionary<string, object>)sourcemoreArr[0];
                    }

                    int vendorIndex = Array.IndexOf(sourceArr, 5);
                    if (vendorIndex >= 0)
                    {
                        source = (int)sourceArr[vendorIndex];
                        if (sourcemoreArr != null && sourcemoreArr.Length > vendorIndex)
                        {
                            sourcemore = (Dictionary<string, object>)sourcemoreArr[vendorIndex];
                        }
                    }

                    string n = string.Empty;
                    if (sourcemore != null && sourcemore.TryGetValue("n", out tmp))
                    {
                        n = tmp.ToString();
                    }

                    string itemId = item.Id.ToString();
                    if (source == 2 && sourcemore == null) {
                        // It's a World Drop
                        item.LocationInfo = new ItemLocationList() { WorldDrop.Construct() };
                        // We dont need more source info
                        needsNewSourceData = false;
                    } else if (source == 5 && sourcemore == null) {
                        // Its a Vendor Purchase
                        item.LocationInfo = new ItemLocationList() { VendorItem.Construct() };
                        // We have to call for more source data at this point because all we have is the 5 saying its a Vendor item
                    } else if (source == 5 && sourcemore != null && sourcemore.TryGetValue("z", out tmp)) {
                        // Its a Vendor Purchase
                        ItemLocation vendorItem = VendorItem.Construct();
                        (vendorItem as VendorItem).VendorName = n;
                        (vendorItem as VendorItem).VendorArea = GetZoneName((tmp as string == null) ? tmp.ToString() : tmp as string);
                        item.LocationInfo = new ItemLocationList() { vendorItem };
                        // We have to call for more source data at this point because all we have is
                        // the 5 saying its a Vendor item and the Vendor's name/area
                    } else if (sourcemore != null && sourcemore.TryGetValue("t", out tmp)) {
                        /*
                        //#define CTYPE_NPC            1
                        //#define CTYPE_OBJECT         2
                        //#define CTYPE_ITEM           3
                        //#define CTYPE_ITEMSET        4
                        //#define CTYPE_QUEST          5
                        //#define CTYPE_SPELL          6
                        //#define CTYPE_ZONE           7
                        //#define CTYPE_FACTION        8
                        //#define CTYPE_PET            9
                        //#define CTYPE_ACHIEVEMENT    10
                        */
                        switch ((int)tmp)
                        {
                            #region Mob Drop
                            case 1: //Dropped by a mob...
                                StaticDrop staticDrop = StaticDrop.Construct() as StaticDrop;
                                if (sourcemore.TryGetValue("n", out tmp))
                                {
                                    staticDrop.Boss = tmp.ToString();
                                }
                                if (sourcemore.TryGetValue("z", out tmp) || sourcemore.TryGetValue("c", out tmp))
                                {
                                    staticDrop.Area = GetZoneName(tmp.ToString());
                                }
                                if (sourcemore.TryGetValue("dd", out tmp))
                                {
                                    string value = tmp.ToString();
                                    staticDrop.Heroic = (value == "-2" || value == "3" || value == "4"); // Normal version is "-1"
                                    staticDrop.Area += (value == "1" || value == "3") ? " (10)" : ((value == "2" || value == "4") ? " (25)" : string.Empty);
                                }
                                item.LocationInfo = new ItemLocationList() { staticDrop };
                                // Since we have everything we need here, we shouldn't call new source data from the non-xml
                                needsNewSourceData = true;//false; // trying something new here to see if I can get Drop Rate data
                                break;
                            #endregion

                            #region Found in a Container
                            case 2: //Found in a container object
                                ContainerItem containerItem = ContainerItem.Construct() as ContainerItem;
                                if (sourcemore.TryGetValue("n", out tmp))
                                {
                                    containerItem.Container = tmp.ToString();
                                }
                                if (sourcemore.TryGetValue("z", out tmp) || sourcemore.TryGetValue("c", out tmp))
                                {
                                    containerItem.Area = GetZoneName(tmp.ToString());
                                }
                                if (sourcemore.TryGetValue("dd", out tmp))
                                {
                                    string value = tmp.ToString();
                                    containerItem.Heroic = (value == "-2" || value == "3" || value == "4"); // Normal version is "-1"
                                    containerItem.Area += (value == "1" || value == "3") ? " (10)" : ((value == "2" || value == "4") ? " (25)" : string.Empty);
                                }
                                item.LocationInfo = new ItemLocationList() { containerItem };
                                // Since we have everything we need here, we shouldn't call new source data from the non-xml
                                needsNewSourceData = false;
                                break;
                            #endregion

                            #region Quest Reward
                            case 5: //Rewarded from a quest...
                                QuestItem questName = QuestItem.Construct() as QuestItem;
                                #region This section only exists to get the zone type and the required level, which is *kind of* crap we don't really care about
                                if (sourcemore.TryGetValue("ti", out tmp))
                                {
                                    /*WebRequestWrapper wrwti = new WebRequestWrapper();
                                    string questItem = wrwti.DownloadQuestWowhead(tmp.ToString());
                                    if (questItem != null && !questItem.Contains("This quest doesn't exist or is not yet in the database."))
                                    {
                                        int levelStart = questItem.IndexOf("<div>Required level: ") + 21;
                                        if (levelStart == 20)
                                        {
                                            levelStart = questItem.IndexOf("<div>Requires level ") + 20;
                                        }
                                        if (levelStart > 19)
                                        {
                                            int levelEnd = questItem.IndexOf("</div>", levelStart);
                                            string level = questItem.Substring(levelStart, levelEnd - levelStart);
                                            if (level == "??")
                                            {
                                                levelStart = questItem.IndexOf("<div>Level: ") + 12;
                                                levelEnd = questItem.IndexOf("</div>", levelStart);
                                                questName.MinLevel = int.Parse(questItem.Substring(levelStart, levelEnd - levelStart));
                                            }
                                            else
                                            {
                                                questName.MinLevel = int.Parse(level);
                                            }
                                        }

                                        int typeStart = questItem.IndexOf("<div>Type: ") + 11;
                                        if (typeStart > 10)
                                        {
                                            int typeEnd = questItem.IndexOf("</div>", typeStart);
                                            switch (questItem.Substring(typeStart, typeEnd - typeStart))
                                            {
                                                case "Group":
                                                    int partyStart = questItem.IndexOf("Suggested Players [") + 19;
                                                    if (partyStart > 18)
                                                    {
                                                        int partyEnd = questItem.IndexOf("]", partyStart);
                                                        questName.Party = int.Parse(questItem.Substring(partyStart, partyEnd - partyStart));
                                                    }
                                                    break;

                                                case "Dungeon": questName.Type = "d"; break;
                                                case "Raid": questName.Type = "r"; break;
                                                default: questName.Type = ""; break;
                                            }
                                        }
                                    }*/
                                }
                                #endregion
                                if (sourcemore.TryGetValue("n", out tmp))
                                {
                                    questName.Quest = tmp.ToString();
                                }
                                if (sourcemore.TryGetValue("z", out tmp) || sourcemore.TryGetValue("c", out tmp))
                                {
                                    questName.Area = GetZoneName(tmp.ToString());
                                }
                                item.LocationInfo = new ItemLocationList() { questName };
                                // I'm going ahead and telling it false here because...
                                // who cares about dungeon type, group size or min level for quest
                                needsNewSourceData = false;
                                // Make the Item Unique
                                item.Unique = true;
                                break;
                            #endregion

                            #region Crafted by a Profession
                            case 6: //Crafted by a profession...
                                CraftedItem craftedItem = CraftedItem.Construct() as CraftedItem;
                                if (sourcemore.TryGetValue("n", out tmp))
                                {
                                    craftedItem.SpellName = tmp.ToString();
                                }
                                if (sourcemore.TryGetValue("s", out tmp))
                                {
                                    if (!(tmp is int) && String.IsNullOrEmpty(tmp as string)) { tmp = ""; }
                                    string profession = GetProfessionName(tmp.ToString());
                                    if (!string.IsNullOrEmpty(profession)) craftedItem.Skill = profession;
                                }
                                // XML does not include Skill Level data. Should ask Wowhead if they can add that.
                                //NOTE: Feedback sent to wowhead asking them to include it
                                item.LocationInfo = new ItemLocationList() { craftedItem };
                                // Since we have everything we need here, we shouldn't call new source data from the non-xml
                                needsNewSourceData = false;
                                break;
                            #endregion

                            #region Achievement Reward
                            case 10: //Rewarded from an achievement...
                                AchievementItem achName = AchievementItem.Construct() as AchievementItem;
                                if (sourcemore.TryGetValue("n", out tmp))
                                {
                                    achName.AchievementName = tmp.ToString();
                                }
                                item.LocationInfo = new ItemLocationList() { achName };
                                needsNewSourceData = false;
                                break;
                            #endregion

                            default:
                                break;
                        }
                    } else if (sourcemore != null && sourcemore.TryGetValue("p", out tmp)) {
                        // It's a PvP item
                        item.LocationInfo = new ItemLocationList() { PvpItem.Construct() };
                        (item.LocationInfo[0] as PvpItem).Points = 0;
                        (item.LocationInfo[0] as PvpItem).PointType = "PvP";
                        // We need to call for more source info
                    }
                    #endregion
                } else if (item.Stats.Resilience > 0) {
                    // We DON'T have Source Data, BUT the item has resilience on it, so it's a pvp item
                    item.LocationInfo = new ItemLocationList() { PvpItem.Construct() };
                    // We need to call for more source info
                } else {
                    // We DON'T have Source Data
                    // Since we are doing nothing, the ItemSource cache doesn't change
                    // Therefore the original ItemSource persists, if it's there
                    // We might possibly get more info from the non-xml, so let that at least run in an attempt
                }

                // If it's Craftable and Binds on Pickup, mark it as such
                if (/*item.LocationInfo != null &&*/ item.LocationInfo[0] is CraftedItem && (item.Bind == BindsOn.BoP)) {
                    (item.LocationInfo[0] as CraftedItem).Bind = BindsOn.BoP;
                }
                #endregion

                #region Meta Gem Effects
                if (item.Slot == ItemSlot.Meta)
                {
                    if (htmlTooltip.Contains("<span class=\"q1\">") && htmlTooltip.Contains("</span>"))
                    {
                        string line = htmlTooltip.Substring(htmlTooltip.IndexOf("<span class=\"q1\">") + "<span class=\"q1\">".Length);
                        line = line.Substring(0, line.IndexOf("</span>"));
                        if (line.Contains("<a"))
                        {
                            {
                                int start = line.IndexOf("<a");
                                int end = line.IndexOf(">", start + 1);
                                line = line.Remove(start, end - start + 1);
                            }
                            {
                                int start = line.IndexOf("</a>");
                                line = line.Remove(start, 4);
                            }
                        }
                        SpecialEffects.ProcessMetaGem(line, item.Stats, false);
                    }
                    else throw (new Exception("Unhandled Metagem:\r\n" + item.Name));
                }
                #endregion

                #region Special Effects
                List<string> useLines = new List<string>();
                List<string> equipLines = new List<string>();
                while (htmlTooltip.Contains("<span class=\"q2\">") && htmlTooltip.Contains("</span>"))
                {
                    htmlTooltip = htmlTooltip.Substring(htmlTooltip.IndexOf("<span class=\"q2\">") + "<span class=\"q2\">".Length);
                    string line = htmlTooltip.Substring(0, htmlTooltip.IndexOf("</span>"));

                    // Remove Comments
                    while (line.Contains("<!--"))
                    {
                        int start = line.IndexOf("<!--");
                        int end = line.IndexOf("-->");
                        string toRemove = line.Substring(start, end - start + 3);
                        line = line.Replace(toRemove, "");
                    }
                    // Swap out to real spaces
                    while (line.Contains("&nbsp;")) { line = line.Replace("&nbsp;", " "); }
                    // Remove the Spell Links
                    // Later we will instead USE the spell links but we aren't set up for that right now
                    while (line.Contains("<a"))
                    {
                        int start = line.IndexOf("<a");
                        int end = line.IndexOf(">");
                        string toRemove = line.Substring(start, end - start + 1);
                        line = line.Replace(toRemove, "");
                    }
                    while (line.Contains("</a>")) { line = line.Replace("</a>", ""); }
                    // Remove the Small tags, we don't use those
                    while (line.Contains("<small"))
                    {
                        int start = line.IndexOf("<small>");
                        int end = line.IndexOf("</small>");
                        string toRemove = line.Substring(start, end - start + "</small>".Length);
                        line = line.Replace(toRemove, "");
                    }
                    // Remove double spaces
                    while (line.Contains("  ")) { line = line.Replace("  ", " "); }
                    // Swap out "sec." with "sec" as sometimes they
                    // do and sometimes they don't, regex for both is annoying
                    while (line.Contains("sec.")) { line = line.Replace("sec.", "sec"); }

                    // Now Process it
                    if (line.StartsWith("Equip: "))
                    {
                        string equipLine = line.Substring("Equip: ".Length);
                        equipLines.Add(equipLine);
                    }
                    else if (line.StartsWith("Chance on hit: "))
                    {
                        string chanceLine = line.Substring("Chance on hit: ".Length);
                        equipLines.Add(chanceLine);
                    }
                    else if (line.StartsWith("Use: "))
                    {
                        string useLine = line.Substring("Use: ".Length);
                        useLines.Add(useLine);
                    }
                    htmlTooltip = htmlTooltip.Substring(line.Length + "</span>".Length);
                }
                foreach (string useLine in useLines) SpecialEffects.ProcessUseLine(useLine, item.Stats, false, item.ItemLevel, item.Id);
                foreach (string equipLine in equipLines) SpecialEffects.ProcessEquipLine(equipLine, item.Stats, false, item.ItemLevel, item.Id);
                #endregion

                #region Armor vs Bonus Armor Fixes
                if (item.Slot == ItemSlot.Finger ||
                    item.Slot == ItemSlot.MainHand ||
                    item.Slot == ItemSlot.Neck ||
                    (item.Slot == ItemSlot.OffHand && item.Type != ItemType.Shield) ||
                    item.Slot == ItemSlot.OneHand ||
                    item.Slot == ItemSlot.Trinket ||
                    item.Slot == ItemSlot.TwoHand)
                {
                    item.Stats.BonusArmor += item.Stats.Armor;
                    item.Stats.Armor = 0f;
                }
                else if (item.Stats.Armor + item.Stats.BonusArmor == 0f)
                { //Fix for wowhead bug where guns/bows/crossbows show up with 0 total armor, but 24.5 (or some such) bonus armor (they really have no armor at all)
                    item.Stats.Armor = 0;
                    item.Stats.BonusArmor = 0;
                }
                #endregion

                #region Belongs to a Set
                if (htmlTooltip.Contains(" (0/"))
                {
                    htmlTooltip = htmlTooltip.Substring(0, htmlTooltip.IndexOf("</a> (0/"));
                    htmlTooltip = htmlTooltip.Substring(htmlTooltip.LastIndexOf(">") + 1);
                    htmlTooltip = htmlTooltip.Replace("Wrathful ", "").Replace("Relentless ", "").Replace("Furious ", "").Replace("Deadly ", "").Replace("Hateful ", "").Replace("Savage ", "")
                        .Replace("Brutal ", "").Replace("Vengeful ", "").Replace("Merciless ", "").Replace("Valorous ", "")
                        .Replace("Heroes' ", "").Replace("Conqueror's ", "").Replace("Totally ", "").Replace("Triumphant ", "").Replace("Kirin'dor", "Kirin Tor").Replace("Regaila", "Regalia").Replace("Sanctified ", "");

                    if (htmlTooltip.Contains("Sunstrider's") || htmlTooltip.Contains("Zabra's") ||
                        htmlTooltip.Contains("Gul'dan's") || htmlTooltip.Contains("Garona's") ||
                        htmlTooltip.Contains("Runetotem's") || htmlTooltip.Contains("Windrunner's Pursuit") ||
                        htmlTooltip.Contains("Thrall's") || htmlTooltip.Contains("Liadrin's") ||
                        htmlTooltip.Contains("Hellscream's") || htmlTooltip.Contains("Kolitra's") || htmlTooltip.Contains("Koltira's"))
                    {
                        item.Faction = ItemFaction.Horde;
                    }
                    else if (htmlTooltip.Contains("Khadgar's") || htmlTooltip.Contains("Velen's") ||
                        htmlTooltip.Contains("Kel'Thuzad's") || htmlTooltip.Contains("VanCleef's") ||
                        htmlTooltip.Contains("Malfurion's") || htmlTooltip.Contains("Windrunner's Battlegear") ||
                        htmlTooltip.Contains("Nobundo's") || htmlTooltip.Contains("Turalyon's") ||
                        htmlTooltip.Contains("Wrynn's") || htmlTooltip.Contains("Thassarian's"))
                    {
                        item.Faction = ItemFaction.Alliance;
                    }

                    // normalize alliance/horde set names
                    htmlTooltip = htmlTooltip.Replace("Sunstrider's", "Khadgar's")   // Mage T9
                                             .Replace("Zabra's", "Velen's") // Priest T9
                                             .Replace("Gul'dan's", "Kel'Thuzad's") // Warlock T9
                                             .Replace("Garona's", "VanCleef's") // Rogue T9
                                             .Replace("Runetotem's", "Malfurion's") // Druid T9
                                             .Replace("Windrunner's Pursuit", "Windrunner's Battlegear") // Hunter T9
                                             .Replace("Thrall's", "Nobundo's") // Shaman T9
                                             .Replace("Liadrin's", "Turalyon's") // Paladin T9
                                             .Replace("Hellscream's", "Wrynn's") // Warrior T9
                                             .Replace("Koltira's", "Thassarian's")  // Death Knight T9
                                             .Replace("Kolitra's", "Thassarian's"); // Death Knight T9
                    item.SetName = htmlTooltip.Replace("Vicious", "").Trim();
                }
                #endregion

                // Filter out random suffix greens
                /*if (filter
                    && item.Quality == ItemQuality.Uncommon
                    && item.Stats <= new Stats() { Armor = 99999, AttackPower = 99999, SpellPower = 99999, BlockValue = 99999 })
                { e.Result = null; return; }*/
                #endregion
            } catch (Exception ex) {
                (sender as BackgroundWorker).ReportProgress(0, ex.Message + "|" + ex.StackTrace);
            }
        }
        private void bwParseItemSource_DoWork(object sender, DoWorkEventArgs e)
        {
            string hdoc = e.Argument as string;
            int id = _lastItemId;
            string itemId = "0";
            try
            {
                // First lets make sure it downloaded properly and pull the Item Id out of it. If not, stop working
                Regex idFinder = new Regex("wowhead.com.item=(\\d+)\"");
                Match match;
                if ((match = idFinder.Match(hdoc)).Success) {
                    id = int.Parse(match.Groups[1].Value);
                } else { return; } // It didn't load right
                Item item = ItemCache.Instance.Items[id];
                itemId = item.Id.ToString();

                //bool LocInfoIsValid = item.LocationInfo != null && item.LocationInfo.Count > 0;

                if (/*LocInfoIsValid &&*/ item.LocationInfo[0] is VendorItem) {
                    List<string> tokenIds = new List<string> { };
                    List<int> tokenCounts = new List<int> { };
                    string tokenName = "Unknown Currency";
                    //TokenDropInfo tokenDropInfo = null;
                    int goldCost = 0;
                    string repSource = "", repLevel = "";

                    #region Try to get the Token Names and individual costs
                    int liststartpos = hdoc.IndexOf("new Listview({template: 'npc', id: 'sold-by'");
                    if (liststartpos > 1) {
                        int listendpos = hdoc.IndexOf(";", liststartpos);
                        string costExcerpt = hdoc.Substring(liststartpos, listendpos - liststartpos);
                        // we are looking for something like cost:[0,0,0,[[40633,1]]]

                        // cost:[gold,[[currencyId1,currencyQu1],[currencyId2,currencyQu2]],[objectId,objectQu]]
                        Regex costRegex = new Regex(@"cost:\[(?<gold>\d+),\[(?:\[(?<currencyId1>\d+),(?<currencyQu1>\d+)\])?,?(?:\[(?<currencyId2>\d+),(?<currencyQu2>\d+)\])?\],\[(?:\[?(?<tokenId1>\d+),(?<tokenQu1>\d+)\]?)?,?(?:\[?(?<tokenId2>\d+),(?<tokenQu2>\d+)\]?)?\]\]");
                        Regex costRegexGoldOnly = new Regex(@"cost:\[(?<gold>\d+)\]");
                        Match costMatch;

                        if ((costMatch = costRegex.Match(costExcerpt)).Success) {
                            // Yay! it worked!
                            // Start with Gold Cost. Items that don't cost Gold will still get a default of 0
                            goldCost = int.Parse(costMatch.Groups["gold"].Value);
                            // Lets try Currency 1, such as Justice Points
                            if (!String.IsNullOrEmpty(costMatch.Groups["currencyId1"].Value)) {
                                tokenIds.Add(costMatch.Groups["currencyId1"].Value);
                                tokenCounts.Add(int.Parse(costMatch.Groups["currencyQu1"].Value));
                            }
                            // Lets try Currency 2, such as Justice Points
                            // Not sure if there would ever actually be a 2nd here, but it was formatted as if it was possible
                            // If nothing else, when currencyId1 fails but 2 is valid we put something in the array
                            if (!String.IsNullOrEmpty(costMatch.Groups["currencyId2"].Value)) {
                                tokenIds.Add(costMatch.Groups["currencyId2"].Value);
                                tokenCounts.Add(int.Parse(costMatch.Groups["currencyQu2"].Value));
                            }
                            // Lets try Token 1 cost, such as Mantle of the Conqueror
                            if (!String.IsNullOrEmpty(costMatch.Groups["tokenId1"].Value)) {
                                tokenIds.Add(costMatch.Groups["tokenId1"].Value);
                                tokenCounts.Add(int.Parse(costMatch.Groups["tokenQu1"].Value));
                            }
                            // Lets try Token 2 cost, such as Mantle of the Conqueror
                            if (!String.IsNullOrEmpty(costMatch.Groups["tokenId2"].Value)) {
                                tokenIds.Add(costMatch.Groups["tokenId2"].Value);
                                tokenCounts.Add(int.Parse(costMatch.Groups["tokenQu2"].Value));
                            }
                        } else if ((costMatch = costRegex.Match(costExcerpt)).Success) {
                            // Yay! it worked!
                            // Just do Gold Cost. Items that don't cost Gold will still get a default of 0
                            goldCost = int.Parse(costMatch.Groups["gold"].Value);
                        }
                    }
                    #endregion
                    #region Check to see if it requires a specific Faction and get its required level
                    liststartpos = hdoc.IndexOf("Requires <a href=\"/faction=");
                    if (liststartpos > 1)
                    {
                        int listendpos = hdoc.IndexOf("</td>", liststartpos);
                        string repExcerpt = hdoc.Substring(liststartpos, listendpos - liststartpos);
                        // we are looking for something like cost:[0,0,0,[[40633,1]]]

                        // cost:[gold,[[currencyId1,currencyQu1],[currencyId2,currencyQu2]],[objectId,objectQu]]
                        Regex repRegex = new Regex(@"Requires .a href=.\/faction=(?<factionId>\d+). class=.q\d.>.+\/a. - (?<level>(?:Friendly|Honored|Revered|Exalted))");
                        Match repMatch;

                        if ((repMatch = repRegex.Match(repExcerpt)).Success)
                        {
                            // Yay! it worked!
                            // Lets get the Faction's name based on the ID
                            repSource = GetItemFactionVendorInfo(repMatch.Groups["factionId"].Value, "0")[0];
                            switch (repMatch.Groups["level"].Value) {
                                case "Friendly":{ repLevel = "4"; break; }
                                case "Honored": { repLevel = "5"; break; }
                                case "Revered": { repLevel = "6"; break; }
                                case "Exalted": { repLevel = "7"; break; }
                                default: { repLevel = "0"; break; }
                            }
                        }
                    }
                    #endregion

                    VendorItem vendorItem = item.LocationInfo[0] as VendorItem;
                    vendorItem.Cost = goldCost;

                    for (int i = 0; i < 3; i++)
                    {
                        if (tokenIds.Count < i + 1) { break; } // stop processing if we don't have any more
                        // Check to see if it's a PvP token/Currency
                        if (!String.IsNullOrEmpty(tokenIds[i]) && _pvpTokenMap.TryGetValue(tokenIds[i], out tokenName)) {
                            item.LocationInfo = new ItemLocationList() { new PvpItem() { TokenCount = tokenCounts[i], TokenType = tokenName } };
                            vendorItem = null; // invalidate the vendor item so it doesn't get added in later
                            break;
                        } else if (tokenIds[i] != null && _vendorTokenMap.TryGetValue(tokenIds[i], out tokenName)) {
                            vendorItem.TokenMap[tokenName] = tokenCounts[i];
                        } else if (tokenIds[i] != null) {
                            if ((tokenName == "Justice Points" || tokenIds[i] == "395")
                                || (tokenName == "Valor Points" || tokenIds[i] == "396"))
                            { item.Cost = tokenCounts[i]; } // Sets the cost on the item for the user
                            #region It's a PvE Token
                            // ok now let's see what info we can get about this token
                            string boss = null; string vendor = null;
                            string area = null; string vendorarea = null;
                            bool heroic = false;
                            bool container = false;
                            if (!_tokenDropMap.ContainsKey(tokenIds[i])) {
                                // Not doing this =^( hopefully we won't actually need to cuz I mapped all the currencies listed in wowhead
                                #region We *really* haven't seen this before so we need to pull the data
                                /*XDocument docToken = wrw.DownloadItemWowhead(site, tokenIds[i]);
                                if (docToken != null)
                                {
                                    tokenNames[i] = docToken.SelectSingleNode("wowhead/item/name").Value;

                                    // we don't want token => boss propagation anymore, otherwise you get weird stuff like 277 gloves dropping from Toravon
                                    /*string tokenJson = docToken.SelectSingleNode("wowhead/item/json").InnerText;

                                    string tokenSource = string.Empty;
                                    if (tokenJson.Contains("\"source\":["))
                                    {
                                        tokenSource = tokenJson.Substring(tokenJson.IndexOf("\"source\":[") + "\"source\":[".Length);
                                        tokenSource = tokenSource.Substring(0, tokenSource.IndexOf("]"));
                                    }

                                    string tokenSourcemore = string.Empty;
                                    if (tokenJson.Contains("\"sourcemore\":[{"))
                                    {
                                        tokenSourcemore = tokenJson.Substring(tokenJson.IndexOf("\"sourcemore\":[{") + "\"sourcemore\":[{".Length);
                                        tokenSourcemore = tokenSourcemore.Substring(0, tokenSourcemore.IndexOf("}]"));
                                    }

                                    if (!string.IsNullOrEmpty(tokenSource) && !string.IsNullOrEmpty(tokenSourcemore))
                                    {
                                        string[] tokenSourceKeys = tokenSource.Split(',');
                                        string[] tokenSourcemoreKeys = tokenSourcemore.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

                                        // for tokens we prefer loot info, we don't care if it can be bought with badges
                                        tokenSource = tokenSourceKeys[0];
                                        tokenSourcemore = tokenSourcemoreKeys[0];

                                        int dropIndex = Array.IndexOf(tokenSourceKeys, "2");
                                        if (dropIndex >= 0)
                                        {
                                            tokenSource = tokenSourceKeys[dropIndex];
                                            tokenSourcemore = tokenSourcemoreKeys[dropIndex];
                                        }

                                        if (tokenSource == "2")
                                        {
                                            foreach (string kv in tokenSourcemore.Split(','))
                                            {
                                                if (!string.IsNullOrEmpty(kv))
                                                {
                                                    string[] keyvalsplit = kv.Split(':');
                                                    string key = keyvalsplit[0];
                                                    string val = keyvalsplit[1];
                                                    switch (key.Trim('"'))
                                                    {
                                                        case "t":
                                                            container = val == "2" || val == "3";
                                                            break;
                                                        case "n":       // NPC 'Name'
                                                            boss = val.Replace("\\'", "'").Trim('"');
                                                            break;
                                                        case "z":       // Zone
                                                            area = GetZoneName(val);
                                                            break;
                                                        case "dd":      // Dungeon Difficulty (1 = Normal, 2 = Heroic)
                                                            heroic = val == "2";
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (boss == null) 
                                    { 
                                        //boss = "Unknown Boss (Wowhead lacks data)";
                                        area = null; // if boss is null prefer treating this as pve token
                                    }*/
                                /*
                                if (tokenNames[i] != null)
                                {
                                    _tokenDropMap[tokenIds[i]] = new TokenDropInfo() { Boss = boss, Area = area, Heroic = heroic, Name = tokenNames[i], Container = container };
                                }
                            }*/
                                #endregion
                            } else {
                                #region We've seen this before so just use that data
                                TokenDropInfo info = _tokenDropMap[tokenIds[i]];
                                boss = info.Boss; vendor = info.Vendor;
                                area = info.Area; vendorarea = info.VendorArea;
                                heroic = info.Heroic;
                                tokenName = info.Name;
                                container = info.Container;
                                #endregion
                            }
                            if (tokenName != null) {
                                #region This is NOT a Dropped Token, so treat it as a normal vendor item and include token info
                                vendorItem.TokenMap[tokenName] = tokenCounts[i];
                                if (!String.IsNullOrEmpty(area) && !String.IsNullOrEmpty(vendorarea) && area != vendorarea && vendorarea != "Unknown Area") {
                                    // We are lucky enough to have BOTH drop points, so lets set both up
                                    vendorItem.VendorArea = vendorarea;
                                    vendorItem.VendorName = vendor;
                                    vendorItem.TokenMap[tokenName] = tokenCounts[i];
                                    ItemLocation droppoint = new StaticDrop() { Area = area, Boss = boss, Heroic = heroic, };
                                    item.LocationInfo = new ItemLocationList() { vendorItem, droppoint }; 
                                    vendorItem = null;
                                    break;
                                } else {
                                    vendorItem.VendorArea = area;
                                    vendorItem.VendorName = boss;
                                }
                                #endregion
                            } else if (area != null) {
                                #region This is a Dropped Token or we know what vendor is dropping it, so assign it to where it drops from
                                if (container) {
                                    ItemLocation locInfo = new ContainerItem()
                                    {
                                        Area = area,
                                        Container = boss,
                                        Heroic = heroic
                                    };
                                    item.LocationInfo = new ItemLocationList() { locInfo };
                                    vendorItem = null;
                                    break;
                                } else {
                                    ItemLocation locInfo = new StaticDrop()
                                    {
                                        Area = area,
                                        Boss = boss,
                                        Heroic = heroic
                                    };
                                    item.LocationInfo = new ItemLocationList() { locInfo };
                                    vendorItem = null;
                                    break;
                                }
                                #endregion
                            } else /*if (tokenNames[i] == null)*/ {
                                // there was an error pulling token data from web
                                // ignore source information
                                vendorItem = null;
                                break;
                            }
                            #endregion
                        } else {
                            #region There is no token so this is a normal vendor item
                            if (!String.IsNullOrEmpty(repSource) && !String.IsNullOrEmpty(repLevel)) {
                                string[] repInfo = GetItemFactionVendorInfo(repSource, repLevel);
                                FactionItem locInfo = new FactionItem()
                                {
                                    FactionName = repInfo[0],
                                    Level = (ReputationLevel)int.Parse(repLevel), // repInfo[3]
                                    Cost = goldCost,
                                };
                                item.LocationInfo = new ItemLocationList() { locInfo };
                                vendorItem = null;
                                break;
                            }/* else {
                                VendorItem locInfo = new VendorItem()
                                {
                                    Cost = goldCost,
                                };
                                if (!string.IsNullOrEmpty(n)) locInfo.VendorName = n;
                                if (sourcemore != null && sourcemore.TryGetValue("z", out tmp))
                                {
                                    locInfo.VendorArea = GetZoneName(tmp.ToString());
                                }
                                item.LocationInfo = new ItemLocationList() { locInfo };
                                vendorItem = null;
                                break;
                            }*/
                            #endregion
                        }
                    }
                    if (vendorItem != null) {
                        // We already set the Vendor Name and Zone
                        item.LocationInfo = new ItemLocationList() { vendorItem };
                    }
                }else if (/*LocInfoIsValid &&*/ item.LocationInfo[0] is StaticDrop) {
                    int count = 0, outof = 0;

                    #region Try to get the Count and OutOf numbers, which are used to make the Drop Rate Perc
                    int liststartpos = hdoc.IndexOf("new Listview({template: 'npc', id: 'dropped-by'");
                    if (liststartpos > 1) {
                        int listendpos = hdoc.IndexOf(";", liststartpos);
                        string dropExcerpt = hdoc.Substring(liststartpos, listendpos - liststartpos);
                        // we are looking for something like count:2939,outof:13677

                        // (?:count:(?<count>\d+),outof:(?<outof>\d+))
                        Regex dropRegex = new Regex(@"(?:count:(?<count>\d+),outof:(?<outof>\d+))");
                        Match dropMatch;

                        if ((dropMatch = dropRegex.Match(dropExcerpt)).Success) {
                            // Yay! it worked!
                            count = int.Parse(dropMatch.Groups["count"].Value);
                            outof = int.Parse(dropMatch.Groups["outof"].Value);
                        }
                    }
                    #endregion

                    StaticDrop dropItem = item.LocationInfo[0] as StaticDrop;

                    if (dropItem != null) {
                        dropItem.Count = count;
                        dropItem.OutOf = outof;
                        item.LocationInfo = new ItemLocationList() { dropItem };
                    }
                } else if (/*LocInfoIsValid &&*/ item.LocationInfo[0] is PvpItem) {
                    List<string> tokenIds = new List<string> { };
                    List<int> tokenCounts = new List<int> { };
                    string tokenName = "Unknown Currency";
                    //TokenDropInfo tokenDropInfo = null;
                    string repSource = "", repLevel = "";

                    #region Try to get the Token Names and individual costs
                    int liststartpos = hdoc.IndexOf("new Listview({template: 'npc', id: 'sold-by'");
                    if (liststartpos > 1) {
                        int listendpos = hdoc.IndexOf(";", liststartpos);
                        string costExcerpt = hdoc.Substring(liststartpos, listendpos - liststartpos);
                        // we are looking for something like cost:[0,0,0,[[40633,1]]]

                        // cost:[gold,[[currencyId1,currencyQu1],[currencyId2,currencyQu2]],[objectId,objectQu]]
                        Regex costRegex = new Regex(@"cost:\[(?<gold>\d+),\[(?:\[(?<currencyId1>\d+),(?<currencyQu1>\d+)\])?,?(?:\[(?<currencyId2>\d+),(?<currencyQu2>\d+)\])?\],\[(?:\[?(?<tokenId1>\d+),(?<tokenQu1>\d+)\]?)?,?(?:\[?(?<tokenId2>\d+),(?<tokenQu2>\d+)\]?)?\]\]");
                        Regex costRegexGoldOnly = new Regex(@"cost:\[(?<gold>\d+)\]");
                        Match costMatch;

                        if ((costMatch = costRegex.Match(costExcerpt)).Success) {
                            // Yay! it worked!
                            // Lets try Currency 1, such as Justice Points
                            if (!String.IsNullOrEmpty(costMatch.Groups["currencyId1"].Value)) {
                                tokenIds.Add(costMatch.Groups["currencyId1"].Value);
                                tokenCounts.Add(int.Parse(costMatch.Groups["currencyQu1"].Value));
                            }
                            // Lets try Currency 2, such as Justice Points
                            // Not sure if there would ever actually be a 2nd here, but it was formatted as if it was possible
                            // If nothing else, when currencyId1 fails but 2 is valid we put something in the array
                            if (!String.IsNullOrEmpty(costMatch.Groups["currencyId2"].Value)) {
                                tokenIds.Add(costMatch.Groups["currencyId2"].Value);
                                tokenCounts.Add(int.Parse(costMatch.Groups["currencyQu2"].Value));
                            }
                            // Lets try Token 1 cost, such as Mantle of the Conqueror
                            if (!String.IsNullOrEmpty(costMatch.Groups["tokenId1"].Value)) {
                                tokenIds.Add(costMatch.Groups["tokenId1"].Value);
                                tokenCounts.Add(int.Parse(costMatch.Groups["tokenQu1"].Value));
                            }
                            // Lets try Token 2 cost, such as Mantle of the Conqueror
                            if (!String.IsNullOrEmpty(costMatch.Groups["tokenId2"].Value)) {
                                tokenIds.Add(costMatch.Groups["tokenId2"].Value);
                                tokenCounts.Add(int.Parse(costMatch.Groups["tokenQu2"].Value));
                            }
                        } else if ((costMatch = costRegex.Match(costExcerpt)).Success) {
                            // Yay! it worked!
                        }
                    }
                    #endregion
                    #region Check to see if it requires a specific Faction and get its required level
                    liststartpos = hdoc.IndexOf("Requires <a href=\"/faction=");
                    if (liststartpos > 1)
                    {
                        int listendpos = hdoc.IndexOf("</td>", liststartpos);
                        string repExcerpt = hdoc.Substring(liststartpos, listendpos - liststartpos);
                        // we are looking for something like cost:[0,0,0,[[40633,1]]]

                        // cost:[gold,[[currencyId1,currencyQu1],[currencyId2,currencyQu2]],[objectId,objectQu]]
                        Regex repRegex = new Regex(@"Requires .a href=.\/faction=(?<factionId>\d+). class=.q\d.>.+\/a. - (?<level>(?:Friendly|Honored|Revered|Exalted))");
                        Match repMatch;

                        if ((repMatch = repRegex.Match(repExcerpt)).Success)
                        {
                            // Yay! it worked!
                            // Lets get the Faction's name based on the ID
                            repSource = GetItemFactionVendorInfo(repMatch.Groups["factionId"].Value, "0")[0];
                            switch (repMatch.Groups["level"].Value) {
                                case "Friendly":{ repLevel = "4"; break; }
                                case "Honored": { repLevel = "5"; break; }
                                case "Revered": { repLevel = "6"; break; }
                                case "Exalted": { repLevel = "7"; break; }
                                default: { repLevel = "0"; break; }
                            }
                        }
                    }
                    #endregion

                    PvpItem pvpItem = item.LocationInfo[0] as PvpItem;

                    for (int i = 0; i < 3; i++)
                    {
                        if (tokenIds.Count < i + 1) { break; } // stop processing if we don't have any more
                        // Check to see if it's a PvP token/Currency
                        if (!String.IsNullOrEmpty(tokenIds[i]) && _pvpTokenMap.TryGetValue(tokenIds[i], out tokenName)) {
                            item.LocationInfo = new ItemLocationList() { new PvpItem() { TokenCount = tokenCounts[i], TokenType = tokenName } };
                            pvpItem = null; // invalidate the vendor item so it doesn't get added in later
                            break;
                        } else if (tokenIds[i] != null && _vendorTokenMap.TryGetValue(tokenIds[i], out tokenName)) {
                            //pvpItem.TokenMap[tokenName] = tokenCounts[i];
                        } else if (tokenIds[i] != null) {
                            if ((tokenName == "Justice Points" || tokenIds[i] == "395")
                                || (tokenName == "Valor Points" || tokenIds[i] == "396"))
                            { item.Cost = tokenCounts[i]; } // Sets the cost on the item for the user
                            #region It's a PvE Token
                            // ok now let's see what info we can get about this token
                            string boss = null; string vendor = null;
                            string area = null; string vendorarea = null;
                            bool heroic = false;
                            bool container = false;
                            if (!_tokenDropMap.ContainsKey(tokenIds[i])) {
                                // Not doing this =^( hopefully we won't actually need to cuz I mapped all the currencies listed in wowhead
                                #region We *really* haven't seen this before so we need to pull the data
                                /*XDocument docToken = wrw.DownloadItemWowhead(site, tokenIds[i]);
                                if (docToken != null)
                                {
                                    tokenNames[i] = docToken.SelectSingleNode("wowhead/item/name").Value;

                                    // we don't want token => boss propagation anymore, otherwise you get weird stuff like 277 gloves dropping from Toravon
                                    /*string tokenJson = docToken.SelectSingleNode("wowhead/item/json").InnerText;

                                    string tokenSource = string.Empty;
                                    if (tokenJson.Contains("\"source\":["))
                                    {
                                        tokenSource = tokenJson.Substring(tokenJson.IndexOf("\"source\":[") + "\"source\":[".Length);
                                        tokenSource = tokenSource.Substring(0, tokenSource.IndexOf("]"));
                                    }

                                    string tokenSourcemore = string.Empty;
                                    if (tokenJson.Contains("\"sourcemore\":[{"))
                                    {
                                        tokenSourcemore = tokenJson.Substring(tokenJson.IndexOf("\"sourcemore\":[{") + "\"sourcemore\":[{".Length);
                                        tokenSourcemore = tokenSourcemore.Substring(0, tokenSourcemore.IndexOf("}]"));
                                    }

                                    if (!string.IsNullOrEmpty(tokenSource) && !string.IsNullOrEmpty(tokenSourcemore))
                                    {
                                        string[] tokenSourceKeys = tokenSource.Split(',');
                                        string[] tokenSourcemoreKeys = tokenSourcemore.Split(new string[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

                                        // for tokens we prefer loot info, we don't care if it can be bought with badges
                                        tokenSource = tokenSourceKeys[0];
                                        tokenSourcemore = tokenSourcemoreKeys[0];

                                        int dropIndex = Array.IndexOf(tokenSourceKeys, "2");
                                        if (dropIndex >= 0)
                                        {
                                            tokenSource = tokenSourceKeys[dropIndex];
                                            tokenSourcemore = tokenSourcemoreKeys[dropIndex];
                                        }

                                        if (tokenSource == "2")
                                        {
                                            foreach (string kv in tokenSourcemore.Split(','))
                                            {
                                                if (!string.IsNullOrEmpty(kv))
                                                {
                                                    string[] keyvalsplit = kv.Split(':');
                                                    string key = keyvalsplit[0];
                                                    string val = keyvalsplit[1];
                                                    switch (key.Trim('"'))
                                                    {
                                                        case "t":
                                                            container = val == "2" || val == "3";
                                                            break;
                                                        case "n":       // NPC 'Name'
                                                            boss = val.Replace("\\'", "'").Trim('"');
                                                            break;
                                                        case "z":       // Zone
                                                            area = GetZoneName(val);
                                                            break;
                                                        case "dd":      // Dungeon Difficulty (1 = Normal, 2 = Heroic)
                                                            heroic = val == "2";
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (boss == null) 
                                    { 
                                        //boss = "Unknown Boss (Wowhead lacks data)";
                                        area = null; // if boss is null prefer treating this as pve token
                                    }*/
                                /*
                                if (tokenNames[i] != null)
                                {
                                    _tokenDropMap[tokenIds[i]] = new TokenDropInfo() { Boss = boss, Area = area, Heroic = heroic, Name = tokenNames[i], Container = container };
                                }
                            }*/
                                #endregion
                            } else {
                                #region We've seen this before so just use that data
                                TokenDropInfo info = _tokenDropMap[tokenIds[i]];
                                boss = info.Boss; vendor = info.Vendor;
                                area = info.Area; vendorarea = info.VendorArea;
                                heroic = info.Heroic;
                                tokenName = info.Name;
                                container = info.Container;
                                #endregion
                            }
                            if (tokenName != null) {
                                #region This is NOT a Dropped Token, so treat it as a normal vendor item and include token info
                                //pvpItem.TokenMap[tokenName] = tokenCounts[i];
                                if (!String.IsNullOrEmpty(area) && !String.IsNullOrEmpty(vendorarea) && area != vendorarea && vendorarea != "Unknown Area") {
                                    // We are lucky enough to have BOTH drop points, so lets set both up
                                    //pvpItem.VendorArea = vendorarea;
                                    //pvpItem.VendorName = vendor;
                                    //pvpItem.TokenMap[tokenName] = tokenCounts[i];
                                    ItemLocation droppoint = new StaticDrop() { Area = area, Boss = boss, Heroic = heroic, };
                                    item.LocationInfo = new ItemLocationList() { pvpItem, droppoint }; 
                                    pvpItem = null;
                                    break;
                                } else {
                                    //pvpItem.VendorArea = area;
                                    //pvpItem.VendorName = boss;
                                }
                                #endregion
                            } else if (area != null) {
                                #region This is a Dropped Token or we know what vendor is dropping it, so assign it to where it drops from
                                if (container) {
                                    ItemLocation locInfo = new ContainerItem()
                                    {
                                        Area = area,
                                        Container = boss,
                                        Heroic = heroic
                                    };
                                    item.LocationInfo = new ItemLocationList() { locInfo };
                                    pvpItem = null;
                                    break;
                                } else {
                                    ItemLocation locInfo = new StaticDrop()
                                    {
                                        Area = area,
                                        Boss = boss,
                                        Heroic = heroic
                                    };
                                    item.LocationInfo = new ItemLocationList() { locInfo };
                                    pvpItem = null;
                                    break;
                                }
                                #endregion
                            } else /*if (tokenNames[i] == null)*/ {
                                // there was an error pulling token data from web
                                // ignore source information
                                pvpItem = null;
                                break;
                            }
                            #endregion
                        } else {
                            #region There is no token so this is a normal vendor item
                            if (!String.IsNullOrEmpty(repSource) && !String.IsNullOrEmpty(repLevel)) {
                                string[] repInfo = GetItemFactionVendorInfo(repSource, repLevel);
                                FactionItem locInfo = new FactionItem()
                                {
                                    FactionName = repInfo[0],
                                    Level = (ReputationLevel)int.Parse(repLevel), // repInfo[3]
                                    //Cost = goldCost,
                                };
                                item.LocationInfo = new ItemLocationList() { locInfo };
                                pvpItem = null;
                                break;
                            }/* else {
                                VendorItem locInfo = new VendorItem()
                                {
                                    Cost = goldCost,
                                };
                                if (!string.IsNullOrEmpty(n)) locInfo.VendorName = n;
                                if (sourcemore != null && sourcemore.TryGetValue("z", out tmp))
                                {
                                    locInfo.VendorArea = GetZoneName(tmp.ToString());
                                }
                                item.LocationInfo = new ItemLocationList() { locInfo };
                                vendorItem = null;
                                break;
                            }*/
                            #endregion
                        }
                    }
                    if (pvpItem != null) {
                        // We already set the Vendor Name and Zone
                        item.LocationInfo = new ItemLocationList() { pvpItem };
                    }
                } /*else if (item.LocationInfo == null) {
                   * // NOTICE: This check isn't possible anymore, but it's generating an Unknown automatically so it's not necessary either
                    // We DON'T have Source Data AND we didn't have one before. So lets set it to Unknown
                    item.LocationInfo = new ItemLocationList() { UnknownItem.Construct() };
                }*/ else {
                    // We DON'T have Source Data
                    // Since we are doing nothing, the ItemSource cache doesn't change
                    // Therefore the original ItemSource persists, if it's there
                }

                // If it's Craftable and Binds on Pickup, mark it as such
                if (item.LocationInfo[0] is CraftedItem && (item.Bind == BindsOn.BoP))
                {
                    (item.LocationInfo[0] as CraftedItem).Bind = BindsOn.BoP;
                }
            } catch (Exception ex) {
                (sender as BackgroundWorker).ReportProgress(0, ex.Message + "|" + ex.StackTrace);
            }
        }

        private void bwParse_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress = e.UserState.ToString();
        }
        private void bwParseItem_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                Progress = "Complete!";
                if (this.GetItemCompleted != null)
                {
                    this.GetItemCompleted(this, new EventArgs<Item>(e.Result as Item));
                    // Now that we have the item, call for its source under async (because we have to call a separate page)
                    if (needsNewSourceData) { GetItemSourceAsync((e.Result as Item).Id, UsePTR); }
                }
            }
        }

        #region Wowhead Lookups
        private class TokenDropInfo
        {
            public string Vendor = "Unknown Vendor";
            public string VendorArea = "Unknown Area";
            public string Boss = "Unknown Boss";
            public string Area = "Unknown Area";
            public bool Heroic = false;
            public bool Container = false;
            public string Name = "Unknown Token";
        }

        private static Dictionary<string, TokenDropInfo> _tokenDropMap = new Dictionary<string, TokenDropInfo>();
        private static Dictionary<string, string> _pvpTokenMap = new Dictionary<string, string>();
        private static Dictionary<string, string> _vendorTokenMap = new Dictionary<string, string>();
        private static Dictionary<string, QuestItem> _questRewardMap = new Dictionary<string, QuestItem>();
        private static List<string> _unhandledKeys = new List<string>();
        private static List<string> _unhandledSocketBonus = new List<string>();
        private static bool ProcessKeyValue(Item item, string key, string value)
        {
            switch (key.Trim('"'))
            {
                #region Item Info/Stat Keys
                case "id": //ID's are parsed out of the main data, not the json
                case "name": //Item names are parsed out of the main data, not the json
                case "subclass": //subclass is combined with class
                case "subsubclass": //Only used for Battle vs Guardian Elixirs
                case "buyprice": //Rawr doesn't care about buy...
                case "sellprice": //...and sell prices
                case "reqlevel": //Rawr assumes that you meet all level requirements
                case "dps": //Rawr calculates weapon dps based on min/max and speed
                case "maxcount": //Rawr doesn't deal with stack sizes
                case "dura": //durability isn't handled
                case "nsockets": //Rawr figures this out itself, Smart program.
                case "races": //Not worried about race restrictions
                case "source": //Handled below by individual keyvals
                case "sourcemore": //Handled below by individual keyvals
                case "nslots": //Don't care about bag sizes...
                case "avgmoney": //For containers, average amount of money inside
                case "glyph": //1=Major, 2=Minor
                    break;
                case "displayid": //A 3d display ID# for each icon
                    item.DisplayId = int.Parse(value);
                    break;
                case "slotbak": //A couple slots actually have two possible slots... ie vests and robes both fit in chest. slotbak distinguishes vests from robes. We don't care for Rawr, so ignored.
                    item.DisplaySlot = int.Parse(value); // it is also used for the 3d display slot id
                    break;
                case "level": //Rawr now handles item levels
                    item.ItemLevel = int.Parse(value);
                    break;
                //TODO:
                case "cooldown": //Not handled yet
                case "skill": //Related to skill requirements
                case "reqskill": //Related to skill requirements
                case "reqskillrank": //Related to skill requirements
                case "reqrep": //reqrep=6: heroic 5 man, reqrep=1: heroic raid, reqrep=5: Arena, reqrep=4: Faction Friendly, reqrep=5: Faction Honored, etc
                case "reqfaction": //Currently faction & reputation is not handled.
                case "itemset": //Contains the itemset id... May want to parse this,
                case "reqspell": //Profession specialization requirements, like weaponcrafting, armorsmithing, etc
                    break;

                case "slot":
                    int slot = int.Parse(value);
                    if (slot == 0) return true; //Not Equippable
                    item.Slot = GetItemSlot(slot);
                    break;

                case "classs":
                    if (value.StartsWith("1.") || value.StartsWith("12.")) return true; //Container and Quest
                    if (value.StartsWith("3."))
                    {
                        item.Type = ItemType.None;
                        switch (value)
                        {
                            case "3.0": item.Slot = ItemSlot.Red; break;
                            case "3.1": item.Slot = ItemSlot.Blue; break;
                            case "3.2": item.Slot = ItemSlot.Yellow; break;
                            case "3.3": item.Slot = ItemSlot.Purple; break;
                            case "3.4": item.Slot = ItemSlot.Green; break;
                            case "3.5": item.Slot = ItemSlot.Orange; break;
                            case "3.6": item.Slot = ItemSlot.Meta; break;
                            case "3.8": item.Slot = ItemSlot.Prismatic; break;
                            case "3.10": item.Slot = ItemSlot.Cogwheel; break;
                            case "3.11": item.Slot = ItemSlot.Hydraulic; break; // We don't actually know that this is 11 yet
                        }
                    }
                    else
                    {
                        item.Type = GetItemType(value);
                    }
                    break;

                case "speed":
                    item.Speed = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "dmgmin1":
                    item.MinDamage += (int)float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "dmgmax1":
                    item.MaxDamage += (int)float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "dmgtype1":
                    item.DamageType = (ItemDamageType)int.Parse(value);
                    break;

                case "armor":
                    item.Stats.Armor = int.Parse(value);
                    break;

                case "armorbonus":
                    item.Stats.Armor -= float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    item.Stats.BonusArmor = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "healthrgn":
                    item.Stats.Hp5 += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "manargn":
                    item.Stats.Mp5 += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "health":
                    item.Stats.Health += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "agi":
                    item.Stats.Agility += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "int":
                    item.Stats.Intellect += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "spi":
                    item.Stats.Spirit += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "sta":
                    item.Stats.Stamina += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "str":
                    item.Stats.Strength += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "hastertng":
                case "mlehastertng":
                case "rgdhastertng":
                case "splhastertng":
                    item.Stats.HasteRating = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "splpwr":
                case "splheal":
                case "spldmg":
                    item.Stats.SpellPower = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "critstrkrtng":
                case "mlecritstrkrtng":
                case "rgdcritstrkrtng":
                case "splcritstrkrtng":
                    item.Stats.CritRating = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "holres":
                    break;

                case "firres":
                    item.Stats.FireResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "natres":
                    item.Stats.NatureResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "frores":
                    item.Stats.FrostResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "shares":
                    item.Stats.ShadowResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "arcres":
                    item.Stats.ArcaneResistance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "hitrtng":
                case "mlehitrtng":
                case "rgdhitrtng":
                case "splhitrtng":
                    item.Stats.HitRating = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "atkpwr":
                case "mleatkpwr":
                    //case "feratkpwr":
                    item.Stats.AttackPower += float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "rgdatkpwr":
                    if (item.Stats.AttackPower != float.Parse(value, System.Globalization.CultureInfo.InvariantCulture))
                        item.Stats.RangedAttackPower = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    break;

                case "exprtng":
                    item.Stats.ExpertiseRating += int.Parse(value);
                    break;

                case "dodgertng":
                    item.Stats.DodgeRating += int.Parse(value);
                    break;

                case "blockrtng":
                    item.Stats.BlockRating += int.Parse(value);
                    break;

                case "socket1":
                    item.SocketColor1 = GetSocketType(value);
                    break;

                case "socket2":
                    item.SocketColor2 = GetSocketType(value);
                    break;

                case "socket3":
                    item.SocketColor3 = GetSocketType(value);
                    break;

                case "socketbonus":
                    item.SocketBonus = GetSocketBonus(value);
                    break;

                case "parryrtng":
                    item.Stats.ParryRating += int.Parse(value);
                    break;

                case "classes":
                    List<string> requiredClasses = new List<string>();
                    int classbitfield = int.Parse(value);
                    if ((classbitfield & 1) > 0)
                        requiredClasses.Add("Warrior");
                    if ((classbitfield & 2) > 0)
                        requiredClasses.Add("Paladin");
                    if ((classbitfield & 4) > 0)
                        requiredClasses.Add("Hunter");
                    if ((classbitfield & 8) > 0)
                        requiredClasses.Add("Rogue");
                    if ((classbitfield & 16) > 0)
                        requiredClasses.Add("Priest");
                    if ((classbitfield & 32) > 0)
                        requiredClasses.Add("Death Knight");
                    if ((classbitfield & 64) > 0)
                        requiredClasses.Add("Shaman");
                    if ((classbitfield & 128) > 0)
                        requiredClasses.Add("Mage");
                    if ((classbitfield & 256) > 0)
                        requiredClasses.Add("Warlock");
                    //if ((classbitfield & 512) > 0) ; // Only seems to occur in PvP gear, along with another huge value
                    //    requiredClasses.Add("");
                    if ((classbitfield & 1024) > 0)
                        requiredClasses.Add("Druid");
                    item.RequiredClasses = string.Join("|", requiredClasses.ToArray());
                    break;

                case "resirtng":
                    item.Stats.Resilience += int.Parse(value);
                    break;

                case "splpen":
                    item.Stats.SpellPenetration = int.Parse(value);
                    break;

                case "mana":
                    item.Stats.Mana = int.Parse(value);
                    break;

                case "dmg":
                    item.Stats.WeaponDamage = int.Parse(value);
                    break;

                case "frospldmg":
                    item.Stats.SpellFrostDamageRating = int.Parse(value);
                    break;

                case "shaspldmg":
                    item.Stats.SpellShadowDamageRating = int.Parse(value);
                    break;

                case "firspldmg":
                    item.Stats.SpellFireDamageRating = int.Parse(value);
                    break;

                case "arcspldmg":
                    item.Stats.SpellArcaneDamageRating = int.Parse(value);
                    break;
                #endregion
                #region Source Keys
                case "t":   //Source Type
                    /*
                    //#define CTYPE_NPC            1
                    //#define CTYPE_OBJECT         2
                    //#define CTYPE_ITEM           3
                    //#define CTYPE_ITEMSET        4
                    //#define CTYPE_QUEST          5
                    //#define CTYPE_SPELL          6
                    //#define CTYPE_ZONE           7
                    //#define CTYPE_FACTION        8
                    //#define CTYPE_PET            9
                    //#define CTYPE_ACHIEVEMENT    10
                    */
                    switch (value)
                    {
                        case "1": //Dropped by a mob...
                            item.LocationInfo = new ItemLocationList() { StaticDrop.Construct() };
                            break;

                        case "2": //Found in a container object
                            item.LocationInfo = new ItemLocationList() { ContainerItem.Construct() };
                            break;

                        case "3": //Found in a container item
                            item.LocationInfo = new ItemLocationList() { ContainerItem.Construct() };
                            break;

                        case "5": //Rewarded from a quest...
                            item.LocationInfo = new ItemLocationList() { QuestItem.Construct() };
                            break;

                        case "6": //Crafted by a profession...
                            item.LocationInfo = new ItemLocationList() { CraftedItem.Construct() };
                            break;

                        default:
                            "".ToString();
                            break;
                    }
                    break;

                case "ti":      // NPC ID that drops/gives... We use the name, so ignoring this
                    ItemLocation questName = item.LocationInfo[0];
                    /*if (questName is QuestItem)
                    {
                        WebRequestWrapper wrw = new WebRequestWrapper();
                        string questItem = wrw.DownloadQuestWowhead(value);
                        if (questItem != null && !questItem.Contains("This quest doesn't exist or is not yet in the database."))
                        {
                            int levelStart = questItem.IndexOf("<div>Required level: ") + 21;
                            if (levelStart == 20)
                            {
                                levelStart = questItem.IndexOf("<div>Requires level ") + 20;
                            }
                            if (levelStart > 19)
                            {
                                int levelEnd = questItem.IndexOf("</div>", levelStart);
                                string level = questItem.Substring(levelStart, levelEnd - levelStart);
                                if (level == "??")
                                {
                                    levelStart = questItem.IndexOf("<div>Level: ") + 12;
                                    levelEnd = questItem.IndexOf("</div>", levelStart);
                                    (questName as QuestItem).MinLevel = int.Parse(questItem.Substring(levelStart, levelEnd - levelStart));
                                }
                                else
                                {
                                    (questName as QuestItem).MinLevel = int.Parse(level);
                                }
                            }

                            int typeStart = questItem.IndexOf("<div>Type: ") + 11;
                            if (typeStart > 10)
                            {
                                int typeEnd = questItem.IndexOf("</div>", typeStart);
                                switch (questItem.Substring(typeStart, typeEnd - typeStart))
                                {
                                    case "Group":
                                        int partyStart = questItem.IndexOf("Suggested Players [") + 19;
                                        if (partyStart > 18)
                                        {
                                            int partyEnd = questItem.IndexOf("]", partyStart);
                                            (questName as QuestItem).Party = int.Parse(questItem.Substring(partyStart, partyEnd - partyStart));
                                        }
                                        break;

                                    case "Dungeon": (questName as QuestItem).Type = "d"; break;
                                    case "Raid": (questName as QuestItem).Type = "r"; break;
                                    default: (questName as QuestItem).Type = ""; break;
                                }
                            }
                        }
                    }*/
                    break;

                case "n":       // NPC 'Name'
                    ItemLocation locationName = item.LocationInfo[0];
                    if (locationName is StaticDrop) (locationName as StaticDrop).Boss = value;
                    if (locationName is ContainerItem) (locationName as ContainerItem).Container = value;
                    if (locationName is QuestItem) (locationName as QuestItem).Quest = value;
                    if (locationName is CraftedItem) (locationName as CraftedItem).SpellName = value;
                    break;

                case "z":       // Zone
                    string zonename = GetZoneName(value);
                    ItemLocation locationZone = item.LocationInfo[0];
                    if (locationZone is StaticDrop) (locationZone as StaticDrop).Area = zonename;
                    else if (locationZone is ContainerItem) (locationZone as ContainerItem).Area = zonename;
                    else if (locationZone is QuestItem) (locationZone as QuestItem).Area = zonename;
                    else if (locationZone is CraftedItem) (locationZone as CraftedItem).Skill = value;
                    else if (locationZone is WorldDrop) (locationZone as WorldDrop).Location = zonename;
                    break;

                case "c": //Zone again, used for quests
                    string continentname = GetZoneName(value);
                    ItemLocation locationContinent = item.LocationInfo[0];
                    if (locationContinent is StaticDrop) (locationContinent as StaticDrop).Area = continentname;
                    else if (locationContinent is ContainerItem) (locationContinent as ContainerItem).Area = continentname;
                    else if (locationContinent is QuestItem) (locationContinent as QuestItem).Area = continentname;
                    break;

                case "c2": //Don't care about continent
                    break;

                case "dd":      // Dungeon Difficulty 
                    // -1 = Normal Dungeon,  -2 = Heroic Dungeon
                    //  1 = Normal Raid (10), 2 = Normal Raid (25)
                    //  3 = Heroic Raid (10), 4 = Heroic Raid (25)
                    //hasdd = true;
                    //ddheroic = (value == "-2" || value == "3" || value == "4");
                    //ddarea = (value == "1" || value == "3" ) ? " (10)" :
                    //	( (value == "2" || value == "4") ? " (25)" : string.Empty  );
                    break;

                case "s":
                    if (item.LocationInfo[0] is CraftedItem)
                    {
                        string profession = "";
                        switch (Rawr.Properties.GeneralSettings.Default.Locale)
                        {
                            #region German
                            case "de":
                                switch (value)
                                {
                                    case "171": profession = "Alchemie"; break;
                                    case "164": profession = "Schmiedekunst"; break;
                                    case "333": profession = "Verzauberkunst"; break;
                                    case "202": profession = "Ingenieurskunst"; break;
                                    case "182": profession = "Kräuterkunde"; break;
                                    case "773": profession = "Inschriftenkunde"; break;
                                    case "755": profession = "Juwelenschleifen"; break;
                                    case "165": profession = "Lederverarbeitung"; break;
                                    case "186": profession = "Bergbau"; break;
                                    case "393": profession = "Kürschnerei"; break;
                                    case "197": profession = "Schneiderei"; break;
                                    case "185": profession = "Kochkunst"; break;
                                    case "129": profession = "Erste Hilfe"; break;
                                    case "356": profession = "Angeln"; break;
                                    case "762": profession = "Reiten"; break;
                                    case "794": profession = "Archäologie"; break;
                                    default:
                                        "".ToString();
                                        break;
                                }
                                break;
                            #endregion
                            #region Spanish
                            case "es":
                                switch (value)
                                {
                                    case "171": profession = "Alquimia"; break;
                                    case "164": profession = "Herrería"; break;
                                    case "333": profession = "Encantamiento"; break;
                                    case "202": profession = "Ingeniería"; break;
                                    case "182": profession = "Herboristería"; break;
                                    case "773": profession = "Inscripción"; break;
                                    case "755": profession = "Joyería"; break;
                                    case "165": profession = "Peletería"; break;
                                    case "186": profession = "Minería"; break;
                                    case "393": profession = "Desuello"; break;
                                    case "197": profession = "Sastrería"; break;
                                    case "185": profession = "Cocina"; break;
                                    case "129": profession = "Primeros auxilios"; break;
                                    case "356": profession = "Pesca"; break;
                                    case "762": profession = "Equitación"; break;
                                    case "794": profession = "Arqueología"; break;
                                    default:
                                        "".ToString();
                                        break;
                                }
                                break;
                            #endregion
                            #region French
                            case "fr":
                                switch (value)
                                {
                                    case "171": profession = "Alchimie"; break;
                                    case "164": profession = "Forge"; break;
                                    case "333": profession = "Enchantement"; break;
                                    case "202": profession = "Ingénierie"; break;
                                    case "182": profession = "Herboristerie"; break;
                                    case "773": profession = "Calligraphie"; break;
                                    case "755": profession = "Joaillerie"; break;
                                    case "165": profession = "Travail du cuir"; break;
                                    case "186": profession = "Minage"; break;
                                    case "393": profession = "Dépeçage"; break;
                                    case "197": profession = "Couture"; break;
                                    case "185": profession = "Cuisine"; break;
                                    case "129": profession = "Secourisme"; break;
                                    case "356": profession = "Pêche"; break;
                                    case "762": profession = "Monte"; break;
                                    case "794": profession = "Archéologie"; break;
                                    default:
                                        "".ToString();
                                        break;
                                }
                                break;
                            #endregion
                            #region Russian
                            case "ru":
                                switch (value)
                                {
                                    case "171": profession = "Алхимия"; break;
                                    case "164": profession = "Кузнечное дело"; break;
                                    case "333": profession = "Наложение чар"; break;
                                    case "202": profession = "Инженерное дело"; break;
                                    case "182": profession = "Травничество"; break;
                                    case "773": profession = "Начертание"; break;
                                    case "755": profession = "Ювелирное дело"; break;
                                    case "165": profession = "Кожевничество"; break;
                                    case "186": profession = "Горное дело"; break;
                                    case "393": profession = "Снятие шкур"; break;
                                    case "197": profession = "Портняжное дело"; break;
                                    case "185": profession = "Кулинария"; break;
                                    case "129": profession = "Первая помощь"; break;
                                    case "356": profession = "Рыбная ловля"; break;
                                    case "762": profession = "Верховая езда"; break;
                                    case "794": profession = "Археология"; break;
                                    default:
                                        "".ToString();
                                        break;
                                }
                                break;
                            #endregion
                            default:
                                switch (value)
                                {
                                    case "171": profession = "Alchemy"; break;
                                    case "164": profession = "Blacksmithing"; break;
                                    case "333": profession = "Enchanting"; break;
                                    case "202": profession = "Engineering"; break;
                                    case "182": profession = "Herbalism"; break;
                                    case "773": profession = "Inscription"; break;
                                    case "755": profession = "Jewelcrafting"; break;
                                    case "165": profession = "Leatherworking"; break;
                                    case "186": profession = "Mining"; break;
                                    case "393": profession = "Skinning"; break;
                                    case "197": profession = "Tailoring"; break;
                                    case "185": profession = "Cooking"; break;
                                    case "129": profession = "First Aid"; break;
                                    case "356": profession = "Fishing"; break;
                                    case "762": profession = "Riding"; break;
                                    case "794": profession = "Archaeology"; break;
                                    default:
                                        "".ToString();
                                        break;
                                }
                                break;
                        }
                        if (!string.IsNullOrEmpty(profession)) (item.LocationInfo[0] as CraftedItem).Skill = profession;
                    }
                    "".ToString();
                    break;

                case "q":
                    "".ToString();
                    break;

                case "p": //PvP
                    item.LocationInfo = new ItemLocationList() { PvpItem.Construct() };
                    (item.LocationInfo[0] as PvpItem).Points = 0;
                    (item.LocationInfo[0] as PvpItem).PointType = "PvP";
                    break;
                #endregion

                default:
                    if (!_unhandledKeys.Contains(key))
                        _unhandledKeys.Add(key);
                    break;
            }
            return false;
        }

        private static ItemType GetItemType(string subclassName, int inventoryType, int classId)
        {
            switch (subclassName)
            {
                case "Cloth": return ItemType.Cloth;
                case "Leather": return ItemType.Leather;
                case "Mail": return ItemType.Mail;
                case "Plate": return ItemType.Plate;
                case "Dagger": return ItemType.Dagger;
                case "Fist Weapon": return ItemType.FistWeapon;
                case "Axe": return (inventoryType == 17 ? ItemType.TwoHandAxe : ItemType.OneHandAxe);
                case "Mace": return (inventoryType == 17 ? ItemType.TwoHandMace : ItemType.OneHandMace);
                case "Sword": return (inventoryType == 17 ? ItemType.TwoHandSword : ItemType.OneHandSword);
                case "Polearm": return ItemType.Polearm;
                case "Staff": return ItemType.Staff;
                case "Shield": return ItemType.Shield;
                case "Bow": return ItemType.Bow;
                case "Crossbow": return ItemType.Crossbow;
                case "Gun": return ItemType.Gun;
                case "Wand": return ItemType.Wand;
                case "Thrown": return ItemType.Thrown;
                case "Arrow": return ItemType.Arrow;
                case "Bullet": return ItemType.Bullet;
                case "Quiver": return ItemType.Quiver;
                case "Ammo Pouch": return ItemType.AmmoPouch;
                case "Idol": //return ItemType.Idol;
                case "Libram": //return ItemType.Libram;
                case "Totem": //return ItemType.Totem;
                case "Sigil": //return ItemType.Sigil;
                case "Relic": return ItemType.Relic;
                default: return ItemType.None;
            }
        }

        private static ItemSlot GetItemSlot(int inventoryType, int classId)
        {
            switch (classId)
            {
                case 6: return ItemSlot.Projectile;
                case 11: return ItemSlot.ProjectileBag;
            }
            switch (inventoryType)
            {
                case 1: return ItemSlot.Head;
                case 2: return ItemSlot.Neck;
                case 3: return ItemSlot.Shoulders;
                case 16: return ItemSlot.Back;
                case 5:
                case 20: return ItemSlot.Chest;
                case 4: return ItemSlot.Shirt;
                case 19: return ItemSlot.Tabard;
                case 9: return ItemSlot.Wrist;
                case 10: return ItemSlot.Hands;
                case 6: return ItemSlot.Waist;
                case 7: return ItemSlot.Legs;
                case 8: return ItemSlot.Feet;
                case 11: return ItemSlot.Finger;
                case 12: return ItemSlot.Trinket;
                case 13: return ItemSlot.OneHand;
                case 17: return ItemSlot.TwoHand;
                case 21: return ItemSlot.MainHand;
                case 14:
                case 22:
                case 23: return ItemSlot.OffHand;
                case 15:
                case 25:
                case 26:
                case 28: return ItemSlot.Ranged;
                case 24: return ItemSlot.Projectile;
                case 27: return ItemSlot.ProjectileBag;
                default: return ItemSlot.None;
            }
        }

        private static string GetProfessionName(string profId) {
            string profession = "";
            switch (Rawr.Properties.GeneralSettings.Default.Locale)
            {
                #region German Translation
                case "de":
                    switch (profId.ToString())
                    {
                        case "171": profession = "Alchemie"; break;
                        case "794": profession = "Archäologie"; break;
                        case "164": profession = "Schmiedekunst"; break;
                        case "333": profession = "Verzauberkunst"; break;
                        case "202": profession = "Ingenieurskunst"; break;
                        case "182": profession = "Kräuterkunde"; break;
                        case "773": profession = "Inschriftenkunde"; break;
                        case "755": profession = "Juwelenschleifen"; break;
                        case "165": profession = "Lederverarbeitung"; break;
                        case "186": profession = "Bergbau"; break;
                        case "393": profession = "Kürschnerei"; break;
                        case "197": profession = "Schneiderei"; break;

                        default:
                            "".ToString();
                            break;
                    }
                    break;
                #endregion
                #region Spanish Translation
                case "es":
                    switch (profId.ToString())
                    {
                        case "171": profession = "Alquimia"; break;
                        case "794": profession = "Arqueología"; break;
                        case "164": profession = "Herrería"; break;
                        case "333": profession = "Encantamiento"; break;
                        case "202": profession = "Ingeniería"; break;
                        case "182": profession = "Herboristería"; break;
                        case "773": profession = "Inscripción"; break;
                        case "755": profession = "Joyería"; break;
                        case "165": profession = "Peletería"; break;
                        case "186": profession = "Minería"; break;
                        case "393": profession = "Desuello"; break;
                        case "197": profession = "Sastrería"; break;

                        default:
                            "".ToString();
                            break;
                    }
                    break;
                #endregion
                #region French Translation
                case "fr":
                    switch (profId.ToString())
                    {
                        case "171": profession = "Alchimie"; break;
                        case "794": profession = "Archéologie"; break;
                        case "164": profession = "Forge"; break;
                        case "333": profession = "Enchantement"; break;
                        case "202": profession = "Ingénierie"; break;
                        case "182": profession = "Herboristerie"; break;
                        case "773": profession = "Calligraphie"; break;
                        case "755": profession = "Joaillerie"; break;
                        case "165": profession = "Travail du cuir"; break;
                        case "186": profession = "Minage"; break;
                        case "393": profession = "Dépeçage"; break;
                        case "197": profession = "Couture"; break;

                        default:
                            "".ToString();
                            break;
                    }
                    break;
                #endregion
                #region Russian Translation
                case "ru":
                    switch (profId.ToString())
                    {
                        case "171": profession = "Алхимия"; break;
                        case "794": profession = "Археология"; break;
                        case "164": profession = "Кузнечное дело"; break;
                        case "333": profession = "Наложение чар"; break;
                        case "202": profession = "Инженерное дело"; break;
                        case "182": profession = "Травничество"; break;
                        case "773": profession = "Начертание"; break;
                        case "755": profession = "Ювелирное дело"; break;
                        case "165": profession = "Кожевничество"; break;
                        case "186": profession = "Горное дело"; break;
                        case "393": profession = "Снятие шкур"; break;
                        case "197": profession = "Портняжное дело"; break;

                        default:
                            "".ToString();
                            break;
                    }
                    break;
                #endregion
                default:
                    switch (profId.ToString())
                    {
                        case "171": profession = "Alchemy"; break;
                        case "794": profession = "Archaeology"; break;
                        case "164": profession = "Blacksmithing"; break;
                        case "333": profession = "Enchanting"; break;
                        case "202": profession = "Engineering"; break;
                        case "182": profession = "Herbalism"; break;
                        case "773": profession = "Inscription"; break;
                        case "755": profession = "Jewelcrafting"; break;
                        case "165": profession = "Leatherworking"; break;
                        case "186": profession = "Mining"; break;
                        case "393": profession = "Skinning"; break;
                        case "197": profession = "Tailoring"; break;

                        default:
                            "".ToString();
                            break;
                    }
                    break;
            }
            return profession;
        }

        private static string GetZoneName(string zoneId)
        {
            switch (Rawr.Properties.GeneralSettings.Default.Locale)
            {
                #region German Translations
                case "de":
                    {
                        switch (zoneId)
                        {
                            #region Arenas
                            case "3698": return "Arena von Nagrand";
                            case "3702": return "Arena des Schergrats";
                            case "3968": return "Ruinen von Lordaeron";
                            case "4378": return "Arena von Dalaran";
                            case "4406": return "Der Ring der Ehre";
                            #endregion
                            #region Battlegrounds
                            case "2597": return "Alteractal";
                            case "3277": return "Kriegshymnenschlucht";
                            case "3358": return "Arathibecken";
                            case "3820": return "Auge des Sturms";
                            case "4384": return "Strand der Uralten";
                            case "4710": return "Insel der Eroberung";
                            case "5031": return "Zwillingsgipfel";
                            case "5449": return "Die Schlacht um Gilneas";
                            #endregion
                            #region Old World
                            #region Kalimdor
                            case "14": return "Durotar";
                            case "15": return "Düstermarschen";
                            case "16": return "Azshara";
                            case "17": return "Nördliches Brachland";
                            case "141": return "Teldrassil";
                            case "148": return "Dunkelküste";
                            case "215": return "Mulgore";
                            case "331": return "Eschental";
                            case "373": return "Schipperküste";
                            case "361": return "Teufelswald";
                            case "400": return "Tausend Nadeln";
                            case "405": return "Desolace";
                            case "406": return "Steinkrallengebirge";
                            case "440": return "Tanaris";
                            case "457": return "Das Verhüllte Meer";
                            case "490": return "Krater von Un'Goro";
                            case "493": return "Mondlichtung";
                            case "618": return "Winterquell";
                            case "876": return "GM-Insel";
                            case "1377": return "Silithus";
                            case "1637": return "Orgrimmar";
                            case "1638": return "Donnerfels";
                            case "1657": return "Darnassus";
                            case "4709": return "Südliches Brachland";
                            case "5695": return "Ahn'Qiraj: Das Gefallene Königreich";
                            #endregion
                            #region Eastern Kingdoms
                            case "1": return "Dun Morogh";
                            case "3": return "Ödland";
                            case "4": return "Verwüstete Lande";
                            case "8": return "Sümpfe des Elends";
                            case "10": return "Dämmerwald";
                            case "11": return "Sumpfland";
                            case "12": return "Wald von Elwynn";
                            case "25": return "Der Schwarzfels";
                            case "28": return "Westliche Pestländer";
                            case "33": return "Nördliches Schlingendorntal";
                            case "38": return "Loch Modan";
                            case "40": return "Westfall";
                            case "41": return "Gebirgspass der Totenwinde";
                            case "44": return "Rotkammgebirge";
                            case "45": return "Arathihochland";
                            case "46": return "Brennende Steppe";
                            case "47": return "Hinterland";
                            case "51": return "Sengende Schlucht";
                            case "85": return "Tirisfal";
                            case "130": return "Silberwald";
                            case "139": return "Östliche Pestländer";
                            case "267": return "Vorgebirge des Hügellands";
                            case "1497": return "Unterstadt";
                            case "1519": return "Sturmwind";
                            case "1537": return "Eisenschmiede";
                            case "2257": return "Die Tiefenbahn";
                            case "5287": return "Das Schlingendornkap";
                            case "5339": return "Schlingendorntal";
                            #endregion
                            #region Dungeons
                            case "133": return "Gnomeregan";
                            case "209": return "Burg Schattenfang";
                            case "491": return "Kral der Klingenhauer";
                            case "717": return "Das Verlies";
                            case "718": return "Die Höhlen des Wehklagens";
                            case "719": return "Tiefschwarze Grotte";
                            case "722": return "Hügel der Klingenhauer";
                            case "796": return "Das Scharlachrote Kloster";
                            case "978": return "Zul'Farrak";
                            case "1337": return "Uldaman";
                            case "1417": return "Versunkener Tempel";
                            case "1581": return "Die Todesminen";
                            case "1583": return "Schwarzfelsspitze";
                            case "1584": return "Schwarzfelstiefen";
                            case "2017": return "Stratholme";
                            case "2057": return "Scholomance";
                            case "2100": return "Maraudon";
                            case "2437": return "Der Flammenschlund";
                            case "2557": return "Düsterbruch";
                            #endregion
                            #region Raids
                            case "2677": return "Pechschwingenhort";
                            case "2717": return "Geschmolzener Kern";
                            case "3428": return "Ahn'Qiraj";
                            case "3429": return "Ruinen von Ahn'Qiraj";
                            #endregion
                            case "-364": return "Dunkelmond-Jahrmarkt";
                            case "2917": return "Halle der Legenden";
                            case "2918": return "Halle der Champions";
                            #endregion
                            #region TBC
                            #region Zones
                            case "3430": return "Immersangwald";
                            case "3433": return "Geisterlande";
                            case "3483": return "Höllenfeuerhalbinsel";
                            case "3487": return "Silbermond";
                            case "3518": return "Nagrand";
                            case "3519": return "Wälder von Terokkar";
                            case "3520": return "Schattenmondtal";
                            case "3521": return "Zangarmarschen";
                            case "3522": return "Schergrat";
                            case "3523": return "Nethersturm";
                            case "3524": return "Azurmythosinsel";
                            case "3525": return "Blutmythosinsel";
                            case "3557": return "Die Exodar";
                            case "3703": return "Shattrath";
                            case "4080": return "Insel von Quel'Danas";
                            #endregion
                            #region Dungeons
                            case "2366": return "Der Schwarze Morast";
                            case "2367": return "Vorgebirge des Alten Hügellands";
                            case "3562": return "Höllenfeuerbollwerk";
                            case "3713": return "Der Blutkessel";
                            case "3714": return "Die Zerschmetterten Hallen";
                            case "3715": return "Die Dampfkammer";
                            case "3716": return "Der Tiefensumpf";
                            case "3717": return "Die Sklavenunterkünfte";
                            case "3789": return "Schattenlabyrinth";
                            case "3790": return "Auchenaikrypta";
                            case "3791": return "Sethekkhallen";
                            case "3792": return "Managruft";
                            case "3846": return "Die Arkatraz";
                            case "3847": return "Die Botanika";
                            case "3849": return "Die Mechanar";
                            case "4095": return "Terrasse der Magister";
                            #endregion
                            #region Raids
                            case "2562": return "Karazhan";
                            case "3606": return "Hyjalgipfel";
                            case "3607": return "Höhle des Schlangenschreins";
                            case "3618": return "Gruuls Unterschlupf";
                            case "3805": return "Zul'Aman";
                            case "3836": return "Magtheridons Kammer";
                            case "3842": return "Festung der Stürme";
                            case "3959": return "Der Schwarze Tempel";
                            case "4075": return "Sonnenbrunnenplateau";
                            #endregion
                            #endregion
                            #region WotLK
                            #region Zones
                            case "65": return "Drachenöde";
                            case "66": return "Zul'Drak";
                            case "67": return "Die Sturmgipfel";
                            case "210": return "Eiskrone";
                            case "394": return "Grizzlyhügel";
                            case "495": return "Der Heulende Fjord";
                            case "2817": return "Kristallsangwald";
                            case "3537": return "Boreanische Tundra";
                            case "3711": return "Sholazarbecken";
                            case "4197": return "Tausendwintersee";
                            case "4298": return "Pestländer: Die Scharlachrote Enklave";
                            case "4395": return "Dalaran";
                            case "4742": return "Hrothgars Landestelle";
                            #endregion
                            #region Dungeons
                            case "206": return "Burg Utgarde";
                            case "1196": return "Turm Utgarde";
                            case "3477": return "Azjol-Nerub";
                            case "4100": return "Das Ausmerzen von Stratholme";
                            case "4120": return "Der Nexus";
                            case "4196": return "Feste Drak'Tharon";
                            case "4228": return "Das Oculus";
                            case "4264": return "Hallen des Steins";
                            case "4272": return "Hallen der Blitze";
                            case "4375": return "Gundrak";
                            case "4415": return "Die Violette Festung";
                            case "4494": return "Ahn'kahet: Das Alte Königreich";
                            case "4723": return "Prüfung des Champions";
                            case "4809": return "Die Seelenschmiede";
                            case "4813": return "Grube von Saron";
                            case "4820": return "Hallen der Reflexion";
                            #endregion
                            #region Raids
                            case "2159": return "Onyxias Hort";
                            case "3456": return "Naxxramas";
                            case "4273": return "Ulduar";
                            case "4493": return "Das Obsidiansanktum";
                            case "4500": return "Das Auge der Ewigkeit";
                            case "4603": return "Archavons Kammer";
                            case "4722": return "Prüfung des Kreuzfahrers";
                            case "4812": return "Eiskronenzitadelle";
                            case "4987": return "Das Rubinsanktum";
                            #endregion
                            #endregion
                            #region Cataclysm
                            #region Zones
                            case "616": return "Hyjal";
                            case "4714": return "Gilneas";
                            case "4720": return "Die Verlorenen Inseln";
                            case "4737": return "Kezan";
                            case "4755": return "Gilneas";
                            case "4815": return "Tang'tharwald";
                            case "4922": return "Schattenhochland";
                            case "5034": return "Uldum";
                            case "5042": return "Tiefenheim";
                            case "5095": return "Tol Barad";
                            case "5144": return "Schimmernde Weiten";
                            case "5145": return "Abyssische Tiefen";
                            case "5146": return "Vashj'ir";
                            case "5416": return "Der Mahlstrom";
                            case "5630": return "Der Mahlstrom";
                            case "5733": return "Geschmolzene Front";
                            #endregion
                            #region Dungeons
                            case "4926": return "Schwarzfelshöhlen";
                            case "4945": return "Hallen des Ursprungs";
                            case "4950": return "Grim Batol";
                            case "5004": return "Thron der Gezeiten";
                            case "5035": return "Der Vortexgipfel";
                            case "5088": return "Der Steinerne Kern";
                            case "5396": return "Die Verlorene Stadt der Tol'vir";
                            #endregion
                            #region Raids
                            case "5094": return "Pechschwingenabstieg";
                            case "5334": return "Die Bastion des Zwielichts";
                            case "5600": return "Baradinfestung";
                            case "5638": return "Thron der Vier Winde";
                            case "5723": return "Feuerlande";
                            #endregion
                            case "5706": return "Die Dampfteiche";
                            #endregion
                            default: return "Unbekannt - " + zoneId;
                        }
                    }
                #endregion
                #region Spanish Translations
                case "es":
                    {
                        switch (zoneId)
                        {
                            #region Arenas
                            case "3698": return "Arena de Nagrand";
                            case "3702": return "Arena Filospada";
                            case "3968": return "Ruinas de Lordaeron";
                            case "4378": return "Arena de Dalaran";
                            case "4406": return "El Círculo del Valor";
                            #endregion
                            #region Battlegrounds
                            case "2597": return "Valle de Alterac";
                            case "3277": return "Garganta Grito de Guerra";
                            case "3358": return "Cuenca de Arathi";
                            case "3820": return "Ojo de la Tormenta";
                            case "4384": return "Playa de los Ancestros";
                            case "4710": return "Isla de la Conquista";
                            case "5031": return "Cumbres Gemelas";
                            case "5449": return "La Batalla por Gilneas";
                            #endregion
                            #region Old World
                            #region Kalimdor
                            case "14": return "Durotar";
                            case "15": return "Marjal Revolcafango";
                            case "16": return "Azshara";
                            case "17": return "Los Baldíos del Norte";
                            case "141": return "Teldrassil";
                            case "148": return "Costa Oscura";
                            case "215": return "Mulgore";
                            case "331": return "Vallefresno";
                            case "361": return "Frondavil";
                            case "373": return "Costa de la Huida";
                            case "400": return "Las Mil Agujas";
                            case "405": return "Desolace";
                            case "406": return "Sierra Espolón";
                            case "440": return "Tanaris";
                            case "457": return "Mar de la Bruma";
                            case "490": return "Cráter de Un'Goro";
                            case "493": return "Claro de la Luna";
                            case "618": return "Cuna del Invierno";
                            case "876": return "Isla de los MJ";
                            case "1377": return "Silithus";
                            case "1637": return "Orgrimmar";
                            case "1638": return "Cima del Trueno";
                            case "1657": return "Darnassus";
                            case "4709": return "Los Baldíos del Sur";
                            case "5695": return "Ahn'Qiraj: El Reino Caído";
                            #endregion
                            #region Eastern Kingdoms
                            case "1": return "Dun Morogh";
                            case "3": return "Tierras Inhóspitas";
                            case "4": return "Las Tierras Devastadas";
                            case "8": return "Pantano de las Penas";
                            case "10": return "Bosque del Ocaso";
                            case "11": return "Los Humedales";
                            case "12": return "Elwynn Forest";
                            case "25": return "Blackrock Mountain";
                            case "28": return "Western Plaguelands";
                            case "33": return "Northern Stranglethorn";
                            case "38": return "Loch Modan";
                            case "40": return "Westfall";
                            case "41": return "Deadwind Pass";
                            case "44": return "Redridge Mountains";
                            case "45": return "Arathi Highlands";
                            case "46": return "Burning Steppes";
                            case "47": return "The Hinterlands";
                            case "51": return "Searing Gorge";
                            case "85": return "Tirisfal Glades";
                            case "130": return "Silverpine Forest";
                            case "139": return "Eastern Plaguelands";
                            case "267": return "Hillsbrad Foothills";
                            case "1497": return "Undercity";
                            case "1519": return "Stormwind City";
                            case "1537": return "Ironforge";
                            case "2257": return "Deeprun Tram";
                            case "5287": return "The Cape of Stranglethorn";
                            case "5339": return "Stranglethorn Vale";
                            #endregion
                            #region Dungeons
                            case "133": return "Gnomeregan";
                            case "209": return "Shadowfang Keep";
                            case "491": return "Razorfen Kraul";
                            case "717": return "The Stockade";
                            case "718": return "Wailing Caverns";
                            case "719": return "Blackfathom Deeps";
                            case "722": return "Razorfen Downs";
                            case "796": return "Scarlet Monastery";
                            case "978": return "Zul'Farrak";
                            case "1337": return "Uldaman";
                            case "1417": return "Sunken Temple";
                            case "1581": return "The Deadmines";
                            case "1583": return "Blackrock Spire";
                            case "1584": return "Blackrock Depths";
                            case "2017": return "Stratholme";
                            case "2057": return "Scholomance";
                            case "2100": return "Maraudon";
                            case "2437": return "Ragefire Chasm";
                            case "2557": return "Dire Maul";
                            #endregion
                            #region Raids
                            case "2677": return "Blackwing Lair";
                            case "2717": return "Molten Core";
                            case "3428": return "Temple of Ahn'Qiraj";
                            case "3429": return "Ruins of Ahn'Qiraj";
                            #endregion
                            case "-364": return "Feria de la Luna Negra";
                            case "2917": return "Hall of Legends";
                            case "2918": return "Champion's Hall";
                            #endregion
                            #region TBC
                            #region Zones
                            case "3430": return "Eversong Woods";
                            case "3433": return "Ghostlands";
                            case "3483": return "Hellfire Peninsula";
                            case "3487": return "Silvermoon City";
                            case "3518": return "Nagrand";
                            case "3519": return "Terokkar Forest";
                            case "3520": return "Shadowmoon Valley";
                            case "3521": return "Zangarmarsh";
                            case "3522": return "Blade's Edge Mountains";
                            case "3523": return "Netherstorm";
                            case "3524": return "Azuremyst Isle";
                            case "3525": return "Bloodmyst Isle";
                            case "3557": return "The Exodar";
                            case "3703": return "Shattrath City";
                            case "4080": return "Isle of Quel'Danas";
                            #endregion
                            #region Dungeons
                            case "2366": return "The Black Morass";
                            case "2367": return "Old Hillsbrad Foothills";
                            case "3562": return "Hellfire Ramparts";
                            case "3713": return "The Blood Furnace";
                            case "3714": return "The Shattered Halls";
                            case "3715": return "The Steamvault";
                            case "3716": return "The Underbog";
                            case "3717": return "The Slave Pens";
                            case "3789": return "Shadow Labyrinth";
                            case "3790": return "Auchenai Crypts";
                            case "3791": return "Sethekk Halls";
                            case "3792": return "Mana-Tombs";
                            case "3846": return "The Arcatraz";
                            case "3847": return "The Botanica";
                            case "3849": return "The Mechanar";
                            case "4095": return "Magisters' Terrace";
                            #endregion
                            #region Raids
                            case "2562": return "Karazhan";
                            case "3606": return "Hyjal Summit";
                            case "3607": return "Serpentshrine Cavern";
                            case "3618": return "Gruul's Lair";
                            case "3805": return "Zul'Aman";
                            case "3836": return "Magtheridon's Lair";
                            case "3842": return "The Eye";
                            case "3959": return "Black Temple";
                            case "4075": return "Sunwell Plateau";
                            #endregion
                            #endregion
                            #region WotLK
                            #region Zones
                            case "65": return "Dragonblight";
                            case "66": return "Zul'Drak";
                            case "67": return "The Storm Peaks";
                            case "210": return "Icecrown";
                            case "394": return "Grizzly Hills";
                            case "495": return "Howling Fjord";
                            case "2817": return "Crystalsong Forest";
                            case "3537": return "Borean Tundra";
                            case "3711": return "Sholazar Basin";
                            case "4197": return "Wintergrasp";
                            case "4298": return "Plaguelands: The Scarlet Enclave";
                            case "4395": return "Dalaran";
                            case "4742": return "Hrothgar's Landing";
                            #endregion
                            #region Dungeons
                            case "206": return "Utgarde Keep";
                            case "1196": return "Utgarde Pinnacle";
                            case "3477": return "Azjol-Nerub";
                            case "4100": return "The Culling of Stratholme";
                            case "4120": return "The Nexus";
                            case "4196": return "Drak'Tharon Keep";
                            case "4228": return "The Oculus";
                            case "4264": return "Halls of Stone";
                            case "4272": return "Halls of Lightning";
                            case "4375": return "Gundrak";
                            case "4415": return "The Violet Hold";
                            case "4494": return "Ahn'kahet: The Old Kingdom";
                            case "4723": return "Trial of the Champion";
                            case "4809": return "Forge of Souls";
                            case "4813": return "Pit of Saron";
                            case "4820": return "Halls of Reflection";
                            #endregion
                            #region Raids
                            case "2159": return "Onyxia's Lair";
                            case "3456": return "Naxxramas";
                            case "4273": return "Ulduar";
                            case "4493": return "The Obsidian Sanctum";
                            case "4500": return "The Eye of Eternity";
                            case "4603": return "Vault of Archavon";
                            case "4722": return "Trial of the Crusader";
                            case "4812": return "Icecrown Citadel";
                            case "4987": return "Ruby Sanctum";
                            #endregion
                            #endregion
                            #region Cataclysm
                            #region Zones
                            case "616": return "Monte Hyjal";
                            case "4714": return "Gilneas";
                            case "4720": return "Las Islas Perdidas";
                            case "4737": return "Kezan";
                            case "4755": return "Ciudad de Gilneas";
                            case "4815": return "Vashj'ir: Bosque Kelp'thar";
                            case "4922": return "Tierras Altas Crepusculares";
                            case "5034": return "Uldum";
                            case "5042": return "Infralar";
                            case "5095": return "Tol Barad";
                            case "5144": return "Vashj'ir: Extensión Bruñida";
                            case "5145": return "Vashj'ir: Profundidades Abisales";
                            case "5146": return "Vashj'ir";
                            case "5416": return "La Vorágine";
                            case "5630": return "La Vorágine";
                            case "5733": return "Frente de Magma";
                            #endregion
                            #region Dungeons
                            case "4926": return "Cavernas Roca Negra";
                            case "4945": return "Cámaras de los Orígenes";
                            case "4950": return "Grim Batol";
                            case "5004": return "Trono de las Mareas";
                            case "5035": return "La Cumbre del Vórtice";
                            case "5088": return "El Núcleo Pétreo";
                            case "5396": return "Ciudad Perdida de los Tol'vir";
							case "1977": return "Zul'Gurub";
                            #endregion
                            #region Raids
                            case "5094": return "Descenso de Alanegra";
                            case "5334": return "El Bastión del Crepúsculo";
                            case "5600": return "Bastión de Baradin";
                            case "5638": return "Trono de los Cuatro Vientos";
                            case "5723": return "Tierras de Fuego";
                            #endregion
                            case "5706": return "The Steam Pools";
                            #endregion
                            default: return "Desconocida - " + zoneId;
                        }
                    }
                #endregion
                #region French Translations
                case "fr":
                    {
                        switch (zoneId)
                        {
                            #region Arenas
                            case "3698": return "Arène de Nagrand";
                            case "3702": return "Arène des Tranchantes";
                            case "3968": return "Ruines de Lordaeron";
                            case "4378": return "Arène de Dalaran";
                            case "4406": return "L'arène des Valeureux";
                            #endregion
                            #region Battlegrounds
                            case "2597": return "Vallée d'Alterac";
                            case "3277": return "Goulet des Chanteguerres";
                            case "3358": return "Bassin Arathi";
                            case "3820": return "L'Œil du cyclone";
                            case "4384": return "Rivage des Anciens";
                            case "4710": return "Île des Conquérants";
                            case "5031": return "Pics-Jumeaux";
                            case "5449": return "La bataille de Gilnéas";
                            #endregion
                            #region Old World
                            #region Kalimdor
                            case "14": return "Durotar";
                            case "15": return "Marécage d'Âprefange";
                            case "16": return "Azshara";
                            case "17": return "Tarides du Nord";
                            case "141": return "Teldrassil";
                            case "148": return "Sombrivage";
                            case "215": return "Mulgore";
                            case "331": return "Orneval";
                            case "361": return "Gangrebois";
                            case "373": return "Côte des Naufrages";
                            case "400": return "Mille pointes";
                            case "405": return "Désolace";
                            case "406": return "Les Serres-Rocheuses";
                            case "440": return "Tanaris";
                            case "457": return "La mer Voilée";
                            case "490": return "Cratère d'Un'Goro";
                            case "493": return "Reflet-de-Lune";
                            case "618": return "Berceau-de-l'Hiver";
                            case "876": return "Île des MJ";
                            case "1377": return "Silithus";
                            case "1637": return "Orgrimmar";
                            case "1638": return "Les Pitons-du-Tonnerre";
                            case "1657": return "Darnassus";
                            case "4709": return "Tarides du Sud";
                            case "5695": return "Ahn'Qiraj : le royaume Déchu";
                            #endregion
                            #region Eastern Kingdoms
                            case "1": return "Dun Morogh";
                            case "3": return "Terres Ingrates";
                            case "4": return "Terres Foudroyées";
                            case "8": return "Marais des Chagrins";
                            case "10": return "Bois de la Pénombre";
                            case "11": return "Les Paluns";
                            case "12": return "Forêt d'Elwynn";
                            case "25": return "Mont Rochenoire";
                            case "28": return "Maleterres de l'Ouest";
                            case "33": return "Strangleronce septentrionale";
                            case "38": return "Loch Modan";
                            case "40": return "Marche de l'Ouest";
                            case "41": return "Défilé de Deuillevent";
                            case "44": return "Les Carmines";
                            case "45": return "Hautes-terres Arathies";
                            case "46": return "Steppes Ardentes";
                            case "47": return "Les Hinterlands";
                            case "51": return "Gorge des Vents brûlants";
                            case "85": return "Clairières de Tirisfal";
                            case "130": return "Forêt des Pins-Argentés";
                            case "139": return "Maleterres de l'Est";
                            case "267": return "Contreforts de Hautebrande";
                            case "1497": return "Fossoyeuse";
                            case "1519": return "Hurlevent";
                            case "1537": return "1537";
                            case "2257": return "Tram des profondeurs";
                            case "5287": return "Cap Strangleronce";
                            case "5339": return "Vallée de Strangleronce";
                            #endregion
                            #region Dungeons
                            case "133": return "Gnomeregan";
                            case "209": return "Donjon d'Ombrecroc";
                            case "491": return "Kraal de Tranchebauge";
                            case "717": return "La Prison";
                            case "718": return "Cavernes des Lamentations";
                            case "719": return "Profondeurs de Brassenoire";
                            case "722": return "Souilles de Tranchebauge";
                            case "796": return "Monastère Écarlate";
                            case "978": return "Zul'Farrak";
                            case "1337": return "Uldaman";
                            case "1417": return "Temple englouti";
                            case "1581": return "Les Mortemines";
                            case "1583": return "Pic Rochenoire";
                            case "1584": return "Profondeurs de Rochenoire";
                            case "2017": return "Stratholme";
                            case "2057": return "Scholomance";
                            case "2100": return "Maraudon";
                            case "2437": return "Gouffre de Ragefeu";
                            case "2557": return "Hache-Tripes";
                            #endregion
                            #region Raids
                            case "2677": return "Repaire de l'Aile noire";
                            case "2717": return "Cœur du Magma";
                            case "3428": return "Ahn'Qiraj";
                            case "3429": return "Ruines d'Ahn'Qiraj";
                            #endregion
                            case "-364": return "Darkmoon Faire";
                            case "2917": return "Hall des Légendes";
                            case "2918": return "Hall des Champions";
                            #endregion
                            #region TBC
                            #region Zones
                            case "3430": return "Bois des Chants éternels";
                            case "3433": return "Les terres Fantômes";
                            case "3483": return "Péninsule des Flammes infernales";
                            case "3487": return "Lune-d'argent";
                            case "3518": return "Nagrand";
                            case "3519": return "Forêt de Terokkar";
                            case "3520": return "Vallée d'Ombrelune";
                            case "3521": return "Marécage de Zangar";
                            case "3522": return "Les Tranchantes";
                            case "3523": return "Raz-de-Néant";
                            case "3524": return "Île de Brume-Azur";
                            case "3525": return "Île de Brume-Sang";
                            case "3557": return "L'Exodar";
                            case "3703": return "Shattrath";
                            case "4080": return "Île de Quel'Danas";
                            #endregion
                            #region Dungeons
                            case "2366": return "Le Noir marécage";
                            case "2367": return "Contreforts de Hautebrande d'antan";
                            case "3562": return "Remparts des Flammes infernales";
                            case "3713": return "La Fournaise du sang";
                            case "3714": return "Les salles Brisées";
                            case "3715": return "Le caveau de la Vapeur";
                            case "3716": return "La Basse-tourbière";
                            case "3717": return "Les enclos aux esclaves";
                            case "3789": return "Labyrinthe des Ombres";
                            case "3790": return "Cryptes Auchenaï";
                            case "3791": return "Les salles des Sethekk";
                            case "3792": return "Tombes-mana";
                            case "3846": return "L'Arcatraz";
                            case "3847": return "La Botanica";
                            case "3849": return "Le Méchanar";
                            case "4095": return "Terrasse des Magistères";
                            #endregion
                            #region Raids
                            case "2562": return "Karazhan";
                            case "3606": return "Sommet d'Hyjal";
                            case "3607": return "Caverne du sanctuaire du Serpent";
                            case "3618": return "Repaire de Gruul";
                            case "3805": return "Zul'Aman";
                            case "3836": return "Le repaire de Magtheridon";
                            case "3842": return "Donjon de la Tempête";
                            case "3959": return "Temple Noir";
                            case "4075": return "Plateau du Puits de soleil";
                            #endregion
                            #endregion
                            #region WotLK
                            #region Zones
                            case "65": return "Désolation des dragons";
                            case "66": return "Zul'Drak";
                            case "67": return "Les pics Foudroyés";
                            case "210": return "La Couronne de glace";
                            case "394": return "Les Grisonnes";
                            case "495": return "Fjord Hurlant";
                            case "2817": return "Forêt du Chant de cristal";
                            case "3537": return "Toundra Boréenne";
                            case "3711": return "Bassin de Sholazar";
                            case "4197": return "Joug-d'hiver";
                            case "4298": return "Maleterres : l'enclave Écarlate";
                            case "4395": return "Dalaran";
                            case "4742": return "Accostage de Hrothgar";
                            #endregion
                            #region Dungeons
                            case "206": return "Donjon d'Utgarde";
                            case "1196": return "Cime d'Utgarde";
                            case "3477": return "Azjol-Nérub";
                            case "4100": return "L'Épuration de Stratholme";
                            case "4120": return "Le Nexus";
                            case "4196": return "Donjon de Drak'Tharon";
                            case "4228": return "L'Oculus";
                            case "4264": return "Les salles de Pierre";
                            case "4272": return "Les salles de Foudre";
                            case "4375": return "Gundrak";
                            case "4415": return "Le fort Pourpre";
                            case "4494": return "Ahn'kahet : l'Ancien royaume";
                            case "4723": return "L'épreuve du champion";
                            case "4809": return "La Forge des Âmes";
                            case "4813": return "Fosse de Saron";
                            case "4820": return "Salles des Reflets";
                            #endregion
                            #region Raids
                            case "2159": return "Repaire d'Onyxia";
                            case "3456": return "Naxxramas";
                            case "4273": return "Ulduar";
                            case "4493": return "Le sanctum Obsidien";
                            case "4500": return "L'Œil de l'éternité";
                            case "4603": return "Caveau d'Archavon";
                            case "4722": return "L'épreuve du croisé";
                            case "4812": return "Citadelle de la Couronne de glace";
                            case "4987": return "Le sanctum Rubis";
                            #endregion
                            #endregion
                            #region Cataclysm
                            #region Zones
                            case "616": return "Mont Hyjal";
                            case "4714": return "Gilnéas";
                            case "4720": return "Les îles Perdues";
                            case "4737": return "Kezan";
                            case "4755": return "Gilnéas";
                            case "4815": return "Vashj'ir: Forêt de Varech'thar";
                            case "4922": return "Hautes-terres du Crépuscule";
                            case "5034": return "Uldum";
                            case "5042": return "Le Tréfonds";
                            case "5095": return "Tol Barad";
                            case "5144": return "Vashj'ir: Étendues Chatoyantes";
                            case "5145": return "Vashj'ir: Profondeurs Abyssales";
                            case "5146": return "Vashj'ir";
                            case "5416": return "Le Maelström";
                            case "5630": return "Le Maelström";
                            case "5733": return "Front du Magma";
                            #endregion
                            #region Dungeons
                            case "4926": return "Cavernes de Rochenoire";
                            case "4945": return "Salles de l'Origine";
                            case "4950": return "Grim Batol";
                            case "5004": return "Trône des marées";
                            case "5035": return "La cime du Vortex";
                            case "5088": return "Le Cœur-de-pierre";
                            case "5396": return "Cité perdue des Tol'vir";
                            case "1977": return "Zul'Gurub";
                            #endregion
                            #region Raids
                            case "5094": return "Descente de l'Aile noire";
                            case "5334": return "Le bastion du Crépuscule";
                            case "5600": return "Bastion de Baradin";
                            case "5638": return "Trône des quatre vents";
                            case "5723": return "Terres de Feu";
                            #endregion
                            case "5706": return "Les bassins de Vapeur";
                            #endregion
                            default: return "Inconnue - " + zoneId;
                        }
                    }
                #endregion
                #region Russian Translations
                case "ru":
                    {
                        switch (zoneId)
                        {
                            #region Arenas
                            case "3698": return "Nagrand Arena (Nagrand Arena)";
                            case "3702": return "Blade's Edge Arena";
                            case "3968": return "Ruins of Lordaeron (Undercity Arena)";
                            case "4378": return "Dalaran Arena (Arena)";
                            case "4406": return "The Ring of Valor (Orgrimmar Arena)";
                            #endregion
                            #region Battlegrounds
                            case "2597": return "Alterac Valley";
                            case "3277": return "Warsong Gulch";
                            case "3358": return "Arathi Basin";
                            case "3820": return "Eye of the Storm";
                            case "4384": return "Strand of the Ancients";
                            case "4710": return "Isle of Conquest";
                            case "5031": return "Twin Peaks";
                            case "5449": return "The Battle for Gilneas";
                            #endregion
                            #region Old World
                            #region Kalimdor
                            case "14": return "Durotar";
                            case "15": return "Dustwallow Marsh";
                            case "16": return "Azshara";
                            case "17": return "Northern Barrens";
                            case "141": return "Teldrassil";
                            case "148": return "Darkshore";
                            case "215": return "Mulgore";
                            case "331": return "Ashenvale";
                            case "361": return "Felwood";
                            case "373": return "Берег Разбитых Кораблей";
                            case "400": return "Thousand Needles";
                            case "405": return "Desolace";
                            case "406": return "Stonetalon Mountains";
                            case "440": return "Tanaris";
                            case "457": return "The Veiled Sea";
                            case "490": return "Un'Goro Crater";
                            case "493": return "Moonglade";
                            case "618": return "Winterspring";
                            case "876": return "GM Island";
                            case "1377": return "Silithus";
                            case "1637": return "Orgrimmar";
                            case "1638": return "Thunder Bluff";
                            case "1657": return "Darnassus";
                            case "4709": return "Southern Barrens";
                            case "5695": return "Ahn'Qiraj: The Fallen Kingdom";
                            #endregion
                            #region Eastern Kingdoms
                            case "1": return "Dun Morogh";
                            case "3": return "Badlands";
                            case "4": return "Blasted Lands";
                            case "8": return "Swamp of Sorrows";
                            case "10": return "Duskwood";
                            case "11": return "Wetlands";
                            case "12": return "Elwynn Forest";
                            case "25": return "Blackrock Mountain";
                            case "28": return "Western Plaguelands";
                            case "33": return "Northern Stranglethorn";
                            case "38": return "Loch Modan";
                            case "40": return "Westfall";
                            case "41": return "Deadwind Pass";
                            case "44": return "Redridge Mountains";
                            case "45": return "Arathi Highlands";
                            case "46": return "Burning Steppes";
                            case "47": return "The Hinterlands";
                            case "51": return "Searing Gorge";
                            case "85": return "Tirisfal Glades";
                            case "130": return "Silverpine Forest";
                            case "139": return "Eastern Plaguelands";
                            case "267": return "Hillsbrad Foothills";
                            case "1497": return "Undercity";
                            case "1519": return "Stormwind City";
                            case "1537": return "Ironforge";
                            case "2257": return "Deeprun Tram";
                            case "5287": return "The Cape of Stranglethorn";
                            case "5339": return "Stranglethorn Vale";
                            #endregion
                            #region Dungeons
                            case "133": return "Gnomeregan";
                            case "209": return "Shadowfang Keep";
                            case "491": return "Razorfen Kraul";
                            case "717": return "The Stockade";
                            case "718": return "Wailing Caverns";
                            case "719": return "Blackfathom Deeps";
                            case "722": return "Razorfen Downs";
                            case "796": return "Scarlet Monastery";
                            case "978": return "Zul'Farrak";
                            case "1337": return "Uldaman";
                            case "1417": return "Sunken Temple";
                            case "1581": return "The Deadmines";
                            case "1583": return "Blackrock Spire";
                            case "1584": return "Blackrock Depths";
                            case "2017": return "Stratholme";
                            case "2057": return "Scholomance";
                            case "2100": return "Maraudon";
                            case "2437": return "Ragefire Chasm";
                            case "2557": return "Dire Maul";
                            #endregion
                            #region Raids
                            case "2677": return "Blackwing Lair";
                            case "2717": return "Molten Core";
                            case "3428": return "Temple of Ahn'Qiraj";
                            case "3429": return "Ruins of Ahn'Qiraj";
                            #endregion
                            case "-364": return "Ярмарка Новолуния";
                            case "2917": return "Hall of Legends";
                            case "2918": return "Champion's Hall";
                            #endregion
                            #region TBC
                            #region Zones
                            case "3430": return "Eversong Woods";
                            case "3433": return "Ghostlands";
                            case "3483": return "Hellfire Peninsula";
                            case "3487": return "Silvermoon City";
                            case "3518": return "Nagrand";
                            case "3519": return "Terokkar Forest";
                            case "3520": return "Shadowmoon Valley";
                            case "3521": return "Zangarmarsh";
                            case "3522": return "Blade's Edge Mountains";
                            case "3523": return "Netherstorm";
                            case "3524": return "Azuremyst Isle";
                            case "3525": return "Bloodmyst Isle";
                            case "3557": return "The Exodar";
                            case "3703": return "Shattrath City";
                            case "4080": return "Isle of Quel'Danas";
                            #endregion
                            #region Dungeons
                            case "2366": return "The Black Morass";
                            case "2367": return "Old Hillsbrad Foothills";
                            case "3562": return "Hellfire Ramparts";
                            case "3713": return "The Blood Furnace";
                            case "3714": return "The Shattered Halls";
                            case "3715": return "The Steamvault";
                            case "3716": return "The Underbog";
                            case "3717": return "The Slave Pens";
                            case "3789": return "Shadow Labyrinth";
                            case "3790": return "Auchenai Crypts";
                            case "3791": return "Sethekk Halls";
                            case "3792": return "Mana-Tombs";
                            case "3846": return "The Arcatraz";
                            case "3847": return "The Botanica";
                            case "3849": return "The Mechanar";
                            case "4095": return "Magisters' Terrace";
                            #endregion
                            #region Raids
                            case "2562": return "Karazhan";
                            case "3606": return "Hyjal Summit";
                            case "3607": return "Serpentshrine Cavern";
                            case "3618": return "Gruul's Lair";
                            case "3805": return "Zul'Aman";
                            case "3836": return "Magtheridon's Lair";
                            case "3842": return "The Eye";
                            case "3959": return "Black Temple";
                            case "4075": return "Sunwell Plateau";
                            #endregion
                            #endregion
                            #region WotLK
                            #region Zones
                            case "65": return "Dragonblight";
                            case "66": return "Zul'Drak";
                            case "67": return "The Storm Peaks";
                            case "210": return "Icecrown";
                            case "394": return "Grizzly Hills";
                            case "495": return "Howling Fjord";
                            case "2817": return "Crystalsong Forest";
                            case "3537": return "Borean Tundra";
                            case "3711": return "Sholazar Basin";
                            case "4197": return "Wintergrasp";
                            case "4298": return "Plaguelands: The Scarlet Enclave";
                            case "4395": return "Dalaran";
                            case "4742": return "Hrothgar's Landing";
                            #endregion
                            #region Dungeons
                            case "206": return "Utgarde Keep";
                            case "1196": return "Utgarde Pinnacle";
                            case "3477": return "Azjol-Nerub";
                            case "4100": return "The Culling of Stratholme";
                            case "4120": return "The Nexus";
                            case "4196": return "Drak'Tharon Keep";
                            case "4228": return "The Oculus";
                            case "4264": return "Halls of Stone";
                            case "4272": return "Halls of Lightning";
                            case "4375": return "Gundrak";
                            case "4415": return "The Violet Hold";
                            case "4494": return "Ahn'kahet: The Old Kingdom";
                            case "4723": return "Trial of the Champion";
                            case "4809": return "Forge of Souls";
                            case "4813": return "Pit of Saron";
                            case "4820": return "Halls of Reflection";
                            #endregion
                            #region Raids
                            case "2159": return "Onyxia's Lair";
                            case "3456": return "Naxxramas";
                            case "4273": return "Ulduar";
                            case "4493": return "The Obsidian Sanctum";
                            case "4500": return "The Eye of Eternity";
                            case "4603": return "Vault of Archavon";
                            case "4722": return "Trial of the Crusader";
                            case "4812": return "Icecrown Citadel";
                            case "4987": return "Ruby Sanctum";
                            #endregion
                            #endregion
                            #region Cataclysm
                            #region Zones
                            case "616": return "Хиджал";
                            case "4714": return "Гилнеас";
                            case "4720": return "Затерянные острова";
                            case "4737": return "Кезан";
                            case "4755": return "Гилнеас";
                            case "4815": return "Вайш'ир: Лес Келп’тар";
                            case "4922": return "Сумеречное нагорье";
                            case "5034": return "Ульдум";
                            case "5042": return "Подземье";
                            case "5095": return "Тол Барад";
                            case "5144": return "Вайш'ир: Мерцающий простор";
                            case "5145": return "Вайш'ир: Бездонные глубины";
                            case "5146": return "Вайш'ир";
                            case "5416": return "Водоворот";
                            case "5630": return "Водоворот";
                            case "5733": return "Огненная передовая";
                            #endregion
                            #region Dungeons
                            case "4926": return "Пещеры Черной горы";
                            case "4945": return "Чертоги Созидания";
                            case "4950": return "Грим Батол";
                            case "5004": return "Трон Приливов";
                            case "5035": return "Вершина Смерча";
                            case "5088": return "Каменные Недра";
                            case "5396": return "Затерянный город Тол'вир";
                            case "1977": return "Зул'Гуруб";
                            #endregion
                            #region Raids
                            case "5094": return "Твердыня Крыла Тьмы";
                            case "5334": return "Сумеречный бастион";
                            case "5600": return "Крепость Барадин";
                            case "5638": return "Трон Четырех Ветров";
                            case "5723": return "Огненные Просторы";
                            #endregion
                            case "5706": return "The Steam Pools";
                            #endregion
                            default: return "Неизвестно - " + zoneId;
                        }
                    }
                #endregion
                default:
                    {
                        switch (zoneId)
                        {
                            #region Arenas
                            case "3698": return "Nagrand Arena (Nagrand Arena)";
                            case "3702": return "Blade's Edge Arena";
                            case "3968": return "Ruins of Lordaeron (Undercity Arena)";
                            case "4378": return "Dalaran Arena (Arena)";
                            case "4406": return "The Ring of Valor (Orgrimmar Arena)";
                            #endregion
                            #region Battlegrounds
                            case "2597": return "Alterac Valley";
                            case "3277": return "Warsong Gulch";
                            case "3358": return "Arathi Basin";
                            case "3820": return "Eye of the Storm";
                            case "4384": return "Strand of the Ancients";
                            case "4710": return "Isle of Conquest";
                            case "5031": return "Twin Peaks";
                            case "5449": return "The Battle for Gilneas";
                            #endregion
                            #region Old World
                            #region Kalimdor
                            case "14": return "Durotar";
                            case "15": return "Dustwallow Marsh";
                            case "16": return "Azshara";
                            case "17": return "Northern Barrens";
                            case "141": return "Teldrassil";
                            case "148": return "Darkshore";
                            case "215": return "Mulgore";
                            case "331": return "Ashenvale";
                            case "373": return "Scuttle Coast";
                            case "361": return "Felwood";
                            case "400": return "Thousand Needles";
                            case "405": return "Desolace";
                            case "406": return "Stonetalon Mountains";
                            case "440": return "Tanaris";
                            case "457": return "The Veiled Sea";
                            case "490": return "Un'Goro Crater";
                            case "493": return "Moonglade";
                            case "618": return "Winterspring";
                            case "876": return "GM Island";
                            case "1377": return "Silithus";
                            case "1637": return "Orgrimmar";
                            case "1638": return "Thunder Bluff";
                            case "1657": return "Darnassus";
                            case "4709": return "Southern Barrens";
                            case "5695": return "Ahn'Qiraj: The Fallen Kingdom";
                            #endregion
                            #region Eastern Kingdoms
                            case "1": return "Dun Morogh";
                            case "3": return "Badlands";
                            case "4": return "Blasted Lands";
                            case "8": return "Swamp of Sorrows";
                            case "10": return "Duskwood";
                            case "11": return "Wetlands";
                            case "12": return "Elwynn Forest";
                            case "25": return "Blackrock Mountain";
                            case "28": return "Western Plaguelands";
                            case "33": return "Northern Stranglethorn";
                            case "38": return "Loch Modan";
                            case "40": return "Westfall";
                            case "41": return "Deadwind Pass";
                            case "44": return "Redridge Mountains";
                            case "45": return "Arathi Highlands";
                            case "46": return "Burning Steppes";
                            case "47": return "The Hinterlands";
                            case "51": return "Searing Gorge";
                            case "85": return "Tirisfal Glades";
                            case "130": return "Silverpine Forest";
                            case "139": return "Eastern Plaguelands";
                            case "267": return "Hillsbrad Foothills";
                            case "1497": return "Undercity";
                            case "1519": return "Stormwind City";
                            case "1537": return "Ironforge";
                            case "2257": return "Deeprun Tram";
                            case "5287": return "The Cape of Stranglethorn";
                            case "5339": return "Stranglethorn Vale";
                            #endregion
                            #region Dungeons
                            case "133": return "Gnomeregan";
                            case "209": return "Shadowfang Keep";
                            case "491": return "Razorfen Kraul";
                            case "717": return "The Stockade";
                            case "718": return "Wailing Caverns";
                            case "719": return "Blackfathom Deeps";
                            case "722": return "Razorfen Downs";
                            case "796": return "Scarlet Monastery";
                            case "978": return "Zul'Farrak";
                            case "1337": return "Uldaman";
                            case "1417": return "Sunken Temple";
                            case "1581": return "The Deadmines";
                            case "1583": return "Blackrock Spire";
                            case "1584": return "Blackrock Depths";
                            case "2017": return "Stratholme";
                            case "2057": return "Scholomance";
                            case "2100": return "Maraudon";
                            case "2437": return "Ragefire Chasm";
                            case "2557": return "Dire Maul";
                            #endregion
                            #region Raids
                            case "2677": return "Blackwing Lair";
                            case "2717": return "Molten Core";
                            case "3428": return "Temple of Ahn'Qiraj";
                            case "3429": return "Ruins of Ahn'Qiraj";
                            #endregion
                            case "-364": return "Darkmoon Faire";
                            case "2917": return "Hall of Legends";
                            case "2918": return "Champion's Hall";
                            #endregion
                            #region TBC
                            #region Zones
                            case "3430": return "Eversong Woods";
                            case "3433": return "Ghostlands";
                            case "3483": return "Hellfire Peninsula";
                            case "3487": return "Silvermoon City";
                            case "3518": return "Nagrand";
                            case "3519": return "Terokkar Forest";
                            case "3520": return "Shadowmoon Valley";
                            case "3521": return "Zangarmarsh";
                            case "3522": return "Blade's Edge Mountains";
                            case "3523": return "Netherstorm";
                            case "3524": return "Azuremyst Isle";
                            case "3525": return "Bloodmyst Isle";
                            case "3557": return "The Exodar";
                            case "3703": return "Shattrath City";
                            case "4080": return "Isle of Quel'Danas";
                            #endregion
                            #region Dungeons
                            case "2366": return "The Black Morass";
                            case "2367": return "Old Hillsbrad Foothills";
                            case "3562": return "Hellfire Ramparts";
                            case "3713": return "The Blood Furnace";
                            case "3714": return "The Shattered Halls";
                            case "3715": return "The Steamvault";
                            case "3716": return "The Underbog";
                            case "3717": return "The Slave Pens";
                            case "3789": return "Shadow Labyrinth";
                            case "3790": return "Auchenai Crypts";
                            case "3791": return "Sethekk Halls";
                            case "3792": return "Mana-Tombs";
                            case "3846": return "The Arcatraz";
                            case "3847": return "The Botanica";
                            case "3849": return "The Mechanar";
                            case "4095": return "Magisters' Terrace";
                            #endregion
                            #region Raids
                            case "2562": return "Karazhan";
                            case "3606": return "Hyjal Summit";
                            case "3607": return "Serpentshrine Cavern";
                            case "3618": return "Gruul's Lair";
                            case "3805": return "Zul'Aman";
                            case "3836": return "Magtheridon's Lair";
                            case "3842": return "The Eye";
                            case "3959": return "Black Temple";
                            case "4075": return "Sunwell Plateau";
                            #endregion
                            #endregion
                            #region WotLK
                            #region Zones
                            case "-101": return "Dalaran"; // This is from the Kalu'ak Fishing Derby
                            case "65": return "Dragonblight";
                            case "66": return "Zul'Drak";
                            case "67": return "The Storm Peaks";
                            case "210": return "Icecrown";
                            case "394": return "Grizzly Hills";
                            case "495": return "Howling Fjord";
                            case "2817": return "Crystalsong Forest";
                            case "3537": return "Borean Tundra";
                            case "3711": return "Sholazar Basin";
                            case "4197": return "Wintergrasp";
                            case "4298": return "Plaguelands: The Scarlet Enclave";
                            case "4395": return "Dalaran";
                            case "4742": return "Hrothgar's Landing";
                            #endregion
                            #region Dungeons
                            case "206": return "Utgarde Keep";
                            case "1196": return "Utgarde Pinnacle";
                            case "3477": return "Azjol-Nerub";
                            case "4100": return "The Culling of Stratholme";
                            case "4120": return "The Nexus";
                            case "4196": return "Drak'Tharon Keep";
                            case "4228": return "The Oculus";
                            case "4264": return "Halls of Stone";
                            case "4272": return "Halls of Lightning";
                            case "4375": return "Gundrak";
                            case "4415": return "The Violet Hold";
                            case "4494": return "Ahn'kahet: The Old Kingdom";
                            case "4723": return "Trial of the Champion";
                            case "4809": return "Forge of Souls";
                            case "4813": return "Pit of Saron";
                            case "4820": return "Halls of Reflection";
                            #endregion
                            #region Raids
                            case "2159": return "Onyxia's Lair";
                            case "3456": return "Naxxramas";
                            case "4273": return "Ulduar";
                            case "4493": return "The Obsidian Sanctum";
                            case "4500": return "The Eye of Eternity";
                            case "4603": return "Vault of Archavon";
                            case "4722": return "Trial of the Crusader";
                            case "4812": return "Icecrown Citadel";
                            case "4987": return "Ruby Sanctum";
                            #endregion
                            #endregion
                            #region Cataclysm
                            #region Zones
                            case "616": return "Mount Hyjal";
                            case "4714": return "Gilneas";
                            case "4720": return "The Lost Isles";
                            case "4737": return "Kezan";
                            case "4755": return "Gilneas City";
                            case "4815": return "Vashj'ir: Kelp'thar Forest";
                            case "4922": return "Twilight Highlands";
                            case "5034": return "Uldum";
                            case "5042": return "Deepholm";
                            case "5095": return "Tol Barad";
                            case "5144": return "Vashj'ir: Shimmering Expanse";
                            case "5145": return "Vashj'ir: Abyssal Depths";
                            case "5146": return "Vashj'ir";
                            case "5416": 
                            case "5630": return "The Maelstrom";
                            case "5733": return "Molten Front";
                            #endregion
                            #region Dungeons
                            case "4926": return "Blackrock Caverns";
                            case "4945": return "Halls of Origination";
                            case "4950": return "Grim Batol";
                            case "5004": return "Throne of the Tides";
                            case "5035": return "The Vortex Pinnacle";
                            case "5088": return "The Stonecore";
                            case "5396": return "Lost City of the Tol'vir";
                            #endregion
                            #region Raids
                            case "5094": return "Blackwing Descent";
                            case "5334": return "The Bastion of Twilight";
                            case "5600": return "Baradin Hold";
                            case "5638": return "Throne of the Four Winds";
                            case "5723": return "Firelands";
                            #endregion
                            case "5706": return "The Steam Pools";
                            #endregion
                            default: return "Unknown - " + zoneId;
                        }
                    }
            }
        }

        private static ItemSlot GetSocketType(string socket)
        {
            switch (socket)
            {
                case "1": return ItemSlot.Meta;
                case "2": return ItemSlot.Red;
                case "4": return ItemSlot.Yellow;
                case "6": return ItemSlot.Orange;
                case "8": return ItemSlot.Blue;
                case "10": return ItemSlot.Purple;
                case "12": return ItemSlot.Green;
                case "14": return ItemSlot.Prismatic;
                case "32": return ItemSlot.Cogwheel;
                // Dont have this id yet, but assuming 33 for now
                case "33": return ItemSlot.Hydraulic;
                default:
                    throw (new Exception("Unknown Slot Type :" + socket));
            }
        }

        private static Stats GetSocketBonus(string socketbonus)
        {
            Stats stats = new Stats();
            switch (socketbonus)
            {
                #region Spell Power
                case "2900": stats.SpellPower += 4; break;
                case "2872":
                case "2889":
                case "3198":
                case "3596":
                case "3752": stats.SpellPower += 5; break;
                case "428":
                case "2770":
                case "3602": stats.SpellPower += 7; break;
                case "430":
                case "440":
                case "2314":
                case "3753": stats.SpellPower += 9; break;
                #endregion
                #region Attack Power
                case "3114": stats.AttackPower += 4; break;
                case "2936": stats.AttackPower += 8; break;
                case "1587":
                case "3356":
                case "3764": stats.AttackPower += 12; break;
                case "3877":
                case "1589": stats.AttackPower += 16; break;
                case "1597": stats.AttackPower += 32; break;
                #endregion
                #region Stamina
                case "2895": stats.Stamina += 4; break;
                case "2868":
                case "2882": stats.Stamina += 6; break;
                case "1886":
                case "3307": stats.Stamina += 9; break;
                case "3354":
                case "3305":
                case "3766": stats.Stamina += 12; break;
                case "4154": stats.Stamina += 15; break;
                case "4134": stats.Stamina += 30; break;
                case "4159": stats.Stamina += 45; break;
                #endregion
                #region Mp5
                case "2367":
                case "2865":
                case "3306": stats.Mp5 += 2; break;
                case "2370":
                case "2854": stats.Mp5 += 3; break;
                case "2371": stats.Mp5 += 4; break;
                #endregion
                #region Hit Rating
                case "2873":
                case "2908": stats.HitRating += 4; break;
                case "3351": stats.HitRating += 6; break;
                case "2767":
                case "2844": stats.HitRating += 8; break;
                case "4160": stats.HitRating += 10; break;
                //case "": stats.HitRating += 20; break;
                //case "": stats.HitRating += 30; break;
                #endregion
                #region Crit Rating
                case "2887":
                case "3204": stats.CritRating += 3; break;
                case "2864":
                case "2874":
                case "2951":
                case "2952":
                case "3263": stats.CritRating += 4; break;
                case "3301":
                case "3316": stats.CritRating += 6; break;
                case "2771":
                case "2787":
                case "2843":
                case "3314": stats.CritRating += 8; break;
                case "4151": stats.CritRating += 10; break;
                case "4152": stats.CritRating += 20; break;
                case "4153": stats.CritRating += 30; break;
                #endregion
                #region Spirit
                case "2890": stats.Spirit += 4; break;
                case "3311": stats.Spirit += 6; break;
                case "2842":
                case "3352": stats.Spirit += 8; break;
                case "4142": stats.Spirit += 10; break;
                case "4129": stats.Spirit += 20; break;
                case "4125": stats.Spirit += 30; break;
                #endregion
                #region Intellect
                case "2869": stats.Intellect += 4; break;
                case "3310": stats.Intellect += 6; break;
                case "3353": stats.Intellect += 8; break;
                case "4143": stats.Intellect += 10; break;
                case "4144": stats.Intellect += 20; break;
                case "4150": stats.Intellect += 30; break;
                #endregion
                #region Dodge Rating
                case "2871": stats.DodgeRating += 4; break;
                case "3358": stats.DodgeRating += 6; break;
                case "3304": stats.DodgeRating += 8; break;
                //case "": stats.DodgeRating += 10; break;
                //case "": stats.DodgeRating += 20; break;
                //case "": stats.DodgeRating += 30; break;
                #endregion
                #region Agility
                case "3149": stats.Agility += 2; break;
                case "2877": stats.Agility += 4; break;
                case "3355": stats.Agility += 6; break;
                case "3313": stats.Agility += 8; break;
                case "2782": stats.Agility += 10; break;
                case "4133": stats.Agility += 20; break;
                case "4145": stats.Agility += 30; break;
                #endregion
                #region Resilience
                case "2878": stats.Resilience += 4; break;
                case "3600": stats.Resilience += 6; break;
                case "3821": stats.Resilience += 8; break;
                case "4184": stats.Resilience += 10; break;
                case "4185": stats.Resilience += 20; break;
                case "4186": stats.Resilience += 30; break;
                #endregion
                #region Strength
                case "2892": stats.Strength += 4; break;
                case "2927": stats.Strength += 4; break;
                case "3312": stats.Strength += 8; break;
                case "3357": stats.Strength += 6; break;
                case "4135": stats.Strength += 10; break;
                case "4136": stats.Strength += 20; break;
                case "4158": stats.Strength += 30; break;
                #endregion
                #region Block Rating
                case "2972": stats.BlockRating += 4; break;
                case "3361": stats.BlockRating += 6; break;
                #endregion
                #region Haste Rating
                case "3267":
                case "3308": stats.HasteRating += 4; break;
                case "3309": stats.HasteRating += 6; break;
                case "2963":
                case "3303": stats.HasteRating += 8; break;
                case "4146": stats.HasteRating += 10; break;
                case "4140": stats.HasteRating += 20; break;
                case "4128": stats.HasteRating += 30; break;
                #endregion
                #region Expertise Rating
                case "3094": stats.ExpertiseRating += 4; break;
                case "3362": stats.ExpertiseRating += 6; break;
                case "3778": stats.ExpertiseRating += 8; break;
                //case "": stats.ExpertiseRating += 10; break;
                //case "": stats.ExpertiseRating += 20; break;
                //case "": stats.ExpertiseRating += 30; break;
                #endregion
                #region Parry Rating
                case "3359": stats.ParryRating += 4; break;
                case "3871": stats.ParryRating += 6; break;
                case "3360": stats.ParryRating += 8; break;
                case "4147": stats.ParryRating += 10; break;
                case "4139": stats.ParryRating += 20; break;
                //case "": stats.ParryRating += 30; break;
                #endregion
                #region Mastery Rating
                case "4123": stats.MasteryRating += 10; break;
                case "4137": stats.MasteryRating += 20; break;
                case "4138": stats.MasteryRating += 30; break;
                #endregion
                default:
                    if (!_unhandledSocketBonus.Contains(socketbonus))
                        _unhandledSocketBonus.Add(socketbonus);
                    break;
            }
            return stats;
        }

        private static ItemType GetItemType(string classSubclass)
        {
            switch (classSubclass)
            {
                case "4.1": return ItemType.Cloth;
                case "4.2": return ItemType.Leather;
                case "4.3": return ItemType.Mail;
                case "4.4": return ItemType.Plate;
                case "2.15": return ItemType.Dagger;
                case "2.13": return ItemType.FistWeapon;
                case "2.1": return ItemType.TwoHandAxe;
                case "2.0": return ItemType.OneHandAxe;
                case "2.5": return ItemType.TwoHandMace;
                case "2.4": return ItemType.OneHandMace;
                case "2.8": return ItemType.TwoHandSword;
                case "2.7": return ItemType.OneHandSword;
                case "2.6": return ItemType.Polearm;
                case "2.10": return ItemType.Staff;
                case "4.6": return ItemType.Shield;
                case "2.2": return ItemType.Bow;
                case "2.18": return ItemType.Crossbow;
                case "2.3": return ItemType.Gun;
                case "2.19": return ItemType.Wand;
                case "2.16": return ItemType.Thrown;
                case "6.2": return ItemType.Arrow;
                case "6.3": return ItemType.Bullet;
                case "11.2": return ItemType.Quiver;
                case "11.3": return ItemType.AmmoPouch;
                case "4.8": //return ItemType.Idol;
                case "4.7": //return ItemType.Libram;
                case "4.9": //return ItemType.Totem;
                case "4.10": //return ItemType.Sigil;
                case "4.11": return ItemType.Relic;
                default: return ItemType.None;
            }
        }

        private static ItemSlot GetItemSlot(int slotId)
        {
            switch (slotId)
            {
                case  1: return ItemSlot.Head;
                case  2: return ItemSlot.Neck;
                case  3: return ItemSlot.Shoulders;
                case 16: return ItemSlot.Back;
                case  5: case 20: return ItemSlot.Chest;
                case  4: return ItemSlot.Shirt;
                case 19: return ItemSlot.Tabard;
                case  9: return ItemSlot.Wrist;
                case 10: return ItemSlot.Hands;
                case  6: return ItemSlot.Waist;
                case  7: return ItemSlot.Legs;
                case  8: return ItemSlot.Feet;
                case 11: return ItemSlot.Finger;
                case 12: return ItemSlot.Trinket;
                case 13: return ItemSlot.OneHand;
                case 17: return ItemSlot.TwoHand;
                case 21: return ItemSlot.MainHand;
                case 14: case 22: case 23: return ItemSlot.OffHand;
                case 15: case 25: case 26: case 28: return ItemSlot.Ranged;
                case 24: return ItemSlot.Projectile;
                case 18: case 27: return ItemSlot.ProjectileBag;
                default: return ItemSlot.None;
            }
        }

        private static string[] GetItemFactionVendorInfo(string repReqdId, string repReqdLevel)
        {
            string[] retVal = new string[] { "Unknown Faction", "Unknown Vendor", "Unknown Zone", "Unknown Level" };

            switch (repReqdId)
            {
                #region Classic
                #region Alliance
                case "47": retVal[0] = "Ironforge"; retVal[1] = ""; retVal[2] = ""; break;
                case "54": retVal[0] = "Gnomeregan Exiles"; retVal[1] = ""; retVal[2] = ""; break;
                case "69": retVal[0] = "Darnassus"; retVal[1] = ""; retVal[2] = ""; break;
                case "72": retVal[0] = "Stormwind"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Alliance Forces
                case "509": retVal[0] = "The League of Arathor"; retVal[1] = ""; retVal[2] = ""; break;
                case "730": retVal[0] = "Stormpike Guard"; retVal[1] = ""; retVal[2] = ""; break;
                case "890": retVal[0] = "Silverwing Sentinels"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Horde
                case "68": retVal[0] = "Undercity"; retVal[1] = ""; retVal[2] = ""; break;
                case "76": retVal[0] = "Orgrimmar"; retVal[1] = ""; retVal[2] = ""; break;
                case "81": retVal[0] = "Thunder Bluff"; retVal[1] = ""; retVal[2] = ""; break;
                case "530": retVal[0] = "Darkspear Trolls"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Horde Forces
                case "510": retVal[0] = "The Defilers"; retVal[1] = ""; retVal[2] = ""; break;
                case "729": retVal[0] = "Frostwolf Clan"; retVal[1] = ""; retVal[2] = ""; break;
                case "889": retVal[0] = "Warsong Outriders"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Steamwheedle Cartel
                case "21": retVal[0] = "Booty Bay"; retVal[1] = ""; retVal[2] = ""; break;
                case "369": retVal[0] = "Gadgetzan"; retVal[1] = ""; retVal[2] = ""; break;
                case "470": retVal[0] = "Ratchet"; retVal[1] = ""; retVal[2] = ""; break;
                case "577": retVal[0] = "Everlook"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                case "59": retVal[0] = "Thorium Brotherhood"; retVal[1] = ""; retVal[2] = ""; break;
                case "87": retVal[0] = "Bloodsail Buccaneers"; retVal[1] = ""; retVal[2] = ""; break;
                case "92": retVal[0] = "Gelkis Clan Centaur"; retVal[1] = ""; retVal[2] = ""; break;
                case "93": retVal[0] = "Magram Clan Centaur"; retVal[1] = ""; retVal[2] = ""; break;
                case "270": retVal[0] = "Zandalar Tribe"; retVal[1] = ""; retVal[2] = ""; break; // Discontinued
                case "349": retVal[0] = "Ravenholdt"; retVal[1] = ""; retVal[2] = ""; break;
                case "529": retVal[0] = "Argent Dawn"; retVal[1] = ""; retVal[2] = ""; break;
                case "576": retVal[0] = "Timbermaw Hold"; retVal[1] = ""; retVal[2] = ""; break;
                case "609": retVal[0] = "Cenarion Circle"; retVal[1] = ""; retVal[2] = ""; break;
                case "749": retVal[0] = "Hydraxian Waterlords"; retVal[1] = ""; retVal[2] = ""; break;
                case "809": retVal[0] = "Shen'dralar"; retVal[1] = ""; retVal[2] = ""; break; // Discontinued
                case "909": retVal[0] = "Darkmoon Faire"; retVal[1] = ""; retVal[2] = ""; break;
                case "910": retVal[0] = "Brood of Nozdormu"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Guild
                case "1168": retVal[0] = "Guild Reputation"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region The Burning Crusade
                #region Shattrath City
                case "932": retVal[0] = "The Aldor"; retVal[1] = ""; retVal[2] = ""; break;
                case "934": retVal[0] = "The Scryers"; retVal[1] = ""; retVal[2] = ""; break;
                case "935": retVal[0] = "The Sha'tar"; retVal[1] = ""; retVal[2] = ""; break;
                case "1011": retVal[0] = "Lower City"; retVal[1] = ""; retVal[2] = ""; break;
                case "1031": retVal[0] = "Sha'tari Skyguard"; retVal[1] = ""; retVal[2] = ""; break;
                case "1077": retVal[0] = "Shattered Sun Offensive"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                case "911": retVal[0] = "Silvermoon City"; retVal[1] = ""; retVal[2] = ""; break;
                case "922": retVal[0] = "Tranquillien"; retVal[1] = ""; retVal[2] = ""; break;
                case "930": retVal[0] = "Exodar"; retVal[1] = ""; retVal[2] = ""; break;
                case "933": retVal[0] = "The Consortium"; retVal[1] = ""; retVal[2] = ""; break;
                case "941": retVal[0] = "The Mag'har"; retVal[1] = ""; retVal[2] = ""; break;
                case "942": retVal[0] = "Cenarion Expedition"; retVal[1] = ""; retVal[2] = ""; break;
                case "946": retVal[0] = "Honor Hold"; retVal[1] = ""; retVal[2] = ""; break;
                case "947": retVal[0] = "Thrallmar"; retVal[1] = ""; retVal[2] = ""; break;
                case "967": retVal[0] = "The Violet Eye"; retVal[1] = ""; retVal[2] = ""; break;
                case "970": retVal[0] = "Sporeggar"; retVal[1] = ""; retVal[2] = ""; break;
                case "978": retVal[0] = "Kurenai"; retVal[1] = ""; retVal[2] = ""; break;
                case "989": retVal[0] = "Keepers of Time"; retVal[1] = ""; retVal[2] = ""; break;
                case "990": retVal[0] = "The Scale of the Sands"; retVal[1] = ""; retVal[2] = ""; break;
                case "1012": retVal[0] = "Ashtongue Deathsworn"; retVal[1] = ""; retVal[2] = ""; break;
                case "1015": retVal[0] = "Netherwing"; retVal[1] = ""; retVal[2] = ""; break;
                case "1038": retVal[0] = "Ogri'la"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Wrath of the Lich King
                #region Alliance Vanguard
                case "1050": retVal[0] = "Valiance Expedition"; retVal[1] = ""; retVal[2] = ""; break;
                case "1068": retVal[0] = "Explorers' League"; retVal[1] = ""; retVal[2] = ""; break;
                case "1094": retVal[0] = "The Silver Covenant"; retVal[1] = ""; retVal[2] = ""; break;
                case "1126": retVal[0] = "The Frostborn"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Horde Expedition
                case "1064": retVal[0] = "The Taunka"; retVal[1] = ""; retVal[2] = ""; break;
                case "1067": retVal[0] = "The Hand of Vengeance"; retVal[1] = ""; retVal[2] = ""; break;
                case "1085": retVal[0] = "Warsong Offensive"; retVal[1] = ""; retVal[2] = ""; break;
                case "1124": retVal[0] = "The Sunreavers"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Sholazar Basin
                case "1104": retVal[0] = "Frenzyheart Tribe"; retVal[1] = ""; retVal[2] = ""; break;
                case "1105": retVal[0] = "The Oracles"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                case "1037": retVal[0] = "Alliance Vanguard"; retVal[1] = ""; retVal[2] = ""; break;
                case "1052": retVal[0] = "Horde Expedition"; retVal[1] = ""; retVal[2] = ""; break;
                case "1073": retVal[0] = "The Kalu'ak"; retVal[1] = ""; retVal[2] = ""; break;
                case "1090": retVal[0] = "Kirin Tor"; retVal[1] = ""; retVal[2] = ""; break;
                case "1091": retVal[0] = "The Wyrmrest Accord"; retVal[1] = ""; retVal[2] = ""; break;
                case "1098": retVal[0] = "Knights of the Ebon Blade"; retVal[1] = ""; retVal[2] = ""; break;
                case "1106": retVal[0] = "Argent Crusade"; retVal[1] = ""; retVal[2] = ""; break;
                case "1119": retVal[0] = "The Sons of Hodir"; retVal[1] = ""; retVal[2] = ""; break;
                case "1156": retVal[0] = "The Ashen Verdict"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Cataclysm
                case "1133": retVal[0] = "Bilgewater Cartel"; retVal[1] = ""; retVal[2] = ""; break;
                case "1134": retVal[0] = "Gilneas"; retVal[1] = ""; retVal[2] = ""; break;
                case "1135": retVal[0] = "The Earthen Ring"; retVal[1] = ""; retVal[2] = ""; break;
                case "1158": retVal[0] = "Guardians of Hyjal"; retVal[1] = ""; retVal[2] = ""; break;
                case "1171": retVal[0] = "Therazane"; retVal[1] = ""; retVal[2] = ""; break;
                case "1172": retVal[0] = "Dragonmaw Clan"; retVal[1] = ""; retVal[2] = ""; break;
                case "1173": retVal[0] = "Ramkahen"; retVal[1] = ""; retVal[2] = ""; break;
                case "1174": retVal[0] = "Wildhammer Clan"; retVal[1] = ""; retVal[2] = ""; break;
                case "1177": retVal[0] = "Baradin's Wardens"; retVal[1] = ""; retVal[2] = ""; break;
                case "1178": retVal[0] = "Hellscream's Reach"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                #region Other
                case "70": retVal[0] = "Syndicate"; retVal[1] = ""; retVal[2] = ""; break;
                case "589": retVal[0] = "Wintersaber Trainers"; retVal[1] = ""; retVal[2] = ""; break;
                #endregion
                default: break;
            }
            switch (repReqdLevel)
            {
                case "4": retVal[3] = "Friendly"; break;
                case "5": retVal[3] = "Honored"; break;
                case "6": retVal[3] = "Revered"; break;
                case "7": retVal[3] = "Exalted"; break;
                default: break;
            }

            if (retVal[0] == "") { retVal[0] = "Unknown Faction"; }
            if (retVal[1] == "") { retVal[1] = "Unknown Vendor"; }
            if (retVal[2] == "") { retVal[2] = "Unknown Zone"; }
            if (retVal[3] == "") { retVal[3] = "0"; }

            return retVal;
        }

        private static string getWowHeadStatID(string Name)
        {
            switch (Name)
            {
                case " Strength": return "20";
                case " Agility": return "21";
                case " Stamina": return "22";
                case " Intellect": return "23";
                case " Spirit": return "24";

                case " Health": return "115";
                case " Mana": return "116";
                case " Health per 5 sec": return "60";
                case " Mana per 5 sec": return "61";
                case " Mastery rating": return "170";

                case " Armor": return "41";
                case " Defense Rating": return "42";
                case " Block Value": return "43";
                case " Block Rating": return "44";
                case " Dodge Rating": return "45";
                case " Parry Rating": return "46";
                case " Bonus Armor": return "109";
                case " Resilience": return "79";

                case " Attack Power": return "77";
                case " Spell Power": return "123";
                case " Expertise Rating": return "117";
                case " Hit Rating": return "119";
                case " Crit Rating": return "96";
                case " Haste Rating": return "103";
                case " Melee Crit": return "84";

                case " Feral Attack Power": return "97";
                case " Spell Crit Rating": return "49";
                case " Spell Arcane Damage": return "52";
                case " Spell Fire Damage": return "53";
                case " Spell Nature Damage": return "56";
                case " Spell Shadow Damage": return "57";
                case " Armor Penetration Rating": return "114";
            }
            return string.Empty;
        }

        public static String GetWowheadWeightedReportURL(Character character)
        {
            return "http://www.wowhead.com/?items&filter=minrl=" + character.Level + ";" + getWowheadClassFilter(character.Class) + getWowheadWeightFilter(character);
        }

        private static string getWowheadClassFilter(CharacterClass className)
        {
            switch (className)
            {
                case CharacterClass.DeathKnight:
                    return "ub=6;";
                case CharacterClass.Druid:
                    return "ub=11;";
                case CharacterClass.Hunter:
                    return "ub=3;";
                case CharacterClass.Mage:
                    return "ub=8;";
                case CharacterClass.Paladin:
                    return "ub=2;";
                case CharacterClass.Priest:
                    return "ub=5;";
                case CharacterClass.Rogue:
                    return "ub=4;";
                case CharacterClass.Shaman:
                    return "ub=7;";
                case CharacterClass.Warlock:
                    return "ub=9;";
                case CharacterClass.Warrior:
                    return "ub=1;";
            }
            return string.Empty;
        }

        private static string getWowheadSlotFilter(CharacterSlot slot)
        {
            switch (slot)
            {
                case CharacterSlot.Back:
                    return "sl=16;";
                case CharacterSlot.Chest:
                    return "sl=5;";
                case CharacterSlot.Feet:
                    return "sl=8;";
                case CharacterSlot.Finger1:
                case CharacterSlot.Finger2:
                    return "sl=11;";
                case CharacterSlot.Hands:
                    return "sl=10;";
                case CharacterSlot.Head:
                    return "sl=1;";
                case CharacterSlot.Legs:
                    return "sl=7;";
                case CharacterSlot.MainHand:
                    return "sl=21:13:17;";
                case CharacterSlot.Neck:
                    return "sl=2;";
                case CharacterSlot.OffHand:
                    return "sl=13:14:22:23;";
                case CharacterSlot.Ranged:
                    return "sl=15:28:25;";
                case CharacterSlot.Shoulders:
                    return "sl=3;";
                case CharacterSlot.Trinket1:
                case CharacterSlot.Trinket2:
                    return "sl=12;";
                case CharacterSlot.Waist:
                    return "sl=6;";
                case CharacterSlot.Wrist:
                    return "sl=9;";
            }
            return string.Empty;
        }

        private static string getWowheadWeightFilter(Character character)
        {
            StringBuilder wt = new StringBuilder("wt=");
            StringBuilder wtv = new StringBuilder(";wtv=");
            if (character.CurrentModel == "Enhance") // force weapon dps ep value 6 to fix caster weapon display issue
            {
                wt.Append("134:");
                wtv.Append("6:");
            }
            ComparisonCalculationBase[] statValues = CalculationsBase.GetRelativeStatValues(character);
            Array.Sort(statValues, StatValueSorter);
            foreach (ComparisonCalculationBase ccb in statValues)
            {
                string stat = getWowHeadStatID(ccb.Name);
                if (!stat.Equals(string.Empty))
                {
                    wt.Append(stat);
                    wtv.Append(ccb.OverallPoints.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
                    wt.Append(":");
                    wtv.Append(":");
                }
            }
            if (wt.Equals("wt="))
                return string.Empty;
            else
                return wt.ToString().Substring(0, wt.Length - 1) + wtv.ToString().Substring(0, wtv.Length - 1);
        }
        #endregion

        public delegate bool UpgradeCancelCheck();

        /*public static void LoadUpgradesFromWowhead(Character character, CharacterSlot slot, bool usePTR, UpgradeCancelCheck cancel)
        {
            if (!string.IsNullOrEmpty(character.Name))
            {
                //WebRequestWrapper.ResetFatalErrorIndicator();
                List<ComparisonCalculationBase> gemCalculations = new List<ComparisonCalculationBase>();
                foreach (Item item in ItemCache.AllItems)
                {
                    if (item.Slot == ItemSlot.Blue || item.Slot == ItemSlot.Green || item.Slot == ItemSlot.Meta
                         || item.Slot == ItemSlot.Orange || item.Slot == ItemSlot.Prismatic || item.Slot == ItemSlot.Purple
                         || item.Slot == ItemSlot.Red || item.Slot == ItemSlot.Yellow)
                    {
                        gemCalculations.Add(Calculations.GetItemCalculations(item, character, item.Slot == ItemSlot.Meta ? CharacterSlot.Metas : CharacterSlot.Gems));
                    }
                }

                ComparisonCalculationBase idealRed = null, idealBlue = null, idealYellow = null, idealMeta = null;
                foreach (ComparisonCalculationBase calc in gemCalculations)
                {
                    if (Item.GemMatchesSlot(calc.Item, ItemSlot.Meta) && (idealMeta == null || idealMeta.OverallPoints < calc.OverallPoints))
                        idealMeta = calc;
                    if (Item.GemMatchesSlot(calc.Item, ItemSlot.Red) && (idealRed == null || idealRed.OverallPoints < calc.OverallPoints))
                        idealRed = calc;
                    if (Item.GemMatchesSlot(calc.Item, ItemSlot.Blue) && (idealBlue == null || idealBlue.OverallPoints < calc.OverallPoints))
                        idealBlue = calc;
                    if (Item.GemMatchesSlot(calc.Item, ItemSlot.Yellow) && (idealYellow == null || idealYellow.OverallPoints < calc.OverallPoints))
                        idealYellow = calc;
                }
                Dictionary<ItemSlot, int> idealGems = new Dictionary<ItemSlot, int>();
                idealGems.Add(ItemSlot.Meta, idealMeta == null ? 0 : idealMeta.Item.Id);
                idealGems.Add(ItemSlot.Red, idealRed == null ? 0 : idealRed.Item.Id);
                idealGems.Add(ItemSlot.Blue, idealBlue == null ? 0 : idealBlue.Item.Id);
                idealGems.Add(ItemSlot.Yellow, idealYellow == null ? 0 : idealYellow.Item.Id);
                idealGems.Add(ItemSlot.None, 0);

                #region status queuing

                if (slot != CharacterSlot.None)
                {
                    StatusMessaging.UpdateStatus(slot.ToString(), "Queued");
                }
                else
                {
                    StatusMessaging.UpdateStatus(CharacterSlot.Head.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Neck.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Shoulders.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Back.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Chest.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Wrist.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Hands.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Waist.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Legs.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Feet.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Finger1.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Finger2.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Trinket1.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Trinket2.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.MainHand.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.OffHand.ToString(), "Queued");
                    StatusMessaging.UpdateStatus(CharacterSlot.Ranged.ToString(), "Queued");
                }

                #endregion

                if (slot != CharacterSlot.None)
                {
                    LoadUpgradesForSlot(character, slot, idealGems, usePTR, cancel);
                }
                else
                {
                    LoadUpgradesForSlot(character, CharacterSlot.Head, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Neck, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Shoulders, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Back, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Chest, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Wrist, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Hands, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Waist, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Legs, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Feet, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Finger1, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Finger2, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Trinket1, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Trinket2, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.MainHand, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.OffHand, idealGems, usePTR, cancel);
                    LoadUpgradesForSlot(character, CharacterSlot.Ranged, idealGems, usePTR, cancel);
                }
            }
            else
            {
                Base.ErrorBox eb = new Base.ErrorBox("", "You need to have a character loaded for Rawr to find Wowhead upgrades.");
                eb.Show();
            }
        }

        public static void ImportItemsFromWowhead(string filter) { ImportItemsFromWowhead(filter, false); }
        public static void ImportItemsFromWowhead(string filter, bool usePTR)
        {
            //WebRequestWrapper.ResetFatalErrorIndicator();

            string docUpgradeSearch = null;
            try
            {
                string site = usePTR ? "ptr" : "www";
                StatusMessaging.UpdateStatus("ImportWowheadFilter", "Downloading Item List");
                WebRequestWrapper wrw = new WebRequestWrapper();
                docUpgradeSearch = wrw.DownloadUpgradesWowhead(site, filter);
                if (docUpgradeSearch != null)
                {
                    // at this stage have an HTML doc that has upgrades in a <div class="listview-void"> block
                    // need to get the itemID list out and then load them and add to cache
                    int startpos = docUpgradeSearch.IndexOf("<div class=\"listview-void\">");
                    if (startpos > 1)
                    {
                        int endpos = docUpgradeSearch.IndexOf("</div>", startpos);
                        XDocument doc = new XDocument();
                        doc.InnerXml = docUpgradeSearch.Substring(startpos, endpos - startpos + 6);
                        List<XElement> nodeList = new List<XElement>(doc.SelectNodes("//a/@href"));

                        Regex toMatch = new Regex("(\\d{5})");
                        Match match;

                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            StatusMessaging.UpdateStatus("ImportWowheadFilter",
                                string.Format("Downloading definition {0} of {1} items", i, nodeList.Count));
                            //string id = nodeList[i].Value.Substring(7);
                            // This new code will let it find the item id without worrying
                            // about wowhead messing with the specifics of the string
                            match = toMatch.Match(nodeList[i].Value);
                            string id = match.Value;
                            {
                                Item item = GetItem(site, id, true);
                                if (item != null)
                                {
                                    ItemCache.AddItem(item, false);
                                }
                            }
                        }
                    }
                }
                else
                {
                    StatusMessaging.ReportError("ImportWowheadFilter", null, "No response returned from Wowhead");
                }
            }
            catch (Exception ex)
            {
                StatusMessaging.ReportError("ImportWowheadFilter", ex, "Error interpreting the data returned from Wowhead");
            }
        }

        private static void LoadUpgradesForSlot(Character character, CharacterSlot slot, Dictionary<ItemSlot, int> idealGems, bool usePTR, UpgradeCancelCheck cancel)
        {
            if (cancel != null && cancel())
                return;

            string docUpgradeSearch = null;
            try
            {
                string site = usePTR ? "ptr" : "www";
                StatusMessaging.UpdateStatus(slot.ToString(), "Downloading Upgrade List");
                ItemInstance itemToUpgrade = character[slot];
                if ((object)itemToUpgrade != null)
                {
                    WebRequestWrapper wrw = new WebRequestWrapper();
                    string minLevel = "minle=" + itemToUpgrade.Item.ItemLevel.ToString() + ";";
                    string filter = getWowheadSlotFilter(slot) + minLevel + getWowheadClassFilter(character.Class) +
                                    getWowheadWeightFilter(character);
                    docUpgradeSearch = wrw.DownloadUpgradesWowhead(site, filter);
                    ComparisonCalculationBase currentCalculation = Calculations.GetItemCalculations(itemToUpgrade, character, slot);
                    if (docUpgradeSearch != null)
                    {
                        // at this stage have an HTML doc that has upgrades in a <div class="listview-void"> block
                        // need to get the itemID list out and then load them and add to cache if better than itemToUpgrade
                        int startpos = docUpgradeSearch.IndexOf("<div class=\"listview-void\">");
                        if (startpos > 1)
                        {
                            int endpos = docUpgradeSearch.IndexOf("</div>", startpos);
                            XDocument doc = new XDocument();
                            doc.InnerXml = docUpgradeSearch.Substring(startpos, endpos - startpos + 6);
                            List<XElement> nodeList = new List<XElement>(doc.SelectNodes("//a/@href"));

                            for (int i = 0; i < nodeList.Count; i++)
                            {
                                if (cancel != null && cancel())
                                    break;

                                StatusMessaging.UpdateStatus(slot.ToString(), string.Format("Downloading definition {0} of {1} possible upgrades", i, nodeList.Count));
                                string id = nodeList[i].Value.Substring(7);
                                if (!ItemCache.Instance.ContainsItemId(int.Parse(id)))
                                {
                                    Item idealItem = GetItem(site, id, true);
                                    if (idealItem != null)
                                    {
                                        ItemInstance idealGemmedItem = new ItemInstance(int.Parse(id), idealGems[idealItem.SocketColor1], idealGems[idealItem.SocketColor2], idealGems[idealItem.SocketColor3], itemToUpgrade.EnchantId);

                                        Item newItem = ItemCache.AddItem(idealItem, false);

                                        //This is calling OnItemsChanged and ItemCache.Add further down the call stack so if we add it to the cache first, 
                                        // then do the compare and remove it if we don't want it, we can avoid that constant event trigger
                                        ComparisonCalculationBase upgradeCalculation = Calculations.GetItemCalculations(idealGemmedItem, character, slot);

                                        if (upgradeCalculation.OverallPoints < (currentCalculation.OverallPoints * .8f))
                                            ItemCache.DeleteItem(newItem, false);
                                    }
                                }
                            }
                        }
                    } else {
                        StatusMessaging.ReportError(slot.ToString(), null, "No response returned from Wowhead");
                    }
                }
                StatusMessaging.UpdateStatusFinished(slot.ToString());
            } catch (Exception ex) {
                StatusMessaging.ReportError(slot.ToString(), ex, "Error interpreting the data returned from Wowhead");
            }
        }
        */

        private static int StatValueSorter(ComparisonCalculationBase x, ComparisonCalculationBase y)
        {
            if (x.OverallPoints > y.OverallPoints)
                return -1;
            else if (x.OverallPoints < y.OverallPoints)
                return 1;
            else
                return 0;
        }

        private static string UrlEncode(string text)
        {
            // elitistarmory expect space to be encoded as %20
#if !RAWRSERVER
#if SILVERLIGHT
            return System.Windows.Browser.HttpUtility.UrlEncode(text).Replace("+", "%20");
#else
            return Utilities.UrlEncode(text).Replace("+", "%20");
#endif
#else
            return System.Web.HttpUtility.UrlEncode(text).Replace("+", "%20");
#endif
        }
    }

    /*public class ItemIdRequest
    {
        private readonly string itemName;
        public string ItemName { get { return itemName; } }

        private readonly Action<int> callback;
        public Action<int> Callback { get { return callback; } }

        public XDocument ItemSearch { get; set; }

        public int Result { get; set; }

        public void Invoke()
        {
            callback(Result);
        }

        public ItemIdRequest(string itemName, Action<int> callback)
        {
            this.itemName = itemName;
            this.callback = callback;
            GetItemId();
        }
        public void GetItemId()
        {
            new NetworkUtils(new EventHandler(ItemSearchReady)).DownloadItemSearch(ItemName);
        }

        private void ItemSearchReady(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;
            ItemSearch = network.Result;
            try
            {
                List<XElement> items_nodes = new List<XElement>(ItemSearch.SelectNodes("/wowhead/searchResults/items/item"));
                // we only want a single match, even if its not exact
                if (items_nodes.Count == 1)
                {
                    int id = Int32.Parse(items_nodes[0].Attribute("id").Value);
                    Result = id;
                }
                else
                {
                    // choose an exact match if it exists
                    foreach (XElement node in items_nodes)
                    {
                        if (node.Attribute("name").Value == itemName)
                        {
                            int id = Int32.Parse(items_nodes[0].Attribute("id").Value);
                            Result = id;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessaging.ReportError("Get Item", ex, "Rawr encountered an error getting Item Id from Armory: " + ItemName);
            }
            Invoke();
        }
    }*/

    public class ItemRequest
    {
        private readonly int id;
        public int Id { get { return id; } }

        private readonly Action<Item> callback;
        public Action<Item> Callback { get { return callback; } }

        public XDocument Tooltip { get; set; }
        public XDocument ItemInfo { get; set; }

        public Item Result { get; set; }

        public void Invoke()
        {
            callback(Result);
        }

        public ItemRequest(int id, Action<Item> callback)
        {
            this.id = id;
            this.callback = callback;
            GetItem();
        }
        public void GetItem()
        {
            new NetworkUtils(new EventHandler(ItemTooltipReady)).DownloadItemToolTipSheet(Id);
        }

        private void ItemTooltipReady(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;

            Tooltip = network.Result;
            network.DocumentReady -= new EventHandler(ItemTooltipReady);
            network.DocumentReady += new EventHandler(ItemInformationReady);
            network.DownloadItemInformation(Id);
        }

        /// <summary>
        /// This actually parses data, but it's set up for reading Armory right now and not Wowhead data
        /// Need to make it run the JsonParser class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemInformationReady(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;
            ItemInfo = network.Result;
            try
            {
                ItemLocation location = LocationFactory.CreateItemLocsFromXDoc(/*Tooltip,*/ ItemInfo, Id.ToString());

                if (Tooltip == null || Tooltip.SelectSingleNode("/wowhead/itemTooltips/htmlTooltip") == null)
                {
                    StatusMessaging.ReportError("Get Item", null, "No item returned from Wowhead for " + Id);
                    return;
                }

                #region Set Up Variables for Parsing
                ItemQuality quality = ItemQuality.Common;
                ItemType type = ItemType.None;
                ItemSlot socketColor1 = ItemSlot.None;
                ItemSlot socketColor2 = ItemSlot.None;
                ItemSlot socketColor3 = ItemSlot.None;
                Stats socketStats = new Stats();
                string name = string.Empty;
                string iconPath = string.Empty;
                string setName = string.Empty;
                ItemSlot slot = ItemSlot.None;
                Stats stats = new Stats();
                int inventoryType = -1;
                int classId = -1;
                string subclassName = string.Empty;
                int minDamage = 0;
                int maxDamage = 0;
                ItemDamageType damageType = ItemDamageType.Physical;
                float speed = 0f;
                List<string> requiredClasses = new List<string>();
                bool unique = false;
                int itemLevel = 0;
                #endregion

                #region Basic Item Info (Name, Type, etc)
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/name")) { name = node.Value; }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/icon")) { iconPath = node.Value; }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/maxCount")) { unique = node.Value == "1"; }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/overallQualityId")) { quality = (ItemQuality)Enum.Parse(typeof(ItemQuality), node.Value, false); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/classId")) { classId = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/equipData/inventoryType")) { inventoryType = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/equipData/subclassName")) { subclassName = node.Value; }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/min")) { minDamage = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/max")) { maxDamage = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/type")) { damageType = (ItemDamageType)Enum.Parse(typeof(ItemDamageType), node.Value, false); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/speed")) { speed = float.Parse(node.Value, System.Globalization.CultureInfo.InvariantCulture); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/setData/name")) { setName = node.Value; setName = setName.Replace("Vicious ", ""); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/allowableClasses/class")) { requiredClasses.Add(node.Value); }

                foreach (XAttribute attr in ItemInfo.SelectNodes("page/itemInfo/item").Attributes("level")) { itemLevel = int.Parse(attr.Value); }

                if (inventoryType >= 0)
                    slot = GetItemSlot(inventoryType, classId);
                if (!string.IsNullOrEmpty(subclassName))
                    type = GetItemType(subclassName, inventoryType, classId);
                #endregion

                #region Item Class Restriction Fixes
                // fix class restrictions on BOP items that can only be made by certain classes
                switch (Id)
                {
                    case 35181:
                    case 32495:
                        requiredClasses.Add("Priest");
                        break;
                    case 32476:
                    case 35184:
                    case 32475:
                    case 34355:
                        requiredClasses.Add("Shaman");
                        break;
                    case 32474:
                    case 34356:
                        requiredClasses.Add("Hunter");
                        break;
                    case 46106:
                    case 32479:
                    case 32480:
                    case 46109:
                        requiredClasses.Add("Druid");
                        break;
                    case 32478:
                    case 34353:
                        requiredClasses.Add("Druid");
                        requiredClasses.Add("Rogue");
                        break;
                }
                #endregion

                #region Item Stats
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusAgility")) { stats.Agility = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusAttackPower")) { stats.AttackPower = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/armor")) { stats.Armor = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusDodgeRating")) { stats.DodgeRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusParryRating")) { stats.ParryRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusBlockRating")) { stats.BlockRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusResilienceRating")) { stats.Resilience = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusStamina")) { stats.Stamina = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusIntellect")) { stats.Intellect = int.Parse(node.Value); }

                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusStrength")) { stats.Strength = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHitRating")) { stats.HitRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteRating")) { stats.HasteRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusCritRating")) { stats.CritRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusExpertiseRating")) { stats.ExpertiseRating = int.Parse(node.Value); }

                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/arcaneResist")) { stats.ArcaneResistance = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/fireResist")) { stats.FireResistance = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/frostResist")) { stats.FrostResistance = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/natureResist")) { stats.NatureResistance = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/shadowResist")) { stats.ShadowResistance = int.Parse(node.Value); }

                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusCritSpellRating")) { stats.CritRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHitSpellRating")) { stats.HitRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteSpellRating")) { stats.HasteRating = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusSpellPower")) { stats.SpellPower = int.Parse(node.Value); }

                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusMana")) { stats.Mana = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusSpirit")) { stats.Spirit = int.Parse(node.Value); }
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusManaRegen")) { stats.Mp5 = int.Parse(node.Value); }
                #endregion

                #region Item Armor vs Bonus Armor fix
                if (slot == ItemSlot.Finger ||
                    slot == ItemSlot.MainHand ||
                    slot == ItemSlot.Neck ||
                    (slot == ItemSlot.OffHand && type != ItemType.Shield) ||
                    slot == ItemSlot.OneHand ||
                    slot == ItemSlot.Trinket ||
                    slot == ItemSlot.TwoHand)
                {
                    stats.BonusArmor += stats.Armor;
                    stats.Armor = 0f;
                }

                if (slot == ItemSlot.Back)
                {
                    float baseArmor = 0;
                    switch (quality)
                    {
                        case ItemQuality.Temp:
                        case ItemQuality.Poor:
                        case ItemQuality.Common:
                        case ItemQuality.Uncommon:
                            baseArmor = (float)itemLevel * 1.19f + 5.1f;
                            break;

                        case ItemQuality.Rare:
                            baseArmor = ((float)itemLevel + 26.6f) * 16f / 25f;
                            break;

                        case ItemQuality.Epic:
                        case ItemQuality.Legendary:
                        case ItemQuality.Artifact:
                        case ItemQuality.Heirloom:
                            baseArmor = ((float)itemLevel + 358f) * 7f / 26f;
                            break;
                    }

                    baseArmor = (float)Math.Floor(baseArmor);
                    stats.BonusArmor = stats.Armor - baseArmor;
                    stats.Armor = baseArmor;
                }
                #endregion

                #region Item Special Effects
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/spellData/spell"))
                {
                    bool isEquip = false;
                    bool isUse = false;
                    string spellDesc = null;
                    foreach (XElement childNode in node.Elements())
                    {
                        if (childNode.Name == "trigger")
                        {
                            isEquip = childNode.Value == "1";
                            isUse = childNode.Value == "0";
                        }
                        if (childNode.Name == "desc")
                            spellDesc = childNode.Value;
                    }

                    //parse Use/Equip lines
                    if (isUse) SpecialEffects.ProcessUseLine(spellDesc, stats, true, itemLevel, Id);
                    if (isEquip) SpecialEffects.ProcessEquipLine(spellDesc, stats, true, itemLevel, Id);
                }
                #endregion

                #region Item Socket Info and Socket Bonus Stats
                List<XElement> socketNodes = new List<XElement>(Tooltip.SelectNodes("page/itemTooltips/itemTooltip/socketData/socket"));
                if (socketNodes.Count > 0) socketColor1 = (ItemSlot)Enum.Parse(typeof(ItemSlot), socketNodes[0].Attribute("color").Value, false);
                if (socketNodes.Count > 1) socketColor2 = (ItemSlot)Enum.Parse(typeof(ItemSlot), socketNodes[1].Attribute("color").Value, false);
                if (socketNodes.Count > 2) socketColor3 = (ItemSlot)Enum.Parse(typeof(ItemSlot), socketNodes[2].Attribute("color").Value, false);
                string socketBonusesString = string.Empty;
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/socketData/socketMatchEnchant")) { socketBonusesString = node.Value.Trim('+'); }
                if (!string.IsNullOrEmpty(socketBonusesString))
                {
                    try
                    {
                        List<string> socketBonuses = new List<string>();
                        string[] socketBonusStrings = socketBonusesString.Split(new string[] { " and ", " & ", ", " }, StringSplitOptions.None);
                        foreach (string socketBonusString in socketBonusStrings)
                        {
                            if (socketBonusString.LastIndexOf('+') > 2 && socketBonusString.LastIndexOf('+') < socketBonusString.Length - 3)
                            {
                                socketBonuses.Add(socketBonusString.Substring(0, socketBonusString.IndexOf(" +")));
                                socketBonuses.Add(socketBonusString.Substring(socketBonusString.IndexOf(" +") + 1));
                            }
                            else
                                socketBonuses.Add(socketBonusString);
                        }
                        foreach (string socketBonus in socketBonuses)
                        {
                            int socketBonusValue = 0;
                            if (socketBonus.IndexOf(' ') > 0) socketBonusValue = int.Parse(socketBonus.Substring(0, socketBonus.IndexOf(' ')));
                            switch (socketBonus.Substring(socketBonus.IndexOf(' ') + 1))
                            {
                                case "Agility":
                                    socketStats.Agility = socketBonusValue;
                                    break;
                                case "Stamina":
                                    socketStats.Stamina = socketBonusValue;
                                    break;
                                case "Dodge Rating":
                                    socketStats.DodgeRating = socketBonusValue;
                                    break;
                                case "Parry Rating":
                                    socketStats.ParryRating = socketBonusValue;
                                    break;
                                case "Block Rating":
                                    socketStats.BlockRating = socketBonusValue;
                                    break;
                                case "Hit Rating":
                                    socketStats.HitRating = socketBonusValue;
                                    break;
                                case "Haste Rating":
                                    socketStats.HasteRating = socketBonusValue;
                                    break;
                                case "Expertise Rating":
                                    socketStats.ExpertiseRating = socketBonusValue;
                                    break;
                                case "Strength":
                                    socketStats.Strength = socketBonusValue;
                                    break;
                                case "Healing":
                                    //case "Healing +4 Spell Damage":
                                    //case "Healing +3 Spell Damage":
                                    //case "Healing +2 Spell Damage":
                                    //case "Healing +1 Spell Damage":
                                    //case "Healing and +4 Spell Damage":
                                    //case "Healing and +3 Spell Damage":
                                    //case "Healing and +2 Spell Damage":
                                    //case "Healing and +1 Spell Damage":
                                    if (socketBonusValue == 0)
                                        socketStats.SpellPower = (float)Math.Round(int.Parse(socketBonuses[0].Substring(0, socketBonuses[0].IndexOf(' '))) / 1.88f);
                                    else
                                        socketStats.SpellPower = (float)Math.Round(socketBonusValue / 1.88f);
                                    break;
                                case "Spell Damage":
                                    // Only update Spell Damage if its not already set (Incase its an old heal bonus)
                                    if (socketStats.SpellPower == 0)
                                        socketStats.SpellPower = socketBonusValue;
                                    //sockets.Stats.Healing = socketBonusValue;
                                    break;
                                case "Spell Power":
                                    socketStats.SpellPower = socketBonusValue;
                                    break;
                                case "Crit Rating":
                                case "Crit Strike Rating":
                                case "Critical Rating":
                                case "Critical Strike Rating":
                                    socketStats.CritRating = socketBonusValue;
                                    break;
                                case "Attack Power":
                                    socketStats.AttackPower = socketBonusValue;
                                    break;
                                case "Weapon Damage":
                                    socketStats.WeaponDamage = socketBonusValue;
                                    break;
                                case "Resilience":
                                case "Resilience Rating":
                                    socketStats.Resilience = socketBonusValue;
                                    break;
                                //case "Spell Damage and Healing":
                                //    sockets.Stats.SpellDamageRating = socketBonusValue;
                                //    sockets.Stats.Healing = socketBonusValue;
                                //    break;
                                case "Spell Hit Rating":
                                    socketStats.HitRating = socketBonusValue;
                                    break;
                                case "Intellect":
                                    socketStats.Intellect = socketBonusValue;
                                    break;
                                case "Spell Crit":
                                case "Spell Crit Rating":
                                case "Spell Critical":
                                case "Spell Critical Rating":
                                case "Spell Critical Strike Rating":
                                    socketStats.CritRating = socketBonusValue;
                                    break;
                                case "Spell Haste Rating":
                                    socketStats.HasteRating = socketBonusValue;
                                    break;
                                case "Spirit":
                                    socketStats.Spirit = socketBonusValue;
                                    break;
                                case "Mana every 5 seconds":
                                case "Mana ever 5 Sec":
                                case "mana per 5 sec":
                                case "mana per 5 sec.":
                                case "Mana per 5 sec.":
                                case "Mana per 5 Seconds":
                                    socketStats.Mp5 = socketBonusValue;
                                    break;
                            }
                        }
                    }
                    catch { }
                }
                #endregion

                #region Gem Stats
                foreach (XElement nodeGemProperties in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/gemProperties"))
                {
                    List<string> gemBonuses = new List<string>();
                    string[] gemBonusStrings = nodeGemProperties.Value.Split(new string[] { " and ", " & ", ", " }, StringSplitOptions.None);
                    foreach (string gemBonusString in gemBonusStrings)
                    {
                        if (gemBonusString.IndexOf('+') != gemBonusString.LastIndexOf('+'))
                        {
                            gemBonuses.Add(gemBonusString.Substring(0, gemBonusString.IndexOf(" +")));
                            gemBonuses.Add(gemBonusString.Substring(gemBonusString.IndexOf(" +") + 1));
                        }
                        else
                            gemBonuses.Add(gemBonusString);
                    }
                    foreach (string gemBonus in gemBonuses)
                    {
                        if (gemBonus == "Spell Damage +6")
                        {
                            stats.SpellPower = 6.0f;
                        }
                        else if (gemBonus == "2% Increased Armor Value from Items")
                        {
                            stats.BaseArmorMultiplier = 0.02f;
                        }
                        else if (gemBonus == "Stamina +6")
                        {
                            stats.Stamina = 6.0f;
                        }
                        else if (gemBonus == "Chance to restore mana on spellcast")
                        {
                            stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { ManaRestore = 300 }, 0f, 15f, .05f));
                        }
                        else if (gemBonus == "Chance on spellcast - next spell cast in half time" || gemBonus == "Chance to Increase Spell Cast Speed")
                        {
                            stats.AddSpecialEffect(new SpecialEffect(Trigger.SpellCast, new Stats() { HasteRating = 320 }, 6, 45, 0.15f));
                        }
                        else if (gemBonus == "+10% Shield Block Value")
                        {
                            stats.BonusBlockValueMultiplier = 0.1f;
                        }
                        else if (gemBonus == "+2% Intellect")
                        {
                            stats.BonusIntellectMultiplier = 0.02f;
                        }
                        else if (gemBonus == "2% Reduced Threat")
                        {
                            stats.ThreatReductionMultiplier = 0.02f;
                        }
                        else if (gemBonus == "3% Increased Critical Healing Effect")
                        {
                            stats.BonusCritHealMultiplier = 0.03f;
                        }
                        else if (gemBonus == "10% Silence Duration Reduction")
                        {
                            stats.SilenceDurReduc = 0.10f;
                        }
                        else if (gemBonus == "2% Maximum Mana")
                        {
                            stats.BonusManaMultiplier = 0.02f;
                        }
                        else
                        {
                            try
                            {
                                int gemBonusValue = int.Parse(gemBonus.Substring(0, gemBonus.IndexOf(' ')).Trim('+').Trim('%'));
                                switch (gemBonus.Substring(gemBonus.IndexOf(' ') + 1).Trim())
                                {
                                    case "to All Stats":
                                    case "All Stats":
                                        stats.Agility = gemBonusValue;
                                        stats.Strength = gemBonusValue;
                                        stats.Stamina = gemBonusValue;
                                        stats.Intellect = gemBonusValue;
                                        stats.Spirit = gemBonusValue;
                                        break;
                                    case "Resist All":
                                        stats.ArcaneResistance = gemBonusValue;
                                        stats.FireResistance = gemBonusValue;
                                        stats.FrostResistance = gemBonusValue;
                                        stats.NatureResistance = gemBonusValue;
                                        stats.ShadowResistance = gemBonusValue;
                                        break;
                                    case "Increased Critical Damage":
                                        stats.BonusCritDamageMultiplier = (float)gemBonusValue / 100f;
                                        stats.BonusSpellCritDamageMultiplier = (float)gemBonusValue / 100f; // both melee and spell crit use the same text, would have to disambiguate based on other stats
                                        break;
                                    case "Agility":
                                        stats.Agility = gemBonusValue;
                                        break;
                                    case "Stamina":
                                        stats.Stamina = gemBonusValue;
                                        break;
                                    case "Dodge Rating":
                                        stats.DodgeRating = gemBonusValue;
                                        break;
                                    case "Parry Rating":
                                        stats.ParryRating = gemBonusValue;
                                        break;
                                    case "Block Rating":
                                        stats.BlockRating = gemBonusValue;
                                        break;
                                    case "Hit Rating":
                                        stats.HitRating = gemBonusValue;
                                        break;
                                    case "Haste Rating":
                                        stats.HasteRating = gemBonusValue;
                                        break;
                                    case "Expertise Rating":
                                        stats.ExpertiseRating = gemBonusValue;
                                        break;
                                    case "Strength":
                                        stats.Strength = gemBonusValue;
                                        break;
                                    case "Crit Rating":
                                    case "Crit Strike Rating":
                                    case "Critical Rating":
                                    case "Critical Strike Rating":
                                        stats.CritRating = gemBonusValue;
                                        break;
                                    case "Attack Power":
                                        stats.AttackPower = gemBonusValue;
                                        break;
                                    case "Weapon Damage":
                                        stats.WeaponDamage = gemBonusValue;
                                        break;
                                    case "Resilience":
                                    case "Resilience Rating":
                                        stats.Resilience = gemBonusValue;
                                        break;
                                    case "Spell Hit Rating":
                                        stats.HitRating = gemBonusValue;
                                        break;
                                    case "Spell Haste Rating":
                                        stats.HasteRating = gemBonusValue;
                                        break;
                                    case "Spell Damage":
                                        // Ignore spell damage from gem if Healing has already been applied, as it might be a "9 Healing 3 Spell" gem. 
                                        if (stats.SpellPower == 0)
                                            stats.SpellPower = gemBonusValue;
                                        break;
                                    case "Spell Damage and Healing":
                                        stats.SpellPower = gemBonusValue;
                                        break;
                                    case "Healing":
                                        stats.SpellPower = (float)Math.Round(gemBonusValue / 1.88f);
                                        break;
                                    case "Spell Power":
                                        stats.SpellPower = gemBonusValue;
                                        break;
                                    case "Spell Crit":
                                    case "Spell Crit Rating":
                                    case "Spell Critical":
                                    case "Spell Critical Rating":
                                        stats.CritRating = gemBonusValue;
                                        break;
                                    case "Mana every 5 seconds":
                                    case "Mana ever 5 Sec":
                                    case "mana per 5 sec":
                                    case "mana per 5 sec.":
                                    case "Mana per 5 Seconds":
                                        stats.Mp5 = gemBonusValue;
                                        break;
                                    case "Intellect":
                                        stats.Intellect = gemBonusValue;
                                        break;
                                    case "Spirit":
                                        stats.Spirit = gemBonusValue;
                                        break;
                                }
                            }
                            catch { }
                        }
                    }
                }
                #endregion

                #region Gem Socket Color
                string desc = string.Empty;
                foreach (XElement node in Tooltip.SelectNodes("page/itemTooltips/itemTooltip/desc")) { desc = node.Value; }
                if (desc.Contains("Matches any socket"))
                {
                    slot = ItemSlot.Prismatic;
                }
                else if (desc.ToLower().Contains("cogwheel"))
                {
                    slot = ItemSlot.Cogwheel;
                }
                else if (desc.ToLower().Contains("hydraulic"))
                {
                    slot = ItemSlot.Hydraulic;
                }
                else if (desc.Contains("Matches a "))
                {
                    bool red = desc.Contains("Red");
                    bool blue = desc.Contains("Blue");
                    bool yellow = desc.Contains("Yellow");
                    slot = red && blue && yellow ? ItemSlot.Prismatic :
                        red && blue ? ItemSlot.Purple :
                        blue && yellow ? ItemSlot.Green :
                        red && yellow ? ItemSlot.Orange :
                        red ? ItemSlot.Red :
                        blue ? ItemSlot.Blue :
                        yellow ? ItemSlot.Yellow :
                        ItemSlot.None;
                }
                else if (desc.Contains("meta gem slot"))
                    slot = ItemSlot.Meta;
                #endregion

                #region Produce the Item
                Item item = new Item()
                {
                    Id = Id,
                    Name = name,
                    Quality = quality,
                    Type = type,
                    IconPath = iconPath,
                    Slot = slot,
                    SetName = setName,
                    Stats = stats,
                    SocketColor1 = socketColor1,
                    SocketColor2 = socketColor2,
                    SocketColor3 = socketColor3,
                    SocketBonus = socketStats,
                    MinDamage = minDamage,
                    MaxDamage = maxDamage,
                    DamageType = damageType,
                    Speed = speed,
                    RequiredClasses = string.Join("|", requiredClasses.ToArray()),
                    Unique = unique,
                    ItemLevel = itemLevel,
                };
                Result = item;
                #endregion
            } catch (Exception ex) {
                StatusMessaging.ReportError("Get Item", ex,
                    string.Format("Rawr encountered an error getting Item from Wowhead: {0}", Id));
            }
            Invoke();
        }

        private static ItemType GetItemType(string subclassName, int inventoryType, int classId)
        {
            switch (subclassName)
            {
                case "Cloth":       return ItemType.Cloth;
                case "Leather":     return ItemType.Leather;
                case "Mail":        return ItemType.Mail;
                case "Plate":       return ItemType.Plate;
                case "Dagger":      return ItemType.Dagger;
                case "Fist Weapon": return ItemType.FistWeapon;
                case "Axe":         return (inventoryType == 17 ? ItemType.TwoHandAxe   : ItemType.OneHandAxe  );
                case "Mace":        return (inventoryType == 17 ? ItemType.TwoHandMace  : ItemType.OneHandMace );
                case "Sword":       return (inventoryType == 17 ? ItemType.TwoHandSword : ItemType.OneHandSword);
                case "Polearm":     return ItemType.Polearm;
                case "Staff":       return ItemType.Staff;
                case "Shield":      return ItemType.Shield;
                case "Bow":         return ItemType.Bow;
                case "Crossbow":    return ItemType.Crossbow;
                case "Gun":         return ItemType.Gun;
                case "Wand":        return ItemType.Wand;
                case "Thrown":      return ItemType.Thrown;
                case "Arrow":       return ItemType.Arrow;
                case "Bullet":      return ItemType.Bullet;
                case "Quiver":      return ItemType.Quiver;
                case "Ammo Pouch":  return ItemType.AmmoPouch;
                case "Idol":        //return ItemType.Idol;
                case "Libram":      //return ItemType.Libram;
                case "Totem":       //return ItemType.Totem;
                case "Sigil":       //return ItemType.Sigil;
                case "Relic":       return ItemType.Relic;
                default:            return ItemType.None;
            }
        }

        private static ItemSlot GetItemSlot(int inventoryType, int classId)
        {
            switch (classId) {
                case 6: return ItemSlot.Projectile;
                case 11: return ItemSlot.ProjectileBag;
            }
            switch (inventoryType) {
                case  1: return ItemSlot.Head;
                case  2: return ItemSlot.Neck;
                case  3: return ItemSlot.Shoulders;
                case 16: return ItemSlot.Back;
                case  5: case 20: return ItemSlot.Chest;
                case  4: return ItemSlot.Shirt;
                case 19: return ItemSlot.Tabard;
                case  9: return ItemSlot.Wrist;
                case 10: return ItemSlot.Hands;
                case  6: return ItemSlot.Waist;
                case  7: return ItemSlot.Legs;
                case  8: return ItemSlot.Feet;
                case 11: return ItemSlot.Finger;
                case 12: return ItemSlot.Trinket;
                case 13: return ItemSlot.OneHand;
                case 17: return ItemSlot.TwoHand;
                case 21: return ItemSlot.MainHand;
                case 14: case 22: case 23: return ItemSlot.OffHand;
                case 15: case 25: case 26: case 28: return ItemSlot.Ranged;
                case 24: return ItemSlot.Projectile;
                case 27: return ItemSlot.ProjectileBag;
                default: return ItemSlot.None;
            }
        }
    }
}
