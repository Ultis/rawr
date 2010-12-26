if not Rawr then return end

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")

local Slots = {
		"HeadSlot",
		"NeckSlot",
		"ShoulderSlot",
		"BackSlot",
		"ChestSlot",
		"ShirtSlot",
		"TabardSlot",
		"WristSlot",
		"HandsSlot",
		"WaistSlot",
		"LegsSlot",
		"FeetSlot",
		"Finger0Slot",
		"Finger1Slot",
		"Trinket0Slot",
		"Trinket1Slot",
		"MainHandSlot",
		"SecondaryHandSlot",
		"RangedSlot",
	}

function Rawr:CreateButton()
	self.button = CreateFrame("Button", "Rawr_ButtonFrame", PaperDollFrame)
	self.button:SetNormalTexture("Interface\\Addons\\Rawr\\Textures\\Rawr.tga")
	self.button:SetHeight(24)
	self.button:SetWidth(96)
	self.button:ClearAllPoints()
	self.button:SetPoint("BOTTOMLEFT", PaperDollFrame, "BOTTOMLEFT", 12, 12)
	self.button:RegisterForClicks("LeftButtonDown", "RightButtonDown")
	self.button:SetScript("OnClick", function(_, button) Rawr:ButtonClick(button) end)
	self.button:SetScript("OnEnter", function() Rawr:ShowButtonTooltip() end)
	self.button:Show()
end

function Rawr:ButtonClick(button)
	if button == "LeftButton" then
		self:ExportToRawr()
	elseif button == "RightButton" then
		self:ShowCharacterFrame()
	end
end

function Rawr:ShowButtonTooltip()
	if not InCombatLockdown() then
		GameTooltip:SetOwner(Rawr.button, "ANCHOR_BOTTOMLEFT")
		GameTooltip:AddLine(L["Button Tooltip Text"])
		GameTooltip:Show()
	else
		GameTooltip:Hide()
	end
end

function Rawr:ShowCharacterFrame()
	if not InCombatLockdown() then
		Rawr:ShowDoll()
	end
end

function Rawr:ItemSlots_OnLoad(slot)
	local buttonName = slot:GetName()
	local slotId
	slot.slotName = buttonName:sub(26)
	slotId, slot.backgroundTextureName = GetInventorySlotInfo(slot.slotName)
	_G[buttonName.."IconTexture"]:SetTexture(slot.backgroundTextureName)
	slot:SetID(slotId)
end

function Rawr:ItemSlots_OnEnter(slot)
	GameTooltip:SetOwner(slot,"ANCHOR_RIGHT")
	if (slot.link) then
		if (GetItemInfo(slot.link)) then
			GameTooltip:SetHyperlink(slot.link)  -- if item slot button has link show it in tooltip 
		else
			GameTooltip:SetText("|c"..colourRed.."Potentially unsafe link|c"..colourYellow.." - you may shift right click to view|nWARNING this may disconnect you from the server!")
		end
	else
		GameTooltip:SetText(_G[slot.slotName:upper()]) -- otherwise just show slot name
	end
end

function Rawr:ItemSlots_OnLeave(slot)
	ResetCursor()
	GameTooltip:Hide()
end

function Rawr:ItemSlots_OnClick(slot,button)
	if( button == "LeftButton" ) then
		if( IsShiftKeyDown() ) then
			if( ChatFrameEditBox:IsVisible() ) then
				ChatFrameEditBox:Insert(slot.link)
			else
				ChatEdit_InsertLink(slot.link)
			end
		elseif( IsControlKeyDown() ) then
			DressUpItemLink(slot.link)
		end
	elseif ( button == "RightButton" ) then
		if( IsShiftKeyDown() ) then
			GameTooltip:SetHyperlink(slot.link)
		end
	end
end

function Rawr:ShowDoll() 
	local button, levelColour
	Rawr_PaperDollFrameTitle:SetText(UnitName("player"))
	Rawr_PaperDollFrameDetails:SetText("Level "..UnitLevel("player").." "..UnitClass("player"))
	Rawr_PaperDollFrameGuild:SetText(GetGuildInfo("player"))
	SetPortraitTexture(Rawr_PaperDollFramePortrait, "player")
	Rawr_PaperDollFrameImportButton:SetText("  "..L["Load from Rawr"])
	Rawr_PaperDollFrameDirectUpgradesButton:SetText("  "..L["Direct Upgrades"])
	for _, slotName in ipairs(Slots) do
		button = _G["Rawr_PaperDoll_ItemButton"..slotName]
		levelColour = "ff808080"
--			if(playerInfo.itemList and playerInfo.itemList[slotName] and playerInfo.itemList[slotName].itemScore) then
--				button.link = playerInfo.itemList[slotName].slotLink
--				levelColour = playerInfo.itemList[slotName].levelColour
--				local itemRarity = playerInfo.itemList[slotName].itemRarity or -1
--				if(ITEM_RARITY[itemRarity].colour) then
--					textstring=" |c"..ITEM_RARITY[itemRarity].colour
--				end
--				textstring = textstring..format("%.2f", playerInfo.itemList[slotName].itemScore)..""
--			else
--				button.link = nil
--			end
--			Rawr:ItemSlots_UpdateItemSlot(button, levelColour)
		button:Show()
	end
	Rawr_PaperDollFrame:SetPoint("BOTTOMLEFT", CharacterFrame, "BOTTOMRIGHT", 25, 0)
	Rawr_PaperDollFrame:SetScale(0.64)
	ShowUIPanel(Rawr_PaperDollFrame)
end
