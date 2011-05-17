if not Rawr then return end

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")
local media = LibStub:GetLibrary("LibSharedMedia-3.0", true)

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

function Rawr:CreateWarningFrame()
	self.warningFrame = CreateFrame("MessageFrame", "RawrWarnings", UIParent)

	self.warningFrame:ClearAllPoints()
	self.warningFrame:SetWidth(400)
	self.warningFrame:SetHeight(75)
	self.warningFrame:SetFrameStrata("BACKGROUND")
	self.warningFrame:SetBackdrop({
		bgFile = "Interface/Tooltips/UI-Tooltip-Background",
		--edgeFile = "Interface/Tooltips/UI-Tooltip-Border",
		tile = false, tileSize = 0, edgeSize = 12,
		insets = { left = 2, right = 2, top = 2, bottom = 2 }
	})
	self.warningFrame:SetBackdropColor(1, 1, 1, 0)
	self.warningFrame:SetMovable(true)
	self.warningFrame:RegisterForDrag("LeftButton")
	self.warningFrame:SetScript("OnDragStart", 
		function()
			self.warningFrame:StartMoving();
		end );
	self.warningFrame:SetScript("OnDragStop",
		function()
			self.warningFrame:StopMovingOrSizing();
			self.warningFrame:SetScript("OnUpdate", nil);
			self:FinishedMoving(self.db.char.warning, self.warningFrame);
		end );
	self.warningFrame:SetPoint(self.db.char.warning.point, self.db.char.warning.relativeTo, self.db.char.warning.relativePoint, self.db.char.warning.xOffset, self.db.char.warning.yOffset)
	self.warningFrame:SetInsertMode("TOP")
	self.warningFrame:SetFrameStrata("HIGH")
	self.warningFrame:SetToplevel(true)
	local font = media:Fetch("font", "Friz Quadrata TT")
	self.warningFrame:SetFont(font, 24, "none")
		
	self.warningFrame:Show()
end

function Rawr:CreateTooltips()
	Rawr.tooltip = {}
	Rawr.tooltip.main = CreateFrame("GameTooltip", "RawrTooltipMain", UIParent, "GameTooltipTemplate")
	Rawr.tooltip.compare = CreateFrame("GameTooltip", "RawrTooltipCompare", UIParent, "GameTooltipTemplate")
	Rawr.tooltip.equipped = CreateFrame("GameTooltip", "RawrTooltipEquipped", UIParent, "GameTooltipTemplate")
	Rawr.tooltip.tinker = CreateFrame("GameTooltip", "RawrTooltipTinker", UIParent, "GameTooltipTemplate")
end

function Rawr:ShowCharacterFrame()
	if not InCombatLockdown() then
		Rawr:ShowDoll()
	end
end

function Rawr:ItemSlots_OnLoad(slot)
	local buttonName = slot:GetName()
	local slotId
	slot.slotName = buttonName:sub(20) -- Rawr_PaperDoll_Item is 19 chars long so 20th char is slot name
	slotId, slot.backgroundTextureName = GetInventorySlotInfo(slot.slotName)
	_G[buttonName.."IconTexture"]:SetTexture(slot.backgroundTextureName)
	slot:SetID(slotId)
end

function Rawr:ItemSlots_OnEnter(slot)
	self.tooltip.main:SetOwner(slot, "ANCHOR_RIGHT")
	self.tooltip.compare:SetOwner(self.tooltip.main, "ANCHOR_RIGHT")
	local showcompare = false
	if (slot.link) then
		self.tooltip.main:SetHyperlink(slot.link)  -- if item slot button has link show it in tooltip 
		if slot.upgrade then
			self:AddRawrUpgradeTooltipData(L["Upgrade List"], self.tooltip.main, slot.item, slot.loadeditem)
		else
			self:AddRawrTooltipData(L["Imported from Rawr Postprocessing"], self.tooltip.main, slot.item, slot.loadeditem)
		end
		if (slot.loadedlink) then
			self.tooltip.compare:ClearAllPoints()
			self.tooltip.compare:SetPoint("TOPLEFT", self.tooltip.main:GetName(), "TOPRIGHT")
			self.tooltip.compare:SetHyperlink(slot.loadedlink)
			self:AddRawrTooltipData(L["Battle.net/Loaded from Addon"], self.tooltip.compare, slot.loadeditem, nil)
			showcompare=true
		end
	else
		if slot.slotName then
			self.tooltip.main:SetText(_G[slot.slotName:upper()]) -- otherwise just show slot name
		else
			self.tooltip.main:SetText("")
		end
	end
	self.tooltip.main:Show()
	if showcompare then
		self.tooltip.compare:Show()
	else
		self.tooltip.compare:Hide()
	end
