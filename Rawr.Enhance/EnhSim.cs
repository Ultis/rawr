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
        BossOptions _bossOpts;

        public EnhSim(Character character, CalculationOptionsEnhance calcOpts, BossOptions bossOpts)
        {
            _character = character;
            _calcOpts = calcOpts;
            _bossOpts = bossOpts;
            CalculationsEnhance ce = new CalculationsEnhance();
            CharacterCalculationsEnhance calcs = ce.GetCharacterCalculations(character, null) as CharacterCalculationsEnhance;
            Stats stats = calcs.EnhSimStats;
            CombatStats cs = new CombatStats(character, stats, _calcOpts, bossOpts);

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
            sb.AppendLine("str                             " + stats.Strength.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("agi                             " + stats.Agility.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("int                             " + stats.Intellect.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spi                             " + stats.Spirit.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("mastery_rating                  " + stats.MasteryRating.ToString("F0", CultureInfo.InvariantCulture));
            float chanceCrit = cs.ExportMeleeCritMH * 100f;
            sb.AppendLine("mh_crit                         " + chanceCrit.ToString("F2", CultureInfo.InvariantCulture));
            chanceCrit = cs.ExportMeleeCritOH * 100f;
            sb.AppendLine("oh_crit                         " + chanceCrit.ToString("F2", CultureInfo.InvariantCulture));
            float hitBonus = StatConversion.GetHitFromRating(stats.HitRating) * 100f + 6f;
            if (character.Race == CharacterRace.Draenei)
            {
                hitBonus += 1f;
            }
            sb.AppendLine("mh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("mh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            float unleashedRage = 0f;
            switch (character.ShamanTalents.UnleashedRage)
            {
                case 1: unleashedRage = .05f; break;
                case 2: unleashedRage = .10f; break;
            }
            sb.AppendLine("ap                              " + (stats.AttackPower * (1f + unleashedRage)).ToString("F0", CultureInfo.InvariantCulture));
            float hasteBonus = StatConversion.GetHasteFromRating(stats.HasteRating, CharacterClass.Shaman) * 100f;
            sb.AppendLine("melee_haste                     " + hasteBonus.ToString("F2", CultureInfo.InvariantCulture));
            float MQUnleashedRageSpellpower = stats.AttackPower * unleashedRage * 0.50f;
            sb.AppendLine("spellpower                      " + (stats.SpellPower + MQUnleashedRageSpellpower).ToString("F0", CultureInfo.InvariantCulture));
            float chanceSpellCrit = cs.DisplaySpellCrit * 100f;
            sb.AppendLine("spell_crit                      " + chanceSpellCrit.ToString("F2", CultureInfo.InvariantCulture));
            float elemPrec = character.ShamanTalents.ElementalPrecision > 0 ? (stats.Spirit - BaseStats.GetBaseStats(character).Spirit) * (character.ShamanTalents.ElementalPrecision / 3f) : 0f;
            hitBonus = StatConversion.GetSpellHitFromRating(stats.HitRating + elemPrec) * 100f;
            if (character.Race == CharacterRace.Draenei)
            {
                hitBonus += 1f;
            }
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
                if (character.MainHand.Type == ItemType.OneHandMace || character.MainHand.Type == ItemType.TwoHandMace)
                    weaponType = "mace";
            }
            sb.AppendLine("mh_weapon                       " + weaponType);
            weaponType = "-";
            if (character.OffHand != null)
            {
                if (character.OffHand.Type == ItemType.OneHandAxe)
                    weaponType = "axe";
                if (character.OffHand.Type == ItemType.FistWeapon)
                    weaponType = "fist";
                if (character.OffHand.Type == ItemType.OneHandMace)
                    weaponType = "mace";
            }
            sb.AppendLine("oh_weapon                       " + weaponType);
            sb.AppendLine();
            sb.AppendLine("trinket1                        " + _trinket1name);
            sb.AppendLine("trinket2                        " + _trinket2name);
            sb.AppendLine();
            sb.AppendLine(getSetBonuses(character));
            sb.AppendLine("metagem                         " + _metagem);
            sb.AppendLine();
            string glovesEnchant = character.HandsTinkering == null ? string.Empty : character.HandsTinkering.Name;
            if (glovesEnchant == "Synapse Springs")
                sb.AppendLine("gloves_enchant                  synapse_springs");
            else if (glovesEnchant == "Tazik Shocker")
                sb.AppendLine("gloves_enchant                  tazik_shocker");
            else
                sb.AppendLine("gloves_enchant                  -");
            string cloakEnchant = character.BackEnchant == null ? string.Empty : character.BackEnchant.Name;
            if (cloakEnchant == "Lightweave Embroidery")
                sb.AppendLine("cloak_enchant                   lightweave_embroidery");
            else if (cloakEnchant == "Swordguard Embroidery")
                sb.AppendLine("cloak_enchant                   swordguard_embroidery");
            else
                sb.AppendLine("cloak_enchant                   -");
            string weaponSetProc = "-";
            if (character.MainHand != null && character.OffHand != null)
            {
                if (character.MainHand.Id == 63537 && character.OffHand.Id == 63538)
                    weaponSetProc = "agony_and_torment";
            }
            sb.AppendLine("weapon_set_proc                   " + weaponSetProc);

            /*addGlyphs(character, sb);
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
            //elemental talents in EnhSim have not been updated, so keep exporting old talent choices where appropriate.
            sb.AppendLine("convection                      " + character.ShamanTalents.Convection + "/2");
            sb.AppendLine("concussion                      " + character.ShamanTalents.Concussion + "/3");
            sb.AppendLine("call_of_flame                   " + character.ShamanTalents.CallOfFlame + "/2");
            sb.AppendLine("reverberation                   " + character.ShamanTalents.Reverberation + "/2");
            sb.AppendLine("elemental_precision             " + character.ShamanTalents.ElementalPrecision + "/3");
            sb.AppendLine("elemental_focus                 " + character.ShamanTalents.ElementalFocus + "/1");
            sb.AppendLine("elemental_oath                  " + character.ShamanTalents.ElementalOath + "/2");
            sb.AppendLine("lava_flows                      " + character.ShamanTalents.LavaFlows + "/3");
            sb.AppendLine("storm_earth_and_fire            " + "0" + "/3");
            sb.AppendLine("elemental_mastery               " + character.ShamanTalents.ElementalMastery + "/1");
            sb.AppendLine("feedback                        " + character.ShamanTalents.Feedback + "/3");
            sb.AppendLine("lava_surge                      " + character.ShamanTalents.LavaSurge + "/2");
            sb.AppendLine();

            addBuffs(character, sb);
            // add extras
            sb.AppendLine();
            sb.AppendLine("combat_length                   " + (_bossOpts.BerserkTimer/60f).ToString("F2", CultureInfo.InvariantCulture));*/
            _configText = sb.ToString();
        }

#if SILVERLIGHT
        public void copyToClipboard()
        {
            try
            {
                EnhSimExportDialog EnhSimExport = new EnhSimExportDialog();
                EnhSimExport.Show();
                EnhSimExport.SetText(_configText);
            }
            catch { }
        }

#else
        public void copyToClipboard()
        {
            try
            {
                System.Windows.Clipboard.SetText(_configText);
            }
            catch { }
            if (_calcOpts.ShowExportMessageBox)
                System.Windows.MessageBox.Show("EnhSim config data copied to clipboard.\n" +
                    "Use the 'Copy from Clipboard' option in EnhSimGUI v1.9.6.0 or higher, to import it\n" +
                    "Or paste the config data into your EnhSim config file in a decent text editor (not Notepad)!",
                    "Enhance Module");
        }
#endif

        #region Buffs & Debuffs
        private void addBuffs(Character character, StringBuilder sb)
        {
            sb.AppendLine("#########");
            sb.AppendLine("# Buffs #");
            sb.AppendLine("#########");
            sb.AppendLine();
            List<Buff> buffs = character.ActiveBuffs;

            if (_character.ActiveBuffsContains("Corrosive Spit") || _character.ActiveBuffsContains("Expose Armor") ||
                _character.ActiveBuffsContains("Sunder Armor") || _character.ActiveBuffsContains("Faerie Fire") ||
                _character.ActiveBuffsContains("Tear Armor"))
                sb.AppendLine("armor_debuff                    12.0/12.0");
            else
                sb.AppendLine("armor_debuff                    0.0/12.0");
            if (_character.ActiveBuffsContains("Blood Frenzy") || _character.ActiveBuffsContains("Savage Combat") ||
                _character.ActiveBuffsContains("Brittle Bones") || _character.ActiveBuffsContains("Ravage") ||
                _character.ActiveBuffsContains("Acid Spit"))
                sb.AppendLine("physical_vulnerability_debuff   4.0/4.0");
            else
                sb.AppendLine("physical_vulnerability_debuff   0.0/4.0");
            if (_character.ActiveBuffsContains("Windfury Totem") || _character.ActiveBuffsContains("Improved Icy Talons") ||
                _character.ActiveBuffsContains("Hunting Party"))
                sb.AppendLine("melee_haste_buff                10.0/10.0");
            else
                sb.AppendLine("melee_haste_buff                0.0/10.0");
            if (_character.ActiveBuffsContains("Leader of the Pack") || _character.ActiveBuffsContains("Rampage") ||
                _character.ActiveBuffsContains("Honor Among Thieves") || _character.ActiveBuffsContains("Elemental Oath") ||
                _character.ActiveBuffsContains("Furious Howl") || _character.ActiveBuffsContains("Terrifying Roar"))
                sb.AppendLine("crit_chance_buff                5.0/5.0");
            else
                sb.AppendLine("crit_chance_buff                0.0/5.0");
            if (_character.ActiveBuffsContains("Trueshot Aura") || _character.ActiveBuffsContains("Unleashed Rage") ||
                _character.ActiveBuffsContains("Abomination's Might") || _character.ActiveBuffsContains("Blessing of Might (AP%)"))
                sb.AppendLine("attack_power_buff_multiplier    10.0/10.0");
            else
                sb.AppendLine("attack_power_buff_multiplier    0.0/10.0");
            if (_character.ActiveBuffsContains("Wrath of Air Totem") || _character.ActiveBuffsContains("Moonkin Form") ||
                _character.ActiveBuffsContains("Mind Quickening"))
                sb.AppendLine("spell_haste_buff                5.0/5.0");
            else
                sb.AppendLine("spell_haste_buff                0.0/5.0");
            if (_character.ActiveBuffsContains("Critical Mass") || _character.ActiveBuffsContains("Improved Shadow Bolt"))
                sb.AppendLine("spell_crit_chance_debuff        5.0/5.0");
            else
                sb.AppendLine("spell_crit_chance_debuff        0.0/5.0");
            if (_character.ActiveBuffsContains("Ebon Plaguebringer") || _character.ActiveBuffsContains("Earth and Moon") ||
                _character.ActiveBuffsContains("Curse of the Elements") || _character.ActiveBuffsContains("Master Poisner") ||
                _character.ActiveBuffsContains("Fire Breath") || _character.ActiveBuffsContains("Lightning Breath"))
                sb.AppendLine("spell_damage_debuff             8.0/8.0");
            else
                sb.AppendLine("spell_damage_debuff             0.0/8.0");
            if (_character.ActiveBuffsContains("Flametongue Totem") || _character.ActiveBuffsContains("Arcane Brilliance (SP%)"))
                sb.AppendLine("spellpower_buff                 6.0/10.0");
            else if (_character.ActiveBuffsContains("Totem of Wrath (Spell Power)") || _character.ActiveBuffsContains("Demonic Pact"))
                sb.AppendLine("spellpower_buff                 10.0/10.0");
            else
                sb.AppendLine("spellpower_buff                 0/10.0");
            if (_character.ActiveBuffsContains("Ferocious Inspiration") || _character.ActiveBuffsContains("Sanctified Retribution") ||
                _character.ActiveBuffsContains("Arcane Tactics"))
                sb.AppendLine("percentage_damage_increase      3.0/3.0");
            else
                sb.AppendLine("percentage_damage_increase      0.0/3.0");
            if (_character.ActiveBuffsContains("Blessing of Kings") || _character.ActiveBuffsContains("Mark of the Wild") ||
                _character.ActiveBuffsContains("Embrace of the Shale Spider"))
                sb.AppendLine("stat_multiplier                 5.0/5.0");
            else
                sb.AppendLine("stat_multiplier                 0.0/5.0");
            if (_character.ActiveBuffsContains("Strength of Earth Totem") || _character.ActiveBuffsContains("Horn of Winter") ||
                _character.ActiveBuffsContains("Battle Shout") || _character.ActiveBuffsContains("Roar of Courage"))
                sb.AppendLine("agi_and_strength_buff           549/549");
            else
                sb.AppendLine("agi_and_strength_buff           0/549");
            if (_character.ActiveBuffsContains("Fel Intelligence (Mana)") || _character.ActiveBuffsContains("Arcane Brilliance (Mana)"))
                sb.AppendLine("mana_buff                       2126/2126");
            else
                sb.AppendLine("mana_buff                       0/2126");
            if (_character.ActiveBuffsContains("Blessing of Might (Mp5)") || _character.ActiveBuffsContains("Mana Spring Totem") ||
                _character.ActiveBuffsContains("Fel Intelligence (Mp5)"))
                sb.AppendLine("mana_regen_buff                 326/326");
            else
                sb.AppendLine("mana_regen_buff                 0/326");
            sb.AppendLine();
            if (_character.ActiveBuffsContains("Revitalize") || _character.ActiveBuffsContains("Communion") ||
                _character.ActiveBuffsContains("Vampiric Touch") || _character.ActiveBuffsContains("Soul Leech") ||
                _character.ActiveBuffsContains("Enduring Winter"))
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
            sb.AppendLine("cast_sr_only_if_mana_left " + _calcOpts.MinManaSR);
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
            if (_character.ActiveBuffsContains("Flask of the Winds"))
                return "flask_of_the_winds";
            if (_character.ActiveBuffsContains("Flask of the Draconic Mind"))
                return "flask_of_the_draconic_mind";
            if (_character.ActiveBuffsContains("Elixer of the Master"))
                return "elixir_of_the_master";
            if (_character.ActiveBuffsContains("Elixir of Impossible Accuracy"))
                return "elixir_of_impossible_accuracy";
            if (_character.ActiveBuffsContains("Elixir of the Cobra"))
                return "elixir_of_the_cobra";
            if (_character.ActiveBuffsContains("Elixir of the Naga"))
                return "elixir_of_the_naga";
            if (_character.ActiveBuffsContains("Elixir of Mighty Speed"))
                return "elixir_of_mighty_speed"; ;
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
            if (_character.ActiveBuffsContains("Potion of the Tol'vir"))
                return "potion_of_the_tolvir";
            if (_character.ActiveBuffsContains("Golemblood Potion"))
                return "golemblood_potion";
            if (_character.ActiveBuffsContains("Volcanic Potion"))
                return "volcanic_potion";
            if (_character.ActiveBuffsContains("Deathblood Venom"))
                return "deathblodd_venom";
            return "-";
        }

        private string addFood(List<Buff> buffs)
        {
            if (_character.ActiveBuffsContains("Agility Food"))
                return "skewered_eel";
            if (_character.ActiveBuffsContains("Mastery Food"))
                return "lavascale_minestrone";
            if (_character.ActiveBuffsContains("Expertise Food"))
                return "crocolisk_au_gratin";
            if (_character.ActiveBuffsContains("Hit Food"))
                return "grilled_dragon";
            if (_character.ActiveBuffsContains("Intellect Food"))
                return "severed_sagefish_head";
            if (_character.ActiveBuffsContains("Haste Food"))
                return "basilisk_liverdog";
            if (_character.ActiveBuffsContains("Crit Food"))
                return "baked_rockfish";
            return "-";
        }
        #endregion

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
            if (setCounts.Count > 0)
            {

                foreach (KeyValuePair<string, int> kvp in setCounts)
                {
                    switch (kvp.Key)
                    {
                        case "Battlegear of the Raging Elements":
                            if (kvp.Value >= 4)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      t11_battlegear_4");
                            }
                            else if (kvp.Value >= 2)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      t11_battlegear_2");
                            }
                            break;
                        case "Volcanic Battlegear":
                            if (kvp.Value >= 4)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      t12_battlegear_4");
                            }
                            else if (kvp.Value >= 2)
                            {
                                bonusCount++;
                                sb.AppendLine("set_bonus" + bonusCount + "                      t12_battlegear_2");
                            }
                            break;
                    }
                }
            }
            while (bonusCount < 3)
            {
                bonusCount++;
                sb.AppendLine("set_bonus" + bonusCount + "                      -");
            }
            return sb.ToString();
        }
    }
}
