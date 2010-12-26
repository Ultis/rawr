if not Rawr then return end

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")

local format, len, lower = _G.string.format, _G.string.len, _G.string.lower
local gsub, trim = _G.string.gsub, _G.strtrim

StaticPopupDialogs["RAWR_IMPORT_WINDOW"] = {
	text = L["import_rawr"],
	button1 = ACCEPT,
	button2 = CLOSE,
	hasEditBox = 1,
	OnShow = function(self)
		local editBox = _G[self:GetName().."EditBox"]
--		editBox:SetMultiLine(true)
--		editBox:SetHeight(200)
--		editBox:SetWidth(200)
		editBox:SetAutoFocus(false)
		editBox:SetJustifyH("LEFT")
		editBox:SetJustifyV("TOP")
		editBox:SetText("")
		editBox:SetFocus()
		local dialogBox = editBox:GetParent()
		dialogBox:SetPoint("CENTER", "UIParent")
	end,
	EditBoxOnEnterPressed = function(self)
		Rawr:DebugPrint("Accept button pressed")
		local editBox = _G[self:GetParent():GetName().."EditBox"]
		Rawr:ImportRawrData(editBox:GetText())
		self:GetParent():Hide()
	end,
	EditBoxOnEscapePressed = function(self)
		Rawr:DebugPrint("Cancel button pressed")
		self:GetParent():Hide()
	end,
	timeout = 0,
	hideOnEscape = 1,
}

function Rawr:ImportButton_OnClick()
	StaticPopup_Show("RAWR_IMPORT_WINDOW")
end

function Rawr:DirectUpgradesButton_OnClick()
	self:DebugPrint("This function isn't available yet")
end

function Rawr:ImportRawrData(editboxtext)
    self:DebugPrint("called ImportRawrData")
	self:DebugPrint(editboxtext)
end