if not Rawr then return end

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")

local format, len, lower = _G.string.format, _G.string.len, _G.string.lower
local gsub, trim = _G.string.gsub, _G.strtrim

local frame = CreateFrame("Frame", "Rawr_ExportFrame", UIParent, "DialogBoxFrame")

local outputText = ""
if not Rawr.vars then
	Rawr.vars = {}
end

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
	self:ExportTalents()
	self:ExportEquipped()
	self:ExportGlyphs()
	self:WriteXMLFooter()
	StaticPopup_Show("RAWR_EXPORT_WINDOW")
end

function Rawr:ExportBasics()
	self:AddLine(2, "<Name>"..UnitName("player").."</Name>")
	self:AddLine(2, "<Level>"..UnitLevel("player").."</Level>")
	self:AddLine(2, "<Class>"..UnitClass("player").."</Class>")
	self:AddLine(2, "<Race>"..UnitRace("player").."</Race>")
	self:AddLine(2, "<Realm>"..GetRealmName().."</Realm>")
	self:AddLine(2, "<Region>"..Rawr:GetRegionName().."</Region>")
end

function Rawr:ExportTalents()

end

function Rawr:ExportEquipped()

end

function Rawr:ExportGlyphs()

end
