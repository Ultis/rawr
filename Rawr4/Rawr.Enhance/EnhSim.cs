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
        private Character _character;
        CalculationOptionsEnhance _calcOpts;

        public EnhSim(Character character, CalculationOptionsEnhance calcOpts)
        {
            _character = character;
            _calcOpts = calcOpts;
            CalculationsEnhance ce = new CalculationsEnhance();
            CharacterCalculationsEnhance calcs = ce.GetCharacterCalculations(character, null) as CharacterCalculationsEnhance;
            Stats stats = calcs.EnhSimStats;
            /*if (_character.ActiveBuffsContains("Master of Anatomy"))
                stats.CritRating += 40;*/
            CombatStats cs = new CombatStats(character, stats, _calcOpts);

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
            float chanceCrit = cs.ExportMeleeCritMH * 100f;
            sb.AppendLine("mh_crit                         " + chanceCrit.ToString("F2", CultureInfo.InvariantCulture));
            chanceCrit = cs.ExportMeleeCritOH * 100f;
            sb.AppendLine("oh_crit                         " + chanceCrit.ToString("F2", CultureInfo.InvariantCulture));
            float hitBonus = StatConversion.GetHitFromRating(stats.HitRating) * 100f;
            sb.AppendLine("mh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("mh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("ap                              " + stats.AttackPower.ToString("F0", CultureInfo.InvariantCulture));
            float hasteBonus = StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Shaman) * 100f;
            sb.AppendLine("melee_haste                     " + hasteBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("str                             " + stats.Strength.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("agi                             " + stats.Agility.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("int                             " + stats.Intellect.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spi                             " + stats.Spirit.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("mastery_rating                " + stats.MasteryRating.ToString("F0", CultureInfo.InvariantCulture));
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
            sb.AppendLine("mh_imbue                        " + _calcOpts.MainhandImbue.ToString().ToLower());
            sb.AppendLine("oh_imbue                        " + _calcOpts.OffhandImbue.ToString().ToLower());
            sb.AppendLine();
            sb.AppendLine("mh_enchant                      " + _mhEnchant);
            sb.AppendLine("oh_enchant                      " + _ohEnchant);
            String weaponType = "-";
            if (character.MainHand != null)
            {
                if (character.MainHand.Type == ItemType.OneHandAxe || character.MainHand.Type == ItemType.TwoHandAxe)
                    weaponType = "axe";
                if (character.MainHand.Type == ItemType.FistWeapon)
                    weaponType = "fist";
            }
            sb.AppendLine("mh_weapon                       " + weaponType);
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
            else if (backEnchant == "Swordguard Embroidery")
                sb.AppendLine("cloak_enchant                   swordguard_embroidery");
            else
                sb.AppendLine("cloak_enchant                   -");
            if (character.Finger1.Id == 50401 || character.Finger2.Id == 50401 ||
                character.Finger1.Id == 50402 || character.Finger2.Id == 50402)
                sb.AppendLine("ring_proc                       ashen_verdict");
            else
                sb.AppendLine("ring_proc                       -");
            addGlyphs(character, sb);
            sb.AppendLine();
            sb.AppendLine("#############");
            sb.AppendLine("## Talents ##");
            sb.AppendLine("#############");
            sb.AppendLine("primary_talent                  enhancement");
            sb.AppendLine();
            sb.AppendLine("elemental_weapons               " + character.ShamanTalents.ElementalWeapons + "/2");
            sb.AppendLine("focused_strikes                 " + character.ShamanTalents.FocusedStrikes + "/3");
            sb.AppendLine("improved_shields                " + character.ShamanTalents.ImprovedShields + "/3");
            sb.AppendLine("elemental_devastation           " + character.ShamanTalents.ElementalDevastation + "/3");
            sb.AppendLine("flurry                          " + character.ShamanTalents.Flurry + "/3");
            sb.AppendLine("static_shock                    " + character.ShamanTalents.StaticShock + "/3");
            sb.AppendLine("improved_fire_nova              " + character.ShamanTalents.ImprovedFireNova + "/2");
            sb.AppendLine("searing_flames                  " + character.ShamanTalents.SearingFlames + "/3");
            sb.AppendLine("frozen_power                    " + character.ShamanTalents.FrozenPower + "/2");
            sb.AppendLine("shamanistic_rage                " + character.ShamanTalents.ShamanisticRage + "/1");
            sb.AppendLine("unleashed_rage                  " + character.ShamanTalents.UnleashedRage + "/2");
            sb.AppendLine("maelstrom_weapon                " + character.ShamanTalents.MaelstromWeapon + "/3");
            sb.AppendLine("improved_lava_lash              " + character.ShamanTalents.ImprovedLavaLash + "/2");
            sb.AppendLine();
            sb.AppendLine("convection                      " + character.ShamanTalents.Convection + "/2");
            sb.AppendLine("concussion                      " + character.ShamanTalents.Concussion + "/3");
            sb.AppendLine("call_of_flame                   " + character.ShamanTalents.CallOfFlame + "/2");
            sb.AppendLine("reverberation                   " + character.ShamanTalents.Reverberation + "/2");
            sb.AppendLine("elemental_precision             " + character.ShamanTalents.ElementalPrecision + "/3");
            sb.AppendLine("elemental_focus                 " + character.ShamanTalents.ElementalFocus + "/1");
            sb.AppendLine("elemental_oath                  " + character.ShamanTalents.ElementalOath + "/2");
            sb.AppendLine("lava_flows                      " + character.ShamanTalents.LavaFlows + "/3");
            sb.AppendLine("storm_earth_and_fire            " + /*character.ShamanTalents.*/"0" + "/3");
            sb.AppendLine("elemental_mastery               " + character.ShamanTalents.ElementalMastery + "/1");
            sb.AppendLine("feedback                        " + character.ShamanTalents.Feedback + "/3");
            sb.AppendLine("lava_surge                      " + character.ShamanTalents.LavaSurge + "/2");
            sb.AppendLine();

            addBuffs(character, sb);
            // add extras
            sb.AppendLine();
            sb.AppendLine("combat_length                   " + _calcOpts.FightLength.ToString("F2", CultureInfo.InvariantCulture));
            _configText = sb.ToString();
        }

#if SILVERLIGHT
        public void copyToClipboard()
        {
            try
            {
                Clipboard.SetText(_configText);
            }
            catch { }
            if (/*Clipboard.Success && */_calcOpts.ShowExportMessageBox)
                MessageBox.Show("EnhSim config data copied to clipboard.\n" +
                    "Use the 'Copy from Clipboard' option in EnhSimGUI v1.9.6.0 or higher, to import it\n" +
                    "Or paste the config data into your EnhSim config file in a decent text editor (not Notepad)!",
                    "Enhance Module", MessageBoxButton.OK);
        }
#else
        public void copyToClipboard()
        {
			try
			{
                System.Windows.Clipboard.SetText(_configText);
			}
			catch { }
            if(_calcOpts.ShowExportMessageBox)  
                System.Windows.MessageBox.Show("EnhSim config data copied to clipboard.\n" +
                    "Use the 'Copy from Clipboard' option in EnhSimGUI v1.9.6.0 or higher, to import it\n" +
                    "Or paste the config data into your EnhSim config file in a decent text editor (not Notepad)!",
                    "Enhance Module");
        }
#endif
        private void addBuffs(Character character, StringBuilder sb)
        {
            sb.AppendLine("#########");
            sb.AppendLine("# Buffs #");
            sb.AppendLine("#########");
            sb.AppendLine();
            List<Buff> buffs = character.ActiveBuffs;

            if (_character.ActiveBuffsContains("Acid Spit") || _character.ActiveBuffsContains("Expose Armor") || _character.ActiveBuffsContains("Sunder Armor") || _character.ActiveBuffsContains("Faerie Fire") || _character.ActiveBuffsContains("Sting") || _character.ActiveBuffsContains("Curse of Recklessness"))
                sb.AppendLine("armor_debuff                    12.0/12.0");
            else
                sb.AppendLine("armor_debuff                    0.0/12.0");
            if (_character.ActiveBuffsContains("Blood Frenzy") || _character.ActiveBuffsContains("Savage Combat"))
                sb.AppendLine("physical_vulnerability_debuff   4.0/4.0");
            else
                sb.AppendLine("physical_vulnerability_debuff   0.0/4.0");
            if (_character.ActiveBuffsContains("Windfury Totem") || _character.ActiveBuffsContains("Improved Icy Talons"))
                sb.AppendLine("melee_haste_buff                10.0/10.0");
            else
                sb.AppendLine("melee_haste_buff                0.0/10.0");
            if (_character.ActiveBuffsContains("Leader of the Pack") || _character.ActiveBuffsContains("Rampage") || _character.ActiveBuffsContains("Elemental Oath"))
                sb.AppendLine("crit_chance_buff                5.0/5.0");
            else
                sb.AppendLine("crit_chance_buff                0.0/5.0");
            if (_character.ActiveBuffsContains("Trueshot Aura") || _character.ActiveBuffsContains("Unleashed Rage") || _character.ActiveBuffsContains("Abomination's Might"))
                sb.AppendLine("attack_power_buff_multiplier    10.0/10.0");
            else
                sb.AppendLine("attack_power_buff_multiplier    0.0/10.0");
            if (_character.ActiveBuffsContains("Wrath of Air Totem"))
                sb.AppendLine("spell_haste_buff                5.0/5.0");
            else
                sb.AppendLine("spell_haste_buff                0.0/5.0");
            if (_character.ActiveBuffsContains("Improved Scorch") || _character.ActiveBuffsContains("Winter's Chill") || _character.ActiveBuffsContains("Improved Shadow Bolt"))
                sb.AppendLine("spell_crit_chance_debuff        5.0/5.0");
            else
                sb.AppendLine("spell_crit_chance_debuff        0.0/5.0");
            if (_character.ActiveBuffsContains("Ebon Plaguebringer") || _character.ActiveBuffsContains("Earth and Moon") || _character.ActiveBuffsContains("Curse of the Elements"))
                sb.AppendLine("spell_damage_debuff             8.0/8.0");
            else
                sb.AppendLine("spell_damage_debuff             0.0/8.0");
            if (_character.ActiveBuffsContains("Flametongue Totem"))
                sb.AppendLine("spellpower_buff                 6.0/10.0");
            else if (_character.ActiveBuffsContains("Totem of Wrath (Spell Power)"))
                sb.AppendLine("spellpower_buff                 10.0/10.0");
            else
                sb.AppendLine("spellpower_buff                 0/10.0");
            if (_character.ActiveBuffsContains("Ferocious Inspiration") || _character.ActiveBuffsContains("Sanctified Retribution"))
                sb.AppendLine("percentage_damage_increase      3.0/3.0");
            else
                sb.AppendLine("percentage_damage_increase      0.0/3.0");
            if (_character.ActiveBuffsContains("Blessing of Kings") || _character.ActiveBuffsContains("Mark of the Wild"))
                sb.AppendLine("stat_multiplier                 5.0/5.0");
            else
                sb.AppendLine("stat_multiplier                 0.0/5.0");
            if (_character.ActiveBuffsContains("Strength of Earth Totem") || _character.ActiveBuffsContains("Horn of Winter"))
                sb.AppendLine("agi_and_strength_buff           155/155");
            else
                sb.AppendLine("agi_and_strength_buff           0/155");
            if (_character.ActiveBuffsContains("Fel Intelligence") || _character.ActiveBuffsContains("Arcane Intellect"))
                sb.AppendLine("mana_buff                       600/600");
            else
                sb.AppendLine("mana_buff                       0/600");
            if (_character.ActiveBuffsContains("Blessing of Might") || _character.ActiveBuffsContains("Mana Spring Totem"))
                sb.AppendLine("mana_regen_buff                 92/92");
            else
                sb.AppendLine("mana_regen_buff                 0/92");
            sb.AppendLine();
            if (_character.ActiveBuffsContains("Hunting Party") || _character.ActiveBuffsContains("Judgements of the Wise") || _character.ActiveBuffsContains("Vampiric Touch") ||
                    _character.ActiveBuffsContains("Improved Soul Leech") || _character.ActiveBuffsContains("Enduring Winter"))
                sb.AppendLine("replenishment             1");
            else
                sb.AppendLine("replenishment             0");
            if (_calcOpts.PriorityInUse(EnhanceAbility.LightningShield))
                sb.AppendLine("water_shield              0");
            else
                sb.AppendLine("water_shield              1");
            sb.AppendLine("mixology                        " +
                (character.PrimaryProfession == Profession.Alchemy ||
                character.SecondaryProfession == Profession.Alchemy ? "1" : "0"));
            //sb.AppendLine("cast_sr_only_if_mana_left " + _calcOpts.MinManaSR);
            sb.AppendLine();
            sb.AppendLine("necrotic_touch                  " + (character.MainHand.Id == 50035 ? "1" : "0"));
            sb.AppendLine("necrotic_touch_heroic           " + (character.MainHand.Id == 50692 ? "1" : "0"));
            sb.AppendLine();
            sb.AppendLine("#############");
            sb.AppendLine("#CONSUMABLES#");
            sb.AppendLine("#############");
            sb.AppendLine();
            sb.AppendLine("flask_elixir                    " + addFlask(buffs));
            sb.AppendLine("guardian_elixir                 " + addGuardianElixir(buffs));
            sb.AppendLine("potion                          " + addPotion(buffs));
            sb.AppendLine("food                            " + addFood(buffs));
        }

        private string addFlask(List<Buff> buffs)
        {
            if (_character.ActiveBuffsContains("Flask of Endless Rage"))
                return "flask_of_endless_rage";
            if (_character.ActiveBuffsContains("Flask of the Frost Wyrm"))
                return "flask_of_the_frost_wyrm";
            if (_character.ActiveBuffsContains("Elixir of Demonslaying"))
                return "elixir_of_demonslaying";
            if (_character.ActiveBuffsContains("Elixir of Major Agility"))
                return "elixir_of_major_agility";
            if (_character.ActiveBuffsContains("Elixir of Mighty Agility"))
                return "elixir_of_mighty_agility";
            if (_character.ActiveBuffsContains("Elixir of Mighty Strength"))
                return "elixir_of_mighty_strength";
            if (_character.ActiveBuffsContains("Elixir of Accuracy"))
                return "elixir_of_accuracy";
            if (_character.ActiveBuffsContains("Elixir of Armor Piercing"))
                return "elixir_of_armor_piercing";
            if (_character.ActiveBuffsContains("Elixir of Deadly Strikes"))
                return "elixir_of_deadly_strikes";
            if (_character.ActiveBuffsContains("Elixir of Expertise"))
                return "elixir_of_expertise";
            if (_character.ActiveBuffsContains("Elixir of Lightning Speed"))
                return "elixir_of_lightning_speed";
            if (_character.ActiveBuffsContains("Guru's Elixir"))
                return "gurus_elixir";
            if (_character.ActiveBuffsContains("Spellpower Elixir"))
                return "spellpower_elixir";
            if (_character.ActiveBuffsContains("Wrath Elixir"))
                return "wrath_elixir";
            return "-";
        }

        private string addGuardianElixir(List<Buff> buffs)
        {
            if (_character.ActiveBuffsContains("Elixir of Draenic Wisdom"))
                return "elixir_of_draenic_wisdom";
            if (_character.ActiveBuffsContains("Elixir of Mighty Thoughts"))
                return "elixir_of_mighty_thoughts";
            return "-";
        }

        private string addPotion(List<Buff> buffs)
        {
            if (_character.ActiveBuffsContains("Potion of Speed"))
                return "potion_of_speed";
            if (_character.ActiveBuffsContains("Potion of Wild Magic"))
                return "potion_of_wild_magic";
            if (_character.ActiveBuffsContains("Heroic Potion"))
                return "heroic_potion";
            if (_character.ActiveBuffsContains("Insane Strength Potion"))
                return "insane_strength_potion";
            return "-";
        }

        private string addFood(List<Buff> buffs)
        {
            if (_character.ActiveBuffsContains("Agility Food"))
                return "blackened_dragonfin";
            if (_character.ActiveBuffsContains("Armor Pen Food"))
                return "hearty_rhino";
            if (_character.ActiveBuffsContains("Expertise Food"))
                return "rhinolicious_wormsteak";
            if (_character.ActiveBuffsContains("Hit Food"))
                return "snapper_extreme";
            if (_character.ActiveBuffsContains("Spell Power Food"))
                return "firecracker_salmon";
            if (_character.ActiveBuffsContains("Haste Food"))
                return "imperial_manta_steak";
            if (_character.ActiveBuffsContains("Attack Power Food"))
                return "poached_northern_sculpin";
            if (_character.ActiveBuffsContains("Crit Food"))
                return "spiced_wyrm_burger";
            if (_character.ActiveBuffsContains("Fish Feast"))
                return "fish_feast";
            return "-";
        }

        private void addGlyphs(Character character, System.Text.StringBuilder sb)
        {
            int glyphPrimeNumber = 0;
            int glyphMajorNumber = 0;
            if (character.ShamanTalents.GlyphofFeralSpirit)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    feral_spirit");
            }
            if (character.ShamanTalents.GlyphofFlametongueWeapon)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    flametongue_weapon");
            }
            if (character.ShamanTalents.GlyphofLightningBolt)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    lightning_bolt");
            }
            if (character.ShamanTalents.GlyphofLavaLash)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    lava_lash");
            }
            if (character.ShamanTalents.GlyphofShocking)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    shocking");
            }
            if (character.ShamanTalents.GlyphofFlameShock)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    flame_shock");
            }
            if (character.ShamanTalents.GlyphofStormstrike)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    stormstrike");
            }
            if (character.ShamanTalents.GlyphofWindfuryWeapon)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    windfury_weapon");
            }
            if (character.ShamanTalents.GlyphofFireElementalTotem)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    fire_elemental_totem");
            }
            while (glyphPrimeNumber < 3)
            {
                glyphPrimeNumber += 1;
                sb.AppendLine("glyph_prime" + glyphPrimeNumber + "                    -");
            }
            sb.AppendLine();
            if (character.ShamanTalents.GlyphofChainLightning)
            {
                glyphMajorNumber += 1;
                sb.AppendLine("glpyh_major" + glyphMajorNumber + "                    chain_lightning");
            }
            while (glyphMajorNumber < 3)
            {
                glyphMajorNumber += 1;
                sb.AppendLine("glyph_major" + glyphMajorNumber + "                    -");
            }
        }

        /*private void addPriorities(System.Text.StringBuilder sb)
        {
            sb.AppendLine();
            sb.AppendLine("rotation_priority_count         " + _calcOpts.ActivePriorities());
            int priorityNumber = 0;
            List<KeyValuePair<EnhanceAbility, Priority>> sortedPriorites = new List<KeyValuePair<EnhanceAbility, Priority>>(_calcOpts.PriorityList);
            sortedPriorites.Sort(
                delegate(KeyValuePair<EnhanceAbility, Priority> firstPair, KeyValuePair<EnhanceAbility, Priority> nextPair)
                {
                    return firstPair.Value.CompareTo(nextPair.Value);
                }
            );
            foreach (var element in sortedPriorites)
            {
                Priority p = element.Value;
                if (p.Checked & p.PriorityValue > 0 & !p.EnhSimName.Equals(""))
                    sb.AppendLine("rotation_priority" + (++priorityNumber) + "              " + p.EnhSimName);
            }
            sb.AppendLine();
        }*/

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
            for (int i = 0; i < items.Length; i++)
            {
                if ((object)items[i] != null && !string.IsNullOrEmpty(items[i].Item.SetName))
                {
                    int count;
                    setCounts.TryGetValue(items[i].Item.SetName, out count);
                    setCounts[items[i].Item.SetName] = count + 1;
                }
            }
            int bonusCount = 0;
            /*if (setCounts.Count > 0)
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
                        case "Frost Witch's Battlegear":
                            if (kvp.Value >= 4)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      t10_battlegear_4");
                            }
                            else if (kvp.Value >= 2)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      t10_battlegear_2");
                            }
                            break;
                    }
                }
            }*/
            while (bonusCount < 3)
            {
                bonusCount++;
                sb.AppendLine("set_bonus" + bonusCount + "                      -");
            }
            return sb.ToString();
        }
    }
}
