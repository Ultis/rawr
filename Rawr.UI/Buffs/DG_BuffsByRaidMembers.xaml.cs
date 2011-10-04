using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;

namespace Rawr.UI
{
    public class PlayerBuffSet
    {
        public PlayerBuffSet() { }
        //
        public CharacterClass Class = CharacterClass.Warrior;
        public String Spec = "Unset";
        public Color Color = Colors.Brown;
        public Dictionary<String, String> BuffsToAdd = new Dictionary<String, String>();
        //
        public override string ToString()
        {
            string retVal = string.Format("{0} ({1})", Class, Spec);
            //
            foreach (String key in BuffsToAdd.Keys)
            {
                retVal += string.Format("\r\n - {0}", BuffsToAdd[key]);
            }
            //
            return retVal;
        }
    }
    public partial class DG_BuffsByRaidMembers : ChildWindow
    {
        public DG_BuffsByRaidMembers()
        {
            InitializeComponent();

#if !SILVERLIGHT
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            this.WindowState = System.Windows.WindowState.Normal;
#endif

            // Set up the dialog
            CB_RaidSize.SelectedIndex = 1;
            CB_Class2Add.SelectedIndex = 9;
            #region Individual Class Work
            RB_Druid_Bear.IsChecked = true;
            RB_DK_Frost.IsChecked = true;
            RB_Hunter_MM.IsChecked = true;
            RB_Mage_Arcane.IsChecked = true;
            RB_Paladin_Ret.IsChecked = true;
            RB_Priest_Holy.IsChecked = true;
            RB_Rogue_Combat.IsChecked = true;
            RB_Shaman_Enhance.IsChecked = true;
            RB_Warlock_Demon.IsChecked = true;
            RB_Warrior_Tank.IsChecked = true;
            #endregion
        }

        public List<PlayerBuffSet> TheSets = new List<PlayerBuffSet>();

