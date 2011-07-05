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
	
Version 0.07
	Reworked the Character XML to be same as save from Rawr
	
Version 0.08
	Removed Equipped Block for equipped items in export
	Removed alternate talent lists for other classes
	Fixed extra line break at start of file
	Added extra indent for available items for visual inspection
	
Version 0.09
	Fixed blacksmithng socket parsing
	Fixed Shoulder and Cloak slot naming for import
	Export Class name as lowercase
	Added RawrBuild tag to export to allow Rawr to know what minimum build addon supports.
	
Version 0.10
	Fix initial uppercase letter on classnames

Version 0.11
	Fix for race names enumeration
	Added support files for Curseforge localisations
	
Version 0.20
	Added button to Character Panel
	Added frame to display imported RAWR Data.
	Added import buttons to display form
	Added import.lua file
	Altered icon to have lower case Rawr
	
Version 0.21
	Added display of items on import frame with working tooltips
	Highlight item borders to show slot item rarity
	
Version 0.22
	Fix crash if missing a profession and blacksmith
	Fix issue with non English locales exporting race name
	Moved buttons on Paper doll frame a bit
	
Version 0.23
	Added export of empty tinkered items until Blizzard adds API call for checking tinkers
	Fix paperdoll display scaling issue
	
Version 0.24
	Fix export of empty profession
	Lock frame to UIParent and use its native scaling
	
Version 0.30
	Added import of subpoint data
	Move bank items into savedvariable
	Reworked Gem exports to use gem ids
	Rawr import now shows dps subpoints on tooltip on import paperdoll frame
	
Version 0.31
	Fix colour display of tooltips
	Changed Direct Upgrades button to show hide changes - doesn't hide at present
	Reworked Import to use new loaded/character import
	
Version 0.40
	Import now shows differences between what was loaded (from addon or Battle.net) and what was displayed when doing export
	This means you can load up your character do some tweaks/optimisations load it back into Addon and see changes in game
	
Version 0.41
	Tweak for dataloaded always being false on reloadUI
	Changed Tooltip to use custom tooltip
	Added comparison tooltip - now shows difference between loaded and exported

Version 0.42
	Fixed Bank Export
	Added Output on scanning bank
	Added GemId to enchantID routine - fixes display of gems IF user has seen gems in itemcache
	Added text to comparison tooltips to identify which is which
	
Version 0.43
	Added localised export of professions
	
Version 0.50
	Added description to toc
	Added code for Display Upgrades frame
	Implemented Check Boxes to select Display Upgrades Filter
	Implemented Select All/Clear All buttons
	Implemented selection to filter Direct Upgrades
	Direct Upgrades now show icons and overall upgrade score
	Direct Upgrades now show tooltips and comparison tooltips
	Direct Upgrade scrolling can now also be done by mousewheel
	
Version 0.51
	Fix issue with first time use of addon
	
Version 0.52
	Changed Import to use GemEnchantId and not GemId
	Added version check of Rawr data on import
	
Version 0.53
	Direct Upgrade values are rounded to two decimal places
	Tooltip values are rounded to two decimal places
	Added fix for Blizzard bug on Mage talents in patch 4.0.3
	
Version 0.60
	Add options to select sounds to play if an upgrade is seen
	Fix issue if slot in direct upgrades isn't loaded from cache yet
	Fixed shift clicking of Rawr slots or upgrade lists puts item links in chat
	Fixed ctrl clicking of  Rawr slots or upgrade lists shows items in dressing room
	Added some default test sounds
	Added check for upgrade when looting
	Changed to have 3 ranks of sounds
	Fixed issue with checking item ids with nil itemstrings
	Looting mobs tested - bug fixes
	Added warning frame and warning frame options
	Tested Warning frame seems to all be working now
	Added Warning frame move button
	Fixed bug with missing locales
	Added Default Sounds File
	
Version 0.61
	Added Loot Upgrade Check when Need/Greed roll window pops up
	
Version 0.62
	Added command line /rawr import as per website description
	Added check that comparison data exists before adding tooltip line
	
Version 0.63
	Added item location info from Rawr requires Rawr 4.0.17.
	
Version 0.64
	Professions export with None instead of empty XML
	Mage talent bug not fixed in client 4.0.6 - export checks for 4.0.6 now

Version 0.65
	Fixed Suffix Id exporting

Version 0.66
	Update for 4.1.0 issues
	
Version 0.67
	Updated for 4.2 - mage talent bug still not fixed by Blizzard.
	
Version 0.68
	Tooltip item drop locations now wrap so that 
	tooltips are not stupidly large and unreadable
	
