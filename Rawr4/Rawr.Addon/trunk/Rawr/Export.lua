if not Rawr then return end

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")

local format, len, lower = _G.string.format, _G.string.len, _G.string.lower
local gsub, trim = _G.string.gsub, _G.strtrim

local frame = CreateFrame("Frame", "Rawr_ExportFrame", UIParent, "DialogBoxFrame")

local outputText = ""
if not Rawr.vars then
	Rawr.vars = {}
end

Rawr.slots = { ["Head"] = 1, 
					["Neck"] = 2, 
					["Shoulder"] = 3,
					["Chest"] = 5,
					["Waist"] = 6,
					["Legs"] = 7,
					["Feet"] = 8,
					["Wrist"] = 9,
					["Hands"] = 10,
					["Finger1"] = 11,
					["Finger2"] = 12,
					["Trinket1"] = 13,
					["Trinket2"] = 14,
					["Cloak"] = 15,
					["MainHand"] = 16,
					["OffHand"] = 17,
					["Ranged"] = 18,
					}

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
	self:DebugPrint(text)
	if text then
		outputText = outputText.."\r\n"..string.rep(" ", indent)..trim(text)
	end
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
	self:AddLine(1, "<Character>")
end

function Rawr:WriteXMLFooter()
	self:AddLine(1, "</Character>")
	self:AddLine(0, "</Rawr>")
end

------------------
-- Export functions
------------------

function Rawr:ExportToRawr()
	outputText = ""
	self:WriteXMLHeader()
	self:ExportBasics()
	self:ExportProfessions()
	self:ExportTalents()
	self:ExportGlyphs()
	self:ExportEquipped()
	self:ExportBags()
	self:ExportBank()
	self:WriteXMLFooter()
	StaticPopup_Show("RAWR_EXPORT_WINDOW")
end

function Rawr:ExportBasics()
	self:AddLine(2, "<Name>"..UnitName("player").."</Name>")
	self:AddLine(2, "<Level>"..UnitLevel("player").."</Level>")
	self:AddLine(2, "<Race>"..UnitRace("player").."</Race>")
	self:AddLine(2, "<Realm>"..GetRealmName().."</Realm>")
	self:AddLine(2, "<Region>"..Rawr:GetRegionName().."</Region>")
	self:AddLine(2, "<Model>"..Rawr:GetModelName().."</Model>")
end

function Rawr:GetModelName(class)
	local _, class = UnitClass("player")
	local primaryTabId = GetPrimaryTalentTree()
	if class == "DEATHKNIGHT" then
		if primaryTabId == 1 then
			return "TankDK"
		else
			return "DPSDK"
		end
	elseif class == "DRUID" then
		if primaryTabId == 1 then
			return "Moonkin"
		elseif primaryTabId == 2 then
			local _, _, _, _, pulverise = GetTalentInfo(2,21)
			if pulverise > 0 then
				return "Bear"
			else
				return "Cat"
			end
		elseif primaryTabId == 3 then
			return "Tree"
		end
		return "Bear"
	elseif class == "HUNTER" then
		return "Hunter"
	elseif class == "MAGE" then
		return "Mage"
	elseif class == "ROGUE" then
		return "Rogue"
	elseif class == "PALADIN" then
		if primaryTabId == 1 then
			return "Healadin"
		elseif primaryTabId == 2 then
			return "ProtPaladin"
		else
			return "Retribution"
		end
	elseif class == "PRIEST" then
		if primaryTabId == 3 then
			return "ShadowPriest"
		else
			return "HealPriest"
		end
	elseif class == "SHAMAN" then
		if primaryTabId == 1 then
			return "Elemental"
		elseif primaryTabId == 2 then
			return "Enhance"
		else
			return "RestoSham"
		end	
		return "Enhance"
	elseif class == "WARLOCK" then
		return "Warlock"
	elseif class == "WARRIOR" then
		if primaryTabId == 3 then
			return "ProtWarr"
		else
			return "DPSWarr"
		end
	end
	return "Unknown"
end

function Rawr:ExportProfessions()
	local profession1, profession2 = GetProfessions()
	self:AddLine(2, "<PrimaryProfession>"..GetProfessionInfo(profession1).."</PrimaryProfession>")
	self:AddLine(2, "<SecondaryProfession>"..GetProfessionInfo(profession2).."</SecondaryProfession>")
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
	self:AddLine(2, "<Talents>"..talents.."</Talents>")
end

function Rawr:ExportGlyphs()

end

function Rawr:ExportEquipped()
	local slotLink, itemLink, itemString
	self:AddLine(2, "<Equipped>")
	for slotName, slotId in pairs(Rawr.slots) do
		self:DebugPrint("examining slot :"..slotId)
		slotLink = GetInventoryItemLink("player", slotId)
		if slotLink then
			_, itemLink = GetItemInfo(slotLink)
			itemString = string.match(itemLink, "item[%-?%d:]+")
			self:DebugPrint("found "..itemString.." in slot")
			self:AddLine(3, "<Slot id="..slotId.." name=\""..slotName.."\">")
			self:AddLine(4, "<![CDATA[")
			self:AddLine(5, itemString)
			self:AddLine(4, "]]>")
			self:AddLine(3, "</Slot>")
		end
	end
	self:AddLine(2, "</Equipped>")
end

function Rawr:ExportBags()

end

function Rawr:ExportBank()

end