        #region List Editing
        public static Color FromKnownColor(string colorName)
        {
#if SILVERLIGHT
            Line lne = (Line)XamlReader.Load("<Line xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Fill=\"" + colorName + "\" />");
#else
            Line lne;
            using (MemoryStream stream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes("<Line xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Fill=\"" + colorName + "\" />")))
            {
                lne = (Line)XamlReader.Load(stream);
            }
#endif
            return (Color)lne.Fill.GetValue(SolidColorBrush.ColorProperty);
        }
        private void RaidSizeCheck()
        {
            // Disable the Add button if we've hit raid size
            if (List_Classes.Items.Count >= (Int32)CB_RaidSize.SelectedItem)
            { BT_Add.IsEnabled = false; } else { BT_Add.IsEnabled = true; }
            // Disable the Delete Button if there is no one in the Raid
            if (List_Classes.Items.Count == 0)
            { BT_Delete.IsEnabled = false; } else { BT_Delete.IsEnabled = true; }
            if (List_Classes.Items.Count == 0)
            { BT_DeleteAll.IsEnabled = false; } else { BT_DeleteAll.IsEnabled = true; }
        }
        private void CB_RaidSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaidSizeCheck();
        }
        private void BT_Add_Click(object sender, RoutedEventArgs e)
        {
            #region Individual Class Work
            PlayerBuffSet theSet = new PlayerBuffSet();
            theSet.Color = Colors.White;
            switch ((string)CB_Class2Add.SelectedItem)
            {
                #region Death Knight
                case "Death Knight": {
                    theSet.Class = CharacterClass.DeathKnight;
                    theSet.Color = FromKnownColor("Maroon");
                    theSet.Spec = (RB_DK_Frost.IsChecked.GetValueOrDefault(false) ? "Frost" :
                                  RB_DK_Unholy.IsChecked.GetValueOrDefault(false) ? "Unholy" :
                                  RB_DK_Blood.IsChecked.GetValueOrDefault(false) ? "Blood" : "Frost");
                    // All Specs
                    theSet.BuffsToAdd.Add("Horn of Winter", "Buff: Horn of Winter (Str, Agi)");
                    // Spec Specific
                    if (theSet.Spec == "Frost") {
                        theSet.BuffsToAdd.Add("Improved Icy Talons", "Buff: Improved Icy Talons (Haste)");
                        theSet.BuffsToAdd.Add("Frost Fever", "Debuff: Frost Fever (Targ AtkSpd Reduc)");
                        theSet.BuffsToAdd.Add("Brittle Bones", "Debuff: Brittle Bones (PhysDmg)");
                    } else if (theSet.Spec == "Unholy") {
                        theSet.BuffsToAdd.Add("Ebon Plaguebringer", "Buff: Ebon Plaguebringer (SpellDmg Multiplier)");
                        theSet.BuffsToAdd.Add("Unholy Frenzy", "Buff: Unholy Fenzy (Temp Haste)");
                    } else if (theSet.Spec == "Blood") {
                        theSet.BuffsToAdd.Add("Scarlet Fever", "Buff: Scarlet Fever (Targ Dmg Dealt Reduc)");
                        theSet.BuffsToAdd.Add("Abomination's Might", "Buff: Abomination's Might (AP)");
                    }
                    break;
                }
                #endregion
                #region Druid
                case "Druid": {
                    theSet.Class = CharacterClass.Druid;
                    theSet.Color = FromKnownColor("Orange");
                    theSet.Spec = RB_Druid_Bear.IsChecked.GetValueOrDefault(false) ? "Bear" :
                                  RB_Druid_Cat.IsChecked.GetValueOrDefault(false) ? "Cat" :
                                  RB_Druid_Moonkin.IsChecked.GetValueOrDefault(false) ? "Moonkin" :
                                  RB_Druid_Tree.IsChecked.GetValueOrDefault(false) ? "Tree" : "Bear";
                    // All Specs
                    theSet.BuffsToAdd.Add("Mark of the Wild", "Buff: Mark of the Wild (Stat %, All Resist)");
                    theSet.BuffsToAdd.Add("Fearie Fire", "Debuff: Fearie Fire (Target Armor Reduction)");
                    // Spec Specific
                    if (theSet.Spec == "Bear") {
                        theSet.BuffsToAdd.Add("Leader of the Pack", "Buff: Leader of the Pack (Crit)");
                        theSet.BuffsToAdd.Add("Infected Wounds", "Debuff: Infected Wounds (Targ AtkSpd Reduc)");
                        theSet.BuffsToAdd.Add("Mangle", "Debuff: Mangle (Bleed Dmg Mult)");
                        theSet.BuffsToAdd.Add("Demoralizing Roar", "Debuff: Demoralizing Roar (Targ Dmg Dealt Reduc)");
                    } else if (theSet.Spec == "Cat") {
                        theSet.BuffsToAdd.Add("Leader of the Pack", "Buff: Leader of the Pack (Crit)");
                        theSet.BuffsToAdd.Add("Infected Wounds", "Debuff: Infected Wounds (Targ AtkSpd Reduc)");
                        theSet.BuffsToAdd.Add("Mangle", "Debuff: Mangle (Bleed Dmg Mult)");
                    } else if (theSet.Spec == "Moonkin") {
                        theSet.BuffsToAdd.Add("Moonkin Form", "Buff: Moonkin Form (SpellHaste)");
                        theSet.BuffsToAdd.Add("Earth and Moon", "Debuff: Earth and Moon (Targ Spell Dmg Vuln)");
                    } else if (theSet.Spec == "Tree") {
                        theSet.BuffsToAdd.Add("Revitalize", "Buff: Revitalize (Mana Regen)");
                    }
                    break;
                }
                #endregion
                #region Hunter
                case "Hunter": {
                    theSet.Class = CharacterClass.Hunter;
                    theSet.Color = FromKnownColor("Green");
                    theSet.Spec = RB_Hunter_BM.IsChecked.GetValueOrDefault(false) ? "BM" :
                                  RB_Hunter_MM.IsChecked.GetValueOrDefault(false) ? "MM" :
                                  RB_Hunter_SV.IsChecked.GetValueOrDefault(false) ? "SV" : "MM";
                    // Spec Specific
                    if (theSet.Spec == "BM") {
                        theSet.BuffsToAdd.Add("Ferocious Inspiration", "Buff: Ferocious Inspiration (Damage)");
                    } else if (theSet.Spec == "MM") {
                        theSet.BuffsToAdd.Add("Trueshot Aura", "Buff: Trueshot Aura (AP)");
                    } else if (theSet.Spec == "SV") {
                        theSet.BuffsToAdd.Add("Hunting Party", "Buff: Hunting Party (Haste)");
                    }
                    // Pet
                    if ((string)CB_Hunter_Pet.SelectedItem != "None" && (string)CB_Hunter_Pet.SelectedItem != "Other") {
                        theSet.BuffsToAdd.Add((string)CB_Hunter_Pet.SelectedItem, "Buff: " + (string)CB_Hunter_Pet.SelectedItem);
                    }
                    // Sting
                    if ((string)CB_Hunter_Sting.SelectedItem != "None" && (string)CB_Hunter_Sting.SelectedItem != "Other") {
                        theSet.BuffsToAdd.Add((string)CB_Hunter_Sting.SelectedItem, "Debuff: " + (string)CB_Hunter_Sting.SelectedItem);
                    }
                    // Hunter's Mark
                    if ((string)CB_Hunter_Mark.SelectedItem != "None") {
                        theSet.BuffsToAdd.Add((string)CB_Hunter_Mark.SelectedItem, "Debuff: " + (string)CB_Hunter_Mark.SelectedItem
                            + (theSet.Spec == "MM" && CK_Hunter_Mark.IsChecked.GetValueOrDefault(false) ? " (Imp)" : ""));
                    }
                    // Aspect
                    if ((string)CB_Hunter_Aspect.SelectedItem != "None" && (string)CB_Hunter_Aspect.SelectedItem != "Other") {
                        theSet.BuffsToAdd.Add((string)CB_Hunter_Aspect.SelectedItem, "Debuff: " + (string)CB_Hunter_Aspect.SelectedItem);
                    }
                    break;
                }
                #endregion
                #region Mage
                case "Mage": {
                    theSet.Class = CharacterClass.Mage;
                    theSet.Color = FromKnownColor("LightBlue");
                    theSet.Spec = RB_Mage_Frost.IsChecked.GetValueOrDefault(false) ? "Frost" :
                                  RB_Mage_Fire.IsChecked.GetValueOrDefault(false) ? "Fire" :
                                  RB_Mage_Arcane.IsChecked.GetValueOrDefault(false) ? "Arcane" : "Arcane";
                    // All Specs
                    theSet.BuffsToAdd.Add("Arcane Brilliance", "Buff: Arcane Brilliance (Intellect)");
                    theSet.BuffsToAdd.Add("Time Warp", "Buff: Time Warp (Temp Haste)");
                    // Spec Specific
                    if (theSet.Spec == "Frost") {
                        theSet.BuffsToAdd.Add("Enduring Winter", "Buff: Enduring Winter (Mana Regen)");
                    } else if (theSet.Spec == "Fire") {
                        theSet.BuffsToAdd.Add("Critical Mass", "Debuff: Critical Mass (SpellCrit Debuff)");
                    } else if (theSet.Spec == "Arcane") {
                        theSet.BuffsToAdd.Add("Arcane Tactics", "Buff: Arcane Tactics (Bonus Damage)");
                        // Focus Magic
                        if ((string)CB_Mage_Focus.SelectedItem != "None")
                        {
                            theSet.BuffsToAdd.Add((string)CB_Mage_Focus.SelectedItem, "Buff: " + (string)CB_Mage_Focus.SelectedItem);
                        }
                    }
                    break;
                }
                #endregion
                #region Paladin
                case "Paladin": {
                    theSet.Class = CharacterClass.Paladin;
                    theSet.Color = FromKnownColor("Pink");
                    theSet.Spec = RB_Paladin_Prot.IsChecked.GetValueOrDefault(false) ? "Prot" :
                                    RB_Paladin_Ret.IsChecked.GetValueOrDefault(false) ? "Retribution" :
                                    RB_Paladin_Holy.IsChecked.GetValueOrDefault(false) ? "Holy" : "Retribution";
                    // All Specs
                    // Spec Specific
                    if (theSet.Spec == "Prot") {
                        theSet.BuffsToAdd.Add("Judgements of the Just", "Debuff: Judgements of the Just (Targ AtkSpd Reduc)");
                        theSet.BuffsToAdd.Add("Vindication", "Debuff: Vindication (Targ Dmg Dealt Reduc)");
                    } else if (theSet.Spec == "Retribution") {
                        theSet.BuffsToAdd.Add("Communion", "Buff: Communion (Mana Regen)");
                        theSet.BuffsToAdd.Add("Sanctified Retribution", "Buff: Sanctified Retribution (Dmg %)");
                    } else if (theSet.Spec == "Holy") {
                    }
                    // Aura
                    if ((string)CB_Paladin_Aura.SelectedItem != "None") {
                        theSet.BuffsToAdd.Add((string)CB_Paladin_Aura.SelectedItem, "Buff: " + (string)CB_Paladin_Aura.SelectedItem);
                    }
                    // Blessing
                    if ((string)CB_Paladin_Blessing.SelectedItem != "None") {
                        theSet.BuffsToAdd.Add((string)CB_Paladin_Blessing.SelectedItem, "Buff: " + (string)CB_Paladin_Blessing.SelectedItem);
                    }
                    break;
                }
                #endregion
                #region Priest
                case "Priest": {
                    theSet.Class = CharacterClass.Priest;
                    theSet.Color = FromKnownColor("LightGray");
                    theSet.Spec = RB_Priest_D.IsChecked.GetValueOrDefault(false) ? "Disc" :
                                  RB_Priest_Holy.IsChecked.GetValueOrDefault(false) ? "Holy" :
                                  RB_Priest_S.IsChecked.GetValueOrDefault(false) ? "Shadow" : "Holy";
                    // All Specs
                    theSet.BuffsToAdd.Add("Power Word: Fortitude", "Buff: Power Word Fortitude (Stamina)");
                    theSet.BuffsToAdd.Add("Shadow Protection", "Buff: Shadow Protection (Shadow Resist)");
                    // Spec Specific
                    if (theSet.Spec == "Disc") {
                        theSet.BuffsToAdd.Add("Power Infusion", "Buff: Power Infusion (Temp Spell Haste, Mana Regen)");
                    } else if (theSet.Spec == "Holy") {
                        theSet.BuffsToAdd.Add("Inspiration", "Buff: Inspiration (Damage Taken Reduc)");
                        theSet.BuffsToAdd.Add("Hymn of Hope", "Buff: Hymn of Hope (Mana, Burst Mana Regen)");
                    } else if (theSet.Spec == "Shadow") {
                        theSet.BuffsToAdd.Add("Vampiric Touch", "Buff: Vampiric Touch (Mana Regen)");
                        theSet.BuffsToAdd.Add("Mind Quickening", "Buff: Mind Quickening (Spell Haste)");
                    }
                    break;
                }
                #endregion
                #region Rogue
                case "Rogue": {
                    theSet.Class = CharacterClass.Rogue;
                    theSet.Color = FromKnownColor("Yellow");
                    theSet.Spec = RB_Rogue_Combat.IsChecked.GetValueOrDefault(false) ? "Combat" :
                                  RB_Rogue_Assassin.IsChecked.GetValueOrDefault(false) ? "Assassin" :
                                  RB_Rogue_Subtlety.IsChecked.GetValueOrDefault(false) ? "Subtlety" : "Combat";
                    // All Specs
                    theSet.BuffsToAdd.Add("Master Poisoner", "Debuff: Master Poisoner (Targ SplDmg Taken)");
                    theSet.BuffsToAdd.Add("Expose Armor", "Debuff: Expose Armor (Targ Armor Reduc)");
                    // Spec Specific
                    if (theSet.Spec == "Combat") {
                        theSet.BuffsToAdd.Add("Savage Combat", "Debuff: Savage Combat (Phys Dmg %)");
                    } else if (theSet.Spec == "Assassin") {
                    } else if (theSet.Spec == "Subtlety") {
                        theSet.BuffsToAdd.Add("Honor Among Thieves", "Buff: Honor Among Thieves (Crit)");
                        theSet.BuffsToAdd.Add("Hemorrhage", "Debuff: Hemorrhage (Bleed Dmg %)");
                    }
                    // Tricks of the Trade
                    if (CB_Rogue_Tricks.SelectedIndex == 1) {
                        string text = "Tricks of the Trade" + (CK_Rogue_Tricks.IsChecked.GetValueOrDefault(false) ? " (Glyphed)" : "");
                        theSet.BuffsToAdd.Add(text, "Buff: " + text + " (Temp Dmg %)");
                    }
                    break;
                }
                #endregion
                #region Shaman
                case "Shaman": {
                    theSet.Class = CharacterClass.Shaman;
                    theSet.Color = FromKnownColor("Blue");
                    theSet.Spec = RB_Shaman_Enhance.IsChecked.GetValueOrDefault(false) ? "Enhance" :
                                  RB_Shaman_Elemental.IsChecked.GetValueOrDefault(false) ? "Elemental" :
                                  RB_Shaman_Resto.IsChecked.GetValueOrDefault(false) ? "Resto" : "Enhance";
                    // All Specs
                    theSet.BuffsToAdd.Add("Heroism/Bloodlust", "Buff: Heroism/Bloodlust (Temp Haste)");
                    // Spec Specific
                    if (theSet.Spec == "Enhance") {
                        theSet.BuffsToAdd.Add("Unleashed Rage", "Buff: Unleashed Rage (AP%)");
                    } else if (theSet.Spec == "Elemental") {
                        theSet.BuffsToAdd.Add("Elemental Oath", "Buff: Elemental Oath (Crit)");
                        theSet.BuffsToAdd.Add("Totemic Wrath", "Buff: Totemic Wrath (SP%)");
                    } else if (theSet.Spec == "Resto") {
                        theSet.BuffsToAdd.Add("Mana Tide Totem", "Buff: Mana Tide Totem (Burst Mana Regen)");
                        theSet.BuffsToAdd.Add("Ancestral Healing", "Buff: Ancestral Healing (DmgTakenReduc)");
                    }
                    // Air Totem
                    if (CB_Shaman_TotemAir.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Wrath of Air Totem", "Buff: Wrath of Air Totem (SpellHaste)");
                    } else if (CB_Shaman_TotemAir.SelectedIndex == 2) {
                        theSet.BuffsToAdd.Add("Windfury Totem", "Buff: Windfury Totem (PhysicalHaste)");
                    }
                    // Water Totem
                    if (CB_Shaman_TotemWater.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Mana Spring Totem", "Buff: Mana Spring Totem (Mana Regen)");
                    } else if (CB_Shaman_TotemWater.SelectedIndex == 2) {
                        theSet.BuffsToAdd.Add("Elemental Resistance Totem", "Buff: Elemental Resistance Totem (Fire, Frost, Nature Resist)");
                    }
                    // Fire Totem
                    if (CB_Shaman_TotemFire.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Flametongue Totem", "Buff: Flametongue Totem (SpellDamageAndHealingBonusMult)");
                    } else if (CB_Shaman_TotemFire.SelectedIndex == 2) {
                        theSet.BuffsToAdd.Add("Totem of Wrath", "Buff: Totem of Wrath (SpellPower)");
                    }
                    // Earth Totem
                    if (CB_Shaman_TotemEarth.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Stoneskin Totem", "Buff: Stoneskin Totem (BonusArmor)");
                    } else if (CB_Shaman_TotemEarth.SelectedIndex == 2) {
                        theSet.BuffsToAdd.Add("Strength of Earth Totem", "Buff: Strength of Earth Totem (Strength, Agility)");
                    }
                    break;
                }
                #endregion
                #region Warlock
                case "Warlock": {
                    theSet.Class = CharacterClass.Warlock;
                    theSet.Color = FromKnownColor("Purple");
                    theSet.Spec = RB_Warlock_Demon.IsChecked.GetValueOrDefault(false) ? "Demon" :
                                  RB_Warlock_Afflic.IsChecked.GetValueOrDefault(false) ? "Afflic" :
                                  RB_Warlock_Destro.IsChecked.GetValueOrDefault(false) ? "Destro" : "Demon";
                    // All Specs
                    theSet.BuffsToAdd.Add("Fel Intelligence (Mana)", "Buff: Fel Intelligence (Mana)");
                    theSet.BuffsToAdd.Add("Curse of Weakness", "Debuff: Curse of Weakness (Targ Dmg Dealt %)");
                    theSet.BuffsToAdd.Add("Improved Shadow Bolt", "Debuff: Improved Shadow Bolt (Targ Crit %)");
                    theSet.BuffsToAdd.Add("Curse of the Elements", "Debuff: Curse of the Elements (Targ SplDmg %)");
                    // Spec Specific
                    if (theSet.Spec == "Demon") {
                        theSet.BuffsToAdd.Add("Demonic Pact", "Buff: Demonic Pact (SP%)");
                    } else if (theSet.Spec == "Afflic") {
                    } else if (theSet.Spec == "Destro") {
                        theSet.BuffsToAdd.Add("Soul Leech", "Buff: Soul Leech (Mana Regen)");
                    }
                    // Pet
                    if (CB_Warlock_Pet.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Blood Pact", "Buff: Blood Pact (Stamina)");
                    } else if (CB_Warlock_Pet.SelectedIndex == 2) {
                        theSet.BuffsToAdd.Add("Fel Intelligence (Mp5)", "Buff: Fel Intelligence (Mp5)");
                    }
                    break;
                }
                #endregion
                #region Warrior
                case "Warrior": {
                    theSet.Class = CharacterClass.Warrior;
                    theSet.Color = FromKnownColor("BurlyWood");
                    theSet.Spec = RB_Warrior_Arms.IsChecked.GetValueOrDefault(false) ? "Arms" :
                                  RB_Warrior_Fury.IsChecked.GetValueOrDefault(false) ? "Fury" :
                                  RB_Warrior_Tank.IsChecked.GetValueOrDefault(false) ? "Prot" : "Arms";
                    // All Specs
                    // Spec Specific
                    if (theSet.Spec == "Prot") {
                    } else if (theSet.Spec == "Arms") {
                        theSet.BuffsToAdd.Add("Trauma", "Debuff: Trauma (Bleed Bonus)");
                        theSet.BuffsToAdd.Add("Blood Frenzy", "Debuff: Blood Frenzy (Phys Dmg Bonus)");
                        theSet.BuffsToAdd.Add("Shattering Throw", "Debuff: Shattering Throw (Temp Armor Reduc)");
                    } else if (theSet.Spec == "Fury") {
                        theSet.BuffsToAdd.Add("Rampage", "Buff: Rampage (Crit)");
                    }
                    // Buff Shout
                    if (CB_Warrior_BuffShout.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Battle Shout", "Buff: Battle Shout (Strength, Agility)");
                    } else if (CB_Warrior_BuffShout.SelectedIndex == 2) {
                        theSet.BuffsToAdd.Add("Commanding Shout", "Buff: Commanding Shout (Stamina)");
                    }
                    // Debuff Shout
                    if (CB_Warrior_DebuffShout.SelectedIndex == 1) {
                        theSet.BuffsToAdd.Add("Demoralizing Shout", "Debuff: Demoralizing Shout (Target Dmg Reduc)");
                    }
                    // Thunderclap
                    if (theSet.Spec == "Prot" || (theSet.Spec == "Arms" && CB_Warrior_Thunderclap.SelectedIndex == 1)) {
                        theSet.BuffsToAdd.Add("Thunder Clap", "Debuff: Thunder Clap (Target AtkSpd Reduc)");
                    }
                    // Sunder Armor
                    if (theSet.Spec == "Prot" || ((theSet.Spec == "Arms" || theSet.Spec == "Fury") && (CB_Warrior_Sunder.SelectedIndex == 1))) {
                        theSet.BuffsToAdd.Add("Sunder Armor", "Debuff: Sunder Armor (Target Armor Reduc)");
                    }
                    break;
                }
                #endregion
                default: { break; }
            }
            #endregion
            // Add if not blank
            if (theSet.BuffsToAdd.Keys.Count > 0)
            {
                ListBoxItem newAdd = new ListBoxItem();
                newAdd.Content = theSet.ToString();
                newAdd.Background = new SolidColorBrush(theSet.Color);
                newAdd.Background = new LinearGradientBrush(new GradientStopCollection() {
                    new GradientStop() { Color = Colors.White, Offset = 0 },
                    new GradientStop() { Color = theSet.Color, Offset = 1 }
                }, 0);
                List_Classes.Items.Add(newAdd);
                TheSets.Add(theSet);
            }
            // Verify we can add more people after this
            RaidSizeCheck();
        }
        private void BT_Delete_Click(object sender, RoutedEventArgs e)
        {
            // Make sure there is something to Remove
            if (List_Classes.SelectedItem == null) return;
            int index = List_Classes.SelectedIndex;
            // Remove the Listing
            List_Classes.Items.RemoveAt(index);
            TheSets.RemoveAt(index);
            // Finish off
            RaidSizeCheck();
        }
        private void BT_DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            // Remove all of the Listings
            List_Classes.Items.Clear();
            TheSets.Clear();
            // Finish off
            RaidSizeCheck();
        }
        #endregion

        private void CB_Class2Add_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GB_DeathKnight.Visibility = Visibility.Collapsed;
            GB_Druid.Visibility = Visibility.Collapsed;
            GB_Hunter.Visibility = Visibility.Collapsed;
            GB_Mage.Visibility = Visibility.Collapsed;
            GB_Priest.Visibility = Visibility.Collapsed;
            GB_Paladin.Visibility = Visibility.Collapsed;
            GB_Rogue.Visibility = Visibility.Collapsed;
            GB_Shaman.Visibility = Visibility.Collapsed;
            GB_Warlock.Visibility = Visibility.Collapsed;
            GB_Warrior.Visibility = Visibility.Collapsed;

            switch ((string)CB_Class2Add.SelectedItem)
            {
                case "Death Knight": { GB_DeathKnight.Visibility = Visibility.Visible; break; }
                case "Druid": { GB_Druid.Visibility = Visibility.Visible; break; }
                case "Hunter": { GB_Hunter.Visibility = Visibility.Visible; break; }
                case "Mage": { GB_Mage.Visibility = Visibility.Visible; break; }
                case "Priest": { GB_Priest.Visibility = Visibility.Visible; break; }
                case "Paladin": { GB_Paladin.Visibility = Visibility.Visible; break; }
                case "Rogue": { GB_Rogue.Visibility = Visibility.Visible; break; }
                case "Shaman": { GB_Shaman.Visibility = Visibility.Visible; break; }
                case "Warlock": { GB_Warlock.Visibility = Visibility.Visible; break; }
                case "Warrior": { GB_Warrior.Visibility = Visibility.Visible; break; }
                default: { break; }
            }
        }

        #region Individual Class Work
        // Druid
        private void CK_DruidSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Death Knight
        private void CK_DeathKnightSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Hunter
        private void CK_HunterSpec_Changed(object sender, RoutedEventArgs e)
        {
            if (RB_Hunter_BM.IsChecked.GetValueOrDefault(false)) {
                // Pet: Optional
                LB_Hunter_Pet.Visibility = Visibility.Visible;
                CB_Hunter_Pet.Visibility = Visibility.Visible;
                // Sting
                LB_Hunter_Sting.Visibility = Visibility.Visible;
                CB_Hunter_Sting.Visibility = Visibility.Visible;
                // Hunter's Mark
                LB_Hunter_Mark.Visibility = Visibility.Visible;
                CB_Hunter_Mark.Visibility = Visibility.Visible;
                CK_Hunter_Mark.Visibility = Visibility.Collapsed;
                // Aspect
                LB_Hunter_Aspect.Visibility = Visibility.Visible;
                CB_Hunter_Aspect.Visibility = Visibility.Visible;
            } else if (RB_Hunter_MM.IsChecked.GetValueOrDefault(false)) {
                // Pet: Optional
                LB_Hunter_Pet.Visibility = Visibility.Visible;
                CB_Hunter_Pet.Visibility = Visibility.Visible;
                // Sting
                LB_Hunter_Sting.Visibility = Visibility.Visible;
                CB_Hunter_Sting.Visibility = Visibility.Visible;
                // Hunter's Mark
                LB_Hunter_Mark.Visibility = Visibility.Visible;
                CB_Hunter_Mark.Visibility = Visibility.Visible;
                CK_Hunter_Mark.Visibility = Visibility.Visible;
                // Aspect
                LB_Hunter_Aspect.Visibility = Visibility.Visible;
                CB_Hunter_Aspect.Visibility = Visibility.Visible;
            } else if (RB_Hunter_SV.IsChecked.GetValueOrDefault(false)) {
                // Pet: Optional
                LB_Hunter_Pet.Visibility = Visibility.Visible;
                CB_Hunter_Pet.Visibility = Visibility.Visible;
                // Sting
                LB_Hunter_Sting.Visibility = Visibility.Visible;
                CB_Hunter_Sting.Visibility = Visibility.Visible;
                // Hunter's Mark
                LB_Hunter_Mark.Visibility = Visibility.Visible;
                CB_Hunter_Mark.Visibility = Visibility.Visible;
                CK_Hunter_Mark.Visibility = Visibility.Collapsed;
                // Aspect
                LB_Hunter_Aspect.Visibility = Visibility.Visible;
                CB_Hunter_Aspect.Visibility = Visibility.Visible;
            }
        }
        // Mage
        private void CK_MageSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Paladin
        private void CK_PaladinSpec_Changed(object sender, RoutedEventArgs e)
        {
            if (RB_Paladin_Prot.IsChecked.GetValueOrDefault(false)) {
                CB_Paladin_Aura.SelectedIndex = 2;
                CB_Paladin_Blessing.SelectedIndex = 1;
            } else if (RB_Paladin_Ret.IsChecked.GetValueOrDefault(false)) {
                CB_Paladin_Aura.SelectedIndex = 3;
                CB_Paladin_Blessing.SelectedIndex = 2;
            } else if (RB_Paladin_Holy.IsChecked.GetValueOrDefault(false)) {
                CB_Paladin_Aura.SelectedIndex = 1;
                CB_Paladin_Blessing.SelectedIndex = 1;
            } else {
                // Set them all to None
                CB_Paladin_Aura.SelectedIndex = 0;
                CB_Paladin_Blessing.SelectedIndex = 0;
            }
        }
        // Priest
        private void CK_PriestSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Rogue
        private void CK_RogueSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Shaman
        private void CK_ShamanSpec_Changed(object sender, RoutedEventArgs e)
        {
            if (RB_Shaman_Enhance.IsChecked.GetValueOrDefault(false)) {
                CB_Shaman_TotemAir.SelectedIndex = 2;
                CB_Shaman_TotemWater.SelectedIndex = 1;
                CB_Shaman_TotemFire.SelectedIndex = 3;
                CB_Shaman_TotemEarth.SelectedIndex = 2;
            } else if (RB_Shaman_Elemental.IsChecked.GetValueOrDefault(false)) {
                CB_Shaman_TotemAir.SelectedIndex = 1;
                CB_Shaman_TotemWater.SelectedIndex = 1;
                CB_Shaman_TotemFire.SelectedIndex = 2;
                CB_Shaman_TotemEarth.SelectedIndex = 2;
            } else if (RB_Shaman_Resto.IsChecked.GetValueOrDefault(false)) {
                CB_Shaman_TotemAir.SelectedIndex = 1;
                CB_Shaman_TotemWater.SelectedIndex = 1;
                CB_Shaman_TotemFire.SelectedIndex = 1;
                CB_Shaman_TotemEarth.SelectedIndex = 1;
            } else {
                // Set them all to None
                CB_Shaman_TotemAir.SelectedIndex = 0;
                CB_Shaman_TotemWater.SelectedIndex = 0;
                CB_Shaman_TotemFire.SelectedIndex = 0;
                CB_Shaman_TotemEarth.SelectedIndex = 0;
            }
        }
        // Warlock
        private void CK_WarlockSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Do nothing because there are no specialties to swap around
        }
        // Warrior
        private void CK_WarriorSpec_Changed(object sender, RoutedEventArgs e)
        {
            // Buff Shouts
            LB_Warrior_BuffShout.Visibility = Visibility.Collapsed;
            CB_Warrior_BuffShout.Visibility = Visibility.Collapsed;
            CB_Warrior_BuffShout.SelectedIndex = 0;
            // Debuff Shouts
            LB_Warrior_DebuffShout.Visibility = Visibility.Collapsed;
            CB_Warrior_DebuffShout.Visibility = Visibility.Collapsed;
            CB_Warrior_DebuffShout.SelectedIndex = 0;
            // Thunderclap
            LB_Warrior_Thunderclap.Visibility = Visibility.Collapsed;
            CB_Warrior_Thunderclap.Visibility = Visibility.Collapsed;
            CB_Warrior_Thunderclap.SelectedIndex = 0;
            // Sunder Armor
            LB_Warrior_Sunder.Visibility = Visibility.Collapsed;
            CB_Warrior_Sunder.Visibility = Visibility.Collapsed;
            CB_Warrior_Sunder.SelectedIndex = 0;

            if        (RB_Warrior_Tank.IsChecked.GetValueOrDefault(false)) {
                // Buff Shout: Optional
                LB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.SelectedIndex = 2;
                // Debuff Shout: Optional
                LB_Warrior_DebuffShout.Visibility = Visibility.Visible;
                CB_Warrior_DebuffShout.Visibility = Visibility.Visible;
                CB_Warrior_DebuffShout.SelectedIndex = 1;
                // Thunderclap: Always
                // Sunder: Always
            } else if (RB_Warrior_Arms.IsChecked.GetValueOrDefault(false)) {
                // Buff Shout: Optional
                LB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.SelectedIndex = 1;
                // Debuff Shout: Optional
                LB_Warrior_DebuffShout.Visibility = Visibility.Visible;
                CB_Warrior_DebuffShout.Visibility = Visibility.Visible;
                CB_Warrior_DebuffShout.SelectedIndex = 1;
                // Thunderclap: Optional
                LB_Warrior_Thunderclap.Visibility = Visibility.Visible;
                CB_Warrior_Thunderclap.Visibility = Visibility.Visible;
                CB_Warrior_Thunderclap.SelectedIndex = 1;
                // Sunder: Optional
                LB_Warrior_Sunder.Visibility = Visibility.Visible;
                CB_Warrior_Sunder.Visibility = Visibility.Visible;
                CB_Warrior_Sunder.SelectedIndex = 1;
            } else if (RB_Warrior_Fury.IsChecked.GetValueOrDefault(false)) {
                // Buff Shout: Optional
                LB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.Visibility = Visibility.Visible;
                CB_Warrior_BuffShout.SelectedIndex = 1;
                // Debuff Shout: Optional
                LB_Warrior_DebuffShout.Visibility = Visibility.Visible;
                CB_Warrior_DebuffShout.Visibility = Visibility.Visible;
                CB_Warrior_DebuffShout.SelectedIndex = 1;
                // Thunderclap: Unavailable
                // Sunder: Optional
                LB_Warrior_Sunder.Visibility = Visibility.Visible;
                CB_Warrior_Sunder.Visibility = Visibility.Visible;
                CB_Warrior_Sunder.SelectedIndex = 1;
            }
        }
        #endregion

        #region Dialog Exiting
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion
    }
}
