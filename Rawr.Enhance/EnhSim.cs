using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Rawr.Enhance
{
    public class EnhSim
    {
        private String _configText = null;
        private String _metagem = null;
        private String _mhEnchant = null;
        private String _ohEnchant = null;
        private String _trinket1name = null;
        private String _trinket2name = null;
        private String _totemname = null;

        public EnhSim(Character character)
        {
            CalculationsEnhance ce = new CalculationsEnhance();
            CalculationOptionsEnhance calcOpts = character.CalculationOptions as CalculationOptionsEnhance;
            CharacterCalculationsEnhance calcs = ce.GetCharacterCalculations(character, null) as CharacterCalculationsEnhance;
            Stats stats = calcs.BasicStats;
            removeUseProcEffects(character, stats);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("##########################################");
            sb.AppendLine("### Rawr.Enhance Data Export to EnhSim ###");
            sb.AppendLine("##########################################");
            sb.AppendLine();
            sb.AppendLine("race                            " + character.Race.ToString().ToLower());
            sb.AppendLine("mh_speed                        " + character.MainHand.Speed.ToString());
            sb.AppendLine("oh_speed                        " + character.OffHand.Speed.ToString());
            sb.AppendLine("mh_dps                          " + character.MainHand.Item.DPS.ToString("F1", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_dps                          " + character.OffHand.Item.DPS.ToString("F1", CultureInfo.InvariantCulture));
            sb.AppendLine("mh_crit                         " + calcs.MeleeCrit.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_crit                         " + calcs.MeleeCrit.ToString("F2", CultureInfo.InvariantCulture));
            float hitBonus = stats.HitRating / 32.78998947f;
            sb.AppendLine("mh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_hit                          " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("mh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_expertise_rating             " + stats.ExpertiseRating.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("ap                              " + stats.AttackPower.ToString("F0", CultureInfo.InvariantCulture));
            float hasteBonus = stats.HasteRating / 32.78998947f;
            sb.AppendLine("haste                           " + hasteBonus.ToString("F2", CultureInfo.InvariantCulture));
            float armourPenBonus = stats.ArmorPenetrationRating / 15.39529991f;
            sb.AppendLine("armor_penetration               " + armourPenBonus.ToString("F2", CultureInfo.InvariantCulture)); 
            sb.AppendLine("str                             " + stats.Strength.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("agi                             " + stats.Agility.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("int                             " + stats.Intellect.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spi                             " + stats.Spirit.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spellpower                      " + stats.SpellPower.ToString("F0", CultureInfo.InvariantCulture));
            sb.AppendLine("spell_crit                      " + calcs.SpellCrit.ToString("F2", CultureInfo.InvariantCulture));
            hitBonus = stats.HitRating / 26.23199272f;
            sb.AppendLine("spell_hit                       " + hitBonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("max_mana                        " + stats.Mana.ToString());
            sb.AppendLine("mp5                             " + stats.Mp5.ToString());
            sb.AppendLine();
            sb.AppendLine("mh_imbue                        " + calcOpts.MainhandImbue.ToString().ToLower());
            sb.AppendLine("oh_imbue                        " + calcOpts.OffhandImbue.ToString().ToLower());
            sb.AppendLine();
            sb.AppendLine("mh_enchant                      " + _mhEnchant);
            String weaponType = "-";
            if (character.MainHand.Type == Item.ItemType.OneHandAxe)
                weaponType = "axe";
            else if (character.MainHand.Type == Item.ItemType.TwoHandAxe)
                weaponType = "axe";
            sb.AppendLine("mh_weapon                       " + weaponType);
            sb.AppendLine("oh_enchant                      " + _ohEnchant);
            if (character.OffHand == null)
                weaponType = "-";
            else
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
            sb.AppendLine("set_bonus                       - # not yet implemented in Rawr Export"); 
            sb.AppendLine("metagem                         - # not yet implemented in Rawr Export" ); 
            sb.AppendLine();

            addGlyphs(calcOpts, sb);
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
            sb.AppendLine("unleashed_rage                  " + character.ShamanTalents.UnleashedRage + "/5");
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

            _configText = sb.ToString();
        }

        public void copyToClipboard()
        {
            Clipboard.SetText(_configText);
            System.Windows.Forms.MessageBox.Show("EnhSim config data copied to clipboard\nPaste the config data into your config file in a decent text editor!",
                "Enhance Module", System.Windows.Forms.MessageBoxButtons.OK);         
            
        }

        private void addGlyphs(CalculationOptionsEnhance calcOpts, System.Text.StringBuilder sb)
        {
            int glyphNumber = 0;
            if (calcOpts.GlyphFT)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    flametongue");
            }
            if (calcOpts.GlyphLB)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    lightning_bolt");
            }
            if (calcOpts.GlyphLL)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    lava_lash");
            }
            if (calcOpts.GlyphLS)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    lightning_shield");
            }
            if (calcOpts.GlyphShocking)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    earth_shock");
            }
            if (calcOpts.GlyphSS)
            {
                glyphNumber += 1;
                sb.AppendLine("glyph_major" + glyphNumber + "                    stormstrike");
            }
            if (calcOpts.GlyphWF)
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
            _trinket1name = adjustTrinketStats(character.Trinket1, stats);
            _trinket2name = adjustTrinketStats(character.Trinket2, stats);
            _totemname = adjustTotemStats(character.Ranged, stats);
            _mhEnchant = adjustWeaponEnchantStats(character.MainHandEnchant, stats);
            _ohEnchant = adjustWeaponEnchantStats(character.OffHandEnchant, stats);
            _metagem = adjustMetaGemStats(character.Head, stats);
            stats.HasteRating -= stats.HasteRatingOnPhysicalAttack * 10 / 45; // deals with Meteorite Whetstone/Dragonspine Trophy

            // having removed all the stuff added by on use/procs we need to take the ceiling values as 100+2.66 would have been floored to 102
            // if we take 2.66 from 102 we get 101.33 which is too low.
            stats.AttackPower = (float) Math.Ceiling(stats.AttackPower);
            stats.SpellPower = (float) Math.Ceiling(stats.SpellPower);
            stats.HasteRating = (float)Math.Ceiling(stats.HasteRating);
            stats.CritRating = (float)Math.Ceiling(stats.CritRating);
            stats.ArmorPenetrationRating = (float)Math.Ceiling(stats.ArmorPenetrationRating);
        }

        private String adjustMetaGemStats(ItemInstance gem, Stats stats)
        {
            return "-";
        }

        private String adjustWeaponEnchantStats(Enchant enchant, Stats stats)
        {
            if (enchant == null)
                return "-";
            // check weapon enchant return enchant name for EnhSim
            switch (enchant.Id) {
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

        private String adjustTotemStats(ItemInstance totem, Stats stats)
        {
            if (totem == null)
                return "-";
            switch (totem.Id) {
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
                    return "totem_of_hex";
                case 40322:
                    return "totem_of_dueling";
                case 40708:
                    return "totem_of_the_elemental_plane";
                case 42607:
                    return "deadly_gladiators_totem_of_indomitability";
                case 42606:
                    return "hateful_gladiators_totem_of_indomitability";
                case 42593:
                    return "savage_gladiators_totem_of_indomitability";
                case 42602:
                    return "deadly_gladiators_totem_of_survival";
                case 42601:
                    return "hateful_gladiators_totem_of_survival";
                case 42594:
                    return "savage_gladiators_totem_of_survival";
                default:
                    return "-";
            }
        }

        private String adjustTrinketStats(ItemInstance trinket, Stats stats)
        {
            if (trinket == null)
                return "-";
            switch (trinket.Id)
            {
	            case 28830:
                    // dealt with by HasteRatingOnPhysicalAttack
		            return "dragonspine_trophy";
	            case 32419:
                    // no effect added in SpecialEffects class
		            return "ashtongue_talisman";
	            case 32505:
                    stats.ArmorPenetrationRating -= 42f / 5f;
		            return "madness_of_the_betrayer";
	            case 28034:
                    stats.AttackPower -= 300f / 6f;
		            return "hourglass_of_the_unraveller";
	            case 30627:
                    stats.AttackPower -= 340f / 6f;
		            return "tsunami_talisman";
	            case 34472:
                    stats.AttackPower -= 90f;
		            return "shard_of_contempt";
	            case 33831:
                    stats.AttackPower -= 360f / 6f;
		            return "berserkers_call";
	            case 29383:
                    stats.AttackPower -= 278f / 6f;
		            return "bloodlust_brooch";
	            case 28288:
                    stats.HasteRating -= 260f / 12f;
		            return "abacus_of_violent_odds";
	            case 32658:
                    stats.CritRating -= ((150f / 6f) / 25f) * 45.90598679f;
                    stats.AttackPower -= (150f / 6f) * 1.03f;
		            return "badge_of_tenacity";
	            case 35702:
                    stats.AttackPower -= 320f / 6f;
		            return "shadowsong_panther";
	            case 34427:
                    // no effect added in SpecialEffects class
		            return "blackened_naaru_silver";
	            case 31856:
                    stats.AttackPower -= 120; 
                    stats.SpellPower -= 80;
		            return "darkmoon_card_crusade";
	            case 40431:
                    stats.AttackPower -= 320;
		            return "fury_of_the_five_flights";
	            case 40256:
                    stats.ArmorPenetrationRating -= 612f / 5f;
		            return "grim_toll";
	            case 39257:
                    stats.AttackPower -= 670f / 6f;
		            return "loathebs_shadow";
	            case 40684:
                    stats.AttackPower -= 1000f / 7f;
		            return "mirror_of_truth";
	            case 32483:
                    // stats.HasteRatingFor20SecOnUse2Min += 175;
		            return "the_skull_of_guldan";
	            case 37390:
                    // dealt with by HasteRatingOnPhysicalAttack
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
		            return "sphere_of_red_dragons_blood";
	            case 36972:
                    stats.HasteRating -= 256f / 6f;
		            return "tome_of_arcane_phenomena";
	            case 40531:
                    stats.HasteRating -= 491f / 6f;
		            return "mark_of_norgannon";
	            case 44014:
                    stats.AttackPower -= 54f;
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
    }
}
