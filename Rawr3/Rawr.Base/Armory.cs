using System;
using System.Net;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Rawr
{
    public class Armory
    {
        public EventHandler ResultReady;

        public Character CharacterResult { get; private set; }
        public string[] ItemsOnCharacterResult { get; private set; }

        private NetworkUtils network;
        private Character.CharacterRegion region;

        public Armory() { }
        public Armory(EventHandler handler) : this() { ResultReady += handler; }

        public void GetCharacter(string name, string realm, Character.CharacterRegion region)
        {
            StatusMessaging.UpdateStatus("Get Character From Armory", "Downloading Character Definition");
            StatusMessaging.UpdateStatus("Get Talent Tree From Armory", "Queued");
            this.region = region;
            network = new NetworkUtils(CharacterSheet_Completed);
            network.GetCharacterSheetDocument(name, realm, region);
        }

        private void CharacterSheet_Completed(object sender, EventArgs e)
        {
            XDocument docCharacter = network.Result;


            if (docCharacter == null || docCharacter.Element("page") == null
                || docCharacter.Element("page").Element("characterInfo") == null
                || docCharacter.Element("page").Element("characterInfo").Attribute("errCode") != null)
            {
                StatusMessaging.UpdateStatusFinished("Get Character From Armory");
                StatusMessaging.ReportError("Get Character From Armory", null, "No character returned from the Armory");
                CharacterResult = null;
                ItemsOnCharacterResult = null;
                if (ResultReady != null) ResultReady.Invoke(this, EventArgs.Empty);
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

            ItemsOnCharacterResult = new string[items.Values.Count];
            items.Values.CopyTo(ItemsOnCharacterResult, 0);
            CharacterResult = new Character(name, realm, region, race,
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
            CharacterResult.Class = charClass;
            ApplyRacialandProfessionBuffs(docCharacter);

            StatusMessaging.UpdateStatus("Get Talent Tree From Armory", "Downloading Talent Tree");
            StatusMessaging.UpdateStatusFinished("Get Character From Armory");
            network = new NetworkUtils(TalentTree_Completed);
            network.GetTalentTreeDocument(CharacterResult.Name, CharacterResult.Realm, CharacterResult.Region);
        }

        private void TalentTree_Completed(object sender, EventArgs e)
        {
            XDocument talentsDoc = network.Result;
            XElement activeGroup = null;
            StatusMessaging.UpdateStatus("Get Talent Tree From Armory", "Processing Talent Tree");
            foreach(XElement group in talentsDoc.Element("page").Element("characterInfo")
                .Element("talentGroups").Elements("talentGroup"))
            {
                if (group.Attribute("active") != null) activeGroup = group;
            }
            string talentCode = activeGroup.Element("talentSpec").Attribute("value").Value;
            switch (CharacterResult.Class)
            {
                case Character.CharacterClass.Warrior:
                    CharacterResult.WarriorTalents = new WarriorTalents(talentCode);
                    if (CharacterResult.WarriorTalents.Devastate > 0) CharacterResult.CurrentModel = "ProtWarr";
                    else CharacterResult.CurrentModel = "DPSWarr";
                    break;
                case Character.CharacterClass.Paladin:
                    CharacterResult.PaladinTalents = new PaladinTalents(talentCode);
                    if (CharacterResult.PaladinTalents.HolyShield > 0) CharacterResult.CurrentModel = "ProtPaladin";
                    else if (CharacterResult.PaladinTalents.CrusaderStrike > 0) CharacterResult.CurrentModel = "Retribution";
                    else CharacterResult.CurrentModel = "Healadin";
                    break;
                case Character.CharacterClass.Hunter:
                    CharacterResult.HunterTalents = new HunterTalents(talentCode);
                    CharacterResult.CurrentModel = "Hunter";
                    break;
                case Character.CharacterClass.Rogue:
                    CharacterResult.RogueTalents = new RogueTalents(talentCode);
                    CharacterResult.CurrentModel = "Rogue";
                    break;
                case Character.CharacterClass.Priest:
                    CharacterResult.PriestTalents = new PriestTalents(talentCode);
                    if (CharacterResult.PriestTalents.Shadowform > 0) CharacterResult.CurrentModel = "ShadowPriest";
                    else CharacterResult.CurrentModel = "HolyPriest";
                    break;
                case Character.CharacterClass.Shaman:
                    CharacterResult.ShamanTalents = new ShamanTalents(talentCode);
                    if (CharacterResult.ShamanTalents.ElementalMastery > 0) CharacterResult.CurrentModel = "Elemental";
                    else if (CharacterResult.ShamanTalents.Stormstrike > 0) CharacterResult.CurrentModel = "Enhance";
                    else CharacterResult.CurrentModel = "RestoSham";
                    break;
                case Character.CharacterClass.Mage:
                    CharacterResult.MageTalents = new MageTalents(talentCode);
                    CharacterResult.CurrentModel = "Mage";
                    break;
                case Character.CharacterClass.Warlock:
                    CharacterResult.WarlockTalents = new WarlockTalents(talentCode);
                    CharacterResult.CurrentModel = "Warlock";
                    break;
                case Character.CharacterClass.Druid:
                    CharacterResult.DruidTalents = new DruidTalents(talentCode);
                    if (CharacterResult.DruidTalents.ProtectorOfThePack > 0) CharacterResult.CurrentModel = "Bear";
                    else if (CharacterResult.DruidTalents.LeaderOfThePack > 0) CharacterResult.CurrentModel = "Cat";
                    else if (CharacterResult.DruidTalents.MoonkinForm > 0) CharacterResult.CurrentModel = "Moonkin";
                    else CharacterResult.CurrentModel = "Tree";
                    break;
                case Character.CharacterClass.DeathKnight:
                    CharacterResult.DeathKnightTalents = new DeathKnightTalents(talentCode);
                    if (CharacterResult.DeathKnightTalents.Anticipation > 0) CharacterResult.CurrentModel = "TankDK";
                    else CharacterResult.CurrentModel = "DPSDK";
                    break;
                default:
                    break;
            }
            TalentsBase talents = CharacterResult.CurrentTalents;
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
            if (ResultReady != null) ResultReady.Invoke(this, EventArgs.Empty);
        }

        private void ApplyRacialandProfessionBuffs(XDocument doc)
        {
            if (CharacterResult.Race == Character.CharacterRace.Draenei) CharacterResult.ActiveBuffs.Add(Buff.GetBuffByName("Heroic Presence"));

            foreach (XElement profession in doc.Element("page").Element("characterInfo")
                .Element("characterTab").Element("professions").Elements("skill"))
            {   // apply profession buffs if max skill
                if (profession.Attribute("name").Value == "Mining" && profession.Attribute("value").Value == "450")
                    CharacterResult.ActiveBuffs.Add(Buff.GetBuffByName("Toughness"));
                if (profession.Attribute("name").Value == "Skinning" && profession.Attribute("value").Value == "450")
                    CharacterResult.ActiveBuffs.Add(Buff.GetBuffByName("Master of Anatomy"));
                if (profession.Attribute("name").Value == "Blacksmithing" && int.Parse(profession.Attribute("value").Value) >= 400)
                {
                    CharacterResult.WristBlacksmithingSocketEnabled = true;
                    CharacterResult.HandsBlacksmithingSocketEnabled = true;
                }
            }

            Calculations.GetModel(CharacterResult.CurrentModel).SetDefaults(CharacterResult);
        }


    }
}
