--[[

Author : Levva of EU Khadgar 

Version 0.01
	Initial release of Concept code to curseforge 

	
--]]

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")
local AceAddon = LibStub("AceAddon-3.0")
Rawr = AceAddon:NewAddon("Rawr", "AceConsole-3.0", "AceEvent-3.0", "AceHook-3.0")
local REVISION = tonumber(("$Revision$"):match("%d+"))

-- Binding Variables
BINDING_HEADER_RAWR_TITLE = L["Keybind Title"]
BINDING_NAME_RAWR_OPEN_EXPORT = L["Open Export Window"]

-------------------
-- Config details
-------------------

Rawr.defaults = {
	char = {
		debug = false,
	}
}

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
 	--self:RegisterEvent("PLAYER_ENTERING_WORLD")
end


----------------------
-- Event Routines
----------------------

function Rawr:PLAYER_ENTERING_WORLD()

end

----------------------
-- Export Routines
----------------------

function Rawr:DisplayExportWindow()
	self:DebugPrint("Opened export window")
	self:ExportToRawr()
end

----------------------
-- Utility Routines
----------------------

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
