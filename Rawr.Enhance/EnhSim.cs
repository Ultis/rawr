using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Globalization;

namespace Rawr.Enhance
{
    public class EnhSim
    {
        private String _configText = String.Empty;
        private String _metagem = String.Empty;
        private String _mhEnchant = String.Empty;
        private String _ohEnchant = String.Empty;
        private String _trinket1name = String.Empty;
        private String _trinket2name = String.Empty;
        private String _totemname = String.Empty;
      
        public EnhSim(Character character)
        {
            CalculationsEnhance ce = new CalculationsEnhance();
            CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            CharacterCalculationsEnhance calcs = ce.GetCharacterCalculations(character, null) as CharacterCalculationsEnhance;
            Stats stats = calcs.BaseStats;
            Stats buffs = calcs.BuffStats;
            float baseMeleeCrit = StatConversion.GetCritFromRating(stats.CritMeleeRating + stats.CritRating) + 
                                  StatConversion.GetCritFromAgility(stats.Agility, character.Class) + .01f * character.ShamanTalents.ThunderingStrikes;
            float chanceCrit = 100f * Math.Min(0.75f, (1 + stats.BonusCritChance) * (baseMeleeCrit + stats.PhysicalCrit) + .00005f); //fudge factor for rounding
            float baseSpellCrit = StatConversion.GetSpellCritFromRating(stats.SpellCritRating + stats.CritRating) +
                                  StatConversion.GetSpellCritFromIntellect(stats.Intellect) + .01f * character.ShamanTalents.ThunderingStrikes;
            float chanceSpellCrit = 100f * Math.Min(0.75f, (1 + stats.BonusCritChance) * (baseSpellCrit + stats.SpellCrit) + .00005f); //fudge factor for rounding

            removeUseProcEffects(character, stats);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("##########################################");
            sb.AppendLine("### Rawr.Enhance Data Export to EnhSim ###");
            sb.AppendLine("##########################################");
            sb.AppendLine();
            float MHSpeed = character.MainHand == null ? 3.0f : character.MainHand.Item.Speed;
            float wdpsMH = character.MainHand == null ? 46.3f : character.MainHand.Item.DPS;
            float OHSpeed = character.OffHand == null ? 3.0f : character.OffHand.Item.Speed;
            float wdpsOH = character.OffHand == null ? 46.3f : character.OffHand.Item.DPS;

            sb.AppendLine("race                            " + character.Race.ToString().ToLower());
            sb.AppendLine("mh_speed                        " + MHSpeed.ToString());
            sb.AppendLine("oh_speed                        " + OHSpeed.ToString());
            sb.AppendLine("mh_dps                          " + wdpsMH.ToString("F1", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_dps                          " + wdpsOH.ToString("F1", CultureInfo.InvariantCulture));
            sb.AppendLine("mh_crit                         " + chanceCrit.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_crit                         " + chanceCrit.ToString("F2", CultureInfo.InvariantCulture));
            float hitBonus = StatConversion.GetHitFromRating(stats.HitRating) * 100f;
            sb.AppendLine("mh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("mh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("ap                              " + stats.AttackPower.ToString("F0", CultureInfo.InvariantCulture));
            float hasteBonus = StatConversion.GetHasteFromRating(stats.HasteRating, Character.CharacterClass.Shaman) * 100f;
            sb.AppendLine("melee_haste                     " + hasteBonus.ToString("F2", CultureInfo.InvariantCulture));
            float armourPenBonus = StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating) * 100f;
            sb.AppendLine("armor_penetration               " + armourPenBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("str                             " + stats.Strength.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("agi                             " + stats.Agility.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("int                             " + stats.Intellect.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spi                             " + stats.Spirit.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spellpower                      " + stats.SpellPower.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spell_crit                      " + chanceSpellCrit.ToString("F2", CultureInfo.InvariantCulture));
            hitBonus = StatConversion.GetSpellHitFromRating(stats.HitRating) * 100f;
            sb.AppendLine("spell_hit                       " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            hasteBonus = StatConversion.GetSpellHasteFromRating(stats.HasteRating) * 100f;
            sb.AppendLine("spell_haste                     " + hasteBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("max_mana                        " + stats.Mana.ToString());
            sb.AppendLine("mp5                             " + stats.Mp5.ToString());
            sb.AppendLine();
            sb.AppendLine("mh_imbue                        " + calcOpts.MainhandImbue.ToString().ToLower());
            sb.AppendLine("oh_imbue                        " + calcOpts.OffhandImbue.ToString().ToLower());
            sb.AppendLine();
            sb.AppendLine("mh_enchant                      " + _mhEnchant);
            String weaponType = "-";
            if (character.MainHand != null)
            {
                if (character.MainHand.Type == Item.ItemType.OneHandAxe)
                    weaponType = "axe";
                else if (character.MainHand.Type == Item.ItemType.TwoHandAxe)
                    weaponType = "axe";
            }
            sb.AppendLine("mh_weapon                       " + weaponType);
            sb.AppendLine("oh_enchant                      " + _ohEnchant);
            weaponType = "-";
            if (character.OffHand != null)
            {
                if (character.OffHand.Type == Item.ItemType.OneHandAxe)
                    weaponType = "axe";
                else if (character.OffHand.Type == Item.ItemType.TwoHandAxe)
                    weaponType = "axe";
            }
            sb.AppendLine("oh_weapon                       " + weaponType);
            sb.AppendLine();
            sb.AppendLine("trinket1                        " + _trinket1name);
            sb.AppendLine("trinket2                        " + _trinket2name);
            sb.AppendLine();
            sb.AppendLine("totem                           " + _totemname);
            sb.AppendLine(getSetBonuses(character));
            sb.AppendLine("metagem                         " + _metagem);
            sb.AppendLine();

            addGlyphs(character, sb);
            sb.AppendLine();
            sb.AppendLine("glyph_minor1                    -");
            sb.AppendLine("glyph_minor2                    -");
            sb.AppendLine("glyph_minor3                    -");
            sb.AppendLine();
            sb.AppendLine("###########");
            sb.AppendLine("# Talents #");
            sb.AppendLine("###########");
            sb.AppendLine();
            sb.AppendLine("ancestral_knowledge             " + character.ShamanTalents.AncestralKnowledge + "/5");
            sb.AppendLine("improved_shields                " + character.ShamanTalents.ImprovedShields + "/3");
            sb.AppendLine("mental_dexterity                " + character.ShamanTalents.MentalDexterity + "/3");
            sb.AppendLine("shamanistic_focus               " + character.ShamanTalents.ShamanisticFocus + "/1");
            sb.AppendLine("flurry                          " + character.ShamanTalents.Flurry + "/5");
            sb.AppendLine("elemental_weapons               " + character.ShamanTalents.ElementalWeapons + "/3");
            sb.AppendLine("unleashed_rage                  " + character.ShamanTalents.UnleashedRage + "/3");
            sb.AppendLine("weapon_mastery                  " + character.ShamanTalents.WeaponMastery + "/3");
            sb.AppendLine("dual_wield_specialization       " + character.ShamanTalents.DualWieldSpecialization + "/3");
            sb.AppendLine("mental_quickness                " + character.ShamanTalents.MentalQuickness + "/3");
            sb.AppendLine("improved_stormstrike            " + character.ShamanTalents.ImprovedStormstrike + "/2");
            sb.AppendLine("static_shock                    " + character.ShamanTalents.StaticShock + "/3");
            sb.AppendLine("maelstrom_weapon                " + character.ShamanTalents.MaelstromWeapon + "/5");
            sb.AppendLine("convection                      " + character.ShamanTalents.Convection + "/5");
            sb.AppendLine("concussion                      " + character.ShamanTalents.Concussion + "/5");
            sb.AppendLine("call_of_flame                   " + character.ShamanTalents.CallOfFlame + "/3");
            sb.AppendLine("elemental_devastation           " + character.ShamanTalents.ElementalDevastation + "/3");
            sb.AppendLine("reverberation                   " + character.ShamanTalents.Reverberation + "/5");
            sb.AppendLine("elemental_focus                 " + character.ShamanTalents.ElementalFocus + "/1");
            sb.AppendLine("elemental_fury                  " + character.ShamanTalents.ElementalFury + "/5");
            sb.AppendLine("call_of_thunder                 " + character.ShamanTalents.CallOfThunder + "/1");
            sb.AppendLine("unrelenting_storm               " + character.ShamanTalents.UnrelentingStorm + "/3");
            sb.AppendLine("elemental_precision             " + character.ShamanTalents.ElementalPrecision + "/3");
            sb.AppendLine("lightning_mastery               " + character.ShamanTalents.LightningMastery + "/5");
            sb.AppendLine("elemental_oath                  " + character.ShamanTalents.ElementalOath + "/2");
            sb.AppendLine("lightning_overload              " + character.ShamanTalents.LightningOverload + "/5");
            sb.AppendLine("lava_flows                      " + character.ShamanTalents.LavaFlows + "/3");
            sb.AppendLine("storm_earth_and_fire            " + character.ShamanTalents.StormEarthAndFire + "/3");
            sb.AppendLine("shamanism                       " + character.ShamanTalents.Shamanism + "/5");
            sb.AppendLine();

            addBuffs(character, sb);
            _configText = sb.ToString();
        }

        public void copyToClipboard()
        {
			try
			{
				Clipboard.SetText(_configText);
			}
			catch { }
            MessageBox.Show("EnhSim config data copied to clipboard\n" + 
                "Use the 'Copy from Clipboard' option in EnhSimGUI, v1.6.7 or higher, to use it\n" +
                "Or paste the config data into your EnhSim config file in a decent text editor (not Notepad)!",
                "Enhance Module", System.Windows.Forms.MessageBoxButtons.OK);
        }

        private void addBuffs(Character character, StringBuilder sb)
        {
            sb.AppendLine("#########");
            sb.AppendLine("# Buffs #");
            sb.AppendLine("#########");
            sb.AppendLine();

            // need to reference tickboxes on buffs form and see what is ticked for buffs.
            TabControl tabs = Calculations.CalculationOptionsPanel.Parent.Parent as TabControl;
            Control buffControl = tabs.TabPages[2].Controls[0];
            Type buffControlType = buffControl.GetType();

            if (buffControlType.FullName == "Rawr.BuffSelector")
            {
                PropertyInfo checkBoxesInfo = buffControlType.GetProperty("BuffCheckBoxes");
                Dictionary<Buff, CheckBox> checkBoxes = checkBoxesInfo.GetValue(buffControl, null) as Dictionary<Buff, CheckBox>;
                if (isBuffChecked(checkBoxes, "Acid Spit") || isBuffChecked(checkBoxes, "Expose Armor") || isBuffChecked(checkBoxes, "Sunder Armor"))
                    sb.AppendLine("armor_debuff_major              20.0/20.0");
                else
                    sb.AppendLine("armor_debuff_major              0.0/20.0");
                if (isBuffChecked(checkBoxes, "Curse of Recklessness") || isBuffChecked(checkBoxes, "Faerie Fire") || isBuffChecked(checkBoxes, "Sting"))
                    sb.AppendLine("armor_debuff_minor              5.0/5.0");
                else
                    sb.AppendLine("armor_debuff_minor              0.0/5.0");
                if (isBuffChecked(checkBoxes, "Blood Frenzy") || isBuffChecked(checkBoxes, "Savage Combat"))
                    sb.AppendLine("physical_vulnerability_debuff   4.0/4.0");
                else
                    sb.AppendLine("physical_vulnerability_debuff   0.0/4.0");
                if (isBuffChecked(checkBoxes, "Windfury Totem"))
                    if (isBuffChecked(checkBoxes, "Improved Windfury Totem"))
                        sb.AppendLine("melee_haste_buff                20.0/20.0");
                    else
                        sb.AppendLine("melee_haste_buff                16.0/20.0");
                else if (isBuffChecked(checkBoxes, "Improved Icy Talons"))
                    sb.AppendLine("melee_haste_buff                20.0/20.0");
                else
                    sb.AppendLine("melee_haste_buff                0.0/20.0");
                if (isBuffChecked(checkBoxes, "Leader of the Pack") || isBuffChecked(checkBoxes, "Rampage"))
                    sb.AppendLine("melee_crit_chance_buff          5.0/5.0");
                else
                    sb.AppendLine("melee_crit_chance_buff          0.0/5.0");
                if (isBuffChecked(checkBoxes, "Battle Shout"))
                {
                    if (isBuffChecked(checkBoxes, "Commanding Presence (Attack Power)"))
                        sb.AppendLine("attack_power_buff_flat          685/688");
                    else
                        sb.AppendLine("attack_power_buff_flat          548/688");
                }
                else if (isBuffChecked(checkBoxes, "Blessing of Might"))
                {
                    if (isBuffChecked(checkBoxes, "Improved Blessing of Might"))
                        sb.AppendLine("attack_power_buff_flat          687/688");
                    else
                        sb.AppendLine("attack_power_buff_flat          550/688");
                }
                else
                    sb.AppendLine("attack_power_buff_flat          0/688");
                if (isBuffChecked(checkBoxes, "Trueshot Aura") || isBuffChecked(checkBoxes, "Unleashed Rage") || isBuffChecked(checkBoxes, "Abomination's Might"))
                    sb.AppendLine("attack_power_buff_multiplier    99.7/99.7");
                else
                    sb.AppendLine("attack_power_buff_multiplier    0.0/99.7");
                if (isBuffChecked(checkBoxes, "Wrath of Air Totem"))
                    sb.AppendLine("spell_haste_buff                5.0/5.0");
                else
                    sb.AppendLine("spell_haste_buff                0.0/5.0");
                if (isBuffChecked(checkBoxes, "Elemental Oath") || isBuffChecked(checkBoxes, "Moonkin Form"))
                    sb.AppendLine("spell_crit_chance_buff          5.0/5.0");
                else
                    sb.AppendLine("spell_crit_chance_buff          0.0/5.0");
                if (isBuffChecked(checkBoxes, "Improved Scorch") || isBuffChecked(checkBoxes, "Winter's Chill") || isBuffChecked(checkBoxes, "Improved Shadow Bolt"))
                    sb.AppendLine("spell_crit_chance_debuff        5.0/5.0");
                else
                    sb.AppendLine("spell_crit_chance_debuff        0.0/5.0");
                if (isBuffChecked(checkBoxes, "Ebon Plaguebringer") || isBuffChecked(checkBoxes, "Earth and Moon") || isBuffChecked(checkBoxes, "Curse of the Elements"))
                    sb.AppendLine("spell_damage_debuff             13.0/13.0");
                else
                    sb.AppendLine("spell_damage_debuff             0.0/13.0");
                if (isBuffChecked(checkBoxes, "Flametongue Totem"))
                {
                    if (isBuffChecked(checkBoxes, "Enhancing Totems (Spell Power)"))
                        sb.AppendLine("spellpower_buff                 165/280");
                    else
                        sb.AppendLine("spellpower_buff                 144/280");
                }
                else if (isBuffChecked(checkBoxes, "Totem of Wrath (Spell Power)"))
                    sb.AppendLine("spellpower_buff                 280/280");
                else
                    sb.AppendLine("spellpower_buff                 0/280");
                if (isBuffChecked(checkBoxes, "Improved Faerie Fire") || isBuffChecked(checkBoxes, "Misery"))
                    sb.AppendLine("spell_hit_chance_debuff         3.0/3.0");
                else
                    sb.AppendLine("spell_hit_chance_debuff         0.0/3.0");
                if (isBuffChecked(checkBoxes, "Improved Moonkin Form") || isBuffChecked(checkBoxes, "Swift Retribution"))
                    sb.AppendLine("haste_buff                      3.0/3.0");
                else
                    sb.AppendLine("haste_buff                      0.0/3.0");
                if (isBuffChecked(checkBoxes, "Ferocious Inspiration") || isBuffChecked(checkBoxes, "Sanctified Retribution"))
                    sb.AppendLine("percentage_damage_increase      3.0/3.0");
                else
                    sb.AppendLine("percentage_damage_increase      0.0/3.0");
                if (isBuffChecked(checkBoxes, "Heart of the Crusader") || isBuffChecked(checkBoxes, "Totem of Wrath") || isBuffChecked(checkBoxes, "Master Poisoner"))
                    sb.AppendLine("crit_chance_debuff              3.0/3.0");
                else
                    sb.AppendLine("crit_chance_debuff              0.0/3.0");
                if (isBuffChecked(checkBoxes, "Blessing of Kings"))
                    sb.AppendLine("stat_multiplier                 10.0/10.0");
                else
                    sb.AppendLine("stat_multiplier                 0.0/10.0");
                if (isBuffChecked(checkBoxes, "Mark of the Wild"))
                    if (isBuffChecked(checkBoxes, "Improved Mark of the Wild"))
                        sb.AppendLine("stat_add_buff                   51/52");
                    else
                        sb.AppendLine("stat_add_buff                   37/52");
                else
                    sb.AppendLine("stat_add_buff                   0/52");
                if (isBuffChecked(checkBoxes, "Strength of Earth Totem") || isBuffChecked(checkBoxes, "Horn of Winter"))
                    if (isBuffChecked(checkBoxes, "Enhancing Totems (Agility/Strength)"))
                        sb.AppendLine("agi_and_strength_buff           178/178");
                    else
                        sb.AppendLine("agi_and_strength_buff           155/178");
                else
                    sb.AppendLine("agi_and_strength_buff           0/178");
                if (isBuffChecked(checkBoxes, "Fel Intelligence (Intellect)"))
                    if (isBuffChecked(checkBoxes, "Improved Felhunter"))
                        sb.AppendLine("intellect_buff                  52/60");
                    else
                        sb.AppendLine("intellect_buff                  48/60");
                else if (isBuffChecked(checkBoxes, "Arcane Intellect"))
                    sb.AppendLine("intellect_buff                  60/60");
                else
                    sb.AppendLine("intellect_buff                  0/60");
            }
            else
            {
                sb.AppendLine("armor_debuff_major              0.0/20.0");//acid spit, expose armor, sunder armor
                sb.AppendLine("armor_debuff_minor              0.0/5.0"); //faerie fire, sting, curse of recklessness
                sb.AppendLine("physical_vulnerability_debuff   0.0/4.0"); //%, bloody frenzy, savage combat
                sb.AppendLine("melee_haste_buff                0.0/20.0");//%, improved icy talons, windfury totem
                sb.AppendLine("melee_crit_chance_buff	  	   0.0/5.0"); //%, leader of the pack, rampage
                sb.AppendLine("attack_power_buff_flat		   0/688");   //battle shout, blessing of might
                sb.AppendLine("attack_power_buff_multiplier	   0.0/99.7");//BUFF UPTIME %, DISABLES UR, abominations might, trueshot aura, (unleashed rage)
                sb.AppendLine("spell_haste_buff                0.0/5.0"); //%, wrath of air totem
                sb.AppendLine("spell_crit_chance_buff          0.0/5.0"); //%, moonkin aura, elemental oath
                sb.AppendLine("spell_crit_chance_debuff        0.0/5.0"); //%, improved scorch, winter's chill
                sb.AppendLine("spell_damage_debuff             0.0/13.0");//%, ebon plaugebearer, earth and moon, curse of elements
                sb.AppendLine("spellpower_buff                 0/280");   //flametongue totem, totem of wrath
                sb.AppendLine("spell_hit_chance_debuff         0.0/3.0"); //%, improved faerie fire, misery
                sb.AppendLine("haste_buff                      0.0/3.0"); //%, improved moonkin aura, swift retribution
                sb.AppendLine("percentage_damage_increase      0.0/3.0"); //%, ferocious inspiration, sanctified retribution
                
                sb.AppendLine("crit_chance_debuff              0.0/3.0"); //%, heart of the crusader, totem of wrath, master poisoner
                sb.AppendLine("stat_multiplier                 0.0/10.0");//%, blessing of kings
                sb.AppendLine("stat_add_buff                   0/52");    //mark of the wild
                sb.AppendLine("agi_and_strength_buff           0/178");   //strength of earth, horn of winter
                sb.AppendLine("intellect_buff                  0/60");    //arcane intellect, fel intelligence
            }
        }

        private bool isBuffChecked(Dictionary<Buff, CheckBox> checkBoxes, string buffName)
        {
            foreach (CheckBox checkbox in checkBoxes.Values)
            {
                Buff buff = checkbox.Tag as Buff;
                if (buff.Name.Equals(buffName))
                    return checkbox.Checked;
            }
            return false;
        }

        private void addGlyphs(Character character, System.Text.StringBuilder sb)
        {
            int glyphNumber = 0;
            if (character.ShamanTalents.GlyphofFeralSpirit)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    feral_spirit");
            }
            if (character.ShamanTalents.GlyphofFlametongueWeapon)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    flametongue_weapon");
            }
            if (character.ShamanTalents.GlyphofLightningBolt)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    lightning_bolt");
            }
            if (character.ShamanTalents.GlyphofLavaLash)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    lava_lash");
            }
            if (character.ShamanTalents.GlyphofLightningShield)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    lightning_shield");
            }
            if (character.ShamanTalents.GlyphofShocking)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    earth_shock");
            }
            if (character.ShamanTalents.GlyphofStormstrike)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    stormstrike");
            }
            if (character.ShamanTalents.GlyphofWindfuryWeapon)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    windfury_weapon");
            }
            while (glyphNumber < 3)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    -");
            }
        }

