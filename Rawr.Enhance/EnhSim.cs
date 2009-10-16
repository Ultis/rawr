using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Windows;

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

        public EnhSim(Character character, CalculationOptionsEnhance calcOpts)
        {
            CalculationsEnhance ce = new CalculationsEnhance();
            CharacterCalculationsEnhance calcs = ce.GetCharacterCalculations(character, null) as CharacterCalculationsEnhance;
            Stats stats = calcs.EnhSimStats;
            if(isMasterOfAnatomy(character))
                stats.CritRating += 32;
            CombatStats cs = new CombatStats(character, stats, calcOpts);
            
            getSpecialsNames(character, stats);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("##########################################");
            sb.AppendLine("### Rawr.Enhance Data Export to EnhSim ###");
            sb.AppendLine("##########################################");
            sb.AppendLine();
            sb.AppendLine("config_source rawr");
            sb.AppendLine();

            float MHSpeed = character.MainHand == null ? 3.0f : character.MainHand.Item.Speed;
            float wdpsMH = character.MainHand == null ? 46.3f : character.MainHand.Item.DPS;
            float OHSpeed = character.OffHand == null ? 3.0f : character.OffHand.Item.Speed;
            float wdpsOH = character.OffHand == null ? 46.3f : character.OffHand.Item.DPS;

            sb.AppendLine("race                            " + character.Race.ToString().ToLower());
            sb.AppendLine("mh_speed                        " + MHSpeed.ToString("F1", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_speed                        " + OHSpeed.ToString("F1", CultureInfo.InvariantCulture));
            sb.AppendLine("mh_dps                          " + wdpsMH.ToString("F1", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_dps                          " + wdpsOH.ToString("F1", CultureInfo.InvariantCulture));
            float chanceCrit = cs.DisplayMeleeCrit * 100f;
            sb.AppendLine("mh_crit                         " + chanceCrit.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_crit                         " + chanceCrit.ToString("F2", CultureInfo.InvariantCulture));
            float hitBonus = StatConversion.GetHitFromRating(stats.HitRating) * 100f;
            sb.AppendLine("mh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("mh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("ap                              " + stats.AttackPower.ToString("F0", CultureInfo.InvariantCulture));
            float hasteBonus = StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Shaman) * 100f;
            sb.AppendLine("melee_haste                     " + hasteBonus.ToString("F2", CultureInfo.InvariantCulture));
            float armourPenBonus = StatConversion.GetArmorPenetrationFromRating(stats.ArmorPenetrationRating) * 100f;
            sb.AppendLine("armor_penetration               " + armourPenBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("str                             " + stats.Strength.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("agi                             " + stats.Agility.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("int                             " + stats.Intellect.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spi                             " + stats.Spirit.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spellpower                      " + stats.SpellPower.ToString("F0", CultureInfo.InvariantCulture));
            float chanceSpellCrit = cs.DisplaySpellCrit * 100f;
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
                if (character.MainHand.Type == ItemType.OneHandAxe || character.MainHand.Type == ItemType.TwoHandAxe)
                    weaponType = "axe";
                if (character.MainHand.Type == ItemType.FistWeapon)
                    weaponType = "fist";
            }
            sb.AppendLine("mh_weapon                       " + weaponType);
            sb.AppendLine("oh_enchant                      " + _ohEnchant);
            weaponType = "-";
            if (character.OffHand != null)
            {
                if (character.OffHand.Type == ItemType.OneHandAxe)
                    weaponType = "axe";
                if (character.OffHand.Type == ItemType.FistWeapon)
                    weaponType = "fist";
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
            string handEnchant = character.HandsEnchant == null ? string.Empty : character.HandsEnchant.Name;
            if (handEnchant == "Hyperspeed Accelerators")
                sb.AppendLine("gloves_enchant                  hyperspeed_accelerators");
            else if (handEnchant == "Hand-Mounted Pyro Rocket")
                sb.AppendLine("gloves_enchant                  hand_mounted_pyro_rocket");
            else
                sb.AppendLine("gloves_enchant                  -");
            string backEnchant = character.BackEnchant == null ? string.Empty : character.BackEnchant.Name;
            if (backEnchant == "Lightweave Embroidery")
                sb.AppendLine("cloak_enchant                   lightweave_embroidery");
            else if (handEnchant == "Swordguard Embroidery")
                sb.AppendLine("cloak_enchant                   swordguard_embroidery");
            else
                sb.AppendLine("cloak_enchant                   -");
            addGlyphs(character, sb);
            sb.AppendLine();
            sb.AppendLine("glyph_minor1                    -");
            sb.AppendLine("glyph_minor2                    -");
            sb.AppendLine("glyph_minor3                    -");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("#############");
            sb.AppendLine("## Talents ##");
            sb.AppendLine("#############");
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
            // add extras
            sb.AppendLine();
            sb.AppendLine("combat_length                   " + calcOpts.FightLength.ToString("F2", CultureInfo.InvariantCulture));
            _configText = sb.ToString();
        }

#if RAWR3
        public void copyToClipboard()
        {
			try
			{
                Clipboard.SetText(_configText);
			}
			catch { }
            MessageBox.Show("EnhSim config data copied to clipboard\n" + 
                "Use the 'Copy from Clipboard' option in EnhSimGUI v1.8.3 or higher, to import it\n" +
                "Or paste the config data into your EnhSim config file in a decent text editor (not Notepad)!",
                "Enhance Module", MessageBoxButton.OK);
        }
#else
        public void copyToClipboard()
        {
			try
			{
                System.Windows.Forms.Clipboard.SetText(_configText);
			}
			catch { }
            System.Windows.Forms.MessageBox.Show("EnhSim config data copied to clipboard\n" + 
                "Use the 'Copy from Clipboard' option in EnhSimGUI v1.8.3 or higher, to import it\n" +
                "Or paste the config data into your EnhSim config file in a decent text editor (not Notepad)!",
                "Enhance Module", System.Windows.Forms.MessageBoxButtons.OK);
        }
#endif
        private void addBuffs(Character character, StringBuilder sb)
        {
            sb.AppendLine("#########");
            sb.AppendLine("# Buffs #");
            sb.AppendLine("#########");
            sb.AppendLine();
            List<Buff> buffs = character.ActiveBuffs;

            if (isBuffChecked(buffs, "Acid Spit") || isBuffChecked(buffs, "Expose Armor") || isBuffChecked(buffs, "Sunder Armor"))
                sb.AppendLine("armor_debuff_major              20.0/20.0");
            else
                sb.AppendLine("armor_debuff_major              0.0/20.0");
            if (isBuffChecked(buffs, "Curse of Recklessness") || isBuffChecked(buffs, "Faerie Fire") || isBuffChecked(buffs, "Sting"))
                sb.AppendLine("armor_debuff_minor              5.0/5.0");
            else
                sb.AppendLine("armor_debuff_minor              0.0/5.0");
            if (isBuffChecked(buffs, "Blood Frenzy") || isBuffChecked(buffs, "Savage Combat"))
                sb.AppendLine("physical_vulnerability_debuff   4.0/4.0");
            else
                sb.AppendLine("physical_vulnerability_debuff   0.0/4.0");
            if (isBuffChecked(buffs, "Windfury Totem"))
                if (isBuffChecked(buffs, "Improved Windfury Totem"))
                    sb.AppendLine("melee_haste_buff                20.0/20.0");
                else
                    sb.AppendLine("melee_haste_buff                16.0/20.0");
            else if (isBuffChecked(buffs, "Improved Icy Talons"))
                sb.AppendLine("melee_haste_buff                20.0/20.0");
            else
                sb.AppendLine("melee_haste_buff                0.0/20.0");
            if (isBuffChecked(buffs, "Leader of the Pack") || isBuffChecked(buffs, "Rampage"))
                sb.AppendLine("melee_crit_chance_buff          5.0/5.0");
            else
                sb.AppendLine("melee_crit_chance_buff          0.0/5.0");
            if (isBuffChecked(buffs, "Battle Shout"))
            {
                if (isBuffChecked(buffs, "Commanding Presence (Attack Power)"))
                    sb.AppendLine("attack_power_buff_flat          685/688");
                else
                    sb.AppendLine("attack_power_buff_flat          548/688");
            }
            else if (isBuffChecked(buffs, "Blessing of Might"))
            {
                if (isBuffChecked(buffs, "Improved Blessing of Might"))
                    sb.AppendLine("attack_power_buff_flat          687/688");
                else
                    sb.AppendLine("attack_power_buff_flat          550/688");
            }
            else
                sb.AppendLine("attack_power_buff_flat          0/688");
            if (isBuffChecked(buffs, "Trueshot Aura") || isBuffChecked(buffs, "Unleashed Rage") || isBuffChecked(buffs, "Abomination's Might"))
                sb.AppendLine("attack_power_buff_multiplier    99.7/99.7");
            else
                sb.AppendLine("attack_power_buff_multiplier    0.0/99.7");
            if (isBuffChecked(buffs, "Wrath of Air Totem"))
                sb.AppendLine("spell_haste_buff                5.0/5.0");
            else
                sb.AppendLine("spell_haste_buff                0.0/5.0");
            if (isBuffChecked(buffs, "Elemental Oath") || isBuffChecked(buffs, "Moonkin Form"))
                sb.AppendLine("spell_crit_chance_buff          5.0/5.0");
            else
                sb.AppendLine("spell_crit_chance_buff          0.0/5.0");
            if (isBuffChecked(buffs, "Improved Scorch") || isBuffChecked(buffs, "Winter's Chill") || isBuffChecked(buffs, "Improved Shadow Bolt"))
                sb.AppendLine("spell_crit_chance_debuff        5.0/5.0");
            else
                sb.AppendLine("spell_crit_chance_debuff        0.0/5.0");
            if (isBuffChecked(buffs, "Ebon Plaguebringer") || isBuffChecked(buffs, "Earth and Moon") || isBuffChecked(buffs, "Curse of the Elements"))
                sb.AppendLine("spell_damage_debuff             13.0/13.0");
            else
                sb.AppendLine("spell_damage_debuff             0.0/13.0");
            if (isBuffChecked(buffs, "Flametongue Totem"))
            {
                if (isBuffChecked(buffs, "Enhancing Totems (Spell Power)"))
                    sb.AppendLine("spellpower_buff                 165/280");
                else
                    sb.AppendLine("spellpower_buff                 144/280");
            }
            else if (isBuffChecked(buffs, "Totem of Wrath (Spell Power)"))
                sb.AppendLine("spellpower_buff                 280/280");
            else
                sb.AppendLine("spellpower_buff                 0/280");
            if (isBuffChecked(buffs, "Improved Faerie Fire") || isBuffChecked(buffs, "Misery"))
                sb.AppendLine("spell_hit_chance_debuff         3.0/3.0");
            else
                sb.AppendLine("spell_hit_chance_debuff         0.0/3.0");
            if (isBuffChecked(buffs, "Improved Moonkin Form") || isBuffChecked(buffs, "Swift Retribution"))
                sb.AppendLine("haste_buff                      3.0/3.0");
            else
                sb.AppendLine("haste_buff                      0.0/3.0");
            if (isBuffChecked(buffs, "Ferocious Inspiration") || isBuffChecked(buffs, "Sanctified Retribution"))
                sb.AppendLine("percentage_damage_increase      3.0/3.0");
            else
                sb.AppendLine("percentage_damage_increase      0.0/3.0");
            if (isBuffChecked(buffs, "Heart of the Crusader") || isBuffChecked(buffs, "Totem of Wrath") || isBuffChecked(buffs, "Master Poisoner"))
                sb.AppendLine("crit_chance_debuff              3.0/3.0");
            else
                sb.AppendLine("crit_chance_debuff              0.0/3.0");
            if (isBuffChecked(buffs, "Blessing of Kings"))
                sb.AppendLine("stat_multiplier                 10.0/10.0");
            else
                sb.AppendLine("stat_multiplier                 0.0/10.0");
            if (isBuffChecked(buffs, "Mark of the Wild"))
                if (isBuffChecked(buffs, "Improved Mark of the Wild"))
                    sb.AppendLine("stat_add_buff                   51/52");
                else
                    sb.AppendLine("stat_add_buff                   37/52");
            else
                sb.AppendLine("stat_add_buff                   0/52");
            if (isBuffChecked(buffs, "Strength of Earth Totem") || isBuffChecked(buffs, "Horn of Winter"))
                if (isBuffChecked(buffs, "Enhancing Totems (Agility/Strength)"))
                    sb.AppendLine("agi_and_strength_buff           178/178");
                else
                    sb.AppendLine("agi_and_strength_buff           155/178");
            else
                sb.AppendLine("agi_and_strength_buff           0/178");
            if (isBuffChecked(buffs, "Fel Intelligence (Intellect)"))
                if (isBuffChecked(buffs, "Improved Felhunter"))
                    sb.AppendLine("intellect_buff                  52/60");
                else
                    sb.AppendLine("intellect_buff                  48/60");
            else if (isBuffChecked(buffs, "Arcane Intellect"))
                sb.AppendLine("intellect_buff                  60/60");
            else
                sb.AppendLine("intellect_buff                  0/60");
            if (isBuffChecked(buffs, "Heroism/Bloodlust"))
                sb.AppendLine("bloodlust_casters               1");
            else
                sb.AppendLine("bloodlust_casters               0");
            sb.AppendLine("flask_elixir                    " + addFlask(buffs));
            sb.AppendLine("guardian_elixir                 " + addGuardianElixir(buffs));
            sb.AppendLine("potion                          " + addPotion(buffs));
            sb.AppendLine("food                            " + addFood(buffs));
        }

        private bool isMasterOfAnatomy(Character character)
        {
            return isBuffChecked(character.ActiveBuffs, "Master of Anatomy");
        }

        private bool isBuffChecked(List<Buff> buffs, string buffName)
        {
            foreach (Buff buff in buffs)
            {
                if (buff.Name.Equals(buffName))
                    return true;
            }
            return false;
        }

        private string addFlask(List<Buff> buffs)
        {
            if (isBuffChecked(buffs, "Flask of Endless Rage"))
                return "flask_of_endless_rage";
            if (isBuffChecked(buffs, "Flask of the Frost Wyrm"))
                return "flask_of_the_frost_wyrm";
            if (isBuffChecked(buffs, "Elixir of Demonslaying"))
                return "elixir_of_demonslaying";
            if (isBuffChecked(buffs, "Elixir of Major Agility"))
                return "elixir_of_major_agility";
            if (isBuffChecked(buffs, "Elixir of Mighty Agility"))
                return "elixir_of_mighty_agility";
            if (isBuffChecked(buffs, "Elixir of Mighty Strength"))
                return "elixir_of_mighty_strength";
            if (isBuffChecked(buffs, "Elixir of Accuracy"))
                return "elixir_of_accuracy";
            if (isBuffChecked(buffs, "Elixir of Armor Piercing"))
                return "elixir_of_armor_piercing";
            if (isBuffChecked(buffs, "Elixir of Deadly Strikes"))
                return "elixir_of_deadly_strikes";
            if (isBuffChecked(buffs, "Elixir of Expertise"))
                return "elixir_of_expertise";
            if (isBuffChecked(buffs, "Elixir of Lightning Speed"))
                return "elixir_of_lightning_speed";
            if (isBuffChecked(buffs, "Guru's Elixir"))
                return "gurus_elixir";
            if (isBuffChecked(buffs, "Spellpower Elixir"))
                return "spellpower_elixir";
            if (isBuffChecked(buffs, "Wrath Elixir"))
                return "wrath_elixir";
            return "-";
        }

        private string addGuardianElixir(List<Buff> buffs)
        {
            if (isBuffChecked(buffs, "Elixir of Draenic Wisdom"))
                return "elixir_of_draenic_wisdom";
            if (isBuffChecked(buffs, "Elixir of Mighty Thoughts"))
                return "elixir_of_mighty_thoughts";
            return "-";
        }

        private string addPotion(List<Buff> buffs)
        {
            if (isBuffChecked(buffs, "Potion of Speed"))
                return "potion_of_speed";
            if (isBuffChecked(buffs, "Potion of Wild Magic"))
                return "potion_of_wild_magic";
            if (isBuffChecked(buffs, "Heroic Potion"))
                return "heroic_potion";
            if (isBuffChecked(buffs, "Insane Strength Potion"))
                return "insane_strength_potion";
            return "-";
        }

        private string addFood(List<Buff> buffs)
        {
            if (isBuffChecked(buffs, "Agility Food"))
                return "blackened_dragonfin";
            if (isBuffChecked(buffs, "Armor Pen Food"))
                return "hearty_rhino";
            if (isBuffChecked(buffs, "Expertise Food"))
                return "rhinolicious_wormsteak";
            if (isBuffChecked(buffs, "Hit Food"))
                return "snapper_extreme";
            if (isBuffChecked(buffs, "Spell Power Food"))
                return "firecracker_salmon";
            if (isBuffChecked(buffs, "Haste Food"))
                return "imperial_manta_steak";
            if (isBuffChecked(buffs, "Attack Power Food"))
                return "poached_northern_sculpin";
            if (isBuffChecked(buffs, "Crit Food"))
                return "spiced_wyrm_burger";
            if (isBuffChecked(buffs, "Fish Feast"))
                return "fish_feast";
            return "-";
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
            if (character.ShamanTalents.GlyphofFlameShock)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    flame_shock");
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

        private void getSpecialsNames(Character character, Stats stats)
        {
            // this routine now just gets the names of the meta gem, enchants, trinkets and totems
            _trinket1name = character.Trinket1 == null ? "-" : character.Trinket1.Id.ToString();
            _trinket2name = character.Trinket2 == null ? "-" : character.Trinket2.Id.ToString();
            _totemname = character.Ranged == null ? "-" : character.Ranged.Id.ToString();
            _mhEnchant = character.MainHandEnchant == null ? "-" : character.MainHandEnchant.Id.ToString();
            _ohEnchant = character.OffHandEnchant == null ? "-" : character.OffHandEnchant.Id.ToString();
            _metagem = character.Head == null ? "-" : (character.Head.Gem1Id == 0 ? "-" : character.Head.Gem1Id.ToString());
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
                                sb.AppendLine("set_bonus" + bonusCount + "                      worldbreaker_battlegear_4");
                            }
                            else if (kvp.Value >= 2)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      worldbreaker_battlegear_2");
                            }
                            break;
                        case "Nobundo's Battlegear":
                            if (kvp.Value >= 4)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      t9_battlegear_4");
                            }
                            else if (kvp.Value >= 2)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      t9_battlegear_2");
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