--]]

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")
local AceAddon = LibStub("AceAddon-3.0")
local media = LibStub:GetLibrary("LibSharedMedia-3.0", true)
Rawr = AceAddon:NewAddon("Rawr", "AceConsole-3.0", "AceEvent-3.0", "AceHook-3.0")
local REVISION = tonumber(("$Revision$"):match("%d+"))

-- Binding Variables
BINDING_HEADER_RAWR_TITLE = L["Keybind Title"]
BINDING_NAME_RAWR_OPEN_EXPORT = L["Open Export Window"]

Rawr.slots = { { slotName = "Head", slotId = 1, frame = "HeadSlot" }, 
				{ slotName = "Neck", slotId = 2, frame = "NeckSlot" }, 
				{ slotName = "Shoulders", slotId = 3, frame = "ShoulderSlot" }, 
				{ slotName = "Shirt", slotId = 4, frame = "ShirtSlot" }, 
				{ slotName = "Chest", slotId = 5, frame = "ChestSlot" }, 
				{ slotName = "Waist", slotId = 6, frame = "WaistSlot" }, 
				{ slotName = "Legs", slotId = 7, frame = "LegsSlot" }, 
				{ slotName = "Feet", slotId = 8, frame = "FeetSlot" }, 
				{ slotName = "Wrist", slotId = 9, frame = "WristSlot" }, 
				{ slotName = "Hands", slotId = 10, frame = "HandsSlot" }, 
				{ slotName = "Finger1", slotId = 11, frame = "Finger0Slot" }, 
				{ slotName = "Finger2", slotId = 12, frame = "Finger1Slot" }, 
				{ slotName = "Trinket1", slotId = 13, frame = "Trinket0Slot" }, 
				{ slotName = "Trinket2", slotId = 14, frame = "Trinket1Slot" }, 
				{ slotName = "Back", slotId = 15, frame = "BackSlot" }, 
				{ slotName = "MainHand", slotId = 16, frame = "MainHandSlot" }, 
				{ slotName = "OffHand", slotId = 17, frame = "SecondaryHandSlot" }, 
				{ slotName = "Ranged", slotId = 18, frame = "RangedSlot" },
				{ slotName = "Tabard", slotId = 19, frame = "TabardSlot" },
			}
					
Rawr.Colour = {}
Rawr.Colour.Red    = "ffff0000"
Rawr.Colour.Orange = "ffff8000"
Rawr.Colour.Yellow = "ffffff00"
Rawr.Colour.Green  = "ff1eff00"
Rawr.Colour.Grey   = "ff9d9d9d"
Rawr.Colour.Blue   = "ff20d0ff"
Rawr.Colour.White  = "ffffffff"
Rawr.Colour.None   = "ff808080"
Rawr.Colour.DarkBlue = "ff0070dd"
Rawr.Colour.Purple   = "ffa335ee"
Rawr.Colour.Gold     = "ffe5cc80"

Rawr.textures = {}
Rawr.borders = {}
Rawr.fonts = {}
Rawr.sounds = {}

-----------------------------------------
-- Initialisation & Startup Routines
-----------------------------------------