        private void removeUseProcEffects(Character character, Stats stats)
        {
            // this routine needs to remove the effects of meta gems, enchants, trinkets and totems
            // Rawr adds in average proc effects whereas EnhSim uses the raw data and the name of
            // the meta gem, enchants, trinkets and totems
            _trinket1name = adjustTrinketStats(character, character.Trinket1, stats);
            _trinket2name = adjustTrinketStats(character, character.Trinket2, stats);
            _totemname = adjustTotemStats(character, character.Ranged, stats);
            _mhEnchant = adjustWeaponEnchantStats(character, character.MainHandEnchant, stats);
            _ohEnchant = adjustWeaponEnchantStats(character, character.OffHandEnchant, stats);
            _metagem = adjustMetaGemStats(character, character.Head, stats);
           
            // having removed all the stuff added by on use/procs we need to take the ceiling values as 100+2.66 would have been floored to 102
            // if we take 2.66 from 102 we get 101.33 which is too low.
            stats.AttackPower = (float) Math.Ceiling(stats.AttackPower);
            stats.SpellPower = (float) Math.Ceiling(stats.SpellPower);
            stats.HasteRating = (float)Math.Ceiling(stats.HasteRating);
            stats.CritRating = (float)Math.Ceiling(stats.CritRating);
            stats.ArmorPenetrationRating = (float)Math.Ceiling(stats.ArmorPenetrationRating);
        }

