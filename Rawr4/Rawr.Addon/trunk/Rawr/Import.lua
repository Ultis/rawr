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
		editBox:SetAutoFocus(false)
		editBox:SetJustifyH("LEFT")
		editBox:SetJustifyV("TOP")
		editBox:SetText("")
		editBox:SetFocus()
		local dialogBox = editBox:GetParent()
		dialogBox:SetPoint("CENTER", "UIParent")
	end,
	OnAccept = function(self)
		Rawr:DebugPrint("Accept button pressed")
		local editBox = _G[self:GetName().."EditBox"]
		Rawr:ImportRawrData(editBox:GetText())
		self:Hide()
	end,
	OnCancel = function(self)
		Rawr:DebugPrint("Cancel button pressed")
		self:Hide()
	end,
	timeout = 0,
	hideOnEscape = 1,
}

function Rawr:ImportRawrData(editboxtext)
    self:DebugPrint("called ImportRawrData")
	if string.sub(editboxtext, 1, 16) ~= "Rawr:LoadWebData" then
		self:DebugPrint("Rawr: failed to find Rawr_App header")
		self:Print(L["Import Error"])
		return
	end
	local f, e = loadstring(editboxtext)
	if f then
		f()
	else
		self:Print(L["Import Error"])
		return
	end
	if Rawr.App.name ~= UnitName("player") or Rawr.App.realm ~= GetRealmName() then
		self:Print(L["Player or Realm doesn't match logged in player"])
	end
	Rawr:FillSlots()
	self.db.char.showchanges = false
	Rawr_PaperDollFrameChangesButton:Show()
end

function Rawr:LoadWebData(data)
	Rawr.App = data
	Rawr.App.version = Rawr.App.version or 0
	Rawr.App.realm = Rawr.App.realm or ""
	Rawr.App.name = Rawr.App.name or ""
	if Rawr.App.subpoints == nil then
		Rawr.App.subpoints = {}
		Rawr.App.subpoints.count = 0
	end
end
