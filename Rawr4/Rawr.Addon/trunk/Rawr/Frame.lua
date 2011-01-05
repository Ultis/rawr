if not Rawr then return end

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")

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
			self:AddTooltipData(slot.item)
		else
			GameTooltip:SetText("|c"..colourRed.."Potentially unsafe link|c"..colourYellow.." - you may shift right click to view|nWARNING this may disconnect you from the server!")
		end
	else
		GameTooltip:SetText(_G[slot.slotName:upper()]) -- otherwise just show slot name
	end
	GameTooltip:Show()
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

function Rawr:AddTooltipData(item)
	if Rawr.App.subpoints.count > 0 then
		GameTooltip:AddLine("\r")
		GameTooltip:AddLine("Rawr", 1, 1, 1)
		GameTooltip:AddLine("Overall : "..item.overall)
		for index = 1, Rawr.App.subpoints.count  do
			local colour = self:GetColour(Rawr.App.subpoints.colour[index])
			GameTooltip:AddLine(Rawr.App.subpoints.subpoint[index].." : "..item.subpoint[index], colour.r, colour.g, colour.b)
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
	self:UpdateChangeButtonText()
	Rawr_PaperDollFrameChangesButton:Hide()
	Rawr_PaperDollFrame:SetPoint("BOTTOMLEFT", CharacterFrame, "BOTTOMRIGHT", 25, 0)
	Rawr:FillSlots()
	ShowUIPanel(Rawr_PaperDollFrame)
end

function Rawr:FillSlots()
	if self.db.char.dataloaded then
		Rawr_PaperDollFrameChangesButton:Show()
	end
	if Rawr.App.character ~= nil and Rawr.App.character.items ~= nil then 
		local items = Rawr.App.character.items
		local loadeditems = Rawr.App.loaded.items
		local rarity = 0
		for index, slot in ipairs(Rawr.slots) do
			button = _G["Rawr_PaperDoll_ItemButton"..slot.frame]
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
			for _, item in ipairs(loadeditems) do
				if item.slot == slot.slotId and item.item ~= nil then
					button.loadeditem = item
				end
			end
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
			SetItemButtonTexture(button,button.backgroundTextureName)
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