function Rawr:OnInitialize()
	local AceConfigReg = LibStub("AceConfigRegistry-3.0")
	local AceConfigDialog = LibStub("AceConfigDialog-3.0")

	self.db = LibStub("AceDB-3.0"):New("RawrDBPC", Rawr.defaults, "char")
	media.RegisterCallback(self, "LibSharedMedia_Registered")
	LibStub("AceConfig-3.0"):RegisterOptionsTable("Rawr", self:GetOptions(), {"Rawr"} )

	-- Add the options to blizzard frame (add them backwards so they show up in the proper order
	self.optionsFrame = AceConfigDialog:AddToBlizOptions("Rawr", "Rawr")
	self.db:RegisterDefaults(self.defaults)
	if not self.db.char.BankItems then
		self.db.char.BankItems = {}
		self.db.char.BankItems.count = 0
	end
	if not self.db.char.App then
		self.db.char.App = {}
	end
	local version = GetAddOnMetadata("Rawr","Version")
	if (REVISION == nil) then
	  REVISION = "Unknown"
	end
	self.version = ("Rawr v%s (r%s)"):format(version, REVISION)
	self:Print(self.version..L[" Loaded."])
	self.xml = {}
	self.xml.version = version
	self.xml.revision = _G.strtrim(string.sub(REVISION, -6))
	self:CreateButton()
	self:CreateTooltips()
	self:CreateWarningFrame()
	if media then
		-- force loading sounds if hasn't triggered
		Rawr:LibSharedMedia_Registered()
	end
end

function Rawr:OnDisable()
	-- Called when the addon is disabled
	self:UnregisterEvent("BANKFRAME_OPENED")
	self:UnregisterEvent("BANKFRAME_CLOSED")
	self:UnregisterEvent("UNIT_INVENTORY_CHANGED")
	self:UnregisterEvent("LOOT_OPENED")
	self:UnregisterEvent("START_LOOT_ROLL")
	self:UnregisterEvent("CHAT_MSG_PARTY")
	self:UnregisterEvent("CHAT_MSG_PARTY_LEADER")
	self:UnregisterEvent("CHAT_MSG_RAID")
	self:UnregisterEvent("CHAT_MSG_RAID_LEADER")
	self:UnregisterEvent("CHAT_MSG_RAID_WARNING")
	self:UnregisterEvent("CHAT_MSG_WHISPER")
end

function Rawr:OnEnable()
	self:RegisterEvent("BANKFRAME_OPENED")
	self:RegisterEvent("BANKFRAME_CLOSED")
	self:RegisterEvent("UNIT_INVENTORY_CHANGED")
	self:RegisterEvent("LOOT_OPENED")
	self:RegisterEvent("START_LOOT_ROLL")
	self:RegisterEvent("CHAT_MSG_PARTY")
	self:RegisterEvent("CHAT_MSG_PARTY_LEADER")
	self:RegisterEvent("CHAT_MSG_RAID")
	self:RegisterEvent("CHAT_MSG_RAID_LEADER")
	self:RegisterEvent("CHAT_MSG_RAID_WARNING")
	self:RegisterEvent("CHAT_MSG_WHISPER")
	Rawr.CharacterFrameOnHideOld = CharacterFrame:GetScript("OnHide")
	CharacterFrame:SetScript("OnHide", function(frame, ...) Rawr:CharacterFrame_OnHide(frame, ...) end)
	self.db.char.dataloaded = false
	self.lastwarning = 0
end

function Rawr:CharacterFrame_OnHide(frame, ...)
	Rawr_PaperDollFrame:Hide()
	Rawr.CharacterFrameOnHideOld(frame, ...)
end

function Rawr:LibSharedMedia_Registered()
	for _, sound in pairs(Rawr.defaultsounds) do
		media:Register("sound", sound.name, sound.file)
	end
	
	for k, v in pairs(media:List("statusbar")) do
		self.textures[v] = v
	end
	for k, v in pairs(media:List("border")) do
		self.borders[v] = v
	end
	for k, v in pairs(media:List("font")) do
		self.fonts[v] = v
	end
	for k, v in pairs(media:List("sound")) do
		self.sounds[v] = v
	end
end

----------------------
-- Event Routines
----------------------

function Rawr:LOOT_OPENED()
	local numLootItems = GetNumLootItems()
	for index = 1, numLootItems do
		if LootSlotIsItem(index) then
			local slotlink = GetLootSlotLink(index)
			if slotlink then
				local itemId = Rawr:GetItemID(slotlink)
				self:CheckIfItemAnUpgrade(itemId)
			end
		end
	end
end

function Rawr:START_LOOT_ROLL(_, rollId)
	local slotLink = GetLootRollItemLink(rollId)
	if slotLink then
		local itemId = Rawr:GetItemID(slotLink)
		if IsEquippableItem(itemId) then
			self:CheckIfItemAnUpgrade(itemId)
		end
	end
end

function Rawr:CHAT_MSG_PARTY(_, msg)
	Rawr:CheckLootMessage(msg)
end

function Rawr:CHAT_MSG_PARTY_LEADER(_, msg)
	Rawr:CheckLootMessage(msg)
end

function Rawr:CHAT_MSG_RAID(_, msg)
	Rawr:CheckLootMessage(msg)
end

function Rawr:CHAT_MSG_RAID_LEADER(_, msg)
	Rawr:CheckLootMessage(_, msg)
end

function Rawr:CHAT_MSG_RAID_WARNING(_, msg)
	Rawr:CheckLootMessage(_, msg)
end

function Rawr:CHAT_MSG_WHISPER(_, msg)
	Rawr:CheckLootMessage(msg)
end

function Rawr:BANKFRAME_OPENED()
	Rawr.BankOpen = true
end

function Rawr:BANKFRAME_CLOSED()
	if Rawr.BankOpen then -- first time event is fired this event is just as bank is closed
		self:UpdateBankContents()
	end
	Rawr.BankOpen = false
end

function Rawr:UNIT_INVENTORY_CHANGED(unitId)
	if unitId == "player" then
		-- TODO warn if details of equipped gear changed too much from imported data.
	end
end

----------------------
-- Export Routines
----------------------

function Rawr:DisplayExportWindow()
	self:ExportToRawr()
end

function Rawr:DisplayImportWindow()
	ShowUIPanel(CharacterFrame)
	self:ShowCharacterFrame()
	StaticPopup_Show("RAWR_IMPORT_WINDOW")
end

----------------------
-- Bank Routines
----------------------

function Rawr:UpdateBankContents()
	self.db.char.BankItems = {}
	self.db.char.BankItems.count = 0
	for index = 1, 28 do
		local _, _, _, _, _, _, link = GetContainerItemInfo(BANK_CONTAINER, index)
		if link then
			self.db.char.BankItems.count = self.db.char.BankItems.count + 1
			self.db.char.BankItems[self.db.char.BankItems.count] = link
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
				if link and IsEquippableItem(link) then
					self.db.char.BankItems.count = self.db.char.BankItems.count + 1
					self.db.char.BankItems[self.db.char.BankItems.count] = link
				end
			end
		end
	end
	self:Print(string.format(L["Rawr : Bank contents updated. %s equippable/usable items found."], Rawr.db.char.BankItems.count))
end

---------------------
-- Looting Routines
---------------------

function Rawr:CheckLootMessage(msg)
	local _,_,itemId = strfind(msg, "(%d+):")
	if self.lastwarning < GetTime() - 3 then
		-- only bother warning if hasn't said anything in last 3 seconds
		-- this avoids multiple check messages/sounds for same link
		self:CheckIfItemAnUpgrade(itemId)
	end
end

function Rawr:CheckIfItemAnUpgrade(itemId)
	if Rawr.db.char.App.upgrades then
		for _, upgrade in ipairs(Rawr.db.char.App.upgrades) do
			upgradeId = self:GetItemID(upgrade.item)
			if itemId == upgradeId then
				self:WarnUpgradeFound(upgrade)
			end
		end
	end
end

function Rawr:WarnUpgradeFound(upgrade)
	local percent
	local _, loadeditem = self:GetLoadedItem(upgrade.slot)
	self.lastwarning = GetTime()
	local _, itemlink = GetItemInfo(upgrade.item)
	if loadeditem and loadeditem.overall > 0 then
		self:DebugPrint("upgrade:"..upgrade.overall.." loaded:"..loadeditem.overall)
		percent = upgrade.overall / loadeditem.overall
	else
		percent = 0
	end
	local msgtext = string.format(L["Alert %s is in your Rawr upgrade list.\nIt is a %.2f%% upgrade."], itemlink, percent * 100)
	self:Print(msgtext)
	self:PrintWarning(msgtext, Rawr.db.char.warning.colour, Rawr.db.char.warning.duration)
	local sounds = Rawr.db.char.sounds
	if sounds then
		if percent > sounds.majorupgrade.value then
			self:DebugPrint("Playing major upgrade sound")
			PlaySoundFile(sounds.majorupgrade.sound)
		elseif percent > sounds.upgrade.value then 
			self:DebugPrint("Playing upgrade sound")
			PlaySoundFile(sounds.upgrade.sound)
		elseif percent > 0 then
			self:DebugPrint("Playing minor upgrade sound")
			PlaySoundFile(sounds.minorupgrade.sound)
		end
	end
end

----------------------
-- Utility Routines
----------------------

function Rawr:GetItemID(slotLink)
	local itemID = 0
	local isEquippable = false
	local itemName, itemString, itemEquipLoc
	if slotLink then
		itemName, itemString, _, _, _, _, _, _, itemEquipLoc = GetItemInfo(slotLink)
		itemString = itemString or "0:0"
		_, itemID = strsplit(":", itemString)
		itemID = itemID or 0		
		isEquippable = (itemEquipLoc or "") ~= ""
	end
	return itemID, isEquippable
end

function Rawr:GetRawrItem(slotId, slotLink)
	local _, itemLink = GetItemInfo(slotLink)
	local itemString = string.match(itemLink, "item[%-?%d:]+") or ""
	local linkType, itemId, enchantId, _, _, _, _, suffixId, uniqueId, linkLevel, reforgeId = strsplit(":", itemString)
	local jewelId1, jewelId2, jewelId3 = GetInventoryItemGems(slotId)
	local tinkerId = self:GetTinkerInfo(slotId, slotLink)
	itemId = itemId or 0
	jewelId1 = jewelId1 or 0
	jewelId2 = jewelId2 or 0
	jewelId3 = jewelId3 or 0
	enchantId = enchantId or 0
	reforgeId = reforgeId or 0
	suffixId = tonumber(suffixId or 0)
	if (suffixId < 0) then
		suffixId = -suffixId
	end
	--self:DebugPrint("itemID: "..itemId.." enchantId: "..enchantId.." tinkerId:"..tinkerId)
	-- Rawr only uses "itemid.gem1id.gem2id.gem3id.enhcantid.reforgeid"
	return itemId.."."..suffixId.."."..jewelId1.."."..jewelId2.."."..jewelId3.."."..enchantId.."."..reforgeId.."."..tinkerId
end

function Rawr:GetTinkerInfo(slotId, slotlink)
	if slotId == 6 or slotId == 10 or slotId == 15 then
		local textline, text
		self.tooltip.tinker:SetOwner(UIParent, "ANCHOR_NONE")
		self.tooltip.tinker:SetHyperlink(slotlink)
		local numlines = self.tooltip.tinker:NumLines()
		for index = 2, numlines do
			textline = _G["RawrTooltipTinkerTextLeft"..index]
			if textline then
				text = textline:GetText() or ""
				--self:DebugPrint("tinker line"..index.." : "..text) -- doesn't seem to have tinker text in tooltip???
			end
		end
		return 0 -- at present return zero
	else
		return 0 -- not a tinker slot so return zero
	end
end

function Rawr:DebugPrint(msg)
	if self.db and self.db.char.debug then
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

function Rawr:Hex2Dec(sValue)
-- nDecimalValue = tonumber(sValue, 16);	
-- the Lua function returns a variable of type 'number'
-- we want this function to return a 'string' variable
	return tonumber(sValue, 16).."";				
end

function Rawr:GetColour(rawrColour)
	-- format of rawrColour is #ffrrggbb
	local colour = {}
	if rawrColour and string.len(rawrColour) == 9 then
		colour.r = self:Hex2Dec(string.sub(rawrColour, 4, 5)) / 255
		colour.g = self:Hex2Dec(string.sub(rawrColour, 6, 7)) / 255
		colour.b = self:Hex2Dec(string.sub(rawrColour, 8, 9)) / 255
	else
		colour.r = 1
		colour.g = 1
		colour.b = 1
	end
	return colour
end

function Rawr:GemToEnchants() -- code thanks to Siz http://forums.wowace.com/showthread.php?t=8422&highlight=enchantid
	Rawr.itemIDtoEnchantID = {}
	local count = 0
	local itemLink = "|cff0070dd|Hitem:27773:0:%d:0:0:0:0:0|h[Barbaric Legstraps]|h|r"

	for enchantID=1,9999 do
		local gem1Link = select(2, GetItemGem(itemLink:format(enchantID), 1) )
		if gem1Link then
			local itemID = gem1Link:match("item:(%d+):")
			Rawr.itemIDtoEnchantID[tonumber(itemID)] = enchantID
			count = count + 1
		end
	end
	self:DebugPrint("Created enchant list "..count.." items stored")
end

function Rawr:FixGems(slotlink)
	if not Rawr.itemIDtoEnchantID then
		Rawr:GemToEnchants()
	end
	local _, itemlink, rarity = GetItemInfo(slotlink)
	if itemlink then
		local itemString = string.match(itemlink, "item[%-?%d:]+") or ""
		local linkType, itemId, enchantId, gem1, gem2, gem3, _, suffixId, uniqueId, linkLevel, reforgeId = strsplit(":", itemString)
		local jewel1 = Rawr.itemIDtoEnchantID[tonumber(gem1)] or 0
		local jewel2 = Rawr.itemIDtoEnchantID[tonumber(gem2)] or 0
		local jewel3 = Rawr.itemIDtoEnchantID[tonumber(gem3)] or 0
--		self:DebugPrint("gems : "..(gem1 or 0)..","..(gem2 or 0)..","..(gem3 or 0).." to jewels :"..jewel1..","..jewel2..","..jewel3)
		itemString = linkType..":"..itemId..":"..enchantId..":"..jewel1..":"..jewel2..":"..jewel3..":0:"..suffixId..":"..uniqueId..":"..linkLevel..":"..reforgeId
		_, itemlink, rarity = GetItemInfo(itemString)
	end
	return itemlink, rarity
end

function Rawr:PrintWarning(msg, col, time)
	if Rawr.db.char.warning.show then
		if col == nil then
			col = { r=1, b=1, g=1, a=1 }
		end
		if time == nil then 
			time = 3
		end
		if time ~= 5 then
			self.warningFrame:SetTimeVisible(time)
		end
		self.warningFrame:AddMessage(msg, col.r, col.g, col.b, 1, col.time)
	end
end