end

function Rawr:ItemSlots_OnLeave(slot)
	ResetCursor()
	self.tooltip.main:Hide()
	self.tooltip.compare:Hide()
end

function Rawr:ItemSlots_OnClick(slot, button, down)
	if( button == "LeftButton" ) then
		if( IsShiftKeyDown() ) then
			if( ChatFrame1EditBox:IsVisible() ) then
				ChatFrame1EditBox:Insert(slot.link)
			else
				ChatEdit_InsertLink(slot.link)
			end
		elseif( IsControlKeyDown() ) then
			DressUpItemLink(slot.link)
		end
	elseif ( button == "RightButton" ) then
		if( IsShiftKeyDown() ) then
			self.tooltip.main:SetHyperlink(slot.link)
		end
	end
end

function Rawr:ImportButton_OnClick()
	StaticPopup_Show("RAWR_IMPORT_WINDOW")
end

function Rawr:ChangesButton_OnClick()
	self.db.char.showchanges = not self.db.char.showchanges
	self:UpdateChangeButtonText()
end

function Rawr:UpdateChangeButtonText()
	if self.db.char.showchanges then
		Rawr_PaperDollFrameChangesButton:SetText("  "..L["Showing Changed"])
	else
		Rawr_PaperDollFrameChangesButton:SetText("  "..L["Showing Loaded"])
	end
	self:FillSlots()
end

function Rawr:AddRawrTooltipData(headertext, tooltip, item, comparison)
	local text
	if Rawr.db.char.App.subpoints.count > 0 then
		tooltip:AddLine("\n")
		tooltip:AddLine("Rawr: "..headertext, 1, 1, 1)
		text = "Overall : "..string.format("%.2f", item.overall)
		if comparison and comparison.overall then
			text = self:AddDifferenceText(text, item.overall, comparison.overall)
		end
		tooltip:AddLine(text)
		for index = 1, Rawr.db.char.App.subpoints.count  do
			local colour = self:GetColour(Rawr.db.char.App.subpoints.colour[index])
			local text = Rawr.db.char.App.subpoints.subpoint[index].." : "..string.format("%.2f", item.subpoint[index])
			if comparison and comparison.subpoint[index] then
				text = self:AddDifferenceText(text, item.subpoint[index], comparison.subpoint[index])
			end
			tooltip:AddLine(text, colour.r, colour.g, colour.b)
		end
	end
	if item.loc then
		tooltip:AddLine(item.loc)
	end
end

function Rawr:AddRawrUpgradeTooltipData(headertext, tooltip, item, comparison)
	local text
	if Rawr.db.char.App.subpoints.count > 0 then
		tooltip:AddLine("\n")
		tooltip:AddLine("Rawr: "..headertext, 1, 1, 1)
		text = "Overall : "
		if comparison and comparison.overall then
			local sum = item.overall + comparison.overall
			text = text..string.format("%.2f", sum)
			text = self:AddDifferenceText(text, sum, comparison.overall)
		else
			text = text..string.format("%.2f", item.overall)
		end
		tooltip:AddLine(text)
		for index = 1, Rawr.db.char.App.subpoints.count  do
			local colour = self:GetColour(Rawr.db.char.App.subpoints.colour[index])
			local text = Rawr.db.char.App.subpoints.subpoint[index].." : "
			if comparison and comparison.subpoint[index] then
				local sum = item.subpoint[index] + comparison.subpoint[index]
				text = text..string.format("%.2f", sum)
				text = self:AddDifferenceText(text, sum, comparison.subpoint[index])
			else
				text = text..string.format("%.2f", item.subpoint[index])
			end
			tooltip:AddLine(text, colour.r, colour.g, colour.b)
		end
	end
	if item.loc then
		tooltip:AddLine(item.loc)
	end