        private String adjustMetaGemStats(Character character, ItemInstance head, Stats stats)
        {
            if (head != null)
            {
                float spellpowerBonus = .1f * character.ShamanTalents.MentalQuickness;
                switch (head.Gem1Id)
                {
                    case 32409:
                        return "relentless_earthstorm_diamond";
                    case 32410:
                        return "thundering_skyfire_diamond";
                    case 34220:
                        return "chaotic_skyfire_diamond";
                    case 41285:
                        return "chaotic_skyflare_diamond";
                    case 41333:
                        return "ember_skyflare_diamond";
                    case 41398:
                        return "relentless_earthsiege_diamond";
                    default:
                        return "-";
                }
            }
            else
            {
                return "-";
            }
        }

        private String adjustWeaponEnchantStats(Character character, Enchant enchant, Stats stats)
        {
            if (enchant == null)
                return "-";
            // check weapon enchant return enchant name for EnhSim
            float spellpowerBonus = .1f * character.ShamanTalents.MentalQuickness;
            switch (enchant.Id)
            {
                case 2673:
                    return "mongoose";
                case 3225:
                    return "executioner";
                case 1900:
                    return "crusader";
                case 3273:
                    return "deathfrost";
                case 3789:
                    return "berserking";
                default:
                    return "-";
            }
        }

