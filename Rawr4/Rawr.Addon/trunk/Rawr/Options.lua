if not Rawr then return end

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")
local media = LibStub:GetLibrary("LibSharedMedia-3.0", true)

-------------------
-- Config defaults
-------------------

Rawr.defaults = {
	char = {
		regionNumber = 1,
		showchanges = false,
		dataloaded = false,
		debug = false,
		sounds = {
			massiveupgrade = { value = 0.20, soundname = "Upgrade Sound 1", sound = "Sound\\Spells\\ShootWandLaunchLightning.ogg" },
			majorupgrade = { value = 0.10, soundname = "Upgrade Sound 2", sound = "Sound\\Spells\\DynamiteExplode.ogg", },
			upgrade = { value = 0.05, soundname = "Upgrade Sound 3", sound = "Sound\\Spells\\ArmorKitBuffSound.ogg", },
			minorupgrade = {value = 0.00, soundname = "Upgrade Sound 4", sound = "Sound\\Spells\\Fizzle\\FizzleShadowA.ogg", },
		},
	}, 
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
			sounds = {
				name = L["Sound Options"],
				type = 'group',
				order = 2,
				args = {
					massiveupgrade = {
						name = L["Massive Upgrade"],
						type = 'group',
						order = 1,
						args = {
							value = {
								name = L["Massive Upgrade Value"],
								type = 'range',
								desc = L["help_massive_upgrade_value"],
								min = 0.00,
								max = 1.00,
								softMax = 0.50,
								step = 0.005,
								isPercent = true,
								get = "GetMassiveUpgradeValue",
								set = "SetMassiveUpgradeValue",
								order = 1,
							},
							sound = {
								type = 'select',
								name = L["Massive Upgrade Sound"],
								desc = L["help_massive_upgrade_sound"],
								get = "GetMassiveUpgradeSound",
								set = "SetMassiveUpgradeSound",
								values = Rawr.sounds,
								order = 2,
							},
						},
					},
					majorupgrade = {
						name = L["Major Upgrade"],
						type = 'group',
						order = 2,
						args = {
							value = {
								type = 'range',
								name = L["Major Upgrade Value"],
								desc = L["help_major_upgrade_value"],
								min = 0.00,
								max = 1.00,
								step = 0.005,
								softMax = 0.50,
								isPercent = true,
								get = "GetMajorUpgradeValue",
								set = "SetMajorUpgradeValue",
								order = 1,
							},
							sound = {
								type = 'select',
								name = L["Major Upgrade Sound"],
								desc = L["help_major_upgrade_sound"],
								get = "GetMajorUpgradeSound",
								set = "SetMajorUpgradeSound",
								values = Rawr.sounds,
								order = 2,
							},
						},
					},
					upgrade = {
						name = L["Upgrade"],
						type = 'group',
						order = 3,
						args = {
							value = {
								type = 'range',
								name = L["Upgrade Value"],
								desc = L["help_upgrade_value"],
								min = 0.00,
								max = 1.00,
								step = 0.005,
								softMax = 0.50,
								isPercent = true,
								get = "GetUpgradeValue",
								set = "SetUpgradeValue",
								order = 1,
							},
							sound = {
								type = 'select',
								name = L["Upgrade Sound"],
								desc = L["help_upgrade_sound"],
								get = "GetUpgradeSound",
								set = "SetUpgradeSound",
								values = Rawr.sounds,
								order = 2,
							},
						},
					},
					minorupgrade = {
						name = L["Minor Upgrade"],
						type = 'group',
						order = 4,
						args = {
							value = {
								type = 'range',
								name = L["Minor Upgrade Value"],
								desc = L["help_minor_upgrade_value"],
								min = 0.00,
								max = 1.00,
								step = 0.005,
								softMax = 0.50,
								isPercent = true,
								get = "GetMinorUpgradeValue",
								set = "SetMinorUpgradeValue",
								order = 1,
							},
							sound = {
								type = 'select',
								name = L["Minor Upgrade Sound"],
								desc = L["help_minor_upgrade_sound"],
								get = "GetMinorUpgradeSound",
								set = "SetMinorUpgradeSound",
								values = Rawr.sounds,
								order = 2,
							},
						},
					},
				},
			},
			export = {
				type = 'execute',
				name = L["Open Export Window"],
				desc = L["help_open"],
				func = "DisplayExportWindow",
				guiHidden = true,
				order = 10,
			},
			debug = {
				type = 'toggle',
				name = L["Debug mode"],
				desc = L["help_debug"],
				get = "GetDebug",
				set = "SetDebug",
				order = 11,
			},
			config = {
				type = 'execute',
				name = L["Configure Options"],
				desc = L["help_config"],
				func = "OpenConfig",
				guiHidden = true,
				order = 12,
			},
			version = {
				type = 'execute',
				name = L["Version"],
				desc = L["help_version"],
				func = "DisplayVersion",
				order = 13,
			},
			help = {
				type = 'description',
				name = L["help"],
				guiHidden = true,
				order = 14,
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

function Rawr:GetMassiveUpgradeValue()
	return Rawr.db.char.sounds.massiveupgrade.value
end

function Rawr:SetMassiveUpgradeValue(info, newvalue)
	-- prevent it going lower than major upgrade value
	if newvalue < Rawr.db.char.sounds.majorupgrade.value then
		newvalue = Rawr.db.char.sounds.majorupgrade.value
	end
	Rawr.db.char.sounds.massiveupgrade.value = newvalue
end

function Rawr:GetMassiveUpgradeSound()
	return Rawr.db.char.sounds.massiveupgrade.soundname
end

function Rawr:SetMassiveUpgradeSound(info, newvalue)
	self:DebugPrint("Massive Sound "..(newvalue or "nil"))
	local newsound = media:Fetch("sound", newvalue)
	Rawr.db.char.sounds.massiveupgrade.soundname = newvalue
	if newsound then
		Rawr.db.char.sounds.massiveupgrade.sound = newsound
		PlaySoundFile(newsound)
	else
		Rawr:DebugPrint(L["Sound not found. Trying to set :"]..newsound)
	end
end

function Rawr:GetMajorUpgradeValue()
	return Rawr.db.char.sounds.majorupgrade.value
end

function Rawr:SetMajorUpgradeValue(info, newvalue)
	-- prevent it going lower than upgrade value or higher than massive upgrade
	if newvalue < Rawr.db.char.sounds.upgrade.value then
		newvalue = Rawr.db.char.sounds.upgrade.value
	elseif newvalue > Rawr.db.char.sounds.massiveupgrade.value then
		newvalue = Rawr.db.char.sounds.massiveupgrade.value
	end
	Rawr.db.char.sounds.majorupgrade.value = newvalue
end

function Rawr:GetMajorUpgradeSound()
	return Rawr.db.char.sounds.majorupgrade.soundname
end

function Rawr:SetMajorUpgradeSound(info, newvalue)
	local newsound = media:Fetch("sound", newvalue)
	Rawr.db.char.sounds.majorupgrade.soundname = newvalue
	if newsound then
		Rawr.db.char.sounds.majorupgrade.sound = newsound
		PlaySoundFile(newsound)
	else
		Rawr:DebugPrint(L["Sound not found. Trying to set :"]..newsound)
	end
end

function Rawr:GetUpgradeValue()
	return Rawr.db.char.sounds.upgrade.value
end

function Rawr:SetUpgradeValue(info, newvalue)
	-- prevent it going lower than minor upgrade value or higher than major upgrade
	if newvalue < Rawr.db.char.sounds.minorupgrade.value then
		newvalue = Rawr.db.char.sounds.minorupgrade.value
	elseif newvalue > Rawr.db.char.sounds.majorupgrade.value then
		newvalue = Rawr.db.char.sounds.majorupgrade.value
	end
	Rawr.db.char.sounds.upgrade.value = newvalue
end

function Rawr:GetUpgradeSound()
	return Rawr.db.char.sounds.upgrade.soundname
end

function Rawr:SetUpgradeSound(info, newvalue)
	local newsound = media:Fetch("sound", newvalue)
	Rawr.db.char.sounds.upgrade.soundname = newvalue
	if newsound then
		Rawr.db.char.sounds.upgrade.sound = newsound
		PlaySoundFile(newsound)
	else
		Rawr:DebugPrint(L["Sound not found. Trying to set :"]..newsound)
	end
end

function Rawr:GetMinorUpgradeValue()
	return Rawr.db.char.sounds.minorupgrade.value
end

function Rawr:SetMinorUpgradeValue(info, newvalue)
	-- prevent it going higher than upgrade
	if newvalue > Rawr.db.char.sounds.upgrade.value then
		newvalue = Rawr.db.char.sounds.upgrade.value
	end
	Rawr.db.char.sounds.minorupgrade.value = newvalue
end

function Rawr:GetMinorUpgradeSound()
	return Rawr.db.char.sounds.minorupgrade.soundname
end

function Rawr:SetMinorUpgradeSound(info, newvalue)
	local newsound = media:Fetch("sound", newvalue)
	Rawr.db.char.sounds.minorupgrade.soundname = newvalue
	if newsound then
		Rawr.db.char.sounds.minorupgrade.sound = newsound
		PlaySoundFile(newsound)
	else
		Rawr:DebugPrint(L["Sound not found. Trying to set :"]..newsound)
	end
end