end

function Rawr:AddDifferenceText(text, newvalue, oldvalue)
	local difference = newvalue - oldvalue
	if difference > 0 then
		text = text .. " (+"
	else
		text = text .. " ("
	end
	return text..string.format("%.2f", difference)..")"
end

function Rawr:ShowDoll() 
	local button, levelColour
	Rawr_PaperDollFrameTitle:SetText(UnitName("player"))
	Rawr_PaperDollFrameDetails:SetText("Level "..UnitLevel("player").." "..UnitClass("player"))
	Rawr_PaperDollFrameGuild:SetText(GetGuildInfo("player"))
	SetPortraitTexture(Rawr_PaperDollFramePortrait, "player")
	Rawr_PaperDollFrameImportButton:SetText("  "..L["Load from Rawr"])
	self:UpdateChangeButtonText()
	if not Rawr.db.char.App.character then
		Rawr_PaperDollFrameChangesButton:Hide()
	end
	Rawr_PaperDollFrame:SetPoint("BOTTOMLEFT", CharacterFrame, "BOTTOMRIGHT", 25, 0)
	Rawr:FillSlots()
	Rawr:BuildUpgradeList()
	ShowUIPanel(Rawr_PaperDollFrame)
end

function Rawr:FillSlots()
	if self.db.char.dataloaded then
		Rawr_PaperDollFrameChangesButton:Show()
	end
	if Rawr.db.char.App.character ~= nil and Rawr.db.char.App.character.items ~= nil then 
		local items = Rawr.db.char.App.character.items
		local loadeditems = Rawr.db.char.App.loaded.items
		local rarity = 0
		for index, slot in ipairs(Rawr.slots) do
			button = _G["Rawr_PaperDoll_Item"..slot.frame]
			levelColour = Rawr.Colour.None
			for _, item in ipairs(items) do
				if item.slot == slot.slotId and item.item ~= nil then
					_, button.link, rarity = GetItemInfo(item.item)
					if rarity == 1 then
						levelColour = Rawr.Colour.White
					elseif rarity == 2 then
						levelColour = Rawr.Colour.Green
					elseif rarity == 3 then
						levelColour = Rawr.Colour.Blue
					elseif rarity == 4 then
						levelColour = Rawr.Colour.Purple
					elseif rarity == 5 then
						levelColour = Rawr.Colour.Orange
					elseif rarity == 6 or rarity == 7 then
						levelColour = Rawr.Colour.Gold
					end
					Rawr:ItemSlots_UpdateItemSlot(button, levelColour)
					button.item = item
				end
			end
			for _, loadeditem in ipairs(loadeditems) do
				if loadeditem.slot == slot.slotId and loadeditem.item ~= nil then
					button.loadeditem = loadeditem
					_, button.loadedlink = GetItemInfo(loadeditem.item)
				end
			end
			button.upgrade = false
			button:Show()
		end
	end
end

