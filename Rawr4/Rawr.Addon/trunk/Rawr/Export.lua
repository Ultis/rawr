if not Rawr then return end

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")

local format, len, lower = _G.string.format, _G.string.len, _G.string.lower
local gsub, trim = _G.string.gsub, _G.strtrim

local frame = CreateFrame("Frame", "Rawr_ExportFrame", UIParent, "DialogBoxFrame")

local outputText = ""

Rawr.talents = {}
Rawr.talents.warrior = {}
Rawr.talents.paladin = {}
Rawr.talents.hunter = {}
Rawr.talents.rogue = {}
Rawr.talents.priest = {}
Rawr.talents.shaman = {}
Rawr.talents.mage = {}
Rawr.talents.warlock = {}
Rawr.talents.druid = {}
Rawr.talents.deathknight = {}
Rawr.talents.warrior.talents = 61
Rawr.talents.warrior.glyphs = 34
Rawr.talents.paladin.talents = 60
Rawr.talents.paladin.glyphs = 34
Rawr.talents.hunter.talents = 58
Rawr.talents.hunter.glyphs = 33
Rawr.talents.rogue.talents = 57
Rawr.talents.rogue.glyphs = 34
Rawr.talents.priest.talents = 61
Rawr.talents.priest.glyphs = 37
Rawr.talents.shaman.talents = 57
Rawr.talents.shaman.glyphs = 32
Rawr.talents.mage.talents = 61
Rawr.talents.mage.glyphs = 20
Rawr.talents.warlock.talents = 56
Rawr.talents.warlock.glyphs = 17
Rawr.talents.druid.talents = 64
Rawr.talents.druid.glyphs = 40
Rawr.talents.deathknight.talents = 58
Rawr.talents.deathknight.glyphs = 29

StaticPopupDialogs["RAWR_EXPORT_WINDOW"] = {
	text = L["export_rawr"],
	button1 = ACCEPT,
	button2 = CLOSE,
	hasEditBox = 1,
	OnShow = function(self)
		local editBox = _G[self:GetName().."EditBox"]
		editBox:SetText(outputText)
		editBox:HighlightText()
		editBox:SetAutoFocus(false)
		editBox:SetJustifyH("LEFT")
		editBox:SetJustifyV("TOP")
		editBox:SetFocus()
		local dialogBox = editBox:GetParent()
		dialogBox:SetPoint("CENTER", "UIParent")
	end,
	EditBoxOnEnterPressed = function(self)
		self:GetParent():Hide();
	end,
	EditBoxOnEscapePressed = function(self)
		self:GetParent():Hide();
	end,
	OnHide = function(self)
		_G[self:GetName().."EditBox"]:SetText("");
	end,
	timeout = 0,
	hideOnEscape = 1,
}

function Rawr:AddLine(level, text)
	indent = 4 * (level or 0)
	if text then
		outputText = outputText..string.rep(" ", indent)..trim(text).."\r\n"
	end
end

function Rawr:SortSlots(a, b)
	return a.slotId > b.slotId
end

------------------
-- XML functions
------------------