        private String adjustTotemStats(Character character, ItemInstance totem, Stats stats)
        {
            if (totem == null)
                return "-";
            float spellpowerBonus = .1f * character.ShamanTalents.MentalQuickness;
            switch (totem.Id)
            {
                case 33507:
                    return "stonebreakers_totem";
                case 27815:
                    return "totem_of_the_astral_winds";
                case 40710:
                    return "totem_of_splintering";
                case 32330:
                    return "totem_of_ancestral_guidance";
                case 28248:
                    return "totem_of_the_void";
                case 33506:
                    return "skycall_totem";
                case 40267:
                    // increase spellpower of CL & LB by 165
                    return "totem_of_hex";
                case 40322:
                    return "totem_of_dueling";
                case 40708:
                    return "totem_of_the_elemental_plane";
                case 42607:
                    // LL ability grants +120 AP for 6 sec
                    return "deadly_gladiators_totem_of_indomitability";
                case 42606:
                    // LL ability grants +106 AP for 6 sec
                    return "hateful_gladiators_totem_of_indomitability";
                case 42593:
                    // LL ability grants +94 AP for 6 sec
                    return "savage_gladiators_totem_of_indomitability";
                case 42602:
                    // Shocks grants +70 spellpower for 6 sec
                    return "deadly_gladiators_totem_of_survival";
                case 42601:
                    // Shocks grants +62 spellpower for 6 sec
                    return "hateful_gladiators_totem_of_survival";
                case 42594:
                    // Shocks grants +52 spellpower for 6 sec
                    return "savage_gladiators_totem_of_survival";
                default:
                    return "-";
            }
        }

