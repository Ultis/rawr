
local L = LibStub("AceLocale-3.0"):NewLocale("Rawr", "enUS", true);
if not L then return end

------------------------------------------------------------------------------------
-- Compulsory translation section - if these are not translated addon will not work
-------------------------------------------------------------------------------------


------------------
-- Headers section
------------------

L["About"] = true
L["Version"] = true 
L["__URL__"] = "http://wow.curseforge.com/addons/rawr-official/"
L[" Loaded."] = true
L["Configure Options"] = true
L["Debug mode"] = true
L["Open Import Window"] = true
L["Open Export Window"] = true
L["Keybind Title"] = "Rawr Keybinds"
L["Global Region"] = true
L["Region set to :"] = true
L["Button Tooltip Text"] =  "Left click to open Export to Rawr Window\rRight click to open Rawr Character Sheet"
L["CheckButton Tooltip Text"] = "Tick box to filter upgrades list to show items for this slot"

------------------
-- Import/Export section
------------------

L["import_rawr"] = "Rawr Import\nPress Ctrl-V to copy data from the\nRawr Website into Rawr Addon\n\nIf you have a lot of upgrade items this may\ncause the game to appear to hang\nfor up to a minute or more"
L["export_rawr"] = "Rawr Export Data Ready\nPress Ctrl-C to copy details\n(Yes! I know it looks empty, it isn't)"
L["Load from Rawr"] = true
L["Showing Loaded"] = true
L["Showing Changed"] = true
L["Import Error"] = "Rawr: Invalid Rawr data entered please check you have copied data from Rawr to the clipboard using CTRL-C"
L["Player or Realm doesn't match logged in player"] = true
L["Rawr : Bank contents updated. %s equippable/usable items found."] = true
L["Battle.net/Loaded from Addon"] = true
L["Imported from Rawr Postprocessing"] = true
L["Upgrade List"] = true
L["Rawr version in data is too low cannot import data"] = true

------------------
-- Help section
------------------

L["help"] = "All options can be configured more easily using the Blizzard options menu. Press ESC and select addons then click on Rawr to configure."
L["help1"] = "All options can be configured more easily using the Blizzard options menu."
L["help2"] = "Press ESC and select addons then click on Rawr to configure."
L["help_debug"] = "Activate or deactivate debug messages" 
L["help_config"] = "Display easy to use graphical config panel"
L["help_version"] = "Show version information" 
L["help_open_import"] = "Open Import to Rawr Window"
L["help_open_export"] = "Open Export to Rawr Window"
L["help_major_upgrade_value"] = "Select the percentage above which to play the major upgrade sound"
L["help_upgrade_value"] = "Select the percentage above which to play the upgrade sound"
L["help_minor_upgrade_value"] = "Select the percentage above which to play the minor upgrade sound"
L["help_major_upgrade_sound"] = "Select the sound to play when a major upgrade is detected"
L["help_upgrade_sound"] = "Select the sound to play when an upgrade is detected"
L["help_minor_upgrade_sound"] = "Select the sound to play when a minor upgrade is detected"
L["help_warningframe"] = "Enable or disable use of warning message frame"
L["help_moveframe"] = "Click to enable/disable moving of the warning frame"

------------------
-- Config section
------------------

L["config_debug_on"] = "Debug info will now be displayed"
L["config_debug_off"] = "Debug info will NOT now be displayed"
L["config_warnframe_on"] = "Warning frame will now be used to display messages"
L["config_warnframe_off"] = "Warning frame will NOT now be used to display messages"

------------------
-- Sounds section
------------------

L["Sound Options"] = true
L["Major Upgrade"] = true
L["Upgrade"] = true
L["Minor Upgrade"] = true
L["Major Upgrade Value"] = true
L["Upgrade Value"] = true
L["Minor Upgrade Value"] = true
L["Major Upgrade Sound"] = true
L["Upgrade Sound"] = true
L["Minor Upgrade Sound"] = true
L["Sound not found. Trying to set :"] = true
L["Alert %s is in your Rawr upgrade list.\nIt is a %.2f%% upgrade."] = true

------------------
-- Warnings section
------------------

L["Warning Message Duration"] = true
L["colWarningMessage"] = "Select which colour to display the warning messages in"
L["Warning Msg Colour"] = true
L["Use Warning Frame"] = true
L["Warning Options"] = true
L["Move Frame"] = true