function Rawr:WriteXMLHeader()
	self:AddLine(0, "<?xml version=\"1.0\" encoding=\"utf-8\"?>")
	self:AddLine(0, "<Rawr xmlns:xsi=\"http://rawr.codeplex.com/somewhere\"")
	self:AddLine(0, "      xsi:noNamespaceSchemaLocation=\"RawrAddon.xsd\">")
	self:AddLine(1, "<Version>"..Rawr.xml.version.."</Version>")
	self:AddLine(1, "<Build>"..Rawr.xml.revision.."</Build>")
	self:AddLine(1, "<RawrBuild>56325</RawrBuild>")
	self:AddLine(1, "<Character")
    self:AddLine(2, "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"")
    self:AddLine(2, "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">")
end

function Rawr:WriteXMLFooter()
	self:AddLine(1, "</Character>")
	self:AddLine(0, "</Rawr>")
end

function Rawr:AddUnusedCharacterXML()
	self:AddLine(2, "<CustomGemmingTemplates />")
    self:AddLine(2, "<GemmingTemplateOverrides />")
    self:AddLine(2, "<OptimizationRequirements />")
end

------------------
-- Export functions
------------------

function Rawr:ExportToRawr()
	outputText = ""
	self:WriteXMLHeader()
	self:ExportBasics()
	self:ExportBags()
	self:ExportBank()
	self:ExportEquipped()
	self:ExportSockets()
	self:AddUnusedCharacterXML()
	self:ExportTalents()
	self:ExportGlyphs()
	self:ExportProfessions()
	self:WriteXMLFooter()
	StaticPopup_Show("RAWR_EXPORT_WINDOW")
end

function Rawr:ExportBasics()
	local _, class = UnitClass("player")
	local modelName, className = Rawr:GetModelName(class)
	self:AddLine(2, "<Name>"..UnitName("player").."</Name>")
	self:AddLine(2, "<Realm>"..GetRealmName().."</Realm>")
	self:AddLine(2, "<Region>"..self:GetRegionName().."</Region>")
	self:AddLine(2, "<Race>"..self:GetRaceName().."</Race>")
	self:AddLine(2, "<Class>"..className.."</Class>")
	self:AddLine(2, "<Level>"..UnitLevel("player").."</Level>")
	self:AddLine(2, "<CurrentModel>"..modelName.."</CurrentModel>")
end

function Rawr:GetRaceName(race)
	local _, race = UnitRace("player")
	if race == "Night Elf" then
		return "NightElf"
	elseif race == "Blood Elf" then 
		return "BloodElf"
	elseif race == "Scourge" then
		return "Undead"
	else
		return race
	end
end

function Rawr:GetModelName(class)
	local primaryTabId = GetPrimaryTalentTree()
	if class == "DEATHKNIGHT" then
		if primaryTabId == 1 then
			return "TankDK", "DeathKnight"
		else
			return "DPSDK", "DeathKnight"
		end
	elseif class == "DRUID" then
		if primaryTabId == 1 then
			return "Moonkin", "Druid"
		elseif primaryTabId == 2 then
			local _, _, _, _, pulverise = GetTalentInfo(2,21)
			if pulverise > 0 then
				return "Bear", "Druid"
			else
				return "Cat", "Druid"
			end
		elseif primaryTabId == 3 then
			return "Tree", "Druid"
		end
		return "Bear", "Druid"
	elseif class == "HUNTER" then
		return "Hunter", "Hunter"
	elseif class == "MAGE" then
		return "Mage", "Mage"
	elseif class == "ROGUE" then
		return "Rogue", "Rogue"
	elseif class == "PALADIN" then
		if primaryTabId == 1 then
			return "Healadin", "Paladin"
		elseif primaryTabId == 2 then
			return "ProtPaladin", "Paladin"
		else
			return "Retribution", "Paladin"
		end
	elseif class == "PRIEST" then
		if primaryTabId == 3 then
			return "ShadowPriest", "Priest"
		else
			return "HealPriest", "Priest"
		end
	elseif class == "SHAMAN" then
		if primaryTabId == 1 then
			return "Elemental", "Shaman"
		elseif primaryTabId == 2 then
			return "Enhance", "Shaman"
		else
			return "RestoSham", "Shaman"
		end	
		return "Enhance", "Shaman"
	elseif class == "WARLOCK" then
		return "Warlock", "Warlock"
	elseif class == "WARRIOR" then
		if primaryTabId == 3 then
			return "ProtWarr", "Warrior"
		else
			return "DPSWarr", "Warrior"
		end
	end
	return "Unknown", "Unknown"
end

function Rawr:ExportProfessions()
	local professionIcon
	local profession1, profession2 = GetProfessions()
	if profession1 then
		_, professionIcon = GetProfessionInfo(profession1)
		self:AddLine(2, "<PrimaryProfession>"..self:IconToProfessionName(professionIcon).."</PrimaryProfession>")
	else
		self:AddLine(2, "<PrimaryProfession>None</PrimaryProfession>")
	end
	if profession2 then
		_, professionIcon = GetProfessionInfo(profession2)
		self:AddLine(2, "<SecondaryProfession>"..self:IconToProfessionName(professionIcon).."</SecondaryProfession>")
	else
		self:AddLine(2, "<SecondaryProfession>None</SecondaryProfession>")
	end
end

function Rawr:IconToProfessionName(icon)
	if icon == "Interface\\Icons\\Trade_Herbalism" then
		return "Herbalism"
	elseif icon == "Interface\\Icons\\INV_Pick_02" then
		return "Mining"
	elseif icon == "Interface\\Icons\\INV_Misc_Pelt_Wolf_01" then
		return "Skinning"
	elseif icon == "Interface\\Icons\\Trade_Alchemy" then
		return "Alchemy"
	elseif icon == "Interface\\Icons\\Trade_BlackSmithing" then
		return "Blacksmithing"
	elseif icon == "Interface\\Icons\\Trade_Engraving" then
		return "Enchanting"
	elseif icon == "Interface\\Icons\\Trade_Engineering" then
		return "Engineering"
	elseif icon == "Interface\\Icons\\INV_Inscription_Tradeskill01" then
		return "Inscription"
	elseif icon == "Interface\\Icons\\INV_Misc_Gem_01" then
		return "Jewelcrafting"
	elseif icon == "Interface\\Icons\\Trade_LeatherWorking" then
		return "Leatherworking"
	elseif icon == "Interface\\Icons\\Trade_Tailoring" then
		return "Tailoring"
	end
	return ""
end

function Rawr:DebugTalents()
	local numTabs = GetNumTalentTabs()
	for t=1, numTabs do
		local numTalents = GetNumTalents(t)
		for i=1, numTalents do
			name = GetTalentInfo(t,i) or "not specified"
			self:Print("tab:"..t.." talent:"..i..": "..name)
		end
	end
end

function Rawr:ExportTalents()
	local talents = ""
	local numTabs = GetNumTalentTabs()
	for t=1, numTabs do
		local numTalents = GetNumTalents(t)
		for i=1, numTalents do
			_, _, _, _, currRank = GetTalentInfo(t,i)
			talents = talents .. (currRank or 0)
		end
	end	
	local _, class = UnitClass("player")
	if class == "WARRIOR" then
		self:AddLine(2, "<WarriorTalents>"..talents.."."..string.rep("0", Rawr.talents.warrior.glyphs).."</WarriorTalents>")
	end
	if class == "PALADIN" then
		self:AddLine(2, "<PaladinTalents>"..talents.."."..string.rep("0", Rawr.talents.paladin.glyphs).."</PaladinTalents>")
	end
	if class == "HUNTER" then
		self:AddLine(2, "<HunterTalents>"..talents.."."..string.rep("0", Rawr.talents.hunter.glyphs).."</HunterTalents>")
	end
	if class == "ROGUE" then
		self:AddLine(2, "<RogueTalents>"..talents.."."..string.rep("0", Rawr.talents.rogue.glyphs).."</RogueTalents>")
	end
	if class == "PRIEST" then
		self:AddLine(2, "<PriestTalents>"..talents.."."..string.rep("0", Rawr.talents.priest.glyphs).."</PriestTalents>")
	end
	if class == "SHAMAN" then
		self:AddLine(2, "<ShamanTalents>"..talents.."."..string.rep("0", Rawr.talents.shaman.glyphs).."</ShamanTalents>")
	end
	if class == "MAGE" then
		local clientVersion = GetBuildInfo()
		if clientVersion == "4.0.6" then
			talents = string.sub(talents, 1, 14)..string.sub(talents, 16)
		end
		self:AddLine(2, "<MageTalents>"..talents.."."..string.rep("0", Rawr.talents.mage.glyphs).."</MageTalents>")
	end
	if class == "WARLOCK" then
		self:AddLine(2, "<WarlockTalents>"..talents.."."..string.rep("0", Rawr.talents.warlock.glyphs).."</WarlockTalents>")
	end
	if class == "DRUID" then
		self:AddLine(2, "<DruidTalents>"..talents.."."..string.rep("0", Rawr.talents.shaman.glyphs).."</DruidTalents>")
	end
	if class == "DEATHKNIGHT" then
		self:AddLine(2, "<DeathKnightTalents>"..talents.."."..string.rep("0", Rawr.talents.deathknight.glyphs).."</DeathKnightTalents>")
	end
end

function Rawr:ExportGlyphs()
	self:AddLine(2, "<AvailableGlyphs>")
	local numglyphs = GetNumGlyphSockets()
	for index = 1, numglyphs do
		local _, _, _, spellID = GetGlyphSocketInfo(index)
		if spellID then
			self:AddLine(3, "<Glyph>"..spellID.."</Glyph>")
		end
	end
	self:AddLine(2, "</AvailableGlyphs>")
end

function Rawr:ExportEquipped()
	local slotLink, itemLink, itemString
	for index, slot in ipairs(Rawr.slots) do
--		self:DebugPrint("examining slot :"..slot.slotId.." "..slot.slotName)
		slotLink = GetInventoryItemLink("player", slot.slotId)
		if slotLink then
			self:AddLine(2, "<"..slot.slotName..">"..self:GetRawrItem(slot.slotId, slotLink).."</"..slot.slotName..">")
		end
	end
end

function Rawr:ExportSockets()
	local profession1, profession2 = GetProfessions()
	local blacksmith = false
	if profession1 ~= nil then
		if GetProfessionInfo(profession1) == "Blacksmithing" then
			blacksmith = true
		end
	end
	if profession2 ~= nil then
		if GetProfessionInfo(profession2) == "Blacksmithing" then
			blacksmith = true
		end
	end
	if blacksmith then
		self:AddLine(2, "<WristBlacksmithingSocketEnabled>"..self:HasSocket(9).."</WristBlacksmithingSocketEnabled>")
		self:AddLine(2, "<HandsBlacksmithingSocketEnabled>"..self:HasSocket(10).."</HandsBlacksmithingSocketEnabled>")
	end
	if self:HasSocket(6) then
		self:AddLine(2, "<WaistBlacksmithingSocketEnabled>"..self:HasSocket(9).."</WaistBlacksmithingSocketEnabled>")
	end
end

function Rawr:HasSocket(slotId) 
	-- this function may be changed at a later date to actually work out if user has socketed something
	return "true" 
end

function Rawr:ExportBags()
	local bag, slot
	self:AddLine(2, "<Bags>")
	for bag = 0, 4 do
		for slot = 1, GetContainerNumSlots(bag) do
			self:WriteAvailableItem(3, GetContainerItemLink(bag, slot))
		end
	end
	self:AddLine(2, "</Bags>")
end

function Rawr:ExportBank()
	local bag, slot
	self:AddLine(2, "<Bank>")
	for index = 1, self.db.char.BankItems.count do
		Rawr:WriteAvailableItem(3, self.db.char.BankItems[index])
	end
	self:AddLine(2, "</Bank>")
end

function Rawr:WriteAvailableItem(indent, slotLink)
	local itemID, isEquippable = self:GetItemID(slotLink)
	if isEquippable then
		self:AddLine(indent, "<AvailableItem>"..itemID.."</AvailableItem>")
	end
end