local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")

function Rawr:GetOptions()
	local options = { 
		name = "Rawr",
		handler = Rawr,
		type='group',
		childGroups ='tree',
		args = {
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

function Rawr:OpenConfig()
	if InterfaceOptionsFrame_OpenToCategory then
	    InterfaceOptionsFrame_OpenToCategory("Rawr");
    else
        InterfaceOptionsFrame_OpenToFrame("Rawr");
    end
end