        private String adjustTrinketStats(Character character, ItemInstance trinket, Stats stats)
        {
            if (trinket == null)
                return "-";
            float spellpowerBonus = .1f * character.ShamanTalents.MentalQuickness;
            switch (trinket.Id)
            {
	            case 28830:
                    stats.HasteRating -= stats.HasteRatingOnPhysicalAttack * 10 / 45;
		            return "dragonspine_trophy";
	            case 32419:
                    // no effect added in SpecialEffects class
		            return "ashtongue_talisman";
	            case 32505:
                    stats.ArmorPenetrationRating -= 42f / 5f;
		            return "madness_of_the_betrayer";
	            case 28034:
                    stats.AttackPower -= 300f / 6f;
                    stats.SpellPower -= spellpowerBonus * 300f / 6f;
		            return "hourglass_of_the_unraveller";
	            case 30627:
                    stats.AttackPower -= 340f / 6f;
                    stats.SpellPower -= spellpowerBonus * 340f / 6f;
		            return "tsunami_talisman";
	            case 34472:
                    stats.AttackPower -= 90f;
                    stats.SpellPower -= spellpowerBonus * 90f;
		            return "shard_of_contempt";
	            case 33831:
                    stats.AttackPower -= 360f / 6f;
                    stats.SpellPower -= spellpowerBonus * 360f / 6f;
		            return "berserkers_call";
	            case 29383:
                    stats.AttackPower -= 278f / 6f;
                    stats.SpellPower -= spellpowerBonus * 278f / 6f;
		            return "bloodlust_brooch";
	            case 28288:
                    stats.HasteRating -= 260f / 12f;
		            return "abacus_of_violent_odds";
	            case 32658:
                    stats.CritRating -= ((150f / 6f) / 25f) * 45.90598679f;
                    stats.AttackPower -= (150f / 6f) * 1.03f;
                    stats.SpellPower -= spellpowerBonus * (150f / 6f) * 1.03f;
		            return "badge_of_tenacity";
	            case 35702:
                    stats.AttackPower -= 320f / 6f;
                    stats.SpellPower -= spellpowerBonus * 320f / 6f;
		            return "shadowsong_panther";
	            case 34427:
                    // no effect added in SpecialEffects class
		            return "blackened_naaru_silver";
	            case 31856:
                    stats.AttackPower -= 120; 
                    stats.SpellPower -= spellpowerBonus * 120;
                    stats.SpellPower -= 80;
		            return "darkmoon_card_crusade";
	            case 40431:
                    stats.AttackPower -= 320;
                    stats.SpellPower -= spellpowerBonus * 320;
                    return "fury_of_the_five_flights";
	            case 40256:
                    //stats.ArmorPenetrationRating -= 612f / 5f;
		            return "grim_toll";
	            case 39257:
                    stats.AttackPower -= 670f / 6f;
                    stats.SpellPower -= spellpowerBonus * 670f / 6f;
		            return "loathebs_shadow";
	            case 40684:
                    stats.AttackPower -= 1000f / 7f;
                    stats.SpellPower -= spellpowerBonus * 1000f / 7f;
		            return "mirror_of_truth";
	            case 32483:
                    // stats.HasteRatingFor20SecOnUse2Min += 175;
		            return "the_skull_of_guldan";
	            case 37390:
                    stats.HasteRating -= stats.HasteRatingOnPhysicalAttack * 10 / 45;
                    return "meteorite_whetstone";
	            case 39229:
                    // stats.SpellHasteFor10SecOnCast_10_45 -= 505;
		            return "embrace_of_the_spider";
	            case 40255:
                    // stats.SpellPowerFor10SecOnCast_15_45 -= 765;
		            return "dying_curse";
	            case 40432:
                    stats.SpellPower -= 200;
		            return "illustration_of_the_dragon_soul";
	            case 40682:
                    // stats.SpellPowerFor10SecOnHit_10_45 -= 590;
		            return "sundial_of_the_exiled";
	            case 37660:
                    // stats.SpellPowerFor10SecOnHit_10_45 -= 512;
		            return "forge_ember";
	            case 37723:
                    stats.ArmorPenetrationRating -= 291;
		            return "incisor_fragment";
	            case 37873:
                    stats.SpellPower -= 346f / 6f;
		            return "mark_of_the_war_prisoner";
	            case 37166:
                    stats.AttackPower -= 670f / 6f;
                    stats.SpellPower -= spellpowerBonus * 670f / 6f;
		            return "sphere_of_red_dragons_blood";
	            case 36972:
                    stats.HasteRating -= 256f / 6f;
		            return "tome_of_arcane_phenomena";
	            case 40531:
                    stats.HasteRating -= 491f / 6f;
		            return "mark_of_norgannon";
	            case 44014:
                    stats.AttackPower -= 54f;
                    stats.SpellPower -= spellpowerBonus * 54f;
		            return "fezziks_pocketwatch";
	            case 43836:
                    stats.HasteRating -= 212f / 6f;
		            return "thorny_rose_brooch";
	            case 38764:
                    stats.HasteRating -= 208f / 6f;
		            return "rune_of_finite_variation";
	            case 40371:
                    // no effect added in SpecialEffects class
		            return "bandits_insignia";
	            case 44253:
                    // stats.GreatnessProc -= 1;
		            return "darkmoon_card_greatness";
	            case 37264:
                    // stats.PendulumOfTelluricCurrentsProc -= 1;
		            return "pendulum_of_telluric_currents";
	            case 37064:
                    // no effect added in SpecialEffects class
		            return "vestige_of_haldor";
	            case 42395:
                    stats.SpellPower -= 292f / 15f;
                    return "twilight_serpent";
                default:
                    return "-";
            }
        }