function Rawr:ItemSlots_UpdateItemSlot(button, levelColour)
	local border = _G[button:GetName().."BorderTexture"]
	if (button.link) then
		-- Only scan the item if it's in the users local cache, to avoid DC's
		if GetItemInfo(button.link) then
			-- Set Border Color
			if (levelColour == Rawr.Colour.Blue) then
				border:SetVertexColor(0.125,0.8125,1,1)
			elseif(levelColour == Rawr.Colour.Grey) then
				border:SetVertexColor(0.9,0.9,0.9,1)
			elseif(levelColour == Rawr.Colour.White) then
				border:SetVertexColor(1,1,1,1)
			elseif(levelColour == Rawr.Colour.Yellow) then
				border:SetVertexColor(1,1,0,1)
			elseif(levelColour == Rawr.Colour.Red) then
				border:SetVertexColor(1,0,0,1)
			elseif(levelColour == Rawr.Colour.Green) then
				border:SetVertexColor(0.117,1,0,1)
			elseif(levelColour == Rawr.Colour.DarkBlue) then
				border:SetVertexColor(0,0.4375,0.863,1)
			elseif(levelColour == Rawr.Colour.Purple) then
				border:SetVertexColor(0.637,0.207,0.93,1)
			elseif(levelColour == Rawr.Colour.Orange) then
				border:SetVertexColor(1,0.5,0,1)
			elseif(levelColour == Rawr.Colour.Gold) then
				border:SetVertexColor(0.898,0.797,0.5,1)
			else
				border:SetVertexColor(0.5,0.5,0.5,1)
			end
			local _, _, _, _, _, _, _, _, _, itemTexture = GetItemInfo(button.link)
			-- Set Texture
			if not self.db.char.showchanges then
				SetItemButtonTexture(button, itemTexture or button.backgroundTextureName)
			else
				if self:CheckChangedItem(button) then
					-- only showing changes and we have a different item so show it
					SetItemButtonTexture(button, itemTexture or button.backgroundTextureName)
				else
					-- only showing changes and we have the same item so show only empty slot
					SetItemButtonTexture(button, button.backgroundTextureName)
					border:SetVertexColor(0.5,0.5,0.5,1)
				end
			end
		else
			-- Cannot find link in local cache so potentially unsafe therefore border blue
			SetItemButtonTexture(button, button.backgroundTextureName)
			border:SetVertexColor(0.125,0.8125,1,1)
		end
		border:Show()
	else
		SetItemButtonTexture(button,button.backgroundTextureName)
		border:Hide()
	end
end

function Rawr:CheckChangedItem(slot)
	if slot.item and slot.loadeditem then
		local _, itemLink = GetItemInfo(slot.item.item)
		local itemString = string.match(itemLink, "item[%-?%d:]+") or ""
		_, itemLink = GetItemInfo(slot.loadeditem.item)
		local loadeditemString = string.match(itemLink, "item[%-?%d:]+") or ""
--		Rawr:DebugPrint("CheckChangedItem-Loaded:"..loadeditemString.." equipped:"..itemString)
		return itemString ~= loadeditemString
	else
		return false
	end
end

function Rawr:CheckChangedEquippedItem(slot)
	if slot.item then
		local _, itemLink = GetItemInfo(slot.item.item)
		local itemString = string.match(itemLink, "item[%-?%d:]+") or ""
		local slotLink = GetInventoryItemLink("player", slot.slotId)
		_, itemLink = GetItemInfo(slotLink)
		local equippeditemString = string.match(itemLink, "item[%-?%d:]+") or ""
		Rawr:DebugPrint("CheckChangedEquippedItem-Loaded:"..equippeditemString.." equipped:"..itemString)
		return itemString ~= equippeditemString
	else
		return false
	end
end

-------------------------------
-- Direct Upgrade Routines
-------------------------------

function Rawr:BuildUpgradeList()
	Rawr.upgrades = {}
	Rawr.upgrades.count = 0
	for index, slot in ipairs(Rawr.slots) do
		checkbutton = _G["Rawr_PaperDoll_Item"..slot.frame.."Check"]
		if checkbutton and checkbutton:GetChecked() then
			self:AddItemsToDisplay(slot.slotId)
		end	
	end
	table.sort(Rawr.upgrades, 
		function(a,b)
			return a.overall > b.overall
		end)
	self:DebugPrint("Upgrade List built "..Rawr.upgrades.count.." entries.")
	if Rawr.upgrades.count == 0 then
		Rawr_UpgradesFrameVSlider:SetMinMaxValues(0, 0)
	else
		Rawr_UpgradesFrameVSlider:SetMinMaxValues(1, Rawr.upgrades.count)
	end
	self:DisplayUpgradeList(1)
