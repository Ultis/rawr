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

        public EnhSim(Character character)
        {
            CalculationsEnhance ce = new CalculationsEnhance();
            CharacterCalculationsEnhance calcs = ce.GetCharacterCalculations(character, null) as CharacterCalculationsEnhance;
            Stats stats = calcs.BasicStats;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("##########################################");
            sb.AppendLine("### Rawr.Enhance Data Export to EnhSim ###");
            sb.AppendLine("##########################################");
            sb.AppendLine(" ");
            sb.AppendLine("race                            " + character.Race.ToString().ToLower());
            sb.AppendLine("mh_speed                        " + character.MainHand.Speed.ToString());
            sb.AppendLine("oh_speed                        " + character.OffHand.Speed.ToString());
            sb.AppendLine(" ");
            sb.AppendLine("mh_dps                          0.0 # not yet implemented in Rawr Export"); // dps 1dp
            sb.AppendLine("oh_dps                          0.0 # not yet implemented in Rawr Export"); // dps 1dp
            sb.AppendLine("mh_crit                         " + calcs.MeleeCrit.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_crit                         " + calcs.MeleeCrit.ToString("F2", CultureInfo.InvariantCulture));
            float hitbonus = stats.HitRating / 32.78998947f;
            sb.AppendLine("mh_hit                          " + hitbonus.ToString("F2", CultureInfo.InvariantCulture));
            sb.AppendLine("oh_hit                          " + hitbonus.ToString("F2", CultureInfo.InvariantCulture)); // %
            sb.AppendLine("mh_expertise_rating             " + stats.ExpertiseRating.ToString());
            sb.AppendLine("oh_expertise_rating             " + stats.ExpertiseRating.ToString());
            sb.AppendLine("ap                              " + stats.AttackPower.ToString());
            sb.AppendLine("haste                           0 # not yet implemented in Rawr Export"); // %
            sb.AppendLine("armor_penetration               0 # not yet implemented in Rawr Export"); // %
            sb.AppendLine("str                             " + stats.Strength.ToString());
            sb.AppendLine("agi                             " + stats.Agility.ToString());
            sb.AppendLine("int                             " + stats.Intellect.ToString());
            sb.AppendLine("spi                             " + stats.Spirit.ToString());
            sb.AppendLine("spellpower                      " + stats.SpellPower.ToString());
            sb.AppendLine("spell_crit                      0.0 # not yet implemented in Rawr Export"); // %
            sb.AppendLine("spell_hit                       0.0 # not yet implemented in Rawr Export"); // %
            sb.AppendLine("max_mana                        " + stats.Mana.ToString());
            sb.AppendLine("mp5                             " + stats.Mp5.ToString());
            sb.AppendLine(" ");
            sb.AppendLine("mh_imbue                        - # not yet implemented in Rawr Export"); // windfury
            sb.AppendLine("oh_imbue                        - # not yet implemented in Rawr Export"); // flametongue
            sb.AppendLine(" ");
            sb.AppendLine("mh_enchant                      - # not yet implemented in Rawr Export"); // mongoose
            sb.AppendLine("oh_enchant                      - # not yet implemented in Rawr Export"); // -
            sb.AppendLine(" ");
            sb.AppendLine("mh_weapon                       - # not yet implemented in Rawr Export"); // -
            sb.AppendLine("oh_weapon                       - # not yet implemented in Rawr Export"); // -
            sb.AppendLine(" ");
            sb.AppendLine("trinket1                        - # not yet implemented in Rawr Export"); // mirror_of_truth
            sb.AppendLine("trinket2                        - # not yet implemented in Rawr Export"); // shard_of_contempt
            sb.AppendLine(" ");
            sb.AppendLine("totem                           " + getTotemName(character.Ranged)); 
            sb.AppendLine(" ");
            sb.AppendLine("set_bonus                       - # not yet implemented in Rawr Export"); 
            sb.AppendLine(" ");
            sb.AppendLine("metagem                         - # not yet implemented in Rawr Export" ); 
            sb.AppendLine(" ");
            sb.AppendLine("glyph_major1                    - # not yet implemented in Rawr Export"); 
            sb.AppendLine("glyph_major2                    - # not yet implemented in Rawr Export"); 
            sb.AppendLine("glyph_major3                    - # not yet implemented in Rawr Export");
            sb.AppendLine(" ");
            sb.AppendLine("glyph_minor1                    -"); 
            sb.AppendLine("glyph_minor2                    -"); 
            sb.AppendLine("glyph_minor3                    -");
            sb.AppendLine(" ");
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
            sb.AppendLine(" ");
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

        private String getTotemName(ItemInstance totem)
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
    }
}
