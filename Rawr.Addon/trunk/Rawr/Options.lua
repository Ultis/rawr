if not Rawr then return end

local L = LibStub("AceLocale-3.0"):GetLocale("Rawr")
local media = LibStub:GetLibrary("LibSharedMedia-3.0", true)

-------------------
-- Config defaults
-------------------

Rawr.defaultsounds = {
	{ name = "Ogre Cheer", file = "Sound\\Event Sounds\\OgreEventCheer1.ogg", },
	{ name = "Ghostly Laugh", file = "Sound\\Creature\\BabyLich\\GhostlySkullPetLaugh.ogg", },
	{ name = "Midsummer", file = "Sound\\Spells\\MidSummer-TorchGameComplete.ogg", },
	{ name = "Ogre Wardrum", file = "Sound\\Event Sounds\\Event_wardrum_ogre.ogg", },
	{ name = "Heroism", file = "Sound\\Spells\\Heroism_Cast.ogg", },
	{ name = "Magic Wand", file = "Sound\\Item\\UseSounds\\iMagicWand1.ogg", },
	{ name = "Water Small", file = "Sound\\Character\\footsteps\\EnterWaterSplash\\EnterWaterSmallA.ogg", },
	{ name = "Water Medium", file = "Sound\\Character\\footsteps\\EnterWaterSplash\\EnterWaterMediumA.ogg", },
	{ name = "Water Giant", file = "Sound\\Character\\footsteps\\EnterWaterSplash\\EnterWaterGiantA.ogg", },
	{ name = "Baby Murloc", file = "Sound\\Creature\\BabyMurloc\\BabyMurlocDance.ogg", },
	{ name = "Shadowmourne", file = "Sound\\Spells\\ShadowMourne_Cast_High_02.ogg", },
	{ name = "Simon Game", file = "Sound\\Spells\\SimonGame_Visual_GameStart.ogg", },
	{ name = "Sindragosa Frost", file = "Sound\\Spells\\Sindragosa_Xplosion_Frost_Impact_01.ogg", },
	{ name = "Archanite Ripper", file = "Sound\\Events\\ArchaniteRipper.ogg", },
	{ name = "Cheering", file = "Sound\\Events\\GuldanCheers.ogg", },
	{ name = "Scream", file = "Sound\\Events\\EbonHold_WomanScream4_01.ogg", },
	{ name = "Felreaver", file = "Sound\\Creature\\FelReaver\\FelReaverPreAggro.ogg", },
}

Rawr.defaults = {
	char = {
		regionNumber = 1,
		showchanges = false,
		dataloaded = false,
		debug = false,
		sounds = {
			majorupgrade = { value = 0.20, soundname = Rawr.defaultsounds[1].name, sound = Rawr.defaultsounds[1].file, },
			upgrade =        { value = 0.10, soundname = Rawr.defaultsounds[2].name, sound = Rawr.defaultsounds[2].file, },
			minorupgrade = {value = 0.00, soundname = Rawr.defaultsounds[3].name, sound = Rawr.defaultsounds[3].file, },
		},
		warning = {
			show = true,
			duration = 3,
			timeleft = 300, 
			colour = { r = 1, g = .5, b = 0, a = 0.5, },
			relativeTo = "UIParent",
			relativePoint = "TOP",
			point = "CENTER",
			fWidth = 400,
			fHeight = 75,
			xOffset = 0,
			yOffset = -250,
			moveframe = false,
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
					majorupgrade = {
						name = L["Major Upgrade"],
						type = 'group',
						order = 1,
						args = {
							value = {
								type = 'range',
								name = L["Major Upgrade Value"],
								desc = L["help_major_upgrade_value"],
								min = 0.00,
								max = 1.00,
								step = 0.005,
								softMax = 0.75,
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
						order = 2,
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
						order = 3,
						args = {
							value = {
								type = 'range',
								name = L["Minor Upgrade Value"],
								desc = L["help_minor_upgrade_value"],
								min = 0.00,
								max = 1.00,
								step = 0.005,
								softMax = 0.25,
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
			warning = {
				name = L["Warning Options"],
				type = 'group',
				order = 3, 
				args = {
					show = {
						type = 'toggle',
						name = L["Use Warning Frame"],
						desc = L["help_warningframe"],
						get = "GetWarningFrame",
						set = "SetWarningFrame",
						order = 1,
					},
					colour = {
						type = 'color',
						name = L["Warning Msg Colour"],
						desc = L["colWarningMessage"],
						get = "GetWarningColour",
						set = "SetWarningColour",
						hasAlpha = true,
						order = 2,
					},
					duration = {
						type = 'range',
						name = L["Warning Message Duration"],
						min = 1,
						max = 10,
						step = .2,
						get = "GetWarningDuration",
						set = "SetWarningDuration",
						order = 3,
					},
					moveframe = {
						type = 'execute',
						name = L["Move Frame"],
						desc = L["help_moveframe"],
						func = "MoveFrame",
						order = 4,
					},
				},
			},
			import = {
				type = 'execute',
				name = L["Open Import Window"],
				desc = L["help_open_import"],
				func = "DisplayImportWindow",
				guiHidden = true,
				order = 9,
			},
			export = {
				type = 'execute',
				name = L["Open Export Window"],
				desc = L["help_open_export"],
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

function Rawr:GetMajorUpgradeValue()
	return Rawr.db.char.sounds.majorupgrade.value
end

function Rawr:SetMajorUpgradeValue(info, newvalue)
	-- prevent it going lower than upgrade value 
	if newvalue < Rawr.db.char.sounds.upgrade.value then
		newvalue = Rawr.db.char.sounds.upgrade.value
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

---------------
-- Warnings
---------------

function Rawr:GetWarningFrame()
	return Rawr.db.char.warning.show
end

function Rawr:SetWarningFrame()
	Rawr.db.char.warning.show = not Rawr.db.char.warning.show
	if (Rawr.db.char.warning.show) then
		Rawr:Print(L["config_warnframe_on"])
	else
		Rawr:Print(L["config_warnframe_off"])
	end
end

function Rawr:GetWarningColour(info)
	local colours = Rawr.db.char.warning.colour
	return colours.r, colours.g, colours.b, colours.a
end

function Rawr:SetWarningColour(info,r,g,b,a)
	Rawr.db.char.warning.colour.r = r
	Rawr.db.char.warning.colour.g = g
	Rawr.db.char.warning.colour.b = b
	Rawr.db.char.warning.colour.a = a
end

function Rawr:GetWarningDuration(info)
	return Rawr.db.char.warning.duration 
end

function Rawr:SetWarningDuration(info, priorityValue)
	Rawr.db.char.warning.duration = priorityValue
end

function Rawr:MoveFrame()
	self.db.char.warning.moveframe = not self.db.char.warning.moveframe
	if self.db.char.warning.moveframe then
		self.warningFrame:EnableMouse(1)
		self.warningFrame:SetBackdropColor(0, 0, 0, 1)
		self.warningFrame:Show()
	else
		self.warningFrame:EnableMouse(0)
		self.warningFrame:SetBackdropColor(1, 1, 1, 0)
		self.warningFrame:Show()
	end
end