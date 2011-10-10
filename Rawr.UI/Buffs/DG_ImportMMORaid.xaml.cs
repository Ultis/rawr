namespace Rawr.UI
{
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

    public partial class DG_ImportMMMORaid : ChildWindow
    {
        public DG_ImportMMMORaid()
        {
            InitializeComponent();
        }

        public List<PlayerBuffSet> toAdds = new List<PlayerBuffSet>();
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            /* Example Build
             * http://raidcomp.mmo-champion.com/?c=123456789abcdefghijklmnopqrstuv000000000
             * 0 Empty
             * 1 Blood Death Knight     * 2 Frost Death Knight  * 3 Unholy Death Knight
             * 4 Balance Druid          * 5 Cat Druid           * 6 Restoration Druid   * v Bear Druid
             * 7 Beast Mastery Hunter   * 8 Marksmanship Hunter * 9 Survival Hunter
             * a Arcane Mage            * b Fire Mage           * c Frost Mage
             * d Holy Paladin           * e Protection Paladin  * f Retribution Paladin
             * g Discipline Priest      * h Holy Priest         * i Shadow Priest
             * j Assassination Rogue    * k Combat Rogue        * l Subtlety Rogue
             * m Elemental Shaman       * n Enhancement Shaman  * o Restoration Shaman
             * p Affliction Warlock     * q Demonology Warlock  * r Destruction Warlock
             * s Arms Warrior           * t Fury Warrior        * u Protection Warrior
            */
            string link = TB_CompLink.Text.Trim();
            string classString = link.Substring(link.IndexOf("c=")+2);
            while (classString.Contains('0')) { classString = classString.Replace("0", ""); }
            List<string> classes = new List<string>();
            foreach (char c in classString)
            {
                classes.Add(c.ToString());
            }
            toAdds = new List<PlayerBuffSet>();
            foreach (string c in classes)
            {
                PlayerBuffSet theSet = new PlayerBuffSet();
                theSet.Color = Colors.White;
                switch (c)
                {
                    #region Death Knight
                    case "1": case "2": case "3": {
                        theSet.Class = CharacterClass.DeathKnight;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("Maroon");
                        theSet.Spec = (c == "2" ? "Frost" : c == "3" ? "Unholy" : c == "1" ? "Blood" : "Frost");
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
                    case "4": case "5": case "6": case "v": {
                        theSet.Class = CharacterClass.Druid;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("Orange");
                        theSet.Spec = c == "v" ? "Bear" : c == "5" ? "Cat" : c == "4" ? "Moonkin" : c == "6" ? "Tree" : "Bear";
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
                    case "7": case "8": case "9": {
                        theSet.Class = CharacterClass.Hunter;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("Green");
                        theSet.Spec = c == "7" ? "BM" : c == "8" ? "MM" : c == "9" ? "SV" : "MM";
                        // Spec Specific
                        if (theSet.Spec == "BM") {
                            theSet.BuffsToAdd.Add("Ferocious Inspiration", "Buff: Ferocious Inspiration (Damage)");
                        } else if (theSet.Spec == "MM") {
                            theSet.BuffsToAdd.Add("Trueshot Aura", "Buff: Trueshot Aura (AP)");
                        } else if (theSet.Spec == "SV") {
                            theSet.BuffsToAdd.Add("Hunting Party", "Buff: Hunting Party (Haste)");
                        }
                        // Pet
                        //if ((string)CB_Hunter_Pet.SelectedItem != "None" && (string)CB_Hunter_Pet.SelectedItem != "Other") {
                            //theSet.BuffsToAdd.Add((string)CB_Hunter_Pet.SelectedItem, "Buff: " + (string)CB_Hunter_Pet.SelectedItem);
                        //}
                        // Sting
                        //if ((string)CB_Hunter_Sting.SelectedItem != "None" && (string)CB_Hunter_Sting.SelectedItem != "Other") {
                            //theSet.BuffsToAdd.Add((string)CB_Hunter_Sting.SelectedItem, "Debuff: " + (string)CB_Hunter_Sting.SelectedItem);
                        //}
                        // Hunter's Mark
                        //if ((string)CB_Hunter_Mark.SelectedItem != "None") {
                            //theSet.BuffsToAdd.Add((string)CB_Hunter_Mark.SelectedItem, "Debuff: " + (string)CB_Hunter_Mark.SelectedItem
                                //+ (theSet.Spec == "MM" && CK_Hunter_Mark.IsChecked.GetValueOrDefault(false) ? " (Imp)" : ""));
                        //}
                        // Aspect
                        //if ((string)CB_Hunter_Aspect.SelectedItem != "None" && (string)CB_Hunter_Aspect.SelectedItem != "Other") {
                            //theSet.BuffsToAdd.Add((string)CB_Hunter_Aspect.SelectedItem, "Debuff: " + (string)CB_Hunter_Aspect.SelectedItem);
                        //}
                        break;
                    }
                    #endregion
                    #region Mage
                    case "a": case "b": case "c": {
                        theSet.Class = CharacterClass.Mage;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("LightBlue");
                        theSet.Spec = c == "c" ? "Frost" : c == "b" ? "Fire" : c == "a" ? "Arcane" : "Arcane";
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
                            //if ((string)CB_Mage_Focus.SelectedItem != "None") {
                                theSet.BuffsToAdd.Add("Focus Magic", "Buff: Focus Magic");
                            //}
                        }
                        break;
                    }
                    #endregion
                    #region Paladin
                        case "d": case "e": case "f": {
                        theSet.Class = CharacterClass.Paladin;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("Pink");
                        theSet.Spec = c == "e" ? "Prot" : c == "f" ? "Retribution" : c == "d" ? "Holy" : "Retribution";
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
                        //if ((string)CB_Paladin_Aura.SelectedItem != "None") {
                            //theSet.BuffsToAdd.Add((string)CB_Paladin_Aura.SelectedItem, "Buff: " + (string)CB_Paladin_Aura.SelectedItem);
                        //}
                        // Blessing
                        //if ((string)CB_Paladin_Blessing.SelectedItem != "None") {
                            //theSet.BuffsToAdd.Add((string)CB_Paladin_Blessing.SelectedItem, "Buff: " + (string)CB_Paladin_Blessing.SelectedItem);
                        //}
                        break;
                    }
                    #endregion
                    #region Priest
                    case "g": case "h": case "i": {
                        theSet.Class = CharacterClass.Priest;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("LightGray");
                        theSet.Spec = c == "g" ? "Disc" : c == "h" ? "Holy" : c == "i" ? "Shadow" : "Holy";
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
                    case "j": case "k": case "l": {
                        theSet.Class = CharacterClass.Rogue;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("Yellow");
                        theSet.Spec = c == "k" ? "Combat" : c == "j" ? "Assassin" : c == "l" ? "Subtlety" : "Combat";
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
                        //if (CB_Rogue_Tricks.SelectedIndex == 1) {
                            string text = "Tricks of the Trade";
                            theSet.BuffsToAdd.Add(text, "Buff: " + text + " (Temp Dmg %)");
                        //}
                        break;
                    }
                    #endregion
                    #region Shaman
                    case "m": case "n": case "o": {
                        theSet.Class = CharacterClass.Shaman;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("Blue");
                        theSet.Spec = c == "n" ? "Enhance" : c == "m" ? "Elemental" : c == "o" ? "Resto" : "Enhance";
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
                        //if (CB_Shaman_TotemAir.SelectedIndex == 1) {
                            //theSet.BuffsToAdd.Add("Wrath of Air Totem", "Buff: Wrath of Air Totem (SpellHaste)");
                        //} else if (CB_Shaman_TotemAir.SelectedIndex == 2) {
                            //theSet.BuffsToAdd.Add("Windfury Totem", "Buff: Windfury Totem (PhysicalHaste)");
                        //}
                        // Water Totem
                        //if (CB_Shaman_TotemWater.SelectedIndex == 1) {
                            //theSet.BuffsToAdd.Add("Mana Spring Totem", "Buff: Mana Spring Totem (Mana Regen)");
                        //} else if (CB_Shaman_TotemWater.SelectedIndex == 2) {
                            //theSet.BuffsToAdd.Add("Elemental Resistance Totem", "Buff: Elemental Resistance Totem (Fire, Frost, Nature Resist)");
                        //}
                        // Fire Totem
                        //if (CB_Shaman_TotemFire.SelectedIndex == 1) {
                            //theSet.BuffsToAdd.Add("Flametongue Totem", "Buff: Flametongue Totem (SpellDamageAndHealingBonusMult)");
                        //} else if (CB_Shaman_TotemFire.SelectedIndex == 2) {
                            //theSet.BuffsToAdd.Add("Totem of Wrath", "Buff: Totem of Wrath (SpellPower)");
                        //}
                        // Earth Totem
                        //if (CB_Shaman_TotemEarth.SelectedIndex == 1) {
                            //theSet.BuffsToAdd.Add("Stoneskin Totem", "Buff: Stoneskin Totem (BonusArmor)");
                        //} else if (CB_Shaman_TotemEarth.SelectedIndex == 2) {
                            //theSet.BuffsToAdd.Add("Strength of Earth Totem", "Buff: Strength of Earth Totem (Strength, Agility)");
                        //}
                        break;
                    }
                    #endregion
                    #region Warlock
                    case "p": case "q": case "r": {
                        theSet.Class = CharacterClass.Warlock;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("Purple");
                        theSet.Spec = c == "q" ? "Demon" : c == "p" ? "Afflic" : c == "r" ? "Destro" : "Demon";
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
                        //if (CB_Warlock_Pet.SelectedIndex == 1) {
                            theSet.BuffsToAdd.Add("Blood Pact", "Buff: Blood Pact (Stamina)");
                        //} else if (CB_Warlock_Pet.SelectedIndex == 2) {
                            theSet.BuffsToAdd.Add("Fel Intelligence (Mp5)", "Buff: Fel Intelligence (Mp5)");
                        //}
                        break;
                    }
                    #endregion
                    #region Warrior
                    case "s": case "t": case "u": {
                        theSet.Class = CharacterClass.Warrior;
                        theSet.Color = DG_BuffsByRaidMembers.FromKnownColor("BurlyWood");
                        theSet.Spec = c == "s" ? "Arms" : c == "t" ? "Fury" : c == "u" ? "Prot" : "Arms";
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
                        //if (CB_Warrior_BuffShout.SelectedIndex == 1) {
                            theSet.BuffsToAdd.Add("Battle Shout", "Buff: Battle Shout (Strength, Agility)");
                        //} else if (CB_Warrior_BuffShout.SelectedIndex == 2) {
                            theSet.BuffsToAdd.Add("Commanding Shout", "Buff: Commanding Shout (Stamina)");
                        //}
                        // Debuff Shout
                        //if (CB_Warrior_DebuffShout.SelectedIndex == 1) {
                            theSet.BuffsToAdd.Add("Demoralizing Shout", "Debuff: Demoralizing Shout (Target Dmg Reduc)");
                        //}
                        // Thunderclap
                        //if (theSet.Spec == "Prot" || (theSet.Spec == "Arms" && CB_Warrior_Thunderclap.SelectedIndex == 1)) {
                            theSet.BuffsToAdd.Add("Thunder Clap", "Debuff: Thunder Clap (Target AtkSpd Reduc)");
                        //}
                        // Sunder Armor
                        //if (theSet.Spec == "Prot" || ((theSet.Spec == "Arms" || theSet.Spec == "Fury") && (CB_Warrior_Sunder.SelectedIndex == 1))) {
                            theSet.BuffsToAdd.Add("Sunder Armor", "Debuff: Sunder Armor (Target Armor Reduc)");
                        //}
                        break;
                    }
                    #endregion
                    default: { continue; } // invalid, don't add to the toAdds list
                }
                toAdds.Add(theSet);
            }
            //
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

