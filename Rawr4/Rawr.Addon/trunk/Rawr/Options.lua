local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")

-------------------
-- Config defaults
-------------------

Rawr.defaults = {
	char = {
		regionNumber = 1,
		debug = false,
	}
}

Rawr.regions = { "US", "EU", "KR", "TW", "CN" }

function Rawr:GetOptions()
	local options = { 
		name = "Rawr",
		handler = Rawr,
		type='group',
		childGroups ='tree',
		args = {
			region = {
				type = 'select',
				name = L["Global Region"],
				get = "GetRegion",
				set = "SetRegion",
				values = Rawr.regions,
				order = 1,
			},
			export = {
				type = 'execute',
				name = L["Open Export Window"],
				desc = L["help_open"],
				func = "DisplayExportWindow",
				guiHidden = true,
				order = 1,
			},
			debug = {
				type = 'toggle',
				name = L["Debug mode"],
				desc = L["help_debug"],
				get = "GetDebug",
				set = "SetDebug",
				order = 9,
			},
			config = {
				type = 'execute',
				name = L["Configure Options"],
				desc = L["help_config"],
				func = "OpenConfig",
				guiHidden = true,
				order = 13,
			},
			version = {
				type = 'execute',
				name = L["Version"],
				desc = L["help_version"],
				func = "DisplayVersion",
				order = 15,
			},
			help = {
				type = 'description',
				name = L["help"],
				guiHidden = true,
				order = 18,
			},
		},
	}
	return options
end

function Rawr:OpenConfig()
	if InterfaceOptionsFrame_OpenToCategory then
	    InterfaceOptionsFrame_OpenToCategory("Rawr");
    else
        InterfaceOptionsFrame_OpenToFrame("Rawr");
    end
end

function Rawr:GetDebug()
	return self.db.char.debug
end

function Rawr:SetDebug()
	self.db.char.debug = not self.db.char.debug
	if (self.db.char.debug) then
		self:Print(L["config_debug_on"])
	else
		self:Print(L["config_debug_off"])
	end
end

function Rawr:GetRegionName()
	return Rawr.regions[self.db.char.regionNumber]
end

function Rawr:GetRegion()
	return self.db.char.regionNumber
end

function Rawr:SetRegion(info, newvalue)
	self.db.char.regionNumber = newvalue
	self:Print(L["Region set to :"]..Rawr.regions[newvalue] )
end