--[[

Author : Levva of EU Khadgar 

Version 0.01
	Initial release of Concept code to curseforge 
	
Version 0.02
	Working XML export of basics character details
	
Version 0.03
	Now can export Character Basics
	
Version 0.04
	Can now export basic equipped items
	
Version 0.05
	Now exports model, talents, professions and glyphs, and equipment in bags
	
Version 0.06
	Added support for logging bank items
	
--]]

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")
local AceAddon = LibStub("AceAddon-3.0")
Rawr = AceAddon:NewAddon("Rawr", "AceConsole-3.0", "AceEvent-3.0", "AceHook-3.0")
local REVISION = tonumber(("$Revision$"):match("%d+"))

-- Binding Variables
BINDING_HEADER_RAWR_TITLE = L["Keybind Title"]
BINDING_NAME_RAWR_OPEN_EXPORT = L["Open Export Window"]

-----------------------------------------
-- Initialisation & Startup Routines
-----------------------------------------

function Rawr:OnInitialize()
	local AceConfigReg = LibStub("AceConfigRegistry-3.0")
	local AceConfigDialog = LibStub("AceConfigDialog-3.0")

	self.db = LibStub("AceDB-3.0"):New("RawrDBPC", Rawr.defaults, "char")
	LibStub("AceConfig-3.0"):RegisterOptionsTable("Rawr", self:GetOptions(), {"Rawr"} )
	
	-- Add the options to blizzard frame (add them backwards so they show up in the proper order
	self.optionsFrame = AceConfigDialog:AddToBlizOptions("Rawr","Rawr")
	self.db:RegisterDefaults(self.defaults)
	
	local version = GetAddOnMetadata("Rawr","Version")
	self.version = ("Rawr v%s (r%s)"):format(version, REVISION)
	self:Print(self.version..L[" Loaded."])
	self.xml = {}
	self.xml.version = version
	self.xml.revision = _G.strtrim(string.sub(REVISION, -6))
end

function Rawr:OnDisable()
    -- Called when the addon is disabled
end

function Rawr:OnEnable()
 	self:RegisterEvent("PLAYERBANKSLOTS_CHANGED")
 	self:RegisterEvent("PLAYERBANKBAGSLOTS_CHANGED")
 	self:RegisterEvent("BANKFRAME_OPENED")
 	self:RegisterEvent("BANKFRAME_CLOSED")
end

----------------------
-- Event Routines
----------------------

function Rawr:BANKFRAME_OPENED()
	Rawr.BankOpen = true
end

function Rawr:BANKFRAME_CLOSED()
	if Rawr.BankOpen then -- first time event is fired this event is just as bank is closed
		self:UpdateBankContents()
	end
	Rawr.BankOpen = false
end

----------------------
-- Export Routines
----------------------

function Rawr:DisplayExportWindow()
	self:ExportToRawr()
end

----------------------
-- Bank Routines
----------------------

function Rawr:UpdateBankContents()
	Rawr.BankItems = {}
	Rawr.BankItems.count = 0
	for index = 1, 28 do
		local _, _, _, _, _, _, link = GetContainerItemInfo(BANK_CONTAINER, index)
		if link then
			Rawr.BankItems.count = Rawr.BankItems.count + 1
			Rawr.BankItems[Rawr.BankItems.count] = link
		end
	end
		for bagNum = 5, 11 do
			local bagNum_ID = BankButtonIDToInvSlotID(bagNum, 1)
			local itemLink = GetInventoryItemLink("player", bagNum_ID)
			if itemLink then
				local theBag = {}
				theBag.link = itemLink
				theBag.size = GetContainerNumSlots(bagNum)
				for bagItem = 1, theBag.size do
					local _, _, _, _, _, _, link = GetContainerItemInfo(bagNum, bagItem)
					if link then
						Rawr.BankItems.count = Rawr.BankItems.count + 1
						Rawr.BankItems[Rawr.BankItems.count] = link
					end
				end
			end
		end
	end
	self:DebugPrint("Rawr : Bank contents updated. "..Rawr.BankItems.count.." items found.")
end

----------------------
-- Utility Routines
----------------------

function Rawr:GetItem(slotLink)
	_, itemLink = GetItemInfo(slotLink)
	itemString = string.match(itemLink, "item[%-?%d:]+")
	if itemString then
		self:DebugPrint("found "..itemString.." in slot")
	end
	return itemString or ""
end

function Rawr:DebugPrint(msg)
	if Rawr.db.char.debug then
		self:Print(msg)
	end
end

function Rawr:FinishedMoving(var, frame)
	local point, relativeTo, relativePoint, xOffset, yOffset = frame:GetPoint();
	var.point = point
	var.relativeTo = relativeTo
	var.relativePoint = relativePoint
	var.xOffset = xOffset
	var.yOffset = yOffset
end

function Rawr:DisplayVersion()
	self:Print(self.version)
end