        private String getSetBonuses(Character character)
        {
            Dictionary<string, int> setCounts = new Dictionary<string, int>();
            ItemInstance[] items = { character.Head, character.Chest, character.Hands, character.Shoulders, character.Legs,
                                     character.Wrist, character.Waist, character.Feet }; // last 3 are for tier 6 eight piece set
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < items.Length; i++ )
            {
                if ((object)items[i] != null && !string.IsNullOrEmpty(items[i].Item.SetName))
                {
                    int count;
                    setCounts.TryGetValue(items[i].Item.SetName, out count);
                    setCounts[items[i].Item.SetName] = count + 1;
                }
            }
            int bonusCount = 0;
            if (setCounts.Count > 0)
            {
                
                foreach (KeyValuePair<string, int> kvp in setCounts)
                {
                    switch (kvp.Key)
                    {
                        case "Earthshatter Battlegear":
                            if (kvp.Value >= 4)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      naxx_melee_4");
                            }
                            else if (kvp.Value >= 2)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      naxx_melee_2");
                            }
                            break;
                        case "Worldbreaker Battlegear":
                            if (kvp.Value >= 4)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      ulduar_melee_4");
                            }
                            else if (kvp.Value >= 2)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      ulduar_melee_2");
                             }
                           break;
                    }
                }
            }
            while (bonusCount < 3)
            {
                bonusCount++;
                sb.AppendLine("set_bonus" + bonusCount+ "                      -");
            }
            return sb.ToString();       
        }
    }
}
