using System;
using System.Net;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Rawr
{
    public class Armory
    {
        private static Dictionary<NetworkUtils, CharacterRequestContext> CharacterContexts;
        private static Dictionary<NetworkUtils, ItemRequestContext> ItemContexts;

        static Armory()
        {
            CharacterContexts = new Dictionary<NetworkUtils, CharacterRequestContext>();
            ItemContexts = new Dictionary<NetworkUtils, ItemRequestContext>();
        }

        #region Character Loading
        public static void GetCharacter(string name, string realm, Character.CharacterRegion region, Action<Character> callback)
        {
            StatusMessaging.UpdateStatus("Get Character From Armory", "Downloading Character Definition");
            StatusMessaging.UpdateStatus("Get Talent Tree From Armory", "Queued");

            CharacterRequestContext context = new CharacterRequestContext(name, realm, region, callback);
            NetworkUtils network = new NetworkUtils(CharacterSheet_Completed);
            CharacterContexts[network] = context;

            network.GetCharacterSheetDocument(name, realm, region);
        }

        private static void CharacterSheet_Completed(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;
            CharacterRequestContext context = CharacterContexts[network];

            context.CharacterSheet = network.Result;
            XDocument docCharacter = context.CharacterSheet;

            if (docCharacter == null || docCharacter.Element("page") == null
                || docCharacter.Element("page").Element("characterInfo") == null
                || docCharacter.Element("page").Element("characterInfo").Attribute("errCode") != null)
            {
                StatusMessaging.UpdateStatusFinished("Get Character From Armory");
                StatusMessaging.ReportError("Get Character From Armory", null, "No character returned from the Armory");
                context.Result = null;
                context.Invoke();
                CharacterContexts.Remove(network);
                return;
            }

            XElement characterInfo = docCharacter.Element("page").Element("characterInfo");
            StatusMessaging.UpdateStatus("Get Character From Armory", "Processing Character Definition");

            XElement characterElement = characterInfo.Element("character");
            Character.CharacterRace race = (Character.CharacterRace)int.Parse(characterElement.Attribute("raceId").Value);
            Character.CharacterClass charClass = (Character.CharacterClass)int.Parse(characterElement.Attribute("classId").Value);
            string name = characterElement.Attribute("name").Value;
            string realm = characterElement.Attribute("realm").Value;

            Dictionary<Character.CharacterSlot, string> items = new Dictionary<Character.CharacterSlot, string>();
            foreach (XElement itemNode in characterInfo.Element("characterTab").Element("items").Elements("item"))
            {
                int slot = int.Parse(itemNode.Attribute("slot").Value) + 1;
                Character.CharacterSlot cslot = Character.GetCharacterSlotFromId(slot);
                items[cslot] = string.Format("{0}.{1}.{2}.{3}.{4}", itemNode.Attribute("id").Value,
                    itemNode.Attribute("gem0Id").Value, itemNode.Attribute("gem1Id").Value, itemNode.Attribute("gem2Id").Value,
                    itemNode.Attribute("permanentenchant").Value);
            }

            string[] ItemsOnCharacterResult = new string[items.Values.Count];
            items.Values.CopyTo(ItemsOnCharacterResult, 0);
            context.Result = new Character(name, realm, context.Region, race,
                items.ContainsKey(Character.CharacterSlot.Head) ? items[Character.CharacterSlot.Head] : null,
                items.ContainsKey(Character.CharacterSlot.Neck) ? items[Character.CharacterSlot.Neck] : null,
                items.ContainsKey(Character.CharacterSlot.Shoulders) ? items[Character.CharacterSlot.Shoulders] : null,
                items.ContainsKey(Character.CharacterSlot.Back) ? items[Character.CharacterSlot.Back] : null,
                items.ContainsKey(Character.CharacterSlot.Chest) ? items[Character.CharacterSlot.Chest] : null,
                items.ContainsKey(Character.CharacterSlot.Shirt) ? items[Character.CharacterSlot.Shirt] : null,
                items.ContainsKey(Character.CharacterSlot.Tabard) ? items[Character.CharacterSlot.Tabard] : null,
                items.ContainsKey(Character.CharacterSlot.Wrist) ? items[Character.CharacterSlot.Wrist] : null,
                items.ContainsKey(Character.CharacterSlot.Hands) ? items[Character.CharacterSlot.Hands] : null,
                items.ContainsKey(Character.CharacterSlot.Waist) ? items[Character.CharacterSlot.Waist] : null,
                items.ContainsKey(Character.CharacterSlot.Legs) ? items[Character.CharacterSlot.Legs] : null,
                items.ContainsKey(Character.CharacterSlot.Feet) ? items[Character.CharacterSlot.Feet] : null,
                items.ContainsKey(Character.CharacterSlot.Finger1) ? items[Character.CharacterSlot.Finger1] : null,
                items.ContainsKey(Character.CharacterSlot.Finger2) ? items[Character.CharacterSlot.Finger2] : null,
                items.ContainsKey(Character.CharacterSlot.Trinket1) ? items[Character.CharacterSlot.Trinket1] : null,
                items.ContainsKey(Character.CharacterSlot.Trinket2) ? items[Character.CharacterSlot.Trinket2] : null,
                items.ContainsKey(Character.CharacterSlot.MainHand) ? items[Character.CharacterSlot.MainHand] : null,
                items.ContainsKey(Character.CharacterSlot.OffHand) ? items[Character.CharacterSlot.OffHand] : null,
                items.ContainsKey(Character.CharacterSlot.Ranged) ? items[Character.CharacterSlot.Ranged] : null,
                items.ContainsKey(Character.CharacterSlot.Projectile) ? items[Character.CharacterSlot.Projectile] : null,
                null);
            context.Result.Class = charClass;
            InitializeAvailableItemList(context);
            ApplyRacialandProfessionBuffs(context);

            StatusMessaging.UpdateStatus("Get Talent Tree From Armory", "Downloading Talent Tree");
            StatusMessaging.UpdateStatusFinished("Get Character From Armory");
            network = new NetworkUtils(TalentTree_Completed);
            network.GetTalentTreeDocument(context.Name, context.Realm, context.Region);
        }

        private static void TalentTree_Completed(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;
            CharacterRequestContext context = CharacterContexts[network];

            context.TalentTree = network.Result;
            XDocument talentsDoc = context.TalentTree;

            XElement activeGroup = null;
            StatusMessaging.UpdateStatus("Get Talent Tree From Armory", "Processing Talent Tree");
            foreach(XElement group in talentsDoc.Element("page").Element("characterInfo")
                .Element("talentGroups").Elements("talentGroup"))
            {
                if (group.Attribute("active") != null) activeGroup = group;
            }
            string talentCode = activeGroup.Element("talentSpec").Attribute("value").Value;
            switch (context.Result.Class)
            {
                case Character.CharacterClass.Warrior:
                    context.Result.WarriorTalents = new WarriorTalents(talentCode);
                    if (context.Result.WarriorTalents.Devastate > 0) context.Result.CurrentModel = "ProtWarr";
                    else context.Result.CurrentModel = "DPSWarr";
                    break;
                case Character.CharacterClass.Paladin:
                    context.Result.PaladinTalents = new PaladinTalents(talentCode);
                    if (context.Result.PaladinTalents.HolyShield > 0) context.Result.CurrentModel = "ProtPaladin";
                    else if (context.Result.PaladinTalents.CrusaderStrike > 0) context.Result.CurrentModel = "Retribution";
                    else context.Result.CurrentModel = "Healadin";
                    break;
                case Character.CharacterClass.Hunter:
                    context.Result.HunterTalents = new HunterTalents(talentCode);
                    context.Result.CurrentModel = "Hunter";
                    break;
                case Character.CharacterClass.Rogue:
                    context.Result.RogueTalents = new RogueTalents(talentCode);
                    context.Result.CurrentModel = "Rogue";
                    break;
                case Character.CharacterClass.Priest:
                    context.Result.PriestTalents = new PriestTalents(talentCode);
                    if (context.Result.PriestTalents.Shadowform > 0) context.Result.CurrentModel = "ShadowPriest";
                    else context.Result.CurrentModel = "HolyPriest";
                    break;
                case Character.CharacterClass.Shaman:
                    context.Result.ShamanTalents = new ShamanTalents(talentCode);
                    if (context.Result.ShamanTalents.ElementalMastery > 0) context.Result.CurrentModel = "Elemental";
                    else if (context.Result.ShamanTalents.Stormstrike > 0) context.Result.CurrentModel = "Enhance";
                    else context.Result.CurrentModel = "RestoSham";
                    break;
                case Character.CharacterClass.Mage:
                    context.Result.MageTalents = new MageTalents(talentCode);
                    context.Result.CurrentModel = "Mage";
                    break;
                case Character.CharacterClass.Warlock:
                    context.Result.WarlockTalents = new WarlockTalents(talentCode);
                    context.Result.CurrentModel = "Warlock";
                    break;
                case Character.CharacterClass.Druid:
                    context.Result.DruidTalents = new DruidTalents(talentCode);
                    if (context.Result.DruidTalents.ProtectorOfThePack > 0) context.Result.CurrentModel = "Bear";
                    else if (context.Result.DruidTalents.LeaderOfThePack > 0) context.Result.CurrentModel = "Cat";
                    else if (context.Result.DruidTalents.MoonkinForm > 0) context.Result.CurrentModel = "Moonkin";
                    else context.Result.CurrentModel = "Tree";
                    break;
                case Character.CharacterClass.DeathKnight:
                    context.Result.DeathKnightTalents = new DeathKnightTalents(talentCode);
                    if (context.Result.DeathKnightTalents.Anticipation > 0) context.Result.CurrentModel = "TankDK";
                    else context.Result.CurrentModel = "DPSDK";
                    break;
                default:
                    break;
            }
            TalentsBase talents = context.Result.CurrentTalents;
            Dictionary<string, PropertyInfo> glyphProperty = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo pi in talents.GetType().GetProperties())
            {
                GlyphDataAttribute[] glyphDatas = pi.GetCustomAttributes(typeof(GlyphDataAttribute), true) as GlyphDataAttribute[];
                if (glyphDatas.Length > 0)
                {
                    GlyphDataAttribute glyphData = glyphDatas[0];
                    glyphProperty[glyphData.Name] = pi;
                }
            }

            foreach (XElement glyph in activeGroup.Element("glyphs").Elements("glyph"))
            {
                PropertyInfo pi;
                if (glyphProperty.TryGetValue(glyph.Attribute("name").Value, out pi))
                {
                    pi.SetValue(talents, true, null);
                }
            }

            StatusMessaging.UpdateStatusFinished("Get Talent Tree From Armory");
            context.Invoke();
            CharacterContexts.Remove(network);
        }

        /// <summary>
        /// 09.01.01 - TankConcrete
        /// Sets up the initial list of gear that the toon has available. Method looks the
        /// gear that is currently equipped and adds it to the "AvailableItems" list if it
        /// is not already there.
        /// 
        /// If a new item is added to the "AvailableItems" list, the "unsaved" changes flag
        /// will be updated so the user knows to save.
        /// </summary>
        /// <param name="currentChar"></param>
        private static void InitializeAvailableItemList(CharacterRequestContext context)
        {
            Character characterResult = context.Result;
            for (Character.CharacterSlot slot = 0; slot < (Character.CharacterSlot)19; slot++)
            {
                ItemInstance item = characterResult[slot];
                if ((object)item != null && item.Id != 0)
                {
                    if (!characterResult.AvailableItems.Contains(item.Id.ToString())) characterResult.AvailableItems.Add(item.Id.ToString());
                    Enchant enchant = item.Enchant;
                    if (enchant != null && enchant.Id != 0)
                    {
                        string enchantString = (-1 * (enchant.Id + (10000 * (int)enchant.Slot))).ToString();
                        if (!characterResult.AvailableItems.Contains(enchantString)) characterResult.AvailableItems.Add(enchantString);
                    }
                }
            }
        }

        private static void ApplyRacialandProfessionBuffs(CharacterRequestContext context)
        {
            XDocument doc = context.CharacterSheet;
            Character characterResult = context.Result;
            if (characterResult.Race == Character.CharacterRace.Draenei) characterResult.ActiveBuffs.Add(Buff.GetBuffByName("Heroic Presence"));

            foreach (XElement profession in doc.Element("page").Element("characterInfo")
                .Element("characterTab").Element("professions").Elements("skill"))
            {   // apply profession buffs if max skill
                if (profession.Attribute("name").Value == "Mining" && profession.Attribute("value").Value == "450")
                    characterResult.ActiveBuffs.Add(Buff.GetBuffByName("Toughness"));
                if (profession.Attribute("name").Value == "Skinning" && profession.Attribute("value").Value == "450")
                    characterResult.ActiveBuffs.Add(Buff.GetBuffByName("Master of Anatomy"));
                if (profession.Attribute("name").Value == "Blacksmithing" && int.Parse(profession.Attribute("value").Value) >= 400)
                {
                    characterResult.WristBlacksmithingSocketEnabled = true;
                    characterResult.HandsBlacksmithingSocketEnabled = true;
                }
            }

            Calculations.GetModel(characterResult.CurrentModel).SetDefaults(characterResult);
        }
        #endregion

        #region Item Loading
        public static void GetItem(int id, Action<Item> callback) { GetItem(id, "Unknown reason", callback); }
        public static void GetItem(int id, string logReason, Action<Item> callback)
        {
            ItemRequestContext context = new ItemRequestContext(id, callback);
            NetworkUtils network = new NetworkUtils(new EventHandler(ItemTooltipReady));
            ItemContexts[network] = context;

            network.DownloadItemToolTipSheet(context.Id);
        }

        private static void ItemTooltipReady(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;
            ItemRequestContext context = ItemContexts[network];

            context.Tooltip = network.Result;
            network.DocumentReady -= new EventHandler(ItemTooltipReady);
            network.DocumentReady += new EventHandler(ItemInformationReady);
            network.DownloadItemInformation(context.Id);
        }

        private static void ItemInformationReady(object sender, EventArgs e)
        {
            NetworkUtils network = sender as NetworkUtils;
            ItemRequestContext context = ItemContexts[network];
            context.ItemInfo = network.Result;
            try
            {
                ItemLocation location = LocationFactory.Create(context.Tooltip, context.ItemInfo, context.Id.ToString());

                if (context.Tooltip == null || context.Tooltip.SelectSingleNode("/page/itemTooltips/itemTooltip") == null)
                {
                    StatusMessaging.ReportError("Get Item", null, "No item returned from Armory for " + context.Id);
                    return;
                }

                Item.ItemQuality quality = Item.ItemQuality.Common;
                Item.ItemType type = Item.ItemType.None;
                Item.ItemSlot socketColor1 = Item.ItemSlot.None;
                Item.ItemSlot socketColor2 = Item.ItemSlot.None;
                Item.ItemSlot socketColor3 = Item.ItemSlot.None;
                Stats socketStats = new Stats();
                string name = string.Empty;
                string iconPath = string.Empty;
                string setName = string.Empty;
                Item.ItemSlot slot = Item.ItemSlot.None;
                Stats stats = new Stats();
                int inventoryType = -1;
                int classId = -1;
                string subclassName = string.Empty;
                int minDamage = 0;
                int maxDamage = 0;
                Item.ItemDamageType damageType = Item.ItemDamageType.Physical;
                float speed = 0f;
                List<string> requiredClasses = new List<string>();
                bool unique = false;
                int itemLevel = 0;

                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/name")) { name = node.Value; }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/icon")) { iconPath = node.Value; }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/maxCount")) { unique = node.Value == "1"; }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/overallQualityId")) { quality = (Item.ItemQuality)Enum.Parse(typeof(Item.ItemQuality), node.Value, false); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/classId")) { classId = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/equipData/inventoryType")) { inventoryType = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/equipData/subclassName")) { subclassName = node.Value; }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/min")) { minDamage = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/max")) { maxDamage = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/damage/type")) { damageType = (Item.ItemDamageType)Enum.Parse(typeof(Item.ItemDamageType), node.Value, false); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/damageData/speed")) { speed = float.Parse(node.Value, System.Globalization.CultureInfo.InvariantCulture); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/setData/name")) { setName = node.Value; }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/allowableClasses/class")) { requiredClasses.Add(node.Value); }

                foreach (XAttribute attr in context.ItemInfo.SelectNodes("page/itemInfo/item").Attributes("level")) { itemLevel = int.Parse(attr.Value); }

                if (inventoryType >= 0)
                    slot = GetItemSlot(inventoryType, classId);
                if (!string.IsNullOrEmpty(subclassName))
                    type = GetItemType(subclassName, inventoryType, classId);

                /* fix class restrictions on BOP items that can only be made by certain classes */
                switch (context.Id)
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

                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusAgility")) { stats.Agility = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusAttackPower")) { stats.AttackPower = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/armor")) { stats.Armor = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusDefenseSkillRating")) { stats.DefenseRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusDodgeRating")) { stats.DodgeRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusParryRating")) { stats.ParryRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusBlockRating")) { stats.BlockRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusBlockValue")) { stats.BlockValue = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/blockValue")) { stats.BlockValue = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusResilienceRating")) { stats.Resilience = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusStamina")) { stats.Stamina = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusIntellect")) { stats.Intellect = int.Parse(node.Value); }

                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusStrength")) { stats.Strength = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHitRating")) { stats.HitRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteRating")) { stats.HasteRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusCritRating")) { stats.CritRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusExpertiseRating")) { stats.ExpertiseRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusCritMeleeRating")) { stats.CritMeleeRating = int.Parse(node.Value); }

                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/arcaneResist")) { stats.ArcaneResistance = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/fireResist")) { stats.FireResistance = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/frostResist")) { stats.FrostResistance = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/natureResist")) { stats.NatureResistance = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/shadowResist")) { stats.ShadowResistance = int.Parse(node.Value); }

                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusCritSpellRating")) { stats.CritRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHitSpellRating")) { stats.HitRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusHasteSpellRating")) { stats.HasteRating = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusSpellPower")) { stats.SpellPower = int.Parse(node.Value); }

                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusMana")) { stats.Mana = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusSpirit")) { stats.Spirit = int.Parse(node.Value); }
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/bonusManaRegen")) { stats.Mp5 = int.Parse(node.Value); }

                if (slot == Item.ItemSlot.Finger ||
                    slot == Item.ItemSlot.MainHand ||
                    slot == Item.ItemSlot.Neck ||
                    (slot == Item.ItemSlot.OffHand && type != Item.ItemType.Shield) ||
                    slot == Item.ItemSlot.OneHand ||
                    slot == Item.ItemSlot.Trinket ||
                    slot == Item.ItemSlot.TwoHand)
                {
                    stats.BonusArmor += stats.Armor;
                    stats.Armor = 0f;
                }

                if (slot == Item.ItemSlot.Back)
                {
                    float baseArmor = 0;
                    switch (quality)
                    {
                        case Item.ItemQuality.Temp:
                        case Item.ItemQuality.Poor:
                        case Item.ItemQuality.Common:
                        case Item.ItemQuality.Uncommon:
                            baseArmor = (float)itemLevel * 1.19f + 5.1f;
                            break;

                        case Item.ItemQuality.Rare:
                            baseArmor = ((float)itemLevel + 26.6f) * 16f / 25f;
                            break;

                        case Item.ItemQuality.Epic:
                        case Item.ItemQuality.Legendary:
                        case Item.ItemQuality.Artifact:
                        case Item.ItemQuality.Heirloom:
                            baseArmor = ((float)itemLevel + 358f) * 7f / 26f;
                            break;
                    }

                    baseArmor = (float)Math.Floor(baseArmor);
                    stats.BonusArmor = stats.Armor - baseArmor;
                    stats.Armor = baseArmor;
                }

                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/spellData/spell"))
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
                    if (isUse) SpecialEffects.ProcessUseLine(spellDesc, stats, true, context.Id);
                    if (isEquip) SpecialEffects.ProcessEquipLine(spellDesc, stats, true);
                }

                List<XElement> socketNodes = new List<XElement>(context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/socketData/socket"));
                if (socketNodes.Count > 0) socketColor1 = (Item.ItemSlot)Enum.Parse(typeof(Item.ItemSlot), socketNodes[0].Attribute("color").Value, false);
                if (socketNodes.Count > 1) socketColor2 = (Item.ItemSlot)Enum.Parse(typeof(Item.ItemSlot), socketNodes[1].Attribute("color").Value, false);
                if (socketNodes.Count > 2) socketColor3 = (Item.ItemSlot)Enum.Parse(typeof(Item.ItemSlot), socketNodes[2].Attribute("color").Value, false);
                string socketBonusesString = string.Empty;
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/socketData/socketMatchEnchant")) { socketBonusesString = node.Value.Trim('+'); }
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
                                case "Block Value":
                                    socketStats.BlockValue = socketBonusValue;
                                    break;
                                case "Defense Rating":
                                    socketStats.DefenseRating = socketBonusValue;
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
                                case "Armor Penetration Rating":
                                    socketStats.ArmorPenetrationRating = socketBonusValue;
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
                foreach (XElement nodeGemProperties in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/gemProperties"))
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
                            stats.ManaRestoreOnCast_5_15 = 300; // IED
                        }
                        else if (gemBonus == "Chance on spellcast - next spell cast in half time" || gemBonus == "Chance to Increase Spell Cast Speed")
                        {
                            stats.SpellHasteFor6SecOnCast_15_45 = 320; // MSD changed in 2.4
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
                                        stats.BonusCritMultiplier = (float)gemBonusValue / 100f;
                                        stats.BonusSpellCritMultiplier = (float)gemBonusValue / 100f; // both melee and spell crit use the same text, would have to disambiguate based on other stats
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
                                    case "Defense Rating":
                                        stats.DefenseRating = gemBonusValue;
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
                                    case "Armor Penetration Rating":
                                        stats.ArmorPenetrationRating = gemBonusValue;
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
                string desc = string.Empty;
                foreach (XElement node in context.Tooltip.SelectNodes("page/itemTooltips/itemTooltip/desc")) { desc = node.Value; }
                if (desc.Contains("Matches any socket"))
                {
                    slot = Item.ItemSlot.Prismatic;
                }
                else if (desc.Contains("Matches a "))
                {
                    bool red = desc.Contains("Red");
                    bool blue = desc.Contains("Blue");
                    bool yellow = desc.Contains("Yellow");
                    slot = red && blue && yellow ? Item.ItemSlot.Prismatic :
                        red && blue ? Item.ItemSlot.Purple :
                        blue && yellow ? Item.ItemSlot.Green :
                        red && yellow ? Item.ItemSlot.Orange :
                        red ? Item.ItemSlot.Red :
                        blue ? Item.ItemSlot.Blue :
                        yellow ? Item.ItemSlot.Yellow :
                        Item.ItemSlot.None;
                }
                else if (desc.Contains("meta gem slot"))
                    slot = Item.ItemSlot.Meta;

                Item item = new Item()
                {
                    Id = context.Id,
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
                context.Result = item;
            }
            catch (Exception ex)
            {
                StatusMessaging.ReportError("Get Item", ex, "Rawr encountered an error getting Item from Armory: " + context.Id);
            }
            context.Invoke();
            ItemContexts.Remove(network);
        }

	private static Item.ItemType GetItemType(string subclassName, int inventoryType, int classId)
		{
			switch (subclassName)
			{
				case "Cloth":
					return Item.ItemType.Cloth;

				case "Leather":
					return Item.ItemType.Leather;

				case "Mail":
					return Item.ItemType.Mail;

				case "Plate":
					return Item.ItemType.Plate;

				case "Dagger":
					return Item.ItemType.Dagger;

				case "Fist Weapon":
					return Item.ItemType.FistWeapon;

				case "Axe":
					if (inventoryType == 17)
						return Item.ItemType.TwoHandAxe;
					else
						return Item.ItemType.OneHandAxe;

				case "Mace":
					if (inventoryType == 17)
						return Item.ItemType.TwoHandMace;
					else
						return Item.ItemType.OneHandMace;

				case "Sword":
					if (inventoryType == 17)
						return Item.ItemType.TwoHandSword;
					else
						return Item.ItemType.OneHandSword;

				case "Polearm":
					return Item.ItemType.Polearm;

				case "Staff":
					return Item.ItemType.Staff;

				case "Shield":
					return Item.ItemType.Shield;

				case "Bow":
					return Item.ItemType.Bow;

				case "Crossbow":
					return Item.ItemType.Crossbow;

				case "Gun":
					return Item.ItemType.Gun;

				case "Wand":
					return Item.ItemType.Wand;

				case "Thrown":
					return Item.ItemType.Thrown;

				case "Idol":
					return Item.ItemType.Idol;

				case "Libram":
					return Item.ItemType.Libram;

				case "Totem":
					return Item.ItemType.Totem;

				case "Arrow":
					return Item.ItemType.Arrow;

				case "Bullet":
					return Item.ItemType.Bullet;

				case "Quiver":
					return Item.ItemType.Quiver;

				case "Ammo Pouch":
					return Item.ItemType.AmmoPouch;

				case "Sigil":
					return Item.ItemType.Sigil;

				default:
					return Item.ItemType.None;
			}
		}

		private static Item.ItemSlot GetItemSlot(int inventoryType, int classId)
		{
			switch (classId)
			{
				case 6:
					return Item.ItemSlot.Projectile;

				case 11:
					return Item.ItemSlot.ProjectileBag;
			}
					
			switch (inventoryType)
			{
				case 1:
					return Item.ItemSlot.Head;

				case 2:
					return Item.ItemSlot.Neck;

				case 3:
					return Item.ItemSlot.Shoulders;

				case 16:
					return Item.ItemSlot.Back;

				case 5:
				case 20:
					return Item.ItemSlot.Chest;

				case 4:
					return Item.ItemSlot.Shirt;

				case 19:
					return Item.ItemSlot.Tabard;

				case 9:
					return Item.ItemSlot.Wrist;

				case 10:
					return Item.ItemSlot.Hands;

				case 6:
					return Item.ItemSlot.Waist;

				case 7:
					return Item.ItemSlot.Legs;

				case 8:
					return Item.ItemSlot.Feet;

				case 11:
					return Item.ItemSlot.Finger;

				case 12:
					return Item.ItemSlot.Trinket;

				case 13:
					return Item.ItemSlot.OneHand;

				case 17:
					return Item.ItemSlot.TwoHand;

				case 21:
					return Item.ItemSlot.MainHand;

				case 14:
				case 22:
				case 23:
					return Item.ItemSlot.OffHand;

				case 15:
				case 25:
				case 26:
				case 28:
					return Item.ItemSlot.Ranged;

				case 24:
					return Item.ItemSlot.Projectile;

				case 27:
					return Item.ItemSlot.ProjectileBag;
				
				default:
					return Item.ItemSlot.None;
			}
		}
        #endregion

    }

    public class ItemRequestContext
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

        public ItemRequestContext(int id, Action<Item> callback)
        {
            this.id = id;
            this.callback = callback;
        }
    }

    public class CharacterRequestContext
    {
        private readonly string name;
        public string Name { get { return name; } }

        private readonly string realm;
        public string Realm { get { return realm; } }

        private readonly Character.CharacterRegion region;
        public Character.CharacterRegion Region { get { return region; } }

        private readonly Action<Character> callback;
        public Action<Character> Callback { get { return callback; } }

        public XDocument CharacterSheet { get; set; }
        public XDocument TalentTree { get; set; }

        public Character Result { get; set; }

        public void Invoke()
        {
            callback(Result);
        }

        public CharacterRequestContext(string name, string realm, Character.CharacterRegion region, Action<Character> callback)
        {
            this.name = name;
            this.realm = realm;
            this.region = region;
            this.callback = callback;
        }
    }
}