end

function Rawr:AddItemsToDisplay(slotId)
	if Rawr.db.char.App.upgrades then
		for _, upgrade in ipairs(Rawr.db.char.App.upgrades) do
			if upgrade.slot == slotId then
				table.insert(Rawr.upgrades, upgrade)
				Rawr.upgrades.count = Rawr.upgrades.count + 1
			end
		end
	end
end

function Rawr:DisplayUpgradeList(startpoint)
	local name = "Rawr_UpgradesFrameButton"
	for i=1, 6 do
		local index = startpoint + i -1
		local upgrade = Rawr.upgrades[index]
		local button = _G[name..i]
		local textfield = _G[name..i.."Text"]
		if not button then
			self:DebugPrint("Couldn't find button "..name..i)
		else
			if upgrade then
				local _, itemLink, _, _, _, _, _, _, _, itemTexture = GetItemInfo(upgrade.item)
				button.link = itemLink
				button.item = upgrade
				button.upgrade = true
				button.loadedlink, button.loadeditem = self:GetLoadedItem(upgrade.slot)
				button:SetNormalTexture(itemTexture)
				textfield:SetText("+"..string.format("%.2f", upgrade.overall))
				button:Show()
				textfield:Show()
			else
				button:Hide()
				textfield:Hide()
			end
		end
	end
end

function Rawr:GetLoadedItem(slotId)
	for index, slot in ipairs(Rawr.slots) do
		if slot.slotId == slotId then
			button = _G["Rawr_PaperDoll_Item"..slot.frame]
			return button.loadedlink, button.loadeditem
		end
	end	
	return nil, nil
end

function Rawr:DirectUpgrade_OnMouseWheel(_, delta)
	local current = Rawr_UpgradesFrameVSlider:GetValue()
	if (delta < 0) and (current < Rawr.upgrades.count) then
		Rawr_UpgradesFrameVSlider:SetValue(current + 1)
	elseif (delta > 0) and (current > 1) then
		Rawr_UpgradesFrameVSlider:SetValue(current - 1)
	end
end

--------------------------
-- Upgrade Buttons
--------------------------

function Rawr:CheckAll_OnClick()
	self:DebugPrint("Clicked CheckAll")
	for index, slot in ipairs(Rawr.slots) do
		checkbutton = _G["Rawr_PaperDoll_Item"..slot.frame.."Check"]
		if checkbutton then
			checkbutton:SetChecked(true)
		end	
	end
	Rawr:BuildUpgradeList()
end

function Rawr:ClearAll_OnClick()
	self:DebugPrint("Clicked ClearAll")
	for index, slot in ipairs(Rawr.slots) do
		checkbutton = _G["Rawr_PaperDoll_Item"..slot.frame.."Check"]
		if checkbutton then
			checkbutton:SetChecked(false)
		end	
	end
	Rawr:BuildUpgradeList()
end

function Rawr:LoadUpgradesList()
	self:DebugPrint("Loaded Upgrade form")
	Rawr_UpgradesFrameHeaderText:SetText("Direct Upgrades List")
	Rawr:BuildUpgradeList()
end

function Rawr:UpgradesScrollBarScrolled(scrollvalue)
	if not Rawr.upgrades then
		self:BuildUpgradeList()
	end
	self:DisplayUpgradeList(scrollvalue)
end

------------------------
-- Checkbox buttons
------------------------

function Rawr:CheckBoxToolTipShow(button)
	GameTooltip:SetOwner(button, "ANCHOR_BOTTOMLEFT")
	GameTooltip:AddLine(L["CheckButton Tooltip Text"])
	GameTooltip:Show()
end

function Rawr:SlotEnableClicked(button, arg1)
	-- need to setup toggles for filtering direct upgrades.
	self:BuildUpgradeList()
end